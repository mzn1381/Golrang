using Janus.Windows.GridEX;
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
    public partial class Frm_135_RFID : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Frm_135_RFID()
        {
            InitializeComponent();
        }

        private void Frm_135_RFID_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_135_RFIDPerson' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_100_ProgramMachine' table. You can move, or remove it, as needed.
            gridEX2.DropDowns["Customer"].DataSource = ClDoc.ReturnTable(ConBase, @"select * from Table_045_PersonInfo");
            this.table_135_RFIDPersonTableAdapter.Fill(this.dataSet_05_Product.Table_135_RFIDPerson);

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_135_RFIDPersonBindingSource.EndEdit();
                table_135_RFIDPersonTableAdapter.Update(dataSet_05_Product.Table_135_RFIDPerson);
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Violation of PRIMARY KEY constraint 'PK_Table_135_RFIDPerson'. Cannot insert duplicate key in object 'dbo.Table_135_RFIDPerson'. The duplicate key"))
                {
                    MessageBox.Show(".مورد نظر تکراری می باشد RFID");
                    return;
                }
               
            }
           
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select RFID from Table_115_Product where Id = "+gridEX2.GetValue("ID")+"");
            if (dt.Rows.Count>0)
            {
                Class_BasicOperation.ShowMsg("", "این شخص کارت تولید ثبت کرده است امکان حذف RFID آن را ندارید.", Class_BasicOperation.MessageType.Stop);
                return;
            }
            ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"DELETE Table_135_RFIDPerson where ID="+ gridEX2.GetValue("ID") + "");
            //table_135_RFIDPersonBindingSource.RemoveCurrent();
            //table_135_RFIDPersonBindingSource.EndEdit();
            //table_135_RFIDPersonTableAdapter.Update(dataSet_05_Product.Table_135_RFIDPerson);

            MessageBox.Show("اطلاعات با موفقیت دخیره شد");
            table_135_RFIDPersonTableAdapter.Fill(dataSet_05_Product.Table_135_RFIDPerson);
        
                }

        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;

            if (e.Column.Key == "Person")
            {
                FilterGridExDropDown(sender, "Column02", "Column01", gridEX2.EditTextBox.Text);
                //Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "Column01", "", "");
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
    }
}
