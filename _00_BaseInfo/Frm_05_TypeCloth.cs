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
    public partial class Frm_05_TypeCloth : Form
    {
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Frm_05_TypeCloth()
        {
            InitializeComponent();
        }

        

        private void Frm_05_TypeCloth_Load(object sender, EventArgs e)
        {
           
            try
            {
                this.table_005_TypeClothTableAdapter.Fill(this.dataSet_05_PCLOR1.Table_005_TypeCloth);
                gridEX1.DropDowns["Code"].DataSource = mlt_Commodity.DataSource = ClDoc.ReturnTable(ConPWHRS, @"select Columnid,Column01,Column02 from table_004_CommodityAndIngredients where column19=1");
           
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
              
            }        
           
            

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes. Class_UserScope UserScope = new  Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 8))
                {
                        table_005_TypeClothBindingSource.AddNew();
                        btn_New.Enabled = false;
                        uiPanel0.Enabled = true;
                        mlt_Commodity.Focus();
                     }
                else{
            
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
    
        }
        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTitleCloth.Text =="")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");

                }

                else
                {
                    if (((DataRowView)table_005_TypeClothBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                    {
                        txtId.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, " table_005_TypeCloth ", "Number").ToString();
                    }

                    table_005_TypeClothBindingSource.EndEdit();
                    table_005_TypeClothTableAdapter.Update(this.dataSet_05_PCLOR1.Table_005_TypeCloth);

                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                    btn_New.Enabled = true;
                    uiPanel0.Enabled = false;
                }

                
            }
            catch (Exception ex)
            {
                
               Class_BasicOperation.CheckExceptionType(ex,this.Name);

            }
           
           
            
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 9))
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select id  from Table_020_DetailReciptClothRaw where TypeCloth=" +((DataRowView)table_005_TypeClothBindingSource.CurrencyManager.Current)["ID"].ToString());
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show(".از این پارچه در قسمت رسید پارچه ها استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                table_005_TypeClothBindingSource.RemoveCurrent();
                                table_005_TypeClothBindingSource.EndEdit();
                                table_005_TypeClothTableAdapter.Update(this.dataSet_05_PCLOR1.Table_005_TypeCloth);
                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                btn_New.Enabled = true;
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
        
     

        private void mlt_Commodity_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Commodity, "Column02", "Column01");
         
            txtTitleCloth.Text = mlt_Commodity.Text;
        }

      

        private void Frm_05_TypeCloth_KeyDown(object sender, KeyEventArgs e)
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

        private void mlt_Commodity_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtTitleCloth.Text == "")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");

                }

                else
                {
                    if (((DataRowView)table_005_TypeClothBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                    {
                        txtId.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, " table_005_TypeCloth ", "Number").ToString();
                    }

                    table_005_TypeClothBindingSource.EndEdit();
                     

                }


            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); mlt_Commodity.Focus();

            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void table_005_TypeClothBindingSource_PositionChanged(object sender, EventArgs e)
        {
            uiPanel0.Enabled = false;
        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 60))
            {

                uiPanel0.Enabled = true;
            }
            else

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

       
    }
}
