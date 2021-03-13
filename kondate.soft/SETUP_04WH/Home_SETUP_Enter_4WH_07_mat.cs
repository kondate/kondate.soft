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

namespace kondate.soft.SETUP_4WH
{
    public partial class Home_SETUP_Enter_4WH_07_mat : Form
    {
        //Move Form ====================================
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //END Move Form ====================================
        string SL = "";
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




        public Home_SETUP_Enter_4WH_07_mat()
        {
            InitializeComponent();
        }



        private void Home_SETUP_Enter_4WH_07_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "S407";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            //this.Paneldate_dtpsupplier_birth_day.Value = DateTime.Now;
            //this.Paneldate_dtpsupplier_birth_day.Format = DateTimePickerFormat.Custom;
            //this.Paneldate_dtpsupplier_birth_day.CustomFormat = this.Paneldate_dtpsupplier_birth_day.Value.ToString("dd-MM-yyyy", UsaCulture);

            //this.Paneldate_txtdate.Text = this.Paneldate_dtpsupplier_birth_day.Value.ToString("dd-MM-yyyy", UsaCulture);

            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มรหัสสินค้าใหม่";
            this.txtmat_id.ReadOnly = false;
            this.ActiveControl = this.txtmat_id;

            this.btnPreview.Enabled = true;
            this.BtnPrint.Enabled = true;
            this.BtnCancel_Doc.Enabled = false;

            PANEL101_MAT_TYPE_GridView1_mat_type();
            PANEL101_MAT_TYPE_Fill_mat_type();

            PANEL102_MAT_SAC_GridView1_mat_sac();
            PANEL102_MAT_SAC_Fill_mat_sac();

            PANEL103_MAT_GROUP_GridView1_mat_group();
            PANEL103_MAT_GROUP_Fill_mat_group();

            PANEL104_MAT_BRAND_GridView1_mat_brand();
            PANEL104_MAT_BRAND_Fill_mat_brand();

            PANEL105_MAT_UNIT1_GridView1_mat_unit();
            PANEL105_MAT_UNIT1_Fill_mat_unit();

            PANEL105_MAT_UNIT2_GridView1_mat_unit();
            PANEL105_MAT_UNIT2_Fill_mat_unit();

            //=====================================
            Fill_Cbomat_detail_group_name();
            this.Cbomat_detail_group_name.Text = "สินค้าปกติ";
            this.txtmat_detail_group_id.Text = "1";
            //=====================================

            Fill_Cbomat_incentive_name();
            this.Cbomat_incentive_name.Text = "ไม่คิดคอมมิชชั่น";
            this.txtmat_incentive_id.Text = "1";
            //=====================================

            Fill_Cbomat_tax_name();
            this.Cbomat_tax_name.Text = "คิดภาษี";
            this.txtmat_tax_id.Text = "1";
            //=====================================

            Fill_Cbomat_credit_charge_name();
            this.Cbomat_credit_charge_name.Text = "ไม่ชาร์จบัตรเครดิต";
            this.txtmat_credit_charge_id.Text = "1";

            //=====================================

            Fill_Cbomat_type_with_acc_name();
            this.Cbomat_type_with_acc_name.Text = "สินค้า";
            this.txtmat_type_with_acc_id.Text = "1";
            //=====================================

            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();

            PANEL161_SUP_GridView2_supplier();

            Run_ID();
            CHECK_UP_NO999();

            PANEL_FORM1_GridView1();
            Fill_PANEL_FORM1_dataGridView1();

            //========================================
            this.cboSearch.Items.Add("รหัสสินค้า");
            this.cboSearch.Items.Add("ชื่อสินค้า");
            //========================================

            PANEL101_MAT_TYPE2_GridView1_mat_type();
            PANEL101_MAT_TYPE2_Fill_mat_type();

            PANEL102_MAT_SAC2_GridView1_mat_sac();
            PANEL102_MAT_SAC2_Fill_mat_sac();

            PANEL103_MAT_GROUP2_GridView1_mat_group();
            PANEL103_MAT_GROUP2_Fill_mat_group();

            PANEL104_MAT_BRAND2_GridView1_mat_brand();
            PANEL104_MAT_BRAND2_Fill_mat_brand();

        }

