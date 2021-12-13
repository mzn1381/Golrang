using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR._03_Bank
{
    public partial class Form06_ExportDocumentForFirstValue : Form
    {
        Int16 _ID=0;
        DataTable Table = new DataTable();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        bool _BackSpace = false;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
       
        int DocNum = 0,DocID=0;

        public Form06_ExportDocumentForFirstValue(Int16 ID)
        {
            InitializeComponent();
            _ID = ID;
        }

        private void Form06_ExportDocumentForFirstValue_Load(object sender, EventArgs e)
        {
           
            this.allHeadersTableAdapter.Fill(this.dataSet_01_Cash.AllHeaders);
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            
            
            SqlDataAdapter Adapter = new SqlDataAdapter("select Table_045_PersonScope.Column01 as ColumnId,Table_045_PersonInfo.Column01 as Column01,Table_045_PersonInfo.Column02 as Column02 from Table_045_PersonScope INNER Join Table_045_PersonInfo On Table_045_PersonInfo.ColumnId=Table_045_PersonScope.Column01 where Table_045_PersonScope.Column02=13 ", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            //mlt_BedPerson.DataSource = Person;
            mlt_BedPerson.DataSource = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column06,Column07,Column08," +
                "Column09,Column10,Column11,Column12,Column35 from Table_020_BankCashAccInfo where ColumnId=" + _ID, ConBank);
            Adapter.Fill(Table);
             txt_BankName.Text = Table.Rows[0]["Column02"].ToString();
             txt_FirstAmount.Text = Table.Rows[0]["Column06"].ToString();
             mlt_Bed.DataSource = ClDoc.ReturnTable(ConACNT, "select  * from AllHeaders() ");
             //mlt_Bed.Value = ClDoc.ReturnTable(ConACNT, "select  * from AllHeaders() where ACC_Code=" + Table.Rows[0]["Column12"].ToString() + "");

             //mlt_BedPerson.Value=ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              //'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"' AND Columnid="+Table.Rows[0]["Column35"].ToString()+")");


             mlt_Bed.Value = Table.Rows[0]["Column12"].ToString();
             mlt_Bes.Value = ClDoc.Account(1, "Column13");
             mlt_BedPerson.Value = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");



             faDatePicker1.SelectedDateTime = DateTime.Now;
             txt_Sharh.Text = "صدور سند موجودی اولیه بانک/صندوق " + txt_BankName.Text;
             txt_Cover.Text = "موجودی اولیه حسابهای بانکی/صندوق ";
             txt_BankName.Select();

        }

   

        private void mlt_Sharh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is FarsiLibrary.Win.Controls.FADatePicker)
            {
                if (Class_BasicOperation.isNotDigit(e.KeyChar))
                    e.Handled = true;
                if (e.KeyChar == 8)
                    _BackSpace = true;
                else
                    _BackSpace = false;
                if (e.KeyChar == 13)
                {
                    Class_BasicOperation.isEnter(e.KeyChar);
                    faDatePicker1.HideDropDown();
                }
            }
            else if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if(!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                txt_Cover.Text = "موجودی اولیه حسابهای بانکی/صندوق ";
                faDatePicker1.SelectedDateTime = DateTime.Now;
            }
            else
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
            }
        }

        private void rdb_last_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_last.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                int LastNum = ClDoc.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = ClDoc.DocDate( LastNum);
                txt_Cover.Text = ClDoc.Cover(LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_Cover.Text = null;
            }
        }

        private void txt_To_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_To.Text.Trim() != "")
                {
                    ClDoc.IsValidNumber( int.Parse(txt_To.Text.Trim()));
                    faDatePicker1.Text = ClDoc.DocDate( int.Parse(txt_To.Text.Trim()));
                    txt_Cover.Text = ClDoc.Cover( int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form06_ExportDocumentForFirstValue");
            }
        }

        private void rdb_TO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_TO.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdb_last.Checked && txt_LastNum.Text.Trim()!="")
                {
                    ClDoc.IsFinal( int.Parse(txt_LastNum.Text.Trim()));
                }
                else if (rdb_TO.Checked && txt_To.Text.Trim() != "")
                {
                    ClDoc.IsValidNumber( int.Parse(txt_To.Text.Trim()));
                    ClDoc.IsFinal( int.Parse(txt_To.Text.Trim()));
                    txt_To_Leave(sender, e);
                }



                if (mlt_Bed.Text.Trim() == "" || mlt_Bes.Text.Trim() == "" || txt_Sharh.Text.Trim() == "" || txt_Cover.Text.Trim() == "" || !faDatePicker1.SelectedDateTime.HasValue)
                    throw new Exception("اطلاعات مورد نیاز را کامل کنید");

                ClDoc.CheckForValidationDate( faDatePicker1.Text);

                //**********Check Bed and Bes for Person,Center and Project needs****//
                int? Person=null;
                if(mlt_BedPerson.Text.Trim()!="")
                    Person=int.Parse(mlt_BedPerson.Value.ToString());
                clCredit.All_Controls(mlt_Bed.Value.ToString(),Person, null, null);
                clCredit.All_Controls(mlt_Bes.Value.ToString(), null, null, null);

                //**********Check Account's nature****//
                DataTable TAccounts = new DataTable();
                TAccounts.Columns.Add("Account", Type.GetType("System.String"));
                TAccounts.Columns.Add("Price", Type.GetType("System.Double"));
                TAccounts.Rows.Add(mlt_Bed.Value.ToString(),Convert.ToDouble( txt_FirstAmount.Value.ToString()));
                TAccounts.Rows.Add(mlt_Bes.Value.ToString(),Convert.ToDouble( txt_FirstAmount.Value.ToString())*-1);
                clCredit.CheckAccountCredit(TAccounts, 0);

                //************* Check Person,Center,Project
              

                if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                   
                    if (rdb_New.Checked)
                    {
                        DocNum = ClDoc.LastDocNum() + 1;
                        DocID = ClDoc.ExportDoc_Header(DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName, 2);
                        DocNum = ClDoc.DocNum(DocID);
                    }
                    else if (rdb_last.Checked)
                    {
                        DocNum = ClDoc.LastDocNum();
                        DocID = ClDoc.DocID( DocNum);
                    }
                    else if (rdb_TO.Checked)
                    {
                        DocNum = int.Parse(txt_To.Text.Trim());
                        DocID = ClDoc.DocID( DocNum);

                    }
                    if (DocID > 0)
                    {
                        string[] _BedInfo = ClDoc.ACC_Info(mlt_Bed.Value.ToString());
                        ClDoc.ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(),
                            Int16.Parse(_BedInfo[0].ToString()),_BedInfo[1].ToString(), _BedInfo[2], _BedInfo[3],_BedInfo[4],
                            (mlt_BedPerson.Text.Trim()!="" ? mlt_BedPerson.Value.ToString():null) , null, null, txt_Sharh.Text, Convert.ToInt64(Convert.ToDouble( txt_FirstAmount.Value.ToString())), 0, 0, 0, -1, 14, _ID, Class_BasicOperation._UserName, 0);

                        string[] _BesInfo = ClDoc.ACC_Info(mlt_Bes.Value.ToString());
                        ClDoc.ExportDoc_Detail( DocID, mlt_Bes.Value.ToString(), Int16.Parse(_BesInfo[0]),
                           _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4], null, null, null, txt_Sharh.Text, 0, Convert.ToInt64(Convert.ToDouble( txt_FirstAmount.Value.ToString())), 0, 0, -1, 14, _ID, Class_BasicOperation._UserName, 0);

                        ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_020_BankCashAccInfo", "Column32", "ColumnID", _ID, DocID);
                        Class_BasicOperation.ShowMsg(" ", "ثبت موجودی اولیه با شماره " +DocNum.ToString()  + " انجام گرفت", Class_BasicOperation.MessageType.Information);
                        bt_Save.Enabled = false;
                        explorerBarContainerControl1.Enabled = false;
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;

                    }
                        

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,this.Name);
            }
        }

        private void bt_View_Click(object sender, EventArgs e)
        {
           Classes.Class_UserScope UserScope = new Classes. Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 22))
            {
                PACNT._2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument(DocID);
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void Form06_ExportDocumentForFirstValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.W)
                bt_View_Click(sender, e);
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    txt_Cover.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (mlt_Bed.Text.Trim() != "" && mlt_BedPerson.Text.Trim() != "")
            {
                DevComponents.DotNetBar.Balloon b = new DevComponents.DotNetBar.Balloon();
                b.Style = DevComponents.DotNetBar.eBallonStyle.Office2007Alert;
                b.CaptionText = "مانده حساب";
                b.CaptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b.CaptionImage = balloonTip1.CaptionImage.Clone() as Image;
                b.Text = ClDoc.PersonRemain(int.Parse(mlt_BedPerson.Value.ToString()), mlt_Bed.Value.ToString()).ToString("N0");
                b.AlertAnimation = DevComponents.DotNetBar.eAlertAnimation.TopToBottom;
                b.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b.AutoResize();
                b.Show(uiButton1, false);
            }
        }

        private void mlt_Bed_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "ACC_Name", "ACC_Code");
        }

        private void mlt_Bed_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void faDatePicker1_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 114))
            {
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی را ندارید", Class_BasicOperation.MessageType.None);
        }

   
    
    }
}
