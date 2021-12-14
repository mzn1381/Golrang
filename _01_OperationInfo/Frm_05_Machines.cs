using Dapper;
using PCLOR.Classes;
using PCLOR.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR._01_OperationInfo
{
    public partial class Frm_05_Machines : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        public Machine SelectedMachine { get; set; }

        IEnumerable<Machine> machines = new List<Machine>();

        public Frm_05_Machines()
        {
            InitializeComponent();
        }

        private void Frm_05_Machines_Load(object sender, EventArgs e)
        {
            Classes.Class_UserScope UserScope = new Classes.Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 147))
                button1.Visible = true;

            //var result = ClDoc.ExScalar(ConPCLOR.ConnectionString, "select value from Table_80_Setting where ID=27");
            using (IDbConnection db = new SqlConnection(ConPCLOR.ConnectionString))
            {


                //var t=db.Query("SELECT X, Y, namemachine as Name   from   Table_60_SpecsTechnical  where   status = 1",null,commandType:CommandType.Text);
                var points = db.Query<MachinePoint>("SELECT X, Y, ID ,namemachine as Name   from   Table_60_SpecsTechnical  where   status = 1",null,commandType:CommandType.Text).OrderBy(x=>x.Y).ToList();

                //var result = ClDoc.ReturnTable(ConPCLOR, @" SELECT X, Y, namemachine as Name   from   Table_60_SpecsTechnical  where   status = 1  ");
                //var points = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<MachinePoint>>(result.ToString()).OrderBy(x => x.Y).ToList();
                machines = ClDoc.ReturnTable(ConPCLOR, @"SELECT ID, Code, namemachine as Namemachine,namemachine as Name ,  status as Status, Specstechnical as Description FROM [dbo].[Table_60_SpecsTechnical]  where  status=1").ToList<Machine>();
                foreach (var item in machines)
                {
                    Button button = new Button();
                    button.Text = item.NameMachine;
                    var point = points.FirstOrDefault(c=>c.ID==item.ID);
                    button.Location = new Point(point.X, point.Y);
                    button.Draggable(false);
                    button.Click += Button_Click;
                    Controls.Add(button);
                }
                
                
                
                
                
                var buttons = this.Controls.OfType<Button>().Where(b => b.Name.StartsWith("B"));

                buttons.ToList().ForEach(button =>
                {
                    var point = points.FirstOrDefault(p => p.Name.Equals(button.Name));
                    if (point != null)
                        button.Location = new Point(point.X, point.Y);

                    var machine = machines.FirstOrDefault(m => m.Name.Equals(button.Name));
                    if (machine != null)
                    {
                        if (machine.Status.Equals(false))
                            button.BackColor = SystemColors.WindowFrame;

                        ToolTip toolTip = new ToolTip();
                        toolTip.SetToolTip(button, machine.Description);

                        button.Tag = machine;
                        button.Click += Button_Click;
                    }
                    else
                        button.BackColor = SystemColors.WindowFrame;
                });

                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    var button = buttons.FirstOrDefault(m => m.Name.Equals(point.Name));

                    if (button != null)
                        button.TabIndex = i + 1;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (startDragging)
                return;

            SelectedMachine = (Machine)((Button)sender).Tag;
            this.DialogResult = DialogResult.OK;
        }

        bool startDragging = false;

        private void button1_Click(object sender, EventArgs e)
        {
            var buttons = this.Controls.OfType<Button>().Where(b => b.Name.StartsWith("B")).ToList();

            startDragging = !startDragging;
            button1.Text = startDragging ? "ذخیره تغییرات" : "شروع جابجایی";
            buttons.ForEach(x => x.Draggable(startDragging));

            if (startDragging == false)
            {
                var result = ClDoc.ExScalar(ConPCLOR.ConnectionString, string.Format("update Table_80_Setting set value=N'{0}' where ID=27   select @@rowcount", Newtonsoft.Json.JsonConvert.SerializeObject(buttons.Select(x => new MachinePoint { Name = x.Name, X = x.Location.X, Y = x.Location.Y }))));
            }
        }
    }
}
