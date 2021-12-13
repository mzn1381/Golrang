
namespace PCLOR.Product
{
    partial class Frm_005_SelectMachine
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
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_005_SelectMachine));
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.table_60_SpecsTechnicalBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_05_PCLOR = new PCLOR.data_PCLOR.DataSet_05_PCLOR();
            this.table_60_SpecsTechnicalTableAdapter = new PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.Table_60_SpecsTechnicalTableAdapter();
            this.tableAdapterManager = new PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_60_SpecsTechnicalBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_PCLOR)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            // 
            // gridEX1
            // 
            this.gridEX1.CardWidth = 180;
            this.gridEX1.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            this.gridEX1.DataSource = this.table_60_SpecsTechnicalBindingSource;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EmptyGridInfoAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.gridEX1.GridLines = Janus.Windows.GridEX.GridLines.Default;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.Location = new System.Drawing.Point(3, 3);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridEX1.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.gridEX1.Size = new System.Drawing.Size(734, 501);
            this.gridEX1.TabIndex = 4;
            this.gridEX1.View = Janus.Windows.GridEX.View.CardView;
            this.gridEX1.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridEX1_FormattingRow);
            this.gridEX1.ColumnButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_ColumnButtonClick);
            // 
            // table_60_SpecsTechnicalBindingSource
            // 
            this.table_60_SpecsTechnicalBindingSource.DataMember = "Table_60_SpecsTechnical";
            this.table_60_SpecsTechnicalBindingSource.DataSource = this.dataSet_05_PCLOR;
            // 
            // dataSet_05_PCLOR
            // 
            this.dataSet_05_PCLOR.DataSetName = "DataSet_05_PCLOR";
            this.dataSet_05_PCLOR.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_60_SpecsTechnicalTableAdapter
            // 
            this.table_60_SpecsTechnicalTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_005_TypeClothTableAdapter = null;
            this.tableAdapterManager.Table_010_TypeColorTableAdapter = null;
            this.tableAdapterManager.Table_015_FormulColorTableAdapter = null;
            this.tableAdapterManager.Table_020_DetailReciptClothRawTableAdapter = null;
            this.tableAdapterManager.Table_020_HeaderReciptClothRowTableAdapter = null;
            this.tableAdapterManager.Table_025_HederOrderColorTableAdapter = null;
            this.tableAdapterManager.Table_030_DetailOrderColorTableAdapter = null;
            this.tableAdapterManager.Table_035_ProductionTableAdapter = null;
            this.tableAdapterManager.Table_050_Packaging1TableAdapter = null;
            this.tableAdapterManager.Table_050_PackagingTableAdapter = null;
            this.tableAdapterManager.Table_055_ColorDefinitionTableAdapter = null;
            this.tableAdapterManager.Table_100_KnittingTableAdapter = null;
            this.tableAdapterManager.Table_105_ProductionTableAdapter = null;
            this.tableAdapterManager.Table_110_ProductionDetailTableAdapter = null;
            this.tableAdapterManager.Table_115_ProductionColorTableAdapter = null;
            this.tableAdapterManager.Table_40_ColorPrductionTableAdapter = null;
            this.tableAdapterManager.Table_45_EditeProductTableAdapter = null;
            this.tableAdapterManager.Table_60_SpecsTechnicalTableAdapter = this.table_60_SpecsTechnicalTableAdapter;
            this.tableAdapterManager.Table_65_HeaderOtherPWHRS1TableAdapter = null;
            this.tableAdapterManager.Table_65_HeaderOtherPWHRSTableAdapter = null;
            this.tableAdapterManager.Table_70_DetailOtherPWHRS1TableAdapter = null;
            this.tableAdapterManager.Table_70_DetailOtherPWHRSTableAdapter = null;
            this.tableAdapterManager.Table_80_SettingTableAdapter = null;
            this.tableAdapterManager.Table_85_BranchsTableAdapter = null;
            this.tableAdapterManager.Table_90_Wares1TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares2TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares3TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares4TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares5TableAdapter = null;
            this.tableAdapterManager.Table_90_Wares6TableAdapter = null;
            this.tableAdapterManager.Table_90_WaresTableAdapter = null;
            this.tableAdapterManager.Table_95_DetailWareTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PCLOR.data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // Frm_005_SelectMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 507);
            this.Controls.Add(this.gridEX1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_005_SelectMachine";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "انتخاب دستگاه";
            this.Load += new System.EventHandler(this.Frm_005_SelectMachine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_60_SpecsTechnicalBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_PCLOR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private data_PCLOR.DataSet_05_PCLOR dataSet_05_PCLOR;
        private System.Windows.Forms.BindingSource table_60_SpecsTechnicalBindingSource;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.Table_60_SpecsTechnicalTableAdapter table_60_SpecsTechnicalTableAdapter;
        private data_PCLOR.DataSet_05_PCLORTableAdapters.TableAdapterManager tableAdapterManager;
    }
}