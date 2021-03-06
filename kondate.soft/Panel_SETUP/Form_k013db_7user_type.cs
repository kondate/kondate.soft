﻿using System;
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
    public partial class Form_k013db_7user_type : Form
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


        public Form_k013db_7user_type()
        {
            InitializeComponent();
        }

        private void Form_k013db_7user_type_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();

            PANEL37_USER_TYPE_GridView1_user_type();
            PANEL37_USER_TYPE_Fill_user_type();

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
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].Visible = true;  //"Col_txtuser_type_name";
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].Width = 150;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].ReadOnly = true;
            this.PANEL37_USER_TYPE_dataGridView1_user_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                    this.PANEL37_USER_TYPE.Location = new Point(116, this.PANEL37_USER_TYPE_txtuser_type_name.Location.Y + 22);
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
                this.PANEL37_USER_TYPE.Location = new Point(116, this.PANEL37_USER_TYPE_txtuser_type_name.Location.Y + 22);
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
        bool allowResize = false;
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


        //==============================
    }
}
