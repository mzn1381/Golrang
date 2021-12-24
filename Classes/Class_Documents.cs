using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
namespace PCLOR.Classes
{
  public  class Class_Documents
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);

        //حذف دیتیل با آی دی سند
        public int DeleteDetail_ID(int DocID, Int16 Type, int Ref)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                try
                {
                    SqlCommand Del = new SqlCommand("Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=" + Type + " and Column17=" + Ref, ConAcnt);
                    return (Del.ExecuteNonQuery());
                }
                catch
                {
                    throw new Exception("حذف سند حسابداری امکان پذیر نمی باشد");
                }
            }
        }
        //برگرداندن ماکزیمم شماره در یک جدول
        public int MaxNumber(string ConnectionString, string TableName, string ColumnName)
        {
            using (SqlConnection Con = new SqlConnection(ConnectionString))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("SELECT ISNULL(Max(" + ColumnName + "),0)+1 as ID from " + TableName, Con);
                int ID = int.Parse(Command.ExecuteScalar().ToString());
                return ID;
            }
        }
        internal bool IsGood(string GoodId)
        {
            using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                ConWare.Open();
                SqlCommand Com = new SqlCommand("Select Column29 from table_004_CommodityAndIngredients where ColumnId=" + GoodId, ConWare);
                bool Result = Convert.ToBoolean(Com.ExecuteScalar().ToString());
                return !Result;
            }
        }

        internal bool AllService(System.Windows.Forms.BindingSource TableBindSource)
        {
            bool Check = true;
            foreach (DataRowView item in TableBindSource)
            {
                if (IsGood(item["Column02"].ToString()))
                    Check = false;
            }
            return Check;
        }
        public void ExScalarvoid(string ConString, string Commad)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand(Commad, Con);
                Select.ExecuteScalar();
            }
        }
        //شماره سند وارد شده معتبر است؟
       
        public void IsValidNumberS(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Count(Column00) from Table_060_SanadHead where ColumnId=" + DocNum, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                if (Num == 0)
                    throw new Exception("شماره سند مورد نظر نامعتبر می باشد");
            }
        }
        public bool IsDocNumberValid(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Count(Column00) from Table_060_SanadHead where Column00=" + DocNum, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                if (Num == 0)
                    return false;
                else return true;
            }
        }
        //سند در حالت یادداشت قرار دارد؟
        public void IsDraft(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Column02 from Table_060_SanadHead where Column00=" + DocNum, ConAcnt);
                Int16 Type = Int16.Parse(Command.ExecuteScalar().ToString());
                if (Type == 3)
                    throw new Exception("سند مورد نظر در حالت یادداشت می باشد");
            }

        }
        int _ID;
        //آخرین آی دی
        public int LastID(SqlConnection Con, string TableName, string ColumnName)
        {
            if (Con.State != ConnectionState.Open)
                Con.Open();
            SqlCommand Command = new SqlCommand("SELECT ISNULL(Max(" + ColumnName + "),0)+1  from " + TableName, Con);
            _ID = int.Parse(Command.ExecuteScalar().ToString());
            return _ID;
        }


        //شماره سند معادل آی دی سند را بر می گرداند
        public int DocNum(int DocId)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Column00 from Table_060_SanadHead where ColumnId=" + DocId, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                return Num;
            }
        }

        public double[] LastLinearDiscount(int CustomerId, int GoodId)
        {
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     TOP (1) PERCENT dbo.Table_010_SaleFactor.column02 AS Date, dbo.Table_010_SaleFactor.column03 AS CustomerId, 
            dbo.Table_011_Child1_SaleFactor.column02 AS GoodID, dbo.Table_011_Child1_SaleFactor.column16 AS Discount, 
            dbo.Table_011_Child1_SaleFactor.column18 AS Extra, 
            dbo.Table_010_SaleFactor.columnid AS SaleID
            FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
            dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
            WHERE     (dbo.Table_010_SaleFactor.column03 = " + CustomerId + @") AND (dbo.Table_011_Child1_SaleFactor.column02 = " + GoodId + @")
            ORDER BY Date DESC, SaleID DESC", ConSale);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            double[] array = new double[2];
            if (Table.Rows.Count == 0)
                return array;
            else
            {
                array[0] = Convert.ToDouble(Table.Rows[0]["Discount"].ToString());
                array[1] = Convert.ToDouble(Table.Rows[0]["Extra"].ToString());
                return array;
            }
        }
        //internal bool IsGood(string GoodId)
        //{
        //    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS))
        //    {
        //        ConWare.Open();
        //        SqlCommand Com = new SqlCommand("Select Column29 from table_004_CommodityAndIngredients where ColumnId=" + GoodId, ConWare);
        //        bool Result = Convert.ToBoolean(Com.ExecuteScalar().ToString());
        //        return !Result;
        //    }
        //}


        //تغییر حالت سند 
        public void ChangeDocType(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Sum(Column11),0) as Bed,ISNULL(Sum(Column12),0) as Bes  from Table_065_SanadDetail where Column00=" +
                    DocID(DocNum), ConAcnt);
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                if (Reader.HasRows)
                {
                    Int64 Bed = Int64.Parse(Reader["Bed"].ToString());
                    Int64 Bes = Int64.Parse(Reader["Bes"].ToString());
                    Reader.Close();
                    if (Bed != Bes)
                    {
                        SqlCommand Update = new SqlCommand("Update Table_060_SanadHead Set Column02=3 where ColumnId=" +
                            DocID(DocNum), ConAcnt);
                        Update.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand Update = new SqlCommand("Update Table_060_SanadHead Set Column02=1 where ColumnId=" +
                            DocID(DocNum), ConAcnt);
                        Update.ExecuteNonQuery();
                    }
                }
            }
        }
        //تغییر حالت سند 
        public void ChangeDocTypeUser(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Sum(Column11),0) as Bed,ISNULL(Sum(Column12),0) as Bes  from Table_065_SanadDetail where Column00=" +
                    DocID(DocNum) + "  ", ConAcnt);
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                if (Reader.HasRows)
                {
                    Int64 Bed = Int64.Parse(Reader["Bed"].ToString());
                    Int64 Bes = Int64.Parse(Reader["Bes"].ToString());
                    Reader.Close();
                    if (Bed != Bes)
                    {
                        SqlCommand Update = new SqlCommand("Update Table_060_SanadHead Set Column02=3 where ColumnId=" +
                            DocID(DocNum), ConAcnt);
                        Update.ExecuteNonQuery();
                    }
                    else
                    {
                        //SqlCommand Update = new SqlCommand("Update Table_060_SanadHead Set Column02=1 where ColumnId=" +
                        //    DocID(DocNum), ConAcnt);
                        //Update.ExecuteNonQuery();
                    }
                }
            }
        }

        //تاریخ سند مورد نظر را بر می گرداند

        public string DocDateS(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column01 from Table_060_SanadHead where ColumnId=" + DocNum, ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }
        public string DocDate(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column01 from Table_060_SanadHead where ColumnId=" + DocID(DocNum), ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }


        public string[] Signature(int RowNumber)
        {
            using (SqlConnection ConSub = new SqlConnection(Properties.Settings.Default.PBASE))
            {
                ConSub.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_125_Signature where ColumnId=" + RowNumber, ConSub);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                string[] Names = new string[8];
                Names[0] = (Table.Rows[0]["Column01"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column01"].ToString());
                Names[1] = (Table.Rows[0]["Column02"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column02"].ToString());
                Names[2] = (Table.Rows[0]["Column03"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column03"].ToString());
                Names[3] = (Table.Rows[0]["Column04"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column04"].ToString());
                Names[4] = (Table.Rows[0]["Column05"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column05"].ToString());
                Names[5] = (Table.Rows[0]["Column06"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column06"].ToString());
                Names[6] = (Table.Rows[0]["Column07"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column07"].ToString());
                Names[7] = (Table.Rows[0]["Column08"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column08"].ToString());
                return Names;
            }
        }

        //شماره آخرین سند را بر می گرداند
        public int LastDocNum()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }

        //شرح سند شماره سند دلخواه را بر می گرداند
        public string Cover(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column04 from Table_060_SanadHead where ColumnId=" + DocID(DocNum), ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }
        public string CoverS(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column04 from Table_060_SanadHead where ColumnId=" + DocNum, ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }
        public void IsValidNumber(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Count(Column00) from Table_060_SanadHead where Column00=" + DocNum, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                if (Num == 0)
                    throw new Exception("شماره سند مورد نظر نامعتبر می باشد");
            }
        }
        //بدهکار یا بستانکار تراکنش خاصی را بر می گرداند
        public string Account(Int16 ID, string ColumnName)
        {
            SqlConnection ConPBASE = new SqlConnection(Properties.Settings.Default.PBASE);
            DataTable Table = ReturnTable(ConPBASE, "Select " + ColumnName + " from Table_105_SystemTransactionInfo where Column00=" + ID);
            if (Table.Rows.Count == 0)
                return "";
            else return Table.Rows[0][0].ToString();
        }


        //صدور هدر سند//
        public int ExportDoc_Header(int Number, string Date, string Cover, string UserName, Int16 SanadType)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                int SanadNumber = 0;
                SqlParameter Key = new SqlParameter("Key", System.Data.SqlDbType.Int);
                Key.Direction = System.Data.ParameterDirection.Output;
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_060_SanadHead 
                        (Column00,Column01,Column02,Column03,Column04,Column05,Column06) VALUES
                    (" + Number + ",'" + Date + "'," + SanadType + ",0,'" + Cover + "','" + UserName + "',getdate()); SET @Key=SCOPE_IDENTITY()", ConAcnt);
                Insert.Parameters.Add(Key);
                Insert.ExecuteNonQuery();
                SanadNumber = int.Parse(Key.Value.ToString());
                return SanadNumber;
            }
        }

        public int ExportDoc_Header_Con(SqlConnection Con, int Number, string Date, string Cover, string UserName, Int16 SanadType)
        {

            int SanadNumber = 0;
            SqlParameter Key = new SqlParameter("Key", System.Data.SqlDbType.Int);
            Key.Direction = System.Data.ParameterDirection.Output;
            SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_060_SanadHead 
            (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
            VALUES(" + Number + ",'" + Date + "'," + SanadType + ",0,'" + Cover + "','" + UserName +
                     "',getdate()); SET @Key=SCOPE_IDENTITY()", Con);
            Insert.Parameters.Add(Key);
            Insert.ExecuteNonQuery();
            SanadNumber = int.Parse(Key.Value.ToString());
            return SanadNumber;
        }


        //صدور دیتیل سند//
        public int ExportDoc_Detail(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
           string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
           Int64 Bes, double CurBed, double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, double CurPrice, short? SubSys)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";


            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                Key.Direction = ParameterDirection.Output;
                ConAcnt.Open();
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) VALUES(" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," +
                    CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" +
                    RegUser + "',getdate()," + CurPrice + "," + (SubSys == (short?)null ? "NULL" : SubSys.ToString()) + "); SET @Key=SCOPE_IDENTITY()", ConAcnt);
                Insert.Parameters.Add(Key);
                Insert.ExecuteNonQuery();
                return int.Parse(Key.Value.ToString());
            }
        }


        public void ExportDoc_Detail(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
           string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
           Int64 Bes, Double CurBed, Double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, Double CurPrice, object Period, object EffDate)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";


            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]      ,[Column23]      ,[Column24]) VALUES (" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," + CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" + RegUser + "',getdate()," + CurPrice + "," +
                    Period + "," + EffDate + ")", ConAcnt);

                Insert.ExecuteNonQuery();
            }

        }

        public void ExportDoc_Detail_BANK(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
                   string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
                   Int64 Bes, Double CurBed, Double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, Double CurPrice, object Period, object EffDate)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";


            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]      ,[Column23]      ,[Column24]) VALUES (" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," + CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" + RegUser + "',getdate()," + CurPrice + "," +
                    Period + "," + EffDate + ")", ConAcnt);

                Insert.ExecuteNonQuery();
            }

        }
        public void ExportDoc_Detail(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
           string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
           Int64 Bes, Double CurBed, Double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, Double CurPrice)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";


            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) VALUES (" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," + CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" + RegUser + "',getdate()," + CurPrice + ")", ConAcnt);
                Insert.ExecuteNonQuery();
            }
        }
        public string ExScalarNULL(string ConString, string TableName, string ColumnName, string ConditionColumn, string ConValue)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("select isnull(( SELECT " + ColumnName + " FROM " + TableName + " WHERE " + ConditionColumn + "=" + ConValue + "),'') as Res", Con);
                return Select.ExecuteScalar().ToString();
            }
        }



//        public void ExportDoc_Detail(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
//           string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
//           Int64 Bes, Double CurBed, Double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, Double CurPrice, object Period, object EffDate)
//        {
//            if (string.IsNullOrEmpty(Moein))
//                Moein = "NULL";
//            else Moein = "'" + Moein + "'";
//            if (string.IsNullOrEmpty(Tafsili))
//                Tafsili = "NULL";
//            else Tafsili = "'" + Tafsili + "'";
//            if (string.IsNullOrEmpty(Joz))
//                Joz = "NULL";
//            else Joz = "'" + Joz + "'";

//            if (string.IsNullOrEmpty(Person))
//                Person = "NULL";

//            if (string.IsNullOrEmpty(Center))
//                Center = "NULL";

//            if (string.IsNullOrEmpty(Project))
//                Project = "NULL";


//            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
//            {
//                ConAcnt.Open();
//                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
//              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
//              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
//              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]      ,[Column23]      ,[Column24]) VALUES (" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
//                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," + CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" + RegUser + "',getdate()," + CurPrice + "," +
//                    Period + "," + EffDate + ")", ConAcnt);
//                Insert.ExecuteNonQuery();
//            }
//        }

        public string ShablonDescGenerate(string ConBase, int Type, string SubTitle, string Date, string FinalDate,
          string Number, string BankJari, string BankVagozari, string FirstPerson, string SecondPerson,
          string Babat, string AccName, string AmelBank)
        {
            SqlConnection Con = new SqlConnection(ConBase);
            string Title = ExScalar(ConBase, @"SELECT    Column01  FROM         Table_285_SignatureTitle where Columnid=" + Type);
            DataTable dt = ReturnTable(Con, @"SELECT    *  FROM         Table_290_SignatureChild where Column01=" + Type);
            string Sort = dt.Rows[0][2].ToString();

            string[] item = Sort.Split(';');
            string DescGenerate = "";
            for (int i = 0; i < item.Length; i++)
            {
                if (item[i] == "1")//عنوان
                {

                    DescGenerate = SubTitle;
                }
                else if (item[i] == "2")//تاریخ
                { DescGenerate = DescGenerate + " " + (Date != "" ? dt.Rows[0][4].ToString() + " " + Date : ""); }
                else if (item[i] == "3")//سر رسید
                { DescGenerate = DescGenerate + " " + (FinalDate != "" ? dt.Rows[0][5].ToString() + " " + FinalDate : ""); }
                else if (item[i] == "4")//شماره 
                { DescGenerate = DescGenerate + " " + (Number != "" ? dt.Rows[0][6].ToString() + " " + Number : ""); }
                else if (item[i] == "5")//بانک جاری
                { DescGenerate = DescGenerate + " " + (BankJari != "" ? dt.Rows[0][7].ToString() + " " + BankJari : ""); }
                else if (item[i] == "6")//بانک واگذاری
                { DescGenerate = DescGenerate + " " + (BankVagozari != "" ? dt.Rows[0][8].ToString() + " " + BankVagozari : ""); }
                else if (item[i] == "7")//طرف حساب اصلی
                { DescGenerate = DescGenerate + " " + (FirstPerson != "" ? dt.Rows[0][9].ToString() + " " + FirstPerson : ""); }
                else if (item[i] == "8")//طرف حساب فرعی
                { DescGenerate = DescGenerate + " " + (SecondPerson != "" ? dt.Rows[0][10].ToString() + " " + SecondPerson : ""); }
                else if (item[i] == "9")//بابت
                { DescGenerate = DescGenerate + " " + (Babat != "" ? dt.Rows[0][11].ToString() + " " + Babat : ""); }
                else if (item[i] == "10")//حساب
                { DescGenerate = DescGenerate + " " + (AccName != "" ? dt.Rows[0][12].ToString() + " " + AccName : ""); }
                else if (item[i] == "11")//بانک عامل اسناد
                { DescGenerate = DescGenerate + " " + (AmelBank != "" ? dt.Rows[0][13].ToString() + " " + AmelBank : ""); }


            }
            return DescGenerate;


        }

        public void ExportDoc_Detail_Con(SqlConnection Con, int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
            string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
            Int64 Bes, Double CurBed, Double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, Double CurPrice)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";



            SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
      ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
      ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
      ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) VALUES (" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," + CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" + RegUser + "',getdate()," + CurPrice + ")", Con);
            Insert.ExecuteNonQuery();
        }

        //آپدیت شماره سند جداول//
        public void Update_Des_Table(string ConString, string TableName, string MainColumn, string ConditionColumn, int ConditionValue, int Value)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Update = new SqlCommand("UPDATE " + TableName + " SET " +
                    MainColumn + "=" + Value + " where " + ConditionColumn + "=" + ConditionValue, Con);
                Update.ExecuteNonQuery();
            }
        }

        //اجرای دستور سی کولی که فقط یک مقدار را بر می گرداند
        public string ExScalar(string ConString, string TableName, string ColumnName, string ConditionColumn, string ConValue)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("SELECT " + ColumnName + " FROM " + TableName + " WHERE " + ConditionColumn + "=" + ConValue, Con);
                return Select.ExecuteScalar().ToString();
            }
        }

        //اجرای دستور سی کولی که فقط یک مقدار را بر می گرداند
        public string ExScalar(string ConString, string query)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "-1";
                }
                Con.Open();
                SqlCommand Select = new SqlCommand(query, Con);
                return Select.ExecuteScalar().ToString();
            }
        }
        //اجرای دستور سی کولی که فقط یک مقدار را بر می گرداند
        public void Execute(string ConString, string query)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand(query, Con);
                Select.ExecuteNonQuery().ToString();
            }
        }

        public bool ExExists(string ConString, string TableName, string ColumnName, string Value)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("SELECT CASE WHEN EXISTS(SELECT 1 FROM " + TableName + " WHERE " + ColumnName + "=" + Value.ToString() + ") THEN 1 ELSE 0 END", Con);
                return Convert.ToBoolean(Select.ExecuteScalar());
            }
        }




        //برگراندن شماره پیشنهادی برگه پرداخت//
        public string SuggestPayPaperNumber()
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                SqlCommand Select = new SqlCommand("Select ISNULL(Max(Column01),0)+1 as SugName from Table_040_CashPayments", ConPBANK);
                return Select.ExecuteScalar().ToString();
            }
        }


        //پیشنهاد پشت نمره چک دریافتی
        public string SuggetstBackNumber()
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                SqlCommand Select = new SqlCommand("Select Isnull(Max(Column00),0)+1 as SugNum from Table_035_ReceiptCheques", ConPBANK);
                return Select.ExecuteScalar().ToString();
            }
        }


        //چک کردن اینکه آیا این چک قبلا صادر شده است---در هنگام صدور اسناد پرداختنی
        public bool ExportedBefore(string PBANKBoxId, string BatchId, string ChqNumber)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                SqlCommand Select = new SqlCommand("Select Count(*) from Table_030_ExportCheques where Column01=" + PBANKBoxId + " and Column02=" + BatchId + " and Column03=" + ChqNumber, ConPBANK);
                int Count = 0;
                Count = int.Parse(Select.ExecuteScalar().ToString());
                if (Count == 0)
                    return false;
                else
                    return true;
            }
        }


        //برگرداندن اطلاعات سرفصل انتخاب شده در هنگام صدور سند
        public string[] ACC_Info(string ACC_Code)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select GroupCode,KolCode,MoeinCode,TafsiliCode,JozCode from AllHeaders() where ACC_Code='" + ACC_Code + "'", ConAcnt);
                using (SqlDataReader Reader = Select.ExecuteReader())
                {
                    string[] Codes = new string[5];
                    while (Reader.Read())
                    {
                        Codes[0] = Reader[0].ToString();
                        Codes[1] = Reader[1].ToString();
                        Codes[2] = Reader[2].ToString();
                        Codes[3] = Reader[3].ToString();
                        Codes[4] = Reader[4].ToString();
                    }
                    return Codes;
                }
            }
        }

        /// <summary>
        /// برگرداندن اطلاعات سرفصل با مقدار نال برای ستونهای نال
        /// </summary>
        /// <param name="ACC_Code">سرفصل مربوط</param>
        /// <returns></returns>
        public string[] ACC_Info_2(string ACC_Code)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select GroupCode,KolCode,MoeinCode,TafsiliCode,JozCode from AllHeaders() where ACC_Code='" + ACC_Code + "'", ConAcnt);
                using (SqlDataReader Reader = Select.ExecuteReader())
                {
                    string[] Codes = new string[5];
                    while (Reader.Read())
                    {
                        Codes[0] = Reader[0].ToString();
                        Codes[1] = Reader[1].ToString();
                        Codes[2] = (Reader[2].ToString().Trim() == "" ? "NULL" : Reader[2].ToString().Trim());
                        Codes[3] = (Reader[3].ToString().Trim() == "" ? "NULL" : Reader[3].ToString().Trim());
                        Codes[4] = (Reader[4].ToString().Trim() == "" ? "NULL" : Reader[4].ToString().Trim());
                    }
                    return Codes;
                }
            }
        }

        // اضافه کردن چک دریافت/پرداخت شده به جدول وضعیتها- در صورت موجود بودن فقط آپدیت می شود
        public void AddingStatus(bool ActionType, int PaperID)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                if (!ActionType)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_035_ReceiptCheques where ColumnId=" + PaperID, ConPBANK);
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);
                    DataRow Row = Table.Rows[0];
                    Adapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" + PaperID + " and Column13=0", ConPBANK);
                    DataTable StatusTable = new DataTable();
                    Adapter.Fill(StatusTable);
                    if (StatusTable.Rows.Count == 1)
                    {
                        //Update Information//
                        SqlCommand Update = new SqlCommand("Update Table_065_TurnReception SET Column02=" + Row["Column48"] +
                            " , Column04='" + Row["Column02"] + "' ,Column06=" + Row["Column01"].ToString() + ", Column05=" + Row["Column07"] + " , Column15= " + (Row["Column15"].ToString() == "" ? "NULL" : Row["Column15"].ToString()) +
                            " where ColumnId=" + StatusTable.Rows[0]["ColumnId"], ConPBANK);
                        Update.ExecuteNonQuery();
                    }
                    else if (StatusTable.Rows.Count == 0)
                    {
                        //Insert New Row
                        SqlCommand Insert = new SqlCommand("INSERT INTO Table_065_TurnReception VALUES (" +
                            Row["ColumnId"] + "," + Row["Column48"] + ",'" + Row["Column02"] + "'," + (Row["Column07"].ToString().Trim() == "" ? "NULL" : Row["Column07"].ToString()) + "," + Row["Column01"].ToString() + ",null,null,null,null,null,null,0,null," + (Row["Column15"].ToString().Trim() == "" ? "NULL" : Row["Column15"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),null)", ConPBANK);
                        Insert.ExecuteNonQuery();
                    }

                }
                else if (ActionType)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_030_ExportCheques where ColumnId=" + PaperID, ConPBANK);
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);
                    DataRow Row = Table.Rows[0];
                    Adapter = new SqlDataAdapter("Select * from Table_070_TurnPaid where Column01=" + PaperID + " and Column13=0", ConPBANK);
                    DataTable StatusTable = new DataTable();
                    Adapter.Fill(StatusTable);
                    if (StatusTable.Rows.Count == 1)
                    {
                        //Update Information//
                        SqlCommand Update = new SqlCommand("Update Table_070_TurnPaid SET Column02=" + Row["Column38"] +
                            " , Column04='" + Row["Column04"] + "' , Column05=" + (Row["Column09"].ToString() == "" ? "NULL" : Row["Column09"].ToString()) +
                            " , Column06= " + (Row["Column29"].ToString() == "" ? "NULL" : Row["Column29"].ToString()) + ", Column07='" + (Row["Column30"].ToString() == "" ? "NULL" : Row["Column30"].ToString()) + "' , Column08=" + (Row["Column31"].ToString() == "" ? "NULL" : Row["Column31"].ToString()) + " , Column09='" + (Row["Column32"].ToString() == "" ? "NULL" : Row["Column32"].ToString()) + "' , Columb10='" + (Row["Column33"].ToString() == "" ? "NULL" : Row["Column33"].ToString()) + "' , Column11='" + (Row["Column34"].ToString() == "" ? "NULL" : Row["Column34"].ToString()) + "' , Column12='" + (Row["Column35"].ToString() == "" ? "NULL" : Row["Column35"].ToString()) +
                            "' ,Column14=" + (Row["Column36"].ToString() == "" ? "NULL" : Row["Column36"].ToString()) + " , Column15=" + (Row["Column37"].ToString() == "" ? "NULL" : Row["Column37"].ToString()) +
                            " where ColumnId=" + StatusTable.Rows[0]["ColumnId"], ConPBANK);
                        Update.ExecuteNonQuery();
                    }
                    else if (StatusTable.Rows.Count == 0)
                    {
                        //Insert New Row


                        SqlCommand Insert = new SqlCommand("INSERT INTO Table_070_TurnPaid VALUES (" +
                            Row["ColumnId"] + "," + Row["Column38"] + "'" + Row["Column04"] + "'," + Row["Column09"] + ", " + Row["Column29"]
                            + ",'" + Row["Column30"] + "'," + Row["Column31"] + ",'" + Row["Column32"] + "','" + Row["Column33"] + "','" + Row["Column34"] + "','" + Row["Column35"] + "'," + Row["Column36"] + "," + Row["Column37"] + ","
                            + Class_BasicOperation._UserName + ",getdate())", ConPBANK);
                        Insert.CommandText = Insert.CommandText.Replace(",,", ",null,");
                        Insert.ExecuteNonQuery();
                    }

                }
            }
        }


        //اضافه کردن گردش چکهای دریافتی به جدول گردش دریافتها
        public int AddTurnReception(int PaperID, Int16 Status, string Date, string Person, string BoxPBANK, string ACC, Int16 Group, string Kol, string Moein, string Tafsili, string Joz,
            int SanadID, string Center, string Project, string User, string Description, string Currency, string CurrencyType, Double CurrencyValue)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                int ID = 0;
                SqlCommand Insert = new SqlCommand("INSERT INTO Table_065_TurnReception VALUES(" + PaperID + "," + Status + ",'" + Date + "'," +
                    Person + "," + BoxPBANK + ",'" + ACC + "'," + Group + "," + Kol + "," + Moein + "," + Tafsili + "," + Joz + "," + SanadID + "," + Center + "," + Project + ",'" + User + "',getdate(),"
                    + (Description.ToString().Trim() == "" ? "NULL" : "'" + Description + "'") +
                    "," + (Currency == "True" ? 1 : 0) +
                    "," + (string.IsNullOrEmpty(CurrencyType) ? "NULL" : CurrencyType) + "," + CurrencyValue +
                    "); SET @ID=SCOPE_IDENTITY()", ConPBANK);
                SqlParameter _ID = new SqlParameter("ID", SqlDbType.BigInt);
                _ID.Direction = ParameterDirection.Output;
                Insert.Parameters.Add(_ID);
                Insert.ExecuteNonQuery();
                ID = int.Parse(_ID.Value.ToString());
                return ID;
            }
        }


        //اضافه کردن گردش چکهای پرداختی به جدول گردش پرداختها
        public int AddTurnExported(int PaperID, Int16 Status, string Date, string Person, string BoxPBANK, string ACC, Int16 Group, string Kol, string Moein, string Tafsili, string Joz,
            int SanadID, string Center, string Project, string User, string Description, string Currency, string CurrencyType, Double CurrencyValue)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                int ID = 0;
                SqlCommand Insert = new SqlCommand("INSERT INTO Table_070_TurnPaid VALUES(" + PaperID + "," + Status + ",'" + Date + "'," +
                    Person + "," + BoxPBANK + ",'" + ACC + "'," + Group + "," + Kol + "," + Moein + "," + Tafsili + "," + Joz + "," + SanadID + "," +
                    Center + "," + Project + ",'" + User + "',getdate()," + (Description.Trim() == "" ? "NULL" : "'" + Description + "'") +
                    "," + (Currency == "True" ? 1 : 0) +
                    "," + (CurrencyType == "" ? "Null" : CurrencyType) + "," + CurrencyValue + "); SET @ID=SCOPE_IDENTITY()", ConPBANK);
                SqlParameter _ID = new SqlParameter("ID", SqlDbType.BigInt);
                _ID.Direction = ParameterDirection.Output;
                Insert.Parameters.Add(_ID);
                Insert.ExecuteNonQuery();
                ID = int.Parse(_ID.Value.ToString());
                return ID;
            }
        }


        //حذف گردشهای چک دریافتی از جدول گردش دریافتها
        public void DeleteTurnReception(Int64 TurnID)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                SqlCommand Delete = new SqlCommand("Delete from Table_065_TurnReception where ColumnId=" + TurnID, ConPBANK);
                Delete.ExecuteNonQuery();
            }
        }


        //حذف گردشهای چک پرداختی از جدول گردش پرداختها
        public void DeleteTurnExported(Int64 TurnID)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                SqlCommand Delete = new SqlCommand("Delete from Table_070_TurnPaid where ColumnId=" + TurnID, ConPBANK);
                Delete.ExecuteNonQuery();
            }
        }


        //آیا چک دریافتی دارای گردش می باشد
        public bool HasTurn_Rec(int PaperID)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                bool _Has = false;
                SqlCommand Check = new SqlCommand("Select ISNULL(COUNT(ColumnId),0) from Table_065_TurnReception where Column01=" + PaperID, ConPBANK);
                int Count = int.Parse(Check.ExecuteScalar().ToString());
                if (Count > 0)
                    _Has = true;
                return _Has;
            }
        }


        //آیا چک پرداختی دارای گردش می باشد
        public bool HasTurn_Exp(int PaperID)
        {
            using (SqlConnection ConPBANK = new SqlConnection(Properties.Settings.Default.PBANK))
            {
                ConPBANK.Open();
                bool _Has = false;
                SqlCommand Check = new SqlCommand("Select ISNULL(COUNT(ColumnId),0) from Table_070_TurnPaid where Column01=" + PaperID, ConPBANK);
                int Count = int.Parse(Check.ExecuteScalar().ToString());
                if (Count > 0)
                    _Has = true;
                return _Has;
            }
        }

        //کنترل قطعی بودن یا نبودن شماره سند وارد شده توسط کاربر
        public void IsFinal(int DocNum)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand(@"Select coalesce(cast((Select Column03 from Table_060_SanadHead
                    where Column00=" + DocNum + ") AS nvarchar(20)),'NO')", Con);
                string Result = Command.ExecuteScalar().ToString();
                //if (Result == "NO")
                //    throw new Exception("سندی با شماره " + DocNum.ToString() + " وجود ندارد");
                //else 
                if (Result == "1")
                    throw new Exception("سند مورد نظر قطعی/تأیید شده می باشد");
            }
        }
        public void IsFinal_ID(int DocID)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                       (
                                                                           SELECT ISNULL(Column03, 0)
                                                                           FROM   Table_060_SanadHead
                                                                           WHERE  ColumnId = " + DocID + @"
                                                                       ),
                                                                       0
                                                                   )", Con);
                bool Final = bool.Parse(Command.ExecuteScalar().ToString());
                if (Final)
                    throw new Exception("سند مورد نظر قطعی/تأیید شده می باشد");
            }
        }
        public bool IsDocFinal(int DocNum)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("Select Column03 from Table_060_SanadHead where Column00=" + DocNum, Con);
                bool Final = bool.Parse(Command.ExecuteScalar().ToString());
                return Final;
            }
        }

        //آی دی معادل یک شماره سند را بر می گرداند
        public int DocID(int DocNum)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Commnad = new SqlCommand("Select ColumnId from Table_060_SanadHead where Column00=" + DocNum, Con);
                int ID = int.Parse(Commnad.ExecuteScalar().ToString());
                return ID;
            }
        }


        //برگرداندن آخرین تاریخ قطعی سازی
        public string LastFinalDate()
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(( select Column00 from Table_085_CloseSettingHeader where ColumnId=5),'NoColumn')", Con);
                string Result = Command.ExecuteScalar().ToString();
                return Result;
            }
        }


        //آیا حسابهای موقت  تا این تاریخ قبلا بسته شده اند
        public void HasClosed(string Date)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("select ISNULL(Max(Column01),'NoColumn') from Table_060_SanadHead where Column02=6", ConAcnt);
                string Result = Command.ExecuteScalar().ToString();
                if (!Result.Equals("NoColumn") && (Result.CompareTo(Date).ToString() == "1" || Result.CompareTo(Date).ToString() == "0"))
                    throw new Exception("حسابهای موقت تا تاریخ " + Result + " بسته شده اند");
            }
        }


        //آخرین تاریخ بستن حسابهای موقت
        public string LastCloseTempDate()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Max(Column01),'NoColumn') from Table_060_SanadHead where Column02=6", ConAcnt);
                string Result = Command.ExecuteScalar().ToString();
                return Result;
            }
        }


        //تاریخ اولین سند صادر شده
        public string FirstDocDate()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Min(Column01),'NoColumn') from Table_060_SanadHead", ConAcnt);
                string Result = Command.ExecuteScalar().ToString();
                return Result;
            }
        }


        //عدم صدور سند تا آخرین تاریخ قطعی سازی
        public void CheckForValidationDate(string Date)
        {
            string LastFinalDate = this.LastFinalDate();
            if (LastFinalDate != "NoColumn")
            {
                using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    ConAcnt.Open();
                    SqlCommand Command = new SqlCommand("Select Case When '" + Date + "'<='" +
                        LastFinalDate + "' then 1 else 0 end As Result", ConAcnt);
                    if (Command.ExecuteScalar().ToString() == "1")
                    {
                        throw new Exception("اسناد تا این تاریخ قطعی شده اند");
                    }
                }

            }
        }


        //آیا سند اختتامیه صادر شده است
        public void CheckExistFinalDoc()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Count(ColumnId),0) as Expr from Table_060_SanadHead where Column02=5", ConAcnt);
                if (int.Parse(Command.ExecuteScalar().ToString()) > 0)
                    throw new Exception("به علت وجود سند اختتامیه، صدور سند جدید امکانپذیر نمی باشد");
            }
        }

        //برگرداندن مانده حساب یک شخص در یک حساب
        public Int64 PersonRemain(int PersonId, string Acc)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                Int64 Remain = 0;
                SqlCommand Command = new SqlCommand("Select ISNULL(SUM(Column11)-SUM(Column12),0) as Remain from Table_065_SanadDetail where Column07=" +
                    PersonId + " and Column01='" + Acc + "'", ConAcnt);
                Remain = Int64.Parse(Command.ExecuteScalar().ToString());
                return Remain;
            }

        }

        public DataTable ReturnTable(SqlConnection Con, string CommandText)
        {
            DataTable Table = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, Con);
            Adapter.Fill(Table);
            return Table;
        }

        ////برگرداندن ماکزیمم شماره در یک جدول
        //public int MaxNumber(string ConString, string TableName, string ColumnName)
        //{
        //    using (SqlConnection Con = new SqlConnection(ConString))
        //    {
        //        Con.Open();
        //        SqlCommand Command = new SqlCommand("SELECT ISNULL(Max(" + ColumnName + "),0)+1 as ID from " + TableName, Con);
        //        int ID = int.Parse(Command.ExecuteScalar().ToString());
        //        return ID;
        //    }
        //}

        //اجرای دستور سیکول بدون خروجی
        public int RunSqlCommand(string ConString, string CommandText)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand(CommandText, Con);
                int i = Command.ExecuteNonQuery();
                return i;
            }
        }

        //بررسی اینکه آیا سندی با یک سرفصل خاص وجود دارد یا خیر
        public bool HeaderHasRow(string ACC_code)
        {
            using (SqlConnection ConACnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConACnt.Open();
                SqlCommand Select = new SqlCommand("Select Column01 from Table_065_SanadDetail where Column01='" + ACC_code + "'", ConACnt);
                SqlDataReader Reader = Select.ExecuteReader();
                Reader.Read();
                if (Reader.HasRows)
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Return Paper's Sanad Type
        /// </summary>
        /// <param name="ConnectinString"></param>
        /// <param name="IDColumn"></param>
        /// <param name="PaperID"></param>
        /// <returns></returns>
        public int SanadType(string ConnectinString, int DocID, int PaperID, int PaperType)
        {
            using (SqlConnection Con = new SqlConnection(ConnectinString))
            {
                Con.Open();
                //SqlCommand SelectCommand = new SqlCommand("select isnull((select distinct Column16 from Table_065_SanadDetail where Column00=" + DocID + " and Column17=" + PaperID + "),0) ", Con);
                SqlCommand SelectCommand = new SqlCommand(@"Select ISNULL((select distinct Column16 from Table_065_SanadDetail where Column00=" + DocID + " and Column17=" + PaperID + " and Column16=" + PaperType + "),0)", Con);
                return int.Parse(SelectCommand.ExecuteScalar().ToString());
            }
        }
        /// <summary>
        /// آیا سطری با آی دی و نوع سند مشخص شده در سند وجود دارد یا خیر؟
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="RefId"></param>
        /// <param name="PaperID"></param>
        /// <returns></returns>
        public int IsRowinDoc(string ConnectionString, int RefId, int PaperID)
        {
            using (SqlConnection Con = new SqlConnection(ConnectionString))
            {
                Con.Open();
                SqlCommand SelectCommand = new SqlCommand("select COUNT(*) from Table_065_SanadDetail where Column16=" + RefId + " and Column17=" + PaperID + "", Con);
                return int.Parse(SelectCommand.ExecuteScalar().ToString());
            }
        }
        /// <summary>
        /// آیا شماره آی دی سند معتبر است؟
        /// </summary>
        /// <param name="DocID"></param>
        /// <returns></returns>

        public bool IsDocIDValid(int DocID)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Count(ColumnId) from Table_060_SanadHead where ColumnId=" + DocID, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                if (Num == 0)
                    return false;
                else return true;
            }
        }

        public DataTable DocTable()
        {
            DataTable SourceTable = new DataTable();
            SourceTable.Columns.Add("Type", Type.GetType("System.Int16"));
            SourceTable.Columns.Add("Column01", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column001", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column07", Type.GetType("System.Int32"));
            SourceTable.Columns["Column07"].AllowDBNull = true;
            SourceTable.Columns.Add("Column08", Type.GetType("System.Int16"));
            SourceTable.Columns["Column08"].AllowDBNull = true;
            SourceTable.Columns.Add("Column09", Type.GetType("System.Int16"));
            SourceTable.Columns["Column09"].AllowDBNull = true;
            SourceTable.Columns.Add("Column10", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column11", Type.GetType("System.Double"));
            SourceTable.Columns.Add("Column12", Type.GetType("System.Double"));
            return SourceTable;
        }

        /// <summary>
        ///     برگرداندن عناوین امضای برگه مشخص شده
        /// </summary>
        /// <param name="RowNumber"></param>
        /// <returns></returns>
        public string[] Signature(int RowNumber, string Name)
        {
            using (SqlConnection ConSub = new SqlConnection(Properties.Settings.Default.PBASE))
            {
                ConSub.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_125_Signature where ColumnId=" + RowNumber, ConSub);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                if (Table.Rows.Count > 0)
                {
                    string[] Names = new string[8];
                    Names[0] = (Table.Rows[0]["Column01"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column01"].ToString());
                    Names[1] = (Table.Rows[0]["Column02"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column02"].ToString());
                    Names[2] = (Table.Rows[0]["Column03"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column03"].ToString());
                    Names[3] = (Table.Rows[0]["Column04"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column04"].ToString());
                    Names[4] = (Table.Rows[0]["Column05"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column05"].ToString());
                    Names[5] = (Table.Rows[0]["Column06"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column06"].ToString());
                    Names[6] = (Table.Rows[0]["Column07"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column07"].ToString());
                    Names[7] = (Table.Rows[0]["Column08"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column08"].ToString());
                    return Names;
                }
                else
                {
                    RunSqlCommand(Properties.Settings.Default.PBASE, "INSERT INTO Table_125_Signature (columnId,Column00) values (" + RowNumber + ",'" + Name.Trim() + "')");
                    Adapter.Fill(Table);
                    string[] Names = new string[8];
                    Names[0] = (Table.Rows[0]["Column01"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column01"].ToString());
                    Names[1] = (Table.Rows[0]["Column02"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column02"].ToString());
                    Names[2] = (Table.Rows[0]["Column03"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column03"].ToString());
                    Names[3] = (Table.Rows[0]["Column04"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column04"].ToString());
                    Names[4] = (Table.Rows[0]["Column05"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column05"].ToString());
                    Names[5] = (Table.Rows[0]["Column06"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column06"].ToString());
                    Names[6] = (Table.Rows[0]["Column07"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column07"].ToString());
                    Names[7] = (Table.Rows[0]["Column08"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column08"].ToString());
                    return Names;
                }
            }
        }

        /// <summary>
        ///  مقدار ستونهای  را جهت انجام عملیات های مربوط چک می کند   
        /// </summary>
        /// <param name="TableName">نام جدول</param>
        /// <param name="ColumnName">ستون عملیاتی</param>
        /// <param name="IDValue">آی دی سطر</param>
        /// <returns>آی دی برگه عملیاتی</returns>
        public int OperationalColumnValue(string TableName, string ColumnName, string IDValue, string ColumnIdName)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL(" + ColumnName + ",0) from " + TableName + " where " + ColumnIdName + "=" + IDValue, Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }

        /// <summary>
        /// کلیه گروههای مربوط به اشخاص را برمی گرداند
        /// </summary>
        /// <returns></returns>
        public DataTable PersonGroup()
        {
            SqlConnection ConPBASE = new SqlConnection(Properties.Settings.Default.PBASE);
            return ReturnTable(ConPBASE, @"Select * from(
            Select distinct Tbl2.PersonId, 
            substring((Select ','+Tbl1.GroupName   AS [text()]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl1
            Where Tbl1.PersonId = Tbl2.PersonId
              
            For XML PATH ('')),2, 1000) [PersonGroup]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl2) as PersonGroup");

        }




        /// <summary>
        ///  مقدار ستونهای عملیاتی مورد نظر را  جهت انجام عملیات های مربوط چک می کند   
        /// </summary>
        /// <param name="TableName">نام جدول</param>
        /// <param name="ColumnName">ستون عملیاتی</param>
        /// <param name="IDValue">آی دی سطر</param>
        /// <returns>آی دی برگه عملیاتی</returns>
        public int OperationalColumnValue(SqlConnection Con, string TableName, string ColumnName, string IDValue)
        {
            using (Con = new SqlConnection(Con.ConnectionString))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL(" + ColumnName + ",0) from " + TableName + " where ColumnId=" + IDValue, Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }

        public DataTable Fill_JanusMultiColumnCombo(string StrConnection, string select)
        {

            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(StrConnection);

            SqlDataAdapter DA = new SqlDataAdapter(select, con);
            DA.SelectCommand.CommandTimeout = 2000;
            DA.Fill(DT);

            return DT;
        }


        //آی دی معادل کد شخص
        public string PersonId(string PersonCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PBASE))
            {
                Con.Open();
                SqlCommand Commnad = new SqlCommand("select isnull(( Select ColumnId from Table_045_PersonInfo where Column01=" + PersonCode + "),0) as Id", Con);
                string ID = (Commnad.ExecuteScalar().ToString());
                return ID;
            }
        }


        public DataTable Fill(string connection, string query, string from, string to, DataTable project, Int16 type)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (type == 1)
                {
                    cmd.Parameters.Add("@Date1", SqlDbType.NVarChar).Value = from;
                    cmd.Parameters.Add("@Date2", SqlDbType.NVarChar).Value = to;
                    if (project.Rows.Count > 0)
                    {
                        cmd.Parameters.Add("@Project", SqlDbType.Structured).Value = project;
                    }
                }
                else
                {
                    cmd.Parameters.Add("@Number1", SqlDbType.NVarChar).Value = from;
                    cmd.Parameters.Add("@Number2", SqlDbType.NVarChar).Value = to;
                    if (project.Rows.Count > 0)
                    {
                        cmd.Parameters.Add("@Project", SqlDbType.Structured).Value = project;
                    }

                }
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                myAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int OperationalColumnValue(string TableName, string ColumnName, string IDValue)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(" + ColumnName + ",0) from " + TableName + " where ColumnId=" + IDValue + "),0)", Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }

        public int OperationalColumnValueSA(string TableName, string ColumnName, string IDValue)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(" + ColumnName + ",0) from " + TableName + " where ColumnId=" + IDValue + "),0)", Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }
        public int WHRSOperationalColumnValue(string TableName, string ColumnName, string IDValue)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(" + ColumnName + ",0) from " + TableName + " where ColumnId=" + IDValue + "),0)", Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }

       public IEnumerable<string> GetNextChars(string str, int iterateCount)
        {
            var words = new List<string>();

            for (int i = 0; i < str.Length; i += iterateCount)
                if (str.Length - i >= iterateCount) words.Add(str.Substring(i, iterateCount));
                else words.Add(str.Substring(i, str.Length - i));

            return words;
        }

       public void ConfirmedDraftReceipt(string Type, string Id)
       {

           using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS))
           {

               ConWare.Open();
               if (Type != "Draft")
               {
                   SqlCommand Select = new SqlCommand(@"SELECT ISNULL(Cast((Select Column19 from Table_011_PwhrsReceipt where columnid=" + Id + ") as nvarchar(10)),'No')", ConWare);
                   string Result = Select.ExecuteScalar().ToString();
                   if (Result == "No")
                       throw new Exception("چنین رسیدی وجود ندارد");
                   else if (Result == "1")
                       throw new Exception("این رسید قطعی شده است");
               }
               else
               {
                   SqlCommand Select = new SqlCommand(@"SELECT ISNULL(Cast((Select Column26 from Table_007_PwhrsDraft where columnid=" + Id + ") as nvarchar(10)),'No')", ConWare);
                   string Result = Select.ExecuteScalar().ToString();
                   if (Result == "No")
                       throw new Exception("چنین حواله ای وجود ندارد");
                   else if (Result == "1")
                       throw new Exception("این حواله قطعی شده است");
               }
           }
       }
       public DataTable LastGoodRemain(string Ware, string Date, string GoodId)
       {
           string Command = @"SELECT GoodCode,
       ISNULL(SUM(IBox) -SUM(OBox), 0) AS Box,
       ISNULL(SUM(TIBox) -SUM(TOBox), 0) AS TBox,
       ISNULL(SUM(IPack) -SUM(OPack), 0) AS Pack,
       ISNULL(SUM(TIPack) -SUM(TOPack), 0) AS TPack,
       ISNULL(SUM(ITotal) -SUM(OTotal), 0) AS Total,
       ISNULL(SUM(ITotalWeight) -SUM(OTotalWeight), 0) AS TotalWeight
FROM   (
           SELECT dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode,
                  0.0 AS IBox,
                  0.0 AS TIBox,
                  0.0 AS IPack,
                  0.0 AS TIPack,
                  0.0 AS ITotalWeight,
                  0.0 AS ITotal,
                  SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS OBox,
                  CAST(
                      ROUND(
                          (
                              SUM(dbo.Table_008_Child_PwhrsDraft.column07) / 
                              NULLIF(
                                  (
                                      SELECT tcai.column09
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                  ),
                                  0
                              )
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TOBox,
                  SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS OPack,
                  CAST(
                      ROUND(
                          (
                              SUM(dbo.Table_008_Child_PwhrsDraft.column07) / 
                              NULLIF(
                                  (
                                      SELECT tcai.column08
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                                  ),
                                  0
                              )
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TOPack,
                  SUM(dbo.Table_008_Child_PwhrsDraft.column07) * ISNULL(
                      (
                          SELECT tcai.column22
                          FROM   table_004_CommodityAndIngredients tcai
                          WHERE  tcai.columnid = dbo.Table_008_Child_PwhrsDraft.column02
                      ),
                      0
                  ) AS OTotalWeight,
                  SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OTotal

                  
           FROM   dbo.Table_007_PwhrsDraft
                  INNER JOIN dbo.Table_008_Child_PwhrsDraft
                       ON  dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
           WHERE  (dbo.Table_007_PwhrsDraft.column02 < '" + Date + @"')
                  AND (dbo.Table_008_Child_PwhrsDraft.column02 = " + GoodId + @")
                      " + (Ware != "0" ?
                     "AND  (dbo.Table_007_PwhrsDraft.column03 = " + Ware + @")"
                     : ""
      )
      + @"
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02
                       union all
                        SELECT dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode,
                  SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS IBox,
                  CAST(
                      ROUND(
                          (
                              SUM(dbo.Table_012_Child_PwhrsReceipt.column07) / 
                              NULLIF(
                                  (
                                      SELECT tcai.column09
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                  ),
                                  0
                              )
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TIBox,
                  SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS IPack,
                  CAST(
                      ROUND(
                          (
                              SUM(dbo.Table_012_Child_PwhrsReceipt.column07) / 
                              NULLIF(
                                  (
                                      SELECT tcai.column08
                                      FROM   table_004_CommodityAndIngredients 
                                             tcai
                                      WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                                  ),
                                  0
                              )
                          ),
                          3
                      )AS DECIMAL(36, 3)
                  ) AS TIPack,
                  SUM(dbo.Table_012_Child_PwhrsReceipt.column07) * ISNULL(
                      (
                          SELECT tcai.column22
                          FROM   table_004_CommodityAndIngredients tcai
                          WHERE  tcai.columnid = dbo.Table_012_Child_PwhrsReceipt.column02
                      ),
                      0
                  ) AS ITotalWeight,
                  SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS ITotal,
                  0.0 AS OBox,
                  0.0 AS TOBox,
                  0.0 AS OPack,
                  0.0 AS TOPack,
                  0.0 AS OTotalWeight,
                  0.0 AS OTotal
                                          
                       FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                                             dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE     (dbo.Table_011_PwhrsReceipt.column02 < '" +
      Date + @"') " + (
          Ware != "0" ? "AND  (dbo.Table_011_PwhrsReceipt.column03 = " + Ware +
          @")" : ""
      ) + @"
                       and (dbo.Table_012_Child_PwhrsReceipt.column02=" + GoodId
      + @")
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02) as Tbl       group by GoodCode";

           SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
           SqlDataAdapter Adapter = new SqlDataAdapter(Command, ConWare);
           DataTable Table = new DataTable();
           Adapter.Fill(Table);
           return Table;
       }
       public DataTable GoodRemain(string GoodId, string Date)
       {
           string Commandtxt = @"Select GoodID,Ware,Table_001_PWHRS.column02 as WareName, SUM(Resid)-SUM(Draft) as Remain from(
            SELECT     Table_008_Child_PwhrsDraft.column02 AS GoodID, Table_007_PwhrsDraft.column03 AS Ware,0.000 as Resid,
             SUM(Table_008_Child_PwhrsDraft.column07) AS Draft
                  
            FROM         Table_008_Child_PwhrsDraft INNER JOIN
                                  Table_007_PwhrsDraft ON Table_007_PwhrsDraft.columnid = Table_008_Child_PwhrsDraft.column01 
            WHERE     (Table_007_PwhrsDraft.column02 <= '{0}')
            GROUP BY Table_008_Child_PwhrsDraft.column02, Table_007_PwhrsDraft.column03
            having Table_008_Child_PwhrsDraft.column02={1}
            union all

            SELECT     Table_012_Child_PwhrsReceipt.column02 AS GoodID, Table_011_PwhrsReceipt.column03 AS Ware, SUM(Table_012_Child_PwhrsReceipt.column07) AS Resid, 0.000 as Draft
            FROM       Table_012_Child_PwhrsReceipt
                              INNER JOIN
                                  Table_011_PwhrsReceipt ON Table_011_PwhrsReceipt.columnid = Table_012_Child_PwhrsReceipt.column01
            WHERE     (Table_011_PwhrsReceipt.column02 <= '{0}')
            GROUP BY Table_012_Child_PwhrsReceipt.column02, Table_011_PwhrsReceipt.column03
            having Table_012_Child_PwhrsReceipt.column02={1}

            ) as tbl
            inner join Table_001_PWHRS on Table_001_PWHRS.columnid=tbl.Ware
            group by GoodID,Ware,Table_001_PWHRS.Column02
            ";
           Commandtxt = string.Format(Commandtxt, Date, GoodId);
           return ReturnTable(ConWare, Commandtxt);
           
       }

       public int GetTypeCheckStatusShablon(string Type, SqlConnection ConBank)
       {
           int Res = Convert.ToInt32(ExScalar(ConBank.ConnectionString, @"SELECT     Column15
FROM         dbo.Table_060_ChequeStatus
WHERE     (ColumnId  = " + Type + @")"));

           return Res;
       }

       public DataTable FillUnitCountByKala(Int32 GoodID)
       {
           SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
           string Commandtxt = @"SELECT    t.kalaID,
                                            tcui.Column00 AS countiD,
                                            ISNULL(t.buy, 0) AS buy,
                                            ISNULL(t.sale, 0) AS sale,
                                            ISNULL(t.customer, 0) AS customer,
                                            ISNULL(t.zarib, 0) AS zarib,
                                            tcui.Column01 AS countName
                                    FROM   " + ConBase.Database + @".dbo.Table_070_CountUnitInfo tcui
                                            LEFT JOIN (
                                                    SELECT tc.[Column00] AS kalaID,
                                                            tc.[Column01] AS countiD,
                                                            tc.[Column02] AS buy,
                                                            tc.[Column03] AS sale,
                                                            tc.[Column04] AS customer,
                                                            tc.[Column05] AS zarib,
                                                            tcui.Column01 AS countName
                                                    FROM   [dbo].[Table_031_CountUnit] tc
                                                            JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo tcui
                                                                ON  tcui.Column00 = tc.Column01
                                                    WHERE  tc.[Column00] = " + GoodID + @" AND tcui.Column00 NOT IN (SELECT column07 FROM  table_004_CommodityAndIngredients WHERE columnid=" + GoodID + @")
                                                    UNION  all                                                                             
                                                    SELECT tcai.columnid AS kalaID,
                                                            tcai.column07 AS countiD,
                                                            CASE 
                                                                WHEN TS003.Column03 IS NULL THEN tcai.Column35
                                                                ELSE TS003.Column03
                                                            END AS buy,
                                                            CASE 
                                                                WHEN TS003.Column07 IS NULL THEN tcai.Column34
                                                                ELSE TS003.Column07
                                                            END AS sale,
                                                            tcai.Column36 AS customer,
                                                            1 AS zarib,
                                                            tcui.Column01 AS countName
                                                    FROM   table_004_CommodityAndIngredients tcai
                                                            JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo tcui
                                                                ON  tcui.Column00 = tcai.column07
                                                            LEFT OUTER JOIN (
                                                                    SELECT columnid,
                                                                            column01,
                                                                            column02,
                                                                            column03,
                                                                            column04,
                                                                            column05,
                                                                            column06,
                                                                            column07,
                                                                            column08,
                                                                            column09,
                                                                            column10,
                                                                            Column11
                                                                    FROM   dbo.Table_003_InformationProductCash
                                                                ) AS TS003
                                                                ON  tcai.columnid = TS003.column01
                                                    WHERE  tcai.columnid = " + GoodID + @"
                                                ) AS t
                                                ON  t.countiD = tcui.Column00 ORDER BY t.kalaID Desc";

           return ReturnTable(ConWare, Commandtxt);

       }
       public float GetZarib(int GoodID, Int16 FromCountUnit, Int16 ToCountUnit)
       {
           float zarib = 1;
           try
           {
               SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);

               using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS))
               {
                   ConWare.Open();

                   SqlCommand command = ConWare.CreateCommand();
                   SqlTransaction transaction;

                   // Start a local transaction.
                   transaction = ConWare.BeginTransaction(
                       IsolationLevel.ReadCommitted, "SampleTransaction");

                   // Must assign both transaction object and connection
                   // to Command object for a pending local transaction.
                   command.Connection = ConWare;
                   command.Transaction = transaction;
                   try
                   {
                       command.CommandText = @"
                                                                DECLARE @from SMALLINT = " + FromCountUnit + @"
                                                                DECLARE @to SMALLINT = " + ToCountUnit + @"
                                                                DECLARE @fromzarib   FLOAT = 0,
                                                                        @fromVzarib  FLOAT = 0,
                                                                        @tozarib     FLOAT = 0,
                                                                        @toVzarib    FLOAT = 0

                                                                IF OBJECT_ID('tempdb.dbo.#Temp') IS NOT NULL
                                                                    DROP TABLE #Temp
		
                                                                SELECT *
                                                                       INTO #Temp
                                                                FROM   (
                                                                           SELECT tc.[Column00] AS kalaID,
                                                                                  tc.[Column01] AS countiD,
                                                                                  tc.[Column02] AS buy,
                                                                                  tc.[Column03] AS sale,
                                                                                  tc.[Column04] AS customer,
                                                                                  tc.[Column05] AS zarib,
                                                                                  (1 / NULLIF(CAST(tc.[Column05] AS FLOAT), 0)) AS vzarib,
                                                                                  tcui.Column01 AS countName
                                                                           FROM   [dbo].[Table_031_CountUnit] tc
                                                                                  JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo 
                                                                                       tcui
                                                                                       ON  tcui.Column00 = tc.Column01
                                                                           WHERE  tc.[Column00] = " + GoodID + @"
                                                                           UNION ALL                                                                             
                                                                           SELECT tcai.columnid AS kalaID,
                                                                                  tcai.column07 AS countiD,
                                                                                  CASE 
                                                                                       WHEN TS003.Column03 IS NULL THEN tcai.Column35
                                                                                       ELSE TS003.Column03
                                                                                  END AS buy,
                                                                                  CASE 
                                                                                       WHEN TS003.Column07 IS NULL THEN tcai.Column34
                                                                                       ELSE TS003.Column07
                                                                                  END AS sale,
                                                                                  tcai.Column36 AS customer,
                                                                                  1 AS zarib,
                                                                                  1 AS vzarib,
                                                                                  tcui.Column01 AS countName
                                                                           FROM   table_004_CommodityAndIngredients tcai
                                                                                  JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo 
                                                                                       tcui
                                                                                       ON  tcui.Column00 = tcai.column07
                                                                                  LEFT OUTER JOIN (
                                                                                           SELECT columnid,
                                                                                                  column01,
                                                                                                  column02,
                                                                                                  column03,
                                                                                                  column04,
                                                                                                  column05,
                                                                                                  column06,
                                                                                                  column07,
                                                                                                  column08,
                                                                                                  column09,
                                                                                                  column10,
                                                                                                  Column11
                                                                                           FROM   dbo.Table_003_InformationProductCash
                                                                                       ) AS TS003
                                                                                       ON  tcai.columnid = TS003.column01
                                                                           WHERE  tcai.columnid = " + GoodID + @"
                                                                       ) AS zaribtable

                                                                SET @fromzarib = (
                                                                        SELECT zarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @from
                                                                    )

                                                                SET @fromVzarib = (
                                                                        SELECT vzarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @from
                                                                    )

                                                                SET @tozarib = (
                                                                        SELECT zarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @to
                                                                    )

                                                                SET @toVzarib = (
                                                                        SELECT vzarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @to
                                                                    )

                                                                IF @fromzarib IS NULL
                                                                   OR @fromVzarib IS NULL
                                                                   OR @tozarib IS NULL
                                                                   OR @toVzarib IS NULL
                                                                    SELECT 1

                                                                IF @fromzarib = 1
                                                                    SELECT ISNULL(@tozarib, 1)

                                                                IF @fromzarib != 1
                                                                   AND @tozarib = 1
                                                                    SELECT ISNULL(@fromVzarib, 1)

                                                                IF @fromzarib != 1
                                                                   AND @tozarib != 1
                                                                    SELECT ISNULL(CAST(@fromVzarib * @tozarib AS FLOAT), 1)

                                                                --SELECT *
                                                                --FROM   #Temp

                                                                IF OBJECT_ID('tempdb.dbo.#Temp') IS NOT NULL
                                                                    DROP TABLE #Temp
		                                                                ";
                       zarib = float.Parse(command.ExecuteScalar().ToString());
                   }
                   catch (Exception e)
                   {
                       try
                       {
                           transaction.Rollback();
                       }
                       catch (SqlException ex)
                       {
                           if (transaction.Connection != null)
                           {

                           }
                       }


                   }
               }
           }
           catch
           {
           }
           return zarib;
       }
    }
}
