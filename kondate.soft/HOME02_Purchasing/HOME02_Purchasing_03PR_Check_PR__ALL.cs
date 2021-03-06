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
    public partial class HOME02_Purchasing_03PR_Check_PR__ALL : Form
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



        public HOME02_Purchasing_03PR_Check_PR__ALL()
        {
            InitializeComponent();
        }

        private void HOME02_Purchasing_03PR_Check_PR__ALL_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0203PRGR";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblword_status.Text = "ระเบียนติดตามสถานะ ใบสั่งซื้อ PO";
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.ActiveControl = this.txtsearch;

            this.BtnNew.Enabled = false;
            this.btnopen.Enabled = false;
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
            this.cboSearch.Items.Add("เลขที่ PO");
            this.cboSearch.Items.Add("ชื่อ Supplier");
            //========================================
            PANEL2_BRANCH_GridView1_branch();
            PANEL2_BRANCH_Fill_branch();


            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();

            Show_GridView1();
            Fill_Show_DATA_GridView1();

            Show_GridView2();
            Show_GridView3();

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


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM k017db_pr_all" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2


                            this.GridView1.Rows[index].Cells["Col_txtstatus_remark"].Value = dt2.Rows[j]["txtstatus_remark"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //4
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtpo_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //9
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //13
                            this.GridView1.Rows[index].Cells["Col_txtapprove_name"].Value = dt2.Rows[j]["txtapprove_name"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //16
                            this.GridView1.Rows[index].Cells["Col_txtRG_name"].Value = dt2.Rows[j]["txtRG_name"].ToString();      //17

                            this.GridView1.Rows[index].Cells["Col_txtReceive_id"].Value = dt2.Rows[j]["txtReceive_id"].ToString();      //18
                            this.GridView1.Rows[index].Cells["Col_txtReceive_date"].Value = dt2.Rows[j]["txtReceive_date"].ToString();      //19
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //20
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //21

                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //22


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //23
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //23
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //24
                            }


                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }

                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //26
                            }

                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //25
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //24
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิก รับเข้าคลัง"; //24
                            }
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //23
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_receive"]).ToString("###,###.00");      //24
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_balance"]).ToString("###,###.00");      //25



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
            GridView1_Color_Column();
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


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;
                if (this.ch_all_branch.Checked == true)
                {
                    cmd2.CommandText = "SELECT *" +
                                       " FROM k017db_pr_all" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //    " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY ID ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT *" +
                                       " FROM k017db_pr_all" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY ID ASC";

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
                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2


                            this.GridView1.Rows[index].Cells["Col_txtstatus_remark"].Value = dt2.Rows[j]["txtstatus_remark"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //4
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtpo_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //9
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //13
                            this.GridView1.Rows[index].Cells["Col_txtapprove_name"].Value = dt2.Rows[j]["txtapprove_name"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //16
                            this.GridView1.Rows[index].Cells["Col_txtRG_name"].Value = dt2.Rows[j]["txtRG_name"].ToString();      //17

                            this.GridView1.Rows[index].Cells["Col_txtReceive_id"].Value = dt2.Rows[j]["txtReceive_id"].ToString();      //18
                            this.GridView1.Rows[index].Cells["Col_txtReceive_date"].Value = dt2.Rows[j]["txtReceive_date"].ToString();      //19
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //20
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //21

                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //22


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //23
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //23
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //24
                            }


                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }

                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //26
                            }

                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //25
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //24
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิก รับเข้าคลัง"; //24
                            }
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //23
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_receive"]).ToString("###,###.00");      //24
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_balance"]).ToString("###,###.00");      //25



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
            GridView1_Color_Column();
            GridView1_Color();
        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 31;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtco_id";
            this.GridView1.Columns[2].Name = "Col_txtbranch_id";

            this.GridView1.Columns[3].Name = "Col_txtstatus_remark";
            this.GridView1.Columns[4].Name = "Col_txtPr_id";
            this.GridView1.Columns[5].Name = "Col_txtdepartment_id";
            this.GridView1.Columns[6].Name = "Col_txtdepartment_name";
            this.GridView1.Columns[7].Name = "Col_txtemp_office_name";

            this.GridView1.Columns[8].Name = "Col_txtPo_id";
            this.GridView1.Columns[9].Name = "Col_txtpo_date";
            this.GridView1.Columns[10].Name = "Col_txtsupplier_id";
            this.GridView1.Columns[11].Name = "Col_txtsupplier_name";



            this.GridView1.Columns[12].Name = "Col_txtapprove_id";
            this.GridView1.Columns[13].Name = "Col_txtapprove_date";
            this.GridView1.Columns[14].Name = "Col_txtapprove_name";


            this.GridView1.Columns[15].Name = "Col_txtRG_id";
            this.GridView1.Columns[16].Name = "Col_txtRG_date";
            this.GridView1.Columns[17].Name = "Col_txtRG_name";

            this.GridView1.Columns[18].Name = "Col_txtReceive_id";
            this.GridView1.Columns[19].Name = "Col_txtReceive_date";
            this.GridView1.Columns[20].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[21].Name = "Col_txtwherehouse_name";
            this.GridView1.Columns[22].Name = "Col_txtmoney_after_vat";
            this.GridView1.Columns[23].Name = "Col_txtpr_status";
            this.GridView1.Columns[24].Name = "Col_txtpo_status";
            this.GridView1.Columns[25].Name = "Col_txtapprove_status";
            this.GridView1.Columns[26].Name = "Col_txtRG_status";
            this.GridView1.Columns[27].Name = "Col_txtreceive_status";
            this.GridView1.Columns[28].Name = "Col_txtsum_qty";
            this.GridView1.Columns[29].Name = "Col_txtsum_qty_receive";
            this.GridView1.Columns[30].Name = "Col_txtsum_qty_balance";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "txtco_id";
            this.GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.GridView1.Columns[3].HeaderText = " สถานะเอกสาร";
            this.GridView1.Columns[4].HeaderText = " เลขที่ PR";
            this.GridView1.Columns[5].HeaderText = " รหัสฝ่าย PR";
            this.GridView1.Columns[6].HeaderText = "ฝ่ายที่ PR";
            this.GridView1.Columns[7].HeaderText = " ผู้บันทึก PR ";

            this.GridView1.Columns[8].HeaderText = " เลขที่ PO";
            this.GridView1.Columns[9].HeaderText = " วันที่ PO";
            this.GridView1.Columns[10].HeaderText = " รหัส Supplier";
            this.GridView1.Columns[11].HeaderText = " ชื่อ Supplier";

            this.GridView1.Columns[12].HeaderText = " เลขที่ AP";
            this.GridView1.Columns[13].HeaderText = " วันที่อนุมัติ";
            this.GridView1.Columns[14].HeaderText = " ผู้อนุมัติ";

            this.GridView1.Columns[15].HeaderText = " เลขที่ RG";
            this.GridView1.Columns[16].HeaderText = " วันที่ RG";
            this.GridView1.Columns[17].HeaderText = " ผู้รับ RG";

            this.GridView1.Columns[18].HeaderText = "เลขที่ใบรับเข้าคลัง";
            this.GridView1.Columns[19].HeaderText = " วันที่รับเข้าคลัง";
            this.GridView1.Columns[20].HeaderText = " รหัสคลัง";
            this.GridView1.Columns[21].HeaderText = " ชื่อ WH";
            this.GridView1.Columns[22].HeaderText = " จำนวนเงิน";
            this.GridView1.Columns[23].HeaderText = " สถานะ PR";
            this.GridView1.Columns[24].HeaderText = " สถานะ PO";
            this.GridView1.Columns[25].HeaderText = " สถานะ AP";
            this.GridView1.Columns[26].HeaderText = "สถานะ RG";
            this.GridView1.Columns[27].HeaderText = " สถานะ WH";
            this.GridView1.Columns[28].HeaderText = "Qty สั่งซื้อ";
            this.GridView1.Columns[29].HeaderText = "Qty รับแล้ว";
            this.GridView1.Columns[30].HeaderText = "Qty ค้างรับ";

            this.GridView1.Columns[0].Visible = false;  //"Col_Auto_num";
            this.GridView1.Columns[1].Visible = false;  //"Col_txtco_id";
            this.GridView1.Columns[2].Visible = false;  //"Col_txtbranch_id";

            this.GridView1.Columns[3].Visible = false;  //"สถานะเอกสาร";
            this.GridView1.Columns[3].Width = 0;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[4].Visible = true;  //"เลขที่ PR"";
            this.GridView1.Columns[4].Width = 140;
            this.GridView1.Columns[4].ReadOnly = true;
            this.GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[5].Visible = false;  //"Col_txtsupplier_id";

            this.GridView1.Columns[6].Visible = true;  //"ฝ่ายที่ PR"";
            this.GridView1.Columns[6].Width = 100;
            this.GridView1.Columns[6].ReadOnly = true;
            this.GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[7].Visible = true;  //"ผู้บันทึก PR";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = true;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[8].Visible = true;  //"เลขที่ PO";
            this.GridView1.Columns[8].Width = 120;
            this.GridView1.Columns[8].ReadOnly = true;
            this.GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[9].Visible = false;  //"วันที่ PO";
            this.GridView1.Columns[9].Width = 0;
            this.GridView1.Columns[9].ReadOnly = true;
            this.GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[10].Visible = false;  //"รหัส Supplier";
            this.GridView1.Columns[10].Width = 0;
            this.GridView1.Columns[10].ReadOnly = true;
            this.GridView1.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[11].Visible = true;  //"ชื่อ Supplier";
            this.GridView1.Columns[11].Width = 200;
            this.GridView1.Columns[11].ReadOnly = true;
            this.GridView1.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[12].Visible = true;  //"เลขที่อนุมัติ";
            this.GridView1.Columns[12].Width = 120;
            this.GridView1.Columns[12].ReadOnly = true;
            this.GridView1.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[13].Visible = false;  //"วันที่อนุมัติ";
            this.GridView1.Columns[13].Width = 0;
            this.GridView1.Columns[13].ReadOnly = true;
            this.GridView1.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[14].Visible = false;  //"ผู้อนุมัติ";
            this.GridView1.Columns[14].Width = 0;
            this.GridView1.Columns[14].ReadOnly = true;
            this.GridView1.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[15].Visible = true;  //"เลขที่ RV";
            this.GridView1.Columns[15].Width = 120;
            this.GridView1.Columns[15].ReadOnly = true;
            this.GridView1.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[16].Visible = false;  //"วันที่ RV";
            this.GridView1.Columns[16].Width = 0;
            this.GridView1.Columns[16].ReadOnly = true;
            this.GridView1.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[17].Visible = true;  //"ผู้บันทึก RV";
            this.GridView1.Columns[17].Width = 100;
            this.GridView1.Columns[17].ReadOnly = true;
            this.GridView1.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[18].Visible = false;  //"เลขที่ใบรับเข้าคลัง";
            this.GridView1.Columns[18].Width = 0;
            this.GridView1.Columns[18].ReadOnly = true;
            this.GridView1.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[19].Visible = false;  //"วันที่รับเข้าคลัง";
            this.GridView1.Columns[19].Width = 0;
            this.GridView1.Columns[19].ReadOnly = true;
            this.GridView1.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[20].Visible = false;  //"รหัสคลัง";
            this.GridView1.Columns[20].Width = 0;
            this.GridView1.Columns[20].ReadOnly = true;
            this.GridView1.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[21].Visible = false;  //"รับเข้าคลัง";
            this.GridView1.Columns[21].Width = 0;
            this.GridView1.Columns[21].ReadOnly = true;
            this.GridView1.Columns[21].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[21].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[22].Visible = true;  //"จำนวนเงิน";
            this.GridView1.Columns[22].Width = 100;
            this.GridView1.Columns[22].ReadOnly = true;
            this.GridView1.Columns[22].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[22].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[23].Visible = false;  //"สถานะ PR";
            this.GridView1.Columns[23].Width = 0;
            this.GridView1.Columns[23].ReadOnly = true;
            this.GridView1.Columns[23].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[23].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[24].Visible = true;  //"สถานะ PO";
            this.GridView1.Columns[24].Width = 100;
            this.GridView1.Columns[24].ReadOnly = true;
            this.GridView1.Columns[24].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[24].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[25].Visible = true;  //"สถานะ ผลอนุมัติ";
            this.GridView1.Columns[25].Width = 100;
            this.GridView1.Columns[25].ReadOnly = true;
            this.GridView1.Columns[25].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[25].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns[26].Visible = true;  //"สถานะ รับจัดซื้อ";
            this.GridView1.Columns[26].Width = 100;
            this.GridView1.Columns[26].ReadOnly = true;
            this.GridView1.Columns[26].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[26].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[27].Visible = false;  //"สถานะ รับเข้าคลัง";
            this.GridView1.Columns[27].Width = 0;
            this.GridView1.Columns[27].ReadOnly = true;
            this.GridView1.Columns[27].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[27].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[28].Visible = true;  //"Col_txtreceive_status";
            this.GridView1.Columns[28].Width = 100;
            this.GridView1.Columns[28].ReadOnly = true;
            this.GridView1.Columns[28].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[28].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView1.Columns[29].Visible = true;  //"Col_txtreceive_status";
            this.GridView1.Columns[29].Width = 100;
            this.GridView1.Columns[29].ReadOnly = true;
            this.GridView1.Columns[29].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[29].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[30].Visible = true;  //"Col_txtreceive_status";
            this.GridView1.Columns[30].Width = 100;
            this.GridView1.Columns[30].ReadOnly = true;
            this.GridView1.Columns[30].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[30].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;





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

                    if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_balance"].Value.ToString())) == 0)
                    {
                    GridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
                else
                    {
                        if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_receive"].Value.ToString())) == 0)
                        {
                            GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                        }
                        if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_receive"].Value.ToString())) > 0)
                        {
                            GridView1.Rows[i].DefaultCellStyle.BackColor = Color.OrangeRed;
                            GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                        }
                    }
 

            }
        }
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {

                GridView1.Rows[i].Cells["Col_txtpo_id"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[i].Cells["Col_txtpo_id"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                //GridView1.Rows[i].Cells["Col_txtdepartment_name"].Style.BackColor = Color.FromArgb(255, 255, 110);

                //GridView1.Rows[i].Cells["Col_txtsupplier_name"].Style.BackColor = Color.FromArgb(0, 172, 237);
                //GridView1.Rows[i].Cells["Col_txtsupplier_name"].Style.ForeColor = Color.FromArgb(255, 255, 255);

                //GridView1.Rows[i].Cells["Col_txtRG_status"].Style.BackColor = Color.FromArgb(255, 61, 0);
                //GridView1.Rows[i].Cells["Col_txtRG_status"].Style.ForeColor = Color.FromArgb(255, 255, 255);

            }
        }
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.cboSearch.Text = "เลขที่ PO";
                    W_ID_Select.TRANS_ID = row.Cells[8].Value.ToString();

                    if (this.cboSearch.Text == "เลขที่ PO")
                    {
                        this.txtsearch.Text = row.Cells[8].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells[8].Value.ToString();

                    }
                    else if (this.cboSearch.Text == "ชื่อ Supplier")
                    {
                        this.txtsearch.Text = row.Cells[11].Value.ToString();

                    }
                    else
                    {
                        this.txtsearch.Text = row.Cells[8].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells[8].Value.ToString();

                    }
                }
                //=====================
                Clear_GridView2();
                Clear_GridView3();
                Fill_Show_DATA_GridView2();
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
                W_ID_Select.WORD_TOP = "ดูข้อมูลใบสั่งซื้อ (PO)";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_detail frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_detail();
                frm2.Show();

                TRANS_LOG();

            }
        }

        private void Fill_Show_DATA_GridView2()
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

            Clear_GridView2();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT *" +
                                   " FROM k017db_pr_all_detail" +

                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtpo_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
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
                            //this.GridView2.Columns[0].Name = "Col_Auto_num";
                            //this.GridView2.Columns[1].Name = "Col_txtpr_id";
                            //this.GridView2.Columns[2].Name = "Col_txtpo_id";
                            //this.GridView2.Columns[3].Name = "Col_txtapprove_id";
                            //this.GridView2.Columns[4].Name = "Col_txtRG_id";
                            //this.GridView2.Columns[5].Name = "Col_txtreceive_id";
                            //this.GridView2.Columns[6].Name = "Col_txtbill_remark";
                            //this.GridView2.Columns[7].Name = "Col_txtwant_receive_date";

                            //this.GridView2.Columns[8].Name = "Col_txtmat_no";
                            //this.GridView2.Columns[9].Name = "Col_txtmat_id";
                            //this.GridView2.Columns[10].Name = "Col_txtmat_name";
                            //this.GridView2.Columns[11].Name = "Col_txtmat_unit1_name";
                            //this.GridView2.Columns[12].Name = "Col_txtprice";
                            //this.GridView2.Columns[13].Name = "Col_txtdiscount_money";
                            //this.GridView2.Columns[14].Name = "Col_txtsum_total";
                            //this.GridView2.Columns[15].Name = "Col_txtitem_no";

                            //this.GridView2.Columns[16].Name = "Col_txtqty_pr";
                            //this.GridView2.Columns[17].Name = "Col_txtqty_po";
                            //this.GridView2.Columns[18].Name = "Col_txtqty_approve";
                            //this.GridView2.Columns[19].Name = "Col_txtqty_rg";
                            //this.GridView2.Columns[20].Name = "Col_txtqty_balance";
                            //this.GridView2.Columns[21].Name = "Col_txtqty_receive";

                            var index = GridView2.Rows.Add();
                            GridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView2.Rows[index].Cells["Col_txtpr_id"].Value = dt2.Rows[j]["txtpr_id"].ToString();      //1
                            GridView2.Rows[index].Cells["Col_txtpo_id"].Value = dt2.Rows[j]["txtpo_id"].ToString();      //2
                            GridView2.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //3
                            GridView2.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //4
                            GridView2.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //5
                            GridView2.Rows[index].Cells["Col_txtbill_remark"].Value = dt2.Rows[j]["txtbill_remark"].ToString();      //6
                            //GridView2.Rows[index].Cells["Col_txtwant_receive_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //7
                            GridView2.Rows[index].Cells["Col_txtwant_receive_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //7
                            GridView2.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //8
                            GridView2.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //9
                            GridView2.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //10
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //11
                            GridView2.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //12
                            GridView2.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //14
                            GridView2.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //15

                            GridView2.Rows[index].Cells["Col_txtqty_pr"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pr"]).ToString("###,###.00");      //16
                            GridView2.Rows[index].Cells["Col_txtqty_po"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_po"]).ToString("###,###.00");      //17
                            GridView2.Rows[index].Cells["Col_txtqty_approve"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_approve"]).ToString("###,###.00");      //18
                            GridView2.Rows[index].Cells["Col_txtqty_rg"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rg"]).ToString("###,###.00");      //19
                            GridView2.Rows[index].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_balance"]).ToString("###,###.00");      //20
                            GridView2.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //21


                        }
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
            GridView2_Color();
            GridView2_Color_Column();
        }
        private void Show_GridView2()
        {
            this.GridView2.ColumnCount = 22;
            this.GridView2.Columns[0].Name = "Col_Auto_num";
            this.GridView2.Columns[1].Name = "Col_txtpr_id";
            this.GridView2.Columns[2].Name = "Col_txtpo_id";
            this.GridView2.Columns[3].Name = "Col_txtapprove_id";
            this.GridView2.Columns[4].Name = "Col_txtRG_id";
            this.GridView2.Columns[5].Name = "Col_txtreceive_id";
            this.GridView2.Columns[6].Name = "Col_txtbill_remark";
            this.GridView2.Columns[7].Name = "Col_txtwant_receive_date";

            this.GridView2.Columns[8].Name = "Col_txtmat_no";
            this.GridView2.Columns[9].Name = "Col_txtmat_id";
            this.GridView2.Columns[10].Name = "Col_txtmat_name";
            this.GridView2.Columns[11].Name = "Col_txtmat_unit1_name";
            this.GridView2.Columns[12].Name = "Col_txtprice";
            this.GridView2.Columns[13].Name = "Col_txtdiscount_money";
            this.GridView2.Columns[14].Name = "Col_txtsum_total";
            this.GridView2.Columns[15].Name = "Col_txtitem_no";

            this.GridView2.Columns[16].Name = "Col_txtqty_pr";
            this.GridView2.Columns[17].Name = "Col_txtqty_po";
            this.GridView2.Columns[18].Name = "Col_txtqty_approve";
            this.GridView2.Columns[19].Name = "Col_txtqty_rg";
            this.GridView2.Columns[20].Name = "Col_txtqty_balance";
            this.GridView2.Columns[21].Name = "Col_txtqty_receive";

            this.GridView2.Columns[0].HeaderText = "No";
            this.GridView2.Columns[1].HeaderText = "PR";
            this.GridView2.Columns[2].HeaderText = "PO";
            this.GridView2.Columns[3].HeaderText = "AP";
            this.GridView2.Columns[4].HeaderText = "RG";
            this.GridView2.Columns[5].HeaderText = "RECEIVE";
            this.GridView2.Columns[6].HeaderText = "หมายเหตุ";
            this.GridView2.Columns[7].HeaderText = "วันที่สินค้าเข้า";

            this.GridView2.Columns[8].HeaderText = "ลำดับ";
            this.GridView2.Columns[9].HeaderText = " รหัส";
            this.GridView2.Columns[10].HeaderText = " ชื่อสินค้า";
            this.GridView2.Columns[11].HeaderText = " หน่วยนับ";
            this.GridView2.Columns[12].HeaderText = " ราคา(บาท)";
            this.GridView2.Columns[13].HeaderText = " ส่วนลด(บาท)";
            this.GridView2.Columns[14].HeaderText = " จำนวนเงิน(บาท)";
            this.GridView2.Columns[15].HeaderText = " ลำดับ";


            this.GridView2.Columns[16].HeaderText = " Qty PR";
            this.GridView2.Columns[17].HeaderText = " Qty PO";
            this.GridView2.Columns[18].HeaderText = "รวมอนุมัติ";
            this.GridView2.Columns[19].HeaderText = "รวมรับแล้ว";
            this.GridView2.Columns[20].HeaderText = "รวมค้างรับ";
            this.GridView2.Columns[21].HeaderText = " Qty รับเข้าคลัง";

            this.GridView2.Columns[0].Visible = false;  //"Col_Auto_num";

            this.GridView2.Columns[1].Visible = false;  //"PR";
            this.GridView2.Columns[1].Width = 0;
            this.GridView2.Columns[1].ReadOnly = true;
            this.GridView2.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[1].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView2.Columns[2].Visible = true;  //"PO";
            this.GridView2.Columns[2].Width = 260;
            this.GridView2.Columns[2].ReadOnly = true;
            this.GridView2.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[2].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[3].Visible = true;  //"Apprive";
            this.GridView2.Columns[3].Width = 120;
            this.GridView2.Columns[3].ReadOnly = true;
            this.GridView2.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[3].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[4].Visible = true;  //"RG";
            this.GridView2.Columns[4].Width = 120;
            this.GridView2.Columns[4].ReadOnly = true;
            this.GridView2.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[4].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[5].Visible = false;  //"Recieve";
            this.GridView2.Columns[5].Width = 0;
            this.GridView2.Columns[5].ReadOnly = false;
            this.GridView2.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[5].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[6].Visible = true;  //"หมายเหตุ";
            this.GridView2.Columns[6].Width = 100;
            this.GridView2.Columns[6].ReadOnly = false;
            this.GridView2.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[6].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[7].Visible = true;  //"วันที่สินค้าเข้า";
            this.GridView2.Columns[7].Width =90;
            this.GridView2.Columns[7].ReadOnly = false;
            this.GridView2.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[7].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[8].Visible = false;  //ลำดับ";
            this.GridView2.Columns[8].Width = 0;
            this.GridView2.Columns[8].ReadOnly = true;
            this.GridView2.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[8].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[9].Visible = true;  //"รหัสสินค้า";
            this.GridView2.Columns[9].Width = 80;
            this.GridView2.Columns[9].ReadOnly = false;
            this.GridView2.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[9].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[10].Visible = true;  //"ชื่อสินค้า";
            this.GridView2.Columns[10].Width = 200;
            this.GridView2.Columns[10].ReadOnly = false;
            this.GridView2.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[10].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[11].Visible = true;  //"หน่วยนับ";
            this.GridView2.Columns[11].Width = 80;
            this.GridView2.Columns[11].ReadOnly = false;
            this.GridView2.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[11].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[12].Visible = true;  //"ราคา/หน่วย";
            this.GridView2.Columns[12].Width = 90;
            this.GridView2.Columns[12].ReadOnly = false;
            this.GridView2.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[12].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[13].Visible = true;  //"ส่วนลด(บาท)";
            this.GridView2.Columns[13].Width = 90;
            this.GridView2.Columns[13].ReadOnly = false;
            this.GridView2.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[13].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[14].Visible = true;  //"จำนวนเงิน(บาท)";
            this.GridView2.Columns[14].Width = 110;
            this.GridView2.Columns[14].ReadOnly = false;
            this.GridView2.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[14].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[15].Visible = false;  //"ลำดับ";
            this.GridView2.Columns[15].Width = 0;
            this.GridView2.Columns[15].ReadOnly = false;
            this.GridView2.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[15].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns[16].Visible = false;  //"จำนวน PR";
            this.GridView2.Columns[16].Width = 0;
            this.GridView2.Columns[16].ReadOnly = false;
            this.GridView2.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[16].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[17].Visible = false;  //"จำนวน PO";
            this.GridView2.Columns[17].Width = 0;
            this.GridView2.Columns[17].ReadOnly = false;
            this.GridView2.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[17].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[18].Visible = true;  //"จำนวน Approve";
            this.GridView2.Columns[18].Width = 100;
            this.GridView2.Columns[18].ReadOnly = false;
            this.GridView2.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[18].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[19].Visible = true;  //"จำนวน RV";
            this.GridView2.Columns[19].Width = 100;
            this.GridView2.Columns[19].ReadOnly = false;
            this.GridView2.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[19].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[20].Visible = true;  //"จำนวน ค้างรับ";
            this.GridView2.Columns[20].Width = 100;
            this.GridView2.Columns[20].ReadOnly = false;
            this.GridView2.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[20].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns[21].Visible = false;  //"จำนวน รับเข้าคลัง";
            this.GridView2.Columns[21].Width = 0;
            this.GridView2.Columns[21].ReadOnly = false;
            this.GridView2.Columns[21].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns[21].HeaderCell.Style.BackColor = Color.FromArgb(255,  255,255);
            this.GridView2.Columns[21].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView2.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView2.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView2.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView2()
        {
            this.GridView2.Rows.Clear();
            this.GridView2.Refresh();
        }
        private void GridView2_Color()
        {
            for (int i = 0; i < this.GridView2.Rows.Count - 0; i++)
            {
                if (Convert.ToDouble(string.Format("{0:n0}", this.GridView2.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) == 0)
                {
                    GridView2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    GridView2.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    GridView2.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
                else
                {
                        GridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        GridView2.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        GridView2.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView2_Color_Column()
        {

            for (int i = 0; i < this.GridView2.Rows.Count - 0; i++)
            {

                GridView2.Rows[i].Cells["Col_txtpo_id"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView2.Rows[i].Cells["Col_txtpo_id"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView2.Rows[i].Cells["Col_txtwant_receive_date"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(0, 195, 0);
                GridView2.Rows[i].Cells["Col_txtwant_receive_date"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView2.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(62, 123, 241);
                GridView2.Rows[i].Cells["Col_txtmat_name"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                //GridView2.Rows[i].Cells["Col_txtqty_balance"].Style.BackColor = Color.FromArgb(255, 61, 0);
                //GridView2.Rows[i].Cells["Col_txtqty_balance"].Style.ForeColor = Color.FromArgb(255, 255, 255);

            }
        }
        private void GridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView2.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    W_ID_Select.TRANS_ID = row.Cells[2].Value.ToString();
                    W_ID_Select.MAT_ID = row.Cells[9].Value.ToString();

                }
                //=====================
                Fill_Show_DATA_GridView3();
            }
        }
        private void GridView2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView2.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }

        private void Fill_Show_DATA_GridView3()
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

            Clear_GridView3();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT *" +
                                   " FROM k017db_pr_all_detail_balance" +

                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtpo_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                    " AND (txtmat_id = '" + W_ID_Select.MAT_ID.Trim() + "')" +
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
                            //this.GridView3.ColumnCount = 22;
                            //this.GridView3.Columns[0].Name = "Col_Auto_num";

                            //this.GridView3.Columns[1].Name = "Col_txttrans_date_server";
                            //this.GridView3.Columns[2].Name = "Col_txttrans_time";

                            //this.GridView3.Columns[3].Name = "Col_txtpr_id";
                            //this.GridView3.Columns[4].Name = "Col_txtpo_id";
                            //this.GridView3.Columns[5].Name = "Col_txtapprove_id";
                            //this.GridView3.Columns[6].Name = "Col_txtRG_id";
                            //this.GridView3.Columns[7].Name = "Col_txtreceive_id";
                            //this.GridView3.Columns[8].Name = "Col_txtbill_remark";
                            //this.GridView3.Columns[9].Name = "Col_txtwant_receive_date";

                            //this.GridView3.Columns[10].Name = "Col_txtmat_no";
                            //this.GridView3.Columns[11].Name = "Col_txtmat_id";
                            //this.GridView3.Columns[12].Name = "Col_txtmat_name";
                            //this.GridView3.Columns[13].Name = "Col_txtmat_unit1_name";
                            //this.GridView3.Columns[14].Name = "Col_txtprice";
                            //this.GridView3.Columns[15].Name = "Col_txtdiscount_money";
                            //this.GridView3.Columns[16].Name = "Col_txtsum_total";
                            //this.GridView3.Columns[17].Name = "Col_txtitem_no";

                            //this.GridView3.Columns[18].Name = "Col_txtqty_pr";
                            //this.GridView3.Columns[19].Name = "Col_txtqty_po";
                            //this.GridView3.Columns[20].Name = "Col_txtqty_approve";
                            //this.GridView3.Columns[21].Name = "Col_txtqty_rg";
                            //this.GridView3.Columns[22].Name = "Col_txtqty_balance";
                            //this.GridView3.Columns[23].Name = "Col_txtqty_receive";

                            var index = GridView3.Rows.Add();
                            GridView3.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView3.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);      //1
                            GridView3.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //2

                            GridView3.Rows[index].Cells["Col_txtpr_id"].Value = dt2.Rows[j]["txtpr_id"].ToString();      //1
                            GridView3.Rows[index].Cells["Col_txtpo_id"].Value = dt2.Rows[j]["txtpo_id"].ToString();      //2
                            GridView3.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //3
                            GridView3.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //4
                            GridView3.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //5
                            GridView3.Rows[index].Cells["Col_txtbill_remark"].Value = dt2.Rows[j]["txtbill_remark"].ToString();      //6
                            //GridView3.Rows[index].Cells["Col_txtwant_receive_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //7
                            GridView3.Rows[index].Cells["Col_txtwant_receive_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //7
                            GridView3.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //8
                            GridView3.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //9
                            GridView3.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //10
                            GridView3.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //11
                            GridView3.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //12
                            GridView3.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //13
                            GridView3.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //14
                            GridView3.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //15

                            GridView3.Rows[index].Cells["Col_txtqty_pr"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pr"]).ToString("###,###.00");      //16
                            GridView3.Rows[index].Cells["Col_txtqty_po"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_po"]).ToString("###,###.00");      //17
                            GridView3.Rows[index].Cells["Col_txtqty_approve"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_approve"]).ToString("###,###.00");      //18
                            //CANCEL
                            if (dt2.Rows[j]["txtreceive_id"].ToString() == "CANCEL")
                            {
                                GridView3.Rows[index].Cells["Col_txtqty_rg"].Value = "-" + Convert.ToSingle(dt2.Rows[j]["txtqty_rg"]).ToString("###,###.00");      //19
                            }
                            else
                            {
                                GridView3.Rows[index].Cells["Col_txtqty_rg"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rg"]).ToString("###,###.00");      //19
                            }
                            GridView3.Rows[index].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_balance"]).ToString("###,###.00");      //20
                            GridView3.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //21


                        }
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
            GridView3_Color_Column();

        }
        private void Show_GridView3()
        {
            this.GridView3.ColumnCount = 24;
            this.GridView3.Columns[0].Name = "Col_Auto_num";

            this.GridView3.Columns[1].Name = "Col_txttrans_date_server";
            this.GridView3.Columns[2].Name = "Col_txttrans_time";

            this.GridView3.Columns[3].Name = "Col_txtpr_id";
            this.GridView3.Columns[4].Name = "Col_txtpo_id";
            this.GridView3.Columns[5].Name = "Col_txtapprove_id";
            this.GridView3.Columns[6].Name = "Col_txtRG_id";
            this.GridView3.Columns[7].Name = "Col_txtreceive_id";
            this.GridView3.Columns[8].Name = "Col_txtbill_remark";
            this.GridView3.Columns[9].Name = "Col_txtwant_receive_date";

            this.GridView3.Columns[10].Name = "Col_txtmat_no";
            this.GridView3.Columns[11].Name = "Col_txtmat_id";
            this.GridView3.Columns[12].Name = "Col_txtmat_name";
            this.GridView3.Columns[13].Name = "Col_txtmat_unit1_name";
            this.GridView3.Columns[14].Name = "Col_txtprice";
            this.GridView3.Columns[15].Name = "Col_txtdiscount_money";
            this.GridView3.Columns[16].Name = "Col_txtsum_total";
            this.GridView3.Columns[17].Name = "Col_txtitem_no";

            this.GridView3.Columns[18].Name = "Col_txtqty_pr";
            this.GridView3.Columns[19].Name = "Col_txtqty_po";
            this.GridView3.Columns[20].Name = "Col_txtqty_approve";
            this.GridView3.Columns[21].Name = "Col_txtqty_rg";
            this.GridView3.Columns[22].Name = "Col_txtqty_balance";
            this.GridView3.Columns[23].Name = "Col_txtqty_receive";

            this.GridView3.Columns[0].HeaderText = "No";
            this.GridView3.Columns[1].HeaderText = "วันที่";
            this.GridView3.Columns[2].HeaderText = "เวลา";

            this.GridView3.Columns[3].HeaderText = "PR";
            this.GridView3.Columns[4].HeaderText = "PO";
            this.GridView3.Columns[5].HeaderText = "AP";
            this.GridView3.Columns[6].HeaderText = "RG";
            this.GridView3.Columns[7].HeaderText = "RECEIVE";
            this.GridView3.Columns[8].HeaderText = "หมายเหตุ";
            this.GridView3.Columns[9].HeaderText = "วันสินค้าเข้า";

            this.GridView3.Columns[10].HeaderText = "ลำดับ";
            this.GridView3.Columns[11].HeaderText = " รหัส";
            this.GridView3.Columns[12].HeaderText = " ชื่อสินค้า";
            this.GridView3.Columns[13].HeaderText = " หน่วย";
            this.GridView3.Columns[14].HeaderText = " ราคา";
            this.GridView3.Columns[15].HeaderText = " ส่วนลด";
            this.GridView3.Columns[16].HeaderText = " จำนวนเงิน";
            this.GridView3.Columns[17].HeaderText = " ลำดับ";


            this.GridView3.Columns[18].HeaderText = " Qty PR";
            this.GridView3.Columns[19].HeaderText = " Qty PO";
            this.GridView3.Columns[20].HeaderText = "อนุมัติ";
            this.GridView3.Columns[21].HeaderText = "รับแล้ว";
            this.GridView3.Columns[22].HeaderText = "ค้างรับ";
            this.GridView3.Columns[23].HeaderText = " Qtyรับเข้าคลัง";

            this.GridView3.Columns[0].Visible = false;  //"Col_Auto_num";

            this.GridView3.Columns[1].Visible = true;  //"วันที่";
            this.GridView3.Columns[1].Width = 80;
            this.GridView3.Columns[1].ReadOnly = true;
            this.GridView3.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[1].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView3.Columns[2].Visible = true;  //"เวลา";
            this.GridView3.Columns[2].Width = 60;
            this.GridView3.Columns[2].ReadOnly = true;
            this.GridView3.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[2].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[3].Visible = false;  //"PR";
            this.GridView3.Columns[3].Width = 0;
            this.GridView3.Columns[3].ReadOnly = true;
            this.GridView3.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[3].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView3.Columns[4].Visible = true;  //"PO";
            this.GridView3.Columns[4].Width = 120;
            this.GridView3.Columns[4].ReadOnly = true;
            this.GridView3.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[4].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[5].Visible = true;  //"Apprive";
            this.GridView3.Columns[5].Width = 120;
            this.GridView3.Columns[5].ReadOnly = true;
            this.GridView3.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[5].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[6].Visible = true;  //"RG";
            this.GridView3.Columns[6].Width = 120;
            this.GridView3.Columns[6].ReadOnly = true;
            this.GridView3.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[6].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[7].Visible = false;  //"Recieve";
            this.GridView3.Columns[7].Width = 0;
            this.GridView3.Columns[7].ReadOnly = false;
            this.GridView3.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[7].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[8].Visible = true;  //"หมายเหตุ";
            this.GridView3.Columns[8].Width = 100;
            this.GridView3.Columns[8].ReadOnly = false;
            this.GridView3.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[8].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[9].Visible = true;  //"วันที่สินค้าเข้า";
            this.GridView3.Columns[9].Width = 90;
            this.GridView3.Columns[9].ReadOnly = false;
            this.GridView3.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[9].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[10].Visible = false;  //ลำดับ";
            this.GridView3.Columns[10].Width = 0;
            this.GridView3.Columns[10].ReadOnly = true;
            this.GridView3.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[10].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[11].Visible = true;  //"รหัสสินค้า";
            this.GridView3.Columns[11].Width = 80;
            this.GridView3.Columns[11].ReadOnly = false;
            this.GridView3.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[11].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[12].Visible = true;  //"ชื่อสินค้า";
            this.GridView3.Columns[12].Width = 200;
            this.GridView3.Columns[12].ReadOnly = false;
            this.GridView3.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[12].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[13].Visible = true;  //"หน่วย";
            this.GridView3.Columns[13].Width = 80;
            this.GridView3.Columns[13].ReadOnly = false;
            this.GridView3.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[13].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[14].Visible = true;  //"ราคา/หน่วย";
            this.GridView3.Columns[14].Width = 90;
            this.GridView3.Columns[14].ReadOnly = false;
            this.GridView3.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[14].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[15].Visible = true;  //"ส่วนลด(บาท)";
            this.GridView3.Columns[15].Width = 90;
            this.GridView3.Columns[15].ReadOnly = false;
            this.GridView3.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[15].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[16].Visible = true;  //"จำนวนเงิน(บาท)";
            this.GridView3.Columns[16].Width = 110;
            this.GridView3.Columns[16].ReadOnly = false;
            this.GridView3.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[16].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[17].Visible = false;  //"ลำดับ";
            this.GridView3.Columns[17].Width = 0;
            this.GridView3.Columns[17].ReadOnly = false;
            this.GridView3.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[17].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView3.Columns[18].Visible = false;  //"จำนวน PR";
            this.GridView3.Columns[18].Width = 0;
            this.GridView3.Columns[18].ReadOnly = false;
            this.GridView3.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[18].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[19].Visible = false;  //"จำนวน PO";
            this.GridView3.Columns[19].Width = 0;
            this.GridView3.Columns[19].ReadOnly = false;
            this.GridView3.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[19].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[20].Visible = true;  //"จำนวน Approve";
            this.GridView3.Columns[20].Width = 100;
            this.GridView3.Columns[20].ReadOnly = false;
            this.GridView3.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[20].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[21].Visible = true;  //"จำนวน RV";
            this.GridView3.Columns[21].Width = 100;
            this.GridView3.Columns[21].ReadOnly = false;
            this.GridView3.Columns[21].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[21].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[21].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[22].Visible = true;  //"จำนวน ค้างรับ";
            this.GridView3.Columns[22].Width = 100;
            this.GridView3.Columns[22].ReadOnly = false;
            this.GridView3.Columns[22].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[22].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[22].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.Columns[23].Visible = false;  //"จำนวน รับเข้าคลัง";
            this.GridView3.Columns[23].Width = 0;
            this.GridView3.Columns[23].ReadOnly = false;
            this.GridView3.Columns[23].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView3.Columns[23].HeaderCell.Style.BackColor =Color.FromArgb(255,  255,255);
            this.GridView3.Columns[23].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView3.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView3.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView3.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView3.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView3()
        {
            this.GridView3.Rows.Clear();
            this.GridView3.Refresh();
        }
        private void GridView3_Color_Column()
        {

            for (int i = 0; i < this.GridView3.Rows.Count - 0; i++)
            {

                GridView3.Rows[i].Cells["Col_txtpo_id"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView3.Rows[i].Cells["Col_txtpo_id"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView3.Rows[i].Cells["Col_txtwant_receive_date"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(0, 195, 0);
                GridView3.Rows[i].Cells["Col_txtwant_receive_date"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                GridView3.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightSkyBlue;//Color.FromArgb(62, 123, 241);
                GridView3.Rows[i].Cells["Col_txtmat_name"].Style.ForeColor = Color.FromArgb(0, 0, 0);

                //GridView3.Rows[i].Cells["Col_txtqty_balance"].Style.BackColor = Color.FromArgb(255, 61, 0);
                //GridView3.Rows[i].Cells["Col_txtqty_balance"].Style.ForeColor = Color.FromArgb(255, 255, 255);

            }
        }
        private void GridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void GridView3_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView3.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView3_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView3.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
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

                W_ID_Select.WORD_TOP = "เพิ่มใบสั่งซื้อ (PO)";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record();
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
                W_ID_Select.WORD_TOP = "ดูข้อมูลใบสั่งซื้อ (PO)";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_detail frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_detail();
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

                W_ID_Select.WORD_TOP = "ยกเลิกใบสั่งซื้อ (PO)";
                kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_detail frm2 = new kondate.soft.HOME02_Purchasing.HOME02_Purchasing_02PO_record_detail();
                frm2.Show();
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_contens_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1_contens.ClientRectangle, Color.WhiteSmoke, ButtonBorderStyle.Solid);
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

        private void btnGo1_Click(object sender, EventArgs e)
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


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (this.cboSearch.Text == "เลขที่ PO")
                {
                    cmd2.CommandText = "SELECT *" +
                                       " FROM k017db_pr_all" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //    " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (txtPr_id = '" + this.txtsearch.Text.Trim() + "')" +
                                      " ORDER BY ID ASC";

                }
                if (this.cboSearch.Text == "ชื่อ Supplier")
                {
                    cmd2.CommandText = "SELECT *" +
                                       " FROM k017db_pr_all" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //    " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (txtsupplier_name LIKE '%" + this.txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY ID ASC";

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
                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2


                            this.GridView1.Rows[index].Cells["Col_txtstatus_remark"].Value = dt2.Rows[j]["txtstatus_remark"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //4
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtpo_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //9
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //13
                            this.GridView1.Rows[index].Cells["Col_txtapprove_name"].Value = dt2.Rows[j]["txtapprove_name"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //16
                            this.GridView1.Rows[index].Cells["Col_txtRG_name"].Value = dt2.Rows[j]["txtRG_name"].ToString();      //17

                            this.GridView1.Rows[index].Cells["Col_txtReceive_id"].Value = dt2.Rows[j]["txtReceive_id"].ToString();      //18
                            this.GridView1.Rows[index].Cells["Col_txtReceive_date"].Value = dt2.Rows[j]["txtReceive_date"].ToString();      //19
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //20
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //21

                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //22


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //23
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //23
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //24
                            }


                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }

                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //26
                            }

                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //25
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //24
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิก รับเข้าคลัง"; //24
                            }
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //23
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_receive"]).ToString("###,###.00");      //24
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_balance"]).ToString("###,###.00");      //25



                        }
                        //=======================================================
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
            GridView1_Color_Column();
            GridView1_Color();


        }

        private void btnGo2_Click(object sender, EventArgs e)
        {
            Fill_Show_DATA_GridView1();
        }

        private void btnGo3_Click(object sender, EventArgs e)
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


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM k017db_pr_all" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //    " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                   " AND (txtsupplier_id = '" + this.PANEL161_SUP_txtsupplier_id.Text.Trim() + "')" +
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
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2


                            this.GridView1.Rows[index].Cells["Col_txtstatus_remark"].Value = dt2.Rows[j]["txtstatus_remark"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtPr_id"].Value = dt2.Rows[j]["txtPr_id"].ToString();      //4
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_id"].Value = dt2.Rows[j]["txtdepartment_id"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtdepartment_name"].Value = dt2.Rows[j]["txtdepartment_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtPo_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtpo_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //9
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtapprove_id"].Value = dt2.Rows[j]["txtapprove_id"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtapprove_date"].Value = dt2.Rows[j]["txtapprove_date"].ToString();      //13
                            this.GridView1.Rows[index].Cells["Col_txtapprove_name"].Value = dt2.Rows[j]["txtapprove_name"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtRG_id"].Value = dt2.Rows[j]["txtRG_id"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtRG_date"].Value = dt2.Rows[j]["txtRG_date"].ToString();      //16
                            this.GridView1.Rows[index].Cells["Col_txtRG_name"].Value = dt2.Rows[j]["txtRG_name"].ToString();      //17

                            this.GridView1.Rows[index].Cells["Col_txtReceive_id"].Value = dt2.Rows[j]["txtReceive_id"].ToString();      //18
                            this.GridView1.Rows[index].Cells["Col_txtReceive_date"].Value = dt2.Rows[j]["txtReceive_date"].ToString();      //19
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //20
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //21

                            this.GridView1.Rows[index].Cells["Col_txtmoney_after_vat"].Value = Convert.ToSingle(dt2.Rows[j]["txtmoney_after_vat"]).ToString("###,###.00");      //22


                            //PR==============================
                            if (dt2.Rows[j]["txtpr_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ออก PR"; //23
                            }
                            else if (dt2.Rows[j]["txtpr_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpr_status"].Value = "ยกเลิก PR"; //23
                            }


                            //PO==============================
                            if (dt2.Rows[j]["txtpo_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = ""; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ออก PO"; //24
                            }
                            else if (dt2.Rows[j]["txtpo_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtpo_status"].Value = "ยกเลิก PO"; //24
                            }


                            //Approve ==============================
                            if (dt2.Rows[j]["txtapprove_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = ""; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "Y")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "R")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ชลอไปก่อน"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "N")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ไม่อนุมัติ"; //20
                            }
                            else if (dt2.Rows[j]["txtapprove_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtapprove_status"].Value = "ยกเลิก"; //20
                            }

                            //RG ==============================
                            if (dt2.Rows[j]["txtRG_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = ""; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ออก RG"; //26
                            }
                            else if (dt2.Rows[j]["txtRG_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtRG_status"].Value = "ยกเลิก RG"; //26
                            }

                            //Receive ==============================
                            if (dt2.Rows[j]["txtreceive_status"].ToString() == "")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = ""; //25
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "รับเข้าคลัง"; //24
                            }
                            else if (dt2.Rows[j]["txtreceive_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txtreceive_status"].Value = "ยกเลิก รับเข้าคลัง"; //24
                            }
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //23
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_receive"]).ToString("###,###.00");      //24
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_balance"]).ToString("###,###.00");      //25



                        }
                        //=======================================================
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
            GridView1_Color_Column();
            GridView1_Color();

        }

        private void btnGo4_Click(object sender, EventArgs e)
        {
            Fill_Show_BRANCH_DATA_GridView1();
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
        private void PANEL2_BRANCH_btnnew_Click(object sender, EventArgs e)
        {

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



        //txtsupplier   =======================================================================
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

            PANEL161_SUP_Clear_GridView1_supplier();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM k016db_1supplier" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtsupplier_id <> '')" +
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
                            var index = PANEL161_SUP_dataGridView1.Rows.Add();
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //1
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //2
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name_eng"].Value = dt2.Rows[j]["txtsupplier_name_eng"].ToString();      //3
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
        private void PANEL161_SUP_GridView1_supplier()
        {
            this.PANEL161_SUP_dataGridView1.ColumnCount = 4;
            this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name_eng";

            this.PANEL161_SUP_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderText = "รหัส";
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderText = " ชื่อ Supplier ";
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderText = " ชื่อ Supplier  Eng";

            this.PANEL161_SUP_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL161_SUP_dataGridView1.Columns[1].Visible = true;  //"Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[1].Width = 100;
            this.PANEL161_SUP_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[2].Visible = true;  //"Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[2].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL161_SUP_dataGridView1.Columns[3].Visible = true;  //"Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[3].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL161_SUP_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL161_SUP_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL161_SUP_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL161_SUP_dataGridView1.EnableHeadersVisualStyles = false;

        }
        private void PANEL161_SUP_Clear_GridView1_supplier()
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
        private void PANEL161_SUP_PANEL161_SUP_btnsupplier_Click(object sender, EventArgs e)
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
        private void PANEL161_SUP_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL161_SUP_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL161_SUP_txtsupplier_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL161_SUP_txtsupplier_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL161_SUP_dataGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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

            PANEL161_SUP_Clear_GridView1_supplier();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k016db_1supplier" +
                                    " WHERE (txtsupplier_name LIKE '%" + this.PANEL161_SUP_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL161_SUP_dataGridView1.Rows.Add();
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //1
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //2
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtsupplier_name_eng"].Value = dt2.Rows[j]["txtsupplier_name_eng"].ToString();      //3
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

        //END txtsupplier   =======================================================================


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
