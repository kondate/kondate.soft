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
    public partial class HOME02_Purchasing_02PO_record : Form
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


        public HOME02_Purchasing_02PO_record()
        {
            InitializeComponent();
            PANEL_MAT_GridView1.Controls.Add(dtp);
            dtp.Visible = false;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.TextChanged += new EventHandler(dtp_TextChange);

            GridView1.Controls.Add(dtp2);
            dtp2.Visible = false;
            dtp2.Format = DateTimePickerFormat.Custom;
            dtp2.TextChanged += new EventHandler(dtp2_TextChange);

        }

        private void HOME02_Purchasing_02PO_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0201PRRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มใบ PO ใหม่";

            this.ActiveControl = this.txtpo_remark;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;



            //ส่วนของ ระเบียน PR =================================================================            
            Show_PANEL_PR_GridView1();
            Fill_Show_DATA_PANEL_PR_GridView1();

            PANEL1316_DEPARTMENT_GridView1_department();
            PANEL1316_DEPARTMENT_Fill_department();

            this.PANEL_PR_dtpend.Value = DateTime.Now;
            this.PANEL_PR_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpend.CustomFormat = this.PANEL_PR_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.PANEL_PR_dtpstart.Value = DateTime.Today.AddDays(-7);
            this.PANEL_PR_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpstart.CustomFormat = this.PANEL_PR_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

            //========================================
            this.PANEL_PR_cboSearch.Items.Add("เลขที่ PR");
            this.PANEL_PR_cboSearch.Items.Add("ชื่อผู้บันทึก PR");
            //ส่วนของ ระเบียน PR =================================================================

            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            Show_GridView1();
            Check_Group_tax_of_user();

            PANEL1316_DEPARTMENT_GridView1_department();
            PANEL1316_DEPARTMENT_Fill_department();


            PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax();
            PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax();


            PANEL1307_PROJECT_GridView1_project();
            PANEL1307_PROJECT_Fill_project();

            PANEL1317_JOB_GridView1_job();
            PANEL1317_JOB_Fill_job();

            //1.ส่วนหน้าหลัก======================================================================
            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();

            //2.MAT ส่วนเลือกรายการสินค้า ===============================================================
            PANEL_MAT_Show_GridView1();

            //========================================
            this.cboSearch.Items.Add("รหัสสินค้า");
            this.cboSearch.Items.Add("ชื่อสินค้า");
            //========================================

            PANEL101_MAT_TYPE2_GridView1_mat_type();
            PANEL101_MAT_TYPE2_Fill_mat_type();

            PANEL102_MAT_SAC2_GridView1_mat_sac();
            PANEL102_MAT_SAC2_Fill_mat_sac();

            PANEL103_MAT_GROUP2_GridView1_mat_group();
            PANEL103_MAT_GROUP2_Fill_mat_group();

            PANEL104_MAT_BRAND2_GridView1_mat_brand();
            PANEL104_MAT_BRAND2_Fill_mat_brand();

            PANEL109_BOM_GridView1_bom();
            PANEL109_BOM_Fill_bom();

            this.PANEL_MAT.Visible = false;
            //END MATส่วนเลือกรายการสินค้า ===========================================================

        }

        private void HOME02_Purchasing_02PO_record_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                this.ActiveControl = this.txtmat_barcode_id;
                this.txtmat_barcode_id.Text = "";
            }
            if (e.KeyCode == Keys.F5)
            {
                UPDATE_TO_GridView1();
                GridView1_Color_Column();
                GridView1_Cal_Sum();
                Sum_group_tax();

                PANEL_MAT_Show_GridView1();
                PANEL_MAT_Clear_GridView1();
                this.PANEL_MAT.Visible = false;
                this.BtnSave.Enabled = true;

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
            var frm2 = new HOME02_Purchasing.HOME02_Purchasing_02PO_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มใบ PO ใหม่";
            this.txtPo_id.ReadOnly = true;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "N")
            {

                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิดแก้ไข";
            TRANS_LOG();

            if (this.txtPr_id.Text != "")
            {
                this.iblword_status.Text = "แก้ใบ PO";
                this.txtPr_id.ReadOnly = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
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
            Cursor.Current = Cursors.WaitCursor;
            string PR_STATUS = "";
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT * FROM k017db_pr_all" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtPr_id = '" + this.txtPr_id.Text.Trim() + "')";
                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;
                    PR_STATUS = "Y";
                    conn.Close();
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    PR_STATUS = "N";
                    conn.Close();

                }

                //
                conn.Close();
            }

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

                    if (this.iblword_status.Text.Trim() == "เพิ่มใบ PO ใหม่")
                    {

                        //1 k018db_po_record_trans
                        if (W_ID_Select.TRANS_BILL_STATUS.Trim() == "N")
                        {
                            cmd2.CommandText = "INSERT INTO k018db_po_record_trans(cdkey," +
                                               "txtco_id,txtbranch_id," +
                                               "txttrans_id)" +
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                               "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                               "'" + this.txtPo_id.Text.Trim() + "')";

                            cmd2.ExecuteNonQuery();


                        }
                        else
                        {
                            cmd2.CommandText = "UPDATE k018db_po_record_trans SET txttrans_id = '" + this.txtPo_id.Text.Trim() + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                            cmd2.ExecuteNonQuery();

                        }

                        //2 k018db_po_record
                        cmd2.CommandText = "INSERT INTO k018db_po_record(cdkey,txtco_id,txtbranch_id," +  //1
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                                "txtuser_name,txtemp_office_name," +  //5
                                               "txtversion_id," +  //6
                                                                   //====================================================

                                               "txtPo_id," + // 7
                                               "txtPr_id," + // 8
                                               "txtPr_date," + // 9
                                                "txtdepartment_id," + // 9
                                              "txtapprove_id," + // 10
                                               "txtapprove_date," + // 11
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
                                               "txtsum_qty_receive," + // 32
                                               "txtsum_qty_balance," + // 32

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


                                               "@txtPo_id," + // 7
                                               "@txtPr_id," + // 8
                                               "@txtPr_date," + // 9
                                                "@txtdepartment_id," + // 9
                                              "@txtapprove_id," + // 10
                                               "@txtapprove_date," + // 11
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
                                                "@txtsum_qty_receive," + // 32
                                               "@txtsum_qty_balance," + // 32

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

                        cmd2.Parameters.Add("@txtPo_id", SqlDbType.NVarChar).Value = this.txtPo_id.Text.Trim();  //7
                        cmd2.Parameters.Add("@txtPr_id", SqlDbType.NVarChar).Value = this.txtPr_id.Text.Trim();  //8
                        cmd2.Parameters.Add("@txtPr_date", SqlDbType.NVarChar).Value = "";  //9
                        cmd2.Parameters.Add("@txtdepartment_id", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.Trim();  //16
                        cmd2.Parameters.Add("@txtapprove_id", SqlDbType.NVarChar).Value ="";  //10
                        cmd2.Parameters.Add("@txtapprove_date", SqlDbType.NVarChar).Value = "";  //11
                        cmd2.Parameters.Add("@txtRG_id", SqlDbType.NVarChar).Value = "";  //12
                        cmd2.Parameters.Add("@txtRG_date", SqlDbType.NVarChar).Value = "";  //13
                        cmd2.Parameters.Add("@txtreceive_id", SqlDbType.NVarChar).Value = "";  //14
                        cmd2.Parameters.Add("@txtreceive_date", SqlDbType.NVarChar).Value = "";  //15


                        cmd2.Parameters.Add("@txtsupplier_id", SqlDbType.NVarChar).Value = this.PANEL161_SUP_txtsupplier_id.Text.Trim();  //16
                        cmd2.Parameters.Add("@txtcontact_person", SqlDbType.NVarChar).Value = this.txtcontact_person.Text.Trim();  //17
                        cmd2.Parameters.Add("@txtwant_mat_in_day", SqlDbType.NVarChar).Value = this.txtwant_mat_in_day.Text.Trim();  //18

                        DateTime date_send_mat = Convert.ToDateTime(this.dtpdate_send_mat.Value.ToString());
                        string d_send_mat = date_send_mat.ToString("yyyy-MM-dd");
                        cmd2.Parameters.Add("@txtdate_send_mat", SqlDbType.NVarChar).Value = d_send_mat;  //19

                        cmd2.Parameters.Add("@txtcredit_in_day", SqlDbType.NVarChar).Value = this.txtcredit_in_day.Text.Trim();  //20
                        cmd2.Parameters.Add("@txtpo_remark", SqlDbType.NVarChar).Value = this.txtpo_remark.Text.Trim();  //21

                        cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.ToString();  //22
                        cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.ToString();  //23

                        cmd2.Parameters.Add("@txtapprove_status_id", SqlDbType.NVarChar).Value = "";  //24

                        cmd2.Parameters.Add("@txtproject_id", SqlDbType.NVarChar).Value = this.PANEL1307_PROJECT_txtproject_id.Text.ToString();  //25
                        cmd2.Parameters.Add("@txtjob_id", SqlDbType.NVarChar).Value = this.PANEL1317_JOB_txtjob_id.Text.ToString();  //26

                        if (this.checkBox1_txtjob_send_mat_status.Checked==true)
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
                        cmd2.Parameters.Add("@txtsum_qty_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //32
                        cmd2.Parameters.Add("@txtsum_qty_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //32

                        cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //33
                        cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //34
                        cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //35
                        cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //36
                        cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //37
                        cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //38
                        cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //39

                        cmd2.Parameters.Add("@txtpr_status", SqlDbType.NVarChar).Value = "0";  //40
                        cmd2.Parameters.Add("@txtpo_status", SqlDbType.NVarChar).Value = "0";  //41
                        cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = "";  //42
                        cmd2.Parameters.Add("@txtRG_status", SqlDbType.NVarChar).Value = "";  //43
                        cmd2.Parameters.Add("@txtreceive_status", SqlDbType.NVarChar).Value = "";  //44
                        cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //45
                        cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);  //46

                        //==============================
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok1");


                        int s = 0;

                        for (int i = 0; i < this.GridView1.Rows.Count; i++)
                        {
                            s = i + 1;
                            if (this.GridView1.Rows[i].Cells[2].Value != null)
                            {
                                this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();

                                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value == null)
                                {
                                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = "0";
                                }
                                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value == null)
                                {
                                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = "0";
                                }
                                if (this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value == null)
                                {
                                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";
                                }
                                if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                                {
                                    //3 k018db_po_record_detail

                                    DateTime want_receive_date = Convert.ToDateTime(this.GridView1.Rows[i].Cells[9].Value.ToString());
                                    string want_date = want_receive_date.ToString("yyyy-MM-dd");

                                    cmd2.CommandText = "INSERT INTO k018db_po_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                       "txttrans_year,txttrans_month,txttrans_day," +
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
                                       "txtitem_no," +   //14
                                       "txtmat_po_remark," +  //15

                                       "txtcoDE_id," +  //16
                                       "txtbranchDE_id," +  //17

                                      "txtqty_cut_yokma," +  //52
                                      "txtqty_cut_yokpai," +  //53
                                      "txtqty_after_cut_yokpai," +  //54

                                      "txtqty_cut," +  //33
                                      "txtqty_after_cut," +  //33

                                       "txtcut_id) " +  //34

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +
                                "'" + this.txtPo_id.Text.Trim() + "'," +  //2
                                "'" + this.txtPr_id.Text.Trim() + "'," +  //2
                                "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //3
                                "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //4
                                "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +    //5
                                "'" + this.GridView1.Rows[i].Cells[4].Value.ToString() + "'," +  //6

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //7
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //7
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //8

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //9
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //10
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //11
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //12
                               "'" + want_date + "'," +  //13
                               "'" + this.GridView1.Rows[i].Cells["Col_Auto_num"].Value.ToString() + "'," +  //14
                               "''," + //15

                               "''," + //16 txtcoDE_id
                               "''," + //17 txtbranchDE_id

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "'," +   //45

                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //52
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //53

                            "'')";   //54

                                    cmd2.ExecuteNonQuery();

                                    //4

                                    //====================================================================================================
                                }
                            }
                        }

                    }

                    //MessageBox.Show("ok2");





                    //5 k018db_po_record_group_tax
                    if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text.Trim() == "N")
                    {
                        cmd2.CommandText = "INSERT INTO k018db_po_record_group_tax(cdkey," +
                                           "txtco_id,txtacc_group_tax_id," +
                                           "txtacc_group_tax_name," +
                                           "txtacc_group_tax_vat_rate," +
                                           "txtuser_name)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() + "'," +
                                           "'" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text.Trim() + "'," +
                                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + "'," +
                                           "'" + W_ID_Select.M_USERNAME.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE k018db_po_record_group_tax SET txtacc_group_tax_id = '" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() + "'," +
                                           "txtacc_group_tax_name = '" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text.Trim() + "'," +
                                           "txtacc_group_tax_vat_rate = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtuser_name = '" + W_ID_Select.M_USERNAME.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //6
                    cmd2.CommandText = "UPDATE k017db_pr_record SET txtPo_id = '" + this.txtPo_id.Text.Trim() + "'," +
                                       "txtpo_date = '" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "'," +
                                       "txtpo_status = '0'" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtPr_id = '" + this.txtPr_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok3");



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

                        if (this.iblword_status.Text.Trim() == "เพิ่มใบ PO ใหม่")
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

            //จบเชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT * FROM k019db_approve_record" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtPo_id = '" + this.txtPo_id.Text.Trim() + "')" +
                                    " AND (txtapprove_status <> 'Y')";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("เอกสารนี้   : '" + this.txtPo_id.Text.Trim() + "' ยังไม่อนุมัติ   ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    conn.Close();
                    return;
                }
            }

            //
            conn.Close();

            //จบเชื่อมต่อฐานข้อมูล=======================================================

            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

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
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_Print frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_Print();
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
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_k018db_po_record.rpt");


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
                rpt.SetParameterValue("txtpo_id", W_ID_Select.TRANS_ID.Trim());

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
            //=========================================================================================================================================
        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnbarcode_set_Click(object sender, EventArgs e)
        {
            this.ActiveControl = this.txtmat_barcode_id;
            this.txtmat_barcode_id.Text = "";

        }

        private void btnadd_mat_Click(object sender, EventArgs e)
        {
            this.PANEL_MAT_iblword_top.Text = "เลือกรายการสินค้า";
            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
                this.PANEL_MAT.BringToFront();
                this.PANEL_MAT.Location = new Point(this.panel_button_top_pictureBox.Location.X, this.panel_button_top_pictureBox.Location.Y);
                this.BtnSave.Enabled = false;
            }
            else
            {
                this.PANEL_MAT.Visible = false;
                this.BtnSave.Enabled = true;
            }
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

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        //1.ส่วนหน้าหลัก ตารางสำหรับบันทึก========================================================================

        DateTimePicker dtp2 = new DateTimePicker();
        Rectangle _Rectangle2;
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow2 = 0;
        private void txtmat_barcode_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_barcode_id.Text.Trim() != "")
            {
                Fill_MatID();

                UPDATE_BARCODE_TO_GridView1();
                this.txtmat_barcode_id.Text = "";
                this.txtmat_barcode_id.Focus();
            }
        }
        private void Fill_MatID()
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
            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                     "b001mat_04barcode.*," +
                                     "b001mat_06price_sale.*," +
                                     "b001_05mat_unit1.*" +
                                     " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_04barcode" +
                                    " ON b001mat.cdkey = b001mat_04barcode.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_04barcode.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_04barcode.txtmat_id" +

                                    " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +


                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat_04barcode.txtmat_barcode_id = '" + this.txtmat_barcode_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();      //2

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
        private void UPDATE_BARCODE_TO_GridView1()
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4
            String stringDateRecord = D1.ToString(); // get value from text field
            DateTime myDateTime_DateRecord = new DateTime();
            myDateTime_DateRecord = DateTime.ParseExact(stringDateRecord, "yyyy-MM-dd", UsaCulture);
            //=================================================================================

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


            PANEL_MAT_Show_GridView1();
            PANEL_MAT_Clear_GridView1();


            string OKY = "";
            double SumQty = 0;
            if (this.GridView1.Rows.Count > 0)
            {
                for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
                {

                    if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() == this.txtmat_id.Text.ToString())
                    {
                        SumQty = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtqty.Text.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = SumQty.ToString("N", new CultureInfo("en-US"));
                        return;
                    }
                    else
                    {
                        OKY = "N";
                    }
                }
            }
            else
            {
                OKY = "N";
            }

            if (OKY == "N")
            {
                Cursor.Current = Cursors.WaitCursor;

                //เชื่อมต่อฐานข้อมูล======================================================
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd2 = conn.CreateCommand();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = conn;


                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                         "b001mat_04barcode.*," +
                                         "b001mat_06price_sale.*," +
                                         "b001_05mat_unit1.*" +
                                         " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                          " INNER JOIN b001mat_04barcode" +
                                        " ON b001mat.cdkey = b001mat_04barcode.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_04barcode.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_04barcode.txtmat_id" +

                                        " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +


                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat_04barcode.txtmat_barcode_id = '" + this.txtmat_barcode_id.Text.Trim() + "')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

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
                                GridView1.Rows[index].Cells["Col_txtqty"].Value = this.txtqty.Text.ToString();      //5
                                GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                                GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                                GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                                GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9

                                GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = ".00";      //8
                                GridView1.Rows[index].Cells["Col_txtqty_cut_yokma"].Value = ".00";      //8
                                GridView1.Rows[index].Cells["Col_txtqty_cut_yokpai"].Value = ".00";      //8
                                GridView1.Rows[index].Cells["Col_txtqty_after_cut_yokpai"].Value = ".00";      //8

                                GridView1.Rows[index].Cells["Col_1"].Value = "1";      //8
                            }
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



            SumQty = 0;
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 15;
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

            this.GridView1.Columns[10].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[11].Name = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[12].Name = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[13].Name = "Col_txtqty_after_cut_yokpai";

            this.GridView1.Columns[14].Name = "Col_1";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "ลำดับ";
            this.GridView1.Columns[2].HeaderText = " รหัส";
            this.GridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.GridView1.Columns[4].HeaderText = " หน่วยนับ";
            this.GridView1.Columns[5].HeaderText = " จำนวน";
            this.GridView1.Columns[6].HeaderText = " ราคา/หน่วย(บาท)";
            this.GridView1.Columns[7].HeaderText = " ส่วนลด(บาท)";
            this.GridView1.Columns[8].HeaderText = " จำนวนเงิน(บาท)";
            this.GridView1.Columns[9].HeaderText = " วันที่ต้องการสินค้า";

            this.GridView1.Columns[10].HeaderText = "Col_txtqty_after_cut";
            this.GridView1.Columns[11].HeaderText = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[12].HeaderText = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[13].HeaderText = "Col_txtqty_after_cut_yokpai";

            this.GridView1.Columns[14].HeaderText = "Col_1";

            this.GridView1.Columns[10].Visible = false;
            this.GridView1.Columns[11].Visible = false;
            this.GridView1.Columns[12].Visible = false;
            this.GridView1.Columns[13].Visible = false;
            this.GridView1.Columns[14].Visible = false;

            this.GridView1.Columns["Col_Auto_num"].Visible = true;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 36;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no";

            this.GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 100;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 150;
            this.GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView1.Columns["Col_txtmat_unit1_name"].Width = 100;
            this.GridView1.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtprice"].Visible = true;  //"Col_txtprice";
            this.GridView1.Columns["Col_txtprice"].Width = 100;
            this.GridView1.Columns["Col_txtprice"].ReadOnly = false;
            this.GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_money"].Visible = true;  //"Col_txtdiscount_money";
            this.GridView1.Columns["Col_txtdiscount_money"].Width = 100;
            this.GridView1.Columns["Col_txtdiscount_money"].ReadOnly = false;
            this.GridView1.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_total"].Visible = true;  //"Col_txtsum_total";
            this.GridView1.Columns["Col_txtsum_total"].Width = 150;
            this.GridView1.Columns["Col_txtsum_total"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_date"].Visible = true;  //"Col_date";
            this.GridView1.Columns["Col_date"].Width = 150;
            this.GridView1.Columns["Col_date"].ReadOnly = false;
            this.GridView1.Columns["Col_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            this.btnremove_row.Visible = true;
            this.btnUp.Visible = true;

            switch (GridView1.Columns[e.ColumnIndex].Name)
            {
                case "Col_txtmat_no":
                    dtp2.Visible = false;
                    break;
                case "Col_txtmat_id":
                    dtp2.Visible = false;
                    break;
                case "Col_txtmat_name":
                    dtp2.Visible = false;
                    break;
                case "Col_txtqty":
                    dtp2.Visible = false;
                    break;
                case "Col_txtprice":
                    dtp2.Visible = false;
                    break;
                case "Col_txtdiscount_money":
                    dtp2.Visible = false;
                    break;
                case "Col_txtsum_total":
                    dtp2.Visible = false;
                    break;
                case "Col_date":
                    dtp2.Visible = false;
                    break;
            }
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
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
        private void GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow2 = GridView1.CurrentRow.Index;
            int rowscount = GridView1.Rows.Count;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();

        }
        private void GridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtp2.Visible = false;
        }
        private void GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            dtp2.Visible = false;
        }
        private void GridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt2 = e.Control as TextBox;
            txt2.PreviewKeyDown += new PreviewKeyDownEventHandler(txt2_PreviewKeyDown);
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
        private void dtp2_TextChange(object sender, EventArgs e)
        {
            //GridView1.CurrentCell.Value = dtp2.Value.ToString("dd-MM-yyyy", UsaCulture);
            GridView1.CurrentCell.Value = dtp2.Value.ToString("yyyy-MM-dd", UsaCulture);
        }
        void txt2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
                    if (this.GridView1.Rows[i].Cells["Col_Auto_num"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtprice"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtprice"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = "0";
                    }


                    //5 * 6 = 8

                    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty"].Value).ToString("###,###.00");     //5
                    this.GridView1.Rows[i].Cells["Col_txtprice"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtprice"].Value).ToString("###,###.00");     //6
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value).ToString("###,###.00");     //8


                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmat_id"].Value;
                if (cell != null)
                {
                    this.txtqty.Text = row.Cells["Col_txtqty"].Value.ToString();
                }
            }
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (this.GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_id"].Value == null)
            {
                return;
            }
            this.GridView1.Rows[selectedRowIndex].Cells["Col_txtqty"].Value = this.txtqty.Text.ToString();
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

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
        private void Check_Group_tax_of_user()
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

                cmd1.CommandText = "SELECT * FROM k017db_pr_record_group_tax" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtuser_name = '" + W_ID_Select.M_USERNAME.Trim() + "')";
                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text = "Y";

                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt.Rows[0]["txtacc_group_tax_id"].ToString();      //1
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt.Rows[0]["txtacc_group_tax_name"].ToString();      //2
                    this.txtvat_rate.Text = Convert.ToSingle(dt.Rows[0]["txtacc_group_tax_vat_rate"]).ToString("###,###.00");        //3
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text = "Y";
                }
                else
                {
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text = "N";
                }

            }

            //
            conn.Close();

            //จบเชื่อมต่อฐานข้อมูล=======================================================

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


        private void Paneldate_btndate1_Click(object sender, EventArgs e)
        {
            if (this.dtpcurrency_date.Visible == false)
            {
                this.dtpcurrency_date.Visible = true;
                this.dtpcurrency_date.BringToFront();
                this.dtpcurrency_date.Location = new Point(this.Paneldate_txtcurrency_date.Location.X, this.Paneldate_txtcurrency_date.Location.Y + 22);
                this.Paneldate_btndate1.Visible = false;
                this.Paneldate_btndate1_close.Visible = true;
                this.Paneldate_btndate1_close.BringToFront();
                this.Paneldate_btndate1_close.Location = new Point(this.Paneldate_txtcurrency_date.Location.X + 140, this.Paneldate_txtcurrency_date.Location.Y + 24);

            }
            else
            {
                this.dtpcurrency_date.Visible = false;
                this.Paneldate_btndate1.Visible = true;
                this.Paneldate_btndate1_close.Visible = false;
            }
        }

        private void Paneldate_btndate1_close_Click(object sender, EventArgs e)
        {
            this.Paneldate_btndate1_close.Visible = false;
            this.Paneldate_btndate1.Visible = true;
            this.dtpcurrency_date.Visible = false;

        }

        private void dtpcurrency_date_ValueChanged(object sender, EventArgs e)
        {
            this.dtpcurrency_date.Format = DateTimePickerFormat.Custom;
            this.dtpcurrency_date.CustomFormat = this.dtpcurrency_date.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.Paneldate_txtcurrency_date.Text = this.dtpcurrency_date.Value.ToString("dd-MM-yyyy", UsaCulture);

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
                                  " FROM k018db_po_record_trans" +
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
                            TMP = "PO" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "PO" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "PO" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtPo_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }



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
                this.PANEL1316_DEPARTMENT.Width = 522;
                this.PANEL1316_DEPARTMENT.Height = 377;

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

                var cell = row.Cells["Col_txtdepartment_id"].Value;
                if (cell != null)
                {
                    this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = row.Cells["Col_txtdepartment_id"].Value.ToString();
                    this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = row.Cells["Col_txtdepartment_name"].Value.ToString();
                }
            }
        }
        private void PANEL1316_DEPARTMENT_dataGridView1_department_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

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



        //PANEL_PR ระเบียน PR ====================================================
        private Point MouseDownLocation;
        private void PANEL_PR_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_PR_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PR.Left = e.X + PANEL_PR.Left - MouseDownLocation.X;
                PANEL_PR.Top = e.Y + PANEL_PR.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PR_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_PR_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PR.Left = e.X + PANEL_PR.Left - MouseDownLocation.X;
                PANEL_PR.Top = e.Y + PANEL_PR.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PR_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void PANEL_PR_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PR.Left = e.X + PANEL_PR.Left - MouseDownLocation.X;
                PANEL_PR.Top = e.Y + PANEL_PR.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PR_btnclose_Click(object sender, EventArgs e)
        {
            this.PANEL_PR.Visible = false;
        }
        private void PANEL_PR_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL_PR_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_PR.Height = PANEL_PR_btnresize_low.Top + e.Y;
                this.PANEL_PR.Width = PANEL_PR_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_PR_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }

        private void PANEL_PR_btnPr_id_Click(object sender, EventArgs e)
        {
            this.PANEL_PR.Visible = true;
            this.PANEL_PR.BringToFront();

        }

        private void Fill_Show_DATA_PANEL_PR_GridView1()
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                   "k013_1db_acc_16department.*" +

                                   " FROM k017db_pr_record" +
                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                   " AND (k017db_pr_record.txtPo_id = '')" +
                                    " AND (k017db_pr_record.txtpr_status = '0')" +
                                   " ORDER BY k017db_pr_record.txtPr_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PR_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PR_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PR_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_date"].Value = dt2.Rows[j]["txtPo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }

                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PR_GridView1_Color();
        }
        private void Show_PANEL_PR_GridView1()
        {
            this.PANEL_PR_GridView1.ColumnCount = 23;
            this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
            this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
            this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
            this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";
            this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
            this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
            this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";
            this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
            this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
            this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtapprove_id";
            this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtapprove_date";
            this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtRG_id";
            this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtRG_date";
            this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtReceive_id";
            this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtReceive_date";
            this.PANEL_PR_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
            this.PANEL_PR_GridView1.Columns[18].Name = "Col_txtpr_status";
            this.PANEL_PR_GridView1.Columns[19].Name = "Col_txtpo_status";
            this.PANEL_PR_GridView1.Columns[20].Name = "Col_txtapprove_status";
            this.PANEL_PR_GridView1.Columns[21].Name = "Col_txtRG_status";
            this.PANEL_PR_GridView1.Columns[22].Name = "Col_txtreceive_status";

            this.PANEL_PR_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_PR_GridView1.Columns[1].HeaderText = "txtco_id";
            this.PANEL_PR_GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.PANEL_PR_GridView1.Columns[3].HeaderText = " PR ID";
            this.PANEL_PR_GridView1.Columns[4].HeaderText = " วันที่";
            this.PANEL_PR_GridView1.Columns[5].HeaderText = " เวลา";
            this.PANEL_PR_GridView1.Columns[6].HeaderText = " รหัสฝ่าย";
            this.PANEL_PR_GridView1.Columns[7].HeaderText = " ฝ่าย";
            this.PANEL_PR_GridView1.Columns[8].HeaderText = " ผู้บันทึก";
            this.PANEL_PR_GridView1.Columns[9].HeaderText = " PO ID";
            this.PANEL_PR_GridView1.Columns[10].HeaderText = " วันที่ PO";
            this.PANEL_PR_GridView1.Columns[11].HeaderText = " Approve ID";
            this.PANEL_PR_GridView1.Columns[12].HeaderText = " วันที่ Approve";
            this.PANEL_PR_GridView1.Columns[13].HeaderText = " RG ID";
            this.PANEL_PR_GridView1.Columns[14].HeaderText = " วันที่ RG";
            this.PANEL_PR_GridView1.Columns[15].HeaderText = " ID รับเข้าคลัง";
            this.PANEL_PR_GridView1.Columns[16].HeaderText = " วันที่ รับเข้าคลัง";

            this.PANEL_PR_GridView1.Columns[17].HeaderText = " จำนวนเงิน(บาท)";
            this.PANEL_PR_GridView1.Columns[18].HeaderText = " สถานะ PR";
            this.PANEL_PR_GridView1.Columns[19].HeaderText = " สถานะ PO";
            this.PANEL_PR_GridView1.Columns[20].HeaderText = " สถานะ ผลอนุมัติ";
            this.PANEL_PR_GridView1.Columns[21].HeaderText = "สถานะ RG";
            this.PANEL_PR_GridView1.Columns[22].HeaderText = " สถานะ รับเข้าคลัง";

            this.PANEL_PR_GridView1.Columns[0].Visible = false;  //"Col_Auto_num";
            this.PANEL_PR_GridView1.Columns[1].Visible = false;  //"Col_txtco_id";
            this.PANEL_PR_GridView1.Columns[2].Visible = false;  //"Col_txtbranch_id";

            this.PANEL_PR_GridView1.Columns[3].Visible = true;  //"Col_txtPr_id";
            this.PANEL_PR_GridView1.Columns[3].Width = 120;
            this.PANEL_PR_GridView1.Columns[3].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[4].Visible = true;  //"Col_txttrans_date_server";
            this.PANEL_PR_GridView1.Columns[4].Width = 100;
            this.PANEL_PR_GridView1.Columns[4].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[5].Visible = true;  //"Col_txttrans_time";
            this.PANEL_PR_GridView1.Columns[5].Width = 80;
            this.PANEL_PR_GridView1.Columns[5].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[6].Visible = false;  //"Col_txtdepartment_id";

            this.PANEL_PR_GridView1.Columns[7].Visible = true;  //"Col_txtdepartment_name";
            this.PANEL_PR_GridView1.Columns[7].Width = 100;
            this.PANEL_PR_GridView1.Columns[7].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[8].Visible = true;  //"Col_txtemp_office_name";
            this.PANEL_PR_GridView1.Columns[8].Width = 120;
            this.PANEL_PR_GridView1.Columns[8].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[9].Visible = true;  //"Col_txtPo_id";
            this.PANEL_PR_GridView1.Columns[9].Width = 120;
            this.PANEL_PR_GridView1.Columns[9].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[10].Visible = false;  //"Col_txtpo_date";
            this.PANEL_PR_GridView1.Columns[10].Width = 0;
            this.PANEL_PR_GridView1.Columns[10].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[11].Visible = false;  //"Col_txtApprove_id";
            this.PANEL_PR_GridView1.Columns[11].Width = 0;
            this.PANEL_PR_GridView1.Columns[11].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[12].Visible = false;  //"Col_txtApprove_date";
            this.PANEL_PR_GridView1.Columns[12].Width = 0;
            this.PANEL_PR_GridView1.Columns[12].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_PR_GridView1.Columns[13].Visible = false;  //"Col_txtRG_id";
            this.PANEL_PR_GridView1.Columns[13].Width = 0;
            this.PANEL_PR_GridView1.Columns[13].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[14].Visible = false;  //"Col_txtRG_date";
            this.PANEL_PR_GridView1.Columns[14].Width = 0;
            this.PANEL_PR_GridView1.Columns[14].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[15].Visible = false;  //"Col_txtReceive_id";
            this.PANEL_PR_GridView1.Columns[15].Width = 0;
            this.PANEL_PR_GridView1.Columns[15].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[16].Visible = false;  //"Col_txtReceive_date";
            this.PANEL_PR_GridView1.Columns[16].Width = 0;
            this.PANEL_PR_GridView1.Columns[16].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[17].Visible = true;  //"Col_txtmoney_after_vat";
            this.PANEL_PR_GridView1.Columns[17].Width = 130;
            this.PANEL_PR_GridView1.Columns[17].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_PR_GridView1.Columns[18].Visible = true;  //"Col_txtpr_status";
            this.PANEL_PR_GridView1.Columns[18].Width = 100;
            this.PANEL_PR_GridView1.Columns[18].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[19].Visible = false;  //"Col_txtpo_status";
            this.PANEL_PR_GridView1.Columns[19].Width = 0;
            this.PANEL_PR_GridView1.Columns[19].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[20].Visible = false;  //"Col_txtapprove_status";
            this.PANEL_PR_GridView1.Columns[20].Width = 0;
            this.PANEL_PR_GridView1.Columns[20].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[21].Visible = false;  //"Col_txtRG_status";
            this.PANEL_PR_GridView1.Columns[21].Width = 0;
            this.PANEL_PR_GridView1.Columns[21].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[21].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[21].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[22].Visible = false;  //"Col_txtreceive_status";
            this.PANEL_PR_GridView1.Columns[22].Width = 0;
            this.PANEL_PR_GridView1.Columns[22].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[22].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[22].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);

            this.PANEL_PR_GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_PR_GridView1.EnableHeadersVisualStyles = false;

        }
        private void Clear_PANEL_PR_GridView1()
        {
            this.PANEL_PR_GridView1.Rows.Clear();
            this.PANEL_PR_GridView1.Refresh();
        }
        private void PANEL_PR_GridView1_Color()
        {
            for (int i = 0; i < this.PANEL_PR_GridView1.Rows.Count - 0; i++)
            {

                if (PANEL_PR_GridView1.Rows[i].Cells["Col_txtPo_id"].Value == null)
                {
                        PANEL_PR_GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        PANEL_PR_GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        PANEL_PR_GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void PANEL_PR_GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.PANEL_PR_GridView1.Rows[e.RowIndex];
                if (row.Cells["Col_txtPo_id"].Value == null)
                {
                    row.Cells["Col_txtPo_id"].Value = "";
                }
                var cell = row.Cells["Col_txtPr_id"].Value;
                if (cell != null)
                {
                    if (row.Cells["Col_txtPo_id"].Value.ToString() != "")
                    {
                        //MessageBox.Show("ddddd ออกใบ PO ไปแล้ว !!!!");

                        if (row.Cells["Col_txtpo_status"].Value.ToString() == "ออก PO")
                        {
                            MessageBox.Show("เอกสารใบนี้ ออกใบ PO ไปแล้ว !!!!");
                            return;
                        }
                        if (row.Cells["Col_txtpr_status"].Value.ToString() == "ยกเลิก PR")
                        {
                            MessageBox.Show("ยกเลิกใบ Pr ไปแล้ว !!!!");
                            return;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("เอกสารใsssssบนี้ ออกใบ PO ไปแล้ว !!!!");

                        this.txtPr_id.Text = row.Cells["Col_txtPr_id"].Value.ToString();

                        if (this.PANEL_PR_cboSearch.Text == "เลขที่ PR")
                        {
                            this.PANEL_PR_txtsearch.Text = row.Cells["Col_txtPr_id"].Value.ToString();
                            this.txtPr_id.Text = row.Cells["Col_txtPr_id"].Value.ToString();

                        }
                        else if (this.PANEL_PR_cboSearch.Text == "ชื่อผู้บันทึก PR")
                        {
                            this.PANEL_PR_txtsearch.Text = row.Cells["Col_txtemp_office_name"].Value.ToString();

                        }
                        else
                        {
                            this.PANEL_PR_txtsearch.Text = row.Cells["Col_txtPr_id"].Value.ToString();
                            this.txtPr_id.Text = row.Cells["Col_txtPr_id"].Value.ToString();

                        }
                        SHOW_PR();
                    }
                    //=====================
                }
            }
        }
        private void PANEL_PR_GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else
                {
                    PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void PANEL_PR_GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else
                {
                        PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                       PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                       PANEL_PR_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                    }
            }
        }
        private void PANEL_PR_GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow = PANEL_PR_GridView1.CurrentRow.Index;
            int rowscount = PANEL_PR_GridView1.Rows.Count;

            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();

 
        }
        private void PANEL_PR_dtpstart_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_PR_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpstart.CustomFormat = this.PANEL_PR_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void PANEL_PR_dtpend_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_PR_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpend.CustomFormat = this.PANEL_PR_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void PANEL_PR_btnGo2_Click(object sender, EventArgs e)
        {
            Fill_Show_DATA_PANEL_PR_GridView1();
        }

        private void PANEL_PR_btnGo3_Click(object sender, EventArgs e)
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                   "k013_1db_acc_16department.*" +

                                   " FROM k017db_pr_record" +
                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " AND (k017db_pr_record.txtdepartment_id = '" + this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.Trim() + "')" +
                                   " ORDER BY k017db_pr_record.txtPr_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PR_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PR_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PR_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_date"].Value = dt2.Rows[j]["txtPo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }

                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }



                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PR_GridView1_Color();
        }

        private void PANEL_PR_btnGo1_Click(object sender, EventArgs e)
        {
            if (this.PANEL_PR_cboSearch.Text == "")
            {
                MessageBox.Show("เลือก ประเภทการค้นหา ก่อน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL_PR_cboSearch.Focus();
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (this.PANEL_PR_cboSearch.Text == "เลขที่ PR")
                {
                    cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                       "k013_1db_acc_16department.*" +

                                       " FROM k017db_pr_record" +
                                       " INNER JOIN k013_1db_acc_16department" +
                                       " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                       " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " AND (k017db_pr_record.txtPr_id = '" + this.PANEL_PR_txtsearch.Text.Trim() + "')" +
                                      " ORDER BY k017db_pr_record.txtPr_id ASC";

                }
                if (this.PANEL_PR_cboSearch.Text == "ชื่อผู้บันทึก PR")
                {
                    cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                       "k013_1db_acc_16department.*" +

                                       " FROM k017db_pr_record" +
                                       " INNER JOIN k013_1db_acc_16department" +
                                       " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                       " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (k017db_pr_record.txtemp_office_name LIKE '%" + this.PANEL_PR_txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY k017db_pr_record.txtPr_id ASC";

                }

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PR_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PR_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PR_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_date"].Value = dt2.Rows[j]["txtPo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }

                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }




                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PR_GridView1_Color();
        }

        private void btnPR_id_Click(object sender, EventArgs e)
        {
            if (this.PANEL_PR.Visible == false)
            {
                this.PANEL_PR.Visible = true;
                this.PANEL_PR.BringToFront();
                this.PANEL_PR.Location = new Point(this.label19.Location.X, this.txtPr_id.Location.Y + 22);
                this.PANEL_PR_iblword_top.Text = "ระเบียนใบขอสั่งซ์้อ PR";
                SHOW_btnGo4();

            }
            else
            {
                this.PANEL_PR.Visible = false;
            }
        }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            SHOW_PR();
        }
        private void SHOW_PR()
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
                            GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //5
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
        private void btnGo4_Click(object sender, EventArgs e)
        {
            SHOW_btnGo4();
        }
        private void SHOW_btnGo4()
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (ch_all_po.Checked == true)
                {
                    cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                       "k013_1db_acc_16department.*" +

                                       " FROM k017db_pr_record" +
                                       " INNER JOIN k013_1db_acc_16department" +
                                       " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +
                                       " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (k017db_pr_record.txtpo_status = '')" +
                                       " ORDER BY k017db_pr_record.txtPr_id ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                       "k013_1db_acc_16department.*" +
                                       " FROM k017db_pr_record" +
                                       " INNER JOIN k013_1db_acc_16department" +
                                       " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                       " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY k017db_pr_record.txtPr_id ASC";

                }
                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";

                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";

                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtapprove_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtapprove_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtReceive_id";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtReceive_date";
                            //this.PANEL_PR_GridView1.Columns[17].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[18].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[19].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[20].Name = "Col_txtapprove_status";
                            //this.PANEL_PR_GridView1.Columns[21].Name = "Col_txtRG_status";
                            //this.PANEL_PR_GridView1.Columns[22].Name = "Col_txtreceive_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_date"].Value = dt2.Rows[j]["txtPo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_date"].Value = dt2.Rows[j]["txtreceive_date"].ToString();      //12

                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //18
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //18
                            }

                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //19
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //19
                            }

                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //21
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //21
                            }


                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //22
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิกรับเข้าคลัง"; //22
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_PR_GridView1_Color();

        }

        //PANEL_PR ระเบียน PR ====================================================


        //txtdepartment ชื่อฝ่าย  =======================================================================


        //END txtdepartment ชื่อฝ่าย  =======================================================================

        //END PANEL_PR  ระเบียน PR====================================================


        //txtsupplier Supplier  =======================================================================
        private void PANEL161_SUP_Fill_supplier()
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

            PANEL161_SUP_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
                //this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
                //this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
                //this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
                //this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtsupplier_status";

                cmd2.CommandText = "SELECT k016db_1supplier.*," +
                                    "k016db_2supplier_address.*" +
                                    " FROM k016db_1supplier" +

                                    " INNER JOIN k016db_2supplier_address" +
                                    " ON k016db_1supplier.cdkey = k016db_2supplier_address.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_2supplier_address.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_2supplier_address.txtsupplier_id" +

                                    " WHERE (k016db_1supplier.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k016db_1supplier.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                     " AND (k016db_1supplier.txtsupplier_id <> '')" +
                                   " ORDER BY k016db_1supplier.txtsupplier_no ASC";

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
                            var index = PANEL161_SUP_dataGridView1.Rows.Add();
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_no"].Value = dt2.Rows[j]["txtsupplier_no"].ToString();      //1
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //2
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //3
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name_eng"].Value = dt2.Rows[j]["txtsupplier_name_eng"].ToString();      //4
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_status"].Value = dt2.Rows[j]["txtsupplier_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL161_SUP_Clear_GridView1_Up_Status();

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
            //================================

        }
        private void PANEL161_SUP_GridView1_supplier()
        {
            this.PANEL161_SUP_dataGridView1.ColumnCount = 9;
            this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
            this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
            this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
            this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
            this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtsupplier_status";

            this.PANEL161_SUP_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderText = " ชื่อ Supplier";
            this.PANEL161_SUP_dataGridView1.Columns[4].HeaderText = " ชื่อ Supplier Eng";
            this.PANEL161_SUP_dataGridView1.Columns[5].HeaderText = " ผู้ติดต่อ";
            this.PANEL161_SUP_dataGridView1.Columns[6].HeaderText = " เบอร์โทร";
            this.PANEL161_SUP_dataGridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.PANEL161_SUP_dataGridView1.Columns[8].HeaderText = " สถานะ";

            this.PANEL161_SUP_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL161_SUP_dataGridView1.Columns[1].Visible = false;  //"Col_txtsupplier_no";
            this.PANEL161_SUP_dataGridView1.Columns[1].Width = 0;
            this.PANEL161_SUP_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[2].Visible = true;  //"Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[2].Width = 100;
            this.PANEL161_SUP_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[3].Visible = true;  //"Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[3].Width = 250;
            this.PANEL161_SUP_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[4].Visible = false;  //"Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[4].Width = 0;
            this.PANEL161_SUP_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[5].Visible = true;  //"Col_txtcontact_person";
            this.PANEL161_SUP_dataGridView1.Columns[5].Width = 200;
            this.PANEL161_SUP_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[6].Visible = false;  //"Col_txtcontact_person_tel";
            this.PANEL161_SUP_dataGridView1.Columns[6].Width = 0;
            this.PANEL161_SUP_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[7].Visible = true;  //"Col_txtremark";
            this.PANEL161_SUP_dataGridView1.Columns[7].Width = 300;
            this.PANEL161_SUP_dataGridView1.Columns[7].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[8].Visible = false;  //"Col_txtsupplier_status";

            this.PANEL161_SUP_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL161_SUP_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL161_SUP_dataGridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL161_SUP_dataGridView1.Columns.Add(dgvCmb);

        }
        private void PANEL161_SUP_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL161_SUP_dataGridView1.Rows.Count; i++)
            {
                if (this.PANEL161_SUP_dataGridView1.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.PANEL161_SUP_dataGridView1.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.PANEL161_SUP_dataGridView1.Rows[i].Cells[9].Value = false;

                }
            }
        }
        private void PANEL161_SUP_Clear_GridView1()
        {
            this.PANEL161_SUP_dataGridView1.Rows.Clear();
            this.PANEL161_SUP_dataGridView1.Refresh();
        }
        private void PANEL161_SUP_txtsupplier_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL161_SUP.Visible == false)
                {
                    this.PANEL161_SUP.Visible = true;
                    this.PANEL161_SUP.Location = new Point(this.PANEL161_SUP_txtsupplier_name.Location.X, this.PANEL161_SUP_txtsupplier_name.Location.Y + 22);
                    this.PANEL161_SUP_dataGridView1.Focus();
                }
                else
                {
                    this.PANEL161_SUP.Visible = false;
                }
        }
        private void PANEL161_SUP_btnsupplier_Click(object sender, EventArgs e)
        {
            if (this.PANEL161_SUP.Visible == false)
            {
                this.PANEL161_SUP.Width = 502;
                this.PANEL161_SUP.Height = 337;

                this.PANEL161_SUP.Visible = true;
                this.PANEL161_SUP.BringToFront();
                this.PANEL161_SUP.Location = new Point(this.PANEL161_SUP_txtsupplier_name.Location.X, this.PANEL161_SUP_txtsupplier_name.Location.Y + 22);
            }
            else
            {
                this.PANEL161_SUP.Visible = false;
            }
        }
        private void PANEL161_SUP_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL161_SUP.Visible == false)
            {
                this.PANEL161_SUP.Visible = true;
            }
            else
            {
                this.PANEL161_SUP.Visible = false;
            }
        }
        private void PANEL161_SUP_dataGridView1_supplier_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL161_SUP_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL161_SUP_txtsupplier_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL161_SUP_txtsupplier_name.Text = row.Cells[3].Value.ToString();
                    //Col_txtcontact_person
                    this.txtcontact_person.Text = row.Cells["Col_txtcontact_person"].Value.ToString();
                }
            }
        }
        private void PANEL161_SUP_dataGridView1_supplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL161_SUP_dataGridView1.CurrentRow.Index;

                this.PANEL161_SUP_txtsupplier_id.Text = PANEL161_SUP_dataGridView1.CurrentRow.Cells[1].Value.ToString();
                this.PANEL161_SUP_txtsupplier_name.Text = PANEL161_SUP_dataGridView1.CurrentRow.Cells[2].Value.ToString();
                this.PANEL161_SUP_txtsupplier_name.Focus();
                this.PANEL161_SUP.Visible = false;
            }
        }
        private void PANEL161_SUP_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL161_SUP_btn_search_Click(object sender, EventArgs e)
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

            PANEL161_SUP_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
                //this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
                //this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
                //this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
                //this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtsupplier_status";

                cmd2.CommandText = "SELECT k016db_1supplier.*," +
                                    "k016db_2supplier_address.*" +
                                    " FROM k016db_1supplier" +

                                    " INNER JOIN k016db_2supplier_address" +
                                    " ON k016db_1supplier.cdkey = k016db_2supplier_address.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_2supplier_address.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_2supplier_address.txtsupplier_id" +

                                    " WHERE (k016db_1supplier.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k016db_1supplier.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (k016db_1supplier.txtsupplier_name LIKE '%" + this.PANEL161_SUP_txtsearch.Text.Trim() + "%')" +
                                    " AND (k016db_1supplier.txtsupplier_id <> '')" +
                                   " ORDER BY k016db_1supplier.txtsupplier_no ASC";

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
                            var index = PANEL161_SUP_dataGridView1.Rows.Add();
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_no"].Value = dt2.Rows[j]["txtsupplier_no"].ToString();      //1
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //2
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //3
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name_eng"].Value = dt2.Rows[j]["txtsupplier_name_eng"].ToString();      //4
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_status"].Value = dt2.Rows[j]["txtsupplier_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL161_SUP_Clear_GridView1_Up_Status();

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
        private void PANEL161_SUP_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL161_SUP_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL161_SUP.Height = PANEL161_SUP_btnresize_low.Top + e.Y;
                this.PANEL161_SUP.Width = PANEL161_SUP_btnresize_low.Left + e.X;
            }
        }
        private void PANEL161_SUP_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL161_SUP_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtsupplier Supplier  =======================================================================

        //txtproject โครงการ  =======================================================================
        private void PANEL1307_PROJECT_Fill_project()
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

            PANEL1307_PROJECT_Clear_GridView1_project();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_07project" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtproject_id <> '')" +
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
                            var index = PANEL1307_PROJECT_dataGridView1_project.Rows.Add();
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_id"].Value = dt2.Rows[j]["txtproject_id"].ToString();      //1
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name"].Value = dt2.Rows[j]["txtproject_name"].ToString();      //2
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name_eng"].Value = dt2.Rows[j]["txtproject_name_eng"].ToString();      //3
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
        private void PANEL1307_PROJECT_GridView1_project()
        {
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnCount = 4;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[0].Name = "Col_Auto_num";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].Name = "Col_txtproject_id";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].Name = "Col_txtproject_name";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].Name = "Col_txtproject_name_eng";

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[0].HeaderText = "No";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].HeaderText = "รหัส";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].HeaderText = " โครงการ ";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].HeaderText = " โครงการ  Eng";

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[0].Visible = false;  //"No";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].Visible = true;  //"Col_txtproject_id";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].Width = 100;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].ReadOnly = true;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].Visible = true;  //"Col_txtproject_name";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].Width = 150;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].ReadOnly = true;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].Visible = true;  //"Col_txtproject_name_eng";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].Width = 150;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].ReadOnly = true;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1307_PROJECT_dataGridView1_project.DefaultCellStyle.Font = new Font("Tahoma", 8F);

            this.PANEL1307_PROJECT_dataGridView1_project.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1307_PROJECT_dataGridView1_project.EnableHeadersVisualStyles = false;

        }
        private void PANEL1307_PROJECT_Clear_GridView1_project()
        {
            this.PANEL1307_PROJECT_dataGridView1_project.Rows.Clear();
            this.PANEL1307_PROJECT_dataGridView1_project.Refresh();
        }
        private void PANEL1307_PROJECT_txtproject_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1307_PROJECT.Visible == false)
                {
                    this.PANEL1307_PROJECT.Visible = true;
                    this.PANEL1307_PROJECT.Location = new Point(this.PANEL1307_PROJECT_txtproject_name.Location.X, this.PANEL1307_PROJECT_txtproject_name.Location.Y + 22);
                    this.PANEL1307_PROJECT_dataGridView1_project.Focus();
                }
                else
                {
                    this.PANEL1307_PROJECT.Visible = false;
                }
        }
        private void PANEL1307_PROJECT_btnproject_Click(object sender, EventArgs e)
        {
            if (this.PANEL1307_PROJECT.Visible == false)
            {
                this.PANEL1307_PROJECT.Visible = true;
                this.PANEL1307_PROJECT.BringToFront();
                this.PANEL1307_PROJECT.Location = new Point(this.txtcontact_person.Location.X+150, this.txtcontact_person.Location.Y+24 );

            }
            else
            {
                this.PANEL1307_PROJECT.Visible = false;
            }
        }
        private void PANEL1307_PROJECT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1307_PROJECT.Visible == false)
            {
                this.PANEL1307_PROJECT.Visible = true;
            }
            else
            {
                this.PANEL1307_PROJECT.Visible = false;
            }
        }
        private void PANEL1307_PROJECT_dataGridView1_project_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1307_PROJECT_dataGridView1_project.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1307_PROJECT_txtproject_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1307_PROJECT_txtproject_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL1307_PROJECT_dataGridView1_project_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1307_PROJECT_dataGridView1_project.CurrentRow.Index;

                this.PANEL1307_PROJECT_txtproject_id.Text = PANEL1307_PROJECT_dataGridView1_project.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1307_PROJECT_txtproject_name.Text = PANEL1307_PROJECT_dataGridView1_project.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1307_PROJECT_txtproject_name.Focus();
                this.PANEL1307_PROJECT.Visible = false;
            }
        }
        private void PANEL1307_PROJECT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1307_PROJECT_btn_search_Click(object sender, EventArgs e)
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

            PANEL1307_PROJECT_Clear_GridView1_project();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_09project" +
                                    " WHERE (txtproject_name LIKE '%" + this.PANEL1307_PROJECT_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtproject_id <> '')" +
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
                            var index = PANEL1307_PROJECT_dataGridView1_project.Rows.Add();
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_id"].Value = dt2.Rows[j]["txtproject_id"].ToString();      //1
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name"].Value = dt2.Rows[j]["txtproject_name"].ToString();      //2
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name_eng"].Value = dt2.Rows[j]["txtproject_name_eng"].ToString();      //3
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
        private void PANEL1307_PROJECT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1307_PROJECT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1307_PROJECT.Height = PANEL1307_PROJECT_btnresize_low.Top + e.Y;
                this.PANEL1307_PROJECT.Width = PANEL1307_PROJECT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1307_PROJECT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1307_PROJECT_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtproject โครงการ  =======================================================================

        //txtjob งาน  =======================================================================
        private void PANEL1317_JOB_Fill_job()
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

            PANEL1317_JOB_Clear_GridView1_job();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_17job" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                      " AND (txtjob_id <> '')" +
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
                            var index = PANEL1317_JOB_dataGridView1_job.Rows.Add();
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_txtjob_id"].Value = dt2.Rows[j]["txtjob_id"].ToString();      //1
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_txtjob_name"].Value = dt2.Rows[j]["txtjob_name"].ToString();      //2
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_txtjob_name_eng"].Value = dt2.Rows[j]["txtjob_name_eng"].ToString();      //3
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
        private void PANEL1317_JOB_GridView1_job()
        {
            this.PANEL1317_JOB_dataGridView1_job.ColumnCount = 4;
            this.PANEL1317_JOB_dataGridView1_job.Columns[0].Name = "Col_Auto_num";
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].Name = "Col_txtjob_id";
            this.PANEL1317_JOB_dataGridView1_job.Columns[2].Name = "Col_txtjob_name";
            this.PANEL1317_JOB_dataGridView1_job.Columns[3].Name = "Col_txtjob_name_eng";

            this.PANEL1317_JOB_dataGridView1_job.Columns[0].HeaderText = "No";
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].HeaderText = "รหัส";
            this.PANEL1317_JOB_dataGridView1_job.Columns[2].HeaderText = " งาน ";
            this.PANEL1317_JOB_dataGridView1_job.Columns[3].HeaderText = " งาน  Eng";

            this.PANEL1317_JOB_dataGridView1_job.Columns[0].Visible = false;  //"No";
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].Visible = true;  //"Col_txtjob_id";
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].Width = 100;
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].ReadOnly = true;
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1317_JOB_dataGridView1_job.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1317_JOB_dataGridView1_job.Columns[2].Visible = true;  //"Col_txtjob_name";
            this.PANEL1317_JOB_dataGridView1_job.Columns[2].Width = 150;
            this.PANEL1317_JOB_dataGridView1_job.Columns[2].ReadOnly = true;
            this.PANEL1317_JOB_dataGridView1_job.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1317_JOB_dataGridView1_job.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1317_JOB_dataGridView1_job.Columns[3].Visible = true;  //"Col_txtjob_name_eng";
            this.PANEL1317_JOB_dataGridView1_job.Columns[3].Width = 150;
            this.PANEL1317_JOB_dataGridView1_job.Columns[3].ReadOnly = true;
            this.PANEL1317_JOB_dataGridView1_job.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1317_JOB_dataGridView1_job.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1317_JOB_dataGridView1_job.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1317_JOB_dataGridView1_job.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1317_JOB_dataGridView1_job.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1317_JOB_dataGridView1_job.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1317_JOB_dataGridView1_job.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1317_JOB_dataGridView1_job.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1317_JOB_dataGridView1_job.EnableHeadersVisualStyles = false;

        }
        private void PANEL1317_JOB_Clear_GridView1_job()
        {
            this.PANEL1317_JOB_dataGridView1_job.Rows.Clear();
            this.PANEL1317_JOB_dataGridView1_job.Refresh();
        }
        private void PANEL1317_JOB_txtjob_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1317_JOB.Visible == false)
                {
                    this.PANEL1317_JOB.Visible = true;
                    this.PANEL1317_JOB.Location = new Point(this.PANEL1317_JOB_txtjob_name.Location.X, this.PANEL1317_JOB_txtjob_name.Location.Y + 22);
                    this.PANEL1317_JOB_dataGridView1_job.Focus();
                }
                else
                {
                    this.PANEL1317_JOB.Visible = false;
                }
        }
        private void PANEL1317_JOB_btnjob_Click(object sender, EventArgs e)
        {
            if (this.PANEL1317_JOB.Visible == false)
            {
                this.PANEL1317_JOB.Width = 502;
                this.PANEL1317_JOB.Height = 337;

                this.PANEL1317_JOB.Visible = true;
                this.PANEL1317_JOB.BringToFront();
                this.PANEL1317_JOB.Location = new Point(this.txtpo_remark.Location.X+150, this.txtpo_remark.Location.Y +24);
            }
            else
            {
                this.PANEL1317_JOB.Visible = false;
            }
        }
        private void PANEL1317_JOB_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1317_JOB.Visible == false)
            {
                this.PANEL1317_JOB.Visible = true;
            }
            else
            {
                this.PANEL1317_JOB.Visible = false;
            }
        }
        private void PANEL1317_JOB_dataGridView1_job_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1317_JOB_dataGridView1_job.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1317_JOB_txtjob_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1317_JOB_txtjob_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL1317_JOB_dataGridView1_job_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1317_JOB_dataGridView1_job.CurrentRow.Index;

                this.PANEL1317_JOB_txtjob_id.Text = PANEL1317_JOB_dataGridView1_job.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1317_JOB_txtjob_name.Text = PANEL1317_JOB_dataGridView1_job.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1317_JOB_txtjob_name.Focus();
                this.PANEL1317_JOB.Visible = false;
            }
        }
        private void PANEL1317_JOB_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1317_JOB_btn_search_Click(object sender, EventArgs e)
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

            PANEL1317_JOB_Clear_GridView1_job();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_17job" +
                                    " WHERE (txtjob_name LIKE '%" + this.PANEL1317_JOB_txtsearch.Text + "%')" +
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
                            var index = PANEL1317_JOB_dataGridView1_job.Rows.Add();
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_txtjob_id"].Value = dt2.Rows[j]["txtjob_id"].ToString();      //1
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_txtjob_name"].Value = dt2.Rows[j]["txtjob_name"].ToString();      //2
                            PANEL1317_JOB_dataGridView1_job.Rows[index].Cells["Col_txtjob_name_eng"].Value = dt2.Rows[j]["txtjob_name_eng"].ToString();      //3
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
        private void PANEL1317_JOB_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1317_JOB_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1317_JOB.Height = PANEL1317_JOB_btnresize_low.Top + e.Y;
                this.PANEL1317_JOB.Width = PANEL1317_JOB_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1317_JOB_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1317_JOB_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtjob งาน  =======================================================================


        //txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================
        private void PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax()
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

            PANEL1313_ACC_GROUP_TAX_Clear_GridView1_acc_group_tax();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_13group_tax" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtacc_group_tax_id <> '')" +
                                  " AND (txtacc_group_tax_status = 'P')" +  //เฉพาะกลุ่มซื้อ
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
                            var index = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows.Add();
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_id"].Value = dt2.Rows[j]["txtacc_group_tax_id"].ToString();      //1
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name"].Value = dt2.Rows[j]["txtacc_group_tax_name"].ToString();      //2
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name_eng"].Value = dt2.Rows[j]["txtacc_group_tax_name_eng"].ToString();      //3
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_vat_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtacc_group_tax_vat_rate"]).ToString("###,###.00");      //4
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
        private void PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax()
        {
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnCount = 5;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[0].Name = "Col_Auto_num";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].Name = "Col_txtacc_group_tax_id";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].Name = "Col_txtacc_group_tax_name";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].Name = "Col_txtacc_group_tax_name_eng";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].Name = "Col_txtacc_group_tax_vat_rate";

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[0].HeaderText = "No";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].HeaderText = "รหัส";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].HeaderText = " กลุ่มภาษี ";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].HeaderText = " กลุ่มภาษี  Eng";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].HeaderText = "อัตราภาษี";

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[0].Visible = false;  //"No";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].Visible = true;  //"Col_txtacc_group_tax_id";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].Width = 100;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].ReadOnly = true;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].Visible = true;  //"Col_txtacc_group_tax_name";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].Width = 100;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].ReadOnly = true;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].Visible = false;  //"Col_txtacc_group_tax_name_eng";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].Width = 0;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].ReadOnly = false;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].Visible = true;  //"Col_txtacc_group_tax_vat_rate";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].Width = 100;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].ReadOnly = true;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;


            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.EnableHeadersVisualStyles = false;

        }
        private void PANEL1313_ACC_GROUP_TAX_Clear_GridView1_acc_group_tax()
        {
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows.Clear();
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Refresh();
        }
        private void PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1313_ACC_GROUP_TAX.Visible == false)
                {
                    this.PANEL1313_ACC_GROUP_TAX.Visible = true;
                    this.PANEL1313_ACC_GROUP_TAX.Location = new Point(this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.X, this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.Y + 22);
                    this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Focus();
                }
                else
                {
                    this.PANEL1313_ACC_GROUP_TAX.Visible = false;
                }
        }
        private void PANEL1313_ACC_GROUP_TAX_btnacc_group_tax_Click(object sender, EventArgs e)
        {
            if (this.PANEL1313_ACC_GROUP_TAX.Visible == false)
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = true;
                this.PANEL1313_ACC_GROUP_TAX.BringToFront();
                this.PANEL1313_ACC_GROUP_TAX.Location = new Point(this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.X - PANEL1313_ACC_GROUP_TAX.Height - 53, this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.Y - PANEL1313_ACC_GROUP_TAX.Height - 2);
            }
            else
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = false;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1313_ACC_GROUP_TAX.Visible == false)
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = true;
            }
            else
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = false;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = row.Cells[2].Value.ToString();
                    this.txtvat_rate.Text = row.Cells[4].Value.ToString();
                    Sum_group_tax();
                }
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.CurrentRow.Index;

                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Focus();
                this.PANEL1313_ACC_GROUP_TAX.Visible = false;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1313_ACC_GROUP_TAX_btn_search_Click(object sender, EventArgs e)
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

            PANEL1313_ACC_GROUP_TAX_Clear_GridView1_acc_group_tax();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_13group_tax" +
                                    " WHERE (txtacc_group_tax_name LIKE '%" + this.PANEL1313_ACC_GROUP_TAX_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtacc_group_tax_status = 'P')" +  //เฉพาะกลุ่มซื้อ
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
                            var index = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows.Add();
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_id"].Value = dt2.Rows[j]["txtacc_group_tax_id"].ToString();      //1
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name"].Value = dt2.Rows[j]["txtacc_group_tax_name"].ToString();      //2
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name_eng"].Value = dt2.Rows[j]["txtacc_group_tax_name_eng"].ToString();      //3
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_vat_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtacc_group_tax_vat_rate"]).ToString("###,###.00");      //4
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
        private void PANEL1313_ACC_GROUP_TAX_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1313_ACC_GROUP_TAX_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1313_ACC_GROUP_TAX.Height = PANEL1313_ACC_GROUP_TAX_btnresize_low.Top + e.Y;
                this.PANEL1313_ACC_GROUP_TAX.Width = PANEL1313_ACC_GROUP_TAX_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1313_ACC_GROUP_TAX_btnnew_Click(object sender, EventArgs e)
        {

        }



        //END txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================





        //2. ส่วนเลือกรายการสินค้า =========================================================================================================================
        //MAT=====================================================================================================================================

        //PANEL_MAT====================================================
        private void PANEL_MAT_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void PANEL_MAT_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_btnclose_Click(object sender, EventArgs e)
        {
            this.PANEL_MAT.Visible = false;
            this.BtnSave.Enabled = true;
        }
        private void PANEL_MAT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL_MAT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_MAT.Height = PANEL_MAT_btnresize_low.Top + e.Y;
                this.PANEL_MAT.Width = PANEL_MAT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_MAT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }
        //END PANEL_MAT====================================================


        private void PANEL_MAT_btnGo_Click(object sender, EventArgs e)
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4

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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " AND (b001mat_02detail.txtmat_type_id = '" + this.PANEL101_MAT_TYPE2_txtmat_type_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo2_Click(object sender, EventArgs e)
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4

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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " AND (b001mat_02detail.txtmat_group_id = '" + this.PANEL103_MAT_GROUP2_txtmat_group_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo3_Click(object sender, EventArgs e)
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4

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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                if (this.cboSearch.Text.Trim() == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                        "b001mat_06price_sale.*," +
                                        "b001_05mat_unit1.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                          " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                                " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtsearch.Text.Trim() + "')" +
                                       " ORDER BY b001mat.txtmat_no ASC";

                }
                else if (this.cboSearch.Text.Trim() == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                        "b001mat_06price_sale.*," +
                                        "b001_05mat_unit1.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                          " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                                " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (b001mat.txtmat_name LIKE '%" + this.PANEL_MAT_txtsearch.Text.Trim() + "%')" +
                                       " ORDER BY b001mat.txtmat_no ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                        "b001_05mat_unit1.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                            " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //" AND (b001mat.txtmat_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                       " ORDER BY b001mat.txtmat_no ASC";

                }
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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else

                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo4_Click(object sender, EventArgs e)
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4

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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " AND (b001mat_02detail.txtmat_sac_id = '" + this.PANEL102_MAT_SAC2_txtmat_sac_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();


        }

        private void PANEL_MAT_btnGo5_Click(object sender, EventArgs e)
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4

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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " AND (b001mat_02detail.txtmat_brand_id = '" + this.PANEL104_MAT_BRAND2_txtmat_brand_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo6_Click(object sender, EventArgs e)
        {
            //=================================================================================
            string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4

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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001_09bom_detail.*," +
                                    "b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001_09bom_detail" +

                                    " INNER JOIN b001mat" +
                                    " ON b001_09bom_detail.cdkey = b001mat.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat.txtmat_id" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001_09bom_detail.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001mat_06price_sale" +
                                    " ON b001_09bom_detail.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " WHERE (b001_09bom_detail.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001_09bom_detail.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001_09bom_detail.txtbom_id = '" + this.PANEL109_BOM_txtbom_id.Text.Trim() + "')" +
                                    " ORDER BY b001_09bom_detail.ID ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = D1;      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnupdate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString())) > 0)
                {
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[9].Value == null)
                    {
                        MessageBox.Show("โปรด ใส่วันที่ต้องการสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            UPDATE_TO_GridView1();
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

            PANEL_MAT_Clear_GridView1();
            this.PANEL_MAT.Visible = false;
            this.BtnSave.Enabled = true;
        }


        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle;
        int curRow = 0;
        private void Fill_PANEL_MAT_GridView1()
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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
        private void UPDATE_TO_GridView1()
        {
            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString())) > 0)
                {
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_date"].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_date"].Value = "";
                        MessageBox.Show("โปรด ใส่วันที่ต้องการสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_date"].Value.ToString() == "")
                    {
                        MessageBox.Show("โปรด ใส่วันที่ต้องการสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
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
                    GridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[1].Value.ToString(); //1
                    GridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[2].Value.ToString(); //2
                    GridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[3].Value.ToString(); //3
                    GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[4].Value.ToString(); //4
                    GridView1.Rows[index].Cells["Col_txtqty"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString(); //5
                    GridView1.Rows[index].Cells["Col_txtprice"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString(); //6
                    GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value.ToString(); //7
                    GridView1.Rows[index].Cells["Col_txtsum_total"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value.ToString(); //8
                    GridView1.Rows[index].Cells["Col_date"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[9].Value.ToString(); //9

                }
            }
        }
        private void PANEL_MAT_Show_GridView1()
        {
            this.PANEL_MAT_GridView1.ColumnCount = 10;
            this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
            this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
            this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
            this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
            this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
            this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

            this.PANEL_MAT_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_MAT_GridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL_MAT_GridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL_MAT_GridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.PANEL_MAT_GridView1.Columns[4].HeaderText = " หน่วยนับ";
            this.PANEL_MAT_GridView1.Columns[5].HeaderText = " จำนวน";
            this.PANEL_MAT_GridView1.Columns[6].HeaderText = " ราคา/หน่วย(บาท)";
            this.PANEL_MAT_GridView1.Columns[7].HeaderText = " ส่วนลด(บาท)";
            this.PANEL_MAT_GridView1.Columns[8].HeaderText = " จำนวนเงิน(บาท)";
            this.PANEL_MAT_GridView1.Columns[9].HeaderText = " วันที่ต้องการสินค้า";

            this.PANEL_MAT_GridView1.Columns[0].Visible = true;  //"Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[0].Width = 36;
            this.PANEL_MAT_GridView1.Columns[0].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns[1].Visible = true;  //"Col_txtmat_no";

            this.PANEL_MAT_GridView1.Columns[2].Visible = true;  //"Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns[2].Width = 100;
            this.PANEL_MAT_GridView1.Columns[2].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns[3].Visible = true;  //"Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns[3].Width = 150;
            this.PANEL_MAT_GridView1.Columns[3].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL_MAT_GridView1.Columns[4].Visible = true;  //"Col_txtmat_unit1_name";
            this.PANEL_MAT_GridView1.Columns[4].Width = 100;
            this.PANEL_MAT_GridView1.Columns[4].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns[5].Visible = true;  //"Col_txtqty";
            this.PANEL_MAT_GridView1.Columns[5].Width = 100;
            this.PANEL_MAT_GridView1.Columns[5].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[6].Visible = true;  //"Col_txtprice";
            this.PANEL_MAT_GridView1.Columns[6].Width = 100;
            this.PANEL_MAT_GridView1.Columns[6].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[7].Visible = true;  //"Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[7].Width = 100;
            this.PANEL_MAT_GridView1.Columns[7].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[8].Visible = true;  //"Col_txtsum_total";
            this.PANEL_MAT_GridView1.Columns[8].Width = 150;
            this.PANEL_MAT_GridView1.Columns[8].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[9].Visible = true;  //"Col_date";
            this.PANEL_MAT_GridView1.Columns[9].Width = 150;
            this.PANEL_MAT_GridView1.Columns[9].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.PANEL_MAT_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_MAT_GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_MAT_GridView1.EnableHeadersVisualStyles = false;

        }
        private void PANEL_MAT_Clear_GridView1()
        {
            if (this.PANEL_MAT_GridView1.Rows.Count > 0)
            {
                this.PANEL_MAT_GridView1.Rows.Clear();
                this.PANEL_MAT_GridView1.Refresh();
            }
        }
        private void PANEL_MAT_GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            switch (PANEL_MAT_GridView1.Columns[e.ColumnIndex].Name)
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

                    _Rectangle = PANEL_MAT_GridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                    dtp.Size = new Size(_Rectangle.Width, _Rectangle.Height); //  
                    dtp.Location = new Point(_Rectangle.X, _Rectangle.Y); //  

                    if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[curRow].Cells[5].Value.ToString())) > 0)
                    {
                        PANEL_MAT_GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
                    }

                    dtp.Visible = true;
                    break;
            }
        }
        private void PANEL_MAT_GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow = PANEL_MAT_GridView1.CurrentRow.Index;
            int rowscount = PANEL_MAT_GridView1.Rows.Count;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();

            //if (this.PANEL_MAT_GridView1.Rows.Count > 0)
            //{
            //    //===============================================================
            //    for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 1; i++)
            //    {

            //        if (PANEL_MAT_GridView1.Rows[i].Cells[3].Value != null)
            //        {
            //            if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
            //            {
            //                this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value = ".00";
            //            }

            //            if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
            //            {
            //                PANEL_MAT_GridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[1].Style.Font = new Font("Tahoma", 12F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[2].Style.Font = new Font("Tahoma", 12F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
            //                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.Font = new Font("Tahoma", 12F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[4].Style.Font = new Font("Tahoma", 12F);

            //                //PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[5].Style.Font = new Font("Tahoma", 12F);


            //                PANEL_MAT_GridView1.Rows[i].Cells[6].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[6].Style.Font = new Font("Tahoma", 12F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[7].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[7].Style.Font = new Font("Tahoma", 12F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[8].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[8].Style.Font = new Font("Tahoma", 12F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[9].Style.Font = new Font("Tahoma", 12F);

            //            }
            //            else
            //            {
            //                PANEL_MAT_GridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[1].Style.Font = new Font("Tahoma", 8F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[2].Style.Font = new Font("Tahoma", 8F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
            //                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.Font = new Font("Tahoma", 8F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[4].Style.Font = new Font("Tahoma", 8F);

            //                //PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[5].Style.Font = new Font("Tahoma", 8F);


            //                PANEL_MAT_GridView1.Rows[i].Cells[6].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[6].Style.Font = new Font("Tahoma", 8F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[7].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[7].Style.Font = new Font("Tahoma", 8F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[8].Style.BackColor = Color.White;
            //                PANEL_MAT_GridView1.Rows[i].Cells[8].Style.Font = new Font("Tahoma", 8F);

            //                PANEL_MAT_GridView1.Rows[i].Cells[9].Style.Font = new Font("Tahoma", 8F);
            //            }


            //        }
            //    }
            //}
            ////===============================================================

            //if (PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.BackColor == Color.LightGoldenrodYellow)
            //{

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.Font = new Font("Tahoma", 8F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.Font = new Font("Tahoma", 8F);


            //    PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.ForeColor = Color.Black;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.Font = new Font("Tahoma", 8F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.Font = new Font("Tahoma", 8F);

            //    //PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.Font = new Font("Tahoma", 8F);


            //    PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.Font = new Font("Tahoma", 8F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.Font = new Font("Tahoma", 8F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.Font = new Font("Tahoma", 8F);

            //    //PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.BackColor = Color.White;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.Font = new Font("Tahoma", 8F);
            //}
            //else
            //{
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.Font = new Font("Tahoma", 12F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.Font = new Font("Tahoma", 12F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.ForeColor = Color.Red;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.Font = new Font("Tahoma", 12F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.Font = new Font("Tahoma", 12F);

            //    //PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.Font = new Font("Tahoma", 12F);


            //    PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.Font = new Font("Tahoma", 12F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.Font = new Font("Tahoma", 12F);

            //    PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.Font = new Font("Tahoma", 12F);

            //    //PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
            //    PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.Font = new Font("Tahoma", 12F);
            //}
            ////======================================



        }
        private void PANEL_MAT_GridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtp.Visible = false;
        }
        private void PANEL_MAT_GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            dtp.Visible = false;
        }
        private void PANEL_MAT_GridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt = e.Control as TextBox;
            txt.PreviewKeyDown += new PreviewKeyDownEventHandler(txt_PreviewKeyDown);
        }
        private void PANEL_MAT_GridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PANEL_MAT_GridView1_Cal_Sum();
            }
        }
        private void PANEL_MAT_GridView1_KeyUp(object sender, KeyEventArgs e)
        {
            PANEL_MAT_GridView1_Cal_Sum();
        }
        private void dtp_TextChange(object sender, EventArgs e)
        {
            //PANEL_MAT_GridView1.CurrentCell.Value = dtp.Value.ToString("dd-MM-yyyy", UsaCulture);
            PANEL_MAT_GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
        }
        void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                PANEL_MAT_GridView1_Cal_Sum();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }
        void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                PANEL_MAT_GridView1_Cal_Sum();
            }
        }
        private void PANEL_MAT_GridView1_Color_Column()
        {

            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 1; i++)
            {
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[5].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
                //}
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[9].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
                //}

                PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightSkyBlue;
                PANEL_MAT_GridView1.Rows[i].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightSkyBlue;

            }
        }
        private void PANEL_MAT_GridView1_Cal_Sum()
        {
            double Sum_Total = 0;
            double Sum_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            int k = 0;

            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.PANEL_MAT_GridView1.Rows[i].Cells[2].Value.ToString();

                if (valu != "")
                {
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[0].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[0].Value = k.ToString();
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value = "0";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value = "0";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value = "0";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value = "0";
                    }


                    //5 * 6 = 8  
                    this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value = Convert.ToSingle(this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value).ToString("###,###.00");     //5
                    this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value = Convert.ToSingle(this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value).ToString("###,###.00");     //6
                    this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value = Convert.ToSingle(this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value).ToString("###,###.00");     //7
                    this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value = Convert.ToSingle(this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value).ToString("###,###.00");     //8

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString()));
                    this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString()));
                        this.PANEL_MAT_txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString()));
                        this.PANEL_MAT_txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value.ToString()));
                        this.PANEL_MAT_txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value.ToString()));
                        this.PANEL_MAT_txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }
                }
            }

            this.PANEL_MAT_txtcount_rows.Text = k.ToString();

            Sum_Total = 0;
            Sum_Qty = 0;
            Sum_Price = 0;
            Sum_Discount = 0;
            MoneySum = 0;

        }

        //txtmat_type ประเภทสินค้า =======================================================================
        private void PANEL101_MAT_TYPE2_Fill_mat_type()
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

            PANEL101_MAT_TYPE2_Clear_GridView1_mat_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_01mat_type" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtmat_type_id <> '')" +
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
                            var index = PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows.Add();
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_id"].Value = dt2.Rows[j]["txtmat_type_id"].ToString();      //1
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name"].Value = dt2.Rows[j]["txtmat_type_name"].ToString();      //2
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name_eng"].Value = dt2.Rows[j]["txtmat_type_name_eng"].ToString();      //3
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
        private void PANEL101_MAT_TYPE2_GridView1_mat_type()
        {
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnCount = 4;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].Name = "Col_txtmat_type_id";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].Name = "Col_txtmat_type_name";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].Name = "Col_txtmat_type_name_eng";

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[0].HeaderText = "No";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].HeaderText = "รหัส";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].HeaderText = " ประเภทสินค้า";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].HeaderText = " ประเภทสินค้า Eng";

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[0].Visible = false;  //"No";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].Visible = true;  //"Col_txtmat_type_id";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].Width = 100;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].ReadOnly = true;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].Visible = true;  //"Col_txtmat_type_name";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].Width = 150;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].ReadOnly = true;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].Visible = true;  //"Col_txtmat_type_name_eng";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].Width = 150;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].ReadOnly = true;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL101_MAT_TYPE2_Clear_GridView1_mat_type()
        {
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows.Clear();
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Refresh();
        }
        private void PANEL101_MAT_TYPE2_txtmat_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL101_MAT_TYPE2.Visible == false)
                {
                    this.PANEL101_MAT_TYPE2.Visible = true;
                    this.PANEL101_MAT_TYPE2.Location = new Point(this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.X, this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.Y + 22);
                    this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Focus();
                }
                else
                {
                    this.PANEL101_MAT_TYPE2.Visible = false;
                }
        }
        private void PANEL101_MAT_TYPE2_btnmat_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL101_MAT_TYPE2.Visible == false)
            {
                this.PANEL101_MAT_TYPE2.Visible = true;
                this.PANEL101_MAT_TYPE2.BringToFront();
                this.PANEL101_MAT_TYPE2.Location = new Point(this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.X, this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL101_MAT_TYPE2.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL101_MAT_TYPE2.Visible == false)
            {
                this.PANEL101_MAT_TYPE2.Visible = true;
            }
            else
            {
                this.PANEL101_MAT_TYPE2.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE2_dataGridView1_mat_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL101_MAT_TYPE2_txtmat_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL101_MAT_TYPE2_txtmat_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL101_MAT_TYPE2_dataGridView1_mat_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL101_MAT_TYPE2_dataGridView1_mat_type.CurrentRow.Index;

                this.PANEL101_MAT_TYPE2_txtmat_type_id.Text = PANEL101_MAT_TYPE2_dataGridView1_mat_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL101_MAT_TYPE2_txtmat_type_name.Text = PANEL101_MAT_TYPE2_dataGridView1_mat_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL101_MAT_TYPE2_txtmat_type_name.Focus();
                this.PANEL101_MAT_TYPE2.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL101_MAT_TYPE2_btn_search_Click(object sender, EventArgs e)
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

            PANEL101_MAT_TYPE2_Clear_GridView1_mat_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_01mat_type" +
                                   " WHERE (txtmat_type_name LIKE '%" + this.PANEL101_MAT_TYPE2_txtsearch.Text + "%')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows.Add();
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_id"].Value = dt2.Rows[j]["txtmat_type_id"].ToString();      //1
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name"].Value = dt2.Rows[j]["txtmat_type_name"].ToString();      //2
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name_eng"].Value = dt2.Rows[j]["txtmat_type_name_eng"].ToString();      //3
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
        private void PANEL101_MAT_TYPE2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL101_MAT_TYPE2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL101_MAT_TYPE2.Height = PANEL101_MAT_TYPE2_btnresize_low.Top + e.Y;
                this.PANEL101_MAT_TYPE2.Width = PANEL101_MAT_TYPE2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL101_MAT_TYPE2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL101_MAT_TYPE2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_type ประเภทสินค้า =======================================================================

        //txtmat_sac หมวดสินค้า =======================================================================
        private void PANEL102_MAT_SAC2_Fill_mat_sac()
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

            PANEL102_MAT_SAC2_Clear_GridView1_mat_sac();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_02mat_sac" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (txtmat_sac_id <> '')" +
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
                            var index = PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows.Add();
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_id"].Value = dt2.Rows[j]["txtmat_sac_id"].ToString();      //1
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name"].Value = dt2.Rows[j]["txtmat_sac_name"].ToString();      //2
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name_eng"].Value = dt2.Rows[j]["txtmat_sac_name_eng"].ToString();      //3
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
        private void PANEL102_MAT_SAC2_GridView1_mat_sac()
        {
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnCount = 4;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[0].Name = "Col_Auto_num";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].Name = "Col_txtmat_sac_id";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].Name = "Col_txtmat_sac_name";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].Name = "Col_txtmat_sac_name_eng";

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[0].HeaderText = "No";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].HeaderText = "รหัส";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].HeaderText = " หมวดสินค้า";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].HeaderText = " หมวดสินค้า Eng";

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[0].Visible = false;  //"No";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].Visible = true;  //"Col_txtmat_sac_id";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].Width = 100;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].ReadOnly = true;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].Visible = true;  //"Col_txtmat_sac_name";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].Width = 150;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].ReadOnly = true;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].Visible = true;  //"Col_txtmat_sac_name_eng";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].Width = 150;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].ReadOnly = true;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.EnableHeadersVisualStyles = false;

        }
        private void PANEL102_MAT_SAC2_Clear_GridView1_mat_sac()
        {
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows.Clear();
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Refresh();
        }
        private void PANEL102_MAT_SAC2_txtmat_sac_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL102_MAT_SAC2.Visible == false)
                {
                    this.PANEL102_MAT_SAC2.Visible = true;
                    this.PANEL102_MAT_SAC2.Location = new Point(this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.X, this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.Y + 22);
                    this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Focus();
                }
                else
                {
                    this.PANEL102_MAT_SAC2.Visible = false;
                }
        }
        private void PANEL102_MAT_SAC2_btnmat_sac_Click(object sender, EventArgs e)
        {
            if (this.PANEL102_MAT_SAC2.Visible == false)
            {
                this.PANEL102_MAT_SAC2.Visible = true;
                this.PANEL102_MAT_SAC2.BringToFront();
                this.PANEL102_MAT_SAC2.Location = new Point(this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.X, this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.Y + 22);
            }
            else
            {
                this.PANEL102_MAT_SAC2.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL102_MAT_SAC2.Visible == false)
            {
                this.PANEL102_MAT_SAC2.Visible = true;
            }
            else
            {
                this.PANEL102_MAT_SAC2.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC2_dataGridView1_mat_sac_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL102_MAT_SAC2_txtmat_sac_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL102_MAT_SAC2_txtmat_sac_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL102_MAT_SAC2_dataGridView1_mat_sac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL102_MAT_SAC2_dataGridView1_mat_sac.CurrentRow.Index;

                this.PANEL102_MAT_SAC2_txtmat_sac_id.Text = PANEL102_MAT_SAC2_dataGridView1_mat_sac.CurrentRow.Cells[1].Value.ToString();
                this.PANEL102_MAT_SAC2_txtmat_sac_name.Text = PANEL102_MAT_SAC2_dataGridView1_mat_sac.CurrentRow.Cells[2].Value.ToString();
                this.PANEL102_MAT_SAC2_txtmat_sac_name.Focus();
                this.PANEL102_MAT_SAC2.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL102_MAT_SAC2_btn_search_Click(object sender, EventArgs e)
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

            PANEL102_MAT_SAC2_Clear_GridView1_mat_sac();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_02mat_sac" +
                                   " WHERE (txtmat_sac_name LIKE '%" + this.PANEL102_MAT_SAC2_txtsearch.Text + "%')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows.Add();
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_id"].Value = dt2.Rows[j]["txtmat_sac_id"].ToString();      //1
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name"].Value = dt2.Rows[j]["txtmat_sac_name"].ToString();      //2
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name_eng"].Value = dt2.Rows[j]["txtmat_sac_name_eng"].ToString();      //3
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
        private void PANEL102_MAT_SAC2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL102_MAT_SAC2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL102_MAT_SAC2.Height = PANEL102_MAT_SAC2_btnresize_low.Top + e.Y;
                this.PANEL102_MAT_SAC2.Width = PANEL102_MAT_SAC2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL102_MAT_SAC2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL102_MAT_SAC2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_sac หมวดสินค้า =======================================================================

        //txtmat_group กลุ่มสินค้า =======================================================================
        private void PANEL103_MAT_GROUP2_Fill_mat_group()
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

            PANEL103_MAT_GROUP2_Clear_GridView1_mat_group();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_03mat_group" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                      " AND (txtmat_group_id <> '')" +
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
                            var index = PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows.Add();
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_id"].Value = dt2.Rows[j]["txtmat_group_id"].ToString();      //1
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name"].Value = dt2.Rows[j]["txtmat_group_name"].ToString();      //2
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name_eng"].Value = dt2.Rows[j]["txtmat_group_name_eng"].ToString();      //3
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
        private void PANEL103_MAT_GROUP2_GridView1_mat_group()
        {
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnCount = 4;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[0].Name = "Col_Auto_num";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].Name = "Col_txtmat_group_id";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].Name = "Col_txtmat_group_name";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].Name = "Col_txtmat_group_name_eng";

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[0].HeaderText = "No";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].HeaderText = "รหัส";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].HeaderText = " กลุ่มสินค้า";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].HeaderText = " กลุ่มสินค้า Eng";

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[0].Visible = false;  //"No";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].Visible = true;  //"Col_txtmat_group_id";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].Width = 100;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].ReadOnly = true;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].Visible = true;  //"Col_txtmat_group_name";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].Width = 150;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].ReadOnly = true;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].Visible = true;  //"Col_txtmat_group_name_eng";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].Width = 150;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].ReadOnly = true;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.EnableHeadersVisualStyles = false;

        }
        private void PANEL103_MAT_GROUP2_Clear_GridView1_mat_group()
        {
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows.Clear();
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Refresh();
        }
        private void PANEL103_MAT_GROUP2_txtmat_group_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL103_MAT_GROUP2.Visible == false)
                {
                    this.PANEL103_MAT_GROUP2.Visible = true;
                    this.PANEL103_MAT_GROUP2.Location = new Point(this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.X, this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.Y + 22);
                    this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Focus();
                }
                else
                {
                    this.PANEL103_MAT_GROUP2.Visible = false;
                }
        }
        private void PANEL103_MAT_GROUP2_btnmat_group_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_MAT_GROUP2.Visible == false)
            {
                this.PANEL103_MAT_GROUP2.Visible = true;
                this.PANEL103_MAT_GROUP2.BringToFront();
                this.PANEL103_MAT_GROUP2.Location = new Point(this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.X, this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.Y + 22);
            }
            else
            {
                this.PANEL103_MAT_GROUP2.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_MAT_GROUP2.Visible == false)
            {
                this.PANEL103_MAT_GROUP2.Visible = true;
            }
            else
            {
                this.PANEL103_MAT_GROUP2.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP2_dataGridView1_mat_group_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL103_MAT_GROUP2_txtmat_group_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL103_MAT_GROUP2_txtmat_group_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL103_MAT_GROUP2_dataGridView1_mat_group_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL103_MAT_GROUP2_dataGridView1_mat_group.CurrentRow.Index;

                this.PANEL103_MAT_GROUP2_txtmat_group_id.Text = PANEL103_MAT_GROUP2_dataGridView1_mat_group.CurrentRow.Cells[1].Value.ToString();
                this.PANEL103_MAT_GROUP2_txtmat_group_name.Text = PANEL103_MAT_GROUP2_dataGridView1_mat_group.CurrentRow.Cells[2].Value.ToString();
                this.PANEL103_MAT_GROUP2_txtmat_group_name.Focus();
                this.PANEL103_MAT_GROUP2.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL103_MAT_GROUP2_btn_search_Click(object sender, EventArgs e)
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

            PANEL103_MAT_GROUP2_Clear_GridView1_mat_group();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_03mat_group" +
                                   " WHERE (txtmat_group_name LIKE '%" + this.PANEL103_MAT_GROUP2_txtsearch.Text + "%')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows.Add();
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_id"].Value = dt2.Rows[j]["txtmat_group_id"].ToString();      //1
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name"].Value = dt2.Rows[j]["txtmat_group_name"].ToString();      //2
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name_eng"].Value = dt2.Rows[j]["txtmat_group_name_eng"].ToString();      //3
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

                //===============================
            }
            //================================

        }
        private void PANEL103_MAT_GROUP2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL103_MAT_GROUP2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL103_MAT_GROUP2.Height = PANEL103_MAT_GROUP2_btnresize_low.Top + e.Y;
                this.PANEL103_MAT_GROUP2.Width = PANEL103_MAT_GROUP2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL103_MAT_GROUP2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL103_MAT_GROUP2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_group กลุ่มสินค้า =======================================================================

        //txtmat_brand =======================================================================
        private void PANEL104_MAT_BRAND2_Fill_mat_brand()
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

            PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04mat_brand" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (txtmat_brand_id <> '')" +
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
                            var index = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND2_GridView1_mat_brand()
        {
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnCount = 4;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[0].Name = "Col_Auto_num";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].Name = "Col_txtmat_brand_id";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].Name = "Col_txtmat_brand_name";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].Name = "Col_txtmat_brand_name_eng";

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[0].HeaderText = "No";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].HeaderText = "รหัส";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].HeaderText = " กลุ่มสินค้า";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].HeaderText = " กลุ่มสินค้า Eng";

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[0].Visible = false;  //"No";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].Visible = true;  //"Col_txt mat_brand_id";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].Width = 100;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].ReadOnly = true;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].Visible = true;  //"Col_txt mat_brand_name";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].Width = 150;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].ReadOnly = true;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].Visible = true;  //"Col_txt mat_brand_name_eng";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].Width = 150;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].ReadOnly = true;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.EnableHeadersVisualStyles = false;

        }
        private void PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand()
        {
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Clear();
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Refresh();
        }
        private void PANEL104_MAT_BRAND2_txtmat_brand_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL104_MAT_BRAND2.Visible == false)
                {
                    this.PANEL104_MAT_BRAND2.Visible = true;
                    this.PANEL104_MAT_BRAND2.Location = new Point(this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.X, this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.Y + 22);
                    this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Focus();
                }
                else
                {
                    this.PANEL104_MAT_BRAND2.Visible = false;
                }
        }
        private void PANEL104_MAT_BRAND2_btnmat_brand_Click(object sender, EventArgs e)
        {
            if (this.PANEL104_MAT_BRAND2.Visible == false)
            {
                this.PANEL104_MAT_BRAND2.Visible = true;
                this.PANEL104_MAT_BRAND2.BringToFront();
                this.PANEL104_MAT_BRAND2.Location = new Point(this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.X, this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.Y + 22);
            }
            else
            {
                this.PANEL104_MAT_BRAND2.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL104_MAT_BRAND2.Visible == false)
            {
                this.PANEL104_MAT_BRAND2.Visible = true;
            }
            else
            {
                this.PANEL104_MAT_BRAND2.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND2_dataGridView1_mat_brand_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL104_MAT_BRAND2_txtmat_brand_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL104_MAT_BRAND2_txtmat_brand_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL104_MAT_BRAND2_dataGridView1_mat_brand_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.CurrentRow.Index;

                this.PANEL104_MAT_BRAND2_txtmat_brand_id.Text = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.CurrentRow.Cells[1].Value.ToString();
                this.PANEL104_MAT_BRAND2_txtmat_brand_name.Text = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.CurrentRow.Cells[2].Value.ToString();
                this.PANEL104_MAT_BRAND2_txtmat_brand_name.Focus();
                this.PANEL104_MAT_BRAND2.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
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

            PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04mat_brand" +
                                  " WHERE (txtmat_brand_name LIKE '%" + this.PANEL104_MAT_BRAND2_txtsearch.Text.ToString() + "%')" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND2_btn_search_Click(object sender, EventArgs e)
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

            PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04 mat_brand" +
                                   " WHERE (txt mat_brand_name LIKE '%" + this.PANEL104_MAT_BRAND2_txtsearch.Text + "%')" +
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
                            var index = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_id"].Value = dt2.Rows[j]["txt mat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_name"].Value = dt2.Rows[j]["txt mat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_name_eng"].Value = dt2.Rows[j]["txt mat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL104_MAT_BRAND2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL104_MAT_BRAND2.Height = PANEL104_MAT_BRAND2_btnresize_low.Top + e.Y;
                this.PANEL104_MAT_BRAND2.Width = PANEL104_MAT_BRAND2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL104_MAT_BRAND2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL104_MAT_BRAND2_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END txtmat_brand=======================================================================
        //txtbom ชื่อ BOM =======================================================================
        private void PANEL109_BOM_Fill_bom()
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

            PANEL109_BOM_Clear_GridView1_bom();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_09bom" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbom_id <> '')" +
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
                            var index = PANEL109_BOM_dataGridView1_bom.Rows.Add();
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_id"].Value = dt2.Rows[j]["txtbom_id"].ToString();      //1
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name"].Value = dt2.Rows[j]["txtbom_name"].ToString();      //2
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name_eng"].Value = dt2.Rows[j]["txtbom_name_eng"].ToString();      //3
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
        private void PANEL109_BOM_GridView1_bom()
        {
            this.PANEL109_BOM_dataGridView1_bom.ColumnCount = 4;
            this.PANEL109_BOM_dataGridView1_bom.Columns[0].Name = "Col_Auto_num";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].Name = "Col_txtbom_id";
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].Name = "Col_txtbom_name";
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].Name = "Col_txtbom_name_eng";

            this.PANEL109_BOM_dataGridView1_bom.Columns[0].HeaderText = "No";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].HeaderText = "รหัส";
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].HeaderText = " ชื่อ BOM";
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].HeaderText = " ชื่อ BOM Eng";

            this.PANEL109_BOM_dataGridView1_bom.Columns[0].Visible = false;  //"No";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].Visible = true;  //"Col_txtbom_id";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].Width = 100;
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].ReadOnly = true;
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL109_BOM_dataGridView1_bom.Columns[2].Visible = true;  //"Col_txtbom_name";
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].Width = 150;
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].ReadOnly = true;
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL109_BOM_dataGridView1_bom.Columns[3].Visible = true;  //"Col_txtbom_name_eng";
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].Width = 150;
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].ReadOnly = true;
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL109_BOM_dataGridView1_bom.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL109_BOM_dataGridView1_bom.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL109_BOM_dataGridView1_bom.EnableHeadersVisualStyles = false;

        }
        private void PANEL109_BOM_Clear_GridView1_bom()
        {
            this.PANEL109_BOM_dataGridView1_bom.Rows.Clear();
            this.PANEL109_BOM_dataGridView1_bom.Refresh();
        }
        private void PANEL109_BOM_txtbom_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL109_BOM.Visible == false)
                {
                    this.PANEL109_BOM.Visible = true;
                    this.PANEL109_BOM.Location = new Point(this.PANEL109_BOM_txtbom_name.Location.X, this.PANEL109_BOM_txtbom_name.Location.Y + 22);
                    this.PANEL109_BOM_dataGridView1_bom.Focus();
                }
                else
                {
                    this.PANEL109_BOM.Visible = false;
                }
        }
        private void PANEL109_BOM_btnbom_Click(object sender, EventArgs e)
        {
            if (this.PANEL109_BOM.Visible == false)
            {
                this.PANEL109_BOM.Visible = true;
                this.PANEL109_BOM.BringToFront();
                this.PANEL109_BOM.Location = new Point(this.PANEL109_BOM_txtbom_name.Location.X, this.PANEL109_BOM_txtbom_name.Location.Y + 22);
            }
            else
            {
                this.PANEL109_BOM.Visible = false;
            }
        }
        private void PANEL109_BOM_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL109_BOM.Visible == false)
            {
                this.PANEL109_BOM.Visible = true;
            }
            else
            {
                this.PANEL109_BOM.Visible = false;
            }
        }
        private void PANEL109_BOM_dataGridView1_bom_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL109_BOM_dataGridView1_bom.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL109_BOM_txtbom_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL109_BOM_txtbom_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL109_BOM_dataGridView1_bom_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL109_BOM_dataGridView1_bom.CurrentRow.Index;

                this.PANEL109_BOM_txtbom_id.Text = PANEL109_BOM_dataGridView1_bom.CurrentRow.Cells[1].Value.ToString();
                this.PANEL109_BOM_txtbom_name.Text = PANEL109_BOM_dataGridView1_bom.CurrentRow.Cells[2].Value.ToString();
                this.PANEL109_BOM_txtbom_name.Focus();
                this.PANEL109_BOM.Visible = false;
            }
        }
        private void PANEL109_BOM_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL109_BOM_btn_search_Click(object sender, EventArgs e)
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

            PANEL109_BOM_Clear_GridView1_bom();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_09bom" +
                                   " WHERE (txtbom_name LIKE '%" + this.PANEL109_BOM_txtsearch.Text + "%')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL109_BOM_dataGridView1_bom.Rows.Add();
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_id"].Value = dt2.Rows[j]["txtbom_id"].ToString();      //1
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name"].Value = dt2.Rows[j]["txtbom_name"].ToString();      //2
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name_eng"].Value = dt2.Rows[j]["txtbom_name_eng"].ToString();      //3
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
        private void PANEL109_BOM_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL109_BOM_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL109_BOM.Height = PANEL109_BOM_btnresize_low.Top + e.Y;
                this.PANEL109_BOM.Width = PANEL109_BOM_btnresize_low.Left + e.X;
            }
        }
        private void PANEL109_BOM_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL109_BOM_btnnew_Click(object sender, EventArgs e)
        {

        }

        private void BtnGrid_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = "ระเบยนใบ PO";
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO();
            frm2.Show();

        }
























        //จบส่วนเลือกรายการสินค้า ==========================================================================================================================
        //END txtbom ชื่อ BOM =======================================================================

        //END_MAT=================================================================================================================================


    }
}
