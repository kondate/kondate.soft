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

namespace kondate.soft.HOME13_Support
{
    public partial class Home13_Support_03get_repair_record : Form
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

        string SELECT_EMP = "";

        public Home13_Support_03get_repair_record()
        {
            InitializeComponent();
        }

        private void Home13_Support_03get_repair_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H1302SPAPRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "บันทึก รับแจ้งซ่อมฮาร์ดแวร์และซอฟท์แวร์";

            this.ActiveControl = this.txtproblem_detail;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();

            Fill_DATA();
            Show_GridView1();
            Run_Number_Grid();

            this.txtemp_get_noti.Text = W_ID_Select.M_EMP_OFFICE_NAME.Trim();

        }

        private void Fill_DATA()
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
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT h013support_01repair_1noti_record.*," +
                                   "h013support_02_problem.*," +
                                   "k013_1db_acc_16department.*" +

                                   " FROM h013support_01repair_1noti_record" +

                                   " INNER JOIN h013support_02_problem" +
                                   " ON h013support_01repair_1noti_record.cdkey = h013support_02_problem.cdkey" +
                                   " AND h013support_01repair_1noti_record.txtco_id = h013support_02_problem.txtco_id" +
                                   " AND h013support_01repair_1noti_record.txtproblem_id = h013support_02_problem.txtproblem_id" +


                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON h013support_01repair_1noti_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND h013support_01repair_1noti_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND h013support_01repair_1noti_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " WHERE (h013support_01repair_1noti_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (h013support_01repair_1noti_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (h013support_01repair_1noti_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (h013support_01repair_1noti_record.txtnoti_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                  " ORDER BY h013support_01repair_1noti_record.txtnoti_id ASC";

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
                            this.txtnoti_id.Text = dt2.Rows[j]["txtnoti_id"].ToString();

                            this.dtpdate_noti.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_server"].ToString());
                            this.dtpdate_noti.Format = DateTimePickerFormat.Custom;
                            this.dtpdate_noti.CustomFormat = this.dtpdate_noti.Value.ToString("dd-MM-yyyy", UsaCulture);

                            this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[j]["txtdepartment_name"].ToString();      //6
                            this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.txtmachine_number.Text = dt2.Rows[j]["txtmachine_number"].ToString();      //7

                            this.PANEL1302_PROBLEM_txtproblem_name.Text = dt2.Rows[j]["txtproblem_name"].ToString();      //8
                            this.PANEL1302_PROBLEM_txtproblem_id.Text = dt2.Rows[j]["txtproblem_id"].ToString();      //8

                            this.txtproblem_detail.Text = dt2.Rows[j]["txtproblem_detail"].ToString();      //9
                            this.txtemp_noti.Text = dt2.Rows[j]["txtemp_noti"].ToString();      //10
                            this.txtemp_noti_apporve.Text = dt2.Rows[j]["txtemp_noti_apporve"].ToString();      //11

                            this.txtapprove_notic_id.Text = dt2.Rows[j]["txtapprove_notic_id"].ToString();      //

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

        private void Run_Number_Grid()
        {
            int k = 0;
            double z = 0;
            z = Convert.ToDouble(this.txtfold_amount.Text);

            //======================================================
            for (int i = 0; i < z; i++)
            {
                k = 1 + i;
                string[] row = new string[] { k.ToString("00"),  //"txtitem_no";
                                                                                                        "",  // "txtdetail";

                                                                                                        "0",  // "txtqty";
                                                                                                        ".00",  // "txtprice";

                                                                                                        "",  // "txtspare_parts_in_department_status";
                                                                                                        "",  // "txtspare_parts_berg_wherehouse_status";

                                                                                                        ""  // "txtremark";
                                                                                                      };
                GridView1.Rows.Add(row);
            }
            //====================================================== 

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 7;
            this.GridView1.Columns[0].Name = "Col_txtitem_no";
            this.GridView1.Columns[1].Name = "Col_txtdetail";
            this.GridView1.Columns[2].Name = "Col_txtqty";
            this.GridView1.Columns[3].Name = "Col_txtprice";
            this.GridView1.Columns[4].Name = "Col_txtspare_parts_in_department_status";
            this.GridView1.Columns[5].Name = "Col_txtspare_parts_berg_wherehouse_status";
            this.GridView1.Columns[6].Name = "Col_txtremark";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "รายการ";
            this.GridView1.Columns[2].HeaderText = " จำนวนที่ใช้";
            this.GridView1.Columns[3].HeaderText = " ราคา";
            this.GridView1.Columns[4].HeaderText = " อะไหล่ในฝ่าย";
            this.GridView1.Columns[5].HeaderText = " อะไหล่เบิกคลัง";
            this.GridView1.Columns[6].HeaderText = " หมายเหตุ";

            this.GridView1.Columns["Col_txtitem_no"].Visible = true;  //"Col_txtitem_no";
            this.GridView1.Columns["Col_txtitem_no"].Width = 80;
            this.GridView1.Columns["Col_txtitem_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtitem_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtitem_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtdetail"].Visible = true;  //"Col_txtdetail";
            this.GridView1.Columns["Col_txtdetail"].Width = 500;
            this.GridView1.Columns["Col_txtdetail"].ReadOnly = false;
            this.GridView1.Columns["Col_txtdetail"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdetail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtprice"].Visible = true;  //"Col_txtprice";
            this.GridView1.Columns["Col_txtprice"].Width = 100;
            this.GridView1.Columns["Col_txtprice"].ReadOnly = false;
            this.GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 140;
            dgvCmb.DisplayIndex = 4;
            dgvCmb.HeaderText = "อะไหล่ในฝ่าย";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb);


            this.GridView1.Columns["Col_txtspare_parts_in_department_status"].Visible = false;  //"Col_txtspare_parts_in_department_status";
            this.GridView1.Columns["Col_txtspare_parts_in_department_status"].Width = 0;
            this.GridView1.Columns["Col_txtspare_parts_in_department_status"].ReadOnly = true;
            this.GridView1.Columns["Col_txtspare_parts_in_department_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtspare_parts_in_department_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb2 = new DataGridViewCheckBoxColumn();
            dgvCmb2.Name = "Col_Chk2";
            dgvCmb2.Width = 140;
            dgvCmb2.DisplayIndex = 5;
            dgvCmb2.HeaderText = "อะไหล่เบิกคลัง";
            dgvCmb2.ValueType = typeof(bool);
            dgvCmb2.ReadOnly = false;
            dgvCmb2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb2.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb2);


            this.GridView1.Columns["Col_txtspare_parts_berg_wherehouse_status"].Visible = false;  //"Col_txtspare_parts_berg_wherehouse_status";
            this.GridView1.Columns["Col_txtspare_parts_berg_wherehouse_status"].Width = 0;
            this.GridView1.Columns["Col_txtspare_parts_berg_wherehouse_status"].ReadOnly = true;
            this.GridView1.Columns["Col_txtspare_parts_berg_wherehouse_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtspare_parts_berg_wherehouse_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtremark"].Visible = true;  //"Col_txtremark";
            this.GridView1.Columns["Col_txtremark"].Width = 500;
            this.GridView1.Columns["Col_txtremark"].ReadOnly = false;
            this.GridView1.Columns["Col_txtremark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtremark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;


        }
        private void Clear_GridView1()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
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



        private void BtnNew_Click(object sender, EventArgs e)
        {

        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void BtnCancel_Doc_Click(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
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

        //txtsupplier Supplier  =======================================================================
        private void PANEL161_SUP_Fill_supplier()
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

            PANEL161_SUP_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
                //this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
                //this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
                //this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
                //this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtsupplier_status";

                cmd2.CommandText = "SELECT k016db_1supplier.*," +
                                    "k016db_2supplier_address.*" +
                                    " FROM k016db_1supplier" +

                                    " INNER JOIN k016db_2supplier_address" +
                                    " ON k016db_1supplier.cdkey = k016db_2supplier_address.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_2supplier_address.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_2supplier_address.txtsupplier_id" +

                                    " WHERE (k016db_1supplier.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k016db_1supplier.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                     " AND (k016db_1supplier.txtsupplier_id <> '')" +
                                   " ORDER BY k016db_1supplier.txtsupplier_no ASC";

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
                            var index = PANEL161_SUP_dataGridView1.Rows.Add();
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_no"].Value = dt2.Rows[j]["txtsupplier_no"].ToString();      //1
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //2
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //3
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name_eng"].Value = dt2.Rows[j]["txtsupplier_name_eng"].ToString();      //4
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_status"].Value = dt2.Rows[j]["txtsupplier_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL161_SUP_Clear_GridView1_Up_Status();

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
        private void PANEL161_SUP_GridView1_supplier()
        {
            this.PANEL161_SUP_dataGridView1.ColumnCount = 9;
            this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
            this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
            this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
            this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
            this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtsupplier_status";

            this.PANEL161_SUP_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderText = " ชื่อ Supplier";
            this.PANEL161_SUP_dataGridView1.Columns[4].HeaderText = " ชื่อ Supplier Eng";
            this.PANEL161_SUP_dataGridView1.Columns[5].HeaderText = " ผู้ติดต่อ";
            this.PANEL161_SUP_dataGridView1.Columns[6].HeaderText = " เบอร์โทร";
            this.PANEL161_SUP_dataGridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.PANEL161_SUP_dataGridView1.Columns[8].HeaderText = " สถานะ";

            this.PANEL161_SUP_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL161_SUP_dataGridView1.Columns[1].Visible = false;  //"Col_txtsupplier_no";
            this.PANEL161_SUP_dataGridView1.Columns[1].Width = 0;
            this.PANEL161_SUP_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[2].Visible = true;  //"Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[2].Width = 100;
            this.PANEL161_SUP_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[3].Visible = true;  //"Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[3].Width = 250;
            this.PANEL161_SUP_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[4].Visible = false;  //"Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[4].Width = 0;
            this.PANEL161_SUP_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[5].Visible = true;  //"Col_txtcontact_person";
            this.PANEL161_SUP_dataGridView1.Columns[5].Width = 200;
            this.PANEL161_SUP_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[6].Visible = false;  //"Col_txtcontact_person_tel";
            this.PANEL161_SUP_dataGridView1.Columns[6].Width = 0;
            this.PANEL161_SUP_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[7].Visible = true;  //"Col_txtremark";
            this.PANEL161_SUP_dataGridView1.Columns[7].Width = 300;
            this.PANEL161_SUP_dataGridView1.Columns[7].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[8].Visible = false;  //"Col_txtsupplier_status";

            this.PANEL161_SUP_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL161_SUP_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL161_SUP_dataGridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL161_SUP_dataGridView1.Columns.Add(dgvCmb);

        }
        private void PANEL161_SUP_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL161_SUP_dataGridView1.Rows.Count; i++)
            {
                if (this.PANEL161_SUP_dataGridView1.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.PANEL161_SUP_dataGridView1.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.PANEL161_SUP_dataGridView1.Rows[i].Cells[9].Value = false;

                }
            }
        }
        private void PANEL161_SUP_Clear_GridView1()
        {
            this.PANEL161_SUP_dataGridView1.Rows.Clear();
            this.PANEL161_SUP_dataGridView1.Refresh();
        }
        private void PANEL161_SUP_txtsupplier_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL161_SUP.Visible == false)
                {
                    this.PANEL161_SUP.Visible = true;
                    this.PANEL161_SUP.Location = new Point(this.PANEL161_SUP_txtsupplier_name.Location.X, this.PANEL161_SUP_txtsupplier_name.Location.Y + 22);
                    this.PANEL161_SUP_dataGridView1.Focus();
                }
                else
                {
                    this.PANEL161_SUP.Visible = false;
                }
        }
        private void PANEL161_SUP_btnsupplier_Click(object sender, EventArgs e)
        {
            SELECT_EMP = "1";
            if (this.PANEL161_SUP.Visible == false)
            {
                this.PANEL161_SUP.Visible = true;
                this.PANEL161_SUP.BringToFront();
                this.PANEL161_SUP.Location = new Point(this.PANEL161_SUP_txtsupplier_name.Location.X, this.PANEL161_SUP_txtsupplier_name.Location.Y + 22);
            }
            else
            {
                this.PANEL161_SUP.Visible = false;
            }
        }
        private void PANEL161_SUP_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL161_SUP.Visible == false)
            {
                this.PANEL161_SUP.Visible = true;
            }
            else
            {
                this.PANEL161_SUP.Visible = false;
            }
        }
        private void PANEL161_SUP_dataGridView1_supplier_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL161_SUP_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    if (SELECT_EMP.Trim() == "1")
                    {
                        this.PANEL161_SUP_txtsupplier_id.Text = row.Cells[2].Value.ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = row.Cells[3].Value.ToString();
                        //Col_txtcontact_person
                        this.txtsend_clem_by_sup_tel.Text = row.Cells["Col_txtcontact_person_tel"].Value.ToString();
                    }
                    if (SELECT_EMP.Trim() == "2")
                    {
                        this.PANEL161_SUP_txtsupplier_id2.Text = row.Cells[2].Value.ToString();
                        this.PANEL161_SUP_txtsupplier_name2.Text = row.Cells[3].Value.ToString();
                        //Col_txtcontact_person
                        this.txtsend_repair_by_sup_tel.Text = row.Cells["Col_txtcontact_person_tel"].Value.ToString();
                    }
                }
            }
        }
        private void PANEL161_SUP_dataGridView1_supplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL161_SUP_dataGridView1.CurrentRow.Index;

                this.PANEL161_SUP_txtsupplier_id.Text = PANEL161_SUP_dataGridView1.CurrentRow.Cells[1].Value.ToString();
                this.PANEL161_SUP_txtsupplier_name.Text = PANEL161_SUP_dataGridView1.CurrentRow.Cells[2].Value.ToString();
                this.PANEL161_SUP_txtsupplier_name.Focus();
                this.PANEL161_SUP.Visible = false;
            }
        }
        private void PANEL161_SUP_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL161_SUP_btn_search_Click(object sender, EventArgs e)
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

            PANEL161_SUP_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
                //this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
                //this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
                //this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
                //this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtsupplier_status";

                cmd2.CommandText = "SELECT k016db_1supplier.*," +
                                    "k016db_2supplier_address.*" +
                                    " FROM k016db_1supplier" +

                                    " INNER JOIN k016db_2supplier_address" +
                                    " ON k016db_1supplier.cdkey = k016db_2supplier_address.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_2supplier_address.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_2supplier_address.txtsupplier_id" +

                                    " WHERE (k016db_1supplier.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k016db_1supplier.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (k016db_1supplier.txtsupplier_name LIKE '%" + this.PANEL161_SUP_txtsearch.Text.Trim() + "%')" +
                                    " AND (k016db_1supplier.txtsupplier_id <> '')" +
                                   " ORDER BY k016db_1supplier.txtsupplier_no ASC";

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
                            var index = PANEL161_SUP_dataGridView1.Rows.Add();
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_no"].Value = dt2.Rows[j]["txtsupplier_no"].ToString();      //1
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //2
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //3
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name_eng"].Value = dt2.Rows[j]["txtsupplier_name_eng"].ToString();      //4
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_status"].Value = dt2.Rows[j]["txtsupplier_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL161_SUP_Clear_GridView1_Up_Status();

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
        private void PANEL161_SUP_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL161_SUP_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL161_SUP.Height = PANEL161_SUP_btnresize_low.Top + e.Y;
                this.PANEL161_SUP.Width = PANEL161_SUP_btnresize_low.Left + e.X;
            }
        }
        private void PANEL161_SUP_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL161_SUP_btnnew_Click(object sender, EventArgs e)
        {

        }

        private void PANEL161_SUP_btnsupplier2_Click(object sender, EventArgs e)
        {
            SELECT_EMP = "2";

            if (this.PANEL161_SUP.Visible == false)
            {
                this.PANEL161_SUP.Visible = true;
                this.PANEL161_SUP.BringToFront();
                this.PANEL161_SUP.Location = new Point(this.PANEL161_SUP_txtsupplier_name2.Location.X, this.PANEL161_SUP_txtsupplier_name2.Location.Y + 22);
            }
            else
            {
                this.PANEL161_SUP.Visible = false;
            }
        }

        //END txtsupplier Supplier  =======================================================================


        private void AUTO_BILL_TRANS_ID()
        {
            string TMP = "";
            string trans_Right = "";
            string trans_Right6 = "";
            double transNum = 0;
            string trans = "";
            string year2 = "";
            string year21 = "";
            string year_now = "";
            string year_now2 = "";
            string month_now = "";
            string day_now = "";


            year_now = DateTime.Now.ToString("yyyy", UsaCulture);
            year_now2 = year_now.Substring(year_now.Length - 2);

            month_now = DateTime.Now.ToString("MM", UsaCulture);
            day_now = DateTime.Now.ToString("dd", UsaCulture);

            //k006db_sale_record_trans
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
                MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้ !!  ", "Performance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //END เชื่อมต่อฐานข้อมูล=======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT *" +
                                  " FROM h013support_01repair_3get_noti_record_trans" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                  " ORDER BY txttrans_id";
                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "Y";

                        trans_Right = dt.Rows[0]["txttrans_id"].ToString();
                        trans_Right6 = trans_Right.Substring(trans_Right.Length - 6);

                        //211201-000001
                        year21 = trans_Right.Substring(trans_Right.Length - 13);
                        year2 = year21.Substring(0, 2);

                        transNum = Convert.ToDouble(string.Format("{0:n}", trans_Right6)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        trans = transNum.ToString("00000#");

                        if (year2.Trim() == year_now2.Trim())
                        {
                            TMP = "GR" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "GR" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "GR" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

                    }
                    conn.Close();
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
                this.txtget_noti_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }

        //จบส่วนตารางสำหรับบันทึก========================================================================



        //Check ADD FORM========================================================================

        //END txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================


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
                                //this.GridView1.Visible = false;
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

                        //this.GridView1.Visible = false;
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
                //this.GridView1.Visible = true;
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
                //this.GridView1.Visible = true;
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









        //=============================================================

    }
}
