using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace PCLOR.Classes
{
    class Class_Settle
    {
        public DataTable SettleTable()
        {
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     columnid, column01, column03, Column28, Cash, Chq, 
            Discount, ReturnGoods, FromFactor, ToFactor, Cash + Chq + Discount + ReturnGoods + FromFactor AS TotalSettle,  
            Cash + Chq + Discount + ReturnGoods + FromFactor - ToFactor AS TotalSettle_Paid,
            Column28 - (Cash + Chq + Discount + ReturnGoods + FromFactor - ToFactor) AS Remain
            FROM         (SELECT     dbo.Table_010_SaleFactor.columnid, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column03, 
            dbo.Table_010_SaleFactor.Column28 - dbo.Table_010_SaleFactor.Column29 - dbo.Table_010_SaleFactor.Column30 -
            dbo.Table_010_SaleFactor.Column31 - dbo.Table_010_SaleFactor.Column33
            + dbo.Table_010_SaleFactor.Column32 AS Column28, ISNULL(SETTLE2.Cash, 0) AS Cash, ISNULL(SETTLE2.Chq, 0) AS Chq, 
            ISNULL(SETTLE2.Discount, 0) AS Discount, 
            ISNULL(SETTLE2.ReturnGoods, 0) AS ReturnGoods, ISNULL(SETTLE2.FromFactor, 0) AS FromFactor, ISNULL(SETTLE2.ToFactor, 0) AS ToFactor
            FROM          (SELECT     Column01, SUM(Cash) AS Cash, SUM(Chq) AS Chq, SUM(Discount) AS Discount, SUM(ReturnGoods) AS ReturnGoods, SUM(FromFactor) AS FromFactor, SUM(ToFactor) 
            AS ToFactor
            FROM          (SELECT     Column01, CASE WHEN Column03 = 1 THEN Column04 ELSE 0 END AS Cash, CASE WHEN Column03 = 2 THEN Column04 ELSE 0 END AS Chq, 
            CASE WHEN Column03 = 3 THEN Column04 ELSE 0 END AS Discount, CASE WHEN Column03 = 4 THEN Column29 ELSE 0 END AS ReturnGoods, 
            CASE WHEN Column03 = 5 THEN Column04 ELSE 0 END AS FromFactor, CASE WHEN Column03 = 6 THEN Column04 ELSE 0 END AS ToFactor
            FROM          dbo.Table_034_SaleFactor_Child3) AS SettleTable
            GROUP BY Column01) AS SETTLE2 RIGHT OUTER JOIN
            dbo.Table_010_SaleFactor ON SETTLE2.Column01 = dbo.Table_010_SaleFactor.columnid) AS TotalTable", ConSale);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            return (Table);
        }

        public DataTable CustomerSettleTable(int CustomerCode)
        {
            DataTable Table = SettleTable();
            Table.DefaultView.RowFilter = "Column03=" + CustomerCode;
            return Table.DefaultView.ToTable();
        }

        public string[] CustomerSettleInfo(SqlConnection ConSale, int CusotmerCode)
        {
            string[] Info = new string[5];
            Int64 FinalRemain = 0;
            DataTable Table = CustomerSettleTable(CusotmerCode);
            Info[0]="تعداد فاکتورهای باز:"+ Table.Compute("Count(Column03)", "Remain>0").ToString();
            Int64 TotalFactor = Convert.ToInt64(Convert.ToDouble(Table.Compute(
                "SUM(Column28)", "").ToString()));
            Int64 TotalPaid=Convert.ToInt64(Convert.ToDouble(Table.Compute("SUM(TotalSettle)","").ToString()));
            Int64 TotalSettle_Paid = Convert.ToInt64(Convert.ToDouble(Table.Compute("SUM(TotalSettle_Paid)", "").ToString()));
            Info[1] = "جمع مبالغ فاکتورها: " + Class_BasicOperation.ToRial(TotalFactor);
            Info[2] = "جمع مبالغ پرداخت شده: " + Class_BasicOperation.ToRial(TotalPaid);
            Info[3] = "مبلغ مانده: " + Class_BasicOperation.ToRial((TotalFactor - TotalSettle_Paid));
            FinalRemain = TotalFactor - TotalSettle_Paid;
            Info[4] = FinalRemain.ToString();
            return Info;

        }
      
    }
}
