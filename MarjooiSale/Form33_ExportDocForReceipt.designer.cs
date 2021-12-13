namespace PCLOR.MarjooiSale
{
    partial class Form33_ExportDocForReceipt
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
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form33_ExportDocForReceipt));
            Janus.Windows.ExplorerBar.ExplorerBarGroup explorerBarGroup1 = new Janus.Windows.ExplorerBar.ExplorerBarGroup();
            Janus.Windows.GridEX.GridEXLayout gridEX1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.explorerBarContainerControl4 = new Janus.Windows.ExplorerBar.ExplorerBarContainerControl();
            this.txt_Cover = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_To = new System.Windows.Forms.RadioButton();
            this.rdb_New = new System.Windows.Forms.RadioButton();
            this.rdb_Last = new System.Windows.Forms.RadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_ExportDoc = new System.Windows.Forms.ToolStripButton();
            this.bt_ViewDocs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.explorerBar1 = new Janus.Windows.ExplorerBar.ExplorerBar();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.explorerBarContainerControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).BeginInit();
            this.explorerBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label4.AutoSize = true;
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.Location = new System.Drawing.Point(420, 9);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(59, 13);
            label4.TabIndex = 35;
            label4.Text = "تاریخ سند :";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label5.AutoSize = true;
            label5.BackColor = System.Drawing.Color.Transparent;
            label5.Location = new System.Drawing.Point(415, 37);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(64, 13);
            label5.TabIndex = 36;
            label5.Text = " شرح سند :";
            // 
            // explorerBarContainerControl4
            // 
            this.explorerBarContainerControl4.Controls.Add(this.txt_Cover);
            this.explorerBarContainerControl4.Controls.Add(this.rdb_To);
            this.explorerBarContainerControl4.Controls.Add(this.rdb_New);
            this.explorerBarContainerControl4.Controls.Add(this.rdb_Last);
            this.explorerBarContainerControl4.Controls.Add(this.txt_To);
            this.explorerBarContainerControl4.Controls.Add(label4);
            this.explorerBarContainerControl4.Controls.Add(this.faDatePicker1);
            this.explorerBarContainerControl4.Controls.Add(label5);
            this.explorerBarContainerControl4.Location = new System.Drawing.Point(3, 32);
            this.explorerBarContainerControl4.Name = "explorerBarContainerControl4";
            this.explorerBarContainerControl4.Size = new System.Drawing.Size(780, 66);
            this.explorerBarContainerControl4.TabIndex = 5;
            // 
            // txt_Cover
            // 
            this.txt_Cover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Cover.Location = new System.Drawing.Point(19, 31);
            this.txt_Cover.Multiline = true;
            this.txt_Cover.Name = "txt_Cover";
            this.txt_Cover.Size = new System.Drawing.Size(386, 24);
            this.txt_Cover.TabIndex = 5;
            this.txt_Cover.Text = "گردش انبار- صدور سند مرجوعی";
            this.txt_Cover.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // rdb_To
            // 
            this.rdb_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_To.AutoSize = true;
            this.rdb_To.BackColor = System.Drawing.Color.Transparent;
            this.rdb_To.Location = new System.Drawing.Point(658, 35);
            this.rdb_To.Name = "rdb_To";
            this.rdb_To.Size = new System.Drawing.Size(120, 17);
            this.rdb_To.TabIndex = 2;
            this.rdb_To.Text = "اضافه به سند شماره";
            this.rdb_To.UseVisualStyleBackColor = false;
            this.rdb_To.CheckedChanged += new System.EventHandler(this.rdb_To_CheckedChanged);
            // 
            // rdb_New
            // 
            this.rdb_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_New.AutoSize = true;
            this.rdb_New.BackColor = System.Drawing.Color.Transparent;
            this.rdb_New.Checked = true;
            this.rdb_New.Location = new System.Drawing.Point(707, 8);
            this.rdb_New.Name = "rdb_New";
            this.rdb_New.Size = new System.Drawing.Size(71, 17);
            this.rdb_New.TabIndex = 0;
            this.rdb_New.TabStop = true;
            this.rdb_New.Text = "سند جدید";
            this.rdb_New.UseVisualStyleBackColor = false;
            this.rdb_New.CheckedChanged += new System.EventHandler(this.rdb_New_CheckedChanged);
            // 
            // rdb_Last
            // 
            this.rdb_Last.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Last.AutoSize = true;
            this.rdb_Last.BackColor = System.Drawing.Color.Transparent;
            this.rdb_Last.Location = new System.Drawing.Point(540, 8);
            this.rdb_Last.Name = "rdb_Last";
            this.rdb_Last.Size = new System.Drawing.Size(116, 17);
            this.rdb_Last.TabIndex = 1;
            this.rdb_Last.Text = "اضافه به آخرین سند";
            this.rdb_Last.UseVisualStyleBackColor = false;
            this.rdb_Last.CheckedChanged += new System.EventHandler(this.rdb_Last_CheckedChanged);
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(540, 33);
            this.txt_To.Name = "txt_To";
            this.txt_To.Size = new System.Drawing.Size(116, 21);
            this.txt_To.TabIndex = 3;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_To_KeyPress);
            this.txt_To.Leave += new System.EventHandler(this.txt_To_Leave);
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.IsDefault = true;
            this.faDatePicker1.Location = new System.Drawing.Point(241, 5);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(164, 20);
            this.faDatePicker1.TabIndex = 4;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PCLOR.Properties.Resources.me_bg;
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_ExportDoc,
            this.bt_ViewDocs,
            this.toolStripSeparator2,
            this.toolStripSeparator3});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(794, 25);
            this.bindingNavigator1.TabIndex = 7;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bt_ExportDoc
            // 
            this.bt_ExportDoc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ExportDoc.Image = ((System.Drawing.Image)(resources.GetObject("bt_ExportDoc.Image")));
            this.bt_ExportDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ExportDoc.Name = "bt_ExportDoc";
            this.bt_ExportDoc.Size = new System.Drawing.Size(75, 22);
            this.bt_ExportDoc.Text = "صدور سند";
            this.bt_ExportDoc.ToolTipText = "Ctrl+S";
            this.bt_ExportDoc.Click += new System.EventHandler(this.bt_ExportDoc_Click);
            // 
            // bt_ViewDocs
            // 
            this.bt_ViewDocs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_ViewDocs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ViewDocs.Image = ((System.Drawing.Image)(resources.GetObject("bt_ViewDocs.Image")));
            this.bt_ViewDocs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ViewDocs.Name = "bt_ViewDocs";
            this.bt_ViewDocs.Size = new System.Drawing.Size(96, 22);
            this.bt_ViewDocs.Text = "مشاهده اسناد";
            this.bt_ViewDocs.ToolTipText = "Ctrl+W";
            this.bt_ViewDocs.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel0.Id = new System.Guid("112b9b45-b0d5-40fe-b926-70d3df0602ad");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("112b9b45-b0d5-40fe-b926-70d3df0602ad"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(788, 127), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("112b9b45-b0d5-40fe-b926-70d3df0602ad"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(3, 322);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(788, 127);
            this.uiPanel0.TabIndex = 4;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.explorerBar1);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 25);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(786, 101);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // explorerBar1
            // 
            this.explorerBar1.Columns = 3;
            this.explorerBar1.ColumnSeparation = 3;
            this.explorerBar1.Controls.Add(this.explorerBarContainerControl4);
            this.explorerBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explorerBar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            explorerBarGroup1.Container = true;
            explorerBarGroup1.ContainerControl = this.explorerBarContainerControl4;
            explorerBarGroup1.ContainerHeight = 67;
            explorerBarGroup1.Expandable = false;
            explorerBarGroup1.Key = "Group3";
            explorerBarGroup1.Text = "اطلاعات ثبت سند";
            this.explorerBar1.Groups.AddRange(new Janus.Windows.ExplorerBar.ExplorerBarGroup[] {
            explorerBarGroup1});
            this.explorerBar1.LeftMargin = 1;
            this.explorerBar1.Location = new System.Drawing.Point(0, 0);
            this.explorerBar1.Name = "explorerBar1";
            this.explorerBar1.RightMargin = 1;
            this.explorerBar1.Size = new System.Drawing.Size(786, 101);
            this.explorerBar1.TabIndex = 3;
            this.explorerBar1.VisualStyle = Janus.Windows.ExplorerBar.VisualStyle.Office2007;
            // 
            // gridEX1
            // 
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.AutomaticSort = false;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX1.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gridEX1.FocusCellFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Diagonal;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GroupByBoxVisible = false;
            gridEX1_Layout_0.IsCurrentLayout = true;
            gridEX1_Layout_0.Key = "PERP";
            gridEX1_Layout_0.LayoutString = resources.GetString("gridEX1_Layout_0.LayoutString");
            this.gridEX1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX1_Layout_0});
            this.gridEX1.Location = new System.Drawing.Point(3, 28);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RecordNavigator = true;
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SettingsKey = "Form01_AccDocuments1";
            this.gridEX1.Size = new System.Drawing.Size(788, 294);
            this.gridEX1.TabIndex = 12;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Moccasin;
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX1.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellValueChanged);
            this.gridEX1.CellEditCanceled += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellEditCanceled);
            this.gridEX1.CellUpdated += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellUpdated);
            // 
            // Form33_ExportDocForReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form33_ExportDocForReceipt";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "صدور سند رسید انبار";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form33_ExportDocForReceipt_FormClosing);
            this.Load += new System.EventHandler(this.Form33_ExportDocForReceipt_Load);
            this.explorerBarContainerControl4.ResumeLayout(false);
            this.explorerBarContainerControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).EndInit();
            this.explorerBar1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_ExportDoc;
        private System.Windows.Forms.ToolStripButton bt_ViewDocs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        protected Janus.Windows.ExplorerBar.ExplorerBar explorerBar1;
        private Janus.Windows.ExplorerBar.ExplorerBarContainerControl explorerBarContainerControl4;
        private Janus.Windows.GridEX.EditControls.EditBox txt_Cover;
        private System.Windows.Forms.RadioButton rdb_To;
        private System.Windows.Forms.RadioButton rdb_New;
        private System.Windows.Forms.RadioButton rdb_Last;
        private Janus.Windows.GridEX.EditControls.EditBox txt_To;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
    }
}