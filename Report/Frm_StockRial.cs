using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace PCLOR.Report
{
    public partial class Frm_StockRial : Form
    {
        bool _BackSpace = false;
        string Date1;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);

        public Frm_StockRial()
        {
            InitializeComponent();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (mlt_Ware.DropDownList.GetCheckedRows().Count() == 0)
            {
                MessageBox.Show("انبار مورد نظر را انتخاب کنید");
                return;
            }

            if (!faDate2.FADatePicker.SelectedDateTime.HasValue)
            {
                MessageBox.Show("تاریخ مورد نظر را وارد کنید");
                return;
            }

            Date1 = null;
            Date1 = faDate2.FADatePicker.Text;

            if (mlt_Ware.Text.Trim() == "")
            {
                MessageBox.Show("انبار مورد نظر را انتخاب کنید");
                return;
            }

            Int16 WareID;
            WareID = Int16.Parse(mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString());

            if (WareID > 0)
            {
                try
                {

                    string whr = string.Empty;
                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Ware.DropDownList.GetCheckedRows())
                    {
                        if (dr.Cells["ColumnId"].Value.ToString() != "0")
                            whr += dr.Cells["ColumnId"].Value + ",";
                    }
                    whr = whr.TrimEnd(',');


                    DataTable SaleRial = new DataTable();

                    SaleRial = clDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_65_HeaderOtherPWHRS.date, dbo.Table_70_DetailOtherPWHRS.Machine, 
                      dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_65_HeaderOtherPWHRS.Recipt, SUM(dbo.Table_70_DetailOtherPWHRS.Count) AS CountInput, 
                      SUM(" + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column07) AS CountOutput, SUM(dbo.Table_70_DetailOtherPWHRS.Count) 
                      - SUM(" + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column07) AS Remain, SUM(dbo.Table_70_DetailOtherPWHRS.weight) AS weight, SUM(dbo.Table_005_TypeCloth.FiSale) AS FiSale, 
                      SUM(dbo.Table_70_DetailOtherPWHRS.weight) * SUM(dbo.Table_005_TypeCloth.FiSale) AS SumWeight, " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column11, 
                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column10, " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02, " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42, 
                      dbo.Table_65_HeaderOtherPWHRS.PWHRS, dbo.Table_050_Packaging.IDProduct
                    FROM         " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor INNER JOIN
                      dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_70_DetailOtherPWHRS.TypeCloth = dbo.Table_005_TypeCloth.ID ON 
                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.Column34 = dbo.Table_70_DetailOtherPWHRS.Barcode INNER JOIN
                      " + ConSale.Database + @".dbo.Table_010_SaleFactor ON " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column01 = " + ConSale.Database + @".dbo.Table_010_SaleFactor.columnid INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_70_DetailOtherPWHRS.Barcode = dbo.Table_050_Packaging.Barcode
                    GROUP BY dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_65_HeaderOtherPWHRS.date, dbo.Table_65_HeaderOtherPWHRS.Recipt, dbo.Table_70_DetailOtherPWHRS.Machine, 
                      dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_70_DetailOtherPWHRS.Count, dbo.Table_70_DetailOtherPWHRS.Barcode, " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column11, 
                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column10, " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02, " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42, 
                      dbo.Table_65_HeaderOtherPWHRS.PWHRS, dbo.Table_050_Packaging.IDProduct
                    HAVING      (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 <= N'" + Date1 + @"') AND (" + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42 IN (" + whr + @")) AND 
                      (dbo.Table_65_HeaderOtherPWHRS.PWHRS IN (" + whr + @")) AND (dbo.Table_65_HeaderOtherPWHRS.date <= N'" + Date1 + @"')");

                    bindingSource1.DataSource = SaleRial;
                   


                }
                catch (System.Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
            else
            {
                try
                {

                    DataTable SaleRial2 = new DataTable();

                    SaleRial2 = clDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_65_HeaderOtherPWHRS.date, dbo.Table_70_DetailOtherPWHRS.Machine, 
                      dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_65_HeaderOtherPWHRS.Recipt, SUM(dbo.Table_70_DetailOtherPWHRS.Count) AS CountInput, 
                      SUM(" + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column07) AS CountOutput, SUM(dbo.Table_70_DetailOtherPWHRS.Count) - SUM(" + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column07) AS Remain, SUM(dbo.Table_70_DetailOtherPWHRS.weight) AS weight, SUM(dbo.Table_005_TypeCloth.FiSale) AS FiSale, 
                      SUM(dbo.Table_70_DetailOtherPWHRS.weight) * SUM(dbo.Table_005_TypeCloth.FiSale) AS SumWeight, " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column11, 
                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column10, " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02, " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42, 
                      dbo.Table_65_HeaderOtherPWHRS.PWHRS, dbo.Table_050_Packaging.IDProduct
                    FROM         " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor INNER JOIN
                      dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_70_DetailOtherPWHRS.TypeCloth = dbo.Table_005_TypeCloth.ID ON 
                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.Column34 = dbo.Table_70_DetailOtherPWHRS.Barcode INNER JOIN
                      " + ConSale.Database + @".dbo.Table_010_SaleFactor ON " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column01 = " + ConSale.Database + @".dbo.Table_010_SaleFactor.columnid INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_70_DetailOtherPWHRS.Barcode = dbo.Table_050_Packaging.Barcode
                    GROUP BY dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_65_HeaderOtherPWHRS.date, dbo.Table_65_HeaderOtherPWHRS.Recipt, dbo.Table_70_DetailOtherPWHRS.Machine, 
                      dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_70_DetailOtherPWHRS.Count, dbo.Table_70_DetailOtherPWHRS.Barcode, " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column11, 
                      " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column10, " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02, " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42, 
                      dbo.Table_65_HeaderOtherPWHRS.PWHRS, dbo.Table_050_Packaging.IDProduct
                    HAVING      (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 <= N'" + Date1 + @"') AND (dbo.Table_65_HeaderOtherPWHRS.date <= N'" + Date1 + @"')");
                    bindingSource1.DataSource = SaleRial2;



                }
                catch (System.Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }




        }

        private void Frm_StockRial_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            DataTable WareTable = clDoc.ReturnTable(ConWare, @"Select * from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                   @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
            mlt_Ware.DropDownDataSource = WareTable;
            DataRow Row = WareTable.NewRow();
            Row["ColumnId"] = 0;
            Row["Column02"] = "همه انبارها";
            WareTable.Rows.InsertAt(Row, 0);
            mlt_Ware.DropDownDataSource = WareTable;
            mlt_Ware.DropDownList.SetValue("ColumnId", 0);
            gridEX2.DropDowns["Codecommodity"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
            gridEX2.DropDowns["Namecommodity"].DataSource = clDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth from Table_005_TypeCloth");
            gridEX2.DropDowns["ware"].DataSource = clDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS");


            string[] Dates = Properties.Settings.Default.date2.Split('-');
            faDate2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
         
            gridEX2.RemoveFilters();
            gridEX2.UpdateData();

        }

        private void faDate2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDate2.FADatePicker.HideDropDown();
                    btn_Search_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDate2_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;

                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        private void tsexcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXExporter1.GridEX = gridEX2;
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    gridEXPrintDocument1.GridEX = gridEX2;
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (sender is Janus.Windows.GridEX.EditControls.CheckedComboBox)
            {
                if (e.KeyChar != 13 && !char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.CheckedComboBox)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar != 13 && !char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else Class_BasicOperation.isEnter(e.KeyChar);
        }
    }
}
