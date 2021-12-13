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

namespace PCLOR._03_Bank
{
    public partial class Frm_13_Rasgiri : Form
    {
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        SqlDataAdapter Adapter;
        SqlDataAdapter GetAdapter;
        bool _BackSpace = false;

        public Frm_13_Rasgiri()
        {
            InitializeComponent();
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePicker)sender).HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker2_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePicker)sender).HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void mlt_BankBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }



        private void Frm_13_Rasgiri_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                 Adapter = new SqlDataAdapter(
              "Select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo", ConBank);
                DataTable BoxBankTable = new DataTable();
                DataTable BoxBankTable2 = new DataTable();
                Adapter.Fill(BoxBankTable);
                Adapter.Fill(BoxBankTable2);
                gridEX_Get.DropDowns["ToBank"].SetDataBinding(BoxBankTable, "");
                gridEX_Pay.DropDowns["ToBank"].SetDataBinding(BoxBankTable, "");
                BoxBankTable2.Rows.Add(0, 0, "همه صندوقها و حسابها");
                mlt_BankBox.DataSource = BoxBankTable2;
                mlt_Pay_BankBox.DataSource = BoxBankTable2;
            }
            else
            {
                Adapter = new SqlDataAdapter(
                 "Select ColumnId,Column01,Column02 from Table_020_BankCashAccInfo where Column37 in(select Column133 from " + ConBase.Database + @".dbo.Table_045_PersonInfo where Column23='" + Class_BasicOperation._UserName + "') ", ConBank);
                DataTable BoxBankTable = new DataTable();
                DataTable BoxBankTable2 = new DataTable();
                Adapter.Fill(BoxBankTable);
                Adapter.Fill(BoxBankTable2);
                gridEX_Get.DropDowns["ToBank"].SetDataBinding(BoxBankTable, "");
                gridEX_Pay.DropDowns["ToBank"].SetDataBinding(BoxBankTable, "");
                BoxBankTable2.Rows.Add(0, 0, "همه صندوقها و حسابها");
                mlt_BankBox.DataSource = BoxBankTable2;
                mlt_Pay_BankBox.DataSource = BoxBankTable2;
            }

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            gridEX_Get.DropDowns["Person"].SetDataBinding(Person, "");
            gridEX_SaleFactor.DropDowns["Person"].SetDataBinding(Person, "");
            gridEX_BuyFactor.DropDowns["Person"].SetDataBinding(Person, "");
            gridEX_Pay.DropDowns["Person"].SetDataBinding(Person, "");
            gridEX_All.DropDowns["Person"].DataSource = Person;


            GetAdapter = new SqlDataAdapter(@"SELECT   * from  dbo.Table_035_ReceiptCheques", ConBank);
            GetAdapter.Fill(dataSet1, "Get");
            GetbindingSource.DataSource = dataSet1.Tables["Get"];

            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip3.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip4.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip5.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip8.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip6.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip7.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePicker3.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker4.SelectedDateTime = DateTime.Now;
            faDatePicker5.SelectedDateTime = DateTime.Now;
            fa_fromDate_All.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            fa_ToDate_All.FADatePicker.SelectedDateTime = DateTime.Now;
            faDate_Base_All.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX_Get.DataSource = dataSet1.Tables["Get"];
            GetbindingSource.Filter = "ColumnId=0";

            UserbindingSource.DataSource = dataSet_01_Cash.Tables["Ras"];
            gridEX_User.DataSource = UserbindingSource;

            mlt_BankBox.Value = 0;
            mlt_Pay_BankBox.Value = 0;
        }

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePickerStrip)sender).FADatePicker.HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            try
            {
                bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

                if (faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue && faDatePicker1.Text.Trim() != "" && faDatePicker2.Text.Trim() != "" && mlt_BankBox.Text.Trim() != "")
                {
                    if (rdb_Get.Checked)
                    {
                        if (isadmin)
                        {
                            GetbindingSource.Filter = "(Column02>= '" + faDatePicker1.Text + "' and Column02<='" + faDatePicker2.Text + "')";
                            if (mlt_BankBox.Value.ToString() != "0")
                                GetbindingSource.Filter += " and Column01=" + mlt_BankBox.Value.ToString();

                        }
                        else
                        {
                            GetbindingSource.Filter = "(Column02>= '" + faDatePicker1.Text + "' and Column02<='" + faDatePicker2.Text + "')";
                            if (mlt_BankBox.Value.ToString() != "0")
                                GetbindingSource.Filter += " and Column01=" + mlt_BankBox.Value.ToString();
                            GetbindingSource.Filter = "(Column42='" + Class_BasicOperation._UserName + "')";
                        }
                    }

                    else
                    {
                        if (isadmin)
                        {
                            GetbindingSource.Filter = "(Column04 >= '" + faDatePicker1.Text + "' and  Column04<='" + faDatePicker2.Text + "')";
                            if (mlt_BankBox.Value.ToString() != "0")
                                GetbindingSource.Filter += " and Column01=" + mlt_BankBox.Value.ToString();
                        }
                        else
                        {
                            GetbindingSource.Filter = "(Column04 >= '" + faDatePicker1.Text + "' and  Column04<='" + faDatePicker2.Text + "')";
                            if (mlt_BankBox.Value.ToString() != "0")
                                GetbindingSource.Filter += " and Column01=" + mlt_BankBox.Value.ToString();
                            GetbindingSource.Filter = "(Column42='" + Class_BasicOperation._UserName + "')";
                        }
                        }
                    }
            }
            catch { }
        }

        private void faDatePicker3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePicker)sender).HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker3_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    ((FarsiLibrary.Win.Controls.FADatePicker)sender).HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker4_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void bt_Ras1_Click(object sender, EventArgs e)
        {
            try
            {
                Double SumPrice = 0;
                Double SumMult = 0;
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Get.GetCheckedRows())
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(item.Cells["Column04"].Text)).ToShortDateString());
                    DateTime SecondDate = faDatePickerStrip1.FADatePicker.SelectedDateTime.Value;
                    SecondDate = new DateTime(SecondDate.Year, SecondDate.Month, SecondDate.Day, 0, 0, 0);
                    TimeSpan Sub = BaseDate - SecondDate;
                    item.BeginEdit();
                    item.Cells["Distance"].Value = Sub.Days;
                    item.Cells["Mult"].Value = Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                        Convert.ToDouble(item.Cells["Distance"].Value.ToString());
                    item.EndEdit();

                    SumPrice += Convert.ToDouble(item.Cells["Column05"].Value.ToString());
                    SumMult += (Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * Convert.ToDouble(item.Cells["Distance"].Value.ToString()));
                }


                txt_Total1.Value = SumPrice;

                if (SumPrice != 0)
                    txt_Ras1.Value = SumMult / SumPrice;
                else txt_Ras1.Value = 0;

                if (Convert.ToDouble(txt_Ras1.Value.ToString()) != 0)
                {
                    DateTime FirstDate = faDatePickerStrip1.FADatePicker.SelectedDateTime.Value;// DateTime.Now;
                    faDate_Rec.SelectedDateTime = FirstDate.AddDays(Convert.ToDouble(txt_Ras1.Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Can not parse the value. Format is incorrect."))
                    Class_BasicOperation.ShowMsg("", "در میان سطرهای انتخاب شده، تاریخ نامعتبر وجود دارد", Class_BasicOperation.MessageType.Warning);
            }
        }

        private void bt_Ras_PaidChqs_Click(object sender, EventArgs e)
        {
            try
            {
                Double SumPrice = 0;
                Double SumMult = 0;
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Pay.GetCheckedRows())
                {
                    DateTime BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDateConverter.
                        ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Parse(item.Cells["Column06"].Text)).ToShortDateString());
                    DateTime SecondDate = faDatePicker5.SelectedDateTime.Value;
                    SecondDate = new DateTime(SecondDate.Year, SecondDate.Month, SecondDate.Day, 0, 0, 0);
                    TimeSpan Sub = BaseDate - SecondDate;
                    item.BeginEdit();
                    item.Cells["Distance"].Value = Sub.Days;
                    item.Cells["Mult"].Value = Convert.ToDouble(item.Cells["Column05"].Value.ToString()) * Convert.ToDouble(item.Cells["Distance"].Value.ToString());
                    item.EndEdit();

                    SumPrice += Convert.ToDouble(item.Cells["Column05"].Value.ToString());
                    SumMult += (Convert.ToDouble(item.Cells["Column05"].Value.ToString()) *
                        Convert.ToDouble(item.Cells["Distance"].Value.ToString()));
                }


                numericEditBox8.Value = SumPrice;

                if (SumPrice != 0)
                    numericEditBox7.Value = SumMult / SumPrice;
                else numericEditBox7.Value = 0;

                if (Convert.ToDouble(numericEditBox7.Value.ToString()) != 0)
                {
                    DateTime FirstDate = faDatePicker5.SelectedDateTime.Value;// DateTime.Now;
                    faDate_Pay.SelectedDateTime = FirstDate.AddDays(Convert.ToDouble(numericEditBox7.Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Can not parse the value. Format is incorrect."))
                    Class_BasicOperation.ShowMsg("", "در میان سطرهای انتخاب شده، تاریخ نامعتبر وجود دارد", Class_BasicOperation.MessageType.Warning);
            }
        }

        private void bt_View_PayChqs_Click(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);

            if (faDatePicker3.SelectedDateTime.HasValue && faDatePicker4.SelectedDateTime.HasValue && faDatePicker3.Text.Trim() != "" && faDatePicker4.Text.Trim() != "" && mlt_Pay_BankBox.Text.Trim() != "")
            {

                string SelectCommand = "Select ColumnId,Column01,Column09,Column03,Column05,Column06,Column07,null as Distance,0.000 as Mult from Table_030_ExportCheques";
                if (rdb_Pay_PayDate.Checked)
                {
                    if (isadmin)
                    {
                    SelectCommand += " where (Column04>= '" + faDatePicker3.Text + "' and Column04<='" + faDatePicker4.Text + "')";
                    if (mlt_Pay_BankBox.Value.ToString() != "0")
                        SelectCommand += " and Column01=" + mlt_Pay_BankBox.Value.ToString();
                      }
                    else
                    {
                        SelectCommand += " where (Column04>= '" + faDatePicker3.Text + "' and Column04<='" + faDatePicker4.Text + "')";
                        if (mlt_Pay_BankBox.Value.ToString() != "0")
                            SelectCommand += " and Column01=" + mlt_Pay_BankBox.Value.ToString();
                        SelectCommand = "Column24='" + Class_BasicOperation._UserName + "'";
                    }
                    }

                else
                {
                    if (isadmin)
                    {
                        SelectCommand += " where (Column06 >= '" + faDatePicker3.Text + "' and  Column06<='" + faDatePicker4.Text + "')";
                        if (mlt_Pay_BankBox.Value.ToString() != "0")
                            SelectCommand += " and Column01=" + mlt_Pay_BankBox.Value.ToString();
                    }
                    else
                    {
                        SelectCommand += " where (Column06 >= '" + faDatePicker3.Text + "' and  Column06<='" + faDatePicker4.Text + "')";
                        if (mlt_Pay_BankBox.Value.ToString() != "0")
                            SelectCommand += " and Column01=" + mlt_Pay_BankBox.Value.ToString();
                        SelectCommand = "Column24='" + Class_BasicOperation._UserName + "'";

                    }
                }

                SqlDataAdapter Adapter = new SqlDataAdapter(SelectCommand, ConBank);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX_Pay.DataSource = Table;
            }
        }
    }
}
