using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using PCLOR.Models;
using PCLOR.Classes;
using System.IO.Ports;
using System.IO;

namespace PCLOR._01_OperationInfo
{
    public partial class Frm_60_ReciptKnitting : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPWHRS = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        SerialPort comport = new SerialPort();
        StringBuilder sb;
        string ouptputtext = "";


        public Frm_60_ReciptKnitting()
        {
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            InitializeComponent();
        }

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

            txtWeight.BeginInvoke(new EventHandler(delegate
            {
                try
                {
                    txtWeight.Text = (Convert.ToInt64(s.Substring(0, 8).Replace("BB", "")) / 1000.0).ToString();
                }
                catch { s = ""; }
                if (s.Length > 8)
                    s = "";

            }));


        }
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
                catch { }
                try
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

        private void Frm_05_TypeCloth_Load(object sender, EventArgs e)
        {
            try
            {
                var machines = ClDoc.ReturnTable(ConPCLOR, @"select ID,Code,namemachine from Table_60_SpecsTechnical");
                gridEX1.DropDowns["Machine"].DataSource = machines;
                cmbMachine.DataSource = machines;

                var typeCloths = ClDoc.ReturnTable(ConPCLOR, @"select ID, TypeCloth,Number,CodeCommondity from Table_005_TypeCloth");
                gridEX1.DropDowns["TypeCloth"].DataSource = typeCloths;
                cmbTypeCloth.DataSource = typeCloths;

                this.table_100_KnittingTableAdapter.Fill(this.dataSet_05_PCLOR1.Table_100_Knitting);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 145))
            {
                table_100_KnittingBindingSource.AddNew();
                btn_New.Enabled = false;
                uiPanel0.Enabled = true;
                txtDate.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                btnSelectMachine.Focus();
            }
            else
            {

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }

        }
        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMachine.SelectedIndex.Equals(-1))
                {
                    MessageBox.Show("دستگاه را انتخاب نمایید");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDate.Text) || txtDate.Text.Length < 10)
                {
                    MessageBox.Show("تاریخ را وارد نمایید");
                    return;
                }
                if (cmbTypeCloth.SelectedItem == null)
                {
                    MessageBox.Show("نوع پارچه را انتخاب نمایید");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtWeight.Text))
                {
                    decimal weight = Convert.ToDecimal(txtWeight.Text);
                    if (weight == 0)
                    {
                        MessageBox.Show("وزن طاقه را وارد نمایید");
                        return;
                    }
                }

                var receipteID = ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["NumberRecipt"];

                if (receipteID != null && string.IsNullOrWhiteSpace(receipteID.ToString()) == false && ClDoc.ExExists(ConPWHRS.ConnectionString, "dbo.Table_011_PwhrsReceipt", "columnid", receipteID.ToString()))
                {
                    MessageBox.Show("رسید برای این شماره پارچه درج شده است  امکان ویرایش وجود ندارد");
                    return;
                }
                else
                {


                    bool tagVerify = Convert.ToBoolean(ClDoc.ExScalar(ConBase.ConnectionString, string.Format("select cast(case when exists (select 1 from [dbo].[Table_045_PersonInfo] where Column148=N'{0}') then 1 else 0 end as bit)", txtUserMachine.Text)));
                    if (tagVerify)
                    {
                        if (tagVerify.Equals(false))
                        {
                            MessageBox.Show("تگ وارد شده تعریف نشده است");
                            txtUserMachine.Focus();
                            return;
                        }
                    }

                    var number = ClDoc.ExScalar(ConPCLOR.ConnectionString, string.Format("select IsNull(MAX(Number), 999)+1 from Table_100_Knitting where Date='{0}' ", txtDate.Text));
                    var serial = ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["Serial"].ToString();
                    if (string.IsNullOrWhiteSpace(serial) == true || ClDoc.ExExists(ConPCLOR.ConnectionString, "[dbo].[Table_110_ProductionDetail]", "Serial", string.Format("'{0}'", serial)) == false)
                    {
                        ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["Serial"] = string.Format("{0}{1}", txtDate.Text.Replace("/", ""), number);
                    }

                    ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["Number"] = number;
                    ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                    ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["TimeSabt"] = Class_BasicOperation.ServerDate().ToString();


                    table_100_KnittingBindingSource.EndEdit();
                    table_100_KnittingTableAdapter.Update(this.dataSet_05_PCLOR1.Table_100_Knitting);

                    btn_Recipt_Click(sender, e);
                    this.table_100_KnittingTableAdapter.Fill(this.dataSet_05_PCLOR1.Table_100_Knitting);

                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                    uiPanel0.Enabled = false;
                    btn_New.Enabled = true;
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
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 146))
                    {
                        var recipteNumber = ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString();
                        if (string.IsNullOrWhiteSpace(recipteNumber) == false && ClDoc.ExExists(ConPWHRS.ConnectionString, "dbo.Table_011_PwhrsReceipt", "columnid", recipteNumber) == true)
                        {
                            MessageBox.Show("این برگه دارای رسید می باشد و امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var serial = ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["Serial"].ToString();
                        if (string.IsNullOrWhiteSpace(serial) == false)
                        {
                            if (ClDoc.ExExists(ConPCLOR.ConnectionString, "[dbo].[Table_110_ProductionDetail]", "Serial", string.Format("'{0}'", serial)))
                            {
                                MessageBox.Show("این برگه دارای کارت تولید می باشد و امکان حذف آن را ندارید", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            table_100_KnittingBindingSource.RemoveCurrent();
                            table_100_KnittingBindingSource.EndEdit();
                            table_100_KnittingTableAdapter.Update(this.dataSet_05_PCLOR1.Table_100_Knitting);
                            MessageBox.Show("اطلاعات با موفقیت حذف شد");
                            btn_New.Enabled = true;
                        }

                    }
                    else

                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void Frm_05_TypeCloth_KeyDown(object sender, KeyEventArgs e)
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

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void table_005_TypeClothBindingSource_PositionChanged(object sender, EventArgs e)
        {
            uiPanel0.Enabled = false;
        }

        private void فعالکردنپنلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 145))
            {
                uiPanel0.Enabled = true;
            }
            else

                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
        }

        private void btnSelectMachine_Click(object sender, EventArgs e)
        {
            Frm_05_Machines frm_05_Machines = new Frm_05_Machines();
            if (frm_05_Machines.ShowDialog() == DialogResult.OK)
            {
                cmbMachine.Value = frm_05_Machines.SelectedMachine.ID;
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
            }
        }

        private void rb_select_CheckedChanged(object sender, EventArgs e)
        {
            txtWeight.Text = "0";
        }


        int ResidNum = 0;


        private void btn_Recipt_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

            var functionID = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=30");
            var wareID = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=31");

            {
                ResidNum = ClDoc.MaxNumber(ConPWHRS.ConnectionString, "Table_011_PwhrsReceipt", "Column01");

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
                                                                 
                                                                          ) VALUES (" + ResidNum + ",'" + txtDate.Text + "' ," + wareID + "," + functionID + ","
                                                                   + 200.ToString() + ",'" + "رسید صادره بابت رسید پارچه از بافندگی ش" + txtKnittingID.Text + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()); SET @Key=Scope_Identity() ";

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
                           ) VALUES (@Key," + ((DataRowView)cmbTypeCloth.SelectedItem)["CodeCommondity"].ToString() + ",1," + 1.ToString() +
                "," + 1.ToString() + ",0,0,'" + Class_BasicOperation._UserName +
                "','" + Class_BasicOperation._UserName + "',getdate(),0,0," + (Convert.ToDecimal(txtWeight.Value) / Convert.ToDecimal(1)).ToString() + "," + Convert.ToDecimal(txtWeight.Value) + ",'" + cmbMachine.Text + "');";

                commandtxt += " Update " + ConPCLOR.Database + ".dbo.Table_100_Knitting set NumberRecipt=" + ResidNum.ToString() + ", ReciptID=@key where KnittingID = " + int.Parse(txtKnittingID.Text).ToString();
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
                        //MessageBox.Show("اطلاعات با موفقیت ذخیره شد و " + "رسید انبار به شماره " + ResidNum.ToString() + "صادر گردید");
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

        private void btnDeleteReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(gridEX1.CurrentRow.RowIndex) >= 0)
                {
                    Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 146))
                    {
                        var recipteNumber = ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString();
                        if (string.IsNullOrWhiteSpace(recipteNumber) == false && ClDoc.ExExists(ConPWHRS.ConnectionString, "dbo.Table_011_PwhrsReceipt", "columnid", recipteNumber))
                        {
                            if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string CommandTexxt = @"Delete from " + ConPWHRS.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 = " + recipteNumber + @"
                                                    Delete from " + ConPWHRS.Database + @".dbo.Table_011_PwhrsReceipt where columnid = " + recipteNumber;

                                Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);

                                ((DataRowView)table_100_KnittingBindingSource.CurrencyManager.Current)["NumberRecipt"] = 0;
                                table_100_KnittingBindingSource.EndEdit();
                                table_100_KnittingTableAdapter.Update(this.dataSet_05_PCLOR1.Table_100_Knitting);
                                this.table_100_KnittingTableAdapter.Fill(this.dataSet_05_PCLOR1.Table_100_Knitting);
                            }
                        }
                        else
                        {
                            MessageBox.Show("این برگه دارای رسید نمی باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }
    }
}
