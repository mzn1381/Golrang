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
    public partial class Frm_003_MojoodiMaghtaiTedadi : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);

        string Date1;
        bool _BackSpace = false;
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
      
        public Frm_003_MojoodiMaghtaiTedadi()
        {
            InitializeComponent();
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (chk_ontatal.Checked)
                gridEXExporter1.GridEX = gridEX1;

            else

                gridEXExporter1.GridEX = gridEX2;
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                FileStream fs = ((FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }

        }

        private void Frm_003_MojoodiMaghtaiTedadi_Load(object sender, EventArgs e)
        {
            // gridEXFieldChooserControl1.GridEX = gridEX2;
            SqlDataAdapter Adapter = new SqlDataAdapter(@"Select * from Table_001_PWHRS
                                                          where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')
                                                                ", ConWare);
            DataTable WareTable = new DataTable();
            Adapter.Fill(WareTable);


            Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            gridEX2.DropDowns["Unit"].DataSource = Table;
            gridEX1.DropDowns["Unit"].DataSource = Table;


            DataRow Row = WareTable.NewRow();
            Row["ColumnId"] = 0;
            Row["Column02"] = "همه انبارها";
            WareTable.Rows.InsertAt(Row, 0);
            mlt_Ware.DropDownDataSource = WareTable;
            mlt_Ware.DropDownList.SetValue("ColumnId", 0);

            faDatePicker1.SelectedDateTime = DateTime.Now;
            chk_ontatal.Checked = Properties.Settings.Default.chk_Total;


        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Ware.DropDownList.GetCheckedRows().Count() == 0)
                {
                    MessageBox.Show("انبار مورد نظر را انتخاب کنید");
                    return;
                }

                if (faDatePicker1.SelectedDateTime.HasValue && faDatePicker1.Text.Trim() != "")
                {
                    Date1 = faDatePicker1.Text;
                    if (chk_ResidDraft.Checked)
                    {
                        if (txt_ResidNumber.Text.Trim() == "" || txt_DraftNumber.Text.Trim() == "")
                            throw new Exception("شماره رسید و حواله را مشخص کنید");

                        CheckResidDraftNumber();
                    }

                    //یک انبار خاص
                    if (mlt_Ware.Text.Trim() != "" && mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() != "0")
                    {

                        string whr = string.Empty;
                        foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Ware.DropDownList.GetCheckedRows())
                        {
                            if (dr.Cells["ColumnId"].Value.ToString() != "0")
                                whr += dr.Cells["ColumnId"].Value + ",";
                        }
                        whr = whr.TrimEnd(',');

                        SqlDataAdapter Adapter = new SqlDataAdapter(


@"  SELECT     TurnTable.GoodCode AS GoodID,TurnTable.Project,Round(SUM(TurnTable.INumberInBox),3) AS INumberInBox,Round(SUM(TurnTable.TINumberInBox),3) AS TINumberInBox, Round(SUM(TurnTable.INumberInPack),3) AS INumberInPack,Round(SUM(TurnTable.TINumberInPack),3) AS TINumberInPack, 
                      Round(SUM(TurnTable.IDetailNumber),3) AS IDetailNumber, Round(SUM(TurnTable.ITotalNumber),3) AS ITotalNumber,Round(SUM(TurnTable.ONumberInBox),3) AS ONumberInBox, Round(SUM(TurnTable.TONumberInBox),3) AS TONumberInBox,
                      Round(SUM(TurnTable.ONumberInPack),3) AS ONumberInPack,Round(SUM(TurnTable.TONumberInPack),3) AS TONumberInPack, Round(SUM(TurnTable.ODetailNumber),3) AS ODetailNumber, Round(SUM(TurnTable.OTotalNumber),3) AS OTotalNumber, 
                      Round(SUM(TurnTable.INumberInBox)- SUM(TurnTable.ONumberInBox),3) AS RNumberInBox,Round(SUM(TurnTable.TINumberInBox)- SUM(TurnTable.TONumberInBox),3) AS TRNumberInBox, Round(SUM(TurnTable.INumberInPack) - SUM(TurnTable.ONumberInPack) ,3)AS RNumberInPack,Round(SUM(TurnTable.TINumberInPack) - SUM(TurnTable.TONumberInPack) ,3)AS TRNumberInPack,
                       Round(SUM(TurnTable.IDetailNumber) - SUM(TurnTable.ODetailNumber),3) AS RDetailNumber,Round( SUM(TurnTable.ITotalNumber) 
                      - SUM(TurnTable.OTotalNumber),3) AS RTotalNumber, dbo.table_004_CommodityAndIngredients.column02 AS GoodName, 
                      dbo.table_004_CommodityAndIngredients.column01 AS GoodCode,dbo.table_004_CommodityAndIngredients.column07 AS UnitCount,dbo.table_004_CommodityAndIngredients.column05 as CompName" +
                    (chk_BuildSeri.Checked ? ",TurnTable.Column30" : "") + (chk_ExpDate.Checked ? ",TurnTable.Column31" : "") + (chk_Brand.Checked ? ",TurnTable.Brand" : "") + (chk_Supplyer.Checked ? ",TurnTable.Supplyer" : "") + @"  , 
                      dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 as MainGroupName,  ROUND(SUM(TurnTable.OSingleWeight), 3) AS OSingleWeight,
                           ROUND(SUM(TurnTable.OTotalWeight), 3) AS OTotalWeight,
                           ROUND(SUM(TurnTable.TOTotalWeight), 3) AS TOTotalWeight,

                           ROUND(SUM(TurnTable.ISingleWeight), 3) AS ISingleWeight,
                           ROUND(SUM(TurnTable.ITotalWeight), 3) AS ITotalWeight,
                           ROUND(SUM(TurnTable.TITotalWeight), 3) AS TITotalWeight,

                                         ROUND(
                                                   SUM(TurnTable.ISingleWeight) 
                                                   - SUM(TurnTable.OSingleWeight),
                                                   3
                                               ) AS RSingleWeight,
                                               ROUND(
                                                   SUM(TurnTable.ITotalWeight) 
                                                   - SUM(TurnTable.OTotalWeight),
                                                   3
                                               ) AS RTotalWeight,ROUND(
                                                   SUM(TurnTable.TITotalWeight) 
                                                   - SUM(TurnTable.TOTotalWeight),
                                                   3
                                               ) AS TRTotalWeight 
                      FROM         (

SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 
{2}.dbo.Table_035_ProjectInfo.Column02 AS Project, SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS ONumberInBox,
                                                     cast(ROUND( (
                                                           isnull(  Sum(  dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF(
                                                                  (
                                                                      SELECT tcai.column09
                                                                      FROM   table_004_CommodityAndIngredients tcai
                                                                      WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                                  ),
                                                                  0
                                                              ),0)
                                                          ),3)AS DECIMAL(36,3)) AS TONumberInBox,
                                              SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS ONumberInPack,
                                                cast(ROUND( (
                                                             isnull(  Sum(   dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF(
                                                                  (
                                                                      SELECT tcai.column08
                                                                      FROM   table_004_CommodityAndIngredients tcai
                                                                      WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                                  ),
                                                                  0
                                                              ),0)
                                                          ),3)AS DECIMAL(36,3)) AS TONumberInPack,
                                                SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS ODetailNumber, 
                                              SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OTotalNumber, SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column34, 0)) AS 
                  OSingleWeight,
                  SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column35, 0)) AS 
                  OTotalWeight,
 SUM(Table_008_Child_PwhrsDraft.column07 ) * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_008_Child_PwhrsDraft.column02
                          ),0)   AS TOTotalWeight,

CAST(0 AS float) AS INumberInBox,CAST(0 AS float) AS TINumberInBox, CAST(0 AS float) AS INumberInPack, CAST(0 AS float) AS TINumberInPack,
                                              CAST(0 AS float) AS IDetailNumber, CAST(0 AS float) AS ITotalNumber, CAST(0 AS FLOAT) AS ISingleWeight,
                  CAST(0 AS FLOAT) AS ITotalWeight,CAST(0 AS FLOAT) AS TITotalWeight " +
                        (chk_BuildSeri.Checked ? ",Table_008_Child_PwhrsDraft.Column30" : "") + (chk_ExpDate.Checked ? ",Table_008_Child_PwhrsDraft.Column31" : "") + (chk_Brand.Checked ? ",Table_008_Child_PwhrsDraft.Column36 AS Brand" : "") + (chk_Supplyer.Checked ? ",Table_008_Child_PwhrsDraft.Column37 AS Supplyer" : "") + @"
                       FROM          dbo.Table_007_PwhrsDraft INNER JOIN
                                              dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01 LEFT OUTER JOIN
                      {2}.dbo.Table_035_ProjectInfo ON dbo.Table_008_Child_PwhrsDraft.column14 = {2}.dbo.Table_035_ProjectInfo.Column00

                       WHERE      (dbo.Table_007_PwhrsDraft.column02 <= '{0}') AND  (dbo.Table_007_PwhrsDraft.column03 in ( {1})) " +
                       (chk_ResidDraft.Checked ? " and dbo.Table_007_PwhrsDraft.Column01<=" + txt_DraftNumber.Text.Trim() : " ") + @"
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02,dbo.Table_008_Child_PwhrsDraft.column14,
                       {2}.dbo.Table_035_ProjectInfo.Column02" + (chk_BuildSeri.Checked ? ",Table_008_Child_PwhrsDraft.Column30" : "")
                                        + (chk_ExpDate.Checked ? ",Table_008_Child_PwhrsDraft.Column31" : "") + (chk_Brand.Checked ? ",Table_008_Child_PwhrsDraft.Column36" : "") + (chk_Supplyer.Checked ? ",Table_008_Child_PwhrsDraft.Column37" : "") + @"


                       UNION ALL


                    SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, 
                    {2}.dbo.Table_035_ProjectInfo.Column02 AS Project, CAST(0 AS float) AS INumberInBox,CAST(0 AS float) AS TINumberInBox, CAST(0 AS float) AS INumberInPack,CAST(0 AS float) AS TINumberInPack, CAST(0 AS float) 
                                    AS IDetailNumber, CAST(0 AS float) AS ITotalNumber, CAST(0 AS FLOAT) AS OSingleWeight,
                  CAST(0 AS FLOAT) AS OTotalWeight,CAST(0 AS FLOAT) AS TOTotalWeight, SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS ONumberInBox, 
                                                                                        cast(ROUND((
                                                          isnull(  Sum(  dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF(
                                                              (
                                                                  SELECT tcai.column09
                                                                  FROM   table_004_CommodityAndIngredients tcai
                                                                  WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                                              ),
                                                              0
                                                          ),0)
                                                      ) ,3)AS DECIMAL(36,3)) as TONumberInBox,
                                    SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS ONumberInPack, 
                                                    cast(ROUND((
                                                            isnull( Sum( dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF(
                                                              (
                                                                  SELECT tcai.column08
                                                                  FROM   table_004_CommodityAndIngredients tcai
                                                                  WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                                              ),
                                                              0
                                                          ),0)
                                                      ) ,3)AS DECIMAL(36,3)) as TONumberInPack,
                                    SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS ODetailNumber, 
                                    SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS OTotalNumber,SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column34, 0)) AS 
                  ISingleWeight,
                  SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column35, 0)) AS 
                  ITotalWeight, SUM(Table_012_Child_PwhrsReceipt.column07) * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_012_Child_PwhrsReceipt.column02
                          ),0)   AS TITotalWeight "
                    + (chk_BuildSeri.Checked ? ",Table_012_Child_PwhrsReceipt.Column30" : "") + (chk_ExpDate.Checked ? ",Table_012_Child_PwhrsReceipt.Column31" : "") + (chk_Brand.Checked ? ",Table_012_Child_PwhrsReceipt.Column36 AS Brand" : "") + (chk_Supplyer.Checked ? ",Table_012_Child_PwhrsReceipt.Column37 AS Supplyer" : "") + @"
                    FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                                    dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01 LEFT OUTER JOIN
                    {2}.dbo.Table_035_ProjectInfo ON dbo.Table_012_Child_PwhrsReceipt.column14 = {2}.dbo.Table_035_ProjectInfo.Column00
                    WHERE     (dbo.Table_011_PwhrsReceipt.column02 <= '{0}') AND  (dbo.Table_011_PwhrsReceipt.column03 in ({1}))" +
                    (chk_ResidDraft.Checked ? " and Table_011_PwhrsReceipt.Column01<=" + txt_ResidNumber.Text.Trim() : " ") +
                    @" GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, 
                    dbo.Table_012_Child_PwhrsReceipt.column14, {2}.dbo.Table_035_ProjectInfo.Column02" +
                    (chk_BuildSeri.Checked ? ",Table_012_Child_PwhrsReceipt.Column30" : "") +
                    (chk_ExpDate.Checked ? ",Table_012_Child_PwhrsReceipt.Column31" : "") + (chk_Brand.Checked ? ",Table_012_Child_PwhrsReceipt.Column36" : "") + (chk_Supplyer.Checked ? ",Table_012_Child_PwhrsReceipt.Column37" : "") + @"


                    ) AS TurnTable INNER JOIN
                    dbo.table_004_CommodityAndIngredients ON TurnTable.GoodCode = dbo.table_004_CommodityAndIngredients.columnid
                    INNER JOIN  dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid AND 
                      dbo.table_004_CommodityAndIngredients.column03 = dbo.table_003_SubsidiaryGroup.column01 INNER JOIN
                      dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid
                    GROUP BY TurnTable.GoodCode, dbo.table_004_CommodityAndIngredients.column02, dbo.table_004_CommodityAndIngredients.column01,dbo.table_004_CommodityAndIngredients.column07, TurnTable.Project, 
                    dbo.table_004_CommodityAndIngredients.column05, dbo.table_003_SubsidiaryGroup.column03, dbo.table_002_MainGroup.column02" + (chk_BuildSeri.Checked ? ",TurnTable.Column30" : "") +
                    (chk_ExpDate.Checked ? ",TurnTable.Column31" : "") + (chk_Brand.Checked ? ",TurnTable.Brand" : "") + (chk_Supplyer.Checked ? ",TurnTable.Supplyer" : "") + " ORDER BY dbo.table_004_CommodityAndIngredients.column02,dbo.table_004_CommodityAndIngredients.column01", ConWare);



                        Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText,
                            Date1, whr, ConBase.Database);
                        DataTable Table = new DataTable();
                        Adapter.Fill(Table);
                        bindingSource1 = new BindingSource();
                        bindingSource1.DataSource = Table;
                        bindingNavigator2.BindingSource = bindingSource1;

                        if (chk_ontatal.Checked)
                        {
                            gridEX1.Visible = true;
                            gridEX2.Visible = false;
                            gridEX1.DataSource = bindingSource1;
                        }

                        else
                        {
                            gridEX1.Visible = false;
                            gridEX2.Visible = true;
                            gridEX2.DataSource = bindingSource1;
                        }
                    }
                    //همه انبارها
                    else if (mlt_Ware.Text.Trim() != "" && mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0")
                    {
                        SqlDataAdapter Adapter = new SqlDataAdapter(
@"  SELECT     TurnTable.GoodCode AS GoodID, TurnTable.Project, Round(SUM(TurnTable.INumberInBox),3) AS INumberInBox, Round(SUM(TurnTable.TINumberInBox),3) AS TINumberInBox,Round(SUM(TurnTable.INumberInPack),3) AS INumberInPack,Round(SUM(TurnTable.TINumberInPack),3) AS TINumberInPack ,
                     Round(SUM(TurnTable.IDetailNumber),3) AS IDetailNumber, Round(SUM(TurnTable.ITotalNumber),3) AS ITotalNumber, Round(SUM(TurnTable.ONumberInBox),3) AS ONumberInBox,  Round(SUM(TurnTable.TONumberInBox),3) AS TONumberInBox,
                     Round( SUM(TurnTable.ONumberInPack),3) AS ONumberInPack,Round( SUM(TurnTable.TONumberInPack),3) AS TONumberInPack, Round(SUM(TurnTable.ODetailNumber),3) AS ODetailNumber, Round(SUM(TurnTable.OTotalNumber),3) AS OTotalNumber, 
                      Round(SUM(TurnTable.INumberInBox) - SUM(TurnTable.ONumberInBox),3) AS RNumberInBox,Round(SUM(TurnTable.TINumberInBox) - SUM(TurnTable.TONumberInBox),3) AS TRNumberInBox, Round(SUM(TurnTable.INumberInPack) - SUM(TurnTable.ONumberInPack),3)
                      AS RNumberInPack, Round(SUM(TurnTable.TINumberInPack) - SUM(TurnTable.TONumberInPack),3)
                      AS TRNumberInPack, Round(SUM(TurnTable.IDetailNumber) - SUM(TurnTable.ODetailNumber),3) AS RDetailNumber, Round(SUM(TurnTable.ITotalNumber) 
                      - SUM(TurnTable.OTotalNumber),3) AS RTotalNumber, dbo.table_004_CommodityAndIngredients.column02 AS GoodName, 
                      dbo.table_004_CommodityAndIngredients.column01 AS GoodCode,dbo.table_004_CommodityAndIngredients.column07 AS UnitCount,dbo.table_004_CommodityAndIngredients.column05 as CompName" +
                    (chk_BuildSeri.Checked ? ",TurnTable.Column30" : "") + (chk_ExpDate.Checked ? ",TurnTable.Column31" : "")+ (chk_Brand.Checked ? ",TurnTable.Brand" : "") + (chk_Supplyer.Checked ? ",TurnTable.Supplyer" : "") + @", 
                      dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 as MainGroupName,
                       ROUND(SUM(TurnTable.OSingleWeight), 3) AS OSingleWeight,
       ROUND(SUM(TurnTable.OTotalWeight), 3) AS OTotalWeight,
       ROUND(SUM(TurnTable.TOTotalWeight), 3) AS TOTotalWeight,

       ROUND(SUM(TurnTable.ISingleWeight), 3) AS ISingleWeight,
       ROUND(SUM(TurnTable.ITotalWeight), 3) AS ITotalWeight,
       ROUND(SUM(TurnTable.TITotalWeight), 3) AS TITotalWeight,

       ROUND(
           SUM(TurnTable.ISingleWeight) 
           - SUM(TurnTable.OSingleWeight),
           3
       ) AS RSingleWeight,
       ROUND(
           SUM(TurnTable.ITotalWeight) 
           - SUM(TurnTable.OTotalWeight),
           3
       ) AS RTotalWeight,ROUND(
           SUM(TurnTable.TITotalWeight) 
           - SUM(TurnTable.TOTotalWeight),
           3
       ) AS TRTotalWeight 
                      FROM         (

SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 
{1}.dbo.Table_035_ProjectInfo.Column02 AS Project, SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS ONumberInBox, 
                                                     cast(ROUND( (
                                                                isnull( Sum( dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF(
                                                                  (
                                                                      SELECT tcai.column09
                                                                      FROM   table_004_CommodityAndIngredients tcai
                                                                      WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                                  ),
                                                                  0
                                                              ),0)
                                                          ),3)AS DECIMAL(36,3)) AS TONumberInBox,
                                              SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS ONumberInPack,
                                                 cast(ROUND( (
                                                              isnull(   Sum( dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF(
                                                                  (
                                                                      SELECT tcai.column08
                                                                      FROM   table_004_CommodityAndIngredients tcai
                                                                      WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                                                  ),
                                                                  0
                                                              ),0)
                                                          ),3)AS DECIMAL(36,3)) AS TONumberInPack,
                                            SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS ODetailNumber, 
                                              SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OTotalNumber,SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column34, 0)) AS 
                  OSingleWeight,
                  SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column35, 0)) AS 
                  OTotalWeight, SUM(Table_008_Child_PwhrsDraft.column07) * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_008_Child_PwhrsDraft.column02
                          ),0)   AS TOTotalWeight, CAST(0 AS float) AS INumberInBox, CAST(0 AS float) AS TINumberInBox, CAST(0 AS float) AS INumberInPack, CAST(0 AS float) AS TINumberInPack,
                                              CAST(0 AS float) AS IDetailNumber, CAST(0 AS float) AS ITotalNumber, CAST(0 AS FLOAT) AS ISingleWeight,
                  CAST(0 AS FLOAT) AS ITotalWeight,CAST(0 AS FLOAT) AS TITotalWeight " +
                       (chk_BuildSeri.Checked ? ",Table_008_Child_PwhrsDraft.Column30" : "") + (chk_ExpDate.Checked ? ",Table_008_Child_PwhrsDraft.Column31" : "")  + (chk_Brand.Checked ? ",Table_008_Child_PwhrsDraft.Column36 AS Brand" : "") + (chk_Supplyer.Checked ? ",Table_008_Child_PwhrsDraft.Column37 AS Supplyer" : "") +@"
                       FROM          dbo.Table_007_PwhrsDraft INNER JOIN
                                              dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01 LEFT OUTER JOIN
                      {1}.dbo.Table_035_ProjectInfo ON dbo.Table_008_Child_PwhrsDraft.column14 = {1}.dbo.Table_035_ProjectInfo.Column00
                       WHERE      (dbo.Table_007_PwhrsDraft.column02 <= '{0}') AND dbo.Table_007_PwhrsDraft.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')   " +
                       (chk_ResidDraft.Checked ? " and dbo.Table_007_PwhrsDraft.Column01<=" + txt_DraftNumber.Text.Trim() : " ")  +
                       @" GROUP BY dbo.Table_008_Child_PwhrsDraft.column02,dbo.Table_008_Child_PwhrsDraft.column14,
{1}.dbo.Table_035_ProjectInfo.Column02" + (chk_BuildSeri.Checked ? ",Table_008_Child_PwhrsDraft.Column30" : "")
                                        + (chk_ExpDate.Checked ? ",Table_008_Child_PwhrsDraft.Column31" : "") + (chk_Brand.Checked ? ",Table_008_Child_PwhrsDraft.Column36" : "") + (chk_Supplyer.Checked ? ",Table_008_Child_PwhrsDraft.Column37" : "") + @"

                       UNION ALL


                       SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, 
{1}.dbo.Table_035_ProjectInfo.Column02 AS Project, CAST(0 AS float) AS INumberInBox , CAST(0 AS float) AS TINumberInBox, CAST(0 AS float) AS INumberInPack, CAST(0 AS float) AS TINumberInPack, CAST(0 AS float) 
                                             AS IDetailNumber, CAST(0 AS float) AS ITotalNumber, CAST(0 AS FLOAT) AS OSingleWeight,
                  CAST(0 AS FLOAT) AS OTotalWeight,CAST(0 AS FLOAT) AS TOTotalWeight, SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS ONumberInBox, 
                                                                                                cast(ROUND((
                                                                    isnull(Sum( dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF(
                                                                      (
                                                                          SELECT tcai.column09
                                                                          FROM   table_004_CommodityAndIngredients tcai
                                                                          WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                                                      ),
                                                                      0
                                                                  ),0)
                                                              ) ,3)AS DECIMAL(36,3)) as TONumberInBox,
                                             SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS ONumberInPack,
                                             cast(ROUND((
                                                                   isnull( Sum(  dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF(
                                                                      (
                                                                          SELECT tcai.column08
                                                                          FROM   table_004_CommodityAndIngredients tcai
                                                                          WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                                                      ),
                                                                      0
                                                                  ),0)
                                                              ) ,3)AS DECIMAL(36,3)) as TONumberInPack,
                                            SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS ODetailNumber, 
                                             SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS OTotalNumber, SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column34, 0)) AS 
                  ISingleWeight,
                  SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column35, 0)) AS 
                  ITotalWeight, SUM(Table_012_Child_PwhrsReceipt.column07 ) * isnull((
                              SELECT tcai.column22
                              FROM   table_004_CommodityAndIngredients tcai
                              WHERE  tcai.columnid = Table_012_Child_PwhrsReceipt.column02
                          ),0)   AS TITotalWeight  "
                    + (chk_BuildSeri.Checked ? ",Table_012_Child_PwhrsReceipt.Column30" : "") + (chk_ExpDate.Checked ? ",Table_012_Child_PwhrsReceipt.Column31" : "") + (chk_Brand.Checked ? ",Table_012_Child_PwhrsReceipt.Column36 AS Brand" : "") + (chk_Supplyer.Checked ? ",Table_012_Child_PwhrsReceipt.Column37 AS Supplyer" : "") + @"
                       FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                                             dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01 LEFT OUTER JOIN
                      {1}.dbo.Table_035_ProjectInfo ON dbo.Table_012_Child_PwhrsReceipt.column14 = {1}.dbo.Table_035_ProjectInfo.Column00 
                       WHERE     (dbo.Table_011_PwhrsReceipt.column02 <= '{0}') AND dbo.Table_011_PwhrsReceipt.column03 not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')  " +
                       (chk_ResidDraft.Checked ? " and Table_011_PwhrsReceipt.Column01<=" + txt_ResidNumber.Text.Trim() : " ")
                                                      + @"
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02,
dbo.Table_012_Child_PwhrsReceipt.column14, {1}.dbo.Table_035_ProjectInfo.Column02  " +
                    (chk_BuildSeri.Checked ? ",Table_012_Child_PwhrsReceipt.Column30" : "") +
                    (chk_ExpDate.Checked ? ",Table_012_Child_PwhrsReceipt.Column31" : "") + (chk_Brand.Checked ? ",Table_012_Child_PwhrsReceipt.Column36  " : "") + (chk_Supplyer.Checked ? ",Table_012_Child_PwhrsReceipt.Column37  " : "") + @"

) AS TurnTable INNER JOIN
                      dbo.table_004_CommodityAndIngredients ON TurnTable.GoodCode = dbo.table_004_CommodityAndIngredients.columnid
