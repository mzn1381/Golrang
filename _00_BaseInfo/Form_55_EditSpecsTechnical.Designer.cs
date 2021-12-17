
namespace PCLOR._00_BaseInfo
{
    partial class Form_55_EditSpecsTechnical
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
            Janus.Windows.GridEX.GridEXLayout listFabricType_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_55_EditSpecsTechnical));
            Janus.Windows.GridEX.GridEXLayout listYarnType_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.txtDeciveMark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericGap = new System.Windows.Forms.NumericUpDown();
            this.Status = new System.Windows.Forms.CheckBox();
            this.listFabricType = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericArea = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.listYarnType = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label6 = new System.Windows.Forms.Label();
            this.numericTeeny = new System.Windows.Forms.NumericUpDown();
            this.btnExit = new Janus.Windows.EditControls.UIButton();
            this.label7 = new System.Windows.Forms.Label();
            this.numericRoundStop = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericTextureLimit = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listFabricType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listYarnType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTeeny)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRoundStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTextureLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDeciveMark
            // 
            this.txtDeciveMark.Location = new System.Drawing.Point(318, 46);
            this.txtDeciveMark.Name = "txtDeciveMark";
            this.txtDeciveMark.Size = new System.Drawing.Size(138, 26);
            this.txtDeciveMark.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(462, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "مارک دستگاه ";
            // 
            // numericGap
            // 
            this.numericGap.Location = new System.Drawing.Point(318, 91);
            this.numericGap.Name = "numericGap";
            this.numericGap.Size = new System.Drawing.Size(138, 26);
            this.numericGap.TabIndex = 2;
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(155, 207);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(18, 17);
            this.Status.TabIndex = 3;
            this.Status.UseVisualStyleBackColor = true;
            // 
            // listFabricType
            // 
            this.listFabricType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listFabricType.AutoComplete = false;
            listFabricType_DesignTimeLayout.LayoutString = resources.GetString("listFabricType_DesignTimeLayout.LayoutString");
            this.listFabricType.DesignTimeLayout = listFabricType_DesignTimeLayout;
            this.listFabricType.DisplayMember = "Column02";
            this.listFabricType.Location = new System.Drawing.Point(318, 144);
            this.listFabricType.Name = "listFabricType";
            this.listFabricType.SelectedIndex = -1;
            this.listFabricType.SelectedItem = null;
            this.listFabricType.SettingsKey = "mlt_unit";
            this.listFabricType.Size = new System.Drawing.Size(138, 26);
            this.listFabricType.TabIndex = 5;
            this.listFabricType.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.listFabricType.ValueMember = "Columnid";
            this.listFabricType.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(379, 310);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(127, 44);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "ثبت ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(462, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "دهنه ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(462, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "نوع بافت ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(462, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "متراژ";
            // 
            // numericArea
            // 
            this.numericArea.Location = new System.Drawing.Point(318, 254);
            this.numericArea.Name = "numericArea";
            this.numericArea.Size = new System.Drawing.Size(138, 26);
            this.numericArea.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(462, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 18);
            this.label5.TabIndex = 1;
            this.label5.Text = "نوع نخ ";
            // 
            // listYarnType
            // 
            this.listYarnType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listYarnType.AutoComplete = false;
            listYarnType_DesignTimeLayout.LayoutString = resources.GetString("listYarnType_DesignTimeLayout.LayoutString");
            this.listYarnType.DesignTimeLayout = listYarnType_DesignTimeLayout;
            this.listYarnType.DisplayMember = "Column02";
            this.listYarnType.Location = new System.Drawing.Point(318, 198);
            this.listYarnType.Name = "listYarnType";
            this.listYarnType.SelectedIndex = -1;
            this.listYarnType.SelectedItem = null;
            this.listYarnType.SettingsKey = "mlt_unit";
            this.listYarnType.Size = new System.Drawing.Size(138, 26);
            this.listYarnType.TabIndex = 5;
            this.listYarnType.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.listYarnType.ValueMember = "Columnid";
            this.listYarnType.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(179, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 18);
            this.label6.TabIndex = 1;
            this.label6.Text = "ریزی ";
            // 
            // numericTeeny
            // 
            this.numericTeeny.Location = new System.Drawing.Point(35, 46);
            this.numericTeeny.Name = "numericTeeny";
            this.numericTeeny.Size = new System.Drawing.Size(138, 26);
            this.numericTeeny.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(46, 310);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(127, 44);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "خروج ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(179, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 18);
            this.label7.TabIndex = 1;
            this.label7.Text = "دور تا استوپ";
            // 
            // numericRoundStop
            // 
            this.numericRoundStop.Location = new System.Drawing.Point(35, 93);
            this.numericRoundStop.Name = "numericRoundStop";
            this.numericRoundStop.Size = new System.Drawing.Size(138, 26);
            this.numericRoundStop.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(179, 150);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 18);
            this.label8.TabIndex = 1;
            this.label8.Text = "حد بافت ";
            // 
            // numericTextureLimit
            // 
            this.numericTextureLimit.Location = new System.Drawing.Point(35, 147);
            this.numericTextureLimit.Name = "numericTextureLimit";
            this.numericTextureLimit.Size = new System.Drawing.Size(138, 26);
            this.numericTextureLimit.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(179, 205);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 18);
            this.label9.TabIndex = 1;
            this.label9.Text = "وضعیت ";
            // 
            // Form_55_EditSpecsTechnical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(584, 368);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.listYarnType);
            this.Controls.Add(this.listFabricType);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.numericTextureLimit);
            this.Controls.Add(this.numericRoundStop);
            this.Controls.Add(this.numericTeeny);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numericArea);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericGap);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDeciveMark);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form_55_EditSpecsTechnical";
            this.Text = "ویرایش و مشخصات دستگاه ";
            this.Load += new System.EventHandler(this.Form_55_EditSpecsTechnical_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listFabricType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listYarnType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTeeny)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRoundStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTextureLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDeciveMark;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericGap;
        private System.Windows.Forms.CheckBox Status;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo listFabricType;
        private Janus.Windows.EditControls.UIButton btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericArea;
        private System.Windows.Forms.Label label5;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo listYarnType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericTeeny;
        private Janus.Windows.EditControls.UIButton btnExit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericRoundStop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericTextureLimit;
        private System.Windows.Forms.Label label9;
    }
}