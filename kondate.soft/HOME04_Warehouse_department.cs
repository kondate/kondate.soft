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
    public partial class HOME04_Warehouse_department : Form
    {
        public HOME04_Warehouse_department()
        {
            InitializeComponent();
        }

        private void HOME04_Warehouse_department_Load(object sender, EventArgs e)
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



        private void HOME03_Production_07_99Gen_barcode_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_07_99Gen_barcode.Text.Trim();
            kondate.soft.HOME04_Warehouse.Form1_Generate_Barcode frm2 = new kondate.soft.HOME04_Warehouse.Form1_Generate_Barcode();
            frm2.Show();
            
        }

        private void HOME03_Production_07_01Receive_mat_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_07_01Receive_mat.Text.Trim();
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record();
            frm2.Show();

        }

        private void HOME03_Production_07_01stock_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME03_Production_07_01stock.Text.Trim();
            kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01Mat_Average frm2 = new kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01Mat_Average();
            frm2.Show();

        }

        private void HOME03_Production_07_01Sell_mat_Click(object sender, EventArgs e)
        {
             W_ID_Select.WORD_TOP = this.HOME03_Production_07_01Sell_mat.Text.Trim();
            kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01request_sell_record frm2 = new kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01request_sell_record();
            frm2.Show();
        }

        private void HOME03_Production_07_01Sell_mat_approve_Click(object sender, EventArgs e)
        {
              W_ID_Select.WORD_TOP = this.HOME03_Production_07_01Sell_mat_approve.Text.Trim();
            kondate.soft.HOME04_Warehouse.HOME04_Warehouse_02approve_request_sell_record frm2 = new kondate.soft.HOME04_Warehouse.HOME04_Warehouse_02approve_request_sell_record();
            frm2.Show();
        }
    }
}
