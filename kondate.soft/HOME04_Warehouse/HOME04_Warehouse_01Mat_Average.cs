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
namespace kondate.soft.HOME04_Warehouse
{
    public partial class HOME04_Warehouse_01Mat_Average : Form
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



        public HOME04_Warehouse_01Mat_Average()
        {
            InitializeComponent();
        }

        private void HOME04_Warehouse_01Mat_Average_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0401ARGR";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblword_status.Text = "ระเบียนสต๊อคสินค้า / วัตถุดิบ";
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.ActiveControl = this.txtsearch;

            this.BtnNew.Enabled = false;
            this.btnopen.Enabled = false;
            this.BtnSave.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.BtnPrint.Enabled = false;
            this.btnPreview.Enabled = false;

            this.dtpend.Value = DateTime.Now;
            this.dtpend.Format = DateTimePickerFormat.Custom;
            this.dtpend.CustomFormat = this.dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.dtpstart.Value = DateTime.Today.AddDays(-7);
            this.dtpstart.Format = DateTimePickerFormat.Custom;
            this.dtpstart.CustomFormat = this.dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

            //========================================
            this.cboSearch.Items.Add("รหัสสินค้า");
            this.cboSearch.Items.Add("ชื่อสินค้า");
            this.cboSearch.Text = "ชื่อสินค้า";
            //========================================

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            Show_GridView2();
            Show_GridView3();
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            //test.....
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpstart_ValueChanged(object sender, EventArgs e)
        {
            this.dtpstart.Format = DateTimePickerFormat.Custom;
            this.dtpstart.CustomFormat = this.dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void dtpend_ValueChanged(object sender, EventArgs e)
        {
            this.dtpend.Format = DateTimePickerFormat.Custom;
            this.dtpend.CustomFormat = this.dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

        }
        private void btnGo1_Click(object sender, EventArgs e)
        {
            Fill_Show_SEARCH_GO1_DATA_GridView2();
        }

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

            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Visible = false;  //"Col_txtwherehouse_name_eng";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Width = 0;
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
                    W_ID_Select.TRANS_ID = row.Cells[1].Value.ToString();
                    Fill_Show_DATA_GridView2();
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
        //END txtwherehouse คลังสินค้า  =======================================================================


        private void Fill_Show_DATA_GridView2()
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

            Clear_GridView2();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT k021_mat_average.*," +
                                   "b001mat.*," +
                                    "b001mat_02detail.*," +
                                   "b001_05mat_unit1.*," +
                                   "b001_05mat_unit2.*," +
                                   "b001_05mat_unit3.*," +
                                   "b001_05mat_unit4.*," +
                                   "b001_05mat_unit5.*," +

                                   "b001mat_13point_phurchase.*" +

                                   " FROM k021_mat_average" +

                                   " INNER JOIN b001mat" +
                                   " ON k021_mat_average.cdkey = b001mat.cdkey" +
                                   " AND k021_mat_average.txtco_id = b001mat.txtco_id" +
                                   " AND k021_mat_average.txtmat_id = b001mat.txtmat_id" +

                                   " INNER JOIN b001mat_02detail" +
                                   " ON k021_mat_average.cdkey = b001mat_02detail.cdkey" +
                                   " AND k021_mat_average.txtco_id = b001mat_02detail.txtco_id" +
                                   " AND k021_mat_average.txtmat_id = b001mat_02detail.txtmat_id" +

                                   " INNER JOIN b001_05mat_unit1" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                   " INNER JOIN b001_05mat_unit2" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                   " INNER JOIN b001_05mat_unit3" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit3.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit3.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit3_id = b001_05mat_unit3.txtmat_unit3_id" +

                                   " INNER JOIN b001_05mat_unit4" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit4.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit4.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit4_id = b001_05mat_unit4.txtmat_unit4_id" +

                                   " INNER JOIN b001_05mat_unit5" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit5.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit5.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit5_id = b001_05mat_unit5.txtmat_unit5_id" +


                                   " INNER JOIN b001mat_13point_phurchase" +
                                   " ON k021_mat_average.cdkey = b001mat_13point_phurchase.cdkey" +
                                   " AND k021_mat_average.txtco_id = b001mat_13point_phurchase.txtco_id" +
                                   " AND k021_mat_average.txtmat_id = b001mat_13point_phurchase.txtmat_id" +

                                   " WHERE (k021_mat_average.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k021_mat_average.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k021_mat_average.txtwherehouse_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                  " ORDER BY k021_mat_average.txtmat_no ASC";

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
                            //this.GridView2.ColumnCount = 16;
                            //this.GridView2.Columns[0].Name = "Col_Auto_num";
                            //this.GridView2.Columns[1].Name = "Col_txtwherehouse_id";

                            //this.GridView2.Columns[2].Name = "Col_txtmat_no";
                            //this.GridView2.Columns[3].Name = "Col_txtmat_id";
                            //this.GridView2.Columns[4].Name = "Col_txtmat_name";
                            //this.GridView2.Columns[5].Name = "Col_txtmat_unit1_name";
                            //this.GridView2.Columns[6].Name = "Col_txtmat_unit1_qty";

                            //this.GridView2.Columns[7].Name = "Col_chmat_unit_status";

                            //this.GridView2.Columns[8].Name = "Col_txtmat_unit2_name";
                            //this.GridView2.Columns[9].Name = "Col_txtmat_unit2_qty";


                            //this.GridView2.Columns[10].Name = "Col_txtcost_qty_balance";
                            //this.GridView2.Columns[11].Name = "Col_txtcost_qty_price_average";
                            //this.GridView2.Columns[12].Name = "Col_txtcost_money_sum";
                            //this.GridView2.Columns[13].Name = "Col_txtcost_qty2_balance";
                            //this.GridView2.Columns[14].Name = "Col_txtmat_amount_phurchase";
                            //this.GridView2.Columns[15].Name = "Col_txtmat_status";

                            var index = GridView2.Rows.Add();
                            GridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView2.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            GridView2.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //2
                            GridView2.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //3
                            GridView2.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //4
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //5
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");        //6
                            GridView2.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();     //7
                            GridView2.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //9

                            GridView2.Rows[index].Cells["Col_txtmat_unit3_name"].Value = dt2.Rows[j]["txtmat_unit3_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit4_name"].Value = dt2.Rows[j]["txtmat_unit4_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit5_name"].Value = dt2.Rows[j]["txtmat_unit5_name"].ToString();     //8

                            GridView2.Rows[index].Cells["Col_txtcost_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");      //10
                            GridView2.Rows[index].Cells["Col_txtcost_qty_price_average"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");      //11
                            GridView2.Rows[index].Cells["Col_txtcost_money_sum"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");      //12

                            GridView2.Rows[index].Cells["Col_txtcost_qty2_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtcost_qty_krasob_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_krasob_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtcost_qty_lod_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_lod_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtcost_qty_pub_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_pub_balance"]).ToString("###,###.00");      //13

                            GridView2.Rows[index].Cells["Col_txtmat_amount_phurchase"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_amount_phurchase"]).ToString("###,###.00");      //14
                            GridView2.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //15

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
            GridView2_Color();
            GridView2_Color_Column();
            GridView2_UP_Status();
        }
        private void Fill_Show_SEARCH_GO1_DATA_GridView2()
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

            Clear_GridView2();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                this.cboSearch.Items.Add("รหัสสินค้า");
                this.cboSearch.Items.Add("ชื่อสินค้า");
                if (this.cboSearch.Text  == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT k021_mat_average.*," +
                                       "b001mat.*," +
                                        "b001mat_02detail.*," +
                                       "b001_05mat_unit1.*," +
                                       "b001_05mat_unit2.*," +
                                       "b001_05mat_unit3.*," +
                                       "b001_05mat_unit4.*," +
                                       "b001_05mat_unit5.*," +

                                       "b001mat_13point_phurchase.*" +

                                       " FROM k021_mat_average" +

                                       " INNER JOIN b001mat" +
                                       " ON k021_mat_average.cdkey = b001mat.cdkey" +
                                       " AND k021_mat_average.txtco_id = b001mat.txtco_id" +
                                       " AND k021_mat_average.txtmat_id = b001mat.txtmat_id" +

                                       " INNER JOIN b001mat_02detail" +
                                       " ON k021_mat_average.cdkey = b001mat_02detail.cdkey" +
                                       " AND k021_mat_average.txtco_id = b001mat_02detail.txtco_id" +
                                       " AND k021_mat_average.txtmat_id = b001mat_02detail.txtmat_id" +

                                       " INNER JOIN b001_05mat_unit1" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                       " INNER JOIN b001_05mat_unit2" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                       " INNER JOIN b001_05mat_unit3" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit3.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit3.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit3_id = b001_05mat_unit3.txtmat_unit3_id" +

                                       " INNER JOIN b001_05mat_unit4" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit4.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit4.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit4_id = b001_05mat_unit4.txtmat_unit4_id" +

                                       " INNER JOIN b001_05mat_unit5" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit5.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit5.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit5_id = b001_05mat_unit5.txtmat_unit5_id" +


                                       " INNER JOIN b001mat_13point_phurchase" +
                                       " ON k021_mat_average.cdkey = b001mat_13point_phurchase.cdkey" +
                                       " AND k021_mat_average.txtco_id = b001mat_13point_phurchase.txtco_id" +
                                       " AND k021_mat_average.txtmat_id = b001mat_13point_phurchase.txtmat_id" +

                                           " WHERE (k021_mat_average.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k021_mat_average.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //    " AND (k021_mat_average.txtwherehouse_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                       " AND (k021_mat_average.txtmat_id = '" + this.txtsearch.Text.Trim() + "')" +
                                      " ORDER BY k021_mat_average.txtmat_no ASC";

                }
                if (this.cboSearch.Text == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT k021_mat_average.*," +
                                       "b001mat.*," +
                                        "b001mat_02detail.*," +
                                       "b001_05mat_unit1.*," +
                                       "b001_05mat_unit2.*," +
                                       "b001_05mat_unit3.*," +
                                       "b001_05mat_unit4.*," +
                                       "b001_05mat_unit5.*," +

                                       "b001mat_13point_phurchase.*" +

                                       " FROM k021_mat_average" +

                                       " INNER JOIN b001mat" +
                                       " ON k021_mat_average.cdkey = b001mat.cdkey" +
                                       " AND k021_mat_average.txtco_id = b001mat.txtco_id" +
                                       " AND k021_mat_average.txtmat_id = b001mat.txtmat_id" +

                                       " INNER JOIN b001mat_02detail" +
                                       " ON k021_mat_average.cdkey = b001mat_02detail.cdkey" +
                                       " AND k021_mat_average.txtco_id = b001mat_02detail.txtco_id" +
                                       " AND k021_mat_average.txtmat_id = b001mat_02detail.txtmat_id" +

                                       " INNER JOIN b001_05mat_unit1" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                       " INNER JOIN b001_05mat_unit2" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                       " INNER JOIN b001_05mat_unit3" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit3.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit3.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit3_id = b001_05mat_unit3.txtmat_unit3_id" +

                                       " INNER JOIN b001_05mat_unit4" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit4.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit4.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit4_id = b001_05mat_unit4.txtmat_unit4_id" +

                                       " INNER JOIN b001_05mat_unit5" +
                                       " ON b001mat_02detail.cdkey = b001_05mat_unit5.cdkey" +
                                       " AND b001mat_02detail.txtco_id = b001_05mat_unit5.txtco_id" +
                                       " AND b001mat_02detail.txtmat_unit5_id = b001_05mat_unit5.txtmat_unit5_id" +


                                       " INNER JOIN b001mat_13point_phurchase" +
                                       " ON k021_mat_average.cdkey = b001mat_13point_phurchase.cdkey" +
                                       " AND k021_mat_average.txtco_id = b001mat_13point_phurchase.txtco_id" +
                                       " AND k021_mat_average.txtmat_id = b001mat_13point_phurchase.txtmat_id" +

                                           " WHERE (k021_mat_average.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k021_mat_average.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //    " AND (k021_mat_average.txtwherehouse_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                       " AND (k021_mat_average.txtmat_name LIKE '%" + this.txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY k021_mat_average.txtmat_no ASC";

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
                            //this.GridView2.ColumnCount = 16;
                            //this.GridView2.Columns[0].Name = "Col_Auto_num";
                            //this.GridView2.Columns[1].Name = "Col_txtwherehouse_id";

                            //this.GridView2.Columns[2].Name = "Col_txtmat_no";
                            //this.GridView2.Columns[3].Name = "Col_txtmat_id";
                            //this.GridView2.Columns[4].Name = "Col_txtmat_name";
                            //this.GridView2.Columns[5].Name = "Col_txtmat_unit1_name";
                            //this.GridView2.Columns[6].Name = "Col_txtmat_unit1_qty";

                            //this.GridView2.Columns[7].Name = "Col_chmat_unit_status";

                            //this.GridView2.Columns[8].Name = "Col_txtmat_unit2_name";
                            //this.GridView2.Columns[9].Name = "Col_txtmat_unit2_qty";


                            //this.GridView2.Columns[10].Name = "Col_txtcost_qty_balance";
                            //this.GridView2.Columns[11].Name = "Col_txtcost_qty_price_average";
                            //this.GridView2.Columns[12].Name = "Col_txtcost_money_sum";
                            //this.GridView2.Columns[13].Name = "Col_txtcost_qty2_balance";
                            //this.GridView2.Columns[14].Name = "Col_txtmat_amount_phurchase";
                            //this.GridView2.Columns[15].Name = "Col_txtmat_status";

                            var index = GridView2.Rows.Add();
                            GridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView2.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            GridView2.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //2
                            GridView2.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //3
                            GridView2.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //4
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //5
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");        //6
                            GridView2.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();     //7
                            GridView2.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //9
                            GridView2.Rows[index].Cells["Col_txtmat_unit3_name"].Value = dt2.Rows[j]["txtmat_unit3_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit4_name"].Value = dt2.Rows[j]["txtmat_unit4_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit5_name"].Value = dt2.Rows[j]["txtmat_unit5_name"].ToString();     //8

                            GridView2.Rows[index].Cells["Col_txtcost_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");      //10
                            GridView2.Rows[index].Cells["Col_txtcost_qty_price_average"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");      //11
                            GridView2.Rows[index].Cells["Col_txtcost_money_sum"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");      //12
                            GridView2.Rows[index].Cells["Col_txtcost_qty2_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtcost_qty_krasob_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_krasob_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtcost_qty_lod_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_lod_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtcost_qty_pub_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_pub_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtmat_amount_phurchase"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_amount_phurchase"]).ToString("###,###.00");      //14
                            GridView2.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //15



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
            GridView2_Color();
            GridView2_Color_Column();
            GridView2_UP_Status();
        }
        private void Show_GridView2()
        {
            this.GridView2.ColumnCount = 22;
            this.GridView2.Columns[0].Name = "Col_Auto_num";
            this.GridView2.Columns[1].Name = "Col_txtwherehouse_id";

            this.GridView2.Columns[2].Name = "Col_txtmat_no";
            this.GridView2.Columns[3].Name = "Col_txtmat_id";
            this.GridView2.Columns[4].Name = "Col_txtmat_name";
            this.GridView2.Columns[5].Name = "Col_txtmat_unit1_name";
            this.GridView2.Columns[6].Name = "Col_txtmat_unit1_qty";

            this.GridView2.Columns[7].Name = "Col_chmat_unit_status";

            this.GridView2.Columns[8].Name = "Col_txtmat_unit2_name";
            this.GridView2.Columns[9].Name = "Col_txtmat_unit2_qty";
            this.GridView2.Columns[10].Name = "Col_txtmat_unit3_name";
            this.GridView2.Columns[11].Name = "Col_txtmat_unit4_name";
            this.GridView2.Columns[12].Name = "Col_txtmat_unit5_name";


            this.GridView2.Columns[13].Name = "Col_txtcost_qty_balance";
            this.GridView2.Columns[14].Name = "Col_txtcost_qty_price_average";
            this.GridView2.Columns[15].Name = "Col_txtcost_money_sum";
            this.GridView2.Columns[16].Name = "Col_txtcost_qty2_balance";
            this.GridView2.Columns[17].Name = "Col_txtcost_qty_krasob_balance";
            this.GridView2.Columns[18].Name = "Col_txtcost_qty_lod_balance";
            this.GridView2.Columns[19].Name = "Col_txtcost_qty_pub_balance";


            this.GridView2.Columns[20].Name = "Col_txtmat_amount_phurchase";
            this.GridView2.Columns[21].Name = "Col_txtmat_status";

            this.GridView2.Columns[0].HeaderText = "No";
            this.GridView2.Columns[1].HeaderText = "รหัสคลัง";

            this.GridView2.Columns[2].HeaderText = "ลำดับ";
            this.GridView2.Columns[3].HeaderText = " รหัส";
            this.GridView2.Columns[4].HeaderText = " ชื่อสินค้า";
            this.GridView2.Columns[5].HeaderText = " หน่วยหลัก";
            this.GridView2.Columns[6].HeaderText = " หน่วย";
            this.GridView2.Columns[7].HeaderText = "แปลง";
            this.GridView2.Columns[8].HeaderText = " หน่วย2";
            this.GridView2.Columns[9].HeaderText = " หน่วย";
            this.GridView2.Columns[10].HeaderText = " หน่วย3";
            this.GridView2.Columns[11].HeaderText = " หน่วย4";
            this.GridView2.Columns[12].HeaderText = " หน่วย5";


            this.GridView2.Columns[13].HeaderText = "คงเหลือ";
            this.GridView2.Columns[14].HeaderText = "ราคาเฉลี่ย";
            this.GridView2.Columns[15].HeaderText = "มูลค่าเฉลี่ย";
            this.GridView2.Columns[16].HeaderText = "คงเหลือ(หน่วย2)";
            this.GridView2.Columns[17].HeaderText = "คงเหลือ(หน่วย3)";
            this.GridView2.Columns[18].HeaderText = "คงเหลือ(หน่วย4)";
            this.GridView2.Columns[19].HeaderText = "คงเหลือ(หน่วย5)";

            this.GridView2.Columns[20].HeaderText = "จุดสั่งซื้อ";
            this.GridView2.Columns[21].HeaderText = "สถานะ";

            this.GridView2.Columns[0].Visible = false;  //"Col_Auto_num";

            this.GridView2.Columns["Col_Auto_num"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView2.Columns["Col_Auto_num"].Width = 0;
            this.GridView2.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView2.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_Auto_num"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView2.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView2.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView2.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtwherehouse_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no"";
            this.GridView2.Columns["Col_txtmat_no"].Width = 60;
            this.GridView2.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_no"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView2.Columns["Col_txtmat_id"].Width = 100;
            this.GridView2.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView2.Columns["Col_txtmat_name"].Width = 200;
            this.GridView2.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView2.Columns["Col_txtmat_unit1_name"].Width = 80;
            this.GridView2.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.GridView2.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView2.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView2.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView2.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView2.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_chmat_unit_status"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit2_name"].Visible = true;  //Col_txtmat_unit2_name";
            this.GridView2.Columns["Col_txtmat_unit2_name"].Width = 80;
            this.GridView2.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView2.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView2.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmat_unit3_name"].Visible = true;  //Col_txtmat_unit3_name";
            this.GridView2.Columns["Col_txtmat_unit3_name"].Width = 80;
            this.GridView2.Columns["Col_txtmat_unit3_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit3_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit3_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit3_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit4_name"].Visible = true;  //Col_txtmat_unit4_name";
            this.GridView2.Columns["Col_txtmat_unit4_name"].Width = 80;
            this.GridView2.Columns["Col_txtmat_unit4_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit4_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit4_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit4_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit5_name"].Visible = true;  //Col_txtmat_unit5_name";
            this.GridView2.Columns["Col_txtmat_unit5_name"].Width = 80;
            this.GridView2.Columns["Col_txtmat_unit5_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit5_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit5_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit5_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView2.Columns["Col_txtcost_qty_balance"].Visible = true;  //"Col_txtcost_qty_balance";
            this.GridView2.Columns["Col_txtcost_qty_balance"].Width = 90;
            this.GridView2.Columns["Col_txtcost_qty_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_price_average"].Visible = true;  //"Col_txtcost_qty_price_average";
            this.GridView2.Columns["Col_txtcost_qty_price_average"].Width = 90;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_price_average"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_money_sum"].Visible = true;  //"Col_txtcost_money_sum";
            this.GridView2.Columns["Col_txtcost_money_sum"].Width =90;
            this.GridView2.Columns["Col_txtcost_money_sum"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_money_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_money_sum"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_money_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty2_balance"].Visible = true;  //"Col_txtcost_qty2_balance";
            this.GridView2.Columns["Col_txtcost_qty2_balance"].Width = 120;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty2_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_krasob_balance"].Visible = true;  //"Col_txtcost_qty_krasob_balance";
            this.GridView2.Columns["Col_txtcost_qty_krasob_balance"].Width = 120;
            this.GridView2.Columns["Col_txtcost_qty_krasob_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_krasob_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_krasob_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_krasob_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_lod_balance"].Visible = true;  //"Col_txtcost_qty_lod_balance";
            this.GridView2.Columns["Col_txtcost_qty_lod_balance"].Width = 120;
            this.GridView2.Columns["Col_txtcost_qty_lod_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_lod_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_lod_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_lod_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_pub_balance"].Visible = true;  //"Col_txtcost_qty_pub_balance";
            this.GridView2.Columns["Col_txtcost_qty_pub_balance"].Width = 120;
            this.GridView2.Columns["Col_txtcost_qty_pub_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_pub_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_pub_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_pub_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView2.Columns["Col_txtmat_amount_phurchase"].Visible = true;  //"Col_txtmat_amount_phurchase";
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].Width = 120;
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmat_status"].Visible = false;  //"Col_txtmat_status";
            this.GridView2.Columns["Col_txtmat_status"].Width = 0;
            this.GridView2.Columns["Col_txtmat_status"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_status"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 70;
            dgvCmb.DisplayIndex = 7;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView2.Columns.Add(dgvCmb);

            DataGridViewCheckBoxColumn dgvCmb2 = new DataGridViewCheckBoxColumn();
            dgvCmb2.ValueType = typeof(bool);
            dgvCmb2.Width = 70;
            dgvCmb2.DisplayIndex = 16;
            dgvCmb2.Name = "Col_Chk2";
            dgvCmb2.HeaderText = "สถานะ";
            dgvCmb2.ReadOnly = true;
            dgvCmb2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb2);


            this.GridView2.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView2.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView2.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView2.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView2()
        {
            this.GridView2.Rows.Clear();
            this.GridView2.Refresh();
        }
        private void GridView2_Color()
        {
            for (int i = 0; i < this.GridView2.Rows.Count - 0; i++)
            {
                if (Convert.ToDouble(string.Format("{0:n0}", this.GridView2.Rows[i].Cells["Col_txtmat_amount_phurchase"].Value.ToString())) < Convert.ToDouble(string.Format("{0:n0}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value.ToString())))
                {
                    GridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    GridView2.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    GridView2.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
               else if (Convert.ToDouble(string.Format("{0:n0}", this.GridView2.Rows[i].Cells["Col_txtmat_amount_phurchase"].Value.ToString())) == Convert.ToDouble(string.Format("{0:n0}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value.ToString())))
                {
                    GridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    GridView2.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    GridView2.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
                else
                {
                    GridView2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    GridView2.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    GridView2.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
            }
        }
        private void GridView2_Color_Column()
        {

            for (int i = 0; i < this.GridView2.Rows.Count - 0; i++)
            {

                GridView2.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView2.Rows[i].Cells["Col_txtmat_name"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(0, 195, 0);
                GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Style.ForeColor = Color.FromArgb(0, 0, 0);

            }
        }
        private void GridView2_UP_Status()
        {
            for (int i = 0; i < this.GridView2.Rows.Count; i++)
            {
                if (this.GridView2.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                {
                    this.GridView2.Rows[i].Cells["Col_Chk1"].Value = true;
                }
                else
                {
                    this.GridView2.Rows[i].Cells["Col_Chk1"].Value = false;

                }
                if (this.GridView2.Rows[i].Cells["Col_txtmat_status"].Value.ToString() == "0")  //
                {
                    this.GridView2.Rows[i].Cells["Col_Chk2"].Value = true;
                }
                else
                {
                    this.GridView2.Rows[i].Cells["Col_Chk2"].Value = false;

                }
            }
        }
        private void GridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView2.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    W_ID_Select.TRANS_ID = row.Cells[1].Value.ToString();
                    W_ID_Select.MAT_ID = row.Cells[3].Value.ToString();

                }
                //=====================
                Fill_Show_DATA_GridView3();
            }
        }
        private void GridView2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }

        private void Fill_Show_DATA_GridView3()
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

            Clear_GridView3();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT k021_mat_average_balance.*," +
                                    "b001mat_02detail.*," +
                                   "b001_05mat_unit1.*," +
                                   "b001_05mat_unit2.*," +
                                   "b001_05mat_unit3.*," +
                                   "b001_05mat_unit4.*," +
                                   "b001_05mat_unit5.*" +

                                   " FROM k021_mat_average_balance" +

                                   " INNER JOIN b001mat_02detail" +
                                   " ON k021_mat_average_balance.cdkey = b001mat_02detail.cdkey" +
                                   " AND k021_mat_average_balance.txtco_id = b001mat_02detail.txtco_id" +
                                   " AND k021_mat_average_balance.txtmat_id = b001mat_02detail.txtmat_id" +

                                   " INNER JOIN b001_05mat_unit1" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                   " INNER JOIN b001_05mat_unit2" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                   " INNER JOIN b001_05mat_unit3" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit3.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit3.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit3_id = b001_05mat_unit3.txtmat_unit3_id" +

                                   " INNER JOIN b001_05mat_unit4" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit4.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit4.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit4_id = b001_05mat_unit4.txtmat_unit4_id" +

                                   " INNER JOIN b001_05mat_unit5" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit5.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit5.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit5_id = b001_05mat_unit5.txtmat_unit5_id" +

                                   " WHERE (k021_mat_average_balance.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k021_mat_average_balance.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k021_mat_average_balance.txtwherehouse_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                    " AND (k021_mat_average_balance.txtmat_id = '" + W_ID_Select.MAT_ID.Trim() + "')" +
                                  " AND (k021_mat_average_balance.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY k021_mat_average_balance.ID ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.dtpend.Value;

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
                            //this.GridView3.ColumnCount = 26;
                            //this.GridView3.Columns[0].Name = "Col_Auto_num";

                            //this.GridView3.Columns[1].Name = "Col_txttrans_date_server";
                            //this.GridView3.Columns[2].Name = "Col_txttrans_time";

                            //this.GridView3.Columns[3].Name = "Col_txtbill_id";
                            //this.GridView3.Columns[4].Name = "Col_txtbill_type";
                            //this.GridView3.Columns[5].Name = "Col_txtbill_remark";

                            //this.GridView3.Columns[6].Name = "Col_txtwherehouse_id";
                            //this.GridView3.Columns[7].Name = "Col_txtmat_no";
                            //this.GridView3.Columns[8].Name = "Col_txtmat_id";
                            //this.GridView3.Columns[9].Name = "Col_txtmat_name";
                            //this.GridView3.Columns[10].Name = "Col_txtmat_unit1_name";
                            //this.GridView3.Columns[11].Name = "Col_txtmat_unit1_qty";

                            //this.GridView3.Columns[12].Name = "Col_chmat_unit_status";

                            //this.GridView3.Columns[13].Name = "Col_txtmat_unit2_name";
                            //this.GridView3.Columns[14].Name = "Col_txtmat_unit2_qty";

                            //this.GridView3.Columns[15].Name = "Col_txtqty_in";
                            //this.GridView3.Columns[16].Name = "Col_txtqty2_in";
                            //this.GridView3.Columns[17].Name = "Col_txtprice_in";
                            //this.GridView3.Columns[18].Name = "Col_txtsum_total_in";

                            //this.GridView3.Columns[19].Name = "Col_txtqty_out";
                            //this.GridView3.Columns[20].Name = "Col_txtprice_out";
                            //this.GridView3.Columns[21].Name = "Col_txtsum_total_out";

                            //this.GridView3.Columns[22].Name = "Col_txtqty_balance";
                            //this.GridView3.Columns[23].Name = "Col_txtprice_balance";
                            //this.GridView3.Columns[24].Name = "Col_txtsum_total_balance";

                            //this.GridView3.Columns[25].Name = "Col_txtitem_no";

                            var index = GridView3.Rows.Add();
                            GridView3.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView3.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);      //1
                            GridView3.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //2

                            GridView3.Rows[index].Cells["Col_txtbill_id"].Value = dt2.Rows[j]["txtbill_id"].ToString();      //3
                            GridView3.Rows[index].Cells["Col_txtbill_type"].Value = dt2.Rows[j]["txtbill_type"].ToString();      //4
                            GridView3.Rows[index].Cells["Col_txtbill_remark"].Value = dt2.Rows[j]["txtbill_remark"].ToString();      //5

                            GridView3.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //6
                            GridView3.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //7
                            GridView3.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            GridView3.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            GridView3.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //10
                            GridView3.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");        //11
                            GridView3.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //12
                            GridView3.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //13
                            GridView3.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.00");        //14
                            GridView3.Rows[index].Cells["Col_txtmat_unit3_name"].Value = dt2.Rows[j]["txtmat_unit3_name"].ToString();      //13
                            GridView3.Rows[index].Cells["Col_txtmat_unit4_name"].Value = dt2.Rows[j]["txtmat_unit4_name"].ToString();      //13
                            GridView3.Rows[index].Cells["Col_txtmat_unit5_name"].Value = dt2.Rows[j]["txtmat_unit5_name"].ToString();      //13

                            GridView3.Rows[index].Cells["Col_txtqty_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_in"]).ToString("###,###.00");      //15
                            GridView3.Rows[index].Cells["Col_txtqty2_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2_in"]).ToString("###,###.00");      //16
                            GridView3.Rows[index].Cells["Col_txtqty_krasob_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_krasob_in"]).ToString("###,###.00");      //16
                            GridView3.Rows[index].Cells["Col_txtqty_lod_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_lod_in"]).ToString("###,###.00");      //16
                            GridView3.Rows[index].Cells["Col_txtqty_pub_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub_in"]).ToString("###,###.00");      //16
                            GridView3.Rows[index].Cells["Col_txtprice_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice_in"]).ToString("###,###.00");      //17
                            GridView3.Rows[index].Cells["Col_txtsum_total_in"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total_in"]).ToString("###,###.00");      //18

                            GridView3.Rows[index].Cells["Col_txtqty_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_out"]).ToString("###,###.00");      //19
                            GridView3.Rows[index].Cells["Col_txtqty2_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2_out"]).ToString("###,###.00");      //19
                            GridView3.Rows[index].Cells["Col_txtqty_krasob_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_krasob_out"]).ToString("###,###.00");      //19
                            GridView3.Rows[index].Cells["Col_txtqty_lod_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_lod_out"]).ToString("###,###.00");      //19
                            GridView3.Rows[index].Cells["Col_txtqty_pub_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub_out"]).ToString("###,###.00");      //19
                            GridView3.Rows[index].Cells["Col_txtprice_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice_out"]).ToString("###,###.00");      //20
                            GridView3.Rows[index].Cells["Col_txtsum_total_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total_out"]).ToString("###,###.00");      //21

                            GridView3.Rows[index].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_balance"]).ToString("###,###.00");      //22
                            GridView3.Rows[index].Cells["Col_txtqty2_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2_balance"]).ToString("###,###.00");      //22
                            GridView3.Rows[index].Cells["Col_txtqty_krasob_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_krasob_balance"]).ToString("###,###.00");      //22
                            GridView3.Rows[index].Cells["Col_txtqty_lod_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_lod_balance"]).ToString("###,###.00");      //22
                            GridView3.Rows[index].Cells["Col_txtqty_pub_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub_balance"]).ToString("###,###.00");      //22
                            GridView3.Rows[index].Cells["Col_txtprice_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice_balance"]).ToString("###,###.00");      //23
                            GridView3.Rows[index].Cells["Col_txtsum_total_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total_balance"]).ToString("###,###.00");      //24

                            GridView3.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //25

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
            GridView3_Color_Column();

        }
        private void Show_GridView3()
        {
            this.GridView3.ColumnCount = 40;
            this.GridView3.Columns[0].Name = "Col_Auto_num";

            this.GridView3.Columns[1].Name = "Col_txttrans_date_server";
            this.GridView3.Columns[2].Name = "Col_txttrans_time";

            this.GridView3.Columns[3].Name = "Col_txtbill_id";
            this.GridView3.Columns[4].Name = "Col_txtbill_type";
            this.GridView3.Columns[5].Name = "Col_txtbill_remark";

            this.GridView3.Columns[6].Name = "Col_txtwherehouse_id";
            this.GridView3.Columns[7].Name = "Col_txtmat_no";
            this.GridView3.Columns[8].Name = "Col_txtmat_id";
            this.GridView3.Columns[9].Name = "Col_txtmat_name";
            this.GridView3.Columns[10].Name = "Col_txtmat_unit1_name";
            this.GridView3.Columns[11].Name = "Col_txtmat_unit1_qty";

            this.GridView3.Columns[12].Name = "Col_chmat_unit_status";

            this.GridView3.Columns[13].Name = "Col_txtmat_unit2_name";
            this.GridView3.Columns[14].Name = "Col_txtmat_unit2_qty";
            this.GridView3.Columns[15].Name = "Col_txtmat_unit3_name";
            this.GridView3.Columns[16].Name = "Col_txtmat_unit4_name";
            this.GridView3.Columns[17].Name = "Col_txtmat_unit5_name";

            this.GridView3.Columns[18].Name = "Col_txtqty_in";
            this.GridView3.Columns[19].Name = "Col_txtqty2_in";
            this.GridView3.Columns[20].Name = "Col_txtqty_krasob_in";
            this.GridView3.Columns[21].Name = "Col_txtqty_lod_in";
            this.GridView3.Columns[22].Name = "Col_txtqty_pub_in";
            this.GridView3.Columns[23].Name = "Col_txtprice_in";
            this.GridView3.Columns[24].Name = "Col_txtsum_total_in";

            this.GridView3.Columns[25].Name = "Col_txtqty_out";
            this.GridView3.Columns[26].Name = "Col_txtqty2_out";
            this.GridView3.Columns[27].Name = "Col_txtqty_krasob_out";
            this.GridView3.Columns[28].Name = "Col_txtqty_lod_out";
            this.GridView3.Columns[29].Name = "Col_txtqty_pub_out";
            this.GridView3.Columns[30].Name = "Col_txtprice_out";
            this.GridView3.Columns[31].Name = "Col_txtsum_total_out";

            this.GridView3.Columns[32].Name = "Col_txtqty_balance";
            this.GridView3.Columns[33].Name = "Col_txtqty2_balance";
            this.GridView3.Columns[34].Name = "Col_txtqty_krasob_balance";
            this.GridView3.Columns[35].Name = "Col_txtqty_lod_balance";
            this.GridView3.Columns[36].Name = "Col_txtqty_pub_balance";
            this.GridView3.Columns[37].Name = "Col_txtprice_balance";
            this.GridView3.Columns[38].Name = "Col_txtsum_total_balance";

            this.GridView3.Columns[39].Name = "Col_txtitem_no";


            this.GridView3.Columns[0].HeaderText = "No";
            this.GridView3.Columns[1].HeaderText = "วันที่";
            this.GridView3.Columns[2].HeaderText = "เวลา";

            this.GridView3.Columns[3].HeaderText = "เลขที่เอกสาร";
            this.GridView3.Columns[4].HeaderText = "ประเภท";
            this.GridView3.Columns[5].HeaderText = "หมายเหตุ";

            this.GridView3.Columns[6].HeaderText = "รหัสคลัง";
            this.GridView3.Columns[7].HeaderText = "ลำดับ";
            this.GridView3.Columns[8].HeaderText = "รหัส";
            this.GridView3.Columns[9].HeaderText = " ชื่อสินค้า";
            this.GridView3.Columns[10].HeaderText = "หน่วยหลัก";
            this.GridView3.Columns[11].HeaderText = "หน่วย";
            this.GridView3.Columns[12].HeaderText = "แปลง";
            this.GridView3.Columns[13].HeaderText = "หน่วย(2)";
            this.GridView3.Columns[14].HeaderText = "หน่วย";
            this.GridView3.Columns[15].HeaderText = "หน่วย(3)";
            this.GridView3.Columns[16].HeaderText = "หน่วย(4)";
            this.GridView3.Columns[17].HeaderText = "หน่วย(5)";


            this.GridView3.Columns[18].HeaderText = "รับ";
            this.GridView3.Columns[19].HeaderText = "รับ(2)";
            this.GridView3.Columns[20].HeaderText = "รับ(3)";
            this.GridView3.Columns[21].HeaderText = "รับ(4)";
            this.GridView3.Columns[22].HeaderText = "รับ(5)";
            this.GridView3.Columns[23].HeaderText = "ราคา";
            this.GridView3.Columns[24].HeaderText = "จำนวนเงิน";

            this.GridView3.Columns[25].HeaderText = "จ่าย";
            this.GridView3.Columns[26].HeaderText = "จ่าย(2)";
            this.GridView3.Columns[27].HeaderText = "จ่าย(3)";
            this.GridView3.Columns[28].HeaderText = "จ่าย(4)";
            this.GridView3.Columns[29].HeaderText = "จ่าย(5)";
            this.GridView3.Columns[30].HeaderText = "ราคา";
            this.GridView3.Columns[31].HeaderText = "จำนวนเงิน";

            this.GridView3.Columns[32].HeaderText = "คงเหลือ";
            this.GridView3.Columns[33].HeaderText = "คงเหลือ(2)";
            this.GridView3.Columns[34].HeaderText = "คงเหลือ(3)";
            this.GridView3.Columns[35].HeaderText = "คงเหลือ(4)";
            this.GridView3.Columns[36].HeaderText = "คงเหลือ(5)";
            this.GridView3.Columns[37].HeaderText = "ราคา";
            this.GridView3.Columns[38].HeaderText = "จำนวนเงิน";

            this.GridView3.Columns[39].HeaderText = "ลำดับ";


            this.GridView3.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";

            this.GridView3.Columns["Col_txttrans_date_server"].Visible = true;  //"วันที่";
            this.GridView3.Columns["Col_txttrans_date_server"].Width = 80;
            this.GridView3.Columns["Col_txttrans_date_server"].ReadOnly = true;
            this.GridView3.Columns["Col_txttrans_date_server"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txttrans_date_server"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txttrans_date_server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView3.Columns["Col_txttrans_time"].Visible = true;  //"เวลา";
            this.GridView3.Columns["Col_txttrans_time"].Width = 60;
            this.GridView3.Columns["Col_txttrans_time"].ReadOnly = true;
            this.GridView3.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txttrans_time"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtbill_id"].Visible = true;  //"Col_txtbill_id";
            this.GridView3.Columns["Col_txtbill_id"].Width = 140;
            this.GridView3.Columns["Col_txtbill_id"].ReadOnly = true;
            this.GridView3.Columns["Col_txtbill_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtbill_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtbill_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView3.Columns["Col_txtbill_type"].Visible = false;  //"Col_txtbill_type";
            this.GridView3.Columns["Col_txtbill_type"].Width = 0;
            this.GridView3.Columns["Col_txtbill_type"].ReadOnly = true;
            this.GridView3.Columns["Col_txtbill_type"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtbill_type"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtbill_type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtbill_remark"].Visible = true;  //"Col_txtbill_remark";
            this.GridView3.Columns["Col_txtbill_remark"].Width = 120;
            this.GridView3.Columns["Col_txtbill_remark"].ReadOnly = true;
            this.GridView3.Columns["Col_txtbill_remark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtbill_remark"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtbill_remark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtwherehouse_id"].Visible = true;  //"Col_txtwherehouse_id";
            this.GridView3.Columns["Col_txtwherehouse_id"].Width = 80;
            this.GridView3.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView3.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtwherehouse_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView3.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";
            this.GridView3.Columns["Col_txtmat_no"].Width = 0;
            this.GridView3.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_no"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView3.Columns["Col_txtmat_id"].Width = 70;
            this.GridView3.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView3.Columns["Col_txtmat_name"].Width = 120;
            this.GridView3.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView3.Columns["Col_txtmat_unit1_name"].Width = 80;
            this.GridView3.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtmat_unit1_qty"].Visible = false;  //Col_txtmat_unit1_qty";
            this.GridView3.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView3.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView3.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView3.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView3.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_chmat_unit_status"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns["Col_txtmat_unit2_name"].Visible = true;  //"Col_txtmat_unit2_name";
            this.GridView3.Columns["Col_txtmat_unit2_name"].Width = 60;
            this.GridView3.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView3.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView3.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView3.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtmat_unit3_name"].Visible = true;  //"Col_txtmat_unit3_name";
            this.GridView3.Columns["Col_txtmat_unit3_name"].Width = 60;
            this.GridView3.Columns["Col_txtmat_unit3_name"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit3_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit3_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit3_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView3.Columns["Col_txtmat_unit4_name"].Visible = true;  //"Col_txtmat_unit4_name";
            this.GridView3.Columns["Col_txtmat_unit4_name"].Width = 60;
            this.GridView3.Columns["Col_txtmat_unit4_name"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit4_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit4_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit4_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView3.Columns["Col_txtmat_unit5_name"].Visible = true;  //"Col_txtmat_unit5_name";
            this.GridView3.Columns["Col_txtmat_unit5_name"].Width = 60;
            this.GridView3.Columns["Col_txtmat_unit5_name"].ReadOnly = true;
            this.GridView3.Columns["Col_txtmat_unit5_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtmat_unit5_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtmat_unit5_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            this.GridView3.Columns["Col_txtqty_in"].Visible = true;  //"Col_txtqty_in";
            this.GridView3.Columns["Col_txtqty_in"].Width = 60;
            this.GridView3.Columns["Col_txtqty_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty2_in"].Visible = true;  //"Col_txtqty2_in";
            this.GridView3.Columns["Col_txtqty2_in"].Width = 60;
            this.GridView3.Columns["Col_txtqty2_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty2_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty2_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty2_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_krasob_in"].Visible = true;  //"Col_txtqty_krasob_in";
            this.GridView3.Columns["Col_txtqty_krasob_in"].Width = 60;
            this.GridView3.Columns["Col_txtqty_krasob_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_krasob_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_krasob_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_krasob_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_lod_in"].Visible = true;  //"Col_txtqty_lod_in";
            this.GridView3.Columns["Col_txtqty_lod_in"].Width = 60;
            this.GridView3.Columns["Col_txtqty_lod_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_lod_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_lod_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_lod_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_pub_in"].Visible = true;  //"Col_txtqty_pub_in";
            this.GridView3.Columns["Col_txtqty_pub_in"].Width = 60;
            this.GridView3.Columns["Col_txtqty_pub_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_pub_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_pub_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_pub_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtprice_in"].Visible = true;  //"Col_txtprice_in";
            this.GridView3.Columns["Col_txtprice_in"].Width = 60;
            this.GridView3.Columns["Col_txtprice_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtprice_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtprice_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtprice_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtsum_total_in"].Visible = true;  //"Col_txtsum_total_in";
            this.GridView3.Columns["Col_txtsum_total_in"].Width = 80;
            this.GridView3.Columns["Col_txtsum_total_in"].ReadOnly = true;
            this.GridView3.Columns["Col_txtsum_total_in"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtsum_total_in"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtsum_total_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_out"].Visible = true;  //"Col_txtqty_out";
            this.GridView3.Columns["Col_txtqty_out"].Width = 60;
            this.GridView3.Columns["Col_txtqty_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty2_out"].Visible = true;  //"Col_txtqty2_out";
            this.GridView3.Columns["Col_txtqty2_out"].Width = 60;
            this.GridView3.Columns["Col_txtqty2_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty2_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty2_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty2_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_krasob_out"].Visible = true;  //"Col_txtqty_krasob_out";
            this.GridView3.Columns["Col_txtqty_krasob_out"].Width = 60;
            this.GridView3.Columns["Col_txtqty_krasob_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_krasob_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_krasob_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_krasob_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_lod_out"].Visible = true;  //"Col_txtqty_lod_out";
            this.GridView3.Columns["Col_txtqty_lod_out"].Width = 60;
            this.GridView3.Columns["Col_txtqty_lod_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_lod_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_lod_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_lod_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_pub_out"].Visible = true;  //"Col_txtqty_pub_out";
            this.GridView3.Columns["Col_txtqty_pub_out"].Width = 60;
            this.GridView3.Columns["Col_txtqty_pub_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_pub_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_pub_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_pub_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtprice_out"].Visible = true;  //"Col_txtprice_out";
            this.GridView3.Columns["Col_txtprice_out"].Width = 60;
            this.GridView3.Columns["Col_txtprice_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtprice_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtprice_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtprice_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtsum_total_out"].Visible = true;  //"Col_txtsum_total_out";
            this.GridView3.Columns["Col_txtsum_total_out"].Width = 80;
            this.GridView3.Columns["Col_txtsum_total_out"].ReadOnly = true;
            this.GridView3.Columns["Col_txtsum_total_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtsum_total_out"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtsum_total_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_balance"].Visible = true;  //"Col_txtqty_balance";
            this.GridView3.Columns["Col_txtqty_balance"].Width = 100;
            this.GridView3.Columns["Col_txtqty_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty2_balance"].Visible = true;  //"Col_txtqty2_balance";
            this.GridView3.Columns["Col_txtqty2_balance"].Width = 100;
            this.GridView3.Columns["Col_txtqty2_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty2_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty2_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty2_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_krasob_balance"].Visible = true;  //"Col_txtqty_krasob_balance";
            this.GridView3.Columns["Col_txtqty_krasob_balance"].Width =100;
            this.GridView3.Columns["Col_txtqty_krasob_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_krasob_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_krasob_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_krasob_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_lod_balance"].Visible = true;  //"Col_txtqty_lod_balance";
            this.GridView3.Columns["Col_txtqty_lod_balance"].Width = 100;
            this.GridView3.Columns["Col_txtqty_lod_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_lod_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_lod_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_lod_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtqty_pub_balance"].Visible = true;  //"Col_txtqty_pub_balance";
            this.GridView3.Columns["Col_txtqty_pub_balance"].Width = 100;
            this.GridView3.Columns["Col_txtqty_pub_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtqty_pub_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtqty_pub_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtqty_pub_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtprice_balance"].Visible = true;  //"Col_txtprice_balance";
            this.GridView3.Columns["Col_txtprice_balance"].Width =60;
            this.GridView3.Columns["Col_txtprice_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtprice_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtprice_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtprice_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtsum_total_balance"].Visible = true;  //"Col_txtsum_total_balance";
            this.GridView3.Columns["Col_txtsum_total_balance"].Width = 80;
            this.GridView3.Columns["Col_txtsum_total_balance"].ReadOnly = true;
            this.GridView3.Columns["Col_txtsum_total_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns["Col_txtsum_total_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView3.Columns["Col_txtsum_total_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns["Col_txtitem_no"].Visible = false;  //"Col_txtitem_no";


            this.GridView3.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView3.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView3.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView3.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView3()
        {
            this.GridView3.Rows.Clear();
            this.GridView3.Refresh();
        }
        private void GridView3_Color_Column()
        {

            for (int i = 0; i < this.GridView3.Rows.Count - 0; i++)
            {

                GridView3.Rows[i].Cells["Col_txtbill_id"].Style.BackColor = Color.LightSkyBlue;
                GridView3.Rows[i].Cells["Col_txtbill_id"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView3.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(62, 123, 241);
                GridView3.Rows[i].Cells["Col_txtmat_name"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView3.Rows[i].Cells["Col_txtqty_in"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(0, 195, 0);
                GridView3.Rows[i].Cells["Col_txtqty_in"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView3.Rows[i].Cells["Col_txtqty_out"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(0, 195, 0);
                GridView3.Rows[i].Cells["Col_txtqty_out"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView3.Rows[i].Cells["Col_txtqty_balance"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(0, 195, 0);
                GridView3.Rows[i].Cells["Col_txtqty_balance"].Style.ForeColor = Color.FromArgb(0, 0, 0);

            }
        }
        private void GridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void GridView3_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView3.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView3_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView3.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }


        //===========================================================

        //=============================================================================================

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
                                this.GridView2.Visible = false;
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

                        this.GridView2.Visible = false;
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
                this.GridView2.Visible = true;
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
                this.GridView2.Visible = true;
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
