using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR.Product
{
    public partial class Frm_005_SelectMachine : Form
    {
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_UserScope UserScope = new Classes.Class_UserScope();

        public Frm_005_SelectMachine()
        {
            InitializeComponent();
        }

        private void Frm_005_SelectMachine_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_PCLOR.Table_60_SpecsTechnical' table. You can move, or remove it, as needed.
            // this.table_60_SpecsTechnicalTableAdapter.Fill(this.dataSet_05_PCLOR.Table_60_SpecsTechnical);
            gridEX1.DataSource = ClDoc.ReturnTable(ConPCLOR, @"select * from Table_60_SpecsTechnical where status=1");
        }

        private void gridEX1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void gridEX1_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 149))
            {
            if (e.Column.Key == "Column1")
                {
                    Frm_010_ProgramMachine frm = new Frm_010_ProgramMachine(Convert.ToInt32(gridEX1.GetValue("ID")));
                    frm.Show();
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 145))
            {
            if (e.Column.Key=="Column2")
            {
                Frm_015_Product frm = new Frm_015_Product(Convert.ToInt32(gridEX1.GetValue("ID")));
                frm.Show();
            }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column44", 154))
            {
            if (e.Column.Key=="Column3")
            {
                Frm_025_ReportDeviceFailure frm = new Frm_025_ReportDeviceFailure(Convert.ToInt32(gridEX1.GetValue("ID")));
                frm.Show();
            }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", Class_BasicOperation.MessageType.None);

        }
    }
}
