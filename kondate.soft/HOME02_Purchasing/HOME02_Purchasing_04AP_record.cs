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

namespace kondate.soft.HOME02_Purchasing
{
    public partial class HOME02_Purchasing_04AP_record : Form
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


        public HOME02_Purchasing_04AP_record()
        {
            InitializeComponent();
        }

        private void HOME02_Purchasing_04AP_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0204APRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "อนุมัติใบสั่งซื้อ";

            this.ActiveControl = this.txtpo_remark;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;



            //ส่วนของ ระเบียน PR =================================================================            
            Show_PANEL_PO_GridView1();
            Fill_Show_DATA_PANEL_PO_GridView1();


            this.PANEL_PO_dtpend.Value = DateTime.Now;
            this.PANEL_PO_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_PO_dtpend.CustomFormat = this.PANEL_PO_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.PANEL_PO_dtpstart.Value = DateTime.Today.AddDays(-7);
            this.PANEL_PO_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_PO_dtpstart.CustomFormat = this.PANEL_PO_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

            //========================================
            this.PANEL_PO_cboSearch.Items.Add("เลขที่ PO");
            this.PANEL_PO_cboSearch.Items.Add("ชื่อผู้บันทึก PO");
            //ส่วนของ ระเบียน PR =================================================================

            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            Show_GridView1();
            this.txtemp_office_name_approve.Text = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
            //1.ส่วนหน้าหลัก======================================================================



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

        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_NEW == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            W_ID_Select.LOG_ID = "3";
            W_ID_Select.LOG_NAME = "ใหม่";
            TRANS_LOG();

