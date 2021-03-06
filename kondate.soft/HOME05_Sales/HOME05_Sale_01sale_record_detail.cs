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

namespace kondate.soft.HOME05_Sales
{
    public partial class HOME05_Sale_01sale_record_detail : Form
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

        public HOME05_Sale_01sale_record_detail()
        {
            InitializeComponent();
        }

        private void HOME05_Sale_01sale_record_detail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0501IVDL";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิด";
            TRANS_LOG();

            this.iblword_status.Text = "ดูข้อมูลใบส่งสินค้า";

            this.ActiveControl = this.txtde_remark;
            this.BtnNew.Enabled = false;
            this.btnopen.Enabled = false;
            this.BtnSave.Enabled = false;
            this.BtnCancel_Doc.Enabled = true;
            this.btnPreview.Enabled = true;
            this.BtnPrint.Enabled = true;

            Show_GridView1();
            Fill_DATA_TO_GridView1();
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();


        }
        //GridView1   =======================================================================
        int selectedRowIndex;
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

            //MessageBox.Show("" + W_ID_Select.CDKEY.Trim() + "");
            //MessageBox.Show("" + W_ID_Select.M_COID.Trim() + "");
            //MessageBox.Show("" + W_ID_Select.M_BRANCHID.Trim() + "");
            //MessageBox.Show("" + W_ID_Select.TRANS_ID.Trim() + "");

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT s001_01sale_record.*," +
                                    "s001_01sale_record_detail.*," +
                                    "s001_03cus.*," +
                                    "b001mat.*," +
                                    "b001mat_02detail.*," +
                                   //"b001mat_06price_sale.*," +
                                   "k013_1db_acc_13group_tax.*," +
                                   "k013_1db_acc_06wherehouse.*," +

                                    "b001_05mat_unit1.*" +

                                    " FROM s001_01sale_record" +

                                    " INNER JOIN s001_01sale_record_detail" +
                                    " ON s001_01sale_record.cdkey = s001_01sale_record_detail.cdkey" +
                                    " AND s001_01sale_record.txtco_id = s001_01sale_record_detail.txtco_id" +
                                    " AND s001_01sale_record.txtDE_id = s001_01sale_record_detail.txtDE_id" +

                                    " INNER JOIN s001_03cus" +
                                    " ON s001_01sale_record.cdkey = s001_03cus.cdkey" +
                                    " AND s001_01sale_record.txtco_id = s001_03cus.txtco_id" +
                                    " AND s001_01sale_record.txtcus_id = s001_03cus.txtcus_id" +

                                    " INNER JOIN b001mat" +
                                    " ON s001_01sale_record_detail.cdkey = b001mat.cdkey" +
                                    " AND s001_01sale_record_detail.txtco_id = b001mat.txtco_id" +
                                    " AND s001_01sale_record_detail.txtmat_id = b001mat.txtmat_id" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON s001_01sale_record_detail.cdkey = b001mat_02detail.cdkey" +
                                    " AND s001_01sale_record_detail.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND s001_01sale_record_detail.txtmat_id = b001mat_02detail.txtmat_id" +

                                   //" INNER JOIN b001mat_06price_sale" +
                                   //" ON s001_01sale_record_detail.cdkey = b001mat_06price_sale.cdkey" +
                                   //" AND s001_01sale_record_detail.txtco_id = b001mat_06price_sale.txtco_id" +
                                   //" AND s001_01sale_record_detail.txtmat_id = b001mat_06price_sale.txtmat_id" +
                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON s001_01sale_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +

                                    " INNER JOIN k013_1db_acc_06wherehouse" +
                                    " ON s001_01sale_record_detail.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                    " AND s001_01sale_record_detail.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                    " AND s001_01sale_record_detail.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +


                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " WHERE (s001_01sale_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_01sale_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (s001_01sale_record.txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')" +
                                    " AND (s001_01sale_record.txtDE_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                    " ORDER BY s001_01sale_record_detail.ID ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {

                        this.txtDE_id.Text = dt2.Rows[0]["txtDE_id"].ToString();
                        this.txtsale_type_id.Text = dt2.Rows[0]["txtsale_type_id"].ToString();

                        this.PANEL1306_WH_txtwherehouse_id.Text = dt2.Rows[0]["txtwherehouse_id"].ToString();
                        this.PANEL1306_WH_txtwherehouse_name.Text = dt2.Rows[0]["txtwherehouse_name"].ToString();

                        this.PANEL103_CUS_txtcus_id.Text = dt2.Rows[0]["txtcus_id"].ToString();
                        this.PANEL103_CUS_txtcus_name.Text = dt2.Rows[0]["txtcus_name"].ToString();

                        this.dtpdate_record.Value = Convert.ToDateTime(dt2.Rows[0]["txttrans_date_server"].ToString());
                        this.dtpdate_record.Format = DateTimePickerFormat.Custom;
                        this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
                        this.txtde_remark.Text = dt2.Rows[0]["txtde_remark"].ToString();

                        this.Paneldate_txtcurrency_date.Text = dt2.Rows[0]["txtcurrency_date"].ToString();
                        this.txtcurrency_id.Text = dt2.Rows[0]["txtcurrency_id"].ToString();
                        this.txtcurrency_rate.Text = dt2.Rows[0]["txtcurrency_rate"].ToString();

                        this.txtemp_office_name.Text = dt2.Rows[0]["txtemp_office_name"].ToString();
                        this.txtconsignee.Text = dt2.Rows[0]["txtconsignee"].ToString();
                        this.txtemp_office_name_manager.Text = dt2.Rows[0]["txtemp_office_name_manager"].ToString();
                        this.txtemp_office_name_approve.Text = dt2.Rows[0]["txtemp_office_name_approve"].ToString();


                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt2.Rows[0]["txtacc_group_tax_name"].ToString();
                        this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt2.Rows[0]["txtacc_group_tax_id"].ToString();
                        this.txtvat_rate.Text = Convert.ToSingle(dt2.Rows[0]["txtvat_rate"]).ToString("###,###.00");


                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {

                            this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = dt2.Rows[j]["txtORDER_id"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //2

                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //6

                            GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_Yokma"]).ToString("###,###.00");//7
                            GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_Remind"]).ToString("###,###.00"); //8
                            GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_Plus"]).ToString("###,###.00"); //9
                            GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_first"]).ToString("###,###.00"); //10
                            GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_out"]).ToString("###,###.00"); //11
                            GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_out_cut"]).ToString("###,###.00"); //12
                            GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtQTY_Balance"]).ToString("###,###.00"); //13


                            GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //14
                            GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //15
                            GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //16
                            GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00"); // Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //17


                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                            GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = "0";      //19
                            GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = "0";      //20

                            GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //21
                            GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //22
                            GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //23

                            GridView1.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtcoPO_id"].ToString();        //24
                            GridView1.Rows[index].Cells["Col_txtbranch_id"].Value = dt2.Rows[j]["txtbranchPO_id"].ToString();        //25

                            GridView1.Rows[index].Cells["Col_txtqtyPO_want"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyPO_want"]).ToString("###,###.00"); //26
                            GridView1.Rows[index].Cells["Col_txtqtyPO"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyPO"]).ToString("###,###.00"); //27
                            GridView1.Rows[index].Cells["Col_txtqtyPO_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyPO_balance"]).ToString("###,###.00"); //28


                            Cursor.Current = Cursors.Default;
                        }
                        //=======================================================

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
            GridView1_Color_Column();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();

        }
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 32;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtORDER_id";
            this.GridView1.Columns[2].Name = "Col_txtwherehouse_id";

            this.GridView1.Columns[3].Name = "Col_txtmat_no";
            this.GridView1.Columns[4].Name = "Col_txtmat_id";
            this.GridView1.Columns[5].Name = "Col_txtmat_name";
            this.GridView1.Columns[6].Name = "Col_txtmat_unit1_name";

            this.GridView1.Columns[7].Name = "Col_txtQTY_Yokma";
            this.GridView1.Columns[8].Name = "Col_txtQTY_Remind";
            this.GridView1.Columns[9].Name = "Col_txtQTY_Plus";
            this.GridView1.Columns[10].Name = "Col_txtQTY_first";
            this.GridView1.Columns[11].Name = "Col_txtQTY_out";
            this.GridView1.Columns[12].Name = "Col_txtQTY_out_cut";
            this.GridView1.Columns[13].Name = "Col_txtQTY_Balance";

            this.GridView1.Columns[14].Name = "Col_txtprice";
            this.GridView1.Columns[15].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[16].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[17].Name = "Col_txtsum_total";

            this.GridView1.Columns[18].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[19].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[20].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[21].Name = "Col_txtcost_sale_qty_price_average";
            this.GridView1.Columns[22].Name = "Col_txtcost_sale_money_sum";

            this.GridView1.Columns[23].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[24].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[25].Name = "Col_txtcost_money_sum_yokpai";
            this.GridView1.Columns[26].Name = "Col_txtco_id";
            this.GridView1.Columns[27].Name = "Col_txtbranch_id";

            this.GridView1.Columns[28].Name = "Col_mat_status";

            this.GridView1.Columns[29].Name = "Col_txtqtyPO_want";
            this.GridView1.Columns[30].Name = "Col_txtqtyPO";
            this.GridView1.Columns[31].Name = "Col_txtqtyPO_balance";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "อ้างอิงใบสั่งซื้อ";
            this.GridView1.Columns[2].HeaderText = "คลัง";

            this.GridView1.Columns[3].HeaderText = "ลำดับ";
            this.GridView1.Columns[4].HeaderText = " รหัส";
            this.GridView1.Columns[5].HeaderText = " ชื่อสินค้า";
            this.GridView1.Columns[6].HeaderText = "หน่วย";

            this.GridView1.Columns[7].HeaderText = "สต๊อคเหลือ";
            this.GridView1.Columns[8].HeaderText = "เหลือมา";
            this.GridView1.Columns[9].HeaderText = "เบิกเพิ่ม";
            this.GridView1.Columns[10].HeaderText = "เบิกแรก";
            this.GridView1.Columns[11].HeaderText = "จำนวนขาย";
            this.GridView1.Columns[12].HeaderText = "ขายได้";
            this.GridView1.Columns[13].HeaderText = "ขายสุทธิ";

            this.GridView1.Columns[14].HeaderText = "ราคา";
            this.GridView1.Columns[15].HeaderText = "ส่วนลด%";
            this.GridView1.Columns[16].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[17].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[18].HeaderText = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[19].HeaderText = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[20].HeaderText = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[21].HeaderText = "txtcost_sale_qty_price_average";
            this.GridView1.Columns[22].HeaderText = "txtcost_sale_money_sum";

            this.GridView1.Columns[23].HeaderText = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[24].HeaderText = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[25].HeaderText = "Col_txtcost_money_sum_yokpai";
            this.GridView1.Columns[26].HeaderText = "Col_txtco_id";
            this.GridView1.Columns[27].HeaderText = "Col_txtbranch_id";

            this.GridView1.Columns[28].HeaderText = "Col_mat_status";

            this.GridView1.Columns[29].HeaderText = "Col_txtqtyPO_want";
            this.GridView1.Columns[30].HeaderText = "Col_txtqtyPO";
            this.GridView1.Columns[31].HeaderText = "Col_txtqtyPO_balance";

            this.GridView1.Columns["Col_Auto_num"].Visible = false;  //"No";

            this.GridView1.Columns["Col_txtORDER_id"].Visible = true;  //"Col_txtORDER_id";
            this.GridView1.Columns["Col_txtORDER_id"].Width = 140;
            this.GridView1.Columns["Col_txtORDER_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtORDER_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtORDER_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtwherehouse_id"].Visible = true;  //"Col_txtwherehouse_id";
            this.GridView1.Columns["Col_txtwherehouse_id"].Width = 100;
            this.GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no";
            this.GridView1.Columns["Col_txtmat_no"].Width = 100;
            this.GridView1.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns["Col_txtmat_id"].Width = 120;
            this.GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns["Col_txtmat_name"].Width = 250;
            this.GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView1.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.GridView1.Columns["Col_txtmat_unit1_name"].Width = 140;
            this.GridView1.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView1.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;



            this.GridView1.Columns["Col_txtQTY_Yokma"].Visible = true;  //"Col_txtQTY_Yokma";
            this.GridView1.Columns["Col_txtQTY_Yokma"].Width = 100;
            this.GridView1.Columns["Col_txtQTY_Yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtQTY_Yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_Yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtQTY_Remind"].Visible = false;  //"Col_txtQTY_Remind";
            this.GridView1.Columns["Col_txtQTY_Remind"].Width = 0;
            this.GridView1.Columns["Col_txtQTY_Remind"].ReadOnly = true;
            this.GridView1.Columns["Col_txtQTY_Remind"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_Remind"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtQTY_Plus"].Visible = false;  //"Col_txtQTY_Plus";
            this.GridView1.Columns["Col_txtQTY_Plus"].Width = 0;
            this.GridView1.Columns["Col_txtQTY_Plus"].ReadOnly = true;
            this.GridView1.Columns["Col_txtQTY_Plus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_Plus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtQTY_first"].Visible = false;  //"Col_txtQTY_first";
            this.GridView1.Columns["Col_txtQTY_first"].Width = 0;
            this.GridView1.Columns["Col_txtQTY_first"].ReadOnly = true;
            this.GridView1.Columns["Col_txtQTY_first"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_first"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtQTY_out"].Visible = true;  //"Col_txtQTY_out";
            this.GridView1.Columns["Col_txtQTY_out"].Width = 100;
            this.GridView1.Columns["Col_txtQTY_out"].ReadOnly = false;
            this.GridView1.Columns["Col_txtQTY_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtQTY_out_cut"].Visible = false;  //"Col_txtQTY_out_cut";
            this.GridView1.Columns["Col_txtQTY_out_cut"].Width = 0;
            this.GridView1.Columns["Col_txtQTY_out_cut"].ReadOnly = true;
            this.GridView1.Columns["Col_txtQTY_out_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_out_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtQTY_Balance"].Visible = false;  //"Col_txtQTY_Balance";
            this.GridView1.Columns["Col_txtQTY_Balance"].Width = 0;
            this.GridView1.Columns["Col_txtQTY_Balance"].ReadOnly = true;
            this.GridView1.Columns["Col_txtQTY_Balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtQTY_Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView1.Columns["Col_txtprice"].Visible = true;  //"Col_txtprice";
            this.GridView1.Columns["Col_txtprice"].Width = 80;
            this.GridView1.Columns["Col_txtprice"].ReadOnly = true;
            this.GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.GridView1.Columns["Col_txtdiscount_rate"].Width = 0;
            this.GridView1.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.GridView1.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtdiscount_money"].Visible = true;  //"Col_txtdiscount_money";
            this.GridView1.Columns["Col_txtdiscount_money"].Width = 100;
            this.GridView1.Columns["Col_txtdiscount_money"].ReadOnly = true;
            this.GridView1.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtsum_total"].Visible = true;  //"Col_txtsum_total";
            this.GridView1.Columns["Col_txtsum_total"].Width = 100;
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

            this.GridView1.Columns["Col_txtcost_sale_qty_price_average"].Visible = false;  //"Col_txtcost_sale_qty_price_average";
            this.GridView1.Columns["Col_txtcost_sale_qty_price_average"].Width = 0;
            this.GridView1.Columns["Col_txtcost_sale_qty_price_average"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_sale_qty_price_average"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_sale_qty_price_average"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView1.Columns["Col_txtcost_sale_money_sum"].Visible = false;  //"Col_txtcost_sale_money_sum";
            this.GridView1.Columns["Col_txtcost_sale_money_sum"].Width = 0;
            this.GridView1.Columns["Col_txtcost_sale_money_sum"].ReadOnly = true;
            this.GridView1.Columns["Col_txtcost_sale_money_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtcost_sale_money_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


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

            this.GridView1.Columns["Col_txtco_id"].Visible = false;  //"Col_txtco_id";
            this.GridView1.Columns["Col_txtbranch_id"].Visible = false;  //"Col_txtbranch_id";

            this.GridView1.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";

            this.GridView1.Columns["Col_txtqtyPO_want"].Visible = false;  //"Col_txtqtyPO_want";
            this.GridView1.Columns["Col_txtqtyPO"].Visible = false;  //"Col_txtqtyPO";
            this.GridView1.Columns["Col_txtqtyPO_balance"].Visible = false;  //"Col_txtqtyPO_balance";


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
        private void CLEAR_TXT()
        {

        }
        private void GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = GridView1.CurrentRow.Index;
            //this.btnremove_row.Visible = true;
        }
        private void btnremove_row_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการ ลบรายการแถว ที่คลิ๊ก ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                //DataGridViewRow row = new DataGridViewRow();
                //row = this.PANEL161_SUP_dataGridView2.Rows[selectedRowIndex];
                this.GridView1.Rows.RemoveAt(selectedRowIndex);
                GridView1_Cal_Sum();
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
        private void GridView1_Color_Column()
        {

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                GridView1.Rows[i].Cells["Col_txtQTY_out"].Style.BackColor = Color.LightSkyBlue;
            }
        }
        private void GridView1_Cal_Sum()
        {
            double Sum_Total = 0;
            double Sum_Total2 = 0;
            double Sum_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            double Sum_Total_cost = 0;

            double QAbyma = 0;
            double Qbypai = 0;
            double Mbypai = 0;
            double QAbypai = 0;


            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells["Col_Auto_num"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();
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

                    //5 * 6 = 8

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

                    //if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value.ToString())))
                    //{
                    //    MessageBox.Show("สต๊อคคงเหลือ น้อยกว่า จำนวน ขาย ไม่สามารถขายได้ !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value).ToString("###,###.00");     //5
                    //    return;
                    //}

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    //Sum_Total_cost  =================================================
                    Sum_Total_cost = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value = Sum_Total_cost.ToString("N", new CultureInfo("en-US"));


                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                        this.txtsum_qty_iv.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));


                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                        this.txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }

                    this.GridView1.Rows[i].Cells["Col_txtqtyPO"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value).ToString("###,###.00");
                    //Sum_Total  =================================================
                    Sum_Total2 = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_want"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value = Sum_Total2.ToString("N", new CultureInfo("en-US"));

                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //1.เหลือยกมา - ขาย = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา - มูลค่าขาย = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) / Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";
                    }
                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                }
            }

            this.txtcount_rows.Text = k.ToString();

            Sum_Total = 0;
            Sum_Total2 = 0;
            Sum_Qty = 0;
            Sum_Price = 0;
            Sum_Discount = 0;
            MoneySum = 0;
            Sum_Total_cost = 0;

            QAbyma = 0;
            Qbypai = 0;
            Mbypai = 0;
            QAbypai = 0;

        }
        private void GridView1_Cal_Sum_For_Cancel()
        {
            double Sum_Total = 0;
            double Sum_Total2 = 0;
            double Sum_Total3 = 0;
            double Sum_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            double Sum_Total_cost = 0;

            double QAbyma = 0;
            double Qbypai = 0;
            double Mbypai = 0;
            double QAbypai = 0;


            int k = 0;

            for (int i = 0; i < this.GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.GridView1.Rows[i].Cells["Col_Auto_num"].Value == null)
                    {
                        this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();
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

                    //5 * 6 = 8

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

                    //if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value.ToString())))
                    //{
                    //    MessageBox.Show("สต๊อคคงเหลือ น้อยกว่า จำนวน ขาย ไม่สามารถขายได้ !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value).ToString("###,###.00");     //5
                    //    return;
                    //}

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    //Sum_Total_cost  =================================================
                    Sum_Total_cost = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value = Sum_Total_cost.ToString("N", new CultureInfo("en-US"));


                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > 0)
                    {
                        //Sum_Qty  =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                        this.txtsum_qty_iv.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));


                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                        this.txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }

                    //Sum_Total  =================================================
                    Sum_Total2 = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value = Sum_Total2.ToString("N", new CultureInfo("en-US"));
                    Sum_Total3 = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_want"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqtyPO"].Value = Sum_Total3.ToString("N", new CultureInfo("en-US"));

                    //คำนวณต้นทุนถัวเฉลี่ย =================================================================
                    //มูลค่าต้นทุนถัวเฉลี่ยยกมา
                    QAbyma = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = QAbyma.ToString("N", new CultureInfo("en-US"));

                    //1.เหลือยกมา + ขาย = จำนวนเหลือทั้งสิ้น
                    Qbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Qbypai.ToString("N", new CultureInfo("en-US"));
                    //2.มูลค่าเหลือยกมา + มูลค่าขาย = มูลค่ารวมทั้งสิ้น
                    Mbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Mbypai.ToString("N", new CultureInfo("en-US"));
                    //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) > 0)
                    {
                        QAbypai = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) / Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = QAbypai.ToString("N", new CultureInfo("en-US"));
                    }
                    else
                    {
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";
                    }
                    //END คำนวณต้นทุนถัวเฉลี่ย==================================================================
                    //  ===========================================================================================================
                }
            }

            this.txtcount_rows.Text = k.ToString();

            Sum_Total = 0;
            Sum_Total2 = 0;
            Sum_Total3 = 0;
            Sum_Qty = 0;
            Sum_Price = 0;
            Sum_Discount = 0;
            MoneySum = 0;
            Sum_Total_cost = 0;

            QAbyma = 0;
            Qbypai = 0;
            Mbypai = 0;
            QAbypai = 0;

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
            string MATID = "";
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {

                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
                    MATID = this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

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
                                           " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                           " AND (txtmat_id = '" + MATID.Trim() + "')" +
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
                                    //Col_txtQTY_Yokma
                                    this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");        //19
                                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");        //20

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
                    //================================


                }
            }
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

                    cmd2.CommandText = "UPDATE s001_01sale_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtDE_id = '" + this.txtDE_id.Text.Trim() + "')";
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
        private void Check_Group_tax_of_user()
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

                cmd1.CommandText = "SELECT * FROM s001_01sale_record_group_tax" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtuser_name = '" + W_ID_Select.M_USERNAME.Trim() + "')";
                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text = "Y";

                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = dt.Rows[0]["txtacc_group_tax_id"].ToString();      //1
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = dt.Rows[0]["txtacc_group_tax_name"].ToString();      //2
                    this.txtvat_rate.Text = Convert.ToSingle(dt.Rows[0]["txtacc_group_tax_vat_rate"]).ToString("###,###.00");        //3
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text = "Y";
                }
                else
                {
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text = "N";
                }

            }

            //
            conn.Close();

            //จบเชื่อมต่อฐานข้อมูล=======================================================

        }
        private void Sum_group_tax()
        {
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "SALE_EX")  //ซื้อคิดvatแยก
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
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "SALE_IN") //ซื้อคิดvatรวม
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
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "SALE_ONvat")  //ซื้อไม่มีvat
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
        //GridView1   =======================================================================


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

                cmd1.CommandText = "SELECT * FROM s001_01sale_record" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtDE_id = '" + this.txtDE_id.Text.Trim() + "')" +
                                    " AND (txtsale_status = '1')";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("เอกสารนี้   : '" + this.txtDE_id.Text.Trim() + "' ยกเลิกไปแล้ว ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                        cmd2.CommandText = "INSERT INTO s001_01sale_record_cancel(cdkey,txtco_id,txtbranch_id," +  //1
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
                        cmd2.CommandText = "UPDATE s001_01sale_record" +
                                                                    " SET txtsale_status = '1'," +
                                                                    "txtmoney_after_vat_creditor = '" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +
                                                                    "txtcreditor_status = '1'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtDE_id = '" + this.txtDE_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("ok2");

                    }

                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {
                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();
                            if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > 0)
                            {
                                //===================================================================================================================
                                //===================================================================================================================
                                //4 k017db_pr_all_detail ยอดค้างรับ
                                cmd2.CommandText = "UPDATE k018db_po_record_detail SET " +
                                                "txtcoDE_id = ''," +
                                                "txtbranchDE_id = ''," +
                                                "txtDE_id = ''," +
                                               "txtqtyDE = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqtyDE"].Value.ToString())) + "'," +
                                               "txtqtyDE_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqtyDE_balance"].Value.ToString())) + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + this.GridView1.Rows[i].Cells["Col_txtco_id"].Value.ToString() + "')" +
                                                " AND (txtbranch_id = '" + this.GridView1.Rows[i].Cells["Col_txtbranch_id"].Value.ToString() + "')" +
                                              " AND (txtPo_id = '" + this.GridView1.Rows[i].Cells["Col_txtORDER_id"].Value.ToString() + "')" +
                                               " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok66");



                                //========================================================
                                //=====================================================================================================
                                //สต๊อคสินค้า ตามคลัง =============================================================================================



                                //1.k021_mat_average
                                cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                                   "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +
                                                   "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +
                                                    "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                   " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


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


                                        "'" + this.txtDE_id.Text.Trim() + "'," +  //7 txtbill_id
                                        "'DE'," +  //9 txtbill_type
                                        "'ยกเลิกขายเชื่อ  " + this.PANEL103_CUS_txtcus_id.Text.ToString() + "  " + this.PANEL103_CUS_txtcus_name.Text.ToString() + "  " + this.txtde_remark.Text.Trim() + "'," +  //9 txtbill_remark

                                         "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //7 txtwherehouse_id
                                       "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //10 
                                        "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //11
                                        "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //12

                                        "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //13
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //14
                                        "'N'," +  //15
                                        "''," +  //16
                                       "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //17

                                       "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) + "'," +  //22 txtqty_in
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //23 txtqty2_in
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_qty_price_average"].Value.ToString())) + "'," +  //24 txtprice_out
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out


                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //18  txtqty_in
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //19 txtqty2_in
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //20 txtprice_in
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //21 txtsum_total_in



                                       "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //26
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //27
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //28
                                       "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //29

                                       "'1')";   //30

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok8");


                                //======================================

                                //สต๊อคสินค้า ตามคลัง =============================================================================================


                                //====================================================================================================
                            }
                        }
                    }
                    //MessageBox.Show("ok6");





                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtDE_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
            W_ID_Select.TRANS_ID = this.txtDE_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME05_Sales.HOME05_Sale_01sale_record_print frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_01sale_record_print();
            frm2.Show();
            frm2.BringToFront();

            //======================================================

        }
        private void btnPreview_copy_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtDE_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME05_Sales.HOME05_Sale_01sale_record_print_copy frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_01sale_record_print_copy();
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
            W_ID_Select.TRANS_ID = this.txtDE_id.Text.Trim();
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
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_s001_01sale_record.rpt");


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
                rpt.SetParameterValue("txtDE_id", W_ID_Select.TRANS_ID.Trim());

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
        private void BtnPrint_copy_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtDE_id.Text.Trim();
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

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_s001_01sale_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_s001_01sale_record_copy.rpt");


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
                rpt.SetParameterValue("txtDE_id", W_ID_Select.TRANS_ID.Trim());

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




        //=================================================

    }
}
