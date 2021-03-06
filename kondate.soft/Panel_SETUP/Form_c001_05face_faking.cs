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
    public partial class Form_c001_05face_baking : Form
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



        public Form_c001_05face_baking()
        {
            InitializeComponent();
        }

        private void Form_c001_05face_faking_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.M_COID = "KD";


            PANEL0105_FACE_BAKING_GridView1_face_baking();
            PANEL0105_FACE_BAKING_Fill_face_baking();

        }

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
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtface_baking_name_remark";
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
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].HeaderText = " ชื่อรหัสอบหน้า";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].HeaderText = "ชื่อรหัสอบหน้า Eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].HeaderText = " สถานะ";

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].Visible = false;  //"No";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Visible = true;  //"Col_txtface_baking_no";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Width = 90;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Visible = true;  //"Col_txtface_baking_id";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Width = 80;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Visible = true;  //"Col_txtface_baking_name";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Width = 150;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Visible = false;  //"Col_txtface_baking_name_eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Visible = false;  //"Col_txtface_baking_name_remark";
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
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtface_baking_name_remark";
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
        bool allowResize = false;
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

    }
}
