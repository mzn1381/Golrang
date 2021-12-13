using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;

namespace PCLOR._03_Bank
{
    public partial class Form08_TotalDoc_Receive : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlDataAdapter ChqAdapter;
        DataTable Status = new DataTable();
        Classes.Class_Documents clDocument = new Classes.Class_Documents();
        int DocNum = 0, DocID = 0;
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        bool _BackSpace = false;
        SqlParameter ID;
        public Form08_TotalDoc_Receive()
        {
            InitializeComponent();
        }

        private void Form08_TotalDoc_Receive_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                ChqAdapter = new SqlDataAdapter(@"SELECT        CAST(0 AS bit) AS Error, ' ' AS ErrorDes, dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, 
                         dbo.Table_035_ReceiptCheques.Column01, dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, 
                         dbo.Table_035_ReceiptCheques.Column04, dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, 
                         dbo.Table_035_ReceiptCheques.Column07, dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, 
                         dbo.Table_035_ReceiptCheques.Column10, dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, 
                         dbo.Table_035_ReceiptCheques.Column15, dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, 
                         dbo.Table_035_ReceiptCheques.Column43, dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, 
                         dbo.Table_035_ReceiptCheques.Column46, dbo.Table_035_ReceiptCheques.Column48, dbo.Table_060_ChequeStatus.Column03 AS Bed, 
                         dbo.Table_035_ReceiptCheques.Column49, dbo.Table_035_ReceiptCheques.Column50, dbo.Table_035_ReceiptCheques.Column51, 
                         dbo.Table_035_ReceiptCheques.Column52, dbo.Table_035_ReceiptCheques.Column53, dbo.Table_065_TurnReception.Column13
FROM            dbo.Table_035_ReceiptCheques INNER JOIN
                         dbo.Table_060_ChequeStatus ON dbo.Table_035_ReceiptCheques.Column48 = dbo.Table_060_ChequeStatus.ColumnId LEFT OUTER JOIN
                         dbo.Table_065_TurnReception ON dbo.Table_035_ReceiptCheques.ColumnId = dbo.Table_065_TurnReception.Column01
WHERE        (dbo.Table_035_ReceiptCheques.Column05 <> 0) AND (dbo.Table_065_TurnReception.Column13 IS NULL) ", ConBank);
            ChqAdapter.Fill(dataSet1, "Chq");
            bindingSource1.DataSource = dataSet1.Tables["Chq"];
            }
            else
            {

           
            ChqAdapter = new SqlDataAdapter(@"SELECT        CAST(0 AS bit) AS Error, ' ' AS ErrorDes, dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, 
                         dbo.Table_035_ReceiptCheques.Column01, dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, 
                         dbo.Table_035_ReceiptCheques.Column04, dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, 
                         dbo.Table_035_ReceiptCheques.Column07, dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, 
                         dbo.Table_035_ReceiptCheques.Column10, dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, 
                         dbo.Table_035_ReceiptCheques.Column15, dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, 
                         dbo.Table_035_ReceiptCheques.Column43, dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, 
                         dbo.Table_035_ReceiptCheques.Column46, dbo.Table_035_ReceiptCheques.Column48, dbo.Table_060_ChequeStatus.Column03 AS Bed, 
                         dbo.Table_035_ReceiptCheques.Column49, dbo.Table_035_ReceiptCheques.Column50, dbo.Table_035_ReceiptCheques.Column51, 
                         dbo.Table_035_ReceiptCheques.Column52, dbo.Table_035_ReceiptCheques.Column53, dbo.Table_065_TurnReception.Column13
FROM            dbo.Table_035_ReceiptCheques INNER JOIN
                         dbo.Table_060_ChequeStatus ON dbo.Table_035_ReceiptCheques.Column48 = dbo.Table_060_ChequeStatus.ColumnId LEFT OUTER JOIN
                         dbo.Table_065_TurnReception ON dbo.Table_035_ReceiptCheques.ColumnId = dbo.Table_065_TurnReception.Column01
WHERE        (dbo.Table_035_ReceiptCheques.Column05 <> 0) AND (dbo.Table_065_TurnReception.Column13 IS NULL) AND  Column42= N'" + Class_BasicOperation._UserName + @"'", ConBank);
            ChqAdapter.Fill(dataSet1, "Chq");
            bindingSource1.DataSource = dataSet1.Tables["Chq"];
            }
            gridEX1.DropDowns["Banks"].SetDataBinding(clDocument.ReturnTable(ConBank, "Select * from Table_010_BankNames"), "");

            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            gridEX1.DropDowns["Person"].SetDataBinding(Person, "");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01,Column02 from Table_035_ProjectInfo";
            DataTable Project = new DataTable();
            Adapter.Fill(Project);
            gridEX1.DropDowns["Project"].SetDataBinding(Project, "");
            mlt_Project.DataSource = Project;
    

            Adapter.SelectCommand.Connection = ConBank;
            Adapter.SelectCommand.CommandText = "Select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo";
            DataTable BoxBankTable = new DataTable();
            Adapter.Fill(BoxBankTable);
            gridEX1.DropDowns["ToBank"].SetDataBinding(BoxBankTable, "");

            Adapter.SelectCommand.CommandText = "Select * from Table_060_ChequeStatus where Column01=0";
            Adapter.Fill(Status);
            cmb_Status.ComboBox.DataSource = clDocument.ReturnTable(ConBank, @"Select * from Table_060_ChequeStatus where Column15=1");
            cmb_Status.ComboBox.DisplayMember = "Column02";
            cmb_Status.ComboBox.ValueMember = "ColumnId";
            bt_Display_Click(sender, e);
            gridEX1.DataSource = dataSet1.Tables["Chq"];


            Adapter.SelectCommand.Connection = ConAcnt;
            Adapter.SelectCommand.CommandText = "Select ACC_Code,ACC_Name from AllHeaders()";
            DataTable Headers = new DataTable();
            Adapter.Fill(Headers);
            gridEX1.DropDowns["Header"].SetDataBinding(Headers, "");
            mlt_Bes.DataSource = Headers;

            gridEX1.DropDowns["Currency"].SetDataBinding(clDocument.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo"), "");

            faDatePicker1.SelectedDateTime = DateTime.Now;
            gridEX1.MoveFirst();
        }

        private void mlt_Bed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);

                
        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                txt_Cover.Text = "گردش خزانه- ثبت اسناد دریافتنی";
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
                int LastNum = clDocument.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = clDocument.DocDate( LastNum);
                txt_Cover.Text = clDocument.Cover( LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_Cover.Text = null;
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

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void txt_To_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_To.Text.Trim() != "")
                {
                    clDocument.IsValidNumber( int.Parse(txt_To.Text.Trim()));
                    faDatePicker1.Text = clDocument.DocDate( int.Parse(txt_To.Text.Trim()));
                    txt_Cover.Text = clDocument.Cover( int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
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

        private void bt_View_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewDocument")
                    {
                        item.BringToFront();
                        ((PACNT._2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Position =
                            ((PACNT._2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Find("ColumnId", DocID);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument(DocID);
                try { frm.MdiParent = Frm_Main.ActiveForm; }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید",Class_BasicOperation.MessageType.None);
        }

        private void Form08_TotalDoc_Receive_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.W)
                bt_View_Click(sender, e);
        }

        private void Form08_TotalDoc_Receive_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        
        }

        private void ReturnDocId()
        {
            if (rdb_New.Checked)
            {
                DocNum = clDocument.LastDocNum() + 1;
                DocID = clDocument.ExportDoc_Header(DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName, 2);
                DocNum = clDocument.DocNum(DocID);
            }
            else if (rdb_last.Checked)
            {
                DocNum = clDocument.LastDocNum();
                DocID = clDocument.DocID(DocNum);
            }
            else if (rdb_TO.Checked)
            {
                DocNum = int.Parse(txt_To.Text.Trim());
                DocID = clDocument.DocID(DocNum);
            }
        }

        private bool CheckNotExported(int PaperId)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                Con.Open();
                SqlCommand Com = new SqlCommand(@"SELECT   COUNT(  Column01) as Number  FROM          dbo.Table_065_TurnReception where Column01=" + PaperId, Con);
                if (int.Parse(Com.ExecuteScalar().ToString()) == 0)
                    return true;
                else return false;

            }
        }

        private void CheckEssentialItems()
        {
            uiGroupBox2.Visible = false;
            //*****Check Essential Information***//
            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Trim() == "" || faDatePicker1.Text.Trim().Length > 10 || mlt_Bes.Text.Trim() == "")
                throw new WarningException("اطلاعات مربوط به صدور سند را کامل کنید");

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDocument.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDocument.CheckExistFinalDoc();

            foreach (GridEXRow item in gridEX1.GetCheckedRows())
            {
                if (item.Cells["Bed"].Text.Trim() == "" || item.Cells["Bed"].Text.Trim().All(char.IsDigit))
                    throw new WarningException("بدهکار برگه شماره " +
                        item.Cells["ColumnId"].Text + " مشخص نگردیده است");

                if (item.Cells["Sharh"].Text.Trim() == "")
                    throw new WarningException("شرح ردیف برگه دریافت شماره " + item.Cells["ColumnId"].Value.ToString() + " مشخص نشده است");

            }

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                item.BeginEdit();
                item.Cells["Error"].Value = false;
                item.Cells["ErrorDes"].Value = DBNull.Value;
                item.EndEdit();
            }

