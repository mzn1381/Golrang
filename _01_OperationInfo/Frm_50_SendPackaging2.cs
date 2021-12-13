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
    public partial class Frm_50_SendPackaging2 : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        SqlParameter DraftId;
        string DraftID = "0";
        string DraftNumber = "";
        BindingSource GoodbindingSource = new BindingSource();
        DataTable GoodTable = new DataTable();
        string DetailsIdDraft = "";
        Int64 countNotBarcode = 0;
        string errorrecipt = "";
        int Count = 0;
        int DraftNum = 0;
        string barcoderror = string.Empty;
        public Frm_50_SendPackaging2()
        {
            InitializeComponent();
        }

        private void Frm_40_SendPackaging_Load(object sender, EventArgs e)
        {

            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull((select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));

                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

                mlt_Ware_S.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
                mlt_Ware_R.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from Table_001_PWHRS");
                if (chek1.Checked == true)
                {
                    mlt_Ware_R.Visible = false;
                    label6.Visible = false;

                }
                mlt_Function_S.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");

                //                DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS  WHERE
                //                                                             'True'='" + isadmin.ToString() +
                //                                                         @"'  or 
                //                                                               Columnid IN 
                //                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");

                //                mlt_Ware_S.DataSource = WareTable;

                gridEX1.DropDowns["ware"].DataSource = ClDoc.ReturnTable(ConWare, @"Select Columnid ,Column01,Column02 from Table_001_PWHRS");


                gridEX1.DropDowns["Customer"].DataSource = mlt_NameCustomer.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT DISTINCT  dbo.Table_025_HederOrderColor.CodeCustomer, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column01
                FROM         dbo.Table_035_Production INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dbo.Table_025_HederOrderColor.CodeCustomer = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId");

                gridEX1.DropDowns["TypeCloth"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeCloth from Table_005_TypeCloth");
                gridEX1.DropDowns["Color"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeColor from Table_010_TypeColor");
                gridEX1.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @"select ColumnId,Column01 from Table_007_PwhrsDraft");
                gridEX1.DropDowns["NumberProduct"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"Select ID,Number from Table_035_Production");

                //string Ware = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=16");

                //mlt_Function_S.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=6");

                gridEX1.DropDowns["Machine"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");
                ToastNotification.ToastForeColor = Color.Black;
                ToastNotification.ToastBackColor = Color.SkyBlue;
                gridEX1.RemoveFilters();
                ///
                GoodTable = clGood.MahsoolInfo();
                GoodbindingSource.DataSource = GoodTable;

            }
            catch { }
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

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 47))
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
                //mlt_Function_S.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=6");
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
                    long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull((select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));



                    if (mlt_NameCustomer.Text == "0" || mlt_NameCustomer.Text == "" || mlt_Ware_S.Text == "" || mlt_Ware_S.Text == "0" || mlt_Function_S.Text == "" || mlt_Function_S.Text == "0")
                    {
                        MessageBox.Show("لطفا اطلاعات مورد نظر را تکمیل نماید");
                        return;
                    }

                    if (mlt_Ware_S.Text.All(char.IsDigit) || mlt_Function_S.Text.All(char.IsDigit))
                    {
                        MessageBox.Show("اطلاعات نام انبار و نوع حواله نامعتبر می باشد");
                        return;
                    }
                    if (((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Confir"].ToString() == "True")
                    {
                        MessageBox.Show("به دلیل صدور حواله برای این ارسال امکان ذخیره مجدد وجود ندارد.");
                        btn_New.Enabled = true;
                    }
                    else
                    {

                        {
                            if (!txt_Number.Text.StartsWith("-"))
                            { checkbarcode(); }

                            bool errore = false;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                if ((item.Cells["DescriptionErrore"].Value).ToString() != "")
                                {
                                    errore = true;
                                    break;
                                }

                            }

                            foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                            {

                                if (Row.Cells["PWHRS"].Value.ToString() == mlt_Ware_S.Value.ToString())
                                {
                                    MessageBox.Show("انبار نامعتبر است ");
                                    return;
                                }
                            }

                            if (gridEX1.RowCount > 0)
                            {


                                if (errore)
                                {
                                    MessageBox.Show("بارکدی دارای خطا می باشد امکان ذخیره و صدور حواله را ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                if (((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                                {
                                    txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, " Table_65_HeaderOtherPWHRS", "Number").ToString();
                                }

                                table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                                table_70_DetailOtherPWHRSBindingSource.EndEdit();
                                table_65_HeaderOtherPWHRSTableAdapter.Update(barcodeDataset.Table_65_HeaderOtherPWHRS);
                                table_70_DetailOtherPWHRSBindingSource.EndEdit();
                                table_70_DetailOtherPWHRSTableAdapter.Update(barcodeDataset.Table_70_DetailOtherPWHRS);
                                ExportDraft();
                               
                                gridEX1.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                                barcodeDataset.EnforceConstraints = false;
                                this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                barcodeDataset.EnforceConstraints = true;

                                if (DraftNumber.Length > 0)
                                {
                                    if (errwareaccess == true)
                                    {
                                        MessageBox.Show("حواله انبار به شماره " + DraftNumber.TrimEnd(',') + " با موفقیت ثبت شد " + "و به دلیل عدم دسترسی به انبار مربوطه امکان صدور حواله برای بعضی آیتم ها وجود ندارد");
                                    }
                                    else
                                        MessageBox.Show("حواله انبار به شماره " + DraftNumber.TrimEnd(',') + " با موفقیت ثبت شد ");
                                }
                                else { MessageBox.Show(" به دلیل عدم دسترسی به انبار مربوطه امکان صدور حواله برای بعضی آیتم ها وجود ندارد"); }

                            }



                            else
                            {
                                MessageBox.Show("سطری برای ذخیره وجود ندارد");
                            }


                        }
                        btn_New.Enabled = true;
                    }

                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }


        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            try
            {
                table_65_HeaderOtherPWHRSBindingSource.EndEdit();

            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            try
            {
                e.DisplayErrorMessage = false;

            }
            catch (Exception)
            {
                Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);

            }
        }
        private float FirstRemain(int GoodCode, string Ware, Int64 Barcode)
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
                CommandText = string.Format(CommandText, Ware, GoodCode, txt_Dat.Text, Barcode);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorrecipt = "";
            countNotBarcode = 0;
            barcoderror = "";
            try
            {
                if (table_65_HeaderOtherPWHRSBindingSource.Count > 0 || table_70_DetailOtherPWHRSBindingSource.Count > 0)
                {


                    string strreplace = System.Text.RegularExpressions.Regex.Replace(txt_Barcode.Text.Trim(), @"\t|\n|\r", "");
                    var b = ClDoc.GetNextChars(strreplace.Trim(), 8);

                 

                    foreach (string s in b)
                    {
                        bool flrepeat = false;

                        if (s != "")
                        {

                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                if (s.ToString() == (item.Cells["Barcode"].Value).ToString())
                                {
                                    flrepeat = true;
                                    break;
                                }
                            }

                      DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Qty, dbo.Table_050_Packaging.weight, 
                      dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_050_Packaging.TypeColor, dbo.Table_005_TypeCloth.CodeCommondity, 
                      dbo.Table_050_Packaging.Machine, dbo.Table_035_Production.ID AS IDProduct
                      FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID WHERE     (dbo.Table_050_Packaging.Barcode =" + s + ") ");

                            if (dt.Rows.Count > 0)
                            {
                                float Remain = FirstRemain(int.Parse(dt.Rows[0][6].ToString()), (s));

                                string wear = ClDoc.ExScalar(ConWare.ConnectionString, @"select isnull((SELECT     TOP (1) PERCENT dbo.Table_011_PwhrsReceipt.column03 AS Wear
                                    FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                                      dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01

                                WHERE     (dbo.Table_012_Child_PwhrsReceipt.Column30 = '" + Convert.ToInt64(s).ToString() + "') " +
                                " ORDER BY  dbo.Table_011_PwhrsReceipt.column02 DESC,dbo.Table_011_PwhrsReceipt.column03 DESC),0)");

                                if (wear != "0" )
                                {
                                    table_65_HeaderOtherPWHRSBindingSource.EndEdit();
                                    gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                                    gridEX1.MoveToNewRecord();
                                    gridEX1.SetValue("FK", ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString());
                                    gridEX1.SetValue("TypeCloth", dt.Rows[0][0].ToString());
                                    gridEX1.SetValue("Barcode", dt.Rows[0][1].ToString());
                                    gridEX1.SetValue("Count", dt.Rows[0][2].ToString());
                                    gridEX1.SetValue("weight", dt.Rows[0][3].ToString());
                                    gridEX1.SetValue("TypeColor", dt.Rows[0][5].ToString());
                                    gridEX1.SetValue("CodeCommondity", dt.Rows[0][6]);
                                    gridEX1.SetValue("Machine", dt.Rows[0][7]);
                                    gridEX1.SetValue("CodeCustomer", dt.Rows[0][4]);
                                    gridEX1.SetValue("IDProduct", dt.Rows[0][8]);
                                    gridEX1.SetValue("NumberDraft", 0);
                                    if (chek1.Checked==true)
                                    {
                                    gridEX1.SetValue("PWHRS", wear);

                                    }
                                   else if (chek1.Checked == false)
                                    {
                                      
                                        gridEX1.SetValue("PWHRS", mlt_Ware_R.Value.ToString());
                                        
                                    }
                                    gridEX1.SetValue("datet", Class_BasicOperation.ServerDate().ToString());
                                    gridEX1.SetValue("datesabt", Class_BasicOperation.ServerDate().ToString());
                                    {
                                        if (Remain > 0 && dt.Rows[0][4].ToString() == mlt_NameCustomer.Value.ToString())
                                        {
                                            if (flrepeat)
                                                gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است");

                                        }
                                        else if (Remain > 0 && dt.Rows[0][4].ToString() != mlt_NameCustomer.Value.ToString())
                                        {
                                            if (flrepeat)
                                                gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است و مشتری آن متفاوت می باشد ");
                                            else
                                                gridEX1.SetValue("DescriptionErrore", " مشتری بارکد کالای جاری مشتری آن نامعتبر می باشد ");
                                        }
                                        else if (Remain <= 0 && dt.Rows[0][4].ToString() == mlt_NameCustomer.Value.ToString())
                                        {
                                            if (flrepeat)
                                                gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است و موجودی برای این بارکد در انبار وجود ندارد");
                                            else
                                                gridEX1.SetValue("DescriptionErrore", "موجودی برای این بارکد در انبار وجود ندارد");

                                        }
                                        else if (Remain <= 0 && dt.Rows[0][4].ToString() != mlt_NameCustomer.Value.ToString())
                                        {
                                            if (flrepeat)
                                                gridEX1.SetValue("DescriptionErrore", "این بارکد تکراری وارد شده است و موجودی برای این بارکد در این انبار وجود ندارد و مشتری آن نامعتبر می باشد.");
                                            else
                                                gridEX1.SetValue("DescriptionErrore", "موجودی برای این بارکد در این انبار وجود ندارد و مشتری آن نامعتبر می باشد.");

                                        }

                                    }
                                    //   mlt_NameCustomer.Value = dt.Rows[0][4].ToString();
                                    gridEX1.UpdateData();
                                    gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;

                                }
                                else errorrecipt = errorrecipt + "," + s;


                            }
                            else
                            {
                                countNotBarcode = Int64.Parse(s);
                                barcoderror += Int64.Parse(s) + Environment.NewLine;

                            }
                        }

                       
                       
                    }

                    if (errorrecipt.Length > 7)
                    {

                        MessageBox.Show("برای بارکدهای زیر رسید صادر نشده است" + errorrecipt);

                    }

                    if (countNotBarcode > 0)
                    {
                        MessageBox.Show("عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید" + Environment.NewLine + barcoderror.ToString());
                        //ToastNotification.Show(this, "" + countNotBarcode.ToString() + ".عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید", 3000, eToastPosition.MiddleCenter);

                    }
                    
                }
            }
            catch (Exception ex)
            {
               
                Class_BasicOperation.CheckExceptionType(ex, this.Name); errorrecipt = "";
            }
            errorrecipt = "";
        }
        private float FirstRemain(int GoodCode, string Barcode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT				 ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date , dbo.Table_012_Child_PwhrsReceipt.Column30 AS Grease
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE       (dbo.Table_012_Child_PwhrsReceipt.column02 = {0}) 
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.Column30
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date, dbo.Table_008_Child_PwhrsDraft.Column30 AS Grease
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_008_Child_PwhrsDraft.column02 = {0})  
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.Column30) AS derivedtbl_1
                       WHERE     (Date <= '{1}' and Grease={2} ) ";
                CommandText = string.Format(CommandText, GoodCode, txt_Dat.Text, Barcode);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
        bool errwareaccess = false;
        private void checkbarcode()
        {

            try
            {
                errorrecipt = "";
                countNotBarcode = 0;
                barcoderror = "";

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Count = 0;

                    if (item.Cells["NumberDraft"].Text == "0")
                    {
                        if (item.Cells["Barcode"].Text != "")
                        {
                            bool errore = false;



                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_050_Packaging.Barcode, dbo.Table_050_Packaging.Qty, dbo.Table_050_Packaging.weight, 
                      dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_050_Packaging.TypeColor, dbo.Table_005_TypeCloth.CodeCommondity, 
                      dbo.Table_050_Packaging.Machine, dbo.Table_035_Production.ID AS IDProduct
                      FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                      dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID INNER JOIN
                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
                      dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct INNER JOIN
                      dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID  WHERE     (dbo.Table_050_Packaging.Barcode =" + item.Cells["Barcode"].Value.ToString() + ") ");

                            if (dt.Rows.Count > 0)
                            {
                                float Remain = FirstRemain(int.Parse(dt.Rows[0][6].ToString()), (item.Cells["Barcode"].Value.ToString()));

                                string wear = ClDoc.ExScalar(ConWare.ConnectionString, @"select isnull((SELECT     TOP (1) PERCENT dbo.Table_011_PwhrsReceipt.column03 AS Wear
                    FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                      dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01

                                WHERE     (dbo.Table_012_Child_PwhrsReceipt.Column30 = '" + item.Cells["Barcode"].Value.ToString() + "')  ORDER BY dbo.Table_011_PwhrsReceipt.columnid DESC, dbo.Table_011_PwhrsReceipt.column02 DESC),0)");

                                if (wear != "0")
                                {
                                    item.BeginEdit();

                                    item.Cells["TypeCloth"].Value = dt.Rows[0][0].ToString();
                                    item.Cells["Barcode"].Value = dt.Rows[0][1].ToString();
                                    item.Cells["Count"].Value = dt.Rows[0][2].ToString();
                                    item.Cells["weight"].Value = dt.Rows[0][3].ToString();
                                    item.Cells["TypeColor"].Value = dt.Rows[0][5].ToString();
                                    item.Cells["CodeCommondity"].Value = dt.Rows[0][6];
                                    item.Cells["Machine"].Value = dt.Rows[0][7];
                                    item.Cells["CodeCustomer"].Value = dt.Rows[0][4].ToString();
                                    item.Cells["IDProduct"].Value = dt.Rows[0][8].ToString();
                                    item.Cells["NumberDraft"].Value = 0;
                                    gridEX1.SetValue("datet", Class_BasicOperation.ServerDate().ToString());
                                    gridEX1.SetValue("datesabt", Class_BasicOperation.ServerDate().ToString());

                                    foreach (Janus.Windows.GridEX.GridEXRow dr in gridEX1.GetRows())
                                    {
                                        if (item.Cells["Barcode"].Value.ToString() == dr.Cells["Barcode"].Value.ToString())
                                            Count++;

                                    }
                                    {

                                        if (Remain > 0 && dt.Rows[0][4].ToString() != mlt_NameCustomer.Value.ToString())
                                        {
                                            if (errore == false)
                                                item.Cells["DescriptionErrore"].Value = " مشتری بارکد کالای جاری مشتری آن نامعتبر می باشد " + (Count > 1 ? " و بارکد جاری تکراری می باشد " : "");

                                        }
                                        else
                                            if (Remain <= 0 && dt.Rows[0][4].ToString() == mlt_NameCustomer.Value.ToString())
                                            {

                                                if (errore == false)
                                                    item.Cells["DescriptionErrore"].Value = "موجودی برای این بارکد در انبار وجود ندارد" + (Count > 1 ? " و بارکد جاری تکراری می باشد " : "");

                                            }
                                            else
                                                if (Remain <= 0 && dt.Rows[0][4].ToString() != mlt_NameCustomer.Value.ToString())
                                                {
                                                    if (errore == false)
                                                        item.Cells["DescriptionErrore"].Value = "موجودی برای این بارکد در این انبار وجود ندارد و مشتری آن نامعتبر می باشد." + (Count > 1 ? " و بارکد جاری تکراری می باشد " : "");

                                                }
                                                else item.Cells["DescriptionErrore"].Value = "" + (Count > 1 ? "  بارکد جاری تکراری می باشد " : "");

                                    }
                                    item.EndEdit();

                                }
                                else errorrecipt = errorrecipt + "," + item.Cells["Barcode"].Value.ToString();

                            }
                            else
                            {
                                countNotBarcode = Int64.Parse(item.Cells["Barcode"].Text);
                                barcoderror += countNotBarcode + " , ";
                            }

                        }
                    }
                }
                if (errorrecipt.Length > 7)
                {
                    MessageBox.Show("برای بارکدهای زیر رسید صادر نشده است" + errorrecipt);
                }
                if (countNotBarcode > 0)
                {

                    ToastNotification.Show(this, "" + Environment.NewLine + barcoderror + ".عدد از بارکدها اشتباه می باشد لطفا اصلاح کنید", 3000, eToastPosition.MiddleCenter);
                    countNotBarcode = 0; errorrecipt = "";

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); errorrecipt = "";
            }
            errorrecipt = "";

        }
        private void ExportDraft()
        {

            this.Cursor = Cursors.WaitCursor;
            errwareaccess = false;
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            DataTable WareTable = ClDoc.ReturnTable(ConWare, @"Select Columnid as ware from Table_001_PWHRS  WHERE
                                                             'True'='" + isadmin.ToString() +
                                                                 @"'  or 
                                                               Columnid IN 
                                                               (select Ware from " + ConPCLOR.Database + ".dbo.Table_95_DetailWare where FK in(select  Column133 from " + ConBase.Database + ".dbo. table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + @"'))");


            string Ware = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=16");

            ////
            DataTable dtPWHRS = new DataTable();
            dtPWHRS.Columns.Add("PWHRS", typeof(Int16));
            string CmdText = "";

            foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
            {
                dtPWHRS.Rows.Add(Row.Cells["PWHRS"].Value.ToString());
            }
            ////
            DataTable dtUnicAnbar = new DataTable();
            dtUnicAnbar = dtPWHRS.DefaultView.ToTable(true, "PWHRS");

            ////
            DraftNumber = DetailsIdDraft = "";

       
            for (int i = 0; dtUnicAnbar.Rows.Count > i; i++)
            {
                ///چک کردن اینکه کاربر یاادمین باشد یا به انباری که در آن حواله می زد دسترسی داشته باشد
                if (isadmin || Convert.ToInt64(WareTable.Select("ware=" + dtUnicAnbar.Rows[i][0]).Count().ToString()) > 0)
                {
                    DraftNum = 0;
                    DraftId = new SqlParameter("DraftID", SqlDbType.Int);
                    DraftId.Direction = ParameterDirection.Output;

                    DraftNum = ClDoc.MaxNumber(Properties.Settings.Default.PWHRS, "Table_007_PwhrsDraft", "Column01");
                    DraftNumber = DraftNumber + DraftNum + ",";
                    DetailsIdDraft = "";
                    ///

                    

                    CmdText = (@"INSERT INTO Table_007_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                    column17, column18, column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26) VALUES(" + DraftNum + ",'" + txt_Dat.Text + "'," +
                    dtUnicAnbar.Rows[i][0] + "," + mlt_Function_S.Value + "," + mlt_NameCustomer.Value + ",'" + "حواله صادره بابت ارسال بارکد ش" + txt_Number.Text + "',0,'" + Class_BasicOperation._UserName +
                    "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0,0,0,0,0,0,null,0,1);SET @DraftID=SCOPE_IDENTITY(); ");

                    int Unit = Convert.ToInt16(ClDoc.ExScalar(ConWare.ConnectionString, @"(select Column07 from table_004_CommodityAndIngredients where Columnid=" + gridEX1.GetValue("CodeCommondity") + ")"));

                    #region ForeChild
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {
                       
                        {
                            if (Convert.ToInt32(item.Cells["Count"].Value) > 0)
                            {
                                

                                if (item.Cells["PWHRS"].Value.ToString() == dtUnicAnbar.Rows[i][0].ToString())
                                {

                                   

                                    GoodbindingSource.RemoveFilter();
                                    GoodbindingSource.Filter = "GoodID=" + (Convert.ToDecimal(item.Cells["CodeCommondity"].Value)).ToString();

                                    DataRowView GoodRow = (DataRowView)GoodbindingSource.CurrencyManager.Current;

                                    Double SingleValue = 0;
                                    if (!Class_BasicOperation._WareType)
                                        SingleValue = clGood.GoodValue(int.Parse((Convert.ToDecimal(item.Cells["CodeCommondity"].Value)).ToString()), short.Parse(dtUnicAnbar.Rows[0][0].ToString()));
                                    Double TotalValue = Math.Round(SingleValue * Convert.ToDouble(item.Cells["Count"].Value.ToString()), 4);

                                   


                                    CmdText = CmdText + (@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09,
                                    column10, column11, column12, column13, column14, column15, column16, 
                                    column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, column28, column29, Column30, Column31, Column32, Column33, Column34,Column35, Column36,Column37) VALUES(@DraftID,"
                                   + (Convert.ToDecimal(item.Cells["CodeCommondity"].Value)).ToString() + "," + Unit + ",0,0,1,1,0,0,0,0,N' " + txt_Description.Text + "',NULL,NULL," + SingleValue + "," + TotalValue + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                                   "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0," + item.Cells["Barcode"].Value + ",NULL,0,0," + item.Cells["weight"].Value + "," + item.Cells["weight"].Value + ",N'" + item.Cells["TypeColor"].Text + "',N'" + item.Cells["Machine"].Text + "')");
                                    DetailsIdDraft = DetailsIdDraft + item.Cells["ID"].Value.ToString() + ",";
                                }
                            }

                        }

                    }
                    #endregion

                   
                    CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.Table_70_DetailOtherPWHRS set NumberDraft=@DraftID  Where ID in (" + DetailsIdDraft.TrimEnd(',') + @");
                    Update " + ConPCLOR.Database + ".dbo.Table_65_HeaderOtherPWHRS set Sends=1,Confir=1  Where ID=" + ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString() + ";";


                    #region TransactionDraft
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
                            DetailsIdDraft = "";


                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;



                    }
                    #endregion


                }
                else { errwareaccess = true; }
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
        private void Fifo(string dfID)
        {
            //محاسبه ارزش حواله
            SqlDataAdapter goodAdapter = new SqlDataAdapter(
                @"SELECT     dbo.Table_008_Child_PwhrsDraft.columnid, dbo.Table_008_Child_PwhrsDraft.column01, dbo.Table_008_Child_PwhrsDraft.column02, 
                      dbo.Table_008_Child_PwhrsDraft.column03, dbo.Table_008_Child_PwhrsDraft.column04, dbo.Table_008_Child_PwhrsDraft.column05, 
                      dbo.Table_008_Child_PwhrsDraft.column06, dbo.Table_008_Child_PwhrsDraft.column07, dbo.Table_008_Child_PwhrsDraft.column08, 
                      dbo.Table_008_Child_PwhrsDraft.column09, dbo.Table_008_Child_PwhrsDraft.column10, dbo.Table_008_Child_PwhrsDraft.column11, 
                      dbo.Table_008_Child_PwhrsDraft.column12, dbo.Table_008_Child_PwhrsDraft.column13, dbo.Table_008_Child_PwhrsDraft.column14, 
                      dbo.Table_008_Child_PwhrsDraft.column15, dbo.Table_008_Child_PwhrsDraft.column16, dbo.Table_008_Child_PwhrsDraft.column17, 
                      dbo.Table_008_Child_PwhrsDraft.column18, dbo.Table_008_Child_PwhrsDraft.column19, dbo.Table_008_Child_PwhrsDraft.column20, 
                      dbo.Table_008_Child_PwhrsDraft.column21, dbo.Table_008_Child_PwhrsDraft.column22, dbo.Table_008_Child_PwhrsDraft.column23, 
                      dbo.Table_008_Child_PwhrsDraft.column24, dbo.Table_008_Child_PwhrsDraft.column25, dbo.Table_008_Child_PwhrsDraft.column26, 
                      dbo.Table_008_Child_PwhrsDraft.column27, dbo.Table_008_Child_PwhrsDraft.column28, dbo.Table_008_Child_PwhrsDraft.column29, 
                      dbo.Table_008_Child_PwhrsDraft.Column30, dbo.Table_008_Child_PwhrsDraft.Column31, dbo.Table_008_Child_PwhrsDraft.Column32, 
                      dbo.Table_008_Child_PwhrsDraft.Column33, dbo.Table_008_Child_PwhrsDraft.Column34, dbo.Table_008_Child_PwhrsDraft.Column35, 
                      dbo.Table_008_Child_PwhrsDraft.Column36, dbo.Table_008_Child_PwhrsDraft.Column37, dbo.Table_007_PwhrsDraft.column03 AS ware
FROM         dbo.Table_008_Child_PwhrsDraft INNER JOIN
                      dbo.Table_007_PwhrsDraft ON dbo.Table_008_Child_PwhrsDraft.column01 = dbo.Table_007_PwhrsDraft.columnid
WHERE     (dbo.Table_008_Child_PwhrsDraft.column01 =" + dfID + ")", ConWare);
            DataTable Table = new DataTable();
            goodAdapter.Fill(Table);

            //DataRow drOrd = (DataRow)tableordr.Rows[0];

            //محاسبه ارزش و ذخیره آن در جدول Child1
            if (Class_BasicOperation._WareType)
            {
                foreach (DataRow item in Table.Rows)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 [dbo].[PR_00_FIFO]  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + item["ware"].ToString(), ConWare);
                    DataTable TurnTable = new DataTable();
                    Adapter.Fill(TurnTable);
                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + Convert.ToInt32(DraftId.Value) + " and DetailID=" + item["Columnid"].ToString());
                    ClDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString());
                }
            }
            else
            {
                foreach (DataRow item in Table.Rows)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 [dbo].[PR_05_AVG]  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + item["ware"].ToString(), ConWare);
                    DataTable TurnTable = new DataTable();
                    Adapter.Fill(TurnTable);
                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + Convert.ToInt32(DraftId.Value) + " and DetailID=" + item["Columnid"].ToString());
                    ClDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString());

                }
            }


        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull((select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 48))
                {
                    if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (gridEX1.RowCount > 0 && Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                        {

                            if (((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0" || ((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "")
                            {
                                int Position = gridEX1.CurrentRow.RowIndex;

                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_70_DetailOtherPWHRS where FK =" + ((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["FK"].ToString() + "");
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_65_HeaderOtherPWHRS where ID=" + ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString() + "");
                                barcodeDataset.EnforceConstraints = false;
                                this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                                barcodeDataset.EnforceConstraints = true;
                                gridEX1.MoveTo(Position);
                                ToastNotification.Show(this, "اطلاعات با موفقیت حذف گردید  ", 2000, eToastPosition.MiddleCenter);

                            }
                            else
                            {
                                if (((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0" || ((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "")
                                {
                                    ToastNotification.Show(this, "این بسته بندی دارای حواله می باشد امکان حذف آن را ندارید", 2000, eToastPosition.MiddleCenter);
                                }
                            }

                        }

                    }
                }
                else
                {

                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch { }
        }
        private void Frm_40_SendPackaging_KeyDown(object sender, KeyEventArgs e)
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
        private void btn_Delete_Draft_Click(object sender, EventArgs e)
        {
            try
            {

                string Delete = "";
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                long Branche = long.Parse(ClDoc.ExScalar(ConBase.ConnectionString, @"select isNull((select top (1) Column133 from Table_045_PersonInfo where Column23=N'" + Class_BasicOperation._UserName + "'),0)"));

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 49))
                {
                    if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ///چک کردن سند حواله

                        if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", ((DataRowView)table_70_DetailOtherPWHRSBindingSource.CurrencyManager.Current)["NumberDraft"].ToString()) != 0)
                        {
                            throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");
                        }
                        else
                        {
//                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
//                            {
//                                if (item.Cells["NumberDraft"].Value.ToString() != "0")
//                                {

//                                    Delete = Delete + @"Delete From " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=" + item.Cells["NumberDraft"].Value.ToString() + @";
//                                    Delete From " + ConWare.Database + ".dbo.Table_007_PwhrsDraft Where ColumnId=" + item.Cells["NumberDraft"].Value.ToString() + ";" +
//                                    "Update  Table_70_DetailOtherPWHRS set NumberDraft=0 Where ID=" + item.Cells["ID"].Value.ToString();
//                                }

//                            }
                            string CommandText = @"Delete from "+ConWare.Database+@".dbo. Table_008_Child_PwhrsDraft where COlumn01 in(
                                                        select NumberDraft from Table_70_DetailOtherPWHRS where FK = "+txt_ID.Text+@")

                                                        Delete  from " + ConWare.Database + @".dbo. Table_007_PwhrsDraft where columnid in(
                                                        select NumberDraft from Table_70_DetailOtherPWHRS where FK = " + txt_ID.Text + @")

                                                        Update Table_70_DetailOtherPWHRS set NumberDraft=0   where FK = " + txt_ID.Text + @"
                            Update Table_65_HeaderOtherPWHRS set   Confir=0  Where Number=" + txt_Number.Text+"";

                            Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR,CommandText);
                            int Position = gridEX1.CurrentRow.RowIndex;
                            barcodeDataset.EnforceConstraints = false;
                            this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                            this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString()));
                            barcodeDataset.EnforceConstraints = true;
                            //   gridEX1.MoveTo(Position);
                            MessageBox.Show("اطلاعات با موفقیت حذف گردید");
                            table_65_HeaderOtherPWHRSBindingSource_PositionChanged(sender, e);

                        }
                    }
                }


                else
                {

                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }


        }
        private void txt_Description_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_Barcode.Focus();

            }
        }
        private void table_65_HeaderOtherPWHRSBindingSource_PositionChanged(object sender, EventArgs e)
        {


            try
            {
                if (ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
    SELECT     ISNULL
                          ((SELECT     COUNT(ISNULL(NumberDraft, 0)) AS Res
                              FROM         dbo.Table_70_DetailOtherPWHRS
WHERE     (FK = " + ((DataRowView)table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["ID"].ToString() + @")
AND (NumberDraft > 0)), 0) AS result").ToString() == "0")
                {
                    uiPanel0Container.Enabled = true;
                }
                else uiPanel0Container.Enabled = false;
            }
            catch { }
        }

        private void mlt_NameCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, null, "Column01");
        }

        private void mlt_NameCustomer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void txt_Barcode_KeyPress(object sender, KeyPressEventArgs e)
        {

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
                    Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_65_HeaderOtherPWHRS where Sends=1),0) as Row");

                }
                else
                {
                    Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_65_HeaderOtherPWHRS where Sends=1 AND UserSabt='" + Class_BasicOperation._UserName + "'),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                    barcodeDataset.EnforceConstraints = false;
                    this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    barcodeDataset.EnforceConstraints = true;


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
                          ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Sends=1),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Number<" +
                          ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Sends=1 AND UserSabt='" + Class_BasicOperation._UserName + "'),0) as Row");
                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                        barcodeDataset.EnforceConstraints = false;
                        this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        barcodeDataset.EnforceConstraints = true;


                    }
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
                        ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Sends=1),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select Min(Number) from Table_65_HeaderOtherPWHRS where Number>" +
                        ((DataRowView)this.table_65_HeaderOtherPWHRSBindingSource.CurrencyManager.Current)["Number"].ToString() + " and Sends=1 and UserSabt='" + Class_BasicOperation._UserName + "'),0) as Row");
                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                        barcodeDataset.EnforceConstraints = false;
                        this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                        barcodeDataset.EnforceConstraints = true;


                    }
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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


                if (barcodeDataset.Table_65_HeaderOtherPWHRS.GetChanges() != null || barcodeDataset.Table_70_DetailOtherPWHRS.GetChanges() != null
                   )
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        btn_Save_Click(sender, e);
                    }
                }
                btn_New.Enabled = true;
                ///
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Sends=1),0) as Row");
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConPCLOR, @"Select ISNULL((Select max(Number) from Table_65_HeaderOtherPWHRS where Sends=1 AND UserSabt='" + Class_BasicOperation._UserName + "'),0) as Row");
                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select ID from Table_65_HeaderOtherPWHRS where Number=" + Table.Rows[0]["Row"].ToString());
                    barcodeDataset.EnforceConstraints = false;
                    this.table_65_HeaderOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_65_HeaderOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_70_DetailOtherPWHRSTableAdapter.FillBySendID(this.barcodeDataset.Table_70_DetailOtherPWHRS, int.Parse(RowId.Rows[0]["ID"].ToString()));
                    barcodeDataset.EnforceConstraints = true;
                }

                ///

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (gridEX1.GetValue("NumberDraft").ToString() != "0")
            {
                MessageBox.Show("به دلیل صدور حواله برای این سطر امکان حذف را ندارد.");
                e.Cancel = true;
            }
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
                string user = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull ((select Usersabt from Table_65_HeaderOtherPWHRS where Number =" + txt_Search.Text + " AND (Sends = 1)),0)");

                barcodeDataset.EnforceConstraints = false;

                if (isadmin)
                {
                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByNumberSend(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                    if (table_65_HeaderOtherPWHRSBindingSource.Count > 0)
                    {
                        this.table_70_DetailOtherPWHRSTableAdapter.FillByNumber(barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                    }
                    else
                    {
                        MessageBox.Show("این شماره ارسالی وجود ندارد");

                    }

                }
                else if (user == Class_BasicOperation._UserName)
                {

                    this.table_65_HeaderOtherPWHRSTableAdapter.FillByNumberSend(this.barcodeDataset.Table_65_HeaderOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                    if (table_65_HeaderOtherPWHRSBindingSource.Count > 0)
                    {
                        this.table_70_DetailOtherPWHRSTableAdapter.FillByNumber(barcodeDataset.Table_70_DetailOtherPWHRS, Convert.ToInt64(txt_Search.Text));
                    }

                    else
                    {
                        MessageBox.Show("این شماره ارسالی وجود ندارد");

                    }

                    txt_Search.SelectAll();
                    barcodeDataset.EnforceConstraints = true;



                }
                else
                {
                    MessageBox.Show("شما به این شماره ارسال دسترسی ندارید");
                }


            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void mlt_Ware_S_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Search_Click(sender, e);
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {


        }
        string NumberR = "";
        private void چاپارسالA4ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetCheckedRows())
            {

                NumberR += Row.Cells["ID"].Value.ToString() + ",";
            }

            NumberR.TrimEnd(',');
            if (NumberR == "")
            {
                MessageBox.Show("لطفا برای چاپ بارکد مورد نظر را انتخاب نمایید");
                return;
            }
            else
            {
                Report.Frm_Report_BarcodSend frm = new Report.Frm_Report_BarcodSend(NumberR);
                frm.Show();
            }
        }

        private void طراحیدلخواهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("barcodsend.mrt");
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
            foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetCheckedRows())
            {

                NumberR += Row.Cells["ID"].Value.ToString() + ",";
            }

            NumberR.TrimEnd(',');
            if (NumberR == "")
            {
                MessageBox.Show("لطفا برای چاپ بارکد مورد نظر را انتخاب نمایید");
                return;
            }
            else
            {
                Report.Frm_Report_BarcodSend_A5 frm = new Report.Frm_Report_BarcodSend_A5(NumberR);
                frm.Show();
            }
        }

        private void طراحیدلخواهA4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("barcodsend-A5.mrt");
            r.Design();
        }

        private void Chek1_CheckedChanged(object sender, EventArgs e)
        {
            if (chek1.Checked == true)
            {
                mlt_Ware_R.Visible = false;
                label6.Visible = false;

            }

            else if (chek1.Checked == false)
            {
                mlt_Ware_R.Visible = true;
                label6.Visible = true;
                label6.Visible = true;
            }
        }

        private void Mlt_Ware_R_KeyPress(object sender, KeyPressEventArgs e)
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
    }







}
