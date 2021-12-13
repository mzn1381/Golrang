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
    public partial class Frm_004_UpdateBrand : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        public decimal Price = 0;
     
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
       

        string ficloth;
        string ficolor;
        string SelectBrand;
        decimal sum;
        
        public Frm_004_UpdateBrand()
        {
            InitializeComponent();
        }

        private void Frm_004_UpdateBrand_Load(object sender, EventArgs e)
        {
            bool del = true;
            int ID=0;
            string frmNum;
            //Frm_002_StoreFaktor frm = new Frm_002_StoreFaktor(del,ID);
           
            //frmNum =frm.txt_Number.Text;
            mlt_Color.DataSource = clDoc.ReturnTable(ConPCLOR, @"select * from  Table_010_TypeColor");

                mlt_Cloth.DataSource = clDoc.ReturnTable(ConPCLOR, @"SELECT  *  from  Table_005_TypeCloth");

       }
   

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Color.Text == "" || mlt_Color.Text == "" || txt_Fi.Text=="")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                    return;
                }

                if (txt_Fi.Text != "")
                {
                    Price = Convert.ToDecimal(txt_Fi.Text);
                    MessageBox.Show(".قیمت پیشنهادی با موفقیت ثبت شد");
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
           
        }

   

    
        private void mlt_Color_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void Mlt_Cloth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void Frm_004_UpdateBrand_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Mlt_Color_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Cloth, "TypeColor", "ID");
        }

        private void Mlt_Cloth_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Color, "TypeCloth", "ID");

        }
    }
}
