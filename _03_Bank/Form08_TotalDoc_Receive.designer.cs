namespace PCLOR._03_Bank
{
    partial class Form08_TotalDoc_Receive
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
            Janus.Windows.Common.JanusColorScheme janusColorScheme1 = new Janus.Windows.Common.JanusColorScheme();
            Janus.Windows.GridEX.GridEXLayout mlt_Project_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout mlt_Bes_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEX1_Layout_0_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.Columns.Column0.ValueList.Item0.Image");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form08_TotalDoc_Receive));
            this.txt_Cover = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.txt_LastNum = new Janus.Windows.GridEX.EditControls.EditBox();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.rdb_last = new Janus.Windows.EditControls.UIRadioButton();
            this.rdb_New = new Janus.Windows.EditControls.UIRadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.rdb_TO = new Janus.Windows.EditControls.UIRadioButton();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.visualStyleManager1 = new Janus.Windows.Common.VisualStyleManager(this.components);
            this.dataSet1 = new System.Data.DataSet();
            this.superTabControl2 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.mlt_Project = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.mlt_Bes = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_Display = new System.Windows.Forms.ToolStripButton();
            this.bt_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_View = new System.Windows.Forms.ToolStripButton();
            this.cmb_Status = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_ExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl2)).BeginInit();
            this.superTabControl2.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Project)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Bes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Cover
            // 
            this.txt_Cover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Cover.Location = new System.Drawing.Point(10, 84);
            this.txt_Cover.Name = "txt_Cover";
            this.txt_Cover.Size = new System.Drawing.Size(712, 21);
            this.txt_Cover.TabIndex = 9;
            this.txt_Cover.Text = "گردش خزانه- ثبت اسناد دریافتنی";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(724, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "شرح سند:";
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(278, 51);
            this.txt_To.Name = "txt_To";
            this.txt_To.Size = new System.Drawing.Size(46, 21);
            this.txt_To.TabIndex = 7;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_To_KeyPress);
            this.txt_To.Leave += new System.EventHandler(this.txt_To_Leave);
            // 
            // txt_LastNum
            // 
            this.txt_LastNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_LastNum.Enabled = false;
            this.txt_LastNum.Location = new System.Drawing.Point(539, 51);
            this.txt_LastNum.Name = "txt_LastNum";
            this.txt_LastNum.Size = new System.Drawing.Size(48, 21);
            this.txt_LastNum.TabIndex = 5;
            this.txt_LastNum.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.Location = new System.Drawing.Point(12, 50);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(167, 20);
            this.faDatePicker1.TabIndex = 8;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // rdb_last
            // 
            this.rdb_last.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_last.BackColor = System.Drawing.Color.Transparent;
            this.rdb_last.Location = new System.Drawing.Point(587, 50);
            this.rdb_last.Name = "rdb_last";
            this.rdb_last.Size = new System.Drawing.Size(114, 23);
            this.rdb_last.TabIndex = 4;
            this.rdb_last.Text = "اضافه به آخرین سند:";
            this.rdb_last.CheckedChanged += new System.EventHandler(this.rdb_last_CheckedChanged);
            // 
            // rdb_New
            // 
            this.rdb_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_New.BackColor = System.Drawing.Color.Transparent;
            this.rdb_New.Checked = true;
            this.rdb_New.Location = new System.Drawing.Point(706, 50);
            this.rdb_New.Name = "rdb_New";
            this.rdb_New.Size = new System.Drawing.Size(77, 23);
            this.rdb_New.TabIndex = 3;
            this.rdb_New.TabStop = true;
            this.rdb_New.Text = "سند جدید";
            this.rdb_New.CheckedChanged += new System.EventHandler(this.rdb_New_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(185, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "تاریخ سند:";
            // 
            // rdb_TO
            // 
            this.rdb_TO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_TO.BackColor = System.Drawing.Color.Transparent;
            this.rdb_TO.Location = new System.Drawing.Point(327, 50);
            this.rdb_TO.Name = "rdb_TO";
            this.rdb_TO.Size = new System.Drawing.Size(118, 23);
            this.rdb_TO.TabIndex = 6;
            this.rdb_TO.Text = "اضافه به سند شماره:";
            this.rdb_TO.CheckedChanged += new System.EventHandler(this.rdb_TO_CheckedChanged);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // visualStyleManager1
            // 
            janusColorScheme1.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            janusColorScheme1.Name = "Scheme0";
            janusColorScheme1.OfficeColorScheme = Janus.Windows.Common.OfficeColorScheme.Custom;
            janusColorScheme1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            janusColorScheme1.VisualStyle = Janus.Windows.Common.VisualStyle.Office2007;
            this.visualStyleManager1.ColorSchemes.Add(janusColorScheme1);
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // superTabControl2
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl2.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl2.ControlBox.MenuBox.Name = "";
            this.superTabControl2.ControlBox.MenuBox.Visible = false;
            this.superTabControl2.ControlBox.Name = "";
            this.superTabControl2.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl2.ControlBox.MenuBox,
            this.superTabControl2.ControlBox.CloseBox});
            this.superTabControl2.ControlBox.Visible = false;
            this.superTabControl2.Controls.Add(this.superTabControlPanel2);
            this.superTabControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.superTabControl2.Location = new System.Drawing.Point(0, 310);
            this.superTabControl2.Name = "superTabControl2";
            this.superTabControl2.ReorderTabsEnabled = false;
            this.superTabControl2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.superTabControl2.SelectedTabFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.superTabControl2.SelectedTabIndex = 0;
            this.superTabControl2.Size = new System.Drawing.Size(794, 142);
            this.superTabControl2.TabFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superTabControl2.TabIndex = 27;
            this.superTabControl2.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem2});
            this.superTabControl2.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            this.superTabControl2.Text = "superTabControl2";
            this.superTabControl2.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.mlt_Project);
            this.superTabControlPanel2.Controls.Add(this.label6);
            this.superTabControlPanel2.Controls.Add(this.mlt_Bes);
            this.superTabControlPanel2.Controls.Add(this.rdb_New);
            this.superTabControlPanel2.Controls.Add(this.rdb_TO);
            this.superTabControlPanel2.Controls.Add(this.txt_Cover);
            this.superTabControlPanel2.Controls.Add(this.rdb_last);
            this.superTabControlPanel2.Controls.Add(this.faDatePicker1);
            this.superTabControlPanel2.Controls.Add(this.label7);
            this.superTabControlPanel2.Controls.Add(this.txt_LastNum);
            this.superTabControlPanel2.Controls.Add(this.label5);
            this.superTabControlPanel2.Controls.Add(this.label9);
            this.superTabControlPanel2.Controls.Add(this.txt_To);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 24);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.superTabControlPanel2.Size = new System.Drawing.Size(794, 118);
            this.superTabControlPanel2.TabIndex = 1;
            this.superTabControlPanel2.TabItem = this.superTabItem2;
            // 
            // mlt_Project
            // 
            this.mlt_Project.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Project_DesignTimeLayout.LayoutString = resources.GetString("mlt_Project_DesignTimeLayout.LayoutString");
            this.mlt_Project.DesignTimeLayout = mlt_Project_DesignTimeLayout;
            this.mlt_Project.DisplayMember = "Column02";
            this.mlt_Project.Location = new System.Drawing.Point(278, 13);
            this.mlt_Project.Name = "mlt_Project";
            this.mlt_Project.SelectedIndex = -1;
            this.mlt_Project.SelectedItem = null;
            this.mlt_Project.Size = new System.Drawing.Size(158, 21);
            this.mlt_Project.TabIndex = 1;
            this.mlt_Project.ValueMember = "Column00";
            this.mlt_Project.Visible = false;
            this.mlt_Project.VisualStyleManager = this.visualStyleManager1;
            this.mlt_Project.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Bed_KeyPress);
            // 
            // mlt_Bes
            // 
            this.mlt_Bes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Bes.AutoComplete = false;
            mlt_Bes_DesignTimeLayout.LayoutString = resources.GetString("mlt_Bes_DesignTimeLayout.LayoutString");
            this.mlt_Bes.DesignTimeLayout = mlt_Bes_DesignTimeLayout;
            this.mlt_Bes.DisplayMember = "ACC_Name";
            this.mlt_Bes.Location = new System.Drawing.Point(535, 13);
            this.mlt_Bes.Name = "mlt_Bes";
            this.mlt_Bes.SelectedIndex = -1;
            this.mlt_Bes.SelectedItem = null;
            this.mlt_Bes.Size = new System.Drawing.Size(153, 21);
            this.mlt_Bes.TabIndex = 0;
            this.mlt_Bes.ValueMember = "ACC_Code";
            this.mlt_Bes.VisualStyleManager = this.visualStyleManager1;
            this.mlt_Bes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Bed_KeyPress);
            this.mlt_Bes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mlt_Bes_KeyUp);
            this.mlt_Bes.Leave += new System.EventHandler(this.mlt_Bes_Leave);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(442, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 43;
            this.label5.Text = "پروژه بدهکار:";
            this.label5.Visible = false;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(693, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "سرفصل بستانکار:";
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel2;
            this.superTabItem2.BeginGroup = true;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.BlueMist;
            this.superTabItem2.Text = "اطلاعات تنظیم سند";
            this.superTabItem2.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bindingNavigator1.BackgroundImage")));
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_Display,
            this.bt_Save,
            this.toolStripSeparator1,
            this.bt_View,
            this.cmb_Status,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.bt_ExportToExcel,
            this.toolStripSeparator3});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(794, 25);
            this.bindingNavigator1.TabIndex = 6;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // bt_Display
            // 
            this.bt_Display.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Display.Image = ((System.Drawing.Image)(resources.GetObject("bt_Display.Image")));
            this.bt_Display.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(23, 22);
            this.bt_Display.Text = "نمایش";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Save.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.bt_Save.Image = ((System.Drawing.Image)(resources.GetObject("bt_Save.Image")));
            this.bt_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Save.Size = new System.Drawing.Size(75, 22);
            this.bt_Save.Text = "صدور سند";
            this.bt_Save.ToolTipText = "Ctrl+S";
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Visible = false;
            // 
            // bt_View
            // 
            this.bt_View.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_View.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.bt_View.Image = ((System.Drawing.Image)(resources.GetObject("bt_View.Image")));
            this.bt_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_View.Name = "bt_View";
            this.bt_View.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_View.Size = new System.Drawing.Size(96, 22);
            this.bt_View.Text = "مشاهده اسناد";
            this.bt_View.ToolTipText = "Ctrl+W";
            this.bt_View.Visible = false;
            this.bt_View.Click += new System.EventHandler(this.bt_View_Click);
            // 
            // cmb_Status
            // 
            this.cmb_Status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Status.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Status.Name = "cmb_Status";
            this.cmb_Status.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmb_Status.Size = new System.Drawing.Size(160, 25);
            this.cmb_Status.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_Status_KeyPress);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel1.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel1.Text = "وضعیت چک:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_ExportToExcel
            // 
            this.bt_ExportToExcel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_ExportToExcel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ExportToExcel.Name = "bt_ExportToExcel";
            this.bt_ExportToExcel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_ExportToExcel.Size = new System.Drawing.Size(79, 22);
            this.bt_ExportToExcel.Text = "ارسال به Excel";
            this.bt_ExportToExcel.Click += new System.EventHandler(this.bt_ExportToExcel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // gridEXExporter1
            // 
            this.gridEXExporter1.GridEX = this.gridEX1;
            this.gridEXExporter1.IncludeCollapsedRows = false;
            // 
            // gridEX1
            // 
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.BackColor = System.Drawing.Color.White;
            this.gridEX1.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.Linen;
            this.gridEX1.CardHeaders = false;
            this.gridEX1.CardInnerSpacing = 9;
            this.gridEX1.CardViewGridlines = Janus.Windows.GridEX.CardViewGridlines.FieldsOnly;
            this.gridEX1.CenterSingleCard = false;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.ExpandableCards = false;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            gridEX1_Layout_0.IsCurrentLayout = true;
            gridEX1_Layout_0.Key = "PERP";
            gridEX1_Layout_0_Reference_0.Instance = ((object)(resources.GetObject("gridEX1_Layout_0_Reference_0.Instance")));
            gridEX1_Layout_0.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridEX1_Layout_0_Reference_0});
            gridEX1_Layout_0.LayoutString = resources.GetString("gridEX1_Layout_0.LayoutString");
            this.gridEX1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX1_Layout_0});
            this.gridEX1.Location = new System.Drawing.Point(3, 28);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowEnterKeyBehavior = Janus.Windows.GridEX.NewRowEnterKeyBehavior.None;
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.NewRowPosition = Janus.Windows.GridEX.NewRowPosition.BottomRow;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RecordNavigator = true;
            this.gridEX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX1.RowFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.gridEX1.RowFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowIndex;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SettingsKey = "Form08_TotalDoc_Receive5";
            this.gridEX1.Size = new System.Drawing.Size(788, 253);
            this.gridEX1.TabIndex = 29;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX1.RowCheckStateChanged += new Janus.Windows.GridEX.RowCheckStateChangeEventHandler(this.gridEX1_RowCheckStateChanged);
            this.gridEX1.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellValueChanged);
            this.gridEX1.CancelingCellEdit += new Janus.Windows.GridEX.ColumnActionCancelEventHandler(this.gridEX1_CancelingCellEdit);
            this.gridEX1.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridEX1_FormattingRow);
            this.gridEX1.CellUpdated += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellUpdated);
            this.gridEX1.SelectionChanged += new System.EventHandler(this.gridEX1_SelectionChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.Panel;
            this.uiGroupBox2.Controls.Add(this.label1);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiGroupBox2.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox2.Location = new System.Drawing.Point(0, 284);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox2.OfficeCustomColor = System.Drawing.Color.Aqua;
            this.uiGroupBox2.Size = new System.Drawing.Size(794, 26);
            this.uiGroupBox2.TabIndex = 28;
            this.uiGroupBox2.Visible = false;
            this.uiGroupBox2.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(777, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "label2";
            // 
            // Form08_TotalDoc_Receive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.uiGroupBox2);
            this.Controls.Add(this.superTabControl2);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form08_TotalDoc_Receive";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "صدور سند حسابداری اسناد دریافتنی";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form08_TotalDoc_Receive_FormClosing);
            this.Load += new System.EventHandler(this.Form08_TotalDoc_Receive_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form08_TotalDoc_Receive_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl2)).EndInit();
            this.superTabControl2.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Project)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Bes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_View;
        private Janus.Windows.GridEX.EditControls.EditBox txt_Cover;
        private System.Windows.Forms.Label label7;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private Janus.Windows.GridEX.EditControls.EditBox txt_To;
        private Janus.Windows.EditControls.UIRadioButton rdb_TO;
        private Janus.Windows.GridEX.EditControls.EditBox txt_LastNum;
        private Janus.Windows.EditControls.UIRadioButton rdb_last;
        private Janus.Windows.EditControls.UIRadioButton rdb_New;
        private System.Windows.Forms.Label label6;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.Common.VisualStyleManager visualStyleManager1;
        private System.Data.DataSet dataSet1;
        private DevComponents.DotNetBar.SuperTabControl superTabControl2;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Project;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Bes;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmb_Status;
        private System.Windows.Forms.ToolStripButton bt_Display;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bt_ExportToExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private System.Windows.Forms.Label label1;
    }
}