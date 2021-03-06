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
    public partial class HOME : Form
    {
        //Move Form ====================================
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //END Move Form ====================================
        //ประกาศ Cultureinfo ของแต่ละแบบที่ต้องการ
        CultureInfo ThaiCulture = new CultureInfo("th-TH");
        CultureInfo UsaCulture = new CultureInfo("en-US");

        private Form currentChildForm;

        public HOME()
        {
            Thread t = new Thread(new ThreadStart(Start_Form));
            t.Start();

            //Thread.Sleep(10000);
            // Thread.Sleep(50);
            Thread.Sleep(W_ID_Select.SLEEP);
            //MessageBox.Show("xxx");
            //top level not really needed
            //MessageBox.Show("xxx2");

            // Thread.Sleep(10000);
            //MessageBox.Show("xxx");
            //top level not really needed
            //f3.TopLevel = true;
            //f3.WindowState = FormWindowState.Minimized;
            //f3.Show(this);
            //MessageBox.Show("xxx2");

            // this.StartPosition = FormStartPosition.Manual;
            //this.WindowState = FormWindowState.Maximized;

            //// this.Size.Width = 1924;
            //// this.Size.Height = 1084;
            //this.Location = new Point(0, 0);


            InitializeComponent();
            // f2.Visible = false;
            // f2.WindowState = FormWindowState.Maximized;

            t.Abort();
            this.BringToFront();
            this.Focus();

            //Resize Form ===================================
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            //END Resize Form ===================================

            this.panel_Enterprise_manager_Sub.Visible = false;

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;


        }
        //Resize Form ===================================
        private const int cGrip = 16;
        private const int cCaption = 32;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }
        //END Resize Form ===================================
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktop.Controls.Add(childForm);
            this.panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void show_menu(Panel panel)
        {
            if (panel.Visible == false)
            {
                panel.Visible = true;
            }
            else
            {
                panel.Visible = false;
            }
        }

        public void Start_Form()
        {

            Application.Run(new Form_Spash_Screen());


        }
        private void Home_Resize(object sender, EventArgs e)
        {
            //Control control = (Control)sender;

            //// Ensure the Form remains square (Height = Width).
            //if (control.Size.Height != control.Size.Width)
            //{
            //    control.Size = new Size(control.Size.Width, control.Size.Width);
            //}

        }
        private void Home_Load(object sender, EventArgs e)
        {

            CHECK_USER_RULE_ALL();

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            this.panel_left.Width = 250;
            this.panelDesktop.Left = 253;
            this.panel_low.Left = 253;
            this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            //this.panel_left.Width = 50;
            //this.panelDesktop.Left = 53;
            //this.panel_low.Left = 53;
            //this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            //this.panel_low.Width = this.Width - this.panel_left.Width - 3;

        }

        private void BtnSlide_Click(object sender, EventArgs e)
        {
            if (this.panel_left.Width == 250 )
            {
                this.panel_left.Width = 50;
                this.panelDesktop.Left = 53;
                this.panel_low.Left = 53;
                this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
                this.panel_low.Width = this.Width - this.panel_left.Width - 3;
            }
            else
            {
                this.panel_left.Width = 250;
                this.panelDesktop.Left = 253;
                this.panel_low.Left = 253;
                this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
                this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            }
        }

        private void iblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void Home_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel_left_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void BtnEnterPrise_manager_Click(object sender, EventArgs e)
        {


            W_ID_Select.M_DEPART_NUMBER = "1";
            W_ID_Select.M_DEPART_NAME = this.Btn01_EnterPrise_manager.Text.ToString();

            CHECK_USER_RULE();


            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //1
            this.Btn01_EnterPrise_manager.Visible = false;
            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager.Text.ToString();

            if (this.Btn01_EnterPrise_manager_false.Visible == false)
            {
                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn01_EnterPrise_manager_false.Visible = false;
            }
            this.panel_left.Width = 250;
            this.panelDesktop.Left = 253;
            this.panel_low.Left = 253;
            this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            if (this.panel_Enterprise_manager_Sub.Visible == true)
            {
                this.Btn01_EnterPrise_manager.Visible = true;
                this.Btn01_EnterPrise_manager_false.Visible = false;
                this.panel_Enterprise_manager_Sub.Visible = false;

            }
            else
            {
                this.panel_Enterprise_manager_Sub.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = true;
                this.Btn01_EnterPrise_manager_false.Visible = false;
            }

        }
        private void BtnEnterPrise_manager_false_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "1";
            W_ID_Select.M_DEPART_NAME = this.Btn01_EnterPrise_manager_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.Btn01_EnterPrise_manager_false.Visible = true;
            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_false.Text.ToString();


            if (this.Btn01_EnterPrise_manager.Visible == false)
            {
                this.Btn01_EnterPrise_manager.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;

            }
            else
            {
                this.Btn01_EnterPrise_manager.Visible = false;
            }

            this.panel_left.Width = 250;
            this.panelDesktop.Left = 253;
            this.panel_low.Left = 253;
            this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            if (this.panel_Enterprise_manager_Sub.Visible == false)
            {
                this.Btn01_EnterPrise_manager.Visible = true;
                this.Btn01_EnterPrise_manager_false.Visible = false;
                this.panel_Enterprise_manager_Sub.Visible = true;

            }
            else
            {
                this.panel_Enterprise_manager_Sub.Visible = false;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_false.Visible = true;
            }
        }

        private void Btn01_EnterPrise_manager_1setup_Purchasing_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "1";
            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_1setup_Purchasing.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            this.panel_left.Width = 250;
            this.panelDesktop.Left = 253;
            this.panel_low.Left = 253;
            this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            OpenChildForm(new Home_SETUP_Enter_1PR());

            if (this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = false;
            }

        }
        private void Btn01_EnterPrise_manager_1setup_Purchasing_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "1";
            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            this.panel_left.Width = 250;
            this.panelDesktop.Left = 253;
            this.panel_low.Left = 253;
            this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            OpenChildForm(new Home_SETUP_Enter_1PR());

            if (this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;

            }
            else
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
            }
        }

        private void Btn01_EnterPrise_manager_2setup_accounting_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "1";

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new Home_SETUP_Enter_2ACC());

            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_2setup_accounting.Text.ToString();

            if (this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = false;
            }
        }
        private void Btn01_EnterPrise_manager_2setup_accounting_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "1";

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new Home_SETUP_Enter_2ACC());



            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_2setup_accounting_false.Text.ToString();

            if (this.Btn01_EnterPrise_manager_2setup_accounting.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;

            }
            else
            {
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
            }
        }

        private void Btn01_EnterPrise_manager_3setup_Registration_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "1";

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new Home_SETUP_Enter_3Member());

            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_3setup_Registration.Text.ToString();

            if (this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = false;
            }
        }
        private void Btn01_EnterPrise_manager_3setup_Registration_false_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "1";

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new Home_SETUP_Enter_3Member());

            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_3setup_Registration_false.Text.ToString();

            if (this.Btn01_EnterPrise_manager_3setup_Registration.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
            }
        }

        private void Btn01_EnterPrise_manager_4setup_Warehouse_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "1";

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new Home_SETUP_Enter_4WH());

            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_4setup_Warehouse.Text.ToString();

            if (this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = false;
            }
        }
        private void Btn01_EnterPrise_manager_4setup_Warehouse_false_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "1";

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new Home_SETUP_Enter_4WH());

            this.iblMenu_name.Text = this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Text.ToString();

            if (this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible == false)
            {
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = true;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = false;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
            }
        }

        private void Btn02_Purchasing_Department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "2";
            W_ID_Select.M_DEPART_NAME = this.Btn02_Purchasing_Department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME02_Purchasing_Department());

            this.iblMenu_name.Text = this.Btn02_Purchasing_Department.Text.ToString();

            this.Btn02_Purchasing_Department.Visible = false;
            if (this.Btn02_Purchasing_Department_false.Visible == false)
            {
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn02_Purchasing_Department_false.Visible = false;
            }
        }
        private void Btn02_Purchasing_Department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "2";
            W_ID_Select.M_DEPART_NAME = this.Btn02_Purchasing_Department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME02_Purchasing_Department());

            this.iblMenu_name.Text = this.Btn02_Purchasing_Department_false.Text.ToString();

            this.Btn02_Purchasing_Department_false.Visible = false;
            if (this.Btn02_Purchasing_Department.Visible == false)
            {
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = true;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;

            }
            else
            {
                this.Btn02_Purchasing_Department.Visible = false;
            }
        }

        private void Btn03_Production_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "3";
            W_ID_Select.M_DEPART_NAME = this.Btn03_Production_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME03_Production_department());

            this.iblMenu_name.Text = this.Btn03_Production_department.Text.ToString();

            this.Btn03_Production_department.Visible = false;
            if (this.Btn03_Production_department_false.Visible == false)
            {
                this.Btn03_Production_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn03_Production_department_false.Visible = false;
            }
        }
        private void Btn03_Production_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "3";
            W_ID_Select.M_DEPART_NAME = this.Btn03_Production_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME03_Production_department());

            this.iblMenu_name.Text = this.Btn03_Production_department_false.Text.ToString();

            this.Btn03_Production_department_false.Visible = false;
            if (this.Btn03_Production_department.Visible == false)
            {
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn03_Production_department.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;

            }
            else
            {
                this.Btn03_Production_department.Visible = false;
            }
        }

        private void Btn04_Warehouse_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "4";
            W_ID_Select.M_DEPART_NAME = this.Btn04_Warehouse_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME04_Warehouse_department());

            this.iblMenu_name.Text = this.Btn04_Warehouse_department.Text.ToString();

            this.Btn04_Warehouse_department.Visible = false;
            if (this.Btn04_Warehouse_department_false.Visible == false)
            {
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn04_Warehouse_department_false.Visible = false;
            }
        }
        private void Btn04_Warehouse_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "4";
            W_ID_Select.M_DEPART_NAME = this.Btn04_Warehouse_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME04_Warehouse_department());

            this.iblMenu_name.Text = this.Btn04_Warehouse_department_false.Text.ToString();

            this.Btn04_Warehouse_department_false.Visible = false;
            if (this.Btn04_Warehouse_department.Visible == false)
            {
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn04_Warehouse_department.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn04_Warehouse_department.Visible = false;
            }
        }

        private void Btn05_Sales_department_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "5";
            W_ID_Select.M_DEPART_NAME = this.Btn05_Sales_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME05_Sales_department());

            this.iblMenu_name.Text = this.Btn05_Sales_department.Text.ToString();

            this.Btn05_Sales_department.Visible = false;
            if (this.Btn05_Sales_department_false.Visible == false)
            {
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn05_Sales_department_false.Visible = false;
            }
        }
        private void Btn05_Sales_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "5";
            W_ID_Select.M_DEPART_NAME = this.Btn05_Sales_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME05_Sales_department());

            this.iblMenu_name.Text = this.Btn05_Sales_department_false.Text.ToString();

            this.Btn05_Sales_department_false.Visible = false;
            if (this.Btn05_Sales_department.Visible == false)
            {
                this.Btn05_Sales_department.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn05_Sales_department.Visible = false;
            }
        }

        private void Btn06_Registration_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "6";
            W_ID_Select.M_DEPART_NAME = this.Btn06_Registration_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME06_Registration_department());

            this.iblMenu_name.Text = this.Btn06_Registration_department.Text.ToString();

            this.Btn06_Registration_department.Visible = false;
            if (this.Btn06_Registration_department_false.Visible == false)
            {
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn06_Registration_department_false.Visible = false;
            }
        }
        private void Btn06_Registration_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "6";
            W_ID_Select.M_DEPART_NAME = this.Btn06_Registration_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME06_Registration_department());

            this.iblMenu_name.Text = this.Btn06_Registration_department_false.Text.ToString();

            this.Btn06_Registration_department_false.Visible = false;
            if (this.Btn06_Registration_department.Visible == false)
            {
                this.Btn06_Registration_department.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn06_Registration_department.Visible = false;
            }
        }

        private void Btn07_Credit_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "7";
            W_ID_Select.M_DEPART_NAME = this.Btn07_Credit_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME07_Credit_department());

            this.iblMenu_name.Text = this.Btn07_Credit_department.Text.ToString();

            this.Btn07_Credit_department.Visible = false;
            if (this.Btn07_Credit_department_false.Visible == false)
            {
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn07_Credit_department_false.Visible = false;
            }
        }
        private void Btn07_Credit_department_false_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "7";
            W_ID_Select.M_DEPART_NAME = this.Btn07_Credit_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME07_Credit_department());

            this.iblMenu_name.Text = this.Btn07_Credit_department_false.Text.ToString();

            this.Btn07_Credit_department_false.Visible = false;
            if (this.Btn07_Credit_department.Visible == false)
            {
                this.Btn07_Credit_department.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn07_Credit_department.Visible = false;
            }
        }

        private void Btn08_Finance_department_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "8";
            W_ID_Select.M_DEPART_NAME = this.Btn08_Finance_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME08_Finance_department());

            this.iblMenu_name.Text = this.Btn08_Finance_department.Text.ToString();

            this.Btn08_Finance_department.Visible = false;
            if (this.Btn08_Finance_department_false.Visible == false)
            {
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn08_Finance_department_false.Visible = false;
            }
        }
        private void Btn08_Finance_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "8";
            W_ID_Select.M_DEPART_NAME = this.Btn08_Finance_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME08_Finance_department());

            this.iblMenu_name.Text = this.Btn08_Finance_department_false.Text.ToString();

            this.Btn08_Finance_department_false.Visible = false;
            if (this.Btn08_Finance_department.Visible == false)
            {
                this.Btn08_Finance_department.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn08_Finance_department.Visible = false;
            }
        }

        private void Btn09_Accounting_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "9";
            W_ID_Select.M_DEPART_NAME = this.Btn09_Accounting_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME09_Accounting_department());

            this.iblMenu_name.Text = this.Btn09_Accounting_department.Text.ToString();

            this.Btn09_Accounting_department.Visible = false;
            if (this.Btn09_Accounting_department_false.Visible == false)
            {
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn09_Accounting_department_false.Visible = false;
            }
        }
        private void Btn09_Accounting_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "9";
            W_ID_Select.M_DEPART_NAME = this.Btn09_Accounting_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME09_Accounting_department());

            this.iblMenu_name.Text = this.Btn09_Accounting_department_false.Text.ToString();

            this.Btn09_Accounting_department_false.Visible = false;
            if (this.Btn09_Accounting_department.Visible == false)
            {
                this.Btn09_Accounting_department.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn09_Accounting_department.Visible = false;
            }
        }

        private void Btn10_HR_department_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "10";
            W_ID_Select.M_DEPART_NAME = this.Btn10_HR_department.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME10_HR_department());

            this.iblMenu_name.Text = this.Btn10_HR_department.Text.ToString();

            this.Btn10_HR_department.Visible = false;
            if (this.Btn10_HR_department_false.Visible == false)
            {
                this.Btn10_HR_department_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn10_HR_department_false.Visible = false;
            }
        }
        private void Btn10_HR_department_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "10";
            W_ID_Select.M_DEPART_NAME = this.Btn10_HR_department_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME10_HR_department());

            this.iblMenu_name.Text = this.Btn10_HR_department_false.Text.ToString();

            this.Btn10_HR_department_false.Visible = false;
            if (this.Btn10_HR_department.Visible == false)
            {
                this.Btn10_HR_department.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;

            }
            else
            {
                this.Btn10_HR_department.Visible = false;
            }
        }

        private void Btn11_Report_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "11";
            W_ID_Select.M_DEPART_NAME = this.Btn11_Report.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME11_Report());

            this.iblMenu_name.Text = this.Btn11_Report.Text.ToString();

            this.Btn11_Report.Visible = false;
            if (this.Btn11_Report_false.Visible == false)
            {
                this.Btn11_Report_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn11_Report_false.Visible = false;
            }
        }
        private void Btn11_Report_false_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "11";
            W_ID_Select.M_DEPART_NAME = this.Btn11_Report_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME11_Report());

            this.iblMenu_name.Text = this.Btn11_Report_false.Text.ToString();

            this.Btn11_Report_false.Visible = false;
            if (this.Btn11_Report.Visible == false)
            {
                this.Btn11_Report.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn11_Report.Visible = false;
            }

        }

        private void Btn12_Set_license_Click(object sender, EventArgs e)
        {

            W_ID_Select.M_DEPART_NUMBER = "12";
            W_ID_Select.M_DEPART_NAME = this.Btn12_Set_license.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME12_Set_license());

            this.iblMenu_name.Text = this.Btn12_Set_license.Text.ToString();

            this.Btn12_Set_license.Visible = false;
            if (this.Btn12_Set_license_false.Visible == false)
            {
                this.Btn12_Set_license_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn12_Set_license_false.Visible = false;
            }
        }
        private void Btn12_Set_license_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "12";
            W_ID_Select.M_DEPART_NAME = this.Btn12_Set_license_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME12_Set_license());

            this.iblMenu_name.Text = this.Btn12_Set_license_false.Text.ToString();

            this.Btn12_Set_license_false.Visible = false;
            if (this.Btn12_Set_license.Visible == false)
            {
                this.Btn12_Set_license.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;
            }
            else
            {
                this.Btn12_Set_license.Visible = false;
            }
        }

        private void Btn13_Set_Support_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "13";
            W_ID_Select.M_DEPART_NAME = this.Btn13_Set_Support.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME13_Set_Support());

            this.iblMenu_name.Text = this.Btn13_Set_Support.Text.ToString();

            this.Btn13_Set_Support.Visible = false;
            if (this.Btn13_Set_Support_false.Visible == false)
            {
                this.Btn13_Set_Support_false.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;
                this.Btn13_Set_Support.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
                this.Btn13_Set_Support_false.Visible = true;


            }
            else
            {
                this.Btn13_Set_Support_false.Visible = false;
            }

        }
        private void Btn13_Set_Support_false_Click(object sender, EventArgs e)
        {
            W_ID_Select.M_DEPART_NUMBER = "13";
            W_ID_Select.M_DEPART_NAME = this.Btn13_Set_Support_false.Text.ToString();

            CHECK_USER_RULE();

            if (W_ID_Select.M_DEPART_LOGIN.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OpenChildForm(new HOME13_Set_Support());

            this.iblMenu_name.Text = this.Btn13_Set_Support_false.Text.ToString();

            this.Btn13_Set_Support_false.Visible = false;
            if (this.Btn13_Set_Support.Visible == false)
            {
                this.Btn13_Set_Support.Visible = true;
                this.Btn01_EnterPrise_manager.Visible = false;
                this.Btn01_EnterPrise_manager_1setup_Purchasing.Visible = false;
                this.Btn01_EnterPrise_manager_2setup_accounting.Visible = false;
                this.Btn01_EnterPrise_manager_3setup_Registration.Visible = false;
                this.Btn01_EnterPrise_manager_4setup_Warehouse.Visible = false;
                this.Btn02_Purchasing_Department.Visible = false;
                this.Btn03_Production_department.Visible = false;
                this.Btn04_Warehouse_department.Visible = false;
                this.Btn05_Sales_department.Visible = false;
                this.Btn06_Registration_department.Visible = false;
                this.Btn07_Credit_department.Visible = false;
                this.Btn08_Finance_department.Visible = false;
                this.Btn09_Accounting_department.Visible = false;
                this.Btn10_HR_department.Visible = false;
                this.Btn11_Report.Visible = false;
                this.Btn12_Set_license.Visible = false;

                this.Btn01_EnterPrise_manager_false.Visible = true;
                this.Btn01_EnterPrise_manager_1setup_Purchasing_false.Visible = true;
                this.Btn01_EnterPrise_manager_2setup_accounting_false.Visible = true;
                this.Btn01_EnterPrise_manager_3setup_Registration_false.Visible = true;
                this.Btn01_EnterPrise_manager_4setup_Warehouse_false.Visible = true;
                this.Btn02_Purchasing_Department_false.Visible = true;
                this.Btn03_Production_department_false.Visible = true;
                this.Btn04_Warehouse_department_false.Visible = true;
                this.Btn05_Sales_department_false.Visible = true;
                this.Btn06_Registration_department_false.Visible = true;
                this.Btn07_Credit_department_false.Visible = true;
                this.Btn08_Finance_department_false.Visible = true;
                this.Btn09_Accounting_department_false.Visible = true;
                this.Btn10_HR_department_false.Visible = true;
                this.Btn11_Report_false.Visible = true;
                this.Btn12_Set_license_false.Visible = true;
            }
            else
            {
                this.Btn13_Set_Support.Visible = false;
            }

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการออกจากโปรแกรม ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return;
            }
        }

        private void btnmaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
                this.btnmaximize.Visible = false;
                this.btnmaximize_full.Visible = true;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.btnmaximize.Visible = false;
                this.btnmaximize_full.Visible = true;
            }

            this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
            this.panel_low.Width = this.Width - this.panel_left.Width - 3;


        }

        private void btnmaximize_full_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.btnmaximize.Visible = true;
                this.btnmaximize_full.Visible = false;
            }
            this.panelDesktop.Width = this.Width - this.panel_left.Width -3;
            this.panel_low.Width = this.Width - this.panel_left.Width -3;



        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void BtnPowerOff_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการออกจากโปรแกรม ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

        }

        private void BtnSlide1_Click(object sender, EventArgs e)
        {
            if (this.panel_left.Width == 250)
            {
                this.panel_left.Width = 50;
                this.panelDesktop.Left = 53;
                this.panel_low.Left = 53;
                this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
                this.panel_low.Width = this.Width - this.panel_left.Width - 3;
            }
            else
            {
                this.panel_left.Width = 250;
                this.panelDesktop.Left = 253;
                this.panel_low.Left = 253;
                this.panelDesktop.Width = this.Width - this.panel_left.Width - 3;
                this.panel_low.Width = this.Width - this.panel_left.Width - 3;

            }
        }

        //Check USER Rule_ALL=====================================================================
        private void CHECK_USER_RULE_ALL()
        {
            //เชื่อมต่อฐานข้อมูล=======================================================
            //SqlConnection conn = new SqlConnection(KRest.W_ID_Select.conn_string);
            SqlConnection conn = new SqlConnection(
                new SqlConnectionStringBuilder()
                {
                    DataSource = W_ID_Select.ADATASOURCE,
                    InitialCatalog = W_ID_Select.DATABASE_NAME,
                    UserID = W_ID_Select.Crytal_USER,
                    Password = W_ID_Select.Crytal_Pass
                }
                .ConnectionString
            );
            try
            {
                //conn.Open();
                //MessageBox.Show("เชื่อมต่อฐานข้อมูลสำเร็จ....");

            }
            catch (SqlException)
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //END เชื่อมต่อฐานข้อมูล=======================================================
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            //ใส่รหัสฐานข้อมูล============================================
            //=======================================================
            string txtusername;
            //ใส่รหัสฐานข้อมูล user============================================
            string clearText_txtuser_name = W_ID_Select.M_USERNAME.Trim();
            string cipherText_txtuser_name = W_CryptorEngine.Encrypt(clearText_txtuser_name, true);
            txtusername = cipherText_txtuser_name.ToString();
            //=======================================================

            //=======================================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT *" +
                          " FROM A003user_sys_1depart" +
                          " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                           " AND (txtuser_name = '" + cipherText_txtuser_name.Trim() + "')" +
                           //" AND (txtsys_depart_id = '" + W_ID_Select.M_DEPART_NUMBER.Trim() + "')" +
                         " ORDER BY ID ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //1 EnterPrise Manager
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "1")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Enterprise_manager.Visible = true;
                                }
                                else
                                {
                                    this.panel_Enterprise_manager.Visible = false;
                                }
                            }
                            //2ฝ่ายจัดซื้อ
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "2")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn02_Purchasing_Department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn02_Purchasing_Department.Visible = false;
                                }
                            }
                            //3ฝ่ายผลิต
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "3")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn03_Production_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn03_Production_department.Visible = false;
                                }
                            }
                            //4ฝ่ายคลังสินค้า
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "4")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn04_Warehouse_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn04_Warehouse_department.Visible = false;
                                }
                            }
                            //5ฝ่ายขาย
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "5")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn05_Sales_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn05_Sales_department.Visible = false;
                                }
                            }
                            //6ฝ่ายทะเบียนสมาชิก
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "6")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn06_Registration_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn06_Registration_department.Visible = false;
                                }
                            }
                            //7ฝ่ายสินเชื่อ
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "7")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn07_Credit_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn07_Credit_department.Visible = false;
                                }
                            }
                            //8ฝ่ายการเงิน
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "8")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn08_Finance_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn08_Finance_department.Visible = false;
                                }
                            }
                            //9ฝ่ายบัญชี
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "9")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn09_Accounting_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn09_Accounting_department.Visible = false;
                                }
                            }
                            //10ฝ่ายบุคคล
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "10")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn10_HR_department.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn10_HR_department.Visible = false;
                                }
                            }
                            //11รายงาน
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "11")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn11_Report.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn11_Report.Visible = false;
                                }
                            }
                            //12กำหนดสิทธิใช้ระบบ 
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "12")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn12_Set_license.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn12_Set_license.Visible = false;
                                }
                            }
                            //13แจ้ง Support
                            if (dt2.Rows[j]["txtsys_depart_id"].ToString() == "13")
                            {
                                if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                {
                                    this.panel_Btn13_Set_Support.Visible = true;
                                }
                                else
                                {
                                    this.panel_Btn13_Set_Support.Visible = false;
                                }
                            }



                        }
                        //=======================================================
                    }
                    else
                    {

                        this.panel_Enterprise_manager.Visible = false;
                        this.panel_Btn02_Purchasing_Department.Visible = false;
                        this.panel_Btn03_Production_department.Visible = false;
                        this.panel_Btn04_Warehouse_department.Visible = false;
                        this.panel_Btn05_Sales_department.Visible = false;
                        this.panel_Btn06_Registration_department.Visible = false;
                        this.panel_Btn07_Credit_department.Visible = false;
                        this.panel_Btn08_Finance_department.Visible = false;
                        this.panel_Btn09_Accounting_department.Visible = false;
                        this.panel_Btn10_HR_department.Visible = false;
                        this.panel_Btn11_Report.Visible = false;
                        this.panel_Btn12_Set_license.Visible = false;
                        this.panel_Btn13_Set_Support.Visible = false;

                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("kondate.soft", ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }

                //===========================================
            }
            //================================
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                this.panel_Enterprise_manager.Visible = true;
                this.panel_Btn02_Purchasing_Department.Visible = true;
                this.panel_Btn03_Production_department.Visible = true;
                this.panel_Btn04_Warehouse_department.Visible = true;
                this.panel_Btn05_Sales_department.Visible = true;
                this.panel_Btn06_Registration_department.Visible = true;
                this.panel_Btn07_Credit_department.Visible = true;
                this.panel_Btn08_Finance_department.Visible = true;
                this.panel_Btn09_Accounting_department.Visible = true;
                this.panel_Btn10_HR_department.Visible = true;
                this.panel_Btn11_Report.Visible = true;
                this.panel_Btn12_Set_license.Visible = true;
                this.panel_Btn13_Set_Support.Visible = true;

            }

        }
        //END Check USER Rule_ALL=====================================================================


        //Check USER Rule=====================================================================
        private void CHECK_USER_RULE()
        {
            //เชื่อมต่อฐานข้อมูล=======================================================
            //SqlConnection conn = new SqlConnection(KRest.W_ID_Select.conn_string);
            SqlConnection conn = new SqlConnection(
                new SqlConnectionStringBuilder()
                {
                    DataSource = W_ID_Select.ADATASOURCE,
                    InitialCatalog = W_ID_Select.DATABASE_NAME,
                    UserID = W_ID_Select.Crytal_USER,
                    Password = W_ID_Select.Crytal_Pass
                }
                .ConnectionString
            );
            try
            {
                //conn.Open();
                //MessageBox.Show("เชื่อมต่อฐานข้อมูลสำเร็จ....");

            }
            catch (SqlException)
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //END เชื่อมต่อฐานข้อมูล=======================================================
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            //ใส่รหัสฐานข้อมูล============================================
            //=======================================================
            string txtusername;
            //ใส่รหัสฐานข้อมูล user============================================
            string clearText_txtuser_name = W_ID_Select.M_USERNAME.Trim();
            string cipherText_txtuser_name = W_CryptorEngine.Encrypt(clearText_txtuser_name, true);
            txtusername = cipherText_txtuser_name.ToString();
            //=======================================================

            //=======================================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT *" +
                          " FROM A003user_sys_1depart" +
                          " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                           " AND (txtuser_name = '" + cipherText_txtuser_name.Trim() + "')" +
                           " AND (txtsys_depart_id = '" + W_ID_Select.M_DEPART_NUMBER.Trim() + "')" +
                         " ORDER BY ID ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //6
                            if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                            {
                                W_ID_Select.M_DEPART_LOGIN = "Y";
                            }
                            else
                            {
                                W_ID_Select.M_DEPART_LOGIN = "N";
                            }
                        }
                        //=======================================================
                    }
                    else
                    {

                        W_ID_Select.M_DEPART_LOGIN = "N";

                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("kondate.soft", ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }

                //===========================================
            }
            //================================
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.M_DEPART_LOGIN = "Y";

            }

        }
        //END Check USER Rule=====================================================================


        //Tans_Log ====================================================================
        private void TRANS_LOG()
        {
            //======================================================
            //เชื่อมต่อฐานข้อมูล=======================================================
            //SqlConnection conn = new SqlConnection(KRest.W_ID_Select.conn_string);
            SqlConnection conn = new SqlConnection(
                new SqlConnectionStringBuilder()
                {
                    DataSource = W_ID_Select.ADATASOURCE,
                    InitialCatalog = W_ID_Select.DATABASE_NAME,
                    UserID = W_ID_Select.Crytal_USER,
                    Password = W_ID_Select.Crytal_Pass
                }
                .ConnectionString
            );
            try
            {
                //conn.Open();
                //MessageBox.Show("เชื่อมต่อฐานข้อมูลสำเร็จ....");

            }
            catch (SqlException)
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //END เชื่อมต่อฐานข้อมูล=======================================================
            //จบเชื่อมต่อฐานข้อมูล=======================================================

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                SqlTransaction trans;
                trans = conn.BeginTransaction();
                cmd2.Transaction = trans;
                try
                {

                    cmd2.CommandText = "INSERT INTO A001_trans_log(cdkey,txtco_id,txtbranch_id," +  //1
                                                                                                    //"txttrans_date," +
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                           "txtform_name,txtform_caption," +  //5
                                            "txtuser_name,txtemp_office_name," +  //6
                                           "txtlog_id,txtlog_name," +  //7
                                          "txtdocument_id,txtversion_id,txtcount) " +  //8
                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                                                                        //"@txttrans_date," +
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtform_name,@txtform_caption," +  //5
                                           "@txtuser_name,@txtemp_office_name," +  //6
                                           "@txtlog_id,@txtlog_name," +  //7
                                           "@txtdocument_id,@txtversion_id,@txtcount)";   //8

                    cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                    cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                    cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();

                    String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                    DateTime myDateTime = new DateTime();
                    myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

                    String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                    DateTime myDateTime2 = new DateTime();
                    myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);


                    cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);

                    //    cmd2.Parameters.Add("@txttrans_date1", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);
                    //    cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = DateTime.Now.ToString("HH:mm:ss");
                    //    cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy", UsaCulture);
                    //    cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = DateTime.Now.ToString("MM", UsaCulture);
                    //    cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = DateTime.Now.ToString("dd", UsaCulture);

                    cmd2.Parameters.Add("@txtcomputer_ip", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_IP.Trim();
                    cmd2.Parameters.Add("@txtcomputer_name", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim();
                    cmd2.Parameters.Add("@txtform_name", SqlDbType.NVarChar).Value = this.Name.ToString();
                    cmd2.Parameters.Add("@txtform_caption", SqlDbType.NVarChar).Value = this.Text.ToString();
                    cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = W_ID_Select.M_USERNAME.Trim();
                    cmd2.Parameters.Add("@txtemp_office_name", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
                    cmd2.Parameters.Add("@txtlog_id", SqlDbType.NVarChar).Value = W_ID_Select.LOG_ID.Trim();
                    cmd2.Parameters.Add("@txtlog_name", SqlDbType.NVarChar).Value = W_ID_Select.LOG_NAME.Trim();
                    cmd2.Parameters.Add("@txtdocument_id", SqlDbType.NVarChar).Value = W_ID_Select.DOCUMENT_ID.Trim();
                    cmd2.Parameters.Add("@txtversion_id", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();
                    cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 1));

                    //==============================
                    //1 Login
                    //2 Logout
                    //3 ใหม่
                    //4 เปิด
                    //5 บันทึกใหม่
                    //6 บันทึกแก้ไข
                    //7  ยกเลิกเอกสาร
                    //8 ปริ๊น
                    //9 ปิดหน้าจอ
                    //==============================
                    cmd2.ExecuteNonQuery();



                    trans.Commit();
                    conn.Close();
                }

                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("kondate.soft", ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }
            //=============================================================

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invoke(new EventHandler(delegate
            {
                //    this.txttime_from_server.Text = DateTime.Now.ToString();
                this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "       |      Host : " + W_ID_Select.ADATASOURCE.ToString() + "      |     Database : " + W_ID_Select.DATABASE_NAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "";

            }));
        }



        //Tans_Log ====================================================================

    }
}
