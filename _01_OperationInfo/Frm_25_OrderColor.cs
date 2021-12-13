using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Stimulsoft.Report.Components;
using Janus.Windows.GridEX;
namespace PCLOR._01_OperationInfo
{
    public partial class Frm_25_OrderColor : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        public Frm_25_OrderColor()
        {
            InitializeComponent();
        }


        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 18))
            {
                table_025_HederOrderColorBindingSource.AddNew();
                txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                uiPanel1.Enabled = true;
                mlt_codecustomer.Focus();
                txt_NumberOrder.Text = "0";
                groupBox1.Enabled = true;
                btn_New.Enabled = false;
                uiPanel0.Enabled = true;

            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void Frm_25_OrderColor_Load(object sender, EventArgs e)
        {

            this.table_025_HederOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_025_HederOrderColor);
            this.table_030_DetailOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_030_DetailOrderColor);
          mlt_TypeCloth.DataSource=  gridEX2.DropDowns["TypeCloth"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth from Table_005_TypeCloth");
            gridEX2.DropDowns["Customer"].DataSource =  ClDoc.ReturnTable(ConBase, @"select Columnid,Column01,Column02 from Table_045_PersonInfo");
          mlt_Color.DataSource=  gridEX2.DropDowns["Color"].DataSource = mlt_Color.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeColor from Table_010_TypeColor");
            mlt_codecustomer.DataSource = ClDoc.ReturnTable(ConBase, @"SELECT        dbo.Table_045_PersonInfo.ColumnId, dbo.Table_045_PersonInfo.Column01
FROM            dbo.Table_040_PersonGroups INNER JOIN
                         dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 INNER JOIN
                         dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId
WHERE        (dbo.Table_045_PersonScope.Column02 IN (8, 9))
Order by  dbo.Table_045_PersonInfo.Column01 ASC");
            gridEX2.DropDowns["Machine"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,namemachine from Table_60_SpecsTechnical");
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report.mrt");
            foreach (StiPage page in r.Pages)
            {
                uiComboBox1.Items.Add(page.Name);
            }
          

           
        }

        private void mlt_codecustomer_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_codecustomer, null, "Column01");
       
               
        }
        private void FillCombo() {
            try
            {
                mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT *,
               ROW_NUMBER() OVER(
                  ORDER BY dt.Machine
               ) AS ROW
                FROM   (
               SELECT     t.Machine, t.namemachine, t.TypeCloth, t.Cloth, t.CodeCustomer, t.weight, t.Remain, dbo.Table_005_TypeCloth.Number
        FROM         (SELECT     tdrcr.Machine, tst.namemachine, tdrcr.TypeCloth, ttc.TypeCloth AS Cloth, thrcr.CodeCustomer, SUM(tdrcr.weight) AS weight, SUM(tdrcr.NumberRoll) 
                                              - ISNULL
                                                  ((SELECT     SUM(tdoc.NumberOrder) AS NumberOrder
                                                      FROM         dbo.Table_030_DetailOrderColor AS tdoc LEFT OUTER JOIN
                                                                            dbo.Table_025_HederOrderColor AS thoc ON thoc.ID = tdoc.Fk
                                                      GROUP BY thoc.CodeCustomer, tdoc.TypeColth, tdoc.Machine
                                                      HAVING      (tdoc.TypeColth = tdrcr.TypeCloth) AND (thoc.CodeCustomer = thrcr.CodeCustomer) AND (tdoc.Machine = tdrcr.Machine)), 0) AS Remain
                       FROM          dbo.Table_020_DetailReciptClothRaw AS tdrcr LEFT OUTER JOIN
                                              dbo.Table_020_HeaderReciptClothRow AS thrcr ON thrcr.ID = tdrcr.FK LEFT OUTER JOIN
                                              dbo.Table_005_TypeCloth AS ttc ON ttc.ID = tdrcr.TypeCloth LEFT OUTER JOIN
                                              dbo.Table_60_SpecsTechnical AS tst ON tst.ID = tdrcr.Machine
                       GROUP BY tdrcr.TypeCloth, thrcr.CodeCustomer, tdrcr.Machine, ttc.TypeCloth, tst.namemachine) AS t LEFT OUTER JOIN
                      dbo.Table_005_TypeCloth ON t.TypeCloth = dbo.Table_005_TypeCloth.ID ) AS dt
                WHERE  dt.Remain > 0
                       AND dt.CodeCustomer =" + mlt_codecustomer.Value + "");
            }
            catch { }
        
        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {

                Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 19))
                {
                    string barcode = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((select OrderWeave from Table_025_HederOrderColor where Id="+txt_ID.Text+"),0)");
                    

                    if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (barcode=="True")
                        {
                            MessageBox.Show("این سفارش رنگ برای بافندگی می باشد، لطفا برای حذف به فرم مربوطه مراجعه کنید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            return;
                        }

                        DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID
FROM         dbo.Table_035_Production
WHERE     (dbo.Table_035_Production.ColorOrderId IN
                          (SELECT     dbo.Table_030_DetailOrderColor.ID
                            FROM          dbo.Table_025_HederOrderColor INNER JOIN
                                                   dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
                            WHERE      (dbo.Table_025_HederOrderColor.ID =" + txt_ID.Text+")))");
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show(".از این سفارش رنگ در قسمت کارت تولید استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                          
                        else
                        {
                            foreach (DataRowView item in this.table_030_DetailOrderColorBindingSource)
                            {
                                item.Delete();
                            }
                            table_025_HederOrderColorBindingSource.RemoveCurrent();
                            table_030_DetailOrderColorBindingSource.EndEdit();
                            table_025_HederOrderColorBindingSource.EndEdit();
                            table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                            table_025_HederOrderColorTableAdapter.Update(this.dataSet_05_PCLOR.Table_025_HederOrderColor);
                            MessageBox.Show("اطلاعات با موفقیت حذف شد");
                            btn_New.Enabled = true;
                            mlt_TypeCloth.Text = "";
                            mlt_Color.Text = "";
                            uiComboBox1.Text = "";
                            txt_NumberOrder.Text = "";
                            txt_Title.Text = "";
                            txt_Total.Text = "";
                            txt_Remining.Text = "";
                            txt_Description.Text = "";
                        }
                    }
                    //else
                    //{
                    //    MessageBox.Show("سطری برای حذف وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}

                }
                else

                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                this.dataSet_05_PCLOR.EnforceConstraints = false;
                this.table_025_HederOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_025_HederOrderColor);
                this.table_030_DetailOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_030_DetailOrderColor);
                this.dataSet_05_PCLOR.EnforceConstraints = true;
            }
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (mlt_TypeCloth.Text == "")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                }
                else
                {

                    if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                    {
                        txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
                    }
                    int a = Convert.ToInt32(txt_NumberOrder.Text);
                    int b = Convert.ToInt32(txt_Remining.Text);
                    if (a >= b)
                    {

                        MessageBox.Show(" تعداد سفارش وارد شده بیشتر از باقی مانده می باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {


                        table_025_HederOrderColorBindingSource.EndEdit();

                        gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;

                        gridEX2.MoveToNewRecord();
                        gridEX2.SetValue("Fk", ((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["ID"].ToString());
                        gridEX2.SetValue("TypeColth", mlt_TypeCloth.Value);
                        gridEX2.SetValue("NumberOrder", txt_NumberOrder.Text);
                        gridEX2.SetValue("TypeColor", mlt_Color.Value);
                        gridEX2.SetValue("Title", txt_Title.Text);
                        gridEX2.SetValue("Description", txt_Description.Text);
                        gridEX2.UpdateData();

                        gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;

                    }
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {

                if (gridEX2.RowCount > 0)
                {

                    if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                    {
                        txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
                    }

                 
                    {

                        table_025_HederOrderColorBindingSource.EndEdit();
                        table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
                        table_030_DetailOrderColorBindingSource.EndEdit();
                        table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                        MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                        btn_New.Enabled = true;
                        gridEX2.Enabled = true;
                        CalculateRemain();

                        uiPanel0.Enabled = false;

                    }
                }

                else
                {
                    MessageBox.Show("سطری برای ذخیره اطلاعات وجود ندارد");
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }


      
        private void mlt_TypeCloth_KeyPress_1(object sender, KeyPressEventArgs e)
        {



            if (e.KeyChar == 13)
            {
                try
                {

                   
                     if (mlt_codecustomer.Text != "" || mlt_codecustomer.Text != "0")
                    {


                        string totalcount = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
        select ISNULL(( SELECT  distinct   SUM(dbo.Table_020_DetailReciptClothRaw.NumberRoll) AS NumberRoll          
            FROM         dbo.Table_020_HeaderReciptClothRow INNER JOIN
                          dbo.Table_020_DetailReciptClothRaw ON dbo.Table_020_HeaderReciptClothRow.ID = dbo.Table_020_DetailReciptClothRaw.FK INNER JOIN
                          dbo.Table_60_SpecsTechnical ON dbo.Table_020_DetailReciptClothRaw.Machine = dbo.Table_60_SpecsTechnical.ID
          where   (dbo.Table_020_HeaderReciptClothRow.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_020_DetailReciptClothRaw.TypeCloth = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["TypeCloth"].ToString() + ") AND (dbo.Table_020_DetailReciptClothRaw.Machine = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["Machine"].ToString() + ")),0) as NumberRoll");

                        string remain = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
        select ISNULL(( SELECT DISTINCT SUM(dbo.Table_030_DetailOrderColor.NumberOrder) AS NumberOrder
            FROM         dbo.Table_025_HederOrderColor INNER JOIN
                                  dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
          where     (dbo.Table_025_HederOrderColor.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_030_DetailOrderColor.TypeColth = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["TypeCloth"].ToString() + ") AND (dbo.Table_030_DetailOrderColor.Machine = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["Machine"].ToString() + ")),0) as NumberOrder");

                        txt_Total.Text = Convert.ToInt32(totalcount).ToString();
                        txt_Remining.Text = (Convert.ToInt32(totalcount) - Convert.ToInt32(remain)).ToString();

                    }

                 

                }
                catch { }

                txt_NumberOrder.Focus();
            }
            else
            {
                mlt_TypeCloth.Focus();
            }


        }

       
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            try
            {

                if (mlt_TypeCloth.Text == "" || uiComboBox1.Text == "" || txt_NumberOrder.Text == "0" || mlt_codecustomer.Text == "0")
                {
                    MessageBox.Show("لطفا اطلاعات را تکمیل نمایید");
                    return;
                }
                if (mlt_Color.Text.All(char.IsDigit) || uiComboBox1.Text.All(char.IsDigit) || mlt_TypeCloth.Text.All(char.IsDigit) || txt_NumberOrder.Text.All(char.IsDigit) == false)
                {
                    MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد");
                    return;
                }
                else
                {

                    if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                    {
                        txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
                    }

                    Int64 a = Convert.ToInt64(txt_NumberOrder.Text);
                    Int64 b = Convert.ToInt64(txt_Remining.Text);

                    Int64 Product = Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select ISNULL(( SELECT DISTINCT SUM(dbo.Table_035_Production.NumberProduct) AS NumberProduct
                     FROM         dbo.Table_035_Production INNER JOIN
                     dbo.Table_030_DetailOrderColor ON dbo.Table_035_Production.ColorOrderId = dbo.Table_030_DetailOrderColor.ID INNER JOIN
                     dbo.Table_025_HederOrderColor ON dbo.Table_030_DetailOrderColor.Fk = dbo.Table_025_HederOrderColor.ID
                   where    (dbo.Table_025_HederOrderColor.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_030_DetailOrderColor.TypeColth = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["TypeCloth"].ToString() + ") AND (dbo.Table_030_DetailOrderColor.TypeColor = " + mlt_Color.Value + ") AND (dbo.Table_030_DetailOrderColor.Machine = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["Machine"].ToString() + ")),0) as NumberProduct"));



                    if (a > b)
                    {

                        MessageBox.Show(" تعداد سفارش وارد شده بیشتر از باقی مانده می باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {

                        table_025_HederOrderColorBindingSource.EndEdit();
                        gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                        gridEX2.MoveToNewRecord();
                        gridEX2.SetValue("Fk", ((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["ID"].ToString());
                        gridEX2.SetValue("TypeColth", ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["TypeCloth"].ToString());
                        gridEX2.SetValue("NumberOrder", txt_NumberOrder.Text);
                        gridEX2.SetValue("TypeColor", mlt_Color.Value);
                        gridEX2.SetValue("Machine", ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["Machine"].ToString());
                        gridEX2.SetValue("weight", ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["weight"].ToString());
                        gridEX2.SetValue("Title", txt_Title.Text);
                        gridEX2.SetValue("Description", txt_Description.Text);
                        gridEX2.SetValue("Printer", uiComboBox1.Text);
                        gridEX2.UpdateData();
                        gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                        ((DataRowView)table_030_DetailOrderColorBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                        ((DataRowView)table_030_DetailOrderColorBindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();

                        table_025_HederOrderColorBindingSource.EndEdit();
                        table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
                        table_030_DetailOrderColorBindingSource.EndEdit();
                        table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);

                        // CalculateRemain();

                        //  FillCombo();
                        mlt_TypeCloth.Value = "";
                        txt_Total.Text = "";
                        txt_Remining.Text = "";
                        txt_NumberOrder.Text = "";
                        mlt_Color.Value = "";
                        txt_Title.Text = "";
                        uiComboBox1.Text = "";
                        txt_Description.Text = "";
                        //
                        FillCombo();

                    }
                }

            }

            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

        }

        private void Frm_25_OrderColor_KeyDown(object sender, KeyEventArgs e)
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

        private void mlt_codecustomer_Leave(object sender, EventArgs e)
        {
            FillCombo();
          
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
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


        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                txt_Total.Text = "";
                txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                uiPanel0.Enabled = false;


                gridEX2.UpdateData();
                table_025_HederOrderColorBindingSource.EndEdit();
                this.table_030_DetailOrderColorBindingSource.EndEdit();
                

                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_025_HederOrderColor where Number>"
                    + ((DataRowView)this.table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");

                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString());
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }





            }
            catch
            {

            }

        }

        private void mlt_Color_ValueChanged_1(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Color, "TypeColor", "ID");
        }

        private void mlt_Color_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void mlt_TypeCloth_Leave_1(object sender, EventArgs e)
        {
            try
            {

                if (mlt_codecustomer.Text == "" || mlt_codecustomer.Text == "0")
                {
                    MessageBox.Show("لطفا اطلاعات مشتری را تکمیل نمایید");
                   
                }

                else if (mlt_codecustomer.Text != "" || mlt_codecustomer.Text != "0")
                {

                    CalculateRemain();

                }

                txt_NumberOrder.Focus();

            }
            catch { }

        }

        private void CalculateRemain()
        {
            try
            {
                string totalcount = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
        select ISNULL(( SELECT  distinct   SUM(dbo.Table_020_DetailReciptClothRaw.NumberRoll) AS NumberRoll          
            FROM         dbo.Table_020_HeaderReciptClothRow INNER JOIN
                          dbo.Table_020_DetailReciptClothRaw ON dbo.Table_020_HeaderReciptClothRow.ID = dbo.Table_020_DetailReciptClothRaw.FK INNER JOIN
                          dbo.Table_60_SpecsTechnical ON dbo.Table_020_DetailReciptClothRaw.Machine = dbo.Table_60_SpecsTechnical.ID
            where      (dbo.Table_020_HeaderReciptClothRow.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_020_DetailReciptClothRaw.TypeCloth = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["TypeCloth"].ToString() + ") AND (dbo.Table_020_DetailReciptClothRaw.Machine = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["Machine"].ToString() + ")),0) as NumberRoll");

                string remain = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
        select ISNULL(( SELECT DISTINCT SUM(dbo.Table_030_DetailOrderColor.NumberOrder) AS NumberOrder
            FROM         dbo.Table_025_HederOrderColor INNER JOIN
                                  dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
           where    (dbo.Table_025_HederOrderColor.CodeCustomer = " + mlt_codecustomer.Value + ") AND (dbo.Table_030_DetailOrderColor.TypeColth = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["TypeCloth"].ToString() + ") AND (dbo.Table_030_DetailOrderColor.Machine = " + ((DataRowView)mlt_TypeCloth.DropDownList.FindItem(mlt_TypeCloth.Value))["Machine"].ToString() + ")),0) as NumberOrder");

                txt_Total.Text = Convert.ToInt32(totalcount).ToString();
                txt_Remining.Text = (Convert.ToInt32(totalcount) - Convert.ToInt32(remain)).ToString();
            }
            catch { }
        }

        private void mlt_codecustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                { mlt_TypeCloth.Focus(); }
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void mlt_TypeCloth_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_TypeCloth, "Cloth", "Number");
        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     ID
FROM         dbo.Table_035_Production
WHERE     dbo.Table_035_Production.ColorOrderId =" + gridEX2.GetValue("ID").ToString() + "");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(".از این نوع پارچه در قسمت کارت تولید استفاده شده است امکان حذف آن وجود ندارد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }

                    else
                    {
                        if (((DataRowView)table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString().StartsWith("-"))
                        {
                            txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_025_HederOrderColor", "Number").ToString();
                        }

                        //  else
                        {
                            gridEX2.GetRow().Delete();
                            table_025_HederOrderColorBindingSource.EndEdit();
                            table_025_HederOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_025_HederOrderColor);
                            e.Cancel = true;
                            table_030_DetailOrderColorBindingSource.EndEdit();
                            table_030_DetailOrderColorTableAdapter.Update(dataSet_05_PCLOR.Table_030_DetailOrderColor);
                            MessageBox.Show("اطلاعات با موفقیت حذف شد");

                            btn_New.Enabled = true;

                            if (mlt_TypeCloth.Text == "")
                            {
                                FillCombo();
                            }

                            // CalculateRemain();


                        }
                    }


                }
                else { e.Cancel = true; }
            }
            catch
            {
                MessageBox.Show("حذف امکان پذیر نیست");
                this.table_030_DetailOrderColorTableAdapter.Fill(this.dataSet_05_PCLOR.Table_030_DetailOrderColor);
            }
        }

        private void mlt_codecustomer_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                {  mlt_TypeCloth.Focus(); }
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
                { txt_NumberOrder.Focus(); }
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void mlt_Color_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                { Class_BasicOperation.isEnter(e.KeyChar); }
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }

        }

        private void txt_Dat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void btn_enable_Click(object sender, EventArgs e)
        {
             Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
             if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 65))
             {
                 uiPanel0.Enabled = true;
             }
             else

                 Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

           
        }

        private void table_025_HederOrderColorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            uiPanel0.Enabled = false;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet_05_PCLOR.EnforceConstraints = false;
                this.table_025_HederOrderColorTableAdapter.FillByNumber(this.dataSet_05_PCLOR.Table_025_HederOrderColor, Convert.ToInt64(txt_Search.Text));

                if (table_025_HederOrderColorBindingSource.Count > 0)
                {
                    this.table_030_DetailOrderColorTableAdapter.FillByNumber(dataSet_05_PCLOR.Table_030_DetailOrderColor, Convert.ToInt64(txt_Search.Text));
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

        private void Txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Search_Click(sender, e);
        }

        private void BindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                txt_Total.Text = "";
                txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                uiPanel0.Enabled = false;

                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select max(Number) from Table_025_HederOrderColor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString());
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;

                }

                

            }
            catch
            {

            }
        }

        private void BindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                txt_Total.Text = "";
                txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                uiPanel0.Enabled = false;

                if (this.table_025_HederOrderColorBindingSource.Count > 0)
                {
                    
                    if (this.table_025_HederOrderColorBindingSource.Count > 0)
                    {
                        try
                        {
                            gridEX2.UpdateData();
                            table_025_HederOrderColorBindingSource.EndEdit();
                            this.table_030_DetailOrderColorBindingSource.EndEdit();
                            DataTable Table = ClDoc.ReturnTable(ConPCLOR,
                                "Select ISNULL((Select max(Number) from Table_025_HederOrderColor where Number<" +
                                ((DataRowView)this.table_025_HederOrderColorBindingSource.CurrencyManager.Current)["Number"].ToString() + "),0) as Row");
                            if (Table.Rows[0]["Row"].ToString() != "0")
                            {
                                DataTable RowId = ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString());
                                dataSet_05_PCLOR.EnforceConstraints = false;
                                this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                                this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
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
            catch
            {

            }
        }

        private void BindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                mlt_TypeCloth.Value = "";
                txt_Total.Text = "";
                txt_Remining.Text = "";
                txt_NumberOrder.Text = "";
                mlt_Color.Value = "";
                txt_Title.Text = "";
                uiComboBox1.Text = "";
                txt_Description.Text = "";
                uiPanel0.Enabled = false;

                gridEX2.UpdateData();
                table_025_HederOrderColorBindingSource.EndEdit();
                this.table_030_DetailOrderColorBindingSource.EndEdit();


                DataTable Table = ClDoc.ReturnTable(ConPCLOR, "Select ISNULL((Select min(Number) from Table_025_HederOrderColor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId =ClDoc.ReturnTable(ConPCLOR, "Select Id from Table_025_HederOrderColor where Number=" + Table.Rows[0]["Row"].ToString());
                    dataSet_05_PCLOR.EnforceConstraints = false;
                    this.table_025_HederOrderColorTableAdapter.FillByID(this.dataSet_05_PCLOR.Table_025_HederOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    this.table_030_DetailOrderColorTableAdapter.FillByheaderid(this.dataSet_05_PCLOR.Table_030_DetailOrderColor, long.Parse(RowId.Rows[0]["ID"].ToString()));
                    dataSet_05_PCLOR.EnforceConstraints = true;
                    
                }



            }
            catch
            {

            }
        }
    }
}
