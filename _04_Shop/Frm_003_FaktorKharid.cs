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


namespace PCLOR._04_Shop
{
    public partial class Frm_003_FaktorKharid : Form
    {
        bool _del;
        int _ID = 0, ReturnId = 0, ReturnNum = 0, DraftID = 0, DraftNum = 0, _ResidID = 0;

        SqlParameter ReturnDocNum;

        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);

        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
       
        DataSet DS = new DataSet();
        SqlDataAdapter ResidAdapter, DocAdapter, ReturnAdapter;
        string ReturnDate = null;
        DataTable ChildResidTbl = new DataTable();
        DataTable ResidHeaderTbl = new DataTable();
        bool _FromRecipt = false;
        SqlCommand _UpdatePriceInReceipt;
        double _arzeshvahed = 0;

        public Frm_003_FaktorKharid(bool del, int ID)
        {
            _del = del;
            InitializeComponent();
            _ID = ID;
            ResidAdapter = new SqlDataAdapter("SELECT ColumnId,Column01 from Table_011_PwhrsReceipt", ConWare);
            ResidAdapter.Fill(DS, "Resid");

            DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            DocAdapter.Fill(DS, "Doc");

            ReturnAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_021_MarjooiBuy", ConSale);
            ReturnAdapter.Fill(DS, "Return");
        }
        public Frm_003_FaktorKharid(bool del, int ResidID, bool FromRecipt)
        {

            InitializeComponent();
            _ResidID = ResidID;
            _del = del;
            _FromRecipt = true;


        }
        private void Frm_003_FaktorKharid_Load(object sender, EventArgs e)
        {
              bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
                ReturnDocNum.Direction = ParameterDirection.Output;
                ToastNotification.ToastBackColor = Color.Aquamarine;
                ToastNotification.ToastForeColor = Color.Black;

                mnu_CalculatePrice.Checked = Properties.Settings.Default.BuyFactorPriceCompute;

                foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
                {
                    col.CellStyle.BackColor = Color.White;
                    if (col.Key == "Column05" || col.Key == "Column07")
                        col.DefaultValue = Class_BasicOperation._UserName;
                    if (col.Key == "Column06" || col.Key == "Column08")
                        col.DefaultValue = Class_BasicOperation.ServerDate();
                }
                DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
                DocAdapter.Fill(DS, "Doc");

                ReturnAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_021_MarjooiBuy", ConSale);
                ReturnAdapter.Fill(DS, "Return");

                ResidAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_011_PwhrsReceipt", ConWare);
                ResidAdapter.Fill(DS, "Resid");

                gridEX1.DropDowns["Resid"].SetDataBinding(DS.Tables["Resid"], "");
                gridEX1.DropDowns["Doc"].SetDataBinding(DS.Tables["Doc"], "");
                gridEX1.DropDowns["Return"].SetDataBinding(DS.Tables["Return"], "");

                GoodbindingSource.DataSource = clGood.GoodInfo();
                DataTable GoodTable = clGood.GoodInfo();
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

                gridEX_List.DropDowns["Brand"].DataSource = clDoc.ReturnTable(ConPCLOR, @"select Id,TypeColor from Table_010_TypeColor");

                gridEX_List.DropDowns["Tamin"].DataSource = clDoc.ReturnTable(ConPCLOR, @"select Id,namemachine from Table_60_SpecsTechnical");
            
            
            //gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase, "Select * from ListPeople(5)"), "");

                gridEX1.DropDowns["Seller"].DataSource = clDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");

                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
                Adapter.Fill(DS, "CountUnit");
                gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

                Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 FROM Table_030_ExpenseCenterInfo", ConBase);
                Adapter.Fill(DS, "Center");
                gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

                Adapter = new SqlDataAdapter("SELECT Column00,Column01,Column02 FROM Table_035_ProjectInfo", ConBase);
                Adapter.Fill(DS, "Project");
                gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


                gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase, "Select * from PeopleScope(8,4)"), "");

                gridEX1.DropDowns["WHRS"].DataSource = clDoc.ReturnTable(ConWare, "Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE 'True'='" + isadmin.ToString() + @"'  or  Columnid IN (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
                gridEX1.DropDowns["Func"].DataSource = clDoc.ReturnTable(ConWare, "Select * from table_005_PwhrsOperation where Column16=0");
                DataTable dt = new DataTable();

                SqlDataAdapter ProjectAdapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
                ProjectAdapter.Fill(dt);
                gridEX1.DropDowns["project"].SetDataBinding(dt, "");
                Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
                Adapter.Fill(DS, "Discount");
                gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

                gridEX_List.DropDowns["Factor"].SetDataBinding(clDoc.ReturnTable(ConWare, "Select Columnid,Column01 from Table_023_RequestBuy"), "");

                DataTable CurrencyTable = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_055_CurrencyInfo");
                gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
                gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

                if (_FromRecipt)
                {
                    ResidHeaderTbl = clDoc.ReturnTable(ConWare, "Select * from Table_011_PwhrsReceipt where ColumnId=" + _ResidID);
                    ChildResidTbl = clDoc.ReturnTable(ConWare, "Select * from Table_012_Child_PwhrsReceipt where Column01=" + _ResidID);
                    this.table_015_BuyFactorBindingSource.AddNew();

                    DataRowView HeaderRow = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;
                    HeaderRow["Column02"] = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
                    HeaderRow["Column03"] = ResidHeaderTbl.Rows[0]["Column05"].ToString();
                    HeaderRow["Column04"] = "فاکتور خرید صادره از رسید شماره " + ResidHeaderTbl.Rows[0]["Column01"].ToString() + " به تاریخ " + ResidHeaderTbl.Rows[0]["Column02"].ToString();
                    HeaderRow["Column05"] = Class_BasicOperation._UserName;
                    HeaderRow["Column06"] = Class_BasicOperation.ServerDate();
                    HeaderRow["Column07"] = Class_BasicOperation._UserName;
                    HeaderRow["Column08"] = Class_BasicOperation.ServerDate();
                    HeaderRow["Column09"] = 0;
                    HeaderRow["Column10"] = _ResidID;
                    HeaderRow["Column11"] = 0;
                    HeaderRow["Column14"] = ResidHeaderTbl.Rows[0]["Column20"];
                    HeaderRow["Column15"] = ResidHeaderTbl.Rows[0]["Column16"];
                    HeaderRow["Column17"] = 0;
                    HeaderRow["Column18"] = 0;
                    HeaderRow["Column19"] = 0;
                    HeaderRow["Column25"] = (ResidHeaderTbl.Rows[0]["Column17"].ToString().Trim() == "" ? "-1" : ResidHeaderTbl.Rows[0]["Column17"].ToString());
                    HeaderRow["Column26"] = ResidHeaderTbl.Rows[0]["Column18"].ToString();
                    HeaderRow["Column27"] = ResidHeaderTbl.Rows[0]["Column03"];
                    HeaderRow["Column28"] = ResidHeaderTbl.Rows[0]["Column04"];
                    HeaderRow["Column31"] = 0;

                    this.table_015_BuyFactorBindingSource.EndEdit();
                    double t = 0;
                    foreach (DataRow item in ChildResidTbl.Rows)
                    {
                        this.table_016_Child1_BuyFactorBindingSource.AddNew();
                        DataRowView Row = (DataRowView)this.table_016_Child1_BuyFactorBindingSource.CurrencyManager.Current;
                        Row["Column02"] = item["Column02"].ToString();
                        Row["Column03"] = item["Column03"].ToString();
                        Row["Column04"] = item["Column04"].ToString();
                        Row["Column05"] = item["Column05"].ToString();
                        Row["Column06"] = item["Column06"].ToString();
                        Row["Column07"] = item["Column07"].ToString();
                        //به درخواست اقای رکابدار به صورت پیشفرض از اخرین فاکتور خرید غیر صفر میخورد در غیر این صورت از رسید میخواند
                        DataTable Table = new DataTable();
                        Adapter = new SqlDataAdapter(@"SELECT top 1 tcbf.column08,
                                                                                   tcbf.column09,
                                                                                   tcbf.column10
                                                                            FROM   Table_016_Child1_BuyFactor tcbf
                                                                                   JOIN Table_015_BuyFactor tbf
                                                                                        ON  tbf.columnid = tcbf.column01
                                                                            WHERE  (tcbf.column08>0 or tcbf.column09>0 or tcbf.column10>0 )
                                                                                and   tcbf.column02=" + item["Column02"] + @" 
                                                                                and  tbf.column03 = " + ResidHeaderTbl.Rows[0]["Column05"] + @"
                                                                            ORDER BY
                                                                                   tbf.column02 DESC", ConSale);
                        Adapter.Fill(Table);
                        SqlCommand InsertDetail = new SqlCommand();
                        double TotalPrice = 0;
                        if (Table.Rows.Count > 0 && (Convert.ToDecimal(Table.Rows[0]["column08"]) > 0 ||
                                                     Convert.ToDecimal(Table.Rows[0]["column09"]) > 0 ||
                                                     Convert.ToDecimal(Table.Rows[0]["column10"]) > 0)
                                                 && global ::PWHRS.Properties.Settings.Default.BuyFactorFromLastFactor
                            )
                        {

                            TotalPrice = (double.Parse(item["Column04"].ToString()) * double.Parse(Table.Rows[0]["column08"].ToString())) +
                                                    (double.Parse(item["Column05"].ToString()) * double.Parse(Table.Rows[0]["column09"].ToString())) +
                                                    (double.Parse(item["Column06"].ToString()) * double.Parse(Table.Rows[0]["column10"].ToString()));

                            Row["Column08"] = Table.Rows[0]["column08"];
                            Row["Column09"] = Table.Rows[0]["column09"];
                            Row["Column10"] = Table.Rows[0]["column10"];
                            Row["Column11"] = TotalPrice;
                        }
                        else
                        {
                            //TotalPrice = (double.Parse(item["Column04"].ToString()) * double.Parse(item["Column08"].ToString())) +
                            //          (double.Parse(item["Column05"].ToString()) * double.Parse(item["Column09"].ToString())) +
                            //          (double.Parse(item["Column06"].ToString()) * double.Parse(item["Column10"].ToString()));
                            Row["Column08"] = item["Column08"].ToString();
                            Row["Column09"] = item["Column09"].ToString();
                            Row["Column10"] = item["Column10"].ToString();

                            if (!mnu_CalculatePrice.Checked)
                            {
                                TotalPrice = Convert.ToInt64((Convert.ToDouble(item["Column04"]) * Convert.ToDouble(item["Column08"]))
                                   + (Convert.ToDouble(item["Column05"]) * Convert.ToDouble(item["Column09"])) +
                                   (Convert.ToDouble(item["Column06"]) * Convert.ToDouble(item["Column10"])));


                            }
                            else
                            {
                                TotalPrice =

                                   Convert.ToInt64(Convert.ToDouble(item["Column07"]) * Convert.ToDouble(item["Column10"]));



                            }
                            Row["Column11"] = TotalPrice;



                        }
                        Row["Column13"] = (item["Column25"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column25"].ToString());
                        Row["Column14"] = item["Column26"].ToString();
                        Row["Column16"] = 0;
                        Row["Column17"] = 0;
                        Row["Column18"] = 0;
                        Row["Column19"] = 0;
                        Row["column20"] = TotalPrice;
                        t += TotalPrice;
                        Row["Column21"] = (item["Column13"].ToString().Trim() != "" ? item["Column13"].ToString().Trim() : (object)DBNull.Value);
                        Row["Column22"] = (item["Column14"].ToString().Trim() != "" ? item["Column14"].ToString() : (object)DBNull.Value);
                        Row["Column23"] = (item["Column12"].ToString().Trim() != "" ? item["Column12"].ToString().Trim() : (object)DBNull.Value);
                        Row["Column24"] = 0;
                        Row["Column25"] = item["columnid"];
                        Row["Column26"] = _ResidID;
                        Row["Column27"] = item["Column28"].ToString();
                        Row["Column28"] = 0;
                        Row["Column29"] = item["Column32"].ToString();
                        Row["Column30"] = item["Column33"].ToString();
                        Row["Column31"] = 100;
                        Row["Column32"] = (item["Column30"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column30"].ToString());
                        Row["Column33"] = (item["Column31"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column31"].ToString());
                        Row["Column34"] = item["Column34"].ToString();
                        Row["Column35"] = item["Column35"].ToString();
                        Row["Column36"] = (item["Column36"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column36"].ToString());
                        Row["Column37"] = (item["Column37"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column37"].ToString());
                        Row.EndEdit();
                        table_016_Child1_BuyFactorBindingSource.EndEdit();
                    }
                    gridEX_List.RemoveFilters() ;
                  
                    gridEX_Extra.RemoveFilters();

                 
                    ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column20"] = t;
                    /* Convert.ToDouble(
                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                        AggregateFunction.Sum).ToString());*/
                    txt_EndPrice.Value = t + Convert.ToDouble(txt_Extra.Value.ToString())
                           - Convert.ToDouble(txt_Reductions.Value.ToString());
                }

                if (_ID != 0)
                {
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, _ID);
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, _ID);
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, _ID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }
                this.WindowState = FormWindowState.Maximized;

                if (gridEX1.GetValue("Column10") == null && gridEX1.GetValue("Column11") == null)
                {
                    gridEX1.AllowEdit = InheritableBoolean.True;
                    gridEX1.Enabled = true;
                    gridEX_List.AllowEdit = InheritableBoolean.True;
                    gridEX_Extra.AllowEdit = InheritableBoolean.True;
                    gridEX_List.AllowAddNew = InheritableBoolean.True;
                    gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                    gridEX_Extra.AllowDelete = InheritableBoolean.True;
                    gridEX_List.AllowDelete = InheritableBoolean.True;
                }
            

        }

        private void table_015_BuyFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_015_BuyFactorBindingSource.Count > 0)
                {
                    DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                    if (Row["Column10"].ToString() != "0" && (string.IsNullOrWhiteSpace(Row["Column27"].ToString()) || string.IsNullOrWhiteSpace(Row["Column28"].ToString())))
                    {

                        DataTable dt = new DataTable();
                        SqlDataAdapter RecipttAdapter = new SqlDataAdapter("SELECT * from Table_011_PwhrsReceipt where columnid=" + Row["Column10"] + "", ConWare);
                        RecipttAdapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Row["Column27"] = dt.Rows[0]["column03"];
                            Row["Column28"] = dt.Rows[0]["column04"];
                        }
                       
                        else
                        {
                            Class_BasicOperation.ShowMsg("", "رسید وجود ندارد", "Information");
                            return;

                        }
                    }

                    //اگر برای فاکتور فقط رسید صادر شده باشد 
                    if (Row["Column10"].ToString() != "0" && Row["Column11"].ToString() == "0")
                    {
                        gridEX1.AllowEdit = InheritableBoolean.False;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowAddNew = InheritableBoolean.False;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.False;
                    }
                    //در صورت اینکه فاکتور دارای سند باشد، یا مرجوعی باشد یا باطل شده باشد 
                    else if (Row["Column11"].ToString().Trim() != "0"
                            || Row["Column17"].ToString().Trim() != "False"
                            || Row["Column19"].ToString().Trim() != "False")
                    {
                        gridEX1.AllowEdit = InheritableBoolean.False;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowEdit = InheritableBoolean.False;
                        gridEX_Extra.AllowEdit = InheritableBoolean.False;
                        gridEX_List.AllowAddNew = InheritableBoolean.False;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.False;
                        gridEX_Extra.AllowDelete = InheritableBoolean.False;
                        gridEX_List.AllowDelete = InheritableBoolean.False;
                    }
                    else
                    {
                        gridEX1.AllowEdit = InheritableBoolean.True;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_Extra.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.True;
                    }

                    try
                    {
                        txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
                    }
                    catch
                    {
                    }
                    DraftID = 0;
                    DraftNum = 0;
                    ReturnId = 0;
                    ReturnNum = 0;
                    ReturnDate = null;
                }
                 
           
               
              

                }
        
            catch
            { }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.Enabled = true;
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, 0);
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, 0);
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, 0);
                dataSet_01_Sale.EnforceConstraints = true;
                gridEX1.MoveToNewRecord();
                //gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_015_BuyFactor", "Column01").ToString());
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());
                //using (SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.PWHRS))
                //{

                //    ConWHRS.Open();
                //    SqlCommand Command = new SqlCommand("Select top 1 columnid from Table_001_PWHRS  ", ConWHRS);
                //    gridEX1.SetValue("Column27", Convert.ToInt16(Command.ExecuteScalar()));
                //}

                using (SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.PWHRS))
                {

                    ConWHRS.Open();
                    SqlCommand Command = new SqlCommand("Select top 1 columnid from table_005_PwhrsOperation where column16=0  ", ConWHRS);
                    gridEX1.SetValue("Column28", Convert.ToInt16(Command.ExecuteScalar()));
                }
                gridEX1.RootTable.Columns["Column25"].Selectable = false;
                gridEX1.RootTable.Columns["Column26"].Selectable = false;
                gridEX1.Select();
                gridEX1.Col = 2;
                bt_New.Enabled = false;
                table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void SaveEvent(object sender, EventArgs e)
        {
            gridEX1.UpdateData();
            DataRowView Row =
                      (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

            if (this.table_015_BuyFactorBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {

                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();


                if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                {
                    Class_BasicOperation.ShowMsg("", "فاکتور تائید شده است امکان ذخیره اطلاعات وجود ندارد", "Stop");
                    this.Cursor = Cursors.Default;

                    return;


                }

                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    //Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("تاریخ معتبر نیست");


                }
                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    //Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    //return;
                    throw new Exception("کالایی ثبت نشده است");

                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد");

                }
                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString,
                        "Table_015_BuyFactor", "Column01").ToString());
                    this.table_015_BuyFactorBindingSource.EndEdit();
                }

                txt_TotalPrice.Value = Convert.ToDouble(
              gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
              AggregateFunction.Sum).ToString());

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                      Convert.ToDouble(txt_Reductions.Value.ToString());


                Row["Column07"] = Class_BasicOperation._UserName;
                Row["Column08"] = Class_BasicOperation.ServerDate();
                Row["Column20"] = gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                Row["Column23"] = gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                Row["Column24"] = gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                //Extra-Reductions

                double Total = double.Parse(txt_TotalPrice.Value.ToString());
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = (gridEX1.GetValue("Column15").ToString().Trim() == "True" ?
                            Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100 :
                            Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
                        item.EndEdit();

                    }
                }
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());




                if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                {
                    //Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("جمع کل فاکتور منفی شده است");

                }

                this.table_015_BuyFactorBindingSource.EndEdit();
                this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                this.table_015_BuyFactorTableAdapter.Update(dataSet_01_Sale.Table_015_BuyFactor);
                this.table_016_Child1_BuyFactorTableAdapter.Update(
                    dataSet_01_Sale.Table_016_Child1_BuyFactor);
                this.table_017_Child2_BuyFactorTableAdapter.Update(
                    dataSet_01_Sale.Table_017_Child2_BuyFactor);
                if (_FromRecipt)
                {
                    //قرار دادن شماره فاکتور خرید در رسید انبار
                    clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET  Column13=" +
                        Row["ColumnId"].ToString() + " where ColumnId=" + _ResidID);
                }


                //آپدیت کردن ارزش کالا در رسید
                if (Convert.ToInt32(gridEX1.GetValue("column10")) > 0)
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                    {
                        Con.Open();
                        //for (int k = 0; k < gridEX_List.RowCount; k++)
                        //{
                        //    gridEX_List.Row = k;

                        //    if (Convert.ToDouble(gridEX_List.GetValue("column07")) == 0)
                        //        _arzeshvahed = 0;
                        //    else
                        //        _arzeshvahed = Convert.ToDouble(gridEX_List.GetValue("column20")) /
                        //        Convert.ToDouble(gridEX_List.GetValue("column07"));

                        //    _UpdatePriceInReceipt = new SqlCommand(
                        //        "update Table_012_Child_PwhrsReceipt " +
                        //        "set column08=" + gridEX_List.GetValue("column08").ToString() +
                        //        ",column09=" + gridEX_List.GetValue("column09").ToString() +
                        //        ",column10=" + gridEX_List.GetValue("column10").ToString() + "," +
                        //        "column11=" + gridEX_List.GetValue("column11").ToString() +
                        //        ",column20=" + _arzeshvahed.ToString() + ",column21=" +
                        //        gridEX_List.GetValue("column20").ToString() +
                        //        " where column01=" + gridEX1.GetValue("column10").ToString() +
                        //        " and column02=" + gridEX_List.GetValue("column02").ToString(),
                        //        Con);

                        //    _UpdatePriceInReceipt.ExecuteNonQuery();

                        //}


                        _UpdatePriceInReceipt = new SqlCommand(
                             @" DECLARE @share  FLOAT,
                                                                            @sum    DECIMAL(18, 3),
                                                                            @Net    DECIMAL(18, 3)

                                                                    SET @sum = (
                                                                            SELECT SUM(ISNULL(tt.VE, 0))
                                                                            FROM   (
                                                                                       SELECT (
                                                                                                  CASE 
                                                                                                       WHEN tcbf.column05 = 0 THEN tcbf.column04
                                                                                                       ELSE ((-1) * tcbf.column04)
                                                                                                  END
                                                                                              ) AS VE
                                                                                       FROM   " + ConSale.Database + @".dbo.Table_017_Child2_BuyFactor tcbf
                                                                                              JOIN " + ConSale.Database + @".dbo.Table_024_Discount_Buy tdb
                                                                                                   ON  tdb.columnid = tcbf.column02
                                                                                       WHERE  tdb.Column18 = 1
                                                                                              AND tcbf.column01 = " + gridEX1.GetValue("columnid").ToString() + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   " + ConSale.Database + @".dbo.Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + gridEX1.GetValue("columnid").ToString() + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    SET @share=1+@share 
                                                UPDATE a
                                            SET    column08  = b.column08,
                                                   column09  = b.column09,
                                                   column10  = b.column10,
                                                   column11  = b.column11,
                                                   column20  =isnull( b.column20 /nullif(isnull(b.column07,0), 0),0) * @share ,
                                                   a.column21 = b.column20 *@share
                                            FROM   (
                                                       SELECT ROW_NUMBER() OVER(PARTITION BY column01 ORDER BY columnid, column02) AS 
                                                              Rown,
                                                              column01,
                                                              column02,
                                                              column20,
                                                              column07,
                                                              column08,
                                                              column09,
                                                              column10,
                                                              column11,
                                                              column21
                                                       FROM   Table_012_Child_PwhrsReceipt
                                                       WHERE  column01 = " + gridEX1.GetValue("column10").ToString() + @"
                                                   ) AS a
                                                   JOIN (
                                                            SELECT ROW_NUMBER() OVER(PARTITION BY column01 ORDER BY columnid, column02) AS 
                                                                   Rown,
                                                                   column01,
                                                                   column02,
                                                                   column20,
                                                                   column07,
                                                                   column08,
                                                                   column09,
                                                                   column10,
                                                                   column11,
                                                                   column21
                                                            FROM   " + ConSale.Database + @".dbo.Table_016_Child1_BuyFactor
                                                            WHERE  column01 = " + gridEX1.GetValue("columnid").ToString() + @"
                                                        ) AS b
                                                        ON  b.Rown = a.Rown
                                                        AND b.column02 = a.column02", Con);
                        _UpdatePriceInReceipt.ExecuteNonQuery();

                        gridEX_List.MoveFirst();
                    }
                }
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

               Frm_009_ExportDocInformation frm =
                                 new Frm_009_ExportDocInformation(true, 0,
                                     int.Parse(RowID), Convert.ToInt16(gridEX1.GetRow().Cells["Column27"].Value), Convert.ToInt16(gridEX1.GetRow().Cells["Column28"].Value));
                frm.ShowDialog();
                DS.Tables["Doc"].Clear();
                DocAdapter.Fill(DS, "Doc");
                DS.Tables["Resid"].Clear();
                ResidAdapter.Fill(DS, "Resid");
              
                //if (sender == bt_Save || sender == this)
                //    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                bt_New.Enabled = true;
            }
            //در هر صورت بعد از ذخیره اطلاعات دوباره اطلاعات به روز رسانی می شوند
            int _ID = int.Parse(Row["ColumnId"].ToString());
            dataSet_01_Sale.EnforceConstraints = false;
            this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, _ID);
            this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, _ID);
            this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, _ID);
            dataSet_01_Sale.EnforceConstraints = true;
            table_015_BuyFactorBindingSource_PositionChanged(sender, e);


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




        public void bt_Del_Click(object sender, EventArgs e)
        {
            DataTable Table = new DataTable();
            string command = string.Empty;

            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 155))
                    throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column11", RowID);
                int ResidId = clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column10", RowID);
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");


                    if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                    {
                        throw new Exception("فاکتور تائید شده است امکان حذف وجود ندارد");

                    }


                    if (int.Parse(RowID) > 0 && clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", RowID) == "True")
                    {
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        dataSet_01_Sale.EnforceConstraints = true;
                        throw new Exception("به علت ارجاع این فاکتور، حذف آن امکانپذیر نمی باشد");
                    }

                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (DocId > 0)
                        {
                            clDoc.IsFinal_ID(DocId);
                            //حذف سند فاکتور 
                            //clDoc.DeleteDetail_ID(DocId, 19, int.Parse(RowID));

                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + int.Parse(RowID);




                            //حذف سند مربوط به رسید
                            // clDoc.DeleteDetail_ID(DocId, 12, int.Parse(RowID));

                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=12 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=12 and Column17=" + int.Parse(RowID);




                        }
                        if (ResidId > 0)
                        {
                            //درج صفر در شماره سند رسید و صفر در شماره فاکتور خرید رسید
                            //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET Column07=0 , Column13=0 where ColumnId=" + ResidId);
                            command += "UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt SET Column07=0 , Column13=0 where ColumnId=" + ResidId;



                        }
                        //حذف فاکتور
                        //foreach (DataRowView item in this.table_016_Child1_BuyFactorBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                        //this.table_016_Child1_BuyFactorTableAdapter.Update(dataSet_01_Sale.Table_016_Child1_BuyFactor);
                        //foreach (DataRowView item in this.table_017_Child2_BuyFactorBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                        //this.table_017_Child2_BuyFactorTableAdapter.Update(dataSet_01_Sale.Table_017_Child2_BuyFactor);
                        //this.table_015_BuyFactorBindingSource.RemoveCurrent();
                        //this.table_015_BuyFactorBindingSource.EndEdit();
                        //this.table_015_BuyFactorTableAdapter.Update(dataSet_01_Sale.Table_015_BuyFactor);

                        command += " Delete from " + ConSale.Database + ".dbo.Table_017_Child2_BuyFactor  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_016_Child1_BuyFactor  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_015_BuyFactor  Where columnid =" + int.Parse(RowID);


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
                                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, 0);
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, 0);
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, 0);
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

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {

                table_015_BuyFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
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
                    gridEX_Extra.SetValue("column04",
                        (gridEX1.GetValue("Column15").ToString() == "False") ?
                        Convert.ToInt64(kol * darsad / 100) : kol * darsad / 100);
                }
            }
            else if (e.Column.Key == "column03")
            {
                Double darsad;
                darsad = Convert.ToDouble(e.Value.ToString());
                Double kol;
                kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                if (kol == 0)
                    return;
                gridEX_Extra.SetValue("column04",
                      (gridEX1.GetValue("Column15").ToString() == "False") ?
                      Convert.ToInt64(kol * darsad / 100) : kol * darsad / 100);
            }
        }

        private void gridEX_List_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }
            try
            {
                if (e.Column.Key == "column22")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column22", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            try
            {
                if (e.Column.Key == "column21")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column21", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
        }

        private void bt_ExportResid_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 74))
                        throw new Exception("کاربر گرامی شما امکان صدور رسید انبار را ندارید");

                    SaveEvent(sender, e);

                    if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID) != 0)
                        throw new Exception("برای این فاکتور رسید انبار صادر شده است");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور رسید انبار وجود ندارد");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت مرجوع شدن این فاکتور امکان صدور رسید انبار وجود ندارد");


                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodId", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value.ToString(),
                            item.Cells["Column02"].Text.Trim());
                    }

                    if (gridEX1.GetRow().Cells["Column27"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column28"].Text.Trim() != "")
                    {
                        try
                        {
                            int ok = 0;
                            using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.PACNT))
                            {
                                Conacnt.Open();
                                SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                                ok = int.Parse(Commnad.ExecuteScalar().ToString());

                            }
                            if (ok == 0)
                            {
                                dataSet_01_Sale.EnforceConstraints = false;
                                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, 0);
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, 0);
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, 0);
                                dataSet_01_Sale.EnforceConstraints = true;
                                MessageBox.Show("به انبار این قاکتور دسترسی ندارید");
                                return;

                            }


                        }
                        catch
                        {
                        }
                        PSALE._04_Buy.Frm_010_ResidInformationDialog frm = new PSALE._04_Buy.Frm_010_ResidInformationDialog(
                            this.table_016_Child1_BuyFactorBindingSource,
                            ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current),
                            Table, Convert.ToInt16(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"]),
                            Convert.ToInt16(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column28"]));
                        frm.ShowDialog();

                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع رسید را مشخص کنید", "None");

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                DS.Tables["Resid"].Clear();
                ResidAdapter.Fill(DS, "Resid");
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(
                    this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(
                    this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(
                    this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                dataSet_01_Sale.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 75))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    //SaveEvent(sender, e);

                    string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column11", RowID) != 0)
                    {
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Resid"].Clear();
                        ResidAdapter.Fill(DS, "Resid");
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(
                            this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(
                            this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
                    }

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور سند وجود ندارد");


                    //SaveEvent(sender, e);
                    //*******************1:اگر رسید قبلا صادر شده باشد
                    if (gridEX1.GetRow().Cells["Column27"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column28"].Text.Trim() != "")
                    {
                        try
                        {
                            int ok = 0;
                            using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.PACNT))
                            {
                                Conacnt.Open();
                                SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                                ok = int.Parse(Commnad.ExecuteScalar().ToString());

                            }
                            if (ok == 0)
                            {
                                dataSet_01_Sale.EnforceConstraints = false;
                                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, 0);
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, 0);
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, 0);
                                dataSet_01_Sale.EnforceConstraints = true;
                                MessageBox.Show("به انبار این قاکتور دسترسی ندارید");
                                return;

                            }


                        }
                        catch
                        {
                        }

                        DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;
                        if (int.Parse(Row["Column10"].ToString()) > 0)
                        {
                            //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                            PSALE. _04_Buy.Frm_009_ExportDocInformation frm =
                                new PSALE._04_Buy.Frm_009_ExportDocInformation(true,
                                    int.Parse(Row["Column10"].ToString()), int.Parse(RowID), Convert.ToInt16(gridEX1.GetRow().Cells["Column27"].Value), Convert.ToInt16(gridEX1.GetRow().Cells["Column28"].Value));
                            frm.ShowDialog();

                        }
                        //اگر رسید صادر نشده باشد
                        else
                        {
                           PSALE. _04_Buy.Frm_009_ExportDocInformation frm =
                                new PSALE._04_Buy.Frm_009_ExportDocInformation(true, 0,
                                    int.Parse(RowID), Convert.ToInt16(gridEX1.GetRow().Cells["Column27"].Value), Convert.ToInt16(gridEX1.GetRow().Cells["Column28"].Value));
                            frm.ShowDialog();
                        }
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Resid"].Clear();
                        ResidAdapter.Fill(DS, "Resid");
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(
                            this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(
                            this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع رسید را مشخص کنید", "None");



                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_015_BuyFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 156))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    int SanadID = clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column11", RowID);
                    int ResidID = clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column10", RowID);

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");


                    if (SanadID != 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";
                        if (ResidID != 0)
                        {
                            Message = "برای این فاکتور، رسید انبار نیز صادر شده است. در صورت تأیید ثبت مربوط به رسید نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.IsFinal_ID(SanadID);
                            //حذف سند فاکتور 
                            //  clDoc.DeleteDetail_ID(SanadID, 19, int.Parse(RowID));

                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=19 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=19 and Column17=" + int.Parse(RowID);



                            //حذف سند مربوط به رسید
                            //clDoc.DeleteDetail_ID(SanadID, 12, int.Parse(RowID));


                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=12 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=12 and Column17=" + int.Parse(RowID);





                            //درج صفر در شماره سند رسید
                            //clDoc.RunSqlCommand(ConWare.ConnectionString,
                            //    "UPDATE Table_011_PwhrsReceipt SET Column07=0 where ColumnId=" + ResidID);

                            command += "UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=0 where ColumnId=" + ResidID;




                            //آپدیت شماره سند در فاکتور
                            // clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column11", "ColumnId", int.Parse(RowID), 0);
                            command += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column11=0 where ColumnId=" + int.Parse(RowID);

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

                                    Class_BasicOperation.ShowMsg("", "حذف سند حسابداری  با موفقیت صورت گرفت", "Information");


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
                    DS.Tables["Doc"].Clear();
                    DocAdapter.Fill(DS, "Doc");
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_ReturnFactor_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 77))
                        throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور خرید را ندارید");
                    if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                    {
                        throw new Exception("فاکتور تائید شده است امکان مرجوع کردن وجود ندارد");

                    }

                    SaveEvent(sender, e);



                    DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                    if (Row["Column17"].ToString() != "" && Row["Column17"].ToString() == "True")
                        throw new Exception("این فاکتور قبلا مرجوع شده است");
                    if (Row["Column11"].ToString().Trim() == "" || Row["Column11"].ToString() == "0")
                        throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و رسید انبار، الزامیست");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        //چک باقی مانده کالا
                        foreach (DataRowView item in this.table_016_Child1_BuyFactorBindingSource)
                        {
                            if (clDoc.IsGood(item["Column02"].ToString()))
                            {

                                float Remain = FirstRemain(int.Parse(item["Column02"].ToString()));
                                if (Remain < float.Parse(item["Column07"].ToString()))
                                {
                                    throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02", "ColumnId", item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
                                }
                            }

                        }

                        //صدور حواله در صورت صادر شدن رسید برای فاکتور خرید

                        InsertDraft(Row);
                        if (ReturnId > 0 && DraftID > 0)
                        {
                            //ثبت عکس فاکتور فروش
                            InvertDoc(Row);
                            Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + ReturnDocNum.Value + Environment.NewLine + "شماره حواله انبار:" + DraftNum, "Information");
                            DS.Tables["Return"].Clear();
                            ReturnAdapter.Fill(DS, "Return");
                            int RowId = int.Parse(Row["ColumnId"].ToString());
                            dataSet_01_Sale.EnforceConstraints = false;
                            this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, RowId);
                            this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, RowId);
                            this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, RowId);
                            dataSet_01_Sale.EnforceConstraints = true;
                            this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                        }
                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private float FirstRemain(int GoodCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column03", "ColumnId",
                    ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column10"].ToString()), GoodCode,
                     ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column02"].ToString());
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void TurnBack(DataRowView Row)
        {
            if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column18", Row["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، فاکتور مرجوعی صادر شده است");

            ReturnDate = InputBox.Show("تاریخ ارجاع را وارد کنید:");
            if (!string.IsNullOrEmpty(ReturnDate))
            {

                //درج هدر مرجوعی
                ReturnNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column01");
                ReturnId = 0;
                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                Key.Direction = ParameterDirection.Output;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                {
                    Con.Open();
                    SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_021_MarjooiBuy  ([column01]
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
            ) VALUES(" + ReturnNum + ",'" + ReturnDate + "'," + Row["Column03"].ToString() + ",'"
                       + "ارجاع فاکتور خرید ش " + Row["Column01"].ToString() + " تاریخ " + Row["Column02"].ToString() + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                        "0,0,0," + (Row["Column12"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column12"].ToString().Trim() + "'") + "," +
                        (Row["Column13"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column13"].ToString().Trim() + "'") + "," +
                        (Row["Column14"].ToString().Trim() == "" ? "NULL" : Row["Column14"].ToString().Trim())
                        + "," + (Row["Column15"].ToString() == "True" ? 1 : 0)
                        + ",0," + Row["ColumnId"].ToString() + "," + Row["Column20"].ToString() + "," + Row["Column21"].ToString() + "," + Row["Column22"].ToString() + "," + Row["Column23"].ToString()
                        + "," + Row["Column24"].ToString() + "," +
                        (Row["Column25"].ToString().Trim() == "" ? "NULL" : Row["Column25"].ToString()) + "," +
                        (Row["Column15"].ToString() == "True" ? Row["Column26"].ToString() : "0") +
                        "); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(Key);
                    InsertHeader.ExecuteNonQuery();
                    ReturnId = int.Parse(Key.Value.ToString());

                    //درج دیتیل1
                    foreach (DataRowView item in this.table_016_Child1_BuyFactorBindingSource)
                    {
                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_022_Child1_MarjooiBuy        ([column01]
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
           ,[Column27]
           ,[Column28]
           ,[Column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]) VALUES(" + ReturnId + "," + item["Column02"].ToString() +
                            "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() +
                            "," + item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," +
                            item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                            (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString())
                            + "," + item["Column14"].ToString() + ",NULL," + item["Column16"].ToString() + "," + item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + "," + item["Column20"].ToString() +
                            "," + (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," +
                            (item["Column22"].ToString().Trim() != "" ? item["Column22"].ToString() : "NULL") + ",NULL," +
                            (item["Column27"].ToString() != "" ? item["Column27"].ToString() : "0") + ",0,0," +
                            (item["Column28"].ToString() != "" ? item["Column28"].ToString() : "0") + "," +
                            item["Column29"].ToString() + "," + item["Column30"].ToString() +
                            "," + (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                            "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", Con);
                        InsertDetail.ExecuteNonQuery();
                    }

                    //درج دیتیل 2
                    foreach (DataRowView item in this.table_017_Child2_BuyFactorBindingSource)
                    {
                        clDoc.RunSqlCommand(ConSale.ConnectionString, "INSERT INTO Table_023_Child2_MarjooiBuy VALUES(" + ReturnId + "," + item["Column02"].ToString()
                            + "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + (item["Column05"].ToString() == "True" ? 1 : 0) + "," +
                            (item["Column06"].ToString().Trim() == "" ? "NULL" : item["Column06"].ToString()) + ")");

                    }
                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_015_BuyFactor SET Column17=1 , Column18=" + ReturnId + " Where ColumnId=" + Row["ColumnId"].ToString());
                }
            }
        }

        private void InsertDraft(DataRowView Row)
        {
            if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
                return;

            PSALE._04_Buy.Frm_011_DraftInformationDialog frm = new PSALE._04_Buy.Frm_011_DraftInformationDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                //صدور فاکتور مرجوعی
                TurnBack(Row);

                //ResidTable
                DataTable ResidTable = clDoc.ReturnTable(ConWare, "Select * from Table_011_PwhrsReceipt where ColumnId=" + Row["Column10"].ToString());
                DataTable ResidChild = clDoc.ReturnTable(ConWare, "Select * from Table_012_Child_PwhrsReceipt where Column01=" + Row["Column10"].ToString());

                string Function = frm.FunctionValue;
                DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
                //, int.Parse(ResidTable.Rows[0]["Column03"].ToString()));

                //**draft Header
                SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                key.Direction = ParameterDirection.Output;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                {
                    Con.Open();
                    SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(" + DraftNum + ",'" + ReturnDate + "'," + ResidTable.Rows[0]["Column03"].ToString()
                     + "," + Function + "," + ResidTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
                     ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                     + "',getdate(),0,NULL,NULL,0,0,0,0," + ReturnId + ",0,0,0," +
                     (ResidTable.Rows[0]["Column16"].ToString() == "True" ? 1 : 0) + "," +
                     (ResidTable.Rows[0]["Column17"].ToString().Trim() != "" ? ResidTable.Rows[0]["Column17"].ToString() : "NULL") + "," +
                     ResidTable.Rows[0]["Column18"].ToString()
                     + ",1); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(key);
                    InsertHeader.ExecuteNonQuery();
                    DraftID = int.Parse(key.Value.ToString());

                    //Resid Detail
                    foreach (DataRow item in ResidChild.Rows)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {

                            SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_008_Child_PwhrsDraft ([column01]
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
           ,[Column35]) VALUES(" + DraftID + "," + item["Column02"].ToString() + "," +
                             item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                             item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                             item["Column11"].ToString() + ",NULL," + (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) +
                             "," + item["Column20"].ToString() +
                             "," + item["Column21"].ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                             + "',getdate(),NULL,NULL," +
                             (item["Column25"].ToString().Trim() == "" ? "NULL" : item["Column25"].ToString()) + "," +
                             item["Column26"].ToString() + ",0,0,0,0,0," +
                             (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString().Trim() + "'") + "," +
                             (item["Column31"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString().Trim() + "'") +
                             "," + item["Column32"].ToString() + "," + item["Column33"].ToString() + "," +
                             item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", Con);
                            InsertDetail.ExecuteNonQuery();
                        }
                    }
                    //درج شماره حواله در فاکتور مرجوعی
                    clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column10", "ColumnId", ReturnId, DraftID);
                }
            }


        }

        private void InvertDoc(DataRowView Row)
        {
            if (Row["Column11"].ToString().Trim() == "" || Row["Column11"].ToString() == "0")
                return;

            DataTable PreDoc = clDoc.ReturnTable(ConAcnt,
                "Select * from Table_065_SanadDetail where Column00=" +
                Row["Column11"].ToString() + " and (Column16=19) and Column17=" + Row["ColumnId"].ToString());
            if (PreDoc.Rows.Count > 0)
            {

                string CommandTxt = string.Empty;
                CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
                //Header
                // ReturnDocNum = clDoc.LastDocNum() + 1;
                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1  INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @Key=SCOPE_IDENTITY()";
                //ReturnDocId = clDoc.ExportDoc_Header(ReturnDocNum, ReturnDate, "فاکتور مرجوعی", Class_BasicOperation._UserName);

                //Detail
                foreach (DataRow item in PreDoc.Rows)
                {
                    string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());

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
                                ' مرجوعی" + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Int16.Parse(item["Column15"].ToString()) + @",
                                " + short.Parse((item["Column16"].ToString() == "12" ? 13 : 20).ToString()) + @",
                                " + ReturnId + @",
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";




                    //clDoc.ExportDoc_Detail(ReturnDocId, item["Column01"].ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                    //, (item["Column07"].ToString().Trim() == "" ? "NULL" : item["Column07"].ToString()), (item["Column08"].ToString().Trim() == "" ? "NULL" : item["Column08"].ToString()),
                    //(item["Column09"].ToString().Trim() == "" ? "NULL" : item["Column09"].ToString()), "مرجوعی-" + item["Column10"].ToString().Trim(), Convert.ToInt64(item["Column12"].ToString()),
                    //Convert.ToInt64(item["Column11"].ToString()),
                    //Convert.ToDouble(item["Column14"].ToString()),
                    //Convert.ToDouble(item["Column13"].ToString()),
                    //Int16.Parse(item["Column15"].ToString()),
                    //short.Parse((item["Column16"].ToString() == "12" ? 13 : 20).ToString()), ReturnId, Class_BasicOperation._UserName,
                    //Convert.ToDouble(item["Column22"].ToString()), (short?)null);
                }
                //درج شماره سند در فاکتور مرجوعی
                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column11", "ColumnId", ReturnId, ReturnDocId);
                CommandTxt += " Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column11=@Key where ColumnId=" + ReturnId;

                //درج شماره سند در حواله انبار
                //clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column07", "ColumnId", DraftID, ReturnDocId);
                CommandTxt += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@Key where ColumnId=" + DraftID;


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

        private void Frm_003_FaktorKharid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control && bt_New.Enabled)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Search.Select();
                txt_Search.SelectAll();
            }
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            //else if (e.Control && e.KeyCode == Keys.E)
            //    bt_ExportDoc_Click(sender, e);
            //else if (e.Control && e.KeyCode == Keys.L)
            //    bt_DelDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)

                toolStripButton7.ShowDropDown();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                //{
                    PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
                    PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                    PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                    PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;
                    bt_Save_Click(sender, e);
                   PSALE. _04_Buy.Reports.Form_BuyFactorPrint frm = new  PSALE. _04_Buy.Reports.Form_BuyFactorPrint
                        (int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()));
                    frm.ShowDialog();
                //}
                //else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
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
                    string user = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull((select column05 from Table_015_BuyFactor where column01 =" + txt_Search.Text + "),0) ");
                    this.table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    //if (dataSet_01_Sale.Table_015_BuyFactor.GetChanges() != null || dataSet_01_Sale.Table_016_Child1_BuyFactor.GetChanges() != null || dataSet_01_Sale.Table_017_Child2_BuyFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}
                    if (isadmin)
                    {
                        dataSet_01_Sale.EnforceConstraints = false;
                        int ReturnedID = ReturnIDNumber(int.Parse(txt_Search.Text.Trim()));
                        this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_01_Sale.Table_015_BuyFactor, ReturnedID);
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_01_Sale.Table_016_Child1_BuyFactor, ReturnedID);
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_017_Child2_BuyFactor, ReturnedID);
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Resid"].Clear();
                        ResidAdapter.Fill(DS, "Resid");
                        dataSet_01_Sale.EnforceConstraints = true;
                        txt_Search.SelectAll();
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);
                    }
                    else if (user == Class_BasicOperation._UserName)
                    {
                        dataSet_01_Sale.EnforceConstraints = false;
                        int ReturnedID = ReturnIDNumber(int.Parse(txt_Search.Text.Trim()));
                        this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_01_Sale.Table_015_BuyFactor, ReturnedID);
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_01_Sale.Table_016_Child1_BuyFactor, ReturnedID);
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_017_Child2_BuyFactor, ReturnedID);
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Resid"].Clear();
                        ResidAdapter.Fill(DS, "Resid");
                        dataSet_01_Sale.EnforceConstraints = true;
                        txt_Search.SelectAll();
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    txt_Search.SelectAll();
                }
            }
        }

        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                Con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_015_BuyFactor where column01=" + FactorNum, Con);
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
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 79))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_012_ViewBuyFactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PSALE._04_Buy.Frm_012_ViewBuyFactors frm = new PSALE._04_Buy.Frm_012_ViewBuyFactors();
                try
                {
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Frm_003_FaktorKharid_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_015_BuyFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString()));
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
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
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewResids_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;

            if (gridEX1.GetRow().Cells["Column10"].Text.Trim() == "0" || gridEX1.GetRow().Cells["Column10"].Text.Trim() == "")
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 22))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form04_ViewWareReceipt")
                        {
                            item.BringToFront();
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt();
                    try
                    {
                        frm.MdiParent = this.MdiParent;
                    }
                    catch { }
                    frm.Show();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            else
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form03_WareReceipt")
                        {
                            item.BringToFront();
                            ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).txt_Search.Text = gridEX1.GetRow().Cells["Column10"].Text;
                            ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(gridEX1.GetValue("Column10").ToString()));
                    frm.ShowDialog();
                    int Buyid = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, Buyid);
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, Buyid);
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, Buyid);
                    dataSet_01_Sale.EnforceConstraints = true;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 47))
            {
                PSALE._02_BasicInfo.Frm_003_TakhfifEzafeBuy ob = new PSALE._02_BasicInfo.Frm_003_TakhfifEzafeBuy(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 48));
                ob.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
                DS.Tables["Discount"].Rows.Clear();
                Adapter.Fill(DS, "Discount");
                //gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_GoodInformation_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
            {
                PSALE._02_BasicInfo.Frm_009_AdditionalGoodsInfo ob = new PSALE._02_BasicInfo.Frm_009_AdditionalGoodsInfo(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));
                ob.ShowDialog();
                GoodbindingSource.DataSource = clGood.GoodInfo();
                DataTable Table = clGood.GoodInfo();
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(Table, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(Table, "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Customers_Click(object sender, EventArgs e)
        {
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();
                gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase, "Select * from ListPeople(5)"), "");
                gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase, "Select * from PeopleScope(8,4)"), "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column05")
                {
                    gridEX1.EnterKeyBehavior = EnterKeyBehavior.None;
                    gridEX_List.Select();
                    gridEX_List.Focus();
                }
                else gridEX1.EnterKeyBehavior = EnterKeyBehavior.NextCell;

            }
            catch
            {
            }
        }

        private void gridEX1_RowEditCanceled(object sender, RowActionEventArgs e)
        {
            gridEX1.Enabled = false;
            bt_New.Enabled = true;
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

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column03")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }


            try
            {
                if (e.Column.Key == "Column27")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column27", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column29")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column29", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;

            gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());

            if (e.Column.Key == "column15")
            {
                if (gridEX1.GetValue("Column15").ToString() == "True")
                {
                    gridEX1.RootTable.Columns["Column25"].Selectable = true;
                    gridEX1.RootTable.Columns["Column26"].Selectable = true;
                }
                else
                {
                    gridEX1.RootTable.Columns["Column25"].Selectable = false;
                    gridEX1.RootTable.Columns["Column26"].Selectable = false;
                }
            }
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
                txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX_List_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Update TotalTextbox
                txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 78))
                        throw new Exception("کاربر گرامی شما امکان ابطال فاکتور خرید را ندارید");
                    if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                    {
                        throw new Exception("فاکتور تائید شده است امکان ابطال وجود ندارد");

                    }


                    if (clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column10", RowID) != 0)
                        throw new Exception("به علت صدور رسید برای این فاکتور، ابطال فاکتور امکانپذیر نیست");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "False")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", int.Parse(RowID), 1);

                        }
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                dataSet_01_Sale.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void mnu_CancelCancel_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 78))
                        throw new Exception("کاربر گرامی شما امکان لغو ابطال فاکتور خرید را ندارید");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "True")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", int.Parse(RowID), 0);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_01_Sale.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                dataSet_01_Sale.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            try
            {
                if (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0" &&
                    ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString() == "0")
                    if (e.Column.Key == "column08" || e.Column.Key == "column09" || e.Column.Key == "column10" || e.Column.Key == "column11" ||
                        e.Column.Key == "column16" || e.Column.Key == "column18" || e.Column.Key == "column17")
                        e.Cancel = false;
                    else
                        e.Cancel = true;

            }
            catch
            {
            }
        }

        private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
        {
            int ReturnFactorID = 0;
            if (this.table_015_BuyFactorBindingSource.Count > 0)
                ReturnFactorID = (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column18"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column18"].ToString()));
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 30))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_014_ReturnFactor")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = ReturnFactorID.ToString();
                        PSALE._04_Buy.Frm_014_ReturnFactor frms = (PSALE._04_Buy.Frm_014_ReturnFactor)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PSALE._04_Buy.Frm_014_ReturnFactor frm = new PSALE._04_Buy.Frm_014_ReturnFactor(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 31), int.Parse(ReturnFactorID.ToString()));
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

        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                try
                {

                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column21");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column22");
                }
                catch { }


                if (e.Column.Key == "column02")
                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode")
                {
                    GoodbindingSource.Filter = "GoodID=" + gridEX_List.GetValue("column02").ToString();
                    gridEX_List.SetValue("tedaddarkartoon", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_List.SetValue("tedaddarbaste", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    gridEX_List.SetValue("column03", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    gridEX_List.SetValue("column10", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BuyPrice"].ToString());
                    gridEX_List.SetValue("column09", 0);
                    gridEX_List.SetValue("column08", 0);
                    if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                    {
                        gridEX_List.SetValue("column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                        gridEX_List.SetValue("column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                    }
                    else
                    {
                        double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                        gridEX_List.SetValue("column16", array[0]);
                        gridEX_List.SetValue("column18", array[1]);
                    }
                    gridEX_List.SetValue("Column34", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                }

                else if (e.Column.Key == "Column13")
                {
                    object Value = gridEX_List.GetValue("Column13");
                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column13"].DropDown.FindItem(Value);
                    gridEX_List.SetValue("Column14", Row["Column02"]);
                }

                if (gridEX_List.GetRow().Cells["Column13"].Text.Trim() == "" && gridEX_List.GetRow().Cells["Column14"].Text.Trim() == "")
                {
                    if (gridEX1.GetRow().Cells["Column25"].Text.Trim() != "" &&
                          gridEX1.GetRow().Cells["Column26"].Text.Trim() != "")
                    {
                        gridEX_List.SetValue("Column13", gridEX1.GetValue("Column25").ToString());
                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column26").ToString());
                    }
                }

                gridEX_List.SetValue("column07",
                 (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon")))
               + (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
               +Convert.ToDouble(gridEX_List.GetValue("column06")));

                gridEX_List.SetValue("Column35", Convert.ToDouble(gridEX_List.GetValue("Column34")) * Convert.ToDouble(gridEX_List.GetValue("column07")));

                //محاسبه معمولی بر اساس کارتن، بسته و جز
                if (!mnu_CalculatePrice.Checked)
                {
                    double TotalPrice = (gridEX1.GetValue("Column15").ToString().Trim() == "True" ?
                            (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("column08")))
                        + (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column06")) * Convert.ToDouble(gridEX_List.GetValue("column10")))
                        : Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("column08")))
                        + (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column06")) * Convert.ToDouble(gridEX_List.GetValue("column10")))));

                    gridEX_List.SetValue("column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column31").ToString()) / 100);
                }
                else
                {
                    double TotalPrice = (gridEX1.GetValue("Column15").ToString().Trim() == "True" ?

                       (Convert.ToDouble(gridEX_List.GetValue("column07")) * Convert.ToDouble(gridEX_List.GetValue("column10")))
                       : Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column07")) * Convert.ToDouble(gridEX_List.GetValue("column10")))));

                    gridEX_List.SetValue("column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column31").ToString()) / 100);
                }

                if (e.Column.Key == "column17")
                    gridEX_List.SetValue("Column16", 0);

                if (e.Column.Key == "column16")
                {
                    if (gridEX1.GetValue("Column15").ToString() == "True")
                        gridEX_List.SetValue("column17", Convert.ToDouble(gridEX_List.GetValue("column11")) * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else
                        gridEX_List.SetValue("column17", Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                }

                if (gridEX1.GetValue("Column15").ToString() == "True")
                {
                    Double jam, takhfif = 0, ezafe;
                    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else takhfif = Convert.ToDouble(gridEX_List.GetValue("Column17").ToString());
                    if (Properties.Settings.Default.ExtraMethod)

                        ezafe = (jam - takhfif) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100;
                    else
                        ezafe = (jam) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100;

                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }
                else
                {
                    Int64 jam, takhfif = 0, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) *
                            Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                    else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                    if (Properties.Settings.Default.ExtraMethod)
                        ezafe = Convert.ToInt64((Convert.ToDouble(jam)
                            - Convert.ToDouble(takhfif)) *
                            Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                    else
                        ezafe = Convert.ToInt64((Convert.ToDouble(jam)) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);


                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }
            }
            catch
            {
            }
        }

        private void bt_DefineSignatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                PSALE._04_Buy.Frm_018_Buy_Signatures frm = new PSALE._04_Buy.Frm_018_Buy_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {

                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column03");

            }
            catch { }
            try
            {

                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column27");

            }
            catch { }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column29");
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column25")
                {
                    object Value = gridEX1.GetValue("Column25");
                    DataRowView Row = (DataRowView)this.gridEX1.RootTable.Columns["Column25"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column26", Row["Column02"]);
                    gridEX_List.RootTable.Columns["Column13"].DefaultValue = gridEX1.GetValue("Column25");
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column26");
                }
                else if (e.Column.Key == "Column26")
                {
                    gridEX_List.RootTable.Columns["Column13"].DefaultValue = gridEX1.GetValue("Column25");
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column26");
                }

            }
            catch
            {
            }
        }

        private void mnu_DelReceipt_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    int RowID = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int ReceiptId = clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column10", RowID.ToString());

                    if (ReceiptId != 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 128))
                        {

                            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;
                            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

                            PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt
                            (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21), -1);
                            frm.txt_Search.Text = clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01", "ColumnId", ReceiptId.ToString());
                            frm.bt_Search_Click(sender, e);
                            frm.bt_Del_Click(sender, e);



                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                    }
                    DS.Tables["Doc"].Clear();
                    DocAdapter.Fill(DS, "Doc");
                    DS.Tables["Resid"].Clear();
                    ResidAdapter.Fill(DS, "Resid");
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_01_Sale.Table_015_BuyFactor, RowID);
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_01_Sale.Table_016_Child1_BuyFactor, RowID);
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_01_Sale.Table_017_Child2_BuyFactor, RowID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_NotConfirmReceipt_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                int ReceiptId = clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column10", ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

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
            }
        }

        private void gridEX_List_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");
            }
            catch { }
        }

        private void bt_ViewRequests_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;


            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 79))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewWareReceipt")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._07_RequestBuy.Form02_ViewRequests frm = new PWHRS._07_RequestBuy.Form02_ViewRequests();
                try
                {
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void gridEX_List_RowDoubleClick(object sender, RowActionEventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 38))
                {
                    PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                    PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                    PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
                    PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                    PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                    PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;

                    PWHRS._05_Gozareshat.Frm_010_KardexRiyali frms = new PWHRS._05_Gozareshat.Frm_010_KardexRiyali(
                        int.Parse(gridEX_List.GetValue("Column02").ToString()),
                        DateTime.Now.AddMonths(-1),
                        DateTime.Now);
                    try
                    {
                        frms.MdiParent = Frm_Main.ActiveForm;
                    }
                    catch { }
                    frms.Show();

                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            catch
            {
            }
        }

        private void gridEX_List_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (gridEX_List.GetValue("Column02").ToString() != "")
                {
                    string Txt = "";
                    DataTable Table = clDoc.GoodRemain(gridEX_List.GetValue("Column02").ToString(), gridEX1.GetValue("Column02").ToString());
                    foreach (DataRow item in Table.Rows)
                    {
                        Txt += " انبار:" + item["WareName"].ToString() + " " + Convert.ToDouble(item["Remain"].ToString()).ToString("#,##0.###")
                            + Environment.NewLine;
                    }
                    Txt += " آخرین قیمت خرید:" + LastBuyGoodPrice(int.Parse(gridEX_List.GetValue("Column02").ToString())).ToString("#,##0.###") + Environment.NewLine;
                    Txt += " آخرین قیمت خرید براساس تامین کننده:" + LastBuyGoodPrice(int.Parse(gridEX_List.GetValue("Column02").ToString()), int.Parse(gridEX1.GetValue("column03").ToString())).ToString("#,##0.###") + Environment.NewLine;

                    if (Txt.Trim() != "")
                        ToastNotification.Show(this, Txt, 3000, eToastPosition.MiddleCenter);
                }
            }
            catch
            {

            }
        }

        private Decimal LastBuyGoodPrice(int GoodCode)
        {
            DataTable Table = clDoc.ReturnTable(ConSale, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_015_BuyFactor.column02) AS Date,1
            FROM         Table_016_Child1_BuyFactor INNER JOIN
            Table_015_BuyFactor ON Table_016_Child1_BuyFactor.column01 = Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY Table_016_Child1_BuyFactor.column02
            order by Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 int , gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_015_BuyFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            if (Table.Rows.Count == 0)
                return 0;
            else
                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

        }

        private Decimal LastBuyGoodPrice(int GoodCode, int provider)
        {
            DataTable Table = clDoc.ReturnTable(ConSale, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_015_BuyFactor.column02) AS Date,1
            FROM         Table_016_Child1_BuyFactor INNER JOIN
            Table_015_BuyFactor ON Table_016_Child1_BuyFactor.column01 = Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @" AND Table_015_BuyFactor.column03=" + provider + @"
            GROUP BY Table_016_Child1_BuyFactor.column02
            order by Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 int , gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_015_BuyFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @" AND Table_015_BuyFactor.column03=" + provider + @"
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            if (Table.Rows.Count == 0)
                return 0;
            else
                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_015_BuyFactorBindingSource.EndEdit();
                this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                //if (dataSet_01_Sale.Table_015_BuyFactor.GetChanges() != null || dataSet_01_Sale.Table_016_Child1_BuyFactor.GetChanges() != null ||
                //    dataSet_01_Sale.Table_017_Child2_BuyFactor.GetChanges() != null)
                //{
                //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //    {
                //        SaveEvent(sender, e);
                //    }
                //}

                if (isadmin)
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_015_BuyFactor),0) as Row");
                }
                else
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_015_BuyFactor where column05='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    //if (dataSet_01_Sale.Table_015_BuyFactor.GetChanges() != null || dataSet_01_Sale.Table_016_Child1_BuyFactor.GetChanges() != null ||
                    //    dataSet_01_Sale.Table_017_Child2_BuyFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        SaveEvent(sender, e);
                    //    }
                    //}

                    if (isadmin)
                    {
                        Table = clDoc.ReturnTable(ConSale,
                           "Select ISNULL((Select max(Column01) from Table_015_BuyFactor where Column01<" +
                           ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = clDoc.ReturnTable(ConSale,
                           "Select ISNULL((Select max(Column01) from Table_015_BuyFactor where Column01<" +
                           ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND column05='" + Class_BasicOperation._UserName + "' ),0) as Row");
                    }
                     if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

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
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    //if (dataSet_01_Sale.Table_015_BuyFactor.GetChanges() != null || dataSet_01_Sale.Table_016_Child1_BuyFactor.GetChanges() != null ||
                    //    dataSet_01_Sale.Table_017_Child2_BuyFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        SaveEvent(sender, e);
                    //    }
                    //}


                    if (isadmin)
                    {
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_015_BuyFactor where Column01>" + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    }

                    else
                    {
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_015_BuyFactor where Column01>" + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND column05='" + Class_BasicOperation._UserName + "'),0) as Row");

                    }


                         if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_01_Sale.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_015_BuyFactorBindingSource.EndEdit();
                this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                //if (dataSet_01_Sale.Table_015_BuyFactor.GetChanges() != null || dataSet_01_Sale.Table_016_Child1_BuyFactor.GetChanges() != null ||
                //    dataSet_01_Sale.Table_017_Child2_BuyFactor.GetChanges() != null)
                //{
                //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //    {
                //        SaveEvent(sender, e);
                //    }
                //}

                if (isadmin)
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_015_BuyFactor),0) as Row");
                }

                else
                {
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_015_BuyFactor where column05='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
                  if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_01_Sale.EnforceConstraints = true;
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch
            {
            }
        }

        private void mnu_CalculatePrice_Click(object sender, EventArgs e)
        {
            if (mnu_CalculatePrice.Checked)
                mnu_CalculatePrice.CheckState = CheckState.Unchecked;
            else
                mnu_CalculatePrice.CheckState = CheckState.Checked;

            Properties.Settings.Default.BuyFactorPriceCompute = mnu_CalculatePrice.Checked;
        }

        private void bt_Attachments_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;
                if (Convert.ToInt32(Row["ColumnId"]) > 0)
                // if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 95))
                {
                    try
                    {
                        PSALE._04_Buy.Form03_BuyFactorAppendix frm = new PSALE._04_Buy.Form03_BuyFactorAppendix(
                            int.Parse(Row["ColumnId"].ToString()),
                            int.Parse(Row["Column01"].ToString()));
                        frm.ShowDialog();
                    }
                    catch
                    {
                    }
                }

                else Class_BasicOperation.ShowMsg("", "ابتدا فاکتور را ذخیره کنید", "None");
            }
        }

        private void تعریفپروژهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 3))
            {
                PACNT._1_BasicMenu.Form02_Projects frm = new PACNT._1_BasicMenu.Form02_Projects(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 4));
                frm.ShowDialog();
                DataTable dt = new DataTable();
                SqlDataAdapter ProjectAdapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
                ProjectAdapter.Fill(dt);
                gridEX1.DropDowns["project"].SetDataBinding(dt, "");
                gridEX_List.DropDowns["Project"].SetDataBinding(dt, "");


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            try
            {


                txt_TotalPrice.Value = Convert.ToDouble(
           gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
           AggregateFunction.Sum).ToString());

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                      Convert.ToDouble(txt_Reductions.Value.ToString());

            }
            catch
            {
            }

        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {

        }

        private void txt_TotalPrice_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_TotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Comfirm_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {

                if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column17"])
                    || Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column19"])
                    )
                {
                    Class_BasicOperation.ShowMsg("", "فاکتور باطل/مرجوع شده است امکان تائید وجود ندارد", "Stop");
                    return;
                }
                string command = string.Empty;
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 237))
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما دسترسی تائید / لغو تائید را ندارید", "Stop");
                    return;
                }
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو تائید فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

                        command = "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column31=0,Column32='" + Class_BasicOperation._UserName + "',Column33=getdate() where columnid=" + RowID;


                }
                else
                {

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به تائید فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

                        command = "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column31=1,Column32='" + Class_BasicOperation._UserName + "',Column33=getdate() where columnid=" + RowID;
                }




                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
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
                        Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد", "Information");
                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_01_Sale.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_01_Sale.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        table_015_BuyFactorBindingSource_PositionChanged(sender, e);


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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از کپی کردن این فاکتور مطمئن هستید؟",
                    "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes &&
                    gridEX_List.RowCount > 0)
                {
                    SaveEvent(this, e);

                    //درج هدر فاکتور  

                    SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                    Key.Direction = ParameterDirection.Output;
                    int FactorNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_015_BuyFactor", "column01");

                    string _CopyCmd = @"INSERT INTO [Table_015_BuyFactor]   ([column01]
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
                                                                       ,[Column20]
                                                                       ,[Column21]
                                                                       ,[Column22]
                                                                       ,[Column23]
                                                                       ,[Column24]
                                                                       ,[Column25]
                                                                       ,[Column26]
                                                                       ,[Column27]
                                                                       ,[Column28]
                                                                       ,[Column29]
                                                                       ,[Column30]
                                                                       ,[Column31]
                                                                       ,[Column32]
                                                                       ,[Column33]) VALUES(" +
                    FactorNum.ToString() + ", '" + gridEX1.GetValue("column02").ToString() + "', " +
                    gridEX1.GetValue("column03").ToString() + ", " +
                    (gridEX1.GetRow().Cells["Column04"].Text.Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column04").ToString() + "'") + ",'" +
                   Class_BasicOperation._UserName + "',getdate(), '" + Class_BasicOperation._UserName + @"',getdate(), 0, 0, 0, " +
                    (gridEX1.GetValue("column12").ToString().Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column12").ToString() + "'") + "," +
                    (gridEX1.GetValue("column13").ToString().Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column13").ToString() + "'") + "," +
                    (gridEX1.GetValue("column14").ToString().Trim() == "" ? "NULL" : gridEX1.GetValue("column14")) + "," +
                    (gridEX1.GetValue("column15").ToString() == "True" ? 1 : 0) + ",  0, 0, 0, 0, " + gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                    AggregateFunction.Sum).ToString() + @", " + txt_Extra.Value.ToString() + @"," + txt_Reductions.Value.ToString() + "," + gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column19"],
                    AggregateFunction.Sum).ToString() + @", " + gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column17"],
                    AggregateFunction.Sum).ToString() + @", " + (gridEX1.GetRow().Cells["Column25"].Text.Trim() == "" ? "NULL" : gridEX1.GetValue("Column25").ToString()) + @", " + gridEX1.GetValue("Column26").ToString() + @", " +
                    gridEX1.GetValue("Column27").ToString() + "," +
                    gridEX1.GetValue("Column28").ToString() + "," +
                     (gridEX1.GetValue("Column29").ToString().Trim() == "" ? "Null" : gridEX1.GetValue("Column29").ToString()) + "," +
                    (gridEX1.GetValue("Column30").ToString().Trim() == "" ? "Null" : gridEX1.GetValue("Column30").ToString()) + ",0,null,null ); SET @Key=SCOPE_IDENTITY()";
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                    {
                        Con.Open();
                        SqlCommand InsertHeader = new SqlCommand(_CopyCmd, Con);

                        InsertHeader.Parameters.Add(Key);
                        InsertHeader.ExecuteNonQuery();
                        int FactorId = int.Parse(Key.Value.ToString());

                        //درج کالاهای فاکتور  
                        for (int ij = 0; ij < gridEX_List.RowCount; ij++)
                        {
                            gridEX_List.Row = ij;

                            _CopyCmd = @"INSERT INTO [Table_016_Child1_BuyFactor]
                                           ([column01]
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
                                           ,[Column27]
                                           ,[Column28]
                                           ,[Column29]
                                           ,[Column30]
                                           ,[Column31]
                                           ,[Column32]
                                           ,[Column33]
                                           ,[Column34]
                                           ,[Column35]
                                           ,[Column36]
                                           ,[Column37])VALUES(" +
                            FactorId.ToString() + ", " +
                            gridEX_List.GetValue("column02").ToString() + ", " +
                            gridEX_List.GetValue("column03").ToString() + ", " +
                            gridEX_List.GetValue("column04").ToString() + ", " +
                            gridEX_List.GetValue("column05").ToString() + ", " +
                            gridEX_List.GetValue("column06").ToString() + ", " +
                            gridEX_List.GetValue("column07").ToString() + ", " +
                            gridEX_List.GetValue("column08").ToString() + ", " +
                            gridEX_List.GetValue("column09").ToString() + ", " +
                            gridEX_List.GetValue("column10").ToString() + ", " +
                            gridEX_List.GetValue("column11").ToString() + ", 0," + (gridEX_List.GetValue("Column13").ToString().Trim() == "" ? "0" : gridEX_List.GetValue("Column13").ToString()) + ",0,0," +
                            gridEX_List.GetValue("column16").ToString() + ", " +
                            gridEX_List.GetValue("column17").ToString() + ", " +
                            gridEX_List.GetValue("column18").ToString() + ", " +
                            gridEX_List.GetValue("column19").ToString() + ", " +
                            gridEX_List.GetValue("column20").ToString() + ", " +
                            (gridEX_List.GetRow().Cells["column21"].Text.Trim() == "" ? "NULL" : gridEX_List.GetValue("column21").ToString()) + ", " +
                            (gridEX_List.GetRow().Cells["column22"].Text.Trim() == "" ? "NULL" : gridEX_List.GetValue("column22").ToString()) + ", " +
                            (gridEX_List.GetRow().Cells["column23"].Text.Trim() == "" ? "NULL" : "N'" + gridEX_List.GetValue("column23").ToString() + "'") + ", " +
                            "  0, 0, 0, 0, 0," +
                            gridEX_List.GetValue("Tedaddarkartoon").ToString() + "," +
                            gridEX_List.GetValue("Tedaddarbaste").ToString() + "," +
                            gridEX_List.GetValue("Column31").ToString() + "," +
                            (gridEX_List.GetRow().Cells["Column32"].Text.Trim() == "" ? "NULL" : "N'" + gridEX_List.GetValue("Column32").ToString() + "'") + "," +
                            (gridEX_List.GetRow().Cells["Column33"].Text.Trim() == "" ? "NULL" : "N'" + gridEX_List.GetValue("Column33").ToString() + "'") + "," +
                            gridEX_List.GetValue("Column34").ToString() + "," + gridEX_List.GetValue("Column35").ToString() + "," +
                            (gridEX_List.GetRow().Cells["Column36"].Text.Trim() == "" ? "NULL" : "N'" + gridEX_List.GetValue("Column36").ToString() + "'") + "," +
                            (gridEX_List.GetRow().Cells["Column37"].Text.Trim() == "" ? "NULL" : "N'" + gridEX_List.GetValue("Column37").ToString() + "'") + ")";



                            SqlCommand InsertChild1 = new SqlCommand(_CopyCmd, Con);

                            InsertChild1.ExecuteNonQuery();
                        }

                        gridEX_List.MoveFirst();


                        //درج اضافات و کسورات فاکتور فروش
                        for (int ik = 0; ik < gridEX_Extra.RowCount; ik++)
                        {
                            gridEX_Extra.Row = ik;

                            _CopyCmd = @"INSERT INTO [Table_017_Child2_BuyFactor]
                                               ([column01]
                                               ,[column02]
                                               ,[column03]
                                               ,[column04]
                                               ,[column05]
                                               ,[column06]) VALUES(" +
                            FactorId.ToString() + ", " +
                            gridEX_Extra.GetValue("column02").ToString() + ", " +
                            gridEX_Extra.GetValue("column03").ToString() + ", " +
                            gridEX_Extra.GetValue("column04").ToString() + ", '" +
                            gridEX_Extra.GetValue("column05").ToString() + "', '" +
                            gridEX_Extra.GetValue("column06").ToString() + "')";

                            SqlCommand InsertChild2 = new SqlCommand(_CopyCmd, Con);

                            InsertChild2.ExecuteNonQuery();
                        }

                        gridEX_Extra.MoveFirst();


                        MessageBox.Show("فاکتور شماره " + FactorNum.ToString() + " صادر شد");
                    }
                }

            }
            catch
            {
            }
        }

        private void gridEX_Extra_Leave(object sender, EventArgs e)
        {

            try
            {
                gridEX_Extra.UpdateData();
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
                table_015_BuyFactorBindingSource.EndEdit();
            }
            catch
            {
            }
        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

       

       

       

       

      

     




    }

}