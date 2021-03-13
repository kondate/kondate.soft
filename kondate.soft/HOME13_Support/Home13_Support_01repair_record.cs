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
using System.Collections;

namespace kondate.soft.HOME13_Support
{
    public partial class Home13_Support_01repair_record : Form
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



        public Home13_Support_01repair_record()
        {
            InitializeComponent();
        }

        private void Home13_Support_01repair_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H1301SPRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "ออกใบแจ้งซ่อมฮาร์ดแวร์และซอฟท์แวร์";

            this.ActiveControl = this.txtproblem_detail;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);


            PANEL1316_DEPARTMENT_GridView1_department();
            PANEL1316_DEPARTMENT_Fill_department();

            PANEL1302_PROBLEM_GridView1_problem();
            PANEL1302_PROBLEM_Fill_problem();

            Load_FIRST_MEMO();

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

        private void panel_button_top_pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel1_contens_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
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


        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_NEW == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                W_ID_Select.LOG_ID = "3";
                W_ID_Select.LOG_NAME = "ใหม่";
                TRANS_LOG();

                W_ID_Select.WORD_TOP = "บันทึกใบแจ้งซ่อมฮาร์ดแวร์และซอฟท์แวร์";
                kondate.soft.HOME13_Support.Home13_Support_01repair_record frm2 = new kondate.soft.HOME13_Support.Home13_Support_01repair_record();
                frm2.Show();
                //this.Close();
            }

        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.PANEL1316_DEPARTMENT_txtdepartment_name.Text == "")
            {
                MessageBox.Show("โปรด เลือก ฝ่าย ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1316_DEPARTMENT_txtdepartment_name.Focus();
                return;
            }
            if (this.PANEL1302_PROBLEM_txtproblem_name.Text == "")
            {
                MessageBox.Show("โปรด เลือก ปัญหาที่ต้องการแจ้ง ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1302_PROBLEM_txtproblem_name.Focus();
                return;
            }
            if (this.txtproblem_detail.Text == "")
            {
                MessageBox.Show("โปรด รายละเอียดปัญหา/อาการเสีย ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtproblem_detail.Focus();
                return;
            }
            if (this.txtemp_noti.Text == "")
            {
                MessageBox.Show("โปรด ชื่อผู้แจ้ง ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtemp_noti.Focus();
                return;
            }
            if (this.txtemp_noti_apporve.Text == "")
            {
                MessageBox.Show("โปรด ชื่อผู้อนุมัติ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtemp_noti_apporve.Focus();
                return;
            }


            Load_FIRST_FIND_INSERT();
            AUTO_BILL_TRANS_ID();


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
                    String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                    DateTime myDateTime = new DateTime();
                    myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

                    String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                    DateTime myDateTime2 = new DateTime();
                    myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);
                    //MessageBox.Show("ok1");



                    //1 k020db_receive_record_trans
                    if (W_ID_Select.TRANS_BILL_STATUS.Trim() == "N")
                    {
                        cmd2.CommandText = "INSERT INTO h013support_01repair_1noti_record_trans(cdkey," +
                                           "txtco_id,txtbranch_id," +
                                           "txttrans_id)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                           "'" + this.txtnoti_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();


                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE h013support_01repair_1noti_record_trans SET txttrans_id = '" + this.txtnoti_id.Text.Trim() + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //MessageBox.Show("ok1");
                    cmd2.CommandText = "INSERT INTO h013support_01repair_1noti_record(cdkey,txtco_id,txtbranch_id," +  //1
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                            "txtuser_name,txtemp_office_name," +  //5
                                           "txtversion_id," +  //6
                                            //====================================================

                                           "txtnoti_id," +  //2
                                           "txtdepartment_id," +  //3
                                           "txtmachine_number," +  //4
                                           "txtproblem_id," +  //5
                                           "txtproblem_detail," +  //6
                                           "txtemp_noti," +  //7
                                           "txtemp_noti_apporve," +  //8
                                           "txtnotic_status," +  //9
                                           "txtapprove_notic_id," +  //10
                                           "txtrepair_status," +  //11
                                           "txtapprove_get_notic_id) " +  //12
                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtuser_name,@txtemp_office_name," +  //5
                                           "@txtversion_id," +  //6
                                            //=========================================================

                                           "@txtnoti_id," +  //2
                                           "@txtdepartment_id," +  //3
                                           "@txtmachine_number," +  //4
                                           "@txtproblem_id," +  //5
                                           "@txtproblem_detail," +  //6
                                           "@txtemp_noti," +  //7
                                           "@txtemp_noti_apporve," +  //8
                                          "@txtnotic_status," +  //9
                                           "@txtapprove_notic_id," +  //10
                                           "@txtrepair_status," +  //11
                                           "@txtapprove_get_notic_id)";   //12

                                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //1


                                        cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                                        cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                                        cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                                        cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                                        cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);
                                        cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);


                                        cmd2.Parameters.Add("@txtcomputer_ip", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_IP.Trim();
                                        cmd2.Parameters.Add("@txtcomputer_name", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim();
                                        cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = W_ID_Select.M_USERNAME.Trim();
                                        cmd2.Parameters.Add("@txtemp_office_name", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
                                        cmd2.Parameters.Add("@txtversion_id", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();  //7
                                         //==============================================================================


                                        cmd2.Parameters.Add("@txtnoti_id", SqlDbType.NVarChar).Value = this.txtnoti_id.Text.ToString();  //2
                                        cmd2.Parameters.Add("@txtdepartment_id", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.ToString();  //3
                                        cmd2.Parameters.Add("@txtmachine_number", SqlDbType.NVarChar).Value = this.txtmachine_number.Text.ToString();  //4
                                        cmd2.Parameters.Add("@txtproblem_id", SqlDbType.NVarChar).Value = this.PANEL1302_PROBLEM_txtproblem_id.Text.ToString();  //5
                                        cmd2.Parameters.Add("@txtproblem_detail", SqlDbType.NVarChar).Value = this.txtproblem_detail.Text.ToString();  //6
                                        cmd2.Parameters.Add("@txtemp_noti", SqlDbType.NVarChar).Value = this.txtemp_noti.Text.ToString();   //7
                                        cmd2.Parameters.Add("@txtemp_noti_apporve", SqlDbType.NVarChar).Value = this.txtemp_noti_apporve.Text.ToString();  //8

                                        cmd2.Parameters.Add("@txtnotic_status", SqlDbType.NChar).Value = "0";  //9
                                        cmd2.Parameters.Add("@txtapprove_notic_id", SqlDbType.NChar).Value = this.txtapprove_notic_id.Text.Trim();  //9
                                        cmd2.Parameters.Add("@txtrepair_status", SqlDbType.NChar).Value = "0";  //10
                                        cmd2.Parameters.Add("@txtapprove_get_notic_id", SqlDbType.NChar).Value = "-";  //9
                                        //==============================================================================

                    cmd2.ExecuteNonQuery();

                                        //MessageBox.Show("ok2");



                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = false;
                        this.btnPreview.Enabled = true;
                        this.BtnPrint.Enabled = true;
                        this.BtnClose_Form.Enabled = true;

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "บันทึกใบแจ้งซ่อมฮาร์ดแวร์และซอฟท์แวร์")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }


                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = true;
                        this.btnPreview.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnClose_Form.Enabled = true;

                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = true;
                        this.btnPreview.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnClose_Form.Enabled = true;

                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

        private void BtnCancel_Doc_Click(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtproblem_detail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtemp_noti.Focus();
        }

        private void txtemp_noti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtemp_noti.Focus();
        }

        private void txtemp_noti_apporve_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.BtnSave.Focus();
        }

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);
        }

        //txtdepartment ชื่อฝ่าย  =======================================================================
        private void PANEL1316_DEPARTMENT_Fill_department()
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

            PANEL1316_DEPARTMENT_Clear_GridView1_department();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_16department" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtdepartment_id <> '')" +
                               " ORDER BY ID ASC";

                //  " AND (k004db_foods_order_1total.txtsupplier_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

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
                            var index = PANEL1316_DEPARTMENT_dataGridView1_department.Rows.Add();
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //1
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //2
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_txtdepartment_name_eng"].Value = dt2.Rows[j]["txtdepartment_name_eng"].ToString();      //3
                        }
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
        private void PANEL1316_DEPARTMENT_GridView1_department()
        {
            this.PANEL1316_DEPARTMENT_dataGridView1_department.ColumnCount = 4;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[0].Name = "Col_Auto_num";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].Name = "Col_txtdepartment_id";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].Name = "Col_txtdepartment_name";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].Name = "Col_txtdepartment_name_eng";

            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[0].HeaderText = "No";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].HeaderText = "รหัส";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].HeaderText = " ชื่อฝ่าย ";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].HeaderText = " ชื่อฝ่าย  Eng";

            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[0].Visible = false;  //"No";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].Visible = true;  //"Col_txtdepartment_id";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].Width = 100;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].ReadOnly = true;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].Visible = true;  //"Col_txtdepartment_name";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].Width = 150;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].ReadOnly = true;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].Visible = true;  //"Col_txtdepartment_name_eng";
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].Width = 150;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].ReadOnly = true;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1316_DEPARTMENT_dataGridView1_department.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1316_DEPARTMENT_dataGridView1_department.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1316_DEPARTMENT_dataGridView1_department.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1316_DEPARTMENT_dataGridView1_department.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1316_DEPARTMENT_dataGridView1_department.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1316_DEPARTMENT_dataGridView1_department.EnableHeadersVisualStyles = false;

        }
        private void PANEL1316_DEPARTMENT_Clear_GridView1_department()
        {
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Rows.Clear();
            this.PANEL1316_DEPARTMENT_dataGridView1_department.Refresh();
        }
        private void PANEL1316_DEPARTMENT_txtdepartment_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1316_DEPARTMENT.Visible == false)
                {
                    this.PANEL1316_DEPARTMENT.Visible = true;
                    this.PANEL1316_DEPARTMENT.Location = new Point(this.PANEL1316_DEPARTMENT_txtdepartment_name.Location.X, this.PANEL1316_DEPARTMENT_txtdepartment_name.Location.Y + 22);
                    this.PANEL1316_DEPARTMENT_dataGridView1_department.Focus();
                }
                else
                {
                    this.PANEL1316_DEPARTMENT.Visible = false;
                }
        }
        private void PANEL1316_DEPARTMENT_btndepartment_Click(object sender, EventArgs e)
        {
            if (this.PANEL1316_DEPARTMENT.Visible == false)
            {
                this.PANEL1316_DEPARTMENT.Visible = true;
                this.PANEL1316_DEPARTMENT.BringToFront();
                this.PANEL1316_DEPARTMENT.Location = new Point(this.PANEL1316_DEPARTMENT_txtdepartment_name.Location.X, this.PANEL1316_DEPARTMENT_txtdepartment_name.Location.Y + 22);
            }
            else
            {
                this.PANEL1316_DEPARTMENT.Visible = false;
            }
        }
        private void PANEL1316_DEPARTMENT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1316_DEPARTMENT.Visible == false)
            {
                this.PANEL1316_DEPARTMENT.Visible = true;
            }
            else
            {
                this.PANEL1316_DEPARTMENT.Visible = false;
            }
        }
        private void PANEL1316_DEPARTMENT_dataGridView1_department_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1316_DEPARTMENT_dataGridView1_department.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL1316_DEPARTMENT_dataGridView1_department_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1316_DEPARTMENT_dataGridView1_department.CurrentRow.Index;

                this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = PANEL1316_DEPARTMENT_dataGridView1_department.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = PANEL1316_DEPARTMENT_dataGridView1_department.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1316_DEPARTMENT_txtdepartment_name.Focus();
                this.PANEL1316_DEPARTMENT.Visible = false;
            }
        }
        private void PANEL1316_DEPARTMENT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1316_DEPARTMENT_btn_search_Click(object sender, EventArgs e)
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

            PANEL1316_DEPARTMENT_Clear_GridView1_department();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_16department" +
                                    " WHERE (txtdepartment_name LIKE '%" + this.PANEL1316_DEPARTMENT_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " ORDER BY ID ASC";

                //  " AND (k004db_foods_order_1total.txtsupplier_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

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
                            var index = PANEL1316_DEPARTMENT_dataGridView1_department.Rows.Add();
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //1
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //2
                            PANEL1316_DEPARTMENT_dataGridView1_department.Rows[index].Cells["Col_txtdepartment_name_eng"].Value = dt2.Rows[j]["txtdepartment_name_eng"].ToString();      //3
                        }
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
        private void PANEL1316_DEPARTMENT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1316_DEPARTMENT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1316_DEPARTMENT.Height = PANEL1316_DEPARTMENT_btnresize_low.Top + e.Y;
                this.PANEL1316_DEPARTMENT.Width = PANEL1316_DEPARTMENT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1316_DEPARTMENT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1316_DEPARTMENT_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtdepartment ชื่อฝ่าย  =======================================================================






        //========================================================================

        //txtproblem ประเภท ปัญหา  =======================================================================
        private void PANEL1302_PROBLEM_Fill_problem()
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

            PANEL1302_PROBLEM_Clear_GridView1_problem();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM h013support_02_problem" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtproblem_id <> '')" +
                                    " ORDER BY txtproblem_no ASC";


                //  " AND (k004db_foods_order_1total.txtsupplier_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtproblem_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtproblem_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtproblem_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtproblem_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtproblem_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtproblem_status";

                            var index = PANEL1302_PROBLEM_dataGridView1_problem.Rows.Add();
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_no"].Value = dt2.Rows[j]["txtproblem_no"].ToString();      //1
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_id"].Value = dt2.Rows[j]["txtproblem_id"].ToString();      //2
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_name"].Value = dt2.Rows[j]["txtproblem_name"].ToString();      //3
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_name_eng"].Value = dt2.Rows[j]["txtproblem_name_eng"].ToString();      //4
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_remark"].Value = dt2.Rows[j]["txtproblem_remark"].ToString();      //5
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_status"].Value = dt2.Rows[j]["txtproblem_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL1302_PROBLEM_dataGridView1_problem_Up_Status();

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
        private void PANEL1302_PROBLEM_dataGridView1_problem_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL1302_PROBLEM_dataGridView1_problem.Rows.Count; i++)
            {
                if (this.PANEL1302_PROBLEM_dataGridView1_problem.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL1302_PROBLEM_dataGridView1_problem.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL1302_PROBLEM_dataGridView1_problem.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL1302_PROBLEM_GridView1_problem()
        {
            this.PANEL1302_PROBLEM_dataGridView1_problem.ColumnCount = 7;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[0].Name = "Col_Auto_num";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].Name = "Col_txtproblem_no";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].Name = "Col_txtproblem_id";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].Name = "Col_txtproblem_name";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].Name = "Col_txtproblem_name_eng";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].Name = "Col_txtproblem_remark";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[6].Name = "Col_txtproblem_status";

            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[0].HeaderText = "No";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].HeaderText = "ลำดับ";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].HeaderText = " รหัส";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].HeaderText = " ชื่อปัญหา";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].HeaderText = "ชื่อปัญหา Eng";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[6].HeaderText = " สถานะ";

            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[0].Visible = false;  //"No";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].Visible = true;  //"Col_txtproblem_no";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].Width = 90;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].ReadOnly = true;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].Visible = true;  //"Col_txtproblem_id";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].Width = 80;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].ReadOnly = true;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].Visible = true;  //"Col_txtproblem_name";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].Width = 150;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].ReadOnly = true;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].Visible = false;  //"Col_txtproblem_name_eng";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].Width = 0;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].ReadOnly = true;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].Visible = false;  //"Col_txtproblem_name_remark";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].Width = 0;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].ReadOnly = true;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[6].Visible = false;  //"Col_txtproblem_status";
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[6].Width = 0;
            this.PANEL1302_PROBLEM_dataGridView1_problem.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1302_PROBLEM_dataGridView1_problem.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1302_PROBLEM_dataGridView1_problem.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1302_PROBLEM_dataGridView1_problem.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1302_PROBLEM_dataGridView1_problem.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1302_PROBLEM_dataGridView1_problem.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1302_PROBLEM_dataGridView1_problem.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1302_PROBLEM_dataGridView1_problem.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL1302_PROBLEM_dataGridView1_problem.Columns.Add(dgvCmb);

        }
        private void PANEL1302_PROBLEM_Clear_GridView1_problem()
        {
            this.PANEL1302_PROBLEM_dataGridView1_problem.Rows.Clear();
            this.PANEL1302_PROBLEM_dataGridView1_problem.Refresh();
        }
        private void PANEL1302_PROBLEM_txtproblem_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1302_PROBLEM.Visible == false)
                {
                    this.PANEL1302_PROBLEM.Visible = true;
                    this.PANEL1302_PROBLEM.Location = new Point(this.PANEL1302_PROBLEM_txtproblem_name.Location.X, this.PANEL1302_PROBLEM_txtproblem_name.Location.Y + 22);
                    this.PANEL1302_PROBLEM_dataGridView1_problem.Focus();
                }
                else
                {
                    this.PANEL1302_PROBLEM.Visible = false;
                }
        }
        private void PANEL1302_PROBLEM_btnproblem_Click(object sender, EventArgs e)
        {
            if (this.PANEL1302_PROBLEM.Visible == false)
            {
                this.PANEL1302_PROBLEM.Visible = true;
                this.PANEL1302_PROBLEM.BringToFront();
                this.PANEL1302_PROBLEM.Location = new Point(this.PANEL1302_PROBLEM_txtproblem_name.Location.X, this.PANEL1302_PROBLEM_txtproblem_name.Location.Y + 22);
            }
            else
            {
                this.PANEL1302_PROBLEM.Visible = false;
            }
        }
        private void PANEL1302_PROBLEM_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1302_PROBLEM.Visible == false)
            {
                this.PANEL1302_PROBLEM.Visible = true;
            }
            else
            {
                this.PANEL1302_PROBLEM.Visible = false;
            }
        }
        private void PANEL1302_PROBLEM_dataGridView1_problem_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1302_PROBLEM_dataGridView1_problem.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1302_PROBLEM_txtproblem_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL1302_PROBLEM_txtproblem_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL1302_PROBLEM_dataGridView1_problem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1302_PROBLEM_dataGridView1_problem.CurrentRow.Index;

                this.PANEL1302_PROBLEM_txtproblem_id.Text = PANEL1302_PROBLEM_dataGridView1_problem.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1302_PROBLEM_txtproblem_name.Text = PANEL1302_PROBLEM_dataGridView1_problem.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1302_PROBLEM_txtproblem_name.Focus();
                this.PANEL1302_PROBLEM.Visible = false;
            }
        }
        private void PANEL1302_PROBLEM_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1302_PROBLEM_btn_search_Click(object sender, EventArgs e)
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

            PANEL1302_PROBLEM_Clear_GridView1_problem();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM h013support_02_problem" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtproblem_name LIKE '%" + this.PANEL1302_PROBLEM_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtproblem_no ASC";


                //  " AND (k004db_foods_order_1total.txtsupplier_id = '" + this.lvw_sale_detail.Items[j].SubItems[0].Text.ToString() + "')" +

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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtproblem_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtproblem_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtproblem_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtproblem_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtproblem_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtproblem_status";

                            var index = PANEL1302_PROBLEM_dataGridView1_problem.Rows.Add();
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_no"].Value = dt2.Rows[j]["txtproblem_no"].ToString();      //1
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_id"].Value = dt2.Rows[j]["txtproblem_id"].ToString();      //2
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_name"].Value = dt2.Rows[j]["txtproblem_name"].ToString();      //3
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_name_eng"].Value = dt2.Rows[j]["txtproblem_name_eng"].ToString();      //4
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_remark"].Value = dt2.Rows[j]["txtproblem_remark"].ToString();      //5
                            PANEL1302_PROBLEM_dataGridView1_problem.Rows[index].Cells["Col_txtproblem_status"].Value = dt2.Rows[j]["txtproblem_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL1302_PROBLEM_dataGridView1_problem_Up_Status();

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
        private void PANEL1302_PROBLEM_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1302_PROBLEM_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1302_PROBLEM.Height = PANEL1302_PROBLEM_btnresize_low.Top + e.Y;
                this.PANEL1302_PROBLEM.Width = PANEL1302_PROBLEM_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1302_PROBLEM_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1302_PROBLEM_btnnew_Click(object sender, EventArgs e)
        {

        }


        private void Load_FIRST_MEMO()
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
            Cursor.Current = Cursors.WaitCursor;

            //สต๊อคสินค้า ตามคลัง =============================================================================================
            string OK = "";
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                            " FROM h013support_01repair_1noti_record_for_load_first" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                             " AND (txtcomputer_name = '" + W_ID_Select.COMPUTER_NAME.Trim() + "')" +
                                           " ORDER BY txtdepartment_id ASC";
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
                            this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();
                            this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();
                            this.txtemp_noti.Text = dt2.Rows[0]["txtemp_noti"].ToString();
                            this.txtemp_noti_apporve.Text = dt2.Rows[0]["txtemp_noti_apporve"].ToString();
                        }
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {


                        Cursor.Current = Cursors.Default;
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("kondate.soft", ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }

        }
        private void Load_FIRST_FIND_INSERT()
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
            Cursor.Current = Cursors.WaitCursor;

            //=============================================================================================
            string OK = "";
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                            " FROM h013support_01repair_1noti_record_for_load_first" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                             " AND (txtcomputer_name = '" + W_ID_Select.COMPUTER_NAME.Trim() + "')" +
                                           " ORDER BY txtdepartment_id ASC";
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
                            //Col_mat_status
                            OK = "Y";
                        }
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        OK = "N";

                        Cursor.Current = Cursors.Default;
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("kondate.soft", ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }

            // =============================================================================================





            // INSERT 

            if (OK.Trim() != "Y")
            {
                //=======================================================
                Cursor.Current = Cursors.WaitCursor;
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

                        cmd2.CommandText = "INSERT INTO h013support_01repair_1noti_record_for_load_first(cdkey,txtco_id," +  //1
                       "txtbranch_id," +  //2
                       "txtcomputer_name," +  //3
                       "txtdepartment_id," +  //4
                       "txtdepartment_name," +  //5
                       "txtemp_noti," + //6
                       "txtemp_noti_apporve) " +  //7
                       "VALUES (@cdkey,@txtco_id," +  //1
                       "@txtbranch_id," +  //2
                       "@txtcomputer_name," +  //3
                       "@txtdepartment_id," +  //4
                       "@txtdepartment_name," +  //5
                       "@txtemp_noti," + //6
                       "@txtemp_noti_apporve)";   //7

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();  //1

                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //2
                        cmd2.Parameters.Add("@txtcomputer_name", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim();  //3
                        cmd2.Parameters.Add("@txtdepartment_id", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.ToString();  //4
                        cmd2.Parameters.Add("@txtdepartment_name", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_name.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtemp_noti", SqlDbType.NVarChar).Value = this.txtemp_noti.Text.ToString();  //6
                        cmd2.Parameters.Add("@txtemp_noti_apporve", SqlDbType.NVarChar).Value = this.txtemp_noti_apporve.Text.ToString();  //7

                        //==============================

                        cmd2.ExecuteNonQuery();


                        Cursor.Current = Cursors.WaitCursor;
                        trans.Commit();
                        conn.Close();

                        Cursor.Current = Cursors.Default;


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

            // END INSERT 

        }

        private void AUTO_BILL_TRANS_ID()
        {
            string TMP = "";
            string trans_Right = "";
            string trans_Right6 = "";
            double transNum = 0;
            string trans = "";
            string year2 = "";
            string year21 = "";
            string year_now = "";
            string year_now2 = "";
            string month_now = "";
            string day_now = "";


            year_now = DateTime.Now.ToString("yyyy", UsaCulture);
            year_now2 = year_now.Substring(year_now.Length - 2);

            month_now = DateTime.Now.ToString("MM", UsaCulture);
            day_now = DateTime.Now.ToString("dd", UsaCulture);

            //k006db_sale_record_trans
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
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !!  ", "Performance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //END เชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                  " FROM h013support_01repair_1noti_record_trans" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                  " ORDER BY txttrans_id";
                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "Y";

                        trans_Right = dt.Rows[0]["txttrans_id"].ToString();
                        trans_Right6 = trans_Right.Substring(trans_Right.Length - 6);

                        //211201-000001
                        year21 = trans_Right.Substring(trans_Right.Length - 13);
                        year2 = year21.Substring(0, 2);

                        transNum = Convert.ToDouble(string.Format("{0:n}", trans_Right6)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        trans = transNum.ToString("00000#");

                        if (year2.Trim() == year_now2.Trim())
                        {
                            TMP = "RN" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "RN" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "RN" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtnoti_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }


        //จบส่วนตารางสำหรับบันทึก========================================================================



        //Check ADD FORM========================================================================

        //END txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================


        //Check ADD FORM========================================================================
        private void CHECK_ADD_FORM()
        {
            //======================================================
            string CHOK = "";
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
            Cursor.Current = Cursors.WaitCursor;

            //END เชื่อมต่อฐานข้อมูล=======================================================
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT * FROM A001_sys_2form" +
                                   " WHERE (txtsys_depart_id = '" + W_ID_Select.M_DEPART_NUMBER.Trim() + "')" +
                                    " AND (txtsys_form_name = '" + this.Name.Trim() + "')";
                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    CHOK = "Y";
                }
                else
                {
                    CHOK = "N";
                }

            }

            //
            conn.Close();

            //จบเชื่อมต่อฐานข้อมูล=======================================================
            if (CHOK.Trim() == "N")
            {
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

                        cmd2.CommandText = "INSERT INTO A001_sys_2form(txtsys_depart_id," +  //1
                                               "txtsys_form_id," +  //2
                                               "txtsys_form_name," +  //3
                                               "txtsys_form_caption," +  //4
                                              "txtsys_form_status) " +  //5
                                               "VALUES (@txtsys_depart_id," +  //1
                                               "@txtsys_form_id," +  //2
                                               "@txtsys_form_name," +  //3
                                               "@txtsys_form_caption," +  //4
                                              "@txtsys_form_status)";   //5

                        cmd2.Parameters.Add("@txtsys_depart_id", SqlDbType.NVarChar).Value = W_ID_Select.M_DEPART_NUMBER.Trim();
                        cmd2.Parameters.Add("@txtsys_form_id", SqlDbType.NVarChar).Value = W_ID_Select.M_FORM_NUMBER.Trim();
                        cmd2.Parameters.Add("@txtsys_form_name", SqlDbType.NVarChar).Value = this.Name.Trim();
                        cmd2.Parameters.Add("@txtsys_form_caption", SqlDbType.NVarChar).Value = this.Text.ToString();
                        cmd2.Parameters.Add("@txtsys_form_status", SqlDbType.NVarChar).Value = "0";
                        //==============================
                        cmd2.ExecuteNonQuery();



                        trans.Commit();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                    }

                    catch (Exception ex)
                    {
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("kondate.soft", ex.Message);
                        return;
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;

                        conn.Close();
                    }
                }

            }
            //=============================================================

        }
        //END Check ADD FORM====================================================================
        //=====================================================================================

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
            Cursor.Current = Cursors.WaitCursor;
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
                          " FROM A003user_sys_2form" +
                          " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                           " AND (txtuser_name = '" + cipherText_txtuser_name.Trim() + "')" +
                           " AND (txtsys_depart_id = '" + W_ID_Select.M_DEPART_NUMBER.Trim() + "')" +
                           " AND (txtsys_form_id = '" + W_ID_Select.M_FORM_NUMBER.Trim() + "')" +
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
                            if (dt2.Rows[j]["txtallow_1grid_status"].ToString() == "Y")
                            {
                                W_ID_Select.M_FORM_GRID = "Y";
                            }
                            else
                            {
                                W_ID_Select.M_FORM_GRID = "N";
                                //this.GridView1.Visible = false;
                            }
                            //7
                            if (dt2.Rows[j]["txtallow_2new_status"].ToString() == "Y")
                            {
                                W_ID_Select.M_FORM_NEW = "Y";
                            }
                            else
                            {
                                W_ID_Select.M_FORM_NEW = "N";
                                this.BtnNew.Enabled = false;
                            }
                            //8
                            if (dt2.Rows[j]["txtallow_3open_status"].ToString() == "Y")
                            {
                                W_ID_Select.M_FORM_OPEN = "Y";
                            }
                            else
                            {
                                W_ID_Select.M_FORM_OPEN = "N";
                                this.btnopen.Enabled = false;
                            }
                            //9
                            if (dt2.Rows[j]["txtallow_4print_status"].ToString() == "Y")
                            {
                                W_ID_Select.M_FORM_PRINT = "Y";
                            }
                            else
                            {
                                W_ID_Select.M_FORM_PRINT = "N";
                                this.BtnPrint.Enabled = false;
                            }
                            //10
                            if (dt2.Rows[j]["txtallow_5cancel_status"].ToString() == "Y")
                            {
                                W_ID_Select.M_FORM_CANCEL = "Y";
                            }
                            else
                            {
                                W_ID_Select.M_FORM_CANCEL = "N";
                                this.BtnCancel_Doc.Enabled = false;
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        //=======================================================
                    }
                    else
                    {

                        W_ID_Select.M_FORM_GRID = "N";
                        W_ID_Select.M_FORM_NEW = "N";
                        W_ID_Select.M_FORM_OPEN = "N";
                        W_ID_Select.M_FORM_PRINT = "N";
                        W_ID_Select.M_FORM_CANCEL = "N";

                        //this.GridView1.Visible = false;
                        this.BtnNew.Enabled = false;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnCancel_Doc.Enabled = false;

                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        // return;
                    }

                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("kondate.soft", ex.Message);
                    return;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;

                    conn.Close();
                }

                //===========================================
            }
            //================================
            //================================
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.M_FORM_GRID = "Y";
                W_ID_Select.M_FORM_NEW = "Y";
                W_ID_Select.M_FORM_OPEN = "Y";
                W_ID_Select.M_FORM_PRINT = "Y";
                W_ID_Select.M_FORM_CANCEL = "Y";
                //this.GridView1.Visible = true;
                this.BtnNew.Enabled = true;
                this.btnopen.Enabled = true;
                this.BtnSave.Enabled = true;
                this.BtnPrint.Enabled = true;
                this.BtnCancel_Doc.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            else if (W_ID_Select.M_USERNAME_TYPE == "3")
            {
                W_ID_Select.M_FORM_GRID = "Y";
                W_ID_Select.M_FORM_NEW = "Y";
                W_ID_Select.M_FORM_OPEN = "Y";
                W_ID_Select.M_FORM_PRINT = "Y";
                W_ID_Select.M_FORM_CANCEL = "Y";
                //this.GridView1.Visible = true;
                this.BtnNew.Enabled = true;
                this.btnopen.Enabled = true;
                this.BtnSave.Enabled = true;
                this.BtnPrint.Enabled = true;
                this.BtnCancel_Doc.Enabled = true;
                Cursor.Current = Cursors.Default;
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





        //=============================================================

        //===============================================


        //====================================================================




        //=============================================================

        //===============================================


        //====================================================================

    }
}
