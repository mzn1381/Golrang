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
    public partial class Frm_Rpt_BarcodeDetail : Form
    {

        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Frm_Rpt_BarcodeDetail()
        {
            InitializeComponent();
        }

        private void Frm_Rpt_BarcodeDetail_Load(object sender, EventArgs e)
        {
            gridEX1.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT        dbo.Table_035_Production.Number, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.weight, dbo.Table_050_Packaging.TypeColor, dbo.Table_050_Packaging.Machine, 
                         dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_050_Packaging.date
FROM            dbo.Table_035_Production INNER JOIN
                         dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                         dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID INNER JOIN
                         dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID
WHERE        (dbo.Table_050_Packaging.NumberRecipt = 0)");

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
        }
    }
}
