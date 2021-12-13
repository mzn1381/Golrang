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

namespace PCLOR._03_Bank
{
    public partial class Form03_BanksBox : Form
    {
        bool _Del = false, _DelDoc = false;
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.PACNT);
        SqlConnection conBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
        SqlDataAdapter DocAdapter, PeopleAdapter, BanksAdapter, BranchAdapter,HeadersAdapter;
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        public Form03_BanksBox(bool Del,bool DelDoc)
        {
            InitializeComponent();
            _Del = Del;
            _DelDoc = DelDoc;
        }

        private void Form03_BanksBox_Load(object sender, EventArgs e)
        {
         
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
           
            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
            }

            PeopleAdapter = new SqlDataAdapter("select Table_045_PersonScope.Column01 as ColumnId,Table_045_PersonInfo.Column01 as Column01,Table_045_PersonInfo.Column02 as Column02 from Table_045_PersonScope INNER Join Table_045_PersonInfo On Table_045_PersonInfo.ColumnId=Table_045_PersonScope.Column01 where Table_045_PersonScope.Column02=13  ", conBase);
            PeopleAdapter.Fill(dataSet1, "Person");
            gridEX1.DropDowns["Person"].SetDataBinding(dataSet1.Tables["Person"], "");

            DocAdapter = new SqlDataAdapter("Select Column00,ColumnId from Table_060_SanadHead", ConAcnt);
            DocAdapter.Fill(dataSet1, "Doc");
            gridEX1.DropDowns["Sanad"].SetDataBinding(dataSet1.Tables["Doc"], "");

            gridEX1.DropDowns["Header1"].DataSource = ClDoc.ReturnTable(ConAcnt, "Select ACC_Code,ACC_Name from AllHeaders()");

