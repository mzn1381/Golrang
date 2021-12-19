using PCLOR._00_BaseInfo;
using PCLOR.Classes;
using PCLOR.EnumStatusesDevice;
using PCLOR.Models;
using PCLOR.MyBasicFunction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace PCLOR._01_OperationInfo
{
    public partial class Frm_05_Machines : Form
    {
        public Frm_05_Machine_Status Status { get; set; }
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Class_Documents ClDoc = new Class_Documents();
       

        public Machine SelectedMachine { get; set; }
        public Frm_05_Machines(Frm_05_Machine_Status status)
        {
            Status = status;
            InitializeComponent();
        }

        public Frm_05_Machines()
        {
            InitializeComponent();
        }

        private void Frm_05_Machines_Load(object sender, EventArgs e)
        {

            if (Status==Frm_05_Machine_Status.RegisterDetailForDevice)
                this.Text = "لیست دستگاه ها برای ثبت اظهارات";

            Class_UserScope UserScope = new Class_UserScope();
            
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 147))
                button1.Visible = true;
            if (Status == Frm_05_Machine_Status.EditOrViewDevice)
                BasicFunction.LoadDevices(ButtonEditViewDetail_Click, ConPCLOR, ConPCLOR.ConnectionString, ClDoc, Controls);
            else if (Status == Frm_05_Machine_Status.RegisterDetailForDevice)
                BasicFunction.LoadDevices(ButtonRegisterDescriptionDevice_Click, ConPCLOR, ConPCLOR.ConnectionString, ClDoc, Controls);
        }

        private void ButtonEditViewDetail_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var machine = ((Machine)btn.Tag);
            Form_55_EditSpecsTechnical form_55 = new Form_55_EditSpecsTechnical(machine.ID);
            form_55.ShowDialog();
        }


        private void ButtonRegisterDescriptionDevice_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var machine = ((Machine)btn.Tag);
            Frm_60_RegDescriptionForDevice form_60 = new Frm_60_RegDescriptionForDevice(machine.ID);
            form_60.ShowDialog();
        }

        bool startDragging = false;

        private void button1_Click(object sender, EventArgs e)
        {
            var buttons = this.Controls.OfType<Button>().ToList();
            startDragging = !startDragging;
            button1.Text = startDragging ? "ذخیره تغییرات" : "شروع جابجایی";
            buttons.ForEach(x => x.Draggable(startDragging));
        }

        private void Frm_05_Machines_FormClosing(object sender, FormClosingEventArgs e)
        {
            BasicFunction.SaveLocationDevices(Controls, ConPCLOR);
        }
    }
}
