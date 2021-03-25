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
using System.Timers;
using System.Globalization;

using System.Deployment;
using System.Deployment.Application;
using System.Reflection;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Net.Cache;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace kondate.soft
{
    public partial class LOGIN : Form
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

        bool allowResize = false;

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

        public LOGIN()
        {
            InitializeComponent();

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;

        }

        private void LOGIN_Load(object sender, EventArgs e)
        {

            this.iblVersion_id.Text = "";
            this.iblVersion_id.Text = W_ID_Select.GetVersion();
            W_ID_Select.VERSION_ID = W_ID_Select.GetVersion();

            this.TopMost = true; //Lock Form ไม่ให้เคลื่อน
            this.txtHost_name.Text = System.Environment.MachineName + "\\SQLEXPRESS,49170";

            //สร้าง folder KONDATE สำหรับ text file ต่างๆ==========================
            string path = @"C:\KD_ERP";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //=======================================================
            string path2 = @"C:\KD_ERP\KD_REPORT";
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
            //Create Text File=======================================================
            string path_a_1cdkey = @"C:\KD_ERP\a_1cdkey.txt";
            if (!File.Exists(path_a_1cdkey))
            {
                // Create a file to write to.
                using (StreamWriter sw0 = File.CreateText(path_a_1cdkey))
                {
                    sw0.WriteLine(this.txtcdkey.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr0 = File.OpenText(path_a_1cdkey))
            {
                string s0 = "";
                while ((s0 = sr0.ReadLine()) != null)
                {
                    this.txtcdkey.Text = s0.ToString();
                    W_ID_Select.CDKEY = s0.ToString();

                }
            }
            //End Create Text File=======================================================

            //Create Text File=======================================================
            string path_host = @"C:\KD_ERP\host.txt";
            if (!File.Exists(path_host))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path_host))
                {
                    sw.WriteLine(this.txtHost_name.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr = File.OpenText(path_host))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    this.txtHost_name.Text = s.ToString();
                    W_ID_Select.ADATASOURCE = s.ToString();

                }
            }
            //End Create Text File=======================================================
            //Create Text File=======================================================
            string path_db = @"C:\KD_ERP\db.txt";
            if (!File.Exists(path_db))
            {
                // Create a file to write to.
                using (StreamWriter sw_db = File.CreateText(path_db))
                {
                    sw_db.WriteLine(this.txtDb_name.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr_db = File.OpenText(path_db))
            {
                string sdb = "";
                while ((sdb = sr_db.ReadLine()) != null)
                {
                    this.txtDb_name.Text = sdb.ToString();
                    W_ID_Select.DATABASE_NAME = sdb.ToString();

                }
            }
            //End Create Text File=======================================================
            //Create Text File=======================================================
            string path_user = @"C:\KD_ERP\user.txt";
            if (!File.Exists(path_user))
            {
                // Create a file to write to.
                using (StreamWriter sw_user = File.CreateText(path_user))
                {
                    sw_user.WriteLine(this.txtuser_name.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr_user = File.OpenText(path_user))
            {
                string suser = "";
                while ((suser = sr_user.ReadLine()) != null)
                {
                    this.txtuser_name.Text = suser.ToString();

                }
            }
            //End Create Text File=======================================================

            //Create Text File=======================================================
            string path_a_3co_name = @"C:\KD_ERP\a_3co_name.txt";
            if (!File.Exists(path_a_3co_name))
            {
                // Create a file to write to.
                using (StreamWriter sw_a_3co_name = File.CreateText(path_a_3co_name))
                {
                    sw_a_3co_name.WriteLine(this.PANEL1_CO_txtco_name.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr_a_3co_name = File.OpenText(path_a_3co_name))
            {
                string sa_3co_name = "";
                while ((sa_3co_name = sr_a_3co_name.ReadLine()) != null)
                {
                    this.PANEL1_CO_txtco_name.Text = sa_3co_name.ToString();
                    W_ID_Select.M_CONAME = sa_3co_name.ToString();
                }
            }
            //End Create Text File=======================================================
            //Create Text File=======================================================
            string path_a_2co_id = @"C:\KD_ERP\a_2co_id.txt";
            if (!File.Exists(path_a_2co_id))
            {
                // Create a file to write to.
                using (StreamWriter sw_a_2co_id = File.CreateText(path_a_2co_id))
                {
                    sw_a_2co_id.WriteLine(this.PANEL1_CO_txtco_id.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr_a_2co_id = File.OpenText(path_a_2co_id))
            {
                string sa_2co_id = "";
                while ((sa_2co_id = sr_a_2co_id.ReadLine()) != null)
                {
                    this.PANEL1_CO_txtco_id.Text = sa_2co_id.ToString();
                    W_ID_Select.M_COID = sa_2co_id.ToString();
                }
            }
            //End Create Text File=======================================================

            //Create Text File=======================================================
            string path_a_5branch_name = @"C:\KD_ERP\a_5branch_name.txt";
            if (!File.Exists(path_a_5branch_name))
            {
                // Create a file to write to.
                using (StreamWriter sw_a_5branch_name = File.CreateText(path_a_5branch_name))
                {
                    sw_a_5branch_name.WriteLine(this.PANEL2_BRANCH_txtbranch_name.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr_a_5branch_name = File.OpenText(path_a_5branch_name))
            {
                string sa_5branch_name = "";
                while ((sa_5branch_name = sr_a_5branch_name.ReadLine()) != null)
                {
                    this.PANEL2_BRANCH_txtbranch_name.Text = sa_5branch_name.ToString();

                }
            }
            //End Create Text File=======================================================
            //Create Text File=======================================================
            string path_a_4branch_id = @"C:\KD_ERP\a_4branch_id.txt";
            if (!File.Exists(path_a_4branch_id))
            {
                // Create a file to write to.
                using (StreamWriter sw_a_4branch_id = File.CreateText(path_a_4branch_id))
                {
                    sw_a_4branch_id.WriteLine(this.PANEL2_BRANCH_txtbranch_id.Text.Trim());
                }
            }
            // Open the file to read from.
            using (StreamReader sr_a_4branch_id = File.OpenText(path_a_4branch_id))
            {
                string sa_4branch_id = "";
                while ((sa_4branch_id = sr_a_4branch_id.ReadLine()) != null)
                {
                    this.PANEL2_BRANCH_txtbranch_id.Text = sa_4branch_id.ToString();
                }
            }

            //FillCboCompany();
            //FillCboBranch();

            //1============================
            this.txtcomputer_name.Text = System.Environment.MachineName.ToString();
            W_ID_Select.COMPUTER_NAME = System.Environment.MachineName.ToString();

            //2===========================
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.txtcomputer_ip.Text = ip.ToString();
                    W_ID_Select.COMPUTER_IP = ip.ToString();
                }
            }

            FillDATE_FROM_SERVER();

            //3============================


            if (this.txtcdkey.Text == "")
            {
                  MessageBox.Show("ไม่พบ ซีเรียล No !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            PANEL1_CO_GridView1_co();
            PANEL1_CO_Fill_CO();

            PANEL2_BRANCH_GridView1_branch();
            PANEL2_BRANCH_Fill_branch();


            CHECK_VERSION();
  
        }

        private void CHECK_VERSION()
        {

            //END เชื่อมต่อฐานข้อมูล=======================================================

            if (W_ID_Select.M_COID.Trim() == "KD")
            {
                this.txtuser_name.Text = "admin";
                this.txtuser_pass.Text = "1234";
                this.txtsleep.Text = "100";
                W_ID_Select.SLEEP = Convert.ToInt16(string.Format("{0:n4}", this.txtsleep.Text.ToString()));
                this.check_version.Checked = false;
            }
            else
            {
                this.txtuser_name.Text = "admin";
                this.txtuser_pass.Text = "1234";
                this.txtsleep.Text = "100";
                W_ID_Select.SLEEP = Convert.ToInt16(string.Format("{0:n4}", this.txtsleep.Text.ToString()));

                //this.txtuser_pass.Text = "";
                //this.txtsleep.Text = "10000";
                //W_ID_Select.SLEEP = Convert.ToInt16(string.Format("{0:n4}", this.txtsleep.Text.ToString()));

                this.check_version.Checked = true;

            }

       }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO_txtco_id.Text == "")
            {
                MessageBox.Show("เลือก บริษัทฯ เพื่อเข้าระบบก่อน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.PANEL1_CO_txtco_name.Text == "")
            {
                MessageBox.Show("เลือก บริษัทฯ เพื่อเข้าระบบก่อน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.PANEL2_BRANCH_txtbranch_id.Text == "")
            {
                MessageBox.Show("เลือก สาขา เพื่อเข้าระบบก่อน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.PANEL2_BRANCH_txtbranch_name.Text == "")
            {
                MessageBox.Show("เลือก สาขา เพื่อเข้าระบบก่อน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //CHECK_VERSION
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
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k008db_branch" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbranch_id = '" + this.PANEL2_BRANCH_txtbranch_id.Text.Trim() + "')" +
                                  " ORDER BY ID ASC";

                //  " AND (k004db_foods_order_1total.txtmat_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

                //   " AND (k011db_receipt.daily_status = '0')";

                //cmd1.Parameters.Add("@txtreceipt_date_start", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd1.Parameters.Add("@txtreceipt_date_end", SqlDbType.Date).Value = this.dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        W_ID_Select.M_BRANCHNAME_SHORT = dt2.Rows[0]["txtbranch_name_short"].ToString();      //1

                    }
                    else
                    {
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


            //
            conn.Close();



            //CHECK_VERSION
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT * FROM A001_sys_version";
                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //MessageBox.Show(""+ dt.Rows[0]["txtversion_id"].ToString() +"");
                    if (this.iblVersion_id.Text.Trim() == dt.Rows[0]["txtversion_id"].ToString())
                    {

                    }
                    else
                    {
                        //if (this.check_version.Checked==true)
                        //{

                        //}

                        if (this.check_version.Checked == true)
                        {
                            //////==================================================
                            //MessageBox.Show("โปรด Update Version ปัจจุบัน  :  " + dt.Rows[0]["txtversion_id"].ToString() + "  ก่อนใช้งาน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            //conn.Close();
                            //Application.Exit();
                            //////==================================================

                        }

                    }
                }
            }

            //
            conn.Close();




            //Create Text File=======================================================
            string path_a_1cdkey = @"C:\KD_ERP\a_1cdkey.txt";
            // Create a file to write to.
            using (StreamWriter sw0 = File.CreateText(path_a_1cdkey))
            {
                sw0.WriteLine(this.txtcdkey.Text.Trim());
                W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            }

            //Create Text File=======================================================
            string path_host = @"C:\KD_ERP\host.txt";
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path_host))
            {
                sw.WriteLine(this.txtHost_name.Text.Trim());
                W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            }
            //Create Text File=======================================================
            string path_db = @"C:\KD_ERP\db.txt";
            // Create a file to write to.
            using (StreamWriter sw_db = File.CreateText(path_db))
            {
                sw_db.WriteLine(this.txtDb_name.Text.Trim());
                W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            }
            //Create Text File=======================================================
            string path_a_3co_name = @"C:\KD_ERP\a_3co_name.txt";
            // Create a file to write to.
            using (StreamWriter sw_a_3co_name = File.CreateText(path_a_3co_name))
            {
                sw_a_3co_name.WriteLine(this.PANEL1_CO_txtco_name.Text.Trim());
                W_ID_Select.M_CONAME = this.PANEL1_CO_txtco_name.Text.Trim();
            }
            //================================

            //Create Text File=======================================================
            string path_a_2co_id = @"C:\KD_ERP\a_2co_id.txt";
            // Create a file to write to.
            using (StreamWriter sw_a_2co_id = File.CreateText(path_a_2co_id))
            {
                sw_a_2co_id.WriteLine(this.PANEL1_CO_txtco_id.Text.Trim());
                W_ID_Select.M_COID = this.PANEL1_CO_txtco_id.Text.Trim();
            }
            //================================
            //Create Text File=======================================================
            string path_a_5branch_name = @"C:\KD_ERP\a_5branch_name.txt";
            // Create a file to write to.
            using (StreamWriter sw_a_5branch_name = File.CreateText(path_a_5branch_name))
            {
                sw_a_5branch_name.WriteLine(this.PANEL2_BRANCH_txtbranch_name.Text.Trim());
                W_ID_Select.M_BRANCHNAME = this.PANEL2_BRANCH_txtbranch_name.Text.Trim();
            }
            //================================
            //Create Text File=======================================================
            string path_a_4branch_id = @"C:\KD_ERP\a_4branch_id.txt";
            // Create a file to write to.
            using (StreamWriter sw_a_4branch_id = File.CreateText(path_a_4branch_id))
            {
                sw_a_4branch_id.WriteLine(this.PANEL2_BRANCH_txtbranch_id.Text.Trim());
                W_ID_Select.M_BRANCHID = this.PANEL2_BRANCH_txtbranch_id.Text.Trim();
            }
            //================================

            //Create Text File=======================================================
            string path_user = @"C:\KD_ERP\user.txt";
            // Create a file to write to.
            using (StreamWriter sw_user = File.CreateText(path_user))
            {
                sw_user.WriteLine(this.txtuser_name.Text.Trim());
            }
            //================================
            //================================
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.COMPUTER_IP = this.txtcomputer_ip.Text.Trim();
            W_ID_Select.COMPUTER_NAME = this.txtcomputer_name.Text.Trim();

            //================================

            if (this.txtuser_name.Text == "")
            {
                MessageBox.Show("ใส่ User Name ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtuser_name.Focus();
                return;
            }
            if (this.txtuser_pass.Text == "")
            {
                MessageBox.Show("ใส่รหัสผ่าน ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtuser_pass.Focus();
                return;
            }
            //ใส่รหัสฐานข้อมูล============================================
            string txtusername;
            //ใส่รหัสฐานข้อมูล user============================================
            string clearText_txtuser_name = this.txtuser_name.Text.Trim();
            string cipherText_txtuser_name = W_CryptorEngine.Encrypt(clearText_txtuser_name, true);
            txtusername = cipherText_txtuser_name.ToString();
            //=======================================================

            //=======================================================
            //ใส่รหัสฐานข้อมูล============================================
            string txtuserpass;
            string clearText_txtuser_pass = this.txtuser_pass.Text.Trim();
            string cipherText_txtuser_pass = W_CryptorEngine.Encrypt(clearText_txtuser_pass, true);
            txtuserpass = cipherText_txtuser_pass.ToString();

            //=======================================================

            //END เชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                  " FROM a003db_user" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtuser_name = '" + txtusername.Trim() + "')";
                //     " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() +"')";
                //ASC,DESC
                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        if (dt.Rows[0]["txtuser_pass"].ToString() == txtuserpass.Trim())
                        {

                            W_ID_Select.M_USERNAME = "";
                            W_ID_Select.M_USERNAME_TYPE = "";

                            //W_ID_Select.M_USERNAME = dt.Rows[0]["txtuser_name"].ToString();
                            W_ID_Select.M_USERNAME = this.txtuser_name.Text.Trim();
                            W_ID_Select.M_USERNAME_TYPE = dt.Rows[0]["txtuser_type_id"].ToString();

                            //ใส่รหัสฐานข้อมูล============================================
                            string clearText_txtname = dt.Rows[0]["txtname"].ToString();
                            string cipherText_txtname = W_CryptorEngine.Decrypt(clearText_txtname, true);
                            W_ID_Select.M_EMP_OFFICE_NAME = cipherText_txtname.ToString();


                            //Form_k004db_foods_order frm2 = new Form_k004db_foods_order();
                            //frm2.Show();
                            // Form_k004db_foods_order.ActiveForm.WindowState = FormWindowState.Minimized;
                            this.Hide();

                            W_ID_Select.LOG_ID = "1";
                            W_ID_Select.LOG_NAME = "Login";
                            TRANS_LOG();
                            //Main frm = new Main();
                            //frm.Show();

                            HOME frm = new HOME();
                            frm.Show();

                            string txt_user = this.txtuser_name.Text.ToString();
                            System.IO.File.WriteAllText(@"C:\KD_ERP\user.txt", txt_user);

                        }
                        else
                        {
                            MessageBox.Show("รหัสผ่านไม่ถูกต้อง !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    else
                    {
                        MessageBox.Show("ไม่พบ User name นี้ !   ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        return;
                    }
                    conn.Close();
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

            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================

        }

        private void LOGIN_Shown(object sender, EventArgs e)
        {




        }

        private void FillDATE_FROM_SERVER()
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

                    cmd2.CommandText = "UPDATE A001_date_now SET " +
                                                                 "datetime_now = GETDATE()";
                        cmd2.ExecuteNonQuery();



                        Cursor.Current = Cursors.WaitCursor;
                        trans.Commit();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

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




            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT datetime_now" +
                                  " FROM A001_date_now";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtdate_from_server.Text = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("dd-MM-yyyy", UsaCulture);          //4
                        this.txttime_from_server.Text = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("HH:mm:ss", UsaCulture);          //4

                        string D1  = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("yyyy-MM-dd", UsaCulture);          //4
                        string T1 = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("HH:mm:ss", UsaCulture);          //4
                        W_ID_Select.DATE_FROM_SERVER = D1.ToString();
                        W_ID_Select.TIME_FROM_SERVER = T1.ToString();

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

            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            conn.Close();
        }
   
        private void Ch_Config_db_server_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Ch_Config_db_server.Checked == true)
            {
                this.groupBox3_config.Visible = true;
                if (this.txtuser_name.Text == "admin")
                {
                    this.checkBox1_TEST_File.Visible = true;
                }
                else
                {
                    this.checkBox1_TEST_File.Visible = false;
                }
            }
            else
            {
                this.groupBox3_config.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invoke(new EventHandler(delegate
            {
                //    this.txttime_from_server.Text = DateTime.Now.ToString();
                this.txttime_from_server.Text = DateTime.Now.ToString("HH:mm:ss");
            }));
        }

        private void checkBox1_Copy_file_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1_Copy_file.Checked == true)
            {
                string SourcePath = @"\\192.168.0.3\\Update_Krest\\Samn\\KD_ERP\\KD_REPORT";
                string DestinationPath = @"C:\\KD_ERP\\KD_REPORT";


                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                                SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                                SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

            }
        }
        private void checkBox1_TEST_File_CheckedChanged(object sender, EventArgs e)
        {
            CHECK_VERSION();

            if (this.checkBox1_TEST_File.Checked == true)
            {
                PANEL1_CO_Fill_CO();

                this.iblEnrypt.Visible = true;
                this.iblCaption.Visible = true;
                this.txtCaption.Visible = true;
                this.txtEncrypt.Visible = true;
                this.btnCaption.Visible = true;
                this.btnEncrypt.Visible = true;
                this.Height = 606;
            }
            else
            {
                PANEL1_CO_Fill_CO();

                this.iblEnrypt.Visible = false;
                this.iblCaption.Visible = false;
                this.txtCaption.Visible = false;
                this.txtEncrypt.Visible = false;
                this.btnCaption.Visible = false;
                this.btnEncrypt.Visible = false;
                this.Height = 521;

            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
        }
        private void btnOK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.txtuser_pass.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                this.BtnCancel.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
            }
        }

        private void btnTest_Connect_Click(object sender, EventArgs e)
        {
            W_ID_Select.LOG_ID = "10";
            W_ID_Select.LOG_NAME = "Test Connection";
            TRANS_LOG();

            this.btnTest_Connect.Enabled = false;

            string DATASOURCE = "";//916909b5121b.sn.mynetname.net,6001
            string DATABASENAME = "";

            DATASOURCE = this.txtHost_name.Text.ToString();
            DATABASENAME = this.txtDb_name.Text.ToString();

            W_ID_Select.ADATASOURCE = DATASOURCE.Trim();
            W_ID_Select.DATABASE_NAME = DATABASENAME.Trim();


            //MessageBox.Show("" + W_ID_Select.ADATASOURCE + "");
            //MessageBox.Show("" + W_ID_Select.DATABASE_NAME + "");

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
                conn.Open();

                //Create Text File=======================================================
                string path_host = @"C:\KD_ERP\host.txt";
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path_host))
                {
                    sw.WriteLine(this.txtHost_name.Text.Trim());
                }
                //Create Text File=======================================================
                string path_db = @"C:\KD_ERP\db.txt";
                // Create a file to write to.
                using (StreamWriter sw_db = File.CreateText(path_db))
                {
                    sw_db.WriteLine(this.txtDb_name.Text.Trim());
                }
                //================================
                W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
                W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
                //================================

                MessageBox.Show("เชื่อมต่อฐานข้อมูลสำเร็จ...", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (SqlException)
            {
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.btnTest_Connect.Enabled = true;
                return;
            }
            //END เชื่อมต่อฐานข้อมูล=======================================================
            this.btnTest_Connect.Enabled = true;

        }

        private void BtnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.btnOK.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                this.txtuser_name.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            W_ID_Select.LOG_ID = "9";
            W_ID_Select.LOG_NAME = "ปิดหน้าจอ";
            TRANS_LOG();

            Application.Exit();

        }

        private void txtuser_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
            }
            if (e.KeyCode == Keys.Down)
            {
                this.txtuser_pass.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
            }
        }

        private void txtuser_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.txtuser_pass.Focus();
            }
        }

        private void txtuser_pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.txtuser_name.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                this.btnOK.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
            }
        }

        private void txtuser_pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.btnOK.Focus();
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            W_ID_Select.LOG_ID = "9";
            W_ID_Select.LOG_NAME = "ปิดหน้าจอ";
            TRANS_LOG();

            Application.Exit();

        }
        //Company=======================================================================
        private void PANEL1_CO_Fill_CO()
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

            //===========================================

            PANEL1_CO_Clear_GridView1_co();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (checkBox1_TEST_File.Checked == true)
                {
                    cmd2.CommandText = "SELECT *" +
                                      " FROM k009db_business" +
                                      " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_id <> '')" +
                                      " ORDER BY ID ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT *" +
                                      " FROM k009db_business" +
                                      " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_status = '0')" +
                                      " AND (txtco_id <> '')" +
                                      " ORDER BY ID ASC";
                }

                //  " AND (k004db_foods_order_1total.txtmat_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

                //   " AND (k011db_receipt.daily_status = '0')";

                //cmd1.Parameters.Add("@txtreceipt_date_start", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd1.Parameters.Add("@txtreceipt_date_end", SqlDbType.Date).Value = this.dtpend.Value;

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
                            var index = PANEL1_CO_dataGridView1_co.Rows.Add();
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_name"].Value = dt2.Rows[j]["txtco_name"].ToString();      //2
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_status"].Value = dt2.Rows[j]["txtco_status"].ToString();      //4
                        }
                        PANEL1_CO_GridView1_co_Up_Status();

                        //=======================================================
                    }
                    else
                    {
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

        }
        private void PANEL1_CO_GridView1_co()
        {
            this.PANEL1_CO_dataGridView1_co.ColumnCount = 5;
            this.PANEL1_CO_dataGridView1_co.Columns[0].Name = "Col_Auto_num";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Name = "Col_txtco_id";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Name = "Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[3].Name = "Col_txthome_id_full";
            this.PANEL1_CO_dataGridView1_co.Columns[4].Name = "Col_txtco_status";

            this.PANEL1_CO_dataGridView1_co.Columns[0].HeaderText = "No";
            this.PANEL1_CO_dataGridView1_co.Columns[1].HeaderText = "รหัสกิจการ";
            this.PANEL1_CO_dataGridView1_co.Columns[2].HeaderText = "ชื่อกิจการ";
            this.PANEL1_CO_dataGridView1_co.Columns[3].HeaderText = "ที่อยู่";  //
            this.PANEL1_CO_dataGridView1_co.Columns[4].HeaderText = "สถานะ";

            this.PANEL1_CO_dataGridView1_co.Columns[0].Visible = false;  //"No";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Visible = true;  //"Col_txtco_id";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Width = 80;
            this.PANEL1_CO_dataGridView1_co.Columns[1].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[2].Visible = true;  //"Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Width = 250;
            this.PANEL1_CO_dataGridView1_co.Columns[2].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[3].Visible = false; // "Col_txthome_id_full
            this.PANEL1_CO_dataGridView1_co.Columns[3].Width =0;
            this.PANEL1_CO_dataGridView1_co.Columns[3].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[4].Visible = false;  // "Col_txtco_status
            this.PANEL1_CO_dataGridView1_co.Columns[4].Width = 0;
            this.PANEL1_CO_dataGridView1_co.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL1_CO_dataGridView1_co.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1_CO_dataGridView1_co.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1_CO_dataGridView1_co.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.FillWeight = 10;
            dgvCmb.ReadOnly = true;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL1_CO_dataGridView1_co.Columns.Add(dgvCmb);

        }
        private void PANEL1_CO_Clear_GridView1_co()
        {
            this.PANEL1_CO_dataGridView1_co.Rows.Clear();
            this.PANEL1_CO_dataGridView1_co.Refresh();
        }
        private void PANEL1_CO_GridView1_co_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL1_CO_dataGridView1_co.Rows.Count ; i++)
            {
                if (this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[4].Value.ToString() == "0")  //Active
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[5].Value = true;
                }
                else
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[5].Value = false;

                }
            }
        }
        private void PANEL1_CO_btnco_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
                this.PANEL1_CO.Location  = new Point(120, this.iblPoint.Location.Y + 2);
            }
            else
            {
                this.PANEL1_CO.Visible = false;
            }
        }
        private void PANEL1_CO_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
            }
            else
            {
                this.PANEL1_CO.Visible = false;
            }
        }
 
        private void PANEL1_CO_dataGridView1_co_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1_CO_dataGridView1_co.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1_CO_txtco_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1_CO_txtco_name.Text = row.Cells[2].Value.ToString();
                    W_ID_Select.M_COID = row.Cells[1].Value.ToString();
                    W_ID_Select.M_CONAME = row.Cells[2].Value.ToString();
                    this.PANEL2_BRANCH_txtbranch_id.Text = "";
                    this.PANEL2_BRANCH_txtbranch_name.Text = "";


                }
            }
        }
        private void PANEL1_CO_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1_CO_btn_search_Click(object sender, EventArgs e)
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

            //===========================================

            PANEL1_CO_Clear_GridView1_co();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k009db_business" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_name LIKE '%" + this.PANEL1_CO_txtsearch.Text + "%')" +
                                  " AND (txtco_status = '0')" +
                                  " ORDER BY ID ASC";

                //  " AND (k004db_foods_order_1total.txtmat_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

                //   " AND (k011db_receipt.daily_status = '0')";

                //cmd1.Parameters.Add("@txtreceipt_date_start", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd1.Parameters.Add("@txtreceipt_date_end", SqlDbType.Date).Value = this.dtpend.Value;

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
                            var index = PANEL1_CO_dataGridView1_co.Rows.Add();
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_name"].Value = dt2.Rows[j]["txtco_name"].ToString();      //2
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_status"].Value = dt2.Rows[j]["txtco_status"].ToString();      //4
                        }
                        PANEL1_CO_GridView1_co_Up_Status();
                        //=======================================================
                    }
                    else
                    {
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

        }

        private void PANEL1_CO_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1_CO_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1_CO.Height = PANEL1_CO_btnresize_low.Top + e.Y;
                this.PANEL1_CO.Width = PANEL1_CO_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1_CO_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1_CO_btnnew_Click(object sender, EventArgs e)
        {
            W_ID_Select.FROM_FORM = "HOME";
            this.Hide();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_04_Co frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_04_Co();
            frm2.Show();
            frm2.BringToFront();
        }
        //Company=======================================================================
        //Branch=======================================================================
        private void PANEL2_BRANCH_Fill_branch()
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

            //===========================================

            PANEL2_BRANCH_Clear_GridView1_branch();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k008db_branch" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbranch_id <> '')" +
                                  " ORDER BY ID ASC";

                //  " AND (k004db_foods_order_1total.txtmat_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

                //   " AND (k011db_receipt.daily_status = '0')";

                //cmd1.Parameters.Add("@txtreceipt_date_start", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd1.Parameters.Add("@txtreceipt_date_end", SqlDbType.Date).Value = this.dtpend.Value;

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
                            var index = PANEL2_BRANCH_dataGridView1_branch.Rows.Add();
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //1
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_name"].Value = dt2.Rows[j]["txtbranch_name"].ToString();      //2
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_name_short"].Value = dt2.Rows[j]["txtbranch_name_short"].ToString();      //3
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_status"].Value = dt2.Rows[j]["txtbranch_status"].ToString();      //4
                        }
                        PANEL2_BRANCH_GridView1_branch_Up_Status();
                        //=======================================================
                    }
                    else
                    {
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

        }
        private void PANEL2_BRANCH_GridView1_branch()
        {
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnCount = 5;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].Name = "Col_Auto_num";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Name = "Col_txtbranch_id";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Name = "Col_txtbranch_name";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Name = "Col_txtbranch_name_short";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Name = "Col_txtbranch_status";

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].HeaderText = "No";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].HeaderText = "รหัสสาขา";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].HeaderText = "ชื่อสาขา";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].HeaderText = "ชื่อย่อสาขา";  //
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].HeaderText = "สถานะ";

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].Visible = false;  //"No";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Visible = true;  //"Col_txtbranch_id";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Width = 80;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Visible = true;  //"Col_txtbranch_name";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Width = 130;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Visible = true; // "Col_txtbranch_name_short
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Width = 100;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Visible = false;  // "Col_txtbranch_status
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Width = 0;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL2_BRANCH_dataGridView1_branch.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL2_BRANCH_dataGridView1_branch.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL2_BRANCH_dataGridView1_branch.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL2_BRANCH_dataGridView1_branch.Columns.Add(dgvCmb);

        }
        private void PANEL2_BRANCH_Clear_GridView1_branch()
        {
            this.PANEL2_BRANCH_dataGridView1_branch.Rows.Clear();
            this.PANEL2_BRANCH_dataGridView1_branch.Refresh();
        }
        private void PANEL2_BRANCH_GridView1_branch_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL2_BRANCH_dataGridView1_branch.Rows.Count ; i++)
            {
                if (this.PANEL2_BRANCH_dataGridView1_branch.Rows[i].Cells[4].Value.ToString() == "0")  //Active
                {
                    this.PANEL2_BRANCH_dataGridView1_branch.Rows[i].Cells[5].Value = true;
                }
                else
                {
                    this.PANEL2_BRANCH_dataGridView1_branch.Rows[i].Cells[5].Value = false;

                }
            }
        }
        private void PANEL2_BRANCH_btnbranch_Click(object sender, EventArgs e)
        {
            if (this.PANEL2_BRANCH.Visible == false)
            {
                this.PANEL2_BRANCH.Visible = true;
                this.PANEL2_BRANCH.Location = new Point(120, this.iblPoint.Location.Y + 2);

                PANEL2_BRANCH_GridView1_branch();
                PANEL2_BRANCH_Fill_branch();

            }
            else
            {
                this.PANEL2_BRANCH.Visible = false;
            }
        }
        private void PANEL2_BRANCH_btnclose_Click(object sender, EventArgs e)
        {
                if (this.PANEL2_BRANCH.Visible == false)
                {
                    this.PANEL2_BRANCH.Visible = true;
                }
                else
                {
                    this.PANEL2_BRANCH.Visible = false;
                }

        }
        private void PANEL2_BRANCH_dataGridView1_branch_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL2_BRANCH_dataGridView1_branch.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL2_BRANCH_txtbranch_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL2_BRANCH_txtbranch_name.Text = row.Cells[2].Value.ToString();
                    W_ID_Select.M_BRANCHID = row.Cells[1].Value.ToString();
                    W_ID_Select.M_BRANCHNAME = row.Cells[2].Value.ToString();
                    W_ID_Select.M_BRANCHNAME_SHORT = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL2_BRANCH_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL2_BRANCH_btn_search_Click(object sender, EventArgs e)
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

            //===========================================

            PANEL2_BRANCH_Clear_GridView1_branch();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k008db_branch" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtbranch_name LIKE '%" + this.PANEL2_BRANCH_txtsearch.Text + "%')" +
                                  " ORDER BY ID ASC";

                //  " AND (k004db_foods_order_1total.txtmat_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

                //   " AND (k011db_receipt.daily_status = '0')";

                //cmd1.Parameters.Add("@txtreceipt_date_start", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd1.Parameters.Add("@txtreceipt_date_end", SqlDbType.Date).Value = this.dtpend.Value;

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
                            var index = PANEL2_BRANCH_dataGridView1_branch.Rows.Add();
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //1
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_name"].Value = dt2.Rows[j]["txtbranch_name"].ToString();      //2
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_status"].Value = dt2.Rows[j]["txtbranch_status"].ToString();      //4
                        }
                        PANEL2_BRANCH_GridView1_branch_Up_Status();
                        //=======================================================
                    }
                    else
                    {
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

        }

        private void PANEL2_BRANCH_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL2_BRANCH_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL2_BRANCH.Height = PANEL2_BRANCH_btnresize_low.Top + e.Y;
                this.PANEL2_BRANCH.Width = PANEL2_BRANCH_btnresize_low.Left + e.X;
            }
        }
        private void PANEL2_BRANCH_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }
        private void PANEL2_BRANCH_btnnew_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO_txtco_id.Text == "")
            {
                MessageBox.Show("โปรดเลือก รหัสบริษัทฯ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1_CO.Visible = true;
                this.PANEL1_CO.Location = new Point(116, 62);
                this.PANEL2_BRANCH.Visible = false;
                return;
            }
            else
            {
                W_ID_Select.M_COID = this.PANEL1_CO_txtco_id.Text.Trim();
                W_ID_Select.M_CONAME = this.PANEL1_CO_txtco_name.Text.Trim();

                W_ID_Select.FROM_FORM = "HOME";
                this.Hide();
                kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_05_Branch frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_05_Branch();
                frm2.Show();
                frm2.BringToFront();
            }
        }

        //Branch=======================================================================


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
                    cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 1));

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

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string clearText_txtuser_name = this.txtCaption.Text.Trim();
            string cipherText_txtuser_name = W_CryptorEngine.Encrypt(clearText_txtuser_name, true);
            this.txtEncrypt.Text = cipherText_txtuser_name.Trim();
        }

        private void btnCaption_Click(object sender, EventArgs e)
        {
            string clearText_txtuser_name = this.txtEncrypt.Text.Trim();
            string cipherText_txtuser_name = W_CryptorEngine.Decrypt(clearText_txtuser_name, true);
            this.txtCaption.Text = cipherText_txtuser_name.Trim();

        }






        //Tans_Log ====================================================================






    }
}

