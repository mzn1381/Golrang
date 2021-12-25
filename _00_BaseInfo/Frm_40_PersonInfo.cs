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
using Dapper;

namespace PCLOR._00_BaseInfo
{
    public partial class Frm_40_PersonInfo : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPACNT = new SqlConnection(Properties.Settings.Default.PACNT);

        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        Classes.DeleteRelatedInfo CheckRelation = new Classes.DeleteRelatedInfo();

        public Frm_40_PersonInfo()
        {
            InitializeComponent();
        }

        private void Frm_40_PersonInfo_Load(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                mlt_Branch.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,CodeBranch,NameBranch from Table_85_Branchs where 'True'='" + isadmin + "' or Id in (select Column133 from PBASE_" + Class_BasicOperation._OrgCode + ".dbo.table_045_personinfo where Column23=N'" + Class_BasicOperation._UserName + "' )");

                this.table_045_PersonInfoTableAdapter.FillByBranch(dataSet_10_PersonInfo.Table_045_PersonInfo, isadmin.ToString(), Class_BasicOperation._UserName);

                gridEX1.DropDowns["Branchs"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,NameBranch from Table_85_Branchs");

                SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01 from Table_040_PersonGroups", ConBase);
                DataTable Group = new DataTable();
                Adapter.Fill(Group);
                gridEX1.DropDowns["Member"].SetDataBinding(Group, "");
                cmb_Scope.DropDownDataSource = Group;
                Adapter = new SqlDataAdapter("Select Column00 from Table_010_UserInfo where Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._FinYear + "'", ConMain);
                DataTable UsersTable = new DataTable();
                Adapter.Fill(UsersTable);
                mlt_Users.DataSource = UsersTable;

                if (isadmin)
                {
                    mlt_Users.Enabled = true;
                }
                else
                {
                    mlt_Users.Enabled = false;
                }
                this.table_045_PersonScopeTableAdapter.Fill(this.dataSet_10_PersonInfo.Table_045_PersonScope);
                this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_10_PersonInfo.Table_040_PersonGroups);
                this.table_045_PersonInfoBindingSource.MoveLast();
                this.table_045_PersonInfoBindingSource_PositionChanged(sender, e);
            }
            catch { }
        }
        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                table_045_PersonInfoBindingSource.AddNew();
                txt_Code.Text = dataSet_10_PersonInfo.Tables["Table_045_PersonInfo"].Compute("Max(Column01)+1", null).ToString();
                rdb_Righful.Checked = true;
                rdb_Active_CheckedChanged(sender, e);
                rdb_Inactive_CheckedChanged(sender, e);
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                if (!isadmin)
                {
                    if (Class_BasicOperation._Branch != 0)
                        mlt_Branch.Value = Class_BasicOperation._Branch;

                }


