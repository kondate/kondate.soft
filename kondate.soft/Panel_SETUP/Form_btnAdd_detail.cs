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

namespace kondate.soft.Panel_SETUP
{
    public partial class Form_btnAdd_detail : Form
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



        public Form_btnAdd_detail()
        {
            InitializeComponent();

            PANEL_MAT_GridView1.Controls.Add(dtp);
            dtp.Visible = false;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.TextChanged += new EventHandler(dtp_TextChange);

        }

        private void Form_btnAdd_detail_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();
            W_ID_Select.M_COID = "01";





            PANEL_MAT_Show_GridView1();
            Fill_PANEL_MAT_GridView1();
            PANEL_MAT_GridView1_Color_Column();
            PANEL_MAT_GridView1_Cal_Sum();

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

        //MAT=====================================================================================================================================
        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle;

        bool allowResize = false;

        //PANEL_MAT====================================================
        private Point MouseDownLocation;
        private void PANEL_MAT_iblword_top_MouseDown(object sender, MouseEventArgs e)
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
        private void PANEL_MAT_panel_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
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
        private void PANEL_MAT_btnclose_Click(object sender, EventArgs e)
        {
            this.PANEL_MAT.Visible = false;
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
        //END PANEL_MAT====================================================

        private void btnadd_detail_Click(object sender, EventArgs e)
        {
            this.PANEL_MAT.Visible = true;
            this.PANEL_MAT.BringToFront();
        }

        private void PANEL_MAT_btnGo_Click(object sender, EventArgs e)
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


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo2_Click(object sender, EventArgs e)
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


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
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
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo3_Click(object sender, EventArgs e)
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


                if (this.cboSearch.Text.Trim() == "รหัสสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                        "b001_05mat_unit1.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                            " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtsearch.Text.Trim() + "')" +
                                       " ORDER BY b001mat.txtmat_no ASC";

                }
                else if (this.cboSearch.Text.Trim() == "ชื่อสินค้า")
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                        "b001_05mat_unit1.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                            " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                         " AND (b001mat.txtmat_name LIKE '%" + this.PANEL_MAT_txtsearch.Text.Trim() + "%')" +
                                       " ORDER BY b001mat.txtmat_no ASC";
                }
                else
                {
                    cmd2.CommandText = "SELECT b001mat.*," +
                                        "b001mat_02detail.*," +
                                        "b001_05mat_unit1.*" +
                                        " FROM b001mat" +

                                        " INNER JOIN b001mat_02detail" +
                                        " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                        " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                        " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                        " INNER JOIN b001_05mat_unit1" +
                                        " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                        " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                        " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo4_Click(object sender, EventArgs e)
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


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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
            PANEL_MAT_GridView1_Cal_Sum();


        }

        private void PANEL_MAT_btnGo5_Click(object sender, EventArgs e)
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


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


