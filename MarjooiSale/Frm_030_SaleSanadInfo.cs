using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;

namespace PCLOR.MarjooiSale
{
    public partial class Frm_030_SaleSanadInfo : Form
    {
        DataRowView SaleRow;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        int ok1 = 0;
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();

      
        SqlParameter DocNum;
        SqlParameter DocID;
        //Classes.CheckCredits clCredit = new PACNT.Classes.CheckCredits();
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        DataTable dt = new DataTable();

        DataTable setting = new DataTable();
        DataTable setting1 = new DataTable();
        DataTable setting2 = new DataTable();
        DataTable setting3 = new DataTable();


        public Frm_030_SaleSanadInfo()
        {
            InitializeComponent();

        }
        public Frm_030_SaleSanadInfo(DataRowView saleRow)
        {
            InitializeComponent();
            SaleRow = saleRow;
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                                                   FactorTable.column01,
                                                                   FactorTable.date,
                                                                   ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                                                   ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                                                   FactorTable.NetTotal
                                                            FROM   (
                                                                       SELECT dbo.Table_010_SaleFactor.columnid ,
                                                                              dbo.Table_010_SaleFactor.column01  ,
                                                                              dbo.Table_010_SaleFactor.column02 AS Date,
                                                                              OtherPrice.PlusPrice AS Ezafat,
                                                                              OtherPrice.MinusPrice AS Kosoorat,
                                                                              dbo.Table_010_SaleFactor.Column28 AS NetTotal
                                                                       FROM   dbo.Table_010_SaleFactor
                                                                              INNER JOIN dbo.Table_011_Child1_SaleFactor
                                                                                   ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                                                                              LEFT OUTER JOIN (
                                                                                       SELECT columnid,
                                                                                              SUM(PlusPrice) AS PlusPrice,
                                                                                              SUM(MinusPrice) AS MinusPrice
                                                                                       FROM   (
                                                                                                  SELECT Table_010_SaleFactor_2.columnid,
                                                                                                         SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                                         PlusPrice,
                                                                                                         0 AS MinusPrice
                                                                                                  FROM   dbo.Table_012_Child2_SaleFactor
                                                                                                         INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                              Table_010_SaleFactor_2
                                                                                                              ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                                                  Table_010_SaleFactor_2.columnid
                                                                                                  WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                                                  GROUP BY
                                                                                                         Table_010_SaleFactor_2.columnid,
                                                                                                         dbo.Table_012_Child2_SaleFactor.column05
                                                                                                  UNION ALL
                                                                                                  SELECT Table_010_SaleFactor_1.columnid,
                                                                                                         0 AS PlusPrice,
                                                                                                         SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                                         MinusPrice
                                                                                                  FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                                         Table_012_Child2_SaleFactor_1
                                                                                                         INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                              Table_010_SaleFactor_1
                                                                                                              ON  
                                                                                                                  Table_012_Child2_SaleFactor_1.column01 = 
                                                                                                                  Table_010_SaleFactor_1.columnid
                                                                                                  WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                                                  GROUP BY
                                                                                                         Table_010_SaleFactor_1.columnid,
                                                                                                         Table_012_Child2_SaleFactor_1.column05
                                                                                              ) AS OtherPrice_1
                                                                                       GROUP BY
                                                                                              columnid
                                                                                   ) AS OtherPrice
                                                                                   ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                                                   ) AS FactorTable
                                                            WHERE FactorTable.columnid=" + int.Parse(SaleRow["ColumnId"].ToString()) + @"
                                                                   ", ConSale);
            Adapter.Fill(dt);
            rdb_New_CheckedChanged(null, null);

            ///////
            try
            {
                Adapter = new SqlDataAdapter(
                                                                    @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 4) ", ConBase);
                Adapter.Fill(setting);


                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر را در تنظیمات فروش وارد کنید");


                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر را در تنظیمات فروش وارد کنید");

                /////////////////////

                Adapter = new SqlDataAdapter(
                                                @"SELECT        column10,column16
                                                                    FROM            Table_024_Discount
                                                                    WHERE        (column02 = 1) ", ConSale);
                setting2 = new DataTable();
                Adapter.Fill(setting2);



                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting2.Rows[0]["Column10"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");



                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting2.Rows[0]["Column16"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");

                //////////////////////////

                Adapter = new SqlDataAdapter(
                                                  @"SELECT        column16,column10
                                                                    FROM            Table_024_Discount
                                                                    WHERE        (Column02 =0) ", ConSale);
                setting3 = new DataTable();
                Adapter.Fill(setting3);


                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting3.Rows[0]["Column16"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting3.Rows[0]["Column10"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");



            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }
        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Text = "فاکتور فروش";
                faDatePicker1.Text = SaleRow["Column02"].ToString();
            }
            else
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
            }
        }

        private void rdb_last_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_last.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                int LastNum = clDoc.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = clDoc.DocDate(LastNum);
                txt_Cover.Text = clDoc.Cover(LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
                txt_Cover.Text = null;
            }
        }

        private void rdb_TO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_TO.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;

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
            Class_BasicOperation.isEnter(e.KeyChar);
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    txt_Cover.Select();
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

        private void txt_Cover_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is FarsiLibrary.Win.Controls.FADatePicker)
            {
                if (Class_BasicOperation.isNotDigit(e.KeyChar))
                    e.Handled = true;
                if (e.KeyChar == 8)
                    _BackSpace = true;
                else
                    _BackSpace = false;
                if (e.KeyChar == 13)
                {
                    Class_BasicOperation.isEnter(e.KeyChar);
                    faDatePicker1.HideDropDown();
                }
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            //*********Just Accounting Document
            if (ok1 == 1)
            {
                try
                {
                    //int RowId = Convert.ToInt32(clDoc.ExScalar(ConSale.ConnectionString, @" select isNull((select Columnid from Table_010_SaleFactor ),0)"));
                    CheckEssentialItems(sender, e);
                    string Message = "آیا مایل به صدور سند حسابداری و حواله انبار هستید؟";
                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {

                        SqlDataAdapter Adapter = new SqlDataAdapter(
                                                                       @"SELECT  [columnid]
                                                                      ,[column01]
                                                                      ,[column02]
                                                                      ,[column03]
                                                                      ,[column04]
                                                                      ,[column05]
                                                                      ,[column06]
                                                                      ,[column07]
                                                                      ,[column08]
                                                                      ,[column09]
                                                                      ,[column10]
                                                                      ,[column11]
                                                                      ,[column12]
                                                                      ,[column13]
                                                                      ,[column14]
                                                                      ,[column15]
                                                                      ,[column16]
                                                                      ,[column17]
                                                                      ,[column18]
                                                                      ,[column19]
                                                                      ,[column20]
                                                                      ,[column21]
                                                                      ,[column22]
                                                                      ,[column23]
                                                                      ,[column24]
                                                                      ,[column25]
                                                                      ,[column26]
                                                                      ,[column27]
                                                                      ,[column28]
                                                                      ,[column29]
                                                                      ,[column30]
                                                                      ,[Column31]
                                                                      ,[Column32]
                                                                      ,[Column33]
                                                                    ,Column34,Column35,Column36,Column37
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" +
                                                                 SaleRow["ColumnId"].ToString(), ConSale);
                        DataTable Child1 = new DataTable();
                        Adapter.Fill(Child1);


//                        foreach (DataRow item in Child1.Rows)
//                        {
//                            if (!clGood.IsGoodInWare(Int16.Parse(SaleRow["Column42"].ToString()),
//                                int.Parse(item["column02"].ToString())))
//                                throw new Exception("کالای " + clDoc.ExScalar(ConWare.ConnectionString,
//                                        "table_004_CommodityAndIngredients", "Column02", "ColumnId",
//                                        item["Column02"].ToString()) +
//                                    " در انبار انتخاب شده فعال نمی باشد");
//                        }


//                        //چک باقی مانده کالا
//                        foreach (DataRow item in Child1.Rows)
//                        {
//                            if (clDoc.IsGood(item["Column02"].ToString()))
//                            {
//                                float Remain = FirstRemain(int.Parse(item["Column02"].ToString()), item["Column34"].ToString());
//                                bool mojoodimanfi = false;
//                                try
//                                {
//                                    using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.PWHRS))
//                                    {

//                                        ConWareGood.Open();
//                                        SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
//                                                                               (
//                                                                                   SELECT ISNULL(Column16, 0) AS Column16
//                                                                                   FROM   table_004_CommodityAndIngredients
//                                                                                   WHERE  ColumnId = " + item["Column02"] + @"
//                                                                               ),
//                                                                               0
//                                                                           ) AS Column16", ConWareGood);
//                                        mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

//                                    }
//                                }
//                                catch
//                                {
//                                }
//                                if (Remain < float.Parse(item["Column07"].ToString()) && !mojoodimanfi)
//                                {
//                                    throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString,
//                                        "table_004_CommodityAndIngredients", "Column02", "ColumnId",
//                                        item["Column02"].ToString()) +
//                                        " کمتر از تعداد مشخص شده در فاکتور است" + Environment.NewLine +
//                                        "موجودی: " + Remain.ToString());
//                                }
//                            }
//                        }


                        //درج هدر حواله
                        SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                        Key.Direction = ParameterDirection.Output;
                       int HavaleNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
                        //, int.Parse(mlt_Ware.Value.ToString()));
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                        {
                            Con.Open();
                            SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(" + HavaleNum + ",'" + SaleRow["column02"].ToString() + "'," + SaleRow["Column42"].ToString()
                                + ",2, " + SaleRow["Column03"].ToString() + ",'" + "حواله صادره بابت فاکتور فروش ش" + SaleRow["column01"].ToString() +
                                "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + SaleRow["ColumnId"].ToString() + ",0,0,0,0,0,0,0,null,0,0); SET @Key=SCOPE_IDENTITY()", Con);
                            InsertHeader.Parameters.Add(Key);
                            InsertHeader.ExecuteNonQuery();
                            int HavaleID = int.Parse(Key.Value.ToString());
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", int.Parse(SaleRow["ColumnId"].ToString()), HavaleID);


                            //درج کالاهای موجود در حواله 
                            foreach (DataRow item1 in Child1.Rows)
                            {
                                if (clDoc.IsGood(item1["Column02"].ToString()))
                                {

                                    SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(" + HavaleID + "," + item1["Column02"].ToString() + "," +
                                       clDoc.ExScalar(ConWare.ConnectionString,
                                        "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                        item1["Column02"].ToString()) + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + item1["Column07"].ToString() + "," +
                                        item1["Column07"].ToString() + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                                        item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
                                        + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                                        (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
                                        item1["Column15"].ToString() +
                                            ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                            (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                            (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                            + "," + item1["Column31"].ToString()
                                            + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")", Con);
                                    InsertDetail.ExecuteNonQuery();
                                }

                            }

                            SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + HavaleID, ConWare);
                            DataTable Table = new DataTable();
                            goodAdapter.Fill(Table);

                            //محاسبه ارزش و ذخیره آن در جدول Child1

                            foreach (DataRow item2 in Table.Rows)
                            {
                                if (Class_BasicOperation._WareType)
                                {
                                    Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_FIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + SaleRow["Column42"].ToString(), Con);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + HavaleID + " and DetailID=" + item2["Columnid"].ToString());
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + SaleRow["Column42"].ToString() + ",@Date='" + SaleRow["column02"].ToString() + "',@id=" + HavaleID + ",@residid=0", ConWare);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                            }
                        }


                        DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                        DocNum.Direction = ParameterDirection.Output;
                        DocID = new SqlParameter("DocID", SqlDbType.Int);
                        DocID.Direction = ParameterDirection.Output;
                        string headercomman = string.Empty;
                        if (rdb_last.Checked)
                        {
                            //DocNum = clDoc.LastDocNum();
                            //DocID = clDoc.DocID(DocNum);
                            headercomman = " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";

                        }
                        else if (rdb_TO.Checked)
                        {
                            //DocNum = int.Parse(txt_To.Text.Trim());
                            //DocID = clDoc.DocID(DocNum);
                            headercomman = " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";

                        }
                        else if (rdb_New.Checked)
                        {
                            //DocNum = clDoc.LastDocNum() + 1;
                            //DocID = clDoc.ExportDoc_Header( DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                            headercomman = @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @DocID=SCOPE_IDENTITY()";
                        }


                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = headercomman;
                                Command.Parameters.Add(DocNum);
                                Command.Parameters.Add(DocID);
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();


                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                MessageBox.Show(es.Message);
                                return;
                            }

                            this.Cursor = Cursors.Default;



                        }

                        //صدور سند
                        //if (rdb_New.Checked)
                        //{
                        //    DocNum = clDoc.LastDocNum() + 1;
                        //    DocID = clDoc.ExportDoc_Header(DocNum,
                        //        faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                        //}
                        //else if (rdb_last.Checked)
                        //{
                        //    DocNum = clDoc.LastDocNum();
                        //    DocID = clDoc.DocID(DocNum);
                        //}
                        //else if (rdb_TO.Checked)
                        //{
                        //    DocNum = int.Parse(txt_To.Text.Trim());
                        //    DocID = clDoc.DocID(DocNum);

                        //}
                        if (Convert.ToInt32( DocID.Value) > 0)
                        {
                            int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", dt.Rows[0]["ColumnId"].ToString()));
                            double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));
                            double value = TotalValue;

                            ////فروش



                            string[] _AccInfo = clDoc.ACC_Info(setting.Rows[0]["Column07"].ToString());
                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                               _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString(),
                                                SaleRow["column03"].ToString(),
                                                null,
                                                null,
                                                "فاکتور فروش ش " + dt.Rows[0]["Column01"].ToString(),

                                                Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["NetTotal"].ToString()), 3)),
                                                  0,
                                                0,
                                                0,
                                                -1,
                                                15,
                                                int.Parse(dt.Rows[0]["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                                0, (short?)null);


                            _AccInfo = clDoc.ACC_Info(setting.Rows[0]["Column13"].ToString());
                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting.Rows[0]["Column13"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                               _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString(),
                                                null,
                                                null,
                                                null,
                                                "فاکتور فروش ش " + dt.Rows[0]["Column01"].ToString(),
                                                0,
                                                Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["NetTotal"].ToString()), 3)),
                                                0,
                                                0,
                                                -1,
                                                15,
                                                int.Parse(dt.Rows[0]["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                                0, (short?)null);

                            ///تخفیف
                            ///
                            if (dt.Rows[0]["Kosoorat"] != null && dt.Rows[0]["Kosoorat"].ToString() != string.Empty && Convert.ToDouble(dt.Rows[0]["Kosoorat"]) > Convert.ToDouble(0))
                            {

                                _AccInfo = clDoc.ACC_Info(setting2.Rows[0]["column10"].ToString());
                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting2.Rows[0]["column10"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                   , SaleRow["column03"].ToString(),

                                                     null,
                                                    null,
                                                   " تخفیف فاکتور فروش ش" + dt.Rows[0]["Column01"].ToString(),
                                                  Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["Kosoorat"].ToString()), 3))
                                                   , 0
                                                   , 0,
                                                  0,
                                                   -1,
                                                      15, int.Parse(dt.Rows[0]["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                                   0, (short?)null);



                                _AccInfo = clDoc.ACC_Info(setting2.Rows[0]["column16"].ToString());
                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting2.Rows[0]["column16"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                   , null,
                                                     null,
                                                    null,
                                                   " تخفیف فاکتور فروش ش" + dt.Rows[0]["Column01"].ToString(),
                                                   0,
                                                  Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["Kosoorat"].ToString()), 3))
                                                   , 0,
                                                  0,
                                                   -1,
                                                      15, int.Parse(dt.Rows[0]["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                                   0, (short?)null);




                            }
                            //ارزش افزوده
                            if (dt.Rows[0]["Ezafat"] != null && dt.Rows[0]["Ezafat"].ToString() != string.Empty && Convert.ToDouble(dt.Rows[0]["Ezafat"]) > 0)
                            {


                                _AccInfo = clDoc.ACC_Info(setting3.Rows[0]["column16"].ToString());
                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting3.Rows[0]["column16"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                   , null,
                                                     null,
                                                    null,
                                                   "ارزش افزوده فاکتور فروش ش" + dt.Rows[0]["Column01"].ToString(),
                                                     0
                                                 , Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["Ezafat"].ToString()), 3))
                                                   , 0,
                                                  0,
                                                  -1,
                                                      15, int.Parse(dt.Rows[0]["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                                   0, (short?)null);



                                _AccInfo = clDoc.ACC_Info(setting3.Rows[0]["column10"].ToString());
                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting3.Rows[0]["column10"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                   , SaleRow["column03"].ToString(),
                                                     null,
                                                    null,
                                                   "ارزش افزوده فاکتور فروش ش" + dt.Rows[0]["Column01"].ToString(),
                                                  Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["Ezafat"].ToString()), 3))
                                                , 0
                                                   , 0,
                                                  0,
                                                  -1,
                                                      15, int.Parse(dt.Rows[0]["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                                   0, (short?)null);



                            }
                            clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column07", "ColumnId", HavaleID, Convert.ToInt32(DocID.Value));
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column10", "ColumnId", int.Parse(dt.Rows[0]["ColumnId"].ToString()), Convert.ToInt32(DocID.Value));

                            Btn_Save.Enabled = false;
                            Class_BasicOperation.ShowMsg("", "ثیت اسناد  با موفقیت انجام شد", "Information");
                            this.Close();

                        }

                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "شماره حساب معتبر وارد نشده است", "Information");

            }
        }
        private void CheckEssentialItems(object sender, EventArgs e)
        {


            if (rdb_last.Checked && txt_LastNum.Text.Trim() != "")
            {
                clDoc.IsFinal(int.Parse(txt_LastNum.Text.Trim()));
            }
            else if (rdb_TO.Checked && txt_To.Text.Trim() != "")
            {
                clDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                clDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
                txt_To_Leave(sender, e);
            }


            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Length != 10)
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");
            if (Convert.ToDouble(dt.Rows[0]["NetTotal"]) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Length != 10)
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");


            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();



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
            Person = null;
            Center = null;
            Project = null;
            All_Controls_Row1(setting.Rows[0]["Column07"].ToString(), int.Parse(SaleRow["column03"].ToString()), Center, Project);
            All_Controls_Row1(setting.Rows[0]["Column13"].ToString(), null, Center, Project);
            All_Controls_Row1(setting2.Rows[0]["column10"].ToString(), int.Parse(SaleRow["column03"].ToString()), Center, Project);
            All_Controls_Row1(setting2.Rows[0]["column16"].ToString(), null, Center, Project);
            All_Controls_Row1(setting3.Rows[0]["column16"].ToString(), null, Center, Project);
            All_Controls_Row1(setting3.Rows[0]["column10"].ToString(), int.Parse(SaleRow["column03"].ToString()), Center, Project);