            this.Hide();
            var frm2 = new HOME02_Purchasing.HOME02_Purchasing_04AP_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "อนุมัติใบสั่งซื้อ";
            this.txtPo_id.ReadOnly = true;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (this.txtPo_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก เลขที่ใบสั่งซื้อ PO ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPo_id.Focus();
                return;
            }
            if (this.PANEL161_SUP_txtsupplier_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก Supplier ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL161_SUP_txtsupplier_id.Focus();
                return;
            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกกลุ่มภาษี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Focus();
                return;
            }

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells[9].Value == null)
                {
                    MessageBox.Show("โปรด ใส่วันที่สินค้าเข้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (this.txtapprove_status_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก ผลพิจารณาอนุมัติ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.ch_approve_y.Checked == true)
            {
                this.txtapprove_status_id.Text = "Y";
                this.ch_approve_r.Checked = false;
                this.ch_approve_n.Checked = false;
                this.txtpo_remark.Text = this.ch_approve_y.Text.ToString() + " " + this.txtpo_remark.Text.ToString();
            }
            if (this.ch_approve_r.Checked == true)
            {
                this.txtapprove_status_id.Text = "R";
                this.ch_approve_y.Checked = false;
                this.ch_approve_n.Checked = false;
                this.txtpo_remark.Text = this.ch_approve_r.Text.ToString() + " " + this.txtpo_remark.Text.ToString();
            }
            if (this.ch_approve_n.Checked == true)
            {
                this.txtapprove_status_id.Text = "N";
                this.ch_approve_y.Checked = false;
                this.ch_approve_r.Checked = false;
                this.txtpo_remark.Text = this.ch_approve_n.Text.ToString() + " " + this.txtpo_remark.Text.ToString();
            }


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

            AUTO_BILL_TRANS_ID();
            GridView1_Cal_Sum();
            Sum_group_tax();

            //END เชื่อมต่อฐานข้อมูล=======================================================
            //string PR_STATUS = "";
            //conn.Open();
            //if (conn.State == System.Data.ConnectionState.Open)
            //{

            //    SqlCommand cmd1 = conn.CreateCommand();
            //    cmd1.CommandType = CommandType.Text;
            //    cmd1.Connection = conn;

            //    cmd1.CommandText = "SELECT * FROM k017db_pr_all" +
            //                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
            //                        " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
            //                        " AND (txtPo_id = '" + this.txtPo_id.Text.Trim() + "')";
            //    cmd1.ExecuteNonQuery();
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd1);
            //    da.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //    {
            //        Cursor.Current = Cursors.Default;
            //        PR_STATUS = "Y";
            //        conn.Close();
            //    }
            //    else
            //    {
            //        Cursor.Current = Cursors.Default;
            //        PR_STATUS = "N";
            //        conn.Close();

            //    }

            //    //
            //    conn.Close();
            //}

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

                    if (this.iblword_status.Text.Trim() == "อนุมัติใบสั่งซื้อ")
                    {

                        //1 k019db_approve_record_trans
                        if (W_ID_Select.TRANS_BILL_STATUS.Trim() == "N")
                        {
                            cmd2.CommandText = "INSERT INTO k019db_approve_record_trans(cdkey," +
                                               "txtco_id,txtbranch_id," +
                                               "txttrans_id)" +
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                               "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                               "'" + this.txtApprove_id.Text.Trim() + "')";

                            cmd2.ExecuteNonQuery();


                        }
                        else
                        {
                            cmd2.CommandText = "UPDATE k019db_approve_record_trans SET txttrans_id = '" + this.txtApprove_id.Text.Trim() + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                            cmd2.ExecuteNonQuery();

                        }

                        //2 k018db_po_record
                        cmd2.CommandText = "INSERT INTO k019db_approve_record(cdkey,txtco_id,txtbranch_id," +  //1
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                                "txtuser_name,txtemp_office_name," +  //5
                                               "txtversion_id," +  //6
                                                                   //====================================================

                                               "txtapprove_id," + // 10
                                               "txtPo_id," + // 7
                                               "txtPr_id," + // 8
                                               "txtPr_date," + // 9
                                               "txtRG_id," + // 12
                                               "txtRG_date," + // 13
                                               "txtreceive_id," + // 14
                                               "txtreceive_date," + // 15

                                               "txtsupplier_id," + // 16
                                               "txtcontact_person," + // 17
                                               "txtwant_mat_in_day," + // 18
                                               "txtdate_send_mat," + // 19
                                               "txtcredit_in_day," + // 20
                                               "txtpo_remark," + // 21

                                               "txtemp_office_name_manager," + //22
                                               "txtemp_office_name_approve," + // 23
                                               "txtapprove_status_id," + // 24
                                               "txtproject_id," + // 25
                                               "txtjob_id," + // 26
                                               "txtjob_send_mat_status," + // 27
                                               "txtcurrency_id," + // 28
                                               "txtcurrency_date," + // 29
                                               "txtcurrency_rate," + // 30

                                               "txtacc_group_tax_id," + // 31

                                               "txtsum_qty," + // 32
                                               "txtsum_price," + // 33
                                               "txtsum_discount," + // 34
                                               "txtmoney_sum," + // 35
                                               "txtmoney_tax_base," + // 36
                                               "txtvat_rate," + // 37
                                               "txtvat_money," + // 38
                                               "txtmoney_after_vat," + // 39

                                               "txtpr_status," + // 40
                                               "txtpo_status," +  //41
                                              "txtapprove_status," +  //42
                                              "txtRG_status," +  //43
                                              "txtreceive_status," +  //44
                                              "txtemp_print,txtemp_print_datetime) " +  //45
                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtuser_name,@txtemp_office_name," +  //5
                                               "@txtversion_id," +  //6
                                                                    //=========================================================


                                               "@txtapprove_id," + // 10
                                               "@txtPo_id," + // 7
                                               "@txtPr_id," + // 8
                                               "@txtPr_date," + // 9
                                               "@txtRG_id," + // 12
                                               "@txtRG_date," + // 13
                                               "@txtreceive_id," + // 14
                                               "@txtreceive_date," + // 15

                                               "@txtsupplier_id," + // 16
                                               "@txtcontact_person," + // 17
                                               "@txtwant_mat_in_day," + // 18
                                               "@txtdate_send_mat," + // 19
                                               "@txtcredit_in_day," + // 20
                                               "@txtpo_remark," + // 21

                                               "@txtemp_office_name_manager," + //22
                                               "@txtemp_office_name_approve," + // 23
                                               "@txtapprove_status_id," + // 24
                                               "@txtproject_id," + // 25
                                               "@txtjob_id," + // 26
                                               "@txtjob_send_mat_status," + // 27
                                               "@txtcurrency_id," + // 28
                                               "@txtcurrency_date," + // 29
                                               "@txtcurrency_rate," + // 30

                                               "@txtacc_group_tax_id," + // 31

                                               "@txtsum_qty," + // 32
                                               "@txtsum_price," + // 33
                                               "@txtsum_discount," + // 34
                                               "@txtmoney_sum," + // 35
                                               "@txtmoney_tax_base," + // 36
                                               "@txtvat_rate," + // 37
                                               "@txtvat_money," + // 38
                                               "@txtmoney_after_vat," + // 39

                                               "@txtpr_status," + // 40
                                               "@txtpo_status," +  //41
                                              "@txtapprove_status," +  //42
                                              "@txtRG_status," +  //43
                                              "@txtreceive_status," +  //44
                                              "@txtemp_print,@txtemp_print_datetime)";   //45

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

                        cmd2.Parameters.Add("@txtapprove_id", SqlDbType.NVarChar).Value = this.txtApprove_id.Text.Trim();  //7
                        cmd2.Parameters.Add("@txtPo_id", SqlDbType.NVarChar).Value = this.txtPo_id.Text.Trim();  //7
                        cmd2.Parameters.Add("@txtPr_id", SqlDbType.NVarChar).Value = this.txtPr_id.Text.Trim();  //8
                        cmd2.Parameters.Add("@txtPr_date", SqlDbType.NVarChar).Value = "";  //9
                        cmd2.Parameters.Add("@txtRG_id", SqlDbType.NVarChar).Value = "";  //12
                        cmd2.Parameters.Add("@txtRG_date", SqlDbType.NVarChar).Value = "";  //13
                        cmd2.Parameters.Add("@txtreceive_id", SqlDbType.NVarChar).Value = "";  //14
                        cmd2.Parameters.Add("@txtreceive_date", SqlDbType.NVarChar).Value = "";  //15


                        cmd2.Parameters.Add("@txtsupplier_id", SqlDbType.NVarChar).Value = this.PANEL161_SUP_txtsupplier_id.Text.Trim();  //16
                        cmd2.Parameters.Add("@txtcontact_person", SqlDbType.NVarChar).Value = this.txtcontact_person.Text.Trim();  //17
                        cmd2.Parameters.Add("@txtwant_mat_in_day", SqlDbType.NVarChar).Value = this.txtwant_mat_in_day.Text.Trim();  //18

                        //DateTime date_send_mat = Convert.ToDateTime(this.txtdate_send_mat.Text.ToString());
                        //string d_send_mat = date_send_mat.ToString("yyyy-MM-dd");
                        cmd2.Parameters.Add("@txtdate_send_mat", SqlDbType.NVarChar).Value = this.txtdate_send_mat.Text.ToString();  //19

                        cmd2.Parameters.Add("@txtcredit_in_day", SqlDbType.NVarChar).Value = this.txtcredit_in_day.Text.Trim();  //20
                        cmd2.Parameters.Add("@txtpo_remark", SqlDbType.NVarChar).Value = this.txtpo_remark.Text.Trim();  //21

                        cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.ToString();  //22
                        cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.ToString();  //23

                        cmd2.Parameters.Add("@txtapprove_status_id", SqlDbType.NVarChar).Value = this.txtapprove_status_id.Text.ToString();  //24

                        cmd2.Parameters.Add("@txtproject_id", SqlDbType.NVarChar).Value = this.PANEL1307_PROJECT_txtproject_id.Text.ToString();  //25
                        cmd2.Parameters.Add("@txtjob_id", SqlDbType.NVarChar).Value = this.PANEL1317_JOB_txtjob_id.Text.ToString();  //26

                        if (this.checkBox1_txtjob_send_mat_status.Checked == true)
                        {
                            cmd2.Parameters.Add("@txtjob_send_mat_status", SqlDbType.NVarChar).Value = "Y";  //27

                        }
                        else
                        {
                            cmd2.Parameters.Add("@txtjob_send_mat_status", SqlDbType.NVarChar).Value = "N";  //27

                        }

                        cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //28
                        cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //29
                        cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtcurrency_rate.Text.ToString()));  //30


                        cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //31

                        cmd2.Parameters.Add("@txtsum_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //32
                        cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //33
                        cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //34
                        cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //35
                        cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //36
                        cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //37
                        cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //38
                        cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //39

                        cmd2.Parameters.Add("@txtpr_status", SqlDbType.NVarChar).Value = "0";  //40
                        cmd2.Parameters.Add("@txtpo_status", SqlDbType.NVarChar).Value = "0";  //41
                        cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = this.txtapprove_status_id.Text.Trim();  //42
                        cmd2.Parameters.Add("@txtRG_status", SqlDbType.NVarChar).Value = "";  //43
                        cmd2.Parameters.Add("@txtreceive_status", SqlDbType.NVarChar).Value = "";  //44
                        cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //45
                        cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);  //46

                        //==============================
                        cmd2.ExecuteNonQuery();




                        //}


                        int s = 0;
                        //command.Parameters.AddWithValue("@Name", txtName.Text);
                        //command.Parameters.AddWithValue("@BirthDate", birthday);

                        for (int i = 0; i < this.GridView1.Rows.Count; i++)
                        {
                            s = i + 1;
                            if (this.GridView1.Rows[i].Cells[2].Value != null)
                            {
                                this.GridView1.Rows[i].Cells[0].Value = s.ToString();
                                if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[5].Value.ToString())) > 0)
                                {
                                    //3 k018db_po_record_detail

                                    DateTime want_receive_date = Convert.ToDateTime(this.GridView1.Rows[i].Cells[9].Value.ToString());
                                    string want_date = want_receive_date.ToString("yyyy-MM-dd");
                                    //string OD_date = DateTime.ParseExact(this.GridView1.Rows[i].Cells[9].Value, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
                                    //cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                                    //cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                                    //cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);

                                    cmd2.CommandText = "INSERT INTO k019db_approve_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                       "txttrans_year,txttrans_month,txttrans_day," +
                                       "txtApprove_id," +  //2
                                       "txtPo_id," +  //2
                                       "txtPr_id," +  //2
                                       "txtmat_no," +  //3
                                       "txtmat_id," +  //4
                                       "txtmat_name," +  //5
                                       "txtmat_unit1_name," +  //6
                                       "txtqty_want," +  //7
                                       "txtqty," +  //7
                                       "txtqty_balance," +  //8
                                       "txtprice," +   //9
                                       "txtdiscount_rate," +  //10
                                       "txtdiscount_money," +  //11
                                       "txtsum_total," +  //12
                                       "txtwant_receive_date," +  //13
                                       "txtitem_no,txtmat_po_remark) " +  //14

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +
                                "'" + this.txtApprove_id.Text.Trim() + "'," +  //2
                                "'" + this.txtPo_id.Text.Trim() + "'," +  //2
                                "'" + this.txtPr_id.Text.Trim() + "'," +  //2
                                "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //3
                                "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //4
                                "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +    //5
                                "'" + this.GridView1.Rows[i].Cells[4].Value.ToString() + "'," +  //6
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[5].Value.ToString())) + "'," +  //7
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //7
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //8
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[6].Value.ToString())) + "'," +  //9
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //10
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[7].Value.ToString())) + "'," +  //11
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[8].Value.ToString())) + "'," +  //12
                               "'" + want_date + "'," +  //13
                               "'" + this.GridView1.Rows[i].Cells[0].Value.ToString() + "','')";   //14

                                    cmd2.ExecuteNonQuery();

                                    //4

                                    //===================================================================================================================
                                    //===================================================================================================================
                                    //}
                                    //else
                                    //{


                                    //}
                                    //========================================================
                                    //5 k017db_pr_all_detail_balance ==============================================================================================


                                    //====================================================================================================
                                }
                            }
                        }

                    }



                    //6
                    cmd2.CommandText = "UPDATE k017db_pr_record SET txtapprove_id = '" + this.txtApprove_id.Text.Trim() + "'," +
                                       "txtapprove_date = '" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "'," +
                                       "txtapprove_status = '" + this.txtapprove_status_id.Text.Trim() + "'" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtPr_id = '" + this.txtPr_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();

                    //7
                    cmd2.CommandText = "UPDATE k018db_po_record SET txtapprove_id = '" + this.txtApprove_id.Text.Trim() + "'," +
                                       "txtapprove_date = '" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "'," +
                                       "txtapprove_status = '" + this.txtapprove_status_id.Text.Trim() + "'," +
                                         "txtemp_office_name_approve = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtPo_id = '" + this.txtPo_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();



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

                        if (this.iblword_status.Text.Trim() == "อนุมัติใบสั่งซื้อ")
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

            ////จบเชื่อมต่อฐานข้อมูล=======================================================
            //conn.Open();
            //if (conn.State == System.Data.ConnectionState.Open)
            //{

            //    SqlCommand cmd1 = conn.CreateCommand();
            //    cmd1.CommandType = CommandType.Text;
            //    cmd1.Connection = conn;

            //    cmd1.CommandText = "SELECT * FROM k019db_approve_record" +
            //                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
            //                        " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
            //                        " AND (txtPo_id = '" + this.txtPo_id.Text.Trim() + "')" +
            //                        " AND (txtapprove_status <> 'Y')";

            //    cmd1.ExecuteNonQuery();
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd1);
            //    da.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //    {
            //        Cursor.Current = Cursors.Default;

            //        MessageBox.Show("เอกสารนี้   : '" + this.txtPo_id.Text.Trim() + "' ยังไม่อนุมัติ   ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        conn.Close();
            //        return;
            //    }
            //}

            ////
            //conn.Close();

            ////จบเชื่อมต่อฐานข้อมูล=======================================================

            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            W_ID_Select.TRANS_ID = this.txtApprove_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_04AP_record_Print frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_04AP_record_Print();
            frm2.Show();
            frm2.BringToFront();

            //======================================================

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            W_ID_Select.TRANS_ID = this.txtPo_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            //Print Role=========================================
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {

                TableLogOnInfo cr_table_logon_info = new TableLogOnInfo();
                ConnectionInfo cr_Connection_Info = new ConnectionInfo();
                Tables CrTables;

                ReportDocument rpt = new ReportDocument();

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_Chart_of_accounts.rpt");
                //E:\01_Project_ERP_Kondate.Soft\kondate.soft\kondate.soft\bin\Debug\KONDATE_REPORT
                //E:\01_Project_ERP_Kondate.Soft\kondate.soft\kondate.soft\KONDATE_REPORT\Report_Chart_of_accounts.rpt
                //C:\KD_ERP\KD_REPORT
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_k019db_approve_record.rpt");


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
                rpt.SetParameterValue("txtApprove_id", W_ID_Select.TRANS_ID.Trim());

                //พิมพ์กับเครื่องที่เราต้องการ ระบุชื่อไปเลย=============================================
                //rpt.PrintOptions.PrinterName = "EPSON TM-T88V Receipt5";
                //rpt.PrintToPrinter(1, false, 0, 0);


                //พิมพ์ออกที่เครื่องพิมพ์ที่เลือกไว้ ในเครื่อง==============================================
                //rpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                //rpt.PrintOptions.PaperSize = PaperSize.PaperA4;
                //rpt.PrintToPrinter(1, false, 0, 15);


                //พิมพ์เป็น ไดอะล็อค เพื่อ save เป็น file อื่นๆ ที่ต้องการอีกที ==============================================
                rpt.PrintOptions.PrinterName = printDialog.PrinterSettings.PrinterName;
                rpt.PrintToPrinter(printDialog.PrinterSettings.Copies, printDialog.PrinterSettings.Collate, printDialog.PrinterSettings.FromPage, printDialog.PrinterSettings.ToPage);

            }
            //=============================================================================================


        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void btnremove_row_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการ ลบรายการแถว ที่คลิ๊ก ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                //DataGridViewRow row = new DataGridViewRow();
                //row = this.PANEL161_SUP_dataGridView2.Rows[selectedRowIndex];
                this.GridView1.Rows.RemoveAt(selectedRowIndex);
                GridView1_Cal_Sum();
                Sum_group_tax();

                Cursor.Current = Cursors.Default;

                //MessageBox.Show("ลบ เรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
                Cursor.Current = Cursors.Default;

                //MessageBox.Show("ยังไม่ได้ ลบ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                Cursor.Current = Cursors.Default;

                //MessageBox.Show("ไม่ได้ ลบ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //1.ส่วนหน้าหลัก ตารางสำหรับบันทึก========================================================================
        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle;
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow = 0;
 
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 10;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtmat_no";
            this.GridView1.Columns[2].Name = "Col_txtmat_id";
            this.GridView1.Columns[3].Name = "Col_txtmat_name";
            this.GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[5].Name = "Col_txtqty";
            this.GridView1.Columns[6].Name = "Col_txtprice";
            this.GridView1.Columns[7].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[8].Name = "Col_txtsum_total";
            this.GridView1.Columns[9].Name = "Col_date";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "ลำดับ";
            this.GridView1.Columns[2].HeaderText = " รหัส";
            this.GridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.GridView1.Columns[4].HeaderText = " หน่วยนับ";
            this.GridView1.Columns[5].HeaderText = " จำนวน";
            this.GridView1.Columns[6].HeaderText = " ราคา/หน่วย(บาท)";
            this.GridView1.Columns[7].HeaderText = " ส่วนลด(บาท)";
            this.GridView1.Columns[8].HeaderText = " จำนวนเงิน(บาท)";
            this.GridView1.Columns[9].HeaderText = " วันที่สินค้าเข้า";

            this.GridView1.Columns[0].Visible = true;  //"Col_Auto_num";
            this.GridView1.Columns[0].Width = 36;
            this.GridView1.Columns[0].ReadOnly = true;
            this.GridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[1].Visible = true;  //"Col_txtmat_no";

            this.GridView1.Columns[2].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns[2].Width = 100;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[3].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns[3].Width = 150;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[4].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView1.Columns[4].Width = 100;
            this.GridView1.Columns[4].ReadOnly = true;
            this.GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[5].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns[5].Width = 100;
            this.GridView1.Columns[5].ReadOnly = true;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[6].Visible = true;  //"Col_txtprice";
            this.GridView1.Columns[6].Width = 100;
            this.GridView1.Columns[6].ReadOnly = true;
            this.GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[7].Visible = true;  //"Col_txtdiscount_money";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = true;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[8].Visible = true;  //"Col_txtsum_total";
            this.GridView1.Columns[8].Width = 150;
            this.GridView1.Columns[8].ReadOnly = true;
            this.GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[9].Visible = true;  //"Col_date";
            this.GridView1.Columns[9].Width = 150;
            this.GridView1.Columns[9].ReadOnly = true;
            this.GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView1()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
        }
        private void GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = GridView1.CurrentRow.Index;
        //    this.btnremove_row.Visible = true;

            switch (GridView1.Columns[e.ColumnIndex].Name)
            {
                case "Col_txtmat_no":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_id":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_name":
                    dtp.Visible = false;
                    break;
                case "Col_txtqty":
                    dtp.Visible = false;
                    break;
                case "Col_txtprice":
                    dtp.Visible = false;
                    break;
                case "Col_txtdiscount_money":
                    dtp.Visible = false;
                    break;
                case "Col_txtsum_total":
                    dtp.Visible = false;
                    break;
                case "Col_date":
                    dtp.Visible = false;
                    break;
            }
        }
        private void GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow = GridView1.CurrentRow.Index;
            int rowscount = GridView1.Rows.Count;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();

        }
        private void GridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtp.Visible = false;
        }
        private void GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            dtp.Visible = false;
        }
        private void GridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt = e.Control as TextBox;
            txt.PreviewKeyDown += new PreviewKeyDownEventHandler(txt_PreviewKeyDown);
        }
        private void GridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GridView1_Cal_Sum();
                Sum_group_tax();

            }
        }
        private void GridView1_KeyUp(object sender, KeyEventArgs e)
        {
            GridView1_Cal_Sum();
            Sum_group_tax();

        }
        private void dtp_TextChange(object sender, EventArgs e)
        {
            GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
        }
        void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                GridView1_Cal_Sum();
                Sum_group_tax();

            }
        }
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[5].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
                //}
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[9].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
                //}

                GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightSkyBlue;

            }
        }
        private void GridView1_Cal_Sum()
        {
            double Sum_Total = 0;
            double Sum_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells[2].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells[0].Value == null)
                    {
                        this.GridView1.Rows[i].Cells[0].Value = k.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells[5].Value == null)
                    {
                        this.GridView1.Rows[i].Cells[5].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells[6].Value == null)
                    {
                        this.GridView1.Rows[i].Cells[6].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells[7].Value == null)
                    {
                        this.GridView1.Rows[i].Cells[7].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells[8].Value == null)
                    {
                        this.GridView1.Rows[i].Cells[8].Value = "0";
                    }


                    //5 * 6 = 8

                    this.GridView1.Rows[i].Cells[5].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells[5].Value).ToString("###,###.00");     //5
                    this.GridView1.Rows[i].Cells[6].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells[6].Value).ToString("###,###.00");     //6
                    this.GridView1.Rows[i].Cells[7].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells[7].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells[8].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells[8].Value).ToString("###,###.00");     //8


                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[5].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[6].Value.ToString()));
                    this.GridView1.Rows[i].Cells[8].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[5].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[5].Value.ToString()));
                        this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[6].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[7].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[8].Value.ToString()));
                        this.txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }
                }
            }

            this.txtcount_rows.Text = k.ToString();

            Sum_Total = 0;
            Sum_Qty = 0;
            Sum_Price = 0;
            Sum_Discount = 0;
            MoneySum = 0;

        }
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }
        private void GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void Sum_group_tax()
        {
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_EX")  //ซื้อคิดvatแยก
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_IN") //ซื้อคิดvatรวม
            {
                double DisCount = 0;
                double VATMONey = 0;
                double VATBASE = 0;
                double VATA = 0;

                //รวมทั้งสิ้น
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_after_vat.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                VATA = Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) + 100;

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_after_vat.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / Convert.ToDouble(string.Format("{0:n}", VATA));
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //ฐานภาษี
                VATBASE = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_after_vat.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_tax_base.Text = VATBASE.ToString("N", new CultureInfo("en-US"));


            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_NOvat")  //ซื้อไม่มีvat
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                this.txtvat_rate.Text = "0";

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));


            }
        }


        private void btnPo_id_Click(object sender, EventArgs e)
        {
            if (this.PANEL_PO.Visible == false)
            {
                this.PANEL_PO.Visible = true;
                this.PANEL_PO.BringToFront();
                this.PANEL_PO.Location = new Point(this.txtPo_id.Location.X, this.txtPo_id.Location.Y + 22);
                this.PANEL_PO_iblword_top.Text = "ระเบียนใบสั่งซื้อ PO";
                SHOW_btnGo3();
            }
            else
            {
                this.PANEL_PO.Visible = false;
            }
        }
        private void btnGo1_Click(object sender, EventArgs e)
        {
            SHOW_PO();
        }
        private void SHOW_PO()
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
            Clear_GridView1();
            //PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT k018db_po_record.*," +
                                   "k018db_po_record_detail.*," +
                                   //"k017db_pr_record.*," +
                                   "k013_1db_acc_16department.*," +
                                   "k013_1db_acc_07project.*," +
                                   "k013_1db_acc_17job.*," +
                                   "k016db_1supplier.*," +
                                   "k013_1db_acc_13group_tax.*" +

                                   " FROM k018db_po_record" +

                                   " INNER JOIN k018db_po_record_detail" +
                                   " ON k018db_po_record.cdkey = k018db_po_record_detail.cdkey" +
                                   " AND k018db_po_record.txtco_id = k018db_po_record_detail.txtco_id" +
                                   " AND k018db_po_record.txtPr_id = k018db_po_record_detail.txtPr_id" +

                                   //" INNER JOIN k017db_pr_record" +
                                   //" ON k018db_po_record.cdkey = k017db_pr_record.cdkey" +
                                   //" AND k018db_po_record.txtco_id = k017db_pr_record.txtco_id" +
                                   //" AND k018db_po_record.txtPr_id = k017db_pr_record.txtPr_id" +


                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k018db_po_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k018db_po_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k018db_po_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " INNER JOIN k013_1db_acc_07project" +
                                   " ON k018db_po_record.cdkey = k013_1db_acc_07project.cdkey" +
                                   " AND k018db_po_record.txtco_id = k013_1db_acc_07project.txtco_id" +
                                   " AND k018db_po_record.txtproject_id = k013_1db_acc_07project.txtproject_id" +

                                   " INNER JOIN k013_1db_acc_17job" +
                                   " ON k018db_po_record.cdkey = k013_1db_acc_17job.cdkey" +
                                   " AND k018db_po_record.txtco_id = k013_1db_acc_17job.txtco_id" +
                                   " AND k018db_po_record.txtjob_id = k013_1db_acc_17job.txtjob_id" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON k018db_po_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND k018db_po_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k018db_po_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON k018db_po_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " WHERE (k018db_po_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k018db_po_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k018db_po_record.txtPo_id = '" + this.txtPo_id.Text.Trim() + "')" +
                                  " ORDER BY k018db_po_record.txtPr_id ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.txtPo_id.Text = dt2.Rows[0]["txtPo_id"].ToString();
                        this.txtPr_id.Text = dt2.Rows[0]["txtPr_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                        this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_server"].ToString());
                        this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                        this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

                        this.txtcontact_person.Text = dt2.Rows[0]["txtcontact_person"].ToString();
                        this.txtpo_remark.Text = dt2.Rows[0]["txtpo_remark"].ToString();

                        this.txtwant_mat_in_day.Text = dt2.Rows[0]["txtwant_mat_in_day"].ToString();
                        this.txtdate_send_mat.Text = dt2.Rows[0]["txtdate_send_mat"].ToString();
                        this.txtcredit_in_day.Text = dt2.Rows[0]["txtcredit_in_day"].ToString();

                        this.PANEL1307_PROJECT_txtproject_id.Text = dt2.Rows[0]["txtproject_id"].ToString();
                        this.PANEL1317_JOB_txtjob_id.Text = dt2.Rows[0]["txtjob_id"].ToString();

                        if (dt2.Rows[0]["txtjob_send_mat_status"].ToString() == "Y")
                        {
                            this.checkBox1_txtjob_send_mat_status.Checked = true;
                        }
                        else
                        {
                            this.checkBox1_txtjob_send_mat_status.Checked = false;
                        }

                        this.Paneldate_txtcurrency_date.Text = dt2.Rows[0]["txtcurrency_date"].ToString();
                        this.txtcurrency_id.Text = dt2.Rows[0]["txtcurrency_id"].ToString();
                        this.txtcurrency_rate.Text = dt2.Rows[0]["txtcurrency_rate"].ToString();

                        //this.txtemp_office_name.Text = dt2.Rows[0]["txtemp_office_name"].ToString();
                        this.txtemp_office_name_manager.Text = dt2.Rows[0]["txtemp_office_name_manager"].ToString();
                        this.txtemp_office_name_approve.Text = dt2.Rows[0]["txtemp_office_name_approve"].ToString();


                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");

                        this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();
                        this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();

                        this.PANEL1307_PROJECT_txtproject_name.Text = dt2.Rows[0]["txtproject_name"].ToString();
                        this.PANEL1307_PROJECT_txtproject_id.Text = dt2.Rows[0]["txtproject_id"].ToString();

                        this.PANEL1317_JOB_txtjob_name.Text = dt2.Rows[0]["txtjob_name"].ToString();
                        this.PANEL1317_JOB_txtjob_id.Text = dt2.Rows[0]["txtjob_id"].ToString();


                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.GridView1.ColumnCount = 10;
                            //this.GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.GridView1.Columns[5].Name = "Col_txtqty";
                            //this.GridView1.Columns[6].Name = "Col_txtprice";
                            //this.GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.GridView1.Columns[9].Name = "Col_date";

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");      //5
                            GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //6
                            GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //7
                            GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //8
                            GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
                        }
                        //=======================================================
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {

                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor.Current = Cursors.Default;
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
                    Cursor.Current = Cursors.Default;
                    conn.Close();
                }

                //===========================================
            }
            //================================
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

        }
        private void Fill_Show_Department()
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

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                   "k017db_pr_record_detail.*," +
                                   "k013_1db_acc_16department.*," +
                                   "k013_1db_acc_13group_tax.*" +

                                   " FROM k017db_pr_record" +

                                   " INNER JOIN k017db_pr_record_detail" +
                                   " ON k017db_pr_record.cdkey = k017db_pr_record_detail.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k017db_pr_record_detail.txtco_id" +
                                   " AND k017db_pr_record.txtPr_id = k017db_pr_record_detail.txtPr_id" +

                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON k017db_pr_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k017db_pr_record.txtPr_id = '" + this.txtPr_id.Text.Trim() + "')" +
                                  " ORDER BY k017db_pr_record.txtPr_id ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();
                        this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();

                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {

                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor.Current = Cursors.Default;
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
                    Cursor.Current = Cursors.Default;
                    conn.Close();
                }

                //===========================================
            }
            //================================

        }

        private void ch_approve_y_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ch_approve_y.Checked == true)
            {
                this.txtapprove_status_id.Text = "Y";
                this.ch_approve_r.Checked = false;
                this.ch_approve_n.Checked = false;
            }
        }

        private void ch_approve_r_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ch_approve_r.Checked == true)
            {
                this.txtapprove_status_id.Text = "R";
                this.ch_approve_y.Checked = false;
                this.ch_approve_n.Checked = false;
            }
        }

        private void ch_approve_n_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ch_approve_n.Checked == true)
            {
                this.txtapprove_status_id.Text = "N";
                this.ch_approve_y.Checked = false;
                this.ch_approve_r.Checked = false;
            }
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
                                  " FROM k019db_approve_record_trans" +
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
                            TMP = "AP" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "AP" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "AP" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtApprove_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }



        //PANEL_PO ระเบียน PO ====================================================
        private Point MouseDownLocation;
        private void PANEL_PO_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_PO_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PO.Left = e.X + PANEL_PO.Left - MouseDownLocation.X;
                PANEL_PO.Top = e.Y + PANEL_PO.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PO_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_PO_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PO.Left = e.X + PANEL_PO.Left - MouseDownLocation.X;
                PANEL_PO.Top = e.Y + PANEL_PO.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PO_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void PANEL_PO_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PO.Left = e.X + PANEL_PO.Left - MouseDownLocation.X;
                PANEL_PO.Top = e.Y + PANEL_PO.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PO_btnclose_Click(object sender, EventArgs e)
        {
            this.PANEL_PO.Visible = false;
        }
        private void PANEL_PO_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL_PO_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_PO.Height = PANEL_PO_btnresize_low.Top + e.Y;
                this.PANEL_PO.Width = PANEL_PO_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_PO_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }

        private void PANEL_PO_btnPr_id_Click(object sender, EventArgs e)
        {
            this.PANEL_PO.Visible = true;
            this.PANEL_PO.BringToFront();

        }

        private void Fill_Show_DATA_PANEL_PO_GridView1()
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

            Clear_PANEL_PO_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k018db_po_record.*," +
                                   "k016db_1supplier.*" +

                                   " FROM k018db_po_record" +
                                   " INNER JOIN k016db_1supplier" +
                                   " ON k018db_po_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND k018db_po_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k018db_po_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " WHERE (k018db_po_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k018db_po_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k018db_po_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " AND (k018db_po_record.txtapprove_id = '')" +
                                    " AND (k018db_po_record.txtsum_qty_balance > 0)" +
                                    " AND (k018db_po_record.txtpo_status = '0')" +

                                    " ORDER BY k018db_po_record.txtPo_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PO_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PO_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PO_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PO_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PO_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PO_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PO_GridView1.Columns[3].Name = "Col_txtPo_id";
                            //this.PANEL_PO_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PO_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PO_GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.PANEL_PO_GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.PANEL_PO_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PO_GridView1.Columns[9].Name = "Col_txtPr_id";
                            //this.PANEL_PO_GridView1.Columns[10].Name = "Col_txtpr_date";
                            //this.PANEL_PO_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PO_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PO_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PO_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PO_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PO_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PO_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PO_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PO_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PO_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PO_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PO_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PO_GridView1.Rows.Add();
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //3
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //9
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPr_date"].Value = dt2.Rows[j]["txtPr_date"].ToString();      //10
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }

                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PO_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PO_GridView1_Color();
        }
        private void Show_PANEL_PO_GridView1()
        {
            this.PANEL_PO_GridView1.ColumnCount = 23;
            this.PANEL_PO_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_PO_GridView1.Columns[1].Name = "Col_txtco_id";
            this.PANEL_PO_GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.PANEL_PO_GridView1.Columns[3].Name = "Col_txtPo_id";
            this.PANEL_PO_GridView1.Columns[4].Name = "Col_txttrans_date_server";
            this.PANEL_PO_GridView1.Columns[5].Name = "Col_txttrans_time";
            this.PANEL_PO_GridView1.Columns[6].Name = "Col_txtsupplier_id";
            this.PANEL_PO_GridView1.Columns[7].Name = "Col_txtsupplier_name";
            this.PANEL_PO_GridView1.Columns[8].Name = "Col_txtemp_office_name";
            this.PANEL_PO_GridView1.Columns[9].Name = "Col_txtPr_id";
            this.PANEL_PO_GridView1.Columns[10].Name = "Col_txtpr_date";
            this.PANEL_PO_GridView1.Columns[11].Name = "Col_txtapprove_id";
            this.PANEL_PO_GridView1.Columns[12].Name = "Col_txtapprove_date";
            this.PANEL_PO_GridView1.Columns[13].Name = "Col_txtRG_id";
            this.PANEL_PO_GridView1.Columns[14].Name = "Col_txtRG_date";
            this.PANEL_PO_GridView1.Columns[15].Name = "Col_txtReceive_id";
            this.PANEL_PO_GridView1.Columns[16].Name = "Col_txtReceive_date";
            this.PANEL_PO_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
            this.PANEL_PO_GridView1.Columns[18].Name = "Col_txtpr_status";
            this.PANEL_PO_GridView1.Columns[19].Name = "Col_txtpo_status";
            this.PANEL_PO_GridView1.Columns[20].Name = "Col_txtapprove_status";
            this.PANEL_PO_GridView1.Columns[21].Name = "Col_txtRG_status";
            this.PANEL_PO_GridView1.Columns[22].Name = "Col_txtreceive_status";

            this.PANEL_PO_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_PO_GridView1.Columns[1].HeaderText = "txtco_id";
            this.PANEL_PO_GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.PANEL_PO_GridView1.Columns[3].HeaderText = " PO ID";
            this.PANEL_PO_GridView1.Columns[4].HeaderText = " วันที่";
            this.PANEL_PO_GridView1.Columns[5].HeaderText = " เวลา";
            this.PANEL_PO_GridView1.Columns[6].HeaderText = " รหัส Supplier";
            this.PANEL_PO_GridView1.Columns[7].HeaderText = " ชื่อ Supplier";
            this.PANEL_PO_GridView1.Columns[8].HeaderText = " ผู้บันทึก";
            this.PANEL_PO_GridView1.Columns[9].HeaderText = " Pr ID";
            this.PANEL_PO_GridView1.Columns[10].HeaderText = " วันที่ Pr";
            this.PANEL_PO_GridView1.Columns[11].HeaderText = " Approve ID";
            this.PANEL_PO_GridView1.Columns[12].HeaderText = " วันที่ Approve";
            this.PANEL_PO_GridView1.Columns[13].HeaderText = " RG ID";
            this.PANEL_PO_GridView1.Columns[14].HeaderText = " วันที่ RG";
            this.PANEL_PO_GridView1.Columns[15].HeaderText = " ID รับเข้าคลัง";
            this.PANEL_PO_GridView1.Columns[16].HeaderText = " วันที่ รับเข้าคลัง";

            this.PANEL_PO_GridView1.Columns[17].HeaderText = " จำนวนเงิน(บาท)";
            this.PANEL_PO_GridView1.Columns[18].HeaderText = " สถานะ PR";
            this.PANEL_PO_GridView1.Columns[19].HeaderText = " สถานะ PO";
            this.PANEL_PO_GridView1.Columns[20].HeaderText = " สถานะ AP";
            this.PANEL_PO_GridView1.Columns[21].HeaderText = "สถานะ RG";
            this.PANEL_PO_GridView1.Columns[22].HeaderText = " สถานะ รับเข้าคลัง";

            this.PANEL_PO_GridView1.Columns[0].Visible = false;  //"Col_Auto_num";
            this.PANEL_PO_GridView1.Columns[1].Visible = false;  //"Col_txtco_id";
            this.PANEL_PO_GridView1.Columns[2].Visible = false;  //"Col_txtbranch_id";

            this.PANEL_PO_GridView1.Columns[3].Visible = true;  //"Col_txtPo_id";
            this.PANEL_PO_GridView1.Columns[3].Width = 120;
            this.PANEL_PO_GridView1.Columns[3].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[4].Visible = true;  //"Col_txttrans_date_server";
            this.PANEL_PO_GridView1.Columns[4].Width = 100;
            this.PANEL_PO_GridView1.Columns[4].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[5].Visible = true;  //"Col_txttrans_time";
            this.PANEL_PO_GridView1.Columns[5].Width = 80;
            this.PANEL_PO_GridView1.Columns[5].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[6].Visible = false;  //"Col_txtdepartment_id";

            this.PANEL_PO_GridView1.Columns[7].Visible = true;  //"Col_txtdepartment_name";
            this.PANEL_PO_GridView1.Columns[7].Width = 150;
            this.PANEL_PO_GridView1.Columns[7].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[8].Visible = true;  //"Col_txtemp_office_name";
            this.PANEL_PO_GridView1.Columns[8].Width = 120;
            this.PANEL_PO_GridView1.Columns[8].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[9].Visible = false;  //"Col_txtPr_id";
            this.PANEL_PO_GridView1.Columns[9].Width = 0;
            this.PANEL_PO_GridView1.Columns[9].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[10].Visible = false;  //"Col_txtpo_date";
            this.PANEL_PO_GridView1.Columns[10].Width = 0;
            this.PANEL_PO_GridView1.Columns[10].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[11].Visible = true;  //"Col_txtApprove_id";
            this.PANEL_PO_GridView1.Columns[11].Width = 120;
            this.PANEL_PO_GridView1.Columns[11].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[12].Visible = false;  //"Col_txtApprove_date";
            this.PANEL_PO_GridView1.Columns[12].Width = 0;
            this.PANEL_PO_GridView1.Columns[12].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_PO_GridView1.Columns[13].Visible = false;  //"Col_txtRG_id";
            this.PANEL_PO_GridView1.Columns[13].Width = 0;
            this.PANEL_PO_GridView1.Columns[13].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[14].Visible = false;  //"Col_txtRG_date";
            this.PANEL_PO_GridView1.Columns[14].Width = 0;
            this.PANEL_PO_GridView1.Columns[14].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[15].Visible = false;  //"Col_txtReceive_id";
            this.PANEL_PO_GridView1.Columns[15].Width = 0;
            this.PANEL_PO_GridView1.Columns[15].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[16].Visible = false;  //"Col_txtReceive_date";
            this.PANEL_PO_GridView1.Columns[16].Width = 0;
            this.PANEL_PO_GridView1.Columns[16].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[17].Visible = true;  //"Col_txtmoney_after_vat";
            this.PANEL_PO_GridView1.Columns[17].Width = 130;
            this.PANEL_PO_GridView1.Columns[17].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_PO_GridView1.Columns[18].Visible = false;  //"Col_txtpr_status";
            this.PANEL_PO_GridView1.Columns[18].Width = 0;
            this.PANEL_PO_GridView1.Columns[18].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[19].Visible = true;  //"Col_txtpo_status";
            this.PANEL_PO_GridView1.Columns[19].Width = 100;
            this.PANEL_PO_GridView1.Columns[19].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[20].Visible = true;  //"Col_txtapprove_status";
            this.PANEL_PO_GridView1.Columns[20].Width = 100;
            this.PANEL_PO_GridView1.Columns[20].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[21].Visible = false;  //"Col_txtRG_status";
            this.PANEL_PO_GridView1.Columns[21].Width = 0;
            this.PANEL_PO_GridView1.Columns[21].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[21].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[21].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.Columns[22].Visible = false;  //"Col_txtreceive_status";
            this.PANEL_PO_GridView1.Columns[22].Width = 0;
            this.PANEL_PO_GridView1.Columns[22].ReadOnly = true;
            this.PANEL_PO_GridView1.Columns[22].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PO_GridView1.Columns[22].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PO_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_PO_GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_PO_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_PO_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_PO_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_PO_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_PO_GridView1.EnableHeadersVisualStyles = false;

        }
        private void Clear_PANEL_PO_GridView1()
        {
            this.PANEL_PO_GridView1.Rows.Clear();
            this.PANEL_PO_GridView1.Refresh();
        }
        private void PANEL_PO_GridView1_Color()
        {
            for (int i = 0; i < this.PANEL_PO_GridView1.Rows.Count - 0; i++)
            {

                if (PANEL_PO_GridView1.Rows[i].Cells["Col_txtapprove_id"].Value == null)
                {
                    PANEL_PO_GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    PANEL_PO_GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    PANEL_PO_GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void PANEL_PO_GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.PANEL_PO_GridView1.Rows[e.RowIndex];
                if (row.Cells["Col_txtapprove_id"].Value == null)
                {
                    row.Cells["Col_txtapprove_id"].Value = "";
                }
                    var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    if (row.Cells["Col_txtapprove_id"].Value.ToString() != "")
                    {
                        //ชลอไปก่อน
                        if (row.Cells["Col_txtapprove_status"].Value.ToString() == "อนุมัติ")
                        {
                            MessageBox.Show("เอกสารใบนี้ อนุมัติ ไปแล้ว !!!!");
                            return;
                        }
                        if (row.Cells["Col_txtapprove_status"].Value.ToString() == "ไม่อนุมัติ")
                        {
                            MessageBox.Show("เอกสารใบนี้ ไม่อนุมัติ ไปแล้ว !!!!");
                            return;
                        }
                        if (row.Cells["Col_txtpo_status"].Value.ToString() == "ยกเลิก PO")
                        {
                            MessageBox.Show("เอกสารใบนี้ ยกเลิก ไปแล้ว !!!!");
                            return;
                        }
                    }
                    else
                    {
                        this.txtPo_id.Text = row.Cells["Col_txtPo_id"].Value.ToString();

                        if (this.PANEL_PO_cboSearch.Text == "เลขที่ PO")
                        {
                            this.PANEL_PO_txtsearch.Text = row.Cells["Col_txtPo_id"].Value.ToString();
                            this.txtPo_id.Text = row.Cells["Col_txtPo_id"].Value.ToString();

                        }
                        else if (this.PANEL_PO_cboSearch.Text == "ชื่อผู้บันทึก PO")
                        {
                            this.PANEL_PO_txtsearch.Text = row.Cells["Col_txtemp_office_name"].Value.ToString();

                        }
                        else
                        {
                            this.PANEL_PO_txtsearch.Text = row.Cells["Col_txtPo_id"].Value.ToString();
                            this.txtPo_id.Text = row.Cells["Col_txtPo_id"].Value.ToString();

                        }
                        SHOW_PO();
                    }
                }
                //=====================
            }
        }
        private void PANEL_PO_GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else
                {
                    PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void PANEL_PO_GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else
                {
                    PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    PANEL_PO_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void PANEL_PO_GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow = PANEL_PO_GridView1.CurrentRow.Index;
            int rowscount = PANEL_PO_GridView1.Rows.Count;

            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();

        }
        private void PANEL_PO_dtpstart_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_PO_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_PO_dtpstart.CustomFormat = this.PANEL_PO_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void PANEL_PO_dtpend_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_PO_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_PO_dtpend.CustomFormat = this.PANEL_PO_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

        }


 
        private void PANEL_PO_btnGo1_Click(object sender, EventArgs e)
        {
            Fill_Show_DATA_PANEL_PO_GridView1();
 

        }
        private void PANEL_PO_btnGo2_Click(object sender, EventArgs e)
        {
            if (this.PANEL_PO_cboSearch.Text == "")
            {
                MessageBox.Show("เลือก ประเภทการค้นหา ก่อน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL_PO_cboSearch.Focus();
                return;
            }
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

            Clear_PANEL_PO_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (this.PANEL_PO_cboSearch.Text == "เลขที่ PR")
                {
                    cmd2.CommandText = "SELECT k018db_po_record.*," +
                                       "k016db_1supplier.*" +

                                       " FROM k018db_po_record" +
                                       " INNER JOIN k016db_1supplier" +
                                       " ON k018db_po_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k018db_po_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND k018db_po_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " WHERE (k018db_po_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k018db_po_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k018db_po_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " AND (k018db_po_record.txtPr_id = '" + this.PANEL_PO_txtsearch.Text.Trim() + "')" +
                                      " ORDER BY k018db_po_record.txtPo_id ASC";

                }
                if (this.PANEL_PO_cboSearch.Text == "ชื่อผู้บันทึก PR")
                {
                    cmd2.CommandText = "SELECT k018db_po_record.*," +
                                       "k016db_1supplier.*" +

                                       " FROM k018db_po_record" +
                                       " INNER JOIN k016db_1supplier" +
                                       " ON k018db_po_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k018db_po_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND k018db_po_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " WHERE (k018db_po_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k018db_po_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k018db_po_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (k018db_po_record.txtemp_office_name LIKE '%" + this.PANEL_PO_txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY k018db_po_record.txtPo_id ASC";

                }

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PO_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PO_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PO_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PO_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PO_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PO_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PO_GridView1.Columns[3].Name = "Col_txtPo_id";
                            //this.PANEL_PO_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PO_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PO_GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.PANEL_PO_GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.PANEL_PO_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PO_GridView1.Columns[9].Name = "Col_txtPr_id";
                            //this.PANEL_PO_GridView1.Columns[10].Name = "Col_txtpr_date";
                            //this.PANEL_PO_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PO_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PO_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PO_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PO_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PO_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PO_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PO_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PO_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PO_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PO_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PO_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PO_GridView1.Rows.Add();
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //3
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //9
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPr_date"].Value = dt2.Rows[j]["txtPr_date"].ToString();      //10
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }

                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }

                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PO_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PO_GridView1_Color();
        }
        private void PANEL_PO_btnGo3_Click(object sender, EventArgs e)
        {
            SHOW_btnGo3();
        }
        private void SHOW_btnGo3()
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

            Clear_PANEL_PO_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (ch_all_po.Checked == true)
                {
                    cmd2.CommandText = "SELECT k018db_po_record.*," +
                                       "k016db_1supplier.*" +

                                       " FROM k018db_po_record" +
                                       " INNER JOIN k016db_1supplier" +
                                       " ON k018db_po_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k018db_po_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND k018db_po_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +
                                       " WHERE (k018db_po_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k018db_po_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (k018db_po_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (k018db_po_record.txtapprove_status = '')" +
                                       " ORDER BY k018db_po_record.txtPo_id ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT k018db_po_record.*," +
                                       "k016db_1supplier.*" +
                                       " FROM k018db_po_record" +
                                       " INNER JOIN k016db_1supplier" +
                                       " ON k018db_po_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k018db_po_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND k018db_po_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " WHERE (k018db_po_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k018db_po_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k018db_po_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY k018db_po_record.txtPo_id ASC";

                }
                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PO_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PO_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PO_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PO_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PO_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PO_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PO_GridView1.Columns[3].Name = "Col_txtPo_id";
                            //this.PANEL_PO_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PO_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PO_GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.PANEL_PO_GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.PANEL_PO_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PO_GridView1.Columns[9].Name = "Col_txtPr_id";
                            //this.PANEL_PO_GridView1.Columns[10].Name = "Col_txtpr_date";
                            //this.PANEL_PO_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PO_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PO_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PO_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PO_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PO_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PO_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PO_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PO_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PO_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PO_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PO_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PO_GridView1.Rows.Add();
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //3
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //9
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtPr_date"].Value = dt2.Rows[j]["txtPr_date"].ToString();      //10
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PO_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PO_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PO_GridView1_Color();

        }




        //PANEL_PO ระเบียน PO ====================================================



        //จบส่วนตารางสำหรับบันทึก========================================================================



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
                                this.GridView1.Visible = false;
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

                        this.GridView1.Visible = false;
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
                this.GridView1.Visible = true;
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
                this.GridView1.Visible = true;
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

        private void BtnGrid_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = "ระเบยนใบ อนุมัติ";
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_04AP frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_04AP();
            frm2.Show();

        }















        //===============================================================


    }
}
