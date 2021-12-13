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
    public partial class Frm_Report_Pack : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        public Frm_Report_Pack()
        {
            InitializeComponent();
        }

        private void Frm_Report_Pack_Load(object sender, EventArgs e)
        {
            //faDate1.FADatePicker.SelectedDateTime = faDate2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.date2.Split('-');
            faDate1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDate2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX1.RemoveFilters();
            gridEX1.UpdateData();
            gridEX2.RemoveFilters();
            gridEX2.UpdateData();

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (faDate1.FADatePicker.SelectedDateTime.HasValue && faDate2.FADatePicker.SelectedDateTime.HasValue)
            {
                try
                {

                    this.dataSet_05_PCLOR.EnforceConstraints = false;


                    DataTable PWHRS = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_010_TypeColor.ID, ISNULL(SUM(derivedtbl_1.InValue) - SUM(derivedtbl_1.OutValue), 0) AS Remain, 
                       " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS Code, " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS Name
                    FROM         dbo.Table_010_TypeColor INNER JOIN
                      dbo.Table_015_FormulColor ON dbo.Table_010_TypeColor.ID = dbo.Table_015_FormulColor.Fk INNER JOIN
                      dbo.Table_055_ColorDefinition ON dbo.Table_015_FormulColor.CodeColore = dbo.Table_055_ColorDefinition.ID INNER JOIN
                      " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      dbo.Table_055_ColorDefinition.CodeCommondity = " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                          (SELECT     " + ConPWHRS.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(" + ConPWHRS.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07) 
                                                   AS InValue, 0 AS OutValue
                            FROM          " + ConPWHRS.Database + @".dbo.Table_011_PwhrsReceipt INNER JOIN
                                                   " + ConPWHRS.Database + @".dbo.Table_012_Child_PwhrsReceipt ON 
                                                   " + ConPWHRS.Database + @".dbo.Table_011_PwhrsReceipt.columnid = " + ConPWHRS.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01
                            GROUP BY " + ConPWHRS.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02, " + ConPWHRS.Database + @".dbo.Table_011_PwhrsReceipt.column02
                            UNION ALL
                            SELECT     Table_008_Child_PwhrsDraft_1.column02 AS GoodCode, 0 AS InValue, SUM(Table_008_Child_PwhrsDraft_1.column07) AS OutValue
                            FROM         " + ConPWHRS.Database + @".dbo.Table_007_PwhrsDraft INNER JOIN
                                                  " + ConPWHRS.Database + @".dbo.Table_008_Child_PwhrsDraft AS Table_008_Child_PwhrsDraft_1 ON 
                                                  " + ConPWHRS.Database + @".dbo.Table_007_PwhrsDraft.columnid = Table_008_Child_PwhrsDraft_1.column01
                            GROUP BY Table_008_Child_PwhrsDraft_1.column02, " + ConPWHRS.Database + @".dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1 ON 
                      dbo.Table_055_ColorDefinition.CodeCommondity = derivedtbl_1.GoodCode
                GROUP BY " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01, dbo.Table_010_TypeColor.ID, 
                      " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02");

                    bindingSource1.DataSource = PWHRS;

                    DataTable Product = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_035_Production.Number, dbo.Table_035_Production.Date, dbo.Table_035_Production.NumberProduct, dbo.Table_010_TypeColor.ID, 
                      dbo.Table_005_TypeCloth.TypeCloth, " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS Code, dbo.Table_010_TypeColor.TypeColor
                    FROM         dbo.Table_035_Production INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID INNER JOIN
                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID INNER JOIN
                      dbo.Table_015_FormulColor ON dbo.Table_010_TypeColor.ID = dbo.Table_015_FormulColor.Fk INNER JOIN
                      dbo.Table_055_ColorDefinition ON dbo.Table_015_FormulColor.CodeColore = dbo.Table_055_ColorDefinition.ID INNER JOIN
                      " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      dbo.Table_055_ColorDefinition.CodeCommondity = " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                    WHERE     (dbo.Table_035_Production.Date >='" + faDate1.FADatePicker.Text + @"') AND (dbo.Table_035_Production.Date <='" + faDate2.FADatePicker.Text + @"')");

                    bindingSource2.DataSource = Product;

                    bindingSource1_PositionChanged(sender, e);

                }





                catch (Exception)
                {


                }
            }

        }

        private void tsexcel_Click(object sender, EventArgs e)
        {
            if (gridEX1.Focused)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gridEXExporter1.GridEX = gridEX1;
                    System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    gridEXExporter1.Export(File);
                    MessageBox.Show( "عملیات ارسال با موفقیت انجام گرفت");
                }
            }
                else if (gridEX2.Focused)
                {
                    if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                    {
                        gridEXExporter2.GridEX = gridEX2;
                        System.IO.FileStream File = (System.IO.FileStream)saveFileDialog2.OpenFile();
                        gridEXExporter2.Export(File);
                        MessageBox.Show( "عملیات ارسال با موفقیت انجام گرفت");
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
           
        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            try
            {


                bindingSource2.RemoveFilter();
                bindingSource2.Filter = " Code =" + ((DataRowView)bindingSource1.CurrencyManager.Current)["Code"].ToString();

            }
            catch (Exception)
            {

            }
        }

        private void bindingSource2_PositionChanged(object sender, EventArgs e)
        {
           
        }

        private void Frm_Report_Pack_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDate1.FADatePicker.SelectedDateTime.HasValue && faDate2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.date2 = faDate1.FADatePicker.Text + "-" + faDate2.FADatePicker.Text;
            Properties.Settings.Default.Save();
        }
    }
}

