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

namespace PCLOR._002_Sale
{
    public partial class Frm_002_StoreFaktor : Form
    {
        bool _del;
        bool search = false;
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
       Classes. Class_UserScope UserScope = new Classes.Class_UserScope();
        DataSet DS = new DataSet();
        SqlDataAdapter DraftAdapter, DocAdapter, ReturnAdapter;
        string ReturnDate = null;
        InputLanguage original;
        bool SalePrice, DiscountLiner, DiscountEnd = false;
        SqlParameter ReturnDocNum;
        decimal sum;
        string ficloth;
        string ficolor;
        string SelectBrand;
        DataTable dtsale = new DataTable();
       

        public Frm_002_StoreFaktor(bool del)
        {
            _del = del;
            InitializeComponent();
        }
        public Frm_002_StoreFaktor(bool del, int ID , bool _Search)
        {
            _del = del;
            _ID = ID;
            search = _Search;

            InitializeComponent();
        }

        public void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {

            mlt_Draft.DataSource = clDoc.ReturnTable(ConWare, @"select Columnid,Column01 from Table_007_PwhrsDraft");
            mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");


            clDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
            clDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
            clDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
            clDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

            DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            DocAdapter.Fill(DS, "Doc");
            

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            //gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");
            gridEX_Extra.DropDowns["Type"].DataSource = clDoc.ReturnTable(ConSale, @"select * from Table_024_Discount");
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            DataTable dt = clDoc.ReturnTable(ConPCLOR, "select * from Table_80_Setting");

            DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                     @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");

            DataTable PersonTable = clDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
            mlt_Ware.DataSource = WareTable;
            mlt_NameCustomer.DataSource = PersonTable;
            mlt_Function.DataSource = clDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");


            gridEX_List.DropDowns["Codecommodity"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
            gridEX_List.DropDowns["Namecommodity"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId,Column02 from table_004_CommodityAndIngredients");

            gridEX_List.DropDowns["UnitCount"].DataSource = clDoc.ReturnTable(ConBase, @"select Column00,Column01 from Table_070_CountUnitInfo");

            if (search)
            {
                
           
                bt_Search_1_Click(sender, e);

            }
            
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {

               

                    table_010_SaleFactorBindingSource.AddNew();
                    txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                    mlt_Function.Value = Properties.Settings.Default.TypePWHRS;
                    ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column13"] = Class_BasicOperation._UserName;
                    ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column14"] = Class_BasicOperation.ServerDate().ToString();
                    mlt_NameCustomer.Focus();
                    bt_New.Enabled = false;
                    mlt_NameCustomer.Text = "";
                    txt_Description.Text = "";
                    txt_Barcode.Text = "";
                    //mlt_Function_S.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=6");
                    //table_65_HeaderOtherPWHRSBindingSource_PositionChanged(sender, e);
             
   

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void Save_Event(object sender, EventArgs e)
        {
            gridEX_List.UpdateData();
            gridEX_Extra.UpdateData();

            if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0")
            {
                MessageBox.Show("این فاکتور حواله صادر شده است");
                return;
            }
            if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
            {
                MessageBox.Show("این فاکتور سند صادر شده است");
                return;
            }



            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
               gridEX_List.AllowEdit == InheritableBoolean.True &&
               mlt_NameCustomer.Text != "" && mlt_Ware.Text != "" && mlt_Function.Text != "")
            {
                if (Properties.Settings.Default.ShowPriceAlert > 0)
                    CheckGoodsPrice();
                this.Cursor = Cursors.WaitCursor;


                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    return;
                }


                if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                {
                    txt_Number.Text = clDoc.MaxNumber(Properties.Settings.Default.PSALE, " Table_010_SaleFactor", "Column01").ToString();

                }
                this.table_010_SaleFactorBindingSource.EndEdit();
                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    this.Cursor = Cursors.Default;

                    return;

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    item.BeginEdit();


                    DataTable dts = clDoc.ReturnTable(ConPCLOR, @"SELECT    " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 AS CodeCommondity, dbo.Table_005_TypeCloth.TypeCloth, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07 AS Count, 0 AS Recipt, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column35 AS weight, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column03 AS vahedshomaresh, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 AS TypeColor, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37 AS Machine, dbo.Table_005_TypeCloth.ID, 
                       ISNULL((dbo.Table_010_TypeColor.ID),0) AS IDColor, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02
                    FROM         " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid INNER JOIN
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = dbo.Table_005_TypeCloth.CodeCommondity LEFT OUTER JOIN
                      dbo.Table_010_TypeColor ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 = dbo.Table_010_TypeColor.TypeColor
                    WHERE     (" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 =  " + item.Cells["Column34"].Text + ")ORDER BY " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid DESC, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 DESC");

                    ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dts.Rows[0][10].ToString() + ")),0)");
                    ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + dts.Rows[0][11].ToString() + ")),0)");
                    SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dts.Rows[0][10].ToString() + ")),0)");
                    sum = Convert.ToDecimal(ficloth) + Convert.ToDecimal(ficolor);


                    decimal fi = Convert.ToDecimal((item.Cells["Column10"].Value.ToString()));
                    decimal Weight = Convert.ToDecimal((item.Cells["Column37"].Value.ToString()));

                    if (SelectBrand.ToString() == "True")
                    {
                        //&& sum <= Convert.ToDecimal(item.Cells["Column10"].Value.ToString())
                        gridEX_List.SetValue("column20", fi * Weight);
                        gridEX_List.SetValue("column11", fi * Weight);


                    }

                    else if (SelectBrand.ToString() == "False")
                    {
                        //&& Convert.ToDecimal(ficloth) <= Convert.ToDecimal(item.Cells["Column10"].Value.ToString())
                        gridEX_List.SetValue("column20", fi * Weight);
                        gridEX_List.SetValue("column11", fi * Weight);

                    }
                    //else
                    //{
                    //    MessageBox.Show("مقدار وارد شده از مقدار پیشنهادی کمتر می باشد امکان اضافه آن را ندارید");
                    //    return;
                    //}

                    item.EndEdit();
                }


                double Total = double.Parse(txt_TotalPrice.Value.ToString());

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = (Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
                        item.EndEdit();

                    }
                }

                foreach (Janus.Windows.GridEX.GridEXRow item1 in gridEX_List.GetRows())
                {
                    if (double.Parse(item1.Cells["Column10"].Value.ToString()) > 0)
                    {
                        item1.BeginEdit();
                        item1.Cells["Column11"].Value = Convert.ToDouble(item1.Cells["Column37"].Value.ToString())* Convert.ToDouble(item1.Cells["Column10"].Value.ToString());
                        item1.Cells["Column20"].Value = Convert.ToDouble(item1.Cells["Column37"].Value.ToString()) * Convert.ToDouble(item1.Cells["Column10"].Value.ToString());

                        item1.EndEdit();

                    }
                }
                txt_TotalPrice.Value = Convert.ToDouble(
                 gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                 AggregateFunction.Sum).ToString());
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                Convert.ToDouble(txt_Extra.Value.ToString()) -
                Convert.ToDouble(txt_Reductions.Value.ToString());

                double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
                   gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                int CustomerCode = int.Parse(Row["Column03"].ToString());
                string Date = Row["Column02"].ToString();
                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column28"] = NetTotal.ToString();
                if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column12"].ToString() == "False")
                {
                    NetTotal = ClDiscount.SpecialGroup(
                        Convert.ToDouble(Row["Column28"].ToString()), CustomerCode, Date);
                    ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column30"] = NetTotal;

                    NetTotal = ClDiscount.VolumeGroup(Convert.ToDouble(Row["Column28"].ToString()) -
                        Convert.ToDouble(Row["Column30"].ToString()), CustomerCode, Date);
                    ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column29"] = NetTotal;

                    NetTotal = ClDiscount.SpecialCustomer(
                        Convert.ToDouble(Row["Column28"].ToString()) - Convert.ToDouble(Row["Column30"].ToString()) -
                        Convert.ToDouble(Row["Column29"].ToString()), CustomerCode, Date);

                    ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column31"] = NetTotal;
                }





                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column15"] = Class_BasicOperation._UserName;
                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column16"] = Class_BasicOperation.ServerDate();
                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                Filter2.Value1 = true;
                ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();


