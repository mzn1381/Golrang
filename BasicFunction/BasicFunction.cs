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
                var machines = ClDoc.ReturnTable(ConPCLOR, @"SELECT ID, Code, namemachine as Namemachine,namemachine as Name , TextureLimit,RoundStop,status as Status, Specstechnical ,Description,FabricType,YarnType,DeviceMark,X,Y ,Area, Gap,teeny FROM [dbo].[Table_60_SpecsTechnical]  where  status=1").ToList<Machine>();
                var TT = machines.Where(c => c.TextureLimit == 0);


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
                    if (item.TextureLimit == 0)
                    {
                        button.BackColor = Color.IndianRed;
                        if (status == Frm_05_Machine_Status.CreateProductForDevice)
                        {
                            button.Click -= Button_Click;
                            button.Click += GetMessageForTextureLimit; 
                        }
                    }
                    Controls.Add(button);
                }

            }

        }
        public static void GetMessageForTextureLimit(object o , EventArgs args) 
        {
            MessageBox.Show("حد بافت دتگاه صفر می باشد و امکان ثبت تولید نیست لطفا حد بافت را افزایش دهید ","اخطار",MessageBoxButtons.OK,MessageBoxIcon.Information);
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

    }
}
