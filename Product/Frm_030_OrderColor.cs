using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Stimulsoft.Report.Components;
using PCLOR.Models;
using System.Threading.Tasks;
using Dapper;

namespace PCLOR.Product
{
    public partial class Frm_030_OrderColor : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        DataTable barcoddt = new DataTable();

        public Frm_030_OrderColor()
        {
            InitializeComponent();
        }

        private void Frm_030_OrderColor_Load(object sender, EventArgs e)
        {


            this.table_025_HederOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_025_HederOrderColor);
            this.table_030_DetailOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_030_DetailOrderColor);



            gridEX2.DropDowns["TypeCloth"].DataSource = mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth from Table_005_TypeCloth");
            //ClDoc.ReturnTable(ConBase, @"select Columnid,Column01,Column02 from Table_045_PersonInfo");
            gridEX2.DropDowns["Color"].DataSource = mlt_Color.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeColor from Table_010_TypeColor");
            gridEX2.DropDowns["Customer"].DataSource = mlt_codecustomer.DataSource = ClDoc.ReturnTable(ConBase, @"SELECT        dbo.Table_045_PersonInfo.ColumnId, dbo.Table_045_PersonInfo.Column01
FROM            dbo.Table_040_PersonGroups INNER JOIN
                         dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 INNER JOIN
                         dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId
WHERE        (dbo.Table_045_PersonScope.Column02 IN (8, 9))
Order by  dbo.Table_045_PersonInfo.Column01 ASC");
            gridEX2.DropDowns["Machine"].DataSource = mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,namemachine from Table_60_SpecsTechnical");
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report.mrt");
            foreach (StiPage page in r.Pages)
            {
                uiComboBox1.Items.Add(page.Name);
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            table_025_HederOrderColorBindingSource.AddNew();
            txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");

            txt_Barcode.Focus();
            txt_NumberOrder.Text = "0";
            txt_NumberOrder.Text = "0";
            txt_Remining.Text = "0";
            txt_Total.Text = "0";
            btn_New.Enabled = false;
            btn_New.Enabled = false;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (txt_Barcode.Text != "")
            //{
            //    barcoddt = new DataTable();
            //    barcoddt = ClDoc.ReturnTable(ConPCLOR, @"SELECT        Barcode, Machine, ClothType, NumberWeave, Weight
            //    FROM            dbo.Table_115_Product
            //    WHERE        (Barcode = " + txt_Barcode.Text + ")");
            //    if (barcoddt.Rows.Count > 0)
            //    {
            //        string Number = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT        SUM(NumberOrder) AS NumberOrder
            //                            FROM            dbo.Table_030_DetailOrderColor
            //                            GROUP BY Barcode
            //                            HAVING        (Barcode = " + txt_Barcode.Text + ")),0)");

            //        mlt_Machine.Value = barcoddt.Rows[0]["Machine"];
            //        mlt_TypeCloth.Value = barcoddt.Rows[0]["clothType"];

            //        txt_Total.Text = barcoddt.Rows[0]["NumberWeave"].ToString();
            //        txt_weight.Text = barcoddt.Rows[0]["Weight"].ToString();

            //        txt_Remining.Text = Convert.ToInt32(Convert.ToDouble(txt_Total.Text) - Convert.ToInt32(Number)).ToString();
            //        if (Convert.ToDouble(txt_Total.Text) == Convert.ToDouble(Number))
            //        {
            //            Class_BasicOperation.ShowMsg("", "برای این بارکد سقف سفارش پر شده است", Class_BasicOperation.MessageType.Warning);
            //            return;
            //        }
            //        mlt_codecustomer.Focus();
            //    }


            //}
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

            if (mlt_TypeCloth.Text == "" || uiComboBox1.Text == "" || txt_NumberOrder.Text == "0" || mlt_codecustomer.Text == "0")
            {
                MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                return;
            }
            if (mlt_Color.Text.All(char.IsDigit) || uiComboBox1.Text.All(char.IsDigit))
            {
                MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                return;
            }
            if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
            {
                txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
            }



            {
                ((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["OrderWeave"] = 1;

                table_025_HederOrderColorBindingSource.EndEdit();
                table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
                table_030_DetailOrderColorBindingSource.EndEdit();
                table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");


                btn_New.Enabled = true;
                //gridEX2.Enabled = true;




            }
        }





        private void Remain()
        {
            string Number = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT        SUM(NumberOrder) AS NumberOrder
                                        FROM            dbo.Table_030_DetailOrderColor
                                        GROUP BY Barcode
                                        HAVING        (Barcode = " + txt_Barcode.Text + ")),0)");
            txt_Remining.Text = Convert.ToInt32(Convert.ToDouble(txt_Total.Text) - Convert.ToDouble(Number)).ToString();

        }
        private void bt_Print_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report.mrt");
            r.Design();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID
FROM         dbo.Table_035_Production
WHERE     (dbo.Table_035_Production.ColorOrderId IN
                          (SELECT     dbo.Table_030_DetailOrderColor.ID
                            FROM          dbo.Table_025_HederOrderColor INNER JOIN
                                                   dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
                            WHERE      (dbo.Table_025_HederOrderColor.ID =" + txt_Id.Text + ")))");
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(".از این سفارش رنگ در قسمت کارت تولید استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    foreach (DataRowView item in this.table_030_DetailOrderColorBindingSource)
                    {
                        item.Delete();
                    }

                    table_025_HederOrderColorBindingSource.RemoveCurrent();

                    table_030_DetailOrderColorBindingSource.EndEdit();
                    table_025_HederOrderColorBindingSource.EndEdit();
                    table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                    table_025_HederOrderColorTableAdapter.Update(this.dataSet_05_PCLOR.Table_025_HederOrderColor);

                    MessageBox.Show("اطلاعات با موفقیت حذف شد");
                    table_025_HederOrderColorTableAdapter.FillByID(dataSet_05_PCLOR.Table_025_HederOrderColor, -1);
                    btn_New.Enabled = true;
                    mlt_TypeCloth.Text = "";
                    mlt_Color.Text = "";
                    uiComboBox1.Text = "";
                    txt_NumberOrder.Text = "";
                    txt_Title.Text = "";
                    txt_Total.Text = "";
                    txt_Remining.Text = "";
                    txt_Description.Text = "";
                    txt_weight.Text = "";
                    mlt_Machine.Text = "";

                }
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;

                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select max(Number) from Table_025_HederOrderColor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString() + " ");
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }



            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;
                txt_Barcode.Text = "";

                gridEX2.UpdateData();
                table_025_HederOrderColorBindingSource.EndEdit();
                this.table_030_DetailOrderColorBindingSource.EndEdit();


                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_025_HederOrderColor where Number>"
                    + ((DataRowView)this.table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");

                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString() + " ");
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }





            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;
                txt_Barcode.Text = "";

                if (this.table_025_HederOrderColorBindingSource.Count > 0)
                {
                    try
                    {
                        gridEX2.UpdateData();
                        table_025_HederOrderColorBindingSource.EndEdit();
                        this.table_030_DetailOrderColorBindingSource.EndEdit();
                        DataTable Table = ClDoc.ReturnTable(ConPCLOR,
                            "Select ISNULL((Select max(Number) from Table_025_HederOrderColor where Number<" + ((DataRowView)this.table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
                        if (Table.Rows[0]["Row"].ToString() != "0")
                        {
                            DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString() + " ");
                            if (RowId.Rows.Count > 0)
                            {
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                                this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                                dataSet_05_PCLOR.EnforceConstraints = true;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
                }




            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;
                txt_Barcode.Text = "";
                gridEX2.UpdateData();
                table_025_HederOrderColorBindingSource.EndEdit();
                this.table_030_DetailOrderColorBindingSource.EndEdit();


                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_025_HederOrderColor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString() + " ");
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }



            }
            catch
            {

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet_05_PCLOR.EnforceConstraints = false;
                this.table_025_HederOrderColorTableAdapter.FillByNumber(this.dataSet_05_PCLOR.Table_025_HederOrderColor, Convert.ToInt64(txt_Search.Text));

                if (table_025_HederOrderColorBindingSource.Count > 0)
                {
                    this.table_030_DetailOrderColorTableAdapter.FillByNumber(dataSet_05_PCLOR.Table_030_DetailOrderColor, Convert.ToInt64(txt_Search.Text));
                }

                else
                {
                    MessageBox.Show(".رسید با این شماره وجود ندارد");

                    txt_Search.SelectAll();
                    dataSet_05_PCLOR.EnforceConstraints = true;
                }
            }
            catch { }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Search_Click(sender, e);
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            //if (mlt_TypeCloth.Text == "" || uiComboBox1.Text == "" || txt_NumberOrder.Text == "0" || mlt_codecustomer.Text == "0")
            //{
            //    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
            //    return;
            //}
            //if (mlt_Color.Text.All(char.IsDigit) || uiComboBox1.Text.All(char.IsDigit) || mlt_TypeCloth.Text.All(char.IsDigit) || txt_NumberOrder.Text.All(char.IsDigit) == false)
            //{
            //    MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
            //    return;
            //}

            //if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
            //{
            //    txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
            //}

            //if (Convert.ToInt64(txt_NumberOrder.Text) > Convert.ToInt64(txt_Remining.Text))
            //{
            //    Class_BasicOperation.ShowMsg("", "تعداد سفارشات برای این بارکد از تعداد تولید شده آن بیشتر است", Class_BasicOperation.MessageType.Stop);
            //    return;
            //}
            //if (Convert.ToInt64(txt_NumberOrder.Text) == 0 && Convert.ToInt64(txt_Remining.Text) == 0)
            //{
            //    Class_BasicOperation.ShowMsg("", "تعداد سفارشات برای این بارکد از تعداد تولید شده آن بیشتر است", Class_BasicOperation.MessageType.Stop);
            //    return;
            //}
            var barcodes = GetBarcodes(txt_Barcode.Text);
            var status = CheckBarcodesDetail(GetBarcodesDetail(barcodes));

            table_025_HederOrderColorBindingSource.EndEdit();
            gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            gridEX2.MoveToNewRecord();
            gridEX2.SetValue("Fk", ((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["ID"].ToString());
            gridEX2.SetValue("Barcode", Convert.ToInt64(txt_Barcode.Text));
            gridEX2.SetValue("TypeColth", mlt_TypeCloth.Value);
            gridEX2.SetValue("NumberOrder", txt_NumberOrder.Text);
            gridEX2.SetValue("TypeColor", mlt_Color.Value);
            gridEX2.SetValue("Machine", mlt_Machine.Value);
            gridEX2.SetValue("weight", txt_weight.Text);
            gridEX2.SetValue("Title", txt_Title.Text);
            gridEX2.SetValue("Description", txt_Description.Text);
            gridEX2.SetValue("Printer", uiComboBox1.Text);
            gridEX2.UpdateData();
            gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
            ((DataRowView)table_030_DetailOrderColorBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
            ((DataRowView)table_030_DetailOrderColorBindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();
            table_025_HederOrderColorBindingSource.EndEdit();
            table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
            table_030_DetailOrderColorBindingSource.EndEdit();
            table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
            Remain();


        }

        private void txt_Barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            //{
            //    if (e.KeyChar == 13)
            //        Class_BasicOperation.isEnter(e.KeyChar);
            //    else if (!char.IsControl(e.KeyChar))
            //        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            //}
            //else
            //{
            //    if (e.KeyChar == 13)
            //        Class_BasicOperation.isEnter(e.KeyChar);
            //}
        }

        private void mlt_codecustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                { txt_NumberOrder.Focus(); }
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    txt_NumberOrder.Focus();
            }
        }

        private void txt_NumberOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                { mlt_TypeCloth.Focus(); }
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    mlt_Color.Focus();
            }
        }

        private void mlt_Color_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Color, "TypeColor", "Id");

        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID
