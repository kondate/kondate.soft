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
    public partial class HOME03_Production_05Send_Dye_GR : Form
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


        public HOME03_Production_05Send_Dye_GR()
        {
            InitializeComponent();
        }

        private void HOME03_Production_05Send_Dye_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            ////สำหรับทดสอบระบบ =====================================================================================================================
            //W_ID_Select.CDKEY = "samn20201125";//  = "chaixifactory2562020601"= "";// = "chaixifactory2562020601"= "";
            //W_ID_Select.ADATASOURCE = "916909b5121b.sn.mynetname.net,6001";  //916909b5121b.sn.mynetname.net,6001 // C4PC_AOT\\SQLEXPRESS,49170" //ASAICAFEKLONG3\\SQLEXPRESS,49170   //172.168.0.15\\SQLEXPRESS,49170
            //W_ID_Select.DATABASE_NAME = "samn_db"; //KREST2020
            //W_ID_Select.Lang = "001";// = "001"= ""; //001ไทย, 002Eng,003ลาว,004กัมพูชา,005พม่า

            //W_ID_Select.Crytal_SERVER = "916909b5121b.sn.mynetname.net,6001";
            //W_ID_Select.Crytal_DATABASE = "samn_db";
            //W_ID_Select.Crytal_USER = "sa";
            //W_ID_Select.Crytal_Pass = "Kon51Aot";

            //W_ID_Select.M_COID = "KD";
            //W_ID_Select.M_CONAME = "บริษัท ทดสอบระบบ จำกัด";
            //W_ID_Select.M_BRANCHID = "KD001";
            //W_ID_Select.M_BRANCHNAME = "สำนักงานใหญ่";
            //W_ID_Select.M_BRANCHNAME_SHORT = "HO";

            //W_ID_Select.M_USERNAME = "admin";
            //W_ID_Select.M_USERNAME_TYPE = "4";

            //FillDATE_FROM_SERVER();
            ////สำหรับทดสอบระบบ =====================================================================================================================

            W_ID_Select.M_FORM_NUMBER = "H0305SDGR";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblword_status.Text = "ตรวจใบส่งผ้าย้อม ค้างรับ";
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
            this.cboSearch.Items.Add("เลขที่ใบส่งผ้าย้อม");
            this.cboSearch.Items.Add("ชื่อผู้บันทึกใบส่งผ้าย้อม");

            //========================================
            PANEL2_BRANCH_GridView1_branch();
            PANEL2_BRANCH_Fill_branch();

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


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                   "k016db_1supplier.*," +
                                   "c002_05Send_dye_record_detail.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +
                                   "c001_07number_color.*," +

                                   "k016db_1supplier.*," +
                                   "k013_1db_acc_13group_tax.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_05Send_dye_record" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN c002_05Send_dye_record_detail" +
                                   " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                   " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                   " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                   " INNER JOIN c001_07number_color" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                    " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                  " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";

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
                            this.GridView1.Rows[index].Cells["Col_txtPPT_id"].Value = dt2.Rows[j]["txtPPT_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();        //4
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //5


                            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");     //8

                            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //13

                            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //16

                            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //17


                            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //18
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //19
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //20
                            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //21

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //22
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //23
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //24

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //25
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //26
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //27

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //28
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //29

                            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //30

                            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = "";      //31
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //32

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub"]).ToString("###,###.00");      //21

                            //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
                            GridView1.Rows[index].Cells["Col_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //9

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rib"]).ToString("###,###.00");      //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";       //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";       //21

                            //GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = dt2.Rows[j]["txtqty_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = dt2.Rows[j]["txtqty_after_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();  //17

                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokma"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokpai"]).ToString("###,###.00");      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive_yokpai"]).ToString("###,###.00");      //37

                            GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //35
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive"]).ToString("###,###.00");      //36

                            GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //37

                            GridView1.Rows[index].Cells["Col_1"].Value = "1";      //37

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
            GridView1_Cal_Sum();
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
                    cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                       "k016db_1supplier.*," +
                                       "c002_05Send_dye_record_detail.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +
                                       "c001_07number_color.*," +

                                       "k016db_1supplier.*," +
                                       "k013_1db_acc_13group_tax.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_05Send_dye_record" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " INNER JOIN c002_05Send_dye_record_detail" +
                                       " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                       " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                       " INNER JOIN c001_07number_color" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                           " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    //" AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                    " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                       " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";

                }
                else
                {
                    cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                       "k016db_1supplier.*," +
                                       "c002_05Send_dye_record_detail.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +
                                       "c001_07number_color.*," +

                                       "k016db_1supplier.*," +
                                       "k013_1db_acc_13group_tax.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_05Send_dye_record" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " INNER JOIN c002_05Send_dye_record_detail" +
                                       " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                       " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                       " INNER JOIN c001_07number_color" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                           " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                    " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";


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
                            this.GridView1.Rows[index].Cells["Col_txtPPT_id"].Value = dt2.Rows[j]["txtPPT_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();        //4
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //5


                            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");     //8

                            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //13

                            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //16

                            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //17


                            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //18
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //19
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //20
                            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //21

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //22
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //23
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //24

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //25
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //26
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //27

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //28
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //29

                            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //30

                            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = "";      //31
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //32

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub"]).ToString("###,###.00");      //21

                            //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
                            GridView1.Rows[index].Cells["Col_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //9

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rib"]).ToString("###,###.00");      //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";       //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";       //21

                            //GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = dt2.Rows[j]["txtqty_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = dt2.Rows[j]["txtqty_after_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();  //17

                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokma"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokpai"]).ToString("###,###.00");      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive_yokpai"]).ToString("###,###.00");      //37

                            GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //35
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive"]).ToString("###,###.00");      //36

                            GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //37
                            GridView1.Rows[index].Cells["Col_1"].Value = "1";      //37

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
            GridView1_Cal_Sum();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 53;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtco_id";
            this.GridView1.Columns[2].Name = "Col_txtbranch_id";
            this.GridView1.Columns[3].Name = "Col_txtPPT_id";
            this.GridView1.Columns[4].Name = "Col_txttrans_date_server";
            this.GridView1.Columns[5].Name = "Col_txttrans_time";
            this.GridView1.Columns[6].Name = "Col_txtsupplier_id";
            this.GridView1.Columns[7].Name = "Col_txtsupplier_name";
            this.GridView1.Columns[8].Name = "Col_txtemp_office_name";

            this.GridView1.Columns[9].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[10].Name = "Col_txtnumber_in_year";
            this.GridView1.Columns[11].Name = "Col_txtnumber_mat_id";
            this.GridView1.Columns[12].Name = "Col_txtnumber_color_id";
            this.GridView1.Columns[13].Name = "Col_txtface_baking_id";


            this.GridView1.Columns[14].Name = "Col_txtlot_no";
            this.GridView1.Columns[15].Name = "Col_txtfold_number";

            this.GridView1.Columns[16].Name = "Col_txtqty";

            this.GridView1.Columns[17].Name = "Col_txtmat_no";
            this.GridView1.Columns[18].Name = "Col_txtmat_id";
            this.GridView1.Columns[19].Name = "Col_txtmat_name";

            this.GridView1.Columns[20].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[21].Name = "Col_txtmat_unit1_qty";
            this.GridView1.Columns[22].Name = "Col_chmat_unit_status";
            this.GridView1.Columns[23].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[24].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[25].Name = "Col_txtqty2";

            this.GridView1.Columns[26].Name = "Col_txtprice";
            this.GridView1.Columns[27].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[28].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[29].Name = "Col_txtsum_total";

            this.GridView1.Columns[30].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[31].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[32].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[33].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[34].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[35].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[36].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns[37].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView1.Columns[38].Name = "Col_txtitem_no";

            this.GridView1.Columns[39].Name = "Col_txtqc_id";
            this.GridView1.Columns[40].Name = "Col_txtsum_qty_pub";
            this.GridView1.Columns[41].Name = "Col_date";
            this.GridView1.Columns[42].Name = "Col_qty_Cal";  //
            this.GridView1.Columns[43].Name = "Col_txtsum_qty_rib";
            this.GridView1.Columns[44].Name = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[45].Name = "Col_txtsum_qty_rib_kg";

            this.GridView1.Columns[46].Name = "Col_txtqty_receive_yokma";
            this.GridView1.Columns[47].Name = "Col_txtqty_receive_yokpai";
            this.GridView1.Columns[48].Name = "Col_txtqty_after_receive_yokpai";

            this.GridView1.Columns[49].Name = "Col_txtqty_receive";
            this.GridView1.Columns[50].Name = "Col_txtqty_after_receive";
            this.GridView1.Columns[51].Name = "Col_txtreceive_id";
            this.GridView1.Columns[52].Name = "Col_1";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "txtco_id";
            this.GridView1.Columns[2].HeaderText = " txtbranch_id";
            this.GridView1.Columns[3].HeaderText = " เลขที่";
            this.GridView1.Columns[4].HeaderText = " วันที่";
            this.GridView1.Columns[5].HeaderText = " เวลา";
            this.GridView1.Columns[6].HeaderText = " รหัส Supplier";
            this.GridView1.Columns[7].HeaderText = " ชื่อ Supplier";
            this.GridView1.Columns[8].HeaderText = " ผู้บันทึก";

            this.GridView1.Columns[9].HeaderText = "คลัง";
            this.GridView1.Columns[10].HeaderText = "ชุดที่";
            this.GridView1.Columns[11].HeaderText = "รหัสผ้า";
            this.GridView1.Columns[12].HeaderText = "รหัสสี";
            this.GridView1.Columns[13].HeaderText = "อบหน้า";


            this.GridView1.Columns[14].HeaderText = "Lot No";
            this.GridView1.Columns[15].HeaderText = "พับที่";

            this.GridView1.Columns[16].HeaderText = "ส่งย้อม (กก.)";

            this.GridView1.Columns[17].HeaderText = "ลำดับ";
            this.GridView1.Columns[18].HeaderText = "รหัส";
            this.GridView1.Columns[19].HeaderText = "ชื่อสินค้า";

            this.GridView1.Columns[20].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[21].HeaderText = " หน่วย";
            this.GridView1.Columns[22].HeaderText = "แปลง";
            this.GridView1.Columns[23].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[24].HeaderText = " หน่วย";

            this.GridView1.Columns[25].HeaderText = "ส่งย้อม(ปอนด์)";

            this.GridView1.Columns[26].HeaderText = "ราคา";
            this.GridView1.Columns[27].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[28].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[29].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[30].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[31].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[32].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[33].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[34].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[35].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[36].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView1.Columns[37].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView1.Columns[38].HeaderText = "item_no";
            this.GridView1.Columns[39].HeaderText = "txtqc_id";
            this.GridView1.Columns[40].HeaderText = "Col_txtsum_qty_pub";
            this.GridView1.Columns[41].HeaderText = " วันที่ต้องการ";
            this.GridView1.Columns[42].HeaderText = "Col_qty_Cal";
            this.GridView1.Columns[43].HeaderText = "Col_txtsum_qty_rib";
            this.GridView1.Columns[44].HeaderText = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[45].HeaderText = "Col_txtsum_qty_rib_kg";

            this.GridView1.Columns[46].HeaderText = "ส่งย้อมแล้วยกมา";
            this.GridView1.Columns[47].HeaderText = "ส่งย้อมแล้วยกไป";
            this.GridView1.Columns[48].HeaderText = "เหลือรอส่งย้อม อีก กก.";

            this.GridView1.Columns[49].HeaderText = "รับแล้ว";  //กก
            this.GridView1.Columns[50].HeaderText = "รอรับ";  //กก
            this.GridView1.Columns[51].HeaderText = "เลขที่รับคืนย้อม";  //
            this.GridView1.Columns[52].HeaderText = "1";  //

            this.GridView1.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView1.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.GridView1.Columns["Col_txtbranch_id"].Visible = false;  //""Col_txtbranch_id"";

            this.GridView1.Columns["Col_txtPPT_id"].Visible = true;  //"Col_txtPPT_id";
            this.GridView1.Columns["Col_txtPPT_id"].Width = 140;
            this.GridView1.Columns["Col_txtPPT_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtPPT_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtPPT_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txttrans_date_server"].Visible = true;  //""Col_txttrans_date_server"";
            this.GridView1.Columns["Col_txttrans_date_server"].Width = 90;
            this.GridView1.Columns["Col_txttrans_date_server"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_date_server"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_date_server"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txttrans_time"].Visible = true;  //"Col_txttrans_time";
            this.GridView1.Columns["Col_txttrans_time"].Width = 70;
            this.GridView1.Columns["Col_txttrans_time"].ReadOnly = true;
            this.GridView1.Columns["Col_txttrans_time"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txttrans_time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsupplier_id"].Visible = false;  //"Col_txtsupplier_id";

            this.GridView1.Columns["Col_txtsupplier_name"].Visible = true;  //"Col_txtsupplier_name";
            this.GridView1.Columns["Col_txtsupplier_name"].Width = 150;
            this.GridView1.Columns["Col_txtsupplier_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsupplier_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsupplier_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtemp_office_name"].Visible = true;  //"Col_txtemp_office_name";
            this.GridView1.Columns["Col_txtemp_office_name"].Width = 120;
            this.GridView1.Columns["Col_txtemp_office_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtemp_office_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtemp_office_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView1.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;



            this.GridView1.Columns["Col_txtnumber_in_year"].Visible = true;  //"Col_txtnumber_in_year";
            this.GridView1.Columns["Col_txtnumber_in_year"].Width = 80;
            this.GridView1.Columns["Col_txtnumber_in_year"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 0;
            this.GridView1.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_color_id"].Visible = true;  //"Col_txtnumber_color_id";
            this.GridView1.Columns["Col_txtnumber_color_id"].Width = 80;
            this.GridView1.Columns["Col_txtnumber_color_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_color_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_color_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView1.Columns["Col_txtface_baking_id"].Width = 80;
            this.GridView1.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            this.GridView1.Columns["Col_txtlot_no"].Visible = true;  //"Col_txtlot_no";
            this.GridView1.Columns["Col_txtlot_no"].Width = 180;
            this.GridView1.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView1.Columns["Col_txtfold_number"].Width = 60;
            this.GridView1.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView1.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns[8].Visible = false;
            DataGridViewCheckBoxColumn dgvCmb_SELECT = new DataGridViewCheckBoxColumn();
            dgvCmb_SELECT.Name = "Col_Chk_SELECT";
            dgvCmb_SELECT.Width = 0;  //70
            dgvCmb_SELECT.DisplayIndex = 8;
            dgvCmb_SELECT.HeaderText = "เลือกส่งย้อม";
            dgvCmb_SELECT.ValueType = typeof(bool);
            dgvCmb_SELECT.ReadOnly = false;
            dgvCmb_SELECT.Visible = false;
            dgvCmb_SELECT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb_SELECT);

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView1.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";
            this.GridView1.Columns["Col_txtmat_no"].Width = 0;
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


            this.GridView1.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.GridView1.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView1.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView1.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView1.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 0;  //70
            dgvCmb.DisplayIndex = 14;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.Visible = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb);

            this.GridView1.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.GridView1.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView1.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;




            this.GridView1.Columns["Col_txtqty2"].Visible = true;  //"Col_txtqty2";
            this.GridView1.Columns["Col_txtqty2"].Width = 110;
            this.GridView1.Columns["Col_txtqty2"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


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

            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].Visible = false;  //"Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].Visible = false;  //"Col_txtcost_qty2_balance_yokpai";
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_qty2_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtitem_no"].Visible = false;  //"Col_txtitem_no";
            this.GridView1.Columns["Col_txtitem_no"].Width = 0;
            this.GridView1.Columns["Col_txtitem_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtitem_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtitem_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtqc_id"].Visible = false;  //"Col_txtqc_id";
            //this.GridView1.Columns["Col_txtqc_id"].Width = 0;
            this.GridView1.Columns["Col_txtqc_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqc_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqc_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsum_qty_pub"].Visible = false;  //"Col_txtsum_qty_pub";
            this.GridView1.Columns["Col_txtsum_qty_pub"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_pub"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_date"].Visible = true;  //"Col_date";
            this.GridView1.Columns["Col_date"].Width = 150;
            this.GridView1.Columns["Col_date"].ReadOnly = false;
            this.GridView1.Columns["Col_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //this.GridView1.Columns[35].HeaderText = " Col_qty_Cal";
            this.GridView1.Columns["Col_qty_Cal"].Visible = false;  //"Col_qty_Cal";
            this.GridView1.Columns["Col_qty_Cal"].Width = 0;
            this.GridView1.Columns["Col_qty_Cal"].ReadOnly = false;
            this.GridView1.Columns["Col_qty_Cal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_qty_Cal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //this.GridView1.Columns[35].HeaderText = " Col_txtsum_qty_rib";
            this.GridView1.Columns["Col_txtsum_qty_rib"].Visible = false;  //"Col_txtsum_qty_rib";
            this.GridView1.Columns["Col_txtsum_qty_rib"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_rib"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].Visible = false;  //"Col_txtsum_qty_pub_kg";
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].Visible = false;  //"Col_txtsum_qty_rib_kg";
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_receive_yokma"].Visible = false;  //"Col_txtqty_receive_yokma";
            this.GridView1.Columns["Col_txtqty_receive_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtqty_receive_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_receive_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_receive_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_receive_yokpai"].Visible = false;  //"Col_txtqty_receive_yokpai";
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView1.Columns["Col_txtqty_after_receive_yokpai"].Visible = false;  //"Col_txtqty_after_receive_yokpai";
            this.GridView1.Columns["Col_txtqty_after_receive_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_after_receive_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_receive_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_receive_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_receive"].Visible = true;  //"Col_txtqty_receive";
            this.GridView1.Columns["Col_txtqty_receive"].Width = 100;
            this.GridView1.Columns["Col_txtqty_receive"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_receive"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_receive"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_after_receive"].Visible = true;  //"Col_txtqty_after_receive";
            this.GridView1.Columns["Col_txtqty_after_receive"].Width = 100;
            this.GridView1.Columns["Col_txtqty_after_receive"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_receive"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_receive"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtreceive_id"].Visible = true;  //"Col_txtreceive_id";
            this.GridView1.Columns["Col_txtreceive_id"].Width = 160;
            this.GridView1.Columns["Col_txtreceive_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtreceive_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtreceive_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_1"].Visible = false;  //"Col_1";
            this.GridView1.Columns["Col_1"].Width = 0;
            this.GridView1.Columns["Col_1"].ReadOnly = true;
            this.GridView1.Columns["Col_1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
                if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty_after_receive"].Value.ToString())) == 0)
                {
                    GridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
                if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty_after_receive"].Value.ToString())) > 0)
                {
                    GridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    GridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    GridView1.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8F);
                }
            }
        }
        private void GridView1_Cal_Sum()
        {
            double Sum_Qty = 0;
            double Sum_Qty2 = 0;
            double Sum2_Qty_Yokpai = 0;

            double Sum11 = 0;
            double Sum12 = 0;

            int k = 0;


            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {


                    if (this.GridView1.Rows[i].Cells["Col_txtqty_after_receive"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_after_receive"].Value = ".00";
                    }

                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_after_receive"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  จำนวนเบิก (กก)=================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_after_receive"].Value.ToString()));
                        this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                        if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                        {
                            Sum_Qty2 = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                            this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Sum_Qty2.ToString("N", new CultureInfo("en-US"));
                            //Sum2_Qty_Yokpai  =================================================
                            Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n}", Sum2_Qty_Yokpai)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                            this.txtsum2_qty.Text = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));
                        }

                        //
                        if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                        {
                            Sum11 = Convert.ToDouble(string.Format("{0:n}", Sum11)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                            this.txtsum_qty_rib.Text = Sum11.ToString("N", new CultureInfo("en-US"));
                        }
                        if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() != "RIB")
                        {
                            Sum12 = Convert.ToDouble(string.Format("{0:n}", Sum12)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                            this.txtsum_qty_roll.Text = Sum12.ToString("N", new CultureInfo("en-US"));
                        }
                        //======================================================
                    }


                }
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
                    W_ID_Select.TRANS_ID = row.Cells[3].Value.ToString();
                    this.cboSearch.Text = "เลขที่ใบส่งผ้าย้อม";

                    if (this.cboSearch.Text == "เลขที่ใบส่งผ้าย้อม")
                    {
                        this.txtsearch.Text = row.Cells[3].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells[3].Value.ToString();

                    }
                    else if (this.cboSearch.Text == "ชื่อผู้บันทึกใบส่งผ้าย้อม")
                    {
                        this.txtsearch.Text = row.Cells[8].Value.ToString();

                    }
                    else
                    {
                        this.txtsearch.Text = row.Cells[3].Value.ToString();
                        W_ID_Select.TRANS_ID = row.Cells[3].Value.ToString();

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
                W_ID_Select.WORD_TOP = "ดูข้อมูลใบส่งผ้าย้อม";
                kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record_detail frm2 = new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record_detail();
                frm2.Show();

                TRANS_LOG();

            }
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

                W_ID_Select.WORD_TOP = "บันทึกใบส่งผ้าย้อม";
                kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record frm2 = new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record();
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
                W_ID_Select.WORD_TOP = "ดูข้อมูลใบส่งผ้าย้อม";
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
                if (this.ch_all_branch.Checked == true)
                {
                    cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                       "k016db_1supplier.*," +
                                       "c002_05Send_dye_record_detail.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +
                                       "c001_07number_color.*," +

                                       "k016db_1supplier.*," +
                                       "k013_1db_acc_13group_tax.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_05Send_dye_record" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " INNER JOIN c002_05Send_dye_record_detail" +
                                       " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                       " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                       " INNER JOIN c001_07number_color" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                           " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    //" AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                    " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                       " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";

                }
                if (this.ch_all_branch.Checked == false)
                {
                    cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                       "k016db_1supplier.*," +
                                       "c002_05Send_dye_record_detail.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +
                                       "c001_07number_color.*," +

                                       "k016db_1supplier.*," +
                                       "k013_1db_acc_13group_tax.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_05Send_dye_record" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " INNER JOIN c002_05Send_dye_record_detail" +
                                       " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                       " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                       " INNER JOIN c001_07number_color" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                           " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                     " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                      " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";


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
                            this.GridView1.Rows[index].Cells["Col_txtPPT_id"].Value = dt2.Rows[j]["txtPPT_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();        //4
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //5


                            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");     //8

                            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //13

                            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //16

                            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //17


                            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //18
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //19
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //20
                            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //21

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //22
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //23
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //24

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //25
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //26
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //27

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //28
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //29

                            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //30

                            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = "";      //31
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //32

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub"]).ToString("###,###.00");      //21

                            //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
                            GridView1.Rows[index].Cells["Col_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //9

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rib"]).ToString("###,###.00");      //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";       //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";       //21

                            //GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = dt2.Rows[j]["txtqty_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = dt2.Rows[j]["txtqty_after_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();  //17

                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokma"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokpai"]).ToString("###,###.00");      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive_yokpai"]).ToString("###,###.00");      //37

                            GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //35
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive"]).ToString("###,###.00");      //36

                            GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //37
                            GridView1.Rows[index].Cells["Col_1"].Value = "1";      //37

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
            GridView1_Cal_Sum();

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

                cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                   "k016db_1supplier.*," +
                                   "c002_05Send_dye_record_detail.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +
                                   "c001_07number_color.*," +

                                   "k016db_1supplier.*," +
                                   "k013_1db_acc_13group_tax.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_05Send_dye_record" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN c002_05Send_dye_record_detail" +
                                   " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                   " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                   " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                   " INNER JOIN c001_07number_color" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                      " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                 " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                  " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";

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
                            this.GridView1.Rows[index].Cells["Col_txtPPT_id"].Value = dt2.Rows[j]["txtPPT_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();        //4
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //5


                            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");     //8

                            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //13

                            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //16

                            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //17


                            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //18
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //19
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //20
                            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //21

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //22
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //23
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //24

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //25
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //26
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //27

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //28
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //29

                            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //30

                            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = "";      //31
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //32

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub"]).ToString("###,###.00");      //21

                            //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
                            GridView1.Rows[index].Cells["Col_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //9

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rib"]).ToString("###,###.00");      //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";       //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";       //21

                            //GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = dt2.Rows[j]["txtqty_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = dt2.Rows[j]["txtqty_after_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();  //17

                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokma"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokpai"]).ToString("###,###.00");      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive_yokpai"]).ToString("###,###.00");      //37

                            GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //35
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive"]).ToString("###,###.00");      //36

                            GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //37
                            GridView1.Rows[index].Cells["Col_1"].Value = "1";      //37

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
            GridView1_Cal_Sum();

        }

        private void btnGo4_Click(object sender, EventArgs e)
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

                //this.cboSearch.Items.Add("เลขที่ใบส่งผ้าย้อม");
                //this.cboSearch.Items.Add("ชื่อผู้บันทึกใบส่งผ้าย้อม");
                if (this.cboSearch.Text == "เลขที่ใบส่งผ้าย้อม")
                {
                    cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                       "k016db_1supplier.*," +
                                       "c002_05Send_dye_record_detail.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +
                                       "c001_07number_color.*," +

                                       "k016db_1supplier.*," +
                                       "k013_1db_acc_13group_tax.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_05Send_dye_record" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " INNER JOIN c002_05Send_dye_record_detail" +
                                       " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                       " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                       " INNER JOIN c001_07number_color" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                           " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    //" AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                    //" AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                    " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                        " AND (c002_05Send_dye_record.txtPPT_id = '" + this.txtsearch.Text.Trim() + "')" +
                                        " ORDER BY c002_05Send_dye_record.txtPPT_id,c002_05Send_dye_record_detail.txtnumber_in_year,c002_05Send_dye_record_detail.txtLot_no ASC";

                }
                if (this.cboSearch.Text == "ชื่อผู้บันทึกใบส่งผ้าย้อม")
                {
                    cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                       "k016db_1supplier.*," +
                                       "c002_05Send_dye_record_detail.*," +
                                       "c001_05face_baking.*," +
                                       //"c001_06number_mat.*," +
                                       "c001_07number_color.*," +

                                       "k016db_1supplier.*," +
                                       "k013_1db_acc_13group_tax.*," +

                                       "k013_1db_acc_06wherehouse.*" +

                                       " FROM c002_05Send_dye_record" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       " INNER JOIN c002_05Send_dye_record_detail" +
                                       " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                       " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                       " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                       " INNER JOIN c001_05face_baking" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                       //" INNER JOIN c001_06number_mat" +
                                       //" ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                       //" AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                       //" AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                       " INNER JOIN c001_07number_color" +
                                       " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c002_05Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c002_05Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c002_05Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                           " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                      //" AND (c002_05Send_dye_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                      " AND (c002_05Send_dye_record.txtPPT_status = '0')" +
                                      " AND (c002_05Send_dye_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                       " AND (c002_05Send_dye_record.txtemp_office_name LIKE '%" + this.txtsearch.Text.Trim() + "%')" +
                                      " ORDER BY c002_05Send_dye_record.txtPPT_id ASC";

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
                            this.GridView1.Rows[index].Cells["Col_txtPPT_id"].Value = dt2.Rows[j]["txtPPT_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txttrans_date_server"].Value = Convert.ToDateTime(dt2.Rows[j]["txttrans_date_server"]).ToString("dd-MM-yyyy", UsaCulture);     //4
                            this.GridView1.Rows[index].Cells["Col_txttrans_time"].Value = dt2.Rows[j]["txttrans_time"].ToString();      //5

                            this.GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //7
                            this.GridView1.Rows[index].Cells["Col_txtemp_office_name"].Value = dt2.Rows[j]["txtemp_office_name"].ToString();      //8

                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();        //4
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //5


                            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");     //8

                            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //9
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //10
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //11

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //13

                            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //16

                            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //17


                            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //18
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //19
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //20
                            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //21

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //22
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //23
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //24

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //25
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //26
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //27

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //28
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //29

                            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //30

                            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = "";      //31
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //32

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub"]).ToString("###,###.00");      //21

                            //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
                            GridView1.Rows[index].Cells["Col_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //9

                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rib"]).ToString("###,###.00");      //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";       //21
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";       //21

                            //GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = dt2.Rows[j]["txtqty_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = dt2.Rows[j]["txtqty_after_receive"].ToString();  //17
                            //GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();  //17

                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokma"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive_yokpai"]).ToString("###,###.00");      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive_yokpai"]).ToString("###,###.00");      //37

                            GridView1.Rows[index].Cells["Col_txtqty_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_receive"]).ToString("###,###.00");      //35
                            GridView1.Rows[index].Cells["Col_txtqty_after_receive"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_receive"]).ToString("###,###.00");      //36

                            GridView1.Rows[index].Cells["Col_txtreceive_id"].Value = dt2.Rows[j]["txtreceive_id"].ToString();      //37
                            GridView1.Rows[index].Cells["Col_1"].Value = "1";      //37

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
            GridView1_Cal_Sum();

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
        private void FillDATE_FROM_SERVER()
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

                    cmd2.CommandText = "UPDATE A001_date_now SET " +
                                                                 "datetime_now = GETDATE()";
                    cmd2.ExecuteNonQuery();



                    Cursor.Current = Cursors.WaitCursor;
                    trans.Commit();
                    conn.Close();
                    Cursor.Current = Cursors.Default;

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




            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT datetime_now" +
                                  " FROM A001_date_now";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //this.txtdate_from_server.Text = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                        //this.txttime_from_server.Text = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("HH:mm:ss", ThaiCulture);          //4

                        string D1 = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("yyyy-MM-dd", ThaiCulture);          //4
                        string T1 = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("HH:mm:ss", ThaiCulture);          //4
                        W_ID_Select.DATE_FROM_SERVER = D1.ToString();
                        W_ID_Select.TIME_FROM_SERVER = T1.ToString();

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

            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================
            conn.Close();
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

        //====================================================================
    }
}
