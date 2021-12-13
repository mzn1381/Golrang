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
    public partial class Frm_Rpt_SumSaleFi : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Frm_Rpt_SumSaleFi()
        {
            InitializeComponent();
        }

        private void gridEX2_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void Frm_Rpt_SumSaleFi_Load(object sender, EventArgs e)
        {
            gridEX2.DataSource = ClDoc.ReturnTable(ConWHRS, @"select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            faDate1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDate2.FADatePicker.SelectedDateTime = DateTime.Now;
            gridEX1.RemoveFilters();
            gridEX1.UpdateData();
            gridEX2.RemoveFilters();
            gridEX2.UpdateData();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.DataSource = ClDoc.ReturnTable(ConSale, @"SELECT        SUM(dbo.Table_011_Child1_SaleFactor.column11) AS Fitotal, SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Totalwight, dbo.Table_010_SaleFactor.column02 AS Date, 
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS NamGood, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS CodeGood, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Remain,  SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS FiSale
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON dbo.Table_011_Child1_SaleFactor.column02 = " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid
GROUP BY dbo.Table_010_SaleFactor.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01
HAVING        (dbo.Table_010_SaleFactor.column02 >= N'" + faDate1.FADatePicker.Text + "') AND  (dbo.Table_010_SaleFactor.column02 <= N'" + faDate2.FADatePicker.Text + "')  And (" + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 = N'" + gridEX2.GetValue("Column01") + "')");

                if (check_B.Checked == true)
                {
                    gridEX1.DataSource = ClDoc.ReturnTable(ConSale, @"SELECT        SUM(dbo.Table_011_Child1_SaleFactor.column11) AS Fitotal, SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Totalwight, dbo.Table_010_SaleFactor.column02 AS Date, 
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS NamGood, PWHRS_5_1398.dbo.table_004_CommodityAndIngredients.column01 AS CodeGood, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Remain,  SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS FiSale, dbo.Table_011_Child1_SaleFactor.Column38 AS Brand
            
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON dbo.Table_011_Child1_SaleFactor.column02 = " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid
GROUP BY dbo.Table_010_SaleFactor.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01, 
                         dbo.Table_011_Child1_SaleFactor.Column38
HAVING        (dbo.Table_010_SaleFactor.column02 >=  N'" + faDate1.FADatePicker.Text + "') AND (dbo.Table_010_SaleFactor.column02 <= N'" + faDate2.FADatePicker.Text + "') AND (" + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 = N'" + gridEX2.GetValue("Column01") + "')");
                }


                if (check_T.Checked == true)
                {
                    gridEX1.DataSource = ClDoc.ReturnTable(ConSale, @"SELECT        SUM(dbo.Table_011_Child1_SaleFactor.column11) AS Fitotal, SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Totalwight, dbo.Table_010_SaleFactor.column02 AS Date, 
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS NamGood, PWHRS_5_1398.dbo.table_004_CommodityAndIngredients.column01 AS CodeGood, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Remain,  SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS FiSale, dbo.Table_011_Child1_SaleFactor.Column39 As Tamin
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON dbo.Table_011_Child1_SaleFactor.column02 = " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid
GROUP BY dbo.Table_010_SaleFactor.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01, 
                          dbo.Table_011_Child1_SaleFactor.Column39
HAVING        (dbo.Table_010_SaleFactor.column02 >=  N'" + faDate1.FADatePicker.Text + "') AND (dbo.Table_010_SaleFactor.column02 <= N'" + faDate2.FADatePicker.Text + "') AND (" + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 = N'" + gridEX2.GetValue("Column01") + "')");
                }

                if (check_T.Checked == true && check_B.Checked == true)
                {
                    gridEX1.DataSource = ClDoc.ReturnTable(ConSale, @"SELECT        SUM(dbo.Table_011_Child1_SaleFactor.column11) AS Fitotal, SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Totalwight, dbo.Table_010_SaleFactor.column02 AS Date, 
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS NamGood, PWHRS_5_1398.dbo.table_004_CommodityAndIngredients.column01 AS CodeGood, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS Remain,  SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.Column37) AS FiSale, dbo.Table_011_Child1_SaleFactor.Column39 As Tamin, dbo.Table_011_Child1_SaleFactor.Column38 AS Brand
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                         " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON dbo.Table_011_Child1_SaleFactor.column02 = " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid
GROUP BY dbo.Table_010_SaleFactor.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column02, " + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01, 
                          dbo.Table_011_Child1_SaleFactor.Column39, dbo.Table_011_Child1_SaleFactor.Column38
HAVING        (dbo.Table_010_SaleFactor.column02 >=  N'" + faDate1.FADatePicker.Text + "') AND (dbo.Table_010_SaleFactor.column02 <= N'" + faDate2.FADatePicker.Text + "') AND (" + ConWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 = N'" + gridEX2.GetValue("Column01") + "')");
                }

            }
            catch { }


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

        private void gridEX2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btn_Search_Click(sender, e);
            }
        }

    }
}
