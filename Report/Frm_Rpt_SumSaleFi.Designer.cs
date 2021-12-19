namespace PCLOR.Report
{
    partial class Frm_Rpt_SumSaleFi
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
            Janus.Windows.GridEX.GridEXLayout gridEX2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Rpt_SumSaleFi));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsexcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDate1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.faDate2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.btn_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.check_B = new System.Windows.Forms.CheckBox();
            this.check_T = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel0.Id = new System.Guid("6f7a19f7-8f0f-42c4-9742-cd6d8d577700");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("6f7a19f7-8f0f-42c4-9742-cd6d8d577700"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(295, 474), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("6f7a19f7-8f0f-42c4-9742-cd6d8d577700"), new System.Drawing.Point(758, 439), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(758, 439);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(528, 30);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(295, 474);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "اطلاعات کالا";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX2);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 24);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(289, 449);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX2.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.BuiltInTextsData = resources.GetString("gridEX2.BuiltInTextsData");
            this.gridEX2.CardWidth = 751;
            this.gridEX2.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            gridEX2_DesignTimeLayout.LayoutString = resources.GetString("gridEX2_DesignTimeLayout.LayoutString");
            this.gridEX2.DesignTimeLayout = gridEX2_DesignTimeLayout;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.None;
            this.gridEX2.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX2.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX2.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX2.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX2.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX2.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.SettingsKey = "reportordr18";
            this.gridEX2.Size = new System.Drawing.Size(289, 449);
            this.gridEX2.TabIndex = 40;
            this.gridEX2.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Red;
            this.gridEX2.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX2.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridEX2_FormattingRow);
            this.gridEX2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridEX2_KeyPress);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bindingNavigator1.BackgroundImage")));
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripSeparator6,
            this.tsexcel,
            this.toolStripLabel1,
            this.faDate1,
            this.toolStripSeparator5,
            this.toolStripLabel2,
            this.toolStripSeparator1,
            this.faDate2,
            this.btn_Search,
            this.toolStripSeparator3});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(826, 27);
            this.bindingNavigator1.TabIndex = 32;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(56, 24);
            this.toolStripButton2.Text = "چاپ";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // tsexcel
            // 
            this.tsexcel.Image = ((System.Drawing.Image)(resources.GetObject("tsexcel.Image")));
            this.tsexcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsexcel.Name = "tsexcel";
            this.tsexcel.Size = new System.Drawing.Size(122, 24);
            this.tsexcel.Text = "ارسال به اکسل";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(47, 24);
            this.toolStripLabel1.Text = "از تاریخ";
            // 
            // faDate1
            // 
            this.faDate1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDate1.BackColor = System.Drawing.SystemColors.Window;
            this.faDate1.Name = "faDate1";
            this.faDate1.Size = new System.Drawing.Size(120, 24);
            this.faDate1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDate1_KeyPress);
            this.faDate1.TextChanged += new System.EventHandler(this.faDate1_TextChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(47, 24);
            this.toolStripLabel2.Text = "تا تاریخ";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // faDate2
            // 
            this.faDate2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDate2.BackColor = System.Drawing.SystemColors.Window;
            this.faDate2.Name = "faDate2";
            this.faDate2.Size = new System.Drawing.Size(120, 24);
            this.faDate2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDate2_KeyPress);
            this.faDate2.TextChanged += new System.EventHandler(this.faDate2_TextChanged);
            // 
            // btn_Search
            // 
            this.btn_Search.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_Search.Image = ((System.Drawing.Image)(resources.GetObject("btn_Search.Image")));
            this.btn_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(29, 24);
            this.btn_Search.ToolTipText = "جستجو";
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
            this.gridEX1.CardWidth = 751;
            this.gridEX1.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.Location = new System.Drawing.Point(3, 30);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SaveSettings = true;
            this.gridEX1.SettingsKey = "reportordr36";
            this.gridEX1.Size = new System.Drawing.Size(525, 474);
            this.gridEX1.TabIndex = 39;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Red;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // check_B
            // 
            this.check_B.AutoSize = true;
            this.check_B.BackgroundImage = global::PCLOR.Properties.Resources.me_bg1;
            this.check_B.Location = new System.Drawing.Point(256, 5);
            this.check_B.Name = "check_B";
            this.check_B.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.check_B.Size = new System.Drawing.Size(52, 21);
            this.check_B.TabIndex = 40;
            this.check_B.Text = "برند";
            this.check_B.UseVisualStyleBackColor = true;
            // 
            // check_T
            // 
            this.check_T.AutoSize = true;
            this.check_T.BackgroundImage = global::PCLOR.Properties.Resources.me_bg1;
            this.check_T.Location = new System.Drawing.Point(173, 5);
            this.check_T.Name = "check_T";
            this.check_T.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.check_T.Size = new System.Drawing.Size(97, 21);
            this.check_T.TabIndex = 41;
            this.check_T.Text = "تامین کننده";
            this.check_T.UseVisualStyleBackColor = true;
            // 
            // Frm_Rpt_SumSaleFi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 507);
            this.Controls.Add(this.check_T);
            this.Controls.Add(this.check_B);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_Rpt_SumSaleFi";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزار تجمیعی فروش بر اساس کالا";
            this.Load += new System.EventHandler(this.Frm_Rpt_SumSaleFi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX1)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsexcel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDate1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDate2;
        public System.Windows.Forms.ToolStripButton btn_Search;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX2;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private System.Windows.Forms.CheckBox check_B;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.CheckBox check_T;
    }
}