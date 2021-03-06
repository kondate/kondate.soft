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


namespace kondate.soft.SETUP_2ACC
{
    public partial class Home_SETUP_Enter_2ACC_08_acc : Form
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




        public Home_SETUP_Enter_2ACC_08_acc()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_2ACC_08_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //this.btnmaximize.Visible = false;
            //this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "S208";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มบัญชีใหม่";
            this.txtacc_id.ReadOnly = false;

            this.ActiveControl = this.txtacc_id;

            PANEL32_ACC_TYPE_GridView1_acc_type();
            PANEL32_ACC_TYPE_Fill_acc_type();

            PANEL33_ACC_WORK_TYPE_GridView1_acc_work_type();
            PANEL33_ACC_WORK_TYPE_Fill_acc_work_type();

            PANEL35_ACC_BALANCE_TYPE_GridView1_acc_balance_type();
            PANEL35_ACC_BALANCE_TYPE_Fill_acc_balance_type();

            PANEL34_ACC_DEGREE_GridView1_acc_degree();
            PANEL34_ACC_DEGREE_Fill_acc_degree();

            PANEL36_ACC_CONTROL_GridView1_acc_control();
            PANEL36_ACC_CONTROL_Fill_acc_control();

            PANEL_FORM1_GridView1_acc();
            PANEL_FORM1_Fill_acc();


            CHECK_UP_NO999();
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
            W_ID_Select.LOG_ID = "9";
            W_ID_Select.LOG_NAME = "ปิดหน้าจอ";
            TRANS_LOG();

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
                        var frm2 = new Home_SETUP_Enter_2ACC_08_acc();
                        frm2.Closed += (s, args) => this.Close();
                        frm2.Show();

                        this.iblword_status.Text = "เพิ่มบัญชีใหม่";
                        this.txtacc_id.ReadOnly = false;

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

                if (this.txtacc_id.Text != "")
                {
                    this.iblword_status.Text = "แก้ไขบัญชี";
                    this.txtacc_id.ReadOnly = true;
                }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

           if (this.txtacc_id.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัสบัญชีก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtacc_id.Focus();
                return;
            }
            else
            {
                if (this.txtacc_id.TextLength == 7)
                {
                }
                else
                {
                    MessageBox.Show("โปรดใส่รหัสบัญชี 7 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtacc_id.Focus();
                    return;
                }
            }
            if (this.txtacc_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อบัญชี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtacc_name.Focus();
                return;
            }
            if (this.PANEL32_ACC_TYPE_txtacc_type_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ หมวดบัญชี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL32_ACC_TYPE_txtacc_type_name.Focus();
                return;
            }
            if (this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ ประเภทการทำงาน ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Focus();
                return;
            }
            if (this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Text == "")
            {
                //MessageBox.Show("โปรดใส่ ยอดคงเหลือปกติ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Focus();
                //return;
            }
            if (this.PANEL34_ACC_DEGREE_txtacc_degree_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ ระดับบัญชี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL34_ACC_DEGREE_txtacc_degree_name.Focus();
                return;
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
            if (this.iblword_status.Text.Trim() == "เพิ่มบัญชีใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM k013db_1acc" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (lang_id = '" + W_ID_Select.Lang.Trim() + "')" +
                                       " AND (txtacc_id = '" + this.txtacc_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("รหัสบัญชีนี้ซ้ำ  : '" + this.txtacc_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtacc_id.Focus();
                        conn.Close();
                        return;
                    }
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มบัญชีใหม่")
                    {
                        cmd2.CommandText = "INSERT INTO k013db_1acc(cdkey,txtco_id,txtbranch_id," +
                                           "txtacc_id,txtacc_name,txtacc_name_eng," +
                                           "txtacc_type_id,txtacc_work_type_id," +
                                           "txtacc_balance_type_id,txtacc_degree_id," +
                                           "txtacc_id_control,txtacc_name_control," +
                                           "txttype_money_id,lang_id) " +
                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +
                                           "@txtacc_id,@txtacc_name,@txtacc_name_eng," +
                                           "@txtacc_type_id,@txtacc_work_type_id," +
                                           "@txtacc_balance_type_id,@txtacc_degree_id," +
                                           "@txtacc_id_control,@txtacc_name_control,@txttype_money_id,@lang_id)";

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();
                        cmd2.Parameters.Add("@txtacc_id", SqlDbType.NVarChar).Value = this.txtacc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_name", SqlDbType.NVarChar).Value = this.txtacc_name.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_name_eng", SqlDbType.NVarChar).Value = this.txtacc_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_type_id", SqlDbType.NVarChar).Value = this.PANEL32_ACC_TYPE_txtacc_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_work_type_id", SqlDbType.NVarChar).Value = this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_balance_type_id", SqlDbType.NVarChar).Value = this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_degree_id", SqlDbType.NVarChar).Value = this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_id_control", SqlDbType.NVarChar).Value = this.PANEL36_ACC_CONTROL_txtacc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_name_control", SqlDbType.NVarChar).Value = this.PANEL36_ACC_CONTROL_txtacc_name.Text.ToString();
                        cmd2.Parameters.Add("@txttype_money_id", SqlDbType.NVarChar).Value ="";
                        cmd2.Parameters.Add("@lang_id", SqlDbType.NVarChar).Value = W_ID_Select.Lang.Trim();
                        //==============================

