using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using Janus.Windows.GridEX;
using System.IO;
using System.Net;
using System.Drawing;
namespace PCLOR
{
    public static class Class_BasicOperation
    {
        public static Int16 _OrgCode;
        public static bool _WareType;
        public static string _FinYear;
        public static bool _FinType;
        public static string _UserName;
        public static int _Branch;
        public enum FilterColumnType { GoodCode, Others, ACCColumn };
        public enum MessageType { Warning, Information, Stop, None };
        //کنترل وارد شدن کاراکتر غیر عددی
        [Description("Control user input for non digits")]
        public static bool isNotDigit(char C)
        {
            if (!char.IsControl(C) && !char.IsDigit(C))
                return true;
            return false;
        }

        //انتقال کرسر به کنترل بعدی
        public static void isEnter(char C)
        {
            if (C == 13)
                SendKeys.Send("{TAB}");
        }
        //تغییر زبان صفحه کلید
        public static void ChangeLanguage(string Culture)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo(Culture));
        }

        public static void ShowMsg(string Title, string Text, MessageType Type)
        {

            if (Type == MessageType.Warning)
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == MessageType.Information)
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == MessageType.Stop)
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == MessageType.None)
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        }
        public static void ShowMsg(string Title, string Text, string Type)
        {
            if (Type == "Warning")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == "Information")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == "Stop")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == "None")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        }
        public static void CheckExceptionType(Exception ex, string FormName)
        {
            System.Data.NoNullAllowedException Notnull = new System.Data.NoNullAllowedException();
            System.Data.ConstraintException Unique = new System.Data.ConstraintException();
            System.OverflowException Overflow = new OverflowException();
            System.Data.InvalidConstraintException Relation = new System.Data.InvalidConstraintException();
            InvalidOperationException invalid = new InvalidOperationException();
            if (ex.GetBaseException().GetType() == Notnull.GetBaseException().GetType())
            {
                MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                return;
            }
            else if (ex.GetBaseException().GetType() == Unique.GetBaseException().GetType())
            {
                
                {
                    MessageBox.Show("کد/اطلاعات وارد شده تکراری می باشد");
                    return;
                }
            }
            else if (ex.GetBaseException().GetType() == Overflow.GetBaseException().GetType())
            {
                MessageBox.Show("طول داده وارده بیش از حد مجاز است");
                return;
            }
            else if (ex.GetBaseException().GetType() == Relation.GetBaseException().GetType())
            {
                MessageBox.Show("حذف اطلاعات امکانپذیر نمی باشد");
                return;
            }
            else if (ex.GetBaseException().GetType() == invalid.GetBaseException().GetType())
            {
                if (ex.Message.StartsWith("Item cannot be added to a read-only or fixed-size list"))
                {
                    if (FormName == "Form06_AccountingHeaders")
                    {
                        MessageBox.Show("سرفصل حساب در سطح بالاتر مشخص نگردیده است");
                        return;
                    }
                   
                }
            }
            else
            {

                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    if (FormName == "Form06_AccountingHeaders")
                    {
                        MessageBox.Show("حذف سرفصل به دلیل استفاده در سایر قسمتها و یا داشتن زیر مجموعه امکانپذیر نمی باشد");
                        return;
                    }
                    if (FormName == "Form11_GroupPeople")
                    {
                        MessageBox.Show("حذف این گروه به علت استفاده در سایر قسمت ها امکانپذیر نمی باشد");
                        return;
                    }
                    else if (FormName == "Form04_Cheques")
                    {
                        MessageBox.Show("حذف این دسته چک به علت استفاده، امکان پذیر نمی باشد");
                        return;
                    }
                    else if (FormName == "Form08_DefineChequeStatus")
                    {
                        MessageBox.Show("حذف این وضعیت به دلیل استفاده امکانپذیر نمی باشد");
                        return;
                    }
                    else if (FormName == "Frm_40_PersonInfo")
                    {
                        MessageBox.Show("حذف این شخص به دلیل استفاده امکانپذیر نمی باشد");
                        return;
                    }
                    else if (FormName == "Form19_FinanceRatioSetting")
                    {
                        MessageBox.Show("جهت حذف نسبت تعریف شده، ابتدا صورت و مخرجهای مربوط را حذف نمایید");
                        return;
                    }
                    else if (FormName == "Form01_AccDocument")
                    {
                        if (ex.Message.Contains("Table_075_SanadDetailNotes"))
                        {
                            MessageBox.Show("حذف آرتیکل(ها) به دلیل داشتن یادداشت امکانپذیر نمی باشد");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("حذف اطلاعات امکانپذیر نمی باشد");
                            return;
                        }
                    }
                }
                else if (ex.Message.StartsWith("Index -1 does not have a value") && (FormName == "Form06_PayCash" || FormName == "Form03_ReceiveCash"))
                {
                    return;
                }
                else if (ex.Message.StartsWith("Cannot insert duplicate key row in object"))
                {
                    if (FormName == "Form10_BenefitSetting")
                    {
                        MessageBox.Show("مجموع انتخابی سرفصل و هزینه تکراریست");
                        return;
                    }
                    else if (FormName == "Form04_PayChq")
                    {
                        MessageBox.Show("این شماره چک قبلا صادر شده است");
                        return;
                    }
                    else if (FormName == "Form04_Contracts")
                    {
                        MessageBox.Show("شماره قرارداد وارده شده تکراری است");
                        return;
                    }
                    else if (FormName == "Form19_FinanceRatioSetting")
                    {
                        if (ex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Table_195_FinanceRatio_Makhraj'"))
                        {
                            MessageBox.Show("در میان سرفصلهای انتخاب شده برای مخرج، سرفصل تکراری وجود دارد");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("در میان سرفصلهای انتخاب شده برای صورت، سرفصل تکراری وجود دارد");
                            return;
                        }
                    }
                    else if (FormName == "Form25_CustomerClub")
                    {
                        if (ex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Table_215_CustomerClub' with unique index 'IX_Table_215_CustomerClub_Mobile'"))
                        {
                            MessageBox.Show("تلفن همراه وارد شده تکراریست");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("کد شخص وارد شده تکراریست");
                            return;
                        }
                    }
                }
                else if (ex.Message.StartsWith("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records"))
                {
                    MessageBox.Show("اطلاعات را به روز رسانی کنید");
                    return;
                }
                else if (ex.Message.StartsWith("The INSERT statement conflicted with the CHECK constraint"))
                {
                    if (FormName == "Form05_Groups")
                    {
                        MessageBox.Show("کد گروه بیش از دو رقم است");
                        return;
                    }
                }
                else if (ex.Message.StartsWith("Violation of PRIMARY KEY constraint"))
                {
                    if (FormName == "Form06_AccountingHeaders")
                    {
                        if (ex.Message.StartsWith("Violation of PRIMARY KEY constraint 'PK_Table045_AccKolInfo'. Cannot insert duplicate key in object 'dbo.Table_010_KolInfo'"))
                        {
                            MessageBox.Show("کد کل تکراری است");
                            return;
                        }
                    }

                }

                else
                {
                    MessageBox.Show(ex.Message);
                    return;
                }


            }

        }



        public static bool CalLinearDis(int CustomerId)
        {
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select cast( ISNULL((SELECT Column02 from Table_105_SystemTransactionInfo where Column00=65),0) as bit)", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            return Convert.ToBoolean(Table.Rows[0][0].ToString());
        }


        public static void CheckSqlExp(System.Data.SqlClient.SqlException ex, string FormName)
        {
            if (ex.Message.StartsWith("Violation of UNIQUE KEY constraint"))
            {
                if (FormName == "Form06_AccountingHeaders")
                {
                    MessageBox.Show("کد وارد شده تکراری می باشد");
                    return;
                }
                else if (FormName == "Form06_PayCash")
                {
                    MessageBox.Show("شماره برگه وارد شده تکراری می باشد");
                    return;
                }
                else if (FormName == "Form01_ReceiveChq")
                {
                    MessageBox.Show("پشت نمره وارد شده تکراری می باشد");
                    return;
                }
            }
            else if (ex.Message.StartsWith("The DELETE statement conflicted with"))
            {
                if (FormName == "Form06_AccountingHeaders")
                {
                    MessageBox.Show("حذف سرفصل به دلیل استفاده در سایر قسمتها و یا داشتن زیر مجموعه امکانپذیر نمی باشد");
                    return;
                }
                else
                    MessageBox.Show("حذف اطلاعات امکانپذیر نمی باشد");
                return;
            }
            else if (ex.Message.StartsWith(@"Violation of PRIMARY KEY constraint 'PK_Table_010_UserInfo'"))
            {
                MessageBox.Show("کاربر با این مشخصات قبلا تعریف شده است");
                return;
            }

            else if (ex.Message.StartsWith("Cannot insert duplicate key row in object"))
            {
                if (FormName == "Frm_40_PersonInfo" && ex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Table_045_PersonInfo' with unique index 'IX_Table_045_PersonInfo_1'"))
                {
                    MessageBox.Show("کدملی/اقتصادی وارد شده تکراری می باشد");
                    return;
                }
                else if (FormName == "Frm_40_PersonInfo" && ex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Table_045_PersonInfo' with unique index 'IX_Table_045_PersonInfo'"))
                {
                    MessageBox.Show("کد شخص وارد شده تکراری می باشد");
                    return;
                }
                else if (FormName == "Form16_CloseAccounts")
                {
                    MessageBox.Show("بین سرفصلهای مرتبط سرفصل تکراری وجود دارد");
                    return;
                }
                else if (FormName == "Form01_ReceiveChq")
                {
                    MessageBox.Show("شماره چک وارد شده تکراری می باشد");
                    return;
                }
                else if (FormName == "Form04_PayChq" && ex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Table_030_ExportCheques' with unique index 'IX_Table_030_ExportCheques'"))
                {
                    MessageBox.Show("این شماره چک قبلاً صادر شده است");
                    return;
                }
                else if (FormName == "Form01_ListOfPrices")
                {
                    MessageBox.Show("در میان شماره های وارد شده شماره تکراری وجود دارد");
                    return;
                }
                else if (FormName == "Form05_CarpetReceipt")
                {
                    MessageBox.Show("شماره شناسه فرش تکراری می باشد");
                    return;
                }
                else if (FormName == "Form04_PayChq_Group")
                {
                    if (ex.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Table_030_ExportCheques' with unique index 'IX_Table_030_ExportCheques'."))
                    {
                        MessageBox.Show("چکهای مشخص شده قبلاً پرداخت شده اند");
                        return;
                    }
                }
            }
            else if (ex.Message.StartsWith("The UPDATE statement conflicted with the FOREIGN KEY constraint"))
            {
                if (FormName == "Form04_PayChq")
                {
                    MessageBox.Show("برای این صندوق/بانک چنین شماره چکی معرفی نشده است");
                    return;
                }
            }
            throw new Exception(ex.Message);
        }
        //پیشنهاد شماره سند
        public static int LastIDNumber()
        {
            using (SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                ConPWHRS.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Max(Column00),0)+1 from Table_35_BeforeFactore", ConPWHRS);
                int SanadNo = int.Parse(Command.ExecuteScalar().ToString());
                return SanadNo;
            }
        }
        //نمایش نام کامل حساب انتخاب شده
        public static string HeadersCompleteName(string ACC_Code)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select ACC_NameComplete from AllHeaders() where ACC_Code='" + ACC_Code + "'", ConAcnt);
                return Select.ExecuteScalar().ToString();
            }
        }
        //برگرداندن تاریخ سرور
        public static DateTime ServerDate()
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Serverdate = new SqlCommand("Select getdate()", Con);
                DateTime DateTime = DateTime.Parse(Serverdate.ExecuteScalar().ToString());
                return DateTime;
            }

        }
        //برگرداندن جدول سازمان جهت استفاده در لوگوی گزارشات
        public static DataTable LogoTable()
        {
            SqlConnection ConPMain = new SqlConnection(Properties.Settings.Default.MAIN);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_000_OrgInfo where ColumnId=" +
                _OrgCode, ConPMain);
            DataTable T000 = new DataTable();
            Adapter.Fill(T000);
            return T000;
        }
        //تبدیل عدد به فرمت ریالی
        public static string Rial(Int64 Price)
        {
            string Rial = Price.ToString();
            int i = Price.ToString().Length - 3;
            while (i > 0)
            {
                Rial = Rial.Insert(i, ",");
                i -= 3;
            }
            return Rial;
        }

        public static void FilterMultiColumns(object sender, string NameColumn, string CodeColumn)
        {
            //if (!string.IsNullOrEmpty(CodeColumn) && ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text.All(char.IsDigit))
            //{
            //    Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
            //        DropDownList.RootTable.Columns[CodeColumn], Janus.Windows.GridEX.ConditionOperator.BeginsWith,
            //        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

            //    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
            //        .DropDownList.ApplyFilter(filter);
            //    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.Update();
            //}
            //else
            //{
            //    Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
            //            DropDownList.RootTable.Columns[NameColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
            //            ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

            //    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
            //        .DropDownList.ApplyFilter(filter);
            //}
            if (!string.IsNullOrEmpty(CodeColumn) && ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text.All(char.IsDigit))
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                    DropDownList.RootTable.Columns[CodeColumn], Janus.Windows.GridEX.ConditionOperator.BeginsWith,
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                    .DropDownList.ApplyFilter(filter);
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.Update();
            }
            else
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                        DropDownList.RootTable.Columns[NameColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
                        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                    .DropDownList.ApplyFilter(filter);
            }
        }

        public static void FilterGridExDropDown(object sender, string ColumnName, string NumericalSearch, string TextualText, string SearchText, FilterColumnType ColumnType)
        {
            try
            {
                if (ColumnType == FilterColumnType.GoodCode)
                {
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                         ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[NumericalSearch],
                         ConditionOperator.BeginsWith, SearchText);
                    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                }
                else if (ColumnType == FilterColumnType.ACCColumn)
                {
                    if (SearchText.All(char.IsDigit))
                    {
                        Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                             ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[NumericalSearch],
                             ConditionOperator.BeginsWith, SearchText);
                        ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                    }
                    else
                    {
                        Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                             ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                             ConditionOperator.Contains, SearchText);
                        ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                    }
                }
                else
                {
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                         ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                         ConditionOperator.Contains, SearchText);
                    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                }

            }
            catch { }
        }

        //public static void GridExDropDownRemoveFilter(object sender, string ColumnName)
        //{
        //    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RemoveFilters();

        //}


        public static string ConString()
        {
            return (Properties.Settings.Default.MAIN);
        }

       

        public static void MultiColumnsRemoveFilter(object sender)
        {
            ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.RemoveFilters();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">ا</param>
        /// <param name="ColumnName">نام ستونی که قرار است فیلتر شود</param>
        /// <param name="NumericalSearch">ستون عددی دراپ دان</param>
        /// <param name="TextualText">ستون کاراکتری دراپ دان</param>
        /// <param name="SearchText">متن قابل سرچ</param>
        public static void FilterGridExDropDown(object sender, string ColumnName, string NumericalSearch, string TextualText, string SearchText)
        {
            try
            {
                if (SearchText.All(char.IsDigit))
                {
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                         ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[NumericalSearch],
                         ConditionOperator.BeginsWith, SearchText);
                    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                }
                else
                {
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                         ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                         ConditionOperator.Contains, SearchText);
                    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                }

            }
            catch { }
        }




        //public static void GridExDropDownRemoveFilter(object sender, string ColumnName)
        //{
        //    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RemoveFilters();

        //}

        public static string AddMonths(string dat, int Daays)
        {

            int y = 0, m = 0, d = 0;
            y = int.Parse(dat.Substring(0, 4).ToString());
            m = int.Parse(dat.Substring(5, 2).ToString());
            d = int.Parse(dat.Substring(8, 2).ToString());

            for (int i = 0; i < Daays; i++)
            {
                d++;
                if (d > 30)
                {
                    m++;
                    d = 1;
                }
                if (m > 12)
                {
                    y++;
                    m = 1;
                }
            }

            string tem = y.ToString();
            if (m < 10)
                tem += "/" + "0" + m.ToString() + "/";
            else
                tem += "/" + m.ToString() + "/";
            if (d < 10)
                tem += "0" + d.ToString();
            else
                tem += d.ToString();

            return tem;
        }





        /// <summary>
        ///  Check Internet Connection
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send("www.google.com", 1000);

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }


        private const string initVector = "zmparya9uzpgzl88";
        private const int keysize = 256;
        public static string Decryptdata(string cipherText)
        {
            try
            {
                string passPhrase = "z.@M~2#13@2_20";
                byte[] initVectorBytes = System.Text.Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                System.Security.Cryptography.PasswordDeriveBytes password = new System.Security.Cryptography.PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                System.Security.Cryptography.RijndaelManaged symmetricKey = new System.Security.Cryptography.RijndaelManaged();
                symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;
                System.Security.Cryptography.ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch
            {
                return "-1111111111";
            }
        }

        public static void GridExDropDownRemoveFilter(object sender, string ColumnName)
        {
            ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RemoveFilters();

        }

        public static string ToRial(Int64 Price)
        {
            string Rial = Price.ToString();
            int i = Price.ToString().Length - 3;
            while (i > 0)
            {
                Rial = Rial.Insert(i, ",");
                i -= 3;
            }
            return Rial;
        }

        public static string SqlTransactionMethodScaler(string ConStr, string command, params object[] args)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand cmd = Con.CreateCommand();
                cmd.Transaction = sqlTran;
                try
                {
                    cmd.CommandText = command;
                    for (int i = 0; i < args.Length; )
                    {
                        if (args[i + 1] == null || args[i + 1].ToString() == "")
                            args[i + 1] = DBNull.Value;
                        cmd.Parameters.Add(args[i].ToString(), args[i + 1]);
                        i += 2;
                    }
                    if (command.Contains("@outid"))
                    {
                        SqlParameter sqlp = new SqlParameter();
                        sqlp.Direction = ParameterDirection.Output;
                        sqlp.ParameterName = "@outid";
                        sqlp.SqlDbType = SqlDbType.Int;
                        sqlp.Value = "";
                        cmd.Parameters.Add(sqlp);
                        cmd.ExecuteNonQuery();
                        result = cmd.Parameters["@outid"].Value.ToString();
                    }
                    else
                        cmd.ExecuteNonQuery();
                    sqlTran.Commit();

                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    throw es;
                }

            }
            return result;
        }
        public static DataTable SqlTransactionMethod(string ConStr, string command, params object[] args)
        {
      
            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand cmd = Con.CreateCommand();
               
                cmd.Transaction = sqlTran;
                try
                {
                    cmd.CommandText = command;
                    for (int i = 0; i < args.Length; )
                    {
                        if (args[i + 1] == null || args[i + 1].ToString() == "")
                            args[i + 1] = DBNull.Value;
                        cmd.Parameters.Add(args[i].ToString(), args[i + 1]);
                        i += 2;
                    }
                   SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                    DataTable dt=new DataTable();
                    Adapter.Fill(dt);
                    sqlTran.Commit();
                    return dt;

                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    throw es;
                }

            }
       
        }

        //public static string SqlTransactionMethodScaler(string ConStr, string command, params object[] args)
        //{
        //    string result = "";
        //    using (SqlConnection Con = new SqlConnection(ConStr))
        //    {
        //        Con.Open();

        //        SqlTransaction sqlTran = Con.BeginTransaction();
        //        SqlCommand cmd = Con.CreateCommand();
        //        cmd.Transaction = sqlTran;
        //        try
        //        {
        //            cmd.CommandText = command;
        //            for (int i = 0; i < args.Length; )
        //            {
        //                if (args[i + 1] == null || args[i + 1].ToString() == "")
        //                    args[i + 1] = DBNull.Value;
        //                cmd.Parameters.Add(args[i].ToString(), args[i + 1]);
        //                i += 2;
        //            }
        //            if (command.Contains("@outid"))
        //            {
        //                SqlParameter sqlp = new SqlParameter();
        //                sqlp.Direction = ParameterDirection.Output;
        //                sqlp.ParameterName = "@outid";
        //                sqlp.SqlDbType = SqlDbType.Int;
        //                sqlp.Value = "";
        //                cmd.Parameters.Add(sqlp);
        //                cmd.ExecuteNonQuery();
        //                result = cmd.Parameters["@outid"].Value.ToString();
        //            }
        //            else
        //                cmd.ExecuteNonQuery();
        //            sqlTran.Commit();

        //        }
        //        catch (Exception es)
        //        {
        //            sqlTran.Rollback();
        //            throw es;
        //        }

        //    }
        //    return result;
        //}

       
    
    }


}

