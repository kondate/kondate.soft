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
using System.Configuration;

using System.Data.SqlClient;

using System.Data.Common;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.IO;
using System.Globalization;
using System.Threading;

namespace kondate.soft
{
    public partial class Home_SETUP_Enter_2ACC : Form
    {

        public Home_SETUP_Enter_2ACC()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_2ACC_Load(object sender, EventArgs e)
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

        private void btnEnter2ACC_Setup1_Debtor_type_code_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter2ACC_Setup2_Debtor_code_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter2ACC_Setup3_Set_Account_period_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter2ACC_Setup4_Company_information_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup4_Company_information.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_04_Co frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_04_Co();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup5_Branch_information_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup5_Branch_information.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_05_Branch frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_05_Branch();
            frm2.Show();
        }

        private void btnEnter2ACC_Setup6_Define_warehouse_information_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup6_Define_warehouse_information.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_06_wherehouse frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_06_wherehouse();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup7_Set_Project_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup7_Set_Project.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_07_project frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_07_project();
            frm2.Show();
        }

        private void btnEnter2ACC_Setup8_Set_Acount_Code_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup8_Set_Acount_Code.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_08_acc frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_08_acc();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup9_Set_bank_code_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup9_Set_bank_code.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_09_code_bank frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_09_code_bank();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup10_Set_bank_branch_code_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup10_Set_bank_branch_code.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_10_code_bank_branch frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_10_code_bank_branch();
            frm2.Show();


        }

        private void btnEnter2ACC_Setup11_Set_passbook_type_code_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter2ACC_Setup12_Set_passbook_code_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter2ACC_Setup13_Set_the_tax_group_Click(object sender, EventArgs e)
        {
             W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup13_Set_the_tax_group.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_13_group_tax frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_13_group_tax();
            frm2.Show();
        }

        private void btnEnter2ACC_Setup14_Set_the_tax_type_Click(object sender, EventArgs e)
        {

        }

        private void btnEnter2ACC_Setup15_Set_wh_type_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup15_Set_wh_type.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_15_wherehouse_type frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_15_wherehouse_type();
            frm2.Show();
        }

        private void btnEnter2ACC_Setup16_Set_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup16_Set_department.Text.Trim();

            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_16_department frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_16_department();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup17_Set_job_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup17_Set_job.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_17_job frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_17_job();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup18_Set_Currency_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup18_Set_Currency.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_18_currency frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_18_currency();
            frm2.Show();

        }

        private void btnEnter2ACC_Setup19_Berg_mat_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup19_Berg_mat.Text.Trim();
            kondate.soft.SETUP_02ACC.Home_SETUP_Enter_2ACC_19_berg_type frm2 = new kondate.soft.SETUP_02ACC.Home_SETUP_Enter_2ACC_19_berg_type();
            frm2.Show();
        }
    }
}
