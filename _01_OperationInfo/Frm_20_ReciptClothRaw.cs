using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;
using System.IO;
using DevComponents.DotNetBar;
namespace PCLOR._01_OperationInfo
{
    public partial class Frm_20_ReciptClothRaw : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        BindingSource Child1Bind;
        DataRowView BuyRow;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        int ResidNum = 0;
        Int64 O;
        Int64 R;
        bool Flag = false;

        public Frm_20_ReciptClothRaw()
        {
            InitializeComponent();

            //_ware = ware;
            //_func = func;
        }

      

      

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 15))
            {
                try
                {

                    table_020_HeaderReciptClothRowBindingSource.AddNew();
                    txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                    mlt_codecustomer.Focus();
                    btn_New.Enabled = false;
                    gridEX2.Enabled = true;
                    uiPanel1.Enabled = true;
                }

                catch (Exception ex)
                {

                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }

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
                Flag = false;
                gridEX2.UpdateData();

                if (mlt_codecustomer.Text == "" || mlt_codecustomer.Text == "0" || gridEX2.RowCount == 0 || (gridEX2.RowCount == 0) || mlt_Function.Text.All(char.IsDigit) || mlt_Ware.Text.All(char.IsDigit))
                    {
                        MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                        return;
                    }

                    if (((DataRowView)table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0")
                    {
                        MessageBox.Show("رسید برای این شماره پارچه درج شده است  امکان ویرایش وجود ندارد");
                        return;
                    }
                    else
                    {
                        if (((DataRowView)table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                        {
                            txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, " table_020_HeaderReciptClothRow", "Number").ToString();
                        }
                        dataSet_05_PCLOR.EnforceConstraints = false;
                        table_020_HeaderReciptClothRowBindingSource.EndEdit();
                        table_020_HeaderReciptClothRowTableAdapter.Update(dataSet_05_PCLOR.Table_020_HeaderReciptClothRow);
                        table_020_DetailReciptClothRawBindingSource.EndEdit();
                        table_020_DetailReciptClothRawTableAdapter.Update(dataSet_05_PCLOR.Table_020_DetailReciptClothRaw);
                        dataSet_05_PCLOR.EnforceConstraints = true;

                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                        {
                            R = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  select isnull((SELECT     SUM(dbo.Table_020_DetailReciptClothRaw.NumberRoll) AS NumberRoll
                                    FROM         dbo.Table_020_DetailReciptClothRaw INNER JOIN
                                                          dbo.Table_020_HeaderReciptClothRow ON dbo.Table_020_DetailReciptClothRaw.FK = dbo.Table_020_HeaderReciptClothRow.ID
                                  where    (dbo.Table_020_HeaderReciptClothRow.CodeCustomer = " + mlt_codecustomer.Value + ")  AND (dbo.Table_020_DetailReciptClothRaw.Machine = " + item.Cells["Machine"].Value.ToString() + ") AND  (dbo.Table_020_DetailReciptClothRaw.TypeCloth = " + item.Cells["TypeCloth"].Value.ToString() + ") ),0)"));

                            O = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL((  SELECT     SUM(dbo.Table_030_DetailOrderColor.NumberOrder) AS NumberOrder
                                    FROM         dbo.Table_025_HederOrderColor INNER JOIN
                                                          dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
                                   where     (dbo.Table_025_HederOrderColor.CodeCustomer =" + mlt_codecustomer.Value + ")  AND (dbo.Table_030_DetailOrderColor.Machine = " + item.Cells["Machine"].Value.ToString() + ") AND (dbo.Table_030_DetailOrderColor.TypeColth = " + item.Cells["TypeCloth"].Value.ToString() + ")),0)"));

                            if (O > R)
                            {
                                Flag = true;
                             
                            }
                        }
                        if (Flag == false)
                        {

                            btn_Recipt_Click(sender, e);
                            mlt_Recipt.DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                            dataSet_05_PCLOR.EnforceConstraints = false;
                            this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(txt_id.Text));
                            this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(txt_id.Text));
                            dataSet_05_PCLOR.EnforceConstraints = true;

                            btn_New.Enabled = true;
                            uiPanel1.Enabled = false;
                        }
                        else { MessageBox.Show("تعداد رسید از تعداد سفارش شده نمیتواند کمتر باشد.اطلاعات را اصلاح کنید "); }
                        

                    }

            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }

        private void mlt_codecustomer_ValueChanged(object sender, EventArgs e)
        {

        }

        private void mlt_codecustomer_ValueChanged_1(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_codecustomer, "Column02", "Column01");
            //mlt_NameCustomer.Value = mlt_codecustomer.Value;

        }

        private void mlt_TypeCloth_ValueChanged(object sender, EventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns(mlt_TypeCloth, "TypeColth", "ID");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 16))
                {

                    if (table_020_HeaderReciptClothRowBindingSource.Count > 0)
                    {
                        if (((DataRowView)table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0")
                        {
                            MessageBox.Show("این برگه دارای رسید می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        else
                        {
                            if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {


                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                                {
                                    Int64 Recipt = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  select isnull((SELECT     SUM(dbo.Table_020_DetailReciptClothRaw.NumberRoll) AS NumberRoll
                                FROM         dbo.Table_020_DetailReciptClothRaw INNER JOIN
                                                      dbo.Table_020_HeaderReciptClothRow ON dbo.Table_020_DetailReciptClothRaw.FK = dbo.Table_020_HeaderReciptClothRow.ID
                               
                                 where     (dbo.Table_020_HeaderReciptClothRow.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_020_HeaderReciptClothRow.ID <>" + txt_id.Text + " )  AND (dbo.Table_020_DetailReciptClothRaw.Machine = " + item.Cells["Machine"].Value.ToString() + ") AND  (dbo.Table_020_DetailReciptClothRaw.TypeCloth = " + item.Cells["TypeCloth"].Value.ToString() + ") ),0)"));

                                    Int64 Order = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL((  SELECT     SUM(dbo.Table_030_DetailOrderColor.NumberOrder) AS NumberOrder
                            FROM         dbo.Table_025_HederOrderColor INNER JOIN
                                                  dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
                         where     (dbo.Table_025_HederOrderColor.CodeCustomer =" + mlt_codecustomer.Value + ")  AND (dbo.Table_030_DetailOrderColor.Machine = " + item.Cells["Machine"].Value.ToString() + ") AND (dbo.Table_030_DetailOrderColor.TypeColth = " + item.Cells["TypeCloth"].Value.ToString() + ")),0)"));

                                    Int64 Count = Order - Recipt;
                                    if (Count > 0)
                                    {

                                        MessageBox.Show("تعداد پارچه " + item.Cells["TypeCloth"].Text + " در قسمت سفارشات بیشتر از تعداد رسید شده می باشد امکان حذف وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                }

                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_020_DetailReciptClothRaw where FK =" + txt_id.Text + "");
                                ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_020_HeaderReciptClothRow where ID=" + txt_id.Text + "");
                                table_020_HeaderReciptClothRowBindingSource.EndEdit();
                                table_020_HeaderReciptClothRowTableAdapter.Update(dataSet_05_PCLOR.Table_020_HeaderReciptClothRow);
                                table_020_DetailReciptClothRawBindingSource.EndEdit();
                                table_020_DetailReciptClothRawTableAdapter.Update(dataSet_05_PCLOR.Table_020_DetailReciptClothRaw);
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(txt_id.Text));
                                this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(txt_id.Text));
                                dataSet_05_PCLOR.EnforceConstraints = true;
                                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                                btn_New.Enabled = true;

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

        private void btn_end_Click(object sender, EventArgs e)
        {

        }



        private void Frm_20_ReciptClothRaw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btn_Save_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.N)
            {
                btn_New_Click(sender, e);
            }

        }

        

    
        private void btn_Recipt_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
            {

                if (mlt_Function.Text.Trim() == "0" || mlt_Ware.Text.Trim() == "0")
                {
                    MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                }
                else
                {

                    {

                        //ClDoc.ReturnTable(ConPCLOR, @" SELECT  ISNULL(( select CodeCommondity  FROM  dbo.Table_005_TypeCloth where  ID=" + ((DataRowView)gridEX2.RootTable.Columns["TypeCloth"].DropDown.FindItem(gridEX2.GetValue("TypeCloth").ToString()))["CodeCommondity"].ToString() + "),0)as commodity");
                       
                         ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");

                        string commandtxt = string.Empty;
                        commandtxt = @"Declare @Key int";

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
                                                                 
                                                                          ) VALUES (" + ResidNum + ",'" + txt_Dat.Text + "' ," + mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + ","
                                                                           + mlt_codecustomer.Value.ToString() + ",'" + "رسید صادره بابت رسید پارچه خام ش" + txt_Number.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()); SET @Key=Scope_Identity() ";
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
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
                                   ,[Column34]
                                   ,[Column35]
                                   ,[Column37]
                           ) VALUES (@Key," + (((DataRowView)gridEX2.RootTable.Columns["TypeCloth"].DropDown.FindItem(item.Cells["TypeCloth"].Value))["CodeCommondity"].ToString()) + ",1," + item.Cells["NumberRoll"].Value +
                        "," + item.Cells["NumberRoll"].Value + ",0,0,'" + Class_BasicOperation._UserName +
                        "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + (Convert.ToDecimal(item.Cells["weight"].Value) / Convert.ToDecimal(item.Cells["NumberRoll"].Value)).ToString() + "," + Convert.ToDecimal(item.Cells["weight"].Value) + ",'" + item.Cells["Machine"].Text + "');";
                        }
                        commandtxt += " Update " + ConPCLOR.Database + ".dbo.Table_020_HeaderReciptClothRow set NumberRecipt=@key where Id = " + int.Parse(txt_id.Text).ToString();
                        commandtxt += " select @key as receiptid";
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.PWHRS))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = commandtxt;
                              //  int receiptid = int.Parse(Command.ExecuteScalar().ToString());
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                                MessageBox.Show("اطلاعات با موفقیت ذخیره شد و " + "رسید انبار به شماره " + ResidNum.ToString() + "صادر گردید");

                                


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
            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void btn_Del_Recipt_Click(object sender, EventArgs e)
        {
            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", mlt_Recipt.Value.ToString()) != 0)

                throw new Exception(" رسید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");

            if (clDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "case when Column19=1 then 1 else 0 end", mlt_Recipt.Value.ToString()) != 0)

                throw new Exception(" رسید قطعی می باشد ابتدا آن را غیر قطعی نمایید");
       
            //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_012_Child_PwhrsReceipt where Column01=" + mlt_Recipt.Value.ToString());
            //ClDoc.RunSqlCommand(ConWare.ConnectionString, "Delete From Table_011_PwhrsReceipt Where ColumnId=" + mlt_Recipt.Value.ToString());
            //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, "Update  Table_020_HeaderReciptClothRow set NumberRecipt=0 Where ID=" + txt_id.Text);

            string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select NumberRecipt from Table_020_HeaderReciptClothRow where Id = " + txt_id.Text + @")

                                                    Delete from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt where columnid in(
                                                    select NumberRecipt from Table_020_HeaderReciptClothRow where Id = " + txt_id.Text + @")
                                                    Update Table_020_HeaderReciptClothRow set NumberRecipt=0   where Id = " + txt_id.Text + "";
        
            Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);


            dataSet_05_PCLOR.EnforceConstraints = false;
            this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(txt_id.Text));
            this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(txt_id.Text));
            gridEX2.Enabled = (table_020_HeaderReciptClothRowBindingSource.Count > 0);
            dataSet_05_PCLOR.EnforceConstraints = true;
           
        }



        private void txt_Number_KeyPress_2(object sender, KeyPressEventArgs e)
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

       

        private void mlt_codecustomer_Leave_1(object sender, EventArgs e)
        {
           Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void Frm_20_ReciptClothRaw_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=14");
                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=1");
            }
            catch { }
        }

        private void Frm_20_ReciptClothRaw_Load(object sender, EventArgs e)
        {
            try
            {
                gridEX2.DropDowns["TypeCloth"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeCloth,Number,CodeCommondity from Table_005_TypeCloth");
                mlt_codecustomer.DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column01,Column02 from Table_045_PersonInfo");

                mlt_Ware.DataSource = clDoc.ReturnTable(ConWare, @"SELECT     " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid, " + ConWare.Database + @".dbo.Table_001_PWHRS.column02, " + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare
                                                                FROM        " + ConPCLOR.Database + @". dbo.Table_90_Wares INNER JOIN
                                                                                      " + ConWare.Database + @".dbo.Table_001_PWHRS ON " + ConPCLOR.Database + @".dbo.Table_90_Wares.IdWare = " + ConWare.Database + @".dbo.Table_001_PWHRS.columnid
                                                                WHERE     (" + ConPCLOR.Database + @".dbo.Table_90_Wares.TypeWare = 1)");

                mlt_Function.DataSource = clDoc.ReturnTable(ConWare, @"Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
                mlt_Recipt.DataSource = clDoc.ReturnTable(ConWare, @"select Columnid,Column01 from Table_011_PwhrsReceipt");
                gridEX2.DropDowns["Machine"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");
                mlt_Ware.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=14");
                mlt_Function.Value = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=1");
                this.table_020_HeaderReciptClothRowTableAdapter.Fill(dataSet_05_PCLOR.Table_020_HeaderReciptClothRow);
                this.table_020_DetailReciptClothRawTableAdapter.Fill(dataSet_05_PCLOR.Table_020_DetailReciptClothRaw);
                gridEX2.Enabled = (table_020_HeaderReciptClothRowBindingSource.Count > 0);


                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_005_TypeCloth SET [TypeCloth] = REPLACE([TypeCloth],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update Table_010_TypeColor SET [TypeColor] = REPLACE([TypeColor],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_008_Child_PwhrsDraft SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");
                ClDoc.RunSqlCommand(ConWare.ConnectionString, @"Update Table_012_Child_PwhrsReceipt SET [Column36] = REPLACE([Column36],NCHAR(1610),NCHAR(1740))");



            }
            catch { }

        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
           try
            {
                gridEX2.UpdateData();
                table_020_HeaderReciptClothRowBindingSource.EndEdit();
                this.table_020_DetailReciptClothRawBindingSource.EndEdit();


                DataTable Table = clDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select max(Number) from Table_020_HeaderReciptClothRow),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConPCLOR, "Select Id from Table_020_HeaderReciptClothRow where Number=" + Table.Rows[0]["Row"].ToString());
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId (this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;
                   
                }

            }
            catch
            {
            }
        }
  
        private void table_020_HeaderReciptClothRowBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                uiPanel1.Enabled = false;
                 if (mlt_Recipt.Text == "0" || mlt_Recipt.Text == "" )
                 { uiPanel1Container.Enabled = true; gridEX2.Enabled = true; }
                 else { uiPanel1Container.Enabled = false; gridEX2.Enabled = false; }

            }
            catch { }
            //try{


            //           if (((DataRowView)table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0"  )
            //        {
            //            gridEX2.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.False;

            //        }
            //        else
            //        {
            //            gridEX2.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True;

            //        }



            //}
            //catch { }
        }

      
        private void حذفرسیدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 40))
            {
                
                btn_Del_Recipt_Click(sender, e);
                dataSet_05_PCLOR.EnforceConstraints = false;
                this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(txt_id.Text));
                this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(txt_id.Text));
                gridEX2.Enabled = (table_020_HeaderReciptClothRowBindingSource.Count > 0);
                dataSet_05_PCLOR.EnforceConstraints = true;
                uiPanel1.Enabled = true;
                MessageBox.Show("اطلاعات با موفقیت حذف شد");
                if (mlt_Recipt.Text != "0")
                { uiPanel1Container.Enabled = false; gridEX2.Enabled = false; }
                else { uiPanel1Container.Enabled = true; gridEX2.Enabled = true; }
             
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

     

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
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

        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {
                table_020_HeaderReciptClothRowBindingSource.EndEdit();

            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
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
        private void gridEX2_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;

            if (e.Column.Key == "TypeCloth")
            {
                FilterGridExDropDown(sender, "CodeCommondity", "TypeCloth", gridEX2.EditTextBox.Text);
            }
            if (e.Column.Key == "Machine")
            {
                FilterGridExDropDown(sender, "Code", "namemachine", gridEX2.EditTextBox.Text);
            }
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                ((DataRowView)table_020_DetailReciptClothRawBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                ((DataRowView)table_020_DetailReciptClothRawBindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();

            }


            catch (Exception)
            {

                throw;
            }
        }

        Int64 Count = 0;
        private void حذفسطرجاریToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 41))
            {
                if (gridEX2.RowCount > 0)
                    {
                        if (((DataRowView)table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0")
                        {
                            MessageBox.Show("این برگه دارای رسید می باشد امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                            {
                                Int64 Recipt = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  select isnull((SELECT     SUM(dbo.Table_020_DetailReciptClothRaw.NumberRoll) AS NumberRoll
                                FROM         dbo.Table_020_DetailReciptClothRaw INNER JOIN
                                                      dbo.Table_020_HeaderReciptClothRow ON dbo.Table_020_DetailReciptClothRaw.FK = dbo.Table_020_HeaderReciptClothRow.ID
                               where    (dbo.Table_020_HeaderReciptClothRow.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_020_HeaderReciptClothRow.ID <>" + txt_id.Text + " )  AND (dbo.Table_020_DetailReciptClothRaw.Machine = " + item.Cells["Machine"].Value.ToString() + ") AND  (dbo.Table_020_DetailReciptClothRaw.TypeCloth = " + item.Cells["TypeCloth"].Value.ToString() + ") ),0)"));

                                Int64 Order = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"  SELECT ISNULL((  SELECT     SUM(dbo.Table_030_DetailOrderColor.NumberOrder) AS NumberOrder
                            FROM         dbo.Table_025_HederOrderColor INNER JOIN
                                                  dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
                           where    (dbo.Table_025_HederOrderColor.CodeCustomer =" + mlt_codecustomer.Value + ")  AND (dbo.Table_030_DetailOrderColor.Machine = " + item.Cells["Machine"].Value.ToString() + ") AND (dbo.Table_030_DetailOrderColor.TypeColth = " + item.Cells["TypeCloth"].Value.ToString() + ")),0)"));

                                 Count = Order - Recipt;

                            
                        }
                        if (Count > 0)
                        {

                            MessageBox.Show(".از این پارجه در قسمت سفارش رنگ استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_020_DetailReciptClothRaw where id =" + gridEX2.GetValue("id").ToString() + "");
                            table_020_HeaderReciptClothRowBindingSource.EndEdit();
                            table_020_HeaderReciptClothRowTableAdapter.Update(dataSet_05_PCLOR.Table_020_HeaderReciptClothRow);
                            this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(txt_id.Text));
                        }  
                }
                else
                {
                    ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_020_DetailReciptClothRaw where FK =" + txt_id.Text + "");
                    ClDoc.Execute(ConPCLOR.ConnectionString, @"delete from Table_020_HeaderReciptClothRow where ID=" + txt_id.Text + "");
                    table_020_HeaderReciptClothRowBindingSource.EndEdit();
                    table_020_HeaderReciptClothRowTableAdapter.Update(dataSet_05_PCLOR.Table_020_HeaderReciptClothRow);
                    table_020_DetailReciptClothRawBindingSource.EndEdit();
                    table_020_DetailReciptClothRawTableAdapter.Update(dataSet_05_PCLOR.Table_020_DetailReciptClothRaw);
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(txt_id.Text));
                    this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(txt_id.Text));
                    dataSet_05_PCLOR.EnforceConstraints = true;
                    MessageBox.Show("اطلاعات با موفقیت حذف شد");
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void bindingNavigatorMoveNextItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                gridEX2.UpdateData();
                table_020_HeaderReciptClothRowBindingSource.EndEdit();
                this.table_020_DetailReciptClothRawBindingSource.EndEdit();


                DataTable Table = clDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_020_HeaderReciptClothRow where Number>" 
                    + ((DataRowView)this.table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");

                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConPCLOR, "Select Id from Table_020_HeaderReciptClothRow where Number=" + Table.Rows[0]["Row"].ToString());
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_020_HeaderReciptClothRowBindingSource.Count > 0)
            {

                if (this.table_020_HeaderReciptClothRowBindingSource.Count > 0)
                {
                    try
                    {
                        gridEX2.UpdateData();
                        table_020_HeaderReciptClothRowBindingSource.EndEdit();
                        this.table_020_DetailReciptClothRawBindingSource.EndEdit();
                        DataTable Table = clDoc.ReturnTable(ConPCLOR,
                            "Select ISNULL((Select max(Number) from Table_020_HeaderReciptClothRow where Number<" +
                            ((DataRowView)this.table_020_HeaderReciptClothRowBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
                        if (Table.Rows[0]["Row"].ToString() != "0")
                        {
                            DataTable RowId = clDoc.ReturnTable(ConPCLOR, "Select Id from Table_020_HeaderReciptClothRow where Number=" + Table.Rows[0]["Row"].ToString());
                            dataSet_05_PCLOR.EnforceConstraints = false;
                            this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(RowId.Rows[0]["ID"].ToString()));
                            this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(RowId.Rows[0]["ID"].ToString()));
                            dataSet_05_PCLOR.EnforceConstraints = true;

                        }
                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
                }
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX2.UpdateData();
                table_020_HeaderReciptClothRowBindingSource.EndEdit();
                this.table_020_DetailReciptClothRawBindingSource.EndEdit();


                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_020_HeaderReciptClothRow),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConPCLOR, "Select Id from Table_020_HeaderReciptClothRow where Number=" + Table.Rows[0]["Row"].ToString());
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_020_HeaderReciptClothRowTableAdapter.FillById(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_020_DetailReciptClothRawTableAdapter.FillByHedearId(this.dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
   
        }

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Ware, "column02", null);
        }

        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "Column02", "Column01");
        }

        private void txt_Description_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            { gridEX2.Focus(); gridEX2.Col = 0; }
        }

        private void mlt_codecustomer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");
        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
             Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
             if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 64))
             {
                 uiPanel1.Enabled = true;
             }
             else
             {
                 Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
             }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet_05_PCLOR.EnforceConstraints = false;
                this.table_020_HeaderReciptClothRowTableAdapter.FillByNumber(this.dataSet_05_PCLOR.Table_020_HeaderReciptClothRow, Convert.ToInt64(txt_Search.Text));
                if (table_020_HeaderReciptClothRowBindingSource.Count > 0)
                {
                    this.table_020_DetailReciptClothRawTableAdapter.FillByNumber(dataSet_05_PCLOR.Table_020_DetailReciptClothRaw, Convert.ToInt64(txt_Search.Text));
                }

                else
                {
                    MessageBox.Show(".رسید با این شماره وجود ندارد");

                    txt_Search.SelectAll();
                    dataSet_05_PCLOR.EnforceConstraints = true;
                }
            }
            catch { }
        }

        private void غیرفعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiPanel1.Enabled = false;
        }

        private void Txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Search_Click(sender, e);
        }

        private void gridEX2_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }
    }
}
