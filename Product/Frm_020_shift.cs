using Ionic.Zip;
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
    public partial class Frm_020_shift : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Frm_020_shift()
        {
            InitializeComponent();
        }

        private void Frm_020_shift_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_105_DefinitionWorkShift' table. You can move, or remove it, as needed.
            this.table_105_DefinitionWorkShiftTableAdapter.Fill(this.dataSet_05_Product.Table_105_DefinitionWorkShift);

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            table_105_DefinitionWorkShiftBindingSource.EndEdit();
            table_105_DefinitionWorkShiftTableAdapter.Update(dataSet_05_Product.Table_105_DefinitionWorkShift);
            Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت ذخیره شد", Class_BasicOperation.MessageType.Information);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 158))
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable dt = new DataTable();
                    dt = ClDoc.ReturnTable(ConPCLOR, @"select Shift from Table_115_Product where Shift=" + gridEX2.GetValue("ID") + "");
                    if (dt.Rows.Count > 0)
                    {
                        Class_BasicOperation.ShowMsg("", "به دلیل استفاده از این شیفت امکان حذف آن را ندارید", Class_BasicOperation.MessageType.Information);
                        return;
                    }
                    if (gridEX2.RowCount > 0)
                    {

                        table_105_DefinitionWorkShiftBindingSource.RemoveCurrent();
                        table_105_DefinitionWorkShiftTableAdapter.Update(dataSet_05_Product.Table_105_DefinitionWorkShift);
                        Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت حذف شد", Class_BasicOperation.MessageType.Information);
                    }
                }
            }
            else

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void Frm_020_shift_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }

           

            else if (e.Control && e.KeyCode == Keys.D)
            {
                btn_Delete_Click(sender, e);
            }
        }
    }
}
