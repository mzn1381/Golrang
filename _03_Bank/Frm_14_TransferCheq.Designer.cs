namespace PCLOR._03_Bank
{
    partial class Frm_14_TransferCheq
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
            Janus.Windows.GridEX.GridEXLayout gridEX2_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEX2_Layout_0_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.Columns.Column0.ValueList.Item0.Image");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_14_TransferCheq));
            Janus.Windows.GridEX.GridEXLayout mlt_FirstBranch_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout mlt_Statuse_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout mlt_SecondBranch_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.table_035_ReceiptChequesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_01_Cash = new PCLOR._03_Bank.DataSet_01_Cash();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.mlt_FirstBranch = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.ribbonBarMergeContainer1 = new DevComponents.DotNetBar.RibbonBarMergeContainer();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.gridEXFieldChooserControl1 = new Janus.Windows.GridEX.GridEXFieldChooserControl();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.microChartItem1 = new DevComponents.DotNetBar.MicroChartItem();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mlt_Statuse = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mlt_SecondBranch = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label1 = new System.Windows.Forms.Label();
            this.table_035_ReceiptChequesTableAdapter = new PCLOR._03_Bank.DataSet_01_CashTableAdapters.Table_035_ReceiptChequesTableAdapter();
            this.tableAdapterManager = new PCLOR._03_Bank.DataSet_01_CashTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.table_035_ReceiptChequesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_01_Cash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_FirstBranch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.ribbonBarMergeContainer1.SuspendLayout();
            this.ribbonBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Statuse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_SecondBranch)).BeginInit();
            this.SuspendLayout();
            // 
            // table_035_ReceiptChequesBindingSource
            // 
            this.table_035_ReceiptChequesBindingSource.DataMember = "Table_035_ReceiptCheques";
            this.table_035_ReceiptChequesBindingSource.DataSource = this.dataSet_01_Cash;
            // 
            // dataSet_01_Cash
            // 
            this.dataSet_01_Cash.DataSetName = "DataSet_01_Cash";
            this.dataSet_01_Cash.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel0.Id = new System.Guid("b08fa1b0-d369-4751-8bcf-4bfa2790b2f4");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("b08fa1b0-d369-4751-8bcf-4bfa2790b2f4"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(778, 313), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("b08fa1b0-d369-4751-8bcf-4bfa2790b2f4"), new System.Drawing.Point(621, 372), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(621, 372);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(3, 86);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(778, 313);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "اطلاعات چک دریافتی";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX2);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 25);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(776, 287);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowDrop = true;
            this.gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX2.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX2.BackColor = System.Drawing.Color.White;
            this.gridEX2.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.Linen;
            this.gridEX2.CardHeaders = false;
            this.gridEX2.CardInnerSpacing = 9;
            this.gridEX2.CardViewGridlines = Janus.Windows.GridEX.CardViewGridlines.FieldsOnly;
            this.gridEX2.CenterSingleCard = false;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX2.ExpandableCards = false;
            this.gridEX2.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX2.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX2.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX2.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX2.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX2.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            gridEX2_Layout_0.IsCurrentLayout = true;
            gridEX2_Layout_0.Key = "PERP";
            gridEX2_Layout_0_Reference_0.Instance = ((object)(resources.GetObject("gridEX2_Layout_0_Reference_0.Instance")));
            gridEX2_Layout_0.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridEX2_Layout_0_Reference_0});
            gridEX2_Layout_0.LayoutString = resources.GetString("gridEX2_Layout_0.LayoutString");
            this.gridEX2.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX2_Layout_0});
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.NewRowEnterKeyBehavior = Janus.Windows.GridEX.NewRowEnterKeyBehavior.None;
            this.gridEX2.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX2.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.NewRowPosition = Janus.Windows.GridEX.NewRowPosition.BottomRow;
            this.gridEX2.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX2.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX2.RecordNavigator = true;
            this.gridEX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX2.RowFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.gridEX2.RowFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX2.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowIndex;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.SaveSettings = true;
            this.gridEX2.SettingsKey = "Form09_TurnDoc_Receive19";
            this.gridEX2.Size = new System.Drawing.Size(776, 287);
            this.gridEX2.TabIndex = 2;
            this.gridEX2.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX2.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX2.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // mlt_FirstBranch
            // 
            this.mlt_FirstBranch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_FirstBranch.AutoComplete = false;
            mlt_FirstBranch_DesignTimeLayout.LayoutString = resources.GetString("mlt_FirstBranch_DesignTimeLayout.LayoutString");
            this.mlt_FirstBranch.DesignTimeLayout = mlt_FirstBranch_DesignTimeLayout;
            this.mlt_FirstBranch.DisplayMember = "column02";
            this.mlt_FirstBranch.Location = new System.Drawing.Point(585, 31);
            this.mlt_FirstBranch.Name = "mlt_FirstBranch";
            this.mlt_FirstBranch.SelectedIndex = -1;
            this.mlt_FirstBranch.SelectedItem = null;
            this.mlt_FirstBranch.SettingsKey = "mlt_unit";
            this.mlt_FirstBranch.Size = new System.Drawing.Size(132, 21);
            this.mlt_FirstBranch.TabIndex = 5;
            this.mlt_FirstBranch.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.mlt_FirstBranch.ValueMember = "columnid";
            this.mlt_FirstBranch.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_FirstBranch.ValueChanged += new System.EventHandler(this.mlt_FirstBranch_ValueChanged);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.ribbonBarMergeContainer1);
            this.uiGroupBox1.Controls.Add(this.button2);
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.mlt_Statuse);
            this.uiGroupBox1.Controls.Add(this.button1);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Controls.Add(this.mlt_SecondBranch);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.mlt_FirstBranch);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(778, 83);
            this.uiGroupBox1.TabIndex = 5;
            // 
            // ribbonBarMergeContainer1
            // 
            this.ribbonBarMergeContainer1.AutoActivateTab = false;
            this.ribbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.ribbonBarMergeContainer1.Controls.Add(this.ribbonBar1);
            this.ribbonBarMergeContainer1.Location = new System.Drawing.Point(326, -2);
            this.ribbonBarMergeContainer1.MergeRibbonGroupName = "SettingTab";
            this.ribbonBarMergeContainer1.Name = "ribbonBarMergeContainer1";
            this.ribbonBarMergeContainer1.RibbonTabColorTable = DevComponents.DotNetBar.eRibbonTabColor.Magenta;
            this.ribbonBarMergeContainer1.RibbonTabText = "تنظیمات";
            this.ribbonBarMergeContainer1.Size = new System.Drawing.Size(211, 19);
            this.ribbonBarMergeContainer1.StretchLastRibbonBar = true;
            // 
            // 
            // 
            this.ribbonBarMergeContainer1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBarMergeContainer1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBarMergeContainer1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBarMergeContainer1.TabIndex = 23;
            this.ribbonBarMergeContainer1.Visible = false;
            // 
            // ribbonBar1
            // 
            this.ribbonBar1.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar1.ContainerControlProcessDialogKey = true;
            this.ribbonBar1.Controls.Add(this.gridEXFieldChooserControl1);
            this.ribbonBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.microChartItem1});
            this.ribbonBar1.Location = new System.Drawing.Point(0, 0);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.ResizeItemsToFit = false;
            this.ribbonBar1.Size = new System.Drawing.Size(211, 19);
            this.ribbonBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar1.TabIndex = 0;
            this.ribbonBar1.Text = "انتخاب ستونهای جدول";
            // 
            // 
            // 
            this.ribbonBar1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // gridEXFieldChooserControl1
            // 
            this.gridEXFieldChooserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEXFieldChooserControl1.GridEX = this.gridEX2;
            this.gridEXFieldChooserControl1.Location = new System.Drawing.Point(0, 0);
            this.gridEXFieldChooserControl1.Name = "gridEXFieldChooserControl1";
            this.gridEXFieldChooserControl1.Size = new System.Drawing.Size(211, 3);
            this.gridEXFieldChooserControl1.TabIndex = 2;
            this.gridEXFieldChooserControl1.Text = "gridEXFieldChooserControl1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItemsExpandWidth = 14;
            this.buttonItem1.Text = "buttonItem1";
            // 
            // buttonItem2
            // 
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.SubItemsExpandWidth = 14;
            this.buttonItem2.Text = "buttonItem2";
            // 
            // microChartItem1
            // 
            this.microChartItem1.Name = "microChartItem1";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(77, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "مشاهده";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(314, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "وضعیت چک:";
            // 
            // mlt_Statuse
            // 
            this.mlt_Statuse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Statuse.AutoComplete = false;
            mlt_Statuse_DesignTimeLayout.LayoutString = resources.GetString("mlt_Statuse_DesignTimeLayout.LayoutString");
            this.mlt_Statuse.DesignTimeLayout = mlt_Statuse_DesignTimeLayout;
            this.mlt_Statuse.DisplayMember = "column02";
            this.mlt_Statuse.Location = new System.Drawing.Point(145, 31);
            this.mlt_Statuse.Name = "mlt_Statuse";
            this.mlt_Statuse.SelectedIndex = -1;
            this.mlt_Statuse.SelectedItem = null;
            this.mlt_Statuse.SettingsKey = "mlt_unit";
            this.mlt_Statuse.Size = new System.Drawing.Size(169, 21);
            this.mlt_Statuse.TabIndex = 10;
            this.mlt_Statuse.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.mlt_Statuse.ValueMember = "columnid";
            this.mlt_Statuse.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_Statuse.ValueChanged += new System.EventHandler(this.mlt_Statuse_ValueChanged);
            this.mlt_Statuse.Leave += new System.EventHandler(this.mlt_Statuse_Leave);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(9, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "انتقال چک";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(515, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "شعبه مقصد:";
            // 
            // mlt_SecondBranch
            // 
            this.mlt_SecondBranch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_SecondBranch.AutoComplete = false;
            mlt_SecondBranch_DesignTimeLayout.LayoutString = resources.GetString("mlt_SecondBranch_DesignTimeLayout.LayoutString");
            this.mlt_SecondBranch.DesignTimeLayout = mlt_SecondBranch_DesignTimeLayout;
            this.mlt_SecondBranch.DisplayMember = "column02";
            this.mlt_SecondBranch.Location = new System.Drawing.Point(382, 31);
            this.mlt_SecondBranch.Name = "mlt_SecondBranch";
            this.mlt_SecondBranch.SelectedIndex = -1;
            this.mlt_SecondBranch.SelectedItem = null;
            this.mlt_SecondBranch.SettingsKey = "mlt_unit";
            this.mlt_SecondBranch.Size = new System.Drawing.Size(132, 21);
            this.mlt_SecondBranch.TabIndex = 7;
            this.mlt_SecondBranch.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.mlt_SecondBranch.ValueMember = "columnid";
            this.mlt_SecondBranch.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(718, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "شعبه مبدا:";
            // 
            // table_035_ReceiptChequesTableAdapter
            // 
            this.table_035_ReceiptChequesTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_020_BankCashAccInfoTableAdapter = null;
            this.tableAdapterManager.Table_035_ReceiptChequesTableAdapter = this.table_035_ReceiptChequesTableAdapter;
            this.tableAdapterManager.Table_040_CashPaymentsTableAdapter = null;
            this.tableAdapterManager.Table_045_ReceiveCashTableAdapter = null;
            this.tableAdapterManager.Table_065_TurnReceptionTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PCLOR._03_Bank.DataSet_01_CashTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // Frm_14_TransferCheq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 402);
            this.Controls.Add(this.uiGroupBox1);
            this.Controls.Add(this.uiPanel0);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_14_TransferCheq";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "انتقال بین شعب";
            this.Load += new System.EventHandler(this.Frm_14_TransferCheq_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table_035_ReceiptChequesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_01_Cash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.Configuration.IPersistComponentSettings)(this.gridEX2)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_FirstBranch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.ribbonBarMergeContainer1.ResumeLayout(false);
            this.ribbonBar1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Statuse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_SecondBranch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_SecondBranch;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_FirstBranch;
        private DataSet_01_Cash dataSet_01_Cash;
        private System.Windows.Forms.BindingSource table_035_ReceiptChequesBindingSource;
        private DataSet_01_CashTableAdapters.Table_035_ReceiptChequesTableAdapter table_035_ReceiptChequesTableAdapter;
        private DataSet_01_CashTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Statuse;
        private System.Windows.Forms.Button button2;
        private Janus.Windows.GridEX.GridEX gridEX2;
        private DevComponents.DotNetBar.RibbonBarMergeContainer ribbonBarMergeContainer1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private Janus.Windows.GridEX.GridEXFieldChooserControl gridEXFieldChooserControl1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.MicroChartItem microChartItem1;
    }
}