                dataSet_01_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);
                this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
                this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(txt_ID.Text));
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(txt_ID.Text));
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(txt_ID.Text));
                dataSet_01_Sale.EnforceConstraints = true;
                table_010_SaleFactorBindingSource_PositionChanged(sender, e);

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
          string  barcoderepeat = "0";
          PWHRSbool = false;
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {

                if (item.Cells["Column34"].Text != "")
                {

                    
                    DataTable dtsale = clDoc.ReturnTable(ConPCLOR, @"SELECT    " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 AS CodeCommondity, dbo.Table_005_TypeCloth.TypeCloth, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07 AS Count, 0 AS Recipt, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column35 AS weight, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column03 AS vahedshomaresh, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 AS TypeColor, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37 AS Machine, dbo.Table_005_TypeCloth.ID, 
                       ISNULL((dbo.Table_010_TypeColor.ID),0) AS IDColor, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02
                    FROM         " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid INNER JOIN
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = dbo.Table_005_TypeCloth.CodeCommondity LEFT OUTER JOIN
                      dbo.Table_010_TypeColor ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 = dbo.Table_010_TypeColor.TypeColor
                    WHERE     (" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 =  " + item.Cells["Column34"].Text + ")ORDER BY " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid DESC, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 DESC ");
                    if (dtsale.Rows.Count > 0)
                    {
                        float Remain = FirstRemain(int.Parse(dtsale.Rows[0][1].ToString()), (item.Cells["Column34"].Value.ToString()), mlt_Ware.Value.ToString());

                        if (Remain > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column34"].Value=dtsale.Rows[0][0].ToString();
                            item.Cells["column02"].Value=dtsale.Rows[0][1].ToString();
                            item.Cells["column03"].Value = dtsale.Rows[0][7].ToString();
                            item.Cells["GoodCode"].Value=dtsale.Rows[0][1].ToString();
                            item.Cells["column06"].Value=dtsale.Rows[0][3].ToString();
                            item.Cells["column07"].Value = dtsale.Rows[0][3].ToString();
                            item.Cells["Column36"].Value=dtsale.Rows[0][5].ToString();
                            item.Cells["Column37"].Value = dtsale.Rows[0][5].ToString();
                            item.EndEdit();
                        }
                        else 
                        {
                            PWHRSbool = true;
                            errorelist += item.Cells["Column34"].Value.ToString() + ",";
                           
                        }

                        

                    }
                   
                }
            }
            
        }


        bool PWHRSbool;
        string errorelist;
        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Function.Text == "" || mlt_NameCustomer.Text == "" || mlt_Ware.Text == "" || mlt_Ware.Text == "0" || mlt_NameCustomer.Text == "0" || mlt_Function.Text == "0")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                    return;
                }
                //Int64 barcode = 0;
                //errorelist = string.Empty;

                //if (!txt_Number.Text.StartsWith("-"))
                //{ checkbarcode(); }

                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_List.GetRows())
                {
                    if (PWHRSbool)
                    {
                        MessageBox.Show("موجودی کالا های زیر در انبار موردنظر کافی نیست" + Environment.NewLine + errorelist.TrimEnd(','));
                        return;
                    }

                }

                Save_Event(sender, e);

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                this.Cursor = Cursors.Default;
            }
        }

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

        public void bt_Del_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 71))
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    try
                    {
                        if (!_del)
                            throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0" || ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
                        {
                            MessageBox.Show("به دلیل صدور سند و حواله ،امکان حذف آن را ندارید");
                            return;
                        }

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID) != 0)
                        {
                            dataSet_01_Sale.EnforceConstraints = false;
                            this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowID));
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                            dataSet_01_Sale.EnforceConstraints = true;
                            throw new Exception("به علت ارجاع این فاکتور، حذف آن امکانپذیر نمی باشد");
                        }



                        int DocId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID);
                        int DraftId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID);
                        int PrefactorId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column07", RowID);


                        if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            if (DocId > 0)
                            {
                                clDoc.IsFinal_ID(DocId);
                                //حذف سند فاکتور 
                                clDoc.DeleteDetail_ID(DocId, 15, int.Parse(RowID));
                                //حذف سند مربوط به حواله
                                clDoc.DeleteDetail_ID(DocId, 26, DraftId);
                            }

                            //حذف فاکتور
                            foreach (DataRowView item in this.table_011_Child1_SaleFactorBindingSource)
                            {
                                item.Delete();
                            }
                            this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                            this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
                            foreach (DataRowView item in this.table_012_Child2_SaleFactorBindingSource)
                            {
                                item.Delete();
                            }
                            this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                            this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);
                            this.table_010_SaleFactorBindingSource.RemoveCurrent();
                            this.table_010_SaleFactorBindingSource.EndEdit();
                            this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);

                            if (DraftId > 0)
                            {
                                //درج صفر در شماره سند حواله و صفر در شماره فاکتور فروش حواله
                                clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column07=0 , Column16=0 where ColumnId=" + DraftId + "AND Column16= " + RowID);
                            }
                            if (PrefactorId > 0)
                            {
                                //درج صفر در شماره فاکتور فروش پیش فاکتور
                                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_007_FactorBefore set Column12=0 where ColumnId=" + PrefactorId);
                            }

                            Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                            bt_New.Enabled = true;
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

        private void gridEX_List_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {

                table_010_SaleFactorBindingSource.EndEdit();
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

                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
               
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
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
                        (Convert.ToInt64(kol * darsad / 100) ));
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
                       ( Convert.ToInt64(kol * darsad / 100) ));
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
            gridEX_Extra.CurrentCellDroppedDown = true;
        }


       
        private void bt_ExportDraft_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 71))
                        throw new Exception("کاربر گرامی شما امکان صدور حواله انبار را ندارید");

                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID) != 0)
                        throw new Exception("برای این فاکتور حواله صادر شده است");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور حواله وجود ندارد");

                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID) != 0)
                        throw new Exception("به علت مرجوع شدن این فاکتور امکان صدور حواله انبار وجود ندارد");

                    if (clDoc.AllService(table_011_Child1_SaleFactorBindingSource))
                        throw new Exception("به علت عدم وجود کالا، حواله ای برای این فاکتور صادر نخواهد شد");

                    Save_Event(sender, e);



                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodID", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value,
                            item.Cells["Column02"].Text);
                    }

                   
                   

                }

                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                DS.Tables["Draft"].Clear();
                DraftAdapter.Fill(DS, "Draft");
                int _ID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, _ID);
                dataSet_01_Sale.EnforceConstraints = true;
 

            }
        }

       
        //private void bt_Export_Click(object sender, EventArgs e)
        //{
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //    {
        //        try
        //        {
        //            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
        //                throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

        //            //if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
        //            Save_Event(sender, e);

        //            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
        //            {
        //                throw new Exception("خطا در ثبت اطلاعات");

        //            }

        //            string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

        //            if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) != 0)
        //            {
        //                dataSet_01_Sale.EnforceConstraints = false;
        //                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowID));
        //                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
        //                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
        //                dataSet_01_Sale.EnforceConstraints = true;
        //                DS.Tables["Doc"].Clear();
        //                DocAdapter.Fill(DS, "Doc");
        //                DS.Tables["Draft"].Clear();
        //                DraftAdapter.Fill(DS, "Draft");
        //                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

        //                throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
        //            }

        //            if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
        //                throw new Exception("به علت باطل شدن این فاکتور امکان صدور سند وجود ندارد");

        //            Save_Event(sender, e);
        //            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
        //            {
        //                throw new Exception("خطا در ثبت اطلاعات");

        //            }


        //            //بعد از سیو کردن اطلاعات سطر خالی می شود
        //            DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

        //            if (Row["Column12"].ToString() == "True")
        //            {
        //                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
        //                {
        //                    if (item.Cells["Column14"].Text.Trim() == "" || Convert.ToDouble(item.Cells["Column15"].Value.ToString()) <= 0)
        //                        throw new Exception("نوع ارز و ارزش ارز اقلام فاکتور را مشخص کنید");
        //                }
        //            }



        //        }
        //        catch (Exception ex)
        //        {
        //            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
        //        }
        //    }
        //}

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {

        }


        bool Doc ;
        bool Receipt;
        private void bt_ReturnFactor_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    Save_Event(sender, e);

                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                    {
                        throw new Exception("خطا در ثبت اطلاعات");

                    }


                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 106))
                        throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور فروش را ندارید");

                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID) != 0)
                        throw new Exception("این فاکتور قبلا مرجوع شده است");

                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID) == 0)
                        throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و حواله انبار، الزامیست");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        //صدور رسید در صورت صادر شدن حواله برای فاکتور فروش
                        DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                  
                        InsertReceipt(Row);

                        if (ReturnId > 0 && ResidId > 0)
                        {
                            //ثبت عکس فاکتور فروش
                            InvertDoc(Row);
                            if (Doc && Receipt)
                            {
                                Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + ReturnDocNum.Value + Environment.NewLine + "شماره رسید انبار:" + ResidNum, "Information");
                                dataSet_01_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowID));
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                                dataSet_01_Sale.EnforceConstraints = true;
                             
                                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("اطلاعات را درست وارد نمایید");
                            }
                           

                        }
                    }


                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("این فاکتور حواله صادر شده است") ||
                        ex.Message.Contains("این فاکتور سند صادر شده است") ||
                        ex.Message.Contains("این فاکتور قبلا تسویه شده است"))
                    {

                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 72))
                            throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور فروش را ندارید");

                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("این فاکتور قبلا مرجوع شده است");

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID) == 0)
                            throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و حواله انبار، الزامیست");

                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //صدور رسید در صورت صادر شدن حواله برای فاکتور فروش
                            DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                            InsertReceipt(Row);
                            if (ReturnId > 0 && ResidId > 0)
                            {
                                //ثبت عکس فاکتور فروش
                                InvertDoc(Row);
                                Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + ReturnDocNum.Value + Environment.NewLine + "شماره رسید انبار:" + ResidNum, "Information");

                                dataSet_01_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowID));
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                                dataSet_01_Sale.EnforceConstraints = true;
                                //DS.Tables["Return"].Clear();
                                //ReturnAdapter.Fill(DS, "Return");
                                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                                
                            }
                        }


                    }
                    else
                        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }



        private void InsertReceipt(DataRowView Row)
        {
            Receipt = true;
            if (Row["Column09"].ToString() == "0")
                return;


            _002_Sale.Frm_011_ResidInformationDialog frm = new Frm_011_ResidInformationDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                //صدور فاکتور مرجوعی
                TurnBack(Row);


                //DraftTable
                DataTable DraftTable = clDoc.ReturnTable(ConWare, "Select * from Table_007_PwhrsDraft where ColumnId=" + Row["Column09"].ToString());
                DataTable DraftChild = clDoc.ReturnTable(ConWare, "Select * from Table_008_Child_PwhrsDraft where Column01=" + Row["Column09"].ToString());

                string Function = frm.FunctionValue;
                ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                //, int.Parse(DraftTable.Rows[0]["Column03"].ToString()));

                //**Resid Header
                SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                key.Direction = ParameterDirection.Output;
                using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.PWHRS))
                {
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
                                                                          )  VALUES (" + ResidNum + ",'" + ReturnDate + "'," +
                     DraftTable.Rows[0]["Column03"].ToString() + "," + Function + "," + DraftTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
                     ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,0," + ReturnId + ",0," +
                     +(DraftTable.Rows[0]["Column23"].ToString() == "True" ? 1 : 0) + "," +
                            (DraftTable.Rows[0]["Column24"].ToString().Trim() == "" ? "NULL" : DraftTable.Rows[0]["Column24"].ToString()) + "," +
                             DraftTable.Rows[0]["Column25"].ToString()
                     + ",1,null); SET @Key=Scope_Identity()", conware);
                    Insert.Parameters.Add(key);
                    Insert.ExecuteNonQuery();
                    ResidId = int.Parse(key.Value.ToString());

                    //Resid Detail
                    //در هنگام صدور فاکتور مرجوعی فروش اگر شماره فاکتور فروش مشخص بود ارزش کالا در حواله مربوطه خوانده شده
                    // وعینا در رسید مربو به فاکتور مرجوعی فروش درج میگردد
                    foreach (DataRow item in DraftChild.Rows)
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
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                            item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + ",0 ,0,0,0,NULL," +
                            (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                            + "',getdate(),0," + item["Column15"].ToString() + "," + item["Column16"].ToString() +
                            ",0,NULL,NULL," +
                            (item["Column23"].ToString().Trim() == "" ? "NULL" : item["Column23"].ToString()) + "," +
                            item["Column24"].ToString() + ",0,0,0," +
                            (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString() + "'") + "," +
                            (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString() + "'") + "," +
                            item["Column32"].ToString() + "," + item["Column33"].ToString() + "," + item["Column34"].ToString() + "," +
                            item["Column35"].ToString() + ")", conware);
                        InsertDetail.ExecuteNonQuery();
                    }
                }
                //درج شماره رسید در فاکتور مرجوعی
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", ReturnId, ResidId);
            }


        }

        private void InvertDoc(DataRowView Row)
        {
            Doc = true;
            ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
            ReturnDocNum.Direction = ParameterDirection.Output;

            if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
                return;

            DataTable PreDoc = clDoc.ReturnTable(ConAcnt, "Select * from Table_065_SanadDetail where Column00=" +
                Row["Column10"].ToString() + " and (Column16=15 and Column17=" + Row["ColumnId"].ToString() +
                    ") or (Column16=26 and Column17=" + Row["Column09"].ToString() + ")");

            if (PreDoc.Rows.Count > 0)
            {
                //Header
                //ReturnDocNum = clDoc.LastDocNum() + 1;
                //ReturnDocId = clDoc.ExportDoc_Header(ReturnDocNum, ReturnDate, "فاکتور مرجوعی", Class_BasicOperation._UserName);
                string CommandTxt = string.Empty;
                CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM  "+ConAcnt.Database+@".dbo.Table_060_SanadHead ), 0 )) + 1;
