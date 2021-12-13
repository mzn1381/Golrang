namespace PCLOR._002_Sale
{
    partial class Frm_004_UpdateBrand
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
            Janus.Windows.GridEX.GridEXLayout mlt_Color_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_004_UpdateBrand));
            Janus.Windows.GridEX.GridEXLayout mlt_Cloth_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.mlt_Color = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.txt_Fi = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mlt_Cloth = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Cloth)).BeginInit();
            this.SuspendLayout();
            // 
            // mlt_Color
            // 
            this.mlt_Color.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Color_DesignTimeLayout.LayoutString = resources.GetString("mlt_Color_DesignTimeLayout.LayoutString");
            this.mlt_Color.DesignTimeLayout = mlt_Color_DesignTimeLayout;
            this.mlt_Color.DisplayMember = "TypeColor";
            this.mlt_Color.Location = new System.Drawing.Point(12, 11);
            this.mlt_Color.Name = "mlt_Color";
            this.mlt_Color.SelectedIndex = -1;
            this.mlt_Color.SelectedItem = null;
            this.mlt_Color.SettingsKey = "mlt_Id";
            this.mlt_Color.Size = new System.Drawing.Size(235, 21);
            this.mlt_Color.TabIndex = 0;
            this.mlt_Color.ValueMember = "ID";
            this.mlt_Color.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_Color.ValueChanged += new System.EventHandler(this.Mlt_Color_ValueChanged);
            this.mlt_Color.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Color_KeyPress);
            // 
            // txt_Fi
            // 
            this.txt_Fi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Fi.Location = new System.Drawing.Point(12, 61);
            this.txt_Fi.Name = "txt_Fi";
            this.txt_Fi.Size = new System.Drawing.Size(235, 21);
            this.txt_Fi.TabIndex = 2;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(100, 85);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 29);
            this.btn_Save.TabIndex = 3;
            this.btn_Save.Text = "به روزرسانی";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "نوع رنگ:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "قیمت جدید:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "نوع پارچه:";
            // 
            // mlt_Cloth
            // 
            this.mlt_Cloth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Cloth_DesignTimeLayout.LayoutString = resources.GetString("mlt_Cloth_DesignTimeLayout.LayoutString");
            this.mlt_Cloth.DesignTimeLayout = mlt_Cloth_DesignTimeLayout;
            this.mlt_Cloth.DisplayMember = "TypeCloth";
            this.mlt_Cloth.Location = new System.Drawing.Point(12, 37);
            this.mlt_Cloth.Name = "mlt_Cloth";
            this.mlt_Cloth.SelectedIndex = -1;
            this.mlt_Cloth.SelectedItem = null;
            this.mlt_Cloth.SettingsKey = "mlt_Id";
            this.mlt_Cloth.Size = new System.Drawing.Size(235, 21);
            this.mlt_Cloth.TabIndex = 1;
            this.mlt_Cloth.ValueMember = "ID";
            this.mlt_Cloth.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_Cloth.ValueChanged += new System.EventHandler(this.Mlt_Cloth_ValueChanged);
            this.mlt_Cloth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mlt_Cloth_KeyPress);
            // 
            // Frm_004_UpdateBrand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 116);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mlt_Cloth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txt_Fi);
            this.Controls.Add(this.mlt_Color);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "Frm_004_UpdateBrand";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "به روز رسانی قیمت";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_004_UpdateBrand_FormClosing);
            this.Load += new System.EventHandler(this.Frm_004_UpdateBrand_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Cloth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Fi;
        private System.Windows.Forms.Button btn_Save;
        public Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Color;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Cloth;
    }
}