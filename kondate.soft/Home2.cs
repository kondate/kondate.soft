using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kondate.soft
{
    public partial class Home2 : Form
    {
        public Home2()
        {
            InitializeComponent();
            this.panel_Enterprise_manager_Sub.Visible = false;
        }


        private void show_menu(Panel panel)
        {
            if (panel.Visible==false)
            {
                panel.Visible = true;
            }
            else
            {
                panel.Visible = false;
            }
        }
        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void iblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void BtnEnterPrise_manager_Click(object sender, EventArgs e)
        {
            if (this.panel_Enterprise_manager_Sub.Visible == false)
            {
                this.panel_Enterprise_manager_Sub.Visible = true;
            }
            else
            {
                this.panel_Enterprise_manager_Sub.Visible = false;
            }
        }

        private void BtnPowerOff_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
