using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Stimulsoft.Report;

namespace PCLOR
{
    public partial class Frm_01_ReportPackiging : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        string Number;
        string pagename;
       
        public Frm_01_ReportPackiging(string NumberOrder, string _pagename)
        {
           
            pagename = _pagename;
            Number = NumberOrder.TrimEnd(',');
            InitializeComponent();
        }

        private void Frm_01_ReportPackiging_Load(object sender, EventArgs e)
        {

            try
            {
                DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_050_Packaging.ID, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_010_TypeColor.TypeColor, dbo.Table_025_HederOrderColor.Number, 
                      dbo.Table_030_DetailOrderColor.Title, dbo.Table_050_Packaging.date, dbo.Table_050_Packaging.Time, dbo.Table_035_Production.Number AS NumberOrder, 
                      dbo.Table_050_Packaging.weight, dbo.Table_030_DetailOrderColor.Description, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Meter, 
                      dbo.Table_050_Packaging.Description AS DescriptionP, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column01 AS CodeCustomer
                      FROM         dbo.Table_050_Packaging INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_050_Packaging.IDProduct = dbo.Table_035_Production.ID INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID INNER JOIN
                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dbo.Table_025_HederOrderColor.CodeCustomer = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId
            WHERE     dbo.Table_050_Packaging.ID in(" + Number + ")");


                StiReport stireport = new StiReport();
                stireport.Load("Report.mrt");
                for (int i = 0; i < stireport.Pages.Count; i++)
                {
                    if (stireport.Pages[i].Name == pagename)
                        stireport.Pages[i].Enabled = true;
                    else
                        stireport.Pages[i].Enabled = false;
                }
                stireport.Compile();
                stireport.RegData("dt", dt);
                this.Cursor = Cursors.Default;
                stireport.Render(false);
                stiViewerControl1.Report = stireport;


            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }

   

        private void btn_Print_Click_1(object sender, EventArgs e)
        {
           
        }

    }
}
