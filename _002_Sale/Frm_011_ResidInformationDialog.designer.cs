namespace PCLOR._002_Sale
{
    partial class Frm_011_ResidInformationDialog
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
            Janus.Windows.GridEX.GridEXLayout mlt_Function_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_011_ResidInformationDialog));
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.mlt_Function = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            column04Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Function)).BeginInit();
            this.SuspendLayout();
            // 
            // column04Label
            // 
            column04Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column04Label.AutoSize = true;
            column04Label.BackColor = System.Drawing.Color.Transparent;
            column04Label.Location = new System.Drawing.Point(335, 17);
            column04Label.Name = "column04Label";
            column04Label.Size = new System.Drawing.Size(54, 13);
            column04Label.TabIndex = 57;
            column04Label.Text = "نوع رسید:";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.uiGroupBox1.Controls.Add(this.uiButton2);
            this.uiGroupBox1.Controls.Add(this.uiButton1);
            this.uiGroupBox1.Controls.Add(this.mlt_Function);
            this.uiGroupBox1.Controls.Add(column04Label);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox1.OfficeCustomColor = System.Drawing.Color.LightSeaGreen;
            this.uiGroupBox1.Size = new System.Drawing.Size(412, 79);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(12, 42);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiButton2.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.uiButton2.Size = new System.Drawing.Size(175, 23);
            this.uiButton2.TabIndex = 59;
            this.uiButton2.Text = "لغو    Ctrl+Q";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(222, 42);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiButton1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.uiButton1.Size = new System.Drawing.Size(175, 23);
            this.uiButton1.TabIndex = 58;
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
            this.mlt_Function.Location = new System.Drawing.Point(12, 12);
            this.mlt_Function.Name = "mlt_Function";
            this.mlt_Function.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
            this.mlt_Function.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.mlt_Function.SelectedIndex = -1;
            this.mlt_Function.SelectedItem = null;
            this.mlt_Function.Size = new System.Drawing.Size(317, 21);
            this.mlt_Function.TabIndex = 55;
            this.mlt_Function.ValueMember = "columnid";
            this.mlt_Function.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.mlt_Function.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Function_KeyPress);
            // 
            // Frm_011_ResidInformationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 79);
            this.ControlBox = false;
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Frm_011_ResidInformationDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "اطلاعات رسید انبار";
            this.Load += new System.EventHandler(this.Frm_010_DraftInformationDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_011_ResidInformationDialog_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Function)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Function;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
    }
}