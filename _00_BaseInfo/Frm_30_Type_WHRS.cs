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
using System.IO;
using DevComponents.DotNetBar;

namespace PCLOR._00_BaseInfo
{
    public partial class Frm_30_Type_WHRS : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        DataTable dt;
        public Frm_30_Type_WHRS()
        {
            InitializeComponent();
        }

        private void Frm_30_WHRS_Load(object sender, EventArgs e)
        {
         
            this.table_90_Wares6TableAdapter.Fill(this.dataSet_05_PCLOR.Table_90_Wares6);
            
            this.table_90_Wares5TableAdapter.Fill(this.dataSet_05_PCLOR.Table_90_Wares5);
          
            this.table_90_Wares4TableAdapter.Fill(this.dataSet_05_PCLOR.Table_90_Wares4);
          
            this.table_90_Wares3TableAdapter.Fill(this.dataSet_05_PCLOR.Table_90_Wares3);
            
            this.table_90_Wares2TableAdapter.Fill(this.dataSet_05_PCLOR.Table_90_Wares2);
           
            this.table_90_Wares1TableAdapter.Fill(this.dataSet_05_PCLOR.Table_90_Wares1);
          
            

            gridEX2.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            gridEX3.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            gridEX4.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            gridEX5.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            gridEX6.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            gridEX1.DropDowns["Ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");

            

                

           
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

            
          
           

           
        }

        private void Frm_30_WHRS_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void gridEX6_AddingRecord(object sender, CancelEventArgs e)

        {
            
        }

        private void gridEX5_AddingRecord(object sender, CancelEventArgs e)
        {
      
        }

        private void gridEX4_AddingRecord(object sender, CancelEventArgs e)
        {
        
        }

        private void gridEX3_AddingRecord(object sender, CancelEventArgs e)
        {
           
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
           
        }

        private void btn_Save_Click_1(object sender, EventArgs e)
        {
            table_90_Wares1BindingSource.EndEdit();
            table_90_Wares1TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares1);