                bt_New.Enabled = false;
                txt_FirstName.Focus();

            }
            catch
            { }
        }
        private void rdb_Righful_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Righful.Checked)
            {
                rdb_Legal.Checked = false;
                txt_Name.ReadOnly = true;
                txt_FirstName.ReadOnly = false;
                txt_LastName.ReadOnly = false;
            }
            else
            {
                rdb_Legal.Checked = true;
                txt_Name.ReadOnly = false;
                txt_FirstName.ReadOnly = true;
                txt_LastName.ReadOnly = true;
            }
        }

        private void rdb_Legal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Legal.Checked)
            {
                rdb_Righful.Checked = false;
                txt_Name.ReadOnly = false;
                txt_FirstName.ReadOnly = true;
                txt_LastName.ReadOnly = true;
            }
            else
            {
                rdb_Righful.Checked = true;
                txt_Name.ReadOnly = true;
                txt_FirstName.ReadOnly = false;
                txt_LastName.ReadOnly = false;
            }
        }

        private void rdb_Active_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Active.Checked)
                rdb_Inactive.Checked = false;
            else
                rdb_Inactive.Checked = true;
        }

        private void rdb_Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Inactive.Checked)
                rdb_Active.Checked = false;
            else
                rdb_Active.Checked = true;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdb_Active.Checked == null || rdb_Inactive.Checked == null || txt_FirstName.Text == "" || txt_Name.Text == "" || txt_NationalCode.Text == "")
                {
                    MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                }
                else
                {
                    var id = int.Parse(((DataRowView)table_045_PersonInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    bool tagVerify = Convert.ToBoolean(ClDoc.ExScalar(ConBase.ConnectionString, string.Format("select cast(case when exists (select 1 from [dbo].[Table_045_PersonInfo] where Column148=N'{0}' and ColumnId <> {1}) then 0 else 1 end as bit)", txtTag.Text, id)));

                    if (tagVerify)
                    {
                        if (tagVerify.Equals(false))
                        {
                            MessageBox.Show("تگ وارد شده تکراری می باشد");
                            return;
                        }
                    }

                    if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                        throw new Exception("گروه شخص را مشخص کنید");
                    cmb_Scope.UpdateValueDataSource();
                    ((DataRowView)table_045_PersonInfoBindingSource.CurrencyManager.Current)["Column03"] = false;
                    dataSet_10_PersonInfo.EnforceConstraints = false;
                    this.table_045_PersonInfoBindingSource.EndEdit();
                    this.table_045_PersonScopeBindingSource.EndEdit();
                    this.table_040_PersonGroupsBindingSource.EndEdit();
                    this.table_045_PersonInfoTableAdapter.Update(dataSet_10_PersonInfo.Table_045_PersonInfo);
                    this.table_045_PersonScopeTableAdapter.Update(dataSet_10_PersonInfo.Table_045_PersonScope);
                    this.table_040_PersonGroupsTableAdapter.Update(dataSet_10_PersonInfo.Table_040_PersonGroups);
                    //table_045_PersonInfoTableAdapter.FillByNumber(dataSet_10_PersonInfo.Table_045_PersonInfo, int.Parse(txt_ID.Text));
                    dataSet_10_PersonInfo.EnforceConstraints = true;
                    if (!string.IsNullOrWhiteSpace(txtTag.Text.Trim()))
                    {
                        using (IDbConnection db = new SqlConnection(ConBase.ConnectionString))
                        {
                            var query = $@"
                                               Insert into PCLOR_1_1400.dbo.Table_135_RFIDPerson
                                               (Person,CodeRFID)
                                               select ColumnId,N'{txtTag.Text}'
                                               from Table_045_PersonInfo
                                               where ColumnId = (select MAX(ColumnId) from Table_045_PersonInfo)";
                            db.Execute(query, null, commandType: CommandType.Text);
                        }
                    }
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                    bt_New.Enabled = true;

                }
            }
            catch (System.Data.SqlClient.SqlException es)
            {
                if (!es.Message.StartsWith("Object reference not set to an instance of an object."))

                    Class_BasicOperation.CheckSqlExp(es, "Frm_40_PersonInfo");
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Object reference not set to an instance of an object."))

                    Class_BasicOperation.CheckExceptionType(ex, "Frm_40_PersonInfo");

            }
            //catch (Exception ex)
            //{ Class_BasicOperation.CheckExceptionType(ex, "Frm_40_PersonInfo"); 
            //}
        }

        private void bt_Delte_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این شخص هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    try
                    {

                        DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT     dbo.Table_020_HeaderReciptClothRow.CodeCustomer
                            FROM         dbo.Table_020_DetailReciptClothRaw INNER JOIN
                             dbo.Table_020_HeaderReciptClothRow ON dbo.Table_020_DetailReciptClothRaw.FK = dbo.Table_020_HeaderReciptClothRow.ID
                            WHERE     (dbo.Table_020_HeaderReciptClothRow.CodeCustomer = " + gridEX1.GetValue("ColumnId") + @")
                                union all
                            SELECT     dbo.Table_025_HederOrderColor.CodeCustomer
                        FROM         dbo.Table_025_HederOrderColor INNER JOIN
                      dbo.Table_030_DetailOrderColor ON dbo.Table_025_HederOrderColor.ID = dbo.Table_030_DetailOrderColor.Fk
                                WHERE     (Table_025_HederOrderColor.CodeCustomer = " + gridEX1.GetValue("ColumnId") + ")");


                        DataTable dtCheck = ClDoc.ReturnTable(ConPACNT, @"select tbl.*,
PERP_MAIN.dbo.Table_000_OrgInfo.Column01  as  CompName,
PERP_MAIN.dbo.Table_000_OrgInfo.Column00 as Codecomp from (
 SELECT name,SUBSTRING(Name,7,(CHARINDEX('_',Name,7)-7)) as code 
  FROM sys.databases where name Like 'PACNT_%' ) as tbl left outer join PERP_MAIN.dbo.Table_000_OrgInfo
 on PERP_MAIN.dbo.Table_000_OrgInfo.ColumnId=code
 where code=" + Class_BasicOperation._OrgCode + "");


                        if (dtCheck.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtCheck.Rows)
                            {
                                CheckRelation.PersonInSanad(int.Parse(
                                        ((DataRowView)this.table_045_PersonInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString())
                                        , dr[0].ToString());
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("برای این شخص کارت تولید ثبت شده است امکان حذف آن را ندارید");
                            return;
                        }
                        else
                        {
                            table_045_PersonInfoBindingSource.RemoveCurrent();
                            table_045_PersonInfoBindingSource.EndEdit();
                            table_045_PersonInfoTableAdapter.Update(dataSet_10_PersonInfo.Table_045_PersonInfo);
                            Class_BasicOperation.ShowMsg("", "شخص مورد نظر حذف گردید", Class_BasicOperation.MessageType.Information);
                        }
                    }
                    catch (Exception ex)
                    { Class_BasicOperation.CheckExceptionType(ex, this.Name); }
                }
            }

        }

        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            table_045_PersonScopeBindingSource.EndEdit();
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
        private void chk_General_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_General.Checked)
            {

                cmb_Scope.Enabled = false;
                cmb_Scope.UncheckAll();
            }
            else
                if (txt_Code.Text != "" && txt_Name.Text.Trim() != "")
                cmb_Scope.Enabled = true;

        }
        private void cmb_Scope_Enter(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MoveNext();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MovePrevious();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_SendToExcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXExporter1.GridEX = gridEX1;
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", Class_BasicOperation.MessageType.Information);
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {

        }

        private void Frm_40_PersonInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                bt_Save_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.N)
            {
                bt_New_Click(sender, e);
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {
                bt_Delte_Click(sender, e);
            }
        }

        private void mlt_Users_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void mlt_Users_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_NationalCode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void table_045_PersonInfoBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (table_045_PersonInfoBindingSource.Count > 0)
            {
                rdb_Active_CheckedChanged(sender, e);
                rdb_Inactive_CheckedChanged(sender, e);
                rdb_Legal_CheckedChanged(sender, e);
                rdb_Righful_CheckedChanged(sender, e);
            }
        }

        private void txt_FirstName_TextChanged(object sender, EventArgs e)
        {
            txt_Name.Text = txt_LastName.Text + " " + txt_FirstName.Text;
        }

        private void Frm_40_PersonInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }





    }
}
