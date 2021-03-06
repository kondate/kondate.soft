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

namespace kondate.soft.Panel_SETUP
{
    public partial class Form_r001_02pay_type_2Checque : Form
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

        public Form_r001_02pay_type_2Checque()
        {
            InitializeComponent();
        }

        private void Form_r001_02pay_type_2Checque_Load(object sender, EventArgs e)
        {
            W_ID_Select.CDKEY = this.txtcdkey.Text.Trim();
            W_ID_Select.ADATASOURCE = this.txtHost_name.Text.Trim();
            W_ID_Select.DATABASE_NAME = this.txtDb_name.Text.Trim();



            Fill_PANEL_02PAY_TYPE_2CHECQUE_BANK();

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
        private void PANEL_02PAY_TYPE_2CHECQUE_cbobank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_PANEL_02PAY_TYPE_2CHECQUE_BANK2();
        }
        private void PANEL_02PAY_TYPE_2CHECQUE_BtnOK_Click(object sender, EventArgs e)
        {

        }
        private void PANEL_02PAY_TYPE_2CHECQUE_btnclose_Click(object sender, EventArgs e)
        {

        }
        private void PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque_ValueChanged(object sender, EventArgs e)
        {
            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Format = DateTimePickerFormat.Custom;
            this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.CustomFormat = this.PANEL_02PAY_TYPE_2CHECQUE_dtpdate_checque.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

    }
}