            table_90_Wares2BindingSource.EndEdit();
            table_90_Wares2TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares2);

            table_90_Wares3BindingSource.EndEdit();
            table_90_Wares3TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares3);

            table_90_Wares4BindingSource.EndEdit();
            table_90_Wares4TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares4);

            table_90_Wares5BindingSource.EndEdit();
            table_90_Wares5TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares5);
           
            
            table_90_Wares6BindingSource.EndEdit();
            table_90_Wares6TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares6);
            
            
            
            
            MessageBox.Show("اطلاعات با موفقیت ثبت شد");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
       
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {

                if (gridEX6.Focused)
                {
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID, Value FROM    dbo.Table_80_Setting where ID=14 AND Value=" + ((DataRowView)table_90_Wares1BindingSource.CurrencyManager.Current)["IdWare"].ToString() + "");

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("از این انبار استفاده شده است امکان حذف آن را ندارید");

                    }
                    else
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_90_Wares1BindingSource.RemoveCurrent();
                            table_90_Wares1BindingSource.EndEdit();
                            table_90_Wares1TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares1);
                            table_90_Wares1TableAdapter.Fill(dataSet_05_PCLOR.Table_90_Wares1);
                            MessageBox.Show("حذف با موفقیت انجام شد");


                        }

                    }

                }
                else if (gridEX5.Focused)
                {
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID, Value FROM    dbo.Table_80_Setting where ID=15 AND Value=" + ((DataRowView)table_90_Wares2BindingSource.CurrencyManager.Current)["IdWare"].ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("از این انبار استفاده شده است امکان حذف آن را ندارید");

                    }
                    else
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_90_Wares2BindingSource.RemoveCurrent();
                            table_90_Wares2BindingSource.EndEdit();
                            table_90_Wares2TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares2);
                            table_90_Wares2TableAdapter.Fill(dataSet_05_PCLOR.Table_90_Wares2);

                            MessageBox.Show("حذف با موفقیت انجام شد");
                        }
                    }
                }
                else if (gridEX4.Focused)
                {
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID, Value FROM    dbo.Table_80_Setting where ID=16 AND Value=" + ((DataRowView)table_90_Wares3BindingSource.CurrencyManager.Current)["IdWare"].ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("از این انبار استفاده شده است امکان حذف آن را ندارید");

                    }
                    else
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_90_Wares3BindingSource.RemoveCurrent();
                            table_90_Wares3BindingSource.EndEdit();
                            table_90_Wares3TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares3);
                            table_90_Wares3TableAdapter.Fill(dataSet_05_PCLOR.Table_90_Wares3);
                            MessageBox.Show("حذف با موفقیت انجام شد");
                        }
                    }
                }
                else if (gridEX3.Focused)
                {
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID, Value FROM    dbo.Table_80_Setting where ID=17 AND Value=" + ((DataRowView)table_90_Wares4BindingSource.CurrencyManager.Current)["IdWare"].ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("از این انبار استفاده شده است امکان حذف آن را ندارید");

                    }
                    else
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_90_Wares4BindingSource.RemoveCurrent();
                            table_90_Wares4BindingSource.EndEdit();
                            table_90_Wares4TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares4);
                            table_90_Wares4TableAdapter.Fill(dataSet_05_PCLOR.Table_90_Wares4);
                            MessageBox.Show("حذف با موفقیت انجام شد");
                        }
                    }
                }
                else if (gridEX2.Focused)
                {
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID, Value FROM    dbo.Table_80_Setting where ID=18 AND Value=" + ((DataRowView)table_90_Wares5BindingSource.CurrencyManager.Current)["IdWare"].ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("از این انبار استفاده شده است امکان حذف آن را ندارید");

                    }
                    else
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_90_Wares5BindingSource.RemoveCurrent();
                            table_90_Wares5BindingSource.EndEdit();
                            table_90_Wares5TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares5);
                            table_90_Wares5TableAdapter.Fill(dataSet_05_PCLOR.Table_90_Wares5);
                            MessageBox.Show("حذف با موفقیت انجام شد");
                        }
                    }
                }

                else if (gridEX1.Focused)
                {
                    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID, Value FROM    dbo.Table_80_Setting where ID=19 AND Value=" + ((DataRowView)table_90_Wares6BindingSource.CurrencyManager.Current)["IdWare"].ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("از این انبار استفاده شده است امکان حذف آن را ندارید");

                    }
                    else
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_90_Wares6BindingSource.RemoveCurrent();
                            table_90_Wares6BindingSource.EndEdit();
                            table_90_Wares6TableAdapter.Update(dataSet_05_PCLOR.Table_90_Wares6);
                            table_90_Wares6TableAdapter.Fill(dataSet_05_PCLOR.Table_90_Wares6);
                            MessageBox.Show("حذف با موفقیت انجام شد");
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
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
        private void gridEX6_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX6.CurrentCellDroppedDown = true;

            if (e.Column.Key == "IdWare")
            {
                FilterGridExDropDown(sender, "Column01", "column02", gridEX6.EditTextBox.Text);
            }
        }

        private void gridEX5_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX5.CurrentCellDroppedDown = true;

            if (e.Column.Key == "IdWare")
            {
                FilterGridExDropDown(sender, "Column01", "column02", gridEX5.EditTextBox.Text);
            }
        }

        private void gridEX4_CellUpdated(object sender, ColumnActionEventArgs e)
        {

        }

        private void gridEX4_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX4.CurrentCellDroppedDown = true;

            if (e.Column.Key == "IdWare")
            {
                FilterGridExDropDown(sender, "Column01", "column02", gridEX4.EditTextBox.Text);
            }
        }

        private void gridEX3_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX3.CurrentCellDroppedDown = true;

            if (e.Column.Key == "IdWare")
            {
                FilterGridExDropDown(sender, "Column01", "column02", gridEX3.EditTextBox.Text);
            }
        }

        private void gridEX2_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;

            if (e.Column.Key == "IdWare")
            {
                FilterGridExDropDown(sender, "Column01", "column02", gridEX2.EditTextBox.Text);
            }
        }

        private void Frm_30_Type_WHRS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click_1(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {
                btn_Delete_Click(sender, e);
            }
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

            if (e.Column.Key == "IdWare")
            {
                FilterGridExDropDown(sender, "Column01", "column02", gridEX1.EditTextBox.Text);
            }
        }
        
    }

}
