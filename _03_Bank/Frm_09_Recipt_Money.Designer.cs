namespace PCLOR._03_Bank
{
    partial class Frm_09_Recipt_Money
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_09_Recipt_Money));
            this.table_045_ReceiveCashBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_01_Cash = new PCLOR._03_Bank.DataSet_01_Cash();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.txt_Search = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btn_New = new System.Windows.Forms.ToolStripButton();
            this.btn_Save = new System.Windows.Forms.ToolStripButton();
            this.btn_Search = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_DelDoc = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bt_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_People = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Project = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Banks = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Accounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Documents = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_ViewPapers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_SignatureSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTime = new System.Windows.Forms.ToolStripMenuItem();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.table_045_ReceiveCashTableAdapter = new PCLOR._03_Bank.DataSet_01_CashTableAdapters.Table_045_ReceiveCashTableAdapter();
            this.tableAdapterManager = new PCLOR._03_Bank.DataSet_01_CashTableAdapters.TableAdapterManager();
            this.dataSet1 = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_ReceiveCashBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_01_Cash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // table_045_ReceiveCashBindingSource
            // 
            this.table_045_ReceiveCashBindingSource.DataMember = "Table_045_ReceiveCash";
            this.table_045_ReceiveCashBindingSource.DataSource = this.dataSet_01_Cash;
            this.table_045_ReceiveCashBindingSource.PositionChanged += new System.EventHandler(this.table_045_ReceiveCashBindingSource_PositionChanged);
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
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txt_Search.Border.Class = "TextBoxBorder";
            this.txt_Search.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_Search.Location = new System.Drawing.Point(583, 1);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(124, 24);
            this.txt_Search.TabIndex = 22;
            this.txt_Search.WatermarkText = "جستجــــــــــــو";
            this.txt_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Search_KeyPress);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMoveFirstItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_Delete.Image")));
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.RightToLeftAutoMirrorImage = true;
            this.btn_Delete.Size = new System.Drawing.Size(61, 24);
            this.btn_Delete.Text = "حذف";
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 27);
            // 
            // bt_Print
            // 
            this.bt_Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.bt_Print.Image = ((System.Drawing.Image)(resources.GetObject("bt_Print.Image")));
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Print.RightToLeftAutoMirrorImage = true;
            this.bt_Print.Size = new System.Drawing.Size(56, 24);
            this.bt_Print.Text = "چاپ";
            this.bt_Print.ToolTipText = "Ctrl+P";
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // btn_New
            // 
            this.btn_New.Image = ((System.Drawing.Image)(resources.GetObject("btn_New.Image")));
            this.btn_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(61, 24);
            this.btn_New.Text = "جدید";
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(66, 24);
            this.btn_Save.Text = "ذخیره";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_Search.Image = ((System.Drawing.Image)(resources.GetObject("btn_Search.Image")));
            this.btn_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(192, 24);
            this.btn_Search.Text = "                                        ";
            this.btn_Search.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
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
            this.bindingNavigatorMoveFirstItem,
            this.toolStripSeparator2,
            this.bindingNavigatorMovePreviousItem,
            this.toolStripSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.toolStripSeparator3,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.btn_Delete,
            this.toolStripButton1,
            this.bt_Print,
            this.toolStripSeparator,
            this.btn_New,
            this.btn_Save,
            this.btn_Search,
            this.bt_DelDoc});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(707, 27);
            this.bindingNavigator1.TabIndex = 21;
            this.bindingNavigator1.TabStop = true;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bt_DelDoc
            // 
            this.bt_DelDoc.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_DelDoc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.bt_DelDoc.Image = ((System.Drawing.Image)(resources.GetObject("bt_DelDoc.Image")));
            this.bt_DelDoc.Name = "bt_DelDoc";
            this.bt_DelDoc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_DelDoc.RightToLeftAutoMirrorImage = true;
            this.bt_DelDoc.Size = new System.Drawing.Size(92, 24);
            this.bt_DelDoc.Text = "حذف سند";
            this.bt_DelDoc.ToolTipText = "Ctrl+L";
            this.bt_DelDoc.Click += new System.EventHandler(this.bt_DelDoc_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_Copy,
            this.toolStripSeparator10,
            this.mnu_People,
            this.mnu_Project,
            this.mnu_Banks,
            this.mnu_Accounts,
            this.mnu_Documents,
            this.toolStripSeparator8,
            this.mnu_ViewPapers,
            this.toolStripSeparator9,
            this.mnu_SignatureSetting,
            this.toolStripSeparator12,
            this.btnTime});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(272, 262);
            // 
            // bt_Copy
            // 
            this.bt_Copy.Image = ((System.Drawing.Image)(resources.GetObject("bt_Copy.Image")));
            this.bt_Copy.Name = "bt_Copy";
            this.bt_Copy.Size = new System.Drawing.Size(271, 26);
            this.bt_Copy.Text = "Copy";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(268, 6);
            // 
            // mnu_People
            // 
            this.mnu_People.Image = ((System.Drawing.Image)(resources.GetObject("mnu_People.Image")));
            this.mnu_People.Name = "mnu_People";
            this.mnu_People.Size = new System.Drawing.Size(271, 26);
            this.mnu_People.Text = "معرفی اشخاص";
            // 
            // mnu_Project
            // 
            this.mnu_Project.Image = ((System.Drawing.Image)(resources.GetObject("mnu_Project.Image")));
            this.mnu_Project.Name = "mnu_Project";
            this.mnu_Project.Size = new System.Drawing.Size(271, 26);
            this.mnu_Project.Text = "معرفی پروژه ها";
            // 
            // mnu_Banks
            // 
            this.mnu_Banks.Image = ((System.Drawing.Image)(resources.GetObject("mnu_Banks.Image")));
            this.mnu_Banks.Name = "mnu_Banks";
            this.mnu_Banks.Size = new System.Drawing.Size(271, 26);
            this.mnu_Banks.Text = "معرفی صندوقها و بانکها";
            // 
            // mnu_Accounts
            // 
            this.mnu_Accounts.Image = ((System.Drawing.Image)(resources.GetObject("mnu_Accounts.Image")));
            this.mnu_Accounts.Name = "mnu_Accounts";
            this.mnu_Accounts.Size = new System.Drawing.Size(271, 26);
            this.mnu_Accounts.Text = "معرفی سرفصل حسابها";
            // 
            // mnu_Documents
            // 
            this.mnu_Documents.Image = ((System.Drawing.Image)(resources.GetObject("mnu_Documents.Image")));
            this.mnu_Documents.Name = "mnu_Documents";
            this.mnu_Documents.Size = new System.Drawing.Size(271, 26);
            this.mnu_Documents.Text = "مشاهده اسناد حسابداری";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(268, 6);
            // 
            // mnu_ViewPapers
            // 
            this.mnu_ViewPapers.Image = ((System.Drawing.Image)(resources.GetObject("mnu_ViewPapers.Image")));
            this.mnu_ViewPapers.Name = "mnu_ViewPapers";
            this.mnu_ViewPapers.Size = new System.Drawing.Size(271, 26);
            this.mnu_ViewPapers.Text = "مشاهده برگه های دریافت";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(268, 6);
            // 
            // mnu_SignatureSetting
            // 
            this.mnu_SignatureSetting.Image = ((System.Drawing.Image)(resources.GetObject("mnu_SignatureSetting.Image")));
            this.mnu_SignatureSetting.Name = "mnu_SignatureSetting";
            this.mnu_SignatureSetting.Size = new System.Drawing.Size(271, 26);
            this.mnu_SignatureSetting.Text = "تنظیم عناوین امضاها در چاپ برگه";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(268, 6);
            // 
            // btnTime
            // 
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(271, 26);
            this.btnTime.Text = "تاثیر مدت";
            this.btnTime.Visible = false;
            // 
            // superTabItem1
            // 
            this.superTabItem1.BeginGroup = true;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "اطلاعات برگه";
            this.superTabItem1.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.MenuBox.Visible = false;
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.ControlBox.Visible = false;
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(3, 30);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = false;
            this.superTabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(701, 329);
            this.superTabControl1.TabFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superTabControl1.TabIndex = 24;
            this.superTabControl1.TabLayoutType = DevComponents.DotNetBar.eSuperTabLayoutType.MultiLine;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem2});
            this.superTabControl1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            this.superTabControl1.Text = "superTabControl1";
            this.superTabControl1.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.gridEX2);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 27);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(701, 302);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem2;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AllowColumnDrag = false;
            this.gridEX2.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX2.BackColor = System.Drawing.Color.AntiqueWhite;
            this.gridEX2.BlendColor = System.Drawing.Color.Transparent;
            this.gridEX2.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEX2.CardBorders = false;
            this.gridEX2.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.AntiqueWhite;
            this.gridEX2.CardInnerSpacing = 8;
            this.gridEX2.CardSpacing = 6;
            this.gridEX2.CardViewGridlines = Janus.Windows.GridEX.CardViewGridlines.FieldsOnly;
            this.gridEX2.CardWidth = 668;
            this.gridEX2.CenterSingleCard = false;
            this.gridEX2.DataSource = this.table_045_ReceiveCashBindingSource;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.Enabled = false;
            this.gridEX2.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX2.ExpandableCards = false;
            this.gridEX2.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX2.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            gridEX2_Layout_0.DataSource = this.table_045_ReceiveCashBindingSource;
            gridEX2_Layout_0.IsCurrentLayout = true;
            gridEX2_Layout_0.Key = "PERP";
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
            this.gridEX2.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.gridEX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX2.RowFormatStyle.BackColor = System.Drawing.Color.AntiqueWhite;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.Size = new System.Drawing.Size(701, 302);
            this.gridEX2.TabIndex = 17;
            this.gridEX2.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX2.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX2.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX2.View = Janus.Windows.GridEX.View.SingleCard;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX2.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX2_CellValueChanged);
            this.gridEX2.RowEditCanceled += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX2_RowEditCanceled);
            this.gridEX2.CellUpdated += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX2_CellUpdated);
            this.gridEX2.UpdatingCell += new Janus.Windows.GridEX.UpdatingCellEventHandler(this.gridEX2_UpdatingCell);
            this.gridEX2.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX2_Error);
            this.gridEX2.CurrentCellChanged += new System.EventHandler(this.gridEX2_CurrentCellChanged);
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel1;
            this.superTabItem2.BeginGroup = true;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.Text = "اطلاعات برگه";
            this.superTabItem2.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            // 
            // table_045_ReceiveCashTableAdapter
            // 
            this.table_045_ReceiveCashTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_020_BankCashAccInfoTableAdapter = null;
            this.tableAdapterManager.Table_035_ReceiptChequesTableAdapter = null;
            this.tableAdapterManager.Table_040_CashPaymentsTableAdapter = null;
            this.tableAdapterManager.Table_045_ReceiveCashTableAdapter = this.table_045_ReceiveCashTableAdapter;
            this.tableAdapterManager.Table_065_TurnReceptionTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PCLOR._03_Bank.DataSet_01_CashTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // Frm_09_Recipt_Money
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 362);
            this.Controls.Add(this.superTabControl1);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_09_Recipt_Money";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "دریافت نقد";
            this.Load += new System.EventHandler(this.Frm_09_Recipt_Money_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table_045_ReceiveCashBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_01_Cash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        public DevComponents.DotNetBar.Controls.TextBoxX txt_Search;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton btn_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton btn_New;
        private System.Windows.Forms.ToolStripButton btn_Save;
        public System.Windows.Forms.ToolStripButton btn_Search;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem bt_Copy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem mnu_People;
        private System.Windows.Forms.ToolStripMenuItem mnu_Project;
        private System.Windows.Forms.ToolStripMenuItem mnu_Banks;
        private System.Windows.Forms.ToolStripMenuItem mnu_Accounts;
        private System.Windows.Forms.ToolStripMenuItem mnu_Documents;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem mnu_ViewPapers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem mnu_SignatureSetting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem btnTime;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private Janus.Windows.GridEX.GridEX gridEX2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DataSet_01_Cash dataSet_01_Cash;
        private System.Windows.Forms.BindingSource table_045_ReceiveCashBindingSource;
        private DataSet_01_CashTableAdapters.Table_045_ReceiveCashTableAdapter table_045_ReceiveCashTableAdapter;
        private DataSet_01_CashTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.ToolStripButton bt_DelDoc;
    }
}