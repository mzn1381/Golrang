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
    public partial class Frm_010_ProgramMachine : Form
    {
        int _Id;
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

        public Frm_010_ProgramMachine()
        {
            InitializeComponent();
        }
        public Frm_010_ProgramMachine(int Id)
        {
            InitializeComponent();
            _Id = Id;
        }
        private void Frm_010_ProgramMachine_Load(object sender, EventArgs e)
        {
           
            mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,namemachine from Table_60_SpecsTechnical");
            mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth,CodeCommondity from Table_005_TypeCloth");
            //mlt_Cotton.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,code,NameCotton from Table_120_TypeCotton");
            //if (_Id!=0)
            //{
            //    table_100_ProgramMachineTableAdapter.FillByMachine(dataSet_05_Product.Table_100_ProgramMachine, _Id);
            //    table_120_TypeCottonTableAdapter.Fill(dataSet_05_Product.Table_120_TypeCotton);
            //    table_125_DetailTypeCottonTableAdapter.Fill(dataSet_05_Product.Table_125_DetailTypeCotton);
            //}
            //else
            //{
                table_100_ProgramMachineTableAdapter.Fill(dataSet_05_Product.Table_100_ProgramMachine);
                table_120_TypeCottonTableAdapter.Fill(dataSet_05_Product.Table_120_TypeCotton);
                table_125_DetailTypeCottonTableAdapter.Fill(dataSet_05_Product.Table_125_DetailTypeCotton);
            //}        

        }

        private void txt_Number_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 150))
            {
                table_100_ProgramMachineBindingSource.AddNew();
            txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
            txt_SatartDate.Text= FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
            //mlt_Machine.Value = _Id;
            mlt_Machine.Focus();
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void mlt_SatartDate_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_TypeCloth_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_TypeCloth, "TypeCloth", "CodeCommondity");

        }

        private void mlt_Cotton_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Cotton, "NameCotton", "Code");

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (mlt_Cotton.Text == "" || mlt_Machine.Text == "" || mlt_TypeCloth.Text == "" || txt_weight.Text == "0" || txt_weight.Text == "" || txt_SatartDate.Text == ""
                || mlt_TypeCloth.Text.All(char.IsDigit)  || mlt_Machine.Text.All(char.IsDigit))
            {
                Class_BasicOperation.ShowMsg("", "لطفا اطلاعات مورد نیاز را تکمیل نمایید", Class_BasicOperation.MessageType.Information);
                return;
            }
            if (((DataRowView)table_100_ProgramMachineBindingSource.CurrencyManager.Current)["ID"].ToString().StartsWith("-"))
            { 
             txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_100_ProgramMachine", "Number").ToString();

            }
            ((DataRowView)table_100_ProgramMachineBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
            ((DataRowView)table_100_ProgramMachineBindingSource.CurrencyManager.Current)["DateSabt"] = Class_BasicOperation.ServerDate();
            ((DataRowView)table_100_ProgramMachineBindingSource.CurrencyManager.Current)["UserEdite"] = Class_BasicOperation._UserName;
            ((DataRowView)table_100_ProgramMachineBindingSource.CurrencyManager.Current)["DateEdite"] = Class_BasicOperation.ServerDate();
            table_100_ProgramMachineBindingSource.EndEdit();
            table_100_ProgramMachineTableAdapter.Update(dataSet_05_Product.Table_100_ProgramMachine);
            table_125_DetailTypeCottonBindingSource.EndEdit();
            table_125_DetailTypeCottonTableAdapter.Update(dataSet_05_Product.Table_125_DetailTypeCotton);
                Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت ثبت شد", Class_BasicOperation.MessageType.Information);
             

           

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 159))
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select Machine from  Table_115_Product where Machine=" + mlt_Machine.Value + "");
                    if (dt.Rows.Count > 0)
                    {
                        Class_BasicOperation.ShowMsg("", "به علت استفاده از این برنامه دستگاه در ثبت تولید امکان حذف آن را ندارید", Class_BasicOperation.MessageType.Warning);
                        return;
                    }
                    if (table_100_ProgramMachineBindingSource.Count > 0)
                    {
                        table_100_ProgramMachineBindingSource.RemoveCurrent();
                        table_100_ProgramMachineTableAdapter.Update(dataSet_05_Product.Table_100_ProgramMachine);
                    }
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }

        }

        private void Frm_010_ProgramMachine_KeyDown(object sender, KeyEventArgs e)
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

        private void mlt_Cotton_Enter(object sender, EventArgs e)
        {
            try
            {
                this.table_100_ProgramMachineBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void mlt_TypeCloth_Leave(object sender, EventArgs e)
        {
            try
            {
                if (table_100_ProgramMachineBindingSource.Count > 0)
                {


                    if (((DataRowView)table_100_ProgramMachineBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                    {
                        txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_100_ProgramMachine", "Number").ToString();
                    }
                    if (txt_Number.Text == "") { MessageBox.Show("نام شعبه را وارد کنید"); txt_Number.Focus(); }
                    else
                    {
                        this.table_100_ProgramMachineBindingSource.EndEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }
    }
}
