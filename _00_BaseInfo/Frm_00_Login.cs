using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLOR.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCLOR._00_BaseInfo
{
    public partial class Frm_00_Login : Form
    {
        public Frm_00_Login()
        {
            InitializeComponent();
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            try
            {
                var userName = txt_userName.Text.ToLower().Trim();
                var password = txt_Password.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("لطفا نام کاربری و رمز عبور را وارد کنید ", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    using (StreamReader r = new StreamReader("ServerAddress.json"))
                    {
                        string json = r.ReadToEnd();
                        var serverInformation = JsonConvert.DeserializeObject<ServerInformationModel>(json);
                        if (!string.IsNullOrEmpty(serverInformation.ServerAddress) && !string.IsNullOrEmpty(serverInformation.ServerAddress) && !string.IsNullOrEmpty(serverInformation.ServerAddress))
                        {
                            using (IDbConnection db = new SqlConnection($"Data Source={serverInformation.ServerAddress};Initial Catalog=PERP_MAIN;Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}"))
                            {
                                string query = $@"  select  column00 as UserName ,column01 as Password , column05 as CompanyCode ,column06 as FinanceYear  from [dbo].[Table_010_UserInfo]
                            where RTRIM(LTRIM( LOWER(Column00))) = N'{txt_userName.Text.ToLower().Trim()}' and RTRIM(LTRIM( LOWER(Column01))) =N'{txt_Password.Text.Trim().ToLower()}'  ";
                                var res = db.QueryFirstOrDefault<ResultLoginViewModel>(query);
                                if (res != null && !string.IsNullOrEmpty(res.CompanyCode) && !string.IsNullOrEmpty(res.FinanceYear))
                                {
                                    PCLOR.Properties.Settings.Default.PCLOR = $"Data Source={serverInformation.ServerAddress};Initial Catalog=PCLOR_{res.CompanyCode}_{res.FinanceYear};Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}";
                                    PCLOR.Properties.Settings.Default.PBASE = $"Data Source={serverInformation.ServerAddress};Initial Catalog=PBASE_{res.CompanyCode}_{res.FinanceYear};Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}";
                                    PCLOR.Properties.Settings.Default.PACNT = $"Data Source={serverInformation.ServerAddress};Initial Catalog=PACNT_{res.CompanyCode}_{res.FinanceYear};Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}";
                                    PCLOR.Properties.Settings.Default.PSALE = $"Data Source={serverInformation.ServerAddress};Initial Catalog=PSALE_{res.CompanyCode}_{res.FinanceYear};Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}";
                                    PCLOR.Properties.Settings.Default.PWHRS = $"Data Source={serverInformation.ServerAddress};Initial Catalog=PWHRS_{res.CompanyCode}_{res.FinanceYear};Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}";
                                    PCLOR.Properties.Settings.Default.PBANK = $"Data Source={serverInformation.ServerAddress};Initial Catalog=PBANK_{res.CompanyCode}_{res.FinanceYear};Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}";
                                    PCLOR.Properties.Settings.Default.Save();
                                    //var frm_main = new F
                                    var task = Task.Factory.StartNew(() =>
                                    {
                                        Application.Run(new Frm_Main(txt_userName.Text.ToLower().Trim(), "admin", res.FinanceYear, Convert.ToInt16(res.CompanyCode), true, false, PCLOR.Properties.Settings.Default.PCLOR));
                                    });
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("نام کاربری یا رمز عبور اشتباه است !", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("مشکلی به وجد آمده !", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btn_Exite_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txt_Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Enter_Click(sender, e);
            }
        }
    }
}
