using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Report;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Threading;
using Stimulsoft.Report;
using DevComponents.DotNetBar;
using System.Drawing;
using Janus.Windows.GridEX;
namespace PCLOR._01_OperationInfo
{
    public partial class Frm_35_packingNew : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();

        //Int16 _ware = 0, _func = 0;
        string DraftID = "0";
        int DraftNumber = 0;
        int ResidNum = 0;
        string IdDraft = "";
        string ouptputtext = "";
        SerialPort comport = new SerialPort();
        StringBuilder sb;
        string commandtxt = string.Empty;
        //SqlParameter DraftId, DraftNum, ReciptId, ReciptNum;
        string Id = "";
        DataTable dtReturn;

        private void OpenPort()
        {
            bool error = false;
            // If the port is open, close it.
            if (comport.IsOpen) comport.Close();
            else
            {
                // Set the port's settings
                try
                {
                    comport.BaudRate = int.Parse(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=12"));//1200
                    comport.DataBits = 8;//7
                    comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
                    comport.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
                    comport.PortName = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=13");
                    comport.ReadBufferSize = 2;
                }
                catch { } try
                {
                    // Open the port
                    if (!comport.IsOpen)
                        comport.Open();
                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (IOException) { error = true; }
                catch (ArgumentException) { error = true; }

                if (error) MessageBox.Show(this, "Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavalible", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        public Frm_35_packingNew()
        {
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            InitializeComponent();
        }
        int i = 0;
        string s = "";
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {


            if (!comport.IsOpen) return;

            int bytes = comport.BytesToRead;
            byte[] buffer = new byte[bytes];
            comport.Read(buffer, 0, bytes);

            sb = new StringBuilder(buffer.Length * 3);

            foreach (byte b in buffer)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }


            ouptputtext = sb.ToString().ToUpper();
            s += ouptputtext;

            txt_weight.BeginInvoke(new EventHandler(delegate
            {
                try
                {
                    txt_weight.Text = (Convert.ToInt64(s.Substring(0, 8).Replace("BB", ""))).ToString();
                }
                catch { s = ""; }
                if (s.Length > 8)
                    s = "";

            }));


        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_Auto.Checked)
            {
                comport.Close();
            }
            else
            {
                OpenPort();
                //comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txt_weight.Text = "0";
        }

        private void Frm_35_packing_Load(object sender, EventArgs e)
        {

            mlt_Num_Product.Focus();
            uiPanel2.TabIndex = 2;
            try
            {
                txtDateHavale.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                txt_Date_Recipt.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");

                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

                mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");
                mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT        dbo.Table_005_TypeCloth.ID, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_005_TypeCloth.CodeCommondity, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh
                    FROM            dbo.Table_005_TypeCloth INNER JOIN
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON dbo.Table_005_TypeCloth.CodeCommondity = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid");
                mlt_NameCustomer.DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column01 from Table_045_PersonInfo");
                mlt_TypeColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeColor from Table_010_TypeColor");
              
//                mlt_CodeOrderColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_035_Production.Number AS NumberOrder, dbo.Table_025_HederOrderColor.Number, dbo.Table_010_TypeColor.TypeColor, 
//                      dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_035_Production.ID
//                      FROM         dbo.Table_025_HederOrderColor INNER JOIN
//                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk INNER JOIN
//                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID INNER JOIN
//                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID INNER JOIN
//                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId");

                mlt_CodeOrderColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,Number from Table_025_HederOrderColor");

                mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 2)");



                mlt_Ware_R.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");


                mlt_Function.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_R.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");

               mlt_TypeRerturn .DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
//               ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
//                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
//                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
//                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");



               mlt_Return.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");


                gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from Table_007_PwhrsDraft");
                gridEX2.DropDowns["NumProduct"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,Number from Table_035_Production ");
                gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");


//                mlt_Num_Product.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT * FROM  ( SELECT tp.ID, tp.Number ,t30.NumberOrder-ISNULL((SELECT sum(NumberProduct) FROM Table_035_Production WHERE ColorOrderId= t30.Id ),0) AS Remain
//											FROM Table_030_DetailOrderColor  AS t30
//											INNER JOIN Table_035_Production tp ON tp.ColorOrderId=t30.ID) AS T  ORDER BY Number DESC");


                mlt_Num_Product.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,Number from Table_035_Production ORDER BY Number DESC");


                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=4");

                mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=16");

                mlt_Function_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=5");

                mlt_Return.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=28");

                mlt_TypeRerturn.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=29"); 

                ToastNotification.ToastForeColor = Color.Black;
                ToastNotification.ToastBackColor = Color.SkyBlue;
                //btn_New.Enabled = true;
                //rb_Auto.Enabled = false;
                //ch_Auto.Enabled = false;
                //rb_select.Enabled = false;
                mlt_Num_Product.Focus();
            }
            catch { }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {

            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 24))
            {
                try
                {
                    table_050_Packaging1BindingSource.AddNew();
                    ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["date"] = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                    ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Time"] = FarsiLibrary.Utils.PersianDate.Now.Time.ToString();
                    ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                    ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();
                    mlt_Num_Product.Focus();

                    btn_New.Enabled = false;
                    rb_Auto.Enabled = true;
                    ch_Auto.Enabled = true;
                    rb_select.Enabled = true;
                }

                catch { }
            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }

        }

        private void btn_end_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Num_Product.Text == "")
                {
                    Class_BasicOperation.ShowMsg("", "لطفا شماره کارت تولید را وارد کنید", Class_BasicOperation.MessageType.None);
                }
                else
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 24))
                    {
                        try
                        {


                            //   btn_New.Enabled = false;
                            rb_Auto.Enabled = true;
                            ch_Auto.Enabled = true;
                            rb_select.Enabled = true;
                        }

                        catch { }

                        //////////////

                        if (txt_weight.Text == "" || txt_weight.Text == "0" || txt_meter.Text == "" )
                        {
                            MessageBox.Show("اطلاعات را تکمیل نمایید");

                        }

                        else
                        {
                            Int64 Barcode = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select isnull((select max(Barcode) from Table_050_Packaging),9999999)+1"));
                            // if (btn_New.Enabled == false)

                            {
                                double weigh = Convert.ToDouble(txt_weight.Text) / 1000;
                                table_050_Packaging1BindingSource.AddNew();
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Barcode"] = Barcode;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["IDProduct"] = mlt_Num_Product.Value;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["weight"] = weigh;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Qty"] = 1;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Meter"] = txt_meter.Text;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["TypeColor"] = mlt_TypeColor.Text;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Machine"] = mlt_Machine.Text;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Description"] = txt_Desc.Text;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["date"] = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["Time"] = FarsiLibrary.Utils.PersianDate.Now.Time.ToString();
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();
                                table_050_Packaging1BindingSource.EndEdit();
                                table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
                                string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
                        FROM         dbo.Table_035_Production  left JOIN
                                  dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                        
                        where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
                                GROUP BY dbo.Table_035_Production.weight");
                                txt_Rem_Weight.Text = sum;
                                this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                                txt_weight.Text = "0";
                                txt_meter.Text = "0";
                                txt_Desc.Text = "";
                                txt_weight.Focus();

                            }
                            //else
                            //{
                            //    MessageBox.Show("لطفا برای درج اطلاعات دکمه جدید را بزنید");
                            //}
                        }

                        if (ch_Auto.Checked == true)
                        {

                            Print1(dataSet_05_PCLOR.Table_050_Packaging1.Compute("Max(ID)", "").ToString(), txt_Print.Text);

                        }
                    }
                    else
                    {

                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                    }
                }

            }

            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Num_Product.Text == "")
                {
                    MessageBox.Show("اطلاعات مورد نظر را تکمیل نمایید");
                }

                else
                {
                    //string NumberProduct = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( select ColorOrderId from Table_035_Production where Id="+mlt_Num_Product.Value+"),0)");
                    //if (NumberProduct!="0")
                    //{
                        
                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_010_TypeColor.TypeColor, 
                      dbo.Table_030_DetailOrderColor.NumberOrder, SUM(dbo.Table_035_Production.NumberProduct) AS NumberProduct, dbo.Table_030_DetailOrderColor.Title, 
                      dbo.Table_035_Production.Number, dbo.Table_025_HederOrderColor.Number AS NumberOrde, dbo.Table_035_Production.ID, dbo.Table_030_DetailOrderColor.Printer,
                       dbo.Table_030_DetailOrderColor.Description, dbo.Table_035_Production.Machine, dbo.Table_035_Production.weight
FROM         dbo.Table_025_HederOrderColor INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID
GROUP BY dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_030_DetailOrderColor.NumberOrder, 
                      dbo.Table_030_DetailOrderColor.Title, dbo.Table_035_Production.Number, dbo.Table_025_HederOrderColor.Number, dbo.Table_035_Production.ID, 
                      dbo.Table_030_DetailOrderColor.Printer, dbo.Table_030_DetailOrderColor.Description, dbo.Table_035_Production.Machine, dbo.Table_035_Production.weight, 
                      dbo.Table_010_TypeColor.TypeColor
                    HAVING      dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + " ");

                    if (dt.Rows.Count > 0)
                    {
                        mlt_NameCustomer.Value = dt.Rows[0][0].ToString();
                        mlt_TypeCloth.Value = dt.Rows[0][1].ToString();
                        mlt_TypeColor.Text = dt.Rows[0][2].ToString();
                        mlt_CodeOrderColor.Value = dt.Rows[0][7].ToString();
                        txt_CountProduct.Text = dt.Rows[0][4].ToString();
                        txt_Title.Text = dt.Rows[0][5].ToString();
                        txt_ID.Text = dt.Rows[0][8].ToString();
                        txt_Title.Text = dt.Rows[0][5].ToString();
                        txt_Print.Text = dt.Rows[0][9].ToString();
                        txt_Description.Text = dt.Rows[0][10].ToString();
                        mlt_Machine.Value = dt.Rows[0][11].ToString();
                        txt_weight_P.Text = dt.Rows[0][12].ToString();
                        string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
                        FROM         dbo.Table_035_Production  left JOIN
                                  dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                         where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
                                GROUP BY dbo.Table_035_Production.weight");
                        txt_Rem_Weight.Text = sum;
                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));


                        //txt_weight.Focus();

                    }
