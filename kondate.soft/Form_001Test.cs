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
    public partial class Form_001Test : Form
    {
        public Form_001Test()
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
        //End ปุ่ม minimize,maximize

    }
}
