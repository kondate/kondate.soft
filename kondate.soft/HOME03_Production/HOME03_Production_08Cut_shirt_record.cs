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
    public partial class HOME03_Production_08Cut_shirt_record : Form
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

        public HOME03_Production_08Cut_shirt_record()
        {
            InitializeComponent();
        }

        private void HOME03_Production_08Cut_shirt_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0308CSRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "บันทึกใบสั่งตัด";

            this.ActiveControl = this.PANEL1306_WH_txtwherehouse_name;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            this.cbotxtcut_type_name.Items.Add("โรงงานเราเย็บเอง");
            this.cbotxtcut_type_name.Items.Add("จ้างบริษัทอื่นเย็บ");
            this.cbotxtcut_type_name.Items.Add("จ้างช่างตัดเย็บ");
            this.cbotxtcut_type_name.Text = "โรงงานเราเย็บเอง";
            this.txtcut_type_id.Text = "01";

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL0108_SHIRT_TYPE_GridView1_shirt_type();
            PANEL0108_SHIRT_TYPE_Fill_shirt_type();

            PANEL0109_SHIRT_SIZE_GridView1_shirt_size();
            PANEL0109_SHIRT_SIZE_Fill_shirt_size();

            PANEL0110_ROOM_COLLECT_GridView1_room_collect();
            PANEL0110_ROOM_COLLECT_Fill_room_collect();

            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();


            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            //1.======================================================================
            this.dtptxtdate_begin_job.Value = DateTime.Now;
            this.dtptxtdate_begin_job.Format = DateTimePickerFormat.Custom;
            this.dtptxtdate_begin_job.CustomFormat = this.dtptxtdate_begin_job.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear_begin_job.Text = this.dtptxtdate_begin_job.Value.ToString("yyyy", UsaCulture);

            //1.======================================================================
            this.dtptxtdate_finish_job.Value = DateTime.Now;
            this.dtptxtdate_finish_job.Format = DateTimePickerFormat.Custom;
            this.dtptxtdate_finish_job.CustomFormat = this.dtptxtdate_finish_job.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear_finish_job.Text = this.dtptxtdate_finish_job.Value.ToString("yyyy", UsaCulture);


            Show_GridView1();

        }

        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle;
        DataTable table = new DataTable();
        int selectedRowIndex;
        int curRow = 0;

        private void btnGo1_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก คลัง ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                return;

            }
            else
            {

            }
            if (this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก ชนิดงาน ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL0108_SHIRT_TYPE.Visible == false)
                {
                    this.PANEL0108_SHIRT_TYPE.Visible = true;
                    this.PANEL0108_SHIRT_TYPE.BringToFront();
                    this.PANEL0108_SHIRT_TYPE.Location = new Point(this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Location.X, this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Location.Y + 22);
                }
                else
                {
                    this.PANEL0108_SHIRT_TYPE.Visible = false;
                }
                return;

            }
            else
            {

            }

            if (this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก ไซส์เสื้อ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL0109_SHIRT_SIZE.Visible == false)
                {
                    this.PANEL0109_SHIRT_SIZE.Visible = true;
                    this.PANEL0109_SHIRT_SIZE.BringToFront();
                    this.PANEL0109_SHIRT_SIZE.Location = new Point(this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Location.X, this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Location.Y + 22);
                }
                else
                {
                    this.PANEL0109_SHIRT_SIZE.Visible = false;
                }
                return;

            }
            else
            {

            }

            if (this.PANEL0110_ROOM_COLLECT_txtroom_collect_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก ที่เก็บผ้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL0110_ROOM_COLLECT.Visible == false)
                {
                    this.PANEL0110_ROOM_COLLECT.Visible = true;
                    this.PANEL0110_ROOM_COLLECT.BringToFront();
                    this.PANEL0110_ROOM_COLLECT.Location = new Point(this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.X, this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.Y + 22);
                }
                else
                {
                    this.PANEL0110_ROOM_COLLECT.Visible = false;
                }
                return;

            }
            else
            {

            }

            if (this.PANEL161_SUP_txtsupplier_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก Supplier ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                return;

            }
            else
            {

            }


            //======================================================
            SHOW_GO1();
            //======================================================








        }
        private void SHOW_GO1()
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
                                     //" AND (c002_07Receive_Send_dye_record.txtRG_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                     " AND (c002_07Receive_Send_dye_record_detail.txtqty_berg_cut_shirt_balance > 0)" +
                                     " AND (c002_07Receive_Send_dye_record.txtrg_status = '0')" +
                               " ORDER BY c002_07Receive_Send_dye_record_detail.txtnumber_in_year,c002_07Receive_Send_dye_record_detail.txtfold_number ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        this.txtmat_no.Text = dt2.Rows[0]["txtmat_no"].ToString();
                        this.PANEL_MAT_txtmat_id.Text = dt2.Rows[0]["txtmat_id"].ToString();
                        this.PANEL_MAT_txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();
                        this.txtmat_name.Text = dt2.Rows[0]["txtmat_name"].ToString();

                        this.txtmat_unit1_name.Text = dt2.Rows[0]["txtmat_unit1_name"].ToString();
                        this.txtmat_unit1_qty.Text = dt2.Rows[0]["txtmat_unit1_qty"].ToString();
                        this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                        this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                        this.txtmat_unit2_qty.Text = dt2.Rows[0]["txtmat_unit2_qty"].ToString();


                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text = dt2.Rows[0]["txtnumber_mat_id"].ToString();
                        this.PANEL0106_NUMBER_MAT_txtnumber_mat_name.Text = dt2.Rows[0]["txtnumber_mat_name"].ToString();

                        this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text = dt2.Rows[0]["txtnumber_color_id"].ToString();
                        this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text = dt2.Rows[0]["txtnumber_color_name"].ToString();


                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");


                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value = dt2.Rows[j]["txtnumber_in_year"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtshirt_size_id"].Value = this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text.Trim();      //3
                            GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();       //4
                            GridView1.Rows[index].Cells["Col_txtnumber_dyed"].Value = dt2.Rows[j]["txtnumber_dyed"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //6



                            GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();       //8


                            GridView1.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();      //9
                            GridView1.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //10

                            GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");     //11

                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //12
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //13
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //14

                            GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //15
                            GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //16

                            GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //17

                            GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //18
                            GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //19

                            GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");     //20


                            GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //21
                            GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //22
                            GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //23
                            GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //24

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //25
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //26
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //27

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //28
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //29
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //30

                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //31
                            GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //32

                            GridView1.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //33

                            GridView1.Rows[index].Cells["Col_txtqc_id"].Value = dt2.Rows[j]["txtqc_id"].ToString();      //34
                            GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //35
                            GridView1.Rows[index].Cells["Col_qty_Cal"].Value = "0";      //36
                            GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = "0";      //37
                            GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";      //38
                            GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";      //39

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

            Show_Qty_Yokma();
            GridView1_Color_Column();
            GridView1_Up_Status();
            GridView1_Cal_Sum();

            //Fill_cboemp();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 41;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[2].Name = "Col_txtnumber_in_year";

            this.GridView1.Columns[3].Name = "Col_txtshirt_size_id";
            this.GridView1.Columns[4].Name = "Col_txtnumber_color_id";  //สีผ้า
            this.GridView1.Columns[5].Name = "Col_txtnumber_dyed";   //เบอร์กอง
            this.GridView1.Columns[6].Name = "Col_txtsupplier_id";   //ซัพพลายเอร์
            this.GridView1.Columns[7].Name = "Col_txtnumber_mat_id";  //ชนิดผ้า เบอร์ผ้า
            this.GridView1.Columns[8].Name = "Col_txtface_baking_id";


            this.GridView1.Columns[9].Name = "Col_txtlot_no";
            this.GridView1.Columns[10].Name = "Col_txtfold_number";

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
            this.GridView1.Columns[35].Name = "Col_txtsum_qty_pub";
            this.GridView1.Columns[36].Name = "Col_date";
            this.GridView1.Columns[37].Name = "Col_qty_Cal";  //
            this.GridView1.Columns[38].Name = "Col_txtsum_qty_rib";
            this.GridView1.Columns[39].Name = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[40].Name = "Col_txtsum_qty_rib_kg";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "คลัง";
            this.GridView1.Columns[2].HeaderText = "ชุดที่";
            this.GridView1.Columns[3].HeaderText = "ไซส์เสื้อ";
            this.GridView1.Columns[4].HeaderText = "รหัสสี";
            this.GridView1.Columns[5].HeaderText = "เบอร์กอง";
            this.GridView1.Columns[6].HeaderText = "ผ้า Customer";

            this.GridView1.Columns[7].HeaderText = "รหัสผ้า..";
            this.GridView1.Columns[8].HeaderText = "อบหน้า..";


            this.GridView1.Columns[9].HeaderText = "Lot No..";
            this.GridView1.Columns[10].HeaderText = "พับที่";

            this.GridView1.Columns[11].HeaderText = "เบิกผ้า (กก.)";

            this.GridView1.Columns[12].HeaderText = "ลำดับ";
            this.GridView1.Columns[13].HeaderText = "รหัส";
            this.GridView1.Columns[14].HeaderText = "ชื่อสินค้า";

            this.GridView1.Columns[15].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[16].HeaderText = " หน่วย";
            this.GridView1.Columns[17].HeaderText = "แปลง";
            this.GridView1.Columns[18].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[19].HeaderText = " หน่วย";

            this.GridView1.Columns[20].HeaderText = "เบิกผ้า(ปอนด์)";

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
            this.GridView1.Columns[35].HeaderText = "Col_txtsum_qty_pub";
            this.GridView1.Columns[36].HeaderText = " วันที่ต้องการ";
            this.GridView1.Columns[37].HeaderText = "Col_qty_Cal";
            this.GridView1.Columns[38].HeaderText = "Col_txtsum_qty_rib";
            this.GridView1.Columns[39].HeaderText = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[40].HeaderText = "Col_txtsum_qty_rib_kg";

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

            this.GridView1.Columns["Col_txtshirt_size_id"].Visible = true;  //"Col_txtshirt_size_id";
            this.GridView1.Columns["Col_txtshirt_size_id"].Width = 80;
            this.GridView1.Columns["Col_txtshirt_size_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtshirt_size_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtshirt_size_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_color_id"].Visible = true;  //"Col_txtnumber_color_id";
            this.GridView1.Columns["Col_txtnumber_color_id"].Width = 120;
            this.GridView1.Columns["Col_txtnumber_color_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_color_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_color_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_dyed"].Visible = true;  //"Col_txtnumber_dyed";
            this.GridView1.Columns["Col_txtnumber_dyed"].Width = 100;
            this.GridView1.Columns["Col_txtnumber_dyed"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_dyed"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_dyed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsupplier_id"].Visible = true;  //"Col_txtsupplier_id";
            this.GridView1.Columns["Col_txtsupplier_id"].Width = 150;
            this.GridView1.Columns["Col_txtsupplier_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsupplier_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsupplier_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = true;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 80;
            this.GridView1.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView1.Columns["Col_txtface_baking_id"].Width = 60;
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

            //this.GridView1.Columns[8].Visible = false;
            DataGridViewCheckBoxColumn dgvCmb_SELECT = new DataGridViewCheckBoxColumn();
            dgvCmb_SELECT.Name = "Col_Chk_SELECT";
            dgvCmb_SELECT.Width = 120;  //70
            dgvCmb_SELECT.DisplayIndex = 11;
            dgvCmb_SELECT.HeaderText = "เลือกสั่งตัด";
            dgvCmb_SELECT.ValueType = typeof(bool);
            dgvCmb_SELECT.ReadOnly = false;
            dgvCmb_SELECT.Visible = true;
            dgvCmb_SELECT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb_SELECT);

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = true;
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

            this.GridView1.Columns["Col_txtsum_qty_pub"].Visible = false;  //"Col_txtsum_qty_pub";
            this.GridView1.Columns["Col_txtsum_qty_pub"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_pub"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_date"].Visible = false;  //"Col_date";
            this.GridView1.Columns["Col_date"].Width = 0;
            this.GridView1.Columns["Col_date"].ReadOnly = false;
            this.GridView1.Columns["Col_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //this.GridView1.Columns[35].HeaderText = " Col_qty_Cal";
            this.GridView1.Columns["Col_qty_Cal"].Visible = false;  //"Col_qty_Cal";
            this.GridView1.Columns["Col_qty_Cal"].Width = 0;
            this.GridView1.Columns["Col_qty_Cal"].ReadOnly = false;
            this.GridView1.Columns["Col_qty_Cal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_qty_Cal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //this.GridView1.Columns[35].HeaderText = " Col_txtsum_qty_rib";
            this.GridView1.Columns["Col_txtsum_qty_rib"].Visible = false;  //"Col_txtsum_qty_rib";
            this.GridView1.Columns["Col_txtsum_qty_rib"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_rib"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].Visible = false;  //"Col_txtsum_qty_pub_kg";
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].ReadOnly = true;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_pub_kg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].Visible = false;  //"Col_txtsum_qty_rib_kg";
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].Width = 0;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].ReadOnly = false;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtsum_qty_rib_kg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
        private void GridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtp.Visible = false;
        }
        private void GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            dtp.Visible = false;
        }
        private void GridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }
        private void GridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void GridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GridView1_Cal_Sum();
                Sum_group_tax();
            }
        }
        private void GridView1_KeyUp(object sender, KeyEventArgs e)
        {
            GridView1_Cal_Sum();
            Sum_group_tax();


        }
        private void dtp_TextChange(object sender, EventArgs e)
        {
            GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
            GridView1_Cal_Sum();
            Sum_group_tax();
        }
        private void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }
        void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                GridView1_Cal_Sum();
                Sum_group_tax();

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



            double Sum_Qty_Pub = 0;
            double Sum_Qty_RIB = 0;
            double Sum_Qty_Pub_kg = 0;
            double Sum_Qty_RIB_kg = 0;

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
                if (double.Parse(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
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
                }

                if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                {
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() != "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "1";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value = this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString();
                    }
                    //Col_txtfold_number
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = "1";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString();
                    }

                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString();
                    //if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString() == "1")
                    //{
                    //Sum_Qty  จำนวนเบิก (กก)=================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub  จำนวนพับ=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub_kg  จำนวนพับ=================================================
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));


                    //Sum_Qty_RIB จำนวนพับ=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_RIB_kg จำนวนพับ=================================================
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));


                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
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
                    }                    //Col_txtfold_number
                    if (this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() == "RIB")
                    {
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value = "0";
                        this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value = "0";
                    }
                    this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value = "0";
                    //if (this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString() == "1")
                    //{
                    //Sum_Qty  จำนวนเบิก (กก)=================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub  จำนวนพับ=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub_kg  จำนวนพับ=================================================
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_RIB จำนวนพับ=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_RIB_kg จำนวนพับ=================================================
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n4}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));

                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }


                }

                //if (double.Parse(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) == 1)
                //{

                //}
                //else
                //{

                //    //this.txtsum_qty.Text = ".00";
                //    //this.txtsum_qty_pub.Text = ".00";
                //    //============================================================================================================
                //}

                //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                QAbyma = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                QAbyma2 = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                //1.เหลือยกมา - เบิก = จำนวนเหลือทั้งสิ้น
                Qbypai = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));
                this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                //2.มูลค่าเหลือยกมา - มูลค่าเบิก = มูลค่ารวมทั้งสิ้น
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

                //1.เหลือ(2)ยกมา - เบิก(2) = จำนวนเหลือ(2)ทั้งสิ้น
                Qbypai2 = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));
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
            Sum_Qty_RIB = 0;
            Sum_Qty_Pub_kg = 0;
            Sum_Qty_RIB_kg = 0;


        }
        private void GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //We make DataGridCheckBoxColumn commit changes with single click
            //use index of logout column
            this.GridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //Check the value of cell
            if (Convert.ToBoolean(this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value) == true)
            {

                //Use index of TimeOut column
                GridView1_Cal_Sum();
                Sum_group_tax();

                //Set other columns values
            }
            else
            {
                //Use index of TimeOut column
                GridView1_Cal_Sum();
                Sum_group_tax();

                //Set other columns values
            }

        }
        private void GridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

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
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = GridView1.CurrentRow.Index;

            switch (GridView1.Columns[e.ColumnIndex].Name)
            {
                case "Col_txtwherehouse_id":
                    dtp.Visible = false;
                    break;
                case "Col_txtnumber_in_year":
                    dtp.Visible = false;
                    break;
                case "Col_txtnumber_mat_id":
                    dtp.Visible = false;
                    break;
                case "Col_txtnumber_color_id":
                    dtp.Visible = false;
                    break;
                case "Col_txtface_baking_id":
                    dtp.Visible = false;
                    break;
                case "Col_txtlot_no":
                    dtp.Visible = false;
                    break;
                case "Col_txtfold_number":
                    dtp.Visible = false;
                    break;
                case "Col_txtqty":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_no":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_id":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_name":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_unit1_name":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_unit1_qty":
                    dtp.Visible = false;
                    break;
                case "Col_chmat_unit_status":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_unit2_name":
                    dtp.Visible = false;
                    break;
                case "Col_txtmat_unit2_qty":
                    dtp.Visible = false;
                    break;
                case "Col_txtqty2":
                    dtp.Visible = false;
                    break;
                case "Col_Chk_SELECT":
                    dtp.Visible = false;
                    break;




                case "Col_date":

                    _Rectangle = GridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                    dtp.Size = new Size(_Rectangle.Width, _Rectangle.Height); //  
                    dtp.Location = new Point(_Rectangle.X, _Rectangle.Y); //  

                    if (Convert.ToBoolean(this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value) == true)
                    {
                        GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
                    }
                    else
                    {
                        GridView1.CurrentCell.Value = null;
                    }
                    dtp.Visible = true;
                    break;
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
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_ONvat")  //ซื้อไม่มีvat
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
        private void GridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void GridView1_MouseClick(object sender, MouseEventArgs e)
        {
            //   "Col_Chk_SELECT"
            if (this.GridView1.CurrentCell.ColumnIndex == 8)
            {
                if (Convert.ToBoolean(this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value) == false)
                {
                    this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value = true;
                    //this.GridView1.Rows[selectedRowIndex].Cells["Col_txtsum_qty_pub"].Value = "1";
                    //this.textBox1.Text = "1";
                    GridView1_Cal_Sum();
                    Sum_group_tax();

                }
                else
                {
                    this.GridView1.Rows[selectedRowIndex].Cells["Col_Chk_SELECT"].Value = false;
                    //this.GridView1.Rows[selectedRowIndex].Cells["Col_txtsum_qty_pub"].Value = "0";
                    //this.textBox1.Text = "0";
                    GridView1_Cal_Sum();
                    Sum_group_tax();

                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GridView1_Cal_Sum();
            Sum_group_tax();
        }



        //===========================================================================================


        //txtwherehouse คลังสินค้า  =======================================================================

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
            if (W_ID_Select.M_FORM_NEW == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            W_ID_Select.LOG_ID = "3";
            W_ID_Select.LOG_NAME = "ใหม่";
            TRANS_LOG();

            this.Hide();
            var frm2 = new HOME03_Production.HOME03_Production_08Cut_shirt_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "บันทึกใบสั่งตัด";
            this.txtCS_id.ReadOnly = true;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกคลังสินค้าที่จะบันทึกเก็บ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1306_WH_txtwherehouse_id.Focus();
                return;
            }
            if (this.PANEL_MAT_txtmat_id.Text == "")
            {
                MessageBox.Show("โปรด ใส่รหัสสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL_MAT_txtmat_id.Focus();
                return;
            }
            if (this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text == "")
            {
                MessageBox.Show("โปรด ใส่เบอร์สินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Focus();
                return;
            }


            AUTO_BILL_TRANS_ID();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();


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
                        cmd2.CommandText = "INSERT INTO c002_08Cut_shirt_record_trans(cdkey," +
                                           "txtco_id,txtbranch_id," +
                                           "txttrans_id)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                           "'" + this.txtCS_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();


                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE c002_08Cut_shirt_record_trans SET txttrans_id = '" + this.txtCS_id.Text.Trim() + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //MessageBox.Show("ok1");

                    //2 c002_08Cut_shirt_record
                    cmd2.CommandText = "INSERT INTO c002_08Cut_shirt_record(cdkey,txtco_id,txtbranch_id," +  //1
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                            "txtuser_name,txtemp_office_name," +  //5
                                           "txtversion_id," +  //6
                                                               //====================================================

                                           "txtCS_id," + // 7
                                           "txtwherehouse_id," + // 8
                                           "txtshirt_type_id," + // 9
                                           "txtshirt_size_id," + // 10
                                          "txtroom_collect_id," + // 11
                                           "txttable_number," + // 12
                                          "txtbegin_poo_pa_time," + // 13
                                          "txtfinish_poo_pa_time," + // 14
                                          "txtcut_type_id," + // 15
                                          "txtemp_name_poo_pa," + // 16
                                          "txtemp_name_jai_pa," + // 17
                                          "txtemp_name_cang_tad," + // 18
                                          "txtsupplier_id," + // 19
                                          "txtshirt_cut_remark," + // 20

                                          "txtdate_begin_job," + // 21
                                           "txtdate_finish_job," + // 22

                                           "txtqty_chan," + // 23
                                          "txtqty_many_per_chan," + // 24
                                          "txtqty_amount," + // 25

                                           "txtmat_no," + // 26
                                           "txtmat_id," + // 27
                                           "txtmat_name," + // 28
                                           "txtnumber_mat_id," + // 29
                                           "txtnumber_color_id," + // 29

                                           "txtcurrency_id," + // 30
                                           "txtcurrency_date," + // 31
                                           "txtcurrency_rate," + // 32

                                           "txtacc_group_tax_id," + // 33

                                           "txtcost_qty_balance_yokma," + // 34
                                           "txtcost_qty_price_average_yokma," + // 35
                                           "txtcost_money_sum_yokma," + // 36

                                           "txtsum_qty_pub," + // 37
                                           "txtsum_qty_pub_receive," + // 38
                                           "txtsum_qty_pub_balance," + // 39

                                           "txtsum_qty_pub_kg," + // 40
                                           "txtsum_qty_pub_receive_kg," + // 41
                                           "txtsum_qty_pub_balance_kg," + // 42


                                           "txtsum_qty_rib," + // 43
                                           "txtsum_qty_rib_receive," + // 44
                                           "txtsum_qty_rib_balance," + // 45

                                           "txtsum_qty_rib_kg," + // 46
                                           "txtsum_qty_rib_receive_kg," + // 47
                                           "txtsum_qty_rib_balance_kg," + // 48


                                           "txtsum_qty," + // 49
                                           "txtsum_qty_receive," + // 50
                                           "txtsum_qty_balance," + // 51


                                           "txtsum_price," + // 52
                                           "txtsum_discount," + // 53
                                           "txtmoney_sum," + // 54
                                           "txtmoney_tax_base," + // 55
                                           "txtvat_rate," + // 56
                                           "txtvat_money," + // 57
                                           "txtmoney_after_vat," + // 58
                                           "txtmoney_after_vat_creditor," + // 59
                                           "txtcreditor_status," + // 60

                                           "txtcost_qty_balance_yokpai," + // 61
                                           "txtcost_qty_price_average_yokpai," + // 62
                                           "txtcost_money_sum_yokpai," + // 63

                                           "txtcost_qty2_balance_yokma," + // 64
                                           "txtsum2_qty," + // 65
                                           "txtcost_qty2_balance_yokpai," + // 66

                                           "txtcs_status," +  //67
                                          "txtapprove_status," +  //68
                                          "txtfg_status," +  //69
                                          "txtsew_status,txtSEW_id," +  //70
                                          "txtpayment_status," +  //71
                                          "txtacc_record_status," +  //72
                                          "txtemp_print,txtemp_print_datetime) " +  //73

                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtuser_name,@txtemp_office_name," +  //5
                                           "@txtversion_id," +  //6
                                                                //=========================================================


                                            "@txtCS_id," + // 7
                                           "@txtwherehouse_id," + // 8
                                           "@txtshirt_type_id," + // 9
                                           "@txtshirt_size_id," + // 10
                                          "@txtroom_collect_id," + // 11
                                           "@txttable_number," + // 12
                                          "@txtbegin_poo_pa_time," + // 13
                                          "@txtfinish_poo_pa_time," + // 14
                                          "@txtcut_type_id," + // 15
                                          "@txtemp_name_poo_pa," + // 16
                                          "@txtemp_name_jai_pa," + // 17
                                          "@txtemp_name_cang_tad," + // 18
                                          "@txtsupplier_id," + // 19
                                          "@txtshirt_cut_remark," + // 20

                                          "@txtdate_begin_job," + // 21
                                           "@txtdate_finish_job," + // 22

                                           "@txtqty_chan," + // 23
                                          "@txtqty_many_per_chan," + // 24
                                          "@txtqty_amount," + // 25

                                           "@txtmat_no," + // 26
                                           "@txtmat_id," + // 27
                                           "@txtmat_name," + // 28
                                           "@txtnumber_mat_id," + // 29
                                           "@txtnumber_color_id," + // 29

                                           "@txtcurrency_id," + // 30
                                           "@txtcurrency_date," + // 31
                                           "@txtcurrency_rate," + // 32

                                           "@txtacc_group_tax_id," + // 33

                                           "@txtcost_qty_balance_yokma," + // 34
                                           "@txtcost_qty_price_average_yokma," + // 35
                                           "@txtcost_money_sum_yokma," + // 36

                                           "@txtsum_qty_pub," + // 37
                                           "@txtsum_qty_pub_receive," + // 38
                                           "@txtsum_qty_pub_balance," + // 39

                                           "@txtsum_qty_pub_kg," + // 40
                                           "@txtsum_qty_pub_receive_kg," + // 41
                                           "@txtsum_qty_pub_balance_kg," + // 42


                                           "@txtsum_qty_rib," + // 43
                                           "@txtsum_qty_rib_receive," + // 44
                                           "@txtsum_qty_rib_balance," + // 45

                                           "@txtsum_qty_rib_kg," + // 46
                                           "@txtsum_qty_rib_receive_kg," + // 47
                                           "@txtsum_qty_rib_balance_kg," + // 48


                                           "@txtsum_qty," + // 49
                                           "@txtsum_qty_receive," + // 50
                                           "@txtsum_qty_balance," + // 51


                                           "@txtsum_price," + // 52
                                           "@txtsum_discount," + // 53
                                           "@txtmoney_sum," + // 54
                                           "@txtmoney_tax_base," + // 55
                                           "@txtvat_rate," + // 56
                                           "@txtvat_money," + // 57
                                           "@txtmoney_after_vat," + // 58
                                           "@txtmoney_after_vat_creditor," + // 59
                                           "@txtcreditor_status," + // 60

                                           "@txtcost_qty_balance_yokpai," + // 61
                                           "@txtcost_qty_price_average_yokpai," + // 62
                                           "@txtcost_money_sum_yokpai," + // 63

                                           "@txtcost_qty2_balance_yokma," + // 64
                                           "@txtsum2_qty," + // 65
                                           "@txtcost_qty2_balance_yokpai," + // 66

                                           "@txtcs_status," +  //67
                                          "@txtapprove_status," +  //68
                                          "@txtfg_status," +  //69
                                          "@txtsew_status,@txtSEW_id," +  //70
                                          "@txtpayment_status," +  //71
                                          "@txtacc_record_status," +  //72
                                          "@txtemp_print,@txtemp_print_datetime) ";  //73

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



                    cmd2.Parameters.Add("@txtCS_id", SqlDbType.NVarChar).Value = this.txtCS_id.Text.Trim();  //7
                    cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.Trim();  //8
                    cmd2.Parameters.Add("@txtshirt_type_id", SqlDbType.NVarChar).Value = this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text.Trim();  //9
                    cmd2.Parameters.Add("@txtshirt_size_id", SqlDbType.NVarChar).Value = this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text.Trim();  //10
                    cmd2.Parameters.Add("@txtroom_collect_id", SqlDbType.NVarChar).Value = this.PANEL0110_ROOM_COLLECT_txtroom_collect_id.Text.Trim();  //11
                    cmd2.Parameters.Add("@txttable_number", SqlDbType.NVarChar).Value = this.txttable_number.Text.Trim();  //12
                    cmd2.Parameters.Add("@txtbegin_poo_pa_time", SqlDbType.NVarChar).Value = this.txtbegin_poo_pa_time.Text.Trim();  //13
                    cmd2.Parameters.Add("@txtfinish_poo_pa_time", SqlDbType.NVarChar).Value = this.txtfinish_poo_pa_time.Text.Trim();  //14
                    cmd2.Parameters.Add("@txtcut_type_id", SqlDbType.NVarChar).Value = this.txtcut_type_id.Text.Trim();  //15

                    cmd2.Parameters.Add("@txtemp_name_poo_pa", SqlDbType.NVarChar).Value = this.txtemp_name_poo_pa.Text.Trim();  //16
                    cmd2.Parameters.Add("@txtemp_name_jai_pa", SqlDbType.NVarChar).Value = this.txtemp_name_jai_pa.Text.Trim();  //17
                    cmd2.Parameters.Add("@txtemp_name_cang_tad", SqlDbType.NVarChar).Value = this.txtemp_name_cang_tad.Text.Trim();  //18

                    cmd2.Parameters.Add("@txtsupplier_id", SqlDbType.NVarChar).Value = this.PANEL161_SUP_txtsupplier_id.Text.Trim();  //19
                    cmd2.Parameters.Add("@txtshirt_cut_remark", SqlDbType.NVarChar).Value = this.txtshirt_cut_remark.Text.Trim();  //20

                    DateTime date_begin_job = Convert.ToDateTime(this.dtptxtdate_begin_job.Value.ToString(),UsaCulture);
                    string d_date_begin_job = date_begin_job.ToString("yyyy-MM-dd");
                    cmd2.Parameters.Add("@txtdate_begin_job", SqlDbType.NVarChar).Value = d_date_begin_job;  //21

                    DateTime date_finish_job = Convert.ToDateTime(this.dtptxtdate_finish_job.Value.ToString(),UsaCulture);
                    string d_date_finish_job = date_finish_job.ToString("yyyy-MM-dd");
                    cmd2.Parameters.Add("@txtdate_finish_job", SqlDbType.NVarChar).Value = d_date_finish_job;  //22

                    cmd2.Parameters.Add("@txtqty_chan", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtqty_chan.Text.ToString()));  //23
                    cmd2.Parameters.Add("@txtqty_many_per_chan", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtqty_many_per_chan.Text.ToString()));  //24
                    cmd2.Parameters.Add("@txtqty_amount", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtqty_amount.Text.ToString()));  //25

                    cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.txtmat_no.Text.Trim();  //26
                    cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_id.Text.Trim();  //27
                    cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_name.Text.Trim();  //28
                    cmd2.Parameters.Add("@txtnumber_mat_id", SqlDbType.NVarChar).Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.Trim();  //29
                    cmd2.Parameters.Add("@txtnumber_color_id", SqlDbType.NVarChar).Value = this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text.Trim();  //29

                    cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //30
                    cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //31
                    cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtcurrency_rate.Text.ToString()));  //32

                    cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //33

                    cmd2.Parameters.Add("@txtcost_qty_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokma.Text.ToString()));  //34
                    cmd2.Parameters.Add("@txtcost_qty_price_average_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokma.Text.ToString()));  //35
                    cmd2.Parameters.Add("@txtcost_money_sum_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokma.Text.ToString()));  //36


                    cmd2.Parameters.Add("@txtsum_qty_pub", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_pub.Text.ToString()));  //37
                    cmd2.Parameters.Add("@txtsum_qty_pub_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //38
                    cmd2.Parameters.Add("@txtsum_qty_pub_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_pub.Text.ToString()));  //39

                    cmd2.Parameters.Add("@txtsum_qty_pub_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_pub_kg.Text.ToString()));  //40
                    cmd2.Parameters.Add("@txtsum_qty_pub_receive_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //41
                    cmd2.Parameters.Add("@txtsum_qty_pub_balance_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_pub_kg.Text.ToString()));  //42

                    cmd2.Parameters.Add("@txtsum_qty_rib", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_rib.Text.ToString()));  //43
                    cmd2.Parameters.Add("@txtsum_qty_rib_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //44
                    cmd2.Parameters.Add("@txtsum_qty_rib_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_rib.Text.ToString()));  //45

                    cmd2.Parameters.Add("@txtsum_qty_rib_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_rib_kg.Text.ToString()));  //46
                    cmd2.Parameters.Add("@txtsum_qty_rib_receive_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //47
                    cmd2.Parameters.Add("@txtsum_qty_rib_balance_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_rib_kg.Text.ToString()));  //48

                    cmd2.Parameters.Add("@txtsum_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //49
                    cmd2.Parameters.Add("@txtsum_qty_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //50
                    cmd2.Parameters.Add("@txtsum_qty_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString()));  //51


                    cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //52
                    cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //53
                    cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //54
                    cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //55
                    cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //56
                    cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //57
                    cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //58
                    cmd2.Parameters.Add("@txtmoney_after_vat_creditor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //59
                    cmd2.Parameters.Add("@txtcreditor_status", SqlDbType.NVarChar).Value = "0";  //60

                    cmd2.Parameters.Add("@txtcost_qty_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString()));  //61
                    cmd2.Parameters.Add("@txtcost_qty_price_average_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString()));  //62
                    cmd2.Parameters.Add("@txtcost_money_sum_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString()));  //63

                    cmd2.Parameters.Add("@txtcost_qty2_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokma.Text.ToString()));  //64
                    cmd2.Parameters.Add("@txtsum2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString()));  //65
                    cmd2.Parameters.Add("@txtcost_qty2_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString()));  //66

                    cmd2.Parameters.Add("@txtcs_status", SqlDbType.NVarChar).Value = "0";  //67
                    cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = "";  //68
                    cmd2.Parameters.Add("@txtfg_status", SqlDbType.NVarChar).Value = "";  //69
                    cmd2.Parameters.Add("@txtsew_status", SqlDbType.NVarChar).Value = "";  //70
                    cmd2.Parameters.Add("@txtSEW_id", SqlDbType.NVarChar).Value = "";  //70
                    cmd2.Parameters.Add("@txtpayment_status", SqlDbType.NVarChar).Value = "";  //71
                    cmd2.Parameters.Add("@txtacc_record_status", SqlDbType.NVarChar).Value = "";  //72
                    cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //73
                    cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);//73

                    //=====================================================================================================================================================
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");



                    //3 c002_08Cut_shirt_record_detail



                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {

                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();


                            if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                            {
                                //===================================================================================================================
                                //3 c002_08Cut_shirt_record_detail

                                cmd2.CommandText = "INSERT INTO c002_08Cut_shirt_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                               "txttrans_year,txttrans_month,txttrans_day," +

                                // //=================================================================
                              "txtCS_id," +  //6
                              "txtRG_id," +  //7
                               "txtshirt_size_id," +  //8
                              "txtnumber_dyed," +  //9
                               "txtsupplier_id," +  //10
                              "txtPPT_id," +  //11
                               "txtqc_id," +  //12
                                "txtnumber_in_year," +  //13
                                "txtwherehouse_id," +  //14
                                "txtfold_number," +  //15
                                "txtnumber_mat_id," +  //16
                                "txtnumber_color_id," +  //17
                                "txtface_baking_id," +  //18

                                "txtmat_no," +  //20
                                "txtmat_id," +  //21
                                "txtmat_name," +  //22

                                "txtmat_unit1_name," +  //23
                                "txtmat_unit1_qty," +  //24
                                 "chmat_unit_status," +  //25
                                "txtmat_unit2_name," +  //26
                                "txtmat_unit2_qty," +  //27

                                 "txtqty_want," +  //28
                                 "txtqty," +  //29
                                "txtqty2," +  //30
                                "txtqty_balance," +  //31

                                 "txtqty_want_pub," +  //32
                                 "txtqty_pub," +  //33
                                 "txtqty_balance_pub," +  //34

                                 "txtqty_want_rib," +  //35
                                 "txtqty_rib," +  //36
                                 "txtqty_balance_rib," +  //37


                                  "txtprice," +   //38
                                  "txtdiscount_rate," +  //39
                                  "txtdiscount_money," +  //40
                                  "txtsum_total," +  //41

                                   "txtcost_qty_balance_yokma," +  //42
                                   "txtcost_qty_price_average_yokma," +  //43
                                   "txtcost_money_sum_yokma," +  //44

                                   "txtcost_qty_balance_yokpai," +  //45
                                   "txtcost_qty_price_average_yokpai," +  //46
                                   "txtcost_money_sum_yokpai," +  //47

                                   "txtcost_qty2_balance_yokma," +  //48
                                   "txtcost_qty2_balance_yokpai," +  //49

                                "txtitem_no," +  //51
                                "txtmat_ppt_remark," +  //52

                                "txtlot_no," +  //53
                                "txtqty_berg_cut_shirt_balance," +  //54
                                "txtqty_sew_shirt_balance," +  //55

                               "txtSW_id) " +  //56

                               "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                               "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +

                                "'" + this.txtCS_id.Text.Trim() + "'," +  //  "txtCS_id," +  //6
                                "''," +  //7
                                "'" + this.GridView1.Rows[i].Cells["Col_txtshirt_size_id"].Value.ToString() + "'," +  //8
                                "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_dyed"].Value.ToString() + "'," +  //9
                                "'" + this.GridView1.Rows[i].Cells["Col_txtsupplier_id"].Value.ToString() + "'," +  //10
                                "''," +  //11


                                "'" + this.GridView1.Rows[i].Cells["Col_txtqc_id"].Value.ToString() + "'," +  //12
                                "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "'," +  //13
                                "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //14

                                "'" + this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() + "'," +  //15
                                "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Value.ToString() + "'," +  //16
                                "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_color_id"].Value.ToString() + "'," +  //17
                                "'" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "'," +  //18

                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //20
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //21
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //22

                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //23
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString())) + "'," +  //24
                                "'" + this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() + "'," +  //25
                                "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Value.ToString() + "'," +  //26
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString())) + "'," +  //27

                                 "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //28
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //29
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString())) + "'," +  //30
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //31

                                 "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) + "'," +  //32
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //33
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) + "'," +  //34

                                 "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString())) + "'," +  //35
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //36
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString())) + "'," +  //37


                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //38
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //39
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //40
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //41

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + "'," +  //42
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString())) + "'," +  //43
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + "'," +  //44

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //45
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //46
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //47

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + "'," +  //48
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value.ToString())) + "'," +  //49

                                "'" + this.GridView1.Rows[i].Cells["Col_txtitem_no"].Value.ToString() + "'," +  //51
                                "''," +  //52

                                "'" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "'," +  //53
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //54
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //55

                               "'')";    //56


                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok3");




                                cmd2.CommandText = "UPDATE c002_07Receive_Send_dye_record_detail SET " +
                                                   "txtCS_id = '" + this.txtCS_id.Text.Trim() + "'," +
                                                   "txtqty_berg_cut_shirt_balance = '" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                   " AND (txtface_baking_id = '" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "')" +
                                                   " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')";

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok7");



                                //=====================================================================================================
                            }
                        }
                    }



                    //สต๊อคสินค้า ตามคลัง =============================================================================================



                    //1.k021_mat_average
                    cmd2.CommandText = "UPDATE k021_mat_average SET " +
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


                            "'" + this.txtCS_id.Text.Trim() + "'," +  //7 txtbill_id
                            "'CS'," +  //9 txtbill_type
                            "'เบิกสั่งตัด " + this.txtshirt_cut_remark.Text.Trim() + "'," +  //9 txtbill_remark

                             "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                           "'" + this.txtmat_no.Text + "'," +  //10 
                            "'" + this.PANEL_MAT_txtmat_id.Text.ToString() + "'," +  //11
                            "'" + this.PANEL_MAT_txtmat_name.Text.ToString() + "'," +    //12

                            "'" + this.txtmat_unit1_name.Text.ToString() + "'," +  //13
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString())) + "'," +  //14
                            "'" + this.chmat_unit_status.Text.ToString() + "'," +  //15
                            "'" + this.txtmat_unit2_name.Text.ToString() + "'," +  //16
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +  //17

                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19 txtqty2_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //20 txtprice_in
                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //21 txtsum_total_in

                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty.Text.ToString())) + "'," +  //22 txtqty_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum2_qty.Text.ToString())) + "'," +  //23 txtqty2_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtprice.Text.ToString())) + "'," +  //24 txtprice_out
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtsum_total.Text.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +  //26
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'," +  //27
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +  //28
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +  //29

                           "'1')";   //30

                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok8");


                    //======================================

                    //สต๊อคสินค้า ตามคลัง =============================================================================================

                    //MessageBox.Show("ok4");


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

                        if (this.iblword_status.Text.Trim() == "บันทึกใบสั่งตัด")
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
            W_ID_Select.TRANS_ID = this.txtCS_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_08Cut_shirt_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_08Cut_shirt_record_print();
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
            W_ID_Select.TRANS_ID = this.txtCS_id.Text.Trim();
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
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_08Cut_shirt_record.rpt");


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
                rpt.SetParameterValue("txcs_id", W_ID_Select.TRANS_ID.Trim());

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

        private void datetxtdate_begin_job_ValueChanged(object sender, EventArgs e)
        {
            this.dtptxtdate_begin_job.Format = DateTimePickerFormat.Custom;
            this.dtptxtdate_begin_job.CustomFormat = this.dtptxtdate_begin_job.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear_begin_job.Text = this.dtptxtdate_begin_job.Value.ToString("yyyy", UsaCulture);

        }

        private void dtptxtdate_finish_job_ValueChanged(object sender, EventArgs e)
        {
            this.dtptxtdate_finish_job.Format = DateTimePickerFormat.Custom;
            this.dtptxtdate_finish_job.CustomFormat = this.dtptxtdate_finish_job.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear_finish_job.Text = this.dtptxtdate_finish_job.Value.ToString("yyyy", UsaCulture);

        }

        private void cbotxtcut_type_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbotxtcut_type_name.Text == "โรงงานเราเย็บเอง")
            {
                this.txtcut_type_id.Text = "01";

            }
            if (this.cbotxtcut_type_name.Text == "จ้างบริษัทอื่นเย็บ")
            {
                this.txtcut_type_id.Text = "02";

            }
            if (this.cbotxtcut_type_name.Text == "จ้างช่างตัดเย็บ")
            {
                this.txtcut_type_id.Text = "03";

            }
        }

        //txtwherehouse คลังสินค้า  =======================================================================
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
                                    "k016db_2supplier_address.*," +
                                    "k016db_3supplier_account.*" +

                                    " FROM k016db_1supplier" +

                                    " INNER JOIN k016db_2supplier_address" +
                                    " ON k016db_1supplier.cdkey = k016db_2supplier_address.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_2supplier_address.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_2supplier_address.txtsupplier_id" +

                                    " INNER JOIN k016db_3supplier_account" +
                                    " ON k016db_1supplier.cdkey = k016db_3supplier_account.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_3supplier_account.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_3supplier_account.txtsupplier_id" +


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
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcredit_day"].Value = dt2.Rows[j]["txtcredit_day"].ToString();      //8
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
            this.PANEL161_SUP_dataGridView1.ColumnCount = 10;
            this.PANEL161_SUP_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL161_SUP_dataGridView1.Columns[1].Name = "Col_txtsupplier_no";
            this.PANEL161_SUP_dataGridView1.Columns[2].Name = "Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[3].Name = "Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[4].Name = "Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
            this.PANEL161_SUP_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
            this.PANEL161_SUP_dataGridView1.Columns[7].Name = "Col_txtremark";
            this.PANEL161_SUP_dataGridView1.Columns[8].Name = "Col_txtcredit_day";
            this.PANEL161_SUP_dataGridView1.Columns[9].Name = "Col_txtsupplier_status";

            this.PANEL161_SUP_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderText = " ชื่อ Supplier";
            this.PANEL161_SUP_dataGridView1.Columns[4].HeaderText = " ชื่อ Supplier Eng";
            this.PANEL161_SUP_dataGridView1.Columns[5].HeaderText = " ผู้ติดต่อ";
            this.PANEL161_SUP_dataGridView1.Columns[6].HeaderText = " เบอร์โทร";
            this.PANEL161_SUP_dataGridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.PANEL161_SUP_dataGridView1.Columns[8].HeaderText = "เครดิต(วัน)";
            this.PANEL161_SUP_dataGridView1.Columns[9].HeaderText = " สถานะ";

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

            this.PANEL161_SUP_dataGridView1.Columns[8].Visible = false;  //"Col_txtcredit_day";
            this.PANEL161_SUP_dataGridView1.Columns[9].Visible = false;  //"Col_txtsupplier_status";

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
                if (this.PANEL161_SUP_dataGridView1.Rows[i].Cells[9].Value.ToString() == "0")  //Active
                {
                    this.PANEL161_SUP_dataGridView1.Rows[i].Cells[10].Value = true;
                }
                else
                {
                    this.PANEL161_SUP_dataGridView1.Rows[i].Cells[10].Value = false;

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
                    ////Col_txtcredit_day
                    //this.txtcredit_in_day.Text = row.Cells["Col_txtcredit_day"].Value.ToString();
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
                                    "k016db_2supplier_address.*," +
                                    "k016db_3supplier_account.*" +

                                    " FROM k016db_1supplier" +

                                    " INNER JOIN k016db_2supplier_address" +
                                    " ON k016db_1supplier.cdkey = k016db_2supplier_address.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_2supplier_address.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_2supplier_address.txtsupplier_id" +

                                    " INNER JOIN k016db_3supplier_account" +
                                    " ON k016db_1supplier.cdkey = k016db_3supplier_account.cdkey" +
                                    " AND k016db_1supplier.txtco_id = k016db_3supplier_account.txtco_id" +
                                    " AND k016db_1supplier.txtsupplier_id = k016db_3supplier_account.txtsupplier_id" +

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
                            PANEL161_SUP_dataGridView1.Rows[index].Cells["Col_txtcredit_day"].Value = dt2.Rows[j]["txtcredit_day"].ToString();      //8
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
        private void PANEL161_SUP_btnnew_Click(object sender, EventArgs e)
        {

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
        private void PANEL1306_WH_btnnew_Click(object sender, EventArgs e)
        {

        }


        //txtshirt_type รหัสชนิดงาน  =======================================================================
        private void PANEL0108_SHIRT_TYPE_Fill_shirt_type()
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

            PANEL0108_SHIRT_TYPE_Clear_GridView1_shirt_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_08shirt_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtshirt_type_id <> '')" +
                                    " ORDER BY txtshirt_type_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtshirt_type_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtshirt_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtshirt_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtshirt_type_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtshirt_type_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtshirt_type_status";

                            var index = PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows.Add();
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_no"].Value = dt2.Rows[j]["txtshirt_type_no"].ToString();      //1
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_id"].Value = dt2.Rows[j]["txtshirt_type_id"].ToString();      //2
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_name"].Value = dt2.Rows[j]["txtshirt_type_name"].ToString();      //3
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_name_eng"].Value = dt2.Rows[j]["txtshirt_type_name_eng"].ToString();      //4
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_remark"].Value = dt2.Rows[j]["txtshirt_type_remark"].ToString();      //5
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_status"].Value = dt2.Rows[j]["txtshirt_type_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type_Up_Status();

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
        private void PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows.Count; i++)
            {
                if (this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0108_SHIRT_TYPE_GridView1_shirt_type()
        {
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.ColumnCount = 7;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].Name = "Col_txtshirt_type_no";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].Name = "Col_txtshirt_type_id";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].Name = "Col_txtshirt_type_name";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].Name = "Col_txtshirt_type_name_eng";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].Name = "Col_txtshirt_type_remark";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[6].Name = "Col_txtshirt_type_status";

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[0].HeaderText = "No";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].HeaderText = " รหัส";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].HeaderText = " ชื่อรหัสสี";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].HeaderText = "ชื่อรหัสสี Eng";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[6].HeaderText = " สถานะ";

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[0].Visible = false;  //"No";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].Visible = true;  //"Col_txtshirt_type_no";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].Width = 90;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].ReadOnly = true;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].Visible = true;  //"Col_txtshirt_type_id";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].Width = 80;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].ReadOnly = true;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].Visible = true;  //"Col_txtshirt_type_name";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].Width = 150;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].ReadOnly = true;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].Visible = false;  //"Col_txtshirt_type_name_eng";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].Width = 0;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].ReadOnly = true;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].Visible = false;  //"Col_txtshirt_type_name_remark";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].Width = 0;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].ReadOnly = true;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[6].Visible = false;  //"Col_txtshirt_type_status";
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[6].Width = 0;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Columns.Add(dgvCmb);

        }
        private void PANEL0108_SHIRT_TYPE_Clear_GridView1_shirt_type()
        {
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows.Clear();
            this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Refresh();
        }
        private void PANEL0108_SHIRT_TYPE_txtshirt_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0108_SHIRT_TYPE.Visible == false)
                {
                    this.PANEL0108_SHIRT_TYPE.Visible = true;
                    this.PANEL0108_SHIRT_TYPE.Location = new Point(this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Location.X, this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Location.Y + 22);
                    this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Focus();
                }
                else
                {
                    this.PANEL0108_SHIRT_TYPE.Visible = false;
                }
        }
        private void PANEL0108_SHIRT_TYPE_btnshirt_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL0108_SHIRT_TYPE.Visible == false)
            {
                this.PANEL0108_SHIRT_TYPE.Visible = true;
                this.PANEL0108_SHIRT_TYPE.BringToFront();
                this.PANEL0108_SHIRT_TYPE.Location = new Point(this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Location.X, this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0108_SHIRT_TYPE.Visible = false;
            }
        }
        private void PANEL0108_SHIRT_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0108_SHIRT_TYPE.Visible == false)
            {
                this.PANEL0108_SHIRT_TYPE.Visible = true;
            }
            else
            {
                this.PANEL0108_SHIRT_TYPE.Visible = false;
            }
        }
        private void PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.CurrentRow.Index;

                this.PANEL0108_SHIRT_TYPE_txtshirt_type_id.Text = PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Text = PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0108_SHIRT_TYPE_txtshirt_type_name.Focus();
                this.PANEL0108_SHIRT_TYPE.Visible = false;
            }
        }
        private void PANEL0108_SHIRT_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0108_SHIRT_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL0108_SHIRT_TYPE_Clear_GridView1_shirt_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_08shirt_type" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtshirt_type_name LIKE '%" + this.PANEL0108_SHIRT_TYPE_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtshirt_type_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtshirt_type_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtshirt_type_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtshirt_type_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtshirt_type_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtshirt_type_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtshirt_type_status";

                            var index = PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows.Add();
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_no"].Value = dt2.Rows[j]["txtshirt_type_no"].ToString();      //1
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_id"].Value = dt2.Rows[j]["txtshirt_type_id"].ToString();      //2
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_name"].Value = dt2.Rows[j]["txtshirt_type_name"].ToString();      //3
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_name_eng"].Value = dt2.Rows[j]["txtshirt_type_name_eng"].ToString();      //4
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_remark"].Value = dt2.Rows[j]["txtshirt_type_remark"].ToString();      //5
                            PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type.Rows[index].Cells["Col_txtshirt_type_status"].Value = dt2.Rows[j]["txtshirt_type_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0108_SHIRT_TYPE_dataGridView1_shirt_type_Up_Status();

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
        private void PANEL0108_SHIRT_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0108_SHIRT_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0108_SHIRT_TYPE.Height = PANEL0108_SHIRT_TYPE_btnresize_low.Top + e.Y;
                this.PANEL0108_SHIRT_TYPE.Width = PANEL0108_SHIRT_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0108_SHIRT_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0108_SHIRT_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }

        //txtshirt_size ไซส์เสื้อ  =======================================================================
        private void PANEL0109_SHIRT_SIZE_Fill_shirt_size()
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

            PANEL0109_SHIRT_SIZE_Clear_GridView1_shirt_size();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_09shirt_size" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtshirt_size_id <> '')" +
                                    " ORDER BY txtshirt_size_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtshirt_size_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtshirt_size_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtshirt_size_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtshirt_size_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtshirt_size_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtshirt_size_status";

                            var index = PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows.Add();
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_no"].Value = dt2.Rows[j]["txtshirt_size_no"].ToString();      //1
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_id"].Value = dt2.Rows[j]["txtshirt_size_id"].ToString();      //2
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_name"].Value = dt2.Rows[j]["txtshirt_size_name"].ToString();      //3
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_name_eng"].Value = dt2.Rows[j]["txtshirt_size_name_eng"].ToString();      //4
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_remark"].Value = dt2.Rows[j]["txtshirt_size_remark"].ToString();      //5
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_status"].Value = dt2.Rows[j]["txtshirt_size_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size_Up_Status();

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
        private void PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows.Count; i++)
            {
                if (this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0109_SHIRT_SIZE_GridView1_shirt_size()
        {
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.ColumnCount = 7;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[0].Name = "Col_Auto_num";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].Name = "Col_txtshirt_size_no";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].Name = "Col_txtshirt_size_id";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].Name = "Col_txtshirt_size_name";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].Name = "Col_txtshirt_size_name_eng";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].Name = "Col_txtshirt_size_remark";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[6].Name = "Col_txtshirt_size_status";

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[0].HeaderText = "No";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].HeaderText = " รหัส";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].HeaderText = " ชื่อรหัสสี";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].HeaderText = "ชื่อรหัสสี Eng";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[6].HeaderText = " สถานะ";

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[0].Visible = false;  //"No";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].Visible = true;  //"Col_txtshirt_size_no";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].Width = 90;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].ReadOnly = true;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].Visible = true;  //"Col_txtshirt_size_id";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].Width = 80;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].ReadOnly = true;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].Visible = true;  //"Col_txtshirt_size_name";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].Width = 150;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].ReadOnly = true;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].Visible = false;  //"Col_txtshirt_size_name_eng";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].Width = 0;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].ReadOnly = true;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].Visible = false;  //"Col_txtshirt_size_name_remark";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].Width = 0;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].ReadOnly = true;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[6].Visible = false;  //"Col_txtshirt_size_status";
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[6].Width = 0;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Columns.Add(dgvCmb);

        }
        private void PANEL0109_SHIRT_SIZE_Clear_GridView1_shirt_size()
        {
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows.Clear();
            this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Refresh();
        }
        private void PANEL0109_SHIRT_SIZE_txtshirt_size_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0109_SHIRT_SIZE.Visible == false)
                {
                    this.PANEL0109_SHIRT_SIZE.Visible = true;
                    this.PANEL0109_SHIRT_SIZE.Location = new Point(this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Location.X, this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Location.Y + 22);
                    this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Focus();
                }
                else
                {
                    this.PANEL0109_SHIRT_SIZE.Visible = false;
                }
        }
        private void PANEL0109_SHIRT_SIZE_btnshirt_size_Click(object sender, EventArgs e)
        {
            if (this.PANEL0109_SHIRT_SIZE.Visible == false)
            {
                this.PANEL0109_SHIRT_SIZE.Visible = true;
                this.PANEL0109_SHIRT_SIZE.BringToFront();
                this.PANEL0109_SHIRT_SIZE.Location = new Point(this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Location.X, this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0109_SHIRT_SIZE.Visible = false;
            }
        }
        private void PANEL0109_SHIRT_SIZE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0109_SHIRT_SIZE.Visible == false)
            {
                this.PANEL0109_SHIRT_SIZE.Visible = true;
            }
            else
            {
                this.PANEL0109_SHIRT_SIZE.Visible = false;
            }
        }
        private void PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.CurrentRow.Index;

                this.PANEL0109_SHIRT_SIZE_txtshirt_size_id.Text = PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Text = PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0109_SHIRT_SIZE_txtshirt_size_name.Focus();
                this.PANEL0109_SHIRT_SIZE.Visible = false;
            }
        }
        private void PANEL0109_SHIRT_SIZE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0109_SHIRT_SIZE_btn_search_Click(object sender, EventArgs e)
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

            PANEL0109_SHIRT_SIZE_Clear_GridView1_shirt_size();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_09shirt_size" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtshirt_size_name LIKE '%" + this.PANEL0109_SHIRT_SIZE_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtshirt_size_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtshirt_size_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtshirt_size_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtshirt_size_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtshirt_size_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtshirt_size_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtshirt_size_status";

                            var index = PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows.Add();
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_no"].Value = dt2.Rows[j]["txtshirt_size_no"].ToString();      //1
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_id"].Value = dt2.Rows[j]["txtshirt_size_id"].ToString();      //2
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_name"].Value = dt2.Rows[j]["txtshirt_size_name"].ToString();      //3
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_name_eng"].Value = dt2.Rows[j]["txtshirt_size_name_eng"].ToString();      //4
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_remark"].Value = dt2.Rows[j]["txtshirt_size_remark"].ToString();      //5
                            PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size.Rows[index].Cells["Col_txtshirt_size_status"].Value = dt2.Rows[j]["txtshirt_size_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0109_SHIRT_SIZE_dataGridView1_shirt_size_Up_Status();

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
        private void PANEL0109_SHIRT_SIZE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0109_SHIRT_SIZE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0109_SHIRT_SIZE.Height = PANEL0109_SHIRT_SIZE_btnresize_low.Top + e.Y;
                this.PANEL0109_SHIRT_SIZE.Width = PANEL0109_SHIRT_SIZE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0109_SHIRT_SIZE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0109_SHIRT_SIZE_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtshirt_size ไซส์เสื้อ =======================================================================


        //END txtshirt_size ไซส์เสื้อ =======================================================================

        //txtroom_collect ห้องเก็บ  =======================================================================
        private void PANEL0110_ROOM_COLLECT_Fill_room_collect()
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

            PANEL0110_ROOM_COLLECT_Clear_GridView1_room_collect();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_10room_collect" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtroom_collect_id <> '')" +
                                    " ORDER BY txtroom_collect_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtroom_collect_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtroom_collect_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtroom_collect_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtroom_collect_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtroom_collect_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtroom_collect_status";

                            var index = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Add();
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_no"].Value = dt2.Rows[j]["txtroom_collect_no"].ToString();      //1
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_id"].Value = dt2.Rows[j]["txtroom_collect_id"].ToString();      //2
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name"].Value = dt2.Rows[j]["txtroom_collect_name"].ToString();      //3
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name_eng"].Value = dt2.Rows[j]["txtroom_collect_name_eng"].ToString();      //4
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_remark"].Value = dt2.Rows[j]["txtroom_collect_remark"].ToString();      //5
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_status"].Value = dt2.Rows[j]["txtroom_collect_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_Up_Status();

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
        private void PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Count; i++)
            {
                if (this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0110_ROOM_COLLECT_GridView1_room_collect()
        {
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnCount = 7;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[0].Name = "Col_Auto_num";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].Name = "Col_txtroom_collect_no";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].Name = "Col_txtroom_collect_id";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].Name = "Col_txtroom_collect_name";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].Name = "Col_txtroom_collect_name_eng";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].Name = "Col_txtroom_collect_remark";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].Name = "Col_txtroom_collect_status";

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[0].HeaderText = "No";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].HeaderText = " รหัส";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].HeaderText = " ชื่อรหัสสี";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].HeaderText = "ชื่อรหัสสี Eng";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].HeaderText = " สถานะ";

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[0].Visible = false;  //"No";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].Visible = true;  //"Col_txtroom_collect_no";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].Width = 90;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].Visible = true;  //"Col_txtroom_collect_id";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].Width = 80;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].Visible = true;  //"Col_txtroom_collect_name";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].Width = 150;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].Visible = false;  //"Col_txtroom_collect_name_eng";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].Width = 0;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].Visible = false;  //"Col_txtroom_collect_name_remark";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].Width = 0;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].ReadOnly = true;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].Visible = false;  //"Col_txtroom_collect_status";
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].Width = 0;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Columns.Add(dgvCmb);

        }
        private void PANEL0110_ROOM_COLLECT_Clear_GridView1_room_collect()
        {
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Clear();
            this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Refresh();
        }
        private void PANEL0110_ROOM_COLLECT_txtroom_collect_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0110_ROOM_COLLECT.Visible == false)
                {
                    this.PANEL0110_ROOM_COLLECT.Visible = true;
                    this.PANEL0110_ROOM_COLLECT.Location = new Point(this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.X, this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.Y + 22);
                    this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Focus();
                }
                else
                {
                    this.PANEL0110_ROOM_COLLECT.Visible = false;
                }
        }
        private void PANEL0110_ROOM_COLLECT_btnroom_collect_Click(object sender, EventArgs e)
        {
            if (this.PANEL0110_ROOM_COLLECT.Visible == false)
            {
                this.PANEL0110_ROOM_COLLECT.Visible = true;
                this.PANEL0110_ROOM_COLLECT.BringToFront();
                this.PANEL0110_ROOM_COLLECT.Location = new Point(this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.X, this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0110_ROOM_COLLECT.Visible = false;
            }
        }
        private void PANEL0110_ROOM_COLLECT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0110_ROOM_COLLECT.Visible == false)
            {
                this.PANEL0110_ROOM_COLLECT.Visible = true;
            }
            else
            {
                this.PANEL0110_ROOM_COLLECT.Visible = false;
            }
        }
        private void PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0110_ROOM_COLLECT_txtroom_collect_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.CurrentRow.Index;

                this.PANEL0110_ROOM_COLLECT_txtroom_collect_id.Text = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Text = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0110_ROOM_COLLECT_txtroom_collect_name.Focus();
                this.PANEL0110_ROOM_COLLECT.Visible = false;
            }
        }
        private void PANEL0110_ROOM_COLLECT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0110_ROOM_COLLECT_btn_search_Click(object sender, EventArgs e)
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

            PANEL0110_ROOM_COLLECT_Clear_GridView1_room_collect();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_10room_collect" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtroom_collect_name LIKE '%" + this.PANEL0110_ROOM_COLLECT_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtroom_collect_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtroom_collect_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtroom_collect_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtroom_collect_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtroom_collect_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtroom_collect_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtroom_collect_status";

                            var index = PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows.Add();
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_no"].Value = dt2.Rows[j]["txtroom_collect_no"].ToString();      //1
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_id"].Value = dt2.Rows[j]["txtroom_collect_id"].ToString();      //2
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name"].Value = dt2.Rows[j]["txtroom_collect_name"].ToString();      //3
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_name_eng"].Value = dt2.Rows[j]["txtroom_collect_name_eng"].ToString();      //4
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_remark"].Value = dt2.Rows[j]["txtroom_collect_remark"].ToString();      //5
                            PANEL0110_ROOM_COLLECT_dataGridView1_room_collect.Rows[index].Cells["Col_txtroom_collect_status"].Value = dt2.Rows[j]["txtroom_collect_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0110_ROOM_COLLECT_dataGridView1_room_collect_Up_Status();

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
        private void PANEL0110_ROOM_COLLECT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0110_ROOM_COLLECT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0110_ROOM_COLLECT.Height = PANEL0110_ROOM_COLLECT_btnresize_low.Top + e.Y;
                this.PANEL0110_ROOM_COLLECT.Width = PANEL0110_ROOM_COLLECT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0110_ROOM_COLLECT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0110_ROOM_COLLECT_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtroom_collect ห้องเก็บ =======================================================================


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

                    cmd2.CommandText = "UPDATE c002_08Cut_shirt_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtCS_id = '" + this.txtCS_id.Text.Trim() + "')";
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
                                  " FROM c002_08Cut_shirt_record_trans" +
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

                        transNum = Convert.ToDouble(string.Format("{0:n4}", trans_Right6)) + Convert.ToDouble(string.Format("{0:n4}", 1));
                        trans = transNum.ToString("00000#");

                        if (year2.Trim() == year_now2.Trim())
                        {
                            TMP = "CS" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "CS" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "CS" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtCS_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }

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
                                                    " AND (txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
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
                                //=======================================================
                                Cursor.Current = Cursors.WaitCursor;
                                //conn.Open();
                                //if (conn.State == System.Data.ConnectionState.Open)
                                //{

                                //SqlCommand cmd2 = conn.CreateCommand();
                                //cmd2.CommandType = CommandType.Text;
                                //cmd2.Connection = conn;

                                SqlTransaction trans;
                                trans = conn.BeginTransaction();
                                cmd2.Transaction = trans;
                                //try
                                //{

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
                               "txtcost_qty2_balance) " +  //14
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
                               "@txtcost_qty2_balance)";   //14

                                cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                                cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();  //1

                                cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();  //2
                                cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString();  //3
                                cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();  //4
                                cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString();  //5
                                cmd2.Parameters.Add("@txtmat_unit1_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString()));  //6
                                cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString();  //7
                                cmd2.Parameters.Add("@txtmat_unit2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));  //8

                                cmd2.Parameters.Add("@txtcost_qty_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //9
                                cmd2.Parameters.Add("@txtcost_qty_price_average", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //10
                                cmd2.Parameters.Add("@txtcost_money_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //11

                                cmd2.Parameters.Add("@txtcost_qty2_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //13

                                //==============================

                                cmd2.ExecuteNonQuery();


                                Cursor.Current = Cursors.WaitCursor;
                                trans.Commit();
                                //conn.Close();

                                Cursor.Current = Cursors.Default;


                                //conn.Close();
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        //conn.Close();
                                //        MessageBox.Show("kondate.soft", ex.Message);
                                //        return;
                                //    }
                                //    finally
                                //    {
                                //        //conn.Close();
                                //    }
                                //}
                                //=============================================================


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

                    }
                }
            }
            // END INSERT ชื่อสินค้าที่สต๊อค ยังไม่มี

        }
        //จบส่วนตารางสำหรับบันทึก========================================================================



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

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

        }

        private void BtnGrid_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = "ระเบียนใบตัดผ้าพับ";
            kondate.soft.HOME03_Production.HOME03_Production_08Cut_shirt frm2 = new kondate.soft.HOME03_Production.HOME03_Production_08Cut_shirt();
            frm2.Show();

        }



        //=============================================================

        //=========================================================


    }
}