INSERT INTO " + ConAcnt.Database + @".dbo.Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00)  from " + ConAcnt.Database + @".dbo.Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @Key=SCOPE_IDENTITY();";

                //Detail
                foreach (DataRow item in PreDoc.Rows)
                {
                    string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());
                    // clDoc.ExportDoc_Detail(ReturnDocId, item["Column01"].ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                    // , (item["Column07"].ToString().Trim() == "" ? "NULL" : item["Column07"].ToString()), (item["Column08"].ToString().Trim() == "" ? "NULL" : item["Column08"].ToString()),
                    // (item["Column09"].ToString().Trim() == "" ? "NULL" : item["Column09"].ToString()), "مرجوعی-" + item["Column10"].ToString().Trim(),
                    //Convert.ToInt64(item["Column12"].ToString()),
                    //Convert.ToInt64(item["Column11"].ToString()),
                    //Convert.ToDouble(item["Column13"].ToString()),
                    //Convert.ToDouble(item["Column14"].ToString()),
                    //(item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1))
                    //, short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()), (item["Column16"].ToString() == "26" ? ResidId : ReturnId), Class_BasicOperation._UserName,
                    //Convert.ToDouble(item["Column22"].ToString()), (short?)null);

                    CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()) + @",
                                " + (item["Column16"].ToString() == "26" ? ResidId : ReturnId) + @",
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";

                }

                //درج شماره سند در فاکتور مرجوعی
                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", ReturnId, ReturnDocId);
                CommandTxt += " Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@Key where ColumnId=" + ReturnId;

                //درج شماره سند در رسید انبار
                //DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" + ReturnDocId + " and Column16=27");
                //if (Table.Rows.Count > 0)
                // clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidId, ReturnDocId);
                CommandTxt += @" IF (Select count(*) from Table_065_SanadDetail where Column00=@Key and Column16=27) >0 Begin  Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=@Key where ColumnId=" + ResidId + " END";

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();

                    SqlTransaction sqlTran = Con.BeginTransaction();
                    SqlCommand Command = Con.CreateCommand();
                   
                    Command.Parameters.Add(ReturnDocNum);

                    Command.Transaction = sqlTran;

                    try
                    {
                        Command.CommandText = CommandTxt;

                        Command.ExecuteNonQuery();
                        sqlTran.Commit();



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

        private void TurnBack(DataRowView Row)
        {
            if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", Row["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، فاکتور مرجوعی صادر شده است");

            ReturnDate = InputBox.Show("تاریخ ارجاع را وارد کنید:");
            if (!string.IsNullOrEmpty(ReturnDate))
            {

                //درج هدر مرجوعی
                ReturnNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column01");
                ReturnId = 0;
                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                {
                    Con.Open();
                    Key.Direction = ParameterDirection.Output;
                    SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_018_MarjooiSale  ([column01]
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
           ,[Column18]
           ,[Column19]
           ,[Column20]
           ,[Column21]
           ,[Column22]
           ,[Column23]
           ,[Column24]
           ) VALUES(" + ReturnNum + ",'" + ReturnDate + "'," + Row["Column03"].ToString() + "," +
                        (Row["Column04"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column04"].ToString().Trim() + "'") + "," +
                        (Row["Column05"].ToString().Trim() == "" ? "NULL" : Row["Column05"].ToString().Trim()) + ",'" + "ارجاع فاکتور فروش ش " + Row["Column01"].ToString() + " تاریخ " + Row["Column02"].ToString() + "'," +
                        (Row["Column07"].ToString().Trim() == "" ? "NULL" : Row["Column07"].ToString().Trim()) + ",0,0,0,0," +
                        (Row["Column12"].ToString() == "True" ? 1 : 0) + ",'" + Class_BasicOperation._UserName
                        + "',Getdate(),'" + Class_BasicOperation._UserName + "',Getdate()," + Row["ColumnId"].ToString() + "," + Row["Column28"].ToString() + "," +
                        Row["Column32"].ToString() + "," + Row["Column33"].ToString() + "," + Row["Column34"].ToString() + "," + Row["Column35"].ToString() +
                        "," + (Row["Column40"].ToString().Trim() == "" ? "NULL" : Row["Column40"].ToString()) + "," +
                         Row["Column41"].ToString() +
                        "); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(Key);
                    InsertHeader.ExecuteNonQuery();
                    ReturnId = int.Parse(Key.Value.ToString());

                    //درج دیتیل1
                    foreach (DataRowView item in this.table_011_Child1_SaleFactorBindingSource)
                    {
                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_019_Child1_MarjooiSale ([column01]
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
           ,[Column35]) VALUES(" + ReturnId + "," + item["Column02"].ToString() +
                            "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() +
                            "," + item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," +
                            item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL,NULL," +
                            (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                            item["Column15"].ToString() + "," + item["Column16"].ToString() + "," + item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + "," + item["Column20"].ToString() +
                            ",NULL," + (item["Column22"].ToString().Trim() != "" ? item["Column22"].ToString() : "NULL") + ",NULL," + (Row["Column07"].ToString().Trim() != "" ? Row["Column07"].ToString() : "0") + ",0,0,0,0,0," +
                            item["Column31"].ToString() + "," + item["Column32"].ToString() + "," +
                            (item["Column34"].ToString() == "" ? "NULL" : "'" + item["Column34"].ToString() + "'") + "," +
                            (item["Column35"].ToString() == "" ? "NULL" : "'" + item["Column35"].ToString() + "'") + "," + item["Column36"].ToString() + "," + item["Column37"].ToString() + ")", Con);
                        InsertDetail.ExecuteNonQuery();
                    }

                    //درج دیتیل 2
                    foreach (DataRowView item in this.table_012_Child2_SaleFactorBindingSource)
                    {
                        clDoc.RunSqlCommand(ConSale.ConnectionString, "INSERT INTO Table_020_Child2_MarjooiSale VALUES(" + ReturnId + "," + item["Column02"].ToString()
                            + "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + (item["Column05"].ToString() == "True" ? 1 : 0) + "," +
                            (item["Column06"].ToString().Trim() == "" ? "NULL" : item["Column06"].ToString()) + ")");

                    }
                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column19=1 , Column20=" + ReturnId + " Where ColumnId=" + Row["ColumnId"].ToString());
                }
            }
        }

      

//        private void InvertDoc(DataRowView Row)
//        {
//            if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
//                return;

//            DataTable PreDoc = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" +
//                Row["Column10"].ToString() + " and (Column16=15 and Column17=" + Row["ColumnId"].ToString() +
//                    ") or (Column16=26 and Column17=" + Row["Column09"].ToString() + ")");

//            if (PreDoc.Rows.Count > 0)
//            {
//                //Header
//                //ReturnDocNum = clDoc.LastDocNum() + 1;
//                //ReturnDocId = clDoc.ExportDoc_Header(ReturnDocNum, ReturnDate, "فاکتور مرجوعی", Class_BasicOperation._UserName);
//                string CommandTxt = string.Empty;
//                CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
//                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1  INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
//                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
//                       "',getdate()); SET @Key=SCOPE_IDENTITY()";

//                //Detail
//                foreach (DataRow item in PreDoc.Rows)
//                {
//                    string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());
//                    // clDoc.ExportDoc_Detail(ReturnDocId, item["Column01"].ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
//                    // , (item["Column07"].ToString().Trim() == "" ? "NULL" : item["Column07"].ToString()), (item["Column08"].ToString().Trim() == "" ? "NULL" : item["Column08"].ToString()),
//                    // (item["Column09"].ToString().Trim() == "" ? "NULL" : item["Column09"].ToString()), "مرجوعی-" + item["Column10"].ToString().Trim(),
//                    //Convert.ToInt64(item["Column12"].ToString()),
//                    //Convert.ToInt64(item["Column11"].ToString()),
//                    //Convert.ToDouble(item["Column13"].ToString()),
//                    //Convert.ToDouble(item["Column14"].ToString()),
//                    //(item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1))
//                    //, short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()), (item["Column16"].ToString() == "26" ? ResidId : ReturnId), Class_BasicOperation._UserName,
//                    //Convert.ToDouble(item["Column22"].ToString()), (short?)null);

//                    CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
//              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
//              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
//              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
//                VALUES (@Key,'" + item["Column01"].ToString() + @"',
//                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
//                                '" + _AccInfo[1].ToString() + @"',
//                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
//                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
//                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
//                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
//                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
//                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
//                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
//                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
//                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
//                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
//                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
//                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
//                                " + short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()) + @",
//                                " + (item["Column16"].ToString() == "26" ? ResidId : ReturnId) + @",
//                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
//                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
//                                NULL)";

//                }

//                //درج شماره سند در فاکتور مرجوعی
//                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", ReturnId, ReturnDocId);
//                CommandTxt += " Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@Key where ColumnId=" + ReturnId;

//                //درج شماره سند در رسید انبار
//                //DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" + ReturnDocId + " and Column16=27");
//                //if (Table.Rows.Count > 0)
//                // clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidId, ReturnDocId);
//                CommandTxt += @" IF (Select count(*) from Table_065_SanadDetail where Column00=@Key and Column16=27) >0 Begin  Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=@Key where ColumnId=" + ResidId + " END";

//                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
//                {
//                    Con.Open();

//                    SqlTransaction sqlTran = Con.BeginTransaction();
//                    SqlCommand Command = Con.CreateCommand();
//                    Command.Parameters.Add(ReturnDocNum);

//                    Command.Transaction = sqlTran;

//                    try
//                    {
//                        Command.CommandText = CommandTxt;

//                        Command.ExecuteNonQuery();
//                        sqlTran.Commit();



//                    }
//                    catch (Exception es)
//                    {
//                        sqlTran.Rollback();
//                        this.Cursor = Cursors.Default;
//                        Class_BasicOperation.CheckExceptionType(es, this.Name);
//                    }

//                    this.Cursor = Cursors.Default;



//                }

//            }


//        }

        private void Frm_002_Faktor_KeyDown(object sender, KeyEventArgs e)
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

        private void bt_Print_Click(object sender, EventArgs e)
        {
            //if (this.table_010_SaleFactorBindingSource.Count > 0)
            //{
            //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
            //    {
            //        _002_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
            //                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
            //        frm.ShowDialog();
            //    }
            //    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            //}
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
                    string user = clDoc.ExScalar(ConSale.ConnectionString, @"select column13 from Table_010_SaleFactor where column01 =" + txt_Search.Text + " ");
                  
                    dataSet_01_Sale.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    if (isadmin)
                    {
                        this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, RowID);
                        if (table_010_SaleFactorBindingSource.Count>0)
                        {
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
                        }
                        else
                        {
                            MessageBox.Show("این شماره فاکتور وجود ندارد");
                        }
                        
                    }
                    else if (user==Class_BasicOperation._UserName)
                    {
                        this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, RowID);
                        if (table_010_SaleFactorBindingSource.Count>0)
                        {
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
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
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

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
               


                    txt_Search.Text = clDoc.ExScalar(ConSale.ConnectionString, @"select isNull ((select column01 from Table_010_SaleFactor where columnid =" + _ID + "),0) ");


                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    string user = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull((select column13 from Table_010_SaleFactor where column01 =" + txt_Search.Text + "),0) ");

                    dataSet_01_Sale.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    if (isadmin)
                    {
                        this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, RowID);
                        if (table_010_SaleFactorBindingSource.Count > 0)
                        {
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
                        }
                        else
                        {
                            MessageBox.Show("این شماره فاکتور وجود ندارد");
                        }

                    }
                    else if (user == Class_BasicOperation._UserName)
                    {
                        this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, RowID);
                        if (table_010_SaleFactorBindingSource.Count > 0)
                        {
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
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
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

               
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                txt_Search.SelectAll();
            }
             
          
        }

        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_010_SaleFactor where column01=" + FactorNum, con);
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
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 76))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_003_ViewFactorSale")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _002_Sale.Frm_003_ViewFactorSale frm = new Frm_003_ViewFactorSale();
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

        //private void mnu_Documents_Click(object sender, EventArgs e)
        //{
        //    int SanadId = 0;
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //        SanadId = (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString()));

        //    PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
        //    PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
        //    PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
        //    PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;


        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
        //    {
        //        foreach (Form item in Application.OpenForms)
        //        {
        //            if (item.Name == "Form01_AccDocument")
        //            {
        //                item.BringToFront();
        //                TextBox txt_S = (TextBox)item.ActiveControl;
        //                txt_S.Text = SanadId.ToString();
        //                PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
        //                frms.bt_Search_Click(sender, e);
        //                return;
        //            }
        //        }
        //        PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
        //          UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(SanadId.ToString()));
        //        try
        //        {
        //            frm.MdiParent = MainForm.ActiveForm;
        //        }
        //        catch
        //        {
        //        }
        //        frm.Show();
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //}

        //private void mnu_Drafts_Click(object sender, EventArgs e)
        //{
        //    PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
        //    PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
        //    PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
        //    PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
        //    PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
        //    PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

        //    if (gridEX1.GetRow().Cells["Column09"].Text.Trim() == "0" || gridEX1.GetRow().Cells["Column09"].Text.Trim() == "")
        //    {
        //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 26))
        //        {
        //            foreach (Form item in Application.OpenForms)
        //            {
        //                if (item.Name == "Form07_ViewDrafts")
        //                {
        //                    item.BringToFront();
        //                    return;
        //                }
        //            }
        //            PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts frm = new PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts();
        //            try
        //            {
        //                frm.MdiParent = MainForm.ActiveForm;
        //            }
        //            catch { }
        //            frm.Show();
        //        }
        //        else
        //            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //    }
        //    else
        //    {
        //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
        //        {
        //            foreach (Form item in Application.OpenForms)
        //            {
        //                if (item.Name == "Form06_RegisterDrafts")
        //                {
        //                    item.BringToFront();
        //                    ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).txt_Search.Text = gridEX1.GetRow().Cells["Column09"].Text;
        //                    ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).bt_Search_Click(sender, e);
        //                    return;
        //                }
        //            }
        //            PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts(
        //                UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
        //                int.Parse(gridEX1.GetValue("Column09").ToString()));
        //            frm.ShowDialog();
        //            int SaleId = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
        //            dataSet_01_Sale.EnforceConstraints = false;
        //            this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, SaleId);
        //            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, SaleId);
        //            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, SaleId);
        //            dataSet_01_Sale.EnforceConstraints = true;
        //        }
        //        else
        //            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //    }
        //}

        //private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
        //    {
        //        _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
        //        ob.ShowDialog();
        //        SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
        //        DS.Tables["Discount"].Rows.Clear();
        //        Adapter.Fill(DS, "Discount");
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //}

        //private void mnu_GoodInformation_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
        //    {
        //        _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob = new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));
        //        ob.ShowDialog();
        //        GoodbindingSource.DataSource = clGood.GoodInfo();
        //        DataTable Table = clGood.GoodInfo();
        //        gridEX_List.DropDowns["GoodCode"].SetDataBinding(Table, "");
        //        gridEX_List.DropDowns["GoodName"].SetDataBinding(Table, "");
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //}

//        private void mnu_Customers_Click(object sender, EventArgs e)
//        {
//            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
//            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
//            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
//            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
//            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
//            {
//                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
//                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
//                frm.ShowDialog();

//                DataTable CustomerTable = clDoc.ReturnTable
//            (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
//                                           dbo.Table_045_PersonInfo.Column01 AS code,
//                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
//                                           dbo.Table_065_CityInfo.Column02 AS shahr,
//                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
//                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
//                                           dbo.Table_045_PersonInfo.Column30,
//                                           Table_045_PersonInfo.Column07,
//                                           Table_045_PersonInfo.Column19 AS Mobile
//                                    FROM   dbo.Table_045_PersonInfo
//                                           LEFT JOIN dbo.Table_065_CityInfo
//                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
//                                           LEFT JOIN dbo.Table_060_ProvinceInfo
//                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
//                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
//                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
//                gridEX1.DropDowns["Tel"].SetDataBinding(CustomerTable, "");


//                DataTable Seller = clDoc.ReturnTable
//            (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId,
//                                           dbo.Table_045_PersonInfo.Column01,
//                                           dbo.Table_045_PersonInfo.Column02,
//                                           dbo.Table_065_CityInfo.Column02 AS shahr,
//                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
//                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
//                                           dbo.Table_045_PersonInfo.Column30,
//                                           Table_045_PersonInfo.Column07,
//                                           Table_045_PersonInfo.Column19 AS Mobile
//                                    FROM   dbo.Table_045_PersonInfo
//                                           LEFT JOIN dbo.Table_065_CityInfo
//                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
//                                           LEFT JOIN dbo.Table_060_ProvinceInfo
//                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
//                                           JOIN Table_045_PersonScope tps
//                                                ON  tps.Column01 = Table_045_PersonInfo.ColumnId
//                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)
//                                           AND tps.Column02 = 3");
//                gridEX1.DropDowns["Seller"].SetDataBinding(Seller, "");


//            }
//            else
//                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

//        }

        private void Frm_002_Faktor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.table_010_SaleFactorBindingSource.CurrencyManager.Position > -1)
            //{
            //    //{
            //    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
            //    try
            //    {
            //        Properties.Settings.Default.Function = gridEX1.GetValue("Column43").ToString();
            //        Properties.Settings.Default.Project = gridEX1.GetValue("Column44").ToString();
            //        Properties.Settings.Default.Ware = gridEX1.GetValue("Column42").ToString();
            //        Properties.Settings.Default.Masool = gridEX1.GetValue("column05").ToString();
            //        Properties.Settings.Default.SaleType = gridEX1.GetValue("Column36").ToString();
            //        Properties.Settings.Default.Customer = gridEX1.GetValue("column03").ToString();
            //        Properties.Settings.Default.Save();
            //    }
            //    catch
            //    {
            //    }

            //}

            //if (chk_Award_Box.Checked)
            //{
            //    Properties.Settings.Default.AwardCompute = "Box";
            //    Properties.Settings.Default.Save();
            //}
            //else
            //{
            //    Properties.Settings.Default.AwardCompute = "Detail";
            //    Properties.Settings.Default.Save();
            //}
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
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            //try
            //{
            //    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)[
            //        "Column09"].ToString() != "0" &&
            //        ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)[
            //        "Column10"].ToString() == "0")
            //    {
            //        if (e.Column.Key == "column08" || e.Column.Key == "column09" || 
            //            e.Column.Key == "column10" || e.Column.Key == "column11" ||
            //            e.Column.Key == "column16" || e.Column.Key == "column18")
            //            e.Cancel = false;
            //        else
            //            e.Cancel = true;
            //    }
            //    else
            //    {
            //        if (gridEX_List.GetRow().Cells["column30"].Value.ToString() == "True")
            //            if (e.Column.Key != "column02" && e.Column.Key != "GoodCode")
            //                e.Cancel = false;
            //            else
            //                e.Cancel = true;
            //    }

            //}
            //catch
            //{
            //}
        }

        private void gridEX_List_FormattingRow(object sender, RowLoadEventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record &&
                        e.Row.Cells["column30"].Value.ToString() == "True")
                        e.Row.RowHeaderImageIndex = 0;
                }
                catch { }
            }
        }

     

       

        

        private void gridEX_List_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            if (e.Row.Cells["column30"].Value.ToString() == "True")
            {
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف کالا و ذخیره تغییرات هستید؟",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    e.Row.Delete();
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);
                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);
                }
                else
                    e.Cancel = true;
            }

        }

    

       

        //private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        //{
        //    if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_010_SaleFactorBindingSource.Count > 0 && this.table_011_Child1_SaleFactorBindingSource.Count > 0)
        //    {
        //        try
        //        {
        //            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
        //            foreach (DataRow item in Table.Rows)
        //            {
        //                this.table_012_Child2_SaleFactorBindingSource.AddNew();
        //                DataRowView New = (DataRowView)this.table_012_Child2_SaleFactorBindingSource.CurrencyManager.Current;
        //                New["Column02"] = item["ColumnId"].ToString();
        //                if (item["Column03"].ToString() == "True")
        //                {
        //                    New["Column03"] = 0;
        //                    New["Column04"] = item["Column04"].ToString();
        //                }
        //                else
        //                {
        //                    New["Column03"] = item["Column04"].ToString();
        //                    New["Column04"] = double.Parse(item["Column04"].ToString()) *
        //                        double.Parse(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString()) / 100;
        //                }
        //                New["Column05"] = item["Column02"].ToString();
        //                this.table_012_Child2_SaleFactorBindingSource.EndEdit();
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
        //        }
        //    }
        //}

        private void bt_SettleFactor_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 114))
                {
                    try
                    {
                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID) == 0)
                            throw new Exception("جهت تسویه فاکتور، صدور سند حسابداری الزامیست");

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("به علت ارجاع این فاکتور تسویه آن امکانپذیر نمی باشد");

                        if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                            throw new Exception("به علت ابطال این فاکتور تسویه آن امکانپذیر نمی باشد");

                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    }
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان تسویه فاکتور را ندارید", "None");
            }
        }

      

        //private void mnu_ViewWareStock_Click(object sender, EventArgs e)
        //{
        //    PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
        //    PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
        //    PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
        //    PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 31))
        //    {
        //        PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi ob = new PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi();
        //        try
        //        {
        //            ob.MdiParent = MainForm.ActiveForm;
        //        }
        //        catch { }
        //        ob.Show();

        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //}

