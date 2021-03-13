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

namespace kondate.soft.HOME03_Production
{
    public partial class HOME03_Production_02Berg_Produce_record : Form
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
        
        //
        //2/2/64 0.43 วันนี้ทำถึงแก้ไข field ใหม่ ให้บันทึกได้
        //ต่อไปหาผลรวมกระสอบ หลอด กกของกระสอบและหลอด
        //insert into c002_01berg_produce_record
        //insert intoc002_01berg_produce_record_detail
        //

        public HOME03_Production_02Berg_Produce_record()
        {
            InitializeComponent();

            //GridView1.Controls.Add(dtp1);
            //dtp1.Visible = true;
            //dtp1.ShowUpDown = true;
            //dtp1.CustomFormat = "HH:mm tt";
            //dtp1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            //dtp1.TextChanged += new EventHandler(dtp1_TextChange);


        }

        private void HOME03_Production_02Berg_Produce_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            ////สำหรับทดสอบระบบ =====================================================================================================================
            //W_ID_Select.CDKEY = "samn20201125";//  = "chaixifactory2562020601"= "";// = "chaixifactory2562020601"= "";
            //W_ID_Select.ADATASOURCE = "916909b5121b.sn.mynetname.net,6001";  //916909b5121b.sn.mynetname.net,6001 // C4PC_AOT\\SQLEXPRESS,49170" //ASAICAFEKLONG3\\SQLEXPRESS,49170   //172.168.0.15\\SQLEXPRESS,49170
            //W_ID_Select.DATABASE_NAME = "samn_db"; //KREST2020
            //W_ID_Select.Lang = "001";// = "001"= ""; //001ไทย, 002Eng,003ลาว,004กัมพูชา,005พม่า

            //W_ID_Select.Crytal_SERVER = "916909b5121b.sn.mynetname.net,6001";
            //W_ID_Select.Crytal_DATABASE = "samn_db";
            //W_ID_Select.Crytal_USER = "sa";
            //W_ID_Select.Crytal_Pass = "Kon51Aot";

            //W_ID_Select.M_COID = "KD";
            //W_ID_Select.M_CONAME = "บริษัท ทดสอบระบบ จำกัด";
            //W_ID_Select.M_BRANCHID = "KD001";
            //W_ID_Select.M_BRANCHNAME = "สำนักงานใหญ่";
            //W_ID_Select.M_BRANCHNAME_SHORT = "HO";

            //W_ID_Select.M_USERNAME = "admin";
            //W_ID_Select.M_USERNAME_TYPE = "4";
            ////สำหรับทดสอบระบบ =====================================================================================================================

            //W_ID_Select.M_FORM_NUMBER = "H0205RGRD";
            //CHECK_ADD_FORM();
            //CHECK_USER_RULE();


            //W_ID_Select.LOG_ID = "1";
            //W_ID_Select.LOG_NAME = "Login";
            //TRANS_LOG();
            // =====================================================================================================================


            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";
            this.iblword_status.Text = "บันทึกใบเบิกวัตถุดิบผลิต (ด้าย)";

            this.ActiveControl = this.txtic_remark;
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

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax();
            PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax();

            PANEL0103_BERG_TYPE_GridView1_berg_type();
            PANEL0103_BERG_TYPE_Fill_berg_type();

            PANEL0102_MACHINE_GridView1_machine();
            PANEL0102_MACHINE_Fill_machine();

            PANEL_MAT_GridView1_mat();
            PANEL_MAT_Fill_mat();
            this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
            this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
            this.PANEL_MAT_cboSearch.Text = "ชื่อสินค้า";

            PANEL0106_NUMBER_MAT_GridView1_number_mat();
            PANEL0106_NUMBER_MAT_Fill_number_mat();