            //Define Table to check person's credit//
            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));
            TPerson.Rows.Clear();
            //Define Table to check account's nature//
            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));
            short? Person = null;
            short? Project = null;
           

            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();
            foreach (GridEXRow item in gridEX1.GetCheckedRows())
            {
                //**********Check Bed and Bes for Person,Center and Project needs****//
                Person = null;
                Project = null;
                if (mlt_Project.Text.Trim() != "")
                    Project = short.Parse(mlt_Project.Value.ToString());

                if (item.Cells["Column46"].Text.Trim() != "")
                    Person = short.Parse(item.Cells["Column46"].Value.ToString());
                
                try
                {
                    clCredit.All_Controls_2(item.Cells["Bed"].Value.ToString(), Person, null, Project);
                }
                catch (Exception es)
                {
                    item.BeginEdit();
                    item.Cells["Error"].Value = true;
                    item.Cells["ErrorDes"].Value = es.Message;
                    item.EndEdit();
                }

                Project = null;
                Person = null;
                if (item.Cells["Column15"].Text.Trim() != "")
                    Project = short.Parse(item.Cells["Column15"].Value.ToString());

                if (item.Cells["Column07"].Text.Trim() != "")
                    Person = short.Parse(item.Cells["Column07"].Value.ToString());
                try
                {
                    clCredit.All_Controls_2(mlt_Bes.Value.ToString(), Person, null, Project);
                }
                catch (Exception es)
                {
                    item.BeginEdit();
                    item.Cells["Error"].Value = true;
                    item.Cells["ErrorDes"].Value = es.Message;
                    item.EndEdit();
                }

                //**********Check Person Credit************//
                if (item.Cells["Column07"].Text.Trim() != "" || item.Cells["Column46"].Text.Trim() != "")
                {
                    if (item.Cells["Column46"].Text.Trim() != "")
                        TPerson.Rows.Add(Int32.Parse(item.Cells["Column46"].Value.ToString()), item.Cells["Bed"].Value.ToString(), Convert.ToDouble(item.Cells["Column05"].Value.ToString()));
                    if (item.Cells["Column07"].Text.Trim() != "")
                        TPerson.Rows.Add(Int32.Parse(item.Cells["Column07"].Value.ToString()), mlt_Bes.Value.ToString(), Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * -1);
                }
                //**********Check Account's nature****//
                TAccounts.Rows.Add(item.Cells["Bed"].Value.ToString(), Convert.ToDouble(item.Cells["Column05"].Value.ToString()));
                TAccounts.Rows.Add(mlt_Bes.Value.ToString(), Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * -1);
            }
            clCredit.CheckPersonCredit(TPerson, 0);
            clCredit.CheckAccountCredit(TAccounts, 0);
            gridEX1.UpdateData();
        }

        string cmdText = "";
        private void bt_Save_Click(object sender, EventArgs e)
        {
            bool _ShowMsg = false;
            DocID = 0;
            DocNum = 0;
            gridEX1.RemoveFilters();
            gridEX1.UpdateData();

            try
            {
                if (rdb_last.Checked && txt_LastNum.Text.Trim() != "")
                {
                    clDocument.IsFinal(int.Parse(txt_LastNum.Text.Trim()));
                }
                else if (rdb_TO.Checked && txt_To.Text.Trim() != "")
                {
                    clDocument.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                    clDocument.IsFinal(int.Parse(txt_To.Text.Trim()));
                    txt_To_Leave(sender, e);
                }

                if (gridEX1.GetCheckedRows().Length > 0)
                {
                    CheckEssentialItems();
                    if (gridEX1.GetCheckedRows().Length > 0 && gridEX1.Find(gridEX1.RootTable.Columns["ErrorDes"], ConditionOperator.NotIsNull, null, null, -1, 1))
                    {
                        uiGroupBox2.Visible = true;
                        gridEX1_SelectionChanged(sender, e);
                        Class_BasicOperation.ShowMsg("", "با توجه به هشدارهای نمایش داده شده صدور سند امکانپذیر نیست", Class_BasicOperation.MessageType.Warning);
                        return;
                    }

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        string[] _BesInfo = clDocument.ACC_Info(mlt_Bes.Value.ToString());
                        cmdText = "";
                        foreach (GridEXRow item in gridEX1.GetCheckedRows())
                        {
                            //اگر سند صادر نشده باشد
                            if (CheckNotExported(int.Parse(item.Cells["ColumnId"].Value.ToString())))
                            {
                                string[] _BedInfo = clDocument.ACC_Info(item.Cells["Bed"].Value.ToString());

                                if (DocID == 0)
                                    ReturnDocId();

                               cmdText=cmdText+    AddTurnReception(int.Parse(item.Cells["ColumnId"].Value.ToString()), short.Parse(item.Cells["Column48"].Value.ToString()), faDatePicker1.Text,
                                     (item.Cells["Column46"].Text.ToString() == "" ? "NULL" : item.Cells["Column46"].Value.ToString()), item.Cells["Column01"].Value.ToString(),
                                      item.Cells["Bed"].Value.ToString(), Int16.Parse(_BedInfo[0].ToString()),
                                      (_BedInfo[1].ToString() == "" ? "NULL" : _BedInfo[1].ToString()), (_BedInfo[2].ToString() == string.Empty ? "NULL" : _BedInfo[2].ToString())
                                      , (_BedInfo[3].ToString() == "" ? "NULL" : _BedInfo[3].ToString()), (_BedInfo[4].ToString() == string.Empty ? "NULL" : _BedInfo[4].ToString()), DocID, "NULL",
                                      (mlt_Project.Text.Trim() == "" ? "NULL" : mlt_Project.Value.ToString()), Class_BasicOperation._UserName, "",
                                      item.Cells["Column49"].Value.ToString(),
                                      (item.Cells["Column50"].Text.Trim() == "" ? null : item.Cells["Column50"].Value.ToString()),
                                      Convert.ToDouble(item.Cells["Column51"].Value.ToString()));


                                if (item.Cells["Column49"].Value.ToString() == "False")
                                {
                                    cmdText = cmdText + ExportDoc_Detail(DocID, item.Cells["Bed"].Value.ToString(),
                                        Int16.Parse(_BedInfo[0].ToString()), _BedInfo[1].ToString(), _BedInfo[2].ToString(), _BedInfo[3].ToString(), _BedInfo[4].ToString()
                                        ,(item.Cells["Column46"].Text.Trim()==""?null: item.Cells["Column46"].Value.ToString()), null,
                                        (mlt_Project.Text.Trim() == "" ? "NULL" : mlt_Project.Value.ToString()),
                                        item.Cells["Sharh"].Text, Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())), 0, 0, 0, -1,
                                        Int16.Parse(cmb_Status.ComboBox.SelectedValue.ToString()), 0, Class_BasicOperation._UserName, 0,
                                      (item.Cells["Column52"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column52"].Value.ToString()),
                                      (item.Cells["Column53"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column53"].Value.ToString() + "'"));

                                    cmdText = cmdText + ExportDoc_Detail(DocID, mlt_Bes.Value.ToString(),
                                        Int16.Parse(_BesInfo[0].ToString()), _BesInfo[1].ToString(),
                                        _BesInfo[2].ToString(), _BesInfo[3].ToString(), _BesInfo[4].ToString(),
                                        ( item.Cells["Column07"].Text.Trim()==""?null:item.Cells["Column07"].Value.ToString()), null, 
                                        (item.Cells["Column15"].Text.Trim()==""?null: item.Cells["Column15"].Value.ToString()),
                                        item.Cells["Sharh"].Text, 0, Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())), 0, 0, -1,
                                        Int16.Parse(cmb_Status.ComboBox.SelectedValue.ToString()), 0, Class_BasicOperation._UserName, 0,
                                      (item.Cells["Column52"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column52"].Value.ToString()),
                                      (item.Cells["Column53"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column53"].Value.ToString() + "'"));
                                }
                                else
                                {
                                    cmdText = cmdText + ExportDoc_Detail(DocID, item.Cells["Bed"].Value.ToString(), Int16.Parse(_BedInfo[0].ToString()), _BedInfo[1].ToString(), _BedInfo[2].ToString(), _BedInfo[3].ToString(), _BedInfo[4].ToString()
                                        ,(item.Cells["Column46"].Text.Trim()==""?null: item.Cells["Column46"].Value.ToString()), null, 
                                        (mlt_Project.Text.Trim() == "" ? "NULL" : mlt_Project.Value.ToString()),
                                     item.Cells["Sharh"].Text,
                                     Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * Convert.ToDouble(item.Cells["Column51"].Value.ToString()))
                                     , 0, Convert.ToDouble(item.Cells["Column05"].Value.ToString()), 0,
                                       Int16.Parse(item.Cells["Column50"].Value.ToString())
                                     , Int16.Parse(cmb_Status.ComboBox.SelectedValue.ToString()),
                                     0, Class_BasicOperation._UserName, Convert.ToDouble(item.Cells["Column51"].Value.ToString()),
                                      (item.Cells["Column52"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column52"].Value.ToString()),
                                      (item.Cells["Column53"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column53"].Value.ToString() + "'"));

                                    cmdText = cmdText + ExportDoc_Detail(DocID, mlt_Bes.Value.ToString(), Int16.Parse(_BesInfo[0].ToString()), _BesInfo[1].ToString(), _BesInfo[2].ToString(), _BesInfo[3].ToString(), _BesInfo[4].ToString(),
                                        (item.Cells["Column07"].Text.Trim()==""?null:item.Cells["Column07"].Value.ToString()), null, 
                                        (item.Cells["Column15"].Text.Trim()==""?null: item.Cells["Column15"].Value.ToString())
                                        , item.Cells["Sharh"].Text, 0,
                                        Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * Convert.ToDouble(item.Cells["Column51"].Value.ToString()))
                                        , 0, Convert.ToDouble(item.Cells["Column05"].Value.ToString()),
                                        Int16.Parse(item.Cells["Column50"].Value.ToString()),
                                        Int16.Parse(cmb_Status.ComboBox.SelectedValue.ToString()), 0, Class_BasicOperation._UserName,
                                         Convert.ToDouble(item.Cells["Column51"].Value.ToString()),
                                      (item.Cells["Column52"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column52"].Value.ToString()),
                                      (item.Cells["Column53"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column53"].Value.ToString() + "'"));
                                }

                                _ShowMsg = true;
                            }
                            else Class_BasicOperation.ShowMsg("", "برای برگه شماره " + item.Cells["ChqId"].Value.ToString() + " گردش ثبت شده است", Class_BasicOperation.MessageType.Warning);
                        }

                        if (_ShowMsg)
                        {
                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                            {
                                Con.Open();

                                SqlTransaction sqlTran = Con.BeginTransaction();
                                SqlCommand Command = Con.CreateCommand();
                                Command.Connection = Con;
                                Command.Transaction = sqlTran;

                                try
                                {
                                    ID = new SqlParameter("ID", SqlDbType.Int);
                                    ID.Direction = ParameterDirection.Output;

                                    Command.CommandText = cmdText;
                                    Command.Parameters.Add(ID);

                                    Command.ExecuteNonQuery();
                                    sqlTran.Commit();

                                    Class_BasicOperation.ShowMsg(" ", "صدور سند حسابداری با شماره  " + DocNum.ToString() + " انجام گرفت", Class_BasicOperation.MessageType.Information);
                      

                                }
                                catch (Exception es)
                                {
                                    MessageBox.Show("ثبت گردش و سند با مشکل مواجه شده است لطفا مجدد سعی نمائید");
                                    sqlTran.Rollback();
                                    this.Cursor = Cursors.Default;
                                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                                }

                                this.Cursor = Cursors.Default;

                            }
                        }
                        dataSet1.Tables["Chq"].Rows.Clear();
                        ChqAdapter.Fill(dataSet1, "Chq");
                        bt_Display_Click(sender, e);
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                        this.Close();
                    }

                  
                }
               
            }

            catch (Exception ex)
            {
                WarningException es = new WarningException();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

                if (ex.GetBaseException().GetType() != es.GetBaseException().GetType())
                {
                    bt_Display_Click(sender, e);
                }
            }

        }
        private string ExportDoc_Detail(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
          string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
          Int64 Bes, Double CurBed, Double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, Double CurPrice, object Period, object EffDate)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";


            string cmd = (@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]      ,[Column23]      ,[Column24]) VALUES (" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                        + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," + CurType + "," + SanadType + ",@ID,'" + RegUser + "',getdate(),'" + RegUser + "',getdate()," + CurPrice + "," +
                        Period + "," + EffDate + ");");


            return cmd;

        }
        private string AddTurnReception(int PaperID, Int16 Status, string Date, string Person, string BoxBank, string ACC, Int16 Group, string Kol, string Moein, string Tafsili, string Joz,
           int SanadID, string Center, string Project, string User, string Description, string Currency, string CurrencyType, Double CurrencyValue)
        {

            string CommandBehavior = (@"INSERT INTO " + ConBank.Database + ".dbo.Table_065_TurnReception VALUES(" + PaperID + "," + Status + ",'" + Date + "'," +
                Person + "," + BoxBank + ",'" + ACC + "'," + Group + "," + Kol + "," + Moein + "," + Tafsili + "," + Joz + "," + SanadID + "," + Center + "," + Project + ",'" + User + "',getdate(),"
                + (Description.ToString().Trim() == "" ? "NULL" : "'" + Description + "'") +
                "," + (Currency == "True" ? 1 : 0) +
                "," + (string.IsNullOrEmpty(CurrencyType) ? "NULL" : CurrencyType) + "," + CurrencyValue +
                "); SET @ID=SCOPE_IDENTITY();");
            //SqlParameter _ID = new SqlParameter("ID", SqlDbType.BigInt);
            return CommandBehavior;

        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_Status.ComboBox.Text != "")
                {
                 
                    dataSet1.Tables["Chq"].Rows.Clear();
                    ChqAdapter.Fill(dataSet1, "Chq");
                    bindingSource1.Filter = "Column48=" + cmb_Status.ComboBox.SelectedValue;
                    mlt_Bes.Value = Status.Select("ColumnId=" + cmb_Status.ComboBox.SelectedValue, "")[0]["Column09"].ToString();
                }
            }
            catch { }
        }

        private void cmb_Status_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                bt_Display_Click(sender, e);
        }

        private void gridEX1_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
        {
            try
            {
                if (e.ChangeType == Janus.Windows.GridEX.CheckStateChangeType.RowChange)
                {
                    if (e.CheckState == Janus.Windows.GridEX.RowCheckState.Checked)
                    {
                        //gridEX1.SetValue("Sharh",
                        //    cmb_Status.ComboBox.Text + "- ش " + gridEX1.GetValue("Column03").ToString() +
                        //    " س " + gridEX1.GetValue("Column04").ToString() + "  به  " +
                        //    gridEX1.GetRow().Cells["Column01"].Text + " از " +
                        //           gridEX1.GetRow().Cells["Column07"].Text + " حساب " +
                        //           gridEX1.GetRow().Cells["Column10"].Text + " بابت " +
                        //           gridEX1.GetRow().Cells["Column06"].Text + " صادره از بانک " +
                        //           gridEX1.GetRow().Cells["Column08"].Text);
                        gridEX1.SetValue("Sharh",
                         clDocument.ShablonDescGenerate(ConBase.ConnectionString,
       clDocument.GetTypeCheckStatusShablon(cmb_Status.ComboBox.SelectedValue.ToString(), ConBank), cmb_Status.ComboBox.Text,
                 gridEX1.GetRow().Cells["Column02"].Text, gridEX1.GetRow().Cells["Column04"].Text, gridEX1.GetRow().Cells["Column03"].Text,
                 "",
               gridEX1.GetRow().Cells["Column01"].Text,
               gridEX1.GetRow().Cells["Column07"].Text.ToString(), gridEX1.GetRow().Cells["Column46"].Text.ToString(),
               gridEX1.GetRow().Cells["Column06"].Text.ToString(), mlt_Bes.Text.Trim(), gridEX1.GetRow().Cells["Column08"].Text));
                    }
                    else gridEX1.SetValue("Sharh", null);
                }
                else
                {
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {
                        if (item.CheckState == Janus.Windows.GridEX.RowCheckState.Checked)
                        {
                            item.BeginEdit();
                            item.Cells["Sharh"].Value = clDocument.ShablonDescGenerate(ConBase.ConnectionString,
                 clDocument.GetTypeCheckStatusShablon(cmb_Status.ComboBox.SelectedValue.ToString(), ConBank), cmb_Status.ComboBox.Text,
                 item.Cells["Column02"].Text,
                item.Cells["Column04"].Text, item.Cells["Column03"].Text.ToString(),
                "",
              item.Cells["Column01"].Text,
               item.Cells["Column07"].Text.ToString(),
              item.Cells["Column46"].Text.ToString(),
              item.Cells["Column06"].Text.ToString(), mlt_Bes.Text.Trim(), item.Cells["Column08"].Text);
                          // cmb_Status.ComboBox.Text + "- ش " + item.Cells["Column03"].Value.ToString() +
                          // " س " + item.Cells["Column04"].Value.ToString() + "  به  " +
                          //item.Cells["Column01"].Text + " از " +
                          //       item.Cells["Column07"].Text + " حساب " +
                          //       item.Cells["Column10"].Text + " بابت " +
                          //       item.Cells["Column06"].Text + " صادره از بانک " +
                          //         gridEX1.GetRow().Cells["Column08"].Text;

                            item.EndEdit();
                        }
                        else
                        {
                            item.BeginEdit();
                            item.Cells["Sharh"].Value = DBNull.Value;
                            item.EndEdit();
                        }
                    }
                }

            }
            catch { }
        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXExporter1.GridEX = gridEX1;
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", Class_BasicOperation.MessageType.Information);
            }
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
            try
            {
                Class_BasicOperation.FilterGridExDropDown(sender, "Bed", "ACC_Code", "ACC_Name", gridEX1.EditTextBox.Text);
            }
            catch 
            {

            }
            try
            {
                Class_BasicOperation.FilterGridExDropDown(sender, "Column46", "Column01", "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
            try
            {
                Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
        }

        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                label1.Text = gridEX1.GetValue("ErrorDes").ToString();
            }
            catch
            {
            }
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender,"Bed");
            }
            catch 
            {
            }
        }

        private void gridEX1_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Bed");
            }
            catch
            {
            }
        }

        private void mlt_Bes_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "ACC_Name", "ACC_Code");
            }
            catch
            {
            }

        }

        private void mlt_Bes_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch 
            {
            }
        }

        private void gridEX1_FormattingRow(object sender, RowLoadEventArgs e)
        {

        }

       

       
    }
}
