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
    class CheckCredits
    {
        private string PersonName(int PersonCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PBASE))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select Column02 from Table_045_PersonInfo where ColumnId=" + PersonCode, Con);
                string _PersonName = Select.ExecuteScalar().ToString();
                return _PersonName;
            }
        }

        private string AccountName(string AccountCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("SElect ISNULL(( Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'),'Not Exist')", Con);
                string _AccountName = Select.ExecuteScalar().ToString();
                if (_AccountName == "Not Exist")
                    throw new Exception("کد حساب " + AccountCode + " نامعتبر می باشد");
                else
                    return _AccountName;
            }
        }

        public void CheckPersonCredit(DataTable Table, int _SanadNumber)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Com = new SqlCommand("", Con);
                DataTable AggTable = Table.Clone();
                AggTable = Table.DefaultView.ToTable();

                //Group Rows by LINQ//
                var query = from p in AggTable.AsEnumerable()
                            group p by new { Account = p.Field<string>("Account"), Person = p.Field<int>("Person") } into groupedTable
                            select new
                            {
                                Person = groupedTable.Key.Person,
                                Account = groupedTable.Key.Account,
                                sumOfValue = groupedTable.Sum(p => p.Field<Double>("Price"))

                            };

                DataTable T = AggTable.Clone();
                foreach (var x in query)
                {
                    T.Rows.Add(x.Person, x.Account, x.sumOfValue);
                }

                foreach (DataRow item in T.Rows)
                {

                    Com.CommandText = "Select Column01,Column08,Column09 from Table_050_CerditInfo where "
                        + "Column00=" + item["Person"].ToString() + " and Column02='" + item["Account"].ToString() + "'";
                    using (SqlDataReader Reader = Com.ExecuteReader())
                    {
                        Reader.Read();
                        if (Reader.HasRows)
                        {
                            // Catch Suitable Action//
                            bool CreditKind = bool.Parse(Reader["Column01"].ToString());
                            Double CreditPrice = Convert.ToDouble(Reader["Column08"].ToString());
                            Int16 Action = Int16.Parse(Reader["Column09"].ToString());

                            Reader.Close();
                            //Catch  Remain for this person in this Account///
                            Com.CommandText = "Select ISNULL(Sum(Column11)-Sum(Column12),0) from Table_065_SanadDetail where Column01='" + item["Account"].ToString() + "'" +
                                " And Column07=" + item["Person"].ToString() + " And Column00<>" + _SanadNumber;
                            Double Remain = Convert.ToDouble(Com.ExecuteScalar().ToString());
                            Double Price = Convert.ToDouble(item["Price"].ToString());

                            if (CreditKind && (Remain + Price) < 0)
                            {
                                //if ((Remain + Price) > 0)
                                //{
                                //    switch (Action)
                                //    {
                                //        case 1:
                                //            MessageBox.Show("عدم هماهنگی نوع اعتبار معرفی شده و مانده اعتبار شخص " + Environment.NewLine +
                                //                PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                //                AccountName(item["Account"].ToString()));
                                //            break;
                                //        case 2:
                                //            throw new Exception("عدم هماهنگی نوع اعتبار معرفی شده و مانده اعتبار شخص " + Environment.NewLine +
                                //               PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                //               AccountName(item["Account"].ToString()) + Environment.NewLine + "جهت ادامه عملیات اصلاح مبالغ الزامیست");
                                //    }
                                //}
                                if (CreditPrice < Math.Abs((Remain + Price)))
                                {
                                    switch (Action)
                                    {
                                        case 1:
                                            MessageBox.Show("مانده اعتبار " +
                                                PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                                AccountName(item["Account"].ToString()) + " بیش از میزان اعتبار تعیین شده است ");
                                            break;
                                        case 2:
                                            throw new Exception(" مانده اعتبار " +
                                               PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                               AccountName(item["Account"].ToString()) + " بیش از میزان اعتبار تعیین شده است " + Environment.NewLine + "جهت ادامه عملیات اصلاح مبالغ الزامیست");
                                    }
                                }
                            }
                            else if (!CreditKind && (Remain + Price) > 0)
                            {
                                if ((Remain + Price) > CreditPrice)
                                {
                                    switch (Action)
                                    {
                                        case 1:
                                            MessageBox.Show("مانده اعتبار " +
                                                PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                                AccountName(item["Account"].ToString()) + " بیش از میزان اعتبار تعیین شده است ");
                                            break;
                                        case 2:
                                            throw new Exception(" مانده اعتبار " +
                                               PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                               AccountName(item["Account"].ToString()) + " بیش از میزان اعتبار تعیین شده است " + Environment.NewLine + "جهت ادامه عملیات اصلاح مبالغ الزامیست");
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        public void CheckAccountCredit(DataTable Table, int _SanadNumber)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Com = new SqlCommand("", Con);
                DataTable AggTable = Table.Clone();
                AggTable = Table.DefaultView.ToTable();

                //Group Rows by LINQ//
                var query = from p in AggTable.AsEnumerable()
                            group p by new { Account = p.Field<string>("Account") } into groupedTable
                            select new
                            {
                                Account = groupedTable.Key.Account,
                                sumOfValue = groupedTable.Sum(p => p.Field<Double>("Price"))

                            };

                DataTable T = AggTable.Clone();
                foreach (var x in query)
                {
                    T.Rows.Add(x.Account, x.sumOfValue);
                }

                foreach (DataRow item in T.Rows)
                {

                    Com.CommandText = "Select Control_Type,Control_Action from AllHeaders() where "
                        + " ACC_Code='" + item["Account"].ToString() + "'";
                    using (SqlDataReader Reader = Com.ExecuteReader())
                    {
                        Reader.Read();
                        if (Reader.HasRows)
                        {
                            // Catch Suitable Action//
                            Int16 Control_Type = Int16.Parse(Reader["Control_Type"].ToString());
                            Int16 Control_Action = Int16.Parse(Reader["Control_Action"].ToString());

                            Reader.Close();
                            //Catch Before Ramin for this person in this Account///
                            Com.CommandText = "Select ISNULL(Sum(Column11)-Sum(Column12),0) from Table_065_SanadDetail where Column01='" + item["Account"].ToString() +
                                "'  And Column00<>" + _SanadNumber;
                            Double Remain = Convert.ToDouble(Com.ExecuteScalar().ToString());
                            Double Price = Convert.ToDouble(item["Price"].ToString());



                            switch (Control_Type)
                            {
                                ///////فقط بدهکار
                                case 1:
                                    {
                                        if ((Remain + Price) < 0)
                                        {
                                            switch (Control_Action)
                                            {
                                                case 1:
                                                    {
                                                        MessageBox.Show(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بدهکار مي باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        throw new Exception(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بدهکار مي باشد و با توجه نوع اقدام حساب،تا اصلاح حساب قادر به ذخيره کردن نمي باشيد");
                                                    }
                                            }
                                        }
                                        break;

                                    }
                                ///////////فقط بستانکار
                                case 2:
                                    {
                                        if ((Remain + Price) > 0)
                                        {

                                            switch (Control_Action)
                                            {
                                                case 1:
                                                    {
                                                        MessageBox.Show(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بستانکار مي باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        throw new Exception(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بستانکار مي باشد و با توجه نوع اقدام حساب، تا اصلاح حساب قادر به ذخيره کردن نمي باشيد");
                                                    }
                                            }
                                        }
                                        break;

                                    }

                            }
                        }
                        else
                        {
                            throw new Exception("کد حساب " + item["Account"].ToString() + "نامعتبر است");
                        }
                    }


                }
            }
        }


        public void All_Controls(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            ////*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
            }

        }

        public void All_Controls_2(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            string Message = "";
            ////*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                    Message = "** انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست";
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                    Message = "** انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد";
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                    Message += "** انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست";
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                    Message += "** انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد";
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                    Message += "** انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست";
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                    Message += "** انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد";
            }
            if (Message.Trim() != "")
                throw new Exception(Message);
        }

        public void All_Controls_Row(string AccountCode, int? Person, Int16? Center, Int16? Project, Janus.Windows.GridEX.GridEXRow Row)
        {
            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {
                    // Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
                    Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {
                    Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
                    Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {
                    Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
                    Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {
                    Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
                    Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {
                    Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
                    Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {
                    Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
                    Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }




        }


        /// <summary>
        /// بررسی الزام شخص برای یک حساب
        /// </summary>
        /// <param name="ACC"></param>
        /// <returns></returns>
        internal Int16 AccHasPerson(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select (Select Control_Person from AllHeaders() where ACC_Code='" + ACC + "')", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }

        /// <summary>
        /// بررسی الزام مرکز هزینه برای یک حساب
        /// </summary>
        /// <param name="ACC"></param>
        /// <returns></returns>
        internal Int16 AccHasCenter(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Center from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }

        /// <summary>
        /// بررسی الزام پروژه برای یک حساب
        /// </summary>
        /// <param name="ACC"></param>
        /// <returns></returns>
        internal Int16 AccHasProject(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Project from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
    }



}

