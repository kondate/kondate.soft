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

namespace kondate.soft.HOME02_Purchasing
{
    public partial class HOME02_Purchasing_05RG : Form
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


        public HOME02_Purchasing_05RG()
        {
            InitializeComponent();
        }

        private void HOME02_Purchasing_05RG_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0204APGR";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblword_status.Text = "ระเบียนใบรับสินค้า หรือ วัตถุดิบ";
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.ActiveControl = this.txtsearch;

            this.BtnSave.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.BtnPrint.Enabled = false;
            this.btnPreview.Enabled = false;

            this.dtpend.Value = DateTime.Now;
            this.dtpend.Format = DateTimePickerFormat.Custom;
            this.dtpend.CustomFormat = this.dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.dtpstart.Value = DateTime.Today.AddDays(-7);
            this.dtpstart.Format = DateTimePickerFormat.Custom;
            this.dtpstart.CustomFormat = this.dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

            //========================================
            this.cboSearch.Items.Add("เลขที่ใบรับสินค้า");
            this.cboSearch.Items.Add("รหัสผู้รับสินค้า");
            this.cboSearch.Text = "เลขที่ใบรับสินค้า";

            //========================================
            PANEL2_BRANCH_GridView1_branch();
            PANEL2_BRANCH_Fill_branch();

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();

