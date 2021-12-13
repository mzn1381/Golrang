using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace PCLOR.Classes
{
    class Class_Discounts
    {

        public double VolumeGroup(double NetTotal, int CustomerCode, string Date)
        {
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                ConSale.Open();

                SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_045_PersonScope where Column01=" + CustomerCode, ConBase);
                DataTable GroupTable = new DataTable();
                Adapter.Fill(GroupTable);
                double Percent = 0;
                foreach (DataRow item in GroupTable.Rows)
                {
                    SqlCommand Select = new SqlCommand(@"SELECT   ISNULL(SUM(column07), 0) AS SumofDiscount
                FROM         dbo.Table_025_Discount_Customer_Group
                WHERE     (column02 = 1) AND (column01 ={0}) AND (column03 <= '{1}') AND (column04 >= '{1}' AND (column05 <= {2}) AND (column06 >= {2}))", ConSale);
                    Select.CommandText = string.Format(Select.CommandText, item["Column02"].ToString(), Date, NetTotal);
                    Percent += double.Parse(Select.ExecuteScalar().ToString());
                }
                double Discount = double.Parse(Math.Round(Percent * NetTotal / 100).ToString());
                return Discount;
            }
        }

        public double SpecialGroup(double Total, int CustomerCode, string Date)
        {
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_045_PersonScope where Column01=" + CustomerCode, ConBase);
            DataTable GroupTable = new DataTable();
            Adapter.Fill(GroupTable);
            double Percent = 0;
            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                ConSale.Open();
                foreach (DataRow item in GroupTable.Rows)
                {
                    SqlCommand Select = new SqlCommand(@"SELECT   ISNULL(SUM(column07), 0) AS SumofDiscount
                FROM         dbo.Table_025_Discount_Customer_Group
                WHERE     (column02 = 0) AND (column01 ={0}) AND (column03 <= '{1}') AND (column04 >= '{1}')", ConSale);
                    Select.CommandText = string.Format(Select.CommandText, item["Column02"].ToString(), Date, Total);
                    Percent += double.Parse(Select.ExecuteScalar().ToString());
                }
                double Discount = double.Parse(Math.Round(Percent * Total / 100).ToString());
                return Discount;
            }
        }

        public double SpecialCustomer(double Total, int CustomerCode, string Date)
        {
            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                ConSale.Open();
                double Percent = 0;
                SqlCommand Select = new SqlCommand(@"SELECT     ISNULL(SUM(Column06), 0) AS Discount
                FROM         dbo.Table_026_Discount_SpecialCustomer
                WHERE     (Column01 = {0}) AND (Column02 <= '{1}') AND (Column03 >= '{1}') AND
                (Column04 <= {2}) AND (Column05 >= {2})", ConSale);
                Select.CommandText = string.Format(Select.CommandText, CustomerCode, Date, Total);
                Percent = double.Parse(Select.ExecuteScalar().ToString());
                double Discount = double.Parse(Math.Round(Percent * Total / 100).ToString());
                return Discount;
            }
        }
        public double SpecialCustomer(double Total, int SaleType )
        {
            using (SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.PBASE))
            {
                ConBASE.Open();
                double Percent = 0;
                SqlCommand Select = new SqlCommand(@"SELECT     ISNULL(Column18, 0) AS Discount
                FROM         dbo.Table_002_SalesTypes
                WHERE   columnid=" + SaleType + "   ", ConBASE);
                
                Percent = double.Parse(Select.ExecuteScalar().ToString());
                double Discount = double.Parse(Math.Round(Percent * Total / 100).ToString());
                return Discount;
            }
        }

//        public double VolumeGroup_double(double NetTotal, int CustomerCode, string Date)
//        {
//            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
//            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
//            {
//                ConSale.Open();

//                SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_045_PersonScope where Column01=" + CustomerCode, ConBase);
//                DataTable GroupTable = new DataTable();
//                Adapter.Fill(GroupTable);
//                double Percent = 0;
//                foreach (DataRow item in GroupTable.Rows)
//                {
//                    SqlCommand Select = new SqlCommand(@"SELECT   ISNULL(SUM(column07), 0) AS SumofDiscount
//                FROM         dbo.Table_025_Discount_Customer_Group
//                WHERE     (column02 = 1) AND (column01 ={0}) AND (column03 <= '{1}') AND (column04 >= '{1}' AND (column05 <= {2}) AND (column06 >= {2}))", ConSale);
//                    Select.CommandText = string.Format(Select.CommandText, item["Column02"].ToString(), Date, NetTotal);
//                    Percent += double.Parse(Select.ExecuteScalar().ToString());
//                }
//                double Discount = double.Parse(Math.Round(Percent * NetTotal / 100).ToString());
//                return Discount;
//            }
//        }

//        public double SpecialGroup_double(double Total, int CustomerCode, string Date)
//        {
//            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
//            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_045_PersonScope where Column01=" + CustomerCode, ConBase);
//            DataTable GroupTable = new DataTable();
//            Adapter.Fill(GroupTable);
//            double Percent = 0;
//            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
//            {
//                ConSale.Open();
//                foreach (DataRow item in GroupTable.Rows)
//                {
//                    SqlCommand Select = new SqlCommand(@"SELECT   ISNULL(SUM(column07), 0) AS SumofDiscount
//                FROM         dbo.Table_025_Discount_Customer_Group
//                WHERE     (column02 = 0) AND (column01 ={0}) AND (column03 <= '{1}') AND (column04 >= '{1}')", ConSale);
//                    Select.CommandText = string.Format(Select.CommandText, item["Column02"].ToString(), Date, Total);
//                    Percent += double.Parse(Select.ExecuteScalar().ToString());
//                }
//                double Discount = double.Parse(Math.Round(Percent * Total / 100).ToString());
//                return Discount;
//            }
//        }

//        public Double SpecialCustomer_double(double Total, int CustomerCode, string Date)
//        {
//            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
//            {
//                ConSale.Open();
//                double Percent = 0;
//                SqlCommand Select = new SqlCommand(@"SELECT     ISNULL(SUM(Column06), 0) AS Discount
//                FROM         dbo.Table_026_Discount_SpecialCustomer
//                WHERE     (Column01 = {0}) AND (Column02 <= '{1}') AND (Column03 >= '{1}') AND
//                (Column04 <= {2}) AND (Column05 >= {2})", ConSale);
//                Select.CommandText = string.Format(Select.CommandText, CustomerCode, Date, Total);
//                Percent = double.Parse(Select.ExecuteScalar().ToString());
//                double Discount = double.Parse(Math.Round(Percent * Total / 100).ToString());
//                return Discount;
//            }
//        }
    }
}
