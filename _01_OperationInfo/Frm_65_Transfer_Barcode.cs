using Dapper;
using Janus.Windows.GridEX;
using PCLOR.Classes;
using PCLOR.Models;
using PCLOR.MyBasicFunction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        //private int FunctionTypeWeave = 0;
        //private int FunctionTypeRecipt = 0;
        private string DateNowShamsi = "";
        private int CodeStoreWeaveHistory = 0;
        private int HeaderWeaveHistoryId = 0;
        private string HeadersReciptId = "";
        private string HeadersDraftId = "";
        private string[] Barcodes;
        public IEnumerable<DetailTranferBarcodeViewModel> DetailsTranferBarcode;

        public Frm_65_Transfer_Barcode()
        {
            InitializeComponent();
        }

        private void Frm_65_Transfer_Barcode_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pCLOR_1_1400DataSet1.Table_140_Transfer_Barcode' table. You can move, or remove it, as needed.
            this.table_140_Transfer_BarcodeTableAdapter.Fill(this.pCLOR_1_1400DataSet1.Table_140_Transfer_Barcode);
            FillFuntionTypes();
            txtTimeCreate.Text = DateTime.Now.TimeOfDay.ToString().Remove(5);
            btnTransfer.Enabled = false;
            DateNowShamsi = DateTime.Now.ToShamsi();
            menuStoresDestination.DataSource = GetStores(null);
            menuStoresStart.DataSource = GetStores(null);
            if (checkRegAuto.Checked)
                menuStoresStart.Text = string.Empty;
        }


        private void FillFuntionTypes()
        {
            var query = $@"Select ColumnId as Id ,Column02 as Name from table_005_PwhrsOperation where Column16=1
                                     Select ColumnId as Id ,Column02  as Name from table_005_PwhrsOperation where Column16=0";
            using (IDbConnection db = new SqlConnection(ConWare.ConnectionString))
            {
                var res = db.QueryMultiple(query);
                menuFunctionTypeDraft.DataSource = res.Read<FunctionTypesViewModel>();
                menuFunctionTypeRecipt.DataSource = res.Read<FunctionTypesViewModel>();
            }
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

        public int Recipt(GridEXRow row)
        {
            try
            {
                //var dateNow = DateTime.Now.ToShamsi();
                var codeCommodity = row.Cells["CodeCommodity"].Value.ToString();
                var barcode = row.Cells["Barcode"].Value.ToString();
                var weight = row.Cells["Weight"].Value.ToString();
                var deviceId = row.Cells["DeviceId"].Value.ToString();
                var headerReciptId = BasicFunction.Recipt(ConWare, txt_DateTime.Text, 0, ClDoc, Convert.ToInt32(menuStoresDestination.Value.ToString()), Convert.ToInt32(menuFunctionTypeRecipt.Value.ToString()), null, "", codeCommodit: codeCommodity);
                HeadersReciptId = headerReciptId.ToString();
                return MyBasicFunction.BasicFunction.ReciptChild(ConWare: ConWare, headerReciptId, value: 1, Convert.ToInt32(codeCommodity), weight: Convert.ToDecimal(weight), barcode: barcode, Convert.ToInt32(deviceId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

        }

        public void ChangeCodeStoreOfBarcode(GridEXRow row, int codeStore)
        {
            try
            {
                var barcode = row.Cells["Barcode"].Value.ToString();
                var codeStoreDestination = Convert.ToInt32(menuStoresDestination.Value);
                var query = $@"
                                update Table_115_Product Set CodeStore={codeStore}
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

        public int Draft(GridEXRow item, int codeSourceStore)
        {
            try
            {
                //var dateNow = DateTime.Now.ToShamsi();
                //var model = GetCodeOfStoreWeave();
                //var functionType = model["FunctionType"];
                ////var item = row;
                //var codeWare = 0;
                if (codeSourceStore == 0)
                    codeSourceStore = Convert.ToInt32(item.Cells["CodeStore"].Value.ToString());
                //else
                //    codeWare = Convert.ToInt32(item.Cells["CodeStore"].Value.ToString());
                if (CodeStoreWeaveHistory != codeSourceStore)
                {
                    var headerWeaveId = BasicFunction.ExportDraftHeader(ConWare, ClDoc, txt_DateTime.Text, codeSourceStore,
                      Convert.ToInt32(menuFunctionTypeDraft.Value.ToString()), 0, $@"انتقال بارکد {item.Cells["Barcode"].Value}  از انبار  {item.Cells["CurrentStore"].Value} به انبار  {menuStoresDestination.Text}", item.Cells["Barcode"].Value.ToString());
                    HeaderWeaveHistoryId = headerWeaveId;
                    CodeStoreWeaveHistory = codeSourceStore;
                    HeadersDraftId += headerWeaveId.ToString() + ",";
                }
                return BasicFunction.ExportDraftChild(HeaderWeaveHistoryId, Convert.ToInt32(item.Cells["CodeStore"].Value.ToString()), CodeStoreWeaveHistory
                       , 1, item.Cells["Barcode"].Value.ToString(), txt_DateTime.Text, ConWare);

                ///جزیئات حواله به انبار بافندگی
                //foreach (var item in gridEX8.CurrentRow)
                //    {
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
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
            DetailsTranferBarcode = GetDetailBarcode(barcodes);
            foreach (var item in arrayBarcodes)
            {
                status = !DetailsTranferBarcode.Any(c => c.Barcode.Trim() == item.Remove(item.Length - 1).Substring(2).Trim());
                if (status)
                {
                    invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item, Message = $@"بار کد {item} وجود ندارد" });
                    IsCheckBarcodesValid = false;
                }
            }
            //if (IsCheckBarcodesValid)
            //{
            //    foreach (var item in models)
            //    {
            //        status = models.Any(c => c.Barcode.Trim() == item.Barcode.Remove(item.Barcode.Length - 1).Substring(2) && item.IsRegToOrderColor == true);
            //        if (status)
            //        {
            //            invalidBarcodes.Add(new InvalidBarcodeViewModel() { Barcode = item.Barcode, Message = $@"بارکد {item.Barcode} قبلا ثبت شده است" });
            //            IsCheckBarcodesValid = false;
            //        }
            //    }
            //}
        }



        //private void DeleteTransferCode

        private int AddToTranferBarcode(string barcode, string previousStore, string currentStore, int idChildRecipt, int idChildDraft, int previousStoreCode)
        {
            var query = $@"
INSERT INTO [dbo].[Table_140_Transfer_Barcode]
           ([Barcode]
           ,[PreviousStore]
           ,[CurrentStore]
           ,[DateTransfer]
           ,[FunctionTypeRecipt]
           ,[FunctionTypeDraft]
           ,[ReciptChildId]
           ,[DraftChildId]
           ,[CreateUser]
           ,[CreateDate]
           ,[EditUser]
           ,[EditDate] ,[CurrentStoreCode]
           ,[PreviousStoreCode],[Description])
VALUES
            (
            @Barcode,@PreviousStore,@CurrentStore,@DateTransfer,@FunctionTypeRecipt,@FunctionTypeDraft,
            @ReciptChildId,@DraftChildId,@CreateUser,GETDATE() , 
            @EditUser,@EditDate,@CurrentStoreCode,@PreviousStoreCode,@Description
            )
SELECT SCOPE_IDENTITY();
";
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                return db.QueryFirstOrDefault<int>(query, new
                {
                    @Barcode = barcode,
                    @PreviousStore = previousStore,
                    @CurrentStore = currentStore,
                    @DateTransfer = txt_DateTime.Text.Trim(),
                    @FunctionTypeRecipt = menuFunctionTypeRecipt.Text.Trim(),
                    @FunctionTypeDraft = menuFunctionTypeDraft.Text.Trim(),
                    @ReciptChildId = idChildRecipt,
                    @DraftChildId = idChildDraft,
                    @CurrentStoreCode = Convert.ToInt32(menuStoresDestination.Value.ToString()),
                    @PreviousStoreCode = previousStoreCode,
                    @CreateUser = Class_BasicOperation._UserName,
                    @EditUser = Class_BasicOperation._UserName,
                    @EditDate = DateTime.Now,
                    @Description = txt_Description.Text.Trim()
                });
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

                if (string.IsNullOrEmpty(menuFunctionTypeRecipt.Text.Trim()) || menuFunctionTypeRecipt.Value.ToString().Trim() == "0" || string.IsNullOrEmpty(menuFunctionTypeRecipt.Value.ToString().Trim()))
                {
                    MessageBox.Show("لطفا نوع رسید را  انتخاب کنید");
                    return;
                }

                if (string.IsNullOrEmpty(menuFunctionTypeDraft.Text.Trim()) || menuFunctionTypeDraft.Value.ToString().Trim() == "0" || string.IsNullOrEmpty(menuFunctionTypeDraft.Value.ToString().Trim()))
                {
                    MessageBox.Show("لطفا نوع حواله را  انتخاب کنید");
                    return;
                }

                if (!checkRegAuto.Checked)
                {
                    if (string.IsNullOrEmpty(menuStoresStart.Text.Trim()) || menuStoresStart.Value.ToString().Trim() == "0" || string.IsNullOrEmpty(menuStoresStart.Value.ToString().Trim()))
                    {
                        MessageBox.Show("لطفا انبار مبدا را انتخاب کنید");
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txt_DateTime.Text))
                {
                    MessageBox.Show("لطفا تاریخ انتقال را مشخص کنید");
                    return;
                }


                var idChildRecipt = 0;
                var idChildDraft = 0;
                var codeSourceStore = 0;
                if (!checkRegAuto.Checked)
                    codeSourceStore = Convert.ToInt32(menuStoresStart.Value);
                try
                {
                    foreach (var item in gridEX8.GetRows())
                    {
                        idChildRecipt = Recipt(item);
                        idChildDraft = Draft(item, codeSourceStore);
                        ChangeCodeStoreOfBarcode(item, Convert.ToInt32(item.Cells["CodeStore"].Value.ToString()));
                        item.BeginEdit();
                        item.Cells["TransferId"].Value = AddToTranferBarcode(item.Cells["Barcode"].Value.ToString(), item.Cells["CurrentStore"].Value.ToString(), menuStoresDestination.Text, idChildRecipt, idChildDraft, Convert.ToInt32(item.Cells["CodeStore"].Value.ToString()));
                        item.Cells["FunctionTypeRecipt"].Text = menuFunctionTypeRecipt.Text;
                        item.Cells["FunctionTypeDraft"].Text = menuFunctionTypeDraft.Text;
                        item.Cells["CreateUser"].Text = Class_BasicOperation._UserName.Trim();
                        item.Cells["DateTransfer"].Value = txt_DateTime.Text.Trim();
                        item.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    return;

                }
                HeadersDraftId = HeadersDraftId.TrimEnd(',');
                MessageBox.Show("");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("انتقال با شکست مواجه شد!");
                return;
            }
        }

        private void gridEX8_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            //var row = gridEX8.GetRow();
            ////var barcode = row.Cells["Barcode"].Text.ToString();
            //if (!string.IsNullOrEmpty(barcode))
            //{
            //    //txtTimeCreate.Text = barcode;
            //    //txtUserRegister.Text = row.Cells["NameDevice"].Text.ToString();
            //    //txtStoreName.Text = row.Cells["StoreName"].Text.ToString();
            //    //txtDescription.Text = row.Cells["Description"].Text.ToString();
            //}
            //return;
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
                Barcodes = barcodes;
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
                ShowDetailBarcode();
                btnTransfer.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        public IEnumerable<DetailTranferBarcodeViewModel> GetDetailBarcode(string codes, string search = null, bool isSearch = false)
        {
            try
            {

                using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
                {
                    var length = codes.Split(',').Length;
                    string query = "";
                    if (isSearch)
                    {
                        query = $@"
       SELECT 
[TransferId]
      ,[ProductId]
      ,[PreviousStore]
      ,[CurrentStore]
      ,[Barcode]
      ,[FunctionTypeRecipt]
      ,[FunctionTypeDraft]
      ,[CreateDate]
      ,[CreateUser]
      ,[DateTransfer]
      ,[Weight]
      ,[BarcodeDescription]
      ,[TypeCloth]
      ,[NameCotton]
      ,[DeviceId]
      ,[CodeCommondity]
      ,[IsRegToOrderColor]
      ,[TransferDescription]
      ,[CodeStore]
  FROM [PCLOR_1_1400].[dbo].[V_DetailTransferBarcode]
       ";

                        if (!string.IsNullOrEmpty(search))
                        {
                            query += $@"     
Where  Barcode    Like    N'{search}'      OR   DateTransfer  = N'{search.Trim()}'  
";
                        }
                    }

                    else
                        query = $@"
SELECT TOP ({length}) [Weight]
      ,[Description] as BarcodeDescription
      ,[Machine] as DeviceId
      ,[CottonName] as NameCotton
      ,[ClothName] as TypeCloth
      ,[CodeCommodity] as CodeCommondity
      ,[Barcode]
      ,[StoreName]  as CurrentStore
      ,[CodeStore] 
      ,[IsRegToOrderColor]
  FROM [PCLOR_1_1400].[dbo].[DeatailBarcodes] 
where Barcode in   ({codes}) 
    ";
                    var res = db.Query<DetailTranferBarcodeViewModel>(query, null, commandType: CommandType.Text);
                    return res;

                    //db.ConnectionString = ConWare.ConnectionString;
                    //if (res != null)
                    //{
                    //    db.ConnectionString = ConBase.ConnectionString;
                    //    foreach (var item in res)
                    //    {
                    //        var names = db.Query<string>("GetNamePersonByTagCode", new { @TagCode1 = item.OperatorTag1, @TagCode2 = item.OperatorTag2 }, commandType: CommandType.StoredProcedure);
                    //        if (names != null && names.Count() != 0)
                    //        {
                    //            item.NameOperator1 = names.Last();
                    //            if (names.Count() == 2)
                    //                item.NameOperator2 = names.First();
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        private string[] GetBarcodes(string barcodes) => barcodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Distinct().ToArray();

        public void ShowDetailBarcode(IEnumerable<DetailTranferBarcodeViewModel> models = null, bool IsSearch = false)
        {
            try
            {
                if (models == null || models.Count() == 0)
                    models = DetailsTranferBarcode;
                foreach (var item in models)
                {
                    var row = gridEX8.AddItem();
                    row.BeginEdit();
                    row.Cells[0].Value = item.Barcode;
                    row.Cells[1].Value = item.CurrentStore;
                    row.Cells[2].Value = item.Weight;
                    row.Cells[3].Value = item.NameCotton;
                    row.Cells[4].Value = item.TypeCloth;
                    row.Cells[5].Value = item.BarcodeDescription;
                    row.Cells[6].Value = item.IsRegToOrderColor;
                    row.Cells[7].Value = item.PreviousStore;
                    if (IsSearch)
                    {
                        row.Cells[8].Value = item.FunctionTypeRecipt;
                        row.Cells[9].Value = item.FunctionTypeDraft;
                        row.Cells[10].Value = item.CreateUser;
                        row.Cells[11].Value = item.DateTransfer;
                        row.Cells[12].Value = item.CreateUser;
                        row.Cells[13].Value = item.TransferId;
                        row.Cells[15].Value = item.TransferDescription;
                    }
                    row.Cells[14].Value = item.ProductId;
                    row.Cells[16].Value = item.CodeCommondity;
                    row.Cells[17].Value = item.DeviceId;
                    row.Cells[18].Value = item.CodeStore;
                    //row.Cells[15].Value = item.Date;
                    //row.Cells[16].Value = item.CodeCommodity;
                    //row.Cells[17].Value = item.CodeStore;
                    //row.Cells[18].Value = item.DeviceId;
                    //row.Cells[19].Value = item.StoreName;
                    //row.Cells[20].Value = item.CodeStore;
                    row.EndEdit();
                }
            }
            catch (Exception)
            {
            }

            //var models = GetDetailBarcode(codes);

        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {

        }

        private void btn_Print_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                Barcodes = GetBarcodes(txtBarcodes.Text.Trim());
                ShowDetailBarcode(GetDetailBarcode(GetOrginalCodes(Barcodes), txt_Search.Text.Trim(), true), true);
            }
            return;
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
