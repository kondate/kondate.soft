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
    public partial class HOME03_Production_01RG_record_detail : Form
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


        public HOME03_Production_01RG_record_detail()
        {
            InitializeComponent();
        }

        private void HOME02_Purchasing_05RG_record_detail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0301RGDL";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิด";
            TRANS_LOG();

            this.iblword_status.Text = "ดูข้อมูลใบรับสินค้า หรือ วัตถุดิบ";

            this.ActiveControl = this.txtrg_remark;
            this.BtnNew.Enabled = false;
            this.btnopen.Enabled = false;
            this.BtnSave.Enabled = false;
            this.BtnCancel_Doc.Enabled = true;
            this.btnPreview.Enabled = true;
            this.BtnPrint.Enabled = true;

            Show_GridView1();
            Fill_DATA_TO_GridView1();
            Fill_EMP();

            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();
            GridView1_Color_Column();

        }


        private void Fill_DATA_TO_GridView1()
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

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                if (W_ID_Select.RECEIVE_TYPE == "01")
                {



                cmd2.CommandText = "SELECT c003_receive_record.*," +
                                   "c003_receive_record_detail.*," +
                                   "k013_1db_acc_07project.*," +
                                   "k013_1db_acc_17job.*," +
                                   "k013_1db_acc_13group_tax.*," +
                                   "k018db_po_record.*," +
                                   "k016db_1supplier.*," +
                                   "k017db_pr_record.*," +
                                    "k013_1db_acc_06wherehouse.*," +
                                  "k013_1db_acc_16department.*" +

                                   " FROM c003_receive_record" +

                                   " INNER JOIN c003_receive_record_detail" +
                                   " ON c003_receive_record.cdkey = c003_receive_record_detail.cdkey" +
                                   " AND c003_receive_record.txtco_id = c003_receive_record_detail.txtco_id" +
                                   " AND c003_receive_record.txtCRG_id = c003_receive_record_detail.txtCRG_id" +

                                   " INNER JOIN k013_1db_acc_07project" +
                                   " ON c003_receive_record.cdkey = k013_1db_acc_07project.cdkey" +
                                   " AND c003_receive_record.txtco_id = k013_1db_acc_07project.txtco_id" +
                                   " AND c003_receive_record.txtproject_id = k013_1db_acc_07project.txtproject_id" +

                                   " INNER JOIN k013_1db_acc_17job" +
                                   " ON c003_receive_record.cdkey = k013_1db_acc_17job.cdkey" +
                                   " AND c003_receive_record.txtco_id = k013_1db_acc_17job.txtco_id" +
                                   " AND c003_receive_record.txtjob_id = k013_1db_acc_17job.txtjob_id" +

                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c003_receive_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                   " INNER JOIN k018db_po_record" +
                                   " ON c003_receive_record.cdkey = k018db_po_record.cdkey" +
                                   " AND c003_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                   " AND c003_receive_record.txtPo_id = k018db_po_record.txtPo_id" +

                                   " INNER JOIN k016db_1supplier" +
                                   " ON c003_receive_record.cdkey = k016db_1supplier.cdkey" +
                                   " AND c003_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                   " AND c003_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                   " INNER JOIN k017db_pr_record" +
                                   " ON c003_receive_record.cdkey = k017db_pr_record.cdkey" +
                                   " AND c003_receive_record.txtco_id = k017db_pr_record.txtco_id" +
                                   " AND c003_receive_record.txtPo_id = k017db_pr_record.txtPo_id" +

                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c003_receive_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c003_receive_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c003_receive_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +


                                   " INNER JOIN k013_1db_acc_16department" +
                                   " ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                   " AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                   " AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                   " WHERE (c003_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (c003_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (c003_receive_record.txtCRG_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                  " ORDER BY c003_receive_record.txtCRG_id ASC";


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        this.txtCRG_id.Text = dt2.Rows[0]["txtCRG_id"].ToString();
                        this.txtreceive_type_id.Text = dt2.Rows[0]["txtreceive_type_id"].ToString();
                        if (this.txtreceive_type_id.Text == "01")
                        {
                            this.cbotxtreceive_type_name.Text = "รับตามใบสั่งซื้อ";
                        }
                        else
                        {
                            this.cbotxtreceive_type_name.Text = "รับไม่มีใบสั่งซื้อ";

                        }
                        this.txtPr_id.Text = dt2.Rows[0]["txtCPr_id"].ToString();
                        this.txtPo_id.Text = dt2.Rows[0]["txtCPo_id"].ToString();
                        this.txtapprove_id.Text = dt2.Rows[0]["txtCapprove_id"].ToString();

                        this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                        this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_client"].ToString());
                        this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                        this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

                        this.txtrg_remark.Text = dt2.Rows[0]["txtcrg_remark"].ToString();

                        this.Paneldate_txtcurrency_date.Text = dt2.Rows[0]["txtcurrency_date"].ToString();
                        this.txtcurrency_id.Text = dt2.Rows[0]["txtcurrency_id"].ToString();
                        this.txtcurrency_rate.Text = dt2.Rows[0]["txtcurrency_rate"].ToString();

                        this.txtemp_office_name.Text = dt2.Rows[0]["txtemp_office_name"].ToString();
                        this.txtemp_office_name_manager.Text = dt2.Rows[0]["txtemp_office_name_manager"].ToString();
                        this.txtemp_office_name_approve.Text = dt2.Rows[0]["txtemp_office_name_approve"].ToString();

                            this.txtprice.Text = Convert.ToSingle(dt2.Rows[0]["txtsum_price"]).ToString("###,###.00");

                            this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");

                        this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                        this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();


                        this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                        this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                        this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();
                        this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();


                        this.PANEL1307_PROJECT_txtproject_id.Text = dt2.Rows[0]["txtproject_id"].ToString();
                        this.PANEL1307_PROJECT_txtproject_name.Text = dt2.Rows[0]["txtproject_name"].ToString();

                        this.PANEL1317_JOB_txtjob_id.Text = dt2.Rows[0]["txtjob_id"].ToString();
                        this.PANEL1317_JOB_txtjob_name.Text = dt2.Rows[0]["txtjob_name"].ToString();

                        this.txtVat_id.Text = dt2.Rows[0]["txtVat_id"].ToString();
                        this.txtVat_date.Text = dt2.Rows[0]["txtVat_date"].ToString();
                        this.PANEL003_EMP_txtemp_id.Text = dt2.Rows[0]["txtemp_id"].ToString();


                            this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                            this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                            this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                            this.txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();


                            this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit1_qty"]).ToString("###,###.00");
                            this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                            this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit1_qty"]).ToString("###,###.00");
                            this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                            this.txtmat_unit2_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit2_qty"]).ToString("###,###.00");


                            this.txtsum_qty_receive_yokma.Text = dt2.Rows[0]["txtsum_qty_receive"].ToString();  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {


                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtLot_no"].Value = dt2.Rows[j]["txtLot_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3

                            GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //5

                            GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //6

                            GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //5

                            GridView1.Rows[index].Cells["Col_txtqty_want"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");      //8
                            //GridView1.Rows[index].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_balance"]).ToString("###,###.00");      //8
                            GridView1.Rows[index].Cells["Col_txtqty_balance"].Value = "0";      //8
                            GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //9
                            GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");      //10

                            GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //11
                            GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //12
                            GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //13
                            GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //14

                            GridView1.Rows[index].Cells["Col_txtwant_receive_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();      //15
                            GridView1.Rows[index].Cells["Col_txtmade_receive_date"].Value = dt2.Rows[j]["txtmade_receive_date"].ToString();   //16
                            GridView1.Rows[index].Cells["Col_txtexpire_receive_date"].Value = dt2.Rows[j]["txtexpire_receive_date"].ToString();  //17

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //21
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //22
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //23

                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";      //24
                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";      //25

                            GridView1.Rows[index].Cells["Col_txtqty_balance_yokpai"].Value = "0";      //26
                            GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = "0";      //26

                            GridView1.Rows[index].Cells["Col_txtqty_cut"].Value = dt2.Rows[j]["txtqty_cut"].ToString();  //17
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = dt2.Rows[j]["txtqty_after_cut"].ToString();  //17
                            GridView1.Rows[index].Cells["Col_txtcut_id"].Value = dt2.Rows[j]["txtcut_id"].ToString();  //17
                            GridView1.Rows[index].Cells["Col_1"].Value = "1";  //17


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
            }
                //===========================================

                if (W_ID_Select.RECEIVE_TYPE == "02")
                {
                    cmd2.CommandText = "SELECT c003_receive_record.*," +
                                       "c003_receive_record_detail.*," +
                                       //"k013_1db_acc_07project.*," +
                                       //"k013_1db_acc_17job.*," +
                                       "k013_1db_acc_13group_tax.*," +
                                       //"k018db_po_record.*," +
                                       "k016db_1supplier.*," +
                                       //"k017db_pr_record.*," +
                                        "k013_1db_acc_06wherehouse.*" +
                                      //"k013_1db_acc_16department.*" +

                                       " FROM c003_receive_record" +

                                       " INNER JOIN c003_receive_record_detail" +
                                       " ON c003_receive_record.cdkey = c003_receive_record_detail.cdkey" +
                                       " AND c003_receive_record.txtco_id = c003_receive_record_detail.txtco_id" +
                                       " AND c003_receive_record.txtCRG_id = c003_receive_record_detail.txtCRG_id" +

                                       //" INNER JOIN k013_1db_acc_07project" +
                                       //" ON c003_receive_record.cdkey = k013_1db_acc_07project.cdkey" +
                                       //" AND c003_receive_record.txtco_id = k013_1db_acc_07project.txtco_id" +
                                       //" AND c003_receive_record.txtproject_id = k013_1db_acc_07project.txtproject_id" +

                                       //" INNER JOIN k013_1db_acc_17job" +
                                       //" ON c003_receive_record.cdkey = k013_1db_acc_17job.cdkey" +
                                       //" AND c003_receive_record.txtco_id = k013_1db_acc_17job.txtco_id" +
                                       //" AND c003_receive_record.txtjob_id = k013_1db_acc_17job.txtjob_id" +

                                       " INNER JOIN k013_1db_acc_13group_tax" +
                                       " ON c003_receive_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                       //" INNER JOIN k018db_po_record" +
                                       //" ON c003_receive_record.cdkey = k018db_po_record.cdkey" +
                                       //" AND c003_receive_record.txtco_id = k018db_po_record.txtco_id" +
                                       //" AND c003_receive_record.txtPo_id = k018db_po_record.txtPo_id" +

                                       " INNER JOIN k016db_1supplier" +
                                       " ON c003_receive_record.cdkey = k016db_1supplier.cdkey" +
                                       " AND c003_receive_record.txtco_id = k016db_1supplier.txtco_id" +
                                       " AND c003_receive_record.txtsupplier_id = k016db_1supplier.txtsupplier_id" +

                                       //" INNER JOIN k017db_pr_record" +
                                       //" ON c003_receive_record.cdkey = k017db_pr_record.cdkey" +
                                       //" AND c003_receive_record.txtco_id = k017db_pr_record.txtco_id" +
                                       //" AND c003_receive_record.txtPo_id = k017db_pr_record.txtPo_id" +

                                       " INNER JOIN k013_1db_acc_06wherehouse" +
                                       " ON c003_receive_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                       " AND c003_receive_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                       " AND c003_receive_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +


                                       //" INNER JOIN k013_1db_acc_16department" +
                                       //" ON k017db_pr_record.cdkey = k013_1db_acc_16department.cdkey" +
                                       //" AND k017db_pr_record.txtco_id = k013_1db_acc_16department.txtco_id" +
                                       //" AND k017db_pr_record.txtdepartment_id = k013_1db_acc_16department.txtdepartment_id" +

                                       " WHERE (c003_receive_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c003_receive_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (c003_receive_record.txtCRG_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                      " ORDER BY c003_receive_record.txtCRG_id ASC";


                    try
                    {
                        //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                        SqlDataAdapter da = new SqlDataAdapter(cmd2);
                        DataTable dt2 = new DataTable();
                        da.Fill(dt2);

                        if (dt2.Rows.Count > 0)
                        {

                            this.txtCRG_id.Text = dt2.Rows[0]["txtCRG_id"].ToString();
                            this.txtreceive_type_id.Text = dt2.Rows[0]["txtreceive_type_id"].ToString();
                            if (this.txtreceive_type_id.Text == "01")
                            {
                                this.cbotxtreceive_type_name.Text = "รับตามใบสั่งซื้อ";
                            }
                            else
                            {
                                this.cbotxtreceive_type_name.Text = "รับไม่มีใบสั่งซื้อ";

                            }
                            this.txtPr_id.Text = dt2.Rows[0]["txtCPr_id"].ToString();
                            this.txtPo_id.Text = dt2.Rows[0]["txtCPo_id"].ToString();
                            this.txtapprove_id.Text = dt2.Rows[0]["txtCapprove_id"].ToString();

                            this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                            this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                            this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_server"].ToString());
                            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);

                            this.txtrg_remark.Text = dt2.Rows[0]["txtcrg_remark"].ToString();

                            this.Paneldate_txtcurrency_date.Text = dt2.Rows[0]["txtcurrency_date"].ToString();
                            this.txtcurrency_id.Text = dt2.Rows[0]["txtcurrency_id"].ToString();
                            this.txtcurrency_rate.Text = dt2.Rows[0]["txtcurrency_rate"].ToString();

                            this.txtemp_office_name.Text = dt2.Rows[0]["txtemp_office_name"].ToString();
                            this.txtemp_office_name_manager.Text = dt2.Rows[0]["txtemp_office_name_manager"].ToString();
                            this.txtemp_office_name_approve.Text = dt2.Rows[0]["txtemp_office_name_approve"].ToString();

                            this.txtprice.Text = Convert.ToSingle(dt2.Rows[0]["txtsum_price"]).ToString("###,###.00");

                            this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                            this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                            this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");

                            this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                            this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();


                            this.PANEL161_SUP_txtsupplier_id.Text = dt2.Rows[0]["txtsupplier_id"].ToString();
                            this.PANEL161_SUP_txtsupplier_name.Text = dt2.Rows[0]["txtsupplier_name"].ToString();

                            //this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();
                            //this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();

                            this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                            this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                            this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                            this.txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();


                            this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit1_qty"]).ToString("###,###.00");
                            this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                            this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit1_qty"]).ToString("###,###.00");
                            this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                            this.txtmat_unit2_qty.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_unit2_qty"]).ToString("###,###.00");

                            //this.PANEL1307_PROJECT_txtproject_id.Text = dt2.Rows[0]["txtproject_id"].ToString();
                            //this.PANEL1307_PROJECT_txtproject_name.Text = dt2.Rows[0]["txtproject_name"].ToString();

                            //this.PANEL1317_JOB_txtjob_id.Text = dt2.Rows[0]["txtjob_id"].ToString();
                            //this.PANEL1317_JOB_txtjob_name.Text = dt2.Rows[0]["txtjob_name"].ToString();

                            this.txtVat_id.Text = dt2.Rows[0]["txtVat_id"].ToString();
                            this.txtVat_date.Text = dt2.Rows[0]["txtVat_date"].ToString();
                            this.PANEL003_EMP_txtemp_id.Text = dt2.Rows[0]["txtemp_id"].ToString();

                            this.txtsum_qty_receive_yokma.Text = ".00";  //ไว้สำหรับคำนวณว่า รับมาแล้ว จำนวนเท่าไร

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {


                                var index = GridView1.Rows.Add();
                                GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                                GridView1.Rows[index].Cells["Col_txtLot_no"].Value = dt2.Rows[j]["txtLot_no"].ToString();      //1
                                GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                                GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3

                                GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                                GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //5

                                GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //6

                                GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //4
                                GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //5

                                GridView1.Rows[index].Cells["Col_txtqty_want"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_want"]).ToString("###,###.00");      //8
                                                                                                                                                                 //GridView1.Rows[index].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_balance"]).ToString("###,###.00");      //8
                                GridView1.Rows[index].Cells["Col_txtqty_balance"].Value = "0";      //8
                                GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //9
                                GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");      //10

                                GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //11
                                GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //12
                                GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //13
                                GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //14

                                GridView1.Rows[index].Cells["Col_txtwant_receive_date"].Value = dt2.Rows[j]["txtwant_receive_date"].ToString();      //15
                                GridView1.Rows[index].Cells["Col_txtmade_receive_date"].Value = dt2.Rows[j]["txtmade_receive_date"].ToString();   //16
                                GridView1.Rows[index].Cells["Col_txtexpire_receive_date"].Value = dt2.Rows[j]["txtexpire_receive_date"].ToString();  //17

                                GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                                GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                                GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                                GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //21
                                GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //22
                                GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //23

                                GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";      //24
                                GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";      //25

                                GridView1.Rows[index].Cells["Col_txtqty_balance_yokpai"].Value = "0";      //26
                                GridView1.Rows[index].Cells["Col_txtqty_receive_yokpai"].Value = "0";      //26

                                GridView1.Rows[index].Cells["Col_txtqty_cut"].Value = dt2.Rows[j]["txtqty_cut"].ToString();  //17
                                GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = dt2.Rows[j]["txtqty_after_cut"].ToString();  //17
                                GridView1.Rows[index].Cells["Col_txtcut_id"].Value = dt2.Rows[j]["txtcut_id"].ToString();  //17
                                GridView1.Rows[index].Cells["Col_1"].Value = "1";  //17

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

                }
            }
            GridView1_Up_Status();
            GridView1_Cal_Sum();
            Sum_group_tax();
            //================================

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 36;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtLot_no";
            this.GridView1.Columns[2].Name = "Col_txtmat_no";
            this.GridView1.Columns[3].Name = "Col_txtmat_id";
            this.GridView1.Columns[4].Name = "Col_txtmat_name";
            this.GridView1.Columns[5].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[6].Name = "Col_txtmat_unit1_qty";

            this.GridView1.Columns[7].Name = "Col_chmat_unit_status";

            this.GridView1.Columns[8].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[9].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[10].Name = "Col_txtqty_want";
            this.GridView1.Columns[11].Name = "Col_txtqty_balance";  //  //ค้างรับยกมา
            this.GridView1.Columns[12].Name = "Col_txtqty";
            this.GridView1.Columns[13].Name = "Col_txtqty2";

            this.GridView1.Columns[14].Name = "Col_txtprice";
            this.GridView1.Columns[15].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[16].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[17].Name = "Col_txtsum_total";

            this.GridView1.Columns[18].Name = "Col_txtwant_receive_date";
            this.GridView1.Columns[19].Name = "Col_txtmade_receive_date";
            this.GridView1.Columns[20].Name = "Col_txtexpire_receive_date";

            this.GridView1.Columns[21].Name = "Col_txtcost_qty_balance_yokma";  //กก
            this.GridView1.Columns[22].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[23].Name = "Col_txtcost_money_sum_yokma";  //กก

            this.GridView1.Columns[24].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[25].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[26].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[27].Name = "Col_txtcost_qty2_balance_yokma";  //ปอนด์
            this.GridView1.Columns[28].Name = "Col_txtcost_qty2_balance_yokpai";  //ปอนด์

            this.GridView1.Columns[29].Name = "Col_txtqty_balance_yokpai";   //ค้างรับ
            this.GridView1.Columns[30].Name = "Col_mat_status";
            this.GridView1.Columns[31].Name = "Col_txtqty_receive_yokpai";  //รับแล้ว

            this.GridView1.Columns[32].Name = "Col_txtqty_cut";
            this.GridView1.Columns[33].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[34].Name = "Col_txtcut_id";
            this.GridView1.Columns[35].Name = "Col_1";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "Lot No";
            this.GridView1.Columns[2].HeaderText = "ลำดับ";
            this.GridView1.Columns[3].HeaderText = " รหัส";
            this.GridView1.Columns[4].HeaderText = " ชื่อสินค้า";
            this.GridView1.Columns[5].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[6].HeaderText = " หน่วย";
            this.GridView1.Columns[7].HeaderText = "แปลง";
            this.GridView1.Columns[8].HeaderText = " หน่วย2";
            this.GridView1.Columns[9].HeaderText = " หน่วย";

            this.GridView1.Columns[10].HeaderText = "จำนวนต้องการ";
            this.GridView1.Columns[11].HeaderText = "จำนวนค้างรับ";  //ค้างรับยกมา
            this.GridView1.Columns[12].HeaderText = "จำนวนรับ(หน่วยหลัก)";
            this.GridView1.Columns[13].HeaderText = "จำนวนรับ(หน่วย2)";

            this.GridView1.Columns[14].HeaderText = "ราคา";
            this.GridView1.Columns[15].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[16].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[17].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[18].HeaderText = "วันที่ต้องการ";
            this.GridView1.Columns[19].HeaderText = "วันผลิต";
            this.GridView1.Columns[20].HeaderText = "วันหมดอายุ";

            this.GridView1.Columns[21].HeaderText = "จำนวนยกมา";   //กก
            this.GridView1.Columns[22].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[23].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[24].HeaderText = "จำนวนยกไป";  //กก
            this.GridView1.Columns[25].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[26].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[27].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";  //ปอนด์
            this.GridView1.Columns[28].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";  //ปอนด์

            this.GridView1.Columns[29].HeaderText = "จำนวนค้างรับยกไป";   //กก
            this.GridView1.Columns[30].HeaderText = "สถานะ";
            this.GridView1.Columns[31].HeaderText = "จำนวนรับแล้วยกไป";  //กก

            this.GridView1.Columns[32].HeaderText = "จำนวนเบิกด้าย";  //กก
            this.GridView1.Columns[33].HeaderText = "จำนวนเหลือ";  //กก
            this.GridView1.Columns[34].HeaderText = "เลขที่เบิกด้าย";  //
            this.GridView1.Columns[35].HeaderText = "1";  //
            this.GridView1.Columns[35].Visible = false;



            this.GridView1.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 0;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtLot_no"].Visible = true;  //"Col_txtLot_no";
            this.GridView1.Columns["Col_txtLot_no"].Width = 140;
            this.GridView1.Columns["Col_txtLot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtLot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtLot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";

            this.GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 120;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 150;
            this.GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView1.Columns["Col_txtmat_unit1_name"].Width = 80;
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
            dgvCmb.Width = 70;
            dgvCmb.DisplayIndex = 7;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb);

            this.GridView1.Columns["Col_txtmat_unit2_name"].Visible = true;  //"Col_txtmat_unit2_name";
            this.GridView1.Columns["Col_txtmat_unit2_name"].Width = 80;
            this.GridView1.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit2_qty"].Visible = true;  //"Col_txtmat_unit2_qty";
            this.GridView1.Columns["Col_txtmat_unit2_qty"].Width = 80;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_want"].Visible = false;  //"Col_txtqty_want";
            this.GridView1.Columns["Col_txtqty_want"].Width = 0;
            this.GridView1.Columns["Col_txtqty_want"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_want"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_want"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_balance"].Visible = false;  //"Col_txtqty_balance";
            this.GridView1.Columns["Col_txtqty_balance"].Width = 0;
            this.GridView1.Columns["Col_txtqty_balance"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 140;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty2"].Visible = true;  //"Col_txtqty2";
            this.GridView1.Columns["Col_txtqty2"].Width = 140;
            this.GridView1.Columns["Col_txtqty2"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtprice"].Visible = true;  //"Col_txtprice";
            this.GridView1.Columns["Col_txtprice"].Width = 80;

            if (this.txtreceive_type_id.Text.Trim() == "01")
            {
                this.GridView1.Columns["Col_txtprice"].ReadOnly = true;
            }
            if (this.txtreceive_type_id.Text.Trim() == "02")
            {
                this.GridView1.Columns["Col_txtprice"].ReadOnly = false;
            }
            this.GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.GridView1.Columns["Col_txtdiscount_rate"].Width = 0;
            this.GridView1.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.GridView1.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_money"].Visible = true;  //"Col_txtdiscount_money";
            this.GridView1.Columns["Col_txtdiscount_money"].Width = 100;

            if (this.txtreceive_type_id.Text.Trim() == "01")
            {
                this.GridView1.Columns["Col_txtdiscount_money"].ReadOnly = true;
            }
            if (this.txtreceive_type_id.Text.Trim() == "02")
            {
                this.GridView1.Columns["Col_txtdiscount_money"].ReadOnly = false;
            }
            this.GridView1.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_total"].Visible = true;  //"Col_txtsum_total";
            this.GridView1.Columns["Col_txtsum_total"].Width = 100;
            this.GridView1.Columns["Col_txtsum_total"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtwant_receive_date"].Visible = false;  //"Col_txtwant_receive_date";
            this.GridView1.Columns["Col_txtwant_receive_date"].Width = 0;
            this.GridView1.Columns["Col_txtwant_receive_date"].ReadOnly = false;
            this.GridView1.Columns["Col_txtwant_receive_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwant_receive_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GridView1.Controls.Add(dtp1);
            //dtp1.Visible = false;
            //dtp1.Format = DateTimePickerFormat.Custom;
            //dtp1.TextChanged += new EventHandler(dtp1_TextChange);

            this.GridView1.Columns["Col_txtmade_receive_date"].Visible = false;  //"Col_txtmade_receive_date";
            this.GridView1.Columns["Col_txtmade_receive_date"].Width = 0;
            this.GridView1.Columns["Col_txtmade_receive_date"].ReadOnly = false;
            this.GridView1.Columns["Col_txtmade_receive_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmade_receive_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GridView1.Controls.Add(dtp2);
            //dtp2.Visible = false;
            //dtp2.Format = DateTimePickerFormat.Custom;
            //dtp2.TextChanged += new EventHandler(dtp2_TextChange);

            this.GridView1.Columns["Col_txtexpire_receive_date"].Visible = false;  //"Col_txtexpire_receive_date";
            this.GridView1.Columns["Col_txtexpire_receive_date"].Width = 0;
            this.GridView1.Columns["Col_txtexpire_receive_date"].ReadOnly = false;
            this.GridView1.Columns["Col_txtexpire_receive_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtexpire_receive_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GridView1.Controls.Add(dtp3);
            //dtp3.Visible = false;
            //dtp3.Format = DateTimePickerFormat.Custom;
            //dtp3.TextChanged += new EventHandler(dtp3_TextChange);

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

            this.GridView1.Columns["Col_txtqty_balance_yokpai"].Visible = false;  //"Col_txtqty_balance_yokpai";
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";
            this.GridView1.Columns["Col_mat_status"].Width = 0;
            this.GridView1.Columns["Col_mat_status"].ReadOnly = true;
            this.GridView1.Columns["Col_mat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_mat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_receive_yokpai"].Visible = false;  //"Col_txtqty_receive_yokpai";
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_receive_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[5].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
                //}
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[9].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
                //}

                GridView1.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtmade_receive_date"].Style.BackColor = Color.LightSkyBlue;
                GridView1.Rows[i].Cells["Col_txtexpire_receive_date"].Style.BackColor = Color.LightSkyBlue;

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

            double QAbyma2 = 0;
            double Qbypai2 = 0;
            double C1 = 0;
            double C1YP = 0;

            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value = k.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = ".0000";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty2"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = "0";
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
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value = "0";
                    }
                    //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())))
                    //{
                    //    MessageBox.Show("จำนวนรับ :  " + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "    มากกว่า จำนวนค้างรับ :  " + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) + "  !!! ระบบจะใส่จำนวนค้างรับให้เลย ");
                    //    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    //}


                    //5 * 6 = 8

                    this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value).ToString("###,###.00");     //5
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value).ToString("###,###.00");     //6
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

                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8

                    //Sum_Qty_Yokma  =================================================
                    Sum_Qty_Yokma = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_Yokma)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString()));
                    this.txtsum_qty_yokma.Text = Sum_Qty_Yokma.ToString("N", new CultureInfo("en-US"));

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  =================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));


                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n4}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n4}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n4}", MoneySum)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                        this.txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }


                    //สำหรับสถานะของบิล PO ว่ารับไปแล้ว เท่าไร   เหลือค้างรับอีกเท่าไร เลยต้องบวกกลับ =================================================
                    //จำนวนรับแล้ว ยกไป
                    Sum_Qty_RECEive_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_receive_yokpai.Text = Sum_Qty_RECEive_Yokpai.ToString("N", new CultureInfo("en-US"));

                    Sum_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_yokpai.Text = Sum_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));
                    //END สำหรับสถานะของบิล PO ว่ารับไปแล้ว เท่าไร   เหลือค้างรับอีกเท่าไร เลยต้องบวกกลับ =================================================

                    //  ===========================================================================================================
                    //รายละเอียด Detail จำนวนค้างรับ ยกไป
                    Sum_Qty_bl_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Sum_Qty_bl_Yokpai.ToString("N", new CultureInfo("en-US"));
                    //รายละเอียด Detail จำนวนรับแล้ว ยกไป
                    Sum_Qty_REceive_bl_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value = Sum_Qty_REceive_bl_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N4", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));
                    }


                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                    this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                    //this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                    QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtprice.Text.ToString()));
                    this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                    //1.เหลือยกมา + ผลิต = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา + มูลค่าผลิต = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString()));
                    this.txtcost_money_sum_yokpai.Text = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) / Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString()));
                        this.txtcost_qty_price_average_yokpai.Text = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.txtcost_qty_price_average_yokpai.Text = "0";
                    }

                    //1.เหลือ(2)ยกมา + ผลิต(2) = จำนวนเหลือ(2)ทั้งสิ้น
                    Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));
                    this.txtcost_qty2_balance_yokpai.Text = Qbypai2.ToString("N", new CultureInfo("en-US"));

                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                    //  ===========================================================================================================

                    //C1==================================
                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        C1 = Convert.ToDouble(string.Format("{0:n4}", C1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtcost_qty1_balance.Text = C1.ToString("N", new CultureInfo("en-US"));
                        C1YP = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance.Text.ToString()));
                        this.txtcost_qty1_balance_yokpai.Text = C1YP.ToString("N", new CultureInfo("en-US"));
                    }
                    //==================================

                }
            }

            this.txtcount_rows.Text = k.ToString();

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

             QAbyma2 = 0;
             Qbypai2 = 0;
             C1 = 0;
             C1YP = 0;

        }
        private void GridView1_Cal_Sum_For_cancel()
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

            double QAbyma2 = 0;
            double Qbypai2 = 0;
            double C1 = 0;
            double C1YP = 0;

            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value = k.ToString();
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value = ".0000";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty2"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = "0";
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
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = "0";
                    }
                    if (this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value = "0";
                    }
                    //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())))
                    //{
                    //    MessageBox.Show("จำนวนรับ :  " + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "    มากกว่า จำนวนค้างรับ :  " + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) + "  !!! ระบบจะใส่จำนวนค้างรับให้เลย ");
                    //    this.GridView1.Rows[i].Cells["Col_txtqty"].Value = this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString();
                    //}


                    //5 * 6 = 8

                    this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value).ToString("###,###.00");     //5
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value).ToString("###,###.00");     //6
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

                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value).ToString("###,###.00");     //8

                    //Sum_Qty_Yokma  =================================================
                    Sum_Qty_Yokma = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_Yokma)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString()));
                    this.txtsum_qty_yokma.Text = Sum_Qty_Yokma.ToString("N", new CultureInfo("en-US"));

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  =================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));


                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n4}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n4}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n4}", MoneySum)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                        this.txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }


                    //สำหรับสถานะของบิล PO ว่ารับไปแล้ว เท่าไร   เหลือค้างรับอีกเท่าไร เลยต้องบวกกลับ =================================================
                    //จำนวนรับแล้ว ยกไป
                    Sum_Qty_RECEive_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_receive_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_receive_yokpai.Text = Sum_Qty_RECEive_Yokpai.ToString("N", new CultureInfo("en-US"));

                    Sum_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_yokma.Text.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtsum_qty_yokpai.Text = Sum_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));
                    //END สำหรับสถานะของบิล PO ว่ารับไปแล้ว เท่าไร   เหลือค้างรับอีกเท่าไร เลยต้องบวกกลับ =================================================

                    //  ===========================================================================================================
                    //รายละเอียด Detail จำนวนค้างรับ ยกไป
                    Sum_Qty_bl_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value = Sum_Qty_bl_Yokpai.ToString("N", new CultureInfo("en-US"));
                    //รายละเอียด Detail จำนวนรับแล้ว ยกไป
                    Sum_Qty_REceive_bl_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value = Sum_Qty_REceive_bl_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N4", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));
                    }


                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                    this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                    //this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                    QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtprice.Text.ToString()));
                    this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                    //1.เหลือยกมา - รับ = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                    this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา + มูลค่ารับ = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString()));
                    this.txtcost_money_sum_yokpai.Text = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) / Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString()));
                        this.txtcost_qty_price_average_yokpai.Text = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.txtcost_qty_price_average_yokpai.Text = "0";
                    }

                    //1.เหลือ(2)ยกมา - รับ(2) = จำนวนเหลือ(2)ทั้งสิ้น
                    Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));
                    this.txtcost_qty2_balance_yokpai.Text = Qbypai2.ToString("N", new CultureInfo("en-US"));

                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                    //  ===========================================================================================================
                    //C1==================================
                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                    {
                        C1 = Convert.ToDouble(string.Format("{0:n4}", C1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtcost_qty1_balance.Text = C1.ToString("N", new CultureInfo("en-US"));
                        C1YP = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance.Text.ToString()));
                        this.txtcost_qty1_balance_yokpai.Text = C1YP.ToString("N", new CultureInfo("en-US"));
                    }
                    //==================================
                }
            }

            this.txtcount_rows.Text = k.ToString();

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

            QAbyma2 = 0;
            Qbypai2 = 0;
             C1 = 0;
             C1YP = 0;

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

                            this.txtcost_qty1_balance_yokma.Text = Convert.ToSingle(dt2.Rows[j]["txtcost_qty1_balance"]).ToString("###,###.00");        //18
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
        private void GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }

        }
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void Sum_group_tax()
        {

            this.txtmoney_sum.Text = this.txtsum_total.Text;

            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_EX")  //ซื้อคิดvatแยก
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_IN") //ซื้อคิดvatรวม
            {
                double DisCount = 0;
                double VATMONey = 0;
                double VATBASE = 0;
                double VATA = 0;

                //รวมทั้งสิ้น
                DisCount = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text));
                this.txtmoney_after_vat.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                VATA = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + 100;

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text)) * Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) / Convert.ToDouble(string.Format("{0:n4}", VATA));
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //ฐานภาษี
                VATBASE = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text));
                this.txtmoney_tax_base.Text = VATBASE.ToString("N", new CultureInfo("en-US"));


            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_NOvat")  //ซื้อไม่มีvat
            {
                double DisCount = 0;
                double VATMONey = 0;
                double MONeyAF_VAT = 0;

                this.txtvat_rate.Text = "0";

                //ฐานภาษี
                DisCount = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));


            }
        }
        private void GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
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
        private void Fill_Project()
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
                                  " FROM k013_1db_acc_07project" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtproject_id = '" + PANEL1307_PROJECT_txtproject_id.Text.Trim() + "')" +
                                  " ORDER BY ID ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.PANEL1307_PROJECT_txtproject_name.Text = dt2.Rows[0]["txtproject_name"].ToString();      //1
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
        private void Fill_Job()
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
                                   " FROM k013_1db_acc_17job" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtjob_id = '" + PANEL1317_JOB_txtjob_id.Text.Trim() + "')" +
                                   " ORDER BY ID ASC";


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        this.PANEL1317_JOB_txtjob_name.Text = dt2.Rows[0]["txtjob_name"].ToString();      //2

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
        private void Fill_EMP()
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


            string clearText_txtemp_id = this.PANEL003_EMP_txtemp_id.Text.Trim();
            string cipherText_txtemp_id = W_CryptorEngine.Encrypt(clearText_txtemp_id, true);

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                   " FROM a003db_user" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (txtemp_id = '" + cipherText_txtemp_id.Trim() + "')" +
                                   " ORDER BY ID ASC";


                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        string clearText_txtemp_name = dt2.Rows[0]["txtname"].ToString();
                        string cipherText_txtemp_name = W_CryptorEngine.Decrypt(clearText_txtemp_name, true);

                        this.PANEL003_EMP_txtemp_name.Text = cipherText_txtemp_name.ToString();      //2

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
            //END เชื่อมต่อฐานข้อมูล====================================================

            //เช็คจำนวนหน้าถัดไป
            //==============================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd2 = conn.CreateCommand();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = conn;


                    cmd2.CommandText = "SELECT c002_01berg_produce_record.*," +
                                       "c002_01berg_produce_record_detail.*" +

                                       " FROM c002_01berg_produce_record" +
                                       " INNER JOIN c002_01berg_produce_record_detail" +
                                       " ON c002_01berg_produce_record.cdkey = c002_01berg_produce_record_detail.cdkey" +
                                       " AND c002_01berg_produce_record.txtco_id = c002_01berg_produce_record_detail.txtco_id" +

                                       " WHERE (c002_01berg_produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_01berg_produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (c002_01berg_produce_record.txtic_status = '0')" +
                                       " AND (c002_01berg_produce_record_detail.txtLot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                       " AND (c002_01berg_produce_record_detail.txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')" +
                                       " ORDER BY c002_01berg_produce_record_detail.txtmat_no ASC";

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

                                MessageBox.Show("Lot no :   "  + dt2.Rows[j]["txtLot_no"].ToString()  + "    นี้ มีการเบิกเข้าเครื่องจักร ไปแล้ว ไม่สามารถยกเลิกรายการได้ !!! ");
                                return;
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
            //==============================================
            //==================================================================


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

                cmd1.CommandText = "SELECT * FROM c003_receive_record" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtCRG_id = '" + this.txtCRG_id.Text.Trim() + "')" +
                                    " AND (txtcrg_status = '1')";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("เอกสารนี้   : '" + this.txtCRG_id.Text.Trim() + "' ยกเลิกไปแล้ว ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    conn.Close();
                    return;
                }
            }

            //
            conn.Close();

            //จบเชื่อมต่อฐานข้อมูล=======================================================

            Show_Qty_Yokma();
            GridView1_Cal_Sum_For_cancel();
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

                        cmd2.CommandText = "INSERT INTO c003_receive_record_cancel(cdkey,txtco_id,txtbranch_id," +  //1
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
                        cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 1));
                        cmd2.Parameters.Add("@cancel_id", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok1");

                        //2
                        cmd2.CommandText = "UPDATE c003_receive_record" +
                                                                    " SET txtcrg_status = '1'," +
                                                                    "txtmoney_after_vat_creditor = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                                    "txtcreditor_status = '1'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtCRG_id = '" + this.txtCRG_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok2");

                        //3
                        cmd2.CommandText = "UPDATE k017db_pr_record" +
                                                                    " SET txtRG_status = ''," +
                                                                     "txtRG_id = ''" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtRG_id = '" + this.txtCRG_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok3");

                        //4
                        cmd2.CommandText = "UPDATE k018db_po_record" +
                                                                    " SET txtRG_status = ''," +
                                                                     "txtRG_id = ''" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtRG_id = '" + this.txtCRG_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok4");


                        //5
                        cmd2.CommandText = "UPDATE k017db_pr_all" +
                                                                    " SET txtRG_status = ''," +
                                                                     "txtRG_id = ''" +
                                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtRG_id = '" + this.txtCRG_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok5");

                        //5
                    }

                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {
                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();
                            if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                            {
                                //===================================================================================================================
                                //===================================================================================================================
                                //3 k018db_po_record_detail  ยอดค้างรับ
                                cmd2.CommandText = "UPDATE k018db_po_record_detail SET " +
                                                   "txtqty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtpo_id = '" + this.txtPo_id.Text.Trim() + "')" +
                                                   " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok56");


                                //===================================================================================================================
                                //4 k017db_pr_all_detail ยอดค้างรับ
                                cmd2.CommandText = "UPDATE k017db_pr_all_detail SET txtRG_id = ''," +
                                                   "txtqty_rg = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_receive_yokpai"].Value.ToString())) + "'," +
                                                   "txtqty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtpo_id = '" + this.txtPo_id.Text.Trim() + "')" +
                                                   " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok66");



                                //========================================================
                                //5 k017db_pr_all_detail_balance ==============================================================================================

                                cmd2.CommandText = "INSERT INTO k017db_pr_all_detail_balance(cdkey,txtco_id,txtbranch_id," +  //1
                               "txttrans_date_server,txttrans_time," +  //2
                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                               "txtcomputer_ip,txtcomputer_name," +  //4
                                "txtuser_name,txtemp_office_name," +  //5
                               "txtversion_id," +  //6
                                //====================================================

                                   "txtpr_id," +  //7
                                   "txtpo_id," +  //8
                                   "txtapprove_id," +  //9
                                   "txtRG_id," +  //10
                                   "txtreceive_id," +  //11
                                   "txtbill_remark," +  //12
                                   "txtwant_receive_date," +  //13

                                   "txtmat_no," +  //14
                                   "txtmat_id," +  //15
                                   "txtmat_name," +  //16
                                   "txtmat_unit1_name," +  //17

                                   "txtprice," +   //18
                                   "txtdiscount_rate," +  //19
                                   "txtdiscount_money," +  //20
                                   "txtsum_total," +  //21
                                   "txtitem_no," +  //22

                                    "txtqty_pr," +  //23
                                   "txtqty_po," +  //24

                                   "txtqty_approve," +  //25
                                   "txtqty_rg," +  //26
                                   "txtqty_balance," +  //27

                                   "txtqty_receive) " +  //28

                            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                            "'" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "','" + myDateTime2.ToString("HH:mm:ss", UsaCulture) + "'," +  //2
                            "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "','" + DateTime.Now.ToString("yyyy-MM-dd", UsaCulture) + "'," +  //3
                            "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                            "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                            "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                          //=======================================================


                            "'" + this.txtPr_id.Text.Trim() + "'," +  //7
                            "'" + this.txtPo_id.Text.Trim() + "'," +  //8
                            "'" + this.txtapprove_id.Text.Trim() + "'," +  //9
                            "'" + this.txtCRG_id.Text.Trim() + "'," +  //10
                            "'CANCEL'," +  //11
                            "'ยกเลิกรับสินค้า" + this.txtrg_remark.Text.Trim() + "'," +  //12
                            "'" + this.GridView1.Rows[i].Cells["Col_txtwant_receive_date"].Value.ToString() + "'," +  //13

                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //14
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //15
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //16
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //17
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //18
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //19
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //20
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //21
                            "'" + this.GridView1.Rows[i].Cells["Col_Auto_num"].Value.ToString() + "'," +  //22

                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value.ToString())) + "'," +  //23
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value.ToString())) + "'," +  //24

                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_want"].Value.ToString())) + "'," +  //25
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //26
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_balance_yokpai"].Value.ToString())) + "'," +  //27

                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "')";   //28

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok76");

                                //====================================================================================================
                            }
                        }
                    }
                    //MessageBox.Show("ok6");



                    //สต๊อคสินค้า ตามคลัง =============================================================================================


                                //1.k021_mat_average
                                //1.k021_mat_average
                                cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                                  "txtcost_qty1_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokpai.Text.ToString())) + "'," +
                                                  "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +
                                                   "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +
                                                    "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +
                                                   "txtcost_qty2_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'" +
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

                                               "txtqty1_in," +  //18
                                             "txtqty_in," +  //18
                                               "txtqty2_in," +  //19
                                              "txtprice_in," +   //20
                                               "txtsum_total_in," +  //21

                                              "txtqty1_out," +  //22
                                              "txtqty_out," +  //22
                                              "txtqty2_out," +  //23
                                              "txtprice_out," +  //24
                                               "txtsum_total_out," +  //25

                                               "txtqty1_balance," +  //26
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


                                        "'" + this.txtCRG_id.Text.Trim() + "'," +  //7 txtbill_id
                                        "'CRG'," +  //9 txtbill_type
                                        "'ยกเลิกรับด้ายจาก " + this.PANEL161_SUP_txtsupplier_name.Text.Trim() + "'," +  //9 txtbill_remark

                                         "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                                       "'" + this.txtmat_no.Text + "'," +  //10 
                                        "'" + this.PANEL_MAT_txtmat_id.Text.ToString() + "'," +  //11
                                        "'" + this.PANEL_MAT_txtmat_name.Text.ToString() + "'," +    //12

                                        "'" + this.txtmat_unit1_name.Text.ToString() + "'," +  //13
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString())) + "'," +  //14
                                        "'" + this.chmat_unit_status.Text.ToString() + "'," +  //15
                                        "'" + this.txtmat_unit2_name.Text.ToString() + "'," +  //16
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +  //17

                                         "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty1_in
                                     "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty_in
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19 txtqty2_in
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //20 txtprice_in
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //21 txtsum_total_in

                                          "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance.Text.ToString())) + "'," +  //22 txtqty1_out
                                    "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) + "'," +  //22 txtqty_out
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString())) + "'," +  //23 txtqty2_out
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtprice.Text.ToString())) + "'," +  //24 txtprice_out
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                                        "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty1_balance_yokpai.Text.ToString())) + "'," +  //26
                                      "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +  //26
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'," +  //27
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +  //28
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +  //29

                                       "'1')";   //30

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok87");


                                //======================================

                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    //MessageBox.Show("ok4");

                    cmd2.CommandText = "UPDATE k017db_pr_all SET txtRG_id = ''," +
                                      "txtRG_date = ''," +
                                       //"txtsupplier_id = ''," +
                                       //"txtsupplier_name = ''," +
                                       "txtsum_qty_receive = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_receive_yokpai.Text.ToString())) + "'," +
                                       "txtsum_qty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_yokpai.Text.ToString())) + "'," +
                                       "txtRG_status = ''" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtPo_id = '" + this.txtPo_id.Text.Trim() + "')";

                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok8");

                    //6
                    cmd2.CommandText = "UPDATE k017db_pr_record SET txtRG_id = ''," +
                                       "txtRG_date = ''," +
                                       "txtRG_status = ''" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtPr_id = '" + this.txtPr_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok9");

                    cmd2.CommandText = "UPDATE k018db_po_record SET txtRG_id = ''," +
                                      "txtRG_date = ''," +
                                       "txtsum_qty_receive = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_receive_yokpai.Text.ToString())) + "'," +
                                       "txtsum_qty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_yokpai.Text.ToString())) + "'," +
                                       "txtRG_status = ''" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtPo_id = '" + this.txtPo_id.Text.Trim() + "')";

                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok10");
                    //8
                    cmd2.CommandText = "UPDATE k019db_approve_record SET txtRG_id = ''," +
                                       "txtRG_date = ''," +
                                       "txtrg_status = ''" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtApprove_id = '" + this.txtapprove_id.Text.Trim() + "')";
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok11");

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtCRG_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
        //=============================================================
    private void btnPreview_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtCRG_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_01RG_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_01RG_record_print();
            frm2.Show();
            frm2.BringToFront();

            //======================================================

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtCRG_id.Text.Trim();
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

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_Chart_of_accounts.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c003_receive_record.rpt");


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
                rpt.SetParameterValue("txtCRG_id", W_ID_Select.TRANS_ID.Trim());

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
            //=========================================================================================================================================
        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    cmd2.CommandText = "UPDATE c003_receive_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtCRG_id = '" + this.txtCRG_id.Text.Trim() + "')";
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


        //=================================================
    }
}
