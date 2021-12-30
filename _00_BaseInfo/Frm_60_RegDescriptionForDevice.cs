using Dapper;
using Janus.Windows.GridEX;
using PCLOR.Classes;
using PCLOR.Models;
using PCLOR.PCLOR_1_1400DataSetTableAdapters;
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
        Table_135_RpeortDevicesTableAdapter table_135_RpeortDevicesTableAdapter = new Table_135_RpeortDevicesTableAdapter();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        public Frm_60_RegDescriptionForDevice()
        {
            InitializeComponent();
        }


        private void Frm_60_RegDescriptionForDevice_Load(object sender, EventArgs e)
        {
            GridLoadDataSource(DeviceId);
            //txtDescForDevice.Text = GetDescForDevice(DeviceId);
            //try
            //{
            //    SaveDescriptioForDevie();
            //    MessageBox.Show("اظهارات با موفقیت ذخیره شد");
            //    this.Close();

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    this.Close();
            //}
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
        public bool ExistTagCode(string code)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                try
                {
                    var query = $@"
                SELECT 
                CASE 
                WHEN EXISTS (SELECT ColumnId FROM PBASE_1.dbo.Table_045_PersonInfo WHERE Column148 = N'{txtCodeTag.Text}')
                THEN 1 
                ELSE 0
                END 
                ";
                    return db.ExecuteScalar<bool>(query, null, commandType: CommandType.Text);
                }
                catch (Exception ex)
                {
                    return false;
                }


            }


        }
        public void GridLoadDataSource(int DeviceId)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
select  v.NameDevice  , v.Description , v.FullName,
v.TagCode,v.Date,v.CreateDate,v.IsDeffective , 
v.IsDeffectiveText,v.DeviceId
from V_ReportDevices as v
where DeviceId = {DeviceId}
                                ";
                var result = db.Query<ReportDeviceViewModel>(query, null, commandType: CommandType.Text);
                gridEX2.DataSource = result;
            }

        }
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
        public bool SaveDescriptioForDevie()
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var check = (checkIsDefective.Checked) ? 1 : 0;
                var query = $@" update Table_60_SpecsTechnical Set IsDefective= @IsDefective
                                where ID=  @ID ;
                            
INSERT INTO Table_135_RpeortDevices 
(DateCreate,Description,DeviceId,IsDeffective,TokenCode)
VALUES 
(N'{DateTime.Now.ToShamsi()}',N'{txtDescForDevice.Text}',{DeviceId},{check},N'{txtCodeTag.Text}')
";
                var status = db.Execute(query, new { @ID = DeviceId, @IsDefective = (checkIsDefective.Checked)?1:0 });
                if (status > 0)
                    return true;
                return false;
            }
        }

        public bool CheckIsNullDesc()
        {
            if (string.IsNullOrEmpty(txtDescForDevice.Text.Trim()))
                return false;
            return true;
        }

        public void RefreshPage()
        {
            txtCodeTag.Text = string.Empty;
            txtDescForDevice.Text = string.Empty;
            checkIsDefective.Checked = false;
            GridLoadDataSource(DeviceId);
        }
        ////////////////////////////////////////////////EndRepositoryForForm


        private void gridEX2_FormattingRow(object sender, RowLoadEventArgs e)
        {

        }

        private void txtCodeTag_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCodeTag_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (CheckIsNullDesc())
                {
                    if (ExistTagCode(txtCodeTag.Text.Trim()))
                    {
                        if (SaveDescriptioForDevie())
                        {
                            MessageBox.Show("ثبت با موفقیت انجام شد ");
                            RefreshPage();
                        }
                        else
                            MessageBox.Show("ثبت با موفقیت انجام نشد !!");
                    }
                    else
                        MessageBox.Show("کد تگ وارد شده معتبر نیست !");
                }
                else
                    MessageBox.Show("لطفا اظهارات مربوط به دستگاه را وارد نمائید");
            }
        }
    }
}
