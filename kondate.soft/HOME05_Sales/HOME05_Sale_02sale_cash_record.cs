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
    public partial class HOME05_Sale_02sale_cash_record : Form
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

        public HOME05_Sale_02sale_cash_record()
        {
            InitializeComponent();
        }
        public string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            if (decNumber == 0)
            {
                return "ศูนย์บาทถ้วน";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("รองรับตัวเลขได้เพียง ล้านล้าน เท่านั้น!");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            if (strInteger != "0")
            {
                BahtText += "บาท";
            }

            if (strSatang == "00")
            {
                BahtText += "ถ้วน";
            }
            else
            {
                strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            return BahtText;
        }
        private void HOME05_Sale_02sale_cash_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "H0502SCRD";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "บันทึกขายสด";

            this.ActiveControl = this.PANEL1306_WH_txtwherehouse_name;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.btnPreview_copy.Enabled = false;
            this.BtnPrint.Enabled = false;
            this.BtnPrint_copy.Enabled = false;

            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);


            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Value = DateTime.Now;
            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Format = DateTimePickerFormat.Custom;
            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.CustomFormat = this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Value.ToString("dd-MM-yyyy", UsaCulture);

            this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.Value = DateTime.Now;
            this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.Format = DateTimePickerFormat.Custom;
            this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.CustomFormat = this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.Value.ToString("dd-MM-yyyy", UsaCulture);



            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL103_CUS_GridView1_cus();
            PANEL103_CUS_Fill_cus();

            PANEL109_BOM_GridView1_bom();
            PANEL109_BOM_Fill_bom();

            PANEL1_CO_GridView1_co();
            PANEL1_CO_Fill_CO();

            PANEL2_BRANCH_GridView1_branch();
            PANEL2_BRANCH_Fill_branch();


            PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax();
            PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax();
            Check_Group_tax_of_user();

            Show_GridView1();

            PANEL_MAT_Show_GridView1();
            PANEL_MAT_Fill_mat();
            this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
            this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
            this.PANEL_MAT_cboSearch.Text = "ชื่อสินค้า";


            //รับเงิน============================================================
            Fill_PANEL_02PAY_TYPE_2CHECQUE_BANK();

            Fill_PANEL_02PAY_TYPE_3CREDIT_CARD_BANK();
            this.PANEL_02PAY_TYPE_3CREDIT_CARD_cbotype_credit_card_name.Items.Add("VISA");
            this.PANEL_02PAY_TYPE_3CREDIT_CARD_cbotype_credit_card_name.Items.Add("MASTER");
            this.PANEL_02PAY_TYPE_3CREDIT_CARD_cbotype_credit_card_name.Items.Add("AMERICAN EXPRESS");
            this.PANEL_02PAY_TYPE_3CREDIT_CARD_cbotype_credit_card_name.Items.Add("PAYPAL");

            Fill_PANEL_02PAY_TYPE_4TRANSFER_BANK();
            //รับเงิน============================================================

        }
        private void HOME05_Sale_01sale_record_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                this.ActiveControl = this.txtmat_barcode_id;
                this.txtmat_barcode_id.Text = "";
            }

            if (e.KeyCode == Keys.F5)
            {
                UPDATE_TO_GridView1();
                GridView1_Cal_Sum();
                Sum_group_tax();

                PANEL_MAT_Show_GridView1();
                PANEL_MAT_Clear_GridView1();
                this.PANEL_MAT.Visible = false;
                this.BtnSave.Enabled = true;
            }


            if (e.KeyCode == Keys.F7)  //เงินสด
            {
                this.PANEL_02PAY_TYPE_txtcash.Enabled = true;
                this.PANEL_02PAY_TYPE_txtcash.ReadOnly = false;

                this.ActiveControl = this.PANEL_02PAY_TYPE_txtcash;
                this.PANEL_02PAY_TYPE_txtcash.Text = this.PANEL_02PAY_TYPE_txtmoney_sum.Text.ToString();
            }



            if (e.KeyCode == Keys.F8)   //เช็ค
            {

                if (this.PANEL_02PAY_TYPE_2CHECQUE.Visible == false)
                {
                    int xLocation = this.txtyear.Location.X;
                    int yLocation = this.txtyear.Location.Y;
                    int xx = xLocation;
                    int yy = yLocation;

                    this.PANEL_02PAY_TYPE_2CHECQUE.Visible = true;
                    this.PANEL_02PAY_TYPE_2CHECQUE.BringToFront();
                    this.PANEL_02PAY_TYPE_2CHECQUE.Location = new Point(xx, yy + 22);
                    this.PANEL_02PAY_TYPE_2CHECQUE_txtsum_receipt_money.Text = this.PANEL_02PAY_TYPE_txtmoney_sum.Text.ToString();

                    this.PANEL_02PAY_TYPE_txtchecque.Enabled = true;
                    this.PANEL_02PAY_TYPE_txtchecque.ReadOnly = false;
                    this.ActiveControl = this.PANEL_02PAY_TYPE_txtchecque;
                    this.PANEL_02PAY_TYPE_2CHECQUE_txtsum_receipt_money.ReadOnly = false;
                }
                else
                {
                    this.PANEL_02PAY_TYPE_2CHECQUE.Visible = false;
                }
            }



            if (e.KeyCode == Keys.F9)  //เครดิต
            {

                if (this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible == false)
                {
                    int xLocation = this.txtyear.Location.X;
                    int yLocation = this.txtyear.Location.Y;
                    int xx = xLocation;
                    int yy = yLocation;

                    this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible = true;
                    this.PANEL_02PAY_TYPE_3CREDIT_CARD.BringToFront();
                    this.PANEL_02PAY_TYPE_3CREDIT_CARD.Location = new Point(xx, yy + 22);
                    this.PANEL_02PAY_TYPE_3CREDIT_CARD_txtsale_cash_money.Text = this.PANEL_02PAY_TYPE_txtmoney_sum.Text.ToString();

                    this.PANEL_02PAY_TYPE_txtcredit_card.Enabled = true;
                    this.PANEL_02PAY_TYPE_txtcredit_card.ReadOnly = false;
                    this.ActiveControl = this.PANEL_02PAY_TYPE_txtcredit_card;
                    this.PANEL_02PAY_TYPE_3CREDIT_CARD_txtsale_cash_money.ReadOnly = false;

                }
                else
                {
                    this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible = false;
                }
            }


            if (e.KeyCode == Keys.F10)  //เงินโอน
            {

                if (this.PANEL_02PAY_TYPE_4TRANSFER.Visible == false)
                {
                    int xLocation = this.txtyear.Location.X;
                    int yLocation = this.txtyear.Location.Y;
                    int xx = xLocation;
                    int yy = yLocation;

                    this.PANEL_02PAY_TYPE_4TRANSFER.Visible = true;
                    this.PANEL_02PAY_TYPE_4TRANSFER.BringToFront();
                    this.PANEL_02PAY_TYPE_4TRANSFER.Location = new Point(xx, yy + 22);
                    this.PANEL_02PAY_TYPE_4TRANSFER_txtsum_receipt_money.Text = this.PANEL_02PAY_TYPE_txtmoney_sum.Text.ToString();

                    this.PANEL_02PAY_TYPE_txttransfer_money.Enabled = true;
                    this.PANEL_02PAY_TYPE_txttransfer_money.ReadOnly = false;
                    this.ActiveControl = this.PANEL_02PAY_TYPE_txttransfer_money;
                    this.PANEL_02PAY_TYPE_4TRANSFER_txtsum_receipt_money.ReadOnly = false;

                }
                else
                {
                    this.PANEL_02PAY_TYPE_4TRANSFER.Visible = false;
                }
            }

        }
        //txtmat  สินค้า  =======================================================================
        private void PANEL_MAT_Fill_mat()
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

            PANEL_MAT_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +

                                    "b001_05mat_unit1.*," +
                                    "b001_05mat_unit2.*," +

                                    "b001mat_61change_price_main.*" +

                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " INNER JOIN b001_05mat_unit2" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                    " INNER JOIN b001mat_61change_price_main" +
                                    " ON b001mat.cdkey = b001mat_61change_price_main.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_61change_price_main.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_61change_price_main.txtmat_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = this.txtOR_id.Text.ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //2

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //6

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = "0"; //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = "0"; //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = "0"; //9
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = "0"; //10
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = "0"; //11
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = "0"; //12
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = "0"; //13

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //14
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //15
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //16
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0"; // Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //17

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = "0";      //21
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = "0";      //22

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //23
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //24
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //25

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcoDE_id"].Value = "";      //26
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtbranchDE_id"].Value = "";      //27

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_want"].Value = Convert.ToSingle(0).ToString("###,###.00"); //28
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE"].Value = Convert.ToSingle(0).ToString("###,###.00"); //29
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_balance"].Value = Convert.ToSingle(0).ToString("###,###.00"); //30

                        }
                        //======================================================= Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00"); 
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL_MAT_GridView1_Up_Status();
                    this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_Show_Qty_Yokma();
            PANEL_MAT_GridView1_Cal_Sum();
        }
        private void PANEL_MAT_GridView1_Up_Status()
        {
            ////สถานะ Checkbox =======================================================
            //for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count; i++)
            //{
            //    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_status"].Value.ToString() == "0")  //Active
            //    {
            //        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_Chk"].Value = true;
            //    }
            //    else
            //    {
            //        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_Chk"].Value = false;

            //    }
            //}

        }
        private void PANEL_MAT_Show_GridView1()
        {
            this.PANEL_MAT_GridView1.ColumnCount = 32;
            this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtORDER_id";
            this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtwherehouse_id";

            this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_no";
            this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtmat_unit1_name";

            this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtQTY_Yokma";
            this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtQTY_Remind";
            this.PANEL_MAT_GridView1.Columns[9].Name = "Col_txtQTY_Plus";
            this.PANEL_MAT_GridView1.Columns[10].Name = "Col_txtQTY_first";
            this.PANEL_MAT_GridView1.Columns[11].Name = "Col_txtQTY_out";
            this.PANEL_MAT_GridView1.Columns[12].Name = "Col_txtQTY_out_cut";
            this.PANEL_MAT_GridView1.Columns[13].Name = "Col_txtQTY_Balance";

            this.PANEL_MAT_GridView1.Columns[14].Name = "Col_txtprice";
            this.PANEL_MAT_GridView1.Columns[15].Name = "Col_txtdiscount_rate";
            this.PANEL_MAT_GridView1.Columns[16].Name = "Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[17].Name = "Col_txtsum_total";

            this.PANEL_MAT_GridView1.Columns[18].Name = "Col_txtcost_qty_balance_yokma";
            this.PANEL_MAT_GridView1.Columns[19].Name = "Col_txtcost_qty_price_average_yokma";
            this.PANEL_MAT_GridView1.Columns[20].Name = "Col_txtcost_money_sum_yokma";

            this.PANEL_MAT_GridView1.Columns[21].Name = "Col_txtcost_sale_qty_price_average";
            this.PANEL_MAT_GridView1.Columns[22].Name = "Col_txtcost_sale_money_sum";

            this.PANEL_MAT_GridView1.Columns[23].Name = "Col_txtcost_qty_balance_yokpai";
            this.PANEL_MAT_GridView1.Columns[24].Name = "Col_txtcost_qty_price_average_yokpai";
            this.PANEL_MAT_GridView1.Columns[25].Name = "Col_txtcost_money_sum_yokpai";
            this.PANEL_MAT_GridView1.Columns[26].Name = "Col_txtcoDE_id";
            this.PANEL_MAT_GridView1.Columns[27].Name = "Col_txtbranchDE_id";

            this.PANEL_MAT_GridView1.Columns[28].Name = "Col_mat_status";

            this.PANEL_MAT_GridView1.Columns[29].Name = "Col_txtqtyDE_want";
            this.PANEL_MAT_GridView1.Columns[30].Name = "Col_txtqtyDE";
            this.PANEL_MAT_GridView1.Columns[31].Name = "Col_txtqtyDE_balance";


            this.PANEL_MAT_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_MAT_GridView1.Columns[1].HeaderText = "อ้างอิงใบสั่งซื้อ";
            this.PANEL_MAT_GridView1.Columns[2].HeaderText = "คลัง";

            this.PANEL_MAT_GridView1.Columns[3].HeaderText = "ลำดับ";
            this.PANEL_MAT_GridView1.Columns[4].HeaderText = " รหัส";
            this.PANEL_MAT_GridView1.Columns[5].HeaderText = " ชื่อสินค้า";
            this.PANEL_MAT_GridView1.Columns[6].HeaderText = "หน่วย";

            this.PANEL_MAT_GridView1.Columns[7].HeaderText = "สต๊อคเหลือ";
            this.PANEL_MAT_GridView1.Columns[8].HeaderText = "เหลือมา";
            this.PANEL_MAT_GridView1.Columns[9].HeaderText = "เบิกเพิ่ม";
            this.PANEL_MAT_GridView1.Columns[10].HeaderText = "เบิกแรก";
            this.PANEL_MAT_GridView1.Columns[11].HeaderText = "จำนวนขาย";
            this.PANEL_MAT_GridView1.Columns[12].HeaderText = "ขายได้";
            this.PANEL_MAT_GridView1.Columns[13].HeaderText = "ขายสุทธิ";

            this.PANEL_MAT_GridView1.Columns[14].HeaderText = "ราคา";
            this.PANEL_MAT_GridView1.Columns[15].HeaderText = "ส่วนลด%";
            this.PANEL_MAT_GridView1.Columns[16].HeaderText = "ส่วนลด(บาท)";
            this.PANEL_MAT_GridView1.Columns[17].HeaderText = "จำนวนเงิน(บาท)";

            this.PANEL_MAT_GridView1.Columns[18].HeaderText = "Col_txtcost_qty_balance_yokma";
            this.PANEL_MAT_GridView1.Columns[19].HeaderText = "Col_txtcost_qty_price_average_yokma";
            this.PANEL_MAT_GridView1.Columns[20].HeaderText = "Col_txtcost_money_sum_yokma";

            this.PANEL_MAT_GridView1.Columns[21].HeaderText = "Col_txtcost_sale_qty_price_average";
            this.PANEL_MAT_GridView1.Columns[22].HeaderText = "Col_txtcost_sale_money_sum";

            this.PANEL_MAT_GridView1.Columns[23].HeaderText = "Col_txtcost_qty_balance_yokpai";
            this.PANEL_MAT_GridView1.Columns[24].HeaderText = "Col_txtcost_qty_price_average_yokpai";
            this.PANEL_MAT_GridView1.Columns[25].HeaderText = "Col_txtcost_money_sum_yokpai";
            this.PANEL_MAT_GridView1.Columns[26].HeaderText = "Col_txtcoDE_id";
            this.PANEL_MAT_GridView1.Columns[27].HeaderText = "Col_txtbranchDE_id";

            this.PANEL_MAT_GridView1.Columns[28].HeaderText = "Col_mat_status";

            this.PANEL_MAT_GridView1.Columns[29].HeaderText = "Col_txtqtyDE_want";
            this.PANEL_MAT_GridView1.Columns[30].HeaderText = "Col_txtqtyDE";
            this.PANEL_MAT_GridView1.Columns[31].HeaderText = "Col_txtqtyDE_balance";


            this.PANEL_MAT_GridView1.Columns[10].HeaderText = "สถานะ";

            this.PANEL_MAT_GridView1.Columns["Col_Auto_num"].Visible = false;  //"No";

            this.PANEL_MAT_GridView1.Columns["Col_txtORDER_id"].Visible = true;  //"Col_txtORDER_id";
            this.PANEL_MAT_GridView1.Columns["Col_txtORDER_id"].Width = 120;
            this.PANEL_MAT_GridView1.Columns["Col_txtORDER_id"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtORDER_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtORDER_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns["Col_txtwherehouse_id"].Visible = true;  //"Col_txtwherehouse_id";
            this.PANEL_MAT_GridView1.Columns["Col_txtwherehouse_id"].Width = 100;
            this.PANEL_MAT_GridView1.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_MAT_GridView1.Columns["Col_txtmat_no"].Visible = true;  //"Col_txtmat_no";
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_no"].Width = 70;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_no"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns["Col_txtmat_id"].Visible = true;  //"Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_id"].Width = 100;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_id"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_name"].Width = 200;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_name"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL_MAT_GridView1.Columns["Col_txtmat_unit1_name"].Visible = true;  //"Col_txtmat_unit1_name";
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_unit1_name"].Width = 80;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;



            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Yokma"].Visible = true;  //"Col_txtQTY_Yokma";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Yokma"].Width = 100;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Yokma"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Remind"].Visible = false;  //"Col_txtQTY_Remind";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Remind"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Remind"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Remind"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Remind"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Plus"].Visible = false;  //"Col_txtQTY_Plus";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Plus"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Plus"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Plus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Plus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_first"].Visible = false;  //"Col_txtQTY_first";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_first"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_first"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_first"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_first"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out"].Visible = true;  //"Col_txtQTY_out";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out"].Width = 100;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out"].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out_cut"].Visible = false;  //"Col_txtQTY_out_cut";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out_cut"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out_cut"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_out_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Balance"].Visible = false;  //"Col_txtQTY_Balance";
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Balance"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Balance"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Balance"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtQTY_Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.PANEL_MAT_GridView1.Columns["Col_txtprice"].Visible = true;  //"Col_txtprice";
            this.PANEL_MAT_GridView1.Columns["Col_txtprice"].Width = 80;
            this.PANEL_MAT_GridView1.Columns["Col_txtprice"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_rate"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_money"].Visible = true;  //"Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_money"].Width = 100;
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_money"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtsum_total"].Visible = true;  //"Col_txtsum_total";
            this.PANEL_MAT_GridView1.Columns["Col_txtsum_total"].Width = 100;
            this.PANEL_MAT_GridView1.Columns["Col_txtsum_total"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokma"].Visible = false;  //"Col_txtcost_qty_balance_yokma";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokma"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokma"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokma"].Visible = false;  //"Col_txtcost_qty_price_average_yokma";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokma"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokma"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokma"].Visible = false;  //"Col_txtcost_money_sum_yokma";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokma"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokma"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_qty_price_average"].Visible = false;  //"Col_txtcost_sale_qty_price_average";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_qty_price_average"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_qty_price_average"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_qty_price_average"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_qty_price_average"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_money_sum"].Visible = false;  //"Col_txtcost_sale_money_sum";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_money_sum"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_money_sum"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_money_sum"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_sale_money_sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokpai"].Visible = false;  //"Col_txtcost_qty_balance_yokpai";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokpai"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokpai"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].Visible = false;  //"Col_txtcost_qty_price_average_yokpai";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_qty_price_average_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokpai"].Visible = false;  //"Col_txtcost_money_sum_yokpai";
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokpai"].Width = 0;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokpai"].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns["Col_txtcost_money_sum_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns["Col_txtcoDE_id"].Visible = false;  //"Col_txtcoDE_id";
            this.PANEL_MAT_GridView1.Columns["Col_txtbranchDE_id"].Visible = false;  //"Col_txtbranchDE_id";

            this.PANEL_MAT_GridView1.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";

            this.PANEL_MAT_GridView1.Columns["Col_txtqtyDE_want"].Visible = false;  //"Col_txtqtyDE_want";
            this.PANEL_MAT_GridView1.Columns["Col_txtqtyDE"].Visible = false;  //"Col_txtqtyDE";
            this.PANEL_MAT_GridView1.Columns["Col_txtqtyDE_balance"].Visible = false;  //"Col_txtqtyDE_balance";


            this.PANEL_MAT_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_MAT_GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_MAT_GridView1.EnableHeadersVisualStyles = false;


        }
        private void PANEL_MAT_Clear_GridView1()
        {
            this.PANEL_MAT_GridView1.Rows.Clear();
            this.PANEL_MAT_GridView1.Refresh();
        }
        private void PANEL_MAT_txtmat_name_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)

            //if (this.PANEL_MAT.Visible == false)
            //{
            //    this.PANEL_MAT.Visible = true;
            //    this.PANEL_MAT.Location = new Point(this.PANEL_MAT_txtmat_name.Location.X, this.PANEL_MAT_txtmat_name.Location.Y + 22);
            //    this.PANEL_MAT_GridView1.Focus();
            //}
            //else
            //{
            //    this.PANEL_MAT.Visible = false;
            //}
        }
        private void PANEL_MAT_btnmat_Click(object sender, EventArgs e)
        {

        }
        private void PANEL_MAT_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL_MAT_GridView1.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmat_id"].Value;
                if (cell != null)
                {
                    //this.txtmat_no.Text = row.Cells["Col_txtmat_no"].Value.ToString();
                    //this.PANEL_MAT_txtmat_id.Text = row.Cells["Col_txtmat_id"].Value.ToString();
                    //this.PANEL_MAT_txtmat_name.Text = row.Cells["Col_txtmat_name"].Value.ToString();
                    //this.txtmat_unit1_name.Text = row.Cells["Col_txtmat_unit1_name"].Value.ToString();
                }
            }
        }
        private void PANEL_MAT_GridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //int i = PANEL_MAT_GridView1.CurrentRow.Index;

                //this.PANEL_MAT_txtmat_id.Text = PANEL_MAT_GridView1.CurrentRow.Cells[1].Value.ToString();
                //this.PANEL_MAT_txtmat_name.Text = PANEL_MAT_GridView1.CurrentRow.Cells[2].Value.ToString();
                //this.PANEL_MAT_txtmat_name.Focus();
                //this.PANEL_MAT.Visible = false;
            }
        }
        private void PANEL_MAT_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL_MAT_btn_search_Click(object sender, EventArgs e)
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

            PANEL_MAT_Clear_GridView1();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_MAT_cboSearch.Items.Add("ชื่อสินค้า");
                //this.PANEL_MAT_cboSearch.Items.Add("รหัสสินค้า");
                if (this.PANEL_MAT_cboSearch.Text.Trim() == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +

                                        "b001_05mat_unit1.*," +
                                        "b001_05mat_unit2.*," +

                                        "b001mat_61change_price_main.*" +

                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                        " INNER JOIN b001_05mat_unit2" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                        " INNER JOIN b001mat_61change_price_main" +
                                        " ON b001mat.cdkey = b001mat_61change_price_main.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_61change_price_main.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_61change_price_main.txtmat_id" +



                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat.txtmat_name LIKE '%" + this.PANEL_MAT_txtsearch.Text.Trim() + "%')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

                }
                if (this.PANEL_MAT_cboSearch.Text.Trim() == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +

                                        "b001_05mat_unit1.*," +
                                        "b001_05mat_unit2.*," +

                                        "b001mat_61change_price_main.*" +

                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                        " INNER JOIN b001_05mat_unit2" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                        " INNER JOIN b001mat_61change_price_main" +
                                        " ON b001mat.cdkey = b001mat_61change_price_main.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_61change_price_main.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_61change_price_main.txtmat_id" +



                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtsearch.Text.Trim() + "')" +
                                        " ORDER BY b001mat.txtmat_no ASC";

                }


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
                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = this.txtOR_id.Text.ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //2

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //6

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = "0"; //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = "0"; //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = "0"; //9
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = "0"; //10
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = "0"; //11
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = "0"; //12
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = "0"; //13

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //14
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //15
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //16
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0"; // Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //17

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = "0";      //21
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = "0";      //22

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //23
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //24
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //25

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcoDE_id"].Value = "";      //26
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtbranchDE_id"].Value = "";      //27

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_want"].Value = Convert.ToSingle(0).ToString("###,###.00"); //28
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE"].Value = Convert.ToSingle(0).ToString("###,###.00"); //29
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_balance"].Value = Convert.ToSingle(0).ToString("###,###.00"); //30

                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL_MAT_GridView1_Up_Status();
                    this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();
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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_Show_Qty_Yokma();
            PANEL_MAT_GridView1_Cal_Sum();

        }
        private void PANEL_MAT_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_MAT_Fill_mat();
        }
        private void PANEL_MAT_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL_MAT_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL_MAT.Height = PANEL_MAT_btnresize_low.Top + e.Y;
                this.PANEL_MAT.Width = PANEL_MAT_btnresize_low.Left + e.X;
            }
        }
        private void PANEL_MAT_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL_MAT_btnnew_Click(object sender, EventArgs e)
        {

        }
        private Point MouseDownLocation;
        private void PANEL_MAT_iblword_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_MAT_iblword_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_MAT.Left = e.X + PANEL_MAT.Left - MouseDownLocation.X;
                PANEL_MAT.Top = e.Y + PANEL_MAT.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_MAT_GridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.PANEL_MAT_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                this.PANEL_MAT_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void PANEL_MAT_GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.PANEL_MAT_GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                this.PANEL_MAT_GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void PANEL_MAT_btnupdate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                for (int j = 0; j < this.GridView1.Rows.Count - 0; j++)
                {
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() == this.GridView1.Rows[j].Cells["Col_txtmat_id"].Value.ToString())
                    {
                        MessageBox.Show("รหัสสินค้านี้  :  " + this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "     เพิ่มเข้ามาในตารางแล้ว ระบบกำหนดให้ 1ตาราง ขายสินค้าได้ 1 รหัสสินค้าเท่านั้น", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            UPDATE_TO_GridView1();
            //GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

            PANEL_MAT_Show_GridView1();
            PANEL_MAT_Clear_GridView1();
            this.PANEL_MAT.Visible = false;
            this.BtnSave.Enabled = true;
        }
        private void btnadd_mat_Click(object sender, EventArgs e)
        {
            if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก คลังสินค้าที่จะขาย ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL1306_WH.Visible == false)
                {
                    this.PANEL1306_WH.Width = 502;
                    this.PANEL1306_WH.Height = 337;

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
            //=====================================================================================
            if (this.PANEL103_CUS_txtcus_name.Text == "")
            {
                MessageBox.Show("โปรด เลือก ลูกค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL103_CUS.Visible == false)
                {
                    this.PANEL103_CUS.Width = 502;
                    this.PANEL103_CUS.Height = 337;

                    this.PANEL103_CUS.Visible = true;
                    this.PANEL103_CUS.BringToFront();
                    this.PANEL103_CUS.Location = new Point(this.PANEL103_CUS_txtcus_name.Location.X, this.PANEL103_CUS_txtcus_name.Location.Y + 22);
                }
                else
                {
                    this.PANEL103_CUS.Visible = false;
                }
                return;

            }
            //=====================================================================================
            if (this.txtOR_id.Text == "")
            {
                //MessageBox.Show("โปรดใส่ อ้างอิงใบสั่งซื้อ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
            }

            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
                this.PANEL_MAT.BringToFront();
                this.PANEL_MAT.Location = new Point(this.btnadd_mat.Location.X, this.btnadd_mat.Location.Y + 42);

                PANEL_MAT_Fill_mat();
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }
        }
        private void UPDATE_TO_GridView1()
        {



            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > 0)
                {
                    var index = GridView1.Rows.Add();
                    GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                    GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtORDER_id"].Value.ToString(); //1
                    GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString(); //2

                    GridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString(); //3
                    GridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString(); //4
                    GridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString(); //5
                    GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString(); //6

                    GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value.ToString(); //7
                    GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Remind"].Value.ToString(); //8
                    GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Plus"].Value.ToString(); //9
                    GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_first"].Value.ToString(); //10
                    GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString(); //11
                    GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out_cut"].Value.ToString(); //12
                    GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Balance"].Value.ToString(); //13

                    GridView1.Rows[index].Cells["Col_txtprice"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString(); //14
                    GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString(); //15
                    GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString(); //16
                    GridView1.Rows[index].Cells["Col_txtsum_total"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString(); //17

                    GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString(); //18
                    GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString(); //19
                    GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString(); //20

                    GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_sale_qty_price_average"].Value.ToString(); //21
                    GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value.ToString(); //22

                    GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString(); //23
                    GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString(); //24
                    GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString(); //25

                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcoDE_id"].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcoDE_id"].Value = "";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtbranchDE_id"].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtbranchDE_id"].Value = "";
                    }
                    GridView1.Rows[index].Cells["Col_txtcoPO_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcoDE_id"].Value.ToString(); //26
                    GridView1.Rows[index].Cells["Col_txtbranchPO_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtbranchDE_id"].Value.ToString(); //27

                    GridView1.Rows[index].Cells["Col_txtqtyPO_want"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE_want"].Value.ToString(); //28
                    GridView1.Rows[index].Cells["Col_txtqtyPO"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE"].Value.ToString(); //29
                    GridView1.Rows[index].Cells["Col_txtqtyPO_balance"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE_balance"].Value.ToString(); //30
                }
            }
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();
            GridView1_Color_Column();
        }
        private void PANEL_MAT_GridView1_Color_Column()
        {

            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Style.BackColor = Color.LightSkyBlue;
            }
        }
        private void PANEL_MAT_Show_Qty_Yokma()
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
            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count; i++)
            {

                if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                {
                    MATID = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

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
                                           " AND (txtwherehouse_id = '" + this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
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
                                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance"]).ToString("###,###.00");        //18
                                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average"]).ToString("###,###.00");        //19
                                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum"]).ToString("###,###.00");        //20

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
        private void PANEL_MAT_GridView1_Cal_Sum()
        {
            double Sum_Total = 0;
            double Sum_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            double Sum_yp = 0;
            double Sum_yp2 = 0;

            int k = 0;

            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                k = 1 + i;

                var valu = this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString();

                if (valu != "")
                {
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_Auto_num"].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_Auto_num"].Value = k.ToString();
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value = "0";
                    }


                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value).ToString("###,###.00");     //5

                    if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value.ToString())))
                    {
                        MessageBox.Show("สต๊อคคงเหลือ น้อยกว่า จำนวน ขาย ไม่สามารถขายได้ !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value).ToString("###,###.00");     //5
                        return;
                    }

                    //===========================================================
                    //Sum_Total  =================================================
                    Sum_yp = Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE_balance"].Value = Sum_yp.ToString("N", new CultureInfo("en-US"));
                    Sum_yp2 = Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE_want"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", Sum_yp));
                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtqtyDE"].Value = Sum_yp2.ToString("N", new CultureInfo("en-US"));

                    //===========================================================

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                    this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtsum_total"].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > 0)
                    {
                        //Sum_Qty =================================================
                        Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                        this.PANEL_MAT_txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                        //Sum_Price  =================================================
                        Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString()));
                        this.PANEL_MAT_txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                        //Sum_Discount  =================================================
                        Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString()));
                        this.PANEL_MAT_txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                        //MoneySum  =================================================
                        MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString()));
                        this.PANEL_MAT_txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));
                    }
                }
            }

            this.PANEL_MAT_txtcount_rows.Text = k.ToString();

            Sum_Total = 0;
            Sum_Qty = 0;
            Sum_Price = 0;
            Sum_Discount = 0;
            MoneySum = 0;
            Sum_yp = 0;
            Sum_yp2 = 0;

        }
        private void PANEL_MAT_GridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PANEL_MAT_GridView1_Cal_Sum();
            }
        }
        private void PANEL_MAT_GridView1_KeyUp(object sender, KeyEventArgs e)
        {
            PANEL_MAT_GridView1_Cal_Sum();
        }
        private void PANEL_MAT_btnGo6_Click(object sender, EventArgs e)
        {
            SHOW_btnGo6();
            PANEL_MAT_GridView1_Color_Column();

        }
        private void PANEL_MAT_btnGo2_Click(object sender, EventArgs e)
        {
            SHOW_btnGo2();

        }
        private void SHOW_btnGo2()
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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT k018db_po_record_detail.*," +
                                    "b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_61change_price_main.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM k018db_po_record_detail" +

                                    " INNER JOIN b001mat" +
                                    " ON k018db_po_record_detail.cdkey = b001mat.cdkey" +
                                    " AND k018db_po_record_detail.txtco_id = b001mat.txtco_id" +
                                    " AND k018db_po_record_detail.txtmat_id = b001mat.txtmat_id" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON k018db_po_record_detail.cdkey = b001mat_02detail.cdkey" +
                                    " AND k018db_po_record_detail.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND k018db_po_record_detail.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001mat_61change_price_main" +
                                    " ON k018db_po_record_detail.cdkey = b001mat_61change_price_main.cdkey" +
                                    " AND k018db_po_record_detail.txtco_id = b001mat_61change_price_main.txtco_id" +
                                    " AND k018db_po_record_detail.txtmat_id = b001mat_61change_price_main.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " WHERE (k018db_po_record_detail.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k018db_po_record_detail.txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                   " AND (k018db_po_record_detail.txtbranch_id = '" + this.PANEL2_BRANCH_txtbranch_id.Text.Trim() + "')" +
                                    " AND (k018db_po_record_detail.txtqtyDE_balance > 0)" +
                                    " ORDER BY k018db_po_record_detail.ID ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = dt2.Rows[j]["txtPo_id"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //2

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //6

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = "0"; //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = "0"; //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyDE_balance"]).ToString("###,###.00"); //9
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyDE_balance"]).ToString("###,###.00"); //10
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyDE_balance"]).ToString("###,###.00"); //11
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = "0"; //12
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = "0"; //13

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //14
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //15
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //16
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0"; // Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //17

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = "0";      //21
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = "0";      //22

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //23
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //24
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //25

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcoDE_id"].Value = this.PANEL1_CO_txtco_id.Text.Trim();      //26
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtbranchDE_id"].Value = this.PANEL2_BRANCH_txtbranch_id.Text.Trim();      //27

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_want"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyDE_want"]).ToString("###,###.00"); //28
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyDE"]).ToString("###,###.00"); //29
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_balance"].Value = Convert.ToSingle(dt2.Rows[j]["txtqtyDE_balance"]).ToString("###,###.00"); //30


                            Cursor.Current = Cursors.Default;
                        }
                        //=======================================================

                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_Show_Qty_Yokma();
            PANEL_MAT_GridView1_Cal_Sum();

        }

        //END txtmat สินค้า =======================================================================

        //txtbom ชื่อ BOM =======================================================================
        private void PANEL109_BOM_Fill_bom()
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

            PANEL109_BOM_Clear_GridView1_bom();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_09bom" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbom_id <> '')" +
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
                            var index = PANEL109_BOM_dataGridView1_bom.Rows.Add();
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_id"].Value = dt2.Rows[j]["txtbom_id"].ToString();      //1
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name"].Value = dt2.Rows[j]["txtbom_name"].ToString();      //2
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name_eng"].Value = dt2.Rows[j]["txtbom_name_eng"].ToString();      //3
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
        private void PANEL109_BOM_GridView1_bom()
        {
            this.PANEL109_BOM_dataGridView1_bom.ColumnCount = 4;
            this.PANEL109_BOM_dataGridView1_bom.Columns[0].Name = "Col_Auto_num";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].Name = "Col_txtbom_id";
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].Name = "Col_txtbom_name";
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].Name = "Col_txtbom_name_eng";

            this.PANEL109_BOM_dataGridView1_bom.Columns[0].HeaderText = "No";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].HeaderText = "รหัส";
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].HeaderText = " ชื่อ BOM";
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].HeaderText = " ชื่อ BOM Eng";

            this.PANEL109_BOM_dataGridView1_bom.Columns[0].Visible = false;  //"No";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].Visible = true;  //"Col_txtbom_id";
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].Width = 100;
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].ReadOnly = true;
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL109_BOM_dataGridView1_bom.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL109_BOM_dataGridView1_bom.Columns[2].Visible = true;  //"Col_txtbom_name";
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].Width = 150;
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].ReadOnly = true;
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL109_BOM_dataGridView1_bom.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL109_BOM_dataGridView1_bom.Columns[3].Visible = true;  //"Col_txtbom_name_eng";
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].Width = 150;
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].ReadOnly = true;
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL109_BOM_dataGridView1_bom.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL109_BOM_dataGridView1_bom.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL109_BOM_dataGridView1_bom.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL109_BOM_dataGridView1_bom.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL109_BOM_dataGridView1_bom.EnableHeadersVisualStyles = false;

        }
        private void PANEL109_BOM_Clear_GridView1_bom()
        {
            this.PANEL109_BOM_dataGridView1_bom.Rows.Clear();
            this.PANEL109_BOM_dataGridView1_bom.Refresh();
        }
        private void PANEL109_BOM_txtbom_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL109_BOM.Visible == false)
                {
                    this.PANEL109_BOM.Visible = true;
                    this.PANEL109_BOM.Location = new Point(this.PANEL109_BOM_txtbom_name.Location.X, this.PANEL109_BOM_txtbom_name.Location.Y + 22);
                    this.PANEL109_BOM_dataGridView1_bom.Focus();
                }
                else
                {
                    this.PANEL109_BOM.Visible = false;
                }
        }
        private void PANEL109_BOM_btnbom_Click(object sender, EventArgs e)
        {

            if (this.PANEL109_BOM.Visible == false)
            {
                int xLocation = PANEL109_BOM_txtbom_name.Location.X;
                int yLocation = PANEL109_BOM_txtbom_name.Location.Y;
                int xx = xLocation;
                int yy = yLocation;

                this.PANEL109_BOM.Visible = true;
                this.PANEL109_BOM.BringToFront();
                this.PANEL109_BOM.Location = new Point(xx, yy + 22);
            }
            else
            {
                this.PANEL109_BOM.Visible = false;
            }
        }
        private void PANEL109_BOM_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL109_BOM.Visible == false)
            {
                this.PANEL109_BOM.Visible = true;
            }
            else
            {
                this.PANEL109_BOM.Visible = false;
            }
        }
        private void PANEL109_BOM_dataGridView1_bom_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL109_BOM_dataGridView1_bom.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL109_BOM_txtbom_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL109_BOM_txtbom_name.Text = row.Cells[2].Value.ToString();
                    SHOW_btnGo6();
                }
            }
        }
        private void SHOW_btnGo6()
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

            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001_09bom_detail.*," +
                                    "b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001mat_61change_price_main.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001_09bom_detail" +

                                    " INNER JOIN b001mat" +
                                    " ON b001_09bom_detail.cdkey = b001mat.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat.txtmat_id" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001_09bom_detail.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001mat_61change_price_main" +
                                    " ON b001_09bom_detail.cdkey = b001mat_61change_price_main.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat_61change_price_main.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat_61change_price_main.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " WHERE (b001_09bom_detail.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001_09bom_detail.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001_09bom_detail.txtbom_id = '" + this.PANEL109_BOM_txtbom_id.Text.Trim() + "')" +
                                    " ORDER BY b001_09bom_detail.ID ASC";

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = this.txtOR_id.Text.ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //2

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //6

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = "0"; //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = "0"; //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = "0"; //9
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = "0"; //10
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = "0"; //11
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = "0"; //12
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = "0"; //13

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //14
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //15
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //16
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0"; // Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //17

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = "0";      //21
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = "0";      //22

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //23
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //24
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //25

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtcoDE_id"].Value = "";      //26
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtbranchDE_id"].Value = "";      //27

                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_want"].Value = Convert.ToSingle(0).ToString("###,###.00"); //28
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE"].Value = Convert.ToSingle(0).ToString("###,###.00"); //29
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqtyDE_balance"].Value = Convert.ToSingle(0).ToString("###,###.00"); //30

                            Cursor.Current = Cursors.Default;
                        }
                        //=======================================================

                    }
                    else
                    {
                        this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

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
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_Show_Qty_Yokma();
            PANEL_MAT_GridView1_Cal_Sum();


        }
        private void PANEL109_BOM_dataGridView1_bom_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL109_BOM_dataGridView1_bom.CurrentRow.Index;

                this.PANEL109_BOM_txtbom_id.Text = PANEL109_BOM_dataGridView1_bom.CurrentRow.Cells[1].Value.ToString();
                this.PANEL109_BOM_txtbom_name.Text = PANEL109_BOM_dataGridView1_bom.CurrentRow.Cells[2].Value.ToString();
                this.PANEL109_BOM_txtbom_name.Focus();
                this.PANEL109_BOM.Visible = false;
            }
        }
        private void PANEL109_BOM_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL109_BOM_btn_search_Click(object sender, EventArgs e)
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

            PANEL109_BOM_Clear_GridView1_bom();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_09bom" +
                                   " WHERE (txtbom_name LIKE '%" + this.PANEL109_BOM_txtsearch.Text + "%')" +
                                  " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
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
                            var index = PANEL109_BOM_dataGridView1_bom.Rows.Add();
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_id"].Value = dt2.Rows[j]["txtbom_id"].ToString();      //1
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name"].Value = dt2.Rows[j]["txtbom_name"].ToString();      //2
                            PANEL109_BOM_dataGridView1_bom.Rows[index].Cells["Col_txtbom_name_eng"].Value = dt2.Rows[j]["txtbom_name_eng"].ToString();      //3
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
        private void PANEL109_BOM_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL109_BOM_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL109_BOM.Height = PANEL109_BOM_btnresize_low.Top + e.Y;
                this.PANEL109_BOM.Width = PANEL109_BOM_btnresize_low.Left + e.X;
            }
        }
        private void PANEL109_BOM_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL109_BOM_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtbom ชื่อ BOM =======================================================================


        //txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================
        private void PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax()
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

            PANEL1313_ACC_GROUP_TAX_Clear_GridView1_acc_group_tax();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k013_1db_acc_13group_tax" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtacc_group_tax_id <> '')" +
                                  " AND (txtacc_group_tax_status = 'S')" +  //เฉพาะกลุ่มขาย
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
                            var index = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows.Add();
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_id"].Value = dt2.Rows[j]["txtacc_group_tax_id"].ToString();      //1
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name"].Value = dt2.Rows[j]["txtacc_group_tax_name"].ToString();      //2
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name_eng"].Value = dt2.Rows[j]["txtacc_group_tax_name_eng"].ToString();      //3
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_vat_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtacc_group_tax_vat_rate"]).ToString("###,###.00");      //4
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
        private void PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax()
        {
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnCount = 5;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[0].Name = "Col_Auto_num";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].Name = "Col_txtacc_group_tax_id";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].Name = "Col_txtacc_group_tax_name";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].Name = "Col_txtacc_group_tax_name_eng";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].Name = "Col_txtacc_group_tax_vat_rate";

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[0].HeaderText = "No";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].HeaderText = "รหัส";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].HeaderText = " กลุ่มภาษี ";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].HeaderText = " กลุ่มภาษี  Eng";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].HeaderText = "อัตราภาษี";

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[0].Visible = false;  //"No";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].Visible = true;  //"Col_txtacc_group_tax_id";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].Width = 100;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].ReadOnly = true;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].Visible = true;  //"Col_txtacc_group_tax_name";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].Width = 100;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].ReadOnly = true;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].Visible = false;  //"Col_txtacc_group_tax_name_eng";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].Width = 0;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].ReadOnly = false;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].Visible = true;  //"Col_txtacc_group_tax_vat_rate";
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].Width = 100;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].ReadOnly = true;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;


            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.EnableHeadersVisualStyles = false;

        }
        private void PANEL1313_ACC_GROUP_TAX_Clear_GridView1_acc_group_tax()
        {
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows.Clear();
            this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Refresh();
        }
        private void PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL1313_ACC_GROUP_TAX.Visible == false)
                {
                    this.PANEL1313_ACC_GROUP_TAX.Visible = true;
                    this.PANEL1313_ACC_GROUP_TAX.Location = new Point(this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.X, this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.Y + 22);
                    this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Focus();
                }
                else
                {
                    this.PANEL1313_ACC_GROUP_TAX.Visible = false;
                }
        }
        private void PANEL1313_ACC_GROUP_TAX_btnacc_group_tax_Click(object sender, EventArgs e)
        {

            if (this.PANEL1313_ACC_GROUP_TAX.Visible == false)
            {
                int xLocation = PANEL1313_ACC_GROUP_TAX_label35.Location.X;
                int yLocation = PANEL1313_ACC_GROUP_TAX_label35.Location.Y;
                int xx = xLocation;
                int yy = yLocation;

                this.PANEL1313_ACC_GROUP_TAX.Visible = true;
                this.PANEL1313_ACC_GROUP_TAX.BringToFront();
                this.PANEL1313_ACC_GROUP_TAX.Location = new Point(xx, yy + 22);
            }
            else
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = false;
            }

        }
        private void PANEL1313_ACC_GROUP_TAX_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1313_ACC_GROUP_TAX.Visible == false)
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = true;
            }
            else
            {
                this.PANEL1313_ACC_GROUP_TAX.Visible = false;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = row.Cells[2].Value.ToString();
                    this.txtvat_rate.Text = row.Cells[4].Value.ToString();
                    Sum_group_tax();
                }
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.CurrentRow.Index;

                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.CurrentRow.Cells[1].Value.ToString();
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.CurrentRow.Cells[2].Value.ToString();
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Focus();
                this.PANEL1313_ACC_GROUP_TAX.Visible = false;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1313_ACC_GROUP_TAX_btn_search_Click(object sender, EventArgs e)
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

            PANEL1313_ACC_GROUP_TAX_Clear_GridView1_acc_group_tax();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM k013_1db_acc_13group_tax" +
                                    " WHERE (txtacc_group_tax_name LIKE '%" + this.PANEL1313_ACC_GROUP_TAX_txtsearch.Text + "%')" +
                                    " AND (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtacc_group_tax_status = 'P')" +  //เฉพาะกลุ่มซื้อ
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
                            var index = PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows.Add();
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_id"].Value = dt2.Rows[j]["txtacc_group_tax_id"].ToString();      //1
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name"].Value = dt2.Rows[j]["txtacc_group_tax_name"].ToString();      //2
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_name_eng"].Value = dt2.Rows[j]["txtacc_group_tax_name_eng"].ToString();      //3
                            PANEL1313_ACC_GROUP_TAX_dataGridView1_acc_group_tax.Rows[index].Cells["Col_txtacc_group_tax_vat_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtacc_group_tax_vat_rate"]).ToString("###,###.00");      //4
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
        private void PANEL1313_ACC_GROUP_TAX_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1313_ACC_GROUP_TAX_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1313_ACC_GROUP_TAX.Height = PANEL1313_ACC_GROUP_TAX_btnresize_low.Top + e.Y;
                this.PANEL1313_ACC_GROUP_TAX.Width = PANEL1313_ACC_GROUP_TAX_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1313_ACC_GROUP_TAX_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1313_ACC_GROUP_TAX_btnnew_Click(object sender, EventArgs e)
        {

        }
        //END txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================

        //Company=======================================================================
        private void PANEL1_CO_Fill_CO()
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

            PANEL1_CO_Clear_GridView1_co();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT *" +
                                  " FROM k009db_business" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_status = '0')" +
                                  " AND (txtco_id <> '')" +
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
                            var index = PANEL1_CO_dataGridView1_co.Rows.Add();
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_name"].Value = dt2.Rows[j]["txtco_name"].ToString();      //2
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_status"].Value = dt2.Rows[j]["txtco_status"].ToString();      //4
                        }
                        PANEL1_CO_GridView1_co_Up_Status();

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
        private void PANEL1_CO_GridView1_co()
        {
            this.PANEL1_CO_dataGridView1_co.ColumnCount = 5;
            this.PANEL1_CO_dataGridView1_co.Columns[0].Name = "Col_Auto_num";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Name = "Col_txtco_id";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Name = "Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[3].Name = "Col_txthome_id_full";
            this.PANEL1_CO_dataGridView1_co.Columns[4].Name = "Col_txtco_status";

            this.PANEL1_CO_dataGridView1_co.Columns[0].HeaderText = "No";
            this.PANEL1_CO_dataGridView1_co.Columns[1].HeaderText = "รหัสกิจการ";
            this.PANEL1_CO_dataGridView1_co.Columns[2].HeaderText = "ชื่อกิจการ";
            this.PANEL1_CO_dataGridView1_co.Columns[3].HeaderText = "ที่อยู่";  //
            this.PANEL1_CO_dataGridView1_co.Columns[4].HeaderText = "สถานะ";

            this.PANEL1_CO_dataGridView1_co.Columns[0].Visible = false;  //"No";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Visible = true;  //"Col_txtco_id";
            this.PANEL1_CO_dataGridView1_co.Columns[1].Width = 80;
            this.PANEL1_CO_dataGridView1_co.Columns[1].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[2].Visible = true;  //"Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Width = 250;
            this.PANEL1_CO_dataGridView1_co.Columns[2].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[3].Visible = false; // "Col_txthome_id_full
            this.PANEL1_CO_dataGridView1_co.Columns[3].Width = 0;
            this.PANEL1_CO_dataGridView1_co.Columns[3].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[4].Visible = false;  // "Col_txtco_status
            this.PANEL1_CO_dataGridView1_co.Columns[4].Width = 0;
            this.PANEL1_CO_dataGridView1_co.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL1_CO_dataGridView1_co.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL1_CO_dataGridView1_co.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL1_CO_dataGridView1_co.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL1_CO_dataGridView1_co.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.FillWeight = 10;
            dgvCmb.ReadOnly = true;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL1_CO_dataGridView1_co.Columns.Add(dgvCmb);

        }
        private void PANEL1_CO_Clear_GridView1_co()
        {
            this.PANEL1_CO_dataGridView1_co.Rows.Clear();
            this.PANEL1_CO_dataGridView1_co.Refresh();
        }
        private void PANEL1_CO_GridView1_co_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL1_CO_dataGridView1_co.Rows.Count; i++)
            {
                if (this.PANEL1_CO_dataGridView1_co.Rows[i].Cells["Col_txtco_status"].Value.ToString() == "0")  //Active
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells["Col_Chk"].Value = true;
                }
                else
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells["Col_Chk"].Value = false;

                }
            }
        }
        private void PANEL1_CO_btnco_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
                this.PANEL1_CO.Location = new Point(PANEL1_CO_txtco_name.Location.X, this.PANEL1_CO_txtco_name.Location.Y + 22);
            }
            else
            {
                this.PANEL1_CO.Visible = false;
            }
        }
        private void PANEL1_CO_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
            }
            else
            {
                this.PANEL1_CO.Visible = false;
            }
        }

        private void PANEL1_CO_dataGridView1_co_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL1_CO_dataGridView1_co.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtco_id"].Value;
                if (cell != null)
                {
                    this.PANEL1_CO_txtco_id.Text = row.Cells["Col_txtco_id"].Value.ToString();
                    this.PANEL1_CO_txtco_name.Text = row.Cells["Col_txtco_name"].Value.ToString();
                    W_ID_Select.M_COID = row.Cells["Col_txtco_id"].Value.ToString();
                    W_ID_Select.M_CONAME = row.Cells["Col_txtco_name"].Value.ToString();
                    this.PANEL2_BRANCH_txtbranch_id.Text = "";
                    this.PANEL2_BRANCH_txtbranch_name.Text = "";


                }
            }
        }
        private void PANEL1_CO_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL1_CO_btn_search_Click(object sender, EventArgs e)
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

            PANEL1_CO_Clear_GridView1_co();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM k009db_business" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_name LIKE '%" + this.PANEL1_CO_txtsearch.Text + "%')" +
                                  " AND (txtco_status = '0')" +
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
                            var index = PANEL1_CO_dataGridView1_co.Rows.Add();
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_id"].Value = dt2.Rows[j]["txtco_id"].ToString();      //1
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_name"].Value = dt2.Rows[j]["txtco_name"].ToString();      //2
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txthome_id_full"].Value = dt2.Rows[j]["txthome_id_full"].ToString();      //3
                            PANEL1_CO_dataGridView1_co.Rows[index].Cells["Col_txtco_status"].Value = dt2.Rows[j]["txtco_status"].ToString();      //4
                        }
                        PANEL1_CO_GridView1_co_Up_Status();
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

        private void PANEL1_CO_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL1_CO_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL1_CO.Height = PANEL1_CO_btnresize_low.Top + e.Y;
                this.PANEL1_CO.Width = PANEL1_CO_btnresize_low.Left + e.X;
            }
        }
        private void PANEL1_CO_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL1_CO_btnnew_Click(object sender, EventArgs e)
        {
            W_ID_Select.FROM_FORM = "HOME";
            this.Hide();
            kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_04_Co frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_04_Co();
            frm2.Show();
            frm2.BringToFront();
        }
        //Company=======================================================================
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
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_name_short"].Value = dt2.Rows[j]["txtbranch_name_short"].ToString();      //3
                            PANEL2_BRANCH_dataGridView1_branch.Rows[index].Cells["Col_txtbranch_status"].Value = dt2.Rows[j]["txtbranch_status"].ToString();      //4
                        }
                        PANEL2_BRANCH_GridView1_branch_Up_Status();
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
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Name = "Col_txtbranch_name_short";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Name = "Col_txtbranch_status";

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].HeaderText = "No";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].HeaderText = "รหัสสาขา";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].HeaderText = "ชื่อสาขา";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].HeaderText = "ชื่อย่อสาขา";  //
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].HeaderText = "สถานะ";

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[0].Visible = false;  //"No";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Visible = true;  //"Col_txtbranch_id";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].Width = 80;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Visible = true;  //"Col_txtbranch_name";
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].Width = 130;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Visible = true; // "Col_txtbranch_name_short
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].Width = 100;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Visible = false;  // "Col_txtbranch_status
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].Width = 0;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].ReadOnly = true;
            this.PANEL2_BRANCH_dataGridView1_branch.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL2_BRANCH_dataGridView1_branch.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL2_BRANCH_dataGridView1_branch.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL2_BRANCH_dataGridView1_branch.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL2_BRANCH_dataGridView1_branch.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL2_BRANCH_dataGridView1_branch.Columns.Add(dgvCmb);

        }
        private void PANEL2_BRANCH_Clear_GridView1_branch()
        {
            this.PANEL2_BRANCH_dataGridView1_branch.Rows.Clear();
            this.PANEL2_BRANCH_dataGridView1_branch.Refresh();
        }
        private void PANEL2_BRANCH_GridView1_branch_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL2_BRANCH_dataGridView1_branch.Rows.Count; i++)
            {
                if (this.PANEL2_BRANCH_dataGridView1_branch.Rows[i].Cells["Col_txtbranch_status"].Value.ToString() == "0")  //Active
                {
                    this.PANEL2_BRANCH_dataGridView1_branch.Rows[i].Cells["Col_Chk"].Value = true;
                }
                else
                {
                    this.PANEL2_BRANCH_dataGridView1_branch.Rows[i].Cells["Col_Chk"].Value = false;

                }
            }
        }
        private void PANEL2_BRANCH_btnbranch_Click(object sender, EventArgs e)
        {
            if (this.PANEL2_BRANCH.Visible == false)
            {
                this.PANEL2_BRANCH.Visible = true;
                this.PANEL2_BRANCH.Location = new Point(PANEL2_BRANCH_txtbranch_name.Location.X, this.PANEL2_BRANCH_txtbranch_name.Location.Y + 22);

                PANEL2_BRANCH_GridView1_branch();
                PANEL2_BRANCH_Fill_branch();

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

                var cell = row.Cells["Col_txtbranch_id"].Value;
                if (cell != null)
                {
                    this.PANEL2_BRANCH_txtbranch_id.Text = row.Cells["Col_txtbranch_id"].Value.ToString();
                    this.PANEL2_BRANCH_txtbranch_name.Text = row.Cells["Col_txtbranch_name"].Value.ToString();
                    W_ID_Select.M_BRANCHID = row.Cells["Col_txtbranch_id"].Value.ToString();
                    W_ID_Select.M_BRANCHNAME = row.Cells["Col_txtbranch_name"].Value.ToString();
                    W_ID_Select.M_BRANCHNAME_SHORT = row.Cells["Col_txtbranch_name_short"].Value.ToString();
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
                        PANEL2_BRANCH_GridView1_branch_Up_Status();
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
            if (this.PANEL1_CO_txtco_id.Text == "")
            {
                MessageBox.Show("โปรดเลือก รหัสบริษัทฯ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1_CO.Visible = true;
                this.PANEL1_CO.Location = new Point(116, 62);
                this.PANEL2_BRANCH.Visible = false;
                return;
            }
            else
            {
                W_ID_Select.M_COID = this.PANEL1_CO_txtco_id.Text.Trim();
                W_ID_Select.M_CONAME = this.PANEL1_CO_txtco_name.Text.Trim();

                W_ID_Select.FROM_FORM = "HOME";
                this.Hide();
                kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_05_Branch frm2 = new kondate.soft.SETUP_2ACC.Home_SETUP_Enter_2ACC_05_Branch();
                frm2.Show();
                frm2.BringToFront();
            }
        }

        //Branch=======================================================================


        //Tans_Log ====================================================================


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
            else
            {
                W_ID_Select.LOG_ID = "3";
                W_ID_Select.LOG_NAME = "ใหม่";
                TRANS_LOG();

                W_ID_Select.WORD_TOP = "ขายสด";
                kondate.soft.HOME05_Sales.HOME05_Sale_02sale_cash_record frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_02sale_cash_record();
                frm2.Show();
                //this.Close();
            }
        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_sc.Text)) == 0)
            {
                MessageBox.Show("ไม่พบ จำนวนส่งสินค้า !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกกลุ่มภาษี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Focus();
                return;
            }

            if (this.PANEL_02PAY_TYPE.Visible == false)
            {
                this.PANEL_02PAY_TYPE.Visible = true;
                this.PANEL_02PAY_TYPE.BringToFront();
                this.PANEL_02PAY_TYPE.Location = new Point(this.PANEL1306_WH_txtwherehouse_name.Location.X, this.PANEL1306_WH_txtwherehouse_name.Location.Y);

                this.PANEL_02PAY_TYPE_txtmoney_sum.Text = this.txtmoney_after_vat.Text.ToString();
                SUM_RECEIVE_MONEY();
                this.BtnSave.Enabled = false;

            }
            else
            {
                this.PANEL_02PAY_TYPE.Visible = false;
            }


        }

        private void INSERT_SALE_CASH()
        {

            STOCK_FIND_INSERT();
            AUTO_BILL_TRANS_ID();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();
            this.txtmoney_after_vat_txt.Text = ThaiBahtText(this.txtmoney_after_vat.Text);

            //จบเชื่อมต่อฐานข้อมูล=======================================================

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
                        cmd2.CommandText = "INSERT INTO s001_01sale_cash_record_trans(cdkey," +
                                           "txtco_id,txtbranch_id," +
                                           "txttrans_id)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                           "'" + this.txtSC_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();


                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE s001_01sale_cash_record_trans SET txttrans_id = '" + this.txtSC_id.Text.Trim() + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //MessageBox.Show("ok1");

                    //2 s001_01sale_cash_record
                    cmd2.CommandText = "INSERT INTO s001_01sale_cash_record(cdkey,txtco_id,txtbranch_id," +  //1
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                            "txtuser_name,txtemp_office_name," +  //5
                                           "txtversion_id," +  //6
                                                               //====================================================

                                          "txtSC_id," + // 7
                                          "txtOR_id," + // 8
                                          "txtsale_type_id," + // 9
                                          "txtcus_id," + // 10
                                          "txtline_id," + // 11
                                          "txtemp_send_id," + // 12
                                          "txtemp_sale_id," + // 13
                                          "txtemp_mg_ket_id," + // 14
                                          "txtemp_mg_pak_id," + // 15

                                          "txtconsignee," + // 16
                                          "txtemp_office_name_manager," + // 16
                                           "txtemp_office_name_approve," + // 17
                                           "txtsc_remark," + // 18

                                           "txtcurrency_id," + // 19
                                           "txtcurrency_date," + // 20
                                           "txtcurrency_rate," + // 21

                                           "txtacc_group_tax_id," + // 22

                                           "txtsum_qty_sc," + // 23

                                           "txtsum_price," + // 24
                                           "txtsum_discount," + // 25
                                           "txtmoney_sum," + // 26
                                           "txtmoney_tax_base," + // 27
                                           "txtvat_rate," + // 28
                                           "txtvat_money," + // 29
                                           "txtmoney_after_vat," + // 30
                                           "txtmoney_after_vat_txt," + // 30
                                           "txtmoney_after_vat_creditor," + // 31
                                           "txtcreditor_status," + // 32

                                           "txtsc_status," +  //33

                                          "txtapprove_status," +  //34
                                          "txtpayment_status," +  //35
                                          "txtacc_record_status," +  //36
                                          "txtemp_print,txtemp_print_datetime,txtIV_id) " +  //37

                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtuser_name,@txtemp_office_name," +  //5
                                           "@txtversion_id," +  //6
                                                                //=========================================================


                                           "@txtSC_id," + // 7
                                          "@txtOR_id," + // 8
                                          "@txtsale_type_id," + // 9
                                          "@txtcus_id," + // 10
                                          "@txtline_id," + // 11
                                          "@txtemp_send_id," + // 12
                                          "@txtemp_sale_id," + // 13
                                          "@txtemp_mg_ket_id," + // 14
                                          "@txtemp_mg_pak_id," + // 15

                                          "@txtconsignee," + // 16
                                          "@txtemp_office_name_manager," + // 16
                                           "@txtemp_office_name_approve," + // 17
                                           "@txtsc_remark," + // 18

                                           "@txtcurrency_id," + // 19
                                           "@txtcurrency_date," + // 20
                                           "@txtcurrency_rate," + // 21

                                           "@txtacc_group_tax_id," + // 22

                                           "@txtsum_qty_sc," + // 23

                                           "@txtsum_price," + // 24
                                           "@txtsum_discount," + // 25
                                           "@txtmoney_sum," + // 26
                                           "@txtmoney_tax_base," + // 27
                                           "@txtvat_rate," + // 28
                                           "@txtvat_money," + // 29
                                           "@txtmoney_after_vat," + // 30
                                           "@txtmoney_after_vat_txt," + // 30
                                           "@txtmoney_after_vat_creditor," + // 31
                                           "@txtcreditor_status," + // 32

                                           "@txtsc_status," +  //33

                                          "@txtapprove_status," +  //34
                                          "@txtpayment_status," +  //35
                                          "@txtacc_record_status," +  //36
                                          "@txtemp_print,@txtemp_print_datetime,@txtIV_id) ";  //46

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


                    cmd2.Parameters.Add("@txtSC_id", SqlDbType.NVarChar).Value = this.txtSC_id.Text.Trim();  //7
                    cmd2.Parameters.Add("@txtOR_id", SqlDbType.NVarChar).Value = this.txtOR_id.Text.Trim();  //8
                    cmd2.Parameters.Add("@txtsale_type_id", SqlDbType.NVarChar).Value = "CASH";  //9
                    cmd2.Parameters.Add("@txtcus_id", SqlDbType.NVarChar).Value = this.PANEL103_CUS_txtcus_id.Text.Trim();  //10

                    cmd2.Parameters.Add("@txtline_id", SqlDbType.NVarChar).Value = this.txtline_id.Text.Trim();  //11
                    cmd2.Parameters.Add("@txtemp_send_id", SqlDbType.NVarChar).Value = this.txtemp_send_id.Text.Trim();  //12
                    cmd2.Parameters.Add("@txtemp_sale_id", SqlDbType.NVarChar).Value = this.txtemp_sale_id.Text.Trim();  //13
                    cmd2.Parameters.Add("@txtemp_mg_ket_id", SqlDbType.NVarChar).Value = this.txtemp_mg_ket_id.Text.Trim();  //14
                    cmd2.Parameters.Add("@txtemp_mg_pak_id", SqlDbType.NVarChar).Value = this.txtemp_mg_pak_id.Text.Trim();  //15

                    cmd2.Parameters.Add("@txtconsignee", SqlDbType.NVarChar).Value = this.txtconsignee.Text.Trim();  //9
                    cmd2.Parameters.Add("@txtemp_office_name_manager", SqlDbType.NVarChar).Value = this.txtemp_office_name_manager.Text.Trim();  //9
                    cmd2.Parameters.Add("@txtemp_office_name_approve", SqlDbType.NVarChar).Value = this.txtemp_office_name_approve.Text.Trim();  //10
                    cmd2.Parameters.Add("@txtsc_remark", SqlDbType.NVarChar).Value = this.txtsc_remark.Text.Trim();  //11

                    cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //12
                    cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //13
                    cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n4}", txtcurrency_rate.Text.ToString()));  //14

                    cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //15

                    cmd2.Parameters.Add("@txtsum_qty_sc", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_qty_sc.Text.ToString()));  //16

                    cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_price.Text.ToString()));  //17
                    cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtsum_discount.Text.ToString()));  //18
                    cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_sum.Text.ToString()));  //19
                    cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_tax_base.Text.ToString()));  //20
                    cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text.ToString()));  //21
                    cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtvat_money.Text.ToString()));  //22
                    cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //23
                    cmd2.Parameters.Add("@txtmoney_after_vat_txt", SqlDbType.NVarChar).Value = this.txtmoney_after_vat_txt.Text.Trim();  //23
                    cmd2.Parameters.Add("@txtmoney_after_vat_creditor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmoney_after_vat.Text.ToString()));  //24
                    cmd2.Parameters.Add("@txtcreditor_status", SqlDbType.NVarChar).Value = "0";  //25

                    cmd2.Parameters.Add("@txtsc_status", SqlDbType.NVarChar).Value = "";  //29
                    cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = "";  //31
                    cmd2.Parameters.Add("@txtpayment_status", SqlDbType.NVarChar).Value = "";  //32
                    cmd2.Parameters.Add("@txtacc_record_status", SqlDbType.NVarChar).Value = "";  //33
                    cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //34
                    cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);//35
                    cmd2.Parameters.Add("@txtIV_id", SqlDbType.NVarChar).Value = "";  //36

                    //=====================================================================================================================================================
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");


                    //r002_01receipt_record_trans
                    //r002_01receipt_record

                    //r002_01receipt_record_record_1sale_cash_trans
                    //r002_01receipt_record_record_1sale_cash

                    //r002_01receipt_record_record_1sale_cash_2pay_type_detail

                    //r002_01receipt_record_record_1sale_cash_3rebate_type_detail

                    //r002_01receipt_record_record_1sale_cash_4fee_type_detail



                    //3 s001_01sale_cash_record_detail
                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {

                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();

                            //===================================================================================================================
                            //3 s001_01sale_cash_record_detail

                            cmd2.CommandText = "INSERT INTO s001_01sale_cash_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                           "txttrans_year,txttrans_month,txttrans_day," +

                          // //=================================================================
                          "txtSC_id," +  //6
                          "txtORDER_id," +  //7
                           "txtwherehouse_id," +  //8

                            "txtmat_no," +  //12
                            "txtmat_id," +  //13
                            "txtmat_name," +  //14
                            "txtmat_unit1_name," +  //15

                              "txtQTY_Yokma," +  //16
                              "txtQTY_Remind," +  //17
                              "txtQTY_Plus," +  //18
                              "txtQTY_first," +  //19
                              "txtQTY_out," +  //20
                              "txtQTY_out_cut," +  //21
                              "txtQTY_Balance," +  //22

                              "txtprice," +   //23
                              "txtdiscount_rate," +  //24
                              "txtdiscount_money," +  //25
                              "txtsum_total," +  //26

                              "txtcost_qty_balance_yokma," +  //27
                              "txtcost_qty_price_average_yokma," +  //28
                              "txtcost_money_sum_yokma," +  //29

                              "txtcost_sale_qty_price_average," +  //28
                              "txtcost_sale_money_sum," +  //29

                              "txtcost_qty_balance_yokpai," +  //30
                              "txtcost_qty_price_average_yokpai," +  //31
                              "txtcost_money_sum_yokpai," +  //32

                              "txtitem_no," +  //33

                              "txtcoPO_id," +  //34
                              "txtbranchPO_id," +  //35
                              "txtqtyPO_want," +  //36
                              "txtqtyPO," +  //37
                              "txtqtyPO_balance) " +  //38

                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                           "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +

                            "'" + this.txtSC_id.Text.Trim() + "'," +  //  "txtSC_id," +  //6
                            "'" + this.GridView1.Rows[i].Cells["Col_txtORDER_id"].Value.ToString() + "'," +  //7
                             "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //8

                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //12
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //13
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //14
                            "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //15

                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value.ToString())) + "'," +  //16
                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_Remind"].Value.ToString())) + "'," +  //17
                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_Plus"].Value.ToString())) + "'," +  //18
                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_first"].Value.ToString())) + "'," +  //19
                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) + "'," +  //20
                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_out_cut"].Value.ToString())) + "'," +  //21
                             "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_Balance"].Value.ToString())) + "'," +  //22

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //23
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //24
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //25
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //26

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + "'," +  //27
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString())) + "'," +  //28
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + "'," +  //29

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_qty_price_average"].Value.ToString())) + "'," +  //28
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value.ToString())) + "'," +  //29

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //30
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //31
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //32

                            "'" + this.GridView1.Rows[i].Cells["Col_Auto_num"].Value.ToString() + "'," +  //33

                            "'" + this.GridView1.Rows[i].Cells["Col_txtcoPO_id"].Value.ToString() + "'," +  //34
                            "'" + this.GridView1.Rows[i].Cells["Col_txtbranchPO_id"].Value.ToString() + "'," +  //35

                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) + "'," +  //36
                            "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) + "'," +  //37
                            "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "')";  //38

                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok3");



                            //1.k018db_po_record_detail
                            cmd2.CommandText = "UPDATE k018db_po_record_detail SET " +
                                                "txtcoDE_id = '" + W_ID_Select.M_COID.Trim() + "'," +
                                                "txtbranchDE_id = '" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                                "txtSC_id = '" + this.txtSC_id.Text.Trim() + "'," +
                                               "txtqtyDE = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqtyPO"].Value.ToString())) + "'," +
                                               "txtqtyDE_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value.ToString())) + "'" +
                                               " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                               " AND (txtco_id = '" + this.GridView1.Rows[i].Cells["Col_txtcoPO_id"].Value.ToString() + "')" +
                                                " AND (txtbranch_id = '" + this.GridView1.Rows[i].Cells["Col_txtbranchPO_id"].Value.ToString() + "')" +
                                              " AND (txtPo_id = '" + this.GridView1.Rows[i].Cells["Col_txtORDER_id"].Value.ToString() + "')" +
                                               " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";


                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok27");

                            //=====================================================================================================
                            //สต๊อคสินค้า ตามคลัง =============================================================================================



                            //1.k021_mat_average
                            cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                               "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +
                                               "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +
                                                "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'" +
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


                                    "'" + this.txtSC_id.Text.Trim() + "'," +  //7 txtbill_id
                                    "'SC'," +  //9 txtbill_type
                                    "'ขายสด  " + this.PANEL103_CUS_txtcus_id.Text.ToString() + "  " + this.PANEL103_CUS_txtcus_name.Text.ToString() + "  " + this.txtsc_remark.Text.Trim() + "'," +  //9 txtbill_remark

                                     "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //7 txtwherehouse_id
                                   "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //10 
                                    "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //11
                                    "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //12

                                    "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //13
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //14
                                    "'N'," +  //15
                                    "''," +  //16
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //17

                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //18  txtqty_in
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //19 txtqty2_in
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //20 txtprice_in
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //21 txtsum_total_in

                                   "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) + "'," +  //22 txtqty_in
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //23 txtqty2_in
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_qty_price_average"].Value.ToString())) + "'," +  //24 txtprice_out
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                                   "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //26
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //27
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //28
                                   "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //29

                                   "'1')";   //30

                            cmd2.ExecuteNonQuery();
                            //MessageBox.Show("ok8");


                            //======================================

                            //สต๊อคสินค้า ตามคลัง =============================================================================================

                            //MessageBox.Show("ok4");


                        }
                    }

                    if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id_ok.Text.Trim() == "N")
                    {
                        cmd2.CommandText = "INSERT INTO s001_01sale_cash_record_group_tax(cdkey," +
                                           "txtco_id,txtacc_group_tax_id," +
                                           "txtacc_group_tax_name," +
                                           "txtacc_group_tax_vat_rate," +
                                           "txtuser_name)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() + "'," +
                                           "'" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text.Trim() + "'," +
                                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + "'," +
                                           "'" + W_ID_Select.M_USERNAME.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE s001_01sale_cash_record_group_tax SET txtacc_group_tax_id = '" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() + "'," +
                                           "txtacc_group_tax_name = '" + this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Text.Trim() + "'," +
                                           "txtacc_group_tax_vat_rate = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtvat_rate.Text)) + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtuser_name = '" + W_ID_Select.M_USERNAME.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }



                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.BtnNew.Enabled = true;
                        this.btnopen.Enabled = false;
                        this.BtnSave.Enabled = false;
                        this.btnPreview.Enabled = true;
                        this.btnPreview_copy.Enabled = true;
                        this.BtnPrint.Enabled = true;
                        this.BtnPrint_copy.Enabled = true;
                        this.BtnClose_Form.Enabled = true;

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "บันทึกขายสด")
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
                        this.btnPreview_copy.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnPrint_copy.Enabled = false;
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
                        this.btnPreview_copy.Enabled = false;
                        this.BtnPrint.Enabled = false;
                        this.BtnPrint_copy.Enabled = false;
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
            W_ID_Select.TRANS_ID = this.txtSC_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME05_Sales.HOME05_Sale_01sale_record_print frm2 = new kondate.soft.HOME05_Sales.HOME05_Sale_01sale_record_print();
            frm2.Show();
            frm2.BringToFront();
            //====================

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
            W_ID_Select.TRANS_ID = this.txtSC_id.Text.Trim();
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
            W_ID_Select.TRANS_ID = this.txtSC_id.Text.Trim();
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

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_s001_01sale_cash_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_s001_01sale_cash_record.rpt");


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
                rpt.SetParameterValue("txtSC_id", W_ID_Select.TRANS_ID.Trim());

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
        private void BtnPrint_copy_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            UPDATE_PRINT_BY();
            W_ID_Select.TRANS_ID = this.txtSC_id.Text.Trim();
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

                //rpt.Load("E:\\01_Project_ERP_Kondate.Soft\\kondate.soft\\kondate.soft\\KONDATE_REPORT\\Report_s001_01sale_cash_record.rpt");
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_s001_01sale_cash_record_copy.rpt");


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
                rpt.SetParameterValue("txtSC_id", W_ID_Select.TRANS_ID.Trim());

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
        private void btnbarcode_set_Click(object sender, EventArgs e)
        {
            this.ActiveControl = this.txtmat_barcode_id;
            this.txtmat_barcode_id.Text = "";
        }
        private void txtmat_barcode_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_barcode_id.Text.Trim() != "")
            {
                if (this.PANEL1306_WH_txtwherehouse_id.Text == "")
                {
                    MessageBox.Show("โปรด เลือก คลังสินค้าที่จะขาย ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                //=====================================================================================
                if (this.PANEL103_CUS_txtcus_name.Text == "")
                {
                    MessageBox.Show("โปรด เลือก ลูกค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (this.PANEL103_CUS.Visible == false)
                    {
                        this.PANEL103_CUS.Visible = true;
                        this.PANEL103_CUS.BringToFront();
                        this.PANEL103_CUS.Location = new Point(this.PANEL103_CUS_txtcus_name.Location.X, this.PANEL103_CUS_txtcus_name.Location.Y + 22);
                    }
                    else
                    {
                        this.PANEL103_CUS.Visible = false;
                    }
                    return;

                }
                //=====================================================================================
                if (this.txtOR_id.Text == "")
                {
                    //MessageBox.Show("โปรดใส่ อ้างอิงใบสั่งซื้อ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //return;
                }

                //for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
                //{
                    for (int j = 0; j < this.GridView1.Rows.Count - 0; j++)
                    {
                        if (this.txtmat_barcode_id.Text.ToString() == this.GridView1.Rows[j].Cells["Col_txtmat_id"].Value.ToString())
                        {
                            MessageBox.Show("รหัสสินค้านี้  :  " + this.GridView1.Rows[j].Cells["Col_txtmat_id"].Value.ToString() + "     เพิ่มเข้ามาในตารางแล้ว ระบบกำหนดให้ 1ตาราง ขายสินค้าได้ 1 รหัสสินค้าเท่านั้น", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                //}

                UPDATE_BARCODE_TO_GridView1();
                this.txtmat_barcode_id.Focus();

            }
        }
        private void btnAdd_qty_Click(object sender, EventArgs e)
        {

            GridView1.Rows[selectedRowIndex].Cells["Col_txtORDER_id"].Value = this.txtOR_id.Text.ToString();
            GridView1.Rows[selectedRowIndex].Cells["Col_txtQTY_out"].Value = this.txtqty.Text.ToString();

            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();
        }
        private void UPDATE_BARCODE_TO_GridView1()
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

            PANEL_MAT_Show_GridView1();
            PANEL_MAT_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                     "b001mat_04barcode.*," +
                                     "b001mat_61change_price_main.*," +
                                     "b001_05mat_unit1.*" +
                                     " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                      " INNER JOIN b001mat_04barcode" +
                                    " ON b001mat.cdkey = b001mat_04barcode.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_04barcode.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_04barcode.txtmat_id" +

                                    " INNER JOIN b001mat_61change_price_main" +
                                    " ON b001mat.cdkey = b001mat_61change_price_main.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_61change_price_main.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_61change_price_main.txtmat_id" +


                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat_04barcode.txtmat_barcode_id = '" + this.txtmat_barcode_id.Text.Trim() + "')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

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
                        //=======================================================
                        var index = GridView1.Rows.Add();
                        GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                        GridView1.Rows[index].Cells["Col_txtORDER_id"].Value = this.txtOR_id.Text.ToString();      //1
                        GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //2

                        GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //3
                        GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //4
                        GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //5
                        GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //6

                        GridView1.Rows[index].Cells["Col_txtQTY_Yokma"].Value = "0"; //7
                        GridView1.Rows[index].Cells["Col_txtQTY_Remind"].Value = "0"; //8
                        GridView1.Rows[index].Cells["Col_txtQTY_Plus"].Value = this.txtqty.Text.ToString(); //9
                        GridView1.Rows[index].Cells["Col_txtQTY_first"].Value = this.txtqty.Text.ToString();//10
                        GridView1.Rows[index].Cells["Col_txtQTY_out"].Value = this.txtqty.Text.ToString();//11
                        GridView1.Rows[index].Cells["Col_txtQTY_out_cut"].Value = "0"; //12
                        GridView1.Rows[index].Cells["Col_txtQTY_Balance"].Value = "0"; //13

                        GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //14
                        GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //15
                        GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";    //Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //16
                        GridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0"; // Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //17

                        GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = "0";      //18
                        GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = "0";      //19
                        GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = "0";      //20

                        GridView1.Rows[index].Cells["Col_txtcost_sale_qty_price_average"].Value = "0";      //21
                        GridView1.Rows[index].Cells["Col_txtcost_sale_money_sum"].Value = "0";      //22

                        GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = "0";      //23
                        GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = "0";      //24
                        GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = "0";      //25

                        GridView1.Rows[index].Cells["Col_txtcoDE_id"].Value = "";      //26
                        GridView1.Rows[index].Cells["Col_txtbranchDE_id"].Value = "";      //27

                        GridView1.Rows[index].Cells["Col_txtqtyDE_want"].Value = Convert.ToSingle(0).ToString("###,###.00"); //28
                        GridView1.Rows[index].Cells["Col_txtqtyDE"].Value = Convert.ToSingle(0).ToString("###,###.00"); //29
                        GridView1.Rows[index].Cells["Col_txtqtyDE_balance"].Value = Convert.ToSingle(0).ToString("###,###.00"); //30

                        }

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
            GridView1_Color_Column();
            GridView1_Cal_Sum();
            Sum_group_tax();

        }



        //GridView1   =======================================================================
        int selectedRowIndex;
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
            this.GridView1.Columns[26].Name = "Col_txtcoPO_id";
            this.GridView1.Columns[27].Name = "Col_txtbranchPO_id";

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
            this.GridView1.Columns[26].HeaderText = "Col_txtcoPO_id";
            this.GridView1.Columns[27].HeaderText = "Col_txtbranchPO_id";

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

            this.GridView1.Columns["Col_txtcoPO_id"].Visible = false;  //"Col_txtcoPO_id";
            this.GridView1.Columns["Col_txtbranchPO_id"].Visible = false;  //"Col_txtbranchPO_id";

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
        private void GridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmat_id"].Value;
                if (cell != null)
                {

                    this.txtOR_id.Text = row.Cells["Col_txtORDER_id"].Value.ToString();
                    this.txtmat_barcode_id.Text = row.Cells["Col_txtmat_id"].Value.ToString();
                    this.txtqty.Text = row.Cells["Col_txtQTY_out"].Value.ToString();

                    //this.PANEL_MAT_txtmat_name.Text = row.Cells["Col_txtmat_name"].Value.ToString();
                    //this.txtmat_unit1_name.Text = row.Cells["Col_txtmat_unit1_name"].Value.ToString();
                }
            }
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
        private void GridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -0)
            {
                this.GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                this.GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = GridView1.CurrentRow.Index;
            this.btnremove_row.Visible = true;
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
                GridView1.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[i].Cells["Col_txtQTY_out"].Style.BackColor = Color.LightSkyBlue;
            }
        }
        private void GridView1_Cal_Sum()
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

                    this.GridView1.Rows[i].Cells["Col_txtcost_sale_qty_price_average"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value).ToString("###,###.00");     //9
                    this.GridView1.Rows[i].Cells["Col_txtcost_sale_money_sum"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value).ToString("###,###.00");     //10

                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value).ToString("###,###.00");     //11
                    this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value).ToString("###,###.00");     //12
                    this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value).ToString("###,###.00");     //13

                    if (Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString())) > Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value.ToString())))
                    {
                        MessageBox.Show("สต๊อคคงเหลือ น้อยกว่า จำนวน ขาย ไม่สามารถขายได้ !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtQTY_Yokma"].Value).ToString("###,###.00");     //14
                        return;
                    }

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
                        this.txtsum_qty_sc.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));


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

                    //this.GridView1.Rows[i].Cells["Col_txtqtyPO"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value).ToString("###,###.00");     
                    //Sum_Total  =================================================
                    Sum_Total2 = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtQTY_out"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value = Sum_Total2.ToString("N", new CultureInfo("en-US"));
                    Sum_Total3 = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_want"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqtyPO_balance"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqtyPO"].Value = Sum_Total3.ToString("N", new CultureInfo("en-US"));

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

                    cmd2.CommandText = "UPDATE s001_01sale_cash_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                 " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                " AND (txtSC_id = '" + this.txtSC_id.Text.Trim() + "')";
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

                cmd1.CommandText = "SELECT * FROM s001_01sale_cash_record_group_tax" +
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
                                  " FROM s001_01sale_cash_record_trans" +
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

                        transNum = Convert.ToDouble(string.Format("{0:n}", trans_Right6)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        trans = transNum.ToString("00000#");

                        if (year2.Trim() == year_now2.Trim())
                        {
                            TMP = "DE" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "DE" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "DE" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtSC_id.Text = TMP.Trim();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



        }

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



        //END txtwherehouse คลังสินค้า  =======================================================================

        //txtcus ลูกค้า  =======================================================================
        private void PANEL103_CUS_Fill_cus()
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

            PANEL103_CUS_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL103_CUS_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL103_CUS_dataGridView1.Columns[1].Name = "Col_txtcus_no";
                //this.PANEL103_CUS_dataGridView1.Columns[2].Name = "Col_txtcus_id";
                //this.PANEL103_CUS_dataGridView1.Columns[3].Name = "Col_txtcus_name";
                //this.PANEL103_CUS_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
                //this.PANEL103_CUS_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL103_CUS_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL103_CUS_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL103_CUS_dataGridView1.Columns[8].Name = "Col_txtcus_status";

                cmd2.CommandText = "SELECT s001_03cus.*," +
                                    "s001_03cus_1address.*" +
                                    " FROM s001_03cus" +

                                    " INNER JOIN s001_03cus_1address" +
                                    " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                    " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                    " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                    " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                     " AND (s001_03cus.txtcus_id <> '')" +
                                   " ORDER BY s001_03cus.txtcus_no ASC";

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
                            var index = PANEL103_CUS_dataGridView1.Rows.Add();
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_no"].Value = dt2.Rows[j]["txtcus_no"].ToString();      //1
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_id"].Value = dt2.Rows[j]["txtcus_id"].ToString();      //2
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name"].Value = dt2.Rows[j]["txtcus_name"].ToString();      //3
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name_eng"].Value = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_status"].Value = dt2.Rows[j]["txtcus_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL103_CUS_Clear_GridView1_Up_Status();

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
        private void PANEL103_CUS_GridView1_cus()
        {
            this.PANEL103_CUS_dataGridView1.ColumnCount = 9;
            this.PANEL103_CUS_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL103_CUS_dataGridView1.Columns[1].Name = "Col_txtcus_no";
            this.PANEL103_CUS_dataGridView1.Columns[2].Name = "Col_txtcus_id";
            this.PANEL103_CUS_dataGridView1.Columns[3].Name = "Col_txtcus_name";
            this.PANEL103_CUS_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
            this.PANEL103_CUS_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
            this.PANEL103_CUS_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
            this.PANEL103_CUS_dataGridView1.Columns[7].Name = "Col_txtremark";
            this.PANEL103_CUS_dataGridView1.Columns[8].Name = "Col_txtcus_status";

            this.PANEL103_CUS_dataGridView1.Columns[0].HeaderText = "No";
            this.PANEL103_CUS_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL103_CUS_dataGridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL103_CUS_dataGridView1.Columns[3].HeaderText = " ชื่อ ลูกค้า";
            this.PANEL103_CUS_dataGridView1.Columns[4].HeaderText = " ชื่อ ลูกค้า Eng";
            this.PANEL103_CUS_dataGridView1.Columns[5].HeaderText = " ผู้ติดต่อ";
            this.PANEL103_CUS_dataGridView1.Columns[6].HeaderText = " เบอร์โทร";
            this.PANEL103_CUS_dataGridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.PANEL103_CUS_dataGridView1.Columns[8].HeaderText = " สถานะ";

            this.PANEL103_CUS_dataGridView1.Columns[0].Visible = false;  //"No";
            this.PANEL103_CUS_dataGridView1.Columns[1].Visible = true;  //"Col_txtcus_no";
            this.PANEL103_CUS_dataGridView1.Columns[1].Width = 100;
            this.PANEL103_CUS_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[2].Visible = true;  //"Col_txtcus_id";
            this.PANEL103_CUS_dataGridView1.Columns[2].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[3].Visible = true;  //"Col_txtcus_name";
            this.PANEL103_CUS_dataGridView1.Columns[3].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[4].Visible = false;  //"Col_txtcus_name_eng";
            this.PANEL103_CUS_dataGridView1.Columns[4].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[5].Visible = true;  //"Col_txtcontact_person";
            this.PANEL103_CUS_dataGridView1.Columns[5].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[6].Visible = false;  //"Col_txtcontact_person_tel";
            this.PANEL103_CUS_dataGridView1.Columns[6].Width = 150;
            this.PANEL103_CUS_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_CUS_dataGridView1.Columns[7].Visible = true;  //"Col_txtremark";
            this.PANEL103_CUS_dataGridView1.Columns[7].Width = 100;
            this.PANEL103_CUS_dataGridView1.Columns[7].ReadOnly = true;
            this.PANEL103_CUS_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_CUS_dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_CUS_dataGridView1.Columns[8].Visible = false;  //"Col_txtcus_status";

            this.PANEL103_CUS_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL103_CUS_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL103_CUS_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL103_CUS_dataGridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL103_CUS_dataGridView1.Columns.Add(dgvCmb);

        }
        private void PANEL103_CUS_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL103_CUS_dataGridView1.Rows.Count; i++)
            {
                if (this.PANEL103_CUS_dataGridView1.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.PANEL103_CUS_dataGridView1.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.PANEL103_CUS_dataGridView1.Rows[i].Cells[9].Value = false;

                }
            }
        }
        private void PANEL103_CUS_Clear_GridView1()
        {
            this.PANEL103_CUS_dataGridView1.Rows.Clear();
            this.PANEL103_CUS_dataGridView1.Refresh();
        }
        private void PANEL103_CUS_txtcus_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL103_CUS.Visible == false)
                {
                    this.PANEL103_CUS.Visible = true;
                    this.PANEL103_CUS.Location = new Point(this.PANEL103_CUS_txtcus_name.Location.X, this.PANEL103_CUS_txtcus_name.Location.Y + 22);
                    this.PANEL103_CUS_dataGridView1.Focus();
                }
                else
                {
                    this.PANEL103_CUS.Visible = false;
                }
        }
        private void PANEL103_CUS_btncus_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_CUS.Visible == false)
            {
                this.PANEL103_CUS.Visible = true;
                this.PANEL103_CUS.BringToFront();
                this.PANEL103_CUS.Location = new Point(this.PANEL103_CUS_txtcus_name.Location.X, this.PANEL103_CUS_txtcus_name.Location.Y + 22);
            }
            else
            {
                this.PANEL103_CUS.Visible = false;
            }
        }
        private void PANEL103_CUS_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_CUS.Visible == false)
            {
                this.PANEL103_CUS.Visible = true;
            }
            else
            {
                this.PANEL103_CUS.Visible = false;
            }
        }
        private void PANEL103_CUS_dataGridView1_cus_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL103_CUS_dataGridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL103_CUS_txtcus_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL103_CUS_txtcus_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL103_CUS_dataGridView1_cus_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL103_CUS_dataGridView1.CurrentRow.Index;

                this.PANEL103_CUS_txtcus_id.Text = PANEL103_CUS_dataGridView1.CurrentRow.Cells[1].Value.ToString();
                this.PANEL103_CUS_txtcus_name.Text = PANEL103_CUS_dataGridView1.CurrentRow.Cells[2].Value.ToString();
                this.PANEL103_CUS_txtcus_name.Focus();
                this.PANEL103_CUS.Visible = false;
            }
        }
        private void PANEL103_CUS_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL103_CUS_btn_search_Click(object sender, EventArgs e)
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

            PANEL103_CUS_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL103_CUS_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL103_CUS_dataGridView1.Columns[1].Name = "Col_txtcus_no";
                //this.PANEL103_CUS_dataGridView1.Columns[2].Name = "Col_txtcus_id";
                //this.PANEL103_CUS_dataGridView1.Columns[3].Name = "Col_txtcus_name";
                //this.PANEL103_CUS_dataGridView1.Columns[4].Name = "Col_txtcus_name_eng";
                //this.PANEL103_CUS_dataGridView1.Columns[5].Name = "Col_txtcontact_person";
                //this.PANEL103_CUS_dataGridView1.Columns[6].Name = "Col_txtcontact_person_tel";
                //this.PANEL103_CUS_dataGridView1.Columns[7].Name = "Col_txtremark";
                //this.PANEL103_CUS_dataGridView1.Columns[8].Name = "Col_txtcus_status";

                cmd2.CommandText = "SELECT s001_03cus.*," +
                                    "s001_03cus_1address.*" +
                                    " FROM s001_03cus" +

                                    " INNER JOIN s001_03cus_1address" +
                                    " ON s001_03cus.cdkey = s001_03cus_1address.cdkey" +
                                    " AND s001_03cus.txtco_id = s001_03cus_1address.txtco_id" +
                                    " AND s001_03cus.txtcus_id = s001_03cus_1address.txtcus_id" +

                                    " WHERE (s001_03cus.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (s001_03cus.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (s001_03cus.txtcus_name LIKE '%" + this.PANEL103_CUS_txtsearch.Text.Trim() + "%')" +
                                    " AND (s001_03cus.txtcus_id <> '')" +
                                   " ORDER BY s001_03cus.txtcus_no ASC";

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
                            var index = PANEL103_CUS_dataGridView1.Rows.Add();
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_no"].Value = dt2.Rows[j]["txtcus_no"].ToString();      //1
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_id"].Value = dt2.Rows[j]["txtcus_id"].ToString();      //2
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name"].Value = dt2.Rows[j]["txtcus_name"].ToString();      //3
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_name_eng"].Value = dt2.Rows[j]["txtcus_name_eng"].ToString();      //4
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person"].Value = dt2.Rows[j]["txtcontact_person"].ToString();      //5
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcontact_person_tel"].Value = dt2.Rows[j]["txtcontact_person_tel"].ToString();      //6
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtremark"].Value = dt2.Rows[j]["txtremark"].ToString();      //7
                            PANEL103_CUS_dataGridView1.Rows[index].Cells["Col_txtcus_status"].Value = dt2.Rows[j]["txtcus_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL103_CUS_Clear_GridView1_Up_Status();

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
        private void PANEL103_CUS_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL103_CUS_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL103_CUS.Height = PANEL103_CUS_btnresize_low.Top + e.Y;
                this.PANEL103_CUS.Width = PANEL103_CUS_btnresize_low.Left + e.X;
            }
        }
        private void PANEL103_CUS_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL103_CUS_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtcus ลูกค้า  =======================================================================

        //จบส่วนตารางสำหรับบันทึก========================================================================






        //รับเงิน===========================================================================================
        private void PANEL_02PAY_TYPE_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_02PAY_TYPE_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_02PAY_TYPE.Left = e.X + PANEL_02PAY_TYPE.Left - MouseDownLocation.X;
                PANEL_02PAY_TYPE.Top = e.Y + PANEL_02PAY_TYPE.Top - MouseDownLocation.Y;
            }
        }
        private void SUM_RECEIVE_MONEY()
        {
            double SUM_A = 0;
            double SUM_B = 0;

            SUM_A = Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtcash.Text)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtchecque.Text))
                               + Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtcredit_card.Text)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txttransfer_money.Text));
            this.PANEL_02PAY_TYPE_txtSum_total_pay.Text = SUM_A.ToString("N", new CultureInfo("en-US"));

            SUM_B = Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtSum_total_pay.Text));
            this.PANEL_02PAY_TYPE_txtSum_total_balance.Text = SUM_B.ToString("N", new CultureInfo("en-US"));


        }
        private void PANEL_02PAY_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE.Visible == false)
            {
                this.PANEL_02PAY_TYPE.Visible = true;
                this.BtnSave.Enabled = false;
            }
            else
            {
                this.PANEL_02PAY_TYPE.Visible = false;
                this.BtnSave.Enabled = true;
            }
        }
        private void PANEL_02PAY_TYPE_BtnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty_sc.Text)) == 0)
            {
                MessageBox.Show("ไม่พบ จำนวนสินค้า !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text == "")
            {
                MessageBox.Show("โปรด เลือกกลุ่มภาษี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Focus();
                return;
            }
            if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtSum_total_balance.Text)) > 0)
            {
                MessageBox.Show("ไม่ให้เหลือ ยอดค้างชำระ !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtSum_total_pay.Text)) != Convert.ToDouble(string.Format("{0:n}", this.PANEL_02PAY_TYPE_txtmoney_sum.Text)))
            {
                MessageBox.Show("ยอดขายกับ ยอดรับชำระไม่ตรงกัน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            INSERT_SALE_CASH();

        }
        private void PANEL_02PAY_TYPE_txtcash_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.PANEL_02PAY_TYPE_txtcash.Text.Trim() != "")
            {
                SUM_RECEIVE_MONEY();
            }
        }



        private void Fill_PANEL_02PAY_TYPE_2CHECQUE_BANK()
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
                                  " FROM k013_1db_acc_09code_bank" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtcode_bank_id <> '')" +
                                  " ORDER BY txtcode_bank_id";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtcode_bank = dr["txtcode_bank_name"].ToString();
                        this.PANEL_02PAY_TYPE_2CHECQUE_cbobank_name.Items.Add(txtcode_bank);
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

        }
        private void Fill_PANEL_02PAY_TYPE_2CHECQUE_BANK2()
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
                                  " FROM k013_1db_acc_09code_bank" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtcode_bank_id <> '')" +
                                  " AND (txtcode_bank_name = '" + this.PANEL_02PAY_TYPE_2CHECQUE_cbobank_name.Text.Trim() + "')";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.PANEL_02PAY_TYPE_2CHECQUE_txtbank_id.Text = dt.Rows[0]["txtcode_bank_id"].ToString();
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

        }
        private void PANEL_02PAY_TYPE_2CHECQUE_cbobank_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_PANEL_02PAY_TYPE_2CHECQUE_BANK2();
        }
        private void PANEL_02PAY_TYPE_2CHECQUE_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_02PAY_TYPE_2CHECQUE_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_02PAY_TYPE_2CHECQUE.Left = e.X + PANEL_02PAY_TYPE_2CHECQUE.Left - MouseDownLocation.X;
                PANEL_02PAY_TYPE_2CHECQUE.Top = e.Y + PANEL_02PAY_TYPE_2CHECQUE.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_02PAY_TYPE_2CHECQUE_BtnOK_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE_2CHECQUE.Visible == false)
            {
                this.PANEL_02PAY_TYPE_2CHECQUE.Visible = true;
            }
            else
            {
                this.PANEL_02PAY_TYPE_2CHECQUE.Visible = false;
                this.PANEL_02PAY_TYPE_txtchecque.Text = this.PANEL_02PAY_TYPE_2CHECQUE_txtsum_receipt_money.Text.ToString();
                SUM_RECEIVE_MONEY();
            }
        }
        private void PANEL_02PAY_TYPE_2CHECQUE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE_2CHECQUE.Visible == false)
            {
                this.PANEL_02PAY_TYPE_2CHECQUE.Visible = true;
            }
            else
            {
                this.PANEL_02PAY_TYPE_2CHECQUE.Visible = false;
            }
        }
        private void PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Format = DateTimePickerFormat.Custom;
            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.CustomFormat = this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Value.ToString("dd-MM-yyyy", UsaCulture);

        }



        private void Fill_PANEL_02PAY_TYPE_3CREDIT_CARD_BANK()
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
                                  " FROM k013_1db_acc_09code_bank" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtcode_bank_id <> '')" +
                                  " ORDER BY txtcode_bank_id";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtcode_bank = dr["txtcode_bank_name"].ToString();
                        this.PANEL_02PAY_TYPE_3CREDIT_CARD_cbobank_name.Items.Add(txtcode_bank);
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

        }
        private void Fill_PANEL_02PAY_TYPE_3CREDIT_CARD_BANK2()
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
                                  " FROM k013_1db_acc_09code_bank" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtcode_bank_id <> '')" +
                                  " AND (txtcode_bank_name = '" + this.PANEL_02PAY_TYPE_3CREDIT_CARD_cbobank_name.Text.Trim() + "')";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.PANEL_02PAY_TYPE_3CREDIT_CARD_txtbank_id.Text = dt.Rows[0]["txtcode_bank_id"].ToString();
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

        }
        private void PANEL_02PAY_TYPE_3CREDIT_CARD_cbobank_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_PANEL_02PAY_TYPE_3CREDIT_CARD_BANK2();
        }
        private void PANEL_02PAY_TYPE_3CREDIT_CARD_cbotype_credit_card_name_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void PANEL_02PAY_TYPE_3CREDIT_CARD_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_02PAY_TYPE_3CREDIT_CARD_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_02PAY_TYPE_3CREDIT_CARD.Left = e.X + PANEL_02PAY_TYPE_3CREDIT_CARD.Left - MouseDownLocation.X;
                PANEL_02PAY_TYPE_3CREDIT_CARD.Top = e.Y + PANEL_02PAY_TYPE_3CREDIT_CARD.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_02PAY_TYPE_3CREDIT_CARD_BtnOK_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible == false)
            {
                this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible = true;
            }
            else
            {
                this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible = false;
                this.PANEL_02PAY_TYPE_txtcredit_card.Text = this.PANEL_02PAY_TYPE_3CREDIT_CARD_txtsale_cash_money.Text.ToString();
                SUM_RECEIVE_MONEY();
            }
        }
        private void PANEL_02PAY_TYPE_3CREDIT_CARD_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible == false)
            {
                this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible = true;
            }
            else
            {
                this.PANEL_02PAY_TYPE_3CREDIT_CARD.Visible = false;
            }
        }



        private void Fill_PANEL_02PAY_TYPE_4TRANSFER_BANK()
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
                                  " FROM k013_1db_acc_09code_bank" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtcode_bank_id <> '')" +
                                  " ORDER BY txtcode_bank_id";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtcode_bank = dr["txtcode_bank_name"].ToString();
                        this.PANEL_02PAY_TYPE_4TRANSFER_cbobank_name.Items.Add(txtcode_bank);
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

        }
        private void Fill_PANEL_02PAY_TYPE_4TRANSFER_BANK2()
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
                                  " FROM k013_1db_acc_09code_bank" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtcode_bank_id <> '')" +
                                  " AND (txtcode_bank_name = '" + this.PANEL_02PAY_TYPE_4TRANSFER_cbobank_name.Text.Trim() + "')";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.PANEL_02PAY_TYPE_4TRANSFER_txtbank_id.Text = dt.Rows[0]["txtcode_bank_id"].ToString();
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

        }
        private void PANEL_02PAY_TYPE_4TRANSFER_cbobank_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_PANEL_02PAY_TYPE_4TRANSFER_BANK2();
        }
        private void PANEL_02PAY_TYPE_4TRANSFER_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void PANEL_02PAY_TYPE_4TRANSFER_panel_top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PANEL_02PAY_TYPE_4TRANSFER.Left = e.X + PANEL_02PAY_TYPE_4TRANSFER.Left - MouseDownLocation.X;
                PANEL_02PAY_TYPE_4TRANSFER.Top = e.Y + PANEL_02PAY_TYPE_4TRANSFER.Top - MouseDownLocation.Y;
            }
        }
        private void PANEL_02PAY_TYPE_4TRANSFER_BtnOK_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE_4TRANSFER.Visible == false)
            {
                this.PANEL_02PAY_TYPE_4TRANSFER.Visible = true;
            }
            else
            {
                this.PANEL_02PAY_TYPE_4TRANSFER.Visible = false;
                this.PANEL_02PAY_TYPE_txttransfer_money.Text = this.PANEL_02PAY_TYPE_4TRANSFER_txtsum_receipt_money.Text.ToString();
                SUM_RECEIVE_MONEY();
            }
        }
        private void PANEL_02PAY_TYPE_4TRANSFER_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL_02PAY_TYPE_4TRANSFER.Visible == false)
            {
                this.PANEL_02PAY_TYPE_4TRANSFER.Visible = true;
            }
            else
            {
                this.PANEL_02PAY_TYPE_4TRANSFER.Visible = false;
            }
        }
        private void PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.Format = DateTimePickerFormat.Custom;
            this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.CustomFormat = this.PANEL_02PAY_TYPE_4TRANSFER_dtpdate_transfer.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

        //รับเงิน===========================================================================================











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
                              "txtcost_qty1_balance," +  //9
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
                               "@txtcost_qty1_balance," +  //9
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

                                cmd2.Parameters.Add("@txtcost_qty1_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));  //9
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

        }
    }
}
