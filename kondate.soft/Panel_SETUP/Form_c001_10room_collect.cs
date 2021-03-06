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
    public partial class Form_c001_10room_collect : Form
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

        public Form_c001_10room_collect()
        {
            InitializeComponent();
        }

        private void Form_c001_10room_collect_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.M_COID = "KD";


            PANEL0110_ROOM_COLLECT_GridView1_room_collect();
            PANEL0110_ROOM_COLLECT_Fill_room_collect();

        }

        bool allowResize = false;

        //txtroom_collect ห้องเก็บ  =======================================================================
        private void PANEL0110_ROOM_COLLECT_Fill_room_collect()
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

            PANEL0110_ROOM_COLLECT_Clear_GridView1_room_collect();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_10room_collect" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtroom_collect_id <> '')" +
                                    " ORDER BY txtroom_collect_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtroom_collect_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtroom_collect_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtroom_collect_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtroom_collect_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtroom_collect_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtroom_collect_status";

                            var index = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Add();
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_no"].Value = dt2.Rows[j]["txtroom_collect_no"].ToString();      //1
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_id"].Value = dt2.Rows[j]["txtroom_collect_id"].ToString();      //2
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name"].Value = dt2.Rows[j]["txtroom_collect_name"].ToString();      //3
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name_eng"].Value = dt2.Rows[j]["txtroom_collect_name_eng"].ToString();      //4
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_remark"].Value = dt2.Rows[j]["txtroom_collect_remark"].ToString();      //5
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_status"].Value = dt2.Rows[j]["txtroom_collect_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_Up_Status();

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
        private void PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Count; i++)
            {
                if (this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0110_ROOM_COLLECT_GridView1_room_collect()
        {
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnCount = 7;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[0].Name = "Col_Auto_num";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].Name = "Col_txtroom_collect_no";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].Name = "Col_txtroom_collect_id";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].Name = "Col_txtroom_collect_name";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].Name = "Col_txtroom_collect_name_eng";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].Name = "Col_txtroom_collect_remark";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].Name = "Col_txtroom_collect_status";

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[0].HeaderText = "No";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].HeaderText = " รหัส";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].HeaderText = " ชื่อรหัสสี";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].HeaderText = "ชื่อรหัสสี Eng";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].HeaderText = " สถานะ";

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[0].Visible = false;  //"No";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].Visible = true;  //"Col_txtroom_collect_no";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].Width = 90;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].Visible = true;  //"Col_txtroom_collect_id";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].Width = 80;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].Visible = true;  //"Col_txtroom_collect_name";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].Width = 150;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].Visible = false;  //"Col_txtroom_collect_name_eng";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].Width = 0;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].Visible = false;  //"Col_txtroom_collect_name_remark";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].Width = 0;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].Visible = false;  //"Col_txtroom_collect_status";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].Width = 0;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns.Add(dgvCmb);

        }
        private void PANEL0110_ROOM_COLLECT_Clear_GridView1_room_collect()
        {
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Clear();
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Refresh();
        }
        private void PANEL0110_ROOM_COLLECT_txtroom_collect_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0110_ROOM_COLLECT.Visible == false)
                {
                    this.PANEL0110_ROOM_COLLECT.Visible = true;
                    this.PANEL0110_ROOM_COLLECT.Location = new Point(this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.X, this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.Y + 22);
                    this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Focus();
                }
                else
                {
                    this.PANEL0110_ROOM_COLLECT.Visible = false;
                }
        }
        private void PANEL0110_ROOM_COLLECT_btnroom_collect_Click(object sender, EventArgs e)
        {
            this.PANEL0110_ROOM_COLLECT.Width = 502;
            this.PANEL0110_ROOM_COLLECT.Height = 337;

            if (this.PANEL0110_ROOM_COLLECT.Visible == false)
            {
                this.PANEL0110_ROOM_COLLECT.Visible = true;
                this.PANEL0110_ROOM_COLLECT.BringToFront();
                this.PANEL0110_ROOM_COLLECT.Location = new Point(this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.X, this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0110_ROOM_COLLECT.Visible = false;
            }
        }
        private void PANEL0110_ROOM_COLLECT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0110_ROOM_COLLECT.Visible == false)
            {
                this.PANEL0110_ROOM_COLLECT.Visible = true;
            }
            else
            {
                this.PANEL0110_ROOM_COLLECT.Visible = false;
            }
        }
        private void PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0110_ROOM_COLLECT_txtroom_collect_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.CurrentRow.Index;

                this.PANEL0110_ROOM_COLLECT_txtroom_collect_id.Text = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Text = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Focus();
                this.PANEL0110_ROOM_COLLECT.Visible = false;
            }
        }
        private void PANEL0110_ROOM_COLLECT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0110_ROOM_COLLECT_btn_search_Click(object sender, EventArgs e)
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

            PANEL0110_ROOM_COLLECT_Clear_GridView1_room_collect();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_10room_collect" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtroom_collect_name LIKE '%" + this.PANEL0110_ROOM_COLLECT_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtroom_collect_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtroom_collect_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtroom_collect_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtroom_collect_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtroom_collect_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtroom_collect_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtroom_collect_status";

                            var index = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Add();
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_no"].Value = dt2.Rows[j]["txtroom_collect_no"].ToString();      //1
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_id"].Value = dt2.Rows[j]["txtroom_collect_id"].ToString();      //2
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name"].Value = dt2.Rows[j]["txtroom_collect_name"].ToString();      //3
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name_eng"].Value = dt2.Rows[j]["txtroom_collect_name_eng"].ToString();      //4
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_remark"].Value = dt2.Rows[j]["txtroom_collect_remark"].ToString();      //5
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_status"].Value = dt2.Rows[j]["txtroom_collect_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_Up_Status();

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
        private void PANEL0110_ROOM_COLLECT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0110_ROOM_COLLECT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0110_ROOM_COLLECT.Height = PANEL0110_ROOM_COLLECT_btnresize_low.Top + e.Y;
                this.PANEL0110_ROOM_COLLECT.Width = PANEL0110_ROOM_COLLECT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0110_ROOM_COLLECT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0110_ROOM_COLLECT_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtroom_collect ห้องเก็บ =======================================================================

    }
}
