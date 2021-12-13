using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PCLOR._01_Frms
{
    public partial class Form02_Restore : Form
    {
        public Form02_Restore()
        {
            InitializeComponent();
        }

        private void bt_BackupPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void bt_Database_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void Form02_Restore_Load(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "")
            {
                if (DialogResult.OK == MessageBox.Show("به علت راه اندازی مجدد برنامه پس از بازیابی نسخه پشتیبان، ابتدا تغییرات خود را ذخیره کرده و در صورت اطمینان ادامه کار را تأیید کنید ", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به بازیابی نسخه پشتیبان هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        this.Cursor = Cursors.WaitCursor;

                        //if (!(textBox1.Text.EndsWith("\\")))
                        //textBox1.Text = textBox1.Text + "\\";

                        if (!(textBox2.Text.EndsWith("\\")))
                            textBox2.Text = textBox2.Text + "\\";

                        try
                        {
                            SqlConnection Connection = new SqlConnection(Properties.Settings.Default.PCLOR);
                            Connection.Open();
                            string LogicalName = null;
                            string PhysicalName = null;
                            SqlCommand GetDatabaseName = new SqlCommand("RESTORE Filelistonly FROM  DISK = N'" + textBox1.Text + "'  with file=1", Connection);
                            SqlDataReader Reader = GetDatabaseName.ExecuteReader();
                            while (Reader.Read())
                            {
                                LogicalName = Reader[0].ToString();
                                PhysicalName = Reader[1].ToString();
                                if (!string.IsNullOrEmpty(LogicalName) && !string.IsNullOrEmpty(PhysicalName))
                                    break;
                            }
                            Reader.Close();
                            SqlCommand Command = new SqlCommand();
                            string cmdtxt;

                            if (PhysicalName == textBox2.Text + Connection.Database + ".mdf")
                                cmdtxt = "USE master ALTER DATABASE [" + Connection.Database.ToString() + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [" + Connection.Database + "] FROM  DISK = N'" + textBox1.Text + "' WITH  FILE = 1,  NOUNLOAD,  STATS = 10; ALTER DATABASE [" + Connection.Database + "] SET MULTI_USER; ";
                            else
                                cmdtxt = "USE master ALTER DATABASE [" + Connection.Database.ToString() + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [" + Connection.Database.ToString() +
                                    "] FROM  DISK = N'" + textBox1.Text + "' WITH  MOVE N'" + LogicalName + "' TO N'" +
                                    textBox2.Text + Connection.Database +
                                    ".mdf',  MOVE N'" + LogicalName + "_Log' TO N'" +
                                    textBox2.Text + Connection.Database + "_log.ldf', REPLACE,  STATS = 10; ALTER DATABASE [" + Connection.Database + "] SET MULTI_USER;";

                            Command.CommandText = cmdtxt;
                            Command.Connection = Connection;
                            Command.ExecuteNonQuery();

                            Connection.Close();

                            MessageBox.Show("بازیابی فایل پشتیبان با موفقیت صورت گرفت", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                            Application.Restart();


                        }
                        catch (Exception er)
                        {
                            MessageBox.Show(er.Message);
                        }
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }
    }
}