            Load_FIRST_MAT();
            Show_GridView1();
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
            var frm2 = new HOME03_Production.HOME03_Production_02Berg_Produce_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "บันทึกใบเบิกวัตถุดิบผลิต (ด้าย)";
            this.txtic_id.ReadOnly = true;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกคลังสินค้าที่จะเบิกออก ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1306_WH_txtwherehouse_id.Focus();
                return;
            }
            if (this.PANEL_MAT_txtmat_id.Text == "")
            {
                MessageBox.Show("โปรด ใส่รหัสวัตถุดิบ(ด้าย) ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL_MAT_txtmat_id.Focus();
                return;
            }
            if (this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text == "")
            {
                MessageBox.Show("โปรด ใส่เบอร์วัตถุดิบ(ด้าย) ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Focus();
                return;
            }
            if (this.PANEL0103_BERG_TYPE_txtberg_type_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกประเภทเบิกคลัง ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL0103_BERG_TYPE_txtberg_type_id.Focus();
                return;
            }
            if (Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) > Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())))
            {
                MessageBox.Show("สต๊อคติดลบ  !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            Load_FIRST_FIND_INSERT();
            AUTO_BILL_TRANS_ID();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();
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
                            cmd2.CommandText = "INSERT INTO c002_01berg_produce_record_trans(cdkey," +
                                               "txtco_id,txtbranch_id," +
                                               "txttrans_id)" +
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                               "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                               "'" + this.txtic_id.Text.Trim() + "')";

                            cmd2.ExecuteNonQuery();


                        }
                        else
                        {
                            cmd2.CommandText = "UPDATE c002_01berg_produce_record_trans SET txttrans_id = '" + this.txtic_id.Text.Trim() + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                            cmd2.ExecuteNonQuery();

                        }
                        //MessageBox.Show("ok1");

                        //2 c002_01berg_produce_record
                        cmd2.CommandText = "INSERT INTO c002_01berg_produce_record(cdkey,txtco_id,txtbranch_id," +  //1
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                                "txtuser_name,txtemp_office_name," +  //5
                                               "txtversion_id," +  //6
                                                 //====================================================

                                               "txtic_id," + // 7
                                               "txtberg_type_id," + // 8
                                               "txtwherehouse_id," + // 9
                                               "txtmachine_id," + // 9

                                               "txtemp1_id," + // 10
                                                "txtemp1_name," + // 11
                                               "txtemp_office_name_manager," + // 12
                                               "txtemp_office_name_approve," + // 13
                                              "txtproject_id," + // 14
                                               "txtjob_id," + // 15
                                               "txtic1_remark," + // 16

                                               "txtcurrency_id," + // 17
                                               "txtcurrency_date," + // 18
                                               "txtcurrency_rate," + // 19

                                               "txtmat_no," + // 19
                                               "txtmat_id," + // 19
                                               "txtmat_name," + // 19
                                               "txtnumber_mat_id," + // 19


                                               "txtacc_group_tax_id," + // 20

                                               "txtcost_qty_balance_yokma," + // 25
                                               "txtcost_qty_price_average_yokma," + // 26
                                               "txtcost_money_sum_yokma," + // 27

                                               "txtsum_qty," + // 28
                                               "txtsum_price," + // 29
                                               "txtsum_discount," + // 30
                                               "txtmoney_sum," + // 31
                                               "txtmoney_tax_base," + // 32
                                               "txtvat_rate," + // 33
                                               "txtvat_money," + // 34
                                               "txtmoney_after_vat," + // 35
                                               "txtmoney_after_vat_creditor," + // 36
                                               "txtcreditor_status," + // 37

                                               "txtcost_qty_balance_yokpai," + // 38
                                               "txtcost_qty_price_average_yokpai," + // 39
                                               "txtcost_money_sum_yokpai," + // 40

                                               "txtcost_qty2_balance_yokma," + // 41
                                               "txtsum2_qty," + // 42
                                               "txtcost_qty2_balance_yokpai," + // 43

                                               "txtic_status," +  //44
                                              "txtpayment_status," +  //45
                                              "txtacc_record_status," +  //46
                                              "txtemp_print," +  //46
                                              "txtemp_print_datetime," +  //46
                                              "txtFG1_id," +  //46
                                              "txtroll_sum) " +  //47

                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtuser_name,@txtemp_office_name," +  //5
                                               "@txtversion_id," +  //6
                                                 //=========================================================


                                               "@txtic_id," + // 7
                                               "@txtberg_type_id," + // 8
                                               "@txtwherehouse_id," + // 9
                                               "@txtmachine_id," + // 9

                                               "@txtemp1_id," + // 10
                                                "@txtemp1_name," + // 11
                                               "@txtemp_office_name_manager," + // 12
                                               "@txtemp_office_name_approve," + // 13
                                              "@txtproject_id," + // 14
                                               "@txtjob_id," + // 15
                                               "@txtic1_remark," + // 16

                                               "@txtcurrency_id," + // 17
                                               "@txtcurrency_date," + // 18
                                               "@txtcurrency_rate," + // 19

                                               "@txtmat_no," + // 19
                                               "@txtmat_id," + // 19
                                               "@txtmat_name," + // 19
                                               "@txtnumber_mat_id," + // 19

                                               "@txtacc_group_tax_id," + // 20

                                               "@txtcost_qty_balance_yokma," + // 25
                                               "@txtcost_qty_price_average_yokma," + // 26
                                               "@txtcost_money_sum_yokma," + // 27

                                               "@txtsum_qty," + // 28
                                               "@txtsum_price," + // 29
                                               "@txtsum_discount," + // 30
                                               "@txtmoney_sum," + // 31
                                               "@txtmoney_tax_base," + // 32
                                               "@txtvat_rate," + // 33
                                               "@txtvat_money," + // 34
                                               "@txtmoney_after_vat," + // 35
                                               "@txtmoney_after_vat_creditor," + // 36
                                               "@txtcreditor_status," + // 37

                                               "@txtcost_qty_balance_yokpai," + // 38
                                               "@txtcost_qty_price_average_yokpai," + // 39
                                               "@txtcost_money_sum_yokpai," + // 40

                                               "@txtcost_qty2_balance_yokma," + // 41
                                               "@txtsum2_qty," + // 42
                                               "@txtcost_qty2_balance_yokpai," + // 43

                                               "@txtic_status," +  //44
                                              "@txtpayment_status," +  //45
                                              "@txtacc_record_status," +  //46
                                              "@txtemp_print," +  //46
                                              "@txtemp_print_datetime," +  //46
                                              "@txtFG1_id," +  //46
                                              "@txtroll_sum)";   //47

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



                        cmd2.Parameters.Add("@txtic_id", SqlDbType.NVarChar).Value = this.txtic_id.Text.Trim();  //7
                        cmd2.Parameters.Add("@txtberg_type_id", SqlDbType.NVarChar).Value = this.PANEL0103_BERG_TYPE_txtberg_type_id.Text.Trim();  //8
                        cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.Trim();  //10
                        cmd2.Parameters.Add("@txtmachine_id", SqlDbType.NVarChar).Value = this.PANEL0102_MACHINE_txtmachine_id.Text.Trim();  //10

                    cmd2.Parameters.Add("@txtemp1_id", SqlDbType.NVarChar).Value = "";  //13
                        cmd2.Parameters.Add("@txtemp1_name", SqlDbType.NVarChar).Value = "";  //13
                        cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.Trim();  //13
                        cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.Trim();  //13


                        cmd2.Parameters.Add("@txtproject_id", SqlDbType.NVarChar).Value = "";  //14
                        cmd2.Parameters.Add("@txtjob_id", SqlDbType.NVarChar).Value = "";  //15
                        cmd2.Parameters.Add("@txtic1_remark", SqlDbType.NVarChar).Value = this.txtic_remark.Text.Trim();  //16

                        cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //17
                        cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //18
                        cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtcurrency_rate.Text.ToString()));  //19

                    //"@txtmat_no," + // 19
                    //"@txtmat_id," + // 19
                    //"@txtmat_name," + // 19
                    //"@txtnumber_mat_id," + // 19
                        cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.txtmat_no.Text.Trim();  //18
                        cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_id.Text.Trim();  //18
                         cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_name.Text.Trim();  //18
                        cmd2.Parameters.Add("@txtnumber_mat_id", SqlDbType.NVarChar).Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.Trim();  //18

                    cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //20

                        cmd2.Parameters.Add("@txtcost_qty_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString()));  //25
                        cmd2.Parameters.Add("@txtcost_qty_price_average_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));  //26
                        cmd2.Parameters.Add("@txtcost_money_sum_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString()));  //27

                        cmd2.Parameters.Add("@txtsum_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //28
                        cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //29
                        cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //30
                        cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //31
                        cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //32
                        cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //33
                        cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //34
                        cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //35
                        cmd2.Parameters.Add("@txtmoney_after_vat_creditor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //36
                        cmd2.Parameters.Add("@txtcreditor_status", SqlDbType.NVarChar).Value = "0";  //37

                        cmd2.Parameters.Add("@txtcost_qty_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString()));  //38
                        cmd2.Parameters.Add("@txtcost_qty_price_average_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString()));  //39
                        cmd2.Parameters.Add("@txtcost_money_sum_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString()));  //40

                        cmd2.Parameters.Add("@txtcost_qty2_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString()));  //41
                        cmd2.Parameters.Add("@txtsum2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));  //42
                        cmd2.Parameters.Add("@txtcost_qty2_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString()));  //43

                        cmd2.Parameters.Add("@txtic_status", SqlDbType.NVarChar).Value = "0";  //44
                        cmd2.Parameters.Add("@txtpayment_status", SqlDbType.NVarChar).Value = "";  //45
                        cmd2.Parameters.Add("@txtacc_record_status", SqlDbType.NVarChar).Value = "";  //46
                        cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //47
                        cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);//47
                        cmd2.Parameters.Add("@txtFG1_id", SqlDbType.NVarChar).Value = "";  //44
                        cmd2.Parameters.Add("@txtroll_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}",0));  //43

                    //=====================================================================================================================================================
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");



                    int s = 0;

                        for (int i = 0; i < this.GridView1.Rows.Count; i++)
                        {
                            s = i + 1;
                            if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                            {
                                this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();
                                if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                                {
                                    //===================================================================================================================
                                    //3 c002_01berg_produce_record_detail

                                    cmd2.CommandText = "INSERT INTO c002_01berg_produce_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                       "txttrans_year,txttrans_month,txttrans_day," +

                                       "txtic_id," +  //6
                                       "txttrans_date_server," +  //7

                                       "txtmat_no," +  //8
                                       "txtmat_id," +  //9
                                       "txtmat_name," +  //10
                                       "txtnumber_mat_id," +  //11

                                       "txtmat_unit1_name," +  //16
                                       "txtmat_unit1_qty," +  //17
                                        "chmat_unit_status," +  //18
                                       "txtmat_unit2_name," +  //19
                                       "txtmat_unit2_qty," +  //20

                                      "txtLot_no," +  //21
                                      "txtqty_after_cut," +  //21
                                      "txtqty," +  //21
                                      "txtqty2," +  //22

                                      "txtmachine_id," +  //23
                                      "txttrans_time_start," +  //24
                                      "txttrans_time_end," +  //25
                                      "txtwherehouse_id," +  //26
                                      "txtemp_id," +  //27
                                      "txtemp_name," +  //28
                                      "txtic_remark," +  //29

                                       "txtprice," +   //30
                                       "txtdiscount_rate," +  //31
                                       "txtdiscount_money," +  //32
                                       "txtsum_total," +  //33

                                      "txtcost_qty_balance_yokma," +  //34
                                      "txtcost_qty_price_average_yokma," +  //35
                                      "txtcost_money_sum_yokma," +  //36

                                      "txtcost_qty_balance_yokpai," +  //37
                                      "txtcost_qty_price_average_yokpai," +  //38
                                      "txtcost_money_sum_yokpai," +  //39

                                      "txtcost_qty2_balance_yokma," +  //40
                                      "txtcost_qty2_balance_yokpai," +  //41
                                      "txtqty_balance_yokpai," +  //42

                                      "txtitem_no," +  //43
                                      "txtqty_cut_yokma," +  //44
                                       "txtqty_cut_yokpai," +  //44
                                      "txtqty_after_cut_yokpai) " +  //45

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +
                                "'" +this.txtic_id.Text.Trim() + "'," +  //6
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'," +  //7

                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //8
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //9
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //10
                                "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Value.ToString() + "'," +    //11

                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //16
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString())) + "'," +  //17
                                "'" + this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() + "'," +  //18
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Value.ToString() + "'," +  //19
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString())) + "'," +  //20

                                "'" + this.GridView1.Rows[i].Cells["Col_txtLot_no"].Value.ToString() + "'," +  //16

                              "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) + "'," +  //21
                              "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //21
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString())) + "'," +  //22

                                "'" + this.GridView1.Rows[i].Cells["Col_txtmachine_id"].Value.ToString() + "'," +    //23
                                "'" + this.GridView1.Rows[i].Cells["Col_txttrans_time_start"].Value.ToString() + "'," +    //24
                                "'" + this.GridView1.Rows[i].Cells["Col_txttrans_time_end"].Value.ToString() + "'," +    //25
                                "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +    //26

                                "'" + this.GridView1.Rows[i].Cells["Col_txtemp_id"].Value.ToString() + "'," +    //27
                                "'" + this.GridView1.Rows[i].Cells["Col_txtemp_name"].Value.ToString() + "'," +    //28
                                "'" + this.GridView1.Rows[i].Cells["Col_txtic_remark"].Value.ToString() + "'," +    //29


                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //30
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //31
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //32
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //33

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + "'," +  //34
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString())) + "'," +  //35
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + "'," +  //36

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //37
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //38
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //39

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + "'," +  //40
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value.ToString())) + "'," +  //41
                                "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //42

                                "'" + this.GridView1.Rows[i].Cells["Col_txtitem_no"].Value.ToString() + "'," +    //43

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "')";   //45

                                    cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok3");

                              //  c003_receive_record_detail
                                //===================================================================================================================
                                // c003_receive_record_detail
                                cmd2.CommandText = "UPDATE c003_receive_record_detail SET " +
                                                   "txtcut_id = '" + this.txtic_id.Text.ToString() + "'," +
                                                   "txtqty_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +
                                                   "txtqty_after_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                    " AND (txtLot_no = '" + this.GridView1.Rows[i].Cells["Col_txtLot_no"].Value.ToString() + "')" +
                                                   " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString()  + "')";

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok4");


                            }
                        }
                        }



                    //สต๊อคสินค้า ตามคลัง =============================================================================================



                                //1.k021_mat_average
                                cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                                   "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +
                                                   "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +
                                                    "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +
                                                   "txtcost_qty2_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                                   " AND (txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.ToString() + "')";


                                cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok7");



                    //2.k021_mat_average_balance

                    cmd2.CommandText = "INSERT INTO k021_mat_average_balance(cdkey,txtco_id,txtbranch_id," +  //1
                               "txttrans_date_server,txttrans_time," +  //2
                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                               "txtcomputer_ip,txtcomputer_name," +  //4
                                "txtuser_name,txtemp_office_name," +  //5
                               "txtversion_id," +  //6
                                //====================================================

                                   "txtbill_id," +  //7
                                   "txtbill_type," +  //8
                                   "txtbill_remark," +  //9

                                   "txtwherehouse_id," +  //10
                                   "txtmat_no," +  //10
                                   "txtmat_id," +  //11
                                   "txtmat_name," +  //12
                                   "txtmat_unit1_name," +  //13

                                   "txtmat_unit1_qty," +  //14
                                   "chmat_unit_status," +  //15
                                   "txtmat_unit2_name," +  //16
                                   "txtmat_unit2_qty," +  //17

                                  "txtqty_in," +  //18
                                   "txtqty2_in," +  //19
                                  "txtprice_in," +   //20
                                   "txtsum_total_in," +  //21

                                  "txtqty_out," +  //22
                                  "txtqty2_out," +  //23
                                  "txtprice_out," +  //24
                                   "txtsum_total_out," +  //25

                                   "txtqty_balance," +  //26
                                   "txtqty2_balance," +  //27
                                  "txtprice_balance," +  //28
                                   "txtsum_total_balance," +  //29

                                   "txtitem_no) " +  //30

                            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                            "'" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "','" + myDateTime2.ToString("HH:mm:ss", UsaCulture) + "'," +  //2
                            "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", UsaCulture) + "'," +  //3
                            "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                            "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                            "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                              //=======================================================


                            "'" + this.txtic_id.Text.Trim() + "'," +  //7 txtbill_id
                            "'IC'," +  //9 txtbill_type
                            "'" + this.PANEL0103_BERG_TYPE_txtberg_type_name.Text.Trim() + "'," +  //9 txtbill_remark

                             "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                           "'" + this.txtmat_no.Text + "'," +  //10 
                            "'" + this.PANEL_MAT_txtmat_id.Text.ToString() + "'," +  //11
                            "'" + this.PANEL_MAT_txtmat_name.Text.ToString() + "'," +    //12

                            "'" + this.txtmat_unit1_name.Text.ToString() + "'," +  //13
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString())) + "'," +  //14
                            "'" + this.chmat_unit_status.Text.ToString() + "'," +  //15
                            "'" + this.txtmat_unit2_name.Text.ToString() + "'," +  //16
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +  //17

                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19 txtqty2_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //20 txtprice_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //21 txtsum_total_in

                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) + "'," +  //22 txtqty_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString())) + "'," +  //23 txtqty2_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtprice.Text.ToString())) + "'," +  //24 txtprice_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +  //26
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'," +  //27
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +  //28
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +  //29

                           "'1')";   //30

                                cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok8");


                    //======================================

                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    //MessageBox.Show("ok4");


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

                        if (this.iblword_status.Text.Trim() == "บันทึกใบเบิกวัตถุดิบผลิต (ด้าย)")
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
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtic_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_02Berg_Produce_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_02Berg_Produce_record_print();
            frm2.Show();
            frm2.BringToFront();
            //====================

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtic_id.Text.Trim();
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
                // rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_c002_01berg_produce_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_01berg_produce_record.rpt");


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
                rpt.SetParameterValue("txtic_id", W_ID_Select.TRANS_ID.Trim());

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
            //============================
        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

        }

        //====================
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


        //===============================================

        //1.ส่วนหน้าหลัก ตารางสำหรับบันทึก========================================================================
        //DateTimePicker dtp1 = new DateTimePicker();
        //Rectangle _Rectangle1;
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow = 0;

        private void btnGo1_Click(object sender, EventArgs e)
        {

                if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
                {
                    MessageBox.Show("โปรด เลือก คลังสินค้าที่เบิก ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (this.PANEL1306_WH.Visible == false)
                    {
                        this.PANEL1306_WH.Visible = true;
                        this.PANEL1306_WH.BringToFront();
                        this.PANEL1306_WH.Location = new Point(this.PANEL1306_WH_txtwherehouse_name.Location.X, this.PANEL1306_WH_txtwherehouse_name.Location.Y + 22);
                    }
                    else
                    {
                        this.PANEL1306_WH.Visible = false;
                    }
                    return;

                }
                else
                {

                }

                Show_GO1();



        }
        private void Show_GO1()
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

            Clear_GridView1();

            //===========================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c003_receive_record.*,c003_receive_record_detail.*," +
                                    "b001mat.*," +
                                    //"k021_mat_average.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +

                                    " FROM c003_receive_record" +

                                    " INNER JOIN c003_receive_record_detail" +
                                    " ON c003_receive_record.cdkey = c003_receive_record_detail.cdkey" +
                                    " AND c003_receive_record.txtco_id = c003_receive_record_detail.txtco_id" +
                                    " AND c003_receive_record.txtCRG_id = c003_receive_record_detail.txtCRG_id" +

                                    " INNER JOIN b001mat" +
                                    " ON c003_receive_record_detail.cdkey = b001mat.cdkey" +
                                    " AND c003_receive_record_detail.txtco_id = b001mat.txtco_id" +
                                    " AND c003_receive_record_detail.txtmat_id = b001mat.txtmat_id" +

                                    //" INNER JOIN k021_mat_average" +
                                    //" ON b001mat.cdkey = k021_mat_average.cdkey" +
                                    //" AND b001mat.txtco_id = k021_mat_average.txtco_id" +
                                    //" AND b001mat.txtmat_id = k021_mat_average.txtmat_id" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON c003_receive_record_detail.cdkey = b001mat_02detail.cdkey" +
                                    " AND c003_receive_record_detail.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND c003_receive_record_detail.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_06price_sale" +
                                    " ON c003_receive_record_detail.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND c003_receive_record_detail.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND c003_receive_record_detail.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " WHERE (c003_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (c003_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (c003_receive_record.txtcrg_status = '0')" +
                                    " AND (c003_receive_record_detail.txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +

                                    //" AND (c003_receive_record.txtCRG_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                    //" AND (c003_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " AND (c003_receive_record_detail.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                    " AND (c003_receive_record_detail.txtqty_after_cut > 0)" +
                                    " ORDER BY c003_receive_record_detail.txtLot_no ASC";


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
                            this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                            this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                            this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                            this.txtmat_unit1_name.Text = dt2.Rows[0]["txtmat_unit1_name"].ToString();
                            this.txtmat_unit1_qty.Text = dt2.Rows[0]["txtmat_unit1_qty"].ToString();
                            this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                            this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                            this.txtmat_unit2_qty.Text = dt2.Rows[0]["txtmat_unit2_qty"].ToString();

                            //   string[] row = new string[] { k.ToString(), "", "", "", this.PANEL1306_WH_txtwherehouse_id.Text, this.PANEL_MAT_txtmat_id.Text.ToString(), this.txtnumber_mat_id.Text.ToString() };
                            //======================================================
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0

                            GridView1.Rows[index].Cells["Col_txtmachine_id"].Value = this.PANEL0102_MACHINE_txtmachine_id.Text.ToString();      //1
                            GridView1.Rows[index].Cells["Col_txttrans_time_start"].Value = "";      //2
                            GridView1.Rows[index].Cells["Col_txttrans_time_end"].Value = "";      //3
                            GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //4

                            //GridView1.Rows[index].Cells["Col_txtmat_unit1_qty_krasob"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty_krasob"]).ToString("###,###.00");      //5
                            //GridView1.Rows[index].Cells["Col_txtmat_unit1_qty_krasob_convert"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty_krasob_convert"]).ToString("###,###.00");      //6
                            //GridView1.Rows[index].Cells["Col_txtmat_unit1_qty_loud"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty_loud"]).ToString("###,###.00");      //7
                            //GridView1.Rows[index].Cells["Col_txtmat_unit1_qty_loud_convert"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty_loud_convert"]).ToString("###,###.00");      //8


                            GridView1.Rows[index].Cells["Col_txtemp_id"].Value = "";      //9
                            GridView1.Rows[index].Cells["Col_txtemp_name"].Value = "";      //10
                            GridView1.Rows[index].Cells["Col_txtic_remark"].Value = "";      //11

                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //12
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //13
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //14
                            GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString();      //15

                            GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //16
                            GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //17

                            GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //18

                            GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //19
                            GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //20


                            GridView1.Rows[index].Cells["Col_txtLot_no"].Value = dt2.Rows[j]["txtLot_no"].ToString();      //12
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_cut"]).ToString("###,###.00");      //21
                            GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //21
                            GridView1.Rows[index].Cells["Col_txtqty2"].Value = "0";      //22


                            GridView1.Rows[index].Cells["Col_txtprice"].Value = "0";         //23
                            GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";      //24
                            GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";       //25
                            GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0";       //26

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";       //27
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";       //28
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //29

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //30
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";       //31
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //32

                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";      //33
                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";       //34

                            GridView1.Rows[index].Cells["Col_txtitem_no"].Value = "";      //35

                            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_cut"]).ToString("###,###.00");       //37
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_cut"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokpai"].Value = "0";      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";      //37

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;
                        //======================================================

                        //=======================================================


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

            Show_Qty_Yokma();
            GridView1_Color_Column();
            GridView1_Up_Status();
            //GridView1_Cal_Sum();

            //Fill_cboemp();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 39;
            this.GridView1.Columns[0].Name = "Col_Auto_num";

            this.GridView1.Columns[1].Name = "Col_txtmachine_id";
            this.GridView1.Columns[2].Name = "Col_txttrans_time_start";
            this.GridView1.Columns[3].Name = "Col_txttrans_time_end";
            this.GridView1.Columns[4].Name = "Col_txtwherehouse_id";

            this.GridView1.Columns[5].Name = "Col_txtemp_id";
            this.GridView1.Columns[6].Name = "Col_txtemp_name";
            this.GridView1.Columns[7].Name = "Col_txtic_remark";

            this.GridView1.Columns[8].Name = "Col_txtmat_no";
            this.GridView1.Columns[9].Name = "Col_txtmat_id";
            this.GridView1.Columns[10].Name = "Col_txtmat_name";
            this.GridView1.Columns[11].Name = "Col_txtnumber_mat_id";

            this.GridView1.Columns[12].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[13].Name = "Col_txtmat_unit1_qty";
            this.GridView1.Columns[14].Name = "Col_chmat_unit_status";
            this.GridView1.Columns[15].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[16].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[17].Name = "Col_txtLot_no";
            this.GridView1.Columns[19].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[20].Name = "Col_txtqty";
            this.GridView1.Columns[21].Name = "Col_txtqty2";

            this.GridView1.Columns[22].Name = "Col_txtprice";
            this.GridView1.Columns[23].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[24].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[25].Name = "Col_txtsum_total";

            this.GridView1.Columns[26].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[27].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[28].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[29].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[30].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[31].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[32].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns[33].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView1.Columns[34].Name = "Col_txtitem_no";

            this.GridView1.Columns[35].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[36].Name = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[37].Name = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[38].Name = "Col_txtqty_after_cut_yokpai";
            this.GridView1.Columns[35].Visible = false;
            this.GridView1.Columns[36].Visible = false;
            this.GridView1.Columns[37].Visible = false;
            this.GridView1.Columns[38].Visible = false;


            this.GridView1.Columns[0].HeaderText = "No";

            this.GridView1.Columns[1].HeaderText = "เครื่องจักร";
            this.GridView1.Columns[2].HeaderText = " เวลา";
            this.GridView1.Columns[3].HeaderText = " เวลาเสร็จ";
            this.GridView1.Columns[4].HeaderText = " คลัง";

            this.GridView1.Columns[5].HeaderText = "รหัสผู้เบิก";
            this.GridView1.Columns[6].HeaderText = "ชื่อผู้เบิก";
            this.GridView1.Columns[7].HeaderText = "หมายเหตุ";

            this.GridView1.Columns[8].HeaderText = "ลำดับ";
            this.GridView1.Columns[9].HeaderText = "รหัส";
            this.GridView1.Columns[10].HeaderText = "ชื่อวัตถุดิบ";
            this.GridView1.Columns[11].HeaderText = "เบอร์เส้นด้าย";

            this.GridView1.Columns[12].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[13].HeaderText = " หน่วย";
            this.GridView1.Columns[14].HeaderText = "แปลง";
            this.GridView1.Columns[15].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[16].HeaderText = " หน่วย";

            this.GridView1.Columns[17].HeaderText = "Lot No";
            this.GridView1.Columns[19].HeaderText = "เหลือในสต๊อค กก.";
            this.GridView1.Columns[20].HeaderText = "เบิก กก.";
            this.GridView1.Columns[21].HeaderText = "เบิก(ปอนด์)";

            this.GridView1.Columns[22].HeaderText = "ราคา";
            this.GridView1.Columns[23].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[24].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[25].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[26].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[27].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[28].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[29].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[30].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[31].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[32].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView1.Columns[33].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView1.Columns[34].HeaderText = "item";
            this.GridView1.Columns[35].HeaderText = "รวมจำนวนตัดแล้วยกมา";
            this.GridView1.Columns[36].HeaderText = "รวมจำนวนตัดแล้วยกไป";
            this.GridView1.Columns[37].HeaderText = "เหลือรอเบิกอีก กก.";

            this.GridView1.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 0;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView1.Columns["Col_txtmachine_id"].Width = 80;
            this.GridView1.Columns["Col_txtmachine_id"].ReadOnly = false;
            this.GridView1.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //DataGridViewTextBoxColumn txttime = new DataGridViewTextBoxColumn();
            //txttime.Name = "Col_Time1";
            //txttime.Width =60;
            //txttime.DisplayIndex = 2;
            //txttime.HeaderText = "เวลา";
            //txttime.DefaultCellStyle.Format = ("HH:mm");
            ////cboemp.MaxDropDownItems = 100;
            ////cboemp.ReadOnly = false;
            //txttime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //txttime.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            //txttime.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //this.GridView1.Columns.Add(txttime);

            this.GridView1.Columns["Col_txttrans_time_start"].Visible = true;  //"Col_txttrans_time_start";
            this.GridView1.Columns["Col_txttrans_time_start"].Width = 80;
            this.GridView1.Columns["Col_txttrans_time_start"].ReadOnly = false;

            this.GridView1.Columns["Col_txttrans_time_start"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time_start"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txttrans_time_end"].Visible = false;  //"Col_txttrans_time_end";
            this.GridView1.Columns["Col_txttrans_time_end"].Width = 0;
            this.GridView1.Columns["Col_txttrans_time_end"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_time_end"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time_end"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView1.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //DataGridViewComboBoxColumn cboemp = new DataGridViewComboBoxColumn();
            //cboemp.Name = "Col_Combo1";
            //cboemp.Width = 200;
            //cboemp.DisplayIndex = 21;
            //cboemp.HeaderText = "ผู้เบิก...";
            ////cboemp.MaxDropDownItems = 100;
            ////cboemp.ReadOnly = false;
            //cboemp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //cboemp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            //cboemp.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //this.GridView1.Columns.Add(cboemp);

            this.GridView1.Columns["Col_txtemp_id"].Visible = false;  //"Col_txtemp_id";
            this.GridView1.Columns["Col_txtemp_id"].Width = 0;
            this.GridView1.Columns["Col_txtemp_id"].ReadOnly = false;
            this.GridView1.Columns["Col_txtemp_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtemp_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtemp_name"].Visible = true;  //"Col_txtemp_name";
            this.GridView1.Columns["Col_txtemp_name"].Width = 150;
            this.GridView1.Columns["Col_txtemp_name"].ReadOnly = false;
            this.GridView1.Columns["Col_txtemp_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtemp_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtic_remark"].Visible = true;  //"Col_txtic_remark";
            this.GridView1.Columns["Col_txtic_remark"].Width = 250;
            this.GridView1.Columns["Col_txtic_remark"].ReadOnly = false;
            this.GridView1.Columns["Col_txtic_remark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtic_remark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";
            this.GridView1.Columns["Col_txtmat_no"].Width = 0;
            this.GridView1.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 80;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 150;
            this.GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = true;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 100;
            this.GridView1.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView1.Columns["Col_txtmat_unit1_name"].Width = 80;
            this.GridView1.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.GridView1.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView1.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView1.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView1.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 70;
            dgvCmb.DisplayIndex = 15;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.Visible = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb);

            this.GridView1.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.GridView1.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView1.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtLot_no"].Visible = true;  //"Col_txtLot_no";
            this.GridView1.Columns["Col_txtLot_no"].Width = 140;
            this.GridView1.Columns["Col_txtLot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtLot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtLot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[18].Visible = false;
            this.GridView1.Columns[19].Visible = false;
            DataGridViewCheckBoxColumn dgvCmb_SELECT = new DataGridViewCheckBoxColumn();
            dgvCmb_SELECT.Name = "Col_Chk_SELECT";
            dgvCmb_SELECT.Width = 120;  //70
            dgvCmb_SELECT.DisplayIndex = 19;
            dgvCmb_SELECT.HeaderText = "เลือกเบิก";
            dgvCmb_SELECT.ValueType = typeof(bool);
            dgvCmb_SELECT.ReadOnly = false;
            dgvCmb_SELECT.Visible = false;
            dgvCmb_SELECT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb_SELECT);


            this.GridView1.Columns["Col_txtqty_after_cut"].Visible = true;  //"Col_txtqty_after_cut";
            this.GridView1.Columns["Col_txtqty_after_cut"].Width = 110;
            this.GridView1.Columns["Col_txtqty_after_cut"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width =110;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty2"].Visible = true;  //"Col_txtqty2";
            this.GridView1.Columns["Col_txtqty2"].Width =110;
            this.GridView1.Columns["Col_txtqty2"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView1.Columns["Col_txtprice"].Visible = false;  //"Col_txtprice";
            this.GridView1.Columns["Col_txtprice"].Width = 0;
            this.GridView1.Columns["Col_txtprice"].ReadOnly = true;
            this.GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.GridView1.Columns["Col_txtdiscount_rate"].Width = 0;
            this.GridView1.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.GridView1.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_money"].Visible = false;  //"Col_txtdiscount_money";
            this.GridView1.Columns["Col_txtdiscount_money"].Width = 0;
            this.GridView1.Columns["Col_txtdiscount_money"].ReadOnly = false;
            this.GridView1.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_total"].Visible = false;  //"Col_txtsum_total";
            this.GridView1.Columns["Col_txtsum_total"].Width = 0;
            this.GridView1.Columns["Col_txtsum_total"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].Visible = false;  //"Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].Visible = false;  //"Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].Visible = false;  //"Col_txtcost_money_sum_yokma";
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].Visible = false;  //"Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].Visible = false;  //"Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].Visible = false;  //"Col_txtcost_money_sum_yokpai";
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].Visible = false;  //"Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].Visible = false;  //"Col_txtcost_qty2_balance_yokpai";
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtitem_no"].Visible = false;  //"Col_txtitem_no";
            this.GridView1.Columns["Col_txtitem_no"].Width = 0;
            this.GridView1.Columns["Col_txtitem_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtitem_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtitem_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty_cut_yokma"].Visible = false;  //"Col_txtqty_cut_yokma";
            this.GridView1.Columns["Col_txtqty_cut_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty_cut_yokpai"].Visible = false;  //"Col_txtqty_cut_yokpai";
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].Visible = true;  //"Col_txtqty_after_cut_yokpai";
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].Width = 140;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[38].Visible = false;

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
            //DateTimePicker dtp1 = new DateTimePicker();

            selectedRowIndex = GridView1.CurrentRow.Index;
            //    this.btnremove_row.Visible = true;

            switch (GridView1.Columns[e.ColumnIndex].Name)
            {
                //case "Col_txtmat_no":
                //    dtp1.Visible = false;
                //    break;
                //case "Col_txtmat_id":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtmat_name":
                //    dtp1.Visible = false;
                //        break;
                //case "Col_txtmat_unit1_name":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtmat_unit1_qty":
                //    dtp1.Visible = false;
                //     break;
                //case "Col_chmat_unit_status":
                //    dtp1.Visible = false;
                //    break;
                //case "Col_txtmat_unit2_name":
                //    dtp1.Visible = false;
                //    break;
                //case "Col_txtmat_unit2_qty":
                //    dtp1.Visible = false;
                //        break;
                //case "Col_txtic_remark":
                //    dtp1.Visible = false;
                //       break;
                //case "Col_Combo1":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtqty":
                //    dtp1.Visible = false;
                //        break;
                //case "Col_txtqty2":
                //    dtp1.Visible = false;
                //    break;
                //case "Col_txtmachine_id":
                //    dtp1.Visible = false;
                //    break;
                //case "Col_txttrans_time_start":
                //    //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_unit1_qty_krasob"].Value.ToString())) > 0)
                //    //{
                //        //GridView1.Controls.Add(dtp1);
                //        //dtp1.Visible = false;
                //        //dtp1.ShowUpDown = true;
                //        //dtp1.CustomFormat = "HH:mm";
                //        //dtp1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;

                //        //dtp1.TextChanged += new EventHandler(dtp1_TextChange);

                //        //this.GridView1.Columns[18].Name = "Col_txtmade_receive_date";
                //        //_Rectangle1 = GridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                //        //dtp1.Size = new Size(_Rectangle1.Width, _Rectangle1.Height); //  
                //        //dtp1.Location = new Point(_Rectangle1.X, _Rectangle1.Y); //  

                //        //dtp1.Visible = true;
                //        //this.GridView1.CurrentCell.Value = dtp1.Text.ToString();
                //        //dtp1.CloseUp += new EventHandler(dtp1_CloseUp);

                //        //GridView1_Cal_Sum();
                //        //Sum_group_tax();
                //    dtp1.Visible = false;
                //    break;

                //    //}
                //    //else
                //    //{
                //    //    dtp1.Visible = false;
                //    //}
                //    //break;
                //case "Col_txtprice":
                //    dtp1.Visible = false;
                //    break;
                //case "Col_txtdiscount_rate":
                //    dtp1.Visible = false;
                //     break;
                //case "Col_txtdiscount_money":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtsum_total":
                //    dtp1.Visible = false;
                //   break;
                //case "Col_txtcost_qty_balance_yokma":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtcost_qty_price_average_yokma":
                //    dtp1.Visible = false;
                //     break;
                //case "Col_txtcost_money_sum_yokma":
                //    dtp1.Visible = false;
                //     break;
                //case "Col_txtcost_qty_balance_yokpai":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtcost_qty_price_average_yokpai":
                //    dtp1.Visible = false;
                //      break;
                //case "Col_txtcost_money_sum_yokpai":
                //    dtp1.Visible = false;
                //     break;
                //case "Col_txtcost_qty2_balance_yokma":
                //    dtp1.Visible = false;
                //     break;
                //case "Col_txtcost_qty2_balance_yokpai":
                //    dtp1.Visible = false;
                //    break;

            }
        }
        private void GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //We make DataGridCheckBoxColumn commit changes with single click
            //use index of logout column
            this.GridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //Check the value of cell
            if (Convert.ToBoolean(this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value) == true)
            {

                //Use index of TimeOut column
                GridView1_Cal_Sum();
                Sum_group_tax();

                //Set other columns values
            }
            else
            {
                //Use index of TimeOut column
                GridView1_Cal_Sum();
                Sum_group_tax();

                //Set other columns values
            }
        }
        private void GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow = GridView1.CurrentRow.Index;
            int rowscount = GridView1.Rows.Count;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            //===============================================================
            if (this.GridView1.Rows.Count > 0)
            {
                //===============================================================
                for (int i = 0; i < this.GridView1.Rows.Count - 1; i++)
                {

                    if (GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                    {
                        if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty_krasob"].Value.ToString())) > 0)
                        {
                            GridView1.Rows[i].Cells["Col_txtmachine_id"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmachine_id"].Style.Font = new Font("Tahoma", 12F);

                           // GridView1.Rows[i].Cells["Col_txttrans_time_start"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txttrans_time_start"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txttrans_time_end"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txttrans_time_end"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtmat_no"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_no"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtmat_id"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_id"].Style.Font = new Font("Tahoma", 12F);


                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.Font = new Font("Tahoma", 12F);


                            GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Style.Font = new Font("Tahoma", 12F);

                          //  GridView1.Rows[i].Cells["Col_txtmat_unit1_qty_krasob"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit1_qty_krasob"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.Font = new Font("Tahoma", 12F);







                            GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Style.Font = new Font("Tahoma", 12F);


                            GridView1.Rows[i].Cells["Col_chmat_unit_status"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_chmat_unit_status"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtqty_want"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty_want"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtqty_balance"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty_balance"].Style.Font = new Font("Tahoma", 12F);

                            //GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtqty2"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty2"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtprice"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtprice"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtdiscount_money"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtdiscount_money"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtsum_total"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtsum_total"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtwant_receive_date"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtwant_receive_date"].Style.Font = new Font("Tahoma", 12F);

                            //GridView1.Rows[i].Cells["Col_txtmade_receive_date"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmade_receive_date"].Style.Font = new Font("Tahoma", 12F);

                            //GridView1.Rows[i].Cells["Col_txtexpire_receive_date"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtexpire_receive_date"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Style.Font = new Font("Tahoma", 12F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Style.Font = new Font("Tahoma", 12F);
                        }
                        else
                        {
                            GridView1.Rows[i].Cells["Col_txtmat_no"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_no"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtmat_id"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_id"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.ForeColor = Color.Black;
                            GridView1.Rows[i].Cells["Col_txtmat_name"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Style.Font = new Font("Tahoma", 8F);


                            GridView1.Rows[i].Cells["Col_chmat_unit_status"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_chmat_unit_status"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtqty_want"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty_want"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtqty_balance"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty_balance"].Style.Font = new Font("Tahoma", 8F);

                            //GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtqty2"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtqty2"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtprice"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtprice"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtdiscount_money"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtdiscount_money"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtsum_total"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtsum_total"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtwant_receive_date"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtwant_receive_date"].Style.Font = new Font("Tahoma", 8F);

                            //GridView1.Rows[i].Cells["Col_txtmade_receive_date"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtmade_receive_date"].Style.Font = new Font("Tahoma", 8F);

                            //GridView1.Rows[i].Cells["Col_txtexpire_receive_date"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtexpire_receive_date"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Style.Font = new Font("Tahoma", 8F);

                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Style.BackColor = Color.White;
                            GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Style.Font = new Font("Tahoma", 8F);
                        }


                    }
                }
            }
            //===============================================================

            //======================================
            if (GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.BackColor == Color.LightGoldenrodYellow)
            {

                GridView1.Rows[curRow].Cells["Col_txtmat_no"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_no"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtmat_id"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_id"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.ForeColor = Color.Black;
                GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_name"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_name"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_qty"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_qty"].Style.Font = new Font("Tahoma", 8F);


                GridView1.Rows[curRow].Cells["Col_chmat_unit_status"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_chmat_unit_status"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_name"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_name"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_qty"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_qty"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtqty_want"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtqty_want"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtqty_balance"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtqty_balance"].Style.Font = new Font("Tahoma", 8F);

                //GridView1.Rows[curRow].Cells["Col_txtqty"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtqty"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtqty2"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtqty2"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtprice"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtprice"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtdiscount_rate"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtdiscount_rate"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtdiscount_money"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtdiscount_money"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtsum_total"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtsum_total"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtwant_receive_date"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtwant_receive_date"].Style.Font = new Font("Tahoma", 8F);

                //GridView1.Rows[curRow].Cells["Col_txtmade_receive_date"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtmade_receive_date"].Style.Font = new Font("Tahoma", 8F);

                //GridView1.Rows[curRow].Cells["Col_txtexpire_receive_date"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtexpire_receive_date"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokma"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokma"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokma"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokma"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokma"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokma"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokpai"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokpai"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokpai"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokpai"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokpai"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokpai"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokma"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokma"].Style.Font = new Font("Tahoma", 8F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokpai"].Style.BackColor = Color.White;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokpai"].Style.Font = new Font("Tahoma", 8F);
            }
            else
            {
                GridView1.Rows[curRow].Cells["Col_txtmat_no"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_no"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtmat_id"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_id"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.ForeColor = Color.Red; ;
                GridView1.Rows[curRow].Cells["Col_txtmat_name"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_name"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_qty"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit1_qty"].Style.Font = new Font("Tahoma", 12F);


                GridView1.Rows[curRow].Cells["Col_chmat_unit_status"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_chmat_unit_status"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_name"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_qty"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmat_unit2_qty"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtqty_want"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtqty_want"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtqty_balance"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtqty_balance"].Style.Font = new Font("Tahoma", 12F);

                //GridView1.Rows[curRow].Cells["Col_txtqty"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtqty"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtqty2"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtqty2"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtprice"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtprice"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtdiscount_rate"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtdiscount_rate"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtdiscount_money"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtdiscount_money"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtsum_total"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtsum_total"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtwant_receive_date"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtwant_receive_date"].Style.Font = new Font("Tahoma", 12F);

                //GridView1.Rows[curRow].Cells["Col_txtmade_receive_date"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtmade_receive_date"].Style.Font = new Font("Tahoma", 12F);

                //GridView1.Rows[curRow].Cells["Col_txtexpire_receive_date"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtexpire_receive_date"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokma"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokma"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokma"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokma"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokma"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokma"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokpai"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_balance_yokpai"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokpai"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty_price_average_yokpai"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokpai"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_money_sum_yokpai"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokma"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokma"].Style.Font = new Font("Tahoma", 12F);

                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokpai"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[curRow].Cells["Col_txtcost_qty2_balance_yokpai"].Style.Font = new Font("Tahoma", 12F);
            }
            //======================================



        }
        private void GridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            //dtp1.Visible = false;
        }
        private void GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            //dtp1.Visible = false;
        }
        private void GridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }
        private void GridView1_KeyDown(object sender, KeyEventArgs e)
        {

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
        private void dtp1_TextChange(object sender, EventArgs e)
        {
            // GridView1.CurrentCell.Value = dtp1.Value.ToString("yyyy-MM-dd", UsaCulture);
            //GridView1.CurrentCell.Value = dtp1.Value.ToString("HH:mm");
        }
        private void dtp1_CloseUp(object sender, EventArgs e)
        {
            //dtp1.Visible = false;
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
                GridView1.Rows[i].Cells["Col_txttrans_time_start"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txttrans_time_end"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtemp_id"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtemp_name"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtic_remark"].Style.BackColor = Color.LightSkyBlue;

                GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.LightSkyBlue;

            }
        }
        private void GridView1_Cal_Sum()
        {

            double QTY_KRASOB = 0;
            double QTY_LOAD = 0;
            double QTY_KRASOB_LOAD = 0;

            double Sum2_Qty_Yokpai = 0;
            double Sum_Qty = 0;
            double Sum2_Qty = 0;
            double Con_QTY = 0;

            double QAbyma = 0;
            double QAbyma2 = 0;
            double Qbypai = 0;
            double Qbypai2 = 0;
            double Mbypai = 0;
            double QAbypai = 0;

            double Sum_Qty_K = 0;
            double Sum_Qty_K_KG = 0;
            double Sum_Qty_L = 0;
            double Sum_Qty_L_KG = 0;

            double Sum_Qty_CUT_Yokpai = 0;
            double Sum_Qty_AF_CUT_Yokpai = 0;


            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value = k.ToString();
                    }
 
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = ".0000";
                    }

                    if (this.GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty2"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = ".00";

                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtprice"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtprice"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = ".00";

                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";

                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";

                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";
                    }

 
                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                           //Sum_Qty  จำนวนเบิก (กก)=================================================
                            Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                            this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                            //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                            Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                            this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));

                            //============================================================================================================
                            //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                            if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                            {
                                Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                                this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                                //Sum2_Qty_Yokpai  =================================================
                                Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                                this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));
                            }

                        //แล้ว เท่าไร = 0     ปกติ บวก =================================================
                        Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //เหลืออีก เท่าไร     ปกติ ลบ  =================================================
                        Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    }

                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                    this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                    this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                    QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                    this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                    //1.เหลือยกมา - เบิก = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา - มูลค่าเบิก = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString()));
                    this.txtcost_money_sum_yokpai.Text = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) / Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString()));
                        this.txtcost_qty_price_average_yokpai.Text = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.txtcost_qty_price_average_yokpai.Text = "0";
                    }

                    //1.เหลือ(2)ยกมา - เบิก(2) = จำนวนเหลือ(2)ทั้งสิ้น
                    Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));
                    this.txtcost_qty2_balance_yokpai.Text = Qbypai2.ToString("N", new CultureInfo("en-US"));

                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                }
            }

            this.txtcount_rows.Text = k.ToString();

            QTY_KRASOB = 0;
            QTY_LOAD = 0;
            QTY_KRASOB_LOAD = 0;

            Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            QAbyma = 0;
            QAbyma2 = 0;
            Qbypai = 0;
            Qbypai2 = 0;
            Mbypai = 0;
            QAbypai = 0;

            Sum_Qty_K = 0;
            Sum_Qty_K_KG = 0;
            Sum_Qty_L = 0;
           Sum_Qty_L_KG = 0;

            Sum_Qty_CUT_Yokpai = 0;
            Sum_Qty_AF_CUT_Yokpai = 0;

        }
        private void Show_Qty_Yokma()
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


                cmd2.CommandText = "SELECT *" +
                                   " FROM k021_mat_average" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                   " AND (txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                  " ORDER BY txtmat_no ASC";

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

                            this.txtcost_qty_balance_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                            this.txtcost_qty_price_average_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");        //19
                            this.txtcost_money_sum_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");        //20
                            this.txtcost_qty2_balance_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00");        //24

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




            //==============================================

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                //if (this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() == this.GridView66.Rows[selectedRowIndex].Cells["Col_txtlot_no"].Value.ToString())
                //{
                //    MessageBox.Show("Lot No นี้ เพิ่มเข้าไปใน ตารางแล้ว ");
                //    return;
                //}
                //if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() == this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_id"].Value.ToString())
                //{

                //}
                //else
                //{
                //    MessageBox.Show("ระบบจะให้ส่งตัดผ้าพับ ได้ที่ละ 1 รหัสผ้าพับ ต่อ 1 ใบส่งตัด เท่านั้น !! ");
                //    return;
                //}
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd2 = conn.CreateCommand();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = conn;


                    cmd2.CommandText = "SELECT *" +
                                       " FROM c003_receive_record_detail" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                       " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')" +
                                      " ORDER BY txtmat_no ASC";

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

                                GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_cut"]).ToString("###,###.00");          //21
                                GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_cut"]).ToString("###,###.00");    //36
                                //GridView1.Rows[j].Cells["Col_txtqty_cut_yokpai"].Value = "0";      //37
                                //GridView1.Rows[j].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";      //37


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


            }
            //==============================================

        }
        private void GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //Active
                {
                    this.GridView1.Rows[i].Cells["Col_Chk1"].Value = true;
                }
                else
                {
                    this.GridView1.Rows[i].Cells["Col_Chk1"].Value = false;

                }
            }
        }
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.GridView1.CurrentCell.ColumnIndex == 8)
            {
                if (Convert.ToBoolean(this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value) == false)
                {
                    this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value = true;
                    //this.GridView1.Rows[selectedRowIndex].Cells["Col_txtsum_qty_pub"].Value = "1";
                    //this.textBox1.Text = "1";
                    GridView1_Cal_Sum();
                    Sum_group_tax();

                }
                else
                {
                    this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value = false;
                    //this.GridView1.Rows[selectedRowIndex].Cells["Col_txtsum_qty_pub"].Value = "0";
                    //this.textBox1.Text = "0";
                    GridView1_Cal_Sum();
                    Sum_group_tax();

                }
            }
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
                DisCount = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_IN") //ซื้อคิดvatรวม
            {
                double DisCount = 0;
                double VATMONey = 0;
                double VATBASE = 0;
                double VATA = 0;

                //รวมทั้งสิ้น
                DisCount = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text));
                this.txtmoney_after_vat.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                VATA = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + 100;

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text)) * Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) / Convert.ToDouble(string.Format("{0:n4}", VATA));
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //ฐานภาษี
                VATBASE = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text));
                this.txtmoney_tax_base.Text = VATBASE.ToString("N", new CultureInfo("en-US"));


            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_ONvat")  //ซื้อไม่มีvat
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                this.txtvat_rate.Text = "0";

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));


            }
        }





        //====================


        //txtwherehouse คลังสินค้า  =======================================================================
        private void PANEL1306_WH_Fill_wherehouse()
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

            PANEL1306_WH_Clear_GridView1_wherehouse();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_06wherehouse" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtwherehouse_id <> '')" +
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
                            var index = PANEL1306_WH_dataGridView1_wherehouse.Rows.Add();
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //2
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_txtwherehouse_name_eng"].Value = dt2.Rows[j]["txtwherehouse_name_eng"].ToString();      //3
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
        private void PANEL1306_WH_GridView1_wherehouse()
        {
            this.PANEL1306_WH_dataGridView1_wherehouse.ColumnCount = 4;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[0].Name = "Col_Auto_num";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].Name = "Col_txtwherehouse_id";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].Name = "Col_txtwherehouse_name";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Name = "Col_txtwherehouse_name_eng";

            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[0].HeaderText = "No";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].HeaderText = "รหัส";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].HeaderText = " ชื่อคลังสินค้า ";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].HeaderText = " ชื่อคลังสินค้า  Eng";

            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[0].Visible = false;  //"No";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].Visible = true;  //"Col_txtwherehouse_id";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].Width = 100;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].ReadOnly = true;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].Visible = true;  //"Col_txtwherehouse_name";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].Width = 150;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].ReadOnly = true;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Visible = true;  //"Col_txtwherehouse_name_eng";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Width = 150;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].ReadOnly = true;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1306_WH_dataGridView1_wherehouse.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1306_WH_dataGridView1_wherehouse.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1306_WH_dataGridView1_wherehouse.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1306_WH_dataGridView1_wherehouse.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1306_WH_dataGridView1_wherehouse.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1306_WH_dataGridView1_wherehouse.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1306_WH_dataGridView1_wherehouse.EnableHeadersVisualStyles = false;

        }
        private void PANEL1306_WH_Clear_GridView1_wherehouse()
        {
            this.PANEL1306_WH_dataGridView1_wherehouse.Rows.Clear();
            this.PANEL1306_WH_dataGridView1_wherehouse.Refresh();
        }
        private void PANEL1306_WH_txtwherehouse_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1306_WH.Visible == false)
                {
                    this.PANEL1306_WH.Visible = true;
                    this.PANEL1306_WH.Location = new Point(this.PANEL1306_WH_txtwherehouse_name.Location.X, this.PANEL1306_WH_txtwherehouse_name.Location.Y + 22);
                    this.PANEL1306_WH_dataGridView1_wherehouse.Focus();
                }
                else
                {
                    this.PANEL1306_WH.Visible = false;
                }
        }
        private void PANEL1306_WH_btnwherehouse_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH.Visible == false)
            {
                this.PANEL1306_WH.Visible = true;
                this.PANEL1306_WH.BringToFront();
                this.PANEL1306_WH.Location = new Point(this.PANEL1306_WH_txtwherehouse_name.Location.X, this.PANEL1306_WH_txtwherehouse_name.Location.Y + 22);
            }
            else
            {
                this.PANEL1306_WH.Visible = false;
            }
        }
        private void PANEL1306_WH_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH.Visible == false)
            {
                this.PANEL1306_WH.Visible = true;
            }
            else
            {
                this.PANEL1306_WH.Visible = false;
            }
        }
        private void PANEL1306_WH_dataGridView1_wherehouse_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1306_WH_dataGridView1_wherehouse.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1306_WH_txtwherehouse_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1306_WH_txtwherehouse_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL1306_WH_dataGridView1_wherehouse_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1306_WH_dataGridView1_wherehouse.CurrentRow.Index;

                this.PANEL1306_WH_txtwherehouse_id.Text = PANEL1306_WH_dataGridView1_wherehouse.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1306_WH_txtwherehouse_name.Text = PANEL1306_WH_dataGridView1_wherehouse.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1306_WH_txtwherehouse_name.Focus();
                this.PANEL1306_WH.Visible = false;
            }
        }
        private void PANEL1306_WH_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1306_WH_btn_search_Click(object sender, EventArgs e)
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

            PANEL1306_WH_Clear_GridView1_wherehouse();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_06wherehouse" +
                                    " WHERE (txtwherehouse_name LIKE '%" + this.PANEL1306_WH_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtwherehouse_id <> '')" +
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
                            var index = PANEL1306_WH_dataGridView1_wherehouse.Rows.Add();
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //2
                            PANEL1306_WH_dataGridView1_wherehouse.Rows[index].Cells["Col_txtwherehouse_name_eng"].Value = dt2.Rows[j]["txtwherehouse_name_eng"].ToString();      //3
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
        private void PANEL1306_WH_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1306_WH_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1306_WH.Height = PANEL1306_WH_btnresize_low.Top + e.Y;
                this.PANEL1306_WH.Width = PANEL1306_WH_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1306_WH_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1306_WH_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtwherehouse คลังสินค้า  =======================================================================


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





        //txtberg_type ประเภทเบิกคลัง  =======================================================================
        private void PANEL0103_BERG_TYPE_Fill_berg_type()
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

            //PANEL0103_BERG_TYPE_Clear_GridView1_berg_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_03berg_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtberg_type_id <> '')" +
                                    " ORDER BY txtberg_type_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtberg_type_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtberg_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtberg_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtberg_type_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtberg_type_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtberg_type_status";

                            var index = PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows.Add();
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_no"].Value = dt2.Rows[j]["txtberg_type_no"].ToString();      //1
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_id"].Value = dt2.Rows[j]["txtberg_type_id"].ToString();      //2
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_name"].Value = dt2.Rows[j]["txtberg_type_name"].ToString();      //3
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_name_eng"].Value = dt2.Rows[j]["txtberg_type_name_eng"].ToString();      //4
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_remark"].Value = dt2.Rows[j]["txtberg_type_remark"].ToString();      //5
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_status"].Value = dt2.Rows[j]["txtberg_type_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0103_BERG_TYPE_dataGridView1_berg_type_Up_Status();

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
        private void PANEL0103_BERG_TYPE_dataGridView1_berg_type_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows.Count; i++)
            {
                if (this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0103_BERG_TYPE_GridView1_berg_type()
        {
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.ColumnCount = 7;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].Name = "Col_txtberg_type_no";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].Name = "Col_txtberg_type_id";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].Name = "Col_txtberg_type_name";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].Name = "Col_txtberg_type_name_eng";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].Name = "Col_txtberg_type_remark";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[6].Name = "Col_txtberg_type_status";

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[0].HeaderText = "No";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].HeaderText = " รหัส";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].HeaderText = " ชื่อรหัสประเภทเบิกสินค้า";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].HeaderText = "ชื่อรหัสประเภทเบิกสินค้า Eng";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[6].HeaderText = " สถานะ";

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[0].Visible = false;  //"No";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].Visible = true;  //"Col_txtberg_type_no";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].Width = 90;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].ReadOnly = true;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].Visible = true;  //"Col_txtberg_type_id";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].Width = 80;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].ReadOnly = true;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].Visible = true;  //"Col_txtberg_type_name";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].Width = 150;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].ReadOnly = true;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].Visible = false;  //"Col_txtberg_type_name_eng";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].Width = 0;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].ReadOnly = true;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].Visible = false;  //"Col_txtberg_type_name_remark";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].Width = 0;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].ReadOnly = true;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[6].Visible = false;  //"Col_txtberg_type_status";
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[6].Width = 0;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Columns.Add(dgvCmb);

        }
        private void PANEL0103_BERG_TYPE_Clear_GridView1_berg_type()
        {
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows.Clear();
            this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Refresh();
        }
        private void PANEL0103_BERG_TYPE_txtberg_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0103_BERG_TYPE.Visible == false)
                {
                    this.PANEL0103_BERG_TYPE.Visible = true;
                    this.PANEL0103_BERG_TYPE.Location = new Point(this.PANEL0103_BERG_TYPE_txtberg_type_name.Location.X, this.PANEL0103_BERG_TYPE_txtberg_type_name.Location.Y + 22);
                    this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Focus();
                }
                else
                {
                    this.PANEL0103_BERG_TYPE.Visible = false;
                }
        }
        private void PANEL0103_BERG_TYPE_btnberg_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL0103_BERG_TYPE.Visible == false)
            {
                this.PANEL0103_BERG_TYPE.Visible = true;
                this.PANEL0103_BERG_TYPE.BringToFront();
                this.PANEL0103_BERG_TYPE.Location = new Point(this.PANEL0103_BERG_TYPE_txtberg_type_name.Location.X, this.PANEL0103_BERG_TYPE_txtberg_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0103_BERG_TYPE.Visible = false;
            }
        }
        private void PANEL0103_BERG_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0103_BERG_TYPE.Visible == false)
            {
                this.PANEL0103_BERG_TYPE.Visible = true;
            }
            else
            {
                this.PANEL0103_BERG_TYPE.Visible = false;
            }
        }
        private void PANEL0103_BERG_TYPE_dataGridView1_berg_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0103_BERG_TYPE_txtberg_type_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0103_BERG_TYPE_txtberg_type_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0103_BERG_TYPE_dataGridView1_berg_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0103_BERG_TYPE_dataGridView1_berg_type.CurrentRow.Index;

                this.PANEL0103_BERG_TYPE_txtberg_type_id.Text = PANEL0103_BERG_TYPE_dataGridView1_berg_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0103_BERG_TYPE_txtberg_type_name.Text = PANEL0103_BERG_TYPE_dataGridView1_berg_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0103_BERG_TYPE_txtberg_type_name.Focus();
                this.PANEL0103_BERG_TYPE.Visible = false;
            }
        }
        private void PANEL0103_BERG_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0103_BERG_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL0103_BERG_TYPE_Clear_GridView1_berg_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_03berg_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtberg_type_name LIKE '%" + this.PANEL0103_BERG_TYPE_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtberg_type_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtberg_type_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtberg_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtberg_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtberg_type_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtberg_type_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtberg_type_status";

                            var index = PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows.Add();
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_no"].Value = dt2.Rows[j]["txtberg_type_no"].ToString();      //1
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_id"].Value = dt2.Rows[j]["txtberg_type_id"].ToString();      //2
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_name"].Value = dt2.Rows[j]["txtberg_type_name"].ToString();      //3
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_name_eng"].Value = dt2.Rows[j]["txtberg_type_name_eng"].ToString();      //4
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_remark"].Value = dt2.Rows[j]["txtberg_type_remark"].ToString();      //5
                            PANEL0103_BERG_TYPE_dataGridView1_berg_type.Rows[index].Cells["Col_txtberg_type_status"].Value = dt2.Rows[j]["txtberg_type_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0103_BERG_TYPE_dataGridView1_berg_type_Up_Status();

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
        private void PANEL0103_BERG_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0103_BERG_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0103_BERG_TYPE.Height = PANEL0103_BERG_TYPE_btnresize_low.Top + e.Y;
                this.PANEL0103_BERG_TYPE.Width = PANEL0103_BERG_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0103_BERG_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0103_BERG_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtberg_type ประเภทเบิกคลัง  =======================================================================

        //txtnumber_mat  เบอร์ผ้า  =======================================================================
        private void PANEL0106_NUMBER_MAT_Fill_number_mat()
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

            PANEL0106_NUMBER_MAT_Clear_GridView1_number_mat();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_06number_mat" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtnumber_mat_id <> '')" +
                                    " ORDER BY txtnumber_mat_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtnumber_mat_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtnumber_mat_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtnumber_mat_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtnumber_mat_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtnumber_mat_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtnumber_mat_status";

                            var index = PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows.Add();
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_no"].Value = dt2.Rows[j]["txtnumber_mat_no"].ToString();      //1
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //2
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_name"].Value = dt2.Rows[j]["txtnumber_mat_name"].ToString();      //3
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_name_eng"].Value = dt2.Rows[j]["txtnumber_mat_name_eng"].ToString();      //4
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_remark"].Value = dt2.Rows[j]["txtnumber_mat_remark"].ToString();      //5
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_status"].Value = dt2.Rows[j]["txtnumber_mat_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0106_NUMBER_MAT_dataGridView1_number_mat_Up_Status();

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
        private void PANEL0106_NUMBER_MAT_dataGridView1_number_mat_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows.Count; i++)
            {
                if (this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0106_NUMBER_MAT_GridView1_number_mat()
        {
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.ColumnCount = 7;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[0].Name = "Col_Auto_num";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].Name = "Col_txtnumber_mat_no";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].Name = "Col_txtnumber_mat_id";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].Name = "Col_txtnumber_mat_name";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].Name = "Col_txtnumber_mat_name_eng";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].Name = "Col_txtnumber_mat_remark";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[6].Name = "Col_txtnumber_mat_status";

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[0].HeaderText = "No";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].HeaderText = " รหัส";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].HeaderText = " ชื่อรหัสเบอร์ผ้า";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].HeaderText = "ชื่อรหัสเบอร์ผ้า Eng";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[6].HeaderText = " สถานะ";

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[0].Visible = false;  //"No";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].Visible = true;  //"Col_txtnumber_mat_no";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].Width = 90;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].ReadOnly = true;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].Visible = true;  //"Col_txtnumber_mat_id";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].Width = 80;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].ReadOnly = true;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].Visible = true;  //"Col_txtnumber_mat_name";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].Width = 150;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].ReadOnly = true;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].Visible = false;  //"Col_txtnumber_mat_name_eng";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].Width = 0;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].ReadOnly = true;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].Visible = false;  //"Col_txtnumber_mat_name_remark";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].Width = 0;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].ReadOnly = true;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[6].Visible = false;  //"Col_txtnumber_mat_status";
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[6].Width = 0;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Columns.Add(dgvCmb);

        }
        private void PANEL0106_NUMBER_MAT_Clear_GridView1_number_mat()
        {
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows.Clear();
            this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Refresh();
        }
        private void PANEL0106_NUMBER_MAT_txtnumber_mat_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0106_NUMBER_MAT.Visible == false)
                {
                    this.PANEL0106_NUMBER_MAT.Visible = true;
                    this.PANEL0106_NUMBER_MAT.Location = new Point(this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Location.X, this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Location.Y + 22);
                    this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Focus();
                }
                else
                {
                    this.PANEL0106_NUMBER_MAT.Visible = false;
                }
        }
        private void PANEL0106_NUMBER_MAT_btnnumber_mat_Click(object sender, EventArgs e)
        {
            if (this.PANEL0106_NUMBER_MAT.Visible == false)
            {
                this.PANEL0106_NUMBER_MAT.Visible = true;
                this.PANEL0106_NUMBER_MAT.BringToFront();
                this.PANEL0106_NUMBER_MAT.Location = new Point(this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Location.X, this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0106_NUMBER_MAT.Visible = false;
            }
        }
        private void PANEL0106_NUMBER_MAT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0106_NUMBER_MAT.Visible == false)
            {
                this.PANEL0106_NUMBER_MAT.Visible = true;
            }
            else
            {
                this.PANEL0106_NUMBER_MAT.Visible = false;
            }
        }
        private void PANEL0106_NUMBER_MAT_dataGridView1_number_mat_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0106_NUMBER_MAT_dataGridView1_number_mat_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0106_NUMBER_MAT_dataGridView1_number_mat.CurrentRow.Index;

                this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = PANEL0106_NUMBER_MAT_dataGridView1_number_mat.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = PANEL0106_NUMBER_MAT_dataGridView1_number_mat.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Focus();
                this.PANEL0106_NUMBER_MAT.Visible = false;
            }
        }
        private void PANEL0106_NUMBER_MAT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0106_NUMBER_MAT_btn_search_Click(object sender, EventArgs e)
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

            PANEL0106_NUMBER_MAT_Clear_GridView1_number_mat();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_06number_mat" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtnumber_mat_name LIKE '%" + this.PANEL0106_NUMBER_MAT_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtnumber_mat_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtnumber_mat_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtnumber_mat_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtnumber_mat_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtnumber_mat_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtnumber_mat_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtnumber_mat_status";

                            var index = PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows.Add();
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_no"].Value = dt2.Rows[j]["txtnumber_mat_no"].ToString();      //1
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //2
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_name"].Value = dt2.Rows[j]["txtnumber_mat_name"].ToString();      //3
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_name_eng"].Value = dt2.Rows[j]["txtnumber_mat_name_eng"].ToString();      //4
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_remark"].Value = dt2.Rows[j]["txtnumber_mat_remark"].ToString();      //5
                            PANEL0106_NUMBER_MAT_dataGridView1_number_mat.Rows[index].Cells["Col_txtnumber_mat_status"].Value = dt2.Rows[j]["txtnumber_mat_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0106_NUMBER_MAT_dataGridView1_number_mat_Up_Status();

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
        private void PANEL0106_NUMBER_MAT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0106_NUMBER_MAT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0106_NUMBER_MAT.Height = PANEL0106_NUMBER_MAT_btnresize_low.Top + e.Y;
                this.PANEL0106_NUMBER_MAT.Width = PANEL0106_NUMBER_MAT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0106_NUMBER_MAT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0106_NUMBER_MAT_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtnumber_mat เบอร์ผ้า =======================================================================


        //txtmat  สินค้า  =======================================================================
        private void PANEL_MAT_Fill_mat()
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

            PANEL_MAT_Clear_GridView1_mat();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +

                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*," +

                                    "b001mat_06price_sale.*" +

                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                    " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +

                                    //" AND (b001mat.txtmat_id <> '')" +
                                    //" AND (b001mat.txtmat_id = '00046')" +
                                    " AND (b001mat_02detail.txtmat_sac_id = '005')" +

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
                            //this.PANEL_MAT_dataGridView1_mat.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[5].Name = "Col_txtmat_unit1_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[6].Name = "Col_chmat_unit_status";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[7].Name = "Col_txtmat_unit2_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[8].Name = "Col_txtmat_unit2_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[9].Name = "Col_txtmat_price_sale1";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[10].Name = "Col_txtmat_status";

                            var index = PANEL_MAT_dataGridView1_mat.Rows.Add();
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = dt2.Rows[j]["txtmat_unit1_qty"].ToString();      //5
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //6
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //7
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = dt2.Rows[j]["txtmat_unit2_qty"].ToString();      //8
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_price_sale1"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");      //9
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //10
                        }
                        //======================================================= Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00"); 
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL_MAT_dataGridView1_mat_Up_Status();
                    this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
        private void PANEL_MAT_dataGridView1_mat_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL_MAT_dataGridView1_mat.Rows.Count; i++)
            {
                if (this.PANEL_MAT_dataGridView1_mat.Rows[i].Cells["Col_txtmat_status"].Value.ToString() == "0")  //Active
                {
                    this.PANEL_MAT_dataGridView1_mat.Rows[i].Cells["Col_Chk"].Value = true;
                }
                else
                {
                    this.PANEL_MAT_dataGridView1_mat.Rows[i].Cells["Col_Chk"].Value = false;

                }
            }

        }
        private void PANEL_MAT_GridView1_mat()
        {
            this.PANEL_MAT_dataGridView1_mat.ColumnCount = 11;
            this.PANEL_MAT_dataGridView1_mat.Columns[0].Name = "Col_Auto_num";
            this.PANEL_MAT_dataGridView1_mat.Columns[1].Name = "Col_txtmat_no";
            this.PANEL_MAT_dataGridView1_mat.Columns[2].Name = "Col_txtmat_id";
            this.PANEL_MAT_dataGridView1_mat.Columns[3].Name = "Col_txtmat_name";
            this.PANEL_MAT_dataGridView1_mat.Columns[4].Name = "Col_txtmat_unit1_name";
            this.PANEL_MAT_dataGridView1_mat.Columns[5].Name = "Col_txtmat_unit1_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns[6].Name = "Col_chmat_unit_status";
            this.PANEL_MAT_dataGridView1_mat.Columns[7].Name = "Col_txtmat_unit2_name";
            this.PANEL_MAT_dataGridView1_mat.Columns[8].Name = "Col_txtmat_unit2_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns[9].Name = "Col_txtmat_price_sale1";
            this.PANEL_MAT_dataGridView1_mat.Columns[10].Name = "Col_txtmat_status";

            this.PANEL_MAT_dataGridView1_mat.Columns[0].HeaderText = "No";
            this.PANEL_MAT_dataGridView1_mat.Columns[1].HeaderText = "ลำดับ";
            this.PANEL_MAT_dataGridView1_mat.Columns[2].HeaderText = " รหัส";
            this.PANEL_MAT_dataGridView1_mat.Columns[3].HeaderText = " ชื่อสินค้า";
            this.PANEL_MAT_dataGridView1_mat.Columns[4].HeaderText = "หน่วยหลัก";
            this.PANEL_MAT_dataGridView1_mat.Columns[5].HeaderText = "หน่วย";
            this.PANEL_MAT_dataGridView1_mat.Columns[6].HeaderText = "แปลง?";
            this.PANEL_MAT_dataGridView1_mat.Columns[7].HeaderText = "หน่วย(2)";
            this.PANEL_MAT_dataGridView1_mat.Columns[8].HeaderText = "หน่วย";
            this.PANEL_MAT_dataGridView1_mat.Columns[9].HeaderText = " ราคาขาย(บาท)";
            this.PANEL_MAT_dataGridView1_mat.Columns[10].HeaderText = "สถานะ";

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_Auto_num"].Visible = false;  //"No";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].Width = 100;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].Width = 120;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].Width = 250;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].Width = 140;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].Visible = true;  //"Col_txtmat_price_sale1";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].Width = 140;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].Visible = false;  //"Col_txtmat_status";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL_MAT_dataGridView1_mat.Columns.Add(dgvCmb);

            this.PANEL_MAT_dataGridView1_mat.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_MAT_dataGridView1_mat.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_MAT_dataGridView1_mat.EnableHeadersVisualStyles = false;


        }
        private void PANEL_MAT_Clear_GridView1_mat()
        {
            this.PANEL_MAT_dataGridView1_mat.Rows.Clear();
            this.PANEL_MAT_dataGridView1_mat.Refresh();
        }
        private void PANEL_MAT_txtmat_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL_MAT.Visible == false)
                {
                    this.PANEL_MAT.Visible = true;
                    this.PANEL_MAT.Location = new Point(this.PANEL_MAT_txtmat_name.Location.X, this.PANEL_MAT_txtmat_name.Location.Y + 22);
                    this.PANEL_MAT_dataGridView1_mat.Focus();
                }
                else
                {
                    this.PANEL_MAT.Visible = false;
                }
        }
        private void PANEL_MAT_btnmat_Click(object sender, EventArgs e)
        {
            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
                this.PANEL_MAT.BringToFront();
                this.PANEL_MAT.Location = new Point(this.PANEL_MAT_txtmat_name.Location.X, this.PANEL_MAT_txtmat_name.Location.Y + 22);
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_dataGridView1_mat_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL_MAT_txtmat_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL_MAT_txtmat_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL_MAT_dataGridView1_mat_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL_MAT_dataGridView1_mat.CurrentRow.Index;

                this.PANEL_MAT_txtmat_id.Text = PANEL_MAT_dataGridView1_mat.CurrentRow.Cells[1].Value.ToString();
                this.PANEL_MAT_txtmat_name.Text = PANEL_MAT_dataGridView1_mat.CurrentRow.Cells[2].Value.ToString();
                this.PANEL_MAT_txtmat_name.Focus();
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL_MAT_btn_search_Click(object sender, EventArgs e)
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

            PANEL_MAT_Clear_GridView1_mat();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
                //this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
                if (this.PANEL_MAT_cboSearch.Text.Trim() == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +

                                        "b001_05mat_unit1.*," +
                                        "b001_05mat_unit2.*," +

                                        "b001mat_06price_sale.*" +

                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                        " INNER JOIN b001_05mat_unit2" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                        " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +



                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat.txtmat_name LIKE '%" + this.PANEL_MAT_txtsearch.Text.Trim() + "%')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

                }
                if (this.PANEL_MAT_cboSearch.Text.Trim() == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +

                                        "b001_05mat_unit1.*," +
                                        "b001_05mat_unit2.*," +

                                        "b001mat_06price_sale.*" +

                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                        " INNER JOIN b001_05mat_unit2" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                        " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +



                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtsearch.Text.Trim() + "')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

                }


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
                            //this.PANEL_MAT_dataGridView1_mat.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[5].Name = "Col_txtmat_unit1_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[6].Name = "Col_chmat_unit_status";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[7].Name = "Col_txtmat_unit2_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[8].Name = "Col_txtmat_unit2_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[9].Name = "Col_txtmat_price_sale1";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[10].Name = "Col_txtmat_status";

                            var index = PANEL_MAT_dataGridView1_mat.Rows.Add();
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = dt2.Rows[j]["txtmat_unit1_qty"].ToString();      //5
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //6
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //7
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = dt2.Rows[j]["txtmat_unit2_qty"].ToString();      //8
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_price_sale1"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");      //9
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //10
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL_MAT_dataGridView1_mat_Up_Status();
                    this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
        private void PANEL_MAT_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_MAT_Fill_mat();
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
        private void PANEL_MAT_btnnew_Click(object sender, EventArgs e)
        {

        }
        private Point MouseDownLocation;
        private void PANEL_MAT_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_MouseDown(object sender, MouseEventArgs e)
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
        private void PANEL_MAT_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
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
        private void PANEL_MAT_dataGridView1_mat_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void PANEL_MAT_dataGridView1_mat_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }



        //END txtmat สินค้า =======================================================================

        //txtmachine เครื่องจักร  =======================================================================
        private void PANEL0102_MACHINE_Fill_machine()
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

            PANEL0102_MACHINE_Clear_GridView1_machine();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT c001_02machine.*," +
                                    "c001_01machine_type.*" +
                                    " FROM c001_02machine" +
                                    " INNER JOIN c001_01machine_type" +
                                    " ON c001_02machine.cdkey = c001_01machine_type.cdkey" +
                                    " AND c001_02machine.txtco_id = c001_01machine_type.txtco_id" +
                                    " AND c001_02machine.txtmachine_type_id = c001_01machine_type.txtmachine_type_id" +
                                    " WHERE (c001_02machine.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (c001_02machine.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (c001_02machine.txtmachine_id <> '')" +
                                    " ORDER BY c001_02machine.txtmachine_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmachine_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmachine_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmachine_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmachine_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmachine_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmachine_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmachine_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmachine_status";

                            var index = PANEL0102_MACHINE_dataGridView1_machine.Rows.Add();
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_no"].Value = dt2.Rows[j]["txtmachine_no"].ToString();      //1
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_name"].Value = dt2.Rows[j]["txtmachine_name"].ToString();      //3
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_name_eng"].Value = dt2.Rows[j]["txtmachine_name_eng"].ToString();      //4
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_remark"].Value = dt2.Rows[j]["txtmachine_remark"].ToString();      //5
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_type_id"].Value = dt2.Rows[j]["txtmachine_type_id"].ToString();      //6
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_type_name"].Value = dt2.Rows[j]["txtmachine_type_name"].ToString();      //7
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_status"].Value = dt2.Rows[j]["txtmachine_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0102_MACHINE_dataGridView1_machine_Up_Status();

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
        private void PANEL0102_MACHINE_dataGridView1_machine_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0102_MACHINE_dataGridView1_machine.Rows.Count; i++)
            {
                if (this.PANEL0102_MACHINE_dataGridView1_machine.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.PANEL0102_MACHINE_dataGridView1_machine.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.PANEL0102_MACHINE_dataGridView1_machine.Rows[i].Cells[9].Value = false;

                }
            }

        }
        private void PANEL0102_MACHINE_GridView1_machine()
        {
            this.PANEL0102_MACHINE_dataGridView1_machine.ColumnCount = 9;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[0].Name = "Col_Auto_num";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].Name = "Col_txtmachine_no";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].Name = "Col_txtmachine_id";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].Name = "Col_txtmachine_name";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].Name = "Col_txtmachine_name_eng";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].Name = "Col_txtmachine_remark";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].Name = "Col_txtmachine_type_id";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].Name = "Col_txtmachine_type_name";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[8].Name = "Col_txtmachine_status";

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[0].HeaderText = "No";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].HeaderText = " รหัส";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].HeaderText = " ชื่อรหัสเครื่องจักร";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].HeaderText = "ชื่อรหัสเครื่องจักร Eng";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].HeaderText = " รหัสประเภทเครื่องจักร";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].HeaderText = " ชื่อประเภทเครื่องจักร";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[8].HeaderText = " สถานะ";

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[0].Visible = false;  //"No";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].Visible = true;  //"Col_txtmachine_no";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].Width = 90;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].Visible = true;  //"Col_txtmachine_id";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].Width = 80;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].Visible = false;  //"Col_txtmachine_name";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].Width = 0;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].Visible = false;  //"Col_txtmachine_name_eng";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].Width = 0;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].Visible = false;  //"Col_txtmachine_name_remark";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].Width = 0;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].Visible = false;  //"Col_txtmachine_type_id";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].Width = 0;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].Visible = true;  //"Col_txtmachine_type_name";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].Width = 150;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].ReadOnly = true;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[8].Visible = false;  //"Col_txtmachine_status";
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[8].Width = 0;
            this.PANEL0102_MACHINE_dataGridView1_machine.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0102_MACHINE_dataGridView1_machine.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0102_MACHINE_dataGridView1_machine.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0102_MACHINE_dataGridView1_machine.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0102_MACHINE_dataGridView1_machine.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0102_MACHINE_dataGridView1_machine.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0102_MACHINE_dataGridView1_machine.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0102_MACHINE_dataGridView1_machine.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0102_MACHINE_dataGridView1_machine.Columns.Add(dgvCmb);

        }
        private void PANEL0102_MACHINE_Clear_GridView1_machine()
        {
            this.PANEL0102_MACHINE_dataGridView1_machine.Rows.Clear();
            this.PANEL0102_MACHINE_dataGridView1_machine.Refresh();
        }
        private void PANEL0102_MACHINE_txtmachine_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0102_MACHINE.Visible == false)
                {
                    this.PANEL0102_MACHINE.Visible = true;
                    this.PANEL0102_MACHINE.Location = new Point(this.PANEL0102_MACHINE_txtmachine_name.Location.X, this.PANEL0102_MACHINE_txtmachine_name.Location.Y + 22);
                    this.PANEL0102_MACHINE_dataGridView1_machine.Focus();
                }
                else
                {
                    this.PANEL0102_MACHINE.Visible = false;
                }
        }
        private void PANEL0102_MACHINE_btnmachine_Click(object sender, EventArgs e)
        {
            if (this.PANEL0102_MACHINE.Visible == false)
            {
                this.PANEL0102_MACHINE.Visible = true;
                this.PANEL0102_MACHINE.BringToFront();
                this.PANEL0102_MACHINE.Location = new Point(this.PANEL0102_MACHINE_txtmachine_name.Location.X, this.PANEL0102_MACHINE_txtmachine_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0102_MACHINE.Visible = false;
            }
        }
        private void PANEL0102_MACHINE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0102_MACHINE.Visible == false)
            {
                this.PANEL0102_MACHINE.Visible = true;
            }
            else
            {
                this.PANEL0102_MACHINE.Visible = false;
            }
        }
        private void PANEL0102_MACHINE_dataGridView1_machine_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0102_MACHINE_dataGridView1_machine.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0102_MACHINE_txtmachine_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0102_MACHINE_txtmachine_name.Text = row.Cells[3].Value.ToString();
                    Show_GO1();
                }
            }
        }
        private void PANEL0102_MACHINE_dataGridView1_machine_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0102_MACHINE_dataGridView1_machine.CurrentRow.Index;

                this.PANEL0102_MACHINE_txtmachine_id.Text = PANEL0102_MACHINE_dataGridView1_machine.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0102_MACHINE_txtmachine_name.Text = PANEL0102_MACHINE_dataGridView1_machine.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0102_MACHINE_txtmachine_name.Focus();
                this.PANEL0102_MACHINE.Visible = false;
            }
        }
        private void PANEL0102_MACHINE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0102_MACHINE_btn_search_Click(object sender, EventArgs e)
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

            PANEL0102_MACHINE_Clear_GridView1_machine();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT c001_02machine.*," +
                                    "c001_01machine_type.*" +
                                    " FROM c001_02machine" +
                                    " INNER JOIN c001_01machine_type" +
                                    " ON c001_02machine.cdkey = c001_01machine_type.cdkey" +
                                    " AND c001_02machine.txtco_id = c001_01machine_type.txtco_id" +
                                    " AND c001_02machine.txtmachine_type_id = c001_01machine_type.txtmachine_type_id" +
                                    " WHERE (c001_02machine.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (c001_02machine.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (c001_02machine.txtmachine_name LIKE '%" + this.PANEL0102_MACHINE_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY c001_02machine.txtmachine_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmachine_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmachine_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmachine_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmachine_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmachine_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmachine_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmachine_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmachine_status";

                            var index = PANEL0102_MACHINE_dataGridView1_machine.Rows.Add();
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_no"].Value = dt2.Rows[j]["txtmachine_no"].ToString();      //1
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_name"].Value = dt2.Rows[j]["txtmachine_name"].ToString();      //3
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_name_eng"].Value = dt2.Rows[j]["txtmachine_name_eng"].ToString();      //4
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_remark"].Value = dt2.Rows[j]["txtmachine_remark"].ToString();      //5
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_type_id"].Value = dt2.Rows[j]["txtmachine_type_id"].ToString();      //6
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_type_name"].Value = dt2.Rows[j]["txtmachine_type_name"].ToString();      //7
                            PANEL0102_MACHINE_dataGridView1_machine.Rows[index].Cells["Col_txtmachine_status"].Value = dt2.Rows[j]["txtmachine_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0102_MACHINE_dataGridView1_machine_Up_Status();

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
        private void PANEL0102_MACHINE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0102_MACHINE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0102_MACHINE.Height = PANEL0102_MACHINE_btnresize_low.Top + e.Y;
                this.PANEL0102_MACHINE.Width = PANEL0102_MACHINE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0102_MACHINE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0102_MACHINE_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmachine เครื่องจักร  =======================================================================




 

        private void UPDATE_PRINT_BY()
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

                    cmd2.CommandText = "UPDATE c002_01berg_produce_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtic_id = '" + this.txtic_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();



                    Cursor.Current = Cursors.WaitCursor;

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
            //=============================================================

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
                                  " FROM c002_01berg_produce_record_trans" +
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

                        transNum = Convert.ToDouble(string.Format("{0:n4}", trans_Right6)) + Convert.ToDouble(string.Format("{0:n4}", 1));
                        trans = transNum.ToString("00000#");

                        if (year2.Trim() == year_now2.Trim())
                        {
                            TMP = "IC" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "IC" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "IC" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtic_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }

        private void Load_FIRST_MAT()
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
                                            " FROM c002_01berg_produce_record_for_load_first" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                            " ORDER BY txtmat_id ASC";
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
                            this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                            this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();
                            this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                            this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                            this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = dt2.Rows[0]["txtnumber_mat_id"].ToString();
                            this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[0]["txtnumber_mat_name"].ToString();
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

            //สต๊อคสินค้า ตามคลัง =============================================================================================
            string OK = "";
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                            " FROM c002_01berg_produce_record_for_load_first" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                            " ORDER BY txtmat_id ASC";
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

            //สต๊อคสินค้า ตามคลัง =============================================================================================





            // INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี

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

                        cmd2.CommandText = "INSERT INTO c002_01berg_produce_record_for_load_first(cdkey,txtco_id," +  //1
                       "txtbranch_id," +  //2
                       "txtwherehouse_id," +  //3
                       "txtwherehouse_name," +  //4
                       "txtmat_id," +  //5
                         "txtmat_name," +  //5
                       "txtnumber_mat_id," +  //5
                     "txtnumber_mat_name) " +  //6
                       "VALUES (@cdkey,@txtco_id," +  //1
                       "@txtbranch_id," +  //2
                       "@txtwherehouse_id," +  //3
                       "@txtwherehouse_name," +  //4
                       "@txtmat_id," +  //5
                       "@txtmat_name," +  //5
                       "@txtnumber_mat_id," +  //5
                       "@txtnumber_mat_name)";   //14

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();  //1

                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //2
                        cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();  //3
                        cmd2.Parameters.Add("@txtwherehouse_name", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_name.Text.ToString();  //4
                        cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_id.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_name.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtnumber_mat_id", SqlDbType.NVarChar).Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtnumber_mat_name", SqlDbType.NVarChar).Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString();  //6

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

            // END INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี

        }

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
            W_ID_Select.WORD_TOP = "ระเบียนใบเบิกด้าย";
            kondate.soft.HOME03_Production.HOME03_Production_02Berg_Produce frm2 = new kondate.soft.HOME03_Production.HOME03_Production_02Berg_Produce();
            frm2.Show();

        }

 













        //=============================================================

        //===============================================
    }
}
