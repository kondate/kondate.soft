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

namespace kondate.soft.HOME05_Sales
{
    public partial class HOME05_Sale_03cus : Form
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



        public HOME05_Sale_03cus()
        {
            InitializeComponent();
        }

        private void HOME05_Sale_03cus_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "S104";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            //this.Paneldate_dtpcus_birth_day.Value = DateTime.Now;
            //this.Paneldate_dtpcus_birth_day.Format = DateTimePickerFormat.Custom;
            //this.Paneldate_dtpcus_birth_day.CustomFormat = this.Paneldate_dtpcus_birth_day.Value.ToString("dd-MM-yyyy", UsaCulture);

            //this.Paneldate_txtdate.Text = this.Paneldate_dtpcus_birth_day.Value.ToString("dd-MM-yyyy", UsaCulture);

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มลูกค้าใหม่";
            this.txtcus_id.ReadOnly = false;
            this.ActiveControl = this.txtcus_id;

            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;

            Fill_cbo_profix();

            PANEL_02CUS_TYPE_GridView1_cus_type();
            PANEL_02CUS_TYPE_Fill_cus_type();


            PANEL36_ACC_CONTROL_GridView1_acc_control();
            PANEL36_ACC_CONTROL_Fill_acc_control();

            Run_ID();
            CHECK_UP_NO999();

