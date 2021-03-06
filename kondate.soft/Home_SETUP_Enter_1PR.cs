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
    public partial class Home_SETUP_Enter_1PR : Form
    {
        public Home_SETUP_Enter_1PR()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_1PR_Load(object sender, EventArgs e)
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

        private void btnEnter1PR_Setup1_profix_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter1PR_Setup1_profix.Text.Trim();
            kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_01_profix frm2 = new kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_01_profix();
            frm2.Show();

        }

        private void btnEnter1PR_Setup2_type_supplier_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter1PR_Setup2_type_supplier.Text.Trim();
            kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_02_Supplier_Type frm2 = new kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_02_Supplier_Type();
            frm2.Show();

        }

        private void btnEnter1PR_Setup3_type_supplier_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter1PR_Setup3_type_supplier.Text.Trim();
            kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_03_Supplier_Group frm2 = new kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_03_Supplier_Group();
            frm2.Show();

        }

        private void btnEnter1PR_Setup4_supplier_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter1PR_Setup4_supplier.Text.Trim();
            kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_04_Supplier frm2 = new kondate.soft.SETUP_1PR.Home_SETUP_Enter_1PR_04_Supplier();
            frm2.Show();

        }
    }
}
