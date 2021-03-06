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
    public partial class Form_001Test_1panel_top : Form
    {
        public Form_001Test_1panel_top()
        {
            InitializeComponent();
        }

        //ปุ่ม minimize,maximize
 
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            //if (WindowState == FormWindowState.Maximized)
            //{
            //    this.WindowState = FormWindowState.Minimized;
            //}
            //else if (WindowState == FormWindowState.Normal)
            //{
            //    this.WindowState = FormWindowState.Minimized;
            //}
        }

        private void btnminimize_Click_1(object sender, EventArgs e)
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

        private void btnclose_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }
        //End ปุ่ม minimize,maximize

    }
}
