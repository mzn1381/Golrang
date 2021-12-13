using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;

namespace PCLOR._03_Bank
{
    public partial class Form01_PrintRecChq : Form
    {
        
        string _Param3, _Param4, _Param5, _Param6, _Param7, _Param8, _Param9, _Param10;
        string _Number = "";
        bool _BackSpace = false;
        SqlConnection ConBase = new SqlConnection(global::PACNT.Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
       
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        DataTable _Table = new DataTable();

        public Form01_PrintRecChq(DataTable Table,string Number)
        {
            InitializeComponent();
            _Table = Table;
            _Number = Number;
        }
        private void Form01_PrintAccDoc_Load(object sender, EventArgs e)
        {
            Chk_NoLogo.Checked = Properties.Settings.Default.PrintWithoutLogo;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_125_Signature where ColumnId=5", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);

            _Param3 = Table.Rows[0]["Column01"].ToString() ?? " ";
            _Param4 = Table.Rows[0]["Column02"].ToString() ?? " ";
            _Param5 = Table.Rows[0]["Column03"].ToString() ?? " ";
            _Param6 = Table.Rows[0]["Column04"].ToString() ?? " ";
            _Param7 = Table.Rows[0]["Column05"].ToString() ?? " ";
            _Param8 = Table.Rows[0]["Column06"].ToString() ?? " ";
            _Param9 = Table.Rows[0]["Column07"].ToString() ?? " ";
            _Param10 = Table.Rows[0]["Column08"].ToString() ?? " ";

            Adapter.SelectCommand.CommandText = "Select ColumnId,Column01,Column02 from Table_045_PersonInfo";
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            mlt_Person.DataSource = Person;

           
            fa_Date1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            fa_Date2.SelectedDateTime = DateTime.Now;
            buttonX1_Click(sender, e);
        }

        private void editBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var Rpt = (CrystalDecisions.CrystalReports.Engine.ReportDocument)crystalReportViewer1.ReportSource;
            //    Rpt.Database.Dispose();
            //    Rpt.Close();
            //    Rpt.Dispose();
            //    GC.Collect();
            //}
            //catch { }
            try
            {
                if (rdb_FromPerson.Checked)
                {
                    if (mlt_Person.Text.Trim() != "" && ((rdb_FromNumber.Checked && txt_FromNum.Text.Trim() != "" && txt_ToNumber.Text.Trim() != "") ||
                        (rdb_FromDate.Checked && fa_Date1.Text.Trim() != "" && fa_Date2.Text.Trim() != "" &&
                        fa_Date1.SelectedDateTime.HasValue && fa_Date2.SelectedDateTime.HasValue)))
                    {
                        if (rdb_FromNumber.Checked)
                            bindingSource1.DataSource = clDoc.ReturnTable(ConBank, "Select * from Table_035_ReceiptCheques where Column07=" + mlt_Person.Value.ToString() + " and Column00>=" + txt_FromNum.Text + " and Column00<=" + txt_ToNumber.Text);
                        else
                            bindingSource1.DataSource = clDoc.ReturnTable(ConBank, "Select * from Table_035_ReceiptCheques where Column07=" + mlt_Person.Value.ToString() + " and Column02>='" + fa_Date1.Text + "' and Column02<='" + fa_Date2.Text + "'");
                       
                        if (this.bindingSource1.Count > 0)
                        {

                            Int64 Price = Convert.ToInt64(bindingSource1.List.OfType<DataRowView>().Sum(row => Convert.ToDouble(row["Column05"].ToString())).ToString());

                            string Context = "گواهی می شود، تعداد   " + bindingSource1.Count + " فقره چک به مبلغ کل    " +
                                Class_BasicOperation.Rial(Price) + " ریال " + "به حروف    " + FarsiLibrary.Utils.ToWords.ToString(Price) +
                                " به شرح ذیل از   " + mlt_Person.Text + " دریافت گردید ";

                            DataTable Table = dataSet_Reports.Print_Rec_Chq.Clone();
                            foreach (DataRowView item in bindingSource1)
                            {
                                Table.Rows.Add(Context, item["Column03"].ToString(),
                                    clDoc.ExScalar(ConBank.ConnectionString, "Table_010_BankNames", "Column01", "Column00", item["Column08"].ToString()),
                                    item["Column10"].ToString(), item["Column02"].ToString(),
                                    item["Column04"].ToString(), item["Column06"].ToString(), item["Column05"].ToString(),
                                    1,"","","","");
                            }
                            //_4_Reports.Reports.Rptt12_Rec_Paper Rpt = new PACNT._4_Reports.Reports.Rptt12_Rec_Paper();

                            //DataTable Logo = Class_BasicOperation.LogoTable();
                            //if (!Chk_NoLogo.Checked)
                            //    Rpt.Subreports[0].SetDataSource(Logo);
                            //else Rpt.Subreports[0].SetDataSource(Logo.Clone());

                            //Rpt.SetDataSource(Table);
                            //Rpt.SetParameterValue("Param1", "تاریخ: " + " " + FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                            //Rpt.SetParameterValue("Param2", " ");
                            //Rpt.SetParameterValue("Param3", _Param3);
                            //Rpt.SetParameterValue("Param4", _Param4);
                            //Rpt.SetParameterValue("Param5", _Param5);
                            //Rpt.SetParameterValue("Param6", _Param6);
                            //Rpt.SetParameterValue("Param7", _Param7);
                            //Rpt.SetParameterValue("Param8", _Param8);
                            //Rpt.SetParameterValue("Param9", _Param9);
                            //Rpt.SetParameterValue("Param10", _Param10);
                            //crystalReportViewer1.ReportSource = Rpt;

                            StiReport stireport = new StiReport();
                            stireport.Load("Rptt12_Rec_Paper.mrt");
                            stireport.Pages["Page1"].Enabled = true;
                            stireport.Compile();
                            StiOptions.Viewer.AllowUseDragDrop = false;

                            stireport.RegData("Print_Rec_Chq", Table);
                           // 
                            DataTable Logo = Class_BasicOperation.LogoTable();
                            if (!Chk_NoLogo.Checked)
                                stireport.RegData("Table_000_OrgInfo", Logo);
                            else stireport.RegData("Table_000_OrgInfo", Logo.Clone());
                            stireport["Param1"] = "تاریخ: " + " " + FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
                            stireport["Param2"] = " ";
                            stireport["Param3"] = _Param3;
                            stireport["Param4"] = _Param4;
                            stireport["Param5"] = _Param5;
                            stireport["Param6"] = _Param6;
                            stireport["Param7"] = _Param7;
                            stireport["Param8"] = _Param8;
                            stireport["Param9"] = _Param9;
                            stireport["Param10"] = _Param10;

                            stireport["PUser3"] = "کاربر تایید";
                            stireport["PUser"] = "کاربر ثبت";
                            stireport.Render();
                            stiViewerControl1.Report = stireport;

                            stiViewerControl1.Visible = true;
                        }
                        else Class_BasicOperation.ShowMsg("", " در این بازه چکی از این شخص دریافت نشده است", Class_BasicOperation.MessageType.Warning);

                    }
                    else Class_BasicOperation.ShowMsg("", "اطلاعات مورد نیاز را تکمیل کنید", Class_BasicOperation.MessageType.Warning);

                }
                else if (rdb_This.Checked)
                {
                    string Command = @"SELECT     dbo.Table_035_ReceiptCheques.ColumnId AS PaperId, dbo.Table_035_ReceiptCheques.Column00 AS PaperNumber, 
                    dbo.Table_035_ReceiptCheques.Column02 AS RecDate, REPLACE(CONVERT(varchar, CAST(dbo.Table_035_ReceiptCheques.Column05 AS Money), 1), '.00', '') 
                    AS Price,dbo.Table_035_ReceiptCheques.Column05 as Price2, 'Riali' AS Riali, dbo.Table_035_ReceiptCheques.Column03 AS ChqNumber, dbo.Table_035_ReceiptCheques.Column04 AS EndDate, 
                    dbo.Table_010_BankNames.Column01 AS FromBank, dbo.Table_035_ReceiptCheques.Column06 AS Babat, PersonInfo.Column02 AS Person, 
                    dbo.Table_020_BankCashAccInfo.Column02 AS ToBank,
Table_035_ReceiptCheques.Column42 as User1,Table_035_ReceiptCheques.Column55 User2, 
dtUser1.Column39 AS Picsignatur,
           case when  Table_035_ReceiptCheques.Column54=1 then dtUser2.Column39 else NULL end AS PicOk ,
Table_035_ReceiptCheques.Column54 as OK

                    FROM         dbo.Table_035_ReceiptCheques INNER JOIN
                    dbo.Table_020_BankCashAccInfo ON dbo.Table_035_ReceiptCheques.Column01 = dbo.Table_020_BankCashAccInfo.ColumnId LEFT OUTER JOIN
                      PERP_MAIN.dbo.Table_010_UserInfo AS dtUser2 ON dbo.Table_035_ReceiptCheques.Column55 = dtUser2.Column00
and 
(dtUser2.Column05 =  " + Class_BasicOperation._OrgCode + @") AND 
                      (dtUser2.Column06 = N'" + Class_BasicOperation._FinYear + @"')
LEFT OUTER JOIN
                      PERP_MAIN.dbo.Table_010_UserInfo AS dtUser1 ON Table_035_ReceiptCheques.Column42= dtUser1.Column00
and (dtUser1.Column05 = " + Class_BasicOperation._OrgCode + @") AND (dtUser1.Column06 = N'" + Class_BasicOperation._FinYear + @"') LEFT OUTER JOIN
                    dbo.Table_010_BankNames ON dbo.Table_035_ReceiptCheques.Column08 = dbo.Table_010_BankNames.Column00 LEFT OUTER JOIN
                    (SELECT     ColumnId, Column01, Column02
                    FROM         {0}.dbo.Table_045_PersonInfo) AS PersonInfo ON dbo.Table_035_ReceiptCheques.Column07 = PersonInfo.ColumnId
                    WHERE     (dbo.Table_035_ReceiptCheques.ColumnId="+_Number+")";
                    Command = string.Format(Command, ConBase.Database);
                    DataTable Table = clDoc.ReturnTable(ConBank, Command);
                    foreach (DataRow item in Table.Rows)
                    {
                        item["Riali"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble(item["Price2"].ToString())));
                    }

                 
                    StiReport stireport = new StiReport();
                    stireport.Load("Rptt13_Rec_Paper_2.mrt");
                    stireport.Pages["Page1"].Enabled = true;
                    stireport.Compile();
                    StiOptions.Viewer.AllowUseDragDrop = false;

                    stireport.RegData("Rpt_PrintRecChqs", Table);
                    DataTable Logo = Class_BasicOperation.LogoTable();
                    if (!Chk_NoLogo.Checked)
                        stireport.RegData("Table_000_OrgInfo", Logo);
                    else stireport.RegData("Table_000_OrgInfo", Logo.Clone());

                    stireport["Param3"] = _Param3;
                    stireport["Param4"] = _Param4;
                    stireport["Param5"] = _Param5;
                    stireport["Param6"] = _Param6;
                    stireport["Param7"] = _Param7;
                    stireport["Param8"] = _Param8;
                    stireport["Param9"] = _Param9;
                    stireport["Param10"] = _Param10;

                    stireport["PUser3"] = "کاربر تایید";
                    stireport["PUser"] = "کاربر ثبت";
                    stireport.Render();
                    stiViewerControl1.Report = stireport;

                    stiViewerControl1.Visible = true;

                }
                

                else if (rdb_FromDate.Checked)
                {
                    if (fa_Date1.Text.Trim() != "" && fa_Date2.Text.Trim() != "" && fa_Date1.SelectedDateTime.HasValue && fa_Date2.SelectedDateTime.HasValue)
                    {

                        string Command = @"SELECT     dbo.Table_035_ReceiptCheques.ColumnId AS PaperId, dbo.Table_035_ReceiptCheques.Column00 AS PaperNumber, 
                    dbo.Table_035_ReceiptCheques.Column02 AS RecDate, REPLACE(CONVERT(varchar, CAST(dbo.Table_035_ReceiptCheques.Column05 AS Money), 1), '.00', '') 
                    AS Price,dbo.Table_035_ReceiptCheques.Column05 as Price2, 'Riali' AS Riali, dbo.Table_035_ReceiptCheques.Column03 AS ChqNumber, dbo.Table_035_ReceiptCheques.Column04 AS EndDate, 
                    dbo.Table_010_BankNames.Column01 AS FromBank, dbo.Table_035_ReceiptCheques.Column06 AS Babat, PersonInfo.Column02 AS Person, 
                    dbo.Table_020_BankCashAccInfo.Column02 AS ToBank,
Table_035_ReceiptCheques.Column42 as User1,Table_035_ReceiptCheques.Column55 User2, 
dtUser1.Column39 AS Picsignatur,
           case when  Table_035_ReceiptCheques.Column54=1 then dtUser2.Column39 else NULL end AS PicOk ,
Table_035_ReceiptCheques.Column54 as OK

                    FROM         dbo.Table_035_ReceiptCheques INNER JOIN
                    dbo.Table_020_BankCashAccInfo ON dbo.Table_035_ReceiptCheques.Column01 = dbo.Table_020_BankCashAccInfo.ColumnId LEFT OUTER JOIN
                      PERP_MAIN.dbo.Table_010_UserInfo AS dtUser2 ON dbo.Table_035_ReceiptCheques.Column55 = dtUser2.Column00
and 
(dtUser2.Column05 =  " + Class_BasicOperation._OrgCode + @") AND 
                      (dtUser2.Column06 = N'" + Class_BasicOperation._FinYear + @"')
LEFT OUTER JOIN
                      PERP_MAIN.dbo.Table_010_UserInfo AS dtUser1 ON Table_035_ReceiptCheques.Column42= dtUser1.Column00
and (dtUser1.Column05 = " + Class_BasicOperation._OrgCode + @") AND (dtUser1.Column06 = N'" + Class_BasicOperation._FinYear + @"') LEFT OUTER JOIN
                    dbo.Table_010_BankNames ON dbo.Table_035_ReceiptCheques.Column08 = dbo.Table_010_BankNames.Column00 LEFT OUTER JOIN
                    (SELECT     ColumnId, Column01, Column02
                    FROM         {0}.dbo.Table_045_PersonInfo) AS PersonInfo ON dbo.Table_035_ReceiptCheques.Column07 = PersonInfo.ColumnId
                    WHERE     (dbo.Table_035_ReceiptCheques.Column02 >= '{1}') and
                    (dbo.Table_035_ReceiptCheques.Column02 <= '{2}')";
                        Command = string.Format(Command, ConBase.Database, fa_Date1.Text, fa_Date2.Text);
                        DataTable Table = clDoc.ReturnTable(ConBank, Command);
                        foreach (DataRow item in Table.Rows)
                        {
                            item["Riali"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble(item["Price2"].ToString())));
                        }
                        //_4_Reports.Reports.Rptt13_Rec_Paper_3 Rpt = new _4_Reports.Reports.Rptt13_Rec_Paper_3();
                        
                        //DataTable Logo = Class_BasicOperation.LogoTable();
                        //if (!Chk_NoLogo.Checked)
                        //    Rpt.Subreports[0].SetDataSource(Logo);
                        //else Rpt.Subreports[0].SetDataSource(Logo.Clone());

                        //Rpt.SetDataSource(Table);
                        //Rpt.SetParameterValue("Param3", _Param3);
                        //Rpt.SetParameterValue("Param4", _Param4);
                        //Rpt.SetParameterValue("Param5", _Param5);
                        //Rpt.SetParameterValue("Param6", _Param6);
                        //Rpt.SetParameterValue("Param7", _Param7);
                        //Rpt.SetParameterValue("Param8", _Param8);
                        //Rpt.SetParameterValue("Param9", _Param9);
                        //Rpt.SetParameterValue("Param10", _Param10);
                        //crystalReportViewer1.ReportSource = Rpt;
                        StiReport stireport = new StiReport();
                        stireport.Load("Rptt13_Rec_Paper_3.mrt");
                        stireport.Pages["Page1"].Enabled = true;
                        stireport.Compile();
                        StiOptions.Viewer.AllowUseDragDrop = false;

                        stireport.RegData("Rpt_PrintRecChqs", Table);
                        DataTable Logo = Class_BasicOperation.LogoTable();
                        if (!Chk_NoLogo.Checked)
                            stireport.RegData("Table_000_OrgInfo", Logo);
                        else stireport.RegData("Table_000_OrgInfo", Logo.Clone());

                        stireport["Param3"] = _Param3;
                        stireport["Param4"] = _Param4;
                        stireport["Param5"] = _Param5;
                        stireport["Param6"] = _Param6;
                        stireport["Param7"] = _Param7;
                        stireport["Param8"] = _Param8;
                        stireport["Param9"] = _Param9;
                        stireport["Param10"] = _Param10;

                        stireport["PUser3"] = "کاربر تایید";
                        stireport["PUser"] = "کاربر ثبت";
                        stireport.Render();
                        stiViewerControl1.Report = stireport;

                        stiViewerControl1.Visible = true;
                    }
                    else Class_BasicOperation.ShowMsg("", "اطلاعات مورد نیاز را تکمیل کنید", Class_BasicOperation.MessageType.Warning);
                }
                else if (rdb_FromNumber.Checked)
                {
                    if (txt_FromNum.Text.Trim() != "" && txt_ToNumber.Text.Trim() != "")
                    {
                        string Command = @"SELECT     dbo.Table_035_ReceiptCheques.ColumnId AS PaperId, dbo.Table_035_ReceiptCheques.Column00 AS PaperNumber, 
                    dbo.Table_035_ReceiptCheques.Column02 AS RecDate, REPLACE(CONVERT(varchar, CAST(dbo.Table_035_ReceiptCheques.Column05 AS Money), 1), '.00', '') 
                    AS Price,dbo.Table_035_ReceiptCheques.Column05 as Price2, 'Riali' AS Riali, dbo.Table_035_ReceiptCheques.Column03 AS ChqNumber, dbo.Table_035_ReceiptCheques.Column04 AS EndDate, 
                    dbo.Table_010_BankNames.Column01 AS FromBank, dbo.Table_035_ReceiptCheques.Column06 AS Babat, PersonInfo.Column02 AS Person, 
                    dbo.Table_020_BankCashAccInfo.Column02 AS ToBank,
Table_035_ReceiptCheques.Column42 as User1,Table_035_ReceiptCheques.Column55 User2, 
dtUser1.Column39 AS Picsignatur,
           case when  Table_035_ReceiptCheques.Column54=1 then dtUser2.Column39 else NULL end AS PicOk ,
Table_035_ReceiptCheques.Column54 as OK

                    FROM         dbo.Table_035_ReceiptCheques INNER JOIN
                    dbo.Table_020_BankCashAccInfo ON dbo.Table_035_ReceiptCheques.Column01 = dbo.Table_020_BankCashAccInfo.ColumnId LEFT OUTER JOIN
                      PERP_MAIN.dbo.Table_010_UserInfo AS dtUser2 ON dbo.Table_035_ReceiptCheques.Column55 = dtUser2.Column00
and 
(dtUser2.Column05 =  " + Class_BasicOperation._OrgCode + @") AND 
                      (dtUser2.Column06 = N'" + Class_BasicOperation._FinYear + @"')
LEFT OUTER JOIN
                      PERP_MAIN.dbo.Table_010_UserInfo AS dtUser1 ON Table_035_ReceiptCheques.Column42= dtUser1.Column00
and (dtUser1.Column05 = " + Class_BasicOperation._OrgCode + @") AND (dtUser1.Column06 = N'" + Class_BasicOperation._FinYear + @"') LEFT OUTER JOIN
                   
                    dbo.Table_010_BankNames ON dbo.Table_035_ReceiptCheques.Column08 = dbo.Table_010_BankNames.Column00 LEFT OUTER JOIN
                    (SELECT     ColumnId, Column01, Column02
                    FROM         {0}.dbo.Table_045_PersonInfo) AS PersonInfo ON dbo.Table_035_ReceiptCheques.Column07 = PersonInfo.ColumnId
                    WHERE     (dbo.Table_035_ReceiptCheques.Column00 >={1}) and
                    (dbo.Table_035_ReceiptCheques.Column00 <= {2})";
                        Command = string.Format(Command, ConBase.Database, txt_FromNum.Text, txt_ToNumber.Text);
                        DataTable Table = clDoc.ReturnTable(ConBank, Command);
                        foreach (DataRow item in Table.Rows)
                        {
                            item["Riali"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Convert.ToDouble(item["Price2"].ToString())));
                        }
                        StiReport stireport = new StiReport();
                        stireport.Load("Rptt13_Rec_Paper_3.mrt");
                        stireport.Pages["Page1"].Enabled = true;
                        stireport.Compile();
                        StiOptions.Viewer.AllowUseDragDrop = false;

                        stireport.RegData("Rpt_PrintRecChqs", Table);
                        DataTable Logo = Class_BasicOperation.LogoTable();
                        if (!Chk_NoLogo.Checked)
                            stireport.RegData("Table_000_OrgInfo", Logo);
                        else stireport.RegData("Table_000_OrgInfo", Logo.Clone());

                        stireport["Param3"] = _Param3;
                        stireport["Param4"] = _Param4;
                        stireport["Param5"] = _Param5;
                        stireport["Param6"] = _Param6;
                        stireport["Param7"] = _Param7;
                        stireport["Param8"] = _Param8;
                        stireport["Param9"] = _Param9;
                        stireport["Param10"] = _Param10;

                        stireport["PUser3"] = "کاربر تایید";
                        stireport["PUser"] = "کاربر ثبت";
                        stireport.Render();
                        stiViewerControl1.Report = stireport;

                        stiViewerControl1.Visible = true;
                        //_4_Reports.Reports.Rptt13_Rec_Paper_3 Rpt = new _4_Reports.Reports.Rptt13_Rec_Paper_3();
                        
                        //DataTable Logo = Class_BasicOperation.LogoTable();
                        //if (!Chk_NoLogo.Checked)
                        //    Rpt.Subreports[0].SetDataSource(Logo);
                        //else Rpt.Subreports[0].SetDataSource(Logo.Clone());

                        //Rpt.SetDataSource(Table);
                        //Rpt.SetParameterValue("Param3", _Param3);
                        //Rpt.SetParameterValue("Param4", _Param4);
                        //Rpt.SetParameterValue("Param5", _Param5);
                        //Rpt.SetParameterValue("Param6", _Param6);
                        //Rpt.SetParameterValue("Param7", _Param7);
                        //Rpt.SetParameterValue("Param8", _Param8);
                        //Rpt.SetParameterValue("Param9", _Param9);
                        //Rpt.SetParameterValue("Param10", _Param10);
                        //crystalReportViewer1.ReportSource = Rpt;
                    }
                    else Class_BasicOperation.ShowMsg("", "اطلاعات مورد نیاز را تکمیل کنید", Class_BasicOperation.MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fa_Date1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void fa_Date1_TextChanged(object sender, EventArgs e)
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

        private void mlt_BesProject_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                mlt_Person.DroppedDown = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Form01_PrintAccDoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
                buttonX1_Click(sender, e);
        }

        private void rdb_FromPerson_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_FromPerson.Checked)
                rdb_FromDate.Checked = true;
        }

        private void rdb_This_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_This.Checked)
                rdb_FromPerson.Checked = false;
        }

        private void Form01_PrintRecChq_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Properties.Settings.Default.PrintWithoutLogo = Chk_NoLogo.Checked;
            //Properties.Settings.Default.Save();

            //try
            //{
            //    var Rpt = (CrystalDecisions.CrystalReports.Engine.ReportDocument)crystalReportViewer1.ReportSource;
            //    Rpt.Database.Dispose();
            //    Rpt.Close();
            //    Rpt.Dispose();
            //    GC.Collect();
            //}
            //catch { }
        }

    

        private void btnPrintOrdr_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rptt13_Rec_Paper_2.mrt");
            stireport.Design();

        }

        private void btnPrintWithOrdr_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rptt13_Rec_Paper_3.mrt");
            stireport.Design();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rptt12_Rec_Paper.mrt");
            stireport.Design();
        }

         
       

    

    }
}
