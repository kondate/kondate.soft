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

namespace kondate.soft.SETUP_4WH
{
    public partial class Home_SETUP_Enter_4WH_04 : Form
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




        public Home_SETUP_Enter_4WH_04()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_4WH_04_Load(object sender, EventArgs e)
        {
            W_ID_Select.M_FORM_NUMBER = "S404";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มยี่ห้อสินค้าใหม่";
            this.txtmat_brand_id.ReadOnly = false;

            this.ActiveControl = this.txtmat_brand_id;

            PANEL_FORM1_GridView1();
            PANEL_FORM1_Fill_GridView1();

            this.BtnCancel_Doc.Enabled = false;
            this.BtnPrint.Enabled = false;

            Run_ID();
            CHECK_UP_NO999();
        }


        private void Run_ID()
        {
            if (this.txtmat_brand_no.Text == "")
            {
                this.txtmat_brand_no.Text = "001";
            }

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
            //เชื่อมต่อฐานข้อมูล======================================================
            Cursor.Current = Cursors.WaitCursor;
            string RID = "";
            double Rid2 = 0;
            double Rid3 = 0;

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM b001_04mat_brand" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (txtmat_brand_id <> '')" +
                                   " ORDER BY txtmat_brand_no DESC";

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

                        RID = dt2.Rows[0]["txtmat_brand_no"].ToString();      //1
                        Rid2 = Convert.ToDouble(RID);


                        Rid3 = Convert.ToDouble(string.Format("{0:n}", Rid2)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        this.txtmat_brand_no.Text = Rid3.ToString("00#");
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

                cmd2.CommandText = "SELECT *" +
                                    " FROM b001_04mat_brand" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtmat_brand_id <> '')" +
                                    " ORDER BY txtmat_brand_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_brand_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_brand_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_brand_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_brand_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_brand_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_brand_status";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_no"].Value = dt2.Rows[j]["txtmat_brand_no"].ToString();      //1
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //2
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //3
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_name_remark"].Value = dt2.Rows[j]["txtmat_brand_name_remark"].ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_status"].Value = dt2.Rows[j]["txtmat_brand_status"].ToString();      //6

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
        private void PANEL_FORM1_GridView1()
        {
            this.PANEL_FORM1_dataGridView1.ColumnCount = 7;
            this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_brand_no";
            this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_brand_id";
            this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_brand_name";
            this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_brand_name_eng";
            this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_brand_name_remark";
            this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_brand_status";

            this.PANEL_FORM1_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderText = " รหัสยี่ห้อ";
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderText = " ชื่อยี่ห้อสินค้า";
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderText = "ชื่อยี่ห้อสินค้า Eng";
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL_FORM1_dataGridView1.Columns[6].HeaderText = " สถานะ";

            this.PANEL_FORM1_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL_FORM1_dataGridView1.Columns[1].Visible = true;  //"Col_txtmat_brand_no";
            this.PANEL_FORM1_dataGridView1.Columns[1].Width = 90;
            this.PANEL_FORM1_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[2].Visible = true;  //"Col_txtmat_brand_id";
            this.PANEL_FORM1_dataGridView1.Columns[2].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[3].Visible = true;  //"Col_txtmat_brand_name";
            this.PANEL_FORM1_dataGridView1.Columns[3].Width = 200;
            this.PANEL_FORM1_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[4].Visible = true;  //"Col_txtmat_brand_name_eng";
            this.PANEL_FORM1_dataGridView1.Columns[4].Width = 200;
            this.PANEL_FORM1_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[5].Visible = true;  //"Col_txtmat_brand_name_remark";
            this.PANEL_FORM1_dataGridView1.Columns[5].Width = 250;
            this.PANEL_FORM1_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[6].Visible = false;  //"Col_txtmat_brand_status";
            this.PANEL_FORM1_dataGridView1.Columns[6].Width = 0;
            this.PANEL_FORM1_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);

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

