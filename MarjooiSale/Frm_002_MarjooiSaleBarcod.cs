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

namespace PCLOR.MarjooiSale
{
    public partial class Frm_002_MarjooiSaleBarcod : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);
        
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
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
        string Idsanad = "";


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

       


        public Frm_002_MarjooiSaleBarcod()
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


        
        private void Frm_002_MarjooiSaleBarcod_Load(object sender, EventArgs e)
        {

            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
           
            mlt_Num_Product.Focus();
            uiPanel0.TabIndex = 2;
            try
            {
                //txtDateHavale.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                txt_Date_Recipt.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");

                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

                mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");

                mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT        dbo.Table_005_TypeCloth.ID, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_005_TypeCloth.CodeCommondity, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS vahedshomaresh
                    FROM            dbo.Table_005_TypeCloth INNER JOIN
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON dbo.Table_005_TypeCloth.CodeCommondity = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid");

               
                mlt_NameCustomer.DataSource = ClDoc.ReturnTable(ConBase, @"Select Columnid ,Column01,Column02 from Table_045_PersonInfo  WHERE
                                                              'True'='" + isadmin.ToString() + @"'  or  column133 in (select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"')");
                mlt_TypeColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeColor from Table_010_TypeColor");
                mlt_CodeOrderColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_035_Production.Number AS NumberOrder, dbo.Table_025_HederOrderColor.Number, dbo.Table_010_TypeColor.TypeColor, 
                      dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_035_Production.ID
                      FROM         dbo.Table_025_HederOrderColor INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID INNER JOIN
                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId");

//                mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
//                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
//                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
//                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 2)");



//                mlt_Ware_R.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT        dbo.Table_001_PWHRS.columnid, dbo.Table_001_PWHRS.column01, dbo.Table_001_PWHRS.column02, "+ConPCLOR.Database+ @".dbo.Table_90_Wares.TypeWare
//FROM            dbo.Table_001_PWHRS INNER JOIN
//                         "+ConPCLOR.Database+ @".dbo.Table_90_Wares ON dbo.Table_001_PWHRS.columnid = "+ConPCLOR.Database+ @".dbo.Table_90_Wares.IdWare
//WHERE        ('True' = '" + isadmin.ToString() + @"')  OR
//                         (dbo.Table_001_PWHRS.columnid IN
//                             (SELECT        Ware
//                               FROM            "+ConPCLOR.Database+ @".dbo.Table_95_DetailWare
//                               WHERE        (Fk IN (SELECT        Column133 FROM            "+ ConBase.Database + @".dbo.Table_045_PersonInfo WHERE        (Column23 = N'" + Class_BasicOperation._UserName + @"')))))");

                DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                    @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");



                mlt_Ware_R.DataSource = WareTable;


                //mlt_Function.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_R.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from Table_007_PwhrsDraft");
                gridEX2.DropDowns["NumProduct"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,Number from Table_035_Production ");
                gridEX2.DropDowns["sanadid"].DataSource = ClDoc.ReturnTable(ConACNT, @"select Columnid,Column00 from Table_060_SanadHead ");
                gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

                mlt_Num_Product.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT * FROM  ( SELECT tp.ID, tp.Number ,t30.NumberOrder-ISNULL((SELECT sum(NumberProduct) FROM Table_035_Production WHERE ColorOrderId= t30.Id ),0) AS Remain
											FROM Table_030_DetailOrderColor  AS t30
											INNER JOIN Table_035_Production tp ON tp.ColorOrderId=t30.ID) AS T  ORDER BY Number DESC");

                //mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                //mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=4");

                //mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=25");

                mlt_Function_R.Value = Properties.Settings.Default.Returnfactorfunction;
                mlt_Ware_R.Value = Properties.Settings.Default.Returnfactor;

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

        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                if (mlt_Num_Product.Text == "")
                {
                    MessageBox.Show("اطلاعات مورد نظر را تکمیل نمایید");
                }

                else
                {


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
                        //mlt_NameCustomer.Value = dt.Rows[0][0].ToString();
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
                        if (isadmin)
                        {
                            this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                        }
                        else
                        {
                            this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                        }
                    }
                }
            }

            catch
            { }

        }
        private void Product()
        {

            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

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
                if (isadmin)
                {
                    this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                }
                else
                {
                    this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                }






            }
        }
            
            
            
            
            
            
        private   void mlt_Ware_R_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Ware_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_R, "Column02", "TypeWare");
        }

        private void mlt_Function_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_R, "Column02", "Column01");

        }

        private void mlt_Function_R_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_weight_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if ( mlt_Machine.Text == "" || mlt_TypeCloth.Text == "" || mlt_TypeColor.Text == "" || Txt_Price.Text == "" )
                    {
                        MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد ");
                        return;
                    }

                    if ( Txt_Price.Text == "" || Txt_Price.Text=="0")
                    {
                        MessageBox.Show("لطفا قیمت طاقه را وارد نمایید");
                        return;
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
        private void btn_end_Click(object sender, EventArgs e)
        {
            try
            {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                if (mlt_Num_Product.Text == "")
                {
                    Class_BasicOperation.ShowMsg("", "لطفا شماره کارت تولید را وارد کنید", Class_BasicOperation.MessageType.None);
                }
                else
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    //if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 53))
                    //{
                        try
                        {


                            //   btn_New.Enabled = false;
                            rb_Auto.Enabled = true;
                            ch_Auto.Enabled = true;
                            rb_select.Enabled = true;
                        }

                        catch { }

                        //////////////

                        if (txt_weight.Text == "" || txt_weight.Text == "0" || txt_meter.Text == "" || Txt_Price.Text == "" || mlt_NameCustomer.Text == "" || mlt_NameCustomer.Text=="0")
                        {
                            MessageBox.Show("اطلاعات را تکمیل نمایید");
                            return;
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
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["type"] = 1;
                                ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["PriceFactor"] = Txt_Price.Text;
                                Properties.Settings.Default.Returnfactor = mlt_Ware_R.Value.ToString();
                                Properties.Settings.Default.Save();
                                
                                table_050_Packaging1BindingSource.EndEdit();
                                table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
                              

                                // btn_New.Enabled = true;
                                //  btn_New_Click(sender, e);
                                //      button1_Click(sender, e);

                                string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
                        FROM         dbo.Table_035_Production  left JOIN
                                  dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
                        
                        where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
                                GROUP BY dbo.Table_035_Production.weight");
                                txt_Rem_Weight.Text = sum;
                                if (isadmin)
                                {
                                    this.table_050_Packaging1TableAdapter.FillByProductID (this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
                                    
                                }
                                else
                                {
                                    this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                                }
                                txt_weight.Text = "0";
                                txt_meter.Text = "0";
                                Txt_Price.Text = "0";
                                txt_Desc.Text = "م/";

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
                    //}
                    //else
                    //{

                    //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                    //}
                }

            }

            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
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



        private void btn_R_D_Click(object sender, EventArgs e)
        {
            try
            {


                {

                    if (mlt_Num_Product.Text != "")
                    {


                        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 130))
                        {
                            if (gridEX2.RowCount > 0)
                            {


                                if (mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit))
                                {
                                    MessageBox.Show("اطلاعات وارد شده مربوط به سند معتبر نمی باشد");
                                    return;
                                }
                                if (mlt_NameCustomer.Text=="")
                                {
                                    MessageBox.Show("شخص را وارد نمایید ");
                                    return;
                                }
                             
                                        finalmarjoei();
                                        Properties.Settings.Default.Returnfactor = mlt_Ware_R.Value.ToString();
                                        Properties.Settings.Default.Save();
                                        Properties.Settings.Default.Returnfactorfunction = mlt_Function_R.Value.ToString();
                                        Properties.Settings.Default.Save();
                                 
                                    
                            }   
                            else
                            {
                                MessageBox.Show("رکوردی برای ثبت نهایی وجود ندارد");
                                return;
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
        string IdRecipt = "";
        string sanadid = "";
        private void finalmarjoei()
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            string Ids = "";
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
            {
                if (item.Cells["NumberRecipt"].Value.ToString() == "0")
                {
                    Ids +=item.Cells["ID"].Value + ",";
                   
                }
               
            }
            //DataTable dtpwhrs = ClDoc.ReturnTable(ConWare, @"select * from Table_011_PwhrsReceipt where ColumnId in ("+Id.TrimEnd(',')+")");
          
            if (Ids != "")
            {



                ExportRecipt();
                //PrepareTable();
                //ExportSanad();

                //MarjooiSale.Form33_ExportDocForReceipt frm = new MarjooiSale.Form33_ExportDocForReceipt((ReceiptId.TrimEnd(',')));

                //if (frm.ShowDialog() == DialogResult.OK)
                //{
                //Class_BasicOperation.ShowMsg("", "ثبت سند حسابداری با شماره " + DocNum.Value + " با موفقیت صورت گرفت", "Information");

                 

                    //sanadid = ClDoc.ExScalar(ConWare.ConnectionString, @"SELECT     column07  FROM      dbo.Table_011_PwhrsReceipt  WHERE   (column01 =" + ResidNum + ") ");

                    //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_050_Packaging set SanadID=" + sanadid + " Where ID in(" + Id.TrimEnd(',') + ")");

                    ////ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_050_Packaging set SanadID=" + sanad + " Where ID in(" + Id.TrimEnd(',') + ")");
                gridEX2.DropDowns["sanadid"].DataSource = ClDoc.ReturnTable(ConACNT, @"select Columnid,Column00 from Table_060_SanadHead ");
                gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

                    if (isadmin)
                    {
                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                    }
                    else
                    {
                        this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                    }
               
                    
                if (txt_ID.Text != "")
                {
                    if (isadmin)
                    {
                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                    }
                    else
                    {
                        this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                    }
                }

                //foreach (DataRowView item in table_050_Packaging1BindingSource)
                //{
                //    if (item["Type"].ToString() == "True")
                //    {

                //        IdRecipt = IdRecipt + item["NumberRecipt"] + ",";

                //    }
                //}


                ReceiptId = "0";
            //}
                if (isadmin)
                {
                    this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                }
                else
                {
                    this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                }
            }
        
        }
        DataTable HeaderTable = new DataTable();
        DataRow ReceiptRow;
        DataTable SourceTable = new DataTable();
        string ReceiptId = "0";
        DataTable BedTable = new DataTable();
        DataTable BesTable = new DataTable();
        DataTable FunctionTable = new DataTable();
        DataTable Alldt = new DataTable();
        DataTable table = new DataTable();

      
        
        private void PrepareTable()
        {
            try
            {
                HeaderTable = ClDoc.ReturnTable(ConWare, "Select * from Table_011_PwhrsReceipt where Columnid in(" + ReceiptId.TrimEnd(',') + ")");
                ReceiptRow = HeaderTable.Rows[0];

                FunctionTable = ClDoc.ReturnTable(ConWare, "Select Column14,Column08 from table_005_PwhrsOperation where ColumnId=" +
                   ReceiptRow["Column04"].ToString());

                string Bed = (FunctionTable.Rows[0]["Column08"].ToString().Trim() == "" ? "NULL" : FunctionTable.Rows[0]["Column08"].ToString());
                string Bes = (FunctionTable.Rows[0]["Column14"].ToString().Trim() == "" ? "NULL" : FunctionTable.Rows[0]["Column14"].ToString());
                SqlDataAdapter Adapter = new SqlDataAdapter();




                SourceTable.Columns.Add("Type", Type.GetType("System.Int16"));
                SourceTable.Columns.Add("Column01", Type.GetType("System.String"));
                SourceTable.Columns.Add("Column001", Type.GetType("System.String"));
                SourceTable.Columns.Add("Column07", Type.GetType("System.Int32"));
                SourceTable.Columns["Column07"].AllowDBNull = true;
                SourceTable.Columns.Add("Column08", Type.GetType("System.Int16"));
                SourceTable.Columns["Column08"].AllowDBNull = true;
                SourceTable.Columns.Add("Column09", Type.GetType("System.Int16"));
                SourceTable.Columns["Column09"].AllowDBNull = true;
                SourceTable.Columns.Add("Column10", Type.GetType("System.String"));
                SourceTable.Columns.Add("Column11", Type.GetType("System.Double"));
                SourceTable.Columns.Add("Column12", Type.GetType("System.Double"));
                //ارزش ارز
                SourceTable.Columns.Add("Column13", Type.GetType("System.Double"));
                ////نوع ارز
                SourceTable.Columns.Add("Column14", Type.GetType("System.Int16"));
                SourceTable.Rows.Clear();


                Adapter = new SqlDataAdapter(@"SELECT     column01 AS HeaderID, column13 AS Center, column14 AS Project, SUM(Column21) AS TotalPrice,
                Column25 as CurType,Column26 as CurValue
                FROM         Table_012_Child_PwhrsReceipt WHERE     (column01 = " + ReceiptId.TrimEnd(',') +
                  ") GROUP BY column01, column13, column14,Column25,Column26", ConWare);


                Adapter.Fill(BedTable);

                //درج سطرهای بدهکار
                foreach (DataRow item in BedTable.Rows)
                {


                    SourceTable.Rows.Add(12, Bed, Bed, DBNull.Value,
                        (item["Center"].ToString().Trim() == "" ? (object)DBNull.Value : item["Center"].ToString()),
                        (item["Project"].ToString().Trim() == "" ? (object)DBNull.Value : item["Project"].ToString()),
                         "رسید شماره " + ReceiptRow["Column01"].ToString() + " به تاریخ " + ReceiptRow["Column02"].ToString() + " شماره عطف " + ReceiptRow["columnid"].ToString(),
                         Convert.ToDouble(item["TotalPrice"].ToString()), 0,
                         Convert.ToDouble(item["CurValue"].ToString()),
                         (item["CurType"].ToString().Trim() == "" ? (object)null : item["CurType"].ToString()));
                }

                //بستانکاران
                bool _InternalUse = bool.Parse(ClDoc.ExScalar(Properties.Settings.Default.PWHRS, "table_005_PwhrsOperation", "Column18", "Columnid", ReceiptRow["Column04"].ToString()).ToString());

                BesTable = ClDoc.ReturnTable(ConWare, @"Select Column01 AS HeaderID, SUM(Column21) AS TotalPrice,Column25 as CurType,Column26 as CurValue
               FROM         Table_012_Child_PwhrsReceipt WHERE     (column01 = " + ReceiptRow["ColumnId"].ToString() + ") GROUP BY column01,Column25,Column26");

                //اگر عملکرد مصرف داخلی باشد شخصی بستانکار نمی شود
                //در غیر این صورت می شود
                if (!_InternalUse)
                {
                    foreach (DataRow item in BesTable.Rows)
                    {

                        SourceTable.Rows.Add(12, Bes, Bes, (ReceiptRow["Column05"].ToString().Trim() != "" ? ReceiptRow["Column05"].ToString().Trim() : (object)DBNull.Value),
                            DBNull.Value, DBNull.Value, "رسید شماره " + ReceiptRow["Column01"].ToString() + " به تاریخ " + ReceiptRow["Column02"].ToString() + " شماره عطف " + ReceiptRow["columnid"].ToString(),
                            0, Convert.ToDouble(item["TotalPrice"].ToString()), Convert.ToDouble(item["CurValue"].ToString()),
                                 (item["CurType"].ToString().Trim() == "" ? (object)null : item["CurType"].ToString()));
                    }
                }
                else
                {
                    foreach (DataRow item in BesTable.Rows)
                    {

                        SourceTable.Rows.Add(12, Bes, Bes, DBNull.Value,
                            DBNull.Value, DBNull.Value, "رسید شماره " + ReceiptRow["Column01"].ToString() + " به تاریخ " + ReceiptRow["Column02"].ToString() + " شماره عطف " + ReceiptRow["columnid"].ToString(),
                            0, Convert.ToDouble(item["TotalPrice"].ToString()), Convert.ToDouble(item["CurValue"].ToString()),
                                 (item["CurType"].ToString().Trim() == "" ? (object)null : item["CurType"].ToString()));
                    }
                }

                Alldt = ClDoc.ReturnTable(ConWare, @"SELECT     column01 AS HeaderIDBed, column13 AS CenterBed, column14 AS ProjectBed, SUM(Column21) AS TotalPriceBed,
                Column25 as CurTypeBed,Column26 as CurValueBed
                FROM         Table_012_Child_PwhrsReceipt WHERE     (column01 = " + ReceiptId.TrimEnd(',') + @")
			    GROUP BY column01, column13, column14,Column25,Column26
			 Union all
			 Select Column01 AS HeaderIDBes,column13 AS CenterBes, column14 AS ProjectBes,
			  SUM(Column21) AS TotalPriceBes,Column25 as CurTypeBes,Column26 as CurValueBes
               FROM         Table_012_Child_PwhrsReceipt WHERE     (column01 = " + ReceiptId.TrimEnd(',') + @")
			    GROUP BY column01, column13, column14,Column25,Column26");

            }
            catch { }
        }


        private void CheckEssentialItems()
        {

            //if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", ReceiptId.TrimEnd(',').ToString()) != 0)
            //    throw new Exception("برای این رسید سند صادر شده است");

            //if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column15", ReceiptId.TrimEnd(',').ToString()) != 0)
            //    throw new Exception("به علت دارا بودن کارت تولید، صدور سند حسابداری امکان پذیر نمی باشد");

            //if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column13", ReceiptId.TrimEnd(',').ToString()) != 0)
            //    throw new Exception("برای این رسید، فاکتور خرید صادر شده است" +
            //            Environment.NewLine + "جهت صدور سند از فرم فاکتور خرید مربوط اقدام نمایید");

            //if (Convert.ToDouble(gridEX2.GetTotalRow().Cells["PriceFactor"].Value.ToString()) == 0)
            //    throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            //*****Check Essential Information***//
            if (txt_Date_Recipt.Text == "")
                throw new Exception("اطلاعات مربوط به تنظیمات سند را کامل کنید");

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            ClDoc.CheckForValidationDate(txt_Date_Recipt.Text);

            //سند اختتامیه صادر نشده باشد
            ClDoc.CheckExistFinalDoc();

            //foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            //{
            //    if (item.Cells["Column01"].Text.Trim() == "" || item.Cells["Column01"].Text.ToString().All(char.IsDigit))
            //        throw new Exception("سرفصل حساب را مشخص کنید");
            //}


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

            //foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            //{
            //    Person = null;
            //    Center = null;
            //    Project = null;
            //    if (item.Cells["Column07"].Text.Trim() != "")
            //        Person = int.Parse(item.Cells["Column07"].Value.ToString());

            //    if (item.Cells["Column08"].Text.Trim() != "")
            //        Center = Int16.Parse(item.Cells["Column08"].Value.ToString());

            //    if (item.Cells["Column09"].Text.Trim() != "")
            //        Project = Int16.Parse(item.Cells["Column09"].Value.ToString());

            //    clCredit.All_Controls_2(item.Cells["Column01"].Value.ToString().Trim(), Person, Center, Project);

            //    //**********Check Person Credit************//
            //    if (item.Cells["Column07"].Text.Trim() != "")
            //    {
            //        if (item.Cells["Column07"].Text.Trim() != "")
            //            TPerson.Rows.Add(Int32.Parse(item.Cells["Column07"].Value.ToString()), item.Cells["Column01"].Value.ToString()
            //                , (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
            //                Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
            //    }
            //    //**********Check Account's nature****//
            //    TAccounts.Rows.Add(item.Cells["Column01"].Value.ToString(), (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
            //                Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
            //}

            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        private void ExportRecipt()
        {

        
            string Id = "";
            string Idfactor="";
            decimal sumreceipt = 0;
            table_050_Packaging1BindingSource.EndEdit();
            table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
            //ClDoc.ReturnTable(ConPCLOR, @" SELECT  ISNULL(( select CodeCommondity  FROM  dbo.Table_005_TypeCloth where  ID=" +
            //    mlt_TypeCloth.Value + "),0)as commodity");
            //ResidNum = ClDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
            string commandtxt = string.Empty;
            commandtxt = @"Declare @ReceipId int
            Declare @DocId int
		
            Declare @ReceiptNo int
	        Declare @DocNo int 
			Declare @ReturnId int 
            Declare @ReturnNum int 
                ";
          
            commandtxt += @"

INSERT INTO Table_011_PwhrsReceipt (  [column01], [column02], [column03], [column04], [column05],[column06], [column08], [column09], [column10], [column11], [column15] ) 
VALUES ((select isnull(max(Column01),0)+1 from Table_011_PwhrsReceipt),'" +
 txt_Date_Recipt.Text + "' ," + mlt_Ware_R.Value.ToString() +"," + mlt_Function_R.Value.ToString() + ","
+ mlt_NameCustomer.Value.ToString() + ",N'" + "رسید صادره بابت مرجوعی بارکدی به شماره کارت تولید " + mlt_Num_Product.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                                       "',getdate()" + ",0); SET @ReceipId=Scope_Identity()  ";

            foreach (DataRowView Rows in table_050_Packaging1BindingSource)
            {
                if (Rows["NumberRecipt"].ToString() == "0")
                {
                    Id = Id + Rows["ID"] + ",";
                       
                    sumreceipt += Convert.ToDecimal(Rows["PriceFactor"]);


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
                    "," + 1 + "," + Rows["PriceFactor"] + "," + Rows["PriceFactor"] + "," + ((DataRowView)mlt_Num_Product.DropDownList.FindItem(mlt_Num_Product.Value))["Number"].ToString() + ",'" + Class_BasicOperation._UserName +
                    "','" + Class_BasicOperation._UserName + "',getdate()," + Rows["PriceFactor"] + "," + (Convert.ToDecimal(Rows["PriceFactor"]) * Convert.ToDecimal(Rows["weight"])).ToString() + "," + Rows["Barcode"] + "," + Rows["weight"] + "," + Rows["weight"] + ",N'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "',N'" + ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "' ); ";


                }
            }

            if (sumreceipt == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

    CheckEssentialItems();





    commandtxt += @"  Insert INTO " + ConSale.Database + @".dbo.Table_018_MarjooiSale      ( column01, column02, column03, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, column17, Column18, 
                         Column19, Column20, Column21, Column22,  Column24) 
						 VALUES((select ISNULL(MAX( Column01),0)+1 from " + ConSale.Database + @" .dbo.Table_018_MarjooiSale),'" + txt_Date_Recipt.Text + "' ," + mlt_NameCustomer.Value + @",0,0,@ReceipId,0,0,0
						 ,'" + Class_BasicOperation._UserName + "',GETDATE(),'" + Class_BasicOperation._UserName + "',GETDATE()," + sumreceipt + ",0,0,0,0,0,0); SET @ReturnId =SCOPE_IDENTITY()";



    foreach (DataRowView Items in table_050_Packaging1BindingSource)
    {
        if (Items["FactorReturnId"].ToString() == "0")
        {

            Idfactor = Idfactor + Items["FactorReturnId"] + ",";
            commandtxt += @"INSERT INTO " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale  (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10,Column11, column15, column16, column17, column18, column19, 
                         column20, Column23, column24, column25, column26, column27, column28, column29, Column30, Column31,Column32, Column34, Column35, Column36, Column37)

 VALUES (@ReturnId," + (((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString()) + "," + (((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["vahedshomaresh"].ToString()) + ",0,0,1,1,0,0," + Items["PriceFactor"] + "," + (Convert.ToDecimal(Items["PriceFactor"]) * Convert.ToDecimal(Items["weight"])).ToString() +
                         ",0,0,0,0,0," + (Convert.ToDecimal(Items["PriceFactor"]) * Convert.ToDecimal(Items["weight"])).ToString() + ",'" + Items["Description"].ToString() + "',0,0,@ReceipId,0,0,0,0,0," + Items["Barcode"].ToString() + "," + Items["weight"] + "," + Items["weight"] + ",'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "','"
                      + ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "')";
        }

    }



    commandtxt += @"INSERT INTO  " + ConACNT.Database + @".dbo.Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select isnull(Max(Column00),0)+1 from  "+ConACNT.Database+@".dbo.Table_060_SanadHead),
				'" + txt_Date_Recipt.Text + "',2,0,'" + "گردش انبار- صدور سند مرجوعی" + "','" + Class_BasicOperation._UserName +
 "',getdate()); SET @DocId=SCOPE_IDENTITY()";


 

    
            commandtxt += @"


set @ReceiptNo =(select Column01 from Table_011_PwhrsReceipt where Columnid=@ReceipId )

 set @DocNo =(select Column00 from " + ConACNT.Database + @".dbo.Table_060_SanadHead where columnid=@DocId)
  
set @ReturnNum=(select Column01 from " + ConSale.Database + @".dbo.Table_018_MarjooiSale where Columnid=@ReturnId)

INSERT INTO " + ConACNT.Database + @".dbo.Table_065_SanadDetail  ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22] )



 select @DocId,AccCode,GroupCode,KolCode,MoeinCode,TafsiliCode,JozCode,Person,Center,Project,

N'رسید شماره '+ cast(@ReceiptNo as nvarchar) + N' به تاریخ '+'" + txt_Date_Recipt.Text + @"'+N' شماره عطف '+cast(@ReceipId as nvarchar) + N'شماره کارت تولید'+'"+mlt_Num_Product.Text+@"' Sharh 

 ,Bed,Bes,  0,0,  -1, 12,@ReceipId ,'" + Class_BasicOperation._UserName + @"',
                                                    getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),0 from (
select 
(Select Column08 from table_005_PwhrsOperation where ColumnId=r.Column04) AccCode,Null Person,Null Center,Null Project ,
SUM(rchild.column21) Bed,0 Bes

from Table_011_PwhrsReceipt as r
left join  Table_012_Child_PwhrsReceipt as rchild on r.columnid=rchild.column01

 where r.columnid=@ReceipId
 group by r.Column05 ,rchild.Column13 ,rchild.Column09 ,r.Column04

 union 

 select 

 (Select Column14 from table_005_PwhrsOperation where ColumnId=r.Column04) AccCode,r.Column05 Person,Null Center,Null Project ,
0 Bed,SUM(rchild.column21) Bes

from Table_011_PwhrsReceipt as r
left join  Table_012_Child_PwhrsReceipt as rchild on r.columnid=rchild.column01
 where r.columnid=@ReceipId
 group by r.Column05 ,rchild.Column13 ,rchild.Column09 ,r.Column04

 ) as t
 left join " + ConACNT.Database + @".dbo.AllHeaders() as h on h.acc_code  collate database_default  =t.AccCode ";



            commandtxt += "Update " + ConPCLOR.Database + @".dbo.Table_050_Packaging set NumberRecipt=@ReceipId , SanadID=@DocId ,FactorReturnId=@ReturnId  where Id in(" + Id.TrimEnd(',') + @")
           Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@DocId  where ColumnId in (@ReturnId)  Update " + ConSale.Database + ".dbo.Table_019_Child1_MarjooiSale set Column29=@ReceiptNo where Column01 in(@ReturnId)";

            commandtxt += " select @DocId as DocId ,@DocNo as DocNo,@ReceipId as ReceipId,@ReceiptNo as ReciptNo,@ReturnNum as ReturnNum ";

        DataTable dt=Class_BasicOperation.SqlTransactionMethod(ConWare.ConnectionString, commandtxt);
        gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
        gridEX2.DropDowns["Factor"].DataSource = ClDoc.ReturnTable(ConSale, @"select Columnid,Column01 from Table_018_MarjooiSale ");

        ToastNotification.Show(this, "رسید انبار بسته بندی به شماره  " + dt.Rows[0][3].ToString() + " با موفقیت صادر شد " + Environment.NewLine +
                        "ثبت سند حسابداری با شماره" + dt.Rows[0][1].ToString() + " با موفقیت صادر شد"+ Environment.NewLine +"ثبت فاکتور مرجوعی با شماره"+dt.Rows[0][4].ToString()+"باموفقیت ثبت شد", 7000, eToastPosition.MiddleCenter);
      
        }

        SqlParameter DocNum;
        string sanad = "0";
        private void ExportSanad()
        {
            try
            {
               
                DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                DocNum.Direction = ParameterDirection.Output;
                string command = "declare @Key int ";
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    command += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + txt_Date_Recipt.Text + "',2,0,'" + "گردش انبار- صدور سند مرجوعی" + "','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @Key=SCOPE_IDENTITY()";
                    
                        foreach (DataRow item in SourceTable.Rows)
                        {
                        //foreach (DataRowView Rows in table_050_Packaging1BindingSource)
                        //    {
                            //if (Convert.ToDouble(item[0].ToString()) > 0 ||
                            //           Convert.ToDouble(item[0].ToString()) > 0)
                            //{
                                //if (Rows["SanadID"].ToString() == "0")
                                //{
                                //    Idsanad = Idsanad + Rows["ID"] + ",";
                                    string[] _AccInfo = ClDoc.ACC_Info(item["Column01"].ToString());

                                    command += @"INSERT INTO Table_065_SanadDetail  ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) VALUES(@Key,
                                                    '" + item["Column01"].ToString() + @"',
                                                    " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                                     " + ((_AccInfo[1] != null && !string.IsNullOrWhiteSpace(_AccInfo[1].ToString())) ? "'" + _AccInfo[1].ToString() + "'" : "NULL") + @" ,
                                                    " + ((_AccInfo[2] != null && !string.IsNullOrWhiteSpace(_AccInfo[2].ToString())) ? "'" + _AccInfo[2].ToString() + "'" : "NULL") + @",
                                                    " + ((_AccInfo[3] != null && !string.IsNullOrWhiteSpace(_AccInfo[3].ToString())) ? "'" + _AccInfo[3].ToString() + "'" : "NULL") + @",
                                                    " + ((_AccInfo[4] != null && !string.IsNullOrWhiteSpace(_AccInfo[4].ToString())) ? "'" + _AccInfo[4].ToString() + "'" : "NULL") + @",
                                                    " + (item["Column07"] != null && !string.IsNullOrWhiteSpace(item["Column07"].ToString()) ? item["Column07"].ToString() : "NULL") + @",
                                                    " + (item["Column08"] != null && !string.IsNullOrWhiteSpace(item["Column08"].ToString()) ? item["Column08"].ToString() : "NULL") + @",
                                                    " + (item["Column09"] != null && !string.IsNullOrWhiteSpace(item["Column09"].ToString()) ? item["Column09"].ToString() : "NULL") + @",
                                                    '" + item["Column10"] + @"',
                                                    " + ( Convert.ToInt64(Convert.ToDouble(item["Column11"].ToString()))) + @",
                                                    " + ( Convert.ToInt64(Convert.ToDouble(item["Column12"].ToString()))) + @",
                                                    0,
                                                    0,
                                                    -1,
                                                    12,
                                                    " + int.Parse(ReceiptRow["ColumnId"].ToString()) + @",
                                                    '" + Class_BasicOperation._UserName + @"',
                                                    getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0)";
                                //}
                            //}
                        //}
                        }

                    command += "UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt Set Column19=1 , Column07=@Key where Columnid=" + ReceiptId.TrimEnd(',');
                    //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt Set Column19=1 , Column07=" + DocID + " where Columnid=" + _ResidId);

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
                    {
                        Con.Open();
                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = command;
                            Command.Parameters.Add(DocNum);
                          sanad= Command.ExecuteNonQuery().ToString();
                            sqlTran.Commit();
                            //Class_BasicOperation.ShowMsg("", "ثبت سند حسابداری با شماره " + DocNum.Value + " با موفقیت صورت گرفت", "Information");
                            //bt_ExportDoc.Enabled = false;
                            //this.DialogResult = DialogResult.Yes;
                            this.DialogResult = DialogResult.OK;
                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;



                    }
                    if (Idsanad.Length > 0)
                    {
                        ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_050_Packaging set SanadID=" + sanad + " Where ID in(" + Id.TrimEnd(',') + ")");
                    }
                }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void mlt_Num_Product_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void mlt_NameCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                  txt_weight.Focus();
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                txt_weight.Focus();

            }
      
        }

        private void mlt_CodeOrderColor_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_TypeCloth_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_TypeColor_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_NameCustomer_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void txt_Desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_meter.Focus();
               
            }
        }

        private void Txt_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
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

        private void txt_weight_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                //Class_BasicOperation.isEnter(e.KeyChar);
                Txt_Price.Focus();
            }
        }

        private void rb_Auto_CheckedChanged(object sender, EventArgs e)
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

        private void rb_select_CheckedChanged(object sender, EventArgs e)
        {
            txt_weight.Text = "0";

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
        private void btn_Print_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            //if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 87))
            //{
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
            //}
            //else
            //{
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            //}
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            table_050_Packaging1BindingSource.EndEdit();
            table_050_Packaging1TableAdapter.Update(dataSet_05_PCLOR.Table_050_Packaging1);
            if (txt_ID.Text != "")
            {
                if (isadmin)
                {
                    this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                }
                else
                {
                    this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                }
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

   

        private void btn_Sanad_Delete_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            string NumberRecipt = gridEX2.GetValue("NumberRecipt").ToString();
            string ID = gridEX2.GetValue("ID").ToString();
            string IDsanad = gridEX2.GetValue("SanadID").ToString();
            string Return = gridEX2.GetValue("FactorReturnId").ToString();

            DataTable Table = new DataTable();
            try
            {
                if (this.table_050_Packaging1BindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column18", 160))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_050_Packaging1BindingSource.CurrencyManager.Current)["Id"].ToString());
                    string SanadID = ClDoc.ExScalar(ConPCLOR.ConnectionString, @" select SanadID from Table_050_Packaging where id=" + RowID.ToString() + "");
                    string ReciptID = ClDoc.ExScalar(ConPCLOR.ConnectionString, @" select NumberRecipt from Table_050_Packaging where id=" + RowID.ToString() + "");
                    string Returnid = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select FactorReturnId from Table_050_Packaging where id=" + RowID.ToString() + "");
                    if (SanadID =="0" || ReciptID=="0" || Returnid=="0")
                    {
                        MessageBox.Show("رسید، سند، فاکتور مرجوعی برای بارکد صادر نشده است امکان حذف آن را ندارید ");
                        return;
                    }
                   
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";

                        if (ReciptID != "0" || IDsanad != "0")
                        {
                            Message = "برای این فاکتور، رسید انبار و فاکتور مرجوعی نیز صادر شده است. در صورت تأیید ثبت مربوط به رسید و فاکتور مرجوعی نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            if (ReciptID.ToString() != "0" || IDsanad.ToString() != "0" || Returnid.ToString() !="0")
                            {
                                ClDoc.IsFinal_ID(int.Parse(SanadID));

                                Table = ClDoc.ReturnTable(ConACNT, "Select ColumnID from  Table_065_SanadDetail where Column00=" + IDsanad + " and Column16=12 and Column17=" + NumberRecipt);
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += " Delete  from Table_065_SanadDetail where Column00=" + IDsanad + " and Column16=12 and Column17=" + NumberRecipt;



                                Table = ClDoc.ReturnTable(ConACNT, "Select ColumnID from  Table_065_SanadDetail where Column00=" + IDsanad + " and Column16=26 and Column17=" + NumberRecipt);
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += " Delete  from Table_065_SanadDetail where Column00=" + IDsanad + " and Column16=26 and Column17=" + NumberRecipt;


                                command += ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_012_Child_PwhrsReceipt where Column01=" + NumberRecipt);
                                command += ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_011_PwhrsReceipt Where ColumnId=" + NumberRecipt);

                                command += ClDoc.RunSqlCommand(ConSale.ConnectionString, "Delete From Table_019_Child1_MarjooiSale Where Column01=" + Return);
                                command += ClDoc.RunSqlCommand(ConSale.ConnectionString, "Delete From Table_018_MarjooiSale Where ColumnId=" + Return);


                                command += ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update table_050_Packaging set NumberRecipt=0 Where NumberRecipt=" + NumberRecipt);

                                command += ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update table_050_Packaging set SanadID=0 Where SanadID=" + IDsanad);

                                command += ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update table_050_Packaging set FactorReturnId=0 Where FactorReturnId=" + Return);


                               


                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT))
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
                                        mlt_Num_Product_ValueChanged(sender, e);
                                       
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
                                MessageBox.Show("به دلیل نداشتن سند یا رسید امکان حذف وجود ندارد");
                            }
                        }

                    //}
                        mlt_Num_Product_ValueChanged(sender, e);

                    //dataSet_05_PCLOR.EnforceConstraints = false;
                    //this.table_050_Packaging1TableAdapter.FillByID(dataSet_05_PCLOR.Table_050_Packaging1, RowID);
                    //dataSet_05_PCLOR.EnforceConstraints = true;
                    //DS.Tables["Doc"].Clear();
                    //DocAdapter.Fill(DS, "Doc");
                    //this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void btn_Recipt_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

             
                string NumberRecipt = gridEX2.GetValue("NumberRecipt").ToString();



                string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select NumberRecipt from table_050_Packaging where Id = " + txt_ID.Text+ @")

                                                    Delete from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt where columnid in(
                                                    select NumberRecipt from table_050_Packaging where Id = " + txt_ID.Text + @")

                                                    Update table_050_Packaging set NumberRecipt=0  where Id = " + txt_ID.Text + "";
                Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);



                MessageBox.Show("اطلاعات با موفقیت حذف گردید");

                if (isadmin)
                {
                    this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                }
                else
                {
                    this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 132))
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

                             else  if (dtsal.Rows.Count > 0)
                            {
                                MessageBox.Show("این بارکد در فاکتور فروش استفاده شده است امکان حذف ان را ندارید ");
                                return;

                            }


                             else  if (dtreturn.Rows.Count > 0)
                            {
                                MessageBox.Show("این بارکد در فاکتور مرجوعی استفاده شده است امکان حذف ان را ندارید ");
                                return;

                            }


                            if (((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" || ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["SanadID"].ToString() != "0")
                            {
                                MessageBox.Show("این این فاکتور مرجوعی دارای رسید یا سند حسابداری می باشد امکان حذف آن را ندارید ");

                            }

                            else
                            {



                                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"delete from  table_050_Packaging where ID=" + gridEX2.GetValue("ID"));

                                /////پیغام حذف شد
                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                //button1.Enabled = true;
                                ///فیل گرید
                                if (txt_ID.Text != "")
                                {
                                    if (isadmin)
                                    {
                                        this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));

                                    }
                                    else
                                    {
                                        this.table_050_Packaging1TableAdapter.FillByProductIdnew(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text), Class_BasicOperation._UserName);

                                    }
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

        private void gridEX2_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 133))
            {
                string Draft = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isNull((select NumberDraftP from Table_035_Production where Id=" + mlt_Num_Product.Value.ToString() + "),0)");
                if (((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" || ((DataRowView)table_050_Packaging1BindingSource.CurrencyManager.Current)["SanadID"].ToString() != "0" )
                {
                    e.Cancel = true;
                    MessageBox.Show("این بارکد دارای رسید و سند حسابدری می باشد امکان حذف آن را ندارید");
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

                DataTable dtreturn = ClDoc.ReturnTable(ConSale, @"SELECT        dbo.Table_019_Child1_MarjooiSale.Column32
                        FROM            dbo.Table_018_MarjooiSale INNER JOIN
                                                 dbo.Table_019_Child1_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                        WHERE        (dbo.Table_019_Child1_MarjooiSale.Column32 = N'" + gridEX2.GetValue("Barcode").ToString() + "')");



                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("از این بار کد در فرم ارسال و دریافت بارکد استفاده شده است امکان حذف آن را ندارید");
                    return;

                }

                else if (dtsal.Rows.Count > 0)
                {
                    MessageBox.Show("این بارکد در فاکتور فروش استفاده شده است امکان حذف ان را ندارید ");
                    return;

                }


                else  if (dtreturn.Rows.Count > 0)
                {
                    MessageBox.Show("این بارکد در فاکتور مرجوعی استفاده شده است امکان حذف ان را ندارید ");
                    return;

                }
                else
                {

                    e.Cancel = false;

                }



            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void Frm_002_MarjooiSaleBarcod_FormClosing(object sender, FormClosingEventArgs e)
        {
          

            mlt_Ware_R.Value = Properties.Settings.Default.Returnfactor;
            Properties.Settings.Default.Save();
        }

        private void txt_meter_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (mlt_NameCustomer.Text == "" || mlt_Machine.Text == "" || mlt_TypeCloth.Text == "" || mlt_TypeColor.Text == "" || Txt_Price.Text == "")
                    {
                        MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد ");
                    }

                    if (Txt_Price.Text == "" || Txt_Price.Text == "0")
                    {
                        MessageBox.Show("لطفا قیمت طاقه را وارد نمایید");
                        return;
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

        private void mlt_NameCustomer_KeyPress_2(object sender, KeyPressEventArgs e)
        {

        }

        private void mlt_NameCustomer_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_NameCustomer, "Column02", "Column01");

        }

        private void mlt_Num_Product_KeyPress(object sender, KeyPressEventArgs e)
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
                    mlt_NameCustomer.Focus();
                  
                  //Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void gridEX2_RowDoubleClick(object sender, RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX2.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
                    {
                        if (gridEX2.GetValue("FactorReturnId").ToString() == "0" || gridEX2.GetValue("FactorReturnId").ToString() == "")
                        {
                            MessageBox.Show("برای این طاقه فاکتور مرجوعی صادر نشده است ");
                            return;
                        }
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_001_MarjooiSale")
                            {
                                item.BringToFront();
                                Frm_001_MarjooiSale frm = (Frm_001_MarjooiSale)item;
                                frm.txt_Search.Text = gridEX2.GetRow().Cells["FactorReturnId"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        //string Num = "";
                        //foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                        //{
                        //    Num = item.Cells["FactorReturnId"].Value.ToString();
                        //}
                        Frm_001_MarjooiSale frms = new Frm_001_MarjooiSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23), Convert.ToInt32(gridEX2.GetValue("FactorReturnId")),true);
                        try
                        {
                            frms.MdiParent = Frm_Main.ActiveForm;
                        }
                        catch { }
                       
                        frms.Show();
                    }
                    else
                         Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                }
               
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }




      
    }
}
