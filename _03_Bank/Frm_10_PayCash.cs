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
    public partial class Frm_10_PayCash : Form
    {
        bool _Del = false, _Export = false, _DelDoc = false, _BackSpace = false, _New = false;
        int _PaperNumber = 0;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
      
        SqlDataAdapter PeopleAdapter, ProjectAdapter, HeadersAdapter, BanksAdapter, CenterAdapters, RequestAdapter, DocAdapter;
        DataTable dtPersonAll, dtPersonActive;

        public Frm_10_PayCash(bool Del, bool Export, bool DelDoc, int PaperNumber)
        {
            InitializeComponent();
            _Del = Del;
            _Export = Export;
            _DelDoc = DelDoc;
            _PaperNumber = PaperNumber;
        }

        private void Frm_10_PayCash_Load(object sender, EventArgs e)
        {
           
                foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
                {
                    col.CellStyle.BackColor = SystemColors.Window;
                    if (col.Key == "Column20" || col.Key == "Column22")
                        col.DefaultValue = Class_BasicOperation._UserName;
                    if (col.Key == "Column21" || col.Key == "Column23")
                        col.DefaultValue = Class_BasicOperation.ServerDate();
                }
                dtPersonActive = new DataTable();
                dtPersonAll = new DataTable();
                dtPersonActive = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeople(3)");
                dtPersonAll = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeopleInActive(3)");

                //btnTime2.Checked = Properties.Settings.Default.EffTime2
                //   ;
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                gridEX1.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");

                //gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
                gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonAll, "");

                ProjectAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
                ProjectAdapter.Fill(dataSet1, "Projects");
                gridEX1.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Projects"], "");

                CenterAdapters = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
                CenterAdapters.Fill(dataSet1, "Centers");
                gridEX1.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Centers"], "");

                HeadersAdapter = new SqlDataAdapter("Select ACC_Code,ACC_Name from AllHeaders()", ConAcnt);
                HeadersAdapter.Fill(dataSet1, "Headers");
                gridEX1.DropDowns["Headers"].SetDataBinding(dataSet1.Tables["Headers"], "");

              


                if (isadmin)
                {
                    BanksAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo", ConBank);
                    BanksAdapter.Fill(dataSet1, "Banks1");
                    BanksAdapter.Fill(dataSet1, "Banks2");
                    gridEX1.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
                    gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");
                }
                else
                {
                    BanksAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')", ConBank);
                    BanksAdapter.Fill(dataSet1, "Banks1");
                    BanksAdapter.Fill(dataSet1, "Banks2");
                    gridEX1.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
                    gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");
                }






                RequestAdapter = new SqlDataAdapter("Select Table_050_PaymentRequests.*,Table_020_BankCashAccInfo.Column02 as BoxName from Table_050_PaymentRequests INNER JOIN Table_020_BankCashAccInfo On Table_020_BankCashAccInfo.ColumnId=Table_050_PaymentRequests.Column06  where Table_050_PaymentRequests.Column11=1 and Table_050_PaymentRequests.Column04=1 and Table_050_PaymentRequests.Column13=0", ConBank);
                RequestAdapter.Fill(dataSet1, "Req");
                gridEX1.DropDowns["RequestList"].SetDataBinding(dataSet1.Tables["Req"], "");

                DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
                DocAdapter.Fill(dataSet1, "Doc");
                gridEX1.DropDowns["Doc"].SetDataBinding(dataSet1.Tables["Doc"], "");

                gridEX1.DropDowns["Currency"].SetDataBinding(ClDoc.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo"), "");

                gridEX1.DropDowns["DefaultDes"].SetDataBinding(ClDoc.ReturnTable(ConAcnt, "Select Column00 from Table_055_DefaultDescription"), "");

                if (_PaperNumber != 0)
                {
                    this.table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, _PaperNumber);
                    if (this.table_040_CashPaymentsBindingSource.Count > 0)
                    {
                        this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);

                    }
                }
            
        }

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "Frm_10_PayCash");
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column09")
                    if (gridEX1.GetValue("Column02").ToString() == gridEX1.GetValue("Column09").ToString())
                        gridEX1.SetValue("Column09", DBNull.Value);

                if (e.Column.Key == "Column14")
                {
                    try
                    {
                        if (gridEX1.GetValue("Column14").ToString().Trim() != "")
                        {
                            string[] _AccInfo = ClDoc.ACC_Info(gridEX1.GetValue("Column14").ToString().Trim());
                            gridEX1.SetValue("Column15", _AccInfo[0]);
                            gridEX1.SetValue("Column16", (_AccInfo[1].ToString() == "" ? (object)DBNull.Value : _AccInfo[1].ToString()));
                            gridEX1.SetValue("Column17", (_AccInfo[2].ToString() == "" ? (object)DBNull.Value : _AccInfo[2].ToString()));
                            gridEX1.SetValue("Column18", (_AccInfo[3].ToString() == "" ? (object)DBNull.Value : _AccInfo[3].ToString()));
                            gridEX1.SetValue("Column19", (_AccInfo[4].ToString() == "" ? (object)DBNull.Value : _AccInfo[4].ToString()));
                        }
                    }
                    catch
                    {
                        gridEX1.SetValue("Column15", DBNull.Value);
                        gridEX1.SetValue("Column16", DBNull.Value);
                        gridEX1.SetValue("Column17", DBNull.Value);
                        gridEX1.SetValue("Column18", DBNull.Value);
                        gridEX1.SetValue("Column19", DBNull.Value);
                    }

                }
                if (e.Column.Key == "Column26")
                {
                    object Value = gridEX1.GetValue("Column26").ToString();
                    DataRowView Row = (DataRowView)gridEX1.RootTable.Columns["Column26"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column27", Row["Column02"].ToString());
                }

                if (Convert.ToDouble(gridEX1.GetValue("Column27").ToString()) > 0 && gridEX1.GetValue("Column25").ToString() == "True")
                {
                    if (e.Column.Key == "Riali")
                    {
                        gridEX1.SetValue("Column04", Convert.ToDouble(gridEX1.GetValue("Riali").ToString()) /
                            Convert.ToDouble(gridEX1.GetValue("Column27").ToString()));
                    }
                    else if (e.Column.Key == "Column04")
                    {
                        gridEX1.SetValue("Riali", Convert.ToDouble(gridEX1.GetValue("Column04").ToString()) *
                            Convert.ToDouble(gridEX1.GetValue("Column27").ToString()));
                    }
                    else if (e.Column.Key == "Column27")
                    {
                        gridEX1.SetValue("Riali", Convert.ToDouble(gridEX1.GetValue("Column04").ToString()) *
                         Convert.ToDouble(gridEX1.GetValue("Column27").ToString()));
                        gridEX1.SetValue("Column04", Convert.ToDouble(gridEX1.GetValue("Riali").ToString()) /
                            Convert.ToDouble(gridEX1.GetValue("Column27").ToString()));

                    }
                }
            }
            catch
            {
            }

            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column06");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column09");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column14");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column08");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column05");
            }
            catch
            {
            }

        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;
            gridEX1.SetValue("Column22", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column23", Class_BasicOperation.ServerDate());
            try
            {
                if (e.Column.Key == "Column12" && gridEX1.GetValue("Column12").ToString().Trim() != "")
                {
                    DataRow[] Row = dataSet1.Tables["Req"].Select("ColumnId=" + gridEX1.GetValue("Column12").ToString().Trim());
                    if (Row.Length > 0)
                    {
                        
                        gridEX1.SetValue("Column02", Row[0]["Column06"]);
                        gridEX1.SetValue("Column03", Row[0]["Column01"]);
                        gridEX1.SetValue("Column04", Row[0]["Column12"]);
                        gridEX1.SetValue("Column05", Row[0]["Column03"]);
                        gridEX1.SetValue("Column06", Row[0]["Column07"]);
                        gridEX1.SetValue("Column07", Row[0]["Column09"]);
                        gridEX1.SetValue("Column08", Row[0]["Column10"]);
                        gridEX1.SetValue("Column09", Row[0]["Column08"]);
                        gridEX1.SetValue("Column13", Row[0]["Column14"]);
                        gridEX1.SetValue("Column14", Row[0]["Column21"]);
                        gridEX1.SetValue("Column15", Row[0]["Column22"]);
                        gridEX1.SetValue("Column16", Row[0]["Column23"]);
                        gridEX1.SetValue("Column17", Row[0]["Column24"]);
                        gridEX1.SetValue("Column18", Row[0]["Column25"]);
                        gridEX1.SetValue("Column19", Row[0]["Column26"]);

                        gridEX1.RootTable.Columns["Column02"].EditType = EditType.NoEdit;
                        gridEX1.RootTable.Columns["Column03"].Selectable = false;
                        gridEX1.RootTable.Columns["Column04"].Selectable = false;
                        gridEX1.RootTable.Columns["Column05"].Selectable = false;
                        gridEX1.RootTable.Columns["Column06"].Selectable = false;
                        gridEX1.RootTable.Columns["Column07"].Selectable = false;
                        gridEX1.RootTable.Columns["Column08"].Selectable = false;
                        gridEX1.RootTable.Columns["Column09"].Selectable = false;
                        gridEX1.RootTable.Columns["Column13"].Selectable = false;
                        gridEX1.RootTable.Columns["Column14"].Selectable = false;
                        gridEX1.RootTable.Columns["Column15"].Selectable = false;
                        gridEX1.RootTable.Columns["Column16"].Selectable = false;
                        gridEX1.RootTable.Columns["Column17"].Selectable = false;
                        gridEX1.RootTable.Columns["Column18"].Selectable = false;
                        gridEX1.RootTable.Columns["Column19"].Selectable = false;
                    }
                }

            }
            catch
            {
                ResetColumns();
            }
            try
            {
                if (e.Column.Key == "Column02")
                {
                    if (gridEX1.GetValue("Column02").ToString().Trim() != "")
                        if (gridEX1.DropDowns["FromBank"].GetValue("Column01").ToString() == "True" || gridEX1.DropDowns["FromBank"].GetValue("Column01").ToString() == "False")
                            gridEX1.SetValue("Column24", gridEX1.DropDowns["FromBank"].GetValue("Column35").ToString());
                        else
                            gridEX1.SetValue("Column24", DBNull.Value);
                }

                if (e.Column.Key == "Column14" && gridEX1.GetValue("Column09").ToString().Trim() != "")
                    gridEX1.SetValue("Column14", DBNull.Value);
                if (e.Column.Key == "Column09" && gridEX1.GetValue("Column14").ToString().Trim() != "")
                    gridEX1.SetValue("Column09", DBNull.Value);

                if (e.Column.Key == "Column25")
                {
                    if (gridEX1.GetValue("Column25").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column26"].Selectable = true;
                        gridEX1.RootTable.Columns["Column27"].Selectable = true;
                        gridEX1.RootTable.Columns["Riali"].Selectable = true;
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column28"].Selectable = false;
                        gridEX1.RootTable.Columns["Column27"].Selectable = false;
                        gridEX1.RootTable.Columns["Riali"].Selectable = true;

                    }


                }

            }
            catch { }

            try
            {
                //فیلتر بانک پرداخت کننده
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", null, "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
            try
            {
                //شخص
                if (e.Column.Key == "Column06")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column06", "Column01", "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }

            try
            {
                //بانک دریافت کننده
                if (e.Column.Key == "Column09")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column09", null, "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }

            try
            {
                //حساب
                if (e.Column.Key == "Column14")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column14", "ACC_Code", "ACC_Name", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
            try
            {
                //مرکز
                if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
            try
            {
                //پروژه
                if (e.Column.Key == "Column08")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column08", "Column01", "Column02", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
            try
            {
                //شرح پیش فرش
                if (e.Column.Key == "Column05")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column05", null, "Column00", gridEX1.EditTextBox.Text);
            }
            catch
            {
            }
        }
        private void ResetColumns()
        {
            gridEX1.RootTable.Columns["Column02"].EditType = EditType.MultiColumnCombo;
            gridEX1.RootTable.Columns["Column03"].Selectable = true;
            gridEX1.RootTable.Columns["Column04"].Selectable = true;
            gridEX1.RootTable.Columns["Column05"].Selectable = true;
            gridEX1.RootTable.Columns["Column06"].Selectable = true;
            gridEX1.RootTable.Columns["Column07"].Selectable = true;
            gridEX1.RootTable.Columns["Column08"].Selectable = true;
            gridEX1.RootTable.Columns["Column09"].Selectable = true;
            gridEX1.RootTable.Columns["Column13"].Selectable = true;
            gridEX1.RootTable.Columns["Column14"].Selectable = true;
            gridEX1.RootTable.Columns["Column15"].Selectable = true;
            gridEX1.RootTable.Columns["Column16"].Selectable = true;
            gridEX1.RootTable.Columns["Column17"].Selectable = true;
            gridEX1.RootTable.Columns["Column18"].Selectable = true;
            gridEX1.RootTable.Columns["Column19"].Selectable = true;
        }

        private void table_040_CashPaymentsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            if (this.table_040_CashPaymentsBindingSource.Count > 0)
            {
                try
                {

                    if (((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column10"].ToString().Trim() != "0"
                        || ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column12"].ToString().Trim() != "")
                    {
                        gridEX1.Enabled = false;
                    }
                    else
                    {
                        gridEX1.Enabled = true;
                    }
                    superTabItem1.Text = "اطلاعات برگه شماره  " + gridEX1.GetValue("Column01").ToString();
                }
                catch
                {

                }


                if (_New == true)
                {

                    //gridEX1.DropDowns["Person"].SetDataBinding(dtPersonActive, "");
                    gridEX1.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");
                    gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonActive, "");

                    _New = false;
                }
                else
                {
                    //gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
                    gridEX1.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");
                    gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonAll, "");
                }


            }
            else
                superTabItem1.Text = "اطلاعات برگه ";
        }

        private void gridEX1_CurrentCellChanging(object sender, CurrentCellChangingEventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column18" || gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column19")
                    gridEX1.EnterKeyBehavior = EnterKeyBehavior.None;
                else gridEX1.EnterKeyBehavior = EnterKeyBehavior.NextCell;

                //if (gridEX1.Col == 3)
                //{
                //    if (gridEX1.GetValue("Column12").ToString().Trim() != "")
                //        gridEX1.EnterKeyBehavior = EnterKeyBehavior.None;
                //    else
                //        gridEX1.EnterKeyBehavior = EnterKeyBehavior.NextCell;
                //}
            }
            catch
            {
            }
        }

        private void gridEX1_RowEditCanceled(object sender, RowActionEventArgs e)
        {
            gridEX1.Enabled = false;
            bt_New.Enabled = true;
            gridEX1.MoveToNewRecord();
            superTabItem1.Text = "اطلاعات برگه ";
        }

        private void gridEX1_UpdatingCell(object sender, UpdatingCellEventArgs e)
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
                if (e.Column.Key == "Column03" && e.Value.ToString() != "")
                {
                    FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                    try
                    {
                        fa.SelectedDateTime = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column03").ToString())).ToShortDateString());
                        //UpdateEfficientDate();
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

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                gridEX1.Enabled = true;
                this.table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, 0);
                gridEX1.MoveToNewRecord();
                superTabItem1.Text = "اطلاعات برگه ";
                gridEX1.SetValue("Column03", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                gridEX1.SetValue("Column20", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column21", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column22", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column23", Class_BasicOperation.ServerDate());
                gridEX1.RootTable.Columns["Column26"].Selectable = false;
                gridEX1.RootTable.Columns["Column27"].Selectable = false;
                gridEX1.RootTable.Columns["Riali"].Selectable = false;
                ResetColumns();
                gridEX1.Select();
                gridEX1.Col = 1;
                bt_New.Enabled = false;

                //if (btnTime2.Checked == false)
                //{
                //    gridEX1.SetValue("Column29", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                //}
                gridEX1.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");
                //gridEX1.DropDowns["Person"].SetDataBinding(dtPersonActive, "");
                gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonActive, "");

                _New = true;



            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form06_PayCash");
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (((DataRowView)table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
                {
                    MessageBox.Show("برای این برگه سند صدور شده است امکان ذخیره مجدد ندارید");
                    return;
                }
                SaveEvent(sender);
            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, "Form06_PayCash");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form06_PayCash");
            }
        }

        private void SaveEvent(object sender)
        {
            if (this.table_040_CashPaymentsBindingSource.Count > 0)
            {
                gridEX1.UpdateData();

                bool Valid = true;
                try
                {
                    FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column03").ToString()));
                    if (gridEX1.GetValue("Column29").ToString() != "") { FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column29").ToString())); }
                }
                catch { Valid = false; }
                if (!Valid) throw new Exception("تاریخ پرداخت/ تاریخ موثر نامعتبر است");


                try
                {
                    if (Convert.ToInt32(gridEX1.GetValue("Column04")) == 0)

                        throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
                }
                catch
                {
                    if ((gridEX1.GetValue("Column04").ToString() == ""))

                        throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
                }

                if (((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                    gridEX1.SetValue("Column01", ClDoc.SuggestPayPaperNumber());
                this.table_040_CashPaymentsBindingSource.EndEdit();

                if (((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column06"].ToString() == "" &&
                       ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column09"].ToString() == "" &&
                       ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column14"].ToString() == "")
                    throw new Exception("دریافت کننده مشخص نگشته است");

                if (gridEX1.GetValue("Column25").ToString() == "True")
                {
                    if (gridEX1.GetRow().Cells["Column26"].Text.Trim() == "" ||
                        Convert.ToDouble(gridEX1.GetValue("Column27").ToString()) <= 0)
                        throw new Exception("اطلاعات مربوط به ارز را مشخص کنید");
                }

                //SqlCommand Mande = new SqlCommand("Select ISNULL(SUM(Column11),0)-ISNULL(SUM(Column12),0) as Mande from Table_065_SanadDetail where Column01='" + ClDoc.ExScalar(ConBank, "Table_020_BankCashAccInfo", "Column12", "ColumnId", ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column02"].ToString()) + "'", ConAcnt);
                //if (Int64.Parse(Mande.ExecuteScalar().ToString()) < Int64.Parse(((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column04"].ToString()))
                //    throw new Exception("موجودی صندوق/بانک پرداخت کننده کافی نمی باشد");

                
                this.table_040_CashPaymentsTableAdapter.Update(dataSet_01_Cash.Table_040_CashPayments);
                _03_Bank.Form07_ExportDocForPaidCash frm = new Form07_ExportDocForPaidCash(int.Parse(gridEX1.GetValue("ColumnId").ToString()), gridEX1.GetRow().Cells["Column02"].Text);
                frm.ShowDialog();
                if (sender == bt_Save || sender == this)
                    //Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
         
                superTabItem1.Text = "اطلاعات برگه شماره " + ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString();
                table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, int.Parse(((DataRowView)table_040_CashPaymentsBindingSource.CurrencyManager.Current)["columnId"].ToString()));
               
                if (gridEX1.GetValue("Column12").ToString().Trim() != "")
                {
                    ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_050_PaymentRequests", "Column13", "ColumnId", int.Parse(gridEX1.GetValue("Column12").ToString()), int.Parse(gridEX1.GetValue("ColumnId").ToString()));
                    gridEX1.Enabled = false;
                }
                else
                    ResetColumns();
                dataSet1.Tables["Req"].Clear();
                RequestAdapter.Fill(dataSet1, "Req");
                bt_New.Enabled = true;
            }
        }

        private void SaveEvent_1(object sender)
        {
            if (this.table_040_CashPaymentsBindingSource.Count > 0)
            {
                gridEX1.UpdateData();

                bool Valid = true;
                try
                {
                    FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column03").ToString()));
                    if (gridEX1.GetValue("Column29").ToString() != "") { FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column29").ToString())); }
                }
                catch { Valid = false; }
                if (!Valid) throw new Exception("تاریخ پرداخت/ تاریخ موثر نامعتبر است");


                try
                {
                    if (Convert.ToInt32(gridEX1.GetValue("Column04")) == 0)

                        throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
                }
                catch
                {
                    if ((gridEX1.GetValue("Column04").ToString() == ""))

                        throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
                }

                if (((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                    gridEX1.SetValue("Column01", ClDoc.SuggestPayPaperNumber());
                this.table_040_CashPaymentsBindingSource.EndEdit();

                if (((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column06"].ToString() == "" &&
                       ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column09"].ToString() == "" &&
                       ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column14"].ToString() == "")
                    throw new Exception("دریافت کننده مشخص نگشته است");

                if (gridEX1.GetValue("Column25").ToString() == "True")
                {
                    if (gridEX1.GetRow().Cells["Column26"].Text.Trim() == "" ||
                        Convert.ToDouble(gridEX1.GetValue("Column27").ToString()) <= 0)
                        throw new Exception("اطلاعات مربوط به ارز را مشخص کنید");
                }

                //SqlCommand Mande = new SqlCommand("Select ISNULL(SUM(Column11),0)-ISNULL(SUM(Column12),0) as Mande from Table_065_SanadDetail where Column01='" + ClDoc.ExScalar(ConBank, "Table_020_BankCashAccInfo", "Column12", "ColumnId", ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column02"].ToString()) + "'", ConAcnt);
                //if (Int64.Parse(Mande.ExecuteScalar().ToString()) < Int64.Parse(((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column04"].ToString()))
                //    throw new Exception("موجودی صندوق/بانک پرداخت کننده کافی نمی باشد");


                this.table_040_CashPaymentsTableAdapter.Update(dataSet_01_Cash.Table_040_CashPayments);
                //_03_Bank.Form07_ExportDocForPaidCash frm = new Form07_ExportDocForPaidCash(int.Parse(gridEX1.GetValue("ColumnId").ToString()), gridEX1.GetRow().Cells["Column02"].Text);
                //frm.ShowDialog();
                if (sender == bt_Save || sender == this)
                    Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);

                superTabItem1.Text = "اطلاعات برگه شماره " + ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString();
                table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, int.Parse(((DataRowView)table_040_CashPaymentsBindingSource.CurrencyManager.Current)["columnId"].ToString()));

                if (gridEX1.GetValue("Column12").ToString().Trim() != "")
                {
                    ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_050_PaymentRequests", "Column13", "ColumnId", int.Parse(gridEX1.GetValue("Column12").ToString()), int.Parse(gridEX1.GetValue("ColumnId").ToString()));
                    gridEX1.Enabled = false;
                }
                else
                    ResetColumns();
                dataSet1.Tables["Req"].Clear();
                RequestAdapter.Fill(dataSet1, "Req");
                bt_New.Enabled = true;
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 88))
            {
            if (_Del)
            {
               
                    int PaperId = int.Parse(gridEX1.GetValue("ColumnId").ToString());
                    if (this.table_040_CashPaymentsBindingSource.Count > 0 && gridEX1.RowCount > 0)
                    {
                        try
                        {
                            //this.table_040_CashPaymentsBindingSource.EndEdit();
                            string Message = "آیا مایل به حذف برگه ثبت شده هستید؟";
                            if (gridEX1.GetValue("Column10").ToString() != "0")
                                Message = "برای این برگه، سند حسابداری صادر شده است. در صورت حذف، ثبت مربوطه نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف برگه هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                if (gridEX1.GetValue("Column10").ToString() != "0")
                                {
                                    //if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()), PaperId, 44) == 44 || ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()), PaperId, 46) == 46)
                                    if ((ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=18 and column17=" + PaperId + "),0) as Result")).Rows[0][0].ToString() != "0")
                                        throw new Exception("این برگه از زیر سیستم های دیگر صادر شده است. جهت حذف از سیستم مربوط اقدام نمایید");

                                    else
                                    {
                                        ClDoc.IsFinal(int.Parse(gridEX1.GetRow().Cells["Column10"].Text.ToString()));

                                        ClDoc.DeleteDetail_ID(int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()), 18, int.Parse(gridEX1.GetValue("ColumnId").ToString()));
                                    }
                                    Message = "حذف برگه و ثبت حسابداری مربوط، انجام گرفت";
                                }
                                else
                                    Message = "حذف برگه انجام گرفت";
                                int _ReqID = 0;
                                try
                                {
                                    _ReqID = int.Parse(gridEX1.GetValue("Column12").ToString());
                                }
                                catch { }
                                this.table_040_CashPaymentsBindingSource.RemoveCurrent();
                                this.table_040_CashPaymentsBindingSource.EndEdit();
                                this.table_040_CashPaymentsTableAdapter.Update(dataSet_01_Cash.Table_040_CashPayments);
                                if (_ReqID != 0)
                                    ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_050_PaymentRequests", "Column13", "ColumnId", _ReqID, 0);
                                this.table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, 0);
                                Class_BasicOperation.ShowMsg("", Message, Class_BasicOperation.MessageType.Information);
                                bt_New.Enabled = true;
                            }

                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                        }
                    }
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف برگه را ندارید", Class_BasicOperation.MessageType.Warning);
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                string user = ClDoc.ExScalar(ConBank.ConnectionString, @"select column20 from Table_040_CashPayments where column01 =" + txt_Search.Text + " ");
                //gridEX1.MoveToNewRecord();
                //this.table_040_CashPaymentsBindingSource.EndEdit();
                if (dataSet_01_Cash.Table_040_CashPayments.GetChanges(DataRowState.Modified) != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }
                bt_New.Enabled = true;
                if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
                {
                     
                
                    int _ID = 0;
                    try
                    {
                        _ID = int.Parse(ClDoc.ExScalar(ConBank.ConnectionString, "Table_040_CashPayments", "ColumnId", "Column01", txt_Search.Text.Trim()));
                    }
                    catch
                    {
                    }
                    if (isadmin)
                    {
                      

                    this.table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, _ID);
                    if (this.table_040_CashPaymentsBindingSource.Count > 0)
                    {
                        this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);

                    }

                    }
                    else if (user == Class_BasicOperation._UserName)
                    {
                        this.table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, _ID);

                 


                    if (this.table_040_CashPaymentsBindingSource.Count > 0)
                    {
                        this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);
                        
                    }
                    }
                    else
                    {
                        gridEX1.Enabled = false;
                        throw new Exception("شماره برگه وارد شده نامعتبر می باشد");
                    }

                }
            }
            catch { }
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

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_040_CashPaymentsBindingSource.EndEdit();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                if (dataSet_01_Cash.Table_040_CashPayments.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent_1(sender);
                       
                    }
                }

                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(Column01) from Table_040_CashPayments),0) as Row");
                    
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(Column01) from Table_040_CashPayments where Column20='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConBank, "Select ColumnId from Table_040_CashPayments where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_040_CashPaymentsTableAdapter.FillByID(this.dataSet_01_Cash.Table_040_CashPayments, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_040_CashPaymentsBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_040_CashPaymentsBindingSource.EndEdit();
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    if (dataSet_01_Cash.Table_040_CashPayments.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent_1(sender);
                        }
                    }
                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(Column01) from Table_040_CashPayments where Column01>" + ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                        
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(Column01) from Table_040_CashPayments where Column01>" + ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND Column20='" + Class_BasicOperation._UserName + "'),0) as Row");

                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = ClDoc.ReturnTable(ConBank, "Select ColumnId from Table_040_CashPayments where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_040_CashPaymentsTableAdapter.FillByID(this.dataSet_01_Cash.Table_040_CashPayments, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);
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
            if (this.table_040_CashPaymentsBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_040_CashPaymentsBindingSource.EndEdit();
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    if (dataSet_01_Cash.Table_040_CashPayments.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent_1(sender);
                        }
                    }

                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                       "Select ISNULL((Select max(Column01) from Table_040_CashPayments where Column01<" +
                       ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                       "Select ISNULL((Select max(Column01) from Table_040_CashPayments where Column01<" +
                       ((DataRowView)this.table_040_CashPaymentsBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND column20='" + Class_BasicOperation._UserName + "'),0) as Row");
                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = ClDoc.ReturnTable(ConBank, "Select ColumnId from Table_040_CashPayments where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_040_CashPaymentsTableAdapter.FillByID(this.dataSet_01_Cash.Table_040_CashPayments, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);
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
                gridEX1.UpdateData();
                table_040_CashPaymentsBindingSource.EndEdit();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                if (dataSet_01_Cash.Table_040_CashPayments.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent_1(sender);
                    }
                }
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(Column01) from Table_040_CashPayments),0) as Row");
                    
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(Column01) from Table_040_CashPayments where column20='" + Class_BasicOperation._UserName + "'),0) as Row");

                }

                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConBank, "Select ColumnId from Table_040_CashPayments where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_040_CashPaymentsTableAdapter.FillByID(this.dataSet_01_Cash.Table_040_CashPayments, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 89))
                {
                    if (this.table_040_CashPaymentsBindingSource.Count > 0 && gridEX1.RowCount > 0 && gridEX1.GetValue("Column10").ToString() != "0")
                    {

                        if (_DelDoc)
                        {
                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف ثبت حسابداری مربوط به این برگه هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                int PaperId = int.Parse(gridEX1.GetValue("ColumnId").ToString());

                                //if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()), PaperId, 44) == 44 || ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()), PaperId, 46) == 46)
                                if ((ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=18 and column17=" + PaperId + "),0) as Result")).Rows[0][0].ToString() != "0" ||
                                    (ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Columnid from Table_065_SanadDetail where Column16=46 and column17=" + PaperId + "),0) as Result")).Rows[0][0].ToString() != "0" ||
                                    (ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Columnid from Table_065_SanadDetail where Column16=90 and column17=" + PaperId + "),0) as Result")).Rows[0][0].ToString() != "0" ||
                                    (ClDoc.ReturnTable(ConAcnt, @"select isnull((select top(1) Columnid from Table_065_SanadDetail where Column16=87 and column17=" + PaperId + "),0) as Result")).Rows[0][0].ToString() != "0")
                                    throw new Exception("این برگه از زیر سیستم های دیگر صادر شده است. جهت حذف از سیستم مربوط اقدام نمایید");
                                else
                                {
                                    ClDoc.IsFinal(int.Parse(gridEX1.GetRow().Cells["Column10"].Text.ToString()));
                                    ClDoc.DeleteDetail_ID(int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()), 18, PaperId);
                                    ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_040_CashPayments", "Column10", "ColumnId", PaperId, 0);
                                    this.table_040_CashPaymentsTableAdapter.FillByID(dataSet_01_Cash.Table_040_CashPayments, PaperId);
                                    if (this.table_040_CashPaymentsBindingSource.Count > 0)
                                    {
                                        this.table_040_CashPaymentsBindingSource_PositionChanged(sender, e);

                                    }
                                    Class_BasicOperation.ShowMsg("", "حذف ثبت حسابداری مربوط به این برگه با موفقیت انجام گرفت", Class_BasicOperation.MessageType.Information);
                                }
                            }
                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف ثبت مربوط به این برگه را ندارید", Class_BasicOperation.MessageType.Warning);
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

        private void bt_Print_Click(object sender, EventArgs e)
        {


             try
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
                PACNT._4_Reports.DataSet_Reports ds = new PACNT._4_Reports.DataSet_Reports();
            Janus.Windows.GridEX.GridEXRow Row = gridEX1.GetRow();
            DataTable Table = ds.Rpt_PrintPayCash.Clone();
            Table.Rows.Add(Row.Cells["Column03"].Value.ToString(),
            Row.Cells["Column01"].Value.ToString(),
            (Row.Cells["Column25"].Value.ToString() == "False" ? Row.Cells["Column04"].Text.ToString() : Row.Cells["Riali"].Text.ToString()),
            FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble
            ((Row.Cells["Column25"].Value.ToString() == "False" ? Row.Cells["Column04"].Text.ToString() : Row.Cells["Riali"].Text.ToString())))),
            Row.Cells["Column05"].Text.ToString(),
            Row.Cells["Column02"].Text,
            (Row.Cells["Column06"].Text == "" ? Row.Cells["Column09"].Text : Row.Cells["Column06"].Text)
            + (Row.Cells["Column14"].Text.Trim() != "" ? "/" +
            Row.Cells["Column14"].Text : ""));
            PACNT._2_Cash_Operation.Reports.Form2_PrintPaidCash frm = new PACNT._2_Cash_Operation.Reports.Form2_PrintPaidCash(Table);
            frm.ShowDialog();
            gridEX1.UnCheckAllRecords();
            }
             catch { }
        }
        
        //private void UpdateEfficientDate()
        //{
        //    try
        //    {
        //        if (gridEX1.GetRow().Cells["Column28"].Text.Trim() != "")
        //        {
        //            if (btnTime2.Checked == true)
        //            {
        //                DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column03").ToString())).ToShortDateString());
        //                DateTime SecDate = BaseDate.AddDays(double.Parse(gridEX1.GetValue("Column28").ToString()));
        //                FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
        //                fa.SelectedDateTime = SecDate;
        //                fa.UpdateTextValue();

        //                gridEX1.SetValue("Column29", fa.Text);
        //            }
        //        }
        //        else if (gridEX1.GetRow().Cells["Column29"].Text.Trim() != "")
        //        {
        //            DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column03").ToString())).ToShortDateString());
        //            DateTime SecDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column29").ToString())).ToShortDateString());
        //            TimeSpan Sub = SecDate - BaseDate;
        //            gridEX1.SetValue("Column28", Sub.Days);
        //        }
        //    }
        //    catch { }
        //}
    }
}
