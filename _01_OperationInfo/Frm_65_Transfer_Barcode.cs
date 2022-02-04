using Dapper;
using PCLOR.Models;
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
        public Frm_65_Transfer_Barcode()
        {
            InitializeComponent();
        }

        private void Frm_65_Transfer_Barcode_Load(object sender, EventArgs e)
        {
            gridEX4.DataSource = GetBarcodes();
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
     SELECT    [Weight]
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

        private void gridEX1_MouseClick(object sender, MouseEventArgs e)
        {
            var row = gridEX4.GetRow();
            var barcode = row.Cells["BarcodeNumber"].Text.ToString();
            if (!string.IsNullOrEmpty(barcode))
            {
                txtBarcode.Text = barcode;
                txtDeviceName.Text = row.Cells["DeviceName"].Text.ToString();
                txtStoreName.Text = row.Cells["StoreName"].Text.ToString();
            }
            return;
        }
    }
}
