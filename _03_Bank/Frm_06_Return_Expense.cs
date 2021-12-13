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
    public partial class Frm_06_Return_Expense : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        bool _BackSpace = false;
        DataTable dt;
        int DocNum = 0, DocID = 0;
        SqlParameter ID;
        SqlDataAdapter DocAdapter;
        public Frm_06_Return_Expense()
        {
            InitializeComponent();
        }

        private void Frm_06_Return_Expense_Load(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                //string Bed = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select Column03 from  Table_060_ChequeStatus  where Column15=5),0)");
                //string Bes = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select Column09 from Table_060_ChequeStatus where Column15=5),0)");
                string status = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select ColumnId from Table_060_ChequeStatus where Column15=5 AND Column09=150100),0)");
                //mlt_Bed.DataSource = ClDoc.ReturnTable(ConACNT, "select  * from AllHeaders() where ACC_Code=" + Bed + "");
                //mlt_Bes.DataSource = ClDoc.ReturnTable(ConACNT, "select  * from AllHeaders() where ACC_Code=" + Bes + "");
                //mlt_Bed.Value = Bed;
                //mlt_Bes.Value = Bes;
                mlt_status.Value = status;
                //mlt_status.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnID,Column01,Column02 from Table_060_ChequeStatus Column15=5");
                mlt_status.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnID,Column01,Column02 from Table_060_ChequeStatus where Column15=5");
                txt_Dat_Recipt.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                faDatePicker1.SelectedDateTime = DateTime.Now;

                gridEX1.DropDowns["ACC"].DataSource = ClDoc.ReturnTable(ConACNT, @"select  * from AllHeaders() ");
                gridEX1.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["ToBank"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["Doc"].DataSource = ClDoc.ReturnTable(ConACNT, @"Select ColumnId,Column00 from Table_060_SanadHead");

                if (isadmin)
                {
                    gridEX1.DataSource = ClDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, dbo.Table_035_ReceiptCheques.Column49, dbo.Table_035_ReceiptCheques.Column50, 
                         dbo.Table_035_ReceiptCheques.Column51, dbo.Table_035_ReceiptCheques.Column52, dbo.Table_035_ReceiptCheques.Column53, 
                         dbo.Table_035_ReceiptCheques.Column54, dbo.Table_035_ReceiptCheques.Column55, Table_065_TurnReception_2.Column01 AS DocId, 
                         dbo.Table_060_ChequeStatus.Column15 AS Status, Table_065_TurnReception_2.Column19 AS TurnDes, Table_065_TurnReception_2.Column21, 
                         Table_065_TurnReception_2.Column20, Table_065_TurnReception_2.Column02 AS gardesh, Table_065_TurnReception_2.Column22, 
                         Table_065_TurnReception_2.Column06 AS TurnBankBox, Table_065_TurnReception_2.Column07 AS TurnACC, 
                         dbo.Table_010_BankNames.Column01 AS MainBankName, Table_065_TurnReception_2.Column13 AS TurnSanad, 
                         dbo.Table_020_BankCashAccInfo.Column12 AS Bed, Table_065_TurnReception_2.Column05 AS PersonRecipt
FROM            dbo.Table_035_ReceiptCheques INNER JOIN
                             (SELECT        TOP (100) PERCENT Column01 AS PaperID, ColumnId AS LastTurnID, Column02 AS LastState
                               FROM            dbo.Table_065_TurnReception
                               WHERE        (Column13 <> 0) AND (ColumnId IN
                                                             (SELECT        MAX(ColumnId) AS Expr1
                                                               FROM            dbo.Table_065_TurnReception AS Table_065_TurnReception_1
                                                               WHERE        (Column13 <> 0)
                                                               GROUP BY Column01))
                               ORDER BY PaperID, LastTurnID DESC) AS TurnReception ON dbo.Table_035_ReceiptCheques.ColumnId = TurnReception.PaperID INNER JOIN
                         dbo.Table_065_TurnReception AS Table_065_TurnReception_2 ON dbo.Table_035_ReceiptCheques.ColumnId = Table_065_TurnReception_2.Column01 INNER JOIN
                         dbo.Table_060_ChequeStatus ON Table_065_TurnReception_2.Column02 = dbo.Table_060_ChequeStatus.ColumnId INNER JOIN
                         dbo.Table_010_BankNames ON dbo.Table_035_ReceiptCheques.Column08 = dbo.Table_010_BankNames.Column00 INNER JOIN
                         dbo.Table_020_BankCashAccInfo ON dbo.Table_035_ReceiptCheques.Column01 = dbo.Table_020_BankCashAccInfo.ColumnId
WHERE        (TurnReception.LastState IN
                             (SELECT        ColumnId
                               FROM            dbo.Table_060_ChequeStatus AS Table_060_ChequeStatus_1
                               WHERE        (Column15 = 2))) AND (Table_065_TurnReception_2.Column01 <> 0) AND (dbo.Table_060_ChequeStatus.Column15 = 2)");
                }

                else
                {
                    gridEX1.DataSource = ClDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, dbo.Table_035_ReceiptCheques.Column49, dbo.Table_035_ReceiptCheques.Column50, 
                         dbo.Table_035_ReceiptCheques.Column51, dbo.Table_035_ReceiptCheques.Column52, dbo.Table_035_ReceiptCheques.Column53, 
                         dbo.Table_035_ReceiptCheques.Column54, dbo.Table_035_ReceiptCheques.Column55, Table_065_TurnReception_2.Column01 AS DocId, 
                         dbo.Table_060_ChequeStatus.Column15 AS Status, Table_065_TurnReception_2.Column19 AS TurnDes, Table_065_TurnReception_2.Column21, 
                         Table_065_TurnReception_2.Column20, Table_065_TurnReception_2.Column02 AS gardesh, Table_065_TurnReception_2.Column22, 
                         Table_065_TurnReception_2.Column06 AS TurnBankBox, Table_065_TurnReception_2.Column07 AS TurnACC, 
                         dbo.Table_010_BankNames.Column01 AS MainBankName, Table_065_TurnReception_2.Column13 AS TurnSanad, 
                         dbo.Table_020_BankCashAccInfo.Column12 AS Bed, Table_065_TurnReception_2.Column16, Table_065_TurnReception_2.Column05 AS PersonRecipt
FROM            dbo.Table_035_ReceiptCheques INNER JOIN
                             (SELECT        TOP (100) PERCENT Column01 AS PaperID, ColumnId AS LastTurnID, Column02 AS LastState
                               FROM            dbo.Table_065_TurnReception
                               WHERE        (Column13 <> 0) AND (ColumnId IN
                                                             (SELECT        MAX(ColumnId) AS Expr1
                                                               FROM            dbo.Table_065_TurnReception AS Table_065_TurnReception_1
                                                               WHERE        (Column13 <> 0)
                                                               GROUP BY Column01))
                               ORDER BY PaperID, LastTurnID DESC) AS TurnReception ON dbo.Table_035_ReceiptCheques.ColumnId = TurnReception.PaperID INNER JOIN
                         dbo.Table_065_TurnReception AS Table_065_TurnReception_2 ON dbo.Table_035_ReceiptCheques.ColumnId = Table_065_TurnReception_2.Column01 INNER JOIN
                         dbo.Table_060_ChequeStatus ON Table_065_TurnReception_2.Column02 = dbo.Table_060_ChequeStatus.ColumnId INNER JOIN
                         dbo.Table_010_BankNames ON dbo.Table_035_ReceiptCheques.Column08 = dbo.Table_010_BankNames.Column00 INNER JOIN
                         dbo.Table_020_BankCashAccInfo ON dbo.Table_035_ReceiptCheques.Column01 = dbo.Table_020_BankCashAccInfo.ColumnId
                        WHERE     (TurnReception.LastState IN
                          (SELECT     ColumnId
                            FROM          dbo.Table_060_ChequeStatus AS Table_060_ChequeStatus_1
                            WHERE      (Column15 = 2))) AND (Table_065_TurnReception_2.Column01 <> 0) AND (dbo.Table_060_ChequeStatus.Column15 = 2) AND (dbo.Table_035_ReceiptCheques.Column42 = N'" + Class_BasicOperation._UserName + "') AND (Table_065_TurnReception_2.Column16 = N'" + Class_BasicOperation._UserName + "')");
                }
            }
            catch { }

        }
        string CommandText = "";
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (mlt_status.Text == "" || mlt_status.Text == "0" 
               || txt_Dat_Recipt.Text == "0" || txt_Dat_Recipt.Text == "")
            {
                MessageBox.Show("لطفا اطلاعات با دقت وارد کنید");
                return;
            }
             gridEX1.UpdateData();
            bool _ShowMsg = false;
            DocID = 0;
            DocNum = 0;
            gridEX1.RemoveFilters();
            int _TurnID = 1;
            
            try
            {
                if (rdb_last.Checked && txt_LastNum.Text.Trim() != "")
                {
                    ClDoc.IsFinal(int.Parse(txt_LastNum.Text.Trim()));
                }
                else if (rdb_TO.Checked && txt_To.Text.Trim() != "")
                {
                    ClDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                    ClDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
                    txt_To_Leave(sender, e);
                }

                if (gridEX1.GetCheckedRows().Length > 0)
                {
                    CheckEssentialItems();
                    if (gridEX1.GetCheckedRows().Length > 0 && gridEX1.Find(gridEX1.RootTable.Columns["ErrorDes"], ConditionOperator.NotIsNull, null, null, -1, 1))
                    {
                        gridEX1_SelectionChanged(sender, e);
                        Class_BasicOperation.ShowMsg("", "با توجه به هشدارهای نمایش داده شده صدور سند امکانپذیر نیست", Class_BasicOperation.MessageType.Warning);
                        return;
                    }



                    if (MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (check.Checked == true)
                        {
                           
                            string[] _Bed_Info = ClDoc.ACC_Info(mlt_Bed.Value.ToString());
                            string Bes = (mlt_Bes.Text.Trim() == "" ? "NULL" : mlt_Bes.Value.ToString());
                            string BesPerson = "NULL";
                            string BesCenter = "NULL";
                            string BesProject = "NULL";
                            string BedProject = "NULL";
                            string BedPerson = (gridEX1.GetValue("Column46").ToString().Trim() == "" ? "NULL" : gridEX1.GetValue("Column46").ToString());
                            CommandText = "";
                            foreach (GridEXRow item in gridEX1.GetCheckedRows())
                            {

                                DateTime Date = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(txt_Dat_Recipt.Text)));
                                DateTime DateR = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(item.Cells["Column04"].Value.ToString())));


                                if (Convert.ToInt32(Math.Floor((Date - DateR).TotalDays)) < 0)
                                {
                                    MessageBox.Show("تاریخ سر رسید به تاریخ روز نمی باشد");
                                    return;
                                }
                            

                                //اگر گردشی برای چک ثبت نشده باشد
                                /// if (CheckNotExported(int.Parse(item.Cells["ChqID"].Value.ToString()), item.Cells["LastState"].Value.ToString()))
                                {
                                    BesPerson = (item.Cells["PersonRecipt"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["PersonRecipt"].Value.ToString());
                                    BesCenter = "NULL";
                                    BesProject = "NULL";
                                    BedPerson = "NULL";
                                    BedProject = "NULL";
                                    BedPerson = (item.Cells["Column46"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column46"].Value.ToString());


                                    if (DocID == 0)
                                        ReturnDocId();

                                  
                                    //Bes Account//

                                    //Bes = item.Cells["TurnACC"].Value.ToString();
                                    Bes = mlt_Bes.Value.ToString();
                                    string[] _Bes_Info = ClDoc.ACC_Info(Bes);
                                    //Bes Person
                                    if (check.Checked == true && item.Cells["Column46"].Text.Trim() != "")
                                        BedPerson = item.Cells["Column46"].Value.ToString();

                                    //*******First Add Turn to TurnTable***************//
                                    CommandText = CommandText + AddTurnReception(int.Parse(item.Cells["ColumnID"].Value.ToString()), short.Parse(mlt_status.Value.ToString()), faDatePicker1.Text,
                                         BedPerson, (item.Cells["TurnBankBox"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["TurnBankBox"].Value.ToString()),
                                          mlt_Bed.Value.ToString(), Int16.Parse(_Bed_Info[0].ToString()),
                                          (_Bed_Info[1].ToString() == "" ? "NULL" : _Bed_Info[1].ToString()), (_Bed_Info[2].ToString() == string.Empty ? "NULL" : _Bed_Info[2].ToString())
                                          , (_Bed_Info[3].ToString() == "" ? "NULL" : _Bed_Info[3].ToString()), (_Bed_Info[4].ToString() == string.Empty ? "NULL" : _Bed_Info[4].ToString()), DocID, ("NULL"),
                                         BedProject, Class_BasicOperation._UserName,
                                          (item.Cells["TurnDes"].Text.Trim() == "" ? "" : item.Cells["TurnDes"].Text.Trim()),
                                          item.Cells["Column20"].Value.ToString(),
                                          (item.Cells["Column21"].Text.Trim() == "" ? item.Cells["Column21"].Text.Trim() : item.Cells["Column21"].Value.ToString()),
                                          Convert.ToDouble(item.Cells["Column22"].Value.ToString())
                                          );

                                    //*******INSERT BED and BES IN SANAD***************//
                                    if (item.Cells["Column20"].Value.ToString() == "False")
                                    {
                                        CommandText = CommandText + ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(), Int16.Parse(_Bed_Info[0].ToString()),
                                        _Bed_Info[1].ToString(), _Bed_Info[2].ToString(), _Bed_Info[3].ToString(),
                                        _Bed_Info[4].ToString(), (BedPerson == "" ? "NULL" : BedPerson),
                                        (null), BedProject,
                                            //(mlt_BedProject.Text.Trim() == "" ? null : mlt_BedProject.Value.ToString()),
                                        item.Cells["Sharh"].Text.Trim(),
                                        Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())), 0, 0, 0, -1,
                                        Int16.Parse(mlt_status.Value.ToString()), 0, Class_BasicOperation._UserName, 0,
                                       0, "NULL");
                                    }
                                    else
                                    {
                                        CommandText = CommandText + ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(), Int16.Parse(_Bed_Info[0].ToString()),
                                       _Bed_Info[1].ToString(), _Bed_Info[2].ToString(), _Bed_Info[3].ToString(),
                                       _Bed_Info[4].ToString(), (BedPerson == "" ? "NULL" : BedPerson),
                                       (null),
                                            //(mlt_BedProject.Text.Trim() == "" ? null : mlt_BedProject.Value.ToString()),
                                      BedProject, item.Cells["Sharh"].Text.Trim(),
                                       Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                                       Convert.ToDouble(item.Cells["Column22"].Value.ToString())), 0,
                                       Convert.ToDouble(item.Cells["Column05"].Value.ToString()), 0,
                                       Int16.Parse(item.Cells["Column21"].Value.ToString()),
                                       Int16.Parse(mlt_status.Value.ToString()), _TurnID, Class_BasicOperation._UserName,
                                       Convert.ToDouble(item.Cells["Column22"].Value.ToString()), 0, "NULL");
                                    }

                                    if (item.Cells["Column20"].Value.ToString() == "False")
                                    {
                                        CommandText = CommandText + (ExportDoc_Detail(DocID, Bes, Int16.Parse(_Bes_Info[0].ToString()), _Bes_Info[1].ToString(), _Bes_Info[2].ToString(), _Bes_Info[3].ToString(),
                                            _Bes_Info[4].ToString(), BesPerson, BesCenter, BesProject, item.Cells["Sharh"].Text.Trim(), 0,
                                            Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())), 0, 0, -1,
                                            Int16.Parse(mlt_status.Value.ToString()), _TurnID, Class_BasicOperation._UserName, 0, 0, "NULL"));
                                    }
                                    else
                                    {
                                        CommandText = CommandText + ExportDoc_Detail(DocID, Bes, Int16.Parse(_Bes_Info[0].ToString()), _Bes_Info[1].ToString(), _Bes_Info[2].ToString(), _Bes_Info[3].ToString(),
                                        _Bes_Info[4].ToString(), BesPerson, BesCenter, BesProject, item.Cells["Sharh"].Text.Trim(), 0,
                                        Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                                        Convert.ToDouble(item.Cells["Column22"].Value.ToString())), 0,
                                        Convert.ToDouble(item.Cells["Column05"].Value.ToString()),
                                        Int16.Parse(item.Cells["Column21"].Value.ToString()),
                                        Int16.Parse(mlt_status.Value.ToString()), _TurnID, Class_BasicOperation._UserName,
                                        Convert.ToDouble(item.Cells["Column22"].Value.ToString()), 0, "NULL");
                                    }
                                }
                                    _ShowMsg = true;

                                }
                                if (_ShowMsg)
                                {
                                    using (SqlConnection Con = new SqlConnection(PCLOR.Properties.Settings.Default.PACNT))
                                    {
                                        Con.Open();

                                        SqlTransaction sqlTran = Con.BeginTransaction();
                                        SqlCommand Command = Con.CreateCommand();
                                        Command.Transaction = sqlTran;

                                        try
                                        {
                                            ID = new SqlParameter("ID", SqlDbType.Int);
                                            ID.Direction = ParameterDirection.Output;

                                            Command.CommandText = CommandText;
                                            Command.Parameters.Add(ID);

                                            Command.ExecuteNonQuery();
                                            sqlTran.Commit();

                                            Class_BasicOperation.ShowMsg(" ", "ثبت گردش و صدور سند حسابداری با شماره  " + DocNum.ToString() + " انجام گرفت", Class_BasicOperation.MessageType.Information);


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
                                    mlt_Bed.Value = null;
                                    mlt_Bes.Value = null;
                                    Frm_06_Return_Expense_Load(sender, e);
                                }

                          
                        }
                        if (check.Checked==false)
                        {
                            string[] _Bed_Info = ClDoc.ACC_Info(mlt_Bed.Value.ToString());
                            string Bes = (mlt_Bes.Text.Trim() == "" ? "NULL" : mlt_Bes.Value.ToString());
                            string BesPerson = "NULL";
                            string BesCenter = "NULL";
                            string BesProject = "NULL";
                            string BedProject = "NULL";
                            string BedPerson = ("NULL");

                            foreach (GridEXRow item in gridEX1.GetCheckedRows())
                            {

                                DateTime Date = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(txt_Dat_Recipt.Text)));
                                DateTime DateR = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(item.Cells["Column04"].Value.ToString())));


                                if (Convert.ToInt32(Math.Floor((Date - DateR).TotalDays)) < 0)
                                {
                                    MessageBox.Show("تاریخ سر رسید به تاریخ روز نمی باشد");
                                    return;
                                }
                            
                                BesPerson = (item.Cells["PersonRecipt"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["PersonRecipt"].Value.ToString());
                                BesCenter = "NULL";
                                BesProject = "NULL";
                                BedPerson = "NULL";
                                BedProject = "NULL";
                                BedPerson = "NULL";
                                   
                                if (DocID == 0)
                                 ReturnDocId();
                                  CommandText = "";
                                   



                                    //Bes Account//

                                    //Bes = item.Cells["TurnACC"].Value.ToString();
                                    Bes = mlt_Bes.Value.ToString();
                                    string[] _Bes_Info = ClDoc.ACC_Info(Bes);
                                    //Bes Person
                                    //if (chk_BesPerson.Checked && item.Cells["TurnPerson"].Text.Trim() != "")
                                    //    BesPerson = item.Cells["TurnPerson"].Value.ToString();



                                    //*******First Add Turn to TurnTable***************//
                                    CommandText = CommandText + AddTurnReception(int.Parse(item.Cells["ColumnID"].Value.ToString()), short.Parse(mlt_status.Value.ToString()), faDatePicker1.Text,
                                         BedPerson, (item.Cells["Column08"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                          mlt_Bed.Value.ToString(), Int16.Parse(_Bed_Info[0].ToString()),
                                          (_Bed_Info[1].ToString() == "" ? "NULL" : _Bed_Info[1].ToString()), (_Bed_Info[2].ToString() == string.Empty ? "NULL" : _Bed_Info[2].ToString())
                                          , (_Bed_Info[3].ToString() == "" ? "NULL" : _Bed_Info[3].ToString()), (_Bed_Info[4].ToString() == string.Empty ? "NULL" : _Bed_Info[4].ToString()), DocID, ("NULL" ),
                                         BedProject, Class_BasicOperation._UserName,
                                          (item.Cells["TurnDes"].Text.Trim() == "" ? "" : item.Cells["TurnDes"].Text.Trim()),
                                          item.Cells["Column20"].Value.ToString(),
                                          (item.Cells["Column21"].Text.Trim() == "" ? item.Cells["Column21"].Text.Trim() : item.Cells["Column21"].Value.ToString()),
                                          Convert.ToDouble(item.Cells["Column22"].Value.ToString())
                                          );



                                    //*******INSERT BED and BES IN SANAD***************//
                                    if (item.Cells["Column20"].Value.ToString() == "False")
                                    {
                                        CommandText = CommandText + ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(), Int16.Parse(_Bed_Info[0].ToString()),
                                        _Bed_Info[1].ToString(), _Bed_Info[2].ToString(), _Bed_Info[3].ToString(),
                                        _Bed_Info[4].ToString(), (BedPerson == "" ? "NULL" : BedPerson),
                                        (null ), BedProject,
                                            //(mlt_BedProject.Text.Trim() == "" ? null : mlt_BedProject.Value.ToString()),
                                        item.Cells["Sharh"].Text.Trim(),
                                        Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())), 0, 0, 0, -1,
                                        Int16.Parse(mlt_status.Value.ToString()), 0, Class_BasicOperation._UserName, 0,
                                       0, "NULL");
                                    }
                                    else
                                    {
                                        CommandText = CommandText + ExportDoc_Detail(DocID, mlt_Bed.Value.ToString(), Int16.Parse(_Bed_Info[0].ToString()),
                                       _Bed_Info[1].ToString(), _Bed_Info[2].ToString(), _Bed_Info[3].ToString(),
                                       _Bed_Info[4].ToString(), (BedPerson == "" ? "NULL" : BedPerson),
                                       ( null ),
                                            //(mlt_BedProject.Text.Trim() == "" ? null : mlt_BedProject.Value.ToString()),
                                      BedProject, item.Cells["Sharh"].Text.Trim(),
                                       Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                                       Convert.ToDouble(item.Cells["Column22"].Value.ToString())), 0,
                                       Convert.ToDouble(item.Cells["Column05"].Value.ToString()), 0,
                                       Int16.Parse(item.Cells["Column21"].Value.ToString()),
                                       Int16.Parse(mlt_status.Value.ToString()), _TurnID, Class_BasicOperation._UserName,
                                       Convert.ToDouble(item.Cells["Column22"].Value.ToString()), 0, "NULL");
                                    }




                                    if (item.Cells["Column20"].Value.ToString() == "False")
                                    {
                                        CommandText = CommandText + (ExportDoc_Detail(DocID, Bes, Int16.Parse(_Bes_Info[0].ToString()), _Bes_Info[1].ToString(), _Bes_Info[2].ToString(), _Bes_Info[3].ToString(),
                                            _Bes_Info[4].ToString(), BesPerson, BesCenter, BesProject, item.Cells["Sharh"].Text.Trim(), 0,
                                            Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())), 0, 0, -1,
                                            Int16.Parse(mlt_status.Value.ToString()), _TurnID, Class_BasicOperation._UserName, 0, 0, "NULL"));
                                    }
                                    else
                                    {
                                        CommandText = CommandText + ExportDoc_Detail(DocID, Bes, Int16.Parse(_Bes_Info[0].ToString()), _Bes_Info[1].ToString(), _Bes_Info[2].ToString(), _Bes_Info[3].ToString(),
                                        _Bes_Info[4].ToString(), BesPerson, BesCenter, BesProject, item.Cells["Sharh"].Text.Trim(), 0,
                                        Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                                        Convert.ToDouble(item.Cells["Column22"].Value.ToString())), 0,
                                        Convert.ToDouble(item.Cells["Column05"].Value.ToString()),
                                        Int16.Parse(item.Cells["Column21"].Value.ToString()),
                                        Int16.Parse(mlt_status.Value.ToString()), _TurnID, Class_BasicOperation._UserName,
                                        Convert.ToDouble(item.Cells["Column22"].Value.ToString()), 0, "NULL");
                                    }

                                    _ShowMsg = true;
                                    //}
                                    //else Class_BasicOperation.ShowMsg("", "برای برگه شماره " + item.Cells["ChqId"].Value.ToString() + " گردش ثبت شده است", Class_BasicOperation.MessageType.Warning);
                                }
                                if (_ShowMsg)
                                {
                                    using (SqlConnection Con = new SqlConnection(PCLOR.Properties.Settings.Default.PACNT))
                                    {
                                        Con.Open();

                                        SqlTransaction sqlTran = Con.BeginTransaction();
                                        SqlCommand Command = Con.CreateCommand();
                                        Command.Transaction = sqlTran;

                                        try
                                        {
                                            ID = new SqlParameter("ID", SqlDbType.Int);
                                            ID.Direction = ParameterDirection.Output;

                                            Command.CommandText = CommandText;
                                            Command.Parameters.Add(ID);

                                            Command.ExecuteNonQuery();
                                            sqlTran.Commit();

                                            Class_BasicOperation.ShowMsg(" ", "ثبت گردش و صدور سند حسابداری با شماره  " + DocNum.ToString() + " انجام گرفت", Class_BasicOperation.MessageType.Information);


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

                    
                                    mlt_Bed.Value = null;
                                    mlt_Bes.Value = null;
                                    Frm_06_Return_Expense_Load(sender, e);
                                  
                                }

                            }
                        }
                    }
            }

            catch (Exception ex)
            {
                WarningException es = new WarningException();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

                //if (ex.GetBaseException().GetType() != es.GetBaseException().GetType())
                //{
                //    bt_Display_Click(sender, e);
                //}
            }
            //}
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



        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {

                label2.Text = gridEX1.GetValue("ErrorDes").ToString();
            }
            catch
            {
            }
        }
        private void CheckEssentialItems()
        {
            uiPanel0.Text = "";
            //*****Check Essential Information***//
            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Trim() == "" || faDatePicker1.Text.Trim().Length > 10 || mlt_Bed.Text.Trim() == "" || (mlt_Bes.Text.Trim() == ""))
                throw new WarningException("اطلاعات مربوط به گردش و تنظیمات سند را کامل کنید");

            if (mlt_Bed.Text.Trim() != "")
                if (mlt_Bed.Text.Trim().All(char.IsDigit))
                    throw new WarningException("سرفصل بدهکار نامعتبر است");

            if ((mlt_Bes.Text.Trim() != ""))
                if (mlt_Bes.Text.Trim().All(char.IsDigit))
                    throw new WarningException("سرفصل بستانکار نامعتبر است");




            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            ClDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            ClDoc.CheckExistFinalDoc();





            //Define Table to check person's credit//
            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));
            //Define Table to check account's nature//
            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {

                item.BeginEdit();
                item.Cells["Error"].Value = false;
                item.Cells["ErrorDes"].Value = DBNull.Value;
                item.EndEdit();
            }

            foreach (GridEXRow item in gridEX1.GetCheckedRows())
            {
                //مقداردهی سرفصل،پروژه و مرکز هزینه بدهکار
                Int16? BesCenter = null;
                Int16? BesProject = null;
                int? BesPerson = null;

                //سرفصل بستانکار
                string Bes = (mlt_Bes.Text.Trim() == "" ? "NULL" : mlt_Bes.Value.ToString());




                //شخص بستانکار
                //if (item.Cells["TurnPerson"].Text.Trim() != "")
                //    BesPerson = int.Parse(item.Cells["TurnPerson"].Value.ToString());


                Int16? BedCenter = null;
                Int16? BedProject = null;
                int? BedPerson = null;
                string Bed = null;

                //سرفصل بدهکار
                Bed = mlt_Bed.Value.ToString();
                //شخص بدهکار
                if (item.Cells["Column07"].ToString() != "")
                    BedPerson = int.Parse(gridEX1.GetValue("Column07").ToString());

                try
                {
                    clCredit.All_Controls_2(Bed, BedPerson, BedCenter, BedProject);
                }
                catch (Exception es)
                {
                    item.BeginEdit();
                    item.Cells["Error"].Value = true;
                    item.Cells["ErrorDes"].Value = es.Message;
                    item.EndEdit();
                }

                try
                {
                    clCredit.All_Controls_2(Bes, BesPerson, BesCenter, BesProject);
                }
                catch (Exception es)
                {
                    item.BeginEdit();
                    item.Cells["Error"].Value = true;
                    item.Cells["ErrorDes"].Value = es.Message;
                    item.EndEdit();
                }

                //**********Check Account's nature****//
                //TAccounts.Rows.Add(Bed, Convert.ToDouble(item.Cells["Column05"].Value.ToString()));
                //TAccounts.Rows.Add(Bes, Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * -1);
                //if (BesPerson != null)
                //    TPerson.Rows.Add(BesPerson, Bes, Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * -1);
                //if (BedPerson != null)
                //    TPerson.Rows.Add(BedPerson, Bed, Convert.ToDouble(item.Cells["Column05"].Value.ToString()));
            }

            //clCredit.CheckAccountCredit(TAccounts, 0);
            //clCredit.CheckPersonCredit(TPerson, 0);
            gridEX1.UpdateData();
        }





        private void txt_To_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_To.Text.Trim() != "")
                {
                    ClDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                    faDatePicker1.Text = ClDoc.DocDate(int.Parse(txt_To.Text.Trim()));
                    txt_Cover.Text = ClDoc.Cover(int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Frm_06_Return_Expense_Load(sender, e);
        }

        private void gridEX1_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
        {
            try
            {

                if (e.ChangeType == Janus.Windows.GridEX.CheckStateChangeType.RowChange)
                {
                    if (e.CheckState == Janus.Windows.GridEX.RowCheckState.Checked)
                    {
                        if (mlt_status.Text == "" || mlt_status.Text == "0")
                        {
                            MessageBox.Show("لطفا وضعیت را انتخاب کنید");
                            return;
                        } 

                        string Persons = ClDoc.ExScalarNULL(ConBank.ConnectionString, "Table_035_ReceiptCheques",
                          "Column07", "ColumnId", gridEX1.GetValue("ColumnId").ToString());

                      
                            gridEX1.SetValue("Sharh",
                            ClDoc.ShablonDescGenerate(ConBase.ConnectionString,
                            ClDoc.GetTypeCheckStatusShablon(mlt_status.Value.ToString(), ConBank),
                            mlt_status.Text,
                            e.Row.Cells["Column02"].Text.Trim(), e.Row.Cells["Column04"].Text.Trim(),
                            e.Row.Cells["Column03"].Text.Trim(),
                            e.Row.Cells["TurnBankBox"].Text.Trim(),//e.Row.Cells["MainBankName"].Text,
                           "",
                            "",
                                ///چون رشته شخص خالی است پس شخص مقابل اینجا خالی می شود
                                (Persons == "" ? "" : ClDoc.ExScalar(ConBase.ConnectionString, "Table_045_PersonInfo", "Column02", "ColumnId", Persons)),
                             e.Row.Cells["Column06"].Text.Trim(),
                             e.Row.Cells["TurnACC"].Text.Trim()
                            , e.Row.Cells["MainBankName"].Text)

                           );

                      
                      

                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 105))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Frm_08_ViewReceivedChq")
                        {
                            item.BringToFront();
                            ((_03_Bank.Frm_08_ViewReceivedChq)item).table_035_ReceiptChequesBindingSource.Position =
                                ((_03_Bank.Frm_08_ViewReceivedChq)item).table_035_ReceiptChequesBindingSource.Find("ColumnId", gridEX1.GetValue("ColumnId").ToString());
                            return;
                        }
                    }
                    _03_Bank.Frm_08_ViewReceivedChq frm = new Frm_08_ViewReceivedChq(int.Parse(gridEX1.GetValue("ColumnId").ToString()),
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 40));
                    try { frm.MdiParent = Frm_Main.ActiveForm; }
                    catch { }
                    frm.Show();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch
            {
            }
        }

        private void gridEX1_DoubleClick(object sender, EventArgs e)
        {
            btn_Delete_Click(sender, e);
        }

        private void mlt_status_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable Bes = ClDoc.ReturnTable(ConBank, @"select Column09 from Table_060_ChequeStatus where Columnid=" + mlt_status.Value + "");
                mlt_Bes.Value = Bes.Rows[0][0].ToString();
                mlt_Bes.DataSource = ClDoc.ReturnTable(ConACNT, "select  * from AllHeaders() where ACC_Code=" + Bes.Rows[0][0].ToString() + "");

                DataTable Bed = ClDoc.ReturnTable(ConBank, @"select Column03 from Table_060_ChequeStatus where Columnid=" + mlt_status.Value + "");
                mlt_Bed.Value = Bed.Rows[0][0].ToString();

                mlt_Bed.DataSource = ClDoc.ReturnTable(ConACNT, "select  * from AllHeaders() where ACC_Code=" + Bed.Rows[0][0].ToString() + "");
            }
            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex, this.Name); };
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
