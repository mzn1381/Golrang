using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.GridEX;
using System.Data.SqlClient;


namespace PCLOR._03_Bank
{
    public partial class Frm_12_View_PayCash : Form
    {
        int _PaperNumber = 0;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
      
        SqlDataAdapter PeopleAdapter, ProjectAdapter, HeadersAdapter, BanksAdapter, CenterAdapters, RequestAdapter, CashAdapter;
       
        public Frm_12_View_PayCash(int PaperId)
        {
            InitializeComponent();
            _PaperNumber = PaperId;

        }

        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
        }

        private void Frm_12_View_PayCash_Load(object sender, EventArgs e)
        {
           
            gridEX2.RemoveFilters();

            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = SystemColors.Window;
                if (col.Key == "Column20" || col.Key == "Column22")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column21" || col.Key == "Column23")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }

            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);


            PeopleAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from ListPeopleInActive(3)", ConBase);
            PeopleAdapter.Fill(dataSet1, "Person");
            PeopleAdapter.Fill(dataSet1, "Cashier");
            gridEX1.DropDowns["Person"].SetDataBinding(dataSet1.Tables["Person"], "");
            gridEX1.DropDowns["Cashier"].SetDataBinding(dataSet1.Tables["Cashier"], "");
            gridEX2.DropDowns["Person"].SetDataBinding(dataSet1.Tables["Person"], "");
            gridEX2.DropDowns["Cashier"].SetDataBinding(dataSet1.Tables["Cashier"], "");

            ProjectAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            ProjectAdapter.Fill(dataSet1, "Projects");
            gridEX1.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Projects"], "");
            gridEX2.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Projects"], "");

            CenterAdapters = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            CenterAdapters.Fill(dataSet1, "Centers");
            gridEX1.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Centers"], "");
            gridEX2.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Centers"], "");

            HeadersAdapter = new SqlDataAdapter("Select ACC_Code,ACC_Name from AllHeaders()", ConAcnt);
            HeadersAdapter.Fill(dataSet1, "Headers");
            gridEX1.DropDowns["Headers"].SetDataBinding(dataSet1.Tables["Headers"], "");
            gridEX2.DropDowns["Headers"].SetDataBinding(dataSet1.Tables["Headers"], "");

            BanksAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo", ConBank);
            BanksAdapter.Fill(dataSet1, "Banks1");
            BanksAdapter.Fill(dataSet1, "Banks2");
            gridEX1.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
            gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");
            gridEX2.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
            gridEX2.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");

            DataTable Currencytable = clDoc.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(Currencytable, "");
            gridEX2.DropDowns["Currency"].SetDataBinding(Currencytable, "");

            DataTable DocTable = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");
            gridEX1.DropDowns["Doc"].SetDataBinding(DocTable, "");
            gridEX2.DropDowns["Doc"].SetDataBinding(DocTable, "");


            RequestAdapter = new SqlDataAdapter("Select Table_050_PaymentRequests.*,Table_020_BankCashAccInfo.Column02 as BoxName from Table_050_PaymentRequests INNER JOIN Table_020_BankCashAccInfo On Table_020_BankCashAccInfo.ColumnId=Table_050_PaymentRequests.Column06  where Table_050_PaymentRequests.Column11=1 and Table_050_PaymentRequests.Column04=1 and Table_050_PaymentRequests.Column13=0", ConBank);
            RequestAdapter.Fill(dataSet1, "Req");
            gridEX1.DropDowns["RequestList"].SetDataBinding(dataSet1.Tables["Req"], "");
            if (isadmin)
            {
                CashAdapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT ColumnId, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, 
            Column13, Column14, Column15, Column16, Column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26, 
            Column27,
            CAST(CASE WHEN Table_040_CashPayments.Column25 = 1 THEN Table_040_CashPayments.Column27 * Table_040_CashPayments.Column04 END AS Decimal(18, 
            0)) AS Riali
            FROM         dbo.Table_040_CashPayments
            ORDER BY Column01", ConBank);
            }
            else
            {
                CashAdapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT ColumnId, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, 
            Column13, Column14, Column15, Column16, Column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26, 
            Column27,
            CAST(CASE WHEN Table_040_CashPayments.Column25 = 1 THEN Table_040_CashPayments.Column27 * Table_040_CashPayments.Column04 END AS Decimal(18, 
            0)) AS Riali
            FROM         dbo.Table_040_CashPayments where Column22='" + Class_BasicOperation._UserName + "'  ORDER BY Column01", ConBank);
            }
            this.CashAdapter.Fill(dataSet1, "Pay");
            this.table_040_CashPaymentsBindingSource.DataSource = dataSet1.Tables["Pay"];
            if (_PaperNumber != 0)
            {
                this.table_040_CashPaymentsBindingSource.Position = this.table_040_CashPaymentsBindingSource.Find("ColumnId", _PaperNumber);
            }

            else
            {
                this.table_040_CashPaymentsBindingSource.MoveLast();
            }
            this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);
        }

        private void table_040_CashPaymentsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_040_CashPaymentsBindingSource.Count > 0)
            {
                try
                {
                    tabItem1.Text = "شماره برگه " + ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString();
                }
                catch
                {

                }
            }
            else
                tabItem1.Text = "شماره برگه ";
        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 87))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_10_PayCash")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString();
                        Frm_10_PayCash frms = (Frm_10_PayCash)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                    Frm_10_PayCash frm = new Frm_10_PayCash(
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 22),
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 23),
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 24),
                    int.Parse(((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                try { frm.MdiParent = Frm_Main.ActiveForm; }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = this.table_040_CashPaymentsBindingSource.Position;
                dataSet1.Tables["Pay"].Clear();
                this.CashAdapter.Fill(dataSet1, "Pay");
                this.table_040_CashPaymentsBindingSource.Position = pos;
            }
            catch { }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
                Janus.Windows.GridEX.GridEXRow Row = gridEX1.GetRow();
                DataTable Table = dataSet_01_Cash.Rpt_PrintPayCash.Clone();
                Table.Rows.Add(Row.Cells["Column03"].Value.ToString(),
                Row.Cells["Column01"].Value.ToString(),
                (Row.Cells["Column25"].Value.ToString() == "False" ? Row.Cells["Column04"].Text.ToString() : Row.Cells["Riali"].Text.ToString()),
                FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble
                ((Row.Cells["Column25"].Value.ToString() == "False" ? Row.Cells["Column04"].Text.ToString() : Row.Cells["Riali"].Text.ToString())))),
                Row.Cells["Column05"].Text.ToString(),
                Row.Cells["Column02"].Text,
                Row.Cells["Column06"].Text + (Row.Cells["Column14"].Text.Trim() != "" ? "/" +
                Row.Cells["Column14"].Text : ""));
                PACNT._2_Cash_Operation.Reports.Form2_PrintPaidCash frm = new PACNT._2_Cash_Operation.Reports.Form2_PrintPaidCash(Table);
                frm.ShowDialog();
                gridEX1.UnCheckAllRecords();
            }
            catch { }
        }

        private void gridEX2_RowDoubleClick(object sender, RowActionEventArgs e)
        {
            bt_Edit_Click(sender, e);
        }
    }
}
