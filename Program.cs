using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PCLOR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_Main("گلرنگ", "admin", "1400", 1, true, false, "Data Source=.;Initial Catalog=PCLOR_1_1400;Persist Security Info=True;User ID=sa;Password=Pars@63"));

            //Application.Run(new _01_Frms._01_Login());
            //Application.Run(new Form1());
        }
    }
}
