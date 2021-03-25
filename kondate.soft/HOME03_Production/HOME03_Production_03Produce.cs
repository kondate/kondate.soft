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
    public partial class HOME03_Production_03Produce : Form
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



        public HOME03_Production_03Produce()
        {
            InitializeComponent();
        }

        private void HOME03_Production_03Produce_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0303ICRFGR";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblword_status.Text = "ระเบียนFG1 ผ้าดิบ";
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
            this.cboSearch.Items.Add("เลขที่FG1 ผ้าดิบ");
            this.cboSearch.Items.Add("ชื่อสินค้าผลิต");

            //========================================
            PANEL2_BRANCH_GridView1_branch();
            PANEL2_BRANCH_Fill_branch();

            Show_GridView1();
            Fill_Show_DATA_GridView1();

            Show_GridView4();
            Fill_Show_DATA_GridView4();

            Show_GridView5();
            Fill_Show_DATA_GridView5();

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



                cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                   //"c002_02produce_record_machine.*," +
                                   "c001_04produce_type.*," +
                                   //"c001_02machine.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_02produce_record" +

                                   //" INNER JOIN c002_02produce_record_machine" +
                                   //" ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                   //" AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +

                                   " INNER JOIN c001_04produce_type" +
                                   " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                   " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   //" INNER JOIN c001_02machine" +
                                   //" ON c002_02produce_record_machine.cdkey = c001_02machine.cdkey" +
                                   //" AND c002_02produce_record_machine.txtco_id = c001_02machine.txtco_id" +
                                   //" AND c002_02produce_record_machine.txtmachine_id = c001_02machine.txtmachine_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                  " WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (c002_02produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                  " AND (c002_02produce_record.txttrans_date_client BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY c002_02produce_record.txticrf_id ASC";


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
                            //this.GridView1.Columns[3].Name = "Col_txtFG1_id";
                            //this.GridView1.Columns[4].Name = "Col_txttrans_date_client";
                            //this.GridView1.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView1.Columns[6].Name = "Col_txtproduce_type_name";
                            //this.GridView1.Columns[7].Name = "Col_txtwherehouse_name";
                            //this.GridView1.Columns[8].Name = "Col_txtmat_id";
                            //this.GridView1.Columns[9].Name = "Col_txtmat_name";
                            //this.GridView1.Columns[10].Name = "Col_txtnumber_mat_id";
                            //this.GridView1.Columns[11].Name = "Col_txtmachine_id";
                            //this.GridView1.Columns[12].Name = "Col_txtface_baking_name";
                            //this.GridView1.Columns[13].Name = "Col_txtsum_qty";
                            //this.GridView1.Columns[14].Name = "Col_txtsum2_qty";
                            //this.GridView1.Columns[15].Name = "Col_txticrf_status";

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = ""; // dt2.Rows[j]["txtnumber_in_year"].ToString(); //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txticrf_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtic_id"].Value = "";      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmachine_id"].Value = "";     //11
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //12

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_ic"].Value = "0";      //13
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_yes"].Value = "0";       //13
                            this.GridView1.Rows[index].Cells["Col_txtsum2_qty"].Value = "0";       //14

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_change"].Value = "0";      //14
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_change_rate"].Value = "0";      //14

                            //ic==============================
                            if (dt2.Rows[j]["txticrf_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txticrf_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txticrf_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txticrf_status"].Value = "ยกเลิก"; //15
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
            GridView1_Color_Column();
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

                cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                   "c002_02produce_record_machine.*," +
                                   "c001_04produce_type.*," +
                                   "c001_02machine.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_02produce_record" +

                                   " INNER JOIN c002_02produce_record_machine" +
                                   " ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                   " AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +

                                   " INNER JOIN c001_04produce_type" +
                                   " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                   " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   " INNER JOIN c001_02machine" +
                                   " ON c002_02produce_record_machine.cdkey = c001_02machine.cdkey" +
                                   " AND c002_02produce_record_machine.txtco_id = c001_02machine.txtco_id" +
                                   " AND c002_02produce_record_machine.txtmachine_id = c001_02machine.txtmachine_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //" AND (c002_02produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (c002_02produce_record.txttrans_date_client BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY c002_02produce_record.txticrf_id ASC";

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
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = ""; // dt2.Rows[j]["txtnumber_in_year"].ToString(); //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txticrf_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtic_id"].Value = dt2.Rows[j]["txtic_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_name"].ToString();      //11
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //12

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_ic"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_ic"]).ToString("###,###.00");      //13
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_yes"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_yes"]).ToString("###,###.00");      //13
                            this.GridView1.Rows[index].Cells["Col_txtsum2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum2_qty"]).ToString("###,###.00");      //14

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_change"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_change"]).ToString("###,###.00");      //14
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_change_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_change_rate"]).ToString("###,###.00");      //14

                            //ic==============================
                            if (dt2.Rows[j]["txticrf_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txticrf_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txticrf_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txticrf_status"].Value = "ยกเลิก"; //15
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
            //================================
            GridView1_Color_Column();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 20;
            this.GridView1.Columns[0].Name = "Col_txtnumber_in_year";
            this.GridView1.Columns[1].Name = "Col_txtco_id";
            this.GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.GridView1.Columns[3].Name = "Col_txtFG1_id";
            this.GridView1.Columns[4].Name = "Col_txtic_id";
            this.GridView1.Columns[5].Name = "Col_txttrans_date_client";
            this.GridView1.Columns[6].Name = "Col_txttrans_time";
            this.GridView1.Columns[7].Name = "Col_txtproduce_type_name";
            this.GridView1.Columns[8].Name = "Col_txtwherehouse_name";
            this.GridView1.Columns[9].Name = "Col_txtmat_id";
            this.GridView1.Columns[10].Name = "Col_txtmat_name";
            this.GridView1.Columns[11].Name = "Col_txtnumber_mat_id";
            this.GridView1.Columns[12].Name = "Col_txtmachine_id";
            this.GridView1.Columns[13].Name = "Col_txtface_baking_name";
            this.GridView1.Columns[14].Name = "Col_txtsum_qty_ic";
            this.GridView1.Columns[15].Name = "Col_txtsum_qty_yes";
            this.GridView1.Columns[16].Name = "Col_txtsum2_qty";
            this.GridView1.Columns[17].Name = "Col_txtsum_qty_change";
            this.GridView1.Columns[18].Name = "Col_txtsum_qty_change_rate";
            this.GridView1.Columns[19].Name = "Col_txticrf_status";

            this.GridView1.Columns[0].HeaderText = "เลขที่ชุด";
            this.GridView1.Columns[1].HeaderText = "txtco_id";
            this.GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.GridView1.Columns[3].HeaderText = " เลขที่ FG1";
            this.GridView1.Columns[4].HeaderText = " เลขที่เบิกด้าย";
            this.GridView1.Columns[5].HeaderText = " วันที่";
            this.GridView1.Columns[6].HeaderText = " เวลา";
            this.GridView1.Columns[7].HeaderText = "ประเภทผลิต";
            this.GridView1.Columns[8].HeaderText = "คลัง";
            this.GridView1.Columns[9].HeaderText = "รหัสวัตถุดิบ";
            this.GridView1.Columns[10].HeaderText = "ชื่อวัตถุดิบ";
            this.GridView1.Columns[11].HeaderText = "เบอร์ด้าย";
            this.GridView1.Columns[12].HeaderText = "เครื่องจักร";
            this.GridView1.Columns[13].HeaderText = "อบหน้า";

            this.GridView1.Columns[14].HeaderText = "เบิกด้ายเข้า กก";
            this.GridView1.Columns[15].HeaderText = "ผลิตผ้าดิบได้ กก.";
            this.GridView1.Columns[16].HeaderText = "ผลิตผ้าดิบได้ ปอนด์";
            this.GridView1.Columns[17].HeaderText = "ส่วนต่าง กก";
            this.GridView1.Columns[18].HeaderText = "% ส่วนต่าง";
            this.GridView1.Columns[19].HeaderText = " สถานะ";

            this.GridView1.Columns["Col_txtnumber_in_year"].Visible = false;  //"Col_txtnumber_in_year";
            this.GridView1.Columns["Col_txtnumber_in_year"].Width = 0;
            this.GridView1.Columns["Col_txtnumber_in_year"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.GridView1.Columns["Col_txtbranch_id"].Visible = false;  //"Col_txtbranch_id";

            this.GridView1.Columns["Col_txtFG1_id"].Visible = true;  //"Col_txtFG1_id";
            this.GridView1.Columns["Col_txtFG1_id"].Width = 140;
            this.GridView1.Columns["Col_txtFG1_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtFG1_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtFG1_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtic_id"].Visible = true;  //"Col_txtic_id";
            this.GridView1.Columns["Col_txtic_id"].Width = 140;
            this.GridView1.Columns["Col_txtic_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtic_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtic_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txttrans_date_client"].Visible = true;  //"Col_txttrans_date_client";
            this.GridView1.Columns["Col_txttrans_date_client"].Width = 100;
            this.GridView1.Columns["Col_txttrans_date_client"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_date_client"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_date_client"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txttrans_time"].Visible = true;  //"Col_txttrans_time";
            this.GridView1.Columns["Col_txttrans_time"].Width = 80;
            this.GridView1.Columns["Col_txttrans_time"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtproduce_type_name"].Visible = true;  //"Col_txtproduce_type_name";
            this.GridView1.Columns["Col_txtproduce_type_name"].Width = 100;
            this.GridView1.Columns["Col_txtproduce_type_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtproduce_type_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtproduce_type_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtwherehouse_name"].Visible = true;  //"Col_txtwherehouse_name";
            this.GridView1.Columns["Col_txtwherehouse_name"].Width = 120;
            this.GridView1.Columns["Col_txtwherehouse_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 120;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 120;
            this.GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = true;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 120;
            this.GridView1.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView1.Columns["Col_txtmachine_id"].Width = 120;
            this.GridView1.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtface_baking_name"].Visible = true;  //"Col_txtface_baking_name";
            this.GridView1.Columns["Col_txtface_baking_name"].Width = 120;
            this.GridView1.Columns["Col_txtface_baking_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtface_baking_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtface_baking_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_qty_ic"].Visible = true;  //"Col_txtsum_qty_ic";
            this.GridView1.Columns["Col_txtsum_qty_ic"].Width = 120;
            this.GridView1.Columns["Col_txtsum_qty_ic"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_ic"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_ic"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_qty_yes"].Visible = true;  //"Col_txtsum_qty_yes";
            this.GridView1.Columns["Col_txtsum_qty_yes"].Width = 120;
            this.GridView1.Columns["Col_txtsum_qty_yes"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_yes"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_yes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum2_qty"].Visible = false;  //"Col_txtsum2_qty";
            this.GridView1.Columns["Col_txtsum2_qty"].Width = 0;
            this.GridView1.Columns["Col_txtsum2_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_qty_change"].Visible = true;  //"Col_txtsum_qty_change";
            this.GridView1.Columns["Col_txtsum_qty_change"].Width = 120;
            this.GridView1.Columns["Col_txtsum_qty_change"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_change"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_change"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_qty_change_rate"].Visible = true;  //"Col_txtsum_qty_change_rate";
            this.GridView1.Columns["Col_txtsum_qty_change_rate"].Width = 120;
            this.GridView1.Columns["Col_txtsum_qty_change_rate"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_change_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_change_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txticrf_status"].Visible = true;  //"Col_txticrf_status";
            this.GridView1.Columns["Col_txticrf_status"].Width = 100;
            this.GridView1.Columns["Col_txticrf_status"].ReadOnly = true;
            this.GridView1.Columns["Col_txticrf_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txticrf_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


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

                //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmoney_after_vat_creditor"].Value.ToString())) > 0)
                //{
                //    GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                //    GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                //    GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                //}

            }
        }
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                GridView1.Rows[i].Cells["Col_txtmachine_id"].Style.BackColor = Color.LightGreen;
                GridView1.Rows[i].Cells["Col_txtsum_qty_ic"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtsum_qty_yes"].Style.BackColor = Color.LightGreen;
                GridView1.Rows[i].Cells["Col_txtsum_qty_change"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtsum_qty_change_rate"].Style.BackColor = Color.LightGreen;

            }
        }
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtco_id"].Value;
                if (cell != null)
                {
                    W_ID_Select.TRANS_ID = row.Cells["Col_txtFG1_id"].Value.ToString();
                    this.cboSearch.Text = "เลขที่FG1 ผ้าดิบ";

                    if (this.cboSearch.Text == "เลขที่FG1 ผ้าดิบ")
                    {
                        this.txtsearch.Text = row.Cells["Col_txtFG1_id"].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells["Col_txtFG1_id"].Value.ToString();

                    }
                    else if (this.cboSearch.Text == "รหัสสินค้า")
                    {
                        this.txtsearch.Text = row.Cells["Col_txtmat_id"].Value.ToString();

                    }
                    else
                    {
                        this.txtsearch.Text = row.Cells["Col_txtFG1_id"].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells["Col_txtFG1_id"].Value.ToString();

                    }
                }
                //=====================
                W_ID_Select.IDS1 = row.Cells["Col_txtnumber_in_year"].Value.ToString();
                Fill_Show_DATA_GridView5();
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
                W_ID_Select.WORD_TOP = "ดูข้อมูลFG1 ผ้าดิบ";
                kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_detail frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_detail();
                frm2.Show();

                TRANS_LOG();

            }
        }

        private void Fill_Show_DATA_GridView4()
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

            Clear_GridView4();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                   //"c002_02produce_record_machine.*," +
                                   "c001_04produce_type.*," +
                                   //"c001_02machine.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_02produce_record" +

                                   //" INNER JOIN c002_02produce_record_machine" +
                                   //" ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                   //" AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +

                                   " INNER JOIN c001_04produce_type" +
                                   " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                   " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   //" INNER JOIN c001_02machine" +
                                   //" ON c002_02produce_record_machine.cdkey = c001_02machine.cdkey" +
                                   //" AND c002_02produce_record_machine.txtco_id = c001_02machine.txtco_id" +
                                   //" AND c002_02produce_record_machine.txtmachine_id = c001_02machine.txtmachine_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_02produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   " AND (c002_02produce_record.txttrans_date_client BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY c002_02produce_record.txticrf_id ASC";

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
                            //this.GridView4.Columns[0].Name = "Col_Auto_num";
                            //this.GridView4.Columns[1].Name = "Col_txtco_id";
                            //this.GridView4.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView4.Columns[3].Name = "Col_txtFG1_id";
                            //this.GridView4.Columns[4].Name = "Col_txttrans_date_client";
                            //this.GridView4.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView4.Columns[6].Name = "Col_txtproduce_type_name";
                            //this.GridView4.Columns[7].Name = "Col_txtwherehouse_name";
                            //this.GridView4.Columns[8].Name = "Col_txtmat_id";
                            //this.GridView4.Columns[9].Name = "Col_txtmat_name";
                            //this.GridView4.Columns[10].Name = "Col_txtnumber_mat_id";
                            //this.GridView4.Columns[11].Name = "Col_txtmachine_id";
                            //this.GridView4.Columns[12].Name = "Col_txtface_baking_name";
                            //this.GridView4.Columns[13].Name = "Col_txtsum_qty";
                            //this.GridView4.Columns[14].Name = "Col_txtsum2_qty";
                            //this.GridView4.Columns[15].Name = "Col_txticrf_status";

                            var index = this.GridView4.Rows.Add();
                            this.GridView4.Rows[index].Cells["Col_txtnumber_in_year"].Value = "";// dt2.Rows[j]["txtnumber_in_year"].ToString(); //0
                            this.GridView4.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView4.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView4.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txticrf_id"].ToString();      //3
                            this.GridView4.Rows[index].Cells["Col_txtic_id"].Value = "";      //3
                            this.GridView4.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView4.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView4.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //6
                            this.GridView4.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //7
                            this.GridView4.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView4.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            this.GridView4.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //10
                            this.GridView4.Rows[index].Cells["Col_txtmachine_id"].Value = "";    //11
                            this.GridView4.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //12

                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_ic"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //13
                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_yes"].Value = "0";        //13
                            this.GridView4.Rows[index].Cells["Col_txtsum2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum2_qty"]).ToString("###,###.00");      //14

                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_change"].Value  = "0";        //14
                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_change_rate"].Value = "0";       //14

                            //ic==============================
                            if (dt2.Rows[j]["txticrf_status"].ToString() == "0")
                            {
                                this.GridView4.Rows[index].Cells["Col_txticrf_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txticrf_status"].ToString() == "1")
                            {
                                this.GridView4.Rows[index].Cells["Col_txticrf_status"].Value = "ยกเลิก"; //15
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
            GridView4_Color();
            GridView4_Color_Column();
        }
        private void Show_GridView4()
        {
            this.GridView4.ColumnCount = 20;
            this.GridView4.Columns[0].Name = "Col_txtnumber_in_year";
            this.GridView4.Columns[1].Name = "Col_txtco_id";
            this.GridView4.Columns[2].Name = "Col_txtbranch_id";
            this.GridView4.Columns[3].Name = "Col_txtFG1_id";
            this.GridView4.Columns[4].Name = "Col_txtic_id";
            this.GridView4.Columns[5].Name = "Col_txttrans_date_client";
            this.GridView4.Columns[6].Name = "Col_txttrans_time";
            this.GridView4.Columns[7].Name = "Col_txtproduce_type_name";
            this.GridView4.Columns[8].Name = "Col_txtwherehouse_name";
            this.GridView4.Columns[9].Name = "Col_txtmat_id";
            this.GridView4.Columns[10].Name = "Col_txtmat_name";
            this.GridView4.Columns[11].Name = "Col_txtnumber_mat_id";
            this.GridView4.Columns[12].Name = "Col_txtmachine_id";
            this.GridView4.Columns[13].Name = "Col_txtface_baking_name";
            this.GridView4.Columns[14].Name = "Col_txtsum_qty_ic";
            this.GridView4.Columns[15].Name = "Col_txtsum_qty_yes";
            this.GridView4.Columns[16].Name = "Col_txtsum2_qty";
            this.GridView4.Columns[17].Name = "Col_txtsum_qty_change";
            this.GridView4.Columns[18].Name = "Col_txtsum_qty_change_rate";
            this.GridView4.Columns[19].Name = "Col_txticrf_status";

            this.GridView4.Columns[0].HeaderText = "เลขที่ชุด";
            this.GridView4.Columns[1].HeaderText = "txtco_id";
            this.GridView4.Columns[2].HeaderText = " txtbranch_id";
            this.GridView4.Columns[3].HeaderText = " เลขที่ FG1";
            this.GridView4.Columns[4].HeaderText = " เลขที่เบิกด้าย";
            this.GridView4.Columns[5].HeaderText = " วันที่";
            this.GridView4.Columns[6].HeaderText = " เวลา";
            this.GridView4.Columns[7].HeaderText = "ประเภทผลิต";
            this.GridView4.Columns[8].HeaderText = "คลัง";
            this.GridView4.Columns[9].HeaderText = "รหัสวัตถุดิบ";
            this.GridView4.Columns[10].HeaderText = "ชื่อวัตถุดิบ";
            this.GridView4.Columns[11].HeaderText = "เบอร์วัตถุดิบ";
            this.GridView4.Columns[12].HeaderText = "เครื่องจักร";
            this.GridView4.Columns[13].HeaderText = "อบหน้า";

            this.GridView4.Columns[14].HeaderText = "นน รวม กก.";
            this.GridView4.Columns[15].HeaderText = "ผลิตผ้าดิบได้ กก.";
            this.GridView4.Columns[16].HeaderText = "ผลิตผ้าดิบได้ ปอนด์";
            this.GridView4.Columns[17].HeaderText = "ส่วนต่าง กก";
            this.GridView4.Columns[18].HeaderText = "% ส่วนต่าง";
            this.GridView4.Columns[19].HeaderText = " สถานะ";

            this.GridView4.Columns["Col_txtnumber_in_year"].Visible = false;  //"Col_txtnumber_in_year";
            this.GridView4.Columns["Col_txtnumber_in_year"].Width = 0;
            this.GridView4.Columns["Col_txtnumber_in_year"].ReadOnly = true;
            this.GridView4.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView4.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.GridView4.Columns["Col_txtbranch_id"].Visible = false;  //"Col_txtbranch_id";

            this.GridView4.Columns["Col_txtFG1_id"].Visible = true;  //"Col_txtFG1_id";
            this.GridView4.Columns["Col_txtFG1_id"].Width = 140;
            this.GridView4.Columns["Col_txtFG1_id"].ReadOnly = true;
            this.GridView4.Columns["Col_txtFG1_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtFG1_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtic_id"].Visible = false;  //"Col_txtic_id";
            this.GridView4.Columns["Col_txtic_id"].Width = 0;
            this.GridView4.Columns["Col_txtic_id"].ReadOnly = true;
            this.GridView4.Columns["Col_txtic_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtic_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txttrans_date_client"].Visible = true;  //"Col_txttrans_date_client";
            this.GridView4.Columns["Col_txttrans_date_client"].Width = 100;
            this.GridView4.Columns["Col_txttrans_date_client"].ReadOnly = true;
            this.GridView4.Columns["Col_txttrans_date_client"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txttrans_date_client"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txttrans_time"].Visible = true;  //"Col_txttrans_time";
            this.GridView4.Columns["Col_txttrans_time"].Width = 80;
            this.GridView4.Columns["Col_txttrans_time"].ReadOnly = true;
            this.GridView4.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtproduce_type_name"].Visible = true;  //"Col_txtproduce_type_name";
            this.GridView4.Columns["Col_txtproduce_type_name"].Width = 100;
            this.GridView4.Columns["Col_txtproduce_type_name"].ReadOnly = true;
            this.GridView4.Columns["Col_txtproduce_type_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtproduce_type_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtwherehouse_name"].Visible = true;  //"Col_txtwherehouse_name";
            this.GridView4.Columns["Col_txtwherehouse_name"].Width = 120;
            this.GridView4.Columns["Col_txtwherehouse_name"].ReadOnly = true;
            this.GridView4.Columns["Col_txtwherehouse_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtwherehouse_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView4.Columns["Col_txtmat_id"].Width = 120;
            this.GridView4.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView4.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView4.Columns["Col_txtmat_name"].Width = 120;
            this.GridView4.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView4.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";
            this.GridView4.Columns["Col_txtnumber_mat_id"].Width = 0;
            this.GridView4.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView4.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtmachine_id"].Visible = false;  //"Col_txtmachine_id";
            this.GridView4.Columns["Col_txtmachine_id"].Width = 0;
            this.GridView4.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView4.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView4.Columns["Col_txtface_baking_name"].Visible = true;  //"Col_txtface_baking_name";
            this.GridView4.Columns["Col_txtface_baking_name"].Width = 120;
            this.GridView4.Columns["Col_txtface_baking_name"].ReadOnly = true;
            this.GridView4.Columns["Col_txtface_baking_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtface_baking_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView4.Columns["Col_txtsum_qty_ic"].Visible = true;  //"Col_txtsum_qty_ic";
            this.GridView4.Columns["Col_txtsum_qty_ic"].Width = 120;
            this.GridView4.Columns["Col_txtsum_qty_ic"].ReadOnly = true;
            this.GridView4.Columns["Col_txtsum_qty_ic"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtsum_qty_ic"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView4.Columns["Col_txtsum_qty_yes"].Visible = false;  //"Col_txtsum_qty_yes";
            this.GridView4.Columns["Col_txtsum_qty_yes"].Width = 0;
            this.GridView4.Columns["Col_txtsum_qty_yes"].ReadOnly = true;
            this.GridView4.Columns["Col_txtsum_qty_yes"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtsum_qty_yes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView4.Columns["Col_txtsum2_qty"].Visible = false;  //"Col_txtsum2_qty";
            this.GridView4.Columns["Col_txtsum2_qty"].Width = 0;
            this.GridView4.Columns["Col_txtsum2_qty"].ReadOnly = true;
            this.GridView4.Columns["Col_txtsum2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtsum2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView4.Columns["Col_txtsum_qty_change"].Visible = false;  //"Col_txtsum_qty_change";
            this.GridView4.Columns["Col_txtsum_qty_change"].Width = 0;
            this.GridView4.Columns["Col_txtsum_qty_change"].ReadOnly = true;
            this.GridView4.Columns["Col_txtsum_qty_change"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtsum_qty_change"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView4.Columns["Col_txtsum_qty_change_rate"].Visible = false;  //"Col_txtsum_qty_change_rate";
            this.GridView4.Columns["Col_txtsum_qty_change_rate"].Width = 0;
            this.GridView4.Columns["Col_txtsum_qty_change_rate"].ReadOnly = true;
            this.GridView4.Columns["Col_txtsum_qty_change_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txtsum_qty_change_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView4.Columns["Col_txticrf_status"].Visible = true;  //"Col_txticrf_status";
            this.GridView4.Columns["Col_txticrf_status"].Width = 100;
            this.GridView4.Columns["Col_txticrf_status"].ReadOnly = true;
            this.GridView4.Columns["Col_txticrf_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView4.Columns["Col_txticrf_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView4.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView4.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView4.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView4.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView4.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView4.EnableHeadersVisualStyles = false;


        }
        private void Clear_GridView4()
        {
            this.GridView4.Rows.Clear();
            this.GridView4.Refresh();
        }
        private void GridView4_Color()
        {
            for (int i = 0; i < this.GridView4.Rows.Count - 0; i++)
            {

                //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView4.Rows[i].Cells["Col_txtmoney_after_vat_creditor"].Value.ToString())) > 0)
                //{
                //    GridView4.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                //    GridView4.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                //    GridView4.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                //}

            }
        }
        private void GridView4_Color_Column()
        {

            for (int i = 0; i < this.GridView4.Rows.Count - 0; i++)
            {
                GridView4.Rows[i].Cells["Col_txtmachine_id"].Style.BackColor = Color.LightGreen;
                GridView4.Rows[i].Cells["Col_txtsum_qty_ic"].Style.BackColor = Color.LightSkyBlue;
                GridView4.Rows[i].Cells["Col_txtsum_qty_yes"].Style.BackColor = Color.LightGreen;
                GridView4.Rows[i].Cells["Col_txtsum_qty_change"].Style.BackColor = Color.LightSkyBlue;
                GridView4.Rows[i].Cells["Col_txtsum_qty_change_rate"].Style.BackColor = Color.LightGreen;

            }
        }
        private void GridView4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView4.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtco_id"].Value;
                if (cell != null)
                {
                    W_ID_Select.TRANS_ID = row.Cells["Col_txtFG1_id"].Value.ToString();
                    this.cboSearch.Text = "เลขที่FG1 ผ้าดิบ";

                    if (this.cboSearch.Text == "เลขที่FG1 ผ้าดิบ")
                    {
                        this.txtsearch.Text = row.Cells["Col_txtFG1_id"].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells["Col_txtFG1_id"].Value.ToString();

                    }
                    else if (this.cboSearch.Text == "รหัสสินค้า")
                    {
                        this.txtsearch.Text = row.Cells["Col_txtmat_id"].Value.ToString();

                    }
                    else
                    {
                        this.txtsearch.Text = row.Cells["Col_txtFG1_id"].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells["Col_txtFG1_id"].Value.ToString();

                    }
                }
                //=====================
                W_ID_Select.IDS1 = row.Cells["Col_txtnumber_in_year"].Value.ToString();
                Fill_Show_DATA_GridView5();
            }
        }
        private void GridView4_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    GridView4.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView4.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView4_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                if (GridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {

                }
                else if (GridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.OrangeRed)
                {

                }
                else
                {
                    GridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    GridView4.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    GridView4.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView4_DoubleClick(object sender, EventArgs e)
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
                W_ID_Select.WORD_TOP = "ดูข้อมูลFG1 ผ้าดิบ";
                kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_detail frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_detail();
                frm2.Show();

                TRANS_LOG();

            }
        }

        private void Fill_Show_DATA_GridView5()
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

            Clear_GridView5();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c002_02produce_record_detail.*," +
                                  //"c002_02produce_record_machine.*," +
                                  //"c002_02produce_record_detail.*," +
                                   //"c001_04produce_type.*," +
                                   "c001_02machine.*" +
                                   //"c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +

                                   //"k013_1db_acc_13group_tax.*," +

                                   //"k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_02produce_record_detail" +

                                   //" INNER JOIN c002_02produce_record_machine" +
                                   //" ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                   //" AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +


                                   //" INNER JOIN c002_02produce_record_detail" +
                                   //" ON c002_02produce_record.cdkey = c002_02produce_record_detail.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c002_02produce_record_detail.txtco_id" +
                                   ////" AND c002_02produce_record.txticrf_id = c002_02produce_record_detail.txticrf_id" +
                                   //" AND c002_02produce_record.txtnumber_in_year = c002_02produce_record_detail.txtnumber_in_year" +

                                   //" INNER JOIN c001_04produce_type" +
                                   //" ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                   //" AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   " INNER JOIN c001_02machine" +
                                   " ON c002_02produce_record_detail.cdkey = c001_02machine.cdkey" +
                                   " AND c002_02produce_record_detail.txtco_id = c001_02machine.txtco_id" +
                                   " AND c002_02produce_record_detail.txtmachine_id = c001_02machine.txtmachine_id" +

                                   //" INNER JOIN c001_05face_baking" +
                                   //" ON c002_02produce_record_detail.cdkey = c001_05face_baking.cdkey" +
                                   //" AND c002_02produce_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                   //" AND c002_02produce_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   //" INNER JOIN k013_1db_acc_13group_tax" +
                                   //" ON c002_02produce_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   //" INNER JOIN k013_1db_acc_06wherehouse" +
                                   //" ON c002_02produce_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   //" AND c002_02produce_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   //" AND c002_02produce_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_02produce_record_detail.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_02produce_record_detail.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                 " AND (c002_02produce_record_detail.txticrf_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                 //   " AND (c002_02produce_record_detail.txtnumber_in_year = '" + W_ID_Select.IDS1.Trim() + "')" +
                                 " ORDER BY c002_02produce_record_detail.txtfold_number ASC";

                
                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        Int32 k = 0;


                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            k = j + 1;

                            var index = GridView5.Rows.Add();
                            GridView5.Rows[index].Cells["Col_Auto_num"].Value = k.ToString("000"); //0
                            GridView5.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            GridView5.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                            GridView5.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //3

                            GridView5.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //4

                            GridView5.Rows[index].Cells["Col_txttrans_time_start"].Value = "0";       //5
                            GridView5.Rows[index].Cells["Col_txttrans_time_end"].Value = "0";       //6

                            GridView5.Rows[index].Cells["Col_Problem1"].Value = "0";       //7
                            GridView5.Rows[index].Cells["Col_Problem2"].Value = "0";       //8
                            GridView5.Rows[index].Cells["Col_Problem3"].Value = "0";       //9
                            GridView5.Rows[index].Cells["Col_Problem4"].Value = "0";       //10

                            GridView5.Rows[index].Cells["Col_txtemp_id"].Value = "0";      //11
                            GridView5.Rows[index].Cells["Col_txtemp_name"].Value = "0";       //12
                            GridView5.Rows[index].Cells["Col_txtshift_name"].Value = "0";       //13


                            GridView5.Rows[index].Cells["Col_txticrf_remark"].Value = "0";       //14

                            GridView5.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //15
                            GridView5.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //16
                            GridView5.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //17
                            GridView5.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();     //18

                            GridView5.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //19
                            GridView5.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //20

                            GridView5.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //21

                            GridView5.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //22
                            GridView5.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //23

                            GridView5.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");      //24


                            GridView5.Rows[index].Cells["Col_txtprice"].Value = "0";        //25
                            GridView5.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";       //26
                            GridView5.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";       //27
                            GridView5.Rows[index].Cells["Col_txtsum_total"].Value = "0";      //28

                            GridView5.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //29
                            GridView5.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";        //30
                            GridView5.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";        //31

                            GridView5.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";       //32
                            GridView5.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";       //33
                            GridView5.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";       //34

                            GridView5.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";        //35
                            GridView5.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";       //36

                            GridView5.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //37

                            GridView5.Rows[index].Cells["Col_mat_status"].Value = "0";

                            GridView5.Rows[index].Cells["Col_txtface_baking_id"].Value = "";   //41
                            GridView5.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();     //42



                            GridView5.Rows[index].Cells["Col_txtqty_cut"].Value = "0";     //35
                            GridView5.Rows[index].Cells["Col_txtqty_after_cut"].Value = "0";     //36
                            GridView5.Rows[index].Cells["Col_txtcut_id"].Value = "0";       //37

                            GridView5.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txticrf_id"].ToString();      //37


                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;
                        //this.GridView5.Columns[37].Name = "Col_txtitem_no";
                        //this.GridView5.Columns[38].Name = "Col_txtqc_status";
                        //this.GridView5.Columns[39].Name = "Col_txtqc_id";
                        //this.GridView5.Columns[40].Name = "Col_txtppt_status";
                        //this.GridView5.Columns[41].Name = "Col_txtppt_id";
                        //this.GridView5.Columns[42].Name = "Col_txtlot_no";


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
            GridView5_Color_Column();

            //================================

        }
        private void Show_GridView5()
        {
            this.GridView5.ColumnCount = 49;
            this.GridView5.Columns[0].Name = "Col_Auto_num";
            this.GridView5.Columns[1].Name = "Col_txtnumber_in_year";
            this.GridView5.Columns[2].Name = "Col_txtmachine_id";
            this.GridView5.Columns[3].Name = "Col_txtfold_number";

            this.GridView5.Columns[4].Name = "Col_txtqty";

            this.GridView5.Columns[5].Name = "Col_txttrans_time_start";
            this.GridView5.Columns[6].Name = "Col_txttrans_time_end";

            this.GridView5.Columns[7].Name = "Col_Problem1";
            this.GridView5.Columns[8].Name = "Col_Problem2";
            this.GridView5.Columns[9].Name = "Col_Problem3";
            this.GridView5.Columns[10].Name = "Col_Problem4";

            this.GridView5.Columns[11].Name = "Col_txtemp_id";
            this.GridView5.Columns[12].Name = "Col_txtemp_name";

            this.GridView5.Columns[13].Name = "Col_txtshift_name";

            this.GridView5.Columns[14].Name = "Col_txticrf_remark";


            this.GridView5.Columns[15].Name = "Col_txtmat_no";
            this.GridView5.Columns[16].Name = "Col_txtmat_id";
            this.GridView5.Columns[17].Name = "Col_txtmat_name";
            this.GridView5.Columns[18].Name = "Col_txtnumber_mat_id";
            //this.GridView5.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";

            this.GridView5.Columns[19].Name = "Col_txtmat_unit1_name";
            this.GridView5.Columns[20].Name = "Col_txtmat_unit1_qty";
            this.GridView5.Columns[21].Name = "Col_chmat_unit_status";
            this.GridView5.Columns[22].Name = "Col_txtmat_unit2_name";
            this.GridView5.Columns[23].Name = "Col_txtmat_unit2_qty";

            this.GridView5.Columns[24].Name = "Col_txtqty2";

            this.GridView5.Columns[25].Name = "Col_txtprice";
            this.GridView5.Columns[26].Name = "Col_txtdiscount_rate";
            this.GridView5.Columns[27].Name = "Col_txtdiscount_money";
            this.GridView5.Columns[28].Name = "Col_txtsum_total";

            this.GridView5.Columns[29].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView5.Columns[30].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView5.Columns[31].Name = "Col_txtcost_money_sum_yokma";

            this.GridView5.Columns[32].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView5.Columns[33].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView5.Columns[34].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView5.Columns[35].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView5.Columns[36].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView5.Columns[37].Name = "Col_txtitem_no";
            this.GridView5.Columns[38].Name = "Col_mat_status";
            this.GridView5.Columns[39].Name = "Col_txtface_baking_id";
            this.GridView5.Columns[40].Name = "Col_txtlot_no";

            this.GridView5.Columns[41].Name = "Col_txtqty_after_cut";
            this.GridView5.Columns[42].Name = "Col_txtqty_cut_yokma";
            this.GridView5.Columns[43].Name = "Col_txtqty_cut_yokpai";
            this.GridView5.Columns[44].Name = "Col_txtqty_after_cut_yokpai";
            this.GridView5.Columns[41].Visible = false;
            this.GridView5.Columns[42].Visible = false;
            this.GridView5.Columns[43].Visible = false;
            this.GridView5.Columns[44].Visible = false;

            this.GridView5.Columns[45].Name = "Col_txtqty_cut";
            this.GridView5.Columns[46].Name = "Col_txtqty_after_cut";
            this.GridView5.Columns[47].Name = "Col_txtcut_id";
            this.GridView5.Columns[48].Name = "Col_txtFG1_id";
            this.GridView5.Columns[45].Visible = false;
            this.GridView5.Columns[46].Visible = false;
            this.GridView5.Columns[47].Visible = false;
            this.GridView5.Columns[48].Visible = false;

            this.GridView5.Columns[0].HeaderText = "No";
            this.GridView5.Columns[1].HeaderText = "เลขที่ชุด";
            this.GridView5.Columns[2].HeaderText = "เครื่องจักร";
            this.GridView5.Columns[3].HeaderText = "ม้วนที่";

            this.GridView5.Columns[4].HeaderText = "น้ำหนัก/ม้วน(กก.)";

            this.GridView5.Columns[5].HeaderText = " เวลาเริ่ม";
            this.GridView5.Columns[6].HeaderText = " เวลาเสร็จ";

            this.GridView5.Columns[7].HeaderText = "เข็มหัก";
            this.GridView5.Columns[8].HeaderText = "เป็นรู";
            this.GridView5.Columns[9].HeaderText = "ผ้าตก";
            this.GridView5.Columns[10].HeaderText = "ด้ายขาด";

            this.GridView5.Columns[11].HeaderText = "รหัสผู้ดูแล";
            this.GridView5.Columns[12].HeaderText = "ชื่อผู้ดูแล";
            this.GridView5.Columns[13].HeaderText = "กะ";
            this.GridView5.Columns[14].HeaderText = "หมายเหตุ";

            this.GridView5.Columns[15].HeaderText = "ลำดับ";
            this.GridView5.Columns[16].HeaderText = "รหัส";
            this.GridView5.Columns[17].HeaderText = "ชื่อสินค้า";
            this.GridView5.Columns[18].HeaderText = "เบอร์เส้นด้าย";

            this.GridView5.Columns[19].HeaderText = " หน่วยหลัก";
            this.GridView5.Columns[20].HeaderText = " หน่วย";
            this.GridView5.Columns[21].HeaderText = "แปลง";
            this.GridView5.Columns[22].HeaderText = " หน่วย(ปอนด์)";
            this.GridView5.Columns[23].HeaderText = " หน่วย";

            this.GridView5.Columns[24].HeaderText = "น้ำหนัก/ม้วน(ปอนด์)";

            this.GridView5.Columns[25].HeaderText = "ราคา";
            this.GridView5.Columns[26].HeaderText = "ส่วนลด(%)";
            this.GridView5.Columns[27].HeaderText = "ส่วนลด(บาท)";
            this.GridView5.Columns[28].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView5.Columns[29].HeaderText = "จำนวนยกมา";
            this.GridView5.Columns[30].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView5.Columns[31].HeaderText = "จำนวนเงิน";

            this.GridView5.Columns[32].HeaderText = "จำนวนยกไป";
            this.GridView5.Columns[33].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView5.Columns[34].HeaderText = "จำนวนเงิน";

            this.GridView5.Columns[35].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView5.Columns[36].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView5.Columns[37].HeaderText = "item_no";
            this.GridView5.Columns[38].HeaderText = "สถานะ";
            this.GridView5.Columns[39].HeaderText = "อบหน้า";
            this.GridView5.Columns[40].HeaderText = "Lot No";

            this.GridView5.Columns[41].HeaderText = "Col_txtqty_after_cut ยกมา";
            this.GridView5.Columns[42].HeaderText = "รวมจำนวนรับคืนแล้วยกมา";
            this.GridView5.Columns[43].HeaderText = "รวมจำนวนรับคืนแล้วยกไป";
            this.GridView5.Columns[44].HeaderText = "เหลือรอรับอีก กก.";
            this.GridView5.Columns[48].HeaderText = "เลขที่ FG1";


            this.GridView5.Columns["Col_Auto_num"].Visible = true;  //"Col_Auto_num";
            this.GridView5.Columns["Col_Auto_num"].Width = 80;
            this.GridView5.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView5.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtnumber_in_year"].Visible = true;  //"Col_txtnumber_in_year";
            this.GridView5.Columns["Col_txtnumber_in_year"].Width = 100;
            this.GridView5.Columns["Col_txtnumber_in_year"].ReadOnly = true;
            this.GridView5.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            this.GridView5.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView5.Columns["Col_txtmachine_id"].Width = 80;
            this.GridView5.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView5.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView5.Columns["Col_txtfold_number"].Width = 60;
            this.GridView5.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView5.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView5.Columns["Col_txtqty"].Width = 140;
            this.GridView5.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView5.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //DataGridViewTextBoxColumn txttime = new DataGridViewTextBoxColumn();
            //txttime.Name = "Col_Time1";
            //txttime.Width =60;
            //txttime.DisplayIndex = 2;
            //txttime.HeaderText = "เวลา";
            //txttime.DefaultCellStyle.Format = ("HH:mm");
            ////cboemp.MaxDropDownItems = 100;
            ////cboemp.ReadOnly = false;
            //txttime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //txttime.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            //txttime.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //this.GridView5.Columns.Add(txttime);

            this.GridView5.Columns["Col_txttrans_time_start"].Visible = false;  //"Col_txttrans_time_start";
            this.GridView5.Columns["Col_txttrans_time_start"].Width = 0;
            this.GridView5.Columns["Col_txttrans_time_start"].ReadOnly = false;
            this.GridView5.Columns["Col_txttrans_time_start"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txttrans_time_start"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView5.Columns["Col_txttrans_time_end"].Visible = false;  //"Col_txttrans_time_end";
            this.GridView5.Columns["Col_txttrans_time_end"].Width = 0;
            this.GridView5.Columns["Col_txttrans_time_end"].ReadOnly = false;
            this.GridView5.Columns["Col_txttrans_time_end"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txttrans_time_end"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_Problem1"].Visible = false;  //"Col_Problem1";
            this.GridView5.Columns["Col_Problem1"].Width = 0;
            this.GridView5.Columns["Col_Problem1"].ReadOnly = false;
            this.GridView5.Columns["Col_Problem1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_Problem1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_Problem2"].Visible = false;  //"Col_Problem2";
            this.GridView5.Columns["Col_Problem2"].Width = 0;
            this.GridView5.Columns["Col_Problem2"].ReadOnly = false;
            this.GridView5.Columns["Col_Problem2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_Problem2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_Problem3"].Visible = false;  //"Col_Problem3";
            this.GridView5.Columns["Col_Problem3"].Width = 0;
            this.GridView5.Columns["Col_Problem3"].ReadOnly = false;
            this.GridView5.Columns["Col_Problem3"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_Problem3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_Problem4"].Visible = false;  //"Col_Problem4";
            this.GridView5.Columns["Col_Problem4"].Width = 0;
            this.GridView5.Columns["Col_Problem4"].ReadOnly = false;
            this.GridView5.Columns["Col_Problem4"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_Problem4"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            //DataGridViewComboBoxColumn cboemp = new DataGridViewComboBoxColumn();
            //cboemp.Name = "Col_Combo1";
            //cboemp.Width = 200;
            //cboemp.DisplayIndex = 21;
            //cboemp.HeaderText = "ผู้เบิก...";
            ////cboemp.MaxDropDownItems = 100;
            ////cboemp.ReadOnly = false;
            //cboemp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //cboemp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            //cboemp.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //this.GridView5.Columns.Add(cboemp);

            this.GridView5.Columns["Col_txtemp_id"].Visible = false;  //"Col_txtemp_id";
            this.GridView5.Columns["Col_txtemp_id"].Width = 0;
            this.GridView5.Columns["Col_txtemp_id"].ReadOnly = false;
            this.GridView5.Columns["Col_txtemp_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtemp_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtemp_name"].Visible = false;  //"Col_txtemp_name";
            this.GridView5.Columns["Col_txtemp_name"].Width = 0;
            this.GridView5.Columns["Col_txtemp_name"].ReadOnly = false;
            this.GridView5.Columns["Col_txtemp_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtemp_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtshift_name"].Visible = false;  //"Col_txtshift_name";
            this.GridView5.Columns["Col_txtshift_name"].Width = 0;
            this.GridView5.Columns["Col_txtshift_name"].ReadOnly = false;
            this.GridView5.Columns["Col_txtshift_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtshift_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_txticrf_remark"].Visible = false;  //"Col_txticrf_remark";
            this.GridView5.Columns["Col_txticrf_remark"].Width = 0;
            this.GridView5.Columns["Col_txticrf_remark"].ReadOnly = false;
            this.GridView5.Columns["Col_txticrf_remark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txticrf_remark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";
            this.GridView5.Columns["Col_txtmat_no"].Width = 0;
            this.GridView5.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView5.Columns["Col_txtmat_id"].Width = 80;
            this.GridView5.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView5.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView5.Columns["Col_txtmat_name"].Width = 150;
            this.GridView5.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView5.Columns["Col_txtmat_name"].Width = 150;
            this.GridView5.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_txtnumber_mat_id"].Visible = true;  //"Col_txtnumber_mat_id";
            this.GridView5.Columns["Col_txtnumber_mat_id"].Width = 120;
            this.GridView5.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView5.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.GridView5.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView5.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView5.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView5.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView5.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 0;  //70
            dgvCmb.DisplayIndex = 20;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.Visible = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView5.Columns.Add(dgvCmb);

            this.GridView5.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.GridView5.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.GridView5.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView5.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView5.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView5.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView5.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;




            this.GridView5.Columns["Col_txtqty2"].Visible = false;  //"Col_txtqty2";
            this.GridView5.Columns["Col_txtqty2"].Width = 0;
            this.GridView5.Columns["Col_txtqty2"].ReadOnly = true;
            this.GridView5.Columns["Col_txtqty2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtqty2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView5.Columns["Col_txtprice"].Visible = false;  //"Col_txtprice";
            this.GridView5.Columns["Col_txtprice"].Width = 0;
            this.GridView5.Columns["Col_txtprice"].ReadOnly = true;
            this.GridView5.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.GridView5.Columns["Col_txtdiscount_rate"].Width = 0;
            this.GridView5.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.GridView5.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtdiscount_money"].Visible = false;  //"Col_txtdiscount_money";
            this.GridView5.Columns["Col_txtdiscount_money"].Width = 0;
            this.GridView5.Columns["Col_txtdiscount_money"].ReadOnly = false;
            this.GridView5.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtsum_total"].Visible = false;  //"Col_txtsum_total";
            this.GridView5.Columns["Col_txtsum_total"].Width = 0;
            this.GridView5.Columns["Col_txtsum_total"].ReadOnly = true;
            this.GridView5.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_qty_balance_yokma"].Visible = false;  //"Col_txtcost_qty_balance_yokma";
            this.GridView5.Columns["Col_txtcost_qty_balance_yokma"].Width = 0;
            this.GridView5.Columns["Col_txtcost_qty_balance_yokma"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_qty_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_qty_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_qty_price_average_yokma"].Visible = false;  //"Col_txtcost_qty_price_average_yokma";
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokma"].Width = 0;
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokma"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_money_sum_yokma"].Visible = false;  //"Col_txtcost_money_sum_yokma";
            this.GridView5.Columns["Col_txtcost_money_sum_yokma"].Width = 0;
            this.GridView5.Columns["Col_txtcost_money_sum_yokma"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_money_sum_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_money_sum_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_qty_balance_yokpai"].Visible = false;  //"Col_txtcost_qty_balance_yokpai";
            this.GridView5.Columns["Col_txtcost_qty_balance_yokpai"].Width = 0;
            this.GridView5.Columns["Col_txtcost_qty_balance_yokpai"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_qty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_qty_price_average_yokpai"].Visible = false;  //"Col_txtcost_qty_price_average_yokpai";
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokpai"].Width = 0;
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokpai"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_qty_price_average_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_money_sum_yokpai"].Visible = false;  //"Col_txtcost_money_sum_yokpai";
            this.GridView5.Columns["Col_txtcost_money_sum_yokpai"].Width = 0;
            this.GridView5.Columns["Col_txtcost_money_sum_yokpai"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_money_sum_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_qty2_balance_yokma"].Visible = false;  //"Col_txtcost_qty2_balance_yokma";
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokma"].Width = 0;
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokma"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtcost_qty2_balance_yokpai"].Visible = false;  //"Col_txtcost_qty2_balance_yokpai";
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokpai"].Width = 0;
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokpai"].ReadOnly = true;
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtcost_qty2_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView5.Columns["Col_txtitem_no"].Visible = false;  //"Col_txtitem_no";
            this.GridView5.Columns["Col_txtitem_no"].Width = 0;
            this.GridView5.Columns["Col_txtitem_no"].ReadOnly = true;
            this.GridView5.Columns["Col_txtitem_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtitem_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";
            this.GridView5.Columns["Col_mat_status"].Width = 0;
            this.GridView5.Columns["Col_mat_status"].ReadOnly = true;
            this.GridView5.Columns["Col_mat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_mat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtface_baking_id"].Visible = false;  //"Col_txtface_baking_id";
            this.GridView5.Columns["Col_txtface_baking_id"].Width = 0;
            this.GridView5.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView5.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView5.Columns["Col_txtlot_no"].Visible = true;  //"Col_txtlot_no";
            this.GridView5.Columns["Col_txtlot_no"].Width = 160;
            this.GridView5.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView5.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            
            this.GridView5.Columns["Col_txtFG1_id"].Visible = true;  //"Col_txtFG1_id";
            this.GridView5.Columns["Col_txtFG1_id"].Width = 160;
            this.GridView5.Columns["Col_txtFG1_id"].ReadOnly = true;
            this.GridView5.Columns["Col_txtFG1_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView5.Columns["Col_txtFG1_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;



            this.GridView5.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView5.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView5.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView5.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView5.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView5.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView5.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView5()
        {
            this.GridView5.Rows.Clear();
            this.GridView5.Refresh();
        }
        private void GridView5_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView5.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView5.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView5_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView5.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView5.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView5_Color_Column()
        {

            for (int i = 0; i < this.GridView5.Rows.Count - 0; i++)
            {
                GridView5.Rows[i].Cells["Col_txtfold_number"].Style.BackColor = Color.LightSkyBlue;
                GridView5.Rows[i].Cells["Col_txtlot_no"].Style.BackColor = Color.LightGreen;

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

                W_ID_Select.WORD_TOP = "บันทึกFG1 ผ้าดิบ";
                kondate.soft.HOME03_Production.HOME03_Production_03Produce_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_record();
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
                W_ID_Select.WORD_TOP = "ดูข้อมูลFG1 ผ้าดิบ";
                kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_detail frm2 = new kondate.soft.HOME03_Production.HOME03_Production_03Produce_record_detail();
                frm2.Show();

                TRANS_LOG();

            }
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
            if (this.ch_all_branch.Checked == true)
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

                //this.cboSearch.Items.Add("เลขที่FG1 ผ้าดิบ");
                //this.cboSearch.Items.Add("ชื่อสินค้าผลิต");

                if (this.cboSearch.Text.Trim() == "เลขที่FG1 ผ้าดิบ")
                {
                    cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                       "c002_02produce_record_machine.*," +
                                       "c001_04produce_type.*," +
                                       "c001_02machine.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_02produce_record" +

                                       " INNER JOIN c002_02produce_record_machine" +
                                       " ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                       " AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                       " AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +

                                       " INNER JOIN c001_04produce_type" +
                                       " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                       " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                       " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   " INNER JOIN c001_02machine" +
                                   " ON c002_02produce_record_machine.cdkey = c001_02machine.cdkey" +
                                   " AND c002_02produce_record_machine.txtco_id = c001_02machine.txtco_id" +
                                   " AND c002_02produce_record_machine.txtmachine_id = c001_02machine.txtmachine_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                                   " WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //     " AND (c002_02produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       //     " AND (c002_02produce_record.txttrans_date_client BETWEEN @datestart AND @dateend)" +
                                       " AND (c002_02produce_record.txticrf_id = '" + this.txtsearch.Text.Trim() + "')" +
                                      " ORDER BY c002_02produce_record.txticrf_id ASC";

                }
                if (this.cboSearch.Text.Trim() == "ชื่อสินค้าผลิต")
                {
                    cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                       "c002_02produce_record_machine.*," +
                                       "c001_04produce_type.*," +
                                       "c001_02machine.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_02produce_record" +

                                       " INNER JOIN c002_02produce_record_machine" +
                                       " ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                       " AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                       " AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +

                                       " INNER JOIN c001_04produce_type" +
                                       " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                       " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                       " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   " INNER JOIN c001_02machine" +
                                   " ON c002_02produce_record_machine.cdkey = c001_02machine.cdkey" +
                                   " AND c002_02produce_record_machine.txtco_id = c001_02machine.txtco_id" +
                                   " AND c002_02produce_record_machine.txtmachine_id = c001_02machine.txtmachine_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                       " WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       //     " AND (c002_02produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (c002_02produce_record.txttrans_date_client BETWEEN @datestart AND @dateend)" +
                                       " AND (c002_02produce_record.txtmat_name LIKE '%" + this.txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY c002_02produce_record.txticrf_id ASC";

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
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = ""; // dt2.Rows[j]["txtnumber_in_year"].ToString(); //0
                            this.GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txticrf_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtic_id"].Value = dt2.Rows[j]["txtic_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView1.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_name"].ToString();      //11
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //12

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_ic"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_ic"]).ToString("###,###.00");      //13
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_yes"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_yes"]).ToString("###,###.00");      //13
                            this.GridView1.Rows[index].Cells["Col_txtsum2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum2_qty"]).ToString("###,###.00");      //14

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_change"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_change"]).ToString("###,###.00");      //14
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_change_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty_change_rate"]).ToString("###,###.00");      //14

                            //ic==============================
                            if (dt2.Rows[j]["txticrf_status"].ToString() == "0")
                            {
                                this.GridView1.Rows[index].Cells["Col_txticrf_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txticrf_status"].ToString() == "1")
                            {
                                this.GridView1.Rows[index].Cells["Col_txticrf_status"].Value = "ยกเลิก"; //15
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
            GridView1_Color_Column();
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
        private void PANEL2_BRANCH_btn_search_Click(object sender, EventArgs e)
        {

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
        //END Check USER Rule=========================================================


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

        private void btng1_Click(object sender, EventArgs e)
        {
            this.GridView4.Visible = true;
            this.GridView1.Visible = false;
        }

        private void btng2_Click(object sender, EventArgs e)
        {
            this.GridView4.Visible = false;
            this.GridView1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Fill_Show_DATA_GridView41();
        }
        private void Fill_Show_DATA_GridView41()
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

            Clear_GridView4();


            //เชื่อมต่อฐานข้อมูล======================================================1กระสอบ มี 15หลอด
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                   //"c002_02produce_record_machine.*," +
                                   "c001_04produce_type.*," +
                                   //"c001_02machine.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_02produce_record" +

                                   //" INNER JOIN c002_02produce_record_machine" +
                                   //" ON c002_02produce_record.cdkey = c002_02produce_record_machine.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c002_02produce_record_machine.txtco_id" +
                                   //" AND c002_02produce_record.txticrf_id = c002_02produce_record_machine.txticrf_id" +

                                   " INNER JOIN c001_04produce_type" +
                                   " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                   " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   //" INNER JOIN c001_02machine" +
                                   //" ON c002_02produce_record_machine.cdkey = c001_02machine.cdkey" +
                                   //" AND c002_02produce_record_machine.txtco_id = c001_02machine.txtco_id" +
                                   //" AND c002_02produce_record_machine.txtmachine_id = c001_02machine.txtmachine_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   //" WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   //" AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //" AND (c002_02produce_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                   //" AND (c002_02produce_record.txttrans_date_client BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY c002_02produce_record.txticrf_id ASC";

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
                            //this.GridView4.Columns[0].Name = "Col_Auto_num";
                            //this.GridView4.Columns[1].Name = "Col_txtco_id";
                            //this.GridView4.Columns[2].Name = "Col_txtbranch_id";
                            //this.GridView4.Columns[3].Name = "Col_txtFG1_id";
                            //this.GridView4.Columns[4].Name = "Col_txttrans_date_client";
                            //this.GridView4.Columns[5].Name = "Col_txttrans_time";
                            //this.GridView4.Columns[6].Name = "Col_txtproduce_type_name";
                            //this.GridView4.Columns[7].Name = "Col_txtwherehouse_name";
                            //this.GridView4.Columns[8].Name = "Col_txtmat_id";
                            //this.GridView4.Columns[9].Name = "Col_txtmat_name";
                            //this.GridView4.Columns[10].Name = "Col_txtnumber_mat_id";
                            //this.GridView4.Columns[11].Name = "Col_txtmachine_id";
                            //this.GridView4.Columns[12].Name = "Col_txtface_baking_name";
                            //this.GridView4.Columns[13].Name = "Col_txtsum_qty";
                            //this.GridView4.Columns[14].Name = "Col_txtsum2_qty";
                            //this.GridView4.Columns[15].Name = "Col_txticrf_status";

                            var index = this.GridView4.Rows.Add();
                            this.GridView4.Rows[index].Cells["Col_txtnumber_in_year"].Value = ""; //  dt2.Rows[j]["txtnumber_in_year"].ToString(); //0
                            this.GridView4.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            this.GridView4.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranch_id"].ToString();      //2
                            this.GridView4.Rows[index].Cells["Col_txtFG1_id"].Value = dt2.Rows[j]["txticrf_id"].ToString();      //3
                            this.GridView4.Rows[index].Cells["Col_txtic_id"].Value = "";      //3
                            this.GridView4.Rows[index].Cells["Col_txttrans_date_client"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_client"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView4.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5
                            this.GridView4.Rows[index].Cells["Col_txtproduce_type_name"].Value = dt2.Rows[j]["txtproduce_type_name"].ToString();      //6
                            this.GridView4.Rows[index].Cells["Col_txtwherehouse_name"].Value = dt2.Rows[j]["txtwherehouse_name"].ToString();      //7
                            this.GridView4.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //8
                            this.GridView4.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //9
                            this.GridView4.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //10
                            this.GridView4.Rows[index].Cells["Col_txtmachine_id"].Value = "";    //11
                            this.GridView4.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //12

                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_ic"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_qty"]).ToString("###,###.00");      //13
                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_yes"].Value = "0";        //13
                            this.GridView4.Rows[index].Cells["Col_txtsum2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum2_qty"]).ToString("###,###.00");      //14

                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_change"].Value = "0";        //14
                            this.GridView4.Rows[index].Cells["Col_txtsum_qty_change_rate"].Value = "0";       //14

                            //ic==============================
                            if (dt2.Rows[j]["txticrf_status"].ToString() == "0")
                            {
                                this.GridView4.Rows[index].Cells["Col_txticrf_status"].Value = ""; //15
                            }
                            else if (dt2.Rows[j]["txticrf_status"].ToString() == "1")
                            {
                                this.GridView4.Rows[index].Cells["Col_txticrf_status"].Value = "ยกเลิก"; //15
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
            GridView4_Color();
            GridView4_Color_Column();
        }






        //Tans_Log ====================================================================

        //====================================================================
    }
}
