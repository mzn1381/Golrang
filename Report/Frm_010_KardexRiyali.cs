using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.SqlClient;

namespace PCLOR.Report
{
    public partial class Frm_010_KardexRiyali : Form
    {
        bool _BackSpace = false;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        DataRowView FirstRow;
        string Date1, Date2;
        DateTime? _Date1 = null, _Date2 = null;
        int _GoodId = 0;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);

        public Frm_010_KardexRiyali()
        {
            InitializeComponent();
        }
        public Frm_010_KardexRiyali(int GoodId, DateTime? Date1, DateTime? Date2)
        {
            InitializeComponent();
            _Date1 = Date1;
            _Date2 = Date2;
            _GoodId = GoodId;
        }
        private void Frm_010_KardexRiyali_Load(object sender, EventArgs e)
        {
          
           
            this.chk_ontatal.Checked = Properties.Settings.Default.chk_Total;

            if (_GoodId == 0)
            {
                string[] Dates = Properties.Settings.Default.KardexRiyali.Split('-');
                faDatePickerStrip1.FADatePicker.SelectedDateTime =
                    FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
                faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
                //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
                //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            }
            else
            {
                faDatePickerStrip1.FADatePicker.SelectedDateTime = _Date1;
                faDatePickerStrip2.FADatePicker.SelectedDateTime = _Date2;
            }

            try
            {
                this.table_004_CommodityAndIngredientsTableAdapter.Fill(this.dataSet_EtelaatePaye.table_004_CommodityAndIngredients);
            }
            catch { }
            //لیست انبارها
            gridEX2.DropDowns["GoodCode"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");
            gridEX1.DropDowns["GoodCode"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");

            gridEX2.DropDowns["GoodName"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");
            gridEX1.DropDowns["GoodName"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");

            gridEX2.DropDowns["Function"].SetDataBinding(clDoc.ReturnTable(ConWare,
                "Select ColumnId,Column02 from table_005_PwhrsOperation"), "");
            gridEX1.DropDowns["Function"].SetDataBinding(clDoc.ReturnTable(ConWare,
                "Select ColumnId,Column02 from table_005_PwhrsOperation"), "");
            gridEX2.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase,
                "Select ColumnId,Column02 from Table_045_PersonInfo"), "");
            gridEX1.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase,
              "Select ColumnId,Column02 from Table_045_PersonInfo"), "");
            gridEX2.DropDowns["Center"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo");
            gridEX1.DropDowns["Center"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo");

            gridEX2.DropDowns["Project"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");
            gridEX1.DropDowns["Project"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");

            gridEX2.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

            DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select ColumnId,Column02 from Table_001_PWHRS 
                                                                                        where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')
                                                                                            UNION ALL Select 0,'همه انبارها' order by ColumnId");
            mlt_Ware.DataSource = WareTable;
            mlt_Ware.Value = 0;
            gridEX2.DropDowns["Ware"].SetDataBinding(clDoc.ReturnTable(ConWare, "Select ColumnId,Column02 from Table_001_PWHRS"), "");
            gridEX1.DropDowns["Ware"].SetDataBinding(clDoc.ReturnTable(ConWare, "Select ColumnId,Column02 from Table_001_PWHRS"), "");

            gridEX1.DropDowns["NumberProduct"].DataSource = gridEX2.DropDowns["NumberProduct"].DataSource = clDoc.ReturnTable(ConPCLOR, @"Select ID,Number from Table_035_Production");



            mlt_Ware.Select();
            mlt_Ware.Focus();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            this.gridEX_Goods.DropDowns["CountUnit"].DataSource = Table;
            gridEX2.RootTable.Caption = null;
            gridEX1.RootTable.Caption = null;

            if (_GoodId != 0)
            {
                gridEX_Goods.FindAll(gridEX_Goods.RootTable.Columns["GoodID"], Janus.Windows.GridEX.ConditionOperator.Equal, _GoodId);
                buttonX1_Click(sender, e);
            }
            this.chk_round.Checked = Properties.Settings.Default.kartexround;

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (mlt_Ware.Text.Trim() == "")
            {
                MessageBox.Show(" انبار مورد نظر را انتخاب کنید");
                return;
            }


            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                Classes.Class_Documents clDocument = new Classes.Class_Documents();
                string s = string.Empty;
                if (clDocument.ReturnTable(ConWare, @"SELECT NAME
                    FROM master.dbo.sysdatabases 
                    WHERE ('[' + name + ']' = '" + ConSale.Database + @"' 
                    OR name = '" + ConSale.Database + @"')").Rows.Count > 0)
                {
                    s = @"
                           SELECT *
FROM   (
           SELECT buy.column01 as Factor,dbo.Table_011_PwhrsReceipt.column06 as Des,Table_011_PwhrsReceipt.columnid AS columnid,
                  Table_011_PwhrsReceipt.column02 AS Tarikh,
                  Table_011_PwhrsReceipt.column04 AS OP,
                  Table_011_PwhrsReceipt.column05 AS Person,
                  Table_011_PwhrsReceipt.column01 AS Shomareh,
                  0 AS TYPE,
                  Table_011_PwhrsReceipt.column03 AS Anbar,
                  Table_012_Child_PwhrsReceipt.column02 AS Kala,
                  Table_012_Child_PwhrsReceipt.column07 AS TedadResid,
                  Table_012_Child_PwhrsReceipt.column04 as tedadCartonResid,
                  CAST(
                      ROUND(
                          (
                            isnull(  dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                                  (
                                      SELECT tcai.column09
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                  ),
                                  0
                              ),0)
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TtedadCartonResid,
                  Table_012_Child_PwhrsReceipt.column05 as tedadBasteResid,

                  CAST(
                      ROUND(
                          (
                            isnull(  dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                                  (
                                      SELECT tcai.column08
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                  ),
                                  0
                              ),0)
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TtedadBasteResid,
                  Table_012_Child_PwhrsReceipt.column20 AS ArzeshVahedResid,
                  Table_012_Child_PwhrsReceipt.column21 AS ArzeshKolResid,
                  0.0 AS TedadHavale,
                  0 AS tedadCartonHavaleh,
                  0 AS TtedadCartonHavaleh,

                  0 AS tedadBasteHavaleh,
                  0 AS TtedadBasteHavaleh,

                  0.0 AS ArzeshVahedHavale,
                  0.0 AS ArzeshKolHavale,
                  0.0 AS TedadMandeh,
                  0.0 AS tedadCartonMadeh,
                  0.0 AS TtedadCartonMadeh,
                  0.0 AS tedadBasteMandeh,
                  0.0 AS TtedadBasteMandeh,
                  CAST(0.0 AS DECIMAL(18, 4)) AS ArzeshVahedMandeh,
                  0.0 AS ArzeshKolMandeh,
                  dbo.Table_012_Child_PwhrsReceipt.Column30,
                  dbo.Table_012_Child_PwhrsReceipt.Column31,
                  Table_011_PwhrsReceipt.Column07 AS DocId,
                  Table_012_Child_PwhrsReceipt.column13 AS Center,
                  Table_012_Child_PwhrsReceipt.column14 AS Project,
                  Table_011_PwhrsReceipt.Column09 AS MiladiDate,
                      Table_012_Child_PwhrsReceipt.column07 * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_012_Child_PwhrsReceipt.column02
                          ),0) AS ResidWeight,
                    0 AS HavaleWeight,
                    0 AS MandeWeight,
                    Table_012_Child_PwhrsReceipt.Column36 as Brand,
                    Table_012_Child_PwhrsReceipt.Column37 as Supplyer,
                    Table_012_Child_PwhrsReceipt.Column30 as Seri,
                    Table_012_Child_PwhrsReceipt.Column31 as ExpireDate,Table_012_Child_PwhrsReceipt.column12  as gooddesc
            
 ,(SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct  FROM  
                   " + ConPCLOR.Database + @".dbo.Table_050_Packaging INNER JOIN " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS ON " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode = " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode 
			WHERE     " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.NumberRecipt = dbo.Table_011_PwhrsReceipt.columnid 
			AND " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode=dbo.Table_012_Child_PwhrsReceipt.Column30 
			UNION ALL
			SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct  FROM  
                   " + ConPCLOR.Database + @".dbo.Table_050_Packaging 
			WHERE     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.NumberRecipt = dbo.Table_011_PwhrsReceipt.columnid 
			AND " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode=dbo.Table_012_Child_PwhrsReceipt.Column30 

	        UNION ALL

			SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct
FROM        " + ConPCLOR.Database + @". dbo.Table_050_Packaging INNER JOIN
                       " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale ON 
                     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode =  " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale.Column32 INNER JOIN
                       " + ConSale.Database + @".dbo.Table_018_MarjooiSale ON 
                       " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale.column01 =  " + ConSale.Database + @".dbo.Table_018_MarjooiSale.columnid
WHERE     ( " + ConSale.Database + @".dbo.Table_018_MarjooiSale.column09 = dbo.Table_011_PwhrsReceipt.columnid )
AND " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode=dbo.Table_012_Child_PwhrsReceipt.Column30 
			) AS NumberProduct


           FROM   Table_011_PwhrsReceipt
                    left join " + ConSale.Database + @".dbo.Table_015_BuyFactor buy on buy.columnid=dbo.Table_011_PwhrsReceipt.column13
                  INNER JOIN Table_012_Child_PwhrsReceipt
                       ON  Table_011_PwhrsReceipt.columnid = 
                           Table_012_Child_PwhrsReceipt.column01
           WHERE  (Table_012_Child_PwhrsReceipt.column02 = {1})
                  AND (Table_011_PwhrsReceipt.column02 <= N'{2}') 
                      "
                                                    + (mlt_Ware.Value.ToString() ==
                        "0" ? "AND   dbo.Table_011_PwhrsReceipt.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"') " :
                        " AND (dbo.Table_011_PwhrsReceipt.column03 = {0})"
         ) + @") as View_010_Resid 
                                                 union all 
                                                 SELECT *
                            FROM   (
                                       SELECT buy.column01 as Factor,dbo.Table_007_PwhrsDraft.column06 as Des,Table_007_PwhrsDraft.columnid AS columnid,
                                              Table_007_PwhrsDraft.column02 AS Tarikh,
                                              Table_007_PwhrsDraft.column04 AS OP,
                                              Table_007_PwhrsDraft.column05 AS Person,
                                              Table_007_PwhrsDraft.column01 AS Shomareh,
                                              1 AS TYPE,
                                              Table_007_PwhrsDraft.column03 AS Anbar,
                                              Table_008_Child_PwhrsDraft.column02 AS Kala,
                                              0.0 AS TedadResid,
                                               0 AS tedadCartonResid,
                                               0 AS TtedadCartonResid,

                                              0 AS tedadBasteResid,
                                              0 AS TtedadBasteResid,

                                              0.0 AS ArzeshVahedResid,
                                              0.0 AS ArzeshKolResid,
                                              Table_008_Child_PwhrsDraft.column07 AS TedadHavale,
                                              dbo.Table_008_Child_PwhrsDraft.column04 as  tedadCartonHavaleh,
                                              cast(ROUND( (
                                              isnull(    dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                                                      (
                                                          SELECT tcai.column09
                                                          FROM   table_004_CommodityAndIngredients tcai
                                                          WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                      ),
                                                      0
                                                  ),0)
                                              ) ,3)AS DECIMAL(36,3)) AS TtedadCartonHavaleh,
                                              dbo.Table_008_Child_PwhrsDraft.column05 as tedadBasteHavaleh,

                                              cast(ROUND( (
                                                 isnull( dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                                                      (
                                                          SELECT tcai.column08
                                                          FROM   table_004_CommodityAndIngredients tcai
                                                          WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                      ),
                                                      0
                                                  ),0)
                                              ),3)AS DECIMAL(36,3)) AS TtedadBasteHavaleh,
                                              Table_008_Child_PwhrsDraft.column15 AS ArzeshVahedHavale,
                                              Table_008_Child_PwhrsDraft.column16 AS ArzeshKolHavale,
                                              0.0 AS TedadMandeh,
                                              0.0 AS tedadCartonMadeh,
                                              0.0 AS TtedadCartonMadeh,

                                              0.0  AS tedadBasteMandeh,
                                              0.0  AS TtedadBasteMandeh,

                                              0.0 AS ArzeshVahedMandeh,
                                              0.0 AS ArzeshKolMandeh,
                                              Table_008_Child_PwhrsDraft.Column30,
                                              Table_008_Child_PwhrsDraft.Column31,
                                              Table_007_PwhrsDraft.Column07 AS DocId,
                                              Table_008_Child_PwhrsDraft.column13 AS Center,
                                              Table_008_Child_PwhrsDraft.column14 AS Project,
                                              Table_007_PwhrsDraft.Column09 AS MiladiDate,
                                                0 AS ResidWeight,
												  Table_008_Child_PwhrsDraft.column07 * isnull((
                                                                  SELECT tcai.column22
                                                                  FROM   table_004_CommodityAndIngredients tcai
                                                                  WHERE  tcai.columnid = Table_008_Child_PwhrsDraft.column02
                                                              ),0)  AS HavaleWeight,
												0 AS MandeWeight,
                                         Table_008_Child_PwhrsDraft.Column36 as Brand,
                                        Table_008_Child_PwhrsDraft.Column37 as Supplyer,
                                Table_008_Child_PwhrsDraft.Column30 as Seri,
                             Table_008_Child_PwhrsDraft.Column31 as ExpireDate,Table_008_Child_PwhrsDraft.column12  as gooddesc

  ,(SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct
					                        FROM        " + ConPCLOR.Database + @". dbo.Table_65_HeaderOtherPWHRS INNER JOIN
										 " + ConPCLOR.Database + @". dbo.Table_70_DetailOtherPWHRS ON " + ConPCLOR.Database + @".dbo.Table_65_HeaderOtherPWHRS.ID = " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.FK INNER JOIN
										 " + ConPCLOR.Database + @". dbo.Table_050_Packaging ON " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode = " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode
					WHERE     (" + ConPCLOR.Database + @".dbo.Table_65_HeaderOtherPWHRS.Sends = 1) AND (" + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.NumberDraft = dbo.Table_007_PwhrsDraft.columnid) 
					AND " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode=dbo.Table_008_Child_PwhrsDraft.Column30


                UNION ALL


                SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct
                FROM         " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor INNER JOIN
                                      " + ConSale.Database + @".dbo.Table_010_SaleFactor ON 
                                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column01 = " + ConSale.Database + @".dbo.Table_010_SaleFactor.columnid INNER JOIN
                                     " + ConPCLOR.Database + @". dbo.Table_050_Packaging ON " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.Column34 = " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode
                WHERE     (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column09 = dbo.Table_007_PwhrsDraft.columnid)
					AND " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode=dbo.Table_008_Child_PwhrsDraft.Column30

)  AS NumberProduct



                                       FROM   Table_007_PwhrsDraft
                            left join " + ConSale.Database + @".dbo.Table_010_SaleFactor buy on buy.columnid=dbo.Table_007_PwhrsDraft.column16
                                              INNER JOIN Table_008_Child_PwhrsDraft
                                                   ON  Table_007_PwhrsDraft.columnid = 
                                                       Table_008_Child_PwhrsDraft.column01
                                       WHERE  (Table_008_Child_PwhrsDraft.column02 = {1})
                                              AND 
                                              (Table_007_PwhrsDraft.column02 >= N'{2}') "
         + (
             mlt_Ware.Value.ToString() == "0" ? "AND  dbo.Table_007_PwhrsDraft.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"') " : " AND (Table_007_PwhrsDraft.column03 = {0}) "
         )
         + @") as View_020_Havaleh " +
         " ORDER BY Tarikh , Type, columnid";
                }
                else
                {
                    s = @"
                           SELECT *
FROM   (
           SELECT 0 as Factor,dbo.Table_011_PwhrsReceipt.column06 as Des,Table_011_PwhrsReceipt.columnid AS columnid,
                  Table_011_PwhrsReceipt.column02 AS Tarikh,
                  Table_011_PwhrsReceipt.column04 AS OP,
                  Table_011_PwhrsReceipt.column05 AS Person,
                  Table_011_PwhrsReceipt.column01 AS Shomareh,
                  0 AS TYPE,
                  Table_011_PwhrsReceipt.column03 AS Anbar,
                  Table_012_Child_PwhrsReceipt.column02 AS Kala,
                  Table_012_Child_PwhrsReceipt.column07 AS TedadResid,
                  Table_012_Child_PwhrsReceipt.column04 as tedadCartonResid,
                  CAST(
                      ROUND(
                          (
                            isnull(  dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                                  (
                                      SELECT tcai.column09
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                  ),
                                  0
                              ),0)
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TtedadCartonResid,
                  Table_012_Child_PwhrsReceipt.column05 as tedadBasteResid,

                  CAST(
                      ROUND(
                          (
                            isnull(  dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                                  (
                                      SELECT tcai.column08
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                  ),
                                  0
                              ),0)
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TtedadBasteResid,
                  Table_012_Child_PwhrsReceipt.column20 AS ArzeshVahedResid,
                  Table_012_Child_PwhrsReceipt.column21 AS ArzeshKolResid,
                  0.0 AS TedadHavale,
                  0 AS tedadCartonHavaleh,
                  0 AS TtedadCartonHavaleh,

                  0 AS tedadBasteHavaleh,
                  0 AS TtedadBasteHavaleh,

                  0.0 AS ArzeshVahedHavale,
                  0.0 AS ArzeshKolHavale,
                  0.0 AS TedadMandeh,
                  0.0 AS tedadCartonMadeh,
                  0.0 AS TtedadCartonMadeh,
                  0.0 AS tedadBasteMandeh,
                  0.0 AS TtedadBasteMandeh,
                  CAST(0.0 AS DECIMAL(18, 4)) AS ArzeshVahedMandeh,
                  0.0 AS ArzeshKolMandeh,
                  dbo.Table_012_Child_PwhrsReceipt.Column30,
                  dbo.Table_012_Child_PwhrsReceipt.Column31,
                  Table_011_PwhrsReceipt.Column07 AS DocId,
                  Table_012_Child_PwhrsReceipt.column13 AS Center,
                  Table_012_Child_PwhrsReceipt.column14 AS Project,
                  Table_011_PwhrsReceipt.Column09 AS MiladiDate,
                      Table_012_Child_PwhrsReceipt.column07 * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_012_Child_PwhrsReceipt.column02
                          ),0) AS ResidWeight,
                    0 AS HavaleWeight,
                    0 AS MandeWeight,
                    Table_012_Child_PwhrsReceipt.Column36 as Brand,
                    Table_012_Child_PwhrsReceipt.Column37 as Supplyer,
                    Table_012_Child_PwhrsReceipt.Column30 as Seri,
                    Table_012_Child_PwhrsReceipt.Column31 as ExpireDate,Table_012_Child_PwhrsReceipt.column12  as gooddesc
            
                    
 ,(SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct  FROM  
                   " + ConPCLOR.Database + @".dbo.Table_050_Packaging INNER JOIN " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS ON " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode = " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode 
			WHERE     " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.NumberRecipt = dbo.Table_011_PwhrsReceipt.columnid 
			AND " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode=dbo.Table_012_Child_PwhrsReceipt.Column30 
			UNION ALL
			SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct  FROM  
                   " + ConPCLOR.Database + @".dbo.Table_050_Packaging 
			WHERE     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.NumberRecipt = dbo.Table_011_PwhrsReceipt.columnid 
			AND " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode=dbo.Table_012_Child_PwhrsReceipt.Column30 

	        UNION ALL
			SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct
FROM        " + ConPCLOR.Database + @". dbo.Table_050_Packaging INNER JOIN
                       " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale ON 
                     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode =  " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale.Column32 INNER JOIN
                       " + ConSale.Database + @".dbo.Table_018_MarjooiSale ON 
                       " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale.column01 =  " + ConSale.Database + @".dbo.Table_018_MarjooiSale.columnid
WHERE     ( " + ConSale.Database + @".dbo.Table_018_MarjooiSale.column09 = dbo.Table_011_PwhrsReceipt.columnid )
        AND " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode=dbo.Table_012_Child_PwhrsReceipt.Column30 

			) AS NumberProduct




           FROM   Table_011_PwhrsReceipt
                  INNER JOIN Table_012_Child_PwhrsReceipt
                       ON  Table_011_PwhrsReceipt.columnid = 
                           Table_012_Child_PwhrsReceipt.column01
           WHERE  (Table_012_Child_PwhrsReceipt.column02 = {1})
                  AND (Table_011_PwhrsReceipt.column02 <= N'{2}') 
                      "
                                                     + (mlt_Ware.Value.ToString() ==
                         "0" ? "AND   dbo.Table_011_PwhrsReceipt.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"') " :
                         " AND (dbo.Table_011_PwhrsReceipt.column03 = {0})"
          ) + @") as View_010_Resid 
                                                 union all 
                                                 SELECT *
                            FROM   (
                                       SELECT 0 as Factor,dbo.Table_007_PwhrsDraft.column06 as Des,Table_007_PwhrsDraft.columnid AS columnid,
                                              Table_007_PwhrsDraft.column02 AS Tarikh,
                                              Table_007_PwhrsDraft.column04 AS OP,
                                              Table_007_PwhrsDraft.column05 AS Person,
                                              Table_007_PwhrsDraft.column01 AS Shomareh,
                                              1 AS TYPE,
                                              Table_007_PwhrsDraft.column03 AS Anbar,
                                              Table_008_Child_PwhrsDraft.column02 AS Kala,
                                              0.0 AS TedadResid,
                                               0 AS tedadCartonResid,
                                               0 AS TtedadCartonResid,

                                              0 AS tedadBasteResid,
                                              0 AS TtedadBasteResid,

                                              0.0 AS ArzeshVahedResid,
                                              0.0 AS ArzeshKolResid,
                                              Table_008_Child_PwhrsDraft.column07 AS TedadHavale,
                                              dbo.Table_008_Child_PwhrsDraft.column04 as  tedadCartonHavaleh,
                                              cast(ROUND( (
                                              isnull(    dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                                                      (
                                                          SELECT tcai.column09
                                                          FROM   table_004_CommodityAndIngredients tcai
                                                          WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                      ),
                                                      0
                                                  ),0)
                                              ) ,3)AS DECIMAL(36,3)) AS TtedadCartonHavaleh,
                                              dbo.Table_008_Child_PwhrsDraft.column05 as tedadBasteHavaleh,

                                              cast(ROUND( (
                                                 isnull( dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                                                      (
                                                          SELECT tcai.column08
                                                          FROM   table_004_CommodityAndIngredients tcai
                                                          WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                      ),
                                                      0
                                                  ),0)
                                              ),3)AS DECIMAL(36,3)) AS TtedadBasteHavaleh,
                                              Table_008_Child_PwhrsDraft.column15 AS ArzeshVahedHavale,
                                              Table_008_Child_PwhrsDraft.column16 AS ArzeshKolHavale,
                                              0.0 AS TedadMandeh,
                                              0.0 AS tedadCartonMadeh,
                                              0.0 AS TtedadCartonMadeh,

                                              0.0  AS tedadBasteMandeh,
                                              0.0  AS TtedadBasteMandeh,

                                              0.0 AS ArzeshVahedMandeh,
                                              0.0 AS ArzeshKolMandeh,
                                              Table_008_Child_PwhrsDraft.Column30,
                                              Table_008_Child_PwhrsDraft.Column31,
                                              Table_007_PwhrsDraft.Column07 AS DocId,
                                              Table_008_Child_PwhrsDraft.column13 AS Center,
                                              Table_008_Child_PwhrsDraft.column14 AS Project,
                                              Table_007_PwhrsDraft.Column09 AS MiladiDate,
                                                0 AS ResidWeight,
												  Table_008_Child_PwhrsDraft.column07 * isnull((
                                                                  SELECT tcai.column22
                                                                  FROM   table_004_CommodityAndIngredients tcai
                                                                  WHERE  tcai.columnid = Table_008_Child_PwhrsDraft.column02
                                                              ),0)  AS HavaleWeight,
												0 AS MandeWeight,
                                         Table_008_Child_PwhrsDraft.Column36 as Brand,
                                        Table_008_Child_PwhrsDraft.Column37 as Supplyer,
                                Table_008_Child_PwhrsDraft.Column30 as Seri,
                             Table_008_Child_PwhrsDraft.Column31 as ExpireDate,Table_008_Child_PwhrsDraft.column12  as gooddesc


  ,(SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct
					                        FROM        " + ConPCLOR.Database + @". dbo.Table_65_HeaderOtherPWHRS INNER JOIN
										 " + ConPCLOR.Database + @". dbo.Table_70_DetailOtherPWHRS ON " + ConPCLOR.Database + @".dbo.Table_65_HeaderOtherPWHRS.ID = " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.FK INNER JOIN
										 " + ConPCLOR.Database + @". dbo.Table_050_Packaging ON " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode = " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode
					WHERE     (" + ConPCLOR.Database + @".dbo.Table_65_HeaderOtherPWHRS.Sends = 1) AND (" + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.NumberDraft = dbo.Table_007_PwhrsDraft.columnid) 
					AND " + ConPCLOR.Database + @".dbo.Table_70_DetailOtherPWHRS.Barcode=dbo.Table_008_Child_PwhrsDraft.Column30


                UNION ALL


                SELECT     " + ConPCLOR.Database + @".dbo.Table_050_Packaging.IDProduct
                FROM         " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor INNER JOIN
                                      " + ConSale.Database + @".dbo.Table_010_SaleFactor ON 
                                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column01 = " + ConSale.Database + @".dbo.Table_010_SaleFactor.columnid INNER JOIN
                                     " + ConPCLOR.Database + @". dbo.Table_050_Packaging ON " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.Column34 = " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode
                WHERE     (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column09 = dbo.Table_007_PwhrsDraft.columnid)
					AND " + ConPCLOR.Database + @".dbo.Table_050_Packaging.Barcode=dbo.Table_008_Child_PwhrsDraft.Column30

        )  AS NumberProduct




                                       FROM   Table_007_PwhrsDraft
                                              INNER JOIN Table_008_Child_PwhrsDraft
                                                   ON  Table_007_PwhrsDraft.columnid = 
                                                       Table_008_Child_PwhrsDraft.column01
                                       WHERE  (Table_008_Child_PwhrsDraft.column02 = {1})
                                              AND 
                                              (Table_007_PwhrsDraft.column02 >= N'{2}') "
          + (
              mlt_Ware.Value.ToString() == "0" ? "AND  dbo.Table_007_PwhrsDraft.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"') " : " AND (Table_007_PwhrsDraft.column03 = {0}) "
          )
          + @") as View_020_Havaleh " +
          " ORDER BY Tarikh , Type, columnid";
                }
                s = string.Format(s, mlt_Ware.Value.ToString(),
                    gridEX_Goods.GetValue("GoodID").ToString(), Date2);

                DataTable Table = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter(s, ConWare);
                Adapter.Fill(Table);

                BindingSource BindSource = new BindingSource();
                BindSource.DataSource = Table;


                double _ArzeshKolMandeh = 0, _TedadMande = 0, _WeightMande = 0, _bastemande = 0, _Tbastemande = 0, _cartonmande = 0, _Tcartonmande = 0;



                //for (int i = 0; i < Table.Rows.Count; i++)
                //{
                //    _TedadMande += Convert.ToDouble(Table.Rows[i]["TedadResid"]) -
                //        Convert.ToDouble(Table.Rows[i]["TedadHavale"]);
                //    _bastemande += Convert.ToDouble(Table.Rows[i]["tedadBasteResid"]) -
                //       Convert.ToDouble(Table.Rows[i]["tedadBasteHavaleh"]);
                //    _Tbastemande += Convert.ToDouble(Table.Rows[i]["TtedadBasteResid"]) -
                //      Convert.ToDouble(Table.Rows[i]["TtedadBasteHavaleh"]);
                //    _cartonmande += Convert.ToDouble(Table.Rows[i]["tedadCartonResid"]) -
                //      Convert.ToDouble(Table.Rows[i]["tedadCartonHavaleh"]);
                //    _Tcartonmande += Convert.ToDouble(Table.Rows[i]["TtedadCartonResid"]) -
                //      Convert.ToDouble(Table.Rows[i]["TtedadCartonHavaleh"]);

                //    _WeightMande += Convert.ToDouble(Table.Rows[i]["ResidWeight"]) -
                //        Convert.ToDouble(Table.Rows[i]["HavaleWeight"]);
                //    Table.Rows[i]["MandeWeight"] = _WeightMande;
                //    Table.Rows[i]["TedadMandeh"] = _TedadMande;
                //    Table.Rows[i]["tedadCartonMadeh"] = _cartonmande;
                //    Table.Rows[i]["TtedadCartonMadeh"] = _Tcartonmande;
                //    Table.Rows[i]["tedadBasteMandeh"] = _bastemande;
                //    Table.Rows[i]["TtedadBasteMandeh"] = _Tbastemande;


                //    if (chk_round.Checked)
                //    {
                //        Table.Rows[i]["ArzeshKolResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["ArzeshKolResid"]));
                //        Table.Rows[i]["ArzeshKolHavale"] = Math.Round(Convert.ToDouble(Table.Rows[i]["ArzeshKolHavale"]));
                //    }

                //    _ArzeshKolMandeh += Convert.ToDouble(Table.Rows[i]["ArzeshKolResid"]) -
                //        Convert.ToDouble(Table.Rows[i]["ArzeshKolHavale"]);

                //    Table.Rows[i]["ArzeshKolMandeh"] = _ArzeshKolMandeh;
                //    if(i==97)
                //    {
                //    }
                //    if (_TedadMande > 0)
                //    {
                //        Table.Rows[i]["ArzeshVahedMandeh"] = Math.Round(_ArzeshKolMandeh / _TedadMande, 3);
                //    }
                //    else
                //    {
                //        Table.Rows[i]["ArzeshVahedMandeh"] = 0;
                //    }
                //}


                for (int i = 0; i < Table.Rows.Count; i++)
                {

                    Table.Rows[i]["TedadResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["TedadResid"]), 3);
                    Table.Rows[i]["TedadHavale"] = Math.Round(Convert.ToDouble(Table.Rows[i]["TedadHavale"]), 3);
                    Table.Rows[i]["tedadBasteResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["tedadBasteResid"]), 3);
                    Table.Rows[i]["tedadBasteHavaleh"] = Math.Round(Convert.ToDouble(Table.Rows[i]["tedadBasteHavaleh"]), 3);
                    Table.Rows[i]["TtedadBasteResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["TtedadBasteResid"]), 3);
                    Table.Rows[i]["TtedadBasteHavaleh"] = Math.Round(Convert.ToDouble(Table.Rows[i]["TtedadBasteHavaleh"]), 3);
                    Table.Rows[i]["tedadCartonResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["tedadCartonResid"]), 3);
                    Table.Rows[i]["tedadCartonHavaleh"] = Math.Round(Convert.ToDouble(Table.Rows[i]["tedadCartonHavaleh"]), 3);
                    Table.Rows[i]["TtedadCartonResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["TtedadCartonResid"]), 3);
                    Table.Rows[i]["TtedadCartonHavaleh"] = Math.Round(Convert.ToDouble(Table.Rows[i]["TtedadCartonHavaleh"]), 3);
                    Table.Rows[i]["ResidWeight"] = Math.Round(Convert.ToDouble(Table.Rows[i]["ResidWeight"]), 3);
                    Table.Rows[i]["HavaleWeight"] = Math.Round(Convert.ToDouble(Table.Rows[i]["HavaleWeight"]), 3);





                    _TedadMande = Math.Round(_TedadMande + (Convert.ToDouble(Table.Rows[i]["TedadResid"]) -
                        Convert.ToDouble(Table.Rows[i]["TedadHavale"])), 3);

                    _bastemande = Math.Round(_bastemande + Convert.ToDouble(Table.Rows[i]["tedadBasteResid"]) -
                       Convert.ToDouble(Table.Rows[i]["tedadBasteHavaleh"]), 3);

                    _Tbastemande = Math.Round(_Tbastemande + Convert.ToDouble(Table.Rows[i]["TtedadBasteResid"]) -
                      Convert.ToDouble(Table.Rows[i]["TtedadBasteHavaleh"]), 3);

                    _cartonmande = Math.Round(_cartonmande + Convert.ToDouble(Table.Rows[i]["tedadCartonResid"]) -
                      Convert.ToDouble(Table.Rows[i]["tedadCartonHavaleh"]), 3);

                    _Tcartonmande = Math.Round(_Tcartonmande + Convert.ToDouble(Table.Rows[i]["TtedadCartonResid"]) -
                      Convert.ToDouble(Table.Rows[i]["TtedadCartonHavaleh"]), 3);

                    _WeightMande = Math.Round(_WeightMande + Convert.ToDouble(Table.Rows[i]["ResidWeight"]) -
                        Convert.ToDouble(Table.Rows[i]["HavaleWeight"]), 3);


                    Table.Rows[i]["MandeWeight"] = _WeightMande;
                    Table.Rows[i]["TedadMandeh"] = _TedadMande;
                    Table.Rows[i]["tedadCartonMadeh"] = _cartonmande;
                    Table.Rows[i]["TtedadCartonMadeh"] = _Tcartonmande;
                    Table.Rows[i]["tedadBasteMandeh"] = _bastemande;
                    Table.Rows[i]["TtedadBasteMandeh"] = _Tbastemande;


                    if (chk_round.Checked)
                    {
                        Table.Rows[i]["ArzeshKolResid"] = Math.Round(Convert.ToDouble(Table.Rows[i]["ArzeshKolResid"]));
                        Table.Rows[i]["ArzeshKolHavale"] = Math.Round(Convert.ToDouble(Table.Rows[i]["ArzeshKolHavale"]));
                    }

                    _ArzeshKolMandeh += Convert.ToDouble(Table.Rows[i]["ArzeshKolResid"]) -
                        Convert.ToDouble(Table.Rows[i]["ArzeshKolHavale"]);
                    if (_ArzeshKolMandeh < 0)
                        _ArzeshKolMandeh = 0;

                    Table.Rows[i]["ArzeshKolMandeh"] = _ArzeshKolMandeh;

                    if (_TedadMande > 0)
                    {
                        Table.Rows[i]["ArzeshVahedMandeh"] = Math.Round(_ArzeshKolMandeh / _TedadMande, 3);
                    }
                    else
                    {
                        Table.Rows[i]["ArzeshVahedMandeh"] = 0;
                        Table.Rows[i]["ArzeshKolMandeh"] = 0;
                    }
                }


                Table.DefaultView.RowFilter = "Tarikh>='" + Date1 + "'";
                if (Table.Rows.Count - Table.DefaultView.Count > 0)
                {
                    DataRow Row = Table.Rows[Table.Rows.Count - Table.DefaultView.Count - 1];

                    BindSource.CurrencyManager.AddNew();
                    FirstRow = (DataRowView)BindSource.CurrencyManager.Current;
                    FirstRow["Kala"] = Table.Rows[0]["Kala"].ToString();
                    FirstRow["TedadResid"] = 0;
                    FirstRow["ArzeshVahedResid"] = 0;
                    FirstRow["ArzeshKolResid"] = 0;
                    FirstRow["TedadHavale"] = 0;
                    FirstRow["ArzeshVahedHavale"] = 0;
                    FirstRow["ArzeshKolHavale"] = 0;
                    FirstRow["Tarikh"] = "مانده از قبل";
                    FirstRow["TedadMandeh"] = Row["TedadMandeh"];
                    FirstRow["MandeWeight"] = Row["MandeWeight"];
                    FirstRow["ArzeshVahedMandeh"] = Row["ArzeshVahedMandeh"];
                    FirstRow["ArzeshKolMandeh"] = Row["ArzeshKolMandeh"];
                    FirstRow["tedadCartonMadeh"] = Row["tedadCartonMadeh"];
                    FirstRow["TtedadCartonMadeh"] = Row["TtedadCartonMadeh"];
                    FirstRow["tedadBasteMandeh"] = Row["tedadBasteMandeh"];
                    FirstRow["TtedadBasteMandeh"] = Row["TtedadBasteMandeh"];

                    DataRow DRow = FirstRow.Row;
                    Table.Rows.InsertAt(DRow, 0);
                }
                gridEX2.DataSource = BindSource;
                gridEX1.DataSource = BindSource;

                bindingNavigator1.BindingSource = BindSource;
            }
        }


        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (chk_ontatal.Checked)
            {
                gridEXExporter1.GridEX = gridEX1;
            }
            else
            {
                gridEXExporter1.GridEX = gridEX2;

            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    faDatePickerStrip2.FADatePicker.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void Frm_010_KardexRiyali_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                mlt_Ware.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                buttonX1_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void uiComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.EditControls.UIComboBox)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.EditControls.UIComboBox)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        //private void editBox1_Enter(object sender, EventArgs e)
        //{
        //    if (mlt_Good.Text.Trim() == "")
        //    {
        //        txt_MinStock.Text = "0";
        //        txt_MaxStock.Text = "0";
        //        return;
        //    }

        //    try
        //    {
        //        txt_MinStock.Text = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "SELECT HadeaghalMojoodi FROM dbo.GetCommodityChanges(" + mlt_Good.Value.ToString() +
        //           ", '" + Date2 + "') AS GetCommodityChanges_2").Rows[0][0].ToString();


        //        txt_MaxStock.Text = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "SELECT HadeAxarMojoodi FROM dbo.GetCommodityChanges(" + mlt_Good.Value.ToString() +
        //            ", '" + Date2 + "') AS GetCommodityChanges_2").Rows[0][0].ToString();
        //    }
        //    catch
        //    {
        //    }

        //}

        private void faDatePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    gridEX_Goods.MoveFirst();
                    gridEX_Goods.Select();
                    gridEX_Goods.Focus();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (chk_ontatal.Checked)
            {

                if (gridEX1.RowCount > 0)
                {
                    DataTable Table = dataSet_001_Gozareshat.Rpt_Riali_Cardex.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {
                        Table.Rows.Add(gridEX1.GetRow().Cells["GoodCode"].Text,
                            gridEX1.GetRow().Cells["Kala"].Text,
                            mlt_Ware.Text,
                            item.Cells["Type"].Text,
                            item.Cells["Number"].Value.ToString(),
                            item.Cells["Date"].Value.ToString(),
                            null,
                            null,
                            item.Cells["InNumber"].Value.ToString(),
                            item.Cells["InValue"].Value.ToString(),
                            item.Cells["InTotalValue"].Value.ToString(),
                            item.Cells["OutNumber"].Value.ToString(),
                            item.Cells["OutValue"].Value.ToString(),
                            item.Cells["OutTotalValue"].Value.ToString(),
                            item.Cells["RemainNumber"].Value.ToString(),
                            item.Cells["RemainValue"].Value.ToString(),
                            item.Cells["RemainTotalValue"].Value.ToString(),
                            gridEX_Goods.GetRow().Cells["UnitCount"].Text);
                    }
                    Report.Form01_ReportForm frm = new Form01_ReportForm(Table, 8, "از تاریخ: " + Date1, "تا تاریخ: " + Date2);
                    frm.ShowDialog();
                }
            }
            else
            {
                if (gridEX2.RowCount > 0)
                {
                    DataTable Table = dataSet_001_Gozareshat.Rpt_Riali_Cardex.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                    {
                        Table.Rows.Add(gridEX2.GetRow().Cells["GoodCode"].Text,
                            gridEX2.GetRow().Cells["Kala"].Text,
                            mlt_Ware.Text,
                            item.Cells["Type"].Text,
                            item.Cells["Number"].Value.ToString(),
                            item.Cells["Date"].Value.ToString(),
                            null,
                            null,
                            item.Cells["InNumber"].Value.ToString(),
                            item.Cells["InValue"].Value.ToString(),
                            item.Cells["InTotalValue"].Value.ToString(),
                            item.Cells["OutNumber"].Value.ToString(),
                            item.Cells["OutValue"].Value.ToString(),
                            item.Cells["OutTotalValue"].Value.ToString(),
                            item.Cells["RemainNumber"].Value.ToString(),
                            item.Cells["RemainValue"].Value.ToString(),
                            item.Cells["RemainTotalValue"].Value.ToString(),
                            gridEX_Goods.GetRow().Cells["UnitCount"].Text);
                    }
                    Report.Form01_ReportForm frm = new Form01_ReportForm(Table, 8, "از تاریخ: " + Date1, "تا تاریخ: " + Date2);
                    frm.ShowDialog();
                }
            }
        }

        private void txt_MinStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                buttonX1_Click(sender, e);
        }

        private void Frm_010_KardexRiyali_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Properties.Settings.Default.KardexRiyali = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
                Properties.Settings.Default.Save();
            }
            gridEX2.RootTable.Caption = null;
            gridEX2.RemoveFilters();
            gridEX1.RemoveFilters();

        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    faDatePickerStrip1.FADatePicker.Select();
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    faDatePickerStrip1.FADatePicker.Select();
            }
        }

        private void gridEX_Goods_KeyPress(object sender, KeyPressEventArgs e)

        {
            try
            {
                if (e.KeyChar == 13 && gridEX_Goods.CurrentRow != null)
                {
                    if (chk_ontatal.Checked)
                    {
                        gridEX1.Visible = true;
                        gridEX2.Visible = false;
                        buttonX1_Click(sender, e);
                        gridEX1.RootTable.Caption = "کد کالا: " + gridEX_Goods.GetValue("GoodCode").ToString() + "--- نام کالا: " + gridEX_Goods.GetValue("GoodName").ToString();
                    }
                    else
                    {
                        gridEX1.Visible = false;
                        gridEX2.Visible = true;
                        buttonX1_Click(sender, e);
                        gridEX2.RootTable.Caption = "کد کالا: " + gridEX_Goods.GetValue("GoodCode").ToString() + "--- نام کالا: " + gridEX_Goods.GetValue("GoodName").ToString();
                    }
                }
            }
            catch { }
        }

        private void gridEX2_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            try
            {

                if (chk_ontatal.Checked)
                {
                    //باز کردن رسید/حواله مربوط به سطر
                    if (gridEX1.GetValue("Type").ToString() == "0")
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 23))
                        {
                            foreach (Form item in Application.OpenForms)
                            {
                                if (item.Name == "Form32_RialiReceipt")
                                {
                                    item.BringToFront();
                                    PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt frm = (PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt)item;
                                    frm.txt_Search.Text = gridEX1.GetValue("Number").ToString();
                                    frm.bt_Search_Click(sender, e);
                                    return;
                                }
                            }
                            PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt frms = new PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt(
                                UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                                Convert.ToInt32(gridEX1.GetValue("columnid")));
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
                    else
                    {

                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
                        {
                            foreach (Form item in Application.OpenForms)
                            {
                                if (item.Name == "Form34_RialiDrafts")
                                {
                                    item.BringToFront();
                                    PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts frm = (PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts)item;
                                    frm.txt_Search.Text = gridEX1.GetValue("Number").ToString();
                                    frm.bt_Search_Click(sender, e);
                                    return;
                                }
                            }
                            PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts frms = new PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts(
                                UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25),
                                Convert.ToInt32(gridEX1.GetValue("columnid")));
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



                }
                else
                {
                    //باز کردن رسید/حواله مربوط به سطر
                    if (gridEX2.GetValue("Type").ToString() == "0")
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 23))
                        {
                            foreach (Form item in Application.OpenForms)
                            {
                                if (item.Name == "Form32_RialiReceipt")
                                {
                                    item.BringToFront();
                                    PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt frm = (PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt)item;
                                    frm.txt_Search.Text = gridEX2.GetValue("Number").ToString();
                                    frm.bt_Search_Click(sender, e);
                                    return;
                                }
                            }
                            PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt frms = new PWHRS._03_AmaliyatAnbar.Form32_RialiReceipt(
                                UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                                Convert.ToInt32(gridEX2.GetValue("columnid")));
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
                    else
                    {

                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
                        {
                            foreach (Form item in Application.OpenForms)
                            {
                                if (item.Name == "Form34_RialiDrafts")
                                {
                                    item.BringToFront();
                                    PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts frm = (PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts)item;
                                    frm.txt_Search.Text = gridEX2.GetValue("Number").ToString();
                                    frm.bt_Search_Click(sender, e);
                                    return;
                                }
                            }
                            PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts frms = new PWHRS._03_AmaliyatAnbar.Form34_RialiDrafts(
                                UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25),
                                Convert.ToInt32(gridEX2.GetValue("columnid")));
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
                }
            }
            catch
            {
            }
        }

        private void chk_round_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.kartexround = this.chk_round.Checked;
            Properties.Settings.Default.chk_Total = this.chk_ontatal.Checked;

            Properties.Settings.Default.Save();
        }

        private void چاپجدولToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    string j = "از تاریخ: " + Date1 + "تا تاریخ: " + Date2;
                    gridEXPrintDocument1.PageHeaderLeft = j;
                    gridEXPrintDocument1.PageHeaderRight = " انبار: " + mlt_Ware.Text + " " + " کالا: " + gridEX_Goods.GetValue("GoodName");
                    gridEXPrintDocument1.PageHeaderCenter = "کارتکس ریالی";
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void mlt_Ware_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "Column02", null);
            }
            catch
            {
            }
        }

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch
            {
            }
        }



    }
}
