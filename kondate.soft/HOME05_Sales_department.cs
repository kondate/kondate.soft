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
    public partial class HOME05_Sales_department : Form
    {
        public HOME05_Sales_department()
        {
            InitializeComponent();
        }

        private void HOME05_Sales_department_Load(object sender, EventArgs e)
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

        private void HOME05_Sale_02Cus_type_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME05_Sale_02Cus_type.Text.Trim();
            kondate.soft.HOME05_Sales.HOME05_Sale_02cus_type frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_02cus_type();
            frm2.Show();
        }

        private void HOME05_Sale_03Cus_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME05_Sale_02Cus_type.Text.Trim();
            kondate.soft.HOME05_Sales.HOME05_Sale_03cus frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_03cus();
            frm2.Show();
        }

        private void HOME05_Sale_01sale_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.HOME05_Sale_02Cus_type.Text.Trim();
            kondate.soft.HOME05_Sales.HOME05_Sale_01sale frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_01sale();
            frm2.Show();
        }

        private void HOME05_Sale_01sale_report_Click(object sender, EventArgs e)
        {

        }

        private void HOME05_Sale_01sale_Cash_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME05_Sale_01sale_Cash.Text.Trim();
            kondate.soft.HOME05_Sales.HOME05_Sale_02sale_cash frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_02sale_cash();
            frm2.Show();
        }

        private void HOME05_Sale_02_IV_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME05_Sale_02_IV.Text.Trim();
            kondate.soft.HOME05_Sales.HOME05_Sale_03IV frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_03IV();
            frm2.Show();
        }

        private void HOME05_Sale_03_Wangbill_Click(object sender, EventArgs e)
        {
            
             W_ID_Select.WORD_TOP = this.HOME05_Sale_03_Wangbill.Text.Trim();
            kondate.soft.HOME05_Sales.HOME05_Sale_04Wang_bill frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_04Wang_bill();
            frm2.Show();
        }







        //
    }
}
