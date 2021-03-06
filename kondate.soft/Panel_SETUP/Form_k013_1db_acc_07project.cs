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
    public partial class Form_k013_1db_acc_07project : Form
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



        public Form_k013_1db_acc_07project()
        {
            InitializeComponent();
        }

        private void Form_k013_1db_acc_07project_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();



            PANEL1307_PROJECT_GridView1_project();
            PANEL1307_PROJECT_Fill_project();

        }

        //txtproject โครงการ  =======================================================================
        private void PANEL1307_PROJECT_Fill_project()
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

            PANEL1307_PROJECT_Clear_GridView1_project();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_07project" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtproject_id <> '')" +
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
                            var index = PANEL1307_PROJECT_dataGridView1_project.Rows.Add();
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_id"].Value = dt2.Rows[j]["txtproject_id"].ToString();      //1
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name"].Value = dt2.Rows[j]["txtproject_name"].ToString();      //2
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name_eng"].Value = dt2.Rows[j]["txtproject_name_eng"].ToString();      //3
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
        private void PANEL1307_PROJECT_GridView1_project()
        {
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnCount = 4;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[0].Name = "Col_Auto_num";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].Name = "Col_txtproject_id";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].Name = "Col_txtproject_name";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].Name = "Col_txtproject_name_eng";

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[0].HeaderText = "No";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].HeaderText = "รหัส";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].HeaderText = " โครงการ ";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].HeaderText = " โครงการ  Eng";

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[0].Visible = false;  //"No";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].Visible = true;  //"Col_txtproject_id";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].Width = 100;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].ReadOnly = true;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].Visible = true;  //"Col_txtproject_name";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].Width = 150;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].ReadOnly = true;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].Visible = true;  //"Col_txtproject_name_eng";
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].Width = 150;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].ReadOnly = true;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1307_PROJECT_dataGridView1_project.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1307_PROJECT_dataGridView1_project.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1307_PROJECT_dataGridView1_project.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1307_PROJECT_dataGridView1_project.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1307_PROJECT_dataGridView1_project.EnableHeadersVisualStyles = false;

        }
        private void PANEL1307_PROJECT_Clear_GridView1_project()
        {
            this.PANEL1307_PROJECT_dataGridView1_project.Rows.Clear();
            this.PANEL1307_PROJECT_dataGridView1_project.Refresh();
        }
        private void PANEL1307_PROJECT_txtproject_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1307_PROJECT.Visible == false)
                {
                    this.PANEL1307_PROJECT.Visible = true;
                    this.PANEL1307_PROJECT.Location = new Point(this.PANEL1307_PROJECT_txtproject_name.Location.X, this.PANEL1307_PROJECT_txtproject_name.Location.Y + 22);
                    this.PANEL1307_PROJECT_dataGridView1_project.Focus();
                }
                else
                {
                    this.PANEL1307_PROJECT.Visible = false;
                }
        }
        private void PANEL1307_PROJECT_btnproject_Click(object sender, EventArgs e)
        {
            if (this.PANEL1307_PROJECT.Visible == false)
            {
                this.PANEL1307_PROJECT.Visible = true;
                this.PANEL1307_PROJECT.BringToFront();
                this.PANEL1307_PROJECT.Location = new Point(this.PANEL1307_PROJECT_txtproject_name.Location.X, this.PANEL1307_PROJECT_txtproject_name.Location.Y + 22);
            }
            else
            {
                this.PANEL1307_PROJECT.Visible = false;
            }
        }
        private void PANEL1307_PROJECT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1307_PROJECT.Visible == false)
            {
                this.PANEL1307_PROJECT.Visible = true;
            }
            else
            {
                this.PANEL1307_PROJECT.Visible = false;
            }
        }
        private void PANEL1307_PROJECT_dataGridView1_project_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1307_PROJECT_dataGridView1_project.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1307_PROJECT_txtproject_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1307_PROJECT_txtproject_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL1307_PROJECT_dataGridView1_project_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1307_PROJECT_dataGridView1_project.CurrentRow.Index;

                this.PANEL1307_PROJECT_txtproject_id.Text = PANEL1307_PROJECT_dataGridView1_project.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1307_PROJECT_txtproject_name.Text = PANEL1307_PROJECT_dataGridView1_project.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1307_PROJECT_txtproject_name.Focus();
                this.PANEL1307_PROJECT.Visible = false;
            }
        }
        private void PANEL1307_PROJECT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1307_PROJECT_btn_search_Click(object sender, EventArgs e)
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

            PANEL1307_PROJECT_Clear_GridView1_project();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_09project" +
                                    " WHERE (txtproject_name LIKE '%" + this.PANEL1307_PROJECT_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtproject_id <> '')" +
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
                            var index = PANEL1307_PROJECT_dataGridView1_project.Rows.Add();
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_id"].Value = dt2.Rows[j]["txtproject_id"].ToString();      //1
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name"].Value = dt2.Rows[j]["txtproject_name"].ToString();      //2
                            PANEL1307_PROJECT_dataGridView1_project.Rows[index].Cells["Col_txtproject_name_eng"].Value = dt2.Rows[j]["txtproject_name_eng"].ToString();      //3
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
        private void PANEL1307_PROJECT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1307_PROJECT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1307_PROJECT.Height = PANEL1307_PROJECT_btnresize_low.Top + e.Y;
                this.PANEL1307_PROJECT.Width = PANEL1307_PROJECT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1307_PROJECT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1307_PROJECT_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtproject โครงการ  =======================================================================

    }
}
