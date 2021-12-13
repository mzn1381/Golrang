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
using DevComponents.DotNetBar;

namespace PCLOR.MarjooiSale
{
    public partial class Frm_001_MarjooiSale : Form
    {
        bool _del;
        int _ID = 0, ReturnId = 0, ReturnNum = 0, ResidId = 0, ResidNum = 0;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        DataSet DS = new DataSet();
        SqlDataAdapter DraftAdapter, DocAdapter, ReturnAdapter;
        string ReturnDate = null;
        InputLanguage original;
        DataTable dts;
        bool messege = false;
        bool SalePrice, DiscountLiner, DiscountEnd = false;
        SqlParameter ReturnDocNum;
        decimal sum;
        string ficloth;
        string ficolor;
        string SelectBrand;
        string NumberProduct;
        bool Search = false;
        public Frm_001_MarjooiSale(bool del)
        {
            _del = del;
            InitializeComponent();
        }
        public Frm_001_MarjooiSale(bool del, int ID, bool _Search)
        {
            _del = del;
            _ID = ID;
            Search = _Search;

            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {
            mlt_Recipt.DataSource = clDoc.ReturnTable(ConWare, @"select Columnid,Column01 from Table_011_PwhrsReceipt");
            mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            DataTable dt = clDoc.ReturnTable(ConPCLOR, "select * from Table_80_Setting");

          

            DataTable PersonTable = clDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");

            mlt_NameCustomer.DataSource = PersonTable;
            gridEX_List.DropDowns["Codecommodity"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
            gridEX_List.DropDowns["Namecommodity"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId,Column02 from table_004_CommodityAndIngredients");
            gridEX_List.DropDowns["UnitCount"].DataSource = clDoc.ReturnTable(ConBase, @"select Column00,Column01 from Table_070_CountUnitInfo");
            if (Search)
            {
                bt_Search_1_Click(sender, e);
                
            }
        }

        //private void bt_New_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 76))
        //        {

        //            table_010_SaleFactorBindingSource.AddNew();
        //            txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
        //            ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column13"] = Class_BasicOperation._UserName;
        //            ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column14"] = Class_BasicOperation.ServerDate().ToString();
        //            mlt_NameCustomer.Focus();
        //            bt_New.Enabled = false;
        //            mlt_NameCustomer.Text = "";
        //            txt_Description.Text = "";
        //            txt_Barcode.Text = "";
        //            //mlt_Function_S.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=6");
        //            //table_65_HeaderOtherPWHRSBindingSource_PositionChanged(sender, e);
        //        }
        //        else
        //        {

        //            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        //        }





        //    }
        //    catch (Exception ex)
        //    {
        //        Class_BasicOperation.CheckExceptionType(ex, this.Name);
        //    }
        //}

        //private void Save_Event(object sender, EventArgs e)
        //{
        //    gridEX_List.UpdateData();
        //    gridEX_Extra.UpdateData();

        //    if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0")
        //    {
        //        MessageBox.Show("این فاکتور حواله صادر شده است");
        //        return;
        //    }
        //    if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
        //    {
        //         MessageBox.Show("این فاکتور سند صادر شده است");
        //        return;
        //    }



        //    if (this.table_010_SaleFactorBindingSource.Count > 0 &&
        //       gridEX_List.AllowEdit == InheritableBoolean.True &&
        //       mlt_NameCustomer.Text!= "" && mlt_Ware.Text != "" && mlt_Function.Text != "")
        //    {
        //        if (Properties.Settings.Default.ShowPriceAlert > 0)
        //            CheckGoodsPrice();
        //        this.Cursor = Cursors.WaitCursor;


        //        if (gridEX_List.GetDataRows().Count() == 0)
        //        {
        //            Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
        //            return;
        //        }


        //        if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
        //        {
        //            txt_Number.Text = clDoc.MaxNumber(Properties.Settings.Default.PSALE, " Table_010_SaleFactor", "Column01").ToString();

        //        }
        //        this.table_010_SaleFactorBindingSource.EndEdit();
        //        DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

        //        if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
        //         Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
        //         Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
        //        {

        //            Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
        //            this.Cursor = Cursors.Default;

        //            return;

        //        }

        //        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
        //        {
        //            item.BeginEdit();
        //            decimal fi = Convert.ToDecimal((item.Cells["Column10"].Value).ToString());
        //            decimal count = Convert.ToDecimal((item.Cells["Column07"].Value).ToString());
        //            item.Cells["column20"].Value =Convert.ToDecimal( fi * count);
        //            item.Cells["column11"].Value = Convert.ToDecimal(fi * count).ToString();
        //            item.EndEdit();
        //        }

        //        ///

        //        txt_TotalPrice.Value = Convert.ToDouble(
        //        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
        //        AggregateFunction.Sum).ToString());
        //        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
        //        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
        //        Convert.ToDouble(txt_Extra.Value.ToString()) -
        //        Convert.ToDouble(txt_Reductions.Value.ToString());
        //        double Total = double.Parse(txt_TotalPrice.Value.ToString());

        //        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
        //        {
        //            if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
        //            {
        //                item.BeginEdit();
        //                item.Cells["Column04"].Value = (Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
        //                item.EndEdit();

        //            }
        //        }



        //        double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
        //           gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
        //        int CustomerCode = int.Parse(Row["Column03"].ToString());
        //        string Date = Row["Column02"].ToString();
        //        ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column28"] = NetTotal.ToString();
        //        if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column12"].ToString() == "False")
        //        {
        //            NetTotal = ClDiscount.SpecialGroup(
        //                Convert.ToDouble(Row["Column28"].ToString()), CustomerCode, Date);
        //            ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column30"] = NetTotal;

        //            NetTotal = ClDiscount.VolumeGroup(Convert.ToDouble(Row["Column28"].ToString()) -
        //                Convert.ToDouble(Row["Column30"].ToString()), CustomerCode, Date);
        //            ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column29"] = NetTotal;

        //            NetTotal = ClDiscount.SpecialCustomer(
        //                Convert.ToDouble(Row["Column28"].ToString()) - Convert.ToDouble(Row["Column30"].ToString()) -
        //                Convert.ToDouble(Row["Column29"].ToString()), CustomerCode, Date);

        //            ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column31"] = NetTotal;
        //        }





        //        Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
        //        txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
        //        Filter.Value1 = true;
        //        txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

        //      ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column15"] = Class_BasicOperation._UserName;
        //      ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column16"] = Class_BasicOperation.ServerDate();
        //      ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
        //      ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

        //        Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
        //        ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
        //        Filter2.Value1 = true;
        //        ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();



        //        this.table_010_SaleFactorBindingSource.EndEdit();
        //        this.table_011_Child1_SaleFactorBindingSource.EndEdit();
        //        this.table_012_Child2_SaleFactorBindingSource.EndEdit();

        //        this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);
        //        this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
        //        this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);

        //        if (sender == bt_Save || sender == this)
        //            Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");

        //        dataSet_01_Sale.EnforceConstraints = false;
        //        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor,int.Parse(txt_ID.Text));
        //        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(txt_ID.Text));
        //        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(txt_ID.Text));
        //        dataSet_01_Sale.EnforceConstraints = true;
        //        table_010_SaleFactorBindingSource_PositionChanged(sender, e);

        //        bt_New.Enabled = true;
        //        this.Cursor = Cursors.Default;

        //    }


        //}
        //        private void checkbarcode() 
        //        {
        //            //int Count = 0;
        //            Int64 barcode = 0;
        //          string  barcoderepeat = "0";
        //            PWHRS = false;
        //            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
        //            {

        //                if (item.Cells["Column34"].Text != "")
        //                {

        //                    DataTable dtsale = clDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, dbo.Table_70_DetailOtherPWHRS.TypeCloth, 
        //                      dbo.Table_70_DetailOtherPWHRS.Count, dbo.Table_65_HeaderOtherPWHRS.Recipt, dbo.Table_70_DetailOtherPWHRS.weight, 
        //                      PWHRS_2_1398.dbo.table_004_CommodityAndIngredients.column01 AS goodcode, 
        //                      PWHRS_2_1398.dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh
        //                    FROM         PWHRS_2_1398.dbo.table_004_CommodityAndIngredients INNER JOIN
        //                      dbo.Table_65_HeaderOtherPWHRS INNER JOIN
        //                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK ON 
        //                      PWHRS_2_1398.dbo.table_004_CommodityAndIngredients.columnid = dbo.Table_70_DetailOtherPWHRS.CodeCommondity
        //                        WHERE     (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (dbo.Table_70_DetailOtherPWHRS.Barcode = " + item.Cells["Column34"].Text + ")");


        //                    //foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_List.GetRows())
        //                    //{
        //                    //    if ((item.Cells["Column34"].Value).ToString() == (item.Cells["Column34"].Value).ToString())
        //                    //    {
        //                    //        Count++;
        //                    //    }
        //                    //}

        //                    //if (Count>1)
        //                    //{
        //                    //    barcode = Int64.Parse(item.Cells["Column34"].Text);
        //                    //    barcoderepeat += barcode + ",";
        //                    //     MessageBox.Show("این بارکد تکراری می باشد و یکبار امکان اضافه آن بوده است" + barcoderepeat);
        //                    //      return;
        //                    //}


        //                    if (dtsale.Rows.Count > 0)
        //                    {
        //                        float Remain = FirstRemain(int.Parse(dtsale.Rows[0][1].ToString()), (item.Cells["Column34"].Value.ToString()), mlt_Ware.Value.ToString());

        //                        if (Remain > 0)
        //                        {
        //                            item.BeginEdit();
        //                            item.Cells["Column34"].Value=dtsale.Rows[0][0].ToString();
        //                            item.Cells["column02"].Value=dtsale.Rows[0][1].ToString();
        //                            item.Cells["column03"].Value = dtsale.Rows[0][7].ToString();
        //                            item.Cells["GoodCode"].Value=dtsale.Rows[0][1].ToString();
        //                            item.Cells["column06"].Value=dtsale.Rows[0][3].ToString();
        //                            item.Cells["column07"].Value = dtsale.Rows[0][3].ToString();
        //                            item.Cells["Column36"].Value=dtsale.Rows[0][5].ToString();
        //                            item.Cells["Column37"].Value = dtsale.Rows[0][5].ToString();
        //                            item.EndEdit();
        //                        }
        //                        else 
        //                        {
        //                            PWHRS = true;
        //                            errorelist += item.Cells["Column34"].Value.ToString() + ",";

        //                        }



        //                    }

        //                }
        //            }

        //        }
        bool PWHRSbool;
        string errorelist;


        //private void bt_Save_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if ( mlt_NameCustomer.Text == "" ||  mlt_NameCustomer.Text == "0" )
        //        {
        //            MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
        //            return;
        //        }
        //        Int64 barcode = 0;
        //        errorelist = string.Empty;

        //        if (!txt_Number.Text.StartsWith("-"))
        //        { checkbarcode(); }

        //        foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_List.GetRows())
        //        {
        //            if (PWHRS )
        //            {
        //                MessageBox.Show("موجودی کالا های زیر در انبار موردنظر کافی نیست" + Environment.NewLine + errorelist.TrimEnd(','));
        //                return;
        //            }

        //        }

        //        Save_Event(sender, e);

        //    }
        //    catch (Exception ex)
        //    {
        //        Class_BasicOperation.CheckExceptionType(ex, this.Name);
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        private void CheckGoodsPrice()
        {
            List<string> Codes = new List<string>();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {
                Codes.Add(item.Cells["Column02"].Value.ToString());
            }

            DataTable Table = clDoc.ReturnTable(ConWare, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_012_Child_PwhrsReceipt.column02,  MAX(Table_011_PwhrsReceipt.column02) AS Date,1
            FROM         Table_012_Child_PwhrsReceipt INNER JOIN
            Table_011_PwhrsReceipt ON Table_012_Child_PwhrsReceipt.column01 = Table_011_PwhrsReceipt.columnid
            where Table_012_Child_PwhrsReceipt.column02 in (" + string.Join(",", Codes.ToArray()) + @")
            GROUP BY Table_012_Child_PwhrsReceipt.column02
            order by Table_012_Child_PwhrsReceipt.column02;
            
            declare @t2 table(codekala2 int , gheymat2 decimal(18,3),date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column10, 
            dbo.Table_011_PwhrsReceipt.column02 AS Date
            FROM         dbo.Table_012_Child_PwhrsReceipt INNER JOIN
            dbo.Table_011_PwhrsReceipt ON dbo.Table_012_Child_PwhrsReceipt.column01 = dbo.Table_011_PwhrsReceipt.columnid
            where Table_012_Child_PwhrsReceipt.column02 in (" + string.Join(",", Codes.ToArray()) + @")
            GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column10, dbo.Table_011_PwhrsReceipt.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {
                DataRow[] Row = Table.Select("GoodCode=" + item.Cells["Column02"].Value.ToString());

                if (Row.Length > 0)
                {
                    if (Properties.Settings.Default.ShowPriceAlert == 2)
                    {
                        if (Convert.ToDouble(Row[0]["Price"].ToString()) > Convert.ToDouble(item.Cells["Column10"].Value.ToString()))
                            throw new Exception("قیمت مندرج برای کالای " + item.Cells["Column02"].Text + Environment.NewLine + " کمتر از آخرین قیمت است" +
                                Environment.NewLine + " آخرین ورود این کالا در تاریخ " + Row[0]["Date"].ToString() + " و با قیمت " +
                                Convert.ToDouble(Row[0]["Price"].ToString()).ToString("#,##0.###") + " صورت گرفته است");
                    }
                    else
                    {
                        if (Convert.ToDouble(Row[0]["Price"].ToString()) > Convert.ToDouble(item.Cells["Column10"].Value.ToString()))
                            Class_BasicOperation.ShowMsg("", "قیمت مندرج برای کالای " + item.Cells["Column02"].Text + Environment.NewLine + " کمتر از آخرین قیمت است" +
                                    Environment.NewLine + " آخرین ورود این کالا در تاریخ " + Row[0]["Date"].ToString() + " و با قیمت " +
                                    Convert.ToDouble(Row[0]["Price"].ToString()).ToString("#,##0.###") + " صورت گرفته است", "Warning");
                    }
                }
            }



        }

       

        private void gridEX_List_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

     
        private void gridEX_Extra_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            if (e.Column.Key == "column02")
            {

                gridEX_Extra.SetValue("column05", (gridEX_Extra.DropDowns["Type"].GetValue("column02")));
                gridEX_Extra.SetValue("column04", "0");
                gridEX_Extra.SetValue("column03", "0");

                if (gridEX_Extra.DropDowns["Type"].GetValue("column03").ToString() == "True")
                {
                    gridEX_Extra.SetValue("column04", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                }
                else
                {

                    gridEX_Extra.SetValue("column03", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                    Double darsad;
                    darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());

                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column11"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Extra.SetValue("column04",
                        (Convert.ToInt64(kol * darsad / 100)));
                }
            }
            else if (e.Column.Key == "column03")
            {
                Double darsad;
                darsad = Convert.ToDouble(e.Value.ToString());
                Double kol;
                kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column11"].Value.ToString());
                if (kol == 0)
                    return;
                gridEX_Extra.SetValue("column04",
                       (Convert.ToInt64(kol * darsad / 100)));
            }
        }

        private void gridEX_Extra_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_List.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }
        }

        private void gridEX_Extra_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }
        }



      
        private void bt_DelDoc_Click(object sender, EventArgs e)
        {

        }

   

   

   
        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_018_MarjooiSale where column01=" + FactorNum, con);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره فاکتور وارد شده نامعتبر است");
                }
            }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            //else if (e.KeyChar == 13)
            //bt_Search_Click(sender, e);
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 83))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_002_ViewFactorReturn")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                MarjooiSale.Frm_002_ViewFactorReturn frm = new Frm_002_ViewFactorReturn();

                try
                {
                    frm.MdiParent = Frm_Main.ActiveForm;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Frm_002_Faktor_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
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
        }

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }





        private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }



        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            try
            {
                txt_TotalPrice.Value = Convert.ToDouble(
                       gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                       AggregateFunction.Sum).ToString());
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }

        }

        private void txt_GoodCode_Enter(object sender, EventArgs e)
        {
            original = InputLanguage.CurrentInputLanguage;
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                InputLanguage.CurrentInputLanguage = language;
            else
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
        }

        private void txt_GoodCode_Leave(object sender, EventArgs e)
        {


            var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
            var language = InputLanguage.FromCulture(culture);
            InputLanguage.CurrentInputLanguage = language;

        }

        private void gridEX_List_CurrentCellChanging(object sender, CurrentCellChangingEventArgs e)
        {
            if (e.Column != null)
            {
                original = InputLanguage.CurrentInputLanguage;

                if (e.Column.Key == "GoodCode")
                {
                    var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                    var language = InputLanguage.FromCulture(culture);
                    if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                        InputLanguage.CurrentInputLanguage = language;
                    else
                        InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
                }


                else
                {
                    var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
                    var language = InputLanguage.FromCulture(culture);
                    InputLanguage.CurrentInputLanguage = language;
                }
            }


        }

      

    

   

        private void gridEX_List_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        {

        }

        private void gridEX_List_AddingRecord(object sender, CancelEventArgs e)
        {

        }

        private void gridEX_List_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {

        }

        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {

                if (gridEX_List.RowCount > 0)
                {

                    decimal fi = Convert.ToDecimal((gridEX_List.GetValue("Column10")).ToString());
                    decimal weight = Convert.ToDecimal((gridEX_List.GetValue("column35")).ToString());

                    DataTable dt = clDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, dbo.Table_70_DetailOtherPWHRS.TypeCloth, 
                      dbo.Table_70_DetailOtherPWHRS.Count, dbo.Table_65_HeaderOtherPWHRS.Recipt, dbo.Table_70_DetailOtherPWHRS.weight, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh, dbo.Table_70_DetailOtherPWHRS.TypeColor, 
                      dbo.Table_70_DetailOtherPWHRS.Machine, dbo.Table_005_TypeCloth.ID, dbo.Table_010_TypeColor.ID AS IDColor
                      FROM         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients INNER JOIN
                      dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK ON 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid = dbo.Table_70_DetailOtherPWHRS.CodeCommondity INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_70_DetailOtherPWHRS.TypeCloth = dbo.Table_005_TypeCloth.ID INNER JOIN
                      dbo.Table_010_TypeColor ON dbo.Table_70_DetailOtherPWHRS.TypeColor = dbo.Table_010_TypeColor.TypeColor
                      WHERE     (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (dbo.Table_70_DetailOtherPWHRS.Barcode = " + (gridEX_List.GetValue("Column32")).ToString() + ")");
                    if (dt.Rows.Count>0)
                    {
                        
                    string ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dt.Rows[0][10].ToString() + ")),0)");
                    string ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + dt.Rows[0][11].ToString() + ")),0)");
                    string SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dt.Rows[0][10].ToString() + ")),0)");
                    sum = Convert.ToDecimal(ficloth) + Convert.ToDecimal(ficolor);
                    if (SelectBrand.ToString() == "True")
                    {

                        gridEX_List.SetValue("column20", fi * weight);
                        gridEX_List.SetValue("column11", fi * weight);
                        txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"], AggregateFunction.Sum).ToString());
                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());


                    }

                    else if (SelectBrand.ToString() == "False" )
                    {
                        gridEX_List.SetValue("column20", fi * weight);
                        gridEX_List.SetValue("column11", fi * weight);
                        txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"], AggregateFunction.Sum).ToString());
                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
                    }
                    //else
                    //{
                    //    MessageBox.Show("مقدار وارد شده از مقدار پیشنهادی کمتر می باشد امکان اضافه آن را ندارید");
                    //    return;
                    //}
                    }
                    else
                    {
                        MessageBox.Show("بارکد وارد شده نامعتبر است");
                    }
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX1_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX1_RecordUpdated(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
        }

       

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {

        }

       

        private void bt_Refresh_Click(object sender, EventArgs e)
        {

        }

        private void mnu_CancelCancel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void bt_SendSms_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void btn_person_Click(object sender, EventArgs e)
        {

        }

        private void mnu_Customers_Click(object sender, EventArgs e)
        {
            _00_BaseInfo.Frm_40_PersonInfo frm = new _00_BaseInfo.Frm_40_PersonInfo();
            frm.Show();
        }

        private void gridEX_List_Enter_1(object sender, EventArgs e)
        {

        }

        private void bt_DelDoc_Click_1(object sender, EventArgs e)
        {

        }

    

        private void mlt_NameCustomer_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_NameCustomer, "column02", "column01");
        }





        private void mlt_NameCustomer_KeyPress(object sender, KeyPressEventArgs e)
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

        string errore = string.Empty;
        string barcoderepeat = string.Empty;
        Int64 barcode = 0;
     

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            errore = "";
            barcoderepeat = "";
        


            try
            {
                if (mlt_NameCustomer.Text == "" || mlt_NameCustomer.Text == "0")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                    return;
                }

                if (table_018_MarjooiSaleBindingSource.Count > 0 || table_019_Child1_MarjooiSaleBindingSource.Count > 0)
                {


                    string strreplace = System.Text.RegularExpressions.Regex.Replace(txt_Barcode.Text.Trim(), @"\t|\n|\r", "");
                    var b = clDoc.GetNextChars(strreplace.Trim(), 8);
                    bool flrepeat = false;
                    

                    foreach (string s in b)
                    {
                        flrepeat = false;
                        if (s != "")
                        {
                            DataTable dtsale = clDoc.ReturnTable(ConPCLOR, @"SELECT        TOP (1) " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 AS Barcode, dbo.Table_005_TypeCloth.CodeCommondity, dbo.Table_005_TypeCloth.TypeCloth, 
                         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column03 AS Count, 0 AS Recipt, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column35 AS weight, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh, 
                         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36 AS TypeColor, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column37 AS Machine, dbo.Table_005_TypeCloth.ID, 
                         dbo.Table_010_TypeColor.ID AS IDColor, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01
FROM            " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients INNER JOIN
                         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ON " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid = " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 INNER JOIN
                         " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column01 = " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid LEFT OUTER JOIN
                         dbo.Table_010_TypeColor ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36 = dbo.Table_010_TypeColor.TypeColor LEFT OUTER JOIN
                         dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 = dbo.Table_005_TypeCloth.CodeCommondity
WHERE        (" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 = " + s + @")
ORDER BY " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid DESC, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01 DESC");



                            if (dtsale.Rows.Count > 0)
                            {

                                ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dtsale.Rows[0][10].ToString() + ")),0)");
                                ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + dtsale.Rows[0][6].ToString() + ")),0)");
                                SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dtsale.Rows[0][10].ToString() + ")),0)");
                                NumberProduct = clDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT        dbo.Table_035_Production.Number
FROM            dbo.Table_035_Production INNER JOIN
                         dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
WHERE        (dbo.Table_050_Packaging.Barcode = " + s + ")");
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                {
                                    if (s.ToString() == (item.Cells["Column32"].Value).ToString())
                                    {
                                        flrepeat = true;
                                        break;
                                    }
                                }


                                if (flrepeat)
                                {
                                    if (barcoderepeat.Length == 0)
                                        barcoderepeat = "این بارکد ها تکراری می باشد و یکبار اضافه شده است";
                                    barcoderepeat += s + ",";

                                }

                                if (dtsale.Rows.Count > 0)
                                {
                                    if (flrepeat == false)
                                    {

                                        table_018_MarjooiSaleBindingSource.EndEdit();
                                        gridEX_List.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                                        gridEX_List.MoveToNewRecord();
                                        gridEX_List.SetValue("column01", ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Columnid"].ToString());
                                        gridEX_List.SetValue("Column32", dtsale.Rows[0][0]);
                                        gridEX_List.SetValue("column02", dtsale.Rows[0][1].ToString());
                                        gridEX_List.SetValue("Column03", dtsale.Rows[0][7].ToString());
                                        gridEX_List.SetValue("GoodCode", dtsale.Rows[0][1].ToString());
                                        gridEX_List.SetValue("column06", dtsale.Rows[0][3].ToString());
                                        gridEX_List.SetValue("column07", dtsale.Rows[0][3].ToString());
                                        gridEX_List.SetValue("Column34", dtsale.Rows[0][5].ToString());
                                        gridEX_List.SetValue("Column35", dtsale.Rows[0][5].ToString());
                                        gridEX_List.SetValue("Column36", dtsale.Rows[0][8].ToString());
                                        gridEX_List.SetValue("Column37", dtsale.Rows[0][9].ToString());
                                        gridEX_List.SetValue("Column23", NumberProduct == null ? "Null" : NumberProduct);


                                        if (SelectBrand.ToString() == "True")
                                        {
                                            sum = Convert.ToDecimal(ficloth) + Convert.ToDecimal(ficolor);
                                            gridEX_List.SetValue("Column10", sum);
                                            decimal fi = Convert.ToDecimal(sum);
                                            decimal Weight = Convert.ToDecimal(dtsale.Rows[0][5].ToString());
                                            gridEX_List.SetValue("column20", fi * Weight);
                                            gridEX_List.SetValue("column11", fi * Weight);


                                        }

                                        if (SelectBrand.ToString() == "False")
                                        {
                                            gridEX_List.SetValue("Column10", ficloth);

                                            decimal fi = Convert.ToDecimal(ficloth);
                                            decimal Weight = Convert.ToDecimal(dtsale.Rows[0][5].ToString());
                                            gridEX_List.SetValue("column20", fi * Weight);
                                            gridEX_List.SetValue("column11", fi * Weight);

                                        }


                                        table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                                        table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                                        gridEX_List.UpdateData();
                                        gridEX_List.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;


                                    }
                                }
                            }
                        }
                    }

                    if (errore.Length > 7 || barcoderepeat.Length > 7)
                    {
                        MessageBox.Show((errore.Length > 0 ? "موجودی کالا های زیر در انبار موردنظر کافی نیست" + Environment.NewLine + errore.TrimEnd(',') : "") + Environment.NewLine
                            + barcoderepeat.TrimEnd(','));
                      
                       
                    }

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
          
            

            table_018_MarjooiSaleBindingSource.AddNew();
            txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
            ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column13"] = Class_BasicOperation._UserName;
            ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column14"] = Class_BasicOperation.ServerDate().ToString();
            ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column15"] = Class_BasicOperation._UserName;
            ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column16"] = Class_BasicOperation.ServerDate().ToString();
            mlt_NameCustomer.Focus();
            bt_New.Enabled = false;
            mlt_NameCustomer.Text = "";
            txt_Description.Text = "";
            txt_Barcode.Text = "";

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEvent(sender, e);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }


        private void SaveEvent(object sender, EventArgs e)
        {

            if (mlt_NameCustomer.Text != "")
            {
                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();
               
                if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0")
                {
                    MessageBox.Show("این فاکتور رسید صادر شده است");
                    return;
                }
                if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
                {
                    MessageBox.Show("این فاکتور سند صادر شده است");
                    return;
                }
               

                this.Cursor = Cursors.WaitCursor;
                gridEX_Extra.MoveToNewRecord();
                gridEX_List.MoveToNewRecord();
                DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    this.Cursor = Cursors.Default;

                    return;

                }

                if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                {
                    txt_Number.Text = clDoc.MaxNumber(Properties.Settings.Default.PSALE, " Table_018_MarjooiSale", "Column01").ToString();
                    this.table_018_MarjooiSaleBindingSource.EndEdit();
                }


                //if (Row["Column01"].ToString().StartsWith("-"))
                //{
                //    txt_Number.Text = clDoc.MaxNumber(Properties.Settings.Default.PSALE, " Table_018_MarjooiSale", "Column01").ToString();
                //    this.table_018_MarjooiSaleBindingSource.EndEdit();
                //}
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    item.BeginEdit();
                 
                    decimal fi = Convert.ToDecimal((item.Cells["Column10"].Value).ToString());
                    decimal weight = Convert.ToDecimal((item.Cells["Column35"].Value).ToString());

                    dts = clDoc.ReturnTable(ConPCLOR, @"SELECT         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 AS Barcode, dbo.Table_005_TypeCloth.CodeCommondity, dbo.Table_005_TypeCloth.TypeCloth, 
                         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column03 AS Count, 0 AS Recipt, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column35 AS weight, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh, 
                         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36 AS TypeColor, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column37 AS Machine, dbo.Table_005_TypeCloth.ID, 
                         dbo.Table_010_TypeColor.ID AS IDColor, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01
FROM            " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients INNER JOIN
                         " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ON " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid = " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 INNER JOIN
                         " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column01 = " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid LEFT OUTER JOIN
                         dbo.Table_010_TypeColor ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36 = dbo.Table_010_TypeColor.TypeColor LEFT OUTER JOIN
                         dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 = dbo.Table_005_TypeCloth.CodeCommondity
WHERE        (" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 = " + item.Cells["Column32"].Text + @")
ORDER BY " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid DESC, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01 DESC");

                    if (dts.Rows.Count>0)
                    {
                        

                        string ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE   (ID = " + dts.Rows[0][10].ToString() + ")),0)");
                        string ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE  (ID = " + dts.Rows[0][11].ToString() + ")),0)");
                        string SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dts.Rows[0][10].ToString() + ")),0)");
                 
                        sum = Convert.ToDecimal(ficloth) + Convert.ToDecimal(ficolor);
                   
                     
                    if (SelectBrand.ToString() == "True" )
                    {
                        gridEX_List.SetValue("column20", fi * weight);
                        gridEX_List.SetValue("column11", fi * weight);

                    }

                    else if (SelectBrand.ToString() == "False" )
                    {
                        gridEX_List.SetValue("column20", fi * weight);
                        gridEX_List.SetValue("column11", fi * weight);
                       
                    }
                    //else
                    //{
                    //    MessageBox.Show("مقدار وارد شده از مقدار پیشنهادی کمتر می باشد امکان اضافه آن را ندارید");
                    //    return;
                    //}


                    item.EndEdit();
                   
                    //else
                    //{
                    //    MessageBox.Show("لطفا موجودر را برررسی کنید");
                    //}            
                  }
                    else
                    {
                            messege = true;
                    }
                   
                }

                
   
                double Total = double.Parse(txt_TotalPrice.Value.ToString());
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = (
                            Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
                        item.EndEdit();

                    }
                }

                foreach (Janus.Windows.GridEX.GridEXRow item1 in gridEX_List.GetRows())
                {
                    if (double.Parse(item1.Cells["Column10"].Value.ToString()) > 0)
                    {
                        item1.BeginEdit();
                        item1.Cells["Column11"].Value = Convert.ToDouble(item1.Cells["Column35"].Value.ToString()) * Convert.ToDouble(item1.Cells["Column10"].Value.ToString());
                        item1.Cells["Column20"].Value = Convert.ToDouble(item1.Cells["Column35"].Value.ToString()) * Convert.ToDouble(item1.Cells["Column10"].Value.ToString());
                        item1.EndEdit();

                    }
                }


                txt_TotalPrice.Value = Convert.ToDouble(
                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                AggregateFunction.Sum).ToString());
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                Convert.ToDouble(txt_Extra.Value.ToString()) -
                Convert.ToDouble(txt_Reductions.Value.ToString());

                Row["Column15"] = Class_BasicOperation._UserName;
                Row["Column16"] = Class_BasicOperation.ServerDate();
                Row["Column18"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                //ذخیره اضافه و کسر خطی
                Row["Column21"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                Row["Column22"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter1 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                Row["Column19"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter1).ToString();
                Filter1.Value1 = true;
                Row["Column20"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter1).ToString();
               
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString())
                    - Convert.ToDouble(txt_Reductions.Value.ToString());


                dataSet_01_Sale.EnforceConstraints = false;
                this.table_018_MarjooiSaleBindingSource.EndEdit();
                this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                this.table_018_MarjooiSaleTableAdapter.Update(dataSet_01_Sale.Table_018_MarjooiSale);
                this.table_019_Child1_MarjooiSaleTableAdapter.Update(dataSet_01_Sale.Table_019_Child1_MarjooiSale);
                this.table_020_Child2_MarjooiSaleTableAdapter.Update(dataSet_01_Sale.Table_020_Child2_MarjooiSale);
                dataSet_01_Sale.EnforceConstraints = false;



                if (sender == bt_Save || sender == this)
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                bt_New.Enabled = true;
                this.Cursor = Cursors.Default;
           
                   
              
            }
           
          

        }
        private void checkbarcode()
        {
            //int Count = 0;
            Int64 barcode = 0;
            string barcoderepeat = "0";
            PWHRSbool = false;
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {

                if (item.Cells["Column34"].Text != "")
                {

                    DataTable dtsale = clDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, dbo.Table_70_DetailOtherPWHRS.TypeCloth, 
                              dbo.Table_70_DetailOtherPWHRS.Count, dbo.Table_65_HeaderOtherPWHRS.Recipt, dbo.Table_70_DetailOtherPWHRS.weight, 
                               " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, 
                               " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh
                              FROM          " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients INNER JOIN
                              dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                              dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK ON 
                              " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid = dbo.Table_70_DetailOtherPWHRS.CodeCommondity
                        WHERE     (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (dbo.Table_70_DetailOtherPWHRS.Barcode = " + item.Cells["Column34"].Text + ")");



                    if (dtsale.Rows.Count > 0)
                    {



                        item.BeginEdit();
                        item.Cells["Column34"].Value = dtsale.Rows[0][0].ToString();
                        item.Cells["column02"].Value = dtsale.Rows[0][1].ToString();
                        item.Cells["column03"].Value = dtsale.Rows[0][7].ToString();
                        item.Cells["GoodCode"].Value = dtsale.Rows[0][1].ToString();
                        item.Cells["column06"].Value = dtsale.Rows[0][3].ToString();
                        item.Cells["column07"].Value = dtsale.Rows[0][3].ToString();
                        item.Cells["Column36"].Value = dtsale.Rows[0][5].ToString();
                        item.Cells["Column37"].Value = dtsale.Rows[0][5].ToString();
                        item.EndEdit();
                    }




                }

            }
        }





        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {

                table_018_MarjooiSaleBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_Enter(object sender, EventArgs e)
        {
            try
            {

                table_018_MarjooiSaleBindingSource.EndEdit();
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column02")
                {

                    gridEX_Extra.SetValue("column05", (gridEX_Extra.DropDowns["Type"].GetValue("column02")));
                    gridEX_Extra.SetValue("column04", "0");
                    gridEX_Extra.SetValue("column03", "0");

                    if (gridEX_Extra.DropDowns["Type"].GetValue("column03").ToString() == "True")
                    {
                        gridEX_Extra.SetValue("column04", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                    }
                    else
                    {

                        gridEX_Extra.SetValue("column03", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                        Double darsad;
                        darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());

                        Double kol;
                        kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                        if (kol == 0)
                            return;
                        gridEX_Extra.SetValue("column04", kol * darsad / 100);
                    }
                }
                else if (e.Column.Key == "column03")
                {
                    Double darsad;
                    darsad = Convert.ToDouble(gridEX_Extra.GetValue("Column03").ToString());
                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Extra.SetValue("column04", kol * darsad / 100);
                }
            }
            catch { }
            try
            {
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch { }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 78))
            {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                try
                {

                    if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0")
                    {
                        MessageBox.Show(" برای این فاکتور رسید صادر شده است");
                        return;
                    }
                    if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
                    {
                        MessageBox.Show("برای این فاکتور سند صادر شده است");
                        return;
                    }



                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;

                    string RowID = Row["ColumnId"].ToString();
                    int DocID = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column10", RowID);
                    int ResidID = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", RowID);
                    int SaleID = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column17", RowID);


                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (DocID > 0)
                        {
                            if (clDoc.SanadType(ConAcnt.ConnectionString, DocID, int.Parse(RowID), 29) != 29)
                            {
                                clDoc.IsFinal_ID(DocID);
                                //حذف سند فاکتور 
                                //  clDoc.DeleteDetail_ID(DocID, 17, int.Parse(RowID));

                                Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=17 and Column17=" + int.Parse(RowID));
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=17 and Column17=" + int.Parse(RowID);





                                //حذف سند مربوط به رسید
                                // clDoc.DeleteDetail_ID(DocID, 27, ResidID);

                                Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=27 and Column17=" + ResidID);
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=27 and Column17=" + ResidID;




                            }
                            else throw new Exception("این فاکتور از بخش تسویه فاکتورها صادر شده است. جهت حذف از قسمت مربوط اقدام کنید");
                        }
                        if (ResidID > 0)
                        {
                            //درج صفر در شماره سند رسید و صفر در شماره فاکتور مرجوعی رسید
                            //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET Column07=0 , Column14=0 where ColumnId=" + ResidID);

                            command += " UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt SET Column07=0,Column14=0  where ColumnId=" + ResidID;


                        }

                        if (SaleID > 0)
                        {
                            // clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column19=0 , Column20=0 where ColumnId=" + SaleID);
                            command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column19=0,Column20=0  where ColumnId=" + SaleID;



                        }

                        //حذف فاکتور
                        //foreach (DataRowView item in this.table_019_Child1_MarjooiSaleBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                        //this.table_019_Child1_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_019_Child1_MarjooiSale);
                        //foreach (DataRowView item in this.table_020_Child2_MarjooiSaleBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                        //this.table_020_Child2_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_020_Child2_MarjooiSale);
                        //this.table_018_MarjooiSaleBindingSource.RemoveCurrent();
                        //this.table_018_MarjooiSaleBindingSource.EndEdit();
                        //this.table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);

                        command += " Delete from " + ConSale.Database + ".dbo.Table_020_Child2_MarjooiSale  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_019_Child1_MarjooiSale  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_018_MarjooiSale  Where columnid =" + int.Parse(RowID);



                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = command;
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();

                                Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                                dataSet_01_Sale.EnforceConstraints = false;
                                this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, 0);
                                this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, 0);
                                this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, 0);
                                dataSet_01_Sale.EnforceConstraints = true;

                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                this.Cursor = Cursors.Default;
                                Class_BasicOperation.CheckExceptionType(es, this.Name);

                            }

                            this.Cursor = Cursors.Default;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
            }
             else

                 Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void FinalSave_Click(object sender, EventArgs e)
        {
            try{

                SaveEvent(sender, e);


                if (messege== true)
                {
                    MessageBox.Show("لطفا موجودی را بررسی کنید");
                    return;
                }



                if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                {
                    throw new Exception("خطا در ثبت اطلاعات");
                }

                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {
                   
                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 108))
                            throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                        
                        SaveEvent(sender, e);


                        string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column10", RowID) != 0)
                        {

                            dataSet_01_Sale.EnforceConstraints = false;
                            this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                            this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                            this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                            dataSet_01_Sale.EnforceConstraints = true;
                            this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                            throw new Exception("برای این فاکتور سند صادر شده است");
                        }

                        SaveEvent(sender, e);
                        if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", RowID) != 0)
                        {
                            //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, false, false, int.Parse(RowID));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, false, true, int.Parse(RowID));
                                frm.ShowDialog();
                            }

                        }
                        //اگر رسید صادر نشده باشد
                        else
                        {
                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, true, false, int.Parse(RowID));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, true, true, int.Parse(RowID));
                                frm.ShowDialog();
                            }
                        }

                        mlt_Recipt.DataSource = clDoc.ReturnTable(ConWare, @"select Columnid,Column01 from Table_011_PwhrsReceipt");
                        mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);

                    }
              
                   
        }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {

        }

        private void bt_DelDoc_Click_2(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 109))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DocID = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column10", RowID.ToString());
                    int ResidID = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", RowID.ToString());

                    if (DocID > 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";
                        if (ResidID > 0)
                        {
                            Message = "برای این فاکتور، رسید انبار نیز صادر شده است. در صورت تأیید ثبت مربوط به رسید نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف سند فاکتور 
                            int Count = clDoc.DeleteDetail_ID(DocID, 17, RowID);
                            if (Count > 0)
                            {
                                clDoc.IsFinal_ID(DocID);
                                //حذف سند مربوط به رسید
                                //clDoc.DeleteDetail_ID(DocID, 27, ResidID);

                                Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=27 and Column17=" + ResidID);
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=27 and Column17=" + ResidID;




                                //درج صفر در شماره سند رسید
                                // clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET Column07=0 where ColumnId=" + ResidID);

                                command += " UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt SET Column07=0 where ColumnId=" + ResidID;


                                //آپدیت شماره سند  در فاکتور
                                //  clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", RowID, 0);

                                command += " UPDATE " + ConSale.Database + ".dbo.Table_018_MarjooiSale SET Column10=0 where ColumnId=" + RowID;


                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                                {
                                    Con.Open();

                                    SqlTransaction sqlTran = Con.BeginTransaction();
                                    SqlCommand Command = Con.CreateCommand();
                                    Command.Transaction = sqlTran;
                                    try
                                    {
                                        Command.CommandText = command;
                                        Command.ExecuteNonQuery();
                                        sqlTran.Commit();
                                        Class_BasicOperation.ShowMsg("", "حذف سند حسابداری با موفقیت صورت گرفت", "Information");

                                    }
                                    catch (Exception es)
                                    {
                                        sqlTran.Rollback();
                                        this.Cursor = Cursors.Default;
                                        Class_BasicOperation.CheckExceptionType(es, this.Name);
                                    }
                                    this.Cursor = Cursors.Default;
                                }
                            }
                            else throw new Exception("این فاکتور از بخش تسویه فاکتورها صادر شده است. جهت حذف از قسمت مربوط اقدام کنید");
                        }
                    }
                    mlt_Doc.DataSource= clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead").ToString();
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, RowID);
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, RowID);
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, RowID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                    
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void table_018_MarjooiSaleBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column09"].ToString() != "0" || ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column10"].ToString() != "0")
                {
                    uiPanel0.Enabled = false;
                    gridEX_List.AllowEdit = InheritableBoolean.False;
                    gridEX_Extra.AllowDelete = InheritableBoolean.False;
                }
                else
                {
                    uiPanel0.Enabled = true;
                    gridEX_List.AllowEdit = InheritableBoolean.True;
                    gridEX_Extra.AllowDelete = InheritableBoolean.True;
                }
            }
            catch
            {


            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                table_018_MarjooiSaleBindingSource.EndEdit();
                this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

                if (dataSet_01_Sale.Table_018_MarjooiSale.GetChanges() != null || dataSet_01_Sale.Table_019_Child1_MarjooiSale.GetChanges() != null ||
                        dataSet_01_Sale.Table_020_Child2_MarjooiSale.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent(sender, e);

                        if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");
                        }

                    }
                }
                if (isadmin)
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_018_MarjooiSale),0) as Row");
                    
                }
                else
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_018_MarjooiSale where column13='"+Class_BasicOperation._UserName+"'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {

                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

                    if (dataSet_01_Sale.Table_018_MarjooiSale.GetChanges() != null || dataSet_01_Sale.Table_019_Child1_MarjooiSale.GetChanges() != null ||
                        dataSet_01_Sale.Table_020_Child2_MarjooiSale.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                            if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }
                    if (isadmin)
                    {
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_018_MarjooiSale where Column01>" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                        
                    }
                    else
                    {
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_018_MarjooiSale where Column01>" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND Column13='"+Class_BasicOperation._UserName+"'),0) as Row");

                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                try
                {
                    table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();

                    if (dataSet_01_Sale.Table_018_MarjooiSale.GetChanges() != null || dataSet_01_Sale.Table_019_Child1_MarjooiSale.GetChanges() != null ||
                       dataSet_01_Sale.Table_020_Child2_MarjooiSale.GetChanges() != null)

                    {

                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                            if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }

                    if (isadmin)
                    {
                        Table = clDoc.ReturnTable(ConSale,
                        "Select ISNULL((Select max(Column01) from Table_018_MarjooiSale where Column01<" +
                        ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = clDoc.ReturnTable(ConSale,
                        "Select ISNULL((Select max(Column01) from Table_018_MarjooiSale where Column01<" +
                        ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND Column13='"+Class_BasicOperation._UserName+"'),0) as Row");
                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();

                table_018_MarjooiSaleBindingSource.EndEdit();
                this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

               if (dataSet_01_Sale.Table_018_MarjooiSale.GetChanges() != null || dataSet_01_Sale.Table_019_Child1_MarjooiSale.GetChanges() != null ||
                        dataSet_01_Sale.Table_020_Child2_MarjooiSale.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                            if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }

                if (isadmin)
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_018_MarjooiSale),0) as Row");
                    
                }
                else
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_018_MarjooiSale where Column13='"+Class_BasicOperation._UserName+"'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    bt_New.Enabled = true;
                    gridEX_Extra.UpdateData();
                    gridEX_List.UpdateData();

                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    string user = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull ((select column13 from Table_018_MarjooiSale where column01 =" + txt_Search.Text + " ),0)");

                    dataSet_01_Sale.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    if (isadmin)
                    {
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_01_Sale.Table_018_MarjooiSale, RowID);
                        if (table_018_MarjooiSaleBindingSource.Count > 0)
                        {
                            this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_01_Sale.Table_019_Child1_MarjooiSale, RowID);
                            this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_020_Child2_MarjooiSale, RowID);
                        }
                        else
                        {
                            MessageBox.Show("این شماره فاکتور وجود ندارد");
                        }

                    }
                    else if (user == Class_BasicOperation._UserName)
                    {
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_01_Sale.Table_018_MarjooiSale, RowID);
                        if (table_018_MarjooiSaleBindingSource.Count > 0)
                        {
                            this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_01_Sale.Table_019_Child1_MarjooiSale, RowID);
                            this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_020_Child2_MarjooiSale, RowID);
                        }
                        else
                        {
                            MessageBox.Show("این شماره فاکتور وجود ندارد");
                        }
                    }
                    else
                    {
                        MessageBox.Show("شما به این شماره فاکتور دسترسی ندارید");
                    }
                    dataSet_01_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    txt_Search.SelectAll();
                }
            }
        }



        public void bt_Search_1_Click(object sender, EventArgs e)
        {
           
                try
                {
                    bt_New.Enabled = true;
                    gridEX_Extra.UpdateData();
                    gridEX_List.UpdateData();
                    txt_Search.Text = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull ((select column01 from Table_018_MarjooiSale where columnid =" + _ID + " ),0)");
                  
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    string user = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull ((select column13 from Table_018_MarjooiSale where column01 =" + txt_Search.Text + " ),0)");
                    
                        dataSet_01_Sale.EnforceConstraints = false;
                        int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                        if (isadmin)
                        {
                            this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_01_Sale.Table_018_MarjooiSale, RowID);
                            if (table_018_MarjooiSaleBindingSource.Count > 0)
                            {
                                this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_01_Sale.Table_019_Child1_MarjooiSale, RowID);
                                this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_020_Child2_MarjooiSale, RowID);
                            }
                            else
                            {
                                MessageBox.Show("این شماره فاکتور وجود ندارد");
                            }

                        }
                        else if (user == Class_BasicOperation._UserName)
                        {
                            this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_01_Sale.Table_018_MarjooiSale, RowID);
                            if (table_018_MarjooiSaleBindingSource.Count > 0)
                            {
                                this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_01_Sale.Table_019_Child1_MarjooiSale, RowID);
                                this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_020_Child2_MarjooiSale, RowID);
                            }
                            else
                            {
                                MessageBox.Show("این شماره فاکتور وجود ندارد");
                            }
                        }
                        else
                        {
                            MessageBox.Show("شما به این شماره فاکتور دسترسی ندارید");
                        }
                        dataSet_01_Sale.EnforceConstraints = true;
                        txt_Search.SelectAll();
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                    

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    txt_Search.SelectAll();
                }
           
        }



        private void mnu_DelDraft_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {
                    int RowID = int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int ResidId = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", RowID.ToString());

                    if (ResidId > 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 81))
                        {
                           
                            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", mlt_Recipt.Value.ToString()) != 0)

                                throw new Exception(" رسید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");

                            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "case when Column19=1 then 1 else 0 end", mlt_Recipt.Value.ToString()) != 0)

                                throw new Exception(" رسید قطعی می باشد ابتدا آن را غیر قطعی نمایید");

                            int NumPWHRS = Convert.ToInt32(clDoc.ExScalar(ConWare.ConnectionString, @"SELECT     TOP (1)  dbo.Table_011_PwhrsReceipt.column03, dbo.Table_011_PwhrsReceipt.columnid
FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                      dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
WHERE     (dbo.Table_011_PwhrsReceipt.columnid = "+mlt_Recipt.Value+")"));


                              string good = string.Empty;
                    bool ok = true;
                    //چک باقی مانده کالا
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {

                       float Remain = FirstRemain(int.Parse(item.Cells["column02"].Value.ToString()), item.Cells["column32"].Value.ToString(), NumPWHRS.ToString());
                       if (Remain < Convert.ToDouble(0) || FirstRemainTotal(int.Parse(item.Cells["column02"].Value.ToString()), item.Cells["column32"].Value.ToString(), NumPWHRS.ToString()) < Convert.ToDouble(0))
                        {
                            good += item.Cells["Column02"].Text + ",";

                        }
                    }

                    if (!string.IsNullOrWhiteSpace(good))
                    {
                        good = good.TrimEnd(',');

                        string Message1 = "";
                        Message1 = "موجودی کالاهای زیر منفی می شود آیا مایل به حذف رسید هستید؟" + Environment.NewLine + good;
                        if (DialogResult.Yes == MessageBox.Show(Message1, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            ok = true;
                        else
                            ok = false;

                    }

                    if (ok)
                    {

                        if (MessageBox.Show("آیا از حذف این رسید انبار مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //clDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_012_Child_PwhrsReceipt where Column01=" + mlt_Recipt.Value);
                            //clDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_011_PwhrsReceipt Where ColumnId=" + mlt_Recipt.Value);
                            //clDoc.RunSqlCommand(ConSale.ConnectionString, "Update Table_018_MarjooiSale set Column09=0 Where ColumnId=" + txt_ID.Text);
                            string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select Column09 from Table_018_MarjooiSale where ColumnId = " + txt_ID.Text + @")

                                                    Delete from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt where columnid in(
                                                    select Column09 from Table_018_MarjooiSale where Columnid = " + txt_ID.Text + @")

                                                    Update Table_018_MarjooiSale set Column09=0  where ColumnId = " + txt_ID.Text + "";

                            Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PSALE, CommandTexxt);
                            MessageBox.Show("اطلاعات با موفقیت حذف گردید");




                        }
                    }
                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رسید انبار را ندارید", "None");
                    }

                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_01_Sale.Table_018_MarjooiSale, RowID);
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_01_Sale.Table_019_Child1_MarjooiSale, RowID);
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_020_Child2_MarjooiSale, RowID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);

                   
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void غیرقطعیکردنرسیدانبارToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                int ReceiptId = clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 69))
                {
                    string Message = null;

                    if (clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column19", "ColumnId", ReceiptId.ToString()) == "True")
                    {
                        Message = "آیا مایل به غیر قطعی کردن رسید انبار هستید؟";
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET Column19=0 where ColumnId=" +
                               ReceiptId);
                            Class_BasicOperation.ShowMsg("", "غیرقطعی کردن رسید انبار با موفقیت انجام گرفت", "Information");
                        }

                    }
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان غیر قطعی کردن رسید انبار را ندارید", "None");
            }
        }
           private float FirstRemain(int GoodCode, string Barcode, string Ware)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT				 ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date , dbo.Table_012_Child_PwhrsReceipt.Column30 AS Grease
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE     (dbo.Table_011_PwhrsReceipt.column03 = {3}) AND  (dbo.Table_012_Child_PwhrsReceipt.column02 = {0}) 
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.Column30
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date, dbo.Table_008_Child_PwhrsDraft.Column30 AS Grease
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE   (dbo.Table_007_PwhrsDraft.column03 = {3}) AND   (dbo.Table_008_Child_PwhrsDraft.column02 = {0})  
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.Column30) AS derivedtbl_1
                       WHERE     (Date <= '{1}' and Grease={2} ) ";
                CommandText = string.Format(CommandText, GoodCode, txt_Dat.Text, Barcode, Ware);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

     private float FirstRemainTotal(int GoodCode, string Barcode, string Ware)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT				 ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date , dbo.Table_012_Child_PwhrsReceipt.Column30 AS Grease
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE     (dbo.Table_011_PwhrsReceipt.column03 = {2}) AND  (dbo.Table_012_Child_PwhrsReceipt.column02 = {0}) 
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.Column30
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date, dbo.Table_008_Child_PwhrsDraft.Column30 AS Grease
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE   (dbo.Table_007_PwhrsDraft.column03 = {2}) AND   (dbo.Table_008_Child_PwhrsDraft.column02 = {0})  
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.Column30) AS derivedtbl_1
                       WHERE     ( Grease={1} ) ";
                CommandText = string.Format(CommandText, GoodCode, Barcode, Ware);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

     private void gridEX_List_FormattingRow(object sender, RowLoadEventArgs e)
     {

     }

     private void bt_ExportDoc_Click(object sender, EventArgs e)
     {
         if (this.table_018_MarjooiSaleBindingSource.Count > 0)
         {
             try
             {
                 if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 108))
                     throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                 // if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                 SaveEvent(sender, e);


                 string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                 if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column10", RowID) != 0)
                 {
                     
                     mlt_Recipt.DataSource=(clDoc.ReturnTable(ConWare, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"));
                     mlt_Doc.DataSource=(clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"));
             
                     dataSet_01_Sale.EnforceConstraints = false;
                     this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                     this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                     this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                     dataSet_01_Sale.EnforceConstraints = true;
                     this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                     throw new Exception("برای این فاکتور سند صادر شده است");
                 }

                 SaveEvent(sender, e);
                 if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", RowID) != 0)
                 {
                     //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                     //سیستم ادواری
                     if (!Class_BasicOperation._FinType)
                     {
                         MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, false, false, int.Parse(RowID));
                         frm.ShowDialog();
                     }
                     //سیستم دائمی
                     else
                     {
                         MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, false, true, int.Parse(RowID));
                         frm.ShowDialog();
                     }

                 }
                 //اگر رسید صادر نشده باشد
                 else
                 {
                     //سیستم ادواری
                     if (!Class_BasicOperation._FinType)
                     {
                         MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, true, false, int.Parse(RowID));
                         frm.ShowDialog();
                     }
                     //سیستم دائمی
                     else
                     {
                         MarjooiSale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, true, true, int.Parse(RowID));
                         frm.ShowDialog();
                     }
                 }

                 mlt_Recipt.DataSource = (clDoc.ReturnTable(ConWare, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"));
                 mlt_Doc.DataSource = (clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"));

                 dataSet_01_Sale.EnforceConstraints = false;
                 this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                 this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                 this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                 dataSet_01_Sale.EnforceConstraints = true;
                 this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                 
             }
             catch (Exception ex)
             {
                 Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
             }
         }
     }

     private void bt_ExportResid_Click(object sender, EventArgs e)
     {
         if (this.table_018_MarjooiSaleBindingSource.Count > 0)
         {
             string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
             try
             {

                 if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 110))
                     throw new Exception("کاربر گرامی شما امکان صدور رسید انبار را ندارید");
                 SaveEvent(sender, e);


                 if (clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID) != 0)
                     throw new Exception("برای این فاکتور رسید انبار صادر شده است");


                 DataTable GoodTable = new DataTable();
                 GoodTable.Columns.Add("GoodId", Type.GetType("System.String"));
                 GoodTable.Columns.Add("GoodName", Type.GetType("System.String"));
                 foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                 {
                     GoodTable.Rows.Add(item.Cells["Column02"].Value.ToString(),
                         item.Cells["Column02"].Text.Trim());
                 }

                 MarjooiSale.Frm_014_ResidDialog_Return frm = new Frm_014_ResidDialog_Return(GoodTable);

                 if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                 {
                     string Function = frm.FunctionValue;
                     string Ware = frm.WareCode.ToString();
                     int ResidNum = 0;
                     if (frm.residnum > 0)
                         ResidNum = frm.residnum;
                     else
                         ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                     //, int.Parse(Ware));

                     //**Resid Header
                     #region
                     SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                     key.Direction = ParameterDirection.Output;
                     using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.PWHRS))
                     {
                         DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;
                         conware.Open();
                         SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          ) VALUES (" + ResidNum + ",'" + Row["Column02"].ToString() + "'," +
                         Ware + "," + Function + "," + Row["Column03"].ToString() + ",'" + "رسید صادر شده از فاکتور مرجوعی ش " +
                          Row["Column01"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0," + Row["ColumnId"].ToString() + ",0," +
                           (Row["Column12"].ToString().Trim() == "True" ? "1" : "0") + "," +
                          (Row["Column23"].ToString().Trim() == "" ? "NULL" : Row["Column23"].ToString().Trim()) + "," +
                          Row["Column24"].ToString() + ",1,null); SET @Key=Scope_Identity()", conware);
                         Insert.Parameters.Add(key);
                         Insert.ExecuteNonQuery();
                         int ResidId = int.Parse(key.Value.ToString());
                     #endregion



                         //Resid Detail


                         //اگر فاکتور مرجوعی فاقد شماره فاکتور فروش باشد
                         //ارزش کالا به صورت آخرین ارزش حواله  بزرگتر از صفر در انبار درج می شود
                         //در تاریخ 980520قرار شد ارزش رسید از پروسیجر میانیگن خوانده شود در صورت صفر بود اخرین ارزش حواله بزرگتر صفر خوانده شود
                         #region
                         if (((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column17"].ToString() == "0")
                         {

                             foreach (DataRowView item in table_019_Child1_MarjooiSaleBindingSource)
                             {
                                 SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35],Column36,Column37) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                                  item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                  item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                                  (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                  + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                                  + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                  item["Column15"].ToString()
                                  + ",0,0,0," +
                                  (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                                  (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                                  "," + item["Column30"].ToString() + "," +
                                  item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + "," +
                                   (item["Column36"].ToString().Trim() == "" ? "NULL" : "'" + item["Column36"].ToString() + "'") + "," +
                                  (item["Column37"].ToString().Trim() == "" ? "NULL" : "'" + item["Column37"].ToString() + "'") +

                                  ")", conware);
                                 InsertDetail.ExecuteNonQuery();
                             }

                             SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_012_Child_PwhrsReceipt where Column01=" + ResidId, conware);
                             DataTable Table = new DataTable();
                             goodAdapter.Fill(Table);

                             foreach (DataRow item in Table.Rows)
                             {
                                 if (Class_BasicOperation._WareType)
                                 {
                                     SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware, conware);
                                     DataTable TurnTable = new DataTable();
                                     Adapter.Fill(TurnTable);
                                     TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'";
                                     if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                     {
                                         int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'").ToString());
                                         DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                         SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                             + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                             Convert.ToDouble(item["Column07"].ToString())
                                             + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                         UpdateCommand.ExecuteNonQuery();
                                     }
                                 }
                                 else
                                 {
                                     SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + ResidId, conware);
                                     DataTable TurnTable = new DataTable();
                                     Adapter.Fill(TurnTable);
                                     if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                     {
                                         SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                 + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                         UpdateCommand.ExecuteNonQuery();
                                     }
                                     else
                                     {
                                         Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Ware + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                         TurnTable = new DataTable();
                                         Adapter.Fill(TurnTable);
                                         if (TurnTable.Rows.Count > 0)
                                         {
                                             SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                               + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                             UpdateCommand.ExecuteNonQuery();
                                         }

                                     }

                                 }
                             }

                         }
                         #endregion



                         //اگر فاکتور مرجوعی فروش دارای شماره فاکتور فروش بود
                         // ارزش رسید بر اساس ارزش کالا در حواله مربوط به فاکتور فروش خوانده می شود
                         #region
                         else
                         {
                             DataTable DraftChildTable = clDoc.ReturnTable(conware, "Select * from Table_008_Child_PwhrsDraft where Column01=" +
                                 clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column17"].ToString()));
                             foreach (DataRowView item in table_019_Child1_MarjooiSaleBindingSource)
                             {
                                 DataRow[] FoundRows = DraftChildTable.Select("Column02=" + item["Column02"].ToString());
                                 //اگر در حواله مذکور چنین کالایی موجود باشد از اولین کالا مقدار ارزش واحد خوانده می شود 
                                 if (FoundRows.Length > 0)
                                 {
                                     Double SingleValue = Convert.ToDouble(FoundRows[0]["Column15"].ToString());

                                     SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                                  item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                  item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                                  (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                  + "',getdate(),0," + SingleValue + "," + Convert.ToDouble(item["Column07"].ToString()) * SingleValue
                                  + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                  item["Column15"].ToString()
                                  + ",0,0,0," +
                         (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                         (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                                  item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", conware);
                                     InsertDetail.ExecuteNonQuery();
                                 }
                                 else
                                 //اگر چنین کالایی در حواله نباشد ارزش بر اساس پروسجر محاسبه می شود
                                 {
                                     SqlParameter DetailKey = new SqlParameter("DetailKey", SqlDbType.Int);
                                     DetailKey.Direction = ParameterDirection.Output;
                                     SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                                  item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                  item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                                  (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                  + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                                  + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                  item["Column15"].ToString()
                                  + ",0,0,0," +
                         (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                         (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                                  item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + "); SET @DetailKey=SCOPE_IDENTITY()", conware);
                                     InsertDetail.Parameters.Add(DetailKey);
                                     InsertDetail.ExecuteNonQuery();
                                     int DetailId = int.Parse(DetailKey.Value.ToString());

                                     if (Class_BasicOperation._WareType)
                                     {
                                         SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware, conware);
                                         DataTable TurnTable = new DataTable();
                                         Adapter.Fill(TurnTable);
                                         TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0";
                                         if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                         {
                                             int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0").ToString());
                                             DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                             SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                 + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString())
                                                 + " where ColumnId=" + DetailId, conware);
                                             UpdateCommand.ExecuteNonQuery();
                                         }
                                     }
                                     else
                                     {
                                         SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + ResidId, conware);
                                         DataTable TurnTable = new DataTable();
                                         Adapter.Fill(TurnTable);
                                         if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                         {
                                             SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                     + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + DetailId, conware);
                                             UpdateCommand.ExecuteNonQuery();
                                         }
                                         else
                                         {
                                             Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Ware + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                             TurnTable = new DataTable();
                                             Adapter.Fill(TurnTable);
                                             if (TurnTable.Rows.Count > 0)
                                             {
                                                 SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                   + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + DetailId, conware);
                                                 UpdateCommand.ExecuteNonQuery();
                                             }
                                         }

                                     }
                                 }
                             }
                         }
                         #endregion


                         //درج شماره رسید در فاکتور مرجوعی
                         
                         mlt_Recipt.DataSource=(clDoc.ReturnTable(ConWare, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"));
                         Row["Column09"] = ResidId;
                         this.table_018_MarjooiSaleBindingSource.EndEdit();
                         this.table_018_MarjooiSaleTableAdapter.Update(dataSet_01_Sale.Table_018_MarjooiSale);

                         table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                         Class_BasicOperation.ShowMsg("", "ثبت رسید انبار با موفقیت انجام شد", "Information");
                     }
                 }
             }

             catch (Exception ex)
             {
                 Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
             }
           
              mlt_Recipt.DataSource=(clDoc.ReturnTable(ConWare, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"));
              mlt_Doc.DataSource=(clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"));
             dataSet_01_Sale.EnforceConstraints = false;
             this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_018_MarjooiSale, int.Parse(RowID));
             this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_01_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
             this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
             dataSet_01_Sale.EnforceConstraints = true;
             this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
             
         }
     }

     private void toolStripButton7_Click(object sender, EventArgs e)
     {

     }

     private void Frm_001_MarjooiSale_KeyDown(object sender, KeyEventArgs e)
     {
         if (e.KeyCode == Keys.S && e.Control)
             bt_Save_Click(sender, e);
         else if (e.KeyCode == Keys.N && e.Control && bt_New.Enabled)
         {
             bt_New_Click(sender, e);

         }
         else if (e.Control && e.KeyCode == Keys.D)
             bt_Del_Click(sender, e);
         else if (e.Control && e.KeyCode == Keys.F)
         {
             txt_Search.Select();
             txt_Search.SelectAll();
         }
         else if (e.Control && e.KeyCode == Keys.P)
             bt_Print_Click(sender, e);
           
     }

     private void مشاهدهحوالهانبارToolStripMenuItem_Click(object sender, EventArgs e)
     {

     }

     private void bt_DefineSignatures_Click(object sender, EventArgs e)
     {
         if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
         {
             PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
             PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
             PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
             PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;

             PSALE._05_Sale.Frm_020_ReturnSale_Signatures frm = new PSALE._05_Sale.Frm_020_ReturnSale_Signatures();
             frm.ShowDialog();
         }
         else
             Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
     }

     private void معرفیاضافاتوکسوراتفروشToolStripMenuItem_Click(object sender, EventArgs e)
     {
         if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
         {
             PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
             PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
             PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
             PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;
             PSALE._02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new PSALE._02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
             ob.ShowDialog();
             SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
             DS.Tables["Discount"].Rows.Clear();
             Adapter.Fill(DS, "Discount");
         }
         else
             Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
     }

     private void مشاهدهرسیدهایهایانبارToolStripMenuItem_Click(object sender, EventArgs e)
     {
         PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;
         PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
         PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
         PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
         PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
         PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

         if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
         {
             foreach (Form item in Application.OpenForms)
             {
                 if (item.Name == "Form03_WareReceipt")
                 {

                        ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).txt_Search.Text =
                            (mlt_Recipt.Text.Trim() != "" ?
                             mlt_Recipt.Value.ToString() : "0");


                        item.BringToFront();
                     return;
                 }
             }
             PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt
                 (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                 (this.table_018_MarjooiSaleBindingSource.Count > 0 ? int.Parse(((DataRowView)
                 this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column09"].ToString()) : -1));
             frm.ShowDialog();
             int ReturnSaleId = int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
             dataSet_01_Sale.EnforceConstraints = false;
             this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_01_Sale.Table_018_MarjooiSale, ReturnSaleId);
             this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_01_Sale.Table_019_Child1_MarjooiSale, ReturnSaleId);
             this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_020_Child2_MarjooiSale, ReturnSaleId);
             dataSet_01_Sale.EnforceConstraints = true;
             
         }
         else
             Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
     }

     private void مشاهدهاسنادحسابداریToolStripMenuItem_Click(object sender, EventArgs e)
     {
         int SanadId = 0;
         if (this.table_018_MarjooiSaleBindingSource.Count > 0)
             SanadId = (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString()));
         PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
         PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
         PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
         PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

         if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
         {
             foreach (Form item in Application.OpenForms)
             {
                 if (item.Name == "Form01_AccDocument")
                 {
                     item.BringToFront();
                     TextBox txt_S = (TextBox)item.ActiveControl;
                     txt_S.Text = SanadId.ToString();
                     PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
                     frms.bt_Search_Click(sender, e);
                     return;
                 }
             }
             PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
               UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(SanadId.ToString()));
             try
             {
                 frm.MdiParent = Frm_Main.ActiveForm;
             }
             catch { }
             frm.Show();
         }
         else
             Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
     }

     private void مشاهدهفاکتورهایفروشToolStripMenuItem_Click(object sender, EventArgs e)
     {
         if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 67))
         {
             foreach (Form item in Application.OpenForms)
             {
                 if (item.Name == "Frm_003_ViewFactorSale")
                 {
                     item.BringToFront();
                     return;
                 }
             }
             _002_Sale.Frm_003_ViewFactorSale frm = new _002_Sale.Frm_003_ViewFactorSale();
             try
             {
                 frm.MdiParent = Frm_Main.ActiveForm;
             }
             catch { }
             frm.Show();
         }
         else
             Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
     }

     private void toolStripButton1_Click(object sender, EventArgs e)
     {
        
     }

     private void bt_Print_Click(object sender, EventArgs e)
     {
         if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 130))
         {
             PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
             PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
             PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
             PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;

             PSALE._05_Sale.Reports.Form_ReturnSaleFactorPrint frm = new PSALE._05_Sale.Reports.Form_ReturnSaleFactorPrint(
                 int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.
                 CurrencyManager.Current)["Column01"].ToString()));
             frm.ShowDialog();
         }
         else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
     }


     private void txt_Search_KeyPress_1(object sender, KeyPressEventArgs e)
     {

         if (Class_BasicOperation.isNotDigit(e.KeyChar))
             e.Handled = true;
         else if (e.KeyChar == 13)
             bt_Search_Click(sender, e);
     }

     private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
     {
         MarjooiSale.Frm_002_ViewFactorReturn frm = new MarjooiSale.Frm_002_ViewFactorReturn();
         frm.ShowDialog();
     }

            
    }
}


           
   



