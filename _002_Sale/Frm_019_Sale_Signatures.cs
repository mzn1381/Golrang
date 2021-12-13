using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR._002_Sale
{
    public partial class Frm_019_Sale_Signatures : Form
    {
        BindingSource T125BindingSource = new BindingSource();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);

        public Frm_019_Sale_Signatures()
        {
            InitializeComponent();
        }

        private void Form11_Signatures_Load(object sender, EventArgs e)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_125_Signature where ColumnId=10", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            T125BindingSource.DataSource = Table;


            txt_Title1.DataBindings.Add("Text", T125BindingSource, "Column01");
            txt_Name1.DataBindings.Add("Text", T125BindingSource, "Column02");
            txt_Title2.DataBindings.Add("Text", T125BindingSource, "Column03");
            txt_Name2.DataBindings.Add("Text", T125BindingSource, "Column04");
            txt_Title3.DataBindings.Add("Text", T125BindingSource, "Column05");
            txt_Name3.DataBindings.Add("Text", T125BindingSource, "Column06");
            txt_Title4.DataBindings.Add("Text", T125BindingSource, "Column07");
            txt_Name4.DataBindings.Add("Text", T125BindingSource, "Column08");
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            foreach (Control item in explorerBarContainerControl1.Controls)
            {
                if (item is Janus.Windows.GridEX.EditControls.EditBox)
                {
                    if (item.Text.Trim() == "")
                        item.Text = "Null";
                    else item.Text="'"+item.Text+"'";
                }
            }
            using (SqlConnection Con=new SqlConnection(Properties.Settings.Default.PBASE))
            {
                Con.Open();
                SqlCommand Update = new SqlCommand("UPDATE Table_125_Signature Set Column01=" +
                    txt_Title1.Text.Trim() + ", Column02=" + txt_Name1.Text.Trim() + ", Column03=" +
                   txt_Title2.Text + ", Column04=" + txt_Name2.Text.Trim() + ", Column05=" + txt_Title3.Text.Trim() + 
                   ", Column06=" + txt_Name3.Text.Trim() + ", Column07=" + txt_Title4.Text.Trim() + ", Column08=" +
                   txt_Name4.Text.Trim() + " where ColumnId=10", Con);
                Update.ExecuteNonQuery();
            }


            foreach (Control item in explorerBarContainerControl1.Controls)
            {
                if (item is Janus.Windows.GridEX.EditControls.EditBox)
                {
                    if (item.Text.Trim() == "Null")
                        item.Text = null;
                    else item.Text = item.Text.Replace("'", "");
                }
            }
            Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
        }

        private void txt_Name4_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Form11_Signatures_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

      
    }
}
