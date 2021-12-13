using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR.Product
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_125_DetailTypeCotton' table. You can move, or remove it, as needed.
            this.table_125_DetailTypeCottonTableAdapter.Fill(this.dataSet_05_Product.Table_125_DetailTypeCotton);
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_125_DetailTypeCotton' table. You can move, or remove it, as needed.
            this.table_125_DetailTypeCottonTableAdapter.Fill(this.dataSet_05_Product.Table_125_DetailTypeCotton);
            // TODO: This line of code loads data into the 'dataSet_05_Product.Table_100_ProgramMachine' table. You can move, or remove it, as needed.
            this.table_100_ProgramMachineTableAdapter.Fill(this.dataSet_05_Product.Table_100_ProgramMachine);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            table_100_ProgramMachineBindingSource.AddNew();
        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            table_100_ProgramMachineBindingSource.EndEdit();
        }
    }
}
