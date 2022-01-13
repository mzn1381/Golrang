using Dapper;
using PCLOR.Classes;
using PCLOR.EnumStatusesDevice;
using PCLOR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR.MyBasicFunction
{
    public class BasicFunction
    {
        public static void LoadDevices(EventHandler Button_Click, SqlConnection ConPCLOR, string connection, Class_Documents ClDoc, Control.ControlCollection Controls, Frm_05_Machine_Status status)
        {

            using (IDbConnection db = new SqlConnection(connection))
            {
                var points = db.Query<MachinePoint>("SELECT X, Y, ID ,namemachine as Name   from   Table_60_SpecsTechnical  where   status = 1", null, commandType: CommandType.Text).OrderBy(x => x.Y).ToList();
                var machines = ClDoc.ReturnTable(ConPCLOR, @"SELECT ID, Code, namemachine as Namemachine,namemachine as Name ,IsDeffective, TextureLimit,RoundStop,status as Status, Specstechnical ,Description,FabricType,YarnType,DeviceMark,X,Y ,Area, Gap,teeny FROM [dbo].[Table_60_SpecsTechnical]  where  status=1").ToList<Machine>();
                foreach (var item in machines)
                {
                    Button button = new Button();
                    button.Text = item.NameMachine;
                    var point = points.FirstOrDefault(c => c.ID == item.ID);
                    button.Location = new Point(point.X, point.Y);
                    button.Draggable(false);
                    button.Tag = item;
                    button.Width = 55;
                    button.Height = 55;
                    button.Click += Button_Click;
                    if (item.TextureLimit == 0 && item.IsInfinitiveTextureLimit == false)
                    {
                        if (item.IsDeffective)
                        {
                            button.BackColor = Color.DarkRed;
                            button.ForeColor = Color.White;
                        }
                        else
                            button.BackColor = Color.MediumVioletRed;
                        if (status == Frm_05_Machine_Status.CreateProductForDevice)
                        {
                            button.Click -= Button_Click;
                            button.Click += GetMessageForTextureLimit;
                        }
                    }
                    else if (item.IsDeffective)
                        button.BackColor = Color.LightYellow;

                    Controls.Add(button);
                }

            }

        }
        public static void GetMessageForTextureLimit(object o, EventArgs args)
        {
            MessageBox.Show("حد بافت دستگاه صفر می باشد و امکان ثبت تولید نیست لطفا حد بافت را افزایش دهید ", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SaveLocationDevices(Control.ControlCollection Controls, SqlConnection ConPCLOR)
        {
            var buttons = Controls.OfType<Button>().ToList();
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    db.Execute($" update Table_60_SpecsTechnical set X =  {buttons[i].Location.X}  ,  Y=  {buttons[i].Location.Y} where ID = {((Machine)buttons[i].Tag).ID} ", commandType: CommandType.Text);
                }
            }

        }

        public static int Recipt(SqlConnection ConWare,string dateCreate, int deviceId, Class_Documents ClDoc,  int wareCode, int functionType, string operationCode, string number = "")
        {
            using (IDbConnection db = new SqlConnection(ConWare.ConnectionString))
            {
                try
                {
                    var queryGetcommodity = $@"  SELECT c.CodeCommondity
                                                 from PCLOR_1_1400.dbo.Table_60_SpecsTechnical as s inner join 
                                                 PCLOR_1_1400.dbo.Table_005_TypeCloth as c on s.FabricType = c.ID
                                                 where s.ID={deviceId}
                                                    ";
                    var codeCommodity = db.QueryFirstOrDefault<int>(queryGetcommodity, null
                        , commandType: CommandType.Text);
                    int ResidNum = ClDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
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
                                                                 
                                                                          ) VALUES (  {ResidNum} , N'{dateCreate}'  , {wareCode},  {functionType} ,
                                                                        {(string.IsNullOrEmpty(operationCode) ? "N''" : operationCode)},N'رسید صادره بابت رسید پارچه خام شماره {number}' , N'{Class_BasicOperation._UserName}' ,getdate(), N'{Class_BasicOperation._UserName}', getdate() );
                                                                       select  Max(columnid)  from Table_011_PwhrsReceipt";
                    var Key = db.QueryFirstOrDefault<int>(commandtxt, null, commandType: CommandType.Text);
                    return Key;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
            }

        }

        public static void ReciptChild(SqlConnection ConWare, int hedearId, int commodityCode,decimal weight, string barcode,int deviceId)
        {

            using (IDbConnection db = new SqlConnection(ConWare.ConnectionString))
            {
                try
                {
                    var query = $@" INSERT INTO Table_012_Child_PwhrsReceipt (
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
               {hedearId},{commodityCode},1,1,1,0,0,N'{Class_BasicOperation._UserName}',getdate(),N'{Class_BasicOperation._UserName}' ,getdate(),0,0, N'{barcode}' , {weight} , {weight} , N'{deviceId}' );";
                    db.Execute(query, null, commandType: CommandType.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["NumberRecipt"] = Key;
            //table_115_ProductBindingSource.EndEdit();
            //var stauts = table_115_ProductTableAdapter1.Update(pCLOR_1_1400DataSet.Table_115_Product);
            //if (stauts <= 0)
            //{
            //    MessageBox.Show("متاسفانه ثبت تولید با شکست مواجه شد ! لطفا دوباره امتحان کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    DeleteLastRecid(Key);
            //    return;
            //}
            //var currenAddProduct = (DataRowView)table_115_ProductBindingSource.Current;
            //foreach (DataRowView Rows in table_115_ProductBindingSource)
            //{
            //if (string.IsNullOrEmpty(Rows["NumberRecipt"].ToString()) || Rows["NumberRecipt"].ToString() == "0")
            //{
            //ID = ID + Rows["ID"] + ",";
            //ID = currenAddProduct["ID"].ToString();
        }



//        private void ExportDraft(string date,int wareCode,int functionType,int personCode,string description,string colorName)
//        {
//            int DraftNumber = 0;
//            string DraftID = "0";
//            DraftId = new SqlParameter("DraftID", SqlDbType.Int);
//            DraftId.Direction = ParameterDirection.Output;
//            string CmdText = "";
//            string DetailsIdDraft = "";
//            //درج کالاهای یک انبار در یک حواله

//            DraftNumber = ClDoc.MaxNumber(Properties.Settings.Default.PWHRS, "Table_007_PwhrsDraft", "Column01");

//            //درج هدر حواله برای هر انبار
//            var query = $@"

//INSERT INTO Table_007_PwhrsDraft 
//(column01, column02, column03, column04, column05, column06, column07, column08, column09,
//column10, column11, column12, column13, column14, column15, 
//column16,
//column17, column18, column19, Column20, Column21,
//Column22, Column23, Column24, Column25, Column26)
//VALUES({DraftNumber},N'{date}',{wareCode},{functionType},{0},N'حواله صادره بابت رنگ مصرفی ش {colorName},0,N'{ Class_BasicOperation._UserName}')

//";
            
            
//            CmdText = (@"INSERT INTO Table_007_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
//            column17, column18, column19, Column20, Column21, Column22, Column23, Column24, Column25, Column26) VALUES(" + DraftNumber + ",'" + txt_Dat.Text + "'," +
//                mlt_Ware.Value + "," + mlt_Function.Value + ",0,'" + "حواله صادره بابت رنگ مصرفی ش" + ((DataRowView)mlt_TypeColor.DropDownList.FindItem(mlt_TypeColor.Value))["ID"].ToString() + "',0,'" + Class_BasicOperation._UserName +
//                "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0,0,0,0,0,0,null,0,1); SET @DraftID = SCOPE_IDENTITY();");

//            foreach (DataRowView item in table_40_ColorPrductionBindingSource)
//            {
//                CmdText = CmdText + (@"INSERT INTO Table_008_Child_PwhrsDraft (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, column16, 
//                        column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, column28, column29, Column30, Column31, Column32, Column33, Column36, Column37) VALUES(@DraftID,"
//                    + (Convert.ToDecimal(item["CodeCommondity"])).ToString() + ",(select Column07 from table_004_CommodityAndIngredients where Columnid=" + item["CodeCommondity"].ToString() + "),0,0," + (Convert.ToDecimal(item["Consumption"])).ToString() + "," +
//                    item["Consumption"].ToString() + ",0,0,0,0,N'به شماره کارت تولید" + txt_Number.Text + "',NULL,NULL,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
//                    "',getdate(),NULL,NULL,NULL,0,0,0,0,0,0,NULL,NULL,0,0,0,0)");
//                DetailsIdDraft = DetailsIdDraft + item["ID"].ToString() + ",";


//                //ClDoc.RunSqlCommand(ConPCLOR.ConnectionString, @"Update table_40_ColorPrduction set NumberDraft=" + DraftID + " where fk=" + txt_Id.Text);

//                CmdText = CmdText + @"Update " + ConPCLOR.Database + ".dbo.table_40_ColorPrduction set NumberDraft=@DraftID  Where fk=" + txt_Id.Text;


//            }


//            using (SqlConnection Con = new SqlConnection(PCLOR.Properties.Settings.Default.PWHRS))
//            {
//                Con.Open();

//                SqlTransaction sqlTran = Con.BeginTransaction();
//                SqlCommand Command = Con.CreateCommand();
//                Command.Transaction = sqlTran;

//                try
//                {
//                    Command.CommandText = CmdText;
//                    Command.Parameters.Add(DraftId);
//                    Command.ExecuteNonQuery();
//                    sqlTran.Commit();
//                    /////

//                    try
//                    {
//                        SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftId, ConWare);
//                        DataTable Table = new DataTable();
//                        goodAdapter.Fill(Table);

//                        //محاسبه ارزش و ذخیره آن در جدول Child1

//                        foreach (DataRow item in Table.Rows)
//                        {
//                            if (Class_BasicOperation._WareType)
//                            {
//                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_FIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value, ConWare);
//                                DataTable TurnTable = new DataTable();
//                                Adapt.Fill(TurnTable);
//                                DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftId + " and DetailID=" + item["Columnid"].ToString());

//                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
//                                    + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
//                                UpdateCommand.ExecuteNonQuery();

//                            }
//                            else
//                            {
//                                SqlDataAdapter Adapt = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value + ",@Date='" + txt_Dat.Text + "',@id=" + DraftId + ",@residid=0", ConWare);
//                                DataTable TurnTable = new DataTable();
//                                Adapt.Fill(TurnTable);
//                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
//                              + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
//                                UpdateCommand.ExecuteNonQuery();
//                            }

//                        }
//                    }


//                    catch
//                    {
//                    }


//                    gridEX1.DropDowns["Draft"].DataSource = ClDoc.ReturnTable(ConWare, @" select Columnid, column01 from Table_007_PwhrsDraft ");
//                    Messages = Messages + "حواله انبار رنگ مصرفی به شماره  " + DraftNumber.ToString() + Environment.NewLine;
//                }
//                catch (Exception es)
//                {
//                    sqlTran.Rollback();
//                    this.Cursor = Cursors.Default;
//                    Class_BasicOperation.CheckExceptionType(es, this.Name);
//                }
//            }

//            this.Cursor = Cursors.Default;


//        }





    }
}
