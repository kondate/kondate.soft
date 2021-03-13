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

namespace kondate.soft.HOME12_license
{
    public partial class HOME12_Set_license_01_user : Form
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



        public HOME12_Set_license_01_user()
        {
            InitializeComponent();
        }

        private void HOME12_Set_license_01_Load(object sender, EventArgs e)
        {

            //W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            //W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            //W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.M_FORM_NUMBER = "1201";
            CHECK_ADD_FORM();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มผู้ใช้ใหม่";
            this.txtemp_id.ReadOnly = false;
            this.txtuser_name.ReadOnly = false;
            this.label9.Visible = true;
            this.label10.Visible = true;

            this.ActiveControl = this.txtemp_id;

            PANEL37_USER_TYPE_GridView1_user_type();
            PANEL37_USER_TYPE_Fill_user_type();


            PANEL_FORM1_GridView1();
            PANEL_FORM1_Fill_GridView1();

        }
        private void PANEL_FORM1_Fill_GridView1()
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
            PANEL_FORM1_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT a003db_user.*," +
                                    "a003db_user_type.*" +
                                    " FROM a003db_user" +
                                    " INNER JOIN a003db_user_type" +
                                    " ON a003db_user_type.txtuser_type_id = a003db_user.txtuser_type_id" +
                                    " WHERE (a003db_user.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " ORDER BY a003db_user.txtemp_id ASC";

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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtemp_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtname";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtname_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtuser_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtco_tel";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_user_status";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                             //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtempid = dt2.Rows[j]["txtemp_id"].ToString();      //2
                            string cipherText_txtempid = W_CryptorEngine.Decrypt(clearText_txtempid, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtempid.ToString();     //1
                           //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname = dt2.Rows[j]["txtname"].ToString();      //2
                            string cipherText_txtname = W_CryptorEngine.Decrypt(clearText_txtname, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtname"].Value = cipherText_txtname.ToString();      //2
                             //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname_eng = dt2.Rows[j]["txtname_eng"].ToString();      //2
                            string cipherText_txtname_eng = W_CryptorEngine.Decrypt(clearText_txtname_eng, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtname_eng"].Value = cipherText_txtname_eng.ToString();      //3

                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_type_name"].Value = dt2.Rows[j]["txtuser_type_name"].ToString();      //4

                            //ใส่รหัสฐานข้อมูล user============================================
                            string clearText_txtuser_name = dt2.Rows[j]["txtuser_name"].ToString();      //6
                            string cipherText_txtuser_name = W_CryptorEngine.Decrypt(clearText_txtuser_name, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = cipherText_txtuser_name.ToString();      //5

                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_user_status"].Value = dt2.Rows[j]["user_status"].ToString();      //6

                        }

                        PANEL_FORM1_Clear_GridView1_Up_Status();

                        Cursor.Current = Cursors.Default;



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
        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_FORM1_Fill_GridView1();
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
            Cursor.Current = Cursors.WaitCursor;
            PANEL_FORM1_Clear_GridView1();

            string clearText_txtuser_name =this.PANEL_FORM1_txtsearch.Text.ToString();      //2
            string cipherText_txtuser_name = W_CryptorEngine.Encrypt(clearText_txtuser_name, true);

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT a003db_user.*," +
                                    "a003db_user_type.*" +
                                    " FROM a003db_user" +
                                    " INNER JOIN a003db_user_type" +
                                    " ON a003db_user_type.txtuser_type_id = a003db_user.txtuser_type_id" +
                                    " WHERE (a003db_user.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (a003db_user.txtname LIKE '%" + cipherText_txtuser_name.ToString() + "%')" +
                                    " ORDER BY a003db_user.txtemp_id ASC";

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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtemp_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtname";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtname_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtuser_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtco_tel";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_user_status";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                             //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtempid = dt2.Rows[j]["txtemp_id"].ToString();      //2
                            string cipherText_txtempid = W_CryptorEngine.Decrypt(clearText_txtempid, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtempid.ToString();     //1
                             //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname = dt2.Rows[j]["txtname"].ToString();      //2
                            string cipherText_txtname = W_CryptorEngine.Decrypt(clearText_txtname, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtname"].Value = cipherText_txtname.ToString();      //2
                            //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname_eng = dt2.Rows[j]["txtname_eng"].ToString();      //2
                            string cipherText_txtname_eng = W_CryptorEngine.Decrypt(clearText_txtname_eng, true);
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtname_eng"].Value = cipherText_txtname_eng.ToString();      //3

                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_type_name"].Value = dt2.Rows[j]["txtuser_type_name"].ToString();      //4

                            //ใส่รหัสฐานข้อมูล user============================================
                            string clearText_txtusername = dt2.Rows[j]["txtuser_name"].ToString();      //6
                            string cipherText_txtusername = W_CryptorEngine.Decrypt(clearText_txtusername, true);
                            //=======================================================
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = cipherText_txtusername.ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_user_status"].Value = dt2.Rows[j]["user_status"].ToString();      //6

                        }
                        Cursor.Current = Cursors.Default;
                        PANEL_FORM1_Clear_GridView1_Up_Status();





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
        private void PANEL_FORM1_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL_FORM1_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_FORM1_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {

                    this.txtemp_id.Text = row.Cells[1].Value.ToString();
                    this.txtname.Text = row.Cells[2].Value.ToString();
                    this.txtname_eng.Text = row.Cells[3].Value.ToString();

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
                    string clearText_txtempid = this.txtemp_id.Text.ToString();      //2
                    string cipherText_txtempid = W_CryptorEngine.Encrypt(clearText_txtempid, true);

                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {

                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;

                        cmd2.CommandText = "SELECT a003db_user.*," +
                                            "a003db_user_type.*" +
                                            " FROM a003db_user" +
                                            " INNER JOIN a003db_user_type" +
                                            " ON a003db_user_type.txtuser_type_id = a003db_user.txtuser_type_id" +
                                            " WHERE (a003db_user.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (a003db_user.txtemp_id = '" + cipherText_txtempid.Trim() + "')" +
                                            " ORDER BY a003db_user.txtemp_id ASC";

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
                                    //ใส่รหัสฐานข้อมูล name============================================
                                    string clearText_txtempid2 = dt2.Rows[j]["txtemp_id"].ToString();      //2
                                    string cipherText_txtempid2 = W_CryptorEngine.Decrypt(clearText_txtempid2, true);
                                    this.txtemp_id.Text = cipherText_txtempid2.ToString();

                                    //ใส่รหัสฐานข้อมูล name============================================
                                    string clearText_txtname = dt2.Rows[j]["txtname"].ToString();      //2
                                    string cipherText_txtname = W_CryptorEngine.Decrypt(clearText_txtname, true);
                                    this.txtname.Text = cipherText_txtname.ToString();
                                    //=======================================================

                                    //ใส่รหัสฐานข้อมูล name============================================
                                    string clearText_txtname_eng = dt2.Rows[j]["txtname_eng"].ToString();      //2
                                    string cipherText_txtname_eng = W_CryptorEngine.Decrypt(clearText_txtname_eng, true);
                                    this.txtname_eng.Text = cipherText_txtname_eng.ToString();
                                    //=======================================================

                                    this.PANEL37_USER_TYPE_txtuser_type_id.Text = dt2.Rows[j]["txtuser_type_id"].ToString();      //4
                                    this.PANEL37_USER_TYPE_txtuser_type_name.Text = dt2.Rows[j]["txtuser_type_name"].ToString();      //5

                                    //ใส่รหัสฐานข้อมูล user============================================
                                    string clearText_txtuser_name = dt2.Rows[j]["txtuser_name"].ToString();      //6
                                    string cipherText_txtuser_name = W_CryptorEngine.Decrypt(clearText_txtuser_name, true);
                                    this.txtuser_name.Text = cipherText_txtuser_name.ToString();
                                    //=======================================================

                                }
                                this.iblword_status.Text = "แก้ไขผู้ใช้";
                                this.BtnCancel_Doc.Enabled = true;
                                this.txtuser_name.Visible = true;
                                this.txtuser_pass.Visible = false;
                                this.txtuser_pass2.Visible = false;
                                this.label9.Visible = false;
                                this.label10.Visible = false;
                                this.txtemp_id.ReadOnly = true;
                                this.txtuser_name.ReadOnly = true;
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
        private void PANEL_FORM1_GridView1()
        {
            this.PANEL_FORM1_dataGridView1.ColumnCount = 7;
            this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtemp_id";
            this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtname";
            this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtname_eng";
            this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtuser_type_name";
            this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtuser_name";
            this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_user_status";

            this.PANEL_FORM1_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderText = "รหัสพนักงาน";
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderText = " ชื่อ - สกุล";
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderText = " ชื่อ - สกุล  Eng";
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderText = "ประเภทผู้ใช้";
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderText = " User Name";
            this.PANEL_FORM1_dataGridView1.Columns[6].HeaderText = " สถานะ";

            this.PANEL_FORM1_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL_FORM1_dataGridView1.Columns[1].Visible = true;  //"Col_txtemp_id";
            this.PANEL_FORM1_dataGridView1.Columns[1].Width = 150;
            this.PANEL_FORM1_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[2].Visible = true;  //"Col_txtname";
            this.PANEL_FORM1_dataGridView1.Columns[2].Width = 200;
            this.PANEL_FORM1_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[3].Visible = true;  //"Col_txtname_eng";
            this.PANEL_FORM1_dataGridView1.Columns[3].Width = 200;
            this.PANEL_FORM1_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[4].Visible = true;  //"Col_txtuser_type_name";
            this.PANEL_FORM1_dataGridView1.Columns[4].Width = 150;
            this.PANEL_FORM1_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[5].Visible = true;  //"Col_txtuser_name";
            this.PANEL_FORM1_dataGridView1.Columns[5].Width = 150;
            this.PANEL_FORM1_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[6].Visible = false;  //"Col_user_status";
            this.PANEL_FORM1_dataGridView1.Columns[6].Width = 0;
            this.PANEL_FORM1_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_FORM1_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_FORM1_dataGridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL_FORM1_dataGridView1.Columns.Add(dgvCmb);

        }
        private void PANEL_FORM1_Clear_GridView1()
        {
            this.PANEL_FORM1_dataGridView1.Rows.Clear();
            this.PANEL_FORM1_dataGridView1.Refresh();
        }

        private void PANEL_FORM1_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL_FORM1_dataGridView1.Rows.Count; i++)
            {
                if (this.PANEL_FORM1_dataGridView1.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL_FORM1_dataGridView1.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL_FORM1_dataGridView1.Rows[i].Cells[7].Value = false;

                }
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
            var frm2 = new HOME12_Set_license_01_user();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มผู้ใช้ใหม่";
            this.txtuser_pass.Visible = true;
            this.txtuser_pass2.Visible = true;
            this.label9.Visible = true;
            this.label10.Visible = true;
            this.txtemp_id.ReadOnly = false;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "Y")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิดแก้ไข";
            TRANS_LOG();

            if (this.txtemp_id.Text != "")
            {
                this.iblword_status.Text = "แก้ไขผู้ใช้";
                this.txtemp_id.ReadOnly = true;
                this.txtuser_name.ReadOnly = true;
                this.label9.Visible = true;
                this.label10.Visible = true;

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (this.txtuser_pass.Text.Trim() == this.txtuser_pass2.Text.Trim())
            {
                this.BtnSave.Focus();
            }
            else
            {
                MessageBox.Show("รหัสผ่าน ที่ยืนยัน ไม่ตรงกัน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtuser_pass2.Focus();
                return;
            }

            if (this.txtemp_id.Text == "")
            {
                MessageBox.Show("โปรดใส่ รหัสพนักงาน ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtemp_id.Focus();
                return;
            }
            if (this.txtname.Text == "")
            {
                MessageBox.Show("โปรดใส่ ชื่อพนักงาน ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtname.Focus();
                return;
            }

            if (this.PANEL37_USER_TYPE_txtuser_type_id.Text == "")
            {
                MessageBox.Show("โปรดใส่ ประเภทผู้ใช้งาน ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL37_USER_TYPE_txtuser_type_id.Focus();
                return;
            }
            if (this.txtuser_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ User Name ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtuser_name.Focus();
                return;
            }

            if (this.iblword_status.Text.Trim() == "เพิ่มผู้ใช้ใหม่")
            {
                if (this.txtuser_pass.Text == "")
                {
                    MessageBox.Show("โปรดใส่ User Pass ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtuser_pass.Focus();
                    return;
                }
                if (this.txtuser_pass2.Text == "")
                {
                    MessageBox.Show("โปรดใส่ ยืนยัน User Pass ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtuser_pass2.Focus();
                    return;
                }
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
            if (this.iblword_status.Text.Trim() == "เพิ่มผู้ใช้ใหม่")
            {
                Cursor.Current = Cursors.WaitCursor;
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM a003db_user" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (txtemp_id = '" + this.txtemp_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("รหัสพนักงาน นี้ซ้ำ   : '" + this.txtemp_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtemp_id.Focus();
                        conn.Close();
                        return;
                    }


                    cmd1.CommandText = "SELECT * FROM a003db_user" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (txtuser_name = '" + this.txtuser_name.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd1);
                    da2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("User Name นี้ซ้ำ   : '" + this.txtuser_name.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtuser_name.Focus();
                        conn.Close();
                        return;
                    }


                }

                //
                conn.Close();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            //ใส่รหัสฐานข้อมูล============================================
            string txtempid;
            //ใส่รหัสฐานข้อมูล user============================================
            string clearText_empid = this.txtemp_id.Text.Trim();
            string cipherText_empid = W_CryptorEngine.Encrypt(clearText_empid, true);
            txtempid = cipherText_empid.ToString();
            //=======================================================

            //ใส่รหัสฐานข้อมูล============================================
            string txtname;
            //ใส่รหัสฐานข้อมูล user============================================
            string clearText_name = this.txtname.Text.Trim();
            string cipherText_txtname = W_CryptorEngine.Encrypt(clearText_name, true);
            txtname = cipherText_txtname.ToString();
            //=======================================================
            string txtname_eng;
            //ใส่รหัสฐานข้อมูล user============================================
            string clearText_name_eng = this.txtname_eng.Text.Trim();
            string cipherText_txtname_eng = W_CryptorEngine.Encrypt(clearText_name_eng, true);
            txtname_eng = cipherText_txtname_eng.ToString();
            //=======================================================
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มผู้ใช้ใหม่")
                    {
                        cmd2.CommandText = "INSERT INTO a003db_user(cdkey,txtco_id," +  //1
                                           "txtbranch_id," +  //2
                                           "txtemp_id,txtname," +  //3
                                           "txtname_eng," +  //4
                                            "txtuser_type_id," +  //5
                                          "txtuser_name,txtuser_pass,user_status) " +  //6
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtbranch_id," +  //2
                                           "@txtemp_id,@txtname," +  //3
                                           "@txtname_eng," +  //4
                                           "@txtuser_type_id," +  //5
                                           "@txtuser_name,@txtuser_pass,@user_status)";   //8

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();
                        cmd2.Parameters.Add("@txtemp_id", SqlDbType.NVarChar).Value = cipherText_empid.ToString(); 
                        cmd2.Parameters.Add("@txtname", SqlDbType.NVarChar).Value = cipherText_txtname.ToString();
                        cmd2.Parameters.Add("@txtname_eng", SqlDbType.NVarChar).Value = cipherText_txtname_eng.ToString();
                        cmd2.Parameters.Add("@txtuser_type_id", SqlDbType.NVarChar).Value = this.PANEL37_USER_TYPE_txtuser_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = txtusername.ToString();
                        cmd2.Parameters.Add("@txtuser_pass", SqlDbType.NVarChar).Value = txtuserpass.ToString();
                        cmd2.Parameters.Add("@user_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;
                    if (this.iblword_status.Text.Trim() == "แก้ไขผู้ใช้")
                    {
                        cmd2.CommandText = "UPDATE a003db_user SET " +
                                                                     "txtname = '" + cipherText_txtname.Trim() + "'," +
                                                                     "txtname_eng = '" + cipherText_txtname_eng.Trim() + "'," +
                                                                     "txtuser_type_id = '" + this.PANEL37_USER_TYPE_txtuser_type_id.Text.Trim() + "'" +
                                                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                     " AND (txtemp_id = '" + cipherText_empid.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        trans.Commit();
                        conn.Close();
                        Cursor.Current = Cursors.Default;
                        if (this.iblword_status.Text.Trim() == "เพิ่มผู้ใช้ใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขผู้ใช้")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtemp_id.Text = "";
                        this.txtname.Text = "";
                        this.txtname_eng.Text = "";
                        this.txtuser_name.Text = "";
                        this.txtuser_pass.Text = "";
                        this.txtuser_pass2.Text = "";
                        PANEL_FORM1_Fill_GridView1();

                        this.iblword_status.Text = "เพิ่มผู้ใช้ใหม่";
                        this.txtuser_pass.Visible = true;
                        this.txtuser_pass2.Visible = true;
                        this.label9.Visible = true;
                        this.label10.Visible = true;
                        this.txtemp_id.ReadOnly = false;
                        this.txtuser_name.ReadOnly = false;
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtemp_id_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == (char)Keys.Enter && this.txtemp_id.Text == "")
            {
                this.txtemp_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtemp_id.Text.Trim() != "")
            {
                this.txtname.Focus();

            }


        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtname_eng.Focus();

        }

        private void txtname_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.PANEL37_USER_TYPE_txtuser_type_id.Focus();

        }

        private void txtuser_name_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == (char)Keys.Enter && this.txtuser_name.Text == "")
            {
                this.txtuser_name.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtuser_name.Text.Trim() != "")
            {
                this.txtuser_pass.Focus();

            }

        }

        private void txtuser_pass_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == (char)Keys.Enter && this.txtuser_pass.Text == "")
            {
                this.txtuser_pass.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtuser_pass.Text.Trim() != "")
            {
                this.txtuser_pass2.Focus();

            }
        }

        private void txtuser_pass2_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == (char)Keys.Enter && this.txtuser_pass2.Text == "")
            {
                this.txtuser_pass2.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtuser_pass2.Text.Trim() != "")
            {
                if (this.txtuser_pass.Text.Trim() == this.txtuser_pass2.Text.Trim())
                {
                    this.BtnSave.Focus();
                }
                else
                {
                    MessageBox.Show("รหัสผ่าน ที่ยืนยัน ไม่ตรงกัน !!  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtuser_pass2.Focus();
                    return;
                }

            }

        }




        //user_type =======================================================================
        private void PANEL37_USER_TYPE_Fill_user_type()
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

            PANEL37_USER_TYPE_Clear_GridView1_user_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM a003db_user_type" +
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
                            var index = PANEL37_USER_TYPE_dataGridView1_user_type.Rows.Add();
                            PANEL37_USER_TYPE_dataGridView1_user_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL37_USER_TYPE_dataGridView1_user_type.Rows[index].Cells["Col_txtuser_type_id"].Value = dt2.Rows[j]["txtuser_type_id"].ToString();      //1
                            PANEL37_USER_TYPE_dataGridView1_user_type.Rows[index].Cells["Col_txtuser_type_name"].Value = dt2.Rows[j]["txtuser_type_name"].ToString();      //2
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
        private void PANEL37_USER_TYPE_GridView1_user_type()
        {
            this.PANEL37_USER_TYPE_dataGridView1_user_type.ColumnCount = 3;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].Name = "Col_txtuser_type_id";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].Name = "Col_txtuser_type_name";

            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[0].HeaderText = "No";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].HeaderText = "รหัส";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].HeaderText = " ประเภทผู้ใช้";

            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[0].Visible = false;  //"No";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].Visible = true;  //"Col_txtuser_type_id";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].Width = 100;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].ReadOnly = true;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].Visible = true;  //"Col_txtuser_type_name";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].Width = 150;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].ReadOnly = true;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL37_USER_TYPE_dataGridView1_user_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL37_USER_TYPE_dataGridView1_user_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL37_USER_TYPE_dataGridView1_user_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL37_USER_TYPE_dataGridView1_user_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL37_USER_TYPE_dataGridView1_user_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL37_USER_TYPE_Clear_GridView1_user_type()
        {
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Rows.Clear();
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Refresh();
        }
        private void PANEL37_USER_TYPE_txtuser_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL37_USER_TYPE.Visible == false)
                {
                    this.PANEL37_USER_TYPE.Visible = true;
                    this.PANEL37_USER_TYPE.Location = new Point(this.PANEL37_USER_TYPE_txtuser_type_name.Location.X, this.PANEL37_USER_TYPE_txtuser_type_name.Location.Y + 22);
                    this.PANEL37_USER_TYPE_dataGridView1_user_type.Focus();
                }
                else
                {
                    this.PANEL37_USER_TYPE.Visible = false;
                }
        }
        private void PANEL37_USER_TYPE_btnuser_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL37_USER_TYPE.Visible == false)
            {
                this.PANEL37_USER_TYPE.Visible = true;
                this.PANEL37_USER_TYPE.BringToFront();
                this.PANEL37_USER_TYPE.Location = new Point(this.PANEL37_USER_TYPE_txtuser_type_name.Location.X, this.PANEL37_USER_TYPE_txtuser_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL37_USER_TYPE.Visible = false;
            }
        }
        private void PANEL37_USER_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL37_USER_TYPE.Visible == false)
            {
                this.PANEL37_USER_TYPE.Visible = true;
            }
            else
            {
                this.PANEL37_USER_TYPE.Visible = false;
            }
        }
        private void PANEL37_USER_TYPE_dataGridView1_user_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL37_USER_TYPE_dataGridView1_user_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL37_USER_TYPE_txtuser_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL37_USER_TYPE_txtuser_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL37_USER_TYPE_dataGridView1_user_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL37_USER_TYPE_dataGridView1_user_type.CurrentRow.Index;