        private void Run_ID()
        {
            if (this.txtmat_no.Text == "")
            {
                this.txtmat_no.Text = "00001";
            }

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
            //เชื่อมต่อฐานข้อมูล======================================================
            Cursor.Current = Cursors.WaitCursor;
            string RID = "";
            double Rid2 = 0;
            double Rid3 = 0;

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM b001mat" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    "AND (txtmat_id <> '')" +
                                    " ORDER BY txtmat_no DESC";

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

                        RID = dt2.Rows[0]["txtmat_no"].ToString();      //1
                        Rid2 = Convert.ToDouble(RID);


                        Rid3 = Convert.ToDouble(string.Format("{0:n}", Rid2)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        this.txtmat_no.Text = Rid3.ToString("0000#");
                        Cursor.Current = Cursors.Default;

                    }
                    else
                    {
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

            W_ID_Select.LOG_ID = "3";
            W_ID_Select.LOG_NAME = "ใหม่";
            TRANS_LOG();

            this.Hide();
            var frm2 = new Home_SETUP_Enter_4WH_07_mat();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มรหัสสินค้าใหม่";
            this.txtmat_id.ReadOnly = false;
            this.btnUp_pic1.Visible = false;
            this.btnUp_pic2.Visible = false;
            this.btnUp_pic3.Visible = false;
            this.btnUp_pic4.Visible = false;

        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_OPEN == "N")
            {

                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            W_ID_Select.LOG_ID = "4";
            W_ID_Select.LOG_NAME = "เปิดแก้ไข";
            TRANS_LOG();

            if (this.txtmat_id.Text != "")
            {
                this.iblword_status.Text = "แก้ไขรหัสสินค้า";
                this.txtmat_id.ReadOnly = true;
                this.btnUp_pic1.Visible = true;
                this.btnUp_pic2.Visible = true;
                this.btnUp_pic3.Visible = true;
                this.btnUp_pic4.Visible = true;

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.check_mat_status.Checked == true)
            {
                this.txtchmat_unit_status.Text = "Y";
            }
            if (this.check_mat_status.Checked == false)
            {
                this.txtchmat_unit_status.Text = "N";
            }

            if (this.txtmat_id.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัส สินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtmat_id.Focus();
                return;
            }
            if (this.txtmat_no.Text == "")
            {
                MessageBox.Show("โปรดใส่ ลำดับ สินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtmat_no.Focus();
                return;
            }
            if (this.txtmat_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtmat_name.Focus();
                return;
            }
            if (this.txtmat_unit1_qty.Text == "")
            {
                this.txtmat_unit1_qty.Text = "0";
            }
            if (this.txtmat_qty_min.Text == "")
            {
                this.txtmat_qty_min.Text = "0";
            }
            if (this.txtmat_qty_max.Text == "")
            {
                this.txtmat_qty_max.Text = "0";
            }
            if (this.txtmat_qty_per_labor.Text == "")
            {
                this.txtmat_qty_per_labor.Text = "0";
            }

            if (this.txtmat_unit2_qty.Text == "")
            {
                this.txtmat_unit2_qty.Text = ".0000";
            }
            if (this.txtmat_price_sale1.Text == "")
            {
                this.txtmat_price_sale1.Text = "0";
            }
            if (this.txtmat_price_sale2.Text == "")
            {
                this.txtmat_price_sale2.Text = "0";
            }
            if (this.txtmat_price_sale3.Text == "")
            {
                this.txtmat_price_sale3.Text = "0";
            }
            if (this.txtmat_price_sale4.Text == "")
            {
                this.txtmat_price_sale4.Text = "0";
            }
            if (this.txtmat_price_sale5.Text == "")
            {
                this.txtmat_price_sale5.Text = "0";
            }
            if (this.txtmat_price_sale6.Text == "")
            {
                this.txtmat_price_sale6.Text = "0";
            }
            if (this.txtmat_price_sale7.Text == "")
            {
                this.txtmat_price_sale7.Text = "0";
            }
            if (this.txtmat_price_sale8.Text == "")
            {
                this.txtmat_price_sale8.Text = "0";
            }
            if (this.txtmat_price_sale9.Text == "")
            {
                this.txtmat_price_sale9.Text = "0";
            }
            if (this.txtmat_price_sale10.Text == "")
            {
                this.txtmat_price_sale10.Text = "0";
            }

            if (this.txtmat_qty_width.Text == "")
            {
                this.txtmat_qty_width.Text = "0";
            }
            if (this.txtmat_qty_weight.Text == "")
            {
                this.txtmat_qty_weight.Text = "0";
            }
            if (this.txtmat_qty_long.Text == "")
            {
                this.txtmat_qty_long.Text = "0";
            }
            if (this.txtmat_qty_high.Text == "")
            {
                this.txtmat_qty_high.Text = "0";
            }
            if (this.txtmat_amount_phurchase.Text == "")
            {
                this.txtmat_amount_phurchase.Text = "0";
            }




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


            if (this.iblword_status.Text.Trim() == "เพิ่มรหัสสินค้าใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM b001mat" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("รหัส สินค้า นี้ซ้ำ  : '" + this.txtmat_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtmat_id.Focus();
                        conn.Close();
                        return;
                    }
                }

                //
                conn.Close();
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มรหัสสินค้าใหม่")
                    {
                        //1
                        cmd2.CommandText = "INSERT INTO b001mat(cdkey,txtco_id," +  //1
                                           "txtmat_id,txtmat_no," +  //2
                                           "txtmat_name,txtmat_name_eng," +  //3
                                           "txtmat_name_market,txtmat_name_bill," +  //4
                                          "txtmat_status) " +  //5
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtmat_id,@txtmat_no," +  //2
                                           "@txtmat_name,@txtmat_name_eng," +  //3
                                           "@txtmat_name_market,@txtmat_name_bill," +  //4
                                           "@txtmat_status)";   //5

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.txtmat_no.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.txtmat_name.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_eng", SqlDbType.NVarChar).Value = this.txtmat_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_market", SqlDbType.NVarChar).Value = this.txtmat_name_market.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_bill", SqlDbType.NVarChar).Value = this.txtmat_name_bill.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //2
                        cmd2.CommandText = "INSERT INTO b001mat_02detail(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_type_id,txtmat_sac_id," +  //2
                                           "txtmat_group_id,txtmat_brand_id," +  //3
                                           "txtmat_unit1_id,txtmat_unit1_qty," +  //4
                                           "chmat_unit_status," +  //5
                                           "txtmat_unit2_id,txtmat_unit2_qty," +  //6
                                           "txtmat_unit3_id," +  //5
                                           "txtmat_unit4_id," +  //5
                                           "txtmat_unit5_id," +  //5
                                           "txtmat_detail_group_id,txtmat_incentive_id," +  //7
                                           "txtmat_tax_id,txtmat_credit_charge_id," +  //8
                                           "txtmat_type_with_acc_id," +  //9
                                            "txtmat_qty_min,txtmat_qty_max," +  //10
                                           "txtmat_qty_per_labor," +  //11
                                           "txtmat_remark) " +  //12
                                           "VALUES (@cdkey2,@txtco_id2,@txtmat_id2," +  //13
                                          "@txtmat_type_id,@txtmat_sac_id," +  //2
                                           "@txtmat_group_id,@txtmat_brand_id," +  //3
                                           "@txtmat_unit1_id,@txtmat_unit1_qty," +  //4
                                           "@chmat_unit_status," +  //5
                                           "@txtmat_unit2_id,@txtmat_unit2_qty," +  //6
                                           "@txtmat_unit3_id," +  //5
                                           "@txtmat_unit4_id," +  //5
                                           "@txtmat_unit5_id," +  //5
                                           "@txtmat_detail_group_id,@txtmat_incentive_id," +  //7
                                           "@txtmat_tax_id,@txtmat_credit_charge_id," +  //8
                                           "@txtmat_type_with_acc_id," +  //9
                                            "@txtmat_qty_min,@txtmat_qty_max," +  //10
                                           "@txtmat_qty_per_labor," +  //11
                                           "@txtmat_remark)";   //12

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id2", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_type_id", SqlDbType.NVarChar).Value = this.PANEL101_MAT_TYPE_txtmat_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_sac_id", SqlDbType.NVarChar).Value = this.PANEL102_MAT_SAC_txtmat_sac_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_group_id", SqlDbType.NVarChar).Value = this.PANEL103_MAT_GROUP_txtmat_group_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_id", SqlDbType.NVarChar).Value = this.PANEL104_MAT_BRAND_txtmat_brand_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_unit1_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_unit1_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString()));

                        //if (this.chmat_unit_status.Checked == true)
                        //{
                            //cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = "Y";
                            cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = this.txtchmat_unit_status.Text.Trim();
                        //}
                        //else
                        //{
                            //cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = "N";
                            //cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = "N";
                        //}

                        cmd2.Parameters.Add("@txtmat_unit2_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_unit2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_unit3_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT3_txtmat_unit3_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_unit4_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT4_txtmat_unit4_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_unit5_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT5_txtmat_unit5_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_detail_group_id", SqlDbType.NVarChar).Value = this.txtmat_detail_group_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_incentive_id", SqlDbType.NVarChar).Value = this.txtmat_incentive_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_tax_id", SqlDbType.NVarChar).Value = this.txtmat_tax_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_credit_charge_id", SqlDbType.NVarChar).Value = this.txtmat_credit_charge_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_type_with_acc_id", SqlDbType.NVarChar).Value = this.txtmat_type_with_acc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_qty_min", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_min.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_max", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_max.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_per_labor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_per_labor.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_remark", SqlDbType.NVarChar).Value = this.txtmat_remark.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //3
                        cmd2.CommandText = "INSERT INTO b001mat_04barcode(cdkey,txtco_id,txtmat_id," +  //1
                                          "txtmat_barcode_id) " +  //2
                                           "VALUES (@cdkey3,@txtco_id3,@txtmat_id3," +  //1
                                           "@txtmat_barcode_id)";   //2

                        cmd2.Parameters.Add("@cdkey3", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id3", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id3", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_barcode_id", SqlDbType.NVarChar).Value = this.txtmat_barcode_id.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //4
                        cmd2.CommandText = "INSERT INTO b001mat_06price_sale(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_price_sale1,txtmat_price_sale2,txtmat_price_sale3,txtmat_price_sale4,txtmat_price_sale5," +  //2
                                           "txtmat_price_sale6,txtmat_price_sale7,txtmat_price_sale8,txtmat_price_sale9,txtmat_price_sale10) " +  //3
                                           "VALUES (@cdkey4,@txtco_id4,@txtmat_id4," +  //1
                                           "@txtmat_price_sale1,@txtmat_price_sale2,@txtmat_price_sale3,@txtmat_price_sale4,@txtmat_price_sale5," +  //2
                                           "@txtmat_price_sale6,@txtmat_price_sale7,@txtmat_price_sale8,@txtmat_price_sale9,@txtmat_price_sale10)";   //3

                        cmd2.Parameters.Add("@cdkey4", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id4", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id4", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_price_sale1", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale1.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale2", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale2.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale3", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale3.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale4", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale4.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale5", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale5.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale6", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale6.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale7", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale7.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale8", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale8.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale9", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale9.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale10", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale10.Text.ToString()));
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //5
                        cmd2.CommandText = "INSERT INTO b001mat_10shipment(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_qty_width,txtmat_qty_long,txtmat_qty_high," +  //2
                                           "txtlength_measurement_unit," +  //3
                                           "txtmat_qty_weight,txtlength_weight_unit) " +  //4
                                           "VALUES (@cdkey5,@txtco_id5,@txtmat_id5," +  //1
                                           "@txtmat_qty_width,@txtmat_qty_long,@txtmat_qty_high," +  //2
                                           "@txtlength_measurement_unit," +  //3
                                           "@txtmat_qty_weight,@txtlength_weight_unit)";   //4

                        cmd2.Parameters.Add("@cdkey5", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id5", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id5", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_qty_width", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_width.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_long", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_long.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_high", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_high.Text.ToString()));
                        cmd2.Parameters.Add("@txtlength_measurement_unit", SqlDbType.NVarChar).Value = this.txtlength_measurement_unit.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_qty_weight", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_weight.Text.ToString()));
                        cmd2.Parameters.Add("@txtlength_weight_unit", SqlDbType.NVarChar).Value = this.txtlength_weight_unit.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //6
                        for (int i = 0; i < this.PANEL161_SUP_dataGridView2.Rows.Count; i++)
                        {
                            if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value != null)
                            {
                                //if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value.ToString() =="")
                                //{
                                //    this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value = "0";
                                //}
                                //this.PANEL161_SUP_dataGridView2.Columns[0].Name = "Col_Auto_num";
                                //this.PANEL161_SUP_dataGridView2.Columns[1].Name = "Col_txtsupplier_id";
                                //this.PANEL161_SUP_dataGridView2.Columns[2].Name = "Col_txtsupplier_name";
                                //this.PANEL161_SUP_dataGridView2.Columns[3].Name = "Col_txtmat_price_phurchase";
                                //this.PANEL161_SUP_dataGridView2.Columns[4].Name = "Col_txtmat_discount";
                                //this.PANEL161_SUP_dataGridView2.Columns[5].Name = "Col_txtmat_bonus";
                                //this.PANEL161_SUP_dataGridView2.Columns[6].Name = "Col_txtmat_phurchase_min";
                                //this.PANEL161_SUP_dataGridView2.Columns[7].Name = "Col_txtmat_phurchase_max";
                                //this.PANEL161_SUP_dataGridView2.Columns[8].Name = "Col_txtmat_Leadtime";
                                //this.PANEL161_SUP_dataGridView2.Columns[9].Name = "Col_txtsupplier_remark";

                                cmd2.CommandText = "INSERT INTO b001mat_11supplier(cdkey,txtco_id,txtmat_id," +  //1
                                                   "txtsupplier_id," +  //2
                                                   "txtsupplier_name," +  //3
                                                   "txtmat_price_phurchase," +  //4
                                                   "txtmat_discount," +  //5
                                                   "txtmat_bonus," +  //6
                                                   "txtmat_phurchase_min," +  //7
                                                   "txtmat_phurchase_max," +  //8
                                                   "txtmat_Leadtime," +  //9
                                                   "txtsupplier_remark) " +  //10
                                                   "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + this.txtmat_id.Text.Trim() + "'," +  //1
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value.ToString() + "'," +  //2
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value.ToString() + "'," +  //4
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[4].Value.ToString() + "'," +  //5
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[5].Value.ToString() + "'," +  //6
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[6].Value.ToString() + "'," +  //7
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[7].Value.ToString() + "'," +  //8
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[8].Value.ToString() + "'," +  //9
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[9].Value.ToString() + "')";   //10

                                //==============================

                                cmd2.ExecuteNonQuery();

                            }
                        }

                        //7
                        cmd2.CommandText = "INSERT INTO b001mat_12picture(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_1picture_size,txtmat_1picture," +  //2
                                           "txtmat_2picture_size,txtmat_2picture," +  //3
                                           "txtmat_3picture_size,txtmat_3picture," +  //4
                                           "txtmat_4picture_size,txtmat_4picture) " +  //5
                                           "VALUES (@cdkey7,@txtco_id7,@txtmat_id7," + //1
                                           "@txtmat_1picture_size,@txtmat_1picture," +  //2
                                           "@txtmat_2picture_size,@txtmat_2picture," +  //3
                                           "@txtmat_3picture_size,@txtmat_3picture," +  //4
                                           "@txtmat_4picture_size,@txtmat_4picture)";  //5

                        cmd2.Parameters.Add("@cdkey7", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id7", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id7", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture1.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture1.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size1.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture2.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture2.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size2.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture3.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture3.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size3.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture4.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture4.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size4.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture", imageBt));
                        }

                        //==============================
                        cmd2.ExecuteNonQuery();

                        //8
                        cmd2.CommandText = "INSERT INTO b001mat_13point_phurchase(cdkey,txtco_id,txtmat_id," +  //1
                                          "txtmat_amount_phurchase) " +  //2
                                           "VALUES (@cdkey8,@txtco_id8,@txtmat_id8," +  //1
                                           "@txtmat_amount_phurchase)";   //2

                        cmd2.Parameters.Add("@cdkey8", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id8", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id8", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_amount_phurchase",SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_amount_phurchase.Text.ToString()));
                        //==============================
                        cmd2.ExecuteNonQuery();

                        //=========================================================================================================
                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                    {

                        //1
                        string ACT = "";
                        if (this.check_mat_status.Checked == true)
                        {
                            ACT = "0";
                        }
                        else
                        {
                            ACT = "1";
                        }
                        cmd2.CommandText = "UPDATE b001mat SET " +
                                                                     "txtmat_no = '" + this.txtmat_no.Text.Trim() + "'," +
                                                                     "txtmat_name = '" + this.txtmat_name.Text.Trim() + "'," +
                                                                     "txtmat_name_eng = '" + this.txtmat_name_eng.Text.Trim() + "'," +
                                                                     "txtmat_name_market = '" + this.txtmat_name_market.Text.Trim() + "'," +
                                                                     "txtmat_name_bill = '" + this.txtmat_name_bill.Text.Trim() + "'," +
                                                                     "txtmat_status = '" +ACT.ToString() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();


                        //2
                        string CHM = "";
                        if (this.chmat_unit_status.Checked == true)
                        {
                            CHM = "Y";
                        }
                        else
                        {
                            CHM = "N";
                        }
                        cmd2.CommandText = "UPDATE b001mat_02detail SET " +
                                                                     "txtmat_type_id = '" + this.PANEL101_MAT_TYPE_txtmat_type_id.Text.Trim() + "'," +
                                                                     "txtmat_sac_id = '" + this.PANEL102_MAT_SAC_txtmat_sac_id.Text.Trim() + "'," +
                                                                     "txtmat_group_id = '" + this.PANEL103_MAT_GROUP_txtmat_group_id.Text.Trim() + "'," +
                                                                     "txtmat_brand_id = '" + this.PANEL104_MAT_BRAND_txtmat_brand_id.Text.Trim() + "'," +
                                                                      "txtmat_unit1_id = '" + this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text.Trim() + "'," +
                                                                     "txtmat_unit1_qty = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString())) + "'," +
                                                                     //"chmat_unit_status = '" + CHM.Trim() + "'," +
                                                                     "chmat_unit_status = '" + this.txtchmat_unit_status.Text.Trim() + "'," +
                                                                     "txtmat_unit2_id = '" + this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text.Trim() + "'," +
                                                                     "txtmat_unit2_qty = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +
                                                                     "txtmat_unit3_id = '" + this.PANEL105_MAT_UNIT3_txtmat_unit3_id.Text.Trim() + "'," +
                                                                     "txtmat_unit4_id = '" + this.PANEL105_MAT_UNIT4_txtmat_unit4_id.Text.Trim() + "'," +
                                                                     "txtmat_unit5_id = '" + this.PANEL105_MAT_UNIT5_txtmat_unit5_id.Text.Trim() + "'," +

                                                                    "txtmat_detail_group_id = '" + this.txtmat_detail_group_id.Text.Trim() + "'," +
                                                                     "txtmat_incentive_id = '" + this.txtmat_incentive_id.Text.Trim() + "'," +
                                                                     "txtmat_tax_id = '" + this.txtmat_tax_id.Text.Trim() + "'," +
                                                                     "txtmat_credit_charge_id = '" + this.txtmat_credit_charge_id.Text.Trim() + "'," +
                                                                     "txtmat_type_with_acc_id = '" + this.txtmat_type_with_acc_id.Text.Trim() + "'," +

                                                                     "txtmat_qty_min = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_min.Text.ToString())) + "'," +
                                                                     "txtmat_qty_max = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_max.Text.ToString())) + "'," +
                                                                     "txtmat_qty_per_labor = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_per_labor.Text.ToString())) + "'," +
                                                                     "txtmat_remark = '" + this.txtmat_remark.Text.ToString() + "'" +

                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                        //3
                        cmd2.CommandText = "UPDATE b001mat_04barcode SET " +
                                                                     "txtmat_barcode_id = '" + this.txtmat_barcode_id.Text.ToString() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        //4
                        cmd2.CommandText = "UPDATE b001mat_06price_sale SET " +
                                                                     "txtmat_price_sale1 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale1.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale2 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale2.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale3 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale3.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale4 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale4.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale5 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale5.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale6 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale6.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale7 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale7.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale8 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale8.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale9 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale9.Text.ToString())) + "'," +
                                                                     "txtmat_price_sale10 = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale10.Text.ToString())) + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        //5
                        cmd2.CommandText = "UPDATE b001mat_10shipment SET " +
                                                                     "txtmat_qty_width = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_width.Text.ToString())) + "'," +
                                                                     "txtmat_qty_long = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_long.Text.ToString())) + "'," +
                                                                     "txtmat_qty_high = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_high.Text.ToString())) + "'," +
                                                                     "txtlength_measurement_unit = '" + this.txtlength_measurement_unit.Text.Trim() + "'," +
                                                                     "txtmat_qty_weight = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_weight.Text.ToString())) + "'," +
                                                                     "txtlength_weight_unit = '" + this.txtlength_weight_unit.Text.Trim() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        //6
                        cmd2.CommandText = "DELETE FROM b001mat_11supplier " +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        for (int i = 0; i < this.PANEL161_SUP_dataGridView2.Rows.Count; i++)
                        {
                            if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value != null)
                            {
                                //if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value.ToString() =="")
                                //{
                                //    this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value = "0";
                                //}
                                //this.PANEL161_SUP_dataGridView2.Columns[0].Name = "Col_Auto_num";
                                //this.PANEL161_SUP_dataGridView2.Columns[1].Name = "Col_txtsupplier_id";
                                //this.PANEL161_SUP_dataGridView2.Columns[2].Name = "Col_txtsupplier_name";
                                //this.PANEL161_SUP_dataGridView2.Columns[3].Name = "Col_txtmat_price_phurchase";
                                //this.PANEL161_SUP_dataGridView2.Columns[4].Name = "Col_txtmat_discount";
                                //this.PANEL161_SUP_dataGridView2.Columns[5].Name = "Col_txtmat_bonus";
                                //this.PANEL161_SUP_dataGridView2.Columns[6].Name = "Col_txtmat_phurchase_min";
                                //this.PANEL161_SUP_dataGridView2.Columns[7].Name = "Col_txtmat_phurchase_max";
                                //this.PANEL161_SUP_dataGridView2.Columns[8].Name = "Col_txtmat_Leadtime";
                                //this.PANEL161_SUP_dataGridView2.Columns[9].Name = "Col_txtsupplier_remark";

                                cmd2.CommandText = "INSERT INTO b001mat_11supplier(cdkey,txtco_id,txtmat_id," +  //1
                                                   "txtsupplier_id," +  //2
                                                   "txtsupplier_name," +  //3
                                                   "txtmat_price_phurchase," +  //4
                                                   "txtmat_discount," +  //5
                                                   "txtmat_bonus," +  //6
                                                   "txtmat_phurchase_min," +  //7
                                                   "txtmat_phurchase_max," +  //8
                                                   "txtmat_Leadtime," +  //9
                                                   "txtsupplier_remark) " +  //10
                                                   "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + this.txtmat_id.Text.Trim() + "'," +  //1
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value.ToString() + "'," +  //2
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value.ToString() + "'," +  //4
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[4].Value.ToString() + "'," +  //5
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[5].Value.ToString() + "'," +  //6
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[6].Value.ToString() + "'," +  //7
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[7].Value.ToString() + "'," +  //8
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[8].Value.ToString() + "'," +  //9
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[9].Value.ToString() + "')";   //10

                                //==============================

                                cmd2.ExecuteNonQuery();

                            }
                        }
                        //7
                        cmd2.CommandText = "UPDATE b001mat_13point_phurchase SET " +
                                                                     "txtmat_amount_phurchase = '" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_amount_phurchase.Text.ToString())) + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มรหัสสินค้าใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();

                            Fill_PANEL_FORM1_dataGridView1();
                            this.iblword_status.Text = "เพิ่มรหัสสินค้าใหม่";
                            this.txtmat_id.ReadOnly = false;

                            Run_ID();

                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();


                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_no"].Value = this.txtmat_no.Text.ToString();      //1
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_id"].Value = this.txtmat_id.Text.ToString();      //2
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_name"].Value = this.txtmat_name.Text.ToString();      //3
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_name_eng"].Value = this.txtmat_name_eng.Text.ToString();      //4
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_name_market"].Value = this.txtmat_name_market.Text.ToString();      //5
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_name_bill"].Value = this.txtmat_name_bill.Text.ToString();      //6
                            GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_remark"].Value = this.txtmat_remark.Text.ToString();      //7
                            if (this.check_mat_status.Checked == true)
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_status"].Value = true;      //8
                            }
                            else
                            {
                                GridView1.Rows[selectedRowIndex].Cells["Col_txtmat_status"].Value = false;      //8
                            }
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                            //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                        }

                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtmat_id.Text = "";
                        Clear_Text();



                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void BtnCancel_Doc_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_CANCEL.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            W_ID_Select.LOG_ID = "7";
            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
            TRANS_LOG();

            this.iblword_status.Text = "ยกเลิกเอกสาร";
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
                    if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                    {
                        String myString = W_ID_Select.DATE_FROM_SERVER; // get value from text field
                        DateTime myDateTime = new DateTime();
                        myDateTime = DateTime.ParseExact(myString, "yyyy-MM-dd", UsaCulture);

                        String myString2 = W_ID_Select.TIME_FROM_SERVER; // get value from text field
                        DateTime myDateTime2 = new DateTime();
                        myDateTime2 = DateTime.ParseExact(myString2, "HH:mm:ss", null);

                        string Cancel_ID = W_ID_Select.CDKEY.Trim() + "-" + W_ID_Select.M_USERNAME.Trim() + "-" + myDateTime.ToString("yyyy-MM-dd", UsaCulture) + "-" + myDateTime2.ToString("HH:mm:ss", UsaCulture);

                        cmd2.CommandText = "INSERT INTO b001mat_cancel(cdkey,txtco_id,txtbranch_id," +  //1
                                                                                                            //"txttrans_date," +
                                               "txttrans_date_server,txttrans_time," +  //2
                                               "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                               "txtcomputer_ip,txtcomputer_name," +  //4
                                               "txtform_name,txtform_caption," +  //5
                                                "txtuser_name,txtemp_office_name," +  //6
                                               "txtlog_id,txtlog_name," +  //7
                                              "txtdocument_id,txtversion_id,txtcount,cancel_id) " +  //8
                                               "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                                                                            //"@txttrans_date," +
                                               "@txttrans_date_server,@txttrans_time," +  //2
                                               "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                               "@txtcomputer_ip,@txtcomputer_name," +  //4
                                               "@txtform_name,@txtform_caption," +  //5
                                               "@txtuser_name,@txtemp_office_name," +  //6
                                               "@txtlog_id,@txtlog_name," +  //7
                                               "@txtdocument_id,@txtversion_id,@txtcount,@cancel_id)";   //8

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
                        cmd2.Parameters.Add("@txtversion_id", SqlDbType.NVarChar).Value = W_ID_Select.VERSION_ID.Trim();
                        cmd2.Parameters.Add("@txtcount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 1));
                        cmd2.Parameters.Add("@cancel_id", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================
                        cmd2.ExecuteNonQuery();


                        cmd2.CommandText = "INSERT INTO b001mat_cancel_detail(cdkey,txtco_id," +
                                           "txtmat_id,txtmat_no," +
                                           "txtmat_name,txtmat_name_eng," +
                                           "txtmat_name_market," +
                                           "txtmat_name_bill," +
                                           "txtmat_status," +
                                           "cancel_id) " +
                                           "VALUES (@cdkey2,@txtco_id2," +
                                          "@txtmat_id2,@txtmat_no2," +
                                           "@txtmat_name2,@txtmat_name_eng2," +
                                           "@txtmat_name_market2," +
                                           "@txtmat_name_bill2," +
                                           "@txtmat_status2," +
                                           "@cancel_id2)";

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id2", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_no2", SqlDbType.NVarChar).Value = this.txtmat_no.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name2", SqlDbType.NVarChar).Value = this.txtmat_name.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_eng2", SqlDbType.NVarChar).Value = this.txtmat_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_market2", SqlDbType.NVarChar).Value = this.txtmat_name_market.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_bill2", SqlDbType.NVarChar).Value = this.txtmat_name_bill.Text.ToString();
                        if (this.check_mat_status.Checked == true)
                        {
                            cmd2.Parameters.Add("@txtmat_status2", SqlDbType.NVarChar).Value = "0";
                        }
                        else
                        {
                            cmd2.Parameters.Add("@txtmat_status2", SqlDbType.NVarChar).Value = "1";
                        }

                        cmd2.Parameters.Add("@cancel_id2", SqlDbType.NVarChar).Value = Cancel_ID.ToString();

                        //==============================

                        cmd2.ExecuteNonQuery();

                        //
                        cmd2.CommandText = "DELETE FROM b001mat" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_02detail" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_04barcode" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_06price_sale" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_10shipment" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_11supplier" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_12picture" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001mat_13point_phurchase" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();



                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัสสินค้า  " + this.txtmat_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "ยกเลิกเอกสาร")
                        {
                            W_ID_Select.LOG_ID = "7";
                            W_ID_Select.LOG_NAME = "ยกเลิกเอกสาร";
                            TRANS_LOG();
                        }

                        MessageBox.Show("ยกเลิกเอกสาร เรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtmat_id.Text = "";
                        this.txtmat_name.Text = "";

                        Fill_PANEL_FORM1_dataGridView1();
                        this.iblword_status.Text = "เพิ่มรหัสสินค้าใหม่";
                        Clear_Text();
                        this.txtmat_id.ReadOnly = false;
                        this.btnUp_pic1.Visible = false;
                        this.btnUp_pic2.Visible = false;
                        this.btnUp_pic3.Visible = false;
                        this.btnUp_pic4.Visible = false;

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
            //=============================================================



        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

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
                //C:\KD_ERP\KD_REPORT
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_b001mat.rpt");


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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = this.btnPreview.Text.Trim();

            if (W_ID_Select.M_FORM_PRINT.Trim() == "N")
            {
                MessageBox.Show("ไม่อนุญาต !!", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_07_mat_Print frm2 = new kondate.soft.SETUP_4WH.Home_SETUP_Enter_4WH_07_mat_Print();
            frm2.Show();
            frm2.BringToFront();

            //======================================================

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //txtmat_type ประเภทสินค้า =======================================================================
        private void PANEL101_MAT_TYPE_Fill_mat_type()
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

            PANEL101_MAT_TYPE_Clear_GridView1_mat_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_01mat_type" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_type_id <> '')" +
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
                            var index = PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows.Add();
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_id"].Value = dt2.Rows[j]["txtmat_type_id"].ToString();      //1
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name"].Value = dt2.Rows[j]["txtmat_type_name"].ToString();      //2
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name_eng"].Value = dt2.Rows[j]["txtmat_type_name_eng"].ToString();      //3
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
        private void PANEL101_MAT_TYPE_GridView1_mat_type()
        {
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.ColumnCount = 4;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].Name = "Col_txtmat_type_id";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].Name = "Col_txtmat_type_name";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].Name = "Col_txtmat_type_name_eng";

            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[0].HeaderText = "No";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].HeaderText = "รหัส";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].HeaderText = " ประเภทสินค้า";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].HeaderText = " ประเภทสินค้า Eng";

            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[0].Visible = false;  //"No";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].Visible = true;  //"Col_txtmat_type_id";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].Width = 100;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].ReadOnly = true;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].Visible = true;  //"Col_txtmat_type_name";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].Width = 150;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].ReadOnly = true;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].Visible = true;  //"Col_txtmat_type_name_eng";
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].Width = 150;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].ReadOnly = true;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL101_MAT_TYPE_Clear_GridView1_mat_type()
        {
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows.Clear();
            this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Refresh();
        }
        private void PANEL101_MAT_TYPE_txtmat_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL101_MAT_TYPE.Visible == false)
                {
                    this.PANEL101_MAT_TYPE.Visible = true;
                    this.PANEL101_MAT_TYPE.Location = new Point(116, this.PANEL101_MAT_TYPE_txtmat_type_name.Location.Y + 22);
                    this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Focus();
                }
                else
                {
                    this.PANEL101_MAT_TYPE.Visible = false;
                }
        }
        private void PANEL101_MAT_TYPE_btnmat_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL101_MAT_TYPE.Visible == false)
            {
                this.PANEL101_MAT_TYPE.Visible = true;
                this.PANEL101_MAT_TYPE.BringToFront();
                this.PANEL101_MAT_TYPE.Location = new Point(103, this.PANEL101_MAT_TYPE_txtmat_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL101_MAT_TYPE.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL101_MAT_TYPE.Visible == false)
            {
                this.PANEL101_MAT_TYPE.Visible = true;
            }
            else
            {
                this.PANEL101_MAT_TYPE.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE_dataGridView1_mat_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL101_MAT_TYPE_txtmat_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL101_MAT_TYPE_txtmat_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL101_MAT_TYPE_dataGridView1_mat_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL101_MAT_TYPE_dataGridView1_mat_type.CurrentRow.Index;

                this.PANEL101_MAT_TYPE_txtmat_type_id.Text = PANEL101_MAT_TYPE_dataGridView1_mat_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL101_MAT_TYPE_txtmat_type_name.Text = PANEL101_MAT_TYPE_dataGridView1_mat_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL101_MAT_TYPE_txtmat_type_name.Focus();
                this.PANEL101_MAT_TYPE.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL101_MAT_TYPE_btn_search_Click(object sender, EventArgs e)
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

            PANEL101_MAT_TYPE_Clear_GridView1_mat_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_01mat_type" +
                                   " WHERE (txtmat_type_name LIKE '%" + this.PANEL101_MAT_TYPE_txtsearch.Text + "%')" +
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
                            var index = PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows.Add();
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_id"].Value = dt2.Rows[j]["txtmat_type_id"].ToString();      //1
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name"].Value = dt2.Rows[j]["txtmat_type_name"].ToString();      //2
                            PANEL101_MAT_TYPE_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name_eng"].Value = dt2.Rows[j]["txtmat_type_name_eng"].ToString();      //3
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
        private void PANEL101_MAT_TYPE_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL101_MAT_TYPE_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL101_MAT_TYPE.Height = PANEL101_MAT_TYPE_btnresize_low.Top + e.Y;
                this.PANEL101_MAT_TYPE.Width = PANEL101_MAT_TYPE_btnresize_low.Left + e.X;
            }
        }
        private void PANEL101_MAT_TYPE_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL101_MAT_TYPE_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL101_MAT_TYPE_Fill_mat_type_Edit()
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
                                  " FROM b001_01mat_type" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtmat_type_id = '" + this.PANEL101_MAT_TYPE_txtmat_type_id.Text.Trim() + "')" +
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

                        PANEL101_MAT_TYPE_txtmat_type_name.Text = dt2.Rows[0]["txtmat_type_name"].ToString();      //2

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
        //END txtmat_type ประเภทสินค้า =======================================================================
        //txtmat_type ประเภทสินค้า =======================================================================
        private void PANEL101_MAT_TYPE2_Fill_mat_type()
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

            PANEL101_MAT_TYPE2_Clear_GridView1_mat_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_01mat_type" +
                                     " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtmat_type_id <> '')" +
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
                            var index = PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows.Add();
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_id"].Value = dt2.Rows[j]["txtmat_type_id"].ToString();      //1
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name"].Value = dt2.Rows[j]["txtmat_type_name"].ToString();      //2
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name_eng"].Value = dt2.Rows[j]["txtmat_type_name_eng"].ToString();      //3
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
        private void PANEL101_MAT_TYPE2_GridView1_mat_type()
        {
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnCount = 4;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[0].Name = "Col_Auto_num";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].Name = "Col_txtmat_type_id";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].Name = "Col_txtmat_type_name";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].Name = "Col_txtmat_type_name_eng";

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[0].HeaderText = "No";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].HeaderText = "รหัส";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].HeaderText = " ประเภทสินค้า";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].HeaderText = " ประเภทสินค้า Eng";

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[0].Visible = false;  //"No";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].Visible = true;  //"Col_txtmat_type_id";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].Width = 100;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].ReadOnly = true;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].Visible = true;  //"Col_txtmat_type_name";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].Width = 150;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].ReadOnly = true;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].Visible = true;  //"Col_txtmat_type_name_eng";
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].Width = 150;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].ReadOnly = true;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.EnableHeadersVisualStyles = false;

        }
        private void PANEL101_MAT_TYPE2_Clear_GridView1_mat_type()
        {
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows.Clear();
            this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Refresh();
        }
        private void PANEL101_MAT_TYPE2_txtmat_type_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL101_MAT_TYPE2.Visible == false)
                {
                    this.PANEL101_MAT_TYPE2.Visible = true;
                    this.PANEL101_MAT_TYPE2.Location = new Point(this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.X, this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.Y + 22);
                    this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Focus();
                }
                else
                {
                    this.PANEL101_MAT_TYPE2.Visible = false;
                }
        }
        private void PANEL101_MAT_TYPE2_btnmat_type_Click(object sender, EventArgs e)
        {
            if (this.PANEL101_MAT_TYPE2.Visible == false)
            {
                this.PANEL101_MAT_TYPE2.Visible = true;
                this.PANEL101_MAT_TYPE2.BringToFront();
                this.PANEL101_MAT_TYPE2.Location = new Point(this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.X, this.PANEL101_MAT_TYPE2_txtmat_type_name.Location.Y + 22);
            }
            else
            {
                this.PANEL101_MAT_TYPE2.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL101_MAT_TYPE2.Visible == false)
            {
                this.PANEL101_MAT_TYPE2.Visible = true;
            }
            else
            {
                this.PANEL101_MAT_TYPE2.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE2_dataGridView1_mat_type_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL101_MAT_TYPE2_txtmat_type_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL101_MAT_TYPE2_txtmat_type_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL101_MAT_TYPE2_dataGridView1_mat_type_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL101_MAT_TYPE2_dataGridView1_mat_type.CurrentRow.Index;

                this.PANEL101_MAT_TYPE2_txtmat_type_id.Text = PANEL101_MAT_TYPE2_dataGridView1_mat_type.CurrentRow.Cells[1].Value.ToString();
                this.PANEL101_MAT_TYPE2_txtmat_type_name.Text = PANEL101_MAT_TYPE2_dataGridView1_mat_type.CurrentRow.Cells[2].Value.ToString();
                this.PANEL101_MAT_TYPE2_txtmat_type_name.Focus();
                this.PANEL101_MAT_TYPE2.Visible = false;
            }
        }
        private void PANEL101_MAT_TYPE2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL101_MAT_TYPE2_btn_search_Click(object sender, EventArgs e)
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

            PANEL101_MAT_TYPE2_Clear_GridView1_mat_type();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_01mat_type" +
                                   " WHERE (txtmat_type_name LIKE '%" + this.PANEL101_MAT_TYPE2_txtsearch.Text + "%')" +
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
                            var index = PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows.Add();
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_id"].Value = dt2.Rows[j]["txtmat_type_id"].ToString();      //1
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name"].Value = dt2.Rows[j]["txtmat_type_name"].ToString();      //2
                            PANEL101_MAT_TYPE2_dataGridView1_mat_type.Rows[index].Cells["Col_txtmat_type_name_eng"].Value = dt2.Rows[j]["txtmat_type_name_eng"].ToString();      //3
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
        private void PANEL101_MAT_TYPE2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL101_MAT_TYPE2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL101_MAT_TYPE2.Height = PANEL101_MAT_TYPE2_btnresize_low.Top + e.Y;
                this.PANEL101_MAT_TYPE2.Width = PANEL101_MAT_TYPE2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL101_MAT_TYPE2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL101_MAT_TYPE2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_type ประเภทสินค้า =======================================================================



        //txtmat_sac หมวดสินค้า =======================================================================
        private void PANEL102_MAT_SAC_Fill_mat_sac()
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

            PANEL102_MAT_SAC_Clear_GridView1_mat_sac();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_02mat_sac" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtmat_sac_id <> '')" +
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
                            var index = PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows.Add();
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_id"].Value = dt2.Rows[j]["txtmat_sac_id"].ToString();      //1
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name"].Value = dt2.Rows[j]["txtmat_sac_name"].ToString();      //2
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name_eng"].Value = dt2.Rows[j]["txtmat_sac_name_eng"].ToString();      //3
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
        private void PANEL102_MAT_SAC_GridView1_mat_sac()
        {
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.ColumnCount = 4;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[0].Name = "Col_Auto_num";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].Name = "Col_txtmat_sac_id";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].Name = "Col_txtmat_sac_name";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].Name = "Col_txtmat_sac_name_eng";

            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[0].HeaderText = "No";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].HeaderText = "รหัส";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].HeaderText = " หมวดสินค้า";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].HeaderText = " หมวดสินค้า Eng";

            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[0].Visible = false;  //"No";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].Visible = true;  //"Col_txtmat_sac_id";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].Width = 100;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].ReadOnly = true;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].Visible = true;  //"Col_txtmat_sac_name";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].Width = 150;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].ReadOnly = true;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].Visible = true;  //"Col_txtmat_sac_name_eng";
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].Width = 150;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].ReadOnly = true;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.EnableHeadersVisualStyles = false;

        }
        private void PANEL102_MAT_SAC_Clear_GridView1_mat_sac()
        {
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows.Clear();
            this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Refresh();
        }
        private void PANEL102_MAT_SAC_txtmat_sac_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL102_MAT_SAC.Visible == false)
                {
                    this.PANEL102_MAT_SAC.Visible = true;
                    this.PANEL102_MAT_SAC.BringToFront();
                    this.PANEL102_MAT_SAC.Location = new Point(116, this.PANEL102_MAT_SAC_txtmat_sac_name.Location.Y + 22);
                    this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Focus();
                }
                else
                {
                    this.PANEL102_MAT_SAC.Visible = false;
                }
        }
        private void PANEL102_MAT_SAC_btnmat_sac_Click(object sender, EventArgs e)
        {
            if (this.PANEL102_MAT_SAC.Visible == false)
            {
                this.PANEL102_MAT_SAC.Visible = true;
                this.PANEL102_MAT_SAC.BringToFront();
                this.PANEL102_MAT_SAC.Location = new Point(103, this.PANEL102_MAT_SAC_txtmat_sac_name.Location.Y + 22);
            }
            else
            {
                this.PANEL102_MAT_SAC.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL102_MAT_SAC.Visible == false)
            {
                this.PANEL102_MAT_SAC.Visible = true;
            }
            else
            {
                this.PANEL102_MAT_SAC.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC_dataGridView1_mat_sac_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL102_MAT_SAC_txtmat_sac_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL102_MAT_SAC_txtmat_sac_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL102_MAT_SAC_dataGridView1_mat_sac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL102_MAT_SAC_dataGridView1_mat_sac.CurrentRow.Index;

                this.PANEL102_MAT_SAC_txtmat_sac_id.Text = PANEL102_MAT_SAC_dataGridView1_mat_sac.CurrentRow.Cells[1].Value.ToString();
                this.PANEL102_MAT_SAC_txtmat_sac_name.Text = PANEL102_MAT_SAC_dataGridView1_mat_sac.CurrentRow.Cells[2].Value.ToString();
                this.PANEL102_MAT_SAC_txtmat_sac_name.Focus();
                this.PANEL102_MAT_SAC.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL102_MAT_SAC_btn_search_Click(object sender, EventArgs e)
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

            PANEL102_MAT_SAC_Clear_GridView1_mat_sac();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_02mat_sac" +
                                   " WHERE (txtmat_sac_name LIKE '%" + this.PANEL102_MAT_SAC_txtsearch.Text + "%')" +
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
                            var index = PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows.Add();
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_id"].Value = dt2.Rows[j]["txtmat_sac_id"].ToString();      //1
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name"].Value = dt2.Rows[j]["txtmat_sac_name"].ToString();      //2
                            PANEL102_MAT_SAC_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name_eng"].Value = dt2.Rows[j]["txtmat_sac_name_eng"].ToString();      //3
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
        private void PANEL102_MAT_SAC_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL102_MAT_SAC_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL102_MAT_SAC.Height = PANEL102_MAT_SAC_btnresize_low.Top + e.Y;
                this.PANEL102_MAT_SAC.Width = PANEL102_MAT_SAC_btnresize_low.Left + e.X;
            }
        }
        private void PANEL102_MAT_SAC_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL102_MAT_SAC_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL102_MAT_SAC_Fill_mat_sac_Edit()
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
                                  " FROM b001_02mat_sac" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_sac_id = '" + this.PANEL102_MAT_SAC_txtmat_sac_id.Text.Trim() + "')" +
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

                        PANEL102_MAT_SAC_txtmat_sac_name.Text = dt2.Rows[0]["txtmat_sac_name"].ToString();      //2

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

        //END txtmat_sac หมวดสินค้า =======================================================================
        //txtmat_sac หมวดสินค้า =======================================================================
        private void PANEL102_MAT_SAC2_Fill_mat_sac()
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

            PANEL102_MAT_SAC2_Clear_GridView1_mat_sac();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_02mat_sac" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (txtmat_sac_id <> '')" +
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
                            var index = PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows.Add();
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_id"].Value = dt2.Rows[j]["txtmat_sac_id"].ToString();      //1
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name"].Value = dt2.Rows[j]["txtmat_sac_name"].ToString();      //2
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name_eng"].Value = dt2.Rows[j]["txtmat_sac_name_eng"].ToString();      //3
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
        private void PANEL102_MAT_SAC2_GridView1_mat_sac()
        {
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnCount = 4;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[0].Name = "Col_Auto_num";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].Name = "Col_txtmat_sac_id";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].Name = "Col_txtmat_sac_name";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].Name = "Col_txtmat_sac_name_eng";

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[0].HeaderText = "No";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].HeaderText = "รหัส";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].HeaderText = " หมวดสินค้า";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].HeaderText = " หมวดสินค้า Eng";

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[0].Visible = false;  //"No";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].Visible = true;  //"Col_txtmat_sac_id";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].Width = 100;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].ReadOnly = true;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].Visible = true;  //"Col_txtmat_sac_name";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].Width = 150;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].ReadOnly = true;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].Visible = true;  //"Col_txtmat_sac_name_eng";
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].Width = 150;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].ReadOnly = true;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.EnableHeadersVisualStyles = false;

        }
        private void PANEL102_MAT_SAC2_Clear_GridView1_mat_sac()
        {
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows.Clear();
            this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Refresh();
        }
        private void PANEL102_MAT_SAC2_txtmat_sac_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL102_MAT_SAC2.Visible == false)
                {
                    this.PANEL102_MAT_SAC2.Visible = true;
                    this.PANEL102_MAT_SAC2.Location = new Point(this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.X, this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.Y + 22);
                    this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Focus();
                }
                else
                {
                    this.PANEL102_MAT_SAC2.Visible = false;
                }
        }
        private void PANEL102_MAT_SAC2_btnmat_sac_Click(object sender, EventArgs e)
        {
            if (this.PANEL102_MAT_SAC2.Visible == false)
            {
                this.PANEL102_MAT_SAC2.Visible = true;
                this.PANEL102_MAT_SAC2.BringToFront();
                this.PANEL102_MAT_SAC2.Location = new Point(this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.X, this.PANEL102_MAT_SAC2_txtmat_sac_name.Location.Y + 22);
            }
            else
            {
                this.PANEL102_MAT_SAC2.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL102_MAT_SAC2.Visible == false)
            {
                this.PANEL102_MAT_SAC2.Visible = true;
            }
            else
            {
                this.PANEL102_MAT_SAC2.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC2_dataGridView1_mat_sac_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL102_MAT_SAC2_txtmat_sac_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL102_MAT_SAC2_txtmat_sac_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL102_MAT_SAC2_dataGridView1_mat_sac_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL102_MAT_SAC2_dataGridView1_mat_sac.CurrentRow.Index;

                this.PANEL102_MAT_SAC2_txtmat_sac_id.Text = PANEL102_MAT_SAC2_dataGridView1_mat_sac.CurrentRow.Cells[1].Value.ToString();
                this.PANEL102_MAT_SAC2_txtmat_sac_name.Text = PANEL102_MAT_SAC2_dataGridView1_mat_sac.CurrentRow.Cells[2].Value.ToString();
                this.PANEL102_MAT_SAC2_txtmat_sac_name.Focus();
                this.PANEL102_MAT_SAC2.Visible = false;
            }
        }
        private void PANEL102_MAT_SAC2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL102_MAT_SAC2_btn_search_Click(object sender, EventArgs e)
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

            PANEL102_MAT_SAC2_Clear_GridView1_mat_sac();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_02mat_sac" +
                                   " WHERE (txtmat_sac_name LIKE '%" + this.PANEL102_MAT_SAC2_txtsearch.Text + "%')" +
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
                            var index = PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows.Add();
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_id"].Value = dt2.Rows[j]["txtmat_sac_id"].ToString();      //1
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name"].Value = dt2.Rows[j]["txtmat_sac_name"].ToString();      //2
                            PANEL102_MAT_SAC2_dataGridView1_mat_sac.Rows[index].Cells["Col_txtmat_sac_name_eng"].Value = dt2.Rows[j]["txtmat_sac_name_eng"].ToString();      //3
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
        private void PANEL102_MAT_SAC2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL102_MAT_SAC2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL102_MAT_SAC2.Height = PANEL102_MAT_SAC2_btnresize_low.Top + e.Y;
                this.PANEL102_MAT_SAC2.Width = PANEL102_MAT_SAC2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL102_MAT_SAC2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL102_MAT_SAC2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_sac หมวดสินค้า =======================================================================

        //txtmat_group กลุ่มสินค้า =======================================================================
        private void PANEL103_MAT_GROUP_Fill_mat_group()
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

            PANEL103_MAT_GROUP_Clear_GridView1_mat_group();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_03mat_group" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (txtmat_group_id <> '')" +
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
                            var index = PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows.Add();
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_id"].Value = dt2.Rows[j]["txtmat_group_id"].ToString();      //1
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name"].Value = dt2.Rows[j]["txtmat_group_name"].ToString();      //2
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name_eng"].Value = dt2.Rows[j]["txtmat_group_name_eng"].ToString();      //3
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
        private void PANEL103_MAT_GROUP_GridView1_mat_group()
        {
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.ColumnCount = 4;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[0].Name = "Col_Auto_num";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].Name = "Col_txtmat_group_id";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].Name = "Col_txtmat_group_name";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].Name = "Col_txtmat_group_name_eng";

            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[0].HeaderText = "No";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].HeaderText = "รหัส";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].HeaderText = " กลุ่มสินค้า";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].HeaderText = " กลุ่มสินค้า Eng";

            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[0].Visible = false;  //"No";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].Visible = true;  //"Col_txtmat_group_id";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].Width = 100;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].ReadOnly = true;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].Visible = true;  //"Col_txtmat_group_name";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].Width = 150;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].ReadOnly = true;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].Visible = true;  //"Col_txtmat_group_name_eng";
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].Width = 150;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].ReadOnly = true;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.EnableHeadersVisualStyles = false;

        }
        private void PANEL103_MAT_GROUP_Clear_GridView1_mat_group()
        {
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows.Clear();
            this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Refresh();
        }
        private void PANEL103_MAT_GROUP_txtmat_group_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL103_MAT_GROUP.Visible == false)
                {
                    this.PANEL103_MAT_GROUP.Visible = true;
                    this.PANEL103_MAT_GROUP.BringToFront();
                    this.PANEL103_MAT_GROUP.Location = new Point(116, this.PANEL103_MAT_GROUP_txtmat_group_name.Location.Y + 22);
                    this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Focus();
                }
                else
                {
                    this.PANEL103_MAT_GROUP.Visible = false;
                }
        }
        private void PANEL103_MAT_GROUP_btnmat_group_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_MAT_GROUP.Visible == false)
            {
                this.PANEL103_MAT_GROUP.Visible = true;
                this.PANEL103_MAT_GROUP.BringToFront();
                this.PANEL103_MAT_GROUP.Location = new Point(103, this.PANEL103_MAT_GROUP_txtmat_group_name.Location.Y + 22);
            }
            else
            {
                this.PANEL103_MAT_GROUP.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_MAT_GROUP.Visible == false)
            {
                this.PANEL103_MAT_GROUP.Visible = true;
            }
            else
            {
                this.PANEL103_MAT_GROUP.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP_dataGridView1_mat_group_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL103_MAT_GROUP_txtmat_group_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL103_MAT_GROUP_txtmat_group_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL103_MAT_GROUP_dataGridView1_mat_group_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL103_MAT_GROUP_dataGridView1_mat_group.CurrentRow.Index;

                this.PANEL103_MAT_GROUP_txtmat_group_id.Text = PANEL103_MAT_GROUP_dataGridView1_mat_group.CurrentRow.Cells[1].Value.ToString();
                this.PANEL103_MAT_GROUP_txtmat_group_name.Text = PANEL103_MAT_GROUP_dataGridView1_mat_group.CurrentRow.Cells[2].Value.ToString();
                this.PANEL103_MAT_GROUP_txtmat_group_name.Focus();
                this.PANEL103_MAT_GROUP.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL103_MAT_GROUP_btn_search_Click(object sender, EventArgs e)
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

            PANEL103_MAT_GROUP_Clear_GridView1_mat_group();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_03mat_group" +
                                   " WHERE (txtmat_group_name LIKE '%" + this.PANEL103_MAT_GROUP_txtsearch.Text + "%')" +
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
                            var index = PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows.Add();
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_id"].Value = dt2.Rows[j]["txtmat_group_id"].ToString();      //1
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name"].Value = dt2.Rows[j]["txtmat_group_name"].ToString();      //2
                            PANEL103_MAT_GROUP_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name_eng"].Value = dt2.Rows[j]["txtmat_group_name_eng"].ToString();      //3
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
        private void PANEL103_MAT_GROUP_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL103_MAT_GROUP_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL103_MAT_GROUP.Height = PANEL103_MAT_GROUP_btnresize_low.Top + e.Y;
                this.PANEL103_MAT_GROUP.Width = PANEL103_MAT_GROUP_btnresize_low.Left + e.X;
            }
        }
        private void PANEL103_MAT_GROUP_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL103_MAT_GROUP_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL103_MAT_GROUP_Fill_mat_group_Edit()
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
                                  " FROM b001_03mat_group" +
                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_group_id = '" + this.PANEL103_MAT_GROUP_txtmat_group_id.Text.Trim() + "')" +
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

                        this.PANEL103_MAT_GROUP_txtmat_group_name.Text  = dt2.Rows[0]["txtmat_group_name"].ToString();      //2

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

        //END txtmat_group กลุ่มสินค้า =======================================================================
        //txtmat_group กลุ่มสินค้า =======================================================================
        private void PANEL103_MAT_GROUP2_Fill_mat_group()
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

            PANEL103_MAT_GROUP2_Clear_GridView1_mat_group();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_03mat_group" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                      " AND (txtmat_group_id <> '')" +
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
                            var index = PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows.Add();
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_id"].Value = dt2.Rows[j]["txtmat_group_id"].ToString();      //1
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name"].Value = dt2.Rows[j]["txtmat_group_name"].ToString();      //2
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name_eng"].Value = dt2.Rows[j]["txtmat_group_name_eng"].ToString();      //3
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
        private void PANEL103_MAT_GROUP2_GridView1_mat_group()
        {
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnCount = 4;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[0].Name = "Col_Auto_num";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].Name = "Col_txtmat_group_id";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].Name = "Col_txtmat_group_name";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].Name = "Col_txtmat_group_name_eng";

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[0].HeaderText = "No";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].HeaderText = "รหัส";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].HeaderText = " กลุ่มสินค้า";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].HeaderText = " กลุ่มสินค้า Eng";

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[0].Visible = false;  //"No";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].Visible = true;  //"Col_txtmat_group_id";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].Width = 100;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].ReadOnly = true;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].Visible = true;  //"Col_txtmat_group_name";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].Width = 150;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].ReadOnly = true;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].Visible = true;  //"Col_txtmat_group_name_eng";
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].Width = 150;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].ReadOnly = true;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.EnableHeadersVisualStyles = false;

        }
        private void PANEL103_MAT_GROUP2_Clear_GridView1_mat_group()
        {
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows.Clear();
            this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Refresh();
        }
        private void PANEL103_MAT_GROUP2_txtmat_group_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL103_MAT_GROUP2.Visible == false)
                {
                    this.PANEL103_MAT_GROUP2.Visible = true;
                    this.PANEL103_MAT_GROUP2.Location = new Point(this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.X, this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.Y + 22);
                    this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Focus();
                }
                else
                {
                    this.PANEL103_MAT_GROUP2.Visible = false;
                }
        }
        private void PANEL103_MAT_GROUP2_btnmat_group_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_MAT_GROUP2.Visible == false)
            {
                this.PANEL103_MAT_GROUP2.Visible = true;
                this.PANEL103_MAT_GROUP2.BringToFront();
                this.PANEL103_MAT_GROUP2.Location = new Point(this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.X, this.PANEL103_MAT_GROUP2_txtmat_group_name.Location.Y + 22);
            }
            else
            {
                this.PANEL103_MAT_GROUP2.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL103_MAT_GROUP2.Visible == false)
            {
                this.PANEL103_MAT_GROUP2.Visible = true;
            }
            else
            {
                this.PANEL103_MAT_GROUP2.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP2_dataGridView1_mat_group_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL103_MAT_GROUP2_txtmat_group_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL103_MAT_GROUP2_txtmat_group_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL103_MAT_GROUP2_dataGridView1_mat_group_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL103_MAT_GROUP2_dataGridView1_mat_group.CurrentRow.Index;

                this.PANEL103_MAT_GROUP2_txtmat_group_id.Text = PANEL103_MAT_GROUP2_dataGridView1_mat_group.CurrentRow.Cells[1].Value.ToString();
                this.PANEL103_MAT_GROUP2_txtmat_group_name.Text = PANEL103_MAT_GROUP2_dataGridView1_mat_group.CurrentRow.Cells[2].Value.ToString();
                this.PANEL103_MAT_GROUP2_txtmat_group_name.Focus();
                this.PANEL103_MAT_GROUP2.Visible = false;
            }
        }
        private void PANEL103_MAT_GROUP2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL103_MAT_GROUP2_btn_search_Click(object sender, EventArgs e)
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

            PANEL103_MAT_GROUP2_Clear_GridView1_mat_group();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_03mat_group" +
                                   " WHERE (txtmat_group_name LIKE '%" + this.PANEL103_MAT_GROUP2_txtsearch.Text + "%')" +
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
                            var index = PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows.Add();
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_id"].Value = dt2.Rows[j]["txtmat_group_id"].ToString();      //1
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name"].Value = dt2.Rows[j]["txtmat_group_name"].ToString();      //2
                            PANEL103_MAT_GROUP2_dataGridView1_mat_group.Rows[index].Cells["Col_txtmat_group_name_eng"].Value = dt2.Rows[j]["txtmat_group_name_eng"].ToString();      //3
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

                //===============================
            }
            //================================

        }
        private void PANEL103_MAT_GROUP2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL103_MAT_GROUP2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL103_MAT_GROUP2.Height = PANEL103_MAT_GROUP2_btnresize_low.Top + e.Y;
                this.PANEL103_MAT_GROUP2.Width = PANEL103_MAT_GROUP2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL103_MAT_GROUP2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL103_MAT_GROUP2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_group กลุ่มสินค้า =======================================================================

        //txtmat_brand =======================================================================
        private void PANEL104_MAT_BRAND_Fill_mat_brand()
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

            PANEL104_MAT_BRAND_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04mat_brand" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                        " AND (txtmat_brand_id <> '')" +
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
                            var index = PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND_GridView1_mat_brand()
        {
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.ColumnCount = 4;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[0].Name = "Col_Auto_num";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].Name = "Col_txtmat_brand_id";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].Name = "Col_txtmat_brand_name";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].Name = "Col_txtmat_brand_name_eng";

            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[0].HeaderText = "No";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].HeaderText = "รหัส";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].HeaderText = " กลุ่มสินค้า";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].HeaderText = " กลุ่มสินค้า Eng";

            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[0].Visible = false;  //"No";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].Visible = true;  //"Col_txt mat_brand_id";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].Width = 100;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].ReadOnly = true;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].Visible = true;  //"Col_txt mat_brand_name";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].Width = 150;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].ReadOnly = true;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].Visible = true;  //"Col_txt mat_brand_name_eng";
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].Width = 150;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].ReadOnly = true;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.EnableHeadersVisualStyles = false;

        }
        private void PANEL104_MAT_BRAND_Clear_GridView1_mat_brand()
        {
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows.Clear();
            this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Refresh();
        }
        private void PANEL104_MAT_BRAND_txtmat_brand_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL104_MAT_BRAND.Visible == false)
                {
                    this.PANEL104_MAT_BRAND.Visible = true;
                    this.PANEL104_MAT_BRAND.BringToFront();
                    this.PANEL104_MAT_BRAND.Location = new Point(116, this.PANEL104_MAT_BRAND_txtmat_brand_name.Location.Y + 22);
                    this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Focus();
                }
                else
                {
                    this.PANEL104_MAT_BRAND.Visible = false;
                }
        }
        private void PANEL104_MAT_BRAND_btnmat_brand_Click(object sender, EventArgs e)
        {
            if (this.PANEL104_MAT_BRAND.Visible == false)
            {
                this.PANEL104_MAT_BRAND.Visible = true;
                this.PANEL104_MAT_BRAND.BringToFront();
                this.PANEL104_MAT_BRAND.Location = new Point(103, this.PANEL104_MAT_BRAND_txtmat_brand_name.Location.Y + 22);
            }
            else
            {
                this.PANEL104_MAT_BRAND.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL104_MAT_BRAND.Visible == false)
            {
                this.PANEL104_MAT_BRAND.Visible = true;
            }
            else
            {
                this.PANEL104_MAT_BRAND.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND_dataGridView1_mat_brand_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL104_MAT_BRAND_txtmat_brand_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL104_MAT_BRAND_txtmat_brand_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL104_MAT_BRAND_dataGridView1_mat_brand_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL104_MAT_BRAND_dataGridView1_mat_brand.CurrentRow.Index;

                this.PANEL104_MAT_BRAND_txtmat_brand_id.Text = PANEL104_MAT_BRAND_dataGridView1_mat_brand.CurrentRow.Cells[1].Value.ToString();
                this.PANEL104_MAT_BRAND_txtmat_brand_name.Text = PANEL104_MAT_BRAND_dataGridView1_mat_brand.CurrentRow.Cells[2].Value.ToString();
                this.PANEL104_MAT_BRAND_txtmat_brand_name.Focus();
                this.PANEL104_MAT_BRAND.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
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

            PANEL104_MAT_BRAND_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04mat_brand" +
                                  " WHERE (txtmat_brand_name LIKE '%" + this.PANEL104_MAT_BRAND_txtsearch.Text.ToString() + "%')" +
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
                            var index = PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND_btn_search_Click(object sender, EventArgs e)
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

            PANEL104_MAT_BRAND_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04 mat_brand" +
                                   " WHERE (txt mat_brand_name LIKE '%" + this.PANEL104_MAT_BRAND_txtsearch.Text + "%')" +
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
                            var index = PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_id"].Value = dt2.Rows[j]["txt mat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_name"].Value = dt2.Rows[j]["txt mat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_name_eng"].Value = dt2.Rows[j]["txt mat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL104_MAT_BRAND_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL104_MAT_BRAND.Height = PANEL104_MAT_BRAND_btnresize_low.Top + e.Y;
                this.PANEL104_MAT_BRAND.Width = PANEL104_MAT_BRAND_btnresize_low.Left + e.X;
            }
        }
        private void PANEL104_MAT_BRAND_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL104_MAT_BRAND_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL104_MAT_BRAND_Fill_mat_brand_Edit()
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
                                  " FROM b001_04mat_brand" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (txtmat_brand_id = '" + this.PANEL104_MAT_BRAND_txtmat_brand_id.Text.Trim() + "')" +
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

                            this.PANEL104_MAT_BRAND_txtmat_brand_name.Text = dt2.Rows[0]["txtmat_brand_name"].ToString();      //2

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
        //END txtmat_brand=======================================================================
        //txtmat_brand =======================================================================
        private void PANEL104_MAT_BRAND2_Fill_mat_brand()
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

            PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04mat_brand" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (txtmat_brand_id <> '')" +
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
                            var index = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND2_GridView1_mat_brand()
        {
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnCount = 4;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[0].Name = "Col_Auto_num";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].Name = "Col_txtmat_brand_id";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].Name = "Col_txtmat_brand_name";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].Name = "Col_txtmat_brand_name_eng";

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[0].HeaderText = "No";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].HeaderText = "รหัส";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].HeaderText = " กลุ่มสินค้า";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].HeaderText = " กลุ่มสินค้า Eng";

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[0].Visible = false;  //"No";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].Visible = true;  //"Col_txt mat_brand_id";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].Width = 100;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].ReadOnly = true;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].Visible = true;  //"Col_txt mat_brand_name";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].Width = 150;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].ReadOnly = true;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].Visible = true;  //"Col_txt mat_brand_name_eng";
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].Width = 150;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].ReadOnly = true;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.EnableHeadersVisualStyles = false;

        }
        private void PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand()
        {
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Clear();
            this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Refresh();
        }
        private void PANEL104_MAT_BRAND2_txtmat_brand_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL104_MAT_BRAND2.Visible == false)
                {
                    this.PANEL104_MAT_BRAND2.Visible = true;
                    this.PANEL104_MAT_BRAND2.Location = new Point(this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.X, this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.Y + 22);
                    this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Focus();
                }
                else
                {
                    this.PANEL104_MAT_BRAND2.Visible = false;
                }
        }
        private void PANEL104_MAT_BRAND2_btnmat_brand_Click(object sender, EventArgs e)
        {
            if (this.PANEL104_MAT_BRAND2.Visible == false)
            {
                this.PANEL104_MAT_BRAND2.Visible = true;
                this.PANEL104_MAT_BRAND2.BringToFront();
                this.PANEL104_MAT_BRAND2.Location = new Point(this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.X, this.PANEL104_MAT_BRAND2_txtmat_brand_name.Location.Y + 22);
            }
            else
            {
                this.PANEL104_MAT_BRAND2.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL104_MAT_BRAND2.Visible == false)
            {
                this.PANEL104_MAT_BRAND2.Visible = true;
            }
            else
            {
                this.PANEL104_MAT_BRAND2.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND2_dataGridView1_mat_brand_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL104_MAT_BRAND2_txtmat_brand_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL104_MAT_BRAND2_txtmat_brand_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL104_MAT_BRAND2_dataGridView1_mat_brand_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.CurrentRow.Index;

                this.PANEL104_MAT_BRAND2_txtmat_brand_id.Text = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.CurrentRow.Cells[1].Value.ToString();
                this.PANEL104_MAT_BRAND2_txtmat_brand_name.Text = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.CurrentRow.Cells[2].Value.ToString();
                this.PANEL104_MAT_BRAND2_txtmat_brand_name.Focus();
                this.PANEL104_MAT_BRAND2.Visible = false;
            }
        }
        private void PANEL104_MAT_BRAND2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
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

            PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04mat_brand" +
                                  " WHERE (txtmat_brand_name LIKE '%" + this.PANEL104_MAT_BRAND2_txtsearch.Text.ToString() + "%')" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
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
                            var index = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_id"].Value = dt2.Rows[j]["txtmat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name"].Value = dt2.Rows[j]["txtmat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txtmat_brand_name_eng"].Value = dt2.Rows[j]["txtmat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND2_btn_search_Click(object sender, EventArgs e)
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

            PANEL104_MAT_BRAND2_Clear_GridView1_mat_brand();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_04 mat_brand" +
                                   " WHERE (txt mat_brand_name LIKE '%" + this.PANEL104_MAT_BRAND2_txtsearch.Text + "%')" +
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
                            var index = PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows.Add();
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_id"].Value = dt2.Rows[j]["txt mat_brand_id"].ToString();      //1
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_name"].Value = dt2.Rows[j]["txt mat_brand_name"].ToString();      //2
                            PANEL104_MAT_BRAND2_dataGridView1_mat_brand.Rows[index].Cells["Col_txt mat_brand_name_eng"].Value = dt2.Rows[j]["txt mat_brand_name_eng"].ToString();      //3
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
        private void PANEL104_MAT_BRAND2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL104_MAT_BRAND2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL104_MAT_BRAND2.Height = PANEL104_MAT_BRAND2_btnresize_low.Top + e.Y;
                this.PANEL104_MAT_BRAND2.Width = PANEL104_MAT_BRAND2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL104_MAT_BRAND2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL104_MAT_BRAND2_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtmat_brand=======================================================================


        //txtmat_unit =======================================================================
        private void PANEL105_MAT_UNIT1_Fill_mat_unit()
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

            PANEL105_MAT_UNIT1_Clear_GridView1_mat_unit();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_05mat_unit1" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (txtmat_unit1_id <> '')" +
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
                            var index = PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows.Add();
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit1_id"].Value = dt2.Rows[j]["txtmat_unit1_id"].ToString();      //1
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //2
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit1_name_eng"].Value = dt2.Rows[j]["txtmat_unit1_name_eng"].ToString();      //3
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
        private void PANEL105_MAT_UNIT1_GridView1_mat_unit()
        {
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.ColumnCount = 4;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[0].Name = "Col_Auto_num";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].Name = "Col_txtmat_unit1_id";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].Name = "Col_txtmat_unit1_name";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].Name = "Col_txtmat_unit1_name_eng";

            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[0].HeaderText = "No";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].HeaderText = "รหัส";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].HeaderText = " หน่วยนับสินค้า";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].HeaderText = " หน่วยนับสินค้า Eng";

            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[0].Visible = false;  //"No";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].Visible = true;  //"Col_txt mat_unit1_id";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].Width = 100;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].ReadOnly = true;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].Visible = true;  //"Col_txt mat_unit1_name";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].Width = 150;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].ReadOnly = true;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].Visible = true;  //"Col_txt mat_unit1_name_eng";
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].Width = 150;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].ReadOnly = true;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.EnableHeadersVisualStyles = false;

        }
        private void PANEL105_MAT_UNIT1_Clear_GridView1_mat_unit()
        {
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows.Clear();
            this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Refresh();
        }
        private void PANEL105_MAT_UNIT1_txtmat_unit_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL105_MAT_UNIT1.Visible == false)
                {
                    this.PANEL105_MAT_UNIT1.Visible = true;
                    this.PANEL105_MAT_UNIT1.BringToFront();
                    this.PANEL105_MAT_UNIT1.Location = new Point(116, this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Location.Y + 22);
                    this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Focus();
                }
                else
                {
                    this.PANEL105_MAT_UNIT1.Visible = false;
                }
        }
        private void PANEL105_MAT_UNIT1_btnmat_unit_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT1.Visible == false)
            {
                this.PANEL105_MAT_UNIT1.Visible = true;
                this.PANEL105_MAT_UNIT1.BringToFront();
                this.PANEL105_MAT_UNIT1.Location = new Point(103, this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Location.Y + 22);
            }
            else
            {
                this.PANEL105_MAT_UNIT1.Visible = false;
            }
        }
        private void PANEL105_MAT_UNIT1_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT1.Visible == false)
            {
                this.PANEL105_MAT_UNIT1.Visible = true;
            }
            else
            {
                this.PANEL105_MAT_UNIT1.Visible = false;
            }
        }
        private void PANEL105_MAT_UNIT1_dataGridView1_mat_unit_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Text = row.Cells[2].Value.ToString();
                }
            }
        }
        private void PANEL105_MAT_UNIT1_dataGridView1_mat_unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL105_MAT_UNIT1_dataGridView1_mat_unit.CurrentRow.Index;

                this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text = PANEL105_MAT_UNIT1_dataGridView1_mat_unit.CurrentRow.Cells[1].Value.ToString();
                this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Text = PANEL105_MAT_UNIT1_dataGridView1_mat_unit.CurrentRow.Cells[2].Value.ToString();
                this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Focus();
                this.PANEL105_MAT_UNIT1.Visible = false;
            }
        }
        private void PANEL105_MAT_UNIT1_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL105_MAT_UNIT1_btn_search_Click(object sender, EventArgs e)
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

            PANEL105_MAT_UNIT1_Clear_GridView1_mat_unit();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_05mat_unit1" +
                                   " WHERE (txtmat_unit1_name LIKE '%" + this.PANEL105_MAT_UNIT1_txtsearch.Text + "%')" +
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
                            var index = PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows.Add();
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit1_id"].Value = dt2.Rows[j]["txtmat_unit1_id"].ToString();      //1
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //2
                            PANEL105_MAT_UNIT1_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit1_name_eng"].Value = dt2.Rows[j]["txtmat_unit1_name_eng"].ToString();      //3
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
        private void PANEL105_MAT_UNIT1_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL105_MAT_UNIT1_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL105_MAT_UNIT1.Height = PANEL105_MAT_UNIT1_btnresize_low.Top + e.Y;
                this.PANEL105_MAT_UNIT1.Width = PANEL105_MAT_UNIT1_btnresize_low.Left + e.X;
            }
        }
        private void PANEL105_MAT_UNIT1_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL105_MAT_UNIT1_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL105_MAT_UNIT1_Fill_mat_unit_Edit()
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
                                  " FROM b001_05mat_unit1" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_unit1_id = '" + this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text.Trim() + "')" +
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
                        this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Text = dt2.Rows[0]["txtmat_unit1_name"].ToString();      //2

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
        //END txtmat_unit=======================================================================


        //txtmat_unit 2=======================================================================
        private void PANEL105_MAT_UNIT2_Fill_mat_unit()
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

            PANEL105_MAT_UNIT2_Clear_GridView1_mat_unit();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_05mat_unit2" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (txtmat_unit2_id <> '')" +
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
                            var index = PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows.Add();
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit2_id"].Value = dt2.Rows[j]["txtmat_unit2_id"].ToString();      //1
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //2
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit2_name_eng"].Value = dt2.Rows[j]["txtmat_unit2_name_eng"].ToString();      //3
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
        private void PANEL105_MAT_UNIT2_GridView1_mat_unit()
        {
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.ColumnCount = 4;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[0].Name = "Col_Auto_num";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].Name = "Col_txtmat_unit2_id";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].Name = "Col_txtmat_unit2_name";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].Name = "Col_txtmat_unit2_name_eng";

            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[0].HeaderText = "No";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].HeaderText = "รหัส";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].HeaderText = " หน่วยนับสินค้า";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].HeaderText = " หน่วยนับสินค้า Eng";

            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[0].Visible = false;  //"No";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].Visible = true;  //"Col_txt mat_unit2_id";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].Width = 100;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].ReadOnly = true;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].Visible = true;  //"Col_txt mat_unit2_name";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].Width = 150;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].ReadOnly = true;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].Visible = true;  //"Col_txt mat_unit2_name_eng";
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].Width = 150;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].ReadOnly = true;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.DefaultCellStyle.Font = new Font("Tahoma", 8F);

            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.EnableHeadersVisualStyles = false;

        }
        private void PANEL105_MAT_UNIT2_Clear_GridView1_mat_unit()
        {
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows.Clear();
            this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Refresh();
        }
        private void PANEL105_MAT_UNIT2_txtmat_unit_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL105_MAT_UNIT2.Visible == false)
                {
                    this.PANEL105_MAT_UNIT2.Visible = true;
                    this.PANEL105_MAT_UNIT2.BringToFront();
                    this.PANEL105_MAT_UNIT2.Location = new Point(116, this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Location.Y + 22);
                    this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Focus();
                }
                else
                {
                    this.PANEL105_MAT_UNIT2.Visible = false;
                }
        }
        private void PANEL105_MAT_UNIT2_btnmat_unit_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT2.Visible == false)
            {
                this.PANEL105_MAT_UNIT2.Visible = true;
                this.PANEL105_MAT_UNIT2.BringToFront();
                this.PANEL105_MAT_UNIT2.Location = new Point(103, this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Location.Y + 22);
                SL = "2";
            }
            else
            {
                this.PANEL105_MAT_UNIT2.Visible = false;
            }
        }
        private void PANEL105_MAT_UNIT2_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT2.Visible == false)
            {
                this.PANEL105_MAT_UNIT2.Visible = true;
            }
            else
            {
                this.PANEL105_MAT_UNIT2.Visible = false;
            }
        }
        private void PANEL105_MAT_UNIT2_dataGridView1_mat_unit_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    if (SL == "2")
                    {
                        this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text = row.Cells[1].Value.ToString();
                        this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Text = row.Cells[2].Value.ToString();
                    }
                    if (SL == "3")
                    {
                        this.PANEL105_MAT_UNIT3_txtmat_unit3_id.Text = row.Cells[1].Value.ToString();
                        this.PANEL105_MAT_UNIT3_txtmat_unit3_name.Text = row.Cells[2].Value.ToString();
                    }
                    if (SL == "4")
                    {
                        this.PANEL105_MAT_UNIT4_txtmat_unit4_id.Text = row.Cells[1].Value.ToString();
                        this.PANEL105_MAT_UNIT4_txtmat_unit4_name.Text = row.Cells[2].Value.ToString();
                    }
                    if (SL == "5")
                    {
                        this.PANEL105_MAT_UNIT5_txtmat_unit5_id.Text = row.Cells[1].Value.ToString();
                        this.PANEL105_MAT_UNIT5_txtmat_unit5_name.Text = row.Cells[2].Value.ToString();
                    }
                }
            }
        }
        private void PANEL105_MAT_UNIT2_dataGridView1_mat_unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL105_MAT_UNIT2_dataGridView1_mat_unit.CurrentRow.Index;

                this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text = PANEL105_MAT_UNIT2_dataGridView1_mat_unit.CurrentRow.Cells[1].Value.ToString();
                this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Text = PANEL105_MAT_UNIT2_dataGridView1_mat_unit.CurrentRow.Cells[2].Value.ToString();
                this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Focus();
                this.PANEL105_MAT_UNIT2.Visible = false;
            }
        }
        private void PANEL105_MAT_UNIT2_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL105_MAT_UNIT2_btn_search_Click(object sender, EventArgs e)
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

            PANEL105_MAT_UNIT2_Clear_GridView1_mat_unit();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                  " FROM b001_05mat_unit2" +
                                   " WHERE (txtmat_unit2_name LIKE '%" + this.PANEL105_MAT_UNIT2_txtsearch.Text + "%')" +
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
                            var index = PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows.Add();
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit2_id"].Value = dt2.Rows[j]["txtmat_unit2_id"].ToString();      //1
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //2
                            PANEL105_MAT_UNIT2_dataGridView1_mat_unit.Rows[index].Cells["Col_txtmat_unit2_name_eng"].Value = dt2.Rows[j]["txtmat_unit2_name_eng"].ToString();      //3
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
        private void PANEL105_MAT_UNIT2_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL105_MAT_UNIT2_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL105_MAT_UNIT2.Height = PANEL105_MAT_UNIT2_btnresize_low.Top + e.Y;
                this.PANEL105_MAT_UNIT2.Width = PANEL105_MAT_UNIT2_btnresize_low.Left + e.X;
            }
        }
        private void PANEL105_MAT_UNIT2_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL105_MAT_UNIT2_btnnew_Click(object sender, EventArgs e)
        {

        }
        private void PANEL105_MAT_UNIT2_Fill_mat_unit_Edit()
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
                                  " FROM b001_05mat_unit2" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_unit2_id = '" + this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text.Trim() + "')" +
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
                             this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();      //2

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
        private void PANEL105_MAT_UNIT3_Fill_mat_unit_Edit()
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
                                  " FROM b001_05mat_unit3" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_unit3_id = '" + this.PANEL105_MAT_UNIT3_txtmat_unit3_id.Text.Trim() + "')" +
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
                        this.PANEL105_MAT_UNIT3_txtmat_unit3_name.Text = dt2.Rows[0]["txtmat_unit3_name"].ToString();      //2

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
        private void PANEL105_MAT_UNIT4_Fill_mat_unit_Edit()
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
                                  " FROM b001_05mat_unit4" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_unit4_id = '" + this.PANEL105_MAT_UNIT4_txtmat_unit4_id.Text.Trim() + "')" +
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
                        this.PANEL105_MAT_UNIT4_txtmat_unit4_name.Text = dt2.Rows[0]["txtmat_unit4_name"].ToString();      //2

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
        private void PANEL105_MAT_UNIT5_Fill_mat_unit_Edit()
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
                                  " FROM b001_05mat_unit5" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtmat_unit5_id = '" + this.PANEL105_MAT_UNIT5_txtmat_unit5_id.Text.Trim() + "')" +
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
                        this.PANEL105_MAT_UNIT5_txtmat_unit5_name.Text = dt2.Rows[0]["txtmat_unit5_name"].ToString();      //2

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

        //END txtmat_unit 2=======================================================================

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
            this.PANEL161_SUP_dataGridView1.Columns[1].Visible = true;  //"Col_txtsupplier_no";
            this.PANEL161_SUP_dataGridView1.Columns[1].Width = 100;
            this.PANEL161_SUP_dataGridView1.Columns[1].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[2].Visible = true;  //"Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView1.Columns[2].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[2].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[3].Visible = true;  //"Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView1.Columns[3].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[3].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[4].Visible = false;  //"Col_txtsupplier_name_eng";
            this.PANEL161_SUP_dataGridView1.Columns[4].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[4].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[5].Visible = true;  //"Col_txtcontact_person";
            this.PANEL161_SUP_dataGridView1.Columns[5].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[5].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[6].Visible = false;  //"Col_txtcontact_person_tel";
            this.PANEL161_SUP_dataGridView1.Columns[6].Width = 150;
            this.PANEL161_SUP_dataGridView1.Columns[6].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView1.Columns[7].Visible = true;  //"Col_txtremark";
            this.PANEL161_SUP_dataGridView1.Columns[7].Width = 100;
            this.PANEL161_SUP_dataGridView1.Columns[7].ReadOnly = true;
            this.PANEL161_SUP_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
                this.PANEL161_SUP.Location = new Point(this.PANEL161_SUP_txtsupplier_name.Location.X+21, this.PANEL161_SUP_txtsupplier_name.Location.Y + 159);
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
        private void PANEL161_SUP_btnnew_Click(object sender, EventArgs e)
        {

        }


        private void Fill_PANEL161_SUP_Gridview2()
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

            PANEL161_SUP_Clear_GridView2();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL161_SUP_dataGridView2.Columns[0].Name = "Col_Auto_num";
                //this.PANEL161_SUP_dataGridView2.Columns[1].Name = "Col_txtsupplier_id";
                //this.PANEL161_SUP_dataGridView2.Columns[2].Name = "Col_txtsupplier_name";
                //this.PANEL161_SUP_dataGridView2.Columns[3].Name = "Col_txtmat_price_phurchase";
                //this.PANEL161_SUP_dataGridView2.Columns[4].Name = "Col_txtmat_discount";
                //this.PANEL161_SUP_dataGridView2.Columns[5].Name = "Col_txtmat_bonus";
                //this.PANEL161_SUP_dataGridView2.Columns[6].Name = "Col_txtmat_phurchase_min";
                //this.PANEL161_SUP_dataGridView2.Columns[7].Name = "Col_txtmat_phurchase_max";
                //this.PANEL161_SUP_dataGridView2.Columns[8].Name = "Col_txtmat_Leadtime";
                //this.PANEL161_SUP_dataGridView2.Columns[9].Name = "Col_txtsupplier_remark";

                cmd2.CommandText = "SELECT *" +
                                    " FROM b001mat_11supplier" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')" +
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
                            var index = PANEL161_SUP_dataGridView2.Rows.Add();
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtsupplier_id"].Value = dt2.Rows[j]["txtsupplier_id"].ToString();      //1
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtsupplier_name"].Value = dt2.Rows[j]["txtsupplier_name"].ToString();      //2
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_price_phurchase"].Value = dt2.Rows[j]["txtmat_price_phurchase"].ToString();      //3
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_discount"].Value = dt2.Rows[j]["txtmat_discount"].ToString();      //4
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_bonus"].Value = dt2.Rows[j]["txtmat_bonus"].ToString();      //5
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_phurchase_min"].Value = dt2.Rows[j]["txtmat_phurchase_min"].ToString();      //6
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_phurchase_max"].Value = dt2.Rows[j]["txtmat_phurchase_max"].ToString();      //7
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_Leadtime"].Value = dt2.Rows[j]["txtmat_Leadtime"].ToString();      //8
                            PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtsupplier_remark"].Value = dt2.Rows[j]["txtsupplier_remark"].ToString();      //9
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

        }
        private void PANEL161_SUP_btnAdd_Gridview2_Click(object sender, EventArgs e)
        {
            if (this.PANEL161_SUP_txtsupplier_id.Text == "")
            {
                return;
            }
            //selectedRowIndex
            string A = "";
            string B = "";
            string MATCHY = "";

                     A = this.PANEL161_SUP_txtsupplier_id.Text.Trim();
                     B = this.PANEL161_SUP_txtsupplier_id2.Text.Trim();
            if (B.Trim() == A.Trim())
                    {
                        MATCHY = "Y";
                    }
                    else
                    {
                        MATCHY = "N";
                    }

           //========================================================
            if (MATCHY.Trim() == "Y")
            {
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtsupplier_id"].Value = this.PANEL161_SUP_txtsupplier_id.Text.ToString();      //1
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtsupplier_name"].Value = this.PANEL161_SUP_txtsupplier_name.Text.ToString();      //2
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtmat_price_phurchase"].Value = this.txtmat_price_phurchase.Text.ToString();      //3
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtmat_discount"].Value = this.txtmat_discount.Text.ToString();      //4
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtmat_bonus"].Value = this.txtmat_bonus.Text.ToString();      //5
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtmat_phurchase_min"].Value = this.txtmat_phurchase_min.Text.ToString();      //6
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtmat_phurchase_max"].Value = this.txtmat_phurchase_max.Text.ToString();      //7
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtmat_Leadtime"].Value = this.txtmat_Leadtime.Text.ToString();      //8
                PANEL161_SUP_dataGridView2.Rows[selectedRowIndex].Cells["Col_txtsupplier_remark"].Value = this.txtsupplier_remark.Text.ToString();      //9
            }
            else if (MATCHY.Trim() == "N")
            {
                for (int i = 0; i < this.PANEL161_SUP_dataGridView2.Rows.Count; i++)
                {
                    if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value != null)
                    {
                        if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value.ToString() == this.PANEL161_SUP_txtsupplier_id.Text.ToString())
                        {
                            MessageBox.Show("รหัส Supplier ซ้ำ !! ");
                            return;
                        }
                        var index = PANEL161_SUP_dataGridView2.Rows.Add();
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtsupplier_id"].Value = this.PANEL161_SUP_txtsupplier_id.Text.ToString();      //1
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtsupplier_name"].Value = this.PANEL161_SUP_txtsupplier_name.Text.ToString();      //2
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_price_phurchase"].Value = this.txtmat_price_phurchase.Text.ToString();      //3
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_discount"].Value = this.txtmat_discount.Text.ToString();      //4
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_bonus"].Value = this.txtmat_bonus.Text.ToString();      //5
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_phurchase_min"].Value = this.txtmat_phurchase_min.Text.ToString();      //6
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_phurchase_max"].Value = this.txtmat_phurchase_max.Text.ToString();      //7
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtmat_Leadtime"].Value = this.txtmat_Leadtime.Text.ToString();      //8
                        PANEL161_SUP_dataGridView2.Rows[index].Cells["Col_txtsupplier_remark"].Value = this.txtsupplier_remark.Text.ToString();      //9
                        return;
                    }
                }
                var index2 = PANEL161_SUP_dataGridView2.Rows.Add();
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_Auto_num"].Value = ""; //0
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtsupplier_id"].Value = this.PANEL161_SUP_txtsupplier_id.Text.ToString();      //1
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtsupplier_name"].Value = this.PANEL161_SUP_txtsupplier_name.Text.ToString();      //2
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtmat_price_phurchase"].Value = this.txtmat_price_phurchase.Text.ToString();      //3
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtmat_discount"].Value = this.txtmat_discount.Text.ToString();      //4
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtmat_bonus"].Value = this.txtmat_bonus.Text.ToString();      //5
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtmat_phurchase_min"].Value = this.txtmat_phurchase_min.Text.ToString();      //6
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtmat_phurchase_max"].Value = this.txtmat_phurchase_max.Text.ToString();      //7
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtmat_Leadtime"].Value = this.txtmat_Leadtime.Text.ToString();      //8
                PANEL161_SUP_dataGridView2.Rows[index2].Cells["Col_txtsupplier_remark"].Value = this.txtsupplier_remark.Text.ToString();      //9

            }
            //==============================================
            Clear_Txt_Sup();

        }
        private void PANEL161_SUP_GridView2_supplier()
        {
            this.PANEL161_SUP_dataGridView2.ColumnCount = 10;
            this.PANEL161_SUP_dataGridView2.Columns[0].Name = "Col_Auto_num";
            this.PANEL161_SUP_dataGridView2.Columns[1].Name = "Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView2.Columns[2].Name = "Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView2.Columns[3].Name = "Col_txtmat_price_phurchase";
            this.PANEL161_SUP_dataGridView2.Columns[4].Name = "Col_txtmat_discount";
            this.PANEL161_SUP_dataGridView2.Columns[5].Name = "Col_txtmat_bonus";
            this.PANEL161_SUP_dataGridView2.Columns[6].Name = "Col_txtmat_phurchase_min";
            this.PANEL161_SUP_dataGridView2.Columns[7].Name = "Col_txtmat_phurchase_max";
            this.PANEL161_SUP_dataGridView2.Columns[8].Name = "Col_txtmat_Leadtime";
            this.PANEL161_SUP_dataGridView2.Columns[9].Name = "Col_txtsupplier_remark";

            this.PANEL161_SUP_dataGridView2.Columns[0].HeaderText = "No";
            this.PANEL161_SUP_dataGridView2.Columns[1].HeaderText = "รหัสผู้จำหน่าย";
            this.PANEL161_SUP_dataGridView2.Columns[2].HeaderText = " ชื่อ Supplier";
            this.PANEL161_SUP_dataGridView2.Columns[3].HeaderText = " ราคาซื้อ";
            this.PANEL161_SUP_dataGridView2.Columns[4].HeaderText = " ส่วนลด";
            this.PANEL161_SUP_dataGridView2.Columns[5].HeaderText = " แถม";
            this.PANEL161_SUP_dataGridView2.Columns[6].HeaderText = "จำนวนซื้อขั้นต่ำ";
            this.PANEL161_SUP_dataGridView2.Columns[7].HeaderText = " จำนวนซื้อสูงสุด";
            this.PANEL161_SUP_dataGridView2.Columns[8].HeaderText = " กำหนดส่ง";
            this.PANEL161_SUP_dataGridView2.Columns[9].HeaderText = " หมายเหตุ";

            this.PANEL161_SUP_dataGridView2.Columns[0].Visible = false;  //"No";
            this.PANEL161_SUP_dataGridView2.Columns[1].Visible = true;  //"Col_txtsupplier_id";
            this.PANEL161_SUP_dataGridView2.Columns[1].Width = 100;
            this.PANEL161_SUP_dataGridView2.Columns[1].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView2.Columns[2].Visible = true;  //"Col_txtsupplier_name";
            this.PANEL161_SUP_dataGridView2.Columns[2].Width = 150;
            this.PANEL161_SUP_dataGridView2.Columns[2].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL161_SUP_dataGridView2.Columns[3].Visible = true;  //"Col_txtmat_price_phurchase";
            this.PANEL161_SUP_dataGridView2.Columns[3].Width = 50;
            this.PANEL161_SUP_dataGridView2.Columns[3].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL161_SUP_dataGridView2.Columns[4].Visible = false;  //"Col_txtmat_discount";
            this.PANEL161_SUP_dataGridView2.Columns[4].Width = 50;
            this.PANEL161_SUP_dataGridView2.Columns[4].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL161_SUP_dataGridView2.Columns[5].Visible = true;  //"Col_txtmat_bonus";
            this.PANEL161_SUP_dataGridView2.Columns[5].Width = 50;
            this.PANEL161_SUP_dataGridView2.Columns[5].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL161_SUP_dataGridView2.Columns[6].Visible = false;  //"Col_txtmat_phurchase_min";
            this.PANEL161_SUP_dataGridView2.Columns[6].Width = 50;
            this.PANEL161_SUP_dataGridView2.Columns[6].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL161_SUP_dataGridView2.Columns[7].Visible = true;  //"Col_txtmat_phurchase_max";
            this.PANEL161_SUP_dataGridView2.Columns[7].Width = 50;
            this.PANEL161_SUP_dataGridView2.Columns[7].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL161_SUP_dataGridView2.Columns[8].Visible = true;  //"Col_txtmat_Leadtime";
            this.PANEL161_SUP_dataGridView2.Columns[8].Width = 50;
            this.PANEL161_SUP_dataGridView2.Columns[8].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL161_SUP_dataGridView2.Columns[9].Visible = true;  //"Col_txtsupplier_remark";
            this.PANEL161_SUP_dataGridView2.Columns[9].Width = 150;
            this.PANEL161_SUP_dataGridView2.Columns[9].ReadOnly = true;
            this.PANEL161_SUP_dataGridView2.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL161_SUP_dataGridView2.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            this.PANEL161_SUP_dataGridView2.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL161_SUP_dataGridView2.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL161_SUP_dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL161_SUP_dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL161_SUP_dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL161_SUP_dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL161_SUP_dataGridView2.EnableHeadersVisualStyles = false;

            //DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            //dgvCmb.ValueType = typeof(bool);
            //dgvCmb.Name = "Col_Chk";
            //dgvCmb.HeaderText = "สถานะ";
            //dgvCmb.ReadOnly = true;
            //dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //PANEL161_SUP_dataGridView2.Columns.Add(dgvCmb);

        }
        private void PANEL161_SUP_Clear_GridView2()
        {
            this.PANEL161_SUP_dataGridView2.Rows.Clear();
            this.PANEL161_SUP_dataGridView2.Refresh();
        }
        private void PANEL161_SUP_dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Clear_Txt_Sup();

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL161_SUP_dataGridView2.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    //this.PANEL161_SUP_dataGridView2.Columns[1].Name = "Col_txtsupplier_id";
                    //this.PANEL161_SUP_dataGridView2.Columns[2].Name = "Col_txtsupplier_name";
                    //this.PANEL161_SUP_dataGridView2.Columns[3].Name = "Col_txtmat_price_phurchase";
                    //this.PANEL161_SUP_dataGridView2.Columns[4].Name = "Col_txtmat_discount";
                    //this.PANEL161_SUP_dataGridView2.Columns[5].Name = "Col_txtmat_bonus";
                    //this.PANEL161_SUP_dataGridView2.Columns[6].Name = "Col_txtmat_phurchase_min";
                    //this.PANEL161_SUP_dataGridView2.Columns[7].Name = "Col_txtmat_phurchase_max";
                    //this.PANEL161_SUP_dataGridView2.Columns[8].Name = "Col_txtmat_Leadtime";
                    //this.PANEL161_SUP_dataGridView2.Columns[9].Name = "Col_txtsupplier_remark";

                    this.PANEL161_SUP_txtsupplier_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL161_SUP_txtsupplier_id2.Text = row.Cells[1].Value.ToString();
                    this.PANEL161_SUP_txtsupplier_name.Text = row.Cells[2].Value.ToString();
                    this.txtmat_price_phurchase.Text = row.Cells[3].Value.ToString();
                    this.txtmat_discount.Text = row.Cells[4].Value.ToString();
                    this.txtmat_bonus.Text = row.Cells[5].Value.ToString();
                    this.txtmat_phurchase_min.Text = row.Cells[6].Value.ToString();
                    this.txtmat_phurchase_max.Text = row.Cells[7].Value.ToString();
                    this.txtmat_Leadtime.Text = row.Cells[8].Value.ToString();
                    this.txtsupplier_remark.Text = row.Cells[9].Value.ToString();

                    //================================
                    this.PANEL161_SUP_btnRemove_Gridview2.Visible = true;


                }
            }
        }
        //===============
        DataTable table = new DataTable();
        int selectedRowIndex;
        private void PANEL161_SUP_dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
            this.PANEL161_SUP_btnRemove_Gridview2.Visible = true;
        }
        private void PANEL161_SUP_btnRemove_Gridview2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการ ลบรายการแถว ที่คลิ๊ก ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                //DataGridViewRow row = new DataGridViewRow();
                //row = this.PANEL161_SUP_dataGridView2.Rows[selectedRowIndex];
                this.PANEL161_SUP_dataGridView2.Rows.RemoveAt(selectedRowIndex);
                Clear_Txt_Sup();
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
        private void Clear_Txt_Sup()
        {

            this.PANEL161_SUP_txtsupplier_name.Text = "";
            this.PANEL161_SUP_txtsupplier_id.Text = "";
            this.txtmat_price_phurchase.Text = ".00";
            this.txtmat_discount.Text = ".00";
            this.txtmat_phurchase_min.Text = ".00";
            this.txtmat_bonus.Text = ".00";
            this.txtmat_phurchase_max.Text = ".00";
            this.txtmat_Leadtime.Text = "";
            this.txtsupplier_remark.Text = "";

        }
        //END txtsupplier Supplier  =======================================================================


        private void Fill_Cbomat_detail_group_name()
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
                                  " FROM b001_06mat_detail_group" +
                                  " ORDER BY txtmat_detail_group_no";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtmat_detail_group_name = dr.GetString(2);
                        this.Cbomat_detail_group_name.Items.Add(txtmat_detail_group_name);
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
        private void Fill_Cbomat_detail_group_name2()
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
                                  " FROM b001_06mat_detail_group" +
                                  " WHERE (txtmat_detail_group_name = '" + this.Cbomat_detail_group_name.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtmat_detail_group_id.Text = dt.Rows[0]["txtmat_detail_group_id"].ToString();
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
        private void Cbomat_detail_group_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Cbomat_detail_group_name2();
        }
        private void Fill_Cbomat_detail_group_name_Edit()
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
                                  " FROM b001_06mat_detail_group" +
                                  " WHERE (txtmat_detail_group_id = '" + this.txtmat_detail_group_id.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.Cbomat_detail_group_name.Text = dt.Rows[0]["txtmat_detail_group_name"].ToString();
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

        private void Fill_Cbomat_incentive_name()
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
                                  " FROM b001_07mat_incentive" +
                                  " ORDER BY txtmat_incentive_no";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtmat_incentive_name = dr.GetString(2);
                        this.Cbomat_incentive_name.Items.Add(txtmat_incentive_name);
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
        private void Fill_Cbomat_incentive_name2()
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
                                  " FROM b001_07mat_incentive" +
                                  " WHERE (txtmat_incentive_name = '" + this.Cbomat_incentive_name.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtmat_incentive_id.Text = dt.Rows[0]["txtmat_incentive_id"].ToString();
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
        private void Cbomat_incentive_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Cbomat_incentive_name2();
        }
        private void Fill_Cbomat_incentive_name_Edit()
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
                                  " FROM b001_07mat_incentive" +
                                  " WHERE (txtmat_incentive_id = '" + this.txtmat_incentive_id.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.Cbomat_incentive_name.Text = dt.Rows[0]["txtmat_incentive_name"].ToString();
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

        private void Fill_Cbomat_tax_name()
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
                                  " FROM b001_10mat_tax" +
                                  " ORDER BY txtmat_tax_no";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtmat_tax_name = dr.GetString(2);
                        this.Cbomat_tax_name.Items.Add(txtmat_tax_name);
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
        private void Fill_Cbomat_tax_name2()
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
                                  " FROM b001_10mat_tax" +
                                  " WHERE (txtmat_tax_name = '" + this.Cbomat_tax_name.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtmat_tax_id.Text = dt.Rows[0]["txtmat_tax_id"].ToString();
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
        private void Cbomat_tax_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Cbomat_tax_name2();
        }
        private void Fill_Cbomat_tax_name_Edit()
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
                                  " FROM b001_10mat_tax" +
                                  " WHERE (txtmat_tax_id = '" + this.txtmat_tax_id.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.Cbomat_tax_name.Text = dt.Rows[0]["txtmat_tax_name"].ToString();
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

        private void Fill_Cbomat_credit_charge_name()
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
                                  " FROM b001_11mat_credit_charge" +
                                  " ORDER BY txtmat_credit_charge_no";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtmat_credit_charge_name = dr.GetString(2);
                        this.Cbomat_credit_charge_name.Items.Add(txtmat_credit_charge_name);
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
        private void Fill_Cbomat_credit_charge_name2()
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
                                  " FROM b001_11mat_credit_charge" +
                                  " WHERE (txtmat_credit_charge_name = '" + this.Cbomat_credit_charge_name.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtmat_credit_charge_id.Text = dt.Rows[0]["txtmat_credit_charge_id"].ToString();
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
        private void Cbomat_credit_charge_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Cbomat_credit_charge_name2();
        }
        private void Fill_Cbomat_credit_charge_name_Edit()
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
                                  " FROM b001_11mat_credit_charge" +
                                  " WHERE (txtmat_credit_charge_id = '" + this.txtmat_credit_charge_id.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.Cbomat_credit_charge_name.Text = dt.Rows[0]["txtmat_credit_charge_name"].ToString();
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

        private void Fill_Cbomat_type_with_acc_name()
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
                                  " FROM b001_12mat_type_with_acc" +
                                  " ORDER BY txtmat_type_with_acc_no";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtmat_type_with_acc_name = dr.GetString(2);
                        this.Cbomat_type_with_acc_name.Items.Add(txtmat_type_with_acc_name);
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
        private void Fill_Cbomat_type_with_acc_name2()
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
                                  " FROM b001_12mat_type_with_acc" +
                                  " WHERE (txtmat_type_with_acc_name = '" + this.Cbomat_type_with_acc_name.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtmat_type_with_acc_id.Text = dt.Rows[0]["txtmat_type_with_acc_id"].ToString();
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
        private void Cbomat_type_with_acc_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Cbomat_type_with_acc_name2();
        }
        private void Fill_Cbomat_type_with_acc_name_Edit()
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
                                  " FROM b001_12mat_type_with_acc" +
                                  " WHERE (txtmat_type_with_acc_id = '" + this.txtmat_type_with_acc_id.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.Cbomat_type_with_acc_name.Text = dt.Rows[0]["txtmat_type_with_acc_name"].ToString();
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

        private void txtmat_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                   && !char.IsDigit(e.KeyChar)
                   && e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
                   && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
                   && e.KeyChar != '/'
                    && e.KeyChar != '_'
                   && e.KeyChar != 'a' && e.KeyChar != 'b' && e.KeyChar != 'c' && e.KeyChar != 'd' && e.KeyChar != 'e' && e.KeyChar != 'f' && e.KeyChar != 'g' && e.KeyChar != 'h' && e.KeyChar != 'i' && e.KeyChar != 'j'
                   && e.KeyChar != 'k' && e.KeyChar != 'l' && e.KeyChar != 'm' && e.KeyChar != 'n' && e.KeyChar != 'o' && e.KeyChar != 'p' && e.KeyChar != 'q' && e.KeyChar != 'r' && e.KeyChar != 's'
                   && e.KeyChar != 't' && e.KeyChar != 'u' && e.KeyChar != 'v' && e.KeyChar != 'w' && e.KeyChar != 'x' && e.KeyChar != 'y' && e.KeyChar != 'z'
                   && e.KeyChar != 'A' && e.KeyChar != 'B' && e.KeyChar != 'C' && e.KeyChar != 'D' && e.KeyChar != 'E' && e.KeyChar != 'F' && e.KeyChar != 'G' && e.KeyChar != 'H' && e.KeyChar != 'I' && e.KeyChar != 'J'
                   && e.KeyChar != 'K' && e.KeyChar != 'L' && e.KeyChar != 'M' && e.KeyChar != 'N' && e.KeyChar != 'O' && e.KeyChar != 'P' && e.KeyChar != 'Q' && e.KeyChar != 'R' && e.KeyChar != 'S'
                   && e.KeyChar != 'T' && e.KeyChar != 'U' && e.KeyChar != 'V' && e.KeyChar != 'W' && e.KeyChar != 'X' && e.KeyChar != 'Y' && e.KeyChar != 'Z'

             )
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter && this.txtmat_id.Text == "")
            {
                this.txtmat_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_id.Text.Trim() != "")
            {
                this.txtmat_no.Focus();

            }

        }

        private void txtmat_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_no.Text == "")
            {
                this.txtmat_no.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_no.Text.Trim() != "")
            {
                if (this.txtmat_no.TextLength == 5)
                {
                    this.txtmat_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่ลำดับให้ครับ  5 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtmat_no.Focus();
                    return;
                }
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_no.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtmat_name_market.Focus();

        }

        private void txtmat_name_market_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtmat_name_eng.Focus();

        }

        private void txtmat_name_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtmat_name_bill.Focus();

        }

        private void txtmat_name_bill_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtmat_unit1_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_unit1_qty.Text == "")
            {
                this.txtmat_unit1_qty.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_unit1_qty.Text.Trim() != "")
            {
                this.txtmat_unit2_qty.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_unit1_qty.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_unit2_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_unit2_qty.Text == "")
            {
                this.txtmat_unit2_qty.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_unit2_qty.Text.Trim() != "")
            {
                this.txtmat_qty_min.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_unit2_qty.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_qty_min_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_min.Text == "")
            {
                this.txtmat_qty_min.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_min.Text.Trim() != "")
            {
                this.txtmat_qty_max.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_qty_min.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_qty_max_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_max.Text == "")
            {
                this.txtmat_qty_max.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_max.Text.Trim() != "")
            {
                this.txtmat_qty_per_labor.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_qty_max.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_qty_per_labor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_per_labor.Text == "")
            {
                this.txtmat_qty_per_labor.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_per_labor.Text.Trim() != "")
            {
                this.txtmat_qty_per_labor.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_qty_per_labor.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_barcode_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '.' && e.KeyChar != '+' && e.KeyChar != '-'
                    && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '*'
                    && e.KeyChar != '/'
                     && e.KeyChar != '_'
                    && e.KeyChar != 'a' && e.KeyChar != 'b' && e.KeyChar != 'c' && e.KeyChar != 'd' && e.KeyChar != 'e' && e.KeyChar != 'f' && e.KeyChar != 'g' && e.KeyChar != 'h' && e.KeyChar != 'i' && e.KeyChar != 'j'
                    && e.KeyChar != 'k' && e.KeyChar != 'l' && e.KeyChar != 'm' && e.KeyChar != 'n' && e.KeyChar != 'o' && e.KeyChar != 'p' && e.KeyChar != 'q' && e.KeyChar != 'r' && e.KeyChar != 's'
                    && e.KeyChar != 't' && e.KeyChar != 'u' && e.KeyChar != 'v' && e.KeyChar != 'w' && e.KeyChar != 'x' && e.KeyChar != 'y' && e.KeyChar != 'z'
                    && e.KeyChar != 'A' && e.KeyChar != 'B' && e.KeyChar != 'C' && e.KeyChar != 'D' && e.KeyChar != 'E' && e.KeyChar != 'F' && e.KeyChar != 'G' && e.KeyChar != 'H' && e.KeyChar != 'I' && e.KeyChar != 'J'
                    && e.KeyChar != 'K' && e.KeyChar != 'L' && e.KeyChar != 'M' && e.KeyChar != 'N' && e.KeyChar != 'O' && e.KeyChar != 'P' && e.KeyChar != 'Q' && e.KeyChar != 'R' && e.KeyChar != 'S'
                    && e.KeyChar != 'T' && e.KeyChar != 'U' && e.KeyChar != 'V' && e.KeyChar != 'W' && e.KeyChar != 'X' && e.KeyChar != 'Y' && e.KeyChar != 'Z'

              )
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char)Keys.Enter && this.txtmat_barcode_id.Text == "")
            {
                this.txtmat_barcode_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_barcode_id.Text.Trim() != "")
            {
                this.tabPage3.Focus();

            }

        }

        private void txtmat_price_sale1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale1.Text == "")
            {
                this. txtmat_price_sale1.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale1.Text.Trim() != "")
            {
                    this.txtmat_price_sale2.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale1.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale2.Text == "")
            {
                this. txtmat_price_sale2.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale2.Text.Trim() != "")
            {
                this.txtmat_price_sale3.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale2.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale3.Text == "")
            {
                this. txtmat_price_sale3.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale3.Text.Trim() != "")
            {
                this.txtmat_price_sale4.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale3.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale4.Text == "")
            {
                this. txtmat_price_sale4.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale4.Text.Trim() != "")
            {
                this.txtmat_price_sale5.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale4.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale5.Text == "")
            {
                this. txtmat_price_sale5.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale5.Text.Trim() != "")
            {
                this.txtmat_price_sale6.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale5.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale6.Text == "")
            {
                this. txtmat_price_sale6.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale6.Text.Trim() != "")
            {
                this.txtmat_price_sale7.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale6.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale7.Text == "")
            {
                this. txtmat_price_sale7.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale7.Text.Trim() != "")
            {
                this.txtmat_price_sale8.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale7.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale8.Text == "")
            {
                this. txtmat_price_sale8.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale8.Text.Trim() != "")
            {
                this.txtmat_price_sale9.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale8.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale9.Text == "")
            {
                this. txtmat_price_sale9.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale9.Text.Trim() != "")
            {
                this.txtmat_price_sale10.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale9.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_price_sale10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale10.Text == "")
            {
                this. txtmat_price_sale10.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_price_sale10.Text.Trim() != "")
            {
                this.txtmat_price_sale10.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_price_sale10.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }


        private void txtmat_qty_width_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_width.Text == "")
            {
                this.txtmat_qty_width.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_width.Text.Trim() != "")
            {
                this.txtmat_qty_weight.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_qty_width.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_qty_weight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_weight.Text == "")
            {
                this.txtmat_qty_weight.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_weight.Text.Trim() != "")
            {
                this.txtmat_qty_long.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_qty_weight.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_qty_long_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_long.Text == "")
            {
                this.txtmat_qty_long.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_qty_long.Text.Trim() != "")
            {
                this.txtlength_weight_unit.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_qty_long.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtlength_weight_unit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtmat_qty_high.Focus();

        }

        private void txtmat_qty_high_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_qty_high.Text == "")
            {
                this. txtmat_qty_high.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_qty_high.Text.Trim() != "")
            {
                this.txtlength_measurement_unit.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_qty_high.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtlength_measurement_unit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tabPage5.Focus();

        }
        private void txtmat_price_phurchase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_price_phurchase.Text == "")
            {
                this.txtmat_price_phurchase.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_price_phurchase.Text.Trim() != "")
            {
                this.txtmat_discount.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_price_phurchase.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_discount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_discount.Text == "")
            {
                this.txtmat_discount.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_discount.Text.Trim() != "")
            {
                this.txtmat_bonus.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_discount.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_bonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this. txtmat_bonus.Text == "")
            {
                this. txtmat_bonus.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this. txtmat_bonus.Text.Trim() != "")
            {
                this.txtmat_phurchase_min.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') &&  txtmat_bonus.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_phurchase_min_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_phurchase_min.Text == "")
            {
                this.txtmat_phurchase_min.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_phurchase_min.Text.Trim() != "")
            {
                this.txtmat_phurchase_max.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_phurchase_min.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_phurchase_max_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_phurchase_max.Text == "")
            {
                this.txtmat_phurchase_max.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_phurchase_max.Text.Trim() != "")
            {
                this.txtmat_Leadtime.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_phurchase_max.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtmat_Leadtime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_Leadtime.Text == "")
            {
                this.txtmat_Leadtime.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_Leadtime.Text.Trim() != "")
            {
                this.txtsupplier_remark.Focus();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_Leadtime.Text.Length == 0)
            {
                //e.KeyChar <= '0' || 
                e.Handled = true;
                return;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtsupplier_remark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.PANEL161_SUP_btnAdd_Gridview2.Focus();

        }


        private void btnpicture1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture1.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture1.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture1.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture1.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size1.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture2.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture2.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture2.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture2.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size2.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture3.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture3.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture3.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture3.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size3.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnpicture4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.InitialDirectory = "C:/Picture/";
                f.Filter = "JPEGs|*.jpg|Bitmaps|*.bmp|GIFs|*.gif|All Files|*.*";
                f.FilterIndex = 1;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string picPath = f.FileName.ToString();
                    this.Pic_picture4.ImageLocation = picPath; //Image.FromFile(f.FileName);
                    this.txtpicture4.Text = picPath; //f.SafeFileName.ToString();
                    this.Pic_picture4.SizeMode = PictureBoxSizeMode.Zoom;
                    this.Pic_picture4.BorderStyle = BorderStyle.FixedSingle;

                    var fileLength = new FileInfo(picPath).Length;
                    this.txtpicture_size4.Text = Convert.ToString(fileLength);
                }
            }
            catch { }
            //เตรียมภาพสำหรับ save
        }

        private void btnUp_pic1_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                    {
                        cmd2.CommandText = "UPDATE b001mat_12picture SET " +
                                                                       "txtmat_1picture_size = @txtmat_1picture_size," +
                                                                       "txtmat_1picture = @txtmat_1picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture1.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture1.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size1.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture", imageBt));
                        }

                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic2_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                    {
                        cmd2.CommandText = "UPDATE b001mat_12picture SET " +
                                                                       "txtmat_2picture_size = @txtmat_2picture_size," +
                                                                       "txtmat_2picture = @txtmat_2picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture2.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture2.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size2.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture", imageBt));
                        }

                        //==============================                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic3_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                    {
                        cmd2.CommandText = "UPDATE b001mat_12picture SET " +
                                                                       "txtmat_3picture_size = @txtmat_3picture_size," +
                                                                       "txtmat_3picture = @txtmat_3picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture3.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture3.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size3.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture", imageBt));
                        }

                        //==============================


                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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

        private void btnUp_pic4_Click(object sender, EventArgs e)
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
                    if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                    {
                        cmd2.CommandText = "UPDATE b001mat_12picture SET " +
                                                                       "txtmat_4picture_size = @txtmat_4picture_size," +
                                                                       "txtmat_4picture = @txtmat_4picture" +
                                                                        " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                       " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                       " AND (txtmat_id = '" + this.txtmat_id.Text.Trim() + "')";
                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture4.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture4.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size4.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture", imageBt));
                        }

                        //==============================
                        //==============================


                        cmd2.ExecuteNonQuery();

                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();


                        if (this.iblword_status.Text.Trim() == "แก้ไขรหัสสินค้า")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("ไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
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


        private void Fill_PANEL_FORM1_dataGridView1()
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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.ColumnCount = 9;
                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

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
                            this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_name_eng"].Value = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name_market"].Value = dt2.Rows[j]["txtmat_name_market"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_name_bill"].Value = dt2.Rows[j]["txtmat_name_bill"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtmat_remark"].Value = dt2.Rows[j]["txtmat_remark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //8
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
        private void PANEL_FORM1_GridView1()
        {
            this.GridView1.ColumnCount = 9;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtmat_no";
            this.GridView1.Columns[2].Name = "Col_txtmat_id";
            this.GridView1.Columns[3].Name = "Col_txtmat_name";
            this.GridView1.Columns[4].Name = "Col_txtmat_name_eng";
            this.GridView1.Columns[5].Name = "Col_txtmat_name_market";
            this.GridView1.Columns[6].Name = "Col_txtmat_name_bill";
            this.GridView1.Columns[7].Name = "Col_txtmat_remark";
            this.GridView1.Columns[8].Name = "Col_txtmat_status";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "ลำดับ";
            this.GridView1.Columns[2].HeaderText = " รหัส";
            this.GridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.GridView1.Columns[4].HeaderText = " ชื่อสินค้า Eng";
            this.GridView1.Columns[5].HeaderText = " ชื่อสินค้าทางการตลาด";
            this.GridView1.Columns[6].HeaderText = " ชื่อใช้ในการออกบิล";
            this.GridView1.Columns[7].HeaderText = " หมายเหตุ";
            this.GridView1.Columns[8].HeaderText = " สถานะ";

            this.GridView1.Columns[0].Visible = false;  //"No";
            this.GridView1.Columns[1].Visible = true;  //"Col_txtmat_no";
            this.GridView1.Columns[1].Width = 100;
            this.GridView1.Columns[1].ReadOnly = true;
            this.GridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[2].Visible = true;  //"Col_txtmat_id";
            this.GridView1.Columns[2].Width = 150;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[3].Visible = true;  //"Col_txtmat_name";
            this.GridView1.Columns[3].Width = 150;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[4].Visible = false;  //"Col_txtmat_name_eng";
            this.GridView1.Columns[4].Width = 150;
            this.GridView1.Columns[4].ReadOnly = true;
            this.GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[5].Visible = true;  //"Col_txtmat_name_market";
            this.GridView1.Columns[5].Width = 150;
            this.GridView1.Columns[5].ReadOnly = true;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[6].Visible = false;  //"Col_txtmat_name_bill";
            this.GridView1.Columns[6].Width = 150;
            this.GridView1.Columns[6].ReadOnly = true;
            this.GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[7].Visible = true;  //"Col_txtmat_remark";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = true;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[8].Visible = false;  //"Col_txtmat_status";

            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridView1.Columns.Add(dgvCmb);


        }
        private void PANEL_FORM1_Clear_GridView1()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
        }
        private void PANEL_FORM1_Clear_GridView1_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells[8].Value.ToString() == "0")  //Active
                {
                    this.GridView1.Rows[i].Cells[9].Value = true;
                }
                else
                {
                    this.GridView1.Rows[i].Cells[9].Value = false;

                }
            }
        }
        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            Fill_PANEL_FORM1_dataGridView1();
        }
        private void PANEL_FORM1_btnsearch_Click(object sender, EventArgs e)
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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.ColumnCount = 9;
                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                //this.cboSearch.Items.Add("รหัสสินค้า");
                //this.cboSearch.Items.Add("ชื่อสินค้า");

                if (this.cboSearch.Text.Trim() == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (b001mat.txtmat_id = '" + this.PANEL_FORM1_txtsearch.Text.Trim() + "')" +
                                       " ORDER BY b001mat.txtmat_no ASC";

                }
                else if (this.cboSearch.Text.Trim() == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (b001mat.txtmat_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                       " ORDER BY b001mat.txtmat_no ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         //" AND (b001mat.txtmat_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                       " ORDER BY b001mat.txtmat_no ASC";

                }

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
                            this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_name_eng"].Value = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name_market"].Value = dt2.Rows[j]["txtmat_name_market"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_name_bill"].Value = dt2.Rows[j]["txtmat_name_bill"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtmat_remark"].Value = dt2.Rows[j]["txtmat_remark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //8

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
            //================================

        }
        private void PANEL_FORM1_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRowIndex = e.RowIndex;
                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {

                    this.txtmat_no.Text = row.Cells[1].Value.ToString();
                    this.txtmat_id.Text = row.Cells[2].Value.ToString();
                    this.txtmat_name.Text = row.Cells[3].Value.ToString();

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
                    Clear_Text();
                    //===========================================
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
                                            "b001mat_06price_sale.*," +
                                            "b001mat_10shipment.*," +
                                            "b001mat_12picture.*," +
                                             "b001mat_13point_phurchase.*" +
                                           " FROM b001mat" +

                                            " INNER JOIN b001mat_02detail" +
                                            " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                            " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                            " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                            " INNER JOIN b001mat_04barcode" +
                                            " ON b001mat.cdkey = b001mat_04barcode.cdkey" +
                                            " AND b001mat.txtco_id = b001mat_04barcode.txtco_id" +
                                            " AND b001mat.txtmat_id = b001mat_04barcode.txtmat_id" +

                                            " INNER JOIN b001mat_06price_sale" +
                                            " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                            " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                            " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                             " INNER JOIN b001mat_10shipment" +
                                            " ON b001mat.cdkey = b001mat_10shipment.cdkey" +
                                            " AND b001mat.txtco_id = b001mat_10shipment.txtco_id" +
                                            " AND b001mat.txtmat_id = b001mat_10shipment.txtmat_id" +

                                            " INNER JOIN b001mat_12picture" +
                                            " ON b001mat.cdkey = b001mat_12picture.cdkey" +
                                            " AND b001mat.txtco_id = b001mat_12picture.txtco_id" +
                                            " AND b001mat.txtmat_id = b001mat_12picture.txtmat_id" +

                                            " INNER JOIN b001mat_13point_phurchase" +
                                            " ON b001mat.cdkey = b001mat_13point_phurchase.cdkey" +
                                            " AND b001mat.txtco_id = b001mat_13point_phurchase.txtco_id" +
                                            " AND b001mat.txtmat_id = b001mat_13point_phurchase.txtmat_id" +

                                            " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                            " AND (b001mat.txtmat_id = '" + this.txtmat_id.Text.Trim() + "')" +
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
                                    this.txtmat_id.Text = dt2.Rows[j]["txtmat_id"].ToString();      //1
                                    this.txtmat_no.Text = dt2.Rows[j]["txtmat_no"].ToString();      //2
                                    this.txtmat_name.Text = dt2.Rows[j]["txtmat_name"].ToString();      //3
                                    this.txtmat_name_eng.Text = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                                    this.txtmat_name_market.Text = dt2.Rows[j]["txtmat_name_market"].ToString();      //3
                                    this.txtmat_name_bill.Text = dt2.Rows[j]["txtmat_name_bill"].ToString();      //3
                                    if (dt2.Rows[j]["txtmat_status"].ToString() == "0") //5
                                    {
                                        this.check_mat_status.Checked = true;
                                    }
                                    else
                                    {
                                        this.check_mat_status.Checked = false;
                                    }
                                    //=================================================================
                                    this.PANEL101_MAT_TYPE_txtmat_type_id.Text = dt2.Rows[j]["txtmat_type_id"].ToString();      //6  *************
                                    //this.PANEL101_MAT_TYPE_txtmat_type_name.Text = dt2.Rows[j]["txtmat_type_name"].ToString();      //7

                                    this.PANEL102_MAT_SAC_txtmat_sac_id.Text = dt2.Rows[j]["txtmat_sac_id"].ToString();      //8
                                    //this.PANEL102_MAT_SAC_txtmat_sac_name.Text = dt2.Rows[j]["txtmat_sec_name"].ToString();      //9
                                    this.PANEL103_MAT_GROUP_txtmat_group_id.Text = dt2.Rows[j]["txtmat_group_id"].ToString();      //10
                                    //this.PANEL103_MAT_GROUP_txtmat_group_name.Text = dt2.Rows[j]["txtmat_group_name"].ToString();      //11
                                    this.PANEL104_MAT_BRAND_txtmat_brand_id.Text = dt2.Rows[j]["txtmat_brand_id"].ToString();      //12
                                    //this.PANEL104_MAT_BRAND_txtmat_brand_name.Text = dt2.Rows[j]["txtmat_brand_name"].ToString();      //13

                                    this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text = dt2.Rows[j]["txtmat_unit1_id"].ToString();      //14
                                    //this.PANEL105_MAT_UNIT1_txtmat_unit1_name.Text = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //15
                                    this.txtmat_unit1_qty.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");     //16

                                    if (dt2.Rows[j]["chmat_unit_status"].ToString() == "Y") //17
                                    {
                                        this.chmat_unit_status.Checked = true;

                                    }
                                    else
                                    {
                                        this.chmat_unit_status.Checked = false;
                                    }
                                    this.txtchmat_unit_status.Text = dt2.Rows[j]["chmat_unit_status"].ToString();

                                    this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text = dt2.Rows[j]["txtmat_unit2_id"].ToString();      //18
                                    //this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Text = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //19
                                    this.txtmat_unit2_qty.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.000#");     //20
                                    this.PANEL105_MAT_UNIT3_txtmat_unit3_id.Text = dt2.Rows[j]["txtmat_unit3_id"].ToString();      //18
                                    this.PANEL105_MAT_UNIT4_txtmat_unit4_id.Text = dt2.Rows[j]["txtmat_unit4_id"].ToString();      //18
                                    this.PANEL105_MAT_UNIT5_txtmat_unit5_id.Text = dt2.Rows[j]["txtmat_unit5_id"].ToString();      //18


                                    this.txtmat_detail_group_id.Text = dt2.Rows[j]["txtmat_detail_group_id"].ToString();      //21
                                    this.txtmat_incentive_id.Text = dt2.Rows[j]["txtmat_incentive_id"].ToString();      //22
                                    this.txtmat_tax_id.Text = dt2.Rows[j]["txtmat_tax_id"].ToString();      //23
                                    this.txtmat_credit_charge_id.Text = dt2.Rows[j]["txtmat_credit_charge_id"].ToString();      //24
                                    this.txtmat_type_with_acc_id.Text = dt2.Rows[j]["txtmat_type_with_acc_id"].ToString();      //25

                                    this.txtmat_qty_min.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_min"]).ToString("###,###.00");     //26
                                    this.txtmat_qty_max.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_max"]).ToString("###,###.00");     //27
                                    this.txtmat_qty_per_labor.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_per_labor"]).ToString("###,###.00");     //28
                                    this.txtmat_remark.Text = dt2.Rows[j]["txtmat_remark"].ToString();      //29
                                    //=================================================================

                                    this.txtmat_barcode_id.Text = dt2.Rows[j]["txtmat_barcode_id"].ToString();      //30

                                    //=================================================================

                                    this.txtmat_price_sale1.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");     //31
                                    this.txtmat_price_sale2.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale2"]).ToString("###,###.00");     //32
                                    this.txtmat_price_sale3.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale3"]).ToString("###,###.00");     //33
                                    this.txtmat_price_sale4.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale4"]).ToString("###,###.00");     //34
                                    this.txtmat_price_sale5.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale5"]).ToString("###,###.00");     //35
                                    this.txtmat_price_sale6.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale6"]).ToString("###,###.00");     //36
                                    this.txtmat_price_sale7.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale7"]).ToString("###,###.00");     //37
                                    this.txtmat_price_sale8.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale8"]).ToString("###,###.00");     //38
                                    this.txtmat_price_sale9.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale9"]).ToString("###,###.00");     //39
                                    this.txtmat_price_sale10.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale10"]).ToString("###,###.00");     //40
                                     //=================================================================

                                    this.txtmat_qty_width.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_width"]).ToString("###,###.00");     //41
                                    this.txtmat_qty_long.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_long"]).ToString("###,###.00");     //42
                                    this.txtmat_qty_high.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_high"]).ToString("###,###.00");     //43
                                    this.txtlength_measurement_unit.Text = dt2.Rows[j]["txtlength_measurement_unit"].ToString();      //44
                                    this.txtmat_qty_weight.Text = Convert.ToSingle(dt2.Rows[j]["txtmat_qty_weight"]).ToString("###,###.00");     //45
                                    this.txtlength_weight_unit.Text = dt2.Rows[j]["txtlength_weight_unit"].ToString();      //46

                                    //Load Picture================================
                                    this.txtpicture_size1.Text = dt2.Rows[0]["txtmat_1picture_size"].ToString();
                                    this.txtpicture_size2.Text = dt2.Rows[0]["txtmat_2picture_size"].ToString();
                                    this.txtpicture_size3.Text = dt2.Rows[0]["txtmat_3picture_size"].ToString();
                                    this.txtpicture_size4.Text = dt2.Rows[0]["txtmat_4picture_size"].ToString();

                                    //=======================================================
                                    if (this.txtpicture_size1.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg1 = (byte[])(dt2.Rows[0]["txtmat_1picture"]);
                                        if (imgg1 == null)
                                        {
                                            this.Pic_picture1.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream1 = new MemoryStream(imgg1);
                                            this.Pic_picture1.Image = Image.FromStream(mstream1);
                                            this.Pic_picture1.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture1.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic1.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size2.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg2 = (byte[])(dt2.Rows[0]["txtmat_2picture"]);
                                        if (imgg2 == null)
                                        {
                                            this.Pic_picture2.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream2 = new MemoryStream(imgg2);
                                            this.Pic_picture2.Image = Image.FromStream(mstream2);
                                            this.Pic_picture2.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture2.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic2.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size3.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg3 = (byte[])(dt2.Rows[0]["txtmat_3picture"]);
                                        if (imgg3 == null)
                                        {
                                            this.Pic_picture3.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream3 = new MemoryStream(imgg3);
                                            this.Pic_picture3.Image = Image.FromStream(mstream3);
                                            this.Pic_picture3.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture3.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic3.Visible = true;
                                        }
                                    }
                                    //=======================================================
                                    if (this.txtpicture_size4.Text == "")
                                    {

                                    }
                                    else
                                    {
                                        byte[] imgg4 = (byte[])(dt2.Rows[0]["txtmat_4picture"]);
                                        if (imgg4 == null)
                                        {
                                            this.Pic_picture4.Image = null;
                                        }
                                        else
                                        {
                                            MemoryStream mstream4 = new MemoryStream(imgg4);
                                            this.Pic_picture4.Image = Image.FromStream(mstream4);
                                            this.Pic_picture4.SizeMode = PictureBoxSizeMode.Zoom;
                                            this.Pic_picture4.BorderStyle = BorderStyle.FixedSingle;
                                            this.btnUp_pic4.Visible = true;
                                        }
                                    }
                                    //=======================================================


                                }
                                //Load Picture================================
                                this.txtmat_amount_phurchase.Text = Convert.ToSingle(dt2.Rows[0]["txtmat_amount_phurchase"]).ToString("###,###.00");     //20

                                //===========================================

                                if (this.txtmat_id.Text != "")
                                {
                                    this.iblword_status.Text = "แก้ไขรหัสสินค้า";
                                    this.txtmat_id.ReadOnly = true;
                                    this.BtnCancel_Doc.Enabled = true;

                                    this.btnUp_pic1.Visible = true;
                                    this.btnUp_pic2.Visible = true;
                                    this.btnUp_pic3.Visible = true;
                                    this.btnUp_pic4.Visible = true;

                                    Fill_PANEL161_SUP_Gridview2();
                                    Fill_Cbomat_detail_group_name_Edit();
                                    Fill_Cbomat_incentive_name_Edit();
                                    Fill_Cbomat_tax_name_Edit();
                                    Fill_Cbomat_credit_charge_name_Edit();
                                    Fill_Cbomat_type_with_acc_name_Edit();

                                    PANEL101_MAT_TYPE_Fill_mat_type_Edit();
                                    PANEL102_MAT_SAC_Fill_mat_sac_Edit();
                                    PANEL103_MAT_GROUP_Fill_mat_group_Edit();
                                    PANEL104_MAT_BRAND_Fill_mat_brand_Edit();
                                    PANEL105_MAT_UNIT1_Fill_mat_unit_Edit();
                                    PANEL105_MAT_UNIT2_Fill_mat_unit_Edit();
                                    PANEL105_MAT_UNIT3_Fill_mat_unit_Edit();
                                    PANEL105_MAT_UNIT4_Fill_mat_unit_Edit();
                                    PANEL105_MAT_UNIT5_Fill_mat_unit_Edit();


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
            }
        }

        private void Clear_Text()
        {
            this.txtmat_name.Text = "";
            this.txtmat_name_eng.Text = "";
            this.txtmat_name_market.Text = "";
            this.txtmat_name_bill.Text = "";

            this.PANEL101_MAT_TYPE_txtmat_type_name.Text = "";
            this.PANEL101_MAT_TYPE_txtmat_type_id.Text = "";
            this.PANEL102_MAT_SAC_txtmat_sac_name.Text = "";
            this.PANEL102_MAT_SAC_txtmat_sac_id.Text = "";
            this.PANEL103_MAT_GROUP_txtmat_group_name.Text = "";
            this.PANEL103_MAT_GROUP_txtmat_group_id.Text = "";
            this.PANEL104_MAT_BRAND_txtmat_brand_name.Text = "";
            this.PANEL104_MAT_BRAND_txtmat_brand_id.Text = "";
            this.txtmat_unit1_qty.Text = "0";
            this.chmat_unit_status.Checked = false;
            this.PANEL105_MAT_UNIT2_txtmat_unit2_name.Text = "";
            this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text = "";
            this.txtmat_unit2_qty.Text = ".0000";
            this.Cbomat_detail_group_name.Text = "";
            this.txtmat_detail_group_id.Text = "";
            this.Cbomat_incentive_name.Text = "";
            this.txtmat_incentive_id.Text = "";
            this.Cbomat_tax_name.Text = "";
            this.txtmat_tax_id.Text = "";
            this.Cbomat_credit_charge_name.Text = "";
            this.txtmat_credit_charge_id.Text = "";
            this.Cbomat_type_with_acc_name.Text = "";
            this.txtmat_type_with_acc_id.Text = "";
            this.txtmat_qty_min.Text = "0";
            this.txtmat_qty_max.Text = "0";
            this.txtmat_qty_per_labor.Text = "0";
            this.txtmat_remark.Text = "";

            this.txtmat_barcode_id.Text = "";

            this.txtmat_price_sale1.Text = ".00";
            this.txtmat_price_sale2.Text = ".00";
            this.txtmat_price_sale3.Text = ".00";
            this.txtmat_price_sale4.Text = ".00";
            this.txtmat_price_sale5.Text = ".00";
            this.txtmat_price_sale6.Text = ".00";
            this.txtmat_price_sale7.Text = ".00";
            this.txtmat_price_sale8.Text = ".00";
            this.txtmat_price_sale9.Text = ".00";
            this.txtmat_price_sale10.Text = ".00";

            this.txtmat_qty_width.Text = ".00";
            this.txtmat_qty_long.Text = ".00";
            this.txtmat_qty_high.Text = ".00";
            this.txtmat_qty_weight.Text = ".00";

            this.PANEL161_SUP_txtsupplier_name.Text = "";
            this.PANEL161_SUP_txtsupplier_id.Text = "";
            this.txtmat_price_phurchase.Text = ".00";
            this.txtmat_discount.Text = ".00";
            this.txtmat_phurchase_min.Text = ".00";
            this.txtmat_bonus.Text = ".00";
            this.txtmat_phurchase_max.Text = ".00";
            this.txtmat_Leadtime.Text = "";
            this.txtsupplier_remark.Text = "";

            PANEL161_SUP_Clear_GridView2();

            this.Pic_picture1.Image = null;
            this.Pic_picture2.Image = null;
            this.Pic_picture3.Image = null;
            this.Pic_picture4.Image = null;

            this.txtmat_amount_phurchase.Text = ".00";

        }


        private void btnGo2_Click(object sender, EventArgs e)
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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.ColumnCount = 9;
                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                //this.cboSearch.Items.Add("รหัสสินค้า");
                //this.cboSearch.Items.Add("ชื่อสินค้า");

                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (b001mat_02detail.txtmat_type_id = '" + this.PANEL101_MAT_TYPE2_txtmat_type_id.Text.Trim() + "')" +
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
                            this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_name_eng"].Value = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name_market"].Value = dt2.Rows[j]["txtmat_name_market"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_name_bill"].Value = dt2.Rows[j]["txtmat_name_bill"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtmat_remark"].Value = dt2.Rows[j]["txtmat_remark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //8

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
            //================================

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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.ColumnCount = 9;
                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                //this.cboSearch.Items.Add("รหัสสินค้า");
                //this.cboSearch.Items.Add("ชื่อสินค้า");

                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (b001mat_02detail.txtmat_sac_id = '" + this.PANEL102_MAT_SAC2_txtmat_sac_id.Text.Trim() + "')" +
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
                            this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_name_eng"].Value = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name_market"].Value = dt2.Rows[j]["txtmat_name_market"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_name_bill"].Value = dt2.Rows[j]["txtmat_name_bill"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtmat_remark"].Value = dt2.Rows[j]["txtmat_remark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //8

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
            //================================

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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.ColumnCount = 9;
                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                //this.cboSearch.Items.Add("รหัสสินค้า");
                //this.cboSearch.Items.Add("ชื่อสินค้า");

                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (b001mat_02detail.txtmat_group_id = '" + this.PANEL103_MAT_GROUP2_txtmat_group_id.Text.Trim() + "')" +
                                   " ORDER BY b001mat.txtmat_no ASC";

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
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_name_eng"].Value = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name_market"].Value = dt2.Rows[j]["txtmat_name_market"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_name_bill"].Value = dt2.Rows[j]["txtmat_name_bill"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtmat_remark"].Value = dt2.Rows[j]["txtmat_remark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //8

                        }

                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
            //================================

        }

        private void btnGo5_Click(object sender, EventArgs e)
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

            PANEL_FORM1_Clear_GridView1();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                //this.PANEL_FORM1_dataGridView1.ColumnCount = 9;
                //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtmat_no";
                //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtmat_id";
                //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtmat_name";
                //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtmat_name_eng";
                //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtmat_name_market";
                //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtmat_name_bill";
                //this.PANEL_FORM1_dataGridView1.Columns[7].Name = "Col_txtmat_remark";
                //this.PANEL_FORM1_dataGridView1.Columns[8].Name = "Col_txtmat_status";

                //this.cboSearch.Items.Add("รหัสสินค้า");
                //this.cboSearch.Items.Add("ชื่อสินค้า");

                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                     " AND (b001mat_02detail.txtmat_brand_id = '" + this.PANEL104_MAT_BRAND2_txtmat_brand_id.Text.Trim() + "')" +
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
                            this.txtcount_rows.Text = dt2.Rows.Count.ToString();

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtmat_name_eng"].Value = dt2.Rows[j]["txtmat_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtmat_name_market"].Value = dt2.Rows[j]["txtmat_name_market"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtmat_name_bill"].Value = dt2.Rows[j]["txtmat_name_bill"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtmat_remark"].Value = dt2.Rows[j]["txtmat_remark"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtmat_status"].Value = dt2.Rows[j]["txtmat_status"].ToString();      //8

                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();

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
            //================================


        }

        private void CHECK_UP_NO999()
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

            string OK = "";

            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id = '')" +
                                    " ORDER BY b001mat.txtmat_no ASC";

                cmd1.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.Default;

                    OK = "Y";
                    conn.Close();
                    return;
                }
            }

            //
            conn.Close();
            //END เชื่อมต่อฐานข้อมูล=======================================================

            if (OK.Trim() != "Y")
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
                        //1
                        cmd2.CommandText = "INSERT INTO b001mat(cdkey,txtco_id," +  //1
                                           "txtmat_id,txtmat_no," +  //2
                                           "txtmat_name,txtmat_name_eng," +  //3
                                           "txtmat_name_market,txtmat_name_bill," +  //4
                                          "txtmat_status) " +  //5
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtmat_id,@txtmat_no," +  //2
                                           "@txtmat_name,@txtmat_name_eng," +  //3
                                           "@txtmat_name_market,@txtmat_name_bill," +  //4
                                           "@txtmat_status)";   //5

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = "999";
                        cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.txtmat_name.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_eng", SqlDbType.NVarChar).Value = this.txtmat_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_market", SqlDbType.NVarChar).Value = this.txtmat_name_market.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_name_bill", SqlDbType.NVarChar).Value = this.txtmat_name_bill.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //2
                        cmd2.CommandText = "INSERT INTO b001mat_02detail(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_type_id,txtmat_sac_id," +  //2
                                           "txtmat_group_id,txtmat_brand_id," +  //3
                                           "txtmat_unit1_id,txtmat_unit1_qty," +  //4
                                           "chmat_unit_status," +  //5
                                           "txtmat_unit2_id,txtmat_unit2_qty," +  //6
                                           "txtmat_detail_group_id,txtmat_incentive_id," +  //7
                                           "txtmat_tax_id,txtmat_credit_charge_id," +  //8
                                           "txtmat_type_with_acc_id," +  //9
                                            "txtmat_qty_min,txtmat_qty_max," +  //10
                                           "txtmat_qty_per_labor," +  //11
                                           "txtmat_remark) " +  //12
                                           "VALUES (@cdkey2,@txtco_id2,@txtmat_id2," +  //13
                                          "@txtmat_type_id,@txtmat_sac_id," +  //2
                                           "@txtmat_group_id,@txtmat_brand_id," +  //3
                                           "@txtmat_unit1_id,@txtmat_unit1_qty," +  //4
                                           "@chmat_unit_status," +  //5
                                           "@txtmat_unit2_id,@txtmat_unit2_qty," +  //6
                                           "@txtmat_detail_group_id,@txtmat_incentive_id," +  //7
                                           "@txtmat_tax_id,@txtmat_credit_charge_id," +  //8
                                           "@txtmat_type_with_acc_id," +  //9
                                            "@txtmat_qty_min,@txtmat_qty_max," +  //10
                                           "@txtmat_qty_per_labor," +  //11
                                           "@txtmat_remark)";   //12

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id2", SqlDbType.NVarChar).Value = "";

                        cmd2.Parameters.Add("@txtmat_type_id", SqlDbType.NVarChar).Value = this.PANEL101_MAT_TYPE_txtmat_type_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_sac_id", SqlDbType.NVarChar).Value = this.PANEL102_MAT_SAC_txtmat_sac_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_group_id", SqlDbType.NVarChar).Value = this.PANEL103_MAT_GROUP_txtmat_group_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_brand_id", SqlDbType.NVarChar).Value = this.PANEL104_MAT_BRAND_txtmat_brand_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_unit1_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT1_txtmat_unit1_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_unit1_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit1_qty.Text.ToString()));
                        if (this.chmat_unit_status.Checked == true)
                        {
                            cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = "Y";
                        }
                        else
                        {
                            cmd2.Parameters.Add("@chmat_unit_status", SqlDbType.NVarChar).Value = "N";
                        }
                        cmd2.Parameters.Add("@txtmat_unit2_id", SqlDbType.NVarChar).Value = this.PANEL105_MAT_UNIT2_txtmat_unit2_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_unit2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString()));

                        cmd2.Parameters.Add("@txtmat_detail_group_id", SqlDbType.NVarChar).Value = this.txtmat_detail_group_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_incentive_id", SqlDbType.NVarChar).Value = this.txtmat_incentive_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_tax_id", SqlDbType.NVarChar).Value = this.txtmat_tax_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_credit_charge_id", SqlDbType.NVarChar).Value = this.txtmat_credit_charge_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_type_with_acc_id", SqlDbType.NVarChar).Value = this.txtmat_type_with_acc_id.Text.ToString();
                        cmd2.Parameters.Add("@txtmat_qty_min", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_min.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_max", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_max.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_per_labor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_per_labor.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_remark", SqlDbType.NVarChar).Value = this.txtmat_remark.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //3
                        cmd2.CommandText = "INSERT INTO b001mat_04barcode(cdkey,txtco_id,txtmat_id," +  //1
                                          "txtmat_barcode_id) " +  //2
                                           "VALUES (@cdkey3,@txtco_id3,@txtmat_id3," +  //1
                                           "@txtmat_barcode_id)";   //2

                        cmd2.Parameters.Add("@cdkey3", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id3", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id3", SqlDbType.NVarChar).Value = "";

                        cmd2.Parameters.Add("@txtmat_barcode_id", SqlDbType.NVarChar).Value = this.txtmat_barcode_id.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //4
                        cmd2.CommandText = "INSERT INTO b001mat_06price_sale(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_price_sale1,txtmat_price_sale2,txtmat_price_sale3,txtmat_price_sale4,txtmat_price_sale5," +  //2
                                           "txtmat_price_sale6,txtmat_price_sale7,txtmat_price_sale8,txtmat_price_sale9,txtmat_price_sale10) " +  //3
                                           "VALUES (@cdkey4,@txtco_id4,@txtmat_id4," +  //1
                                           "@txtmat_price_sale1,@txtmat_price_sale2,@txtmat_price_sale3,@txtmat_price_sale4,@txtmat_price_sale5," +  //2
                                           "@txtmat_price_sale6,@txtmat_price_sale7,@txtmat_price_sale8,@txtmat_price_sale9,@txtmat_price_sale10)";   //3

                        cmd2.Parameters.Add("@cdkey4", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id4", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id4", SqlDbType.NVarChar).Value = this.txtmat_id.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_price_sale1", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale1.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale2", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale2.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale3", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale3.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale4", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale4.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale5", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale5.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale6", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale6.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale7", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale7.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale8", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale8.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale9", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale9.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_price_sale10", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_price_sale10.Text.ToString()));
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //5
                        cmd2.CommandText = "INSERT INTO b001mat_10shipment(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_qty_width,txtmat_qty_long,txtmat_qty_high," +  //2
                                           "txtlength_measurement_unit," +  //3
                                           "txtmat_qty_weight,txtlength_weight_unit) " +  //4
                                           "VALUES (@cdkey5,@txtco_id5,@txtmat_id5," +  //1
                                           "@txtmat_qty_width,@txtmat_qty_long,@txtmat_qty_high," +  //2
                                           "@txtlength_measurement_unit," +  //3
                                           "@txtmat_qty_weight,@txtlength_weight_unit)";   //4

                        cmd2.Parameters.Add("@cdkey5", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id5", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id5", SqlDbType.NVarChar).Value = "";

                        cmd2.Parameters.Add("@txtmat_qty_width", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_width.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_long", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_long.Text.ToString()));
                        cmd2.Parameters.Add("@txtmat_qty_high", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_high.Text.ToString()));
                        cmd2.Parameters.Add("@txtlength_measurement_unit", SqlDbType.NVarChar).Value = this.txtlength_measurement_unit.Text.ToString();

                        cmd2.Parameters.Add("@txtmat_qty_weight", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", this.txtmat_qty_weight.Text.ToString()));
                        cmd2.Parameters.Add("@txtlength_weight_unit", SqlDbType.NVarChar).Value = this.txtlength_weight_unit.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();


                        //6
                        for (int i = 0; i < this.PANEL161_SUP_dataGridView2.Rows.Count; i++)
                        {
                            if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value != null)
                            {
                                //if (this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value.ToString() =="")
                                //{
                                //    this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value = "0";
                                //}
                                //this.PANEL161_SUP_dataGridView2.Columns[0].Name = "Col_Auto_num";
                                //this.PANEL161_SUP_dataGridView2.Columns[1].Name = "Col_txtsupplier_id";
                                //this.PANEL161_SUP_dataGridView2.Columns[2].Name = "Col_txtsupplier_name";
                                //this.PANEL161_SUP_dataGridView2.Columns[3].Name = "Col_txtmat_price_phurchase";
                                //this.PANEL161_SUP_dataGridView2.Columns[4].Name = "Col_txtmat_discount";
                                //this.PANEL161_SUP_dataGridView2.Columns[5].Name = "Col_txtmat_bonus";
                                //this.PANEL161_SUP_dataGridView2.Columns[6].Name = "Col_txtmat_phurchase_min";
                                //this.PANEL161_SUP_dataGridView2.Columns[7].Name = "Col_txtmat_phurchase_max";
                                //this.PANEL161_SUP_dataGridView2.Columns[8].Name = "Col_txtmat_Leadtime";
                                //this.PANEL161_SUP_dataGridView2.Columns[9].Name = "Col_txtsupplier_remark";

                                cmd2.CommandText = "INSERT INTO b001mat_11supplier(cdkey,txtco_id,txtmat_id," +  //1
                                                   "txtsupplier_id," +  //2
                                                   "txtsupplier_name," +  //3
                                                   "txtmat_price_phurchase," +  //4
                                                   "txtmat_discount," +  //5
                                                   "txtmat_bonus," +  //6
                                                   "txtmat_phurchase_min," +  //7
                                                   "txtmat_phurchase_max," +  //8
                                                   "txtmat_Leadtime," +  //9
                                                   "txtsupplier_remark) " +  //10
                                                   "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "',''," +  //1
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[1].Value.ToString() + "'," +  //2
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[3].Value.ToString() + "'," +  //4
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[4].Value.ToString() + "'," +  //5
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[5].Value.ToString() + "'," +  //6
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[6].Value.ToString() + "'," +  //7
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[7].Value.ToString() + "'," +  //8
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[8].Value.ToString() + "'," +  //9
                                                   "'" + this.PANEL161_SUP_dataGridView2.Rows[i].Cells[9].Value.ToString() + "')";   //10

                                //==============================

                                cmd2.ExecuteNonQuery();

                            }
                        }

                        //7
                        cmd2.CommandText = "INSERT INTO b001mat_12picture(cdkey,txtco_id,txtmat_id," +  //1
                                           "txtmat_1picture_size,txtmat_1picture," +  //2
                                           "txtmat_2picture_size,txtmat_2picture," +  //3
                                           "txtmat_3picture_size,txtmat_3picture," +  //4
                                           "txtmat_4picture_size,txtmat_4picture) " +  //5
                                           "VALUES (@cdkey7,@txtco_id7,@txtmat_id7," + //1
                                           "@txtmat_1picture_size,@txtmat_1picture," +  //2
                                           "@txtmat_2picture_size,@txtmat_2picture," +  //3
                                           "@txtmat_3picture_size,@txtmat_3picture," +  //4
                                           "@txtmat_4picture_size,@txtmat_4picture)";  //5

                        cmd2.Parameters.Add("@cdkey7", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id7", SqlDbType.NChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id7", SqlDbType.NVarChar).Value = "";

                        //รูปภาพ ========================
                        //'===================================='
                        if (this.txtpicture1.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture1.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size1.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture1.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture_size", this.txtpicture_size1.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_1picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture2.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture2.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size2.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture2.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture_size", this.txtpicture_size2.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_2picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture3.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture3.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size3.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture3.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture_size", this.txtpicture_size3.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_3picture", imageBt));
                        }

                        //==============================
                        //'===================================='
                        if (this.txtpicture4.Text != "")
                        {
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture", imageBt));
                        }
                        else
                        {
                            this.txtpicture4.Text = "C:\\KD_ERP\\KD_REPORT\\x.jpg";
                            this.txtpicture_size4.Text = "78782";
                            byte[] imageBt = null;
                            FileStream fstream = new FileStream(this.txtpicture4.Text, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fstream);
                            imageBt = br.ReadBytes((int)fstream.Length);
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture_size", this.txtpicture_size4.Text.ToString()));
                            cmd2.Parameters.Add(new SqlParameter("@txtmat_4picture", imageBt));
                        }

                        //==============================
                        cmd2.ExecuteNonQuery();

                        //8
                        cmd2.CommandText = "INSERT INTO b001mat_13point_phurchase(cdkey,txtco_id,txtmat_id," +  //1
                                          "txtmat_amount_phurchase) " +  //2
                                           "VALUES (@cdkey8,@txtco_id8,@txtmat_id8," +  //1
                                           "@txtmat_amount_phurchase)";   //2

                        cmd2.Parameters.Add("@cdkey8", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id8", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtmat_id8", SqlDbType.NVarChar).Value = "";

                        cmd2.Parameters.Add("@txtmat_amount_phurchase", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n4}", 0));
                        //==============================
                        cmd2.ExecuteNonQuery();

                        //=========================================================================================================




                        trans.Commit();
                        conn.Close();

                        Cursor.Current = Cursors.Default;
                    }
                    //MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    catch (SqlException)
                    {
                        return;
                    }
                    //END เชื่อมต่อฐานข้อมูล=======================================================
                }
                //=============================================================

            }


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
            //================================
            if (W_ID_Select.M_USERNAME_TYPE == "4")
            {
                W_ID_Select.M_FORM_GRID = "Y";
                W_ID_Select.M_FORM_NEW = "Y";
                W_ID_Select.M_FORM_OPEN = "Y";
                W_ID_Select.M_FORM_PRINT = "Y";
                W_ID_Select.M_FORM_CANCEL = "Y";
                this.PANEL_FORM1.Visible = true;
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
                this.PANEL_FORM1.Visible = true;
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

        private void chmat_unit_status_CheckedChanged(object sender, EventArgs e)
        {
            if (this.check_mat_status.Checked == true)
            {
                this.txtchmat_unit_status.Text = "Y";
            }
            if (this.check_mat_status.Checked == false)
            {
                this.txtchmat_unit_status.Text = "N";
            }
        }

        private void PANEL105_MAT_UNIT3_btnmat_brand_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT2.Visible == false)
            {
                this.PANEL105_MAT_UNIT2.Visible = true;
                this.PANEL105_MAT_UNIT2.BringToFront();
                this.PANEL105_MAT_UNIT2.Location = new Point(103, this.PANEL105_MAT_UNIT3_txtmat_unit3_name.Location.Y + 22);
                SL = "3";

            }
            else
            {
                this.PANEL105_MAT_UNIT2.Visible = false;
            }
        }

        private void PANEL105_MAT_UNIT4_btnmat_brand_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT2.Visible == false)
            {
                this.PANEL105_MAT_UNIT2.Visible = true;
                this.PANEL105_MAT_UNIT2.BringToFront();
                this.PANEL105_MAT_UNIT2.Location = new Point(103, this.PANEL105_MAT_UNIT4_txtmat_unit4_name.Location.Y + 22);
                SL = "4";

            }
            else
            {
                this.PANEL105_MAT_UNIT2.Visible = false;
            }
        }

        private void PANEL105_MAT_UNIT5_btnmat_brand_Click(object sender, EventArgs e)
        {
            if (this.PANEL105_MAT_UNIT2.Visible == false)
            {
                this.PANEL105_MAT_UNIT2.Visible = true;
                this.PANEL105_MAT_UNIT2.BringToFront();
                this.PANEL105_MAT_UNIT2.Location = new Point(103, this.PANEL105_MAT_UNIT5_txtmat_unit5_name.Location.Y + 22);
                SL = "5";

            }
            else
            {
                this.PANEL105_MAT_UNIT2.Visible = false;
            }
        }

        //Tans_Log ====================================================================


    }
}
