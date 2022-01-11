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

        private void Recipt(SqlConnection ConWare, int deviceId, Class_Documents ClDoc, string date, int wareCode, int functionType, string operationCode, string number = "")
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
                                                                 
                                                                          ) VALUES (  {ResidNum} , N'{txt_DateTime.Text}'  , {WareCode},  {FunctionType} ,
                                                                        {(string.IsNullOrEmpty(lblOperationCode.Text) ? "N''" : lblOperationCode.Text)},N'رسید صادره بابت رسید پارچه خام شماره {txt_Number.Text}' , N'{Class_BasicOperation._UserName}' ,getdate(), N'{Class_BasicOperation._UserName}', getdate() );
                                                                       select  Max(columnid)  from Table_011_PwhrsReceipt";
                    var Key = db.QueryFirstOrDefault<int>(commandtxt, null, commandType: CommandType.Text);
                    string query = "";
                    ((DataRowView)table_115_ProductBindingSource.CurrencyManager.Current)["NumberRecipt"] = Key;
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
                    MessageBox.Show(ex.Message);
                    //Class_BasicOperation.CheckExceptionType(ex);
                }
            }

        }







    }
}
