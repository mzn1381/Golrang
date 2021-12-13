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
    public partial class Frm_01_new_Recipt_Cheques : Form
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
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        SqlDataAdapter BoxAdapter, PersonAdapter, BanksAdapter, ProjectAdapter, ProvinceAdapter, CityAdapter, StatusAdapter;
        DataTable dtPersonAll, dtPersonActive;

        public Frm_01_new_Recipt_Cheques(bool Del, bool Export, bool DelDoc, int ID)
        {
            InitializeComponent();
            _Del = Del;
            _DelDoc = DelDoc;
            _Export = Export;
            _ID = ID;
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

        private void Frm_01_new_Recipt_Cheques_Load(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                foreach (Janus.Windows.GridEX.GridEXColumn col in this.gridEX1.RootTable.Columns)
                {
                    col.CellStyle.BackColor = SystemColors.Window;
                    if (col.Key == "Column42" || col.Key == "Column44")
                        col.DefaultValue = Class_BasicOperation._UserName;
                    if (col.Key == "Column43" || col.Key == "Column45")
                        col.DefaultValue = Class_BasicOperation.ServerDate();
                }

                if (isadmin)
                {
                    BoxAdapter = new SqlDataAdapter("Select * from Table_020_BankCashAccInfo where Column01=1", ConBank);
                    BoxAdapter.Fill(dataSet1, "Box");
                    gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Box"], "");
                    gridEX_Turn.DropDowns["Bank"].DataSource = ClDoc.ReturnTable(ConBank, @"Select * from Table_020_BankCashAccInfo");
                }

                else
                {
                    BoxAdapter = new SqlDataAdapter("Select * from Table_020_BankCashAccInfo where column01=1 AND Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')", ConBank);
                    BoxAdapter.Fill(dataSet1, "Box");
                    gridEX1.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Box"], "");
                    //gridEX_Turn.DropDowns["ToBank"].SetDataBinding(dataSet1.Tables["Box"], "");
                    gridEX_Turn.DropDowns["Bank"].DataSource = ClDoc.ReturnTable(ConBank, @"Select * from Table_020_BankCashAccInfo");

                }
               

                dtPersonActive = new DataTable();
                dtPersonAll = new DataTable();
                dtPersonActive = ClDoc.ReturnTable(ConBase, @"select * from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
                dtPersonAll = ClDoc.ReturnTable(ConBase, @"select * from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");



                PersonAdapter = new SqlDataAdapter("select * from Table_045_PersonInfo  WHERE 'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')", ConBase);
                PersonAdapter.Fill(dataSet1, "Person");
                gridEX1.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
                gridEX1.DropDowns["Cashier"].DataSource=ClDoc.ReturnTable(ConBase,@"Select * from Table_045_PersonInfo");
                //gridEX_Turn.DropDowns["Person"].SetDataBinding(dtPersonAll, "");
                gridEX_Turn.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select * from Table_045_PersonInfo");
             
                BanksAdapter = new SqlDataAdapter("Select * from Table_010_BankNames", ConBank);
                BanksAdapter.Fill(dataSet1, "Banks");
                gridEX1.DropDowns["Banks"].SetDataBinding(dataSet1.Tables["Banks"], "");

                StatusAdapter = new SqlDataAdapter("Select * from Table_060_ChequeStatus where Column15=1", ConBank);
                StatusAdapter.Fill(dataSet1, "Status");
                gridEX1.DropDowns["Status"].SetDataBinding(dataSet1.Tables["Status"], "");
               
                //gridEX_Turn.DropDowns["Status"].SetDataBinding(dataSet1.Tables["Status"], "");
                //gridEX_Turn.DropDowns["Status"].SetDataBinding(@"select * from Table_060_ChequeStatus ", "");
                gridEX_Turn.DropDowns["Status"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_060_ChequeStatus");


                ProjectAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
                ProjectAdapter.Fill(dataSet1, "Project");
                gridEX1.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");
                gridEX_Turn.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");

                gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConACNT, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                gridEX_Turn.DropDowns["Header"].SetDataBinding(ClDoc.ReturnTable(ConACNT, "Select ACC_Code,ACC_Name from AllHeaders()"), "");

                DataTable CurrencyTable = ClDoc.ReturnTable(ConBase, "Select * from Table_055_CurrencyInfo");
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
                CitybindingSource.DataMember = "Province_City";
                gridEX1.DropDowns["Province"].SetDataBinding(ProvincebindingSource, "");
                gridEX1.DropDowns["City"].SetDataBinding(CitybindingSource, "");


                if (_ID != 0)
                {
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, _ID);
                    this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet_01_Cash.Table_065_TurnReception, _ID);
                    dataSet_01_Cash.EnforceConstraints = true;

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
            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex,this.Name); }

        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column50")
                    gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.None;
                else gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
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

        private void gridEX1_RowEditCanceled(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            gridEX1.Enabled = false;
            btn_New.Enabled = true;
            gridEX1.MoveToNewRecord();
            superTabItem1.Text = "اطلاعات برگه دریافت چک ";
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

        private void btn_New_Click(object sender, EventArgs e)
        {
            //string personbank = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column35 from Table_020_BankCashAccInfo where ColumnId=" + ((DataRowView)gridEX1.RootTable.Columns["Column01"].DropDown.FindItem(gridEX1.GetValue("Column01")))["ColumnId"].ToString() + "");
            
            
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

                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, 0);
                    this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet_01_Cash.Table_065_TurnReception, 0);
                    dataSet_01_Cash.EnforceConstraints = true;

                    gridEX1.MoveToNewRecord();
                    superTabItem1.Text = "اطلاعات برگه دریافت چک ";

                    //table_035_ReceiptChequesBindingSource.AddNew();
                    gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD"));
                    gridEX1.SetValue("Column42", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column43", Class_BasicOperation.ServerDate().ToString());
                    gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());

                    //gridEX1.SetValue("Column48", Column48);
                    //gridEX1.SetValue("Column01", Column01);
                    //gridEX1.SetValue("Column02", Column02);
                    //gridEX1.SetValue("Column08", Column08);
                    //gridEX1.SetValue("Column09", Column09);
                    //gridEX1.SetValue("Column10", Column10);
                    //gridEX1.SetValue("Column07", Column07);
                    //gridEX1.SetValue("Column06", Column06);

                    gridEX1.RootTable.Columns["Column50"].Selectable = false;
                    gridEX1.RootTable.Columns["Column51"].Selectable = false;
                    gridEX1.Select();
                    gridEX1.Col = 1;
            //mlt_status.Focus();
            //string status = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select ColumnId from Table_060_ChequeStatus where Column15=1),0)");
            //string fund = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select ColumnId from Table_020_BankCashAccInfo where columnid=1),0)");

            //mlt_status.Value = status;

            //mlt_fund.Value = fund;
            btn_New.Enabled = false;
            //uiPanel1.Enabled = true;
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
                    string status = ClDoc.ExScalar(ConBank.ConnectionString, @"select Columnid from Table_060_ChequeStatus where Column09=101100 and Column03=103100");

                    gridEX1.Enabled = true;
                    foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                    {
                        if (item.Key == "Column46" || item.Key == "Column54" || item.Key == "Column55")
                            item.Selectable = false;
                        else
                            item.Selectable = true;
                    }

                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, 0);
                    this.table_065_TurnReceptionTableAdapter.FillBy(this.dataSet_01_Cash.Table_065_TurnReception, 0);
                    dataSet_01_Cash.EnforceConstraints = true;

                    gridEX1.MoveToNewRecord();
                    superTabItem1.Text = "اطلاعات برگه دریافت چک ";
                    gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                    gridEX1.SetValue("Column42", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column43", Class_BasicOperation.ServerDate());
                    gridEX1.SetValue("Column44", Class_BasicOperation._UserName);
                    gridEX1.SetValue("Column45", Class_BasicOperation.ServerDate());
                    //gridEX1.SetValue("Column48", dataSet1.Tables["Status"].Rows[0]["ColumnId"].ToString());
                    gridEX1.SetValue("Column48", status); 
                    gridEX1.RootTable.Columns["Column50"].Selectable = false;
                    gridEX1.RootTable.Columns["Column51"].Selectable = false;
                    gridEX1.Select();
                    gridEX1.Col = 1;
                    btn_New.Enabled = false;

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
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.GetValue("Column05").ToString() == "0")
                { MessageBox.Show("امکان ثبت مبلغ 0 وجود ندارد"); return; }
                /////
                if (gridEX1.GetValue("ColumnId").ToString().StartsWith("-"))
                {
                    DataTable dtrecipt = ClDoc.ReturnTable(ConBank, @"select column03 from Table_035_ReceiptCheques where column03='" + gridEX1.GetValue("Column03").ToString() + "'");
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
                //        dataSet_01_Cash.EnforceConstraints = false;
                //        this.table_035_ReceiptChequesTableAdapter.FillBy(this.dataSet_01_Cash.Table_035_ReceiptCheques, RowId);
                //        this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, RowId);
                //        dataSet_01_Cash.EnforceConstraints = true;
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

        private void SaveEvent(object sender, EventArgs e)
        {
            int PaperID = 0;
            PaperID = int.Parse(gridEX1.GetValue("ColumnId").ToString());

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
                dataSet_01_Cash.EnforceConstraints = false;
                this.table_035_ReceiptChequesBindingSource.EndEdit();
                this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                dataSet_01_Cash.EnforceConstraints = true;
                _03_Bank.Form02_ExportDocForReceive frm = new Form02_ExportDocForReceive(int.Parse(gridEX1.GetValue("ColumnId").ToString()), gridEX1.GetRow().Cells["Column01"].Text, gridEX1.GetRow().Cells["Column08"].Text, gridEX1.GetRow().Cells["Column02"].Text);
                frm.ShowDialog();
                dataSet_01_Cash.EnforceConstraints = false;
                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques,int.Parse(((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception,int.Parse(((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                dataSet_01_Cash.EnforceConstraints = true;
                if (sender == btn_Save || sender == this)
                 //Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
                gridEX1.UpdateData();
                superTabItem1.Text = "اطلاعات برگه دریافت چک شماره " + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                btn_New.Enabled = true;
               
            }
        }
        private void SaveEvent(object sender)
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
                this.table_035_ReceiptChequesTableAdapter.Update(dataSet_01_Cash.Table_035_ReceiptCheques);
                //_03_Bank.Form02_ExportDocForReceive frm = new Form02_ExportDocForReceive(int.Parse(gridEX1.GetValue("ColumnId").ToString()), gridEX1.GetRow().Cells["Column01"].Text, gridEX1.GetRow().Cells["Column08"].Text, gridEX1.GetRow().Cells["Column02"].Text);
                //frm.ShowDialog();

                if (sender == btn_Save || sender == this)
                //Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
                gridEX1.UpdateData();
                superTabItem1.Text = "اطلاعات برگه دریافت چک شماره " + ((DataRowView)this.table_035_ReceiptChequesBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                btn_New.Enabled = true;
            }
        
        

        }
        private void table_035_ReceiptChequesBindingSource_PositionChanged(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

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
                            if (item.Key == "Column41" || item.Key == "Column42")
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
                            if (item.Key == "Column46" || item.Key == "Column54" || item.Key == "Column55")
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


                //string personbank = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column35 from Table_020_BankCashAccInfo where ColumnId=" + ((DataRowView)gridEX1.RootTable.Columns["Column01"].DropDown.FindItem(gridEX1.GetValue("Column01")))["ColumnId"].ToString() + "");

                //mlt_PersonBank.Value = personbank;

                gridEX1.DropDowns["Cashier"].SetDataBinding(dtPersonAll, "");

                _New = false;
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Int16 _StatusId; Int64 _TurnID = 0;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 94))
            {
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
                                gridEX1.GetValue("ColumnId").ToString());
                                dataSet_01_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, 0);
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, 0);
                                dataSet_01_Cash.EnforceConstraints = true;


                                Class_BasicOperation.ShowMsg("", "حذف برگه با موفقیت انجام شد", Class_BasicOperation.MessageType.Information);
                                btn_New.Enabled = true;
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
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

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
                        item.Cells["Column01"].Text.Trim(), null
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

        private void mnu_Search_ChqNumber_Click(object sender, EventArgs e)
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
                        SaveEvent(sender);
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
                            SaveEvent(sender);
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
                            SaveEvent(sender);
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
                        SaveEvent(sender);
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
        string user = "";
        public void btn_Search_Click(object sender, EventArgs e)
        {

            try
            {
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
                        SaveEvent(sender);
                    }
                }
                if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    if (mnu_Search_NumberBack.Checked)
                    {
                         user = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull ((select Column42 from Table_035_ReceiptCheques where Column00 =" + txt_Search.Text + "),0)");

                    }
                    if (mnu_Search_ChqNumber.Checked)
                    {
                         user = ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull ((select Column42 from Table_035_ReceiptCheques where Column03 ='" + txt_Search.Text + "'),0)");

                    }
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
                           
                                string num1 = ClDoc.ExScalar(ConBank.ConnectionString, @"select Columnid from Table_035_ReceiptCheques where Column00="+txt_Search.Text.TrimEnd(',')+"");
                                dataSet_01_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillByBackNumber(this.dataSet_01_Cash.Table_035_ReceiptCheques, int.Parse(txt_Search.Text.Trim()));
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, int.Parse(num1.Trim()));
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

        private void btn_Doc_Click(object sender, EventArgs e)
        {
            try
            {

                if (gridEX1.GetValue("ColumnId").ToString()=="-1" || gridEX1.GetValue("Column00").ToString()=="-1")
                {
                    MessageBox.Show("ابتدا برگه چک را ذخیره کنید");
                    return;
                }

                if (_Export)
                {
                    int PaperID = 0;
                 Classes.   Class_UserScope UserScope = new Classes. Class_UserScope();
                    if (this.table_035_ReceiptChequesBindingSource.Count > 0 && !ClDoc.HasTurn_Rec(int.Parse(gridEX1.GetValue("ColumnId").ToString())))
                    {


                        if (gridEX1.GetValue("Column48").ToString() != "")
                        {
                            try
                            {
                                btn_Save_Click(sender, e);
                                ClDoc.CheckExistFinalDoc();


                                PaperID = int.Parse(gridEX1.GetValue("ColumnId").ToString());
                            }
                            catch { }
                            if (PaperID > 0)
                            {
                                _03_Bank.Form02_ExportDocForReceive frm = new Form02_ExportDocForReceive(int.Parse(gridEX1.GetValue("ColumnId").ToString()), gridEX1.GetRow().Cells["Column01"].Text, gridEX1.GetRow().Cells["Column08"].Text, gridEX1.GetRow().Cells["Column02"].Text);
                                frm.ShowDialog();

                                dataSet_01_Cash.EnforceConstraints = false;
                                this.table_035_ReceiptChequesTableAdapter.FillBy(dataSet_01_Cash.Table_035_ReceiptCheques, PaperID);
                                this.table_065_TurnReceptionTableAdapter.FillBy(dataSet_01_Cash.Table_065_TurnReception, PaperID);
                                dataSet_01_Cash.EnforceConstraints = true;
                            }
                            gridEX_Turn.DropDowns["Doc"].SetDataBinding(ClDoc.ReturnTable(ConACNT, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
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

        private void Frm_01_new_Recipt_Cheques_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void Frm_01_new_Recipt_Cheques_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                btn_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.N && btn_New.Enabled)
                btn_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                btn_Delete_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E) ;
            //    btn_Export_Click(sender, e);
            //else if (e.Control && e.KeyCode == Keys.F)
            //{

            //    mnu_SearchType.ShowDropDown();

            //}
            //else if (e.Control && e.KeyCode == Keys.P)
            //    bt_Print_Click(sender, e);
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Search_Click(sender, e);
        }



   

    }
}

     
    