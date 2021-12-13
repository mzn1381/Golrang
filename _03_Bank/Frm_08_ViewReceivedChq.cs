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
    public partial class Frm_08_ViewReceivedChq : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        DataTable dt = new DataTable();
        int _ChqID = 0;
        bool _Del = false, _DelDoc = false, _New = false;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

        public Frm_08_ViewReceivedChq(int ChqID, bool Del)
        {
            InitializeComponent();
            _ChqID = ChqID;
            _Del = Del;
        }

        private void uiPanel1Container_Click(object sender, EventArgs e)
        {

        }

        private void uiPanelGroup2_SelectedPanelChanged(object sender, Janus.Windows.UI.Dock.PanelActionEventArgs e)
        {

        }

        public void Frm_08_ViewReceivedChq_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            try
            {

                gridEX1.DropDowns["Status"].DataSource = gridEX2.DropDowns["Status"].DataSource = gridEX_Turn.DropDowns["Status"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_060_ChequeStatus");
                gridEX_Turn.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo");
                gridEX_Turn.DropDowns["Bank"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");
                gridEX_Turn.DropDowns["Doc"].DataSource = ClDoc.ReturnTable(ConACNT, @"Select ColumnId,Column00 from Table_060_SanadHead");
                gridEX_Turn.DropDowns["Header"].DataSource = ClDoc.ReturnTable(ConACNT, @"select  * from AllHeaders() ");
                gridEX_Turn.DropDowns["Doc"].DataSource = ClDoc.ReturnTable(ConACNT, @"Select ColumnId,Column00 from Table_060_SanadHead");

                gridEX1.DropDowns["Person"].DataSource = gridEX2.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["ToBank"].DataSource = gridEX2.DropDowns["ToBank"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");
                gridEX1.DropDowns["Banks"].DataSource = gridEX2.DropDowns["Banks"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");


                //mlt_fund.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo ");
                //mlt_Bank.DataSource = ClDoc.ReturnTable(ConBank, @"select Column00,Column01 from Table_010_BankNames ");
                //mlt_Person_pay.DataSource = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                //                                                  'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
                ////if (table_035_ReceiptChequesBindingSource.Count>0 && table_065_TurnReceptionBindingSource.Count>0)
                //{


                if (isadmin)
                {
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.Fill(this.dataSet_01_Cash.Table_035_ReceiptCheques);
                    this.table_065_TurnReceptionTableAdapter.Fill(dataSet_01_Cash.Table_065_TurnReception);
                    dataSet_01_Cash.EnforceConstraints = true;
                }
                else
                {
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillByUser(this.dataSet_01_Cash.Table_035_ReceiptCheques, Class_BasicOperation._UserName);
                    this.table_065_TurnReceptionTableAdapter.Fill(dataSet_01_Cash.Table_065_TurnReception);
                    dataSet_01_Cash.EnforceConstraints = true;
                }


                if (_ChqID != 0)
                {
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, _ChqID);
                    this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet_01_Cash.Table_065_TurnReception, _ChqID);
                    dataSet_01_Cash.EnforceConstraints = true;

                    if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                    {
                        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);

                    }

                }
            }
            catch { }
            //}
            
        }

        private void bt_DelTurn_Click(object sender, EventArgs e)
        {
            try
            {
                 if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 109))
                {
                if (this.table_065_TurnReceptionBindingSource.Count > 0 && gridEX_Turn.GetCheckedRows().Length > 0)
                {
                    if (_Del)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف گردش چک هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Turn.GetCheckedRows())
                            {
                                int _TurnID = int.Parse(item.Cells["ColumnId"].Value.ToString());
                                Int16 _StatusId = Int16.Parse(item.Cells["Column02"].Value.ToString());
                                int _DocID = int.Parse(item.Cells["Column13"].Value.ToString());
                                if (ClDoc.IsRowinDoc(ConACNT.ConnectionString, _StatusId, _TurnID) > 0)
                                {
                                    ClDoc.IsFinal(int.Parse(item.Cells["Column13"].Text));
                                    int res = ClDoc.DeleteDetail_ID(_DocID, _StatusId, _TurnID);
                                    //if (res > 0)
                                    { ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString())); }
                                }
                                else
                                {
                                    if (ClDoc.IsDocIDValid(_DocID))
                                        if (ClDoc.IsRowinDoc(ConACNT.ConnectionString, 28, _TurnID) > 0)
                                            throw new Exception("به علت صدور این برگه از قسمت تسویه فاکتورها، حذف گردش آن امکانپذیر نمی باشد");
                                    ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString()));
                                }
                            }
                            this.table_065_TurnReceptionTableAdapter.Fill(dataSet_01_Cash.Table_065_TurnReception);
                            Class_BasicOperation.ShowMsg("", "گردش مورد نظر حذف شد", Class_BasicOperation.MessageType.Information);


                        }
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف گردش چکها را ندارید", Class_BasicOperation.MessageType.Warning);
                }
                }
                 else
                     Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form03_ViewReceivedChq");
            }
        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 93))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_01_new_Recipt_Cheques")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                        _03_Bank.Frm_01_Recipt_Cheques frms = (_03_Bank.Frm_01_Recipt_Cheques)item;
                        frms.btn_Search_Click(sender, e);
                        return;
                    }
                }
                try
                {
                    _03_Bank.Frm_01_new_Recipt_Cheques frm = new Frm_01_new_Recipt_Cheques(UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 28),
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 29),
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 30),
                        int.Parse(((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                    try { frm.MdiParent = Frm_Main.ActiveForm; }
                    catch { }
                    frm.Show();
                }
                catch { }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = this.table_035_ReceiptChequesBindingSource.Position;
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                if (isadmin)
                {
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.Fill(this.dataSet_01_Cash.Table_035_ReceiptCheques);
                    this.table_065_TurnReceptionTableAdapter.Fill(dataSet_01_Cash.Table_065_TurnReception);
                    dataSet_01_Cash.EnforceConstraints = true;
                }
                else
                {
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillByUser(this.dataSet_01_Cash.Table_035_ReceiptCheques, Class_BasicOperation._UserName);
                    this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                    dataSet_01_Cash.EnforceConstraints = true;
                }

                this.table_035_ReceiptChequesBindingSource.Position = pos;
            }
            catch { }
        }
        DataTable Table;
        private void btnPrintList_Click(object sender, EventArgs e)
        {
            try
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

                PACNT._4_Reports.DataSet_Reports.Rec_ReportDataTable tbPrint = new PACNT._4_Reports.DataSet_Reports.Rec_ReportDataTable();

                foreach (Janus.Windows.GridEX.GridEXRow Item in gridEX2.GetCheckedRows())
                {
                    DataRow dr = tbPrint.NewRow();
                    dr["ChqId"] = Item.Cells["ColumnId"].Text.ToString();
                    dr["BackNum"] = Item.Cells["Column00"].Text.ToString();
                    dr["ChqNum"] = Item.Cells["Column03"].Text.ToString();
                    dr["Price"] = Convert.ToInt64(Convert.ToDouble(Item.Cells["Column05"].Value.ToString()));
                    dr["Date1"] = Item.Cells["Column02"].Text.ToString();
                    dr["Date2"] = Item.Cells["Column04"].Text.ToString();
                    dr["Person"] = Item.Cells["Column07"].Text.ToString();// mlt_BedPerson.Text;
                    dr["Bank"] = Item.Cells["Column08"].Text;
                    dr["Branch"] = Item.Cells["Column09"].Text;
                    dr["Project"] = Item.Cells["Column15"].Text.ToString();
                    dr["Cashier"] = Item.Cells["Column46"].Text.ToString();
                    dr["Babt"] = Item.Cells["Column06"].Text.ToString();


                    tbPrint.Rows.Add(dr);
                }
                if (tbPrint.Rows.Count > 0)
                {
                 
                    PACNT._3_Cheque_Operation.Reports.Frm_04_Rec_Report frm = new PACNT._3_Cheque_Operation.Reports.Frm_04_Rec_Report(tbPrint);
                    frm.ShowDialog();
                }


              
            }
            catch { }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {
                    if (gridEX2.GetCheckedRows().Length == 0)
                    {
                        DataTable Table = dataSet_01_Cash.Rpt_PrintRecChqs.Clone();
                        
                        Janus.Windows.GridEX.GridEXRow item = gridEX2.GetRow();
                        Int64 Price = (item.Cells["Column49"].Value.ToString() == "True" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["Column51"].Value.ToString())) : Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())));

                        Table.Rows.Add(item.Cells["ColumnId"].Value.ToString(),
                            item.Cells["Column00"].Value.ToString(),
                            item.Cells["Column02"].Value.ToString(),
                            Price.ToString("#,##0.###"),
                            FarsiLibrary.Utils.ToWords.ToString(Price),
                            item.Cells["Column03"].Value.ToString(),
                            item.Cells["Column04"].Value.ToString(),
                            item.Cells["Column08"].Text.Trim(),
                            item.Cells["Column06"].Text.Trim(),
                            item.Cells["Column07"].Text.Trim(),
                            item.Cells["Column01"].Text.Trim(), null
                        , null,
                        ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column42"].ToString()
                        , ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column55"].ToString(),
                        Convert.ToBoolean(((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column54"].ToString()));
                        PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq frm = new PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq(Table, item.Cells["ColumnId"].Value.ToString());
                        frm.ShowDialog();
                    }
                    else
                    {
                        
                        DataTable Table = dataSet_01_Cash.Rpt_PrintRecChqs.Clone();
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetCheckedRows())
                        {
                            Int64 Price = (item.Cells["Column49"].Value.ToString() == "True" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["Column51"].Value.ToString())) : Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())));

                            Table.Rows.Add(item.Cells["ColumnId"].Value.ToString(),
                            item.Cells["Column00"].Value.ToString(),
                            item.Cells["Column02"].Value.ToString(),
                            Price.ToString("#,##0.###"),
                            FarsiLibrary.Utils.ToWords.ToString(Price),
                            item.Cells["Column03"].Value.ToString(),
                            item.Cells["Column04"].Value.ToString(),
                            item.Cells["Column08"].Text.Trim(),
                            item.Cells["Column06"].Text.Trim(),
                            item.Cells["Column07"].Text.Trim(),
                            item.Cells["Column01"].Text.Trim(), null
                        , null,
                         item.Cells["Column42"].Value.ToString());
                        }
                        PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq frm = new PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq(Table, "0");
                        frm.ShowDialog();
                    }
                }
                catch
                {
                }
            }




        }

        private void gridEX2_DoubleClick(object sender, EventArgs e)
        {
            bt_Edit_Click(sender, e);
        }

        private void table_035_ReceiptChequesBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {
                    tabItem1.Text = " شماره برگه دریافت چک " + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                }
                catch
                {

                }
            }
            else
                tabItem1.Text = " شماره برگه دریافت چک ";
        }
    }
}
