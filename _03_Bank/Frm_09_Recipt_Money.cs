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
    public partial class Frm_09_Recipt_Money : Form
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
        SqlDataAdapter ProjectAdapter, HeadersAdapter, BanksAdapter, DocAdapter;
        DataTable dtPersonAll, dtPersonActive;
        bool _Del = false, _Export = false, _DelDoc = false, _BackSpace = false, _New = false;
        int _PaperNumber = 0;

        public Frm_09_Recipt_Money(bool Del, bool Export, bool DelDoc, int PaperNumber)
        {
            InitializeComponent();
            _Del = Del;
            _Export = Export;
            _DelDoc = DelDoc;
            _PaperNumber = PaperNumber;
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX2_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column05");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column11");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column06");
            }
            catch
            {
            }

            try
            {
                if (e.Column.Key == "Column07")
                    if (gridEX2.GetValue("Column01").ToString() == gridEX2.GetValue("Column07").ToString())
                        gridEX2.SetValue("Column07", DBNull.Value);

                if (e.Column.Key == "Column11")
                {
                    try
                    {
                        if (gridEX2.GetValue("Column11").ToString().Trim() != "")
                        {
                            string[] _AccInfo = ClDoc.ACC_Info(gridEX2.GetValue("Column11").ToString().Trim());
                            gridEX2.SetValue("Column12", _AccInfo[0]);
                            gridEX2.SetValue("Column13", (_AccInfo[1].ToString() == "" ? (object)DBNull.Value : _AccInfo[1].ToString()));
                            gridEX2.SetValue("Column14", (_AccInfo[2].ToString() == "" ? (object)DBNull.Value : _AccInfo[2].ToString()));
                            gridEX2.SetValue("Column15", (_AccInfo[3].ToString() == "" ? (object)DBNull.Value : _AccInfo[3].ToString()));
                            gridEX2.SetValue("Column16", (_AccInfo[4].ToString() == "" ? (object)DBNull.Value : _AccInfo[4].ToString()));
                        }
                    }
                    catch
                    {
                        gridEX2.SetValue("Column12", DBNull.Value);
                        gridEX2.SetValue("Column13", DBNull.Value);
                        gridEX2.SetValue("Column14", DBNull.Value);
                        gridEX2.SetValue("Column15", DBNull.Value);
                        gridEX2.SetValue("Column16", DBNull.Value);
                    }

                }


                if (e.Column.Key == "Column23")
                {
                    object Value = gridEX2.GetValue("Column23");
                    DataRowView Row = (DataRowView)gridEX2.RootTable.Columns["Column23"].DropDown.FindItem(Value);
                    gridEX2.SetValue("Column24", Row["Column02"].ToString());
                }

                if (Convert.ToDouble(gridEX2.GetValue("Column24").ToString()) > 0 && gridEX2.GetValue("Column22").ToString() == "True")
                {
                    if (e.Column.Key == "Riali")
                    {
                        gridEX2.SetValue("Column03", Convert.ToDouble(gridEX2.GetValue("Riali").ToString()) /
                            Convert.ToDouble(gridEX2.GetValue("Column24").ToString()));
                    }
                    else if (e.Column.Key == "Column03")
                    {
                        gridEX2.SetValue("Riali", Convert.ToDouble(gridEX2.GetValue("Column03").ToString()) *
                            Convert.ToDouble(gridEX2.GetValue("Column24").ToString()));
                    }
                    else if (e.Column.Key == "Column24")
                    {
                        gridEX2.SetValue("Riali", Convert.ToDouble(gridEX2.GetValue("Column03").ToString()) *
                         Convert.ToDouble(gridEX2.GetValue("Column24").ToString()));
                        gridEX2.SetValue("Column03", Convert.ToDouble(gridEX2.GetValue("Riali").ToString()) /
                            Convert.ToDouble(gridEX2.GetValue("Column24").ToString()));

                    }
                }



            }
            catch
            {
            }

            try
            {

                if (e.Column.Key == "Column25")
                {
                    if (btnTime.Checked == true)
                    {
                        DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString())).ToShortDateString());
                        DateTime SecDate = BaseDate.AddDays(double.Parse(gridEX2.GetValue("Column25").ToString()));
                        FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                        fa.SelectedDateTime = SecDate;
                        fa.UpdateTextValue();

                        gridEX2.SetValue("Column26", fa.Text);
                    }
                }
                else if (e.Column.Key == "Column26")
                {
                    //DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString())).ToShortDateString());
                    //DateTime SecDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column26").ToString())).ToShortDateString());
                    //TimeSpan Sub = SecDate - BaseDate;
                    //gridEX2.SetValue("Column25", Sub.Days);
                }
            }
            catch
            {
            }
        }



        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;
            gridEX2.SetValue("Column19", Class_BasicOperation._UserName);
            gridEX2.SetValue("Column20", Class_BasicOperation.ServerDate());

            if (e.Column.Key == "Column22")
            {
                if (gridEX2.GetValue("Column22").ToString() == "True")
                {
                    gridEX2.RootTable.Columns["Column23"].Selectable = true;
                    gridEX2.RootTable.Columns["Column24"].Selectable = true;
                    gridEX2.RootTable.Columns["Riali"].Selectable = true;
                }
                else
                {
                    gridEX2.RootTable.Columns["Column23"].Selectable = false;
                    gridEX2.RootTable.Columns["Column24"].Selectable = false;
                    gridEX2.RootTable.Columns["Riali"].Selectable = false;
                }
            }

            if (e.Column.Key == "Column01")
            {
                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);


                    if (gridEX2.GetValue("Column01").ToString().Trim() != "")
                        if (gridEX2.DropDowns["ToBank"].GetValue("Column01").ToString() == "True" || gridEX2.DropDowns["ToBank"].GetValue("Column01").ToString() == "False")
                            gridEX2.SetValue("Column21", gridEX2.DropDowns["ToBank"].GetValue("Column35").ToString());
                        else
                            gridEX2.SetValue("Column21", DBNull.Value);

                }


                catch { }
            }

            try
            {
                if (e.Column.Key == "Column11" && gridEX2.GetValue("Column07").ToString().Trim() != "")
                    gridEX2.SetValue("Column11", DBNull.Value);
                if (e.Column.Key == "Column07" && gridEX2.GetValue("Column11").ToString().Trim() != "")
                    gridEX2.SetValue("Column07", DBNull.Value);





            }
            catch { }

            //فیلتر بانک دریافت کننده
            try
            {
                if (e.Column.Key == "Column01")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column01", null, "Column02", gridEX2.EditTextBox.Text);
            }
            catch
            {
            }

            //فیلتر شخص پرداخت کننده
            try
            {
                if (e.Column.Key == "Column05")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column05", "Column01", "Column02", gridEX2.EditTextBox.Text);
            }
            catch
            {
            }
            //فیلتر بانک پرداخت کننده
            try
            {
                if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", null, "Column02", gridEX2.EditTextBox.Text);
            }
            catch
            {
            }
            //فیلتر حساب پرداخت کننده
            try
            {
                if (e.Column.Key == "Column11")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column11", "ACC_Code", "ACC_Name", gridEX2.EditTextBox.Text);
            }
            catch
            {
            }
            //فیلتر پروژه
            try
            {
                if (e.Column.Key == "Column06")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column06", "Column01", "Column02", gridEX2.EditTextBox.Text);
            }
            catch
            {
            }
            try
            {

                if (btnTime.Checked == false)
                {
                    if (e.Column.Key == "Column02")
                        gridEX2.SetValue("Column26", gridEX2.GetValue("Column02"));
                }
            }
            catch { }
        }

        private void Frm_09_Recipt_Money_Load(object sender, EventArgs e)
        {
            foreach (GridEXColumn col in this.gridEX2.RootTable.Columns)
            {
                if (col.Key == "Column17" || col.Key == "Column19")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column18" || col.Key == "Column20")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }
            dtPersonActive = new DataTable();
            dtPersonAll = new DataTable();
            dtPersonActive = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeople(3)");
            dtPersonAll = ClDoc.ReturnTable(ConBase, @"Select ColumnId,Column01,Column02 from ListPeopleInActive(3)");
            btnTime.Checked = Properties.Settings.Default.EffTime1;
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            gridEX2.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");
            gridEX2.DropDowns["Cashier"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"select Table_045_PersonScope.Column01 as ColumnId,
            Table_045_PersonInfo.Column01 as Column01,Table_045_PersonInfo.Column02 as Column02 
            from Table_045_PersonScope INNER Join Table_045_PersonInfo On Table_045_PersonInfo.ColumnId=
            Table_045_PersonScope.Column01 where Table_045_PersonScope.Column02=13"), "");

            ProjectAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            ProjectAdapter.Fill(dataSet1, "Projects");
            gridEX2.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Projects"], "");

            HeadersAdapter = new SqlDataAdapter("Select ACC_Code,ACC_Name from AllHeaders()", ConACNT);
            HeadersAdapter.Fill(dataSet1, "Headers");
            gridEX2.DropDowns["Headers"].SetDataBinding(dataSet1.Tables["Headers"], "");



            if (isadmin)
            {
                BanksAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo  ", ConBank);
                BanksAdapter.Fill(dataSet1, "Banks1");
                BanksAdapter.Fill(dataSet1, "Banks2");
                gridEX2.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
                gridEX2.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");
            }
            else
            {
                BanksAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')", ConBank);
                BanksAdapter.Fill(dataSet1, "Banks1");
                BanksAdapter.Fill(dataSet1, "Banks2");
                gridEX2.DropDowns["FromBank"].SetDataBinding(dataSet1.Tables["Banks1"], "");
                gridEX2.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Banks2"], "");
            }
            //DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConACNT);
            //DocAdapter.Fill(dataSet1, "Doc");
            //gridEX2.DropDowns["Doc"].SetDataBinding(dataSet1.Tables["Doc"], "");

            gridEX2.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConACNT, @"Select ColumnId,Column00 from Table_060_SanadHead"), "");

            gridEX2.DropDowns["Currency"].SetDataBinding(ClDoc.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo"), "");
            if (_PaperNumber != 0)
            {
                this.table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, _PaperNumber);
                if (this.table_045_ReceiveCashBindingSource.Count > 0)
                {
                    //    if (this.gridEX2.GetValue("Column08").ToString().Trim() != "0")
                    //        gridEX2.SetValue("Column008", ClDoc.DocNum(ConAcnt, int.Parse(gridEX2.GetValue("Column08").ToString())));
                    //    else
                    //        gridEX2.SetValue("Column008", "0");
                    this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);

                }

            }

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                //gridEX2.AllowAddNew = InheritableBoolean.True;
                gridEX2.Enabled = true;
                this.table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, 0);
                gridEX2.MoveToNewRecord();
                superTabItem1.Text = "اطلاعات برگه ";
                gridEX2.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                gridEX2.SetValue("Column17", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column18", Class_BasicOperation.ServerDate());
                gridEX2.SetValue("Column19", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column20", Class_BasicOperation.ServerDate());
                gridEX2.RootTable.Columns["Column23"].Selectable = false;
                gridEX2.RootTable.Columns["Column24"].Selectable = false;
                gridEX2.RootTable.Columns["Riali"].Selectable = false;
                //gridEX2.SetValue("Column008", "0");
                if (btnTime.Checked == false)
                {
                    gridEX2.SetValue("Column26", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                }
                gridEX2.Select();
                gridEX2.Col = 1;
                btn_New.Enabled = false;
                _New = true;
                gridEX2.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void table_045_ReceiveCashBindingSource_PositionChanged(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            if (this.table_045_ReceiveCashBindingSource.Count > 0)
            {
                try
                {
                    if (gridEX2.GetValue("Column08").ToString().Trim() != "0")
                    {
                        gridEX2.Enabled = false;
                    }
                    else
                    {
                        gridEX2.Enabled = true;
                    }
                    superTabItem1.Text = "اطلاعات برگه شماره  " + gridEX2.GetValue("ColumnId").ToString();

                    //try
                    //{
                    //    //if (gridEX2.GetRow().Cells["Column22"].Value.ToString() == "True")
                    //    //{
                    //    //    gridEX2.GetRow().Cells["Riali"].Value = 20000;
                    //    //}
                    //    //if (gridEX2.GetValue("Column22").ToString() == "True")
                    //    //{
                    //    //    gridEX2.SetValue("Riali", Convert.ToDouble(gridEX2.GetValue("Column03").ToString()) *
                    //    //        Convert.ToDouble(gridEX2.GetValue("Column24").ToString()));
                    //    //}
                    //}
                    //catch
                    //{ }
                }
                catch { }

                if (_New == true)
                {
                    gridEX2.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')"), "");
                    _New = false;
                }


            }
            else
                superTabItem1.Text = "اطلاعات برگه ";
        }
        private void SaveEvent(object sender)
        {
            gridEX2.UpdateData();
            bool Valid = true;
            //string personhesab = ((DataRowView)gridEX2.RootTable.Columns["Column01"].DropDown.FindItem(gridEX2.GetValue("Column01")))["Column01"].ToString();
            //if (personhesab=="False")
            //{
            //    string Person = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column36 from Table_020_BankCashAccInfo where ColumnId = "+gridEX2.GetValue("Column01")+"");
            //    ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column21"] =Convert.ToInt32( Person).ToString();
            //}
            try
            {
                FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString()));
                if (gridEX2.GetValue("Column26").ToString() != "") { FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column26").ToString())); }
            }
            catch { Valid = false; }
            if (!Valid) throw new Exception("تاریخ دریافت/ تاریخ موثر نامعتبر است");
            try
            {
                if (Convert.ToInt32(gridEX2.GetValue("Column03")) == 0)

                    throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
            }
            catch
            {
                if ((gridEX2.GetValue("Column03").ToString() == ""))

                    throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
            }

            this.table_045_ReceiveCashBindingSource.EndEdit();
            if (((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column05"].ToString() == "" &&
                ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column07"].ToString() == "" &&
                ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column11"].ToString() == "")
                throw new Exception("پرداخت کننده را مشخص کنید");
            if (gridEX2.GetValue("Column22").ToString() == "True")
            {
                if (gridEX2.GetRow().Cells["Column23"].Text.Trim() == "" ||
                    Convert.ToDouble(gridEX2.GetValue("Column24").ToString()) <= 0)
                    throw new Exception("اطلاعات مربوط به ارز را مشخص کنید");
            }


            this.table_045_ReceiveCashBindingSource.EndEdit();
            this.table_045_ReceiveCashTableAdapter.Update(dataSet_01_Cash.Table_045_ReceiveCash);
            _03_Bank.Form04_ExportDocForReceive frm = new Form04_ExportDocForReceive(int.Parse(gridEX2.GetValue("ColumnId").ToString()), gridEX2.GetRow().Cells["Column01"].Text, gridEX2.GetRow().Cells["Column05"].Text);
            frm.ShowDialog();
            if (sender == btn_Save || sender == this)
                //Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
                superTabItem1.Text = "اطلاعات برگه شماره " + ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
            table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, int.Parse(((DataRowView)table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
            btn_New.Enabled = true;


        }
        private void SaveEvent_1(object sender)
        {
            gridEX2.UpdateData();
            bool Valid = true;
            try
            {
                FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString()));
                if (gridEX2.GetValue("Column26").ToString() != "") { FarsiLibrary.Utils.PersianDateConverter.ToGregorianDate(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column26").ToString())); }
            }
            catch { Valid = false; }
            if (!Valid) throw new Exception("تاریخ دریافت/ تاریخ موثر نامعتبر است");
            try
            {
                if (Convert.ToInt32(gridEX2.GetValue("Column03")) == 0)

                    throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
            }
            catch
            {
                if ((gridEX2.GetValue("Column03").ToString() == ""))

                    throw new Exception("امکان ثبت برگه با مبلغ صفر وجود ندارد.");
            }

            this.table_045_ReceiveCashBindingSource.EndEdit();
            if (((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column05"].ToString() == "" &&
                ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column07"].ToString() == "" &&
                ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column11"].ToString() == "")
                throw new Exception("پرداخت کننده را مشخص کنید");
            if (gridEX2.GetValue("Column22").ToString() == "True")
            {
                if (gridEX2.GetRow().Cells["Column23"].Text.Trim() == "" ||
                    Convert.ToDouble(gridEX2.GetValue("Column24").ToString()) <= 0)
                    throw new Exception("اطلاعات مربوط به ارز را مشخص کنید");
            }


            this.table_045_ReceiveCashBindingSource.EndEdit();
            this.table_045_ReceiveCashTableAdapter.Update(dataSet_01_Cash.Table_045_ReceiveCash);
            //_03_Bank.Form04_ExportDocForReceive frm = new Form04_ExportDocForReceive(int.Parse(gridEX2.GetValue("ColumnId").ToString()), gridEX2.GetRow().Cells["Column01"].Text, gridEX2.GetRow().Cells["Column05"].Text);
            //frm.ShowDialog();
            if (sender == btn_Save || sender == this)
                //Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
                superTabItem1.Text = "اطلاعات برگه شماره " + ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
            btn_New.Enabled = true;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (((DataRowView)table_045_ReceiveCashBindingSource.CurrencyManager.Current)["Column08"].ToString() != "0")
                {
                    MessageBox.Show("برای این برگه سند صدور شده است امکان ذخیره مجدد ندارید");
                    return;
                }
                SaveEvent(sender);
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

        private void gridEX2_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX2.RootTable.Columns[gridEX2.Col].Key == "Column17")
                    gridEX2.EnterKeyBehavior = EnterKeyBehavior.None;
                else gridEX2.EnterKeyBehavior = EnterKeyBehavior.NextCell;
            }
            catch
            {
            }
        }

        private void gridEX2_UpdatingCell(object sender, UpdatingCellEventArgs e)
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
                        fa.SelectedDateTime = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString())).ToShortDateString());
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
                if (gridEX2.GetRow().Cells["Column25"].Text.Trim() != "")
                {
                    if (btnTime.Checked == true)
                    {
                        DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString())).ToShortDateString());
                        DateTime SecDate = BaseDate.AddDays(double.Parse(gridEX2.GetValue("Column25").ToString()));
                        FarsiLibrary.Win.Controls.FADatePicker fa = new FarsiLibrary.Win.Controls.FADatePicker();
                        fa.SelectedDateTime = SecDate;
                        fa.UpdateTextValue();

                        gridEX2.SetValue("Column26", fa.Text);
                    }
                }
                else if (gridEX2.GetRow().Cells["Column26"].Text.Trim() != "")
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column02").ToString())).ToShortDateString());
                    DateTime SecDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(gridEX2.GetValue("Column26").ToString())).ToShortDateString());
                    TimeSpan Sub = SecDate - BaseDate;
                    gridEX2.SetValue("Column25", Sub.Days);
                }
            }
            catch { }
        }

        private void gridEX2_RowEditCanceled(object sender, RowActionEventArgs e)
        {
            gridEX2.Enabled = false;
            btn_New.Enabled = true;
            gridEX2.MoveToNewRecord();
            superTabItem1.Text = "اطلاعات برگه ";
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 85))
            {
                if (_Del)
                {
                    if (this.table_045_ReceiveCashBindingSource.Count > 0 && gridEX2.RowCount > 0)
                    {
                        try
                        {
                            //this.table_045_ReceiveCashBindingSource.EndEdit();
                            string Message = "آیا مایل به حذف برگه ثبت شده هستید؟";
                            if (gridEX2.GetValue("Column08").ToString() != "0")
                                Message = "برای این برگه، سند حسابداری صادر شده است. در صورت حذف، ثبت مربوطه نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف برگه هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                if (gridEX2.GetValue("Column08").ToString() != "0")
                                {
                                    ClDoc.IsFinal(int.Parse(gridEX2.GetRow().Cells["Column08"].Text.ToString()));
                                    // if (ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(gridEX2.GetRow().Cells["Column08"].Value.ToString()), int.Parse(gridEX2.GetValue("ColumnId").ToString()),24) != 24)
                                    if ((ClDoc.ReturnTable(ConACNT, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=16 and column17=" + int.Parse(gridEX2.GetValue("ColumnId").ToString()) + "),0) as Result")).Rows[0][0].ToString() == "0")
                                    {
                                        ClDoc.DeleteDetail_ID(int.Parse(gridEX2.GetRow().Cells["Column08"].Value.ToString()), 16
                                             , int.Parse(gridEX2.GetValue("ColumnId").ToString()));
                                        Message = "حذف برگه و ثبت حسابداری مربوط، انجام گرفت";
                                    }
                                    else throw new Exception("به علت صدور این برگه از زیر سیستم های دیگر، حذف  آن امکانپذیر نمی باشد. جهت حذف از قسمت مربوط اقدام کنید");
                                }
                                else
                                    Message = "حذف برگه انجام گرفت";

                                this.table_045_ReceiveCashBindingSource.RemoveCurrent();
                                this.table_045_ReceiveCashBindingSource.EndEdit();
                                this.table_045_ReceiveCashTableAdapter.Update(dataSet_01_Cash.Table_045_ReceiveCash);
                                this.table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, 0);
                                Class_BasicOperation.ShowMsg("", Message, Class_BasicOperation.MessageType.Information);
                                btn_New.Enabled = true;
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
        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                gridEX2.UpdateData();
                table_045_ReceiveCashBindingSource.EndEdit();

                if (dataSet_01_Cash.Table_045_ReceiveCash.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent_1(sender);
                    }
                }
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_045_ReceiveCash),0) as Row");

                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_045_ReceiveCash where Column17='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_045_ReceiveCashTableAdapter.FillByID(this.dataSet_01_Cash.Table_045_ReceiveCash, RowId);
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_045_ReceiveCashBindingSource.Count > 0)
            {

                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();

                    gridEX2.UpdateData();
                    table_045_ReceiveCashBindingSource.EndEdit();

                    if (dataSet_01_Cash.Table_045_ReceiveCash.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent_1(sender);

                        }
                    }
                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_045_ReceiveCash where ColumnId>" + ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");

                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_045_ReceiveCash where ColumnId>" + ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + " AND Column17='" + Class_BasicOperation._UserName + "'),0) as Row");

                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_045_ReceiveCashTableAdapter.FillByID(this.dataSet_01_Cash.Table_045_ReceiveCash, RowId);
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);
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
            if (this.table_045_ReceiveCashBindingSource.Count > 0)
            {
                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    gridEX2.UpdateData();
                    table_045_ReceiveCashBindingSource.EndEdit();

                    if (dataSet_01_Cash.Table_045_ReceiveCash.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent_1(sender);
                        }
                    }

                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                       "Select ISNULL((Select max(ColumnId) from Table_045_ReceiveCash where ColumnId<" +
                       ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    }

                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                       "Select ISNULL((Select max(ColumnId) from Table_045_ReceiveCash where ColumnId<" +
                       ((DataRowView)this.table_045_ReceiveCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + " AND column17='" + Class_BasicOperation._UserName + "'),0) as Row");
                    }

                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_045_ReceiveCashTableAdapter.FillByID(this.dataSet_01_Cash.Table_045_ReceiveCash, RowId);
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);
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
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                gridEX2.UpdateData();
                table_045_ReceiveCashBindingSource.EndEdit();

                if (dataSet_01_Cash.Table_045_ReceiveCash.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent_1(sender);
                    }
                }

                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_045_ReceiveCash),0) as Row");

                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_045_ReceiveCash where column17='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    int RowId = int.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_045_ReceiveCashTableAdapter.FillByID(this.dataSet_01_Cash.Table_045_ReceiveCash, RowId);
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        public void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX2.MoveToNewRecord();
                this.table_045_ReceiveCashBindingSource.EndEdit();
                if (dataSet_01_Cash.Table_045_ReceiveCash.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent_1(sender);
                    }
                }
                btn_New.Enabled = true;

                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                string user = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull ((select Column17 from Table_045_ReceiveCash where ColumnId =" + txt_Search.Text + "),0)");



                if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
                {
                    if (isadmin)
                    {
                        this.table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, int.Parse(txt_Search.Text.Trim()));

                        if (this.table_045_ReceiveCashBindingSource.Count > 0)
                        {

                            this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);

                        }


                    }

                    if (user == Class_BasicOperation._UserName)
                    {
                        this.table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, int.Parse(txt_Search.Text.Trim()));

                        if (this.table_045_ReceiveCashBindingSource.Count > 0)
                        {

                            this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);

                        }
                    }
                    else
                    {
                        gridEX2.Enabled = false;
                        throw new Exception("شماره برگه وارد شده نامعتبر می باشد");
                    }

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                txt_Search.SelectAll();
            }
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 86))
                {
                    if (this.table_045_ReceiveCashBindingSource.Count > 0 && gridEX2.RowCount > 0 && gridEX2.GetValue("Column08").ToString() != "0")
                    {
                        if (_DelDoc)
                        {
                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف ثبت حسابداری مربوط به این برگه هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                int PaperId = int.Parse(gridEX2.GetValue("ColumnId").ToString());
                                ClDoc.IsFinal(int.Parse(gridEX2.GetRow().Cells["Column08"].Text.ToString()));
                                if ((ClDoc.ReturnTable(ConACNT, @"select isnull((select top(1) Column27 from Table_065_SanadDetail where Column16=16 and column17=" + PaperId + "),0) as Result")).Rows[0][0].ToString() == "0")//ClDoc.SanadType(ConAcnt.ConnectionString, int.Parse(gridEX2.GetRow().Cells["Column08"].Value.ToString()), PaperId, 24) != 24)
                                {
                                    ClDoc.DeleteDetail_ID(int.Parse(gridEX2.GetRow().Cells["Column08"].Value.ToString()), 16, PaperId);
                                    ///
                                    #region DelDoc
                                    ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_045_ReceiveCash", "Column08", "ColumnId", PaperId, 0);
                                    this.table_045_ReceiveCashTableAdapter.FillByID(dataSet_01_Cash.Table_045_ReceiveCash, PaperId);
                                    if (this.table_045_ReceiveCashBindingSource.Count > 0)
                                    {

                                        this.table_045_ReceiveCashBindingSource_PositionChanged(sender, e);

                                    }
                                    Class_BasicOperation.ShowMsg("", "حذف ثبت حسابداری مربوط به این برگه با موفقیت انجام گرفت", Class_BasicOperation.MessageType.Information);
                                    #endregion
                                }
                                else
                                    throw new Exception("به علت صدور این برگه از زیر سیستم های دیگر، حذف سند آن امکانپذیر نمی باشد. جهت حذف از قسمت مربوط اقدام کنید");

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

                DataTable Table = dataSet_01_Cash.Rpt_PrintPayCash.Clone();
                Janus.Windows.GridEX.GridEXRow Row = gridEX2.GetRow();
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

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                btn_Search_Click(sender, e);
                txt_Search.SelectAll();
            }
        }

    }
}
