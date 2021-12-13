using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;

namespace PCLOR._00_BaseInfo
{
    public partial class Frm_10_TypeColor : Form
    {
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Frm_10_TypeColor()
        {
            InitializeComponent();
        }

        private void Frm_10_TypeColor_Load(object sender, EventArgs e)
        {
          
            this.table_010_TypeColorTableAdapter.Fill(dataSet_05_PCLOR.Table_010_TypeColor);
            this.table_015_FormulColorTableAdapter.Fill(dataSet_05_PCLOR.Table_015_FormulColor);
            gridEX2.DropDowns["Commodity"].DataSource = ClDoc.ReturnTable(ConPCLOR, @" select ID,NameColor from Table_055_ColorDefinition");
           
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 5))
            {
                table_010_TypeColorBindingSource.AddNew();
                btn_New.Enabled = false;
                uiPanel1.Enabled = true;
                txtTitleColor.Enabled = true;

                txtTitleColor.Focus();
            }
            else {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTitleColor.Text == "" || gridEX2.RowCount==0)
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");

                }
                else
                {
                    gridEX2.UpdateData();
                   this.table_010_TypeColorBindingSource.EndEdit();
                   this. table_010_TypeColorTableAdapter.Update(dataSet_05_PCLOR.Table_010_TypeColor);
                   this.table_015_FormulColorBindingSource.EndEdit();
                   this.table_015_FormulColorTableAdapter.Update(dataSet_05_PCLOR.Table_015_FormulColor);
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                    btn_New.Enabled = true;
                    uiPanel1.Enabled = false;
                    txtTitleColor.Enabled = false;

                }
               
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {
                table_010_TypeColorBindingSource.EndEdit();

            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX2_Error(object sender, ErrorEventArgs e)
        {
            try
            {
                e.DisplayErrorMessage = false;

            }
            catch (Exception)
            {
                Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);

            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 6))
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select id  from Table_030_DetailOrderColor where TypeColor=" + gridEX1.GetValue("ID"));
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show(".از این رنگ در قسمت سفارش رنگ استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                               

                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_015_FormulColor where FK =" + txtId.Text + "");
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_010_TypeColor where ID=" + txtId.Text + "");
                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_010_TypeColorTableAdapter.Fill(dataSet_05_PCLOR.Table_010_TypeColor);
                                this.table_015_FormulColorTableAdapter.Fill(dataSet_05_PCLOR.Table_015_FormulColor);
                                dataSet_05_PCLOR.EnforceConstraints = true;
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

        private void Frm_10_TypeColor_KeyDown(object sender, KeyEventArgs e)
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

      
        private void txtTitleColor_KeyPress_2(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            { gridEX2.Focus(); gridEX2.Col = 0; }
         
        }
        void FilterGridExDropDown(object sender, string ColumnName, string TextualText, string SearchText)
        {
            try
            {

                Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                ConditionOperator.Contains, SearchText);
                ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
            }

            catch { }
        }

        private void gridEX2_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;

            if (e.Column.Key == "CodeColore")
            {
                FilterGridExDropDown(sender, "ID", "NameColor", gridEX2.EditTextBox.Text);
            }
        }

        private void table_010_TypeColorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (gridEX2.RowCount>0)
            {
                uiPanel1.Enabled = false;
            txtTitleColor.Enabled = false;
            }
            
        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
             Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
             if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 61))
             {
                 uiPanel1.Enabled = true;
                 txtTitleColor.Enabled = true;
             }
             else

                 Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

        }
 

     

    }
}
