using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kondate.soft
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
            //Application.Run(new Home());
            // Application.Run(new Main());

            Application.Run(new LOGIN());
            //  Application.Run(new kondate.soft.HOME12_license.HOME12_Set_license_02());
            //Application.Run(new kondate.soft.Panel_SETUP.Form_s001_03cus());
            //Application.Run(new kondate.soft.SETUP_02ACC.Home_SETUP_Enter_2ACC_19());
            //Application.Run(new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye());



        }
    }
}
