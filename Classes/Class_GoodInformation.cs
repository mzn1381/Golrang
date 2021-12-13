using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace PCLOR.Classes
{
    public class Class_GoodInformation
    {
        public DataTable GoodInfo()
        {
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);

            string SelectCommand = @"SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroup, 
            GoodsInformation.goroohfari AS SubGroup, CountUnit,Weight,ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, ISNULL(GoodsInformation.NumberInPack, 0) 
            AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, GoodsInformation.Active1, 
            ISNULL(GoodsInformation.BuyPrice, 0) AS BuyPrice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, ISNULL(GoodsInformation.SalePackPrice, 0) 
            AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) AS UserPrice, 
            ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
             dbo.table_003_SubsidiaryGroup.column03 as SubGroupName,  dbo.table_002_MainGroup.column02 as MainGroupName
            FROM         (SELECT      dbo.table_004_CommodityAndIngredients.columnid AS id,  dbo.table_004_CommodityAndIngredients.column01 AS Code, 
             dbo.table_004_CommodityAndIngredients.column02 AS name,  dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
             dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, dbo.table_004_CommodityAndIngredients.column07 AS CountUnit,
             table_004_CommodityAndIngredients.Column22 as Weight, CASE WHEN  dbo.table_006_CommodityChanges.Column07 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column09 ELSE  dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
            CASE WHEN  dbo.table_006_CommodityChanges.Column06 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column08 ELSE  dbo.table_006_CommodityChanges.Column06 END AS NumberInPack, 
            CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
            CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
            ISNULL( dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
            THEN table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
            THEN table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
            THEN table_004_CommodityAndIngredients.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
            THEN table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
            THEN table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
            THEN table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
            THEN table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
             dbo.table_004_CommodityAndIngredients.column28 AS Active3
            FROM           dbo.table_004_CommodityAndIngredients  LEFT OUTER JOIN
            (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
            FROM          dbo.Table_003_InformationProductCash) AS TS003 ON 
             dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
            dbo.table_006_CommodityChanges ON  dbo.table_004_CommodityAndIngredients.columnid = dbo.table_006_CommodityChanges.column01) 
            AS GoodsInformation INNER JOIN
             dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari =  dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
             dbo.table_002_MainGroup ON  dbo.table_003_SubsidiaryGroup.column01 =  dbo.table_002_MainGroup.columnid
            WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1)";


            SqlDataAdapter Adapter = new SqlDataAdapter(SelectCommand, ConWare);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            return GoodTable;
        }

        public double GoodValue(int GoodId, short Ware)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"SELECT  CAST((  CASE WHEN (ISNULL(View_1_Resid.TR, 0) 
                      - ISNULL(View_2_Havaleh.TH, 0)) = 0 THEN 0 ELSE (ISNULL(View_1_Resid.PR, 0) - ISNULL(View_2_Havaleh.PH, 0)) / (ISNULL(View_1_Resid.TR, 0) 
                      - ISNULL(View_2_Havaleh.TH, 0)) END) as decimal(18,4)) AS PAVG
                FROM         dbo.table_004_CommodityAndIngredients LEFT OUTER JOIN
                (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS IdKala, SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS KR, 
                SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS BR, SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS JR, 
                SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS TR, SUM(dbo.Table_012_Child_PwhrsReceipt.column21) AS PR
                FROM         dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                dbo.Table_011_PwhrsReceipt ON dbo.Table_012_Child_PwhrsReceipt.column01 = dbo.Table_011_PwhrsReceipt.columnid
                WHERE     (dbo.Table_011_PwhrsReceipt.column03 = " + Ware + @")
                GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02) AS View_1_Resid ON 
                dbo.table_004_CommodityAndIngredients.columnid = View_1_Resid.IdKala LEFT OUTER JOIN
                (SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS IdKala, SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS KH, 
                SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS BH, SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS JH, 
                SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS TH, SUM(dbo.Table_008_Child_PwhrsDraft.column16) AS PH
                FROM         dbo.Table_008_Child_PwhrsDraft INNER JOIN
                dbo.Table_007_PwhrsDraft ON dbo.Table_008_Child_PwhrsDraft.column01 = dbo.Table_007_PwhrsDraft.columnid
                WHERE     (dbo.Table_007_PwhrsDraft.column03 =  " + Ware + @")
                GROUP BY dbo.Table_008_Child_PwhrsDraft.column02) AS View_2_Havaleh ON 
                dbo.table_004_CommodityAndIngredients.columnid = View_2_Havaleh.IdKala
                WHERE     (dbo.table_004_CommodityAndIngredients.columnid = " + GoodId + ")", Con);
                return Convert.ToDouble(Comm.ExecuteScalar().ToString());
            }
        }

        public DataTable MahsoolInfo()
        {
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);

            string SelectCommand = @"SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroup, 
            GoodsInformation.goroohfari AS SubGroup,CountUnit,Weight, ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, ISNULL(GoodsInformation.NumberInPack, 0) 
            AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, GoodsInformation.Active1, 
            ISNULL(GoodsInformation.BuyPrice, 0) AS BuyPrice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, ISNULL(GoodsInformation.SalePackPrice, 0) 
            AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) AS UserPrice, 
            ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
             dbo.table_003_SubsidiaryGroup.column03 as SubGroupName,  dbo.table_002_MainGroup.column02 as MainGroupName
            FROM         (SELECT      dbo.table_004_CommodityAndIngredients.columnid AS id,  dbo.table_004_CommodityAndIngredients.column01 AS Code, 
             dbo.table_004_CommodityAndIngredients.column02 AS name,  dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
             dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, dbo.table_004_CommodityAndIngredients.column07 AS CountUnit,
             table_004_CommodityAndIngredients.Column22 as Weight, CASE WHEN  dbo.table_006_CommodityChanges.Column07 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column09 ELSE  dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
            CASE WHEN  dbo.table_006_CommodityChanges.Column06 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column08 ELSE  dbo.table_006_CommodityChanges.Column06 END AS NumberInPack, 
            CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
            CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
            ISNULL( dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
            THEN table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
            THEN table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
            THEN table_004_CommodityAndIngredients.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
            THEN table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
            THEN table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
            THEN table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
            THEN table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
             dbo.table_004_CommodityAndIngredients.column28 AS Active3
            FROM           dbo.table_004_CommodityAndIngredients 
            LEFT OUTER JOIN
            (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
            FROM        dbo.Table_003_InformationProductCash) AS TS003 ON 
             dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
             dbo.table_006_CommodityChanges ON  
            dbo.table_004_CommodityAndIngredients.columnid =  dbo.table_006_CommodityChanges.column01 WHERE     ( dbo.table_004_CommodityAndIngredients.column19 = 1)) 
            AS GoodsInformation INNER JOIN
             dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari =  dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
             dbo.table_002_MainGroup ON  dbo.table_003_SubsidiaryGroup.column01 =  dbo.table_002_MainGroup.columnid
            WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1)";

            SqlDataAdapter Adapter = new SqlDataAdapter(SelectCommand, ConWare);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            return GoodTable;
        }

        /// <summary>
        /// return all general goods and goods in specified warehause
        /// </summary>
        /// <param name="WareCode"></param>
        /// <returns></returns>
        public DataTable GoodInfo(Int16 WareCode)
        {
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);

            string SelectCommand = @"SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroup, 
                      GoodsInformation.goroohfari AS SubGroup,  GoodsInformation.CountUnit,  GoodsInformation.Weight,ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, 
                      ISNULL(GoodsInformation.NumberInPack, 0) AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, 
                      GoodsInformation.Active1, ISNULL(GoodsInformation.BuyPrice, 0) AS BuyPrice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, 
                      ISNULL(GoodsInformation.SalePackPrice, 0) AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) 
                      AS UserPrice, ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
                      dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 AS MainGroupName, dbo.Table_004_GoodsWare.Column02
                      FROM         (SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS Code, 
                                              dbo.table_004_CommodityAndIngredients.column02 AS name, dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
                                              dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, dbo.table_004_CommodityAndIngredients.column07 AS CountUnit, 
                                              table_004_CommodityAndIngredients.Column22 as Weight,
                                              CASE WHEN dbo.table_006_CommodityChanges.Column07 IS NULL 
                                              THEN dbo.table_004_CommodityAndIngredients.column09 ELSE dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
                                              CASE WHEN dbo.table_006_CommodityChanges.Column06 IS NULL 
                                              THEN dbo.table_004_CommodityAndIngredients.column08 ELSE dbo.table_006_CommodityChanges.Column06 END AS NumberInPack, 
                                              CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
                                              THEN dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
                                              CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
                                              THEN dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
                                              ISNULL(dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
                                              THEN table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
                                              THEN table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
                                              THEN table_004_CommodityAndIngredients.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
                                              THEN table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
                                              THEN table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
                                              THEN table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
                                              THEN table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
                                              dbo.table_004_CommodityAndIngredients.column28 AS Active3
                        FROM         dbo.table_004_CommodityAndIngredients  LEFT OUTER JOIN
                                                  (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
                                                     FROM         dbo.Table_003_InformationProductCash) AS TS003 ON 
                                              dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
                                              dbo.table_006_CommodityChanges ON dbo.table_004_CommodityAndIngredients.columnid = dbo.table_006_CommodityChanges.column01) 
                      AS GoodsInformation INNER JOIN
                      dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                      dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid LEFT OUTER JOIN
                      dbo.Table_004_GoodsWare ON GoodsInformation.id = dbo.Table_004_GoodsWare.Column01
                      WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1) AND (dbo.Table_004_GoodsWare.Column02 = " +
                       WareCode + ") OR  (dbo.Table_004_GoodsWare.Column02 IS NULL)";


            SqlDataAdapter Adapter = new SqlDataAdapter(SelectCommand, ConWare);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            return GoodTable;
        }


        /// <summary>
        /// Returns all Active goods in warehauseS and General goods
        /// </summary>
        /// <returns></returns>
        public DataTable GoodInfo_Ware()
        {
            DataTable GoodTable = new DataTable();
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC [dbo].[PR_10_GoodInfo]", ConWare);
            Adapter.Fill(GoodTable);
            return GoodTable;
        }
        public DataTable BarCodeGoodInfo_Ware()
        {
            DataTable GoodTable = new DataTable();
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC [dbo].[PR_10_GoodInfoBarCode]", ConWare);
            Adapter.Fill(GoodTable);
            return GoodTable;
        }


        /// <summary>
        /// Check if a good is
        /// </summary>
        /// <param name="wareid"></param>
        /// <param name="GoodId"></param>
        /// <returns></returns>
        public bool IsGoodInWare(Int16 wareid, int GoodId)
        {
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);

            string SelectCommand = @"SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroup, 
			 GoodsInformation.goroohfari AS SubGroup,GoodsInformation.CountUnit,GoodsInformation.Weight, ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, 
			ISNULL(GoodsInformation.NumberInPack, 0) AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, 
			GoodsInformation.Active1, ISNULL(GoodsInformation.BuyPrice, 0) AS BuyPrice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, 
			ISNULL(GoodsInformation.SalePackPrice, 0) AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) 
			AS UserPrice, ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
			dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 AS MainGroupName, dbo.Table_004_GoodsWare.Column02
			FROM         (SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS Code, 
			dbo.table_004_CommodityAndIngredients.column02 AS name, dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
			dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, dbo.table_004_CommodityAndIngredients.column07 AS CountUnit, 
			table_004_CommodityAndIngredients.Column22 as Weight,
			dbo.table_004_CommodityAndIngredients.column32 AS AllWare, CASE WHEN dbo.table_006_CommodityChanges.Column07 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column09 ELSE dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
			CASE WHEN dbo.table_006_CommodityChanges.Column06 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column08 ELSE dbo.table_006_CommodityChanges.Column06 END AS NumberInPack, 
			CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
			CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
			ISNULL(dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
			 THEN table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
          THEN table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
          THEN table_004_CommodityAndIngredients.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
          THEN table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
          THEN table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
          THEN table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
          THEN table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
			dbo.table_004_CommodityAndIngredients.column28 AS Active3
			FROM         dbo.table_004_CommodityAndIngredients  LEFT OUTER JOIN
			(SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
			FROM         dbo.Table_003_InformationProductCash) AS TS003 ON 
			dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
			dbo.table_006_CommodityChanges ON dbo.table_004_CommodityAndIngredients.columnid = dbo.table_006_CommodityChanges.column01) 
			AS GoodsInformation INNER JOIN
			dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
			dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid INNER JOIN
			dbo.Table_004_GoodsWare ON GoodsInformation.id = dbo.Table_004_GoodsWare.Column01
			WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1)
	        and GoodsInformation.id={0}  and dbo.Table_004_GoodsWare.Column02={1}
			union all
			SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroup, 
			 GoodsInformation.goroohfari AS SubGroup,GoodsInformation.CountUnit,GoodsInformation.Weight, ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, 
			ISNULL(GoodsInformation.NumberInPack, 0) AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, 
			GoodsInformation.Active1, ISNULL(GoodsInformation.BuyPrice, 0) AS BuyPrice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, 
			ISNULL(GoodsInformation.SalePackPrice, 0) AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) 
			AS UserPrice, ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
			dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 AS MainGroupName,null as Column02
			FROM         (SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS Code, 
			dbo.table_004_CommodityAndIngredients.column02 AS name, dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
			dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, dbo.table_004_CommodityAndIngredients.column07 AS CountUnit,
			table_004_CommodityAndIngredients.Column22 as Weight, 
			dbo.table_004_CommodityAndIngredients.column32 AS AllWare, CASE WHEN dbo.table_006_CommodityChanges.Column07 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column09 ELSE dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
			CASE WHEN dbo.table_006_CommodityChanges.Column06 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column08 ELSE dbo.table_006_CommodityChanges.Column06 END AS NumberInPack, 
			CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
			CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
			THEN dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
			ISNULL(dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
			THEN table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
			THEN table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
			THEN table_004_CommodityAndIngredients.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
			THEN table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
			THEN table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
			THEN table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
			THEN table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
			dbo.table_004_CommodityAndIngredients.column28 AS Active3
			FROM         dbo.table_004_CommodityAndIngredients LEFT OUTER JOIN
			(SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
			FROM         dbo.Table_003_InformationProductCash) AS TS003 ON 
			dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
			dbo.table_006_CommodityChanges ON dbo.table_004_CommodityAndIngredients.columnid = dbo.table_006_CommodityChanges.column01) 
			AS GoodsInformation INNER JOIN
			dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
			dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid
			WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1) AND (GoodsInformation.AllWare = 1)
            and GoodsInformation.id={0}";

            SelectCommand = string.Format(SelectCommand, GoodId, wareid);

            SqlDataAdapter Adapter = new SqlDataAdapter(SelectCommand, ConWare);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            if (GoodTable.Rows.Count == 0)
                return false;
            else return true;
        }
        public DataTable MahsoolInfo(Int16 ware)
        {
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);

            string SelectCommand = @"SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroup, CountUnit,Weight,
            GoodsInformation.goroohfari AS SubGroup, ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, ISNULL(GoodsInformation.NumberInPack, 0) 
            AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, GoodsInformation.Active1, 
            ISNULL(GoodsInformation.BuyPrice, 0) AS buyprice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, ISNULL(GoodsInformation.SalePackPrice, 0) 
            AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) AS UserPrice, 
            ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
             dbo.table_003_SubsidiaryGroup.column03 as SubGroupName,  dbo.table_002_MainGroup.column02 as MainGroupName, 
            GoodsInformation.Khas,
       GoodsInformation.totalReceipt-GoodsInformation.totalDraft AS totalremain,
       GoodsInformation.wareReceipt-GoodsInformation.wareDraft AS wareremain
            FROM         (SELECT      dbo.table_004_CommodityAndIngredients.columnid AS id,  dbo.table_004_CommodityAndIngredients.column01 AS Code, 
             dbo.table_004_CommodityAndIngredients.column02 AS name,  dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
             dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, dbo.table_004_CommodityAndIngredients.column07 AS CountUnit,
            table_004_CommodityAndIngredients.Column22 as Weight,
             dbo.table_004_CommodityAndIngredients.column29 AS Khas, CASE WHEN  dbo.table_006_CommodityChanges.Column07 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column09 ELSE  dbo.table_006_CommodityChanges.Column07 END AS NumberInBox, 
            CASE WHEN  dbo.table_006_CommodityChanges.Column06 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column08 ELSE  dbo.table_006_CommodityChanges.Column06 END AS NumberInPack, 
            CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
            CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
            ISNULL( dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
            THEN  dbo.table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
             dbo.table_004_CommodityAndIngredients.column28 AS Active3,ISNULL(
                      (
                          SELECT SUM(tcpr.column07)
                          FROM   Table_012_Child_PwhrsReceipt tcpr
                                 JOIN Table_011_PwhrsReceipt tpr
                                      ON  tpr.columnid = tcpr.column01
                          WHERE  tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                                 AND tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                      ),
                      0
                  ) AS totalReceipt,
                  ISNULL(
                      (
                          SELECT SUM(tcpr.column07)
                          FROM   Table_008_Child_PwhrsDraft tcpr
                                 JOIN Table_007_PwhrsDraft tpr
                                      ON  tpr.columnid = tcpr.column01
                          WHERE  tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                                 AND tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                      ),
                      0
                  ) AS totalDraft,
                  ISNULL(
                      (
                          SELECT SUM(tcpr.column07)
                          FROM   Table_012_Child_PwhrsReceipt tcpr
                                 JOIN Table_011_PwhrsReceipt tpr
                                      ON  tpr.columnid = tcpr.column01
                          WHERE  tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                                 AND tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                                 AND tpr.column03 = " + ware + @"
                      ),
                      0
                  ) AS wareReceipt,
                  ISNULL(
                      (
                          SELECT SUM(tcpr.column07)
                          FROM   Table_008_Child_PwhrsDraft tcpr
                                 JOIN Table_007_PwhrsDraft tpr
                                      ON  tpr.columnid = tcpr.column01
                          WHERE  tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                                 AND tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                                 AND tpr.column03 = " + ware + @"
                      ),
                      0
                  ) AS wareDraft
            FROM           dbo.table_004_CommodityAndIngredients  LEFT OUTER JOIN
            (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
            FROM           dbo.Table_003_InformationProductCash) AS TS003 ON 
             dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
             dbo.table_006_CommodityChanges ON  
            dbo.table_004_CommodityAndIngredients.columnid =  dbo.table_006_CommodityChanges.column01 WHERE     ( dbo.table_004_CommodityAndIngredients.column19 = 1)) 
            AS GoodsInformation INNER JOIN
             dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari =  dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
             dbo.table_002_MainGroup ON  dbo.table_003_SubsidiaryGroup.column01 =  dbo.table_002_MainGroup.columnid
            WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1)";



            SqlDataAdapter Adapter = new SqlDataAdapter(SelectCommand, ConWare);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            return GoodTable;
        }


    }



}
