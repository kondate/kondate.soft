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

namespace kondate.soft.Panel_SETUP
{
    public partial class Form_a003db_user : Form
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



        public Form_a003db_user()
        {
            InitializeComponent();
        }

        private void Form_a003db_user_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();



            PANEL003_EMP_GridView1_emp();
            PANEL003_EMP_Fill_emp();

        }

        //txtemp พนักงาน =======================================================================
        private void PANEL003_EMP_Fill_emp()
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

            PANEL003_EMP_Clear_GridView1_emp();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


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
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //ใส่รหัสฐานข้อมูล============================================
                            //ใส่รหัสฐานข้อมูล user============================================
                            string clearText_txtemp_id = dt2.Rows[j]["txtemp_id"].ToString();      //1
                            string cipherText_txtemp_id = W_CryptorEngine.Decrypt(clearText_txtemp_id, true);
                            //=======================================================

                            //=======================================================
                            //ใส่รหัสฐานข้อมูล============================================
                            string clearText_txtemp_name = dt2.Rows[j]["txtname"].ToString();
                            string cipherText_txtemp_name = W_CryptorEngine.Decrypt(clearText_txtemp_name, true);

                            var index = PANEL003_EMP_dataGridView1_emp.Rows.Add();
                            PANEL003_EMP_dataGridView1_emp.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL003_EMP_dataGridView1_emp.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtemp_id.ToString();      //1
                            PANEL003_EMP_dataGridView1_emp.Rows[index].Cells["Col_txtname"].Value = cipherText_txtemp_name.ToString();      //2

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
        private void PANEL003_EMP_GridView1_emp()
        {
            this.PANEL003_EMP_dataGridView1_emp.ColumnCount = 3;
            this.PANEL003_EMP_dataGridView1_emp.Columns[0].Name = "Col_Auto_num";
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].Name = "Col_txtemp_id";
            this.PANEL003_EMP_dataGridView1_emp.Columns[2].Name = "Col_txtname";

            this.PANEL003_EMP_dataGridView1_emp.Columns[0].HeaderText = "No";
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].HeaderText = "รหัสพนักงาน";
            this.PANEL003_EMP_dataGridView1_emp.Columns[2].HeaderText = " ชื่อพนักงาน";

            this.PANEL003_EMP_dataGridView1_emp.Columns[0].Visible = false;  //"No";
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].Visible = true;  //"Col_txtemp_id";
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].Width = 100;
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].ReadOnly = true;
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL003_EMP_dataGridView1_emp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL003_EMP_dataGridView1_emp.Columns[2].Visible = true;  //"Col_txtname";
            this.PANEL003_EMP_dataGridView1_emp.Columns[2].Width = 150;
            this.PANEL003_EMP_dataGridView1_emp.Columns[2].ReadOnly = true;
            this.PANEL003_EMP_dataGridView1_emp.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL003_EMP_dataGridView1_emp.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL003_EMP_dataGridView1_emp.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL003_EMP_dataGridView1_emp.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL003_EMP_dataGridView1_emp.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL003_EMP_dataGridView1_emp.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL003_EMP_dataGridView1_emp.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL003_EMP_dataGridView1_emp.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL003_EMP_dataGridView1_emp.EnableHeadersVisualStyles = false;

        }
        private void PANEL003_EMP_Clear_GridView1_emp()
        {
            this.PANEL003_EMP_dataGridView1_emp.Rows.Clear();
            this.PANEL003_EMP_dataGridView1_emp.Refresh();
        }
        private void PANEL003_EMP_txtemp_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL003_EMP.Visible == false)
                {
                    this.PANEL003_EMP.Visible = true;
                    this.PANEL003_EMP.Location = new Point(this.PANEL003_EMP_txtemp_name.Location.X, this.PANEL003_EMP_txtemp_name.Location.Y + 22);
                    this.PANEL003_EMP_dataGridView1_emp.Focus();
                }
                else
                {
                    this.PANEL003_EMP.Visible = false;
                }
        }
        private void PANEL003_EMP_btnemp_Click(object sender, EventArgs e)
        {
            if (this.PANEL003_EMP.Visible == false)
            {
                this.PANEL003_EMP.Visible = true;
                this.PANEL003_EMP.BringToFront();
                this.PANEL003_EMP.Location = new Point(this.PANEL003_EMP_txtemp_name.Location.X, this.PANEL003_EMP_txtemp_name.Location.Y + 22);
            }
            else
            {
                this.PANEL003_EMP.Visible = false;
            }
        }
        private void PANEL003_EMP_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL003_EMP.Visible == false)
            {
                this.PANEL003_EMP.Visible = true;
            }
            else
            {
                this.PANEL003_EMP.Visible = false;
            }
        }
        private void PANEL003_EMP_dataGridView1_emp_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL003_EMP_dataGridView1_emp.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL003_EMP_txtemp_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL003_EMP_txtemp_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL003_EMP_dataGridView1_emp_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL003_EMP_dataGridView1_emp.CurrentRow.Index;

                this.PANEL003_EMP_txtemp_id.Text = PANEL003_EMP_dataGridView1_emp.CurrentRow.Cells[1].Value.ToString();
                this.PANEL003_EMP_txtemp_name.Text = PANEL003_EMP_dataGridView1_emp.CurrentRow.Cells[2].Value.ToString();
                this.PANEL003_EMP_txtemp_name.Focus();
                this.PANEL003_EMP.Visible = false;
            }
        }
        private void PANEL003_EMP_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL003_EMP_btn_search_Click(object sender, EventArgs e)
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

            PANEL003_EMP_Clear_GridView1_emp();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                string clearText_txtemp_name_s = this.PANEL003_EMP_txtsearch.Text.Trim();      //1
                string cipherText_txtemp_name_s = W_CryptorEngine.Encrypt(clearText_txtemp_name_s, true);

                cmd2.CommandText = "SELECT *" +
                                  " FROM a003db_user" +
                                   " WHERE (txtname LIKE '%" + cipherText_txtemp_name_s.Trim() + "%')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                              //    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            //ใส่รหัสฐานข้อมูล============================================
                            //ใส่รหัสฐานข้อมูล user============================================
                            string clearText_txtemp_id = dt2.Rows[j]["txtemp_id"].ToString();      //1
                            string cipherText_txtemp_id = W_CryptorEngine.Decrypt(clearText_txtemp_id, true);
                            //=======================================================

                            //=======================================================
                            //ใส่รหัสฐานข้อมูล============================================
                            string clearText_txtemp_name = dt2.Rows[j]["txtname"].ToString();
                            string cipherText_txtemp_name = W_CryptorEngine.Decrypt(clearText_txtemp_name, true);

                            var index = PANEL003_EMP_dataGridView1_emp.Rows.Add();
                            PANEL003_EMP_dataGridView1_emp.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL003_EMP_dataGridView1_emp.Rows[index].Cells["Col_txtemp_id"].Value = cipherText_txtemp_id.ToString();      //1
                            PANEL003_EMP_dataGridView1_emp.Rows[index].Cells["Col_txtname"].Value = cipherText_txtemp_name.ToString();      //2
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
        bool allowResize = false;
        private void PANEL003_EMP_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL003_EMP_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL003_EMP.Height = PANEL003_EMP_btnresize_low.Top + e.Y;
                this.PANEL003_EMP.Width = PANEL003_EMP_btnresize_low.Left + e.X;
            }
        }
        private void PANEL003_EMP_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL003_EMP_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtemp พนักงาน =======================================================================


    }
}
