using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace PCLOR._01_Frms
{
    public partial class _01_Login : Form
    {
       
        string _MachineId = null, _LicenceKey = null;
        Int16 _CompID, _BackCompId, _RestoreCompID;
        bool UserOk, Connected2DB = false;
        string _DatabaseName, UserNameFromFile, _BankName, _OrgCode, _Year, CompName,
            _PERPConnectionString, _OldOrgCode, _OldYear, RestoreDB;
        bool _FinType, _WareType, _ShowMessage;
       Classes.  Class_Documents clDoc = new Classes. Class_Documents();
        SqlConnection ConMain, ConStrPER;
        string lastdate = null;
        public _01_Login()
        {
            InitializeComponent();
           

        }

        private void bt_OpenComp_Click(object sender, EventArgs e)
        {
            txt_Year.Text = null;
            txt_CompName.Text = null;
            _01_Frms._02_CompanyList frm = new _02_CompanyList();
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                _CompID = frm.CompID;
                txt_CompName.Text = frm.CompName;
                _FinType = frm._FinType;
                _WareType = frm._WareType;
            }
        }

        private void bt_OpenYear_Click(object sender, EventArgs e)
        {
            txt_Year.Text = null;
            if (!string.IsNullOrEmpty(txt_CompName.Text.Trim()))
            {
                _01_Frms._03_FinanceYearList frm = new _03_FinanceYearList(_CompID);
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    txt_Year.Text = frm._Year; lastdate = frm.lastUpdate;
                }
                else
                    txt_Year.Text = null;
            }
        }

        private void bt_Login_Click(object sender, EventArgs e)
        {


            SqlConnection conn = new SqlConnection(Properties.Settings.Default.MAIN);
           
           Classes. Class_UserScope UserScope = new Classes.Class_UserScope();
             
            if (txt_UserName2.Text.Trim() != "" && txt_CompName.Text.Trim() != "" && txt_Password2.Text.Trim() != "" &&
                txt_Year.Text.Trim() != "")
            {
                try
                {
                    _OrgCode = _CompID.ToString();
                    _Year = txt_Year.Text;

                    Class_BasicOperation._UserName = txt_UserName2.Text.Trim();
                    Class_BasicOperation._FinYear = txt_Year.Text.Trim();
                    Class_BasicOperation._OrgCode = Convert.ToInt16(_OrgCode);
                    _BankName = _DatabaseName + "_" + _OrgCode + "_" + _Year;
                   



                    _PERPConnectionString =
                             Properties.Settings.Default.MAIN;
                    try
                    {
                        ConStrPER = new SqlConnection(_PERPConnectionString);

                        ConStrPER.Open();
                        ConStrPER.Close();
                        Connected2DB = true;
                    }
                    catch
                    {
                        Connected2DB = false;
                    }
                    if (Connected2DB)
                    {
                        UserOk = UserScope.isValid(txt_UserName2.Text.Trim(), txt_Password2.Text.Trim());

                        Class_BasicOperation._Branch = UserScope.ReturnBranch(txt_UserName2.Text);

                        //SqlConnection ConMAIN = new SqlConnection(Properties.Settings.Default.MAIN);

                        Frm_Main frm = new Frm_Main(txt_CompName.Text.Trim(), txt_UserName2.Text.Trim(),
                                                   txt_Year.Text.Trim(), _CompID, _WareType, _FinType, _PERPConnectionString.Replace("PERP_MAIN", "PCLOR_" + _CompID + "_" + txt_Year.Text.Trim()));
                        txt_Password2.Text = null;
                    
                        /////
                       
                        
                                   

                        frm.Show();
                        this.Hide();


                        Properties.Settings.Default.LastUserName = txt_UserName2.Text.Trim();
                        Properties.Settings.Default.OrgCode = _CompID.ToString();
                        Properties.Settings.Default.Year = txt_Year.Text.Trim();
                        Properties.Settings.Default.Save();
                    }

                    
                }

                catch (Exception ex)
                {
                    Class_BasicOperation.ShowMsg("", ex.Message, Class_BasicOperation.MessageType.Warning);
                    txt_Password2.Focus();
                    txt_Password2.SelectAll();
                }
            }

            else
            {
                UserOk = false;
                MessageBox.Show("اطلاعات مورد نیاز را کامل کنید");
                txt_UserName2.Focus();
                txt_UserName2.SelectAll();
                //MessageBox.Show("برقراری ارتباط با سرویس دهنده بانک اطلاعاتی میسر نیست",
                //       "", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);

                return;
            }
        }

        private void _01_Login_Load(object sender, EventArgs e)
        {
            try
            {
                UserNameFromFile = Properties.Settings.Default.LastUserName;
                _OldOrgCode = Properties.Settings.Default.OrgCode;
                _OldYear = Properties.Settings.Default.Year;

                try
                {
                    ConMain = new SqlConnection(
                              Properties.Settings.Default.MAIN);
                    ConMain.Open();
           ///////
                    string OCode = "-1";
                    if (!string.IsNullOrEmpty(_OldOrgCode))
                        OCode = _OldOrgCode;
                    SqlCommand SelectOrg = new SqlCommand(
                        "Select ColumnId,Column00,Column01,Column15,Column16 from Table_000_OrgInfo"
                        + " where ColumnId=" + OCode, ConMain);
                    using (SqlDataReader Reader = SelectOrg.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        Reader.Read();
                        if (Reader.HasRows)
                        {
                            _CompID = short.Parse(Reader["ColumnId"].ToString());
                            txt_CompName.Text = Reader["Column01"].ToString();
                            txt_Year.Text = _OldYear;
                            _Year = _OldYear;
                            _FinType = bool.Parse(Reader["Column15"].ToString());
                            _WareType = bool.Parse(Reader["Column16"].ToString());
                        }
                    }

                    if (UserNameFromFile != null)
                        txt_UserName2.Text = UserNameFromFile;
                    else
                        txt_UserName2.Text = "Admin";
                }
                catch
                {
                    MessageBox.Show("برقراری ارتباط با سرویس دهنده بانک اطلاعاتی میسر نیست",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                  
                 

                    return;

                }
                
            }
            catch { }
        }

        private void txt_CompName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void txt_Password2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void p_Logo_Click(object sender, EventArgs e)
        {

        }

        private void btnreturn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbl_SysName_Click(object sender, EventArgs e)
        {

        }

    
    }
}
