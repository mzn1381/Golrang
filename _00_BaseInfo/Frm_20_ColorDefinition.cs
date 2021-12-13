using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR._00_BaseInfo
{
    public partial class Frm_20_ColorDefinition : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        public Frm_20_ColorDefinition()
        {
            InitializeComponent();
        }

        private void Frm_055_ColorDefinition_Load(object sender, EventArgs e)
        {
            

            this.table_055_ColorDefinitionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_055_ColorDefinition);
            gridEX1.DropDowns["Code"].DataSource = mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid,Column01,Column02 from table_004_CommodityAndIngredients where Column18=1");
            
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 2))
            {
            table_055_ColorDefinitionBindingSource.AddNew();
            mlt_Ware.Focus();
            btn_New.Enabled = false;
            uiPanel0.Enabled = true;
           
            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        
        {

            try
            {
                if (txt_NameColor.Text == "")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");

                }
                else
                {
                    table_055_ColorDefinitionBindingSource.EndEdit();
                    table_055_ColorDefinitionTableAdapter.Update(dataSet_05_PCLOR.Table_055_ColorDefinition);
                    gridEX1.MoveLast();
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                    btn_New.Enabled = true;
                    uiPanel0.Enabled = false;

                }



            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }
   
        private void Frm_055_ColorDefinition_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 3))
                    {

                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select id,CodeColore from Table_015_FormulColor where CodeColore="  +((DataRowView)table_055_ColorDefinitionBindingSource.CurrencyManager.Current)["ID"].ToString());

                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show(".از این رنگ در قسمت فرمول رنگ استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            else
                            {
                                if (gridEX1.RowCount > 0)
                                {
                                    table_055_ColorDefinitionBindingSource.RemoveCurrent();
                                    table_055_ColorDefinitionBindingSource.EndEdit();
                                    table_055_ColorDefinitionTableAdapter.Update(dataSet_05_PCLOR.Table_055_ColorDefinition);
                                    MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                    btn_New.Enabled = true;
                                }
                                else
                                {
                                    MessageBox.Show("سطری برای حذف کردن موجود نمی باشد ");
                                }

                            }

                        }

                    }
                    else

                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware, "Column02", "Column01");
            txt_NameColor.Text = mlt_Ware.Text;
        }

        private void Frm_055_ColorDefinition_KeyDown(object sender, KeyEventArgs e)
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

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_NameColor.Text == "")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید"); 
                    mlt_Ware.Focus();

                }
                else
                {
                    table_055_ColorDefinitionBindingSource.EndEdit();
                   

                }



            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); mlt_Ware.Focus();

            }
        }

        private void table_055_ColorDefinitionBindingSource_PositionChanged(object sender, EventArgs e)
        {
            uiPanel0.Enabled = false;
        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 62))
            {
                uiPanel0.Enabled = true;
            }
            else

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }
    }
}