//        private void toolStripMenuItem1_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (MessageBox.Show("آیا از کپی کردن این فاکتور مطمئن هستید؟",
//                    "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes &&
//                    gridEX_List.RowCount > 0)
//                {
//                    Save_Event(this, e);

//                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
//                    {
//                        MessageBox.Show("خطا در ثبت اطلاعات");
//                        return;
//                    }


//                    //درج هدر فاکتور فروش

//                    SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
//                    Key.Direction = ParameterDirection.Output;
//                    int FactorNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "column01");
//                    string _CopyCmd = @"INSERT INTO Table_010_SaleFactor ([column01]
//                                                                       ,[column02]
//                                                                       ,[column03]
//                                                                       ,[column04]
//                                                                       ,[column05]
//                                                                       ,[column06]
//                                                                       ,[column07]
//                                                                       ,[column08]
//                                                                       ,[column09]
//                                                                       ,[column10]
//                                                                       ,[column11]
//                                                                       ,[column12]
//                                                                       ,[column13]
//                                                                       ,[column14]
//                                                                       ,[column15]
//                                                                       ,[column16]
//                                                                       ,[column17]
//                                                                       ,[column18]
//                                                                       ,[column19]
//                                                                       ,[column20]
//                                                                       ,[column21]
//                                                                       ,[column22]
//                                                                       ,[column23]
//                                                                       ,[column24]
//                                                                       ,[column25]
//                                                                       ,[column26]
//                                                                       ,[column27]
//                                                                       ,[Column28]
//                                                                       ,[Column29]
//                                                                       ,[Column30]
//                                                                       ,[Column31]
//                                                                       ,[Column32]
//                                                                       ,[Column33]
//                                                                       ,[Column34]
//                                                                       ,[Column35]
//                                                                       ,[Column36]
//                                                                       ,[Column37]
//                                                                       ,[Column38]
//                                                                       ,[Column39]
//                                                                       ,[Column40]
//                                                                       ,[Column41]
//                                                                       ,[Column42]
//                                                                       ,[Column43]) VALUES(" +
//                               FactorNum.ToString() + ", '" + gridEX1.GetValue("column02").ToString() + "', " +
//                               gridEX1.GetValue("column03").ToString() + ", " +
//                               (gridEX1.GetRow().Cells["Column04"].Text.Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column04").ToString() + "'") + "," +
//                               (gridEX1.GetRow().Cells["Column05"].Text.Trim() == "" ? "Null" : gridEX1.GetValue("column05").ToString()) + "," +
//                               (gridEX1.GetRow().Cells["Column06"].Text.Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column06").ToString() + "'")
//                               + ", 0, 0, 0, 0, 0, " +
//                               (gridEX1.GetValue("Column12").ToString() == "True" ? 1 : 0) + ", '" +
//                               Class_BasicOperation._UserName + "', getdate(), '" +
//                               Class_BasicOperation._UserName + "', getdate(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, " +
//                               txt_TotalPrice.Value.ToString() + ",0,0,0," +
//                               txt_Extra.Value.ToString() + "," +
//                               txt_Reductions.Value.ToString() + "," +
//                               gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column19"],
//                               AggregateFunction.Sum).ToString() + "," +
//                               gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column17"],
//                               AggregateFunction.Sum).ToString() + "," +
//                               (gridEX1.GetValue("column36").ToString().Trim() == "" ? "Null" : gridEX1.GetValue("Column36").ToString()) +
//                               ",null,0,null," +
//                               (gridEX1.GetRow().Cells["Column40"].Text.Trim() == "" ? "NULL" : gridEX1.GetValue("Column40").ToString()) + "," +
//                               gridEX1.GetValue("Column41").ToString() + "," + gridEX1.GetValue("Column42").ToString() + "," + gridEX1.GetValue("Column43").ToString() + " ); SET @Key=SCOPE_IDENTITY()";
//                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
//                    {
//                        Con.Open();
//                        SqlCommand InsertHeader = new SqlCommand(_CopyCmd, Con);