            PANEL_FORM1_GridView1();
            Fill_PANEL_FORM1_dataGridView1();

        }

        private void Run_ID()
        {
            if (this.txtcus_no.Text == "")
            {
                this.txtcus_no.Text = "001";
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
            //เชื่อมต่อฐานข้อมูล======================================================
            Cursor.Current = Cursors.WaitCursor;
            string RID = "";
            double Rid2 = 0;
            double Rid3 = 0;

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM s001_03cus" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    "AND (txtcus_id <> '')" +
                                    " ORDER BY txtcus_no DESC";

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

                        RID = dt2.Rows[0]["txtcus_no"].ToString();      //1
                        Rid2 = Convert.ToDouble(RID);


                        Rid3 = Convert.ToDouble(string.Format("{0:n}", Rid2)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        this.txtcus_no.Text = Rid3.ToString("00#");
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

                //===========================================
            }
            //================================




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
            var frm2 = new HOME05_Sale_03cus();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มลูกค้าใหม่";
            this.txtcus_id.ReadOnly = false;
            this.btnUp_pic1.Visible = false;
            this.btnUp_pic2.Visible = false;
            this.btnUp_pic3.Visible = false;
            this.btnUp_pic4.Visible = false;
            this.btnUp_pic5.Visible = false;
            this.btnUp_pic6.Visible = false;


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

            if (this.txtcus_id.Text != "")
            {
                this.iblword_status.Text = "แก้ไขลูกค้า";
                this.txtcus_id.ReadOnly = true;
                this.btnUp_pic1.Visible = true;
                this.btnUp_pic2.Visible = true;
                this.btnUp_pic3.Visible = true;
                this.btnUp_pic4.Visible = true;
                this.btnUp_pic5.Visible = true;
                this.btnUp_pic6.Visible = true;

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.txtcus_id.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัสลูกค้าก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtcus_id.Focus();
                return;
            }
            if (this.txtcus_no.Text == "")
            {
                MessageBox.Show("โปรดใส่ ลำดับลูกค้าก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtcus_no.Focus();
                return;
            }
            if (this.txtcus_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อลูกค้าก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtcus_name.Focus();
                return;
            }
            if (this.txtcus_registered_capital.Text == "")
            {
                this.txtcus_registered_capital.Text = "0";
            }

            if (this.chcus_office.Checked == true)
            {
                this.txtcus_branch_name.Text = "สำนักงานใหญ่";
            }
            if (this.chcus_branch.Checked == true)
            {
                this.txtcus_branch_name.Text = this.txtcus_branch_id.Text.Trim();
            }
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


            if (this.iblword_status.Text.Trim() == "เพิ่มลูกค้าใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM s001_03cus" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("รหัสลูกค้านี้ซ้ำ  : '" + this.txtcus_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtcus_id.Focus();
                        conn.Close();
                        return;
                    }
                }

                //
                conn.Close();
            }
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มลูกค้าใหม่")
                    {
                        //1
                        cmd2.CommandText = "INSERT INTO s001_03cus(cdkey,txtco_id," +
                                           "txtcus_no,txtcus_id," +
                                           "txtcus_name," +
                                           "txtcus_name_eng," +
                                           "txtcus_status) " +
                                           "VALUES (@cdkey,@txtco_id," +
                                           "@txtcus_no,@txtcus_id," +
                                           "@txtcus_name," +
                                           "@txtcus_name_eng," +
                                           "@txtcus_status)";

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_no", SqlDbType.NVarChar).Value = this.txtcus_no.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_id", SqlDbType.NVarChar).Value = this.txtcus_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_name", SqlDbType.NVarChar).Value = this.txtcus_name.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_name_eng", SqlDbType.NVarChar).Value = this.txtcus_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //2
                        cmd2.CommandText = "INSERT INTO s001_03cus_1address(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtprefix_id," +  //3
                                           "txtcontact_person," +  //4
                                           "txtcontact_person_tel," +  //5
                                           "chcus_branch," +  //6
                                           "txtcus_branch_id," +  //7
                                           "txtcus_branch_name," +  //7
                                           "txtcus_tel," +  //8
                                           "txtcus_fax," +  //9
                                           "txtcus_email," +  //10
                                           "txtcus_homepage," +  //11
                                           "txthome_id," +  //12
                                           "txttambon," +  //13
                                           "txtamphur," +  //14
                                           "txtchangwat," +  //15
                                           "txtpost_id," +  //16
                                           "txthome_id_full," +  //17
                                           "txthome_id_full_eng," +  //18
                                           "txtremark) " +  //19
                                           "VALUES (@cdkey2,@txtco_id2," +
                                          "@txtcus_id2," +  //2
                                           "@txtprefix_id," +  //3
                                           "@txtcontact_person," +  //4
                                           "@txtcontact_person_tel," +  //5
                                           "@chcus_branch," +  //6
                                           "@txtcus_branch_id," +  //7
                                           "@txtcus_branch_name," +  //7
                                           "@txtcus_tel," +  //8
                                           "@txtcus_fax," +  //9
                                           "@txtcus_email," +  //10
                                           "@txtcus_homepage," +  //11
                                           "@txthome_id," +  //12
                                           "@txttambon," +  //13
                                           "@txtamphur," +  //14
                                           "@txtchangwat," +  //15
                                           "@txtpost_id," +  //16
                                           "@txthome_id_full," +  //17
                                           "@txthome_id_full_eng," +  //18
                                           "@txtremark)";  //19

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id2", SqlDbType.NVarChar).Value = this.txtcus_id.Text.ToString();
                        cmd2.Parameters.Add("@txtprefix_id", SqlDbType.NVarChar).Value = this.txtprefix_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcontact_person", SqlDbType.NVarChar).Value = this.txtcontact_person.Text.ToString();
                        cmd2.Parameters.Add("@txtcontact_person_tel", SqlDbType.NVarChar).Value = this.txtcontact_person_tel.Text.ToString();

                        string HH = "";
                        if (this.chcus_office.Checked == true)
                        {
                            HH = "HO";
                        }
                        if (this.chcus_branch.Checked == true)
                        {
                            HH = "BR";
                        }
                        cmd2.Parameters.Add("@chcus_branch", SqlDbType.NVarChar).Value = HH.ToString();
                        cmd2.Parameters.Add("@txtcus_branch_id", SqlDbType.NVarChar).Value = this.txtcus_branch_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_branch_name", SqlDbType.NVarChar).Value = this.txtcus_branch_name.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_tel", SqlDbType.NVarChar).Value = this.txtcus_tel.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_fax", SqlDbType.NVarChar).Value = this.txtcus_fax.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_email", SqlDbType.NVarChar).Value = this.txtcus_email.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_homepage", SqlDbType.NVarChar).Value = this.txtcus_homepage.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id", SqlDbType.NVarChar).Value = this.txthome_id.Text.ToString();
                        cmd2.Parameters.Add("@txttambon", SqlDbType.NVarChar).Value = this.txttambon.Text.ToString();
                        cmd2.Parameters.Add("@txtamphur", SqlDbType.NVarChar).Value = this.txtamphur.Text.ToString();
                        cmd2.Parameters.Add("@txtchangwat", SqlDbType.NVarChar).Value = this.txtchangwat.Text.ToString();
                        cmd2.Parameters.Add("@txtpost_id", SqlDbType.NVarChar).Value = this.txtpost_id.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id_full", SqlDbType.NVarChar).Value = this.txthome_id_full.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id_full_eng", SqlDbType.NVarChar).Value = this.txthome_id_full_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtremark", SqlDbType.NVarChar).Value = this.txtremark.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //3
                        cmd2.CommandText = "INSERT INTO s001_03cus_2account(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtacc_id," +  //3
                                           "txtcredit_day," +  //4
                                           "txtbranch_id," +  //5
                                           "txtcus_type_id," + //6
                                           "txtcus_group_id," +  //7
                                           "txtacc_group_tax_id," +  //8
                                           "txtcode_bank_id," +  //9
                                           "txtcode_bank_branch_id," +  //10
                                           "txtnumber_acc_bank," +  //11
                                           "txtcharge_to_id) " +   //12
                                           "VALUES (@cdkey3,@txtco_id3," +
                                           "@txtcus_id3," +  //2
                                           "@txtacc_id," +  //3
                                           "@txtcredit_day," +  //4
                                           "@txtbranch_id," +  //5
                                           "@txtcus_type_id," + //6
                                           "@txtcus_group_id," +  //7
                                           "@txtacc_group_tax_id," +  //8
                                           "@txtcode_bank_id," +  //9
                                           "@txtcode_bank_branch_id," +  //10
                                           "@txtnumber_acc_bank," +  //11
                                           "@txtcharge_to_id)";  //12

                        cmd2.Parameters.Add("@cdkey3", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id3", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id3", SqlDbType.NVarChar).Value = this.txtcus_id.Text.ToString();

                        cmd2.Parameters.Add("@txtacc_id", SqlDbType.NVarChar).Value = this.PANEL36_ACC_CONTROL_txtacc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcredit_day", SqlDbType.NVarChar).Value = this.txtcredit_day.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcus_type_id", SqlDbType.NVarChar).Value = this.PANEL_02CUS_TYPE_txtcus_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_group_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcode_bank_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcode_bank_branch_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtnumber_acc_bank", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcharge_to_id", SqlDbType.NVarChar).Value = "";

                        //==============================

                        cmd2.ExecuteNonQuery();

                        //4
                        cmd2.CommandText = "INSERT INTO s001_03cus_3detail(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtcus_birth_day," +  //3
                                           "txtcus_card_id," +  //4
                                           "txtcus_registered_id," +  //5
                                           "txtcus_registered_capital," +  //6
                                           "txtcus_tax_id," +  //7
                                           "txtcus_kind_id) " +  //8
                                           "VALUES (@cdkey4,@txtco_id4," +
                                           "@txtcus_id4," +  //2
                                           "@txtcus_birth_day," +  //3
                                           "@txtcus_card_id," +  //4
                                           "@txtcus_registered_id," +  //5
                                           "@txtcus_registered_capital," +  //6
                                           "@txtcus_tax_id," +  //7
                                           "@txtcus_kind_id)";  //8

                        cmd2.Parameters.Add("@cdkey4", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id4", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id4", SqlDbType.NVarChar).Value = this.txtcus_id.Text.ToString();


                        cmd2.Parameters.Add("@txtcus_birth_day", SqlDbType.NVarChar).Value = this.Paneldate_txtdate.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_card_id", SqlDbType.NVarChar).Value = this.txtcus_card_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_registered_id", SqlDbType.NVarChar).Value = this.txtcus_registered_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_registered_capital", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcus_registered_capital.Text.ToString()));
                        cmd2.Parameters.Add("@txtcus_tax_id", SqlDbType.NVarChar).Value = this.txtcus_tax_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_kind_id", SqlDbType.NVarChar).Value = "";
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //5
                        cmd2.CommandText = "INSERT INTO s001_03cus_4picture(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtcus_1picture_size,txtcus_1picture," +  //3
                                           "txtcus_2picture_size,txtcus_2picture," +  //4
                                           "txtcus_3picture_size,txtcus_3picture," +  //5
                                           "txtcus_4picture_size,txtcus_4picture," +  //6
                                           "txtcus_5picture_size,txtcus_5picture," +  //7
                                           "txtcus_6picture_size,txtcus_6picture) " +  //8
                                           "VALUES (@cdkey5,@txtco_id5," +
                                           "@txtcus_id5," +  //2
                                           "@txtcus_1picture_size,@txtcus_1picture," +  //3
                                           "@txtcus_2picture_size,@txtcus_2picture," +  //4
                                           "@txtcus_3picture_size,@txtcus_3picture," +  //5
                                           "@txtcus_4picture_size,@txtcus_4picture," +  //6
                                           "@txtcus_5picture_size,@txtcus_5picture," +  //7
                                           "@txtcus_6picture_size,@txtcus_6picture)";  //8

                        cmd2.Parameters.Add("@cdkey5", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id5", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id5", SqlDbType.NVarChar).Value = this.txtcus_id.Text.ToString();

                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture1.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture1.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size1.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture2.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture2.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size2.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture3.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture3.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size3.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture4.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture4.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size4.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture5.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture5.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture_size", this.txtpicture_size5.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture5.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size5.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture5.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture_size", this.txtpicture_size5.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture6.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture6.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture_size", this.txtpicture_size6.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture6.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size6.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture6.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture_size", this.txtpicture_size6.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture", imageBt));
                        }

                        //==============================


                        cmd2.ExecuteNonQuery();





                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        string STA2 = "";
                        if (this.txtcus_status.Checked == true)
                        {
                            STA2 = "0";
                        }
                        if (this.txtcus_status.Checked == false)
                        {
                            STA2 = "1";
                        }

                        //1
                        cmd2.CommandText = "UPDATE s001_03cus SET " +
                                                                     "txtcus_no = '" + this.txtcus_no.Text.Trim() + "'," +
                                                                     "txtcus_name = '" + this.txtcus_name.Text.Trim() + "'," +
                                                                     "txtcus_name_eng = '" + this.txtcus_name_eng.Text.Trim() + "'," +
                                                                      "txtcus_status = '" + STA2.ToString() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        //2
                        string HH2 = "";
                        if (this.chcus_office.Checked == true)
                        {
                            HH2 = "HO";
                        }
                        if (this.chcus_branch.Checked == true)
                        {
                            HH2 = "BR";
                        }
                        cmd2.CommandText = "UPDATE s001_03cus_1address SET " +
                                                                     "txtprefix_id = '" + this.txtprefix_id.Text.Trim() + "'," +
                                                                     "txtcontact_person = '" + this.txtcontact_person.Text.Trim() + "'," +
                                                                     "txtcontact_person_tel = '" + this.txtcontact_person_tel.Text.Trim() + "'," +
                                                                     "chcus_branch = '" + HH2.Trim() + "'," +
                                                                     "txtcus_branch_id = '" + this.txtcus_branch_id.Text.Trim() + "'," +
                                                                      "txtcus_branch_name = '" + this.txtcus_branch_name.Text.Trim() + "'," +
                                                                    "txtcus_tel = '" + this.txtcus_tel.Text.Trim() + "'," +
                                                                     "txtcus_fax = '" + this.txtcus_fax.Text.Trim() + "'," +
                                                                     "txtcus_email = '" + this.txtcus_email.Text.Trim() + "'," +
                                                                     "txtcus_homepage = '" + this.txtcus_homepage.Text.Trim() + "'," +
                                                                     "txthome_id = '" + this.txthome_id.Text.Trim() + "'," +
                                                                     "txttambon = '" + this.txttambon.Text.Trim() + "'," +
                                                                     "txtamphur = '" + this.txtamphur.Text.Trim() + "'," +
                                                                     "txtchangwat = '" + this.txtchangwat.Text.Trim() + "'," +
                                                                     "txtpost_id = '" + this.txtpost_id.Text.Trim() + "'," +
                                                                     "txthome_id_full = '" + this.txthome_id_full.Text.Trim() + "'," +
                                                                     "txthome_id_full_eng = '" + this.txthome_id_full_eng.Text.Trim() + "'," +
                                                                      "txtremark = '" + this.txtremark.Text.Trim() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        //3
                        cmd2.CommandText = "UPDATE s001_03cus_2account SET " +
                                                                     "txtacc_id = '" + this.PANEL36_ACC_CONTROL_txtacc_id.Text.Trim() + "'," +
                                                                     "txtcredit_day = '" + this.txtcredit_day.Text.Trim() + "'," +
                                                                     "txtbranch_id = ''," +
                                                                     "txtcus_type_id = '" + this.PANEL_02CUS_TYPE_txtcus_type_id.Text.Trim() + "'," +
                                                                     "txtcus_group_id = ''," +
                                                                     "txtacc_group_tax_id = ''," +
                                                                     "txtcode_bank_id = ''," +
                                                                     "txtcode_bank_branch_id = ''," +
                                                                       "txtnumber_acc_bank = ''," +
                                                                    "txtcharge_to_id = ''" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        //4
                        cmd2.CommandText = "UPDATE s001_03cus_3detail SET " +
                                                                     "txtcus_birth_day = '" + this.Paneldate_txtdate.Text.Trim() + "'," +
                                                                     "txtcus_card_id = '" + this.txtcus_card_id.Text.Trim() + "'," +
                                                                     "txtcus_registered_id = '" + this.txtcus_registered_id.Text.Trim() + "'," +
                                                                     "txtcus_registered_capital = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcus_registered_capital.Text.ToString())) + "'," +
                                                                     "txtcus_tax_id = '" + this.txtcus_tax_id.Text.Trim() + "'," +
                                                                     "txtcus_kind_id = ''" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                    " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();


                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มลูกค้าใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtcus_id.Text = "";
                        Clear_Text();


                        Fill_PANEL_FORM1_dataGridView1();
                        this.iblword_status.Text = "เพิ่มลูกค้าใหม่";
                        this.txtcus_id.ReadOnly = false;

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
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
            if (W_ID_Select.M_FORM_CANCEL.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            W_ID_Select.LOG_ID = "7";
            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
            TRANS_LOG();

            this.iblword_status.Text = "ยกเลิกเอกสาร";
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
                    if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                    {
                        String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                        DateTime myDateTime = new DateTime();
                        myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

                        String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                        DateTime myDateTime2 = new DateTime();
                        myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);

                        string Cancel_ID = W_ID_Select.CDKEY.Trim() + "-" + W_ID_Select.M_USERNAME.Trim() + "-" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "-" + myDateTime2.ToString("HH:mm:ss", UsaCulture);




                        cmd2.CommandText = "INSERT INTO s001_03cus_cancel(cdkey,txtco_id,txtbranch_id," +  //1
                                                                                                                 //"txttrans_date," +
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                               "txtform_name,txtform_caption," +  //5
                                                "txtuser_name,txtemp_office_name," +  //6
                                               "txtlog_id,txtlog_name," +  //7
                                              "txtdocument_id,txtversion_id,txtcount,cancel_id) " +  //8
                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                                                                            //"@txttrans_date," +
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtform_name,@txtform_caption," +  //5
                                               "@txtuser_name,@txtemp_office_name," +  //6
                                               "@txtlog_id,@txtlog_name," +  //7
                                               "@txtdocument_id,@txtversion_id,@txtcount,@cancel_id)";   //8

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();


                        cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);


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
                        cmd2.Parameters.Add("@cancel_id", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "INSERT INTO s001_03cus_cancel_detail(cdkey,txtco_id," +  //1
                                             "txtcus_no,txtcus_id," +  //2
                                             "txtcus_name,txtcus_name_eng," +  //3
                                            "txtcus_status,cancel_id) " +  //5
                                             "VALUES (@cdkey2,@txtco_id2," +  //1
                                             "@txtcus_no,@txtcus_id," +  //2
                                             "@txtcus_name,@txtcus_name_eng," +  //3
                                            "@txtcus_status,@cancel_id2)";   //5

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_no", SqlDbType.NVarChar).Value = this.txtcus_no.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_id", SqlDbType.NVarChar).Value = this.txtcus_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_name", SqlDbType.NVarChar).Value = this.txtcus_name.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_name_eng", SqlDbType.NVarChar).Value = this.txtcus_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_status", SqlDbType.NChar).Value = "0";
                        cmd2.Parameters.Add("@cancel_id2", SqlDbType.NVarChar).Value = Cancel_ID.ToString();
                        //==============================
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM s001_03cus" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM s001_03cus_1address" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM s001_03cus_2account" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM s001_03cus_3detail" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM s001_03cus_4picture" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtcus_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                        {
                            W_ID_Select.LOG_ID = "7";
                            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
                            TRANS_LOG();
                        }

                        MessageBox.Show("ยกเลิกเอกสาร เรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtcus_id.Text = "";
                        this.txtcus_name.Text = "";


                        Fill_PANEL_FORM1_dataGridView1();
                        this.iblword_status.Text = "เพิ่มลูกค้าใหม่";
                        this.txtcus_id.ReadOnly = false;

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้ ยกเลิกเอกสาร", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ไม่ได้ ยกเลิกเอกสาร", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcus_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                   && !char.IsDigit(e.KeyChar)
                   && e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
                   && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
                   && e.KeyChar != '/'
                    && e.KeyChar != '_'
                   && e.KeyChar != 'a' && e.KeyChar != 'b' && e.KeyChar != 'c' && e.KeyChar != 'd' && e.KeyChar != 'e' && e.KeyChar != 'f' && e.KeyChar != 'g' && e.KeyChar != 'h' && e.KeyChar != 'i' && e.KeyChar != 'j'
                   && e.KeyChar != 'k' && e.KeyChar != 'l' && e.KeyChar != 'm' && e.KeyChar != 'n' && e.KeyChar != 'o' && e.KeyChar != 'p' && e.KeyChar != 'q' && e.KeyChar != 'r' && e.KeyChar != 's'
                   && e.KeyChar != 't' && e.KeyChar != 'u' && e.KeyChar != 'v' && e.KeyChar != 'w' && e.KeyChar != 'x' && e.KeyChar != 'y' && e.KeyChar != 'z'
                   && e.KeyChar != 'A' && e.KeyChar != 'B' && e.KeyChar != 'C' && e.KeyChar != 'D' && e.KeyChar != 'E' && e.KeyChar != 'F' && e.KeyChar != 'G' && e.KeyChar != 'H' && e.KeyChar != 'I' && e.KeyChar != 'J'
                   && e.KeyChar != 'K' && e.KeyChar != 'L' && e.KeyChar != 'M' && e.KeyChar != 'N' && e.KeyChar != 'O' && e.KeyChar != 'P' && e.KeyChar != 'Q' && e.KeyChar != 'R' && e.KeyChar != 'S'
                   && e.KeyChar != 'T' && e.KeyChar != 'U' && e.KeyChar != 'V' && e.KeyChar != 'W' && e.KeyChar != 'X' && e.KeyChar != 'Y' && e.KeyChar != 'Z'

             )
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter && this.txtcus_id.Text == "")
            {
                this.txtcus_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtcus_id.Text.Trim() != "")
            {
                this.txtcus_no.Focus();

            }

        }

        private void txtcus_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtcus_no.Text == "")
            {
                this.txtcus_no.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtcus_no.Text.Trim() != "")
            {
                if (this.txtcus_no.TextLength == 3)
                {
                    this.txtcus_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่ลำดับให้ครับ  3 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtcus_no.Focus();
                    return;
                }
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtcus_no.Text.Length == 0)
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

        private void txtcus_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcus_name_eng.Focus();

        }

        private void txtcus_name_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tabPage1.Focus();

        }

        private void Fill_cbo_profix()
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
            //END เชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                  " FROM k016db_supplier_1profix" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtprefix_id <> '')" +
                                  " ORDER BY txtprefix_id";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtprefix_name = dr.GetString(5);
                        this.cboprefix_name.Items.Add(txtprefix_name);
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

        }
        private void fill_cbo_profix2()
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
            //END เชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                  " FROM k016db_supplier_1profix" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtprefix_id <> '')" +
                                  " AND (txtprefix_name = '" + this.cboprefix_name.Text.Trim() + "')";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtprefix_id.Text = dt.Rows[0]["txtprefix_id"].ToString();
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
        }
        private void fill_cbo_profix_Edit()
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
            //END เชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                  " FROM k016db_supplier_1profix" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtprefix_id = '" + this.txtprefix_id.Text.Trim() + "')";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.cboprefix_name.Text = dt.Rows[0]["txtprefix_name"].ToString();
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
        }
        private void cboprefix_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill_cbo_profix2();
        }

        private void cboprefix_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcontact_person.Focus();

        }

        private void txtcontact_person_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcontact_person_tel.Focus();

        }

        private void txtcontact_person_tel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcus_tel.Focus();


        }

        private void txtcus_tel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcus_fax.Focus();

        }

        private void txtcus_fax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcus_email.Focus();

        }

        private void txtcus_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtcus_homepage.Focus();

        }

        private void txtcus_homepage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txthome_id.Focus();

        }

        private void txthome_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txttambon.Focus();

        }

        private void txttambon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtamphur.Focus();

        }

        private void txtamphur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtchangwat.Focus();

        }

        private void txtchangwat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtpost_id.Focus();

        }

        private void txtpost_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txthome_id_full.Focus();

        }

        private void txthome_id_full_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txthome_id_full_eng.Focus();

        }

        private void txthome_id_full_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtremark.Focus();

        }

        private void txtremark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tabPage2.Focus();

        }


        //cus_type =======================================================================
        private void PANEL_02CUS_TYPE_Fill_cus_type()
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

            PANEL_02CUS_TYPE_Clear_GridView1_cus_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM s001_02cus_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtcus_type_id <> '')" +
                                    " ORDER BY txtcus_type_no ASC";

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
                            var index = PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows.Add();
                            PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[index].Cells["Col_txtcus_type_id"].Value = dt2.Rows[j]["txtcus_type_id"].ToString();      //1
                            PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[index].Cells["Col_txtcus_type_name"].Value = dt2.Rows[j]["txtcus_type_name"].ToString();      //2
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
        private void PANEL_02CUS_TYPE_GridView1_cus_type()
        {
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.ColumnCount = 3;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].Name = "Col_txtcus_type_id";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].Name = "Col_txtcus_type_name";

            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[0].HeaderText = "No";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].HeaderText = "รหัส";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].HeaderText = " ประเภทลูกค้า";

            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[0].Visible = false;  //"No";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].Visible = true;  //"Col_txtcus_type_id";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].Width = 100;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].ReadOnly = true;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].Visible = true;  //"Col_txtcus_type_name";
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].Width = 150;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].ReadOnly = true;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL_02CUS_TYPE_Clear_GridView1_cus_type()
        {
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows.Clear();
            this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Refresh();
        }
        private void PANEL_02CUS_TYPE_txtcus_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL_02CUS_TYPE.Visible == false)
                {
                    this.PANEL_02CUS_TYPE.Visible = true;
                    this.PANEL_02CUS_TYPE.Location = new Point(116, this.PANEL_02CUS_TYPE_txtcus_type_name.Location.Y + 22);
                    this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Focus();
                }
                else
                {
                    this.PANEL_02CUS_TYPE.Visible = false;
                }
        }
        private void PANEL_02CUS_TYPE_btncus_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02CUS_TYPE.Visible == false)
            {

                int xLocation = PANEL_02CUS_TYPE_txtcus_type_name.Location.X;
                int yLocation = PANEL_02CUS_TYPE_txtcus_type_name.Location.Y;
                int xx = xLocation + tabControl1.Location.X;
                int yy = yLocation + tabControl1.Location.Y;

                this.PANEL_02CUS_TYPE.Visible = true;
                this.PANEL_02CUS_TYPE.BringToFront();
                this.PANEL_02CUS_TYPE.Location = new Point(xx + 12, yy + 28);
            }
            else
            {
                this.PANEL_02CUS_TYPE.Visible = false;
            }

            //if (this.PANEL_02CUS_TYPE.Visible == false)
            //{
            //    this.PANEL_02CUS_TYPE.Visible = true;
            //    this.PANEL_02CUS_TYPE.BringToFront();
            //    this.PANEL_02CUS_TYPE.Location = new Point(116, this.PANEL_02CUS_TYPE_txtcus_type_name.Location.Y + 22);
            //}
            //else
            //{
            //    this.PANEL_02CUS_TYPE.Visible = false;
            //}
        }
        private void PANEL_02CUS_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02CUS_TYPE.Visible == false)
            {
                this.PANEL_02CUS_TYPE.Visible = true;
            }
            else
            {
                this.PANEL_02CUS_TYPE.Visible = false;
            }
        }
        private void PANEL_02CUS_TYPE_dataGridView1_cus_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL_02CUS_TYPE_txtcus_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL_02CUS_TYPE_txtcus_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL_02CUS_TYPE_dataGridView1_cus_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL_02CUS_TYPE_dataGridView1_cus_type.CurrentRow.Index;

                this.PANEL_02CUS_TYPE_txtcus_type_id.Text = PANEL_02CUS_TYPE_dataGridView1_cus_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL_02CUS_TYPE_txtcus_type_name.Text = PANEL_02CUS_TYPE_dataGridView1_cus_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL_02CUS_TYPE_txtcus_type_name.Focus();
                this.PANEL_02CUS_TYPE.Visible = false;
            }
        }
        private void PANEL_02CUS_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL_02CUS_TYPE_Clear_GridView1_cus_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM s001_02cus_type" +
                                   " WHERE (txtcus_type_name LIKE '%" + this.PANEL_02CUS_TYPE_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    //" AND (txtcus_type_id <> '')" +
                                    " ORDER BY txtcus_type_no ASC";


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
                            var index = PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows.Add();
                            PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[index].Cells["Col_txtcus_type_id"].Value = dt2.Rows[j]["txtcus_type_id"].ToString();      //1
                            PANEL_02CUS_TYPE_dataGridView1_cus_type.Rows[index].Cells["Col_txtcus_type_name"].Value = dt2.Rows[j]["txtcus_type_name"].ToString();      //2
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
        private void PANEL_02CUS_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL_02CUS_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_02CUS_TYPE.Height = PANEL_02CUS_TYPE_btnresize_low.Top + e.Y;
                this.PANEL_02CUS_TYPE.Width = PANEL_02CUS_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_02CUS_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL_02CUS_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END cus_type=======================================================================


        //Acc_control =======================================================================
        private void PANEL36_ACC_CONTROL_Fill_acc_control()
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

            PANEL36_ACC_CONTROL_Clear_GridView1_acc_control();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_1acc" +
                                  " WHERE (left(txtacc_id,1) = '1')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtacc_id <> '')" +
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
                            var index = PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows.Add();
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_id"].Value = dt2.Rows[j]["txtacc_id"].ToString();      //1
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_name"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //2
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_name_eng"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //3
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //3
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
        private void PANEL36_ACC_CONTROL_GridView1_acc_control()
        {
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.ColumnCount = 5;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[0].Name = "Col_Auto_num";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[1].Name = "Col_txtacc_id";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[2].Name = "Col_txtacc_name";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[3].Name = "Col_txtacc_name_eng";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[4].Name = "Col_txtacc_degree_id";

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[0].HeaderText = "No";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[1].HeaderText = "รหัส";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[2].HeaderText = " ชื่อบัญชี";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[3].HeaderText = " ชื่อบัญชี Eng";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[4].HeaderText = " ระดับบัญชี";

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[0].Visible = false;  //"No";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[1].Visible = true;  //"Col_txtacc_id";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[1].Width = 100;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[1].ReadOnly = true;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[2].Visible = true;  //"Col_txtacc_name";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[2].Width = 150;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[2].ReadOnly = true;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[3].Visible = true;  //"Col_txtacc_name_eng";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[3].Width = 150;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[3].ReadOnly = true;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[4].Visible = true;  //"Col_txtacc_id";
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[4].Width = 100;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[4].ReadOnly = true;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.EnableHeadersVisualStyles = false;

        }
        private void PANEL36_ACC_CONTROL_Clear_GridView1_acc_control()
        {
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows.Clear();
            this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Refresh();
        }
        private void PANEL36_ACC_CONTROL_txtacc_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL36_ACC_CONTROL.Visible == false)
                {
                    this.PANEL36_ACC_CONTROL.Visible = true;
                    this.PANEL36_ACC_CONTROL.BringToFront();
                    this.PANEL36_ACC_CONTROL.Location = new Point(this.tabControl1.Location.X + 133, this.tabControl1.Location.Y + 36);
                }
                else
                {
                    this.PANEL36_ACC_CONTROL.Visible = false;
                }
        }
        private void PANEL36_ACC_CONTROL_btnacc_control_Click(object sender, EventArgs e)
        {
            if (this.PANEL36_ACC_CONTROL.Visible == false)
            {

                int xLocation = PANEL36_ACC_CONTROL_txtacc_name.Location.X;
                int yLocation = PANEL36_ACC_CONTROL_txtacc_name.Location.Y;
                int xx = xLocation + tabControl1.Location.X;
                int yy = yLocation + tabControl1.Location.Y;

                this.PANEL36_ACC_CONTROL.Visible = true;
                this.PANEL36_ACC_CONTROL.BringToFront();
                this.PANEL36_ACC_CONTROL.Location = new Point(xx + 12, yy + 28);
                this.PANEL36_ACC_CONTROL.Width = 1024;
                this.PANEL36_ACC_CONTROL.Height = 700;
            }
            else
            {
                this.PANEL36_ACC_CONTROL.Visible = false;
            }



            //if (this.PANEL36_ACC_CONTROL.Visible == false)
            //{
            //    this.PANEL36_ACC_CONTROL.Visible = true;
            //    this.PANEL36_ACC_CONTROL.BringToFront();
            //    this.PANEL36_ACC_CONTROL.Location = new Point(this.tabControl1.Location.X + 133, this.tabControl1.Location.Y + 36);
            //}
            //else
            //{
            //    this.PANEL36_ACC_CONTROL.Visible = false;
            //}
        }
        private void PANEL36_ACC_CONTROL_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL36_ACC_CONTROL.Visible == false)
            {
                this.PANEL36_ACC_CONTROL.Visible = true;
            }
            else
            {
                this.PANEL36_ACC_CONTROL.Visible = false;
            }
        }
        private void PANEL36_ACC_CONTROL_dataGridView1_acc_control_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL36_ACC_CONTROL_txtacc_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL36_ACC_CONTROL_txtacc_name.Text = row.Cells[2].Value.ToString();
                    this.PANEL36_ACC_CONTROL_txtacc_name_eng.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL36_ACC_CONTROL_dataGridView1_acc_control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL36_ACC_CONTROL_dataGridView1_acc_control.CurrentRow.Index;

                this.PANEL36_ACC_CONTROL_txtacc_id.Text = PANEL36_ACC_CONTROL_dataGridView1_acc_control.CurrentRow.Cells[1].Value.ToString();
                this.PANEL36_ACC_CONTROL_txtacc_name.Text = PANEL36_ACC_CONTROL_dataGridView1_acc_control.CurrentRow.Cells[2].Value.ToString();
                this.PANEL36_ACC_CONTROL_txtacc_name.Focus();
                this.PANEL36_ACC_CONTROL.Visible = false;
            }
        }
        private void PANEL36_ACC_CONTROL_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL36_ACC_CONTROL_btn_search_Click(object sender, EventArgs e)
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

            PANEL36_ACC_CONTROL_Clear_GridView1_acc_control();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_1acc" +
                                   " WHERE (txtacc_name LIKE '%" + this.PANEL36_ACC_CONTROL_txtsearch.Text + "%')" +
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
                            var index = PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows.Add();
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_id"].Value = dt2.Rows[j]["txtacc_id"].ToString();      //1
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_name"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //2
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_name_eng"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //3
                            PANEL36_ACC_CONTROL_dataGridView1_acc_control.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //3
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
        private void PANEL36_ACC_CONTROL_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL36_ACC_CONTROL_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL36_ACC_CONTROL.Height = PANEL36_ACC_CONTROL_btnresize_low.Top + e.Y;
                this.PANEL36_ACC_CONTROL.Width = PANEL36_ACC_CONTROL_btnresize_low.Left + e.X;
            }
        }
        private void PANEL36_ACC_CONTROL_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL36_ACC_CONTROL_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL36_ACC_CONTROL_Fill_acc_control_Edit()
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



            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_1acc" +
                                  " WHERE (left(txtacc_id,1) = '1')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtacc_id = '" + PANEL36_ACC_CONTROL_txtacc_id.Text.Trim() + "')" +
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

                        PANEL36_ACC_CONTROL_txtacc_name.Text = dt2.Rows[0]["txtacc_name"].ToString();      //2
                        PANEL36_ACC_CONTROL_txtacc_name_eng.Text = dt2.Rows[0]["txtacc_name_eng"].ToString();      //3

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
        //END Acc_control =======================================================================

        //Branch=======================================================================

        private void Paneldate_btndate1_Click(object sender, EventArgs e)
        {
            if (this.Paneldate_dtptxtcus_birth_day.Visible == false)
            {
                this.Paneldate_dtptxtcus_birth_day.Visible = true;
                this.Paneldate_dtptxtcus_birth_day.BringToFront();
                this.Paneldate_dtptxtcus_birth_day.Location = new Point(this.Paneldate_txtdate.Location.X, this.Paneldate_txtdate.Location.Y + 22);
                this.Paneldate_btndate1.Visible = false;
                this.Paneldate_btndate1_close.Visible = true;
                this.Paneldate_btndate1_close.BringToFront();
                this.Paneldate_btndate1_close.Location = new Point(this.Paneldate_txtdate.Location.X + 140, this.Paneldate_txtdate.Location.Y + 24);

            }
            else
            {
                this.Paneldate_dtptxtcus_birth_day.Visible = false;
                this.Paneldate_btndate1.Visible = true;
                this.Paneldate_btndate1_close.Visible = false;
            }
        }

        private void dtpcus_birth_day_ValueChanged(object sender, EventArgs e)
        {
            this.Paneldate_dtptxtcus_birth_day.Format = DateTimePickerFormat.Custom;
            this.Paneldate_dtptxtcus_birth_day.CustomFormat = this.Paneldate_dtptxtcus_birth_day.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.Paneldate_txtdate.Text = this.Paneldate_dtptxtcus_birth_day.Value.ToString("dd-MM-yyyy", UsaCulture);
        }
        private void Paneldate_btndate1_close_Click(object sender, EventArgs e)
        {
            this.Paneldate_btndate1_close.Visible = false;
            this.Paneldate_btndate1.Visible = true;
            this.Paneldate_dtptxtcus_birth_day.Visible = false;
        }
        private void txtcus_card_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtcus_card_id.Text == "")
            {
                this.txtcus_card_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtcus_card_id.Text.Trim() != "")
            {
                this.txtcus_registered_id.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtcus_card_id.Text.Length == 0)
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

        private void txtcus_registered_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtcus_registered_id.Text == "")
            {
                this.txtcus_registered_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtcus_registered_id.Text.Trim() != "")
            {
                this.txtcus_registered_capital.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtcus_registered_id.Text.Length == 0)
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

        private void txtcus_registered_capital_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtcus_registered_capital.Text == "")
            {
                this.txtcus_registered_capital.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtcus_registered_capital.Text.Trim() != "")
            {
                this.txtcus_tax_id.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtcus_registered_capital.Text.Length == 0)
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


        private void txtcus_tax_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtcus_tax_id.Text == "")
            {
                this.txtcus_tax_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtcus_tax_id.Text.Trim() != "")
            {
                this.tabPage6.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtcus_tax_id.Text.Length == 0)
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

        private void btnpicture1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture1.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture1.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture1.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture1.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size1.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture2.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture2.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture2.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture2.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size2.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture3.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture3.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture3.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture3.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size3.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture4.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture4.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture4.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture4.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size4.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture5.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture5.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture5.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture5.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size5.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture6_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture6.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture6.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture6.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture6.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size6.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnUp_pic1_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        cmd2.CommandText = "UPDATE s001_03cus_4picture SET " +
                                                                       "txtcus_1picture_size = @txtcus_1picture_size," +
                                                                       "txtcus_1picture = @txtcus_1picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture1.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture1.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size1.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture", imageBt));
                        }

                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic2_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        cmd2.CommandText = "UPDATE s001_03cus_4picture SET " +
                                                                       "txtcus_2picture_size = @txtcus_2picture_size," +
                                                                       "txtcus_2picture = @txtcus_2picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture2.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture2.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size2.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture", imageBt));
                        }

                        //==============================                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic3_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        cmd2.CommandText = "UPDATE s001_03cus_4picture SET " +
                                                                       "txtcus_3picture_size = @txtcus_3picture_size," +
                                                                       "txtcus_3picture = @txtcus_3picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture3.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture3.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size3.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture", imageBt));
                        }

                        //==============================


                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic4_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        cmd2.CommandText = "UPDATE s001_03cus_4picture SET " +
                                                                       "txtcus_4picture_size = @txtcus_4picture_size," +
                                                                       "txtcus_4picture = @txtcus_4picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture4.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture4.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size4.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture", imageBt));
                        }

                        //==============================
                        //==============================


                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic5_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        cmd2.CommandText = "UPDATE s001_03cus_4picture SET " +
                                                                       "txtcus_5picture_size = @txtcus_5picture_size," +
                                                                       "txtcus_5picture = @txtcus_5picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        //'===================================='
                        if (this.txtpicture5.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture5.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture_size", this.txtpicture_size5.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture5.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size5.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture5.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture_size", this.txtpicture_size5.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture", imageBt));
                        }

                        //==============================


                        //==============================
                        //==============================


                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic6_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                    {
                        cmd2.CommandText = "UPDATE s001_03cus_4picture SET " +
                                                                       "txtcus_6picture_size = @txtcus_6picture_size," +
                                                                       "txtcus_6picture = @txtcus_6picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtcus_id = '" + this.txtcus_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        //'===================================='
                        //'===================================='
                        if (this.txtpicture6.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture6.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture_size", this.txtpicture_size6.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture6.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size6.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture6.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture_size", this.txtpicture_size6.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture", imageBt));
                        }

                        //==============================

                        //==============================


                        //==============================
                        //==============================


                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขลูกค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void Fill_PANEL_FORM1_dataGridView1()
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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtcus_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtcus_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtcus_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtcus_status";

                cmd2.CommandText = "SELECT s001_03cus.*," +
                                    "s001_03cus_1address.*" +
                                    " FROM s001_03cus" +

                                    " INNER JOIN s001_03cus_1address" +
                                    " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                    " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                    " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                    " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (s001_03cus.txtcus_id <> '')" +
                                  " ORDER BY s001_03cus.txtcus_no ASC";

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
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtcus_no"].Value = dt2.Rows[j]["txtcus_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtcus_id"].Value = dt2.Rows[j]["txtcus_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtcus_name"].Value = dt2.Rows[j]["txtcus_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtcus_name_eng"].Value = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtcus_status"].Value = dt2.Rows[j]["txtcus_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
        private void PANEL_FORM1_GridView1()
        {
            this.GridView1.ColumnCount = 9;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtcus_no";
            this.GridView1.Columns[2].Name = "Col_txtcus_id";
            this.GridView1.Columns[3].Name = "Col_txtcus_name";
            this.GridView1.Columns[4].Name = "Col_txtcus_name_eng";
            this.GridView1.Columns[5].Name = "Col_txtcontact_person";
            this.GridView1.Columns[6].Name = "Col_txtcontact_person_tel";
            this.GridView1.Columns[7].Name = "Col_txtremark";
            this.GridView1.Columns[8].Name = "Col_txtcus_status";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "ลำดับ";
            this.GridView1.Columns[2].HeaderText = " รหัส";
            this.GridView1.Columns[3].HeaderText = " ชื่อ cus";
            this.GridView1.Columns[4].HeaderText = " ชื่อลูกค้าEng";
            this.GridView1.Columns[5].HeaderText = " ผู้ติดต่อ";
            this.GridView1.Columns[6].HeaderText = " เบอร์โทร";
            this.GridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.GridView1.Columns[8].HeaderText = " สถานะ";

            this.GridView1.Columns[0].Visible = false;  //"No";
            this.GridView1.Columns[1].Visible = true;  //"Col_txtcus_no";
            this.GridView1.Columns[1].Width = 100;
            this.GridView1.Columns[1].ReadOnly = true;
            this.GridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[2].Visible = true;  //"Col_txtcus_id";
            this.GridView1.Columns[2].Width = 150;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[3].Visible = true;  //"Col_txtcus_name";
            this.GridView1.Columns[3].Width = 150;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[4].Visible = false;  //"Col_txtcus_name_eng";
            this.GridView1.Columns[4].Width = 150;
            this.GridView1.Columns[4].ReadOnly = true;
            this.GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[5].Visible = true;  //"Col_txtcontact_person";
            this.GridView1.Columns[5].Width = 150;
            this.GridView1.Columns[5].ReadOnly = true;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[6].Visible = false;  //"Col_txtcontact_person_tel";
            this.GridView1.Columns[6].Width = 150;
            this.GridView1.Columns[6].ReadOnly = true;
            this.GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[7].Visible = true;  //"Col_txtremark";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = true;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[8].Visible = false;  //"Col_txtcus_status";

            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView1.Columns.Add(dgvCmb);


        }
        private void PANEL_FORM1_Clear_GridView1()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
        }
        private void PANEL_FORM1_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.GridView1.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.GridView1.Rows[i].Cells[9].Value = false;

                }
            }
        }
        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            Fill_PANEL_FORM1_dataGridView1();
        }
        private void PANEL_FORM1_btnsearch_Click(object sender, EventArgs e)
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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtcus_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtcus_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtcus_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtcus_status";

                cmd2.CommandText = "SELECT s001_03cus.*," +
                                    "s001_03cus_1address.*" +
                                    " FROM s001_03cus" +

                                    " INNER JOIN s001_03cus_1address" +
                                    " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                    " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                    " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                    " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (s001_03cus.txtcus_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                     " AND (s001_03cus.txtcus_id <> '')" +
                                   " ORDER BY s001_03cus.txtcus_no ASC";

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
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtcus_no"].Value = dt2.Rows[j]["txtcus_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtcus_id"].Value = dt2.Rows[j]["txtcus_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtcus_name"].Value = dt2.Rows[j]["txtcus_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtcus_name_eng"].Value = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtcus_status"].Value = dt2.Rows[j]["txtcus_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
        private void PANEL_FORM1_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {

                    this.txtcus_no.Text = row.Cells[1].Value.ToString();
                    this.txtcus_id.Text = row.Cells[2].Value.ToString();
                    this.txtcus_name.Text = row.Cells[3].Value.ToString();

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
                    Clear_Text();
                    //===========================================
                    //เชื่อมต่อฐานข้อมูล======================================================
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {

                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;

                        cmd2.CommandText = "SELECT s001_03cus.*," +
                                            "s001_03cus_1address.*," +
                                            "s001_03cus_2account.*," +
                                            "s001_03cus_3detail.*," +
                                            "s001_03cus_4picture.*" +
                                            " FROM s001_03cus" +

                                            " INNER JOIN s001_03cus_1address" +
                                            " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                            " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                            " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                            " INNER JOIN s001_03cus_2account" +
                                            " ON s001_03cus.cdkey = s001_03cus_2account.cdkey" +
                                            " AND s001_03cus.txtco_id = s001_03cus_2account.txtco_id" +
                                            " AND s001_03cus.txtcus_id = s001_03cus_2account.txtcus_id" +

                                            " INNER JOIN s001_03cus_3detail" +
                                            " ON s001_03cus.cdkey = s001_03cus_3detail.cdkey" +
                                            " AND s001_03cus.txtco_id = s001_03cus_3detail.txtco_id" +
                                            " AND s001_03cus.txtcus_id = s001_03cus_3detail.txtcus_id" +

                                            " INNER JOIN s001_03cus_4picture" +
                                            " ON s001_03cus.cdkey = s001_03cus_4picture.cdkey" +
                                            " AND s001_03cus.txtco_id = s001_03cus_4picture.txtco_id" +
                                            " AND s001_03cus.txtcus_id = s001_03cus_4picture.txtcus_id" +


                                            " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (s001_03cus.txtcus_id = '" + this.txtcus_id.Text.Trim() + "')" +
                                            " ORDER BY s001_03cus.txtcus_no ASC";

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
                                    this.txtcus_id.Text = dt2.Rows[j]["txtcus_id"].ToString();      //1
                                    this.txtcus_no.Text = dt2.Rows[j]["txtcus_no"].ToString();      //2
                                    this.txtcus_name.Text = dt2.Rows[j]["txtcus_name"].ToString();      //3
                                    this.txtcus_name_eng.Text = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                                    if (dt2.Rows[j]["txtcus_status"].ToString() == "0") //5
                                    {
                                        this.txtcus_status.Checked = true;
                                    }
                                    else
                                    {
                                        this.txtcus_status.Checked = false;
                                    }
                                    this.txtprefix_id.Text = dt2.Rows[j]["txtprefix_id"].ToString();      //6  *************
                                    this.txtcontact_person.Text = dt2.Rows[j]["txtcontact_person"].ToString();      //7
                                    this.txtcontact_person_tel.Text = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //8
                                    if (dt2.Rows[j]["chcus_branch"].ToString() == "HO") //9
                                    {
                                        this.chcus_office.Checked = true;
                                        this.chcus_branch.Checked = false;
                                    }
                                    else
                                    {
                                        this.chcus_office.Checked = false;
                                        this.chcus_branch.Checked = true;
                                    }
                                    this.txtcus_branch_id.Text = dt2.Rows[j]["txtcus_branch_id"].ToString();      //10
                                    this.txtcus_tel.Text = dt2.Rows[j]["txtcus_tel"].ToString();      //11
                                    this.txtcus_fax.Text = dt2.Rows[j]["txtcus_fax"].ToString();      //12
                                    this.txtcus_email.Text = dt2.Rows[j]["txtcus_email"].ToString();      //13
                                    this.txtcus_homepage.Text = dt2.Rows[j]["txtcus_homepage"].ToString();      //14
                                    this.txthome_id.Text = dt2.Rows[j]["txthome_id"].ToString();      //15
                                    this.txttambon.Text = dt2.Rows[j]["txttambon"].ToString();      //16
                                    this.txtamphur.Text = dt2.Rows[j]["txtamphur"].ToString();      //17
                                    this.txtchangwat.Text = dt2.Rows[j]["txtchangwat"].ToString();      //18
                                    this.txtpost_id.Text = dt2.Rows[j]["txtpost_id"].ToString();      //19
                                    this.txthome_id_full.Text = dt2.Rows[j]["txthome_id_full"].ToString();      //20
                                    this.txthome_id_full_eng.Text = dt2.Rows[j]["txthome_id_full_eng"].ToString();      //21
                                    this.txtremark.Text = dt2.Rows[j]["txtremark"].ToString();      //22

                                    this.PANEL36_ACC_CONTROL_txtacc_id.Text = dt2.Rows[j]["txtacc_id"].ToString();      //23  *****************
                                    this.txtcredit_day.Text = dt2.Rows[j]["txtcredit_day"].ToString();      //24
                                    //this.PANEL2_BRANCH_txtbranch_id.Text = dt2.Rows[j]["txtbranch_id"].ToString();      //25
                                    //this.PANEL162_SUP_TYPE_txtcus_type_id.Text = dt2.Rows[j]["txtcus_type_id"].ToString();      //26
                                    //this.PANEL163_SUP_GROUP_txtcus_group_id.Text = dt2.Rows[j]["txtcus_group_id"].ToString();      //27
                                    //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[j]["txtacc_group_tax_id"].ToString();      //28
                                    //this.PANEL1309_CODE_BANK_txtcode_bank_id.Text = dt2.Rows[j]["txtcode_bank_id"].ToString();      //29
                                    //this.PANEL1310_CODE_BANK_BRANCH_txtcode_bank_branch_id.Text = dt2.Rows[j]["txtcode_bank_branch_id"].ToString();      //30


                                    //this.txtnumber_acc_bank.Text = dt2.Rows[j]["txtnumber_acc_bank"].ToString();      //31
                                    //this.txtcharge_to_id.Text = dt2.Rows[j]["txtcharge_to_id"].ToString();      //32

                                    this.Paneldate_txtdate.Text = dt2.Rows[j]["txtcus_birth_day"].ToString();      //33

                                    this.txtcus_card_id.Text = dt2.Rows[j]["txtcus_card_id"].ToString();      //34
                                    this.txtcus_registered_id.Text = dt2.Rows[j]["txtcus_registered_id"].ToString();      //35
                                    this.txtcus_registered_capital.Text = Convert.ToSingle(dt2.Rows[j]["txtcus_registered_capital"]).ToString("###,###.00");     //36

                                    this.txtcus_tax_id.Text = dt2.Rows[j]["txtcus_tax_id"].ToString();      //37
                                    //this.txtcus_kind_id.Text = dt2.Rows[j]["txtcus_kind_id"].ToString();      //38

                                    //Load Picture================================
                                    this.txtpicture_size1.Text = dt2.Rows[0]["txtcus_1picture_size"].ToString();
                                    this.txtpicture_size2.Text = dt2.Rows[0]["txtcus_2picture_size"].ToString();
                                    this.txtpicture_size3.Text = dt2.Rows[0]["txtcus_3picture_size"].ToString();
                                    this.txtpicture_size4.Text = dt2.Rows[0]["txtcus_4picture_size"].ToString();
                                    this.txtpicture_size5.Text = dt2.Rows[0]["txtcus_5picture_size"].ToString();
                                    this.txtpicture_size6.Text = dt2.Rows[0]["txtcus_6picture_size"].ToString();

                                    //=======================================================
                                    if (this.txtpicture_size1.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg1 = (byte[])(dt2.Rows[0]["txtcus_1picture"]);
                                        if (imgg1 == null)
                                        {
                                            this.Pic_picture1.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream1 = new MemoryStream(imgg1);
                                            this.Pic_picture1.Image = Image.FromStream(mstream1);
                                            this.Pic_picture1.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture1.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic1.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size2.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg2 = (byte[])(dt2.Rows[0]["txtcus_2picture"]);
                                        if (imgg2 == null)
                                        {
                                            this.Pic_picture2.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream2 = new MemoryStream(imgg2);
                                            this.Pic_picture2.Image = Image.FromStream(mstream2);
                                            this.Pic_picture2.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture2.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic2.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size3.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg3 = (byte[])(dt2.Rows[0]["txtcus_3picture"]);
                                        if (imgg3 == null)
                                        {
                                            this.Pic_picture3.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream3 = new MemoryStream(imgg3);
                                            this.Pic_picture3.Image = Image.FromStream(mstream3);
                                            this.Pic_picture3.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture3.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic3.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size4.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg4 = (byte[])(dt2.Rows[0]["txtcus_4picture"]);
                                        if (imgg4 == null)
                                        {
                                            this.Pic_picture4.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream4 = new MemoryStream(imgg4);
                                            this.Pic_picture4.Image = Image.FromStream(mstream4);
                                            this.Pic_picture4.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture4.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic4.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size5.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg5 = (byte[])(dt2.Rows[0]["txtcus_5picture"]);
                                        if (imgg5 == null)
                                        {
                                            this.Pic_picture5.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream5 = new MemoryStream(imgg5);
                                            this.Pic_picture5.Image = Image.FromStream(mstream5);
                                            this.Pic_picture5.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture5.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic5.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size6.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg6 = (byte[])(dt2.Rows[0]["txtcus_6picture"]);
                                        if (imgg6 == null)
                                        {
                                            this.Pic_picture6.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream6 = new MemoryStream(imgg6);
                                            this.Pic_picture6.Image = Image.FromStream(mstream6);
                                            this.Pic_picture6.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture6.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic6.Visible = true;
                                        }
                                    }
                                    //=======================================================

                                }
                                //Load Picture================================
                                //===========================================

                                if (this.txtcus_id.Text != "")
                                {
                                    this.iblword_status.Text = "แก้ไขลูกค้า";
                                    this.txtcus_id.ReadOnly = true;
                                    this.BtnCancel_Doc.Enabled = true;

                                    this.btnUp_pic1.Visible = true;
                                    this.btnUp_pic2.Visible = true;
                                    this.btnUp_pic3.Visible = true;
                                    this.btnUp_pic4.Visible = true;
                                    this.btnUp_pic5.Visible = true;
                                    this.btnUp_pic6.Visible = true;

                                    fill_cbo_profix_Edit();
                                    //fill_cbo_charge_to_Edit();
                                    //fill_cbo_cus_kind_Edit();
                                    //PANEL1310_CODE_BANK_BRANCH_Fill_code_bank_branch_Edit();
                                    //PANEL1309_CODE_BANK_Fill_code_bank_Edit();
                                    //PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax_Edit();
                                    //PANEL163_SUP_GROUP_Fill_cus_group_Edit();
                                    //PANEL162_SUP_TYPE_Fill_cus_type_Edit();
                                    //PANEL2_BRANCH_Fill_branch_Edit();
                                    PANEL36_ACC_CONTROL_Fill_acc_control_Edit();


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
            }
        }
        private void PANEL_FORM1_dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }

        private void PANEL_FORM1_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void Clear_Text()
        {
            this.txtcus_name.Text = "";
            this.txtcus_name_eng.Text = "";
            this.txtcontact_person.Text = "";
            this.txtcontact_person_tel.Text = "";
            this.txtcus_branch_id.Text = "";
            this.txtcus_tel.Text = "";
            this.txtcus_fax.Text = "";
            this.txtcus_email.Text = "";
            this.txtcus_homepage.Text = "";
            this.txthome_id.Text = "";
            this.txttambon.Text = "";
            this.txtamphur.Text = "";
            this.txtchangwat.Text = "";
            this.txtpost_id.Text = "";
            this.txthome_id_full.Text = "";
            this.txthome_id_full_eng.Text = "";
            this.txtremark.Text = "";

            this.PANEL36_ACC_CONTROL_txtacc_name.Text = "";
            this.PANEL36_ACC_CONTROL_txtacc_id.Text = "";
            this.PANEL36_ACC_CONTROL_txtacc_name_eng.Text = "";
            this.txtcredit_day.Text = "";
            //this.PANEL2_BRANCH_txtbranch_name.Text = "";
            //this.PANEL2_BRANCH_txtbranch_id.Text = "";
            //this.PANEL162_SUP_TYPE_txtcus_type_name.Text = "";
            //this.PANEL162_SUP_TYPE_txtcus_type_id.Text = "";
            //this.PANEL163_SUP_GROUP_txtcus_group_name.Text = "";
            //this.PANEL163_SUP_GROUP_txtcus_group_id.Text = "";
            //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = "";
            //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = "";
            //this.PANEL1309_CODE_BANK_txtcode_bank_name.Text = "";
            //this.PANEL1309_CODE_BANK_txtcode_bank_id.Text = "";
            //this.PANEL1310_CODE_BANK_BRANCH_txtcode_bank_branch_name.Text = "";
            //this.PANEL1310_CODE_BANK_BRANCH_txtcode_bank_branch_id.Text = "";
            //this.txtnumber_acc_bank.Text = "";
            //this.cbocharge_to_name.Text = "";
            //this.txtcharge_to_id.Text = "";

            this.Paneldate_txtdate.Text = "";

            this.txtcus_card_id.Text = "";
            this.txtcus_registered_id.Text = "";
            this.txtcus_registered_capital.Text = "0";
            this.txtcus_tax_id.Text = "";
            //this.Cbocus_kind_name.Text = "";

            this.Pic_picture1.Image = null;
            this.Pic_picture2.Image = null;
            this.Pic_picture3.Image = null;
            this.Pic_picture4.Image = null;
            this.Pic_picture5.Image = null;
            this.Pic_picture6.Image = null;

        }


        private void CHECK_UP_NO999()
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

            string OK = "";

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT s001_03cus.*," +
                                    "s001_03cus_1address.*" +
                                    " FROM s001_03cus" +

                                    " INNER JOIN s001_03cus_1address" +
                                    " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                    " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                    " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                    " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (s001_03cus.txtcus_id = '')" +
                                  " ORDER BY s001_03cus.txtcus_no ASC";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    OK = "Y";
                    conn.Close();
                    return;
                }
            }

            //
            conn.Close();
            //END เชื่อมต่อฐานข้อมูล=======================================================

            if (OK.Trim() != "Y")
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
                        //1
                        cmd2.CommandText = "INSERT INTO s001_03cus(cdkey,txtco_id," +
                                           "txtcus_no,txtcus_id," +
                                           "txtcus_name," +
                                           "txtcus_name_eng," +
                                           "txtcus_status) " +
                                           "VALUES (@cdkey,@txtco_id," +
                                           "@txtcus_no,@txtcus_id," +
                                           "@txtcus_name," +
                                           "@txtcus_name_eng," +
                                           "@txtcus_status)";

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_no", SqlDbType.NVarChar).Value = "999";
                        cmd2.Parameters.Add("@txtcus_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcus_name", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcus_name_eng", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcus_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok1");

                        //2
                        cmd2.CommandText = "INSERT INTO s001_03cus_1address(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtprefix_id," +  //3
                                           "txtcontact_person," +  //4
                                           "txtcontact_person_tel," +  //5
                                           "chcus_branch," +  //6
                                           "txtcus_branch_id," +  //7
                                           "txtcus_tel," +  //8
                                           "txtcus_fax," +  //9
                                           "txtcus_email," +  //10
                                           "txtcus_homepage," +  //11
                                           "txthome_id," +  //12
                                           "txttambon," +  //13
                                           "txtamphur," +  //14
                                           "txtchangwat," +  //15
                                           "txtpost_id," +  //16
                                           "txthome_id_full," +  //17
                                           "txthome_id_full_eng," +  //18
                                           "txtremark) " +  //19
                                           "VALUES (@cdkey2,@txtco_id2," +
                                          "@txtcus_id2," +  //2
                                           "@txtprefix_id," +  //3
                                           "@txtcontact_person," +  //4
                                           "@txtcontact_person_tel," +  //5
                                           "@chcus_branch," +  //6
                                           "@txtcus_branch_id," +  //7
                                           "@txtcus_tel," +  //8
                                           "@txtcus_fax," +  //9
                                           "@txtcus_email," +  //10
                                           "@txtcus_homepage," +  //11
                                           "@txthome_id," +  //12
                                           "@txttambon," +  //13
                                           "@txtamphur," +  //14
                                           "@txtchangwat," +  //15
                                           "@txtpost_id," +  //16
                                           "@txthome_id_full," +  //17
                                           "@txthome_id_full_eng," +  //18
                                           "@txtremark)";  //19

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id2", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtprefix_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcontact_person", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcontact_person_tel", SqlDbType.NVarChar).Value = "";

                        string HH = "";
                        if (this.chcus_office.Checked == true)
                        {
                            HH = "HO";
                        }
                        if (this.chcus_branch.Checked == true)
                        {
                            HH = "BR";
                        }
                        cmd2.Parameters.Add("@chcus_branch", SqlDbType.NVarChar).Value = HH.ToString();
                        cmd2.Parameters.Add("@txtcus_branch_id", SqlDbType.NVarChar).Value = this.txtcus_branch_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_tel", SqlDbType.NVarChar).Value = this.txtcus_tel.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_fax", SqlDbType.NVarChar).Value = this.txtcus_fax.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_email", SqlDbType.NVarChar).Value = this.txtcus_email.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_homepage", SqlDbType.NVarChar).Value = this.txtcus_homepage.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id", SqlDbType.NVarChar).Value = this.txthome_id.Text.ToString();
                        cmd2.Parameters.Add("@txttambon", SqlDbType.NVarChar).Value = this.txttambon.Text.ToString();
                        cmd2.Parameters.Add("@txtamphur", SqlDbType.NVarChar).Value = this.txtamphur.Text.ToString();
                        cmd2.Parameters.Add("@txtchangwat", SqlDbType.NVarChar).Value = this.txtchangwat.Text.ToString();
                        cmd2.Parameters.Add("@txtpost_id", SqlDbType.NVarChar).Value = this.txtpost_id.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id_full", SqlDbType.NVarChar).Value = this.txthome_id_full.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id_full_eng", SqlDbType.NVarChar).Value = this.txthome_id_full_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtremark", SqlDbType.NVarChar).Value = this.txtremark.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok2");

                        //3
                        cmd2.CommandText = "INSERT INTO s001_03cus_2account(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtacc_id," +  //3
                                           "txtcredit_day," +  //4
                                           "txtbranch_id," +  //5
                                           "txtcus_type_id," + //6
                                           "txtcus_group_id," +  //7
                                           "txtacc_group_tax_id," +  //8
                                           "txtcode_bank_id," +  //9
                                           "txtcode_bank_branch_id," +  //10
                                           "txtnumber_acc_bank," +  //11
                                           "txtcharge_to_id) " +   //12
                                           "VALUES (@cdkey3,@txtco_id3," +
                                           "@txtcus_id3," +  //2
                                           "@txtacc_id," +  //3
                                           "@txtcredit_day," +  //4
                                           "@txtbranch_id," +  //5
                                           "@txtcus_type_id," + //6
                                           "@txtcus_group_id," +  //7
                                           "@txtacc_group_tax_id," +  //8
                                           "@txtcode_bank_id," +  //9
                                           "@txtcode_bank_branch_id," +  //10
                                           "@txtnumber_acc_bank," +  //11
                                           "@txtcharge_to_id)";  //12

                        cmd2.Parameters.Add("@cdkey3", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id3", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id3", SqlDbType.NVarChar).Value = "";

                        cmd2.Parameters.Add("@txtacc_id", SqlDbType.NVarChar).Value = this.PANEL36_ACC_CONTROL_txtacc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcredit_day", SqlDbType.NVarChar).Value = this.txtcredit_day.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcus_type_id", SqlDbType.NVarChar).Value = this.PANEL_02CUS_TYPE_txtcus_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_group_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value =  "";
                        cmd2.Parameters.Add("@txtcode_bank_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcode_bank_branch_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtnumber_acc_bank", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtcharge_to_id", SqlDbType.NVarChar).Value = "";

                        //==============================

                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok3");

                        //4
                        cmd2.CommandText = "INSERT INTO s001_03cus_3detail(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtcus_birth_day," +  //3
                                           "txtcus_card_id," +  //4
                                           "txtcus_registered_id," +  //5
                                           "txtcus_registered_capital," +  //6
                                           "txtcus_tax_id," +  //7
                                           "txtcus_kind_id) " +  //8
                                           "VALUES (@cdkey4,@txtco_id4," +
                                           "@txtcus_id4," +  //2
                                           "@txtcus_birth_day," +  //3
                                           "@txtcus_card_id," +  //4
                                           "@txtcus_registered_id," +  //5
                                           "@txtcus_registered_capital," +  //6
                                           "@txtcus_tax_id," +  //7
                                           "@txtcus_kind_id)";  //8

                        cmd2.Parameters.Add("@cdkey4", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id4", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id4", SqlDbType.NVarChar).Value = "";


                        cmd2.Parameters.Add("@txtcus_birth_day", SqlDbType.NVarChar).Value = this.Paneldate_txtdate.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_card_id", SqlDbType.NVarChar).Value = this.txtcus_card_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_registered_id", SqlDbType.NVarChar).Value = this.txtcus_registered_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_registered_capital", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcus_registered_capital.Text.ToString()));
                        cmd2.Parameters.Add("@txtcus_tax_id", SqlDbType.NVarChar).Value = this.txtcus_tax_id.Text.ToString();
                        cmd2.Parameters.Add("@txtcus_kind_id", SqlDbType.NVarChar).Value = "";
                        //==============================

                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok4");

                        //5
                        cmd2.CommandText = "INSERT INTO s001_03cus_4picture(cdkey,txtco_id," +  //1
                                           "txtcus_id," +  //2
                                           "txtcus_1picture_size,txtcus_1picture," +  //3
                                           "txtcus_2picture_size,txtcus_2picture," +  //4
                                           "txtcus_3picture_size,txtcus_3picture," +  //5
                                           "txtcus_4picture_size,txtcus_4picture," +  //6
                                           "txtcus_5picture_size,txtcus_5picture," +  //7
                                           "txtcus_6picture_size,txtcus_6picture) " +  //8
                                           "VALUES (@cdkey5,@txtco_id5," +
                                           "@txtcus_id5," +  //2
                                           "@txtcus_1picture_size,@txtcus_1picture," +  //3
                                           "@txtcus_2picture_size,@txtcus_2picture," +  //4
                                           "@txtcus_3picture_size,@txtcus_3picture," +  //5
                                           "@txtcus_4picture_size,@txtcus_4picture," +  //6
                                           "@txtcus_5picture_size,@txtcus_5picture," +  //7
                                           "@txtcus_6picture_size,@txtcus_6picture)";  //8

                        cmd2.Parameters.Add("@cdkey5", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id5", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtcus_id5", SqlDbType.NVarChar).Value = "";

                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture1.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture1.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size1.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_1picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture2.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture2.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size2.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_2picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture3.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture3.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size3.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_3picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture4.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture4.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size4.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_4picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture5.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture5.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture_size", this.txtpicture_size5.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture5.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size5.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture5.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture_size", this.txtpicture_size5.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_5picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture6.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture6.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture_size", this.txtpicture_size6.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture6.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size6.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture6.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture_size", this.txtpicture_size6.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtcus_6picture", imageBt));
                        }

                        //==============================


                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok5");






                        trans.Commit();
                        conn.Close();

                        Cursor.Current = Cursors.Default;
                    }
                    //MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    catch (SqlException)
                    {
                        return;
                    }
                    //END เชื่อมต่อฐานข้อมูล=======================================================
                }
                //=============================================================

            }


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





        //Tans_Log ====================================================================
    }
}
