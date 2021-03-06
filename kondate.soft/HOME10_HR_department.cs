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
    public partial class HOME10_HR_department : Form
    {
        public HOME10_HR_department()
        {
            InitializeComponent();
        }

        private void HOME10_HR_department_Load(object sender, EventArgs e)
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

        private void btnHR_01_Department_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnHR_01_Department.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_16_department frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_16_department();
            frm2.Show();
        }
    }
}
