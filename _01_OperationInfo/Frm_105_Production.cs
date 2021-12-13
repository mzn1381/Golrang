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
    public partial class Frm_105_Production : Form
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

        int clothTypeID = 0;
        int commodityCode = -1;
        int customerID = 52;
        string machineName = string.Empty;
        int machineID = -1;

        DataTable details = new DataTable();

        public Frm_105_Production()
        {
            InitializeComponent();
        }
        private void Frm_105_Production_Load(object sender, EventArgs e)
        {
            this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
            this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);

            try
            {

                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");

                gridEX1.DropDowns["Draft"].DataSource = gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
                gridEX2.DropDowns["Number"].DataSource =
                    ClDoc.ReturnTable(ConPCLOR, @" SELECT     dbo.Table_030_DetailOrderColor.Fk, dbo.Table_030_DetailOrderColor.ID, dbo.Table_025_HederOrderColor.Number
                        FROM         dbo.Table_025_HederOrderColor INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk");

                gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                gridEX1.DropDowns["Color"].DataSource = mlt_TypeColor.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeColor from Table_010_TypeColor");
                gridEX1.DropDowns["Commodity2"].DataSource = gridEX1.DropDowns["Commodity"].DataSource = ClDoc.ReturnTable(ConWare, @"select Columnid, Column01,Column02,column07 from table_004_CommodityAndIngredients");
                ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");

                mlt_Ware_D.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 1)");


                mlt_Ware_R.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 2)");


                mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 5)");


                mlt_Function_D.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");
                mlt_Function_R.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Function.DataSource = ClDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1");


                mlt_Ware_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=31");

                mlt_Function_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=32");

                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=18");

                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=8");

                mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                mlt_Function_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=3");

                gridEX1.DropDowns["mount"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id ,NameColor from Table_055_ColorDefinition");
                gridEX1.DropDowns["ColorM"].DataSource = ClDoc.ReturnTable(ConPCLOR, @" select Id,NameColor from Table_055_ColorDefinition");

                this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
                this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);



                ToastNotification.ToastForeColor = Color.Black;
                ToastNotification.ToastBackColor = Color.SkyBlue;
                //Table_105_ProductionBindingSource_PositionChanged(sender, e);

            }
            catch { }
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 149))
            {
                table_105_ProductionBindingSource.AddNew();
                //table_115_ProductionColorBindingSource.AddNew();
                txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();
                txt_Dat.Focus();
                txt_weight.Text = "0";
                txt_NumberProduct.Text = "0";
                btn_New.Enabled = false;
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Dat.Text) || txt_Dat.Text.Length < 10)
            {
                MessageBox.Show("تاریخ را وارد نمایید");
                return;
            }
            if (mlt_TypeColor.SelectedItem == null)
            {
                MessageBox.Show("نوع رنگ را انتخاب نمایید");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_Barcode.Text))
            {
                MessageBox.Show("تاریخ را وارد نمایید");
                return;
            }

            int Position = gridEX2.CurrentRow.RowIndex;

            Messages = "";
            try
            {
                if (gridEX1.RowCount > 0)
                {


                    #region برگه ولید هست یا نه
                    if (mlt_Ware.Text == "" || mlt_Ware_D.Text == "" || mlt_Ware_R.Text == "" || mlt_Function.Text == "" || mlt_Function_D.Text == "" || mlt_Function_R.Text == "" || txt_NumberProduct.Text == "")
                    {
                        throw new Exception("اطلاعات مورد نظر را تکمیل نمایید");
                    }

                    #endregion

                    if (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["ID"].ToString().StartsWith("-"))
                    {
                        ///چک کردن باقی مانده کالا
                        //table_105_ProductionBindingSource_PositionChanged(sender, e);
                    }

                    ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["TypeCloth"] = clothTypeID;
                    ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["MachineID"] = machineID;


                    /////
                    bool Sodorhavelmasrafi = true;
                    foreach (DataRowView Row in table_115_ProductionColorBindingSource)
                    {
                        if (Row["NumberDraft"].ToString() == "0")
                        {
                            Sodorhavelmasrafi = false;
                            break;
                        }
                    }

                    if ((((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0") && (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0") && (Sodorhavelmasrafi == false))
                    {
                        IEnumerable<string> lines = txt_Barcode.Lines.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => "'" + x + "'");

                        if (lines.Count().Equals(0))
                        {
                            txt_NumberProduct.Text = "0";
                            txt_weight.Text = "0";

                            clothTypeID = -1;
                            commodityCode = -1;
                            machineID = -1;
                            machineName = string.Empty;

                            MessageBox.Show("سریال ها را وارد کنید");
                            txt_Barcode.Focus();
                            return;
                        }

                        string fields = string.Join(", ", lines);

                        string command = string.Format("select Serial, Weight, Machine from Table_100_Knitting where Serial in ({ 0})", fields);
                        var tbl_serial = ClDoc.ReturnTable(ConPCLOR, command);

                        if(tbl_serial.Rows.Count != lines.Count())
                        {
                            MessageBox.Show("سریال های وارد شده معتبر نمی باشد");
                            txt_Barcode.Focus();
                            return;
                        }

                        command = string.Format("select COUNT(1), SUM(tbl.Weight), tbl.TypeCloth, (select TypeCloth from Table_005_TypeCloth where ID=tbl.TypeCloth), (select CodeCommondity from Table_005_TypeCloth where ID=tbl.TypeCloth), tbl.Machine, (select namemachine from Table_60_SpecsTechnical where ID=tbl.Machine) MachineName from Table_100_Knitting tbl where Serial in ({0}) group by tbl.TypeCloth, tbl.Machine", fields);
                        DataTable dt = ClDoc.ReturnTable(ConPCLOR, command);

                        if (dt.Rows.Count.Equals(0))
                        {
                            MessageBox.Show("سریال های وارد شده معتبر نمی باشد");
                            txt_Barcode.Focus();
                            return;
                        }

                        if (dt.Rows.Count > 1)
                        {
                            MessageBox.Show("سریال های وارد شده باید از یک نوع و یک دستگاه باشند");
                            txt_Barcode.Focus();
                            return;
                        }

                        table_105_ProductionBindingSource.EndEdit();
                        table_105_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_105_Production);
                        table_115_ProductionColorBindingSource.EndEdit();
                        table_115_ProductionColorTableAdapter.Update(dataSet_05_PCLOR.Table_115_ProductionColor);

                        var id = ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["ID"];
                        command = "insert into dbo.Table_110_ProductionDetail (FK, Serial, Weight, Machine) values " + Environment.NewLine;
                        int count = tbl_serial.Rows.Count;
                        int i = 1;

                        foreach (DataRow item in tbl_serial.Rows)
                        {
                            command += string.Format("({0}, '{1}', {2}, {3})", id, item["Serial"].ToString(), item["Weight"].ToString(), item["Machine"].ToString());

                            if (i < count)
                                command += ", " + Environment.NewLine;
                            i++;
                        }

                        Class_BasicOperation.SqlTransactionMethodScaler(ConPCLOR.ConnectionString, command);

                    }


                    #region بررسی موجودی برای حواله هدر
                    if (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0")
                    {
                        if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware_D.Value.ToString()), commodityCode))
                            throw new Exception("کالای " + commodityCode.ToString() + " در این انبار فعال نمی باشد ");

                        //float Remain = FirstRemain(commodityCode, mlt_Ware_D.Value.ToString(), machineName, customerName);
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
                                                                                   WHERE  ColumnId = " + commodityCode.ToString() + @"
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
                        //if (Remain < float.Parse(txt_NumberProduct.Text))
                        //{
                        //    if (!mojoodimanfi)
                        //    {


                        //        good1 += ClDoc.ExScalar(ConWare.ConnectionString,
                        //           "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                        //          commodityCode.ToString());

                        //        Brand += "'" + mlt_TypeColor.Text + "'";
                        //        Tamin += "'" + machineName + "'";
                        //        throw new Exception("عدم موجودی کالای : " + good1 + Environment.NewLine + "برند : " + Brand + Environment.NewLine + "دستگاه : " + Tamin);

                        //    }
                        //}
                    }
                    #endregion

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
                    if ((((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0"))
                    {
                        if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Ware_D.Text.All(char.IsDigit) || mlt_Function_D.Text.All(char.IsDigit))
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
                    if (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0")
                    {
                        if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Ware_D.Text.All(char.IsDigit) || mlt_Function_D.Text.All(char.IsDigit))
                        {
                            MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                            return;
                        }
                        else
                        {
                            ExportDraftCloth();

                        }
                    }
                    #endregion

                    #region صدور حواله مواد مصرفی در صورتی که حواله صادر نشده باشد
                    if (!Sodorhavelmasrafi)
                    {
                        if (mlt_Ware.Text.All(char.IsDigit) || mlt_Ware_R.Text.All(char.IsDigit) || mlt_Function.Text.All(char.IsDigit) || mlt_Function_R.Text.All(char.IsDigit) || mlt_Ware_D.Text.All(char.IsDigit) || mlt_Function_D.Text.All(char.IsDigit))
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
                                this.table_115_ProductionColorBindingSource.AddNew();
                                DataRowView Tb_40 = (DataRowView)table_115_ProductionColorBindingSource.CurrencyManager.Current;

                                Tb_40["CodeColor"] = Row["CodeColore"];
                                Tb_40["TypeColor"] = Row["ID"];
                                Tb_40["CodeCommondity"] = Row["CodeCommondity"];
                                Decimal Weightt = ((Convert.ToDecimal(txt_weight.Text) * Convert.ToDecimal(Row["NumberKilo"]))) / 100;
                                Tb_40["Consumption"] = Weightt;
                                table_115_ProductionColorBindingSource.EndEdit();


                            }
                        }
                    }
                    #endregion
                    this.dataSet_05_PCLOR.EnforceConstraints = false;
                    //table_105_ProductionBindingSource.EndEdit();
                    //table_115_ProductionColorBindingSource.EndEdit();
                    //table_105_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_105_Production);
                    //table_115_ProductionColorTableAdapter.Update(dataSet_05_PCLOR.Table_115_ProductionColor);

                    this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
                    this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);
                    this.dataSet_05_PCLOR.EnforceConstraints = true;
                    gridEX2.MoveTo(Position);
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد" + Environment.NewLine +
                    Messages);
                    gridEX2.Enabled = true;
                    btn_New.Enabled = true;
                    table_105_ProductionBindingSource_PositionChanged(sender, e);
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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 150))
                    {

                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {

                            if (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0" ||
                                ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" ||

                                ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraftP"].ToString() != "0")
                            {
                                MessageBox.Show(".این کارت تولید دارای حواله و رسید می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                return;

                            }
                            if (table_115_ProductionColorBindingSource.Count > 0)
                            {
                                if (((DataRowView)table_115_ProductionColorBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0")
                                {
                                    MessageBox.Show(".این کارت تولید دارای حواله و رسید می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select Id from Table_050_Packaging where IDProduct=" + gridEX2.GetValue("ID"));
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show(".ازاین کارت تولید در قسمت بسته بندی استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0" ||
                               ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() == "0" ||
                               ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraftP"].ToString() == "0")
                            {
                                int Position = gridEX2.CurrentRow.RowIndex;
                                table_105_ProductionBindingSource.EndEdit();
                                table_105_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_105_Production);
                                table_115_ProductionColorBindingSource.EndEdit();
                                table_115_ProductionColorTableAdapter.Update(dataSet_05_PCLOR.Table_115_ProductionColor);

                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_110_ProductionDetail where FK =" + txt_Id.Text + "");
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_115_ProductionColor where FK =" + txt_Id.Text + "");
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_105_Production where ID=" + txt_Id.Text + "");
                                //dataSet_05_PCLOR.EnforceConstraints = true;
                                // btn_Draft_Colore_Click(sender, e);
                                //btn_Delete_Recipt_Click(sender, e);
                                //btn_Delete_Draft_Click(sender, e);
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
                                this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);
                                dataSet_05_PCLOR.EnforceConstraints = true;


                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                btn_New.Enabled = true;
                                // gridEX2.Enabled = true;

                            }

                            //try
                            //{
                            //    Int64 order = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT     dbo.Table_030_DetailOrderColor.NumberOrder
                            //                    FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                            //                                          dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID
                            //                    WHERE     (dbo.Table_030_DetailOrderColor.ID = " + mlt_CodeOrderColor.Value + ")),0) as Result"));


                            //    Int64 Product = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT     SUM(dbo.Table_105_Production.NumberProduct) AS NumberProduct
                            //FROM         dbo.Table_105_Production INNER JOIN
                            //                      dbo.Table_030_DetailOrderColor ON dbo.Table_105_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID
                            //                    where      (dbo.Table_105_Production.ColorOrderId = " + mlt_CodeOrderColor.Value + ") AND (dbo.Table_105_Production.ID <> " + txt_Id.Text + ")),0)as result"));

                            //    Int64 Remain = order - Product;
                            //    //txt_Remining.Text = Remain.ToString();
                            //}
                            //catch { }
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
            //        this.table_105_ProductionTableAdapter.FillByNum(this.dataSet_05_PCLOR.Table_105_Production, Convert.ToInt32(txtSearch.Text));

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
                ///چک کردن باقی مانده کالا
                table_105_ProductionBindingSource_PositionChanged(sender, e);

                //if (Convert.ToInt32(txt_NumberProduct.Text) > Convert.ToInt32(txt_Remining.Text))
                //{
                //    throw new Exception(" تعداد سفارش وارد شده بیشتر از باقی مانده می باشد");
                //}
                ////
                if (txt_weight.Text == "0")
                {
                    MessageBox.Show(".لطفا وزن پارچه را وارد کنید");
                }
                else
                {

                    try
                    {

                        table_105_ProductionBindingSource.EndEdit();

                        if (gridEX1.GetRows().Count() > 0 && DialogResult.Yes == MessageBox.Show("آیا مایل به حذف مواد اولیه و محاسبه مجدد مواد اولیه می باشید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign) && (table_115_ProductionColorBindingSource.Count > 0))
                        {
                            //if (gridEX1.GetValue("NumberDraft").ToString() != "0" )
                            //{
                            //    MessageBox.Show("این رنگ مصرفی دارای حواله می باشد امکان محاسبه مجدد ندارد");
                            //    return;
                            //}
                            //else{
                            foreach (DataRowView item in table_115_ProductionColorBindingSource)
                            {
                                item.Delete();
                            }
                            //}
                        }
                        else if (table_115_ProductionColorBindingSource.Count > 0)
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
                            this.table_115_ProductionColorBindingSource.AddNew();
                            DataRowView Tb_40 = (DataRowView)table_115_ProductionColorBindingSource.CurrencyManager.Current;

                            Tb_40["CodeColor"] = Row["CodeColore"];
                            Tb_40["TypeColor"] = Row["ID"];
                            Tb_40["CodeCommondity"] = Row["CodeCommondity"];
                            Decimal Weightt = ((Convert.ToDecimal(txt_weight.Text) * Convert.ToDecimal(Row["NumberKilo"]))) / 100;
                            Tb_40["Consumption"] = Weightt;
                            table_115_ProductionColorBindingSource.EndEdit();
                        }
                        gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
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
                if (table_115_ProductionColorBindingSource.Count > 0)
                {
                    if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                    {

                        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 150))
                        {


                            if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", gridEX1.GetValue("NumberDraft").ToString()) != 0)
                            {
                                throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");


                            }

                            {
                                int Position = gridEX2.CurrentRow.RowIndex;
                                table_105_ProductionBindingSource.EndEdit();
                                ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_008_Child_PwhrsDraft where Column01=" + gridEX1.GetValue("NumberDraft").ToString());
                                ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_007_PwhrsDraft Where ColumnId=" + gridEX1.GetValue("NumberDraft").ToString());
                                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_115_ProductionColor set NumberDraft=" + 0 + " Where FK=" + txt_Id.Text);
                                MessageBox.Show("حواله با موفقیت حذف گردید");
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
                                this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);
                                dataSet_05_PCLOR.EnforceConstraints = true;
                                gridEX2.RemoveFilters();
                                gridEX2.MoveTo(Position);
                                table_105_ProductionBindingSource_PositionChanged(sender, e);

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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 150))
                    {


                        if (ClDoc.OperationalColumnValue("Table_007_PwhrsDraft", "Column07", gridEX2.GetValue("NumberDraft").ToString()) != 0)
                        {
                            throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");


                        }



                        {
                            table_105_ProductionBindingSource.EndEdit();

                            int Position = gridEX2.CurrentRow.RowIndex;

                            ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_008_Child_PwhrsDraft where Column01=" + gridEX2.GetValue("NumberDraft").ToString() +
                            ";Delete From Table_007_PwhrsDraft Where ColumnId=" + gridEX2.GetValue("NumberDraft").ToString() +
                            @";Update " + ConPCLOR.Database + ".dbo.Table_105_Production set NumberDraft=" + 0 + " Where ID=" + txt_Id.Text);

                            MessageBox.Show("حواله با موفقیت حذف گردید");
                            dataSet_05_PCLOR.EnforceConstraints = false;
                            this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
                            this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);
                            dataSet_05_PCLOR.EnforceConstraints = true;
                            gridEX2.RemoveFilters();
                            gridEX2.MoveTo(Position);
                            table_105_ProductionBindingSource_PositionChanged(sender, e);

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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 150))
                    {

                        if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", gridEX2.GetValue("NumberRecipt").ToString()) != 0)
                        {
                            throw new Exception(" حواله این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");
                        }
                        {
                            int Position = gridEX2.CurrentRow.RowIndex;
                            ClDoc.RunSqlCommand(ConWare.ConnectionString, @"
                    Delete From Table_012_Child_PwhrsReceipt where Column01=" + gridEX2.GetValue("NumberRecipt").ToString() + @";
                    Delete From Table_011_PwhrsReceipt Where ColumnId=" + gridEX2.GetValue("NumberRecipt").ToString() + @";
                    Update " + ConPCLOR.Database + ".dbo.Table_105_Production set NumberRecipt=0  Where ID=" + txt_Id.Text);

                            dataSet_05_PCLOR.EnforceConstraints = false;
                            this.table_105_ProductionTableAdapter.Fill(this.dataSet_05_PCLOR.Table_105_Production);
                            this.table_115_ProductionColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_115_ProductionColor);
                            dataSet_05_PCLOR.EnforceConstraints = true;
                            MessageBox.Show("رسید با موفقیت حذف گردید");
                            gridEX2.RemoveFilters();
                            gridEX2.MoveTo(Position);
                            table_105_ProductionBindingSource_PositionChanged(sender, e);
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

            foreach (DataRowView item in table_115_ProductionColorBindingSource)
            {
                CmdText = CmdText + (@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                        column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, column28, column29, Column30, Column31, Column32, Column33, Column36, Column37) VALUES(@DraftID,"
                    + (Convert.ToDecimal(item["CodeCommondity"])).ToString() + ",(select Column07 from table_004_CommodityAndIngredients where Columnid=" + item["CodeCommondity"].ToString() + "),0,0," + (Convert.ToDecimal(item["Consumption"])).ToString() + "," +
                    item["Consumption"].ToString() + ",0,0,0,0,N'به شماره کارت تولید" + txt_Number.Text + "',NULL,NULL,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                    "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0,NULL,NULL,0,0,0,0)");
                DetailsIdDraft = DetailsIdDraft + item["ID"].ToString() + ",";


                //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_115_ProductionColor set NumberDraft=" + DraftID + " where fk=" + txt_Id.Text);

                CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.Table_115_ProductionColor set NumberDraft=@DraftID  Where fk=" + txt_Id.Text;


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


                    catch (Exception ex)
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
                mlt_Ware_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=31");

                mlt_Function_D.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=32");

                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=18");

                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=8");

                mlt_Ware_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=15");

                mlt_Function_R.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=3");
            }
            catch { }
        }
        private float FirstRemain(int GoodCode, string Ware, string Machin, string customer)
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
                //CommandText = string.Format(CommandText, Ware, GoodCode, txt_Dat.Text,mlt_Machine.Text,mlt_NameCustomer.Value);
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
           mlt_Ware_D.Value + "," + mlt_Function_D.Value + ",0,'" + "حواله صادره بابت کارت تولید ش" + txt_Number.Text + "',0,'" + Class_BasicOperation._UserName +
           "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0,0,0,0,0,0,null,0,1); SET @DraftID = SCOPE_IDENTITY();");
            int Unit = Convert.ToInt16(ClDoc.ExScalar(ConWare.ConnectionString, @"(select Column07 from table_004_CommodityAndIngredients where Columnid=" +
                commodityCode.ToString() + ")"));

            //string weight = ((DataRowView)mlt_CodeOrderColor.DropDownList.FindItem(mlt_CodeOrderColor.Value))["weight"].ToString();
            CmdText = CmdText + (@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04,
        column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
                column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27,
        column28, column29, Column30, Column31, Column32, Column33,Column34,column35, Column36, Column37) VALUES(@DraftID,"
            + commodityCode.ToString() + "," + Unit + @",0,0," + txt_NumberProduct.Text + "," +
            txt_NumberProduct.Text + ",0,0,0,0,'N " + txt_Number.Text + "',NULL,NULL,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
            "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0,NULL,NULL,0,0," + ((Convert.ToDecimal(txt_weight.Text)) / Convert.ToInt64(txt_NumberProduct.Text)) + "," + (Convert.ToDecimal(txt_weight.Text)) + ",N'" +
            ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "',N'" +
            machineName.ToString() + "')");


            CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.Table_105_Production set NumberDraft=@DraftID  Where ID=" + txt_Id.Text + "";




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
                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware_D.Value, ConWare);
                                DataTable TurnTable = new DataTable();
                                Adapt.Fill(TurnTable);
                                DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftId + " and DetailID=" + item["Columnid"].ToString());

                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                    + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                UpdateCommand.ExecuteNonQuery();

                            }
                            else
                            {
                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware_D.Value + ",@Date='" + txt_Dat.Text + "',@id=" + DraftId + ",@residid=0", ConWare);
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


                    gridEX2.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");

                    Messages = Messages + "حواله انبار بافندگی به شماره " + DraftNumber1.ToString() + Environment.NewLine;

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
                                              + customerID.ToString() + ",'" + "رسید صادره بابت کارت تولید ش" + txt_Number.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()); SET @Key=Scope_Identity() ";
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
                           ) VALUES (@Key," + (commodityCode.ToString()) +
                                    ",1," + txt_NumberProduct.Text +
        "," + txt_NumberProduct.Text + ",0,0," + txt_Number.Text + ",'" + Class_BasicOperation._UserName +
        "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + (Convert.ToDecimal(txt_weight.Text) / Convert.ToDecimal(txt_NumberProduct.Text)).ToString() +
        "," + txt_weight.Text + ",N'" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["TypeColor"].ToString() + "',N'" +
        machineName.ToString() + "'); ";

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


                    ((DataRowView)this.table_105_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"] = ReceiptID;
                    this.dataSet_05_PCLOR.EnforceConstraints = false;
                    table_105_ProductionBindingSource.EndEdit();
                    table_115_ProductionColorBindingSource.EndEdit();
                    table_105_ProductionTableAdapter.Update(dataSet_05_PCLOR.Table_105_Production);
                    table_115_ProductionColorTableAdapter.Update(dataSet_05_PCLOR.Table_115_ProductionColor);
                    this.dataSet_05_PCLOR.EnforceConstraints = true;
                    gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                    // ToastNotification.Show(this, " رسید انبار سالن تولید به شماره " + ResidNum.ToString() + "  با موفقیت صادر شد  ", 3000, eToastPosition.MiddleCenter);
                    Messages = Messages + " رسید انبار سالن تولید به شماره " + ResidNum.ToString() + Environment.NewLine;
                }


                catch (Exception es)
                {
                    sqlTran.Rollback();
                    this.Cursor = Cursors.Default;
                    throw es;

                }
            }
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
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
            //    table_105_ProductionBindingSource.EndEdit();
            //    this.table_115_ProductionColorBindingSource.EndEdit();


            //    DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select max(Number) from Table_105_Production),0) as Row");
            //    if (Table.Rows[0]["Row"].ToString() != "0")
            //    {
            //        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_105_Production where Number=" + Table.Rows[0]["Row"].ToString());
            //        dataSet_05_PCLOR.EnforceConstraints = false;
            //        this.table_105_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_105_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //        this.table_115_ProductionColorTableAdapter.FillById(this.dataSet_05_PCLOR.Table_115_ProductionColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //        dataSet_05_PCLOR.EnforceConstraints = true;

            //    }

            //}
            //catch
            //{
            //}
        }
        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            //if (this.table_105_ProductionBindingSource.Count > 0)
            //{

            //    try
            //    {
            //        gridEX2.UpdateData();
            //        table_105_ProductionBindingSource.EndEdit();
            //        this.table_115_ProductionColorBindingSource.EndEdit();




            //        DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select Min(Number) from Table_105_Production where Number>" + ((DataRowView)this.table_105_ProductionBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
            //        if (Table.Rows[0]["Row"].ToString() != "0")
            //        {
            //            DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_105_Production where Number=" + Table.Rows[0]["Row"].ToString());

            //            dataSet_05_PCLOR.EnforceConstraints = false;
            //            this.table_105_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_105_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //            this.table_115_ProductionColorTableAdapter.FillById(this.dataSet_05_PCLOR.Table_115_ProductionColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
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
            //if (this.table_105_ProductionBindingSource.Count > 0)
            //{
            //    try
            //    {
            //        gridEX2.UpdateData();
            //        table_105_ProductionBindingSource.EndEdit();
            //        this.table_115_ProductionColorBindingSource.EndEdit();

            //        DataTable Table = ClDoc.ReturnTable(ConPCLOR,
            //            "Select ISNULL((Select max(Number) from Table_105_Production where Number<" +
            //            ((DataRowView)this.table_105_ProductionBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
            //        if (Table.Rows[0]["Row"].ToString() != "0")
            //        {
            //            DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_105_Production where Number=" + Table.Rows[0]["Row"].ToString());
            //            dataSet_05_PCLOR.EnforceConstraints = false;
            //            this.table_105_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_105_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //            this.table_115_ProductionColorTableAdapter.FillById(this.dataSet_05_PCLOR.Table_115_ProductionColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
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
            //    table_105_ProductionBindingSource.EndEdit();
            //    this.table_115_ProductionColorBindingSource.EndEdit();


            //    DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_105_Production),0) as Row");
            //    if (Table.Rows[0]["Row"].ToString() != "0")
            //    {
            //        DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_105_Production where Number=" + Table.Rows[0]["Row"].ToString());
            //        dataSet_05_PCLOR.EnforceConstraints = false;
            //        this.table_105_ProductionTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_105_Production, long.Parse(RowId.Rows[0]["ID"].ToString()));
            //        this.table_115_ProductionColorTableAdapter.FillById(this.dataSet_05_PCLOR.Table_115_ProductionColor, long.Parse(RowId.Rows[0]["ID"].ToString()));

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
            Class_BasicOperation.FilterMultiColumns(mlt_Ware_D, null, "Number");

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
            Class_BasicOperation.FilterMultiColumns(mlt_Function_D, "Column02", "Column01");

        }
        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "Column02", "Column01");

        }
        private void mlt_Function_R_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function_R, "Column02", "Column01");

        }
        private void table_105_ProductionBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() != "0" ||
                    ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0" ||
                    ((DataRowView)table_105_ProductionBindingSource.CurrencyManager.Current)["NumberDraftP"].ToString() != "0")
                {
                    if (table_115_ProductionColorBindingSource.Count > 0)
                    {
                        if (((DataRowView)table_115_ProductionColorBindingSource.CurrencyManager.Current)["NumberDraft"].ToString() == "0")
                        {
                            uiPanel8Container.Enabled = true;

                        }
                        else uiPanel8Container.Enabled = false;
                    }
                    else uiPanel8Container.Enabled = false;


                }
                else uiPanel8Container.Enabled = true;


                /////
                //Int64 order = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT     dbo.Table_030_DetailOrderColor.NumberOrder
                //    FROM         dbo.Table_030_DetailOrderColor INNER JOIN
                //                          dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID
                //    WHERE     (dbo.Table_030_DetailOrderColor.ID = " + mlt_CodeOrderColor.Value + ")),0) as Result"));


                //Int64 Product = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT     SUM(dbo.Table_105_Production.NumberProduct) AS NumberProduct
                //FROM         dbo.Table_105_Production INNER JOIN
                //      dbo.Table_030_DetailOrderColor ON dbo.Table_105_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID
                //    where      (dbo.Table_105_Production.ColorOrderId = " + mlt_CodeOrderColor.Value + ") AND (dbo.Table_105_Production.ID <> " + txt_Id.Text + ")),0)as result"));

                //Int64 Remain = order - Product;
                //txt_Remining.Text = Remain.ToString();
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

        private void txt_Barcode_Leave(object sender, EventArgs e)
        {
            IEnumerable<string> lines = txt_Barcode.Lines.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x=> "'"+x+"'");

            if (lines.Count().Equals(0))
            {
                txt_NumberProduct.Text = "0";
                txt_weight.Text = "0";

                clothTypeID = -1;
                commodityCode = -1;
                machineID = -1;
                machineName = string.Empty;
                return;
            }

            string fields = string.Join(", ", lines);

            string command = string.Format("select Serial from [dbo].[Table_110_ProductionDetail] where Serial in ({0})", fields);
            details = ClDoc.ReturnTable(ConPCLOR, command);

            bool hasError = false;
            string errorMessage = string.Empty;

            if (details.Rows.Count > 0)
            {
                errorMessage = "سریال های وارد شده معتبر نمی باشد";
                hasError = true;                
            }

            command = string.Format("select COUNT(1), SUM(tbl.Weight), tbl.TypeCloth, (select TypeCloth from Table_005_TypeCloth where ID=tbl.TypeCloth), (select CodeCommondity from Table_005_TypeCloth where ID=tbl.TypeCloth), tbl.Machine, (select namemachine from Table_60_SpecsTechnical where ID=tbl.Machine) MachineName from Table_100_Knitting tbl where Serial in ({0}) group by tbl.TypeCloth, tbl.Machine", fields);

            DataTable dt = ClDoc.ReturnTable(ConPCLOR, command);

            if (dt.Rows.Count.Equals(0))
            {
                errorMessage = "سریال های وارد شده معتبر نمی باشد";
                hasError = true;
            }

            if (dt.Rows.Count > 1)
            {
                errorMessage = "سریال های وارد شده باید از یک نوع و یک دستگاه باشند";
                hasError = true;
            }

            if(hasError)
            {
                MessageBox.Show(errorMessage);

                //this.TopMost = true;
                //this.BringToFront();
                //this.Focus();
                //txt_Barcode.Focus();

                return;
            }
            txt_NumberProduct.Text = dt.Rows[0][0].ToString();
            txt_weight.Text = dt.Rows[0][1].ToString();

            clothTypeID = int.Parse(dt.Rows[0][2].ToString());
            commodityCode = int.Parse(dt.Rows[0][4].ToString());
            machineID = int.Parse(dt.Rows[0][5].ToString());
            machineName = dt.Rows[0][6].ToString();
        }

        private void txt_Barcode_Enter(object sender, EventArgs e)
        {
        }
    }
}
