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
    public partial class Frm_Report_Order : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        public Frm_Report_Order()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (faDate1.FADatePicker.SelectedDateTime.HasValue && faDate2.FADatePicker.SelectedDateTime.HasValue)
            {
                try
                {

                    DataTable Order = ClDoc.ReturnTable(ConPCLOR, @"SELECT DISTINCT 
                      dbo.Table_030_DetailOrderColor.NumberOrder - ISNULL(SUM(dbo.Table_035_Production.NumberProduct), 0) AS Remini, 
                      dbo.Table_025_HederOrderColor.Number AS NumOrder, dbo.Table_030_DetailOrderColor.NumberOrder AS Contorder, dbo.Table_025_HederOrderColor.Date, 
                      dbo.Table_025_HederOrderColor.ID, dbo.Table_030_DetailOrderColor.TypeColor, dbo.Table_030_DetailOrderColor.TypeColth, 
                      dbo.Table_025_HederOrderColor.CodeCustomer
                    FROM          dbo.Table_030_DetailOrderColor RIGHT OUTER JOIN
                                          dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID LEFT OUTER JOIN
                                          dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId
                    GROUP BY    dbo.Table_030_DetailOrderColor.ID, dbo.Table_025_HederOrderColor.Number, dbo.Table_030_DetailOrderColor.NumberOrder, 
                                          dbo.Table_025_HederOrderColor.Date, dbo.Table_025_HederOrderColor.ID, dbo.Table_030_DetailOrderColor.TypeColor, dbo.Table_030_DetailOrderColor.TypeColth, 
                      dbo.Table_025_HederOrderColor.CodeCustomer
                    HAVING      dbo.Table_025_HederOrderColor.Date >= '" + faDate1.FADatePicker.Text + @"' AND    dbo.Table_025_HederOrderColor.Date <= '" + faDate2.FADatePicker.Text + @"'");
                    bindingSource1.DataSource = Order;

                    DataTable Product = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_035_Production.Number, dbo.Table_035_Production.NumberProduct, dbo.Table_025_HederOrderColor.Number AS NumOrder, 
                      dbo.Table_025_HederOrderColor.ID, dbo.Table_025_HederOrderColor.Date, dbo.Table_035_Production.weight, ISNULL
                          ((SELECT     SUM(weight) AS weight
                              FROM         dbo.Table_050_Packaging
                              WHERE     (IDProduct = dbo.Table_035_Production.ID)), 0) AS Sumpack, dbo.Table_030_DetailOrderColor.weight - ISNULL
                          ((SELECT     SUM(weight) AS weight
                              FROM         dbo.Table_050_Packaging AS Table_050_Packaging_1
                              WHERE     (IDProduct = dbo.Table_035_Production.ID)), 0) AS diff, dbo.Table_035_Production.ID AS IdProduct, dbo.Table_030_DetailOrderColor.TypeColth, 
                      dbo.Table_030_DetailOrderColor.TypeColor
                    FROM                     dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId
                      WHERE     dbo.Table_025_HederOrderColor.Date >= '" + faDate1.FADatePicker.Text + @"' AND    dbo.Table_025_HederOrderColor.Date <= '" + faDate2.FADatePicker.Text + @"'");

                    bindingSource2.DataSource = Product;

                    DataTable Packaging = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_050_Packaging.IDProduct, dbo.Table_025_HederOrderColor.Number AS NumOrder, dbo.Table_050_Packaging.date, dbo.Table_050_Packaging.weight, 
                      dbo.Table_035_Production.NumberProduct, dbo.Table_025_HederOrderColor.ID, dbo.Table_025_HederOrderColor.Date AS date, 
                      dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.TypeColor, dbo.Table_030_DetailOrderColor.TypeColth
                        FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                WHERE    dbo.Table_025_HederOrderColor.Date >= '" + faDate1.FADatePicker.Text + @"' AND  dbo.Table_025_HederOrderColor.Date <= '" + faDate2.FADatePicker.Text + @"'");
                    bindingSource3.DataSource = Packaging;

                    bindingSource1_PositionChanged(sender, e);
                    bindingSource2_PositionChanged(sender, e);

                }

                catch (Exception)
                {


                }
            }
        }

        private void Frm_Report_Order_Load(object sender, EventArgs e)
        {

            try
            {
                string[] Dates = Properties.Settings.Default.date1.Split('-');
                faDate1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
                faDate2.FADatePicker.SelectedDateTime = DateTime.Now;

                gridEX1.DropDowns["TypeColor"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,TypeColor from Table_010_TypeColor");
                gridEX1.DropDowns["TypeCloth"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,TypeCloth from Table_005_TypeCloth ");
                gridEX1.DropDowns["Customer"].DataSource = ClDoc.ReturnTable(ConBase, @"select columnId, column01 from Table_045_PersonInfo");

                gridEX1.RemoveFilters();
                gridEX1.UpdateData();
            }
            
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
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

        private void tsexcel_Click(object sender, EventArgs e)
        {
            if (gridEX1.Focused)
	        {
		     if (saveFileDialog1.ShowDialog() == DialogResult.OK )
            {
                gridEXExporter1.GridEX = gridEX1;
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
            }
	        }
            else if (gridEX2.Focused)
            {
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    gridEXExporter2.GridEX = gridEX2;
                    System.IO.FileStream File = (System.IO.FileStream)saveFileDialog2.OpenFile();
                    gridEXExporter2.Export(File);
                    MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
                }
            }
            else if (gridEX3.Focused)
            {
                if (saveFileDialog3.ShowDialog() == DialogResult.OK)
                {
                    gridEXExporter3.GridEX = gridEX3;
                    System.IO.FileStream File = (System.IO.FileStream)saveFileDialog3.OpenFile();
                    gridEXExporter3.Export(File);
                    MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
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
            else if (gridEX2.Focused)
            {
                 if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.GridEX = gridEX2;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            else if (gridEX3.Focused)
            {
                 if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.GridEX = gridEX3;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            
        }


        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            try
            {

                bindingSource2.RemoveFilter();
                bindingSource2.Filter = " ID =" + ((DataRowView)bindingSource1.CurrencyManager.Current)["ID"].ToString();
               

            }
            catch (Exception)
            {

            }
        }

        private void bindingSource3_PositionChanged(object sender, EventArgs e)
        {
           

        }

        private void bindingSource2_PositionChanged(object sender, EventArgs e)
        {
            try
            {

               
                bindingSource3.RemoveFilter();
                bindingSource3.Filter = " IDProduct =" + ((DataRowView)bindingSource2.CurrencyManager.Current)["IDProduct"].ToString();

            }
            catch (Exception)
            {

            }
        }

        private void Frm_Report_Order_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDate1.FADatePicker.SelectedDateTime.HasValue && faDate2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.date1 = faDate1.FADatePicker.Text + "-" + faDate2.FADatePicker.Text;
            Properties.Settings.Default.Save();
        }

    }
}
