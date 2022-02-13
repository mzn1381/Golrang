using Dapper;
using Janus.Windows.GridEX;
using PCLOR.Classes;
using PCLOR.Models;
using PCLOR.MyBasicFunction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCLOR._01_OperationInfo
{
    public partial class Frm_65_Transfer_Barcode : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        private List<InvalidBarcodeViewModel> invalidBarcodes = new List<InvalidBarcodeViewModel>();
        private bool IsCheckBarcodesValid = true;

        public Frm_65_Transfer_Barcode()
        {
            InitializeComponent();
        }

        private void Frm_65_Transfer_Barcode_Load(object sender, EventArgs e)
        {
            //gridEX8.DataSource = GetBarcodes();
            menuStoresDestination.DataSource = GetStores(null);
            menuStoresStart.DataSource = GetStores(null);
            if (checkRegAuto.Checked)
                menuStoresStart.Text = string.Empty;
        }





        private IEnumerable<ShowStoresViewModel> GetStores(string name)
        {
            try
            {
                var query = $@"
    SELECT 
       [StoreCode]
      ,[StoreName]
    FROM [PWHRS_1_1400].[dbo].[ShowStoreBarcode]";
                if (!string.IsNullOrEmpty(name))
                    query += $@"        where  StoreName like    N'%{name}%'    ";
                using (IDbConnection db = new SqlConnection(Properties.Settings.Default.PWHRS))
                {
                    var res = db.Query<ShowStoresViewModel>(query);
                    return res;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }



        private IEnumerable<FillDetailBarcodeViewModel> GetBarcodes()
        {
            try
            {
                var query = $@"
SELECT 
       [Weight]
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
       FROM [PCLOR_1_1400].[dbo].[DeatailBarcodes]
       ";
                using (IDbConnection db = new SqlConnection(Properties.Settings.Default.PCLOR))
                {
                    var res = db.Query<FillDetailBarcodeViewModel>(query, null);
                    return res;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void gridEX8_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void gridEX4_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void gridEX8_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Recipt();
            //Draft();
            //ChangeCodeStoreOfBarcode();
        }

        private void checkRegAuto_CheckedChanged(object sender, EventArgs e)
        {
            var status = checkRegAuto.Checked;
            if (status)
                menuStoresStart.ReadOnly = true;
            else
                menuStoresStart.ReadOnly = false;
        }

        public void Recipt(GridEXRow row)
        {
            try
            {
                var dateNow = DateTime.Now.ToShamsi();
                int functionType = Convert.ToInt16(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=30"));
                var codeCommodity = row.Cells["CodeCommodity"].Value.ToString();
                var barcode = row.Cells["Barcode"].Value.ToString();
                var weight = row.Cells["Weight"].Value.ToString();
                var deviceId = row.Cells["DeviceId"].Value.ToString();
                var headerReciptId = BasicFunction.Recipt(ConWare, dateNow, 0, ClDoc, Convert.ToInt32(menuStoresDestination.Value.ToString()), functionType, null, "", codeCommodit: codeCommodity);
                MyBasicFunction.BasicFunction.ReciptChild(ConWare: ConWare, headerReciptId, value: 1, Convert.ToInt32(codeCommodity), weight: Convert.ToDecimal(weight), barcode: barcode, Convert.ToInt32(deviceId));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void ChangeCodeStoreOfBarcode(GridEXRow row)
        {
            try
            {
                var barcode = row.Cells["Barcode"].Value.ToString();
                var codeStoreDestination = Convert.ToInt32(menuStoresDestination.Value);
                var query = $@"
                                update Table_115_Product Set CodeStore={codeStoreDestination}
                                where Barcode = N'{barcode}'  ";
                using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
                {
                    db.Execute(query);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Draft(GridEXRow item)
        {
            try
            {
                var dateNow = DateTime.Now.ToShamsi();
                var model = GetCodeOfStoreWeave();
                var functionType = model["FunctionType"];
                //var item = row;
                var codeWare = 0;
                if (!checkRegAuto.Checked)
                    codeWare = Convert.ToInt32(menuStoresStart.Value);
                else
                    codeWare = Convert.ToInt32(item.Cells["CodeStore"].Value.ToString());


                var headerWeaveId = BasicFunction.ExportDraftHeader(ConWare, ClDoc, dateNow, codeWare,
                    functionType, 0, $@"انتقال بارکد {item.Cells["Barcode"]}  از انبار  {item.Cells["StoreName"]} به انبار  {menuStoresDestination.Text}", item.Cells["Barcode"].Value.ToString());

                BasicFunction.ExportDraftChild(headerWeaveId, Convert.ToInt32(item.Cells["CodeStore"].Value.ToString()), codeWare
                    , 1, item.Cells["Barcode"].Value.ToString(), dateNow, ConWare);

                ///جزیئات حواله به انبار بافندگی
                //foreach (var item in gridEX8.CurrentRow)
                //    {
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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


        public void CheckBarcodesManagement(string barcodes)
        {
            var status = false;
            var arrayBarcodes = barcodes.Split(',');
            var models = GetDetailBarcode(barcodes);
            foreach (var item in arrayBarcodes)
            {
                status = !models.Any(c => c.Barcode.Trim() == item.Trim());
                if (status)
                {
                    invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item, Message = $@"بار کد {item} وجود ندارد" });
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
                        invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item.Barcode, Message = $@"بارکد {item.Barcode} قبلا ثبت شده است" });
                        IsCheckBarcodesValid = false;
                    }
                }
            }


        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBarcodes.Text))
                {
                    MessageBox.Show("لطفا یک بارکد را برای انتقال انتخاب کنید");
                    return;
                }
                //var stateLoop = Parallel.ForEach(gridEX8.GetRows(), item =>
                //   {
                //       Recipt(item);
                //       Draft(item);
                //       ChangeCodeStoreOfBarcode(item);
                //   }).IsCompleted;
                //while (!stateLoop)
                //{
                //}
                if (!checkRegAuto.Checked)
                {
                    if (string.IsNullOrEmpty(menuStoresStart.Text.Trim()) || menuStoresStart.Value.ToString().Trim() == "0" || string.IsNullOrEmpty(menuStoresStart.Value.ToString().Trim()))
                    {
                        MessageBox.Show("لطفا انبار مبدا را انتخاب کنید");
                        return;
                    }
                }
                var res = Task.Factory.StartNew(() =>
                  {

                      foreach (var item in gridEX8.GetRows())
                      {
                          Recipt(item);
                          Draft(item);
                          ChangeCodeStoreOfBarcode(item);
                      }
                  }).ContinueWith((t) =>
                  {
                      MessageBox.Show("انتقال با موفقیت انجام شد !");
                      this.Close();
                  });
                //res.Wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show("انتقال با شکست مواجه شد!");
                MessageBox.Show(ex.Message);
            }

        }

        private void gridEX8_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            var row = gridEX8.GetRow();
            var barcode = row.Cells["Barcode"].Text.ToString();
            if (!string.IsNullOrEmpty(barcode))
            {
                txtBarcode.Text = barcode;
                txtDeviceName.Text = row.Cells["NameDevice"].Text.ToString();
                txtStoreName.Text = row.Cells["StoreName"].Text.ToString();
                txtDescription.Text = row.Cells["Description"].Text.ToString();
            }
            return;
        }

        private void gridEX8_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void menuStoresDestination_ValueChanged(object sender, EventArgs e)
        {

        }

        private void menuStoresDestination_TextChanged(object sender, EventArgs e)
        {
            //var name = menuStoresDestination.Text;
            //if (!string.IsNullOrEmpty(name))
            //{
            //    menuStoresDestination.DataSource = GetStores(name);
            //    return;
            //}
            //else
            //{
            //    menuStoresDestination.DataSource = GetStores(null);
            //    return;
            //}
        }

        private void menuStoresStart_TextChanged(object sender, EventArgs e)
        {
            //var name = menuStoresStart.Text;
            //if (!string.IsNullOrEmpty(name))
            //    menuStoresStart.DataSource = GetStores(name);
            //else
            //    menuStoresStart.DataSource = GetStores(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBarcodes.Text.Trim()))
                {
                    MessageBox.Show("لطفا بارکد هارا وارد نمایئد سپس اقدام فرمایئد");
                    return;
                }
                gridEX8.ClearItems();
                var barcodes = GetBarcodes(txtBarcodes.Text.Trim());
                CheckBarcodesManagement(GetOrginalCodes(barcodes));
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
                //ShowMessageBarcodeNotValid(BarcodeIsValid(barcodes));
                //FillDetailBarcode(barcodes);
                ShowDetailBarcode(barcodes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public string GetOrginalCodes(string[] barcodes)
        {
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
        private string[] GetBarcodes(string barcodes) => barcodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Distinct().ToArray();
        public void ShowDetailBarcode(string[] barcodes)
        {
            var codes = GetOrginalCodes(barcodes);
            var models = GetDetailBarcode(codes);
            foreach (var item in models)
            {
                var row = gridEX8.AddItem();
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
                row.Cells[19].Value = item.StoreName;
                row.Cells[20].Value = item.CodeStore;
                row.EndEdit();
            }
        }
        //public void FillDetailBarcode(string[] barcodes)
        //{

        //    var codes = GetOrginalCodes(barcodes);
        //    detailBarcodeViewModels = GetDetailBarcode(codes);

        //    var descs = detailBarcodeViewModels.Where(d => !string.IsNullOrEmpty(d.Description)).Select(c => c.Description);
        //    if (detailBarcodeViewModels != null && detailBarcodeViewModels.Count() != 0)
        //    {
        //        var first = detailBarcodeViewModels.First();
        //        txt_weight.Text = detailBarcodeViewModels.Sum(d => d.Weight).ToString();
        //        txt_Description.Text = string.Join(System.Environment.NewLine, descs);
        //        lblNameDevice.Text = first.NameDevice;
        //        lblClothType.Text = first.ClothName;
        //        txt_NumberOrder.Text = barcodes.Count().ToString();
        //        countHaveDesc = descs.Count();
        //    }
        //}



    }
}
