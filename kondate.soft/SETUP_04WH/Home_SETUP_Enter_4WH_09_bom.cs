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
    public partial class Home_SETUP_Enter_4WH_09_bom : Form
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




        public Home_SETUP_Enter_4WH_09_bom()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_4WH_09_Load(object sender, EventArgs e)
        {
            W_ID_Select.M_FORM_NUMBER = "S409";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มBOMใหม่";
            this.txtbom_id.ReadOnly = false;

            this.ActiveControl = this.txtbom_id;

            PANEL_FORM1_GridView1();
            PANEL_FORM1_Fill_GridView1();

            this.BtnCancel_Doc.Enabled = false;
            this.BtnPrint.Enabled = false;

            Run_ID();
            CHECK_UP_NO999();
            Fill_Cbobom_type_name();
            MAT_Show_dataGridView1();

            //MAT===============================================================
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

            this.PANEL_MAT.Visible = false;
            //END MAT===========================================================


        }
        private void Home_SETUP_Enter_4WH_09_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                UPDATE_MAT_Show_dataGridView1();
                this.PANEL_MAT.Visible = false;
                PANEL_MAT_Clear_GridView1();
            }
        }

        private void Run_ID()
        {
            if (this.txtbom_no.Text == "")
            {
                this.txtbom_no.Text = "001";
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
                                    " FROM b001_09bom" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtbom_id <> '')" +
                                    " ORDER BY txtbom_no DESC";

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

                        RID = dt2.Rows[0]["txtbom_no"].ToString();      //1
                        Rid2 = Convert.ToDouble(RID);


                        Rid3 = Convert.ToDouble(string.Format("{0:n}", Rid2)) + Convert.ToDouble(string.Format("{0:n}", 1));
                        this.txtbom_no.Text = Rid3.ToString("00#");
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

        //ชื่อ BOM ====================================================================
        private void PANEL_FORM1_Fill_GridView1()
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

            PANEL_FORM1_Clear_GridView1();


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
                                    " ORDER BY txtbom_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtbom_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtbom_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbom_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtbom_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtbom_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtbom_status";

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtbom_no"].Value = dt2.Rows[j]["txtbom_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtbom_id"].Value = dt2.Rows[j]["txtbom_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtbom_name"].Value = dt2.Rows[j]["txtbom_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtbom_name_eng"].Value = dt2.Rows[j]["txtbom_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtbom_remark"].Value = dt2.Rows[j]["txtbom_remark"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtbom_status"].Value = dt2.Rows[j]["txtbom_status"].ToString();      //6

                        }
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();





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
        private void PANEL_FORM1_GridView1()
        {
            this.GridView1.ColumnCount = 7;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtbom_no";
            this.GridView1.Columns[2].Name = "Col_txtbom_id";
            this.GridView1.Columns[3].Name = "Col_txtbom_name";
            this.GridView1.Columns[4].Name = "Col_txtbom_name_eng";
            this.GridView1.Columns[5].Name = "Col_txtbom_remark";
            this.GridView1.Columns[6].Name = "Col_txtbom_status";

            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "ลำดับ";
            this.GridView1.Columns[2].HeaderText = " รหัสประเภท";
            this.GridView1.Columns[3].HeaderText = " ชื่อBOM";
            this.GridView1.Columns[4].HeaderText = "ชื่อBOM Eng";
            this.GridView1.Columns[5].HeaderText = " หมายเหตุ";
            this.GridView1.Columns[6].HeaderText = " สถานะ";

            this.GridView1.Columns[0].Visible = false;  //"No";
            this.GridView1.Columns[1].Visible = true;  //"Col_txtbom_no";
            this.GridView1.Columns[1].Width = 90;
            this.GridView1.Columns[1].ReadOnly = true;
            this.GridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[2].Visible = true;  //"Col_txtbom_id";
            this.GridView1.Columns[2].Width = 100;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[3].Visible = true;  //"Col_txtbom_name";
            this.GridView1.Columns[3].Width = 200;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[4].Visible = true;  //"Col_txtbom_name_eng";
            this.GridView1.Columns[4].Width = 200;
            this.GridView1.Columns[4].ReadOnly = true;
            this.GridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[5].Visible = true;  //"Col_txtbom_remark";
            this.GridView1.Columns[5].Width = 250;
            this.GridView1.Columns[5].ReadOnly = true;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[6].Visible = false;  //"Col_txtbom_status";
            this.GridView1.Columns[6].Width = 0;
            this.GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
                if (this.GridView1.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.GridView1.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.GridView1.Rows[i].Cells[7].Value = false;

                }
            }
        }
        private void PANEL_FORM1_dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.GridView1.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {

                    this.txtbom_no.Text = row.Cells[1].Value.ToString();
                    this.txtbom_id.Text = row.Cells[2].Value.ToString();
                    this.txtbom_name.Text = row.Cells[3].Value.ToString();

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

                        cmd2.CommandText = "SELECT b001_09bom.*," +
                                            "b001_08bom_type.*" +
                                            " FROM b001_09bom" +

                                            " INNER JOIN b001_08bom_type" +
                                            " ON b001_09bom.cdkey = b001_08bom_type.cdkey" +
                                            " AND b001_09bom.txtco_id = b001_08bom_type.txtco_id" +
                                            " AND b001_09bom.txtbom_type_id = b001_08bom_type.txtbom_type_id" +

                                            " WHERE (b001_09bom.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                            " AND (b001_09bom.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                             " AND (b001_09bom.txtbom_id = '" + this.txtbom_id.Text.Trim() + "')" +
                                           " ORDER BY b001_09bom.txtbom_no ASC";

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
                                    this.txtbom_no.Text = dt2.Rows[j]["txtbom_no"].ToString();      //1
                                    this.txtbom_id.Text = dt2.Rows[j]["txtbom_id"].ToString();      //2
                                    this.txtbom_name.Text = dt2.Rows[j]["txtbom_name"].ToString();      //3
                                    this.txtbom_name_eng.Text = dt2.Rows[j]["txtbom_name_eng"].ToString();      //4
                                    this.txtbom_remark.Text = dt2.Rows[j]["txtbom_remark"].ToString();      //5
                                    this.txtbom_type_id.Text = dt2.Rows[j]["txtbom_type_id"].ToString();      //6
                                    this.Cbobom_type_name.Text = dt2.Rows[j]["txtbom_type_name"].ToString();      //7
                                }
                                this.iblword_status.Text = "แก้ไขBOM";
                                this.txtbom_id.ReadOnly = true;
                                this.BtnCancel_Doc.Enabled = true;
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
            Fill_Show_Mat_dataGridView1();
        }
        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_FORM1_Fill_GridView1();
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
            Cursor.Current = Cursors.WaitCursor;

            PANEL_FORM1_Clear_GridView1();


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
                                     " AND (txtbom_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                   " ORDER BY txtbom_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtbom_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtbom_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtbom_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtbom_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtbom_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtbom_status";

                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtbom_no"].Value = dt2.Rows[j]["txtbom_no"].ToString();      //1
                            GridView1.Rows[index].Cells["Col_txtbom_id"].Value = dt2.Rows[j]["txtbom_id"].ToString();      //2
                            GridView1.Rows[index].Cells["Col_txtbom_name"].Value = dt2.Rows[j]["txtbom_name"].ToString();      //3
                            GridView1.Rows[index].Cells["Col_txtbom_name_eng"].Value = dt2.Rows[j]["txtbom_name_eng"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtbom_remark"].Value = dt2.Rows[j]["txtbom_remark"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtbom_status"].Value = dt2.Rows[j]["txtbom_status"].ToString();      //6

                        }
                        Cursor.Current = Cursors.Default;

                        PANEL_FORM1_Clear_GridView1_Up_Status();





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
        //ชื่อ BOM =====================================================================
        private void Fill_Cbobom_type_name()
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
                                  " FROM b001_08bom_type" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbom_type_id <> '')" +
                                  " ORDER BY txtbom_type_no";
                try
                {
                    //แบบที่ 1 ใช้ SqlDataReader =========================================================
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        string txtbom_type_name = dr.GetString(5);
                        this.Cbobom_type_name.Items.Add(txtbom_type_name);
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
        private void Fill_Cbobom_type_name2()
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
                                  " FROM b001_08bom_type" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbom_type_name = '" + this.Cbobom_type_name.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.txtbom_type_id.Text = dt.Rows[0]["txtbom_type_id"].ToString();
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
        private void Cbobom_type_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Cbobom_type_name2();
        }
        private void Fill_Cbobom_type_name_Edit()
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
                                  " FROM b001_08bom_type" +
                                  " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                  " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                  " AND (txtbom_type_id = '" + this.txtbom_type_id.Text.Trim() + "')";

                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.Cbobom_type_name.Text = dt.Rows[0]["txtbom_type_name"].ToString();
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
            var frm2 = new Home_SETUP_Enter_4WH_09_bom();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มBOMใหม่";
            this.txtbom_id.ReadOnly = false;
            Run_ID();
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

            if (this.txtbom_id.Text != "")
            {
                this.iblword_status.Text = "แก้ไขBOM";
                this.txtbom_id.ReadOnly = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.txtbom_no.Text == "")
            {
                MessageBox.Show("โปรดใส่ลำดับ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbom_no.Focus();
                return;
            }
            else
            {
                if (this.txtbom_no.TextLength == 3)
                {
                }
                else
                {
                    MessageBox.Show("โปรดใส่ลำดับ 3 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbom_no.Focus();
                    return;
                }
            }
            if (this.txtbom_id.Text == "")
            {
                MessageBox.Show("โปรดใส่รหัสประเภท ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbom_id.Focus();
                return;
            }
            if (this.txtbom_name.Text == "")
            {
                MessageBox.Show("โปรดใส่ชื่อBOMก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtbom_name.Focus();
                return;
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
            Cursor.Current = Cursors.WaitCursor;
            if (this.iblword_status.Text.Trim() == "เพิ่มBOMใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM b001_09bom" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                      " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (txtbom_id = '" + this.txtbom_id.Text.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("รหัสBOM นี้ซ้ำ   : '" + this.txtbom_id.Text.Trim() + "' โปรดใส่ใหม่ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtbom_id.Focus();
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มBOMใหม่")
                    {
                        //1
                        cmd2.CommandText = "INSERT INTO b001_09bom(cdkey,txtco_id," +  //1
                                           "txtbom_no,txtbom_id," +  //2
                                           "txtbom_name,txtbom_name_eng," +  //3
                                           "txtbom_remark," +  //4
                                          "txtbom_status,txtbom_type_id) " +  //5
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtbom_no,@txtbom_id," +  //2
                                           "@txtbom_name,@txtbom_name_eng," +  //3
                                           "@txtbom_remark," +  //4
                                          "@txtbom_status,@txtbom_type_id)";   //5

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbom_no", SqlDbType.NVarChar).Value = this.txtbom_no.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_id", SqlDbType.NVarChar).Value = this.txtbom_id.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_name", SqlDbType.NVarChar).Value = this.txtbom_name.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_name_eng", SqlDbType.NVarChar).Value = this.txtbom_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_remark", SqlDbType.NVarChar).Value = this.txtbom_remark.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_status", SqlDbType.NChar).Value = "0";
                        cmd2.Parameters.Add("@txtbom_type_id", SqlDbType.NVarChar).Value = this.txtbom_type_id.Text.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                        //2
                        //for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
                        //{
                        //    if (Convert.ToBoolean(this.PANEL_MAT_GridView1.Rows[i].Cells[9].Value) == true)
                        //    {
                        //        var index = Mat_dataGridView1.Rows.Add();
                        //        Mat_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[1].Value.ToString(); //1
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[2].Value.ToString(); //2
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[3].Value.ToString(); //3
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[4].Value.ToString(); //4
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtqty"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString(); //5
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtprice"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString(); //6
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value.ToString(); //7
                        //        Mat_dataGridView1.Rows[index].Cells["Col_txtsum_total"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value.ToString(); //8
                        //    }
                        //}

                        for (int i = 0; i < this.Mat_dataGridView1.Rows.Count; i++)
                        {
                            if (this.Mat_dataGridView1.Rows[i].Cells[2].Value != null)
                            {
                                if (this.Mat_dataGridView1.Rows[i].Cells[5].Value == null)
                                {
                                    this.Mat_dataGridView1.Rows[i].Cells[5].Value = "0";
                                }
                                    //if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[4].Value.ToString())) > 0)
                                    //{
                                    cmd2.CommandText = "INSERT INTO b001_09bom_detail(cdkey,txtco_id," +  //1
                                       "txtbom_id," +  //2
                                       "txtmat_id," +  //3
                                       "txtqty) " +  //4

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim()  + "'," +  //1
                                "'" + this.txtbom_id.Text.Trim() + "'," +  //2
                                "'" + this.Mat_dataGridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                                "'" + this.Mat_dataGridView1.Rows[i].Cells[5].Value.ToString() + "')";   //4
                                //==============================

                                cmd2.ExecuteNonQuery();
                                //}
                            }
                        }

                    }
                    if (this.iblword_status.Text.Trim() == "แก้ไขBOM")
                    {
                        cmd2.CommandText = "UPDATE b001_09bom SET " +
                                                                     "txtbom_no = '" + this.txtbom_no.Text.Trim() + "'," +
                                                                     "txtbom_name = '" + this.txtbom_name.Text.Trim() + "'," +
                                                                     "txtbom_name_eng = '" + this.txtbom_name_eng.Text.Trim() + "'," +
                                                                     "txtbom_remark = '" + this.txtbom_remark.Text.ToString() + "'," +
                                                                     "txtbom_type_id = '" + this.txtbom_type_id.Text.ToString() + "'" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtbom_id = '" + this.txtbom_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();


                        cmd2.CommandText = "DELETE FROM b001_09bom_detail" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtbom_id = '" + this.txtbom_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();

                        for (int i = 0; i < this.Mat_dataGridView1.Rows.Count; i++)
                        {
                            if (this.Mat_dataGridView1.Rows[i].Cells[2].Value != null)
                            {
                                if (this.Mat_dataGridView1.Rows[i].Cells[5].Value == null)
                                {
                                    this.Mat_dataGridView1.Rows[i].Cells[5].Value = "0";
                                }
                                //if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[4].Value.ToString())) > 0)
                                //{
                                cmd2.CommandText = "INSERT INTO b001_09bom_detail(cdkey,txtco_id," +  //1
                                   "txtbom_id," +  //2
                                   "txtmat_id," +  //3
                                   "txtqty) " +  //4

                            "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "'," +  //1
                            "'" + this.txtbom_id.Text.Trim() + "'," +  //2
                            "'" + this.Mat_dataGridView1.Rows[i].Cells[2].Value.ToString() + "'," +  //3
                            "'" + this.Mat_dataGridView1.Rows[i].Cells[5].Value.ToString() + "')";   //4
                                //==============================

                                cmd2.ExecuteNonQuery();
                                //}
                            }
                        }


                    }
                    Cursor.Current = Cursors.Default;

                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มBOMใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }
                        if (this.iblword_status.Text.Trim() == "แก้ไขBOM")
                        {
                            W_ID_Select.LOG_ID = "6";
                            W_ID_Select.LOG_NAME = "บันทึกแก้ไข";
                            TRANS_LOG();
                        }
                        Cursor.Current = Cursors.Default;

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtbom_id.Text = "";
                        this.txtbom_name.Text = "";
                        this.txtbom_name_eng.Text = "";
                        this.txtbom_remark.Text = "";

                        PANEL_FORM1_Fill_GridView1();

                        this.iblword_status.Text = "เพิ่มBOMใหม่";
                        this.txtbom_id.ReadOnly = false;

                        Run_ID();

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




                        cmd2.CommandText = "INSERT INTO b001_09bom_cancel(cdkey,txtco_id,txtbranch_id," +  //1
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

                        cmd2.CommandText = "INSERT INTO b001_09bom_cancel_detail(cdkey,txtco_id," +  //1
                                             "txtbom_no,txtbom_id," +  //2
                                             "txtbom_name,txtbom_name_eng," +  //3
                                             "txtbom_remark," +  //4
                                            "txtbom_status,txtbom_type_id,cancel_id) " +  //5
                                             "VALUES (@cdkey2,@txtco_id2," +  //1
                                             "@txtbom_no,@txtbom_id," +  //2
                                             "@txtbom_name,@txtbom_name_eng," +  //3
                                             "@txtbom_remark," +  //4
                                            "@txtbom_status,@txtbom_type_id,@cancel_id2)";   //5

                        cmd2.Parameters.Add("@cdkey2", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id2", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbom_no", SqlDbType.NVarChar).Value = this.txtbom_no.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_id", SqlDbType.NVarChar).Value = this.txtbom_id.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_name", SqlDbType.NVarChar).Value = this.txtbom_name.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_name_eng", SqlDbType.NVarChar).Value = this.txtbom_name_eng.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_remark", SqlDbType.NVarChar).Value = this.txtbom_remark.Text.ToString();
                        cmd2.Parameters.Add("@txtbom_status", SqlDbType.NChar).Value = "0";
                        cmd2.Parameters.Add("@txtbom_type_id", SqlDbType.NVarChar).Value = this.txtbom_type_id.Text.ToString();
                        cmd2.Parameters.Add("@cancel_id2", SqlDbType.NVarChar).Value = Cancel_ID.ToString();
                        //==============================

                        cmd2.ExecuteNonQuery();

                        cmd2.CommandText = "DELETE FROM b001_09bom" +
                                                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                                   " AND (txtbom_id = '" + this.txtbom_id.Text.Trim() + "')";
                        cmd2.ExecuteNonQuery();


                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการ ยกเลิกเอกสาร รหัส  " + this.txtbom_id.Text.ToString() + " ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
                        this.txtbom_id.Text = "";
                        this.txtbom_name.Text = "";

                        PANEL_FORM1_Fill_GridView1();

                        this.iblword_status.Text = "เพิ่มBOMใหม่";
                        this.txtbom_id.ReadOnly = false;

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

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void txtbom_id_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == (char)Keys.Enter && this.txtbom_id.Text == "")
            {
                this.txtbom_id.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtbom_id.Text.Trim() != "")
            {
                this.txtbom_no.Focus();

            }

        }
        private void txtbom_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtbom_no.Text == "")
            {
                this.txtbom_no.Focus();
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtbom_no.Text.Trim() != "")
            {
                if (this.txtbom_no.TextLength == 3)
                {
                    this.txtbom_name.Focus();
                }
                else
                {
                    MessageBox.Show("โปรดใส่ลำดับให้ครับ  3 หลัก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtbom_no.Focus();
                    return;
                }
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtbom_no.Text.Length == 0)
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
        private void txtbom_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtbom_name_eng.Focus();

        }

        private void txtbom_name_eng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.txtbom_remark.Focus();

        }

        private void txtbom_remark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.BtnSave.Focus();

        }

        private void btnadd_mat_Click(object sender, EventArgs e)
        {
            this.PANEL_MAT_iblword_top.Text = "เลือกรายการสินค้า";
            if (this.PANEL_MAT.Visible == false)
            {
                this.PANEL_MAT.Visible = true;
                this.PANEL_MAT.BringToFront();
                this.PANEL_MAT.Location = new Point(this.panel_button_top_pictureBox.Location.X, this.panel_button_top_pictureBox.Location.Y);
            }
            else
            {
                this.PANEL_MAT.Visible = false;
            }

        }

        private void btnremove_row_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("คุณต้องการ ลบรายการแถว ที่คลิ๊ก ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                //DataGridViewRow row = new DataGridViewRow();
                //row = this.PANEL161_SUP_dataGridView2.Rows[selectedRowIndex];
                this.Mat_dataGridView1.Rows.RemoveAt(selectedRowIndex);
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
        //รายละเอียด BOM =======================================
        DataTable table = new DataTable();
        int selectedRowIndex;
        private void Mat_dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
            this.btnremove_row.Visible = true;

        }
        private void UPDATE_MAT_Show_dataGridView1()
        {
            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
            {
                if (Convert.ToBoolean(this.PANEL_MAT_GridView1.Rows[i].Cells[9].Value) == true)
                     {
                            var index = Mat_dataGridView1.Rows.Add();
                            Mat_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[1].Value.ToString(); //1
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[2].Value.ToString(); //2
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[3].Value.ToString(); //3
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[4].Value.ToString(); //4
                            Mat_dataGridView1.Rows[index].Cells["Col_txtqty"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString(); //5
                            Mat_dataGridView1.Rows[index].Cells["Col_txtprice"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString(); //6
                            Mat_dataGridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value.ToString(); //7
                            Mat_dataGridView1.Rows[index].Cells["Col_txtsum_total"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value.ToString(); //8
                     }
            }
        }
        private void Fill_Show_Mat_dataGridView1()
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

            MAT_Clear_dataGridView1();


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
                                    "b001mat_06price_sale.*," +
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

                                    " INNER JOIN b001mat_06price_sale" +
                                    " ON b001_09bom_detail.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001_09bom_detail.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001_09bom_detail.txtmat_id = b001mat_06price_sale.txtmat_id" +

                                    " INNER JOIN b001_05mat_unit1" +
                                    " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                    " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                    " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                    " WHERE (b001_09bom_detail.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (b001_09bom_detail.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (b001_09bom_detail.txtbom_id = '" + this.txtbom_id.Text.Trim() + "')" +
                                    " ORDER BY b001_09bom_detail.ID ASC";

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);

                    int s = 1;
                    if (dt2.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            s = j + 1;
                            //Mat_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[1].Value.ToString(); //1
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[2].Value.ToString(); //2
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[3].Value.ToString(); //3
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[4].Value.ToString(); //4
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtqty"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[5].Value.ToString(); //5
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtprice"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[6].Value.ToString(); //6
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[7].Value.ToString(); //7
                            //Mat_dataGridView1.Rows[index].Cells["Col_txtsum_total"].Value = this.PANEL_MAT_GridView1.Rows[i].Cells[8].Value.ToString(); //8

                            var index = Mat_dataGridView1.Rows.Add();
                            Mat_dataGridView1.Rows[index].Cells["Col_Auto_num"].Value = s.ToString() ; //0
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            Mat_dataGridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            Mat_dataGridView1.Rows[index].Cells["Col_txtqty"].Value = dt2.Rows[j]["txtqty"].ToString();      //5
                            Mat_dataGridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");       //6
                            Mat_dataGridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = "0";       //7
                            Mat_dataGridView1.Rows[index].Cells["Col_txtsum_total"].Value = "0";      //8

                        }
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
        private void MAT_Show_dataGridView1()
        {
            this.Mat_dataGridView1.ColumnCount = 9;
            this.Mat_dataGridView1.Columns[0].Name = "Col_Auto_num";
            this.Mat_dataGridView1.Columns[1].Name = "Col_txtmat_no";
            this.Mat_dataGridView1.Columns[2].Name = "Col_txtmat_id";
            this.Mat_dataGridView1.Columns[3].Name = "Col_txtmat_name";
            this.Mat_dataGridView1.Columns[4].Name = "Col_txtmat_unit1_name";
            this.Mat_dataGridView1.Columns[5].Name = "Col_txtqty";
            this.Mat_dataGridView1.Columns[6].Name = "Col_txtprice";
            this.Mat_dataGridView1.Columns[7].Name = "Col_txtdiscount_money";
            this.Mat_dataGridView1.Columns[8].Name = "Col_txtsum_total";

            this.Mat_dataGridView1.Columns[0].HeaderText = "No";
            this.Mat_dataGridView1.Columns[1].HeaderText = "ลำดับ";
            this.Mat_dataGridView1.Columns[2].HeaderText = " รหัส";
            this.Mat_dataGridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.Mat_dataGridView1.Columns[4].HeaderText = " หน่วยนับ";
            this.Mat_dataGridView1.Columns[5].HeaderText = " จำนวน";
            this.Mat_dataGridView1.Columns[6].HeaderText = " ราคา/หน่วย(บาท)";
            this.Mat_dataGridView1.Columns[7].HeaderText = " ส่วนลด(บาท)";
            this.Mat_dataGridView1.Columns[8].HeaderText = " จำนวนเงิน(บาท)";

            this.Mat_dataGridView1.Columns[0].Visible = true;  //"Col_Auto_num";
            this.Mat_dataGridView1.Columns[0].Width = 36;
            this.Mat_dataGridView1.Columns[0].ReadOnly = true;
            this.Mat_dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.Mat_dataGridView1.Columns[1].Visible = true;  //"Col_txtmat_no";

            this.Mat_dataGridView1.Columns[2].Visible = true;  //"Col_txtmat_id";
            this.Mat_dataGridView1.Columns[2].Width = 100;
            this.Mat_dataGridView1.Columns[2].ReadOnly = true;
            this.Mat_dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.Mat_dataGridView1.Columns[3].Visible = true;  //"Col_txtmat_name";
            this.Mat_dataGridView1.Columns[3].Width = 150;
            this.Mat_dataGridView1.Columns[3].ReadOnly = true;
            this.Mat_dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.Mat_dataGridView1.Columns[4].Visible = true;  //"Col_txtmat_unit1_name";
            this.Mat_dataGridView1.Columns[4].Width = 100;
            this.Mat_dataGridView1.Columns[4].ReadOnly = true;
            this.Mat_dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.Mat_dataGridView1.Columns[5].Visible = true;  //"Col_txtqty";
            this.Mat_dataGridView1.Columns[5].Width = 100;
            this.Mat_dataGridView1.Columns[5].ReadOnly = false;
            this.Mat_dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Mat_dataGridView1.Columns[6].Visible = true;  //"Col_txtprice";
            this.Mat_dataGridView1.Columns[6].Width = 100;
            this.Mat_dataGridView1.Columns[6].ReadOnly = true;
            this.Mat_dataGridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Mat_dataGridView1.Columns[7].Visible = false;  //"Col_txtdiscount_money";
            this.Mat_dataGridView1.Columns[7].Width = 0;
            this.Mat_dataGridView1.Columns[7].ReadOnly = true;
            this.Mat_dataGridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.Mat_dataGridView1.Columns[8].Visible = false;  //"Col_txtsum_total";
            this.Mat_dataGridView1.Columns[8].Width = 0;
            this.Mat_dataGridView1.Columns[8].ReadOnly = true;
            this.Mat_dataGridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.Mat_dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.Mat_dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.Mat_dataGridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.Mat_dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.Mat_dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.Mat_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.Mat_dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.Mat_dataGridView1.EnableHeadersVisualStyles = false;


        }
        private void MAT_Clear_dataGridView1()
        {
            this.Mat_dataGridView1.Rows.Clear();
            this.Mat_dataGridView1.Refresh();
        }
        //รายละเอียด BOM =======================================

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

                cmd1.CommandText = "SELECT *" +
                                    " FROM b001_09bom" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtbom_id = '')" +
                                    " ORDER BY txtbom_no ASC";

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
                        cmd2.CommandText = "INSERT INTO b001_09bom(cdkey,txtco_id," +  //1
                                           "txtbom_no,txtbom_id," +  //2
                                           "txtbom_name,txtbom_name_eng," +  //3
                                           "txtbom_remark," +  //4
                                          "txtbom_status) " +  //5
                                           "VALUES (@cdkey,@txtco_id," +  //1
                                           "@txtbom_no,@txtbom_id," +  //2
                                           "@txtbom_name,@txtbom_name_eng," +  //3
                                           "@txtbom_remark," +  //4
                                          "@txtbom_status)";   //5

                        cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                        cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                        cmd2.Parameters.Add("@txtbom_no", SqlDbType.NVarChar).Value = "999";
                        cmd2.Parameters.Add("@txtbom_id", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtbom_name", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtbom_name_eng", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtbom_remark", SqlDbType.NVarChar).Value = "";
                        cmd2.Parameters.Add("@txtbom_status", SqlDbType.NChar).Value = "0";
                        //==============================

                        cmd2.ExecuteNonQuery();



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
                                this.PANEL_FORM1.Visible = false;
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

                        this.PANEL_FORM1.Visible = false;
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

        //Tans_Log ====================================================================


        //MAT=====================================================================================================================================


        //PA55NEL_55MAT555====================================================

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

                            //this.PANEL_MAT_GridView1.ColumnCount = 9;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

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

                            //this.PANEL_MAT_GridView1.ColumnCount = 9;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

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

                            //this.PANEL_MAT_GridView1.ColumnCount = 9;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

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

                            //this.PANEL_MAT_GridView1.ColumnCount = 9;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

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

                            //this.PANEL_MAT_GridView1.ColumnCount = 9;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

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

        private void PANEL_MAT_btnupdate_Click(object sender, EventArgs e)
        {
            UPDATE_MAT_Show_dataGridView1();
            this.PANEL_MAT.Visible = false;
            PANEL_MAT_Clear_GridView1();
        }

        //เลือกรายการสินค้า==============================================================================
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
                                    "b001mat_06price_sale.*," +
                                    "b001_05mat_unit1.*" +
                                    " FROM b001mat" +

                                    " INNER JOIN b001mat_02detail" +
                                    " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                    " INNER JOIN b001mat_06price_sale" +
                                    " ON b001mat.cdkey = b001mat_06price_sale.cdkey" +
                                    " AND b001mat.txtco_id = b001mat_06price_sale.txtco_id" +
                                    " AND b001mat.txtmat_id = b001mat_06price_sale.txtmat_id" +

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

                            //this.PANEL_MAT_GridView1.ColumnCount = 9;
                            //this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
                            //this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
                            //this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
                            //this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
                            //this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
                            //this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
                            //this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
                            //this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
                            //this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

                            var index = PANEL_MAT_GridView1.Rows.Add();
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //1
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //2
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //3
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //4
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtqty"].Value = "0";      //5
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_price_sale1"]).ToString("###,###.00");        //6
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = ".00";      //7
                            PANEL_MAT_GridView1.Rows[index].Cells["Col_txtsum_total"].Value = ".00";      //8
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
            this.PANEL_MAT_GridView1.ColumnCount = 9;
            this.PANEL_MAT_GridView1.Columns[0].Name = "Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[1].Name = "Col_txtmat_no";
            this.PANEL_MAT_GridView1.Columns[2].Name = "Col_txtmat_id";
            this.PANEL_MAT_GridView1.Columns[3].Name = "Col_txtmat_name";
            this.PANEL_MAT_GridView1.Columns[4].Name = "Col_txtmat_unit1_name";
            this.PANEL_MAT_GridView1.Columns[5].Name = "Col_txtqty";
            this.PANEL_MAT_GridView1.Columns[6].Name = "Col_txtprice";
            this.PANEL_MAT_GridView1.Columns[7].Name = "Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[8].Name = "Col_txtsum_total";

            this.PANEL_MAT_GridView1.Columns[0].HeaderText = "No";
            this.PANEL_MAT_GridView1.Columns[1].HeaderText = "ลำดับ";
            this.PANEL_MAT_GridView1.Columns[2].HeaderText = " รหัส";
            this.PANEL_MAT_GridView1.Columns[3].HeaderText = " ชื่อสินค้า";
            this.PANEL_MAT_GridView1.Columns[4].HeaderText = " หน่วยนับ";
            this.PANEL_MAT_GridView1.Columns[5].HeaderText = " จำนวน";
            this.PANEL_MAT_GridView1.Columns[6].HeaderText = " ราคา/หน่วย(บาท)";
            this.PANEL_MAT_GridView1.Columns[7].HeaderText = " ส่วนลด(บาท)";
            this.PANEL_MAT_GridView1.Columns[8].HeaderText = " จำนวนเงิน(บาท)";

            this.PANEL_MAT_GridView1.Columns[0].Visible = true;  //"Col_Auto_num";
            this.PANEL_MAT_GridView1.Columns[0].Width = 36;
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
            this.PANEL_MAT_GridView1.Columns[3].Width = 350;
            this.PANEL_MAT_GridView1.Columns[3].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
            this.PANEL_MAT_GridView1.Columns[6].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[7].Visible = false;  //"Col_txtdiscount_money";
            this.PANEL_MAT_GridView1.Columns[7].Width = 0;
            this.PANEL_MAT_GridView1.Columns[7].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.PANEL_MAT_GridView1.Columns[8].Visible = false;  //"Col_txtsum_total";
            this.PANEL_MAT_GridView1.Columns[8].Width = 0;
            this.PANEL_MAT_GridView1.Columns[8].ReadOnly = true;
            this.PANEL_MAT_GridView1.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL_MAT_GridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.PANEL_MAT_GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL_MAT_GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL_MAT_GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL_MAT_GridView1.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "เลือกรายการ";
            dgvCmb.ReadOnly = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL_MAT_GridView1.Columns.Add(dgvCmb);

        }
        private void PANEL_MAT_Clear_GridView1()
        {
            this.PANEL_MAT_GridView1.Rows.Clear();
            this.PANEL_MAT_GridView1.Refresh();
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

                PANEL_MAT_GridView1.Rows[i].Cells[2].Style.BackColor = Color.LightSkyBlue;
                PANEL_MAT_GridView1.Rows[i].Cells[3].Style.BackColor = Color.LightGoldenrodYellow;
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


            for (int i = 0; i < this.PANEL_MAT_GridView1.Rows.Count - 0; i++)
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

                    this.PANEL_MAT_txtcount_rows.Text = k.ToString();
                }
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
        //เลือกรายการสินค้า===============================================================================

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


        //=================================================================================
    }
}
