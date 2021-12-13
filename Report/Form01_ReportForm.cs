using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;

namespace PCLOR.Report
{
  
    public partial class Form01_ReportForm : Form
    {
        DataTable _Table = new DataTable();
        DataTable _Table2 = new DataTable();
        int _FormNumber = 0;
        string _Param1, _Param2, _Param3, _Param4, _Param5, _Param6;
        public Form01_ReportForm(DataTable Table,int FormNumber)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
        }
        public Form01_ReportForm(DataTable Table, int FormNumber,string Param1,string Param2)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        public Form01_ReportForm(DataTable Table, int FormNumber, string Param1, string Param2,string Param3)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
            _Param1 = Param1;
            _Param2 = Param2;
            _Param3 = Param3;
        }
        public Form01_ReportForm(DataTable Table1,DataTable Table2, int FormNumber, string Param1, string Param2)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table1;
            _Table2 = Table2;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        public Form01_ReportForm(DataTable Table, int FormNumber, string Param1, string Param2,
            string Param3, string Param4, string Param5,string Param6)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
            _Param1 = Param1;
            _Param2 = Param2;
            _Param3 = Param3;
            _Param4 = Param4;
            _Param5 = Param5;
            _Param6 = Param6;
        }
        private void Form01_ReportForm_Load(object sender, EventArgs e)
        {
            //نمایش دفتر تعدادی انبار
            if (_FormNumber == 1)
            {
                Report.Rpt01_NumericDoc rpt = new Rpt01_NumericDoc();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش دفتر تعدادی انبار
            else if (_FormNumber == 2)
                {
                    Report.Rpt02_RialiDoc rpt = new Rpt02_RialiDoc();
                    rpt.SetDataSource(_Table);
                    rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                    rpt.SetParameterValue("Param1", _Param1);
                    rpt.SetParameterValue("Param2", _Param2);
                    crystalReportViewer1.ReportSource = rpt;
                }
            //نمایش گزارش مقطعی تعدادی
            else if (_FormNumber == 3)
            {
                Report.Rpt03_MaghtaeeTedadi rpt = new Rpt03_MaghtaeeTedadi();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                TextObject whichWare = (TextObject)rpt.ReportDefinition.ReportObjects["WhichWare"];
                whichWare.Text = _Param1;
                crystalReportViewer1.ReportSource = rpt;
            }
                //نمایش گزارش دوره ای تعدادی
            else if (_FormNumber == 4)
            {
                Report.Rpt04_Periodic_Numeric rpt = new Rpt04_Periodic_Numeric();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                rpt.SetParameterValue("Param4", _Param4);

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش مقطعی ریالی
            else if (_FormNumber == 5)
            {
                Report.Rpt05_MaghtaeeRiali rpt = new Rpt05_MaghtaeeRiali();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش دوره ای ریالی
            else if (_FormNumber == 6)
            {
                Report.Rpt06_Periodic_Riali rpt = new Rpt06_Periodic_Riali();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش کارتکس تعدادی
            else if (_FormNumber == 7)
            {
                Report.Rpt07_Numeric_Cardex rpt = new Rpt07_Numeric_Cardex();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("DateStart", _Param1);
                rpt.SetParameterValue("DateEnd", _Param2);
                rpt.SetParameterValue("Ware", _Param3);
                rpt.SetParameterValue("GoodName", _Param4);
                rpt.SetParameterValue("GoodCode", _Param5);
                rpt.SetParameterValue("Unit", _Param6);

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش کارتکس ریالی
            else if (_FormNumber == 8)
            {
                Report.Rpt08_Riali_Cardex rpt = new Rpt08_Riali_Cardex();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
              

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش دفتر ریالی با ستون های چربی و آب
            else if (_FormNumber == 9)
            {
                Report.Rpt02_RialiDoc_2 rpt = new Rpt02_RialiDoc_2();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش دفتر تعدادی با ستون های چربی و آب
            else if (_FormNumber ==10)
            {
                Report.Rpt01_NumericDoc_2 rpt = new Rpt01_NumericDoc_2();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
                //نمایش گردش ریالی
            else if (_FormNumber == 11)
            {
                Report.Rpt09_Riali_Trun rpt = new Rpt09_Riali_Trun();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1",_Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
                //نمایش گردش ریالی با چربی و آب
            else if (_FormNumber == 12)
            {
                Report.Rpt09_Riali_Trun_2 rpt = new Rpt09_Riali_Trun_2();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گردش تعدادی
            else if (_FormNumber == 13)
            {
                Report.Rpt10_Numeric_Trun rpt = new Rpt10_Numeric_Trun();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
                //گردش تعدادی با چربی و آب
            else if (_FormNumber == 14)
            {
                Report.Rpt10_Numeric_Trun_2 rpt = new Rpt10_Numeric_Trun_2();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            else if (_FormNumber == 15)
            {
                Report.Rpt07_Numeric_Cardex_2 rpt = new Rpt07_Numeric_Cardex_2();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("DateStart", _Param1);
                rpt.SetParameterValue("DateEnd", _Param2);
                rpt.SetParameterValue("Ware", _Param3);
                rpt.SetParameterValue("GoodName", _Param4);
                rpt.SetParameterValue("GoodCode", _Param5);
                crystalReportViewer1.ReportSource = rpt;
            }
                //گزارش کسر موجودی
            else if (_FormNumber == 16)
            {
                Report.Rpt11_StockReduction rpt = new Rpt11_StockReduction();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش خروج گاریها
            else if (_FormNumber == 17)
            {
                Report.Rpt12_ExitCarrier rpt = new Rpt12_ExitCarrier();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }

            else if (_FormNumber == 18)
            {
                Report.Rpt07_Numeric_Cardex_2 rpt = new Rpt07_Numeric_Cardex_2();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("DateStart", _Param1);
                rpt.SetParameterValue("DateEnd", _Param2);
                rpt.SetParameterValue("Ware", _Param3);
                rpt.SetParameterValue("GoodName", _Param4);
                rpt.SetParameterValue("GoodCode", _Param5);
                rpt.SetParameterValue("Unit", _Param6);

                crystalReportViewer1.ReportSource = rpt;
            }

            else if (_FormNumber == 19)
            {
                Report.Rpt05_MaghtaeeRiali3 rpt = new Rpt05_MaghtaeeRiali3();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }

        }
    }
}
