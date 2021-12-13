namespace PCLOR.MarjooiSale
{
    partial class Frm_014_ResidDialog_Return
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
            System.Windows.Forms.Label column04Label;
            System.Windows.Forms.Label column03Label;
            Janus.Windows.GridEX.GridEXLayout mlt_Ware_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_014_ResidDialog_Return));
            Janus.Windows.GridEX.GridEXLayout mlt_Function_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.mlt_Ware = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.mlt_Function = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.txt_DraftNum = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.chk_DraftNum = new Janus.Windows.EditControls.UICheckBox();
            column04Label = new System.Windows.Forms.Label();
            column03Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Ware)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Function)).BeginInit();
            this.SuspendLayout();
            // 
            // column04Label
            // 
            column04Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column04Label.AutoSize = true;
            column04Label.BackColor = System.Drawing.Color.Transparent;
            column04Label.Location = new System.Drawing.Point(346, 45);
            column04Label.Name = "column04Label";
            column04Label.Size = new System.Drawing.Size(54, 13);
            column04Label.TabIndex = 57;
            column04Label.Text = "نوع رسید:";
            // 
            // column03Label
            // 
            column03Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column03Label.AutoSize = true;
            column03Label.BackColor = System.Drawing.Color.Transparent;
            column03Label.Location = new System.Drawing.Point(372, 17);
            column03Label.Name = "column03Label";
            column03Label.Size = new System.Drawing.Size(28, 13);
            column03Label.TabIndex = 61;
            column03Label.Text = "انبار:";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.uiGroupBox1.Controls.Add(this.txt_DraftNum);
            this.uiGroupBox1.Controls.Add(this.chk_DraftNum);
            this.uiGroupBox1.Controls.Add(this.mlt_Ware);
            this.uiGroupBox1.Controls.Add(column03Label);
            this.uiGroupBox1.Controls.Add(this.uiButton2);
            this.uiGroupBox1.Controls.Add(this.uiButton1);
            this.uiGroupBox1.Controls.Add(this.mlt_Function);
            this.uiGroupBox1.Controls.Add(column04Label);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox1.OfficeCustomColor = System.Drawing.Color.Olive;
            this.uiGroupBox1.Size = new System.Drawing.Size(412, 147);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            // 
            // mlt_Ware
            // 
            this.mlt_Ware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Ware_DesignTimeLayout.LayoutString = resources.GetString("mlt_Ware_DesignTimeLayout.LayoutString");
            this.mlt_Ware.DesignTimeLayout = mlt_Ware_DesignTimeLayout;
            this.mlt_Ware.DisplayMember = "column02";
            this.mlt_Ware.Location = new System.Drawing.Point(12, 12);
            this.mlt_Ware.Name = "mlt_Ware";
            this.mlt_Ware.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.mlt_Ware.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.mlt_Ware.SelectedIndex = -1;
            this.mlt_Ware.SelectedItem = null;
            this.mlt_Ware.SelectInDataSource = true;
            this.mlt_Ware.Size = new System.Drawing.Size(354, 21);
            this.mlt_Ware.TabIndex = 0;
            this.mlt_Ware.ValueMember = "columnid";
            this.mlt_Ware.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.mlt_Ware.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Function_KeyPress);
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(12, 110);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiButton2.OfficeCustomColor = System.Drawing.Color.YellowGreen;
            this.uiButton2.Size = new System.Drawing.Size(175, 23);
            this.uiButton2.TabIndex = 5;
            this.uiButton2.Text = "لغو    Ctrl+Q";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(222, 110);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiButton1.OfficeCustomColor = System.Drawing.Color.YellowGreen;
            this.uiButton1.Size = new System.Drawing.Size(175, 23);
            this.uiButton1.TabIndex = 4;
            this.uiButton1.Text = "تأیید    Ctrl+S";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // mlt_Function
            // 
            this.mlt_Function.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Function_DesignTimeLayout.LayoutString = resources.GetString("mlt_Function_DesignTimeLayout.LayoutString");
            this.mlt_Function.DesignTimeLayout = mlt_Function_DesignTimeLayout;
            this.mlt_Function.DisplayMember = "column02";
            this.mlt_Function.Location = new System.Drawing.Point(12, 40);
            this.mlt_Function.Name = "mlt_Function";
            this.mlt_Function.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.mlt_Function.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.mlt_Function.SelectedIndex = -1;
            this.mlt_Function.SelectedItem = null;
            this.mlt_Function.Size = new System.Drawing.Size(328, 21);
            this.mlt_Function.TabIndex = 1;
            this.mlt_Function.ValueMember = "columnid";
            this.mlt_Function.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.mlt_Function.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Function_KeyPress);
            // 
            // txt_DraftNum
            // 
            this.txt_DraftNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_DraftNum.Location = new System.Drawing.Point(12, 67);
            this.txt_DraftNum.Name = "txt_DraftNum";
            this.txt_DraftNum.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_DraftNum.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_DraftNum.Size = new System.Drawing.Size(266, 21);
            this.txt_DraftNum.TabIndex = 3;
            this.txt_DraftNum.Text = "0";
            this.txt_DraftNum.Value = 0;
            this.txt_DraftNum.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int32;
            this.txt_DraftNum.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // chk_DraftNum
            // 
            this.chk_DraftNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_DraftNum.BackColor = System.Drawing.Color.Transparent;
            this.chk_DraftNum.Location = new System.Drawing.Point(293, 67);
            this.chk_DraftNum.Name = "chk_DraftNum";
            this.chk_DraftNum.Size = new System.Drawing.Size(104, 23);
            this.chk_DraftNum.TabIndex = 2;
            this.chk_DraftNum.Text = "شماره رسید دلخواه";
            this.chk_DraftNum.CheckedChanged += new System.EventHandler(this.chk_DraftNum_CheckedChanged);
            // 
            // Frm_014_ResidDialog_Return
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 147);
            this.ControlBox = false;
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Frm_014_ResidDialog_Return";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "اطلاعات رسید انبار";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_010_DraftInformationDialog_FormClosing);
            this.Load += new System.EventHandler(this.Frm_010_DraftInformationDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_014_ResidDialog_Return_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Ware)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Function)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Function;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Ware;
        private Janus.Windows.GridEX.EditControls.NumericEditBox txt_DraftNum;
        private Janus.Windows.EditControls.UICheckBox chk_DraftNum;
    }
}