using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace kondate.soft
{
    public partial class Form1 : Form
    {
        //Move Form ====================================
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //Resize Form ===================================
 
        //============================================

        public Form1()
        {
            InitializeComponent();
            this.panel_Enterprise_manager_Sub.Visible = false;
        }

        private void show_menu(Panel panel)
        {
            if (panel.Visible == false)
            {
                panel.Visible = true;
            }
            else
            {
                panel.Visible = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.panel_left.Width = 53;
        }

        private void BtnSlide_Click(object sender, EventArgs e)
        {
            if (this.panel_left.Width == 250 )
            {
                this.panel_left.Width = 53;
            }
            else
            {
                this.panel_left.Width = 250;
            }
        }

        private void iblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnEnterPrise_manager_Click(object sender, EventArgs e)
        {
            this.BtnEnterPrise_manager.Visible = false;
            if (this.BtnEnterPrise_manager_false.Visible == false)
            {
                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnEnterPrise_manager_false.Visible = false;
            }
            this.panel_left.Width = 250;
            if (this.panel_Enterprise_manager_Sub.Visible == true)
            {
                this.BtnEnterPrise_manager.Visible = true;
                this.BtnEnterPrise_manager_false.Visible = false;
                this.panel_Enterprise_manager_Sub.Visible = false;

            }
            else
            {
                this.panel_Enterprise_manager_Sub.Visible = true;
                this.BtnEnterPrise_manager.Visible = true;
                this.BtnEnterPrise_manager_false.Visible = false;
            }

        }
        private void BtnEnterPrise_manager_false_Click(object sender, EventArgs e)
        {
            this.BtnEnterPrise_manager_false.Visible = false;
            if (this.BtnEnterPrise_manager.Visible == false)
            {
                this.BtnEnterPrise_manager.Visible = true;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnEnterPrise_manager.Visible = false;
            }

            this.panel_left.Width = 250;
            if (this.panel_Enterprise_manager_Sub.Visible == false)
            {
                this.BtnEnterPrise_manager.Visible = true;
                this.BtnEnterPrise_manager_false.Visible = false;
                this.panel_Enterprise_manager_Sub.Visible = true;

            }
            else
            {
                this.panel_Enterprise_manager_Sub.Visible = false;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnterPrise_manager_false.Visible = true;
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel_left_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void BtnEnter_PR_Click(object sender, EventArgs e)
        {
            this.BtnEnter_PR.Visible = false;
            if (this.BtnEnter_PR_f.Visible == false)
            {
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnEnter_PR_f.Visible = false;
            }

        }

        private void BtnEnter_PR_f_Click(object sender, EventArgs e)
        {
            this.BtnEnter_PR_f.Visible = false;
            if (this.BtnEnter_PR.Visible == false)
            {
                this.BtnEnter_PR.Visible = true;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnEnter_PR.Visible = false;
            }
        }

        private void BtnEnter_AC_Click(object sender, EventArgs e)
        {
            this.BtnEnter_AC.Visible = false;
            if (this.BtnEnter_AC_f.Visible == false)
            {
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnEnter_AC_f.Visible = false;
            }
        }

        private void BtnEnter_AC_f_Click(object sender, EventArgs e)
        {
            this.BtnEnter_AC_f.Visible = false;
            if (this.BtnEnter_AC.Visible == false)
            {
                this.BtnEnter_AC.Visible = true;

                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnEnter_AC.Visible = false;
            }
        }

        private void BtnEnter_Mem_Click(object sender, EventArgs e)
        {
            this.BtnEnter_Mem.Visible = false;
            if (this.BtnEnter_Mem_f.Visible == false)
            {
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnEnter_Mem_f.Visible = false;
            }
        }

        private void BtnEnter_Mem_f_Click(object sender, EventArgs e)
        {
            this.BtnEnter_Mem_f.Visible = false;
            if (this.BtnEnter_Mem.Visible == false)
            {
                this.BtnEnter_Mem.Visible = true;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnEnter_Mem.Visible = false;
            }
        }

        private void BtnEnter_WH_Click(object sender, EventArgs e)
        {
            this.BtnEnter_WH.Visible = false;
            if (this.BtnEnter_WH_f.Visible == false)
            {
                this.BtnEnter_WH_f.Visible = true;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnEnter_WH_f.Visible = false;
            }
        }

        private void BtnEnter_WH_f_Click(object sender, EventArgs e)
        {
            this.BtnEnter_WH_f.Visible = false;
            if (this.BtnEnter_WH.Visible == false)
            {
                this.BtnEnter_WH.Visible = true;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnEnter_WH.Visible = false;
            }
        }

        private void BtnPR_Click(object sender, EventArgs e)
        {
            this.BtnPR.Visible = false;
            if (this.BtnPR_f.Visible == false)
            {
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnPR_f.Visible = false;
            }
        }

        private void BtnPR_f_Click(object sender, EventArgs e)
        {
            this.BtnPR_f.Visible = false;
            if (this.BtnPR.Visible == false)
            {
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = true;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnPR.Visible = false;
            }
        }

        private void BtnPD_Click(object sender, EventArgs e)
        {
            this.BtnPD.Visible = false;
            if (this.BtnPD_f.Visible == false)
            {
                this.BtnPD_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnPD_f.Visible = false;
            }
        }

        private void BtnPD_f_Click(object sender, EventArgs e)
        {
            this.BtnPD_f.Visible = false;
            if (this.BtnPD.Visible == false)
            {
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnPD.Visible = true;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnPD.Visible = false;
            }
        }

        private void BtnWH_Click(object sender, EventArgs e)
        {
            this.BtnWH.Visible = false;
            if (this.BtnWH_f.Visible == false)
            {
                this.BtnWH_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnWH_f.Visible = false;
            }
        }

        private void BtnWH_f_Click(object sender, EventArgs e)
        {
            this.BtnWH_f.Visible = false;
            if (this.BtnWH.Visible == false)
            {
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnWH.Visible = true;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnWH.Visible = false;
            }
        }

        private void BtnSA_Click(object sender, EventArgs e)
        {
            this.BtnSA.Visible = false;
            if (this.BtnSA_f.Visible == false)
            {
                this.BtnSA_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnSA_f.Visible = false;
            }
        }

        private void BtnSA_f_Click(object sender, EventArgs e)
        {
            this.BtnSA_f.Visible = false;
            if (this.BtnSA.Visible == false)
            {
                this.BtnSA.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnSA.Visible = false;
            }
        }

        private void BtnMem_Click(object sender, EventArgs e)
        {
            this.BtnMem.Visible = false;
            if (this.BtnMem_f.Visible == false)
            {
                this.BtnMem_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnMem_f.Visible = false;
            }
        }

        private void BtnMem_f_Click(object sender, EventArgs e)
        {
            this.BtnMem_f.Visible = false;
            if (this.BtnMem.Visible == false)
            {
                this.BtnMem.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnMem.Visible = false;
            }
        }

        private void BtnDebt_Click(object sender, EventArgs e)
        {
            this.BtnDebt.Visible = false;
            if (this.BtnDebt_f.Visible == false)
            {
                this.BtnDebt_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnDebt_f.Visible = false;
            }
        }

        private void BtnDebt_f_Click(object sender, EventArgs e)
        {
            this.BtnDebt_f.Visible = false;
            if (this.BtnDebt.Visible == false)
            {
                this.BtnDebt.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnDebt.Visible = false;
            }
        }

        private void BtnFn_Click(object sender, EventArgs e)
        {
            this.BtnFn.Visible = false;
            if (this.BtnFn_f.Visible == false)
            {
                this.BtnFn_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnFn_f.Visible = false;
            }
        }

        private void BtnFn_f_Click(object sender, EventArgs e)
        {
            this.BtnFn_f.Visible = false;
            if (this.BtnFn.Visible == false)
            {
                this.BtnFn.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnFn.Visible = false;
            }
        }

        private void BtnAcc_Click(object sender, EventArgs e)
        {
            this.BtnAcc.Visible = false;
            if (this.BtnAcc_f.Visible == false)
            {
                this.BtnAcc_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnAcc_f.Visible = false;
            }
        }

        private void BtnAcc_f_Click(object sender, EventArgs e)
        {
            this.BtnAcc_f.Visible = false;
            if (this.BtnAcc.Visible == false)
            {
                this.BtnAcc.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnAcc.Visible = false;
            }
        }

        private void BtnPayroll_Click(object sender, EventArgs e)
        {
            this.BtnPayroll.Visible = false;
            if (this.BtnPayroll_f.Visible == false)
            {
                this.BtnPayroll_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnPayroll_f.Visible = false;
            }
        }

        private void BtnPayroll_f_Click(object sender, EventArgs e)
        {
            this.BtnPayroll_f.Visible = false;
            if (this.BtnPayroll.Visible == false)
            {
                this.BtnPayroll.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;

            }
            else
            {
                this.BtnPayroll.Visible = false;
            }
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            this.BtnReport.Visible = false;
            if (this.BtnReport_f.Visible == false)
            {
                this.BtnReport_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnReport_f.Visible = false;
            }
        }

        private void BtnReport_f_Click(object sender, EventArgs e)
        {
            this.BtnReport_f.Visible = false;
            if (this.BtnReport.Visible == false)
            {
                this.BtnReport.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnRole_f.Visible = true;
            }
            else
            {
                this.BtnReport.Visible = false;
            }

        }

        private void BtnRole_Click(object sender, EventArgs e)
        {
            this.BtnRole.Visible = false;
            if (this.BtnRole_f.Visible == false)
            {
                this.BtnRole_f.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;
                this.BtnRole.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
                this.BtnRole_f.Visible = true;


            }
            else
            {
                this.BtnRole_f.Visible = false;
            }
        }

        private void BtnRole_f_Click(object sender, EventArgs e)
        {
            this.BtnRole_f.Visible = false;
            if (this.BtnRole.Visible == false)
            {
                this.BtnRole.Visible = true;
                this.BtnEnterPrise_manager.Visible = false;
                this.BtnEnter_PR.Visible = false;
                this.BtnEnter_AC.Visible = false;
                this.BtnEnter_Mem.Visible = false;
                this.BtnEnter_WH.Visible = false;
                this.BtnPR.Visible = false;
                this.BtnPD.Visible = false;
                this.BtnWH.Visible = false;
                this.BtnSA.Visible = false;
                this.BtnMem.Visible = false;
                this.BtnDebt.Visible = false;
                this.BtnFn.Visible = false;
                this.BtnAcc.Visible = false;
                this.BtnPayroll.Visible = false;
                this.BtnReport.Visible = false;

                this.BtnEnterPrise_manager_false.Visible = true;
                this.BtnEnter_PR_f.Visible = true;
                this.BtnEnter_AC_f.Visible = true;
                this.BtnEnter_Mem_f.Visible = true;
                this.BtnEnter_WH_f.Visible = true;
                this.BtnPR_f.Visible = true;
                this.BtnPD_f.Visible = true;
                this.BtnWH_f.Visible = true;
                this.BtnSA_f.Visible = true;
                this.BtnMem_f.Visible = true;
                this.BtnDebt_f.Visible = true;
                this.BtnFn_f.Visible = true;
                this.BtnAcc_f.Visible = true;
                this.BtnPayroll_f.Visible = true;
                this.BtnReport_f.Visible = true;
            }
            else
            {
                this.BtnRole.Visible = false;
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
                this.btnmaximize.Visible = false;
                this.btnmaximize_full.Visible = true;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.btnmaximize.Visible = false;
                this.btnmaximize_full.Visible = true;
            }
        }

        private void btnmaximize_full_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.btnmaximize.Visible = true;
                this.btnmaximize_full.Visible = false;
            }

        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
