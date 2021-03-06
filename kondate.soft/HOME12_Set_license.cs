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
    public partial class HOME12_Set_license : Form
    {
        public HOME12_Set_license()
        {
            InitializeComponent();
        }

        private void HOME12_Set_license_Load(object sender, EventArgs e)
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

        private void btnSet_license_01_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.WORD_TOP = this.btnSet_license_01.Text.Trim();
                kondate.soft.HOME12_license.HOME12_Set_license_01_user frm2 = new kondate.soft.HOME12_license.HOME12_Set_license_01_user();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
        }

        private void btnSet_license_02_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnSet_license_02.Text.Trim();

            kondate.soft.HOME12_license.HOME12_Set_license_02_user_type frm2 = new kondate.soft.HOME12_license.HOME12_Set_license_02_user_type();
            frm2.Show();

        }

        private void btnSet_license_03_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnSet_license_03.Text.Trim();

            kondate.soft.HOME12_license.HOME12_Set_license_03_change_pass frm2 = new kondate.soft.HOME12_license.HOME12_Set_license_03_change_pass();
            frm2.Show();

        }

        private void btnSet_license_04_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_USERNAME_TYPE == "4" )
            {
            W_ID_Select.WORD_TOP = this.btnSet_license_04.Text.Trim();

            kondate.soft.HOME12_license.HOME12_Set_license_04_user_role frm2 = new kondate.soft.HOME12_license.HOME12_Set_license_04_user_role();
            frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void btnSet_license_05_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.WORD_TOP = this.btnSet_license_05.Text.Trim();

                kondate.soft.HOME12_license.HOME12_Set_license_05_user_trans_log frm2 = new kondate.soft.HOME12_license.HOME12_Set_license_05_user_trans_log();
                frm2.Show();
            }
            else if (W_ID_Select.M_USERNAME_TYPE == "3")
            {
                W_ID_Select.WORD_TOP = this.btnSet_license_05.Text.Trim();

                kondate.soft.HOME12_license.HOME12_Set_license_05_user_trans_log frm2 = new kondate.soft.HOME12_license.HOME12_Set_license_05_user_trans_log();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            }

        private void btnSet_license_06_Click(object sender, EventArgs e)
        {

        }
    }
}