            Show_GridView1();
            Fill_Show_DATA_GridView1();

        }

        private void Fill_Show_DATA_GridView1()
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

            Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k020db_receive_record.*," +
                                   //"k018db_po_record.*," +
                                   "k020db_receive_record_type.*," +
                                   "k016db_1supplier.*" +

                                   " FROM k020db_receive_record" +

                                   //" INNER JOIN k018db_po_record" +
                                   //" ON k020db_receive_record.cdkey = k018db_po_record.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                   //" AND k020db_receive_record.txtPo_id = k018db_po_record.txtPo_id" +
                                   " INNER JOIN k020db_receive_record_type" +
                                   " ON k020db_receive_record.cdkey = k020db_receive_record_type.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k020db_receive_record_type.txtco_id" +
                                   " AND k020db_receive_record.txtreceive_type_id = k020db_receive_record_type.txtreceive_type_id" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON k020db_receive_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND k020db_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k020db_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " WHERE (k020db_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k020db_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (k020db_receive_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (k020db_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY k020db_receive_record.txtRG_id ASC";

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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView1.Columns[3].Name = "Col_txtreceive_type_id";
                            //this.GridView1.Columns[4].Name = "Col_txtreceive_type_name";

                            //this.GridView1.Columns[3].Name = "Col_txtRG_id";
                            //this.GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.GridView1.Columns[10].Name = "Col_txtmoney_after_vat";
                            //this.GridView1.Columns[11].Name = "Col_txtmoney_after_vat_creditor";
                            //this.GridView1.Columns[12].Name = "Col_txtRG_status";

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_id"].Value = dt2.Rows[j]["txtreceive_type_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_name"].Value = dt2.Rows[j]["txtreceive_type_name"].ToString();      //2

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //10
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat_creditor"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat_creditor"]).ToString("###,###.00");      //11

                            //RG==============================
                            if (dt2.Rows[j]["txtrg_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //12
                            }
                            else if (dt2.Rows[j]["txtrg_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //12
                            }


                        }
                        //=======================================================
                    }
                    else
                    {
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            GridView1_Color();
        }
        private void Fill_Show_BRANCH_DATA_GridView1()
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

            Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k020db_receive_record.*," +
                                   //"k018db_po_record.*," +
                                   "k020db_receive_record_type.*," +
                                   "k016db_1supplier.*" +

                                   " FROM k020db_receive_record" +

                                   //" INNER JOIN k018db_po_record" +
                                   //" ON k020db_receive_record.cdkey = k018db_po_record.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                   //" AND k020db_receive_record.txtPo_id = k018db_po_record.txtPo_id" +
                                   " INNER JOIN k020db_receive_record_type" +
                                   " ON k020db_receive_record.cdkey = k020db_receive_record_type.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k020db_receive_record_type.txtco_id" +
                                   " AND k020db_receive_record.txtreceive_type_id = k020db_receive_record_type.txtreceive_type_id" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON k020db_receive_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND k020db_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k020db_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " WHERE (k020db_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k020db_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //" AND (k020db_receive_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (k020db_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY k020db_receive_record.txtRG_id ASC";

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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView1.Columns[3].Name = "Col_txtRG_id";
                            //this.GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.GridView1.Columns[10].Name = "Col_txtmoney_after_vat";
                            //this.GridView1.Columns[11].Name = "Col_txtRG_status";

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_id"].Value = dt2.Rows[j]["txtreceive_type_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_name"].Value = dt2.Rows[j]["txtreceive_type_name"].ToString();      //2

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //10
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat_creditor"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat_creditor"]).ToString("###,###.00");      //11

                            //RG==============================
                            if (dt2.Rows[j]["txtrg_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //12
                            }
                            else if (dt2.Rows[j]["txtrg_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //12
                            }


                        }
                        //=======================================================
                    }
                    else
                    {
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            GridView1_Color();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 15;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtco_id";
            this.GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.GridView1.Columns[3].Name = "Col_txtreceive_type_id";
            this.GridView1.Columns[4].Name = "Col_txtreceive_type_name";

            this.GridView1.Columns[5].Name = "Col_txtRG_id";
            this.GridView1.Columns[6].Name = "Col_txttrans_date_server";
            this.GridView1.Columns[7].Name = "Col_txttrans_time";
            this.GridView1.Columns[8].Name = "Col_txtsupplier_id";
            this.GridView1.Columns[9].Name = "Col_txtsupplier_name";
            this.GridView1.Columns[10].Name = "Col_txtemp_office_name";
            this.GridView1.Columns[11].Name = "Col_txtPo_id";
            this.GridView1.Columns[12].Name = "Col_txtmoney_after_vat";
            this.GridView1.Columns[13].Name = "Col_txtmoney_after_vat_creditor";
            this.GridView1.Columns[14].Name = "Col_txtRG_status";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "txtco_id";
            this.GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.GridView1.Columns[3].HeaderText = " รหัสประเภทรับ";
            this.GridView1.Columns[4].HeaderText = " ประเภทรับ";

            this.GridView1.Columns[5].HeaderText = " RG ID";
            this.GridView1.Columns[6].HeaderText = " วันที่";
            this.GridView1.Columns[7].HeaderText = " เวลา";
            this.GridView1.Columns[8].HeaderText = " รหัส Supplier";
            this.GridView1.Columns[9].HeaderText = " ชื่อ Supplier";
            this.GridView1.Columns[10].HeaderText = " ผู้บันทึก";
            this.GridView1.Columns[11].HeaderText = " PO ID";
            this.GridView1.Columns[12].HeaderText = " จำนวนเงิน";
            this.GridView1.Columns[13].HeaderText = " จำนวนเงินค้างจ่าย";
            this.GridView1.Columns[14].HeaderText = " สถานะ";

            this.GridView1.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView1.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.GridView1.Columns["Col_txtbranch_id"].Visible = false;  //"Col_txtbranch_id";

            this.GridView1.Columns["Col_txtreceive_type_id"].Visible = false;  //"Col_txtreceive_type_id";

            this.GridView1.Columns["Col_txtreceive_type_name"].Visible = true;  //"Col_txtRG_id";
            this.GridView1.Columns["Col_txtreceive_type_name"].Width = 150;
            this.GridView1.Columns["Col_txtreceive_type_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtreceive_type_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtreceive_type_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtRG_id"].Visible = true;  //"Col_txtRG_id";
            this.GridView1.Columns["Col_txtRG_id"].Width = 120;
            this.GridView1.Columns["Col_txtRG_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtRG_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtRG_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txttrans_date_server"].Visible = true;  //"Col_txttrans_date_server";
            this.GridView1.Columns["Col_txttrans_date_server"].Width = 100;
            this.GridView1.Columns["Col_txttrans_date_server"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_date_server"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_date_server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txttrans_time"].Visible = true;  //"Col_txttrans_time";
            this.GridView1.Columns["Col_txttrans_time"].Width = 80;
            this.GridView1.Columns["Col_txttrans_time"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsupplier_id"].Visible = false;  //"Col_txtsupplier_id";

            this.GridView1.Columns["Col_txtsupplier_name"].Visible = true;  //"Col_txtsupplier_name";
            this.GridView1.Columns["Col_txtsupplier_name"].Width = 250;
            this.GridView1.Columns["Col_txtsupplier_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsupplier_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsupplier_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtemp_office_name"].Visible = true;  //"Col_txtemp_office_name";
            this.GridView1.Columns["Col_txtemp_office_name"].Width = 120;
            this.GridView1.Columns["Col_txtemp_office_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtemp_office_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtemp_office_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //this.GridView1.Columns[9].Name = "Col_txtPo_id";
            //this.GridView1.Columns[10].Name = "Col_txtmoney_after_vat";
            //this.GridView1.Columns[11].Name = "Col_txtRG_status";

            this.GridView1.Columns["Col_txtPo_id"].Visible = true;  //"Col_txtPo_id";
            this.GridView1.Columns["Col_txtPo_id"].Width = 120;
            this.GridView1.Columns["Col_txtPo_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtPo_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtPo_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmoney_after_vat"].Visible = true;  //"Col_txtmoney_after_vat";
            this.GridView1.Columns["Col_txtmoney_after_vat"].Width = 120;
            this.GridView1.Columns["Col_txtmoney_after_vat"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmoney_after_vat"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmoney_after_vat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtmoney_after_vat_creditor"].Visible = true;  //"Col_txtmoney_after_vat_creditor";
            this.GridView1.Columns["Col_txtmoney_after_vat_creditor"].Width = 120;
            this.GridView1.Columns["Col_txtmoney_after_vat_creditor"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmoney_after_vat_creditor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmoney_after_vat_creditor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView1.Columns["Col_txtRG_status"].Visible = true;  //"Col_txtRG_status";
            this.GridView1.Columns["Col_txtRG_status"].Width = 100;
            this.GridView1.Columns["Col_txtRG_status"].ReadOnly = true;
            this.GridView1.Columns["Col_txtRG_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtRG_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


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
        private void GridView1_Color()
        {
            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {

                if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtmoney_after_vat_creditor"].Value.ToString())) > 0)
                {
                    GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }

            }
        }
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtRG_id"].Value;
                if (cell != null)
                {
                    W_ID_Select.RECEIVE_TYPE = row.Cells["Col_txtreceive_type_id"].Value.ToString();
                    W_ID_Select.TRANS_ID = row.Cells["Col_txtRG_id"].Value.ToString();
                    this.cboSearch.Text = "เลขที่ใบรับสินค้า";

                    if (this.cboSearch.Text == "เลขที่ใบรับสินค้า")
                    {
                        this.txtsearch.Text = row.Cells["Col_txtRG_id"].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells["Col_txtRG_id"].Value.ToString();

                    }
                    else if (this.cboSearch.Text == "รหัสผู้รับสินค้า")
                    {
                        this.txtsearch.Text = row.Cells["Col_txtemp_office_name"].Value.ToString();

                    }
                    else
                    {
                        this.txtsearch.Text = row.Cells["Col_txtRG_id"].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells["Col_txtRG_id"].Value.ToString();

                    }
                }
                //=====================
            }
        }
        private void GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "N")
            {

                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            else
            {
                W_ID_Select.LOG_ID = "4";
                W_ID_Select.LOG_NAME = "เปิดแก้ไข";
                W_ID_Select.WORD_TOP = "ดูข้อมูลใบรับสินค้า";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record_detail frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record_detail();
                frm2.Show();

                TRANS_LOG();

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
        private void panel_top_MouseDown(object sender, MouseEventArgs e)
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
        private void panel_button_top_pictureBox_MouseDown(object sender, MouseEventArgs e)
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
            else
            {
                W_ID_Select.LOG_ID = "3";
                W_ID_Select.LOG_NAME = "ใหม่";
                TRANS_LOG();

                W_ID_Select.WORD_TOP = "ออกใบรับสินค้า";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record();
                frm2.Show();
                //this.Close();
            }
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "N")
            {

                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            else
            {
                W_ID_Select.LOG_ID = "4";
                W_ID_Select.LOG_NAME = "เปิดแก้ไข";
                W_ID_Select.WORD_TOP = "ดูข้อมูลใบรับสินค้า";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record_detail frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record_detail();
                frm2.Show();

                TRANS_LOG();

            }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void BtnCancel_Doc_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "N")
            {

                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            else
            {
                W_ID_Select.LOG_ID = "7";
                W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";

                W_ID_Select.WORD_TOP = "ยกเลิกใบรับสินค้า)";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record_detail frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_05RG_record_detail();
                frm2.Show();
            }
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
        private void btnGo1_Click(object sender, EventArgs e)
        {
            if (this.ch_all_branch.Checked==true)
            {
                Fill_Show_BRANCH_DATA_GridView1();

            }
            else
            {
                Fill_Show_DATA_GridView1();

            }
        }

        private void btnGo2_Click(object sender, EventArgs e)
        {
            Fill_Show_DATA_GridView1();

        }

        private void btnGo3_Click(object sender, EventArgs e)
        {
            //this.cboSearch.Items.Add("เลขที่ ใบรับสินค้า");
            //this.cboSearch.Items.Add("ชื่อผู้รับสินค้า");
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

            Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.cboSearch.Items.Add("เลขที่ ใบรับสินค้า");
                //this.cboSearch.Items.Add("ชื่อผู้รับสินค้า");
                if (this.cboSearch.Text == "เลขที่ใบรับสินค้า")
                {
                    cmd2.CommandText = "SELECT k020db_receive_record.*," +
                                       //"k018db_po_record.*," +
                                       "k020db_receive_record_type.*," +
                                       "k016db_1supplier.*" +

                                       " FROM k020db_receive_record" +

                                       //" INNER JOIN k018db_po_record" +
                                       //" ON k020db_receive_record.cdkey = k018db_po_record.cdkey" +
                                       //" AND k020db_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                       //" AND k020db_receive_record.txtPo_id = k018db_po_record.txtPo_id" +
                                       " INNER JOIN k020db_receive_record_type" +
                                       " ON k020db_receive_record.cdkey = k020db_receive_record_type.cdkey" +
                                       //" AND k020db_receive_record.txtco_id = k020db_receive_record_type.txtco_id" +
                                       " AND k020db_receive_record.txtreceive_type_id = k020db_receive_record_type.txtreceive_type_id" +

                                           " INNER JOIN k016db_1supplier" +
                                       " ON k020db_receive_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k020db_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k020db_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " WHERE (k020db_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k020db_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //" AND (k020db_receive_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (k020db_receive_record.txtRG_id = '" + this.txtsearch.Text.Trim() + "')" +
                                       " AND (k020db_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY k020db_receive_record.txtRG_id ASC";

                }
                if (this.cboSearch.Text == "รหัสผู้รับสินค้า")
                {
                    cmd2.CommandText = "SELECT k020db_receive_record.*," +
                                       //"k018db_po_record.*," +
                                       "k020db_receive_record_type.*," +
                                       "k016db_1supplier.*" +

                                       " FROM k020db_receive_record" +

                                       //" INNER JOIN k018db_po_record" +
                                       //" ON k020db_receive_record.cdkey = k018db_po_record.cdkey" +
                                       //" AND k020db_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                       //" AND k020db_receive_record.txtPo_id = k018db_po_record.txtPo_id" +
                                       " INNER JOIN k020db_receive_record_type" +
                                       " ON k020db_receive_record.cdkey = k020db_receive_record_type.cdkey" +
                                       //" AND k020db_receive_record.txtco_id = k020db_receive_record_type.txtco_id" +
                                       " AND k020db_receive_record.txtreceive_type_id = k020db_receive_record_type.txtreceive_type_id" +

                                           " INNER JOIN k016db_1supplier" +
                                       " ON k020db_receive_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k020db_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k020db_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " WHERE (k020db_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k020db_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //" AND (k020db_receive_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (k020db_receive_record.txtemp_id = '" + this.txtsearch.Text.Trim() + "')" +
                                       " AND (k020db_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY k020db_receive_record.txtRG_id ASC";

                }
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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView1.Columns[3].Name = "Col_txtRG_id";
                            //this.GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.GridView1.Columns[10].Name = "Col_txtmoney_after_vat";
                            //this.GridView1.Columns[11].Name = "Col_txtRG_status";

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_id"].Value = dt2.Rows[j]["txtreceive_type_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_name"].Value = dt2.Rows[j]["txtreceive_type_name"].ToString();      //2

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //10
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat_creditor"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat_creditor"]).ToString("###,###.00");      //11

                            //RG==============================
                            if (dt2.Rows[j]["txtrg_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //12
                            }
                            else if (dt2.Rows[j]["txtrg_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //12
                            }


                        }
                        //=======================================================
                    }
                    else
                    {
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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

            GridView1_Color();

        }

        private void btnGo4_Click(object sender, EventArgs e)
        {
            //this.cboSearch.Items.Add("เลขที่ ใบรับสินค้า");
            //this.cboSearch.Items.Add("ชื่อผู้รับสินค้า");
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

            Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.cboSearch.Items.Add("เลขที่ ใบรับสินค้า");
                //this.cboSearch.Items.Add("ชื่อผู้รับสินค้า");

                cmd2.CommandText = "SELECT k020db_receive_record.*," +
                                   //"k018db_po_record.*," +
                                   "k020db_receive_record_type.*," +
                                   "k016db_1supplier.*" +

                                   " FROM k020db_receive_record" +

                                   //" INNER JOIN k018db_po_record" +
                                   //" ON k020db_receive_record.cdkey = k018db_po_record.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                   //" AND k020db_receive_record.txtPo_id = k018db_po_record.txtPo_id" +
                                   " INNER JOIN k020db_receive_record_type" +
                                   " ON k020db_receive_record.cdkey = k020db_receive_record_type.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k020db_receive_record_type.txtco_id" +
                                   " AND k020db_receive_record.txtreceive_type_id = k020db_receive_record_type.txtreceive_type_id" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON k020db_receive_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND k020db_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k020db_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " WHERE (k020db_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (k020db_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //" AND (k020db_receive_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (k020db_receive_record.txtsupplier_id = '" + this.PANEL161_SUP_txtsupplier_id.Text.Trim() + "')" +
                                       " AND (k020db_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY k020db_receive_record.txtRG_id ASC";



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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView1.Columns[3].Name = "Col_txtRG_id";
                            //this.GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.GridView1.Columns[10].Name = "Col_txtmoney_after_vat";
                            //this.GridView1.Columns[11].Name = "Col_txtRG_status";

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_id"].Value = dt2.Rows[j]["txtreceive_type_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_name"].Value = dt2.Rows[j]["txtreceive_type_name"].ToString();      //2

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //10
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat_creditor"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat_creditor"]).ToString("###,###.00");      //11

                            //RG==============================
                            if (dt2.Rows[j]["txtrg_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //12
                            }
                            else if (dt2.Rows[j]["txtrg_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //12
                            }

                        }
                        //=======================================================
                    }
                    else
                    {
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            GridView1_Color();

        }

        private void btnGo5_Click(object sender, EventArgs e)
        {
            //this.cboSearch.Items.Add("เลขที่ ใบรับสินค้า");
            //this.cboSearch.Items.Add("ชื่อผู้รับสินค้า");
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

            Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.cboSearch.Items.Add("เลขที่ ใบรับสินค้า");
                //this.cboSearch.Items.Add("ชื่อผู้รับสินค้า");

                cmd2.CommandText = "SELECT k020db_receive_record.*," +
                                   //"k018db_po_record.*," +
                                   "k020db_receive_record_type.*," +
                                   "k016db_1supplier.*" +

                                   " FROM k020db_receive_record" +

                                   //" INNER JOIN k018db_po_record" +
                                   //" ON k020db_receive_record.cdkey = k018db_po_record.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                   //" AND k020db_receive_record.txtPo_id = k018db_po_record.txtPo_id" +
                                   " INNER JOIN k020db_receive_record_type" +
                                   " ON k020db_receive_record.cdkey = k020db_receive_record_type.cdkey" +
                                   //" AND k020db_receive_record.txtco_id = k020db_receive_record_type.txtco_id" +
                                   " AND k020db_receive_record.txtreceive_type_id = k020db_receive_record_type.txtreceive_type_id" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON k020db_receive_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND k020db_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND k020db_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " WHERE (k020db_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k020db_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //" AND (k020db_receive_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (k020db_receive_record.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                   " AND (k020db_receive_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY k020db_receive_record.txtRG_id ASC";



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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            //this.GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.GridView1.Columns[1].Name = "Col_txtco_id";
                            //this.GridView1.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView1.Columns[3].Name = "Col_txtRG_id";
                            //this.GridView1.Columns[4].Name = "Col_txttrans_date_server";
                            //this.GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView1.Columns[6].Name = "Col_txtsupplier_id";
                            //this.GridView1.Columns[7].Name = "Col_txtsupplier_name";
                            //this.GridView1.Columns[8].Name = "Col_txtemp_office_name";
                            //this.GridView1.Columns[9].Name = "Col_txtPo_id";
                            //this.GridView1.Columns[10].Name = "Col_txtmoney_after_vat";
                            //this.GridView1.Columns[11].Name = "Col_txtRG_status";

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_id"].Value = dt2.Rows[j]["txtreceive_type_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtreceive_type_name"].Value = dt2.Rows[j]["txtreceive_type_name"].ToString();      //2

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //10
                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat_creditor"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat_creditor"]).ToString("###,###.00");      //11

                            //RG==============================
                            if (dt2.Rows[j]["txtrg_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //12
                            }
                            else if (dt2.Rows[j]["txtrg_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //12
                            }


                        }
                        //=======================================================
                    }
                    else
                    {
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            GridView1_Color();

        }

        private void dtpstart_ValueChanged(object sender, EventArgs e)
        {
            this.dtpstart.Format = DateTimePickerFormat.Custom;
            this.dtpstart.CustomFormat = this.dtpstart.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        private void dtpend_ValueChanged(object sender, EventArgs e)
        {
            this.dtpend.Format = DateTimePickerFormat.Custom;
            this.dtpend.CustomFormat = this.dtpend.Value.ToString("dd-MM-yyyy", UsaCulture);

        }
        //Branch=======================================================================
        private void PANEL2_BRANCH_Fill_branch()
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

            PANEL2_BRANCH_Clear_GridView1_branch();


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
                            var index = PANEL2_BRANCH_dataGridView1_branch.Rows.Add();
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //1
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_name"].Value = dt2.Rows[j]["txtbranch_name"].ToString();      //2
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_status"].Value = dt2.Rows[j]["txtbranch_status"].ToString();      //4
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
        private void PANEL2_BRANCH_GridView1_branch()
        {
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnCount = 5;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].Name = "Col_Auto_num";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Name = "Col_txtbranch_id";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Name = "Col_txtbranch_name";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Name = "Col_txthome_id_full";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Name = "Col_txtbranch_status";

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].HeaderText = "No";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].HeaderText = "รหัสสาขา";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].HeaderText = "ชื่อสาขา";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].HeaderText = "ที่อยู่";  //
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].HeaderText = "สถานะ";

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].Visible = false;  //"No";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Visible = true;  //"Col_txtbranch_id";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Width = 100;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Visible = true;  //"Col_txtbranch_name";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Width = 150;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Visible = true; // "Col_txthome_id_full
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Width = 250;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Visible = true;  // "Col_txtbranch_status
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Width = 50;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL2_BRANCH_dataGridView1_branch.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL2_BRANCH_dataGridView1_branch.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL2_BRANCH_dataGridView1_branch.EnableHeadersVisualStyles = false;

        }
        private void PANEL2_BRANCH_Clear_GridView1_branch()
        {
            this.PANEL2_BRANCH_dataGridView1_branch.Rows.Clear();
            this.PANEL2_BRANCH_dataGridView1_branch.Refresh();
        }
        private void txtbranch_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL2_BRANCH.Visible == false)
                {
                    this.PANEL2_BRANCH.Visible = true;
                    this.PANEL2_BRANCH.BringToFront();
                    this.PANEL2_BRANCH.Location = new Point(this.txtbranch_name.Location.X + 133, this.txtbranch_name.Location.Y + 142);
                    this.PANEL2_BRANCH_dataGridView1_branch.Focus();
                }
                else
                {
                    this.PANEL2_BRANCH.Visible = false;
                }

        }
        private void btnbranch_Click(object sender, EventArgs e)
        {
            if (this.PANEL2_BRANCH.Visible == false)
            {
                this.PANEL2_BRANCH.Visible = true;
                this.PANEL2_BRANCH.BringToFront();
                this.PANEL2_BRANCH.Location = new Point(this.txtbranch_name.Location.X, this.txtbranch_name.Location.Y + 22);
                this.PANEL2_BRANCH_dataGridView1_branch.Focus();
            }
            else
            {
                this.PANEL2_BRANCH.Visible = false;
            }
        }
        private void PANEL2_BRANCH_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL2_BRANCH.Visible == false)
            {
                this.PANEL2_BRANCH.Visible = true;
            }
            else
            {
                this.PANEL2_BRANCH.Visible = false;
            }
        }
        private void PANEL2_BRANCH_dataGridView1_branch_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL2_BRANCH_dataGridView1_branch.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.txtbranch_id.Text = row.Cells[1].Value.ToString();
                    this.txtbranch_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL2_BRANCH_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL2_BRANCH_btn_search_Click(object sender, EventArgs e)
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

            PANEL2_BRANCH_Clear_GridView1_branch();


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
                                  " AND (txtbranch_name LIKE '%" + this.PANEL2_BRANCH_txtsearch.Text + "%')" +
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
                            var index = PANEL2_BRANCH_dataGridView1_branch.Rows.Add();
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //1
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_name"].Value = dt2.Rows[j]["txtbranch_name"].ToString();      //2
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_status"].Value = dt2.Rows[j]["txtbranch_status"].ToString();      //4
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
        private void PANEL2_BRANCH_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;

        }
        private void PANEL2_BRANCH_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL2_BRANCH.Height = PANEL2_BRANCH_btnresize_low.Top + e.Y;
                this.PANEL2_BRANCH.Width = PANEL2_BRANCH_btnresize_low.Left + e.X;
            }
        }
        private void PANEL2_BRANCH_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;

        }
        private void PANEL2_BRANCH_Fill_branch_Edit()
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

                cmd2.CommandText = "SELECT *" +
                                  " FROM k008db_branch" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtbranch_id = '" + txtbranch_id.Text.Trim() + "')" +
                                " ORDER BY ID ASC";
                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        txtbranch_name.Text = dt2.Rows[0]["txtbranch_name"].ToString();      //2

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

        //Branch=======================================================================




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
                    this.PANEL161_SUP_txtsupplier_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL161_SUP_txtsupplier_name.Text = row.Cells[3].Value.ToString();
                    //Col_txtcontact_person
                    //this.txtcontact_person.Text = row.Cells["Col_txtcontact_person"].Value.ToString();
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

        //END txtsupplier Supplier  =======================================================================


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

        //END txtwherehouse คลังสินค้า  =======================================================================


        //=============================================================================================

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



















        //Tans_Log ====================================================================
    }
}
