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
    public partial class Frm_015_ExportDoc_Return : Form
    {
        bool _Tab1 = false, _Tab2 = false, _Tab3 = false;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
       Classes. Class_UserScope UserScope = new Classes. Class_UserScope();
        int _SaleReturnID, DocNum = 0, DocID = 0, _ResidID = 0, _ResidNum = 0;
        SqlDataAdapter SaleReturnAdapter, Child2Adapter, Child1Adapter;
        BindingSource SaleReturnBind, Child1Bind, Child2Bind;
        DataSet DS = new DataSet();
        DataRowView SalReturnRow;
        DataTable setting = new DataTable();

        DataTable SourceTable = new DataTable();

        public Frm_015_ExportDoc_Return(bool Tab1, bool Tab2, bool Tab3, int SaleReturnID)
        {
            InitializeComponent();
            _Tab1 = Tab1;
            _Tab2 = Tab2;
            _Tab3 = Tab3;
            _SaleReturnID = SaleReturnID;
        }

        private void Frm_009_ExportDocInformation_Load(object sender, EventArgs e)
        {


           
            //chk_RegisterGoods.Checked = Properties.Settings.Default.RegisterReturnSaleFactorWithGoods;
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
            SourceTable.Columns.Add("Column13", Type.GetType("System.Double"));
            SourceTable.Columns.Add("Column14", Type.GetType("System.Int16"));
            SourceTable.Columns.Add("Bed", Type.GetType("System.Int16"));

            SourceTable.Columns["Column14"].AllowDBNull = true;
            gridEX1.DataSource = SourceTable;

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * from AllHeaders()", ConAcnt);
            Adapter.Fill(DS, "Header");
            mlt_SaleReturnBed.DataSource = DS.Tables["Header"];
            mlt_SaleReturnBes.DataSource = DS.Tables["Header"];
            mlt_ValueBed.DataSource = DS.Tables["Header"];
            mlt_ValueBes.DataSource = DS.Tables["Header"];
            mlt_LinAddBed.DataSource = DS.Tables["Header"];
            mlt_LinAddBes.DataSource = DS.Tables["Header"];
            mlt_LinDisBed.DataSource = DS.Tables["Header"];
            mlt_LinDisBes.DataSource = DS.Tables["Header"];

            //Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS  where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')", ConWare);
            //Adapter.Fill(DS, "Ware");
            //mlt_Ware.DataSource = DS.Tables["Ware"];
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                    @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
            mlt_Ware.DataSource = WareTable;

            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=0", ConWare);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];

            //*********************
            SaleReturnAdapter = new SqlDataAdapter("Select * from Table_018_MarjooiSale where ColumnId=" + _SaleReturnID, ConSale);
            SaleReturnAdapter.Fill(DS, "Sale");
            SaleReturnBind = new BindingSource();
            SaleReturnBind.DataSource = DS.Tables["Sale"];
            SalReturnRow = (DataRowView)this.SaleReturnBind.CurrencyManager.Current;

            Child2Adapter = new SqlDataAdapter("Select * from Table_020_Child2_MarjooiSale where Column01=" + _SaleReturnID, ConSale);
            Child2Adapter.Fill(DS, "Child2");
            Child2Bind = new BindingSource();
            Child2Bind.DataSource = DS.Tables["Child2"];

            Child1Adapter = new SqlDataAdapter("Select *,CAST(0 as decimal(18, 4)) as UnitValue,CAST(0 as decimal(18, 4)) as TotalValue from Table_019_Child1_MarjooiSale where Column01=" + _SaleReturnID, ConSale);
            Child1Adapter.Fill(DS, "Child1");
            Child1Bind = new BindingSource();
            Child1Bind.DataSource = DS.Tables["Child1"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            gridEX1.DropDowns["Person"].SetDataBinding(Person, "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            DataTable Center = new DataTable();
            Adapter.Fill(Center);
            gridEX1.DropDowns["Center"].SetDataBinding(Center, "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            DataTable Project = new DataTable();
            Adapter.Fill(Project);
            gridEX1.DropDowns["Project"].SetDataBinding(Project, "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_055_CurrencyInfo", ConBase);
            DataTable Currency = new DataTable();
            Adapter.Fill(Currency);
            mlt_CurrencyType.DataSource = Currency;

            Adapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            DataTable Headers = new DataTable();
            Adapter.Fill(Headers);
            gridEX1.DropDowns["Header_Code"].SetDataBinding(Headers, "");
            gridEX1.DropDowns["Header_Name"].SetDataBinding(Headers, "");

            if (SalReturnRow["Column12"].ToString() == "False")
            {
                mlt_SaleReturnBed.Value = clDoc.Account(12, "Column07");
                mlt_SaleReturnBes.Value = clDoc.Account(12, "Column13");
            }
            else
            {
                mlt_SaleReturnBed.Value = clDoc.Account(21, "Column07");
                mlt_SaleReturnBes.Value = clDoc.Account(21, "Column13");
            }
            mlt_LinAddBed.Value = clDoc.Account(7, "Column13");
            mlt_LinAddBes.Value = clDoc.Account(7, "Column07");
            mlt_LinDisBed.Value = clDoc.Account(6, "Column13");
            mlt_LinDisBes.Value = clDoc.Account(6, "Column07");

            mlt_ValueBed.Value = clDoc.Account(14, "Column07");
            mlt_ValueBes.Value = clDoc.Account(14, "Column13");
            faDatePicker1.Text = SalReturnRow["Column02"].ToString();

            uiTab1.Enabled = _Tab1;
            uiTab2.Enabled = _Tab2;
            uiTab3.Enabled = _Tab3;

            GridEXColumn dateColumn = gridEX1.RootTable.Columns["Column11"];
            GridEXFilterCondition composite = new GridEXFilterCondition();
            GridEXFilterCondition filter1 = new GridEXFilterCondition(dateColumn,
            ConditionOperator.Equal, 0);
            GridEXFilterCondition filter2 = new GridEXFilterCondition(gridEX1.RootTable.Columns["Column12"],
            ConditionOperator.Equal, 0);
            composite.AddCondition(filter1);
            composite.AddCondition(LogicalOperator.And, filter2);
            GridEXFormatCondition fc = new GridEXFormatCondition();
            fc.FilterCondition = composite;
            fc.FormatStyle.ForeColor = Color.Blue;
            gridEX1.RootTable.FormatConditions.Add(fc);
            Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConMain);
            Adapter.Fill(setting);
            chk_Baha.Checked = Properties.Settings.Default.ExtraMethod;
            if (Properties.Settings.Default.SalePCBes)
                chk_PCBes.Checked = true;
            else
                chk_PCBes.Checked = false;
            if (Properties.Settings.Default.SalePCBed)

                chk_PCBed.Checked = true;
            else
                chk_PCBed.Checked = false;
            chk_RegisterGoods.Checked = Properties.Settings.Default.RegisterSaleFactorWithGoods;
            chk_Nots.Checked = Properties.Settings.Default.RegisterSaleFactorNoteGoods;
            this.chk_Net.Checked = Properties.Settings.Default.chk_Net;
            chk_AggDoc.Checked = Properties.Settings.Default.AggregationSaleDoc;
            chk_GoodACCNum.Checked = Properties.Settings.Default.SaleGoodACCNum;

            if (!chk_Nots.Checked && !chk_RegisterGoods.Checked)
                chk_without.Checked = true;
            PrepareDoc();
            chk_DraftNum.Checked = false;
            txt_DraftNum.Enabled = false;
        }


        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Text = "فاکتور مرجوعی فروش";
                faDatePicker1.Text = SalReturnRow["Column02"].ToString();
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
        
        private void rdb_TO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_TO.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                this.txt_serial.Enabled = false;
                txt_serial.Text = null;
                txt_To.Enabled = true;
                txt_To.Select();

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

       

     

        private void PrepareDoc()
        {
            SourceTable.Rows.Clear();
            #region تفکیک پروژه
            if (chk_Baha.Checked)
            {
                SqlDataAdapter Adapter;
                if (chk_GoodACCNum.Checked)

                    Adapter = new SqlDataAdapter(@"SELECT     Project, Total, Discount, Adding, column01, Total - Discount + Adding AS Net ,column33
                             FROM         (SELECT     dbo.Table_019_Child1_MarjooiSale.column22 AS Project, ISNULL(SUM(dbo.Table_019_Child1_MarjooiSale.column11), 0) AS Total, ISNULL(SUM(dbo.Table_019_Child1_MarjooiSale.column17), 0) AS Discount, ISNULL(SUM(dbo.Table_019_Child1_MarjooiSale.column19), 0) AS Adding, dbo.Table_019_Child1_MarjooiSale.column01,tcai.column33
                             
                             FROM          dbo.Table_019_Child1_MarjooiSale
                                JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_019_Child1_MarjooiSale.column02 
                             GROUP BY dbo.Table_019_Child1_MarjooiSale.column22, dbo.Table_019_Child1_MarjooiSale.column01,tcai.column33
                             HAVING      (dbo.Table_019_Child1_MarjooiSale.column01 = {0})) AS derivedtbl_1", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT     Project, Total, Discount, Adding, column01, Total - Discount + Adding AS Net ,null as column33
                             FROM         (SELECT     column22 AS Project, ISNULL(SUM(column11), 0) AS Total, ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01
                             
                             FROM          dbo.Table_019_Child1_MarjooiSale
                             GROUP BY column22, column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SalReturnRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                DataTable detali = new DataTable();

                if (chk_GoodACCNum.Checked)

                    Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project,tcai.column33
                                            FROM   Table_019_Child1_MarjooiSale tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SalReturnRow["ColumnId"].ToString() + "  ", ConSale);
                else


                    Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project,null as column33
                                            FROM   Table_019_Child1_MarjooiSale tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SalReturnRow["ColumnId"].ToString() + "  ", ConSale);
                Adapter.Fill(detali);


                //  فاکتور مرجوعی فروش با احتساب تخفیفات و اضافات خطی

                //********Bed

                if (chk_RegisterGoods.Checked)
                {

                    foreach (DataRow dt in detali.Rows)
                    {
                        string sharhjoz = string.Empty;
                        sharhjoz = " کالای  " + dt["column02"].ToString();
                        if (Convert.ToBoolean(setting.Rows[32]["Column02"]))
                            sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                        if (Convert.ToBoolean(setting.Rows[34]["Column02"]))
                            sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                        if (!chk_Net.Checked)
                        {
                            //********Bed
                            if (chk_PCBed.Checked)
                                SourceTable.Rows.Add(17,
                                ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                null,
                                null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                Convert.ToDouble(dt["column11"].ToString()),
                                0, 0, -1);
                            else
                                SourceTable.Rows.Add(17,
                                    ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                               null,
                               null,
                               null,
                               "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                               Convert.ToDouble(dt["column11"].ToString()),
                               0, 0, -1);

                            //********Bes
                            if (chk_PCBes.Checked)
                                SourceTable.Rows.Add(17, (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                                (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(),
                                null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                 0,
                                 Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                            else
                                SourceTable.Rows.Add(17, (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                              (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(),
                              null,
                              null,
                              "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                               0,
                               Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                        }
                        else
                        {
                            //********Bed
                            if (chk_PCBed.Checked)
                                SourceTable.Rows.Add(17,
                                   ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                                null,
                                null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                Convert.ToDouble(dt["Net"].ToString()),
                                0, 0, -1);
                            else
                                SourceTable.Rows.Add(17,
                                    ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                               null,
                               null,
                               null,
                               "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                               Convert.ToDouble(dt["Net"].ToString()),
                               0, 0, -1);
                            //********Bes
                            if (chk_PCBes.Checked)
                                SourceTable.Rows.Add(17, (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                                (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(),
                                null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null),
                                "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                 0,
                                 Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                            else
                                SourceTable.Rows.Add(17, (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                               (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(),
                               null,
                               null,
                               "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                0,
                                Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                        }

                    }


                }
                else
                {
                    foreach (DataRow item in Table.Rows)
                    {

                        if (chk_Net.Checked)
                        {
                            //********Bed
                            if (chk_PCBed.Checked)
                                SourceTable.Rows.Add(17,
                                    ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                                    null,
                                    null,
                                    (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                    "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                   Convert.ToDouble(item["Net"].ToString()), 0, 0, -1);
                            else

                                SourceTable.Rows.Add(17,
                                      ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                             null,
                             null,
                             null,
                             "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                            Convert.ToDouble(item["Net"].ToString()), 0, 0, -1);

                            //********Bes
                            if (chk_PCBes.Checked)
                                SourceTable.Rows.Add(17, (
                                mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                                SalReturnRow["Column03"].ToString(),
                                null,
                                (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                0, Convert.ToDouble(item["Net"].ToString()), 0, -1);
                            else
                                SourceTable.Rows.Add(17, (
                              mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                              SalReturnRow["Column03"].ToString(),
                              null,
                              null,
                              "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                              0, Convert.ToDouble(item["Net"].ToString()), 0, -1);

                        }
                        else
                        {
                            //********Bed
                            if (chk_PCBed.Checked)
                                SourceTable.Rows.Add(17,
                                      ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                                   null,
                                   null,
                                   (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                   "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                  Convert.ToDouble(item["Total"].ToString()), 0, 0, -1);
                            else
                                SourceTable.Rows.Add(17,
                                     ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                               Convert.ToDouble(item["Total"].ToString()), 0, 0, -1);
                            //********Bes
                            if (chk_PCBes.Checked)
                                SourceTable.Rows.Add(17, (
                                mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                                SalReturnRow["Column03"].ToString(),
                                null,
                                (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                0, Convert.ToDouble(item["Total"].ToString()), 0, -1);
                            else
                                SourceTable.Rows.Add(17, (
                                mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                                SalReturnRow["Column03"].ToString(),
                                null,
                                null,
                                "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                0, Convert.ToDouble(item["Total"].ToString()), 0, -1);

                        }
                    }
                }



                // مربوط به اضافات خطی فاکتور
                if (Convert.ToDouble(SalReturnRow["Column21"].ToString()) != 0 && !chk_Net.Checked)
                {


                    DataTable ezafeTable = clDoc.ReturnTable(ConSale,
               "Select SUM(column19) as ezafe, column22 from Table_019_Child1_MarjooiSale where column01=" + SalReturnRow["ColumnId"].ToString() + "  group by column22");
                    foreach (DataRow d in ezafeTable.Rows)
                    {
                        if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                        {
                            //********Bed
                            if (chk_PCBed.Checked)

                                SourceTable.Rows.Add(17, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                    (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), null,
                                    null, ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null), (Convert.ToDouble(SalReturnRow["Column21"].ToString()) > 0 ? "اضافه خطی  فاکتور مرجوعی فروش ش " : "تخفیف خطی2 فاکتور مرجوعی فروش ش ") +
                                    SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                    Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                            else

                                SourceTable.Rows.Add(17, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                               (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), null,
                               null,
                               null,
                               (Convert.ToDouble(SalReturnRow["Column21"].ToString()) > 0 ? "اضافه خطی  فاکتور مرجوعی فروش ش " : "تخفیف خطی2 فاکتور مرجوعی فروش ش ") +
                               SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                               Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);

                            //*********Bes
                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(17, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(), null, ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                    (Convert.ToDouble(SalReturnRow["Column21"].ToString()) > 0 ? "اضافه خطی  فاکتور مرجوعی فروش ش " : "تخفیف خطی2 فاکتور مرجوعی فروش ش ")
                                    + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                    0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                            else
                                SourceTable.Rows.Add(17, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                             (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(), null, null,
                             (Convert.ToDouble(SalReturnRow["Column21"].ToString()) > 0 ? "اضافه خطی  فاکتور مرجوعی فروش ش " : "تخفیف خطی2 فاکتور مرجوعی فروش ش ")
                             + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                             0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                        }
                    }

                }


                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(SalReturnRow["Column22"].ToString()) > 0 && !chk_Net.Checked)
                {
                    DataTable takhfifTable = clDoc.ReturnTable(ConSale,
                        "Select SUM(column17) as takhfif, column22 from Table_019_Child1_MarjooiSale where column01=" + SalReturnRow["ColumnId"].ToString() + "  group by column22");

                    foreach (DataRow h in takhfifTable.Rows)
                    {

                        if (Convert.ToInt64(h["takhfif"]) > 0)
                        {
                            //********Bed
                            if (chk_PCBed.Checked)

                                SourceTable.Rows.Add(17, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), SalReturnRow["Column03"].ToString(), null, ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                            else
                                SourceTable.Rows.Add(17, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), SalReturnRow["Column03"].ToString(), null, null,
                                    "تخفیف خطی فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);


                            //*********Bes
                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(17, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), null, null, ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                            else
                                SourceTable.Rows.Add(17, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), null, null, null,
                                    "تخفیف خطی فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);

                        }
                    }

                }


                //سایر اضافات و کسورات
                foreach (DataRowView item in Child2Bind)
                {
                    string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column16", "ColumnId", item["Column02"].ToString());
                    string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column10", "ColumnId", item["Column02"].ToString());
                    string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column01", "ColumnId", item["Column02"].ToString());

                    //********Bed
                    SourceTable.Rows.Add(17, Bed, Bed, (item["Column05"].ToString() == "True" ? SalReturnRow["Column03"].ToString() : null), null, null,
                        Name + " فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                         Convert.ToDouble(item["Column04"].ToString()), 0,
                         (SalReturnRow["Column24"].ToString().Trim() == "" ? "0" : SalReturnRow["Column24"].ToString()),
                         (SalReturnRow["Column23"].ToString().Trim() == "" ? "-1" : SalReturnRow["Column23"].ToString()));

                    //*********Bes
                    SourceTable.Rows.Add(17, Bes, Bes, (item["Column05"].ToString() == "False" ? SalReturnRow["Column03"].ToString() : null), null, null, Name +
                        " فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), 0,
                        Convert.ToDouble(item["Column04"].ToString())
                        , (SalReturnRow["Column24"].ToString().Trim() == "" ? "0" : SalReturnRow["Column24"].ToString()), (SalReturnRow["Column23"].ToString().Trim() == "" ? "-1" : SalReturnRow["Column23"].ToString()));

                }
                //صدور سند ارزش رسید 
                if (uiTab3.Enabled)
                {
                    int ResidId = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", SalReturnRow["ColumnId"].ToString()));
                    double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_012_Child_PwhrsReceipt", "ISNULL(SUM(Column21),0)", "Column01", ResidId.ToString()));

                    if (ResidId > 0)
                    {
                        DataTable baha = new DataTable();
                        Adapter = new SqlDataAdapter("SELECT column14,ISNULL(SUM(Column21),0) as Column16  from Table_012_Child_PwhrsReceipt Where Column01=" + ResidId + " Group by column14 ", ConWare);
                        Adapter.Fill(baha);
                        foreach (DataRow row in baha.Rows)
                        {
                            if (Convert.ToDouble(row["Column16"]) > 0)
                            {
                                if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                                {
                                    //********Bed
                                    if (chk_PCBed.Checked)

                                        SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                               (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]),
                               "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                               " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"]), 0).ToString()), 0, 0, -1, 1);

                                    else
                                        SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                                        (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                        "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                                        " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"]), 0).ToString()), 0, 0, -1, 1);

                                    //*********Bes
                                    if (chk_PCBes.Checked)

                                        SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                            (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]),
                                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                            SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"])).ToString()), 0, -1, 0);

                                    else
                                        SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                            (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                            SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"])).ToString()), 0, -1, 0);
                                }
                                else
                                {
                                    //********Bed
                                    SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                                   (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                   "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                                   " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"]), 0).ToString()), 0, 0, -1, 1);


                                    //*********Bes
                                    SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                     (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                     "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                     SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"])).ToString()), 0, -1, 0);
                                }
                            }
                            else
                            {
                                if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                                {
                                    //********Bed
                                    if (chk_PCBed.Checked)

                                        SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                                       (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]),
                                       "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                                       " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"]), 0).ToString()), 0, 0, -1, 1);

                                    else
                                        SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                                           (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                           "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                                           " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"]), 0).ToString()), 0, 0, -1, 1);


                                    //*********Bes
                                    if (chk_PCBes.Checked)

                                        SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                            (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]),
                                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                            SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"])).ToString()), 0, -1, 0);
                                    else
                                        SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                        (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                        "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                        SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"])).ToString()), 0, -1, 0);
                                }
                                else
                                {
                                    //********Bed
                                    SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                           (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                           "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                           " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"]), 0).ToString()), 0, 0, -1, 1);


                                    //*********Bes
                                    SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                        (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                        "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                        SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(Convert.ToDouble(row["Column16"])).ToString()), 0, -1, 0);
                                }
                            }

                        }



                    }
                    else
                    {
                        DataTable baha = new DataTable();
                        Adapter = new SqlDataAdapter("SELECT column22 as column14,ISNULL(SUM(column20),0) as Column16  from Table_019_Child1_MarjooiSale Where Column01=" + SalReturnRow["columnid"].ToString() + " Group by column22 ", ConSale);
                        Adapter.Fill(baha);

                        foreach (DataRow row in baha.Rows)
                        {

                            if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                                   (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]),
                                   "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                                   " به تاریخ " + SalReturnRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                                else
                                    SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                                    (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                    "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                                    " به تاریخ " + SalReturnRow["Column02"].ToString(), 0, 0, 0, -1, 1);


                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                        (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]),
                                        "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                        SalReturnRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                                else

                                    SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                        (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                        "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                        SalReturnRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                            }
                            else
                            {
                                //********Bed
                                SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                               (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                               "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                               " به تاریخ " + SalReturnRow["Column02"].ToString(), 0, 0, 0, -1, 1);


                                //*********Bes
                                SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                                    (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                    "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                                    SalReturnRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                            }
                        }

                    }


                }
            }
            #endregion
            #region عدم تفکیک پروژه
            else
            {
                SqlDataAdapter Adapter;
                if (chk_GoodACCNum.Checked)

                    Adapter = new SqlDataAdapter(@"SELECT       Total, Discount, Adding, column01, Total - Discount + Adding AS Net , column33
                             FROM         (SELECT       ISNULL(SUM(dbo.Table_019_Child1_MarjooiSale.column11), 0) AS Total, ISNULL(SUM(dbo.Table_019_Child1_MarjooiSale.column17), 0) AS Discount, ISNULL(SUM(dbo.Table_019_Child1_MarjooiSale.column19), 0) AS Adding, dbo.Table_019_Child1_MarjooiSale.column01,tcai.column33
                             
                             FROM          dbo.Table_019_Child1_MarjooiSale
                            JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_019_Child1_MarjooiSale.column02 
                             GROUP BY  dbo.Table_019_Child1_MarjooiSale.column01 ,tcai.column33
                             HAVING      (dbo.Table_019_Child1_MarjooiSale.column01 = {0})) AS derivedtbl_1", ConSale);
                else


                    Adapter = new SqlDataAdapter(@"SELECT       Total, Discount, Adding, column01, Total - Discount + Adding AS Net ,null as column33
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01
                             
                             FROM          dbo.Table_019_Child1_MarjooiSale
                             GROUP BY  column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SalReturnRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                DataTable detali = new DataTable();


                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02 ,tcai.column33
                                            FROM   Table_019_Child1_MarjooiSale tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SalReturnRow["ColumnId"].ToString() + "  ", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02 ,null as column33
                                            FROM   Table_019_Child1_MarjooiSale tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SalReturnRow["ColumnId"].ToString() + "  ", ConSale);
                Adapter.Fill(detali);


                //  فاکتور مرجوعی فروش با احتساب تخفیفات و اضافات خطی

                //********Bed

                if (chk_RegisterGoods.Checked)
                {

                    foreach (DataRow dt in detali.Rows)
                    {
                        string sharhjoz = string.Empty;
                        sharhjoz = " کالای  " + dt["column02"].ToString();
                        if (Convert.ToBoolean(setting.Rows[32]["Column02"]))
                            sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                        if (Convert.ToBoolean(setting.Rows[34]["Column02"]))
                            sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                        if (!chk_Net.Checked)
                        {
                            SourceTable.Rows.Add(17,
                                     ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                     ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                    null,
                                    null, null, "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                    Convert.ToDouble(dt["column11"].ToString()),
                                    0, 0, -1);

                            SourceTable.Rows.Add(17,
                                (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                        (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                        SalReturnRow["Column03"].ToString(),
                        null, null, "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                         0,
                         Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                        }
                        else
                        {
                            SourceTable.Rows.Add(17,
                               ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                               ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                                null,
                                null, null, "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                                Convert.ToDouble(dt["Net"].ToString()),
                                0, 0, -1);

                            SourceTable.Rows.Add(17, (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                        (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(),
                        null, null,
                        "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString() + sharhjoz,
                         0,
                         Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                        }

                    }


                }
                else
                {
                    foreach (DataRow item in Table.Rows)
                    {

                        if (chk_Net.Checked)
                        {
                            SourceTable.Rows.Add(17,
                               ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                               ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                               Convert.ToDouble(item["Net"].ToString()), 0, 0, -1);

                            SourceTable.Rows.Add(17, (
                        mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                        SalReturnRow["Column03"].ToString(),
                        null,
                        null,
                        "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                        0, Convert.ToDouble(item["Net"].ToString()), 0, -1);
                        }
                        else
                        {
                            SourceTable.Rows.Add(17,
                                ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),
                               ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleReturnBed.Text.Trim() != "" ? mlt_SaleReturnBed.Value.ToString() : null)),

                               null,
                               null,
                               null,
                               "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                              Convert.ToDouble(item["Total"].ToString()), 0, 0, -1);

                            SourceTable.Rows.Add(17, (
                        mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null), (mlt_SaleReturnBes.Text.Trim() != "" ? mlt_SaleReturnBes.Value.ToString() : null),
                        SalReturnRow["Column03"].ToString(),
                        null,
                        null,
                        "فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                        0, Convert.ToDouble(item["Total"].ToString()), 0, -1);
                        }
                    }
                }



                // مربوط به اضافات خطی فاکتور
                if (Convert.ToDouble(SalReturnRow["Column21"].ToString()) != 0 && !chk_Net.Checked)
                {


                    DataTable ezafeTable = clDoc.ReturnTable(ConSale,
               "Select SUM(column19) as ezafe from Table_019_Child1_MarjooiSale where column01=" + SalReturnRow["ColumnId"].ToString() + "  ");
                    foreach (DataRow d in ezafeTable.Rows)
                    {
                        if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                        {
                            //********Bed
                            SourceTable.Rows.Add(17, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), null,
                                null, null, (Convert.ToDouble(SalReturnRow["Column21"].ToString()) > 0 ? "اضافه خطی  فاکتور مرجوعی فروش ش " : "تخفیف خطی2 فاکتور مرجوعی فروش ش ") +
                                SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                            //*********Bes
                            SourceTable.Rows.Add(17, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), SalReturnRow["Column03"].ToString(), null, null,
                                (Convert.ToDouble(SalReturnRow["Column21"].ToString()) > 0 ? "اضافه خطی  فاکتور مرجوعی فروش ش " : "تخفیف خطی2 فاکتور مرجوعی فروش ش ")
                                + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                                0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                        }
                    }

                }


                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(SalReturnRow["Column22"].ToString()) > 0 && !chk_Net.Checked)
                {
                    DataTable takhfifTable = clDoc.ReturnTable(ConSale,
                        "Select SUM(column17) as takhfif from Table_019_Child1_MarjooiSale where column01=" + SalReturnRow["ColumnId"].ToString() + "  ");

                    foreach (DataRow h in takhfifTable.Rows)
                    {

                        if (Convert.ToInt64(h["takhfif"]) > 0)
                        {
                            //********Bed
                            SourceTable.Rows.Add(17, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), SalReturnRow["Column03"].ToString(), null,
                                null, "تخفیف خطی فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                            //*********Bes
                            SourceTable.Rows.Add(17, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), null, null,
                                null, "تخفیف خطی فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                        }
                    }

                }


                //سایر اضافات و کسورات
                foreach (DataRowView item in Child2Bind)
                {
                    string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column16", "ColumnId", item["Column02"].ToString());
                    string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column10", "ColumnId", item["Column02"].ToString());
                    string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column01", "ColumnId", item["Column02"].ToString());

                    //********Bed
                    SourceTable.Rows.Add(17, Bed, Bed, (item["Column05"].ToString() == "True" ? SalReturnRow["Column03"].ToString() : null), null, null,
                        Name + " فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(),
                         Convert.ToDouble(item["Column04"].ToString()), 0,
                         (SalReturnRow["Column24"].ToString().Trim() == "" ? "0" : SalReturnRow["Column24"].ToString()),
                         (SalReturnRow["Column23"].ToString().Trim() == "" ? "-1" : SalReturnRow["Column23"].ToString()));

                    //*********Bes
                    SourceTable.Rows.Add(17, Bes, Bes, (item["Column05"].ToString() == "False" ? SalReturnRow["Column03"].ToString() : null), null, null, Name +
                        " فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString(), 0,
                        Convert.ToDouble(item["Column04"].ToString())
                        , (SalReturnRow["Column24"].ToString().Trim() == "" ? "0" : SalReturnRow["Column24"].ToString()), (SalReturnRow["Column23"].ToString().Trim() == "" ? "-1" : SalReturnRow["Column23"].ToString()));

                }


                if (uiTab3.Enabled)
                {
                    int ResidId = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", SalReturnRow["ColumnId"].ToString()));
                    double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_012_Child_PwhrsReceipt", "ISNULL(SUM(Column21),0)", "Column01", ResidId.ToString()));
                    if (TotalValue > 0)
                    {
                        //********Bed
                        SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                            (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() +
                            " به تاریخ " + SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(TotalValue, 0).ToString()), 0, 1, -1, 1);
                        //*********Bes
                        SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                            (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                            SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(TotalValue, 0).ToString()), 1, -1, 0);
                    }
                    else
                    {
                        //********Bed
                        SourceTable.Rows.Add(27, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null),
                            (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                            SalReturnRow["Column02"].ToString(), Convert.ToDouble(Math.Round(TotalValue, 0).ToString()), 0, 1, -1, 1);
                        //*********Bes
                        SourceTable.Rows.Add(27, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null),
                            (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                            "بهای تمام شده- فاکتور مرجوعی فروش ش " + SalReturnRow["Column01"].ToString() + " به تاریخ " +
                            SalReturnRow["Column02"].ToString(), 0, Convert.ToDouble(Math.Round(TotalValue, 0).ToString()), 1, -1, 0);
                    }

                }
            }

            #endregion
            gridEX1.DataSource = SourceTable;

            if (chk_AggDoc.Checked)
                AggDoc();

        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            //*********Just Accounting Document
            gridEX1.UpdateData();
            SqlParameter DocNum;
            DocNum = new SqlParameter("DocNum", SqlDbType.Int);
            DocNum.Direction = ParameterDirection.Output;
            string CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
            try
            {
                CheckEssentialItems(sender, e);

                string Message = "آیا مایل به صدور سند حسابداری هستید؟";
                if (uiTab2.Enabled)
                    Message = "آیا مایل به صدور سند حسابداری و رسید انبار هستید؟";

                if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //ثبت رسید
                    if (uiTab2.Enabled)
                    {
                        if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", _SaleReturnID.ToString()) != 0)
                            throw new Exception("برای این فاکتور رسید انبار صادر شده است");
                        else ExportResid();
                    }

                    if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column10", _SaleReturnID.ToString()) != 0)
                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");

                    //صدور سند
                    if (rdb_New.Checked)
                    {
                        //DocNum = clDoc.LastDocNum() + 1;
                        CommandTxt += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @Key=SCOPE_IDENTITY()";
                        //DocID = clDoc.ExportDoc_Header(DocNum,
                        //    faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                    }
                    else if (rdb_last.Checked)
                    {
                        //DocNum = clDoc.LastDocNum();
                        //DocID = clDoc.DocID(DocNum);
                        CommandTxt += " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";
                    }
                    else if (rdb_TO.Checked)
                    {
                        //DocNum = int.Parse(txt_To.Text.Trim());
                        //DocID = clDoc.DocID(DocNum);
                        CommandTxt += " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";


                    }
                    else if (rb_toSerial.Checked)
                        CommandTxt += " set @DocNum=(Select Column00 from Table_060_SanadHead where ColumnId=" + int.Parse(this.txt_serial.Text.Trim()) + ")    SET @Key=" + int.Parse(this.txt_serial.Text.Trim()) + "";

                    // if (DocID > 0)
                    {
                        gridEX1.UpdateData();
                        CommandTxt += @" set @ResidID=(select Column09 from " + ConSale.Database + ".dbo.Table_018_MarjooiSale where  ColumnId=" + SalReturnRow["ColumnId"].ToString() + ")";
                        CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column21),0) from " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt where Column01=@ResidID),0)";
                        CommandTxt += "set @value=@TotalValue   ";

                        // int ResidID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", SalReturnRow["ColumnId"].ToString()));
                        // double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_012_Child_PwhrsReceipt", "ISNULL(SUM(Column21),0)", "Column01", ResidID.ToString()));
                        //  double value = TotalValue;


                        DateTime BaseDate;
                        DateTime SecDate;
                        BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDate.Parse(faDatePicker1.Text));
                        FarsiLibrary.Win.Controls.FADatePicker fa;
                        fa = new FarsiLibrary.Win.Controls.FADatePicker();
                        fa.Text = faDatePicker1.Text;
                        try
                        {
                            SecDate = BaseDate.AddDays(double.Parse(SalReturnRow["Column26"].ToString()));
                            fa.SelectedDateTime = SecDate;
                            fa.UpdateTextValue();
                        }
                        catch
                        {
                        }


                        foreach (GridEXRow item in gridEX1.GetRows())
                        {
                            string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());

                            if (item.Cells["Type"].Value.ToString() == "17" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0
                                || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                            {
                                //int DetialID= clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //     , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //     (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(), 
                                //     (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                //     (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                //        17, int.Parse(SalReturnRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);


                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,17," + int.Parse(SalReturnRow["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
              Class_BasicOperation._UserName + "',getdate(),0," + (SalReturnRow["Column26"] != null && !string.IsNullOrWhiteSpace(SalReturnRow["Column26"].ToString()) ? SalReturnRow["Column26"] : "0") + ",N'" + fa.Text + "'); SET @DetialID=SCOPE_IDENTITY()";


                                //اضافه کردن اقلام کالا به آرتیکل بدهکار فاکتور 
                                if (item.RowIndex == 1 && this.chk_Nots.Checked)
                                {
                                    foreach (DataRowView items in Child1Bind)
                                    {
                                        //clDoc.RunSqlCommand(ConAcnt.ConnectionString, "INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (" + DetialID + ",1,'" +
                                        //    clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02", "ColumnId", items["Column02"].ToString()) + "'," +
                                        //    items["Column07"].ToString() + "," + items["Column10"].ToString() + ")");

                                        CommandTxt += @"INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + items["Column02"].ToString() + " ) ," +
                                          items["Column07"].ToString() + "," + items["Column10"].ToString() + ")";


                                    }
                                }
                            }
                            else if (item.Cells["Type"].Value.ToString() == "27" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0
                                || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                            {

                                //clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                //    (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                //       27, ResidID, Class_BasicOperation._UserName, 0);


                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],column23,column24) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,27,@ResidID ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
               Class_BasicOperation._UserName + "',getdate(),0," + (SalReturnRow["Column26"] != null && !string.IsNullOrWhiteSpace(SalReturnRow["Column26"].ToString()) ? SalReturnRow["Column26"] : "0") + ",N'" + fa.Text + "');  ";


                            }
                            else if (item.Cells["Type"].Value.ToString() == "27" &&
                                (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 && Convert.ToDouble(item.Cells["Column12"].Value.ToString()) == 0))
                            {
                                //if (TotalValue > 0)
                                //{
                                //    clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //   , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //   (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(), Convert.ToInt64(Convert.ToDouble(value)),
                                //   (value > 0 ? 0 : Convert.ToInt64(Convert.ToDouble(TotalValue))), 0, 0, -1,
                                //      27, ResidID, Class_BasicOperation._UserName, 0);
                                //    value = 0;
                                //}
                                if (chk_Baha.Checked)
                                    CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column21),0) from " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt where Column01=@ResidID" + (item.Cells["Column09"].Text.Trim() == "" ? " and (Column14 is null or Column14='') " : " and Column14=" + item.Cells["Column09"].Value) + "),0)";
                                else
                                    CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column21),0) from " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt where Column01=@ResidID),0)";

                                CommandTxt += "set @value=@TotalValue   ";
                                if (Convert.ToInt16(item.Cells["Bed"].Value) == 1)
                                {
                                    CommandTxt += @" if @TotalValue>0 begin INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],column23,column24) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                               " + "'" + item.Cells["Column10"].Text.Trim() + @"',
                                @TotalValue , 0,0,0,-1,27,@ResidID ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                Class_BasicOperation._UserName + "',getdate(),0," + (SalReturnRow["Column26"] != null && !string.IsNullOrWhiteSpace(SalReturnRow["Column26"].ToString()) ? SalReturnRow["Column26"] : "0") + ",N'" + fa.Text + "'); set @value = 0 end ";

                                }
                                else
                                {
                                    CommandTxt += @" if @TotalValue>0 begin INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],column23,column24) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                               " + "'" + item.Cells["Column10"].Text.Trim() + @"',
                               0 ,  @TotalValue,0,0,-1,27,@ResidID ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                Class_BasicOperation._UserName + "',getdate(),0," + (SalReturnRow["Column26"] != null && !string.IsNullOrWhiteSpace(SalReturnRow["Column26"].ToString()) ? SalReturnRow["Column26"] : "0") + ",N'" + fa.Text + "'); set @value = 0 end ";
                                }
                            }

                        }



                        //if (ResidID == 0)
                        //    clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", int.Parse(SalReturnRow["Column09"].ToString()), DocID);
                        //else
                        //    clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidID, DocID);

                        //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", int.Parse(SalReturnRow["Column09"].ToString()), DocID);

                        CommandTxt += @"Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07= @Key  where ColumnId =@ResidID    ";
                        CommandTxt += @"Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10= @Key,Column15='" + Class_BasicOperation._UserName + "',Column16=getdate()  where ColumnId =" + int.Parse(SalReturnRow["ColumnId"].ToString());




                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = CommandTxt;
                                Command.Parameters.Add(DocNum);
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                if (_ResidID == 0)
                                    Class_BasicOperation.ShowMsg("", "سند حسابداری با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");
                                else Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                    "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + _ResidNum.ToString(), "Information");

                                bt_ExportDoc.Enabled = false;
                                this.DialogResult = DialogResult.Yes;
                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                this.Cursor = Cursors.Default;
                                Class_BasicOperation.CheckExceptionType(es, this.Name);
                            }

                            //محاسبه ارزش




                            this.Cursor = Cursors.Default;



                        }


                    }

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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
            else if (rb_toSerial.Checked && this.txt_serial.Text.Trim() != "")
            {
                clDoc.IsValidNumber(clDoc.DocNum(int.Parse(txt_serial.Text.Trim())));
                clDoc.IsFinal(clDoc.DocNum(int.Parse(txt_serial.Text.Trim())));
                txt_serial_Leave(sender, e);
            }

            if (Convert.ToDouble(gridEX1.GetTotalRow().Cells["Column11"].Value.ToString()) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");
            foreach (GridEXRow item in gridEX1.GetRows())
            {
                if (item.Cells["Column01"].Text.Trim() == "" || item.Cells["Column10"].Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");
                if (item.Cells["Column01"].Text.Trim().All(char.IsDigit))
                {
                    throw new Exception("سرفصل" + item.Cells["Column01"].Text + "نامعتبر است");

                }
            }

            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Length != 10)
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");

            if (uiTab2.Enabled && (mlt_Ware.Text.Trim() == "" || mlt_Function.Text.Trim() == ""))
                throw new Exception("اطلاعات مورد نیاز جهت صدور رسید انبار را کامل کنید");
            if (uiTab2.Enabled && (chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) <= 0))
                throw new Exception("اطلاعات مورد نیاز جهت صدور رسید انبار را کامل کنید");

            if (uiTab2.Enabled && (chk_DraftNum.Checked))
            {
                int ok = 0;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                {
                    Con.Open();
                    SqlCommand Select = new SqlCommand(@"IF EXISTS(
                                                       SELECT *
                                                       FROM   Table_011_PwhrsReceipt tpd
                                                       WHERE  tpd.column01 = " + txt_DraftNum.Value + @"
                                                   )
                                                    SELECT 0 AS ok 
                                                ELSE
                                                    SELECT 1 ok", Con);
                    ok = Convert.ToInt32(Select.ExecuteScalar().ToString());
                }
                if (ok == 0)
                    throw new Exception("این شماره رسید استفاده شده است");

            }


            if (SalReturnRow["Column12"].ToString() == "True" && Class_BasicOperation._FinType)
            {
                if (mlt_CurrencyType.Text.Trim() == "" || txt_CurrencyValue.Text.Trim() == "")
                    throw new Exception("اطلاعات مربوط به ارز را کامل کنید");
            }

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();


            if (uiTab2.Enabled)
            {
                foreach (DataRowView item in Child1Bind)
                {
                    if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item["Column02"].ToString())))
                        throw new Exception("کالای " +
                            clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02",
                            "ColumnId", item["Column02"].ToString()) + " در انبار انتخاب شده فعال نمی باشد");

                }
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

                clCredit.All_Controls_Row(item.Cells["Column01"].Value.ToString(), Person, Center, Project, item);

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

        private void Frm_009_ExportDocInformation_KeyDown(object sender, KeyEventArgs e)
        {
            if (bt_ExportDoc.Enabled && e.Control && e.KeyCode == Keys.S)
                bt_ExportDoc_Click(sender, e);
          
        }

        private void ExportResid()
        {
            if (clDoc.OperationalColumnValueSA("Table_018_MarjooiSale", "Column09", _SaleReturnID.ToString()) != 0)
                throw new Exception("برای این فاکتور رسید انبار صادر شده است");

            //درج هدر رسید
            //**Resid Header
            //, int.Parse(mlt_Ware.Value.ToString()));
            SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
            key.Direction = ParameterDirection.Output;
            using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                SqlCommand Insert;
                conware.Open();
                if (chk_DraftNum.Checked)
                    _ResidNum = Convert.ToInt32(txt_DraftNum.Value);
                else
                    _ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");

                Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          ) VALUES (" + _ResidNum + ",'" + SalReturnRow["Column02"].ToString() + "'," +
              mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + SalReturnRow["Column03"].ToString() + ",'" + "رسید صادر شده از فاکتور مرجوعی ش " +
              SalReturnRow["Column01"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
              "',getdate(),0,0," + SalReturnRow["ColumnId"].ToString() + ",0," +
              (SalReturnRow["Column12"].ToString().Trim() == "True" ? "1" : "0") + "," +
              (SalReturnRow["Column23"].ToString().Trim() == "" ? "NULL" : SalReturnRow["Column23"].ToString()) + "," +
              SalReturnRow["Column24"].ToString() + ",1,null); SET @Key=Scope_Identity()", conware);
                Insert.Parameters.Add(key);
                Insert.ExecuteNonQuery();
                _ResidID = int.Parse(key.Value.ToString());

                //درج شماره رسید در فاکتور مرجوعی فروش
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", int.Parse(SalReturnRow["ColumnId"].ToString()), _ResidID);


                //Resid Detail


                //اگر فاکتور مرجوعی فاقد شماره فاکتور فروش باشد
                //ارزش کالا به صورت آخرین ارزش حواله  بزرگتر از صفر در انبار درج می شود
                #region
                if (SalReturnRow["Column17"].ToString() == "0")
                {

                    foreach (DataRowView item in Child1Bind)
                    {
                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]
           ,[Column36]
,[Column37]) VALUES (" + _ResidID + "," + item["Column02"].ToString() + "," +
                         item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                         item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",N'" + item["Column23"].ToString() + "'," +
                         (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                         + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                         + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                         item["Column15"].ToString()
                         + ",0,0,0," +
                            (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                         item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ",'"+item["Column36"].ToString()+"','"+item["Column37"].ToString()+"')", conware);
                        InsertDetail.ExecuteNonQuery();
                    }

                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_012_Child_PwhrsReceipt where Column01=" + _ResidID, conware);
                    DataTable Table = new DataTable();
                    goodAdapter.Fill(Table);

                    foreach (DataRow item in Table.Rows)
                    {
                        if (Class_BasicOperation._WareType)
                        {
                            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), conware);
                            DataTable TurnTable = new DataTable();
                            Adapter.Fill(TurnTable);
                            TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0 and Date<='" + SalReturnRow["Column02"].ToString() + "'";
                            if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                            {
                                int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0 and Date<='" + SalReturnRow["Column02"].ToString() + "'").ToString());
                                DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                    + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                    Convert.ToDouble(item["Column07"].ToString())
                                    + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                UpdateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString() + ",@Date='" + SalReturnRow["Column02"].ToString() + "',@id=0,@residid="+_ResidID, conware);
                            DataTable TurnTable = new DataTable();
                            Adapter.Fill(TurnTable);
                            if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                            {
                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                        + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                UpdateCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + SalReturnRow["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + mlt_Ware.Value.ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                TurnTable = new DataTable();
                                Adapter.Fill(TurnTable);
                                if (TurnTable.Rows.Count > 0)
                                {
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                      + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                            }
                        }


                    }

                }
                #endregion



                //اگر فاکتور مرجوعی فروش دارای شماره فاکتور فروش بود
                // ارزش رسید بر اساس ارزش کالا در حواله مربوط به فاکتور فروش خوانده می شود
                #region
                else
                {
                    DataTable DraftChildTable = clDoc.ReturnTable(conware, "Select * from Table_008_Child_PwhrsDraft where Column01=" +
                        clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", SalReturnRow["Column17"].ToString()));
                    foreach (DataRowView item in Child1Bind)
                    {
                        DataRow[] FoundRows = DraftChildTable.Select("Column02=" + item["Column02"].ToString());
                        //اگر در حواله مذکور چنین کالایی موجود باشد از اولین کالا مقدار ارزش واحد خوانده می شود 
                        if (FoundRows.Length > 0)
                        {
                            Double SingleValue = Convert.ToDouble(FoundRows[0]["Column15"].ToString());

                            SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35],[Column36],[Column37]) VALUES (" + _ResidID + "," + item["Column02"].ToString() + "," +
                         item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                         item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                         (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                         + "',getdate(),0," + SingleValue + "," + Convert.ToDouble(item["Column07"].ToString()) * SingleValue
                         + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                         item["Column15"].ToString()
                         + ",0,0,0," +
                            (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                         item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ",'"+item["Column36"].ToString()+"','"+item["Column37"].ToString()+"')", conware);
                            InsertDetail.ExecuteNonQuery();
                        }
                        else
                        //اگر چنین کالایی در حواله نباشد ارزش بر اساس پروسجر محاسبه می شود
                        {
                            SqlParameter DetailKey = new SqlParameter("DetailKey", SqlDbType.Int);
                            DetailKey.Direction = ParameterDirection.Output;
                            SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]
           ,[Column36]
           ,[Column37]) VALUES (" + _ResidID + "," + item["Column02"].ToString() + "," +
                         item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                         item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",N'" + item["Column23"].ToString() + "'," +
                         (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                         + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                         + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                         item["Column15"].ToString()
                         + ",0,0,0," +
                            (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                         item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() +",'"+item["Column36"].ToString()+"','"+item["Column37"].ToString()+"'); SET @DetailKey=SCOPE_IDENTITY()", conware);
                            InsertDetail.Parameters.Add(DetailKey);
                            InsertDetail.ExecuteNonQuery();
                            int DetailId = int.Parse(DetailKey.Value.ToString());

                            if (Class_BasicOperation._WareType)
                            {
                                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), conware);
                                DataTable TurnTable = new DataTable();
                                Adapter.Fill(TurnTable);
                                TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0";
                                if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                {
                                    int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0").ToString());
                                    DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                        + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString())
                                        + " where ColumnId=" + DetailId, conware);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString() + ",@Date='" + SalReturnRow["Column02"].ToString() + "',@id=0,@residid=" + _ResidID, conware);
                                DataTable TurnTable = new DataTable();
                                Adapter.Fill(TurnTable);
                                if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                {
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                            + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + SalReturnRow["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + mlt_Ware.Value.ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                    TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    if (TurnTable.Rows.Count > 0)
                                    {
                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                          + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                        UpdateCommand.ExecuteNonQuery();
                                    }
                                }
                            }


                        }
                    }
                }
                #endregion

            }
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
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

        private void mlt_CurrencyType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {

                    if (item.Cells["Type"].Value.ToString() == "26")
                    {
                        item.BeginEdit();
                        item.Cells["Column13"].Value = txt_CurrencyValue.Value;
                        item.EndEdit();
                    }
                }
            }
            catch
            {
            }
        }

        private void chk_RegisterGoods_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RegisterReturnSaleFactorWithGoods = chk_RegisterGoods.Checked;
            Properties.Settings.Default.Save();
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
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

        private void gridEX1_CellEditCanceled(object sender, ColumnActionEventArgs e)
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

        private void AggDoc()
        {
            try
            {
                //تجمیع سطرها

                DataTable _1Table = SourceTable.DefaultView.ToTable("_1Table", true, new string[] { "Type", "Column01", "Column07", "Column08", "Column09", "Column13", "Column14", "Bed" });
                DataTable _2Table = SourceTable.Clone();
                foreach (DataRow item in _1Table.Rows)
                {
                    SourceTable.DefaultView.RowFilter = "Column01='" + item["Column01"].ToString() + "' and Type=" + item["Type"].ToString() +
                         (item["Column07"].ToString().Trim() != "" ? " and Column07=" + item["Column07"].ToString() : " and Column07 is null") +
                         (item["Column08"].ToString().Trim() != "" ? " and Column08=" + item["Column08"].ToString() : " and Column08 is null") +
                         (item["Column09"].ToString().Trim() != "" ? " and Column09=" + item["Column09"].ToString() : " and Column09 is null");

                    if (SourceTable.DefaultView.ToTable().Rows.Count > 0)
                    {
                        string Description = SourceTable.DefaultView.ToTable().Rows[0]["Column10"].ToString();
                        Double Bed = Convert.ToDouble(SourceTable.DefaultView.ToTable().Rows[0]["Column11"].ToString());
                        Double Bes = Convert.ToDouble(SourceTable.DefaultView.ToTable().Rows[0]["Column12"].ToString());
                        if (SourceTable.DefaultView.ToTable().Rows.Count > 1)
                        {
                            Description = "فاکتورمرجوعی فروش ش" + SalReturnRow["Column01"].ToString() + " به تاریخ " + SalReturnRow["Column02"].ToString();
                            Bed = Convert.ToDouble(SourceTable.DefaultView.ToTable().Compute("SUM(Column11)", "").ToString());
                            Bes = Convert.ToDouble(SourceTable.DefaultView.ToTable().Compute("SUM(Column12)", "").ToString());
                            if (Bed - Bes > 0)
                            {
                                Bed = Bed - Bes;
                                Bes = 0;
                            }
                            else
                            {
                                Bes = Math.Abs(Bed - Bes);
                                Bed = 0;
                            }
                        }

                        if (Bed - Bes != 0 && item["Type"].ToString() == "17")
                            _2Table.Rows.Add(item["Type"], item["Column01"].ToString(), item["Column01"].ToString(),
                                (item["Column07"].ToString().Trim() == "" ? null : item["Column07"].ToString()),
                                (item["Column08"].ToString().Trim() == "" ? null : item["Column08"].ToString()),
                                (item["Column09"].ToString().Trim() == "" ? null : item["Column09"].ToString()),
                                Description, Bed, Bes, Convert.ToDouble(item["Column13"].ToString()),
                                (item["Column14"].ToString().Trim() == "" ? (object)null : Convert.ToInt16(item["Column14"].ToString())));
                        else if (item["Type"].ToString() == "27")
                            _2Table.Rows.Add(item["Type"], item["Column01"].ToString(), item["Column01"].ToString(),
                         (item["Column07"].ToString().Trim() == "" ? null : item["Column07"].ToString()),
                         (item["Column08"].ToString().Trim() == "" ? null : item["Column08"].ToString()),
                         (item["Column09"].ToString().Trim() == "" ? null : item["Column09"].ToString()),
                         Description, Bed, Bes, Convert.ToDouble(item["Column13"].ToString()),
                         (item["Column14"].ToString().Trim() == "" ? (object)null : Convert.ToInt16(item["Column14"].ToString())), item["Bed"]);

                    }

                }
                SourceTable.DefaultView.RowFilter = "";
                gridEX1.DataSource = _2Table;

            }
            catch { }
        }

        private void chk_AggDoc_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();
        }

        private void chk_Baha_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Baha.Checked)
            {
                chk_PCBed.Checked = true;
                chk_PCBes.Checked = true;

                chk_PCBed.Enabled = true;
                chk_PCBes.Enabled = true;
            }
            else
            {
                chk_PCBed.Enabled = false;
                chk_PCBes.Enabled = false;

                chk_PCBed.Checked = false;
                chk_PCBes.Checked = false;
            }
            PrepareDoc();

        }

        private void chk_Net_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void uiRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void chk_Nots_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void chk_without_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void chk_PCBed_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_PCBed.Checked && !chk_PCBes.Checked)
            {
                chk_Baha.Checked = false;
                chk_PCBed.Enabled = false;
                chk_PCBes.Enabled = false;
            }
            PrepareDoc();
        }

        private void chk_PCBes_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_PCBed.Checked && !chk_PCBes.Checked)
            {
                chk_Baha.Checked = false;
                chk_PCBed.Enabled = false;
                chk_PCBes.Enabled = false;
            }
            PrepareDoc();
        }

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DraftNum.Checked)
                txt_DraftNum.Enabled = true;
            else
                txt_DraftNum.Enabled = false;
        }

        private void chk_GoodACCNum_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void rb_toSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_toSerial.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_LastNum.Text = null;
                this.txt_serial.Text = string.Empty;
                txt_Cover.Text = string.Empty;
                txt_Cover.Enabled = false;
                txt_serial.Enabled = true;
                txt_To.Enabled = false;
                txt_To.Text = null;
                txt_serial.Select();
            }
            else
            {
                this.txt_serial.Text = null;

                faDatePicker1.Enabled = true;
            }
        }

        private void txt_serial_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.txt_serial.Text.Trim() != "")
                {
                    clDoc.IsValidNumberS(int.Parse(txt_serial.Text.Trim()));
                    faDatePicker1.Text = clDoc.DocDateS(int.Parse(txt_serial.Text.Trim()));
                    txt_Cover.Text = clDoc.CoverS(int.Parse(txt_serial.Text.Trim()));

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void txt_serial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }
    }
}
