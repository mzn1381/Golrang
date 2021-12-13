using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;

namespace PCLOR._01_OperationInfo
{
    public partial class Frm_55_ReciptPackaging2 : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        string DraftID = "0";
        int DraftNumber = 0;
        string DetailsIdRecipt = "";
        string idfactor = "";
        public Frm_55_ReciptPackaging2()
        {
            InitializeComponent();
        }

        private void Frm_55_ReciptPackaging2_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull((select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));

            //mlt_Ware_RE.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
            ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
            ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
            ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

            mlt_Function_RE.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
            mlt_NameCustomer.DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column01 from Table_045_PersonInfo");
            gridEX1.DropDowns["TypeCloth"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeCloth from Table_005_TypeCloth");
            gridEX1.DropDowns["Color"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeColor from Table_010_TypeColor");
            gridEX1.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
            gridEX1.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from Table_007_PwhrsDraft");
            gridEX1.DropDowns["Machine"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");
            gridEX1.DropDowns["NumberProduct"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"Select ID,Number from Table_035_Production");



            DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                      @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");
       
            mlt_Ware_RE.DataSource = WareTable;
            gridEX1.RemoveFilters();

            ToastNotification.ToastForeColor = Color.Black;
            ToastNotification.ToastBackColor = Color.SkyBlue;
           
        }
        Int64 countNotBarcode = 0;
        string messageError = "";
        bool errorware = true;
        string barcoderror = string.Empty;
        string messegerepeat = "";
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            try
            {
                string strreplace = System.Text.RegularExpressions.Regex.Replace(txt_Barcode.Text.Trim(), @"\t|\n|\r", "");
                var b = ClDoc.GetNextChars(strreplace.Trim(), 8);
                bool Notsend = false;
                countNotBarcode = 0;
                barcoderror = "";

                foreach (string s in b)
                {

                bool flrepeat = false;

                    Notsend = false;
                    if (s != "")
                    {
                        messageError = "";
                        errorware = false;
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                        {
                            if (s.ToString() == (item.Cells["Barcode"].Value).ToString())
                            {
                                flrepeat = true;
                                break;
                            }
                        }
                       
                        bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                        DataTable dt = ClDoc.ReturnTable(ConPCLOR, "select * from Table_80_Setting");

                        DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select Columnid from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                                  @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");



                        DataTable dtInformation = ClDoc.ReturnTable(ConPCLOR, @"SELECT     TOP (1) dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Qty, dbo.Table_050_Packaging.weight, 
                      dbo.Table_050_Packaging.TypeColor, dbo.Table_005_TypeCloth.CodeCommondity, dbo.Table_70_DetailOtherPWHRS.NumberDraft, 
                      dbo.Table_050_Packaging.Machine, dbo.Table_70_DetailOtherPWHRS.CodeCustomer, dbo.Table_035_Production.ID AS IDProduct
                      FROM         dbo.Table_70_DetailOtherPWHRS INNER JOIN
                      dbo.Table_65_HeaderOtherPWHRS ON dbo.Table_70_DetailOtherPWHRS.FK = dbo.Table_65_HeaderOtherPWHRS.ID INNER JOIN
                      dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID ON 
                      dbo.Table_70_DetailOtherPWHRS.Barcode = dbo.Table_050_Packaging.Barcode
                      WHERE dbo.Table_050_Packaging.Barcode=" + s + " AND dbo.Table_65_HeaderOtherPWHRS.Sends=1 ORDER BY dbo.Table_65_HeaderOtherPWHRS.date DESC, dbo.Table_65_HeaderOtherPWHRS.ID DESC");

                        
                        
                        if (dtInformation.Rows.Count==0)
                        {
                            Notsend = true;
                            dtInformation = ClDoc.ReturnTable(ConPCLOR, @"SELECT     TOP (1) dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Qty, dbo.Table_050_Packaging.weight, 
                      dbo.Table_050_Packaging.TypeColor, dbo.Table_005_TypeCloth.CodeCommondity, 0 AS NumberDraft, dbo.Table_050_Packaging.Machine, 
                      dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_035_Production.ID AS IDProduct
                    FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID
                                        WHERE     (dbo.Table_050_Packaging.Barcode = " + s+")");

                        }

                        if (dtInformation.Rows.Count > 0)
                        {
                        string PWHRS = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL(( SELECT  TOP(1)   dbo.Table_65_HeaderOtherPWHRS.PWHRS
                            FROM         dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                                  dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                            WHERE     (dbo.Table_65_HeaderOtherPWHRS.Sends = 1) AND (dbo.Table_70_DetailOtherPWHRS.Barcode = " + s + ") ORDER BY dbo.Table_65_HeaderOtherPWHRS.date DESC , dbo.Table_70_DetailOtherPWHRS.Id DESC),0)");

                       

                            string Draft = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL(( SELECT     TOP (1)   dbo.Table_70_DetailOtherPWHRS.NumberDraft
                                        FROM         dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                                              dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                                        WHERE     (dbo.Table_70_DetailOtherPWHRS.Barcode = "+s+") AND (dbo.Table_65_HeaderOtherPWHRS.Sends = 1) AND (dbo.Table_70_DetailOtherPWHRS.NumberDraft = 0)),0) ");

                            ////
                            if (WareTable.Rows.Count > 0)
                            {
                                foreach (DataRow item in WareTable.Rows)
                                {
                                    if (mlt_Ware_RE.Value.ToString() == item[0].ToString())
                                    { errorware = true; break; }

                                }
                            }

                            ////

                            table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                            gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                            gridEX1.MoveToNewRecord();
                            gridEX1.SetValue("TypeCloth", dtInformation.Rows[0][0].ToString());
                            gridEX1.SetValue("Barcode", dtInformation.Rows[0][1].ToString());
                            gridEX1.SetValue("Count", dtInformation.Rows[0][2].ToString());
                            gridEX1.SetValue("weight", dtInformation.Rows[0][3].ToString());
                            gridEX1.SetValue("TypeColor", dtInformation.Rows[0][4].ToString());
                            gridEX1.SetValue("CodeCommondity", dtInformation.Rows[0][5].ToString());
                            gridEX1.SetValue("NumberDraft", dtInformation.Rows[0][6].ToString());
                            gridEX1.SetValue("Machine", dtInformation.Rows[0][7].ToString());
                            gridEX1.SetValue("CodeCustomer", dtInformation.Rows[0][8].ToString());
                            gridEX1.SetValue("IDProduct", dtInformation.Rows[0][9].ToString());
                            gridEX1.SetValue("Errware", errorware);
                            gridEX1.SetValue("datet", Class_BasicOperation.ServerDate().ToString());
                            gridEX1.SetValue("datesabt", Class_BasicOperation.ServerDate().ToString());
                        
                            
                            {
                                if ( dtInformation.Rows[0][8].ToString() == mlt_NameCustomer.Value.ToString())
                                {
                                    if (errorware == false || mlt_Ware_RE.Value.ToString()!= PWHRS)
                                    { messageError = "انبار مبدا با مقصد برابر نمی باشد/ شما به این انبار دسترسی ندارید "; }

                                   if (Draft != "0" || Notsend==true)
                                    {
                                        messageError=(messageError==null?"این بارکد در قسمت ارسال ثبت نشده است":messageError+" و این بارکد در قسمت ارسال ثبت نشده است");
                                        gridEX1.SetValue("DescriptionErrore", messageError );
                                      
                                    }


                                    if (flrepeat)
                                    {
 
                                            gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است" + (messageError == "" ? null : " و " + messageError));
                                            gridEX1.SetValue("OtherError", true);

                                    }
                                    else
                                    {
                                        gridEX1.SetValue("DescriptionErrore", messageError);
                                        gridEX1.SetValue("OtherError", false);
                                    }

                                }
                                else if (  dtInformation.Rows[0][8].ToString() != mlt_NameCustomer.Value.ToString())
                                {
                                    if (errorware == false || mlt_Ware_RE.Value.ToString() != PWHRS)
                                    { messageError = "انبار مبدا با مقصد برابر نمی باشد/ شما به این انبار دسترسی ندارید "; 
                                                                            }


                                    if (Draft != "0" || Notsend == true )
                                    {
                                        messageError=(messageError==null?"این بارکد در قسمت ارسال ثبت نشده است":messageError+" و این بارکد در قسمت ارسال ثبت نشده است");
                                        gridEX1.SetValue("DescriptionErrore", messageError );
                                      
                                    }


                                    if (flrepeat)
                                    {

                                        gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است و مشتری آن نامعتبر می باشد " + (messageError == "" ? null : "و" + messageError));
                                        gridEX1.SetValue("OtherError", true);
                                    }
                                    else
                                    {
                                        gridEX1.SetValue("DescriptionErrore", " مشتری بارکد کالای جاری نامعتبر می باشد " + (messageError == "" ? null : " و " + messageError));
                                        gridEX1.SetValue("OtherError", true);
                                    }
                                }
                               

                            }
                            gridEX1.UpdateData();
                            gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;


                        }
                        else
                        {
                            countNotBarcode = Int64.Parse(s);
                            barcoderror += countNotBarcode + Environment.NewLine;

                            messegerepeat="این بارکد نامعتبر است و با مقادیر پیش فرض ثبت شده است";

                            table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                            gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                            gridEX1.MoveToNewRecord();
                            gridEX1.SetValue("TypeCloth", dt.Rows[18][2].ToString());
                            gridEX1.SetValue("Barcode", countNotBarcode);
                            gridEX1.SetValue("Count", 1);
                            gridEX1.SetValue("weight", dt.Rows[21][2].ToString());
                            gridEX1.SetValue("TypeColor", dt.Rows[20][2].ToString());
                            gridEX1.SetValue("CodeCommondity", dt.Rows[23][2].ToString());
                            gridEX1.SetValue("NumberDraft", 0);
                            gridEX1.SetValue("Machine", dt.Rows[19][2].ToString());
                            gridEX1.SetValue("CodeCustomer", dt.Rows[22][2].ToString());
                            gridEX1.SetValue("Errware", errorware);
                            gridEX1.SetValue("datet", Class_BasicOperation.ServerDate().ToString());
                            gridEX1.SetValue("datesabt", Class_BasicOperation.ServerDate().ToString());
                            gridEX1.SetValue("DescriptionErrore", messegerepeat);

                            if (flrepeat)
                            {

                                gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است" + (messegerepeat == "" ? null : " و " + messegerepeat));
                                gridEX1.SetValue("OtherError", true);

                            }
                            gridEX1.UpdateData();
                            gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                        }
                    }
                   
                }
                if (countNotBarcode > 0)
                {

                    ToastNotification.Show(this, "" + Environment.NewLine +barcoderror + ".عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید", 3000, eToastPosition.MiddleCenter);
                    countNotBarcode = 0;

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex , this.Name);
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 51))
            {

                table_65_HeaderOtherPWHRSBindingSource.AddNew();
                txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["usersabt"] = Class_BasicOperation._UserName;
                ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["datesabt"] = Class_BasicOperation.ServerDate().ToString();
                mlt_NameCustomer.Focus();
                btn_New.Enabled = false;
                mlt_NameCustomer.Text = "";
                txt_Description.Text = "";
                txt_Barcode.Text = "";
                table_65_HeaderOtherPWHRSBindingSource_PositionChanged(sender, e);
            }

            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (table_65_HeaderOtherPWHRSBindingSource.Count > 0)
                {

                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull( (select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));

                    if (mlt_NameCustomer.Text == "0" || mlt_NameCustomer.Text == "" || mlt_Ware_RE.Text == "" || mlt_Ware_RE.Text == "0" || mlt_Function_RE.Text == "" || mlt_Function_RE.Text == "0")
                    {
                        MessageBox.Show("لطفا اطلاعات مورد نظر را تکمیل نماید");
                        return;
                    }

                    if (mlt_Ware_RE.Text.All(char.IsDigit) || mlt_Function_RE.Text.All(char.IsDigit))
                    {
                        MessageBox.Show("اطلاعات نامعتبر می باشد");
                        return;
                    }
                    if (gridEX1.RowCount > 0)
                    {
                        if (((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0")
                        {
                            MessageBox.Show("برای این بسته بندی رسید صادر شده است امکان ویرایش آن را ندارید");
                            return;
                        }
                   
                        if (!txt_Number.Text.StartsWith("-")||((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString()=="0")
                        {
                          
                            checkbarcode(); }



                        bool errore = false;
                       
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                        {
                            
                            if ((item.Cells["OtherError"].Value).ToString() == "True")
                            {
                                errore = true;
                                break;
                            }

                        }

                        if (gridEX1.RowCount > 0)
                        {


                            if (((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0")
                            {
                                if (errore)
                                {
                                    MessageBox.Show("بارکدی دارای خطا می باشد امکان ذخیره و صدور رسید را ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                                {
                                    txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, " Table_65_HeaderOtherPWHRS", "Number").ToString();
                                }


                                {
                                    int Position = gridEX1.CurrentRow.RowIndex;
                                    table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                                    table_65_HeaderOtherPWHRSTableAdapter.Update(barcodeDataset.Table_65_HeaderOtherPWHRS);
                                    table_70_DetailOtherPWHRSBindingSource.EndEdit();
                                    table_70_DetailOtherPWHRSTableAdapter.Update(barcodeDataset.Table_70_DetailOtherPWHRS);
                                    btn_Recipt_Click(sender, e);
                                    gridEX1.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                                    barcodeDataset.EnforceConstraints = false;
                                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                    this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                    barcodeDataset.EnforceConstraints = true;
                                    gridEX1.MoveTo(Position);
                                    btn_New.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("سطری برای ذخیره وجود ندارد");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }
        private void txt_Description_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_Barcode.Focus();

            }
        }

        int Count=0 ;
       
        private void checkbarcode()      
        {

           
            countNotBarcode = 0;
            barcoderror = "";
          
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {

                Count = 0;
                if (item.Cells["NumberRecipt"].Text == "0")
                {
                    if (item.Cells["Barcode"].Text != "")
                    {
                      
                        errorware = false;
                        bool Notsend = false;
                        messageError = "";
                        foreach (Janus.Windows.GridEX.GridEXRow dr in gridEX1.GetRows())
                        {
                           
                            if (item.Cells["Barcode"].Value.ToString() == dr.Cells["Barcode"].Value.ToString())
                                Count++;
                          
                        }

                        bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                        DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select Columnid from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                                  @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");

                        DataTable dtInformation = ClDoc.ReturnTable(ConPCLOR, @"SELECT     TOP (1) dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Qty, dbo.Table_050_Packaging.weight, 
                      dbo.Table_050_Packaging.TypeColor, dbo.Table_005_TypeCloth.CodeCommondity, dbo.Table_70_DetailOtherPWHRS.NumberDraft, 
                      dbo.Table_050_Packaging.Machine, dbo.Table_70_DetailOtherPWHRS.CodeCustomer, dbo.Table_035_Production.ID AS IDProduct
                      FROM         dbo.Table_70_DetailOtherPWHRS INNER JOIN
                      dbo.Table_65_HeaderOtherPWHRS ON dbo.Table_70_DetailOtherPWHRS.FK = dbo.Table_65_HeaderOtherPWHRS.ID INNER JOIN
                      dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID ON 
                      dbo.Table_70_DetailOtherPWHRS.Barcode = dbo.Table_050_Packaging.Barcode
                      WHERE dbo.Table_050_Packaging.Barcode=" + item.Cells["Barcode"].Text + " AND dbo.Table_65_HeaderOtherPWHRS.Sends=1 ORDER BY dbo.Table_65_HeaderOtherPWHRS.date DESC, dbo.Table_65_HeaderOtherPWHRS.ID DESC");
                    
                        DataTable dt = ClDoc.ReturnTable(ConPCLOR, "select * from Table_80_Setting");


                        if (dtInformation.Rows.Count==0)
                        {
                            dtInformation = ClDoc.ReturnTable(ConPCLOR, @"SELECT     TOP (1) dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Qty, dbo.Table_050_Packaging.weight, 
                      dbo.Table_050_Packaging.TypeColor, dbo.Table_005_TypeCloth.CodeCommondity, 0 AS NumberDraft, dbo.Table_050_Packaging.Machine, 
                      dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_035_Production.ID AS IDProduct
                    FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID
                       WHERE     (dbo.Table_050_Packaging.Barcode = " + item.Cells["Barcode"].Text + ")");

                        }

                         if (dtInformation.Rows.Count > 0)
                        {
                            string Draft = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL(( SELECT     TOP (1)   dbo.Table_70_DetailOtherPWHRS.NumberDraft
                                        FROM         dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                                              dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                                        WHERE     (dbo.Table_70_DetailOtherPWHRS.Barcode = " + item.Cells["Barcode"].Text + @")
                AND (dbo.Table_65_HeaderOtherPWHRS.Sends = 1) AND (dbo.Table_70_DetailOtherPWHRS.NumberDraft = 0)),0) ");

                            ////


                            string PWHRS = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL(( SELECT  TOP(1)   dbo.Table_65_HeaderOtherPWHRS.PWHRS
                            FROM         dbo.Table_65_HeaderOtherPWHRS INNER JOIN
                                                  dbo.Table_70_DetailOtherPWHRS ON dbo.Table_65_HeaderOtherPWHRS.ID = dbo.Table_70_DetailOtherPWHRS.FK
                            WHERE     (dbo.Table_65_HeaderOtherPWHRS.Sends = 1) AND (dbo.Table_70_DetailOtherPWHRS.Barcode = " + item.Cells["Barcode"].Text + ") ORDER BY dbo.Table_65_HeaderOtherPWHRS.date DESC, dbo.Table_70_DetailOtherPWHRS.Id DESC),0)");

                            foreach (DataRow dr in WareTable.Rows)
                            {
                                if (mlt_Ware_RE.Value.ToString() == dr[0].ToString())
                                { errorware = true; break; }
                              
                            }


                            ////
                            item.BeginEdit();
                            item.Cells["TypeCloth"].Value= dtInformation.Rows[0][0].ToString();
                            item.Cells["Barcode"].Value= dtInformation.Rows[0][1].ToString();
                            item.Cells["Count"].Value= dtInformation.Rows[0][2].ToString();
                            item.Cells["weight"].Value= dtInformation.Rows[0][3].ToString();
                            item.Cells["TypeColor"].Value= dtInformation.Rows[0][4].ToString();
                            item.Cells["CodeCommondity"].Value= dtInformation.Rows[0][5].ToString();
                            item.Cells["NumberDraft"].Value=dtInformation.Rows[0][6].ToString();
                            item.Cells["Machine"].Value= dtInformation.Rows[0][7].ToString();
                            item.Cells["CodeCustomer"].Value= dtInformation.Rows[0][8];
                            item.Cells["IDProduct"].Value = dtInformation.Rows[0][9];
                            item.Cells["Errware"].Value= errorware;
                            item.Cells["datet"].Value = Class_BasicOperation.ServerDate().ToString();
                            item.Cells["datesabt"].Value = Class_BasicOperation.ServerDate().ToString();

                            if ( dtInformation.Rows[0][8].ToString() == mlt_NameCustomer.Value.ToString())
                            {


                                if (Draft != "0" || Notsend == true)
                                {
                                    messageError = (messageError == null ? "این بارکد در قسمت ارسال ثبت نشده است" : messageError + " و این بارکد در قسمت ارسال ثبت نشده است");
                                    gridEX1.SetValue("DescriptionErrore", messageError);

                                }

                                if (errorware == false || mlt_Ware_RE.Value.ToString() != PWHRS)
                                {
                                    messageError = "انبار مبدا با مقصد برابر نمی باشد/ شما به این انبار دسترسی ندارید ";
                                }
                                {
                                item.Cells["DescriptionErrore"].Value = messageError + (Count > 1 ? " و بارکد جاری تکراری می باشد " : "");
                                 }
                                if (Count > 1)
                                {
                                    item.Cells["OtherError"].Value = true;
                                }
                                else { item.Cells["OtherError"].Value = false; }

                            }
                            else if ( dtInformation.Rows[0][8].ToString() != mlt_NameCustomer.Value.ToString())
                            {
                                if (Draft != "0"  || Notsend == true)
                                {
                                    messageError = (messageError == null ? "این بارکد در قسمت ارسال ثبت نشده است" : messageError + " و این بارکد در قسمت ارسال ثبت نشده است");
                                    gridEX1.SetValue("DescriptionErrore", messageError);

                                }

                                if (errorware == false || mlt_Ware_RE.Value.ToString() != PWHRS)
                                { messageError = "انبار مبدا با مقصد برابر نمی باشد/ شما به این انبار دسترسی ندارید "; }

                                {
                                    item.Cells["DescriptionErrore"].Value = " مشتری بارکد کالای جاری نامعتبر می باشد " + (messageError == "" ? null : " و " + messageError) + (Count > 1 ? " و بارکد جاری تکراری می باشد " : ""); 
                                     item.Cells["OtherError"].Value= true;
                                }
                            }
                           
                            item.EndEdit();
                            gridEX1.UpdateData();
                        }
                         else
                         {
                             countNotBarcode = Int64.Parse(item.Cells["Barcode"].Text);
                             barcoderror += countNotBarcode + " , ";
                             messegerepeat = "این بارکد نامعتبر است ";
                             item.BeginEdit();
                             if (Count > 1)
                             {
                                 item.Cells["DescriptionErrore"].Value = messegerepeat + (Count > 1 ? " و بارکد جاری تکراری می باشد " : "");
                                 item.Cells["OtherError"].Value = true;
                             }
                             else { item.Cells["OtherError"].Value = false; }

                             item.EndEdit();
                             gridEX1.UpdateData();
                         }
                    }
                }

            }
            if (countNotBarcode > 0)
            {
                MessageBox.Show(" عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید" + barcoderror + Environment.NewLine);
                countNotBarcode = 0; 
            }
        }
        private void mlt_NameCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, null, "Column01");
        }
        private void btn_Recipt_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
            {


                {
                    ClDoc.ReturnTable(ConPCLOR, @" SELECT  ISNULL(( select CodeCommondity  FROM  dbo.Table_005_TypeCloth where  ID=" + gridEX1.GetValue("CodeCommondity") + "),0)as commodity");

                    int ResidNum = ClDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");

                    string commandtxt = string.Empty;
                    commandtxt = "Declare @Key int";
                    DetailsIdRecipt = "";
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
                                                                            [column11]
                                                                 
                                                                          ) VALUES (" + ResidNum + ",'" + txt_Dat.Text + "' ," + mlt_Ware_RE.Value.ToString() + "," + mlt_Function_RE.Value.ToString() + ","
                                                                                              + mlt_NameCustomer.Value.ToString() + ",'" + "رسید صادره بابت دریافت بارکد ش" + txt_Number.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()); SET @Key=Scope_Identity() ";
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {

                        commandtxt += @" INSERT INTO Table_012_Child_PwhrsReceipt (
                                    [column01]
                                   ,[column02]
                                   ,[column03]
                                   ,[column06]
                                   ,[column07]
                                   ,[column10]
                                   ,[column11]
                                   ,[column15]
                                   ,[column17]
                                   ,[column18]
                                   ,[column20]
                                   ,[column21]
                                   ,[column30]
                                   ,[column34]
                                   ,[column35]
                                   ,[column36]
                                   ,[column37]
                           ) VALUES (@Key," + (item.Cells["CodeCommondity"].Value.ToString()) + ",1," + 1 +
                    "," + 1 + ",0,0,'" + Class_BasicOperation._UserName +
                    "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + item.Cells["Barcode"].Value.ToString() + "," + item.Cells["weight"].Value.ToString() + "," + item.Cells["weight"].Value.ToString() + ",N'" + item.Cells["TypeColor"].Text  + "',N'" + item.Cells["Machine"].Text + "'); ";
                        DetailsIdRecipt = DetailsIdRecipt + item.Cells["ID"].Value.ToString() + ",";
                    }


                    commandtxt += " Update " + ConPCLOR.Database + ".dbo.Table_70_DetailOtherPWHRS set NumberRecipt=@Key where ID in ( " + DetailsIdRecipt.TrimEnd(',') + @");
                  Update " + ConPCLOR.Database + ".dbo. Table_65_HeaderOtherPWHRS set Recipt=1  Where ID=" + ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString() + ";";
                    
                    
                    
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = commandtxt;
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();
                            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                            //ToastNotification.Show(this, "رسید انبار به شماره " + ResidNum.ToString() + ".با موفقیت صادر شد", 3000, eToastPosition.MiddleCenter);
                            MessageBox.Show("رسید انبار به شماره " + ResidNum.ToString() + " با موفقیت ثبت شد ");

                            ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");

                        }



                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }
                    }
                    this.Cursor = Cursors.Default;

                }


            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 52))
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull((select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));

                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                   
//                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
//                    {
//                        idfactor = ClDoc.ExScalar(ConSale.ConnectionString, @" select isnull ((SELECT        dbo.Table_010_SaleFactor.columnid
//                        FROM            dbo.Table_010_SaleFactor INNER JOIN
//                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
//                        WHERE        (dbo.Table_011_Child1_SaleFactor.Column34 = " + item.Cells["Barcode"].Value.ToString() + ")),0)");
//                    }
//                    if (idfactor != "0")
//                    {
//                        MessageBox.Show("بارکد دارای فاکتور فروش می باشد امکان حذف این برگه نیست");
//                        return;
//                    }
//                    else
//                    {
                        if (gridEX1.RowCount > 0 && Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                        {
                            if (((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" || ((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "")
                            {

                                ToastNotification.Show(this, "این بسته بندی دارای رسید امکان حذف آن را ندارید", 2000, eToastPosition.MiddleCenter);
                            }

                            else
                            {

                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_70_DetailOtherPWHRS where FK =" + ((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["FK"].ToString() +
                                ";delete from Table_65_HeaderOtherPWHRS where ID=" + ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString() + "");
                                try
                                {

                                    barcodeDataset.EnforceConstraints = false;
                                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                    this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                    barcodeDataset.EnforceConstraints = true;
                                }
                                catch { }
                                ToastNotification.Show(this, "اطلاعات با موفقیت حذف گردید", 2000, eToastPosition.MiddleCenter);

                            }
                        }

                    }
                //}
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این را ندارید", Class_BasicOperation.MessageType.None);
            }
        }
        private void mlt_NameCustomer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void Frm_55_ReciptPackaging2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.N)
            {
                btn_New_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {
                btn_Delete_Click(sender, e);
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

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
             try
             {
                 DataTable Table = new DataTable();
                 bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                gridEX1.UpdateData();
                table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                this.table_70_DetailOtherPWHRSBindingSource.EndEdit();


                if (barcodeDataset.Table_65_HeaderOtherPWHRS.GetChanges() != null || barcodeDataset.Table_70_DetailOtherPWHRS.GetChanges() != null )
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        btn_Save_Click(sender, e);
                    }
                }
                btn_New.Enabled = true;
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Recipt=1),0) as Row");
                    
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Recipt=1 AND UserSabt='"+Class_BasicOperation._UserName+"'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                    barcodeDataset.EnforceConstraints = false;
                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    barcodeDataset.EnforceConstraints = true;
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
        try
            {
                DataTable Table = new DataTable();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                if (txt_Number.Text != "")
                {
                    gridEX1.UpdateData();
                    table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                    this.table_70_DetailOtherPWHRSBindingSource.EndEdit();


                    if (barcodeDataset.Table_65_HeaderOtherPWHRS.GetChanges() != null || barcodeDataset.Table_70_DetailOtherPWHRS.GetChanges() != null
                       )
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            btn_Save_Click(sender, e);
                        }
                    }
                    btn_New.Enabled = true;
                    if (isadmin)
                    {
                    Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select Min(Number) from Table_65_HeaderOtherPWHRS where Number>" +
                        ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Recipt=1),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select Min(Number) from Table_65_HeaderOtherPWHRS where Number>" +
                        ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Recipt=1 AND UserSabt='"+Class_BasicOperation._UserName+"'),0) as Row");
                    }
                        if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                        barcodeDataset.EnforceConstraints = false;
                        this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        barcodeDataset.EnforceConstraints = true;


                    }
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
         try
            {
                DataTable Table = new DataTable();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
              
             if (txt_Number.Text != "")
                {
                    gridEX1.UpdateData();
                    table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                    this.table_70_DetailOtherPWHRSBindingSource.EndEdit();


                    if (barcodeDataset.Table_65_HeaderOtherPWHRS.GetChanges() != null || barcodeDataset.Table_70_DetailOtherPWHRS.GetChanges() != null
                       )
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            btn_Save_Click(sender, e);
                        }
                    }
                    btn_New.Enabled = true;
                    if (isadmin)
                    {
                    Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Number<" +
                            ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Recipt=1),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Number<" +
                            ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Recipt=1 AND UserSabt='"+Class_BasicOperation._UserName+"'),0) as Row");
                    }
                        
                        if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                        barcodeDataset.EnforceConstraints = false;
                        this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        barcodeDataset.EnforceConstraints = true;


                    }
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
             try
             {
                 DataTable Table = new DataTable();
                 bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                gridEX1.UpdateData();
                table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                this.table_70_DetailOtherPWHRSBindingSource.EndEdit();


                if (barcodeDataset.Table_65_HeaderOtherPWHRS.GetChanges() != null || barcodeDataset.Table_70_DetailOtherPWHRS.GetChanges() != null
                   )
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        btn_Save_Click(sender, e);
                    }
                }
                btn_New.Enabled = true;
                if (isadmin)
                {
             Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_65_HeaderOtherPWHRS where Recipt=1),0) as Row");
                    
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_65_HeaderOtherPWHRS where Recipt=1 AND UserSabt='"+Class_BasicOperation._UserName+"'),0) as Row");

                }
                 if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                    barcodeDataset.EnforceConstraints = false;
                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                      barcodeDataset.EnforceConstraints = true;
                   

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void table_65_HeaderOtherPWHRSBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
                    SELECT     ISNULL
                                          ((SELECT     COUNT(ISNULL(NumberRecipt, 0)) AS Res
                                              FROM         dbo.Table_70_DetailOtherPWHRS
                WHERE     (FK = " + ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString() + @")
                AND (NumberRecipt > 0)), 0) AS result").ToString() == "0")
                {
                    uiPanel0Container.Enabled = true;
                }
                else uiPanel0Container.Enabled = false;
            }
            catch { }
        }

        private void btn_Del_R_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 53))
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull( (select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", gridEX1.GetValue("NumberDraft").ToString()) != 0)
                    {
                        throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");

                    }

                    else
                    {
                        

                        string CommandTexxt=@"Delete from "+ConWare.Database+@".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select NumberRecipt from Table_70_DetailOtherPWHRS where FK = "+txt_Id.Text+@")

                                                    Delete from "+ConWare.Database+@".dbo.Table_011_PwhrsReceipt where columnid in(
                                                    select NumberRecipt from Table_70_DetailOtherPWHRS where FK = "+txt_Id.Text+@")

                                                    Update Table_70_DetailOtherPWHRS set NumberRecipt=0   where FK = "+txt_Id.Text+"";
                        Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR,CommandTexxt);

                       

                        int Position = gridEX1.CurrentRow.RowIndex;
                        barcodeDataset.EnforceConstraints = false;
                        this.table_65_HeaderOtherPWHRSTableAdapter.FillByReciptID(barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                        this.table_70_DetailOtherPWHRSTableAdapter.FillByReciptID(this.barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                        barcodeDataset.EnforceConstraints = true;
                        gridEX1.MoveTo(Position);
                        MessageBox.Show("عملیات حذف با موفقیت انجام شد");
                        table_65_HeaderOtherPWHRSBindingSource_PositionChanged(sender, e);
                    }
                }
            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            try
            {
//                idfactor = ClDoc.ExScalar(ConSale.ConnectionString, @" select isnull ((SELECT        dbo.Table_010_SaleFactor.columnid
//                        FROM            dbo.Table_010_SaleFactor INNER JOIN
//                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
//                        WHERE        (dbo.Table_011_Child1_SaleFactor.Column34 = " +  gridEX1.GetValue("Barcode").ToString()+ ")),0)");
                if (gridEX1.GetValue("NumberRecipt").ToString() != "0"  )
                {
                    MessageBox.Show("به دلیل صدور رسید برای این سطر امکان حذف را ندارد.");
                    e.Cancel = true;
                }
                //else if (idfactor != "0")
                //{
                //    MessageBox.Show("این بارکد دارای فاکتور فروش می باشد امکان حذف آن را ندارید");
                //    e.Cancel = true;
                //}

            }
            catch { }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Search.Text == "")
                {
                    MessageBox.Show("لطفا شماره مورد نظر را وارد نمایید");
                    return;
                }
               
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                string user = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull ((select Usersabt from Table_65_HeaderOtherPWHRS where Number =" + txt_Search.Text + " AND (Recipt = 1)),0)");

                barcodeDataset.EnforceConstraints = false;
                if (isadmin)
                {
                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByNumberRecipt(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                    if (table_65_HeaderOtherPWHRSBindingSource.Count > 0)
                    {
                        this.table_70_DetailOtherPWHRSTableAdapter.FillByNumber(barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                    }
                    else
                    {
                        MessageBox.Show("این شماره دریافتی وجود ندارد");

                    }
                }
                else  if (user == Class_BasicOperation._UserName)
                {
                   
                    
                        this.table_65_HeaderOtherPWHRSTableAdapter.FillByNumberRecipt(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                        if (table_65_HeaderOtherPWHRSBindingSource.Count > 0)
                        {
                            this.table_70_DetailOtherPWHRSTableAdapter.FillByNumber(barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                        }

                        else
                        {
                            MessageBox.Show("این شماره دریافتی وجود ندارد");

                        }

                        txt_Search.SelectAll();
                        barcodeDataset.EnforceConstraints = true;
                    }
              
                    else
                    {
                        MessageBox.Show("شما به این شماره دریافت دسترسی ندارید");
                    }


            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Search_Click(sender, e); 
        }

        private void چاپارسالA4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string NumberRecipt = "";
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetCheckedRows())
                {
                    NumberRecipt += Row.Cells["ID"].Value.ToString() + ",";
                }
                NumberRecipt.TrimEnd(',');
                if (NumberRecipt == "")
                {
                    MessageBox.Show("لطفا برای چاپ بارکد مورد نظر را انتخاب نمایید");
                    return;
                }
                else
                {
                    Report.Frm_Report_BarcodeRecipt frm = new Report.Frm_Report_BarcodeRecipt(NumberRecipt);
                    frm.Show();
                }
            }
            catch { }
        }
      
        private void طراحیدلخواهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("barcodRecipt.mrt");
            r.Design();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXExporter1.GridEX = gridEX1;
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                MessageBox.Show("عملیات ارسال با موفقیت انجام گرفت");
            }
        }

        private void چاپA5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string NumberRecipt = "";
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetCheckedRows())
                {
                    NumberRecipt += Row.Cells["ID"].Value.ToString() + ",";
                }
                NumberRecipt.TrimEnd(',');
                if (NumberRecipt == "")
                {
                    MessageBox.Show("لطفا برای چاپ بارکد مورد نظر را انتخاب نمایید");
                    return;
                }
                else
                {
                    Report.Frm_Report_BarcodeRecipt_A5 frm = new Report.Frm_Report_BarcodeRecipt_A5(NumberRecipt);
                    frm.Show();
                }
            }
            catch { }
        }

        private void طراحیدلخواهA5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("barcodRecipt-A5.mrt");
            r.Design();
        }
    }
}
