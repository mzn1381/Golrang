using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PACNT._3_Cheque_Operation
{
    public partial class Form01_ReceiveChq : Form
    {
        int _ID = 0;
        bool _Del = false, _Export = false, _DelDoc = false,_New=false;
        Classes.Class_Documents ClDoc = new PACNT.Classes.Class_Documents();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlDataAdapter BoxAdapter, PersonAdapter, BanksAdapter, ProjectAdapter,ProvinceAdapter,CityAdapter,StatusAdapter;
        Class_UserScope UserScope = new Class_UserScope();
        DataTable dtPersonAll, dtPersonActive;
        public Form01_ReceiveChq(bool Del,bool Export,bool DelDoc,int ID)
        {
            InitializeComponent();
            _Del = Del;
            _DelDoc = DelDoc;
            _Export = Export;
            _ID = ID;
        }

        private void Form01_ReceiveChq_Load(object sender, EventArgs e)
        {
            
           foreach (Janus.Windows.GridEX.GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = SystemColors.Window;
                if (col.Key == "Column42" || col.Key == "Column44")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column43" || col.Key == "Column45")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }

            BoxAdapter = new SqlDataAdapter("Select * from Table_020_BankCashAccInfo", ConBank);
            BoxAdapter.Fill(dataSet1, "Box");
            gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Box"], "");
            gridEX_Turn.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Box"], "");
         
            dtPersonActive = new DataTable();
            dtPersonAll = new DataTable();
            dtPersonActive = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeople(3)");
            dtPersonAll = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeopleInActive(3)");


            //PersonAdapter = new SqlDataAdapter("Select * from ListPeople(3)", ConBase);
            //PersonAdapter.Fill(dataSet1, "Person");
            gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
            gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonAll, "");
            gridEX_Turn.DropDowns["Person"].SetDataBinding(dtPersonAll, "");

            BanksAdapter = new SqlDataAdapter("Select * from Table_010_BankNames", ConBank);
            BanksAdapter.Fill(dataSet1, "Banks");
            gridEX1.DropDowns["Banks"].SetDataBinding(dataSet1.Tables["Banks"], "");

            StatusAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_060_ChequeStatus where Column01=0", ConBank);
            StatusAdapter.Fill(dataSet1, "Status");
            gridEX1.DropDowns["Status"].SetDataBinding(dataSet1.Tables["Status"], "");
            gridEX_Turn.DropDowns["Status"].SetDataBinding(dataSet1.Tables["Status"], "");

            ProjectAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            ProjectAdapter.Fill(dataSet1, "Project");
            gridEX1.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");
            gridEX_Turn.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");

            gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Turn.DropDowns["Header"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select ACC_Code,ACC_Name from AllHeaders()"), "");

            DataTable CurrencyTable=ClDoc.ReturnTable(ConBase,"Select * from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_Turn.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            ProvinceAdapter = new SqlDataAdapter("Select Column00,Column01 from Table_060_ProvinceInfo", ConBase);
            ProvinceAdapter.Fill(dataSet1, "Province");
            CityAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_065_CityInfo", ConBase);
            CityAdapter.Fill(dataSet1, "City");
            DataRelation Province_City = new DataRelation("Province_City", dataSet1.Tables["Province"].Columns["Column00"], dataSet1.Tables["City"].Columns["Column00"], false);
            ForeignKeyConstraint Fkc = new ForeignKeyConstraint("F_Province_City", dataSet1.Tables["Province"].Columns["Column00"], dataSet1.Tables["City"].Columns["Column00"]);
            Fkc.AcceptRejectRule = AcceptRejectRule.None;
            Fkc.UpdateRule = Rule.Cascade;
            Fkc.DeleteRule = Rule.None;
            dataSet1.Relations.Add(Province_City);
            dataSet1.Tables["City"].Constraints.Add(Fkc);
            ProvincebindingSource.DataSource = dataSet1.Tables["Province"];
            CitybindingSource.DataSource = ProvincebindingSource;
            CitybindingSource.DataMember="Province_City";
            gridEX1.DropDowns["Province"].SetDataBinding(ProvincebindingSource, "");
            gridEX1.DropDowns["City"].SetDataBinding(CitybindingSource, "");

            
            if (_ID != 0)
            {
                dataSet02_Cash.EnforceConstraints = false;
                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, _ID);
                this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet02_Cash.Table_065_TurnReception,_ID);
                dataSet02_Cash.EnforceConstraints = true;

                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                    try
                    {
                        this.ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", gridEX1.GetValue("Column11").ToString());
                    }
                    catch { }
                }

            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {

            Janus.Windows.GridEX.GridEXRow dr = gridEX1.CurrentRow;
            if (gridEX1.GetRows().Count() > 0)
            {
                var Column48 = dr.Cells["Column48"].Value.ToString();
                var Column01 = dr.Cells["Column01"].Value.ToString();
                var Column02 = dr.Cells["Column02"].Value.ToString();
                var Column08 = dr.Cells["Column08"].Value.ToString();
                var Column09 = dr.Cells["Column09"].Value.ToString();
                var Column10 = dr.Cells["Column10"].Value.ToString();
                var Column07 = dr.Cells["Column07"].Value.ToString();
                var Column06 = dr.Cells["Column06"].Value.ToString();
                try
                {
                    gridEX1.Enabled = true;
                    foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                    {
                        if (item.Key == "Column46"||item.Key == "Column54"||item.Key == "Column55")
                            item.Selectable = false;
                        else
                            item.Selectable = true;
                    }

                    dataSet02_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, 0);
                    this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet02_Cash.Table_065_TurnReception, 0);
                    dataSet02_Cash.EnforceConstraints = true;

                    gridEX1.MoveToNewRecord();
                    superTabItem1.Text = "اطلاعات برگه دریافت چک ";
                    gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                    gridEX1.SetValue("Column42", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column43", Class_BasicOperation.ServerDate());
                    gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());
                    // gridEX1.SetValue("Column48", dataSet1.Tables["Status"].Rows[0]["ColumnId"].ToString());
                    gridEX1.SetValue("Column48", Column48);
                    gridEX1.SetValue("Column01", Column01);
                    gridEX1.SetValue("Column02", Column02);
                    gridEX1.SetValue("Column08", Column08);
                    gridEX1.SetValue("Column09", Column09);
                    gridEX1.SetValue("Column10", Column10);
                    gridEX1.SetValue("Column07", Column07);
                    gridEX1.SetValue("Column06", Column06);

                    gridEX1.RootTable.Columns["Column50"].Selectable = false;
                    gridEX1.RootTable.Columns["Column51"].Selectable = false;
                    gridEX1.Select();
                    gridEX1.Col = 1;
                    bt_New.Enabled = false;

                 


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
                    gridEX1.Enabled = true;
                    foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                    {
                        if (item.Key == "Column46" || item.Key == "Column54" || item.Key == "Column55")
                            item.Selectable = false;
                        else
                            item.Selectable = true;
                    }

                    dataSet02_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, 0);
                    this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet02_Cash.Table_065_TurnReception, 0);
                    dataSet02_Cash.EnforceConstraints = true;

                    gridEX1.MoveToNewRecord();
                    superTabItem1.Text = "اطلاعات برگه دریافت چک ";
                    gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                    gridEX1.SetValue("Column42", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column43", Class_BasicOperation.ServerDate());
                    gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());
                    gridEX1.SetValue("Column48", dataSet1.Tables["Status"].Rows[0]["ColumnId"].ToString());

                    gridEX1.RootTable.Columns["Column50"].Selectable = false;
                    gridEX1.RootTable.Columns["Column51"].Selectable = false;
                    gridEX1.Select();
                    gridEX1.Col = 1;
                    bt_New.Enabled = false;

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }


            /////
            gridEX1.DropDowns["Person"].SetDataBinding(dtPersonActive, "");
            gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonActive, "");
            _New = true;
        }

        private void SaveEvent(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                gridEX1.UpdateData();
                bool Valid = true;
                try
                {
                    FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString()));
                    FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column04").ToString()));
                    if (gridEX1.GetValue("Column53").ToString() != "") { FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column53").ToString())); }
                }
                catch { Valid = false; }
                if (!Valid) throw new Exception("تاریخ دریافت/ تاریخ موثر/تاریخ سررسید چک نامعتبر است");


                this.table_035_ReceiptChequesBindingSource.EndEdit();

                if (gridEX1.GetValue("Column49").ToString() == "True")
                {
                    if (gridEX1.GetRow().Cells["Column50"].Text.ToString().Trim() == "" ||
                        Convert.ToDouble(gridEX1.GetRow().Cells["Column51"].Value.ToString()) <= 0)
                    {
                        throw new Exception("نوع ارز و ارزش ارز را مشخص کنید");
                    }
                }

                if (((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column00"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column00", ClDoc.SuggetstBackNumber());
                }
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                this.table_035_ReceiptChequesTableAdapter.Update(dataSet02_Cash.Table_035_ReceiptCheques);
                if (sender == bt_Save || sender == this)
                    Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
                gridEX1.UpdateData();
                superTabItem1.Text = "اطلاعات برگه دریافت چک شماره " + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                bt_New.Enabled = true;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.GetValue("Column05").ToString() == "0")
                { MessageBox.Show("امکان ثبت مبلغ 0 وجود ندارد"); return; }
                 /////
                if (gridEX1.GetValue("ColumnId").ToString().StartsWith("-"))
                {
                    DataTable dtrecipt = ClDoc.ReturnTable(ConBank, @"select column03 from Table_035_ReceiptCheques where column03='" + gridEX1.GetValue("Column03").ToString()+"'");
                    if (dtrecipt.Rows.Count > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("شماره چک تکراری است.آیا مایل به ذخیره و ادامه کار هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            checkreturn();
                            SaveEvent(sender, e);
                        }
                        else return;
                    }
                    else
                    {
                        checkreturn();
                        SaveEvent(sender, e);
                    }
                }
                else
                {
                    checkreturn();
                    SaveEvent(sender, e);
                }
                //try
                //{
                //    DataTable Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques),0) as Row");
                //    if (Table.Rows[0]["Row"].ToString() != "0")
                //    {
                //        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                //        dataSet02_Cash.EnforceConstraints = false;
                //        this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet02_Cash.Table_035_ReceiptCheques, RowId);
                //        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, RowId);
                //        dataSet02_Cash.EnforceConstraints = true;
                //        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                //    }
                //}
                //catch { }

                
            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }
        private void checkreturn()
        {
         
            string account = gridEX1.GetValue("Column10").ToString();
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
        private void bt_Del_Click(object sender, EventArgs e)
        {
            Int16 _StatusId;Int64 _TurnID = 0;
            if (_Del)
            {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    try
                    {
                        string _Message = "در صورت حذف این برگه، سند حسابداری و گردشهای مربوطه حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این برگه هستید؟";

                        if (DialogResult.Yes == MessageBox.Show(_Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            SqlDataAdapter SelectAdapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" +
                                gridEX1.GetValue("ColumnId").ToString(), ConBank);
                            DataTable TurnRows = new DataTable();
                            SelectAdapter.Fill(TurnRows);
                            // if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(item["Column13"].ToString()), int.Parse(item["ColumnId"].ToString()), 28) != 28)
                               // edit by hosseiny
                            foreach (DataRow item in TurnRows.Rows)
                            {
                                _StatusId = short.Parse(item["Column02"].ToString());
                                _TurnID = long.Parse(item["ColumnId"].ToString());
                                 if ((ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=" + _StatusId + " and column17=" + _TurnID + "),0) as Result")).Rows[0][0].ToString() == "0")
                                {
                                    ClDoc.IsFinal(int.Parse(item["Column13"].ToString()));
                                    ClDoc.DeleteDetail_ID(int.Parse(item["Column13"].ToString()),
                                     (_StatusId),int.Parse( _TurnID.ToString()));
                                    ClDoc.DeleteTurnReception(_TurnID);
                                }
                                else throw new Exception("به علت صدور این برگه از زیر سیستم های دیگر، حذف آن امکانپذیر نمی باشد");
                            }

                            ClDoc.RunSqlCommand(ConBank.ConnectionString, "Delete from Table_035_ReceiptCheques where ColumnId=" +
                                 gridEX1.GetValue("ColumnId").ToString());
                            dataSet02_Cash.EnforceConstraints = false;
                            this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, 0);
                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, 0);
                            dataSet02_Cash.EnforceConstraints = true;


                            Class_BasicOperation.ShowMsg("", "حذف برگه با موفقیت انجام شد", Class_BasicOperation.MessageType.Information);
                            bt_New.Enabled = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شماامکان حذف اطلاعات را ندارید", Class_BasicOperation.MessageType.Warning);
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
          //  var UserSignature1 = ClDoc.ExScalar3(ConMain.ConnectionString, @"select isnull((select column39 from Table_010_UserInfo where Column00='"+((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column42"].ToString()+"'),'') as res");
          //  var UserSignature2 = ClDoc.ExScalar3(ConMain.ConnectionString, @"select isnull((select column39 from Table_010_UserInfo where Column00='" + ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column55"].ToString() + "'),'') as res");
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = dataSet_Reports.Rpt_PrintRecChqs.Clone();
                    Janus.Windows.GridEX.GridEXRow item = gridEX1.GetRow();
                    Table.Rows.Add(item.Cells["ColumnId"].Value.ToString(),
                        item.Cells["Column00"].Value.ToString(),
                        item.Cells["Column02"].Value.ToString(),
                        Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString())).ToString("#,##0.###"),
                        FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble(item.Cells["Column05"].Value.ToString()))),
                        item.Cells["Column03"].Value.ToString(),
                        item.Cells["Column04"].Value.ToString(),
                        item.Cells["Column08"].Text.Trim(),
                        item.Cells["Column06"].Text.Trim(),
                        item.Cells["Column07"].Text.Trim(),
                        item.Cells["Column01"].Text.Trim(),null
                        , null,
                        ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column42"].ToString()
                        , ((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column55"].ToString(),
                        Convert.ToBoolean(((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column54"].ToString()));
                    _3_Cheque_Operation.Reports.Form01_PrintRecChq frm = new Reports.Form01_PrintRecChq(Table,((DataRowView)table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Columnid"].ToString());
                    frm.ShowDialog();

                }
                catch
                {
                }
            }
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
           
            try
            {
                gridEX1.MoveToNewRecord();
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                if (dataSet02_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }
                if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
                {
                    if (mnu_Search_Paper.Checked)
                    {
                        dataSet02_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, int.Parse(txt_Search.Text.Trim()));
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, int.Parse(txt_Search.Text.Trim()));
                        dataSet02_Cash.EnforceConstraints = true;
                    }
                    else
                    {
                        dataSet02_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillByChq(dataSet02_Cash.Table_035_ReceiptCheques,txt_Search.Text.Trim());
                        if (this.table_035_ReceiptChequesBindingSource.Count > 1)
                            throw new Exception("برای این شماره چک بیش از دو برگه موجود است");
                        else if (this.table_035_ReceiptChequesBindingSource.Count == 1)
                        {
                            
                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, int.Parse(
                                ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                        }
                        dataSet02_Cash.EnforceConstraints = true;
                    }
                    
                    if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                    {
                        try
                        {
                            this.ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", gridEX1.GetValue("Column11").ToString());
                        }
                        catch { }
                        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);

                    }
                    else
                    {
                        gridEX1.Enabled = false;
                        throw new Exception("شماره برگه/چک وارد شده نامعتبر می باشد");
                    }

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                txt_Search.SelectAll();
            }
        }

        private void bt_Export_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Export)
                {
                    int PaperID = 0;
                    Class_UserScope UserScope = new Class_UserScope();
                    if (this.table_035_ReceiptChequesBindingSource.Count > 0 && !ClDoc.HasTurn_Rec(int.Parse(gridEX1.GetValue("ColumnId").ToString())))
                    
                    {
                       

                        if (gridEX1.GetValue("Column48").ToString() != "")
                        {
                            try
                            {
                                bt_Save_Click(sender, e);
                                ClDoc.CheckExistFinalDoc();


                                PaperID = int.Parse(gridEX1.GetValue("ColumnId").ToString());
                            }
                            catch { }
                              if (PaperID >0)
                              {
                                  _3_Cheque_Operation.Form02_ExportDocForReceive frm = new Form02_ExportDocForReceive(int.Parse(gridEX1.GetValue("ColumnId").ToString()), gridEX1.GetRow().Cells["Column01"].Text, gridEX1.GetRow().Cells["Column08"].Text, gridEX1.GetRow().Cells["Column02"].Text);
                                  frm.ShowDialog();

                                  dataSet02_Cash.EnforceConstraints = false;
                                  this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, PaperID);
                                  this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, PaperID);
                                  dataSet02_Cash.EnforceConstraints = true;
                              }
                                gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                                if (this.table_035_ReceiptChequesBindingSource.Count > 0 && gridEX1.RowCount > 0)
                                {

                                    try
                                    {
                                        this.ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", gridEX1.GetValue("Column11").ToString());
                                    }
                                    catch { }
                                    this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);


                                }
                         
                        }
                        else MessageBox.Show("وضعیت چک را انتخاب کنید");
                    }
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }
      
        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                bt_Search_Click(sender, e);
                txt_Search.SelectAll();
            }
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                 if (e.Column.Key == "Column11")
                    ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", gridEX1.GetValue("Column11").ToString());
            }
            catch
            {
            }
            try
            {
                if (e.Column.Key == "Column01")
                {

                    if (gridEX1.GetValue("Column01").ToString().Trim() != "")
                        if (gridEX1.DropDowns["ToBank"].GetValue("Column01").ToString() == "True")
                            gridEX1.SetValue("Column46", gridEX1.DropDowns["ToBank"].GetValue("Column35").ToString());
                        else
                            gridEX1.SetValue("Column46", DBNull.Value);

                }
            }
            catch { gridEX1.SetValue("Column46", DBNull.Value); }
  try
            {
               
                if (e.Column.Key == "Column49")
                {
                    if (gridEX1.GetValue("Column49").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column50"].Selectable = true;
                        gridEX1.RootTable.Columns["Column51"].Selectable = true;
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column50"].Selectable = false;
                        gridEX1.RootTable.Columns["Column51"].Selectable = false;

                    }


                }
               
            }
            catch { }
            gridEX1.CurrentCellDroppedDown = true;
            gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());
            
            try
            {
                if (e.Column.Key == "Column48")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column48", null, "Column02", gridEX1.EditTextBox.Text);
                
                else if (e.Column.Key == "Column01")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column01", null, "Column02", gridEX1.EditTextBox.Text);

                else if (e.Column.Key == "Column08")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column08", null, "Column01", gridEX1.EditTextBox.Text);

                else if (e.Column.Key == "Column11")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column11", null, "Column01", gridEX1.EditTextBox.Text);

                else if (e.Column.Key == "Column12")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column12", null, "Column02", gridEX1.EditTextBox.Text);

                else if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text);
                
                else if (e.Column.Key == "Column15")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column15", "Column01", "Column02", gridEX1.EditTextBox.Text);
        
            
            
            }
            catch 
            {
            }
            try
            {
                if (e.Column.Key == "Column04")
                    gridEX1.SetValue("Column53", gridEX1.GetValue("Column04").ToString());
            }
            catch { }

        }

        private void Form01_ReceiveChq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.N && bt_New.Enabled)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E)
                bt_Export_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
               
                mnu_SearchType.ShowDropDown();

            }
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void table_035_ReceiptChequesBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {
                    try
                    {
                        this.ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column11"].ToString());
                    }
                    catch { }
                    if (ClDoc.HasTurn_Rec(int.Parse(gridEX1.GetValue("ColumnId").ToString())))
                    {
                        gridEX1.Enabled = true;
                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                        {
                            if (item.Key == "Column41" || item.Key=="Column42")
                                item.Selectable = true;
                            else
                                item.Selectable = false;
                        }
                    }
                    else
                    {
                        gridEX1.Enabled = true;
                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                        {
                            if (item.Key == "Column46" ||item.Key == "Column54"||item.Key == "Column55")
                                item.Selectable = false;
                            else
                                item.Selectable = true;
                        }
                    }
                    superTabItem1.Text = "اطلاعات برگه دریافت چک شماره  " + gridEX1.GetValue("ColumnId").ToString();
                }
                catch
                {

                }
            }
            else
                superTabItem1.Text = "اطلاعات برگه دریافت چک ";




            ////////////
            if (_New == true)
            {
                gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
                gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonAll, "");

                _New = false;
            }


        }


        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column50")
                    gridEX1.EnterKeyBehavior =  Janus.Windows.GridEX.EnterKeyBehavior.None;
                else gridEX1.EnterKeyBehavior =  Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            }
            catch
            {
            }
        }

        private void gridEX1_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
            catch
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }

            try
            {
                //if (e.Column.Key == "Column02" && e.Value.ToString() != "")
                //{
                //    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                //    try
                //    {
                //        fa.SelectedDateTime = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                //        UpdateEfficientDate();
                //    }
                //    catch
                //    {
                //        e.Cancel = true;
                //    }
                //}
            }
            catch
            {
            }

            try
            {
                //if (e.Column.Key == "Column04" && e.Value.ToString() != "")
                //{
                //    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                //    try
                //    {
                //        fa.SelectedDateTime = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column04").ToString())).ToShortDateString());
                //        UpdateEfficientDate();
                //    }
                //    catch
                //    {
                //        e.Cancel = true;
                //    }
                //}
            }
            catch
            {
            }
        }

        private void mnu_People_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();
                //dataSet1.Tables["Person"].Rows.Clear();
                //PersonAdapter.Fill(dataSet1, "Person");
                dtPersonActive = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeople(3)");
                dtPersonAll = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeopleInActive(3)");
                if (_New == true)
                {
                    gridEX1.DropDowns["Person"].SetDataBinding(dtPersonActive, "");
                    gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonActive, "");
                }
                else
                {
                    gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
                    gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonAll, "");
                }

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void mnu_Project_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 3))
            {
                PACNT._1_BasicMenu.Form02_Projects frm = new PACNT._1_BasicMenu.Form02_Projects(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 4));
                frm.ShowDialog();
                dataSet1.Tables["Projects"].Rows.Clear();
                ProjectAdapter.Fill(dataSet1, "Projects");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void mnu_Banks_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 4))
            {
                _1_Basic_Menu.Form03_BanksBox frm = new PACNT._1_Basic_Menu.Form03_BanksBox(
                      UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 5),
                      UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 6));
                frm.ShowDialog();
                dataSet1.Tables["Box"].Rows.Clear();
                BoxAdapter.Fill(dataSet1, "Box");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewDocument")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument();
               try { frm.MdiParent = MainForm.ActiveForm; } catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void mnu_ViewPapers_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 31))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form03_ViewReceivedChq")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _3_Cheque_Operation.Form03_ViewReceivedChq frm = new Form03_ViewReceivedChq(0, UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 40));
               try { frm.MdiParent = MainForm.ActiveForm; } catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void Form01_ReceiveChq_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void mnu_BanksAndBranches_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 2))
            {
                _1_Basic_Menu.Form02_BanksBranches frm = new PACNT._1_Basic_Menu.Form02_BanksBranches(
                      UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 3));
                frm.ShowDialog();
                dataSet1.Tables["Banks"].Clear();
                BanksAdapter.Fill(dataSet1,"Banks");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void mnu_Status_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 37))
            {
                _1_Basic_Menu.Form08_DefineChequeStatus frm=new PACNT._1_Basic_Menu.Form08_DefineChequeStatus(
                      UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 38));
                frm.ShowDialog();
                dataSet1.Tables["Status"].Clear();
                StatusAdapter.Fill(dataSet1, "Status");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void mnu_SignatureSetting_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 57))
            {
                _3_Cheque_Operation.Form12_RecChqSignatures frm = new Form12_RecChqSignatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.Stop);
        }

        private void gridEX1_RowEditCanceled(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            gridEX1.Enabled = false;
            bt_New.Enabled = true;
            gridEX1.MoveToNewRecord();
            superTabItem1.Text = "اطلاعات برگه دریافت چک ";
        }

        private void mnu_Search_ChqNumber_Click(object sender, EventArgs e)
        {
            mnu_Search_ChqNumber.Checked = true;
            mnu_Search_Paper.Checked = false;
            txt_Search.Focus();
            txt_Search.SelectAll();
        }

        private void mnu_Search_Paper_Click(object sender, EventArgs e)
        {
            mnu_Search_ChqNumber.Checked = false;
            mnu_Search_Paper.Checked = true;
            txt_Search.Focus();
            txt_Search.SelectAll();
        }

        private void mnu_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0 &&
                    !((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ثبت برگه جدید بر اساس اطلاعات جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BANK))
                        {
                            DataRowView Row = (DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current;
                            Con.Open();

                            _3_Cheque_Operation.Form15_CopyRecChq_Info frm = new Form15_CopyRecChq_Info(Convert.ToDouble(Row["Column05"].ToString()));
                            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                string ChqNumber, Status,FinalDate;
                                ChqNumber = frm._ChqNumber;
                                Status = frm._Status;
                                FinalDate = frm._Date;
                                Double Price = frm._Price;
                                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                                Key.Direction = ParameterDirection.Output;

                                SqlCommand InsertCommand = new SqlCommand(@"INSERT INTO Table_035_ReceiptCheques ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
,[Column12]      ,[Column15]      ,[Column41]      ,[Column42]      ,[Column43]      ,[Column44]
,[Column45]      ,[Column46]      ,[Column48]      ,[Column49]      ,[Column50]      ,[Column51]     
,[Column52]      ,[Column53])
                                    VALUES (" +
                                    ClDoc.MaxNumber(Con.ConnectionString, "Table_035_ReceiptCheques", "Column00").ToString() +
                                    "," + Row["Column01"].ToString() + ",'" + Row["Column02"].ToString() + "','" + ChqNumber + "','" +
                                   FinalDate + "'," +Price + ",'" + Row["Column06"].ToString().Trim() + "'," +
                                    Row["Column07"].ToString() + "," + Row["Column08"].ToString() + "," +
                                    (Row["Column09"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column09"].ToString().Trim() + "'") + "," +
                                    (Row["Column10"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column10"].ToString().Trim() + "'") + "," +
                                    (Row["Column11"].ToString().Trim() == "" ? "NULL" : Row["Column11"].ToString().Trim()) + "," +
                                    (Row["Column12"].ToString().Trim() == "" ? "NULL" : Row["Column12"].ToString().Trim()) + "," +
                                    (Row["Column15"].ToString().Trim() == "" ? "NULL" : Row["Column15"].ToString().Trim()) + "," +
                                    (Row["Column41"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column41"].ToString().Trim() + "'") + ",'" +
                                    Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                                    (Row["Column46"].ToString().Trim() == "" ? "NULL" : Row["Column46"].ToString().Trim()) + "," +
                                    Status+","+
                                    (Row["Column49"].ToString().Trim()=="True"?"1":"0")+","+
                                    (Row["Column50"].ToString().Trim()==""?"NULL":Row["Column50"].ToString())+","+
                                    Row["Column51"].ToString()+","+
                                    (Row["Column52"].ToString().Trim() == "" ? "NULL" : Row["Column52"].ToString())+","+
                                    (Row["Column53"].ToString().Trim() == "" ? "NULL" : "'"+Row["Column53"].ToString()+"'")
                                    +"); Set @Key=Scope_Identity()" ,Con);

                                InsertCommand.Parameters.Add(Key);
                                InsertCommand.ExecuteNonQuery();
                                Class_BasicOperation.ShowMsg("", "برگه جدید با شماره " + Key.Value.ToString() + " ثبت شد", Class_BasicOperation.MessageType.Information);
                                txt_Search.Text = Key.Value.ToString();
                                dataSet02_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, int.Parse(Key.Value.ToString()));
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, int.Parse(Key.Value.ToString()));
                                dataSet02_Cash.EnforceConstraints = true;
                                foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                                {
                                    if (item.Key == "Column46")
                                        item.Selectable = false;
                                    else
                                        item.Selectable = true;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column48");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column08");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column11");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column12");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column15");
            }
            catch 
            {
            }
            try
            {
                if (e.Column.Key == "Column50")
                {
                    object value = gridEX1.GetValue("Column50");
                    DataRowView Row = (DataRowView)gridEX1.RootTable.Columns["Column50"].DropDown.FindItem(value);
                    gridEX1.SetValue("Column51", Row["Column02"].ToString());
                }
                else if (e.Column.Key == "Column49")
                {
                    gridEX1.SetValue("Column50", DBNull.Value);
                    gridEX1.SetValue("Column51", 0);
                }
            }
            catch
            {
            }
              try
            {
                if (e.Column.Key == "Column52")
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                    DateTime SecDate = BaseDate.AddDays(double.Parse(gridEX1.GetValue("Column52").ToString()));
                    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                    fa.SelectedDateTime = SecDate;
                    fa.UpdateTextValue();

                 //   gridEX1.SetValue("Column53", fa.Text);
                }
                else if (e.Column.Key == "Column53")
                {
                    //DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                    //DateTime SecDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column53").ToString())).ToShortDateString());
                    //TimeSpan Sub = SecDate - BaseDate;
                    //gridEX1.SetValue("Column52", Sub.Days);
                }
            }
            catch
            {
            }
        
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_035_ReceiptChequesBindingSource.EndEdit();

                if (dataSet02_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }

                DataTable Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_035_ReceiptCheques),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet02_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet02_Cash.Table_035_ReceiptCheques, RowId);
                    this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, RowId);
                    dataSet02_Cash.EnforceConstraints = true;
                    this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_035_ReceiptChequesBindingSource.EndEdit();

                    if (dataSet02_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }


                    DataTable Table = ClDoc.ReturnTable(ConBank,
                        "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques where ColumnId<" +
                        ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet02_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet02_Cash.Table_035_ReceiptCheques, RowId);
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, RowId);
                        dataSet02_Cash.EnforceConstraints = true;
                        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_035_ReceiptChequesBindingSource.EndEdit();

                    if (dataSet02_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }

                    DataTable Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_035_ReceiptCheques where ColumnId>" + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet02_Cash.EnforceConstraints = false;
                        this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet02_Cash.Table_035_ReceiptCheques, RowId);
                        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, RowId);
                        dataSet02_Cash.EnforceConstraints = true;
                        this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_035_ReceiptChequesBindingSource.EndEdit();

                if (dataSet02_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }

                DataTable Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_035_ReceiptCheques),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet02_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet02_Cash.Table_035_ReceiptCheques, RowId);
                    this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, RowId);
                    dataSet02_Cash.EnforceConstraints = true;
                    this.table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bt_ImportFromExcel_Click(object sender, EventArgs e)
        {
            _3_Cheque_Operation.Form19_Import_ReceiverChq_FromExcel frm = new Form19_Import_ReceiverChq_FromExcel();
            frm.ShowDialog();
        }

        private void bt_DelTurns_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_065_TurnReceptionBindingSource.Count > 0 && gridEX_Turn.GetCheckedRows().Length>0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName,"Column09",40))
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف گردش چک هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Turn.GetCheckedRows())
                            {
                                int _TurnID = int.Parse(item.Cells["ColumnId"].Value.ToString());
                                Int16 _StatusId = Int16.Parse(item.Cells["Column02"].Value.ToString());
                                int _DocID = int.Parse(item.Cells["Column13"].Value.ToString());
                                //edit by hosseiny
                                if ((ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=" + _StatusId + " and column17=" + _TurnID + "),0) as Result")).Rows[0][0].ToString() == "0")
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
                            dataSet02_Cash.EnforceConstraints = false;
                            this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet02_Cash.Table_035_ReceiptCheques, PaperID);
                            this.table_065_TurnReceptionTableAdapter.FillBy(dataSet02_Cash.Table_065_TurnReception, PaperID);
                            dataSet02_Cash.EnforceConstraints = true;
                            table_035_ReceiptChequesBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", "گردش مورد نظر حذف شد", Class_BasicOperation.MessageType.Information);


                        }
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف گردش چکها را ندارید", Class_BasicOperation.MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_EditPaper_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 117))
            {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    try
                    {
                        try
                        {
                            this.ProvincebindingSource.Position = ProvincebindingSource.Find("Column00", ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["Column11"].ToString());
                        }
                        catch { }

                        gridEX1.Enabled = true;
                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                        {
                            if (item.Key == "Column46")
                                item.Selectable = false;
                            else
                                item.Selectable = true;
                        }
                        superTabItem1.Text = "اطلاعات برگه دریافت چک شماره  " + gridEX1.GetValue("ColumnId").ToString();
                    }
                    catch
                    {

                    }
                }
                else
                    superTabItem1.Text = "اطلاعات برگه دریافت چک ";
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان ویرایش اسناد دارای گردش را ندارید", Class_BasicOperation.MessageType.None);

        }

        private void UpdateEfficientDate()
        {
            try
            {
                if (gridEX1.GetRow().Cells["Column52"].Text.Trim() != "")
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                    DateTime SecDate = BaseDate.AddDays(double.Parse(gridEX1.GetValue("Column52").ToString()));
                    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                    fa.SelectedDateTime = SecDate;
                    fa.UpdateTextValue();

                    gridEX1.SetValue("Column53", fa.Text);
                }
                else if (gridEX1.GetRow().Cells["Column53"].Text.Trim() != "")
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                    DateTime SecDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column53").ToString())).ToShortDateString());
                    TimeSpan Sub = SecDate - BaseDate;
                    gridEX1.SetValue("Column52", Sub.Days);
                }
            }
            catch { }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 202))
            {
                if (gridEX1.GetValue("Column54").ToString() == "True")
                {

                    ClDoc.RunSqlCommand(ConBank.ConnectionString, @"update Table_035_ReceiptCheques set column55='" + Class_BasicOperation._UserName + @"' ,Column54=0 
                    where ColumnId=" + gridEX1.GetValue("ColumnId").ToString());
                }
                else
                {
                    ClDoc.RunSqlCommand(ConBank.ConnectionString, @"update Table_035_ReceiptCheques set column55='" + Class_BasicOperation._UserName + @"' ,
Column54=1 where ColumnId=" + gridEX1.GetValue("ColumnId").ToString());
                }
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                txt_Search.Text = gridEX1.GetValue("ColumnId").ToString();
                mnu_Search_Paper_Click(sender, e);
                bt_Search_Click(sender, e);

            }
            else { Class_BasicOperation.ShowMsg("", "شما به این قسمت دسترسی ندارید", Class_BasicOperation.MessageType.Stop); }
        }

         

        

    }
}
