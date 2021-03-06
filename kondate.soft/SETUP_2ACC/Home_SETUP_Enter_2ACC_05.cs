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

namespace kondate.soft.SETUP_2ACC
{
    public partial class Home_SETUP_Enter_2ACC_05 : Form
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




        public Home_SETUP_Enter_2ACC_05()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_2ACC_05_Load(object sender, EventArgs e)
        {
            W_ID_Select.M_FORM_NUMBER = "S205";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.PANEL1_CO_txtco_id.Text = W_ID_Select.M_COID.Trim();
            this.PANEL1_CO_txtco_name.Text = W_ID_Select.M_CONAME.Trim();

            this.iblword_status.Text = "เพิ่มสาขาใหม่";
            this.txtbranch_id.ReadOnly = false;
            this.BtnSave.Enabled = true;

            this.ActiveControl = this.txtbranch_id;


            PANEL_FORM1_GridView1();
            PANEL_FORM1_Fill_GridView1();

            //Run_ID1();
            Run_ID2();
            CHECK_UP_NO999();
        }
        private void Run_ID1()
        {
            if (this.txtbranch_id.Text == "")
            {
                this.txtbranch_id.Text = this.PANEL1_CO_txtco_id.Text + "001";
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
            string RID = "";
            string RID1 = "";
            double Rid2 = 0;
            double Rid3 = 0;

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k008db_branch" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                    " ORDER BY txtbranch_id DESC";

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

                        RID = dt2.Rows[0]["txtbranch_id"].ToString();      //1
                        RID1 = RID.Substring(RID.Length-3);
                        //MessageBox.Show(RID);
                        //MessageBox.Show(RID1);

                        Rid2 = Convert.ToDouble(RID1);


                        Rid3 = Convert.ToDouble(string.Format("{0:n}", Rid2)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        this.txtbranch_id.Text = this.PANEL1_CO_txtco_id.Text + "" + Rid3.ToString("00#");
                        //MessageBox.Show(Rid3.ToString());

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
        private void Run_ID2()
        {
            if (this.txtbranch_id_second.Text == "")
            {
                this.txtbranch_id_second.Text = "00000";
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
                                    " FROM k008db_branch" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                    " ORDER BY txtbranch_id_second DESC";

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

                        RID = dt2.Rows[0]["txtbranch_id_second"].ToString();      //1
                        Rid2 = Convert.ToDouble(RID);


                        Rid3 = Convert.ToDouble(string.Format("{0:n}", Rid2)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        this.txtbranch_id_second.Text = Rid3.ToString("0000#");

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

            PANEL_FORM1_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k008db_branch" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtbranch_id <> '')" +
                                    " ORDER BY txtbranch_id ASC";

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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtbranch_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtbranch_id_second";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtbranch_name_short";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtbranch_tel";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txthome_id_full";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtbranch_status";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //1
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_id_second"].Value = dt2.Rows[j]["txtbranch_id_second"].ToString();      //2
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_name"].Value = dt2.Rows[j]["txtbranch_name"].ToString();      //3
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_name_short"].Value = dt2.Rows[j]["txtbranch_name_short"].ToString();      //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_tel"].Value = dt2.Rows[j]["txtbranch_tel"].ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //6
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_status"].Value = dt2.Rows[j]["txtbranch_status"].ToString();      //7

                        }

                        PANEL_FORM1_Clear_GridView1_Up_Status();



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
        private void PANEL_FORM1_GridView1()
        {
            this.PANEL_FORM1_dataGridView1.ColumnCount = 8;
            this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtbranch_id";
            this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtbranch_id_second";
            this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_name";
            this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtbranch_name_short";
            this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtbranch_tel";
            this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txthome_id_full";
            this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtbranch_status";

            this.PANEL_FORM1_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderText = "รหัส";
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderText = " รหัสสาขาย่อย";
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderText = " ชื่อสาขา";
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderText = "ชื่อย่อสาขา";
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderText = " เบอร์โทร";
            this.PANEL_FORM1_dataGridView1.Columns[6].HeaderText = " ที่อยู่";
            this.PANEL_FORM1_dataGridView1.Columns[7].HeaderText = " สถานะ";

            this.PANEL_FORM1_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL_FORM1_dataGridView1.Columns[1].Visible = true;  //"Col_txtbranch_id";
            this.PANEL_FORM1_dataGridView1.Columns[1].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[2].Visible = true;  //"Col_txtbranch_id_second";
            this.PANEL_FORM1_dataGridView1.Columns[2].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[3].Visible = true;  //"Col_txtbranch_name";
            this.PANEL_FORM1_dataGridView1.Columns[3].Width = 200;
            this.PANEL_FORM1_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[4].Visible = true;  //"Col_txtbranch_name_short";
            this.PANEL_FORM1_dataGridView1.Columns[4].Width = 150;
            this.PANEL_FORM1_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[5].Visible = true;  //"Col_txtco_tel";
            this.PANEL_FORM1_dataGridView1.Columns[5].Width = 200;
            this.PANEL_FORM1_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[6].Visible = true;  //"Col_txthome_id_full";
            this.PANEL_FORM1_dataGridView1.Columns[6].Width = 350;
            this.PANEL_FORM1_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL_FORM1_dataGridView1.Columns[7].Visible = false;  //"Col_txtco_status";
            this.PANEL_FORM1_dataGridView1.Columns[7].Width = 0;
            this.PANEL_FORM1_dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
        private void PANEL_FORM1_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_FORM1_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                    //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtbranch_id";
                    //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtbranch_id_second";
                    //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_name";
                    //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtbranch_name_short";
                    //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtbranch_tel";
                    //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txthome_id_full";
                    //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtbranch_status";


                    this.txtbranch_id.Text = row.Cells[1].Value.ToString();
                    this.txtbranch_id_second.Text = row.Cells[2].Value.ToString();
                    this.txtbranch_name.Text = row.Cells[3].Value.ToString();
                    this.txtbranch_name_short.Text = row.Cells[4].Value.ToString();
                    this.txtbranch_tel.Text = row.Cells[4].Value.ToString();
                    this.txthome_id_full.Text = row.Cells[4].Value.ToString();

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
                                            " FROM k008db_branch" +
                                            " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                             " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                             " AND (txtbranch_id = '" + this.txtbranch_id.Text.Trim() + "')" +
                                            " ORDER BY txtbranch_id ASC";

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
                                    this.txtbranch_id.Text = dt2.Rows[j]["txtbranch_id"].ToString();      //1
                                    this.txtbranch_id_second.Text = dt2.Rows[j]["txtbranch_id_second"].ToString();      //2

                                    if (dt2.Rows[j]["txthead_office_status"].ToString() == "Y")
                                    {
                                        this.checkBox1_head_office_status.Checked = true;      //3
                                    }
                                    else
                                    {
                                        this.checkBox1_head_office_status.Checked = false;      //3
                                    }
                                    this.txtbranch_name.Text = dt2.Rows[j]["txtbranch_name"].ToString();      //4
                                    this.txtbranch_name_short.Text = dt2.Rows[j]["txtbranch_name_short"].ToString();      //5
                                    this.txtbranch_name_eng.Text = dt2.Rows[j]["txtbranch_name_eng"].ToString();      //6
                                    this.txtEmail.Text = dt2.Rows[j]["txtEmail"].ToString();      //7
                                    this.txtbranch_tel.Text = dt2.Rows[j]["txtbranch_tel"].ToString();      //8
                                    this.txthome_id.Text = dt2.Rows[j]["txthome_id"].ToString();      //9
                                    this.txttambon.Text = dt2.Rows[j]["txttambon"].ToString();      //10
                                    this.txtamphur.Text = dt2.Rows[j]["txtamphur"].ToString();      //11
                                    this.txtchangwat.Text = dt2.Rows[j]["txtchangwat"].ToString();      //12
                                    this.txtpost_id.Text = dt2.Rows[j]["txtpost_id"].ToString();      //13
                                    this.txthome_id_full.Text = dt2.Rows[j]["txthome_id_full"].ToString();      //14
                                    this.txthome_id_full_eng.Text = dt2.Rows[j]["txthome_id_full_eng"].ToString();      //15
                                    this.txtremark.Text = dt2.Rows[j]["txtremark"].ToString();      //16
                                }
                                this.iblword_status.Text = "แก้ไขสาขา";
                                this.txtbranch_id.ReadOnly = true;
                                this.txtbranch_id_second.ReadOnly = true;
                                this.txtbranch_name_short.ReadOnly = true;
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

                //=================================================================================
            }
        }
        private void PANEL_FORM1_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL_FORM1_dataGridView1.Rows.Count ; i++)
            {
                if (this.PANEL_FORM1_dataGridView1.Rows[i].Cells[7].Value.ToString() == "0")  //Active
                {
                    this.PANEL_FORM1_dataGridView1.Rows[i].Cells[8].Value = true;
                }
                else
                {
                    this.PANEL_FORM1_dataGridView1.Rows[i].Cells[8].Value = false;

                }
            }
        }

        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_FORM1_Fill_GridView1();
        }

        private void PANEL_FORM1_btnsearch_Click(object sender, EventArgs e)
        {

        }

        private void panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void iblword_top_MouseDown(object sender, MouseEventArgs e)
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

        private void btnmaximize_full_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.btnmaximize.Visible = true;
                this.btnmaximize_full.Visible = false;
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

        private void btnclose_Click(object sender, EventArgs e)
        {
            W_ID_Select.LOG_ID = "9";
            W_ID_Select.LOG_NAME = "ปิดหน้าจอ";
            TRANS_LOG();

            if (W_ID_Select.FROM_FORM == "HOME")
            {
                DialogResult dialogResult = MessageBox.Show("เมื่อคุณปิดหน้าจอนี้ คุณจำเป็นต้อง เข้าระบบใหม่ คุณแน่ใจแล้ว ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    Application.Exit();

                }
                else if (dialogResult == DialogResult.No)
                {
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                }
            }
            else
            {
                this.Close();
            }
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
            var frm2 = new Home_SETUP_Enter_2ACC_05();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มสาขาใหม่";
            this.txtbranch_id.ReadOnly = false;
            this.txtbranch_id_second.ReadOnly = false;
            this.txtbranch_name_short.ReadOnly = false;

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

            if (this.txtbranch_id.Text != "")
            {
                this.iblword_status.Text = "แก้ไขสาขา";
                this.txtbranch_id.ReadOnly = true;
                this.txtbranch_id_second.ReadOnly = true;
                this.txtbranch_name_short.ReadOnly = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.txtbranch_id.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัสสาขา ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbranch_id.Focus();
                return;
            }
            else
            {
                if (this.txtbranch_id.TextLength == 5)
                {
                }
                else
                {
                    MessageBox.Show("โปรดใส่รหัสสาขา  5 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbranch_id.Focus();
                    return;
                }
            }
            if (this.txtbranch_id_second.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัสสาขาย่อย ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbranch_id_second.Focus();
                return;
            }
            else
            {
                if (this.txtbranch_id_second.TextLength == 5)
                {
                }
                else
                {
                    MessageBox.Show("โปรดใส่รหัสสาขาย่อย  5 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbranch_id_second.Focus();
                    return;
                }
            }

            if (this.txtbranch_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อสาขา ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbranch_name.Focus();
                return;
            }
            if (this.txtbranch_name_short.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อย่อสาขา ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbranch_name_short.Focus();
                return;
            }

            if (this.txtbranch_name_short.Text == "")
            {
                MessageBox.Show("โปรดใส่ ชื่อย่อ สาขา ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbranch_name_short.Focus();
                return;
            }
            else
            {
                if (this.txtbranch_name_short.TextLength == 2)
                {
                }
                else
                {
                    MessageBox.Show("โปรดใส่ ชื่อย่อสาขา  2 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbranch_name_short.Focus();
                    return;
                }
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
            if (this.iblword_status.Text.Trim() == "เพิ่มสาขาใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;


                    cmd1.CommandText = "SELECT * FROM k008db_branch" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                      " AND (txtbranch_id = '" + this.txtbranch_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("รหัสสาขา นี้ซ้ำ   : '" + this.txtbranch_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtbranch_id.Focus();
                        conn.Close();
                        return;
                    }



                    cmd1.CommandText = "SELECT * FROM k008db_branch" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                      " AND (txtbranch_id = '" + this.txtbranch_id.Text.Trim() + "')" +
                                      " AND (txtbranch_id_second = '" + this.txtbranch_id_second.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd1);
                    da2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        MessageBox.Show("รหัสสาขาย่อย นี้ซ้ำ   : '" + this.txtbranch_id_second.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtbranch_id.Focus();
                        conn.Close();
                        return;
                    }


                    cmd1.CommandText = "SELECT * FROM k008db_branch" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                      " AND (txtbranch_id = '" + this.txtbranch_id.Text.Trim() + "')" +
                                      " AND (txtbranch_name_short = '" + this.txtbranch_name_short.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt3 = new DataTable();
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd1);
                    da3.Fill(dt3);
                    if (dt3.Rows.Count > 0)
                    {
                        MessageBox.Show("ชื่อย่อ สาขา ซ้ำ !   : '" + this.txtbranch_name_short.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtbranch_id.Focus();
                        conn.Close();
                        return;
                    }



                }

                //
                conn.Close();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มสาขาใหม่")
                    {
                        cmd2.CommandText = "INSERT INTO k008db_branch(cdkey,txtco_id," +  //1
                                           "txtbranch_id,txtbranch_id_second,txthead_office_status," +  //2
                                           "txtbranch_name,txtbranch_name_short,txtbranch_name_eng," +  //3
                                           "txtEmail,txtbranch_tel," +  //4

                                           "txthome_id,txttambon," +  //5
                                           "txtamphur,txtchangwat," +  //6
                                            "txtpost_id," +  //7

                                           "txtbranch_status,txtuser_name," +  //8
                                           "txthome_id_full," +  //9
                                           "txthome_id_full_eng," +  //10
                                          "txtremark) " +  //11
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtbranch_id,@txtbranch_id_second,@txthead_office_status," +  //2
                                           "@txtbranch_name,@txtbranch_name_short,@txtbranch_name_eng," +  //3
                                           "@txtEmail,@txtbranch_tel," +  //4

                                           "@txthome_id,@txttambon," +  //5
                                           "@txtamphur,@txtchangwat," +  //6
                                            "@txtpost_id," +  //7

                                           "@txtbranch_status,@txtuser_name," +  //8
                                           "@txthome_id_full," +  //9
                                           "@txthome_id_full_eng," +  //10
                                           "@txtremark)";   //11

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = this.PANEL1_CO_txtco_id.Text.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = this.txtbranch_id.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_id_second", SqlDbType.NVarChar).Value = this.txtbranch_id_second.Text.Trim();
                        if (this.checkBox1_head_office_status.Checked == true)
                        {
                            cmd2.Parameters.Add("@txthead_office_status", SqlDbType.NVarChar).Value = "Y";
                        }
                        else
                        {
                            cmd2.Parameters.Add("@txthead_office_status", SqlDbType.NVarChar).Value = "N";
                        }
                        cmd2.Parameters.Add("@txtbranch_name", SqlDbType.NVarChar).Value = this.txtbranch_name.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_name_short", SqlDbType.NVarChar).Value = this.txtbranch_name_short.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_name_eng", SqlDbType.NVarChar).Value = this.txtbranch_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtEmail", SqlDbType.NVarChar).Value = this.txtEmail.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_tel", SqlDbType.NVarChar).Value = this.txtbranch_tel.Text.ToString();

                        cmd2.Parameters.Add("@txthome_id", SqlDbType.NVarChar).Value = this.txthome_id.Text.ToString();
                        cmd2.Parameters.Add("@txttambon", SqlDbType.NVarChar).Value = this.txttambon.Text.ToString();
                        cmd2.Parameters.Add("@txtamphur", SqlDbType.NVarChar).Value = this.txtamphur.Text.ToString();
                        cmd2.Parameters.Add("@txtchangwat", SqlDbType.NVarChar).Value = this.txtchangwat.Text.ToString();
                        cmd2.Parameters.Add("@txtpost_id", SqlDbType.NVarChar).Value = this.txtpost_id.Text.ToString();

                        cmd2.Parameters.Add("@txtbranch_status", SqlDbType.NChar).Value = "0";
                        cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = W_ID_Select.M_USERNAME.Trim();
                        //cmd2.Parameters.Add("@txthome_id_full", SqlDbType.NVarChar).Value = this.txthome_id.Text.ToString() + "  ตำบล" + this.txttambon.Text.ToString() + "  อำเภอ" + this.txtamphur.Text.ToString() + "  จังหวัด" + this.txtchangwat.Text.ToString() + " " + this.txtpost_id.Text.ToString() + "";
                        cmd2.Parameters.Add("@txthome_id_full", SqlDbType.NVarChar).Value = this.txthome_id_full.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id_full_eng", SqlDbType.NVarChar).Value = this.txthome_id_full_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtremark", SqlDbType.NVarChar).Value = this.txtremark.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขสาขา")
                    {
                        cmd2.CommandText = "UPDATE k008db_branch SET " +
                                                                     "txtbranch_name = '" + this.txtbranch_name.Text.Trim() + "'," +
                                                                     "txtbranch_name_short = '" + this.txtbranch_name_short.Text.Trim() + "'," +
                                                                     "txtbranch_name_eng = '" + this.txtbranch_name_eng.Text.Trim() + "'," +
                                                                     "txtEmail = '" + this.txtEmail.Text.Trim() + "'," +
                                                                     "txtbranch_tel = '" + this.txtbranch_tel.Text.Trim() + "'," +
                                                                     "txthome_id = '" + this.txthome_id.Text.Trim() + "'," +
                                                                     "txttambon = '" + this.txttambon.Text.Trim() + "'," +
                                                                     "txtamphur = '" + this.txtamphur.Text.Trim() + "'," +
                                                                     "txtchangwat = '" + this.txtchangwat.Text.Trim() + "'," +
                                                                      "txtpost_id = '" + this.txtpost_id.Text.Trim() + "'," +
                                                                     "txtuser_name = '" + W_ID_Select.M_USERNAME.Trim() + "'," +
                                                                     "txthome_id_full = '" + this.txthome_id_full.Text.Trim() + "'," +
                                                                     "txthome_id_full_eng = '" + this.txthome_id_full_eng.Text.Trim() + "'," +
                                                                    "txtremark = '" + this.txtremark.Text.ToString() + "'" +
                                                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                     " AND (txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                                                     " AND (txtbranch_id = '" + this.txtbranch_id.Text.Trim() + "')";


                        cmd2.ExecuteNonQuery();

                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มสาขาใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขสาขา")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtbranch_id.Text = "";
                        this.txtbranch_id_second.Text = "";
                        this.txtbranch_name.Text = "";
                        this.txtbranch_name_short.Text = "";
                        this.txtbranch_name_eng.Text = "";
                        this.txtEmail.Text = "";
                        this.txtbranch_tel.Text = "";
                        this.txthome_id.Text = "";
                        this.txttambon.Text = "";
                        this.txtamphur.Text = "";
                        this.txtchangwat.Text = "";
                        this.txtpost_id.Text = "";
                        this.txthome_id_full.Text = "";
                        this.txthome_id_full_eng.Text = "";
                        this.txtremark.Text = "";

                        PANEL_FORM1_Fill_GridView1();

                        this.iblword_status.Text = "เพิ่มสาขาใหม่";
                        this.txtbranch_id.ReadOnly = false;
                        this.txtbranch_id_second.ReadOnly = false;
                        this.txtbranch_name_short.ReadOnly = false;

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            W_ID_Select.LOG_ID = "9";
            W_ID_Select.LOG_NAME = "ปิดหน้าจอ";
            TRANS_LOG();

            if (W_ID_Select.FROM_FORM == "HOME")
            {
                DialogResult dialogResult = MessageBox.Show("เมื่อคุณปิดหน้าจอนี้ คุณจำเป็นต้อง เข้าระบบใหม่ คุณแน่ใจแล้ว ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    Application.Exit();

                }
                else if (dialogResult == DialogResult.No)
                {
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                }
            }
            else
            {
                this.Close();
            }
        }

        private void txtbranch_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
       && !char.IsDigit(e.KeyChar)
       && e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
       && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
       && e.KeyChar != '/'
        && e.KeyChar != '_'
       //&& e.KeyChar != 'a' && e.KeyChar != 'b' && e.KeyChar != 'c' && e.KeyChar != 'd' && e.KeyChar != 'e' && e.KeyChar != 'f' && e.KeyChar != 'g' && e.KeyChar != 'h' && e.KeyChar != 'i' && e.KeyChar != 'j'
       //&& e.KeyChar != 'k' && e.KeyChar != 'l' && e.KeyChar != 'm' && e.KeyChar != 'n' && e.KeyChar != 'o' && e.KeyChar != 'p' && e.KeyChar != 'q' && e.KeyChar != 'r' && e.KeyChar != 's'
       //&& e.KeyChar != 't' && e.KeyChar != 'u' && e.KeyChar != 'v' && e.KeyChar != 'w' && e.KeyChar != 'x' && e.KeyChar != 'y' && e.KeyChar != 'z'
       && e.KeyChar != 'A' && e.KeyChar != 'B' && e.KeyChar != 'C' && e.KeyChar != 'D' && e.KeyChar != 'E' && e.KeyChar != 'F' && e.KeyChar != 'G' && e.KeyChar != 'H' && e.KeyChar != 'I' && e.KeyChar != 'J'
       && e.KeyChar != 'K' && e.KeyChar != 'L' && e.KeyChar != 'M' && e.KeyChar != 'N' && e.KeyChar != 'O' && e.KeyChar != 'P' && e.KeyChar != 'Q' && e.KeyChar != 'R' && e.KeyChar != 'S'
       && e.KeyChar != 'T' && e.KeyChar != 'U' && e.KeyChar != 'V' && e.KeyChar != 'W' && e.KeyChar != 'X' && e.KeyChar != 'Y' && e.KeyChar != 'Z'

 )
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter && this.txtbranch_id.Text == "")
            {
                this.txtbranch_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtbranch_id.Text.Trim() != "")
            {
                if (this.txtbranch_id.TextLength == 5)
                {
                    this.txtbranch_id_second.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่รหัสสาขาให้ครบ 5 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbranch_id.Focus();
                    return;
                }
            }

        }


        private void txtbranch_id_second_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
       && !char.IsDigit(e.KeyChar)
       //&& e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
       //&& e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
       //&& e.KeyChar != '/'
       // && e.KeyChar != '_'
       //&& e.KeyChar != 'a' && e.KeyChar != 'b' && e.KeyChar != 'c' && e.KeyChar != 'd' && e.KeyChar != 'e' && e.KeyChar != 'f' && e.KeyChar != 'g' && e.KeyChar != 'h' && e.KeyChar != 'i' && e.KeyChar != 'j'
       //&& e.KeyChar != 'k' && e.KeyChar != 'l' && e.KeyChar != 'm' && e.KeyChar != 'n' && e.KeyChar != 'o' && e.KeyChar != 'p' && e.KeyChar != 'q' && e.KeyChar != 'r' && e.KeyChar != 's'
       //&& e.KeyChar != 't' && e.KeyChar != 'u' && e.KeyChar != 'v' && e.KeyChar != 'w' && e.KeyChar != 'x' && e.KeyChar != 'y' && e.KeyChar != 'z'
       //&& e.KeyChar != 'A' && e.KeyChar != 'B' && e.KeyChar != 'C' && e.KeyChar != 'D' && e.KeyChar != 'E' && e.KeyChar != 'F' && e.KeyChar != 'G' && e.KeyChar != 'H' && e.KeyChar != 'I' && e.KeyChar != 'J'
       //&& e.KeyChar != 'K' && e.KeyChar != 'L' && e.KeyChar != 'M' && e.KeyChar != 'N' && e.KeyChar != 'O' && e.KeyChar != 'P' && e.KeyChar != 'Q' && e.KeyChar != 'R' && e.KeyChar != 'S'
       //&& e.KeyChar != 'T' && e.KeyChar != 'U' && e.KeyChar != 'V' && e.KeyChar != 'W' && e.KeyChar != 'X' && e.KeyChar != 'Y' && e.KeyChar != 'Z'

 )
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter && this.txtbranch_id_second.Text == "")
            {
                this.txtbranch_id_second.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtbranch_id_second.Text.Trim() != "")
            {
                if (this.txtbranch_id_second.TextLength == 5)
                {
                    this.txtbranch_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่รหัสสาขาย่อยให้ครบ 5 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbranch_id_second.Focus();
                    return;
                }
            }
        }

        private void txtbranch_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtbranch_name_short.Focus();

        }

        private void txtbranch_name_short_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void txtbranch_name_short_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
       //&& !char.IsDigit(e.KeyChar)
       //&& e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
       //&& e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
       //&& e.KeyChar != '/'
       // && e.KeyChar != '_'
       //&& e.KeyChar != 'a' && e.KeyChar != 'b' && e.KeyChar != 'c' && e.KeyChar != 'd' && e.KeyChar != 'e' && e.KeyChar != 'f' && e.KeyChar != 'g' && e.KeyChar != 'h' && e.KeyChar != 'i' && e.KeyChar != 'j'
       //&& e.KeyChar != 'k' && e.KeyChar != 'l' && e.KeyChar != 'm' && e.KeyChar != 'n' && e.KeyChar != 'o' && e.KeyChar != 'p' && e.KeyChar != 'q' && e.KeyChar != 'r' && e.KeyChar != 's'
       //&& e.KeyChar != 't' && e.KeyChar != 'u' && e.KeyChar != 'v' && e.KeyChar != 'w' && e.KeyChar != 'x' && e.KeyChar != 'y' && e.KeyChar != 'z'
       && e.KeyChar != 'A' && e.KeyChar != 'B' && e.KeyChar != 'C' && e.KeyChar != 'D' && e.KeyChar != 'E' && e.KeyChar != 'F' && e.KeyChar != 'G' && e.KeyChar != 'H' && e.KeyChar != 'I' && e.KeyChar != 'J'
       && e.KeyChar != 'K' && e.KeyChar != 'L' && e.KeyChar != 'M' && e.KeyChar != 'N' && e.KeyChar != 'O' && e.KeyChar != 'P' && e.KeyChar != 'Q' && e.KeyChar != 'R' && e.KeyChar != 'S'
       && e.KeyChar != 'T' && e.KeyChar != 'U' && e.KeyChar != 'V' && e.KeyChar != 'W' && e.KeyChar != 'X' && e.KeyChar != 'Y' && e.KeyChar != 'Z'

 )
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter && this.txtbranch_name_short.Text == "")
            {
                this.txtbranch_name_short.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtbranch_name_short.Text.Trim() != "")
            {
                if (this.txtbranch_name_short.TextLength == 2)
                {
                    this.txtbranch_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่ชื่อย่อสาขาให้ครบ 2 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbranch_name_eng.Focus();
                    return;
                }
            }

        }

        private void txtbranch_name_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtEmail.Focus();

        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtbranch_tel.Focus();

        }

        private void txtbranch_tel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txthome_id.Focus();

        }

        private void txthome_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txttambon.Focus();

        }

        private void txttambon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtamphur.Focus();

        }

        private void txtamphur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtchangwat.Focus();

        }

        private void txtchangwat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtpost_id.Focus();

        }

        private void txtpost_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txthome_id_full.Focus();

        }

        private void txthome_id_full_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txthome_id_full_eng.Focus();

        }

        private void txthome_id_full_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtremark.Focus();

        }

        private void txtremark_KeyDown(object sender, KeyEventArgs e)
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
                                    " FROM k008db_branch" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtbranch_id = '')" +
                                    " ORDER BY txtbranch_id ASC";

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
                        cmd2.CommandText = "INSERT INTO k008db_branch(cdkey,txtco_id," +  //1
                                           "txtbranch_id,txtbranch_id_second,txthead_office_status," +  //2
                                           "txtbranch_name,txtbranch_name_short,txtbranch_name_eng," +  //3
                                           "txtEmail,txtbranch_tel," +  //4

                                           "txthome_id,txttambon," +  //5
                                           "txtamphur,txtchangwat," +  //6
                                            "txtpost_id," +  //7

                                           "txtbranch_status,txtuser_name," +  //8
                                           "txthome_id_full," +  //9
                                           "txthome_id_full_eng," +  //10
                                          "txtremark) " +  //11
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtbranch_id,@txtbranch_id_second,@txthead_office_status," +  //2
                                           "@txtbranch_name,@txtbranch_name_short,@txtbranch_name_eng," +  //3
                                           "@txtEmail,@txtbranch_tel," +  //4

                                           "@txthome_id,@txttambon," +  //5
                                           "@txtamphur,@txtchangwat," +  //6
                                            "@txtpost_id," +  //7

                                           "@txtbranch_status,@txtuser_name," +  //8
                                           "@txthome_id_full," +  //9
                                           "@txthome_id_full_eng," +  //10
                                           "@txtremark)";   //11

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = this.PANEL1_CO_txtco_id.Text.Trim();
                        cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtbranch_id_second", SqlDbType.NVarChar).Value = "";
                        if (this.checkBox1_head_office_status.Checked == true)
                        {
                            cmd2.Parameters.Add("@txthead_office_status", SqlDbType.NVarChar).Value = "Y";
                        }
                        else
                        {
                            cmd2.Parameters.Add("@txthead_office_status", SqlDbType.NVarChar).Value = "N";
                        }
                        cmd2.Parameters.Add("@txtbranch_name", SqlDbType.NVarChar).Value = this.txtbranch_name.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_name_short", SqlDbType.NVarChar).Value = this.txtbranch_name_short.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_name_eng", SqlDbType.NVarChar).Value = this.txtbranch_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtEmail", SqlDbType.NVarChar).Value = this.txtEmail.Text.ToString();
                        cmd2.Parameters.Add("@txtbranch_tel", SqlDbType.NVarChar).Value = this.txtbranch_tel.Text.ToString();

                        cmd2.Parameters.Add("@txthome_id", SqlDbType.NVarChar).Value = this.txthome_id.Text.ToString();
                        cmd2.Parameters.Add("@txttambon", SqlDbType.NVarChar).Value = this.txttambon.Text.ToString();
                        cmd2.Parameters.Add("@txtamphur", SqlDbType.NVarChar).Value = this.txtamphur.Text.ToString();
                        cmd2.Parameters.Add("@txtchangwat", SqlDbType.NVarChar).Value = this.txtchangwat.Text.ToString();
                        cmd2.Parameters.Add("@txtpost_id", SqlDbType.NVarChar).Value = this.txtpost_id.Text.ToString();

                        cmd2.Parameters.Add("@txtbranch_status", SqlDbType.NChar).Value = "0";
                        cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = "";
                        //cmd2.Parameters.Add("@txthome_id_full", SqlDbType.NVarChar).Value = this.txthome_id.Text.ToString() + "  ตำบล" + this.txttambon.Text.ToString() + "  อำเภอ" + this.txtamphur.Text.ToString() + "  จังหวัด" + this.txtchangwat.Text.ToString() + " " + this.txtpost_id.Text.ToString() + "";
                        cmd2.Parameters.Add("@txthome_id_full", SqlDbType.NVarChar).Value = this.txthome_id_full.Text.ToString();
                        cmd2.Parameters.Add("@txthome_id_full_eng", SqlDbType.NVarChar).Value = this.txthome_id_full_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtremark", SqlDbType.NVarChar).Value = this.txtremark.Text.ToString();
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

            }
            //=============================================================

        }
        //END Check ADD FORM====================================================================
        //=====================================================================================

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

            }

        }
        //END Check USER Rule=====================================================================


        //Tans_Log ====================================================================
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
