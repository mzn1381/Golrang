namespace PCLOR._03_Bank
{
    partial class Form01_PrintRecChq
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
            Janus.Windows.GridEX.GridEXLayout mlt_Person_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form01_PrintRecChq));
            Janus.Windows.Common.JanusColorScheme janusColorScheme1 = new Janus.Windows.Common.JanusColorScheme();
            Janus.Windows.ExplorerBar.ExplorerBarGroup explorerBarGroup1 = new Janus.Windows.ExplorerBar.ExplorerBarGroup();
            this.explorerBarContainerControl1 = new Janus.Windows.ExplorerBar.ExplorerBarContainerControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.btnList = new DevComponents.DotNetBar.ButtonItem();
            this.btnPrintOrdr = new DevComponents.DotNetBar.ButtonItem();
            this.btnPrintWithOrdr = new DevComponents.DotNetBar.ButtonItem();
            this.Chk_NoLogo = new System.Windows.Forms.CheckBox();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.mlt_Person = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.visualStyleManager1 = new Janus.Windows.Common.VisualStyleManager(this.components);
            this.rdb_FromPerson = new Janus.Windows.EditControls.UICheckBox();
            this.txt_FromNum = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_This = new Janus.Windows.EditControls.UIRadioButton();
            this.txt_ToNumber = new Janus.Windows.GridEX.EditControls.EditBox();
            this.fa_Date2 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.rdb_FromNumber = new Janus.Windows.EditControls.UIRadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.fa_Date1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.rdb_FromDate = new Janus.Windows.EditControls.UIRadioButton();
            this.explorerBar1 = new Janus.Windows.ExplorerBar.ExplorerBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Reports = new PACNT._4_Reports.DataSet_Reports();
            this.explorerBarContainerControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Person)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).BeginInit();
            this.explorerBar1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Reports)).BeginInit();
            this.SuspendLayout();
            // 
            // explorerBarContainerControl1
            // 
            this.explorerBarContainerControl1.Controls.Add(this.tableLayoutPanel1);
            this.explorerBarContainerControl1.Location = new System.Drawing.Point(4, 7);
            this.explorerBarContainerControl1.Name = "explorerBarContainerControl1";
            this.explorerBarContainerControl1.Size = new System.Drawing.Size(786, 101);
            this.explorerBarContainerControl1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.61832F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(786, 101);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonX1);
            this.panel2.Controls.Add(this.Chk_NoLogo);
            this.panel2.Controls.Add(this.bt_Display);
            this.panel2.Controls.Add(this.mlt_Person);
            this.panel2.Controls.Add(this.rdb_FromPerson);
            this.panel2.Controls.Add(this.txt_FromNum);
            this.panel2.Controls.Add(this.rdb_This);
            this.panel2.Controls.Add(this.txt_ToNumber);
            this.panel2.Controls.Add(this.fa_Date2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.rdb_FromNumber);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.fa_Date1);
            this.panel2.Controls.Add(this.rdb_FromDate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(780, 95);
            this.panel2.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.BackColor = System.Drawing.Color.Transparent;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX1.Location = new System.Drawing.Point(32, 59);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor();
            this.buttonX1.Size = new System.Drawing.Size(167, 25);
            this.buttonX1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnList,
            this.btnPrintOrdr,
            this.btnPrintWithOrdr});
            this.buttonX1.TabIndex = 10;
            this.buttonX1.Text = "طراحی چاپ";
            // 
            // btnList
            // 
            this.btnList.BeginGroup = true;
            this.btnList.GlobalItem = false;
            this.btnList.Name = "btnList";
            this.btnList.Text = "طرح چاپ چک های پرداختی به شخص";
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnPrintOrdr
            // 
            this.btnPrintOrdr.Name = "btnPrintOrdr";
            this.btnPrintOrdr.Text = "طرح چاپ چک جاری";
            this.btnPrintOrdr.Click += new System.EventHandler(this.btnPrintOrdr_Click);
            // 
            // btnPrintWithOrdr
            // 
            this.btnPrintWithOrdr.GlobalItem = false;
            this.btnPrintWithOrdr.Name = "btnPrintWithOrdr";
            this.btnPrintWithOrdr.Text = "طرح چاپ چک در بازه زمانی و شماره";
            this.btnPrintWithOrdr.Click += new System.EventHandler(this.btnPrintWithOrdr_Click);
            // 
            // Chk_NoLogo
            // 
            this.Chk_NoLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Chk_NoLogo.AutoSize = true;
            this.Chk_NoLogo.BackColor = System.Drawing.Color.Transparent;
            this.Chk_NoLogo.Location = new System.Drawing.Point(206, 33);
            this.Chk_NoLogo.Name = "Chk_NoLogo";
            this.Chk_NoLogo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Chk_NoLogo.Size = new System.Drawing.Size(148, 17);
            this.Chk_NoLogo.TabIndex = 3;
            this.Chk_NoLogo.Text = "چاپ بدون لوگو و نام شرکت";
            this.Chk_NoLogo.UseVisualStyleBackColor = false;
            // 
            // bt_Display
            // 
            this.bt_Display.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bt_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Display.BackColor = System.Drawing.Color.Transparent;
            this.bt_Display.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.bt_Display.Location = new System.Drawing.Point(32, 33);
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(167, 20);
            this.bt_Display.TabIndex = 9;
            this.bt_Display.Text = "نمایش";
            this.bt_Display.Tooltip = "Ctrl+D";
            this.bt_Display.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // mlt_Person
            // 
            this.mlt_Person.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Person_DesignTimeLayout.LayoutString = resources.GetString("mlt_Person_DesignTimeLayout.LayoutString");
            this.mlt_Person.DesignTimeLayout = mlt_Person_DesignTimeLayout;
            this.mlt_Person.DisplayMember = "Column02";
            this.mlt_Person.Location = new System.Drawing.Point(32, 6);
            this.mlt_Person.Name = "mlt_Person";
            this.mlt_Person.SelectedIndex = -1;
            this.mlt_Person.SelectedItem = null;
            this.mlt_Person.Size = new System.Drawing.Size(167, 21);
            this.mlt_Person.TabIndex = 8;
            this.mlt_Person.ValueMember = "ColumnId";
            this.mlt_Person.VisualStyleManager = this.visualStyleManager1;
            this.mlt_Person.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_BesProject_KeyPress);
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
            // rdb_FromPerson
            // 
            this.rdb_FromPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_FromPerson.AutoSize = true;
            this.rdb_FromPerson.BackColor = System.Drawing.Color.Transparent;
            this.rdb_FromPerson.Location = new System.Drawing.Point(195, 6);
            this.rdb_FromPerson.Name = "rdb_FromPerson";
            this.rdb_FromPerson.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.rdb_FromPerson.Size = new System.Drawing.Size(159, 18);
            this.rdb_FromPerson.TabIndex = 7;
            this.rdb_FromPerson.Text = "چکهای دریافت شده از شخص:";
            this.rdb_FromPerson.VisualStyleManager = this.visualStyleManager1;
            this.rdb_FromPerson.CheckedChanged += new System.EventHandler(this.rdb_FromPerson_CheckedChanged);
            // 
            // txt_FromNum
            // 
            this.txt_FromNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_FromNum.Location = new System.Drawing.Point(580, 28);
            this.txt_FromNum.Name = "txt_FromNum";
            this.txt_FromNum.Size = new System.Drawing.Size(102, 21);
            this.txt_FromNum.TabIndex = 2;
            this.txt_FromNum.VisualStyleManager = this.visualStyleManager1;
            this.txt_FromNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editBox1_KeyPress);
            // 
            // rdb_This
            // 
            this.rdb_This.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_This.AutoSize = true;
            this.rdb_This.BackColor = System.Drawing.Color.Transparent;
            this.rdb_This.Checked = true;
            this.rdb_This.Location = new System.Drawing.Point(695, 6);
            this.rdb_This.Name = "rdb_This";
            this.rdb_This.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.rdb_This.Size = new System.Drawing.Size(69, 18);
            this.rdb_This.TabIndex = 0;
            this.rdb_This.TabStop = true;
            this.rdb_This.Text = "همین چک";
            this.rdb_This.VisualStyleManager = this.visualStyleManager1;
            this.rdb_This.CheckedChanged += new System.EventHandler(this.rdb_This_CheckedChanged);
            // 
            // txt_ToNumber
            // 
            this.txt_ToNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_ToNumber.Location = new System.Drawing.Point(385, 29);
            this.txt_ToNumber.Name = "txt_ToNumber";
            this.txt_ToNumber.Size = new System.Drawing.Size(114, 21);
            this.txt_ToNumber.TabIndex = 2;
            this.txt_ToNumber.VisualStyleManager = this.visualStyleManager1;
            this.txt_ToNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editBox1_KeyPress);
            // 
            // fa_Date2
            // 
            this.fa_Date2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fa_Date2.Location = new System.Drawing.Point(385, 55);
            this.fa_Date2.Name = "fa_Date2";
            this.fa_Date2.Size = new System.Drawing.Size(143, 20);
            this.fa_Date2.TabIndex = 6;
            this.fa_Date2.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.fa_Date2.TextChanged += new System.EventHandler(this.fa_Date1_TextChanged);
            this.fa_Date2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fa_Date1_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(528, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "تا تاریخ:";
            // 
            // rdb_FromNumber
            // 
            this.rdb_FromNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_FromNumber.AutoSize = true;
            this.rdb_FromNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_FromNumber.Location = new System.Drawing.Point(681, 30);
            this.rdb_FromNumber.Name = "rdb_FromNumber";
            this.rdb_FromNumber.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.rdb_FromNumber.Size = new System.Drawing.Size(83, 18);
            this.rdb_FromNumber.TabIndex = 1;
            this.rdb_FromNumber.Text = "از پشت نمره:";
            this.rdb_FromNumber.VisualStyleManager = this.visualStyleManager1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(501, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "تا پشت نمره:";
            // 
            // fa_Date1
            // 
            this.fa_Date1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fa_Date1.Location = new System.Drawing.Point(580, 52);
            this.fa_Date1.Name = "fa_Date1";
            this.fa_Date1.Size = new System.Drawing.Size(131, 20);
            this.fa_Date1.TabIndex = 5;
            this.fa_Date1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.fa_Date1.TextChanged += new System.EventHandler(this.fa_Date1_TextChanged);
            this.fa_Date1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fa_Date1_KeyPress);
            // 
            // rdb_FromDate
            // 
            this.rdb_FromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_FromDate.AutoSize = true;
            this.rdb_FromDate.BackColor = System.Drawing.Color.Transparent;
            this.rdb_FromDate.Location = new System.Drawing.Point(708, 54);
            this.rdb_FromDate.Name = "rdb_FromDate";
            this.rdb_FromDate.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.rdb_FromDate.Size = new System.Drawing.Size(56, 18);
            this.rdb_FromDate.TabIndex = 4;
            this.rdb_FromDate.Text = "از تاریخ:";
            this.rdb_FromDate.VisualStyleManager = this.visualStyleManager1;
            // 
            // explorerBar1
            // 
            this.explorerBar1.Controls.Add(this.explorerBarContainerControl1);
            this.explorerBar1.Dock = System.Windows.Forms.DockStyle.Top;
            explorerBarGroup1.Container = true;
            explorerBarGroup1.ContainerControl = this.explorerBarContainerControl1;
            explorerBarGroup1.ContainerHeight = 102;
            explorerBarGroup1.HeaderHeight = 0;
            explorerBarGroup1.Key = "Group1";
            this.explorerBar1.Groups.AddRange(new Janus.Windows.ExplorerBar.ExplorerBarGroup[] {
            explorerBarGroup1});
            this.explorerBar1.LeftMargin = 2;
            this.explorerBar1.Location = new System.Drawing.Point(0, 0);
            this.explorerBar1.Name = "explorerBar1";
            this.explorerBar1.OfficeColorScheme = Janus.Windows.ExplorerBar.OfficeColorScheme.Custom;
            this.explorerBar1.RightMargin = 2;
            this.explorerBar1.Size = new System.Drawing.Size(794, 114);
            this.explorerBar1.TabIndex = 1;
            this.explorerBar1.VisualStyleManager = this.visualStyleManager1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stiViewerControl1);
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 114);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(794, 338);
            this.panel1.TabIndex = 2;
            // 
            // stiViewerControl1
            // 
            this.stiViewerControl1.AllowDrop = true;
            this.stiViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stiViewerControl1.Location = new System.Drawing.Point(0, 0);
            this.stiViewerControl1.Name = "stiViewerControl1";
            this.stiViewerControl1.Report = null;
            this.stiViewerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stiViewerControl1.ShowZoom = true;
            this.stiViewerControl1.Size = new System.Drawing.Size(794, 338);
            this.stiViewerControl1.TabIndex = 3;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableDrillDown = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(794, 338);
            this.crystalReportViewer1.TabIndex = 1;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            this.crystalReportViewer1.Visible = false;
            // 
            // dataSet_Reports
            // 
            this.dataSet_Reports.DataSetName = "DataSet_Reports";
            this.dataSet_Reports.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Form01_PrintRecChq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.explorerBar1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form01_PrintRecChq";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ برگه دریافت چک";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form01_PrintRecChq_FormClosing);
            this.Load += new System.EventHandler(this.Form01_PrintAccDoc_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form01_PrintAccDoc_KeyDown);
            this.explorerBarContainerControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Person)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).EndInit();
            this.explorerBar1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Reports)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.Common.VisualStyleManager visualStyleManager1;
        private Janus.Windows.ExplorerBar.ExplorerBarContainerControl explorerBarContainerControl1;
        private Janus.Windows.ExplorerBar.ExplorerBar explorerBar1;
        private Janus.Windows.EditControls.UIRadioButton rdb_FromNumber;
        private Janus.Windows.GridEX.EditControls.EditBox txt_ToNumber;
        private Janus.Windows.GridEX.EditControls.EditBox txt_FromNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIRadioButton rdb_FromDate;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.ButtonX bt_Display;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private FarsiLibrary.Win.Controls.FADatePicker fa_Date2;
        private FarsiLibrary.Win.Controls.FADatePicker fa_Date1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Person;
        private System.Windows.Forms.BindingSource bindingSource1;
        private Janus.Windows.EditControls.UIRadioButton rdb_This;
        private Janus.Windows.EditControls.UICheckBox rdb_FromPerson;
        private PACNT._4_Reports.DataSet_Reports dataSet_Reports;
        private System.Windows.Forms.CheckBox Chk_NoLogo;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonItem btnList;
        private DevComponents.DotNetBar.ButtonItem btnPrintOrdr;
        private DevComponents.DotNetBar.ButtonItem btnPrintWithOrdr;


    }
}