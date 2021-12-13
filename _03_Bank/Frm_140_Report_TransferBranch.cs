using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR._03_Bank
{
    public partial class Frm_140_Report_TransferBranch : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);

        public Frm_140_Report_TransferBranch()
        {
            InitializeComponent();
        }

        private void Frm_140_Report_TransferBranch_Load(object sender, EventArgs e)
        {
            gridEX2.DropDowns["ToBank"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");

            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                gridEX2.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select * from Table_130_TransferBranch");


            }
            else
            {
                gridEX2.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select * from Table_130_TransferBranch where usersabt='"+Class_BasicOperation._UserName+"'");
            }

            gridEX2.RemoveFilters();
            gridEX2.UpdateData();
        }

        private void tsexcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXExporter1.GridEX = gridEX2;
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
            }
        }
    }
}
