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
    public partial class Frm_01_Recipt_Cheques : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        DataTable dt = new DataTable();
        int _ID = 0;
        bool _Del = false, _Export = false, _DelDoc = false, _New = false;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
       Classes. Class_UserScope UserScope = new  Classes.Class_UserScope();

       public Frm_01_Recipt_Cheques(bool Del, bool Export, bool DelDoc, int ID)
        {
            InitializeComponent();
            _Del = Del;
            _DelDoc = DelDoc;
            _Export = Export;
            _ID = ID;
            
        }

        private void Frm_01_Recipt_Cheques_Load(object sender, EventArgs e)
        {
          
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                mlt_fund.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo ");
                
            }
            else
            {
                mlt_fund.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "') ");

            }
           mlt_Person_pay.DataSource = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
           mlt_status.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnID,Column01,Column02 from Table_060_ChequeStatus where Column15=1");
           gridEX_Turn.DropDowns["Status"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_060_ChequeStatus");
           mlt_PersonBank.DataSource=gridEX_Turn.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo");
           gridEX_Turn.DropDowns["Bank"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");
           gridEX_Turn.DropDowns["Doc"].DataSource = ClDoc.ReturnTable(ConACNT, @"Select ColumnId,Column00 from Table_060_SanadHead");
           gridEX_Turn.DropDowns["Header"].DataSource = ClDoc.ReturnTable(ConACNT, @"Select * from AllHeaders()");
           
            mlt_Bank.DataSource = ClDoc.ReturnTable(ConBank, @"select Column00,Column01 from Table_010_BankNames ");

           if (_ID != 0)
           {
               dataSet_01_Cash.EnforceConstraints = false;
               this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, _ID);
               this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet_01_Cash.Table_065_TurnReception, _ID);
               dataSet_01_Cash.EnforceConstraints = true;

               if (this.table_035_ReceiptChequesBindingSource.Count > 0)
               {
                   this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                  
               }

           }



        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            table_035_ReceiptChequesBindingSource.AddNew();
            txt_Dat_Recipt.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
            txt_Usersabt.Text = Class_BasicOperation._UserName;
            txt_TimSabt.Text = Class_BasicOperation.ServerDate().ToString();
            mlt_status.Focus();
            //string status = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select ColumnId from Table_060_ChequeStatus where Column15=1),0)");
            //string fund = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select ColumnId from Table_020_BankCashAccInfo where columnid=1),0)");
            
            //mlt_status.Value = status;
           
            //mlt_fund.Value = fund;
            btn_New.Enabled = false;
            uiPanel1.Enabled = true;
        }

      

        private void mlt_status_KeyPress(object sender, KeyPressEventArgs e)
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



        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Number_Back.Text == "" || txt_Number_Back.Text == "0" || mlt_fund.Text == "" || mlt_fund.Text == "0" || txt_ChequeNumber.Text == "" || txt_ChequeNumber.Text == "0"
                    || txt_Fi.Text == "" || txt_Fi.Text == "0" || txt_Date_SRecipt.Text == "" || mlt_Bank.Text == "" || mlt_Bank.Text == "0" )
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل کنید ");
                    return;
                }

               
                
                btn_New.Enabled = true;
                
                if (MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    int PaperID = 0;
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (this.table_035_ReceiptChequesBindingSource.Count > 0 && !ClDoc.HasTurn_Rec(int.Parse(txt_Id.Text)))
                    {


                        if (mlt_status.Value.ToString() != "")
                        {
                            try
                            {
                                Save();
                                ClDoc.CheckExistFinalDoc();


                                PaperID = int.Parse(txt_Id.Text);
                            }
                            catch { }
                            if (PaperID > 0)
                            {
                              
                                _03_Bank.Form02_ExportDocForReceive frm = new Form02_ExportDocForReceive(int.Parse(txt_Id.Text), mlt_fund.Text, mlt_Bank.Text, txt_Dat_Recipt.Text);
                                frm.ShowDialog();
                                //((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column53"] = frm.Date;
                                dataSet_01_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, PaperID);
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, PaperID);
                                dataSet_01_Cash.EnforceConstraints = true;
                            }
                            gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConACNT, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                            {

                                //try
                                //{
                                //    this.ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", txt_Id.Text);
                                //}
                                //catch { }
                                this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);


                            }

                        }
                        else MessageBox.Show("وضعیت چک را انتخاب کنید");
                    }
                }
                 //}
                 ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column44"] = Class_BasicOperation._UserName;
                 ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column45"] = Class_BasicOperation.ServerDate().ToString();
                 table_035_ReceiptChequesBindingSource.EndEdit();
                 table_035_ReceiptChequesTableAdapter.Update(this.dataSet_01_Cash.Table_035_ReceiptCheques);
                 //MessageBox.Show("اطلاعات با موفقیت ثبت شد");
                 btn_New.Enabled = true;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }

 private void checkreturn()
        {

            string account = txt_HesabNumber.Text;
            if (account != "")
            {
                DataTable dt = ClDoc.ReturnTable(ConBank, @"SELECT     dt.Column02
FROM         (SELECT     dbo.Table_065_TurnReception.Column02
                       FROM          dbo.Table_035_ReceiptCheques INNER JOIN
                                              dbo.Table_065_TurnReception ON dbo.Table_035_ReceiptCheques.ColumnId = dbo.Table_065_TurnReception.Column01
                       WHERE      (dbo.Table_035_ReceiptCheques.Column10 = '" + account + @"')
                       GROUP BY dbo.Table_065_TurnReception.Column02) AS dt INNER JOIN
                      dbo.Table_060_ChequeStatus ON dt.Column02 = dbo.Table_060_ChequeStatus.ColumnId
WHERE     (dbo.Table_060_ChequeStatus.Column15 = 5)");
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("برای شماره حساب مربوطه چک برگشتی ثبت شده است.");
                }
            }
        
        }
        private void Save()
        {
            try
            {
                if (txt_Number_Back.Text == "" || txt_Number_Back.Text == "0" || mlt_fund.Text == "" || mlt_fund.Text == "0" || txt_ChequeNumber.Text == "" || txt_ChequeNumber.Text == "0"
                    || txt_Fi.Text == "" || txt_Fi.Text == "0" || txt_Date_SRecipt.Text == "" || mlt_Bank.Text == "" || mlt_Bank.Text == "0" || txt_Babat.Text == "" || txt_Babat.Text == "0")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل کنید ");
                    return;
                }

                if (((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column00"].ToString().StartsWith("-"))
                {
                    txt_Number_Back.Text = ClDoc.MaxNumber(Properties.Settings.Default.PBANK, "Table_035_ReceiptCheques", "Column00").ToString();
                }



                ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column44"] = Class_BasicOperation._UserName;
                ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column45"] = Class_BasicOperation.ServerDate().ToString();
                table_035_ReceiptChequesBindingSource.EndEdit();
                table_035_ReceiptChequesTableAdapter.Update(this.dataSet_01_Cash.Table_035_ReceiptCheques);
                //MessageBox.Show("اطلاعات با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }
        private void Frm_01_Recipt_Cheques_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.N)
            {
                btn_New_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {
                btn_Delete_Click(sender, e);
            }

            }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Int16 _StatusId; Int64 _TurnID = 0;
           if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 94))
                    {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    try
                    {
                        string _Message = "در صورت حذف این برگه، سند حسابداری و گردشهای مربوطه حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این برگه هستید؟";

                        if (DialogResult.Yes == MessageBox.Show(_Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            SqlDataAdapter SelectAdapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" +
                                txt_Id.Text, ConBank);
                            DataTable TurnRows = new DataTable();
                            SelectAdapter.Fill(TurnRows);
                            // if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(item["Column13"].ToString()), int.Parse(item["ColumnId"].ToString()), 28) != 28)
                            // edit by hosseiny
                            foreach (DataRow item in TurnRows.Rows)
                            {
                                _StatusId = short.Parse(item["Column02"].ToString());
                                _TurnID = long.Parse(item["ColumnId"].ToString());
                                if ((ClDoc.ReturnTable(ConACNT, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=" + _StatusId + " and column17=" + _TurnID + "),0) as Result")).Rows[0][0].ToString() == "0")
                                {
                                    ClDoc.IsFinal(int.Parse(item["Column13"].ToString()));
                                    ClDoc.DeleteDetail_ID(int.Parse(item["Column13"].ToString()),
                                     (_StatusId), int.Parse(_TurnID.ToString()));
                                    ClDoc.DeleteTurnReception(_TurnID);
                                }
                                else throw new Exception("به علت صدور این برگه از زیر سیستم های دیگر، حذف آن امکانپذیر نمی باشد");
                            }

                            ClDoc.RunSqlCommand(ConBank.ConnectionString, "Delete from Table_035_ReceiptCheques where ColumnId=" +
                                txt_Id.Text);
                            dataSet_01_Cash.EnforceConstraints = false;
                            this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, 0);
                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, 0);
                            dataSet_01_Cash.EnforceConstraints = true;
                            

                            Class_BasicOperation.ShowMsg("", "حذف برگه با موفقیت انجام شد", Class_BasicOperation.MessageType.Information);
                            btn_New.Enabled = true;
                            uiPanel1.Enabled=true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }

                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شماامکان حذف اطلاعات را ندارید", Class_BasicOperation.MessageType.Warning);

                }

            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                
                table_035_ReceiptChequesBindingSource.EndEdit();

                if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save();
                    }
                }
                if (isadmin)
                {
                     Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques),0) as Row");
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques where column42='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
             
                     if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet_01_Cash.Table_035_ReceiptCheques, RowId);
                    this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, RowId);
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void table_035_ReceiptChequesBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
               
                if (table_065_TurnReceptionBindingSource.Count>0)
                {
                    uiPanel1Container.Enabled = false;
                }
                else
                {

                    uiPanel1Container.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {

                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();

                    table_035_ReceiptChequesBindingSource.EndEdit();

                    if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save();
                        }
                    }
                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_035_ReceiptCheques where ColumnId>" + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_035_ReceiptCheques where ColumnId>" + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + " AND Column42='" + Class_BasicOperation._UserName + "'),0) as Row");

                    }
                          if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet_01_Cash.Table_035_ReceiptCheques, RowId);
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, RowId);
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    table_035_ReceiptChequesBindingSource.EndEdit();

                    if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save();
                        }
                    }

                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                            "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques where ColumnId<" +
                            ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                           "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques where ColumnId<" +
                           ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + " AND Column42='" + Class_BasicOperation._UserName + "'),0) as Row");
                    }
                         
                         if (Table.Rows[0]["Row"].ToString() != "0")
                       {
                        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet_01_Cash.Table_035_ReceiptCheques, RowId);
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, RowId);
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                table_035_ReceiptChequesBindingSource.EndEdit();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save();
                    }
                }
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_035_ReceiptCheques),0) as Row");
                    
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_035_ReceiptCheques where column42='" + Class_BasicOperation._UserName + "' ),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet_01_Cash.Table_035_ReceiptCheques, RowId);
                    this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, RowId);
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                }
                
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_DelTurns_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 95))
                {
                if (this.table_065_TurnReceptionBindingSource.Count > 0 && gridEX_Turn.GetCheckedRows().Length > 0)
                {
                    
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف گردش چک هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Turn.GetCheckedRows())
                            {
                                int _TurnID = int.Parse(item.Cells["ColumnId"].Value.ToString());
                                Int16 _StatusId = Int16.Parse(item.Cells["Column02"].Value.ToString());
                                int _DocID = int.Parse(item.Cells["Column13"].Value.ToString());
                                //edit by hosseiny
                                if ((ClDoc.ReturnTable(ConACNT, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=" + _StatusId + " and column17=" + _TurnID + "),0) as Result")).Rows[0][0].ToString() == "0")
                                {
                                    if (ClDoc.IsDocIDValid(_DocID))
                                    {
                                        ClDoc.IsFinal(_DocID);
                                        int res = ClDoc.DeleteDetail_ID(_DocID, _StatusId, _TurnID);
                                        //if (res > 0)
                                        { ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString())); }
                                    }
                                }
                                else { throw new Exception("به علت صدور این برگه از زیر سیستم های دیگر، حذف گردش آن امکانپذیر نمی باشد"); }
                                //if (ClDoc.IsRowinDoc(ConAcnt.ConnectionString, _StatusId, _TurnID) > 0)
                                //{
                                //    ClDoc.IsFinal(int.Parse(item.Cells["Column13"].Text));
                                //    ClDoc.DeleteDetail_ID(_DocID, _StatusId, _TurnID);
                                //    ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString()));
                                //}
                                //else
                                //{
                                //    if (ClDoc.IsDocIDValid(_DocID))
                                //        if (ClDoc.IsRowinDoc(ConAcnt.ConnectionString, 28, _TurnID) > 0)
                                //            throw new Exception("به علت صدور این برگه از قسمت تسویه فاکتورها، حذف گردش آن امکانپذیر نمی باشد");
                                //    ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString()));
                                //}
                            }

                            int PaperID = int.Parse(((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                            dataSet_01_Cash.EnforceConstraints = false;
                            this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, PaperID);
                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, PaperID);
                            dataSet_01_Cash.EnforceConstraints = true;
                            table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", "گردش مورد نظر حذف شد", Class_BasicOperation.MessageType.Information);

                            
                        }
                       
                    }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف گردش چکها را ندارید", Class_BasicOperation.MessageType.Warning);
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }



        public void btn_Search_Click(object sender, EventArgs e)
        {
            try{
            if (txt_Search.Text == "")
            {
                MessageBox.Show("لطفا شماره مورد نظر را وارد نمایید");
                return;
            }
            this.table_035_ReceiptChequesBindingSource.EndEdit();
            if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save();
                    }
                }
                if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
                {
              bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
              string user = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull ((select Column42 from Table_035_ReceiptCheques where Column00 =" + txt_Search.Text + "),0)");
            
                if (isadmin)
                {
                    if (mnu_Search_NumberBack.Checked)
                    {
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillByBackNumber(this.dataSet_01_Cash.Table_035_ReceiptCheques, int.Parse(txt_Search.Text.TrimEnd()));
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                        dataSet_01_Cash.EnforceConstraints = true;

                    }
                 
                    else
                    {
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillByChq(dataSet_01_Cash.Table_035_ReceiptCheques, txt_Search.Text.Trim());
                        if (this.table_035_ReceiptChequesBindingSource.Count > 1)
                            throw new Exception("برای این شماره چک بیش از دو برگه موجود است");
                        else if (this.table_035_ReceiptChequesBindingSource.Count == 1)
                        {

                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(
                                ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                        }
                        dataSet_01_Cash.EnforceConstraints = true;

                    }

                }
                else if (user == Class_BasicOperation._UserName)
                {

                    if (mnu_Search_NumberBack.Checked)
                    {
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillByBackNumber(this.dataSet_01_Cash.Table_035_ReceiptCheques, int.Parse(txt_Search.Text.TrimEnd()));
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(txt_Search.Text.Trim()));
                        dataSet_01_Cash.EnforceConstraints = true;

                    }

                    else
                    {
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillByChq(dataSet_01_Cash.Table_035_ReceiptCheques, txt_Search.Text.Trim());
                        if (this.table_035_ReceiptChequesBindingSource.Count > 1)
                            throw new Exception("برای این شماره چک بیش از دو برگه موجود است");
                        else if (this.table_035_ReceiptChequesBindingSource.Count == 1)
                        {

                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(
                                ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                        }
                        dataSet_01_Cash.EnforceConstraints = true;

                    }
                }
                    txt_Search.SelectAll();
                    dataSet_01_Cash.EnforceConstraints = true;

                    

                }
                else
                {
                    MessageBox.Show("شما به این شماره ارسال دسترسی ندارید");
                }


            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }

        private void mnu_Search_Paper_Click(object sender, EventArgs e)
        {

        }

        private void mnu_Search_ChqNumber_Click(object sender, EventArgs e)
        {

        }

        private void mnu_Search_ChqNumber_Click_1(object sender, EventArgs e)
        {
            mnu_Search_ChqNumber.Checked = true;
            mnu_Search_NumberBack.Checked = false;
            txt_Search.Focus();
            txt_Search.SelectAll();
        }

        private void mnu_Search_NumberBack_Click(object sender, EventArgs e)
        {
            mnu_Search_ChqNumber.Checked = false;
            mnu_Search_NumberBack.Checked = true;
            txt_Search.Focus();
            txt_Search.SelectAll();
        }

        private void uiPanel0_Click(object sender, EventArgs e)
        {

        }

        private void gridEX_Turn_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {

                    DataTable Table = dataSet_01_Cash.Rpt_PrintRecChqs.Clone();
                    //Janus.Windows.GridEX.GridEXRow item = gridEX1.GetRow();
                    Table.Rows.Add(txt_Id.Text.Trim(),
                        txt_Number_Back.Text.Trim(),
                        txt_Dat_Recipt.Text.Trim(),
                        Convert.ToInt64(Convert.ToDouble(txt_Fi.Text.Trim())).ToString("#,##0.###"),
                        FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble(txt_Fi.Text.Trim()))),
                        txt_ChequeNumber.Text.Trim(),
                        txt_Date_SRecipt.Text.Trim(),
                         mlt_Bank.Text.Trim(),
                        txt_Babat.Text.Trim(),
                        mlt_Person_pay.Text.Trim(),
                        mlt_fund.Text.Trim(), null
                        , null,
                        ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column42"].ToString()
                        , ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column55"].ToString(),
                        Convert.ToBoolean(((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column54"].ToString()));
                    PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq frm = new PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq(Table, ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Columnid"].ToString());
                    frm.ShowDialog();
                    
                }
                catch
                {
                }
            }
        }

        private void btn_Doc_Click(object sender, EventArgs e)
        {
            btn_Save_Click(sender, e);
        }

        private void uiPanel1Container_Click(object sender, EventArgs e)
        {

        }

        private void mlt_fund_ValueChanged(object sender, EventArgs e)
        {
            string personbank = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column35 from Table_020_BankCashAccInfo where ColumnId="+mlt_fund.Value+"");
            mlt_PersonBank.Value = personbank;
        }


     

      

       


    
    }
}
