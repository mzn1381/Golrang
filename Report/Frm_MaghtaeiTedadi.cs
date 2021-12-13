using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Stimulsoft.Report.Components;
using Janus.Windows.GridEX;

namespace PCLOR.Report
{
    public partial class Frm_MaghtaeiTedadi : Form
    {
        bool _BackSpace = false;
        string Date1;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        public Frm_MaghtaeiTedadi()
        {
            InitializeComponent();
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            if (mlt_Ware.DropDownList.GetCheckedRows().Count() == 0)
            {
                MessageBox.Show("انبار مورد نظر را انتخاب کنید");
                return;
            }

            if (!faDate1.FADatePicker.SelectedDateTime.HasValue)
            {
                MessageBox.Show("تاریخ مورد نظر را وارد کنید");
                return;
            }

            Date1 = null;
            Date1 = faDate1.FADatePicker.Text;

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

                    SaleRial = clDoc.ReturnTable(ConPCLOR, @"
select T.TypeCloth, T.Barcode, T.Weighth, T.FiSale, SUM(T.CountInput) AS CountInput, SUM(T.CountOut) AS CountOut, SUM(T.CountInput) - SUM(T.CountOut) AS Remain, T.Brand, T.Tamin,T.PWHRS,
                      T.CodeCommondity, Table_005_TypeCloth_2.SelectBrand
                      , case when selectbrand=1 then  (isnull((Select Ficolor from Table_010_TypeColor where TypeColor=T.Brand   ),0)+T.FiSale) * T .Weighth else T.FiSale*T.Weighth end  SumFisale
                       from (
SELECT     Table_005_TypeCloth_1.TypeCloth, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 AS Barcode, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column03 AS PWHRS, 
                      SUM(" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07) AS CountInput, 0 AS CountOut, SUM(" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07) - 0 AS Remain, 
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 AS dateinput, Table_005_TypeCloth_1.FiSale, SUM(" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column35) AS Weighth, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 AS Brand, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37 AS Tamin, 
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column01 AS NumR, 0 AS NumD, Table_050_Packaging_1.IDProduct, Table_005_TypeCloth_1.CodeCommondity
FROM         " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid INNER JOIN
                      dbo.Table_005_TypeCloth AS Table_005_TypeCloth_1 ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = Table_005_TypeCloth_1.CodeCommondity INNER JOIN
                      dbo.Table_050_Packaging AS Table_050_Packaging_1 ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 = Table_050_Packaging_1.Barcode
GROUP BY Table_005_TypeCloth_1.TypeCloth, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column03, 
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02, Table_005_TypeCloth_1.FiSale, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column01, Table_050_Packaging_1.IDProduct, 
                      Table_005_TypeCloth_1.CodeCommondity
HAVING      (" + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column03 IN (" + whr + @")) AND (" + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 <= N'" + Date1 + @"') AND 
                      (" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 <> N'0')
              union        
                      SELECT     dbo.Table_005_TypeCloth.TypeCloth, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 AS Barcode, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column03 AS PWHRS, 
                                              0 AS CountInput, SUM(" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column07) AS CountOut, 0 - SUM(" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column07) AS Remain, 
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column02 AS dateout, dbo.Table_005_TypeCloth.FiSale, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column35 AS Weighth, 
                                              " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36 AS Brand, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column37 AS Tamin, 0 AS NumR, 
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01 AS NumD, dbo.Table_050_Packaging.IDProduct, dbo.Table_005_TypeCloth.CodeCommondity
                       FROM          " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft INNER JOIN
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column01 = " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid INNER JOIN
                                              dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 = dbo.Table_005_TypeCloth.CodeCommondity INNER JOIN
                                              dbo.Table_050_Packaging ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 = dbo.Table_050_Packaging.Barcode
                       GROUP BY dbo.Table_005_TypeCloth.TypeCloth, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column03, 
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column02, dbo.Table_005_TypeCloth.FiSale, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column35, 
                                              " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column37, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01, 
                                              dbo.Table_050_Packaging.IDProduct, dbo.Table_005_TypeCloth.CodeCommondity
                       HAVING      (" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 <> N'0') AND (" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column03 IN (" + whr + @")) AND 
                                              (" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column02 <= N'" + Date1 + @"')
                                              
                                               ) AS T LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth AS Table_005_TypeCloth_2 ON T.CodeCommondity = Table_005_TypeCloth_2.CodeCommondity
                      
                    
                      
                      GROUP BY T.Barcode, T.Brand, T.FiSale, T.TypeCloth, T.Tamin,  T.CodeCommondity, Table_005_TypeCloth_2.SelectBrand,  T.Weighth,T.PWHRS
                      ,SumFi


                         ");
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
                   

                    string whr1 = string.Empty;

                    foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Ware.DropDownList.GetRows())
                    {

                        whr1 += dr.Cells["ColumnId"].Value + ",";


                    }
                    whr1 = whr1.TrimEnd(',');
                    DataTable SaleRial2 = new DataTable();

                    SaleRial2 = clDoc.ReturnTable(ConPCLOR, @"select T.TypeCloth, T.Barcode, T.Weighth, T.FiSale, SUM(T.CountInput) AS CountInput, SUM(T.CountOut) AS CountOut, SUM(T.CountInput) - SUM(T.CountOut) AS Remain, T.Brand, T.Tamin,T.PWHRS,
                      T.CodeCommondity, Table_005_TypeCloth_2.SelectBrand
                      , case when selectbrand=1 then  (isnull((Select Ficolor from Table_010_TypeColor where TypeColor=T.Brand   ),0)+T.FiSale) * T .Weighth else T.FiSale*T.Weighth end  SumFisale
                       from (
SELECT     Table_005_TypeCloth_1.TypeCloth, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 AS Barcode, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column03 AS PWHRS, 
                      SUM(" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07) AS CountInput, 0 AS CountOut, SUM(" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column07) - 0 AS Remain, 
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 AS dateinput, Table_005_TypeCloth_1.FiSale, SUM(" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column35) AS Weighth, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36 AS Brand, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37 AS Tamin, 
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column01 AS NumR, 0 AS NumD, Table_050_Packaging_1.IDProduct, Table_005_TypeCloth_1.CodeCommondity
FROM         " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt INNER JOIN
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column01 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid INNER JOIN
                      dbo.Table_005_TypeCloth AS Table_005_TypeCloth_1 ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.column02 = Table_005_TypeCloth_1.CodeCommondity INNER JOIN
                      dbo.Table_050_Packaging AS Table_050_Packaging_1 ON " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 = Table_050_Packaging_1.Barcode
GROUP BY Table_005_TypeCloth_1.TypeCloth, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column03, 
                      " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02, Table_005_TypeCloth_1.FiSale, " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column36, 
                      " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column37, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column01, Table_050_Packaging_1.IDProduct, 
                      Table_005_TypeCloth_1.CodeCommondity
HAVING      (" + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 <= N'" + Date1 + @"') AND 
                      (" + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt.Column30 <> N'0')
              union        
                      SELECT     dbo.Table_005_TypeCloth.TypeCloth, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 AS Barcode, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column03 AS PWHRS, 
                                              0 AS CountInput, SUM(" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column07) AS CountOut, 0 - SUM(" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column07) AS Remain, 
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column02 AS dateout, dbo.Table_005_TypeCloth.FiSale, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column35 AS Weighth, 
                                              " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36 AS Brand, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column37 AS Tamin, 0 AS NumR, 
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01 AS NumD, dbo.Table_050_Packaging.IDProduct, dbo.Table_005_TypeCloth.CodeCommondity
                       FROM          " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft INNER JOIN
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column01 = " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid INNER JOIN
                                              dbo.Table_005_TypeCloth ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 = dbo.Table_005_TypeCloth.CodeCommondity INNER JOIN
                                              dbo.Table_050_Packaging ON " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 = dbo.Table_050_Packaging.Barcode
                       GROUP BY dbo.Table_005_TypeCloth.TypeCloth, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column03, 
                                              " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column02, dbo.Table_005_TypeCloth.FiSale, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column35, 
                                              " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column36, " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column37, " + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column01, 
                                              dbo.Table_050_Packaging.IDProduct, dbo.Table_005_TypeCloth.CodeCommondity
                       HAVING      (" + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.Column30 <> N'0') AND 
                                              (" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.column02 <= N'" + Date1 + @"')
                                              
                                               ) AS T LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth AS Table_005_TypeCloth_2 ON T.CodeCommondity = Table_005_TypeCloth_2.CodeCommondity
                      
                    
                      
                      GROUP BY T.Barcode, T.Brand, T.FiSale, T.TypeCloth, T.Tamin,  T.CodeCommondity, Table_005_TypeCloth_2.SelectBrand,  T.Weighth,T.PWHRS
                      ,SumFi


                         
                      ");
                    bindingSource1.DataSource = SaleRial2;



                }
                catch (System.Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
           


        

        }
    }

        private void Frm_MaghtaeiTedadi_Load(object sender, EventArgs e)
        {
          
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            DataTable WareTable = clDoc.ReturnTable(ConWare, @"select 0 as columnid,0 as column01,N'همه انبارها' as  column02
union all
Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                                @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
            mlt_Ware.DropDownDataSource = WareTable;
                //DataRow Row = WareTable.NewRow();
                //Row["ColumnId"] = 0;
                //Row["Column02"] = "همه انبارها";
                //WareTable.Rows.InsertAt(Row, 0);
                mlt_Ware.DropDownDataSource = WareTable;
                mlt_Ware.DropDownList.SetValue("ColumnId", 0);
                //string[] Dates = Properties.Settings.Default.date1.Split('-');
                faDate1.FADatePicker.SelectedDateTime = DateTime.Now;
            gridEX1.DropDowns["IDProduct"].DataSource = clDoc.ReturnTable(ConPCLOR, @"select ID,Number from Table_035_Production");
            
        }


        private void FaDate2_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void FaDate2_TextChanged(object sender, EventArgs e)
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

        private void FaDate1_TextChanged(object sender, EventArgs e)
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

        private void FaDate1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Tsexcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXExporter1.GridEX = gridEX1;
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
            }
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    gridEXPrintDocument1.GridEX = gridEX1;
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void faDate2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void faDate2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tsexcel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
