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
            using (StreamReader r = new StreamReader("ServerAddress.json"))
            {
                string json = r.ReadToEnd();
                var serverInformation = JsonConvert.DeserializeObject<ServerInformationModel>(json);
                if (!string.IsNullOrEmpty(serverInformation.ServerAddress)&& !string.IsNullOrEmpty(serverInformation.ServerAddress)&& !string.IsNullOrEmpty(serverInformation.ServerAddress))
                {
                    using (IDbConnection db = new SqlConnection($"Data Source={serverInformation.ServerAddress};Initial Catalog=PERP_MAIN;Persist Security Info=True;User ID={serverInformation.UserName};Password={serverInformation.Password}"))
                    {
                        string query = $@"  select  column00 as UserName ,column01 as Password , column05 as CompanyCode ,column06 as FinanceYear  from [dbo].[Table_010_UserInfo]
                            where RTRIM(LTRIM( LOWER(Column00))) = N'{txt_userName.Text.ToLower().Trim()}' and RTRIM(LTRIM( LOWER(Column01))) =N'{txt_Password.Text.Trim().ToLower()}'  ";
                        var res = db.QueryFirstOrDefault<ResultLoginViewModel>(query);
                        if (!string.IsNullOrEmpty(res.CompanyCode) && string.IsNullOrEmpty( res.FinanceYear))
                        {
                            PCLOR.Properties.Settings.Default.PSALE
                        }
                        else
                        {

                        }
                    }
                    //Application.Run(new Frm_Main("گلرنگ", "admin", "1400", 1, true, false, "Data Source=.;Initial Catalog=PCLOR_1_1400;Persist Security Info=True;User ID=sa;Password=Pars@63"));
                }
            }

        }
    }
}
