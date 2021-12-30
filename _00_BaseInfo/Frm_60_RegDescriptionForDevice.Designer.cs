
namespace PCLOR._00_BaseInfo
{
    partial class Frm_60_RegDescriptionForDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_60_RegDescriptionForDevice));
            Janus.Windows.GridEX.GridEXLayout gridEX2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.txtDescForDevice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCodeTag = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkIsDefective = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.Table_135_BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pCLOR_1_1400DataSet = new PCLOR.PCLOR_1_1400DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Table_135_BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCLOR_1_1400DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDescForDevice
            // 
            this.txtDescForDevice.Font = new System.Drawing.Font("Tahoma", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtDescForDevice.Location = new System.Drawing.Point(36, 12);
            this.txtDescForDevice.Multiline = true;
            this.txtDescForDevice.Name = "txtDescForDevice";
            this.txtDescForDevice.Size = new System.Drawing.Size(1121, 88);
            this.txtDescForDevice.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(927, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(420, 36);
            this.label1.TabIndex = 1;
            this.label1.Text = "دستگاه دارای مشکل فنی است ";
            // 
            // txtCodeTag
            // 
            this.txtCodeTag.Font = new System.Drawing.Font("Tahoma", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtCodeTag.Location = new System.Drawing.Point(36, 106);
            this.txtCodeTag.Multiline = true;
            this.txtCodeTag.Name = "txtCodeTag";
            this.txtCodeTag.Size = new System.Drawing.Size(693, 88);
            this.txtCodeTag.TabIndex = 0;
            this.txtCodeTag.TextChanged += new System.EventHandler(this.txtCodeTag_TextChanged);
            this.txtCodeTag.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodeTag_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(729, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "شماره کارت :";
            // 
            // checkIsDefective
            // 
            this.checkIsDefective.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.checkIsDefective.Location = new System.Drawing.Point(900, 144);
            this.checkIsDefective.Name = "checkIsDefective";
            this.checkIsDefective.Size = new System.Drawing.Size(21, 29);
            this.checkIsDefective.TabIndex = 2;
            this.checkIsDefective.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label3.Location = new System.Drawing.Point(1163, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 36);
            this.label3.TabIndex = 1;
            this.label3.Text = "اظهارات :";
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanel0.Id = new System.Guid("029ed858-c2cf-4e14-af93-ab7a308b17fe");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("029ed858-c2cf-4e14-af93-ab7a308b17fe"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(1273, 392), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("029ed858-c2cf-4e14-af93-ab7a308b17fe"), new System.Drawing.Point(684, 491), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(684, 491);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(3, 208);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(1273, 392);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Panel 0";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX2);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 27);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(1271, 364);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX2.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.BuiltInTextsData = resources.GetString("gridEX2.BuiltInTextsData");
            this.gridEX2.ColumnAutoResize = true;
            this.gridEX2.ColumnAutoSizeMode = Janus.Windows.GridEX.ColumnAutoSizeMode.DisplayedCellsAndHeader;
            this.gridEX2.DataSource = this.Table_135_BindingSource;
            gridEX2_DesignTimeLayout.LayoutString = resources.GetString("gridEX2_DesignTimeLayout.LayoutString");
            this.gridEX2.DesignTimeLayout = gridEX2_DesignTimeLayout;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX2.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX2.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX2.FilterRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX2.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.LavenderBlush;
            this.gridEX2.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX2.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.ScrollBars = Janus.Windows.GridEX.ScrollBars.Both;
            this.gridEX2.SettingsKey = "Frm_15_InfoServiceGrid_61";
            this.gridEX2.Size = new System.Drawing.Size(1271, 364);
            this.gridEX2.TabIndex = 2;
            this.gridEX2.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX2.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX2.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX2.UseCompatibleTextRendering = false;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX2.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridEX2_FormattingRow);
            // 
            // Table_135_BindingSource
            // 
            this.Table_135_BindingSource.AllowNew = true;
            this.Table_135_BindingSource.DataMember = "Table_135_RpeortDevices";
            this.Table_135_BindingSource.DataSource = this.pCLOR_1_1400DataSet;
            // 
            // pCLOR_1_1400DataSet
            // 
            this.pCLOR_1_1400DataSet.DataSetName = "PCLOR_1_1400DataSet";
            this.pCLOR_1_1400DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Frm_60_RegDescriptionForDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 603);
            this.Controls.Add(this.checkIsDefective);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCodeTag);
            this.Controls.Add(this.txtDescForDevice);
            this.Controls.Add(this.uiPanel0);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Frm_60_RegDescriptionForDevice";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ثبت اظهارات دستگاه ";
            this.Load += new System.EventHandler(this.Frm_60_RegDescriptionForDevice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Table_135_BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCLOR_1_1400DataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescForDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCodeTag;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkIsDefective;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private System.Windows.Forms.BindingSource Table_135_BindingSource;
        private PCLOR_1_1400DataSet pCLOR_1_1400DataSet;
        private Janus.Windows.GridEX.GridEX gridEX2;
    }
}