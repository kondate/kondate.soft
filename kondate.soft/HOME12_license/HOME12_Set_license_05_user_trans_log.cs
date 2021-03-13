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


namespace kondate.soft.HOME12_license
{
    public partial class HOME12_Set_license_05_user_trans_log : Form
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



        public HOME12_Set_license_05_user_trans_log()
        {
            InitializeComponent();
        }

        private void HOME12_Set_license_05_Load(object sender, EventArgs e)
        {
            
            W_ID_Select.M_FORM_NUMBER = "1205";
            CHECK_ADD_FORM();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            this.BtnPrint.Enabled = false;

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            PANEL_FORM1_GridView1();
            PANEL_FORM1_Fill_GridView1();

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

            String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
            DateTime myDateTime = new DateTime();
            myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

            String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
            DateTime myDateTime2 = new DateTime();
            myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);


            //cmd2.Parameters.Add("@txttrans_date1", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy-MM-dd", UsaCulture);
            //cmd2.Parameters.Add("@txttrans_time", SqlDbType.NVarChar).Value = myDateTime2.ToString("HH:mm:ss", UsaCulture);
            //cmd2.Parameters.Add("@txttrans_year", SqlDbType.NVarChar).Value = myDateTime.ToString("yyyy", UsaCulture);
            //cmd2.Parameters.Add("@txttrans_month", SqlDbType.NVarChar).Value = myDateTime.ToString("MM", UsaCulture);
            //cmd2.Parameters.Add("@txttrans_day", SqlDbType.NVarChar).Value = myDateTime.ToString("dd", UsaCulture);

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM A001_trans_log" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " ORDER BY ID ASC";

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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_cdkey";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtco_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txttrans_year";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txttrans_month";
                            //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txttrans_day";
                            //this.PANEL_FORM1_dataGridView1.Columns[9].Name = "Col_txttrans_date_client";
                            //this.PANEL_FORM1_dataGridView1.Columns[10].Name = "Col_txtcomputer_ip";
                            //this.PANEL_FORM1_dataGridView1.Columns[11].Name = "Col_txtcomputer_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[12].Name = "Col_txtform_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[13].Name = "Col_txtform_caption";
                            //this.PANEL_FORM1_dataGridView1.Columns[14].Name = "Col_txtuser_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[15].Name = "Col_txtemp_office_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[16].Name = "Col_txtlog_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[17].Name = "Col_txtlog_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[18].Name = "Col_txtdocument_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[19].Name = "Col_txtversion_id";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_cdkey"].Value = dt2.Rows[j]["cdkey"].ToString();      //1
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //2
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //3
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy",ThaiCulture);          //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_year"].Value = dt2.Rows[j]["txttrans_year"].ToString();      //6
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_month"].Value = dt2.Rows[j]["txttrans_month"].ToString();      //7
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_day"].Value = dt2.Rows[j]["txttrans_day"].ToString();      //8
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtcomputer_ip"].Value = dt2.Rows[j]["txtcomputer_ip"].ToString();      //9
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtcomputer_name"].Value = dt2.Rows[j]["txtcomputer_name"].ToString();      //10
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtform_name"].Value = dt2.Rows[j]["txtform_name"].ToString();      //11
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtform_caption"].Value = dt2.Rows[j]["txtform_caption"].ToString();      //12
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = dt2.Rows[j]["txtuser_name"].ToString();      //13
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //14
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtlog_id"].Value = dt2.Rows[j]["txtlog_id"].ToString();      //15
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtlog_name"].Value = dt2.Rows[j]["txtlog_name"].ToString();      //16
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtdocument_id"].Value = dt2.Rows[j]["txtdocument_id"].ToString();      //17
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtversion_id"].Value = dt2.Rows[j]["txtversion_id"].ToString();      //18

                        }

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
                    Cursor.Current = Cursors.Default;

                    conn.Close();
                }

                //===========================================
            }
            //================================

        }
        private void PANEL_FORM1_GridView1()
        {

            this.PANEL_FORM1_dataGridView1.ColumnCount = 20;
            this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_cdkey";
            this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtco_id";
            this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_id";
            this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txttrans_date_server";
            this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txttrans_time";
            this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txttrans_year";
            this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txttrans_month";
            this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txttrans_day";
            this.PANEL_FORM1_dataGridView1.Columns[9].Name = "Col_txttrans_date_client";
            this.PANEL_FORM1_dataGridView1.Columns[10].Name = "Col_txtcomputer_ip";
            this.PANEL_FORM1_dataGridView1.Columns[11].Name = "Col_txtcomputer_name";
            this.PANEL_FORM1_dataGridView1.Columns[12].Name = "Col_txtform_name";
            this.PANEL_FORM1_dataGridView1.Columns[13].Name = "Col_txtform_caption";
            this.PANEL_FORM1_dataGridView1.Columns[14].Name = "Col_txtuser_name";
            this.PANEL_FORM1_dataGridView1.Columns[15].Name = "Col_txtemp_office_name";
            this.PANEL_FORM1_dataGridView1.Columns[16].Name = "Col_txtlog_id";
            this.PANEL_FORM1_dataGridView1.Columns[17].Name = "Col_txtlog_name";
            this.PANEL_FORM1_dataGridView1.Columns[18].Name = "Col_txtdocument_id";
            this.PANEL_FORM1_dataGridView1.Columns[19].Name = "Col_txtversion_id";

            this.PANEL_FORM1_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL_FORM1_dataGridView1.Columns[1].HeaderText = "ซีเรียล";
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderText = " รหัสบริษัทฯ";
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderText = " รหัสสาขา";
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderText = "วันที่ server";
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderText = " เวลา";
            this.PANEL_FORM1_dataGridView1.Columns[6].HeaderText = " ปี";
            this.PANEL_FORM1_dataGridView1.Columns[7].HeaderText = " เดือน";
            this.PANEL_FORM1_dataGridView1.Columns[8].HeaderText = " วัน";
            this.PANEL_FORM1_dataGridView1.Columns[9].HeaderText = "วันที่ client";
            this.PANEL_FORM1_dataGridView1.Columns[10].HeaderText = " ไอพีคอมพิวเตอร์";
            this.PANEL_FORM1_dataGridView1.Columns[11].HeaderText = " ชื่อคอมพิวเตอร์";
            this.PANEL_FORM1_dataGridView1.Columns[12].HeaderText = " Form Code";
            this.PANEL_FORM1_dataGridView1.Columns[13].HeaderText = " ชื่อฟอร์ม";
            this.PANEL_FORM1_dataGridView1.Columns[14].HeaderText = " user name";
            this.PANEL_FORM1_dataGridView1.Columns[15].HeaderText = " ชื่อผู้ใช้";
            this.PANEL_FORM1_dataGridView1.Columns[16].HeaderText = " รหัสรายการ";
            this.PANEL_FORM1_dataGridView1.Columns[17].HeaderText = " รายการ";
            this.PANEL_FORM1_dataGridView1.Columns[18].HeaderText = " เลขที่เอกสาร";
            this.PANEL_FORM1_dataGridView1.Columns[19].HeaderText = " Version";

            this.PANEL_FORM1_dataGridView1.Columns[0].Visible = false;  //"Col_Auto_num";
            this.PANEL_FORM1_dataGridView1.Columns[1].Visible = false;  //"Col_cdkey";

            this.PANEL_FORM1_dataGridView1.Columns[2].Visible = true;  //"Col_txtco_id";
            this.PANEL_FORM1_dataGridView1.Columns[2].Width = 80;
            this.PANEL_FORM1_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[3].Visible = true;  //"Col_txtbranch_id";
            this.PANEL_FORM1_dataGridView1.Columns[3].Width = 80;
            this.PANEL_FORM1_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[4].Visible = true;  //"Col_txttrans_date_server";
            this.PANEL_FORM1_dataGridView1.Columns[4].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[5].Visible = true;  //"Col_txttrans_time";
            this.PANEL_FORM1_dataGridView1.Columns[5].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[6].Visible = true;  //"Col_txttrans_year";
            this.PANEL_FORM1_dataGridView1.Columns[6].Width = 50;
            this.PANEL_FORM1_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[7].Visible = true;  //"Col_txttrans_month";
            this.PANEL_FORM1_dataGridView1.Columns[7].Width = 50;
            this.PANEL_FORM1_dataGridView1.Columns[7].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[8].Visible = true;  //"Col_txttrans_day";
            this.PANEL_FORM1_dataGridView1.Columns[8].Width = 50;
            this.PANEL_FORM1_dataGridView1.Columns[8].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[9].Visible = true;  //"Col_txttrans_date_client";
            this.PANEL_FORM1_dataGridView1.Columns[9].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[9].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_FORM1_dataGridView1.Columns[10].Visible = true;  //"Col_txtcomputer_ip";
            this.PANEL_FORM1_dataGridView1.Columns[10].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[10].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[11].Visible = true;  //"Col_txtcomputer_name";
            this.PANEL_FORM1_dataGridView1.Columns[11].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[11].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[12].Visible = true;  //"Col_txtform_name";
            this.PANEL_FORM1_dataGridView1.Columns[12].Width = 150;
            this.PANEL_FORM1_dataGridView1.Columns[12].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL_FORM1_dataGridView1.Columns[13].Visible = true;  //"Col_txtform_caption";
            this.PANEL_FORM1_dataGridView1.Columns[13].Width = 150;
            this.PANEL_FORM1_dataGridView1.Columns[13].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL_FORM1_dataGridView1.Columns[14].Visible = true;  //"Col_txtuser_name";
            this.PANEL_FORM1_dataGridView1.Columns[14].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[14].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[15].Visible = true;  //"Col_txtemp_office_name";
            this.PANEL_FORM1_dataGridView1.Columns[15].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[15].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[16].Visible = false;  //"Col_txtlog_id";
            this.PANEL_FORM1_dataGridView1.Columns[16].Width = 0;
            this.PANEL_FORM1_dataGridView1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[17].Visible = true;  //"Col_txtlog_name";
            this.PANEL_FORM1_dataGridView1.Columns[17].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[17].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[18].Visible = true;  //"Col_txtdocument_id";
            this.PANEL_FORM1_dataGridView1.Columns[18].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[18].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_FORM1_dataGridView1.Columns[19].Visible = true;  //"Col_txtversion_id";
            this.PANEL_FORM1_dataGridView1.Columns[19].Width = 100;
            this.PANEL_FORM1_dataGridView1.Columns[19].ReadOnly = true;
            this.PANEL_FORM1_dataGridView1.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_FORM1_dataGridView1.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_FORM1_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_FORM1_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_FORM1_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_FORM1_dataGridView1.EnableHeadersVisualStyles = false;

        }
        private void PANEL_FORM1_Clear_GridView1()
        {
            this.PANEL_FORM1_dataGridView1.Rows.Clear();
            this.PANEL_FORM1_dataGridView1.Refresh();
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
            Cursor.Current = Cursors.WaitCursor;

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
                                    " FROM A001_trans_log" +
                                    " WHERE (txtemp_office_name LIKE '%" + PANEL_FORM1_txtsearch.Text.ToString() + "%')" +
                                    " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " ORDER BY ID ASC";

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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_cdkey";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtco_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txttrans_year";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txttrans_month";
                            //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txttrans_day";
                            //this.PANEL_FORM1_dataGridView1.Columns[9].Name = "Col_txttrans_date_client";
                            //this.PANEL_FORM1_dataGridView1.Columns[10].Name = "Col_txtcomputer_ip";
                            //this.PANEL_FORM1_dataGridView1.Columns[11].Name = "Col_txtcomputer_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[12].Name = "Col_txtform_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[13].Name = "Col_txtform_caption";
                            //this.PANEL_FORM1_dataGridView1.Columns[14].Name = "Col_txtuser_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[15].Name = "Col_txtemp_office_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[16].Name = "Col_txtlog_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[17].Name = "Col_txtlog_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[18].Name = "Col_txtdocument_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[19].Name = "Col_txtversion_id";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_cdkey"].Value = dt2.Rows[j]["cdkey"].ToString();      //1
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //2
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //3
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_year"].Value = dt2.Rows[j]["txttrans_year"].ToString();      //6
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_month"].Value = dt2.Rows[j]["txttrans_month"].ToString();      //7
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_day"].Value = dt2.Rows[j]["txttrans_day"].ToString();      //8
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtcomputer_ip"].Value = dt2.Rows[j]["txtcomputer_ip"].ToString();      //9
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtcomputer_name"].Value = dt2.Rows[j]["txtcomputer_name"].ToString();      //10
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtform_name"].Value = dt2.Rows[j]["txtform_name"].ToString();      //11
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtform_caption"].Value = dt2.Rows[j]["txtform_caption"].ToString();      //12
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = dt2.Rows[j]["txtuser_name"].ToString();      //13
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //14
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtlog_id"].Value = dt2.Rows[j]["txtlog_id"].ToString();      //15
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtlog_name"].Value = dt2.Rows[j]["txtlog_name"].ToString();      //16
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtdocument_id"].Value = dt2.Rows[j]["txtdocument_id"].ToString();      //17
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtversion_id"].Value = dt2.Rows[j]["txtversion_id"].ToString();      //18

                        }
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
                    Cursor.Current = Cursors.Default;

                    conn.Close();
                }

                //===========================================
            }
            //================================

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

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
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
            Cursor.Current = Cursors.WaitCursor;

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
                    cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 1));

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

        private void PANEL_FORM1_dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                PANEL_FORM1_dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                PANEL_FORM1_dataGridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }

        private void PANEL_FORM1_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                PANEL_FORM1_dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                PANEL_FORM1_dataGridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }

        private void btnGo2_Click(object sender, EventArgs e)
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
            Cursor.Current = Cursors.WaitCursor;

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
                                    " FROM A001_trans_log" +
                                    " WHERE (txtemp_office_name LIKE '%" + PANEL_FORM1_txtsearch.Text.ToString() + "%')" +
                                    " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " ORDER BY ID ASC";

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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_cdkey";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtco_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbranch_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txttrans_year";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txttrans_month";
                            //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txttrans_day";
                            //this.PANEL_FORM1_dataGridView1.Columns[9].Name = "Col_txttrans_date_client";
                            //this.PANEL_FORM1_dataGridView1.Columns[10].Name = "Col_txtcomputer_ip";
                            //this.PANEL_FORM1_dataGridView1.Columns[11].Name = "Col_txtcomputer_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[12].Name = "Col_txtform_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[13].Name = "Col_txtform_caption";
                            //this.PANEL_FORM1_dataGridView1.Columns[14].Name = "Col_txtuser_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[15].Name = "Col_txtemp_office_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[16].Name = "Col_txtlog_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[17].Name = "Col_txtlog_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[18].Name = "Col_txtdocument_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[19].Name = "Col_txtversion_id";

                            var index = PANEL_FORM1_dataGridView1.Rows.Add();
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_cdkey"].Value = dt2.Rows[j]["cdkey"].ToString();      //1
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //2
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //3
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_year"].Value = dt2.Rows[j]["txttrans_year"].ToString();      //6
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_month"].Value = dt2.Rows[j]["txttrans_month"].ToString();      //7
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_day"].Value = dt2.Rows[j]["txttrans_day"].ToString();      //8
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtcomputer_ip"].Value = dt2.Rows[j]["txtcomputer_ip"].ToString();      //9
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtcomputer_name"].Value = dt2.Rows[j]["txtcomputer_name"].ToString();      //10
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtform_name"].Value = dt2.Rows[j]["txtform_name"].ToString();      //11
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtform_caption"].Value = dt2.Rows[j]["txtform_caption"].ToString();      //12
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtuser_name"].Value = dt2.Rows[j]["txtuser_name"].ToString();      //13
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //14
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtlog_id"].Value = dt2.Rows[j]["txtlog_id"].ToString();      //15
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtlog_name"].Value = dt2.Rows[j]["txtlog_name"].ToString();      //16
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtdocument_id"].Value = dt2.Rows[j]["txtdocument_id"].ToString();      //17
                            PANEL_FORM1_dataGridView1.Rows[index].Cells["Col_txtversion_id"].Value = dt2.Rows[j]["txtversion_id"].ToString();      //18

                        }
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
                    Cursor.Current = Cursors.Default;

                    conn.Close();
                }

                //===========================================
            }
            //================================

        }






        //Tans_Log ====================================================================

    }
}
