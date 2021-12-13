using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR._01_Frms
{
    public partial class _02_CompanyList : Form
    {
        public Int16 CompID = 0;
        public string CompName = null;
        public bool _FinType;
        public bool _WareType;
        public _02_CompanyList()
        {
            InitializeComponent();
        }

        private void Form04_CompList_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection Con = new SqlConnection(Class_BasicOperation.ConString());
                Con.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column00,Column01,Column15,Column16 from Table_000_OrgInfo", Con);
                DataTable T000 = new DataTable();
                Adapter.Fill(T000);
                gridEX1.DataSource = T000;
                Con.Close();
            }
            catch { }

        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            if (gridEX1.RowCount > 0)
            {
                CompID = Int16.Parse(gridEX1.GetValue("ColumnId").ToString());
                CompName = gridEX1.GetValue("Column01").ToString();
                _FinType =bool.Parse(gridEX1.GetValue("Column15").ToString());
                _WareType = bool.Parse(gridEX1.GetValue("Column16").ToString());
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                CompID = 0;
                CompName = null;
                _FinType = false;
                _WareType = false;
                this.DialogResult = DialogResult.No;
                this.Close();
            }

        }
    }
}
