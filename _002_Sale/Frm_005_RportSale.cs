using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR._002_Sale
{
    public partial class Frm_005_RportSale : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
       
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        bool _BackSpace = false;
        public Frm_005_RportSale()
        {
            
            InitializeComponent();
        }

        private void Frm_005_RportSale_Load(object sender, EventArgs e)
        {
            try
            {

                string[] Dates = Properties.Settings.Default.date1.Split('-');
                //faDate1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
                faDate2.FADatePicker.SelectedDateTime = DateTime.Now;
                faDate1.FADatePicker.SelectedDateTime = DateTime.Now;
                gridEX_Extra.RemoveFilters();
                gridEX_List.RemoveFilters();
                gridEX1.RemoveFilters();
                gridEX_List.DropDowns["Codecommodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
                gridEX_List.DropDowns["Namecommodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column02 from table_004_CommodityAndIngredients");
                gridEX_List.DropDowns["UnitCount"].DataSource = ClDoc.ReturnTable(ConBase, @"select Column00,Column01 from Table_070_CountUnitInfo");
                
                
                gridEX1.DropDowns["TypeColor"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,TypeColor from Table_010_TypeColor");
                gridEX1.DropDowns["DocId"].DataSource = ClDoc.ReturnTable(ConACNT, @"Select ColumnId,Column00 from Table_060_SanadHead");
                gridEX1.DropDowns["Customer"].DataSource = ClDoc.ReturnTable(ConBase, @"select columnId, column01,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["VahedNumber"].DataSource = ClDoc.ReturnTable(ConBase, @"select column00, column01 from Table_070_CountUnitInfo");
                gridEX1.DropDowns["NameKala"].DataSource = ClDoc.ReturnTable(ConWHRS, @"select columnId, column02 from table_004_CommodityAndIngredients");
                gridEX1.DropDowns["NumberDraft"].DataSource = ClDoc.ReturnTable(ConWHRS, @"select Columnid,Column01 from Table_007_PwhrsDraft");
                this.gridEX_Extra.Font = new System.Drawing.Font("B Mitra", 10, System.Drawing.FontStyle.Bold);
                this.gridEX1.Font = new System.Drawing.Font("B Mitra",10, System.Drawing.FontStyle.Bold);
                this.gridEX_List.Font = new System.Drawing.Font("B Mitra", 10, System.Drawing.FontStyle.Bold);
            }
            catch (Exception ex)

            { 
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }


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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (faDate1.FADatePicker.SelectedDateTime.HasValue && faDate2.FADatePicker.SelectedDateTime.HasValue)
            {
                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable user = ClDoc.ReturnTable(ConSale, @"select column13 from Table_010_SaleFactor  where Column42 IN (select Ware from " + ConPCLOR.Database + @".dbo.Table_95_DetailWare) AND Column13='" + Class_BasicOperation._UserName + "'");

                    if (isadmin)
                    {
                        DataTable saleheader = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_010_SaleFactor.columnid AS ID, dbo.Table_010_SaleFactor.column01 AS Number, dbo.Table_010_SaleFactor.column02 AS Date, dbo.Table_010_SaleFactor.column03 AS Kharidar, 
                         dbo.Table_010_SaleFactor.column09 AS NumberHavale, dbo.Table_010_SaleFactor.column10 AS DocId, dbo.Table_010_SaleFactor.column13 AS UserSabt, dbo.Table_010_SaleFactor.column14 AS DateTime, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column10) AS UnitPrice, SUM(dbo.Table_011_Child1_SaleFactor.column11) AS TotalPrice, SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS sumweight
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
GROUP BY dbo.Table_010_SaleFactor.columnid, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column02, dbo.Table_010_SaleFactor.column03, dbo.Table_010_SaleFactor.column09, 
                         dbo.Table_010_SaleFactor.column10, dbo.Table_010_SaleFactor.column13, dbo.Table_010_SaleFactor.column14
HAVING        (dbo.Table_010_SaleFactor.column02 >= '" + faDate1.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"')");
                        bindingSource1.DataSource = saleheader;

                        DataTable salechild = ClDoc.ReturnTable(ConSale, @"SELECT     *
FROM            dbo.Table_011_Child1_SaleFactor INNER JOIN
                         dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
WHERE        (dbo.Table_010_SaleFactor.column02 >='" + faDate1.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"')");
                        bindingSource2.DataSource = salechild;


                        DataTable salechild2 = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_010_SaleFactor.column02 AS date, dbo.Table_012_Child2_SaleFactor.*
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01
WHERE        (dbo.Table_010_SaleFactor.column02 >= '" + faDate1.FADatePicker.Text + @"' AND dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"')");
                        bindingSource3.DataSource = salechild2;
                    }
                    else
                    {
                        if (user.Rows.Count > 0)
                        {
                            if (user.Rows[0][0].ToString() == Class_BasicOperation._UserName)
                            {
                                DataTable saleheader = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_010_SaleFactor.columnid AS ID, dbo.Table_010_SaleFactor.column01 AS Number, dbo.Table_010_SaleFactor.column02 AS Date, 
                         dbo.Table_010_SaleFactor.column03 AS Kharidar, dbo.Table_010_SaleFactor.column09 AS NumberHavale, dbo.Table_010_SaleFactor.column10 AS DocId, 
                         dbo.Table_010_SaleFactor.column13 AS UserSabt, dbo.Table_010_SaleFactor.column14 AS DateTime, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column10) AS UnitPrice, SUM(dbo.Table_011_Child1_SaleFactor.column11) AS TotalPrice
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
GROUP BY dbo.Table_010_SaleFactor.columnid, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column02, dbo.Table_010_SaleFactor.column03, 
                         dbo.Table_010_SaleFactor.column09, dbo.Table_010_SaleFactor.column10, dbo.Table_010_SaleFactor.column13, 
                         dbo.Table_010_SaleFactor.column14
HAVING        (dbo.Table_010_SaleFactor.column02 >= '" + faDate1.FADatePicker.Text + @"' AND dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column13='" + Class_BasicOperation._UserName + "')");
                                bindingSource1.DataSource = saleheader;

                                DataTable salechild = ClDoc.ReturnTable(ConSale, @"SELECT     *
FROM            dbo.Table_011_Child1_SaleFactor INNER JOIN
                         dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
WHERE        (dbo.Table_010_SaleFactor.column02 >='" + faDate1.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column13='" + Class_BasicOperation._UserName + "')");
                                bindingSource2.DataSource = salechild;


                                DataTable salechild2 = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_010_SaleFactor.column02 AS date, dbo.Table_012_Child2_SaleFactor.*
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01
WHERE        (dbo.Table_010_SaleFactor.column02 >= '" + faDate1.FADatePicker.Text + @"' AND dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column13='" + Class_BasicOperation._UserName + "')");
                                bindingSource3.DataSource = salechild2;
                            }
                        }
                    }
                }



              catch (Exception ex)
                {

                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }

        private void tsexcel_Click(object sender, EventArgs e)
        {
            if (gridEX1.Focused)
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.GridEX = gridEX1;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            else if (gridEX_List.Focused)
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.GridEX = gridEX_List;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            else if (gridEX_Extra.Focused)
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.GridEX = gridEX_Extra;
                        printPreviewDialog1.ShowDialog();
                    }
            }
    }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (gridEX1.Focused)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gridEXExporter1.GridEX = gridEX1;
                    System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    gridEXExporter1.Export(File);
                    MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
                }
            }
            else if (gridEX_List.Focused)
            {
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    gridEXExporter2.GridEX = gridEX_List;
                    System.IO.FileStream File = (System.IO.FileStream)saveFileDialog2.OpenFile();
                    gridEXExporter2.Export(File);
                    MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
                }
            }

            else if (gridEX_Extra.Focused)
            {
                if (saveFileDialog3.ShowDialog() == DialogResult.OK)
                {
                    gridEXExporter3.GridEX = gridEX_Extra;
                    System.IO.FileStream File = (System.IO.FileStream)saveFileDialog3.OpenFile();
                    gridEXExporter3.Export(File);
                    MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
                }
            }
        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource1.Count>0)
                {
                bindingSource2.RemoveFilter();
                bindingSource2.Filter = " Column01 =" + ((DataRowView)bindingSource1.CurrencyManager.Current)["id"].ToString();
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void bindingSource2_PositionChanged(object sender, EventArgs e)
        {
            try
            {

                if (bindingSource1.Count > 0)
                {
                    bindingSource3.RemoveFilter();
                    bindingSource3.Filter = " Column01 =" + ((DataRowView)bindingSource1.CurrencyManager.Current)["ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void ribbonBarMergeContainer1_Click(object sender, EventArgs e)
        {

        }
        }
}