            BanksAdapter = new SqlDataAdapter("Select Column00,Column01 from Table_010_BankNames", ConBank);
            BanksAdapter.Fill(dataSet1, "Bank");
            BranchAdapter = new SqlDataAdapter("Select Column00,Column01,Column02 from  Table_015_BankBranchInfo", ConBank);
            BranchAdapter.Fill(dataSet1, "Branch");
            DataRelation Relation = new DataRelation("R_Bank_Branch", dataSet1.Tables["Bank"].Columns["Column00"], dataSet1.Tables["Branch"].Columns["Column00"]);
            dataSet1.Relations.Add(Relation);
            BankBindingSource.DataSource = dataSet1.Tables["Bank"];
            BranchBindingSource.DataSource = BankBindingSource;
            BranchBindingSource.DataMember = "R_Bank_Branch";
            gridEX1.DropDowns["Banks"].SetDataBinding(BankBindingSource, "");
            gridEX1.DropDowns["Branch"].SetDataBinding(BranchBindingSource, "");
            //this.table_020_BankCashAccInfoTableAdapter.Fill(this.dataSet_01_Cash.Table_020_BankCashAccInfo);
            if (isadmin)
            {
                gridEX_List.DataSource = ClDoc.ReturnTable(ConBank, @"select * from Table_020_BankCashAccInfo");

            }
            else
            {
                gridEX_List.DataSource = ClDoc.ReturnTable(ConBank, @"select * from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + conBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "') ");

            }
            textBox1_TextChanged(sender, e);
            gridEX1.MoveLast();
            bindingNavigatorMoveLastItem_Click(sender, e);
            
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                this.table_020_BankCashAccInfoBindingSource.EndEdit();
                if (gridEX1.GetValue("Column01").ToString() == "True")
                {
                    if (gridEX1.GetValue("Column35") == DBNull.Value)
                        throw new Exception("صندوقدار را مشخص کنید");
                }
                else
                {
                    if (gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column04") == DBNull.Value || gridEX1.GetValue("Column05") == DBNull.Value || gridEX1.GetValue("Column35") == DBNull.Value)
                        throw new Exception("اطلاعات حساب بانکی را مشخص کنید");
                }
                string Branch = ClDoc.ExScalar(conBase.ConnectionString, @"select ISNULL(( select distinct column133 from Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "'),0) AS Branch");
                ((DataRowView)table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["Column37"]= Branch;

                dataSet_01_Cash.EnforceConstraints = false;
                this.table_020_BankCashAccInfoBindingSource.EndEdit();
                this.table_020_BankCashAccInfoTableAdapter.Update(dataSet_01_Cash.Table_020_BankCashAccInfo);
                dataSet_01_Cash.EnforceConstraints = true;
                
                _03_Bank.Form06_ExportDocumentForFirstValue frm = new Form06_ExportDocumentForFirstValue(short.Parse(gridEX1.GetValue("ColumnId").ToString()));
                frm.ShowDialog();
                dataSet1.Tables["Doc"].Clear();
                DocAdapter.Fill(dataSet1, "Doc");
                int pos = this.table_020_BankCashAccInfoBindingSource.Position;
                this.table_020_BankCashAccInfoBindingSource.Position = pos;
                //Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", Class_BasicOperation.MessageType.Information);
                table_020_BankCashAccInfoBindingSource_PositionChanged(sender, e);
                this.table_020_BankCashAccInfoTableAdapter.FillByID(dataSet_01_Cash.Table_020_BankCashAccInfo,Convert.ToInt16(((DataRowView)table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["Columnid"]));

            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.Enabled = true;
                gridEX1.MoveToNewRecord();
                gridEX1.SetValue("Column01", DBNull.Value);
                superTabItem1.Text = "اطلاعات حساب/صندوق ";
                gridEX1.Select();
                gridEX1.Col = 0;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 91))
            {
                if (this.table_020_BankCashAccInfoBindingSource.Count > 0)
                {
                    if (_Del)
                    {
                        string Message = "آیا مایل به حذف صندوق/بانک معرفی شده هستید؟";
                        if (gridEX1.GetValue("Column32").ToString() != "0")
                            Message = "برای موجودی اولیه صندوق/بانک مورد نظر سند حسابداری صادر شده است. در صورت حذف ثبت مربوط نیز حذف می گردد" + Environment.NewLine + " آیا مایل به حذف صندوق/بانک مورد نظر هستید؟";

                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            int CurrentPos = this.table_020_BankCashAccInfoBindingSource.Position;
                            try
                            {
                                if (gridEX1.GetValue("Column32").ToString() != "0")
                                {
                                    ClDoc.DeleteDetail_ID(int.Parse(gridEX1.GetRow().Cells["Column32"].Value.ToString()), 14, int.Parse(gridEX1.GetValue("ColumnId").ToString()));
                                }

                                this.table_020_BankCashAccInfoBindingSource.RemoveCurrent();
                                this.table_020_BankCashAccInfoBindingSource.EndEdit();
                                this.table_020_BankCashAccInfoTableAdapter.Update(dataSet_01_Cash.Table_020_BankCashAccInfo);
                                Class_BasicOperation.ShowMsg("", "حذف صندوق/بانک مورد نظر انجام شد", Class_BasicOperation.MessageType.Information);
                            }
                            catch (Exception ex)
                            {
                                this.table_020_BankCashAccInfoTableAdapter.Fill(dataSet_01_Cash.Table_020_BankCashAccInfo);
                                this.table_020_BankCashAccInfoBindingSource.Position = CurrentPos;
                                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            }
                        }
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "Column01" )
                    if (gridEX1.GetValue("Column01").ToString() == "False")
                    {
                        gridEX1.RootTable.Columns["Column03"].Selectable = true;
                        gridEX1.RootTable.Columns["Column04"].Selectable = true;
                        gridEX1.RootTable.Columns["Column05"].Selectable = true;
                        gridEX1.RootTable.Columns["Column35"].Selectable = true;
                        gridEX1.RootTable.Columns["Column18"].Selectable = true;
                        gridEX1.RootTable.Columns["Column24"].Selectable = true;
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column03"].Selectable =  false;
                        gridEX1.RootTable.Columns["Column04"].Selectable = false;
                        gridEX1.RootTable.Columns["Column05"].Selectable = false;
                        gridEX1.RootTable.Columns["Column35"].Selectable = true;
                        gridEX1.RootTable.Columns["Column18"].Selectable = false;
                        gridEX1.RootTable.Columns["Column24"].Selectable = false;
                    }

                if (e.Column.Key == "Column03")
                {
                    try
                    {
                        BankBindingSource.Position = BankBindingSource.Find("Column00", gridEX1.GetValue("Column03").ToString());
                    }
                    catch { }
                }

                if (e.Column.Key == "Column12")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column12", "ACC_Code", "ACC_Name", gridEX1.EditTextBox.Text);

            }
            catch 
            {
            }
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column11" )
                    gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.None;
                else gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            }
            catch
            {
            }
        }

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
            catch
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
        }

