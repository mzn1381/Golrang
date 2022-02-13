﻿using Dapper;
using PCLOR.Classes;
using PCLOR.Models;
using Stimulsoft.Report.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR.Product
{
    public partial class Frm_015_Product : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SerialPort comport = new SerialPort();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        int ResidNum = 0;
        private bool GoStepAfter = false;
        private int DeviceId = 0;
        public int ShiftNow { get; set; }
        //private string DeviceName;
        private int clothType = 0;
        private Int16 TextureLimit { get; set; }
        private int cottonType = 0;
        private Int16 WareCode = 0;
        private Int16 FunctionType = 0;
        private bool IsCreateAutomatic = false;
        private bool IsInfinitiveTextureLimit = false;
        private DateTime TimeLastProduct { get; set; }
        bool Machine = false;
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
        public Frm_015_Product()
        {
            InitializeComponent();
        }
        public Frm_015_Product(int Id)
        {

            InitializeComponent();
            DeviceId = Id;
            Machine = true;
        }

        private void Frm_015_Product_Load(object sender, EventArgs e)
        {
            //IsJoinShift(DeviceId);
            // TODO: This line of code loads data into the 'pCLOR_1_1400DataSet.Table_115_Product' table. You can move, or remove it, as needed.
            txt_DateTime.Text = DateTime.Now.ToShamsi();
            this.table_115_ProductTableAdapter1.Fill(this.pCLOR_1_1400DataSet.Table_115_Product);
            gridEX2.MoveLast();
            gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
            gridEX2.DropDowns["Customer"].DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,Column02 from Table_045_PersonInfo");
            FillDetailMachine();
            //SetDeviceName();
            if (IsInfinitiveTextureLimit)
                lblTextureLimit.Text = "نا محدود";
            //gridEX2.DropDowns["Programer"].DataSource = mlt_Num_Programer.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Number from Table_100_ProgramMachine");
            //gridEX2.DropDowns["shift"].DataSource = mlt_shift.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,Shift from Table_105_DefinitionWorkShift");
            //gridEX2.DropDowns["Machine"].DataSource = mlt_Machine.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,namemachine from Table_60_SpecsTechnical");
            //gridEX2.DropDowns["TypeCloth"].DataSource = mlt_TypeCloth.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select ID,TypeCloth,CodeCommondity from Table_005_TypeCloth");
            //gridEX2.DropDowns["cottone"].DataSource = ClDoc.ReturnTable(ConPCLOR, @"select Id,code,NameCotton from Table_120_TypeCotton");
            //mlt_codecustomer.DataSource = ClDoc.ReturnTable(ConBase, @"select Columnid,column01,Column02  from Table_045_PersonInfo");
            //mlt_Ware.DataSource = ClDoc.ReturnTable(ConWare, @"select Columnid,Column01,Column02 from Table_001_PWHRS");
            //mlt_Function.DataSource = ClDoc.ReturnTable(ConWare, @"select Columnid,Column01,Column02 from table_005_PwhrsOperation where Column16=0");
            SetStatusForCreateProduct();
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Report1.mrt");
            foreach (StiPage page in r.Pages)
            {
                uiComboBox1.Items.Add(page.Name);
            }
            WareCode = Convert.ToInt16(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=31"));
            FunctionType = Convert.ToInt16(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=30"));
            uiComboBox1.Text = uiComboBox1.Items[0].Text.Trim();
            //mlt_Num_Programer.Focus();
        }



        private void btn_New_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 146))
            {
                if (TextureLimit == 0 && IsInfinitiveTextureLimit == false)
                {
                    //MessageBox.Show(" حد بافت دستگاه به صفر رسیده است !!", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    lblWarningTextureLimit.Visible = true;
                    //this.Close();
                    //return;
                }


                //if (mlt_Num_Programer.Text == "" || mlt_Num_Programer.Text == "0")
                //{
                //    Class_BasicOperation.ShowMsg("", "لطفا شماره دستگاه مورد نظر را وارد نمایید", Class_BasicOperation.MessageType.None);

                //}
                //if (mlt_shift.Value == null || mlt_shift.Text == "")
                //{
                //    Class_BasicOperation.ShowMsg("", "لطفا شیفت کاری مورد نظر را وارد نمایید", Class_BasicOperation.MessageType.None);

                //}
                if (string.IsNullOrEmpty(txt_weight.Text) || txt_weight.Text.Trim() == "0")
                {
                    Class_BasicOperation.ShowMsg("", "لطفا وزن مورد نظر را وارد نمایید", Class_BasicOperation.MessageType.None);
                    return;
                }
                if (txt_weight.Text.Trim().Length > 6)
                {
                    Class_BasicOperation.ShowMsg("", "وزن وارد شده صحیح نمی باشد", Class_BasicOperation.MessageType.None);
                    return;
                }
                table_115_ProductBindingSource.AddNew();
                //mlt_Machine.Value = _Id;
                chek_TowPerson.Checked = false;
                var Barcode = "B" + Convert.ToInt64(ClDoc.ExScalar(ConPCLOR.ConnectionString, "select isnull((select max(ID) from Table_115_Product),1)+10000")) + "P";
                //txt_Barcode.Text = Barcode.ToString();
                double weigh = Convert.ToDouble(txt_weight.Text) / 1000;

                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Barcode"] = Barcode.ToString();
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["weight"] = weigh;

                //((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["ProgramerMachine"] = DeviceId;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Machine"] = DeviceId;

                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["ClothType"] = clothType;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["CottonType"] = cottonType;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["ReportDescriptin"] = txt_Description.Text;
                if (lblShiftOperator.Text.Trim().ToLower() == "شب")
                {
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["shift"] = 3;
                    ShiftNow = 3;
                }
                else if (lblShiftOperator.Text.Trim().ToLower() == "صبح")
                {
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["shift"] = 2;
                    ShiftNow = 2;
                }
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Date"] = lblDateCreate.Text;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Time"] = lblCreateTime.Text;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["UserSabt"] = Class_BasicOperation._UserName;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["DateSabt"] = DateTime.Now;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["UserEdite"] = Class_BasicOperation._UserName;
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["DateEdite"] = Class_BasicOperation.ServerDate().ToString();
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Operator"] = txtCodeTag.Text.Trim().ToString();
                //if (((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["ID"].ToString().StartsWith("-"))
                //{
                //    txt_Number.Text = ClDoc.MaxNumber(Properties.Settings.Default.PCLOR, "Table_115_Product", "Number").ToString();

                //    ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"update Table_115_Product set Number=" + txt_Number.Text + " where Id=" + txt_Id.Text);
                //}
                //table_115_ProductBindingSource.EndEdit();
                //table_115_ProductTableAdapter1.Update(pCLOR_1_1400DataSet.Table_115_Product);
                //if (!IsInfinitiveTextureLimit)
                //    DecreaseTextureLimit(DeviceId);
                //table_115_ProductTableAdapter.FillByProgramerMachine(dataSet_05_Product.Table_115_Product,Convert.ToInt32 (mlt_Num_Programer.Value));
                gridEX2.MoveLast();
                txt_weight.Text = "0";
                txt_Description.Text = string.Empty;
                txt_weight.Focus();
                ch_Auto.Enabled = true;
                GoStepAfter = true;
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void mlt_Num_Programer_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Num_Programer_ValueChanged(object sender, EventArgs e)

        {
            try
            {
                //if (mlt_Num_Programer.Value.ToString() != "")
                //{

                //    DataTable dt = new DataTable();

                //    dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT        Machine, Cloth,d.IDCotton,Weave,Printer
                //        FROM            dbo.Table_100_ProgramMachine h INNER JOIN Table_125_DetailTypeCotton d on  d.fk=h.id where h.Id=" + mlt_Num_Programer.Value + "");

                //    if (dt.Rows.Count > 0)
                //    {
                //        mlt_Machine.Value = dt.Rows[0]["Machine"];
                //        mlt_TypeCloth.Value = dt.Rows[0]["Cloth"];
                //        txt_NumberWeave.Text = dt.Rows[0]["Weave"].ToString();
                //        txt_Dat.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("YYYY/MM/DD");
                //        uiComboBox1.Text = dt.Rows[0]["Printer"].ToString();
                //        txt_Time.Text = DateTime.Now.ToString("HH:mm");
                //        if (Properties.Settings.Default.Print != "")
                //        {
                //            uiComboBox1.Text = Properties.Settings.Default.Print;

                //        }
                //    }

                //    table_115_ProductTableAdapter.FillByProgramerMachine(dataSet_05_Product.Table_115_Product, Convert.ToInt32(mlt_Num_Programer.Value));
                //}
            }
            catch
            {


            }


        }

        public string StatusShift()
        {
            ///یراساس شیفت صبح ,  شیفت تعیین می شود
            SetStatusShiftViewModel model;
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                               select TimeStart,TimeEnd
                               from Table_105_DefinitionWorkShift
                               where ID=2";
                model = db.QueryFirstOrDefault<SetStatusShiftViewModel>(query, null, commandType: CommandType.Text);
            }
            //var statusNow = DateTime.Now.ToString("tt");
            var hourNow = DateTime.Now.Hour;
            //if (statusNow.ToLower().Trim() == "pm")
            //    hourNow += 12;
            if (model.TimeStart.Hours <= hourNow && hourNow <= model.TimeEnd.Hours)

                return "صبح";
            return "شب ";
        }

        public Machine GetMachine(int ID)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"select m.*,c.NameCotton as YarnTypeName,t.TypeCloth as FabricTypeName 
                                from Table_60_SpecsTechnical as m
                                left join Table_120_TypeCotton as c
                                on m.YarnType = c.ID
                                left join Table_005_TypeCloth as t
                                on m.FabricType = t.ID
                                where m.ID ={ID}";
                var machine = db.QueryFirstOrDefault<Machine>(query, null, commandType: CommandType.Text);
                return machine;
            }
        }

        public void FillDetailMachine()
        {
            var machine = GetMachine(DeviceId);
            if (machine.FabricType == 0 || machine.YarnType == 0 || string.IsNullOrEmpty(machine.NameMachine))
            {
                MessageBox.Show("دستگاه انتخاب فاقد حداقل مشخصات لازم برای ثبت تولید است !! لطفا ابتدا مشخصات دستگاه را در منوی انتخاب دستگاه ثبت کنید ");
                Close();
                return;
            }
            clothType = (int)machine.FabricType;
            cottonType = machine.YarnType;
            lblDateCreate.Text = DateTime.Now.ToShamsi();
            lblGapDevice.Text = machine.Gap.ToString();
            lblNameDevice.Text = machine.NameMachine;
            lblRoundStop.Text = Convert.ToInt32(machine.RoundStop).ToString();
            lblShiftOperator.Text = StatusShift();
            lblYarnType.Text = machine.YarnTypeName;
            lblTeeny.Text = Convert.ToInt32(machine.teeny).ToString();
            lblTextureLimit.Text = machine.TextureLimit.ToString();
            TextureLimit = (short)machine.TextureLimit;
            lblTypeDevice.Text = machine.DeviceMark;
            lblTypeFabric.Text = machine.FabricTypeName;
            var hourNow = DateTime.Now.Hour;

            //if (statusOfDate.ToLower().Trim() == "pm")
            //    hourNow += 12;
            //if (hourNow >= 24)
            //    hourNow = 0;
            lblCreateTime.Text = hourNow + ":" + DateTime.Now.TimeOfDay.Minutes.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            lblArea.Text = machine.Area.ToString();
            txtDescDevice.Text = machine.Description;
            IsInfinitiveTextureLimit = machine.IsInfinitiveTextureLimit;
            if (TextureLimit == 0 && !IsInfinitiveTextureLimit)
                lblWarningTextureLimit.Visible = true;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (table_115_ProductBindingSource.Count > 0)
            {

                if (string.IsNullOrEmpty(((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString()) == false)
                {
                    Class_BasicOperation.ShowMsg("", "برای این کارت تولید رسید ثبت شده است ابتدا رسید آن را حذف کنید", Class_BasicOperation.MessageType.Stop);
                    return;
                }
                //if (mlt_Function.Text.Trim() == "0" || mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim() == "0"
                //    //|| mlt_Ware.Text.Trim() == "" || mlt_shift.Text.Trim() == ""
                //    //|| mlt_codecustomer.Text == "" || mlt_Function.Text.All(char.IsDigit) || mlt_Ware.Text.All(char.IsDigit)
                //    )
                //{
                //    MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                //    return;
                //}
                if (uiComboBox1.Text == "")
                {
                    MessageBox.Show("چاپ موردنظر را انتخاب کنید ");
                    return;
                }

                //int Position = gridEX2.CurrentRow.RowIndex;




                //string lastdate = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((select top(1) Time from Table_115_Product where Machine=" + mlt_Machine.Value + "  order by Date,DateSabt Desc),'0:0')");
                //string LastShift = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((SELECT        TOP (1)  dbo.Table_105_DefinitionWorkShift.TimeEnd
                //                 FROM            dbo.Table_105_DefinitionWorkShift LEFT OUTER JOIN
                //                 dbo.Table_115_Product ON dbo.Table_105_DefinitionWorkShift.ID = dbo.Table_115_Product.shift
                //                WHERE        (dbo.Table_115_Product.Machine = " + mlt_Machine.Value + @")
                //                ORDER BY dbo.Table_115_Product.Date DESC),'0:0')");
                //txt_Lastdate.Text = lastdate;
                //txt_Lastshift.Text = LastShift;
                //if (lastdate != "00:00:00" && LastShift != "00:00:00")
                //{
                //    if (Convert.ToDateTime(txt_Time.Text) < Convert.ToDateTime(txt_Lastshift.Text) && Convert.ToDateTime(txt_Lastdate.Text) < Convert.ToDateTime(txt_Lastshift.Text))
                //    {
                //        chek_TowPerson.Checked = true;
                //    }
                //    else
                //    {
                //        chek_TowPerson.Checked = false;

                //    }
                //}
                var code = txtCodeTag.Text;
                if (string.IsNullOrEmpty(code))
                {
                    code = "-1";
                }
                string idrfid = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"
                                                        select isnull( (select  Id  from  Table_135_RFIDPerson  where CodeRFID = " + code + ") , 0 ) ");

                //table_115_ProductBindingSource.EndEdit();
                //table_115_ProductTableAdapter1.Update(pCLOR_1_1400DataSet.Table_115_Product);
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["RFID"] = Convert.ToInt32(idrfid);
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Printer"] = uiComboBox1.Text;
                var isJoinShift = IsJoinShift(DeviceId);
                ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["IsJoinShift"] = isJoinShift;

                if (isJoinShift == 1)
                {
                    var model = FillIsJoinShift(DeviceId);
                    var p1 = Convert.ToDecimal(model["Percent1"]);
                    var p2 = Convert.ToDecimal(model["Percent2"]);
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Operator2"] = model["Operator2"];
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["PersonTexture"] = p1;
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["PersonTexture2"] = p2;
                }
                else
                {
                    //((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["Operator2"] = 
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["PersonTexture"] = 100.00;
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["PersonTexture2"] = 0.00;
                }

                Recipt();
                //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, $@"
                //     update Table_115_Product set  Operator =N'{txtCodeTag.Text.Trim()}'
                //     where id = {ID}");
                ////+
                //",TimeLastShift='" + txt_Lastshift.Text + 
                //",TimeLastProduct='" + txt_Lastdate.Text + 
                //" Update Table_100_ProgramMachine set Printer=N'" + uiComboBox1.Text + "' where ID=" + mlt_Num_Programer.Value);
                Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت دخیره شد" + Environment.NewLine + "رسید به شماره" + ResidNum + "با موفقیت صدور شد", Class_BasicOperation.MessageType.Information);
                //gridEX2.DropDowns["Recipt"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_011_PwhrsReceipt ");
                //table_115_ProductTableAdapter.FillByProgramerMachine(dataSet_05_Product.Table_115_Product, Convert.ToInt32(DeviceId));
                //gridEX2.MoveTo(Position);
                if (!IsInfinitiveTextureLimit)
                {
                    TextureLimit -= 1;
                    lblTextureLimit.Text = TextureLimit.ToString();
                    if (TextureLimit == 0)
                        lblWarningTextureLimit.Visible = true;
                }
                gridEX2.MoveLast();
                //this.Close();
            }
        }
        //string ID = "";
        private void Recipt()
        {
            using (IDbConnection db = new SqlConnection(ConWare.ConnectionString))
            {
                try
                {
                    var queryGetcommodity = $@"  SELECT c.CodeCommondity
                                                 from PCLOR_1_1400.dbo.Table_60_SpecsTechnical as s inner join 
                                                 PCLOR_1_1400.dbo.Table_005_TypeCloth as c on s.FabricType = c.ID
                                                 where s.ID={DeviceId}";
                    var codeCommodity = db.QueryFirstOrDefault<int>(queryGetcommodity, null
                        , commandType: CommandType.Text);
                    ResidNum = ClDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                    string commandtxt = string.Empty;
                    //commandtxt = @"Declare   @Key   int";
                    commandtxt += $@" INSERT INTO Table_011_PwhrsReceipt (
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
                                                                 
                                                                          ) VALUES (  {ResidNum} , N'{txt_DateTime.Text}'  , {WareCode},  {FunctionType} ,
                                                                        {(string.IsNullOrEmpty(lblOperationCode.Text) ? "N''" : lblOperationCode.Text)},N'رسید صادره بابت رسید پارچه خام شماره {txt_Number.Text}' , N'{Class_BasicOperation._UserName}' ,getdate(), N'{Class_BasicOperation._UserName}', getdate() );
                                                                       select  Max(columnid)  from Table_011_PwhrsReceipt";
                    var Key = db.QueryFirstOrDefault<int>(commandtxt, null, commandType: CommandType.Text);
                    string query = "";
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["NumberRecipt"] = Key;
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["CodeStore"] = WareCode;
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["IsRegToOrderColor"] = 0;
                    if (checkIsLineCutting.Checked)
                        ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["IsLineCutting"] = 1;
                    else
                        ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["IsLineCutting"] = 0;
                    table_115_ProductBindingSource.EndEdit();
                    var stauts = table_115_ProductTableAdapter1.Update(pCLOR_1_1400DataSet.Table_115_Product);
                    if (stauts <= 0)
                    {
                        MessageBox.Show("متاسفانه ثبت تولید با شکست مواجه شد ! لطفا دوباره امتحان کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DeleteLastRecid(Key);
                        return;
                    }
                    var currenAddProduct = (DataRowView)table_115_ProductBindingSource.Current;
                    //foreach (DataRowView Rows in table_115_ProductBindingSource)
                    //{
                    //if (string.IsNullOrEmpty(Rows["NumberRecipt"].ToString()) || Rows["NumberRecipt"].ToString() == "0")
                    //{
                    //ID = ID + Rows["ID"] + ",";
                    //ID = currenAddProduct["ID"].ToString();
                    query = $@" INSERT INTO Table_012_Child_PwhrsReceipt (
                                    [column01]
                                   ,[column02]
                                   ,[column03]
                                   ,[column06]
                                   ,[column07]
                                   ,[column10]
                                   ,[column11]
                                   ,[column15]
                                   ,[column16]
                                   ,[column17]
                                   ,[column18]
                                   ,[column20]
                                   ,[column21]
                                   ,[column30]
                                   ,[Column34]
                                   ,[Column35]
                                   ,[Column37]
                           ) VALUES (
               {Key},{codeCommodity},1,1,1,0,0,N'{Class_BasicOperation._UserName}',getdate(),N'{Class_BasicOperation._UserName}' ,getdate(),0,0, N'{currenAddProduct["Barcode"].ToString()}' , {currenAddProduct["Weight"]} , {currenAddProduct["Weight"]} , N'{currenAddProduct["Machine"] }' );";
                    db.Execute(query, null, commandType: CommandType.Text);
                    //}
                    //}
                    //var t = ID.TrimEnd(',');
                    //var queryFinall = $"update  Table_115_Product  set  NumberRecipt={Key}  where  ID  = {t}";
                    //db.ConnectionString = ConPCLOR.ConnectionString;
                    //db.Execute(queryFinall, null, commandType: CommandType.Text);
                    //Class_BasicOperation.SqlTransactionMethod(ConWare.ConnectionString, commandtxt);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Frm_015_Product_Load(sender, e);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 147))
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["NumberRecipt"].ToString() != "0")
                    {
                        Class_BasicOperation.ShowMsg("", "برای این کارت تولید رسید ثبت شده است ابتدا رسید آن را حذف کنید", Class_BasicOperation.MessageType.Stop);
                        return;
                    }
                    DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"select Barcode from Table_030_DetailOrderColor where Barcode=" + gridEX2.GetValue("barcode") + "");
                    if (dt.Rows.Count > 0)
                    {
                        Class_BasicOperation.ShowMsg("", "به دلیل استفاده از این کارت تولید در سفارش رنگ امکان حذف آن را ندارید", Class_BasicOperation.MessageType.Stop);
                        return;
                    }

                    table_115_ProductBindingSource.RemoveCurrent();
                    table_115_ProductBindingSource.EndEdit();
                    table_115_ProductTableAdapter.Update(dataSet_05_Product.Table_115_Product);
                    Class_BasicOperation.ShowMsg("", "اطلاعات با موفقیت حذف شد", Class_BasicOperation.MessageType.Information);
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void chek_TowPerson_CheckedChanged(object sender, EventArgs e)
        {
            if (chek_TowPerson.Checked == false)
            {
                chek_TowPerson.Checked = false;
            }
            else if (chek_TowPerson.Checked == true)
            {
                chek_TowPerson.Checked = true;

            }
        }

        private void Delet_Recipt_Click(object sender, EventArgs e)
        {


        }

        private void Frm_015_Product_KeyDown(object sender, KeyEventArgs e)
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

        private void btn_delete_recipt_Click(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 144))
            {
                if (MessageBox.Show("آیا از حذف اطلاعات جاری مطمئن هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (ClDoc.OperationalColumnValue("Table_011_PwhrsReceipt", "Column07", gridEX2.GetValue("NumberRecipt").ToString()) != 0)
                    {
                        throw new Exception(" رسید این کارت تولید دارای سند حسابداری می باشد ابتدا سند آن را حذف نمایید");
                    }

                    table_115_ProductBindingSource.EndEdit();

                    int Position = gridEX2.CurrentRow.RowIndex;

                    string CommandTexxt = @"Delete from " + ConWare.Database + @".dbo. Table_012_Child_PwhrsReceipt where Column01 in(
                                                    select NumberRecipt from Table_115_Product where Number = " + txt_Number.Text + @")

                                                    Delete from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt where columnid in(
                                                     select NumberRecipt from Table_115_Product where Number = " + txt_Number.Text + @")

                                                    Update Table_115_Product set NumberRecipt=0  where Number = " + txt_Number.Text + "";

                    Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.PCLOR, CommandTexxt);


                    dataSet_05_Product.EnforceConstraints = false;
                    this.table_115_ProductTableAdapter.Fill(this.dataSet_05_Product.Table_115_Product);

                    dataSet_05_Product.EnforceConstraints = true;
                    MessageBox.Show("رسید با موفقیت حذف گردید");
                    gridEX2.RemoveFilters();
                    gridEX2.MoveTo(Position);
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);
            }
        }

        private void mlt_Num_Programer_KeyPress_1(object sender, KeyPressEventArgs e)
        {

            //if (e.KeyChar == 13)
            //mlt_shift.Focus();

        }

        private void mlt_shift_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Shift", "ID");
        }

        private void txt_weight_ValueChanged(object sender, EventArgs e)
        {
            //if (txt_weight.Text!="0" || txt_weight.Text!="")
            //{
            //    txt_RFID.Focus();
            //}
        }

        private void txt_weight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtCodeTag.Focus();
                //btn_New_Click(sender, e);
                if (ch_Auto.Checked)
                {
                    Print1(dataSet_05_Product.Table_115_Product.Compute("Max(ID)", "").ToString(), uiComboBox1.Text);

                }
            }
        }

        private void Print1(string v, object text)
        {
            throw new NotImplementedException();
        }

        private void txt_RFID_TextChanged(object sender, EventArgs e)
        {
            //if (txt_RFID.Text != "" || txt_RFID.Text != "0")
            //{
            //    string v = ClDoc.ExScalar(ConPCLOR.ConnectionString, @"select isnull((select Person from Table_135_RFIDPerson where CodeRFID =" + txt_RFID.Text + "),0)");
            //    if (v != "0")
            //    {
            //        mlt_codecustomer.Value = int.Parse(v);
            //        btn_Save_Click(sender, e);

            //    }
            //    else
            //    {
            //        Class_BasicOperation.ShowMsg("", "برای این کارت شخصی تعریف نشده است لطفا برای آن شخصی را معرفی کنید", Class_BasicOperation.MessageType.Warning);
            //        return;
            //    }

            //}
        }

        private void mlt_shift_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
                txt_Description.Focus();

        }

        private void txt_Description_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
                txt_weight.Focus();

        }

        private void txt_RFID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (/*txt_RFID.Text != "" || txt_RFID.Text != "0"*/true)
                {
                    //string v = ClDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select ColumnId from Table_045_PersonInfo where Column148 =" + txt_RFID.Text + "),0)");
                    //if (v != "0")
                    //{
                    //    //mlt_codecustomer.Value = int.Parse(v);
                    //    btn_Save_Click(sender, e);

                    //}
                    //else
                    //{
                    //    Class_BasicOperation.ShowMsg("", "برای این کارت شخصی تعریف نشده است لطفا برای آن شخصی را معرفی کنید", Class_BasicOperation.MessageType.Warning);
                    //    return;
                    //}

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void uiGroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void rb_select_CheckedChanged(object sender, EventArgs e)
        {
            txt_weight.Text = "0";
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

        private void txt_NumberWeave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //mlt_shift.Focus();
            }
        }

        private void rb_Auto_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (!rb_Auto.Checked)
                {
                    comport.Close();
                }
                else
                {
                    OpenPort();
                    //comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                }
            }

        }

        private void rb_select_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void ch_Auto_CheckedChanged(object sender, EventArgs e)
        {
            //            if (ch_Auto.Checked == true)
            //            {

            //                Print1(dataSet_05_Product.Table_115_Product.Compute("Max(ID)", "").ToString(), uiComboBox1.Text);

            //            }
            //            DataTable dt = ClDoc.ReturnTable(ConPCLOR, @"SELECT        dbo.Table_100_ProgramMachine.Number AS numdivice, dbo.Table_115_Product.ID, dbo.Table_115_Product.Barcode
            //FROM            dbo.Table_115_Product INNER JOIN
            //                         dbo.Table_100_ProgramMachine ON dbo.Table_115_Product.ProgramerMachine = dbo.Table_100_ProgramMachine.ID");

        }

        private void bindingNavigator1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txt_weight_Click(object sender, EventArgs e)
        {

        }

        private void uiPanel0_Click(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtCodeTag_TextChanged(object sender, EventArgs e)
        {





        }

        private void txtCodeTag_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

                var text = ((TextBox)sender).Text.Trim();
                using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
                {
                    try
                    {
                        var query = $@"
              select p.Column01 as Code,p.Column04+' '+ p.Column05 as Name
              from PCLOR_1_1400.dbo.Table_135_RFIDPerson as r
              inner join PBASE_1.dbo.Table_045_PersonInfo as p
              on p.ColumnId = r.Person
              where r.CodeRFID =N'{text}'";
                        var model = db.QueryFirstOrDefault<PersonInfoViewModel>(query, null, commandType: CommandType.Text);
                        if (model is null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Name))
                        {
                            MessageBox.Show("برای کارت وارد شده کاربری ثبت نشده است لطفا ابتدا کاربر مورد نظر برای کارت را وارد کنید");
                            txtCodeTag.Text = string.Empty;
                            txtCodeTag.Focus();
                            return;
                        }
                        lblOperationCode.Text = model.Code;
                        lblOperatorName.Text = model.Name;
                        btn_New_Click(sender, e);
                        if (GoStepAfter)
                        {
                            btn_Save_Click(sender, e);
                            txtCodeTag.Text = string.Empty;
                            if (!IsCreateAutomatic)
                            {
                                DecreaseTextureLimit(DeviceId, TextureLimit);
                                this.Close();
                            }
                        }
                        txtCodeTag.Text = string.Empty;

                    }
                    catch (Exception es)
                    {
                        MessageBox.Show(es.Message);
                        txtCodeTag.Text = string.Empty;

                    }

                }
            }

        }

        public void DecreaseTextureLimit(int DeviceId, short TextureLimit)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                if (TextureLimit < 0)
                    TextureLimit = 0;
                try
                {
                    var query = $@"
                                  update Table_60_SpecsTechnical Set
                                  TextureLimit = {TextureLimit}
                                  where ID = {DeviceId}";

                    db.Execute(query, null, commandType: CommandType.Text);
                    //lblTextureLimit.Text = (Convert.ToInt64(lblTextureLimit.Text) - 1).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        public void SetStatusForCreateProduct()
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = @"select value
                              from Table_80_Setting 
                              where Id = 34";
                var res = db.QueryFirstOrDefault<int>(query, null, commandType: CommandType.Text);
                if (res == 1)
                    IsCreateAutomatic = true;
            }
        }

        //public void SetDeviceName()
        //{
        //    using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
        //    {
        //        var query = $@"
        //                        select namemachine
        //                        from Table_60_SpecsTechnical
        //                        where ID = {DeviceId}                               
        //                     ";
        //        DeviceName = db.QueryFirstOrDefault<string>(query, null, commandType: CommandType.Text);
        //    }
        //}

        public void DeleteLastRecid(int Id)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {

                var query = $@"
                               delete from Table_011_PwhrsReceipt 
                               where columnid={Id}";
                db.Execute(query, null, commandType: CommandType.Text);
            }
        }

        private DateTime GetTimeLastProduct(int deviceId)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                var query = $@"
                select Top(1)
                DateSabt as Date  
                from Table_115_Product
                where Machine ={deviceId}
                order By DateSabt Desc ;";
                return db.QueryFirstOrDefault<DateTime>(query, null, commandType: CommandType.Text);
            }
        }

        public int IsJoinShift(int DeviceId)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                int res = 0;
                var query = $@"
                    select Top(1) shift
                    from Table_115_Product
                    Order by DateSabt desc
                            ";
                try
                {
                    res = db.QueryFirstOrDefault<int>(query, null, commandType: CommandType.Text);

                }
                catch (Exception)
                {
                    res = 0;
                }
                if (res == 0 || res == ShiftNow)
                    return 0;
                return 1;
                ///////////////////---------------------------------                
                //                string lastProductShift = "صبح";
                //                string shiftNow = "صبح";
                //                var query = $@"
                //                    select Top(1)
                //                    Time as Date  
                //                    from Table_115_Product
                //                    where Machine ={DeviceId}
                //                    order By DateSabt Desc ;
                //                    select TimeEnd
                //                    from Table_105_DefinitionWorkShift
                //                    where ID=2";
                //                var date = db.QueryMultiple(query, null, commandType: CommandType.Text);
                //                var lastProducttt = date.ReadFirstOrDefault(typeof(TimeSpan));
                //                if (lastProducttt == null)
                //                    return 0;
                //                TimeSpan lastProduct = (TimeSpan)lastProducttt;
                //TimeLastProduct = lastProduct;
                //                //var gg = lastProduct.ToString("tt");
                //                var endShiftttt = date.ReadFirstOrDefault(typeof(TimeSpan));
                //                var endShift = ((TimeSpan)endShiftttt).Hours;
                //                if (lastProduct.Hours > endShift)
                //                    lastProductShift = "شب";
                //                var statusHourDateNow = DateTime.Now.ToString("tt");
                //                var hourNow = DateTime.Now.Hour;
                //                if (statusHourDateNow.ToLower() == "pm")
                //                    hourNow += 12;
                //                if (hourNow > endShift)
                //                    lastProductShift = "شب";
                //                if (lastProductShift.Trim() == shiftNow.Trim())
                //                    return 1;
                //                return 0;
            }

        }

        private Dictionary<string, string> FillIsJoinShift(int deviceId)
        {

            var timeStartShiftNow = GetStartTimeShiftByShiftName(lblShiftOperator.Text.Trim());
            //var statusTimeNow = DateTime.Now.ToString("tt");
            TimeLastProduct = GetTimeLastProduct(deviceId);
            //var t = ;
            var t = DateTime.Now;
            var totalTimeOfProductSeconds = (t - TimeLastProduct).TotalSeconds;
            //var hourNow = DateTime.Now.TimeOfDay.TotalSeconds;
            //if (statusTimeNow.Trim().ToLower() == "pm")
            //    hourNow += 12;
            var totalTimeForOperator1 = timeStartShiftNow.TotalSeconds - TimeLastProduct.TimeOfDay.TotalSeconds;
            var totalTimeForOperator2 = totalTimeOfProductSeconds - totalTimeForOperator1;
            //var hh2 = timeStartShiftNow.Subtract(h.TimeOfDay);
            //var hh = timeStartShiftNow - h.TimeOfDay;
            //var totalTimeOfProduct = hourNow - TimeLastProduct.Hours;
            var percent1 = ((decimal)(totalTimeForOperator1) / (decimal)totalTimeOfProductSeconds) * 100;
            var percent2 = ((decimal)(totalTimeForOperator2) / (decimal)totalTimeOfProductSeconds) * 100;
            percent1 = Decimal.Round(percent1);
            percent2 = 100 - percent1;
            var operatorCode2 = GetCodeOperatorofLastproductForDevice(DeviceId);
            var res = new Dictionary<string, string>();
            res.Add("Operator2", operatorCode2);
            res.Add("Percent1", (percent1).ToString());
            res.Add("Percent2", percent2.ToString());
            return res;
        }

        private string GetCodeOperatorofLastproductForDevice(int deviceId)

        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {

                var query = $@"
                    select Top(1)
                    Operator as OperatorCode
                    from Table_115_Product
                    where Machine ={deviceId}
                    order By DateSabt Desc ;";
                return db.QueryFirstOrDefault<string>(query, null, commandType: CommandType.Text);

            }


        }

        private TimeSpan GetStartTimeShiftByShiftName(string shiftName)
        {
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {

                var query = $@"
                            SELECT 
                            [TimeStart]
                            FROM [PCLOR_1_1400].[dbo].[Table_105_DefinitionWorkShift]
                            where   RTrim(LTRIM([Shift])) = N'{shiftName.Trim()}'";
                return db.QueryFirstOrDefault<TimeSpan>(query, null, commandType: CommandType.Text);
            }
        }

        private void Frm_015_Product_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsInfinitiveTextureLimit)
                DecreaseTextureLimit(DeviceId, TextureLimit);
        }

        private void checkIsLineCutting_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkIsLineCutting.Checked)
            //{
            //    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["IsLineCutting"] = 1;
            //    return;
            //}
            //((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["IsLineCutting"] = 0;
            //return;
        }
    }
}
