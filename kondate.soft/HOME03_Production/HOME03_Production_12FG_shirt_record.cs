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

namespace kondate.soft.HOME03_Production
{
    public partial class HOME03_Production_12FG_shirt_record : Form
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

        public HOME03_Production_12FG_shirt_record()
        {
            InitializeComponent();
        }

        private void HOME03_Production_12FG_shirt_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0312FGRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "บันทึกใบผลิตเสร็จ FG";

            this.ActiveControl = this.txtSEW_id;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            this.PANEL_SEW_dtpstart.Value = DateTime.Now;
            this.PANEL_SEW_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_SEW_dtpstart.CustomFormat = this.PANEL_SEW_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.PANEL_SEW_dtpend.Value = DateTime.Now;
            this.PANEL_SEW_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_SEW_dtpend.CustomFormat = this.PANEL_SEW_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL_SEW_Show_GridView1();
            Show_GridView1();

            PANEL_MAT_GridView1_mat();
            PANEL_MAT_Fill_mat();
            this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
            this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
            this.PANEL_MAT_cboSearch.Text = "ชื่อสินค้า";

        }

        //ใบสั่งตัด===================================================================================
        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle;
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow = 0;
        private void PANEL_SEW_Show_GridView1()
        {

            this.PANEL_SEW_GridView1.ColumnCount = 14;
            this.PANEL_SEW_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_SEW_GridView1.Columns[1].Name = "Col_txtco_id";
            this.PANEL_SEW_GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.PANEL_SEW_GridView1.Columns[3].Name = "Col_txtSEW_id";
            this.PANEL_SEW_GridView1.Columns[4].Name = "Col_txttrans_date_server";
            this.PANEL_SEW_GridView1.Columns[5].Name = "Col_txttrans_time";
            this.PANEL_SEW_GridView1.Columns[6].Name = "Col_txtsew_remark";
            this.PANEL_SEW_GridView1.Columns[7].Name = "Col_txtsum_qty_amount";
            this.PANEL_SEW_GridView1.Columns[8].Name = "Col_txtsum_qty_amount_all";

            this.PANEL_SEW_GridView1.Columns[9].Name = "Col_txtsum_qty_amount_Difference";
            this.PANEL_SEW_GridView1.Columns[10].Name = "Col_txtsew_status";

            this.PANEL_SEW_GridView1.Columns[11].Name = "Col_txtrol_id";  //
            this.PANEL_SEW_GridView1.Columns[12].Name = "Col_txtqcs_id";
            this.PANEL_SEW_GridView1.Columns[13].Name = "Col_txtfg_id";   //


            this.PANEL_SEW_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_SEW_GridView1.Columns[1].HeaderText = "txtco_id";
            this.PANEL_SEW_GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.PANEL_SEW_GridView1.Columns[3].HeaderText = " เลขที่ใบสั่งเย็บ";
            this.PANEL_SEW_GridView1.Columns[4].HeaderText = " วันที่";
            this.PANEL_SEW_GridView1.Columns[5].HeaderText = " เวลา";

            this.PANEL_SEW_GridView1.Columns[6].HeaderText = " หมายเหตุุ ";
            this.PANEL_SEW_GridView1.Columns[7].HeaderText = "จำนวนสั่งตัด";
            this.PANEL_SEW_GridView1.Columns[8].HeaderText = "จำนวนสั่งเย็บ";
            this.PANEL_SEW_GridView1.Columns[9].HeaderText = "จำนวนผลตาง";

            this.PANEL_SEW_GridView1.Columns[10].HeaderText = " สถานะ";
            this.PANEL_SEW_GridView1.Columns[11].HeaderText = " เลขที่ รีด";
            this.PANEL_SEW_GridView1.Columns[12].HeaderText = " เลขที่ QC";
            this.PANEL_SEW_GridView1.Columns[13].HeaderText = " เลขที่ FG";


            this.PANEL_SEW_GridView1.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.PANEL_SEW_GridView1.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.PANEL_SEW_GridView1.Columns["Col_txtbranch_id"].Visible = false;  //""Col_txtbranch_id"";

            this.PANEL_SEW_GridView1.Columns["Col_txtSEW_id"].Visible = true;  //"Col_txtSEW_id";
            this.PANEL_SEW_GridView1.Columns["Col_txtSEW_id"].Width = 140;
            this.PANEL_SEW_GridView1.Columns["Col_txtSEW_id"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtSEW_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtSEW_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_SEW_GridView1.Columns["Col_txttrans_date_server"].Visible = true;  //""Col_txttrans_date_server"";
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_date_server"].Width = 90;
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_date_server"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_date_server"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_date_server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_SEW_GridView1.Columns["Col_txttrans_time"].Visible = true;  //"Col_txttrans_time";
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_time"].Width = 70;
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_time"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_SEW_GridView1.Columns["Col_txtsew_remark"].Visible = true;  //"Col_txtsew_remark";
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_remark"].Width = 300;
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_remark"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_remark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_remark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount"].Visible = true;  //"Col_txtsum_qty_amount";
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount"].Width = 120;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_all"].Visible = true;  //"Col_txtsum_qty_amount_all";
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_all"].Width = 120;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_all"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_all"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_all"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_Difference"].Visible = true;  //"Col_txtsum_qty_amount_Difference";
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_Difference"].Width = 120;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_Difference"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_Difference"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtsum_qty_amount_Difference"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_SEW_GridView1.Columns["Col_txtsew_status"].Visible = true;  //"Col_txtsew_status";
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_status"].Width = 70;
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_status"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtsew_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_SEW_GridView1.Columns["Col_txtrol_id"].Visible = true;  //"Col_txtrol_id";
            this.PANEL_SEW_GridView1.Columns["Col_txtrol_id"].Width = 140;
            this.PANEL_SEW_GridView1.Columns["Col_txtrol_id"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtrol_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtrol_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_SEW_GridView1.Columns["Col_txtqcs_id"].Visible = true;  //"Col_txtqcs_id";
            this.PANEL_SEW_GridView1.Columns["Col_txtqcs_id"].Width = 140;
            this.PANEL_SEW_GridView1.Columns["Col_txtqcs_id"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtqcs_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtqcs_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_SEW_GridView1.Columns["Col_txtfg_id"].Visible = true;  //"Col_txtfg_id";
            this.PANEL_SEW_GridView1.Columns["Col_txtfg_id"].Width = 140;
            this.PANEL_SEW_GridView1.Columns["Col_txtfg_id"].ReadOnly = true;
            this.PANEL_SEW_GridView1.Columns["Col_txtfg_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_SEW_GridView1.Columns["Col_txtfg_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_SEW_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_SEW_GridView1.GridColor = Color.FromArgb(227, 227, 227);
            this.PANEL_SEW_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_SEW_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_SEW_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_SEW_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_SEW_GridView1.EnableHeadersVisualStyles = false;

        }
        private void PANEL_SEW_Clear_GridView1()
        {
            this.PANEL_SEW_GridView1.Rows.Clear();
            this.PANEL_SEW_GridView1.Refresh();
        }
        private void btntxtSEW_id_Click(object sender, EventArgs e)
        {
            if (this.PANEL_SEW.Visible == false)
            {
                this.PANEL_SEW.Visible = true;
                this.PANEL_SEW.BringToFront();
                this.PANEL_SEW.Location = new Point(this.label1.Location.X, this.txtSEW_id.Location.Y + 22);
                this.PANEL_SEW_iblword_top.Text = "ระเบียนใบสั่งตัด";
                SHOW_PANEL_SEW_btnGo2();

            }
            else
            {
                this.PANEL_SEW.Visible = false;
            }
        }
        private void PANEL_SEW_dtpstart_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_SEW_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_SEW_dtpstart.CustomFormat = this.PANEL_SEW_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);


        }
        private void PANEL_SEW_dtpend_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_SEW_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_SEW_dtpend.CustomFormat = this.PANEL_SEW_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

        }
        private void PANEL_SEW_btnGo1_Click(object sender, EventArgs e)
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

            PANEL_SEW_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM c002_09Sew_shirt_record" +

                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY txtSEW_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_SEW_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_SEW_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_SEW_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            var index = this.PANEL_SEW_GridView1.Rows.Add();
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtSEW_id"].Value = dt2.Rows[j]["txtSEW_id"].ToString();      //3
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_remark"].Value = dt2.Rows[j]["txtsew_remark"].ToString();      //6

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount"]).ToString("###,###.00");      //7
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount_all"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount_all"]).ToString("###,###.00");      //8
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount_Difference"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount_Difference"]).ToString("###,###.00");      //9

                            //SEW ==============================
                            if (dt2.Rows[j]["txtsew_status"].ToString() == "")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = ""; //10
                            }
                            else if (dt2.Rows[j]["txtsew_status"].ToString() == "0")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = ""; //10
                            }
                            else if (dt2.Rows[j]["txtsew_status"].ToString() == "1")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = "ยกเลิก"; //10
                            }
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtrol_id"].Value = dt2.Rows[j]["txtrol_id"].ToString(); //11
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtqcs_id"].Value = dt2.Rows[j]["txtqcs_id"].ToString();//12
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtfg_id"].Value = dt2.Rows[j]["txtfg_id"].ToString();//13



                        }
                        //=======================================================
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_SEW_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_SEW_GridView1_Color();

        }
        private void PANEL_SEW_btnGo2_Click(object sender, EventArgs e)
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

            PANEL_SEW_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                if (this.PANEL_SEW_ch_no_cs.Checked == true)
                {
                    cmd2.CommandText = "SELECT *" +
                                       " FROM c002_09Sew_shirt_record" +

                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                         //" AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                         " AND (txtfg_id = '')" +
                                         " ORDER BY txtSEW_id ASC";

                }
                if (this.PANEL_SEW_ch_no_cs.Checked == false)
                {
                    cmd2.CommandText = "SELECT *" +
                                       " FROM c002_09Sew_shirt_record" +

                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                         " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                         //" AND (txtrol_id = '')" +
                                         " ORDER BY txtSEW_id ASC";

                }

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_SEW_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_SEW_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_SEW_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            var index = this.PANEL_SEW_GridView1.Rows.Add();
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtSEW_id"].Value = dt2.Rows[j]["txtSEW_id"].ToString();      //3
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_remark"].Value = dt2.Rows[j]["txtsew_remark"].ToString();      //6

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount"]).ToString("###,###.00");      //7
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount_all"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount_all"]).ToString("###,###.00");      //8
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount_Difference"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount_Difference"]).ToString("###,###.00");      //9

                            //SEW ==============================
                            if (dt2.Rows[j]["txtsew_status"].ToString() == "")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = ""; //10
                            }
                            else if (dt2.Rows[j]["txtsew_status"].ToString() == "0")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = ""; //10
                            }
                            else if (dt2.Rows[j]["txtsew_status"].ToString() == "1")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = "ยกเลิก"; //10
                            }
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtrol_id"].Value = dt2.Rows[j]["txtrol_id"].ToString(); //11
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtqcs_id"].Value = dt2.Rows[j]["txtqcs_id"].ToString();//12
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtfg_id"].Value = dt2.Rows[j]["txtfg_id"].ToString();//13



                        }
                        //=======================================================
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_SEW_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_SEW_GridView1_Color();

        }
        private void SHOW_PANEL_SEW_btnGo2()
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

            PANEL_SEW_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM c002_09Sew_shirt_record" +

                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                         //" AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                         " AND (txtfg_id = '')" +
                                  " ORDER BY txtSEW_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_SEW_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_SEW_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_SEW_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            var index = this.PANEL_SEW_GridView1.Rows.Add();
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtSEW_id"].Value = dt2.Rows[j]["txtSEW_id"].ToString();      //3
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_remark"].Value = dt2.Rows[j]["txtsew_remark"].ToString();      //6

                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount"]).ToString("###,###.00");      //7
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount_all"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount_all"]).ToString("###,###.00");      //8
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsum_qty_amount_Difference"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_amount_Difference"]).ToString("###,###.00");      //9

                            //SEW ==============================
                            if (dt2.Rows[j]["txtsew_status"].ToString() == "")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = ""; //10
                            }
                            else if (dt2.Rows[j]["txtsew_status"].ToString() == "0")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = ""; //10
                            }
                            else if (dt2.Rows[j]["txtsew_status"].ToString() == "1")
                            {
                                this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtsew_status"].Value = "ยกเลิก"; //10
                            }
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtrol_id"].Value = dt2.Rows[j]["txtrol_id"].ToString(); //11
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtqcs_id"].Value = dt2.Rows[j]["txtqcs_id"].ToString();//12
                            this.PANEL_SEW_GridView1.Rows[index].Cells["Col_txtfg_id"].Value = dt2.Rows[j]["txtfg_id"].ToString();//13



                        }
                        //=======================================================
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_SEW_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_SEW_GridView1_Color();

        }
        private void PANEL_SEW_GridView1_Color()
        {
            for (int i = 0; i < this.PANEL_SEW_GridView1.Rows.Count - 0; i++)
            {
                if (this.PANEL_SEW_GridView1.Rows[i].Cells["Col_txtfg_id"].Value.ToString() != "")
                {
                    PANEL_SEW_GridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    PANEL_SEW_GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    PANEL_SEW_GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
                else
                {
                    PANEL_SEW_GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    PANEL_SEW_GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    PANEL_SEW_GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private Point MouseDownLocation;
        private void PANEL_SEW_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_SEW_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_SEW.Left = e.X + PANEL_SEW.Left - MouseDownLocation.X;
                PANEL_SEW.Top = e.Y + PANEL_SEW.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_SEW_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_SEW_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_SEW.Left = e.X + PANEL_SEW.Left - MouseDownLocation.X;
                PANEL_SEW.Top = e.Y + PANEL_SEW.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_SEW_btnclose_Click(object sender, EventArgs e)
        {
            this.PANEL_SEW.Visible = false;

        }
        private void PANEL_SEW_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL_SEW_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_SEW.Height = PANEL_SEW_btnresize_low.Top + e.Y;
                this.PANEL_SEW.Width = PANEL_SEW_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_SEW_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }
        private void PANEL_SEW_GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.PANEL_SEW_GridView1.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtSEW_id"].Value;
                if (cell != null)
                {
                    if (row.Cells["Col_txtfg_id"].Value.ToString() != "")
                    {
                        //MessageBox.Show(" " + row.Cells["Col_txtfg_id"].Value.ToString() + " ");
                        MessageBox.Show("เอกสารใบนี้ ออกใบจัดเก็บจำนวนรีด ไปแล้ว !!!!");
                        return;

                    }
                    else
                    {
                        this.txtSEW_id.Text = row.Cells["Col_txtSEW_id"].Value.ToString();
                        SHOW_SEW();
                    }
                    //=====================
                }
            }
        }
        private void SHOW_SEW()
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
            //===========================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c002_08Cut_shirt_record.*," +
                                   "c002_08Cut_shirt_record_detail.*," +
                                   "c002_09Sew_shirt_record.*," +
                                   "c001_05face_baking.*," +
                                   "c001_06number_mat.*," +
                                   "c001_07number_color.*," +

                                   "c001_08shirt_type.*," +
                                   "c001_09shirt_size.*," +
                                   "c001_10room_collect.*," +
                                   "c002_08Cut_shirt_cut_type.*," +
                                   "k016db_1supplier.*," +
                                   "k013_1db_acc_13group_tax.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_08Cut_shirt_record" +

                                   " INNER JOIN c002_08Cut_shirt_record_detail" +
                                   " ON c002_08Cut_shirt_record.cdkey = c002_08Cut_shirt_record_detail.cdkey" +
                                   " AND c002_08Cut_shirt_record.txtco_id = c002_08Cut_shirt_record_detail.txtco_id" +
                                   " AND c002_08Cut_shirt_record.txtCS_id = c002_08Cut_shirt_record_detail.txtCS_id" +

                                   " INNER JOIN c002_09Sew_shirt_record" +
                                   " ON c002_08Cut_shirt_record.cdkey = c002_09Sew_shirt_record.cdkey" +
                                   " AND c002_08Cut_shirt_record.txtco_id = c002_09Sew_shirt_record.txtco_id" +
                                   " AND c002_08Cut_shirt_record.txtSEW_id = c002_09Sew_shirt_record.txtSEW_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_08Cut_shirt_record_detail.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_08Cut_shirt_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_08Cut_shirt_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   " INNER JOIN c001_06number_mat" +
                                   " ON c002_08Cut_shirt_record_detail.cdkey = c001_06number_mat.cdkey" +
                                   " AND c002_08Cut_shirt_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                   " AND c002_08Cut_shirt_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                   " INNER JOIN c001_07number_color" +
                                   " ON c002_08Cut_shirt_record_detail.cdkey = c001_07number_color.cdkey" +
                                   " AND c002_08Cut_shirt_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                   " AND c002_08Cut_shirt_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                   " INNER JOIN c001_08shirt_type" +
                                   " ON c002_08Cut_shirt_record.cdkey = c001_08shirt_type.cdkey" +
                                   " AND c002_08Cut_shirt_record.txtco_id = c001_08shirt_type.txtco_id" +
                                   " AND c002_08Cut_shirt_record.txtshirt_type_id = c001_08shirt_type.txtshirt_type_id" +

                                   " INNER JOIN c001_09shirt_size" +
                                   " ON c002_08Cut_shirt_record.cdkey = c001_09shirt_size.cdkey" +
                                   " AND c002_08Cut_shirt_record.txtco_id = c001_09shirt_size.txtco_id" +
                                   " AND c002_08Cut_shirt_record.txtshirt_size_id = c001_09shirt_size.txtshirt_size_id" +

                                      " INNER JOIN c001_10room_collect" +
                                   " ON c002_08Cut_shirt_record.cdkey = c001_10room_collect.cdkey" +
                                   " AND c002_08Cut_shirt_record.txtco_id = c001_10room_collect.txtco_id" +
                                   " AND c002_08Cut_shirt_record.txtroom_collect_id = c001_10room_collect.txtroom_collect_id" +

                                       " INNER JOIN c002_08Cut_shirt_cut_type" +
                                   " ON c002_08Cut_shirt_record.txtcut_type_id = c002_08Cut_shirt_cut_type.txtcut_type_id" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON c002_08Cut_shirt_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND c002_08Cut_shirt_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND c002_08Cut_shirt_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_08Cut_shirt_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_08Cut_shirt_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_08Cut_shirt_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_08Cut_shirt_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_08Cut_shirt_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_08Cut_shirt_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_08Cut_shirt_record.txtSEW_id = '" + this.txtSEW_id.Text.Trim() + "')" +
                                   " ORDER BY c002_08Cut_shirt_record_detail.txtnumber_in_year,c002_08Cut_shirt_record_detail.txtfold_number ASC";


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        this.txtSEW_id.Text = dt2.Rows[0]["txtSEW_id"].ToString();

                        this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text = dt2.Rows[0]["txtshirt_type_id"].ToString();
                        this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Text = dt2.Rows[0]["txtshirt_type_name"].ToString();

                        this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text = dt2.Rows[0]["txtshirt_size_id"].ToString();
                        this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Text = dt2.Rows[0]["txtshirt_size_name"].ToString();

                        this.txttable_number.Text = dt2.Rows[0]["txttable_number"].ToString();

                        this.txtcut_type_id.Text = dt2.Rows[0]["txtcut_type_id"].ToString();
                        this.cbotxtcut_type_name.Text = dt2.Rows[0]["txtcut_type_name"].ToString();


                        this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                        this.txtqty_chan.Text = Convert.ToSingle(dt2.Rows[0]["txtqty_chan"]).ToString("###,###.00");
                        this.txtqty_many_per_chan.Text = Convert.ToSingle(dt2.Rows[0]["txtqty_many_per_chan"]).ToString("###,###.00");
                        this.txtqty_amount.Text = Convert.ToSingle(dt2.Rows[0]["txtqty_amount"]).ToString("###,###.00");

                        this.txtsum_qty_amount_all.Text = Convert.ToSingle(dt2.Rows[0]["txtsum_qty_amount_all"]).ToString("###,###.00");

                        this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                        this.PANEL_MAT_txtmat_id2.Text = dt2.Rows[0]["txtmat_id"].ToString();
                        this.PANEL_MAT_txtmat_name2.Text = dt2.Rows[0]["txtmat_name"].ToString();

                        this.txtmat_unit1_name.Text = dt2.Rows[0]["txtmat_unit1_name"].ToString();
                        this.txtmat_unit1_qty.Text = dt2.Rows[0]["txtmat_unit1_qty"].ToString();
                        this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                        this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                        this.txtmat_unit2_qty.Text = dt2.Rows[0]["txtmat_unit2_qty"].ToString();
                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = dt2.Rows[0]["txtnumber_mat_id"].ToString();
                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[0]["txtnumber_mat_name"].ToString();

                        this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text = dt2.Rows[0]["txtnumber_color_id"].ToString();
                        this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text = dt2.Rows[0]["txtnumber_color_name"].ToString();

                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = "ซื้อไม่มีvat";
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = "PUR_ONvat";



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


        }
        private void PANEL_SEW_GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else
                {
                    PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void PANEL_SEW_GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else
                {
                    PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        //ใบสั่งตัด===================================================================================


        //txtmat  สินค้า  =======================================================================
        private void PANEL_MAT_Fill_mat()
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

            PANEL_MAT_Clear_GridView1_mat();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +

                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*," +

                                    "b001mat_06price_sale.*" +

                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                    " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            //this.PANEL_MAT_dataGridView1_mat.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[5].Name = "Col_txtmat_unit1_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[6].Name = "Col_chmat_unit_status";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[7].Name = "Col_txtmat_unit2_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[8].Name = "Col_txtmat_unit2_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[9].Name = "Col_txtmat_price_sale1";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[10].Name = "Col_txtmat_status";

                            var index = PANEL_MAT_dataGridView1_mat.Rows.Add();
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = dt2.Rows[j]["txtmat_unit1_qty"].ToString();      //5
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //6
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //7
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = dt2.Rows[j]["txtmat_unit2_qty"].ToString();      //8
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_price_sale1"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");      //9
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //10
                        }
                        //======================================================= Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00"); 
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL_MAT_dataGridView1_mat_Up_Status();
                    this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
        private void PANEL_MAT_dataGridView1_mat_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL_MAT_dataGridView1_mat.Rows.Count; i++)
            {
                if (this.PANEL_MAT_dataGridView1_mat.Rows[i].Cells["Col_txtmat_status"].Value.ToString() == "0")  //Active
                {
                    this.PANEL_MAT_dataGridView1_mat.Rows[i].Cells["Col_Chk"].Value = true;
                }
                else
                {
                    this.PANEL_MAT_dataGridView1_mat.Rows[i].Cells["Col_Chk"].Value = false;

                }
            }

        }
        private void PANEL_MAT_GridView1_mat()
        {
            this.PANEL_MAT_dataGridView1_mat.ColumnCount = 11;
            this.PANEL_MAT_dataGridView1_mat.Columns[0].Name = "Col_Auto_num";
            this.PANEL_MAT_dataGridView1_mat.Columns[1].Name = "Col_txtmat_no";
            this.PANEL_MAT_dataGridView1_mat.Columns[2].Name = "Col_txtmat_id";
            this.PANEL_MAT_dataGridView1_mat.Columns[3].Name = "Col_txtmat_name";
            this.PANEL_MAT_dataGridView1_mat.Columns[4].Name = "Col_txtmat_unit1_name";
            this.PANEL_MAT_dataGridView1_mat.Columns[5].Name = "Col_txtmat_unit1_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns[6].Name = "Col_chmat_unit_status";
            this.PANEL_MAT_dataGridView1_mat.Columns[7].Name = "Col_txtmat_unit2_name";
            this.PANEL_MAT_dataGridView1_mat.Columns[8].Name = "Col_txtmat_unit2_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns[9].Name = "Col_txtmat_price_sale1";
            this.PANEL_MAT_dataGridView1_mat.Columns[10].Name = "Col_txtmat_status";

            this.PANEL_MAT_dataGridView1_mat.Columns[0].HeaderText = "No";
            this.PANEL_MAT_dataGridView1_mat.Columns[1].HeaderText = "ลำดับ";
            this.PANEL_MAT_dataGridView1_mat.Columns[2].HeaderText = " รหัส";
            this.PANEL_MAT_dataGridView1_mat.Columns[3].HeaderText = " ชื่อสินค้า";
            this.PANEL_MAT_dataGridView1_mat.Columns[4].HeaderText = "หน่วยหลัก";
            this.PANEL_MAT_dataGridView1_mat.Columns[5].HeaderText = "หน่วย";
            this.PANEL_MAT_dataGridView1_mat.Columns[6].HeaderText = "แปลง?";
            this.PANEL_MAT_dataGridView1_mat.Columns[7].HeaderText = "หน่วย(2)";
            this.PANEL_MAT_dataGridView1_mat.Columns[8].HeaderText = "หน่วย";
            this.PANEL_MAT_dataGridView1_mat.Columns[9].HeaderText = " ราคาขาย(บาท)";
            this.PANEL_MAT_dataGridView1_mat.Columns[10].HeaderText = "สถานะ";

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_Auto_num"].Visible = false;  //"No";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].Width = 100;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].Width = 120;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].Width = 250;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].Width = 140;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].Visible = true;  //"Col_txtmat_price_sale1";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].Width = 140;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_price_sale1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].Visible = false;  //"Col_txtmat_status";
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].Width = 0;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].ReadOnly = true;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_dataGridView1_mat.Columns["Col_txtmat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL_MAT_dataGridView1_mat.Columns.Add(dgvCmb);

            this.PANEL_MAT_dataGridView1_mat.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_MAT_dataGridView1_mat.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_MAT_dataGridView1_mat.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_MAT_dataGridView1_mat.EnableHeadersVisualStyles = false;


        }
        private void PANEL_MAT_Clear_GridView1_mat()
        {
            this.PANEL_MAT_dataGridView1_mat.Rows.Clear();
            this.PANEL_MAT_dataGridView1_mat.Refresh();
        }
        private void PANEL_MAT_txtmat_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL_MAT.Visible == false)
                {
                    this.PANEL_MAT.Visible = true;
                    this.PANEL_MAT.Location = new Point(this.PANEL_MAT_txtmat_name.Location.X, this.PANEL_MAT_txtmat_name.Location.Y + 22);
                    this.PANEL_MAT_dataGridView1_mat.Focus();
                }
                else
                {
                    this.PANEL_MAT.Visible = false;
                }
        }
        private void PANEL_MAT_btnmat_Click(object sender, EventArgs e)
        {
            if (this.PANEL_MAT.Visible == false)
            {

                int xLocation = PANEL_MAT_txtmat_name.Location.X;
                int yLocation = PANEL_MAT_txtmat_name.Location.Y;
                int xx = xLocation;
                int yy = yLocation;

                this.PANEL_MAT.Visible = true;
                this.PANEL_MAT.BringToFront();
                this.PANEL_MAT.Location = new Point(xx, yy + 24);
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_dataGridView1_mat_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmat_id"].Value;
                if (cell != null)
                {
                    this.txtmat_no.Text = row.Cells["Col_txtmat_no"].Value.ToString();
                    this.PANEL_MAT_txtmat_id.Text = row.Cells["Col_txtmat_id"].Value.ToString();
                    this.PANEL_MAT_txtmat_name.Text = row.Cells["Col_txtmat_name"].Value.ToString();
                    this.txtmat_unit1_name.Text = row.Cells["Col_txtmat_unit1_name"].Value.ToString();
                }
            }
        }
        private void PANEL_MAT_dataGridView1_mat_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL_MAT_dataGridView1_mat.CurrentRow.Index;

                this.PANEL_MAT_txtmat_id.Text = PANEL_MAT_dataGridView1_mat.CurrentRow.Cells[1].Value.ToString();
                this.PANEL_MAT_txtmat_name.Text = PANEL_MAT_dataGridView1_mat.CurrentRow.Cells[2].Value.ToString();
                this.PANEL_MAT_txtmat_name.Focus();
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL_MAT_btn_search_Click(object sender, EventArgs e)
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

            PANEL_MAT_Clear_GridView1_mat();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
                //this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
                if (this.PANEL_MAT_cboSearch.Text.Trim() == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +

                                        "b001_05mat_unit1.*," +
                                        "b001_05mat_unit2.*," +

                                        "b001mat_06price_sale.*" +

                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                        " INNER JOIN b001_05mat_unit2" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                        " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +



                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat.txtmat_name LIKE '%" + this.PANEL_MAT_txtsearch.Text.Trim() + "%')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

                }
                if (this.PANEL_MAT_cboSearch.Text.Trim() == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +

                                        "b001_05mat_unit1.*," +
                                        "b001_05mat_unit2.*," +

                                        "b001mat_06price_sale.*" +

                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                        " INNER JOIN b001_05mat_unit2" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                        " INNER JOIN b001mat_06price_sale" +
                                        " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +



                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtsearch.Text.Trim() + "')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

                }


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
                            //this.PANEL_MAT_dataGridView1_mat.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[5].Name = "Col_txtmat_unit1_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[6].Name = "Col_chmat_unit_status";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[7].Name = "Col_txtmat_unit2_name";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[8].Name = "Col_txtmat_unit2_qty";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[9].Name = "Col_txtmat_price_sale1";
                            //this.PANEL_MAT_dataGridView1_mat.Columns[10].Name = "Col_txtmat_status";

                            var index = PANEL_MAT_dataGridView1_mat.Rows.Add();
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = dt2.Rows[j]["txtmat_unit1_qty"].ToString();      //5
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //6
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //7
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = dt2.Rows[j]["txtmat_unit2_qty"].ToString();      //8
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_price_sale1"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");      //9
                            PANEL_MAT_dataGridView1_mat.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //10
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL_MAT_dataGridView1_mat_Up_Status();
                    this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
        private void PANEL_MAT_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_MAT_Fill_mat();
        }
        private void PANEL_MAT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL_MAT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_MAT.Height = PANEL_MAT_btnresize_low.Top + e.Y;
                this.PANEL_MAT.Width = PANEL_MAT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_MAT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL_MAT_btnnew_Click(object sender, EventArgs e)
        {

        }
        //private Point MouseDownLocation;
        private void PANEL_MAT_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_dataGridView1_mat_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void PANEL_MAT_dataGridView1_mat_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                this.PANEL_MAT_dataGridView1_mat.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }

        //END txtmat สินค้า =======================================================================


        private void panel_button_top_pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel_button_top_pictureBox_MouseMove(object sender, MouseEventArgs e)
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

        private void panel1_contens_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        //====================
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
            if (Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_fg.Text)) == 0)
            {
                MessageBox.Show("ไม่พบ จำนวนผลิตเสร็จ FG !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกกลุ่มภาษี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Focus();
                return;
            }


            STOCK_FIND_INSERT();
            AUTO_BILL_TRANS_ID();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();

            //จบเชื่อมต่อฐานข้อมูล=======================================================

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
                    String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                    DateTime myDateTime = new DateTime();
                    myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

                    String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                    DateTime myDateTime2 = new DateTime();
                    myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);
                    //MessageBox.Show("ok1");



                    //1 k020db_receive_record_trans
                    if (W_ID_Select.TRANS_BILL_STATUS.Trim() == "N")
                    {
                        cmd2.CommandText = "INSERT INTO c002_12FG_shirt_record_trans(cdkey," +
                                           "txtco_id,txtbranch_id," +
                                           "txttrans_id)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                           "'" + this.txtFG_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();


                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE c002_12FG_shirt_record_trans SET txttrans_id = '" + this.txtFG_id.Text.Trim() + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //MessageBox.Show("ok1");

                    //2 c002_12FG_shirt_record
                    cmd2.CommandText = "INSERT INTO c002_12FG_shirt_record(cdkey,txtco_id,txtbranch_id," +  //1
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                            "txtuser_name,txtemp_office_name," +  //5
                                           "txtversion_id," +  //6
                                                               //====================================================

                                          "txtFG_id," + // 7
                                           "txtemp_office_name_manager," + // 9
                                           "txtemp_office_name_approve," + // 10
                                           "txtFG_remark," + // 11

                                           "txtcurrency_id," + // 12
                                           "txtcurrency_date," + // 13
                                           "txtcurrency_rate," + // 14

                                           "txtacc_group_tax_id," + // 15

                                           "txtsum_qty_fg," + // 16

                                           "txtsum_price," + // 17
                                           "txtsum_discount," + // 18
                                           "txtmoney_sum," + // 19
                                           "txtmoney_tax_base," + // 20
                                           "txtvat_rate," + // 21
                                           "txtvat_money," + // 22
                                           "txtmoney_after_vat," + // 23
                                           "txtmoney_after_vat_creditor," + // 24
                                           "txtcreditor_status," + // 25

                                           "txtfg_status," +  //29

                                          "txtapprove_status," +  //31
                                          "txtpayment_status," +  //32
                                          "txtacc_record_status," +  //33
                                          "txtemp_print,txtemp_print_datetime) " +  //34

                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtuser_name,@txtemp_office_name," +  //5
                                           "@txtversion_id," +  //6
                                                                //=========================================================


                                          "@txtFG_id," + // 7
                                           "@txtemp_office_name_manager," + // 9
                                           "@txtemp_office_name_approve," + // 10
                                           "@txtFG_remark," + // 11

                                           "@txtcurrency_id," + // 12
                                           "@txtcurrency_date," + // 13
                                           "@txtcurrency_rate," + // 14

                                           "@txtacc_group_tax_id," + // 15

                                           "@txtsum_qty_fg," + // 16

                                           "@txtsum_price," + // 17
                                           "@txtsum_discount," + // 18
                                           "@txtmoney_sum," + // 19
                                           "@txtmoney_tax_base," + // 20
                                           "@txtvat_rate," + // 21
                                           "@txtvat_money," + // 22
                                           "@txtmoney_after_vat," + // 23
                                           "@txtmoney_after_vat_creditor," + // 24
                                           "@txtcreditor_status," + // 25

                                           "@txtfg_status," +  //29

                                          "@txtapprove_status," +  //31
                                          "@txtpayment_status," +  //32
                                          "@txtacc_record_status," +  //33
                                          "@txtemp_print,@txtemp_print_datetime) ";  //46

                    cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                    cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                    cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();  //1


                    cmd2.Parameters.Add("@txttrans_date_server", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);
                    cmd2.Parameters.Add("@txttrans_date_client", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd", UsaCulture);


                    cmd2.Parameters.Add("@txtcomputer_ip", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_IP.Trim();
                    cmd2.Parameters.Add("@txtcomputer_name", SqlDbType.NVarChar).Value = W_ID_Select.COMPUTER_NAME.Trim();
                    cmd2.Parameters.Add("@txtuser_name", SqlDbType.NVarChar).Value = W_ID_Select.M_USERNAME.Trim();
                    cmd2.Parameters.Add("@txtemp_office_name", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();
                    cmd2.Parameters.Add("@txtversion_id", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();  //7
                    //==============================================================================



                    cmd2.Parameters.Add("@txtFG_id", SqlDbType.NVarChar).Value = this.txtFG_id.Text.Trim();  //7
                    cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.Trim();  //9
                    cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.Trim();  //10
                    cmd2.Parameters.Add("@txtFG_remark", SqlDbType.NVarChar).Value = this.txtFG_remark.Text.Trim();  //11

                    cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //12
                    cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //13
                    cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n0}", txtcurrency_rate.Text.ToString()));  //14

                    cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //15

                    cmd2.Parameters.Add("@txtsum_qty_fg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_fg.Text.ToString()));  //16

                    cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_price.Text.ToString()));  //17
                    cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_discount.Text.ToString()));  //18
                    cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_sum.Text.ToString()));  //19
                    cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_tax_base.Text.ToString()));  //20
                    cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtvat_rate.Text.ToString()));  //21
                    cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtvat_money.Text.ToString()));  //22
                    cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_after_vat.Text.ToString()));  //23
                    cmd2.Parameters.Add("@txtmoney_after_vat_creditor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_after_vat.Text.ToString()));  //24
                    cmd2.Parameters.Add("@txtcreditor_status", SqlDbType.NVarChar).Value = "0";  //25

                    cmd2.Parameters.Add("@txtfg_status", SqlDbType.NVarChar).Value = "";  //29
                    cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = "";  //31
                    cmd2.Parameters.Add("@txtpayment_status", SqlDbType.NVarChar).Value = "";  //32
                    cmd2.Parameters.Add("@txtacc_record_status", SqlDbType.NVarChar).Value = "";  //33
                    cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //34
                    cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);//35

                    //=====================================================================================================================================================
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");



                    //3 c002_12FG_shirt_record_detail



                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtSEW_id"].Value != null)
                        {

                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();

                            //===================================================================================================================
                            //3 c002_12FG_shirt_record_detail

                            cmd2.CommandText = "INSERT INTO c002_12FG_shirt_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                           "txttrans_year,txttrans_month,txttrans_day," +

                          // //=================================================================
                          "txtFG_id," +  //6
                          "txtSEW_id," +  //7
                           "txtwherehouse_id," +  //8
                           "txttable_number," +  //8
                          "txtshirt_size_id," +  //9
                           "txtshirt_type_id," +  //10
                          "txtnumber_color_id," +  //11

                            "txtmat_no," +  //12
                            "txtmat_id," +  //13
                            "txtmat_name," +  //14
                            "txtmat_unit1_name," +  //15

                              "txtqty," +  //16

                              "txtprice," +   //17
                              "txtdiscount_rate," +  //18
                              "txtdiscount_money," +  //19
                              "txtsum_total," +  //20

                              "txtcost_qty_balance_yokma," +  //20
                              "txtcost_qty_price_average_yokma," +  //20
                              "txtcost_money_sum_yokma," +  //20

                              "txtcost_qty_balance_yokpai," +  //20
                              "txtcost_qty_price_average_yokpai," +  //20
                              "txtcost_money_sum_yokpai," +  //20

                           "txtitem_no) " +  //21

                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                           "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +

                            "'" + this.txtFG_id.Text.Trim() + "'," +  //  "txtFG_id," +  //6
                            "'" + this.GridView1.Rows[i].Cells["Col_txtSEW_id"].Value.ToString() + "'," +  //7
                             "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //8
                           "'" + this.GridView1.Rows[i].Cells["Col_txttable_number"].Value.ToString() + "'," +  //8
                            "'" + this.GridView1.Rows[i].Cells["Col_txtshirt_size_id"].Value.ToString() + "'," +  //9
                            "'" + this.GridView1.Rows[i].Cells["Col_txtshirt_type_id"].Value.ToString() + "'," +  //10
                            "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_color_id"].Value.ToString() + "'," +  //11

                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //12
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //13
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //14
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //15

                            //"''," +  //12
                            //"''," +  //13
                            //"''," +  //14
                            //"''," +  //15

                             "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value.ToString())) + "'," +  //16

                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //17
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //18
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //19
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //20

                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + "'," +  //17
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString())) + "'," +  //18
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + "'," +  //19

                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //17
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //18
                            "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //19

                            "'" + this.GridView1.Rows[i].Cells["Col_Auto_num"].Value.ToString() + "')";  //21

                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok3");




                            cmd2.CommandText = "UPDATE c002_09Sew_shirt_record SET " +
                                               "txtfg_status = '0'," +
                                               "txtfg_id = '" + this.txtFG_id.Text.Trim() + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtSEW_id = '" + this.GridView1.Rows[i].Cells["Col_txtSEW_id"].Value.ToString() + "')";

                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok7");



                            //=====================================================================================================
                            //สต๊อคสินค้า ตามคลัง =============================================================================================



                            //1.k021_mat_average
                            cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                               "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +
                                               "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +
                                                "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                               " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString()  + "')" +
                                               " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok7");



                            //2.k021_mat_average_balance

                            cmd2.CommandText = "INSERT INTO k021_mat_average_balance(cdkey,txtco_id,txtbranch_id," +  //1
                                       "txttrans_date_server,txttrans_time," +  //2
                                       "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                       "txtcomputer_ip,txtcomputer_name," +  //4
                                        "txtuser_name,txtemp_office_name," +  //5
                                       "txtversion_id," +  //6
                                                           //====================================================

                                           "txtbill_id," +  //7
                                           "txtbill_type," +  //8
                                           "txtbill_remark," +  //9

                                           "txtwherehouse_id," +  //10
                                           "txtmat_no," +  //10
                                           "txtmat_id," +  //11
                                           "txtmat_name," +  //12
                                           "txtmat_unit1_name," +  //13

                                           "txtmat_unit1_qty," +  //14
                                           "chmat_unit_status," +  //15
                                           "txtmat_unit2_name," +  //16
                                           "txtmat_unit2_qty," +  //17

                                          "txtqty_in," +  //18
                                           "txtqty2_in," +  //19
                                          "txtprice_in," +   //20
                                           "txtsum_total_in," +  //21

                                          "txtqty_out," +  //22
                                          "txtqty2_out," +  //23
                                          "txtprice_out," +  //24
                                           "txtsum_total_out," +  //25

                                           "txtqty_balance," +  //26
                                           "txtqty2_balance," +  //27
                                          "txtprice_balance," +  //28
                                           "txtsum_total_balance," +  //29

                                           "txtitem_no) " +  //30

                                    "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                    "'" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "','" + myDateTime2.ToString("HH:mm:ss", UsaCulture) + "'," +  //2
                                    "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", UsaCulture) + "'," +  //3
                                    "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                                    "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                                    "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                                                                                  //=======================================================


                                    "'" + this.txtFG_id.Text.Trim() + "'," +  //7 txtbill_id
                                    "'FG'," +  //9 txtbill_type
                                    "'ผลิตเสร็จ FG " + this.txtFG_remark.Text.Trim() + "'," +  //9 txtbill_remark

                                     "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //7 txtwherehouse_id
                                   "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //10 
                                    "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //11
                                    "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //12

                                    "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //13
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //14
                                    "'N'," +  //15
                                    "''," +  //16
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //17

                                   "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value.ToString())) + "'," +  //22 txtqty_in
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //23 txtqty2_in
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //24 txtprice_out
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out


                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //18  txtqty_in
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //19 txtqty2_in
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //20 txtprice_in
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //21 txtsum_total_in


                                   "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //26
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //27
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //28
                                   "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //29

                                   "'1')";   //30

                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok8");


                            //======================================

                            //สต๊อคสินค้า ตามคลัง =============================================================================================

                            //MessageBox.Show("ok4");


                        }
                    }





                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = false;
                        this.btnPreview.Enabled = true;
                        this.BtnPrint.Enabled = true;
                        this.BtnClose_Form.Enabled = true;

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "บันทึกใบผลิตเสร็จ FG")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }


                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = true;
                        this.btnPreview.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnClose_Form.Enabled = true;

                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = true;
                        this.btnPreview.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnClose_Form.Enabled = true;

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

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtFG_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_12FG_shirt_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_12FG_shirt_record_print();
            frm2.Show();
            frm2.BringToFront();
            //====================

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtFG_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            //Print Role=========================================
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {

                TableLogOnInfo cr_table_logon_info = new TableLogOnInfo();
                ConnectionInfo cr_Connection_Info = new ConnectionInfo();
                Tables CrTables;

                ReportDocument rpt = new ReportDocument();

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_Chart_of_accounts.rpt");
                //E:\01_Project_ERP_Kondate.Soft\kondate.soft\kondate.soft\bin\Debug\KONDATE_REPORT
                //E:\01_Project_ERP_Kondate.Soft\kondate.soft\kondate.soft\KONDATE_REPORT\Report_Chart_of_accounts.rpt

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_c002_12FG_shirt_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_12FG_shirt_record.rpt");


                string cr_server = W_ID_Select.ADATASOURCE.Trim();
                string cr_database = W_ID_Select.DATABASE_NAME.ToString();
                string cr_user = W_ID_Select.Crytal_USER.ToString();
                string cr_pass = W_ID_Select.Crytal_Pass.ToString();

                cr_Connection_Info.DatabaseName = cr_server;
                cr_Connection_Info.DatabaseName = cr_database;
                cr_Connection_Info.UserID = cr_user;
                cr_Connection_Info.Password = cr_pass;
                cr_Connection_Info.IntegratedSecurity = false;
                CrTables = rpt.Database.Tables;


                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in CrTables)
                {
                    cr_table_logon_info = crTable.LogOnInfo;
                    cr_table_logon_info.ConnectionInfo = cr_Connection_Info;
                    crTable.ApplyLogOnInfo(cr_table_logon_info);
                }
                foreach (ReportDocument subreport in rpt.Subreports)
                {
                    foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in subreport.Database.Tables)
                    {
                        cr_table_logon_info = crTable.LogOnInfo;
                        cr_table_logon_info.ConnectionInfo = cr_Connection_Info;
                        crTable.ApplyLogOnInfo(cr_table_logon_info);
                    }
                }


                rpt.SetParameterValue("cdkey", W_ID_Select.CDKEY.Trim());
                rpt.SetParameterValue("txtco_id", W_ID_Select.M_COID.Trim());
                rpt.SetParameterValue("txtfg_id", W_ID_Select.TRANS_ID.Trim());

                //พิมพ์กับเครื่องที่เราต้องการ ระบุชื่อไปเลย=============================================
                //rpt.PrintOptions.PrinterName = "EPSON TM-T88V Receipt5";
                //rpt.PrintToPrinter(1, false, 0, 0);


                //พิมพ์ออกที่เครื่องพิมพ์ที่เลือกไว้ ในเครื่อง==============================================
                //rpt.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                //rpt.PrintOptions.PaperSize = PaperSize.PaperA4;
                //rpt.PrintToPrinter(1, false, 0, 15);


                //พิมพ์เป็น ไดอะล็อค เพื่อ save เป็น file อื่นๆ ที่ต้องการอีกที ==============================================
                rpt.PrintOptions.PrinterName = printDialog.PrinterSettings.PrinterName;
                rpt.PrintToPrinter(printDialog.PrinterSettings.Copies, printDialog.PrinterSettings.Collate, printDialog.PrinterSettings.FromPage, printDialog.PrinterSettings.ToPage);

            }
        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 24;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtSEW_id";  //txtSEW_id
            this.GridView1.Columns[2].Name = "Col_txttable_number";  //โต๊ะ
            this.GridView1.Columns[3].Name = "Col_txtshirt_size_id";  // ไซส์
            this.GridView1.Columns[4].Name = "Col_txtshirt_type_id";  // คอ กลม คอ วี
            this.GridView1.Columns[5].Name = "Col_txtshirt_type_name";  // คอ กลม คอ วี
            this.GridView1.Columns[6].Name = "Col_txtnumber_color_id";  //สี

            this.GridView1.Columns[7].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[8].Name = "Col_txtmat_no";
            this.GridView1.Columns[9].Name = "Col_txtmat_id";
            this.GridView1.Columns[10].Name = "Col_txtmat_name";
            this.GridView1.Columns[11].Name = "Col_txtmat_unit1_name";


            this.GridView1.Columns[12].Name = "Col_txtsum_qty_amount_all";

            this.GridView1.Columns[13].Name = "Col_txtprice";
            this.GridView1.Columns[14].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[15].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[16].Name = "Col_txtsum_total";

            this.GridView1.Columns[17].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[18].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[19].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[20].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[21].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[22].Name = "Col_txtcost_money_sum_yokpai";
            this.GridView1.Columns[23].Name = "Col_mat_status";
            //Col_mat_status

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "เลขที่ใบสั่งตัด";
            this.GridView1.Columns[2].HeaderText = "โต๊ะ";
            this.GridView1.Columns[3].HeaderText = "ไซส์";
            this.GridView1.Columns[4].HeaderText = "ชนิดเสื้อ";
            this.GridView1.Columns[5].HeaderText = "ชนิดเสื้อ";
            this.GridView1.Columns[6].HeaderText = "รหัสสี";

            this.GridView1.Columns[7].HeaderText = "คลังสินค้า";

            this.GridView1.Columns[8].HeaderText = "ลำดับ";
            this.GridView1.Columns[9].HeaderText = "รหัสสินค้า";
            this.GridView1.Columns[10].HeaderText = "ชื่อสินค้า";
            this.GridView1.Columns[11].HeaderText = "หน่วยนับ";

            this.GridView1.Columns[12].HeaderText = "จำนวนผลิตเสร็จ FG (ตัว)";

            this.GridView1.Columns[13].HeaderText = "ราคา";
            this.GridView1.Columns[14].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[15].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[16].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[17].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[18].HeaderText = "ราคาเฉลี่ย";
            this.GridView1.Columns[19].HeaderText = "จำนวนเงินยกมา";

            this.GridView1.Columns[20].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[21].HeaderText = "ราคาเฉลี่ย";
            this.GridView1.Columns[22].HeaderText = "จำนวนเงินยกไป";
            this.GridView1.Columns[23].HeaderText = "Col_mat_status";
            //Col_mat_status


            this.GridView1.Columns["Col_Auto_num"].Visible = true;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 60;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtSEW_id"].Visible = true;  //"Col_txtSEW_id";
            this.GridView1.Columns["Col_txtSEW_id"].Width = 150;
            this.GridView1.Columns["Col_txtSEW_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtSEW_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtSEW_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txttable_number"].Visible = true;  //"Col_txttable_number";
            this.GridView1.Columns["Col_txttable_number"].Width = 150;
            this.GridView1.Columns["Col_txttable_number"].ReadOnly = true;
            this.GridView1.Columns["Col_txttable_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttable_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtshirt_size_id"].Visible = true;  //"Col_txtshirt_size_id";
            this.GridView1.Columns["Col_txtshirt_size_id"].Width = 150;
            this.GridView1.Columns["Col_txtshirt_size_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtshirt_size_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtshirt_size_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtshirt_type_id"].Visible = false;  //"Col_txtshirt_type_id";
            this.GridView1.Columns["Col_txtshirt_type_id"].Width = 0;
            this.GridView1.Columns["Col_txtshirt_type_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtshirt_type_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtshirt_type_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtshirt_type_name"].Visible = true;  //"Col_txtshirt_type_name";
            this.GridView1.Columns["Col_txtshirt_type_name"].Width = 150;
            this.GridView1.Columns["Col_txtshirt_type_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtshirt_type_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtshirt_type_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtnumber_color_id"].Visible = true;  //"Col_txtnumber_color_id";
            this.GridView1.Columns["Col_txtnumber_color_id"].Width = 150;
            this.GridView1.Columns["Col_txtnumber_color_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_color_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_color_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtwherehouse_id"].Visible = true;  //"Col_txtwherehouse_id";
            this.GridView1.Columns["Col_txtwherehouse_id"].Width = 100;
            this.GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no";
            this.GridView1.Columns["Col_txtmat_no"].Width = 100;
            this.GridView1.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 80;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 150;
            this.GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit1_name"].Visible = false;  //"Col_txtmat_unit1_name";
            this.GridView1.Columns["Col_txtmat_unit1_name"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtsum_qty_amount_all"].Visible = true;  //"Col_txtsum_qty_amount_all";
            this.GridView1.Columns["Col_txtsum_qty_amount_all"].Width = 150;
            this.GridView1.Columns["Col_txtsum_qty_amount_all"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_amount_all"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_amount_all"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtprice"].Visible = false;  //"Col_txtprice";
            this.GridView1.Columns["Col_txtprice"].Width = 0;
            this.GridView1.Columns["Col_txtprice"].ReadOnly = true;
            this.GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.GridView1.Columns["Col_txtdiscount_rate"].Width = 0;
            this.GridView1.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.GridView1.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_money"].Visible = false;  //"Col_txtdiscount_money";
            this.GridView1.Columns["Col_txtdiscount_money"].Width = 0;
            this.GridView1.Columns["Col_txtdiscount_money"].ReadOnly = false;
            this.GridView1.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_total"].Visible = false;  //"Col_txtsum_total";
            this.GridView1.Columns["Col_txtsum_total"].Width = 0;
            this.GridView1.Columns["Col_txtsum_total"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].Visible = false;  //"Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].Visible = false;  //"Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].Visible = false;  //"Col_txtcost_money_sum_yokma";
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_money_sum_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].Visible = false;  //"Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].Visible = false;  //"Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].Visible = false;  //"Col_txtcost_money_sum_yokpai";
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_money_sum_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //Col_mat_status
            this.GridView1.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";
            this.GridView1.Columns["Col_mat_status"].Width = 0;
            this.GridView1.Columns["Col_mat_status"].ReadOnly = true;
            this.GridView1.Columns["Col_mat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_mat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


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
        private void btnGo1_Click(object sender, EventArgs e)
        {
            if (this.txtSEW_id.Text == "")
            {
                MessageBox.Show("โปรด ใส่เลขที่ใบสั่งเย็บ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL_SEW.Visible == false)
                {
                    this.PANEL_SEW.Visible = true;
                    this.PANEL_SEW.BringToFront();
                    this.PANEL_SEW.Location = new Point(this.label1.Location.X, this.txtSEW_id.Location.Y + 22);
                }
                else
                {
                    this.PANEL_SEW.Visible = false;
                }
                return;

            }
            else
            {


                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtSEW_id"].Value.ToString() == this.txtSEW_id.Text.Trim())
                    {
                        MessageBox.Show("เลขที่ใบสั่งเย็บนี้  " + this.txtSEW_id.Text.Trim() + "    นี้ เพิ่มไปแล้ว !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtSEW_id.Focus();
                        return;
                    }
                }

                 var index = GridView1.Rows.Add();
                GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                GridView1.Rows[index].Cells["Col_txtSEW_id"].Value = this.txtSEW_id.Text.ToString();      //1
                GridView1.Rows[index].Cells["Col_txttable_number"].Value = this.txttable_number.Text.ToString();      //2
                GridView1.Rows[index].Cells["Col_txtshirt_size_id"].Value = this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text.Trim();      //3
                GridView1.Rows[index].Cells["Col_txtshirt_type_id"].Value = this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text.ToString();      //4
                GridView1.Rows[index].Cells["Col_txtshirt_type_name"].Value = this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Text.ToString();      //5
                GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text.ToString();     //6

                GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();     //6
                GridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.txtmat_no.Text.ToString();     //6
                GridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.PANEL_MAT_txtmat_id.Text.ToString();     //6
                GridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.PANEL_MAT_txtmat_name.Text.ToString();     //6
                GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.txtmat_unit1_name.Text.ToString();     //6


                GridView1.Rows[index].Cells["Col_txtsum_qty_amount_all"].Value = Convert.ToSingle(this.txtsum_qty_amount_all.Text).ToString("###,###.00");     //7

                GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(0).ToString("###,###.00");     //8
                GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(0).ToString("###,###.00");     //9
                GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(0).ToString("###,###.00");     //10
                GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(0).ToString("###,###.00");     //11

                GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";     //8
                GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";     //8
                GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";     //8

                GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";     //8
                GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";     //8
                GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";     //8

                CLEAR_TXT();
                Show_Qty_Yokma();
                GridView1_Cal_Sum();
                Sum_group_tax();
            }



        }
        private void CLEAR_TXT()
        {
            this.txtSEW_id.Text = "";
            this.txtsum_qty_amount_all.Text = "0";
            this.txttable_number.Text = "";
            this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Text = "";
            this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text = "";
            this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Text = "";
            this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text = "";
            this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text = "";
            this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text = "";
            this.cbotxtcut_type_name.Text = "";
            this.txtcut_type_id.Text = "";
            this.txtqty_chan.Text = ".00";
            this.txtqty_many_per_chan.Text = ".00";
            this.txtqty_amount.Text = ".00";
            this.txtsum_qty_amount_all.Text = ".00";
            this.PANEL161_SUP_txtsupplier_name.Text = "";
            this.PANEL161_SUP_txtsupplier_id.Text = "";
            this.PANEL_MAT_txtmat_name.Text = "";
            this.PANEL_MAT_txtmat_id.Text = "";
            this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = "";
            this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = "";

        }
        private void GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                PANEL_SEW_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_Cal_Sum()
        {
            double Sum_Total = 0;
            double Sum_Qty_Yokma = 0;
            double Sum_Qty_Yokpai = 0;
            double Sum2_Qty_Yokpai = 0;
            double Sum_Qty = 0;
            double Sum2_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            double Con_QTY = 0;

            double QAbyma = 0;
            double Qbypai = 0;
            double Mbypai = 0;
            double QAbypai = 0;

            double Sum_Qty_RECEive_Yokpai = 0;
            double Sum_Qty_bl_Yokpai = 0;
            double Sum_Qty_REceive_bl_Yokpai = 0;

            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells["Col_Auto_num"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtprice"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtprice"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = "0";

                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = "0";

                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";

                    }
                    //5 * 6 = 8

                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value).ToString("###,###.00");     //5

                    this.GridView1.Rows[i].Cells["Col_txtprice"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtprice"].Value).ToString("###,###.00");     //6
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value).ToString("###,###.00");     //8

                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value).ToString("###,###.00");     //6
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value).ToString("###,###.00");     //8

                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value).ToString("###,###.00");     //8



                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value.ToString()));
                        this.txtsum_qty_fg.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                        this.txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }

                   //  ===========================================================================================================
                    //============================================================================================================
                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //1.เหลือยกมา + รับ = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_amount_all"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา + มูลค่ารับ = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) / Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";
                    }
                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                }
            }

            this.txtcount.Text = k.ToString();

            Sum_Total = 0;
            Sum_Qty = 0;
            Sum_Price = 0;
            Sum_Discount = 0;
            MoneySum = 0;
            Sum_Qty_Yokma = 0;
            Sum_Qty_Yokpai = 0;
            Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            QAbyma = 0;
            Qbypai = 0;
            Mbypai = 0;
            QAbypai = 0;
            Sum_Qty_RECEive_Yokpai = 0;
            Sum_Qty_bl_Yokpai = 0;
            Sum_Qty_REceive_bl_Yokpai = 0;

        }
        private void Show_Qty_Yokma()
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
            string MATID = "";
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {

                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
                    MATID = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                    Cursor.Current = Cursors.WaitCursor;

                    //เชื่อมต่อฐานข้อมูล======================================================
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {

                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;


                        cmd2.CommandText = "SELECT *" +
                                           " FROM k021_mat_average" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                           " AND (txtmat_id = '" + MATID.Trim() + "')" +
                                          " ORDER BY txtmat_no ASC";

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

                                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");        //19
                                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");        //20

                                }
                                //=======================================================
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


                }
            }
        }
        private void UPDATE_PRINT_BY()
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

                    cmd2.CommandText = "UPDATE c002_12FG_shirt_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtFG_id = '" + this.txtFG_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();



                    Cursor.Current = Cursors.WaitCursor;

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
            //=============================================================

        }
        private void Sum_group_tax()
        {
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_EX")  //ซื้อคิดvatแยก
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_IN") //ซื้อคิดvatรวม
            {
                double DisCount = 0;
                double VATMONey = 0;
                double VATBASE = 0;
                double VATA = 0;

                //รวมทั้งสิ้น
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_after_vat.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                VATA = Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) + 100;

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_after_vat.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / Convert.ToDouble(string.Format("{0:n}", VATA));
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //ฐานภาษี
                VATBASE = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_after_vat.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_tax_base.Text = VATBASE.ToString("N", new CultureInfo("en-US"));


            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_ONvat")  //ซื้อไม่มีvat
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                this.txtvat_rate.Text = "0";

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));


            }
        }

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
                                  " FROM c002_12FG_shirt_record_trans" +
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
                            TMP = "FG" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "FG" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "FG" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtFG_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



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

            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Visible = true;  //"Col_txtwherehouse_name_eng";
            this.PANEL1306_WH_dataGridView1_wherehouse.Columns[3].Width = 150;
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
        private void PANEL1306_WH_txtwherehouse_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1306_WH.Visible == false)
                {
                    this.PANEL1306_WH.Visible = true;
                    this.PANEL1306_WH.Location = new Point(this.PANEL1306_WH_txtwherehouse_name.Location.X, this.PANEL1306_WH_txtwherehouse_name.Location.Y + 22);
                    this.PANEL1306_WH_dataGridView1_wherehouse.Focus();
                }
                else
                {
                    this.PANEL1306_WH.Visible = false;
                }
        }
        private void PANEL1306_WH_btnwherehouse_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH.Visible == false)
            {
                this.PANEL1306_WH.Visible = true;
                this.PANEL1306_WH.BringToFront();
                this.PANEL1306_WH.Location = new Point(this.PANEL1306_WH_txtwherehouse_name.Location.X, this.PANEL1306_WH_txtwherehouse_name.Location.Y + 22);
            }
            else
            {
                this.PANEL1306_WH.Visible = false;
            }
        }
        private void PANEL1306_WH_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH.Visible == false)
            {
                this.PANEL1306_WH.Visible = true;
            }
            else
            {
                this.PANEL1306_WH.Visible = false;
            }
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
                this.PANEL1306_WH.Visible = false;
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
        private void PANEL1306_WH_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1306_WH_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1306_WH.Height = PANEL1306_WH_btnresize_low.Top + e.Y;
                this.PANEL1306_WH.Width = PANEL1306_WH_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1306_WH_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1306_WH_btnnew_Click(object sender, EventArgs e)
        {

        }



        //END txtwherehouse คลังสินค้า  =======================================================================


        //จบส่วนตารางสำหรับบันทึก========================================================================

        private void STOCK_FIND_INSERT()
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

            //สต๊อคสินค้า ตามคลัง =============================================================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {

                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;

                        cmd2.CommandText = "SELECT *" +
                                                    " FROM k021_mat_average" +
                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                    " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                    " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')" +
                                                    " ORDER BY txtmat_no ASC";
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
                                    //Col_mat_status
                                    this.GridView1.Rows[i].Cells["Col_mat_status"].Value = "Y";
                                }
                                Cursor.Current = Cursors.Default;
                            }
                            else
                            {
                                this.GridView1.Rows[i].Cells["Col_mat_status"].Value = "N";

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
                    }
                } //== if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
            } //== for (int i = 0; i < this.GridView1.Rows.Count; i++)

            //สต๊อคสินค้า ตามคลัง =============================================================================================





            // INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
                    if (this.GridView1.Rows[i].Cells["Col_mat_status"].Value.ToString() != "Y")
                    {
                        //=======================================================
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

                                cmd2.CommandText = "INSERT INTO k021_mat_average(cdkey,txtco_id," +  //1
                               "txtwherehouse_id," +  //2
                               "txtmat_no," +  //3
                               "txtmat_id," +  //4
                               "txtmat_name," +  //5
                               "txtmat_unit1_qty," +  //6
                               "chmat_unit_status," +  //7
                               "txtmat_unit2_qty," +  //8
                               "txtcost_qty_balance," +  //9
                               "txtcost_qty_price_average," +  //10
                               "txtcost_money_sum," +  //11
                               "txtcost_qty2_balance," +  //11
                               "txtcost_qty_krasob_balance," +  //11
                               "txtcost_qty_lod_balance," +  //11
                              "txtcost_qty_pub_balance) " +  //14
                               "VALUES (@cdkey,@txtco_id," +  //1
                               "@txtwherehouse_id," +  //2
                               "@txtmat_no," +  //3
                               "@txtmat_id," +  //4
                               "@txtmat_name," +  //5
                               "@txtmat_unit1_qty," +  //6
                               "@chmat_unit_status," +  //7
                               "@txtmat_unit2_qty," +  //8
                               "@txtcost_qty_balance," +  //9
                               "@txtcost_qty_price_average," +  //10
                               "@txtcost_money_sum," +  //11
                               "@txtcost_qty2_balance," +  //11
                               "@txtcost_qty_krasob_balance," +  //11
                               "@txtcost_qty_lod_balance," +  //11
                               "@txtcost_qty_pub_balance)";   //14

                                cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                                cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();  //1

                                cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();  //2
                                cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString();  //3
                                cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();  //4
                                cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString();  //5
                                cmd2.Parameters.Add("@txtmat_unit1_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString()));  //6
                                cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString();  //7
                                cmd2.Parameters.Add("@txtmat_unit2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));  //8

                                cmd2.Parameters.Add("@txtcost_qty_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //9
                                cmd2.Parameters.Add("@txtcost_qty_price_average", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //10
                                cmd2.Parameters.Add("@txtcost_money_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //11

                                cmd2.Parameters.Add("@txtcost_qty2_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //13
                                cmd2.Parameters.Add("@txtcost_qty_krasob_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //13
                                cmd2.Parameters.Add("@txtcost_qty_lod_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //13
                                cmd2.Parameters.Add("@txtcost_qty_pub_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //13

                                //==============================

                                cmd2.ExecuteNonQuery();


                                Cursor.Current = Cursors.WaitCursor;
                                trans.Commit();
                                conn.Close();

                                Cursor.Current = Cursors.Default;


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
                }
            }
            // END INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี

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
                                this.GridView1.Visible = false;
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

                        this.GridView1.Visible = false;
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
                this.GridView1.Visible = true;
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
                this.GridView1.Visible = true;
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

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

        }

        private void BtnGrid_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = "ระเบียนใบผลิตเสร็จ FG";
            kondate.soft.HOME03_Production.HOME03_Production_12FG_shirt frm2 = new kondate.soft.HOME03_Production.HOME03_Production_12FG_shirt();
            frm2.Show();

        }



        //=============================================================

        //=========================================================

    }
}
