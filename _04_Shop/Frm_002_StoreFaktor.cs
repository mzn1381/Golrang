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
    public partial class Frm_002_StoreFaktor : Form
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
        // SqlDataAdapter DraftAdapter, DocAdapter, ReturnAdapter;
        string ReturnDate = null;
        InputLanguage original;
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable waredt = new DataTable();
        DataTable Sanaddt = new DataTable();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();

        int LastDocnum = 0;
        SqlParameter ReturnDocNum;

        public Frm_002_StoreFaktor(bool del)
        {
            _del = del;
            InitializeComponent();
        }
        public Frm_002_StoreFaktor(bool del, int ID)
        {
            _del = del;
            _ID = ID;

            InitializeComponent();
        }
        private void Frm_002_StoreFaktor_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
           
            ToastNotification.ToastBackColor = Color.Aquamarine;
            ToastNotification.ToastForeColor = Color.Black;
            ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
            ReturnDocNum.Direction = ParameterDirection.Output;
            mlt_Draft.DataSource = clDoc.ReturnTable(ConWare, @"select Columnid,Column01 from Table_007_PwhrsDraft");
            mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead");

            //GoodbindingSource.DataSource = clGood.MahsoolInfo(0);
            //DataTable GoodTable = clGood.MahsoolInfo(0);
            //gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            //gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            //gridEX_List.DropDowns["GoodCode"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId as GoodId,Column01 as GoodCode,Column02 as GoodName,Column07 as CountUnit from table_004_CommodityAndIngredients");
            //gridEX_List.DropDowns["GoodName"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId as GoodId,Column01 as GoodCode,Column02 as GoodName,Column07 as CountUnit from table_004_CommodityAndIngredients");
            DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                 @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
            mlt_Ware.DataSource = WareTable;

            DataTable dt = clDoc.ReturnTable(ConWare,string.Format( @"SELECT 
GoodsInformation.Columnid AS GoodID,
GoodsInformation.Column01 AS GoodCode,
GoodsInformation.Column02 AS GoodName,
GoodsInformation.Column03 AS MainGroup, 
GoodsInformation.column07 AS CountUnit,
GoodsInformation.Column22 as Weight,
GoodsInformation.Column04 AS SubGroup,
CASE WHEN  dbo.table_006_CommodityChanges.Column07 IS NULL 
            THEN  GoodsInformation.column09 ELSE  dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
            CASE WHEN  dbo.table_006_CommodityChanges.Column06 IS NULL 
            THEN  GoodsInformation.column08 ELSE  dbo.table_006_CommodityChanges.Column06 END AS NumberInPack,
                   CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
            THEN  GoodsInformation.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
            CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
            THEN  GoodsInformation.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
            ISNULL( dbo.table_006_CommodityChanges.Column18, 1) AS Active1,

 CASE WHEN TS003.Column03 IS NULL 
            THEN  GoodsInformation.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
            THEN  GoodsInformation.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
            THEN  GoodsInformation.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
            THEN  GoodsInformation.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
            THEN  GoodsInformation.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
            THEN  GoodsInformation.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
            THEN  GoodsInformation.Column38 ELSE ts003.column06 END AS Extra,
ISNULL(TS003.Column11, 1) AS Active2, 
             dbo.table_003_SubsidiaryGroup.column03 as SubGroupName,
               dbo.table_002_MainGroup.column02 as MainGroupName, 
            Goodsinformation.column29 AS Khas
            , WareRemain
 FROM table_004_CommodityAndIngredients GoodsInformation
LEFT JOIN dbo.Table_003_InformationProductCash AS TS003 ON  GoodsInformation.columnid = TS003.column01
LEFT JOIN  dbo.table_006_CommodityChanges ON   GoodsInformation.columnid =  dbo.table_006_CommodityChanges.column01
left join dbo.table_003_SubsidiaryGroup ON GoodsInformation.Column03 =  dbo.table_003_SubsidiaryGroup.columnid 
LEFT  JOIN dbo.table_002_MainGroup ON  dbo.table_003_SubsidiaryGroup.column01 =  dbo.table_002_MainGroup.columnid
LEFT JOIN 
	(SELECT SUM(R)-SUM(D) wareremain,Column02 FROM 
	
	(
SELECT tcpr.Column02 ,tcpr.Column07 R,0 D FROM Table_012_Child_PwhrsReceipt tcpr 
LEFT JOIN Table_011_PwhrsReceipt tpr ON tpr.columnid = tcpr.column01 
WHERE tpr.Column02<='{0}'  AND tpr.column03={1}

union
SELECT tcpr.Column02,0 AS R,tcpr.Column07 D FROM Table_008_Child_PwhrsDraft tcpr 
LEFT JOIN Table_007_PwhrsDraft  tpr ON tpr.columnid = tcpr.column01 
WHERE  tpr.Column02<='{0}'  AND tpr.column03={1}

	) AS t1 
	 GROUP BY t1.Column02) AS T  on t.Column02=goodsinformation.columnid
	

 WHERE ( Goodsinformation.column19 = 1) and     ISNULL( dbo.table_006_CommodityChanges.Column18, 1)=1 
 AND (ISNULL(TS003.Column11, 1) = 1) AND (GoodsInformation.Column28 = 1)
",txt_date.Text=="" ? FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD") : txt_date.Text ,mlt_Ware.Value !=null && mlt_Ware.Value!="" ? mlt_Ware.Value.ToString() : "0" ));
            
            
            gridEX_List.DropDowns["GoodCode"].DataSource = dt;
            gridEX_List.DropDowns["GoodName"].DataSource = dt;

            mlt_Function.DataSource = clDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
          



            DataTable PersonTable = clDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");
            DataTable CustomerTable = clDoc.ReturnTable
          (ConBase, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            mlt_Customer.DataSource = PersonTable;


            Adapter = new SqlDataAdapter("SELECT  [Column00] AS countiD, Column01 AS countName FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            gridEX_List.DropDowns["Brand"].DataSource = clDoc.ReturnTable(ConPCLOR, @"select Id,TypeColor from Table_010_TypeColor");

            gridEX_List.DropDowns["Tamin"].DataSource = clDoc.ReturnTable(ConPCLOR, @"select Id,namemachine from Table_60_SpecsTechnical");


            //mlt_SaleType.DataSource = clDoc.ReturnTable(ConBase, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");

            if (_ID != 0)
            {
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, _ID);
                table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                
                

            }
            try
            {
                using (SqlConnection ConWare1 = new SqlConnection(Properties.Settings.Default.PWHRS))
                {
                    ConWare1.Open();
                    SqlCommand Update = new SqlCommand(@"UPDATE Table_032_GoodPrice
                                                SET    [Column01] = REPLACE([Column01], NCHAR(1610), NCHAR(1740))

                                                UPDATE Table_032_GoodPrice
                                                SET    [Column01] = REPLACE([Column01], NCHAR(1603), NCHAR(1705))", ConWare1);
                    Update.ExecuteNonQuery();

                }
                using (SqlConnection Conbase1 = new SqlConnection(Properties.Settings.Default.PBASE))
                {
                    Conbase1.Open();
                    SqlCommand Update1 = new SqlCommand(@"UPDATE Table_002_SalesTypes
                                                    SET    [Column02] = REPLACE([Column02], NCHAR(1610), NCHAR(1740))

                                                    UPDATE Table_002_SalesTypes
                                                    SET    [Column02] = REPLACE([Column02], NCHAR(1603), NCHAR(1705))", Conbase1);
                    Update1.ExecuteNonQuery();


                }


            }
            catch
            {
            }
            Adapter = new SqlDataAdapter(
                                                                @"SELECT        isnull(Column02,0) as Column02
                                                                        FROM           Table_030_Setting
                                                                        WHERE        (ColumnId in (45,46)) order by ColumnId  ", ConMain);
            Adapter.Fill(waredt);

            DataTable subg = clDoc.ReturnTable(ConWare, @"SELECT DISTINCT tsg.columnid
                                                                                          ,tsg.column03
                                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                                           JOIN table_003_SubsidiaryGroup tsg
                                                                                                ON  tsg.column01 = tcai.column03
                                                                                                    AND tsg.columnid = tcai.column04
                                                                                    WHERE  tcai.Column51 = 1
                                                                                           AND tcai.column28 = 1
                                                                                           AND tcai.column19 = 1");
            foreach (DataRow dr in subg.Rows)
            {
                Janus.Windows.UI.Tab.UITabPage uit = new Janus.Windows.UI.Tab.UITabPage();
                uit.Location = new System.Drawing.Point(3, 1);
                uit.Name = "uitab" + dr["columnid"].ToString();
                uit.Size = new System.Drawing.Size(115, 373);
                uit.TabStop = true;
                uit.Text = dr["column03"].ToString();
                ////
                Janus.Windows.GridEX.GridEX grid = new Janus.Windows.GridEX.GridEX();


                grid.AllowColumnDrag = false;
                grid.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
                grid.AlternatingColors = true;
                grid.AlternatingRowFormatStyle.BackColor = System.Drawing.Color.MistyRose;
                grid.AlternatingRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
                grid.CardWidth = 751;
                grid.ColumnAutoResize = true;
                grid.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
                grid.ColumnSetHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
                grid.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;



                /// grid.DataSource = this.table_004_CommodityAndIngredientsBindingSource;
                //gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
                //grid.DesignTimeLayout = gridEX1_DesignTimeLayout;
                grid.Dock = System.Windows.Forms.DockStyle.Fill;
                grid.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
                grid.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
                grid.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
                grid.Font = new System.Drawing.Font("B Mitra", 14F);
                grid.GroupByBoxVisible = false;
                grid.ImageList = this.imageList1;
                grid.Location = new System.Drawing.Point(0, 0);
                grid.Margin = new System.Windows.Forms.Padding(6);
                grid.Name = "gridEX" + dr["columnid"].ToString();
                grid.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
                grid.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
                grid.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
                grid.OfficeCustomColor = System.Drawing.Color.SteelBlue;
                grid.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
                grid.SettingsKey = "gridEX" + dr["columnid"].ToString();
                grid.Size = new System.Drawing.Size(115, 373);
                //grid.TabIndex = 4;
                grid.TotalRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
                grid.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
                grid.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
                grid.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
                grid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
                //grid.RootTable = gridEX1.RootTable;

                //foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX3.RootTable.Columns)
                //{
                //    grid.RootTable.Columns.Add(item);
                //}
                grid.DataSource = clDoc.ReturnTable(ConWare, @"SELECT [columnid]
                                                                                                          ,[column01]
                                                                                                          ,[column02]
                                                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                                                            
                                                                                                    WHERE  tcai.Column51 = 1
                                                                                                           AND tcai.column28 = 1
                                                                                                           AND tcai.column19 = 1 and  tcai.column04=" + dr["columnid"]);
                uit.Controls.Add(grid);
                //this.uiTab1.TabPages.Add(uit);
                grid.SelectionChanged += gridEX1_SelectionChanged;
                grid.Enter += gridEX1_Enter;
                grid.Click += gridEX1_Click;
                grid.MouseClick += gridEX1_MouseClick;
            }

            //uiTab1.SelectedIndex = 1;

            //gridEX_List.Enabled = false;
            //uiTab1.Enabled = false;
            //this.WindowState = FormWindowState.Maximized;

        



        }
        private void gridEX1_Click(object sender, EventArgs e)
        {

        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            // gridEX1_SelectionChanged(sender, null);
        }
        private void gridEX1_MouseClick(object sender, MouseEventArgs e)
        {
            Janus.Windows.GridEX.GridEX j = (Janus.Windows.GridEX.GridEX)sender;

            var id = j.GetValue("columnid");
            if (id != null && !string.IsNullOrWhiteSpace(id.ToString()))
                InitialNewRowwithid(Convert.ToInt32(id));
        }
        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {
            //if (e != null)
            //{
            //    Janus.Windows.GridEX.GridEX j = (Janus.Windows.GridEX.GridEX)sender;

            //    var id = j.GetValue("columnid");
            //    if (id != null && !string.IsNullOrWhiteSpace(id.ToString()))
            //        InitialNewRowwithid(Convert.ToInt32(id));
            //}
        }
        private void InitialNewRowwithid(int goodid)
        {
            try
            {
                bool isthere = false;
                if (goodid > 0)
                {

                    Int64 codeid = goodid;
                    double buyprice = 0;
                    int ok = 1;

                    if (ok == 1)
                    {

                        try
                        {
                            
                            table_010_SaleFactorBindingSource.EndEdit();
                        }
                        catch (Exception ex)
                        {

                            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                            return;
                        }
                        if (gridEX_List.GetRows().Count() > 0)
                        {
                            string goodcode;
                            Int16 unit;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    goodcode = (Comm.ExecuteScalar().ToString());
                                    Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    unit = Convert.ToInt16(Comm.ExecuteScalar());
                                }


                                if (Convert.ToInt64(item.Cells["GoodCode"].Value) == codeid && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                {

                                    isthere = true;
                                    item.BeginEdit();

                                    //float h = clDoc.GetZarib(Convert.ToInt32(codeid), Convert.ToInt16(item.Cells["column03"].Value));
                                    //item.Cells["Column07"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column07"].Value);
                                    //item.Cells["Column06"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column06"].Value);


                                    item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                    item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                    item.EndEdit();
                                    double TotalPrice;
                                    if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                        item.BeginEdit();
                                        item.Cells["column11"].Value = TotalPrice;
                                        item.Cells["column16"].Value = 0;
                                        item.Cells["column18"].Value = 0;
                                        item.Cells["column17"].Value = 0;
                                        item.Cells["Column19"].Value = 0;
                                        item.Cells["Column40"].Value = TotalPrice;
                                        item.Cells["Column20"].Value = TotalPrice;

                                        item.EndEdit();

                                    }
                                    gridEX_List.UpdateData();

                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                    break;

                                }

                            }
                            if (!isthere)
                            {

                                gridEX_List.MoveToNewRecord();
                                gridEX_List.SetValue("GoodCode", codeid);
                                gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("column02", codeid);

                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                    buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                }
                                gridEX_List.SetValue("Column41", buyprice);



                                //GoodbindingSource.Filter = "GoodID=" +
                                //        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    //gridEX_List.SetValue("tedaddarkartoon",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    //gridEX_List.SetValue("tedaddarbaste",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    //gridEX_List.SetValue("column03",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    //gridEX_List.SetValue("column16",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                    DataTable dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    double amunt = 0;
                                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    //{
                                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                    //    if (dr.Count() > 0)
                                    //    {
                                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                    //        gridEX_List.SetValue("column10",
                                    //         dr[0].ItemArray[3]);
                                    //    }
                                    //}

                                    if (amunt == Convert.ToDouble(0))
                                    {
                                        gridEX_List.SetValue("column10",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                        "SalePrice"].ToString());

                                    }
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);

                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);
                                        gridEX_List.SetValue("Column40", TotalPrice);
                                        gridEX_List.SetValue("Column20", TotalPrice);
                                    }


                                    gridEX_List.UpdateData();
                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());




                                }



                            }


                        }
                        else
                        {

                            gridEX_List.MoveToNewRecord();
                            gridEX_List.SetValue("GoodCode", codeid);
                            gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("column02", codeid);

                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                            {
                                Con.Open();
                                SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                            }
                            gridEX_List.SetValue("Column41", buyprice);
                            {
                                //GoodbindingSource.Filter = "GoodID=" +
                                //    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    //gridEX_List.SetValue("tedaddarkartoon",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    //gridEX_List.SetValue("tedaddarbaste",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    //gridEX_List.SetValue("column03",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    //gridEX_List.SetValue("column16",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                    DataTable dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    double amunt = 0;
                                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    //{
                                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                    //    if (dr.Count() > 0)
                                    //    {
                                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                    //        gridEX_List.SetValue("column10",
                                    //         dr[0].ItemArray[3]);
                                    //    }
                                    //}

                                    if (amunt == Convert.ToDouble(0))
                                    {
                                        gridEX_List.SetValue("column10",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                        "SalePrice"].ToString());

                                    }
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);
                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);
                                        gridEX_List.SetValue("Column40", TotalPrice);
                                        gridEX_List.SetValue("Column20", TotalPrice);
                                    }

                                    gridEX_List.UpdateData();

                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());
                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                }
                            }
                        }



                    }





                }
            }
            catch (Exception ex)
            {
            }


        }
        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                
                gridEX_List.Enabled = true;
                gridEX_Extra.Enabled = true;
                uiPanel1.Enabled = true;
                gb_factor.Enabled = true;

                dataSet_01_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, 0);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, 0);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, 0);
                dataSet_01_Sale.EnforceConstraints = true;
         
                table_010_SaleFactorBindingSource.AddNew();
                //gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01").ToString());
                txt_date.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                mlt_Function.Value = Properties.Settings.Default.TypePWHRS;

                //try
                //{
                //    if (!string.IsNullOrWhiteSpace(ConWare.ConnectionString))
                //        try
                //        {
                //            mlt_Ware.Value = Convert.ToInt16(Properties.Settings.Default.Ware);
                //        }
                //        catch { mlt_Ware.Value = waredt.Rows[1]["Column02"]; }
                //    else if (waredt.Rows[1]["Column02"] != null && !string.IsNullOrWhiteSpace(waredt.Rows[1]["Column02"].ToString()))
                //        mlt_Ware.Value = waredt.Rows[1]["Column02"];
                //}
                //catch { }
                //GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                //DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                //gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                //gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                //btn_addtax.Enabled = true;
                dt_edittime.Value = Class_BasicOperation.ServerDate();
                dt_inserttime.Value = Class_BasicOperation.ServerDate();
                txt_edituser.Text = Class_BasicOperation._UserName;
                txt_insertuser.Text = Class_BasicOperation._UserName;
                bt_New.Enabled = false;
                bt_Save.Enabled = true;
                bt_Del.Enabled = true;
                gridEX_List.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowEdit = InheritableBoolean.True;
                gridEX_List.AllowDelete = InheritableBoolean.True;

                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.Customer))
                    mlt_Customer.Value = Convert.ToInt32(Properties.Settings.Default.Customer);

                //                using (SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN))
                //                {

                //                    ConMain.Open();
                //                    SqlCommand Command = new SqlCommand(@"
                //                                                            SELECT ISNULL(
                //                                                                       (
                //                                                                           SELECT ISNULL(Column02, 0) AS Column02
                //                                                                           FROM   Table_030_Setting
                //                                                                           WHERE  [ColumnId] = 54
                //                                                                       ),
                //                                                                       0
                //                                                                   )", ConMain);
                //                    if (Convert.ToInt32(Command.ExecuteScalar()) != 0)
                //                        mlt_SaleType.Value = Convert.ToInt32(Command.ExecuteScalar());
                //                }
                //if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.SaleType))
                //    mlt_SaleType.Value = Properties.Settings.Default.SaleType;

                if (string.IsNullOrWhiteSpace(txt_desc.Text) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.SaleDescription))

                    txt_desc.Text = Properties.Settings.Default.SaleDescription;


                foreach (Janus.Windows.GridEX.GridEXColumn item in this.gridEX_List.RootTable.Columns)
                {
                    if (item.Key == "column20")
                        item.Selectable = false;
                    if (item.Key == "column11")
                        item.Selectable = false;
                    if (item.Key == "column07")
                        item.Selectable = false;

                }
                //if (mlt_SaleType.Value == null || string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString()))
                //    mlt_SaleType.Select();
                if (mlt_Ware.Value == null || string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()))
                    mlt_Ware.Select();
                else if (mlt_Customer.Value == null || string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString()))
                {
                    mlt_Customer.Select();

                }
                else
                {
                    txt_GoodCode.Select();
                    txt_GoodCode_Enter(null, null);
                }


            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void table_010_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {


                    gridEX_List.Enabled = true;
                    //uiTab1.Enabled = true;

                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                    if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    {
                        if (Row["Column42"] == DBNull.Value || Row["Column42"] == null || string.IsNullOrWhiteSpace(Row["Column42"].ToString()))
                            MessageBox.Show("انبار فاکتور انتخاب نشده است");
                        bt_New.Enabled = true;
                        DataTable dt = new DataTable();

                        SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as column19 FROM Table_010_SaleFactor where columnid=" + Row["ColumnId"], ConSale);
                        Adapter.Fill(dt);

                       

                        if (Convert.ToBoolean(dt.Rows[0]["Column53"]) ||// بستن صندوق
                            Convert.ToBoolean(dt.Rows[0]["column19"])// مرجوعی
                            )
                        {

                            gridEX_List.AllowAddNew = InheritableBoolean.False;
                            gridEX_List.AllowEdit = InheritableBoolean.False;
                            gridEX_List.AllowDelete = InheritableBoolean.False;
                            //btn_addtax.Enabled = false;
                            bt_Del.Enabled = false;
                            bt_Save.Enabled = false;
                        }
                        else
                        {
                            if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() == "0" ||// سند
                             ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() == "0")// حواله
                            {

                                gridEX_List.AllowAddNew = InheritableBoolean.True;
                                gridEX_List.AllowEdit = InheritableBoolean.True;
                                gridEX_List.AllowDelete = InheritableBoolean.True;

                                gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                                gridEX_Extra.AllowEdit = InheritableBoolean.True;
                                gridEX_Extra.AllowDelete = InheritableBoolean.True;

                                bt_Del.Enabled = true;
                                bt_Save.Enabled = true;
                                uiPanel1.Enabled = true;


                            }
                            else
                            {
                                gridEX_List.AllowAddNew = InheritableBoolean.False;
                                gridEX_List.AllowEdit = InheritableBoolean.False;
                                gridEX_List.AllowDelete = InheritableBoolean.False;



                                gridEX_Extra.AllowAddNew = InheritableBoolean.False;
                                gridEX_Extra.AllowEdit = InheritableBoolean.False;
                                gridEX_Extra.AllowDelete = InheritableBoolean.False;
                                //btn_addtax.Enabled = true;
                                bt_Del.Enabled = true;
                                bt_Save.Enabled = true;
                                uiPanel1.Enabled = false;
                            }
                           
                        }

                    }
                    else
                    {
                        //btn_addtax.Enabled = false;

                    }
                }
                else
                {
                    //btn_addtax.Enabled = false;
                    gridEX_List.Enabled = false;
                    //uiTab1.Enabled = false;

                }

            }
            catch
            { }
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    txt_TotalPrice.Value = Convert.ToDouble(
                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                AggregateFunction.Sum).ToString());
                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                        Convert.ToDouble(txt_Reductions.Value.ToString());


                }

            }
            catch
            {
            }
        }
        private void InitialNewRow()
        {
            try
            {
                bool isthere = false;
                if (txt_GoodCode.Text != string.Empty)
                {

                    long codeid = 0;
                    double buyprice = 0;
                    int ok = 0;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand("SELECT tcc.columnid FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.column06='" + txt_GoodCode.Text + "'", Con);
                        codeid = Convert.ToInt64(Comm.ExecuteScalar());




                        Comm = new SqlCommand(@"if exists (select * from table_004_CommodityAndIngredients where column06='" + txt_GoodCode.Text + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok", Con);
                        ok = Convert.ToInt32(Comm.ExecuteScalar());


                    }
                    if (ok == 1)
                    {


                        if (gridEX_List.GetRows().Count() > 0)
                        {
                            string goodcode;
                            Int16 unit;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    goodcode = (Comm.ExecuteScalar().ToString());
                                    Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    unit = Convert.ToInt16(Comm.ExecuteScalar());
                                }


                                if (goodcode == txt_GoodCode.Text && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                {

                                    isthere = true;
                                    item.BeginEdit();

                                    //float h = clDoc.GetZarib(Convert.ToInt32(codeid), Convert.ToInt16(item.Cells["column03"].Value));
                                    //item.Cells["Column07"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column07"].Value);
                                    //item.Cells["Column06"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column06"].Value);


                                    item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                    item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                    item.EndEdit();
                                    double TotalPrice;
                                    if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                        item.BeginEdit();
                                        item.Cells["column11"].Value = TotalPrice;
                                        item.Cells["column16"].Value = 0;
                                        item.Cells["column18"].Value = 0;
                                        item.Cells["column17"].Value = 0;
                                        item.Cells["Column19"].Value = 0;
                                        item.Cells["Column40"].Value = TotalPrice;
                                        item.Cells["Column20"].Value = TotalPrice;

                                        item.EndEdit();

                                    }
                                    gridEX_List.UpdateData();

                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                    break;

                                }

                            }
                            if (!isthere)
                            {

                                gridEX_List.MoveToNewRecord();
                                gridEX_List.SetValue("GoodCode", codeid);
                                gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("column02", codeid);

                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                    buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                }
                                gridEX_List.SetValue("Column41", buyprice);



                                //GoodbindingSource.Filter = "GoodID=" +
                                //        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    //gridEX_List.SetValue("tedaddarkartoon",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    //gridEX_List.SetValue("tedaddarbaste",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    //gridEX_List.SetValue("column03",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    //gridEX_List.SetValue("column16",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                    

                                    DataTable dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    double amunt = 0;
                                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    //{
                                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                    //    if (dr.Count() > 0)
                                    //    {
                                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                    //        gridEX_List.SetValue("column10",
                                    //         dr[0].ItemArray[3]);
                                    //    }
                                    //}

                                    if (amunt == Convert.ToDouble(0))
                                    {
                                        gridEX_List.SetValue("column10",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                        "SalePrice"].ToString());

                                    }
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);

                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);
                                        gridEX_List.SetValue("Column40", TotalPrice);
                                        gridEX_List.SetValue("Column20", TotalPrice);
                                    }


                                    gridEX_List.UpdateData();
                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());




                                }



                            }


                        }
                        else
                        {

                            gridEX_List.MoveToNewRecord();
                            gridEX_List.SetValue("GoodCode", codeid);
                            gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("column02", codeid);

                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
                            {
                                Con.Open();
                                SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                            }
                            gridEX_List.SetValue("Column41", buyprice);
                            {
                                //GoodbindingSource.Filter = "GoodID=" +
                                //    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    //gridEX_List.SetValue("tedaddarkartoon",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    //gridEX_List.SetValue("tedaddarbaste",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    //gridEX_List.SetValue("column03",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    //gridEX_List.SetValue("column16",
                                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                    //((DataRowView)gridEX1.RootTable.Columns["Column001"].DropDown.FindItem(gridEX1.GetValue("Column001")))["ColumnId"].ToString();


                                    DataTable dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    double amunt = 0;
                                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    //{
                                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                    //    if (dr.Count() > 0)
                                    //    {
                                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                    //        gridEX_List.SetValue("column10",
                                    //         dr[0].ItemArray[3]);
                                    //    }
                                    //}

                                    //if (amunt == Convert.ToDouble(0))
                                    //{
                                    //    gridEX_List.SetValue("column10",
                                    //        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                    //    "SalePrice"].ToString());

                                    //}
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);
                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);
                                        gridEX_List.SetValue("Column40", TotalPrice);
                                        gridEX_List.SetValue("Column20", TotalPrice);
                                    }

                                    gridEX_List.UpdateData();

                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());
                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                }
                            }
                        }


                        addnew();
                    }
                    else//کد کالا در جدول کالا نبوده است
                    {
                        if (txt_GoodCode.Text.Length >= 6)
                        {
                            long codeid1 = 0;
                            int ok1 = 0;
                            decimal count = 0;
                            string barcode = string.Empty;
                            barcode = txt_GoodCode.Text.Substring(0, txt_GoodCode.Text.Length - 6);
                            count = (Convert.ToDecimal(txt_GoodCode.Text.Substring(txt_GoodCode.Text.Length - 6, 6)) / 10000);
                            txt_Count.Text = string.Format("{0:#,##0.###;(#,##0.###)}", count);
                            count = Convert.ToDecimal(txt_Count.Text);
                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                            {
                                Con.Open();
                                SqlCommand Comm = new SqlCommand("SELECT tcc.columnid FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.column06='" + barcode + "'", Con);
                                codeid1 = Convert.ToInt64(Comm.ExecuteScalar());
                                Comm = new SqlCommand(@"if exists (select * from table_004_CommodityAndIngredients where column06='" + barcode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok", Con);
                                ok1 = Convert.ToInt32(Comm.ExecuteScalar());
                                //////////کالای وزنی
                                if (ok1 == 1)
                                {




                                    if (gridEX_List.GetRows().Count() > 0)
                                    {
                                        string goodcode;
                                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                        {
                                            using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.PWHRS))
                                            {
                                                Con1.Open();
                                                SqlCommand Comm1 = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con1);
                                                goodcode = (Comm1.ExecuteScalar().ToString());

                                            }


                                            if (goodcode == barcode && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                            {

                                                isthere = true;
                                                item.BeginEdit();
                                                item.Cells["Column07"].Value = Convert.ToDecimal(item.Cells["Column07"].Value.ToString()) + count;
                                                item.Cells["Column06"].Value = Convert.ToDecimal(item.Cells["Column06"].Value.ToString()) + count;

                                                item.EndEdit();
                                                double TotalPrice;
                                                if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                                {
                                                    TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                                    item.BeginEdit();
                                                    item.Cells["column11"].Value = TotalPrice;
                                                    item.Cells["column16"].Value = 0;
                                                    item.Cells["column18"].Value = 0;
                                                    item.Cells["column17"].Value = 0;
                                                    item.Cells["Column19"].Value = 0;
                                                    item.Cells["Column40"].Value = TotalPrice;
                                                    item.Cells["Column20"].Value = TotalPrice;

                                                    item.EndEdit();

                                                }

                                                gridEX_List.UpdateData();

                                                //محاسبه قیمتهای انتهای فاکتور
                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());


                                                break;

                                            }

                                        }
                                        if (!isthere)
                                        {

                                            gridEX_List.MoveToNewRecord();
                                            gridEX_List.SetValue("GoodCode", codeid1);
                                            gridEX_List.SetValue("Column07", count);
                                            gridEX_List.SetValue("Column06", count);

                                            gridEX_List.SetValue("column02", codeid1);



                                            //GoodbindingSource.Filter = "GoodID=" +
                                            //        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                            if (GoodbindingSource.CurrencyManager.Position > -1)
                                            {
                                                //gridEX_List.SetValue("tedaddarkartoon",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                                //gridEX_List.SetValue("tedaddarbaste",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                                //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                                //gridEX_List.SetValue("column03",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                                //gridEX_List.SetValue("column16",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());




                                                DataTable dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                                double amunt = 0;
                                                //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                                //{
                                                //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                //    if (dr.Count() > 0)
                                                //    {
                                                //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                //        gridEX_List.SetValue("column10",
                                                //         dr[0].ItemArray[3]);
                                                //    }
                                                //}

                                                if (amunt == Convert.ToDouble(0))
                                                {
                                                    gridEX_List.SetValue("column10",
                                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                                    "SalePrice"].ToString());

                                                }
                                                gridEX_List.SetValue("column09",
                                                      0);
                                                gridEX_List.SetValue("column08",
                                                   0);

                                                //if (Convert.ToBoolean(((DataRowView)GoodbindingSource.CurrencyManager.Current)["HasExtera"]) == true)
                                                //    gridEX_List.SetValue("column18",
                                                //                                   clDoc.ReturnTable(this.ConSale.ConnectionString, "SELECT  [column04] FROM   [dbo].[Table_024_Discount] WHERE column02=0").Rows[0]["column04"].ToString());
                                                //else
                                                //    gridEX_List.SetValue("column18", 0);
                                                //gridEX_List.SetValue("column10",
                                                //  ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());

                                                double TotalPrice;
                                                if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                                {
                                                    TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                    gridEX_List.SetValue("column11", TotalPrice);
                                                    gridEX_List.SetValue("Column16", 0);
                                                    gridEX_List.SetValue("Column18", 0);
                                                    gridEX_List.SetValue("column17", 0);
                                                    gridEX_List.SetValue("Column19", 0);
                                                    gridEX_List.SetValue("Column40", TotalPrice);
                                                    gridEX_List.SetValue("Column20", TotalPrice);
                                                }


                                                gridEX_List.UpdateData();
                                                //محاسبه قیمتهای انتهای فاکتور
                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());



                                            }



                                        }


                                    }
                                    else
                                    {

                                        gridEX_List.MoveToNewRecord();
                                        gridEX_List.SetValue("GoodCode", codeid1);
                                        gridEX_List.SetValue("Column07", count);
                                        gridEX_List.SetValue("Column06", count);

                                        gridEX_List.SetValue("column02", codeid1);


                                        {
                                            //GoodbindingSource.Filter = "GoodID=" +
                                            //    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                            if (GoodbindingSource.CurrencyManager.Position > -1)
                                            {
                                                //gridEX_List.SetValue("tedaddarkartoon",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                                //gridEX_List.SetValue("tedaddarbaste",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                                //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                                //gridEX_List.SetValue("column03",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                                //gridEX_List.SetValue("column16",
                                                //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                                
                                                DataTable dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                                double amunt = 0;
                                                //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                                //{
                                                //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                //    if (dr.Count() > 0)
                                                //    {
                                                //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                //        gridEX_List.SetValue("column10",
                                                //         dr[0].ItemArray[3]);
                                                //    }
                                                //}

                                                if (amunt == Convert.ToDouble(0))
                                                {
                                                    gridEX_List.SetValue("column10",
                                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                                    "SalePrice"].ToString());

                                                }
                                                gridEX_List.SetValue("column09",
                                                      0);
                                                gridEX_List.SetValue("column08",
                                                   0);

                                                double TotalPrice;
                                                if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                                {
                                                    TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                    gridEX_List.SetValue("column11", TotalPrice);
                                                    gridEX_List.SetValue("column11", TotalPrice);
                                                    gridEX_List.SetValue("Column16", 0);
                                                    gridEX_List.SetValue("Column18", 0);
                                                    gridEX_List.SetValue("column17", 0);
                                                    gridEX_List.SetValue("Column19", 0);
                                                    gridEX_List.SetValue("Column40", TotalPrice);
                                                    gridEX_List.SetValue("Column20", TotalPrice);
                                                }


                                                gridEX_List.UpdateData();

                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());



                                            }
                                        }
                                    }


                                    addnew();







                                }
                                else
                                    MessageBox.Show("کد کالا نامعتبر است");
                            }

                            //                        }

                            //throw new Exception("کد کالا نامعتبر است");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            addnew();

        }

        private void addnew()
        {
            txt_Count.Text = "1";
            txt_GoodCode.Text = string.Empty;
            txt_GoodCode.Focus();
            txt_GoodCode.SelectAll();

        }

        private void txt_GoodCode_Leave(object sender, EventArgs e)
        {
            var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
            var language = InputLanguage.FromCulture(culture);
            InputLanguage.CurrentInputLanguage = language;
        }

        private void gridEX_List_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
           
                if (txt_GoodCode.Text == string.Empty)
                {
                    long codeid = 0;

                    string txt_GoodCode1 = string.Empty;

                    {
                        if (gridEX_List.GetRows().Count() > 0)
                        {
                            Int16 unit = 0;

                            if (gridEX_List.GetValue("column02").ToString() != string.Empty)
                            {
                                codeid = Convert.ToInt64(gridEX_List.GetValue("column02").ToString());
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + codeid + " ", Con);
                                    txt_GoodCode1 = (Comm.ExecuteScalar()).ToString();
                                    Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + codeid + "", Con);
                                    unit = Convert.ToInt16(Comm.ExecuteScalar());


                                }

                                string goodcode;
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                {
                                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                                    {
                                        Con.Open();
                                        SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                        goodcode = (Comm.ExecuteScalar().ToString());
                                    }


                                    if (goodcode == txt_GoodCode1 && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                    {

                                        e.Cancel = true;
                                        gridEX_List.CancelCurrentEdit();
                                    }
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("اضافه کردن کالا با خطا مواجه شد.شرح خطا" + ex.Message);
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

        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

            }
            catch { }

            try
            {

                //if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0")
                //{
                //    MessageBox.Show("این فاکتور حواله صادر شده است");
                //    return;
                //}
                //if (((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0")
                //{
                //    MessageBox.Show("این فاکتور سند صادر شده است");
                //    return;
                //}
               

                //درج نام کالا، کد کالا
                if (e.Column.Key == "column02")
                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

                //درج تخفیف، اضافه خطی، واحد شمارش، تعداد در کارتن، تعداد در بسته
                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode" ||
                    gridEX_List.GetRow().Cells["column30"].Text.ToString() == "True")
                {





                    //GoodbindingSource.Filter = "GoodID=" +
                    //    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                    //gridEX_List.SetValue("Column41", Convert.ToDouble(((DataRowView)GoodbindingSource.CurrencyManager.Current)["buyprice"]));
                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(Int32));
                    dt.Columns.Add("Column00", typeof(Int32));
                    dt.Columns.Add("Column01", typeof(String));
                    dt.Columns.Add("Column02", typeof(Double));

                    dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                    //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                    

                    //gridEX_List.DropDowns[6].SetDataBinding(dt, "");
                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                    gridEX_List.SetValue("tedaddarkartoon",
                        0);
                    gridEX_List.SetValue("tedaddarbaste",
                        0);
                    gridEX_List.SetValue("column03", ((DataRowView)gridEX_List.RootTable.Columns["Column02"].DropDown.FindItem(gridEX_List.GetValue("Column02")))["CountUnit"].ToString());
                
                    gridEX_List.SetValue("column16", 0);
                    gridEX_List.SetValue("column18", 0);

                    //gridEX_List.SetValue("Column36", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                    double amunt = 0;
                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString()))
                    //{
                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text + "'");
                    //    if (dr.Count() > 0)
                    //    {
                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                    //        gridEX_List.SetValue("column10",
                    //         dr[0].ItemArray[3]);
                    //    }
                    //}

                    //if (amunt == Convert.ToDouble(0))
                    //{
                    //    gridEX_List.SetValue("column10",
                    //        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                    //    "SalePrice"].ToString());

                    //}
                    gridEX_List.SetValue("column09",
                          0);
                    gridEX_List.SetValue("column08",
                       0);

                }


                if (e.Column.Key == "column03")
                {
                    string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                               "table_004_CommodityAndIngredients", "column07", "ColumnId",
                               gridEX_List.GetValue("GoodCode").ToString());
                    if (gridEX_List.GetValue("column03").ToString() != orginalunit)
                    { }
                    //gridEX_List.SetValue("column10",
                    //     ((DataRowView)gridEX_List.RootTable.Columns["column03"].DropDown.FindItem(gridEX_List.GetValue("column03")))["sale"].ToString());
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", typeof(Int32));
                        dt.Columns.Add("Column00", typeof(Int32));
                        dt.Columns.Add("Column01", typeof(String));
                        dt.Columns.Add("Column02", typeof(Double));

                        dt = clDoc.ReturnTable(this.ConWare, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");

                        double amunt = 0;
                        //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString()))
                        //{
                        //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text + "'");
                        //    if (dr.Count() > 0)
                        //    {
                        //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                        //        gridEX_List.SetValue("column10",
                        //         dr[0].ItemArray[3]);
                        //    }
                        //}

                        //if (amunt == Convert.ToDouble(0))
                        //{
                        //    gridEX_List.SetValue("column10",
                        //        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                        //    "SalePrice"].ToString());

                        //}
                    }
                    //if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    //{
                    //    float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")),Convert.ToInt16(gridEX_List.GetValue("column03")));
                    //    gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    //    gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    //}
                }

                if (e.Column.Key == "column06")
                {
                    //float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(gridEX_List.GetValue("column03")));

                    //gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    //gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    gridEX_List.SetValue("column07", gridEX_List.GetValue("column06"));
                }

                Double TotalPrice =
                        Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                     Convert.ToDouble(gridEX_List.GetValue("column10")));
                gridEX_List.SetValue("Column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100);


                gridEX_List.SetValue("column17", 0);
                gridEX_List.SetValue("column19", 0);
                gridEX_List.SetValue("column20", TotalPrice);


                //محاسبه قیمتهای انتهای فاکتور
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

        private void gridEX_List_CellValueChanged(object sender, ColumnActionEventArgs e)
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
            //try
            //{
            //    gridEX_List.RootTable.Columns["column10"].FormatString = "#,##0.###";
            //}
            //catch { }
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

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {
                //setnull();
                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_List_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
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
            catch (Exception ex)
            {
                MessageBox.Show("اضافه کردن کالا با خطا مواجه شد.شرح خطا" + ex.Message);
            }
        }

        private void gridEX_List_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (
                    //((Janus.Windows.GridEX.GridEX)(sender)).Col == 5 &&

                 gridEX_List.GetValue("GoodCode") != null)
                {
                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                }
                if (
                    //((Janus.Windows.GridEX.GridEX)(sender)).Col == 14 &&

                 gridEX_List.GetValue("GoodCode") != null)
                {
                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                }
                // gridEX_List.DropDowns["GoodPrice"].SetDataBinding(clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   "), "");
                //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_01_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
            }
            catch
            {
            }
        }

        private void gridEX_List_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column03")
                {


                    if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    {
                        float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(e.InitialValue), Convert.ToInt16(e.Value));
                        gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                        gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    }
                }
            }
            catch { }
        }

        private void Save_Event(object sender, EventArgs e)
        {
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
            gridEX_List.UpdateData();
            gridEX_Extra.UpdateData();
            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                mlt_Customer.Value != null && !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString())
                //&& mlt_SaleType.Value != null && !string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString())
                && !string.IsNullOrWhiteSpace(txt_date.Text) && txt_date.IsTextValid()
                && mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()))

                {

                this.Cursor = Cursors.WaitCursor;


                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column10"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(txt_date.Text.Substring(0, 4)),
                 Convert.ToInt32(txt_date.Text.Substring(5, 2)),
                 Convert.ToInt32(txt_date.Text.Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    this.Cursor = Cursors.Default;

                    return;

                }
                //chehckessentioal();
                using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Conacnt.Open();
                    SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + mlt_Ware.Value + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور حواله به انبار انتخاب شده دسترسی ندارید");

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");
                }

                if (txt_num.Value.ToString().StartsWith("-"))
                {
                    txt_num.Value = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01");
                    //((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column61"] = 0;
                    this.table_010_SaleFactorBindingSource.EndEdit();
                }


                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                DataTable fdt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                Adapter.Fill(fdt);

                if (fdt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                        throw new Exception("به علت بسته شدن صندوق امکان دخیره اطلاعات وجود ندارد");

                    if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
                        throw new Exception("به علت ارجاع فاکتور امکان دخیره اطلاعات وجود ندارد");
                }




                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                int DocId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID);
                int DraftId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID);
                this.Cursor = Cursors.WaitCursor;


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID;



                    Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + RowID;
                    command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column10=0,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column09=0,column15='" + Class_BasicOperation._UserName + "',column16=getdate()  where   columnid=" + RowID;



                }





                if (!string.IsNullOrWhiteSpace(command))
                {
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


                        }
                        catch (Exception es)
                        {
                            ok = false;
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;

                            Class_BasicOperation.CheckExceptionType(es, this.Name);

                        }
                    }
                }


                if (ok)
                {
                    txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());

                    double Total = double.Parse(txt_TotalPrice.Value.ToString());

                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column04"].Value = Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100);
                            item.EndEdit();

                        }
                    }
                    Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                    Filter.Value1 = true;
                    txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                   Convert.ToDouble(txt_Extra.Value.ToString()) -
                   Convert.ToDouble(txt_Reductions.Value.ToString());


                    if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                    {
                        Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                        this.Cursor = Cursors.Default;

                        return;
                    }


                    Row["Column15"] = Class_BasicOperation._UserName;
                    Row["Column16"] = Class_BasicOperation.ServerDate();
                    Row["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //****************Calculate Discounts

                    double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                    int CustomerCode = int.Parse(Row["Column03"].ToString());
                    string Date = Row["Column02"].ToString();
                    Row["Column28"] = NetTotal;
                    Row["Column30"] = 0;
                    Row["Column29"] = 0;
                    Row["Column31"] = 0;


                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                    Filter2.Value1 = true;
                    Row["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                   
                   
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);
                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);
                    dataSet_01_Sale.EnforceConstraints = true;
                  

                    try
                    {

                        // Properties.Settings.Default.Customer = mlt_Customer.Value.ToString();
                        //try
                        //{
                        //    if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
                        //        ConWare.ConnectionString = (mlt_Ware.Value.ToString());
                        //    Properties.Settings.Default.Save();
                        //}
                        //catch { }
                        //clDoc.RunSqlCommand(ConMain.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + mlt_SaleType.Value + " where ColumnId=54");
                        //string cm = string.Empty;
                        //foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        //{
                        //    cm += @" Update table_004_CommodityAndIngredients set Column34=" + item.Cells["column10"].Value + " where columnid=" + item.Cells["GoodCode"].Value;

                        //}
                        //clDoc.RunSqlCommand(Properties.Settings.Default.PWHRS, cm);

                    }
                    catch
                    {
                    }
                    checksanad();
                    Adapter = new SqlDataAdapter(@"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 4) ", ConBase);
                    Adapter.Fill(factordt);
                    string sanadcmd = string.Empty;
                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                    DraftNum.Direction = ParameterDirection.Output;

                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                    DocNum.Direction = ParameterDirection.Output;
                    sanadcmd = "   declare @draftkey int declare @DocID int set @DraftNum=( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)";
                    sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + txt_date.Text + "'," + mlt_Ware.Value
                                 + "," + mlt_Function.Value + @", " + mlt_Customer.Value + ",'" + "حواله صادره بابت فاکتور فروش ش" + txt_num.Value +
                                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

                    Adapter = new SqlDataAdapter(
                                                                @"SELECT  [columnid] ,[column01] ,[column02] ,[column03] ,[column04] ,[column05] ,[column06] ,[column07] ,[column08] ,[column09]
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
                                                                      ,[column30]
                                                                      ,[Column31]
                                                                      ,[Column32]
                                                                      ,[Column33]
                                                                    ,Column34,Column35,Column36,Column37
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" +
                                                          ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString(), ConSale);
                    DataTable Child1 = new DataTable();
                    Adapter.Fill(Child1);
                    string salepric = string.Empty;
                    foreach (DataRow item1 in Child1.Rows)
                    {
                        //salepric += " Update " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients set Column34=" + item1["column10"] + " where columnid=" + item1["Column02"];

                        if (clDoc.IsGood(item1["Column02"].ToString()))
                        {
                            double value = Convert.ToDouble(item1["Column07"]);
                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                item1["Column02"].ToString());


                            if (item1["column03"].ToString() != orginalunit)
                            {
                                float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["column03"]), Convert.ToInt16(orginalunit));
                                value = value * h;
                            }

                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
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
           ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," + orginalunit
                               + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
                                value + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                                item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
                                + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                                (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
                                item1["Column15"].ToString() +
                                    ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                    (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                    (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                    + "," + item1["Column31"].ToString()
                                    + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")";
                        }
                    }
                    sanadcmd += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@draftkey,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (LastDocnum > 0)
                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                    else
                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + txt_date.Text + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + mlt_Customer.Value + @", NULL , NULL ,
                   " + "'فاکتور فروش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                  ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                        VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                                    " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                                    " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                                    " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                                    " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                                    NULL, NULL , NULL ,
                                       " + "'فاکتور فروش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";

                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["Kosoorat"] != null &&
                            dr["Kosoorat"].ToString() != string.Empty &&
                            Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
                        {


                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                                VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                                            NULL, NULL , NULL ,
                                               " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                                VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                                           " + mlt_Customer.Value + @", NULL , NULL ,
                                               " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        }

                        if (dr["Ezafat"] != null &&
                          dr["Ezafat"].ToString() != string.Empty &&
                          Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
                        {

                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                                VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                                            " + mlt_Customer.Value + @", NULL , NULL ,
                                               " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                                VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                                            NULL, NULL , NULL ,
                                               " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }


                    }
                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10=@DocID,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId =" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    sanadcmd += salepric;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = sanadcmd;
                            Command.Parameters.Add(DocNum);
                            Command.Parameters.Add(DraftNum);
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();

                            if (sender == bt_Save || sender == this)
                                Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");
                            //if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            //{
                            //    List<string> List = new List<string>();
                            //    //List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
                            //    PSHOP._05_Sale.Reports.Form_SaleFactorPrint1 frm = new PSHOP._05_Sale.Reports.Form_SaleFactorPrint1(List,
                            //        int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
                            //    frm.Form_FactorPrint_Load(null, null);

                            //    //_05_Sale.Reports.Form_SaleFactorPrint1 frm =
                            //    //   new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                            //    //frm.ShowDialog();
                            //}
                            bt_New.Enabled = true;
                            dataSet_01_Sale.EnforceConstraints = false;
                            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(
                            this.dataSet_01_Sale.Table_011_Child1_SaleFactor, int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
                            dataSet_01_Sale.EnforceConstraints = true;

                            txt_TotalPrice.Value = Convert.ToDouble(
                   gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                   AggregateFunction.Sum).ToString());
                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                Convert.ToDouble(txt_Reductions.Value.ToString());
                            //int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());


                            this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                            Frm_002_StoreFaktor_Load(sender, e);
                           

                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;

                    }

                    /*  int _ID = int.Parse(Row["ColumnId"].ToString());
                      dataSet_01_Sale.EnforceConstraints = false;
                      this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, _ID);
                      this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, _ID);
                      this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, _ID);
                      dataSet_01_Sale.EnforceConstraints = true;
                      table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                      bt_New.Enabled = true;*/
                    this.Cursor = Cursors.Default;



                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "امکان ثبت وجود ندارد یا اطلاعات کامل نیست", "Information");
                }
            }
        }




        private void chehckessentioal()
        {

            discountdt = new DataTable();
            taxdt = new DataTable();
            factordt = new DataTable();
            // waredt = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(
                                               @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16
                                                                    FROM            Table_024_Discount
                                                                     group by column10,column16
                                                                     ", ConSale);
            discountdt = new DataTable();
            Adapter.Fill(discountdt);
            foreach (DataRow dr in discountdt.Rows)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column10"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");

                }





                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column16"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");
                }


            }


