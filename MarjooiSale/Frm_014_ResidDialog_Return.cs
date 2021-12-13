using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR.MarjooiSale
{
    public partial class Frm_014_ResidDialog_Return : Form
    {
        public string FunctionValue;
        public string WareCode;
        public int residnum=0;

        SqlConnection Conware = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);

        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        DataTable _Table = new DataTable();
        public Frm_014_ResidDialog_Return(DataTable Table)
        {
            InitializeComponent();
            _Table = Table;
        }

        private void Frm_010_DraftInformationDialog_Load(object sender, EventArgs e)
        {
            Conware.Open();

            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=0", Conware);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')", Conware);
            Adapter.Fill(DS, "Ware");
            mlt_Ware.DataSource = DS.Tables["Ware"];
            chk_DraftNum.Checked = false;
            txt_DraftNum.Enabled = false;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim()=="")
                    throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");

                if (  (chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) <= 0))
                    throw new Exception("اطلاعات مورد نیاز جهت صدور رسید انبار را کامل کنید");

                if (  (chk_DraftNum.Checked))
                {
                    int ok = 0;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                    {
                        Con.Open();
                        SqlCommand Select = new SqlCommand(@"IF EXISTS(
                                                       SELECT *
                                                       FROM   Table_011_PwhrsReceipt tpd
                                                       WHERE  tpd.column01 = " + txt_DraftNum.Value + @"
                                                   )
                                                    SELECT 0 AS ok 
                                                ELSE
                                                    SELECT 1 ok", Con);
                        ok = Convert.ToInt32(Select.ExecuteScalar().ToString());
                    }
                    if (ok == 0)
                        throw new Exception("این شماره رسید استفاده شده است");

                }

                foreach (DataRow item in _Table.Rows)
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item["GoodId"].ToString())))
                        throw new Exception("کالای " +
                            item["GoodName"].ToString() + " در انبار انتخاب شده فعال نمی باشد");

                }

                FunctionValue = mlt_Function.Value.ToString();
                WareCode = mlt_Ware.Value.ToString();
                residnum =Convert.ToInt32( txt_DraftNum.Value);
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

       

        private void mlt_Function_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Frm_010_DraftInformationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conware.Close();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Frm_014_ResidDialog_Return_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                uiButton1_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.Q)
                uiButton2_Click(sender, e);
        }

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DraftNum.Checked)
                txt_DraftNum.Enabled = true;
            else
                txt_DraftNum.Enabled = false;
        }
    }
}
