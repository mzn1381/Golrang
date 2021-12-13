using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Report;
using System.Data.SqlClient;

namespace PCLOR.Report
{
    public partial class Frm_Report_BarcodeRecipt : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        string _Report;
        public Frm_Report_BarcodeRecipt(string NumberD)
        {
            _Report = NumberD;
            InitializeComponent();
        }

        private void Frm_Report_BarcodeRecipt_Load(object sender, EventArgs e)
        {

            DataTable barcodRecipt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_65_HeaderOtherPWHRS.ID, dbo.Table_70_DetailOtherPWHRS.FK, dbo.Table_70_DetailOtherPWHRS.ID AS IDD, 
                      dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_70_DetailOtherPWHRS.weight, 
                      dbo.Table_65_HeaderOtherPWHRS.Number, dbo.Table_65_HeaderOtherPWHRS.date, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column01 AS Costumer, 
                      " + ConPWHRS.Database + @".dbo.Table_001_PWHRS.column02 AS PWHRS, dbo.Table_65_HeaderOtherPWHRS.description, dbo.Table_70_DetailOtherPWHRS.TypeColor, 
                      dbo.Table_70_DetailOtherPWHRS.Machine, dbo.Table_65_HeaderOtherPWHRS.Recipt, 
                      " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS Commondity, dbo.Table_005_TypeCloth.Number AS NumberCloth, 
                      dbo.Table_035_Production.Number AS NumberProduct, dbo.Table_050_Packaging.Meter
                    FROM         dbo.Table_035_Production INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct RIGHT OUTER JOIN
                      dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_70_DetailOtherPWHRS.TypeCloth = dbo.Table_005_TypeCloth.ID INNER JOIN
                     " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dbo.Table_65_HeaderOtherPWHRS.Coustomer = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId INNER JOIN
                      " + ConPWHRS.Database + @".dbo.Table_001_PWHRS ON dbo.Table_65_HeaderOtherPWHRS.PWHRS = " + ConPWHRS.Database + @".dbo.Table_001_PWHRS.columnid INNER JOIN
                      " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients ON 
                      dbo.Table_70_DetailOtherPWHRS.CodeCommondity = " + ConPWHRS.Database + @".dbo.table_004_CommodityAndIngredients.columnid ON 
                      dbo.Table_050_Packaging.Barcode = dbo.Table_70_DetailOtherPWHRS.Barcode
                      where Table_70_DetailOtherPWHRS.Id in (" + _Report.TrimEnd(',') + ") ");


            byte[] data = (Byte[])Class_BasicOperation.LogoTable().Rows[0]["Column17"];
            System.IO.MemoryStream stream = new System.IO.MemoryStream(data);



            try
            {
                StiReport stireport = new StiReport();
                stireport.Load("barcodRecipt.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                stireport.RegData("barcodRecipt", barcodRecipt);
                stireport["Image"] = Image.FromStream(stream);
                this.Cursor = Cursors.Default;
                stireport.Render(false);
                stiViewerControl1.Report = stireport;

            }
            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex, this.Name); }
        }
    }
}
