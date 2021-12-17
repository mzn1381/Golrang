using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.Timers;
using PCLOR.Classes;
//using Stimulsoft.Controls.Win.DotNetBar;
//using Tulpep.NotificationWindow;
using DevComponents.DotNetBar;
using System.Media;
using PCLOR._01_OperationInfo;

namespace PCLOR
{
    public partial class Frm_Main : Form
    {
        string _CompyName, _FinYear, _ConnectionString;
        bool _WareType, _FinType;
        public string _UserName;
        public Int16 _CompID;
        public bool _ShowMsg = true;
        Int16 ware;
        Int16 func;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
       
        public Frm_Main(string CompName, string UserName,
        string FinYear, Int16 CompID, bool WareType, bool FinType, string ConnectionString)
        {
            _CompyName = CompName;
            _CompID = CompID;
            _ConnectionString = ConnectionString;
            _FinType = FinType;
            _WareType = WareType;
            _FinYear = FinYear;
            _UserName = UserName;
            Class_ChangeConnectionString.CurrentConnection = _ConnectionString;
            Class_BasicOperation._OrgCode = _CompID;
            Class_BasicOperation._FinYear = _FinYear;
            Class_BasicOperation._UserName = _UserName;

            InitializeComponent();
        }



        private bool CheckOpenForms(string FormName)
        {
            foreach (Form item in Application.OpenForms)
            {
                if (item.Name == FormName)
                {
                    item.BringToFront();
                    return true;
                }
            }
            return false;
        }

