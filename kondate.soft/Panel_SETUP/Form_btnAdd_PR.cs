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

namespace kondate.soft.Panel_SETUP
{
    public partial class Form_btnAdd_PR : Form
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


        public Form_btnAdd_PR()
        {
            InitializeComponent();
        }

        private void Form_btnAdd_PR_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.M_COID = "KD";

            this.PANEL_PR_dtpend.Value = DateTime.Now;
            this.PANEL_PR_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpend.CustomFormat = this.PANEL_PR_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.PANEL_PR_dtpstart.Value = DateTime.Today.AddDays(-7);
            this.PANEL_PR_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpstart.CustomFormat = this.PANEL_PR_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

            Show_PANEL_PR_GridView1();
            Fill_Show_DATA_PANEL_PR_GridView1();

            //========================================
            this.PANEL_PR_cboSearch.Items.Add("เลขที่ PR");
            this.PANEL_PR_cboSearch.Items.Add("ชื่อผู้บันทึก PR");
            //========================================

        }

        bool allowResize = false;

        //PANEL_PR====================================================
        private Point MouseDownLocation;
        private void PANEL_PR_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_PR_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PR.Left = e.X + PANEL_PR.Left - MouseDownLocation.X;
                PANEL_PR.Top = e.Y + PANEL_PR.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PR_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_PR_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_PR.Left = e.X + PANEL_PR.Left - MouseDownLocation.X;
                PANEL_PR.Top = e.Y + PANEL_PR.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_PR_btnclose_Click(object sender, EventArgs e)
        {
            this.PANEL_PR.Visible = false;
        }
        private void PANEL_PR_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL_PR_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_PR.Height = PANEL_PR_btnresize_low.Top + e.Y;
                this.PANEL_PR.Width = PANEL_PR_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_PR_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }

        private void PANEL_PR_btnPr_id_Click(object sender, EventArgs e)
        {
            this.PANEL_PR.Visible = true;
            this.PANEL_PR.BringToFront();

        }

        private void Fill_Show_DATA_PANEL_PR_GridView1()
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                   "k013_1db_acc_16department.*" +

                                   " FROM k017db_pr_record" +
                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY k017db_pr_record.txtPr_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtRG_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_date"].Value = dt2.Rows[j]["txtpo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //14
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //14
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "2")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "อนุมัติ PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "3")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ไม่อนุมัติ PO"; //15
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "2")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "คลังรับสินค้าแล้ว"; //15
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
        private void Show_PANEL_PR_GridView1()
        {
            this.PANEL_PR_GridView1.ColumnCount = 17;
            this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
            this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
            this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
            this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";
            this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
            this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
            this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";
            this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
            this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
            this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtRG_id";
            this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtRG_date";
            this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtmoney_after_vat";
            this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtpr_status";
            this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtpo_status";
            this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtRG_status";

            this.PANEL_PR_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_PR_GridView1.Columns[1].HeaderText = "txtco_id";
            this.PANEL_PR_GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.PANEL_PR_GridView1.Columns[3].HeaderText = " PR ID";
            this.PANEL_PR_GridView1.Columns[4].HeaderText = " วันที่";
            this.PANEL_PR_GridView1.Columns[5].HeaderText = " เวลา";
            this.PANEL_PR_GridView1.Columns[6].HeaderText = " รหัสฝ่าย";
            this.PANEL_PR_GridView1.Columns[7].HeaderText = " ฝ่าย";
            this.PANEL_PR_GridView1.Columns[8].HeaderText = " ผู้บันทึก";
            this.PANEL_PR_GridView1.Columns[9].HeaderText = " PO ID";
            this.PANEL_PR_GridView1.Columns[10].HeaderText = " วันที่ PO";
            this.PANEL_PR_GridView1.Columns[11].HeaderText = " RG ID";
            this.PANEL_PR_GridView1.Columns[12].HeaderText = " วันที่ RG";
            this.PANEL_PR_GridView1.Columns[13].HeaderText = " จำนวนเงิน(บาท)";
            this.PANEL_PR_GridView1.Columns[14].HeaderText = " สถานะ PR";
            this.PANEL_PR_GridView1.Columns[15].HeaderText = " สถานะ PO";
            this.PANEL_PR_GridView1.Columns[16].HeaderText = "สถานะ RG";

            this.PANEL_PR_GridView1.Columns[0].Visible = false;  //"Col_Auto_num";
            this.PANEL_PR_GridView1.Columns[1].Visible = false;  //"Col_txtco_id";
            this.PANEL_PR_GridView1.Columns[2].Visible = false;  //"Col_txtbranch_id";

            this.PANEL_PR_GridView1.Columns[3].Visible = true;  //"Col_txtPr_id";
            this.PANEL_PR_GridView1.Columns[3].Width = 150;
            this.PANEL_PR_GridView1.Columns[3].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[4].Visible = true;  //"Col_txttrans_date_server";
            this.PANEL_PR_GridView1.Columns[4].Width = 100;
            this.PANEL_PR_GridView1.Columns[4].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[5].Visible = true;  //"Col_txttrans_time";
            this.PANEL_PR_GridView1.Columns[5].Width = 80;
            this.PANEL_PR_GridView1.Columns[5].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[6].Visible = false;  //"Col_txtdepartment_id";

            this.PANEL_PR_GridView1.Columns[7].Visible = true;  //"Col_txtdepartment_name";
            this.PANEL_PR_GridView1.Columns[7].Width = 100;
            this.PANEL_PR_GridView1.Columns[7].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[8].Visible = true;  //"Col_txtemp_office_name";
            this.PANEL_PR_GridView1.Columns[8].Width = 120;
            this.PANEL_PR_GridView1.Columns[8].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[9].Visible = true;  //"Col_txtPo_id";
            this.PANEL_PR_GridView1.Columns[9].Width = 100;
            this.PANEL_PR_GridView1.Columns[9].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[10].Visible = true;  //"Col_txtpo_date";
            this.PANEL_PR_GridView1.Columns[10].Width = 100;
            this.PANEL_PR_GridView1.Columns[10].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[11].Visible = true;  //"Col_txtRG_id";
            this.PANEL_PR_GridView1.Columns[11].Width = 100;
            this.PANEL_PR_GridView1.Columns[11].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[12].Visible = true;  //"Col_txtRG_date";
            this.PANEL_PR_GridView1.Columns[12].Width = 100;
            this.PANEL_PR_GridView1.Columns[12].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_PR_GridView1.Columns[13].Visible = true;  //"Col_txtmoney_after_vat";
            this.PANEL_PR_GridView1.Columns[13].Width = 130;
            this.PANEL_PR_GridView1.Columns[13].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_PR_GridView1.Columns[14].Visible = true;  //"Col_txtpr_status";
            this.PANEL_PR_GridView1.Columns[14].Width = 100;
            this.PANEL_PR_GridView1.Columns[14].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[15].Visible = true;  //"Col_txtpo_status";
            this.PANEL_PR_GridView1.Columns[15].Width = 100;
            this.PANEL_PR_GridView1.Columns[15].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.Columns[16].Visible = true;  //"Col_txtRG_status";
            this.PANEL_PR_GridView1.Columns[16].Width = 100;
            this.PANEL_PR_GridView1.Columns[16].ReadOnly = true;
            this.PANEL_PR_GridView1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_PR_GridView1.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_PR_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_PR_GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_PR_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_PR_GridView1.EnableHeadersVisualStyles = false;

        }
        private void Clear_PANEL_PR_GridView1()
        {
            this.PANEL_PR_GridView1.Rows.Clear();
            this.PANEL_PR_GridView1.Refresh();
        }
        private void PANEL_PR_GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_PR_GridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL_PR_txtPr_id.Text = row.Cells[3].Value.ToString();

                    if (this.PANEL_PR_cboSearch.Text == "เลขที่ PR")
                    {
                        this.PANEL_PR_txtsearch.Text = row.Cells[3].Value.ToString();
                        this.PANEL_PR_txtPr_id.Text = row.Cells[3].Value.ToString();

                    }
                    else if (this.PANEL_PR_cboSearch.Text == "ชื่อผู้บันทึก PR")
                    {
                        this.PANEL_PR_txtsearch.Text = row.Cells[8].Value.ToString();

                    }
                    else
                    {
                        this.PANEL_PR_txtsearch.Text = row.Cells[3].Value.ToString();
                        this.PANEL_PR_txtPr_id.Text  = row.Cells[3].Value.ToString();

                    }
                }
                //=====================
            }
        }

        private void PANEL_PR_dtpstart_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_PR_dtpstart.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpstart.CustomFormat = this.PANEL_PR_dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void PANEL_PR_dtpend_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_PR_dtpend.Format = DateTimePickerFormat.Custom;
            this.PANEL_PR_dtpend.CustomFormat = this.PANEL_PR_dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void PANEL_PR_btnGo2_Click(object sender, EventArgs e)
        {
            Fill_Show_DATA_PANEL_PR_GridView1();
        }

        private void PANEL_PR_btnGo3_Click(object sender, EventArgs e)
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                   "k013_1db_acc_16department.*" +

                                   " FROM k017db_pr_record" +
                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " AND (k017db_pr_record.txtdepartment_id = '" + this.PANEL1316_DEPARTMENT_txtdepartment_id.Text.Trim() + "')" +
                                   " ORDER BY k017db_pr_record.txtPr_id ASC";

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtRG_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_date"].Value = dt2.Rows[j]["txtpo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //14
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //14
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "2")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "อนุมัติ PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "3")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ไม่อนุมัติ PO"; //15
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "2")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "คลังรับสินค้าแล้ว"; //15
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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

        private void PANEL_PR_btnGo1_Click(object sender, EventArgs e)
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

            Clear_PANEL_PR_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (this.PANEL_PR_cboSearch.Text == "เลขที่ PR")
                {
                    cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                       "k013_1db_acc_16department.*" +

                                       " FROM k017db_pr_record" +
                                       " INNER JOIN k013_1db_acc_16department" +
                                       " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                       " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " AND (k017db_pr_record.txtPr_id = '" + this.PANEL_PR_txtsearch.Text.Trim() + "')" +
                                      " ORDER BY k017db_pr_record.txtPr_id ASC";

                }
                if (this.PANEL_PR_cboSearch.Text == "ชื่อผู้บันทึก PR")
                {
                    cmd2.CommandText = "SELECT k017db_pr_record.*," +
                                       "k013_1db_acc_16department.*" +

                                       " FROM k017db_pr_record" +
                                       " INNER JOIN k013_1db_acc_16department" +
                                       " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                       " WHERE (k017db_pr_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k017db_pr_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (k017db_pr_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (k017db_pr_record.txtemp_office_name LIKE '%" + this.PANEL_PR_txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY k017db_pr_record.txtPr_id ASC";

                }

                cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.PANEL_PR_dtpstart.Value;
                cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.PANEL_PR_dtpend.Value;

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.PANEL_PR_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_PR_GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.PANEL_PR_GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.PANEL_PR_GridView1.Columns[3].Name = "Col_txtPr_id";
                            //this.PANEL_PR_GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.PANEL_PR_GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.PANEL_PR_GridView1.Columns[6].Name = "Col_txtdepartment_id";
                            //this.PANEL_PR_GridView1.Columns[7].Name = "Col_txtdepartment_name";
                            //this.PANEL_PR_GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.PANEL_PR_GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.PANEL_PR_GridView1.Columns[10].Name = "Col_txtpo_date";
                            //this.PANEL_PR_GridView1.Columns[11].Name = "Col_txtRG_id";
                            //this.PANEL_PR_GridView1.Columns[12].Name = "Col_txtRG_date";
                            //this.PANEL_PR_GridView1.Columns[13].Name = "Col_txtmoney_after_vat";
                            //this.PANEL_PR_GridView1.Columns[14].Name = "Col_txtpr_status";
                            //this.PANEL_PR_GridView1.Columns[15].Name = "Col_txtpo_status";
                            //this.PANEL_PR_GridView1.Columns[16].Name = "Col_txtRG_status";
                            //    Convert.ToDateTime(dt.Rows[i]["txtreceipt_date"]).ToString("dd-MM-yyyy", UsaCulture)
                            var index = this.PANEL_PR_GridView1.Rows.Add();
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //3
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //6
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //7
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_date"].Value = dt2.Rows[j]["txtpo_date"].ToString();      //10
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //11
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //12
                            this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //13


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //14
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //14
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "2")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "อนุมัติ PO"; //15
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "3")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ไม่อนุมัติ PO"; //15
                            }


                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //15
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "2")
                            {
                                this.PANEL_PR_GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "คลังรับสินค้าแล้ว"; //15
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.PANEL_PR_txtcount_rows.Text = dt2.Rows.Count.ToString();
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

        private void PANEL1316_DEPARTMENT_btndepartment_Click(object sender, EventArgs e)
        {

        }

        //END PANEL_PR====================================================

    }
}
