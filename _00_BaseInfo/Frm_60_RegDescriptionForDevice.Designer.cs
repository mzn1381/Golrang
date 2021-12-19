
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
            this.txtDescForDevice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDescForDevice
            // 
            this.txtDescForDevice.Location = new System.Drawing.Point(12, 33);
            this.txtDescForDevice.Multiline = true;
            this.txtDescForDevice.Name = "txtDescForDevice";
            this.txtDescForDevice.Size = new System.Drawing.Size(483, 88);
            this.txtDescForDevice.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(501, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "اظهارات";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.button1.Location = new System.Drawing.Point(12, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(573, 79);
            this.button1.TabIndex = 2;
            this.button1.Text = "ثبت ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Frm_60_RegDescriptionForDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 259);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescForDevice);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Name = "Frm_60_RegDescriptionForDevice";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "ثبت اظهارات دستگاه ";
            this.Load += new System.EventHandler(this.Frm_60_RegDescriptionForDevice_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescForDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}