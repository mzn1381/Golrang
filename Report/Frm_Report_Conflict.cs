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
    public partial class Frm_Report_Conflict : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        bool _BackSpace = false;
        string Date1;
        public Frm_Report_Conflict()
        {
            InitializeComponent();
        }

        private void Frm_Report_Conflict_Load(object sender, EventArgs e)
        {

            gridEX1.DropDowns["Codecommodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
            gridEX1.DropDowns["Namecommodity"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth from Table_005_TypeCloth");


            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            //DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select * from Table_001_PWHRS  WHERE
            //                                                 'True'='" + isadmin.ToString() +
            //                                       @"'  or 
            //                                                   Columnid IN 
            //                                                   (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
            //mlt_Ware.DropDownDataSource = WareTable;
            //DataRow Row = WareTable.NewRow();
            //Row["ColumnId"] = 0;
            //Row["Column02"] = "همه انبارها";
            //WareTable.Rows.InsertAt(Row, 0);
            //mlt_Ware.DropDownDataSource = WareTable;
            //mlt_Ware.DropDownList.SetValue("ColumnId", 0);

            //string[] Dates = Properties.Settings.Default.date1.Split('-');
            //faDate1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            string[] Dates = Properties.Settings.Default.date1.Split('-');
            faDate1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDate2.FADatePicker.SelectedDateTime = DateTime.Now;
            //gridEX1.DropDowns["IDProduct"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Number from Table_035_Production");

            gridEX1.RemoveFilters();
            gridEX1.UpdateData();



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void faDate1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
            {
                faDate1.FADatePicker.HideDropDown();
                faDate2.FADatePicker.Select();
            }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDate1_TextChanged(object sender, EventArgs e)
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

        private void faDate2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
            {
                faDate2.FADatePicker.HideDropDown();
                btnSearch_Click(sender, e);
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
                gridEXExporter1.GridEX = gridEX1;
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
                    gridEXPrintDocument1.GridEX = gridEX1;
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
//            if (mlt_Ware.DropDownList.GetCheckedRows().Count() == 0)
//            {
//                MessageBox.Show("انبار مورد نظر را انتخاب کنید");
//                return;
//            }

//            if (!faDate1.FADatePicker.SelectedDateTime.HasValue)
//            {
//                MessageBox.Show("تاریخ مورد نظر را وارد کنید");
//                return;
//            }

//            Date1 = null;
//            Date1 = faDate1.FADatePicker.Text;

//            if (mlt_Ware.Text.Trim() == "")
//            {
//                MessageBox.Show("انبار مورد نظر را انتخاب کنید");
//                return;
//            }

//            Int16 WareID;
//            WareID = Int16.Parse(mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString());

//            if (WareID > 0)
//            {
//                try
//                {

//                    string whr = string.Empty;
//                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Ware.DropDownList.GetCheckedRows())
//                    {
//                        if (dr.Cells["ColumnId"].Value.ToString() != "0")
//                            whr += dr.Cells["ColumnId"].Value + ",";
//                    }
//                    whr = whr.TrimEnd(',');


//                    DataTable SaleRial = new DataTable();

//                    SaleRial = ClDoc.ReturnTable(ConPCLOR, @"SELECT     TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity, SUM(SumSend) AS SumSend, SUM(SumRecipt) AS SumRecipt, SUM(SumSend) - SUM(SumRecipt) AS Remain
//FROM         (SELECT     dbo.Table_70_DetailOtherPWHRS.TypeCloth, dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.Machine, dbo.Table_70_DetailOtherPWHRS.weight, 
//                                              dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, 0 AS SumSend, dbo.Table_70_DetailOtherPWHRS.Count AS SumRecipt, 
//                                              dbo.Table_65_HeaderOtherPWHRS.PWHRS
//                       FROM          dbo.Table_65_HeaderOtherPWHRS INNER JOIN
//                                              dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
//                       WHERE      (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (dbo.Table_65_HeaderOtherPWHRS.date >='" + faDate1.FADatePicker.Text + @"') AND (dbo.Table_65_HeaderOtherPWHRS.date <= '" + faDate2.FADatePicker.Text + @"')
//                       UNION ALL
//                       SELECT     Table_70_DetailOtherPWHRS_1.TypeCloth, Table_70_DetailOtherPWHRS_1.Barcode, Table_70_DetailOtherPWHRS_1.Machine, Table_70_DetailOtherPWHRS_1.weight, 
//                                             Table_70_DetailOtherPWHRS_1.TypeColor, Table_70_DetailOtherPWHRS_1.CodeCommondity, Table_70_DetailOtherPWHRS_1.Count AS SumSend, 0 AS SumRecipt, 
//                                             Table_65_HeaderOtherPWHRS_1.PWHRS
//                       FROM         dbo.Table_65_HeaderOtherPWHRS AS Table_65_HeaderOtherPWHRS_1 INNER JOIN
//                                             dbo.Table_70_DetailOtherPWHRS AS Table_70_DetailOtherPWHRS_1 ON Table_65_HeaderOtherPWHRS_1.ID = Table_70_DetailOtherPWHRS_1.FK
//                       WHERE     (Table_65_HeaderOtherPWHRS_1.Sends = 1) AND (Table_65_HeaderOtherPWHRS_1.date >= '" + faDate1.FADatePicker.Text + @"') AND (Table_65_HeaderOtherPWHRS_1.date <= '" + faDate2.FADatePicker.Text + @"')) AS dt
//WHERE     (PWHRS IN (" + whr + @"))
//GROUP BY TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity, PWHRS ");

//                    bindingSource1.DataSource = SaleRial;


//                }
//                catch (System.Exception ex)
//                {
//                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
//                }
//            }
//            else
//            {
                try
                {

                    DataTable SaleRial2 = new DataTable();

                    SaleRial2 = ClDoc.ReturnTable(ConPCLOR, @"SELECT      TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity, SUM(SumSend) AS SumSend, SUM(SumRecipt) AS SumRecipt, SUM(SumSend) - SUM(SumRecipt) 
                      AS Remain
FROM         (SELECT     dbo.Table_70_DetailOtherPWHRS.TypeCloth, dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.Machine, dbo.Table_70_DetailOtherPWHRS.weight, 
                                              dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, 0 AS SumSend, dbo.Table_70_DetailOtherPWHRS.Count AS SumRecipt
                       FROM          dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                              dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                       WHERE      (dbo.Table_70_DetailOtherPWHRS.NumberRecipt <> 0) AND  (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1) AND (dbo.Table_65_HeaderOtherPWHRS.date >= '" + faDate1.FADatePicker.Text + @"') AND (dbo.Table_65_HeaderOtherPWHRS.date <= '" + faDate2.FADatePicker.Text + @"')
                       UNION ALL
                       SELECT     Table_70_DetailOtherPWHRS_1.TypeCloth, Table_70_DetailOtherPWHRS_1.Barcode, Table_70_DetailOtherPWHRS_1.Machine, Table_70_DetailOtherPWHRS_1.weight, 
                                             Table_70_DetailOtherPWHRS_1.TypeColor, Table_70_DetailOtherPWHRS_1.CodeCommondity, Table_70_DetailOtherPWHRS_1.Count AS SumSend, 0 AS SumRecipt
                       FROM         dbo.Table_65_HeaderOtherPWHRS AS Table_65_HeaderOtherPWHRS_1 INNER JOIN
                                             dbo.Table_70_DetailOtherPWHRS AS Table_70_DetailOtherPWHRS_1 ON Table_65_HeaderOtherPWHRS_1.ID = Table_70_DetailOtherPWHRS_1.FK
                       WHERE     (Table_70_DetailOtherPWHRS_1.NumberDraft <>0) AND (Table_65_HeaderOtherPWHRS_1.Sends = 1) AND (Table_65_HeaderOtherPWHRS_1.date >= '" + faDate1.FADatePicker.Text + @"') AND (Table_65_HeaderOtherPWHRS_1.date <= '" + faDate2.FADatePicker.Text + @"')) AS dt
GROUP BY TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity
ORDER BY Barcode");
                    bindingSource1.DataSource = SaleRial2;
                }
                catch (System.Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }





            //}

        }
    }
}
