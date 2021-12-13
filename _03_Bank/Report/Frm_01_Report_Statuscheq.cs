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
    public partial class Frm_01_Report_Statuscheq : Form
    {

        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        SqlDataAdapter GetAdapter;
        bool _BackSpace = false;
       Classes. Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1, Date2; 

        public Frm_01_Report_Statuscheq()
        {
            InitializeComponent();
        }

        private void uiPanel0Container_Click(object sender, EventArgs e)
        {

        }

        private void Frm_01_Report_Statuscheq_Load(object sender, EventArgs e)
        {

            string[] Dates = Properties.Settings.Default.Bank_Rec_Report.Split('-');
            faDatePicker1.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePicker2.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
           
            
            if (isadmin)
            {
                mlt_BankBox.DropDownDataSource = clDoc.ReturnTable(ConBank, @"select * from Table_020_BankCashAccInfo where Column01=1");

            }
            else
            {
                mlt_BankBox.DropDownDataSource = clDoc.ReturnTable(ConBank, @"select * from Table_020_BankCashAccInfo where Column01=1 and Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "') ");

            }
            mlt_Status.DropDownDataSource = clDoc.ReturnTable(ConBank, @"select ColumnID,Column01,Column02 from Table_060_ChequeStatus");


            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo  ", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            gridEX_Get.DropDowns["Person"].SetDataBinding(Person, "");
            gridEX_Get.DropDowns["Header"].SetDataBinding(clDoc.ReturnTable(ConAcnt, "Select ACC_Code,ACC_Name from Allheaders()"), "");
            Adapter.SelectCommand.CommandText = "Select Column00,Column01,Column02 from Table_035_ProjectInfo";
            DataTable Project = new DataTable();
            Adapter.Fill(Project);
            gridEX_Get.DropDowns["Project"].SetDataBinding(Project, "");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01 from Table_060_ProvinceInfo";
            DataTable Province = new DataTable();
            Adapter.Fill(Province);
            gridEX_Get.DropDowns["Province"].SetDataBinding(Province, "");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01,Column02 from Table_065_CityInfo";
            DataTable City = new DataTable();
            Adapter.Fill(City);
            gridEX_Get.DropDowns["City"].SetDataBinding(City, "");

            Adapter.SelectCommand.Connection = ConBank;
            Adapter.SelectCommand.CommandText = "Select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo";
            DataTable BoxBankTable = new DataTable();
            DataTable BoxBankTable2 = new DataTable();
            Adapter.Fill(BoxBankTable);
            Adapter.Fill(BoxBankTable2);
            gridEX_Get.DropDowns["ToBank"].SetDataBinding(BoxBankTable, "");
            BoxBankTable2.Rows.Add(0, 0, "همه صندوقها و حسابها");

            Adapter.SelectCommand.CommandText = "Select ColumnId,Column02 from Table_060_ChequeStatus where Column01=0";
            DataTable StatusTable = new DataTable();
            DataTable StatusTable2 = new DataTable();
            Adapter.Fill(StatusTable);
            Adapter.Fill(StatusTable2);
            gridEX_Get.DropDowns["Status"].SetDataBinding(StatusTable, "");
            StatusTable2.Rows.Add(0, "همه وضعیتها");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01 from Table_010_BankNames";
            DataTable Bank = new DataTable();
            Adapter.Fill(Bank);
            gridEX_Get.DropDowns["Bank"].SetDataBinding(Bank, "");

            Adapter.SelectCommand.CommandText = "Select Column00,ColumnId from Table_060_SanadHead";
            Adapter.SelectCommand.Connection = ConAcnt;
            DataTable DocTable = new DataTable();
            Adapter.Fill(DocTable);
            gridEX_Get.DropDowns["Doc"].SetDataBinding(DocTable, "");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue)
            {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                Date1 = null; Date2 = null;
                Date1 = faDatePicker1.Text;
                Date2 = faDatePicker2.Text;


                if (mlt_Status.DropDownList.GetCheckedRows().Count() == 0)
                {
                    MessageBox.Show("وضعیت را انتخاب کنید");
                    return;
                }
                if (mlt_BankBox.DropDownList.GetCheckedRows().Count() == 0)
                {
                    MessageBox.Show("صندوق را انتخاب کنید");
                    return;
                }

                if (Date1=="" || Date2=="")
                {
                    MessageBox.Show("تاریخ مورد نظر را وارد کنید");
                    return;
                }
                Int16 Status;
                Status = Int16.Parse(mlt_Status.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString());
                Int16 BankBox;
                BankBox = Int16.Parse(mlt_BankBox.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString());
               
                if (Status>0 && BankBox>0)
                {
                    try
                    {
                    string Statu = string.Empty;
                    string Bank = string.Empty;

                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Status.DropDownList.GetCheckedRows())
                    {
                        if (dr.Cells["ColumnId"].Value.ToString() != "0")
                            Statu += dr.Cells["ColumnId"].Value + ",";
                    }
                    Statu = Statu.TrimEnd(',');

                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_BankBox.DropDownList.GetCheckedRows())
                    {
                        if (dr.Cells["ColumnId"].Value.ToString() != "0")
                            Bank += dr.Cells["ColumnId"].Value + ",";
                    }
                    Bank = Bank.TrimEnd(',');

                    //DataTable dtReciptch = new DataTable();
                    if (isadmin)
                    {


                        if (rdb_Get.Checked)
                        {
                            gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
                         FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
                         WHERE        (Rec_LastTurn_1.Column02 IN (" + Statu + @")) AND (dbo.Table_035_ReceiptCheques.Column01 IN (" + Bank + @")) AND 
                         (dbo.Table_035_ReceiptCheques.Column02 >=  N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column02 <=  N'" + Date2 + @"')");

                        }
                        else
                        {
                            gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
WHERE        (Rec_LastTurn_1.Column02 IN (" + Statu + @")) AND (dbo.Table_035_ReceiptCheques.Column01 IN (" + Bank + @")) AND 
                         (dbo.Table_035_ReceiptCheques.Column04 >= N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column04 <= N'" + Date2 + @"')");
                        }
                    }
                    else
                    {

                        if (rdb_Get.Checked)
                        {
                            gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
                         FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
                         WHERE        (Rec_LastTurn_1.Column02 IN (" + Statu + @")) AND (dbo.Table_035_ReceiptCheques.Column01 IN (" + Bank + @")) AND 
                         (dbo.Table_035_ReceiptCheques.Column02 >=  N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column02 <=  N'" + Date2 + @"')AND 
                         (dbo.Table_035_ReceiptCheques.Column42 = '" + Class_BasicOperation._UserName + "')");

                        }
                        else
                        {
                            gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
WHERE        (Rec_LastTurn_1.Column02 IN (" + Statu + @")) AND (dbo.Table_035_ReceiptCheques.Column01 IN (" + Bank + @")) AND 
                         (dbo.Table_035_ReceiptCheques.Column04 >= N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column04 <= N'" + Date2 + @"')AND 
                         (dbo.Table_035_ReceiptCheques.Column42 = '" + Class_BasicOperation._UserName + "')");
                        }



                    }
                    }
                    catch (Exception ex)
                    {

                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
               
                }
                else
                {
                    try
                    {
                    string Statu1 = string.Empty;
                    string Bank1 = string.Empty;
                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Status.DropDownList.GetCheckedRows())
                    {
                        
                            Statu1 += dr.Cells["ColumnId"].Value + ",";
                    }
                    Statu1 = Statu1.TrimEnd(',');

                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_BankBox.DropDownList.GetCheckedRows())
                    {
                       
                            Bank1 += dr.Cells["ColumnId"].Value + ",";
                    }
                    Bank1 = Bank1.TrimEnd(',');
                    if (isadmin)
                    {
                    if (rdb_Get.Checked)
                    {
                        gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
                         FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
                         WHERE       

                         (dbo.Table_035_ReceiptCheques.Column02 >=  N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column02 <=  N'" + Date2 + @"')");

                    }
                    else
                    {
                        gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
                         FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
                         WHERE        
                         (dbo.Table_035_ReceiptCheques.Column04 >= N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column04 <= N'" + Date2 + @"')");
                    }
                    }

                    else
                    {
                        if (rdb_Get.Checked)
                        {
                            gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
                         FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
                         WHERE       

                         (dbo.Table_035_ReceiptCheques.Column02 >=  N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column02 <=  N'" + Date2 + @"')AND 
                         (dbo.Table_035_ReceiptCheques.Column42 = '" + Class_BasicOperation._UserName + "')");

                        }
                        else
                        {
                            gridEX_Get.DataSource = clDoc.ReturnTable(ConBank, @"SELECT        dbo.Table_035_ReceiptCheques.ColumnId, dbo.Table_035_ReceiptCheques.Column00, dbo.Table_035_ReceiptCheques.Column01, 
                         dbo.Table_035_ReceiptCheques.Column02, dbo.Table_035_ReceiptCheques.Column03, dbo.Table_035_ReceiptCheques.Column04, 
                         dbo.Table_035_ReceiptCheques.Column05, dbo.Table_035_ReceiptCheques.Column06, dbo.Table_035_ReceiptCheques.Column07, 
                         dbo.Table_035_ReceiptCheques.Column08, dbo.Table_035_ReceiptCheques.Column09, dbo.Table_035_ReceiptCheques.Column10, 
                         dbo.Table_035_ReceiptCheques.Column11, dbo.Table_035_ReceiptCheques.Column12, dbo.Table_035_ReceiptCheques.Column15, 
                         dbo.Table_035_ReceiptCheques.Column41, dbo.Table_035_ReceiptCheques.Column42, dbo.Table_035_ReceiptCheques.Column43, 
                         dbo.Table_035_ReceiptCheques.Column44, dbo.Table_035_ReceiptCheques.Column45, dbo.Table_035_ReceiptCheques.Column46, 
                         dbo.Table_035_ReceiptCheques.Column48, Rec_LastTurn_1.Column02 AS LastTurn, CASE WHEN Rec_LastTurn_1.Column02 IS NULL 
                         THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column05 AS TurnPerson, Rec_LastTurn_1.Column04 AS TurnDate, 
                         Rec_LastTurn_1.Column07 AS TurnAcc, Rec_LastTurn_1.Column06 AS TurnBank, Rec_LastTurn_1.Column13, dbo.Table_035_ReceiptCheques.Column51
                         FROM            dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                         dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01
                         WHERE        
                         (dbo.Table_035_ReceiptCheques.Column04 >= N'" + Date1 + @"' AND dbo.Table_035_ReceiptCheques.Column04 <= N'" + Date2 + @"')AND 
                         (dbo.Table_035_ReceiptCheques.Column42 = '" + Class_BasicOperation._UserName + "')");
                        }
                    }




                    }
                    catch (Exception ex)
                    {

                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }






                }

            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePicker)sender).HideDropDown();
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

        private void faDatePicker2_TextChanged(object sender, EventArgs e)
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

        private void faDatePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePicker)sender).HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void gridEXFieldChooserControl1_Click(object sender, EventArgs e)
        {

        }

        private void mlt_Status_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void mlt_BankBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }


    }
}
