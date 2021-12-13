using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;
using System.IO;
using DevComponents.DotNetBar;

namespace PCLOR._00_BaseInfo
{
    public partial class Setting_15_Weighbridge : Form
    {
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        public Setting_15_Weighbridge()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ClDoc.Execute(ConPCLOR.ConnectionString, "Update Table_80_Setting set value=N'"
                    + txt_Bund.Text + "' where id =12;Update Table_80_Setting set value=N'"
                    +txt_Port.Text+"' where id=13; Update Table_80_Setting set value=N'"+mlt_Print.Value+ "' where id=11");
               
               
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
            }
            catch { }
        }

      

        private void Setting_15_Weighbridge_Load(object sender, EventArgs e)
        {
           
           
        
            try
            {
                txt_Bund.Focus();
                DataTable dt = ClDoc.ReturnTable(ConPCLOR, "select * from Table_80_Setting");
                txt_Bund.Text = dt.Rows[11][2].ToString();
                txt_Port.Text = dt.Rows[12][2].ToString();
                mlt_Print.Value = dt.Rows[10][2].ToString();

                DataTable Table = new DataTable();
                Table.Columns.Add("Name", Type.GetType("System.String"));

                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    Table.Rows.Add(printer);
                }
                mlt_Print.DataSource = Table;
                mlt_Print.Value = dt.Rows[10][2].ToString();
            }
            catch
            {

            }
        }
    }
}
