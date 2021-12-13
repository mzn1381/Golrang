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
    public partial class _03_FinanceYearList : Form
    {
        Int16 _CompID;
        public string _Year,lastUpdate=null;
        public _03_FinanceYearList(Int16 CmpID)
        {
            InitializeComponent();
            _CompID = CmpID;
        }

        private void Form05_FinanceYear_Load(object sender, EventArgs e)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConString());
            Con.Open();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column03,Column04 from Table_005_FinanceYear where Column01=" + _CompID, Con);
            DataTable T005 = new DataTable();
            Adapter.Fill(T005);
            gridEX2.DataSource = T005;
            Con.Close();
        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            if (gridEX2.RowCount > 0)
            {
                _Year = gridEX2.GetValue("Column00").ToString();
                lastUpdate = gridEX2.GetValue("Column04").ToString();
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
                _Year = null;
        }
    }
}
