using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlTypes;

using System.IO;
using System.Threading;
using System.Globalization;

using System.Deployment;
using System.Deployment.Application;
using System.Reflection;
using System.Management;

using System.Net;
using System.Runtime.InteropServices;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace kondate.soft.HOME03_Production
{
    public partial class HOME03_Production_13send_FG2_Reduce_Debt_record_print : Form
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
        //ประกาศ DateTime เพื่อมาเป็นเวลาปัจจุบัน

        //เชื่อมต่อฐานข้อมูล=======================================================
        //SqlConnection conn = new SqlConnection(KRest.W_ID_Select.conn_string);

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public HOME03_Production_13send_FG2_Reduce_Debt_record_print()
        {
            InitializeComponent();
        }

        private void HOME03_Production_13send_FG2_Reduce_Debt_record_print_Load(object sender, EventArgs e)
        {
            //W_ID_Select.TRANS_ID = "PRHO-21-000002";

            //MessageBox.Show("" + W_ID_Select.CDKEY.Trim() + "");
            //MessageBox.Show("" + W_ID_Select.M_COID.Trim() + "");
            //MessageBox.Show("" + W_ID_Select.TRANS_ID.Trim() + "");

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();

            TableLogOnInfo cr_table_logon_info = new TableLogOnInfo();
            ConnectionInfo cr_Connection_Info = new ConnectionInfo();
            Tables CrTables;

            ReportDocument rpt = new ReportDocument();

            //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_Chart_of_accounts.rpt");
            //E:\01_Project_ERP_Kondate.Soft\kondate.soft\kondate.soft\bin\Debug\KONDATE_REPORT
            //E:\01_Project_ERP_Kondate.Soft\kondate.soft\kondate.soft\KONDATE_REPORT\Report_Chart_of_accounts.rpt
            //C:\KD_ERP\KD_REPORT


            rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_13send_FG2_Reduce_Debt_record.rpt");
            //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_c002_01berg_produce_record.rpt");


            string cr_server = W_ID_Select.ADATASOURCE.Trim();
            string cr_database = W_ID_Select.DATABASE_NAME.ToString();
            string cr_user = W_ID_Select.Crytal_USER.ToString();
            string cr_pass = W_ID_Select.Crytal_Pass.ToString();

            cr_Connection_Info.DatabaseName = cr_server;
            cr_Connection_Info.DatabaseName = cr_database;
            cr_Connection_Info.UserID = cr_user;
            cr_Connection_Info.Password = cr_pass;
            cr_Connection_Info.IntegratedSecurity = false;
            CrTables = rpt.Database.Tables;


            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in CrTables)
            {
                cr_table_logon_info = crTable.LogOnInfo;
                cr_table_logon_info.ConnectionInfo = cr_Connection_Info;
                crTable.ApplyLogOnInfo(cr_table_logon_info);
            }
            foreach (ReportDocument subreport in rpt.Subreports)
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in subreport.Database.Tables)
                {
                    cr_table_logon_info = crTable.LogOnInfo;
                    cr_table_logon_info.ConnectionInfo = cr_Connection_Info;
                    crTable.ApplyLogOnInfo(cr_table_logon_info);
                }
            }


            rpt.SetParameterValue("cdkey", W_ID_Select.CDKEY.Trim());
            rpt.SetParameterValue("txtco_id", W_ID_Select.M_COID.Trim());
            rpt.SetParameterValue("txtRCD_id", W_ID_Select.TRANS_ID.Trim());

            this.crystalReportViewer1.ReportSource = rpt;

        }

        private void iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        bool allowResize = false;
        private void button_low_right_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void button_low_right_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.Height = button_low_right.Top + e.Y;
                this.Width = button_low_right.Left + e.X;
            }
        }
        private void button_low_right_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

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
        }

        private void btnmaximize_full_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.btnmaximize.Visible = true;
                this.btnmaximize_full.Visible = false;
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //====================================================================
    }
}
