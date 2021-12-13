namespace PCLOR._03_Bank
{
    partial class Frm_01_Report_Statuscheq
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
            Janus.Windows.GridEX.GridEXLayout mlt_BankBox_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout mlt_Status_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Get_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_01_Report_Statuscheq));
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.mlt_BankBox = new Janus.Windows.GridEX.EditControls.CheckedComboBox();
            this.mlt_Status = new Janus.Windows.GridEX.EditControls.CheckedComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdb_Pay = new Janus.Windows.EditControls.UIRadioButton();
            this.faDatePicker2 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.rdb_Get = new Janus.Windows.EditControls.UIRadioButton();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Get = new Janus.Windows.GridEX.GridEX();
            this.dataSet1 = new System.Data.DataSet();
            this.GetbindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ribbonBarMergeContainer1 = new DevComponents.DotNetBar.RibbonBarMergeContainer();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.gridEXFieldChooserControl1 = new Janus.Windows.GridEX.GridEXFieldChooserControl();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.microChartItem1 = new DevComponents.DotNetBar.MicroChartItem();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Get)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetbindingSource)).BeginInit();
            this.ribbonBarMergeContainer1.SuspendLayout();
            this.ribbonBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel0.Id = new System.Guid("698787fd-c536-41a7-90b7-319c95af5107");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            this.uiPanel1.Id = new System.Guid("aeb9b7e4-e8d2-44ff-993c-7efe37862161");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("aeb9b7e4-e8d2-44ff-993c-7efe37862161"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(578, 442), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("698787fd-c536-41a7-90b7-319c95af5107"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(200, 442), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("698787fd-c536-41a7-90b7-319c95af5107"), new System.Drawing.Point(731, 373), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("aeb9b7e4-e8d2-44ff-993c-7efe37862161"), new System.Drawing.Point(258, 404), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(731, 373);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(581, 3);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(200, 442);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "تنظیمات";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.mlt_BankBox);
            this.uiPanel0Container.Controls.Add(this.mlt_Status);
            this.uiPanel0Container.Controls.Add(this.button1);
            this.uiPanel0Container.Controls.Add(this.label2);
            this.uiPanel0Container.Controls.Add(this.label3);
            this.uiPanel0Container.Controls.Add(this.label1);
            this.uiPanel0Container.Controls.Add(this.rdb_Pay);
            this.uiPanel0Container.Controls.Add(this.faDatePicker2);
            this.uiPanel0Container.Controls.Add(this.label5);
            this.uiPanel0Container.Controls.Add(this.rdb_Get);
            this.uiPanel0Container.Controls.Add(this.faDatePicker1);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 21);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(194, 420);
            this.uiPanel0Container.TabIndex = 0;
            this.uiPanel0Container.Click += new System.EventHandler(this.uiPanel0Container_Click);
            // 
            // mlt_BankBox
            // 
            this.mlt_BankBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_BankBox.ButtonCancelText = "انصراف";
            this.mlt_BankBox.ButtonOKText = "تائید";
            mlt_BankBox_DesignTimeLayout.LayoutString = resources.GetString("mlt_BankBox_DesignTimeLayout.LayoutString");
            this.mlt_BankBox.DesignTimeLayout = mlt_BankBox_DesignTimeLayout;
            this.mlt_BankBox.DropDownDisplayMember = "Column02";
            this.mlt_BankBox.DropDownValueMember = "ColumnId";
            this.mlt_BankBox.Location = new System.Drawing.Point(8, 229);
            this.mlt_BankBox.Name = "mlt_BankBox";
            this.mlt_BankBox.SaveSettings = false;
            this.mlt_BankBox.Size = new System.Drawing.Size(178, 21);
            this.mlt_BankBox.TabIndex = 67;
            this.mlt_BankBox.ValuesDataMember = null;
            this.mlt_BankBox.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.mlt_BankBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_BankBox_KeyPress);
            // 
            // mlt_Status
            // 
            this.mlt_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Status.ButtonCancelText = "انصراف";
            this.mlt_Status.ButtonOKText = "تائید";
            mlt_Status_DesignTimeLayout.LayoutString = resources.GetString("mlt_Status_DesignTimeLayout.LayoutString");
            this.mlt_Status.DesignTimeLayout = mlt_Status_DesignTimeLayout;
            this.mlt_Status.DropDownDisplayMember = "Column02";
            this.mlt_Status.DropDownValueMember = "ColumnId";
            this.mlt_Status.Location = new System.Drawing.Point(8, 171);
            this.mlt_Status.Name = "mlt_Status";
            this.mlt_Status.SaveSettings = false;
            this.mlt_Status.Size = new System.Drawing.Size(178, 21);
            this.mlt_Status.TabIndex = 66;
            this.mlt_Status.ValuesDataMember = null;
            this.mlt_Status.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.mlt_Status.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Status_KeyPress);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(56, 288);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 23);
            this.button1.TabIndex = 65;
            this.button1.Text = "مشاهده";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(142, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 64;
            this.label2.Text = "صندوق:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(123, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "وضعیت چک:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(148, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "تا تاریخ:";
            // 
            // rdb_Pay
            // 
            this.rdb_Pay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Pay.Location = new System.Drawing.Point(56, 43);
            this.rdb_Pay.Name = "rdb_Pay";
            this.rdb_Pay.Size = new System.Drawing.Size(132, 23);
            this.rdb_Pay.TabIndex = 3;
            this.rdb_Pay.Text = "بر اساس تاریخ سررسید";
            // 
            // faDatePicker2
            // 
            this.faDatePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker2.Location = new System.Drawing.Point(8, 109);
            this.faDatePicker2.Name = "faDatePicker2";
            this.faDatePicker2.Size = new System.Drawing.Size(138, 20);
            this.faDatePicker2.TabIndex = 58;
            this.faDatePicker2.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker2.TextChanged += new System.EventHandler(this.faDatePicker2_TextChanged);
            this.faDatePicker2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker2_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(147, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 59;
            this.label5.Text = "از تاریخ:";
            // 
            // rdb_Get
            // 
            this.rdb_Get.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Get.Checked = true;
            this.rdb_Get.Location = new System.Drawing.Point(56, 14);
            this.rdb_Get.Name = "rdb_Get";
            this.rdb_Get.Size = new System.Drawing.Size(132, 23);
            this.rdb_Get.TabIndex = 2;
            this.rdb_Get.TabStop = true;
            this.rdb_Get.Text = "بر اساس تاریخ دریافت";
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.IsDefault = true;
            this.faDatePicker1.Location = new System.Drawing.Point(8, 83);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(138, 20);
            this.faDatePicker1.TabIndex = 57;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // uiPanel1
            // 
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.FloatingLocation = new System.Drawing.Point(258, 404);
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 3);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(578, 442);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "لیست اسناد";
            this.uiPanel1.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.gridEX_Get);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 21);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(576, 420);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // gridEX_Get
            // 
            this.gridEX_Get.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Get.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Get.AlternatingColors = true;
            this.gridEX_Get.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX_Get.BackColor = System.Drawing.Color.White;
            this.gridEX_Get.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.Linen;
            this.gridEX_Get.CardHeaders = false;
            this.gridEX_Get.CardInnerSpacing = 9;
            this.gridEX_Get.CardViewGridlines = Janus.Windows.GridEX.CardViewGridlines.FieldsOnly;
            this.gridEX_Get.CenterSingleCard = false;
            this.gridEX_Get.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Get.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Get.ExpandableCards = false;
            this.gridEX_Get.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Get.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Get.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX_Get.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Get.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Get.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX_Get.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Get.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Get.GroupByBoxVisible = false;
            this.gridEX_Get.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            gridEX_Get_Layout_0.IsCurrentLayout = true;
            gridEX_Get_Layout_0.Key = "PERP";
            gridEX_Get_Layout_0.LayoutString = resources.GetString("gridEX_Get_Layout_0.LayoutString");
            this.gridEX_Get.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX_Get_Layout_0});
            this.gridEX_Get.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Get.Name = "gridEX_Get";
            this.gridEX_Get.NewRowEnterKeyBehavior = Janus.Windows.GridEX.NewRowEnterKeyBehavior.None;
            this.gridEX_Get.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Get.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Get.NewRowPosition = Janus.Windows.GridEX.NewRowPosition.BottomRow;
            this.gridEX_Get.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Get.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Get.RecordNavigator = true;
            this.gridEX_Get.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX_Get.RowFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.gridEX_Get.RowFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Get.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowIndex;
            this.gridEX_Get.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Get.SaveSettings = true;
            this.gridEX_Get.SettingsKey = "Form03_Rec_Reports07";
            this.gridEX_Get.Size = new System.Drawing.Size(576, 420);
            this.gridEX_Get.TabIndex = 11;
            this.gridEX_Get.TableHeaderFormatStyle.Font = new System.Drawing.Font("B Traffic", 9F, System.Drawing.FontStyle.Bold);
            this.gridEX_Get.TableHeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Get.TableHeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.gridEX_Get.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Get.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Get.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX_Get.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Get.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // ribbonBarMergeContainer1
            // 
            this.ribbonBarMergeContainer1.AutoActivateTab = false;
            this.ribbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.ribbonBarMergeContainer1.Controls.Add(this.ribbonBar1);
            this.ribbonBarMergeContainer1.Location = new System.Drawing.Point(36, 3);
            this.ribbonBarMergeContainer1.MergeRibbonGroupName = "SettingTab";
            this.ribbonBarMergeContainer1.Name = "ribbonBarMergeContainer1";
            this.ribbonBarMergeContainer1.RibbonTabColorTable = DevComponents.DotNetBar.eRibbonTabColor.Magenta;
            this.ribbonBarMergeContainer1.RibbonTabText = "تنظیمات";
            this.ribbonBarMergeContainer1.Size = new System.Drawing.Size(447, 31);
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
            this.ribbonBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ribbonBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.microChartItem1});
            this.ribbonBar1.Location = new System.Drawing.Point(246, 0);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.ResizeItemsToFit = false;
            this.ribbonBar1.Size = new System.Drawing.Size(201, 31);
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
            this.gridEXFieldChooserControl1.GridEX = this.gridEX_Get;
            this.gridEXFieldChooserControl1.Location = new System.Drawing.Point(0, 0);
            this.gridEXFieldChooserControl1.Name = "gridEXFieldChooserControl1";
            this.gridEXFieldChooserControl1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEXFieldChooserControl1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEXFieldChooserControl1.Size = new System.Drawing.Size(201, 15);
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
            // Frm_01_Report_Statuscheq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 448);
            this.Controls.Add(this.ribbonBarMergeContainer1);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.uiPanel0);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_01_Report_Statuscheq";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش وضعیت چک ها";
            this.Load += new System.EventHandler(this.Frm_01_Report_Statuscheq_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            this.uiPanel0Container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.Configuration.IPersistComponentSettings)(this.gridEX_Get)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Get)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetbindingSource)).EndInit();
            this.ribbonBarMergeContainer1.ResumeLayout(false);
            this.ribbonBar1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIRadioButton rdb_Pay;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker2;
        private System.Windows.Forms.Label label5;
        private Janus.Windows.EditControls.UIRadioButton rdb_Get;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.CheckedComboBox mlt_BankBox;
        private Janus.Windows.GridEX.EditControls.CheckedComboBox mlt_Status;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.BindingSource GetbindingSource;
        private DevComponents.DotNetBar.RibbonBarMergeContainer ribbonBarMergeContainer1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private Janus.Windows.GridEX.GridEXFieldChooserControl gridEXFieldChooserControl1;
        private Janus.Windows.GridEX.GridEX gridEX_Get;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.MicroChartItem microChartItem1;
    }
}