using System;
using System.Drawing;
using System.Windows.Forms;


public class InputBox
{

    private static string returnFinalValue = String.Empty;


    public static string Show(string prompt, string title)
	{	
		InputBoxForm frm = new InputBoxForm(prompt,title);
        frm.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        frm.BackColor = Color.Lavender;
        frm.Closing += MyClosingEvent;
		frm.ShowDialog();
		
		return returnFinalValue;
	}
	
	public static string Show(string prompt)
	{
		return Show(prompt,"");
	}

	
	public class InputBoxForm:Form
    {
        public string returnValue = String.Empty;
        private Janus.Windows.EditControls.UIButton btnOk = new Janus.Windows.EditControls.UIButton();
        private Janus.Windows.EditControls.UIButton btnCancel;
		private  Janus.Windows.GridEX.EditControls.EditBox txt;
		private  Label lbl;
    

		private string prompt;
		private string title;
		
		public InputBoxForm(string prompt,string title)  //constructor
		{
			this.title = title;
			this.prompt = prompt;
		//	MyInitializeComponent();
            pro();
		}

		private void pro()
		{
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.Size = new Size(360,150);
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = title;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
		//	btnOk = new Button();
            btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            btnOk.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            btnOk.OfficeCustomColor = Color.LightPink;
            btnCancel = new Janus.Windows.EditControls.UIButton();
            btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            btnCancel.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            btnCancel.OfficeCustomColor = Color.LightPink;
            txt = new Janus.Windows.GridEX.EditControls.EditBox();
            txt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            txt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            txt.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
			lbl = new Label();
            lbl.Anchor = AnchorStyles.Right;
            lbl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.DoubleClick += FrmDblClick;// new EventHandler(FrmDblClick);
			//btnOk
			btnOk.Click += new EventHandler(btnOk_Click);
			btnOk.Location = new Point(280,8);
			btnOk.Size = new Size(64,24);
			btnOk.TabIndex = 1;
			btnOk.Text = "تأیید";
			//btnCancel
            btnCancel.Click += btnCancel_Click; 
			btnCancel.Location = new Point(280,38);
			btnCancel.Size = new Size(64,24);
			btnCancel.TabIndex = 2;
			btnCancel.Text = "لغو";
			//lbl
			lbl.Location = new Point(8,8);
			lbl.Size = new Size(254,48);
			lbl.Text = prompt;
			//txt
            txt.KeyPress += textBox1_KeyPress; //new  KeyPressEventHandler(textBox1_KeyPress);            
			txt.Location = new Point(8,88);
			txt.Size = new Size(336,20);
			txt.TabIndex = 0;
			txt.Text = String.Empty;
			Control[] ctrls = {btnOk,btnCancel,txt,lbl};
			this.Controls.AddRange(ctrls);  
		}		

        private void FrmDblClick(object sender, EventArgs e)
        {
            txt.Text = "Double Click On The Form";
        }
		private void btnOk_Click(object sender, EventArgs e)
		{
			returnValue = txt.Text;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			returnValue = String.Empty;
			this.Close();
		}
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(13)) btnOk_Click(sender, e);
        }

    
	}

    private static void MyClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
	{
        returnFinalValue = ((InputBoxForm)sender).returnValue;
        // چون کلاس فوق نقش ارسال کننده دارد پس از سندر استفاده می شود.
	}
}