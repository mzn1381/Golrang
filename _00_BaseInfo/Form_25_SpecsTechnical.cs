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
    public partial class Form_25_SpecsTechnical : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Form_25_SpecsTechnical()
        {
            InitializeComponent();
        }

        private void Form_25_SpecsTechnical_Load(object sender, EventArgs e)
        {
            this.table_60_SpecsTechnicalTableAdapter.Fill(this.dataSet_05_PCLOR.Table_60_SpecsTechnical);
            txt_NameColor.Focus();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 11))
            {
                table_60_SpecsTechnicalBindingSource.AddNew();
                //((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["IsForProduction"] = true;
                //((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["IsForColor"] = false;
                ((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["status"] = true;
                ((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["X"] = xNumeric.Value;
                ((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["Y"] = yNumeric.Value;
                txt_NameColor.Focus();
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
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید"); return;
                }
                if (((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["Code"].ToString().StartsWith("-"))
                    txt_Code.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "table_60_SpecsTechnical", "Code").ToString();
                int Position = gridEX1.CurrentRow.RowIndex;
                table_60_SpecsTechnicalBindingSource.EndEdit();
                table_60_SpecsTechnicalTableAdapter.Update(dataSet_05_PCLOR.Table_60_SpecsTechnical);
                gridEX1.MoveTo(Position);
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                btn_New.Enabled = true;
                uiPanel0.Enabled = false;
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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 12))
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select id  from Table_020_DetailReciptClothRaw where Machine=" + gridEX1.GetValue("ID"));
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show(".از این دستگاه در قسمت رسید پارچه ها استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                table_60_SpecsTechnicalBindingSource.RemoveCurrent();
                                table_60_SpecsTechnicalBindingSource.EndEdit();
                                table_60_SpecsTechnicalTableAdapter.Update(dataSet_05_PCLOR.Table_60_SpecsTechnical);
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

        private void Form_25_SpecsTechnical_KeyDown(object sender, KeyEventArgs e)
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

        private void txt_NameColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
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

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 59))
            {
                uiPanel0.Enabled = true;
            }
            else

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

        }

        private void table_60_SpecsTechnicalBindingSource_PositionChanged(object sender, EventArgs e)
        {
            uiPanel0.Enabled = false;
        }

        private void fillByMZNToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_60_SpecsTechnicalTableAdapter.FillByMZN(this.dataSet_05_PCLOR.Table_60_SpecsTechnical);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gridEX1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void gridEX1_RowCheckStateChanging(object sender, Janus.Windows.GridEX.RowCheckStateChangingEventArgs e)
        {

        }

        private void gridEX1_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {

        }

        private void radioForColor_Click(object sender, EventArgs e)
        {

        }

        private void radioForProduction_Click(object sender, EventArgs e)
        {

        }

        private void gridEX1_Click(object sender, EventArgs e)
        {

        }

        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {

            //radioButton2.Checked = true;
            //txt_Code.Text = "@@@@";
            uiPanel0.Enabled = true;
            //xNumeric.Value
            xNumeric.Value = Convert.ToDecimal(((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["X"].ToString());
            yNumeric.Value = Convert.ToDecimal(((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["Y"].ToString());
            checkStatus.Checked = (bool)((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["status"];

        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {

        }

        private void xNumeric_ValueChanged(object sender, EventArgs e)
        {
            ((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["X"] = xNumeric.Value;
        }

        private void yNumeric_ValueChanged(object sender, EventArgs e)
        {
            ((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["Y"] = yNumeric.Value;
        }

        private void checkStatus_CheckedChanged(object sender, EventArgs e)
        {
            ((DataRowView)table_60_SpecsTechnicalBindingSource.CurrencyManager.Current)["status"] = checkStatus.Checked;
        }
    }
}
