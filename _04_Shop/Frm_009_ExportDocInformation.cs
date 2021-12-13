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

namespace PCLOR._04_Shop
{
    public partial class Frm_009_ExportDocInformation : Form
    {
        bool _Tab1 = false;
        int _Tab2 = 0;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        DataTable setting = new DataTable();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        int _BuyID, DocNum = 0, DocID = 0, ResidID = 0, ResidNum = 0;
        SqlDataAdapter BuyAdapter, Child2Adapter, Child1Adapter;
        BindingSource BuyBind, Child1Bind, Child2Bind;
        DataSet DS = new DataSet();
        DataRowView BuyRow;
        DataTable SourceTable = new DataTable();
        Int16 _ware, _Func;
        string CommandTxt = string.Empty;
        public Frm_009_ExportDocInformation(bool Tab1, int Tab2, int BuyID, Int16 ware, Int16 func)
        {
            InitializeComponent();
            _Tab1 = Tab1;
            _Tab2 = Tab2;
            _BuyID = BuyID;
            _ware = ware;
            _Func = func;
        }

        private void Frm_009_ExportDocInformation_Load(object sender, EventArgs e)
        {
            try
            {
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
                gridEX1.DataSource = SourceTable;
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * from AllHeaders()", ConAcnt);
                Adapter.Fill(DS, "Header");
                gridEX1.DropDowns["Header_Code"].SetDataBinding(DS.Tables["Header"], "");
                gridEX1.DropDowns["Header_Name"].SetDataBinding(DS.Tables["Header"], "");
                mlt_LinAddBed.DataSource = DS.Tables["Header"];
                mlt_LinAddBes.DataSource = DS.Tables["Header"];
                mlt_LinDisBed.DataSource = DS.Tables["Header"];
                mlt_LinDisBes.DataSource = DS.Tables["Header"];
                mlt_BuyBed.DataSource = DS.Tables["Header"];
                mlt_BuyBes.DataSource = DS.Tables["Header"];
                mlt_ValueBed.DataSource = DS.Tables["Header"];
                mlt_ValueBes.DataSource = DS.Tables["Header"];

                Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')", ConWare);
                Adapter.Fill(DS, "Ware");
                mlt_Ware.DataSource = DS.Tables["Ware"];

                Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=0", ConWare);
                Adapter.Fill(DS, "Fun");
                mlt_Function.DataSource = DS.Tables["Fun"];

                //*********************
                BuyAdapter = new SqlDataAdapter("Select * from Table_015_BuyFactor where ColumnId=" + _BuyID, ConSale);
                BuyAdapter.Fill(DS, "Buy");
                BuyBind = new BindingSource();
                BuyBind.DataSource = DS.Tables["Buy"];
                BuyRow = (DataRowView)this.BuyBind.CurrencyManager.Current;

                Child2Adapter = new SqlDataAdapter("Select * from Table_017_Child2_BuyFactor  where Column01=" + _BuyID, ConSale);
                Child2Adapter.Fill(DS, "Child2");
                Child2Bind = new BindingSource();
                Child2Bind.DataSource = DS.Tables["Child2"];

                Child1Adapter = new SqlDataAdapter("Select *,CAST(0 as decimal(18, 4)) as UnitValue,CAST(0 as decimal(18, 4)) as TotalValue from Table_016_Child1_BuyFactor where Column01=" + _BuyID, ConSale);
                Child1Adapter.Fill(DS, "Child1");
                Child1Bind = new BindingSource();
                Child1Bind.DataSource = DS.Tables["Child1"];

                //Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
                //DataTable Person = new DataTable();
                //Adapter.Fill(Person);
                gridEX1.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");

                Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
                DataTable Center = new DataTable();
                Adapter.Fill(Center);
                gridEX1.DropDowns["Center"].SetDataBinding(Center, "");

                Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
                DataTable Project = new DataTable();
                Adapter.Fill(Project);
                gridEX1.DropDowns["Project"].SetDataBinding(Project, "");


                if (BuyRow["Column15"].ToString() == "False")
                {
                    mlt_BuyBed.Value = clDoc.Account(9, "Column07");
                    mlt_BuyBes.Value = clDoc.Account(9, "Column13");
                    mlt_LinAddBed.Value = clDoc.Account(11, "Column07");
                    mlt_LinAddBes.Value = clDoc.Account(11, "Column13");
                    mlt_LinDisBed.Value = clDoc.Account(10, "Column07");
                    mlt_LinDisBes.Value = clDoc.Account(10, "Column13");
                }
                else
                {
                    mlt_BuyBed.Value = clDoc.Account(22, "Column07");
                    mlt_BuyBes.Value = clDoc.Account(22, "Column13");
                    mlt_LinAddBed.Value = clDoc.Account(24, "Column07");
                    mlt_LinAddBes.Value = clDoc.Account(24, "Column13");
                    mlt_LinDisBed.Value = clDoc.Account(23, "Column07");
                    mlt_LinDisBes.Value = clDoc.Account(23, "Column13");
                }


                faDatePicker1.Text = BuyRow["Column02"].ToString();

                uiTab1.Enabled = _Tab1;

                if (_Tab2 == 0)
                    uiTab2.Enabled = true;
                else
                    uiTab2.Enabled = false;

                gridEX1.MoveFirst();
                Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConMain);
                Adapter.Fill(setting);
                try
                {
                    chk_RegisterGoods.Checked = Properties.Settings.Default.RegisterBuyFactorWithGoods;
                    chk_Nots.Checked = Properties.Settings.Default.RegisterBuyFactorNoteGoods;
                    chk_Baha.Checked = Properties.Settings.Default.BuyBaha;
                    chk_GoodACCNum.Checked = Properties.Settings.Default.BuyGoodACCNum;
                }
                catch
                {
                }
                if (Properties.Settings.Default.PCBes)
                    chk_PCBes.Checked = true;
                else
                    chk_PCBes.Checked = false;
                if (Properties.Settings.Default.PCBed)

                    chk_PCBed.Checked = true;
                else
                    chk_PCBed.Checked = false;
                chk_AggDoc.Checked = Properties.Settings.Default.BuyAggregationSaleDoc;
                chk_Net.Checked = Properties.Settings.Default.chk_ByuNet;
                if (!chk_Nots.Checked && !chk_RegisterGoods.Checked)
                    chk_without.Checked = true;
                PrepareDoc();
                mlt_Function.Value = _Func;
                mlt_Ware.Value = _ware;
                chk_DraftNum.Checked = false;
                txt_DraftNum.Enabled = false;
            }
            catch { }
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
                txt_Cover.Text = "فاکتور خرید";
                faDatePicker1.Text = BuyRow["Column02"].ToString();
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
                txt_serial.Enabled = false;
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