                                    " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001mat.txtmat_id <> '')" +
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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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
            PANEL_MAT_GridView1_Cal_Sum();

        }

        private void PANEL_MAT_btnGo6_Click(object sender, EventArgs e)
        {

        }

        private void Fill_PANEL_MAT_GridView1()
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


                cmd2.CommandText = "SELECT b001mat.*," +
                                    "b001mat_02detail.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +


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
                            this.PANEL_MAT_txtcount_rows.Text = dt2.Rows.Count.ToString();

                            //this.PANEL_MAT_GridView1.ColumnCount = 10;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
                            //this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = ".00";      //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_date"].Value = "";      //9
                        }
                        //=======================================================
                        Cursor.Current = Cursors.Default;


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

        }
        private void PANEL_MAT_Show_GridView1()
        {
            this.PANEL_MAT_GridView1.ColumnCount = 10;
            this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
            this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
            this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
            this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
            this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";
            this.PANEL_MAT_GridView1.Columns[9].Name = "Col_date";

            this.PANEL_MAT_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_MAT_GridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL_MAT_GridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL_MAT_GridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.PANEL_MAT_GridView1.Columns[4].HeaderText = " หน่วยนับ";
            this.PANEL_MAT_GridView1.Columns[5].HeaderText = " จำนวน";
            this.PANEL_MAT_GridView1.Columns[6].HeaderText = " ราคา/หน่วย(บาท)";
            this.PANEL_MAT_GridView1.Columns[7].HeaderText = " ส่วนลด(บาท)";
            this.PANEL_MAT_GridView1.Columns[8].HeaderText = " จำนวนเงิน(บาท)";
            this.PANEL_MAT_GridView1.Columns[9].HeaderText = " วันที่ต้องการสินค้า";

            this.PANEL_MAT_GridView1.Columns[0].Visible = true;  //"Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[0].Width =36;
            this.PANEL_MAT_GridView1.Columns[0].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns[1].Visible = true;  //"Col_txtmat_no";

            this.PANEL_MAT_GridView1.Columns[2].Visible = true;  //"Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns[2].Width = 100;
            this.PANEL_MAT_GridView1.Columns[2].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns[3].Visible = true;  //"Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns[3].Width = 150;
            this.PANEL_MAT_GridView1.Columns[3].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL_MAT_GridView1.Columns[4].Visible = true;  //"Col_txtmat_unit1_name";
            this.PANEL_MAT_GridView1.Columns[4].Width = 100;
            this.PANEL_MAT_GridView1.Columns[4].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL_MAT_GridView1.Columns[5].Visible = true;  //"Col_txtqty";
            this.PANEL_MAT_GridView1.Columns[5].Width = 100;
            this.PANEL_MAT_GridView1.Columns[5].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[6].Visible = true;  //"Col_txtprice";
            this.PANEL_MAT_GridView1.Columns[6].Width = 100;
            this.PANEL_MAT_GridView1.Columns[6].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[7].Visible = true;  //"Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[7].Width = 100;
            this.PANEL_MAT_GridView1.Columns[7].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[8].Visible = true;  //"Col_txtsum_total";
            this.PANEL_MAT_GridView1.Columns[8].Width = 150;
            this.PANEL_MAT_GridView1.Columns[8].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[9].Visible = true;  //"Col_date";
            this.PANEL_MAT_GridView1.Columns[9].Width = 150;
            this.PANEL_MAT_GridView1.Columns[9].ReadOnly = false;
            this.PANEL_MAT_GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
        private void PANEL_MAT_GridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (PANEL_MAT_GridView1.Columns[e.ColumnIndex].Name)
            {
                case "Col_date":

                    _Rectangle = PANEL_MAT_GridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                    dtp.Size = new Size(_Rectangle.Width, _Rectangle.Height); //  
                    dtp.Location = new Point(_Rectangle.X, _Rectangle.Y); //  
                    dtp.Visible = true;
                    break;
                case "Col_txtqty":
                    dtp.Visible = false;
                    break;
                case "Col_txtprice":
                    dtp.Visible = false;
                    break;
                case "Col_txtdiscount_money":
                    dtp.Visible = false;
                    break;
                case "Col_txtsum_total":
                    dtp.Visible = false;
                    break;


            }
        }
        private void dtp_TextChange(object sender, EventArgs e)
        {
            //PANEL_MAT_GridView1.CurrentCell.Value = dtp.Value.ToString("dd-MM-yyyy", UsaCulture);
            PANEL_MAT_GridView1.CurrentCell.Value = dtp.Value.ToString("yyyy-MM-dd", UsaCulture);
        }
        private void PANEL_MAT_GridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtp.Visible = false;
        }
        private void PANEL_MAT_GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            dtp.Visible = false;
        }
        private void PANEL_MAT_GridView1_Color_Column()
        {
            int rowscount = PANEL_MAT_GridView1.Rows.Count;

            for (int i = 0; i < rowscount; i++)
            {
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[5].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightGoldenrodYellow;
                //}
                //if (!(PANEL_MAT_GridView1.Rows[i].Cells[9].Value == null))
                //{
                //    PANEL_MAT_GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightGoldenrodYellow;
                //}

                PANEL_MAT_GridView1.Rows[i].Cells[5].Style.BackColor = Color.LightSkyBlue;
                PANEL_MAT_GridView1.Rows[i].Cells[6].Style.BackColor = Color.LightGoldenrodYellow;
                PANEL_MAT_GridView1.Rows[i].Cells[9].Style.BackColor = Color.LightSkyBlue;

            }
        }
        private void PANEL_MAT_GridView1_Cal_Sum()
        {
            double Sum_Total = 0;
            double Sum_Qty = 0;
            double Sum_Price = 0;
            double Sum_Discount = 0;
            double MoneySum = 0;
            int k = 0;


            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count-0; i++)
            {
                k = 1 + i;

                var valu = this.PANEL_MAT_GridView1.Rows[i].Cells[2].Value.ToString();

                if (valu != "")
                {
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[0].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[0].Value = k.ToString();
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value = "0";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value = "0";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value = "0";
                    }
                    if (this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value == null)
                    {
                        this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value = "0";
                    }

                    //5 * 6 = 8

                    //Sum_Total  =================================================
                    Sum_Total = Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString()));
                    this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value = Sum_Total.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty  =================================================
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString()));
                    this.PANEL_MAT_txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //Sum_Price  =================================================
                    Sum_Price = Convert.ToDouble(string.Format("{0:n}", Sum_Price)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString()));
                    this.PANEL_MAT_txtsum_price.Text = Sum_Price.ToString("N", new CultureInfo("en-US"));

                    //Sum_Discount  =================================================
                    Sum_Discount = Convert.ToDouble(string.Format("{0:n}", Sum_Discount)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value.ToString()));
                    this.PANEL_MAT_txtsum_discount.Text = Sum_Discount.ToString("N", new CultureInfo("en-US"));

                    //MoneySum  =================================================
                    MoneySum = Convert.ToDouble(string.Format("{0:n}", MoneySum)) + Convert.ToDouble(string.Format("{0:n}", this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value.ToString()));
                    this.PANEL_MAT_txtmoney_sum.Text = MoneySum.ToString("N", new CultureInfo("en-US"));

                    this.PANEL_MAT_txtcount_rows.Text =k.ToString();
                }
            }

        }
        private void Form_btnAdd_detail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                this.ActiveControl = this.PANEL_MAT_txtsearch;
                this.PANEL_MAT_txtsearch.Text = "OK";
            }
        }
        private void PANEL_MAT_GridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt = e.Control as TextBox;
            txt.PreviewKeyDown += new PreviewKeyDownEventHandler(txt_PreviewKeyDown);
        }
        void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                PANEL_MAT_GridView1_Cal_Sum();
            }
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


        //========================================




        //====================================

        //END_MAT=====================================================================================================================================
    }
}
