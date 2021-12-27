using Dapper;
using PCLOR.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR._00_BaseInfo
{
    public partial class Frm_60_RegDescriptionForDevice : Form
    {
        public int DeviceId { get; set; }
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        public Frm_60_RegDescriptionForDevice(int Id)
        {
            DeviceId = Id;
            InitializeComponent();
        }
        public Frm_60_RegDescriptionForDevice()
        {
            InitializeComponent();
        }

        private void Frm_60_RegDescriptionForDevice_Load(object sender, EventArgs e)
        {
            txtDescForDevice.Text = GetDescForDevice(DeviceId);
            try
            {
                SaveDescriptioForDevie();
                MessageBox.Show("اظهارات با موفقیت ذخیره شد");
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            SaveDescriptioForDevie();
                MessageBox.Show("اظهارات با موفقیت ذخیره شد");
                this.Close(); 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }







        ////////////////////////////////////////////////RepositoryForForm
        public string GetDescForDevice(int Id) 
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = @" select [Description]
                                from  Table_60_SpecsTechnical where ID = @ID";
                var status = db.QueryFirstOrDefault<string>(query, new { @ID = DeviceId });
                return status;
            }

        }
        public void SaveDescriptioForDevie() 
        {

            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = @" update Table_60_SpecsTechnical Set Description = @Desc
                              where ID = @ID ";
                var status = db.Execute(query,new {@ID = DeviceId,@Desc = txtDescForDevice.Text});
            }


        }

    }
}