        private void bt_Export_Click(object sender, EventArgs e)
        {
            //try
            //{
            //if (this.table_020_BankCashAccInfoBindingSource.Count > 0 && gridEX1.GetValue("Column32").ToString()=="0")
            //{
            //    ClDoc.CheckExistFinalDoc();
            //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 5))
            //        {
            //            _03_Bank.Form06_ExportDocumentForFirstValue frm = new Form06_ExportDocumentForFirstValue(short.Parse(gridEX1.GetValue("ColumnId").ToString()));
            //            frm.ShowDialog();
            //            dataSet1.Tables["Doc"].Clear();
            //            DocAdapter.Fill(dataSet1, "Doc");
            //            int pos = this.table_020_BankCashAccInfoBindingSource.Position;
            //            this.table_020_BankCashAccInfoTableAdapter.Fill(dataSet_01_Cash.Table_020_BankCashAccInfo);
            //            this.table_020_BankCashAccInfoBindingSource.Position = pos;
                        
            //        }
            //        else
            //            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            //}
            //}
            //catch(Exception ex)
            //{
            //    Class_BasicOperation.CheckExceptionType(ex,this.Name);
            //}
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 92))
                {
                    if (gridEX1.GetValue("Column32").ToString() != "0")
                    {
                        if (_DelDoc)
                        {
                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف ثبت حسابداری مربوط به این صندوق/بانک هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                ClDoc.DeleteDetail_ID(int.Parse(gridEX1.GetRow().Cells["Column32"].Value.ToString()), 14, int.Parse(gridEX1.GetValue("ColumnId").ToString()));
                                ClDoc.Update_Des_Table(ConBank.ConnectionString, "Table_020_BankCashAccInfo", "Column32", "ColumnId", int.Parse(gridEX1.GetValue("ColumnId").ToString()), 0);
                                int pos = this.table_020_BankCashAccInfoBindingSource.Position;
                              this.table_020_BankCashAccInfoTableAdapter.FillByID(dataSet_01_Cash.Table_020_BankCashAccInfo,Convert.ToInt16(((DataRowView)table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["Columnid"]));
                                
                                this.table_020_BankCashAccInfoBindingSource.Position = pos;
                                Class_BasicOperation.ShowMsg("", "حذف ثبت حسابداری مربوط به این صندوق/بانک با موفقیت انجام گرفت", Class_BasicOperation.MessageType.Information);
                            }
                        }

                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف سند حسابداری را ندارید", Class_BasicOperation.MessageType.None);
                    }

                    if (gridEX1.GetValue("Column32").ToString() == "0")
                    {
                        ChangeSelectable();
                        gridEX1.Enabled = true;
                    }
                    else
                        gridEX1.Enabled = false;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BankBindingSource.Position = BankBindingSource.Find("Column00", textBox1.Text);
            }
            catch { }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (gridEX1.GetValue("Column01").ToString() == "True")
            //    {
            //        if (gridEX1.GetValue("Column35") == DBNull.Value)
            //            throw new Exception("صندوقدار را مشخص کنید");
            //    }
            //    else if (gridEX1.GetValue("Column01").ToString() == "False")
            //    {
            //        if (gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column04") == DBNull.Value || gridEX1.GetValue("Column05") == DBNull.Value)
            //            throw new Exception("مشخصات بانک و حساب را کامل کنید");
            //    }
            //    table_020_BankCashAccInfoBindingSource.MoveNext();
            //    if (this.table_020_BankCashAccInfoBindingSource.Position == this.table_020_BankCashAccInfoBindingSource.Count - 1)
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = true;
            //        bindingNavigatorMovePreviousItem.Enabled = true;
            //        bindingNavigatorMoveNextItem.Enabled = false;
            //        bindingNavigatorMoveLastItem.Enabled = false;

            //    }
            //    else
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = true;
            //        bindingNavigatorMovePreviousItem.Enabled = true;
            //        bindingNavigatorMoveNextItem.Enabled = true;
            //        bindingNavigatorMoveLastItem.Enabled = true;
            //    }
            //    if (gridEX1.GetValue("Column32").ToString() == "0")
            //    {
            //        ChangeSelectable();
            //        gridEX1.Enabled = true;
            //    }
            //    else
            //        gridEX1.Enabled = false;

            //}
            //catch (Exception ex)
            //{
            //    Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //}
            if (this.table_020_BankCashAccInfoBindingSource.Count > 0)
            {

                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();

                    table_020_BankCashAccInfoBindingSource.EndEdit();

                    if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender,e);
                        }
                        
                    }
                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_020_BankCashAccInfo where ColumnId>" + ((DataRowView)this.table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select Min(ColumnId) from Table_020_BankCashAccInfo where ColumnId>" + ((DataRowView)this.table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + " and Column37 in(select Column133 from " + conBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')),0) as Row");

                    }
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        Int16 RowId = Int16.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_020_BankCashAccInfoTableAdapter.FillByID(this.dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                        this.table_020_BankCashAccInfoTableAdapter.FillByID(dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_020_BankCashAccInfoBindingSource_PositionChanged(sender, e);
                        
                    }

                    if (gridEX1.GetValue("Column32").ToString() == "0")
                    {
                        ChangeSelectable();
                        gridEX1.Enabled = true;
                    }
                    else
                        gridEX1.Enabled = false;
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }
        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (gridEX1.GetValue("Column01").ToString() == "True")
            //    {
            //        if (gridEX1.GetValue("Column35") == DBNull.Value)
            //            throw new Exception("صندوقدار را مشخص کنید");
            //    }
            //    else if (gridEX1.GetValue("Column01").ToString() == "False")
            //    {
            //        if (gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column04") == DBNull.Value || gridEX1.GetValue("Column05") == DBNull.Value)
            //            throw new Exception("مشخصات بانک و حساب را کامل کنید");
            //    }
            //    table_020_BankCashAccInfoBindingSource.MoveLast();
            //    if (this.table_020_BankCashAccInfoBindingSource.Position == this.table_020_BankCashAccInfoBindingSource.Count-1)
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = true;
            //        bindingNavigatorMovePreviousItem.Enabled = true;
            //        bindingNavigatorMoveNextItem.Enabled = false;
            //        bindingNavigatorMoveLastItem.Enabled = false;

            //    }
            //    else
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = true;
            //        bindingNavigatorMovePreviousItem.Enabled = true;
            //        bindingNavigatorMoveNextItem.Enabled = true;
            //        bindingNavigatorMoveLastItem.Enabled = true;
            //    }
     
            //}
            //catch (Exception ex)
            //{
            //    Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //}

            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();

                table_020_BankCashAccInfoBindingSource.EndEdit();
                
                if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender,e);
                    }
                }
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_020_BankCashAccInfo),0) as Row");
                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select max(ColumnId) from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + conBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')),0) as Row");

                }
                
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    Int16 RowId = Int16.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_020_BankCashAccInfoTableAdapter.FillByID(this.dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                    this.table_020_BankCashAccInfoTableAdapter.FillByID(dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_020_BankCashAccInfoBindingSource_PositionChanged(sender, e);
                    
                }
                if (gridEX1.GetValue("Column32").ToString() == "0")
                {
                    ChangeSelectable();
                    gridEX1.Enabled = true;
                }
                else
                    gridEX1.Enabled = false;
            }
            catch
            {
            }


        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (gridEX1.GetValue("Column01").ToString() == "True")
            //    {
            //        if (gridEX1.GetValue("Column35") == DBNull.Value)
            //            throw new Exception("صندوقدار را مشخص کنید");
            //    }
            //    else if (gridEX1.GetValue("Column01").ToString() == "False")
            //    {
            //        if (gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column04") == DBNull.Value || gridEX1.GetValue("Column05") == DBNull.Value)
            //            throw new Exception("مشخصات بانک و حساب را کامل کنید");
            //    }
            //    table_020_BankCashAccInfoBindingSource.MovePrevious();
            //    if (this.table_020_BankCashAccInfoBindingSource.Position == 0)
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = false;
            //        bindingNavigatorMovePreviousItem.Enabled = false;
            //        bindingNavigatorMoveNextItem.Enabled = true;
            //        bindingNavigatorMoveLastItem.Enabled = true;

            //    }
            //    else
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = true;
            //        bindingNavigatorMovePreviousItem.Enabled = true;
            //        bindingNavigatorMoveNextItem.Enabled = true;
            //        bindingNavigatorMoveLastItem.Enabled = true;
            //    }
            //    if (gridEX1.GetValue("Column32").ToString() == "0")
            //    {
            //        ChangeSelectable();
            //        gridEX1.Enabled = true;
            //    }
            //    else
            //        gridEX1.Enabled = false;

            //}
            //catch (Exception ex)
            //{
            //    Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //}
           
            if (this.table_020_BankCashAccInfoBindingSource.Count > 0)
            {
                try
                {
                    bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                    DataTable Table = new DataTable();
                    table_020_BankCashAccInfoBindingSource.EndEdit();

                    if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender,e);
                        }
                    }

                    if (isadmin)
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                            "Select ISNULL((Select max(ColumnId) from Table_020_BankCashAccInfo where ColumnId<" +
                            ((DataRowView)this.table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + "),0) as Row");
                    }
                    else
                    {
                        Table = ClDoc.ReturnTable(ConBank,
                           "Select ISNULL((Select max(ColumnId) from Table_020_BankCashAccInfo where ColumnId<" +
                           ((DataRowView)this.table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + " and Column37 in(select Column133 from " + conBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')),0) as Row");
                    }

                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        Int16 RowId = Int16.Parse(Table.Rows[0]["Row"].ToString());
                        dataSet_01_Cash.EnforceConstraints = false;
                        this.table_020_BankCashAccInfoTableAdapter.FillByID(this.dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                        this.table_020_BankCashAccInfoTableAdapter.FillByID(dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                        dataSet_01_Cash.EnforceConstraints = true;
                        this.table_020_BankCashAccInfoBindingSource_PositionChanged(sender, e);
                    }

                    if (gridEX1.GetValue("Column32").ToString() == "0")
                    {
                        ChangeSelectable();
                        gridEX1.Enabled = true;
                    }
                    else
                        gridEX1.Enabled = false;
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }




        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (gridEX1.GetValue("Column01").ToString() == "True")
            //    {
            //        if (gridEX1.GetValue("Column35") == DBNull.Value)
            //            throw new Exception("صندوقدار را مشخص کنید");
            //    }
            //    else if (gridEX1.GetValue("Column01").ToString() == "False")
            //    {
            //        if (gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column04") == DBNull.Value || gridEX1.GetValue("Column05") == DBNull.Value)
            //            throw new Exception("مشخصات بانک و حساب را کامل کنید");
            //    }
            //    table_020_BankCashAccInfoBindingSource.MoveFirst();
            //    if (this.table_020_BankCashAccInfoBindingSource.Position == 0)
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = false;
            //        bindingNavigatorMovePreviousItem.Enabled = false;
            //        bindingNavigatorMoveNextItem.Enabled = true;
            //        bindingNavigatorMoveLastItem.Enabled = true;

            //    }
            //    else
            //    {
            //        bindingNavigatorMoveFirstItem.Enabled = true;
            //        bindingNavigatorMovePreviousItem.Enabled = true;
            //        bindingNavigatorMoveNextItem.Enabled = true;
            //        bindingNavigatorMoveLastItem.Enabled = true;
            //    }
            //    if (gridEX1.GetValue("Column32").ToString() == "0")
            //    {
            //        ChangeSelectable();
            //        gridEX1.Enabled = true;
            //    }
            //    else
            //        gridEX1.Enabled = false;
            //}
            //catch (Exception ex)
            //{
            //    Class_BasicOperation.CheckExceptionType(ex, this.Name);
            //}

            try
            {

                table_020_BankCashAccInfoBindingSource.EndEdit();
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
                DataTable Table = new DataTable();
                if (dataSet_01_Cash.Table_035_ReceiptCheques.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender,e);
                    }
                }
                if (isadmin)
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_020_BankCashAccInfo),0) as Row");

                }
                else
                {
                    Table = ClDoc.ReturnTable(ConBank, "Select ISNULL((Select min(ColumnId) from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + conBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "')),0) as Row");

                }
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    Int16 RowId = Int16.Parse(Table.Rows[0]["Row"].ToString());
                    dataSet_01_Cash.EnforceConstraints = false;
                    this.table_020_BankCashAccInfoTableAdapter.FillByID (this.dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                    this.table_020_BankCashAccInfoTableAdapter.FillByID(dataSet_01_Cash.Table_020_BankCashAccInfo, RowId);
                    dataSet_01_Cash.EnforceConstraints = true;
                    this.table_020_BankCashAccInfoBindingSource_PositionChanged(sender, e);
                }
                if (gridEX1.GetValue("Column32").ToString() == "0")
                {
                    ChangeSelectable();
                    gridEX1.Enabled = true;
                }
                else
                    gridEX1.Enabled = false;

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }



        }
        private void ChangeSelectable()
        {
            if (gridEX1.GetValue("Column01").ToString() == "False")
            {
                gridEX1.RootTable.Columns["Column03"].Selectable = true;
                gridEX1.RootTable.Columns["Column04"].Selectable = true;
                gridEX1.RootTable.Columns["Column05"].Selectable = true;
                gridEX1.RootTable.Columns["Column35"].Selectable = true;
                gridEX1.RootTable.Columns["Column18"].Selectable = true;
                gridEX1.RootTable.Columns["Column24"].Selectable = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Column03"].Selectable = false;
                gridEX1.RootTable.Columns["Column04"].Selectable = false;
                gridEX1.RootTable.Columns["Column05"].Selectable = false;
                gridEX1.RootTable.Columns["Column35"].Selectable = true;
                gridEX1.RootTable.Columns["Column18"].Selectable = false;
                gridEX1.RootTable.Columns["Column24"].Selectable = false;
            }

        }

        private void Form03_BanksBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.N)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E)
                bt_Export_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.L)
                bt_DelDoc_Click(sender, e);
            //else if (e.Control && e.KeyCode == Keys.P)
            //    bt_Print_Click(sender, e);
        }

        //private void mnu_DefineBanks_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 2))
        //    {
        //        _03_Bank.Form02_BanksBranches frm = new PACNT._03_Bank.Form02_BanksBranches(
        //              UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 3));
        //        frm.ShowDialog();
        //        dataSet1.EnforceConstraints = false;
        //        dataSet1.Tables["Bank"].Clear();
        //        dataSet1.Tables["Branch"].Clear();
        //        dataSet1.EnforceConstraints = true;
        //        BanksAdapter.Fill(dataSet1, "Bank");
        //        BranchAdapter.Fill(dataSet1, "Branch");
          
        //            try
        //            {
        //                BankBindingSource.Position = BankBindingSource.Find("Column00", gridEX1.GetValue("Column03").ToString());
        //            }
        //            catch { }
              
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        //}

        private void mnu_DefinePeople_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();
                dataSet1.Tables["Person"].Clear();
                PeopleAdapter.Fill(dataSet1, "Person");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        //private void mnu_DefineHeaders_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 11))
        //    {
        //        PACNT._1_BasicMenu.Form06_AccountingHeaders frm = new PACNT._1_BasicMenu.Form06_AccountingHeaders(
        //           UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 12));
        //        frm.ShowDialog();
        //        dataSet1.Tables["Headers"].Clear();
        //        HeadersAdapter.Fill(dataSet1, "Headers");
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        //}

        private void mnu_ControlNature_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 30))
            {
                PACNT._1_BasicMenu.Form12_ControlAccounts frm = new PACNT._1_BasicMenu.Form12_ControlAccounts();
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        //private void mnu_ViewDocs_Click(object sender, EventArgs e)
        //{
        //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 22))
        //    {
        //        foreach (Form item in Application.OpenForms)
        //        {
        //            if (item.Name == "Form04_ViewDocument")
        //            {
        //                item.BringToFront();
        //                ((_2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Position =
        //                    ((_2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Find("ColumnId", int.Parse(gridEX1.GetValue("Column32").ToString()));
        //                return;
        //            }
        //        }
        //        _2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument(int.Parse(gridEX1.GetValue("Column32").ToString()));
        //        try
        //        {
        //            frm.MdiParent = MainForm.ActiveForm;
        //        }
        //        catch { }
        //        frm.Show();
        //    }
        //    else
        //        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        //}

        private void table_020_BankCashAccInfoBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_020_BankCashAccInfoBindingSource.Count > 0)
            {
                try
                {

                    if (((DataRowView)this.table_020_BankCashAccInfoBindingSource.CurrencyManager.Current)["Column32"].ToString()!="0")
                    {
                        gridEX1.Enabled = false;
                    }
                    else
                    {
                        gridEX1.Enabled = true;
                    }
                }
                catch
                { }
                try
                {
                    if (gridEX_List.GetValue("Column01").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column03"].Selectable = false;
                        gridEX1.RootTable.Columns["Column04"].Selectable = false;
                        gridEX1.RootTable.Columns["Column05"].Selectable = false;
                        gridEX1.RootTable.Columns["Column35"].Selectable = true;
                        gridEX1.RootTable.Columns["Column18"].Selectable = false;
                        gridEX1.RootTable.Columns["Column24"].Selectable = false;
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column03"].Selectable = true;
                        gridEX1.RootTable.Columns["Column04"].Selectable = true;
                        gridEX1.RootTable.Columns["Column05"].Selectable = true;
                        gridEX1.RootTable.Columns["Column35"].Selectable = false;
                        gridEX1.RootTable.Columns["Column18"].Selectable = true;
                        gridEX1.RootTable.Columns["Column24"].Selectable = true;
                    }
                }
                catch 
                { }
            }
        }

        //private void bt_Print_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _4_Reports.DataSet_Reports dataSet_Reports = new _4_Reports.DataSet_Reports();
        //        DataTable Table = dataSet_Reports.BanksBox.Clone();
        //        foreach(Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
        //        {
        //            Table.Rows.Add((item.Cells["Column01"].Value.ToString() == "True" ? "صندوق" : "بانک"),
        //                item.Cells["Column02"].Text,
        //                item.Cells["Column03"].Text.ToString().Trim(),
        //                item.Cells["Column04"].Text.ToString().Trim(),
        //                item.Cells["Column05"].Text.ToString().Trim(),
        //                Int64.Parse(item.Cells["Column06"].Value.ToString()),
        //                item.Cells["Column34"].Text.ToString(),
        //                item.Cells["Column35"].Text,
        //                item.Cells["Column12"].Text,
        //                item.Cells["Column31"].Text.ToString());

        //        }
        //        if (Table.Rows.Count > 0)
        //        {
        //            _4_Reports.Reports.ReportForm frm = new _4_Reports.Reports.ReportForm(20, Table, " ", " ");
        //            frm.Show();
        //        }
        //    }
        //    catch { }
        //}

        private void bt_SendToExcel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", Class_BasicOperation.MessageType.Information);
            }
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column12");
            }
            catch 
            {
            }
            try
            {
                if (e.Column.Key == "Column12")
                {
                    string[] _ACCInfo = ClDoc.ACC_Info(gridEX1.GetValue("Column12").ToString());
                    gridEX1.SetValue("Column07", _ACCInfo[0]);
                    gridEX1.SetValue("Column08",(_ACCInfo[1].ToString() == "" ?(object)DBNull.Value : _ACCInfo[1].ToString()));
                    gridEX1.SetValue("Column09", (_ACCInfo[2].ToString() == "" ? (object)DBNull.Value : _ACCInfo[2].ToString()));
                    gridEX1.SetValue("Column10", (_ACCInfo[3].ToString() == "" ? (object)DBNull.Value : _ACCInfo[3].ToString()));
                    gridEX1.SetValue("Column011", (_ACCInfo[4].ToString() == "" ? (object)DBNull.Value : _ACCInfo[4].ToString()));
                }
            }
            catch 
            {
            }
        }

        private void mnu_DefineBanks_Click(object sender, EventArgs e)
        {

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            

            try
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.PACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._FinYear;

                PACNT._4_Reports.DataSet_Reports dataSet_Reports = new PACNT._4_Reports.DataSet_Reports();
                DataTable Table = dataSet_Reports.BanksBox.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add((item.Cells["Column01"].Value.ToString() == "True" ? "صندوق" : "بانک"),
                        item.Cells["Column02"].Text,
                        item.Cells["Column03"].Text.ToString().Trim(),
                        item.Cells["Column04"].Text.ToString().Trim(),
                        item.Cells["Column05"].Text.ToString().Trim(),
                        Int64.Parse(item.Cells["Column06"].Value.ToString()),
                        item.Cells["Column34"].Text.ToString(),
                        item.Cells["Column35"].Text,
                        item.Cells["Column12"].Text,
                        item.Cells["Column31"].Text.ToString());

                }
                if (Table.Rows.Count > 0)
                {
                    PACNT._4_Reports.Reports.ReportForm frm = new PACNT._4_Reports.Reports.ReportForm(20, Table, " ", " ");
                    frm.Show();
                }
            }
            catch { }

        }
    }
}