        private void PANEL_FORM1_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_FORM1_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {

                    this.txtmat_brand_no.Text = row.Cells[1].Value.ToString();
                    this.txtmat_brand_id.Text = row.Cells[2].Value.ToString();
                    this.txtmat_brand_name.Text = row.Cells[3].Value.ToString();

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
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {

                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;

                        cmd2.CommandText = "SELECT *" +
                                            " FROM b001_04mat_brand" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                             " AND (txtmat_brand_id = '" + this.txtmat_brand_id.Text.Trim() + "')" +
                                           " ORDER BY txtmat_brand_no ASC";

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
                                    this.txtmat_brand_no.Text = dt2.Rows[j]["txtmat_brand_no"].ToString();      //1
                                    this.txtmat_brand_id.Text = dt2.Rows[j]["txtmat_brand_id"].ToString();      //2
                                    this.txtmat_brand_name.Text = dt2.Rows[j]["txtmat_brand_name"].ToString();      //3
                                    this.txtmat_brand_name_eng.Text = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //4
                                    this.txtmat_brand_name_remark.Text = dt2.Rows[j]["txtmat_brand_name_remark"].ToString();      //5
                                }
                                this.iblword_status.Text = "แก้ไขยี่ห้อสินค้า";
                                this.txtmat_brand_id.ReadOnly = true;
                                this.BtnCancel_Doc.Enabled = true;
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


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM b001_03mat_brand" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (txtmat_brand_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                   " ORDER BY txtmat_brand_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_brand_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_brand_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_brand_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_brand_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_brand_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_brand_status";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_no"].Value = dt2.Rows[j]["txtmat_brand_no"].ToString();      //1
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //2
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //3
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_name_remark"].Value = dt2.Rows[j]["txtmat_brand_name_remark"].ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtmat_brand_status"].Value = dt2.Rows[j]["txtmat_brand_status"].ToString();      //6

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
            var frm2 = new Home_SETUP_Enter_4WH_04();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มยี่ห้อสินค้าใหม่";
            this.txtmat_brand_id.ReadOnly = false;

            Run_ID();
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "N")
            {

                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิดแก้ไข";
            TRANS_LOG();

            if (this.txtmat_brand_id.Text != "")
            {
                this.iblword_status.Text = "แก้ไขยี่ห้อสินค้า";
                this.txtmat_brand_id.ReadOnly = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.txtmat_brand_no.Text == "")
            {
                MessageBox.Show("โปรดใส่ลำดับ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtmat_brand_no.Focus();
                return;
            }
            else
            {
                if (this.txtmat_brand_no.TextLength == 3)
                {
                }
                else
                {
                    MessageBox.Show("โปรดใส่ลำดับ 3 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtmat_brand_no.Focus();
                    return;
                }
            }
            if (this.txtmat_brand_id.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัสยี่ห้อ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtmat_brand_id.Focus();
                return;
            }
            if (this.txtmat_brand_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อยี่ห้อก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtmat_brand_name.Focus();
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
            if (this.iblword_status.Text.Trim() == "เพิ่มยี่ห้อสินค้าใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM b001_04mat_brand" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtmat_brand_id = '" + this.txtmat_brand_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("รหัสยี่ห้อสินค้า นี้ซ้ำ   : '" + this.txtmat_brand_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtmat_brand_id.Focus();
                        conn.Close();
                        return;
                    }
                }

                //
                conn.Close();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            //จบเชื่อมต่อฐานข้อมูล=======================================================
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มยี่ห้อสินค้าใหม่")
                    {
                        cmd2.CommandText = "INSERT INTO b001_04mat_brand(cdkey,txtco_id," +  //1
                                           "txtmat_brand_no,txtmat_brand_id," +  //2
                                           "txtmat_brand_name,txtmat_brand_name_eng," +  //3
                                           "txtmat_brand_name_remark," +  //4
                                          "txtmat_brand_status) " +  //5
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtmat_brand_no,@txtmat_brand_id," +  //2
                                           "@txtmat_brand_name,@txtmat_brand_name_eng," +  //3
                                           "@txtmat_brand_name_remark," +  //4
                                          "@txtmat_brand_status)";   //5

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_brand_no", SqlDbType.NVarChar).Value = this.txtmat_brand_no.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_id", SqlDbType.NVarChar).Value = this.txtmat_brand_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_name", SqlDbType.NVarChar).Value = this.txtmat_brand_name.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_name_eng", SqlDbType.NVarChar).Value = this.txtmat_brand_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_name_remark", SqlDbType.NVarChar).Value = this.txtmat_brand_name_remark.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();

                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขยี่ห้อสินค้า")
                    {
                        cmd2.CommandText = "UPDATE b001_04mat_brand SET " +
                                                                     "txtmat_brand_no = '" + this.txtmat_brand_no.Text.Trim() + "'," +
                                                                     "txtmat_brand_name = '" + this.txtmat_brand_name.Text.Trim() + "'," +
                                                                     "txtmat_brand_name_eng = '" + this.txtmat_brand_name_eng.Text.Trim() + "'," +
                                                                     "txtmat_brand_name_remark = '" + this.txtmat_brand_name_remark.Text.ToString() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_brand_id = '" + this.txtmat_brand_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มยี่ห้อสินค้าใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขยี่ห้อสินค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtmat_brand_id.Text = "";
                        this.txtmat_brand_name.Text = "";
                        this.txtmat_brand_name_eng.Text = "";
                        this.txtmat_brand_name_remark.Text = "";

                        PANEL_FORM1_Fill_GridView1();

                        this.iblword_status.Text = "เพิ่มยี่ห้อสินค้าใหม่";
                        this.txtmat_brand_id.ReadOnly = false;

                        Run_ID();

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
            if (W_ID_Select.M_FORM_CANCEL.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            W_ID_Select.LOG_ID = "7";
            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
            TRANS_LOG();

            this.iblword_status.Text = "ยกเลิกเอกสาร";
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
                    if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                    {
                        String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                        DateTime myDateTime = new DateTime();
                        myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", null);

                        String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                        DateTime myDateTime2 = new DateTime();
                        myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);

                        string Cancel_ID = W_ID_Select.CDKEY.Trim() + "-" + W_ID_Select.M_USERNAME.Trim() + "-" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "-" + myDateTime2.ToString("HH:mm:ss", UsaCulture);




                        cmd2.CommandText = "INSERT INTO b001_04mat_brand_cancel(cdkey,txtco_id,txtbranch_id," +  //1
                                                                                                                 //"txttrans_date," +
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                               "txtform_name,txtform_caption," +  //5
                                                "txtuser_name,txtemp_office_name," +  //6
                                               "txtlog_id,txtlog_name," +  //7
                                              "txtdocument_id,txtversion_id,txtcount,cancel_id) " +  //8
                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                                                                            //"@txttrans_date," +
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtform_name,@txtform_caption," +  //5
                                               "@txtuser_name,@txtemp_office_name," +  //6
                                               "@txtlog_id,@txtlog_name," +  //7
                                               "@txtdocument_id,@txtversion_id,@txtcount,@cancel_id)";   //8

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();


                        cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);
                        cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);


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
                        cmd2.Parameters.Add("@cancel_id", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "INSERT INTO b001_04mat_brand_cancel_detail(cdkey,txtco_id," +  //1
                                             "txtmat_brand_no,txtmat_brand_id," +  //2
                                             "txtmat_brand_name,txtmat_brand_name_eng," +  //3
                                             "txtmat_brand_name_remark," +  //4
                                            "txtmat_brand_status,cancel_id) " +  //5
                                             "VALUES (@cdkey2,@txtco_id2," +  //1
                                             "@txtmat_brand_no,@txtmat_brand_id," +  //2
                                             "@txtmat_brand_name,@txtmat_brand_name_eng," +  //3
                                             "@txtmat_brand_name_remark," +  //4
                                            "@txtmat_brand_status,@cancel_id2)";   //5

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_brand_no", SqlDbType.NVarChar).Value = this.txtmat_brand_no.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_id", SqlDbType.NVarChar).Value = this.txtmat_brand_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_name", SqlDbType.NVarChar).Value = this.txtmat_brand_name.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_name_eng", SqlDbType.NVarChar).Value = this.txtmat_brand_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_name_remark", SqlDbType.NVarChar).Value = this.txtmat_brand_name_remark.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_status", SqlDbType.NChar).Value = "0";
                        cmd2.Parameters.Add("@cancel_id2", SqlDbType.NVarChar).Value = Cancel_ID.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001_04mat_brand" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_brand_id = '" + this.txtmat_brand_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();


                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtmat_brand_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                        {
                            W_ID_Select.LOG_ID = "7";
                            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
                            TRANS_LOG();
                        }

                        MessageBox.Show("ยกเลิกเอกสาร เรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtmat_brand_id.Text = "";
                        this.txtmat_brand_name.Text = "";

                        PANEL_FORM1_Fill_GridView1();

                        this.iblword_status.Text = "เพิ่มยี่ห้อสินค้าใหม่";
                        this.txtmat_brand_id.ReadOnly = false;

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้ ยกเลิกเอกสาร", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ไม่ได้ ยกเลิกเอกสาร", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtmat_brand_id_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == (char)Keys.Enter && this.txtmat_brand_id.Text == "")
            {
                this.txtmat_brand_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_brand_id.Text.Trim() != "")
            {
                this.txtmat_brand_no.Focus();

            }

        }

        private void txtmat_brand_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_brand_no.Text == "")
            {
                this.txtmat_brand_no.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_brand_no.Text.Trim() != "")
            {
                if (this.txtmat_brand_no.TextLength == 3)
                {
                    this.txtmat_brand_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่ลำดับให้ครับ  3 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtmat_brand_no.Focus();
                    return;
                }
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_brand_no.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_brand_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtmat_brand_name_eng.Focus();

        }

        private void txtmat_brand_name_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtmat_brand_name_remark.Focus();

        }

        private void txtmat_brand_name_remark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.BtnSave.Focus();
        }

        private void CHECK_UP_NO999()
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

            string OK = "";

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                    " FROM b001_04mat_brand" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtmat_brand_id = '')" +
                                    " ORDER BY txtmat_brand_no ASC";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    OK = "Y";
                    conn.Close();
                    return;
                }
            }

            //
            conn.Close();
            //END เชื่อมต่อฐานข้อมูล=======================================================

            if (OK.Trim() != "Y")
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
                        cmd2.CommandText = "INSERT INTO b001_04mat_brand(cdkey,txtco_id," +  //1
                                           "txtmat_brand_no,txtmat_brand_id," +  //2
                                           "txtmat_brand_name,txtmat_brand_name_eng," +  //3
                                           "txtmat_brand_name_remark," +  //4
                                          "txtmat_brand_status) " +  //5
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtmat_brand_no,@txtmat_brand_id," +  //2
                                           "@txtmat_brand_name,@txtmat_brand_name_eng," +  //3
                                           "@txtmat_brand_name_remark," +  //4
                                          "@txtmat_brand_status)";   //5

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_brand_no", SqlDbType.NVarChar).Value = "999";
                        cmd2.Parameters.Add("@txtmat_brand_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtmat_brand_name", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtmat_brand_name_eng", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtmat_brand_name_remark", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtmat_brand_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();




                        trans.Commit();
                        conn.Close();

                        Cursor.Current = Cursors.Default;
                    }
                    //MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    catch (SqlException)
                    {
                        return;
                    }
                    //END เชื่อมต่อฐานข้อมูล=======================================================
                }
                //=============================================================

            }


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
                                this.PANEL_FORM1.Visible = false;
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

                        this.PANEL_FORM1.Visible = false;
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
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.M_FORM_GRID = "Y";
                W_ID_Select.M_FORM_NEW = "Y";
                W_ID_Select.M_FORM_OPEN = "Y";
                W_ID_Select.M_FORM_PRINT = "Y";
                W_ID_Select.M_FORM_CANCEL = "Y";
                this.PANEL_FORM1.Visible = true;
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
                    myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", null);

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
