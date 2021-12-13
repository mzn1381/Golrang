using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace PCLOR._01_Frms
{
    public partial class Form01_Backup : Form
    {
        public Form01_Backup()
        {
            InitializeComponent();
        }

        private void Form01_Backup_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.BackupPath;
            textBox2.Text = "PCLOR_" + Class_BasicOperation._OrgCode + "_" + Class_BasicOperation._FinYear + "_" + FarsiLibrary.Utils.PersianDate.Now.ToString("00000000").Replace("/", "");
            folderBrowserDialog1.SelectedPath = Properties.Settings.Default.BackupPath;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()== DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                try
                {
                    SqlConnection Con = new SqlConnection(Properties.Settings.Default.PCLOR);
                    string DataBaseName = Con.Database.ToString();
                    Con.Open();
                    if (!textBox1.Text.EndsWith("\\"))
                        textBox1.Text = textBox1.Text + "\\";
                    SqlCommand Backup = new SqlCommand("BACKUP DATABASE [" + DataBaseName + "] TO  DISK = N'" + textBox1.Text + textBox2.Text + "' WITH NOFORMAT, INIT,  NAME = N'" + DataBaseName + " Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", Con);
                    Backup.ExecuteNonQuery();
                    Con.Close();
                    MessageBox.Show("عملیات پشتیبان گیری با موفقیت انجام شد", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطا در پشتیبان گیری: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    this.Close();
                }
            }
        }

        private void Form01_Backup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.BackupPath = textBox1.Text;
            Properties.Settings.Default.Save();
        }

     
    }
}
