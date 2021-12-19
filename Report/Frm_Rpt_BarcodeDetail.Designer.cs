namespace PCLOR.Report
{
    partial class Frm_Rpt_BarcodeDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Rpt_BarcodeDetail));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.tsexcel = new System.Windows.Forms.ToolStripButton();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.SuspendLayout();
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
            this.tsexcel});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(740, 27);
            this.bindingNavigator1.TabIndex = 13;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // tsexcel
            // 
            this.tsexcel.Image = ((System.Drawing.Image)(resources.GetObject("tsexcel.Image")));
            this.tsexcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsexcel.Name = "tsexcel";
            this.tsexcel.Size = new System.Drawing.Size(122, 24);
            this.tsexcel.Text = "ارسال به اکسل";
            this.tsexcel.Click += new System.EventHandler(this.tsexcel_Click);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
            this.gridEX1.ColumnAutoSizeMode = Janus.Windows.GridEX.ColumnAutoSizeMode.DisplayedCellsAndHeader;
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
            this.gridEX1.Location = new System.Drawing.Point(3, 30);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SettingsKey = "gridfrm_3";
            this.gridEX1.Size = new System.Drawing.Size(734, 474);
            this.gridEX1.TabIndex = 26;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.AliceBlue;
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.UseCompatibleTextRendering = false;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "e";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            // 
            // Frm_Rpt_BarcodeDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 507);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_Rpt_BarcodeDetail";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش بارکد های ثبت نشده";
            this.Load += new System.EventHandler(this.Frm_Rpt_BarcodeDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton tsexcel;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}