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
    public partial class HOME12_Set_license_04_user_role : Form
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



        public HOME12_Set_license_04_user_role()
        {
            InitializeComponent();
        }

        private void HOME12_Set_license_04_Load(object sender, EventArgs e)
        {
            //W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            //W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            //W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.M_FORM_NUMBER = "1204";
            CHECK_ADD_FORM();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.txtuser_name.Text = W_ID_Select.M_USERNAME.Trim();
            Fill_USER();

            PANEL_FORM9_GridView1();
            PANEL_FORM9_Fill_GridView1();

            Show_GridView1();
            Fill_GridView1();
            UP_GridView1();

            Show_GridView2();
            Fill_GridView2();
            UP_GridView2();
        }

        private void Fill_USER()
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
            string clearText_txtusername = this.txtuser_name.Text.ToString();      //2
            string cipherText_txtusername = W_CryptorEngine.Encrypt(clearText_txtusername, true);
            Cursor.Current = Cursors.WaitCursor;

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
                                    " AND (a003db_user.txtuser_name = '" + cipherText_txtusername.Trim() + "')" +
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
                        this.txtuser_name.ReadOnly = true;
                        this.txtemp_id.ReadOnly = true;
                        Cursor.Current = Cursors.Default;
                        //=======================================================
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
                    Cursor.Current = Cursors.Default;

                    conn.Close();
                }

                //===========================================
            }
            //================================
        }

        //User ===============================================================================
        private void PANEL_FORM9_Fill_GridView1()
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

            PANEL_FORM9_Clear_GridView1();


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
                            //this.PANEL_FORM9_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM9_dataGridView1.Columns[1].Name = "Col_txtemp_id";
                            //this.PANEL_FORM9_dataGridView1.Columns[2].Name = "Col_txtname";
                            //this.PANEL_FORM9_dataGridView1.Columns[3].Name = "Col_txtname_eng";
                            //this.PANEL_FORM9_dataGridView1.Columns[4].Name = "Col_txtuser_type_name";
                            //this.PANEL_FORM9_dataGridView1.Columns[5].Name = "Col_txtco_tel";
                            //this.PANEL_FORM9_dataGridView1.Columns[6].Name = "Col_user_status";

                            var index = PANEL_FORM9_dataGridView1.Rows.Add();
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                                                                                                    //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtempid = dt2.Rows[j]["txtemp_id"].ToString();      //2
                            string cipherText_txtempid = W_CryptorEngine.Decrypt(clearText_txtempid, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtempid.ToString();     //1
                                                                                                                                     //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname = dt2.Rows[j]["txtname"].ToString();      //2
                            string cipherText_txtname = W_CryptorEngine.Decrypt(clearText_txtname, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtname"].Value = cipherText_txtname.ToString();      //2
                                                                                                                                   //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname_eng = dt2.Rows[j]["txtname_eng"].ToString();      //2
                            string cipherText_txtname_eng = W_CryptorEngine.Decrypt(clearText_txtname_eng, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtname_eng"].Value = cipherText_txtname_eng.ToString();      //3

                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtuser_type_name"].Value = dt2.Rows[j]["txtuser_type_name"].ToString();      //4

                            //ใส่รหัสฐานข้อมูล user============================================
                            string clearText_txtuser_name = dt2.Rows[j]["txtuser_name"].ToString();      //6
                            string cipherText_txtuser_name = W_CryptorEngine.Decrypt(clearText_txtuser_name, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = cipherText_txtuser_name.ToString();      //5

                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_user_status"].Value = dt2.Rows[j]["user_status"].ToString();      //6

                        }

                        PANEL_FORM9_Clear_GridView1_Up_Status();
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
        private void PANEL_FORM9_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_FORM9_Fill_GridView1();
        }
        private void PANEL_FORM9_btnsearch_Click(object sender, EventArgs e)
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

            PANEL_FORM9_Clear_GridView1();

            string clearText_txtuser_name = this.PANEL_FORM9_txtsearch.Text.ToString();      //2
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
                            //this.PANEL_FORM9_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM9_dataGridView1.Columns[1].Name = "Col_txtemp_id";
                            //this.PANEL_FORM9_dataGridView1.Columns[2].Name = "Col_txtname";
                            //this.PANEL_FORM9_dataGridView1.Columns[3].Name = "Col_txtname_eng";
                            //this.PANEL_FORM9_dataGridView1.Columns[4].Name = "Col_txtuser_type_name";
                            //this.PANEL_FORM9_dataGridView1.Columns[5].Name = "Col_txtco_tel";
                            //this.PANEL_FORM9_dataGridView1.Columns[6].Name = "Col_user_status";

                            var index = PANEL_FORM9_dataGridView1.Rows.Add();
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                                                                                                    //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtempid = dt2.Rows[j]["txtemp_id"].ToString();      //2
                            string cipherText_txtempid = W_CryptorEngine.Decrypt(clearText_txtempid, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtempid.ToString();     //1
                                                                                                                                     //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname = dt2.Rows[j]["txtname"].ToString();      //2
                            string cipherText_txtname = W_CryptorEngine.Decrypt(clearText_txtname, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtname"].Value = cipherText_txtname.ToString();      //2
                            //ใส่รหัสฐานข้อมูล name============================================
                            string clearText_txtname_eng = dt2.Rows[j]["txtname_eng"].ToString();      //2
                            string cipherText_txtname_eng = W_CryptorEngine.Decrypt(clearText_txtname_eng, true);
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtname_eng"].Value = cipherText_txtname_eng.ToString();      //3

                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtuser_type_name"].Value = dt2.Rows[j]["txtuser_type_name"].ToString();      //4

                            //ใส่รหัสฐานข้อมูล user============================================
                            string clearText_txtusername = dt2.Rows[j]["txtuser_name"].ToString();      //6
                            string cipherText_txtusername = W_CryptorEngine.Decrypt(clearText_txtusername, true);
                            //=======================================================
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = cipherText_txtusername.ToString();      //5
                            PANEL_FORM9_dataGridView1.Rows[index].Cells["Col_user_status"].Value = dt2.Rows[j]["user_status"].ToString();      //6

                        }

                        PANEL_FORM9_Clear_GridView1_Up_Status();
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
                    Cursor.Current = Cursors.Default;

                    conn.Close();
                }

                //===========================================
            }
            //================================

        }
        private void PANEL_FORM9_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL_FORM9_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_FORM9_dataGridView1.Rows[e.RowIndex];

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
                    Cursor.Current = Cursors.WaitCursor;

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
                                    Cursor.Current = Cursors.Default;

                                }
                                //=======================================================
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
                            Cursor.Current = Cursors.Default;

                            conn.Close();
                        }

                        //===========================================
                    }
                    //================================
                    Cursor.Current = Cursors.WaitCursor;

                    UP_GridView1();

                    if (this.tabControl1.SelectedIndex == 1)
                    {
                        this.ch_all_1grid.Visible = true;
                        this.ch_all_2new.Visible = true;
                        this.ch_all_3edit.Visible = true;
                        this.ch_all_4print.Visible = true;
                        this.ch_all_5cancel.Visible = true;
                        Fill_GridView2();
                    }
                    else
                    {
                        this.ch_all_1grid.Visible = false;
                        this.ch_all_2new.Visible = false;
                        this.ch_all_3edit.Visible = false;
                        this.ch_all_4print.Visible = false;
                        this.ch_all_5cancel.Visible = false;

                    }
                    Cursor.Current = Cursors.Default;


                }
            }
        }
        private void PANEL_FORM9_GridView1()
        {
            this.PANEL_FORM9_dataGridView1.ColumnCount = 7;
            this.PANEL_FORM9_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_FORM9_dataGridView1.Columns[1].Name = "Col_txtemp_id";
            this.PANEL_FORM9_dataGridView1.Columns[2].Name = "Col_txtname";
            this.PANEL_FORM9_dataGridView1.Columns[3].Name = "Col_txtname_eng";
            this.PANEL_FORM9_dataGridView1.Columns[4].Name = "Col_txtuser_type_name";
            this.PANEL_FORM9_dataGridView1.Columns[5].Name = "Col_txtuser_name";
            this.PANEL_FORM9_dataGridView1.Columns[6].Name = "Col_user_status";

            this.PANEL_FORM9_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL_FORM9_dataGridView1.Columns[1].HeaderText = "รหัสพนักงาน";
            this.PANEL_FORM9_dataGridView1.Columns[2].HeaderText = " ชื่อ - สกุล";
            this.PANEL_FORM9_dataGridView1.Columns[3].HeaderText = " ชื่อ - สกุล  Eng";
            this.PANEL_FORM9_dataGridView1.Columns[4].HeaderText = "ประเภทผู้ใช้";
            this.PANEL_FORM9_dataGridView1.Columns[5].HeaderText = " User Name";
            this.PANEL_FORM9_dataGridView1.Columns[6].HeaderText = " สถานะ";

            this.PANEL_FORM9_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL_FORM9_dataGridView1.Columns[1].Visible = true;  //"Col_txtemp_id";
            this.PANEL_FORM9_dataGridView1.Columns[1].Width = 90;
            this.PANEL_FORM9_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL_FORM9_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM9_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM9_dataGridView1.Columns[2].Visible = true;  //"Col_txtname";
            this.PANEL_FORM9_dataGridView1.Columns[2].Width = 120;
            this.PANEL_FORM9_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL_FORM9_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM9_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM9_dataGridView1.Columns[3].Visible = false;  //"Col_txtname_eng";
            this.PANEL_FORM9_dataGridView1.Columns[3].Width = 0;
            this.PANEL_FORM9_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL_FORM9_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM9_dataGridView1.Columns[4].Visible = true;  //"Col_txtuser_type_name";
            this.PANEL_FORM9_dataGridView1.Columns[4].Width = 80;
            this.PANEL_FORM9_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL_FORM9_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM9_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM9_dataGridView1.Columns[5].Visible = false;  //"Col_txtuser_name";
            this.PANEL_FORM9_dataGridView1.Columns[5].Width = 00;
            this.PANEL_FORM9_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM9_dataGridView1.Columns[6].Visible = false;  //"Col_user_status";
            this.PANEL_FORM9_dataGridView1.Columns[6].Width = 0;
            this.PANEL_FORM9_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM9_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_FORM9_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_FORM9_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_FORM9_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_FORM9_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_FORM9_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_FORM9_dataGridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.Width = 70;
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL_FORM9_dataGridView1.Columns.Add(dgvCmb);

        }
        private void PANEL_FORM9_Clear_GridView1()
        {
            this.PANEL_FORM9_dataGridView1.Rows.Clear();
            this.PANEL_FORM9_dataGridView1.Refresh();
        }
        private void PANEL_FORM9_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL_FORM9_dataGridView1.Rows.Count ; i++)
            {
                if (this.PANEL_FORM9_dataGridView1.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL_FORM9_dataGridView1.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL_FORM9_dataGridView1.Rows[i].Cells[7].Value = false;

                }
            }
        }
        //User ===============================================================================

        //=================================================================================

        //1.สิทธิเข้าฝ่าย =======================================================================
        private void Fill_GridView1()
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

           Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM A001_sys_1depart" +
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
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtsys_depart_id"].Value = dt2.Rows[j]["txtsys_depart_id"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtsys_depart_name"].Value = dt2.Rows[j]["txtsys_depart_name"].ToString();      //2
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
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 3;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtsys_depart_id";
            this.GridView1.Columns[2].Name = "Col_txtsys_depart_name";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "รหัส";
            this.GridView1.Columns[2].HeaderText = " ระบบ";

            this.GridView1.Columns[0].Visible = false;  //"No";
            this.GridView1.Columns[1].Visible = true;  //"Col_txtsys_depart_id";
            this.GridView1.Columns[1].Width = 100;
            this.GridView1.Columns[1].ReadOnly = true;
            this.GridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[2].Visible = true;  //"Col_txtsys_depart_name";
            this.GridView1.Columns[2].Width = 250;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);

            dgvCmb.Name = "Col_1grid";  //3
            dgvCmb.HeaderText = "อนุญาต";
            dgvCmb.ReadOnly = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            GridView1.Columns.Add(dgvCmb);


        }
        private void Clear_GridView1()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
        }
        private void UP_GridView1()
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
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                for (int i = 0; i < this.GridView1.Rows.Count ; i++)
                {
                    if (this.GridView1.Rows[i].Cells[1].Value != null)
                    {
                        cmd2.CommandText = "SELECT *" +
                                  " FROM A003user_sys_1depart" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtuser_name = '" + cipherText_txtuser_name.Trim() + "')" +
                                   " AND (txtsys_depart_id = '" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "')" +
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
                                    if (dt2.Rows[j]["txtallow_login_status"].ToString() == "Y")
                                    {
                                        this.GridView1.Rows[i].Cells[3].Value = true;
                                    }
                                    else
                                    {
                                        this.GridView1.Rows[i].Cells[3].Value = false;
                                    }
                                }
                                //=======================================================
                            }
                            else
                            {

                                this.GridView1.Rows[i].Cells[3].Value = false;

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
        //สิทธิเข้าฝ่าย =======================================================================

        //================================================================================

        //1.สิทธิเข้าฟอร์ม =======================================================================
        private void Fill_GridView2()
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

           Clear_GridView2();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //Col_txtsys_depart_id


                for (int k = 0; k < this.GridView1.Rows.Count; k++)
                {
                    if (Convert.ToBoolean(this.GridView1.Rows[k].Cells["Col_1grid"].Value) == true)
                     {
                        cmd2.CommandText = "SELECT *" +
                                      " FROM A001_sys_2form" +
                                      " ORDER BY ID ASC";

                        cmd2.CommandText = "SELECT A001_sys_2form.*," +
                                            "A001_sys_1depart.*" +
                                            " FROM A001_sys_2form" +
                                            " INNER JOIN A001_sys_1depart" +
                                            " ON A001_sys_1depart.txtsys_depart_id = A001_sys_2form.txtsys_depart_id" +
                                            " WHERE (A001_sys_2form.txtsys_depart_id = '" + this.GridView1.Rows[k].Cells["Col_txtsys_depart_id"].Value.ToString() + "')" +
                                            " ORDER BY A001_sys_2form.txtsys_depart_id,A001_sys_2form.txtsys_form_id ASC";


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
                                    var index = GridView2.Rows.Add();
                                    GridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                                    GridView2.Rows[index].Cells["Col_txtsys_depart_id"].Value = dt2.Rows[j]["txtsys_depart_id"].ToString();      //1
                                    GridView2.Rows[index].Cells["Col_txtsys_depart_name"].Value = dt2.Rows[j]["txtsys_depart_name"].ToString();      //2
                                    GridView2.Rows[index].Cells["Col_txtsys_form_id"].Value = dt2.Rows[j]["txtsys_form_id"].ToString();      //3
                                    GridView2.Rows[index].Cells["Col_txtsys_form_name"].Value = dt2.Rows[j]["txtsys_form_name"].ToString();      //4
                                    GridView2.Rows[index].Cells["Col_txtsys_form_caption"].Value = dt2.Rows[j]["txtsys_form_caption"].ToString();      //5
                                }
                                //=======================================================
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
                            Cursor.Current = Cursors.Default;

                            conn.Close();
                        }

                        //===========================================


                    }
                }
            }
            //================================

        }
        private void Show_GridView2()
        {
            this.GridView2.ColumnCount = 6;
            this.GridView2.Columns[0].Name = "Col_Auto_num";
            this.GridView2.Columns[1].Name = "Col_txtsys_depart_id";
            this.GridView2.Columns[2].Name = "Col_txtsys_depart_name";
            this.GridView2.Columns[3].Name = "Col_txtsys_form_id";
            this.GridView2.Columns[4].Name = "Col_txtsys_form_name";
            this.GridView2.Columns[5].Name = "Col_txtsys_form_caption";

            this.GridView2.Columns[0].HeaderText = "No";
            this.GridView2.Columns[1].HeaderText = "รหัส";
            this.GridView2.Columns[2].HeaderText = " ระบบ";
            this.GridView2.Columns[3].HeaderText = " รหัสฟอร์ม";
            this.GridView2.Columns[4].HeaderText = " ฟอร์ม Code";
            this.GridView2.Columns[5].HeaderText = " ชื่อฟอร์ม";

            this.GridView2.Columns[0].Visible = false;  //"No";
            this.GridView2.Columns[1].Visible = false;  //"Col_txtsys_depart_id";
            this.GridView2.Columns[1].Width = 0;
            this.GridView2.Columns[1].ReadOnly = true;
            this.GridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[2].Visible = true;  //"Col_txtsys_depart_name";
            this.GridView2.Columns[2].Width = 150;
            this.GridView2.Columns[2].ReadOnly = true;
            this.GridView2.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[3].Visible = true;  //"Col_txtsys_form_id";
            this.GridView2.Columns[3].Width = 100;
            this.GridView2.Columns[3].ReadOnly = true;
            this.GridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[4].Visible = false;  //"Col_txtsys_form_name";
            this.GridView2.Columns[4].Width = 0;
            this.GridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[5].Visible = true;  //"Col_txtsys_form_caption";
            this.GridView2.Columns[5].Width = 250;
            this.GridView2.Columns[5].ReadOnly = true;
            this.GridView2.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;



            this.GridView2.DefaultCellStyle.Font = new Font("Tahoma", 8F);

            this.GridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView2.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView2.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb1 = new DataGridViewCheckBoxColumn();
            dgvCmb1.ValueType = typeof(bool);
            dgvCmb1.Name = "Col_1grid";   //6
            dgvCmb1.HeaderText = "ดูระเบียน";
            dgvCmb1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb1);

            DataGridViewCheckBoxColumn dgvCmb2 = new DataGridViewCheckBoxColumn();
            dgvCmb2.ValueType = typeof(bool);
            dgvCmb2.Name = "Col_2new";   //7
            dgvCmb2.HeaderText = "สร้างเอกสาร";
            dgvCmb2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb2);


            DataGridViewCheckBoxColumn dgvCmb3 = new DataGridViewCheckBoxColumn();
            dgvCmb3.ValueType = typeof(bool);
            dgvCmb3.Name = "Col_3Open";   //8
            dgvCmb3.HeaderText = "แก้ไขเอกสาร";
            dgvCmb3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb3);


            DataGridViewCheckBoxColumn dgvCmb4 = new DataGridViewCheckBoxColumn();
            dgvCmb4.ValueType = typeof(bool);
            dgvCmb4.Name = "Col_4Print";   //9
            dgvCmb4.HeaderText = "ปริ๊นเอกสาร";
            dgvCmb4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb4);

            DataGridViewCheckBoxColumn dgvCmb5 = new DataGridViewCheckBoxColumn();
            dgvCmb5.ValueType = typeof(bool);
            dgvCmb5.Name = "Col_5Cancel";   //10
            dgvCmb5.HeaderText = "ยกเลิกเอกสาร";
            dgvCmb5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb5);


        }
        private void Clear_GridView2()
        {
            this.GridView2.Rows.Clear();
            this.GridView2.Refresh();
        }
        private void UP_GridView2()
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
            Cursor.Current = Cursors.WaitCursor;
            //=======================================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                for (int i = 0; i < this.GridView2.Rows.Count ; i++)
                {
                    if (this.GridView2.Rows[i].Cells[1].Value != null)
                    {
                        cmd2.CommandText = "SELECT *" +
                                  " FROM A003user_sys_2form" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtuser_name = '" + cipherText_txtuser_name.Trim() + "')" +
                                   " AND (txtsys_depart_id = '" + this.GridView2.Rows[i].Cells[1].Value.ToString() + "')" +
                                   " AND (txtsys_form_id = '" + this.GridView2.Rows[i].Cells[3].Value.ToString() + "')" +
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
                                        this.GridView2.Rows[i].Cells[6].Value = true;
                                    }
                                    else
                                    {
                                        this.GridView2.Rows[i].Cells[6].Value = false;
                                    }
                                    //7
                                    if (dt2.Rows[j]["txtallow_2new_status"].ToString() == "Y")
                                    {
                                        this.GridView2.Rows[i].Cells[7].Value = true;
                                    }
                                    else
                                    {
                                        this.GridView2.Rows[i].Cells[7].Value = false;
                                    }
                                    //8
                                    if (dt2.Rows[j]["txtallow_3open_status"].ToString() == "Y")
                                    {
                                        this.GridView2.Rows[i].Cells[8].Value = true;
                                    }
                                    else
                                    {
                                        this.GridView2.Rows[i].Cells[8].Value = false;
                                    }
                                    //9
                                    if (dt2.Rows[j]["txtallow_4print_status"].ToString() == "Y")
                                    {
                                        this.GridView2.Rows[i].Cells[9].Value = true;
                                    }
                                    else
                                    {
                                        this.GridView2.Rows[i].Cells[9].Value = false;
                                    }
                                    //10
                                    if (dt2.Rows[j]["txtallow_5cancel_status"].ToString() == "Y")
                                    {
                                        this.GridView2.Rows[i].Cells[10].Value = true;
                                    }
                                    else
                                    {
                                        this.GridView2.Rows[i].Cells[10].Value = false;
                                    }
                                }
                                //=======================================================
                                Cursor.Current = Cursors.Default;
                            }
                            else
                            {

                                this.GridView2.Rows[i].Cells[6].Value = false;
                                this.GridView2.Rows[i].Cells[7].Value = false;
                                this.GridView2.Rows[i].Cells[8].Value = false;
                                this.GridView2.Rows[i].Cells[9].Value = false;
                                this.GridView2.Rows[i].Cells[10].Value = false;

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
                }
            }

        }
        //สิทธิเข้าฟอร์ม =======================================================================

        private void BtnNew_Click(object sender, EventArgs e)
        {

        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
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
            MessageBox.Show("" + W_ID_Select.CDKEY.Trim() +"");
            MessageBox.Show("" + cipherText_txtuser_name.Trim() + "");
            //=======================================================

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
                    //1 สิทธิเข้าฝ่าย

                    cmd2.CommandText = "DELETE FROM A003user_sys_1depart" +  //1
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                " AND (txtuser_name = '" + cipherText_txtuser_name.Trim() + "')";

                    cmd2.ExecuteNonQuery();

                    //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                    //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtsys_depart_id";
                    //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtsys_depart_name";

                    for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
                    {
                        if (this.GridView1.Rows[i].Cells[1].Value != null)
                        {
                            string ALLOW1 = "";

                            if (Convert.ToBoolean(this.GridView1.Rows[i].Cells[3].Value) == true)
                            {
                                ALLOW1 = "Y";
                            }
                            else
                            {
                                ALLOW1 = "N";
                            }
                            cmd2.CommandText = "INSERT INTO A003user_sys_1depart(cdkey," +  //1
                                               "txtuser_name," +  //2
                                               "txtsys_depart_id," +  //3
                                               "txtallow_login_status)" +  //4
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim()  + "'," +  //1
                                               "'" + cipherText_txtuser_name.ToString() + "'," +  //2
                                               "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +   //3
                                               "'" + ALLOW1.ToString()  + "')";

                            cmd2.ExecuteNonQuery();
                        }
                    }

                    //2 สิทธิเข้าใช้ฟอร์ม
                    cmd2.CommandText = "DELETE FROM A003user_sys_2form" +  //1
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                " AND (txtuser_name = '" + txtusername.Trim() + "')";

                    cmd2.ExecuteNonQuery();

                    //this.PANEL_FORM2_dataGridView1.Columns[0].Name = "Col_Auto_num";
                    //this.PANEL_FORM2_dataGridView1.Columns[1].Name = "Col_txtsys_depart_id";
                    //this.PANEL_FORM2_dataGridView1.Columns[2].Name = "Col_txtsys_depart_name";
                    //this.PANEL_FORM2_dataGridView1.Columns[3].Name = "Col_txtsys_form_id";
                    //this.PANEL_FORM2_dataGridView1.Columns[4].Name = "Col_txtsys_form_name";
                    //this.PANEL_FORM2_dataGridView1.Columns[5].Name = "Col_txtsys_form_caption";
                    //dgvCmb1.Name = "Col_1grid";   //6
                    //dgvCmb2.Name = "Col_2new";   //7
                    //dgvCmb3.Name = "Col_3Open";   //8
                    //dgvCmb5.Name = "Col_4Print";   //9
                    //dgvCmb6.Name = "Col_5Cancel";   //10


                    for (int i = 0; i < this.GridView2.Rows.Count - 0; i++)
                    {
                        if (this.GridView2.Rows[i].Cells[1].Value != null)
                        {
                            string ALLOW6 = "";
                            string ALLOW7 = "";
                            string ALLOW8 = "";
                            string ALLOW9 = "";
                            string ALLOW10 = "";

                            if (Convert.ToBoolean(this.GridView2.Rows[i].Cells[6].Value) == true) //dgvCmb1.Name = "Col_1grid";   //6
                            {
                                ALLOW6 = "Y";
                            }
                            else
                            {
                                ALLOW6 = "N";
                            }
                            if (Convert.ToBoolean(this.GridView2.Rows[i].Cells[7].Value) == true)  //dgvCmb2.Name = "Col_2new";   //7
                            {
                                ALLOW7 = "Y";
                            }
                            else
                            {
                                ALLOW7 = "N";
                            }
                            if (Convert.ToBoolean(this.GridView2.Rows[i].Cells[8].Value) == true)   //dgvCmb3.Name = "Col_3Open";   //8
                            {
                                ALLOW8 = "Y";
                            }
                            else
                            {
                                ALLOW8 = "N";
                            }
                            if (Convert.ToBoolean(this.GridView2.Rows[i].Cells[9].Value) == true)    //dgvCmb5.Name = "Col_4Print";   //10
                            {
                                ALLOW9 = "Y";
                            }
                            else
                            {
                                ALLOW9 = "N";
                            }
                            if (Convert.ToBoolean(this.GridView2.Rows[i].Cells[10].Value) == true)   //dgvCmb6.Name = "Col_5Cancel";   //11
                            {
                                ALLOW10 = "Y";
                            }
                            else
                            {
                                ALLOW10 = "N";
                            }

                            cmd2.CommandText = "INSERT INTO A003user_sys_2form(cdkey," +  //1
                                               "txtuser_name," +  //2
                                               "txtsys_depart_id," +  //3
                                               "txtsys_form_id," +  //4
                                               "txtallow_1grid_status," +  //5
                                               "txtallow_2new_status," +  //6
                                               "txtallow_3open_status," +  //7
                                               "txtallow_4print_status," +  //8
                                               "txtallow_5cancel_status)" +  //9
                                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +  //1
                                               "'" + cipherText_txtuser_name.ToString() + "'," +  //2
                                               "'" + this.GridView2.Rows[i].Cells[1].Value.ToString() + "'," +   //3
                                               "'" + this.GridView2.Rows[i].Cells[3].Value.ToString() + "'," +   //4
                                               "'" + ALLOW6.ToString() + "'," +   //5
                                               "'" + ALLOW7.ToString() + "'," +   //6
                                               "'" + ALLOW8.ToString() + "'," +   //7
                                               "'" + ALLOW9.ToString() + "'," +   //8
                                              "'" + ALLOW10.ToString() + "')"; //9

                            cmd2.ExecuteNonQuery();
                        }
                    }
                    //3 สิทธิรายงาน
                    Cursor.Current = Cursors.Default;



                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();
                        Cursor.Current = Cursors.Default;


                        W_ID_Select.LOG_ID = "6";
                        W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                        TRANS_LOG();

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
            Cursor.Current = Cursors.WaitCursor;
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

        private void ch_all_1grid_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < this.GridView2.Rows.Count; j++)
            {
                if (this.ch_all_1grid.Checked == true)
                {
                    this.GridView2.Rows[j].Cells["Col_1grid"].Value = true;
                }
                else
                {
                    this.GridView2.Rows[j].Cells["Col_1grid"].Value = false;
                }
            }
        }

        private void ch_all_2new_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < this.GridView2.Rows.Count; j++)
            {
                if (this.ch_all_2new.Checked == true)
                {
                    this.GridView2.Rows[j].Cells["Col_2new"].Value = true;

                }
                else
                {
                    this.GridView2.Rows[j].Cells["Col_2new"].Value = false;

                }
            }
        }

        private void ch_all_3edit_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < this.GridView2.Rows.Count; j++)
            {
                if (this.ch_all_3edit.Checked == true)
                {
                    this.GridView2.Rows[j].Cells["Col_3Open"].Value = true;

                }
                else
                {
                    this.GridView2.Rows[j].Cells["Col_3Open"].Value = false;

                }
            }
        }

        private void ch_all_4print_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < this.GridView2.Rows.Count; j++)
            {
                if (this.ch_all_4print.Checked==true)
                {
                    this.GridView2.Rows[j].Cells["Col_4Print"].Value = true;

                }
                else
                {
                    this.GridView2.Rows[j].Cells["Col_4Print"].Value = false;

                }
            }
        }

        private void ch_all_5cancel_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < this.GridView2.Rows.Count; j++)
            {
                if (this.ch_all_5cancel.Checked==true)
                {
                    this.GridView2.Rows[j].Cells["Col_5Cancel"].Value = true;

                }
                else
                {
                    this.GridView2.Rows[j].Cells["Col_5Cancel"].Value = false;

                }
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 1 )
            {
                this.ch_all_1grid.Visible = true;
                this.ch_all_2new.Visible = true;
                this.ch_all_3edit.Visible = true;
                this.ch_all_4print.Visible = true;
                this.ch_all_5cancel.Visible = true;
                Fill_GridView2();
            }
            else
            {
                this.ch_all_1grid.Visible = false;
                this.ch_all_2new.Visible = false;
                this.ch_all_3edit.Visible = false;
                this.ch_all_4print.Visible = false;
                this.ch_all_5cancel.Visible = false;

            }
        }






        //Tans_Log ====================================================================

    }
}
