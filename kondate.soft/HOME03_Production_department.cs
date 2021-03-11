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
    public partial class HOME03_Production_department : Form
    {
        public HOME03_Production_department()
        {
            InitializeComponent();
        }

        private void HOME03_Production_department_Load(object sender, EventArgs e)
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

        private void HOME03_Production_01_Click(object sender, EventArgs e)
        {

            W_ID_Select.WORD_TOP = this.HOME03_Production_01FG.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_01RG_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_01RG_record();
            frm2.Show();

        }

        private void HOME03_Production_07_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_07.Text.Trim();
            kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01Mat_Average frm2 = new kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01Mat_Average();
            frm2.Show();

        }

        private void HOME03_Production_13_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_13.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_13Type_Machine frm2 = new kondate.soft.HOME03_Production.HOME03_Production_13Type_Machine();
            frm2.Show();

        }

        private void HOME03_Production_14_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_14.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_14Machine frm2 = new kondate.soft.HOME03_Production.HOME03_Production_14Machine();
            frm2.Show();

        }

        private void HOME03_Production_02_Click(object sender, EventArgs e)
        {
            
          W_ID_Select.WORD_TOP = this.HOME03_Production_02_BG.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_02Berg_Produce_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_02Berg_Produce_record();
            frm2.Show();
        }

        private void HOME03_Production_03_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_03_FG1.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_03Produce_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_record();
            frm2.Show();
        }

        private void HOME03_Production_15_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_15.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_15produce_type frm2 = new kondate.soft.HOME03_Production.HOME03_Production_15produce_type();
            frm2.Show();
        }

        private void HOME03_Production_16_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_16.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_16face_baking frm2 = new kondate.soft.HOME03_Production.HOME03_Production_16face_baking();
            frm2.Show();

        }

        private void HOME03_Production_04_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_04.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_04QC_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_04QC_record();
            frm2.Show();
        }

        private void HOME03_Production_05_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_05.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record();
            frm2.Show();
        }

        private void HOME03_Production_17_Click(object sender, EventArgs e)
        {
              W_ID_Select.WORD_TOP = this.HOME03_Production_17.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_17Number_mat frm2 = new kondate.soft.HOME03_Production.HOME03_Production_17Number_mat();
            frm2.Show();

        }

        private void HOME03_Production_18_Click(object sender, EventArgs e)
        {
              W_ID_Select.WORD_TOP = this.HOME03_Production_18.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_18Number_Color frm2 = new kondate.soft.HOME03_Production.HOME03_Production_18Number_Color();
            frm2.Show();
        }

        private void HOME03_Production_06_Click(object sender, EventArgs e)
        {
              W_ID_Select.WORD_TOP = this.HOME03_Production_06.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_07Receive_Send_Dye_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_07Receive_Send_Dye_record();
            frm2.Show();
        }

        private void HOME03_Production_19_shirt_type_Click(object sender, EventArgs e)
        {
              W_ID_Select.WORD_TOP = this.HOME03_Production_19_shirt_type.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_19shirt_type frm2 = new kondate.soft.HOME03_Production.HOME03_Production_19shirt_type();
            frm2.Show();
        }

        private void HOME03_Production_20_shirt_size_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_20_shirt_size.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_20shirt_size frm2 = new kondate.soft.HOME03_Production.HOME03_Production_20shirt_size();
            frm2.Show();
        }

        private void HOME03_Production_21_room_collect_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_21_room_collect.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_21room_collect frm2 = new kondate.soft.HOME03_Production.HOME03_Production_21room_collect();
            frm2.Show();
        }

        private void HOME03_Production_08_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_08.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_08Cut_shirt_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_08Cut_shirt_record();
            frm2.Show();

        }

        private void HOME03_Production_09_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_09.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_09Sew_shirt_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_09Sew_shirt_record();
            frm2.Show();
        }

        private void HOME03_Production_10_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_10.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_10Rolled_shirt_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_10Rolled_shirt_record();
            frm2.Show();
        }

        private void HOME03_Production_11_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_11.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_11QCS_shirt_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_11QCS_shirt_record();
            frm2.Show();
        }

        private void HOME03_Production_12_Click(object sender, EventArgs e)
        {
             W_ID_Select.WORD_TOP = this.HOME03_Production_12.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_12FG_shirt_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_12FG_shirt_record();
            frm2.Show();
        }

        private void HOME03_Production_02_BG_G_Click(object sender, EventArgs e)
        {
             W_ID_Select.WORD_TOP = this.HOME03_Production_02_BG_G.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_01RG_Stock frm2 = new kondate.soft.HOME03_Production.HOME03_Production_01RG_Stock();
            frm2.Show();
        }

        private void HOME03_Production_03_FG1_G_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME03_Production_03_FG1_G.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_03Produce_Stock frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_Stock();
            frm2.Show();
        }

        private void HOME03_Production_06_G_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME03_Production_06_G.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_07Receive_Send_Dye_Stock frm2 = new kondate.soft.HOME03_Production.HOME03_Production_07Receive_Send_Dye_Stock();
            frm2.Show();
        }

        private void HOME03_Production_05_GR_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME03_Production_05_GR.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_GR frm2 = new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_GR();
            frm2.Show();

        }

        private void HOME03_Production_06_ST_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_06_ST.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_07Send_Cut_shirt_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_07Send_Cut_shirt_record();
            frm2.Show();
        }

        private void HOME03_Production_06_ST_GR_Click(object sender, EventArgs e)
        {
            
            W_ID_Select.WORD_TOP = this.HOME03_Production_06_ST_GR.Text.Trim();
            kondate.soft.HOME03_Production.HOME03_Production_07Send_Cut_shirt_GR frm2 = new kondate.soft.HOME03_Production.HOME03_Production_07Send_Cut_shirt_GR();
            frm2.Show();
        }
    }
}
