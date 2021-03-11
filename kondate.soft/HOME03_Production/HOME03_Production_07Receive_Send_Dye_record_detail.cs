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
    public partial class HOME03_Production_07Receive_Send_Dye_record_detail : Form
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

        public HOME03_Production_07Receive_Send_Dye_record_detail()
        {
            InitializeComponent();
        }

        private void HOME03_Production_07Receive_Send_Dye_record_detail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0307RGDL";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิด";
            TRANS_LOG();

            this.iblword_status.Text = "ดูข้อมูลใบรับผ้าย้อม";

            this.ActiveControl = this.txtrg_remark;
            this.BtnNew.Enabled = false;
            this.btnopen.Enabled = false;
            this.BtnSave.Enabled = false;
            this.BtnCancel_Doc.Enabled = true;
            this.btnPreview.Enabled = true;
            this.BtnPrint.Enabled = true;

            Show_GridView1();
            Fill_DATA_TO_GridView1();
            GridView1_Up_Status();

            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();
            GridView1_Color_Column();

        }

        private void Fill_DATA_TO_GridView1()
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
            Clear_GridView1();
            //===========================================
            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c002_07Receive_Send_dye_record.*," +
                                   "c002_07Receive_Send_dye_record_detail.*," +
                                   "c001_05face_baking.*," +
                                   "c001_06number_mat.*," +
                                   "c001_07number_color.*," +

                                   "k016db_1supplier.*," +
                                   "k013_1db_acc_13group_tax.*," +

                                   "k013_1db_acc_06wherehouse.*" +
                                  //"k013_1db_acc_16department.*" +

                                   " FROM c002_07Receive_Send_dye_record" +

                                   " INNER JOIN c002_07Receive_Send_dye_record_detail" +
                                   " ON c002_07Receive_Send_dye_record.cdkey = c002_07Receive_Send_dye_record_detail.cdkey" +
                                   " AND c002_07Receive_Send_dye_record.txtco_id = c002_07Receive_Send_dye_record_detail.txtco_id" +
                                   " AND c002_07Receive_Send_dye_record.txtRG_id = c002_07Receive_Send_dye_record_detail.txtRG_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_07Receive_Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   " INNER JOIN c001_06number_mat" +
                                   " ON c002_07Receive_Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                   " INNER JOIN c001_07number_color" +
                                   " ON c002_07Receive_Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +


                                   " INNER JOIN k016db_1supplier" +
                                   " ON c002_07Receive_Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND c002_07Receive_Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND c002_07Receive_Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_07Receive_Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_07Receive_Send_dye_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_07Receive_Send_dye_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                   //" INNER JOIN k013_1db_acc_16department" +
                                   //" ON c002_07Receive_Send_dye_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   //" AND c002_07Receive_Send_dye_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   //" AND c002_07Receive_Send_dye_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +


                                   " WHERE (c002_07Receive_Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_07Receive_Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_07Receive_Send_dye_record.txtRG_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                   " ORDER BY c002_07Receive_Send_dye_record_detail.txtnumber_in_year,c002_07Receive_Send_dye_record_detail.txtfold_number ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.txtRG_id.Text = dt2.Rows[0]["txtRG_id"].ToString();

                        this.txtnumber_dyed.Text = dt2.Rows[0]["txtnumber_dyed"].ToString();
                        this.txtreceive_send_dye_type_id.Text = dt2.Rows[0]["txtreceive_send_dye_type_id"].ToString();

                        if (this.txtreceive_send_dye_type_id.Text == "01")
                        {
                            this.cbotxtreceive_send_dye_type_name.Text = "รับตามใบส่งย้อม";
                        }
                        else
                        {
                            this.cbotxtreceive_send_dye_type_name.Text = "รับไม่มีใบส่งย้อม";

                        }
                        this.txtPPT_id.Text = dt2.Rows[0]["txtPPT_id"].ToString();

                        this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                        this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_server"].ToString());
                        this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                        this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

                        this.txtrg_remark.Text = dt2.Rows[0]["txtrg_remark"].ToString();

                        this.Paneldate_txtcurrency_date.Text = dt2.Rows[0]["txtcurrency_date"].ToString();
                        this.txtcurrency_id.Text = dt2.Rows[0]["txtcurrency_id"].ToString();
                        this.txtcurrency_rate.Text = dt2.Rows[0]["txtcurrency_rate"].ToString();

                        this.txtemp_office_name.Text = dt2.Rows[0]["txtemp_office_name"].ToString();
                        this.txtemp_office_name_receive.Text = dt2.Rows[0]["txtemp_office_name_receive"].ToString();
                        this.txtemp_office_name_audit.Text = dt2.Rows[0]["txtemp_office_name_audit"].ToString();
                        this.txtemp_office_name_send.Text = dt2.Rows[0]["txtemp_office_name_send"].ToString();


                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");

                        this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                        this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();


                        this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                        this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();
                        //this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();


                        this.PANEL1307_PROJECT_txtproject_id.Text = dt2.Rows[0]["txtproject_id"].ToString();
                        //this.PANEL1307_PROJECT_txtproject_name.Text = dt2.Rows[0]["txtproject_name"].ToString();

                        this.PANEL1317_JOB_txtjob_id.Text = dt2.Rows[0]["txtjob_id"].ToString();
                        //this.PANEL1317_JOB_txtjob_name.Text = dt2.Rows[0]["txtjob_name"].ToString();

                        this.txtVat_id.Text = dt2.Rows[0]["txtVat_id"].ToString();
                        this.txtVat_date.Text = dt2.Rows[0]["txtVat_date"].ToString();
                        this.PANEL003_EMP_txtemp_id.Text = dt2.Rows[0]["txtemp_id"].ToString();
                        this.PANEL003_EMP_txtemp_name.Text = dt2.Rows[0]["txtemp_name"].ToString();

                        this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                        this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                        this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                        this.txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();

                        this.txtmat_unit1_name.Text = dt2.Rows[0]["txtmat_unit1_name"].ToString();
                        this.txtmat_unit1_qty.Text = dt2.Rows[0]["txtmat_unit1_qty"].ToString();
                        this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                        this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                        this.txtmat_unit2_qty.Text = dt2.Rows[0]["txtmat_unit2_qty"].ToString();

                        //this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                        //this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();


                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = dt2.Rows[0]["txtnumber_mat_id"].ToString();
                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[0]["txtnumber_mat_name"].ToString();

                        //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = "ซื้อไม่มีvat";
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = "PUR_ONvat";

                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");


                        this.txtsum_qty_pub_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_pub_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร
                        this.txtsum_qty_pub_receive_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_pub_receive_kg"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร

                        this.txtsum_qty_pub_yokma.Text = dt2.Rows[0]["txtsum_qty_pub_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร
                        this.txtsum_qty_pub_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_pub_balance_kg"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร

                        this.txtsum_qty_rib_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_rib_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร
                        this.txtsum_qty_rib_receive_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_rib_receive_kg"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร

                        this.txtsum_qty_rib_yokma.Text = dt2.Rows[0]["txtsum_qty_rib_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร
                        this.txtsum_qty_rib_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_rib_balance_kg"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร

                        this.txtsum_qty_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร
                        this.txtsum_qty_yokma.Text = dt2.Rows[0]["txtsum_qty_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร

                        //MessageBox.Show("ok1");
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            var index = this.GridView1.Rows.Add();
                            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //3
                            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();      //4
                            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //5


                            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //6
                            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //7

                            this.GridView1.Rows[index].Cells["Col_txtqty_want"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");     //8
                            this.GridView1.Rows[index].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");     //9
                            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";    //11

                            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //12
                            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //13
                            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //14

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //15
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //16

                            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //17

                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //18
                            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //19

                            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //20


                            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //21
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //22
                            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //23
                            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //24

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //25
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //26
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //27

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //28
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //29
                            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //30

                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //31
                            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //32

                            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //33

                            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = dt2.Rows[j]["txtqc_id"].ToString();      //34


                            this.GridView1.Rows[index].Cells["Col_txtqty_want_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want_pub"]).ToString("###,###.00");      //35
                            this.GridView1.Rows[index].Cells["Col_txtqty_balance_pub"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_pub"]).ToString("###,###.00");     //36
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";   //37

                            this.GridView1.Rows[index].Cells["Col_txtqty_want_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want_rib"]).ToString("###,###.00");      //38
                            this.GridView1.Rows[index].Cells["Col_txtqty_balance_rib"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_rib"]).ToString("###,###.00");     //39
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = "0";   //40


                            this.GridView1.Rows[index].Cells["Col_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();     //41
                            this.GridView1.Rows[index].Cells["Col_mat_status"].Value = "0";     //42

                            this.GridView1.Rows[index].Cells["Col_txtqty_balance_yokpai"].Value = "0";     //43
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_yokpai"].Value = "0";     //44
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_yokpai"].Value = "0";     //45
                            this.GridView1.Rows[index].Cells["Col_qty_Cal"].Value = "0";     //46
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";     //47
                            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";     //48
                            this.GridView1.Rows[index].Cells["Col_txtqty_berg_cut_shirt_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_berg_cut_shirt_balance"]).ToString("###,###.00");     //49

                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_cut_yokma"]).ToString("###,###.00");      //36
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_cut_yokpai"]).ToString("###,###.00");      //37
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_cut_yokpai"]).ToString("###,###.00");      //37

                            GridView1.Rows[index].Cells["Col_txtqty_cut"].Value = dt2.Rows[j]["txtqty_cut"].ToString();  //17
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = dt2.Rows[j]["txtqty_after_cut"].ToString();  //17
                            GridView1.Rows[index].Cells["Col_txtcut_id"].Value = dt2.Rows[j]["txtcut_id"].ToString();  //17


                        }

                        //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9


                    }
                    //=======================================================

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
            SHOW_YOKMA1();
            GridView1_Up_Status();
            GridView1_Color_Column();
            Show_Qty_Yokma();
            //GridView1_Cal_Sum();
            GridView1_Cal_Sum_For_Cancel();

            //Fill_cboemp();

        }
        private void SHOW_YOKMA1()
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


                cmd2.CommandText = "SELECT c002_05Send_dye_record.*," +
                                   "c002_05Send_dye_record_detail.*," +

                                   "b001mat_02detail.*," +
                                   //"k021_mat_average.*," +

                                   //"k013_1db_acc_16department.*," +
                                   //"k013_1db_acc_07project.*," +
                                   //"k013_1db_acc_17job.*," +
                                   "c001_05face_baking.*," +
                                   "c001_06number_mat.*," +
                                   "c001_07number_color.*," +

                                   "k016db_1supplier.*," +
                                   "b001_05mat_unit1.*," +
                                   "b001_05mat_unit2.*," +
                                   "k013_1db_acc_13group_tax.*" +

                                   " FROM c002_05Send_dye_record" +

                                   " INNER JOIN c002_05Send_dye_record_detail" +
                                   " ON c002_05Send_dye_record.cdkey = c002_05Send_dye_record_detail.cdkey" +
                                   " AND c002_05Send_dye_record.txtco_id = c002_05Send_dye_record_detail.txtco_id" +
                                   " AND c002_05Send_dye_record.txtPPT_id = c002_05Send_dye_record_detail.txtPPT_id" +

                                   " INNER JOIN b001mat_02detail" +
                                   " ON c002_05Send_dye_record_detail.cdkey = b001mat_02detail.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = b001mat_02detail.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtmat_id = b001mat_02detail.txtmat_id" +

                                   //" INNER JOIN k021_mat_average" +
                                   //" ON c002_05Send_dye_record_detail.cdkey = k021_mat_average.cdkey" +
                                   //" AND c002_05Send_dye_record_detail.txtco_id = k021_mat_average.txtco_id" +
                                   //" AND c002_05Send_dye_record_detail.txtmat_id = k021_mat_average.txtmat_id" +

                                   //" INNER JOIN k013_1db_acc_16department" +
                                   //" ON c002_05Send_dye_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   //" AND c002_05Send_dye_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   //" AND c002_05Send_dye_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   //" INNER JOIN k013_1db_acc_07project" +
                                   //" ON c002_05Send_dye_record.cdkey = k013_1db_acc_07project.cdkey" +
                                   //" AND c002_05Send_dye_record.txtco_id = k013_1db_acc_07project.txtco_id" +
                                   //" AND c002_05Send_dye_record.txtproject_id = k013_1db_acc_07project.txtproject_id" +

                                   //" INNER JOIN k013_1db_acc_17job" +
                                   //" ON c002_05Send_dye_record.cdkey = k013_1db_acc_17job.cdkey" +
                                   //" AND c002_05Send_dye_record.txtco_id = k013_1db_acc_17job.txtco_id" +
                                   //" AND c002_05Send_dye_record.txtjob_id = k013_1db_acc_17job.txtjob_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   " INNER JOIN c001_06number_mat" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_06number_mat.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_06number_mat.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +

                                   " INNER JOIN c001_07number_color" +
                                   " ON c002_05Send_dye_record_detail.cdkey = c001_07number_color.cdkey" +
                                   " AND c002_05Send_dye_record_detail.txtco_id = c001_07number_color.txtco_id" +
                                   " AND c002_05Send_dye_record_detail.txtnumber_color_id = c001_07number_color.txtnumber_color_id" +


                                   " INNER JOIN k016db_1supplier" +
                                   " ON c002_05Send_dye_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND c002_05Send_dye_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND c002_05Send_dye_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN b001_05mat_unit1" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                   " INNER JOIN b001_05mat_unit2" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +


                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_05Send_dye_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " WHERE (c002_05Send_dye_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c002_05Send_dye_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c002_05Send_dye_record.txtPPT_id = '" + this.txtPPT_id.Text.Trim() + "')" +
                                   //" AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                   " ORDER BY c002_05Send_dye_record_detail.txtmat_no ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {


                        this.txtsum_qty_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร
                        this.txtsum_qty_pub_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_pub_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร
                        this.txtsum_qty_rib_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_rib_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร

                        this.txtsum_qty_pub_receive_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_pub_receive_kg"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร
                        this.txtsum_qty_rib_receive_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_rib_receive_kg"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร


                        this.txtsum_qty_yokma.Text = dt2.Rows[0]["txtsum_qty_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร
                        this.txtsum_qty_pub_yokma.Text = dt2.Rows[0]["txtsum_qty_pub_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร
                        this.txtsum_qty_rib_yokma.Text = dt2.Rows[0]["txtsum_qty_rib_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร

                        this.txtsum_qty_pub_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_pub_balance_kg"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร
                        this.txtsum_qty_rib_yokma_kg.Text = dt2.Rows[0]["txtsum_qty_rib_balance_kg"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร

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
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 56;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[2].Name = "Col_txtnumber_in_year";
            this.GridView1.Columns[3].Name = "Col_txtnumber_mat_id";
            this.GridView1.Columns[4].Name = "Col_txtnumber_color_id";
            this.GridView1.Columns[5].Name = "Col_txtface_baking_id";


            this.GridView1.Columns[6].Name = "Col_txtlot_no";
            this.GridView1.Columns[7].Name = "Col_txtfold_number";

            this.GridView1.Columns[8].Name = "Col_txtqty_want";
            this.GridView1.Columns[9].Name = "Col_txtqty_balance";
            this.GridView1.Columns[11].Name = "Col_txtqty";

            this.GridView1.Columns[12].Name = "Col_txtmat_no";
            this.GridView1.Columns[13].Name = "Col_txtmat_id";
            this.GridView1.Columns[14].Name = "Col_txtmat_name";

            this.GridView1.Columns[15].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[16].Name = "Col_txtmat_unit1_qty";
            this.GridView1.Columns[17].Name = "Col_chmat_unit_status";
            this.GridView1.Columns[18].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[19].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[20].Name = "Col_txtqty2";

            this.GridView1.Columns[21].Name = "Col_txtprice";
            this.GridView1.Columns[22].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[23].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[24].Name = "Col_txtsum_total";

            this.GridView1.Columns[25].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[26].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[27].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[28].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[29].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[30].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[31].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns[32].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView1.Columns[33].Name = "Col_txtitem_no";

            this.GridView1.Columns[34].Name = "Col_txtqc_id";

            this.GridView1.Columns[35].Name = "Col_txtqty_want_pub";
            this.GridView1.Columns[36].Name = "Col_txtqty_balance_pub";
            this.GridView1.Columns[37].Name = "Col_txtsum_qty_pub";

            this.GridView1.Columns[38].Name = "Col_txtqty_want_rib";
            this.GridView1.Columns[39].Name = "Col_txtqty_balance_rib";
            this.GridView1.Columns[40].Name = "Col_txtsum_qty_rib";

            this.GridView1.Columns[41].Name = "Col_date";
            //Col_mat_status
            this.GridView1.Columns[42].Name = "Col_mat_status";

            this.GridView1.Columns[43].Name = "Col_txtqty_balance_yokpai";
            this.GridView1.Columns[44].Name = "Col_txtsum_qty_pub_yokpai";
            this.GridView1.Columns[45].Name = "Col_txtsum_qty_rib_yokpai";

            this.GridView1.Columns[46].Name = "Col_qty_Cal";  //
            this.GridView1.Columns[47].Name = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[48].Name = "Col_txtsum_qty_rib_kg";
            this.GridView1.Columns[49].Name = "Col_txtqty_berg_cut_shirt_balance";

            this.GridView1.Columns[50].Name = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[51].Name = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[52].Name = "Col_txtqty_after_cut_yokpai";


            this.GridView1.Columns[53].Name = "Col_txtqty_cut";
            this.GridView1.Columns[54].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[55].Name = "Col_txtcut_id";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "คลัง";
            this.GridView1.Columns[2].HeaderText = "ชุดที่";
            this.GridView1.Columns[3].HeaderText = "รหัสผ้า";
            this.GridView1.Columns[4].HeaderText = "รหัสสี";
            this.GridView1.Columns[5].HeaderText = "อบหน้า";


            this.GridView1.Columns[6].HeaderText = "Lot No";
            this.GridView1.Columns[7].HeaderText = "พับที่";

            this.GridView1.Columns[8].HeaderText = "ส่งย้อม (กก.)";
            this.GridView1.Columns[9].HeaderText = "ส่งย้อมค้างรับ (กก.)";
            this.GridView1.Columns[11].HeaderText = "รับ (กก.)";

            this.GridView1.Columns[12].HeaderText = "ลำดับ";
            this.GridView1.Columns[13].HeaderText = "รหัส";
            this.GridView1.Columns[14].HeaderText = "ชื่อสินค้า";

            this.GridView1.Columns[15].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[16].HeaderText = " หน่วย";
            this.GridView1.Columns[17].HeaderText = "แปลง";
            this.GridView1.Columns[18].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[19].HeaderText = " หน่วย";

            this.GridView1.Columns[20].HeaderText = "รับ(ปอนด์)";

            this.GridView1.Columns[21].HeaderText = "ราคา";
            this.GridView1.Columns[22].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[23].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[24].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[25].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[26].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[27].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[28].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[29].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[30].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[31].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView1.Columns[32].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView1.Columns[33].HeaderText = "item_no";
            this.GridView1.Columns[34].HeaderText = "txtqc_id";

            this.GridView1.Columns[35].HeaderText = "Col_txtqty_want_pub";
            this.GridView1.Columns[36].HeaderText = "Col_txtqty_balance_pub";
            this.GridView1.Columns[37].HeaderText = "Col_txtsum_qty_pub";

            this.GridView1.Columns[38].HeaderText = "Col_txtqty_want_rib";
            this.GridView1.Columns[39].HeaderText = "Col_txtqty_balance_rib";
            this.GridView1.Columns[40].HeaderText = "Col_txtsum_qty_rib";

            this.GridView1.Columns[41].HeaderText = " วันที่ต้องการ";
            this.GridView1.Columns[42].HeaderText = " Col_mat_status";

            this.GridView1.Columns[43].HeaderText = "Col_txtqty_balance_yokpai";
            this.GridView1.Columns[44].HeaderText = "Col_txtsum_qty_pub_yokpai";
            this.GridView1.Columns[45].HeaderText = "Col_txtsum_qty_rib_yokpai";

            this.GridView1.Columns[46].HeaderText = "Col_qty_Cal";
            this.GridView1.Columns[47].HeaderText = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[48].HeaderText = "Col_txtsum_qty_rib_kg";
            this.GridView1.Columns[49].HeaderText = "รอสั่งตัด";  //Col_txtqty_berg_cut_shirt_balance

            this.GridView1.Columns[50].HeaderText = "จำนวนส่งตัด";  //กก
            this.GridView1.Columns[51].HeaderText = "จำนวนเหลือ";  //กก
            this.GridView1.Columns[52].HeaderText = "เลขที่ส่งตัด";  //

            this.GridView1.Columns[53].HeaderText = "จำนวนส่งตัด";  //กก
            this.GridView1.Columns[54].HeaderText = "จำนวนเหลือ";  //กก
            this.GridView1.Columns[55].HeaderText = "เลขที่ส่งตัด";  //

            this.GridView1.Columns["Col_Auto_num"].Visible = true;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 60;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = true;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 80;
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
            this.GridView1.Columns["Col_txtlot_no"].Width = 160;
            this.GridView1.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView1.Columns["Col_txtfold_number"].Width = 60;
            this.GridView1.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView1.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtqty_want"].Visible = true;  //"Col_txtqty_want";
            this.GridView1.Columns["Col_txtqty_want"].Width = 100;
            this.GridView1.Columns["Col_txtqty_want"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_want"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_want"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_balance"].Visible = true;  //"Col_txtqty_balance";
            this.GridView1.Columns["Col_txtqty_balance"].Width = 140;
            this.GridView1.Columns["Col_txtqty_balance"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns[10].Visible = false;
            DataGridViewCheckBoxColumn dgvCmb_SELECT = new DataGridViewCheckBoxColumn();
            dgvCmb_SELECT.Name = "Col_Chk_SELECT";
            dgvCmb_SELECT.Width = 120;  //70
            dgvCmb_SELECT.DisplayIndex = 10;
            dgvCmb_SELECT.HeaderText = "เลือกรับ ย้อมเสร็จ";
            dgvCmb_SELECT.ValueType = typeof(bool);
            dgvCmb_SELECT.ReadOnly = false;
            dgvCmb_SELECT.Visible = true;
            dgvCmb_SELECT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            GridView1.Columns.Add(dgvCmb_SELECT);

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


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

            this.GridView1.Columns["Col_txtqty_want_pub"].Visible = false;  //"Col_txtqty_want_pub";
            this.GridView1.Columns["Col_txtqty_want_pub"].Width = 0;
            this.GridView1.Columns["Col_txtqty_want_pub"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_want_pub"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_want_pub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty_balance_pub"].Visible = false;  //"Col_txtqty_balance_pub";
            this.GridView1.Columns["Col_txtqty_balance_pub"].Width = 0;
            this.GridView1.Columns["Col_txtqty_balance_pub"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_balance_pub"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_balance_pub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsum_qty_pub"].Visible = false;  //"Col_txtsum_qty_pub";
            this.GridView1.Columns["Col_txtsum_qty_pub"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_pub"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtqty_want_rib"].Visible = false;  //"Col_txtqty_want_rib";
            this.GridView1.Columns["Col_txtqty_want_rib"].Width = 0;
            this.GridView1.Columns["Col_txtqty_want_rib"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_want_rib"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_want_rib"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty_balance_rib"].Visible = false;  //"Col_txtqty_balance_rib";
            this.GridView1.Columns["Col_txtqty_balance_rib"].Width = 0;
            this.GridView1.Columns["Col_txtqty_balance_rib"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_balance_rib"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_balance_rib"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsum_qty_rib"].Visible = false;  //"Col_txtsum_qty_rib";
            this.GridView1.Columns["Col_txtsum_qty_rib"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_rib"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_date"].Visible = false;  //"Col_date";
            this.GridView1.Columns["Col_date"].Width = 0;
            this.GridView1.Columns["Col_date"].ReadOnly = false;
            this.GridView1.Columns["Col_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Col_mat_status
            this.GridView1.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";
            this.GridView1.Columns["Col_mat_status"].Width = 0;
            this.GridView1.Columns["Col_mat_status"].ReadOnly = false;
            this.GridView1.Columns["Col_mat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_mat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //this.GridView1.Columns[40].HeaderText = "Col_txtqty_balance_yokpai";
            //this.GridView1.Columns[41].HeaderText = "Col_txtsum_qty_pub_yokpai";

            this.GridView1.Columns["Col_txtqty_balance_yokpai"].Visible = false;  //"Col_txtqty_balance_yokpai";
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtsum_qty_pub_yokpai"].Visible = false;  //"Col_txtsum_qty_pub_yokpai";
            this.GridView1.Columns["Col_txtsum_qty_pub_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub_yokpai"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_pub_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtsum_qty_rib_yokpai"].Visible = false;  //"Col_txtsum_qty_rib_yokpai";
            this.GridView1.Columns["Col_txtsum_qty_rib_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib_yokpai"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_rib_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Col_qty_Cal
            this.GridView1.Columns["Col_qty_Cal"].Visible = false;  //"Col_qty_Cal";
            this.GridView1.Columns["Col_qty_Cal"].Width = 0;
            this.GridView1.Columns["Col_qty_Cal"].ReadOnly = false;
            this.GridView1.Columns["Col_qty_Cal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_qty_Cal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].Visible = false;  //"Col_txtsum_qty_pub_kg";
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].Visible = false;  //"Col_txtsum_qty_rib_kg";
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtqty_berg_cut_shirt_balance"].Visible = true;  //"Col_txtqty_berg_cut_shirt_balance";
            this.GridView1.Columns["Col_txtqty_berg_cut_shirt_balance"].Width = 100;
            this.GridView1.Columns["Col_txtqty_berg_cut_shirt_balance"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty_berg_cut_shirt_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_berg_cut_shirt_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtqty_cut_yokma"].Visible = false;  //"Col_txtqty_cut_yokma";
            this.GridView1.Columns["Col_txtqty_cut_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtqty_cut_yokpai"].Visible = false;  //"Col_txtqty_cut_yokpai";
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].Visible = false;  //"Col_txtqty_after_cut_yokpai";
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtqty_cut"].Visible = true;  //"Col_txtqty_cut";
            this.GridView1.Columns["Col_txtqty_cut"].Width = 140;
            this.GridView1.Columns["Col_txtqty_cut"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_after_cut"].Visible = true;  //"Col_txtqty_after_cut";
            this.GridView1.Columns["Col_txtqty_after_cut"].Width = 140;
            this.GridView1.Columns["Col_txtqty_after_cut"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcut_id"].Visible = true;  //"Col_txtcut_id";
            this.GridView1.Columns["Col_txtcut_id"].Width = 140;
            this.GridView1.Columns["Col_txtcut_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcut_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcut_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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
                                   " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                   " AND (txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
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

                            this.txtcost_qty_balance_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                            this.txtcost_qty_price_average_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");        //19
                            this.txtcost_money_sum_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");        //20
                            this.txtcost_qty2_balance_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00");        //24

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
        }
        private void GridView1_Cal_Sum()
        {

            double Sum2_Qty_Yokpai = 0;
            double Sum_Qty = 0;
            double Sum2_Qty = 0;
            double Con_QTY = 0;

            double QAbyma = 0;
            double QAbyma2 = 0;
            double Qbypai = 0;
            double Qbypai2 = 0;
            double Mbypai = 0;
            double QAbypai = 0;

            double Q_Q = 0;
            double Q_P = 0;


            double Sum_Qty_Pub = 0;

            double Sum_R_Kg = 0;
            double Sum_B_Kg = 0;
            double Sum_R_Pub = 0;
            double Sum_B_Pub = 0;

            double Sum_Qty_RIB = 0;
            double Sum_Qty_Pub_kg = 0;
            double Sum_Qty_RIB_kg = 0;

            double Sum_R_rib = 0;
            double Sum_B_rib = 0;
            double Q_rib_Q = 0;
            double Sum_R_Pub2 = 0;
            double Sum_B_Pub2 = 0;
            double Sum_R_rib2 = 0;
            double Sum_B_rib2 = 0;

            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                k = 1 + i;


                this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();

                if (this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = ".0000";
                }

                if (this.GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty2"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtprice"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtprice"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";
                }

                if (this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = ".00";
                }


                if (this.GridView1.Rows[i].Cells["Col_txtqty_want_pub"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_want_pub"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_want_rib"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_want_rib"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value = ".00";
                }
                if (double.Parse(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                {

                    this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value).ToString("###,###.0000");     //7


                    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty2"].Value).ToString("###,###.00");     //8

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

                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8

                    //this.GridView1.Columns[40].Name = "Col_txtqty_balance_yokpai";
                    //this.GridView1.Columns[41].Name = "Col_txtsum_qty_pub_yokpai";



                }

                if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() != "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "1";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = "1";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }
                    //จำนวน Kg ยกไป
                    Q_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Q_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน rib ยกไป
                    Q_rib_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value = Q_rib_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน พับ ยกไป
                    Q_P = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_yokpai"].Value = Q_P.ToString("N", new CultureInfo("en-US"));


                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    //จบ GRID===============================================================================================================================================================

                    //รวมรับ (Kg)[1]  : =================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //ผ้าดิบ[2] :=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));


                    //RIB [3] :=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));
                    //=========================================================================================================================================================================

                    //1.รับแล้ว (พับ) ยกไป :
                    Sum_R_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai.Text = Sum_R_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_R_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai_kg.Text = Sum_R_Pub2.ToString("N", new CultureInfo("en-US"));

                    //2.ค้างรับ (พับ) ยกไป :
                    Sum_B_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_yokpai.Text = Sum_B_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_B_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_yokpai_kg.Text = Sum_B_Pub2.ToString("N", new CultureInfo("en-US"));

                    //3.RIB รับแล้ว (พับ) ยกไป : :
                    Sum_R_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai.Text = Sum_R_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_R_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai_kg.Text = Sum_R_rib2.ToString("N", new CultureInfo("en-US"));

                    //4.RIB ค้างรับ (พับ) ยกไป :
                    Sum_B_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_yokpai.Text = Sum_B_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_B_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_yokpai_kg.Text = Sum_B_rib2.ToString("N", new CultureInfo("en-US"));

                    //5.รับแล้ว (Kg) ยกไป :
                    Sum_R_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_receive_yokpai.Text = Sum_R_Kg.ToString("N", new CultureInfo("en-US"));

                    //6.ค้างรับ (Kg) ยกไป :
                    Sum_B_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_yokpai.Text = Sum_B_Kg.ToString("N", new CultureInfo("en-US"));



                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    //============================================================================================================
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }


                }
                else
                {


                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() != "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }

                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = "0";

                    //จำนวน Kg ยกไป
                    Q_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Q_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน rib ยกไป
                    Q_rib_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value = Q_rib_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน พับ ยกไป
                    Q_P = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_yokpai"].Value = Q_P.ToString("N", new CultureInfo("en-US"));


                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    //จบ GRID===============================================================================================================================================================

                    //รวมรับ (Kg)[1]  : =================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //ผ้าดิบ[2] :=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));


                    //RIB [3] :=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));
                    //=========================================================================================================================================================================
                    //1.รับแล้ว (พับ) ยกไป :
                    Sum_R_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai.Text = Sum_R_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_R_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai_kg.Text = Sum_R_Pub2.ToString("N", new CultureInfo("en-US"));

                    //2.ค้างรับ (พับ) ยกไป :
                    Sum_B_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_yokpai.Text = Sum_B_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_B_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_yokpai_kg.Text = Sum_B_Pub2.ToString("N", new CultureInfo("en-US"));

                    //3.RIB รับแล้ว (พับ) ยกไป : :
                    Sum_R_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai.Text = Sum_R_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_R_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai_kg.Text = Sum_R_rib2.ToString("N", new CultureInfo("en-US"));

                    //4.RIB ค้างรับ (พับ) ยกไป :
                    Sum_B_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_yokpai.Text = Sum_B_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_B_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_yokpai_kg.Text = Sum_B_rib2.ToString("N", new CultureInfo("en-US"));

                    //5.รับแล้ว (Kg) ยกไป :
                    Sum_R_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_receive_yokpai.Text = Sum_R_Kg.ToString("N", new CultureInfo("en-US"));

                    //6.ค้างรับ (Kg) ยกไป :
                    Sum_B_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_yokpai.Text = Sum_B_Kg.ToString("N", new CultureInfo("en-US"));

                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    //============================================================================================================
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }



                }


                //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                QAbyma = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                QAbyma2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                //1.เหลือยกมา - รับ = จำนวนเหลือทั้งสิ้น
                Qbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                //2.มูลค่าเหลือยกมา - มูลค่ารับ = มูลค่ารวมทั้งสิ้น
                Mbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_total.Text.ToString()));
                this.txtcost_money_sum_yokpai.Text = Mbypai.ToString("N", new CultureInfo("en-US"));
                //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                if (Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokpai.Text.ToString())) > 0)
                {
                    QAbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokpai.Text.ToString())) / Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokpai.Text.ToString()));
                    this.txtcost_qty_price_average_yokpai.Text = QAbypai.ToString("N", new CultureInfo("en-US"));
                }
                else
                {
                    this.txtcost_qty_price_average_yokpai.Text = "0";
                }

                //1.เหลือ(2)ยกมา - รับ(2) = จำนวนเหลือ(2)ทั้งสิ้น
                Qbypai2 = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty2_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum2_qty.Text.ToString()));
                this.txtcost_qty2_balance_yokpai.Text = Qbypai2.ToString("N", new CultureInfo("en-US"));

                //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                //  ===========================================================================================================

            }

            this.txtcount_rows.Text = k.ToString();


            Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            QAbyma = 0;
            QAbyma2 = 0;
            Qbypai = 0;
            Qbypai2 = 0;
            Mbypai = 0;
            QAbypai = 0;

            Q_Q = 0;
            Q_P = 0;

            Sum_R_Kg = 0;
            Sum_B_Kg = 0;
            Sum_R_Pub = 0;
            Sum_B_Pub = 0;

            Sum_Qty_Pub = 0;
            Sum_Qty_RIB = 0;
            Sum_Qty_Pub_kg = 0;
            Sum_Qty_RIB_kg = 0;

            Sum_R_rib = 0;
            Sum_B_rib = 0;
            Q_rib_Q = 0;

            Sum_R_Pub2 = 0;
            Sum_B_Pub2 = 0;
            Sum_R_rib2 = 0;
            Sum_B_rib2 = 0;


        }
        private void GridView1_Cal_Sum_For_Cancel()
        {

            double Sum2_Qty_Yokpai = 0;
            double Sum_Qty = 0;
            double Sum2_Qty = 0;
            double Con_QTY = 0;

            double QAbyma = 0;
            double QAbyma2 = 0;
            double Qbypai = 0;
            double Qbypai2 = 0;
            double Mbypai = 0;
            double QAbypai = 0;

            double Q_Q = 0;
            double Q_P = 0;


            double Sum_Qty_Pub = 0;

            double Sum_R_Kg = 0;
            double Sum_B_Kg = 0;
            double Sum_R_Pub = 0;
            double Sum_B_Pub = 0;

            double Sum_Qty_RIB = 0;
            double Sum_Qty_Pub_kg = 0;
            double Sum_Qty_RIB_kg = 0;

            double Sum_R_rib = 0;
            double Sum_B_rib = 0;
            double Q_rib_Q = 0;
            double Sum_R_Pub2 = 0;
            double Sum_B_Pub2 = 0;
            double Sum_R_rib2 = 0;
            double Sum_B_rib2 = 0;

            double Sum_Qty_CUT_Yokpai = 0;
            double Sum_Qty_AF_CUT_Yokpai = 0;


            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                k = 1 + i;


                this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();

                if (this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = ".0000";
                }

                if (this.GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty2"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtprice"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtprice"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";

                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";
                }

                if (this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = ".00";
                }


                if (this.GridView1.Rows[i].Cells["Col_txtqty_want_pub"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_want_pub"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_want_rib"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_want_rib"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value = ".00";
                }
                if (double.Parse(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                {

                    this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value).ToString("###,###.0000");     //7


                    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty"].Value).ToString("###,###.00");     //7
                    this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty2"].Value).ToString("###,###.00");     //8

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

                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8

                    //this.GridView1.Columns[40].Name = "Col_txtqty_balance_yokpai";
                    //this.GridView1.Columns[41].Name = "Col_txtsum_qty_pub_yokpai";



                }

                if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() != "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "1";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = "1";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }
                    //จำนวน Kg ยกไป
                    Q_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Q_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน rib ยกไป
                    Q_rib_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value = Q_rib_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน พับ ยกไป
                    Q_P = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_yokpai"].Value = Q_P.ToString("N", new CultureInfo("en-US"));


                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    //จบ GRID===============================================================================================================================================================

                    //รวมรับ (Kg)[1]  : =================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //ผ้าดิบ[2] :=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));


                    //RIB [3] :=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));
                    //=========================================================================================================================================================================

                    //1.รับแล้ว (พับ) ยกไป :
                    Sum_R_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai.Text = Sum_R_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_R_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai_kg.Text = Sum_R_Pub2.ToString("N", new CultureInfo("en-US"));

                    //2.ค้างรับ (พับ) ยกไป :
                    Sum_B_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_yokpai.Text = Sum_B_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_B_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_yokpai_kg.Text = Sum_B_Pub2.ToString("N", new CultureInfo("en-US"));

                    //3.RIB รับแล้ว (พับ) ยกไป : :
                    Sum_R_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai.Text = Sum_R_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_R_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai_kg.Text = Sum_R_rib2.ToString("N", new CultureInfo("en-US"));

                    //4.RIB ค้างรับ (พับ) ยกไป :
                    Sum_B_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_yokpai.Text = Sum_B_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_B_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_yokpai_kg.Text = Sum_B_rib2.ToString("N", new CultureInfo("en-US"));

                    //5.รับแล้ว (Kg) ยกไป :
                    Sum_R_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_receive_yokpai.Text = Sum_R_Kg.ToString("N", new CultureInfo("en-US"));

                    //6.ค้างรับ (Kg) ยกไป :
                    Sum_B_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_yokpai.Text = Sum_B_Kg.ToString("N", new CultureInfo("en-US"));



                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    //============================================================================================================
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }

                    //300 - 300 =   0  "txtqty_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value.ToString())) + "'," +
                    //"txtqty_after_cut = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty_after_receive_yokpai"].Value.ToString())) + "'" +

                    //Sum_Qty_CUT_Yokpai       300 - 300 =   0 =================================================
                    Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_AF_CUT_Yokpai    0 + 300 = 300 =================================================
                    Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));


                }
                else
                {


                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() != "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    }

                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = "0";

                    //จำนวน Kg ยกไป
                    Q_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Q_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน rib ยกไป
                    Q_rib_Q = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_rib"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value = Q_rib_Q.ToString("N", new CultureInfo("en-US"));

                    //จำนวน พับ ยกไป
                    Q_P = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_pub"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_yokpai"].Value = Q_P.ToString("N", new CultureInfo("en-US"));


                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    //จบ GRID===============================================================================================================================================================

                    //รวมรับ (Kg)[1]  : =================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //ผ้าดิบ[2] :=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));


                    //RIB [3] :=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));
                    //=========================================================================================================================================================================
                    //1.รับแล้ว (พับ) ยกไป :
                    Sum_R_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai.Text = Sum_R_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_R_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_receive_yokpai_kg.Text = Sum_R_Pub2.ToString("N", new CultureInfo("en-US"));

                    //2.ค้างรับ (พับ) ยกไป :
                    Sum_B_Pub = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub.Text.ToString()));
                    this.txtsum_qty_pub_yokpai.Text = Sum_B_Pub.ToString("N", new CultureInfo("en-US"));
                    Sum_B_Pub2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_pub_kg.Text.ToString()));
                    this.txtsum_qty_pub_yokpai_kg.Text = Sum_B_Pub2.ToString("N", new CultureInfo("en-US"));

                    //3.RIB รับแล้ว (พับ) ยกไป : :
                    Sum_R_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai.Text = Sum_R_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_R_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_receive_yokma_kg.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_receive_yokpai_kg.Text = Sum_R_rib2.ToString("N", new CultureInfo("en-US"));

                    //4.RIB ค้างรับ (พับ) ยกไป :
                    Sum_B_rib = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib.Text.ToString()));
                    this.txtsum_qty_rib_yokpai.Text = Sum_B_rib.ToString("N", new CultureInfo("en-US"));
                    Sum_B_rib2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_yokma_kg.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_rib_kg.Text.ToString()));
                    this.txtsum_qty_rib_yokpai_kg.Text = Sum_B_rib2.ToString("N", new CultureInfo("en-US"));

                    //5.รับแล้ว (Kg) ยกไป :
                    Sum_R_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_receive_yokpai.Text = Sum_R_Kg.ToString("N", new CultureInfo("en-US"));

                    //6.ค้างรับ (Kg) ยกไป :
                    Sum_B_Kg = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_yokpai.Text = Sum_B_Kg.ToString("N", new CultureInfo("en-US"));

                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    //============================================================================================================
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }

                    //300 - 300 =   0  "txtqty_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value.ToString())) + "'," +
                    //"txtqty_after_cut = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty_after_receive_yokpai"].Value.ToString())) + "'" +

                    //Sum_Qty_CUT_Yokpai       300 - 300 =   0 =================================================
                    Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_AF_CUT_Yokpai    0 + 300 = 300 =================================================
                    Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));


                }


                //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                QAbyma = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                QAbyma2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                //1.เหลือยกมา - รับ = จำนวนเหลือทั้งสิ้น
                Qbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                //2.มูลค่าเหลือยกมา - มูลค่ารับ = มูลค่ารวมทั้งสิ้น
                Mbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_total.Text.ToString()));
                this.txtcost_money_sum_yokpai.Text = Mbypai.ToString("N", new CultureInfo("en-US"));
                //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                if (Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokpai.Text.ToString())) > 0)
                {
                    QAbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokpai.Text.ToString())) / Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokpai.Text.ToString()));
                    this.txtcost_qty_price_average_yokpai.Text = QAbypai.ToString("N", new CultureInfo("en-US"));
                }
                else
                {
                    this.txtcost_qty_price_average_yokpai.Text = "0";
                }

                //1.เหลือ(2)ยกมา - รับ(2) = จำนวนเหลือ(2)ทั้งสิ้น
                Qbypai2 = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty2_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum2_qty.Text.ToString()));
                this.txtcost_qty2_balance_yokpai.Text = Qbypai2.ToString("N", new CultureInfo("en-US"));

                //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                //  ===========================================================================================================

            }

            this.txtcount_rows.Text = k.ToString();


            Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            QAbyma = 0;
            QAbyma2 = 0;
            Qbypai = 0;
            Qbypai2 = 0;
            Mbypai = 0;
            QAbypai = 0;

            Q_Q = 0;
            Q_P = 0;

            Sum_R_Kg = 0;
            Sum_B_Kg = 0;
            Sum_R_Pub = 0;
            Sum_B_Pub = 0;

            Sum_Qty_Pub = 0;
            Sum_Qty_RIB = 0;
            Sum_Qty_Pub_kg = 0;
            Sum_Qty_RIB_kg = 0;

            Sum_R_rib = 0;
            Sum_B_rib = 0;
            Q_rib_Q = 0;

            Sum_R_Pub2 = 0;
            Sum_B_Pub2 = 0;
            Sum_R_rib2 = 0;
            Sum_B_rib2 = 0;

             Sum_Qty_CUT_Yokpai = 0;
             Sum_Qty_AF_CUT_Yokpai = 0;

        }
        private void GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value = true;

                if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //Active
                {
                    this.GridView1.Rows[i].Cells["Col_Chk1"].Value = true;
                }
                else
                {
                    this.GridView1.Rows[i].Cells["Col_Chk1"].Value = false;

                }
            }
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
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                //Col_Chk_SELECT    Col_date
                GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Style.BackColor = Color.Black;
                GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Style.ForeColor = Color.LightGreen;

                GridView1.Rows[i].Cells["Col_txtlot_no"].Style.BackColor = Color.Blue;
                GridView1.Rows[i].Cells["Col_txtlot_no"].Style.ForeColor = Color.White;

                GridView1.Rows[i].Cells["Col_txtfold_number"].Style.BackColor = Color.LightGoldenrodYellow;


                GridView1.Rows[i].Cells["Col_Chk_SELECT"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_date"].Style.BackColor = Color.LightSkyBlue;

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
            var frm2 = new HOME03_Production.HOME03_Production_04QC_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "บันทึกใบรับผ้าย้อม";
            this.txtPPT_id.ReadOnly = true;
        }
        private void btnopen_Click(object sender, EventArgs e)
        {

        }
        private void BtnSave_Click(object sender, EventArgs e)
        {

        }
        private void BtnCancel_Doc_Click(object sender, EventArgs e)
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

            if (W_ID_Select.M_FORM_CANCEL.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            this.txtword_cancel1.Visible = true;
            this.txtword_cancel2.Visible = true;
            if (this.txtword_cancel2.Text == "")
            {
                MessageBox.Show("กรุณาระบุสาเหตุ ที่ยกเลิก ก่อน !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtword_cancel2.Focus();
                return;

            }

            W_ID_Select.LOG_ID = "7";
            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
            TRANS_LOG();

            this.iblword_status.Text = "ยกเลิกเอกสาร";
            //======================================================
            //======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT * FROM c002_07Receive_Send_dye_record" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtRG_id = '" + this.txtRG_id.Text.Trim() + "')" +
                                    " AND (txtrg_status = '1')";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("เอกสารนี้   : '" + this.txtPPT_id.Text.Trim() + "' ยกเลิกไปแล้ว ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    conn.Close();
                    return;
                }
            }

            //
            conn.Close();

            //จบเชื่อมต่อฐานข้อมูล=======================================================

            Show_Qty_Yokma();
            GridView1_Cal_Sum_For_Cancel();
            Sum_group_tax();

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

                    string Cancel_ID = W_ID_Select.CDKEY.Trim() + "-" + W_ID_Select.M_USERNAME.Trim() + "-" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "-" + myDateTime2.ToString("HH:mm:ss", UsaCulture);

                    if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                    {

                        cmd2.CommandText = "INSERT INTO c002_07Receive_Send_dye_record_cancel(cdkey,txtco_id,txtbranch_id," +  //1
                                                                                                                       //"txttrans_date," +
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                               "txtform_name,txtform_caption," +  //5
                                                "txtuser_name,txtemp_office_name," +  //6
                                               "txtlog_id,txtlog_name," +  //7
                                              "txtdocument_id,txtcancel_remark,txtversion_id,txtcount,cancel_id) " +  //8
                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                                                                            //"@txttrans_date," +
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtform_name,@txtform_caption," +  //5
                                               "@txtuser_name,@txtemp_office_name," +  //6
                                               "@txtlog_id,@txtlog_name," +  //7
                                               "@txtdocument_id,@txtcancel_remark,@txtversion_id,@txtcount,@cancel_id)";   //8

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
                        cmd2.Parameters.Add("@txtcancel_remark", SqlDbType.NVarChar).Value = this.txtword_cancel2.Text.Trim();
                        cmd2.Parameters.Add("@txtversion_id", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();
                        cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 1));
                        cmd2.Parameters.Add("@cancel_id", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok1");

                        //2
                        cmd2.CommandText = "UPDATE c002_07Receive_Send_dye_record" +
                                                                    " SET txtrg_status = '1'," +
                                                                     "txtsum_qty_pub = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_pub_receive = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_pub_balance = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +

                                                                     "txtsum_qty_pub_kg = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_pub_receive_kg = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_pub_balance_kg = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +

                                                                     "txtsum_qty_rib = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_rib_receive = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_rib_balance = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +

                                                                     "txtsum_qty_rib_kg = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_rib_receive_kg = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_rib_balance_kg = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +

                                                                     "txtsum_qty = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_receive = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                     "txtsum_qty_balance = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'" +

                                                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                     " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                     " AND (txtRG_id = '" + this.txtRG_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok2");
                        //2
                        //MessageBox.Show("ok2");
                        cmd2.CommandText = "UPDATE c002_05Send_dye_record SET txtRG_id = ''," +
                                          "txtRG_date = ''," +

                                           "txtsum_qty_pub_receive = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub_receive_yokpai.Text.ToString())) + "'," +
                                           "txtsum_qty_pub_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub_yokpai.Text.ToString())) + "'," +

                                           "txtsum_qty_pub_receive_kg = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub_receive_yokpai_kg.Text.ToString())) + "'," +
                                           "txtsum_qty_pub_balance_kg = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub_yokpai_kg.Text.ToString())) + "'," +

                                           "txtsum_qty_rib_receive = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib_receive_yokpai.Text.ToString())) + "'," +
                                           "txtsum_qty_rib_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib_yokpai.Text.ToString())) + "'," +

                                           "txtsum_qty_rib_receive_kg = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib_receive_yokpai_kg.Text.ToString())) + "'," +
                                           "txtsum_qty_rib_balance_kg = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib_yokpai_kg.Text.ToString())) + "'," +

                                           "txtsum_qty_receive = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_receive_yokpai.Text.ToString())) + "'," +
                                           "txtsum_qty_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_yokpai.Text.ToString())) + "'," +

                                           "txtRG_status = ''" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtPPT_id = '" + this.txtPPT_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                        //5
                    }

                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {

                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();


                            if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                            {
                                if (this.txtreceive_send_dye_type_id.Text == "01")
                                {
                                    cmd2.CommandText = "UPDATE c002_07Receive_Send_dye_record_detail SET " +
                                                           "txtqty = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString())) + "'," +
                                                            "txtqty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +

                                                           "txtqty_pub = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_yokpai"].Value.ToString())) + "'," +
                                                           "txtqty_balance_pub = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) + "'," +

                                                           "txtqty_rib = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value.ToString())) + "'," +
                                                           "txtqty_balance_rib = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString())) + "'" +

                                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                           " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                           " AND (txtface_baking_id = '" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "')" +
                                                           " AND (txtnumber_in_year = '" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "')" +
                                                           " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                                           " AND (txtRG_id = '" + this.txtRG_id.Text.Trim() + "')";

                                    cmd2.ExecuteNonQuery();

                                }
                                if (this.txtreceive_send_dye_type_id.Text == "02")
                                {
                                    cmd2.CommandText = "UPDATE c002_07Receive_Send_dye_record_detail SET " +
                                                           "txtqty = '" + Convert.ToDouble(string.Format("{0:n4}",0)) + "'," +
                                                            "txtqty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +

                                                           "txtqty_pub = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                           "txtqty_balance_pub = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +

                                                           "txtqty_rib = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                           "txtqty_balance_rib = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'" +

                                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                           " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                           " AND (txtface_baking_id = '" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "')" +
                                                           " AND (txtnumber_in_year = '" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "')" +
                                                           " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                                           " AND (txtRG_id = '" + this.txtRG_id.Text.Trim() + "')";

                                    cmd2.ExecuteNonQuery();

                                }

                                ///
                                cmd2.CommandText = "UPDATE c002_05Send_dye_record_detail SET " +
                                                       "txtqty = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString())) + "'," +
                                                        "txtqty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +

                                                       "txtqty_pub = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_yokpai"].Value.ToString())) + "'," +
                                                       "txtqty_balance_pub = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) + "'," +

                                                       "txtqty_rib = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_yokpai"].Value.ToString())) + "'," +
                                                       "txtqty_balance_rib = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString())) + "'" +

                                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                       " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                       " AND (txtface_baking_id = '" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "')" +
                                                       " AND (txtnumber_in_year = '" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "')" +
                                                       " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                                       " AND (txtPPT_id = '" + this.txtPPT_id.Text.Trim() + "')";

                                cmd2.ExecuteNonQuery();



                                //=====================================================================================================
                            }
                        }
                    }


                    //สต๊อคสินค้า ตามคลัง =============================================================================================
                    //สต๊อคสินค้า ตามคลัง =============================================================================================



                    //1.k021_mat_average
                    cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                       "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +
                                       "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +
                                        "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +
                                       "txtcost_qty2_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                       " AND (txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.ToString() + "')";


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


                            "'" + this.txtRG_id.Text.Trim() + "'," +  //7 txtbill_id
                            "'RGMF'," +  //9 txtbill_type
                            "'ยกเลิกรับผ้าย้อม" + this.txtword_cancel2.Text.Trim() + "'," +  //9 txtbill_remark

                             "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                           "'" + this.txtmat_no.Text + "'," +  //10 
                            "'" + this.PANEL_MAT_txtmat_id.Text.ToString() + "'," +  //11
                            "'" + this.PANEL_MAT_txtmat_name.Text.ToString() + "'," +    //12

                            "'" + this.txtmat_unit1_name.Text.ToString() + "'," +  //13
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtmat_unit1_qty.Text.ToString())) + "'," +  //14
                            "'" + this.chmat_unit_status.Text.ToString() + "'," +  //15
                            "'" + this.txtmat_unit2_name.Text.ToString() + "'," +  //16
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +  //17

                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //18  txtqty_in
                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //19 txtqty2_in
                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //20 txtprice_in
                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //21 txtsum_total_in

                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty.Text.ToString())) + "'," +  //22 txtqty_out
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum2_qty.Text.ToString())) + "'," +  //23 txtqty2_out
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtprice.Text.ToString())) + "'," +  //24 txtprice_out
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_total.Text.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +  //26
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'," +  //27
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +  //28
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +  //29

                           "'1')";   //30

                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok8");


                    //======================================

                    //สต๊อคสินค้า ตามคลัง =============================================================================================



                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    //MessageBox.Show("ok4");


                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtRG_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        this.BtnCancel_Doc.Enabled = false;

                        if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                        {
                            W_ID_Select.LOG_ID = "7";
                            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
                            TRANS_LOG();
                        }

                        MessageBox.Show("ยกเลิกเอกสาร เรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            W_ID_Select.TRANS_ID = this.txtRG_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_07Receive_Send_Dye_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_07Receive_Send_Dye_record_print();
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
            W_ID_Select.TRANS_ID = this.txtRG_id.Text.Trim();
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

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_c002_02produce_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_07Receive_Send_dye_record.rpt");


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
                rpt.SetParameterValue("txtrg_id", W_ID_Select.TRANS_ID.Trim());

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

                    cmd2.CommandText = "UPDATE c002_07Receive_Send_dye_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtRG_id = '" + this.txtRG_id.Text.Trim() + "')";
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






        //=========================================================

    }
}
