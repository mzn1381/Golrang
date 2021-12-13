using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCLOR._03_Bank
{
    public partial class Frm_14_TransferCheq : Form
    {

        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.PBASE);
        SqlConnection ConPCLOR = new SqlConnection(Properties.Settings.Default.PCLOR);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.PWHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.PSALE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.PBANK);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.PACNT);

        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();

        public Frm_14_TransferCheq()
        {
            InitializeComponent();
        }

        private void Frm_14_TransferCheq_Load(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                mlt_FirstBranch.DataSource = ClDoc.ReturnTable(ConBank, @"select Columnid,Column02 from Table_020_BankCashAccInfo where Column01=1");

            }
            else
            {

                mlt_FirstBranch.DataSource = ClDoc.ReturnTable(ConPCLOR, @"SELECT  
                         " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.ColumnId , 
                         " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column02 
FROM            " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo INNER JOIN
                         " + ConBase.Database + @".dbo.Table_045_PersonInfo ON " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column35 = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId
WHERE        (" + ConBase.Database + @".dbo.Table_045_PersonInfo.Column23 = '" + Class_BasicOperation._UserName + "') and " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column01=1");
              
                string branch = ClDoc.ExScalar(ConBank.ConnectionString, @"SELECT  
                         " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.ColumnId 
FROM            " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo INNER JOIN
                         " + ConBase.Database + @".dbo.Table_045_PersonInfo ON " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column35 = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId
WHERE        (" + ConBase.Database + @".dbo.Table_045_PersonInfo.Column23 = '" + Class_BasicOperation._UserName + "')  and " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column01=1");

                mlt_FirstBranch.Value = int.Parse(branch);

                
                //gridEX2.DataSource = ClDoc.ReturnTable(ConBank, @"select * from Table_035_ReceiptCheques where Column01="+mlt_FirstBranch.Value+"");

               

            }

            mlt_SecondBranch.DataSource = ClDoc.ReturnTable(ConBank, @"select Columnid,Column02 from Table_020_BankCashAccInfo where column01=1");
            mlt_Statuse.DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_060_ChequeStatus ");
            mlt_Statuse.Value = 100;
            gridEX2.DropDowns["Project"].DataSource = ClDoc.ReturnTable(ConBase, @"Select Column00,Column01,Column02 from Table_035_ProjectInfo");
            gridEX2.DropDowns["Center"].DataSource = ClDoc.ReturnTable(ConBase, @"Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo");
            gridEX2.DropDowns["ToBank"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");
            gridEX2.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo");

            gridEX2.DropDowns["Doc"].DataSource = ClDoc.ReturnTable(ConACNT, "Select ColumnId,Column00 from Table_060_SanadHead");
            //gridEX2.DropDowns["Status"].DataSource = 

            gridEX2.DropDowns["Person"].DataSource = ClDoc.ReturnTable(ConBase, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            ////gridEX2.DropDowns["Bank"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");
            
            // gridEX2.DropDowns["Banks"].DataSource = ClDoc.ReturnTable(ConBank, @"select ColumnId,Column02 from Table_020_BankCashAccInfo");

        }

        private void gridEX2_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void mlt_FirstBranch_ValueChanged(object sender, EventArgs e)
        {
            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                if (mlt_FirstBranch.Text!="")
                {
                    button2_Click(sender, e);

                }
            }
            else
            {
                mlt_Statuse.Value = 100;

                if (mlt_FirstBranch.Text != "")
                {
                    button2_Click(sender, e);

                }
            }

            }
        string number = "";
        string cmd = "";
        string updatesanad = "";
        DataTable sanad=new DataTable();
        string Usersabt, Bank,  cashier;
        private void button1_Click(object sender, EventArgs e)
        {
           
            if (gridEX2.RowCount>0)
            {
                if (mlt_SecondBranch.Text == "")
                {
                    MessageBox.Show("لطفا شعبه مقصد را مشخص نمایید");
                    return;
                }

              

                number = "";
            cmd = "";
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetCheckedRows())
            {
                number += item.Cells["ChqBack"].Value.ToString()+",";
                if (number=="")
                {
                    Class_BasicOperation.ShowMsg("", "لطفا چک موردنطر را انتخاب نمایید", Class_BasicOperation.MessageType.Stop);
                    return;
                }
                else
                {
                     cashier = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column35  from Table_020_BankCashAccInfo where Columnid="+mlt_SecondBranch.Value+"");
                         Bank = ClDoc.ExScalar(ConBank.ConnectionString, @"select Columnid  from Table_020_BankCashAccInfo where Columnid=" + mlt_SecondBranch.Value + "");
                         Usersabt = ClDoc.ExScalar(ConBank.ConnectionString, @"select Column31  from Table_020_BankCashAccInfo where Columnid=" + mlt_SecondBranch.Value + "");
                         sanad = ClDoc.ReturnTable(ConACNT, @"select * from Table_065_SanadDetail where Column00 ="+ item.Cells["TurnSanad"].Value.ToString() + "");
                        cmd += @"Insert Into "+ConPCLOR.Database+ @".dbo.Table_130_TransferBranch 
                       ( IDCheq,BranchFirst,BranchSecond,Backcheq,Usersabt,DateSabt) values(" + item.Cells["ChqID"].Value.ToString() +
                        ", " +mlt_FirstBranch.Value+ "," +mlt_SecondBranch.Value+ ","+ item.Cells["ChqBack"].Value.ToString() + ",'" +Class_BasicOperation._UserName+"',getdate())";
                    cmd += @"; update Table_035_ReceiptCheques set Column01=" + mlt_SecondBranch.Value + ", Column46=" + cashier + ",Column42='"+ Usersabt + "',Column44='" + Usersabt + "' where Columnid=" + item.Cells["ChqID"].Value + ";  " +
                            "Update Table_065_TurnReception set Column05=" + cashier + ",column06=" + Bank + ",column16='" + Usersabt + "' where columnid=" + item.Cells["LastTurnID"].Value + "; " +
                           " ";
                       

                    }

           
                if (sanad.Rows.Count>0)
                {
                   string Userfirstbranch = ClDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select distinct Columnid   from Table_045_PersonInfo where Columnid=" + gridEX2.GetValue("TurnPerson").ToString() + "),0)");
                    string number = ClDoc.ExScalar(ConBank.ConnectionString, @"select columnid from Table_065_TurnReception where column01=" + item.Cells["ChqID"].Value + "");
                    string UserSecoundBranch= ClDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select Column35  from Table_020_BankCashAccInfo where Columnid='" + mlt_SecondBranch.Value + "'),0)");
                    foreach (DataRow row in sanad.Rows)
                    {
                        if (row["Column17"].ToString()== number)
                        {

                        if (row["Column07"].ToString() == Userfirstbranch.ToString()  )
                        {
                            updatesanad += " update " + ConACNT.Database + ".dbo.Table_065_SanadDetail set Column07=" + UserSecoundBranch +" where column07="+ row["Column07"]+" and column17 IN ("+ number +")";
                        }

                        updatesanad += " update " + ConACNT.Database + ".dbo.Table_065_SanadDetail set Column18='" + Usersabt + "',Column20='" + Usersabt + "' where Column00 = " + row["Column00"]+ "and column17 IN (" + number + ")" +
                            " update " + ConACNT.Database + ".dbo.Table_065_SanadDetail set Column18='" + Usersabt + "',Column20='" + Usersabt + "' where Column00 = " + row["Column00"]+ "and column17 IN (" + number + ")";
                        }
                        }
                    }
                }
            if (cmd!="")
            {
                Class_BasicOperation.SqlTransactionMethodScaler(ConBank.ConnectionString, cmd + updatesanad);
                Class_BasicOperation.ShowMsg("", "شماره چک های زیر با موفیقت انتقال داده شد"+Environment.NewLine+ number.TrimEnd(','), Class_BasicOperation.MessageType.Information);
                    button2_Click(sender, e);
                //Frm_14_TransferCheq_Load(sender, e);
                //mlt_FirstBranch_ValueChanged(sender, e);
            }
        }
        }

        private void mlt_Statuse_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Statuse, "Column02", "Columnid");

            bool isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._FinYear);
            if (isadmin)
            {
                if (mlt_FirstBranch.Text != "")
                {
                    button2_Click(sender, e);

                }
            }
            else
            {
                if (mlt_FirstBranch.Text != "")
                {
                    button2_Click(sender, e);

                }
            }

        }

        private void mlt_Statuse_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (mlt_Statuse.Text == "")
            {
                MessageBox.Show("لطفا وضعیت را مشخص کنید");
                return;
            }
            gridEX2.DataSource = ClDoc.ReturnTable(ConBank, @"
                    SELECT              CAST(0 AS bit) AS Error, ' ' AS ErrorDes, dbo.Table_035_ReceiptCheques.ColumnId AS ChqID, TurnReception.LastState, TurnReception.LastTurnID, 
                      dbo.Table_035_ReceiptCheques.Column00 AS ChqBack, dbo.Table_035_ReceiptCheques.Column02 AS ChqDate1, 
                      dbo.Table_035_ReceiptCheques.Column03 AS ChqNum, dbo.Table_035_ReceiptCheques.Column04 AS ChqDate2,
                      dbo.Table_035_ReceiptCheques.Column05 AS ChqPrice, Table_065_TurnReception_2.Column04 AS TurnDate, 
Table_065_TurnReception_2.Column05 AS TurnPerson, 
                      Table_065_TurnReception_2.Column06 AS TurnBankBox, Table_065_TurnReception_2.Column07 AS TurnACC,
Table_065_TurnReception_2.Column08 AS GroupCode, 
                      Table_065_TurnReception_2.Column09 AS KolCode, Table_065_TurnReception_2.Column10 AS MoeinCode, 
Table_065_TurnReception_2.Column11 AS TafsiliCode, 
                      Table_065_TurnReception_2.Column12 AS JozCode, Table_065_TurnReception_2.Column13 AS TurnSanad, Table_065_TurnReception_2.Column14 AS TurnCenter, 
                      Table_065_TurnReception_2.Column15 AS TurnProject, Table_065_TurnReception_2.Column16 AS RegisterUser, Table_065_TurnReception_2.Column19 AS TurnDes, 
                      Table_065_TurnReception_2.Column20, Table_065_TurnReception_2.Column21, Table_065_TurnReception_2.Column22, 
                      dbo.Table_010_BankNames.Column01 AS MainBankName, dbo.Table_035_ReceiptCheques.Column09 AS ChqBranch, 
                      dbo.Table_035_ReceiptCheques.Column10 AS ChqAccNum, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column02 AS PayPersonName, 
                      dbo.Table_035_ReceiptCheques.Column52, dbo.Table_035_ReceiptCheques.Column53, dbo.Table_035_ReceiptCheques.Column06,dbo.Table_035_ReceiptCheques.Column01 as cash
FROM         dbo.Table_035_ReceiptCheques INNER JOIN
                          (SELECT     TOP (100) PERCENT Column01 AS PaperID, ColumnId AS LastTurnID, Column02 AS LastState
                            FROM          dbo.Table_065_TurnReception
                            WHERE      (Column13 <> 0) AND (ColumnId IN
                                                       (SELECT     MAX(ColumnId) AS Expr1
                                                         FROM          dbo.Table_065_TurnReception AS Table_065_TurnReception_1
                                                         WHERE      (Column13 <> 0)
                                                         GROUP BY Column01))
                            ORDER BY PaperID, LastTurnID DESC) AS TurnReception ON dbo.Table_035_ReceiptCheques.ColumnId = TurnReception.PaperID INNER JOIN
                      dbo.Table_065_TurnReception AS Table_065_TurnReception_2 ON TurnReception.LastTurnID = Table_065_TurnReception_2.ColumnId INNER JOIN
                      dbo.Table_010_BankNames ON dbo.Table_035_ReceiptCheques.Column08 = dbo.Table_010_BankNames.Column00 INNER JOIN
                      " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dbo.Table_035_ReceiptCheques.Column07 = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId
 WHERE  TurnReception.LastState=" + mlt_Statuse.Value + " and dbo.Table_035_ReceiptCheques.Column01=" + mlt_FirstBranch.Value + "");

        }
    }
}
