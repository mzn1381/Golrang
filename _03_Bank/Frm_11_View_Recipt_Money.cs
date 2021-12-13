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
    public partial class Frm_11_View_Recipt_Money : Form
    {
        int _PaperID = 0;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlDataAdapter PeopleAdapter, ProjectAdapter, HeadersAdapter, BanksAdapter, RecAdapter;
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        public Frm_11_View_Recipt_Money(int PaperId)
        {
            InitializeComponent();
            _PaperID = PaperId;
        }

        private void Frm_11_View_Recipt_Money_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);


            PeopleAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from ListPeopleInActive(3)", ConBase);
            PeopleAdapter.Fill(dataSet1, "Person");
            gridEX1.DropDowns["Person"].SetDataBinding(dataSet1.Tables["Person"], "");
            gridEX2.DropDowns["Person"].SetDataBinding(dataSet1.Tables["Person"], "");

            ProjectAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            ProjectAdapter.Fill(dataSet1, "Projects");
            gridEX1.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Projects"], "");
            gridEX2.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Projects"], "");

            HeadersAdapter = new SqlDataAdapter("Select ACC_Code,ACC_Name from AllHeaders()", ConAcnt);
            HeadersAdapter.Fill(dataSet1, "Headers");
            gridEX1.DropDowns["Headers"].SetDataBinding(dataSet1.Tables["Headers"], "");
            gridEX2.DropDowns["Headers"].SetDataBinding(dataSet1.Tables["Headers"], "");

            BanksAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo", ConBank);
            BanksAdapter.Fill(dataSet1, "Banks1");
            BanksAdapter.Fill(dataSet1, "Banks2");
            gridEX1.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
            gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");
            gridEX2.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
            gridEX2.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX2.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            DataTable DocTable = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");
            gridEX1.DropDowns["Doc"].SetDataBinding(DocTable, "");
            gridEX2.DropDowns["Doc"].SetDataBinding(DocTable, "");
            if (isadmin)
            {
                RecAdapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT ColumnId, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, 
            Column13, Column14, Column15, Column16, Column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24,CAST(CASE WHEN Table_045_ReceiveCash.Column22 = 1 THEN Table_045_ReceiveCash.Column24 * Table_045_ReceiveCash.Column03 END AS Decimal(18, 0)) 
            AS Riali
            FROM         dbo.Table_045_ReceiveCash
            ORDER BY ColumnId", ConBank);
            }
            else
            {
                RecAdapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT ColumnId, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, 
            Column13, Column14, Column15, Column16, Column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24,CAST(CASE WHEN Table_045_ReceiveCash.Column22 = 1 THEN Table_045_ReceiveCash.Column24 * Table_045_ReceiveCash.Column03 END AS Decimal(18, 0)) 
            AS Riali
            FROM         dbo.Table_045_ReceiveCash  where Column17='" + Class_BasicOperation._UserName + "' ORDER BY ColumnId", ConBank);
            }
            RecAdapter.SelectCommand.CommandText = RecAdapter.SelectCommand.CommandText.Replace("BaseDB", ConAcnt.Database);
            RecAdapter.Fill(dataSet1, "RecTable");
            this.table_045_ReceiveCashBindingSource.DataSource = dataSet1.Tables["RecTable"];
            if (_PaperID != 0)
            {
                this.table_045_ReceiveCashBindingSource.Position = this.table_045_ReceiveCashBindingSource.Find("ColumnId", _PaperID);
            }

            else
            {
                this.table_045_ReceiveCashBindingSource.MoveLast();
            }


        }

        private void Frm_11_View_Recipt_Money_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX2.RemoveFilters();
        }

        private void gridEX2_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;

        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 104))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_09_Recipt_Money")
                    {
                        item.BringToFront();
                        _03_Bank.Frm_09_Recipt_Money frms = (_03_Bank.Frm_09_Recipt_Money)item;
                        frms.txt_Search.Text = ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                        frms.btn_Search_Click(sender, e);
                        return;

                    }
                }
                Frm_09_Recipt_Money frm = new Frm_09_Recipt_Money(
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 17),
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 18),
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 19),
                    int.Parse(((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                try { frm.MdiParent = Frm_Main.ActiveForm; }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void gridEX2_DoubleClick(object sender, EventArgs e)
        {
          
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = this.table_045_ReceiveCashBindingSource.Position;
                dataSet1.Tables["RecTable"].Clear();
                RecAdapter.Fill(dataSet1, "RecTable");
                this.table_045_ReceiveCashBindingSource.Position = pos;
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

                DataTable Table = dataSet_01_Cash.Rpt_PrintPayCash.Clone();
                Janus.Windows.GridEX.GridEXRow Row = gridEX1.GetRow();
                Table.Rows.Add(Row.Cells["Column02"].Value.ToString(),
                    Row.Cells["ColumnId"].Value.ToString(),
                    (Row.Cells["Column22"].Value.ToString() == "False" ? Row.Cells["Column03"].Text.ToString() : Row.Cells["Riali"].Text.ToString()),
                        FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble
                        ((Row.Cells["Column22"].Value.ToString() == "False" ? Row.Cells["Column03"].Text.ToString() : Row.Cells["Riali"].Text.ToString())))),
                    Row.Cells["Column04"].Text.ToString(),
                    Row.Cells["Column01"].Text,
                     (Row.Cells["Column07"].Text.Trim() == "" ? Row.Cells["Column11"].Text :
                     Row.Cells["Column07"].Text.Trim()) + "/" + Row.Cells["Column05"].Text);


                PACNT._2_Cash_Operation.Reports.Form1_PrintReceiptCash frm = new PACNT._2_Cash_Operation.Reports.Form1_PrintReceiptCash(Table);
                frm.ShowDialog();

            }
            catch { }
        }

        private void gridEX2_RowDoubleClick(object sender, RowActionEventArgs e)
        {
            bt_Edit_Click(sender, e);
        }
    }
}