            TPerson.Rows.Add(Int32.Parse(SaleRow["column03"].ToString()), setting.Rows[0]["Column07"].ToString(), Convert.ToDouble(dt.Rows[0]["NetTotal"].ToString()));
            TPerson.Rows.Add(Int32.Parse(SaleRow["column03"].ToString()), setting2.Rows[0]["column10"].ToString(), Convert.ToDouble(dt.Rows[0]["Kosoorat"].ToString()));
            TPerson.Rows.Add(Int32.Parse(SaleRow["column03"].ToString()), setting3.Rows[0]["column10"].ToString(), Convert.ToDouble(dt.Rows[0]["Ezafat"].ToString()));


            TAccounts.Rows.Add(setting.Rows[0]["Column07"].ToString(), Convert.ToDouble(dt.Rows[0]["NetTotal"].ToString()));
            TAccounts.Rows.Add(setting2.Rows[0]["column10"].ToString(), Convert.ToDouble(dt.Rows[0]["Kosoorat"].ToString()));
            TAccounts.Rows.Add(setting3.Rows[0]["column10"].ToString(), Convert.ToDouble(dt.Rows[0]["Ezafat"].ToString()));

            TAccounts.Rows.Add(setting.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble(dt.Rows[0]["NetTotal"].ToString())));
            TAccounts.Rows.Add(setting2.Rows[0]["column16"].ToString(), (-1 * Convert.ToDouble(dt.Rows[0]["Kosoorat"].ToString())));
            TAccounts.Rows.Add(setting3.Rows[0]["column16"].ToString(), (-1 * Convert.ToDouble(dt.Rows[0]["Ezafat"].ToString())));


            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);




        }
      
        public Int16 AccHasPerson(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Person from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private string AccountName(string AccountCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
                string _AccountName = Select.ExecuteScalar().ToString();
                return _AccountName;
            }
        }
        private Int16 AccHasCenter(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Center from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private Int16 AccHasProject(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Project from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }


            //using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            //{
            //    ConAcnt.Open();
            //    SqlCommand Command = new SqlCommand("Select Control_Person from AllHeaders() where ACC_Code='" + AccountCode + "'", ConAcnt);
            //    if (Person == null && bool.Parse(Command.ExecuteScalar().ToString()))
            //    {
            //        Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            //        Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
            //        throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Center from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Center == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Project from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Project == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }
            //}

        }

        private void Frm_030_SaleSanadInfo_Load(object sender, EventArgs e)
        {

        }
        private float FirstRemain(int GoodCode, string Barcode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT				 ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date , dbo.Table_012_Child_PwhrsReceipt.Column30 AS Grease
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_012_Child_PwhrsReceipt.column02 = {0}) 
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.Column30
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date, dbo.Table_008_Child_PwhrsDraft.Column30 AS Grease
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_008_Child_PwhrsDraft.column02 = {0})  
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.Column30) AS derivedtbl_1
                       WHERE     (Date <= '{1}' and Grease={2} ) ";
                CommandText = string.Format(CommandText, GoodCode, faDatePicker1.Text, Barcode);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
    }
}
