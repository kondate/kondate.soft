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
    public partial class HOME02_Purchasing_Department : Form
    {
        public HOME02_Purchasing_Department()
        {
            InitializeComponent();
        }

        private void HOME02_Purchasing_Department_Load(object sender, EventArgs e)
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

        private void btnPurchasing_01PR_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPurchasing_01PR.Text.Trim();
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_01PR_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_01PR_record();
            frm2.Show();
        }

        private void btnPurchasing_02PO_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPurchasing_02PO.Text.Trim();
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record();
            frm2.Show();

        }

        private void btnPurchasing_03PO_Check_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPurchasing_03PO_Check.Text.Trim();
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_03PR_Check_PR__ALL frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_03PR_Check_PR__ALL();
            frm2.Show();
        }

        private void btnPurchasing_04PO_Approve_Click(object sender, EventArgs e)
        {
               W_ID_Select.WORD_TOP = this.btnPurchasing_04PO_Approve.Text.Trim();
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_04AP_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_04AP_record();
            frm2.Show();

        }

        private void btnPurchasing_05receive_mat_Click(object sender, EventArgs e)
        {
            
           W_ID_Select.WORD_TOP = this.btnPurchasing_05receive_mat.Text.Trim();
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record();
            frm2.Show();
        }

        private void btnPurchasing_06payment_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_07acc_debt_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_08acc_debt_low_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_09debt_plus_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_10change_price_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_11promotion_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_12discount_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_13report_phurchase_Click(object sender, EventArgs e)
        {

        }

        private void btnPurchasing_14report_debt_Click(object sender, EventArgs e)
        {

        }

        private void HOME03_Production_07_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME03_Production_07.Text.Trim();
            kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01Mat_Average frm2 = new kondate.soft.HOME04_Warehouse.HOME04_Warehouse_01Mat_Average();
            frm2.Show();

        }
    }
}
