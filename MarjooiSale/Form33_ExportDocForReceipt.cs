using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR.MarjooiSale
{
    public partial class Form33_ExportDocForReceipt : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        DataTable SourceTable = new DataTable();
        string _ResidId = "";
        SqlParameter DraftNum;
        DataTable HeaderTable = new DataTable();
        DataRow ReceiptRow;

        public Form33_ExportDocForReceipt(string ResidId)
        {
            InitializeComponent();
            _ResidId = ResidId;
        }



        private void Form33_ExportDocForReceipt_Load(object sender, EventArgs e)
        {
            try
            {
                faDatePicker1.SelectedDateTime = DateTime.Now;
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
                //ارزش ارز
                SourceTable.Columns.Add("Column13", Type.GetType("System.Double"));
                //نوع ارز
                SourceTable.Columns.Add("Column14", Type.GetType("System.Int16"));
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                //DropDowns
                DataTable HeaderTable = clDoc.ReturnTable(ConAcnt, "Select ACC_Code,ACC_Name from AllHeaders()");
                gridEX1.DropDowns["Header_Name"].DataSource = HeaderTable;
                gridEX1.DropDowns["Header_Code"].DataSource = HeaderTable;

                gridEX1.DropDowns["Project"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");
                gridEX1.DropDowns["Center"].DataSource = clDoc.ReturnTable(ConBase, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo");
                //gridEX1.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase, "Select Columnid,Column01,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
                gridEX1.DataSource = SourceTable;

                HeaderTable = clDoc.ReturnTable(ConWare, "Select * from Table_011_PwhrsReceipt where Columnid in("+ _ResidId.TrimEnd(',')+")");
                ReceiptRow = HeaderTable.Rows[0];

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 145))
                {
                    gridEX1.RootTable.Columns["Column11"].EditType = Janus.Windows.GridEX.EditType.TextBox;
                    gridEX1.RootTable.Columns["Column12"].EditType = Janus.Windows.GridEX.EditType.TextBox;

                }
                else
                {
                    gridEX1.RootTable.Columns["Column11"].EditType = Janus.Windows.GridEX.EditType.NoEdit;
                    gridEX1.RootTable.Columns["Column12"].EditType = Janus.Windows.GridEX.EditType.NoEdit;

                }

                PrepareTable();
            }
            catch { }
        }

        private void PrepareTable()
        {

            DataTable FunctionTable = clDoc.ReturnTable(ConWare, "Select Column14,Column08 from table_005_PwhrsOperation where ColumnId=" +
                ReceiptRow["Column04"].ToString());

            string Bed = (FunctionTable.Rows[0]["Column08"].ToString().Trim() == "" ? "NULL" : FunctionTable.Rows[0]["Column08"].ToString());
            string Bes = (FunctionTable.Rows[0]["Column14"].ToString().Trim() == "" ? "NULL" : FunctionTable.Rows[0]["Column14"].ToString());
            SqlDataAdapter Adapter = new SqlDataAdapter();

            SourceTable.Rows.Clear();


            Adapter = new SqlDataAdapter(@"SELECT     column01 AS HeaderID, column13 AS Center, column14 AS Project, SUM(Column21) AS TotalPrice,
                Column25 as CurType,Column26 as CurValue
                FROM         Table_012_Child_PwhrsReceipt WHERE     (column01 = " + _ResidId.TrimEnd(',') +
              ") GROUP BY column01, column13, column14,Column25,Column26", ConWare);


            DataTable BedTable = new DataTable();
            Adapter.Fill(BedTable);

            //درج سطرهای بدهکار
            foreach (DataRow item in BedTable.Rows)
            {


                SourceTable.Rows.Add(12, Bed, Bed, DBNull.Value,
                    (item["Center"].ToString().Trim() == "" ? (object)DBNull.Value : item["Center"].ToString()),
                    (item["Project"].ToString().Trim() == "" ? (object)DBNull.Value : item["Project"].ToString()),
                     "رسید شماره " + ReceiptRow["Column01"].ToString() + " به تاریخ " + ReceiptRow["Column02"].ToString() + " شماره عطف " + ReceiptRow["columnid"].ToString(),
                     Convert.ToDouble(item["TotalPrice"].ToString()), 0,
                     Convert.ToDouble(item["CurValue"].ToString()),
                     (item["CurType"].ToString().Trim() == "" ? (object)null : item["CurType"].ToString()));
            }

            //بستانکاران
            bool _InternalUse = bool.Parse(clDoc.ExScalar(Properties.Settings.Default.PWHRS, "table_005_PwhrsOperation", "Column18", "Columnid", ReceiptRow["Column04"].ToString()).ToString());

            DataTable BesTable = clDoc.ReturnTable(ConWare, @"Select Column01 AS HeaderID, SUM(Column21) AS TotalPrice,Column25 as CurType,Column26 as CurValue
               FROM         Table_012_Child_PwhrsReceipt WHERE     (column01 = " + ReceiptRow["ColumnId"].ToString() + ") GROUP BY column01,Column25,Column26");

            //اگر عملکرد مصرف داخلی باشد شخصی بستانکار نمی شود
            //در غیر این صورت می شود
            if (!_InternalUse)
            {
                foreach (DataRow item in BesTable.Rows)
                {

                    SourceTable.Rows.Add(12, Bes, Bes, (ReceiptRow["Column05"].ToString().Trim() != "" ? ReceiptRow["Column05"].ToString().Trim() : (object)DBNull.Value),
                        DBNull.Value, DBNull.Value, "رسید شماره " + ReceiptRow["Column01"].ToString() + " به تاریخ " + ReceiptRow["Column02"].ToString() + " شماره عطف " + ReceiptRow["columnid"].ToString(),
                        0, Convert.ToDouble(item["TotalPrice"].ToString()), Convert.ToDouble(item["CurValue"].ToString()),
                             (item["CurType"].ToString().Trim() == "" ? (object)null : item["CurType"].ToString()));
                }
            }
            else
            {
                foreach (DataRow item in BesTable.Rows)
                {

                    SourceTable.Rows.Add(12, Bes, Bes, DBNull.Value,
                        DBNull.Value, DBNull.Value, "رسید شماره " + ReceiptRow["Column01"].ToString() + " به تاریخ " + ReceiptRow["Column02"].ToString() + " شماره عطف " + ReceiptRow["columnid"].ToString(),
                        0, Convert.ToDouble(item["TotalPrice"].ToString()), Convert.ToDouble(item["CurValue"].ToString()),
                             (item["CurType"].ToString().Trim() == "" ? (object)null : item["CurType"].ToString()));
                }
            }

        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                txt_Cover.Text = "گردش انبار-صدور سند رسید ";
                faDatePicker1.SelectedDateTime = DateTime.Now;
                rdb_To.Text = null;
                rdb_To.Text = null;
            }
            else
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
            }
        }

        private void rdb_Last_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Last.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                int LastNum = clDoc.LastDocNum();
                txt_To.Text = LastNum.ToString();
                faDatePicker1.Text = clDoc.DocDate(LastNum);
                txt_Cover.Text = clDoc.Cover(LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_Cover.Text = null;
            }
        }

        private void rdb_To_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_To.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                txt_To.Text = null;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
            }
        }

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


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

        private void txt_To_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_To.Text.Trim() != "")
                {
                    clDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                    faDatePicker1.Text = clDoc.DocDate(int.Parse(txt_To.Text.Trim()));
                    txt_Cover.Text = clDoc.Cover(int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void CheckEssentialItems(object sender, EventArgs e)
        {

            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", _ResidId.TrimEnd(',').ToString()) != 0)
                throw new Exception("برای این رسید سند صادر شده است");

            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column15", _ResidId.TrimEnd(',').ToString()) != 0)
                throw new Exception("به علت دارا بودن کارت تولید، صدور سند حسابداری امکان پذیر نمی باشد");

            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column13", _ResidId.TrimEnd(',').ToString()) != 0)
                throw new Exception("برای این رسید، فاکتور خرید صادر شده است" +
                        Environment.NewLine + "جهت صدور سند از فرم فاکتور خرید مربوط اقدام نمایید");

            if (rdb_Last.Checked)
            {
                clDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
            }
            else if (rdb_To.Checked && txt_To.Text.Trim() != "")
            {
                clDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                clDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
                txt_To_Leave(sender, e);
            }


            gridEX1.UpdateData();

            if (Convert.ToDouble(gridEX1.GetTotalRow().Cells["Column11"].Value.ToString()) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            //*****Check Essential Information***//
            if (txt_Cover.Text.Trim() == "" || !faDatePicker1.SelectedDateTime.HasValue)
                throw new Exception("اطلاعات مربوط به تنظیمات سند را کامل کنید");

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                if (item.Cells["Column01"].Text.Trim() == "" || item.Cells["Column01"].Text.ToString().All(char.IsDigit))
                    throw new Exception("سرفصل حساب را مشخص کنید");
            }


            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            //Person--Center--Project//
            int? Person = null;
            Int16? Center = null;
            Int16? Project = null;
            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Person = null;
                Center = null;
                Project = null;
                if (item.Cells["Column07"].Text.Trim() != "")
                    Person = int.Parse(item.Cells["Column07"].Value.ToString());

                if (item.Cells["Column08"].Text.Trim() != "")
                    Center = Int16.Parse(item.Cells["Column08"].Value.ToString());

                if (item.Cells["Column09"].Text.Trim() != "")
                    Project = Int16.Parse(item.Cells["Column09"].Value.ToString());

                clCredit.All_Controls_2(item.Cells["Column01"].Value.ToString().Trim(), Person, Center, Project);

                //**********Check Person Credit************//
                if (item.Cells["Column07"].Text.Trim() != "")
                {
                    if (item.Cells["Column07"].Text.Trim() != "")
                        TPerson.Rows.Add(Int32.Parse(item.Cells["Column07"].Value.ToString()), item.Cells["Column01"].Value.ToString()
                            , (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
                            Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
                }
                //**********Check Account's nature****//
                TAccounts.Rows.Add(item.Cells["Column01"].Value.ToString(), (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
                            Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
            }

            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            try
            {
              

                gridEX1.UpdateData();
                CheckEssentialItems(sender, e);

                SqlParameter DocNum;
                DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                DocNum.Direction = ParameterDirection.Output;
                string command = "declare @Key int ";
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //صدور سند
                    if (rdb_New.Checked)
                    {
                        //DocNum = clDoc.LastDocNum() + 1;
                        //DocID = clDoc.ExportDoc_Header(DocNum,
                        //    faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                        command += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
                        "',getdate()); SET @Key=SCOPE_IDENTITY()";
                    }
                    else if (rdb_Last.Checked)
                    {
                        //DocNum = clDoc.LastDocNum();
                        //DocID = clDoc.DocID(DocNum);
                        command += " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";

                    }
                    else if (rdb_To.Checked)
                    {
                        //DocNum = int.Parse(txt_To.Text.Trim());
                        //DocID = clDoc.DocID(DocNum);
                        command += " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";

                    }
                    //if (DocID > 0)
                    {
                        gridEX1.UpdateData();
                       
                        //اگر رسید ریالی باشد
                        if (ReceiptRow["Column16"].ToString() == "False")
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                if (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 ||
                                    Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0)
                                {
                                    string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());
                                    //clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                    //            , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                    //            (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                    //            (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                    //            (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                    //               12, int.Parse(ReceiptRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);



                                    command += @"INSERT INTO Table_065_SanadDetail  ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) VALUES(@Key,
                                                    '" + item.Cells["Column01"].Value.ToString() + @"',
                                                    " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                                     " + ((_AccInfo[1] != null && !string.IsNullOrWhiteSpace(_AccInfo[1].ToString())) ? "'" + _AccInfo[1].ToString() + "'" : "NULL") + @" ,
                                                    " + ((_AccInfo[2] != null && !string.IsNullOrWhiteSpace(_AccInfo[2].ToString())) ? "'" + _AccInfo[2].ToString() + "'" : "NULL") + @",
                                                    " + ((_AccInfo[3] != null && !string.IsNullOrWhiteSpace(_AccInfo[3].ToString())) ? "'" + _AccInfo[3].ToString() + "'" : "NULL") + @",
                                                    " + ((_AccInfo[4] != null && !string.IsNullOrWhiteSpace(_AccInfo[4].ToString())) ? "'" + _AccInfo[4].ToString() + "'" : "NULL") + @",
                                                    " + (item.Cells["Column07"] != null && !string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? item.Cells["Column07"].Value.ToString() : "NULL") + @",
                                                    " + (item.Cells["Column08"] != null && !string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? item.Cells["Column08"].Value.ToString() : "NULL") + @",
                                                    " + (item.Cells["Column09"] != null && !string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? item.Cells["Column09"].Value.ToString() : "NULL") + @",
                                                    '" + item.Cells["Column10"].Text.Trim() + @"',
                                                    " + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                                                    " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + @",
                                                    0,
                                                    0,
                                                    -1,
                                                    12,
                                                    " + int.Parse(ReceiptRow["ColumnId"].ToString()) + @",
                                                    '" + Class_BasicOperation._UserName + @"',
                                                    getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0)";



                                }
                            }
                        }
                        //اگر رسید ارزی باشد
                        else
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                if (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 ||
                                    Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0)
                                {
                                    string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());

                                    //clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                    //    _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                    //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()),
                                    //    (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                    //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()),
                                    //    item.Cells["Column10"].Text.Trim(),
                                    //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Math.Round(Convert.ToDouble(item.Cells["Column11"].Value.ToString()), 3) *
                                    //    Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0)
                                    //    , (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Math.Round(Convert.ToDouble(item.Cells["Column12"].Value.ToString()), 3)
                                    //    * Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0)
                                    //    , (item.Cells["Column12"].Text == "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) : 0),
                                    //    (item.Cells["Column11"].Text == "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) : 0),
                                    //    Int16.Parse(item.Cells["Column14"].Value.ToString()),
                                    //      12, int.Parse(ReceiptRow["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                    //    float.Parse(item.Cells["Column13"].Value.ToString()));



                                    command += @"INSERT INTO Table_065_SanadDetail  ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) VALUES(@Key,
                                                    '" + item.Cells["Column01"].Value.ToString() + @"',
                                                    " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                                     " + ((_AccInfo[1] != null && !string.IsNullOrWhiteSpace(_AccInfo[1].ToString())) ? "'" + _AccInfo[1].ToString() + "'" : "NULL") + @" ,
                                                    " + ((_AccInfo[2] != null && !string.IsNullOrWhiteSpace(_AccInfo[2].ToString())) ? "'" + _AccInfo[2].ToString() + "'" : "NULL") + @",
                                                    " + ((_AccInfo[3] != null && !string.IsNullOrWhiteSpace(_AccInfo[3].ToString())) ? "'" + _AccInfo[3].ToString() + "'" : "NULL") + @",
                                                    " + ((_AccInfo[4] != null && !string.IsNullOrWhiteSpace(_AccInfo[4].ToString())) ? "'" + _AccInfo[4].ToString() + "'" : "NULL") + @",
                                                    " + (item.Cells["Column07"] != null && !string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? item.Cells["Column07"].Value.ToString() : "NULL") + @",
                                                    " + (item.Cells["Column08"] != null && !string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? item.Cells["Column08"].Value.ToString() : "NULL") + @",
                                                    " + (item.Cells["Column09"] != null && !string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? item.Cells["Column09"].Value.ToString() : "NULL") + @",
                                                    '" + item.Cells["Column10"].Text.Trim() + @"',
                                                    " + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Math.Round(Convert.ToDouble(item.Cells["Column11"].Value.ToString()), 3) *
                                        Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0) + @",
                                                    " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Math.Round(Convert.ToDouble(item.Cells["Column12"].Value.ToString()), 3)
                                        * Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0) + @",
                                                    " + (item.Cells["Column12"].Text == "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) : 0) + @",
                                                    " + (item.Cells["Column11"].Text == "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) : 0) + @",
                                                    " + Int16.Parse(item.Cells["Column14"].Value.ToString()) + @",
                                                    12,
                                                    " + int.Parse(ReceiptRow["ColumnId"].ToString()) + @",
                                                    '" + Class_BasicOperation._UserName + @"',
                                                    getdate(),'" + Class_BasicOperation._UserName + "',getdate()," + float.Parse(item.Cells["Column13"].Value.ToString()) + ")";


                                }

                            }
                        }
                        command += "UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt Set Column19=1 , Column07=@Key where Columnid=" + _ResidId.TrimEnd(',');
                        //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt Set Column19=1 , Column07=" + DocID + " where Columnid=" + _ResidId);





                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = command;
                                Command.Parameters.Add(DocNum);
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                Class_BasicOperation.ShowMsg("", "ثبت سند حسابداری با شماره " + DocNum.Value + " با موفقیت صورت گرفت", "Information");
                                bt_ExportDoc.Enabled = false;
                                //this.DialogResult = DialogResult.Yes;
                                this.DialogResult = DialogResult.OK;
                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                this.Cursor = Cursors.Default;
                                Class_BasicOperation.CheckExceptionType(es, this.Name);
                            }

                            this.Cursor = Cursors.Default;

                            

                        }




                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "Column01")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column01", "ACC_Code", "ACC_Name", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
                else if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
                else if (e.Column.Key == "Column08")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column08", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
                else if (e.Column.Key == "Column09")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column09", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
            }
            catch
            {
            }
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column08");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column09");
            }
            catch
            {
            }
        }

        private void gridEX1_CellEditCanceled(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column08");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column09");
            }
            catch
            {
            }
        }

        private void Form33_ExportDocForReceipt_FormClosing(object sender, FormClosingEventArgs e)
        {
           // this.DialogResult = DialogResult.Cancel;
           
        }

    }
}