//                }

//                    else if (NumberProduct=="0")
//                    {
//                                           DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT        SUM(dbo.Table_035_Production.NumberProduct) AS NumberProduct, dbo.Table_035_Production.Number, dbo.Table_035_Production.ID, dbo.Table_035_Production.Machine, dbo.Table_035_Production.weight, 
//                         dbo.Table_035_Production.CodeCustomer, dbo.Table_035_Production.TypeCloth, dbo.Table_010_TypeColor.TypeColor, dbo.Table_035_Production.Printer, dbo.Table_035_Production.Description
//FROM            dbo.Table_035_Production INNER JOIN
//                         dbo.Table_010_TypeColor ON dbo.Table_035_Production.TypeColor = dbo.Table_010_TypeColor.ID
//GROUP BY dbo.Table_035_Production.Number, dbo.Table_035_Production.ID, dbo.Table_035_Production.Machine, dbo.Table_035_Production.weight, dbo.Table_035_Production.CodeCustomer, 
//                         dbo.Table_035_Production.TypeCloth, dbo.Table_010_TypeColor.TypeColor, dbo.Table_035_Production.Printer, dbo.Table_035_Production.Description
//HAVING        (Number = "+mlt_Num_Product.Text+") ");

//                    if (dt.Rows.Count > 0)
//                    {
//                        mlt_NameCustomer.Value = dt.Rows[0]["CodeCustomer"].ToString();
//                        mlt_TypeCloth.Value = dt.Rows[0]["TypeCloth"].ToString();
//                        mlt_TypeColor.Text = dt.Rows[0]["TypeColor"].ToString();
//                        mlt_CodeOrderColor.Value = null;
//                        txt_CountProduct.Text = dt.Rows[0]["NumberProduct"].ToString();
//                        txt_Title.Text = "";
//                        txt_ID.Text = dt.Rows[0]["ID"].ToString();
//                        txt_Title.Text = "";
//                        txt_Print.Text = dt.Rows[0]["Printer"].ToString();
//                        txt_Description.Text = dt.Rows[0]["Description"].ToString();
//                        mlt_Machine.Value = dt.Rows[0]["Machine"].ToString();
//                        txt_weight_P.Text = dt.Rows[0]["weight"].ToString();
//                        string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
//                        FROM         dbo.Table_035_Production  left JOIN
//                                  dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
//                         where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
//                                GROUP BY dbo.Table_035_Production.weight");
//                        txt_Rem_Weight.Text = sum;
//                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
//                        txt_weight.Focus();
//                    } 
                    //}




                }

            }

            catch
            { }

        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 58))
            {
                if (gridEX2.RowCount > 0)
                {


                    string pagename = txt_Print.Text;
                    string NumberOrder = txt_ID_P.Text;
                    Print(NumberOrder, pagename);
                }
                else
                {
                    MessageBox.Show("سطری برای چاپ وجود ندارد");
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void Print(string NumberOrder, string pagename)
        {
            if (txt_Print.Text == "")
            {
                MessageBox.Show("لطفا اسم چاپ مورد نظر را وارد نمایید");
            }
            else
            {

                Frm_01_ReportPackiging frm = new Frm_01_ReportPackiging(NumberOrder, pagename);
                frm.ShowDialog();

            }

        }
        private void Print1(string NumberOrder, string pagename)
        {
            if (txt_Print.Text == "")
            {
                MessageBox.Show("لطفا اسم چاپ مورد نظر را وارد نمایید");
            }
            else
            {
                try
                {

                    string s = ClDoc.ExScalar(ConPCLOR.ConnectionString, @" select Title from Table_80_Setting where ID=11 ");


                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_050_Packaging.ID, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_010_TypeColor.TypeColor, dbo.Table_025_HederOrderColor.Number, 
                      dbo.Table_030_DetailOrderColor.Title, dbo.Table_050_Packaging.date, dbo.Table_050_Packaging.Time, dbo.Table_035_Production.Number AS NumberOrder, 
                      dbo.Table_050_Packaging.weight, dbo.Table_030_DetailOrderColor.Description, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Meter, 
                      dbo.Table_050_Packaging.Description AS DescriptionP, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column01 AS CodeCustomer
                      FROM         dbo.Table_050_Packaging INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_050_Packaging.IDProduct = dbo.Table_035_Production.ID INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID INNER JOIN
                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dbo.Table_025_HederOrderColor.CodeCustomer = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId
                  WHERE     dbo.Table_050_Packaging.ID in(" + (NumberOrder == "" ? gridEX2.GetValue("ID").ToString() : NumberOrder) + ")");


                    StiReport stireport = new StiReport();

                    stireport.Load("Report.mrt");

                    for (int i = 0; i < stireport.Pages.Count; i++)
                    {
                        stireport.Pages[i].Enabled = false;
                    }

                    stireport.Pages[pagename].Enabled = true;
                    stireport.Compile();
                    stireport.RegData("dt", dt);
                    this.Cursor = Cursors.Default;
                    stireport.Render(false);
                    System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();
                    printerSettings.Copies = 1;
                    printerSettings.FromPage = 1;
                    printerSettings.ToPage = stireport.RenderedPages.Count;
                    String PrinterName = s;
                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (printer.ToString() == PrinterName)
                            printerSettings.PrinterName = printer;
                    }
                    stireport.Print(false, printerSettings);
                }

                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }
        string a;
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 25))
                {
                    if (gridEX2.RowCount > 0 && Convert.ToInt32(gridEX2.CurrentRow.RowIndex) >= 0)
                    {
                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode
                        FROM                    dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                            WHERE     (dbo.Table_70_DetailOtherPWHRS.Barcode = " + gridEX2.GetValue("Barcode").ToString() + ")");

                           
                            DataTable dtsal = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_011_Child1_SaleFactor.Column34
                            FROM            dbo.Table_010_SaleFactor INNER JOIN
                                                     dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                            WHERE        (dbo.Table_011_Child1_SaleFactor.Column34 = N'" + gridEX2.GetValue("Barcode").ToString() + "')");
                          
                            DataTable dtreturn = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_019_Child1_MarjooiSale.Column32
                        FROM            dbo.Table_018_MarjooiSale INNER JOIN
                                                 dbo.Table_019_Child1_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                        WHERE        (dbo.Table_019_Child1_MarjooiSale.Column32 = N'" + gridEX2.GetValue("Barcode").ToString() + "')");



                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("از این بار کد در فرم ارسال و دریافت بارکد استفاده شده است امکان حذف آن را ندارید");
                                return;

                            }

                            else     if (dtsal.Rows.Count > 0)
                            {
                                MessageBox.Show("این بارکد در فاکتور فروش استفاده شده است امکان حذف ان را ندارید ");
                                return;

                            }


                            else  if (dtreturn.Rows.Count > 0)
                            {
                                MessageBox.Show("این بارکد در فاکتور مرجوعی استفاده شده است امکان حذف ان را ندارید ");
                                return;

                            }





                            if (((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" || ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0" || IdDraft != "0"
                                               || ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["FactorReturnId"].ToString() != "0")
                            {
                               
                                MessageBox.Show("این بارکد دارای رسید یا حواله یا فاکتور مرجوعی می باشد امکان حذف آن را ندارید");
                                return;
                            }
                      


                            else
                            {



                                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"delete from  table_050_Packaging where ID=" + gridEX2.GetValue("ID"));

                                /////پیغام حذف شد
                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                button1.Enabled = true;
                                ///فیل گرید
                                if (txt_ID.Text != "")
                                {
                                    this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                                }
                                ////
                                string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
                                      FROM         dbo.Table_035_Production  left JOIN
                                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                                      where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
                                GROUP BY dbo.Table_035_Production.weight");
                                txt_Rem_Weight.Text = sum;


                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("سطری برای حذف وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void txt_Number_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Frm_35_packing_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Control && e.KeyCode == Keys.N)
            {
                btn_New_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {
                btn_Delete_Click(sender, e);
            }


        }

        private void Frm_35_packing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comport.IsOpen)
            {

                e.Cancel = true; //cancel the fom closing

                Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnExit)); //close port in new thread to avoid hang

                CloseDown.Start(); //close port in new thread to avoid hang

            }


            try
            {
                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=4");

                mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=16");

                mlt_Function_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=5");

            }
            catch
            {

            }


        }


        private void CloseSerialOnExit()
        {

            try
            {

                comport.Close(); //close the serial port

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message); //catch any serial port closing error messages

            }

            this.Invoke(new EventHandler(NowClose)); //now close back in the main thread

        }

        private void NowClose(object sender, EventArgs e)
        {

            this.Close(); //now close the form

        }


        //private void gridEX2_EditingCell(object sender, Janus.Windows.GridEX.EditingCellEventArgs e)
        //{
        //    table_050_Packaging1BindingSource.EndEdit();
        //    table_050_PackagingTableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging);
        //    if (txt_ID.Text != "")
        //    {
        //        this.table_050_PackagingTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_050_Packaging, long.Parse(txt_ID.Text));
        //    }
        //}



        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
                    FROM         dbo.Table_035_Production  left JOIN
                    dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                    where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
                                GROUP BY dbo.Table_035_Production.weight");
            txt_Rem_Weight.Text = sum;
        }



        private void btn_edit_save_Click(object sender, EventArgs e)
        {
            table_050_Packaging1BindingSource.EndEdit();
            table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
            if (txt_ID.Text != "")
            {
                this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
            }
            ////
            string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
                                      FROM         dbo.Table_035_Production  left JOIN
                                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                                      where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
                                GROUP BY dbo.Table_035_Production.weight");
            txt_Rem_Weight.Text = sum;
            MessageBox.Show("تغییرات با موفقیت ذخیره شد");
        }



    

        private void btn_Recipt_Click(object sender, EventArgs e)
        {
            try
            {
                

                    if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", gridEX2.GetValue("NumberRecipt").ToString()) != 0)
                    {
                        throw new Exception(" رسید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");

                        if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column19", gridEX2.GetValue("NumberRecipt").ToString()) != 0)

                            throw new Exception(" رسید قطعی می باشد ابتدا آن را غیر قطعی نمایید");
                    }
                    else
                    {

                        table_050_Packaging1BindingSource.EndEdit();
                        ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_012_Child_PwhrsReceipt where Column01=" + gridEX2.GetValue("NumberRecipt"));
                        ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_011_PwhrsReceipt Where ColumnId=" + gridEX2.GetValue("NumberRecipt"));
                        dataSet_05_PCLOR.EnforceConstraints = false;
                        if (txt_ID.Text != "")
                        {
                            this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                        }
                        dataSet_05_PCLOR.EnforceConstraints = true;
                     
                    }

               
            }
            catch { }
        }

        private void btn_Draft_Click_1(object sender, EventArgs e)
        {
            try
            {
               

                    if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", gridEX2.GetValue("NumberDraft").ToString()) != 0)
                    {
                        throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");

                        if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column26", gridEX2.GetValue("NumberDraft").ToString()) != 0)

                            throw new Exception(" حواله این کارت تولید قطعی می باشد ابتدا آن را غیر قطعی نمایید");

                    }
                    else
                    {
                        table_050_Packaging1BindingSource.EndEdit();
                        ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_008_Child_PwhrsDraft where Column01=" + gridEX2.GetValue("NumberDraft"));
                        ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_007_PwhrsDraft Where ColumnId=" + gridEX2.GetValue("NumberDraft"));
                        int index = gridEX2.CurrentRow.RowIndex;
                        dataSet_05_PCLOR.EnforceConstraints = false;
                        if (txt_ID.Text != "")
                        {
                            this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                        }
                        dataSet_05_PCLOR.EnforceConstraints = true;
                        try
                        {
                            gridEX2.MoveToRowIndex(index);
                        }
                        catch { }
                
                    }

                }
          
            catch { }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);

        }


        private void btn_R_D_Click(object sender, EventArgs e)
        {
            try
            {


                {

                    if (mlt_Num_Product.Text != "")
                    {

                        

                        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 45))
                        {
                            if (gridEX2.RowCount > Convert.ToDecimal(txt_CountProduct.Text))
                            {
                               
                                if (MessageBox.Show("تعداد طاقه های وارد شده بیشتر از تعداد تولید می باشد آیا مایل به ثبت نهایی هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                    if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit)|| mlt_Return.Text.All(char.IsDigit) || mlt_TypeRerturn.Text.All(char.IsDigit))
                                    {
                                        MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                                        return;
                                    }
                                    else
                                    {
                                        FinalSave();
                                    }
                                }
                            }
                            else if (gridEX2.RowCount < Convert.ToDecimal(txt_CountProduct.Text))
                            {
                               
                                if (MessageBox.Show("تعداد طاقه های وارد شده کمتر از تعداد تولید می باشد آیا مایل به ثبت نهایی هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Return.Text.All(char.IsDigit) || mlt_TypeRerturn.Text.All(char.IsDigit))
                                    {
                                        MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                                        return;
                                    }
                                    else
                                    {
                                        FinalSave();
                                    }
                                }
                            }
                            else if (gridEX2.RowCount == Convert.ToDecimal(txt_CountProduct.Text))
                            {
                                if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Return.Text.All(char.IsDigit) || mlt_TypeRerturn.Text.All(char.IsDigit))
                                {
                                    MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                                    return;
                                }
                                else
                                {
                                    FinalSave();
                                }
                            }
                        }
                        else
                        {
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                        }
                        btn_New.Enabled = true;
                    }
                    else
                    {
                        Class_BasicOperation.ShowMsg("", "شماره کارت تولید را انتخاب کنید", Class_BasicOperation.MessageType.None);
                    }
                }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }


        private void FinalSave()
        {
           
            string s = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select NumberDraftP from Table_035_Production where ID=" + mlt_Num_Product.Value + "");
            string Id = "";
            string  IdReturn = "";
            bool Return = false;
            ResidNum = 0;
          
           

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
            {
                if (item.Cells["NumberRecipt"].Value.ToString() == "0"  && item.Cells["ReturnFactor"].Value.ToString() == "False")
                {
                    Id = Id + item.Cells["ID"].Value + ",";

                }
                if (item.Cells["NumberRecipt"].Value.ToString() == "0" && item.Cells["ReturnFactor"].Value.ToString() == "True")
                {
                    IdReturn = IdReturn + item.Cells["ID"].Value + ",";
                    Return = true;
                }
            }
            if (IdReturn != "" )
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

                //if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column37", 134))
                //{
                    ExportReciptReturn();
                //}
                // else
                //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

                      
            }

            if (Id != "" )
            {
                if (s == "0")
                {

                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()), int.Parse(((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString())))
                        throw new Exception("کالای " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + " در این انبار فعال نمی باشد ");
                    #region چک کردن باقی مانده برای کارت تولید
                    bool ok = true;

                    {
                        float Remain = FirstRemain(int.Parse(((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()), mlt_Ware.Value.ToString());
                        bool mojoodimanfi = false;
                        try
                        {
                            using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.PWHRS))
                            {
                                ConWareGood.Open();
                                SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                                           (
                                                                                               SELECT ISNULL(Column16, 0) AS Column16
                                                                                               FROM   table_004_CommodityAndIngredients
                                                                                               WHERE  ColumnId = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + @"
                                                                                           ),
                                                                                           0
                                                                                       ) AS Column16", ConWareGood);
                                mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());
                            }
                        }
                        catch
                        {
                        }
                        string good1 = string.Empty;
                        string Brand = string.Empty;
                        string Tamin = string.Empty;
                        if (Remain < int.Parse(txt_CountProduct.Text))
                        {
                            if (!mojoodimanfi)
                            {
                                good1 += ClDoc.ExScalar(ConWare.ConnectionString,
                                   "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()) + " , ";

                                Brand += "'" + mlt_TypeColor.Text + "'";
                                Tamin += "'" + mlt_Machine.Text + "'";
                                throw new Exception("عدم موجودی کالای : " + good1 + Environment.NewLine + "برند : " + Brand + Environment.NewLine + "دستگاه : " + Tamin);

                            }

                        }
                    }

                    #endregion چک کردن باقی مانده برای کارت تولید

                    ExportDraft();

                    ExportRecipt();
                  
                          
            
                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                    gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                    gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

                    if (IdReturn!="")
                    {
                        
                    ToastNotification.Show(this, "رسید انبار بسته بندی به شماره " + (ResidNum == null ? "0" : ResidNum.ToString()) + Environment.NewLine +
                       "حواله انبار سالن تولید به شماره " + DraftNumber.ToString() + Environment.NewLine + "رسید انبار مرجوعی به شماره " + 
                       (dtReturn.Rows[0][1] == null ? "0" : dtReturn.Rows[0][1]) + Environment.NewLine
                
                 + " با موفقیت صادر شد " + Environment.NewLine , 9000, eToastPosition.MiddleCenter);
                    }
                    else
                    {
                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره  " + ResidNum.ToString() + Environment.NewLine +
                   "حواله انبار سالن تولید به شماره" + DraftNumber.ToString() + " با موفقیت صادر شد ", 9000, eToastPosition.MiddleCenter);
                    }
                        
                        
                        if (txt_ID.Text != "")
                    { this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text)); }
                    ReceiptId = "0";
                    //DraftID = "0";

                }

                if (s != "0")
                {

                  
                        ExportRecipt();
                   
                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                    gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                    gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

                    if (IdReturn != "")
                    {

                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره " + (ResidNum == null ? "0" : ResidNum.ToString()) + Environment.NewLine +
                           "حواله انبار سالن تولید به شماره " + DraftNumber.ToString() + Environment.NewLine + "رسید انبار مرجوعی به شماره " + (dtReturn.Rows[0][1] == null ? "0" : dtReturn.Rows[0][1]) + Environment.NewLine
                   
                     + " با موفقیت صادر شد " + Environment.NewLine , 9000, eToastPosition.MiddleCenter);
                    }
                    else
                    {
                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره  " + ResidNum.ToString() + Environment.NewLine +
                   "حواله انبار سالن تولید به شماره" + DraftNumber.ToString() + " با موفقیت صادر شد ", 9000, eToastPosition.MiddleCenter);
                    }
                 
               
          

                    if (txt_ID.Text != "")
                    {
                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                    }
                    ReceiptId = "0";
                    DraftID = "0";


                }
            }
            else  if (IdReturn!="" && Id=="")
            {
                if (s == "0")
                {

                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()), int.Parse(((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString())))
                        throw new Exception("کالای " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + " در این انبار فعال نمی باشد ");
                    #region چک کردن باقی مانده برای کارت تولید
                    bool ok = true;

                    {
                        float Remain = FirstRemain(int.Parse(((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()), mlt_Ware.Value.ToString());
                        bool mojoodimanfi = false;
                        try
                        {
                            using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.PWHRS))
                            {
                                ConWareGood.Open();
                                SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                                           (
                                                                                               SELECT ISNULL(Column16, 0) AS Column16
                                                                                               FROM   table_004_CommodityAndIngredients
                                                                                               WHERE  ColumnId = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + @"
                                                                                           ),
                                                                                           0
                                                                                       ) AS Column16", ConWareGood);
                                mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());
                            }
                        }
                        catch
                        {
                        }
                        string good1 = string.Empty;
                        string Brand = string.Empty;
                        string Tamin = string.Empty;
                        if (Remain < int.Parse(txt_CountProduct.Text))
                        {
                            if (!mojoodimanfi)
                            {
                                good1 += ClDoc.ExScalar(ConWare.ConnectionString,
                                   "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()) + " , ";

                                Brand += "'" + mlt_TypeColor.Text + "'";
                                Tamin += "'" + mlt_Machine.Text + "'";
                                throw new Exception("عدم موجودی کالای : " + good1 + Environment.NewLine + "برند : " + Brand + Environment.NewLine + "دستگاه : " + Tamin);

                            }

                        }
                    }

                    #endregion چک کردن باقی مانده برای کارت تولید

                    ExportDraft();

                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                    gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                    gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

                    if (IdReturn != "")
                    {

                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره " + (ResidNum == null ? "0" : ResidNum.ToString()) + Environment.NewLine +
                           "حواله انبار سالن تولید به شماره " + DraftNumber.ToString() + Environment.NewLine + "رسید انبار مرجوعی به شماره " + (dtReturn.Rows[0][1] == null ? "0" : dtReturn.Rows[0][1]) + Environment.NewLine
                 
                     + " با موفقیت صادر شد " + Environment.NewLine, 9000, eToastPosition.MiddleCenter);
                    }
                    else
                    {
                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره  " + ResidNum.ToString() + Environment.NewLine +
                   "حواله انبار سالن تولید به شماره" + DraftNumber.ToString() + " با موفقیت صادر شد ", 9000, eToastPosition.MiddleCenter);
                    }


                    if (txt_ID.Text != "")
                    { this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text)); }
                    ReceiptId = "0";
                    //DraftID = "0";

                }

                if (s != "0")
                {


                   

                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                    gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                    gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

                    if (IdReturn != "")
                    {

                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره " + (ResidNum == null ? "0" : ResidNum.ToString()) + Environment.NewLine +
                           "حواله انبار سالن تولید به شماره " + DraftNumber.ToString() + Environment.NewLine + "رسید انبار مرجوعی به شماره " + (dtReturn.Rows[0][1] == null ? "0" : dtReturn.Rows[0][1]) + Environment.NewLine
                 
                     + " با موفقیت صادر شد " + Environment.NewLine, 9000, eToastPosition.MiddleCenter);
                    }
                    else
                    {
                        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره  " + ResidNum.ToString() + Environment.NewLine +
                   "حواله انبار سالن تولید به شماره" + DraftNumber.ToString() + " با موفقیت صادر شد ", 9000, eToastPosition.MiddleCenter);
                    }




                    if (txt_ID.Text != "")
                    {
                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                    }
                    ReceiptId = "0";
                    DraftID = "0";


                }
            }
            //else
            //{
            //    MessageBox.Show("یک بار رسید و حواله صادر شده است امکان ذخیره مجدد وجود ندارد");
            //}
        }
        string DetailsIdDraft = "";
        SqlParameter DraftId;
        private void ExportDraft()
        {
            //string Id = "";
            DraftNumber = 0;
            DraftId = new SqlParameter("DraftID", SqlDbType.Int);
            DraftId.Direction = ParameterDirection.Output;
            DetailsIdDraft = "";
            string CmdText = "";

            //درج کالاهای یک انبار در یک حواله
            DraftNumber = ClDoc.MaxNumber(Properties.Settings.Default.PWHRS, "Table_007_PwhrsDraft", "Column01");
            //درج هدر حواله برای هر انبار
            CmdText = (@"INSERT INTO Table_007_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                      column17, column18, column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26) VALUES(" + DraftNumber + ",'" + txtDateHavale.Text + "'," +
               mlt_Ware.Value + "," + mlt_Function.Value + ",0,'" + "حواله صادره بابت کارت تولید ش" + ((DataRowView)gridEX2.RootTable.Columns["IDProduct"].DropDown.FindItem(gridEX2.GetValue("IDProduct")))["Number"].ToString() + "',0,'" + Class_BasicOperation._UserName +
               "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0,0,0,0,0,0,null,0,1); SET @DraftID = SCOPE_IDENTITY();");
            int Unit = Convert.ToInt16(ClDoc.ExScalar(ConWare.ConnectionString, @"(select Column07 from table_004_CommodityAndIngredients where Columnid=" + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + ")"));

            CmdText = CmdText+(@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                        column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, column28, column29, Column31, Column32, Column33,Column34,Column35, Column36, Column37) VALUES(@DraftID,"
            + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + "," + Unit + @",0,0," + txt_CountProduct.Text + "," +
           txt_CountProduct.Text + ",0,0,0,0,N' " + ((DataRowView)mlt_Num_Product.DropDownList.FindItem(mlt_Num_Product.Value))["Number"].ToString() + "',NULL,NULL,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
            "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0,NULL,0,0," + (Convert.ToDecimal(txt_weight_P.Text) / Convert.ToDecimal(txt_CountProduct.Text)).ToString()
            + "," + txt_weight_P.Text + ",N'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() 
            + "',N'" + ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "')");






            CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.Table_035_Production set NumberDraftP=@DraftID  Where ID=" + gridEX2.GetValue("IDProduct") + "";

           
            using (SqlConnection Con = new SqlConnection(PCLOR.Properties.Settings.Default.PWHRS))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand Command = Con.CreateCommand();
                Command.Transaction = sqlTran;

                try
                {
                    Command.CommandText = CmdText;
                    Command.Parameters.Add(DraftId);
                    Command.ExecuteNonQuery();
                    sqlTran.Commit();
                    /////


                    // Fifo(DraftId.Value.ToString());
                    //////DetailsIdDraft = "";


                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    this.Cursor = Cursors.Default;
                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                }
                this.Cursor = Cursors.Default;
            }

        }

        public static string InsertOutid(string CommandText, string connectionstring, out string outid)
        {
            string commandText = CommandText;
            commandText += "; select @outid=scope_identity()";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlParameter sqlp = new SqlParameter();
                sqlp.Direction = ParameterDirection.Output;
                sqlp.ParameterName = "@outid";
                sqlp.SqlDbType = SqlDbType.Int;
                sqlp.Value = "";
                command.Parameters.Add(sqlp);
                try
                {
                    connection.Open();
                    int cnt = command.ExecuteNonQuery();
                    outid = command.Parameters["@outid"].Value.ToString();
                    return outid;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        string ReceiptId = "0";


        private void ExportRecipt()
        {

            string Id = "";
            table_050_Packaging1BindingSource.EndEdit();
            table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
            ClDoc.ReturnTable(ConPCLOR, @" SELECT  ISNULL(( select CodeCommondity  FROM  dbo.Table_005_TypeCloth where  ID=" +
                mlt_TypeCloth.Value + "),0)as commodity");
            ResidNum = ClDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
            string commandtxt = string.Empty;
            commandtxt = "Declare @Key int";

            commandtxt += @" INSERT INTO Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column15]
                                                                 
                                                                          ) VALUES (" + ResidNum + ",'" + txt_Date_Recipt.Text + "' ," + mlt_Ware_R.Value.ToString() +
                                                                                  "," + mlt_Function_R.Value.ToString() + ","

                                       + mlt_NameCustomer.Value.ToString() + ",'" + "رسید صادره بابت بسته بندی ش" + mlt_Num_Product.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                                       "',getdate()" + ",0); SET @Key=Scope_Identity() ";

            foreach (DataRowView Rows in table_050_Packaging1BindingSource)
            {
                if (Rows["NumberRecipt"].ToString() == "0" && Rows["ReturnFactor"].ToString() == "False")
                {
                    Id = Id + Rows["ID"] + ",";

                    commandtxt += @" INSERT INTO Table_012_Child_PwhrsReceipt (
                                        [column01]
                                       ,[column02]
                                       ,[column03]
                                       ,[column06]
                                       ,[column07]
                                       ,[column10]
                                       ,[column11]
                                       ,[column12]
                                       ,[column15]
                                       ,[column17]
                                       ,[column18]
                                       ,[column20]
                                       ,[column21]
                                       ,[column30]
                                       ,[column34]
                                       ,[column35]
                                       ,[Column36]
                                       ,[Column37]
                                 ) VALUES (@Key," + (((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()) + ",1," + 1 +
                    "," + 1 + ",0,0," + ((DataRowView)mlt_Num_Product.DropDownList.FindItem(mlt_Num_Product.Value))["Number"].ToString() + ",'" + Class_BasicOperation._UserName +
                    "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + Rows["Barcode"] + "," + Rows["weight"] + "," + Rows["weight"] + ",N'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "',N'" + ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "' );  Select @Key";

                }
            }
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand Command = Con.CreateCommand();
                Command.Transaction = sqlTran;

                try
                {
                    Command.CommandText = commandtxt;
                    ReceiptId = Command.ExecuteScalar().ToString();
                    sqlTran.Commit();
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                }


                catch (Exception es)
                {
                    sqlTran.Rollback();
                    this.Cursor = Cursors.Default;
                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                }

                this.Cursor = Cursors.Default;

            }

            if (Id.Length > 0)
            {
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_050_Packaging set NumberRecipt=" + ReceiptId.ToString() + " Where ID in(" + Id.TrimEnd(',') + ")");
            }
        }

        private void txt_weight_KeyPress(object sender, KeyPressEventArgs e)
       {

           try
           {
               if (e.KeyChar == 13)
               {
                   if (mlt_NameCustomer.Text == "" || mlt_Machine.Text == "" || mlt_TypeCloth.Text == "" )
                   {
                       MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد ");
                   }
                   string Number = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"(select isnull ((select Number from Table_035_Production where id =" + mlt_Num_Product.Value + "),0))");
                   if (mlt_Num_Product.Text == Number)
                   {
                       btn_end_Click(sender, e);
                       //if (chek_Return.Checked == true)
                       //{
                       //    ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["ReturnFactor"] = true;
                       //}
                   }
                   else
                   {
                       MessageBox.Show("لطفا شماره کارت تولید را درست وارد نمایید");

                   }

               }
           }
           catch { }
        }

        private void txt_meter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btn_end_Click(sender, e);

            }
        }
        private float FirstRemain(int GoodCode, string Ware)
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
                CommandText = string.Format(CommandText, Ware, GoodCode, ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["date"].ToString());
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        

        private void mlt_Num_Product_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

       

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware, "Column02", "TypeWare");

        }

        private void mlt_Ware_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_R, "Column02", "TypeWare");

        }

        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "Column02", "Column01");

        }

        private void mlt_Function_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_R, "Column02", "Column01");

        }

        private void mlt_NameCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns(mlt_Num_Product, "Id", "Number");
           
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    //Class_BasicOperation.isEnter(e.KeyChar);
                    txt_weight.Focus();

                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    //Class_BasicOperation.isEnter(e.KeyChar);
                txt_weight.Focus();

            }

             //if (e.KeyChar == 13)
             //           {
             //               txt_weight.Focus();
             //           }
        }

        private void txtDateHavale_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Num_Product_KeyUp(object sender, KeyEventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns(mlt_Num_Product, "Column02", "TypeWare");
        }

        private void btn_Del_Product_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 67))
                {
                    IdDraft = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select NumberDraftP from Table_035_Production where ID=" + gridEX2.GetValue("IDProduct").ToString() + "");

                    if (MessageBox.Show("آیا مایل به حذف حواله کارت تولید جاری هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_008_Child_PwhrsDraft where Column01=" + IdDraft);
                        //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_007_PwhrsDraft Where ColumnId=" + IdDraft);
                        //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_035_Production set NumberDraftP=0  Where ID=" + gridEX2.GetValue("IDProduct"));

                        string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_008_Child_PwhrsDraft where Column01 in(
                                                    select NumberDraftP from Table_035_Production where Id = " + gridEX2.GetValue("IDProduct") + @")

                                                    Delete from " + ConWare.Database + @".dbo.Table_007_PwhrsDraft where columnid in(
                                                    select NumberDraftP from Table_035_Production where Id = " + gridEX2.GetValue("IDProduct") + @")

                                                    Update Table_035_Production set NumberDraftP=0  Where ID = " + gridEX2.GetValue("IDProduct") + "";
                       
                        Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);



                        MessageBox.Show("اطلاعات با موفقیت حذف گردید");
                    }
                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void gridEX2_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
             Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
             if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 66))
             {
                 string Draft = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isNull((select NumberDraftP from Table_035_Production where Id=" + mlt_Num_Product.Value.ToString() + "),0)");

                 if (((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" || ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0" || Draft != "0"
                     || ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["FactorReturnId"].ToString() != "0")
                 {
                     e.Cancel = true;
                     MessageBox.Show("این بارکد دارای رسید یا حواله یا فاکتور مرجوعی می باشد امکان حذف آن را ندارید");
                     return;
                 }

                 DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_70_DetailOtherPWHRS.Barcode
                        FROM                    dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                      dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                            WHERE     (dbo.Table_70_DetailOtherPWHRS.Barcode = " + gridEX2.GetValue("Barcode").ToString() + ")");


                 DataTable dtsal = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_011_Child1_SaleFactor.Column34
                            FROM            dbo.Table_010_SaleFactor INNER JOIN
                                                     dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                            WHERE        (dbo.Table_011_Child1_SaleFactor.Column34 = N'" + gridEX2.GetValue("Barcode").ToString() + "')");

                 //                 DataTable dtreturn = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_019_Child1_MarjooiSale.Column32
                 //                        FROM            dbo.Table_018_MarjooiSale INNER JOIN
                 //                                                 dbo.Table_019_Child1_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                 //                        WHERE        (dbo.Table_019_Child1_MarjooiSale.Column32 = N'" + gridEX2.GetValue("Barcode").ToString() + "')");



                 if (dt.Rows.Count > 0)
                 {
                     e.Cancel = true;
                     MessageBox.Show("از این بار کد در فرم ارسال و دریافت بارکد استفاده شده است امکان حذف آن را ندارید");
                     return;

                 }

                 else if (dtsal.Rows.Count > 0)
                 {
                     e.Cancel = true;
                     MessageBox.Show("این بارکد در فاکتور فروش استفاده شده است امکان حذف ان را ندارید ");
                     return;

                 }


               //else  if (dtreturn.Rows.Count > 0)
                 //{
                 //    MessageBox.Show("این بارکد در فاکتور مرجوعی استفاده شده است امکان حذف ان را ندارید ");
                 //    return;

               //}

                 else
                 {

                     e.Cancel = false;

                 }

             }
             else
             {
                 e.Cancel = true;
                 Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
             }
        }

        private void حذفرسیدسالنتولیدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 68))
                {
                    ///چک کردن سند حواله
                    if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", gridEX2.GetValue("NumberDraft").ToString()) != 0)
                    {
                        throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");
                    }

                    //چک کردن سند رسید
                    if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", gridEX2.GetValue("NumberRecipt").ToString()) != 0)
                    {
                        throw new Exception(" رسید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");


                    }
                    else
                    {
                        string NumberRecipt = gridEX2.GetValue("NumberRecipt").ToString();
                        string Message = "آیا مایل به حذف رسید مربوط به این بسته بندی هستید؟";

                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_012_Child_PwhrsReceipt where Column01=" + NumberRecipt);
                            //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_011_PwhrsReceipt Where ColumnId=" + NumberRecipt);
                            //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update table_050_Packaging set NumberRecipt=0 Where NumberRecipt=" + NumberRecipt);


                            string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select NumberRecipt from table_050_Packaging where NumberRecipt in( " + NumberRecipt.TrimEnd(',') + @"))

                                                    Delete from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt where columnid in(
                                                    select NumberRecipt from table_050_Packaging where NumberRecipt in( " + NumberRecipt.TrimEnd(',') + @"))

                                                    Update table_050_Packaging set NumberRecipt=0  where NumberRecipt in( " + NumberRecipt.TrimEnd(',') + ")";
                            Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);





                            MessageBox.Show("اطلاعات با موفقیت حذف گردید");
                        }
                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                    }
                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void txt_Desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_meter.Focus();
            }

        }

        private void txt_meter_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (mlt_NameCustomer.Text == "" || mlt_Machine.Text == "" || mlt_TypeCloth.Text == "")
                    {
                        MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد ");
                    }
                    string Number = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"(select isnull ((select Number from Table_035_Production where id =" + mlt_Num_Product.Value + "),0))");
                    if (mlt_Num_Product.Text == Number)
                    {
                        btn_end_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("لطفا شماره کارت تولید را درست وارد نمایید");

                    }

                }
            }
            catch { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void uiPanel6Container_Click(object sender, EventArgs e)
        {

        }

        private void mlt_Return_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Return, "Column02", "TypeWare");

        }

        private void mlt_TypeRerturn_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_TypeRerturn, "Column02", "Column01");

        }


        private void CheckEssentialItems()
        {

            if (txt_Date_Recipt.Text == "")
                throw new Exception("اطلاعات مربوط به تنظیمات سند را کامل کنید");

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            ClDoc.CheckForValidationDate(txt_Date_Recipt.Text);

            //سند اختتامیه صادر نشده باشد
            ClDoc.CheckExistFinalDoc();



            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            //Person--Center--Project//
            int? Person = null;
            Int16? Center = null;
            Int16? Project = null;
            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();


            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        private void ExportReciptReturn()
        {


            string IdReturn = "";
            string Idfactor = "";
            decimal sumreceipt = 0;
            table_050_Packaging1BindingSource.EndEdit();
            table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
            //ClDoc.ReturnTable(ConPCLOR, @" SELECT  ISNULL(( select CodeCommondity  FROM  dbo.Table_005_TypeCloth where  ID=" +
            //    mlt_TypeCloth.Value + "),0)as commodity");
            //ResidNum = ClDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
            string commandtxt = string.Empty;
            commandtxt = @"Declare @ReceipId int
            Declare @ReceiptNo int
			Declare @ReturnId int 
            Declare @ReturnNum int 
  
                ";

            commandtxt += @"
INSERT INTO Table_011_PwhrsReceipt (  [column01], [column02], [column03], [column04], [column05],[column06], [column08], [column09], [column10], [column11], [column15] ) 
VALUES ((select isnull(max(Column01),0)+1 from Table_011_PwhrsReceipt),'" +
 txt_Date_Recipt.Text + "' ," + mlt_Return.Value.ToString() + "," + mlt_TypeRerturn.Value.ToString() + ","
+ mlt_NameCustomer.Value.ToString() + ",N'" + "رسید صادره بابت مرجوعی بارکدی به شماره کارت تولید " + mlt_Num_Product.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                                       "',getdate()" + ",0); SET @ReceipId=Scope_Identity()  ";

            foreach (DataRowView Rows in table_050_Packaging1BindingSource)
            {
                if (Rows["NumberRecipt"].ToString() == "0" && Rows["ReturnFactor"].ToString() == "True")
                {
                    IdReturn = IdReturn + Rows["ID"] + ",";

                    //sumreceipt += Convert.ToDecimal(Rows["PriceFactor"]);


                    commandtxt += @" INSERT INTO Table_012_Child_PwhrsReceipt (
                                        [column01]
                                       ,[column02]
                                       ,[column03]
                                       ,[column06]
                                       ,[column07]
                                       ,[column10]
                                       ,[column11]
                                       ,[column12]
                                       ,[column15]
                                       ,[column17]
                                       ,[column18]
                                       ,[column20]
                                       ,[column21]
                                       ,[column30]
                                       ,[column34]
                                       ,[column35]
                                       ,[Column36]
                                       ,[Column37]
                                 ) VALUES (@ReceipId," + (((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()) + ",1," + 1 +
                    "," + 1 + ",0 ,0," + ((DataRowView)mlt_Num_Product.DropDownList.FindItem(mlt_Num_Product.Value))["Number"].ToString() + ",'" + Class_BasicOperation._UserName +
                    "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + Rows["Barcode"] + "," + Rows["weight"] + "," + Rows["weight"] + ",N'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "',N'" + ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "' ); ";


                }
            }



            commandtxt += @"
 set @ReceiptNo =(select Column01 from Table_011_PwhrsReceipt where Columnid=@ReceipId )";
//
//

//
//
////Insert INTO " + ConSale.Database + @".dbo.Table_018_MarjooiSale      ( column01, column02, column03, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, column17, Column18, 
////                         Column19, Column20, Column21, Column22,  Column24) 
////						 VALUES((select ISNULL(MAX( Column01),0)+1 from " + ConSale.Database + @" .dbo.Table_018_MarjooiSale),'" + txt_Date_Recipt.Text + "' ," + mlt_NameCustomer.Value + @",0,0,@ReceipId,0,0,0
////						 ,'" + Class_BasicOperation._UserName + "',GETDATE(),'" + Class_BasicOperation._UserName + "',GETDATE()," + sumreceipt + ",0,0,0,0,0,0); SET @ReturnId =SCOPE_IDENTITY()";



//            foreach (DataRowView Items in table_050_Packaging1BindingSource)
//            {
//                if (Items["FactorReturnId"].ToString() == "0" && Items["ReturnFactor"].ToString() == "True")
//                {

//                    Idfactor = Idfactor + Items["FactorReturnId"] + ",";
//                    commandtxt += @"
//
//
//set @ReturnNum=(select Column01 from " + ConSale.Database + @".dbo.Table_018_MarjooiSale where Columnid=@ReturnId)
//
//
//INSERT INTO " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale  (column01, column02, column03, column04, column05, column06, 
//column07, column08, column09, column10,Column11, column15, column16, column17, column18, column19, 
//                         column20, Column23, column24, column25, column26, column27, column28, column29, Column30, Column31,Column32, Column34, Column35, Column36, Column37)
//
// VALUES (@ReturnId," + (((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()) + "," + (((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["vahedshomaresh"].ToString()) +
//                     ",0,0,1,1,0,0,0,0,0,0,0,0,0,0 ,'" + Items["Description"].ToString() + "',0,0,@ReceipId,0,0,@ReceiptNo,0,0," + Items["Barcode"].ToString() + "," + Items["weight"] + "," + Items["weight"] + ",'" +
//                     ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "','"
//                              + ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "')";
//                }

//            }


            commandtxt += "Update " + ConPCLOR.Database + @".dbo.Table_050_Packaging set NumberRecipt=@ReceipId   where Id in(" + IdReturn.TrimEnd(',') + @")";

            commandtxt += " select @ReceipId as ReceipId,@ReceiptNo as ReciptNo,@ReturnNum as ReturnNum ";

             dtReturn = Class_BasicOperation.SqlTransactionMethod(ConWare.ConnectionString, commandtxt);
            gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
            gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

        

        }

        private void btn_Delete_FactorReturn_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            string NumberRecipt = gridEX2.GetValue("NumberRecipt").ToString();
            string ID = gridEX2.GetValue("ID").ToString();
            string IDsanad = gridEX2.GetValue("SanadID").ToString();
            string Return = gridEX2.GetValue("FactorReturnId").ToString();

            DataTable Table = new DataTable();
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 134))
                {
                    if (this.table_050_Packaging1BindingSource.Count > 0)
                    {

                        int RowID = int.Parse(((DataRowView)this.table_050_Packaging1BindingSource.CurrencyManager.Current)["Id"].ToString());

                        string Returnid = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select FactorReturnId from Table_050_Packaging where id=" + RowID.ToString() + "");

                        string Message = "آیا مایل به حذف فاکتور مرجوعی مربوط به این بسته بندی هستید؟";

                        if (IDsanad != "0")
                        {
                            Message = "برای این فاکتور، رسید انبار و فاکتور مرجوعی نیز صادر شده است. در صورت تأیید ثبت مربوط به رسید و فاکتور مرجوعی نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            if (Returnid.ToString() != "0")
                            {


                                command += @"Delete From "+ConSale.Database+".dbo.Table_019_Child1_MarjooiSale Where Column01 in(" + Return.TrimEnd(',') + @");
                                Delete From " + ConSale.Database + ".dbo.Table_018_MarjooiSale Where ColumnId in (" + Return.TrimEnd(',') + @");
                                Update table_050_Packaging set FactorReturnId=0 Where FactorReturnId in(" + Return.TrimEnd(',')+")";



                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PCLOR))
                                {
                                    Con.Open();

                                    SqlTransaction sqlTran = Con.BeginTransaction();
                                    SqlCommand Command = Con.CreateCommand();
                                    Command.Transaction = sqlTran;
                                    try
                                    {
                                        Command.CommandText = command;
                                        Command.ExecuteNonQuery();
                                        sqlTran.Commit();
                                        Class_BasicOperation.ShowMsg("", "حذف  با موفقیت صورت گرفت", "Information");


                                    }
                                    catch (Exception es)
                                    {
                                        sqlTran.Rollback();
                                        this.Cursor = Cursors.Default;
                                        Class_BasicOperation.CheckExceptionType(es, this.Name);
                                    }
                                    this.Cursor = Cursors.Default;

                                    mlt_Num_Product_ValueChanged(sender, e);

                                }
                            }

                            else
                            {
                                MessageBox.Show("به دلیل نداشتن فاکتور امکان حذف وجود ندارد");
                            }
                        }

                        //}


                        //dataSet_05_PCLOR.EnforceConstraints = false;
                        //this.table_050_Packaging1TableAdapter.FillByID(dataSet_05_PCLOR.Table_050_Packaging1, RowID);
                        //dataSet_05_PCLOR.EnforceConstraints = true;
                        //DS.Tables["Doc"].Clear();
                        //DocAdapter.Fill(DS, "Doc");
                        //this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    }
                }

                else

                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
//            mlt_Num_Product.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT * FROM  ( SELECT tp.ID, tp.Number ,t30.NumberOrder-ISNULL((SELECT sum(NumberProduct) FROM Table_035_Production WHERE ColorOrderId= t30.Id ),0) AS Remain
//											FROM Table_030_DetailOrderColor  AS t30
//											INNER JOIN Table_035_Production tp ON tp.ColorOrderId=t30.ID) AS T  ORDER BY Number DESC");

            //mlt_Num_Product.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,Number from Table_035_Production ORDER BY Number DESC");

            Frm_35_packing_Load(sender, e);

        }

        private void mlt_Num_Product_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void ch_Auto_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_CountProduct_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