//                        InsertHeader.Parameters.Add(Key);
//                        InsertHeader.ExecuteNonQuery();
//                        int FactorId = int.Parse(Key.Value.ToString());

//                        //درج کالاهای فاکتور فروش
//                        for (int ij = 0; ij < gridEX_List.RowCount; ij++)
//                        {
//                            gridEX_List.Row = ij;

//                            _CopyCmd = @"INSERT INTO Table_011_Child1_SaleFactor ([column01]
//           ,[column02]
//           ,[column03]
//           ,[column04]
//           ,[column05]
//           ,[column06]
//           ,[column07]
//           ,[column08]
//           ,[column09]
//           ,[column10]
//           ,[column11]
//           ,[column12]
//           ,[column13]
//           ,[column14]
//           ,[column15]
//           ,[column16]
//           ,[column17]
//           ,[column18]
//           ,[column19]
//           ,[column20]
//           ,[column21]
//           ,[column22]
//           ,[column23]
//           ,[column24]
//           ,[column25]
//           ,[column26]
//           ,[column27]
//           ,[column28]
//           ,[column29]
//           ,[column30]
//           ,[Column31]
//           ,[Column32]
//           ,[Column33]
//           ,[Column34]
//           ,[Column35]
//           ,[Column36]
//           ,[Column37]) VALUES(" +
//                            FactorId.ToString() + ", " +
//                            gridEX_List.GetValue("column02").ToString() + ", " +
//                            gridEX_List.GetValue("column03").ToString() + ", " +
//                            gridEX_List.GetValue("column04").ToString() + ", " +
//                            gridEX_List.GetValue("column05").ToString() + ", " +
//                            gridEX_List.GetValue("column06").ToString() + ", " +
//                            gridEX_List.GetValue("column07").ToString() + ", " +
//                            gridEX_List.GetValue("column08").ToString() + ", " +
//                            gridEX_List.GetValue("column09").ToString() + ", " +
//                            gridEX_List.GetValue("column10").ToString() + ", " +
//                            gridEX_List.GetValue("column11").ToString() + ", 0, 0, " +
//                            (gridEX_List.GetValue("Column14").ToString().Trim() == "" ? "Null" : gridEX_List.GetValue("Column14").ToString()) +
//                            "," + (gridEX_List.GetValue("Column15").ToString().Trim() == "" ? "0" : gridEX_List.GetValue("Column15").ToString()) + "," +
//                            gridEX_List.GetValue("column16").ToString() + ", " +
//                            gridEX_List.GetValue("column17").ToString() + ", " +
//                            gridEX_List.GetValue("column18").ToString() + ", " +
//                            gridEX_List.GetValue("column19").ToString() + ", " +
//                            gridEX_List.GetValue("column20").ToString() + ", NULL, NUll, '" +
//                            gridEX_List.GetValue("column23").ToString() + "', 0, 0, 0, 0, 0, 0, '" +
//                            gridEX_List.GetValue("column30").ToString() + "'," +
//                            gridEX_List.GetValue("Tedaddarkartoon").ToString() + "," +
//                            gridEX_List.GetValue("Tedaddarbaste").ToString() + "," +
//                            gridEX_List.GetValue("Column33").ToString() + "," +
//                            (gridEX_List.GetRow().Cells["Column34"].Text.Trim() == "" ? "NULL" : "'" + gridEX_List.GetValue("Column34").ToString() + "'") + "," +
//                            (gridEX_List.GetRow().Cells["Column35"].Text.Trim() == "" ? "NULL" : "'" + gridEX_List.GetValue("Column35").ToString() + "'")
//                            + "," + gridEX_List.GetValue("Column36").ToString() + "," + gridEX_List.GetValue("Column37").ToString() + ")";

//                            SqlCommand InsertChild1 = new SqlCommand(_CopyCmd, Con);

//                            InsertChild1.ExecuteNonQuery();
//                        }

//                        gridEX_List.MoveFirst();


//                        //درج اضافات و کسورات فاکتور فروش
//                        for (int ik = 0; ik < gridEX_Extra.RowCount; ik++)
//                        {
//                            gridEX_Extra.Row = ik;

//                            _CopyCmd = @"INSERT INTO Table_012_Child2_SaleFactor ([column01]
//                                                                           ,[column02]
//                                                                           ,[column03]
//                                                                           ,[column04]
//                                                                           ,[column05]
//                                                                           ,[column06]) VALUES(" +
//                            FactorId.ToString() + ", " +
//                            gridEX_Extra.GetValue("column02").ToString() + ", " +
//                            gridEX_Extra.GetValue("column03").ToString() + ", " +
//                            gridEX_Extra.GetValue("column04").ToString() + ", '" +
//                            gridEX_Extra.GetValue("column05").ToString() + "', '" +
//                            gridEX_Extra.GetValue("column06").ToString() + "')";

//                            SqlCommand InsertChild2 = new SqlCommand(_CopyCmd, Con);

//                            InsertChild2.ExecuteNonQuery();
//                        }

//                        gridEX_Extra.MoveFirst();


//                        MessageBox.Show("فاکتور شماره " + FactorNum.ToString() + " صادر شد");
//                    }
//                }
//            }
//            catch
//            {
//            }

//        }

        //private void chk_Award_Detial_Click(object sender, EventArgs e)
        //{
        //    if (chk_Award_Detial.Checked)
        //    {
        //        chk_Award_Detial.Checked = false;
        //        chk_Award_Box.Checked = true;
        //    }
        //    else
        //    {
        //        chk_Award_Detial.Checked = true;
        //        chk_Award_Box.Checked = false;
        //    }
        //}

        //private void chk_Award_Box_Click(object sender, EventArgs e)
        //{
        //    if (chk_Award_Box.Checked)
        //    {
        //        chk_Award_Detial.Checked = true;
        //        chk_Award_Box.Checked = false;
        //    }
        //    else
        //    {
        //        chk_Award_Detial.Checked = false;
        //        chk_Award_Box.Checked = true;
        //    }
        //}

        //private void bt_DefineSignatures_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
        //    {
        //        _05_Sale.Frm_019_Sale_Signatures frm = new Frm_019_Sale_Signatures();
        //        frm.ShowDialog();
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //}

        //private void mnu_DefineCurrency_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 7))
        //    {
        //        PACNT.Class_ChangeConnectionString.CurrentConnection = Class_ChangeConnectionString.CurrentConnection;
        //        PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
        //        PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
        //        PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
        //        PACNT._1_BasicMenu.Form04_Currency frm = new PACNT._1_BasicMenu.Form04_Currency(
        //            UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 8));
        //        frm.ShowDialog();
        //        SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_055_CurrencyInfo", ConBase);
        //        DataTable TCurrency = new DataTable();
        //        Adapter.Fill(TCurrency);
        //        gridEX1.DropDowns["Currency"].SetDataBinding(TCurrency, "");
        //        gridEX_List.DropDowns["Currency"].SetDataBinding(TCurrency, "");
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //}

        //private void mnu_DefaultDescription_Click(object sender, EventArgs e)
        //{
        //    Frm_025_SaleDefaultDescription frm = new Frm_025_SaleDefaultDescription();
        //    frm.ShowDialog();
        //}

        //private void mnu_DelDraft_Click(object sender, EventArgs e)
        //{
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //    {
        //        try
        //        {
        //            int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
        //            int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID.ToString());

        //            if (DraftId != 0)
        //            {
        //                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
        //                {

        //                    PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
        //                    PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
        //                    PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
        //                    PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
        //                    PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
        //                    PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

        //                    PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts
        //                    (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25), -1);
        //                    frm.txt_Search.Text = clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01", "ColumnId", DraftId.ToString());
        //                    frm.bt_Search_Click(sender, e);
        //                    frm.bt_Del_Click(sender, e);

        //                }
        //                else
        //                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //            }
        //            DS.Tables["Draft"].Clear();
        //            DraftAdapter.Fill(DS, "Draft");
        //            dataSet_01_Sale.EnforceConstraints = false;
        //            this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, RowID);
        //            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
        //            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
        //            dataSet_01_Sale.EnforceConstraints = true;
        //            txt_Search.SelectAll();
           

        //        }
        //        catch (Exception ex)
        //        {
        //            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
        //        }
        //    }

        //}

//        private void gridEX_List_ColumnButtonClick(object sender, ColumnActionEventArgs e)
//        {
//            try
//            {

//                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 169))
//                {



//                    if (gridEX_List.GetValue("Column02").ToString() != "")
//                    {
//                        string Txt = "";
//                        DataTable Table = clDoc.GoodRemain(gridEX_List.GetValue("Column02").ToString(), gridEX1.GetValue("Column02").ToString());
//                        foreach (DataRow item in Table.Rows)
//                        {
//                            Txt += " انبار:" + item["WareName"].ToString() + " " + Convert.ToDouble(item["Remain"].ToString()).ToString("#,##0.###")
//                                + Environment.NewLine;
//                        }
//                        Txt += " آخرین قیمت خرید:" + LastBuyGoodPrice(int.Parse(gridEX_List.GetValue("Column02").ToString())).ToString("#,##0.###");

//                        try
//                        {
//                            DataTable SaleTable = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     TOP (1) PERCENT dbo.Table_010_SaleFactor.column03 AS CustomerId, dbo.Table_010_SaleFactor.column02 AS Date, 
//                        dbo.Table_011_Child1_SaleFactor.column02 AS GoodID, dbo.Table_010_SaleFactor.column01 AS FactorNumber, 
//                        dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
//                        dbo.Table_011_Child1_SaleFactor.column10 AS JozPrice
//                        FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
//                        dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
//                        WHERE     (dbo.Table_010_SaleFactor.column03 = " + gridEX1.GetValue("Column03") + @") AND (dbo.Table_011_Child1_SaleFactor.column02 = " + gridEX_List.GetValue("Column02") + @")
//                        ORDER BY Date DESC, FactorNumber DESC");

