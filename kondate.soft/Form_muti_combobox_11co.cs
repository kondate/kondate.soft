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
namespace kondate.soft
{
    public partial class Form_muti_combobox_co : Form
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


        public Form_muti_combobox_co()
        {
            InitializeComponent();
        }


        private void Form_muti_combobox_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();

            panel1_co_GridView1_co();
            panel1_co_Fill_CO();

        }

        //Company=======================================================================
        private void panel1_co_Fill_CO()
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

            panel1_co_Clear_GridView1_co();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k009db_business" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_status = '0')" +
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
                            var index = PANEL1_CO_dataGridView1_co.Rows.Add();
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_name"].Value = dt2.Rows[j]["txtco_name"].ToString();      //2
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_status"].Value = dt2.Rows[j]["txtco_status"].ToString();      //4
                        }
                        //=======================================================
                        PANEL1_CO_GridView1_co_Up_Status();
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
        private void panel1_co_GridView1_co()
        {
            this.PANEL1_CO_dataGridView1_co.ColumnCount = 5;
            this.PANEL1_CO_dataGridView1_co.Columns[0].Name = "Col_Auto_num";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Name = "Col_txtco_id";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Name = "Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[3].Name = "Col_txthome_id_full";
            this.PANEL1_CO_dataGridView1_co.Columns[4].Name = "Col_txtco_status";

            this.PANEL1_CO_dataGridView1_co.Columns[0].HeaderText = "No";
            this.PANEL1_CO_dataGridView1_co.Columns[1].HeaderText = "รหัสกิจการ";
            this.PANEL1_CO_dataGridView1_co.Columns[2].HeaderText = "ชื่อกิจการ";
            this.PANEL1_CO_dataGridView1_co.Columns[3].HeaderText = "ที่อยู่";  //
            this.PANEL1_CO_dataGridView1_co.Columns[4].HeaderText = "สถานะ";

            this.PANEL1_CO_dataGridView1_co.Columns[0].Visible = false;  //"No";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Visible = true;  //"Col_txtco_id";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Width = 100;
            this.PANEL1_CO_dataGridView1_co.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[2].Visible = true;  //"Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Width = 150;
            this.PANEL1_CO_dataGridView1_co.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1_CO_dataGridView1_co.Columns[3].Visible = true; // "Col_txthome_id_full
            this.PANEL1_CO_dataGridView1_co.Columns[3].Width = 250;
            this.PANEL1_CO_dataGridView1_co.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1_CO_dataGridView1_co.Columns[4].Visible = true;  // "Col_txtco_status
            this.PANEL1_CO_dataGridView1_co.Columns[4].Width = 50;
            this.PANEL1_CO_dataGridView1_co.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL1_CO_dataGridView1_co.DefaultCellStyle.Font = new Font("Tahoma", 8F);

            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1_CO_dataGridView1_co.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL1_CO_dataGridView1_co.Columns.Add(dgvCmb);

        }
        private void panel1_co_Clear_GridView1_co()
        {
            this.PANEL1_CO_dataGridView1_co.Rows.Clear();
            this.PANEL1_CO_dataGridView1_co.Refresh();
        }
        private void PANEL1_CO_GridView1_co_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL1_CO_dataGridView1_co.Rows.Count - 1; i++)
            {
                if (this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[4].Value.ToString() == "0")  //Active
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[5].Value = true;
                }
                else
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[5].Value = false;

                }
            }
        }
        private void panel1_co_btnco_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
            }
            else
            {
                this.PANEL1_CO.Visible = false;
            }
        }
         private void panel1_co_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
            }
            else
            {
                this.PANEL1_CO.Visible = false;
            }
        }
        bool allowResize = false;
        private void panel1_co_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }
        private void panel1_co_MouseMove(object sender, MouseEventArgs e)
        {

            if (allowResize)
            {
                this.PANEL1_CO.Height = this.Top + e.Y;
                this.PANEL1_CO.Width = this.Left + e.X;

            }
        }
       private void panel1_co_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void panel1_co_dataGridView1_co_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1_CO_dataGridView1_co.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1_CO_txtco_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1_CO_txtco_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void panel1_co_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void panel1_co_btn_search_Click(object sender, EventArgs e)
        {

        }
        //Company=======================================================================


        private Point _Offset = Point.Empty;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _Offset = new Point(e.X, e.Y);
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _Offset = Point.Empty;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Offset != Point.Empty)
            {
                Point newlocation = panel1.Location;
                newlocation.X += e.X - _Offset.X;
                newlocation.Y += e.Y - _Offset.Y;
                panel1.Location = newlocation;
            }
        }

        private void panel1_co_dataGridView1_co_SelectionChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in panel1_co_dataGridView1_co.SelectedRows)
            //{
            //    string value1 = row.Cells[0].Value.ToString();
            //    string value2 = row.Cells[1].Value.ToString();

            //    this.panel1_co_txtco_id.Text = value1.ToString();
            //    this.panel1_co_txtco_name.Text = value2.ToString();

            }
        }























        //=================================================================================



        //END Company====================================================================

}
