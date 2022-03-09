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
using System.Threading;
using Dapper;
using Janus.Windows.GridEX;
using PCLOR.Classes;

namespace PCLOR.Product
{
    public partial class Frm_030_OrderColor : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        private int countHaveDesc = 0;
        private string CodeAnbarRang = "";
        private int ClothTypeId = 0;
        private int DeviceId = 0;
        private int ColorTypeId = 0;
        private int CottonTypeId = 0;
        private int CodeCustomer = 200;
        private bool IsBtnSaveFinalNotEnable = false;
        private bool IsCheckBarcodesValid = true;
        private int CodeStoreOrderColor = 0;
        private int FunctionTypeOrderColor = 0;
        private int CodeStoreProduction = 0;
        private int FunctionTypeProduction = 0;
        private int HeaderOrderColorId = 0;
        private int HeaderReciptIdForPrudct = 0;
        private int ProductionId { get; set; }
        private int ProductionNumber = 0;
        private int OrderColorNumber = 0;
        private List<InvalidBarcodeViewModel> invalidBarcodes = new List<InvalidBarcodeViewModel>();
        string OrginalBarcode = "";
        IEnumerable<FillDetailBarcodeViewModel> detailBarcodeViewModels = new List<FillDetailBarcodeViewModel>();


        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        DataTable barcoddt = new DataTable();


        public Frm_030_OrderColor()
        {
            InitializeComponent();
        }

