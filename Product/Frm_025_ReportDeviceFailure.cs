using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR.Product
{
    public partial class Frm_025_ReportDeviceFailure : Form
    {
        int _Id ;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);

        public Frm_025_ReportDeviceFailure()
        {
            InitializeComponent();
        }
        public Frm_025_ReportDeviceFailure(int Id)
        {
            _Id = Id;
            InitializeComponent();
        }
        private void Frm_020_ReportDeviceFailure_Load(object sender, EventArgs e)
        {
           
            this.table_110_ReportDeviceFailureTableAdapter.Fill(this.dataSet_05_Product.Table_110_ReportDeviceFailure);
            //mlt_Machine.Value = _Id;
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 155))
            {
                table_110_ReportDeviceFailureBindingSource.AddNew();
            ((DataRowView)table_110_ReportDeviceFailureBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
            ((DataRowView)table_110_ReportDeviceFailureBindingSource.CurrencyManager.Current)["DateSabt"] = Class_BasicOperation.ServerDate().ToString();
            ((DataRowView)table_110_ReportDeviceFailureBindingSource.CurrencyManager.Current)["UserEdite"] = Class_BasicOperation._UserName;
            ((DataRowView)table_110_ReportDeviceFailureBindingSource.CurrencyManager.Current)["DateEdite"] = Class_BasicOperation.ServerDate().ToString();
            txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
            gridEX2.DropDowns["Machine"].DataSource =mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,namemachine from Table_60_SpecsTechnical");

            txt_Cause.Focus();
            mlt_Machine.Value = _Id;
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {


            if (mlt_Machine.Text == "" || mlt_Machine.Text.All(char.IsDigit) || txt_Cause.Text == "" || txt_Description.Text == "" 
                || txt_StartDate.Text == "" || txt_EndDate.Text == "" || txt_StartDate.Text == "00:00:00" || txt_EndDate.Text == "00:00:00")
            {
                Class_BasicOperation.ShowMsg("", "لطفا اطلاعات مورد نیاز را تکمیل نمایید", Class_BasicOperation.MessageType.Information);
                return;
            }

            if (((DataRowView)table_110_ReportDeviceFailureBindingSource.CurrencyManager.Current)["ID"].ToString().StartsWith("-"))
            {
                txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_115_Product", "Number").ToString();

            }
            table_110_ReportDeviceFailureBindingSource.EndEdit();
            table_110_ReportDeviceFailureTableAdapter.Update(dataSet_05_Product.Table_110_ReportDeviceFailure);
            Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت دخیره شد", Class_BasicOperation.MessageType.Information);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 156))
            {
             if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                table_110_ReportDeviceFailureBindingSource.RemoveCurrent();
                table_110_ReportDeviceFailureBindingSource.EndEdit();
                table_110_ReportDeviceFailureTableAdapter.Update(dataSet_05_Product.Table_110_ReportDeviceFailure);
                Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت حذف شد", Class_BasicOperation.MessageType.Information);
            }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void txt_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void Frm_025_ReportDeviceFailure_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.N)
            {
                btn_New_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {
                btn_Delete_Click(sender, e);
            }
        }
    }
}
