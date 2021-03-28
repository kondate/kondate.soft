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
    public partial class HOME03_Production_03Produce_record : Form
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



        public HOME03_Production_03Produce_record()
        {
            InitializeComponent();
        }

        private void HOME03_Production_03Produce_record_Load(object sender, EventArgs e)
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
            this.iblword_status.Text = "บันทึกFG1 ผ้าดิบ";

            this.ActiveControl = this.txticrf_remark;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = "ซื้อไม่มีvat";
            this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = "PUR_NOvat";

            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL0104_PRODUCE_TYPE_GridView1_produce_type();
            PANEL0104_PRODUCE_TYPE_Fill_produce_type();

            PANEL0102_MACHINE_GridView1_machine();
            PANEL0102_MACHINE_Fill_machine();

            PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax();
            PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax();

            PANEL0106_NUMBER_MAT_GridView1_number_mat();
            PANEL0106_NUMBER_MAT_Fill_number_mat();

            PANEL_MAT_GridView1_mat();
            PANEL_MAT_Fill_mat();
            this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
            this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
            this.PANEL_MAT_cboSearch.Text = "ชื่อสินค้า";

            PANEL0105_FACE_BAKING_GridView1_face_baking();
            PANEL0105_FACE_BAKING_Fill_face_baking();

            Show_GridView66();
            Fill_Show_DATA_GridView66();

            Load_FIRST_MAT();
            Show_GridView1();

            Show_GridView1_Machine();
            Fill_GridView1_Machine();

            Show_GridView_Import();
            Fill_Show_DATA_GridView_Import();
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

        private void panel1_MouseDown(object sender, MouseEventArgs e)
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
            var frm2 = new HOME03_Production.HOME03_Production_03Produce_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "บันทึกFG1 ผ้าดิบ";
            this.txticrf_id.ReadOnly = true;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกคลังสินค้าที่จะบันทึกเก็บ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1306_WH_txtwherehouse_id.Focus();
                return;
            }
            if (this.PANEL_MAT_txtmat_id.Text == "")
            {
                MessageBox.Show("โปรด ใส่รหัสสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL_MAT_txtmat_id.Focus();
                return;
            }

            if (this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Text == "")
            {
                MessageBox.Show("โปรด เลือกประเภทผลิต ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Focus();
                return;
            }
            

            //if (this.PANEL0102_MACHINE_txtmachine_id.Text == "")
            //{
            //    MessageBox.Show("โปรด เลือก เครื่องจักร ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    this.PANEL0102_MACHINE_txtmachine_id.Focus();
            //    return;
            //}

            if (this.PANEL0105_FACE_BAKING_txtface_baking_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก อบหน้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL0105_FACE_BAKING_txtface_baking_id.Focus();
                return;
            }

            //if (Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) > Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())))
            //{
            //    MessageBox.Show("สต๊อคติดลบ  !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            Load_FIRST_FIND_INSERT();
            AUTO_BILL_TRANS_ID();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            GridView1_Cal_Sum_M();
            GridView1_Run_Lot_Num();
            Sum_group_tax();
            STOCK_FIND_INSERT();

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

                    //=================================================================================
                    string D1 = Convert.ToDateTime(this.dtpdate_record.Value.Date).ToString("yyyy-MM-dd", UsaCulture);          //4
                    String stringDateRecord = D1.ToString(); // get value from text field
                    DateTime myDateTime_DateRecord = new DateTime();
                    myDateTime_DateRecord = DateTime.ParseExact(stringDateRecord, "yyyy-MM-dd", UsaCulture);
                    //=================================================================================



                    //1 k020db_receive_record_trans
                    if (W_ID_Select.TRANS_BILL_STATUS.Trim() == "N")
                    {
                        cmd2.CommandText = "INSERT INTO c002_02produce_record_trans(cdkey," +
                                           "txtco_id,txtbranch_id," +
                                           "txttrans_id)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                           "'" + this.txticrf_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();


                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE c002_02produce_record_trans SET txttrans_id = '" + this.txticrf_id.Text.Trim() + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //MessageBox.Show("ok1");

                    //2 c002_02produce_record
                    cmd2.CommandText = "INSERT INTO c002_02produce_record(cdkey,txtco_id,txtbranch_id," +  //1
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                            "txtuser_name,txtemp_office_name," +  //5
                                           "txtversion_id," +  //6
                                            //====================================================

                                           "txticrf_id," + // 7
                                          "txtproduce_type_id," + // 8
                                            //"txtnumber_in_year," + // 9
                                          "txtwherehouse_id," + // 9
                                           "txtfold_amount," + // 11


                                           "txtemp1_id," + // 12
                                            "txtemp1_name," + // 13
                                           "txtemp_office_name_manager," + // 14
                                           "txtemp_office_name_approve," + // 15
                                          "txtproject_id," + // 16
                                           "txtjob_id," + // 17
                                           "txticrf1_remark," + // 18

                                           "txtcurrency_id," + // 19
                                           "txtcurrency_date," + // 20
                                           "txtcurrency_rate," + // 21

                                            "txtacc_group_tax_id," + // 22

                                           "txtmat_no," + // 23
                                           "txtmat_id," + // 24
                                           "txtmat_name," + // 25
                                           "txtnumber_mat_id," + // 26

                                           "txtface_baking_id," + // 27

                                           "txtcost_qty_balance_yokma," + // 28
                                           "txtcost_qty_price_average_yokma," + // 29
                                           "txtcost_money_sum_yokma," + // 30

                                           "txtsum_qty," + // 31
                                           "txtsum_price," + // 32
                                           "txtsum_discount," + // 33
                                           "txtmoney_sum," + // 34
                                           "txtmoney_tax_base," + // 35
                                           "txtvat_rate," + // 36
                                           "txtvat_money," + // 37
                                           "txtmoney_after_vat," + // 38
                                           "txtmoney_after_vat_creditor," + // 39
                                           "txtcreditor_status," + // 40

                                           "txtcost_qty_balance_yokpai," + // 41
                                           "txtcost_qty_price_average_yokpai," + // 42
                                           "txtcost_money_sum_yokpai," + // 43

                                           "txtcost_qty2_balance_yokma," + // 44
                                           "txtsum2_qty," + // 45
                                           "txtcost_qty2_balance_yokpai," + // 46

                                           "txticrf_status," +  //47
                                          "txtpayment_status," +  //48
                                          "txtacc_record_status," +  //49

                                          "txtemp_print," +  //49
                                          "txtemp_print_datetime)" +  //49

                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtuser_name,@txtemp_office_name," +  //5
                                           "@txtversion_id," +  //6
                                            //=========================================================


                                           "@txticrf_id," + // 7
                                         "@txtproduce_type_id," + // 8
                                             //"@txtnumber_in_year," + // 7
                                           "@txtwherehouse_id," + // 9

                                           "@txtfold_amount," + // 11


                                           "@txtemp1_id," + // 12
                                            "@txtemp1_name," + // 13
                                           "@txtemp_office_name_manager," + // 14
                                           "@txtemp_office_name_approve," + // 15
                                          "@txtproject_id," + // 16
                                           "@txtjob_id," + // 17
                                           "@txticrf1_remark," + // 18

                                           "@txtcurrency_id," + // 19
                                           "@txtcurrency_date," + // 20
                                           "@txtcurrency_rate," + // 21

                                            "@txtacc_group_tax_id," + // 22

                                           "@txtmat_no," + // 23
                                           "@txtmat_id," + // 24
                                           "@txtmat_name," + // 25
                                           "@txtnumber_mat_id," + // 26

                                           "@txtface_baking_id," + // 27

                                           "@txtcost_qty_balance_yokma," + // 28
                                           "@txtcost_qty_price_average_yokma," + // 29
                                           "@txtcost_money_sum_yokma," + // 30

                                           "@txtsum_qty," + // 31
                                           "@txtsum_price," + // 32
                                           "@txtsum_discount," + // 33
                                           "@txtmoney_sum," + // 34
                                           "@txtmoney_tax_base," + // 35
                                           "@txtvat_rate," + // 36
                                           "@txtvat_money," + // 37
                                           "@txtmoney_after_vat," + // 38
                                           "@txtmoney_after_vat_creditor," + // 39
                                           "@txtcreditor_status," + // 40

                                           "@txtcost_qty_balance_yokpai," + // 41
                                           "@txtcost_qty_price_average_yokpai," + // 42
                                           "@txtcost_money_sum_yokpai," + // 43

                                           "@txtcost_qty2_balance_yokma," + // 44
                                           "@txtsum2_qty," + // 45
                                           "@txtcost_qty2_balance_yokpai," + // 46

                                           "@txticrf_status," +  //47
                                          "@txtpayment_status," +  //48
                                          "@txtacc_record_status," +  //49

                                          "@txtemp_print," +  //49
                                          "@txtemp_print_datetime)";   //50


                    cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                    cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                    cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //1

                    //DateTime date_send_mat = Convert.ToDateTime(this.dtpdate_record.Value.ToString());
                    //string d_send_mat = date_send_mat.ToString("yyyy-MM-dd");
                    cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.Date).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);

                    //cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                    //cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.Date).Value = this.dtpdate_record.Value;
                    //cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);

                    //DateTime date_send_mat2 = Convert.ToDateTime(this.dtpdate_record.Value.ToString());
                    //string d_send_mat2 = date_send_mat2.ToString("yyyy-MM-dd");
                    cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.Date).Value = myDateTime_DateRecord;  //19
                    //19
                //cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);
                //cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.Date).Value = this.dtpdate_record.Value;


                    cmd2.Parameters.Add("@txtcomputer_ip", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_IP.Trim();
                    cmd2.Parameters.Add("@txtcomputer_name", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim();
                    cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = W_ID_Select.M_USERNAME.Trim();
                    cmd2.Parameters.Add("@txtemp_office_name", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
                    cmd2.Parameters.Add("@txtversion_id", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();  //7
                      //==============================================================================



                    cmd2.Parameters.Add("@txticrf_id", SqlDbType.NVarChar).Value = this.txticrf_id.Text.Trim();  //7
                    cmd2.Parameters.Add("@txtproduce_type_id", SqlDbType.NVarChar).Value = this.PANEL0104_PRODUCE_TYPE_txtproduce_type_id.Text.Trim();  //8
                    //cmd2.Parameters.Add("@txtnumber_in_year", SqlDbType.NVarChar).Value = this.txtnumber_in_year.Text.Trim();  //7
                    cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.Trim();  //9
                    cmd2.Parameters.Add("@txtfold_amount", SqlDbType.NVarChar).Value = this.txtfold_amount.Text.Trim();  //11

                    cmd2.Parameters.Add("@txtemp1_id", SqlDbType.NVarChar).Value = "";  //12
                    cmd2.Parameters.Add("@txtemp1_name", SqlDbType.NVarChar).Value = "";  //13
                    cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.Trim();  //14
                    cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.Trim();  //15


                    cmd2.Parameters.Add("@txtproject_id", SqlDbType.NVarChar).Value = "";  //16
                    cmd2.Parameters.Add("@txtjob_id", SqlDbType.NVarChar).Value = "";  //17
                    cmd2.Parameters.Add("@txticrf1_remark", SqlDbType.NVarChar).Value = this.txticrf_remark.Text.Trim();  //18

                    cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //19
                    cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //20
                    cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtcurrency_rate.Text.ToString()));  //21

                    cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //22

                    cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.txtmat_no.Text.Trim();  //23
                    cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_id.Text.Trim();  //24
                    cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_name.Text.Trim();  //25
                    cmd2.Parameters.Add("@txtnumber_mat_id", SqlDbType.NVarChar).Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.Trim(); //this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.Trim();  //26

                    cmd2.Parameters.Add("@txtface_baking_id", SqlDbType.NVarChar).Value = this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim();  //27


                    cmd2.Parameters.Add("@txtcost_qty_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString()));  //28
                    cmd2.Parameters.Add("@txtcost_qty_price_average_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));  //29
                    cmd2.Parameters.Add("@txtcost_money_sum_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString()));  //30

                    cmd2.Parameters.Add("@txtsum_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //31
                    cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //32
                    cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //33
                    cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //34
                    cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //35
                    cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //36
                    cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //37
                    cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //38
                    cmd2.Parameters.Add("@txtmoney_after_vat_creditor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //39
                    cmd2.Parameters.Add("@txtcreditor_status", SqlDbType.NVarChar).Value = "0";  //40

                    cmd2.Parameters.Add("@txtcost_qty_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString()));  //41
                    cmd2.Parameters.Add("@txtcost_qty_price_average_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString()));  //42
                    cmd2.Parameters.Add("@txtcost_money_sum_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString()));  //43

                    cmd2.Parameters.Add("@txtcost_qty2_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString()));  //44
                    cmd2.Parameters.Add("@txtsum2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));  //45
                    cmd2.Parameters.Add("@txtcost_qty2_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString()));  //46

                    cmd2.Parameters.Add("@txticrf_status", SqlDbType.NVarChar).Value = "0";  //47
                    cmd2.Parameters.Add("@txtpayment_status", SqlDbType.NVarChar).Value = "";  //48
                    cmd2.Parameters.Add("@txtacc_record_status", SqlDbType.NVarChar).Value = "";  //49
                    cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //50
                    cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);//50


                    //=====================================================================================================================================================
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");


                    DateTime date_send_mat3 = Convert.ToDateTime(this.dtpdate_record.Value.ToString());
                    string d_send_mat3 = date_send_mat3.ToString("yyyy-MM-dd");

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
                                //3 c002_02produce_record_detail

                                cmd2.CommandText = "INSERT INTO c002_02produce_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                   "txttrans_year,txttrans_month,txttrans_day," +  //2

                                       "txticrf_id," +  //6
                                        "txtic_id," +  //6
                                        "txtnumber_in_year," +  //6
                                                       "txttrans_date_server," +  //7
                                                        "txtwherehouse_id," +  //8
                                                        "txtmachine_id," +  //9
                                                        "txtfold_number," +  //10

                                          "txtqty," +  //11

                                           "txttrans_time_start," +  //12
                                           "txttrans_time_end," +  //13

                                          "Problem1," +  //14
                                          "Problem2," +  //15
                                          "Problem3," +  //16
                                          "Problem4," +  //17

                                         "txtemp_id," +  //18
                                         "txtemp_name," +  //19
                                         "txtshift_name," +  //20
                                         "txticrf_remark," +  //21

                                         "txtmat_no," +  //22
                                         "txtmat_id," +  //23
                                         "txtmat_name," +  //24
                                         "txtnumber_mat_id," +  //24

                                         "txtmat_unit1_name," +  //27
                                         "txtmat_unit1_qty," +  //28
                                          "chmat_unit_status," +  //29
                                         "txtmat_unit2_name," +  //30
                                         "txtmat_unit2_qty," +  //31

                                        "txtqty2," +  //32


                                         "txtprice," +   //33
                                         "txtdiscount_rate," +  //34
                                         "txtdiscount_money," +  //35
                                         "txtsum_total," +  //36

                                        "txtcost_qty_balance_yokma," +  //37
                                        "txtcost_qty_price_average_yokma," +  //38
                                        "txtcost_money_sum_yokma," +  //39

                                        "txtcost_qty_balance_yokpai," +  //40
                                        "txtcost_qty_price_average_yokpai," +  //41
                                        "txtcost_money_sum_yokpai," +  //42

                                        "txtcost_qty2_balance_yokma," +  //43
                                        "txtcost_qty2_balance_yokpai," +  //44

                                        "txtitem_no," +  //45
                                        "txtqc_status," +  //46
                                        "txtqc_id," +  //47
                                        "txtppt_status," +  //48
                                        "txtppt_id," +  //49
                                        "txtlot_no," +  //50

                                        "txtLot_no_status," +  //51


                                       "txtqty_cut_yokma," +  //33
                                       "txtqty_cut_yokpai," +  //33
                                        "txtqty_after_cut_yokpai," +  //34


                                       "txtqty_cut," +  //33
                                       "txtqty_after_cut," +  //33

                                       "txtcut_id) " +  //34


                            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +

                                "'" + this.txticrf_id.Text.Trim() + "'," +  //6
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtic_id"].Value.ToString() + "'," +    //6
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "'," +    //6
                            "@txttrans_date_client," +
                                //"'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'," +  //7
                                "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +    //8
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmachine_id"].Value.ToString() + "'," +    //9
                                "'" + this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() + "'," +    //10

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //11

                                "'" + this.GridView1.Rows[i].Cells["Col_txttrans_time_start"].Value.ToString() + "'," +    //12
                                "'" + this.GridView1.Rows[i].Cells["Col_txttrans_time_end"].Value.ToString() + "'," +    //13

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_Problem1"].Value.ToString())) + "'," +  //14
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_Problem2"].Value.ToString())) + "'," +  //15
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_Problem3"].Value.ToString())) + "'," +  //16
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_Problem4"].Value.ToString())) + "'," +  //17

                                "'" + this.GridView1.Rows[i].Cells["Col_txtemp_id"].Value.ToString() + "'," +    //18
                                "'" + this.GridView1.Rows[i].Cells["Col_txtemp_name"].Value.ToString() + "'," +    //19
                                "'" + this.GridView1.Rows[i].Cells["Col_txtshift_name"].Value.ToString() + "'," +    //20
                                "'" + this.GridView1.Rows[i].Cells["Col_txticrf_remark"].Value.ToString() + "'," +    //21

                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //22
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //23
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //24
                                "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Value.ToString() + "'," +    //24


                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //27
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString())) + "'," +  //28
                                "'" + this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() + "'," +  //29
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Value.ToString() + "'," +  //30
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString())) + "'," +  //31

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString())) + "'," +  //32

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //33
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //34
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //35
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //36

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + "'," +  //37
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString())) + "'," +  //38
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + "'," +  //39

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //40
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //41
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //42

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + "'," +  //43
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value.ToString())) + "'," +  //44

                                "'" + this.GridView1.Rows[i].Cells["Col_txtitem_no"].Value.ToString() + "'," +    //45
                                "''," +    //46
                                "''," +    //47
                                "''," +    //48
                                "''," +    //49
                                "'" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "'," +    //50
                                "'0'," +  //51

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "'," +   //45

                               "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //29
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //29


                               "'')";   //34

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok3");


                                //===================================================================================================================



                            }
                        }
                    }
                    //this.GridView1.Columns[41].Name = "Col_txtqty_after_cut_";
                    //this.GridView1.Columns[42].Name = "Col_txtqty_after_cut";
                    //this.GridView1.Columns[43].Name = "Col_txtqty_cut_yokma";
                    //this.GridView1.Columns[44].Name = "Col_txtqty_cut_yokpai";
                    //this.GridView1.Columns[45].Name = "Col_txtqty_after_cut_yokpai";


                    for (int i = 0; i < this.GridView1_Machine.Rows.Count; i++)
                    {
                        if (this.GridView1_Machine.Rows[i].Cells["Col_txtmachine_id"].Value != null)
                        {
                            //this.GridView1_Machine.Columns[0].Name = "Col_txtic_id";
                            //this.GridView1_Machine.Columns[1].Name = "Col_txtmachine_no";
                            //this.GridView1_Machine.Columns[2].Name = "Col_txtmachine_id";
                            //this.GridView1_Machine.Columns[3].Name = "Col_txtmachine_name";
                            //this.GridView1_Machine.Columns[4].Name = "Col_txtsum_qty_ic";
                            //this.GridView1_Machine.Columns[5].Name = "Col_txtsum_qty_yes";
                            //this.GridView1_Machine.Columns[6].Name = "Col_txtsum_qty_change";
                            //this.GridView1_Machine.Columns[7].Name = "Col_txtsum_qty_change_rate";

                            if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[i].Cells["Col_txtsum_qty_ic"].Value.ToString())) > 0)
                            {
                                //===================================================================================================================
                                //3 c002_02produce_record_detail

                                cmd2.CommandText = "INSERT INTO c002_02produce_record_machine(cdkey,txtco_id,txtbranch_id," +  //1
                                   "txttrans_year,txttrans_month,txttrans_day," +  //2

                                       "txticrf_id," +  //6
                                        "txtic_id," +  //6
                                      "txtmachine_id," +  //7
                                       "txtsum_qty_ic," +  //8
                                       "txtsum_qty_yes," +  //9
                                       "txtsum_qty_change," +  //10
                                       "txtsum_qty_change_rate) " +  //34


                            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                            "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +  //2

                                "'" + this.txticrf_id.Text.Trim() + "'," +  //6
                                 "'" + this.GridView1_Machine.Rows[i].Cells["Col_txtic_id"].Value.ToString() + "'," +    //6
                                "'" + this.GridView1_Machine.Rows[i].Cells["Col_txtmachine_id"].Value.ToString() + "'," +    //9

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[i].Cells["Col_txtsum_qty_ic"].Value.ToString())) + "'," +  //33
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[i].Cells["Col_txtsum_qty_yes"].Value.ToString())) + "'," +  //34
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[i].Cells["Col_txtsum_qty_change"].Value.ToString())) + "'," +  //35
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[i].Cells["Col_txtsum_qty_change_rate"].Value.ToString())) + "')";   //34


                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok3");


                                //===================================================================================================================

                                //1.c002_01berg_produce_record
                                cmd2.CommandText = "UPDATE c002_01berg_produce_record SET " +
                                                   "txtFG1_id = '" + this.txticrf_id.Text.ToString() + "'," +
                                                   "txtroll_sum = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[i].Cells["Col_txtsum_qty_yes"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtic_id = '" + this.GridView1_Machine.Rows[i].Cells["Col_txtic_id"].Value.ToString() + "')" +
                                                   " AND (txtmachine_id = '" + this.GridView1_Machine.Rows[i].Cells["Col_txtmachine_id"].Value.ToString() + "')";

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok53");


                            }
                        }
                    }
                    //MessageBox.Show("ok513");

                    //1.c002_02produce_record_for_import
                    cmd2.CommandText = "UPDATE c002_02produce_record_for_import SET " +
                                       "txtstatus = '1'" +
                                       " WHERE (txttrans_date_server = '" + d_send_mat3 + "')" +
                                       " AND (txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                       " AND (txtnumber_in_year = '" + this.txtnumber_in_year.Text.Trim() + "')";

                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok53");


                    //สต๊อคสินค้า ตามคลัง =============================================================================================



                    //1.k021_mat_average
                    cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                     "txtcost_qty1_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokpai.Text.ToString())) + "'," +
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

                                   "txtqty1_in," +  //18
                                 "txtqty_in," +  //18
                                   "txtqty2_in," +  //19
                                  "txtprice_in," +   //20
                                   "txtsum_total_in," +  //21

                                   "txtqty1_out," +  //22
                                 "txtqty_out," +  //22
                                  "txtqty2_out," +  //23
                                  "txtprice_out," +  //24
                                   "txtsum_total_out," +  //25

                                     "txtqty1_balance," +  //26
                                 "txtqty_balance," +  //26
                                   "txtqty2_balance," +  //27
                                  "txtprice_balance," +  //28
                                   "txtsum_total_balance," +  //29

                                   "txtitem_no) " +  //30

                            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                            "'" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "','" + myDateTime2.ToString("HH:mm:ss", UsaCulture) + "'," +  //2
                            //"'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", UsaCulture) + "'," +  //3
                             "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +
                             //"'" + d_send_mat3 + "'," +  //3
                             "@txttrans_date_client," +
                           "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                            "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                            "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                                                                          //=======================================================


                            "'" + this.txticrf_id.Text.Trim() + "'," +  //7 txtbill_id
                            "'FG1'," +  //9 txtbill_type
                            "'บันทึก FG1 ผ้าดิบ" + this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Text.Trim() + "'," +  //9 txtbill_remark

                             "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                           "'" + this.txtmat_no.Text + "'," +  //10 
                            "'" + this.PANEL_MAT_txtmat_id.Text.ToString() + "'," +  //11
                            "'" + this.PANEL_MAT_txtmat_name.Text.ToString() + "'," +    //12

                            "'" + this.txtmat_unit1_name.Text.ToString() + "'," +  //13
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString())) + "'," +  //14
                            "'" + this.chmat_unit_status.Text.ToString() + "'," +  //15
                            "'" + this.txtmat_unit2_name.Text.ToString() + "'," +  //16
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +  //17

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance.Text.ToString())) + "'," +  //22 txtqty1_out
                          "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) + "'," +  //22 txtqty_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString())) + "'," +  //23 txtqty2_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtprice.Text.ToString())) + "'," +  //24 txtprice_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty1_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19 txtqty2_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //20 txtprice_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //21 txtsum_total_in

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokpai.Text.ToString())) + "'," +  //26
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

                        if (this.iblword_status.Text.Trim() == "บันทึกFG1 ผ้าดิบ")
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
            W_ID_Select.TRANS_ID = this.txticrf_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_print();
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
            W_ID_Select.TRANS_ID = this.txticrf_id.Text.Trim();
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
                // rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_c002_02produce_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_02produce_record.rpt");


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
                rpt.SetParameterValue("txticrf_id", W_ID_Select.TRANS_ID.Trim());

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

        private void Fill_Show_DATA_GridView66()
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

            Clear_GridView66();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT c002_01berg_produce_record.*," +
                                   "c001_03berg_type.*," +
                                   "c001_06number_mat.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_01berg_produce_record" +

                                   " INNER JOIN c001_03berg_type" +
                                   " ON c002_01berg_produce_record.cdkey = c001_03berg_type.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = c001_03berg_type.txtco_id" +
                                   " AND c002_01berg_produce_record.txtberg_type_id = c001_03berg_type.txtberg_type_id" +

                                   " INNER JOIN c001_06number_mat" +
                                   " ON c002_01berg_produce_record.cdkey = c001_06number_mat.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   " AND c002_01berg_produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_01berg_produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_01berg_produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_01berg_produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_01berg_produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_01berg_produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   //" AND (c002_01berg_produce_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                   " AND (c002_01berg_produce_record.txtic_status = '0')" +
                                   " AND (c002_01berg_produce_record.txtFG1_id = '')" +
                                   " AND (c002_01berg_produce_record.txtroll_sum = 0)" +
                                  " ORDER BY c002_01berg_produce_record.txtic_id ASC";

                //cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        //this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            var index = this.GridView66.Rows.Add();
                            this.GridView66.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView66.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView66.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView66.Rows[index].Cells["Col_txtic_id"].Value = dt2.Rows[j]["txtic_id"].ToString();      //3
                            this.GridView66.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView66.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView66.Rows[index].Cells["Col_txtberg_type_name"].Value = dt2.Rows[j]["txtberg_type_name"].ToString();      //6
                            this.GridView66.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //7
                            this.GridView66.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //7
                            this.GridView66.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView66.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            this.GridView66.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //9

                            this.GridView66.Rows[index].Cells["Col_txtsum_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //10
                            this.GridView66.Rows[index].Cells["Col_txtsum2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum2_qty"]).ToString("###,###.00");      //11

                            //ic==============================
                            if (dt2.Rows[j]["txtic_status"].ToString() == "0")
                            {
                                this.GridView66.Rows[index].Cells["Col_txtic_status"].Value = ""; //12
                            }
                            else if (dt2.Rows[j]["txtic_status"].ToString() == "1")
                            {
                                this.GridView66.Rows[index].Cells["Col_txtic_status"].Value = "ยกเลิก"; //12
                            }

                            this.GridView66.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txtFG1_id"].ToString();      //9
                            this.GridView66.Rows[index].Cells["Col_txtroll_sum"].Value = Convert.ToSingle(dt2.Rows[j]["txtroll_sum"]).ToString("###,###.00");      //11

                        }
                        //=======================================================
                    }
                    else
                    {
                        //this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            GridView66_Color_Column();
        }
        private void Show_GridView66()
        {
            this.GridView66.ColumnCount = 17;
            this.GridView66.Columns[0].Name = "Col_Auto_num";
            this.GridView66.Columns[1].Name = "Col_txtco_id";
            this.GridView66.Columns[2].Name = "Col_txtbranch_id";
            this.GridView66.Columns[3].Name = "Col_txtic_id";
            this.GridView66.Columns[4].Name = "Col_txttrans_date_server";
            this.GridView66.Columns[5].Name = "Col_txttrans_time";
            this.GridView66.Columns[6].Name = "Col_txtberg_type_name";
            this.GridView66.Columns[7].Name = "Col_txtwherehouse_name";
            this.GridView66.Columns[8].Name = "Col_txtmachine_id";
            this.GridView66.Columns[9].Name = "Col_txtmat_id";
            this.GridView66.Columns[10].Name = "Col_txtmat_name";
            this.GridView66.Columns[11].Name = "Col_txtnumber_mat_id";
            this.GridView66.Columns[12].Name = "Col_txtsum_qty";
            this.GridView66.Columns[13].Name = "Col_txtsum2_qty";
            this.GridView66.Columns[14].Name = "Col_txtic_status";
            this.GridView66.Columns[15].Name = "Col_txtFG1_id";
            this.GridView66.Columns[16].Name = "Col_txtroll_sum";

            this.GridView66.Columns[0].HeaderText = "No";
            this.GridView66.Columns[1].HeaderText = "txtco_id";
            this.GridView66.Columns[2].HeaderText = " txtbranch_id";
            this.GridView66.Columns[3].HeaderText = " เลขที่เบิกด้าย";
            this.GridView66.Columns[4].HeaderText = " วันที่";
            this.GridView66.Columns[5].HeaderText = " เวลา";
            this.GridView66.Columns[6].HeaderText = "ประเภทเบิก";
            this.GridView66.Columns[7].HeaderText = "คลัง";
            this.GridView66.Columns[8].HeaderText = "เครื่องจักร";
            this.GridView66.Columns[9].HeaderText = "รหัสวัตถุดิบ";
            this.GridView66.Columns[10].HeaderText = "ชื่อวัตถุดิบ";
            this.GridView66.Columns[11].HeaderText = "เบอร์วัตถุดิบ";
            this.GridView66.Columns[12].HeaderText = "เบิก กก.";
            this.GridView66.Columns[13].HeaderText = "เบิก ปอนด์";
            this.GridView66.Columns[14].HeaderText = " สถานะ";
            this.GridView66.Columns[15].HeaderText = "เลขที่ FG1";
            this.GridView66.Columns[16].HeaderText = "ผลิตผ้าดิบได้(ม้วน)";

            this.GridView66.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView66.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.GridView66.Columns["Col_txtbranch_id"].Visible = false;  //"Col_txtbranch_id";

            this.GridView66.Columns["Col_txtic_id"].Visible = true;  //"Col_txtic_id";
            this.GridView66.Columns["Col_txtic_id"].Width = 120;
            this.GridView66.Columns["Col_txtic_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtic_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtic_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txttrans_date_server"].Visible = true;  //"Col_txttrans_date_server";
            this.GridView66.Columns["Col_txttrans_date_server"].Width = 100;
            this.GridView66.Columns["Col_txttrans_date_server"].ReadOnly = true;
            this.GridView66.Columns["Col_txttrans_date_server"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txttrans_date_server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txttrans_time"].Visible = false;  //"Col_txttrans_time";
            this.GridView66.Columns["Col_txttrans_time"].Width = 0;
            this.GridView66.Columns["Col_txttrans_time"].ReadOnly = true;
            this.GridView66.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtberg_type_name"].Visible = false;  //"Col_txtberg_type_name";
            this.GridView66.Columns["Col_txtberg_type_name"].Width = 0;
            this.GridView66.Columns["Col_txtberg_type_name"].ReadOnly = true;
            this.GridView66.Columns["Col_txtberg_type_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtberg_type_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtwherehouse_name"].Visible = false;  //"Col_txtwherehouse_name";
            this.GridView66.Columns["Col_txtwherehouse_name"].Width = 0;
            this.GridView66.Columns["Col_txtwherehouse_name"].ReadOnly = true;
            this.GridView66.Columns["Col_txtwherehouse_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtwherehouse_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView66.Columns["Col_txtmachine_id"].Width = 100;
            this.GridView66.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txtmat_id"].Visible = false;  //"Col_txtmat_id";
            this.GridView66.Columns["Col_txtmat_id"].Width = 0;
            this.GridView66.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView66.Columns["Col_txtmat_name"].Width = 120;
            this.GridView66.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";
            this.GridView66.Columns["Col_txtnumber_mat_id"].Width = 0;
            this.GridView66.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtsum_qty"].Visible = true;  //"Col_txtsum_qty";
            this.GridView66.Columns["Col_txtsum_qty"].Width = 120;
            this.GridView66.Columns["Col_txtsum_qty"].ReadOnly = true;
            this.GridView66.Columns["Col_txtsum_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtsum_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtsum2_qty"].Visible = false;  //"Col_txtsum2_qty";
            this.GridView66.Columns["Col_txtsum2_qty"].Width = 0;
            this.GridView66.Columns["Col_txtsum2_qty"].ReadOnly = true;
            this.GridView66.Columns["Col_txtsum2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtsum2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView66.Columns["Col_txtic_status"].Visible = false;  //"Col_txtic_status";
            this.GridView66.Columns["Col_txtic_status"].Width = 0;
            this.GridView66.Columns["Col_txtic_status"].ReadOnly = true;
            this.GridView66.Columns["Col_txtic_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtic_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtFG1_id"].Visible = true;  //"Col_txtFG1_id";
            this.GridView66.Columns["Col_txtFG1_id"].Width = 120;
            this.GridView66.Columns["Col_txtFG1_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtFG1_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtFG1_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtroll_sum"].Visible = true;  //"Col_txtroll_sum";
            this.GridView66.Columns["Col_txtroll_sum"].Width = 120;
            this.GridView66.Columns["Col_txtroll_sum"].ReadOnly = true;
            this.GridView66.Columns["Col_txtroll_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtroll_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView66.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView66.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView66.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView66.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView66.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView66.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView66.EnableHeadersVisualStyles = false;


        }
        private void Clear_GridView66()
        {
            this.GridView66.Rows.Clear();
            this.GridView66.Refresh();
        }
        private void GridView66_Color()
        {
            for (int i = 0; i < this.GridView66.Rows.Count - 0; i++)
            {

                //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmoney_after_vat_creditor"].Value.ToString())) > 0)
                //{
                //    GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                //    GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                //    GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                //}

            }
        }
        private void GridView66_Color_Column()
        {

            for (int i = 0; i < this.GridView66.Rows.Count - 0; i++)
            {
                GridView66.Rows[i].Cells["Col_txtmachine_id"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView66.Rows[i].Cells["Col_txtsum_qty"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView66.Rows[i].Cells["Col_txtroll_sum"].Style.BackColor = Color.LightGoldenrodYellow;

            }
        }
        private void GridView66_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView66.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmachine_id"].Value;
                if (cell != null)
                {
                    W_ID_Select.TRANS_ID = row.Cells["Col_txtic_id"].Value.ToString();
                    this.txtic_id.Text = row.Cells["Col_txtic_id"].Value.ToString();

                    this.PANEL0102_MACHINE_txtmachine_id.Text = row.Cells["Col_txtmachine_id"].Value.ToString();
                    this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = row.Cells["Col_txtnumber_mat_id"].Value.ToString();


                    if (this.PANEL0105_FACE_BAKING_txtface_baking_id.Text == "")
                    {
                        MessageBox.Show("โปรด เลือก อบหน้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        if (this.PANEL0105_FACE_BAKING.Visible == false)
                        {
                            this.PANEL0105_FACE_BAKING.Visible = true;
                            this.PANEL0105_FACE_BAKING.BringToFront();
                            this.PANEL0105_FACE_BAKING.Location = new Point(this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.X, this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.Y + 22);
                        }
                        else
                        {
                            this.PANEL0105_FACE_BAKING.Visible = false;
                        }
                        return;

                    }
                    else
                    {

                    }
                    //======================================================

                    if (this.PANEL_MAT_txtmat_id.Text == "")
                    {
                        MessageBox.Show("โปรด เลือก รหัสสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        return;

                    }
                    else
                    {

                    }
                    //======================================================



                    Fill_DATA_TO_GridView1();
                    GridView1_Add_Qty();

                    //FILL_To_GRID();

                    if (this.PANEL_MAT_txtmat_id.Text.ToString() == "RIB")
                    {
                        this.btnGo1.Visible = false;
                        this.btnGo1_RIB.Visible = true;
                    }
                    else
                    {
                        this.btnGo1.Visible = true;
                        this.btnGo1_RIB.Visible = false;

                    }
                }
                //=====================
            }
        }
        private void GridView66_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView66_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void Fill_DATA_TO_GridView1()
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

            //Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c002_01berg_produce_record.*," +
                                   "c002_01berg_produce_record_detail.*," +
                                   "c001_03berg_type.*," +
                                   "k013_1db_acc_13group_tax.*," +
                                   "c001_02machine.*," +
                                   "c001_06number_mat.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_01berg_produce_record" +

                                   " INNER JOIN c002_01berg_produce_record_detail" +
                                   " ON c002_01berg_produce_record.cdkey = c002_01berg_produce_record_detail.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = c002_01berg_produce_record_detail.txtco_id" +
                                   " AND c002_01berg_produce_record.txtic_id = c002_01berg_produce_record_detail.txtic_id" +


                                   " INNER JOIN c001_03berg_type" +
                                   " ON c002_01berg_produce_record.cdkey = c001_03berg_type.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = c001_03berg_type.txtco_id" +
                                   " AND c002_01berg_produce_record.txtberg_type_id = c001_03berg_type.txtberg_type_id" +

                                   " INNER JOIN c001_02machine" +
                                   " ON c002_01berg_produce_record.cdkey = c001_02machine.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = c001_02machine.txtco_id" +
                                   " AND c002_01berg_produce_record.txtmachine_id = c001_02machine.txtmachine_id" +

                                   " INNER JOIN c001_06number_mat" +
                                   " ON c002_01berg_produce_record.cdkey = c001_06number_mat.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   " AND c002_01berg_produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_01berg_produce_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_01berg_produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_01berg_produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_01berg_produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_01berg_produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_01berg_produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_01berg_produce_record.txtic_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                  " ORDER BY c002_01berg_produce_record.txtic_id ASC";


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        this.txtic_id.Text = dt2.Rows[0]["txtic_id"].ToString();

                        this.PANEL0102_MACHINE_txtmachine_id.Text = dt2.Rows[0]["txtmachine_id"].ToString();
                        this.PANEL0102_MACHINE_txtmachine_name.Text = dt2.Rows[0]["txtmachine_name"].ToString();

                        this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                        this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();

                        this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_server"].ToString());
                        this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                        this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

                        //this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                        //this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                        //this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                        //this.txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();


                        //this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit1_qty"]).ToString("###,###.00");
                        //this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                        //this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit1_qty"]).ToString("###,###.00");
                        //this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                        //this.txtmat_unit2_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit2_qty"]).ToString("###,###.00");


                        this.Paneldate_txtcurrency_date.Text = dt2.Rows[0]["txtcurrency_date"].ToString();
                        this.txtcurrency_id.Text = dt2.Rows[0]["txtcurrency_id"].ToString();
                        this.txtcurrency_rate.Text = dt2.Rows[0]["txtcurrency_rate"].ToString();

                        this.txtemp_office_name_manager.Text = dt2.Rows[0]["txtemp_office_name_manager"].ToString();
                        this.txtemp_office_name_approve.Text = dt2.Rows[0]["txtemp_office_name_approve"].ToString();


                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");

                        this.txtsum_qty_ic.Text = Convert.ToSingle(dt2.Rows[0]["txtsum_qty"]).ToString("###,###.00");

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





            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    //"k021_mat_average.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*" +
                                    " FROM b001mat" +

                                    //" INNER JOIN k021_mat_average" +
                                    //" ON b001mat.cdkey = k021_mat_average.cdkey" +
                                    //" AND b001mat.txtco_id = k021_mat_average.txtco_id" +
                                    //" AND b001mat.txtmat_id = k021_mat_average.txtmat_id" +

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

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +



                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    //" AND (c002_02produce_record_for_import.txttrans_date_server = @datestart)" +
                                    " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                    //" AND (c002_02produce_record_for_import.txtnumber_in_year = '" + this.txtnumber_in_year.Text.Trim() + "')" +
                                    //" AND (c002_02produce_record_for_import.txtface_baking_id = '" + this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim() + "')" +
                                    //" AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.ID ASC";


                //cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.dtpdate_record.Value;


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    int k = 0;
                    double z = 0;
                    z = Convert.ToDouble(this.txtfold_amount.Text);
                    double z2 = 0;
                    z2 = Convert.ToDouble(1);

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
                            //}
                            //=======================================================
                            Cursor.Current = Cursors.Default;
                            //=======================================================
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {

                        MessageBox.Show("ไม่พบรหัสสินค้า " + this.PANEL_MAT_txtmat_id.Text.Trim() + "  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor.Current = Cursors.Default;
                        conn.Close();
                        return;
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
                                    " AND (c001_02machine.txtmachine_id = '" + this.PANEL0102_MACHINE_txtmachine_id.Text.Trim() + "')" +
                                    " ORDER BY c001_02machine.txtmachine_no ASC";

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
                            this.PANEL0102_MACHINE_txtmachine_name.Text = dt2.Rows[j]["txtmachine_name"].ToString();      //3
                        }
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

                //===========================================
            }
            //================================




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
                                    " AND (txtnumber_mat_id = '" + this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.Trim() + "')" +
                                    " ORDER BY txtnumber_mat_id ASC";

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
                            this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[j]["txtnumber_mat_name"].ToString();      //3
                        }
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

                //===========================================
            }
            //================================


        }

        //===============================================
        private void Fill_Show_DATA_GridView_Import()
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

            Clear_GridView_Import();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM c002_02produce_record_for_import" +
                                   " WHERE (txtstatus = '0')" +
                                  " order by txttrans_date_server,txtmat_id,txtnumber_in_year,txtface_baking_id,txtmachine_id,txtfold_number ASC";

                //cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        //this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            var index = this.GridView_Import.Rows.Add();
                            this.GridView_Import.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView_Import.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView_Import.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();      //1
                            this.GridView_Import.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView_Import.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //3
                            this.GridView_Import.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //7
                            this.GridView_Import.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //10
                           this.GridView_Import.Rows[index].Cells["Col_txtstatus"].Value = dt2.Rows[j]["txtstatus"].ToString();      //2

                        }
                        //=======================================================
                    }
                    else
                    {
                        //this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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
        private void Show_GridView_Import()
        {
            this.GridView_Import.ColumnCount = 8;
            this.GridView_Import.Columns[0].Name = "Col_txttrans_date_server";
            this.GridView_Import.Columns[1].Name = "Col_txtmat_id";
            this.GridView_Import.Columns[2].Name = "Col_txtface_baking_id";
            this.GridView_Import.Columns[3].Name = "Col_txtnumber_in_year";
            this.GridView_Import.Columns[4].Name = "Col_txtfold_number";
            this.GridView_Import.Columns[5].Name = "Col_txtmachine_id";
            this.GridView_Import.Columns[6].Name = "Col_txtqty";
            this.GridView_Import.Columns[7].Name = "Col_txtstatus";

            this.GridView_Import.Columns[0].HeaderText = "Col_txttrans_date_server";
            this.GridView_Import.Columns[1].HeaderText = "Col_txtmat_id";
            this.GridView_Import.Columns[2].HeaderText = " Col_txtface_baking_id";
            this.GridView_Import.Columns[3].HeaderText = " Col_txtnumber_in_year";
            this.GridView_Import.Columns[4].HeaderText = " Col_txtfold_number";
            this.GridView_Import.Columns[5].HeaderText = " Col_txtmachine_id";
            this.GridView_Import.Columns[6].HeaderText = "Col_txtqty";
            this.GridView_Import.Columns[7].HeaderText = "Col_txtstatus";

            this.GridView_Import.Columns["Col_txttrans_date_server"].Visible = true;  //"Col_txttrans_date_server";
            this.GridView_Import.Columns["Col_txttrans_date_server"].Width = 140;
            this.GridView_Import.Columns["Col_txttrans_date_server"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txttrans_date_server"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txttrans_date_server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView_Import.Columns["Col_txtmat_id"].Width = 100;
            this.GridView_Import.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView_Import.Columns["Col_txtface_baking_id"].Width =100;
            this.GridView_Import.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.Columns["Col_txtnumber_in_year"].Visible = true;  //"Col_txtnumber_in_year";
            this.GridView_Import.Columns["Col_txtnumber_in_year"].Width = 100;
            this.GridView_Import.Columns["Col_txtnumber_in_year"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView_Import.Columns["Col_txtfold_number"].Width = 100;
            this.GridView_Import.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView_Import.Columns["Col_txtmachine_id"].Width = 100;
            this.GridView_Import.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView_Import.Columns["Col_txtqty"].Width = 80;
            this.GridView_Import.Columns["Col_txtqty"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView_Import.Columns["Col_txtstatus"].Visible = true;  //"Col_txtstatus";
            this.GridView_Import.Columns["Col_txtstatus"].Width = 80;
            this.GridView_Import.Columns["Col_txtstatus"].ReadOnly = true;
            this.GridView_Import.Columns["Col_txtstatus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView_Import.Columns["Col_txtstatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView_Import.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView_Import.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView_Import.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView_Import.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView_Import.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView_Import.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView_Import.EnableHeadersVisualStyles = false;


        }
        private void Clear_GridView_Import()
        {
            this.GridView_Import.Rows.Clear();
            this.GridView_Import.Refresh();
        }
        private void GridView_Import_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.GridView_Import.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmat_id"].Value;
                if (cell != null)
                {
                    //this.GridView_Import.Columns[0].Name = "Col_txttrans_date_server";
                    //this.GridView_Import.Columns[1].Name = "Col_txtmat_id";
                    //this.GridView_Import.Columns[2].Name = "Col_txtface_baking_id";
                    //this.GridView_Import.Columns[3].Name = "Col_txtnumber_in_year";
                    //this.GridView_Import.Columns[4].Name = "Col_txtfold_number";
                    //this.GridView_Import.Columns[5].Name = "Col_txtmachine_id";
                    //this.GridView_Import.Columns[6].Name = "Col_txtqty";
                    //this.GridView_Import.Columns[7].Name = "Col_txtstatus";

                    this.dtpdate_record.Value = Convert.ToDateTime(row.Cells["Col_txttrans_date_server"].Value.ToString());
                    this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                    this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

                    //DateTime date_send_mat = Convert.ToDateTime(this.dtpdate_record.Value.ToString());
                    //string d_send_mat = date_send_mat.ToString("yyyy-MM-dd");
                    //cmd2.Parameters.Add("@txtdate_send_mat", SqlDbType.NVarChar).Value = d_send_mat;  //19


                    this.PANEL_MAT_txtmat_id.Text = row.Cells["Col_txtmat_id"].Value.ToString();
                    this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = row.Cells["Col_txtface_baking_id"].Value.ToString();
                    this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = row.Cells["Col_txtface_baking_id"].Value.ToString();
                    this.txtnumber_in_year.Text = row.Cells["Col_txtnumber_in_year"].Value.ToString();


                    FILL_GridView_Import_To_GRID();
                    GridView1_Add_Qty();

                }
                //=====================
            }
        }
        private void FILL_GridView_Import_To_GRID()
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
            Clear_GridView_Import();
            //===========================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c002_02produce_record_for_import.*," +
                                      "b001mat.*," +
                                    //"k021_mat_average.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*" +
                                    " FROM c002_02produce_record_for_import" +

                                      " INNER JOIN b001mat" +
                                    " ON c002_02produce_record_for_import.txtmat_id = b001mat.txtmat_id" +

                                    //" INNER JOIN k021_mat_average" +
                                    //" ON b001mat.cdkey = k021_mat_average.cdkey" +
                                    //" AND b001mat.txtco_id = k021_mat_average.txtco_id" +
                                    //" AND b001mat.txtmat_id = k021_mat_average.txtmat_id" +

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

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +



                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (c002_02produce_record_for_import.txttrans_date_server = @datestart)" +
                                    " AND (c002_02produce_record_for_import.txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                     " AND (c002_02produce_record_for_import.txtnumber_in_year = '" + this.txtnumber_in_year.Text.Trim() + "')" +
                                     " AND (c002_02produce_record_for_import.txtface_baking_id = '" + this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim() + "')" +
                                    //" AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                    " ORDER BY c002_02produce_record_for_import.txtfold_number ASC";


                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.dtpdate_record.Value;


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    int k = 0;
                    double z = 0;
                    z = Convert.ToDouble(this.txtfold_amount.Text);
                    double z2 = 0;
                    z2 = Convert.ToDouble(1);

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

                            //   string[] row = new string[] { k.ToString(), "", "", "", this.PANEL1306_WH_txtwherehouse_id.Text, this.txtmat_id.Text.ToString(), this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString() };
                            //======================================================
                            //for (int i = 0; i < z; i++)
                            //{
                            //    k = 1 + i;
                            //    string Lot_NO = DateTime.Now.ToString("yyMMddHHmmss", ThaiCulture) + "-" + this.PANEL0102_MACHINE_txtmachine_id.Text.Trim() + "-" + this.PANEL0105_FACE_BAKING_txtface_baking_name.Text.Trim() + "-" + k.ToString("00");

                            //    string[] row = new string[] { k.ToString(),   //"Col_Auto_num";
                            //                                                                            this.PANEL1306_WH_txtwherehouse_id.Text.Trim(),  // "Col_txtwherehouse_id";
                            //                                                                           dt2.Rows[j]["txtmachine_id"].ToString(),  // "Col_txtmachine_id";
                            //                                                                            dt2.Rows[j]["txtfold_number"].ToString(),   //"Col_txtfold_number";
                            //                                                                            Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00"),  // "Col_txtqty";

                            //                                                                            "",  //"Col_txttrans_time_start";
                            //                                                                            "",  // "Col_txttrans_time_end";

                            //                                                                            "0",  // "Col_Problem1";
                            //                                                                            "0",  // "Col_Problem2";
                            //                                                                            "0",  // "Col_Problem3";
                            //                                                                            "0",  // "Col_Problem4";

                            //                                                                            "",  // "Col_txtemp_id";
                            //                                                                            "",  // "Col_txtemp_name";
                            //                                                                            "",  // "Col_txtshift_name";
                            //                                                                            "",  // "Col_txticrf_remark";

                            //                                                                            this.txtmat_no.Text.ToString(),  // "Col_txtmat_no";
                            //                                                                            this.PANEL_MAT_txtmat_id.Text.ToString(),  // "Col_txtmat_id";
                            //                                                                            this.PANEL_MAT_txtmat_name.Text.ToString(),  // "Col_txtmat_name";

                            //                                                                            "",  // "Col_PANEL0106_NUMBER_MAT_txtnumber_mat_name";

                            //                                                                            this.txtmat_unit1_name.Text.ToString(),  //"Col_txtmat_unit1_name";
                            //                                                                            Convert.ToSingle(this.txtmat_unit1_qty.Text).ToString("###,###.00"),  // "Col_txtmat_unit1_qty";
                            //                                                                            this.chmat_unit_status.Text.ToString(),  // "Col_chmat_unit_status";
                            //                                                                            this.txtmat_unit2_name.Text.ToString(),   // "Col_txtmat_unit2_name";
                            //                                                                            Convert.ToSingle(this.txtmat_unit2_qty.Text).ToString("###,###.00"),  // "Col_txtmat_unit1_qty";

                            //                                                                            "0",  // "Col_txtqty2";


                            //                                                                            "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00"),  //"Col_txtprice";
                            //                                                                            "0",  // "Col_txtdiscount_rate";
                            //                                                                            "0",  // "Col_txtdiscount_money";
                            //                                                                            "0",  // "Col_txtsum_total";

                            //                                                                           "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00"),  // "Col_txtcost_qty_balance_yokma";
                            //                                                                           "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00"),  // "Col_txtcost_qty_price_average_yokma";
                            //                                                                           "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00"),  // "Col_txtcost_money_sum_yokma";

                            //                                                                            "0",  // "Col_txtcost_qty_balance_yokpai";
                            //                                                                            "0",  // "Col_txtcost_qty_price_average_yokpai";
                            //                                                                            "0",  // "Col_txtcost_money_sum_yokpai";

                            //                                                                            "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00"),  // "Col_txtcost_qty2_balance_yokma";
                            //                                                                            "0",  // "Col_txtcost_qty2_balance_yokpai";

                            //                                                                               k.ToString(),  // "Col_txtitem_no";
                            //                                                                              "",  // "Col_mat_status";
                            //                                                                             this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim(),  // "Col_txtface_baking_id";
                            //                                                                             Lot_NO.Trim(),
                            //                                                                                "0",  // "Col_txtqty_after_cut_";
                            //                                                                             "0",  // "Col_txtqty_after_cut";
                            //                                                                              "0",  // "Col_txtqty_cut_yokma";
                            //                                                                              "0",  // "Col_txtqty_cut_yokpai";
                            //                                                                              "0",  // "Col_txtqty_after_cut_yokpai";
                            //                                                                             ""  // "Col_txtic_id";

                            //                                                                          };
                            //    GridView1.Rows.Add(row);
                            //}
                            //====================================================== 

                            //for (int j = 0; j < dt2.Rows.Count; j++)
                            //{
                                k = 1 + j;
                                var index = GridView1.Rows.Add();
                                GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                                GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.Trim();      //1
                                GridView1.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //3

                                GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //4

                                GridView1.Rows[index].Cells["Col_txttrans_time_start"].Value = "";      //5
                                GridView1.Rows[index].Cells["Col_txttrans_time_end"].Value = "";      //6

                                GridView1.Rows[index].Cells["Col_Problem1"].Value = "0";       //7
                                GridView1.Rows[index].Cells["Col_Problem2"].Value = "0";      //8
                                GridView1.Rows[index].Cells["Col_Problem3"].Value = "0";      //9
                                GridView1.Rows[index].Cells["Col_Problem4"].Value = "0";      //10

                                GridView1.Rows[index].Cells["Col_txtemp_id"].Value = "";       //11
                                GridView1.Rows[index].Cells["Col_txtemp_name"].Value = "";      //12
                                GridView1.Rows[index].Cells["Col_txtshift_name"].Value = "";      //13


                                GridView1.Rows[index].Cells["Col_txticrf_remark"].Value = "";       //14

                                GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //15
                                GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //16
                                GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //17
                                GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = "";      //18

                                GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //19
                                GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //20

                                GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //21

                                GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //22
                                GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //23

                                GridView1.Rows[index].Cells["Col_txtqty2"].Value = "0";      //24


                                GridView1.Rows[index].Cells["Col_txtprice"].Value = "0";        //25
                                GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";       //26
                                GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";       //27
                                GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0";       //28

                                GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";     //29
                                GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";       //30
                                GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //31

                                GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //32
                                GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";       //33
                                GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //34

                                GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";      //35
                                GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";       //36

                                GridView1.Rows[index].Cells["Col_txtitem_no"].Value = k.ToString();      //37

                                GridView1.Rows[index].Cells["Col_mat_status"].Value = "0";

                                GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim();     //41
                                GridView1.Rows[index].Cells["Col_txtlot_no"].Value = DateTime.Now.ToString("yyMMddHHmmss", ThaiCulture) + "-" + dt2.Rows[j]["txtmachine_id"].ToString() + "-" + this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim() + "-" + dt2.Rows[j]["txtfold_number"].ToString();    //42


                            //                                                                                "0",  // "Col_txtqty_after_cut_";
                            //                                                                             "0",  // "Col_txtqty_after_cut";
                            //                                                                              "0",  // "Col_txtqty_cut_yokma";
                            //                                                                              "0",  // "Col_txtqty_cut_yokpai";
                            //                                                                              "0",  // "Col_txtqty_after_cut_yokpai";

                            GridView1.Rows[index].Cells["Col_txtqty_after_cut_"].Value = "0";      //35
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = "0";     //36
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokma"].Value = "0";     //37
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokpai"].Value = "0";     //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";     //37

                            GridView1.Rows[index].Cells["Col_txtic_id"].Value = "0";       //37


                            //}
                            //=======================================================
                            Cursor.Current = Cursors.Default;
                            //=======================================================
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {

                        MessageBox.Show("ไม่พบรหัสสินค้า " + this.PANEL_MAT_txtmat_id.Text.Trim() + "  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor.Current = Cursors.Default;
                        conn.Close();
                        return;
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
        private void btnImport_excel_Click(object sender, EventArgs e)
        {
            Show_GridView_Import();
            Fill_Show_DATA_GridView_Import();

        }



        //1.ส่วนหน้าหลัก ตารางสำหรับบันทึก========================================================================
        //DateTimePicker dtp1 = new DateTimePicker();
        //Rectangle _Rectangle1;
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow = 0;

        private void btnGo1_Click(object sender, EventArgs e)
        {
            //if (this.txtnumber_in_year.Text == "")
            //{
            //    MessageBox.Show("โปรด ใส่เลขชุดที่ รับเข้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {

                    MessageBox.Show("โปรด เลือก คลังสินค้าที่ รับเข้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            //======================================================

            if (this.PANEL0102_MACHINE_txtmachine_name.Text == "")
            {
                if (this.ch_yokma.Checked == false)
                {
                    MessageBox.Show("โปรด เลือก รหัสเครื่องจักรผลิต ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (this.PANEL0102_MACHINE.Visible == false)
                    {
                        this.PANEL0102_MACHINE.Visible = true;
                        this.PANEL0102_MACHINE.BringToFront();
                        this.PANEL0102_MACHINE.Location = new Point(this.PANEL0102_MACHINE.Location.X, this.PANEL0102_MACHINE.Location.Y + 22);
                    }
                    else
                    {
                        this.PANEL0102_MACHINE.Visible = false;
                    }
                    return;
                }
            }
            else
            {

            }

            if (this.PANEL_MAT_txtmat_id.Text.ToString() == "")
            {
                MessageBox.Show("โปรดเลือก รหัสสินค้า ก่อน !! ");
                return;
            }
            if (this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.ToString() == "")
            {
                MessageBox.Show("โปรดเลือก รหัสอบหน้า ก่อน ");
                return;
            }
            //======================================================
            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                        if (this.ch_yokma.Checked == false)
                        {
                            //if (this.GridView1.Rows[i].Cells["Col_txtic_id"].Value.ToString() == this.txtic_id.Text.ToString())
                            //{
                            //    MessageBox.Show("เลขที่ นี้  เพิ่มเข้าไปในตาางแล้ว แล้ว!! ");
                            //    return;
                            //}
                        }
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() != this.PANEL_MAT_txtmat_id.Text.ToString())
                        {
                            MessageBox.Show("ในการบันทึก FG1   ใช้รหัสสินค้าได้ 1 รหัส เท่านั้น!! ");
                            return;
                        }
                        if (this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() != this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.ToString())
                        {
                            MessageBox.Show("ในการบันทึก FG1   ใช้รหัสอบหน้า 1 รหัส เท่านั้น!! ");
                            return;
                        }
                        if (this.GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Value.ToString() != this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString())
                        {
                            MessageBox.Show("ในการบันทึก FG1   ใช้รหัสเบอร์ด้าย 1 รหัส เท่านั้น!! ");
                            return;
                        }
                    if (this.GridView1.Rows[i].Cells["Col_txtmachine_id"].Value.ToString() != this.PANEL0102_MACHINE_txtmachine_id.Text.ToString())
                    {
                        MessageBox.Show("ในการบันทึก FG1   ใช้รหัสเครื่องจักร 1 รหัส เท่านั้น!! ");
                        return;
                    }
                }
            }


            FILL_To_GRID();
            GridView1_Add_Qty();

            this.btnGo1.Visible = false;
            this.btnGo1_RIB.Visible = false;




        }
        private void FILL_To_GRID()
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
            //Clear_GridView1();
            //===========================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    //"k021_mat_average.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*" +
                                    " FROM b001mat" +

                                    //" INNER JOIN k021_mat_average" +
                                    //" ON b001mat.cdkey = k021_mat_average.cdkey" +
                                    //" AND b001mat.txtco_id = k021_mat_average.txtco_id" +
                                    //" AND b001mat.txtmat_id = k021_mat_average.txtmat_id" +

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

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                    //" AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    int k = 0;
                    double z = 0;
                    z = Convert.ToDouble(this.txtfold_amount.Text);
                    double z2 = 0;
                    z2 = Convert.ToDouble(1);

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

                            //   string[] row = new string[] { k.ToString(), "", "", "", this.PANEL1306_WH_txtwherehouse_id.Text, this.txtmat_id.Text.ToString(), this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString() };
                            //======================================================
                            for (int i = 0; i < z; i++)
                            {
                                k = 1 + i;
                                string Lot_NO = DateTime.Now.ToString("yyMMddHHmmss", ThaiCulture) + "-" + this.PANEL0102_MACHINE_txtmachine_id.Text.Trim() + "-" + this.PANEL0105_FACE_BAKING_txtface_baking_name.Text.Trim() + "-" + k.ToString("00");

                                string[] row = new string[] { k.ToString(),   //"Col_Auto_num";
                                                                                                         "",  //"txtnumber_in_year"; 1
                                                                                                       this.PANEL1306_WH_txtwherehouse_id.Text.Trim(),  // "Col_txtwherehouse_id"; 2
                                                                                                        this.PANEL0102_MACHINE_txtmachine_id.Text.Trim(),  // "Col_txtmachine_id"; 3
                                                                                                        k.ToString("00"),  //"Col_txtfold_number"; 4
                                                                                                        ".00",  // "Col_txtqty"; 5

                                                                                                        "",  //"Col_txttrans_time_start";5
                                                                                                        "",  // "Col_txttrans_time_end";7

                                                                                                        "0",  // "Col_Problem1";8
                                                                                                        "0",  // "Col_Problem2";9
                                                                                                        "0",  // "Col_Problem3";10
                                                                                                        "0",  // "Col_Problem4";11

                                                                                                        "",  // "Col_txtemp_id";12
                                                                                                        "",  // "Col_txtemp_name";13
                                                                                                        "",  // "Col_txtshift_name"; 14
                                                                                                        "",  // "Col_txticrf_remark";15

                                                                                                        this.txtmat_no.Text.ToString(),  // "Col_txtmat_no";16
                                                                                                        this.PANEL_MAT_txtmat_id.Text.ToString(),  // "Col_txtmat_id";17
                                                                                                        this.PANEL_MAT_txtmat_name.Text.ToString(),  // "Col_txtmat_name";18

                                                                                                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString(),  // "Col_txtnumber_mat_id"; 19

                                                                                                        this.txtmat_unit1_name.Text.ToString(),  //"Col_txtmat_unit1_name"; 20
                                                                                                        Convert.ToSingle(this.txtmat_unit1_qty.Text).ToString("###,###.00"),  // "Col_txtmat_unit1_qty"; 21
                                                                                                        this.chmat_unit_status.Text.ToString(),  // "Col_chmat_unit_status";  22
                                                                                                        this.txtmat_unit2_name.Text.ToString(),   // "Col_txtmat_unit2_name";  23
                                                                                                        Convert.ToSingle(this.txtmat_unit2_qty.Text).ToString("###,###.00"),  // "Col_txtmat_unit1_qty";  24

                                                                                                        "0",  // "Col_txtqty2";  25


                                                                                                        "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00"),  //"Col_txtprice";  26
                                                                                                        "0",  // "Col_txtdiscount_rate"; 27
                                                                                                        "0",  // "Col_txtdiscount_money"; 28
                                                                                                        "0",  // "Col_txtsum_total"; 29

                                                                                                       "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00"),  // "Col_txtcost_qty_balance_yokma"; 30
                                                                                                       "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00"),  // "Col_txtcost_qty_price_average_yokma"; 31
                                                                                                       "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00"),  // "Col_txtcost_money_sum_yokma"; 32

                                                                                                        "0",  // "Col_txtcost_qty_balance_yokpai"; 33
                                                                                                        "0",  // "Col_txtcost_qty_price_average_yokpai";  34
                                                                                                        "0",  // "Col_txtcost_money_sum_yokpai";  35

                                                                                                        "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00"),  // "Col_txtcost_qty2_balance_yokma";  36
                                                                                                        "0",  // "Col_txtcost_qty2_balance_yokpai";  37

                                                                                                           k.ToString(),  // "Col_txtitem_no";  38
                                                                                                          "",  // "Col_mat_status"; 39
                                                                                                         this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim(),  // "Col_txtface_baking_id";  40
                                                                                                         Lot_NO.Trim(),  //41
                                                                                                         "0",  // "Col_txtqty_after_cut_";  42
                                                                                                         "0",  // "Col_txtqty_after_cut"; 43
                                                                                                          "0",  // "Col_txtqty_cut_yokma";  44
                                                                                                          "0",  // "Col_txtqty_cut_yokpai";  45
                                                                                                          "0",  // "Col_txtqty_after_cut_yokpai"; 46
                                                                                                          "0",  // "Col_txtqty_after_cut_yokpai"; 47
                                                                                                         this.txtic_id.Text.Trim(),  // "Col_txtic_id";  48
                                                                                                         "1",
                                                                                                          "0"
                                                                                                     };
                                GridView1.Rows.Add(row);
                            }
                            //====================================================== 

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {

                        MessageBox.Show("ไม่พบรหัสสินค้า " + this.PANEL_MAT_txtmat_id.Text.Trim() + "  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor.Current = Cursors.Default;
                        conn.Close();
                        return;
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
        private void btnGo1_RIB_Click(object sender, EventArgs e)
        {
            //if (this.txtnumber_in_year.Text == "")
            //{
            //    MessageBox.Show("โปรด ใส่เลขชุดที่ รับเข้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก คลังสินค้าที่ รับเข้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            //======================================================

            if (this.PANEL0102_MACHINE_txtmachine_name.Text == "")
            {
                if (this.ch_yokma.Checked == false)
                {
                    MessageBox.Show("โปรด เลือก รหัสเครื่องจักรผลิต ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (this.PANEL0102_MACHINE.Visible == false)
                    {
                        this.PANEL0102_MACHINE.Visible = true;
                        this.PANEL0102_MACHINE.BringToFront();
                        this.PANEL0102_MACHINE.Location = new Point(this.PANEL0102_MACHINE.Location.X, this.PANEL0102_MACHINE.Location.Y + 22);
                    }
                    else
                    {
                        this.PANEL0102_MACHINE.Visible = false;
                    }
                    return;
                }

            }
            else
            {

            }

            if (this.PANEL_MAT_txtmat_id.Text.ToString() == "")
            {
                MessageBox.Show("โปรดเลือก รหัสสินค้า ก่อน !! ");
                return;
            }
            if (this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.ToString() == "")
            {
                MessageBox.Show("โปรดเลือก รหัสอบหน้า ก่อน ");
                return;
            }
            //======================================================
            //======================================================
            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.ch_yokma.Checked == false)
                    {
                        //if (this.GridView1.Rows[i].Cells["Col_txtic_id"].Value.ToString() == this.txtic_id.Text.ToString())
                        //{
                        //    MessageBox.Show("เลขที่ นี้  เพิ่มเข้าไปในตาางแล้ว แล้ว!! ");
                        //    return;
                        //}
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() != this.PANEL_MAT_txtmat_id.Text.ToString())
                    {
                        MessageBox.Show("ในการบันทึก FG1   ใช้รหัสสินค้าได้ 1 รหัส เท่านั้น!! ");
                        return;
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() != this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.ToString())
                    {
                        MessageBox.Show("ในการบันทึก FG1   ใช้รหัสอบหน้า 1 รหัส เท่านั้น!! ");
                        return;
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Value.ToString() != this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString())
                    {
                        MessageBox.Show("ในการบันทึก FG1   ใช้รหัสเบอร์ด้าย 1 รหัส เท่านั้น!! ");
                        return;
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmachine_id"].Value.ToString() != this.PANEL0102_MACHINE_txtmachine_id.Text.ToString())
                    {
                        MessageBox.Show("ในการบันทึก FG1   ใช้รหัสเครื่องจักร 1 รหัส เท่านั้น!! ");
                        return;
                    }
                }
            }

            FILL_To_GRID_RIB();
            GridView1_Add_Qty();

            this.btnGo1_RIB.Visible = false;
            this.btnGo1.Visible = false;

        }
        private void FILL_To_GRID_RIB()
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
            //Clear_GridView1();
            //===========================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    //"k021_mat_average.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*" +
                                    " FROM b001mat" +

                                    //" INNER JOIN k021_mat_average" +
                                    //" ON b001mat.cdkey = k021_mat_average.cdkey" +
                                    //" AND b001mat.txtco_id = k021_mat_average.txtco_id" +
                                    //" AND b001mat.txtmat_id = k021_mat_average.txtmat_id" +

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

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                    //" AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    int k = 0;
                    double z = 0;
                    z = Convert.ToDouble(this.txtfold_amount.Text);
                    double z2 = 0;
                    z2 = Convert.ToDouble(1);

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

                            //   string[] row = new string[] { k.ToString(), "", "", "", this.PANEL1306_WH_txtwherehouse_id.Text, this.txtmat_id.Text.ToString(), this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString() };
                            //======================================================
   
                            //====================================================== 
                            for (int i = 0; i < z2; i++)
                            {
                                k = 1 + i;
                                string Lot_NO = DateTime.Now.ToString("yyMMddHHmmss", ThaiCulture) + "-" + this.PANEL0102_MACHINE_txtmachine_id.Text.Trim() + "-" + this.PANEL0105_FACE_BAKING_txtface_baking_name.Text.Trim() + "-RIB";

                                string[] row2 = new string[] { k.ToString(),   //"Col_Auto_num";
                                                                                                         "",  //"txtnumber_in_year";
                                                                                                        this.PANEL1306_WH_txtwherehouse_id.Text.Trim(),  // "Col_txtwherehouse_id";
                                                                                                        this.PANEL0102_MACHINE_txtmachine_id.Text.Trim(),  // "Col_txtmachine_id";
                                                                                                        "RIB",  //"Col_txtfold_number";
                                                                                                        ".00",  // "Col_txtqty";

                                                                                                        "",  //"Col_txttrans_time_start";
                                                                                                        "",  // "Col_txttrans_time_end";

                                                                                                        "0",  // "Col_Problem1";
                                                                                                        "0",  // "Col_Problem2";
                                                                                                        "0",  // "Col_Problem3";
                                                                                                        "0",  // "Col_Problem4";

                                                                                                        "",  // "Col_txtemp_id";
                                                                                                        "",  // "Col_txtemp_name";
                                                                                                        "",  // "Col_txtshift_name";
                                                                                                        "",  // "Col_txticrf_remark";

                                                                                                        this.txtmat_no.Text.ToString(),  // "Col_txtmat_no";
                                                                                                        this.PANEL_MAT_txtmat_id.Text.ToString(),  // "Col_txtmat_id";
                                                                                                        this.PANEL_MAT_txtmat_name.Text.ToString(),  // "Col_txtmat_name";

                                                                                                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString(),  // "Col_txtnumber_mat_id";

                                                                                                        this.txtmat_unit1_name.Text.ToString(),  //"Col_txtmat_unit1_name";
                                                                                                        Convert.ToSingle(this.txtmat_unit1_qty.Text).ToString("###,###.00"),  // "Col_txtmat_unit1_qty";
                                                                                                        this.chmat_unit_status.Text.ToString(),  // "Col_chmat_unit_status";
                                                                                                        this.txtmat_unit2_name.Text.ToString(),   // "Col_txtmat_unit2_name";
                                                                                                        Convert.ToSingle(this.txtmat_unit2_qty.Text).ToString("###,###.00"),  // "Col_txtmat_unit1_qty";

                                                                                                        "0",  // "Col_txtqty2";


                                                                                                        "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00"),  //"Col_txtprice";
                                                                                                        "0",  // "Col_txtdiscount_rate";
                                                                                                        "0",  // "Col_txtdiscount_money";
                                                                                                        "0",  // "Col_txtsum_total";

                                                                                                       "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00"),  // "Col_txtcost_qty_balance_yokma";
                                                                                                       "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00"),  // "Col_txtcost_qty_price_average_yokma";
                                                                                                       "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00"),  // "Col_txtcost_money_sum_yokma";

                                                                                                        "0",  // "Col_txtcost_qty_balance_yokpai";
                                                                                                        "0",  // "Col_txtcost_qty_price_average_yokpai";
                                                                                                        "0",  // "Col_txtcost_money_sum_yokpai";

                                                                                                        "0", // Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00"),  // "Col_txtcost_qty2_balance_yokma";
                                                                                                        "0",  // "Col_txtcost_qty2_balance_yokpai";

                                                                                                           k.ToString(),  // "Col_txtitem_no";
                                                                                                          "",  // "Col_mat_status";
                                                                                                         this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim(),  // "Col_txtface_baking_id";
                                                                                                         Lot_NO.Trim(),
                                                                                                           "0",  // "Col_txtqty_after_cut_";
                                                                                                         "0",  // "Col_txtqty_after_cut";
                                                                                                          "0",  // "Col_txtqty_cut_yokma";
                                                                                                          "0",  // "Col_txtqty_cut_yokpai";
                                                                                                          "0",  // "Col_txtqty_after_cut_yokpai";
                                                                                                           "0",  // "Col_txtqty_after_cut_yokpai"; 47
                                                                                                           this.txtic_id.Text.Trim(),  // "Col_txtic_id";
                                                                                                           "1",
                                                                                                            "0"
                                                                                                     };
                                GridView1.Rows.Add(row2);
                            }

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


                    }
                    else
                    {

                        MessageBox.Show("ไม่พบรหัสสินค้า " + this.PANEL_MAT_txtmat_id.Text.Trim() + "  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor.Current = Cursors.Default;
                        conn.Close();
                        return;
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
            this.GridView1.ColumnCount = 51;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtnumber_in_year";
            this.GridView1.Columns[2].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[3].Name = "Col_txtmachine_id";
            this.GridView1.Columns[4].Name = "Col_txtfold_number";

            this.GridView1.Columns[5].Name = "Col_txtqty";

            this.GridView1.Columns[6].Name = "Col_txttrans_time_start";
            this.GridView1.Columns[7].Name = "Col_txttrans_time_end";

            this.GridView1.Columns[8].Name = "Col_Problem1";
            this.GridView1.Columns[9].Name = "Col_Problem2";
            this.GridView1.Columns[10].Name = "Col_Problem3";
            this.GridView1.Columns[11].Name = "Col_Problem4";

            this.GridView1.Columns[12].Name = "Col_txtemp_id";
            this.GridView1.Columns[13].Name = "Col_txtemp_name";

            this.GridView1.Columns[14].Name = "Col_txtshift_name";

            this.GridView1.Columns[15].Name = "Col_txticrf_remark";


            this.GridView1.Columns[16].Name = "Col_txtmat_no";
            this.GridView1.Columns[17].Name = "Col_txtmat_id";
            this.GridView1.Columns[18].Name = "Col_txtmat_name";
            this.GridView1.Columns[19].Name = "Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";

            this.GridView1.Columns[20].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[21].Name = "Col_txtmat_unit1_qty";
            this.GridView1.Columns[22].Name = "Col_chmat_unit_status";
            this.GridView1.Columns[23].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[24].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[25].Name = "Col_txtqty2";

            this.GridView1.Columns[26].Name = "Col_txtprice";
            this.GridView1.Columns[27].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[28].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[29].Name = "Col_txtsum_total";

            this.GridView1.Columns[30].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[31].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[32].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[33].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[34].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[35].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[36].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns[37].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView1.Columns[38].Name = "Col_txtitem_no";
            this.GridView1.Columns[39].Name = "Col_mat_status";
            this.GridView1.Columns[40].Name = "Col_txtface_baking_id";
            this.GridView1.Columns[41].Name = "Col_txtlot_no";

            this.GridView1.Columns[42].Name = "Col_txtqty_after_cut_";
            this.GridView1.Columns[43].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[44].Name = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[45].Name = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[46].Name = "Col_txtqty_after_cut_yokpai";
            this.GridView1.Columns[47].Name = "Col_txtqty_after_cut_yokpai";

            this.GridView1.Columns[48].Name = "Col_txtic_id";
            this.GridView1.Columns[49].Name = "Col_1";
            this.GridView1.Columns[50].Name = "Col_2";

            this.GridView1.Columns[42].Visible = false;
            this.GridView1.Columns[43].Visible = false;
            this.GridView1.Columns[44].Visible = false;
            this.GridView1.Columns[45].Visible = false;
            this.GridView1.Columns[46].Visible = false;
            this.GridView1.Columns[47].Visible = false;
            this.GridView1.Columns[48].Visible = true;
            this.GridView1.Columns[49].Visible = false;
            this.GridView1.Columns[50].Visible = false;
            //this.GridView1.Columns[41].Width = 150;
            //this.GridView1.Columns[42].Width = 150;
            //this.GridView1.Columns[43].Width = 150;
            //this.GridView1.Columns[44].Width = 150;
            //this.GridView1.Columns[45].Width = 150;

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "เลขที่ชุด";
            this.GridView1.Columns[2].HeaderText = "คลัง";
            this.GridView1.Columns[3].HeaderText = "เครื่องจักร";
            this.GridView1.Columns[4].HeaderText = "ม้วนที่";

            this.GridView1.Columns[5].HeaderText = "น้ำหนัก/ม้วน(กก.)";

            this.GridView1.Columns[6].HeaderText = " เวลาเริ่ม";
            this.GridView1.Columns[7].HeaderText = " เวลาเสร็จ";

            this.GridView1.Columns[8].HeaderText = "เข็มหัก";
            this.GridView1.Columns[9].HeaderText = "เป็นรู";
            this.GridView1.Columns[10].HeaderText = "ผ้าตก";
            this.GridView1.Columns[11].HeaderText = "ด้ายขาด";

            this.GridView1.Columns[12].HeaderText = "รหัสผู้ดูแล";
            this.GridView1.Columns[13].HeaderText = "ชื่อผู้ดูแล";
            this.GridView1.Columns[14].HeaderText = "กะ";
            this.GridView1.Columns[15].HeaderText = "หมายเหตุ";

            this.GridView1.Columns[16].HeaderText = "ลำดับ";
            this.GridView1.Columns[17].HeaderText = "รหัส";
            this.GridView1.Columns[18].HeaderText = "ชื่อสินค้า";
            this.GridView1.Columns[19].HeaderText = "เบอร์เส้นด้าย";

            this.GridView1.Columns[20].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[21].HeaderText = " หน่วย";
            this.GridView1.Columns[22].HeaderText = "แปลง";
            this.GridView1.Columns[23].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[24].HeaderText = " หน่วย";

            this.GridView1.Columns[25].HeaderText = "น้ำหนัก/ม้วน(ปอนด์)";

            this.GridView1.Columns[26].HeaderText = "ราคา";
            this.GridView1.Columns[27].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[28].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[29].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[30].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[31].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[32].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[33].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[34].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[35].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[36].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView1.Columns[37].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView1.Columns[38].HeaderText = "item_no";
            this.GridView1.Columns[39].HeaderText = "สถานะ";
            this.GridView1.Columns[40].HeaderText = "อบหน้า";
            this.GridView1.Columns[41].HeaderText = "Lot No";

            this.GridView1.Columns[42].HeaderText = "Col_txtqty_after_cut ยกมา";
            this.GridView1.Columns[43].HeaderText = "รวมจำนวนรับคืนแล้วยกมา";
            this.GridView1.Columns[44].HeaderText = "รวมจำนวนรับคืนแล้วยกไป";
            this.GridView1.Columns[45].HeaderText = "เหลือรอรับอีก กก.";
            this.GridView1.Columns[46].HeaderText = "Col_txtqty_after_cut_yokpai";
            this.GridView1.Columns[47].HeaderText = "Col_txtqty_after_cut_yokpai.";

            this.GridView1.Columns[48].HeaderText = "เลขที่ใบเบิกด้าย";
            this.GridView1.Columns[49].HeaderText = "1";
            this.GridView1.Columns[50].HeaderText = "1";

            this.GridView1.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 0;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = false;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_in_year"].Visible = true;  //"Col_txtnumber_in_year";
            this.GridView1.Columns["Col_txtnumber_in_year"].Width = 100;
            this.GridView1.Columns["Col_txtnumber_in_year"].ReadOnly = false;
            this.GridView1.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView1.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView1.Columns["Col_txtmachine_id"].Width = 80;
            this.GridView1.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView1.Columns["Col_txtfold_number"].Width = 60;
            this.GridView1.Columns["Col_txtfold_number"].ReadOnly = false;
            this.GridView1.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width =140;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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

            this.GridView1.Columns["Col_txttrans_time_start"].Visible = false;  //"Col_txttrans_time_start";
            this.GridView1.Columns["Col_txttrans_time_start"].Width = 0;
            this.GridView1.Columns["Col_txttrans_time_start"].ReadOnly = false;
            this.GridView1.Columns["Col_txttrans_time_start"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time_start"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txttrans_time_end"].Visible = false;  //"Col_txttrans_time_end";
            this.GridView1.Columns["Col_txttrans_time_end"].Width = 0;
            this.GridView1.Columns["Col_txttrans_time_end"].ReadOnly = false;
            this.GridView1.Columns["Col_txttrans_time_end"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time_end"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_Problem1"].Visible = false;  //"Col_Problem1";
            this.GridView1.Columns["Col_Problem1"].Width = 0;
            this.GridView1.Columns["Col_Problem1"].ReadOnly = false;
            this.GridView1.Columns["Col_Problem1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Problem1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_Problem2"].Visible = false;  //"Col_Problem2";
            this.GridView1.Columns["Col_Problem2"].Width = 0;
            this.GridView1.Columns["Col_Problem2"].ReadOnly = false;
            this.GridView1.Columns["Col_Problem2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Problem2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_Problem3"].Visible = false;  //"Col_Problem3";
            this.GridView1.Columns["Col_Problem3"].Width = 0;
            this.GridView1.Columns["Col_Problem3"].ReadOnly = false;
            this.GridView1.Columns["Col_Problem3"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Problem3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_Problem4"].Visible = false;  //"Col_Problem4";
            this.GridView1.Columns["Col_Problem4"].Width = 0;
            this.GridView1.Columns["Col_Problem4"].ReadOnly = false;
            this.GridView1.Columns["Col_Problem4"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Problem4"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


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

            this.GridView1.Columns["Col_txtemp_name"].Visible = false;  //"Col_txtemp_name";
            this.GridView1.Columns["Col_txtemp_name"].Width = 0;
            this.GridView1.Columns["Col_txtemp_name"].ReadOnly = false;
            this.GridView1.Columns["Col_txtemp_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtemp_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtshift_name"].Visible = false;  //"Col_txtshift_name";
            this.GridView1.Columns["Col_txtshift_name"].Width = 0;
            this.GridView1.Columns["Col_txtshift_name"].ReadOnly = false;
            this.GridView1.Columns["Col_txtshift_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtshift_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txticrf_remark"].Visible = false;  //"Col_txticrf_remark";
            this.GridView1.Columns["Col_txticrf_remark"].Width = 0;
            this.GridView1.Columns["Col_txticrf_remark"].ReadOnly = false;
            this.GridView1.Columns["Col_txticrf_remark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txticrf_remark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


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
            dgvCmb.Width = 0;  //70
            dgvCmb.DisplayIndex = 20;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.Visible = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb);

            this.GridView1.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.GridView1.Columns["Col_txtmat_unit2_name"].Width =0;
            this.GridView1.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView1.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;




            this.GridView1.Columns["Col_txtqty2"].Visible = true;  //"Col_txtqty2";
            this.GridView1.Columns["Col_txtqty2"].Width =100;
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
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].Width =0;
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

            this.GridView1.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";
            this.GridView1.Columns["Col_mat_status"].Width = 0;
            this.GridView1.Columns["Col_mat_status"].ReadOnly = true;
            this.GridView1.Columns["Col_mat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_mat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            this.GridView1.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView1.Columns["Col_txtface_baking_id"].Width = 80;
            this.GridView1.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtlot_no"].Visible = true;  //"Col_txtlot_no";
            this.GridView1.Columns["Col_txtlot_no"].Width = 160;
            this.GridView1.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtic_id"].Visible = true;  //"Col_txtic_id";
            this.GridView1.Columns["Col_txtic_id"].Width = 160;
            this.GridView1.Columns["Col_txtic_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtic_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtic_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_1"].Visible = false;  //"Col_1";
            this.GridView1.Columns["Col_1"].Width = 0;
            this.GridView1.Columns["Col_1"].ReadOnly = true;
            this.GridView1.Columns["Col_1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
            GridView1_Cal_Sum_M();
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

                GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtfold_number"].Style.BackColor = Color.LightSkyBlue;

                GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txttrans_time_start"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txttrans_time_end"].Style.BackColor = Color.LightSkyBlue;

                GridView1.Rows[i].Cells["Col_Problem1"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_Problem2"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_Problem3"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_Problem4"].Style.BackColor = Color.LightSkyBlue;

                GridView1.Rows[i].Cells["Col_txtemp_name"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtshift_name"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txticrf_remark"].Style.BackColor = Color.LightSkyBlue;

            }
        }
        private void GridView1_Cal_Sum()
        {


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

            double Sum_Qty_IC = 0;
            double Sum_Qty_IC_R = 0;
            double Sum_row = 0;

            double C1 = 0;
            double C1YP = 0;


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

                     if (double.Parse(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {

                        this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value).ToString("###,###.00");     //7
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value).ToString("###,###.0000");     //7


                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty"].Value).ToString("###,###.00");     //7
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty2"].Value).ToString("###,###.00");     //8

                        this.GridView1.Rows[i].Cells["Col_txtprice"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtprice"].Value).ToString("###,###.00");     //6
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value).ToString("###,###.00");     //7
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value).ToString("###,###.00");     //8
                        this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value).ToString("###,###.00");     //8

                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value).ToString("###,###.00");     //6
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value).ToString("###,###.00");     //7
                        this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value).ToString("###,###.00");     //8

                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value).ToString("###,###.00");     //8
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value).ToString("###,###.00");     //8
                        this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value).ToString("###,###.00");     //8

                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value).ToString("###,###.00");     //8
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8


                    }

                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  จำนวนเบิก (กก)=================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));


                        //============================================================================================================

                        //============================================================================================================
                        //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                        if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                        {
                            Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                            this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                            //Sum2_Qty_Yokpai  =================================================
                            Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                            this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                            //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                            Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                            this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));

                        }

                        Sum_row = Convert.ToDouble(string.Format("{0:n4}", Sum_row)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtcount_rows.Text = Sum_row.ToString("N", new CultureInfo("en-US"));

                    }


                    //หายอดสูญเสีย
                    this.txtsum_qty_yes.Text = this.txtsum_qty.Text;
                    Sum_Qty_IC = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_ic.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_yes.Text.ToString()));
                    this.txtsum_qty_change.Text = Sum_Qty_IC.ToString("N", new CultureInfo("en-US"));

                    Sum_Qty_IC_R = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_change.Text.ToString())) * 100 / Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_ic.Text.ToString()));
                    this.txtsum_qty_change_rate.Text = Sum_Qty_IC_R.ToString("N", new CultureInfo("en-US"));



                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                    this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                    this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                    QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                    this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                    //1.เหลือยกมา + ผลิต = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา + มูลค่าผลิต = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString()));
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

                    //1.เหลือ(2)ยกมา + ผลิต(2) = จำนวนเหลือ(2)ทั้งสิ้น
                    Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));
                    this.txtcost_qty2_balance_yokpai.Text = Qbypai2.ToString("N", new CultureInfo("en-US"));

                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                    //C2==================================
                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        this.GridView1.Rows[i].Cells["Col_2"].Value = "1";
                        C1 = Convert.ToDouble(string.Format("{0:n4}", C1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_2"].Value.ToString()));
                        this.txtcost_qty1_balance.Text = C1.ToString("N", new CultureInfo("en-US"));
                        C1YP = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance.Text.ToString()));
                        this.txtcost_qty1_balance_yokpai.Text = C1YP.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.GridView1.Rows[i].Cells["Col_2"].Value = "0";
                        C1 = Convert.ToDouble(string.Format("{0:n4}", C1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_2"].Value.ToString()));
                        this.txtcost_qty1_balance.Text = C1.ToString("N", new CultureInfo("en-US"));
                        C1YP = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance.Text.ToString()));
                        this.txtcost_qty1_balance_yokpai.Text = C1YP.ToString("N", new CultureInfo("en-US"));

                    }
                    //==================================            
                }
             }

                //this.txtcount_rows.Text = k.ToString();


                Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            QAbyma = 0;
            QAbyma2 = 0;
            Qbypai = 0;
            Qbypai2 = 0;
            Mbypai = 0;
            QAbypai = 0;

             Sum_Qty_IC = 0;
             Sum_Qty_IC_R = 0;

             Sum_row = 0;

             C1 = 0;
             C1YP = 0;

        }
        private void GridView1_Run_Lot_Num()
        {
            //this.GridView1.Columns[2].Name = "Col_txtmachine_id";
            //this.GridView1.Columns[3].Name = "Col_txtfold_number";
            //this.GridView1.Columns[39].Name = "Col_txtface_baking_id";
            //this.GridView1.Columns[40].Name = "Col_txtlot_no";
            //string Lot_NO = DateTime.Now.ToString("yyMMddHHmmss", ThaiCulture) + "-" + this.PANEL0102_MACHINE_txtmachine_id.Text.Trim() + "-" + this.PANEL0105_FACE_BAKING_txtface_baking_name.Text.Trim() + "-RIB";

            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value = DateTime.Now.ToString("yyMMddHHmmss", ThaiCulture) + "-" + this.GridView1.Rows[i].Cells["Col_txtmachine_id"].Value + "-" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value + "-" + this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString();
                }
            }


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

                            this.txtcost_qty1_balance_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty1_balance"]).ToString("###,###.00");        //18
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
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_NOvat")  //ซื้อไม่มีvat
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


        private void Fill_GridView1_Machine()
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

            Clear_GridView1_machine();


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

                            var index = GridView1_Machine.Rows.Add();
                            GridView1_Machine.Rows[index].Cells["Col_txtic_id"].Value = ""; //0
                            GridView1_Machine.Rows[index].Cells["Col_txtmachine_no"].Value = dt2.Rows[j]["txtmachine_no"].ToString();      //1
                            GridView1_Machine.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                            GridView1_Machine.Rows[index].Cells["Col_txtmachine_name"].Value = dt2.Rows[j]["txtmachine_name"].ToString();      //3
                            GridView1_Machine.Rows[index].Cells["Col_txtsum_qty_ic"].Value = "0";      //4
                            GridView1_Machine.Rows[index].Cells["Col_txtsum_qty_yes"].Value = "0";      //5
                            GridView1_Machine.Rows[index].Cells["Col_txtsum_qty_change"].Value = "0";      //6
                            GridView1_Machine.Rows[index].Cells["Col_txtsum_qty_change_rate"].Value = "0";      //7
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
        private void Show_GridView1_Machine()
        {
            this.GridView1_Machine.ColumnCount = 8;
            this.GridView1_Machine.Columns[0].Name = "Col_txtic_id";
            this.GridView1_Machine.Columns[1].Name = "Col_txtmachine_no";
            this.GridView1_Machine.Columns[2].Name = "Col_txtmachine_id";
            this.GridView1_Machine.Columns[3].Name = "Col_txtmachine_name";
            this.GridView1_Machine.Columns[4].Name = "Col_txtsum_qty_ic";
            this.GridView1_Machine.Columns[5].Name = "Col_txtsum_qty_yes";
            this.GridView1_Machine.Columns[6].Name = "Col_txtsum_qty_change";
            this.GridView1_Machine.Columns[7].Name = "Col_txtsum_qty_change_rate";

            this.GridView1_Machine.Columns[0].HeaderText = "No";
            this.GridView1_Machine.Columns[1].HeaderText = "ลำดับ";
            this.GridView1_Machine.Columns[2].HeaderText = " รหัส";
            this.GridView1_Machine.Columns[3].HeaderText = " ชื่อรหัสเครื่องจักร";
            this.GridView1_Machine.Columns[4].HeaderText = "Col_txtsum_qty_ic";
            this.GridView1_Machine.Columns[5].HeaderText = "Col_txtsum_qty_yes";
            this.GridView1_Machine.Columns[6].HeaderText = "Col_txtsum_qty_change";
            this.GridView1_Machine.Columns[7].HeaderText = "Col_txtsum_qty_change_rate";

            this.GridView1_Machine.Columns[0].Visible = false;
            this.GridView1_Machine.Columns[1].Visible = false;
            this.GridView1_Machine.Columns[3].Visible = false;

            this.GridView1_Machine.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1_Machine.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1_Machine.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1_Machine.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1_Machine.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1_Machine.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1_Machine.EnableHeadersVisualStyles = false;


        }
        private void Clear_GridView1_machine()
        {
            this.GridView1_Machine.Rows.Clear();
            this.GridView1_Machine.Refresh();
        }


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


        //txtproduce_type ประเภทผลิต  =======================================================================
        private void PANEL0104_PRODUCE_TYPE_Fill_produce_type()
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

            PANEL0104_PRODUCE_TYPE_Clear_GridView1_produce_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_04produce_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtproduce_type_id <> '')" +
                                    " ORDER BY txtproduce_type_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtproduce_type_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtproduce_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtproduce_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtproduce_type_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtproduce_type_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtproduce_type_status";

                            var index = PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows.Add();
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_no"].Value = dt2.Rows[j]["txtproduce_type_no"].ToString();      //1
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_id"].Value = dt2.Rows[j]["txtproduce_type_id"].ToString();      //2
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //3
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_name_eng"].Value = dt2.Rows[j]["txtproduce_type_name_eng"].ToString();      //4
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_remark"].Value = dt2.Rows[j]["txtproduce_type_remark"].ToString();      //5
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_status"].Value = dt2.Rows[j]["txtproduce_type_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type_Up_Status();

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
        private void PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows.Count; i++)
            {
                if (this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0104_PRODUCE_TYPE_GridView1_produce_type()
        {
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.ColumnCount = 7;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].Name = "Col_txtproduce_type_no";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].Name = "Col_txtproduce_type_id";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].Name = "Col_txtproduce_type_name";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].Name = "Col_txtproduce_type_name_eng";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].Name = "Col_txtproduce_type_remark";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[6].Name = "Col_txtproduce_type_status";

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[0].HeaderText = "No";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].HeaderText = " รหัส";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].HeaderText = "ชื่อประเภทผลิต";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].HeaderText = "ชื่อประเภทผลิต Eng";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[6].HeaderText = " สถานะ";

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[0].Visible = false;  //"No";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].Visible = true;  //"Col_txtproduce_type_no";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].Width = 90;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].ReadOnly = true;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].Visible = true;  //"Col_txtproduce_type_id";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].Width = 80;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].ReadOnly = true;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].Visible = true;  //"Col_txtproduce_type_name";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].Width = 150;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].ReadOnly = true;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].Visible = false;  //"Col_txtproduce_type_name_eng";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].Width = 0;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].ReadOnly = true;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].Visible = false;  //"Col_txtproduce_type_name_remark";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].Width = 0;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].ReadOnly = true;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[6].Visible = false;  //"Col_txtproduce_type_status";
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[6].Width = 0;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Columns.Add(dgvCmb);

        }
        private void PANEL0104_PRODUCE_TYPE_Clear_GridView1_produce_type()
        {
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows.Clear();
            this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Refresh();
        }
        private void PANEL0104_PRODUCE_TYPE_txtproduce_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0104_PRODUCE_TYPE.Visible == false)
                {
                    this.PANEL0104_PRODUCE_TYPE.Visible = true;
                    this.PANEL0104_PRODUCE_TYPE.Location = new Point(this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Location.X, this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Location.Y + 22);
                    this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Focus();
                }
                else
                {
                    this.PANEL0104_PRODUCE_TYPE.Visible = false;
                }
        }
        private void PANEL0104_PRODUCE_TYPE_btnproduce_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL0104_PRODUCE_TYPE.Visible == false)
            {
                this.PANEL0104_PRODUCE_TYPE.Visible = true;
                this.PANEL0104_PRODUCE_TYPE.BringToFront();
                this.PANEL0104_PRODUCE_TYPE.Location = new Point(this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Location.X, this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0104_PRODUCE_TYPE.Visible = false;
            }
        }
        private void PANEL0104_PRODUCE_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0104_PRODUCE_TYPE.Visible == false)
            {
                this.PANEL0104_PRODUCE_TYPE.Visible = true;
            }
            else
            {
                this.PANEL0104_PRODUCE_TYPE.Visible = false;
            }
        }
        private void PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0104_PRODUCE_TYPE_txtproduce_type_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.CurrentRow.Index;

                this.PANEL0104_PRODUCE_TYPE_txtproduce_type_id.Text = PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Text = PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0104_PRODUCE_TYPE_txtproduce_type_name.Focus();
                this.PANEL0104_PRODUCE_TYPE.Visible = false;
            }
        }
        private void PANEL0104_PRODUCE_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0104_PRODUCE_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL0104_PRODUCE_TYPE_Clear_GridView1_produce_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_04produce_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtproduce_type_name LIKE '%" + this.PANEL0104_PRODUCE_TYPE_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtproduce_type_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtproduce_type_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtproduce_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtproduce_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtproduce_type_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtproduce_type_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtproduce_type_status";

                            var index = PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows.Add();
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_no"].Value = dt2.Rows[j]["txtproduce_type_no"].ToString();      //1
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_id"].Value = dt2.Rows[j]["txtproduce_type_id"].ToString();      //2
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //3
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_name_eng"].Value = dt2.Rows[j]["txtproduce_type_name_eng"].ToString();      //4
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_remark"].Value = dt2.Rows[j]["txtproduce_type_remark"].ToString();      //5
                            PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type.Rows[index].Cells["Col_txtproduce_type_status"].Value = dt2.Rows[j]["txtproduce_type_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0104_PRODUCE_TYPE_dataGridView1_produce_type_Up_Status();

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
        private void PANEL0104_PRODUCE_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0104_PRODUCE_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0104_PRODUCE_TYPE.Height = PANEL0104_PRODUCE_TYPE_btnresize_low.Top + e.Y;
                this.PANEL0104_PRODUCE_TYPE.Width = PANEL0104_PRODUCE_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0104_PRODUCE_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0104_PRODUCE_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtproduce_type ประเภทผลิต  =======================================================================


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


        //txtface_baking ประเภท อบหน้า  =======================================================================
        private void PANEL0105_FACE_BAKING_Fill_face_baking()
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

            PANEL0105_FACE_BAKING_Clear_GridView1_face_baking();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_05face_baking" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtface_baking_id <> '')" +
                                    " ORDER BY txtface_baking_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtface_baking_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtface_baking_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtface_baking_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtface_baking_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtface_baking_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtface_baking_status";

                            var index = PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Add();
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_no"].Value = dt2.Rows[j]["txtface_baking_no"].ToString();      //1
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();      //2
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //3
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name_eng"].Value = dt2.Rows[j]["txtface_baking_name_eng"].ToString();      //4
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_remark"].Value = dt2.Rows[j]["txtface_baking_remark"].ToString();      //5
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_status"].Value = dt2.Rows[j]["txtface_baking_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0105_FACE_BAKING_dataGridView1_face_baking_Up_Status();

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
        private void PANEL0105_FACE_BAKING_dataGridView1_face_baking_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Count; i++)
            {
                if (this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0105_FACE_BAKING_GridView1_face_baking()
        {
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnCount = 7;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].Name = "Col_Auto_num";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Name = "Col_txtface_baking_no";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Name = "Col_txtface_baking_id";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Name = "Col_txtface_baking_name";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Name = "Col_txtface_baking_name_eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Name = "Col_txtface_baking_remark";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].Name = "Col_txtface_baking_status";

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].HeaderText = "No";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].HeaderText = " รหัส";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].HeaderText = " อบหน้า";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].HeaderText = " อบหน้า  Eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].HeaderText = " สถานะ";

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].Visible = false;  //"No";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Visible = false;  //"Col_txtface_baking_no";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Visible = false;  //"Col_txtface_baking_id";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Visible = true;  //"Col_txtface_baking_id";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Width = 150;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Visible = false;  //"Col_txtface_baking_id_eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Visible = false;  //"Col_txtface_baking_id_remark";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].Visible = false;  //"Col_txtface_baking_status";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns.Add(dgvCmb);

        }
        private void PANEL0105_FACE_BAKING_Clear_GridView1_face_baking()
        {
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Clear();
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Refresh();
        }
        private void PANEL0105_FACE_BAKING_txtface_baking_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0105_FACE_BAKING.Visible == false)
                {
                    this.PANEL0105_FACE_BAKING.Visible = true;
                    this.PANEL0105_FACE_BAKING.Location = new Point(this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.X, this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.Y + 22);
                    this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Focus();
                }
                else
                {
                    this.PANEL0105_FACE_BAKING.Visible = false;
                }
        }
        private void PANEL0105_FACE_BAKING_btnface_baking_Click(object sender, EventArgs e)
        {
            if (this.PANEL0105_FACE_BAKING.Visible == false)
            {
                this.PANEL0105_FACE_BAKING.Visible = true;
                this.PANEL0105_FACE_BAKING.BringToFront();
                this.PANEL0105_FACE_BAKING.Location = new Point(this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.X, this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0105_FACE_BAKING.Visible = false;
            }
        }
        private void PANEL0105_FACE_BAKING_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0105_FACE_BAKING.Visible == false)
            {
                this.PANEL0105_FACE_BAKING.Visible = true;
            }
            else
            {
                this.PANEL0105_FACE_BAKING.Visible = false;
            }
        }
        private void PANEL0105_FACE_BAKING_dataGridView1_face_baking_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0105_FACE_BAKING_dataGridView1_face_baking_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0105_FACE_BAKING_dataGridView1_face_baking.CurrentRow.Index;

                this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = PANEL0105_FACE_BAKING_dataGridView1_face_baking.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = PANEL0105_FACE_BAKING_dataGridView1_face_baking.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0105_FACE_BAKING_txtface_baking_name.Focus();
                this.PANEL0105_FACE_BAKING.Visible = false;
            }
        }
        private void PANEL0105_FACE_BAKING_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0105_FACE_BAKING_btn_search_Click(object sender, EventArgs e)
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

            PANEL0105_FACE_BAKING_Clear_GridView1_face_baking();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_05face_baking" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtface_baking_name LIKE '%" + this.PANEL0105_FACE_BAKING_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtface_baking_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtface_baking_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtface_baking_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtface_baking_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtface_baking_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtface_baking_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtface_baking_status";

                            var index = PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Add();
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_no"].Value = dt2.Rows[j]["txtface_baking_no"].ToString();      //1
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();      //2
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //3
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name_eng"].Value = dt2.Rows[j]["txtface_baking_name_eng"].ToString();      //4
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_remark"].Value = dt2.Rows[j]["txtface_baking_remark"].ToString();      //5
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_status"].Value = dt2.Rows[j]["txtface_baking_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0105_FACE_BAKING_dataGridView1_face_baking_Up_Status();

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
        private void PANEL0105_FACE_BAKING_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0105_FACE_BAKING_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0105_FACE_BAKING.Height = PANEL0105_FACE_BAKING_btnresize_low.Top + e.Y;
                this.PANEL0105_FACE_BAKING.Width = PANEL0105_FACE_BAKING_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0105_FACE_BAKING_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0105_FACE_BAKING_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtface_baking ประเภท อบหน้า =======================================================================

        //END txtberg_type ประเภทเบิกคลัง  =======================================================================


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
                                     " AND (b001mat_02detail.txtmat_sac_id = '" + this.txtmat_sac_id.Text.Trim() + "')" +    //ผ้าดิบ
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

                    Clear_GridView1();

                    this.PANEL_MAT_txtmat_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL_MAT_txtmat_name.Text = row.Cells[3].Value.ToString();
                    if (this.PANEL_MAT_txtmat_id.Text.ToString() == "RIB")
                    {
                        this.btnGo1.Visible = false;
                        this.btnGo1_RIB.Visible = true;
                    }
                    else
                    {
                        this.btnGo1.Visible = true;
                        this.btnGo1_RIB.Visible = false;

                    }
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



        private void Fill_cboemp()
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
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !!  ", "Performance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                ArrayList StringList = new ArrayList();
                //=======================================================

                cmd2.CommandText = "SELECT *" +
                                  " FROM a003db_user" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  //      " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtemp_id <> '')" +
                                  " ORDER BY ID ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        //DataGridViewComboBoxColumn cboemp = new DataGridViewComboBoxColumn();
                        //cboemp.Name = "Col_Combo1";
                        //cboemp.Width = 150;
                        //cboemp.DisplayIndex = 21;
                        //cboemp.HeaderText = "ผู้เบิก...";
                        //cboemp.MaxDropDownItems = 2;
                        ////cboemp.ValueType = typeof(bool);
                        ////cboemp.ReadOnly = false;

                        //cboemp.Items.Add("true1");
                        //cboemp.Items.Add("true2");
                        //cboemp.Items.Add("true3");
                        //cboemp.Items.Add("true4");

                        //cboemp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //cboemp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //cboemp.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        //this.GridView1.Columns.Add(cboemp);

                        //=========================================================================
                        //var index = GridView1.Rows.Add();
                        //GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                        //GridView1.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtemp_id.ToString();      //1
                        //GridView1.Rows[index].Cells["Col_txtname"].Value = cipherText_txtemp_name.ToString();      //2

                        foreach (DataRow item in dt2.Rows)
                        {
                            //=======================================================
                            //ใส่รหัสฐานข้อมูล============================================
                            string clearText_txtemp_name = item["txtname"].ToString();
                            string cipherText_txtemp_name = W_CryptorEngine.Decrypt(clearText_txtemp_name, true);
                            //=========================================================================

                            StringList.Add(cipherText_txtemp_name.ToString());

                        }
                        int k = 0;
                        double z = 0;
                        z = Convert.ToDouble(this.txtfold_amount.Text);

                        for (int i = 0; i < z; i++)
                        {

                            var CellSample = new DataGridViewComboBoxCell();
                            CellSample.DataSource = StringList;

                            GridView1.Rows[i].Cells["Col_Combo1"] = CellSample;
                        }



                        //for (int j = 0; j < dt2.Rows.Count; j++)
                        //{
                        //    //ใส่รหัสฐานข้อมูล============================================
                        //    //ใส่รหัสฐานข้อมูล user============================================
                        //    string clearText_txtemp_id = dt2.Rows[j]["txtemp_id"].ToString();      //1
                        //    string cipherText_txtemp_id = W_CryptorEngine.Decrypt(clearText_txtemp_id, true);
                        //    //=======================================================

                        //    //=======================================================
                        //    //ใส่รหัสฐานข้อมูล============================================
                        //    string clearText_txtemp_name = dt2.Rows[j]["txtname"].ToString();
                        //    string cipherText_txtemp_name = W_CryptorEngine.Decrypt(clearText_txtemp_name, true);
                        //    //=========================================================================
                        //    string[] StringList = {cipherText_txtemp_id.ToString(),cipherText_txtemp_name.ToString() };

                        //    foreach (DataRow item in dt2.Rows)
                        //    {
                        //        int n = GridView1.Rows.Add();

                        //        var CellSample = new DataGridViewComboBoxCell();
                        //        CellSample.DataSource = StringList;

                        //        GridView1.Rows[j].Cells["Col_Combo1"] = CellSample;
                        //    }

                        //}
                        //

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

                    cmd2.CommandText = "UPDATE c002_02produce_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txticrf_id = '" + this.txticrf_id.Text.Trim() + "')";
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
                                  " FROM c002_02produce_record_trans" +
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
                            TMP = "FG1" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "FG1" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "FG1" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txticrf_id.Text = TMP.Trim();
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
                                            " FROM c002_02produce_record_for_load_first" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                             " AND (txtcomputer_name = '" + W_ID_Select.COMPUTER_NAME.Trim() + "')" +
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
                            //this.txtnumber_in_year.Text = dt2.Rows[0]["txtnumber_in_year"].ToString();
                            this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                            this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();
                            this.PANEL0102_MACHINE_txtmachine_id.Text = dt2.Rows[0]["txtmachine_id"].ToString();
                            this.PANEL0102_MACHINE_txtmachine_name.Text = dt2.Rows[0]["txtmachine_name"].ToString();
                            this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = dt2.Rows[0]["txtface_baking_id"].ToString();
                            this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = dt2.Rows[0]["txtface_baking_name"].ToString();
                            //this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                            //this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                            if (dt2.Rows[0]["txtstatus_import"].ToString() == "Y")
                            {
                                //this.check_import.Checked = true;
                                //this.GridView_Import.Visible = true;
                                //this.GridView66.Visible = false;
                            }
                            else
                            {
                                //this.check_import.Checked = false;
                                //this.GridView_Import.Visible = false;
                                //this.GridView66.Visible = true;
                            }

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
                                            " FROM c002_02produce_record_for_load_first" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                             " AND (txtcomputer_name = '" + W_ID_Select.COMPUTER_NAME.Trim() + "')" +
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

                        cmd2.CommandText = "INSERT INTO c002_02produce_record_for_load_first(cdkey,txtco_id," +  //1
                       "txtbranch_id," +  //2
                       "txtnumber_in_year," +  //3
                       "txtwherehouse_id," +  //3
                       "txtwherehouse_name," +  //4
                       "txtmachine_id," +  //4
                       "txtmachine_name," +  //4
                       "txtface_baking_id," +  //4
                       "txtface_baking_name," +  //4
                       "txtmat_id," +  //5
                       "txtmat_name," +  //5
                       "txtnumber_mat_id," +  //5
                       "txtnumber_mat_name," +
                        "txtcomputer_name," +
                       "txtstatus_import) " +  //6
                       "VALUES (@cdkey,@txtco_id," +  //1
                       "@txtbranch_id," +  //2
                       "@txtnumber_in_year," +  //3
                       "@txtwherehouse_id," +  //3
                       "@txtwherehouse_name," +  //4
                       "@txtmachine_id," +  //4
                       "@txtmachine_name," +  //4
                       "@txtface_baking_id," +  //4
                       "@txtface_baking_name," +  //4
                       "@txtmat_id," +  //5
                       "@txtmat_name," +  //5
                       "@txtnumber_mat_id," +  //5
                       "@txtnumber_mat_name," +
                        "@txtcomputer_name," +
                        "@txtstatus_import)";   //14

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();  //1

                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //2
                        cmd2.Parameters.Add("@txtnumber_in_year", SqlDbType.NVarChar).Value = this.txtnumber_in_year.Text.ToString();  //3
                        cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();  //3
                        cmd2.Parameters.Add("@txtwherehouse_name", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_name.Text.ToString();  //4
                        cmd2.Parameters.Add("@txtmachine_id", SqlDbType.NVarChar).Value = this.PANEL0102_MACHINE_txtmachine_id.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtmachine_name", SqlDbType.NVarChar).Value = this.PANEL0102_MACHINE_txtmachine_name.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtface_baking_id", SqlDbType.NVarChar).Value = this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtface_baking_name", SqlDbType.NVarChar).Value = this.PANEL0105_FACE_BAKING_txtface_baking_name.Text.ToString();  //
                        cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_id.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_name.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtnumber_mat_id", SqlDbType.NVarChar).Value = ""; // this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.ToString();  //5
                        cmd2.Parameters.Add("@txtnumber_mat_name", SqlDbType.NVarChar).Value = ""; // this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString();  //6
                        cmd2.Parameters.Add("@txtcomputer_name", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim(); // this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString();  //6
                        if (this.check_import.Checked == true)
                        {
                            cmd2.Parameters.Add("@txtstatus_import", SqlDbType.NVarChar).Value = "Y"; // this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString();  //6
                        }
                        else
                        {
                            cmd2.Parameters.Add("@txtstatus_import", SqlDbType.NVarChar).Value = "N"; // this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text.ToString();  //6
                        }
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
            else
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

                        cmd2.CommandText = "UPDATE c002_02produce_record_for_load_first SET " +
                                           "txtnumber_in_year = '" + this.txtnumber_in_year.Text + "'," +
                                           "txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text + "'," +
                                           "txtwherehouse_name = '" + this.PANEL1306_WH_txtwherehouse_name.Text + "'," +
                                           "txtmachine_id = '" + this.PANEL0102_MACHINE_txtmachine_id.Text + "'," +
                                           "txtmachine_name = '" + this.PANEL0102_MACHINE_txtmachine_name.Text + "'," +
                                           "txtface_baking_id = '" + this.PANEL0105_FACE_BAKING_txtface_baking_id.Text + "'," +
                                           "txtface_baking_name = '" + this.PANEL0105_FACE_BAKING_txtface_baking_name.Text + "'," +
                                           "txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text + "'," +
                                           "txtmat_name = '" + this.PANEL_MAT_txtmat_name.Text + "'," +
                                           "txtcomputer_name = '" + W_ID_Select.COMPUTER_NAME.Trim() + "'" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (txtcomputer_name = '" + W_ID_Select.COMPUTER_NAME.Trim() + "')";

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


        private void STOCK_FIND_INSERT()
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
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
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
                                    //Col_mat_status
                                    this.GridView1.Rows[i].Cells["Col_mat_status"].Value = "Y";
                                }
                                Cursor.Current = Cursors.Default;
                            }
                            else
                            {
                                this.GridView1.Rows[i].Cells["Col_mat_status"].Value = "N";
                                //=======================================================
                                Cursor.Current = Cursors.WaitCursor;
                                //conn.Open();
                                //if (conn.State == System.Data.ConnectionState.Open)
                                //{

                                    //SqlCommand cmd2 = conn.CreateCommand();
                                    //cmd2.CommandType = CommandType.Text;
                                    //cmd2.Connection = conn;

                                    SqlTransaction trans;
                                    trans = conn.BeginTransaction();
                                    cmd2.Transaction = trans;
                                //try
                                //{

                                cmd2.CommandText = "INSERT INTO k021_mat_average(cdkey,txtco_id," +  //1
                               "txtwherehouse_id," +  //2
                               "txtmat_no," +  //3
                               "txtmat_id," +  //4
                               "txtmat_name," +  //5
                               "txtmat_unit1_qty," +  //6
                               "chmat_unit_status," +  //7
                               "txtmat_unit2_qty," +  //8
                              "txtcost_qty1_balance," +  //9
                               "txtcost_qty_balance," +  //9
                               "txtcost_qty_price_average," +  //10
                               "txtcost_money_sum," +  //11
                               "txtcost_qty2_balance) " +  //14
                               "VALUES (@cdkey,@txtco_id," +  //1
                               "@txtwherehouse_id," +  //2
                               "@txtmat_no," +  //3
                               "@txtmat_id," +  //4
                               "@txtmat_name," +  //5
                               "@txtmat_unit1_qty," +  //6
                               "@chmat_unit_status," +  //7
                               "@txtmat_unit2_qty," +  //8
                               "@txtcost_qty1_balance," +  //9
                               "@txtcost_qty_balance," +  //9
                               "@txtcost_qty_price_average," +  //10
                               "@txtcost_money_sum," +  //11
                               "@txtcost_qty2_balance)";   //14

                                cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                                cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();  //1

                                cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();  //2
                                cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString();  //3
                                cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();  //4
                                cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString();  //5
                                cmd2.Parameters.Add("@txtmat_unit1_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString()));  //6
                                cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString();  //7
                                cmd2.Parameters.Add("@txtmat_unit2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));  //8

                                cmd2.Parameters.Add("@txtcost_qty1_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //9
                                cmd2.Parameters.Add("@txtcost_qty_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //9
                                cmd2.Parameters.Add("@txtcost_qty_price_average", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //10
                                cmd2.Parameters.Add("@txtcost_money_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //11

                                cmd2.Parameters.Add("@txtcost_qty2_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //13

                                //==============================

                                cmd2.ExecuteNonQuery();


                                Cursor.Current = Cursors.WaitCursor;
                                        trans.Commit();
                                        //conn.Close();

                                        Cursor.Current = Cursors.Default;


                                    //conn.Close();
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        //conn.Close();
                                    //        MessageBox.Show("kondate.soft", ex.Message);
                                    //        return;
                                    //    }
                                    //    finally
                                    //    {
                                    //        //conn.Close();
                                    //    }
                                //}
                                //=============================================================


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
                } //== if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
            } //== for (int i = 0; i < this.GridView1.Rows.Count; i++)

            //สต๊อคสินค้า ตามคลัง =============================================================================================





            // INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
                    if (this.GridView1.Rows[i].Cells["Col_mat_status"].Value.ToString() != "Y")
                    {

                    }
                }
            }
            // END INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี

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
            W_ID_Select.WORD_TOP = "ระเบียนFG1 ผ้าดิบ";
            kondate.soft.HOME03_Production.HOME03_Production_03Produce frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce();
            frm2.Show();

        }

        private void btnGo_txtic_id_Click(object sender, EventArgs e)
        {
            Fill_DATA_TO_GridView1();
        }

        private void btn_txtic_id_Click(object sender, EventArgs e)
        {
            MessageBox.Show("โปรดคลิ๊ก เลือก รายการเบิกเข้าเครื่องจักร ที่ตารางบนขวา เพื่อทำรายการ ");

            this.txtic_id.Text = W_ID_Select.TRANS_ID.Trim();
            Fill_DATA_TO_GridView1();
        }

        private void btnGo_M_Click(object sender, EventArgs e)
        {
            GridView1_Cal_Sum_M();
        }
        private void GridView1_Cal_Sum_M()
        {


            double Sum2_Qty_Yokpai = 0;
            double Sum_Qty = 0;
            double Sum2_Qty = 0;
            double Con_QTY = 0;

            double Sum_Qty_IC = 0;
            double Sum_Qty_IC_R = 0;


            int k = 0;
            for (int s = 0; s < this.GridView1_Machine.Rows.Count; s++)
            {
                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {


                    k = 1 + i;

                    var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                    if (valu != "")
                    {
                        if (this.GridView1_Machine.Rows[s].Cells["Col_txtmachine_id"].Value.ToString() == this.GridView1.Rows[i].Cells["Col_txtmachine_id"].Value.ToString())
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

                            if (double.Parse(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                            {

                                this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value).ToString("###,###.00");     //7
                                this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value).ToString("###,###.0000");     //7


                                this.GridView1.Rows[i].Cells["Col_txtqty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty"].Value).ToString("###,###.00");     //7
                                this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty2"].Value).ToString("###,###.00");     //8

                                this.GridView1.Rows[i].Cells["Col_txtprice"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtprice"].Value).ToString("###,###.00");     //6
                                this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value).ToString("###,###.00");     //7
                                this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value).ToString("###,###.00");     //8
                                this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value).ToString("###,###.00");     //8

                                this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value).ToString("###,###.00");     //6
                                this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value).ToString("###,###.00");     //7
                                this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value).ToString("###,###.00");     //8

                                this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value).ToString("###,###.00");     //8
                                this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value).ToString("###,###.00");     //8
                                this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value).ToString("###,###.00");     //8

                                this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value).ToString("###,###.00");     //8
                                this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8

                            }

                            if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                            {
                                //Sum_Qty  จำนวนเบิก (กก)=================================================
                                Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                                this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_yes"].Value = Sum_Qty.ToString("N", new CultureInfo("en-US"));


                                //============================================================================================================

                                //============================================================================================================
                                //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                                if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                                {
                                    Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                                    this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                                    //Sum2_Qty_Yokpai  =================================================
                                    Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                                    //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                                    Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                                    this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                                }


                                //หายอดสูญเสีย
                                //this.GridView1_Machine.Columns[4].HeaderText = "Col_txtsum_qty_ic";
                                //this.GridView1_Machine.Columns[5].HeaderText = "Col_txtsum_qty_yes";
                                //this.GridView1_Machine.Columns[6].HeaderText = "Col_txtsum_qty_change";
                                //this.GridView1_Machine.Columns[7].HeaderText = "Col_txtsum_qty_change_rate";
                                //this.txtsum_qty_yes.Text = this.txtsum_qty.Text;
                                   Sum_Qty_IC = Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_ic"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_yes"].Value.ToString()));
                                    this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_change"].Value = Sum_Qty_IC.ToString("N", new CultureInfo("en-US"));

                                    Sum_Qty_IC_R = Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_change"].Value.ToString())) * 100 / Convert.ToDouble(string.Format("{0:n4}", this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_ic"].Value.ToString()));
                                    this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_change_rate"].Value = Sum_Qty_IC_R.ToString("N", new CultureInfo("en-US"));

                                //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                                //========================================

                            }
                            //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                            //========================================

                        }
                        //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                        //========================================
                    }
                    //this.txtcount_rows.Text = k.ToString();
                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //========================================
                }
                //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                //========================================
                Sum_Qty = 0;
                Sum2_Qty_Yokpai = 0;
                Con_QTY = 0;

                Sum_Qty_IC = 0;
                Sum_Qty_IC_R = 0;

            }

        }
        private void GridView1_Add_Qty()
        {
            for (int s = 0; s < this.GridView1_Machine.Rows.Count; s++)
            {
                if (this.GridView1_Machine.Rows[s].Cells["Col_txtmachine_id"].Value.ToString() == this.PANEL0102_MACHINE_txtmachine_id.Text.Trim())
                {
                    this.GridView1_Machine.Rows[s].Cells["Col_txtic_id"].Value = this.txtic_id.Text.ToString();
                    this.GridView1_Machine.Rows[s].Cells["Col_txtsum_qty_ic"].Value = this.txtsum_qty_ic.Text.ToString();
                }
            }

        }

        private void ch_yokma_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ch_yokma.Checked == true)
            {
                this.btnGo1.Visible = true;
                this.btnGo1_RIB.Visible = true;
            }
            else
            {
                this.btnGo1.Visible = false;
                this.btnGo1_RIB.Visible = false;
            }
        }

        private void btnrun_num_Click(object sender, EventArgs e)
        {
            GridView1_Run_Lot_Num();
         }

        private void check_import_CheckedChanged(object sender, EventArgs e)
        {
            if (check_import.Checked == true)
            {
                //this.GridView66.Visible = false;
                //this.GridView_Import.Visible = true;
            }
            else
            {
                //this.GridView66.Visible = true;
                //this.GridView_Import.Visible = false;
            }
        }










        //=============================================================

        //===============================================


        //====================================================================

    }
}
