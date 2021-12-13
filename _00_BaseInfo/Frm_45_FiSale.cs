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
    public partial class Frm_45_FiSale : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Ficolor;
        public Frm_45_FiSale()
        {
            InitializeComponent();
        }

        private void Frm_45_FiSale_Load(object sender, EventArgs e)
        {
            
            this.table_005_TypeClothTableAdapter.Fill(this.dataSet_05_PCLOR.Table_005_TypeCloth);
            gridEX1.DropDowns["Codecommodity"].DataSource = clDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from table_004_CommodityAndIngredients");
            mlt_Color.DataSource = clDoc.ReturnTable(ConPCLOR, @"select * from  Table_010_TypeColor");
        }
        string NumberR = "";
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Color.Text == "" || mlt_Color.Text == "0")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                    return;
                }
                string CommandTex = "";
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetCheckedRows())
                {
                    string ficloth = clDoc.ExScalar(ConPCLOR.ConnectionString, @" select isnull ((SELECT     FiSale  FROM     dbo.Table_005_TypeCloth  WHERE     (ID = " + Row.Cells["ID"].Value.ToString() + ")),0)");
                    string ficolor = clDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( SELECT     FiColor FROM         dbo.Table_010_TypeColor WHERE     (ID = " + mlt_Color.Value.ToString() + ")),0)");
                    decimal sum = Convert.ToDecimal(ficloth) + Convert.ToDecimal(ficolor);

                    
                    if (Row.Cells["SelectBrand"].Value.ToString() == "True")
                    {
                      CommandTex=CommandTex+ @" Update Table_005_TypeCloth set SumFi = " + sum + " where ID=" +Row.Cells["ID"].Value.ToString() + ";";
                    }

                    if (Row.Cells["SelectBrand"].Value.ToString() == "False")
                    {
                        CommandTex=CommandTex+ @" Update Table_005_TypeCloth set SumFi = " + ficloth + "  where ID=" + Row.Cells["ID"].Value.ToString() + "; ";
                    }
                   
                }
                if(CommandTex.Length>0)
                 clDoc.Execute(ConPCLOR.ConnectionString,CommandTex);

               CommandTex="";
               
                table_005_TypeClothBindingSource.EndEdit();
                table_005_TypeClothTableAdapter.Update(dataSet_05_PCLOR.Table_005_TypeCloth);
                table_005_TypeClothTableAdapter.Fill(dataSet_05_PCLOR.Table_005_TypeCloth);
                MessageBox.Show("اطلاعات با موفقیت ثبت شد");
          
        }
            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex, this.Name); }
        }
        private void Frm_45_FiSale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

        }
    }
}
