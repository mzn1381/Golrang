using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace PCLOR.Classes
{
    class DeleteRelatedInfo
    {
        public void KolInSanad( string _KolCode)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column03) from Table_065_SanadDetail where Column03=N'" + _KolCode + "'", ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
            }
             
        }
        public void MoeinInSanad(string _MoeinCode,string _KolCode)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column04) from Table_065_SanadDetail where Column04=N'" + _MoeinCode +
                    "' and Column03='" + _KolCode + "'", ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
            }
        }
        public void TafsiliInSanad( string _TafsiliCode,string _MoeinCode,string _KolCode)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column05) from Table_065_SanadDetail where Column05=N'" + _TafsiliCode +
                    "' and Column04=N'" + _MoeinCode + "' and Column03=N'" + _KolCode + "'", ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
            }
           
        }
        public void JozInSanad(string _JozCode,string _TafsiliCode,string _MoeinCode,string _KolCode)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column06) from Table_065_SanadDetail where Column06=N'" + _JozCode + "' and "
                    + " Column05=N'" + _TafsiliCode + "' and Column04=N'" + _MoeinCode + "' and Column03=N'" + _KolCode + "'", ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
            }
          
        }
        public void PersonInSanad( int _PersonCode,string dbName)
        {
            SqlConnection con=new SqlConnection (Properties.Settings.Default.PACNT);
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT.Replace(con.Database,dbName)))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column07) from Table_065_SanadDetail where Column07=" + _PersonCode, ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این شخص در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد"+" در سال مالی "+dbName.Substring(dbName.Length-4,4));
            }
           

        }
        public void CenterInSanad(Int16 _CenterCode, string dbName)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.PACNT);
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT.Replace(con.Database, dbName)))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column08) from Table_065_SanadDetail where Column08=" + _CenterCode, ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این مرکز هزینه در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد" + " در سال مالی " + dbName.Substring(dbName.Length - 4, 4));
            }
         
        }
        public void ProjInSanad(Int16 _ProjectCode, string dbName)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.PACNT);
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT.Replace(con.Database, dbName)))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column09) from Table_065_SanadDetail where Column09=" + _ProjectCode, ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این پروژه در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد" + " در سال مالی " + dbName.Substring(dbName.Length - 4, 4));
            }
           
        }

        public void CurrencyInSanad( Int16 _CurrencyCode)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Count(Column15) from Table_065_SanadDetail where Column15=" + _CurrencyCode, ConAcnt);
                if (Select.ExecuteScalar().ToString() != "0")
                    throw new Exception("به علت استفاده از این ارز در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
            }

        }

    }
}
