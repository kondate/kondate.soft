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
    public partial class Home_SETUP_Enter_4WH : Form
    {
        public Home_SETUP_Enter_4WH()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_4WH_Load(object sender, EventArgs e)
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



        private void btnEnter4WH_Setup1_Set_mat_type_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup1_Set_mat_type.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_01_mat_type frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_01_mat_type();
            frm2.Show();

        }
        private void btnEnter4WH_Setup2_Set_mat_sac_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup2_Set_mat_sac.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_02_mat_sac frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_02_mat_sac();
            frm2.Show();

        }
        private void btnEnter4WH_Setup3_Set_mat_groups_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup3_Set_mat_groups.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_03_mat_group frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_03_mat_group();
            frm2.Show();

        }

        private void btnEnter4WH_Setup4_Set_mat_brand_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup4_Set_mat_brand.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_04_mat_brand frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_04_mat_brand();
            frm2.Show();

        }

        private void btnEnter4WH_Setup5_Set_unit_count_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup5_Set_unit_count.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_05_mat_unit1 frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_05_mat_unit1();
            frm2.Show();

        }

        private void btnEnter4WH_Setup7_Set_product_code_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup7_Set_product_code.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_07_mat frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_07_mat();
            frm2.Show();

        }

        private void btnEnter4WH_Setup6_Set_unit_count_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup5_Set_unit_count.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_06_mat_unit2 frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_06_mat_unit2();
            frm2.Show();

        }

        private void btnEnter4WH_Setup8_Set_type_BOM_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup8_Set_type_BOM.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_08_bom_type frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_08_bom_type();
            frm2.Show();

        }

        private void btnEnter4WH_Setup9_Set_BOM_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter4WH_Setup9_Set_BOM.Text.Trim();
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_09_bom frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_09_bom();
            frm2.Show();

        }
    }
}
