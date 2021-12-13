using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace PCLOR.Report
{
    public partial class Frm_009_KardexTedadi : Form
    {
        bool _BackSpace = false;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
       

        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);

        string Date1, Date2;
        SqlDataAdapter Adapter;
        DataTable Table;
       Classes. Class_UserScope UserScope = new Classes. Class_UserScope();
        DateTime? _Date1 = null, _Date2 = null;
        int _GoodId = 0;

        public Frm_009_KardexTedadi()
        {
            InitializeComponent();
        }
        public Frm_009_KardexTedadi(int GoodId, DateTime? Date1, DateTime? Date2)
        {
            InitializeComponent();
            _Date1 = Date1;
            _Date2 = Date2;
            _GoodId = GoodId;


        }
        private void Frm_010_KardexRiyali_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatePaye1.table_004_CommodityAndIngredients' table. You can move, or remove it, as needed.
            try
            {
                if (_GoodId == 0)
                {
                    string[] Dates = Properties.Settings.Default.KardexTedadi.Split('-');
                    faDatePickerStrip1.FADatePicker.SelectedDateTime =
                        FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
                    faDatePickerStrip2.FADatePicker.SelectedDateTime =DateTime.Now;
                    //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
                    //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
                }
                else
                {
                    faDatePickerStrip1.FADatePicker.SelectedDateTime = _Date1;
                    faDatePickerStrip2.FADatePicker.SelectedDateTime = _Date2;
                }
                chk_ontatal.Checked = Properties.Settings.Default.chk_Total;

                
            this.table_004_CommodityAndIngredientsTableAdapter.Fill(this.dataSet_EtelaatePaye1.table_004_CommodityAndIngredients);

                gridEX2.DropDowns["Good"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");
                gridEX1.DropDowns["Good"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");

                gridEX2.DropDowns["GoodCode"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");
                gridEX1.DropDowns["GoodCode"].SetDataBinding(table_004_CommodityAndIngredientsBindingSource, "");

                gridEX2.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase,
                    "Select ColumnId,Column02 from Table_045_PersonInfo"), "");
                gridEX1.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase,
                    "Select ColumnId,Column02 from Table_045_PersonInfo"), "");
                gridEX2.DropDowns["Center"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo");
                gridEX1.DropDowns["Center"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo");

                gridEX2.DropDowns["Project"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");
                gridEX1.DropDowns["Project"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");


               gridEX1.DropDowns["NumberProduct"].DataSource = gridEX2.DropDowns["NumberProduct"].DataSource = clDoc.ReturnTable(ConPCLOR, @"Select ID,Number from Table_035_Production");

                gridEX_Goods.DropDowns["CountUnit"].DataSource = clDoc.ReturnTable(ConBase, "Select * from Table_070_CountUnitInfo");
                DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select ColumnId,Column02 from Table_001_PWHRS 
                                                                                        where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')
                                                                                        UNION ALL Select 0,'همه انبارها' order by ColumnId");


                //            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                //            DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                //                                                             'True'='" + isadmin.ToString() +
                //                                                    @"'  or 
                //                                                               Columnid IN 
                //                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");




                mlt_Ware.DataSource = WareTable;
                mlt_Ware.Value = 0;
                gridEX2.DropDowns["Ware"].SetDataBinding(clDoc.ReturnTable(ConWare, "Select ColumnId,Column02 from Table_001_PWHRS"), "");
                gridEX1.DropDowns["Ware"].SetDataBinding(clDoc.ReturnTable(ConWare, "Select ColumnId,Column02 from Table_001_PWHRS"), "");

                mlt_Ware.Select();
                mlt_Ware.Focus();

                if (_GoodId != 0)
                {
                    gridEX_Goods.FindAll(gridEX_Goods.RootTable.Columns["GoodID"], Janus.Windows.GridEX.ConditionOperator.Equal, _GoodId);
                    buttonX1_Click(sender, e);
                }
            }
            catch { }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            txt_BasteHavale.Value = 0;
            txt_BasteMande.Value = 0;
            txt_BasteResid.Value = 0;
            txt_CartonHavale.Value = 0;
            txt_CartonMande.Value = 0;
            txt_CartonResid.Value = 0;
            txt_Count.Value = 0;
            txt_KolHavale.Value = 0;
            txt_KolMande.Value = 0;
            txt_KolResid.Value = 0;
            if (mlt_Ware.Text.Trim() == "")
            {
                MessageBox.Show("انبار مورد نظر را انتخاب کنید");
                return;
            }

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                string s = string.Empty;
                Classes.Class_Documents clDocument = new Classes.Class_Documents();

                if (clDocument.ReturnTable(ConWare, @"SELECT NAME
                    FROM master.dbo.sysdatabases 
                    WHERE ('[' + name + ']' = '" + ConSale.Database + @"' 
                    OR name = '" + ConSale.Database + @"')").Rows.Count > 0)
                {
                    s = @" 

        SELECT *
        FROM   (
           SELECT  buy.column01 as Factor, dbo.Table_011_PwhrsReceipt.column06 as Des,dbo.Table_011_PwhrsReceipt.column03 AS Ware,
                  dbo.Table_011_PwhrsReceipt.column05 AS Person,
                  dbo.Table_012_Child_PwhrsReceipt.column02 AS Kala,
                  0 AS TYPE,
                  dbo.Table_011_PwhrsReceipt.columnid AS Id,
                  dbo.Table_011_PwhrsReceipt.column01 AS No,
                  dbo.Table_011_PwhrsReceipt.column02 AS Date,
                  dbo.table_005_PwhrsOperation.column02 AS OP,
                  DocTable.Column00 AS Sanad,
                  dbo.Table_012_Child_PwhrsReceipt.column03 AS Vahed,
                  dbo.Table_012_Child_PwhrsReceipt.column04 AS CartoonResid,
                  dbo.Table_012_Child_PwhrsReceipt.column05 AS BasteResid,
                   cast(ROUND((
                      ISNULL(dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                          (
                              SELECT tcai.column09
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                          ),
                          0
                      ),0)
                  ) ,3)AS DECIMAL(36,3)) AS TCartoonResid,
                   cast(ROUND((
                      ISNULL(dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                          (
                              SELECT tcai.column08
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                          ),
                          0
                      ),0)
                  ) ,3)AS DECIMAL(36,3)) AS TBasteResid,
                dbo.Table_012_Child_PwhrsReceipt.column07 AS KolResid,
                  dbo.Table_012_Child_PwhrsReceipt.column20 AS ValueResid,
                  dbo.Table_012_Child_PwhrsReceipt.column21 AS ValueKolResid,
                  0.000 AS CartoonHavaleh,
                  0.000 AS BasteHavaleh,
                  0.000 AS TCartoonHavaleh,
                  0.000 AS TBasteHavaleh,
                  0.000 AS KolHavaleh,
                  0.000 AS ValueHavaleh,
                  0.000 AS ValueKolHavaleh,
                  0.000 AS MandehCartoon,
                  0.000 as MandehBaste ,
                  0.000 as  TMandehCarton ,
                  0.000 as TMandehBaste ,
                  0.000 AS MandehKol,
                  0.000 AS ValueMandeh,
                  0.000 AS ValueKolMandeh,
                  dbo.Table_012_Child_PwhrsReceipt.Column30,
                  dbo.Table_012_Child_PwhrsReceipt.Column31,
                  Table_012_Child_PwhrsReceipt.column13 AS Center,
                  Table_012_Child_PwhrsReceipt.column14 AS Project,
                  Table_011_PwhrsReceipt.Column09 AS MiladiDate,
                  Table_012_Child_PwhrsReceipt.column07 * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_012_Child_PwhrsReceipt.column02
                          ),0)   AS WeightResid,
                  0 AS WeightHavale,
                  0.000 AS WeightMande,
            dbo.Table_012_Child_PwhrsReceipt.Column36 as Brand,
           dbo.Table_012_Child_PwhrsReceipt.Column37 as Supplyer,
   Table_012_Child_PwhrsReceipt.Column30 as Seri,
                             Table_012_Child_PwhrsReceipt.Column31 as ExpireDate ,Table_012_Child_PwhrsReceipt.column12  as gooddesc  
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

           FROM   dbo.Table_011_PwhrsReceipt
                    left join " + ConSale.Database + @".dbo.Table_015_BuyFactor buy on buy.columnid=dbo.Table_011_PwhrsReceipt.column13
                  INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                       ON  dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                  INNER JOIN dbo.table_005_PwhrsOperation
                       ON  dbo.Table_011_PwhrsReceipt.column04 = dbo.table_005_PwhrsOperation.columnid
                  LEFT OUTER JOIN (
                           SELECT ColumnId,
                                  Column00
                           FROM   {4}.dbo.Table_060_SanadHead
                       ) AS DocTable
                       ON  DocTable.ColumnId = Table_011_PwhrsReceipt.column07
           WHERE  (
                      dbo.Table_012_Child_PwhrsReceipt.column02 = {3}
                      AND (dbo.Table_011_PwhrsReceipt.column02 >= '{1}')
                      AND (dbo.Table_011_PwhrsReceipt.column02 <= '{2}')
                  )
                 " + (mlt_Ware.Value.ToString() == "0" ? "AND   dbo.Table_011_PwhrsReceipt.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')" : " AND (dbo.Table_011_PwhrsReceipt.column03 = {0})") + @"
                ) as Resid
                UNION ALL
               SELECT *
FROM   (
           SELECT buy.column01 as Factor,dbo.Table_007_PwhrsDraft.column06 as Des,dbo.Table_007_PwhrsDraft.column03 AS Ware,
                  dbo.Table_007_PwhrsDraft.column05 AS Person,
                  dbo.Table_008_Child_PwhrsDraft.column02 AS Kala,
                  1 AS TYPE,
                  dbo.Table_007_PwhrsDraft.columnid AS Id,
                  dbo.Table_007_PwhrsDraft.column01 AS No,
                  dbo.Table_007_PwhrsDraft.column02 AS Date,
                  dbo.table_005_PwhrsOperation.column02 AS OP,
                  DocTable.Column00 AS Sanad,
                  dbo.Table_008_Child_PwhrsDraft.column03 AS Vahed,
                  0.000 AS CartoonResid,
                  0.000 AS BasteResid,
                    0.000 AS TCartoonResid,
                  0.000 AS TBasteResid,
                  0.000 AS KolResid,
                  0.000 AS ValueResid,
                  0.000 AS ValueKolResid,
                  dbo.Table_008_Child_PwhrsDraft.column04 AS CartoonHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column05 AS BaasteHavaleh,
                cast(ROUND( (
                     ISNULL( dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                          (
                              SELECT tcai.column09
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                          ),
                          0
                      ),0)
                  ),3)AS DECIMAL(36,3)) AS TCartoonHavaleh,
                 cast(ROUND( (
                     ISNULL( dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                          (
                              SELECT tcai.column08
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                          ),
                          0
                      ),0)
                  ) ,3)AS DECIMAL(36,3)) AS TBaasteHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column07 AS KolHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column15 AS ValueHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column16 AS ValueKolHavaleh,
                0.000 AS MandehCartoon,
                  0.000 as MandehBaste ,
                  0.000 as  TMandehCarton ,
                  0.000 as TMandehBaste ,
                  0.000 AS MandehKol,
                  0.000 AS ValueMandeh,
                  0.000 AS ValueKolMandeh,
                  Table_008_Child_PwhrsDraft.Column30,
                 Table_008_Child_PwhrsDraft. Column31,
                  Table_008_Child_PwhrsDraft.column13 AS Center,
                  Table_008_Child_PwhrsDraft.column14 AS Project,
                  Table_007_PwhrsDraft.Column09 AS MiladiDate,
                  0 AS WeightResid,
                  Table_008_Child_PwhrsDraft.column07 * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_008_Child_PwhrsDraft.column02
                          ),0) AS WeightHavale,
                  0.000 AS WeightMande,
 dbo.Table_008_Child_PwhrsDraft.Column36 as Brand,
           dbo.Table_008_Child_PwhrsDraft.Column37 as Supplyer,
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



           FROM   dbo.table_005_PwhrsOperation
                  INNER JOIN dbo.Table_007_PwhrsDraft
  left join " + ConSale.Database + @".dbo.Table_010_SaleFactor buy on buy.columnid=dbo.Table_007_PwhrsDraft.column16
                       ON  dbo.table_005_PwhrsOperation.columnid = dbo.Table_007_PwhrsDraft.column04
                  INNER JOIN dbo.Table_008_Child_PwhrsDraft
                       ON  dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                  LEFT OUTER JOIN (
                           SELECT ColumnId,
                                  column00
                           FROM   {4}.dbo.Table_060_SanadHead
                       ) AS DocTable
                       ON  DocTable.ColumnId = Table_007_PwhrsDraft.column07
           WHERE  (dbo.Table_008_Child_PwhrsDraft.column02 = {3})
                  AND (dbo.Table_007_PwhrsDraft.column02 >= '{1}')
                  AND (dbo.Table_007_PwhrsDraft.column02 <= '{2}')
                  " + (mlt_Ware.Value.ToString() == "0" ? "AND  dbo.Table_007_PwhrsDraft.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')" : " And  (dbo.Table_007_PwhrsDraft.column03 = {0}) ") + @"
                ) as Havaleh

                order by Date,Type,Id";
                }
                else
                {
                  s=  @" 

SELECT *
FROM   (
           SELECT  0 as Factor, dbo.Table_011_PwhrsReceipt.column06 as Des,dbo.Table_011_PwhrsReceipt.column03 AS Ware,
                  dbo.Table_011_PwhrsReceipt.column05 AS Person,
                  dbo.Table_012_Child_PwhrsReceipt.column02 AS Kala,
                  0 AS TYPE,
                  dbo.Table_011_PwhrsReceipt.columnid AS Id,
                  dbo.Table_011_PwhrsReceipt.column01 AS No,
                  dbo.Table_011_PwhrsReceipt.column02 AS Date,
                  dbo.table_005_PwhrsOperation.column02 AS OP,
                  DocTable.Column00 AS Sanad,
                  dbo.Table_012_Child_PwhrsReceipt.column03 AS Vahed,
                  dbo.Table_012_Child_PwhrsReceipt.column04 AS CartoonResid,
                  dbo.Table_012_Child_PwhrsReceipt.column05 AS BasteResid,
                   cast(ROUND((
                      ISNULL(dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                          (
                              SELECT tcai.column09
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                          ),
                          0
                      ),0)
                  ) ,3)AS DECIMAL(36,3)) AS TCartoonResid,
                   cast(ROUND((
                      ISNULL(dbo.Table_012_Child_PwhrsReceipt.column07 / NULLIF(
                          (
                              SELECT tcai.column08
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                          ),
                          0
                      ),0)
                  ) ,3)AS DECIMAL(36,3)) AS TBasteResid,
                dbo.Table_012_Child_PwhrsReceipt.column07 AS KolResid,
                  dbo.Table_012_Child_PwhrsReceipt.column20 AS ValueResid,
                  dbo.Table_012_Child_PwhrsReceipt.column21 AS ValueKolResid,
                  0.000 AS CartoonHavaleh,
                  0.000 AS BasteHavaleh,
                  0.000 AS TCartoonHavaleh,
                  0.000 AS TBasteHavaleh,
                  0.000 AS KolHavaleh,
                  0.000 AS ValueHavaleh,
                  0.000 AS ValueKolHavaleh,
                  0.000 AS MandehCartoon,
                  0.000 as MandehBaste ,
                  0.000 as  TMandehCarton ,
                  0.000 as TMandehBaste ,
                  0.000 AS MandehKol,
                  0.000 AS ValueMandeh,
                  0.000 AS ValueKolMandeh,
                  dbo.Table_012_Child_PwhrsReceipt.Column30,
                  dbo.Table_012_Child_PwhrsReceipt.Column31,
                  Table_012_Child_PwhrsReceipt.column13 AS Center,
                  Table_012_Child_PwhrsReceipt.column14 AS Project,
                  Table_011_PwhrsReceipt.Column09 AS MiladiDate,
                  Table_012_Child_PwhrsReceipt.column07 * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_012_Child_PwhrsReceipt.column02
                          ),0)   AS WeightResid,
                  0 AS WeightHavale,
                  0.000 AS WeightMande,
            dbo.Table_012_Child_PwhrsReceipt.Column36 as Brand,
           dbo.Table_012_Child_PwhrsReceipt.Column37 as Supplyer,
   Table_012_Child_PwhrsReceipt.Column30 as Seri,
                             Table_012_Child_PwhrsReceipt.Column31 as ExpireDate ,Table_012_Child_PwhrsReceipt.column12  as gooddesc

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


           FROM   dbo.Table_011_PwhrsReceipt
                    
                  INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                       ON  dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                  INNER JOIN dbo.table_005_PwhrsOperation
                       ON  dbo.Table_011_PwhrsReceipt.column04 = dbo.table_005_PwhrsOperation.columnid
                  LEFT OUTER JOIN (
                           SELECT ColumnId,
                                  Column00
                           FROM   {4}.dbo.Table_060_SanadHead
                       ) AS DocTable
                       ON  DocTable.ColumnId = Table_011_PwhrsReceipt.column07
           WHERE  (
                      dbo.Table_012_Child_PwhrsReceipt.column02 = {3}
                      AND (dbo.Table_011_PwhrsReceipt.column02 >= '{1}')
                      AND (dbo.Table_011_PwhrsReceipt.column02 <= '{2}')
                  )
                 " + (mlt_Ware.Value.ToString() == "0" ? "AND   dbo.Table_011_PwhrsReceipt.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')" : " AND (dbo.Table_011_PwhrsReceipt.column03 = {0})") + @"
                ) as Resid
                UNION ALL
               SELECT *
