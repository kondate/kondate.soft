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
    public partial class HOME02_Purchasing_01PR_record : Form
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



        public HOME02_Purchasing_01PR_record()
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

        private void HOME02_Purchasing_01PR_record_Load(object sender, EventArgs e)
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

            this.iblword_status.Text = "เพิ่มใบ PR ใหม่";

            this.ActiveControl = this.txtpr_remark;
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

            PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax();
            PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax();

            Show_GridView1();
            Check_Group_tax_of_user();
            //จบส่วนหน้าหลัก======================================================================


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

            AUTO_BILL_TRANS_ID();

        }
        private void HOME02_Purchasing_01PR_record_KeyDown(object sender, KeyEventArgs e)
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

        private void panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void iblword_top_MouseDown(object sender, MouseEventArgs e)
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
            var frm2 = new HOME02_Purchasing.HOME02_Purchasing_01PR_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มใบ PR ใหม่";
            this.txtPr_id.ReadOnly = true;
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
                this.iblword_status.Text = "แก้ใบ PR";
                this.txtPr_id.ReadOnly = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.PANEL1316_DEPARTMENT_txtdepartment_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกฝ่าย ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1316_DEPARTMENT_txtdepartment_id.Focus();
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
                    MessageBox.Show("โปรด ใส่วันที่ต้องการสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มใบ PR ใหม่")
                    {
                        String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                        DateTime myDateTime = new DateTime();
                        myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

                        String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                        DateTime myDateTime2 = new DateTime();
                        myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);

                        //1 k017db_pr_record_trans
                        if (W_ID_Select.TRANS_BILL_STATUS.Trim() == "N")
                        {
                            cmd2.CommandText = "INSERT INTO k017db_pr_record_trans(cdkey," +
                                               "txtco_id,txtbranch_id," +
                                               "txttrans_id)" +
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                               "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                               "'" + this.txtPr_id.Text.Trim() + "')";

                            cmd2.ExecuteNonQuery();


                        }
                        else
                        {
                            cmd2.CommandText = "UPDATE k017db_pr_record_trans SET txttrans_id = '" + this.txtPr_id.Text.Trim() + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                            cmd2.ExecuteNonQuery();

                        }

                        //2 k017db_pr_record
                        cmd2.CommandText = "INSERT INTO k017db_pr_record(cdkey,txtco_id,txtbranch_id," +  //1
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                                "txtuser_name,txtemp_office_name," +  //5
                                               "txtversion_id," +  //6
                                               //====================================================

                                               "txtPr_id," + // 7
                                               "txtdepartment_id," + // 9
                                               "txtpr_remark," + // 9
                                               "txtsupplier_recommend," + // 10
                                               "txtapprove_status_id," + // 11
                                               "txtemp_office_name_manager,txtemp_office_name_approve," + // 11
                                               "txtPo_id," + // 12
                                               "txtpo_date," + // 13
                                               "txtapprove_id," + // 14
                                               "txtapprove_date," + // 15
                                               "txtRG_id," + // 16
                                               "txtRG_date," + // 17
                                               "txtreceive_id," + // 18
                                               "txtreceive_date," + // 19


                                               "txtacc_group_tax_id," + // 20
                                               "txtsum_qty," + // 21
                                               "txtsum_price," + // 22
                                               "txtsum_discount," + // 23
                                               "txtmoney_sum," + // 24
                                               "txtmoney_tax_base," + // 25
                                               "txtvat_rate," + // 26
                                               "txtvat_money," + // 27
                                               "txtmoney_after_vat," + // 28

                                               "txtpr_status," + // 29
                                               "txtpo_status," + // 30
                                               "txtapprove_status," + // 31
                                               "txtRG_status," + // 32
                                              "txtreceive_status," +  //33
                                              "txtemp_print,txtemp_print_datetime) " +  //34
                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtuser_name,@txtemp_office_name," +  //5
                                               "@txtversion_id," +  //6
                                               //=========================================================


                                               "@txtPr_id," + // 7
                                               "@txtdepartment_id," + // 8
                                               "@txtpr_remark," + // 9
                                               "@txtsupplier_recommend," + // 10
                                               "@txtapprove_status_id," + // 11
                                               "@txtemp_office_name_manager,@txtemp_office_name_approve," + // 11
                                               "@txtPo_id," + // 12
                                               "@txtpo_date," + // 13

                                               "@txtapprove_id," + // 14
                                               "@txtapprove_date," + // 15
                                               "@txtRG_id," + // 16
                                               "@txtRG_date," + // 17
                                               "@txtreceive_id," + // 18
                                               "@txtreceive_date," + // 19

                                               "@txtacc_group_tax_id," + // 20
                                               "@txtsum_qty," + // 21
                                               "@txtsum_price," + // 22
                                               "@txtsum_discount," + // 23
                                               "@txtmoney_sum," + // 24
                                               "@txtmoney_tax_base," + // 25
                                               "@txtvat_rate," + // 26
                                               "@txtvat_money," + // 27
                                               "@txtmoney_after_vat," + // 28

                                               "@txtpr_status," + // 29
                                               "@txtpo_status," + // 30
                                               "@txtapprove_status," + // 31
                                               "@txtRG_status," + // 32
                                              "@txtreceive_status," +  //33
                                              "@txtemp_print,@txtemp_print_datetime)";   //34

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

                        cmd2.Parameters.Add("@txtPr_id", SqlDbType.NVarChar).Value = this.txtPr_id.Text.Trim();  //7
                        cmd2.Parameters.Add("@txtdepartment_id", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.Trim();  //8
                        cmd2.Parameters.Add("@txtpr_remark", SqlDbType.NVarChar).Value = this.txtpr_remark.Text.Trim();  //9
                        cmd2.Parameters.Add("@txtsupplier_recommend", SqlDbType.NVarChar).Value = this.txtsupplier_recommend.Text.Trim();  //10
                        cmd2.Parameters.Add("@txtapprove_status_id", SqlDbType.NVarChar).Value = "";  //11
                        cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.ToString();  //11
                        cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.ToString();  //11
                        cmd2.Parameters.Add("@txtPo_id", SqlDbType.NVarChar).Value ="";  //12
                        cmd2.Parameters.Add("@txtpo_date", SqlDbType.NVarChar).Value = "";  //13

                        cmd2.Parameters.Add("@txtapprove_id", SqlDbType.NVarChar).Value = "";  //14
                        cmd2.Parameters.Add("@txtapprove_date", SqlDbType.NVarChar).Value = "";  //15
                        cmd2.Parameters.Add("@txtRG_id", SqlDbType.NVarChar).Value = "";  //16
                        cmd2.Parameters.Add("@txtRG_date", SqlDbType.NVarChar).Value = "";  //17
                        cmd2.Parameters.Add("@txtreceive_id", SqlDbType.NVarChar).Value = "";  //18
                        cmd2.Parameters.Add("@txtreceive_date", SqlDbType.NVarChar).Value = "";  //19


                        cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //20
                        cmd2.Parameters.Add("@txtsum_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //21
                        cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //22
                        cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //23
                        cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //24
                        cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //25
                        cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //26
                        cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //27
                        cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //28

                        cmd2.Parameters.Add("@txtpr_status", SqlDbType.NVarChar).Value = "0";  //29
                        cmd2.Parameters.Add("@txtpo_status", SqlDbType.NVarChar).Value = "";  //30
                        cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = "";  //31
                        cmd2.Parameters.Add("@txtRG_status", SqlDbType.NVarChar).Value = "";  //32
                        cmd2.Parameters.Add("@txtreceive_status", SqlDbType.NVarChar).Value = "";  //33
                        cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
                        cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);

                        //==============================
                        cmd2.ExecuteNonQuery();

                        //if (PR_STATUS.Trim() == "Y")
                        //{

                        //}
                        //else
                        //{
                        //    //2 k017db_pr_record
                        //    cmd2.CommandText = "INSERT INTO k017db_pr_all(cdkey,txtco_id,txtbranch_id," +  //1
                        //                           "txttrans_date_server,txttrans_time," +  //2
                        //                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                        //                           "txtcomputer_ip,txtcomputer_name," +  //4
                        //                            "txtuser_name,txtemp_office_name," +  //5
                        //                           "txtversion_id," +  //6
                        //                             //====================================================

                        //                           "txtPr_id," + // 7
                        //                           "txtdepartment_id," + // 8
                        //                           "txtdepartment_name," + // 9
                        //                           "txtPo_id," + // 10
                        //                           "txtpo_date," + // 11
                        //                           "txtsupplier_id," + // 12
                        //                           "txtsupplier_name," + // 13
                        //                           "txtapprove_id," + // 14
                        //                           "txtapprove_date," + // 15
                        //                           "txtapprove_name," + // 16
                        //                           "txtRG_id," + // 17
                        //                           "txtRG_date," + // 18
                        //                           "txtRG_name," + // 19
                        //                           "txtReceive_id," + // 20
                        //                           "txtReceive_date," + // 21
                        //                           "txtwherehouse_id," + // 22
                        //                           "txtwherehouse_name," + // 23
                        //                           "txtmoney_after_vat," + // 25
                        //                           "txtpr_status," + // 26
                        //                           "txtpo_status," + // 27
                        //                           "txtapprove_status," + // 28
                        //                          "txtRG_status," +  //29
                        //                          "txtreceive_status," +  //30
                        //                          "txtemp_print,txtemp_print_datetime) " +  //31
                        //                           "VALUES (@cdkey2,@txtco_id2,@txtbranch_id2," +  //1
                        //                           "@txttrans_date_server2,@txttrans_time2," +  //2
                        //                           "@txttrans_year2,@txttrans_month2,@txttrans_day2,@txttrans_date_client2," +  //3
                        //                           "@txtcomputer_ip2,@txtcomputer_name2," +  //4
                        //                           "@txtuser_name2,@txtemp_office_name2," +  //5
                        //                           "@txtversion_id2," +  //6
                        //                                                //=========================================================


                        //                            "@txtPr_id2," + // 7
                        //                           "@txtdepartment_id2," + // 8
                        //                           "@txtdepartment_name2," + // 9
                        //                           "@txtPo_id2," + // 10
                        //                           "@txtpo_date2," + // 11
                        //                           "@txtsupplier_id2," + // 12
                        //                           "@txtsupplier_name2," + // 13
                        //                           "@txtapprove_id2," + // 14
                        //                           "@txtapprove_date2," + // 15
                        //                           "@txtapprove_name2," + // 16
                        //                           "@txtRG_id2," + // 17
                        //                           "@txtRG_date2," + // 18
                        //                           "@txtRG_name2," + // 19
                        //                           "@txtReceive_id2," + // 20
                        //                           "@txtReceive_date2," + // 21
                        //                           "@txtwherehouse_id2," + // 22
                        //                           "@txtwherehouse_name2," + // 23
                        //                           "@txtmoney_after_vat2," + // 25
                        //                           "@txtpr_status2," + // 26
                        //                           "@txtpo_status2," + // 27
                        //                           "@txtapprove_status2," + // 28
                        //                          "@txtRG_status2," +  //29
                        //                          "@txtreceive_status2," +  //30
                        //                          "@txtemp_print2,@txtemp_print_datetime2)";   //31 

                        //    cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        //    cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        //    cmd2.Parameters.Add("@txtbranch_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //1


                        //    cmd2.Parameters.Add("@txttrans_date_server2", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                        //    cmd2.Parameters.Add("@txttrans_time2", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                        //    cmd2.Parameters.Add("@txttrans_year2", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                        //    cmd2.Parameters.Add("@txttrans_month2", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                        //    cmd2.Parameters.Add("@txttrans_day2", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);
                        //    cmd2.Parameters.Add("@txttrans_date_client2", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);


                        //    cmd2.Parameters.Add("@txtcomputer_ip2", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_IP.Trim();
                        //    cmd2.Parameters.Add("@txtcomputer_name2", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim();
                        //    cmd2.Parameters.Add("@txtuser_name2", SqlDbType.NVarChar).Value = W_ID_Select.M_USERNAME.Trim();
                        //    cmd2.Parameters.Add("@txtemp_office_name2", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
                        //    cmd2.Parameters.Add("@txtversion_id2", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();  //7
                        //                                                                                                      //==============================================================================

                        //    cmd2.Parameters.Add("@txtPr_id2", SqlDbType.NVarChar).Value = this.txtPr_id.Text.Trim();  //7
                        //    cmd2.Parameters.Add("@txtdepartment_id2", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.Trim();  //8
                        //    cmd2.Parameters.Add("@txtdepartment_name2", SqlDbType.NVarChar).Value = this.PANEL1316_DEPARTMENT_txtdepartment_name.Text.Trim();  //9
                        //    cmd2.Parameters.Add("@txtPo_id2", SqlDbType.NVarChar).Value = "";  //10
                        //    cmd2.Parameters.Add("@txtpo_date2", SqlDbType.NVarChar).Value = "";  //11
                        //    cmd2.Parameters.Add("@txtsupplier_id2", SqlDbType.NVarChar).Value = "";  //12
                        //    cmd2.Parameters.Add("@txtsupplier_name2", SqlDbType.NVarChar).Value = "";  //13
                        //    cmd2.Parameters.Add("@txtapprove_id2", SqlDbType.NVarChar).Value = "";  //14
                        //    cmd2.Parameters.Add("@txtapprove_date2", SqlDbType.NVarChar).Value = "";  //15
                        //    cmd2.Parameters.Add("@txtapprove_name2", SqlDbType.NVarChar).Value = "";  //16
                        //    cmd2.Parameters.Add("@txtRG_id2", SqlDbType.NVarChar).Value = "";  //17
                        //    cmd2.Parameters.Add("@txtRG_date2", SqlDbType.NVarChar).Value = "";  //18
                        //    cmd2.Parameters.Add("@txtRG_name2", SqlDbType.NVarChar).Value = "";  //19
                        //    cmd2.Parameters.Add("@txtReceive_id2", SqlDbType.NVarChar).Value = "";  //20
                        //    cmd2.Parameters.Add("@txtReceive_date2", SqlDbType.NVarChar).Value = "";  //21
                        //    cmd2.Parameters.Add("@txtwherehouse_id2", SqlDbType.NVarChar).Value = "";  //22
                        //    cmd2.Parameters.Add("@txtwherehouse_name2", SqlDbType.NVarChar).Value = "";  //23
                        //    cmd2.Parameters.Add("@txtmoney_after_vat2", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //24

                        //    cmd2.Parameters.Add("@txtpr_status2", SqlDbType.NVarChar).Value = "0";  //25
                        //    cmd2.Parameters.Add("@txtpo_status2", SqlDbType.NVarChar).Value = "";  //26
                        //    cmd2.Parameters.Add("@txtapprove_status2", SqlDbType.NVarChar).Value = "";  //27
                        //    cmd2.Parameters.Add("@txtRG_status2", SqlDbType.NVarChar).Value = "";  //28
                        //    cmd2.Parameters.Add("@txtreceive_status2", SqlDbType.NVarChar).Value = "";  //29
                        //    cmd2.Parameters.Add("@txtemp_print2", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();   //30
                        //    cmd2.Parameters.Add("@txtemp_print_datetime2", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);  //31

                        //    //==============================
                        //    cmd2.ExecuteNonQuery();

                        //}

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


                                    //=====================================================================================================
                                    //3 k017db_pr_record_detail
                                    DateTime want_receive_date = Convert.ToDateTime(this.GridView1.Rows[i].Cells[9].Value.ToString());
                                    string want_date = want_receive_date.ToString("yyyy-MM-dd");
                                    //string OD_date = DateTime.ParseExact(this.GridView1.Rows[i].Cells[9].Value, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
                                    //cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                                    //cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                                    //cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);

                                    cmd2.CommandText = "INSERT INTO k017db_pr_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                       "txttrans_year,txttrans_month,txttrans_day," +
                                       "txtPr_id," +  //2
                                       "txtmat_no," +  //3
                                       "txtmat_id," +  //4
                                       "txtmat_name," +  //5
                                       "txtmat_unit1_name," +  //6
                                       "txtqty," +  //7
                                       "txtqty_balance," +  //8
                                       "txtprice," +   //9
                                       "txtdiscount_rate," +  //10
                                       "txtdiscount_money," +  //11
                                       "txtsum_total," +  //12
                                       "txtwant_receive_date," +  //13
                                       "txtitem_no) " +  //14

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +
                                "'" + this.txtPr_id.Text.Trim() + "'," +  //2
                                "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //3
                                "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //4
                                "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +    //5
                                "'" + this.GridView1.Rows[i].Cells[4].Value.ToString() + "'," +  //6
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[5].Value.ToString())) + "'," +  //7
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //8
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[6].Value.ToString())) + "'," +  //9
                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //10
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[7].Value.ToString())) + "'," +  //11
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[8].Value.ToString())) + "'," +  //12
                               "'" + want_date + "'," +  //13
                               "'" + this.GridView1.Rows[i].Cells[0].Value.ToString() + "')";   //14

                                cmd2.ExecuteNonQuery();

                                    //===================================================================================================================
                                    ////4 k017db_pr_all_detail
                                    //if (PR_STATUS.Trim() == "Y")
                                    //{

                                    //}
                                    //else
                                    //{
                                    //                cmd2.CommandText = "INSERT INTO k017db_pr_all_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                    //               "txttrans_date_server,txttrans_time," +  //2
                                    //               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                    //               "txtcomputer_ip,txtcomputer_name," +  //4
                                    //                "txtuser_name,txtemp_office_name," +  //5
                                    //               "txtversion_id," +  //6
                                    //                                   //====================================================

                                    //                   "txtpr_id," +  //7
                                    //                   "txtpo_id," +  //8
                                    //                   "txtapprove_id," +  //9
                                    //                   "txtRG_id," +  //10
                                    //                   "txtreceive_id," +  //11
                                    //                   "txtbill_remark," +  //12
                                    //                   "txtwant_receive_date," +  //13

                                    //                   "txtmat_no," +  //14
                                    //                   "txtmat_id," +  //15
                                    //                   "txtmat_name," +  //16
                                    //                   "txtmat_unit1_name," +  //17
                                    //                   "txtprice," +   //18
                                    //                   "txtdiscount_rate," +  //19
                                    //                   "txtdiscount_money," +  //20
                                    //                   "txtsum_total," +  //21
                                    //                   "txtitem_no," +  //22

                                    //                    "txtqty_pr," +  //23
                                    //                   "txtqty_po," +  //24
                                    //                   "txtqty_approve," +  //25
                                    //                   "txtqty_rg," +  //26
                                    //                   "txtqty_balance," +  //27
                                    //                   "txtqty_receive) " +  //28

                                    //            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                    //            "'" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "','" + myDateTime2.ToString("HH:mm:ss", UsaCulture) + "'," +  //2
                                    //            "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", UsaCulture) + "'," +  //3
                                    //            "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                                    //            "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                                    //            "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                                    //            //=======================================================


                                    //            "'" + this.txtPr_id.Text.Trim() + "'," +  //7
                                    //            "''," +  //8
                                    //            "''," +  //9
                                    //            "''," +  //10
                                    //            "''," +  //11
                                    //            "'" + this.txtpr_remark.Text.Trim() + "'," +  //12
                                    //            "'" + want_date + "'," +  //13

                                    //            "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //14
                                    //            "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //15
                                    //            "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +    //16
                                    //            "'" + this.GridView1.Rows[i].Cells[4].Value.ToString() + "'," +  //17
                                    //           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[6].Value.ToString())) + "'," +  //18
                                    //          "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19
                                    //           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[7].Value.ToString())) + "'," +  //20
                                    //           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[8].Value.ToString())) + "'," +  //21
                                    //            "'" + this.GridView1.Rows[i].Cells[0].Value.ToString() + "'," +  //22

                                    //           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[5].Value.ToString())) + "'," +  //23
                                    //          "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //24
                                    //          "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //25
                                    //          "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //26
                                    //          "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //27
                                    //           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "')";   //28

                                    //                cmd2.ExecuteNonQuery();

                                    //    //this.GridView1.Columns[0].Name = "Col_Auto_num";
                                    //    //this.GridView1.Columns[1].Name = "Col_txtmat_no";
                                    //    //this.GridView1.Columns[2].Name = "Col_txtmat_id";
                                    //    //this.GridView1.Columns[3].Name = "Col_txtmat_name";
                                    //    //this.GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                                    //    //this.GridView1.Columns[5].Name = "Col_txtqty";
                                    //    //this.GridView1.Columns[6].Name = "Col_txtprice";
                                    //    //this.GridView1.Columns[7].Name = "Col_txtdiscount_money";
                                    //    //this.GridView1.Columns[8].Name = "Col_txtsum_total";
                                    //    //this.GridView1.Columns[9].Name = "Col_date";


                                    //}
                                    //========================================================
                                    //5 k017db_pr_all_detail_balance ==============================================================================================

                              //      cmd2.CommandText = "INSERT INTO k017db_pr_all_detail_balance(cdkey,txtco_id,txtbranch_id," +  //1
                              //     "txttrans_date_server,txttrans_time," +  //2
                              //     "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                              //     "txtcomputer_ip,txtcomputer_name," +  //4
                              //      "txtuser_name,txtemp_office_name," +  //5
                              //     "txtversion_id," +  //6
                              //                         //====================================================

                              //         "txtpr_id," +  //7
                              //         "txtpo_id," +  //8
                              //         "txtapprove_id," +  //9
                              //         "txtRG_id," +  //10
                              //         "txtreceive_id," +  //11
                              //         "txtbill_remark," +  //12
                              //         "txtwant_receive_date," +  //13

                              //         "txtmat_no," +  //14
                              //         "txtmat_id," +  //15
                              //         "txtmat_name," +  //16
                              //         "txtmat_unit1_name," +  //17
                              //         "txtprice," +   //18
                              //         "txtdiscount_rate," +  //19
                              //         "txtdiscount_money," +  //20
                              //         "txtsum_total," +  //21
                              //         "txtitem_no," +  //22

                              //          "txtqty_pr," +  //23
                              //         "txtqty_po," +  //24
                              //         "txtqty_approve," +  //25
                              //         "txtqty_rg," +  //26
                              //         "txtqty_balance," +  //27
                              //         "txtqty_receive) " +  //28

                              //  "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                              //  "'" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "','" + myDateTime2.ToString("HH:mm:ss", UsaCulture) + "'," +  //2
                              //  "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", UsaCulture) + "'," +  //3
                              //  "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                              //  "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                              //  "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                              //                                                //=======================================================


                              //  "'" + this.txtPr_id.Text.Trim() + "'," +  //7
                              //  "''," +  //8
                              //  "''," +  //9
                              //  "''," +  //10
                              //  "''," +  //11
                              //  "'" + this.txtpr_remark.Text.Trim() + "'," +  //12
                              //  "'" + want_date + "'," +  //13

                              //  "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //14
                              //  "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //15
                              //  "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +    //16
                              //  "'" + this.GridView1.Rows[i].Cells[4].Value.ToString() + "'," +  //17
                              // "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[6].Value.ToString())) + "'," +  //18
                              //"'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19
                              // "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[7].Value.ToString())) + "'," +  //20
                              // "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[8].Value.ToString())) + "'," +  //21
                              //  "'" + this.GridView1.Rows[i].Cells[0].Value.ToString() + "'," +  //22

                              // "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells[5].Value.ToString())) + "'," +  //23
                              //"'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //24
                              //"'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //25
                              //"'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //26
                              //"'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //27
                              // "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "')";   //28

                              //      cmd2.ExecuteNonQuery();

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
                                    //====================================================================================================
                                }
                            }
                        }

                    
                        
                        //6 k017db_pr_record_group_tax
                        if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text.Trim() == "N")
                        {
                            cmd2.CommandText = "INSERT INTO k017db_pr_record_group_tax(cdkey," +
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
                            cmd2.CommandText = "UPDATE k017db_pr_record_group_tax SET txtacc_group_tax_id = '" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() + "'," +
                                               "txtacc_group_tax_name = '" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text.Trim() + "'," +
                                               "txtacc_group_tax_vat_rate = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtuser_name = '" + W_ID_Select.M_USERNAME.Trim() + "')";

                            cmd2.ExecuteNonQuery();

                        }


                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขใบ PR")
                    {

                    }
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

                        if (this.iblword_status.Text.Trim() == "เพิ่มใบ PR ใหม่")
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
            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            W_ID_Select.TRANS_ID = this.txtPr_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_01PR_record_Print frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_01PR_record_Print();
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

            W_ID_Select.TRANS_ID = this.txtPr_id.Text.Trim();
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
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_k017db_pr_record.rpt");


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
                rpt.SetParameterValue("txtpr_id", W_ID_Select.TRANS_ID.Trim());

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


                UPDATE_BARCODE_TO_GridView1();
                this.txtmat_barcode_id.Focus();

            }
        }
        private void UPDATE_BARCODE_TO_GridView1()
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

            PANEL_MAT_Show_GridView1();
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
                            GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
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
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

        }
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
            this.GridView1.Columns[9].HeaderText = " วันที่ต้องการสินค้า";

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
            this.GridView1.Columns[5].ReadOnly = false;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[6].Visible = true;  //"Col_txtprice";
            this.GridView1.Columns[6].Width = 100;
            this.GridView1.Columns[6].ReadOnly = false;
            this.GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[7].Visible = true;  //"Col_txtdiscount_money";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = false;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[8].Visible = true;  //"Col_txtsum_total";
            this.GridView1.Columns[8].Width = 150;
            this.GridView1.Columns[8].ReadOnly = true;
            this.GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[9].Visible = true;  //"Col_date";
            this.GridView1.Columns[9].Width = 150;
            this.GridView1.Columns[9].ReadOnly = false;
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
            this.btnremove_row.Visible = true;

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
        private void GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow2 = GridView1.CurrentRow.Index;
            int rowscount = GridView1.Rows.Count;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();

            //if (this.GridView1.Rows.Count > 0)
            //{
            //    //===============================================================
            //    for (int i = 0; i < this.GridView1.Rows.Count - 1; i++)
            //    {

            //        if (GridView1.Rows[i].Cells[3].Value != null)
            //        {
            //            if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells[5].Value.ToString())) > 0)
            //            {
            //                GridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[1].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[2].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
            //                GridView1.Rows[i].Cells[3].Style.Font = new Font("Tahoma", 12F);

            //                GridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[4].Style.Font = new Font("Tahoma", 12F);

            //                //GridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[5].Style.Font = new Font("Tahoma", 12F);


            //                GridView1.Rows[i].Cells[6].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[6].Style.Font = new Font("Tahoma", 12F);

            //                GridView1.Rows[i].Cells[7].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[7].Style.Font = new Font("Tahoma", 12F);

            //                GridView1.Rows[i].Cells[8].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[8].Style.Font = new Font("Tahoma", 12F);

            //                GridView1.Rows[i].Cells[9].Style.Font = new Font("Tahoma", 12F);

            //            }
            //            else
            //            {
            //                GridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[1].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[2].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
            //                GridView1.Rows[i].Cells[3].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[4].Style.Font = new Font("Tahoma", 8F);

            //                //GridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[5].Style.Font = new Font("Tahoma", 8F);


            //                GridView1.Rows[i].Cells[6].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[6].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[7].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[7].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[8].Style.BackColor = Color.White;
            //                GridView1.Rows[i].Cells[8].Style.Font = new Font("Tahoma", 8F);

            //                GridView1.Rows[i].Cells[9].Style.Font = new Font("Tahoma", 8F);
            //            }


            //        }
            //    }
            //}
            ////===============================================================

            //if (GridView1.Rows[curRow].Cells[3].Style.BackColor == Color.LightGoldenrodYellow)
            //{

            //    GridView1.Rows[curRow].Cells[1].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[1].Style.Font = new Font("Tahoma", 8F);

            //    GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[2].Style.Font = new Font("Tahoma", 8F);


            //    GridView1.Rows[curRow].Cells[3].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[3].Style.ForeColor = Color.Black;
            //    GridView1.Rows[curRow].Cells[3].Style.Font = new Font("Tahoma", 8F);

            //    GridView1.Rows[curRow].Cells[4].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[4].Style.Font = new Font("Tahoma", 8F);

            //    //GridView1.Rows[curRow].Cells[5].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[5].Style.Font = new Font("Tahoma", 8F);


            //    GridView1.Rows[curRow].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[6].Style.Font = new Font("Tahoma", 8F);

            //    GridView1.Rows[curRow].Cells[7].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[7].Style.Font = new Font("Tahoma", 8F);

            //    GridView1.Rows[curRow].Cells[8].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[8].Style.Font = new Font("Tahoma", 8F);

            //    //GridView1.Rows[curRow].Cells[9].Style.BackColor = Color.White;
            //    GridView1.Rows[curRow].Cells[9].Style.Font = new Font("Tahoma", 8F);
            //}
            //else
            //{
            //    GridView1.Rows[curRow].Cells[1].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[1].Style.Font = new Font("Tahoma", 12F);

            //    GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[2].Style.Font = new Font("Tahoma", 12F);

            //    GridView1.Rows[curRow].Cells[3].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[3].Style.ForeColor = Color.Red;
            //    GridView1.Rows[curRow].Cells[3].Style.Font = new Font("Tahoma", 12F);

            //    GridView1.Rows[curRow].Cells[4].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[4].Style.Font = new Font("Tahoma", 12F);

            //    //GridView1.Rows[curRow].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[5].Style.Font = new Font("Tahoma", 12F);


            //    GridView1.Rows[curRow].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[6].Style.Font = new Font("Tahoma", 12F);

            //    GridView1.Rows[curRow].Cells[7].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[7].Style.Font = new Font("Tahoma", 12F);

            //    GridView1.Rows[curRow].Cells[8].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[8].Style.Font = new Font("Tahoma", 12F);

            //    //GridView1.Rows[curRow].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
            //    GridView1.Rows[curRow].Cells[9].Style.Font = new Font("Tahoma", 12F);
            //}
            ////======================================
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
        void txt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GridView1_Cal_Sum();
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
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_ONvat")  //ซื้อไม่มีvat
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
        //จบส่วนตารางสำหรับบันทึก========================================================================





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
                this.PANEL1313_ACC_GROUP_TAX.Location = new Point(this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.X- PANEL1313_ACC_GROUP_TAX.Height -53 , this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.Y - PANEL1313_ACC_GROUP_TAX.Height-2);
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
                                  " FROM k017db_pr_record_trans" +
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
                            TMP = "PR" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "PR" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "PR" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtPr_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }













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
       
        
        
        
        
        
        //2. ส่วนเลือกรายการสินค้า =========================================================================================================================
        //MAT=====================================================================================================================================

        //PANEL_MAT====================================================
        private Point MouseDownLocation;
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
            SHOW_btnGo();
        }
        private void SHOW_btnGo()
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }
        private void PANEL_MAT_btnGo2_Click(object sender, EventArgs e)
        {
            SHOW_btnGo2();
        }
        private void SHOW_btnGo2()
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }
        private void PANEL_MAT_btnGo3_Click(object sender, EventArgs e)
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }
        private void PANEL_MAT_btnGo4_Click(object sender, EventArgs e)
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }
        private void PANEL_MAT_btnGo5_Click(object sender, EventArgs e)
        {
            SHOW_btnGo5();
        }
        private void SHOW_btnGo5()
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

        }
        private void PANEL_MAT_btnGo6_Click(object sender, EventArgs e)
        {
            SHOW_btnGo6();
        }
        private void SHOW_btnGo6()
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();


        }
        private void PANEL_MAT_btnupdate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
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
                }
            }

            UPDATE_TO_GridView1();
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

            PANEL_MAT_Show_GridView1();
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

            PANEL_MAT_Show_GridView1();
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
            selectedRowIndex = PANEL_MAT_GridView1.CurrentRow.Index;

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

                    if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[curRow].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        this.PANEL_MAT_GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
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

            if (this.PANEL_MAT_GridView1.Rows.Count > 0)
            {
                //===============================================================
                for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 1; i++)
                {

                    if (PANEL_MAT_GridView1.Rows[i].Cells[3].Value != null)
                    {
                        if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                        {
                            this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value = ".00";
                        }

                        if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                        {
                            PANEL_MAT_GridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[1].Style.Font = new Font("Tahoma", 12F);

                            PANEL_MAT_GridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[2].Style.Font = new Font("Tahoma", 12F);

                            PANEL_MAT_GridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
                            PANEL_MAT_GridView1.Rows[i].Cells[3].Style.Font = new Font("Tahoma", 12F);

                            PANEL_MAT_GridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[4].Style.Font = new Font("Tahoma", 12F);

                            //PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[5].Style.Font = new Font("Tahoma", 12F);


                            PANEL_MAT_GridView1.Rows[i].Cells[6].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[6].Style.Font = new Font("Tahoma", 12F);

                            PANEL_MAT_GridView1.Rows[i].Cells[7].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[7].Style.Font = new Font("Tahoma", 12F);

                            PANEL_MAT_GridView1.Rows[i].Cells[8].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[8].Style.Font = new Font("Tahoma", 12F);

                            PANEL_MAT_GridView1.Rows[i].Cells[9].Style.Font = new Font("Tahoma", 12F);

                        }
                        else
                        {
                            PANEL_MAT_GridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[1].Style.Font = new Font("Tahoma", 8F);

                            PANEL_MAT_GridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[2].Style.Font = new Font("Tahoma", 8F);

                            PANEL_MAT_GridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
                            PANEL_MAT_GridView1.Rows[i].Cells[3].Style.Font = new Font("Tahoma", 8F);

                            PANEL_MAT_GridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[4].Style.Font = new Font("Tahoma", 8F);

                            //PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[5].Style.Font = new Font("Tahoma", 8F);


                            PANEL_MAT_GridView1.Rows[i].Cells[6].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[6].Style.Font = new Font("Tahoma", 8F);

                            PANEL_MAT_GridView1.Rows[i].Cells[7].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[7].Style.Font = new Font("Tahoma", 8F);

                            PANEL_MAT_GridView1.Rows[i].Cells[8].Style.BackColor = Color.White;
                            PANEL_MAT_GridView1.Rows[i].Cells[8].Style.Font = new Font("Tahoma", 8F);

                            PANEL_MAT_GridView1.Rows[i].Cells[9].Style.Font = new Font("Tahoma", 8F);
                        }


                    }
                }
            }
            //===============================================================

            if (PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.BackColor == Color.LightGoldenrodYellow)
            {

                PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.Font = new Font("Tahoma", 8F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.Font = new Font("Tahoma", 8F);


                PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.ForeColor = Color.Black;
                PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.Font = new Font("Tahoma", 8F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.Font = new Font("Tahoma", 8F);

                //PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.Font = new Font("Tahoma", 8F);


                PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.Font = new Font("Tahoma", 8F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.Font = new Font("Tahoma", 8F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.Font = new Font("Tahoma", 8F);

                //PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.BackColor = Color.White;
                PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.Font = new Font("Tahoma", 8F);
            }
            else
            {
                PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[1].Style.Font = new Font("Tahoma", 12F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[2].Style.Font = new Font("Tahoma", 12F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.ForeColor = Color.Red;
                PANEL_MAT_GridView1.Rows[curRow].Cells[3].Style.Font = new Font("Tahoma", 12F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[4].Style.Font = new Font("Tahoma", 12F);

                //PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[5].Style.Font = new Font("Tahoma", 12F);


                PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[6].Style.Font = new Font("Tahoma", 12F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[7].Style.Font = new Font("Tahoma", 12F);

                PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[8].Style.Font = new Font("Tahoma", 12F);

                //PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[curRow].Cells[9].Style.Font = new Font("Tahoma", 12F);
            }
            //======================================


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
            //PANEL_MAT_GridView1.CurrentCell.Value = dtp.Text.ToString();

        }
        void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {

                PANEL_MAT_GridView1_Cal_Sum();
            }
        }
        void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter )
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
        private void PANEL_MAT_GridView1_Color_Column()
        {

            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
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
                    SHOW_btnGo();
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
                    SHOW_btnGo4();
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
                    SHOW_btnGo2();
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
                    SHOW_btnGo5();
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
                    SHOW_btnGo6();
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
            W_ID_Select.WORD_TOP = "ระเบยนใบ PR";
            kondate.soft.HOME02_Purchasing.HOME02_Purchasing_01PR frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_01PR();
            frm2.Show();

        }
        //END txtbom ชื่อ BOM =======================================================================

        //END_MAT=================================================================================================================================
        //จบส่วนเลือกรายการสินค้า ==========================================================================================================================

    }
}