                        cmd2.ExecuteNonQuery();

                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขบัญชี")
                    {
                        cmd2.CommandText = "UPDATE k013db_1acc SET " +
                                                                     "txtacc_name = '" + this.txtacc_name.Text.Trim() + "'," +
                                                                     "txtacc_name_eng = '" + this.txtacc_name_eng.Text.Trim() + "'," +
                                                                     "txtacc_type_id = '" + this.PANEL32_ACC_TYPE_txtacc_type_id.Text.Trim() + "'," +
                                                                     "txtacc_work_type_id = '" + this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text.Trim() + "'," +
                                                                     "txtacc_balance_type_id = '" + this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text.Trim() + "'," +
                                                                     "txtacc_degree_id = '" + this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text.Trim() + "'," +
                                                                     "txtacc_id_control = '" + this.PANEL36_ACC_CONTROL_txtacc_id.Text.Trim() + "'," +
                                                                      "txtacc_name_control = '" + this.PANEL36_ACC_CONTROL_txtacc_name.Text.Trim() + "'," +
                                                                     "txttype_money_id = ''" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (lang_id = '" + W_ID_Select.Lang.Trim() + "')" +
                                                                   " AND (txtacc_id = '" + this.txtacc_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มบัญชีใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();

                            PANEL_FORM1_Fill_acc();

                            this.iblword_status.Text = "เพิ่มบัญชีใหม่";
                            this.txtacc_id.ReadOnly = false;

                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขบัญชี")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();

                            //this.PANEL_FORM1_dataGridView1_acc.Columns[1].Name = "Col_txtacc_id";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[2].Name = "Col_txtacc_name";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[3].Name = "Col_txtacc_name_eng";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[4].Name = "Col_txtacc_type_id";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[5].Name = "Col_txtacc_type_name";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[6].Name = "Col_txtacc_work_type_id";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[7].Name = "Col_txtacc_work_type_name";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[8].Name = "Col_txtacc_degree_id";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[9].Name = "Col_txtacc_degree_name";

                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_id"].Value = this.txtacc_id.Text.ToString();      //1
                            if (PANEL34_ACC_DEGREE_txtacc_degree_id.Text == "1")
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name"].Value = this.txtacc_name.Text.ToString();      //2
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name_eng"].Value = this.txtacc_name_eng.Text.ToString();      //3
                            }
                            if (PANEL34_ACC_DEGREE_txtacc_degree_id.Text == "2")
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name"].Value = "     " + this.txtacc_name.Text.ToString();      //2
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name_eng"].Value = "     " + this.txtacc_name_eng.Text.ToString();      //3
                            }
                            if (PANEL34_ACC_DEGREE_txtacc_degree_id.Text == "3")
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name"].Value = "          " + this.txtacc_name.Text.ToString();      //2
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name_eng"].Value = "          " + this.txtacc_name_eng.Text.ToString();      //3
                            }
                            if (PANEL34_ACC_DEGREE_txtacc_degree_id.Text == "4")
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name"].Value = "               " + this.txtacc_name.Text.ToString();      //2
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name_eng"].Value = "               " + this.txtacc_name_eng.Text.ToString();      //3
                            }
                            if (PANEL34_ACC_DEGREE_txtacc_degree_id.Text == "5")
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name"].Value = "                    " + this.txtacc_name.Text.ToString();      //2
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_name_eng"].Value = "                    " + this.txtacc_name_eng.Text.ToString();      //3
                            }

                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_type_id"].Value = this.PANEL32_ACC_TYPE_txtacc_type_id.Text.ToString();      //4
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_type_name"].Value = this.PANEL32_ACC_TYPE_txtacc_type_name.Text.ToString();      //5
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_work_type_id"].Value = this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text.ToString();      //6
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_work_type_name"].Value = this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Text.ToString();      //7
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_degree_id"].Value = this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text.ToString();      //8
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtacc_degree_name"].Value = this.PANEL34_ACC_DEGREE_txtacc_degree_name.Text.ToString();      //9


                        }

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtacc_id.Text = "";
                        this.txtacc_name.Text = "";


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

                        string Cancel_ID =  W_ID_Select.CDKEY.Trim() + "-" + W_ID_Select.M_USERNAME.Trim() + "-" +  myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "-" + myDateTime2.ToString("HH:mm:ss", UsaCulture);

                        cmd2.CommandText = "INSERT INTO k013db_1acc_cancel(cdkey,txtco_id,txtbranch_id," +  //1
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


                        cmd2.CommandText = "INSERT INTO k013db_1acc_cancel_detail(cdkey,txtco_id,txtbranch_id," +
                                           "txtacc_id,txtacc_name,txtacc_name_eng," +
                                           "txtacc_type_id,txtacc_work_type_id," +
                                           "txtacc_balance_type_id,txtacc_degree_id," +
                                           "txtacc_id_control,txtacc_name_control," +
                                           "txttype_money_id,lang_id,cancel_id) " +
                                           "VALUES (@cdkey2,@txtco_id2,@txtbranch_id2," +
                                           "@txtacc_id2,@txtacc_name2,@txtacc_name_eng2," +
                                           "@txtacc_type_id2,@txtacc_work_type_id2," +
                                           "@txtacc_balance_type_id2,@txtacc_degree_id2," +
                                           "@txtacc_id_control2,@txtacc_name_control2,@txttype_money_id2,@lang_id2,@cancel_id2)";

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbranch_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();
                        cmd2.Parameters.Add("@txtacc_id2", SqlDbType.NVarChar).Value = this.txtacc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_name2", SqlDbType.NVarChar).Value = this.txtacc_name.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_name_eng2", SqlDbType.NVarChar).Value = this.txtacc_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_type_id2", SqlDbType.NVarChar).Value = this.PANEL32_ACC_TYPE_txtacc_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_work_type_id2", SqlDbType.NVarChar).Value = this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_balance_type_id2", SqlDbType.NVarChar).Value = this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_degree_id2", SqlDbType.NVarChar).Value = this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_id_control2", SqlDbType.NVarChar).Value = this.PANEL36_ACC_CONTROL_txtacc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtacc_name_control2", SqlDbType.NVarChar).Value = this.PANEL36_ACC_CONTROL_txtacc_name.Text.ToString();
                        cmd2.Parameters.Add("@txttype_money_id2", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@lang_id2", SqlDbType.NVarChar).Value = W_ID_Select.Lang.Trim();
                        cmd2.Parameters.Add("@cancel_id2", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================

                        cmd2.ExecuteNonQuery();


                        cmd2.CommandText = "DELETE FROM k013db_1acc" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (lang_id = '" + W_ID_Select.Lang.Trim() + "')" +
                                                                   " AND (txtacc_id = '" + this.txtacc_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();


                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัสบัญชี  " + this.txtacc_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
                        this.txtacc_id.Text = "";
                        this.txtacc_name.Text = "";

                        PANEL_FORM1_Fill_acc();
                        this.iblword_status.Text = "เพิ่มบัญชีใหม่";
                        this.txtacc_id.ReadOnly = false;

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
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
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
                            cmd2.CommandText = "DELETE FROM k013db_1acc_print";
                            cmd2.ExecuteNonQuery();


                    //this.PANEL_FORM1_dataGridView1_acc.Columns[0].Name = "Col_Auto_num";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[1].Name = "Col_txtacc_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[2].Name = "Col_txtacc_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[3].Name = "Col_txtacc_name_eng";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[4].Name = "Col_txtacc_type_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[5].Name = "Col_txtacc_type_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[6].Name = "Col_txtacc_work_type_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[7].Name = "Col_txtacc_work_type_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[8].Name = "Col_txtacc_degree_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[9].Name = "Col_txtacc_degree_name";


                    for (int i = 0; i < this.GridView1.Rows.Count ; i++)
                    {
                        if (this.GridView1.Rows[i].Cells[1].Value != null)
                        {

                                cmd2.CommandText = "INSERT INTO k013db_1acc_print(cdkey,txtco_id," +  //1
                                                   "txtacc_id," +  //2
                                                   "txtacc_name," +  //3
                                                   "txtacc_name_eng," +  //4
                                                   "txtacc_type_name," +  //5
                                                   "txtacc_work_type_name," +  //6
                                                   "txtacc_degree_name)" +   //7
                                                   "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" +  W_ID_Select.M_COID.Trim()  + "'," +  //1
                                                   "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //2
                                                   "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                                                   "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +  //4
                                                   "'" + this.GridView1.Rows[i].Cells[5].Value.ToString() + "'," +  //5
                                                   "'" + this.GridView1.Rows[i].Cells[7].Value.ToString() + "'," +  //6
                                                   "'" + this.GridView1.Rows[i].Cells[9].Value.ToString()  + "')";  //7

                                cmd2.ExecuteNonQuery();

                        }
                    }
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================


                    //===============================================================================================

                        trans.Commit();
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
                        rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_Chart_of_accounts.rpt");


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

                    //    //Print=============================================
                    //    var path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;


                //PrintDialog printDialog = new PrintDialog();
                //if (printDialog.ShowDialog() == DialogResult.OK)
                //{
                //    CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //    //reportDocument.Load(Application.StartupPath + "\\SETUP_2ACC\\Report_Chart_of_accounts.rpt");
                //    reportDocument.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\SETUP_2ACC\\Report_Chart_of_accounts.rpt");

                //    reportDocument.PrintOptions.PrinterName = printDialog.PrinterSettings.PrinterName;
                //    reportDocument.PrintToPrinter(printDialog.PrinterSettings.Copies, printDialog.PrinterSettings.Collate, printDialog.PrinterSettings.FromPage, printDialog.PrinterSettings.ToPage);
                //}
                ////END Print========================================
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
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
                    cmd2.CommandText = "DELETE FROM k013db_1acc_print";
                    cmd2.ExecuteNonQuery();


                    //this.PANEL_FORM1_dataGridView1_acc.Columns[0].Name = "Col_Auto_num";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[1].Name = "Col_txtacc_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[2].Name = "Col_txtacc_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[3].Name = "Col_txtacc_name_eng";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[4].Name = "Col_txtacc_type_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[5].Name = "Col_txtacc_type_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[6].Name = "Col_txtacc_work_type_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[7].Name = "Col_txtacc_work_type_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[8].Name = "Col_txtacc_degree_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[9].Name = "Col_txtacc_degree_name";


                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        if (this.GridView1.Rows[i].Cells[1].Value != null)
                        {

                            cmd2.CommandText = "INSERT INTO k013db_1acc_print(cdkey,txtco_id," +  //1
                                               "txtacc_id," +  //2
                                               "txtacc_name," +  //3
                                               "txtacc_name_eng," +  //4
                                               "txtacc_type_name," +  //5
                                               "txtacc_work_type_name," +  //6
                                               "txtacc_degree_name)" +   //7
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "'," +  //1
                                               "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //2
                                               "'" + this.GridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                                               "'" + this.GridView1.Rows[i].Cells[3].Value.ToString() + "'," +  //4
                                               "'" + this.GridView1.Rows[i].Cells[5].Value.ToString() + "'," +  //5
                                               "'" + this.GridView1.Rows[i].Cells[7].Value.ToString() + "'," +  //6
                                               "'" + this.GridView1.Rows[i].Cells[9].Value.ToString() + "')";  //7

                            cmd2.ExecuteNonQuery();

                        }
                    }
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================
                    //=============================================================================


                    //===============================================================================================

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


            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_08_acc_Print frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_08_acc_Print();
            frm2.Show();
            frm2.BringToFront();

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            W_ID_Select.LOG_ID = "9";
            W_ID_Select.LOG_NAME = "ปิดหน้าจอ";
            TRANS_LOG();

            this.Close();
        }


        private void btnEnter2ACC_Setup8_Set_Acount_Code_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnEnter2ACC_Setup8_Set_Acount_Code.Text.Trim();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_08_acc_import frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_08_acc_import();
            frm2.Show();

        }
        private void PANEL_FORM1_Fill_acc()
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

            PANEL_FORM1_Clear_GridView1_acc();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k013db_1acc.*," +
                                  "k013db_2acc_type.*," +
                                   "k013db_3acc_work_type.*," +
                                    "k013db_4acc_degree.*" +
                                    " FROM k013db_1acc" +

                                    " INNER JOIN k013db_2acc_type" +
                                    " ON k013db_1acc.txtacc_type_id = k013db_2acc_type.txtacc_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_2acc_type.lang_id" +

                                    " INNER JOIN k013db_3acc_work_type" +
                                    " ON k013db_1acc.txtacc_work_type_id = k013db_3acc_work_type.txtacc_work_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_3acc_work_type.lang_id" +

                                    " INNER JOIN k013db_4acc_degree" +
                                    " ON k013db_1acc.txtacc_degree_id = k013db_4acc_degree.txtacc_degree_id" +
                                    //" AND k013db_1acc.lang_id = k013db_4acc_degree.lang_id" +

                                    " WHERE (k013db_1acc.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k013db_1acc.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (k013db_1acc.txtacc_id <> '')" +
                                    " ORDER BY k013db_1acc.txtacc_id ASC";

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
                            GridView1.Rows[index].Cells["Col_txtacc_id"].Value = dt2.Rows[j]["txtacc_id"].ToString();      //1
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "1")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "2")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "     " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "     " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "3")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "          " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "          " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "4")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "               " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "               " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "5")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "                    " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "                    " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            GridView1.Rows[index].Cells["Col_txtacc_type_id"].Value = dt2.Rows[j]["txtacc_type_id"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtacc_type_name"].Value = dt2.Rows[j]["txtacc_type_name"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_id"].Value = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_name"].Value = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //8
                            GridView1.Rows[index].Cells["Col_txtacc_degree_name"].Value = dt2.Rows[j]["txtacc_degree_name"].ToString();      //9
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
        private void PANEL_FORM1_GridView1_acc()
        {
            this.GridView1.ColumnCount = 10;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtacc_id";
            this.GridView1.Columns[2].Name = "Col_txtacc_name";
            this.GridView1.Columns[3].Name = "Col_txtacc_name_eng";
            this.GridView1.Columns[4].Name = "Col_txtacc_type_id";
            this.GridView1.Columns[5].Name = "Col_txtacc_type_name";
            this.GridView1.Columns[6].Name = "Col_txtacc_work_type_id";
            this.GridView1.Columns[7].Name = "Col_txtacc_work_type_name";
            this.GridView1.Columns[8].Name = "Col_txtacc_degree_id";
            this.GridView1.Columns[9].Name = "Col_txtacc_degree_name";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "รหัส";
            this.GridView1.Columns[2].HeaderText = " ชื่อบัญชี";
            this.GridView1.Columns[3].HeaderText = " ชื่อบัญชี Eng";
            this.GridView1.Columns[4].HeaderText = "รหัสหมวดบัญชี";
            this.GridView1.Columns[5].HeaderText = " หมวดบัญชี";
            this.GridView1.Columns[6].HeaderText = " รหัสประเภท";
            this.GridView1.Columns[7].HeaderText = " ประเภท";
            this.GridView1.Columns[8].HeaderText = " รหัสระดับ";
            this.GridView1.Columns[9].HeaderText = " ระดับ";

            this.GridView1.Columns[0].Visible = false;  //"No";
            this.GridView1.Columns[1].Visible = true;  //"Col_txtacc_id";
            this.GridView1.Columns[1].Width = 100;
            this.GridView1.Columns[1].ReadOnly = true;
            this.GridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[2].Visible = true;  //"Col_txtacc_name";
            this.GridView1.Columns[2].Width = 150;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[3].Visible = true;  //"Col_txtacc_name_eng";
            this.GridView1.Columns[3].Width = 150;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[4].Visible = false;  //"Col_txtacc_type_id";
            this.GridView1.Columns[5].Visible = true;  //"Col_txtacc_type_name";
            this.GridView1.Columns[5].Width = 100;
            this.GridView1.Columns[5].ReadOnly = true;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[6].Visible = false;  //"Col_txtacc_work_type_id";
            this.GridView1.Columns[7].Visible = true;  //"Col_txtacc_work_type_name";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = true;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[8].Visible = false;  //"Col_txtacc_degree_id";
            this.GridView1.Columns[9].Visible = true;  //"Col_txtacc_degree_name";
            this.GridView1.Columns[9].Width = 100;
            this.GridView1.Columns[9].ReadOnly = true;
            this.GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;

        }
        private void PANEL_FORM1_Clear_GridView1_acc()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
        }
        private void PANEL_FORM1_dataGridView1_acc_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }

        private void PANEL_FORM1_dataGridView1_acc_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_FORM1_Fill_acc();
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

            PANEL_FORM1_Clear_GridView1_acc();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k013db_1acc.*," +
                                  "k013db_2acc_type.*," +
                                   "k013db_3acc_work_type.*," +
                                    "k013db_4acc_degree.*" +
                                    " FROM k013db_1acc" +

                                    " INNER JOIN k013db_2acc_type" +
                                    " ON k013db_1acc.txtacc_type_id = k013db_2acc_type.txtacc_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_2acc_type.lang_id" +

                                    " INNER JOIN k013db_3acc_work_type" +
                                    " ON k013db_1acc.txtacc_work_type_id = k013db_3acc_work_type.txtacc_work_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_3acc_work_type.lang_id" +

                                    " INNER JOIN k013db_4acc_degree" +
                                    " ON k013db_1acc.txtacc_degree_id = k013db_4acc_degree.txtacc_degree_id" +
                                    //" AND k013db_1acc.lang_id = k013db_4acc_degree.lang_id" +

                                    " WHERE (k013db_1acc.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k013db_1acc.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (k013db_1acc.txtacc_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY k013db_1acc.txtacc_id ASC";

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
                            GridView1.Rows[index].Cells["Col_txtacc_id"].Value = dt2.Rows[j]["txtacc_id"].ToString();      //1
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "1")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "2")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "     " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "     " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "3")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "          " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "          " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "4")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "               " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "               " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "5")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "                    " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "                    " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            GridView1.Rows[index].Cells["Col_txtacc_type_id"].Value = dt2.Rows[j]["txtacc_type_id"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtacc_type_name"].Value = dt2.Rows[j]["txtacc_type_name"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_id"].Value = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_name"].Value = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //8
                            GridView1.Rows[index].Cells["Col_txtacc_degree_name"].Value = dt2.Rows[j]["txtacc_degree_name"].ToString();      //9
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
        //===============
        DataTable table = new DataTable();
        int selectedRowIndex;
        private void PANEL_FORM1_dataGridView1_acc_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRowIndex = e.RowIndex;
                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[0].Name = "Col_Auto_num";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[1].Name = "Col_txtacc_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[2].Name = "Col_txtacc_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[3].Name = "Col_txtacc_name_eng";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[4].Name = "Col_txtacc_type_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[5].Name = "Col_txtacc_type_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[6].Name = "Col_txtacc_work_type_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[7].Name = "Col_txtacc_work_type_name";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[8].Name = "Col_txtacc_degree_id";
                    //this.PANEL_FORM1_dataGridView1_acc.Columns[9].Name = "Col_txtacc_degree_name";

                    this.txtacc_id.Text = row.Cells[1].Value.ToString();
                    //this.txtacc_name.Text = row.Cells[2].Value.ToString();
                    //this.txtacc_name_eng.Text = row.Cells[3].Value.ToString();

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

                        cmd2.CommandText = "SELECT k013db_1acc.*," +
                                          "k013db_2acc_type.*," +
                                           "k013db_3acc_work_type.*," +
                                            "k013db_4acc_degree.*" +
                                            //"k013db_5acc_balance_type.*" +
                                            " FROM k013db_1acc" +

                                            " INNER JOIN k013db_2acc_type" +
                                            " ON k013db_1acc.txtacc_type_id = k013db_2acc_type.txtacc_type_id" +
                                            //" AND k013db_1acc.lang_id = k013db_2acc_type.lang_id" +

                                            " INNER JOIN k013db_3acc_work_type" +
                                            " ON k013db_1acc.txtacc_work_type_id = k013db_3acc_work_type.txtacc_work_type_id" +
                                            //" AND k013db_1acc.lang_id = k013db_3acc_work_type.lang_id" +

                                            " INNER JOIN k013db_4acc_degree" +
                                            " ON k013db_1acc.txtacc_degree_id = k013db_4acc_degree.txtacc_degree_id" +
                                            //" AND k013db_1acc.lang_id = k013db_4acc_degree.lang_id" +

                                            //" INNER JOIN k013db_5acc_balance_type" +
                                            //" ON k013db_1acc.txtacc_balance_type_id = k013db_5acc_balance_type.txtacc_balance_type_id" +
                                            //" AND k013db_1acc.lang_id = k013db_5acc_balance_type.lang_id" +

                                            " WHERE (k013db_1acc.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (k013db_1acc.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (k013db_1acc.txtacc_id = '" + this.txtacc_id.Text.Trim() + "')" +
                                            " ORDER BY k013db_1acc.txtacc_id ASC";

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
                                    this.txtacc_id.Text = dt2.Rows[j]["txtacc_id"].ToString();      //1
                                    this.txtacc_name.Text = dt2.Rows[j]["txtacc_name"].ToString();      //2
                                    this.txtacc_name_eng.Text = dt2.Rows[j]["txtacc_name_eng"].ToString();      //3
                                    this.PANEL32_ACC_TYPE_txtacc_type_id.Text = dt2.Rows[j]["txtacc_type_id"].ToString();      //4
                                    this.PANEL32_ACC_TYPE_txtacc_type_name.Text = dt2.Rows[j]["txtacc_type_name"].ToString();      //5
                                    this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //6
                                    this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Text  = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //7
                                    this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text  = dt2.Rows[j]["txtacc_degree_id"].ToString();      //8
                                    this.PANEL34_ACC_DEGREE_txtacc_degree_name.Text = dt2.Rows[j]["txtacc_degree_name"].ToString();      //9
                                    this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text = dt2.Rows[j]["txtacc_balance_type_id"].ToString();      //10
                                    //this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Text = dt2.Rows[j]["txtacc_balance_type_name"].ToString();      //11
                                    this.PANEL36_ACC_CONTROL_txtacc_id.Text = dt2.Rows[j]["txtacc_id_control"].ToString();      //12
                                    this.PANEL36_ACC_CONTROL_txtacc_name.Text = dt2.Rows[j]["txtacc_name_control"].ToString();      //13
                                }
                                this.iblword_status.Text = "แก้ไขบัญชี";
                                this.txtacc_id.ReadOnly = true;
                                this.BtnCancel_Doc.Enabled = true;
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

                    //เชื่อมต่อฐานข้อมูล======================================================
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {

                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;

                        cmd2.CommandText = "SELECT *" +
                                            " FROM k013db_5acc_balance_type" +
                                            " WHERE (txtacc_balance_type_id = '" + this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text.Trim() + "')";

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
                                    //this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text = dt2.Rows[j]["txtacc_balance_type_id"].ToString();      //10
                                    this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Text = dt2.Rows[j]["txtacc_balance_type_name"].ToString();      //11
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
        //Acc_type =======================================================================
        private void PANEL32_ACC_TYPE_Fill_acc_type()
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

            PANEL32_ACC_TYPE_Clear_GridView1_acc_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_2acc_type" +
                                  " WHERE (txtacc_type_id <> '')" +
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
                            var index = PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows.Add();
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_txtacc_type_id"].Value = dt2.Rows[j]["txtacc_type_id"].ToString();      //1
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_txtacc_type_name"].Value = dt2.Rows[j]["txtacc_type_name"].ToString();      //2
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_txtacc_type_name_eng"].Value = dt2.Rows[j]["txtacc_type_name_eng"].ToString();      //3
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
        private void PANEL32_ACC_TYPE_GridView1_acc_type()
        {
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.ColumnCount = 4;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[1].Name = "Col_txtacc_type_id";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[2].Name = "Col_txtacc_type_name";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[3].Name = "Col_txtacc_type_name_eng";

            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[0].HeaderText = "No";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[1].HeaderText = "รหัส";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[2].HeaderText = " ประเภทบัญชี";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[3].HeaderText = " ประเภทบัญชี Eng";

            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[0].Visible = false;  //"No";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[1].Visible = true;  //"Col_txtacc_type_id";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[1].Width = 100;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[1].ReadOnly = true;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[2].Visible = true;  //"Col_txtacc_type_name";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[2].Width = 150;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[2].ReadOnly = true;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[3].Visible = true;  //"Col_txtacc_type_name_eng";
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[3].Width = 150;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[3].ReadOnly = true;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL32_ACC_TYPE_Clear_GridView1_acc_type()
        {
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows.Clear();
            this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Refresh();
        }
        private void PANEL32_ACC_TYPE_txtacc_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL32_ACC_TYPE.Visible == false)
                {
                    this.PANEL32_ACC_TYPE.Visible = true;
                    this.PANEL32_ACC_TYPE.Location = new Point(this.PANEL32_ACC_TYPE_txtacc_type_name.Location.X, this.PANEL32_ACC_TYPE_txtacc_type_name.Location.Y + 22);
                    this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Focus();
                }
                else
                {
                    this.PANEL32_ACC_TYPE.Visible = false;
                }
        }
        private void PANEL32_ACC_TYPE_btnacc_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL32_ACC_TYPE.Visible == false)
            {
                this.PANEL32_ACC_TYPE.Visible = true;
                this.PANEL32_ACC_TYPE.BringToFront();
                this.PANEL32_ACC_TYPE.Location = new Point(this.PANEL32_ACC_TYPE_txtacc_type_name.Location.X, this.PANEL32_ACC_TYPE_txtacc_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL32_ACC_TYPE.Visible = false;
            }
        }
        private void PANEL32_ACC_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL32_ACC_TYPE.Visible == false)
            {
                this.PANEL32_ACC_TYPE.Visible = true;
            }
            else
            {
                this.PANEL32_ACC_TYPE.Visible = false;
            }
        }
        private void PANEL32_ACC_TYPE_dataGridView1_acc_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL32_ACC_TYPE_txtacc_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL32_ACC_TYPE_txtacc_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL32_ACC_TYPE_dataGridView1_acc_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL32_ACC_TYPE_dataGridView1_acc_type.CurrentRow.Index;

                this.PANEL32_ACC_TYPE_txtacc_type_id.Text = PANEL32_ACC_TYPE_dataGridView1_acc_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL32_ACC_TYPE_txtacc_type_name.Text = PANEL32_ACC_TYPE_dataGridView1_acc_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL32_ACC_TYPE_txtacc_type_name.Focus();
                this.PANEL32_ACC_TYPE.Visible = false;
            }
        }
        private void PANEL32_ACC_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL32_ACC_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL32_ACC_TYPE_Clear_GridView1_acc_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_2acc_type" +
                                   " WHERE (txtacc_type_name LIKE '%" + this.PANEL32_ACC_TYPE_txtsearch.Text + "%')" +
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
                            var index = PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows.Add();
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_txtacc_type_id"].Value = dt2.Rows[j]["txtacc_type_id"].ToString();      //1
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_txtacc_type_name"].Value = dt2.Rows[j]["txtacc_type_name"].ToString();      //2
                            PANEL32_ACC_TYPE_dataGridView1_acc_type.Rows[index].Cells["Col_txtacc_type_name_eng"].Value = dt2.Rows[j]["txtacc_type_name_eng"].ToString();      //3
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
        private void PANEL32_ACC_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL32_ACC_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL32_ACC_TYPE.Height = PANEL32_ACC_TYPE_btnresize_low.Top + e.Y;
                this.PANEL32_ACC_TYPE.Width = PANEL32_ACC_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL32_ACC_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL32_ACC_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END Acc_type=======================================================================

        //acc_work_type =======================================================================
        private void PANEL33_ACC_WORK_TYPE_Fill_acc_work_type()
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

            PANEL33_ACC_WORK_TYPE_Clear_GridView1_acc_work_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_3acc_work_type" +
                                  " WHERE (txtacc_work_type_id <> '')" +
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
                            var index = PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows.Add();
                            PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[index].Cells["Col_txtacc_work_type_id"].Value = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //1
                            PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[index].Cells["Col_txtacc_work_type_name"].Value = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //2
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
        private void PANEL33_ACC_WORK_TYPE_GridView1_acc_work_type()
        {
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.ColumnCount = 3;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[1].Name = "Col_txtacc_work_type_id";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[2].Name = "Col_txtacc_work_type_name";

            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[0].HeaderText = "No";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[1].HeaderText = "รหัส";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[2].HeaderText = " ประเภทการทำงาน";

            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[0].Visible = false;  //"No";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[1].Visible = true;  //"Col_txtacc_work_type_id";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[1].Width = 100;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[1].ReadOnly = true;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[2].Visible = true;  //"Col_txtacc_work_type_name";
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[2].Width = 150;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[2].ReadOnly = true;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL33_ACC_WORK_TYPE_Clear_GridView1_acc_work_type()
        {
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows.Clear();
            this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Refresh();
        }
        private void PANEL33_ACC_WORK_TYPE_txtacc_work_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL33_ACC_WORK_TYPE.Visible == false)
                {
                    this.PANEL33_ACC_WORK_TYPE.Visible = true;
                    this.PANEL33_ACC_WORK_TYPE.Location = new Point(this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Location.X, this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Location.Y + 22);
                    this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Focus();
                }
                else
                {
                    this.PANEL33_ACC_WORK_TYPE.Visible = false;
                }
        }
        private void PANEL33_ACC_WORK_TYPE_btnacc_work_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL33_ACC_WORK_TYPE.Visible == false)
            {
                this.PANEL33_ACC_WORK_TYPE.Visible = true;
                this.PANEL33_ACC_WORK_TYPE.BringToFront();
                this.PANEL33_ACC_WORK_TYPE.Location = new Point(this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Location.X, this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL33_ACC_WORK_TYPE.Visible = false;
            }
        }
        private void PANEL33_ACC_WORK_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL33_ACC_WORK_TYPE.Visible == false)
            {
                this.PANEL33_ACC_WORK_TYPE.Visible = true;
            }
            else
            {
                this.PANEL33_ACC_WORK_TYPE.Visible = false;
            }
        }
        private void PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.CurrentRow.Index;

                this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_id.Text = PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Text = PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL33_ACC_WORK_TYPE_txtacc_work_type_name.Focus();
                this.PANEL33_ACC_WORK_TYPE.Visible = false;
            }
        }
        private void PANEL33_ACC_WORK_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL33_ACC_WORK_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL33_ACC_WORK_TYPE_Clear_GridView1_acc_work_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_3acc_work_type" +
                                   " WHERE (txtacc_work_type_name LIKE '%" + this.PANEL33_ACC_WORK_TYPE_txtsearch.Text + "%')" +
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
                            var index = PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows.Add();
                            PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[index].Cells["Col_txtacc_work_type_id"].Value = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //1
                            PANEL33_ACC_WORK_TYPE_dataGridView1_acc_work_type.Rows[index].Cells["Col_txtacc_work_type_name"].Value = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //2
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
        private void PANEL33_ACC_WORK_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL33_ACC_WORK_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL33_ACC_WORK_TYPE.Height = PANEL33_ACC_WORK_TYPE_btnresize_low.Top + e.Y;
                this.PANEL33_ACC_WORK_TYPE.Width = PANEL33_ACC_WORK_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL33_ACC_WORK_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL33_ACC_WORK_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END acc_work_type=======================================================================

        //acc_balance_type =======================================================================
        private void PANEL35_ACC_BALANCE_TYPE_Fill_acc_balance_type()
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

            PANEL35_ACC_BALANCE_TYPE_Clear_GridView1_acc_balance_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_5acc_balance_type" +
                                  " WHERE (txtacc_balance_type_id <> '')" +
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
                            var index = PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows.Add();
                            PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[index].Cells["Col_txtacc_balance_type_id"].Value = dt2.Rows[j]["txtacc_balance_type_id"].ToString();      //1
                            PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[index].Cells["Col_txtacc_balance_type_name"].Value = dt2.Rows[j]["txtacc_balance_type_name"].ToString();      //2
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
        private void PANEL35_ACC_BALANCE_TYPE_GridView1_acc_balance_type()
        {
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.ColumnCount = 3;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[1].Name = "Col_txtacc_balance_type_id";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[2].Name = "Col_txtacc_balance_type_name";

            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[0].HeaderText = "No";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[1].HeaderText = "รหัส";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[2].HeaderText = " ยอดคงเหลือปกติ";

            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[0].Visible = false;  //"No";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[1].Visible = true;  //"Col_txtacc_balance_type_id";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[1].Width = 100;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[1].ReadOnly = true;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[2].Visible = true;  //"Col_txtacc_balance_type_name";
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[2].Width = 150;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[2].ReadOnly = true;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL35_ACC_BALANCE_TYPE_Clear_GridView1_acc_balance_type()
        {
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows.Clear();
            this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Refresh();
        }
        private void PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL35_ACC_BALANCE_TYPE.Visible == false)
                {
                    this.PANEL35_ACC_BALANCE_TYPE.Visible = true;
                    this.PANEL35_ACC_BALANCE_TYPE.Location = new Point(this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Location.X, this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Location.Y + 22);
                    this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Focus();
                }
                else
                {
                    this.PANEL35_ACC_BALANCE_TYPE.Visible = false;
                }
        }
        private void PANEL35_ACC_BALANCE_TYPE_btnacc_balance_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL35_ACC_BALANCE_TYPE.Visible == false)
            {
                this.PANEL35_ACC_BALANCE_TYPE.Visible = true;
                this.PANEL35_ACC_BALANCE_TYPE.BringToFront();
                this.PANEL35_ACC_BALANCE_TYPE.Location = new Point(this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Location.X, this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL35_ACC_BALANCE_TYPE.Visible = false;
            }
        }
        private void PANEL35_ACC_BALANCE_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL35_ACC_BALANCE_TYPE.Visible == false)
            {
                this.PANEL35_ACC_BALANCE_TYPE.Visible = true;
            }
            else
            {
                this.PANEL35_ACC_BALANCE_TYPE.Visible = false;
            }
        }
        private void PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.CurrentRow.Index;

                this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_id.Text = PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Text = PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL35_ACC_BALANCE_TYPE_txtacc_balance_type_name.Focus();
                this.PANEL35_ACC_BALANCE_TYPE.Visible = false;
            }
        }
        private void PANEL35_ACC_BALANCE_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL35_ACC_BALANCE_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL35_ACC_BALANCE_TYPE_Clear_GridView1_acc_balance_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_5acc_balance_type" +
                                   " WHERE (txtacc_balance_type_name LIKE '%" + this.PANEL35_ACC_BALANCE_TYPE_txtsearch.Text + "%')" +
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
                            var index = PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows.Add();
                            PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[index].Cells["Col_txtacc_balance_type_id"].Value = dt2.Rows[j]["txtacc_balance_type_id"].ToString();      //1
                            PANEL35_ACC_BALANCE_TYPE_dataGridView1_acc_balance_type.Rows[index].Cells["Col_txtacc_balance_type_name"].Value = dt2.Rows[j]["txtacc_balance_type_name"].ToString();      //2
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
        private void PANEL35_ACC_BALANCE_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL35_ACC_BALANCE_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL35_ACC_BALANCE_TYPE.Height = PANEL35_ACC_BALANCE_TYPE_btnresize_low.Top + e.Y;
                this.PANEL35_ACC_BALANCE_TYPE.Width = PANEL35_ACC_BALANCE_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL35_ACC_BALANCE_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL35_ACC_BALANCE_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END acc_balance_type=======================================================================
        //acc_degree =======================================================================
        private void PANEL34_ACC_DEGREE_Fill_acc_degree()
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

            PANEL34_ACC_DEGREE_Clear_GridView1_acc_degree();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_4acc_degree" +
                                   " WHERE (txtacc_degree_id <> '')" +
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
                            var index = PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows.Add();
                            PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //1
                            PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[index].Cells["Col_txtacc_degree_name"].Value = dt2.Rows[j]["txtacc_degree_name"].ToString();      //2
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
        private void PANEL34_ACC_DEGREE_GridView1_acc_degree()
        {
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.ColumnCount = 3;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[0].Name = "Col_Auto_num";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[1].Name = "Col_txtacc_degree_id";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[2].Name = "Col_txtacc_degree_name";

            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[0].HeaderText = "No";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[1].HeaderText = "รหัส";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[2].HeaderText = " ระดับบัญชี";

            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[0].Visible = false;  //"No";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[1].Visible = true;  //"Col_txtacc_degree_id";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[1].Width = 100;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[1].ReadOnly = true;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[2].Visible = true;  //"Col_txtacc_degree_name";
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[2].Width = 150;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[2].ReadOnly = true;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.EnableHeadersVisualStyles = false;

        }
        private void PANEL34_ACC_DEGREE_Clear_GridView1_acc_degree()
        {
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows.Clear();
            this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Refresh();
        }
        private void PANEL34_ACC_DEGREE_txtacc_degree_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL34_ACC_DEGREE.Visible == false)
                {
                    this.PANEL34_ACC_DEGREE.Visible = true;
                    this.PANEL34_ACC_DEGREE.Location = new Point(this.PANEL34_ACC_DEGREE_txtacc_degree_name.Location.X, this.PANEL34_ACC_DEGREE_txtacc_degree_name.Location.Y + 22);
                    this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Focus();
                }
                else
                {
                    this.PANEL34_ACC_DEGREE.Visible = false;
                }
        }
        private void PANEL34_ACC_DEGREE_btnacc_degree_Click(object sender, EventArgs e)
        {
            if (this.PANEL34_ACC_DEGREE.Visible == false)
            {
                this.PANEL34_ACC_DEGREE.Visible = true;
                this.PANEL34_ACC_DEGREE.BringToFront();
                this.PANEL34_ACC_DEGREE.Location = new Point(this.PANEL34_ACC_DEGREE_txtacc_degree_name.Location.X, this.PANEL34_ACC_DEGREE_txtacc_degree_name.Location.Y + 22);
            }
            else
            {
                this.PANEL34_ACC_DEGREE.Visible = false;
            }
        }
        private void PANEL34_ACC_DEGREE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL34_ACC_DEGREE.Visible == false)
            {
                this.PANEL34_ACC_DEGREE.Visible = true;
            }
            else
            {
                this.PANEL34_ACC_DEGREE.Visible = false;
            }
        }
        private void PANEL34_ACC_DEGREE_dataGridView1_acc_degree_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL34_ACC_DEGREE_txtacc_degree_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL34_ACC_DEGREE_dataGridView1_acc_degree_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL34_ACC_DEGREE_dataGridView1_acc_degree.CurrentRow.Index;

                this.PANEL34_ACC_DEGREE_txtacc_degree_id.Text = PANEL34_ACC_DEGREE_dataGridView1_acc_degree.CurrentRow.Cells[1].Value.ToString();
                this.PANEL34_ACC_DEGREE_txtacc_degree_name.Text = PANEL34_ACC_DEGREE_dataGridView1_acc_degree.CurrentRow.Cells[2].Value.ToString();
                this.PANEL34_ACC_DEGREE_txtacc_degree_name.Focus();
                this.PANEL34_ACC_DEGREE.Visible = false;
            }
        }
        private void PANEL34_ACC_DEGREE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL34_ACC_DEGREE_btn_search_Click(object sender, EventArgs e)
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

            PANEL34_ACC_DEGREE_Clear_GridView1_acc_degree();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013db_4acc_degree" +
                                   " WHERE (txtacc_degree_name LIKE '%" + this.PANEL34_ACC_DEGREE_txtsearch.Text + "%')" +
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
                            var index = PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows.Add();
                            PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //1
                            PANEL34_ACC_DEGREE_dataGridView1_acc_degree.Rows[index].Cells["Col_txtacc_degree_name"].Value = dt2.Rows[j]["txtacc_degree_name"].ToString();      //2
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
        private void PANEL34_ACC_DEGREE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL34_ACC_DEGREE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL34_ACC_DEGREE.Height = PANEL34_ACC_DEGREE_btnresize_low.Top + e.Y;
                this.PANEL34_ACC_DEGREE.Width = PANEL34_ACC_DEGREE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL34_ACC_DEGREE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL34_ACC_DEGREE_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END acc_degree=======================================================================
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
                                   " WHERE (txtacc_id <> '')" +
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
                    this.PANEL36_ACC_CONTROL.Location = new Point(this.PANEL36_ACC_CONTROL_txtacc_name.Location.X, this.PANEL36_ACC_CONTROL_txtacc_name.Location.Y + 22);
                    this.PANEL36_ACC_CONTROL_dataGridView1_acc_control.Focus();
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
                this.PANEL36_ACC_CONTROL.Visible = true;
                this.PANEL36_ACC_CONTROL.BringToFront();
                this.PANEL36_ACC_CONTROL.Location = new Point(this.PANEL36_ACC_CONTROL_txtacc_name.Location.X, this.PANEL36_ACC_CONTROL_txtacc_name.Location.Y + 22);
            }
            else
            {
                this.PANEL36_ACC_CONTROL.Visible = false;
            }
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
        //END Acc_control =======================================================================

        private void txtacc_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"[^0-9^+^\-^\/^\*^\(^\)]"))
            //{
            //    // Stop the character from being entered into the control since it is illegal.
            //    e.Handled = true;
            //}
            if (!char.IsControl(e.KeyChar)
                   && !char.IsDigit(e.KeyChar)
                   && e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
                   && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
                   && e.KeyChar != '/')
                            {
                                e.Handled = true;
                                return;
            }


            if (e.KeyChar == (char)Keys.Enter && this.txtacc_id.Text == "")
            {
                this.txtacc_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtacc_id.Text.Trim() != "")
            {
                if (this.txtacc_id.TextLength == 7)
                {
                    this.txtacc_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่รหัสบัญชี  7 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtacc_id.Focus();
                    return;
                }
            }
         }
        private void txtacc_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtacc_name_eng.Focus();

        }
        private void txtacc_name_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.PANEL32_ACC_TYPE_txtacc_type_name.Focus();

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

                cmd1.CommandText = "SELECT k013db_1acc.*," +
                                  "k013db_2acc_type.*," +
                                   "k013db_3acc_work_type.*," +
                                    "k013db_4acc_degree.*" +
                                    " FROM k013db_1acc" +

                                    " INNER JOIN k013db_2acc_type" +
                                    " ON k013db_1acc.txtacc_type_id = k013db_2acc_type.txtacc_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_2acc_type.lang_id" +

                                    " INNER JOIN k013db_3acc_work_type" +
                                    " ON k013db_1acc.txtacc_work_type_id = k013db_3acc_work_type.txtacc_work_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_3acc_work_type.lang_id" +

                                    " INNER JOIN k013db_4acc_degree" +
                                    " ON k013db_1acc.txtacc_degree_id = k013db_4acc_degree.txtacc_degree_id" +
                                    //" AND k013db_1acc.lang_id = k013db_4acc_degree.lang_id" +

                                    " WHERE (k013db_1acc.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k013db_1acc.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (k013db_1acc.txtacc_id = '')" +
                                    " ORDER BY k013db_1acc.txtacc_id ASC";
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
                        cmd2.CommandText = "INSERT INTO k013db_1acc(cdkey,txtco_id,txtbranch_id," +
                                           "txtacc_id,txtacc_name,txtacc_name_eng," +
                                           "txtacc_type_id,txtacc_work_type_id," +
                                           "txtacc_balance_type_id,txtacc_degree_id," +
                                           "txtacc_id_control,txtacc_name_control," +
                                           "txttype_money_id,lang_id) " +
                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +
                                           "@txtacc_id,@txtacc_name,@txtacc_name_eng," +
                                           "@txtacc_type_id,@txtacc_work_type_id," +
                                           "@txtacc_balance_type_id,@txtacc_degree_id," +
                                           "@txtacc_id_control,@txtacc_name_control,@txttype_money_id,@lang_id)";

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();
                        cmd2.Parameters.Add("@txtacc_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_name", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_name_eng", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_type_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_work_type_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_balance_type_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_degree_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_id_control", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtacc_name_control", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txttype_money_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@lang_id", SqlDbType.NVarChar).Value = W_ID_Select.Lang.Trim();
                        //==============================

                        cmd2.ExecuteNonQuery();


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
                                this.PANEL_FORM1.Visible = false;
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

                        this.PANEL_FORM1.Visible = false;
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
                this.PANEL_FORM1.Visible = true;
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
                this.PANEL_FORM1.Visible = true;
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
