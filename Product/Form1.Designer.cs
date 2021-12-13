namespace PCLOR.Product
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.table_100_ProgramMachineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_05_Product = new PCLOR.Product.DataSet_05_Product();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.table_100_ProgramMachineTableAdapter = new PCLOR.Product.DataSet_05_ProductTableAdapters.Table_100_ProgramMachineTableAdapter();
            this.tableAdapterManager = new PCLOR.Product.DataSet_05_ProductTableAdapters.TableAdapterManager();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.table_125_DetailTypeCottonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_125_DetailTypeCottonTableAdapter = new PCLOR.Product.DataSet_05_ProductTableAdapters.Table_125_DetailTypeCottonTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.table_100_ProgramMachineBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Product)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_125_DetailTypeCottonBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(624, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.table_100_ProgramMachineBindingSource, "ID", true));
            this.textBox1.Location = new System.Drawing.Point(575, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // table_100_ProgramMachineBindingSource
            // 
            this.table_100_ProgramMachineBindingSource.DataMember = "Table_100_ProgramMachine";
            this.table_100_ProgramMachineBindingSource.DataSource = this.dataSet_05_Product;
            // 
            // dataSet_05_Product
            // 
            this.dataSet_05_Product.DataSetName = "DataSet_05_Product";
            this.dataSet_05_Product.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.DataSource = this.table_125_DetailTypeCottonBindingSource;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Location = new System.Drawing.Point(51, 62);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.Size = new System.Drawing.Size(400, 376);
            this.gridEX1.TabIndex = 2;
            this.gridEX1.Enter += new System.EventHandler(this.gridEX1_Enter);
            // 
            // table_100_ProgramMachineTableAdapter
            // 
            this.table_100_ProgramMachineTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_100_ProgramMachineTableAdapter = this.table_100_ProgramMachineTableAdapter;
            this.tableAdapterManager.Table_105_DefinitionWorkShiftTableAdapter = null;
            this.tableAdapterManager.Table_110_ReportDeviceFailureTableAdapter = null;
            this.tableAdapterManager.Table_115_ProductTableAdapter = null;
            this.tableAdapterManager.Table_120_TypeCottonTableAdapter = null;
            this.tableAdapterManager.Table_125_DetailTypeCottonTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PCLOR.Product.DataSet_05_ProductTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.table_100_ProgramMachineBindingSource, "Number", true));
            this.textBox2.Location = new System.Drawing.Point(565, 122);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 3;
            // 
            // table_125_DetailTypeCottonBindingSource
            // 
            this.table_125_DetailTypeCottonBindingSource.DataMember = "FK_Table_125_DetailTypeCotton_Table_100_ProgramMachine";
            this.table_125_DetailTypeCottonBindingSource.DataSource = this.table_100_ProgramMachineBindingSource;
            // 
            // table_125_DetailTypeCottonTableAdapter
            // 
            this.table_125_DetailTypeCottonTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table_100_ProgramMachineBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Product)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_125_DetailTypeCottonBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private DataSet_05_Product dataSet_05_Product;
        private System.Windows.Forms.BindingSource table_100_ProgramMachineBindingSource;
        private DataSet_05_ProductTableAdapters.Table_100_ProgramMachineTableAdapter table_100_ProgramMachineTableAdapter;
        private DataSet_05_ProductTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.BindingSource table_125_DetailTypeCottonBindingSource;
        private DataSet_05_ProductTableAdapters.Table_125_DetailTypeCottonTableAdapter table_125_DetailTypeCottonTableAdapter;
    }
}