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
    public partial class Frm_01_Recipt_Cheques_Group : Form
    {
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);

        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        DataTable dtPersonAll, dtPersonActive;
        public Frm_01_Recipt_Cheques_Group()
        {
            InitializeComponent();
        }

        private void Frm_01_Recipt_Cheques_Group_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            dtPersonActive = new DataTable();
            dtPersonAll = new DataTable();
            dtPersonActive = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
            dtPersonAll = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");

            //gridEXFieldChooserControl1.GridEX = gridEX_Turn;
            //gridEXFieldChooserControl2.GridEX = gridEX1;
            gridEX1.DropDowns["Cashier"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"select Table_045_PersonScope.Column01 as ColumnId,Table_045_PersonInfo.Column01 as Column01
            ,Table_045_PersonInfo.Column02 as Column02 from Table_045_PersonScope 
            INNER Join Table_045_PersonInfo On Table_045_PersonInfo.ColumnId=Table_045_PersonScope.Column01 
            where Table_045_PersonScope.Column02=13"), "");
            DataTable BankBoxTable = ClDoc.ReturnTable(ConBank, "Select ColumnId,Column02,Column01,Column35 from Table_020_BankCashAccInfo  where    Column01=1  AND  'True'='" + isadmin.ToString() + @"' or Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "') ");
            gridEX1.DropDowns["ToBank"].SetDataBinding(BankBoxTable, "");
            gridEX_Turn.DropDowns["ToBank"].SetDataBinding(BankBoxTable, "");

            gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
            gridEX_Turn.DropDowns["Person"].SetDataBinding(dtPersonAll, "");

            DataTable BankTable = ClDoc.ReturnTable(ConBank, "Select * from Table_010_BankNames");
            gridEX1.DropDowns["Banks"].SetDataBinding(BankTable, "");

            DataTable StatusTable = ClDoc.ReturnTable(ConBank, "Select ColumnId,Column01,Column02 from Table_060_ChequeStatus where Column01=0");
            gridEX1.DropDowns["Status"].SetDataBinding(StatusTable, "");
            gridEX_Turn.DropDowns["Status"].SetDataBinding(StatusTable, "");

            DataTable ProjectTable = ClDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");
            gridEX1.DropDowns["Project"].SetDataBinding(ProjectTable, "");
            gridEX_Turn.DropDowns["Project"].SetDataBinding(ProjectTable, "");

            gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Turn.DropDowns["Header"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select ACC_Code,ACC_Name from AllHeaders()"), "");

            DataTable CurrencyTable = ClDoc.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_Turn.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            gridEX1.DropDowns["Province"].SetDataBinding(ClDoc.ReturnTable(ConBase, "Select Column00,Column01 from Table_060_ProvinceInfo"), "");
            gridEX1.DropDowns["City"].SetDataBinding(ClDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_065_CityInfo"), "");

            gridEX_Turn.DropDowns["Center"].SetDataBinding(ClDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo"), "");
        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                gridEX1.SetValue("Column42", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column43", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());

                if (gridEX1.GetValue("Column05").ToString() == "0" || gridEX1.GetValue("Column05").ToString() == "")
                {
                    MessageBox.Show("امکان ثبت مبلغ صفر وجود ندارد");
                    e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.Key == "Column05")
            {
                if (gridEX1.GetValue("Column05").ToString() == "0" || gridEX1.GetValue("Column05").ToString() == "")
                {
                    MessageBox.Show("امکان ثبت مبلغ صفر وجود ندارد");

                    return;
                }
            }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
            }
            catch
            { }

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
                if (e.Column.Key == "Column11")
                {
                    Janus.Windows.GridEX.GridEXColumn filterColumn = gridEX1.RootTable.Columns["Column12"].DropDown.Columns["Column00"];
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(
                        filterColumn, Janus.Windows.GridEX.ConditionOperator.Equal, gridEX1.GetValue("Column11"));
                    gridEX1.RootTable.Columns["Column12"].DropDown.ApplyFilter(filter);
                    gridEX1.SetValue("Column12", DBNull.Value);
                }
            }
            catch
            {
            }
            //try
            //{
            //    if (e.Column.Key == "Column03")
            //    {
            //        if (gridEX1.GetRows().Length > 0)
            //        {
            //            Janus.Windows.GridEX.GridEXRow LastRow = gridEX1.GetRow(gridEX1.LastVisibleRow(false));

            //            gridEX1.SetValue("Column01", (LastRow.Cells["Column01"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column01"].Value.ToString()));
            //            gridEX1.SetValue("Column48", (LastRow.Cells["Column48"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column48"].Value.ToString()));
            //            gridEX1.SetValue("Column46", (LastRow.Cells["Column46"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column46"].Value.ToString()));
            //            gridEX1.SetValue("Column04", (LastRow.Cells["Column04"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column04"].Value.ToString()));
            //            gridEX1.SetValue("Column08", (LastRow.Cells["Column08"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column08"].Value.ToString()));
            //            gridEX1.SetValue("Column09", (LastRow.Cells["Column09"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column09"].Value.ToString()));
            //            gridEX1.SetValue("Column10", (LastRow.Cells["Column10"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column10"].Value.ToString()));
            //            gridEX1.SetValue("Column11", (LastRow.Cells["Column11"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column11"].Value.ToString()));
            //            gridEX1.SetValue("Column12", (LastRow.Cells["Column12"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column12"].Value.ToString()));
            //            gridEX1.SetValue("Column07", (LastRow.Cells["Column07"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column07"].Value.ToString()));
            //            gridEX1.SetValue("Column15", (LastRow.Cells["Column15"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column15"].Value.ToString()));
            //            gridEX1.SetValue("Column06", (LastRow.Cells["Column06"].Text.Trim() == "" ? (object)DBNull.Value : LastRow.Cells["Column06"].Value.ToString()));

            //        }
            //    }
            //}
            //catch
            //{
            //}
            try
            {
                if (e.Column.Key == "Column52")
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                    DateTime SecDate = BaseDate.AddDays(double.Parse(gridEX1.GetValue("Column52").ToString()));
                    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                    fa.SelectedDateTime = SecDate;
                    fa.UpdateTextValue();

                    gridEX1.SetValue("Column53", fa.Text);
                }
                else if (e.Column.Key == "Column53")
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                    DateTime SecDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column53").ToString())).ToShortDateString());
                    TimeSpan Sub = SecDate - BaseDate;
                    gridEX1.SetValue("Column52", Sub.Days);
                }
            }
            catch
            {
            }
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
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
            try
            {
                if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text);

            }
            catch
            {
            }
            gridEX1.CurrentCellDroppedDown = true;
            gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellValueChanged_1(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
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
            try
            {
                if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text);

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
            gridEX1.CurrentCellDroppedDown = true;
            gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_EditingCell(object sender, Janus.Windows.GridEX.EditingCellEventArgs e)
        {
            try
            {
                if (ClDoc.HasTurn_Rec(int.Parse(gridEX1.GetValue("ColumnId").ToString())) && e.Column.Key != "Selector")
                    e.Cancel = true;

            }
            catch
            {
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
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
                            _03_Bank.Frm_01_new_Recipt_Cheques frms = (_03_Bank.Frm_01_new_Recipt_Cheques)item;
                            frms.btn_Search_Click(sender, e);
                            return;
                        }
                    }
                    try
                    {
                        _03_Bank.Frm_01_new_Recipt_Cheques frm = new _03_Bank.Frm_01_new_Recipt_Cheques(
                            UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 28),
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
            catch { }
        }

        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.Row == -1)
                    gridEX1.RootTable.Columns["Column02"].DefaultValue = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");

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
                if (e.Column.Key == "Column02" && e.Value.ToString() != "")
                {
                    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                    try
                    {
                        fa.SelectedDateTime = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString())).ToShortDateString());
                        UpdateEfficientDate();
                    }
                    catch
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch
            {
            }

            try
            {
                if (e.Column.Key == "Column04" && e.Value.ToString() != "")
                {
                    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                    try
                    {
                        fa.SelectedDateTime = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column04").ToString())).ToShortDateString());
                        UpdateEfficientDate();
                    }
                    catch
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch
            {
            }
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

        private void gridEX_Turn_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {

                dataSet_01_Cash.EnforceConstraints = false;
                table_035_ReceiptChequesTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_035_ReceiptCheques where ColumnId=0", ConBank);
                dataSet_01_Cash.Table_035_ReceiptCheques.Clear();
                table_035_ReceiptChequesTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_035_ReceiptCheques");
                table_065_TurnReceptionTableAdapter.Adapter.SelectCommand = new SqlCommand("Select * from Table_065_TurnReception where Column01=0", ConBank);
                dataSet_01_Cash.Table_065_TurnReception.Clear();
                table_065_TurnReceptionTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_065_TurnReception");
                dataSet_01_Cash.EnforceConstraints = true;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }



        private void SaveEvent(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                gridEX1.UpdateData();
                this.table_035_ReceiptChequesBindingSource.Filter = "Column49=1";
                if (table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    foreach (DataRowView item in table_035_ReceiptChequesBindingSource)
                    {
                        if (item["Column50"].ToString().Trim() == "" || Convert.ToDouble(item["Column51"].ToString()) <= 0)
                        {
                            this.table_035_ReceiptChequesBindingSource.RemoveFilter();
                            throw new Exception("نوع ارز و ارزش ارز مربوط به دریافتهای ارزی را مشخص کنید");
                        }

                        if (item["Column05"].ToString() == "0")
                            throw new Exception("امکان ثبت مبلغ صفر وجود ندارد");
                    }
                }
                else
                {
                    table_035_ReceiptChequesBindingSource.RemoveFilter();
                    foreach (DataRowView item in table_035_ReceiptChequesBindingSource)
                    {


                        if (item["Column05"].ToString() == "0")
                            throw new Exception("امکان ثبت مبلغ صفر وجود ندارد");
                    }
                }

                table_035_ReceiptChequesBindingSource.RemoveFilter();
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                this.table_035_ReceiptChequesBindingSource.Filter = "Column00 <0";
                foreach (DataRowView item in table_035_ReceiptChequesBindingSource)
                {
                    item["Column00"] = ClDoc.SuggetstBackNumber();
                    this.table_035_ReceiptChequesBindingSource.EndEdit();
                    this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                }
                
                table_035_ReceiptChequesBindingSource.RemoveFilter();
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                //if (sender == bt_Save || sender == this)
                ////Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);

            
              _03_Bank. Form08_TotalDoc_Receive frm = new _03_Bank.Form08_TotalDoc_Receive();
              frm.Show();
                FillGridexList();
                gridEX1.UpdateData();
            }
        }

        private void SaveEvent1(object sender, EventArgs e)
        {
            if (this.table_035_ReceiptChequesBindingSource.Count > 0)
            {
                gridEX1.UpdateData();
                this.table_035_ReceiptChequesBindingSource.Filter = "Column49=1";
                if (table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    foreach (DataRowView item in table_035_ReceiptChequesBindingSource)
                    {
                        if (item["Column50"].ToString().Trim() == "" || Convert.ToDouble(item["Column51"].ToString()) <= 0)
                        {
                            this.table_035_ReceiptChequesBindingSource.RemoveFilter();
                            throw new Exception("نوع ارز و ارزش ارز مربوط به دریافتهای ارزی را مشخص کنید");
                        }

                        if (item["Column05"].ToString() == "0")
                            throw new Exception("امکان ثبت مبلغ صفر وجود ندارد");
                    }
                }
                else
                {
                    table_035_ReceiptChequesBindingSource.RemoveFilter();
                    foreach (DataRowView item in table_035_ReceiptChequesBindingSource)
                    {


                        if (item["Column05"].ToString() == "0")
                            throw new Exception("امکان ثبت مبلغ صفر وجود ندارد");
                    }
                }

                table_035_ReceiptChequesBindingSource.RemoveFilter();
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                this.table_035_ReceiptChequesBindingSource.Filter = "Column00 <0";
                foreach (DataRowView item in table_035_ReceiptChequesBindingSource)
                {
                    item["Column00"] = ClDoc.SuggetstBackNumber();
                    this.table_035_ReceiptChequesBindingSource.EndEdit();
                    this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                }

                table_035_ReceiptChequesBindingSource.RemoveFilter();
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                //if (sender == bt_Save || sender == this)
                ////Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);


                //_03_Bank.Form08_TotalDoc_Receive frm = new _03_Bank.Form08_TotalDoc_Receive();
                //frm.Show();
                //FillGridexList();
                gridEX1.UpdateData();
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEvent(sender, e);
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

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 117))
            {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    //حذف تکی
                    #region
                    if (gridEX1.GetCheckedRows().Length == 0)
                    {
                        try
                        {
                            string _Message = "در صورت حذف این برگه، سند حسابداری و گردشهای مربوطه حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این برگه هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(_Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                SqlDataAdapter SelectAdapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" +
                                    gridEX1.GetValue("ColumnId").ToString(), ConBank);
                                DataTable TurnRows = new DataTable();
                                SelectAdapter.Fill(TurnRows);

                                foreach (DataRow item in TurnRows.Rows)
                                {
                                    if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(item["Column13"].ToString()), int.Parse(item["ColumnId"].ToString()), 28) != 28)
                                    {
                                        ClDoc.IsFinal_ID(int.Parse(item["Column13"].ToString()));
                                        ClDoc.DeleteDetail_ID(int.Parse(item["Column13"].ToString()),
                                             short.Parse(item["Column02"].ToString()), int.Parse(item["ColumnId"].ToString()));
                                        ClDoc.DeleteTurnReception(long.Parse(item["ColumnId"].ToString()));
                                    }
                                    else throw new Exception("به علت صدور این برگه از قسمت تسویه فاکتورها، حذف آن امکانپذیر نمی باشد");
                                }
                                
                                table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(gridEX1.GetValue("Columnid").ToString()));
                                //ClDoc.RunSqlCommand(ConBank.ConnectionString, "Delete from Table_035_ReceiptCheques where ColumnId=" +
                                //     gridEX1.GetValue("ColumnId").ToString());
                                this.table_035_ReceiptChequesBindingSource.RemoveCurrent();
                                this.table_035_ReceiptChequesBindingSource.EndEdit();
                                this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);

                                Class_BasicOperation.ShowMsg("", "حذف برگه انجام شد", Class_BasicOperation.MessageType.Information);
                                FillGridexList();


                            }

                        }
                        catch (Exception ex)
                        {
                            FillGridexList();
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                        }
                    }
                    #endregion

                    //حذف گروهی
                    #region
                    else
                    {
                        try
                        {
                            string _Message = "در صورت حذف این برگه، سند حسابداری و گردشهای مربوطه حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این برگه هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(_Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetCheckedRows())
                                {
                                    SqlDataAdapter SelectAdapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" +
                                        item.Cells["ColumnId"].Value.ToString(), ConBank);
                                    DataTable TurnRows = new DataTable();
                                    SelectAdapter.Fill(TurnRows);

                                    foreach (DataRow Childitem in TurnRows.Rows)
                                    {
                                        if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(Childitem["Column13"].ToString()),
                                            int.Parse(Childitem["ColumnId"].ToString()), 28) != 28)
                                        {
                                            ClDoc.IsFinal_ID(int.Parse(Childitem["Column13"].ToString()));
                                            ClDoc.DeleteDetail_ID(int.Parse(Childitem["Column13"].ToString()),
                                                 short.Parse(Childitem["Column02"].ToString()), int.Parse(Childitem["ColumnId"].ToString()));
                                            ClDoc.DeleteTurnReception(long.Parse(Childitem["ColumnId"].ToString()));
                                        }
                                        else throw new Exception("به علت صدور این برگه از قسمت تسویه فاکتورها، حذف آن امکانپذیر نمی باشد");
                                    }

                                    //ClDoc.RunSqlCommand(ConBank.ConnectionString, "Delete from Table_035_ReceiptCheques where ColumnId=" +item.Cells["ColumnId"].Value.ToString());

                                    table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(item.Cells["ColumnId"].Value.ToString()));
                                    item.Delete();
                                    this.table_035_ReceiptChequesBindingSource.EndEdit();
                                    this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                                }
                                Class_BasicOperation.ShowMsg("", "حذف برگه ها با موفقیت صورت گرفت", Class_BasicOperation.MessageType.Information);
                                FillGridexList();



                            }

                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            FillGridexList();
                        }
                    }
                    #endregion
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف برگه را ندارید", Class_BasicOperation.MessageType.Warning);
        }



        private void FillGridexList()
        {
            List<string> Codes = new List<string>();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Codes.Add(item.Cells["ColumnId"].Value.ToString());
            }
            if (Codes.Count > 0)
            {
                gridEX1.MoveToNewRecord();
                dataSet_01_Cash.EnforceConstraints = false;
                table_035_ReceiptChequesTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_035_ReceiptCheques  where ColumnId in (" + string.Join(",", Codes.ToArray()) + ")", ConBank);
                dataSet_01_Cash.Table_040_CashPayments.Clear();
                table_035_ReceiptChequesTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_035_ReceiptCheques");
                table_035_ReceiptChequesTableAdapter.Adapter.Update(dataSet_01_Cash, "Table_035_ReceiptCheques");
                
                table_065_TurnReceptionTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_065_TurnReception where Column01 in (Select ColumnId from Table_035_ReceiptCheques where ColumnId IN (" + string.Join(",", Codes.ToArray()) + "))", ConBank);
                dataSet_01_Cash.Table_065_TurnReception.Clear();
                table_065_TurnReceptionTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_065_TurnReception");
                dataSet_01_Cash.EnforceConstraints = true;
            }
            gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 119))
            {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    //حذف تکی
                    #region

                    if (gridEX_Turn.GetCheckedRows().Length > 0)
                    {
                        try
                        {
                            string _Message = "در صورت حذف گردش، سند حسابداری مربوطه نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف گردشهای انتخاب شده هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(_Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Turn.GetCheckedRows())
                                {
                                    if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(item.Cells["Column13"].Value.ToString()), int.Parse(item.Cells["ColumnId"].Value.ToString()), 28) != 28)
                                    {
                                        ClDoc.IsFinal_ID(int.Parse(item.Cells["Column13"].Value.ToString()));
                                        ClDoc.DeleteDetail_ID(int.Parse(item.Cells["Column13"].Value.ToString()),
                                             short.Parse(item.Cells["Column02"].Value.ToString()), int.Parse(item.Cells["ColumnId"].Value.ToString()));
                                        ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString()));
                                    }
                                    else throw new Exception("به علت صدور این برگه از قسمت تسویه فاکتورها، حذف آن امکانپذیر نمی باشد");
                                }
                                FillGridexList();
                                Class_BasicOperation.ShowMsg("", "حذف گردشهای انتخاب شده انجام شد", Class_BasicOperation.MessageType.Information);
                            }

                        }
                        catch (Exception ex)
                        {
                            FillGridexList();
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                        }
                    }
                }
                    #endregion


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف برگه را ندارید", Class_BasicOperation.MessageType.Warning);
        }

        private void bt_DeleteAllTruns_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 120))
            {
                if (this.table_035_ReceiptChequesBindingSource.Count > 0)
                {
                    //حذف تکی
                    #region

                    if (gridEX1.GetCheckedRows().Length > 0)
                    {
                        try
                        {
                            string _Message = "در صورت حذف گردش، سند حسابداری مربوطه نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف گردشهای انتخاب شده هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(_Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetCheckedRows())
                                {
                                    gridEX1.MoveTo(Row);
                                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Turn.GetRows())
                                    {
                                        if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(item.Cells["Column13"].Value.ToString()), int.Parse(item.Cells["ColumnId"].Value.ToString()), 28) != 28)
                                        {
                                            ClDoc.IsFinal_ID(int.Parse(item.Cells["Column13"].Value.ToString()));
                                            ClDoc.DeleteDetail_ID(int.Parse(item.Cells["Column13"].Value.ToString()),
                                                 short.Parse(item.Cells["Column02"].Value.ToString()), int.Parse(item.Cells["ColumnId"].Value.ToString()));
                                            ClDoc.DeleteTurnReception(long.Parse(item.Cells["ColumnId"].Value.ToString()));
                                        }
                                        else throw new Exception("به علت صدور برگه " + Row.Cells["ColumnId"].Value.ToString() + "  از قسمت تسویه فاکتورها، حذف آن امکانپذیر نمی باشد");
                                    }
                                }
                                FillGridexList();
                                Class_BasicOperation.ShowMsg("", "حذف گردشهای انتخاب شده انجام شد", Class_BasicOperation.MessageType.Information);
                            }

                        }
                        catch (Exception ex)
                        {
                            FillGridexList();
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                        }
                    }
                }
                    #endregion


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف برگه را ندارید", Class_BasicOperation.MessageType.Warning);
        }

        private void bt_Export_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEvent1(sender, e);
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 118))
                {
                    Form08_TotalDoc_Receive frm = new Form08_TotalDoc_Receive();
                    frm.ShowDialog();
                    FillGridexList();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
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
                    if (gridEX1.GetCheckedRows().Length == 0)
                    {
                        DataTable Table = dataSet_01_Cash.Rpt_PrintRecChqs.Clone();
                        Janus.Windows.GridEX.GridEXRow item = gridEX1.GetRow();

                        if (!item.Cells["ColumnId"].Value.ToString().StartsWith("-"))
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
                                item.Cells["Column01"].Text.Trim());
                            PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq frm = new PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq(Table, item.Cells["ColumnId"].Value.ToString());
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        DataTable Table = dataSet_01_Cash.Rpt_PrintRecChqs.Clone();
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetCheckedRows())
                        {
                            if (!item.Cells["ColumnId"].Value.ToString().StartsWith("-"))
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
                                item.Cells["Column01"].Text.Trim());
                            }
                        }
                        PACNT._3_Cheque_Operation.Reports.Form01_PrintRecChq frm = new PACNT._3_Cheque_Operation.Reports. Form01_PrintRecChq(Table, "0");
                        frm.ShowDialog();
                    }
                }
                catch
                {
                }
            }
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            try
            {


                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
               


                    if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
                    {
                        if (!txt_Search.Text.Trim().Contains(',') && !txt_Search.Text.Trim().Contains('-'))
                        {

                    DataTable user = ClDoc.ReturnTable(ConBank, @"select Column42 from Table_035_ReceiptCheques where Column00 in(" + txt_Search.Text.TrimEnd() + ")");

                            if (isadmin)
                            {
                                dataSet_01_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, int.Parse(txt_Search.Text.Trim()));
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(txt_Search.Text.Trim()));
                                dataSet_01_Cash.EnforceConstraints = true;
                                if (this.table_035_ReceiptChequesBindingSource.Count == 0)
                                    throw new Exception("شماره برگه وارد شده نامعتبر می باشد");
                            }
                            else if (user.Rows[0][0] == Class_BasicOperation._UserName)
                            {
                                dataSet_01_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, int.Parse(txt_Search.Text.Trim()));
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(txt_Search.Text.Trim()));
                                dataSet_01_Cash.EnforceConstraints = true;
                                if (this.table_035_ReceiptChequesBindingSource.Count == 0)
                                    throw new Exception("شماره برگه وارد شده نامعتبر می باشد");
                            }
                        }
                        else if (txt_Search.Text.Trim().Contains(','))
                        {

                            try
                            {
                                DataTable user1 = ClDoc.ReturnTable(ConBank, @"select Column42 from Table_035_ReceiptCheques where Columnid in(" + txt_Search.Text.TrimEnd() + ")");

                                foreach (DataRow row in user1.Rows)
                                {
                                    if (isadmin)
                                    {
                                        dataSet_01_Cash.EnforceConstraints = false;
                                        table_035_ReceiptChequesTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_035_ReceiptCheques where ColumnId in (" + txt_Search.Text + ")", ConBank);
                                        dataSet_01_Cash.Table_035_ReceiptCheques.Clear();
                                        table_035_ReceiptChequesTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_035_ReceiptCheques");
                                        table_065_TurnReceptionTableAdapter.Adapter.SelectCommand = new SqlCommand("Select * from Table_065_TurnReception where Column01 in (Select ColumnId from Table_035_ReceiptCheques where ColumnId in (" + txt_Search.Text + "))", ConBank);
                                        dataSet_01_Cash.Table_065_TurnReception.Clear();
                                        table_065_TurnReceptionTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_065_TurnReception");
                                        dataSet_01_Cash.EnforceConstraints = true;
                                    }
                                    else if (row["Column42"].ToString() == Class_BasicOperation._UserName)
                                    {
                                        dataSet_01_Cash.EnforceConstraints = false;
                                        table_035_ReceiptChequesTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_035_ReceiptCheques where ColumnId in (" + txt_Search.Text + ")  and  Column42 in('" + Class_BasicOperation._UserName + "')", ConBank);
                                        dataSet_01_Cash.Table_035_ReceiptCheques.Clear();
                                        table_035_ReceiptChequesTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_035_ReceiptCheques");
                                        table_065_TurnReceptionTableAdapter.Adapter.SelectCommand = new SqlCommand("Select * from Table_065_TurnReception where Column01 in (Select ColumnId from Table_035_ReceiptCheques where ColumnId in (" + txt_Search.Text + ")) AND  Column16 in('" + Class_BasicOperation._UserName + "')", ConBank);
                                        dataSet_01_Cash.Table_065_TurnReception.Clear();
                                        table_065_TurnReceptionTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_065_TurnReception");
                                        dataSet_01_Cash.EnforceConstraints = true;
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                if (ex.Message.StartsWith("Incorrect syntax near"))
                                    Class_BasicOperation.ShowMsg("", "اطلاعات مورد جستجو، صحیح نمی باشند", Class_BasicOperation.MessageType.Warning);
                                else Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            }

                        }
                        else if (txt_Search.Text.Trim().Contains('-'))
                        {
                            string s = txt_Search.Text.Trim().Replace("-", " and ");

                            if (s != "")
                            {

                                DataTable user2 = ClDoc.ReturnTable(ConBank, @"select Column42 from Table_035_ReceiptCheques where ColumnId between " + s + "");



                                try
                                {
                                    foreach (DataRow row1 in user2.Rows)
                                    {
                                        if (isadmin)
                                        {
                                            dataSet_01_Cash.EnforceConstraints = false;
                                            table_035_ReceiptChequesTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_035_ReceiptCheques  where ColumnId between " + s, ConBank);
                                            dataSet_01_Cash.Table_035_ReceiptCheques.Clear();
                                            table_035_ReceiptChequesTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_035_ReceiptCheques");

                                            table_065_TurnReceptionTableAdapter.Adapter.SelectCommand = new SqlCommand("Select * from Table_065_TurnReception where Column01 in (Select ColumnId from Table_035_ReceiptCheques where ColumnId between " + s + ")  ", ConBank);
                                            dataSet_01_Cash.Table_065_TurnReception.Clear();
                                            table_065_TurnReceptionTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_065_TurnReception");

                                            dataSet_01_Cash.EnforceConstraints = true;
                                        }
                                        else if (row1["Column42"].ToString() == Class_BasicOperation._UserName)
                                        {
                                            //string s = txt_Search.Text.Trim().Replace("-", " and ");
                                            dataSet_01_Cash.EnforceConstraints = false;
                                            table_035_ReceiptChequesTableAdapter.Adapter.SelectCommand = new SqlCommand(@"Select * from Table_035_ReceiptCheques  where ColumnId between " + s + " and  Column42 in('" + Class_BasicOperation._UserName + "')", ConBank);
                                            dataSet_01_Cash.Table_035_ReceiptCheques.Clear();
                                            table_035_ReceiptChequesTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_035_ReceiptCheques");

                                            table_065_TurnReceptionTableAdapter.Adapter.SelectCommand = new SqlCommand("Select * from Table_065_TurnReception where Column01 in (Select ColumnId from Table_035_ReceiptCheques where ColumnId between " + s + ") AND  Column16 in('" + Class_BasicOperation._UserName + "') ", ConBank);
                                            dataSet_01_Cash.Table_065_TurnReception.Clear();
                                            table_065_TurnReceptionTableAdapter.Adapter.Fill(dataSet_01_Cash, "Table_065_TurnReception");

                                            dataSet_01_Cash.EnforceConstraints = true;
                                        }
                                    }
                                }


                                catch (Exception ex)
                                {

                                    if (ex.Message.StartsWith("Incorrect syntax near"))
                                        Class_BasicOperation.ShowMsg("", "اطلاعات مورد جستجو، صحیح نمی باشند", Class_BasicOperation.MessageType.Warning);
                                    else Class_BasicOperation.CheckExceptionType(ex, this.Name);
                                }

                            }
                        }
                    }
            }
         
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                txt_Search.SelectAll();

            }
        }

        private void txt_Search_Enter(object sender, EventArgs e)
        {
           
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar) && e.KeyChar != 44 && e.KeyChar != 1608 && e.KeyChar != 45)
                e.Handled = true;
            if (txt_Search.Text.Contains(",") && e.KeyChar == 45)
                e.Handled = true;
            if (txt_Search.Text.Contains("-") && (e.KeyChar == 44 || e.KeyChar == 1608))
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                bt_Search_Click(sender, e);
                txt_Search.SelectAll();
            }
            else if (e.KeyChar == 1608)
                e.KeyChar = ',';
        }

        private void uiPanel0_Click(object sender, EventArgs e)
        {

        }

       

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void bt_Update_Click(object sender, EventArgs e)
        {
            if (gridEX1.RowCount>0)
            {
                bt_Search_Click(sender, e);
                
            }
            
        }

      

      

       

    }
}
