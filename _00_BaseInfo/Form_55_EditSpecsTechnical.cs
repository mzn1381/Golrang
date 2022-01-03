using Dapper;
using PCLOR.Models;
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
    public partial class Form_55_EditSpecsTechnical : Form
    {

        private int DeviceId { get; set; } = 0;
        public Form_55_EditSpecsTechnical(int deviceId)
        {
            DeviceId = deviceId;
            InitializeComponent();
        }

        public Form_55_EditSpecsTechnical()
        {
            InitializeComponent();
        }

        private void Form_55_EditSpecsTechnical_Load(object sender, EventArgs e)
        {
            if (DeviceId == 0)
            {
                throw new Exception("Id Should isn`t 0");
            }
            using (IDbConnection db = new SqlConnection(Properties.Settings.Default.PCLOR))
            {
                string querySelectDevice = $@" select top (1)  [ID] ,  [Code] ,[namemachine]
                                        ,[Specstechnical]
                                        ,[status]
                                        ,[X]
                                        ,[Y]
                                        ,[DeviceMark]
                                        ,[Gap]
                                        ,[teeny]
                                        ,[Area]
                                        ,[YarnType]
                                        ,[RoundStop]
                                        ,[TextureLimit]
                                        ,[Description]
                                        ,[FabricType]
                                        ,[IsInfinitiveTextureLimit]
                                        ,[IsDeffective]
                                        ,[Speed]
                                    from Table_60_SpecsTechnical 
                                    where ID =  @DeviceId";
                string querySelectFabricType = @"
                                 select ID , TypeCloth
                                 from Table_005_TypeCloth";
                string querySelectYarnType = $@"
                                select ID , NameCotton
                                from Table_120_TypeCotton";
                var device = db.QueryFirstOrDefault<Machine>(querySelectDevice, param: new { @DeviceId = DeviceId }, commandType: CommandType.Text);
                txtDeciveMark.Text = device.DeviceMark;
                txtDeviceCode.Text = device.Code.Value.ToString();
                numericGap.Value = device.Gap;
                listFabricType.Value = device.FabricType;
                listYarnType.Value = device.YarnType;
                numericArea.Value = device.Area;
                txtDeviceName.Text = device.NameMachine;
                numericTeeny.Value = device.teeny;
                numericRoundStop.Value = device.RoundStop;
                numericTextureLimit.Value = device.TextureLimit;
                txtSpecstechnical.Text = device.Specstechnical;
                txtDeviceDescription.Text = device.Description;
                checkIsInfinitiveTextureLimit.Checked = device.IsInfinitiveTextureLimit;
                Status.Checked = device.Status;
                listFabricType.DataSource = db.Query<FabricTypeDropDownViewModel>(querySelectFabricType, commandType: CommandType.Text);
                listYarnType.DataSource = db.Query<YarnTypeDropDownViewModel>(querySelectYarnType, commandType: CommandType.Text);
                checkIsDeffective.Checked = device.IsInfinitiveTextureLimit;
                numericSpeed.Value = device.Speed;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDeviceName_Click(object sender, EventArgs e)
        {

        }

        private void txtDeviceCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listFabricType_KeyUp(object sender, KeyEventArgs e)
        {
            using (IDbConnection db = new SqlConnection(Properties.Settings.Default.PCLOR))
            {
                if (!string.IsNullOrEmpty(listFabricType.Text))
                {
                    var searchName = listFabricType.Text.Trim();
                    var query = $@"
                                 select ID , TypeCloth
                                 from   Table_005_TypeCloth
                                 where   TypeCloth   like    @Name  ";
                    searchName = "%" + searchName + "%";
                    listFabricType.DataSource = db.Query<FabricTypeDropDownViewModel>(query, param: new { @Name = searchName }, commandType: CommandType.Text);
                }
                else
                {

                    string querySelectFabricType = @"
                                 select ID , TypeCloth
                                 from Table_005_TypeCloth";
                    listFabricType.DataSource = db.Query<FabricTypeDropDownViewModel>(querySelectFabricType, commandType: CommandType.Text);
                }
            }
        }

        private void listYarnType_KeyUp(object sender, KeyEventArgs e)
        {
            using (IDbConnection db = new SqlConnection(Properties.Settings.Default.PCLOR))
            {
                if (!string.IsNullOrEmpty(listYarnType.Text))
                {
                    var searchName = listYarnType.Text.Trim();
                    var query = @"
                                 select ID , NameCotton
                                 from   Table_120_TypeCotton
                                 where   NameCotton   like    @Name  ";
                    searchName = "%" + searchName + "%";
                    listYarnType.DataSource = db.Query<YarnTypeDropDownViewModel>(query, param: new { @Name = searchName }, commandType: CommandType.Text);
                }
                else
                {

                    string querySelectYarnType = @"
                                 select ID , NameCotton
                                 from Table_120_TypeCotton";
                    listYarnType.DataSource = db.Query<YarnTypeDropDownViewModel>(querySelectYarnType, commandType: CommandType.Text);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                var queryUpdate = @"
                update
                Table_60_SpecsTechnical 
                Set 
                namemachine = @name ,
                Specstechnical = @specsTechnical ,
                status = @status , 
                DeviceMark = @deviceMark , 
                Gap = @gap ,
                teeny = @teeny , 
                Area = @area ,
                YarnType = @yarnType ,
                RoundStop = @roundStop ,               
                TextureLimit = @textureLimit , 
                Description = @description , 
                FabricType = @fabricType,
                IsInfinitiveTextureLimit = @isInfinitiveTextureLimit,
                IsDeffective=@IsDeffective,
                Speed = @Speed
                where ID = @Id";
                using (IDbConnection db = new SqlConnection(Properties.Settings.Default.PCLOR))
                {
                    var sTatus = db.Execute(queryUpdate, param: new
                    {
                        @name = txtDeviceName.Text,
                        @specsTechnical = txtSpecstechnical.Text,
                        @status = Status.Checked,
                        @deviceMark = txtDeciveMark.Text,
                        @gap = numericGap.Value,
                        @teeny = numericTeeny.Value,
                        @area = numericArea.Value,
                        @yarnType = listYarnType.Value,
                        @roundStop = numericRoundStop.Value,
                        @textureLimit = numericTextureLimit.Value,
                        @description = txtDeviceDescription.Text,
                        @fabricType = listFabricType.Value,
                        @Id = DeviceId,
                        @isInfinitiveTextureLimit = checkIsInfinitiveTextureLimit.Checked,
                        @IsDeffective = checkIsDeffective.Checked,
                        @Speed = numericSpeed.Value
                    }, commandType: CommandType.Text);
                }
                MessageBox.Show("تغیرات با موفقیت ذخیره شد");
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("اسم دستگاه وارد شده تکراری می باشد ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ذخیره تغیرات با شکست مواجه شد");
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }

        private void checkIsInfinitiveTextureLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (checkIsInfinitiveTextureLimit.Checked)
                numericTextureLimit.Enabled = false;
            else
                numericTextureLimit.Enabled = true;
        }
    }
}