        private void bt_ViewDocs_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form01_AccDocument")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = (DocNum != 0 ? DocNum.ToString() : "1");
                        PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(DocID.ToString()));
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewDrafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PWHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewWareReceipt")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt();
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void PrepareDoc()
        {
            #region تفکیک پروژه


            SqlDataAdapter Adapter;
            if (chk_Baha.Checked)
            {
                SourceTable.Rows.Clear();

                Adapter = new SqlDataAdapter(@" SELECT * FROM  [Table_016_Child1_BuyFactor]
                                                    WHERE column01=  " + BuyRow["ColumnId"].ToString() + " AND [column22] is NOT NULL", ConSale);
                DataTable Project = new DataTable();
                Adapter.Fill(Project);
                if (Project.Rows.Count > 0)
                {
                    #region کالاها پروژه دارند

                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT    Center, Project, column01, Column20 AS Net, TotalValue,Total,column50
                             FROM         (SELECT     dbo.Table_016_Child1_BuyFactor.Column21 as Center,dbo.Table_016_Child1_BuyFactor.column22 AS Project, ISNULL(SUM(dbo.Table_016_Child1_BuyFactor.column20), 0) AS Column20, dbo.Table_016_Child1_BuyFactor.column01,
                               ISNULL(SUM(dbo.Table_016_Child1_BuyFactor.column11), 0) AS Total,ISNULL(Sum(dbo.Table_016_Child1_BuyFactor.column07),0) as TotalValue,tcai.column50
                             FROM          dbo.Table_016_Child1_BuyFactor
                                            JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_016_Child1_BuyFactor.column02
                             GROUP BY dbo.Table_016_Child1_BuyFactor.column21,dbo.Table_016_Child1_BuyFactor.column22, dbo.Table_016_Child1_BuyFactor.column01 ,tcai.column50
                             HAVING      (dbo.Table_016_Child1_BuyFactor.column01 = {0})) AS derivedtbl_1", ConSale);
                    else
                        Adapter = new SqlDataAdapter(@"SELECT    Center, Project, column01, Column20 AS Net, TotalValue,Total,column50
                             FROM         (SELECT     Column21 as Center,column22 AS Project, ISNULL(SUM(column20), 0) AS Column20, column01,
                               ISNULL(SUM(column11), 0) AS Total,ISNULL(Sum(column07),0) as TotalValue,null as column50
                             FROM          dbo.Table_016_Child1_BuyFactor
                                            
                             GROUP BY column21,column22, column01  
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, BuyRow["ColumnId"].ToString());
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);

                    //  فاکتور خرید با احتساب تخفیفات و اضافات خطی



                    DataTable detali = new DataTable();
                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02,tcsf.column22,tcsf.column21,tcai.column50,tcsf.column23 as [Desc]
                                            FROM   Table_016_Child1_BuyFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + BuyRow["ColumnId"].ToString() + "", ConSale);
                    else

                        Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02,tcsf.column22,tcsf.column21, null as column50,tcsf.column23 as [Desc]
                                            FROM   Table_016_Child1_BuyFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + BuyRow["ColumnId"].ToString() + "", ConSale);
                    Adapter.Fill(detali);
                    if (chk_RegisterGoods.Checked)//ریز اقلام
                    {
                        foreach (DataRow dt in detali.Rows)
                        {

                            string sharhjoz = string.Empty;
                            sharhjoz = " کالای  " + dt["column02"].ToString();
                            if (Convert.ToBoolean(setting.Rows[31]["Column02"]))
                                sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                            if (Convert.ToBoolean(setting.Rows[33]["Column02"]))
                                sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                            if (Convert.ToBoolean(setting.Rows[61]["Column02"]) &&
                             dt["Desc"] != DBNull.Value &&
                             dt["Desc"] != null &&
                             !string.IsNullOrWhiteSpace(dt["Desc"].ToString()))
                                sharhjoz += " توضیح  " + dt["Desc"].ToString();

                            if (!chk_Net.Checked)
                            {

                                //********Bed

                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19,
                                         ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                         ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null,
                                        ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                         ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                                         CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                        Convert.ToDouble(dt["column11"])
                                        , 0, 0, -1);
                                else
                                    SourceTable.Rows.Add(19,
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                         ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                   null,
                                   null,
                                  null,
                                    CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                   Convert.ToDouble(dt["column11"])
                                   , 0, 0, -1);



                                //********Bes

                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                   (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                   BuyRow["Column03"].ToString(),
                                  ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                   ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                                   CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                   0
                                   , Convert.ToDouble(dt["column11"]), 0, -1);

                                else

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                             (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                             BuyRow["Column03"].ToString(),
                              null,
                               null,
                             CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                             0
                             , Convert.ToDouble(dt["column11"]), 0, -1);

                            }
                            else
                            {

                                //********Bed

                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19,
                                        ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                         ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null,
                                        ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                         ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                                                         CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,

                                        Convert.ToDouble(dt["Net"])
                                        , 0, 0, -1);

                                else
                                    SourceTable.Rows.Add(19,
                                      ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                         ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                    null,
                                    null,
                                    null,
                                    CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                     Convert.ToDouble(dt["Net"])
                                     , 0, 0, -1);
                                //********Bes

                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                 ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                  ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                                  CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                  0
                                  , Convert.ToDouble(dt["Net"]), 0, -1);
                                else

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                  (null),
                                  (null),
                                  CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                  0
                                  , Convert.ToDouble(dt["Net"]), 0, -1);


                            }

                        }
                    }
                    else
                    {
                        foreach (DataRow item in Table.Rows)
                        {
                            if (!chk_Net.Checked)
                            {
                                //*********Bed
                                if (chk_PCBed.Checked)
                                    SourceTable.Rows.Add(19,
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null, (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                        (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                       CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                       Convert.ToDouble(item["Total"].ToString()),
                                       0, 0, -1
                                       );
                                else

                                    SourceTable.Rows.Add(19,
                                          ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                null, (null),
                                (null),
                               CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                               Convert.ToDouble(item["Total"].ToString()),
                               0, 0, -1
                               );

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                          (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null), BuyRow["Column03"].ToString(),
                                         (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                        (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                          CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                          0
                                          , Convert.ToDouble(item["Total"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                    (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null), BuyRow["Column03"].ToString(),
                                   (null),
                                   (null),
                                    CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                    0
                                    , Convert.ToDouble(item["Total"].ToString()), 0, -1);
                            }
                            else
                            {
                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19,
                                         ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null,
                                        (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                        (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                       CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                       Convert.ToDouble(item["Net"].ToString()),
                                       0, 0, -1
                                       );
                                else
                                    SourceTable.Rows.Add(19,
                                      ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                   null,
                                   (null),
                                   (null),
                                  CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                  Convert.ToDouble(item["Net"].ToString()),
                                  0, 0, -1
                                  );
                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                      (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                      BuyRow["Column03"].ToString(),
                                     (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                    (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                      CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                      0
                                      , Convert.ToDouble(item["Net"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                 (null),
                                 (null),
                                  CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                  0
                                  , Convert.ToDouble(item["Net"].ToString()), 0, -1);

                            }


                        }
                    }

                    // مربوط به اضافات خطی فاکتور
                    if (Convert.ToDouble(BuyRow["Column23"].ToString()) != 0 && !chk_Net.Checked)
                    {


                        DataTable ezafeTable = clDoc.ReturnTable(ConSale,
                   "Select SUM(column19) as ezafe, column22,column21 from Table_016_Child1_BuyFactor where column01=" + BuyRow["ColumnId"].ToString() + "  group by column22,column21");
                        foreach (DataRow d in ezafeTable.Rows)
                        {
                            if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                        (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                        null,
                                        ((d["column21"] != DBNull.Value && d["column21"] != null && !string.IsNullOrWhiteSpace(d["column21"].ToString())) ? d["column21"].ToString() : null),
                                        ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                        (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ") +
                                        BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                        Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                 (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                 null,
                                 (null),
                                 (null),
                                 (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ") +
                                 BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                 Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                        (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                        BuyRow["Column03"].ToString(),
                                        ((d["column21"] != DBNull.Value && d["column21"] != null && !string.IsNullOrWhiteSpace(d["column21"].ToString())) ? d["column21"].ToString() : null),
                                        ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                        (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ")
                                        + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                        0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                                else

                                    SourceTable.Rows.Add(19, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                   (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                   BuyRow["Column03"].ToString(),
                                   (null),
                                   (null),
                                   (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ")
                                   + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                   0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);

                            }
                        }



                    }

                    //ثبت مربوط به تخفیفات خطی فاکتور
                    if (Convert.ToDouble(BuyRow["Column24"].ToString()) > 0 && !chk_Net.Checked)
                    {
                        DataTable takhfifTable = clDoc.ReturnTable(ConSale,
                            "Select SUM(column17) as takhfif, column22,column21 from Table_016_Child1_BuyFactor where column01=" + BuyRow["ColumnId"].ToString() + "  group by column22,column21");


                        foreach (DataRow h in takhfifTable.Rows)
                        {

                            if (Convert.ToInt64(h["takhfif"]) > 0)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                        (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                        BuyRow["Column03"].ToString(),
                                        ((h["column21"] != DBNull.Value && h["column21"] != null && !string.IsNullOrWhiteSpace(h["column21"].ToString())) ? h["column21"].ToString() : null),
                                        ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null),
                                        "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                                else

                                    SourceTable.Rows.Add(19, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                 (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                 BuyRow["Column03"].ToString(),
                                 (null),
                                 (null),
                                 "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                        (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                        null,
                                        ((h["column21"] != DBNull.Value && h["column21"] != null && !string.IsNullOrWhiteSpace(h["column21"].ToString())) ? h["column21"].ToString() : null),
                                        ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                    (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                    null,
                                    (null),
                                    (null), "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                            }
                        }


                    }


                    //سایر اضافات و کسورات
                    foreach (DataRowView item in Child2Bind)
                    {
                        string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column10", "ColumnId", item["Column02"].ToString());
                        string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column16", "ColumnId", item["Column02"].ToString());
                        string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column01", "ColumnId", item["Column02"].ToString());

                        //********Bed
                        SourceTable.Rows.Add(19, Bed, Bed, (item["Column05"].ToString() == "True" ? BuyRow["Column03"].ToString() : null), null, null, Name +
                            " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                           Convert.ToDouble(item["Column04"].ToString()), 0, (BuyRow["Column26"].ToString().Trim() == "" ? "0" : BuyRow["Column26"].ToString()),
                            (BuyRow["Column25"].ToString().Trim() == "" ? "-1" : BuyRow["Column25"].ToString()));
                        //*********Bes
                        SourceTable.Rows.Add(19, Bes, Bes, (item["Column05"].ToString() == "False" ? BuyRow["Column03"].ToString() : null), null, null,
                            Name + " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0,
                           Convert.ToDouble(item["Column04"].ToString()), (BuyRow["Column26"].ToString().Trim() == "" ? "0" : BuyRow["Column26"].ToString()),
                            (BuyRow["Column25"].ToString().Trim() == "" ? "-1" : BuyRow["Column25"].ToString()));
                    }
                    #endregion
                }

                #region کالاها پروژه ندارند
                else
                {

                    Adapter = new SqlDataAdapter(@" SELECT Column29 FROM  Table_015_BuyFactor
                                                    WHERE columnid=  " + BuyRow["ColumnId"].ToString(), ConSale);
                    Project = new DataTable();
                    Adapter.Fill(Project);
                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT    Center , column01, Column20 AS Net, TotalValue,Total,column50
                             FROM         (SELECT     dbo.Table_016_Child1_BuyFactor.Column21 as Center , ISNULL(SUM(dbo.Table_016_Child1_BuyFactor.column20), 0) AS Column20, dbo.Table_016_Child1_BuyFactor.column01,
                               ISNULL(SUM(dbo.Table_016_Child1_BuyFactor.column11), 0) AS Total,ISNULL(Sum(dbo.Table_016_Child1_BuyFactor.Column07),0) as TotalValue,tcai.column50
                             FROM          dbo.Table_016_Child1_BuyFactor
                                JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_016_Child1_BuyFactor.column02 
                             GROUP BY dbo.Table_016_Child1_BuyFactor.column21,  dbo.Table_016_Child1_BuyFactor.column01 ,tcai.column50
                             HAVING      (dbo.Table_016_Child1_BuyFactor.column01 = {0})) AS derivedtbl_1", ConSale);
                    else
                        Adapter = new SqlDataAdapter(@"SELECT    Center , column01, Column20 AS Net, TotalValue,Total,column50
                             FROM         (SELECT     Column21 as Center , ISNULL(SUM(column20), 0) AS Column20, column01,
                               ISNULL(SUM(column11), 0) AS Total,ISNULL(Sum(column07),0) as TotalValue,null  as column50
                             FROM          dbo.Table_016_Child1_BuyFactor
                             GROUP BY column21,  column01  
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, BuyRow["ColumnId"].ToString());
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);

                    //  فاکتور خرید با احتساب تخفیفات و اضافات خطی



                    DataTable detali = new DataTable();
                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02,tcsf.column21,tcai.column50,tcsf.column23 as [Desc]
                                            FROM   Table_016_Child1_BuyFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + BuyRow["ColumnId"].ToString() + "", ConSale);
                    else
                        Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02,tcsf.column21,null as column50,tcsf.column23 as [Desc]
                                            FROM   Table_016_Child1_BuyFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + BuyRow["ColumnId"].ToString() + "", ConSale);
                    Adapter.Fill(detali);
                    if (chk_RegisterGoods.Checked)//ریز اقلام
                    {
                        foreach (DataRow dt in detali.Rows)
                        {

                            string sharhjoz = string.Empty;
                            sharhjoz = " کالای  " + dt["column02"].ToString();
                            if (Convert.ToBoolean(setting.Rows[31]["Column02"]))
                                sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                            if (Convert.ToBoolean(setting.Rows[33]["Column02"]))
                                sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                            if (Convert.ToBoolean(setting.Rows[61]["Column02"]) &&
                             dt["Desc"] != DBNull.Value &&
                             dt["Desc"] != null &&
                             !string.IsNullOrWhiteSpace(dt["Desc"].ToString()))
                                sharhjoz += " توضیح  " + dt["Desc"].ToString();
                            if (!chk_Net.Checked)
                            {

                                //********Bed
                                if (chk_PCBed.Checked)
                                    SourceTable.Rows.Add(19,
                                        ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null,
                                        ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                                         CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                        Convert.ToDouble(dt["column11"])
                                        , 0, 0, -1);
                                else

                                    SourceTable.Rows.Add(19,
                                      ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                       null,
                                       (null),
                                        (null),
                                        CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                       Convert.ToDouble(dt["column11"])
                                       , 0, 0, -1);
                                //********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                    (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                    BuyRow["Column03"].ToString(),
                                   ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                    ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                                    CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                    0
                                    , Convert.ToDouble(dt["column11"]), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                     (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                     BuyRow["Column03"].ToString(),
                                    (null),
                                     (null),
                                     CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                     0
                                     , Convert.ToDouble(dt["column11"]), 0, -1);


                            }
                            else
                            {
                                //********Bed
                                if (chk_PCBed.Checked)
                                    SourceTable.Rows.Add(19,
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                       null,
                                       ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                       CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                       Convert.ToDouble(dt["Net"])
                                       , 0, 0, -1);

                                else
                                    SourceTable.Rows.Add(19,
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                  null,
                                  (null),
                                    (null),

                                  CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                  Convert.ToDouble(dt["Net"])
                                  , 0, 0, -1);
                                //********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                   (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                   BuyRow["Column03"].ToString(),
                                  ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                  ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                                   CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                   0
                                   , Convert.ToDouble(dt["Net"]), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                 (null),
                                 (null),
                                  CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                                  0
                                  , Convert.ToDouble(dt["Net"]), 0, -1);

                            }

                        }
                    }
                    else
                    {
                        foreach (DataRow item in Table.Rows)
                        {
                            if (!chk_Net.Checked)
                            {
                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19,
                                        ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null, (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                        ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                                       CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                       Convert.ToDouble(item["Total"].ToString()),
                                       0, 0, -1
                                       );
                                else
                                    SourceTable.Rows.Add(19,
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                   null, null,
                                   (null),
                                  CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                  Convert.ToDouble(item["Total"].ToString()),
                                  0, 0, -1
                                  );

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                          (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null), BuyRow["Column03"].ToString(),
                                         (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                          CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                          0
                                          , Convert.ToDouble(item["Total"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                    (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null), BuyRow["Column03"].ToString(),
                                   null,
                                   (null),
                                    CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                    0
                                    , Convert.ToDouble(item["Total"].ToString()), 0, -1);
                            }
                            else
                            {
                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19,
                                        ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                        null,
                                        (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                       CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                       Convert.ToDouble(item["Net"].ToString()),
                                       0
                                       , 0, -1);
                                else
                                    SourceTable.Rows.Add(19,
                                        ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                       ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                null,
                                null,
                                null,
                               CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                               Convert.ToDouble(item["Net"].ToString()),
                               0
                               , 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                          (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                          BuyRow["Column03"].ToString(),
                                         (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                          CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                          0
                                          , Convert.ToDouble(item["Net"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                   (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                   BuyRow["Column03"].ToString(),
                                   null,
                                   null,
                                   CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                   0
                                   , Convert.ToDouble(item["Net"].ToString()), 0, -1);

                            }


                        }
                    }

                    // مربوط به اضافات خطی فاکتور
                    if (Convert.ToDouble(BuyRow["Column23"].ToString()) != 0 && !chk_Net.Checked)
                    {


                        DataTable ezafeTable = clDoc.ReturnTable(ConSale,
                   "Select SUM(column19) as ezafe,  column21 from Table_016_Child1_BuyFactor where column01=" + BuyRow["ColumnId"].ToString() + "  group by  column21");
                        foreach (DataRow d in ezafeTable.Rows)
                        {
                            if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                        (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                        null,
                                        ((d["column21"] != DBNull.Value && d["column21"] != null && !string.IsNullOrWhiteSpace(d["column21"].ToString())) ? d["column21"].ToString() : null),
                                                                        ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                        (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ") +
                                        BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                        Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                else

                                    SourceTable.Rows.Add(19, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                             (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                             null,
                             null,
                             null,
                             (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ") +
                             BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                             Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    BuyRow["Column03"].ToString(),
                                    ((d["column21"] != DBNull.Value && d["column21"] != null && !string.IsNullOrWhiteSpace(d["column21"].ToString())) ? d["column21"].ToString() : null),
                                                                   ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                    (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ")
                                    + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                    0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                  (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                  null,
                                  null,
                                  (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ")
                                  + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                  0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);

                            }
                        }



                    }

                    //ثبت مربوط به تخفیفات خطی فاکتور
                    if (Convert.ToDouble(BuyRow["Column24"].ToString()) > 0 && !chk_Net.Checked)
                    {
                        DataTable takhfifTable = clDoc.ReturnTable(ConSale,
                            "Select SUM(column17) as takhfif,  column21 from Table_016_Child1_BuyFactor where column01=" + BuyRow["ColumnId"].ToString() + "  group by  column21");


                        foreach (DataRow h in takhfifTable.Rows)
                        {

                            if (Convert.ToInt64(h["takhfif"]) > 0)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                        (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                        BuyRow["Column03"].ToString(),
                                        ((h["column21"] != DBNull.Value && h["column21"] != null && !string.IsNullOrWhiteSpace(h["column21"].ToString())) ? h["column21"].ToString() : null),
                                                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),

                                        "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                   (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                   BuyRow["Column03"].ToString(),
                                   null,
                                   null,
                                   "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(19, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                        (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                        null,
                                        ((h["column21"] != DBNull.Value && h["column21"] != null && !string.IsNullOrWhiteSpace(h["column21"].ToString())) ? h["column21"].ToString() : null),
                                         ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                                        "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(19, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                null,
                                null,
                                null,
                                "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);

                            }
                        }


                    }


                    //سایر اضافات و کسورات
                    foreach (DataRowView item in Child2Bind)
                    {
                        string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column10", "ColumnId", item["Column02"].ToString());
                        string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column16", "ColumnId", item["Column02"].ToString());
                        string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column01", "ColumnId", item["Column02"].ToString());

                        //********Bed
                        SourceTable.Rows.Add(19, Bed, Bed, (item["Column05"].ToString() == "True" ? BuyRow["Column03"].ToString() : null),
                            null,
                            ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                            Name +
                            " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                           Convert.ToDouble(item["Column04"].ToString()), 0, (BuyRow["Column26"].ToString().Trim() == "" ? "0" : BuyRow["Column26"].ToString()),
                            (BuyRow["Column25"].ToString().Trim() == "" ? "-1" : BuyRow["Column25"].ToString()));
                        //*********Bes
                        SourceTable.Rows.Add(19, Bes, Bes, (item["Column05"].ToString() == "False" ? BuyRow["Column03"].ToString() : null),
                            null,
                            ((Project.Rows.Count > 0 && Project.Rows[0]["Column29"] != DBNull.Value && Project.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column29"].ToString())) ? Project.Rows[0]["Column29"] : null),
                            Name + " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0,
                           Convert.ToDouble(item["Column04"].ToString()), (BuyRow["Column26"].ToString().Trim() == "" ? "0" : BuyRow["Column26"].ToString()),
                            (BuyRow["Column25"].ToString().Trim() == "" ? "-1" : BuyRow["Column25"].ToString()));
                    }
                }
            }
                #endregion
            #endregion

            #region عدم تفکیک پروژه
            else
            {
                SourceTable.Rows.Clear();

                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT      column01, Column20 AS Net, TotalValue,Total,column50
                             FROM         (SELECT       ISNULL(SUM(dbo.Table_016_Child1_BuyFactor.column20), 0) AS Column20,dbo.Table_016_Child1_BuyFactor. column01,
                               ISNULL(SUM(dbo.Table_016_Child1_BuyFactor.column11), 0) AS Total,ISNULL(Sum(dbo.Table_016_Child1_BuyFactor.column07),0) as TotalValue,tcai.column50
                             FROM          dbo.Table_016_Child1_BuyFactor
                            JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_016_Child1_BuyFactor.column02 
                             GROUP BY    dbo.Table_016_Child1_BuyFactor.column01 ,tcai.column50
                             HAVING      (dbo.Table_016_Child1_BuyFactor.column01 = {0})) AS derivedtbl_1", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT      column01, Column20 AS Net, TotalValue,Total,column50
                             FROM         (SELECT       ISNULL(SUM(column20), 0) AS Column20, column01,
                               ISNULL(SUM(column11), 0) AS Total,ISNULL(Sum(column07),0) as TotalValue,null as column50
                             FROM          dbo.Table_016_Child1_BuyFactor
                           
                             GROUP BY    column01  
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, BuyRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                //  فاکتور خرید با احتساب تخفیفات و اضافات خطی



                DataTable detali = new DataTable();
                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02 ,tcai.column50,tcsf.column23 as [Desc]
                                            FROM   Table_016_Child1_BuyFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + BuyRow["ColumnId"].ToString() + "", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02 ,null as column50,tcsf.column23 as [Desc]
                                            FROM   Table_016_Child1_BuyFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + BuyRow["ColumnId"].ToString() + "", ConSale);
                Adapter.Fill(detali);
                if (chk_RegisterGoods.Checked)//ریز اقلام
                {
                    foreach (DataRow dt in detali.Rows)
                    {

                        string sharhjoz = string.Empty;
                        sharhjoz = " کالای  " + dt["column02"].ToString();
                        if (Convert.ToBoolean(setting.Rows[31]["Column02"]))
                            sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                        if (Convert.ToBoolean(setting.Rows[33]["Column02"]))
                            sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                        if (Convert.ToBoolean(setting.Rows[61]["Column02"]) &&
                             dt["Desc"] != DBNull.Value &&
                             dt["Desc"] != null &&
                             !string.IsNullOrWhiteSpace(dt["Desc"].ToString()))
                            sharhjoz += " توضیح  " + dt["Desc"].ToString();
                        if (!chk_Net.Checked)
                        {
                            //********Bed

                            SourceTable.Rows.Add(19,
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                              null,
                             null,
                              null,
                              CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                              Convert.ToDouble(dt["column11"])
                              , 0, 0, -1);

                            //********Bes


                            SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                            (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                            BuyRow["Column03"].ToString(),
                           null,
                            null,
                            CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                            0
                            , Convert.ToDouble(dt["column11"]), 0, -1);



                        }
                        else
                        {

                            //********Bed

                            SourceTable.Rows.Add(19,
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                             CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,

                                Convert.ToDouble(dt["Net"])
                                , 0, 0, -1);


                            //********Bes

                            SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                           (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                           BuyRow["Column03"].ToString(),
                          null,
                         null,
                           CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())) + sharhjoz,
                           0
                           , Convert.ToDouble(dt["Net"]), 0, -1);


                        }

                    }
                }
                else
                {
                    foreach (DataRow item in Table.Rows)
                    {
                        if (!chk_Net.Checked)
                        {
                            //*********Bed
                            SourceTable.Rows.Add(19,
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                null,
                                null,
                               null,
                               CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                               Convert.ToDouble(item["Total"].ToString()),
                               0
                               , 0, -1);


                            //*********Bes
                            SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                   null,
                                   null,
                                   CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                   0,
                                   Convert.ToDouble(item["Total"].ToString()), 0, -1);
                        }
                        else
                        {
                            //*********Bed
                            SourceTable.Rows.Add(19,
                                 ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_BuyBed.Text.Trim() != "" ? mlt_BuyBed.Value.ToString() : null)),

                                null,
                                null,
                               null,

                               CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                               Convert.ToDouble(item["Net"].ToString()),
                               0
                               , 0, -1);


                            //*********Bes
                            SourceTable.Rows.Add(19, (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  (mlt_BuyBes.Text.Trim() != "" ? mlt_BuyBes.Value.ToString() : null),
                                  BuyRow["Column03"].ToString(),
                                 null,
                                 null,

                                  CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(BuyRow["Column20"].ToString()), Convert.ToDouble(BuyRow["Column20"].ToString())),
                                  0
                                  , Convert.ToDouble(item["Net"].ToString()), 0, -1);
                        }


                    }
                }

                // مربوط به اضافات خطی فاکتور
                if (Convert.ToDouble(BuyRow["Column23"].ToString()) != 0 && !chk_Net.Checked)
                {


                    DataTable ezafeTable = clDoc.ReturnTable(ConSale,
               "Select SUM(column19) as ezafe  from Table_016_Child1_BuyFactor where column01=" + BuyRow["ColumnId"].ToString() + "   ");
                    foreach (DataRow d in ezafeTable.Rows)
                    {
                        if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                        {
                            //********Bed
                            SourceTable.Rows.Add(19, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                null,
                                null,
                                null,
                                (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ") +
                                BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                            //*********Bes
                            SourceTable.Rows.Add(19, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                BuyRow["Column03"].ToString(),
                                null,
                                null,
                                (Convert.ToDouble(BuyRow["Column23"].ToString()) > 0 ? "اضافه خطی فاکتور خرید ش " : "تخفیف خطی2 فاکتور خرید ش ")
                                + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                                0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                        }
                    }



                }

                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(BuyRow["Column24"].ToString()) > 0 && !chk_Net.Checked)
                {
                    DataTable takhfifTable = clDoc.ReturnTable(ConSale,
                        "Select SUM(column17) as takhfif  from Table_016_Child1_BuyFactor where column01=" + BuyRow["ColumnId"].ToString() + "   ");


                    foreach (DataRow h in takhfifTable.Rows)
                    {

                        if (Convert.ToInt64(h["takhfif"]) > 0)
                        {
                            //********Bed
                            SourceTable.Rows.Add(19, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                BuyRow["Column03"].ToString(),
                                null,
                                null,
                                "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                            //*********Bes
                            SourceTable.Rows.Add(19, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                null,
                                null,
                                null,
                                "تخفیف خطی فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                        }
                    }


                }


                //سایر اضافات و کسورات
                foreach (DataRowView item in Child2Bind)
                {
                    string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column10", "ColumnId", item["Column02"].ToString());
                    string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column16", "ColumnId", item["Column02"].ToString());
                    string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column01", "ColumnId", item["Column02"].ToString());

                    //********Bed
                    SourceTable.Rows.Add(19, Bed, Bed, (item["Column05"].ToString() == "True" ? BuyRow["Column03"].ToString() : null),
                        null,
                        null,
                        Name +
                        " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(),
                       Convert.ToDouble(item["Column04"].ToString()), 0, (BuyRow["Column26"].ToString().Trim() == "" ? "0" : BuyRow["Column26"].ToString()),
                        (BuyRow["Column25"].ToString().Trim() == "" ? "-1" : BuyRow["Column25"].ToString()));
                    //*********Bes
                    SourceTable.Rows.Add(19, Bes, Bes, (item["Column05"].ToString() == "False" ? BuyRow["Column03"].ToString() : null),
                        null,
                      null,
                        Name + " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString(), 0,
                       Convert.ToDouble(item["Column04"].ToString()), (BuyRow["Column26"].ToString().Trim() == "" ? "0" : BuyRow["Column26"].ToString()),
                        (BuyRow["Column25"].ToString().Trim() == "" ? "-1" : BuyRow["Column25"].ToString()));
                }
            }
            gridEX1.DataSource = SourceTable;
            #endregion
            if (chk_AggDoc.Checked)
                AggDoc();
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            gridEX1.UpdateData();

            //*********Just Accounting Document
            SqlParameter DocNum;
            DocNum = new SqlParameter("DocNum", SqlDbType.Int);
            DocNum.Direction = ParameterDirection.Output;
            CommandTxt = string.Empty;
            CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";

            try
            {
                if (clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column11", _BuyID.ToString()) != 0)
                    throw new Exception("برای این فاکتور سند حسابداری صادر شده است");

                CheckEssentialItems(sender, e);

                string Message = "آیا مایل به صدور سند حسابداری هستید؟";
                if (uiTab2.Enabled)
                    Message = "آیا مایل به صدور سند حسابداری و رسید انبار هستید؟";

                if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //ثبت رسید
                    if (uiTab2.Enabled)
                    {
                        if (clDoc.OperationalColumnValueSA("Table_015_BuyFactor", "Column10", _BuyID.ToString()) == 0)
                            ExportResid();
                    }
                    else
                    {
                        UpdateResid();
                    }



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
                        //int ResidID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column10", "ColumnId", BuyRow["ColumnId"].ToString()));
                        //double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_012_Child_PwhrsReceipt", "ISNULL(SUM(Column21),0)", "Column01", ResidID.ToString()));
                        //double value = TotalValue;
                        CommandTxt += @" set @ResidID=(select Column10 from " + ConSale.Database + ".dbo.Table_015_BuyFactor where  ColumnId=" + BuyRow["ColumnId"].ToString() + ")";
                        CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column21),0) from " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt where Column01=@ResidID),0)";
                        CommandTxt += "set @value=@TotalValue   ";

                        DateTime BaseDate;
                        DateTime SecDate;
                        BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDate.Parse(faDatePicker1.Text));
                        FarsiLibrary.Win.Controls.FADatePicker fa;
                        fa = new FarsiLibrary.Win.Controls.FADatePicker();
                        fa.Text = faDatePicker1.Text;
                        try
                        {
                            if (BuyRow["Column30"].ToString()!="")
                            {
                            SecDate = BaseDate.AddDays(double.Parse(BuyRow["Column30"].ToString()));
                            fa.SelectedDateTime = SecDate;
                            fa.UpdateTextValue();
                        }
                        }
                        catch
                        {
                        }

                        if (BuyRow["Column15"].ToString() == "False")
                        {
                            foreach (GridEXRow item in gridEX1.GetRows())
                            {
                                string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());
                                if (item.Cells["Type"].Value.ToString() == "19" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0
                                    || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                                {
                                    //int DetialID = clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                    //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                    //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                    //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                    //    (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                    //       19, int.Parse(BuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);


                                    CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
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
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,19," + int.Parse(BuyRow["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
               Class_BasicOperation._UserName + "',getdate(),0," + (BuyRow["Column30"] != null && !string.IsNullOrWhiteSpace(BuyRow["Column30"].ToString()) ? BuyRow["Column30"] : "0") + ",N'" + fa.Text + "'); SET @DetialID=SCOPE_IDENTITY()";


                                    //اضافه کردن اقلام کالا به آرتیکل اول فاکتور مرجوعی
                                    if (item.RowIndex == 1 && chk_Nots.Checked)
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
                                //else if (item.Cells["Type"].Value.ToString() == "12" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                                //{
                                //    clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //        , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //        (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                //        (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                //        (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                //           12, int.Parse(BuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);
                                //}
                                //else if (item.Cells["Type"].Value.ToString() == "12" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 && Convert.ToDouble(item.Cells["Column12"].Value.ToString()) == 0))
                                //{
                                //    if (TotalValue > 0)
                                //    {
                                //        clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //       , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //       (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), 
                                //       item.Cells["Column10"].Text.Trim(),
                                //       Convert.ToInt64(value),
                                //       (value > 0 ? 0 : Convert.ToInt64(TotalValue)), 0, 0, -1,
                                //          12, int.Parse(BuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);
                                //        value = 0;
                                //    }
                                //}

                            }
                        }
                        else
                        {
                            foreach (GridEXRow item in gridEX1.GetRows())
                            {
                                string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());
                                if (item.Cells["Type"].Value.ToString() == "19" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0
                                    || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                                {
                                    //clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                    //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                    //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                    //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString()) *
                                    //    Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0)
                                    //    , (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())
                                    //    * Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0)
                                    //    , (item.Cells["Column12"].Text == "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) : 0),
                                    //    (item.Cells["Column11"].Text == "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) : 0),
                                    //    Int16.Parse(item.Cells["Column14"].Value.ToString()),
                                    //       19, int.Parse(BuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                    //    float.Parse(item.Cells["Column13"].Value.ToString()));


                                    CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],column23,column24) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString()) *
                                        Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())
                                        * Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0) + @",
" + (item.Cells["Column12"].Text == "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) : 0) + @",
" + (item.Cells["Column11"].Text == "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) : 0) + @",
" + Int16.Parse(item.Cells["Column14"].Value.ToString()) + ",19," + int.Parse(BuyRow["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
              Class_BasicOperation._UserName + "',getdate()," + float.Parse(item.Cells["Column13"].Value.ToString()) + "," + (BuyRow["Column30"] != null && !string.IsNullOrWhiteSpace(BuyRow["Column30"].ToString()) ? BuyRow["Column30"] : "0") + ",N'" + fa.Text + "'); ";

                                }
                                //    else if (item.Cells["Type"].Value.ToString() == "12" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                                //    {
                                //        clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //            , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //            (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                //             (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                //            (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0),
                                //            (item.Cells["Column11"].Text.Trim() != "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) / Convert.ToDouble(txt_CurrencyValue.Value.ToString()) : 0),
                                //            (item.Cells["Column12"].Text.Trim() != "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) / Convert.ToDouble(txt_CurrencyValue.Value.ToString()) : 0),
                                //            short.Parse(mlt_CurrencyType.Value.ToString()), 26, int.Parse(SaleRow["Column09"].ToString()), Class_BasicOperation._UserName,
                                //            float.Parse(txt_CurrencyValue.Value.ToString()));
                                //    }
                                //    else if (item.Cells["Type"].Value.ToString() == "12" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 && Convert.ToDouble(item.Cells["Column12"].Value.ToString()) == 0))
                                //    {
                                //        if (TotalValue > 0)
                                //        {
                                //            clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //           , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //           (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),Convert.ToDouble(Math.Round(value, 0).ToString()),
                                //           (value > 0 ? 0 :Convert.ToDouble(Math.Round(TotalValue, 0).ToString())), 0, 0, -1,
                                //              12, int.Parse(BuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);
                                //            value = 0;
                                //        }
                                //    }

                            }
                        }

                        //if (ResidID == 0)
                        //    clDoc.Update_Des_Table(Conware.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", int.Parse(BuyRow["Column10"].ToString()), DocID);
                        //else
                        //clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidID, DocID);
                        //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column11", "ColumnId", int.Parse(BuyRow["ColumnId"].ToString()), DocID);

                        CommandTxt += @"Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07= @Key  where ColumnId =@ResidID    ";
                        CommandTxt += @"Update " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column11= @Key,Column07='" + Class_BasicOperation._UserName + "',Column08=getdate()  where ColumnId =" + int.Parse(BuyRow["ColumnId"].ToString());



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
                                if (ResidNum == 0)
                                    Class_BasicOperation.ShowMsg("", "سند حسابداری با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");
                                else Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                    "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + ResidNum.ToString(), "Information");
                                bt_ExportDoc.Enabled = false;
                                this.DialogResult = System.Windows.Forms.DialogResult.Yes;


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
            foreach (GridEXRow item in gridEX1.GetRows())
            {
                if (item.Cells["Column01"].Text.Trim() == "" || item.Cells["Column10"].Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");
                if (item.Cells["Column01"].Text.Trim().All(char.IsDigit))
                {
                    throw new Exception("سرفصل" + item.Cells["Column01"].Text + "نامعتبر است");

                }
            }

            if (Convert.ToDouble(gridEX1.GetTotalRow().Cells["Column11"].Value.ToString()) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

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
            else if (e.Control && e.KeyCode == Keys.W)
                bt_ViewDocs_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_ViewDrafts_Click(sender, e);
        }

        private void ExportResid()
        {
            //if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", _BuyID.ToString()) != 0)
            //    throw new Exception("برای این فاکتور، رسید انبار صادر شده است");

            //**Resid Header
            CommandTxt += " Declare @Key1 int ";

            if (chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) > 0)

                ResidNum = Convert.ToInt32(txt_DraftNum.Value);


            else
                ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");

            CommandTxt += @" INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
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
                                                                          ) VALUES (" + ResidNum + ",'" + BuyRow["Column02"].ToString() + "'," +
                 mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + BuyRow["Column03"].ToString() + ",'" + "فاکتور خرید ش" +
                 BuyRow["Column01"].ToString() + " تاریخ " + BuyRow["Column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                 (BuyRow["Column09"].ToString().Trim() == "" ? "NULL" : BuyRow["Column09"].ToString()) + "," + BuyRow["ColumnId"].ToString() + ",0,0," +
                 (BuyRow["Column15"].ToString() == "True" ? "1" : "0") + "," +
                 (BuyRow["Column25"].ToString().Trim() == "" ? "NULL" : BuyRow["Column25"].ToString()) + "," +
                 BuyRow["Column26"].ToString()
                 + ",1,null); SET @Key1=Scope_Identity()";

            //            SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
            //            key.Direction = ParameterDirection.Output;
            //            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //            {
            //                Con.Open();
            //                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
            //                                                                            [column01],
            //                                                                            [column02],
            //                                                                            [column03],
            //                                                                            [column04],
            //                                                                            [column05],
            //                                                                            [column06],
            //                                                                            [column07],
            //                                                                            [column08],
            //                                                                            [column09],
            //                                                                            [column10],
            //                                                                            [column11],
            //                                                                            [column12],
            //                                                                            [column13],
            //                                                                            [column14],
            //                                                                            [Column15],
            //                                                                            [Column16],
            //                                                                            [Column17],
            //                                                                            [Column18],
            //                                                                            [Column19],
            //                                                                            [Column20]
            //                                                                          ) VALUES (" + ResidNum + ",'" + BuyRow["Column02"].ToString() + "'," +
            //                 mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + BuyRow["Column03"].ToString() + ",'" + "فاکتور خرید ش" +
            //                 BuyRow["Column01"].ToString() + " تاریخ " + BuyRow["Column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
            //                 (BuyRow["Column09"].ToString().Trim() == "" ? "NULL" : BuyRow["Column09"].ToString()) + "," + BuyRow["ColumnId"].ToString() + ",0,0," +
            //                 (BuyRow["Column15"].ToString() == "True" ? "1" : "0") + "," +
            //                 (BuyRow["Column25"].ToString().Trim() == "" ? "NULL" : BuyRow["Column25"].ToString()) + "," +
            //                 BuyRow["Column26"].ToString()
            //                 + ",1,null); SET @Key=Scope_Identity()", Con);
            //                Insert.Parameters.Add(key);
            //                Insert.ExecuteNonQuery();
            //                ResidID = int.Parse(key.Value.ToString());

            //Resid Detail
            foreach (DataRowView item in Child1Bind)
            {

                //// added by Roostaee 96/07/05 
                // اعمال تسهیم
                DataTable value = new DataTable();
                try
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"DECLARE @share  FLOAT,
                                                                            @sum    DECIMAL(18, 3),
                                                                            @Net    DECIMAL(18, 3)

                                                                    SET @sum = (
                                                                            SELECT SUM(ISNULL(tt.VE, 0))
                                                                            FROM   (
                                                                                       SELECT (
                                                                                                  CASE 
                                                                                                       WHEN tcbf.column05 = 0 THEN tcbf.column04
                                                                                                       ELSE ((-1) * tcbf.column04)
                                                                                                  END
                                                                                              ) AS VE
                                                                                       FROM   Table_017_Child2_BuyFactor tcbf
                                                                                              JOIN Table_024_Discount_Buy tdb
                                                                                                   ON  tdb.columnid = tcbf.column02
                                                                                       WHERE  tdb.Column18 = 1
                                                                                              AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + BuyRow["columnid"].ToString() + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue    DECIMAL(18, 3),
                                                                            @totalvalue   DECIMAL(18, 3)
    
                                                                    SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item["Column20"].ToString()) + @" /nullif( " + Convert.ToDouble(item["Column07"].ToString()) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                       AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                            isnull( @unitvalue,0) AS unitvalue,
                                                                            isnull(  @totalvalue,0) AS totalvalue
                                                                    ", ConSale);
                    Adapter.Fill(value);

                }
                catch
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT 1 AS share,
                                                                            0 AS unitvalue,
                                                                            0 AS totalvalue
                                                                    ", ConSale);
                    Adapter.Fill(value);

                }

                //SqlCommand InsertDetail = new SqlCommand("INSERT INTO Table_012_Child_PwhrsReceipt VALUES (" + ResidID + "," + item["Column02"].ToString() + "," +
                //    item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                //    item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                //    (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                //    + "',getdate(),0," + (Convert.ToDouble(item["Column07"].ToString()) > 0 ? Convert.ToDouble(value.Rows[0]["unitvalue"]) : 0)
                //        + "," + Convert.ToDouble(value.Rows[0]["unitvalue"]) * Convert.ToDouble(item["Column07"].ToString()) + "," + item["ColumnId"].ToString()
                //    + ",NULL,NULL," + (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString())
                //    + "," + item["Column14"].ToString()
                //    + ",0,0,0," +
                //    (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                //    (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                //    "," + item["Column29"].ToString() + "," + item["Column30"].ToString() +
                //    "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", Con);
                //InsertDetail.ExecuteNonQuery();


                //                    SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
                //           ,[column02]
                //           ,[column03]
                //           ,[column04]
                //           ,[column05]
                //           ,[column06]
                //           ,[column07]
                //           ,[column08]
                //           ,[column09]
                //           ,[column10]
                //           ,[column11]
                //           ,[column12]
                //           ,[column13]
                //           ,[column14]
                //           ,[column15]
                //           ,[column16]
                //           ,[column17]
                //           ,[column18]
                //           ,[column19]
                //           ,[column20]
                //           ,[column21]
                //           ,[column22]
                //           ,[column23]
                //           ,[column24]
                //           ,[column25]
                //           ,[column26]
                //           ,[column27]
                //           ,[column28]
                //           ,[column29]
                //           ,[Column30]
                //           ,[Column31]
                //           ,[Column32]
                //           ,[Column33]
                //           ,[Column34]
                //           ,[Column35]) VALUES (" + ResidID + "," + item["Column02"].ToString() + "," +
                //                        item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                //                        item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                //                        (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                //                        + "',getdate(),0," + (Convert.ToDouble(item["Column07"].ToString()) > 0 ? (Convert.ToDouble(item["Column20"].ToString()) / Convert.ToDouble(item["Column07"].ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0)
                //                            + "," + Convert.ToDouble(item["Column20"]) * Convert.ToDouble(value.Rows[0]["share"]) + "," + item["ColumnId"].ToString()
                //                        + ",NULL,NULL," + (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString())
                //                        + "," + item["Column14"].ToString()
                //                        + ",0,0,0," +
                //                        (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                //                        (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                //                        "," + item["Column29"].ToString() + "," + item["Column30"].ToString() +
                //                        "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", Con);
                //                    InsertDetail.ExecuteNonQuery();

                //}

                CommandTxt += @" INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35],[Column36],[Column37]) VALUES (@Key1," + item["Column02"].ToString() + "," +
                    item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                    item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                    (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                    + "',getdate(),0," + (Convert.ToDouble(item["Column07"].ToString()) > 0 ? (Convert.ToDouble(item["Column20"].ToString()) / Convert.ToDouble(item["Column07"].ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0)
                        + "," + Convert.ToDouble(item["Column20"]) * Convert.ToDouble(value.Rows[0]["share"]) + "," + item["ColumnId"].ToString()
                    + ",NULL,NULL," + (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString())
                    + "," + item["Column14"].ToString()
                    + ",0,0,0," +
                    (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                    (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                    "," + item["Column29"].ToString() + "," + item["Column30"].ToString() +
                    "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + "," + (item["Column36"].ToString().Trim() == "" ? "NULL" : "N'" + item["Column36"].ToString() + "'") + "," + (item["Column37"].ToString().Trim() == "" ? "NULL" : "N'" + item["Column37"].ToString() + "'") + ") ";

                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column10", "ColumnId", int.Parse(BuyRow["ColumnId"].ToString()), ResidID);
            }
            CommandTxt += "Update " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column10=@Key1 where ColumnId=" + int.Parse(BuyRow["ColumnId"].ToString());

        }

        private void UpdateResid()
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                //Update Resid Detail
                foreach (DataRowView item in Child1Bind)
                {
                    try
                    {
                        DataTable value = new DataTable();
                        SqlDataAdapter Adapter = new SqlDataAdapter(@"DECLARE @share  FLOAT,
                                                                            @sum    DECIMAL(18, 3),
                                                                            @Net    DECIMAL(18, 3)

                                                                    SET @sum = (
                                                                            SELECT SUM(ISNULL(tt.VE, 0))
                                                                            FROM   (
                                                                                       SELECT (
                                                                                                  CASE 
                                                                                                       WHEN tcbf.column05 = 0 THEN tcbf.column04
                                                                                                       ELSE ((-1) * tcbf.column04)
                                                                                                  END
                                                                                              ) AS VE
                                                                                       FROM   Table_017_Child2_BuyFactor tcbf
                                                                                              JOIN Table_024_Discount_Buy tdb
                                                                                                   ON  tdb.columnid = tcbf.column02
                                                                                       WHERE  tdb.Column18 = 1
                                                                                              AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + BuyRow["columnid"].ToString() + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue   DECIMAL(18, 3),
                                                                            @totalvalue  DECIMAL(18, 3)
    
                                                                    SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item["Column20"].ToString()) + @" /nullif( " + Convert.ToDouble(item["Column07"].ToString()) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                       AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                            isnull( @unitvalue,0) AS unitvalue,
                                                                            isnull(  @totalvalue,0) AS totalvalue
                                                                    ", ConSale);
                        Adapter.Fill(value);

                        SqlCommand UpdateDetail = new SqlCommand(
                            "Update Table_012_Child_PwhrsReceipt set " +
                            "column08 = " + item["column08"].ToString() + ", " +
                            "column09 = " + item["column09"].ToString() + ", " +
                            "column10 = " + item["column10"].ToString() + ", " +
                            "column11 = " + item["column11"].ToString() + ", " +
                            "column20 = " + (Convert.ToDouble(item["column07"].ToString()) > 0 ? (Convert.ToDouble(item["Column20"].ToString()) / Convert.ToDouble(item["Column07"].ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0)
                             + ", " +
                            "column21 = " + Convert.ToDouble(item["Column20"]) * Convert.ToDouble(value.Rows[0]["share"]) + " Where (column01 = " +
                            _Tab2.ToString() + ") And (column22 = " + item["columnid"].ToString() + ")",
                            Con);
                        UpdateDetail.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
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

        private void chk_RegisterGoods_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RegisterBuyFactorWithGoods = chk_RegisterGoods.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
        }

        private string CerateSharh(double TotalValue, double TotalAmount, double TotalNetPrice)
        {
            string Sharh = string.Empty;



            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConMain);
            DataTable setting = new DataTable();
            Adapter.Fill(setting);


            try
            {
                if (Convert.ToBoolean(setting.Rows[17]["Column02"]))
                    Sharh = "فاکتور خرید ش " + BuyRow["Column01"].ToString();

                if (Convert.ToBoolean(setting.Rows[18]["Column02"]))
                    Sharh += " به تاریخ " + BuyRow["Column02"].ToString();

                if (Convert.ToBoolean(setting.Rows[19]["Column02"]))
                    Sharh += " مقدارکل " + TotalValue;

                if (Convert.ToBoolean(setting.Rows[20]["Column02"]))
                    Sharh += " قیمت کل " + string.Format("{0:#,##0.###}", TotalAmount);

                if (Convert.ToBoolean(setting.Rows[22]["Column02"]))
                    Sharh += " مدت اعتبار " + BuyRow["column24"].ToString();

                if (Convert.ToBoolean(setting.Rows[23]["Column02"]))
                    Sharh += " مبلغ خالص " + string.Format("{0:#,##0.###}", TotalNetPrice);
                if (Convert.ToBoolean(setting.Rows[24]["Column02"]))
                    Sharh += " مبلغ قابل پرداخت " + string.Format("{0:#,##0.###}", (TotalNetPrice + Convert.ToDouble(BuyRow["Column21"]) - Convert.ToDouble(BuyRow["Column22"])));

                if (Convert.ToBoolean(setting.Rows[25]["Column02"]))
                    Sharh += " جمع تخفیف " + string.Format("{0:#,##0.###}", BuyRow["Column24"]);
                if (Convert.ToBoolean(setting.Rows[26]["Column02"]))
                    Sharh += " جمع اضافه " + string.Format("{0:#,##0.###}", BuyRow["Column23"]);

                if (Convert.ToBoolean(setting.Rows[27]["Column02"]))
                {
                    string name = string.Empty;

                    using (SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE))
                    {
                        ConBase.Open();
                        string CommandText = @"SELECT     Column02 From Table_045_PersonInfo WHERE ColumnId=" + BuyRow["column03"] + "";

                        SqlCommand Command = new SqlCommand(CommandText, ConBase);
                        name = Command.ExecuteScalar().ToString();
                    }
                    if (name != string.Empty)
                        Sharh += " خریدار " + name;

                }


                if (Convert.ToBoolean(setting.Rows[28]["Column02"]))
                {
                    string Ware = string.Empty;

                    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS))
                    {
                        ConWare.Open();
                        string CommandText = @"SELECT     Column02 From Table_001_PWHRS WHERE ColumnId=" + BuyRow["Column27"] + "";

                        SqlCommand Command = new SqlCommand(CommandText, ConWare);
                        Ware = Command.ExecuteScalar().ToString();

                    }
                    if (Ware != string.Empty)
                        Sharh += " انبار " + Ware;



                }
                if (Convert.ToBoolean(setting.Rows[30]["Column02"]))
                {
                    string Func = string.Empty;
                    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS))
                    {
                        string CommandText1 = @"SELECT     Column02 From table_005_PwhrsOperation WHERE ColumnId=" + BuyRow["Column28"] + "";

                        SqlCommand Command1 = new SqlCommand(CommandText1, ConWare);
                        Func = Command1.ExecuteScalar().ToString();


                    }
                    if (Func != string.Empty)
                        Sharh += " نوع رسید " + Func;
                }

                if (Convert.ToBoolean(setting.Rows[21]["Column02"]) && BuyRow["column04"] != null && BuyRow["column04"].ToString() != string.Empty)
                    Sharh += "  " + BuyRow["column04"].ToString();
                if (Convert.ToBoolean(setting.Rows[29]["Column02"]) && BuyRow["column12"] != null && BuyRow["column12"].ToString() != string.Empty)
                    Sharh += "  " + BuyRow["column12"].ToString();

                //if (Convert.ToBoolean(setting.Rows[31]["Column02"]))
                //    Sharh += " میانگین فی " + string.Format("{0:#,##0.###}", (TotalAmount / TotalValue));

                //if (Convert.ToBoolean(setting.Rows[34]["Column02"]))
                //{

                //    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                //    {
                //        ConWare.Open();
                //        string CommandText = @"SELECT     Column02 From table_004_CommodityAndIngredients WHERE ColumnId=(select top 1 column02 from " + ConSale.Database + ".dbo.Table_016_Child1_BuyFactor where column01 =" + BuyRow["columnid"] + ")   ";

                //        SqlCommand Command = new SqlCommand(CommandText, ConWare);
                //        string good = Command.ExecuteScalar().ToString();

                //        Sharh += "  " + good;


                //    }
                //}

            }
            catch
            { }
            return Sharh;
        }

        private void chk_RegisterGoods_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.RegisterBuyFactorWithGoods = chk_RegisterGoods.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
        }

        private void chk_Nots_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RegisterBuyFactorNoteGoods = chk_Nots.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
        }

        private void chk_without_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.whtoutgoods = chk_without.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
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

        private void chk_Baha_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.BuyBaha = chk_Baha.Checked;
            Properties.Settings.Default.Save();
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

        private void chk_AggDoc_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.BuyAggregationSaleDoc = chk_AggDoc.Checked;
            Properties.Settings.Default.Save();

            PrepareDoc();

        }

        private void chk_Net_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.chk_ByuNet = chk_Net.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();

        }

        private void AggDoc()
        {
            try
            {
                //تجمیع سطرها

                DataTable _1Table = SourceTable.DefaultView.ToTable("_1Table", true, new string[] { "Type", "Column01", "Column07", "Column08", "Column09", "Column13", "Column14" });
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
                            Description = " فاکتور خرید ش " + BuyRow["Column01"].ToString() + " به تاریخ " + BuyRow["Column02"].ToString();
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

                        if (Bed - Bes != 0 && item["Type"].ToString() == "19")
                            _2Table.Rows.Add(item["Type"], item["Column01"].ToString(), item["Column01"].ToString(),
                                (item["Column07"].ToString().Trim() == "" ? null : item["Column07"].ToString()),
                                (item["Column08"].ToString().Trim() == "" ? null : item["Column08"].ToString()),
                                (item["Column09"].ToString().Trim() == "" ? null : item["Column09"].ToString()),
                                Description, Bed, Bes, Convert.ToDouble(item["Column13"].ToString()),
                                (item["Column14"].ToString().Trim() == "" ? (object)null : Convert.ToInt16(item["Column14"].ToString())));


                    }

                }
                SourceTable.DefaultView.RowFilter = "";
                gridEX1.DataSource = _2Table;

            }
            catch { }
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

        private void Frm_009_ExportDocInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.PCBes = chk_PCBes.Checked;
            Properties.Settings.Default.PCBed = chk_PCBed.Checked;
            Properties.Settings.Default.BuyGoodACCNum = chk_GoodACCNum.Checked;

            Properties.Settings.Default.Save();
        }

        private void chk_GoodACCNum_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DraftNum.Checked)
            {
                txt_DraftNum.Enabled = true;
                txt_DraftNum.Select();
            }
            else
                txt_DraftNum.Enabled = false;
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
    }
}
