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
using Janus.Windows.GridEX;
using Stimulsoft.Report.Components;

namespace PCLOR.MarjooiSale
{
    public partial class Frm_30_Production_Edite : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();  
        string s;
        int ResidNum = 0;
        string Messages = "";
        SqlParameter DraftId;
        DataTable dtt;
        bool errore = false;
        string Messege = string.Empty;
        string repeat = string.Empty;
        string Num = "";
        public Frm_30_Production_Edite()
        {
            InitializeComponent();
        }
        private void Frm_30_Production_Load(object sender, EventArgs e)
        {
            

            try
            {

                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

                gridEX2.DropDowns["Machine"].DataSource = gridEX3.DropDowns["Machine"].DataSource = mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");
                gridEX1.DropDowns["Draft"].DataSource = gridEX2.DropDowns["Draft"].DataSource = gridEX3.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
               
                gridEX2.DropDowns["Number"].DataSource = ClDoc.ReturnTable(ConPCLOR, @" SELECT     dbo.Table_030_DetailOrderColor.Fk, dbo.Table_030_DetailOrderColor.ID, dbo.Table_025_HederOrderColor.Number
                        FROM         dbo.Table_025_HederOrderColor INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk");

                gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");

//                gridEX2.DropDowns["CodeColor"].DataSource = mlt_CodeOrderColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT * FROM(SELECT t30.ID,t30.TypeColor ,t10.TypeColor AS Color ,t60.namemachine,t25.Number,t05.TypeCloth,t30.NumberOrder AS NumberOrder,t30.weight,
//			     t30.NumberOrder-ISNULL((SELECT sum(NumberProduct) FROM Table_035_Production WHERE ColorOrderId= t30.Id ),0) AS Remain
//											    FROM Table_030_DetailOrderColor  AS t30 
//											    LEFT JOIN Table_010_TypeColor AS t10 ON t10.Id = t30.TypeColor
//											    LEFT JOIN Table_60_SpecsTechnical t60 ON t60.ID = t30.[Machine]
//											    LEFT JOIN Table_025_HederOrderColor t25 ON t25.ID=t30.Fk
//											    LEFT JOIN Table_005_TypeCloth t05 ON t05.ID=t30.TypeColth
//											    LEFT JOIN  Table_035_Production t35 ON t35.ColorOrderId=t30.ID
//			    GROUP BY t30.id,t30.TypeColor,t30.NumberOrder,t10.TypeColor,t60.namemachine,t25.Number,t05.TypeCloth,t30.NumberOrder,t30.weight  ) AS t WHERE t.Remain>0");

                gridEX2.DropDowns["TypeCloth"].DataSource = gridEX3.DropDowns["TypeCloth"].DataSource= mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeCloth,CodeCommondity from Table_005_TypeCloth");
                gridEX1.DropDowns["Color"].DataSource = gridEX3.DropDowns["Color"].DataSource = mlt_TypeColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeColor from Table_010_TypeColor");
                gridEX3.DropDowns["Customer"].DataSource = mlt_NameCustomer.DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column01,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["Commodity2"].DataSource = gridEX1.DropDowns["Commodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select Columnid, Column01,Column02,column07 from table_004_CommodityAndIngredients");
                //ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");

//                mlt_Ware_D.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
//                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
//                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
//                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 1)");


                mlt_Ware_R.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 2)");


                mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 5)");



                mlt_Return.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 3)");


               

                mlt_TypeRerturn.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_R.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");


                //mlt_Ware_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=14");

                //mlt_Function_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=2");

                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=18");

                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=8");

                mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                mlt_Function_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=3");


                Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
                r.Load("Report.mrt");
                foreach (StiPage page in r.Pages)
                {
                    uiComboBox1.Items.Add(page.Name);
                }


                mlt_Return.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=28");

                mlt_TypeRerturn.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=33"); 