FROM   (
           SELECT 0 as Factor,dbo.Table_007_PwhrsDraft.column06 as Des,dbo.Table_007_PwhrsDraft.column03 AS Ware,
                  dbo.Table_007_PwhrsDraft.column05 AS Person,
                  dbo.Table_008_Child_PwhrsDraft.column02 AS Kala,
                  1 AS TYPE,
                  dbo.Table_007_PwhrsDraft.columnid AS Id,
                  dbo.Table_007_PwhrsDraft.column01 AS No,
                  dbo.Table_007_PwhrsDraft.column02 AS Date,
                  dbo.table_005_PwhrsOperation.column02 AS OP,
                  DocTable.Column00 AS Sanad,
                  dbo.Table_008_Child_PwhrsDraft.column03 AS Vahed,
                  0.000 AS CartoonResid,
                  0.000 AS BasteResid,
                    0.000 AS TCartoonResid,
                  0.000 AS TBasteResid,
                  0.000 AS KolResid,
                  0.000 AS ValueResid,
                  0.000 AS ValueKolResid,
                  dbo.Table_008_Child_PwhrsDraft.column04 AS CartoonHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column05 AS BaasteHavaleh,
                cast(ROUND( (
                     ISNULL( dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                          (
                              SELECT tcai.column09
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                          ),
                          0
                      ),0)
                  ),3)AS DECIMAL(36,3)) AS TCartoonHavaleh,
                 cast(ROUND( (
                     ISNULL( dbo.Table_008_Child_PwhrsDraft.column07 / NULLIF(
                          (
                              SELECT tcai.column08
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                          ),
                          0
                      ),0)
                  ) ,3)AS DECIMAL(36,3)) AS TBaasteHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column07 AS KolHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column15 AS ValueHavaleh,
                  dbo.Table_008_Child_PwhrsDraft.column16 AS ValueKolHavaleh,
                0.000 AS MandehCartoon,
                  0.000 as MandehBaste ,
                  0.000 as  TMandehCarton ,
                  0.000 as TMandehBaste ,
                  0.000 AS MandehKol,
                  0.000 AS ValueMandeh,
                  0.000 AS ValueKolMandeh,
                  Table_008_Child_PwhrsDraft.Column30,
                 Table_008_Child_PwhrsDraft. Column31,
                  Table_008_Child_PwhrsDraft.column13 AS Center,
                  Table_008_Child_PwhrsDraft.column14 AS Project,
                  Table_007_PwhrsDraft.Column09 AS MiladiDate,
                  0 AS WeightResid,
                  Table_008_Child_PwhrsDraft.column07 * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_008_Child_PwhrsDraft.column02
                          ),0) AS WeightHavale,
                  0.000 AS WeightMande,
 dbo.Table_008_Child_PwhrsDraft.Column36 as Brand,
           dbo.Table_008_Child_PwhrsDraft.Column37 as Supplyer,
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






           FROM   dbo.table_005_PwhrsOperation
                  INNER JOIN dbo.Table_007_PwhrsDraft
                       ON  dbo.table_005_PwhrsOperation.columnid = dbo.Table_007_PwhrsDraft.column04
                  INNER JOIN dbo.Table_008_Child_PwhrsDraft
                       ON  dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                  LEFT OUTER JOIN (
                           SELECT ColumnId,
                                  column00
                           FROM   {4}.dbo.Table_060_SanadHead
                       ) AS DocTable
                       ON  DocTable.ColumnId = Table_007_PwhrsDraft.column07
           WHERE  (dbo.Table_008_Child_PwhrsDraft.column02 = {3})
                  AND (dbo.Table_007_PwhrsDraft.column02 >= '{1}')
                  AND (dbo.Table_007_PwhrsDraft.column02 <= '{2}')
                  " + (mlt_Ware.Value.ToString() == "0" ? "AND  dbo.Table_007_PwhrsDraft.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')" : " And  (dbo.Table_007_PwhrsDraft.column03 = {0}) ") + @"
                ) as Havaleh

                order by Date,Type,Id";
                }


                s = string.Format(s, mlt_Ware.Value.ToString(), Date1, Date2, gridEX_Goods.GetValue("GoodID").ToString(), ConAcnt.Database);


                Adapter = new SqlDataAdapter(s, ConWare);
                Table = new DataTable();
                Adapter.Fill(Table);


                BindingSource BindSource = new BindingSource();
                BindSource.DataSource = Table;
                bindingNavigator1.BindingSource = BindSource;

                double _MandehValueKol = 0, _MandehKol = 0, _MandehCartoon = 0, _TMandehCartoon = 0, _TMandehPack = 0, _MandehPack = 0, _MandeWeight = 0;

                //مانده از قبل
                DataTable RemainTable = clDoc.LastGoodRemain(mlt_Ware.Value.ToString(), Date1, gridEX_Goods.GetValue("GoodID").ToString());
                if (RemainTable.Rows.Count > 0 && (Convert.ToDouble(RemainTable.Rows[0]["Box"].ToString()) != 0 || Convert.ToDouble(RemainTable.Rows[0]["Total"].ToString()) != 0))
                {

                    BindSource.CurrencyManager.AddNew();
                    DataRowView Row = (DataRowView)BindSource.CurrencyManager.Current;
                    Row["Kala"] = RemainTable.Rows[0]["GoodCode"].ToString();
                    Row["Date"] = "مانده از قبل";
                    //Row["MandehCartoon"] = RemainTable.Rows[0]["Box"].ToString();
                    Row["MandehKol"] = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["Total"].ToString()), 3);
                    Row["WeightMande"] = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["TotalWeight"].ToString()), 3);
                    Row["MandehCartoon"] = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["Box"].ToString()), 3);
                    Row["TMandehCarton"] = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["TBox"].ToString()), 3);
                    Row["MandehBaste"] = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["Pack"].ToString()), 3);
                    Row["TMandehBaste"] = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["TPack"].ToString()), 3);
                    Row["KolResid"] = 0;
                    Row["ValueResid"] = 0;
                    Row["ValueKolResid"] = 0;

                    Row["KolHavaleh"] = 0;
                    Row["ValueHavaleh"] = 0;
                    Row["ValueKolHavaleh"] = 0;

                    DataRow DRow = Row.Row;
                    Table.Rows.InsertAt(DRow, 0);

                    _MandehKol = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["Total"].ToString()), 3);
                    _MandehCartoon = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["Box"].ToString()), 3);
                    _TMandehCartoon = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["TBox"].ToString()), 3);
                    _TMandehPack = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["TPack"].ToString()), 3);
                    _MandehPack = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["Pack"].ToString()), 3);
                    _MandeWeight = Math.Round(Convert.ToDouble(RemainTable.Rows[0]["TotalWeight"].ToString()), 3);
                }


                for (int i = 0; i < Table.Rows.Count; i++)
                {
                    if (Table.Rows[i]["Date"].ToString() != "مانده از قبل")
                    {
                        _MandehKol += Math.Round(Convert.ToDouble(Table.Rows[i]["KolResid"]) -
                            Convert.ToDouble(Table.Rows[i]["KolHavaleh"]), 3);
                        Table.Rows[i]["MandehKol"] = _MandehKol;


                        _MandeWeight += Math.Round(Convert.ToDouble(Table.Rows[i]["WeightResid"]) -
                           Convert.ToDouble(Table.Rows[i]["WeightHavale"]), 3);
                        Table.Rows[i]["WeightMande"] = _MandeWeight;



                        _MandehCartoon = Math.Round(_MandehCartoon + Convert.ToDouble(Table.Rows[i]["CartoonResid"]) -
                            Convert.ToDouble(Table.Rows[i]["CartoonHavaleh"]), 3);
                        Table.Rows[i]["MandehCartoon"] = _MandehCartoon;

                        _TMandehCartoon = Math.Round(_TMandehCartoon + Convert.ToDouble(Table.Rows[i]["TCartoonResid"]) -
                            Convert.ToDouble(Table.Rows[i]["TCartoonHavaleh"]), 3);
                        Table.Rows[i]["TMandehCarton"] = Math.Round(_TMandehCartoon, 3);

                        _MandehPack = Math.Round(_MandehPack + Convert.ToDouble(Table.Rows[i]["BasteResid"]) -
                              Convert.ToDouble(Table.Rows[i]["BasteHavaleh"]), 3);
                        Table.Rows[i]["MandehBaste"] = _MandehPack;


                        _TMandehPack = Math.Round(_TMandehPack + Convert.ToDouble(Table.Rows[i]["TBasteResid"]) -
                               Convert.ToDouble(Table.Rows[i]["TBasteHavaleh"]), 3);
                        Table.Rows[i]["TMandehBaste"] = Math.Round(_TMandehPack, 3);



                        _MandehValueKol = Math.Round(_MandehValueKol + Convert.ToDouble(Table.Rows[i]["ValueKolResid"]) -
                            Convert.ToDouble(Table.Rows[i]["ValueKolHavaleh"]), 3);
                        Table.Rows[i]["ValueKolMandeh"] = _MandehValueKol;

                        if (_MandehKol > 0)
                        {
                            Table.Rows[i]["ValueMandeh"] = _MandehValueKol / _MandehKol;

                        }

                        else
                        {
                            Table.Rows[i]["ValueMandeh"] = 0;
                            Table.Rows[i]["ValueKolMandeh"] = 0;

                        }
                    }

                }

                gridEX2.DataSource = BindSource;
                gridEX1.DataSource = BindSource;

            }

        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void gridEX2_KeyPress(object sender, KeyPressEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {

            if (chk_ontatal.Checked)
            {
                s.GridEX = gridEX1;
            }
            else
            {
                s.GridEX = gridEX2;

            }


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = ((FileStream)saveFileDialog1.OpenFile());
                s.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

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

        private void mlt_Good_KeyPress(object sender, KeyPressEventArgs e)
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



        private void Frm_009_KardexTedadi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                mlt_Ware.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                buttonX1_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (chk_ontatal.Checked)
            {
                if (gridEX1.RowCount > 0)
                {

                    Report.Form01_ReportForm frm = new Form01_ReportForm(Table, 7,
                        "از تاریخ: " + Date1, "تا تاریخ: " + Date2,
                       mlt_Ware.Text, gridEX1.GetRow().Cells["Kala"].Text,
                       gridEX1.GetRow().Cells["GoodCode"].Text, gridEX_Goods.GetRow().Cells["UnitCount"].Text);
                    frm.ShowDialog();
                }
            }
            else
            {
                if (gridEX2.RowCount > 0)
                {

                    Report.Form01_ReportForm frm = new Form01_ReportForm(Table, 18,
                        "از تاریخ: " + Date1, "تا تاریخ: " + Date2,
                       mlt_Ware.Text, gridEX2.GetRow().Cells["Kala"].Text,
                       gridEX2.GetRow().Cells["GoodCode"].Text, gridEX_Goods.GetRow().Cells["UnitCount"].Text);
                    frm.ShowDialog();
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

        private void txt_MinStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            buttonX1_Click(sender, e);
        }

        private void Frm_009_KardexTedadi_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX2.RemoveFilters();
            gridEX1.RemoveFilters();
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Properties.Settings.Default.KardexTedadi = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
                Properties.Settings.Default.Save();
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
                                Convert.ToInt32(gridEX1.GetValue("Id")));
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
                                Convert.ToInt32(gridEX1.GetValue("Id")));
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
                                Convert.ToInt32(gridEX2.GetValue("Id")));
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
                                Convert.ToInt32(gridEX2.GetValue("Id")));
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

        private void gridEX_Goods_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (chk_ontatal.Checked)
                {

                    if (e.KeyChar == 13 && gridEX_Goods.CurrentRow != null)
                    {
                        gridEX2.Visible = false;
                        gridEX1.Visible = true;
                        buttonX1_Click(sender, e);
                        gridEX1.RootTable.Caption = "کارتکس تعدادی " + gridEX_Goods.GetValue("GoodName").ToString();
                    }
                }
                else
                {
                    if (e.KeyChar == 13 && gridEX_Goods.CurrentRow != null)
                    {
                        gridEX2.Visible = true;
                        gridEX1.Visible = false;
                        buttonX1_Click(sender, e);
                        gridEX2.RootTable.Caption = "کارتکس تعدادی " + gridEX_Goods.GetValue("GoodName").ToString();
                    }
                }
            }
            catch { }
        }

        private void chk_ontatal_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.chk_Total = chk_ontatal.Checked;
            Properties.Settings.Default.Save();
        }

        private void btn_table_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (chk_ontatal.Checked)
                    {
                        gridEXPrintDocument1.GridEX = gridEX1;
                    }
                    else
                    {
                        gridEXPrintDocument1.GridEX = gridEX2;

                    }
                    string j = "از تاریخ: " + Date1 + "تا تاریخ: " + Date2;
                    gridEXPrintDocument1.PageHeaderLeft = j;
                    gridEXPrintDocument1.PageHeaderRight = " انبار: " + mlt_Ware.Text + " " + " کالا: " + gridEX_Goods.GetValue("GoodName");
                    gridEXPrintDocument1.PageHeaderCenter = "کارتکس تعدادی";
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void gridEX2_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                decimal resid = 0;
                decimal havale = 0;
                if (gridEX2.GetCheckedRows().Length > 0)
                {
                    uiGroupBox1.Visible = true;
                    txt_Count.Value = gridEX2.GetCheckedRows().Length;

                    resid = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["KolResid"].Value.ToString()));
                    txt_KolResid.Value = resid;
                    havale = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["KolHavaleh"].Value.ToString()));
                    txt_KolHavale.Value = havale;
                    txt_KolMande.Value = resid - havale;
                    /////
                    resid = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["WeightResid"].Value.ToString()));
                    txt_BasteResid.Value = resid;
                    havale = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["WeightHavale"].Value.ToString()));
                    txt_BasteHavale.Value = havale;
                    txt_BasteMande.Value = resid - havale;
                    ///
                    resid = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["CartoonResid"].Value.ToString()));
                    txt_CartonResid.Value = resid;
                    havale = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["CartoonHavaleh"].Value.ToString()));
                    txt_CartonHavale.Value = havale;
                    txt_CartonMande.Value = resid - havale;
                }
                else uiGroupBox1.Visible = false;


            }
            catch { }
        }

        private void gridEX1_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                decimal resid = 0;
                decimal havale = 0;
                if (gridEX1.GetCheckedRows().Length > 0)
                {
                    uiGroupBox1.Visible = true;
                    txt_Count.Value = gridEX1.GetCheckedRows().Length;

                    resid = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["KolResid"].Value.ToString()));
                    txt_KolResid.Value = resid;
                    havale = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["KolHavaleh"].Value.ToString()));
                    txt_KolHavale.Value = havale;
                    txt_KolMande.Value = resid - havale;
                    /////
                    resid = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["WeightResid"].Value.ToString()));
                    txt_BasteResid.Value = resid;
                    havale = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["WeightHavale"].Value.ToString()));
                    txt_BasteHavale.Value = havale;
                    txt_BasteMande.Value = resid - havale;
                    ///
                    resid = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["TCartoonResid"].Value.ToString()));
                    txt_CartonResid.Value = resid;
                    havale = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["TCartoonHavaleh"].Value.ToString()));
                    txt_CartonHavale.Value = havale;
                    txt_CartonMande.Value = resid - havale;

                }
                else uiGroupBox1.Visible = false;


            }
            catch { }
        }







    }
}
