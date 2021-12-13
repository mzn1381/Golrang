namespace PCLOR._03_Bank
{
    partial class Frm_140_Report_TransferBranch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_140_Report_TransferBranch));
            Janus.Windows.GridEX.GridEXLayout gridEX2_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.table_130_TransferBranchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_05_Product = new PCLOR.Product.DataSet_05_Product();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.tsexcel = new System.Windows.Forms.ToolStripButton();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.table_130_TransferBranchTableAdapter = new PCLOR.Product.DataSet_05_ProductTableAdapters.Table_130_TransferBranchTableAdapter();
            this.tableAdapterManager = new PCLOR.Product.DataSet_05_ProductTableAdapters.TableAdapterManager();
            this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.table_130_TransferBranchBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Product)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            this.SuspendLayout();
            // 
            // table_130_TransferBranchBindingSource
            // 
            this.table_130_TransferBranchBindingSource.DataMember = "Table_130_TransferBranch";
            this.table_130_TransferBranchBindingSource.DataSource = this.dataSet_05_Product;
            // 
            // dataSet_05_Product
            // 
            this.dataSet_05_Product.DataSetName = "DataSet_05_Product";
            this.dataSet_05_Product.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bindingNavigator1.BackgroundImage")));
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F);
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
            this.bindingNavigator1.Size = new System.Drawing.Size(784, 25);
            this.bindingNavigator1.TabIndex = 35;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // tsexcel
            // 
            this.tsexcel.Image = ((System.Drawing.Image)(resources.GetObject("tsexcel.Image")));
            this.tsexcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsexcel.Name = "tsexcel";
            this.tsexcel.Size = new System.Drawing.Size(98, 22);
            this.tsexcel.Text = "ارسال به اکسل";
            this.tsexcel.Click += new System.EventHandler(this.tsexcel_Click);
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
            this.gridEX2.DataSource = this.table_130_TransferBranchBindingSource;
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
            gridEX2_Layout_0.DataSource = this.table_130_TransferBranchBindingSource;
            gridEX2_Layout_0.IsCurrentLayout = true;
            gridEX2_Layout_0.Key = "PERP";
            gridEX2_Layout_0.LayoutString = resources.GetString("gridEX2_Layout_0.LayoutString");
            this.gridEX2.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX2_Layout_0});
            this.gridEX2.Location = new System.Drawing.Point(0, 25);
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
            this.gridEX2.SettingsKey = "Form09_TurnDoc_Receive22";
            this.gridEX2.ShowEmptyFields = false;
            this.gridEX2.Size = new System.Drawing.Size(784, 377);
            this.gridEX2.TabIndex = 36;
            this.gridEX2.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX2.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX2.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // table_130_TransferBranchTableAdapter
            // 
            this.table_130_TransferBranchTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_100_ProgramMachineTableAdapter = null;
            this.tableAdapterManager.Table_105_DefinitionWorkShiftTableAdapter = null;
            this.tableAdapterManager.Table_110_ReportDeviceFailureTableAdapter = null;
            this.tableAdapterManager.Table_115_ProductTableAdapter = null;
            this.tableAdapterManager.Table_120_TypeCottonTableAdapter = null;
            this.tableAdapterManager.Table_125_DetailTypeCottonTableAdapter = null;
            this.tableAdapterManager.Table_126_DetailTypeCottonProductTableAdapter = null;
            this.tableAdapterManager.Table_130_TransferBranchTableAdapter = this.table_130_TransferBranchTableAdapter;
            this.tableAdapterManager.Table_135_RFIDPersonTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PCLOR.Product.DataSet_05_ProductTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "e";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            // 
            // Frm_140_Report_TransferBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 402);
            this.Controls.Add(this.gridEX2);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_140_Report_TransferBranch";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش انتقال بین شعب";
            this.Load += new System.EventHandler(this.Frm_140_Report_TransferBranch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table_130_TransferBranchBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Product)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton tsexcel;
        private Janus.Windows.GridEX.GridEX gridEX2;
        private Product.DataSet_05_Product dataSet_05_Product;
        private System.Windows.Forms.BindingSource table_130_TransferBranchBindingSource;
        private Product.DataSet_05_ProductTableAdapters.Table_130_TransferBranchTableAdapter table_130_TransferBranchTableAdapter;
        private Product.DataSet_05_ProductTableAdapters.TableAdapterManager tableAdapterManager;
        private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}