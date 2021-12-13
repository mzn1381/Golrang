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
    public partial class Frm_003_ViewFactorSale : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        DataSet DS = new DataSet();

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        public Frm_003_ViewFactorSale()
        {
            InitializeComponent();
        }

        private void Frm_003_ViewFactorSale_Load(object sender, EventArgs e)
        {

            string[] Dates = Properties.Settings.Default.date1.Split('-');
            faDate1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDate2.FADatePicker.SelectedDateTime = DateTime.Now;


            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);

            gridEX_List.DropDowns["Codecommodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
            gridEX_List.DropDowns["Namecommodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column02 from table_004_CommodityAndIngredients");
            gridEX_List.DropDowns["UnitCount"].DataSource = ClDoc.ReturnTable(ConBase, @"select Column00,Column01 from Table_070_CountUnitInfo");
            gridEX1.DropDowns["ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS");
            gridEX1.DropDowns["Operationware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from table_005_PwhrsOperation");
            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");
            gridEX1.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column01,Column02 from Table_045_PersonInfo");
            
            gridEX_List.UpdateData();
            gridEX_Extra.UpdateData();
            gridEX1.UpdateData();


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
            if (faDate1.FADatePicker.SelectedDateTime.HasValue && faDate2.FADatePicker.SelectedDateTime.HasValue && faDate1.FADatePicker.Text.Length == 10 && faDate2.FADatePicker.Text.Length == 10)
            {
                try
                {


                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable user = ClDoc.ReturnTable(ConSale, @"select column13 from Table_010_SaleFactor  where Column42 IN (select Ware from " + ConPCLOR.Database + @".dbo.Table_95_DetailWare) AND Column13='" + Class_BasicOperation._UserName + "'");
                    DataTable id = ClDoc.ReturnTable(ConSale, @"select columnid from Table_010_SaleFactor  where   'True'='" + isadmin.ToString() + @"'");
                    DataTable idUser = ClDoc.ReturnTable(ConSale, @"select columnid from Table_010_SaleFactor  where    Column42 in(select Ware from " + ConPCLOR.Database + @".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + @".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"') AND Column13= N'" + Class_BasicOperation._UserName + @"')  ");
                    string Usersabt = Class_BasicOperation._UserName;

                    if (isadmin)
                    {


                        dataSet_01_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_Date_Admin(this.dataSet_01_Sale.Table_010_SaleFactor, faDate1.FADatePicker.Text, faDate2.FADatePicker.Text);
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_Date_Admin(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, faDate1.FADatePicker.Text, faDate2.FADatePicker.Text);
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_Date_Admin(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, faDate1.FADatePicker.Text, faDate2.FADatePicker.Text);
                        dataSet_01_Sale.EnforceConstraints = true;


                    }
                    else
                    {
                        if (user.Rows[0][0].ToString() == Class_BasicOperation._UserName)
                        {

                            dataSet_01_Sale.EnforceConstraints = false;
                            this.table_010_SaleFactorTableAdapter.FillBy_Date(this.dataSet_01_Sale.Table_010_SaleFactor, faDate1.FADatePicker.Text, faDate2.FADatePicker.Text, Usersabt);
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_Date(this.dataSet_01_Sale.Table_011_Child1_SaleFactor, faDate1.FADatePicker.Text, faDate2.FADatePicker.Text, Usersabt);
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_Date(this.dataSet_01_Sale.Table_012_Child2_SaleFactor, faDate1.FADatePicker.Text, faDate2.FADatePicker.Text, Usersabt);
                            dataSet_01_Sale.EnforceConstraints = true;


                        }

                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }
        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.PSALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_010_SaleFactor where column01=" + FactorNum, con);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره فاکتور وارد شده نامعتبر است");
                }
            }
        }
        private void table_010_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (table_010_SaleFactorBindingSource.Count>0)
                {
                    
               
                table_011_Child1_SaleFactorBindingSource.RemoveFilter();
                table_011_Child1_SaleFactorBindingSource.Filter = " Column01 =" + ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Columnid"].ToString();
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void table_011_Child1_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {

                if (table_010_SaleFactorBindingSource.Count > 0)
                {
                    table_012_Child2_SaleFactorBindingSource.RemoveFilter();
                    table_012_Child2_SaleFactorBindingSource.Filter = " Column01 =" + ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["Columnid"].ToString();
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX1.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 70))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_StoreFaktor")
                            {
                                item.BringToFront();
                                _002_Sale.Frm_002_StoreFaktor frm = (_002_Sale.Frm_002_StoreFaktor)item;
                                frm.txt_Search.Text = gridEX1.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Frm_002_StoreFaktor frms = new Frm_002_StoreFaktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21), Convert.ToInt32(gridEX1.GetValue("columnid")),true);
                        try
                        {
                            frms.MdiParent = Frm_Main.ActiveForm;
                        }
                        catch { }
                        frms.Show();
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

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

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            btnEdit_Click(sender, e);
        }






    }
}
