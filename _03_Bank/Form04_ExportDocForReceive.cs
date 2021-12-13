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
    public partial class Form04_ExportDocForReceive : Form
    {
        int _PaperID=0;
        DataTable RecTable = new DataTable();
        bool _BackSpace = false;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
       
        int DocNum = 0, DocID = 0;
        string _BankName;
        string _Person;
        DataRow RecRow;
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);

        

        public Form04_ExportDocForReceive(int PaperID,string BoxName,string Person)
        {
            InitializeComponent();
            _PaperID = PaperID;
            _BankName = BoxName;
            _Person = Person;
        }

        private void Form04_ExportDocForReceive_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            SqlDataAdapter ViewAdapter = new SqlDataAdapter("Select ACC_Code,ACC_Name from AllHeaders()", ConACNT);
            DataTable Headers = new DataTable();
            ViewAdapter.Fill(Headers);
            mlt_Bes.DataSource = Headers;
            DataTable Header2 = new DataTable();
            ViewAdapter.Fill(Header2);
            mlt_Bed.DataSource = Header2;
            ViewAdapter.SelectCommand.Connection = ConBase;
            ViewAdapter.SelectCommand.CommandText = "Select ColumnId,Column01,Column02 from Table_045_PersonInfo";
            DataTable Person = new DataTable();
            ViewAdapter.Fill(Person);
            mlt_BesPerson.DataSource = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
            mlt_BedPerson.DataSource = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
            ViewAdapter.SelectCommand.CommandText = "Select Column00,Column01,Column02 from Table_035_ProjectInfo";
            DataTable Project = new DataTable();
            ViewAdapter.Fill(Project);
            mlt_BesProject.DataSource = Project;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_045_ReceiveCash where ColumnId=" + _PaperID, ConBank);
            Adapter.Fill(RecTable);
            txt_BankName.Text = _BankName;
            txt_FirstAmount.Value = Convert.ToDouble(RecTable.Rows[0]["Column03"].ToString());

            RecRow = RecTable.Rows[0];
            mlt_BesPerson.Value = RecRow["Column05"].ToString();
            mlt_BedPerson.Value = RecRow["Column21"].ToString();
            mlt_BesProject.Value = RecRow["Column06"].ToString();
            mlt_Bed.Value = ClDoc.ExScalar(ConBank.ConnectionString, "Table_020_BankCashAccInfo", "Column12", "ColumnId", RecRow["Column01"].ToString());

            if (RecRow["Column22"].ToString() == "False")
            {
                //انتخاب سرفصل اصلی صندوق یا بانک به عنوان بستانکار
                if (RecRow["Column07"].ToString() != "" && RecRow["Column11"].ToString() == "")
                {
                    mlt_Bes.Value = ClDoc.ExScalar(ConBank.ConnectionString, "Table_020_BankCashAccInfo", "Column12", "ColumnId", RecRow["Column07"].ToString());
                }
                //انتخاب سرفصل حساب مشخص شده در برگه پرداخت به عنوان بستانکار
                else if (RecRow["Column07"].ToString() == "" && RecRow["Column11"].ToString().Trim() != "")
                {
                    mlt_Bes.Value = RecRow["Column11"].ToString();
                }
                else
                {
                    mlt_Bes.Value = ClDoc.Account(2, "Column13");
                }
            }
            else
            {
                mlt_Bed.Value = ClDoc.Account(26, "Column07");
                mlt_Bes.Value = ClDoc.Account(26, "Column13");
            }

            faDatePicker1.SelectedDateTime = DateTime.Now;

            //****Generate Sharh****//
            //txt_Sharh.Text = "واریز نقد به " + txt_BankName.Text + " توسط ";
            //if (mlt_BesPerson.Text.Trim() != "")
            //    txt_Sharh.Text += mlt_BesPerson.Text + " در تاریخ " + RecRow["Column02"].ToString();
            //else if (RecRow["Column07"].ToString() != "")
            //    txt_Sharh.Text = "واریز نقد به " + txt_BankName.Text + " از " + ClDoc.ExScalar(ConBank.ConnectionString, "Table_020_BankCashAccInfo", "Column02", "ColumnId", RecRow["Column07"].ToString()) + " در تاریخ " + RecRow["Column02"].ToString();

            //else if (RecRow["Column11"].ToString() != "")
            //    txt_Sharh.Text = "واریز نقد به " + txt_BankName.Text + " از حساب  " + mlt_Bes.Text.Trim() + " در تاریخ " + RecRow["Column02"].ToString();

            //txt_Sharh.Text += " طی برگه شماره " + _PaperID +
            //    " بابت " + RecRow["Column04"].ToString();

            txt_Sharh.Text = ClDoc.ShablonDescGenerate(ConBase.ConnectionString,
                15, "واریز نقد",
                RecRow["Column02"].ToString(), "", RecRow["ColumnId"].ToString(),
                (RecRow["Column07"].ToString()!=""?  ClDoc.ExScalar(ConBank.ConnectionString, "Table_020_BankCashAccInfo", "Column02", "ColumnId", RecRow["Column07"].ToString()):""),
                txt_BankName.Text, _Person, mlt_BedPerson.Text, RecRow["Column04"].ToString(), mlt_Bes.Text.Trim(),"");


            txt_Cover.Text = "گردش خزانه- ثبت دریافت و پرداخت نقد";
            txt_BankName.Select();

            explorerBar1.Groups[0].Text = "صدور سند برگه دریافت شماره " + RecRow["ColumnId"].ToString();
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
                txt_Cover.Text = "گردش خزانه- ثبت دریافت و پرداخت نقد";
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
                Class_BasicOperation.CheckExceptionType(ex, "Form04_ExportDocForReceive");
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

        private void CheckEssentialItems()
        {

            if (mlt_Bed.Text.Trim() == "" || mlt_Bes.Text.Trim() == "" || txt_Sharh.Text.Trim() == "" || txt_Cover.Text.Trim() == "" || !faDatePicker1.SelectedDateTime.HasValue)
                throw new Exception("اطلاعات مورد نیاز را کامل کنید");

            if (mlt_Bed.Text.Trim().All(char.IsDigit) || mlt_Bes.Text.Trim().All(char.IsDigit))
                throw new Exception("سرفصل بدهکار یا بستانکار نامعتبر است");

            ClDoc.CheckForValidationDate(faDatePicker1.Text);
            //**********Check Bed and Bes for Person,Center and Project needs****//
            short? Person = null;
            if (mlt_BedPerson.Text.Trim() != "")
                Person = short.Parse(mlt_BedPerson.Value.ToString());
            clCredit.All_Controls(mlt_Bed.Value.ToString(), Person, null, null);

            short? Project = null;
            if (mlt_BesProject.Text.Trim() != "")
                Project = Int16.Parse(mlt_BesProject.Value.ToString());
            Person = null;
            if (mlt_BesPerson.Text.Trim() != "")
                Person = short.Parse(mlt_BesPerson.Value.ToString());
            clCredit.All_Controls(mlt_Bes.Value.ToString(), Person, null, Project);

            //**********Check Person Credit************//
            if (mlt_BesPerson.Text.Trim() != "" || mlt_BedPerson.Text.Trim() != "")
            {
                DataTable TPerson = new DataTable();
                TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
                TPerson.Columns.Add("Account", Type.GetType("System.String"));
                TPerson.Columns.Add("Price", Type.GetType("System.Double"));


                if (mlt_BedPerson.Text.Trim() != "")
                    TPerson.Rows.Add(Int32.Parse(mlt_BedPerson.Value.ToString()), mlt_Bed.Value.ToString(), Convert.ToDouble(txt_FirstAmount.Value.ToString()));
                if (mlt_BesPerson.Text.Trim() != "")
                    TPerson.Rows.Add(Int32.Parse(mlt_BesPerson.Value.ToString()), mlt_Bes.Value.ToString(), Convert.ToDouble(txt_FirstAmount.Value.ToString()) * -1);
                clCredit.CheckPersonCredit(TPerson, 0);
            }

            //**********Check Account's nature****//
            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));
            TAccounts.Rows.Add(mlt_Bed.Value.ToString(), Convert.ToDouble(txt_FirstAmount.Value.ToString()));
            TAccounts.Rows.Add(mlt_Bes.Value.ToString(), Convert.ToDouble(txt_FirstAmount.Value.ToString()) * -1);
            clCredit.CheckAccountCredit(TAccounts, 0);

        }

        private void ReturnDocId()
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
                DocID = ClDoc.DocID(DocNum);
            }
            else if (rdb_TO.Checked)
            {
                DocNum = int.Parse(txt_To.Text.Trim());
                DocID = ClDoc.DocID(DocNum);
            }
        }

        private bool CheckNotExported(int PaperId)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                Con.Open();
                SqlCommand Com = new SqlCommand(@"SELECT  ISNULL(Column08,0)   FROM   dbo.Table_045_ReceiveCash where ColumnId=" + PaperId, Con);
                if (int.Parse(Com.ExecuteScalar().ToString()) == 0)
                    return true;
                else return false;

            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            DocID = 0;
            DocNum = 0;
            //string personbed = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column36 from Table_020_BankCashAccInfo where Column01=0 AND Column12=" + mlt_Bed.Value+ "");
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

                CheckEssentialItems();

                if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //اگر سند صادر نشده باشد
                    if (CheckNotExported(_PaperID))
                    {
                        if (DocID == 0)
                            ReturnDocId();

                        string[] _BedInfo = ClDoc.ACC_Info(mlt_Bed.Value.ToString());
                        string[] _BesInfo = ClDoc.ACC_Info(mlt_Bes.Value.ToString());

                        if (RecRow["Column22"].ToString() == "False")
                        {
                            ClDoc.ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(), Int16.Parse(_BedInfo[0]),
                               _BedInfo[1], _BedInfo[2], _BedInfo[3], _BedInfo[4],
                               (mlt_BedPerson.Text.Trim() == "" ? null : mlt_BedPerson.Value.ToString()), null, null, txt_Sharh.Text,
                               Convert.ToInt64(Convert.ToDouble(txt_FirstAmount.Value.ToString())), 0, 0, 0, -1, 16, _PaperID, Class_BasicOperation._UserName, 0,
                               (RecRow["Column25"].ToString().Trim()==""?"NULL":RecRow["Column25"].ToString()),
                               (RecRow["Column26"].ToString().Trim() == "" ? "NULL" : "'" + RecRow["Column26"].ToString() + "'"));
                               
                            
                            
                            ClDoc.ExportDoc_Detail(DocID, mlt_Bes.Value.ToString(), Int16.Parse(_BesInfo[0]),
                               _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4], (mlt_BesPerson.Text.Trim() == "" ? null : mlt_BesPerson.Value.ToString()),
                               null, (mlt_BesProject.Text.Trim() == "" ? null : mlt_BesProject.Value.ToString()), txt_Sharh.Text, 0,
                               Convert.ToInt64(Convert.ToDouble(txt_FirstAmount.Value.ToString())), 0, 0, -1, 16, _PaperID, Class_BasicOperation._UserName, 0,
                               (RecRow["Column25"].ToString().Trim() == "" ? "NULL" : RecRow["Column25"].ToString()),
                               (RecRow["Column26"].ToString().Trim() == "" ? "NULL" : "'" + RecRow["Column26"].ToString() + "'"));
                        }
                        else
                        {
                            ClDoc.ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(), Int16.Parse(_BedInfo[0]),
                             _BedInfo[1], _BedInfo[2], _BedInfo[3], _BedInfo[4],
                              (mlt_BedPerson.Text.Trim() == "" ? null : mlt_BedPerson.Value.ToString()), null, null, txt_Sharh.Text,
                             Convert.ToInt64(
                             Convert.ToDouble(txt_FirstAmount.Value.ToString()) * Convert.ToDouble(RecRow["Column24"].ToString())
                             ), 0, Convert.ToDouble(txt_FirstAmount.Value.ToString())
                             , 0, Int16.Parse(RecRow["Column23"].ToString()), 16, _PaperID, Class_BasicOperation._UserName,
                             Convert.ToDouble(RecRow["Column24"].ToString()),
                               (RecRow["Column25"].ToString().Trim() == "" ? "NULL" : RecRow["Column25"].ToString()),
                               (RecRow["Column26"].ToString().Trim() == "" ? "NULL" : "'" + RecRow["Column26"].ToString() + "'"));

                            ClDoc.ExportDoc_Detail(DocID, mlt_Bes.Value.ToString(), Int16.Parse(_BesInfo[0]),
                               _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4],
                               (mlt_BesPerson.Text.Trim() == "" ? null : mlt_BesPerson.Value.ToString()),
                               null, (mlt_BesProject.Text.Trim() == "" ? null : mlt_BesProject.Value.ToString()), txt_Sharh.Text, 0,
                               Convert.ToInt64(Convert.ToDouble(txt_FirstAmount.Value.ToString()) * Convert.ToDouble(RecRow["Column24"].ToString())
                             ), 0, Convert.ToDouble(txt_FirstAmount.Value.ToString()),
                             Int16.Parse(RecRow["Column23"].ToString()), 16, _PaperID, Class_BasicOperation._UserName,
                             Convert.ToDouble(RecRow["Column24"].ToString()),
                               (RecRow["Column25"].ToString().Trim() == "" ? "NULL" : RecRow["Column25"].ToString()),
                               (RecRow["Column26"].ToString().Trim() == "" ? "NULL" : "'" + RecRow["Column26"].ToString() + "'"));
                        }

                        ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_045_ReceiveCash", "Column08", "ColumnId",
                            _PaperID, DocID);

                        Class_BasicOperation.ShowMsg(" ", "ثبت برگه دریافت مورد نظر با شماره " +
                            DocNum.ToString() + " انجام گرفت", Class_BasicOperation.MessageType.Information);
                        bt_Save.Enabled = false;
                        explorerBarContainerControl1.Enabled = false;

                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "برای این برگه سند حسابداری صادر شده است", Class_BasicOperation.MessageType.Warning);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_View_Click(object sender, EventArgs e)
        {
           Classes. Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 22))
            {
                PACNT._2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument(DocID);
                frm.ShowDialog();
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
            if (mlt_Bed.Text.Trim() != "" && mlt_BedPerson.Text.Trim() != ""  && !mlt_Bed.Text.Trim().All(char.IsDigit))
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

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (mlt_Bes.Text.Trim() != "" && mlt_BesPerson.Text.Trim() != "" && !mlt_Bes.Text.Trim().All(char.IsDigit))
            {
                DevComponents.DotNetBar.Balloon b = new DevComponents.DotNetBar.Balloon();
                b.Style = DevComponents.DotNetBar.eBallonStyle.Office2007Alert;
                b.CaptionText = "مانده حساب";
                b.CaptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b.CaptionImage = balloonTip1.CaptionImage.Clone() as Image;
                b.Text = ClDoc.PersonRemain(int.Parse(mlt_BesPerson.Value.ToString()), mlt_Bes.Value.ToString()).ToString("N0");
                b.AlertAnimation = DevComponents.DotNetBar.eAlertAnimation.TopToBottom;
                b.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b.AutoResize();
                b.Show(uiButton1, false);
            }
        }

        private void mlt_BedPerson_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");
            }
            catch 
            {
            }
        }

        private void mlt_BedPerson_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch 
            {
            }
        }

        private void mlt_Bed_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "ACC_Name", "ACC_Code");
            }
            catch 
            {
            }

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
