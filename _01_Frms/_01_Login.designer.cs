namespace PCLOR._01_Frms
{
    partial class _01_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_01_Login));
            this.bt_Login = new Janus.Windows.EditControls.UIButton();
            this.bt_OpenYear = new Janus.Windows.EditControls.UIButton();
            this.bt_OpenComp = new Janus.Windows.EditControls.UIButton();
            this.txt_Year = new System.Windows.Forms.TextBox();
            this.txt_CompName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Password2 = new System.Windows.Forms.TextBox();
            this.txt_UserName2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_SysName = new System.Windows.Forms.Label();
            this.p_Logo = new System.Windows.Forms.PictureBox();
            this.btnreturn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.p_Logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnreturn)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Login
            // 
            this.bt_Login.Location = new System.Drawing.Point(211, 219);
            this.bt_Login.Name = "bt_Login";
            this.bt_Login.Size = new System.Drawing.Size(245, 21);
            this.bt_Login.TabIndex = 4;
            this.bt_Login.Text = "ورود";
            this.bt_Login.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
            this.bt_Login.Click += new System.EventHandler(this.bt_Login_Click);
            // 
            // bt_OpenYear
            // 
            this.bt_OpenYear.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Ellipsis;
            this.bt_OpenYear.Location = new System.Drawing.Point(180, 127);
            this.bt_OpenYear.Name = "bt_OpenYear";
            this.bt_OpenYear.Size = new System.Drawing.Size(25, 21);
            this.bt_OpenYear.TabIndex = 10;
            this.bt_OpenYear.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
            this.bt_OpenYear.Click += new System.EventHandler(this.bt_OpenYear_Click);
            // 
            // bt_OpenComp
            // 
            this.bt_OpenComp.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Ellipsis;
            this.bt_OpenComp.Location = new System.Drawing.Point(180, 100);
            this.bt_OpenComp.Name = "bt_OpenComp";
            this.bt_OpenComp.Size = new System.Drawing.Size(25, 21);
            this.bt_OpenComp.TabIndex = 9;
            this.bt_OpenComp.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
            this.bt_OpenComp.Click += new System.EventHandler(this.bt_OpenComp_Click);
            // 
            // txt_Year
            // 
            this.txt_Year.BackColor = System.Drawing.Color.White;
            this.txt_Year.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Year.Location = new System.Drawing.Point(211, 127);
            this.txt_Year.Name = "txt_Year";
            this.txt_Year.ReadOnly = true;
            this.txt_Year.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_Year.Size = new System.Drawing.Size(245, 21);
            this.txt_Year.TabIndex = 1;
            this.txt_Year.Tag = "";
            this.txt_Year.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CompName_KeyPress);
            // 
            // txt_CompName
            // 
            this.txt_CompName.BackColor = System.Drawing.Color.White;
            this.txt_CompName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_CompName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CompName.Location = new System.Drawing.Point(211, 100);
            this.txt_CompName.Name = "txt_CompName";
            this.txt_CompName.ReadOnly = true;
            this.txt_CompName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_CompName.Size = new System.Drawing.Size(245, 21);
            this.txt_CompName.TabIndex = 0;
            this.txt_CompName.Tag = "";
            this.txt_CompName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CompName_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(463, 102);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "نام سازمان:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(463, 129);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "سال مالی :";
            // 
            // txt_Password2
            // 
            this.txt_Password2.BackColor = System.Drawing.Color.White;
            this.txt_Password2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Password2.Location = new System.Drawing.Point(211, 182);
            this.txt_Password2.Name = "txt_Password2";
            this.txt_Password2.PasswordChar = '*';
            this.txt_Password2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_Password2.Size = new System.Drawing.Size(245, 21);
            this.txt_Password2.TabIndex = 3;
            this.txt_Password2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Password2_KeyPress);
            // 
            // txt_UserName2
            // 
            this.txt_UserName2.BackColor = System.Drawing.Color.White;
            this.txt_UserName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserName2.Location = new System.Drawing.Point(211, 156);
            this.txt_UserName2.Name = "txt_UserName2";
            this.txt_UserName2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txt_UserName2.Size = new System.Drawing.Size(245, 21);
            this.txt_UserName2.TabIndex = 2;
            this.txt_UserName2.Tag = "";
            this.txt_UserName2.Text = "a";
            this.txt_UserName2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_CompName_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(471, 184);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "رمز عبور :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(463, 158);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "نام کاربری :";
            // 
            // lbl_SysName
            // 
            this.lbl_SysName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SysName.Font = new System.Drawing.Font("B Homa", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lbl_SysName.ForeColor = System.Drawing.Color.White;
            this.lbl_SysName.Location = new System.Drawing.Point(211, 29);
            this.lbl_SysName.Name = "lbl_SysName";
            this.lbl_SysName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_SysName.Size = new System.Drawing.Size(315, 36);
            this.lbl_SysName.TabIndex = 11;
            this.lbl_SysName.Text = "ورود به سیستم";
            this.lbl_SysName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_SysName.Click += new System.EventHandler(this.lbl_SysName_Click);
            // 
            // p_Logo
            // 
            this.p_Logo.Image = ((System.Drawing.Image)(resources.GetObject("p_Logo.Image")));
            this.p_Logo.Location = new System.Drawing.Point(22, 87);
            this.p_Logo.Name = "p_Logo";
            this.p_Logo.Size = new System.Drawing.Size(127, 129);
            this.p_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.p_Logo.TabIndex = 116;
            this.p_Logo.TabStop = false;
            this.p_Logo.Click += new System.EventHandler(this.p_Logo_Click);
            // 
            // btnreturn
            // 
            this.btnreturn.Image = ((System.Drawing.Image)(resources.GetObject("btnreturn.Image")));
            this.btnreturn.Location = new System.Drawing.Point(14, 13);
            this.btnreturn.Name = "btnreturn";
            this.btnreturn.Size = new System.Drawing.Size(29, 28);
            this.btnreturn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnreturn.TabIndex = 117;
            this.btnreturn.TabStop = false;
            this.btnreturn.Click += new System.EventHandler(this.btnreturn_Click);
            // 
            // _01_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(547, 293);
            this.ControlBox = false;
            this.Controls.Add(this.btnreturn);
            this.Controls.Add(this.p_Logo);
            this.Controls.Add(this.lbl_SysName);
            this.Controls.Add(this.bt_Login);
            this.Controls.Add(this.bt_OpenYear);
            this.Controls.Add(this.bt_OpenComp);
            this.Controls.Add(this.txt_Year);
            this.Controls.Add(this.txt_CompName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_Password2);
            this.Controls.Add(this.txt_UserName2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "_01_Login";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ورود به سیستم";
            this.Load += new System.EventHandler(this._01_Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.p_Logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnreturn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.EditControls.UIButton bt_Login;
        private Janus.Windows.EditControls.UIButton bt_OpenYear;
        private Janus.Windows.EditControls.UIButton bt_OpenComp;
        private System.Windows.Forms.TextBox txt_Year;
        private System.Windows.Forms.TextBox txt_CompName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Password2;
        private System.Windows.Forms.TextBox txt_UserName2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_SysName;
        private System.Windows.Forms.PictureBox p_Logo;
        private System.Windows.Forms.PictureBox btnreturn;
    }
}