        private void Frm_030_OrderColor_Load(object sender, EventArgs e)
        {
            btnSaveFinal.Enabled = false;
            gridEX3.BoundMode = BoundMode.Unbound;
            gridEX4.BoundMode = BoundMode.Unbound;

            var dec = GetCodeOfStorePrduct();
            int codeStore;
            int functionType;
            dec.TryGetValue("WareCode", out codeStore);
            dec.TryGetValue("FunctionType", out functionType);


            GetCodeAnbarRang();
            txt_Dat.Text = DateTime.Now.ToShamsi();
            this.table_025_HederOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_025_HederOrderColor);
            this.table_030_DetailOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_030_DetailOrderColor);
            //gridEX3.DropDowns["TypeCloth"].DataSource = mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth from Table_005_TypeCloth");
            //ClDoc.ReturnTable(ConBase, @"select Columnid,Column01,Column02 from Table_045_PersonInfo");
            var colorModels = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeColor from Table_010_TypeColor").ToList<TypeColorViewModel>();
            multiColumnColor.DataSource = colorModels;
            //            gridEX3.DropDowns["Customer"].DataSource = mlt_codecustomer.DataSource = ClDoc.ReturnTable(ConBase, @"SELECT        dbo.Table_045_PersonInfo.ColumnId, dbo.Table_045_PersonInfo.Column01
            //FROM            dbo.Table_040_PersonGroups INNER JOIN
            //                         dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 INNER JOIN
            //                         dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId
            //WHERE        (dbo.Table_045_PersonScope.Column02 IN (8, 9))
            //Order by  dbo.Table_045_PersonInfo.Column01 ASC");
            //gridEX3.DropDowns["Machine"].DataSource = mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,namemachine from Table_60_SpecsTechnical");
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report.mrt");
            foreach (StiPage page in r.Pages)
            {
                uiComboBox2.Items.Add(page.Name);
            }
            uiComboBox2.Text = uiComboBox2.Items[0].Text;
            uiComboBox2.SelectedValue = uiComboBox2.Items[0].Value;
            //-------------
            multiColumnColor.Value = colorModels.FirstOrDefault().ID;
            multiColumnColor.Text = colorModels.FirstOrDefault().TypeColor;
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            table_025_HederOrderColorBindingSource.AddNew();
            txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");

            txt_Barcode.Focus();
            txt_NumberOrder.Text = "0";
            txt_NumberOrder.Text = "0";
            //txt_Remining.Text = "0";
            //txt_Total.Text = "0";
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

            //if (mlt_TypeCloth.Text == "" || uiComboBox2.Text == "" || txt_NumberOrder.Text == "0" || mlt_codecustomer.Text == "0")
            //{
            //    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
            //    return;
            //}
            if (multiColumnColor.Text.All(char.IsDigit) || uiComboBox2.Text.All(char.IsDigit))
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
                //gridEX3.Enabled = true;




            }
        }

        private void Remain()
        {
            //string Number = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT        SUM(NumberOrder) AS NumberOrder
            //                            FROM            dbo.Table_030_DetailOrderColor
            //                            GROUP BY Barcode
            //                            HAVING        (Barcode = " + txt_Barcode.Text + ")),0)");
            //txt_Remining.Text = Convert.ToInt32(Convert.ToDouble(txt_Total.Text) - Convert.ToDouble(Number)).ToString();

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
                    //mlt_TypeCloth.Text = "";
                    multiColumnColor.Text = "";
                    uiComboBox2.Text = "";
                    txt_NumberOrder.Text = "";
                    txt_Title.Text = "";
                    //txt_Total.Text = "";
                    //txt_Remining.Text = "";
                    txt_Description.Text = "";
                    txt_weight.Text = "";
                    //mlt_Machine.Text = "";

                }
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                //mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                multiColumnColor.Value = "";
                txt_Title.Text = "";
                uiComboBox2.Text = "";
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
                //mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                multiColumnColor.Value = "";
                txt_Title.Text = "";
                uiComboBox2.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;
                txt_Barcode.Text = "";

                gridEX3.UpdateData();
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
                //mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                multiColumnColor.Value = "";
                txt_Title.Text = "";
                uiComboBox2.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;
                txt_Barcode.Text = "";

                if (this.table_025_HederOrderColorBindingSource.Count > 0)
                {
                    try
                    {
                        gridEX3.UpdateData();
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
                //mlt_TypeCloth.Value = "";
                //txt_Total.Text = "";
                //txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                multiColumnColor.Value = "";
                txt_Title.Text = "";
                uiComboBox2.Text = "";
                txt_Description.Text = "";
                //uiPanel0.Enabled = false;
                txt_Barcode.Text = "";
                gridEX3.UpdateData();
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
            try
            {
              
                if (multiColumnColor.Text.All(char.IsDigit)  )
                {
                    MessageBox.Show("لطفا نوع رنگ را انتخاب نمائید");
                    return;
                }
                if (uiComboBox2.Text.All(char.IsDigit))
                {
                    MessageBox.Show("لطفاطرح چاپ  را انتخاب نمائید");
                    return;
                }

         
                if (string.IsNullOrEmpty(txt_Barcode.Text.Trim()))
                {
                    MessageBox.Show("لطفا بارکد هارا وارد نمایئد سپس اقدام فرمایئد");
                    return;
                }
                gridEX4.ClearItems();
                gridEX3.ClearItems();
                var barcodes = GetBarcodes(txt_Barcode.Text.Trim());
                CheckBarcodesManagement(GetOrginalCodes(barcodes));
                CheckBarcodesDetail(detailBarcodeViewModels);
                if (!IsCheckBarcodesValid)
                {
                    string message = "";
                    foreach (var item in invalidBarcodes)
                    {
                        message += item.Message + "   /   ";
                    }
                    message = message.Trim().TrimEnd('/');
                    MessageBox.Show(message);
                    invalidBarcodes.Clear();
                    IsCheckBarcodesValid = true;
                    return;
                }

              


                FillDetailBarcode(barcodes);
                FillDetailColor(Convert.ToInt32((multiColumnColor.Value).ToString()), Convert.ToDecimal(txt_weight.Text));
                ShowDetailBarcode();
          
                btnSaveFinal.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
            //if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            //{
            //    if (e.KeyChar == 13)
            //    { mlt_TypeCloth.Focus(); }
            //    else if (!char.IsControl(e.KeyChar))
            //        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            //}
            //else
            //{
            //    if (e.KeyChar == 13)
            //        multiColumnColor.Focus();
            //}
        }

        private void multiColumnColor_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(multiColumnColor, "TypeColor", "Id");

        }

        private void gridEX3_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID
FROM         dbo.Table_035_Production
WHERE     dbo.Table_035_Production.ColorOrderId =" + gridEX3.GetValue("ID").ToString() + "");
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
                            gridEX3.GetRow().Delete();
                            table_025_HederOrderColorBindingSource.EndEdit();
                            table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
                            e.Cancel = true;
                            table_030_DetailOrderColorBindingSource.EndEdit();
                            table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                            MessageBox.Show("اطلاعات با موفقیت حذف شد");


                            btn_New.Enabled = true;

                            //if (mlt_TypeCloth.Text == "")
                            //{
                            //    Remain();
                            //}

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

        private string[] GetBarcodes(string barcodes) => barcodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Distinct().ToArray();

        private void ShowMessageBarcodeNotValid(List<int> models)
        {
            if (models.Count != 0)
            {
                string message = string.Join("بارکد  , ", models) + " یافت نشد !";
                MessageBox.Show(message, "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        private List<int> BarcodeIsValid(string[] barcodes)
        {
            var model = new List<int>();
            string newCode = "";
            for (int i = 0; i < barcodes.Count(); i++)
            {
                newCode = "N'" + barcodes[i] + "'";
                barcodes[i] = newCode;
            }
            string codes = "";
            if (barcodes.Length > 1)
            {
                codes = string.Join(",", barcodes);
            }
            else if (barcodes.Length == 1)
                codes = barcodes[0];
            codes = codes.Trim(',');

            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                 select Barcode
                 from Table_115_Product
                 where Barcode in ({codes})";
                var res = db.Query<int>(query, null, commandType: CommandType.Text);
                if (barcodes.Length == 1)
                {
                    var first = Convert.ToInt32(barcodes[0]);
                    var status = res.Any(d => d == first);
                    if (!status)
                        model.Add(first);
                    return model;
                }
                if (barcodes.Length == 0)
                    return null;
                foreach (var item in barcodes)
                {
                    var mod = Convert.ToInt32(item);
                    var status = res.Any(d => d == mod);
                    if (!status)
                        model.Add(mod);
                }
                return model;
            }
        }

        private IEnumerable<CheckBarcodeViewModel> GetBarcodesDetail(string[] barcodes)
        {
            string codes = "";
            if (barcodes.Length > 1)
            {
                codes = string.Join(",", barcodes);
            }
            else if (barcodes.Length == 1)
                codes = barcodes[0];
            codes = codes.Trim(',');
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                            select BarCode, Machine,CottonType as Cotton,ClothType as Cloth 
                            from Table_115_Product 
                            where Barcode in ({codes}) ";
                return db.Query<CheckBarcodeViewModel>(query, null, commandType: CommandType.Text);

            }
        }

        public void CheckBarcodesDetail(IEnumerable<FillDetailBarcodeViewModel> models)
        {
            //if (models == null || models.Count() == 0)
            //    return ErrorBarcodeProductEnum.Null;
            //if (models.Count() == 1)
            //    return ErrorBarcodeProductEnum.Success;
            if (models == null || models.Count() == 0)
                return;
            var first = models.First();
            foreach (var item in models)
            {
                if (item.Cloth != first.Cloth)
                {
                    invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item.Barcode, Message = $@"بارکد {item.Barcode.Trim('N')} با پارچه بارکد اول مطابقت ندارد" });
                    IsCheckBarcodesValid = false;
                    //return ErrorBarcodeProductEnum.Cloth;
                }
                if (item.Cotton != first.Cotton)
                {
                    IsCheckBarcodesValid = false;
                    invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item.Barcode, Message = $@"بارکد {item.Barcode.Trim('N')} با نخ بارکد اول مطابقت ندارد" });
                    //return ErrorBarcodeProductEnum.Cotton;
                }
                if (item.Machine != first.Machine)
                {
                    invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item.Barcode, Message = $@"بارکد {item.Barcode.Trim('N')} با دستگاه  بارکد اول مطابقت ندارد" });
                    //return ErrorBarcodeProductEnum.Machine;
                    IsCheckBarcodesValid = false;
                }
            }
            DeviceId = Convert.ToInt32(first.Machine);
            ClothTypeId = Convert.ToInt32(first.Cloth);
            CottonTypeId = Convert.ToInt32(first.Cotton);
            //return ErrorBarcodeProductEnum.Success;
        }



        public void CheckBarcodesManagement(string barcodes)
        {
            var status = false;
            var arrayBarcodes = barcodes.Split(',');
            var models = detailBarcodeViewModels = GetDetailBarcode(barcodes);
            if (models == null || models.Count() == 0)
            {
                invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = arrayBarcodes[0], Message = $@"بار کد {arrayBarcodes[0].TrimStart('N') } وجود ندارد" });
                IsCheckBarcodesValid = false;
                return;
            }
            //if (arrayBarcodes.Length == models.Count())
            //    return;
            foreach (var item in arrayBarcodes)
            {
                //item.Remove(item.Length-1).Substring(2);
                status = !models.Any(c => c.Barcode.Trim() == item.Remove(item.Length - 1).Substring(2));
                if (status)
                {
                    invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item, Message = $@"بار کد {item.Trim('N')} وجود ندارد" });
                    IsCheckBarcodesValid = false;
                }
            }
            if (IsCheckBarcodesValid)
            {
                foreach (var item in models)
                {
                    status = models.Any(c => c.Barcode.Trim() == item.Barcode.Trim() && item.IsRegToOrderColor == true);
                    if (status)
                    {
                        invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item.Barcode, Message = $@"بارکد {item.Barcode.Trim('N')} قبلا ثبت شده است" });
                        IsCheckBarcodesValid = false;
                    }
                }
            }


        }



        public string GetOrginalCodes(string[] barcodes)
        {
            string newCode = "";
            for (int i = 0; i < barcodes.Count(); i++)
            {
                newCode = "N'" + barcodes[i] + "'";
                barcodes[i] = newCode;
            }
            string codes = "";
            if (barcodes.Length > 1)
            {
                codes = string.Join(",", barcodes);
            }
            else if (barcodes.Length == 1)
                codes = barcodes[0];
            codes = codes.Trim(',');
            return codes;
        }

        public void FillDetailBarcode(string[] barcodes)
        {

            var codes = OrginalBarcode = GetOrginalCodes(barcodes);
            if (detailBarcodeViewModels.Count() == 0)
                detailBarcodeViewModels = GetDetailBarcode(codes);


            //var descs = detailBarcodeViewModels.Where(d => !string.IsNullOrEmpty(d.Description)).Select(c => c.Description);
            if (detailBarcodeViewModels != null && detailBarcodeViewModels.Count() != 0)
            {
                var first = detailBarcodeViewModels.First();
                txt_weight.Text = detailBarcodeViewModels.Sum(d => d.Weight).ToString();
                //txt_Description.Text = string.Join(System.Environment.NewLine, descs);
                lblNameDevice.Text = first.NameDevice;
                lblClothType.Text = first.ClothName;
                txt_NumberOrder.Text = barcodes.Count().ToString();
                //countHaveDesc = descs.Count();
            }
        }

        public IEnumerable<FillDetailBarcodeViewModel> GetDetailBarcode(string codes)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var t = codes.Split(',').Length;
                var query = $@"