FROM         dbo.Table_035_Production
WHERE     dbo.Table_035_Production.ColorOrderId =" + gridEX2.GetValue("ID").ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(".از این نوع پارچه در قسمت کارت تولید استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }

                    else
                    {
                        if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                        {
                            txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
                        }

                        //  else
                        {
                            gridEX2.GetRow().Delete();
                            table_025_HederOrderColorBindingSource.EndEdit();
                            table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
                            e.Cancel = true;
                            table_030_DetailOrderColorBindingSource.EndEdit();
                            table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                            MessageBox.Show("اطلاعات با موفقیت حذف شد");


                            btn_New.Enabled = true;

                            if (mlt_TypeCloth.Text == "")
                            {
                                Remain();
                            }

                            Remain();



                        }
                    }


                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch
            {
                MessageBox.Show("حذف امکان پذیر نیست");
                this.table_030_DetailOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_030_DetailOrderColor);
            }
        }

        private float FirstRemain1(int GoodCode, string Ware)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, Ware, GoodCode, txt_Dat.Text);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private string[] GetBarcodes(string barcodes) => barcodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        private IEnumerable<CheckBarcodeViewModel> GetBarcodesDetail(string[] barcodes)
        {
            var codes = string.Join(",", barcodes);
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
            select Machine,CottonType as Cotton,ClothType as Cloth 
            from Table_115_Product 
            where Barcode in ({codes}) ";
                return db.Query<CheckBarcodeViewModel>(query, null, commandType: CommandType.Text);
            }
        }

        public ErrorBarcodeProductEnum CheckBarcodesDetail(IEnumerable<CheckBarcodeViewModel> models)
        {
            var first = models.First();
            foreach (var item in models)
            {
                if (item.Cloth != first.Cloth)
                    return ErrorBarcodeProductEnum.Cloth;
                if (item.Cotton != first.Cotton)
                    return ErrorBarcodeProductEnum.Cotton;
                if (item.Machine != first.Machine)
                    return ErrorBarcodeProductEnum.Machine;
            }
            return ErrorBarcodeProductEnum.Success;
        }


    }
}
