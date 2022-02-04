using Dapper;
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
using System.Windows.Forms;

namespace PCLOR._01_OperationInfo
{
    public partial class Frm_65_Transfer_Barcode : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();


        public Frm_65_Transfer_Barcode()
        {
            InitializeComponent();
        }

        private void Frm_65_Transfer_Barcode_Load(object sender, EventArgs e)
        {
            gridEX8.DataSource = GetBarcodes();
            menuStoresDestination.DataSource = GetStores(null);
            menuStoresStart.DataSource = GetStores(null);
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
                    query += $@"        where  StoreName like    N'{name}'  ";
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
SELECT  [Weight]
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
            var row = gridEX8.GetRow();
            var barcode = row.Cells["Barcode"].Text.ToString();
            if (!string.IsNullOrEmpty(barcode))
            {
                txtBarcode.Text = barcode;
                txtDeviceName.Text = row.Cells["NameDevice"].Text.ToString();
                txtStoreName.Text = row.Cells["StoreName"].Text.ToString();
            }
            return;
        }

        private void checkRegAuto_CheckedChanged(object sender, EventArgs e)
        {
            var status = checkRegAuto.Checked;
            if (status)
            {
                menuStoresStart.ReadOnly = false;
            }
            else
            {
                menuStoresStart.ReadOnly = true;
            }
        }

        public void Recipt()
        {
            try
            {
                var dateNow = DateTime.Now.ToShamsi();
                int functionType = Convert.ToInt16(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=30"));
                var codeCommodity = gridEX8.CurrentRow.Cells["CodeCommodity"].Value.ToString();
                BasicFunction.Recipt(ConWare, dateNow, 0, ClDoc, Convert.ToInt32(menuStoresDestination.Value.ToString()), functionType, null, "", codeCommodit: codeCommodity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void ChangeCodeStoreOfBarcode()
        {
            try
            {
                var barcode = gridEX8.CurrentRow.Cells["Barcode"].Value.ToString();
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

        public void Draft()
        {
            try
            {
                var dateNow = DateTime.Now.ToShamsi();
                var model = GetCodeOfStoreWeave();
                var functionType = model["FunctionType"];
                var item = gridEX8.CurrentRow;
                var codeWare = 0;
                if (checkRegAuto.Checked)
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

        private void btnTransfer_Click(object sender, EventArgs e)
        {

        }
    }
}
