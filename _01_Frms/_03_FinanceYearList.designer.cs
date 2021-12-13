namespace PCLOR._01_Frms
{
    partial class _03_FinanceYearList
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
            Janus.Windows.GridEX.GridEXLayout gridEX2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_03_FinanceYearList));
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridEX2
            // 
            this.gridEX2.AllowColumnDrag = false;
            this.gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.ColumnAutoResize = true;
            gridEX2_DesignTimeLayout.LayoutString = resources.GetString("gridEX2_DesignTimeLayout.LayoutString");
            this.gridEX2.DesignTimeLayout = gridEX2_DesignTimeLayout;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.FilterRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.NewRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX2.NewRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX2.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.Size = new System.Drawing.Size(274, 262);
            this.gridEX2.TabIndex = 3;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2005;
            this.gridEX2.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX1_RowDoubleClick);
            // 
            // _03_FinanceYearList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 262);
            this.Controls.Add(this.gridEX2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "_03_FinanceYearList";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "سال مورد نظر را انتخاب کرده و دوبار کلیک کنید";
            this.Load += new System.EventHandler(this.Form05_FinanceYear_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.GridEX.GridEX gridEX2;

    }
}