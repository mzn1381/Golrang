
namespace PCLOR._01_OperationInfo
{
    partial class Frm_65_Transfer_Barcode
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
            Janus.Windows.GridEX.GridEXLayout gridEX4_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout menuStoresDestination_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout menuStoresStart_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_65_Transfer_Barcode));
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX4 = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStoresDestination = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.checkRegAuto = new Janus.Windows.EditControls.UICheckBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.txtStoreName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStoresStart = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuStoresDestination)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuStoresStart)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanel0.Id = new System.Guid("da7045e2-8dbe-43b0-b262-2bed3cc27079");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            this.uiPanel1.Id = new System.Guid("38cef559-5409-431b-80ba-1e13343f4c79");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("da7045e2-8dbe-43b0-b262-2bed3cc27079"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(510, 460), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("38cef559-5409-431b-80ba-1e13343f4c79"), Janus.Windows.UI.Dock.PanelDockStyle.Left, new System.Drawing.Size(296, 460), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("da7045e2-8dbe-43b0-b262-2bed3cc27079"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("38cef559-5409-431b-80ba-1e13343f4c79"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(296, 3);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(510, 460);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "بارکدها";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX4);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 25);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(504, 434);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX4
            // 
            this.gridEX4.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX4.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX4.AlternatingColors = true;
            this.gridEX4.BuiltInTextsData = resources.GetString("gridEX4.BuiltInTextsData");
            this.gridEX4.ColumnAutoSizeMode = Janus.Windows.GridEX.ColumnAutoSizeMode.DisplayedCellsAndHeader;
            gridEX4_DesignTimeLayout.LayoutString = resources.GetString("gridEX4_DesignTimeLayout.LayoutString");
            this.gridEX4.DesignTimeLayout = gridEX4_DesignTimeLayout;
            this.gridEX4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX4.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX4.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX4.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX4.FilterRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX4.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.LavenderBlush;
            this.gridEX4.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX4.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX4.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX4.GroupByBoxVisible = false;
            this.gridEX4.Location = new System.Drawing.Point(0, 0);
            this.gridEX4.Name = "gridEX4";
            this.gridEX4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX4.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX4.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX4.SaveSettings = true;
            this.gridEX4.ScrollBars = Janus.Windows.GridEX.ScrollBars.Both;
            this.gridEX4.SettingsKey = "Frm_15_InfoServiceGrid_61";
            this.gridEX4.Size = new System.Drawing.Size(504, 434);
            this.gridEX4.TabIndex = 1;
            this.gridEX4.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX4.TotalRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX4.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX4.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX4.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX4.UseCompatibleTextRendering = false;
            this.gridEX4.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // uiPanel1
            // 
            this.uiPanel1.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel1.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 3);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(296, 460);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "مشخصات";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.groupBox1);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 25);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(290, 434);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.menuStoresStart);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.menuStoresDestination);
            this.groupBox1.Controls.Add(this.checkRegAuto);
            this.groupBox1.Controls.Add(this.btnTransfer);
            this.groupBox1.Controls.Add(this.txtStoreName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDeviceName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 434);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "مشخصات بارکد";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(211, 235);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 18);
            this.label6.TabIndex = 205;
            this.label6.Text = "انبار مبدا";
            // 
            // menuStoresDestination
            // 
            this.menuStoresDestination.AutoComplete = false;
            this.menuStoresDestination.BorderStyle = Janus.Windows.GridEX.BorderStyle.SunkenLight3D;
            this.menuStoresDestination.Cursor = System.Windows.Forms.Cursors.Default;
            menuStoresDestination_DesignTimeLayout.LayoutString = resources.GetString("menuStoresDestination_DesignTimeLayout.LayoutString");
            this.menuStoresDestination.DesignTimeLayout = menuStoresDestination_DesignTimeLayout;
            this.menuStoresDestination.DisplayMember = "StoreName";
            this.menuStoresDestination.Location = new System.Drawing.Point(6, 277);
            this.menuStoresDestination.Name = "menuStoresDestination";
            this.menuStoresDestination.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
            this.menuStoresDestination.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.menuStoresDestination.SelectedIndex = -1;
            this.menuStoresDestination.SelectedItem = null;
            this.menuStoresDestination.SelectInDataSource = true;
            this.menuStoresDestination.Size = new System.Drawing.Size(193, 26);
            this.menuStoresDestination.TabIndex = 204;
            this.menuStoresDestination.ValueMember = "StoreCode";
            this.menuStoresDestination.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // checkRegAuto
            // 
            this.checkRegAuto.Location = new System.Drawing.Point(161, 335);
            this.checkRegAuto.Name = "checkRegAuto";
            this.checkRegAuto.Size = new System.Drawing.Size(24, 28);
            this.checkRegAuto.TabIndex = 203;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(6, 377);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(269, 46);
            this.btnTransfer.TabIndex = 202;
            this.btnTransfer.Text = "انتقال بارکد";
            this.btnTransfer.UseVisualStyleBackColor = true;
            // 
            // txtStoreName
            // 
            this.txtStoreName.Location = new System.Drawing.Point(6, 177);
            this.txtStoreName.Name = "txtStoreName";
            this.txtStoreName.ReadOnly = true;
            this.txtStoreName.Size = new System.Drawing.Size(186, 26);
            this.txtStoreName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 341);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "ثبت اتوماتیک";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(211, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "انبار مقصد";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "انبار فعلی";
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(6, 115);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.ReadOnly = true;
            this.txtDeviceName.Size = new System.Drawing.Size(186, 26);
            this.txtDeviceName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "نام دستگاه";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(6, 53);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.ReadOnly = true;
            this.txtBarcode.Size = new System.Drawing.Size(186, 26);
            this.txtBarcode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "شماره بارکد";
            // 
            // menuStoresStart
            // 
            this.menuStoresStart.AutoComplete = false;
            this.menuStoresStart.BorderStyle = Janus.Windows.GridEX.BorderStyle.SunkenLight3D;
            this.menuStoresStart.Cursor = System.Windows.Forms.Cursors.Default;
            menuStoresStart_DesignTimeLayout.LayoutString = resources.GetString("menuStoresStart_DesignTimeLayout.LayoutString");
            this.menuStoresStart.DesignTimeLayout = menuStoresStart_DesignTimeLayout;
            this.menuStoresStart.DisplayMember = "StoreName";
            this.menuStoresStart.Location = new System.Drawing.Point(6, 235);
            this.menuStoresStart.Name = "menuStoresStart";
            this.menuStoresStart.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
            this.menuStoresStart.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.menuStoresStart.SelectedIndex = -1;
            this.menuStoresStart.SelectedItem = null;
            this.menuStoresStart.SelectInDataSource = true;
            this.menuStoresStart.Size = new System.Drawing.Size(193, 26);
            this.menuStoresStart.TabIndex = 206;
            this.menuStoresStart.ValueMember = "StoreCode";
            this.menuStoresStart.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // Frm_65_Transfer_Barcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 466);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.uiPanel0);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_65_Transfer_Barcode";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Frm_65_Transfer_Barcode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.Configuration.IPersistComponentSettings)(this.gridEX4)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuStoresDestination)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuStoresStart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtStoreName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UICheckBox checkRegAuto;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo menuStoresDestination;
        private Janus.Windows.GridEX.GridEX gridEX4;
        private System.Windows.Forms.Label label6;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo menuStoresStart;
    }
}