                gridEX1.DropDowns["mount"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id ,NameColor from Table_055_ColorDefinition");
                gridEX1.DropDowns["ColorM"].DataSource = ClDoc.ReturnTable(ConPCLOR, @" select Id,NameColor from Table_055_ColorDefinition");

                dataSet_05_PCLOR.EnforceConstraints = false;
                this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                this.table_45_EditeProductTableAdapter.Fill(this.dataSet_05_PCLOR.Table_45_EditeProduct);
                dataSet_05_PCLOR.EnforceConstraints = true;
                
                
                ToastNotification.ToastForeColor = Color.Black;
                ToastNotification.ToastBackColor = Color.SkyBlue;
                table_035_ProductionBindingSource_PositionChanged(sender, e);
                gridEX1.RemoveFilters();
                gridEX2.RemoveFilters();
                gridEX3.RemoveFilters();
            }
            catch { }
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 136))
            {
               

                table_035_ProductionBindingSource.AddNew();
                table_40_ColorPrductionBindingSource.AddNew();
                txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();
                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["EditProduct"]="True";
                txt_weight.Text = "0";
                txt_NumberProduct.Text = "0";
                mlt_TypeCloth.Text = "";
                txt_Barcode.Text = "";
                mlt_TypeColor.Text = "";
                mlt_Machine.Text = "";
                txt_Barcode.Focus();
                btn_New.Enabled = false;
               
               
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }

        }
        private void btn_Save_Click(object sender, EventArgs e)
         {
             int Position = gridEX2.CurrentRow.RowIndex;

            Messages = "";
            try
            {



                if (gridEX1.RowCount > 0)
                {

                    if (gridEX3.RowCount>0)
                    {
                        
                    #region برگه ولید هست یا نه
                    if (mlt_Ware.Text == "" || mlt_Ware_R.Text == "" || mlt_Function.Text == "" || mlt_Function_R.Text == "" || txt_NumberProduct.Text == "" || mlt_TypeRerturn.Text=="" || mlt_Return.Text=="")
                    {
                        throw new Exception("اطلاعات مورد نظر را تکمیل نمایید");
                    }
                   

                    #endregion


                  
                    bool Sodorhavelmasrafi = true;
                    foreach (DataRowView Row in table_40_ColorPrductionBindingSource)
                    {
                        if (Row["NumberDraft"].ToString() == "0")
                        {
                            Sodorhavelmasrafi = false;
                            break;
                        }
                    }

                    //if ((((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ReturnDraftId"].ToString() == "0") && (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0") && (Sodorhavelmasrafi == false))
                    //{

                        table_035_ProductionBindingSource.EndEdit();
                        table_035_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_035_Production);
                        table_40_ColorPrductionBindingSource.EndEdit();
                        table_40_ColorPrductionTableAdapter.Update(dataSet_05_PCLOR.Table_40_ColorPrduction);
                        table_45_EditeProductBindingSource.EndEdit();
                        table_45_EditeProductTableAdapter.Update(dataSet_05_PCLOR.Table_45_EditeProduct);

                        #region بررسی موجودی برای حواله های مصرفی
                        if (Sodorhavelmasrafi == false)
                        {
                            string good1 = string.Empty;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()), int.Parse(item.Cells["CodeCommondity"].Value.ToString())))
                                    throw new Exception("کالای " + item.Cells["CodeCommondity"].Text + " در این انبار فعال نمی باشد ");

                                float Remain = FirstRemain1(int.Parse(item.Cells["CodeCommondity"].Value.ToString()), mlt_Ware.Value.ToString());
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
                                                                                                       WHERE  ColumnId = " + item.Cells["CodeCommondity"].Value + @"
                                                                                                   ),
                                                                                                   0
                                                                                               ) AS Column16", ConWareGood);
                                        mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                    }
                                }
                                catch
                                {
                                }
                                if (Remain < float.Parse(item.Cells["Consumption"].Value.ToString()))
                                {
                                    if (!mojoodimanfi)
                                    {
                                        good1 += item.Cells["CodeCommondity"].Text + " , ";
                                    }

                                }

                            }
                            if (!string.IsNullOrEmpty(good1))
                                throw new Exception("عدم موجودی کالای " + good1.TrimEnd(','));
                        }
                        #endregion

                        #region صدور رسید در صورتی که رسید خورده نشده باشد
                        if ((((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0"))
                        {
                            if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Return.Text.All(char.IsDigit) || mlt_TypeRerturn.Text.All(char.IsDigit))
                            {
                                MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                                return;
                            }
                            else
                            {
                                btn_Recipt_Click(sender, e);
                            }
                        }
                        #endregion

                        #region صدور حواله هدر در صورتی که حواله خورده نشده باشد
                        if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ReturnDraftId"].ToString() == "0")
                        {
                            if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Return.Text.All(char.IsDigit) || mlt_TypeRerturn.Text.All(char.IsDigit))
                            {
                                MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                                return;
                            }
                            else
                            {

                                if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ColorOrderId"].ToString() == "")
                                {
                                      ExportOrderColorId();
                                }
                              
                                ExportDraftCloth();

                            }
                        }
                        #endregion

                        #region صدور حواله مواد مصرفی در صورتی که حواله صادر نشده باشد
                        if (!Sodorhavelmasrafi)
                        {
                            if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Return.Text.All(char.IsDigit) || mlt_TypeRerturn.Text.All(char.IsDigit))
                            {
                                MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                                return;
                            }
                            else
                            {

                                ExportDraft();
                                gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                                DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_015_FormulColor.CodeColore, dbo.Table_015_FormulColor.NumberKilo, dbo.Table_010_TypeColor.ID, dbo.Table_055_ColorDefinition.CodeCommondity,dbo.Table_010_TypeColor.TypeColor, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS Count
                        FROM         dbo.Table_010_TypeColor INNER JOIN
                              dbo.Table_015_FormulColor ON dbo.Table_010_TypeColor.ID = dbo.Table_015_FormulColor.Fk INNER JOIN
                              dbo.Table_055_ColorDefinition ON dbo.Table_015_FormulColor.CodeColore = dbo.Table_055_ColorDefinition.ID INNER JOIN
                              " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                              dbo.Table_055_ColorDefinition.CodeCommondity = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                         WHERE     (dbo.Table_010_TypeColor.ID  = " + mlt_TypeColor.Value + ")");

                                foreach (DataRow Row in dt.Rows)
                                {
                                    this.table_40_ColorPrductionBindingSource.AddNew();
                                    DataRowView Tb_40 = (DataRowView)table_40_ColorPrductionBindingSource.CurrencyManager.Current;

                                    Tb_40["CodeColor"] = Row["CodeColore"];
                                    Tb_40["TypeColor"] = Row["ID"];
                                    Tb_40["CodeCommondity"] = Row["CodeCommondity"];
                                    Decimal Weightt = ((Convert.ToDecimal(txt_weight.Text) * Convert.ToDecimal(Row["NumberKilo"]))) / 100;
                                    Tb_40["Consumption"] = Weightt;
                                    table_40_ColorPrductionBindingSource.EndEdit();


                                }
                            }
                        }
                        #endregion
                        //int Position = gridEX2.CurrentRow.RowIndex;
                        this.dataSet_05_PCLOR.EnforceConstraints = false;
                        this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                        this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                        this.table_45_EditeProductTableAdapter.Fill(this.dataSet_05_PCLOR.Table_45_EditeProduct);
                        this.dataSet_05_PCLOR.EnforceConstraints = true;
                        gridEX2.MoveTo(Position);
                        MessageBox.Show("اطلاعات با موفقیت ذخیره شد" + Environment.NewLine + Messages);
                        gridEX2.Enabled = true;
                        btn_New.Enabled = true;
                        table_035_ProductionBindingSource_PositionChanged(sender, e);
                    //}
                    }
                    else
                    {
                        MessageBox.Show("اصلاعات بارکد خالی می باشد");
                    }

                }
                else
                {
                    MessageBox.Show("لطفا ابتدا محاسبه رنگ انجام شود");
                    return;
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
                if (Convert.ToInt32(gridEX2.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 137))
                    {

                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int Position = gridEX2.CurrentRow.RowIndex;
                            if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ReturnDraftId"].ToString() != "0" ||
                                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" ||

                                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberDraftP"].ToString() != "0")
                            {
                                MessageBox.Show(".این کارت تولید دارای حواله و رسید می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                return;

                            }
                             if (table_40_ColorPrductionBindingSource.Count > 0)
                            {
                                if (((DataRowView)table_40_ColorPrductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0")
                                { MessageBox.Show(".این کارت تولید دارای حواله و رسید می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                                }
                            }
                             gridEX2.MoveTo(Position);

                             DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select Id from Table_050_Packaging where IDProduct=" + gridEX2.GetValue("ID"));
                             if (dt.Rows.Count > 0)
                             {
                                 MessageBox.Show(".ازاین کارت تولید در قسمت بسته بندی استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                 return;
                             }

                             if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["EditProduct"].ToString() == "False")
                             {

                                 MessageBox.Show("این کارت تولید عادی می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                 return;
                             
                             }



                             if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ReturnDraftId"].ToString() == "0" ||
                                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0" ||
                                ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberDraftP"].ToString() == "0")
                            {
                              
                                table_035_ProductionBindingSource.EndEdit();
                                table_035_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_035_Production);
                                table_40_ColorPrductionBindingSource.EndEdit();
                                table_40_ColorPrductionTableAdapter.Update(dataSet_05_PCLOR.Table_40_ColorPrduction);
                                table_45_EditeProductBindingSource.EndEdit();
                                table_45_EditeProductTableAdapter.Update(dataSet_05_PCLOR.Table_45_EditeProduct);
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_40_ColorPrduction where FK =" + txt_Id.Text + " ; delete from Table_45_EditeProduct where Fk=" + txt_Id.Text + " ");
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_035_Production where ID=" + txt_Id.Text + " ");
                               
                             
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                                this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                                this.table_45_EditeProductTableAdapter.Fill(this.dataSet_05_PCLOR.Table_45_EditeProduct);
                                this.table_45_EditeProductBindingSource.EndEdit();
                                this.table_45_EditeProductTableAdapter.Update(this.dataSet_05_PCLOR.Table_45_EditeProduct);
                                dataSet_05_PCLOR.EnforceConstraints = true;
                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                btn_DeleteBarcode_Click(sender, e);
                                btn_New.Enabled = true;
                                // gridEX2.Enabled = true;

                            }
//                      
                        }
                    }

                    else

                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
                else { MessageBox.Show("لطفا یک سطر را انتخاب کنید."); }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }
        private void mlt_CodeOrderColor_ValueChanged(object sender, EventArgs e)
        {
//            try
//            {
//                if (mlt_CodeOrderColor.Value.ToString() != "" && mlt_CodeOrderColor.Value != null && mlt_CodeOrderColor.Value != DBNull.Value)
//                {

//                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_030_DetailOrderColor.ID, dbo.Table_025_HederOrderColor.Number, SUM(dbo.Table_030_DetailOrderColor.NumberOrder) AS NumberOrder, 
//                      dbo.Table_010_TypeColor.TypeColor, dbo.Table_005_TypeCloth.TypeCloth, dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_010_TypeColor.ID AS IDColor, 
//                      dbo.Table_030_DetailOrderColor.Machine, dbo.Table_005_TypeCloth.CodeCommondity, dbo.Table_005_TypeCloth.ID AS IDCloth
//                    FROM         dbo.Table_025_HederOrderColor INNER JOIN
//                                          dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk INNER JOIN
//                                          dbo.Table_005_TypeCloth ON dbo.Table_030_DetailOrderColor.TypeColth = dbo.Table_005_TypeCloth.ID INNER JOIN
//                                          dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID
//                    GROUP BY dbo.Table_025_HederOrderColor.Number, dbo.Table_010_TypeColor.TypeColor, dbo.Table_005_TypeCloth.TypeCloth, 
//                                          dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.ID, dbo.Table_010_TypeColor.ID, dbo.Table_030_DetailOrderColor.Machine, 
//                      dbo.Table_005_TypeCloth.CodeCommondity, dbo.Table_005_TypeCloth.ID
//                    HAVING      (dbo.Table_030_DetailOrderColor.ID =" + mlt_CodeOrderColor.Value + ") ");


//                    string s = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT    
//                     dbo.Table_030_DetailOrderColor.NumberOrder - ISNULL(SUM(dbo.Table_035_Production.NumberProduct), 0) AS Remini
//                    FROM         dbo.Table_025_HederOrderColor LEFT OUTER JOIN
//                                          dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk LEFT OUTER JOIN
//                                          dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId
//                    GROUP BY dbo.Table_030_DetailOrderColor.ID
//                    ,dbo.Table_030_DetailOrderColor.NumberOrder
//
//                    HAVING      (dbo.Table_030_DetailOrderColor.ID = " + mlt_CodeOrderColor.Value + ")");

//                    txt_Remining.Text = s;

//                    if (dt.Rows.Count > 0)
//                    {
//                        mlt_TypeCloth.Value = dt.Rows[0][9].ToString();
//                        mlt_TypeColor.Value = dt.Rows[0][6].ToString();
//                        mlt_NameCustomer.Value = dt.Rows[0][5].ToString();
//                        txt_Order.Text = dt.Rows[0][2].ToString();
//                        mlt_Machine.Value = dt.Rows[0][7].ToString();
//                    }
//                    else
//                    {
//                        mlt_TypeCloth.Value = null;
//                        mlt_TypeColor.Value = null;
//                        mlt_NameCustomer.Value = null;
//                        txt_Order.Text = "0";

//                    }


//                }
//                else
//                {
//                    mlt_TypeCloth.Value = null;
//                    mlt_TypeColor.Value = null;
//                    mlt_NameCustomer.Value = null;
//                    txt_Order.Text = "0";
//                    txt_Remining.Text = "0";
//                }


//                //txt_Remining.Text = (Convert.ToInt64(dt1.Rows[0][0].ToString()) - Convert.ToInt64(txt_Order.Text)).ToString();
//            }

//            catch
//            {
//                mlt_TypeCloth.Value = null;
//                mlt_TypeColor.Value = null;
//                mlt_NameCustomer.Value = null;
//                txt_Order.Text = "0";
//                txt_Remining.Text = "0";
//            }
        }
        private void txt_NumberProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { txt_weight.Focus(); }
        }
        private void txt_Description_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { gridEX2.Focus(); }
        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            //{

            //    try
            //    {
            //        dataSet_05_PCLOR.EnforceConstraints = false;
            //        this.table_035_ProductionTableAdapter.FillByNum(this.dataSet_05_PCLOR.Table_035_Production, Convert.ToInt32(txtSearch.Text));

            //    }
            //    catch { }
            //}


        }
        private void Frm_30_Production_KeyDown(object sender, KeyEventArgs e)
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
        private void btn_Insert_Click(object sender, EventArgs e)
        {
           try
            {
              
                if (txt_weight.Text == "0")
                {
                    MessageBox.Show(".لطفا وزن پارچه را وارد کنید");
                    return;
                }

                if (uiComboBox1.Text=="")
                {
                    MessageBox.Show(".لطفا طرح چاپ را انتخاب کنید");
                    return;
                }


                else
                {

                    try
                    {

                        table_035_ProductionBindingSource.EndEdit();

                        if (gridEX1.GetRows().Count() > 0 && DialogResult.Yes == MessageBox.Show("آیا مایل به حذف مواد اولیه و محاسبه مجدد مواد اولیه می باشید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign) && (table_40_ColorPrductionBindingSource.Count > 0))
                        {
                            //if (gridEX1.GetValue("NumberDraft").ToString() != "0" )
                            //{
                            //    MessageBox.Show("این رنگ مصرفی دارای حواله می باشد امکان محاسبه مجدد ندارد");
                            //    return;
                            //}
                            //else{
                            foreach (DataRowView item in table_40_ColorPrductionBindingSource)
                            {
                                item.Delete();
                            }
                        //}
                        }
                        else if (table_40_ColorPrductionBindingSource.Count > 0)
                            return;

                        gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                        DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_015_FormulColor.CodeColore, dbo.Table_015_FormulColor.NumberKilo, dbo.Table_010_TypeColor.ID, dbo.Table_055_ColorDefinition.CodeCommondity,dbo.Table_010_TypeColor.TypeColor, 
                      " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column07 AS Count
                        FROM         dbo.Table_010_TypeColor INNER JOIN
                              dbo.Table_015_FormulColor ON dbo.Table_010_TypeColor.ID = dbo.Table_015_FormulColor.Fk INNER JOIN
                              dbo.Table_055_ColorDefinition ON dbo.Table_015_FormulColor.CodeColore = dbo.Table_055_ColorDefinition.ID INNER JOIN
                              " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                              dbo.Table_055_ColorDefinition.CodeCommondity = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                         WHERE     (dbo.Table_010_TypeColor.ID  = " + mlt_TypeColor.Value + ")");

                        foreach (DataRow Row in dt.Rows)
                        {
                            this.table_40_ColorPrductionBindingSource.AddNew();
                            DataRowView Tb_40 = (DataRowView)table_40_ColorPrductionBindingSource.CurrencyManager.Current;

                            Tb_40["CodeColor"] = Row["CodeColore"];
                            Tb_40["TypeColor"] = Row["ID"];
                            Tb_40["CodeCommondity"] = Row["CodeCommondity"];
                            Decimal Weightt = ((Convert.ToDecimal(txt_weight.Text) * Convert.ToDecimal(Row["NumberKilo"]))) / 100;
                            Tb_40["Consumption"] = Weightt;
                            table_40_ColorPrductionBindingSource.EndEdit();


                        }
                        //gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                       //gridEX2.MoveLast();
                    }
                  
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
                }
            }
           catch (Exception ex)
           {
               Class_BasicOperation.CheckExceptionType(ex, this.Name);
           }
        }
        private void btn_Draft_Colore_Click(object sender, EventArgs e)
        {

            try
            {
                if (table_40_ColorPrductionBindingSource.Count > 0)
                {
                    if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                    {


                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 140))
                        {


                            if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", gridEX1.GetValue("NumberDraft").ToString()) != 0)
                            {
                                throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");


                            }

                            {
                                int Position = gridEX2.CurrentRow.RowIndex;
                                table_035_ProductionBindingSource.EndEdit();
                                //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_008_Child_PwhrsDraft where Column01=" + gridEX1.GetValue("NumberDraft").ToString());
                                //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_007_PwhrsDraft Where ColumnId=" + gridEX1.GetValue("NumberDraft").ToString());
                                //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_40_ColorPrduction set NumberDraft=" + 0 + " Where FK=" + txt_Id.Text);


                                string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_008_Child_PwhrsDraft where Column01 in(
                                                    select NumberDraft from Table_40_ColorPrduction where NumberDraft in (" + gridEX1.GetValue("NumberDraft") + @"))

                                                    Delete from " + ConWare.Database + @".dbo.Table_007_PwhrsDraft where columnid in(
                                                      select NumberDraft from Table_40_ColorPrduction where NumberDraft in( " + gridEX1.GetValue("NumberDraft") + @"))

                                                    Update Table_40_ColorPrduction set NumberDraft=0  where NumberDraft in( " + gridEX1.GetValue("NumberDraft") + ")";
                                Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);



                                MessageBox.Show("حواله با موفقیت حذف گردید");
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                                this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                                this.table_45_EditeProductTableAdapter.Fill(this.dataSet_05_PCLOR.Table_45_EditeProduct);
                                dataSet_05_PCLOR.EnforceConstraints = true;
                                gridEX2.RemoveFilters();
                                gridEX2.MoveTo(Position);
                                table_035_ProductionBindingSource_PositionChanged(sender, e);

                            }

                            //}
                        }
                        else
                        {
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                        }
                    }
                    else { MessageBox.Show("حواله مصرفی را انتخاب کنید."); }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }

        }
        private void btn_Delete_Draft_Click(object sender, EventArgs e)
        {

            try
            {
                if (Convert.ToInt32(gridEX2.CurrentRow.RowIndex) >= 0)
                {

                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 139))
                    {


                        if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", gridEX2.GetValue("ReturnDraftId").ToString()) != 0)
                        {
                            throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");


                        }

                       


                        {



                            table_035_ProductionBindingSource.EndEdit();
                            string number = "";
                            int Position = gridEX2.CurrentRow.RowIndex;
                            //Panel_Barcode.Visible = true;
                            //Panel_Barcode.Visible = false;
                            if (gridEX3.RowCount>0)
                            {
                                
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX3.GetRows())
                            {
                                number += item.Cells["Barcode"].Value.ToString()+",";
                            }

                           string commandtext= "Delete From Table_008_Child_PwhrsDraft where Column01=" + gridEX2.GetValue("ReturnDraftId").ToString() +
                            ";Delete From Table_007_PwhrsDraft Where ColumnId=" + gridEX2.GetValue("ReturnDraftId").ToString() +
                            @";Update " + ConPCLOR.Database + ".dbo.Table_035_Production set ReturnDraftId= 0   Where ID=" + txt_Id.Text+
                            @";Update  " + ConPCLOR.Database + ".dbo.Table_45_EditeProduct set DrafReturnId=0 where Barcode in ("+number.TrimEnd(',')+@")" ;

                           Class_BasicOperation.SqlTransactionMethodScaler(ConWare.ConnectionString,commandtext);
                            MessageBox.Show("حواله با موفقیت حذف گردید");
                            dataSet_05_PCLOR.EnforceConstraints = false;
                            this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                            this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                            this.table_45_EditeProductTableAdapter.Fill(this.dataSet_05_PCLOR.Table_45_EditeProduct);
                            dataSet_05_PCLOR.EnforceConstraints = true;
                            gridEX2.RemoveFilters();
                            gridEX2.MoveTo(Position);
                            table_035_ProductionBindingSource_PositionChanged(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("این کارت تولید دارا حواله مرجوعی نمی باشد");
                                return;
                            }
                        }
                    }
                    else
                    {
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                    }
                }
                else { MessageBox.Show("لطفا یک سطر را انتخاب کنید."); }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }

        }
        private void btn_Delete_Recipt_Click(object sender, EventArgs e)
        {
             
            try
            {
                if (Convert.ToInt32(gridEX2.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 138))
                    {

                        if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", gridEX2.GetValue("NumberRecipt").ToString()) != 0)
                        {
                            throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");
                        }
                        {
                            int Position = gridEX2.CurrentRow.RowIndex;
//                            ClDoc.RunSqlCommand(ConWare.ConnectionString, @"
//                    Delete From Table_012_Child_PwhrsReceipt where Column01=" + gridEX2.GetValue("NumberRecipt").ToString() + @";
//                    Delete From Table_011_PwhrsReceipt Where ColumnId=" + gridEX2.GetValue("NumberRecipt").ToString() + @";
//                    Update " + ConPCLOR.Database + ".dbo.Table_035_Production set NumberRecipt=0  Where ID=" + txt_Id.Text);




                            string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select NumberRecipt from Table_035_Production where Id = " + txt_Id.Text + @")

                                                    Delete from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt where columnid in(
                                                      select NumberRecipt from Table_035_Production where Id = " + txt_Id.Text + @")

                                                    Update Table_035_Production set NumberRecipt=0  where Id = " + txt_Id.Text + "";
                            Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);






                            dataSet_05_PCLOR.EnforceConstraints = false;
                            this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                            this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                            dataSet_05_PCLOR.EnforceConstraints = true;
                            MessageBox.Show("رسید با موفقیت حذف گردید");
                            gridEX2.RemoveFilters();
                            gridEX2.MoveTo(Position);
                            table_035_ProductionBindingSource_PositionChanged(sender, e);
                        }


                    }

                    else
                    {
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                    }
                }
                else { MessageBox.Show("لطفا یک سطر را انتخاب کنید."); }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }

        }
        private void ExportDraft()
        {
            int DraftNumber = 0;
            string DraftID = "0";
            DraftId = new SqlParameter("DraftID", SqlDbType.Int);
            DraftId.Direction = ParameterDirection.Output;
            string CmdText = "";
            string DetailsIdDraft = "";
            //درج کالاهای یک انبار در یک حواله

            DraftNumber = ClDoc.MaxNumber(Properties.Settings.Default.PWHRS, "Table_007_PwhrsDraft", "Column01");

            //درج هدر حواله برای هر انبار
            CmdText = (@"INSERT INTO Table_007_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
            column17, column18, column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26) VALUES(" + DraftNumber + ",'" + txt_Dat.Text + "'," +
                mlt_Ware.Value + "," + mlt_Function.Value + ",0,'" + "حواله صادره بابت رنگ مصرفی ش" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["ID"].ToString() + "',0,'" + Class_BasicOperation._UserName +
                "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0,0,0,0,0,0,null,0,1); SET @DraftID = SCOPE_IDENTITY();");

            foreach (DataRowView item in table_40_ColorPrductionBindingSource)
            {
                CmdText = CmdText + (@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                        column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, column28, column29, Column30, Column31, Column32, Column33, Column36, Column37) VALUES(@DraftID,"
                    + (Convert.ToDecimal(item["CodeCommondity"])).ToString() + ",(select Column07 from table_004_CommodityAndIngredients where Columnid=" + item["CodeCommondity"].ToString() + "),0,0," + (Convert.ToDecimal(item["Consumption"])).ToString() + "," +
                    item["Consumption"].ToString() + ",0,0,0,0,N'به شماره کارت تولید" + txt_Number.Text + "',NULL,NULL,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                    "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0,NULL,NULL,0,0,0,0)");
                DetailsIdDraft = DetailsIdDraft + item["ID"].ToString() + ",";


                //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update table_40_ColorPrduction set NumberDraft=" + DraftID + " where fk=" + txt_Id.Text);

                CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.table_40_ColorPrduction set NumberDraft=@DraftID  Where fk=" + txt_Id.Text;


            }


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

                    try
                    {
                        SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftId, ConWare);
                        DataTable Table = new DataTable();
                        goodAdapter.Fill(Table);

                        //محاسبه ارزش و ذخیره آن در جدول Child1

                        foreach (DataRow item in Table.Rows)
                        {
                            if (Class_BasicOperation._WareType)
                            {
                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value, ConWare);
                                DataTable TurnTable = new DataTable();
                                Adapt.Fill(TurnTable);
                                DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftId + " and DetailID=" + item["Columnid"].ToString());

                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                    + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                UpdateCommand.ExecuteNonQuery();

                            }
                            else
                            {
                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value + ",@Date='" + txt_Dat.Text + "',@id=" + DraftId + ",@residid=0", ConWare);
                                DataTable TurnTable = new DataTable();
                                Adapt.Fill(TurnTable);
                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                              + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                UpdateCommand.ExecuteNonQuery();
                            }

                        }
                    }


                    catch
                    {
                    }


                    gridEX1.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                    Messages = Messages + "حواله انبار رنگ مصرفی به شماره  " + DraftNumber.ToString() + Environment.NewLine;
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
        private void mlt_NameCustomer_Leave(object sender, EventArgs e)
        {
            txt_NumberProduct.Focus();
        }
        private void txt_weight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                mlt_Ware.Focus();
            }
        }
        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {
            mlt_Function.Focus();
        }
        private void mlt_Function_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_Description.Focus();
        }
        private void Frm_30_Production_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //mlt_Ware_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=14");

                //mlt_Function_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=2");

                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=18");

                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=8");

                mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                mlt_Function_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=3");
            }
            catch { }
        }
        private float FirstRemain(int GoodCode, string Ware)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date,dbo.Table_012_Child_PwhrsReceipt.Column37 AS Tamin , dbo.Table_011_PwhrsReceipt.column05 AS Customer
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1}) AND (dbo.Table_012_Child_PwhrsReceipt.column37 = N'{3}') AND (dbo.Table_011_PwhrsReceipt.column05 = {4})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02,dbo.Table_012_Child_PwhrsReceipt.Column37, dbo.Table_011_PwhrsReceipt.column05
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date,dbo.Table_008_Child_PwhrsDraft.Column37 AS Tamin, dbo.Table_007_PwhrsDraft.column05 As Customer
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1}) AND (dbo.Table_008_Child_PwhrsDraft.column37 = '{3}') AND (dbo.Table_008_Child_PwhrsDraft.column05 = {4})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02,dbo.Table_008_Child_PwhrsDraft.Column37, dbo.Table_007_PwhrsDraft.column05) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, Ware, GoodCode, txt_Dat.Text);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
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


        string draftreturn = "";
        //حواله پارچه ها
        private void ExportDraftCloth()
        {
            string DraftID = "0";
            int DraftNumber1 = 0;
            DraftId = new SqlParameter("DraftID", SqlDbType.Int);
            DraftId.Direction = ParameterDirection.Output;
            string CmdText = "";
           
            //درج کالاهای یک انبار در یک حواله
            DraftNumber1 = ClDoc.MaxNumber(Properties.Settings.Default.PWHRS, "Table_007_PwhrsDraft", "Column01");
            //درج هدر حواله برای هر انبار
            CmdText = (@"INSERT INTO Table_007_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, 
        column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                  column17, column18, column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26) VALUES(" + DraftNumber1 + ",'" + txt_Dat.Text + "'," +
           mlt_Return.Value + "," + mlt_TypeRerturn.Value + ",0,'" + "حواله صادره بابت بارکد های مرجوعی - کارت تولید ش" + txt_Number.Text + "',0,'" + Class_BasicOperation._UserName +
           "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0,0,0,0,0,0,null,0,1); SET @DraftID = SCOPE_IDENTITY();");
            int Unit = Convert.ToInt16(ClDoc.ExScalar(ConWare.ConnectionString, @"(select Column07 from table_004_CommodityAndIngredients where Columnid=" +
                ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"].ToString() + ")"));
          
                
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX3.GetRows())
            {
                
            //string weight = ((DataRowView)mlt_CodeOrderColor.DropDownList.FindItem(mlt_CodeOrderColor.Value))["weight"].ToString();
            CmdText = CmdText+(@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04,
        column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27,
        column28, column29, Column30, Column31, Column32, Column33,Column34,column35, Column36, Column37) VALUES(@DraftID,"
            + item.Cells["CodeCommondity"].Value.ToString() + "," + Unit + @",0,0," + item.Cells["Qty"].Value.ToString() + "," +
            item.Cells["Qty"].Value.ToString() + ",0,0,0,0,'N " + txt_Number.Text + "',NULL,NULL,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
            "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0,"+item.Cells["Barcode"].Value.ToString()+",NULL,0,0," + ((Convert.ToDecimal(item.Cells["weight"].Value.ToString())) / Convert.ToInt64(item.Cells["Qty"].Value.ToString())) + "," + (Convert.ToDecimal(item.Cells["weight"].Value.ToString())) + ",N'" +
            item.Cells["TypeColor"].Text + "',N'" +
           item.Cells["Machine"].Text + "')");


            draftreturn += item.Cells["Barcode"].Value.ToString() + ",";


            }
           

            

         
            CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.Table_035_Production set ReturnDraftId=@DraftID  Where ID=" + txt_Id.Text + @";

            Update " + ConPCLOR.Database + ".dbo.Table_45_EditeProduct set DrafReturnId=@DraftID where Barcode in (" + draftreturn.TrimEnd(',') + @")";

          
             
             

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

                    try
                    {

                        SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftId, ConWare);
                        DataTable Table = new DataTable();
                        goodAdapter.Fill(Table);

                        //محاسبه ارزش و ذخیره آن در جدول Child1

                        foreach (DataRow item in Table.Rows)
                        {
                            if (Class_BasicOperation._WareType)
                            {
                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Return.Value, ConWare);
                                DataTable TurnTable = new DataTable();
                                Adapt.Fill(TurnTable);
                                DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftId + " and DetailID=" + item["Columnid"].ToString());

                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                    + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                UpdateCommand.ExecuteNonQuery();

                            }
                            else
                            {
                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Return.Value + ",@Date='" + txt_Dat.Text + "',@id=" + DraftId + ",@residid=0", ConWare);
                                DataTable TurnTable = new DataTable();
                                Adapt.Fill(TurnTable);
                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                              + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                UpdateCommand.ExecuteNonQuery();
                            }

                        }
                    }
                    catch
                    {
                    }

        
               gridEX3.DropDowns["Draft"].DataSource = gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                  
                    Messages = Messages + "حواله انبار پارچه های مرجوعی به شماره " + DraftNumber1.ToString() + Environment.NewLine;

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

        private void ExportOrderColorId()
        {
            try
            {


                string commandtext = @"
                Declare @OrderColorId int
                Declare @DetilId int
                
                ";

                commandtext += @"INSERT INTO Table_025_HederOrderColor (  [Number], [Date], [CodeCustomer] ) 
VALUES ((select isnull(max(Number),0)+1 from Table_025_HederOrderColor),'" + txt_Dat.Text + "'," + mlt_NameCustomer.Value + @"); SET @OrderColorId=Scope_Identity() 
                
INSERT INTO Table_030_DetailOrderColor ([Fk], [TypeColth], [NumberOrder],[TypeColor],[Description],[Machine],[weight],[Printer],[TimeSabt],[UserSabt] ) 
VALUES (@OrderColorId," + mlt_TypeCloth.Value + "," + txt_NumberProduct.Text + "," + mlt_TypeColor.Value + ", N'این سفارش از بارکد های مرجوعی کارت تولید به شماره" 
                        + txt_Number.Text + " صدور شده است'," + mlt_Machine.Value + "," + txt_weight.Text + ",N'" + uiComboBox1.Text + "',getdate(),N'"+Class_BasicOperation._UserName+"')";

                commandtext += @"
                set @DetilId =(select Id from Table_030_DetailOrderColor where Fk=@OrderColorId);
                Update Table_035_Production set ColorOrderId=@DetilId where id= "+txt_Id.Text+"";

                Class_BasicOperation.SqlTransactionMethodScaler(ConPCLOR.ConnectionString, commandtext);

                gridEX2.DropDowns["Number"].DataSource = ClDoc.ReturnTable(ConPCLOR, @" SELECT     dbo.Table_030_DetailOrderColor.Fk, dbo.Table_030_DetailOrderColor.ID, dbo.Table_025_HederOrderColor.Number
                        FROM         dbo.Table_025_HederOrderColor INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk");

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
          
            }
        
        
        
        }

        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        private void btn_Recipt_Click(object sender, EventArgs e)
        {
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
                                    [column11]
                                                                 
                                    ) VALUES (" + ResidNum + ",'" + txt_Dat.Text + "' ," + mlt_Ware_R.Value.ToString() + "," + mlt_Function_R.Value.ToString() + ","
                                              + mlt_NameCustomer.Value.ToString() + ",'" + "رسید صادره بابت کارت تولید ش" + txt_Number.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()); SET @Key=Scope_Identity() ";
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
                                   ,[column34]
                                   ,[column35]
                                   ,[column36]
                                   ,[column37]
                           ) VALUES (@Key," + ((((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["CodeCommondity"]).ToString()) +
                                    ",1," + txt_NumberProduct.Text +
        "," + txt_NumberProduct.Text + ",0,0," + txt_Number.Text + ",'" + Class_BasicOperation._UserName +
        "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + (Convert.ToDecimal(txt_weight.Text) / Convert.ToDecimal(txt_NumberProduct.Text)).ToString() +
        "," + txt_weight.Text + ",N'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "',N'" +
        ((DataRowView)mlt_Machine.DropDownList.FindItem(mlt_Machine.Value))["namemachine"].ToString() + "'); ";

            commandtxt += " select @Key as ReceiptID";

            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand Command = Con.CreateCommand();
                Command.Transaction = sqlTran;

                try
                {
                    Command.CommandText = commandtxt;
                    string ReceiptID = Command.ExecuteScalar().ToString();
                    sqlTran.Commit();
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;


                    ((DataRowView)this.table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"] = ReceiptID;
                    this.dataSet_05_PCLOR.EnforceConstraints = false;
                    table_035_ProductionBindingSource.EndEdit();
                    table_40_ColorPrductionBindingSource.EndEdit();
                    table_035_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_035_Production);
                    table_40_ColorPrductionTableAdapter.Update(dataSet_05_PCLOR.Table_40_ColorPrduction);
                    this.dataSet_05_PCLOR.EnforceConstraints = true;
                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                    // ToastNotification.Show(this, " رسید انبار سالن تولید به شماره " + ResidNum.ToString() + "  با موفقیت صادر شد  ", 3000, eToastPosition.MiddleCenter);
                    Messages = Messages + " رسید انبار سالن تولید به شماره " + ResidNum.ToString()  + Environment.NewLine;
                }


                catch (Exception es)
                {
                    sqlTran.Rollback();
                    this.Cursor = Cursors.Default;
                    throw es;

                }
            }
        }
        private void txt_Number_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }
        private void txt_Remining_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void txt_Order_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    gridEX2.UpdateData();
            //    table_035_ProductionBindingSource.EndEdit();
            //    this.table_40_ColorPrductionBindingSource.EndEdit();


            //    DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select max(Number) from Table_035_Production),0) as Row");
            //    if (Table.Rows[0]["Row"].ToString() != "0")
            //    {
            //        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_035_Production where Number=" + Table.Rows[0]["Row"].ToString());
            //        dataSet_05_PCLOR.EnforceConstraints = false;
            //        this.table_035_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_035_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //        this.table_40_ColorPrductionTableAdapter.FillById(this.dataSet_05_PCLOR.Table_40_ColorPrduction, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //        dataSet_05_PCLOR.EnforceConstraints = true;

            //    }

            //}
            //catch
            //{
            //}
        }
        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            //if (this.table_035_ProductionBindingSource.Count > 0)
            //{

            //    try
            //    {
            //        gridEX2.UpdateData();
            //        table_035_ProductionBindingSource.EndEdit();
            //        this.table_40_ColorPrductionBindingSource.EndEdit();




            //        DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select Min(Number) from Table_035_Production where Number>" + ((DataRowView)this.table_035_ProductionBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
            //        if (Table.Rows[0]["Row"].ToString() != "0")
            //        {
            //            DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_035_Production where Number=" + Table.Rows[0]["Row"].ToString());

            //            dataSet_05_PCLOR.EnforceConstraints = false;
            //            this.table_035_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_035_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //            this.table_40_ColorPrductionTableAdapter.FillById(this.dataSet_05_PCLOR.Table_40_ColorPrduction, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //            dataSet_05_PCLOR.EnforceConstraints = true;

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //    }
            //}
        }
        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            //if (this.table_035_ProductionBindingSource.Count > 0)
            //{
            //    try
            //    {
            //        gridEX2.UpdateData();
            //        table_035_ProductionBindingSource.EndEdit();
            //        this.table_40_ColorPrductionBindingSource.EndEdit();

            //        DataTable Table = ClDoc.ReturnTable(ConPCLOR,
            //            "Select ISNULL((Select max(Number) from Table_035_Production where Number<" +
            //            ((DataRowView)this.table_035_ProductionBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
            //        if (Table.Rows[0]["Row"].ToString() != "0")
            //        {
            //            DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_035_Production where Number=" + Table.Rows[0]["Row"].ToString());
            //            dataSet_05_PCLOR.EnforceConstraints = false;
            //            this.table_035_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_035_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //            this.table_40_ColorPrductionTableAdapter.FillById(this.dataSet_05_PCLOR.Table_40_ColorPrduction, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //            dataSet_05_PCLOR.EnforceConstraints = true;

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //    }
            //}
        }
        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    gridEX2.UpdateData();
            //    table_035_ProductionBindingSource.EndEdit();
            //    this.table_40_ColorPrductionBindingSource.EndEdit();


            //    DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_035_Production),0) as Row");
            //    if (Table.Rows[0]["Row"].ToString() != "0")
            //    {
            //        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_035_Production where Number=" + Table.Rows[0]["Row"].ToString());
            //        dataSet_05_PCLOR.EnforceConstraints = false;
            //        this.table_035_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_035_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //        this.table_40_ColorPrductionTableAdapter.FillById(this.dataSet_05_PCLOR.Table_40_ColorPrduction, long.Parse(RowId.Rows[0]["ID"].ToString()));

            //        dataSet_05_PCLOR.EnforceConstraints = true;

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //}
        }
        private void mlt_Ware_D_ValueChanged(object sender, EventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns(mlt_Ware_D, null, "Number");

        }
        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware, "Column02", "TypeWare");

        }
        private void mlt_Ware_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_R, "Column02", "TypeWare");

        }
        private void mlt_Function_D_ValueChanged(object sender, EventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns(mlt_Function_D, "Column02", "Column01");

        }
        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "Column02", "Column01");

        }
        private void mlt_Function_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_R, "Column02", "Column01");

        }
        private void table_035_ProductionBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0" ||
                    ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" ||
                    ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ReturnDraftId"].ToString() != "0")
                {
                    if (table_40_ColorPrductionBindingSource.Count > 0)
                    {
                        if (((DataRowView)table_40_ColorPrductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0")
                        {
                            uiPanel8Container.Enabled = true;
                           

                        }

                        else
                        {

                            uiPanel8Container.Enabled = false;
                           

                        }
                    }
                    else uiPanel8Container.Enabled = false;


                }
                else uiPanel8Container.Enabled = true;


                if (((DataRowView)table_40_ColorPrductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0")
                {
                    gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                    gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.True;

                }
                else
                {
                    gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                    gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
                }

                /////
//                Int64 order = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT     dbo.Table_030_DetailOrderColor.NumberOrder
//                    FROM         dbo.Table_030_DetailOrderColor INNER JOIN
//                                          dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID
//                    WHERE     (dbo.Table_030_DetailOrderColor.ID = " + mlt_CodeOrderColor.Value + ")),0) as Result"));


//                Int64 Product = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT     SUM(dbo.Table_035_Production.NumberProduct) AS NumberProduct
//                FROM         dbo.Table_035_Production INNER JOIN
//                      dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID
//                    where      (dbo.Table_035_Production.ColorOrderId = " + mlt_CodeOrderColor.Value + ") AND (dbo.Table_035_Production.ID <> " + txt_Id.Text + ")),0)as result"));

//                Int64 Remain = order - Product;
//                txt_Remining.Text = Remain.ToString();
                ////

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

        private void mlt_NameCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_NumberProduct.Focus();

        }

        //private void mlt_Num_Product_ValueChanged(object sender, EventArgs e)
        //{
        //    Product();
        //}
      

         
        
       

//          private void Product()
//          {
//              try
//              {
//                  if (mlt_Num_Product.Text == "")
//                  {
//                      MessageBox.Show("اطلاعات مورد نظر را تکمیل نمایید");
//                  }

//                  else
//                  {


//                      DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_010_TypeColor.TypeColor, 
//                      dbo.Table_030_DetailOrderColor.NumberOrder, SUM(dbo.Table_035_Production.NumberProduct) AS NumberProduct, dbo.Table_030_DetailOrderColor.Title, 
//                      dbo.Table_035_Production.Number, dbo.Table_025_HederOrderColor.Number AS NumberOrde, dbo.Table_035_Production.ID, dbo.Table_030_DetailOrderColor.Printer,
//                       dbo.Table_030_DetailOrderColor.Description, dbo.Table_035_Production.Machine, dbo.Table_035_Production.weight
//FROM         dbo.Table_025_HederOrderColor INNER JOIN
//                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk INNER JOIN
//                      dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId INNER JOIN
//                      dbo.Table_010_TypeColor ON dbo.Table_030_DetailOrderColor.TypeColor = dbo.Table_010_TypeColor.ID
//GROUP BY dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_030_DetailOrderColor.NumberOrder, 
//                      dbo.Table_030_DetailOrderColor.Title, dbo.Table_035_Production.Number, dbo.Table_025_HederOrderColor.Number, dbo.Table_035_Production.ID, 
//                      dbo.Table_030_DetailOrderColor.Printer, dbo.Table_030_DetailOrderColor.Description, dbo.Table_035_Production.Machine, dbo.Table_035_Production.weight, 
//                      dbo.Table_010_TypeColor.TypeColor
//                    HAVING      dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + " ");

//                      if (dt.Rows.Count > 0)
//                      {
//                          mlt_NameCustomer.Value = dt.Rows[0][0].ToString();
//                          mlt_TypeCloth.Value = dt.Rows[0][1].ToString();
//                          mlt_TypeColor.Text = dt.Rows[0][2].ToString();
//                          mlt_CodeOrderColor.Value = dt.Rows[0][7].ToString();
//                          //txt_CountProduct.Text = dt.Rows[0][4].ToString();
//                          //txt_Title.Text = dt.Rows[0][5].ToString();
//                          //txt_ID.Text = dt.Rows[0][8].ToString();
//                          //txt_Title.Text = dt.Rows[0][5].ToString();
//                          //txt_Print.Text = dt.Rows[0][9].ToString();
//                          txt_Description.Text = dt.Rows[0][10].ToString();
//                          mlt_Machine.Value = dt.Rows[0][11].ToString();
//                          txt_weight.Text = dt.Rows[0][12].ToString();
//                         txt_NumberProduct.Text=dt.Rows[0][4].ToString();
////                          string sum = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"SELECT     ISNULL(dbo.Table_035_Production.weight - ISNULL(SUM(dbo.Table_050_Packaging.weight), 0), 0) AS Remin
////                        FROM         dbo.Table_035_Production  left JOIN
////                                  dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
////                         where      (dbo.Table_035_Production.Number = " + mlt_Num_Product.Text + @")
////                                GROUP BY dbo.Table_035_Production.weight");
//                          //txt_Rem_Weight.Text = sum;
//                          //this.table_050_Packaging1TableAdapter.FillByProductID(this.dataSet_05_PCLOR.Table_050_Packaging1, long.Parse(txt_ID.Text));
//                          //txt_weight.Focus();
//                      }
//                  }
//              }

//              catch
//              { }
//          }


        bool flag = false;
        string ReturnBarcode = "";
        string Repate = "";
        bool flagRepate = false;
        float Remain;
        string MsRemain = "";
        string Invalid = "";
        bool flaginvalid = false;
        bool Repeat = false;
        private void button1_Click(object sender, EventArgs e)
        {
           


            string strreplace = System.Text.RegularExpressions.Regex.Replace(txt_Barcode.Text.Trim(), @"\t|\n|\r", "");
            var b = ClDoc.GetNextChars(strreplace.Trim(), 8);
            repeat = "";
            Repate = "";
            MsRemain = "";
            Invalid = "";
            flaginvalid = false;
            flag = false;
            flagRepate = false;
            Repeat = false;
            Messages = string.Empty;

            if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["ReturnDraftId"].ToString() != "0" && ((DataRowView)table_45_EditeProductBindingSource.CurrencyManager.Current)["DrafReturnId"].ToString() != "0")
            {
                MessageBox.Show("این کارت تولید دارای حواله مرجوعی می باشد امکان درج بارکد جدید نمی باشد");
                return;
            }
            foreach (string barcode in b)
            {
                Num = "";
                flag = false;
                ReturnBarcode = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull(( select  ReturnFactor from  Table_050_Packaging  where Barcode=" + barcode + "),0)");
                if (ReturnBarcode == "False")
                {
                    Repate += barcode.Trim(',') + ",";
                    flagRepate = true;
                }

                dtt = ClDoc.ReturnTable(ConPCLOR, @"SELECT        dbo.Table_035_Production.Number, dbo.Table_050_Packaging.Barcode, dbo.Table_035_Production.Machine, dbo.Table_035_Production.ColorOrderId, dbo.Table_050_Packaging.weight, 
                         dbo.Table_025_HederOrderColor.CodeCustomer, dbo.Table_030_DetailOrderColor.TypeColth, dbo.Table_030_DetailOrderColor.TypeColor, dbo.Table_050_Packaging.TypeColor AS txtColor, 
                         dbo.Table_050_Packaging.Machine AS txtMachine, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column02 AS txtName, dbo.Table_005_TypeCloth.TypeCloth AS txtCloth, 
                         dbo.Table_005_TypeCloth.CodeCommondity
FROM            dbo.Table_005_TypeCloth INNER JOIN
                         dbo.Table_030_DetailOrderColor ON dbo.Table_005_TypeCloth.ID = dbo.Table_030_DetailOrderColor.TypeColth RIGHT OUTER JOIN
                         dbo.Table_025_HederOrderColor LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dbo.Table_025_HederOrderColor.CodeCustomer = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId ON 
                         dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID RIGHT OUTER JOIN
                         dbo.Table_035_Production ON dbo.Table_030_DetailOrderColor.ID = dbo.Table_035_Production.ColorOrderId LEFT OUTER JOIN
                         dbo.Table_050_Packaging ON dbo.Table_035_Production.ID = dbo.Table_050_Packaging.IDProduct
WHERE        (dbo.Table_050_Packaging.Barcode = (" + barcode + "))");
               
                   if (dtt.Rows.Count>0)
                {
                    
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX3.GetRows())
                {

                    if (dtt.Rows[0]["Machine"].ToString() != (item.Cells["Machine"].Value).ToString() ||
                        dtt.Rows[0]["CodeCustomer"].ToString() != (item.Cells["Costumer"].Value).ToString() ||
                        dtt.Rows[0]["TypeColth"].ToString() != (item.Cells["TypeCloth"].Value).ToString() ||
                        dtt.Rows[0]["TypeColor"].ToString() != (item.Cells["TypeColor"].Value).ToString())
                        {
                        Messages += (dtt.Rows[0]["Barcode"].ToString() == null ? "" :  dtt.Rows[0]["Barcode"].ToString()) + "," + Environment.NewLine +
                            (dtt.Rows[0]["txtMachine"].ToString() == null ? "" :  dtt.Rows[0]["txtMachine"].ToString()) + "," + Environment.NewLine +
           (dtt.Rows[0]["txtCloth"].ToString() == null ? "" :  dtt.Rows[0]["txtCloth"].ToString()) + "," + Environment.NewLine +
           (dtt.Rows[0]["txtColor"].ToString() == null ? "" : dtt.Rows[0]["txtColor"].ToString()) + "," + Environment.NewLine +
           (dtt.Rows[0]["txtName"].ToString() == null ? "" : dtt.Rows[0]["txtName"].ToString()) + Environment.NewLine ;

                        Repeat = true;
                        break;
                    }

                    if (barcode.ToString() == (item.Cells["Barcode"].Text))
                    {
                        repeat += (item.Cells["Barcode"].Value).ToString() + ",";
                        //flag = true;
                        //break;
                    }

                }
              
                 Remain = FirstRemainBarcode(Convert.ToInt32(dtt.Rows[0]["CodeCommondity"].ToString()), barcode,Convert.ToInt32(mlt_Return.Value.ToString()));

                 if (Remain > 0 && repeat == "" && !Repeat && !flagRepate)
                {
                    table_035_ProductionBindingSource.EndEdit();
                    gridEX3.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                    gridEX3.MoveToNewRecord();
                    gridEX3.SetValue("Fk", txt_Id.Text);
                    gridEX3.SetValue("Barcode", dtt.Rows[0]["Barcode"].ToString());
                    gridEX3.SetValue("Machine", dtt.Rows[0]["Machine"].ToString());
                    gridEX3.SetValue("Costumer", dtt.Rows[0]["CodeCustomer"].ToString());
                    gridEX3.SetValue("TypeCloth", dtt.Rows[0]["TypeColth"].ToString());
                    gridEX3.SetValue("TypeColor", dtt.Rows[0]["TypeColor"].ToString());
                    gridEX3.SetValue("weight", dtt.Rows[0]["weight"].ToString());
                    gridEX3.SetValue("Qty", 1);
                    gridEX3.SetValue("CodeCommondity", dtt.Rows[0]["CodeCommondity"].ToString());

                    //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_050_Packaging set DraftReturnFasctor=1 where Barcode=" + barcode + ""); 
                    gridEX3.UpdateData();
                    gridEX3.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;

                    txt_weight.Text = gridEX3.GetTotalRow().Cells["weight"].Value.ToString();
                    txt_NumberProduct.Text = gridEX3.GetTotalRow().Cells["Qty"].Value.ToString();
                    mlt_Machine.Value = gridEX3.GetValue("Machine");
                    mlt_TypeCloth.Value = gridEX3.GetValue("TypeCloth");
                    mlt_TypeColor.Value = gridEX3.GetValue("TypeColor");
                    mlt_NameCustomer.Value = gridEX3.GetValue("Costumer");
 
                }

                else if (Remain <= 0)
                {
                    MsRemain += barcode.Trim(',') + ",";
                }
              
               

            }
                else
                {
                    flaginvalid = true;
                    Invalid += barcode.Trim(',') + ",";
                   
                }
               

           }

                if(flaginvalid)
                {

                    MessageBox.Show("این بارکد نا معتبر می باشد" + Environment.NewLine + Invalid);
                    return;
                }

                else if (flagRepate)
            {
                MessageBox.Show("این بارکد جزء طاقه مرجوعی نمی باشد" + Environment.NewLine + Repate);
                return;
            }

            else if (repeat != "" )
            {
                MessageBox.Show("این بارکد ها تکراری می باشند " + Environment.NewLine + repeat);
                return;
            }
                else if (Messages != "" && Repeat )
            {
                MessageBox.Show("مغایرت  هایی که در بارکد ها وجود دارد" + Environment.NewLine + Messages);
                return;
            }
            else if (Remain <= 0)
            {
                MessageBox.Show("موجودی بارکد ها کافی نمی باشند" + Environment.NewLine + MsRemain);
                return;
            }


            //if (mlt_Machine.Text == "" && mlt_TypeColor.Text == "" && mlt_TypeCloth.Text == "" && txt_weight.Text == "0" && txt_NumberProduct.Text == "0" && mlt_NameCustomer.Text == "")
            //{

            
            //}



            table_035_ProductionBindingSource.EndEdit();
            table_45_EditeProductBindingSource.EndEdit();
            table_035_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_035_Production);
            table_45_EditeProductTableAdapter.Update(dataSet_05_PCLOR.Table_45_EditeProduct);


        }
         

      


        private float FirstRemainBarcode(int GoodCode, string Barcode,int wear)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
            {
                Con.Open();
                string CommandText = @"SELECT				 ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date , dbo.Table_012_Child_PwhrsReceipt.Column30 AS Grease
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE       (dbo.Table_012_Child_PwhrsReceipt.column02 = {0}) AND (dbo.Table_011_PwhrsReceipt.column03={3})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.Column30
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date, dbo.Table_008_Child_PwhrsDraft.Column30 AS Grease
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_008_Child_PwhrsDraft.column02 = {0})  AND (dbo.Table_007_PwhrsDraft.column03={3})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.Column30) AS derivedtbl_1
                       WHERE     (Date <= '{1}' and Grease={2} ) ";
                CommandText = string.Format(CommandText, GoodCode, txt_Dat.Text, Barcode,wear);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }



        private void uiPanel8_Click(object sender, EventArgs e)
        {

        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {

                this.dataSet_05_PCLOR.EnforceConstraints = false;
                table_035_ProductionBindingSource.EndEdit();
                table_40_ColorPrductionBindingSource.EndEdit();
                table_035_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_035_Production);
                table_40_ColorPrductionTableAdapter.Update(dataSet_05_PCLOR.Table_40_ColorPrduction);
                this.table_035_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_035_Production);
                this.table_40_ColorPrductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_40_ColorPrduction);
                this.dataSet_05_PCLOR.EnforceConstraints = true;
            }
            catch { }
        }

        private void btn_DeleteBarcode_Click(object sender, EventArgs e)
        {

          
            table_45_EditeProductTableAdapter.FillById(dataSet_05_PCLOR.Table_45_EditeProduct, int.Parse(txt_Id.Text));
           
        }

     


      
       
        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

            
            if (e.Column.Key == "CodeColor")
            {
                FilterGridExDropDown(sender, "ID", "NameColor", gridEX1.EditTextBox.Text);
            }
        }



        void FilterGridExDropDown(object sender, string ColumnName, string TextualText, string SearchText)
        {
            try
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                ConditionOperator.Contains, SearchText);
                ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
            }

            catch { }
        }

        private void gridEX3_DeletingRecords(object sender, CancelEventArgs e)
        {



            if (((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" || ((DataRowView)table_45_EditeProductBindingSource.CurrencyManager.Current)["DrafReturnId"].ToString() != "0" || ((DataRowView)table_035_ProductionBindingSource.CurrencyManager.Current)["NumberDraftP"].ToString() != "0")
            {
                
                e.Cancel = true;
                MessageBox.Show(".به دلیل صدور حواله و رسید برای این کارت تولید امکان حذف بارکد را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            
            }

          

            //txt_weight.Text = "0";
            //txt_NumberProduct.Text = "0";
            //mlt_TypeCloth.Text = "";
            //txt_Barcode.Text = "";
            //mlt_TypeColor.Text = "";
            //mlt_Machine.Text = "";
            //mlt_NameCustomer.Text = "";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataSet_05_PCLOR.EnforceConstraints = false;
            table_035_ProductionBindingSource.EndEdit();
            table_035_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_035_Production);
            table_40_ColorPrductionBindingSource.EndEdit();
            table_40_ColorPrductionTableAdapter.Update(dataSet_05_PCLOR.Table_40_ColorPrduction);
            table_45_EditeProductBindingSource.EndEdit();
            table_45_EditeProductTableAdapter.Update(dataSet_05_PCLOR.Table_45_EditeProduct);
            dataSet_05_PCLOR.EnforceConstraints = true;
            MessageBox.Show("تغییرات با موفقیت ذخیره شد");

        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report.mrt");
            r.Design();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            uiComboBox1.Items.Clear();
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report.mrt");
            foreach (StiPage page in r.Pages)
            {
                uiComboBox1.Items.Add(page.Name);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Frm_30_Production_Load(sender, e);
            btn_New.Enabled = true;
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                gridEX1.CurrentCellDroppedDown = true;

                if (e.Column.Key == "CodeColor")
                {
                    gridEX1.SetValue("CodeCommondity", gridEX1.GetValue("CodeColor").ToString());

                }

               
            }
            catch { }
        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            try
            {
                table_035_ProductionBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

    }
}