SELECT TOP ({t}) [Weight]
      ,[Description]
      ,[NameDevice]
      ,[ClothName]
      ,[CodeCommodity]
      ,[Barcode]
      ,[Weaver]
      ,[Date]
      ,[Shift]
      ,[CottonName]
      ,[JoinShift]
      ,[PurityOperator1]
      ,[Purityoperator2]
      ,[OperatorTag1]
      ,[OperatorTag2]
      ,[StoreName]
      ,[CodeStore]
      ,[DeviceId]
      ,[IsRegToOrderColor],
[Machine],
[Cotton],
[Cloth]
  FROM [PCLOR_1_1400].[dbo].[DeatailBarcodes]
       where Barcode in   ({codes}) ";
                var res = db.Query<FillDetailBarcodeViewModel>(query, null, commandType: CommandType.Text);
                db.ConnectionString = ConWare.ConnectionString;
                if (res != null)
                {
                    db.ConnectionString = ConBase.ConnectionString;
                    foreach (var item in res)
                    {
                        var names = db.Query<string>("GetNamePersonByTagCode", new { @TagCode1 = item.OperatorTag1, @TagCode2 = item.OperatorTag2 }, commandType: CommandType.StoredProcedure);
                        if (names != null && names.Count() != 0)
                        {
                            item.NameOperator1 = names.Last();
                            if (names.Count() == 2)
                                item.NameOperator2 = names.First();
                        }
                    }
                }
                return res;
            }
        }

        public void ShowDetailBarcode()
        {
            foreach (var item in detailBarcodeViewModels)
            {
                var row = gridEX3.AddItem();
                row.BeginEdit();
                row.Cells[0].Value = item.Weight;
                row.Cells[1].Value = item.Description;
                row.Cells[2].Value = item.NameDevice;
                row.Cells[3].Value = item.Barcode;
                row.Cells[4].Value = item.StoreName;
                row.Cells[5].Value = item.ClothName;
                row.Cells[6].Value = item.CottonName;
                row.Cells[7].Value = item.JoinShift;
                row.Cells[8].Value = item.NameOperator1;
                row.Cells[9].Value = item.OperatorTag1;
                row.Cells[10].Value = item.PurityOperator1;
                row.Cells[11].Value = item.NameOperator2;
                row.Cells[12].Value = item.OperatorTag2;
                row.Cells[13].Value = item.PurityOperator2;
                row.Cells[14].Value = item.Shift;
                row.Cells[15].Value = item.Date;
                row.Cells[16].Value = item.CodeCommodity;
                row.Cells[17].Value = item.CodeStore;
                row.Cells[18].Value = item.DeviceId;
                row.EndEdit();
                if (!string.IsNullOrEmpty(item.Description.Trim()))
                {
                    countHaveDesc++;
                }
            }
        }

        public bool StatusBarcodesEntered(ErrorBarcodeProductEnum status)
        {
            if (status == ErrorBarcodeProductEnum.Success)
                return true;
            return false;
        }

        public void CleareAllDetailBarcodes()
        {
            lblClothType.Text = string.Empty;
            lblNameDevice.Text = string.Empty;
            txt_Description.Text = string.Empty;
            txt_weight.Text = string.Empty;
        }

        private void uiPanel0_SelectedPanelChanged(object sender, Janus.Windows.UI.Dock.PanelActionEventArgs e)
        {

        }

        private bool CheckNegativeInventory(int code)
        {
            using (IDbConnection db = new SqlConnection(ConWare.ConnectionString))
            {
                return db.QueryFirstOrDefault<bool>("CheckNegativeInventory", new { @Code = code }, commandType: CommandType.StoredProcedure);
            }
        }

        private void FillDetailColor(int colorId, decimal Weight)
        {

            ///اگر موجودی منفی داشتیم و کالا هم دچار عدم موجودی بود رنگ صورتی 
            ///اگر موجودی منفی نداشتیم و کالا هم دچار عدم موجودی بود رنگ قرمز به همراه غیر فعال کردن دکمه 
            ///اگر دچار عدم موجودی نبودیم رنگ سبز 

            Janus.Windows.GridEX.GridEXFormatStyle rowcol = new GridEXFormatStyle();
            rowcol.BackColor = Color.LightGreen;
            var model = GetDetailColor(colorId, Weight);
            foreach (var item in model)
            {
                var row = gridEX4.AddItem();
                row.BeginEdit();
                var inventoryNegative = CheckNegativeInventory(Convert.ToInt32(item.CodeCommodity));
                row.Cells[0].Value = item.CodeCommodity;
                row.Cells[1].Value = item.NameColor;
                row.Cells[2].Value = item.AmountRequierd;
                row.Cells[5].Value = inventoryNegative.ToString();
                var status = FirstRemain1(Convert.ToInt32(item.CodeCommodity), CodeAnbarRang);
                row.Cells[4].Value = status.ToString();
                if (Convert.ToDecimal(status) < item.AmountRequierd)
                {
                    rowcol.BackColor = Color.IndianRed;
                    //btnSaveFinal.Enabled = false;
                    IsBtnSaveFinalNotEnable = true;
                    if (inventoryNegative)
                    {
                        rowcol.BackColor = Color.LightPink;
                        IsBtnSaveFinalNotEnable = true;
                        //btnSaveFinal.Enabled = true;
                    }
                    row.RowStyle = rowcol;
                    row.Cells[3].Value = item.AmountRequierd - Convert.ToDecimal(status);
                }
                else
                {
                    row.RowStyle = rowcol;
                    row.Cells[3].Value = 0;
                }
                row.EndEdit();
            }
            ColorTypeId = colorId;
            txt_Description.Focus();
        }

        public IEnumerable<ShowDetailColorViewModel> GetDetailColor(int colorId, decimal Weight)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                return db.Query<ShowDetailColorViewModel>("GetDetailColor", new { @Weight = Weight, @ColorId = colorId }, commandType: CommandType.StoredProcedure);
            }
        }

        public void GetCodeAnbarRang()
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = @"
                    select Value
                    from Table_80_Setting 
                    where ID=18
                                ";
                CodeAnbarRang = db.QueryFirstOrDefault<string>(query, null, commandType: CommandType.Text);
            }

        }

        public void FinallSave()
        {
            if (ClothTypeId == 0 || DeviceId == 0 || ColorTypeId == 0)
            {
                MessageBox.Show("مشخصات به طور دقیق برای ثبت وارد نشده است", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public int SaveToHeaderColorOrder(int codeCustomer)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var getNumberQuery = @"
                    select Max(Number) +1
                    from  [PCLOR_1_1400].[dbo].[Table_025_HederOrderColor]";
                var number = db.ExecuteScalar<int>(getNumberQuery, null, commandType: CommandType.Text);
                OrderColorNumber = number;
                var addToHeaderquery = $@"
                    insert into  Table_025_HederOrderColor (CodeCustomer,Date,Number,OrderWeave,TotalWeight,TypeColor,TypeCotton,TypeCloth,Machine
                    ,CountHaveDesc,Title,Printer,Description)
                    values ({codeCustomer},N'{DateTime.Now.ToShamsi()}',{number},null,{Convert.ToDecimal(txt_weight.Text)},{ColorTypeId},{CottonTypeId},{ClothTypeId}
                    ,{DeviceId},{countHaveDesc},N'{txt_Title.Text.Trim()}',N'{uiComboBox2.Text.Trim()}',N'{txt_Description.Text.Trim()}')
                    select SCOPE_IDENTITY(); ";
                var headerId = db.QueryFirstOrDefault<int>(addToHeaderquery, null, commandType: CommandType.Text);
                return headerId;
            }
        }

        public int SaveToDetailOrderColor(int headerId, int numberOrder, decimal weight, string barcode)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                        insert into Table_030_DetailOrderColor 
                        (Fk,NumberOrder,UserSabt,TimeSabt,weight,Barcode)
                        values
                        ({headerId},{numberOrder},N'{Class_BasicOperation._UserName}',GETDATE(),{weight},N'{barcode}')
                        select SCOPE_IDENTITY();";
                return db.Execute(query, null, commandType: CommandType.Text);
            }
        }

        private void btnSaveFinal_Click(object sender, EventArgs e)
        {
            ////Check Exist Color
            bool CheckColor()
            {
                try
                {
                    string colorsName = "";
                    foreach (var item in gridEX4.GetRows())
                    {
                        var inventory = Convert.ToDecimal(item.Cells[4].Value.ToString());
                        var colorName = item.Cells[1].Value.ToString();
                        var amountRequierd = Convert.ToDecimal(item.Cells[2].Value.ToString());

                        var inventoryNegative = Convert.ToBoolean(item.Cells[5].Value.ToString());
                        if (amountRequierd < 0)
                            amountRequierd = 0;
                        if (inventory < 0)
                            inventory = 0;

                        if (inventory < amountRequierd && !inventoryNegative)
                        {
                            colorsName += colorName + " , ";
                        }
                    }
                    if (!string.IsNullOrEmpty(colorsName))
                    {
                        colorsName = colorsName.TrimEnd(',');
                        MessageBox.Show($" {colorsName} دارای کمبود موجودی در انبار هستند");
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("مقدار رنگ را با فرمت صحیح وارد کنید ");
                    MessageBox.Show(ex.Message);
                    return true;
                }

            }
            if (CheckColor())
                return;
            ////End Check Exist Color

            var headerId = SaveToHeaderColorOrder(CodeCustomer);
            var detailheaderId = 0;
            try
            {
                if (headerId != 0)
                {
                    HeaderOrderColorId = headerId;
                    foreach (var item in gridEX3.GetRows())
                    {

                        detailheaderId = SaveToDetailOrderColor(headerId, Convert.ToInt32(txt_NumberOrder.Text.Trim()), Convert.ToDecimal(item.Cells["Weight"].Value.ToString()), item.Cells["Barcode"].Value.ToString());
                    }

                    if (detailheaderId != 0)
                    {
                        var dec = GetCodeOfStorePrduct();
                        int codeStore;
                        int functionType;
                        dec.TryGetValue("WareCode", out codeStore);
                        dec.TryGetValue("FunctionType", out functionType);
                        var headerReciptId = MyBasicFunction.BasicFunction.Recipt(ConWare, DateTime.Now.ToShamsi(), DeviceId, ClDoc, codeStore, functionType, lblCodeCustomer.Text, "0");
                        HeaderReciptIdForPrudct = headerReciptId;
                        foreach (var item in gridEX3.GetDataRows())
                        {
                            var CodeCommodity = item.Cells["CodeCommodity"].Value.ToString();
                            var Weight = item.Cells["Weight"].Value.ToString();
                            var BarCode = item.Cells["BarCode"].Value.ToString();
                            MyBasicFunction.BasicFunction.ReciptChild(ConWare: ConWare, headerReciptId, value: 1, Convert.ToInt32(CodeCommodity), weight: Convert.ToDecimal(Weight), barcode: BarCode, DeviceId);
                        }
                        Draft();
                    }
                    else
                    {
                        MessageBox.Show("مشکلی در ثبت به وجود آمده است");
                    }
                }
                else
                {
                    MessageBox.Show("مشکلی در ثبت به وجود آمده است");
                }
                if (ProductionNumber > 0 && OrderColorNumber > 0)
                    MessageBox.Show($@"سفارش رنگ با موفقیت ثبت شد " + "\n\n" + $"  کارت  تولید  با کد {ProductionNumber}  صادر شد  " + "\n\n" + $"سفارش رنگ با  کد {OrderColorNumber} صادر شد ", "ثبت سفارش رنگ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("مشکلی در ثبت به وجود آمده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }

        }

        private int InsertToProduction(int numberDraft, int numberRecipt, string date
            , int deviceId, int orderColorId, int numberProduct, decimal weight, string description, string createUser
            , int codeCustomer, int typecloth, int typeColor, string printer)
        {
            try
            {
                var number = GetMaxNumberOfProduction();
                ProductionNumber = number;
                var query = $@"
INSERT INTO [dbo].[Table_035_Production]
           
           (
            [Number],
            [NumberDraft]
           ,[NumberRecipt]
           ,[Date]
           ,[Machine]
           ,[ColorOrderIdHeader]
           ,[NumberProduct]
           ,[weight]
           ,[Description]
           ,[UserSabt]
           ,[TimeSabt]
           ,[CodeCustomer]
           ,[TypeCloth]
           ,[TypeColor]
           ,[Printer]
           ,[EditProduct])
     VALUES
    ({number},{numberDraft},{numberRecipt},N'{date}',{deviceId},{orderColorId},{numberProduct},{weight},N'{description}',N'{createUser}',GETDATE(),{codeCustomer},{typecloth},{typeColor},N'{printer}',0)
    SELECT SCOPE_IDENTITY()
";
                using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
                {
                    return db.QueryFirstOrDefault<int>(query);
                }

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        private int GetMaxNumberOfProduction()
        {
            try
            {
                var query = $@"SELECT  ISNULL (MAX(Number),0) +1 FROM Table_035_Production";
                using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
                {
                    return db.QueryFirstOrDefault<int>(query);
                }

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        private bool InsertToColorProduction(int ProductionId, int codeColor
            , int numberDraft, int typeColor, decimal consumption, int codeCommondity)
        {
            try
            {
                var query = $@"
INSERT INTO [dbo].[Table_40_ColorPrduction]
           ([FK]
           ,[CodeColor]
           ,[NumberDraft]
           ,[TypeColor]
           ,[Consumption]
           ,[CodeCommondity])
     VALUES
        ({ProductionId},{codeColor},{numberDraft},{typeColor},{consumption},
        {codeCommondity})";
                using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
                {
                    return db.Execute(query) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public void IsRegToOrderColor(string barcodes)
        {
            //barcodes = barcodes.Remove(barcodes.Length - 1).Substring(2);
            var query = $@"  update Table_115_Product Set IsRegToOrderColor=1
                                    where Barcode =N'{barcodes}' ";
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                db.Execute(query, null);
            }
        }

        private bool Draft()
        {
            try
            {
                var dec = GetCodeOfStoreWeave();
                int codeStoreWeave = 0;
                int functionTypeWeave = 0;
                dec.TryGetValue("WareCode", out codeStoreWeave);
                dec.TryGetValue("FunctionType", out functionTypeWeave);


                ///گرفتن کد انبار و عملکرد حواله مخصوص انبار سایر (Title in Setting ---> سایر)
                var material = GetCodeOfStoreMaterial();
                int codeStoreMaterial;
                int functionTypeMaterial;
                material.TryGetValue("WareCode", out codeStoreMaterial);
                material.TryGetValue("FunctionType", out functionTypeMaterial);

                var headerWeaveId = MyBasicFunction.BasicFunction.ExportDraftHeader(ConWare, ClDoc, txt_Dat.Text, codeStoreWeave,
                     ///حواله به انبار بافندگی 
                     functionTypeWeave, Convert.ToInt32(lblCodeCustomer.Text), txt_Description.Text, "");

                foreach (var item in gridEX3.GetRows())
                {
                    ///جزیئات حواله به انبار بافندگی
                    MyBasicFunction.BasicFunction.ExportDraftChild(headerWeaveId, Convert.ToInt32(item.Cells["CodeStore"].Value.ToString()), Convert.ToInt32(item.Cells["CodeCommodity"].Value.ToString())
                     , 1, item.Cells["Barcode"].Value.ToString(), txt_Dat.Text, ConWare);
                    IsRegToOrderColor(item.Cells["Barcode"].Value.ToString());

                }

                //  ---------------------------------------------------------------------------------------------------

                ///حواله به انبار مواد رنگ مصرفی 
                var headerMaterialId = MyBasicFunction.BasicFunction.ExportDraftHeader(ConWare, ClDoc, txt_Dat.Text, codeStoreMaterial,
                    functionTypeMaterial, Convert.ToInt32(lblCodeCustomer.Text), txt_Description.Text, "");

                var productionId = InsertToProduction(headerWeaveId, HeaderReciptIdForPrudct, DateTime.Now.ToShamsi(), DeviceId, HeaderOrderColorId, 1,
                Convert.ToDecimal(txt_weight.Text), txt_Description.Text, Class_BasicOperation._UserName, CodeCustomer, ClothTypeId, Convert.ToInt32(multiColumnColor.Value.ToString()), uiComboBox2.Text.Trim());
                ProductionId = productionId;
                foreach (var item in gridEX4.GetRows())
                {
                    ///جزیئات حواله به انبار رنگرزی
                    var draftChildId = MyBasicFunction.BasicFunction.ExportDraftChild(headerMaterialId, codeStoreMaterial, Convert.ToInt32(item.Cells["CodeCommodity"].Value.ToString())
                          , Convert.ToDecimal(item.Cells["AmountRequierd"].Value.ToString()), "1", txt_Dat.Text, ConWare);
                    InsertToColorProduction(productionId, Convert.ToInt32(item.Cells["CodeCommodity"].Value.ToString()), draftChildId, Convert.ToInt32(multiColumnColor.Value.ToString())
                        , Convert.ToDecimal(item.Cells["AmountRequierd"].Value.ToString()), Convert.ToInt32(item.Cells["CodeCommodity"].Value.ToString()));
                }

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }


            ///گرفتن کد انبار و عملکرد حواله مخصوص انبار بافندگی (Title in Setting ---> بافندگی)



        }

        public Dictionary<string, int> GetCodeOfStoreWeave()
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                        select Value
                        from Table_80_Setting 
                        where ID in(31,32)
";
                var res = db.Query<int>(query, null, commandType: CommandType.Text).ToList();
                var dec = new Dictionary<string, int>();
                dec.Add("FunctionType", res[0]);
                dec.Add("WareCode", res[1]);
                return dec;
            }
        }

        public Dictionary<string, int> GetCodeOfStorePrduct()
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                        select Value
                        from Table_80_Setting 
                        where ID in(15,3)
";
                var res = db.Query<int>(query, null, commandType: CommandType.Text).ToList();
                var dec = new Dictionary<string, int>();
                dec.Add("FunctionType", res[0]);
                dec.Add("WareCode", res[1]);
                return dec;
            }
        }

        public Dictionary<string, int> GetCodeOfStoreMaterial()
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                        select Value
                        from Table_80_Setting 
                        where ID in(18,8)
";
                var res = db.Query<int>(query, null, commandType: CommandType.Text).ToList();
                var dec = new Dictionary<string, int>();
                dec.Add("FunctionType", res[0]);
                dec.Add("WareCode", res[1]);
                return dec;
            }
        }

        private void gridEX4_FormattingRow(object sender, RowLoadEventArgs e)
        {

        }
    }
}

