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
    public partial class Frm_30_WHRS : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Frm_30_WHRS()
        {
            InitializeComponent();
        }

        private void Frm_30_WHRS_Load(object sender, EventArgs e)
        {
            try
            {

                mlt_Function_D.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_knittingDraft.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_Other_D.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_Packaging_D.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_Product_D.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_Packaging.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function_Product.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_knittingReceipt.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function_Spare.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function_Spare_D.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_Other.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function_Other.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function_Sale.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_type_marjoei.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_TypeReturnPack.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_TypeReturnDrafPack.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");


                mlt_Cloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @" select ID,TypeCloth from Table_005_TypeCloth");
                mlt_Color.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeColor from Table_010_TypeColor ");
                mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,namemachine from Table_60_SpecsTechnical");
                mlt_Commodity.DataSource = ClDoc.ReturnTable(ConPWHRS, @"select Columnid,Column01,Column02 from table_004_CommodityAndIngredients where column19=1");
                mlt_NameCustomer.DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column01 from Table_045_PersonInfo");





                mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 1)");



                mlt_Ware_T.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 2)");



                mlt_Ware_M.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");


                mlt_Ware_S.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 4)");


                mlt_Ware_O.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 5)");

                mlt_marjoei_recipt.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");


                mlt_ReturnPack.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");

                mlt_KnittingWare.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 1)");


                DataTable dt = ClDoc.ReturnTable(ConPCLOR, "select * from Table_80_Setting");

                mlt_Function.Value = dt.Rows[0][2];
                mlt_knittingReceipt.Value = dt.Rows[29][2];
                mlt_Function_D.Value = dt.Rows[1][2];
                mlt_knittingDraft.Value = dt.Rows[31][2];

                mlt_Function_Product.Value = dt.Rows[2][2];
                mlt_Function_Product_D.Value = dt.Rows[3][2];

                mlt_Function_Packaging.Value = dt.Rows[4][2];
                mlt_Function_Packaging_D.Value = dt.Rows[5][2];

                mlt_Function_Other.Value = dt.Rows[6][2];

                mlt_Function_Other.Value = dt.Rows[6][2];
                mlt_Function_Other_D.Value = dt.Rows[7][2];

                mlt_Function_Spare.Value = dt.Rows[8][2];
                mlt_Function_Spare_D.Value = dt.Rows[9][2];


                mlt_Ware.Value = dt.Rows[13][2];
                mlt_KnittingWare.Value = dt.Rows[30][2];
                mlt_Ware_T.Value = dt.Rows[14][2];
                mlt_Ware_M.Value = dt.Rows[15][2];
                mlt_Ware_S.Value = dt.Rows[16][2];
                mlt_Ware_O.Value = dt.Rows[17][2];
                mlt_Cloth.Value = dt.Rows[18][2];
                mlt_Machine.Value = dt.Rows[19][2];
                mlt_Color.Value = dt.Rows[20][2];
                txt_Weight.Text = dt.Rows[21][2].ToString();
                mlt_NameCustomer.Value = dt.Rows[22][2].ToString();
                mlt_Commodity.Value = dt.Rows[23][2].ToString();
                mlt_marjoei_recipt.Value = dt.Rows[24][2];
                mlt_type_marjoei.Value = dt.Rows[25][2];
                mlt_ReturnPack.Value = dt.Rows[27][2];
                mlt_TypeReturnPack.Value = dt.Rows[28][2];
                mlt_TypeReturnDrafPack.Value = dt.Rows[32][2];
                checkRegisterAutomaticProduct.Checked = Convert.ToBoolean(Convert.ToInt32(dt.Rows[33][2]) < 1 ? "false" : "true");

            }
            catch (Exception ex)
            {
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (mlt_knittingReceipt.Text == "" || mlt_Function.Text == "" || mlt_Function_Product.Text == "" || mlt_Function_Product.Text == "" || mlt_Function_Product_D.Text == "" || mlt_Function_Packaging.Text == "" || mlt_Function_Packaging_D.Text == ""
                || mlt_Function_Other.Text == "" || mlt_Function_Spare.Text == "" || mlt_Function_Spare_D.Text == ""
                || mlt_Ware.Text == "" || mlt_KnittingWare.Text == "" || mlt_Ware_T.Text == "" || mlt_Ware_M.Text == "" || mlt_Ware_S.Text == "" || mlt_Ware_O.Text == "" || mlt_Color.Text == "" || mlt_Cloth.Text == "" || mlt_Machine.Text == "" || txt_Weight.Text == "" || mlt_TypeReturnDrafPack.Text == "")
            {
                MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                return;
            }
            var IsCheck = checkRegisterAutomaticProduct.Checked ? 1 : 0;
            ClDoc.Execute(ConPCLOR.ConnectionString,
                " Update Table_80_Setting set value="
                + mlt_Function.Value + " where id=1; Update Table_80_Setting set value ="
                + mlt_knittingReceipt.Value + " where id=30; Update Table_80_Setting set value ="
                + mlt_Function_D.Value + " where id=2;  Update Table_80_Setting set value="
                + mlt_knittingDraft.Value + " where id=32;  Update Table_80_Setting set value="
                + mlt_Function_Product.Value + "where id=3; Update Table_80_Setting set value="
                + mlt_Function_Product_D.Value + "where id=4; Update Table_80_Setting set value="
                + mlt_Function_Packaging.Value + "where id=5; Update Table_80_Setting set value="
                + mlt_Function_Packaging_D.Value + "where id=6 ;Update Table_80_Setting set value="
                + mlt_Function_Other.Value + "where id=7;Update Table_80_Setting set value="
                + mlt_Function_Other_D.Value + " where id=8;Update Table_80_Setting set value="
                + mlt_Function_Spare.Value + "where id=9;Update Table_80_Setting set value="
                + mlt_Function_Spare_D.Value + "where id=10;Update Table_80_Setting set value="
                + mlt_Ware.Value + "where id=14; Update Table_80_Setting set value="
                + mlt_KnittingWare.Value + "where id=31; Update Table_80_Setting set value="
                + mlt_Ware_T.Value + "where id=15;Update Table_80_Setting set value="
                + mlt_Ware_M.Value + "where id=16;Update Table_80_Setting set value="
                + mlt_Ware_S.Value + "where id=17;Update Table_80_Setting set value="
                + mlt_Ware_O.Value + "where id=18;Update Table_80_Setting set value="
                 + mlt_Cloth.Value + "where id=19; Update Table_80_Setting set value=N'"
                 + mlt_Machine.Text + "' where id=20;Update Table_80_Setting set value=N'"
                 + mlt_Color.Text + "' where id=21;Update Table_80_Setting set value="
                 + txt_Weight.Text + " where id =22;Update Table_80_Setting set value="
                 + mlt_NameCustomer.Value + " where id=23;Update Table_80_Setting set value="
                 + mlt_Commodity.Value + "where id=24 ;Update Table_80_Setting set value="
                 + mlt_marjoei_recipt.Value + " where id=25;Update Table_80_Setting set value="
                 + mlt_type_marjoei.Value + "where id=26;Update Table_80_Setting set value="
                 + mlt_ReturnPack.Value + "where id=28;Update Table_80_Setting set value="
                 + mlt_TypeReturnPack.Value + "where id=29;Update Table_80_Setting set value="
                 + mlt_TypeReturnDrafPack.Value + "where id=33;Update Table_80_Setting set value="
                 + IsCheck+"where id=34"

                 );
            Properties.Settings.Default.TypePWHRS = mlt_Function_Sale.Value.ToString();
            MessageBox.Show("اطلاعات با موفقیت ثبت شد");
        }

        private void Frm_30_WHRS_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void column03Label_Click(object sender, EventArgs e)
        {

        }

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware, "Column02", "TypeWare");
        }

        private void mlt_Ware_T_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_T, "Column02", "TypeWare");
        }

        private void mlt_Ware_M_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_M, "Column02", "TypeWare");

        }

        private void mlt_Ware_S_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_S, "Column02", "TypeWare");

        }

        private void mlt_Ware_O_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_O, "Column02", "TypeWare");

        }

        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "Column02", "Column01");

        }

        private void mlt_Function_D_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_D, "Column02", "Column01");

        }

        private void mlt_Function_Product_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_Product, "Column02", "Column01");

        }

        private void mlt_Function_Product_D_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_Product_D, "Column02", "Column01");

        }

        private void mlt_Function_Packaging_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_Packaging, "Column02", "Column01");

        }

        private void mlt_Function_Packaging_D_ValueChanged(object sender, EventArgs e)
        {

            Class_BasicOperation.FilterMultiColumns(mlt_Function_Packaging_D, "Column02", "Column01");

        }

        private void mlt_Function_Spare_ValueChanged(object sender, EventArgs e)
        {

            Class_BasicOperation.FilterMultiColumns(mlt_Function_Spare, "Column02", "Column01");

        }

        private void mlt_Function_Spare_D_ValueChanged(object sender, EventArgs e)
        {

            Class_BasicOperation.FilterMultiColumns(mlt_Function_Spare_D, "Column02", "Column01");

        }

        private void mlt_Function_Other_ValueChanged(object sender, EventArgs e)
        {

            Class_BasicOperation.FilterMultiColumns(mlt_Function_Other, "Column02", "Column01");

        }

        private void mlt_Function_Other_D_ValueChanged(object sender, EventArgs e)
        {

            Class_BasicOperation.FilterMultiColumns(mlt_Function_Other_D, "Column02", "Column01");

        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Frm_30_WHRS_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }

        }

        private void mlt_Cloth_ValueChanged(object sender, EventArgs e)
        {

        }

        private void mlt_Cloth_ValueChanged_1(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Cloth, "TypeCloth", "ID");

        }

        private void mlt_Machine_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Machine, "namemachine", "ID");

        }

        private void mlt_Color_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Color, "TypeColor", "ID");

        }

        private void mlt_NameCustomer_ValueChanged(object sender, EventArgs e)
        {

        }

        private void mlt_NameCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, null, "Column01");
        }

        private void mlt_Commodity_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Commodity, "Column02", "Column01");
        }

        private void BindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void mlt_marjoei_recipt_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_marjoei_recipt, "Column02", "TypeWare");

        }

        private void mlt_type_marjoei_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_type_marjoei, "Column02", "Column01");

        }

        private void mlt_ReturnPack_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_ReturnPack, "Column02", "TypeWare");

        }

        private void mlt_TypeReturnPack_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_TypeReturnPack, "Column02", "Column01");

        }

        private void mlt_TypeReturnDrafPack_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_TypeReturnDrafPack, "Column02", "Column01");

        }

        private void mlt_ReturnPack_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_TypeReturnPack_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_TypeReturnDrafPack_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_knittingReceipt_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_knittingReceipt, "Column02", "Column01");
        }

        private void mlt_KnittingWare_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_KnittingWare, "Column02", "TypeWare");
        }

        private void mlt_knittingDraft_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_knittingDraft, "Column02", "Column01");

        }
    }

}