        public string _CompName { get; set; }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
            Class_BasicOperation.ChangeLanguage("fa-IR");
            lbl_Year.Text = "سال جاری:" + _FinYear;
            lbl_User.Text = "کاربر جاری:" + _UserName;
            lbl_Org.Text = "شرکت مهندسی پارسینا پردازان آریا" + _CompName;
            lbl_Today.Text = "امروز:" + FarsiLibrary.Utils.PersianDate.Now.ToWritten();
            //StartBckWorker(sender, e);
            ToastNotification.ToastBackColor = Color.OrangeRed;
            ToastNotification.ToastForeColor = Color.Black;
            //ToastNotification.Show(this, "این تعداد بارکد دارای مغایرت می باشند" , 9000, eToastPosition.MiddleCenter);

        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_05_TypeCloth"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 7))
                {
                    _00_BaseInfo.Frm_05_TypeCloth frm = new _00_BaseInfo.Frm_05_TypeCloth();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_10_TypeColor"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 4))
                {
                    _00_BaseInfo.Frm_10_TypeColor frm = new _00_BaseInfo.Frm_10_TypeColor();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btnBeforeFactor_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_20_ReciptClothRaw"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 14))
                {
                    _01_OperationInfo.Frm_20_ReciptClothRaw frm = new _01_OperationInfo.Frm_20_ReciptClothRaw();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btnViewBeforeFactor_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_25_OrderColor"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 17))
                {
                    _01_OperationInfo.Frm_25_OrderColor frm = new _01_OperationInfo.Frm_25_OrderColor();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btnReturnFactor_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_30_Production"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 20))
                {
                    _01_OperationInfo.Frm_30_Production frm = new _01_OperationInfo.Frm_30_Production();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }



        private void buttonItem16_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Report_Order"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 54))
                {
                    Report.Frm_Report_Order frm = new Report.Frm_Report_Order();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Report_Pack"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 55))
                {
                    Report.Frm_Report_Pack frm = new Report.Frm_Report_Pack();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem17_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_20_ColorDefinition"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 1))
                {
                    _00_BaseInfo.Frm_20_ColorDefinition frm = new _00_BaseInfo.Frm_20_ColorDefinition();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem19_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Form_25_SpecsTechnical"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 10))
                {
                    _00_BaseInfo.Form_25_SpecsTechnical frm = new _00_BaseInfo.Form_25_SpecsTechnical();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem20_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem22_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_50_SendPackaging2"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 46))
                {
                    _01_OperationInfo.Frm_50_SendPackaging2 frm = new _01_OperationInfo.Frm_50_SendPackaging2();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem23_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_55_ReciptPackaging2"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 50))
                {
                    _01_OperationInfo.Frm_55_ReciptPackaging2 frm = new _01_OperationInfo.Frm_55_ReciptPackaging2();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem20_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Setting_15_Weighbridge"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 13))
                {
                    _00_BaseInfo.Setting_15_Weighbridge frm = new _00_BaseInfo.Setting_15_Weighbridge();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem24_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_30_Type_WHRS"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 26))
                {
                    _00_BaseInfo.Frm_30_Type_WHRS frm = new _00_BaseInfo.Frm_30_Type_WHRS();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem25_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_35_Branchs"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 27))
                {
                    _00_BaseInfo.Frm_35_Branchs frm = new _00_BaseInfo.Frm_35_Branchs();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem26_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_40_PersonInfo"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 29))
                {
                    _00_BaseInfo.Frm_40_PersonInfo frm = new _00_BaseInfo.Frm_40_PersonInfo();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem27_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_30_WHRS"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 39))
                {
                    _00_BaseInfo.Frm_30_WHRS frm = new _00_BaseInfo.Frm_30_WHRS();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void buttonItem29_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_35_packingNew"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 23))
                {
                    _01_OperationInfo.Frm_35_packingNew frm = new _01_OperationInfo.Frm_35_packingNew();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }

        }

        private void buttonItem30_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_50_SendPackaging2"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 23))
                {
                    _01_OperationInfo.Frm_50_SendPackaging2 frm = new _01_OperationInfo.Frm_50_SendPackaging2();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem30_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Form01_Backup"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 56))
                {
                    _01_Frms.Form01_Backup frm = new _01_Frms.Form01_Backup();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem31_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Form01_Backup"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 57))
                {
                    _01_Frms.Form02_Restore frm = new _01_Frms.Form02_Restore();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem32_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem35_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Report_Conflict"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 57))
                {
                    Report.Frm_Report_Conflict frm = new Report.Frm_Report_Conflict();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem36_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem37_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_45_FiSale"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 86))
                {
                    _00_BaseInfo.Frm_45_FiSale frm = new _00_BaseInfo.Frm_45_FiSale();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem38_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_45_FiSale"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 86))
                {
                    Report.Frm_StockRial frm = new Report.Frm_StockRial();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem39_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem40_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem42_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem41_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem43_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem45_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_009_KardexTedadi"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 110))
                {
                    Report.Frm_009_KardexTedadi frm = new Report.Frm_009_KardexTedadi();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem46_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_010_KardexRiyali"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 111))
                {
                    Report.Frm_010_KardexRiyali frm = new Report.Frm_010_KardexRiyali();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem33_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_003_ViewFactorSale"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 76))
                {
                    _002_Sale.Frm_003_ViewFactorSale frm = new _002_Sale.Frm_003_ViewFactorSale();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem34_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_002_ViewFactorReturn"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 83))
                {
                    MarjooiSale.Frm_002_ViewFactorReturn frm = new MarjooiSale.Frm_002_ViewFactorReturn();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void ButtonItem36_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_MaghtaeiTedadi"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 69))
                {
                    Report.Frm_MaghtaeiTedadi frm = new Report.Frm_MaghtaeiTedadi();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem39_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_002_StoreFaktor"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 70))
                {
                    _002_Sale.Frm_002_StoreFaktor frm = new _002_Sale.Frm_002_StoreFaktor(UserScope.CheckScope(_UserName, "Column11", 21));
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem32_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_001_MarjooiSale"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 77))
                {
                    MarjooiSale.Frm_001_MarjooiSale frm = new MarjooiSale.Frm_001_MarjooiSale(UserScope.CheckScope(_UserName, "Column11", 23));
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void ribbonTabItem2_Click(object sender, EventArgs e)
        {

        }

        int ID = 0;
        private void buttonItem40_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_01_Recipt_Cheques"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 93))
                {
                    _03_Bank.Frm_01_new_Recipt_Cheques frm = new _03_Bank.Frm_01_new_Recipt_Cheques(UserScope.CheckScope(_UserName, "Column09", 28), UserScope.CheckScope(_UserName, "Column09", 29),
                           UserScope.CheckScope(_UserName, "Column09", 30), 0);
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem42_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_02_Transfer_Banck"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 96))
                {
                    _03_Bank.Frm_02_Transfer_Banck frm = new _03_Bank.Frm_02_Transfer_Banck();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem43_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_03_Transfer_Person"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 98))
                {
                    _03_Bank.Frm_03_Transfer_Person frm = new _03_Bank.Frm_03_Transfer_Person();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem47_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_04_Recipt_checks"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 100))
                {
                    _03_Bank.Frm_04_Recipt_checks frm = new _03_Bank.Frm_04_Recipt_checks();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }



        private void buttonItem48_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_05_Return_checks"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 102))
                {
                    _03_Bank.Frm_05_Return_checks frm = new _03_Bank.Frm_05_Return_checks();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem49_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_06_Return_Expense"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 104))
                {
                    _03_Bank.Frm_06_Return_Expense frm = new _03_Bank.Frm_06_Return_Expense();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem50_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_07_Refund_Check"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 106))
                {
                    _03_Bank.Frm_07_Refund_Check frm = new _03_Bank.Frm_07_Refund_Check();
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem51_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_08_ViewReceivedChq"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 108))
                {
                    _03_Bank.Frm_08_ViewReceivedChq frm = new _03_Bank.Frm_08_ViewReceivedChq(0,
                        UserScope.CheckScope(_UserName, "Column09", 40));
                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem52_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_08_ViewReceivedChq"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 84))
                {
                    _03_Bank.Frm_09_Recipt_Money frm = new _03_Bank.Frm_09_Recipt_Money(
                          UserScope.CheckScope(_UserName, "Column09", 17), UserScope.CheckScope(_UserName, "Column09", 18),
                           UserScope.CheckScope(_UserName, "Column09", 19), 0);

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem53_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_08_ViewReceivedChq"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 90))
                {
                    _03_Bank.Form03_BanksBox frm = new _03_Bank.Form03_BanksBox(UserScope.CheckScope(_UserName, "Column09", 5),
                          UserScope.CheckScope(_UserName, "Column09", 6));

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem54_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_10_PayCash"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 87))
                {
                    _03_Bank.Frm_10_PayCash frm = new _03_Bank.Frm_10_PayCash(
                          UserScope.CheckScope(_UserName, "Column09", 22), UserScope.CheckScope(_UserName, "Column09", 23),
                           UserScope.CheckScope(_UserName, "Column09", 24), 0);

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }



        private void buttonItem56_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_11_View_Recipt_Money"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 141))
                {
                    _03_Bank.Frm_11_View_Recipt_Money frm = new _03_Bank.Frm_11_View_Recipt_Money(0);

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem57_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_12_View_PayCash"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 142))
                {
                    _03_Bank.Frm_12_View_PayCash frm = new _03_Bank.Frm_12_View_PayCash(0);

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void bt_Ras_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_13_Rasgiri"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 116))
                {
                    _03_Bank.Frm_13_Rasgiri frm = new _03_Bank.Frm_13_Rasgiri();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem41_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonItem58_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_006_Handcheckback"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 112))
                {
                    _03_Bank.Frm_006_Handcheckback frm = new _03_Bank.Frm_006_Handcheckback();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem59_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem60_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_01_Report_Statuscheq"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 115))
                {
                    _03_Bank.Frm_01_Report_Statuscheq frm = new _03_Bank.Frm_01_Report_Statuscheq();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem61_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_01_Report_Statuscheq"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 116))
                {
                    _03_Bank.Frm_01_Recipt_Cheques_Group frm = new _03_Bank.Frm_01_Recipt_Cheques_Group();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem63_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_005_RportSale"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 121))
                {
                    _002_Sale.Frm_005_RportSale frm = new  _002_Sale.Frm_005_RportSale();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem60_Click_1(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_01_Report_Statuscheq"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 115))
                {
                    _03_Bank.Frm_01_Report_Statuscheq frm = new _03_Bank.Frm_01_Report_Statuscheq();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem64_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_002_StoreFaktor"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 122))
                {
                    _04_Shop.Frm_002_StoreFaktor frm = new _04_Shop.Frm_002_StoreFaktor(UserScope.CheckScope(_UserName, "Column11", 21));

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem65_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_003_FaktorKharid"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 125))
                {
                    _04_Shop.Frm_003_FaktorKharid frm = new _04_Shop.Frm_003_FaktorKharid(UserScope.CheckScope(_UserName, "Column11", 29), 0);

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem67_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_003_FaktorKharid"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 129))
                {
                    MarjooiSale.Frm_002_MarjooiSaleBarcod frm = new MarjooiSale.Frm_002_MarjooiSaleBarcod();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void ribbonBar3_ItemClick(object sender, EventArgs e)
        {

        }

        private void buttonItem69_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_30_Production_Edite"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column37", 135))
                {
                    MarjooiSale.Frm_30_Production_Edite frm = new MarjooiSale.Frm_30_Production_Edite();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
      
                    DataTable dt = new DataTable();
                    int Count = 0;
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT       TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity, SUM(SumSend) AS SumSend, SUM(SumRecipt) AS SumRecipt, SUM(SumSend) - SUM(SumRecipt) AS Remain
FROM            (SELECT        dbo.Table_70_DetailOtherPWHRS.TypeCloth, dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.Machine, dbo.Table_70_DetailOtherPWHRS.weight, 
                                                    dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, 0 AS SumSend, dbo.Table_70_DetailOtherPWHRS.Count AS SumRecipt
                          FROM            dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                                    dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                          WHERE        (dbo.Table_70_DetailOtherPWHRS.NumberRecipt <> 0) AND (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1)
                          UNION ALL
                          SELECT        Table_70_DetailOtherPWHRS_1.TypeCloth, Table_70_DetailOtherPWHRS_1.Barcode, Table_70_DetailOtherPWHRS_1.Machine, Table_70_DetailOtherPWHRS_1.weight, 
                                                   Table_70_DetailOtherPWHRS_1.TypeColor, Table_70_DetailOtherPWHRS_1.CodeCommondity, Table_70_DetailOtherPWHRS_1.Count AS SumSend, 0 AS SumRecipt
                          FROM            dbo.Table_65_HeaderOtherPWHRS AS Table_65_HeaderOtherPWHRS_1 INNER JOIN
                                                   dbo.Table_70_DetailOtherPWHRS AS Table_70_DetailOtherPWHRS_1 ON Table_65_HeaderOtherPWHRS_1.ID = Table_70_DetailOtherPWHRS_1.FK
                          WHERE        (Table_70_DetailOtherPWHRS_1.NumberDraft <> 0) AND (Table_65_HeaderOtherPWHRS_1.Sends = 1)) AS dt
GROUP BY TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity
HAVING        (SUM(SumSend) - SUM(SumRecipt) <> 0)
ORDER BY Barcode");

                    foreach (DataRow item in dt.Rows)
                    {
                        Count++;
                    }

                   ToastNotification.Show(this, "این تعداد بارکد دارای مغایرت می باشند" + Count, 9000, eToastPosition.MiddleCenter);
                    //MessageBox.Show("Test");
                  //ToastNotification.Show(this, "", 3000, eToastPosition.MiddleCenter);
             
                Thread.Sleep(2000);

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void StartBckWorker(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartBckWorker(sender, e);
        }

        private void buttonItem70_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Rpt_SumSaleFi"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 143))
                {
                    Report.Frm_Rpt_SumSaleFi frm = new Report.Frm_Rpt_SumSaleFi();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem72_Click(object sender, EventArgs e)
        {



            


            if (!CheckOpenForms("Frm_005_SelectMachine"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 148))
                {

                    Frm_05_Machines frm_05_Machines = new Frm_05_Machines();


                    frm_05_Machines.MdiParent = this;
                    if (frm_05_Machines.MdiParent.MdiChildren.Length > 1 && frm_05_Machines.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm_05_Machines.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm_05_Machines.WindowState = FormWindowState.Maximized;
                    }
                    frm_05_Machines.Show(); frm_05_Machines.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void bt_Programmachine_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_010_ProgramMachine"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 149))
                {
                    Product.Frm_010_ProgramMachine frm = new Product.Frm_010_ProgramMachine();

                frm.MdiParent = this;
                if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                {
                    frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                }
                frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void bt_Product_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_015_Product"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 145))
                {
                    Product.Frm_015_Product frm = new Product.Frm_015_Product();

                frm.MdiParent = this;
                if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                {
                    frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                }
                frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_030_OrderColor"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 151))
                {
                    Product.Frm_030_OrderColor frm = new Product.Frm_030_OrderColor();

                frm.MdiParent = this;
                if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                {
                    frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                }
                frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

      
        private void buttonItem73_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Rpt_SumSaleFi"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 154))
                {
                    Product.Frm_025_ReportDeviceFailure frm = new Product.Frm_025_ReportDeviceFailure();

                frm.MdiParent = this;
                if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                {
                    frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                }
                frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem74_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_020_shift"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 157))
                {
                    Product.Frm_020_shift frm = new Product.Frm_020_shift();

                frm.MdiParent = this;
                if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                {
                    frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                }
                frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            int Count = 0;
            dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT       TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity, SUM(SumSend) AS SumSend, SUM(SumRecipt) AS SumRecipt, SUM(SumSend) - SUM(SumRecipt) AS Remain
FROM            (SELECT        dbo.Table_70_DetailOtherPWHRS.TypeCloth, dbo.Table_70_DetailOtherPWHRS.Barcode, dbo.Table_70_DetailOtherPWHRS.Machine, dbo.Table_70_DetailOtherPWHRS.weight, 
                                                    dbo.Table_70_DetailOtherPWHRS.TypeColor, dbo.Table_70_DetailOtherPWHRS.CodeCommondity, 0 AS SumSend, dbo.Table_70_DetailOtherPWHRS.Count AS SumRecipt
                          FROM            dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                                    dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                          WHERE        (dbo.Table_70_DetailOtherPWHRS.NumberRecipt <> 0) AND (dbo.Table_65_HeaderOtherPWHRS.Recipt = 1)
                          UNION ALL
                          SELECT        Table_70_DetailOtherPWHRS_1.TypeCloth, Table_70_DetailOtherPWHRS_1.Barcode, Table_70_DetailOtherPWHRS_1.Machine, Table_70_DetailOtherPWHRS_1.weight, 
                                                   Table_70_DetailOtherPWHRS_1.TypeColor, Table_70_DetailOtherPWHRS_1.CodeCommondity, Table_70_DetailOtherPWHRS_1.Count AS SumSend, 0 AS SumRecipt
                          FROM            dbo.Table_65_HeaderOtherPWHRS AS Table_65_HeaderOtherPWHRS_1 INNER JOIN
                                                   dbo.Table_70_DetailOtherPWHRS AS Table_70_DetailOtherPWHRS_1 ON Table_65_HeaderOtherPWHRS_1.ID = Table_70_DetailOtherPWHRS_1.FK
                          WHERE        (Table_70_DetailOtherPWHRS_1.NumberDraft <> 0) AND (Table_65_HeaderOtherPWHRS_1.Sends = 1)) AS dt
GROUP BY TypeCloth, Barcode, Machine, weight, TypeColor, CodeCommondity
HAVING        (SUM(SumSend) - SUM(SumRecipt) <> 0)
ORDER BY Barcode");

            foreach (DataRow item in dt.Rows)
            {
                Count++;
            }

            ToastNotification.Show(this, "این تعداد بارکد دارای مغایرت می باشند"+Environment.NewLine + Count,4000, eToastPosition.MiddleLeft);
            System.Media.SystemSounds.Hand.Play();
            //SoundPlayer x = new SoundPlayer("Music.mp3");
            //x.Play();
        }

        private void buttonItem59_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonItem77_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Rpt_BarcodeDetail"))
            {
                //Class_UserScope UserScope = new Class_UserScope();
                //if (UserScope.CheckScope(_UserName, "Column44", 143))
                //{
                    _03_Bank.Frm_14_TransferCheq frm = new _03_Bank.Frm_14_TransferCheq();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                //}
                //else
                //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem78_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Rpt_BarcodeDetail"))
            {
                //Class_UserScope UserScope = new Class_UserScope();
                //if (UserScope.CheckScope(_UserName, "Column44", 143))
                //{
                Product.Frm_135_RFID frm = new Product.Frm_135_RFID();

                frm.MdiParent = this;
                if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                {
                    frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                }
                frm.Show(); frm.Focus();
                //}
                //else
                //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem79_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_140_Report_TransferBranch"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 165))
                {
                   _03_Bank.Frm_140_Report_TransferBranch frm = new _03_Bank.Frm_140_Report_TransferBranch();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void ribbonTabItem4_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem71_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Rpt_BarcodeDetail"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 143))
                {
                    Report.Frm_Rpt_BarcodeDetail frm = new Report.Frm_Rpt_BarcodeDetail();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem75_Click(object sender, EventArgs e)
        {





            if (!CheckOpenForms("Frm_Rpt_BarcodeDetail"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 160))
                {
                    Report.Frm_Rpt_BarcodeDetail frm = new Report.Frm_Rpt_BarcodeDetail();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void buttonItem76_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_50_TypeCotton"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(_UserName, "Column44", 161))
                {
                    _00_BaseInfo.Frm_50_TypeCotton frm = new _00_BaseInfo.Frm_50_TypeCotton();

                    frm.MdiParent = this;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }
    }
}