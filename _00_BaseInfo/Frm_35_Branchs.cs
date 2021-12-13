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
    public partial class Frm_35_Branchs : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection conBase = new SqlConnection(Properties.Settings.Default.PBASE);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        public Frm_35_Branchs()
        {
            InitializeComponent();
        }

        private void Frm_35_Branchs_Load(object sender, EventArgs e)
        {



            gridEX1.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            this.table_85_BranchsTableAdapter.Fill(this.dataSet_05_PCLOR.Table_85_Branchs);
            this.table_001_PWHRSTableAdapter.Fill(this.dataSet_15_Ware.Table_001_PWHRS);
            this.table_95_DetailWareTableAdapter.Fill(this.dataSet_05_PCLOR.Table_95_DetailWare);

        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 27))
            {
                table_85_BranchsBindingSource.AddNew();
                txt_Name.Focus();
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
                if (txt_Name.Text == "" || cmb_Branch.Text.Trim() == "")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                }
                else
                {
                    if (((DataRowView)table_85_BranchsBindingSource.CurrencyManager.Current)["CodeBranch"].ToString().StartsWith("-"))
                    {
                        txt_Code.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "table_85_Branchs", "CodeBranch").ToString();
                    }
                    table_85_BranchsBindingSource.EndEdit();
                    table_85_BranchsTableAdapter.Update(dataSet_05_PCLOR.Table_85_Branchs);
                    table_95_DetailWareBindingSource.EndEdit();
                    table_95_DetailWareTableAdapter.Update(dataSet_05_PCLOR.Table_95_DetailWare);
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

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 28))
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataTable dt = ClDoc.ReturnTable(conBase, "select column01 from Table_045_PersonInfo where column133=" + ((DataRowView)table_85_BranchsBindingSource.CurrencyManager.Current)["ID"].ToString());
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("به دلیل استفاده از این شعبه در فرم اشخاص امکان حذف این اطلاعات وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                btn_New.Enabled = true;
                            }
                            else
                            {
                               



                                    ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_95_DetailWare where FK =" + txt_Id.Text + "");
                                    ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_85_Branchs where ID=" + txt_Id.Text + "");

                                    MessageBox.Show("اطلاعات با موفقیت حذف شد");

                                    dataSet_05_PCLOR.EnforceConstraints = false;
                                    this.table_85_BranchsTableAdapter.Fill(this.dataSet_05_PCLOR.Table_85_Branchs);
                                    this.table_95_DetailWareTableAdapter.Fill(this.dataSet_05_PCLOR.Table_95_DetailWare);
                                    dataSet_05_PCLOR.EnforceConstraints = true;
                                    btn_New.Enabled = true;
                                }
                           
                        }

                    }
                    else
                    {

                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void Frm_35_Branchs_KeyDown(object sender, KeyEventArgs e)
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

        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }


        private void cmb_Branch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.CheckedComboBox)
            {
                if (e.KeyChar != 13 && !char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.CheckedComboBox)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar != 13 && !char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else Class_BasicOperation.isEnter(e.KeyChar);
        }



        //private void gridEX1_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.table_85_BranchsBindingSource.EndEdit();
        //    }
        //    catch (Exception ex)
        //    {
        //        Class_BasicOperation.CheckExceptionType(ex, this.Name);
        //    }
        //}

        private void txt_Name_Leave(object sender, EventArgs e)
        {
            try
            {
                if (table_85_BranchsBindingSource.Count>0)
                {
                    
               
                if (((DataRowView)table_85_BranchsBindingSource.CurrencyManager.Current)["CodeBranch"].ToString().StartsWith("-"))
                {
                    txt_Code.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "table_85_Branchs", "CodeBranch").ToString();
                }
                if (txt_Name.Text == "") { MessageBox.Show("نام شعبه را وارد کنید"); txt_Name.Focus(); }
                else
                {
                    this.table_85_BranchsBindingSource.EndEdit();
                }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void cmb_Branch_Enter(object sender, EventArgs e)
        {
            try
            {
                this.table_85_BranchsBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }

        private void Frm_35_Branchs_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Frm_35_Branchs_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void table_85_BranchsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            uiPanel0.Enabled = false;
        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 63))
            {
                uiPanel0.Enabled = true;
            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }
    }
    
}
