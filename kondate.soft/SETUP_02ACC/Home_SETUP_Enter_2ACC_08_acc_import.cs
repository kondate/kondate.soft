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

namespace kondate.soft.SETUP_2ACC
{
    public partial class Home_SETUP_Enter_2ACC_08_acc_import : Form
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




        public Home_SETUP_Enter_2ACC_08_acc_import()
        {
            InitializeComponent();
        }

        private void Home_SETUP_Enter_2ACC_08_1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //this.btnmaximize.Visible = false;
            //this.btnmaximize_full.Visible = true;

            W_ID_Select.M_FORM_NUMBER = "S2081";
            CHECK_ADD_FORM();

            CHECK_USER_RULE();

            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";


            W_ID_Select.LOG_ID = "1";
            W_ID_Select.LOG_NAME = "Login";
            TRANS_LOG();

            this.iblword_status.Text = "เพิ่มบัญชีใหม่";

            PANEL_FORM1_GridView1_acc();
            //PANEL_FORM1_Fill_acc();

            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            PANEL1_CO_GridView1_co();
            PANEL1_CO_Fill_CO();

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
            var frm2 = new Home_SETUP_Enter_2ACC_08_acc_import();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "เพิ่มบัญชีใหม่";

        }

        private void btnopen_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
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
            if (this.iblword_status.Text.Trim() == "เพิ่มบัญชีใหม่")
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = conn;

                    cmd1.CommandText = "SELECT * FROM k013db_1acc" +
                                       " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                        " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                       " AND (lang_id = '" + W_ID_Select.Lang.Trim() + "')";
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("กิจการนี้ รหัสกิจการ  : '" +dt.Rows[0]["txtco_id"].ToString() + "'  เคยมีการ เพิ่มรหัสบัญชีไปแล้ว ไม่สามารถนำเข้าใหม่ได้ ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        return;
                    }
                }

                //
                conn.Close();
            }
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
                    if (this.iblword_status.Text.Trim() == "เพิ่มบัญชีใหม่")
                    {
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[0].Name = "Col_Auto_num";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[1].Name = "Col_txtacc_id";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[2].Name = "Col_txtacc_name";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[3].Name = "Col_txtacc_name_eng";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[4].Name = "Col_txtacc_type_id";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[5].Name = "Col_txtacc_type_name";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[6].Name = "Col_txtacc_work_type_id";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[7].Name = "Col_txtacc_work_type_name";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[8].Name = "Col_txtacc_degree_id";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[9].Name = "Col_txtacc_degree_name";

                        //this.PANEL_FORM1_dataGridView1_acc.Columns[10].Name = "Col_txtacc_name2";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[11].Name = "Col_txtacc_name_eng2";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[12].Name = "Col_txtacc_balance_type_id";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[13].Name = "Col_txtacc_id_control";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[14].Name = "Col_txtacc_name_control";
                        //this.PANEL_FORM1_dataGridView1_acc.Columns[15].Name = "Col_txttype_money_id";

                        for (int i = 0; i < this.GridView1.Rows.Count ; i++)
                        {
                            if (this.GridView1.Rows[i].Cells[1].Value != null)
                            {
                                //if (Convert.ToDouble(string.Format("{0:n4}", this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[4].Value.ToString())) > 0)
                                //{
                                cmd2.CommandText = "INSERT INTO k013db_1acc(cdkey,txtco_id,txtbranch_id," +  //1
                                       "txtacc_id," +  //2
                                       "txtacc_name," +  //3
                                       "txtacc_name_eng," +  //4
                                       "txtacc_type_id," +  //5
                                       "txtacc_work_type_id," +  //6
                                       "txtacc_balance_type_id," +  //7
                                       "txtacc_degree_id," +  //8
                                       "txtacc_id_control," +   //9
                                       "txtacc_name_control," +  //10
                                       "txttype_money_id," +  //11
                                       "lang_id) " +  //12

                                //"VALUES (@cdkey,@txtco_id,@txtbranch_id," +
                                //"@txtacc_id,@txtacc_name,@txtacc_name_eng," +
                                //"@txtacc_type_id,@txtacc_work_type_id," +
                                //"@txtacc_balance_type_id,@txtacc_degree_id," +
                                //"@txtacc_id_control,@txtacc_name_control,@txttype_money_id,@lang_id)";

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + this.GridView1.Rows[i].Cells[1].Value.ToString() + "'," +  //2
                                "'" + this.GridView1.Rows[i].Cells[10].Value.ToString() + "'," +  //3
                                "'" + this.GridView1.Rows[i].Cells[11].Value.ToString() + "'," +  //4
                                "'" + this.GridView1.Rows[i].Cells[4].Value.ToString() + "'," +    //5
                                "'" + this.GridView1.Rows[i].Cells[6].Value.ToString() + "'," +  //6
                                "'" + this.GridView1.Rows[i].Cells[12].Value.ToString() + "'," +  //7
                                "'" + this.GridView1.Rows[i].Cells[8].Value.ToString() + "'," +  //8
                                "'" + this.GridView1.Rows[i].Cells[13].Value.ToString() + "'," +  //9
                                "'" + this.GridView1.Rows[i].Cells[14].Value.ToString() + "'," +  //10
                                "'" + this.GridView1.Rows[i].Cells[15].Value.ToString() + "'," +  //11
                                "'" + W_ID_Select.Lang.Trim() + "')";   //12

                                    //cmd2.Parameters.Add("@cdkey", SqlDbType.NVarChar).Value = W_ID_Select.CDKEY.Trim();
                                    //cmd2.Parameters.Add("@txtco_id", SqlDbType.NVarChar).Value = W_ID_Select.M_COID.Trim();
                                    //cmd2.Parameters.Add("@txtbranch_id", SqlDbType.NVarChar).Value = W_ID_Select.M_BRANCHID.Trim();
                                    //cmd2.Parameters.Add("@txtacc_id", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[1].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_name", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[10].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_name_eng", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[11].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_type_id", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[4].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_work_type_id", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[6].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_balance_type_id", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[12].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_degree_id", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[8].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_id_control", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[13].Value.ToString();
                                    //cmd2.Parameters.Add("@txtacc_name_control", SqlDbType.NVarChar).Value = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[14].Value.ToString();
                                    //cmd2.Parameters.Add("@txttype_money_id", SqlDbType.NVarChar).Value  = this.PANEL_FORM1_dataGridView1_acc.Rows[i].Cells[15].Value.ToString();
                                    //cmd2.Parameters.Add("@lang_id", SqlDbType.NVarChar).Value = W_ID_Select.Lang.Trim();
                                    //==============================

                                    cmd2.ExecuteNonQuery();
                                //}
                            }
                        }


                    }
                    DialogResult dialogResult = MessageBox.Show("คุณต้องการบันทึกข้อมูล ใช่หรือไม่่ ?", "โปรดยืนยัน", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        trans.Commit();
                        conn.Close();

                        if (this.iblword_status.Text.Trim() == "เพิ่มบัญชีใหม่")
                        {
                            W_ID_Select.LOG_ID = "5";
                            W_ID_Select.LOG_NAME = "บันทึกใหม่";
                            TRANS_LOG();
                        }

                        MessageBox.Show("บันทึกเรียบร้อย", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PANEL_FORM1_Fill_acc();
                        this.iblword_status.Text = "เพิ่มบัญชีใหม่";

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                        trans.Rollback();
                        conn.Close();
                        MessageBox.Show("ยังไม่ได้บันทึก", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
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

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
                                  " AND (txtco_id <> '" + W_ID_Select.M_COID.Trim() + "')" +
                                   //" AND (txtco_status = '0')" +
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
            this.PANEL1_CO_dataGridView1_co.Columns[1].Width = 100;
            this.PANEL1_CO_dataGridView1_co.Columns[1].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL1_CO_dataGridView1_co.Columns[2].Visible = true;  //"Col_txtco_name";
            this.PANEL1_CO_dataGridView1_co.Columns[2].Width = 180;
            this.PANEL1_CO_dataGridView1_co.Columns[2].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.PANEL1_CO_dataGridView1_co.Columns[3].Visible = true; // "Col_txthome_id_full
            this.PANEL1_CO_dataGridView1_co.Columns[3].Width = 250;
            this.PANEL1_CO_dataGridView1_co.Columns[3].ReadOnly = true;
            this.PANEL1_CO_dataGridView1_co.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
                if (this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[4].Value.ToString() == "0")  //Active
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[5].Value = true;
                }
                else
                {
                    this.PANEL1_CO_dataGridView1_co.Rows[i].Cells[5].Value = false;

                }
            }
        }
        private void PANEL1_CO_btnco_Click(object sender, EventArgs e)
        {
            if (this.PANEL1_CO.Visible == false)
            {
                this.PANEL1_CO.Visible = true;
                this.PANEL1_CO.Location = new Point(this.PANEL1_CO_txtco_name.Location.X, this.PANEL1_CO_txtco_name.Location.Y + 22);
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

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL1_CO_txtco_id.Text = row.Cells[1].Value.ToString();
                    this.PANEL1_CO_txtco_name.Text = row.Cells[2].Value.ToString();


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
                                   //" AND (txtco_status = '0')" +
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
        //Company=======================================================================

        private void btnGo_Click(object sender, EventArgs e)
        {
            PANEL_FORM1_Fill_acc();
        }

        private void PANEL_FORM1_Fill_acc()
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

            PANEL_FORM1_Clear_GridView1_acc();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k013db_1acc.*," +
                                  "k013db_2acc_type.*," +
                                   "k013db_3acc_work_type.*," +
                                    "k013db_4acc_degree.*" +
                                    " FROM k013db_1acc" +

                                    " INNER JOIN k013db_2acc_type" +
                                    " ON k013db_1acc.txtacc_type_id = k013db_2acc_type.txtacc_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_2acc_type.lang_id" +

                                    " INNER JOIN k013db_3acc_work_type" +
                                    " ON k013db_1acc.txtacc_work_type_id = k013db_3acc_work_type.txtacc_work_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_3acc_work_type.lang_id" +

                                    " INNER JOIN k013db_4acc_degree" +
                                    " ON k013db_1acc.txtacc_degree_id = k013db_4acc_degree.txtacc_degree_id" +
                                    //" AND k013db_1acc.lang_id = k013db_4acc_degree.lang_id" +

                                    " WHERE (k013db_1acc.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k013db_1acc.txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                    " AND (k013db_1acc.txtacc_id <> '')" +
                                   " ORDER BY k013db_1acc.txtacc_id ASC";

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
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtacc_id"].Value = dt2.Rows[j]["txtacc_id"].ToString();      //1
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "1")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "2")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "     " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "     " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "3")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "          " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "          " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "4")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "               " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "               " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "5")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "                    " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "                    " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            GridView1.Rows[index].Cells["Col_txtacc_type_id"].Value = dt2.Rows[j]["txtacc_type_id"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtacc_type_name"].Value = dt2.Rows[j]["txtacc_type_name"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_id"].Value = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_name"].Value = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //8
                            GridView1.Rows[index].Cells["Col_txtacc_degree_name"].Value = dt2.Rows[j]["txtacc_degree_name"].ToString();      //9


                            //this.PANEL_FORM1_dataGridView1_acc.Columns[10].HeaderText = " Col_txtacc_name2";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[11].HeaderText = " Col_txtacc_name_eng2";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[12].HeaderText = " Col_txtacc_balance_type_id";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[13].HeaderText = " Col_txtacc_id_control";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[14].HeaderText = " Col_txtacc_name_control";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[15].HeaderText = " Col_txttype_money_id";

                            GridView1.Rows[index].Cells["Col_txtacc_name2"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //10
                            GridView1.Rows[index].Cells["Col_txtacc_name_eng2"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //11
                            GridView1.Rows[index].Cells["Col_txtacc_balance_type_id"].Value = dt2.Rows[j]["txtacc_balance_type_id"].ToString();      //12
                            GridView1.Rows[index].Cells["Col_txtacc_id_control"].Value = dt2.Rows[j]["txtacc_id_control"].ToString();      //13
                            GridView1.Rows[index].Cells["Col_txtacc_name_control"].Value = dt2.Rows[j]["txtacc_name_control"].ToString();      //14
                            GridView1.Rows[index].Cells["Col_txttype_money_id"].Value = dt2.Rows[j]["txttype_money_id"].ToString();      //15

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
        private void PANEL_FORM1_GridView1_acc()
        {
            this.GridView1.ColumnCount = 16;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtacc_id";
            this.GridView1.Columns[2].Name = "Col_txtacc_name";
            this.GridView1.Columns[3].Name = "Col_txtacc_name_eng";
            this.GridView1.Columns[4].Name = "Col_txtacc_type_id";
            this.GridView1.Columns[5].Name = "Col_txtacc_type_name";
            this.GridView1.Columns[6].Name = "Col_txtacc_work_type_id";
            this.GridView1.Columns[7].Name = "Col_txtacc_work_type_name";
            this.GridView1.Columns[8].Name = "Col_txtacc_degree_id";
            this.GridView1.Columns[9].Name = "Col_txtacc_degree_name";

            this.GridView1.Columns[10].Name = "Col_txtacc_name2";
            this.GridView1.Columns[11].Name = "Col_txtacc_name_eng2";
            this.GridView1.Columns[12].Name = "Col_txtacc_balance_type_id";
            this.GridView1.Columns[13].Name = "Col_txtacc_id_control";
            this.GridView1.Columns[14].Name = "Col_txtacc_name_control";
            this.GridView1.Columns[15].Name = "Col_txttype_money_id";



            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "รหัส";
            this.GridView1.Columns[2].HeaderText = " ชื่อบัญชี";
            this.GridView1.Columns[3].HeaderText = " ชื่อบัญชี Eng";
            this.GridView1.Columns[4].HeaderText = "รหัสหมวดบัญชี";
            this.GridView1.Columns[5].HeaderText = " หมวดบัญชี";
            this.GridView1.Columns[6].HeaderText = " รหัสประเภท";
            this.GridView1.Columns[7].HeaderText = " ประเภท";
            this.GridView1.Columns[8].HeaderText = " รหัสระดับ";
            this.GridView1.Columns[9].HeaderText = " ระดับ";
            this.GridView1.Columns[10].HeaderText = " Col_txtacc_name2";
            this.GridView1.Columns[11].HeaderText = " Col_txtacc_name_eng2";
            this.GridView1.Columns[12].HeaderText = " Col_txtacc_balance_type_id";
            this.GridView1.Columns[13].HeaderText = " Col_txtacc_id_control";
            this.GridView1.Columns[14].HeaderText = " Col_txtacc_name_control";
            this.GridView1.Columns[15].HeaderText = " Col_txttype_money_id";

            this.GridView1.Columns[0].Visible = false;  //"No";
            this.GridView1.Columns[1].Visible = true;  //"Col_txtacc_id";
            this.GridView1.Columns[1].Width = 100;
            this.GridView1.Columns[1].ReadOnly = true;
            this.GridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[2].Visible = true;  //"Col_txtacc_name";
            this.GridView1.Columns[2].Width = 150;
            this.GridView1.Columns[2].ReadOnly = true;
            this.GridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[3].Visible = true;  //"Col_txtacc_name_eng";
            this.GridView1.Columns[3].Width = 150;
            this.GridView1.Columns[3].ReadOnly = true;
            this.GridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.GridView1.Columns[4].Visible = false;  //"Col_txtacc_type_id";
            this.GridView1.Columns[5].Visible = true;  //"Col_txtacc_type_name";
            this.GridView1.Columns[5].Width = 100;
            this.GridView1.Columns[5].ReadOnly = true;
            this.GridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[6].Visible = false;  //"Col_txtacc_work_type_id";
            this.GridView1.Columns[7].Visible = true;  //"Col_txtacc_work_type_name";
            this.GridView1.Columns[7].Width = 100;
            this.GridView1.Columns[7].ReadOnly = true;
            this.GridView1.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[8].Visible = false;  //"Col_txtacc_degree_id";
            this.GridView1.Columns[9].Visible = true;  //"Col_txtacc_degree_name";
            this.GridView1.Columns[9].Width = 100;
            this.GridView1.Columns[9].ReadOnly = true;
            this.GridView1.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns[10].Visible = false;  //"Col_txtacc_name2";
            this.GridView1.Columns[11].Visible = false;  //"Col_txtacc_name_eng2";
            this.GridView1.Columns[12].Visible = false;  //"Col_txtacc_balance_type_id";
            this.GridView1.Columns[13].Visible = false;  //"Col_txtacc_id_control";
            this.GridView1.Columns[14].Visible = false;  //"Col_txtacc_name_control";
            this.GridView1.Columns[15].Visible = false;  //"Col_txttype_money_id";


            this.GridView1.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView1.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView1.EnableHeadersVisualStyles = false;

        }
        private void PANEL_FORM1_Clear_GridView1_acc()
        {
            this.GridView1.Rows.Clear();
            this.GridView1.Refresh();
        }
        private void PANEL_FORM1_dataGridView1_acc_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }

        private void PANEL_FORM1_dataGridView1_acc_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                GridView1.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);
            }
        }
        private void PANEL_FORM1_btnrefresh_Click(object sender, EventArgs e)
        {
            PANEL_FORM1_Fill_acc();
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

            PANEL_FORM1_Clear_GridView1_acc();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT k013db_1acc.*," +
                                  "k013db_2acc_type.*," +
                                   "k013db_3acc_work_type.*," +
                                    "k013db_4acc_degree.*" +
                                    " FROM k013db_1acc" +

                                    " INNER JOIN k013db_2acc_type" +
                                    " ON k013db_1acc.txtacc_type_id = k013db_2acc_type.txtacc_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_2acc_type.lang_id" +

                                    " INNER JOIN k013db_3acc_work_type" +
                                    " ON k013db_1acc.txtacc_work_type_id = k013db_3acc_work_type.txtacc_work_type_id" +
                                    //" AND k013db_1acc.lang_id = k013db_3acc_work_type.lang_id" +

                                    " INNER JOIN k013db_4acc_degree" +
                                    " ON k013db_1acc.txtacc_degree_id = k013db_4acc_degree.txtacc_degree_id" +
                                    //" AND k013db_1acc.lang_id = k013db_4acc_degree.lang_id" +

                                    " WHERE (k013db_1acc.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (k013db_1acc.txtco_id = '" + this.PANEL1_CO_txtco_id.Text.Trim() + "')" +
                                    " AND (k013db_1acc.txtacc_name LIKE '%" + this.PANEL_FORM1_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY k013db_1acc.txtacc_id ASC";

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
                            var index = GridView1.Rows.Add();
                            GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView1.Rows[index].Cells["Col_txtacc_id"].Value = dt2.Rows[j]["txtacc_id"].ToString();      //1
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "1")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "2")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "     " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "     " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "3")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "          " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "          " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "4")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "               " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "               " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            if (dt2.Rows[j]["txtacc_degree_id"].ToString() == "5")
                            {
                                GridView1.Rows[index].Cells["Col_txtacc_name"].Value = "                    " + dt2.Rows[j]["txtacc_name"].ToString();      //2
                                GridView1.Rows[index].Cells["Col_txtacc_name_eng"].Value = "                    " + dt2.Rows[j]["txtacc_name_eng"].ToString();      //2
                            }
                            GridView1.Rows[index].Cells["Col_txtacc_type_id"].Value = dt2.Rows[j]["txtacc_type_id"].ToString();      //4
                            GridView1.Rows[index].Cells["Col_txtacc_type_name"].Value = dt2.Rows[j]["txtacc_type_name"].ToString();      //5
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_id"].Value = dt2.Rows[j]["txtacc_work_type_id"].ToString();      //6
                            GridView1.Rows[index].Cells["Col_txtacc_work_type_name"].Value = dt2.Rows[j]["txtacc_work_type_name"].ToString();      //7
                            GridView1.Rows[index].Cells["Col_txtacc_degree_id"].Value = dt2.Rows[j]["txtacc_degree_id"].ToString();      //8
                            GridView1.Rows[index].Cells["Col_txtacc_degree_name"].Value = dt2.Rows[j]["txtacc_degree_name"].ToString();      //9


                            //this.PANEL_FORM1_dataGridView1_acc.Columns[10].HeaderText = " Col_txtacc_name2";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[11].HeaderText = " Col_txtacc_name_eng2";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[12].HeaderText = " Col_txtacc_balance_type_id";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[13].HeaderText = " Col_txtacc_id_control";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[14].HeaderText = " Col_txtacc_name_control";
                            //this.PANEL_FORM1_dataGridView1_acc.Columns[15].HeaderText = " Col_txttype_money_id";

                            GridView1.Rows[index].Cells["Col_txtacc_name2"].Value = dt2.Rows[j]["txtacc_name"].ToString();      //10
                            GridView1.Rows[index].Cells["Col_txtacc_name_eng2"].Value = dt2.Rows[j]["txtacc_name_eng"].ToString();      //11
                            GridView1.Rows[index].Cells["Col_txtacc_balance_type_id"].Value = dt2.Rows[j]["txtacc_balance_type_id"].ToString();      //12
                            GridView1.Rows[index].Cells["Col_txtacc_id_control"].Value = dt2.Rows[j]["txtacc_id_control"].ToString();      //13
                            GridView1.Rows[index].Cells["Col_txtacc_name_control"].Value = dt2.Rows[j]["txtacc_name_control"].ToString();      //14
                            GridView1.Rows[index].Cells["Col_txttype_money_id"].Value = dt2.Rows[j]["txttype_money_id"].ToString();      //15



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

        //Tans_Log ====================================================================

    }
}
