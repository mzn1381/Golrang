namespace PCLOR._00_BaseInfo
{
    partial class Setting_15_Weighbridge
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
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout mlt_Print_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting_15_Weighbridge));
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.mlt_Print = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.txt_Bund = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataSet_05_PCLOR = new PCLOR.data_PCLOR.DataSet_05_PCLOR();
            this.table_80_SettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_80_SettingTableAdapter = new PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.Table_80_SettingTableAdapter();
            this.tableAdapterManager = new PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Print)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_PCLOR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_80_SettingBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.LightGray;
            this.uiGroupBox1.Controls.Add(this.mlt_Print);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.button1);
            this.uiGroupBox1.Controls.Add(this.txt_Port);
            this.uiGroupBox1.Controls.Add(this.txt_Bund);
            this.uiGroupBox1.Controls.Add(this.label10);
            this.uiGroupBox1.Controls.Add(this.label5);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(436, 121);
            this.uiGroupBox1.TabIndex = 99;
            this.uiGroupBox1.Text = "اطلاعات باسکول";
            // 
            // mlt_Print
            // 
            this.mlt_Print.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Print.AutoComplete = false;
            mlt_Print_DesignTimeLayout.LayoutString = resources.GetString("mlt_Print_DesignTimeLayout.LayoutString");
            this.mlt_Print.DesignTimeLayout = mlt_Print_DesignTimeLayout;
            this.mlt_Print.DisplayMember = "Name";
            this.mlt_Print.Location = new System.Drawing.Point(83, 65);
            this.mlt_Print.Name = "mlt_Print";
            this.mlt_Print.SelectedIndex = -1;
            this.mlt_Print.SelectedItem = null;
            this.mlt_Print.SettingsKey = "mlt_Id";
            this.mlt_Print.Size = new System.Drawing.Size(236, 21);
            this.mlt_Print.TabIndex = 128;
            this.mlt_Print.ValueMember = "Name";
            this.mlt_Print.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(325, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Printer:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MistyRose;
            this.button1.Location = new System.Drawing.Point(135, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 25);
            this.button1.TabIndex = 6;
            this.button1.Text = "ذخیره";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_Port
            // 
            this.txt_Port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Port.Location = new System.Drawing.Point(83, 38);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(236, 21);
            this.txt_Port.TabIndex = 5;
            // 
            // txt_Bund
            // 
            this.txt_Bund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Bund.Location = new System.Drawing.Point(83, 11);
            this.txt_Bund.Name = "txt_Bund";
            this.txt_Bund.Size = new System.Drawing.Size(236, 21);
            this.txt_Bund.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(324, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "PortName:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(325, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "BaudRate:";
            // 
            // dataSet_05_PCLOR
            // 
            this.dataSet_05_PCLOR.DataSetName = "DataSet_05_PCLOR";
            this.dataSet_05_PCLOR.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_80_SettingBindingSource
            // 
            this.table_80_SettingBindingSource.DataMember = "Table_80_Setting";
            this.table_80_SettingBindingSource.DataSource = this.dataSet_05_PCLOR;
            // 
            // table_80_SettingTableAdapter
            // 
            this.table_80_SettingTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_005_TypeClothTableAdapter = null;
            this.tableAdapterManager.Table_010_TypeColorTableAdapter = null;
            this.tableAdapterManager.Table_015_FormulColorTableAdapter = null;
            this.tableAdapterManager.Table_020_DetailReciptClothRawTableAdapter = null;
            this.tableAdapterManager.Table_020_HeaderReciptClothRowTableAdapter = null;
            this.tableAdapterManager.Table_025_HederOrderColorTableAdapter = null;
            this.tableAdapterManager.Table_030_DetailOrderColorTableAdapter = null;
            this.tableAdapterManager.Table_035_ProductionTableAdapter = null;
            this.tableAdapterManager.Table_050_Packaging1TableAdapter = null;
            this.tableAdapterManager.Table_050_PackagingTableAdapter = null;
            this.tableAdapterManager.Table_055_ColorDefinitionTableAdapter = null;
            this.tableAdapterManager.Table_40_ColorPrductionTableAdapter = null;
            this.tableAdapterManager.Table_60_SpecsTechnicalTableAdapter = null;
            this.tableAdapterManager.Table_65_HeaderOtherPWHRS1TableAdapter = null;
            this.tableAdapterManager.Table_65_HeaderOtherPWHRSTableAdapter = null;
            this.tableAdapterManager.Table_70_DetailOtherPWHRS1TableAdapter = null;
            this.tableAdapterManager.Table_70_DetailOtherPWHRSTableAdapter = null;
            this.tableAdapterManager.Table_80_SettingTableAdapter = this.table_80_SettingTableAdapter;
            this.tableAdapterManager.Table_85_BranchsTableAdapter = null;
            this.tableAdapterManager.Table_90_Wares1TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares2TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares3TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares4TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares5TableAdapter = null;
            this.tableAdapterManager.Table_90_WaresTableAdapter = null;
            this.tableAdapterManager.Table_95_DetailWareTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // Setting_15_Weighbridge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 127);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "Setting_15_Weighbridge";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تنظیمات ";
            this.Load += new System.EventHandler(this.Setting_15_Weighbridge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Print)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_PCLOR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_80_SettingBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.TextBox txt_Bund;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource table_80_SettingBindingSource;
        private data_PCLOR.DataSet_05_PCLOR dataSet_05_PCLOR;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.Table_80_SettingTableAdapter table_80_SettingTableAdapter;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Print;
    }
}