//                            Txt += Environment.NewLine + "اطلاعات آخرین فروش این کالا به مشتری مشخص شده:" + Environment.NewLine +
//                            "شماره فاکتور: " + SaleTable.Rows[0]["FactorNumber"].ToString() + "-- تاریخ: " + SaleTable.Rows[0]["Date"].ToString() + " -- قیمت کارتن:" +
//                            Convert.ToInt64(Convert.ToDouble(SaleTable.Rows[0]["BoxPrice"].ToString())).ToString("n0") + " -- قیمت بسته: " +
//                            Convert.ToInt64(Convert.ToDouble(SaleTable.Rows[0]["PackPrice"].ToString())).ToString("n0") + " -- قیمت جز: " +
//                            Convert.ToInt64(Convert.ToDouble(SaleTable.Rows[0]["JozPrice"].ToString())).ToString("n0");

//                        }
//                        catch
//                        {
//                        }

//                        if (Txt.Trim() != "")
//                            ToastNotification.Show(this, Txt, 3000, eToastPosition.MiddleCenter);
//                    }
//                }
//                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان مشاهده اطلاعات را ندارید", "Warning");

//            }
//            catch
//            {

//            }
//        }

//        private Decimal LastBuyGoodPrice(int GoodCode)
//        {
//            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
//            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_015_BuyFactor.column02) AS Date,1
//            FROM         Table_016_Child1_BuyFactor INNER JOIN
//            Table_015_BuyFactor ON Table_016_Child1_BuyFactor.column01 = Table_015_BuyFactor.columnid
//            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
//            GROUP BY Table_016_Child1_BuyFactor.column02
//            order by Table_016_Child1_BuyFactor.column02;
//            
//            declare @t2 table(codekala2 int, gheymat2 int,date2 nvarchar(50)
//            ,UNIQUE (codekala2,gheymat2,date2)
//            );
//
//            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
//            dbo.Table_015_BuyFactor.column02 AS Date
//            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
//            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
//            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
//            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
//            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
//            select * from @t");

//            if (Table.Rows.Count == 0)
//                return 0;
//            else
//                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

