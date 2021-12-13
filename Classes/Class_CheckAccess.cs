using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PCLOR.Classes
{
    class Class_CheckAccess
    {
        Class_Documents clDoc = new Class_Documents();
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);

        /// <summary>
        /// بررسی مدیر بودن یک کاربر
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="OrgCode"></param>
        /// <param name="FinYear"></param>
        /// <returns></returns>
        public bool IsAdmin(string UserName, string OrgCode, string FinYear)
        {
            bool Result = false;
            DataTable Table = clDoc.ReturnTable(ConMain, @"Select Column00,Column02 from Table_010_UserInfo where Column00='" + UserName + @"' and Column05=" + OrgCode + " and Column06='" + FinYear + "'");
            if (Table.Rows.Count == 0)
            {
                Class_BasicOperation.ShowMsg("", "اطلاعات کاربری صحیح نمی باشد", Class_BasicOperation.MessageType.Warning);
                Result = false;
            }
            else if (Convert.ToBoolean(Table.Rows[0]["Column02"].ToString()))
            {
                Result = true;
            }
            return Result;

        }

        /// <summary>
        /// کنترل تکی دسترسی کاربر به یک حساب/شخص/مرکز و پروژه
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Code"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool Control(string UserName, string Code, Int16 Type)
        {
            DataTable Table = clDoc.ReturnTable(ConAcnt, "Select * from Table_200_UserAccessInfo where Column01='" + UserName + "' , Column02='" + Code + "' , Column03=" + Type);
            if (Table.Rows.Count > 0)
                return false;
            else return true;
        }


        /// <summary>
        /// برگرداندن حسابها/اشخاص/مراکز/پروژه هایی که کاربر دسترسی ندارد
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Type">2: Person, 3:Center, 4: Project</param>
        /// <returns></returns>
        public List<string> Rows(string UserName, Int16 Type)
        {
            DataTable Table = clDoc.ReturnTable(ConAcnt, "Select Column02 from Table_200_UserAccessInfo where Column01='" + UserName + "' and Column03=" + Type);
            List<string> rows = Enumerable.Select(
            Table.AsEnumerable(), vendor => vendor["Column02"].ToString()).ToList();
            if (rows.Count == 0)
                rows.Add("0");
            return rows;
        }

        /// <summary>
        /// برگرداندن کل/معین/تفصیلی/جز حسابهایی که کاربر دسترسی ندارد
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Type"></param>
        /// <param name="StartPos"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public List<string> Roows(string UserName)
        {
            DataTable Table = clDoc.ReturnTable(ConAcnt, "Select Column02 from Table_200_UserAccessInfo where Column01='" + UserName + "' and Column03=1");
            List<string> rows = Enumerable.Select(
            Table.AsEnumerable(), vendor => "'" + vendor["Column02"].ToString() + "'").ToList();
            if (rows.Count == 0)
                rows.Add("'-'");
            return rows;
        }


    }
}