INNER JOIN
                      dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid AND 
                      dbo.table_004_CommodityAndIngredients.column03 = dbo.table_003_SubsidiaryGroup.column01 INNER JOIN
                      dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid
                    GROUP BY TurnTable.GoodCode, dbo.table_004_CommodityAndIngredients.column02, dbo.table_004_CommodityAndIngredients.column01,dbo.table_004_CommodityAndIngredients.column07, TurnTable.Project,   
                      dbo.table_004_CommodityAndIngredients.column05, dbo.table_003_SubsidiaryGroup.column03, dbo.table_002_MainGroup.column02" + (chk_BuildSeri.Checked ? ",TurnTable.Column30" : "") +
                    (chk_ExpDate.Checked ? ",TurnTable.Column31" : "") + (chk_Brand.Checked ? ",TurnTable.Brand" : "") + (chk_Supplyer.Checked ? ",TurnTable.Supplyer" : "")+  " ORDER BY dbo.table_004_CommodityAndIngredients.column02,dbo.table_004_CommodityAndIngredients.column01"

                                                      , ConWare);
                        Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText,
                            Date1, ConBase.Database);
                        DataTable Table = new DataTable();
                        Adapter.Fill(Table);
                        bindingSource1 = new BindingSource();
                        bindingSource1.DataSource = Table;
                        bindingNavigator2.BindingSource = bindingSource1;
                        gridEX1.DataSource = bindingSource1;
                        gridEX2.DataSource = bindingSource1;

                        //if (chk_ontatal.Checked)
                        //{
                        //    gridEX1.Visible = true;
                        //    gridEX2.Visible = false;
                        //    gridEX1.DataSource = bindingSource1;
                        //}

                        //else
                        //{
                        //    gridEX1.Visible = false;
                        //    gridEX2.Visible = true;
                        //    gridEX2.DataSource = bindingSource1;
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (chk_ontatal.Checked)
            {

                if (gridEX1.RowCount > 0)
                {
                    DataTable Table = dataSet_001_Gozareshat.Rpt_Maghta_Tedad.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {
                        Table.Rows.Add(item.Cells["GoodCode"].Text, item.Cells["GoodName"].Text,
                            item.Cells["TINumberInBox"].Value.ToString(),
                            item.Cells["TINumberInPack"].Value.ToString(),
                            item.Cells["InDetail"].Value.ToString(),
                            item.Cells["InTotal"].Value.ToString(),
                         (item.Cells["TONumberInBox"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TONumberInBox"].Value.ToString()) ? Convert.ToDouble(item.Cells["TONumberInBox"].Value.ToString()) : Convert.ToDouble(0)),
                         (item.Cells["TONumberInPack"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TONumberInPack"].Value.ToString()) ? Convert.ToDouble(item.Cells["TONumberInPack"].Value.ToString()) : Convert.ToDouble(0)),
                            item.Cells["OutDetail"].Value.ToString(),
                            item.Cells["OutTotal"].Value.ToString(),
                         (item.Cells["TRNumberInBox"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TRNumberInBox"].Value.ToString()) ? Convert.ToDouble(item.Cells["TRNumberInBox"].Value.ToString()) : Convert.ToDouble(0)),
                         (item.Cells["TRNumberInPack"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TRNumberInPack"].Value.ToString()) ? Convert.ToDouble(item.Cells["TRNumberInPack"].Value.ToString()) : Convert.ToDouble(0)),
                            item.Cells["ReDetail"].Value.ToString(),
                            item.Cells["ReTotal"].Value.ToString(),
                            item.Cells["Project"].Text,
                         (item.Cells["TOTotalWeight"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TOTotalWeight"].Value.ToString()) ? Convert.ToDouble(item.Cells["TOTotalWeight"].Value.ToString()) : Convert.ToDouble(0)),
                         (item.Cells["TITotalWeight"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TITotalWeight"].Value.ToString()) ? Convert.ToDouble(item.Cells["TITotalWeight"].Value.ToString()) : Convert.ToDouble(0)),
                         (item.Cells["TRTotalWeight"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TRTotalWeight"].Value.ToString()) ? Convert.ToDouble(item.Cells["TRTotalWeight"].Value.ToString()) : Convert.ToDouble(0))
                            );

                    }
                    if (Table.Rows.Count > 0)
                    {
                        Report.Form01_ReportForm frm = new Form01_ReportForm
                            (Table, 3, (mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0" ? "همه انبارها" : mlt_Ware.Text), "تا تاریخ: " +
                            Date1);
                        frm.ShowDialog();
                    }
                }

            }
            else
            {
                if (gridEX2.RowCount > 0)
                {
                    DataTable Table = dataSet_001_Gozareshat.Rpt_Maghta_Tedad.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                    {
                        Table.Rows.Add(item.Cells["GoodCode"].Text, item.Cells["GoodName"].Text,
                            item.Cells["InBox"].Value.ToString(),
                            item.Cells["InPack"].Value.ToString(),
                            item.Cells["InDetail"].Value.ToString(),
                            item.Cells["InTotal"].Value.ToString(),
                            item.Cells["OutBox"].Value.ToString(),
                            item.Cells["OutPack"].Value.ToString(),
                            item.Cells["OutDetail"].Value.ToString(),
                            item.Cells["OutTotal"].Value.ToString(),
                            item.Cells["ReBox"].Value.ToString(),
                            item.Cells["RePack"].Value.ToString(),
                            item.Cells["ReDetail"].Value.ToString(),
                            item.Cells["ReTotal"].Value.ToString(),
                            item.Cells["Project"].Text,
                            item.Cells["OTotalWeight"].Text,
                            item.Cells["ITotalWeight"].Text,
                            item.Cells["RTotalWeight"].Text);

                    }
                    if (Table.Rows.Count > 0)
                    {
                        Report.Form01_ReportForm frm = new Form01_ReportForm
                            (Table, 3, (mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0" ? "همه انبارها" : mlt_Ware.Text), "تا تاریخ: " +
                            Date1);
                        frm.ShowDialog();
                    }
                }
            }
        }


        private void Frm_003_MojoodiMaghtaiTedadi_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX2.RemoveFilters();
            gridEX1.RemoveFilters();
        }

        private void Frm_003_MojoodiMaghtaiTedadi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePicker1.Select();
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox =
                    (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            Class_BasicOperation.isEnter(e.KeyChar);

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
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

        private void txt_FromNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void CheckResidDraftNumber()
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL((select Column01 from Table_007_PwhrsDraft where column01=" + txt_DraftNumber.Text + "),0)", Con);
                if (Command.ExecuteScalar().ToString() == "0")
                    throw new Exception("شماره حواله وارد شده نامعتبر است");

                Command = new SqlCommand("Select ISNULL((select Column01 from Table_011_PwhrsReceipt where column01=" + txt_ResidNumber.Text + "),0)", Con);
                if (Command.ExecuteScalar().ToString() == "0")
                    throw new Exception("شماره رسید وارد شده نامعتبر است");
            }
        }

        private void bt_NumericalCardex_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 37))
                {
                    Report.Frm_009_KardexTedadi frms = new Report.Frm_009_KardexTedadi(
                        int.Parse(gridEX2.GetValue("GoodID").ToString()),
                          faDatePicker1.SelectedDateTime.Value.AddMonths(-2),
                        faDatePicker1.SelectedDateTime);
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

        private void bt_RialiCardex_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 38))
                {
                    Report.Frm_010_KardexRiyali frms = new Report.Frm_010_KardexRiyali(
                        int.Parse(gridEX2.GetValue("GoodID").ToString()),
                        faDatePicker1.SelectedDateTime.Value.AddMonths(-2),
                        faDatePicker1.SelectedDateTime);
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

        private void chk_ontatal_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.chk_Total = chk_ontatal.Checked;
            Properties.Settings.Default.Save();
        }



    }
}