//        }

        private void bt_NotConfirmDraft_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                int DraftId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                if (DraftId == 0)
                    return;
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 69))
                {
                    string Message = null;

                    if (clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column26", "ColumnId", DraftId.ToString()) == "True")
                    {
                        Message = "آیا مایل به غیر قطعی کردن حواله انبار هستید؟";
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column26=0 where ColumnId=" +
                              DraftId);
                            Class_BasicOperation.ShowMsg("", "غیرقطعی کردن حواله انبار با موفقیت انجام گرفت", "Information");
                        }

                    }
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        //private void gridEX_List_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        //{
        //    try
        //    {
        //        Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
        //        Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

        //    }
        //    catch { }
        //}

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                        if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");

                        }

                    }
                }

                if (isadmin)
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_010_SaleFactor),0) as Row");
                    
                }
                else
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_010_SaleFactor where Column13='"+Class_BasicOperation._UserName+"'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        //private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        //{
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //    {
        //        try
        //        {
        //            gridEX1.UpdateData();
        //            table_010_SaleFactorBindingSource.EndEdit();
        //            this.table_011_Child1_SaleFactorBindingSource.EndEdit();
        //            this.table_012_Child2_SaleFactorBindingSource.EndEdit();

        //            if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
        //                dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
        //            {
        //                if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
        //                {
        //                    Save_Event(sender, e);
        //                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
        //                    {
        //                        throw new Exception("خطا در ثبت اطلاعات");

        //                    }

        //                }
        //            }


        //            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
        //                "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column01<" +
        //                ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
        //            if (Table.Rows[0]["Row"].ToString() != "0")
        //            {
        //                DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
        //                dataSet_01_Sale.EnforceConstraints = false;
        //                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //                dataSet_01_Sale.EnforceConstraints = true;
        //                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
        //        }
        //    }
        //}

        //private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        //{
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //    {

        //        try
        //        {
        //            gridEX1.UpdateData();
        //            table_010_SaleFactorBindingSource.EndEdit();
        //            this.table_011_Child1_SaleFactorBindingSource.EndEdit();
        //            this.table_012_Child2_SaleFactorBindingSource.EndEdit();

        //            if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
        //                dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
        //            {
        //                if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
        //                {
        //                    Save_Event(sender, e);
        //                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
        //                    {
        //                        throw new Exception("خطا در ثبت اطلاعات");

        //                    }

        //                }
        //            }

        //            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_010_SaleFactor where Column01>" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
        //            if (Table.Rows[0]["Row"].ToString() != "0")
        //            {
        //                DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
        //                dataSet_01_Sale.EnforceConstraints = false;
        //                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //                dataSet_01_Sale.EnforceConstraints = true;
        //                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
        //        }
        //    }
        //}

        //private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gridEX1.UpdateData();
        //        table_010_SaleFactorBindingSource.EndEdit();
        //        this.table_011_Child1_SaleFactorBindingSource.EndEdit();
        //        this.table_012_Child2_SaleFactorBindingSource.EndEdit();

        //        if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
        //            dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
        //        {
        //            if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
        //            {
        //                Save_Event(sender, e);
        //                if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
        //                {
        //                    throw new Exception("خطا در ثبت اطلاعات");

        //                }

        //            }
        //        }

        //        DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_010_SaleFactor),0) as Row");
        //        if (Table.Rows[0]["Row"].ToString() != "0")
        //        {
        //            DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
        //            dataSet_01_Sale.EnforceConstraints = false;
        //            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
        //            dataSet_01_Sale.EnforceConstraints = true;
        //            this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

        //        }

        //    }
        //    catch
        //    {
        //    }
        //}

        private void SendSMEM()
        {

        }

      

       

        //private void چاپ8سانتیToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //    {
        //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
        //        {
        //            List<string> List = new List<string>();
        //            List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
        //            _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
        //                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 23);
        //            frm.Form_FactorPrint_Load(sender, e);

        //        }
        //        else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
        //    }
        //}

        private void bt_Print_Click_1(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.table_010_SaleFactorBindingSource.Count > 0)
            //    {
            //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
            //        {
            //            _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
            //                    int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
            //            frm.ShowDialog();
            //        }
            //        else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            //    }
            //}
            //catch { }
        }

        //private void ForiFctor_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this.table_010_SaleFactorBindingSource.Count > 0)
        //        {
        //            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
        //            {
        //                Save_Event(sender, e);


        //                List<string> List = new List<string>();
        //                //List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
        //                _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
        //                    int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
        //                frm.Form_FactorPrint_Load(sender, e);

        //            }
        //            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
        //        }
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            List<string> List = new List<string>();
        //            _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
        //                    int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
        //            frm.Form_FactorPrint_Load(sender, e);

        //        }
        //        catch (Exception ex1)
        //        {
        //            MessageBox.Show(ex1.Message);
        //        }
        //    }
        //}

        private void rasmiFactor_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
                        PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                        PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                        PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;

                        PSALE._05_Sale.Reports.Form_SaleFactorTotalPrint frm = new PSALE._05_Sale.Reports.Form_SaleFactorTotalPrint(
                                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch { }
           
        }

        private void FinalSave_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 167))
            {
                try
                {
                   
                        Int64 barcode = 0;
                        errorelist = string.Empty;

                        checkbarcode();

                        foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_List.GetRows())
                        {
                            if (PWHRSbool)
                            {
                                MessageBox.Show("موجودی کالا های زیر در انبار موردنظر کافی نیست" + Environment.NewLine + errorelist.TrimEnd(','));
                                return;
                            }

                        }
                        Save_Event(sender, e);
                        if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");

                        }

                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        //using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                        //{
                        //    Con.Open();
                        //    SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL( Column45,0) from Table_010_SaleFactor where ColumnId=" + RowID + "),0)", Con);
                        //    if (Convert.ToBoolean(Comm.ExecuteScalar()) == true)

                        //        throw new Exception("این فاکتور  تسویه شده است");
                        //}
                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID) != 0)
                            throw new Exception("برای این فاکتور قبلا سند زده  شده است");
                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                            throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");
                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 71))
                            throw new Exception("کاربر گرامی شما امکان صدور حواله انبار را ندارید");

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID) != 0)
                            throw new Exception("برای این فاکتور حواله صادر شده است");

                        if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                            throw new Exception("به علت باطل شدن این فاکتور امکان صدور حواله وجود ندارد");

                        if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("به علت مرجوع شدن این فاکتور امکان صدور حواله انبار وجود ندارد");

                        if (clDoc.AllService(table_011_Child1_SaleFactorBindingSource))
                            throw new Exception("به علت عدم وجود کالا، حواله ای برای این فاکتور صادر نخواهد شد");

                        Frm_030_SaleSanadInfo frm = new Frm_030_SaleSanadInfo(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current));

                        frm.ShowDialog();
                        using (SqlConnection Consale = new SqlConnection(Properties.Settings.Default.PSALE))
                        {
                            Consale.Open();
                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_010_SaleFactor SET  Column15='" + Class_BasicOperation._UserName
                                    + "', Column16=getdate() where ColumnId=" + int.Parse(RowID), Consale);
                            UpdateCommand.ExecuteNonQuery();
                            Consale.Close();
                        }

                        mlt_Draft.DataSource = clDoc.ReturnTable(ConWare, @"select Columnid,Column01 from Table_007_PwhrsDraft");
                        mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(
                        this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                    }
               
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
        }

        //private void txt_GoodCode_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Add)
        //    {
        //        txt_Count.Text = (Convert.ToInt32(txt_Count.Text) + 1).ToString();
        //    }
        //    else if (e.KeyCode == Keys.Subtract)
        //    {
        //        if ((Convert.ToInt32(txt_Count.Text) - 1) > 0)

        //            txt_Count.Text = (Convert.ToInt32(txt_Count.Text) - 1).ToString();

        //    }
        //}


        //private void addnew()
        //{
        //    txt_Count.Text = "1";
        //    ch_IsGift.Checked = false;
        //    txt_GoodCode.Text = string.Empty;
        //    txt_GoodCode.Focus();
        //    txt_GoodCode.SelectAll();

        //}

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

        //private void btn_person_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
        //        PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
        //        PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
        //        PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

        //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 40))
        //        {
        //            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
        //            DateTime dt = new DateTime(Convert.ToInt32(FarsiLibrary.Utils.PersianDate.Now.Year),
        //                   Convert.ToInt32(1),
        //                   Convert.ToInt32(1), pc);
        //            PACNT._4_Person_Menu.Form01_PersonOperationList frm = new PACNT._4_Person_Menu.Form01_PersonOperationList
        //                (gridEX1.GetValue("column03").ToString(), dt, Class_BasicOperation.ServerDate());

        //            frm.ShowDialog();



        //        }
        //        else
        //            Class_BasicOperation.ShowMsg("",
        //                "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        //    }
        //    catch
        //    {
        //    }
        //}

        //private void bt_Refresh_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable GoodTable = clGood.MahsoolInfoForFactor(gridEX1.GetValue("Column02").ToString(), gridEX1.GetValue("Column42"));

        //        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
        //        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
        //    }
        //    catch
        //    {
        //    }
        //}

        string errore = string.Empty;
        string barcoderepeat = string.Empty;
        string barcode = string.Empty;
        Int64 countNotBarcode = 0;
        string barcoderror = string.Empty;
        string NumberProduct = string.Empty;
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            countNotBarcode = 0;
            barcoderror = "";
            errore = "";
           barcoderepeat = "";
           barcode = "";
          


            try
            {
                if (mlt_Function.Text==""||mlt_NameCustomer.Text==""||mlt_Ware.Text=="" || mlt_Ware.Text=="0"|| mlt_NameCustomer.Text=="0" || mlt_Function.Text=="0")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                    return;
                }

                if (table_010_SaleFactorBindingSource.Count>0|| table_011_Child1_SaleFactorBindingSource.Count>0)
                {
                      


                    string strreplace = System.Text.RegularExpressions.Regex.Replace(txt_Barcode.Text.Trim(), @"\t|\n|\r", "");
                    var b = clDoc.GetNextChars(strreplace.Trim(), 8);
                    bool flrepeat = false;
             countNotBarcode = 0;


                    foreach (string s in b)
                    {


                        flrepeat = false;
                     

                        if (s != "")
                        {


                           

                            dtsale = clDoc.ReturnTable(ConPCLOR, @"SELECT     TOP (1) " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 AS CodeCommondity, dbo.Table_005_TypeCloth.TypeCloth, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07 AS Count, 0 AS Recipt, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column35 AS weight, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column03 AS vahedshomaresh, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 AS TypeColor, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37 AS Machine, dbo.Table_005_TypeCloth.ID, 
                       ISNULL((dbo.Table_010_TypeColor.ID),0) AS IDColor, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02
FROM         " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid INNER JOIN
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = dbo.Table_005_TypeCloth.CodeCommondity LEFT OUTER JOIN
                      dbo.Table_010_TypeColor ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 = dbo.Table_010_TypeColor.TypeColor
WHERE     (" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 = " + s + ")ORDER BY " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid DESC, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 DESC");

                           

                            if (dtsale.Rows.Count>0 )
                            {

                                ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dtsale.Rows[0][10].ToString() + ")  ),0)");
                                ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + dtsale.Rows[0][11].ToString() + ") ),0)");
                                SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dtsale.Rows[0][10].ToString() + ") ),0)");
                                NumberProduct = clDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT        dbo.Table_035_Production.Number
FROM            dbo.Table_035_Production INNER JOIN
                         dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
WHERE        (dbo.Table_050_Packaging.Barcode = " + s + ")");
                            float Remain = FirstRemain(int.Parse(dtsale.Rows[0][1].ToString()), (s), mlt_Ware.Value.ToString());

                             foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                             {
                               
                                     if (s.ToString() == (item.Cells["Column34"].Value).ToString())
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

                            if (dtsale.Rows.Count > 0 )
                            {

                                    if (Remain > 0)
                                    {
                                        if (flrepeat == false)
                                        {
                                            //if (wear != "0")
                                            //{
                                            string Desc = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select Description from Table_050_Packaging where Barcode="+s+"");
                                                table_010_SaleFactorBindingSource.EndEdit();
                                                gridEX_List.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                                                gridEX_List.MoveToNewRecord();
                                                gridEX_List.SetValue("column01", ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Columnid"].ToString());
                                                gridEX_List.SetValue("Column34", dtsale.Rows[0][0].ToString());
                                                gridEX_List.SetValue("column02", dtsale.Rows[0][1].ToString());
                                                gridEX_List.SetValue("Column03", dtsale.Rows[0][7].ToString());
                                                gridEX_List.SetValue("GoodCode", dtsale.Rows[0][1].ToString());
                                                gridEX_List.SetValue("column06", dtsale.Rows[0][3].ToString());
                                                gridEX_List.SetValue("column07", dtsale.Rows[0][3].ToString());
                                                gridEX_List.SetValue("Column36", dtsale.Rows[0][5].ToString());
                                                gridEX_List.SetValue("Column37", dtsale.Rows[0][5].ToString());
                                                gridEX_List.SetValue("Column38", dtsale.Rows[0][8].ToString());
                                                gridEX_List.SetValue("Column39", dtsale.Rows[0][9].ToString());
                                                gridEX_List.SetValue("Column23", Desc == null ? "Null" : Desc + " - " + NumberProduct);


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

                                                table_011_Child1_SaleFactorBindingSource.EndEdit();
                                                table_012_Child2_SaleFactorBindingSource.EndEdit();
                                                gridEX_List.UpdateData();
                                                gridEX_List.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                                            }
                                        //}
                                    }
                                    else
                                    {
                                        errore += s + ",";


                                    }
                                }
                               

                            }
                           
                            else
                            {

                                countNotBarcode = Int64.Parse(s);
                                barcoderror += Int64.Parse(s) + " , ";

                            }
                        }
                        
                    }
                    
                       

                    if (countNotBarcode > 0)
                    {
                        MessageBox.Show("عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید" + Environment.NewLine + barcoderror.ToString());
                        //ToastNotification.Show(this, "" + countNotBarcode.ToString() + ".عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید", 3000, eToastPosition.MiddleCenter);

                    }
                    if (errore.Length > 7 || barcoderepeat.Length > 7)
                    {
                        MessageBox.Show((errore.Length>0 ?  "موجودی کالا های زیر در انبار موردنظر کافی نیست" + Environment.NewLine + errore.TrimEnd(','):"") + Environment.NewLine 
                            + barcoderepeat.TrimEnd(','));
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

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
                    decimal Weight = Convert.ToDecimal((gridEX_List.GetValue("column37")).ToString());

                    

                    DataTable dt = clDoc.ReturnTable(ConPCLOR, @"SELECT    " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 AS CodeCommondity, dbo.Table_005_TypeCloth.TypeCloth, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07 AS Count, 0 AS Recipt, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column35 AS weight, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS goodcode, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column03 AS vahedshomaresh, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 AS TypeColor, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37 AS Machine, dbo.Table_005_TypeCloth.ID, 
                      ISNULL((dbo.Table_010_TypeColor.ID),0) AS IDColor, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02
                    FROM         " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid INNER JOIN
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = dbo.Table_005_TypeCloth.CodeCommondity LEFT OUTER JOIN
                      dbo.Table_010_TypeColor ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 = dbo.Table_010_TypeColor.TypeColor
                    WHERE     (" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 = " + gridEX_List.GetValue("Column34").ToString() + ")ORDER BY " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid DESC, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 DESC");

                    ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dt.Rows[0][10].ToString() + ")),0)");
                    ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + dt.Rows[0][11].ToString() + ")),0)");
                    SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + dt.Rows[0][10].ToString() + ")),0)");
                    sum = Convert.ToDecimal(ficloth) + Convert.ToDecimal(ficolor);


                    if (SelectBrand.ToString() == "True")
                    {
                        //&& sum <= Convert.ToDecimal(gridEX_List.GetValue("Column10").ToString())
                        gridEX_List.SetValue("column20", fi * Weight);
                        gridEX_List.SetValue("column11", fi * Weight);
                        txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"], AggregateFunction.Sum).ToString());
                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());


                    }

                    else if (SelectBrand.ToString() == "False")
                    {
                        //&& Convert.ToDecimal(ficloth) <= Convert.ToDecimal(gridEX_List.GetValue("Column10").ToString())
                        gridEX_List.SetValue("column20", fi * Weight);
                        gridEX_List.SetValue("column11", fi * Weight);
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

        private void table_010_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {


                if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column09"].ToString() != "0" || ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column10"].ToString() != "0" )
                {
                    uiPanel0.Enabled = false;
                    gridEX_List.AllowEdit = InheritableBoolean.False;
                    gridEX_Extra.AllowDelete = InheritableBoolean.False;
                }
                if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column09"].ToString() == "0" || ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column10"].ToString() == "0")
                {
                    uiPanel0.Enabled = true;
                    gridEX_List.AllowEdit = InheritableBoolean.True;
                    gridEX_Extra.AllowDelete = InheritableBoolean.True;
                }
                else
                {

                    

                    txt_TotalPrice.Value = Convert.ToDouble(
                   gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                   AggregateFunction.Sum).ToString());
                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());
                    double Total = double.Parse(txt_TotalPrice.Value.ToString());
                }
            }
            catch
            {


            }
                
        }

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                        if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");

                        }

                    }
                }

                if (isadmin)
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_010_SaleFactor),0) as Row");
                    
                }
                else
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column13='"+Class_BasicOperation._UserName+"'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_010_SaleFactorBindingSource.Count > 0 && this.table_011_Child1_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_012_Child2_SaleFactorBindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_012_Child2_SaleFactorBindingSource.CurrencyManager.Current;
                        New["Column02"] = item["ColumnId"].ToString();
                        if (item["Column03"].ToString() == "True")
                        {
                            New["Column03"] = 0;
                            New["Column04"] = item["Column04"].ToString();
                        }
                        else
                        {
                            New["Column03"] = item["Column04"].ToString();
                            New["Column04"] = double.Parse(item["Column04"].ToString()) *
                                double.Parse(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString()) / 100;
                        }
                        New["Column05"] = item["Column02"].ToString();
                        this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {

                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                    if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }


                    if (isadmin)
                    {
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_010_SaleFactor where Column01>" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                        
                    }
                    else
                    {
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_010_SaleFactor where Column01>" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND Column13='"+Class_BasicOperation._UserName+"'),0) as Row");

                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

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
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                   
                    table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();

                    if (dataSet_01_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_01_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_01_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }

                    if (isadmin)
                    {
                        Table = clDoc.ReturnTable(ConSale,
                      "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column01<" +
                      ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = clDoc.ReturnTable(ConSale,
                      "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column01<" +
                      ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "  AND Column13='"+Class_BasicOperation._UserName+"'),0) as Row");
                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
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

        private void DelPaper()
        {
            string id = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
            string RowID = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", id).ToString();
            int SanadID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", RowID);
            int SaleID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column16", RowID);
            int ReturnID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column19", RowID);
            int OrderID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column17", RowID);
            int ExitID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", RowID);
            string command = string.Empty;
            //اگر حواله دارای سند باشد
            if (SanadID != 0)
            {
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 59))
                    throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید \n\r حذف حواله امکانپذیر نیست");

                clDoc.IsFinal_ID(SanadID);

                //***Delete Doc
                if (SaleID != 0)
                {


                    DataTable Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + SaleID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + SaleID;
                    command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column10=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + SaleID;



                    Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID;



                    Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID));
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID);

                }
                else if (ReturnID != 0)
                {
                    //حذف سند فاکتور مرجوعی خرید 
                    clDoc.DeleteDetail_ID(SanadID, 20, ReturnID);

                    DataTable Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=20 and Column17=" + ReturnID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=20 and Column17=" + ReturnID;

                    command += " UPDATE " + ConSale.Database + ".dbo.Table_021_MarjooiBuy SET Column11=0  where ColumnId=" + ReturnID;



                }
                else
                //حذف سند مربوط به حواله
                {
                    DataTable Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID;



                    Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID));
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID);
                }

            }
            //صفر شدن شماره حواله در فاکتور فروش
            if (SaleID != 0)
                command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + SaleID;



            //صفر شدن شماره حواله در فاکتور مرجوعی خرید
            if (ReturnID != 0)
                command += " UPDATE " + ConSale.Database + ".dbo.Table_021_MarjooiBuy SET Column10=0  where ColumnId=" + ReturnID;


            //اگر حواله دارای شماره سفارش باشد خروج کالاهای سفارش صفر شده و خروج برگه سفارش نیز برداشته می شود 
            if (OrderID != 0)
            {

                command += " UPDATE " + ConSale.Database + ".dbo.Table_005_OrderHeader SET Column33=0  where ColumnId=" + OrderID;


                command +=
                    @"UPDATE " + ConSale.Database + @".dbo.Table_006_OrderDetails
                    SET    Column16  = " + ConSale.Database + @".dbo.Table_006_OrderDetails. Column16 -b.column04,
                           column15  = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column15 - b.column05,
                           column14  = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column14 -b.column06,
                           column17  = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column17 -b.column07,
                           Column23  = 0,
                           Column24  = NULL,
                           Column25  = NULL,
                           Column26  = NULL,
                           Column27  = NULL
                    FROM   " + ConSale.Database + @".dbo.Table_006_OrderDetails
                           JOIN " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft AS b
                                ON  b.column28 = " + ConSale.Database + @".dbo.Table_006_OrderDetails.columnid
                                AND b.column27 = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column01
                    WHERE " + ConSale.Database + @".dbo.Table_006_OrderDetails.column01=" + OrderID + " AND b.Column01=" + RowID;
                //اگر محاسبه جایزه در سفارش بر اساس خروجی باشد کلیه جوایز مربوط به سفارش حذف می شود
                if (Convert.ToBoolean(clDoc.ExScalar(ConSale.ConnectionString, "Table_005_OrderHeader", "Column34", "ColumnId", OrderID.ToString())))
                {

                    command += " Delete " + ConSale.Database + ".dbo.Table_006_OrderDetails where Column31=1 and Column01=" + OrderID;

                }
            }

            //****Delete Exit Paper
            if (ExitID != 0)
            {
                command += " Delete " + this.ConWare.Database + ".dbo.Table_009_ExitPwhrs where Column19=" + RowID;
            }
            //***Delete Detail
            command += " Delete " + this.ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=" + RowID;

            //***Delete Header
            command += " Delete " + this.ConWare.Database + ".dbo.Table_007_PwhrsDraft where ColumnId=" + RowID;

            //Update Table_035_DraftRequest
            command += " UPDATE " + this.ConWare.Database + ".dbo.Table_035_DraftRequest SET Column07=0 , Column12=0 where Column07=" + RowID;


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
                    Class_BasicOperation.ShowMsg("", "حذف حواله با موفقیت صورت گرفت", "Information");

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
        private void mnu_DelDraft_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DraftId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID.ToString());

                    if (DraftId != 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 73))
                        {



                            clDoc.ConfirmedDraftReceipt("Draft", DraftId.ToString());

                            if (clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column20", DraftId.ToString()) != 0)
                                throw new Exception("به علت دارا بودن کارت تولید، حذف این حواله امکانپذیر نمی باشد" + Environment.NewLine + "جهت حذف از کارت تولید مربوط اقدام کنید");


                            if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID.ToString()) != 0)
                                throw new Exception("به علت ارجاع این فاکتور، حذف حواله امکانپذیر نمی باشد");


                            string Message = "";
                            if ((clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", DraftId.ToString()) != 0) &&
                                clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", DraftId.ToString()) != 0)
                                Message = "برای این حواله سند حسابداری و برگه خروج صادر شده است. در صورت تأیید حذف برگه خروج و سند مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف این حواله هستید؟";
                            else if (clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", DraftId.ToString()) != 0)
                                Message = "برای این حواله سند حسابداری صادر شده است. در صورت تأیید حذف، سند مربوط نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این حواله هستید؟";
                            else if (clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", DraftId.ToString()) != 0)
                                Message = "برای این حواله برگه خروج صادر شده است. در صورت تأیید حذف، برگه مربوطه نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این حواله هستید؟";

                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این حواله هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                if (Message.Trim() != "")
                                {
                                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                        DelPaper();
                                }
                                else
                                    DelPaper();



                            }

                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                    }
                   
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                    
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }

        }

        private void bt_DelDoc_Click_2(object sender, EventArgs e)
        {
            string command = string.Empty;
           

            DataTable Table = new DataTable();
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 101))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int SanadID = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID.ToString());
                    int DraftID = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID.ToString());

                    string id = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    string RowID1 = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", id).ToString();
                    int SaleID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column16", RowID1);
                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID.ToString()) != 0)

                        throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");

                    if (SanadID != 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";

                        if (DraftID != 0)
                        {
                            Message = "برای این فاکتور، حواله انبار نیز صادر شده است. در صورت تأیید ثبت مربوط به حواله نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.IsFinal_ID(SanadID);

                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + RowID;



                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + DraftID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + DraftID;

                            command += " Delete " + this.ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=" + RowID1;

                            command += " Delete " + this.ConWare.Database + ".dbo.Table_007_PwhrsDraft where ColumnId=" + RowID1;


                            command += " UPDATE " + ConWare.Database + ".dbo. Table_007_PwhrsDraft SET Column07=0,Column10='" + Class_BasicOperation._UserName + "', Column11=getdate() where ColumnId=" + DraftID;


                            command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column10=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;

                            if (SaleID != 0)
                                command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + SaleID;
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
                    }
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    DS.Tables["Doc"].Clear();
                    DocAdapter.Fill(DS, "Doc");
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void mlt_NameCustomer_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_NameCustomer, "column02", "column01");
        }

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware, "column02", "column01");
            
        }

        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "column02", "column01");
            
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

        private void ForiFctor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {

                        PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
                        PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                        PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                        PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;

                        PSALE._05_Sale.Reports.Form_SaleFactorPrint frm = new PSALE._05_Sale.Reports.Form_SaleFactorPrint(
                                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);

                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 103))
                        throw new Exception("کاربر گرامی شما امکان ابطال فاکتور فروش را ندارید");

                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID) != 0 || clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID) != 0)
                        throw new Exception("به علت صدور حواله برای این فاکتور، ابطال فاکتور امکانپذیر نیست");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("این فاکتور باطل شده است");


                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", Convert.ToInt32(RowID), 1);
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, _ID);
                dataSet_01_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                
            }
        }

        private void mnu_CancelCancel_Click_1(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 104))
                        throw new Exception("کاربر گرامی شما امکان لغو ابطال فاکتور فروش را ندارید");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 0);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 0);
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(RowID));
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                dataSet_01_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                
            }
        }

        private void uiGroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void مشاهدهحوالهانبارToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;


            if (mlt_Draft.Text.Trim() == "0" || mlt_Draft.Text.Trim() == "")
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 26))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form07_ViewDrafts")
                        {
                            item.BringToFront();
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts frm = new PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts();
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
            else
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form06_RegisterDrafts")
                        {
                            item.BringToFront();
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).txt_Search.Text = mlt_Draft.Text;
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(mlt_Draft.Value.ToString()));
                    frm.ShowDialog();
                    int SaleId = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_01_Sale.Table_010_SaleFactor, SaleId);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_01_Sale.Table_011_Child1_SaleFactor, SaleId);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_012_Child2_SaleFactor, SaleId);
                    dataSet_01_Sale.EnforceConstraints = true;
                    
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void ToolStripButton7_Click(object sender, EventArgs e)
        {

        }

        private void مشاهدهاسنادحسابداریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString()));

            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;


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
                catch
                {
                }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void مشاهدهفاکتورمرجوعیToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void معرفیعناوینامضاToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _002_Sale.Frm_019_Sale_Signatures frm = new Frm_019_Sale_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void موجودیمقطعیتعدادیانبارToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 31))
            {
                PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi ob = new PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi();
                try
                {
                    ob.MdiParent = Frm_Main.ActiveForm;
                }
                catch { }
                ob.Show();

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

               PSALE. _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new PSALE. _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
                ob.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
                DS.Tables["Discount"].Rows.Clear();
                Adapter.Fill(DS, "Discount");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 92))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_002_ViewFactorReturn")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                MarjooiSale.Frm_002_ViewFactorReturn frm = new MarjooiSale.Frm_002_ViewFactorReturn();

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

        private void gridEX_List_Click(object sender, EventArgs e)
        {

        }

        private void بهروزرسانیقیمتپیشنهادیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);
                this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
                this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);

                if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0" && ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
                {
                    MessageBox.Show("فاکتور فروش داری سند و حواله می باشد امکان تغییر قیمت را ندارید");
                    return;
                }

                Frm_004_UpdateBrand frm = new Frm_004_UpdateBrand();
                frm.ShowDialog();

                ///
                decimal PriceNew = frm.Price;
                ///update grid
                if (gridEX_List.RowCount > 0 || gridEX_Extra.RowCount > 0)
                {
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        item.BeginEdit();

                        if (item.Cells["Column38"].Value.ToString() == frm.mlt_Color.Text && item.Cells["Column02"].Value.ToString() == ((DataRowView)frm.mlt_Cloth.DropDownList.FindItem(frm.mlt_Cloth.Value))["CodeCommondity"].ToString())
                        {
                            ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + frm.mlt_Cloth.Value.ToString() + ")),0)");
                            ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + frm.mlt_Color.Value.ToString() + ")),0)");
                            SelectBrand = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     SelectBrand  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + frm.mlt_Cloth.Value.ToString() + ")),0)");

                            sum = PriceNew + Convert.ToDecimal(ficolor);
                            decimal fi = PriceNew;
                            decimal Weight = Convert.ToDecimal((item.Cells["Column37"].Value.ToString()));
                            if (SelectBrand.ToString() == "True")
                            {
                                //&& Convert.ToDecimal(ficloth) <= PriceNew
                                gridEX_List.SetValue("column20", fi * Weight);
                                gridEX_List.SetValue("column11", fi * Weight);
                                clDoc.Execute(ConSale.ConnectionString, @"Update Table_011_Child1_SaleFactor set Column10="
                               + sum + " where Column38=N'" + item.Cells["Column38"].Value.ToString() + "' and Column01=" + ((DataRowView)table_011_Child1_SaleFactorBindingSource.CurrencyManager.Current)["Column01"] + "");
                            }

                            else if (SelectBrand.ToString() == "False")
                            {
                                //&& Convert.ToDecimal(ficloth) <= PriceNew
                                gridEX_List.SetValue("column20", fi * Weight);
                                gridEX_List.SetValue("column11", fi * Weight);
                                clDoc.Execute(ConSale.ConnectionString, @"Update Table_011_Child1_SaleFactor set Column10="
                               + PriceNew + " where Column38=N'" + item.Cells["Column38"].Value.ToString() + "' and Column01=" + ((DataRowView)table_011_Child1_SaleFactorBindingSource.CurrencyManager.Current)["Column01"] + "");
                            }

                            //else
                            //{
                            //    MessageBox.Show("مقدار وارد شده از مقدار پیشنهادی کمتر می باشد امکان اضافه آن را ندارید");
                            //    return;
                            //}

                        }



                        item.EndEdit();
                    }

                    //txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"], AggregateFunction.Sum).ToString());
                    //txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                    //txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());

                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(txt_ID.Text));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, int.Parse(txt_ID.Text));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(txt_ID.Text));
                    dataSet_01_Sale.EnforceConstraints = true;
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void bt_Print_ButtonClick(object sender, EventArgs e)
        {

        }

        private void مشاهدهفاکتورهایفروشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _002_Sale.Frm_003_ViewFactorSale frm = new _002_Sale.Frm_003_ViewFactorSale();
            frm.ShowDialog();
        }


    }
}
