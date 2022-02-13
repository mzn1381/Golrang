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
    public partial class Frm_50_TypeCotton : Form
    {


        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

        public Frm_50_TypeCotton()
        {
            InitializeComponent();
        }

        private void Frm_50_TypeCotton_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_120_TypeCotton' table. You can move, or remove it, as needed.
            this.table_120_TypeCottonTableAdapter.Fill(this.dataSet_05_Product.Table_120_TypeCotton);
            gridEX1.DropDowns["Code"].DataSource = mlt_Commodity.DataSource = ClDoc.ReturnTable(ConPWHRS, @"select Columnid,Column01,Column02 from table_004_CommodityAndIngredients where column19=1");

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 162))
            {
                table_120_TypeCottonBindingSource.AddNew();
                txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                mlt_Commodity.Focus();
                txtDescription1.Text = string.Empty;
                txtDescription2.Text = string.Empty;
                txtBrandName.Text = string.Empty;
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Code"].ToString().StartsWith("-"))
            {
                txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, " Table_120_TypeCotton", "Code").ToString();
            }
            ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["BrandName"] = txtBrandName.Text;
            ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Description1"] = txtDescription1.Text;
            ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Description2"] = txtDescription2.Text;
            table_120_TypeCottonBindingSource.EndEdit();
            table_120_TypeCottonTableAdapter.Update(dataSet_05_Product.Table_120_TypeCotton);
            Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت  دخیره شد", Class_BasicOperation.MessageType.Information);

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 163))
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable dt = new DataTable();
                    dt = ClDoc.ReturnTable(ConPCLOR, @"select Cotton from Table_100_ProgramMachine where Cotton = " + txtId.Text + "");

                    if (dt.Rows.Count > 0)
                    {
                        Class_BasicOperation.ShowMsg("", "به دلیل استفاده از این نخ در برنامه دستگاه امکان حذف آن را ندارید", Class_BasicOperation.MessageType.Warning);
                        return;

                    }
                    DataTable dt1 = new DataTable();
                    dt1 = ClDoc.ReturnTable(ConPCLOR, @"select CottonType from  Table_115_Product where CottonType=" + txtId.Text + "");

                    if (dt1.Rows.Count > 0)
                    {
                        Class_BasicOperation.ShowMsg("", "به دلیل استفاده از این نخ در ثبت تولید امکان حذف آن را ندارید", Class_BasicOperation.MessageType.Warning);
                        return;

                    }

                    table_120_TypeCottonBindingSource.RemoveCurrent();
                    table_120_TypeCottonBindingSource.EndEdit();
                    table_120_TypeCottonTableAdapter.Update(dataSet_05_Product.Table_120_TypeCotton);
                    Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت  حذف انجام شد", Class_BasicOperation.MessageType.Information);
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void mlt_Commodity_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Commodity, "Column02", "Column01");

            txtTitleCotton.Text = mlt_Commodity.Text;
        }

        private void mlt_Commodity_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Commodity_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void gridEX1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_120_TypeCottonTableAdapter.FillBy(this.dataSet_05_Product.Table_120_TypeCotton);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {
            //txtBrandName.Text = ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["BrandName"].ToString();
            //txtDescription1.Text = ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Description1"].ToString();
            //txtDescription2.Text = ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Description2"].ToString();

        }

        private void gridEX1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX1.CurrentRow != null)
            {
                txtBrandName.Text = ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["BrandName"].ToString();
                txtDescription1.Text = ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Description1"].ToString();
                txtDescription2.Text = ((DataRowView)table_120_TypeCottonBindingSource.CurrencyManager.Current)["Description2"].ToString();
            }
        }
    }
}
