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

            this.iblword_status.Text = "ดูข้อมูลใบรับผ้าพับ";

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
            GridView1_Cal_Sum();
            GridView1_Cal_Sum_For_Cancel();
            GridView1_Color_Column();

            Show_GridView2();
            Fill_Show_DATA_GridView2();

            Show_Qty_Yokma();
            Show_Qty_Yokma2();
            GridView2_Cal_Sum_M();
            GridView2_Cal_Sum();
            GridView2_Cal_Sum_For_Cancel();

            Sum_group_tax();

        }

 
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow = 0;

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
                                   " AND c002_07Receive_Send_dye_record.txtFG2_id = c002_07Receive_Send_dye_record_detail.txtFG2_id" +

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
                                   " AND (c002_07Receive_Send_dye_record.txtFG2_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                   " ORDER BY c002_07Receive_Send_dye_record_detail.txtnumber_in_year,c002_07Receive_Send_dye_record_detail.txtfold_number ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.txtFG2_id.Text = dt2.Rows[0]["txtFG2_id"].ToString();

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

                        this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_client"].ToString());
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


                        this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = dt2.Rows[0]["txtface_baking_id"].ToString();
                        this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = dt2.Rows[0]["txtface_baking_name"].ToString();

                        //this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = dt2.Rows[0]["txtnumber_mat_id"].ToString();
                        //this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[0]["txtnumber_mat_name"].ToString();

                        this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text = dt2.Rows[0]["txtnumber_color_id"].ToString();
                        this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text = dt2.Rows[0]["txtnumber_color_name"].ToString();

                        //this.PANEL1316_DEPARTMENT_txtdepartment_id.Text = dt2.Rows[0]["txtdepartment_id"].ToString();
                        //this.PANEL1316_DEPARTMENT_txtdepartment_name.Text = dt2.Rows[0]["txtdepartment_name"].ToString();


                        //this.PANEL1307_PROJECT_txtproject_id.Text = dt2.Rows[0]["txtproject_id"].ToString();
                        //this.PANEL1307_PROJECT_txtproject_name.Text = dt2.Rows[0]["txtproject_name"].ToString();

                        //this.PANEL1317_JOB_txtjob_id.Text = dt2.Rows[0]["txtjob_id"].ToString();
                        //this.PANEL1317_JOB_txtjob_name.Text = dt2.Rows[0]["txtjob_name"].ToString();

                        this.txtVat_id.Text = dt2.Rows[0]["txtVat_id"].ToString();
                        //this.txtVat_date.Text = dt2.Rows[0]["txtVat_date"].ToString();
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


                        //this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = dt2.Rows[0]["txtnumber_mat_id"].ToString();
                        //this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[0]["txtnumber_mat_name"].ToString();

                        //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        //this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = "ซื้อไม่มีvat";
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = "PUR_NOvat";

                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");

                        this.txtsum_qty_yokma.Text = dt2.Rows[0]["txtsum_qty_balance"].ToString();  //ไว้สำหรับคำนวณว่า ค้างรับ จำนวนเท่าไร

                        Int32 k = 0;

                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            k = j + 1;
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = k.ToString("000"); //0
                            GridView1.Rows[index].Cells["Col_txtPPT_id"].Value = dt2.Rows[j]["txtPPT_id"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtnumber_dyed"].Value = dt2.Rows[j]["txtnumber_dyed"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //18
                            GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();     //41
                            GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();     //42

                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //15
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //16
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //17

                            GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //19
                            GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //20
                            GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //21
                            GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //22
                            GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //23

                            GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //4
                            GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");      //24

                            GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //25
                            GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //26
                            GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //27
                            GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //28

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //29
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";         //30
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";        //31

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";         //32
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";        //33
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";        //34

                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = "0";       //35
                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = "0";         //36

                            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = "0";         //36
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokma"].Value = "0";         //36
                            GridView1.Rows[index].Cells["Col_txtqty_cut_yokpai"].Value = "0";         //36
                            GridView1.Rows[index].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";         //36
                            GridView1.Rows[index].Cells["Col_1"].Value = "1";      //37

                            GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();         //36
                            GridView1.Rows[index].Cells["Col_txtnumber_sup_color_id"].Value = dt2.Rows[j]["txtnumber_sup_color_id"].ToString();         //36

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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

            //Fill_cboemp();

        }


        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 41;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtPPT_id";
            this.GridView1.Columns[2].Name = "Col_txtnumber_dyed";
            this.GridView1.Columns[3].Name = "Col_txtnumber_in_year";
            this.GridView1.Columns[4].Name = "Col_txtsupplier_id";
            this.GridView1.Columns[5].Name = "Col_txtsupplier_name";
            this.GridView1.Columns[6].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[7].Name = "Col_txtmachine_id";
            this.GridView1.Columns[8].Name = "Col_txtfold_number";
            this.GridView1.Columns[9].Name = "Col_txtnumber_mat_id";
            this.GridView1.Columns[10].Name = "Col_txtface_baking_id";
            this.GridView1.Columns[11].Name = "Col_txtlot_no";

            this.GridView1.Columns[12].Name = "Col_txtmat_no";
            this.GridView1.Columns[13].Name = "Col_txtmat_id";
            this.GridView1.Columns[14].Name = "Col_txtmat_name";

            this.GridView1.Columns[15].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[16].Name = "Col_txtmat_unit1_qty";
            this.GridView1.Columns[17].Name = "Col_chmat_unit_status";
            this.GridView1.Columns[18].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[19].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[20].Name = "Col_txtqty";
            this.GridView1.Columns[21].Name = "Col_txtqty2";

            this.GridView1.Columns[22].Name = "Col_txtprice";
            this.GridView1.Columns[23].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[24].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[25].Name = "Col_txtsum_total";

            this.GridView1.Columns[26].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[27].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[28].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[29].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[30].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[31].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[32].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns[33].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView1.Columns[34].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[35].Name = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[36].Name = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[37].Name = "Col_txtqty_after_cut_yokpai";

            this.GridView1.Columns[38].Name = "Col_1";
            this.GridView1.Columns[39].Name = "Col_txtnumber_color_id";
            this.GridView1.Columns[40].Name = "Col_txtnumber_sup_color_id";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "เลขที่ FG1";
            this.GridView1.Columns[2].HeaderText = "เบอร์กอง";
            this.GridView1.Columns[3].HeaderText = "เลขชุดที่";
            this.GridView1.Columns[4].HeaderText = "รหัส Sup";
            this.GridView1.Columns[5].HeaderText = "Supplier";
            this.GridView1.Columns[6].HeaderText = "คลัง";
            this.GridView1.Columns[7].HeaderText = "เครื่องจักร";
            this.GridView1.Columns[8].HeaderText = "ม้วนที่";
            this.GridView1.Columns[9].HeaderText = "เบอร์ด้าย";
            this.GridView1.Columns[10].HeaderText = "อบหน้า";
            this.GridView1.Columns[11].HeaderText = "Lot No";

            this.GridView1.Columns[12].HeaderText = "ลำดับ";
            this.GridView1.Columns[13].HeaderText = "รหัส";
            this.GridView1.Columns[14].HeaderText = "ชื่อสินค้า";

            this.GridView1.Columns[15].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[16].HeaderText = " หน่วย";
            this.GridView1.Columns[17].HeaderText = "แปลง";
            this.GridView1.Columns[18].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[19].HeaderText = " หน่วย2";

            this.GridView1.Columns[20].HeaderText = "น้ำหนัก(กก.)";
            this.GridView1.Columns[21].HeaderText = "น้ำหนัก/ม้วน(ปอนด์)";

            this.GridView1.Columns[22].HeaderText = "ราคา";
            this.GridView1.Columns[23].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[24].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[25].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[26].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[27].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[28].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[29].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[30].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[31].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[32].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView1.Columns[33].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView1.Columns[34].HeaderText = "ยอดยกมา";
            this.GridView1.Columns[35].HeaderText = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[36].HeaderText = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[37].HeaderText = "คงเหลือยกไป";

            this.GridView1.Columns[38].HeaderText = "1";  //ไว้นับจำนวน
            this.GridView1.Columns[39].HeaderText = "รหัสสี";
            this.GridView1.Columns[40].HeaderText = "รหัสสี Supplier";

            this.GridView1.Columns["Col_Auto_num"].Visible = true;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width = 40;
            this.GridView1.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView1.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtPPT_id"].Visible = false;  //"Col_txtPPT_id";
            this.GridView1.Columns["Col_txtPPT_id"].Width = 0;
            this.GridView1.Columns["Col_txtPPT_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtPPT_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtPPT_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_dyed"].Visible = true;  //"Col_txtnumber_dyed";
            this.GridView1.Columns["Col_txtnumber_dyed"].Width = 90;
            this.GridView1.Columns["Col_txtnumber_dyed"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_dyed"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_dyed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_in_year"].Visible = true;  //"Col_txtnumber_in_year";
            this.GridView1.Columns["Col_txtnumber_in_year"].Width = 90;
            this.GridView1.Columns["Col_txtnumber_in_year"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_in_year"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_in_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsupplier_id"].Visible = false;  //"Col_txtsupplier_id";
            this.GridView1.Columns["Col_txtsupplier_id"].Width = 0;
            this.GridView1.Columns["Col_txtsupplier_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsupplier_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsupplier_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsupplier_name"].Visible = true;  //"Col_txtsupplier_name";
            this.GridView1.Columns["Col_txtsupplier_name"].Width = 100;
            this.GridView1.Columns["Col_txtsupplier_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsupplier_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsupplier_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView1.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmachine_id"].Visible = false;  //"Col_txtmachine_id";
            this.GridView1.Columns["Col_txtmachine_id"].Width = 0;
            this.GridView1.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView1.Columns["Col_txtfold_number"].Width = 60;
            this.GridView1.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView1.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = true;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 80;
            this.GridView1.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView1.Columns["Col_txtface_baking_id"].Width = 90;
            this.GridView1.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtlot_no"].Visible = true;  //"Col_txtlot_no";
            this.GridView1.Columns["Col_txtlot_no"].Width = 180;
            this.GridView1.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";
            this.GridView1.Columns["Col_txtmat_no"].Width = 0;
            this.GridView1.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_id"].Visible = false;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 0;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 120;
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



            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty2"].Visible = false;  //"Col_txtqty2";
            this.GridView1.Columns["Col_txtqty2"].Width = 0;
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

            this.GridView1.Columns["Col_txtqty_after_cut"].Visible = false;  //"Col_txtqty_after_cut";
            this.GridView1.Columns["Col_txtqty_after_cut"].Width = 0;
            this.GridView1.Columns["Col_txtqty_after_cut"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_cut_yokma"].Visible = false;  //"Col_txtqty_cut_yokma";
            this.GridView1.Columns["Col_txtqty_cut_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtqty_cut_yokpai"].Visible = false;  //"Col_txtqty_cut_yokpai";
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].Visible = false;  //"Col_txtqty_after_cut_yokpai";
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].Width = 0;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_1"].Visible = false;  //"Col_1";
            this.GridView1.Columns["Col_1"].Width = 0;
            this.GridView1.Columns["Col_1"].ReadOnly = true;
            this.GridView1.Columns["Col_1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_color_id"].Visible = true;  //"Col_txtnumber_color_id";
            this.GridView1.Columns["Col_txtnumber_color_id"].Width = 90;
            this.GridView1.Columns["Col_txtnumber_color_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_color_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_color_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_sup_color_id"].Visible = true;  //"Col_txtnumber_sup_color_id";
            this.GridView1.Columns["Col_txtnumber_sup_color_id"].Width = 90;
            this.GridView1.Columns["Col_txtnumber_sup_color_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_sup_color_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_sup_color_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
        private void GridView1_SelectionChanged(object sender, EventArgs e)
        {
            curRow = GridView1.CurrentRow.Index;
            int rowscount = GridView1.Rows.Count;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            //===============================================================
            //===============================================================

            //======================================

            //======================================



        }
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                GridView1.Rows[i].Cells["Col_txtnumber_dyed"].Style.BackColor = Color.GreenYellow;
                //Col_Chk_SELECT    Col_date
                //GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Style.BackColor = Color.Black;
                //GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Style.ForeColor = Color.LightGreen;

                //GridView1.Rows[i].Cells["Col_txtlot_no"].Style.BackColor = Color.Blue;
                //GridView1.Rows[i].Cells["Col_txtlot_no"].Style.ForeColor = Color.White;

                //GridView1.Rows[i].Cells["Col_txtfold_number"].Style.BackColor = Color.LightGoldenrodYellow;


                GridView1.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.LightSkyBlue;

            }
        }
        private void GridView1_Cal_Sum()
        {

            double Sum2_Qty_Yokpai = 0;
            double Sum2_Qty = 0;
            double Con_QTY = 0;

            double Sum_Qty_CUT_Yokpai = 0;
            double Sum_Qty_AF_CUT_Yokpai = 0;

            double Sum_Qtyx1 = 0;
            double Sum_Qty1 = 0;
            double Sum_Qty2 = 0;
            double Sum_Qty3 = 0;
            double Sum_Qty4 = 0;
            double Sum_Qty5 = 0;

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

                if (this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = ".00";
                }

                if (double.Parse(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                {
                    //======================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    //if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    //{
                        Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  ===================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)==============================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    //}



                    Sum_Qtyx1 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qtyx1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                    this.txtcount_rows.Text = Sum_Qtyx1.ToString("N", new CultureInfo("en-US"));


                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        Sum_Qty1 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtsum_qty_rib.Text = Sum_Qty1.ToString("N", new CultureInfo("en-US"));

                        Sum_Qty2 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty2)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty_rib_kg.Text = Sum_Qty2.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        Sum_Qty3 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty3)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtsum_qty_pub.Text = Sum_Qty3.ToString("N", new CultureInfo("en-US"));

                        Sum_Qty4 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty4)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty_pub_kg.Text = Sum_Qty4.ToString("N", new CultureInfo("en-US"));
                    }

                    Sum_Qty5 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty5)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty5.ToString("N", new CultureInfo("en-US"));


                    //แล้ว เท่าไร = ปกติ บวก  ยกเลิก ลบ ================================================
                    Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //เหลืออีก เท่าไร  ปกติ ลบ  ยกเลิก บวก ===============================================
                    Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));


                }


            }



            Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            Sum_Qty_CUT_Yokpai = 0;
            Sum_Qty_AF_CUT_Yokpai = 0;

            Sum_Qtyx1 = 0;
            Sum_Qty1 = 0;
            Sum_Qty2 = 0;
            Sum_Qty3 = 0;
            Sum_Qty4 = 0;
            Sum_Qty5 = 0;


        }
        private void GridView1_Cal_Sum_For_Cancel()
        {

            double Sum2_Qty_Yokpai = 0;
            double Sum2_Qty = 0;
            double Con_QTY = 0;

            double Sum_Qty_CUT_Yokpai = 0;
            double Sum_Qty_AF_CUT_Yokpai = 0;

            double Sum_Qtyx1 = 0;
            double Sum_Qty1 = 0;
            double Sum_Qty2 = 0;
            double Sum_Qty3 = 0;
            double Sum_Qty4 = 0;
            double Sum_Qty5 = 0;

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

                if (this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = ".00";
                }

                if (double.Parse(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                {
                    //======================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    //if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    //{
                        Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  ===================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)==============================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    //}



                    Sum_Qtyx1 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qtyx1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                    this.txtcount_rows.Text = Sum_Qtyx1.ToString("N", new CultureInfo("en-US"));


                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        Sum_Qty1 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtsum_qty_rib.Text = Sum_Qty1.ToString("N", new CultureInfo("en-US"));

                        Sum_Qty2 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty2)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty_rib_kg.Text = Sum_Qty2.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        Sum_Qty3 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty3)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                        this.txtsum_qty_pub.Text = Sum_Qty3.ToString("N", new CultureInfo("en-US"));

                        Sum_Qty4 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty4)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                        this.txtsum_qty_pub_kg.Text = Sum_Qty4.ToString("N", new CultureInfo("en-US"));
                    }

                    Sum_Qty5 = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty5)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty5.ToString("N", new CultureInfo("en-US"));


                    //แล้ว เท่าไร = ปกติ บวก  ยกเลิก ลบ ================================================
                    Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //เหลืออีก เท่าไร  ปกติ ลบ  ยกเลิก บวก ===============================================
                    Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));


                }


            }



            Sum2_Qty_Yokpai = 0;
            Con_QTY = 0;

            Sum_Qty_CUT_Yokpai = 0;
            Sum_Qty_AF_CUT_Yokpai = 0;

            Sum_Qtyx1 = 0;
            Sum_Qty1 = 0;
            Sum_Qty2 = 0;
            Sum_Qty3 = 0;
            Sum_Qty4 = 0;
            Sum_Qty5 = 0;


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

                for (int i = 0; i < this.GridView2.Rows.Count; i++)
                {

                    var valu = this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                    if (valu != "")
                    {
                        cmd2.CommandText = "SELECT *" +
                                                               " FROM k021_mat_average" +
                                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                                               " AND (txtmat_id = '" + this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')" +
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
                                    this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty1_balance"]).ToString("###,###.00");        //18
                                    this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                                    this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");        //19
                                    this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");        //20
                                    this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00");        //24

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

                    }                      //===========================================
                }

            }

        }
        private void Show_Qty_Yokma2()
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

            //==============================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                //if (this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() == this.GridView66.Rows[selectedRowIndex].Cells["Col_txtlot_no"].Value.ToString())
                //{
                //    MessageBox.Show("Lot No นี้ เพิ่มเข้าไปใน ตารางแล้ว ");
                //    return;
                //}
                //if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() == this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_id"].Value.ToString())
                //{

                //}
                //else
                //{
                //    MessageBox.Show("ระบบจะให้ส่งตัดผ้าพับ ได้ที่ละ 1 รหัสผ้าพับ ต่อ 1 ใบส่งตัด เท่านั้น !! ");
                //    return;
                //}
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd2 = conn.CreateCommand();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = conn;


                    cmd2.CommandText = "SELECT *" +
                                       " FROM c002_05Send_dye_record_detail" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                       //" AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')" +
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

                                GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_cut"]).ToString("###,###.00");    //36
                                GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_cut"]).ToString("###,###.00");          //21
                                //GridView1.Rows[j].Cells["Col_txtqty_cut_yokpai"].Value = "0";      //37
                                //GridView1.Rows[j].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";      //37


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
        private void GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -0)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = GridView1.CurrentRow.Index;
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการ ลบรายการแถว ที่คลิ๊ก ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                //DataGridViewRow row = new DataGridViewRow();
                //row = this.PANEL161_SUP_dataGridView2.Rows[selectedRowIndex];
                this.GridView1.Rows.RemoveAt(selectedRowIndex);

                Show_Qty_Yokma();
                Show_Qty_Yokma2();
                GridView1_Cal_Sum();
                GridView2_Cal_Sum_M();
                GridView2_Cal_Sum();
                Sum_group_tax();

                Cursor.Current = Cursors.Default;

                //MessageBox.Show("ลบ เรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
                Cursor.Current = Cursors.Default;

                //MessageBox.Show("ยังไม่ได้ ลบ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                Cursor.Current = Cursors.Default;

                //MessageBox.Show("ไม่ได้ ลบ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


                cmd2.CommandText = "SELECT k021_mat_average.*," +
                                   "b001mat.*," +
                                    "b001mat_02detail.*," +
                                   "b001_05mat_unit1.*," +
                                   "b001_05mat_unit2.*," +

                                   "b001mat_13point_phurchase.*" +

                                   " FROM k021_mat_average" +

                                   " INNER JOIN b001mat" +
                                   " ON k021_mat_average.cdkey = b001mat.cdkey" +
                                   " AND k021_mat_average.txtco_id = b001mat.txtco_id" +
                                   " AND k021_mat_average.txtmat_id = b001mat.txtmat_id" +

                                   " INNER JOIN b001mat_02detail" +
                                   " ON k021_mat_average.cdkey = b001mat_02detail.cdkey" +
                                   " AND k021_mat_average.txtco_id = b001mat_02detail.txtco_id" +
                                   " AND k021_mat_average.txtmat_id = b001mat_02detail.txtmat_id" +

                                   " INNER JOIN b001_05mat_unit1" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                   " INNER JOIN b001_05mat_unit2" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                   " INNER JOIN b001mat_13point_phurchase" +
                                   " ON k021_mat_average.cdkey = b001mat_13point_phurchase.cdkey" +
                                   " AND k021_mat_average.txtco_id = b001mat_13point_phurchase.txtco_id" +
                                   " AND k021_mat_average.txtmat_id = b001mat_13point_phurchase.txtmat_id" +

                                   " WHERE (k021_mat_average.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (k021_mat_average.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                    //" AND (k021_mat_average.txtwherehouse_id = 'SMN-001')" +
                                    " AND (b001mat_02detail.txtmat_sac_id = '" + this.txtmat_sac_id.Text.Trim() + "')" +   //ผ้าพับ
                                    " AND (b001mat.txtmat_id <> '')" +
                                   " ORDER BY k021_mat_average.txtmat_no ASC";

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

                            var index = GridView2.Rows.Add();
                            GridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView2.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            GridView2.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //2
                            GridView2.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //3
                            GridView2.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //4
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //5
                            GridView2.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");        //6
                            GridView2.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();     //7
                            GridView2.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();     //8
                            GridView2.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //9

                            GridView2.Rows[index].Cells["Col_txtcost_qty_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");      //10
                            GridView2.Rows[index].Cells["Col_txtcost_qty_price_average"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");      //11
                            GridView2.Rows[index].Cells["Col_txtcost_money_sum"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");      //12

                            GridView2.Rows[index].Cells["Col_txtcost_qty2_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance"]).ToString("###,###.00");      //13
                            GridView2.Rows[index].Cells["Col_txtmat_amount_phurchase"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_amount_phurchase"]).ToString("###,###.00");      //14
                            GridView2.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //15

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                    }
                    else
                    {
                        this.txtcount_rows.Text = dt2.Rows.Count.ToString();

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

        private void Show_GridView2()
        {
            this.GridView2.ColumnCount = 34;
            this.GridView2.Columns[0].Name = "Col_Auto_num";
            this.GridView2.Columns[1].Name = "Col_txtwherehouse_id";

            this.GridView2.Columns[2].Name = "Col_txtmat_no";
            this.GridView2.Columns[3].Name = "Col_txtmat_id";
            this.GridView2.Columns[4].Name = "Col_txtmat_name";
            this.GridView2.Columns[5].Name = "Col_txtmat_unit1_name";
            this.GridView2.Columns[6].Name = "Col_txtmat_unit1_qty";

            this.GridView2.Columns[7].Name = "Col_chmat_unit_status";

            this.GridView2.Columns[8].Name = "Col_txtmat_unit2_name";
            this.GridView2.Columns[9].Name = "Col_txtmat_unit2_qty";

            this.GridView2.Columns[10].Name = "Col_txtmat_amount_phurchase";
            this.GridView2.Columns[11].Name = "Col_txtmat_status";

            this.GridView2.Columns[12].Name = "Col_txtcost_qty_balance";
            this.GridView2.Columns[13].Name = "Col_txtcost_qty_price_average";
            this.GridView2.Columns[14].Name = "Col_txtcost_money_sum";
            this.GridView2.Columns[15].Name = "Col_txtcost_qty2_balance";

            this.GridView2.Columns[16].Name = "Col_txtcost_qty1_balance";
            this.GridView2.Columns[17].Name = "Col_txtsum_qty";

            this.GridView2.Columns[18].Name = "Col_txtsum_price";
            this.GridView2.Columns[19].Name = "Col_txtsum_discount";
            this.GridView2.Columns[20].Name = "Col_txtmoney_sum";
            this.GridView2.Columns[21].Name = "Col_txtmoney_tax_base";
            this.GridView2.Columns[22].Name = "Col_txtvat_rate";
            this.GridView2.Columns[23].Name = "Col_txtvat_money";
            this.GridView2.Columns[24].Name = "Col_txtmoney_after_vat";

            this.GridView2.Columns[25].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView2.Columns[26].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView2.Columns[27].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView2.Columns[28].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView2.Columns[29].Name = "Col_txtsum2_qty";
            this.GridView2.Columns[30].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView2.Columns[31].Name = "Col_txtcost_qty1_balance_yokma";
            this.GridView2.Columns[32].Name = "Col_txtcost_qty1_balance_yokpai";

            this.GridView2.Columns[33].Name = "Col_1";



            this.GridView2.Columns[0].HeaderText = "No";
            this.GridView2.Columns[1].HeaderText = "รหัสคลัง";

            this.GridView2.Columns[2].HeaderText = "ลำดับ";
            this.GridView2.Columns[3].HeaderText = " รหัส";
            this.GridView2.Columns[4].HeaderText = " ชื่อสินค้า";
            this.GridView2.Columns[5].HeaderText = " หน่วยหลัก";
            this.GridView2.Columns[6].HeaderText = " หน่วย";
            this.GridView2.Columns[7].HeaderText = "แปลง";
            this.GridView2.Columns[8].HeaderText = " หน่วย2";
            this.GridView2.Columns[9].HeaderText = " หน่วย";

            this.GridView2.Columns[10].HeaderText = "จุดสั่งซื้อ";
            this.GridView2.Columns[11].HeaderText = "สถานะ";

            this.GridView2.Columns[12].HeaderText = "คงเหลือ";
            this.GridView2.Columns[13].HeaderText = "ราคาเฉลี่ย";
            this.GridView2.Columns[14].HeaderText = "มูลค่าเฉลี่ย";
            this.GridView2.Columns[15].HeaderText = "คงเหลือ(หน่วย2)";

            this.GridView2.Columns[16].HeaderText = "รับผ้าพับ(พับ) ";
            this.GridView2.Columns[17].HeaderText = "รับผ้าพับ(กก) ";

            this.GridView2.Columns[18].HeaderText = "ราคา";
            this.GridView2.Columns[19].HeaderText = "ส่วน";
            this.GridView2.Columns[20].HeaderText = "ยอดรวม";
            this.GridView2.Columns[21].HeaderText = "ฐานภาษี";
            this.GridView2.Columns[22].HeaderText = "ภาษี%";
            this.GridView2.Columns[23].HeaderText = "ภาษี";
            this.GridView2.Columns[24].HeaderText = "จำนวนเงิน";

            this.GridView2.Columns[25].HeaderText = "คงเหลือ ยกไป";
            this.GridView2.Columns[26].HeaderText = "ราคาเฉี่ยยกไป";
            this.GridView2.Columns[27].HeaderText = "จำนวนเงินยกไป";

            this.GridView2.Columns[28].HeaderText = "รับผ้าพับ ยกมา";
            this.GridView2.Columns[29].HeaderText = "รับผ้าพับ ปอนด์";
            this.GridView2.Columns[30].HeaderText = "รับผ้าพับ2 ยกไป";

            this.GridView2.Columns[31].HeaderText = "รับผ้าพับ  ยกมา";
            this.GridView2.Columns[32].HeaderText = "รับผ้าพับ  ยกไป";

            this.GridView2.Columns[33].HeaderText = "1";

            this.GridView2.Columns[0].Visible = false;  //"Col_Auto_num";

            this.GridView2.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView2.Columns["Col_Auto_num"].Width = 0;
            this.GridView2.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView2.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_Auto_num"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView2.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView2.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView2.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtwherehouse_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no"";
            this.GridView2.Columns["Col_txtmat_no"].Width = 0;
            this.GridView2.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_no"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView2.Columns["Col_txtmat_id"].Width = 90;
            this.GridView2.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_id"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView2.Columns["Col_txtmat_name"].Width = 140;
            this.GridView2.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView2.Columns["Col_txtmat_unit1_name"].Width = 80;
            this.GridView2.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.GridView2.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView2.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView2.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView2.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView2.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_chmat_unit_status"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 0;
            dgvCmb.DisplayIndex = 7;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.Visible = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView2.Columns.Add(dgvCmb);


            this.GridView2.Columns["Col_txtmat_unit2_name"].Visible = false;  //Col_txtmat_unit2_name";
            this.GridView2.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.GridView2.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView2.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView2.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView2.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmat_amount_phurchase"].Visible = false;  //"Col_txtmat_amount_phurchase";
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].Width = 0;
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_amount_phurchase"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmat_status"].Visible = false;  //"Col_txtmat_status";
            this.GridView2.Columns["Col_txtmat_status"].Width = 0;
            this.GridView2.Columns["Col_txtmat_status"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmat_status"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCheckBoxColumn dgvCmb2 = new DataGridViewCheckBoxColumn();
            dgvCmb2.ValueType = typeof(bool);
            dgvCmb2.Width = 0;
            dgvCmb2.DisplayIndex = 16;
            dgvCmb2.Name = "Col_Chk2";
            dgvCmb2.HeaderText = "สถานะ";
            dgvCmb2.ReadOnly = true;
            dgvCmb2.Visible = false;
            dgvCmb2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView2.Columns.Add(dgvCmb2);


            this.GridView2.Columns["Col_txtcost_qty1_balance"].Visible = true;  //"Col_txtcost_qty1_balance";
            this.GridView2.Columns["Col_txtcost_qty1_balance"].Width = 100;
            this.GridView2.Columns["Col_txtcost_qty1_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty1_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty1_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty1_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_balance"].Visible = true;  //"Col_txtcost_qty_balance";
            this.GridView2.Columns["Col_txtcost_qty_balance"].Width = 100;
            this.GridView2.Columns["Col_txtcost_qty_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_price_average"].Visible = false;  //"Col_txtcost_qty_price_average";
            this.GridView2.Columns["Col_txtcost_qty_price_average"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_price_average"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_money_sum"].Visible = false;  //"Col_txtcost_money_sum";
            this.GridView2.Columns["Col_txtcost_money_sum"].Width = 0;
            this.GridView2.Columns["Col_txtcost_money_sum"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_money_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_money_sum"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_money_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty2_balance"].Visible = false;  //"Col_txtcost_qty2_balance";
            this.GridView2.Columns["Col_txtcost_qty2_balance"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty2_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtsum_qty"].Visible = true;  //"Col_txtsum_qty";
            this.GridView2.Columns["Col_txtsum_qty"].Width = 100;
            this.GridView2.Columns["Col_txtsum_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtsum_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtsum_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtsum_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_balance"].Visible = true;  //"Col_txtcost_qty_balance";
            this.GridView2.Columns["Col_txtcost_qty_balance"].Width = 100;
            this.GridView2.Columns["Col_txtcost_qty_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_price_average"].Visible = false;  //"Col_txtcost_qty_price_average";
            this.GridView2.Columns["Col_txtcost_qty_price_average"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_price_average"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_price_average"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_money_sum"].Visible = false;  //"Col_txtcost_money_sum";
            this.GridView2.Columns["Col_txtcost_money_sum"].Width = 0;
            this.GridView2.Columns["Col_txtcost_money_sum"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_money_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_money_sum"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_money_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty2_balance"].Visible = false;  //"Col_txtcost_qty2_balance";
            this.GridView2.Columns["Col_txtcost_qty2_balance"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty2_balance"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty2_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtsum_qty"].Visible = true;  //"Col_txtcost_qty2_balance";
            this.GridView2.Columns["Col_txtsum_qty"].Width = 100;
            this.GridView2.Columns["Col_txtsum_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtsum_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtsum_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtsum_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtsum_price"].Visible = false;  //"Col_txtsum_price";
            this.GridView2.Columns["Col_txtsum_price"].Width = 0;
            this.GridView2.Columns["Col_txtsum_price"].ReadOnly = true;
            this.GridView2.Columns["Col_txtsum_price"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtsum_price"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtsum_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtsum_discount"].Visible = false;  //"Col_txtsum_discount";
            this.GridView2.Columns["Col_txtsum_discount"].Width = 0;
            this.GridView2.Columns["Col_txtsum_discount"].ReadOnly = true;
            this.GridView2.Columns["Col_txtsum_discount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtsum_discount"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtsum_discount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmoney_sum"].Visible = false;  //"Col_txtmoney_sum";
            this.GridView2.Columns["Col_txtmoney_sum"].Width = 0;
            this.GridView2.Columns["Col_txtmoney_sum"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmoney_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmoney_sum"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmoney_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmoney_tax_base"].Visible = false;  //"Col_txtmoney_tax_base";
            this.GridView2.Columns["Col_txtmoney_tax_base"].Width = 0;
            this.GridView2.Columns["Col_txtmoney_tax_base"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmoney_tax_base"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmoney_tax_base"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmoney_tax_base"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtvat_rate"].Visible = false;  //"Col_txtvat_rate";
            this.GridView2.Columns["Col_txtvat_rate"].Width = 0;
            this.GridView2.Columns["Col_txtvat_rate"].ReadOnly = true;
            this.GridView2.Columns["Col_txtvat_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtvat_rate"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtvat_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtvat_money"].Visible = false;  //"Col_txtvat_money";
            this.GridView2.Columns["Col_txtvat_money"].Width = 0;
            this.GridView2.Columns["Col_txtvat_money"].ReadOnly = true;
            this.GridView2.Columns["Col_txtvat_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtvat_money"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtvat_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtmoney_after_vat"].Visible = false;  //"Col_txtmoney_after_vat";
            this.GridView2.Columns["Col_txtmoney_after_vat"].Width = 0;
            this.GridView2.Columns["Col_txtmoney_after_vat"].ReadOnly = true;
            this.GridView2.Columns["Col_txtmoney_after_vat"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtmoney_after_vat"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtmoney_after_vat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_balance_yokpai"].Visible = true;  //"Col_txtcost_qty_balance_yokpai";
            this.GridView2.Columns["Col_txtcost_qty_balance_yokpai"].Width = 100;
            this.GridView2.Columns["Col_txtcost_qty_balance_yokpai"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty_price_average_yokpai"].Visible = false;  //"Col_txtcost_qty_price_average_yokpai";
            this.GridView2.Columns["Col_txtcost_qty_price_average_yokpai"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty_price_average_yokpai"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty_price_average_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_money_sum_yokpai"].Visible = false;  //"Col_txtcost_money_sum_yokpai";
            this.GridView2.Columns["Col_txtcost_money_sum_yokpai"].Width = 0;
            this.GridView2.Columns["Col_txtcost_money_sum_yokpai"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_money_sum_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty2_balance_yokma"].Visible = false;  //"Col_txtcost_qty2_balance_yokma";
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokma"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokma"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokma"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtsum2_qty"].Visible = false;  //"Col_txtsum2_qty";
            this.GridView2.Columns["Col_txtsum2_qty"].Width = 0;
            this.GridView2.Columns["Col_txtsum2_qty"].ReadOnly = true;
            this.GridView2.Columns["Col_txtsum2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtsum2_qty"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtsum2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty2_balance_yokpai"].Visible = false;  //"Col_txtcost_qty2_balance_yokpai";
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokpai"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokpai"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokpai"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty2_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty1_balance_yokma"].Visible = false;  //"Col_txtcost_qty1_balance_yokma";
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokma"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokma"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokma"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_txtcost_qty1_balance_yokpai"].Visible = false;  //"Col_txtcost_qty1_balance_yokpai";
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokpai"].Width = 0;
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokpai"].ReadOnly = true;
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokpai"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_txtcost_qty1_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView2.Columns["Col_1"].Visible = false;  //"Col_1";
            this.GridView2.Columns["Col_1"].Width = 0;
            this.GridView2.Columns["Col_1"].ReadOnly = true;
            this.GridView2.Columns["Col_1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView2.Columns["Col_1"].HeaderCell.Style.BackColor = Color.FromArgb(255, 255, 255);
            this.GridView2.Columns["Col_1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


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
        private void GridView2_Cal_Sum_M()
        {
            double Sum_Qty = 0;
            double C1 = 0;
            int k = 0;
            for (int s = 0; s < this.GridView2.Rows.Count; s++)
            {
                this.GridView2.Rows[s].Cells["Col_txtsum_qty"].Value = "0";

                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {

                    k = 1 + i;

                    var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                    if (valu != "")
                    {
                        if (this.GridView2.Rows[s].Cells["Col_txtmat_id"].Value.ToString() == this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString())
                        {

                            if (Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
                            {
                                //Sum_Qty  จำนวนเบิก (กก)=================================================
                                Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                                this.GridView2.Rows[s].Cells["Col_txtsum_qty"].Value = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                                C1 = Convert.ToDouble(string.Format("{0:n4}", C1)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_1"].Value.ToString()));
                                this.GridView2.Rows[s].Cells["Col_txtcost_qty1_balance"].Value = C1.ToString("N", new CultureInfo("en-US"));
                            }
                            //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                            //========================================

                        }
                        //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                        //========================================
                    }
                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //========================================
                }
                //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                //========================================
                Sum_Qty = 0;
                C1 = 0;
            }

        }
        private void GridView2_Cal_Sum()
        {
            double QAbyma = 0;
            double QAbyma2 = 0;
            double Qbypai = 0;
            double Mbypai = 0;
            double QAbypai = 0;
            double Qbypai2 = 0;
            double C1YP = 0;

            int k = 0;
            for (int i = 0; i < this.GridView2.Rows.Count; i++)
            {
                var valu = this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString();
                if (valu != "")
                {
                    //==============================================
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                    }
                    //==============================================

                    //==============================================

                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value = ".00";
                    }

                    //==============================================
                    if (this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtsum_discount"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum_discount"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtmoney_tax_base"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtmoney_tax_base"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtvat_rate"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtvat_rate"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtvat_money"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtvat_money"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtmoney_after_vat"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtmoney_after_vat"].Value = ".00";
                    }
                    //==============================================

                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";
                    }
                    //==============================================

                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";
                    }
                    //==============================================
                    //==============================================
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value = ".00";
                    }


                    //==============================================

                    //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString())) > 0)
                    //{

                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //มูลค่าต้นทุนเบิก 
                    this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value.ToString()));
                    QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value = QAbyma2.ToString("N", new CultureInfo("en-US"));


                    //1.เหลือยกมา + รับ = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา+- มูลค่ารับ = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) / Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString()));
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else

                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";
                    }

                    //1.เหลือ(2)ยกมา + รับ(2) = จำนวนเหลือ(2)ทั้งสิ้น
                    Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Qbypai2.ToString("N", new CultureInfo("en-US"));

                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                    //C2  ===========================================================================================================
                    C1YP = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value = C1YP.ToString("N", new CultureInfo("en-US"));
                    //}

                }
                QAbyma = 0;
                QAbyma2 = 0;
                Qbypai = 0;
                Mbypai = 0;
                QAbypai = 0;
                Qbypai2 = 0;
                C1YP = 0;

            }

            //====================
        }
        private void GridView2_Cal_Sum_For_Cancel()
        {
            double QAbyma = 0;
            double QAbyma2 = 0;
            double Qbypai = 0;
            double Mbypai = 0;
            double QAbypai = 0;
            double Qbypai2 = 0;
            double C1YP = 0;

            int k = 0;
            for (int i = 0; i < this.GridView2.Rows.Count; i++)
            {
                var valu = this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString();
                if (valu != "")
                {
                    //==============================================
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                    }
                    //==============================================

                    //==============================================

                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value = ".00";
                    }

                    //==============================================
                    if (this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtsum_discount"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum_discount"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtmoney_tax_base"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtmoney_tax_base"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtvat_rate"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtvat_rate"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtvat_money"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtvat_money"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtmoney_after_vat"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtmoney_after_vat"].Value = ".00";
                    }
                    //==============================================

                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";
                    }
                    //==============================================

                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";
                    }
                    //==============================================
                    //==============================================
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value = ".00";
                    }
                    if (this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value == null)
                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value = ".00";
                    }

                    //==============================================

                    //if (Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString())) > 0)
                    //{

                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //มูลค่าต้นทุนเบิก 
                    this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value.ToString()));
                    QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value = QAbyma2.ToString("N", new CultureInfo("en-US"));


                    //1.เหลือยกมา - รับ = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา- มูลค่ารับ = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) / Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString()));
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else

                    {
                        this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";
                    }

                    //1.เหลือ(2)ยกมา - รับ(2) = จำนวนเหลือ(2)ทั้งสิ้น
                    Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Qbypai2.ToString("N", new CultureInfo("en-US"));

                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                    //C2  ===========================================================================================================
                    C1YP = Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value.ToString()));
                    this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value = C1YP.ToString("N", new CultureInfo("en-US"));
                    //}

                }
                QAbyma = 0;
                QAbyma2 = 0;
                Qbypai = 0;
                Mbypai = 0;
                QAbypai = 0;
                Qbypai2 = 0;
                C1YP = 0;

            }

            //====================
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
            var frm2 = new HOME03_Production.HOME03_Production_07Receive_Send_Dye_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "บันทึกใบรับผ้าพับ";
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


                    cmd2.CommandText = "SELECT c002_081Send_Cut_shirt_record.*," +
                                       "c002_081Send_Cut_shirt_record_detail.*" +

                                       " FROM c002_081Send_Cut_shirt_record" +
                                       " INNER JOIN c002_081Send_Cut_shirt_record_detail" +
                                       " ON c002_081Send_Cut_shirt_record.cdkey = c002_081Send_Cut_shirt_record_detail.cdkey" +
                                       " AND c002_081Send_Cut_shirt_record.txtco_id = c002_081Send_Cut_shirt_record_detail.txtco_id" +

                                       " WHERE (c002_081Send_Cut_shirt_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (c002_081Send_Cut_shirt_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (c002_081Send_Cut_shirt_record.txtSPT_status = '0')" +
                                       " AND (c002_081Send_Cut_shirt_record_detail.txtLot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                       " AND (c002_081Send_Cut_shirt_record_detail.txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')" +
                                       " ORDER BY c002_081Send_Cut_shirt_record_detail.txtmat_no ASC";

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

                                MessageBox.Show("Lot no :   " + dt2.Rows[j]["txtLot_no"].ToString() + "    นี้ มีการบันทึกผ้าตัด ไปแล้ว ไม่สามารถยกเลิกรายการได้ !!! ");
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

                cmd1.CommandText = "SELECT * FROM c002_07Receive_Send_dye_record" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtFG2_id = '" + this.txtFG2_id.Text.Trim() + "')" +
                                    " AND (txtFG2_status = '1')";

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

            GridView1_Cal_Sum_For_Cancel();
            Show_Qty_Yokma();
            Show_Qty_Yokma2();
            GridView2_Cal_Sum_M();
            GridView2_Cal_Sum();
            GridView2_Cal_Sum_For_Cancel();

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

                    //if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                    //{

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
                        cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 1));
                        cmd2.Parameters.Add("@cancel_id", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok1");

                        //2
                        cmd2.CommandText = "UPDATE c002_07Receive_Send_dye_record" +
                                                                    " SET txtFG2_status = '1'," +
                                                                     "txtsum_qty_pub = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                                     "txtsum_qty_pub_kg = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                                     "txtsum_qty_rib = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                                     "txtsum_qty_rib_kg = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +
                                                                     "txtsum_qty = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'" +
                                                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                     " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                     " AND (txtFG2_id = '" + this.txtFG2_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");

                    //5

                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {

                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();

                            //===================================================================================================================
                            // c002_02produce_record_detail
                            cmd2.CommandText = "UPDATE c002_05Send_dye_record_detail SET " +
                                                   "txtcut_id = ''," +
                                                    "txtqty_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +
                                                   "txtqty_after_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                     " AND (txtPPT_id = '" + this.GridView1.Rows[i].Cells["Col_txtPPT_id"].Value.ToString() + "')" +
                                                   " AND (txtLot_no = '" + this.GridView1.Rows[i].Cells["Col_txtLot_no"].Value.ToString() + "')";
                                                   //" AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";

                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok7");

                            //=====================================================================================================
                            //}
                        }
                    }


                    //}

                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    for (int i = 0; i < this.GridView2.Rows.Count; i++)
                    {
                        var valu = this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString();
                        if (valu != "")
                        {
                            if (Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString())) > 0)
                            {

                                //1.k021_mat_average
                                cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                        "txtcost_qty1_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value.ToString())) + "'," +
                                       "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +
                                       "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +
                                        "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +
                                       "txtcost_qty2_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value.ToString())) + "'" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                       " AND (txtmat_id = '" + this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


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
                                        "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +
                                        //"'" + myDateTime_DateRecord + "'," +  //3
                                        "@txttrans_date_client," +
                                        "'" + W_ID_Select.COMPUTER_IP.Trim() + "','" + W_ID_Select.COMPUTER_NAME.Trim() + "'," +  //4
                                        "'" + W_ID_Select.M_USERNAME.Trim() + "','" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +  //5
                                        "'" + W_ID_Select.VERSION_ID.Trim() + "'," +  //6
                                                                                      //=======================================================


                                        "'" + this.txtFG2_id.Text.Trim() + "'," +  //7 txtbill_id
                                        "'FG2'," +  //9 txtbill_type
                                        "'ยกเลิกรับผ้าพับ " + this.txtrg_remark.Text.Trim() + "'," +  //9 txtbill_remark

                                         "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                                       "'" + this.GridView2.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //10 
                                       "'" + this.GridView2.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //10 
                                       "'" + this.GridView2.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +  //10 

                                       "'" + this.GridView2.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //10 
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString())) + "'," +  //14
                                       "'" + this.GridView2.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() + "'," +  //10 
                                       "'" + this.GridView2.Rows[i].Cells["Col_txtmat_unit2_name"].Value.ToString() + "'," +  //10 
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString())) + "'," +  //14

                                         "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty1_in
                                     "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty_in
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19 txtqty2_in
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //20 txtprice_in
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //21 txtsum_total_in

                                         "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance"].Value.ToString())) + "'," +  //14
                                     "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_qty"].Value.ToString())) + "'," +  //14
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum2_qty"].Value.ToString())) + "'," +  //14
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtsum_price"].Value.ToString())) + "'," +  //14
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtmoney_sum"].Value.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                                        "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty1_balance_yokpai"].Value.ToString())) + "'," +  //14
                                      "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //14
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value.ToString())) + "'," +  //14
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //14
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView2.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //14

                                       "'1')";   //30

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok8");
                            }
                        }
                    }



                    //======================================

                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    //MessageBox.Show("ok4");


                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtFG2_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
            W_ID_Select.TRANS_ID = this.txtFG2_id.Text.Trim();
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
            W_ID_Select.TRANS_ID = this.txtFG2_id.Text.Trim();
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
                                                               " AND (txtFG2_id = '" + this.txtFG2_id.Text.Trim() + "')";
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






        //=========================================================

    }
}
