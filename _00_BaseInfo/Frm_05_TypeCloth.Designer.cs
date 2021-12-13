namespace PCLOR._00_BaseInfo
{
    partial class Frm_05_TypeCloth
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label partNameLabel;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_05_TypeCloth));
            Janus.Windows.GridEX.GridEXLayout mlt_Commodity_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.table_005_TypeClothBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_05_PCLOR1 = new PCLOR.data_PCLOR.DataSet_05_PCLOR();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btn_New = new System.Windows.Forms.ToolStripButton();
            this.btn_Save = new System.Windows.Forms.ToolStripButton();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.txt_Fi = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.mlt_Commodity = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtTitleCloth = new System.Windows.Forms.TextBox();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.tableAdapterManager1 = new PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager();
            this.table_005_TypeClothTableAdapter = new PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.Table_005_TypeClothTableAdapter();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.فعالکردنپنلToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label2 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            partNameLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_005_TypeClothBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_PCLOR1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Commodity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(206, 105);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 17);
            label2.TabIndex = 19;
            label2.Text = "کد کالا : ";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(177, 22);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(96, 17);
            label5.TabIndex = 17;
            label5.Text = "شناسه پارچه :";
            // 
            // partNameLabel
            // 
            partNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            partNameLabel.AutoSize = true;
            partNameLabel.Location = new System.Drawing.Point(186, 63);
            partNameLabel.Name = "partNameLabel";
            partNameLabel.Size = new System.Drawing.Size(82, 17);
            partNameLabel.TabIndex = 18;
            partNameLabel.Text = "عنوان پارچه :";
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(206, 150);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(58, 17);
            label1.TabIndex = 20;
            label1.Text = "قیمت  : ";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bindingNavigator1.BackgroundImage")));
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.BindingSource = this.table_005_TypeClothBindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.btn_Delete,
            this.toolStripSeparator,
            this.btn_New,
            this.btn_Save});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(624, 31);
            this.bindingNavigator1.TabIndex = 1;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // table_005_TypeClothBindingSource
            // 
            this.table_005_TypeClothBindingSource.DataMember = "Table_005_TypeCloth";
            this.table_005_TypeClothBindingSource.DataSource = this.dataSet_05_PCLOR1;
            this.table_005_TypeClothBindingSource.PositionChanged += new System.EventHandler(this.table_005_TypeClothBindingSource_PositionChanged);
            // 
            // dataSet_05_PCLOR1
            // 
            this.dataSet_05_PCLOR1.DataSetName = "DataSet_05_PCLOR";
            this.dataSet_05_PCLOR1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(46, 28);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(29, 28);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(29, 28);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(29, 28);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(29, 28);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_Delete.Image")));
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.RightToLeftAutoMirrorImage = true;
            this.btn_Delete.Size = new System.Drawing.Size(61, 28);
            this.btn_Delete.Text = "حذف";
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // btn_New
            // 
            this.btn_New.Image = ((System.Drawing.Image)(resources.GetObject("btn_New.Image")));
            this.btn_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(61, 28);
            this.btn_New.Text = "جدید";
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(66, 28);
            this.btn_Save.Text = "ذخیره";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel0.Id = new System.Guid("3be028f0-ec60-419d-9cfa-749f21564d8a");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            this.uiPanel1.Id = new System.Guid("064803d1-d7a2-4f2a-8eb6-068986ca2d62");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("064803d1-d7a2-4f2a-8eb6-068986ca2d62"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(357, 341), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("3be028f0-ec60-419d-9cfa-749f21564d8a"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(261, 341), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("064803d1-d7a2-4f2a-8eb6-068986ca2d62"), new System.Drawing.Point(363, 300), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("3be028f0-ec60-419d-9cfa-749f21564d8a"), new System.Drawing.Point(769, 339), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(769, 339);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(360, 34);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(261, 341);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "اطلاعات نوع پارچه";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.txt_Fi);
            this.uiPanel0Container.Controls.Add(this.checkBox1);
            this.uiPanel0Container.Controls.Add(label1);
            this.uiPanel0Container.Controls.Add(this.mlt_Commodity);
            this.uiPanel0Container.Controls.Add(label2);
            this.uiPanel0Container.Controls.Add(label5);
            this.uiPanel0Container.Controls.Add(this.txtId);
            this.uiPanel0Container.Controls.Add(this.txtTitleCloth);
            this.uiPanel0Container.Controls.Add(partNameLabel);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 24);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(255, 316);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // txt_Fi
            // 
            this.txt_Fi.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.table_005_TypeClothBindingSource, "FiSale", true));
            this.txt_Fi.FormatString = "#,##0.####";
            this.txt_Fi.Location = new System.Drawing.Point(5, 146);
            this.txt_Fi.Name = "txt_Fi";
            this.txt_Fi.Size = new System.Drawing.Size(172, 24);
            this.txt_Fi.TabIndex = 23;
            this.txt_Fi.Text = "0";
            this.txt_Fi.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.table_005_TypeClothBindingSource, "SelectBrand", true));
            this.checkBox1.Location = new System.Drawing.Point(206, 188);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(52, 21);
            this.checkBox1.TabIndex = 22;
            this.checkBox1.Text = "برند";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // mlt_Commodity
            // 
            this.mlt_Commodity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Commodity.AutoComplete = false;
            this.mlt_Commodity.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.table_005_TypeClothBindingSource, "CodeCommondity", true));
            mlt_Commodity_DesignTimeLayout.LayoutString = resources.GetString("mlt_Commodity_DesignTimeLayout.LayoutString");
            this.mlt_Commodity.DesignTimeLayout = mlt_Commodity_DesignTimeLayout;
            this.mlt_Commodity.DisplayMember = "Column02";
            this.mlt_Commodity.Location = new System.Drawing.Point(5, 102);
            this.mlt_Commodity.Name = "mlt_Commodity";
            this.mlt_Commodity.SelectedIndex = -1;
            this.mlt_Commodity.SelectedItem = null;
            this.mlt_Commodity.SettingsKey = "mlt_unit";
            this.mlt_Commodity.Size = new System.Drawing.Size(171, 24);
            this.mlt_Commodity.TabIndex = 2;
            this.mlt_Commodity.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.mlt_Commodity.ValueMember = "Columnid";
            this.mlt_Commodity.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_Commodity.ValueChanged += new System.EventHandler(this.mlt_Commodity_ValueChanged);
            this.mlt_Commodity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtId_KeyPress);
            this.mlt_Commodity.Leave += new System.EventHandler(this.mlt_Commodity_Leave);
            // 
            // txtId
            // 
            this.txtId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.table_005_TypeClothBindingSource, "Number", true));
            this.txtId.Location = new System.Drawing.Point(5, 20);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(172, 24);
            this.txtId.TabIndex = 0;
            this.txtId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtId_KeyPress);
            // 
            // txtTitleCloth
            // 
            this.txtTitleCloth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleCloth.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.table_005_TypeClothBindingSource, "TypeCloth", true));
            this.txtTitleCloth.Location = new System.Drawing.Point(5, 61);
            this.txtTitleCloth.Name = "txtTitleCloth";
            this.txtTitleCloth.ReadOnly = true;
            this.txtTitleCloth.Size = new System.Drawing.Size(172, 24);
            this.txtTitleCloth.TabIndex = 1;
            this.txtTitleCloth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtId_KeyPress);
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.CausesValidation = false;
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.FloatingLocation = new System.Drawing.Point(363, 300);
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 34);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(357, 341);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "Panel 1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.gridEX1);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(355, 339);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
            this.gridEX1.ColumnAutoSizeMode = Janus.Windows.GridEX.ColumnAutoSizeMode.DisplayedCellsAndHeader;
            this.gridEX1.DataSource = this.table_005_TypeClothBindingSource;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX1.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.LavenderBlush;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SettingsKey = "Frm_15_InfoServiceGrid_6";
            this.gridEX1.Size = new System.Drawing.Size(355, 339);
            this.gridEX1.TabIndex = 2;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.UseCompatibleTextRendering = false;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // tableAdapterManager1
            // 
            this.tableAdapterManager1.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager1.Connection = null;
            this.tableAdapterManager1.Table_005_TypeClothTableAdapter = null;
            this.tableAdapterManager1.Table_010_TypeColorTableAdapter = null;
            this.tableAdapterManager1.Table_015_FormulColorTableAdapter = null;
            this.tableAdapterManager1.Table_020_DetailReciptClothRawTableAdapter = null;
            this.tableAdapterManager1.Table_020_HeaderReciptClothRowTableAdapter = null;
            this.tableAdapterManager1.Table_025_HederOrderColorTableAdapter = null;
            this.tableAdapterManager1.Table_030_DetailOrderColorTableAdapter = null;
            this.tableAdapterManager1.Table_035_ProductionTableAdapter = null;
            this.tableAdapterManager1.Table_050_Packaging1TableAdapter = null;
            this.tableAdapterManager1.Table_050_PackagingTableAdapter = null;
            this.tableAdapterManager1.Table_055_ColorDefinitionTableAdapter = null;
            this.tableAdapterManager1.Table_100_KnittingTableAdapter = null;
            this.tableAdapterManager1.Table_105_ProductionTableAdapter = null;
            this.tableAdapterManager1.Table_110_ProductionDetailTableAdapter = null;
            this.tableAdapterManager1.Table_115_ProductionColorTableAdapter = null;
            this.tableAdapterManager1.Table_40_ColorPrductionTableAdapter = null;
            this.tableAdapterManager1.Table_45_EditeProductTableAdapter = null;
            this.tableAdapterManager1.Table_60_SpecsTechnicalTableAdapter = null;
            this.tableAdapterManager1.Table_65_HeaderOtherPWHRS1TableAdapter = null;
            this.tableAdapterManager1.Table_65_HeaderOtherPWHRSTableAdapter = null;
            this.tableAdapterManager1.Table_70_DetailOtherPWHRS1TableAdapter = null;
            this.tableAdapterManager1.Table_70_DetailOtherPWHRSTableAdapter = null;
            this.tableAdapterManager1.Table_80_SettingTableAdapter = null;
            this.tableAdapterManager1.Table_85_BranchsTableAdapter = null;
            this.tableAdapterManager1.Table_90_Wares1TableAdapter = null;
            this.tableAdapterManager1.Table_90_Wares2TableAdapter = null;
            this.tableAdapterManager1.Table_90_Wares3TableAdapter = null;
            this.tableAdapterManager1.Table_90_Wares4TableAdapter = null;
            this.tableAdapterManager1.Table_90_Wares5TableAdapter = null;
            this.tableAdapterManager1.Table_90_Wares6TableAdapter = null;
            this.tableAdapterManager1.Table_90_WaresTableAdapter = null;
            this.tableAdapterManager1.Table_95_DetailWareTableAdapter = null;
            this.tableAdapterManager1.UpdateOrder = PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // table_005_TypeClothTableAdapter
            // 
            this.table_005_TypeClothTableAdapter.ClearBeforeFill = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.فعالکردنپنلToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 28);
            // 
            // فعالکردنپنلToolStripMenuItem
            // 
            this.فعالکردنپنلToolStripMenuItem.Name = "فعالکردنپنلToolStripMenuItem";
            this.فعالکردنپنلToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.فعالکردنپنلToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.فعالکردنپنلToolStripMenuItem.Text = "فعال کردن پنل";
            this.فعالکردنپنلToolStripMenuItem.Click += new System.EventHandler(this.فعالکردنپنلToolStripMenuItem_Click);
            // 
            // Frm_05_TypeCloth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 378);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "Frm_05_TypeCloth";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "معرفی انواع پارچه";
            this.Load += new System.EventHandler(this.Frm_05_TypeCloth_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_05_TypeCloth_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_005_TypeClothBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_PCLOR1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            this.uiPanel0Container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Commodity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton btn_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton btn_New;
        private System.Windows.Forms.ToolStripButton btn_Save;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Commodity;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtTitleCloth;
        private data_PCLOR.DataSet_05_PCLOR dataSet_05_PCLOR;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager tableAdapterManager;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager tableAdapterManager1;
        private data_PCLOR.DataSet_05_PCLOR dataSet_05_PCLOR1;
        private System.Windows.Forms.BindingSource table_005_TypeClothBindingSource;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.Table_005_TypeClothTableAdapter table_005_TypeClothTableAdapter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem فعالکردنپنلToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox1;
        private Janus.Windows.GridEX.EditControls.NumericEditBox txt_Fi;
    }
}