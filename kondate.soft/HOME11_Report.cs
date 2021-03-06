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
    public partial class HOME11_Report : Form
    {
        public HOME11_Report()
        {
            InitializeComponent();
        }

        private void HOME11_Report_Load(object sender, EventArgs e)
        {

        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