//            Adapter = new SqlDataAdapter(
//                                                                   @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
//                                                                        FROM            Table_002_SalesTypes
//                                                                        WHERE        (columnid = " + mlt_SaleType.Value + ") ", ConBase);
//            Adapter.Fill(factordt);


            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                    throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");


            }



//            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
//            {
//                Con.Open();
//                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
//                                                                   SELECT *
//                                                                   FROM   AllHeaders()
//                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column07"].ToString() + @"'
//                                                               )
//                                                                SELECT 1 AS ok
//                                                            ELSE
//                                                                SELECT 0 AS ok", Con);
//                if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
//                    throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");
//            }



            if (waredt.Rows.Count >= 1)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   table_005_PwhrsOperation
                                                                   WHERE  columnid = " + waredt.Rows[0]["Column02"] + @"
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("عملکرد در قسمت تنظیمات انتخاب نشده است");
                }



                //                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                //                {
                //                    Con.Open();
                //                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                //                                                                   SELECT *
                //                                                                   FROM   Table_001_PWHRS
                //                                                                   WHERE  columnid = " + waredt.Rows[1]["Column02"] + @"
                //                                                               )
                //                                                                SELECT 1 AS ok
                //                                                            ELSE
                //                                                                SELECT 0 AS ok", Con);
                //                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                //                        throw new Exception("انبار در قسمت تنطیمات انتخاب نشده است");
                //                }

            }
            else
                throw new Exception(" عملکرد تعریف نشده است");
            ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"] = waredt.Rows[0]["Column02"];


            LastDocnum = LastDocNum();
            if (LastDocnum > 0)
                clDoc.IsFinal(LastDocnum);

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(txt_date.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

        }

        public int LastDocNum()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(isnull( Column00,0)) from Table_060_SanadHead where Column01='" + txt_date.Text + "'),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {

                Checknumber();
                Save_Event(sender, e);
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); 
                this.Cursor = Cursors.Default;
            }
        }

        private void Checknumber() {


            #region بررسی موجودی برای حواله های مصرفی
            //if (Sodorhavelmasrafi == false)
            //{
                string good1 = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()), int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text + " در این انبار فعال نمی باشد ");

                    float Remain = FirstRemain1(int.Parse(item.Cells["column02"].Value.ToString()), mlt_Ware.Value.ToString());
                    bool mojoodimanfi = false;
                    try
                    {
                        using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.PWHRS))
                        {

                            ConWareGood.Open();
                            SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                                                   (
                                                                                                       SELECT ISNULL(Column16, 0) AS Column16
                                                                                                       FROM   table_004_CommodityAndIngredients
                                                                                                       WHERE  ColumnId = " + item.Cells["column02"].Value + @"
                                                                                                   ),
                                                                                                   0
                                                                                               ) AS Column16", ConWareGood);
                            mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                        }
                    }
                    catch
                    {
                    }
                    if (Remain < float.Parse(item.Cells["column07"].Value.ToString()))
                    {
                        if (!mojoodimanfi)
                        {
                            good1 += item.Cells["column02"].Text + " , ";
                        }

                    }

                }
                if (!string.IsNullOrEmpty(good1))
                    throw new Exception("عدم موجودی کالای " + good1.TrimEnd(','));
            //}
            #endregion

        }

    
        private float FirstRemain1(int GoodCode, string Ware)
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
                CommandText = string.Format(CommandText, Ware, GoodCode, txt_date.Text);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            string m = "فاکتور شماره ی " + txt_num.Value + "دخیره شد اما صدور سند به دلیل زیر با خطا مواجه شد" + Environment.NewLine;
            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {
                    bt_New.Enabled = true;
                    throw new Exception(m + "انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {
                    bt_New.Enabled = true;
                    throw new Exception(m + "انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }


            //using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            //{
            //    ConAcnt.Open();
            //    SqlCommand Command = new SqlCommand("Select Control_Person from AllHeaders() where ACC_Code='" + AccountCode + "'", ConAcnt);
            //    if (Person == null && bool.Parse(Command.ExecuteScalar().ToString()))
            //    {
            //        Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            //        Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
            //        throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Center from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Center == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Project from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Project == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }
            //}

        }
        public Int16 AccHasPerson(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Person from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private string AccountName(string AccountCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
                string _AccountName = Select.ExecuteScalar().ToString();
                return _AccountName;
            }
        }
        private Int16 AccHasCenter(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Center from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private Int16 AccHasProject(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Project from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }

        private void checksanad()
        {
            Sanaddt = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal
                                                   FROM   dbo.Table_010_SaleFactor
                                                          
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + @"
                                                                                                           ", ConSale);
            Adapter.Fill(Sanaddt);

            if (Convert.ToDouble(txt_EndPrice.Value) <= Convert.ToDouble(0) ||
                Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()) <= Convert.ToDouble(0)
                )
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            //All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), int.Parse(mlt_Customer.Value.ToString()), null, null);
            //All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), null, null, null);
            //TPerson.Rows.Add(Int32.Parse(mlt_Customer.Value.ToString()), factordt.Rows[0]["Column07"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
            //TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
            //TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

            foreach (DataRow dr in Sanaddt.Rows)
            {


                if (Convert.ToDouble(dr["Ezafat"]) > 0)
                {
                    All_Controls_Row1(dr["Bed"].ToString(), int.Parse(mlt_Customer.Value.ToString()), null, null);
                    All_Controls_Row1(dr["Bes"].ToString(), null, null, null);
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                    TPerson.Rows.Add(Int32.Parse(mlt_Customer.Value.ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                }
                if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                {
                    All_Controls_Row1(dr["Bes"].ToString(), int.Parse(mlt_Customer.Value.ToString()), null, null);
                    All_Controls_Row1(dr["Bed"].ToString(), null, null, null);
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                    TPerson.Rows.Add(Int32.Parse(mlt_Customer.Value.ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                }



            }
            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 152))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    DataTable fdt = new DataTable();
                    SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                    Adapter.Fill(fdt);

                    if (fdt.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                            throw new Exception("به علت بسته شدن صندوق امکان حذف اطلاعات وجود ندارد");

                        if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
                            throw new Exception("به ارجاع فاکتور امکان حذف اطلاعات وجود ندارد");
                    }
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");


                    int DocId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID);
                    int DraftId = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID);
                    bool ok = true;
                    if (DocId > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری  و حواله انبار مربوطه نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            ok = true;
                        else
                            ok = false;

                    }
                    if (ok)
                    {
                        string command = string.Empty;
                        if (DocId > 0)
                        {
                            clDoc.IsFinal_ID(DocId);
                            DataTable Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID;



                            Table = clDoc.ReturnTable(ConAcnt, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + RowID;

                        }
                        if (DraftId > 0)
                        {
                            command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                            command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;


                        }

                        command += "delete from " + ConSale.Database + ".dbo.Table_012_Child2_SaleFactor where column01=" + RowID;
                        command += "delete from " + ConSale.Database + ".dbo.Table_011_Child1_SaleFactor where column01=" + RowID;
                        command += "delete from " + ConSale.Database + ".dbo.Table_010_SaleFactor where columnid=" + RowID;





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
                                bt_New.Enabled = true;
                                Class_BasicOperation.ShowMsg("", "حذف با موفقیت صورت گرفت", "Information");
                                dataSet_01_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, 0);
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, 0);
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, 0);
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

        private void bt_ReturnFactor_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();



                    DataTable fdt = new DataTable();
                    SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as column19 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                    Adapter.Fill(fdt);

                    if (fdt.Rows.Count > 0)

                        //if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                        //    throw new Exception("به علت بسته شدن صندوق امکان مرجوع کردن وجود ندارد");



                        Save_Event(sender, e);

                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                    {
                        throw new Exception("خطا در ثبت اطلاعات");

                    }


                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 72))
                        throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور فروش را ندارید");


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

                            this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

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

                                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                            }
                        }


                    }
                    else
                        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }



        private void InvertDoc(DataRowView Row)
        {
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
                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1  INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @Key=SCOPE_IDENTITY()";

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


        private void InsertReceipt(DataRowView Row)
        {
            if (Row["Column09"].ToString() == "0")
                return;


            _002_Sale.Frm_011_ResidInformationDialog frm = new _002_Sale.Frm_011_ResidInformationDialog();
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


        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Ware_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");
        }

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            try
            {
            //    if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
            //    {
            //        GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
            //DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
            //        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            //        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            //    }

                DataTable dt = clDoc.ReturnTable(ConWare, @"SELECT 
GoodsInformation.Columnid AS GoodID,
GoodsInformation.Column01 AS GoodCode,
GoodsInformation.Column02 AS GoodName,
GoodsInformation.Column03 AS MainGroup, 
GoodsInformation.column07 AS CountUnit,
GoodsInformation.Column22 as Weight,
GoodsInformation.Column04 AS SubGroup,
CASE WHEN  dbo.table_006_CommodityChanges.Column07 IS NULL 
            THEN  GoodsInformation.column09 ELSE  dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
            CASE WHEN  dbo.table_006_CommodityChanges.Column06 IS NULL 
            THEN  GoodsInformation.column08 ELSE  dbo.table_006_CommodityChanges.Column06 END AS NumberInPack,
                   CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
            THEN  GoodsInformation.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
            CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
            THEN  GoodsInformation.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
            ISNULL( dbo.table_006_CommodityChanges.Column18, 1) AS Active1,

 CASE WHEN TS003.Column03 IS NULL 
            THEN  GoodsInformation.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
            THEN  GoodsInformation.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
            THEN  GoodsInformation.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
            THEN  GoodsInformation.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
            THEN  GoodsInformation.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
            THEN  GoodsInformation.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
            THEN  GoodsInformation.Column38 ELSE ts003.column06 END AS Extra,
ISNULL(TS003.Column11, 1) AS Active2, 
             dbo.table_003_SubsidiaryGroup.column03 as SubGroupName,
               dbo.table_002_MainGroup.column02 as MainGroupName, 
            Goodsinformation.column29 AS Khas
            , WareRemain
 FROM table_004_CommodityAndIngredients GoodsInformation
LEFT JOIN dbo.Table_003_InformationProductCash AS TS003 ON  GoodsInformation.columnid = TS003.column01
LEFT JOIN  dbo.table_006_CommodityChanges ON   GoodsInformation.columnid =  dbo.table_006_CommodityChanges.column01
left join dbo.table_003_SubsidiaryGroup ON GoodsInformation.Column03 =  dbo.table_003_SubsidiaryGroup.columnid 
LEFT  JOIN dbo.table_002_MainGroup ON  dbo.table_003_SubsidiaryGroup.column01 =  dbo.table_002_MainGroup.columnid
LEFT JOIN 
	(SELECT SUM(R)-SUM(D) wareremain,Column02 FROM 
	
	(
SELECT tcpr.Column02 ,tcpr.Column07 R,0 D FROM Table_012_Child_PwhrsReceipt tcpr 
LEFT JOIN Table_011_PwhrsReceipt tpr ON tpr.columnid = tcpr.column01 
WHERE tpr.Column02<='"+txt_date.Text+@"'  AND tpr.column03="+mlt_Ware.Value+@"

union
SELECT tcpr.Column02,0 AS R,tcpr.Column07 D FROM Table_008_Child_PwhrsDraft tcpr 
LEFT JOIN Table_007_PwhrsDraft  tpr ON tpr.columnid = tcpr.column01 
WHERE tpr.Column02<='" + txt_date.Text + @"'  AND tpr.column03=" + mlt_Ware.Value + @"

	) AS t1 
	 GROUP BY t1.Column02) AS T  on t.Column02=goodsinformation.columnid
	

 WHERE ( Goodsinformation.column19 = 1) and     ISNULL( dbo.table_006_CommodityChanges.Column18, 1)=1 
 AND (ISNULL(TS003.Column11, 1) = 1) AND (GoodsInformation.Column28 = 1)
");


                gridEX_List.DropDowns["GoodCode"].DataSource=dt;
                gridEX_List.DropDowns["GoodName"].DataSource = dt;



            }
            catch
            {
            }
        }

        private void mlt_Customer_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Customer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");


        }

        private void mlt_Customer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mlt_Customer_ValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (mlt_Customer.Value != null &&
            //        !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString()))
            //    {

            //        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
            //        {
            //            Con.Open();
            //            SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(Column30,0) from Table_045_PersonInfo where ColumnId=" + mlt_Customer.Value + "),0)", Con);
            //            //if (int.Parse(Comm.ExecuteScalar().ToString()) > 0)
            //            //    //mlt_SaleType.Value = int.Parse(Comm.ExecuteScalar().ToString());


            //        }

            //        //txt_buyername.Text = mlt_Customer.Text;
            //    }
            //}
            //catch
            //{
            //}
        }

        private void txt_buyername_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                {
                    txt_GoodCode.Focus();
                    txt_GoodCode.Select(txt_GoodCode.Text.Length, 0);
                }
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    txt_GoodCode.Focus();
                    txt_GoodCode.Select(txt_GoodCode.Text.Length, 0);
                }
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

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                // gridEX1.UpdateData();
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
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column13='" + Class_BasicOperation._UserName + "'),0) as Row");

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

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {

                try
                {
                    //   gridEX1.UpdateData();
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
                        Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select Min(Column01) from Table_010_SaleFactor where Column01>" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " AND Column13='" + Class_BasicOperation._UserName + "'),0) as Row");

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
                    //   gridEX1.UpdateData();
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
                      ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "  AND Column13='" + Class_BasicOperation._UserName + "'),0) as Row");
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

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                // gridEX1.UpdateData();
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
                    Table = clDoc.ReturnTable(ConSale, "Select ISNULL((Select min(Column01) from Table_010_SaleFactor where Column13='" + Class_BasicOperation._UserName + "'),0) as Row");

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

      

        private void gridEX_Extra_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
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

        private void gridEX_Extra_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
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
                        gridEX_Extra.SetValue("column04",

                            Convert.ToInt64(kol * darsad / 100));
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

                           Convert.ToInt64(kol * darsad / 100));
                }
            }
            catch
            {
            }
        }

       

     

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 153))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int SanadID = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column10", RowID.ToString());
                    int DraftID = clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column09", RowID.ToString());

                    if (clDoc.OperationalColumnValueSA("Table_010_SaleFactor", "Column20", RowID.ToString()) != 0)
                        throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");

                    //using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                    //{
                    //    Con.Open();
                    //    SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(Column53,0) from Table_010_SaleFactor where ColumnId=" + RowID + "),0)", Con);
                    //    if (Convert.ToBoolean((Comm.ExecuteScalar().ToString())))
                    //        throw new Exception("به علت بسته شدن، حذف سند حسابداری امکانپذیر نمی باشد");

                    //}

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سند و حواله مربوط به این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
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

                        command += "Delete from " + ConWare.Database + ".dbo. Table_008_Child_PwhrsDraft where column01=" + DraftID;
                        command += "Delete from " + ConWare.Database + ".dbo. Table_007_PwhrsDraft where ColumnId=" + DraftID;



                        command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column10=0,Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;


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
                                Class_BasicOperation.ShowMsg("", "حذف سند حسابداری و حواله با موفقیت صورت گرفت", "Information");

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
                    dataSet_01_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    this.table_010_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_010_SaleFactor);
                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_011_Child1_SaleFactor);
                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_01_Sale.Table_012_Child2_SaleFactor);
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_01_Sale.Table_010_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, RowID);
                    dataSet_01_Sale.EnforceConstraints = true;
                    gridEX_List.AllowAddNew = InheritableBoolean.True;
                    gridEX_List.AllowEdit = InheritableBoolean.True;
                    gridEX_List.AllowDelete = InheritableBoolean.True;
                    //btn_addtax.Enabled = true;
                    bt_Del.Enabled = true;
                    bt_Save.Enabled = true;
                    uiPanel1.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void gb_factor_Click(object sender, EventArgs e)
        {

        }

        private void bt_Search_Click(object sender, EventArgs e)
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

        private void btn_PrintPreweiw_Click(object sender, EventArgs e)
        {
            try
            {
                PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
                PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                        {
                            //  Save_Event(sender, e);


                            PSALE._05_Sale.Reports.Form_SaleFactorPrint1 frm =
                               new PSALE._05_Sale.Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                            frm.ShowDialog();
                        }
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch
            {
                try
                {
                    PSALE._05_Sale.Reports.Form_SaleFactorPrint1 frm =
                              new PSALE._05_Sale.Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                    frm.ShowDialog();
                }
                catch
                { }

            }
            this.Cursor = Cursors.Default;
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                PSALE.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PSALE;
                PSALE.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PSALE.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PSALE.Class_BasicOperation._Year = Class_BasicOperation._FinYear;
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        // Save_Event(sender, e);


                        List<string> List = new List<string>();
                        //List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
                        PSALE._05_Sale.Reports.Form_SaleFactorPrint1 frm = new PSALE._05_Sale.Reports.Form_SaleFactorPrint1(List,
                            int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
                        frm.Form_FactorPrint_Load(null, null);


                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch
            {
                try
                {
                    List<string> List = new List<string>();
                    PSALE._05_Sale.Reports.Form_SaleFactorPrint1 frm = new PSALE._05_Sale.Reports.Form_SaleFactorPrint1(List,
                            int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);


                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }
            this.Cursor = Cursors.Default;
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
            try
            {
                //setnull();
                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void txt_GoodCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                txt_Count.Text = (Convert.ToInt32(txt_Count.Text) + 1).ToString();
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                if ((Convert.ToInt32(txt_Count.Text) - 1) > 0)

                    txt_Count.Text = (Convert.ToInt32(txt_Count.Text) - 1).ToString();

            }
        }

        private void txt_GoodCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                InitialNewRow();
            }


            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_GoodCode_Leave_1(object sender, EventArgs e)
        {

            var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
            var language = InputLanguage.FromCulture(culture);
            InputLanguage.CurrentInputLanguage = language;
        }

        private void gridEX_Extra_CellValueChanged_1(object sender, ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
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

        private void gridEX_Extra_UpdatingCell_1(object sender, UpdatingCellEventArgs e)
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
                        gridEX_Extra.SetValue("column04",

                            Convert.ToInt64(kol * darsad / 100));
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

                           Convert.ToInt64(kol * darsad / 100));
                }
            }
            catch
            {
            }
        }

        private void gridEX_Extra_CellUpdated(object sender, ColumnActionEventArgs e)
        {

        }

        private void txt_num_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void mlt_Function_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_desc_TextChanged(object sender, EventArgs e)
        {

        }

        private void mlt_Draft_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_desc.Focus();

        }

        //private void bt_Attachments_Click(object sender, EventArgs e)
        //{
        //    if (this.table_010_SaleFactorBindingSource.Count > 0)
        //    {
        //        DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

        //        if (Convert.ToInt32(Row["ColumnId"]) > 0)

        //        // if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 95))
        //        {
        //            try
        //            {
        //                PSALE._05_Sale.Form03_FactorAppendix frm = new PSALE._05_Sale.Form03_FactorAppendix(
        //                    int.Parse(Row["ColumnId"].ToString()),
        //                    int.Parse(Row["Column01"].ToString()));
        //                frm.ShowDialog();
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        else Class_BasicOperation.ShowMsg("", "ایتدا فاکتور را ذخیره کنید", "None");
        //    }
        //}

        
 
    }
}
