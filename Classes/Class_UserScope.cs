using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR.Classes
{
    class Class_UserScope
    {
        SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE); 
        bool _IsAdmin = false;
        bool _PerOrg = false;

        public bool isValid(string User, string Pass)
        {
            if (Con.State != System.Data.ConnectionState.Open)
                Con.Open();
            DataTable UserTable = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_010_UserInfo where Column00='" + User + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._FinYear + "'"
            , Con);
            Adapter.Fill(UserTable);
            Con.Close();
            if (UserTable.Rows.Count == 0)
            {
                throw new Exception("نام کاربری وارد شده معتبر نمی باشد");
            }


            if (UserTable.Rows[0]["Column01"].ToString() != Pass)
            {
                throw new Exception("رمز عبور وارد شده نامعتبر است");
            }

            if (!Convert.ToBoolean(UserTable.Rows[0]["Column03"].ToString()))
                throw new Exception("این کاربر غیرفعال می باشد");


            //else if (UserTable.Rows.Count == 1)
            //{
            //    if (UserTable.Rows[0]["Column33"].ToString() == "True" && UserTable.Rows[0]["Column34"].ToString() == "True")
            //    {
            //        _All = true;
            //        _PerOrg = false;
            //    }
            //    else
            //    {
            //        _PerOrg = true;
            //        _All = false;
            //    }
            //}
            return true;
        }
        public bool isAdmin(string UserName)
        {
            if (Con.State != ConnectionState.Open)
                Con.Open();
            SqlCommand Command = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._FinYear + "'"
                , Con);
            _IsAdmin = bool.Parse(Command.ExecuteScalar().ToString());
            Command.CommandText = Command.CommandText.Replace("Column02", "Column34");
            if (_IsAdmin)
                return true;
            else return false;
        }
        public int ReturnBranch(string User)
        {
            if (ConBase.State != ConnectionState.Open)
                ConBase.Open();
            SqlCommand Command = new SqlCommand("select isnull((SELECT top(1) Column133 FROM table_045_personinfo AS table_045_personinfo_1 WHERE  Column23='" + User + "' ),0) as Res"
                , ConBase);
            int Branch = int.Parse(Command.ExecuteScalar().ToString());
            return Branch;
        }
        public bool CheckScope(string UserName, string ColumnName, int Position)
        {

            if (Con.State != ConnectionState.Open)
                Con.Open();
            SqlCommand Command = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._FinYear + "'"
                , Con);
            _IsAdmin = bool.Parse(Command.ExecuteScalar().ToString());
            Command.CommandText = Command.CommandText.Replace("Column02", "Column34");
            _PerOrg = bool.Parse(Command.ExecuteScalar().ToString());

            if (_IsAdmin)
                return true;
            else
            {

                SqlCommand Select = new SqlCommand("select substring(" + ColumnName + "," + Position + ",1) from  Table_010_UserInfo where Column00='" + UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._FinYear + "'", Con);
                if (Select.ExecuteScalar().ToString() == "1")
                    return true;
                else
                    return false;
            }
        }


    }
}
