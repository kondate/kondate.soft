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
    public partial class Form_s001_03cus : Form
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


        public Form_s001_03cus()
        {
            InitializeComponent();
        }

        private void Form_s001_03cus_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();

            W_ID_Select.M_COID = "KD";


            PANEL103_CUS_GridView1_cus();
            PANEL103_CUS_Fill_cus();

        }

        //txtcus ลูกค้า  =======================================================================
        private void PANEL103_CUS_Fill_cus()
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

            PANEL103_CUS_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL103_CUS_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL103_CUS_dataGridView1.Columns[1].Name = "Col_txtcus_no";
                //this.PANEL103_CUS_dataGridView1.Columns[2].Name = "Col_txtcus_id";
                //this.PANEL103_CUS_dataGridView1.Columns[3].Name = "Col_txtcus_name";
                //this.PANEL103_CUS_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
                //this.PANEL103_CUS_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL103_CUS_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL103_CUS_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL103_CUS_dataGridView1.Columns[8].Name = "Col_txtcus_status";

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
                            var index = PANEL103_CUS_dataGridView1.Rows.Add();
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_no"].Value = dt2.Rows[j]["txtcus_no"].ToString();      //1
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_id"].Value = dt2.Rows[j]["txtcus_id"].ToString();      //2
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name"].Value = dt2.Rows[j]["txtcus_name"].ToString();      //3
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name_eng"].Value = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_status"].Value = dt2.Rows[j]["txtcus_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL103_CUS_Clear_GridView1_Up_Status();

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
        private void PANEL103_CUS_GridView1_cus()
        {
            this.PANEL103_CUS_dataGridView1.ColumnCount = 9;
            this.PANEL103_CUS_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL103_CUS_dataGridView1.Columns[1].Name = "Col_txtcus_no";
            this.PANEL103_CUS_dataGridView1.Columns[2].Name = "Col_txtcus_id";
            this.PANEL103_CUS_dataGridView1.Columns[3].Name = "Col_txtcus_name";
            this.PANEL103_CUS_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
            this.PANEL103_CUS_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
            this.PANEL103_CUS_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
            this.PANEL103_CUS_dataGridView1.Columns[7].Name = "Col_txtremark";
            this.PANEL103_CUS_dataGridView1.Columns[8].Name = "Col_txtcus_status";

            this.PANEL103_CUS_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL103_CUS_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL103_CUS_dataGridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL103_CUS_dataGridView1.Columns[3].HeaderText = " ชื่อ ลูกค้า";
            this.PANEL103_CUS_dataGridView1.Columns[4].HeaderText = " ชื่อ ลูกค้า Eng";
            this.PANEL103_CUS_dataGridView1.Columns[5].HeaderText = " ผู้ติดต่อ";
            this.PANEL103_CUS_dataGridView1.Columns[6].HeaderText = " เบอร์โทร";
            this.PANEL103_CUS_dataGridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.PANEL103_CUS_dataGridView1.Columns[8].HeaderText = " สถานะ";

            this.PANEL103_CUS_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL103_CUS_dataGridView1.Columns[1].Visible = true;  //"Col_txtcus_no";
            this.PANEL103_CUS_dataGridView1.Columns[1].Width = 100;
            this.PANEL103_CUS_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[2].Visible = true;  //"Col_txtcus_id";
            this.PANEL103_CUS_dataGridView1.Columns[2].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[3].Visible = true;  //"Col_txtcus_name";
            this.PANEL103_CUS_dataGridView1.Columns[3].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[4].Visible = false;  //"Col_txtcus_name_eng";
            this.PANEL103_CUS_dataGridView1.Columns[4].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[5].Visible = true;  //"Col_txtcontact_person";
            this.PANEL103_CUS_dataGridView1.Columns[5].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[6].Visible = false;  //"Col_txtcontact_person_tel";
            this.PANEL103_CUS_dataGridView1.Columns[6].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[7].Visible = true;  //"Col_txtremark";
            this.PANEL103_CUS_dataGridView1.Columns[7].Width = 100;
            this.PANEL103_CUS_dataGridView1.Columns[7].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_CUS_dataGridView1.Columns[8].Visible = false;  //"Col_txtcus_status";

            this.PANEL103_CUS_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL103_CUS_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL103_CUS_dataGridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL103_CUS_dataGridView1.Columns.Add(dgvCmb);

        }
        private void PANEL103_CUS_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL103_CUS_dataGridView1.Rows.Count; i++)
            {
                if (this.PANEL103_CUS_dataGridView1.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.PANEL103_CUS_dataGridView1.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.PANEL103_CUS_dataGridView1.Rows[i].Cells[9].Value = false;

                }
            }
        }
        private void PANEL103_CUS_Clear_GridView1()
        {
            this.PANEL103_CUS_dataGridView1.Rows.Clear();
            this.PANEL103_CUS_dataGridView1.Refresh();
        }
        private void PANEL103_CUS_txtcus_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL103_CUS.Visible == false)
                {
                    this.PANEL103_CUS.Visible = true;
                    this.PANEL103_CUS.Location = new Point(this.PANEL103_CUS_txtcus_name.Location.X, this.PANEL103_CUS_txtcus_name.Location.Y + 22);
                    this.PANEL103_CUS_dataGridView1.Focus();
                }
                else
                {
                    this.PANEL103_CUS.Visible = false;
                }
        }
        private void PANEL103_CUS_btncus_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_CUS.Visible == false)
            {
                this.PANEL103_CUS.Visible = true;
                this.PANEL103_CUS.BringToFront();
                this.PANEL103_CUS.Location = new Point(this.PANEL103_CUS_txtcus_name.Location.X, this.PANEL103_CUS_txtcus_name.Location.Y + 22);
            }
            else
            {
                this.PANEL103_CUS.Visible = false;
            }
        }
        private void PANEL103_CUS_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_CUS.Visible == false)
            {
                this.PANEL103_CUS.Visible = true;
            }
            else
            {
                this.PANEL103_CUS.Visible = false;
            }
        }
        private void PANEL103_CUS_dataGridView1_cus_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL103_CUS_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL103_CUS_txtcus_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL103_CUS_txtcus_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL103_CUS_dataGridView1_cus_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL103_CUS_dataGridView1.CurrentRow.Index;

                this.PANEL103_CUS_txtcus_id.Text = PANEL103_CUS_dataGridView1.CurrentRow.Cells[1].Value.ToString();
                this.PANEL103_CUS_txtcus_name.Text = PANEL103_CUS_dataGridView1.CurrentRow.Cells[2].Value.ToString();
                this.PANEL103_CUS_txtcus_name.Focus();
                this.PANEL103_CUS.Visible = false;
            }
        }
        private void PANEL103_CUS_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL103_CUS_btn_search_Click(object sender, EventArgs e)
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

            PANEL103_CUS_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL103_CUS_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL103_CUS_dataGridView1.Columns[1].Name = "Col_txtcus_no";
                //this.PANEL103_CUS_dataGridView1.Columns[2].Name = "Col_txtcus_id";
                //this.PANEL103_CUS_dataGridView1.Columns[3].Name = "Col_txtcus_name";
                //this.PANEL103_CUS_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
                //this.PANEL103_CUS_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL103_CUS_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL103_CUS_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL103_CUS_dataGridView1.Columns[8].Name = "Col_txtcus_status";

                cmd2.CommandText = "SELECT s001_03cus.*," +
                                    "s001_03cus_1address.*" +
                                    " FROM s001_03cus" +

                                    " INNER JOIN s001_03cus_1address" +
                                    " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                    " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                    " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                    " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (s001_03cus.txtcus_name LIKE '%" + this.PANEL103_CUS_txtsearch.Text.Trim() + "%')" +
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
                            var index = PANEL103_CUS_dataGridView1.Rows.Add();
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_no"].Value = dt2.Rows[j]["txtcus_no"].ToString();      //1
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_id"].Value = dt2.Rows[j]["txtcus_id"].ToString();      //2
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name"].Value = dt2.Rows[j]["txtcus_name"].ToString();      //3
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name_eng"].Value = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_status"].Value = dt2.Rows[j]["txtcus_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL103_CUS_Clear_GridView1_Up_Status();

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
        bool allowResize = false;
        private void PANEL103_CUS_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL103_CUS_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL103_CUS.Height = PANEL103_CUS_btnresize_low.Top + e.Y;
                this.PANEL103_CUS.Width = PANEL103_CUS_btnresize_low.Left + e.X;
            }
        }
        private void PANEL103_CUS_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL103_CUS_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtcus ลูกค้า  =======================================================================

    }
}
