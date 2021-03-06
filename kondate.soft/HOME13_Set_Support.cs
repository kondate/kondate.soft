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
    public partial class HOME13_Set_Support : Form
    {
        public HOME13_Set_Support()
        {
            InitializeComponent();
        }

        private void HOME13_Set_Support_Load(object sender, EventArgs e)
        {

        }

        private void btnSet_support_01_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.WORD_TOP = this.btnSet_support_01.Text.Trim();
                kondate.soft.SETUP_13SP.Home_SETUP_Enter_13SP_01_support_02_problem frm2 = new kondate.soft.SETUP_13SP.Home_SETUP_Enter_13SP_01_support_02_problem();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
        }

        private void HOME13_Support_01_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.WORD_TOP = this.HOME13_Support_01.Text.Trim();
                kondate.soft.HOME13_Support.Home13_Support_01repair frm2 = new kondate.soft.HOME13_Support.Home13_Support_01repair();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
        }

        private void HOME13_Support_02_Click(object sender, EventArgs e)
        {
            
          if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.WORD_TOP = this.HOME13_Support_02.Text.Trim();
                kondate.soft.HOME13_Support.Home13_Support_02approve_repair_1reques frm2 = new kondate.soft.HOME13_Support.Home13_Support_02approve_repair_1reques();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
        }

        private void HOME13_Support_03_Click(object sender, EventArgs e)
        {
            
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.WORD_TOP = this.HOME13_Support_03.Text.Trim();
                kondate.soft.HOME13_Support.Home13_Support_03get_repair_1approve frm2 = new kondate.soft.HOME13_Support.Home13_Support_03get_repair_1approve();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
        }

        //=======================================================
    }
}