                this.PANEL37_USER_TYPE_txtuser_type_id.Text = PANEL37_USER_TYPE_dataGridView1_user_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL37_USER_TYPE_txtuser_type_name.Text = PANEL37_USER_TYPE_dataGridView1_user_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL37_USER_TYPE_txtuser_type_name.Focus();
                this.PANEL37_USER_TYPE.Visible = false;
            }
        }
        private void PANEL37_USER_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL37_USER_TYPE_Clear_GridView1_user_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM a003db_user_type" +
                                   " WHERE (txtuser_type_name LIKE '%" + this.PANEL37_USER_TYPE_txtsearch.Text + "%')" +
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
                            var index = PANEL37_USER_TYPE_dataGridView1_user_type.Rows.Add();
                            PANEL37_USER_TYPE_dataGridView1_user_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL37_USER_TYPE_dataGridView1_user_type.Rows[index].Cells["Col_txtuser_type_id"].Value = dt2.Rows[j]["txtuser_type_id"].ToString();      //1
                            PANEL37_USER_TYPE_dataGridView1_user_type.Rows[index].Cells["Col_txtuser_type_name"].Value = dt2.Rows[j]["txtuser_type_name"].ToString();      //2
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
        private void PANEL37_USER_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL37_USER_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL37_USER_TYPE.Height = PANEL37_USER_TYPE_btnresize_low.Top + e.Y;
                this.PANEL37_USER_TYPE.Width = PANEL37_USER_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL37_USER_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL37_USER_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END user_type=======================================================================
        //====================================================================================

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
            Cursor.Current = Cursors.WaitCursor;
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
                        Cursor.Current = Cursors.Default;
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

        //Tans_Log ====================================================================

    }
}
