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
using System.Collections;

namespace kondate.soft.HOME03_Production
{
    public partial class HOME03_Production_05Send_Dye_record : Form
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

        public HOME03_Production_05Send_Dye_record()
        {
            InitializeComponent();

            GridView1.Controls.Add(dtp);
            dtp.Visible = false;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.TextChanged += new EventHandler(dtp_TextChange);

        }

        private void HOME03_Production_05Send_Dye_record_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.btnmaximize.Visible = false;
            this.btnmaximize_full.Visible = true;

            ////สำหรับทดสอบระบบ =====================================================================================================================
            //W_ID_Select.CDKEY = "samn20201125";//  = "chaixifactory2562020601"= "";// = "chaixifactory2562020601"= "";
            //W_ID_Select.ADATASOURCE = "916909b5121b.sn.mynetname.net,6001";  //916909b5121b.sn.mynetname.net,6001 // C4PC_AOT\\SQLEXPRESS,49170" //ASAICAFEKLONG3\\SQLEXPRESS,49170   //172.168.0.15\\SQLEXPRESS,49170
            //W_ID_Select.DATABASE_NAME = "samn_db"; //KREST2020
            //W_ID_Select.Lang = "001";// = "001"= ""; //001ไทย, 002Eng,003ลาว,004กัมพูชา,005พม่า

            //W_ID_Select.Crytal_SERVER = "916909b5121b.sn.mynetname.net,6001";
            //W_ID_Select.Crytal_DATABASE = "samn_db";
            //W_ID_Select.Crytal_USER = "sa";
            //W_ID_Select.Crytal_Pass = "Kon51Aot";

            //W_ID_Select.M_COID = "KD";
            //W_ID_Select.M_CONAME = "บริษัท ทดสอบระบบ จำกัด";
            //W_ID_Select.M_BRANCHID = "KD001";
            //W_ID_Select.M_BRANCHNAME = "สำนักงานใหญ่";
            //W_ID_Select.M_BRANCHNAME_SHORT = "HO";

            //W_ID_Select.M_USERNAME = "admin";
            //W_ID_Select.M_USERNAME_TYPE = "4";

            //FillDATE_FROM_SERVER();
            ////สำหรับทดสอบระบบ =====================================================================================================================

            //W_ID_Select.M_FORM_NUMBER = "H0205RGRD";
            //CHECK_ADD_FORM();
            //CHECK_USER_RULE();


            //W_ID_Select.LOG_ID = "1";
            //W_ID_Select.LOG_NAME = "Login";
            //TRANS_LOG();
            // =====================================================================================================================


            this.iblword_top.Text = W_ID_Select.WORD_TOP.Trim();
            this.iblstatus.Text = "Version : " + W_ID_Select.GetVersion() + "      |       User name (ชื่อผู้ใช้) : " + W_ID_Select.M_EMP_OFFICE_NAME.ToString() + "       |       กิจการ : " + W_ID_Select.M_CONAME.ToString() + "      |      สาขา : " + W_ID_Select.M_BRANCHNAME.ToString() + "      |     วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy") + "";
            this.iblword_status.Text = "บันทึกใบส่งผ้าย้อม";

            this.ActiveControl = this.txtPPT_record_remark;
            this.BtnNew.Enabled = false;
            this.BtnSave.Enabled = true;
            this.btnopen.Enabled = false;
            this.BtnCancel_Doc.Enabled = false;
            this.btnPreview.Enabled = false;
            this.BtnPrint.Enabled = false;

            //1.ส่วนหน้าหลัก======================================================================
            this.dtpdate_record.Value = DateTime.Now;
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

            PANEL161_SUP_GridView1_supplier();
            PANEL161_SUP_Fill_supplier();

            PANEL1306_WH_GridView1_wherehouse();
            PANEL1306_WH_Fill_wherehouse();

            PANEL0107_NUMBER_COLOR_GridView1_number_color();
            PANEL0107_NUMBER_COLOR_Fill_number_color();

            PANEL0105_FACE_BAKING_GridView1_face_baking();
            PANEL0105_FACE_BAKING_Fill_face_baking();

            PANEL1313_ACC_GROUP_TAX_GridView1_acc_group_tax();
            PANEL1313_ACC_GROUP_TAX_Fill_acc_group_tax();

            Show_GridView1();

            Show_GridView66();
            Fill_Show_DATA_GridView66();
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
            if (this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text == "")
            {
                MessageBox.Show("โปรด เลือก รหัสสี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (this.PANEL0107_NUMBER_COLOR.Visible == false)
                {
                    this.PANEL0107_NUMBER_COLOR.Visible = true;
                    this.PANEL0107_NUMBER_COLOR.BringToFront();
                    this.PANEL0107_NUMBER_COLOR.Location = new Point(this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Location.X, this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Location.Y + 22);
                }
                else
                {
                    this.PANEL0107_NUMBER_COLOR.Visible = false;
                }
                return;

            }
            else
            {

            }
            //======================================================
            //======================================================








        }
 
        private void Show_GridView1()
        {
            this.GridView1.ColumnCount = 43;
            this.GridView1.Columns[0].Name = "Col_Auto_num";
            this.GridView1.Columns[1].Name = "Col_txtwherehouse_id";
            this.GridView1.Columns[2].Name = "Col_txtnumber_in_year";
            this.GridView1.Columns[3].Name = "Col_txtnumber_mat_id";
            this.GridView1.Columns[4].Name = "Col_txtnumber_color_id";
            this.GridView1.Columns[5].Name = "Col_txtface_baking_id";


            this.GridView1.Columns[6].Name = "Col_txtlot_no";
            this.GridView1.Columns[7].Name = "Col_txtfold_number";

            this.GridView1.Columns[9].Name = "Col_txtqty";

            this.GridView1.Columns[10].Name = "Col_txtmat_no";
            this.GridView1.Columns[11].Name = "Col_txtmat_id";
            this.GridView1.Columns[12].Name = "Col_txtmat_name";

            this.GridView1.Columns[13].Name = "Col_txtmat_unit1_name";
            this.GridView1.Columns[14].Name = "Col_txtmat_unit1_qty";
            this.GridView1.Columns[15].Name = "Col_chmat_unit_status";
            this.GridView1.Columns[16].Name = "Col_txtmat_unit2_name";
            this.GridView1.Columns[17].Name = "Col_txtmat_unit2_qty";

            this.GridView1.Columns[18].Name = "Col_txtqty2";

            this.GridView1.Columns[19].Name = "Col_txtprice";
            this.GridView1.Columns[20].Name = "Col_txtdiscount_rate";
            this.GridView1.Columns[21].Name = "Col_txtdiscount_money";
            this.GridView1.Columns[22].Name = "Col_txtsum_total";
        
            this.GridView1.Columns[23].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView1.Columns[24].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView1.Columns[25].Name = "Col_txtcost_money_sum_yokma";

            this.GridView1.Columns[26].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView1.Columns[27].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView1.Columns[28].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView1.Columns[29].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView1.Columns[30].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView1.Columns[31].Name = "Col_txtitem_no";

            this.GridView1.Columns[32].Name = "Col_txtqc_id";
            this.GridView1.Columns[33].Name = "Col_txtsum_qty_pub";
            this.GridView1.Columns[34].Name = "Col_date";
            this.GridView1.Columns[35].Name = "Col_qty_Cal";  //
            this.GridView1.Columns[36].Name = "Col_txtsum_qty_rib";
            this.GridView1.Columns[37].Name = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[38].Name = "Col_txtsum_qty_rib_kg";

            this.GridView1.Columns[39].Name = "Col_txtqty_after_cut";
            this.GridView1.Columns[40].Name = "Col_txtqty_cut_yokma";
            this.GridView1.Columns[41].Name = "Col_txtqty_cut_yokpai";
            this.GridView1.Columns[42].Name = "Col_txtqty_after_cut_yokpai";


            this.GridView1.Columns[0].HeaderText = "No";
            this.GridView1.Columns[1].HeaderText = "คลัง";
            this.GridView1.Columns[2].HeaderText = "ชุดที่";
            this.GridView1.Columns[3].HeaderText = "รหัสผ้า";
            this.GridView1.Columns[4].HeaderText = "รหัสสี";
            this.GridView1.Columns[5].HeaderText = "อบหน้า";


            this.GridView1.Columns[6].HeaderText = "Lot No";
            this.GridView1.Columns[7].HeaderText = "พับที่";

            this.GridView1.Columns[9].HeaderText = "ส่งย้อม (กก.)";

            this.GridView1.Columns[10].HeaderText = "ลำดับ";
            this.GridView1.Columns[11].HeaderText = "รหัส";
            this.GridView1.Columns[12].HeaderText = "ชื่อสินค้า";

            this.GridView1.Columns[13].HeaderText = " หน่วยหลัก";
            this.GridView1.Columns[14].HeaderText = " หน่วย";
            this.GridView1.Columns[15].HeaderText = "แปลง";
            this.GridView1.Columns[16].HeaderText = " หน่วย(ปอนด์)";
            this.GridView1.Columns[17].HeaderText = " หน่วย";

            this.GridView1.Columns[18].HeaderText = "ส่งย้อม(ปอนด์)";

            this.GridView1.Columns[19].HeaderText = "ราคา";
            this.GridView1.Columns[20].HeaderText = "ส่วนลด(%)";
            this.GridView1.Columns[21].HeaderText = "ส่วนลด(บาท)";
            this.GridView1.Columns[22].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView1.Columns[23].HeaderText = "จำนวนยกมา";
            this.GridView1.Columns[24].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView1.Columns[25].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[26].HeaderText = "จำนวนยกไป";
            this.GridView1.Columns[27].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView1.Columns[28].HeaderText = "จำนวนเงิน";

            this.GridView1.Columns[29].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView1.Columns[30].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView1.Columns[31].HeaderText = "item_no";
            this.GridView1.Columns[32].HeaderText = "txtqc_id";
            this.GridView1.Columns[33].HeaderText = "Col_txtsum_qty_pub";
            this.GridView1.Columns[34].HeaderText = " วันที่ต้องการ";
            this.GridView1.Columns[35].HeaderText = "Col_qty_Cal";
            this.GridView1.Columns[36].HeaderText = "Col_txtsum_qty_rib";
            this.GridView1.Columns[37].HeaderText = "Col_txtsum_qty_pub_kg";
            this.GridView1.Columns[38].HeaderText = "Col_txtsum_qty_rib_kg";

            this.GridView1.Columns[39].HeaderText = "Col_txtqty_after_cut ยกมา";
            this.GridView1.Columns[40].HeaderText = "รวมจำนวนรับคืนแล้วยกมา";
            this.GridView1.Columns[41].HeaderText = "รวมจำนวนรับคืนแล้วยกไป";
            this.GridView1.Columns[42].HeaderText = "เหลือรอรับอีก กก.";


            this.GridView1.Columns["Col_Auto_num"].Visible = true;  //"Col_Auto_num";
            this.GridView1.Columns["Col_Auto_num"].Width =60;
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

            this.GridView1.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";
            this.GridView1.Columns["Col_txtnumber_mat_id"].Width = 0;
            this.GridView1.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtnumber_color_id"].Visible = true;  //"Col_txtnumber_color_id";
            this.GridView1.Columns["Col_txtnumber_color_id"].Width = 80;
            this.GridView1.Columns["Col_txtnumber_color_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtnumber_color_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtnumber_color_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView1.Columns["Col_txtface_baking_id"].Width = 80;
            this.GridView1.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView1.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            this.GridView1.Columns["Col_txtlot_no"].Visible = true;  //"Col_txtlot_no";
            this.GridView1.Columns["Col_txtlot_no"].Width = 200;
            this.GridView1.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView1.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView1.Columns["Col_txtfold_number"].Width = 60;
            this.GridView1.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView1.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView1.Columns[8].Visible = false;
            DataGridViewCheckBoxColumn dgvCmb_SELECT = new DataGridViewCheckBoxColumn();
            dgvCmb_SELECT.Name = "Col_Chk_SELECT";
            dgvCmb_SELECT.Width = 120;  //70
            dgvCmb_SELECT.DisplayIndex = 8;
            dgvCmb_SELECT.HeaderText = "เลือกส่งย้อม";
            dgvCmb_SELECT.ValueType = typeof(bool);
            dgvCmb_SELECT.ReadOnly = false;
            dgvCmb_SELECT.Visible = true;
            dgvCmb_SELECT.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb_SELECT.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView1.Columns.Add(dgvCmb_SELECT);

            this.GridView1.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView1.Columns["Col_txtqty"].Width = 100;
            this.GridView1.Columns["Col_txtqty"].ReadOnly = false;
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

            this.GridView1.Columns["Col_date"].Visible = true;  //"Col_date";
            this.GridView1.Columns["Col_date"].Width = 150;
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

             this.GridView1.Columns["Col_txtqty_after_cut"].Visible = false;  //"Col_txtqty_after_cut";
            this.GridView1.Columns["Col_txtqty_after_cut"].Width = 0;
            this.GridView1.Columns["Col_txtqty_after_cut"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_after_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_after_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView1.Columns["Col_txtqty_cut_yokma"].Visible = false;  //"Col_txtqty_cut_yokma";
            this.GridView1.Columns["Col_txtqty_cut_yokma"].Width = 0;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].ReadOnly = true;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView1.Columns["Col_txtqty_cut_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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

            double Sum_Qty_CUT_Yokpai = 0;
            double Sum_Qty_AF_CUT_Yokpai = 0;


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

                if (this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = ".00";
                }
                if (this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value == null)
                {
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value = ".00";
                }

                if (double.Parse(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) > 0)
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

                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value).ToString("###,###.00");     //8
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value = Convert.ToSingle(this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value).ToString("###,###.00");     //8
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
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub  จำนวนพับ=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub_kg  จำนวนพับ=================================================
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));


                    //Sum_Qty_RIB จำนวนพับ=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_RIB_kg จำนวนพับ=================================================
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));


                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }

                    //Sum_Qty_CUT_Yokpai  =================================================
                    Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_AF_CUT_Yokpai  =================================================
                    Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

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
                    Sum_Qty = Convert.ToDouble(string.Format("{0:n}", Sum_Qty)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_qty_Cal"].Value.ToString()));
                    this.txtsum_qty.Text = Sum_Qty.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub  จำนวนพับ=================================================
                    Sum_Qty_Pub = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString()));
                    this.txtsum_qty_pub.Text = Sum_Qty_Pub.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_Pub_kg  จำนวนพับ=================================================
                    Sum_Qty_Pub_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_Pub_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub_kg"].Value.ToString()));
                    this.txtsum_qty_pub_kg.Text = Sum_Qty_Pub_kg.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_RIB จำนวนพับ=================================================
                    Sum_Qty_RIB = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString()));
                    this.txtsum_qty_rib.Text = Sum_Qty_RIB.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_RIB_kg จำนวนพับ=================================================
                    Sum_Qty_RIB_kg = Convert.ToDouble(string.Format("{0:n}", Sum_Qty_RIB_kg)) + Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib_kg"].Value.ToString()));
                    this.txtsum_qty_rib_kg.Text = Sum_Qty_RIB_kg.ToString("N", new CultureInfo("en-US"));

                    //============================================================================================================
                    //แปลงหน่วย เป็นหน่วย 2 จาก กก. เป็น ปอนด์
                    if (this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() == "Y")  //
                    {
                        Con_QTY = Convert.ToDouble(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) * Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtqty2"].Value = Con_QTY.ToString("N", new CultureInfo("en-US"));
                        //Sum2_Qty_Yokpai  =================================================
                        Sum2_Qty_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Sum2_Qty_Yokpai.ToString("N", new CultureInfo("en-US"));

                        //Sum2_Qty  จำนวนเบิก (ปอนด์)=================================================
                        Sum2_Qty = Convert.ToDouble(string.Format("{0:n4}", Sum2_Qty)) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString()));
                        this.txtsum2_qty.Text = Sum2_Qty.ToString("N", new CultureInfo("en-US"));
                    }

                    //Sum_Qty_CUT_Yokpai  =================================================
                    Sum_Qty_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value = Sum_Qty_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));

                    //Sum_Qty_AF_CUT_Yokpai  =================================================
                    Sum_Qty_AF_CUT_Yokpai = Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut"].Value.ToString())) - Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString()));
                    this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value = Sum_Qty_AF_CUT_Yokpai.ToString("N", new CultureInfo("en-US"));
                }

                //if (double.Parse(string.Format("{0:n}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) == 1)
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
                QAbyma = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokma.Text.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtcost_money_sum_yokma.Text = QAbyma.ToString("N", new CultureInfo("en-US"));

                //มูลค่าต้นทุนเบิก ใช้ราคาถัวเฉลี่ยยกมา
                this.txtprice.Text = txtcost_qty_price_average_yokma.Text;
                QAbyma2 = Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString())) * Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_price_average_yokma.Text.ToString()));
                this.txtsum_total.Text = QAbyma2.ToString("N", new CultureInfo("en-US"));


                //1.เหลือยกมา - เบิก = จำนวนเหลือทั้งสิ้น
                Qbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_qty.Text.ToString()));
                this.txtcost_qty_balance_yokpai.Text = Qbypai.ToString("N", new CultureInfo("en-US"));
                //2.มูลค่าเหลือยกมา - มูลค่าเบิก = มูลค่ารวมทั้งสิ้น
                Mbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_total.Text.ToString()));
                this.txtcost_money_sum_yokpai.Text = Mbypai.ToString("N", new CultureInfo("en-US"));
                //3.มูลค่ารวมทั้งสิ้น / จำนวนเหลือทั้งสิ้น = ราคาต่อหน่วยเฉลี่ย
                if (Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokpai.Text.ToString())) > 0)
                {
                    QAbypai = Convert.ToDouble(string.Format("{0:n}", this.txtcost_money_sum_yokpai.Text.ToString())) / Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty_balance_yokpai.Text.ToString()));
                    this.txtcost_qty_price_average_yokpai.Text = QAbypai.ToString("N", new CultureInfo("en-US"));
                }
                else
                {
                    this.txtcost_qty_price_average_yokpai.Text = "0";
                }

                //1.เหลือ(2)ยกมา - เบิก(2) = จำนวนเหลือ(2)ทั้งสิ้น
                Qbypai2 = Convert.ToDouble(string.Format("{0:n}", this.txtcost_qty2_balance_yokma.Text.ToString())) - Convert.ToDouble(string.Format("{0:n}", this.txtsum2_qty.Text.ToString()));
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

             Sum_Qty_CUT_Yokpai = 0;
             Sum_Qty_AF_CUT_Yokpai = 0;

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
                DisCount = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_sum.Text)) - Convert.ToDouble(string.Format("{0:n}", this.txtsum_discount.Text));
                this.txtmoney_tax_base.Text = DisCount.ToString("N", new CultureInfo("en-US"));

                //ภาษีเงิน
                VATMONey = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) * Convert.ToDouble(string.Format("{0:n}", this.txtvat_rate.Text)) / 100;
                this.txtvat_money.Text = VATMONey.ToString("N", new CultureInfo("en-US"));

                //รวมทั้งสิ้น
                MONeyAF_VAT = Convert.ToDouble(string.Format("{0:n}", this.txtmoney_tax_base.Text)) + Convert.ToDouble(string.Format("{0:n}", this.txtvat_money.Text));
                this.txtmoney_after_vat.Text = MONeyAF_VAT.ToString("N", new CultureInfo("en-US"));

            }
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_IN") //ซื้อคิดvatรวม
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
            if (this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim() == "PUR_ONvat")  //ซื้อไม่มีvat
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



        //====================


        //txtwherehouse คลังสินค้า  =======================================================================
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
            var frm2 = new HOME03_Production.HOME03_Production_04QC_record();
            frm2.Closed += (s, args) => this.Close();
            frm2.Show();

            this.iblword_status.Text = "บันทึกใบส่งผ้าย้อม";
            this.txtPPT_id.ReadOnly = true;
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
            //if (this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text == "")
            //{
            //    MessageBox.Show("โปรด ใส่เบอร์สินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Focus();
            //    return;
            //}


            AUTO_BILL_TRANS_ID();
            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();



            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                {
                    if (this.GridView1.Rows[i].Cells["Col_date"].Value == null)
                    {
                        MessageBox.Show("โปรด ใส่เวันที่ต้องการสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
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
                        cmd2.CommandText = "INSERT INTO c002_05Send_dye_record_trans(cdkey," +
                                           "txtco_id,txtbranch_id," +
                                           "txttrans_id)" +
                                           "VALUES ('" + W_ID_Select.CDKEY.Trim() + "'," +
                                           "'" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +
                                           "'" + this.txtPPT_id.Text.Trim() + "')";

                        cmd2.ExecuteNonQuery();


                    }
                    else
                    {
                        cmd2.CommandText = "UPDATE c002_05Send_dye_record_trans SET txttrans_id = '" + this.txtPPT_id.Text.Trim() + "'" +
                                           " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                           " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                           " AND (txtbranch_id = '" + W_ID_Select.M_BRANCHID.Trim() + "')";

                        cmd2.ExecuteNonQuery();

                    }
                    //MessageBox.Show("ok1");

                    //2 c002_05Send_dye_record
                    cmd2.CommandText = "INSERT INTO c002_05Send_dye_record(cdkey,txtco_id,txtbranch_id," +  //1
                                           "txttrans_date_server,txttrans_time," +  //2
                                           "txttrans_year,txttrans_month,txttrans_day,txttrans_date_client," +  //3
                                           "txtcomputer_ip,txtcomputer_name," +  //4
                                            "txtuser_name,txtemp_office_name," +  //5
                                           "txtversion_id," +  //6
                                                               //====================================================

                                           "txtPPT_id," + // 7
                                           "txtapprove_id," + // 8
                                           "txtapprove_date," + // 9
                                           "txtRG_id," + // 10
                                          "txtRG_date," + // 11
                                           //"txtreceive_id," + // 12
                                          "txtreceive_date," + // 13
                                          "txtwherehouse_id," + // 14
                                          "txtsupplier_id," + // 15
                                          "txtcontact_person," + // 16
                                          "txtwant_mat_in_day," + // 17
                                          "txtdate_send_mat," + // 18
                                          "txtcredit_in_day," + // 19
                                          "txtPPT_record_remark," + // 20
                                          "txtemp_office_name_receive," + // 21

                                           "txtemp_office_name_audit," + // 21
                                           "txtemp_office_name_send," + // 22
                                          "txtapprove_status_id," + // 23
                                          "txtdepartment_id," + // 24
                                          "txtproject_id," + // 24
                                           "txtjob_id," + // 25
                                           "txtjob_send_mat_status," + // 26

                                           "txtmat_no," + // 27
                                           "txtmat_id," + // 28
                                           "txtmat_name," + // 29
                                           "txtnumber_mat_id," + // 30

                                           "txtcurrency_id," + // 31
                                           "txtcurrency_date," + // 32
                                           "txtcurrency_rate," + // 33

                                           "txtacc_group_tax_id," + // 34

                                           "txtcost_qty_balance_yokma," + // 25
                                           "txtcost_qty_price_average_yokma," + // 26
                                           "txtcost_money_sum_yokma," + // 27

                                           "txtsum_qty_pub," + // 28
                                           "txtsum_qty_pub_receive," + // 29
                                           "txtsum_qty_pub_balance," + // 30

                                           "txtsum_qty_pub_kg," + // 28
                                           "txtsum_qty_pub_receive_kg," + // 29
                                           "txtsum_qty_pub_balance_kg," + // 30


                                           "txtsum_qty_rib," + // 28
                                           "txtsum_qty_rib_receive," + // 29
                                           "txtsum_qty_rib_balance," + // 30

                                           "txtsum_qty_rib_kg," + // 28
                                           "txtsum_qty_rib_receive_kg," + // 29
                                           "txtsum_qty_rib_balance_kg," + // 30


                                           "txtsum_qty," + // 31
                                           "txtsum_qty_receive," + // 32
                                           "txtsum_qty_balance," + // 33


                                           "txtsum_price," + // 34
                                           "txtsum_discount," + // 35
                                           "txtmoney_sum," + // 36
                                           "txtmoney_tax_base," + // 37
                                           "txtvat_rate," + // 38
                                           "txtvat_money," + // 39
                                           "txtmoney_after_vat," + // 40
                                           "txtmoney_after_vat_creditor," + // 41
                                           "txtcreditor_status," + // 42

                                           "txtcost_qty_balance_yokpai," + // 43
                                           "txtcost_qty_price_average_yokpai," + // 44
                                           "txtcost_money_sum_yokpai," + // 45

                                           "txtcost_qty2_balance_yokma," + // 46
                                           "txtsum2_qty," + // 47
                                           "txtcost_qty2_balance_yokpai," + // 48

                                           "txtPPT_status," +  //49
                                          "txtapprove_status," +  //50
                                          "txtRG_status," +  //51
                                          "txtreceive_status," +  //52
                                          "txtpayment_status," +  //53
                                          "txtacc_record_status," +  //54
                                          "txtemp_print,txtemp_print_datetime) " +  //55

                                           "VALUES (@cdkey,@txtco_id,@txtbranch_id," +  //1
                                           "@txttrans_date_server,@txttrans_time," +  //2
                                           "@txttrans_year,@txttrans_month,@txttrans_day,@txttrans_date_client," +  //3
                                           "@txtcomputer_ip,@txtcomputer_name," +  //4
                                           "@txtuser_name,@txtemp_office_name," +  //5
                                           "@txtversion_id," +  //6
                                                                //=========================================================


                                           "@txtPPT_id," + // 7
                                           "@txtapprove_id," + // 8
                                           "@txtapprove_date," + // 9
                                           "@txtRG_id," + // 10
                                          "@txtRG_date," + // 11
                                           //"@txtreceive_id," + // 12
                                          "@txtreceive_date," + // 13
                                          "@txtwherehouse_id," + // 14
                                          "@txtsupplier_id," + // 15
                                          "@txtcontact_person," + // 16
                                          "@txtwant_mat_in_day," + // 17
                                          "@txtdate_send_mat," + // 18
                                          "@txtcredit_in_day," + // 19
                                          "@txtPPT_record_remark," + // 20
                                          "@txtemp_office_name_receive," + // 21

                                           "@txtemp_office_name_audit," + // 21
                                           "@txtemp_office_name_send," + // 22
                                          "@txtapprove_status_id," + // 23
                                          "@txtdepartment_id," + // 24
                                          "@txtproject_id," + // 24
                                           "@txtjob_id," + // 25
                                           "@txtjob_send_mat_status," + // 26

                                           "@txtmat_no," + // 27
                                           "@txtmat_id," + // 28
                                           "@txtmat_name," + // 29
                                           "@txtnumber_mat_id," + // 30

                                           "@txtcurrency_id," + // 31
                                           "@txtcurrency_date," + // 32
                                           "@txtcurrency_rate," + // 33

                                           "@txtacc_group_tax_id," + // 34

                                           "@txtcost_qty_balance_yokma," + // 25
                                           "@txtcost_qty_price_average_yokma," + // 26
                                           "@txtcost_money_sum_yokma," + // 27

                                           "@txtsum_qty_pub," + // 28
                                           "@txtsum_qty_pub_receive," + // 29
                                           "@txtsum_qty_pub_balance," + // 30

                                           "@txtsum_qty_pub_kg," + // 28
                                           "@txtsum_qty_pub_receive_kg," + // 29
                                           "@txtsum_qty_pub_balance_kg," + // 30

                                           "@txtsum_qty_rib," + // 28
                                           "@txtsum_qty_rib_receive," + // 29
                                           "@txtsum_qty_rib_balance," + // 30

                                           "@txtsum_qty_rib_kg," + // 28
                                           "@txtsum_qty_rib_receive_kg," + // 29
                                           "@txtsum_qty_rib_balance_kg," + // 30

                                           "@txtsum_qty," + // 31
                                           "@txtsum_qty_receive," + // 32
                                           "@txtsum_qty_balance," + // 33

                                           "@txtsum_price," + // 34
                                           "@txtsum_discount," + // 35
                                           "@txtmoney_sum," + // 36
                                           "@txtmoney_tax_base," + // 37
                                           "@txtvat_rate," + // 38
                                           "@txtvat_money," + // 39
                                           "@txtmoney_after_vat," + // 40
                                           "@txtmoney_after_vat_creditor," + // 41
                                           "@txtcreditor_status," + // 42

                                           "@txtcost_qty_balance_yokpai," + // 43
                                           "@txtcost_qty_price_average_yokpai," + // 44
                                           "@txtcost_money_sum_yokpai," + // 45

                                           "@txtcost_qty2_balance_yokma," + // 46
                                           "@txtsum2_qty," + // 47
                                           "@txtcost_qty2_balance_yokpai," + // 48

                                           "@txtPPT_status," +  //49
                                          "@txtapprove_status," +  //50
                                          "@txtRG_status," +  //51
                                          "@txtreceive_status," +  //52
                                          "@txtpayment_status," +  //53
                                          "@txtacc_record_status," +  //54
                                          "@txtemp_print,@txtemp_print_datetime) ";  //55

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



                    cmd2.Parameters.Add("@txtPPT_id", SqlDbType.NVarChar).Value = this.txtPPT_id.Text.Trim();  //7
                    cmd2.Parameters.Add("@txtapprove_id", SqlDbType.NVarChar).Value = "";  //8
                    cmd2.Parameters.Add("@txtapprove_date", SqlDbType.NVarChar).Value = "";  //9

                    cmd2.Parameters.Add("@txtRG_id", SqlDbType.NVarChar).Value = "";  //10
                    cmd2.Parameters.Add("@txtRG_date", SqlDbType.NVarChar).Value = "";  //11
                    //cmd2.Parameters.Add("@txtreceive_id", SqlDbType.NVarChar).Value = "";  //12
                    cmd2.Parameters.Add("@txtreceive_date", SqlDbType.NVarChar).Value = "";  //13
                    cmd2.Parameters.Add("@txtwherehouse_id", SqlDbType.NVarChar).Value = this.PANEL1306_WH_txtwherehouse_id.Text.Trim();  //14
                    cmd2.Parameters.Add("@txtsupplier_id", SqlDbType.NVarChar).Value = this.PANEL161_SUP_txtsupplier_id.Text.Trim();  //15
                    cmd2.Parameters.Add("@txtcontact_person", SqlDbType.NVarChar).Value = this.txtcontact_person.Text.Trim();  //16
                    cmd2.Parameters.Add("@txtwant_mat_in_day", SqlDbType.NVarChar).Value = this.txtwant_mat_in_day.Text.Trim();  //17
                    cmd2.Parameters.Add("@txtdate_send_mat", SqlDbType.NVarChar).Value = this.dtpdate_send_mat.Text.Trim();  //18
                    cmd2.Parameters.Add("@txtcredit_in_day", SqlDbType.NVarChar).Value = this.txtcredit_in_day.Text.Trim();  //19
                    cmd2.Parameters.Add("@txtPPT_record_remark", SqlDbType.NVarChar).Value = this.txtPPT_record_remark.Text.Trim();  //20



                    cmd2.Parameters.Add("@txtemp_office_name_receive", SqlDbType.NVarChar).Value = this.txtemp_office_name_receive.Text.Trim();  //21
                    cmd2.Parameters.Add("@txtemp_office_name_audit", SqlDbType.NVarChar).Value = this.txtemp_office_name_audit.Text.Trim();  //21
                    cmd2.Parameters.Add("@txtemp_office_name_send", SqlDbType.NVarChar).Value = this.txtemp_office_name_send.Text.Trim();  //22

                    cmd2.Parameters.Add("@txtapprove_status_id", SqlDbType.NVarChar).Value = "";  //23

                    cmd2.Parameters.Add("@txtdepartment_id", SqlDbType.NVarChar).Value = "";  //24
                    cmd2.Parameters.Add("@txtproject_id", SqlDbType.NVarChar).Value = "";  //24
                    cmd2.Parameters.Add("@txtjob_id", SqlDbType.NVarChar).Value = "";  //25
                    cmd2.Parameters.Add("@txtjob_send_mat_status", SqlDbType.NVarChar).Value = "";  //26

                    cmd2.Parameters.Add("@txtmat_no", SqlDbType.NVarChar).Value = this.txtmat_no.Text.Trim();  //27
                    cmd2.Parameters.Add("@txtmat_id", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_id.Text.Trim();  //28
                    cmd2.Parameters.Add("@txtmat_name", SqlDbType.NVarChar).Value = this.PANEL_MAT_txtmat_name.Text.Trim();  //29
                    cmd2.Parameters.Add("@txtnumber_mat_id", SqlDbType.NVarChar).Value = this.PANEL0106_NUMBER_MAT_txtnumber_mat_id.Text.Trim();  //30

                    cmd2.Parameters.Add("@txtcurrency_id", SqlDbType.NVarChar).Value = this.txtcurrency_id.Text.Trim();  //31
                    cmd2.Parameters.Add("@txtcurrency_date", SqlDbType.NVarChar).Value = this.Paneldate_txtcurrency_date.Text.Trim();  //32
                    cmd2.Parameters.Add("@txtcurrency_rate", SqlDbType.NVarChar).Value = Convert.ToDouble(string.Format("{0:n0}", txtcurrency_rate.Text.ToString()));  //33

                    cmd2.Parameters.Add("@txtacc_group_tax_id", SqlDbType.NVarChar).Value = this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_id.Text.Trim();  //34

                    cmd2.Parameters.Add("@txtcost_qty_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_balance_yokma.Text.ToString()));  //25
                    cmd2.Parameters.Add("@txtcost_qty_price_average_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_price_average_yokma.Text.ToString()));  //26
                    cmd2.Parameters.Add("@txtcost_money_sum_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_money_sum_yokma.Text.ToString()));  //27


                    cmd2.Parameters.Add("@txtsum_qty_pub", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub.Text.ToString()));  //28
                    cmd2.Parameters.Add("@txtsum_qty_pub_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //29
                    cmd2.Parameters.Add("@txtsum_qty_pub_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub.Text.ToString()));  //30

                    cmd2.Parameters.Add("@txtsum_qty_pub_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub_kg.Text.ToString()));  //28
                    cmd2.Parameters.Add("@txtsum_qty_pub_receive_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //29
                    cmd2.Parameters.Add("@txtsum_qty_pub_balance_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_pub_kg.Text.ToString()));  //30

                    cmd2.Parameters.Add("@txtsum_qty_rib", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib.Text.ToString()));  //28
                    cmd2.Parameters.Add("@txtsum_qty_rib_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //29
                    cmd2.Parameters.Add("@txtsum_qty_rib_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib.Text.ToString()));  //30

                    cmd2.Parameters.Add("@txtsum_qty_rib_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib_kg.Text.ToString()));  //28
                    cmd2.Parameters.Add("@txtsum_qty_rib_receive_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //29
                    cmd2.Parameters.Add("@txtsum_qty_rib_balance_kg", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty_rib_kg.Text.ToString()));  //30

                    cmd2.Parameters.Add("@txtsum_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty.Text.ToString()));  //31
                    cmd2.Parameters.Add("@txtsum_qty_receive", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", 0));  //32
                    cmd2.Parameters.Add("@txtsum_qty_balance", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty.Text.ToString()));  //33


                    cmd2.Parameters.Add("@txtsum_price", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_price.Text.ToString()));  //34
                    cmd2.Parameters.Add("@txtsum_discount", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum_discount.Text.ToString()));  //35
                    cmd2.Parameters.Add("@txtmoney_sum", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_sum.Text.ToString()));  //36
                    cmd2.Parameters.Add("@txtmoney_tax_base", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_tax_base.Text.ToString()));  //37
                    cmd2.Parameters.Add("@txtvat_rate", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtvat_rate.Text.ToString()));  //38
                    cmd2.Parameters.Add("@txtvat_money", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtvat_money.Text.ToString()));  //39
                    cmd2.Parameters.Add("@txtmoney_after_vat", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_after_vat.Text.ToString()));  //40
                    cmd2.Parameters.Add("@txtmoney_after_vat_creditor", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtmoney_after_vat.Text.ToString()));  //41
                    cmd2.Parameters.Add("@txtcreditor_status", SqlDbType.NVarChar).Value = "0";  //42

                    cmd2.Parameters.Add("@txtcost_qty_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_balance_yokpai.Text.ToString()));  //43
                    cmd2.Parameters.Add("@txtcost_qty_price_average_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_price_average_yokpai.Text.ToString()));  //44
                    cmd2.Parameters.Add("@txtcost_money_sum_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_money_sum_yokpai.Text.ToString()));  //45

                    cmd2.Parameters.Add("@txtcost_qty2_balance_yokma", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty2_balance_yokma.Text.ToString()));  //46
                    cmd2.Parameters.Add("@txtsum2_qty", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtsum2_qty.Text.ToString()));  //47
                    cmd2.Parameters.Add("@txtcost_qty2_balance_yokpai", SqlDbType.Float).Value = Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty2_balance_yokpai.Text.ToString()));  //48

                    cmd2.Parameters.Add("@txtPPT_status", SqlDbType.NVarChar).Value = "0";  //49
                    cmd2.Parameters.Add("@txtapprove_status", SqlDbType.NVarChar).Value = "";  //50
                    cmd2.Parameters.Add("@txtRG_status", SqlDbType.NVarChar).Value = "";  //51
                    cmd2.Parameters.Add("@txtreceive_status", SqlDbType.NVarChar).Value = "";  //52
                    cmd2.Parameters.Add("@txtpayment_status", SqlDbType.NVarChar).Value = "";  //53
                    cmd2.Parameters.Add("@txtacc_record_status", SqlDbType.NVarChar).Value = "";  //54
                    cmd2.Parameters.Add("@txtemp_print", SqlDbType.NVarChar).Value = W_ID_Select.M_EMP_OFFICE_NAME.Trim();  //55
                    cmd2.Parameters.Add("@txtemp_print_datetime", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture);//55

                    //=====================================================================================================================================================
                    cmd2.ExecuteNonQuery();
                    //MessageBox.Show("ok2");



                    //3 c002_05Send_dye_record_detail



                    int s = 0;

                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        s = i + 1;
                        if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value != null)
                        {

                            this.GridView1.Rows[i].Cells["Col_Auto_num"].Value = s.ToString();


                                    if (Convert.ToBoolean(this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value) == true)
                                    {
                                        //this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "1";
                                        //DateTime want_receive_date = Convert.ToDateTime(this.GridView1.Rows[i].Cells["Col_date"].Value.ToString());
                                        //string want_date = want_receive_date.ToString("dd-MM-yyyy",ThaiCulture);
                            //        }
                            //        else
                            //        {
                            //            this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value = "0";
                            //        }

                            //if (Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) > 0)
                            // {

                                //===================================================================================================================
                                //3 c002_05Send_dye_record_detail

                                 cmd2.CommandText = "INSERT INTO c002_05Send_dye_record_detail(cdkey,txtco_id,txtbranch_id," +  //1
                                "txttrans_year,txttrans_month,txttrans_day," +

                                // //=================================================================
                                "txtPPT_id," +  //6
                                 "txtqc_id," +  //7
                                 "txtnumber_in_year," +  //8
                                 "txtwherehouse_id," +  //9
                                 "txtfold_number," +  //10
                                 "txtnumber_mat_id," +  //11
                                 "txtnumber_color_id," +  //12
                                 "txtface_baking_id," +  //13

                                 "txtdate_send," + //14

                                 "txtmat_no," +  //15
                                 "txtmat_id," +  //16
                                 "txtmat_name," +  //17

                                 "txtmat_unit1_name," +  //18
                                 "txtmat_unit1_qty," +  //19
                                  "chmat_unit_status," +  //20
                                 "txtmat_unit2_name," +  //21
                                 "txtmat_unit2_qty," +  //22

                                "txtqty_want," +  //23
                                "txtqty," +  //24
                               "txtqty2," +  //25
                               "txtqty_balance," +  //26

                                "txtqty_want_pub," +  //27
                                "txtqty_pub," +  //28
                                "txtqty_balance_pub," +  //29

                                "txtqty_want_rib," +  //27
                                "txtqty_rib," +  //28
                                "txtqty_balance_rib," +  //29


                                 "txtprice," +   //30
                                 "txtdiscount_rate," +  //31
                                 "txtdiscount_money," +  //32
                                 "txtsum_total," +  //33

                                  "txtcost_qty_balance_yokma," +  //34
                                  "txtcost_qty_price_average_yokma," +  //35
                                  "txtcost_money_sum_yokma," +  //36

                                  "txtcost_qty_balance_yokpai," +  //37
                                  "txtcost_qty_price_average_yokpai," +  //38
                                  "txtcost_money_sum_yokpai," +  //39

                                  "txtcost_qty2_balance_yokma," +  //40
                                  "txtcost_qty2_balance_yokpai," +  //41

                                "txtwant_receive_date," +  //42
                                "txtitem_no," +  //43
                                "txtmat_ppt_remark," +  //44

                                      "txtLot_no," +  //33
                                      "txtLot_no_status," +  //33

                                      "txtqty_receive_yokma," +  //33
                                      "txtqty_receive_yokpai," +  //33

                                       "txtqty_after_receive_yokpai," +  //34
                                   "txtqty_receive," +  //52
                                   "txtqty_after_receive," +  //53

                                   "txtreceive_id) " +  //54

                                "VALUES ('" + W_ID_Select.CDKEY.Trim() + "','" + W_ID_Select.M_COID.Trim() + "','" + W_ID_Select.M_BRANCHID.Trim() + "'," +  //1
                                "'" + myDateTime.ToString("yyyy", UsaCulture) + "','" + myDateTime.ToString("MM", UsaCulture) + "','" + myDateTime.ToString("dd", UsaCulture) + "'," +

                                 "'" + this.txtPPT_id.Text.Trim() + "'," +  //6
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtqc_id"].Value.ToString() + "'," +  //7
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "'," +  //8
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "'," +  //9

                                 "'" + this.GridView1.Rows[i].Cells["Col_txtfold_number"].Value.ToString() + "'," +  //10
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_mat_id"].Value.ToString() + "'," +  //11
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtnumber_color_id"].Value.ToString() + "'," +  //12
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "'," +  //13



                                 "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'," +  //14

                                 "'" + this.GridView1.Rows[i].Cells["Col_txtmat_no"].Value.ToString() + "'," +  //15
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "'," +  //16
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtmat_name"].Value.ToString() + "'," +    //17

                                 "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit1_name"].Value.ToString() + "'," +  //18
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtmat_unit1_qty"].Value.ToString())) + "'," +  //19
                                 "'" + this.GridView1.Rows[i].Cells["Col_chmat_unit_status"].Value.ToString() + "'," +  //20
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtmat_unit2_name"].Value.ToString() + "'," +  //21
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtmat_unit2_qty"].Value.ToString())) + "'," +  //22

                                 "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //23
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //24
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty2"].Value.ToString())) + "'," +  //25
                                 "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //26

                                 "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) + "'," +  //27
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //28
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_pub"].Value.ToString())) + "'," +  //29

                                 "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString())) + "'," +  //27
                                 "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //28
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_qty_rib"].Value.ToString())) + "'," +  //29


                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtprice"].Value.ToString())) + "'," +  //30
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtdiscount_rate"].Value.ToString())) + "'," +  //31
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtdiscount_money"].Value.ToString())) + "'," +  //32
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtsum_total"].Value.ToString())) + "'," +  //33

                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokma"].Value.ToString())) + "'," +  //34
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokma"].Value.ToString())) + "'," +  //35
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokma"].Value.ToString())) + "'," +  //36

                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_balance_yokpai"].Value.ToString())) + "'," +  //37
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_qty_price_average_yokpai"].Value.ToString())) + "'," +  //38
                                "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtcost_money_sum_yokpai"].Value.ToString())) + "'," +  //39

                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokma"].Value.ToString())) + "'," +  //40
                                "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtcost_qty2_balance_yokpai"].Value.ToString())) + "'," +  //41

                                 "'" + this.GridView1.Rows[i].Cells["Col_date"].Value.ToString() + "'," +  //42
                                 "'" + this.GridView1.Rows[i].Cells["Col_txtitem_no"].Value.ToString() + "'," +  //43
                                 "''," +  //44

                                "'" + this.GridView1.Rows[i].Cells["Col_txtLot_no"].Value.ToString() + "'," +  //23
                                "'0'," +

                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokma"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +  //44
                               "'" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "'," +   //45

                           "'" + Convert.ToDouble(string.Format("{0:n4}", 0)) + "'," +  //52
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty"].Value.ToString())) + "'," +  //53

                            "'')";   //54

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok3");


                                //===================================================================================================================
                                // c002_02produce_record_detail
                                cmd2.CommandText = "UPDATE c002_02produce_record_detail SET " +
                                                   "txtppt_status = '0'," +
                                                   "txtppt_id = '" + this.txtPPT_id.Text.ToString() + "'," +
                                                    "txtqty_cut = '" + Convert.ToDouble(string.Format("{0:n4}", this.GridView1.Rows[i].Cells["Col_txtqty_cut_yokpai"].Value.ToString())) + "'," +
                                                   "txtqty_after_cut = '" + Convert.ToDouble(string.Format("{0:n0}", this.GridView1.Rows[i].Cells["Col_txtqty_after_cut_yokpai"].Value.ToString())) + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                    " AND (txtLot_no = '" + this.GridView1.Rows[i].Cells["Col_txtLot_no"].Value.ToString() + "')" +
                                                   " AND (txtmat_id = '" + this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() + "')";

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok7");

                                cmd2.CommandText = "UPDATE c002_03QC_record_detail SET " +
                                                   "txtppt_status = '0'," +
                                                   "txtppt_id = '" + this.txtPPT_id.Text.ToString() + "'" +
                                                   " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                   " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                   " AND (txtwherehouse_id = '" + this.GridView1.Rows[i].Cells["Col_txtwherehouse_id"].Value.ToString() + "')" +
                                                   " AND (txtface_baking_id = '" + this.GridView1.Rows[i].Cells["Col_txtface_baking_id"].Value.ToString() + "')" +
                                                   " AND (txtnumber_in_year = '" + this.GridView1.Rows[i].Cells["Col_txtnumber_in_year"].Value.ToString() + "')" +
                                                   " AND (txtlot_no = '" + this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() + "')" +
                                                   " AND (txtppt_id = '')";

                                cmd2.ExecuteNonQuery();
                                //MessageBox.Show("ok7");


                                //" WHERE (c002_03QC_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                //" AND (c002_03QC_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                //" AND (c002_03QC_record_detail.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                //" AND (c002_03QC_record_detail.txtppt_id = '')" +


                                //=====================================================================================================
                            }
                        }
                    }



                    //สต๊อคสินค้า ตามคลัง =============================================================================================



                    //1.k021_mat_average
                    cmd2.CommandText = "UPDATE k021_mat_average SET " +
                                       "txtcost_qty_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +
                                       "txtcost_qty_price_average = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +
                                        "txtcost_money_sum = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +
                                       "txtcost_qty2_balance = '" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'" +
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


                            "'" + this.txtPPT_id.Text.Trim() + "'," +  //7 txtbill_id
                            "'PPT'," +  //9 txtbill_type
                            "'ส่งย้อม " + this.txtPPT_record_remark.Text.Trim() + "'," +  //9 txtbill_remark

                             "'" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "'," +  //7 txtwherehouse_id
                           "'" + this.txtmat_no.Text + "'," +  //10 
                            "'" + this.PANEL_MAT_txtmat_id.Text.ToString() + "'," +  //11
                            "'" + this.PANEL_MAT_txtmat_name.Text.ToString() + "'," +    //12

                            "'" + this.txtmat_unit1_name.Text.ToString() + "'," +  //13
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtmat_unit1_qty.Text.ToString())) + "'," +  //14
                            "'" + this.chmat_unit_status.Text.ToString() + "'," +  //15
                            "'" + this.txtmat_unit2_name.Text.ToString() + "'," +  //16
                           "'" + Convert.ToDouble(string.Format("{0:n4}", this.txtmat_unit2_qty.Text.ToString())) + "'," +  //17

                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //18  txtqty_in
                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //19 txtqty2_in
                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //20 txtprice_in
                           "'" + Convert.ToDouble(string.Format("{0:n0}", 0)) + "'," +  //21 txtsum_total_in

                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_qty.Text.ToString())) + "'," +  //22 txtqty_out
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum2_qty.Text.ToString())) + "'," +  //23 txtqty2_out
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtprice.Text.ToString())) + "'," +  //24 txtprice_out
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtsum_total.Text.ToString())) + "'," +  //25   // **** เป็นราคาที่ยังไม่หักส่วนลด มาคิดต้นทุนถัวเฉลี่ย txtsum_total_out

                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_balance_yokpai.Text.ToString())) + "'," +  //26
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty2_balance_yokpai.Text.ToString())) + "'," +  //27
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_qty_price_average_yokpai.Text.ToString())) + "'," +  //28
                           "'" + Convert.ToDouble(string.Format("{0:n0}", this.txtcost_money_sum_yokpai.Text.ToString())) + "'," +  //29

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

                        if (this.iblword_status.Text.Trim() == "บันทึกใบส่งผ้าย้อม")
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
            W_ID_Select.TRANS_ID = this.txtPPT_id.Text.Trim();
            W_ID_Select.LOG_ID = "8";
            W_ID_Select.LOG_NAME = "ปริ๊น";
            TRANS_LOG();
            //======================================================
            kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record_print frm2 = new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye_record_print();
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
            W_ID_Select.TRANS_ID = this.txtPPT_id.Text.Trim();
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
                rpt.Load("C:\\KD_ERP\\KD_REPORT\\Report_c002_05Send_dye_record.rpt");


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
                rpt.SetParameterValue("txtppt_id", W_ID_Select.TRANS_ID.Trim());

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

        private void dtpdate_send_mat_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_send_mat.Format = DateTimePickerFormat.Custom;
            this.dtpdate_send_mat.CustomFormat = this.dtpdate_send_mat.Value.ToString("dd-MM-yyyy", UsaCulture);

        }

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
                    this.txtcontact_person.Text = row.Cells["Col_txtcontact_person"].Value.ToString();
                    //Col_txtcredit_day
                    this.txtcredit_in_day.Text = row.Cells["Col_txtcredit_day"].Value.ToString();
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

        //END txtwherehouse คลังสินค้า  =======================================================================


        //txtnumber_color รหัสสี  =======================================================================
        private void PANEL0107_NUMBER_COLOR_Fill_number_color()
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

            PANEL0107_NUMBER_COLOR_Clear_GridView1_number_color();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_07number_color" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtnumber_color_id <> '')" +
                                    " ORDER BY txtnumber_color_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtnumber_color_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtnumber_color_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtnumber_color_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtnumber_color_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtnumber_color_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtnumber_color_status";

                            var index = PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows.Add();
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_no"].Value = dt2.Rows[j]["txtnumber_color_no"].ToString();      //1
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();      //2
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_name"].Value = dt2.Rows[j]["txtnumber_color_name"].ToString();      //3
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_name_eng"].Value = dt2.Rows[j]["txtnumber_color_name_eng"].ToString();      //4
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_remark"].Value = dt2.Rows[j]["txtnumber_color_remark"].ToString();      //5
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_status"].Value = dt2.Rows[j]["txtnumber_color_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0107_NUMBER_COLOR_dataGridView1_number_color_Up_Status();

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
        private void PANEL0107_NUMBER_COLOR_dataGridView1_number_color_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows.Count; i++)
            {
                if (this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0107_NUMBER_COLOR_GridView1_number_color()
        {
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.ColumnCount = 7;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[0].Name = "Col_Auto_num";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].Name = "Col_txtnumber_color_no";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].Name = "Col_txtnumber_color_id";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].Name = "Col_txtnumber_color_name";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].Name = "Col_txtnumber_color_name_eng";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].Name = "Col_txtnumber_color_remark";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[6].Name = "Col_txtnumber_color_status";

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[0].HeaderText = "No";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].HeaderText = " รหัส";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].HeaderText = " ชื่อรหัสสี";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].HeaderText = "ชื่อรหัสสี Eng";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[6].HeaderText = " สถานะ";

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[0].Visible = false;  //"No";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].Visible = true;  //"Col_txtnumber_color_no";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].Width = 90;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].ReadOnly = true;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].Visible = true;  //"Col_txtnumber_color_id";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].Width = 80;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].ReadOnly = true;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].Visible = true;  //"Col_txtnumber_color_name";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].Width = 150;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].ReadOnly = true;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].Visible = false;  //"Col_txtnumber_color_name_eng";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].Width = 0;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].ReadOnly = true;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].Visible = false;  //"Col_txtnumber_color_name_remark";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].Width = 0;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].ReadOnly = true;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[6].Visible = false;  //"Col_txtnumber_color_status";
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[6].Width = 0;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Columns.Add(dgvCmb);

        }
        private void PANEL0107_NUMBER_COLOR_Clear_GridView1_number_color()
        {
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows.Clear();
            this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Refresh();
        }
        private void PANEL0107_NUMBER_COLOR_txtnumber_color_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0107_NUMBER_COLOR.Visible == false)
                {
                    this.PANEL0107_NUMBER_COLOR.Visible = true;
                    this.PANEL0107_NUMBER_COLOR.Location = new Point(this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Location.X, this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Location.Y + 22);
                    this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Focus();
                }
                else
                {
                    this.PANEL0107_NUMBER_COLOR.Visible = false;
                }
        }
        private void PANEL0107_NUMBER_COLOR_btnnumber_color_Click(object sender, EventArgs e)
        {
            if (this.PANEL0107_NUMBER_COLOR.Visible == false)
            {
                this.PANEL0107_NUMBER_COLOR.Visible = true;
                this.PANEL0107_NUMBER_COLOR.BringToFront();
                this.PANEL0107_NUMBER_COLOR.Location = new Point(this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Location.X, this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0107_NUMBER_COLOR.Visible = false;
            }
        }
        private void PANEL0107_NUMBER_COLOR_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0107_NUMBER_COLOR.Visible == false)
            {
                this.PANEL0107_NUMBER_COLOR.Visible = true;
            }
            else
            {
                this.PANEL0107_NUMBER_COLOR.Visible = false;
            }
        }
        private void PANEL0107_NUMBER_COLOR_dataGridView1_number_color_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text = row.Cells[3].Value.ToString();
                }
            }
        }
        private void PANEL0107_NUMBER_COLOR_dataGridView1_number_color_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0107_NUMBER_COLOR_dataGridView1_number_color.CurrentRow.Index;

                this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text = PANEL0107_NUMBER_COLOR_dataGridView1_number_color.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text = PANEL0107_NUMBER_COLOR_dataGridView1_number_color.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Focus();
                this.PANEL0107_NUMBER_COLOR.Visible = false;
            }
        }
        private void PANEL0107_NUMBER_COLOR_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0107_NUMBER_COLOR_btn_search_Click(object sender, EventArgs e)
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

            PANEL0107_NUMBER_COLOR_Clear_GridView1_number_color();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_07number_color" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtnumber_color_name LIKE '%" + this.PANEL0107_NUMBER_COLOR_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtnumber_color_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtnumber_color_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtnumber_color_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtnumber_color_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtnumber_color_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtnumber_color_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtnumber_color_status";

                            var index = PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows.Add();
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_no"].Value = dt2.Rows[j]["txtnumber_color_no"].ToString();      //1
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_id"].Value = dt2.Rows[j]["txtnumber_color_id"].ToString();      //2
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_name"].Value = dt2.Rows[j]["txtnumber_color_name"].ToString();      //3
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_name_eng"].Value = dt2.Rows[j]["txtnumber_color_name_eng"].ToString();      //4
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_remark"].Value = dt2.Rows[j]["txtnumber_color_remark"].ToString();      //5
                            PANEL0107_NUMBER_COLOR_dataGridView1_number_color.Rows[index].Cells["Col_txtnumber_color_status"].Value = dt2.Rows[j]["txtnumber_color_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0107_NUMBER_COLOR_dataGridView1_number_color_Up_Status();

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
        private void PANEL0107_NUMBER_COLOR_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0107_NUMBER_COLOR_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0107_NUMBER_COLOR.Height = PANEL0107_NUMBER_COLOR_btnresize_low.Top + e.Y;
                this.PANEL0107_NUMBER_COLOR.Width = PANEL0107_NUMBER_COLOR_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0107_NUMBER_COLOR_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0107_NUMBER_COLOR_btnnew_Click(object sender, EventArgs e)
        {

        }

        //END txtnumber_color รหัสสี =======================================================================


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
                this.PANEL1313_ACC_GROUP_TAX.Visible = true;
                this.PANEL1313_ACC_GROUP_TAX.BringToFront();
                this.PANEL1313_ACC_GROUP_TAX.Location = new Point(this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.X - PANEL1313_ACC_GROUP_TAX.Height - 53, this.PANEL1313_ACC_GROUP_TAX_txtacc_group_tax_name.Location.Y - PANEL1313_ACC_GROUP_TAX.Height - 2);
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
        //txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================

        //txtface_baking ประเภท อบหน้า  =======================================================================
        private void PANEL0105_FACE_BAKING_Fill_face_baking()
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

            PANEL0105_FACE_BAKING_Clear_GridView1_face_baking();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_05face_baking" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtface_baking_id <> '')" +
                                    " ORDER BY txtface_baking_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtface_baking_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtface_baking_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtface_baking_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtface_baking_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtface_baking_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtface_baking_status";

                            var index = PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Add();
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_no"].Value = dt2.Rows[j]["txtface_baking_no"].ToString();      //1
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();      //2
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //3
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name_eng"].Value = dt2.Rows[j]["txtface_baking_name_eng"].ToString();      //4
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_remark"].Value = dt2.Rows[j]["txtface_baking_remark"].ToString();      //5
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_status"].Value = dt2.Rows[j]["txtface_baking_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0105_FACE_BAKING_dataGridView1_face_baking_Up_Status();

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
        private void PANEL0105_FACE_BAKING_dataGridView1_face_baking_Up_Status()
        {
            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Count; i++)
            {
                if (this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[i].Cells[6].Value.ToString() == "0")  //Active
                {
                    this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[i].Cells[7].Value = true;
                }
                else
                {
                    this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[i].Cells[7].Value = false;

                }
            }

        }
        private void PANEL0105_FACE_BAKING_GridView1_face_baking()
        {
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnCount = 7;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].Name = "Col_Auto_num";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Name = "Col_txtface_baking_no";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Name = "Col_txtface_baking_id";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Name = "Col_txtface_baking_name";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Name = "Col_txtface_baking_name_eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Name = "Col_txtface_baking_remark";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].Name = "Col_txtface_baking_status";

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].HeaderText = "No";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].HeaderText = "ลำดับ";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].HeaderText = " รหัส";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].HeaderText = " อบหน้า";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].HeaderText = "อบหน้า  Eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].HeaderText = " หมายเหตุ";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].HeaderText = " สถานะ";

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[0].Visible = false;  //"No";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Visible = false;  //"Col_txtface_baking_no";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Visible = false;  //"Col_txtface_baking_id";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Visible = true;  //"Col_txtface_baking_name";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].Width = 150;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Visible = false;  //"Col_txtface_baking_name_eng";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Visible = false;  //"Col_txtface_baking_name_remark";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].ReadOnly = true;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].Visible = false;  //"Col_txtface_baking_status";
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].Width = 0;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.GridColor = Color.FromArgb(227, 227, 227);

            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.EnableHeadersVisualStyles = false;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "Col_Chk";
            dgvCmb.HeaderText = "สถานะ";
            dgvCmb.ReadOnly = true;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Columns.Add(dgvCmb);

        }
        private void PANEL0105_FACE_BAKING_Clear_GridView1_face_baking()
        {
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Clear();
            this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Refresh();
        }
        private void PANEL0105_FACE_BAKING_txtface_baking_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

                if (this.PANEL0105_FACE_BAKING.Visible == false)
                {
                    this.PANEL0105_FACE_BAKING.Visible = true;
                    this.PANEL0105_FACE_BAKING.Location = new Point(this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.X, this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.Y + 22);
                    this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Focus();
                }
                else
                {
                    this.PANEL0105_FACE_BAKING.Visible = false;
                }
        }
        private void PANEL0105_FACE_BAKING_btnface_baking_Click(object sender, EventArgs e)
        {
            if (this.PANEL0105_FACE_BAKING.Visible == false)
            {
                this.PANEL0105_FACE_BAKING.Visible = true;
                this.PANEL0105_FACE_BAKING.BringToFront();
                this.PANEL0105_FACE_BAKING.Location = new Point(this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.X, this.PANEL0105_FACE_BAKING_txtface_baking_name.Location.Y + 22);
            }
            else
            {
                this.PANEL0105_FACE_BAKING.Visible = false;
            }
        }
        private void PANEL0105_FACE_BAKING_btnclose_Click(object sender, EventArgs e)
        {
            if (this.PANEL0105_FACE_BAKING.Visible == false)
            {
                this.PANEL0105_FACE_BAKING.Visible = true;
            }
            else
            {
                this.PANEL0105_FACE_BAKING.Visible = false;
            }
        }
        private void PANEL0105_FACE_BAKING_dataGridView1_face_baking_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[e.RowIndex];

                var cell = row.Cells[1].Value;
                if (cell != null)
                {
                    this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = row.Cells[2].Value.ToString();
                    this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = row.Cells[3].Value.ToString();
                    Fill_Show_DATA_GridView66();
                }
            }
        }
        private void PANEL0105_FACE_BAKING_dataGridView1_face_baking_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = PANEL0105_FACE_BAKING_dataGridView1_face_baking.CurrentRow.Index;

                this.PANEL0105_FACE_BAKING_txtface_baking_id.Text = PANEL0105_FACE_BAKING_dataGridView1_face_baking.CurrentRow.Cells[1].Value.ToString();
                this.PANEL0105_FACE_BAKING_txtface_baking_name.Text = PANEL0105_FACE_BAKING_dataGridView1_face_baking.CurrentRow.Cells[2].Value.ToString();
                this.PANEL0105_FACE_BAKING_txtface_baking_name.Focus();
                this.PANEL0105_FACE_BAKING.Visible = false;
            }
        }
        private void PANEL0105_FACE_BAKING_txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void PANEL0105_FACE_BAKING_btn_search_Click(object sender, EventArgs e)
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

            PANEL0105_FACE_BAKING_Clear_GridView1_face_baking();


            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;

                cmd2.CommandText = "SELECT *" +
                                    " FROM c001_05face_baking" +
                                    " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (txtface_baking_name LIKE '%" + this.PANEL0105_FACE_BAKING_txtsearch.Text.Trim() + "%')" +
                                    " ORDER BY txtface_baking_no ASC";


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
                            //this.PANEL_FORM1_dataGridView1.Columns[1].Name = "Col_txtface_baking_no";
                            //this.PANEL_FORM1_dataGridView1.Columns[2].Name = "Col_txtface_baking_id";
                            //this.PANEL_FORM1_dataGridView1.Columns[3].Name = "Col_txtface_baking_name";
                            //this.PANEL_FORM1_dataGridView1.Columns[4].Name = "Col_txtface_baking_name_eng";
                            //this.PANEL_FORM1_dataGridView1.Columns[5].Name = "Col_txtface_baking_name_remark";
                            //this.PANEL_FORM1_dataGridView1.Columns[6].Name = "Col_txtface_baking_status";

                            var index = PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows.Add();
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_no"].Value = dt2.Rows[j]["txtface_baking_no"].ToString();      //1
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();      //2
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name"].Value = dt2.Rows[j]["txtface_baking_name"].ToString();      //3
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_name_eng"].Value = dt2.Rows[j]["txtface_baking_name_eng"].ToString();      //4
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_remark"].Value = dt2.Rows[j]["txtface_baking_remark"].ToString();      //5
                            PANEL0105_FACE_BAKING_dataGridView1_face_baking.Rows[index].Cells["Col_txtface_baking_status"].Value = dt2.Rows[j]["txtface_baking_status"].ToString();      //8
                        }
                        //=======================================================
                    }
                    else
                    {
                        // MessageBox.Show("Not found k006db_sale_record2020  ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        // return;
                    }
                    PANEL0105_FACE_BAKING_dataGridView1_face_baking_Up_Status();

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
        private void PANEL0105_FACE_BAKING_btnresize_low_MouseDown(object sender, MouseEventArgs e)
        {
            allowResize = true;
        }
        private void PANEL0105_FACE_BAKING_btnresize_low_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.PANEL0105_FACE_BAKING.Height = PANEL0105_FACE_BAKING_btnresize_low.Top + e.Y;
                this.PANEL0105_FACE_BAKING.Width = PANEL0105_FACE_BAKING_btnresize_low.Left + e.X;
            }
        }
        private void PANEL0105_FACE_BAKING_btnresize_low_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
        }
        private void PANEL0105_FACE_BAKING_btnnew_Click(object sender, EventArgs e)
        {

        }






        private void Fill_Show_DATA_GridView66()
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

            Clear_GridView66();

            Cursor.Current = Cursors.WaitCursor;

            //เชื่อมต่อฐานข้อมูล======================================================
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = conn;


                cmd2.CommandText = "SELECT c002_02produce_record.*," +
                                   "c002_02produce_record_detail.*," +
                                   "c001_04produce_type.*," +
                                   "c001_02machine.*," +
                                   "c001_05face_baking.*," +
                                   //"c001_06number_mat.*," +

                                   "k013_1db_acc_13group_tax.*," +

                                   "k013_1db_acc_06wherehouse.*" +

                                   " FROM c002_02produce_record" +

                                   " INNER JOIN c002_02produce_record_detail" +
                                   " ON c002_02produce_record.cdkey = c002_02produce_record_detail.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c002_02produce_record_detail.txtco_id" +
                                   " AND c002_02produce_record.txticrf_id = c002_02produce_record_detail.txticrf_id" +

                                   " INNER JOIN c001_04produce_type" +
                                   " ON c002_02produce_record.cdkey = c001_04produce_type.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_04produce_type.txtco_id" +
                                   " AND c002_02produce_record.txtproduce_type_id = c001_04produce_type.txtproduce_type_id" +

                                   " INNER JOIN c001_02machine" +
                                   " ON c002_02produce_record.cdkey = c001_02machine.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_02machine.txtco_id" +
                                   " AND c002_02produce_record.txtmachine_id = c001_02machine.txtmachine_id" +

                                   " INNER JOIN c001_05face_baking" +
                                   " ON c002_02produce_record.cdkey = c001_05face_baking.cdkey" +
                                   " AND c002_02produce_record.txtco_id = c001_05face_baking.txtco_id" +
                                   " AND c002_02produce_record.txtface_baking_id = c001_05face_baking.txtface_baking_id" +

                                   //" INNER JOIN c001_06number_mat" +
                                   //" ON c002_02produce_record.cdkey = c001_06number_mat.cdkey" +
                                   //" AND c002_02produce_record.txtco_id = c001_06number_mat.txtco_id" +
                                   //" AND c002_02produce_record.txtnumber_mat_id = c001_06number_mat.txtnumber_mat_id" +


                                   " INNER JOIN k013_1db_acc_13group_tax" +
                                   " ON c002_02produce_record.txtacc_group_tax_id = k013_1db_acc_13group_tax.txtacc_group_tax_id" +


                                   " INNER JOIN k013_1db_acc_06wherehouse" +
                                   " ON c002_02produce_record.cdkey = k013_1db_acc_06wherehouse.cdkey" +
                                   " AND c002_02produce_record.txtco_id = k013_1db_acc_06wherehouse.txtco_id" +
                                   " AND c002_02produce_record.txtwherehouse_id = k013_1db_acc_06wherehouse.txtwherehouse_id" +

                                    " WHERE (c002_02produce_record.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                    " AND (c002_02produce_record.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                    " AND (c002_02produce_record.txticrf_status = '0')" +
                                      //" AND (c002_02produce_record_detail.txtmat_id = '" + W_ID_Select.MAT_ID + "')" +

                                      //" AND (c002_02produce_record.txticrf_id = '" + W_ID_Select.TRANS_ID.Trim() + "')" +
                                      //" AND (c002_02produce_record.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                                      //" AND (c002_02produce_record_detail.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                      " AND (c002_02produce_record_detail.txtface_baking_id = '" + this.PANEL0105_FACE_BAKING_txtface_baking_id.Text.Trim() + "')" +
                                  " AND (c002_02produce_record_detail.txtqty_after_cut > 0)" +
                                    " ORDER BY c002_02produce_record_detail.txtLot_no ASC";

                // " AND (k021_mat_average_balance.txttrans_date_server BETWEEN @datestart AND @dateend)" +
                //" ORDER BY k021_mat_average_balance.ID ASC";

                //cmd2.Parameters.Add("@datestart", SqlDbType.Date).Value = this.dtpstart.Value;
                //cmd2.Parameters.Add("@dateend", SqlDbType.Date).Value = this.dtpend.Value;


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

                            var index = GridView66.Rows.Add();
                            GridView66.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
                            GridView66.Rows[index].Cells["Col_txtwherehouse_id"].Value = dt2.Rows[j]["txtwherehouse_id"].ToString();      //1
                            GridView66.Rows[index].Cells["Col_txtmachine_id"].Value = dt2.Rows[j]["txtmachine_id"].ToString();      //2
                            GridView66.Rows[index].Cells["Col_txtfold_number"].Value = dt2.Rows[j]["txtfold_number"].ToString();      //3

                            GridView66.Rows[index].Cells["Col_txtqty"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty"]).ToString("###,###.00");      //4

                            GridView66.Rows[index].Cells["Col_txttrans_time_start"].Value = dt2.Rows[j]["txttrans_time_start"].ToString();      //5
                            GridView66.Rows[index].Cells["Col_txttrans_time_end"].Value = dt2.Rows[j]["txttrans_time_end"].ToString();      //6

                            GridView66.Rows[index].Cells["Col_Problem1"].Value = Convert.ToSingle(dt2.Rows[j]["Problem1"]).ToString("###,##0");      //7
                            GridView66.Rows[index].Cells["Col_Problem2"].Value = Convert.ToSingle(dt2.Rows[j]["Problem2"]).ToString("###,##0");      //8
                            GridView66.Rows[index].Cells["Col_Problem3"].Value = Convert.ToSingle(dt2.Rows[j]["Problem3"]).ToString("###,##0");      //9
                            GridView66.Rows[index].Cells["Col_Problem4"].Value = Convert.ToSingle(dt2.Rows[j]["Problem4"]).ToString("###,##0");      //10

                            GridView66.Rows[index].Cells["Col_txtemp_id"].Value = dt2.Rows[j]["txtemp_id"].ToString();      //11
                            GridView66.Rows[index].Cells["Col_txtemp_name"].Value = dt2.Rows[j]["txtemp_name"].ToString();      //12
                            GridView66.Rows[index].Cells["Col_txtshift_name"].Value = dt2.Rows[j]["txtshift_name"].ToString();      //13


                            GridView66.Rows[index].Cells["Col_txticrf_remark"].Value = dt2.Rows[j]["txticrf_remark"].ToString();      //14

                            GridView66.Rows[index].Cells["Col_txtmat_no"].Value = dt2.Rows[j]["txtmat_no"].ToString();      //15
                            GridView66.Rows[index].Cells["Col_txtmat_id"].Value = dt2.Rows[j]["txtmat_id"].ToString();      //16
                            GridView66.Rows[index].Cells["Col_txtmat_name"].Value = dt2.Rows[j]["txtmat_name"].ToString();      //17
                            GridView66.Rows[index].Cells["Col_txtnumber_mat_id"].Value = dt2.Rows[j]["txtnumber_mat_id"].ToString();      //18

                            GridView66.Rows[index].Cells["Col_txtmat_unit1_name"].Value = dt2.Rows[j]["txtmat_unit1_name"].ToString();      //19
                            GridView66.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit1_qty"]).ToString("###,###.00");      //20

                            GridView66.Rows[index].Cells["Col_chmat_unit_status"].Value = dt2.Rows[j]["chmat_unit_status"].ToString();      //21

                            GridView66.Rows[index].Cells["Col_txtmat_unit2_name"].Value = dt2.Rows[j]["txtmat_unit2_name"].ToString();      //22
                            GridView66.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToSingle(dt2.Rows[j]["txtmat_unit2_qty"]).ToString("###,###.0000");      //23

                            GridView66.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty2"]).ToString("###,###.00");      //24


                            GridView66.Rows[index].Cells["Col_txtprice"].Value = Convert.ToSingle(dt2.Rows[j]["txtprice"]).ToString("###,###.00");        //25
                            GridView66.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_rate"]).ToString("###,###.00");      //26
                            GridView66.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToSingle(dt2.Rows[j]["txtdiscount_money"]).ToString("###,###.00");      //27
                            GridView66.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToSingle(dt2.Rows[j]["txtsum_total"]).ToString("###,###.00");      //28

                            GridView66.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance_yokma"]).ToString("###,###.00");      //29
                            GridView66.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average_yokma"]).ToString("###,###.00");      //30
                            GridView66.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum_yokma"]).ToString("###,###.00");      //31

                            GridView66.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_balance_yokpai"]).ToString("###,###.00");      //32
                            GridView66.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty_price_average_yokpai"]).ToString("###,###.00");      //33
                            GridView66.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_money_sum_yokpai"]).ToString("###,###.00");      //34

                            GridView66.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance_yokma"]).ToString("###,###.00");      //35
                            GridView66.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = Convert.ToSingle(dt2.Rows[j]["txtcost_qty2_balance_yokpai"]).ToString("###,###.00");      //36

                            GridView66.Rows[index].Cells["Col_txtitem_no"].Value = dt2.Rows[j]["txtitem_no"].ToString();      //37

                            GridView66.Rows[index].Cells["Col_mat_status"].Value = "0";

                            GridView66.Rows[index].Cells["Col_txtface_baking_id"].Value = dt2.Rows[j]["txtface_baking_id"].ToString();     //41
                            GridView66.Rows[index].Cells["Col_txtlot_no"].Value = dt2.Rows[j]["txtlot_no"].ToString();     //42

                            GridView66.Rows[index].Cells["Col_txtqty_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_cut"]).ToString("###,###.00");      //35
                            GridView66.Rows[index].Cells["Col_txtqty_after_cut"].Value = Convert.ToSingle(dt2.Rows[j]["txtqty_after_cut"]).ToString("###,###.00");      //36

                            GridView66.Rows[index].Cells["Col_txtcut_id"].Value = dt2.Rows[j]["txtcut_id"].ToString();      //37

                            GridView66.Rows[index].Cells["Col_1"].Value = "1";      //37


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
            GridView66_Color_Column();

        }
        private void Show_GridView66()
        {
            this.GridView66.ColumnCount = 45;
            this.GridView66.Columns[0].Name = "Col_Auto_num";
            this.GridView66.Columns[1].Name = "Col_txtwherehouse_id";
            this.GridView66.Columns[2].Name = "Col_txtmachine_id";
            this.GridView66.Columns[3].Name = "Col_txtfold_number";

            this.GridView66.Columns[4].Name = "Col_txtqty";

            this.GridView66.Columns[5].Name = "Col_txttrans_time_start";
            this.GridView66.Columns[6].Name = "Col_txttrans_time_end";

            this.GridView66.Columns[7].Name = "Col_Problem1";
            this.GridView66.Columns[8].Name = "Col_Problem2";
            this.GridView66.Columns[9].Name = "Col_Problem3";
            this.GridView66.Columns[10].Name = "Col_Problem4";

            this.GridView66.Columns[11].Name = "Col_txtemp_id";
            this.GridView66.Columns[12].Name = "Col_txtemp_name";

            this.GridView66.Columns[13].Name = "Col_txtshift_name";

            this.GridView66.Columns[14].Name = "Col_txticrf_remark";


            this.GridView66.Columns[15].Name = "Col_txtmat_no";
            this.GridView66.Columns[16].Name = "Col_txtmat_id";
            this.GridView66.Columns[17].Name = "Col_txtmat_name";
            this.GridView66.Columns[18].Name = "Col_txtnumber_mat_id";

            this.GridView66.Columns[19].Name = "Col_txtmat_unit1_name";
            this.GridView66.Columns[20].Name = "Col_txtmat_unit1_qty";
            this.GridView66.Columns[21].Name = "Col_chmat_unit_status";
            this.GridView66.Columns[22].Name = "Col_txtmat_unit2_name";
            this.GridView66.Columns[23].Name = "Col_txtmat_unit2_qty";

            this.GridView66.Columns[24].Name = "Col_txtqty2";

            this.GridView66.Columns[25].Name = "Col_txtprice";
            this.GridView66.Columns[26].Name = "Col_txtdiscount_rate";
            this.GridView66.Columns[27].Name = "Col_txtdiscount_money";
            this.GridView66.Columns[28].Name = "Col_txtsum_total";

            this.GridView66.Columns[29].Name = "Col_txtcost_qty_balance_yokma";
            this.GridView66.Columns[30].Name = "Col_txtcost_qty_price_average_yokma";
            this.GridView66.Columns[31].Name = "Col_txtcost_money_sum_yokma";

            this.GridView66.Columns[32].Name = "Col_txtcost_qty_balance_yokpai";
            this.GridView66.Columns[33].Name = "Col_txtcost_qty_price_average_yokpai";
            this.GridView66.Columns[34].Name = "Col_txtcost_money_sum_yokpai";

            this.GridView66.Columns[35].Name = "Col_txtcost_qty2_balance_yokma";
            this.GridView66.Columns[36].Name = "Col_txtcost_qty2_balance_yokpai";

            this.GridView66.Columns[37].Name = "Col_txtitem_no";
            this.GridView66.Columns[38].Name = "Col_mat_status";
            this.GridView66.Columns[39].Name = "Col_txtface_baking_id";
            this.GridView66.Columns[40].Name = "Col_txtlot_no";

            this.GridView66.Columns[41].Name = "Col_txtqty_cut";
            this.GridView66.Columns[42].Name = "Col_txtqty_after_cut";
            this.GridView66.Columns[43].Name = "Col_txtcut_id";

            this.GridView66.Columns[44].Name = "Col_1";


            this.GridView66.Columns[0].HeaderText = "No";
            this.GridView66.Columns[1].HeaderText = "คลัง";
            this.GridView66.Columns[2].HeaderText = "เครื่องจักร";
            this.GridView66.Columns[3].HeaderText = "ม้วนที่";

            this.GridView66.Columns[4].HeaderText = "น้ำหนัก/ม้วน (กก.)";

            this.GridView66.Columns[5].HeaderText = " เวลาเริ่ม";
            this.GridView66.Columns[6].HeaderText = " เวลาเสร็จ";

            this.GridView66.Columns[7].HeaderText = "เข็มหัก";
            this.GridView66.Columns[8].HeaderText = "เป็นรู";
            this.GridView66.Columns[9].HeaderText = "ผ้าตก";
            this.GridView66.Columns[10].HeaderText = "ด้ายขาด";

            this.GridView66.Columns[11].HeaderText = "รหัสผู้ดูแล";
            this.GridView66.Columns[12].HeaderText = "ชื่อผู้ดูแล";
            this.GridView66.Columns[13].HeaderText = "กะ";
            this.GridView66.Columns[14].HeaderText = "หมายเหตุ";

            this.GridView66.Columns[15].HeaderText = "ลำดับ";
            this.GridView66.Columns[16].HeaderText = "รหัส";
            this.GridView66.Columns[17].HeaderText = "ชื่อสินค้า";
            this.GridView66.Columns[18].HeaderText = "เบอร์เส้นด้าย";

            this.GridView66.Columns[19].HeaderText = " หน่วยหลัก";
            this.GridView66.Columns[20].HeaderText = " หน่วย";
            this.GridView66.Columns[21].HeaderText = "แปลง";
            this.GridView66.Columns[22].HeaderText = " หน่วย(ปอนด์)";
            this.GridView66.Columns[23].HeaderText = " หน่วย";

            this.GridView66.Columns[24].HeaderText = "น้ำหนัก/ม้วน(ปอนด์)";

            this.GridView66.Columns[25].HeaderText = "ราคา";
            this.GridView66.Columns[26].HeaderText = "ส่วนลด(%)";
            this.GridView66.Columns[27].HeaderText = "ส่วนลด(บาท)";
            this.GridView66.Columns[28].HeaderText = "จำนวนเงิน(บาท)";

            this.GridView66.Columns[29].HeaderText = "จำนวนยกมา";
            this.GridView66.Columns[30].HeaderText = "ราคาเฉลี่ยยกมา";
            this.GridView66.Columns[31].HeaderText = "จำนวนเงิน";

            this.GridView66.Columns[32].HeaderText = "จำนวนยกไป";
            this.GridView66.Columns[33].HeaderText = "ราคาเฉลี่ยยกไป";
            this.GridView66.Columns[34].HeaderText = "จำนวนเงิน";

            this.GridView66.Columns[35].HeaderText = "จำนวน(แปลงหน่วย)ยกมา";
            this.GridView66.Columns[36].HeaderText = "จำนวน(แปลงหน่วย)ยกไป";

            this.GridView66.Columns[37].HeaderText = "item_no";
            this.GridView66.Columns[38].HeaderText = "สถานะ";
            this.GridView66.Columns[39].HeaderText = "อบหน้า";
            this.GridView66.Columns[40].HeaderText = "Lot No";

            this.GridView66.Columns[41].HeaderText = "จำนวนส่งย้อม";  //กก
            this.GridView66.Columns[42].HeaderText = "จำนวนเหลือ";  //กก
            this.GridView66.Columns[43].HeaderText = "เลขที่ส่งย้อม";  //

            this.GridView66.Columns[44].HeaderText = "1";  //

            this.GridView66.Columns["Col_Auto_num"].Visible = false;  //"Col_Auto_num";
            this.GridView66.Columns["Col_Auto_num"].Width = 0;
            this.GridView66.Columns["Col_Auto_num"].ReadOnly = true;
            this.GridView66.Columns["Col_Auto_num"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_Auto_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtwherehouse_id"].Visible = false;  //"Col_txtwherehouse_id";
            this.GridView66.Columns["Col_txtwherehouse_id"].Width = 0;
            this.GridView66.Columns["Col_txtwherehouse_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtwherehouse_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtwherehouse_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txtmachine_id"].Visible = true;  //"Col_txtmachine_id";
            this.GridView66.Columns["Col_txtmachine_id"].Width = 80;
            this.GridView66.Columns["Col_txtmachine_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmachine_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmachine_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView66.Columns["Col_txtfold_number"].Visible = true;  //"Col_txtfold_number";
            this.GridView66.Columns["Col_txtfold_number"].Width = 60;
            this.GridView66.Columns["Col_txtfold_number"].ReadOnly = true;
            this.GridView66.Columns["Col_txtfold_number"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtfold_number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txtqty"].Visible = true;  //"Col_txtqty";
            this.GridView66.Columns["Col_txtqty"].Width = 140;
            this.GridView66.Columns["Col_txtqty"].ReadOnly = false;
            this.GridView66.Columns["Col_txtqty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtqty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //DataGridViewTextBoxColumn txttime = new DataGridViewTextBoxColumn();
            //txttime.Name = "Col_Time1";
            //txttime.Width =60;
            //txttime.DisplayIndex = 2;
            //txttime.HeaderText = "เวลา";
            //txttime.DefaultCellStyle.Format = ("HH:mm");
            ////cboemp.MaxDropDownItems = 100;
            ////cboemp.ReadOnly = false;
            //txttime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //txttime.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            //txttime.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //this.GridView66.Columns.Add(txttime);

            this.GridView66.Columns["Col_txttrans_time_start"].Visible = false;  //"Col_txttrans_time_start";
            this.GridView66.Columns["Col_txttrans_time_start"].Width = 0;
            this.GridView66.Columns["Col_txttrans_time_start"].ReadOnly = false;
            this.GridView66.Columns["Col_txttrans_time_start"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txttrans_time_start"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView66.Columns["Col_txttrans_time_end"].Visible = false;  //"Col_txttrans_time_end";
            this.GridView66.Columns["Col_txttrans_time_end"].Width = 0;
            this.GridView66.Columns["Col_txttrans_time_end"].ReadOnly = false;
            this.GridView66.Columns["Col_txttrans_time_end"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txttrans_time_end"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_Problem1"].Visible = false;  //"Col_Problem1";
            this.GridView66.Columns["Col_Problem1"].Width = 0;
            this.GridView66.Columns["Col_Problem1"].ReadOnly = false;
            this.GridView66.Columns["Col_Problem1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_Problem1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_Problem2"].Visible = false;  //"Col_Problem2";
            this.GridView66.Columns["Col_Problem2"].Width = 0;
            this.GridView66.Columns["Col_Problem2"].ReadOnly = false;
            this.GridView66.Columns["Col_Problem2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_Problem2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_Problem3"].Visible = false;  //"Col_Problem3";
            this.GridView66.Columns["Col_Problem3"].Width = 0;
            this.GridView66.Columns["Col_Problem3"].ReadOnly = false;
            this.GridView66.Columns["Col_Problem3"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_Problem3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_Problem4"].Visible = false;  //"Col_Problem4";
            this.GridView66.Columns["Col_Problem4"].Width = 0;
            this.GridView66.Columns["Col_Problem4"].ReadOnly = false;
            this.GridView66.Columns["Col_Problem4"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_Problem4"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            //DataGridViewComboBoxColumn cboemp = new DataGridViewComboBoxColumn();
            //cboemp.Name = "Col_Combo1";
            //cboemp.Width = 200;
            //cboemp.DisplayIndex = 21;
            //cboemp.HeaderText = "ผู้เบิก...";
            ////cboemp.MaxDropDownItems = 100;
            ////cboemp.ReadOnly = false;
            //cboemp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //cboemp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            //cboemp.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            //this.GridView66.Columns.Add(cboemp);

            this.GridView66.Columns["Col_txtemp_id"].Visible = false;  //"Col_txtemp_id";
            this.GridView66.Columns["Col_txtemp_id"].Width = 0;
            this.GridView66.Columns["Col_txtemp_id"].ReadOnly = false;
            this.GridView66.Columns["Col_txtemp_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtemp_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtemp_name"].Visible = false;  //"Col_txtemp_name";
            this.GridView66.Columns["Col_txtemp_name"].Width = 0;
            this.GridView66.Columns["Col_txtemp_name"].ReadOnly = false;
            this.GridView66.Columns["Col_txtemp_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtemp_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtshift_name"].Visible = false;  //"Col_txtshift_name";
            this.GridView66.Columns["Col_txtshift_name"].Width = 0;
            this.GridView66.Columns["Col_txtshift_name"].ReadOnly = false;
            this.GridView66.Columns["Col_txtshift_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtshift_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txticrf_remark"].Visible = false;  //"Col_txticrf_remark";
            this.GridView66.Columns["Col_txticrf_remark"].Width = 0;
            this.GridView66.Columns["Col_txticrf_remark"].ReadOnly = false;
            this.GridView66.Columns["Col_txticrf_remark"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txticrf_remark"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txtmat_no"].Visible = false;  //"Col_txtmat_no";
            this.GridView66.Columns["Col_txtmat_no"].Width = 0;
            this.GridView66.Columns["Col_txtmat_no"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtmat_id"].Visible = false;  //"Col_txtmat_id";
            this.GridView66.Columns["Col_txtmat_id"].Width = 0;
            this.GridView66.Columns["Col_txtmat_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.GridView66.Columns["Col_txtmat_name"].Visible = true;  //"Col_txtmat_name";
            this.GridView66.Columns["Col_txtmat_name"].Width = 200;
            this.GridView66.Columns["Col_txtmat_name"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtnumber_mat_id"].Visible = false;  //"Col_txtnumber_mat_id";
            this.GridView66.Columns["Col_txtnumber_mat_id"].Width = 0;
            this.GridView66.Columns["Col_txtnumber_mat_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtnumber_mat_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtnumber_mat_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtmat_unit1_name"].Visible = false;  //"Col_txtmat_unit1_name";
            this.GridView66.Columns["Col_txtmat_unit1_name"].Width = 0;
            this.GridView66.Columns["Col_txtmat_unit1_name"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_unit1_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_unit1_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txtmat_unit1_qty"].Visible = false;  //"Col_txtmat_unit1_qty";
            this.GridView66.Columns["Col_txtmat_unit1_qty"].Width = 0;
            this.GridView66.Columns["Col_txtmat_unit1_qty"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_unit1_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_unit1_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_chmat_unit_status"].Visible = false;  //"Col_chmat_unit_status";
            this.GridView66.Columns["Col_chmat_unit_status"].Width = 0;
            this.GridView66.Columns["Col_chmat_unit_status"].ReadOnly = true;
            this.GridView66.Columns["Col_chmat_unit_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_chmat_unit_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.Name = "Col_Chk1";
            dgvCmb.Width = 0;  //70
            dgvCmb.DisplayIndex = 20;
            dgvCmb.HeaderText = "แปลงหน่วย?";
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.ReadOnly = true;
            dgvCmb.Visible = false;
            dgvCmb.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCmb.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            GridView66.Columns.Add(dgvCmb);

            this.GridView66.Columns["Col_txtmat_unit2_name"].Visible = false;  //"Col_txtmat_unit2_name";
            this.GridView66.Columns["Col_txtmat_unit2_name"].Width = 0;
            this.GridView66.Columns["Col_txtmat_unit2_name"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_unit2_name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_unit2_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_txtmat_unit2_qty"].Visible = false;  //"Col_txtmat_unit2_qty";
            this.GridView66.Columns["Col_txtmat_unit2_qty"].Width = 0;
            this.GridView66.Columns["Col_txtmat_unit2_qty"].ReadOnly = true;
            this.GridView66.Columns["Col_txtmat_unit2_qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtmat_unit2_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;




            this.GridView66.Columns["Col_txtqty2"].Visible = false;  //"Col_txtqty2";
            this.GridView66.Columns["Col_txtqty2"].Width = 0;
            this.GridView66.Columns["Col_txtqty2"].ReadOnly = true;
            this.GridView66.Columns["Col_txtqty2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtqty2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            this.GridView66.Columns["Col_txtprice"].Visible = false;  //"Col_txtprice";
            this.GridView66.Columns["Col_txtprice"].Width = 0;
            this.GridView66.Columns["Col_txtprice"].ReadOnly = true;
            this.GridView66.Columns["Col_txtprice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtdiscount_rate"].Visible = false;  //"Col_txtdiscount_rate";
            this.GridView66.Columns["Col_txtdiscount_rate"].Width = 0;
            this.GridView66.Columns["Col_txtdiscount_rate"].ReadOnly = true;
            this.GridView66.Columns["Col_txtdiscount_rate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtdiscount_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtdiscount_money"].Visible = false;  //"Col_txtdiscount_money";
            this.GridView66.Columns["Col_txtdiscount_money"].Width = 0;
            this.GridView66.Columns["Col_txtdiscount_money"].ReadOnly = false;
            this.GridView66.Columns["Col_txtdiscount_money"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtdiscount_money"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtsum_total"].Visible = false;  //"Col_txtsum_total";
            this.GridView66.Columns["Col_txtsum_total"].Width = 0;
            this.GridView66.Columns["Col_txtsum_total"].ReadOnly = true;
            this.GridView66.Columns["Col_txtsum_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtsum_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_qty_balance_yokma"].Visible = false;  //"Col_txtcost_qty_balance_yokma";
            this.GridView66.Columns["Col_txtcost_qty_balance_yokma"].Width = 0;
            this.GridView66.Columns["Col_txtcost_qty_balance_yokma"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_qty_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_qty_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_qty_price_average_yokma"].Visible = false;  //"Col_txtcost_qty_price_average_yokma";
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokma"].Width = 0;
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokma"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_money_sum_yokma"].Visible = false;  //"Col_txtcost_money_sum_yokma";
            this.GridView66.Columns["Col_txtcost_money_sum_yokma"].Width = 0;
            this.GridView66.Columns["Col_txtcost_money_sum_yokma"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_money_sum_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_money_sum_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_qty_balance_yokpai"].Visible = false;  //"Col_txtcost_qty_balance_yokpai";
            this.GridView66.Columns["Col_txtcost_qty_balance_yokpai"].Width = 0;
            this.GridView66.Columns["Col_txtcost_qty_balance_yokpai"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_qty_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_qty_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_qty_price_average_yokpai"].Visible = false;  //"Col_txtcost_qty_price_average_yokpai";
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokpai"].Width = 0;
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokpai"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_qty_price_average_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_money_sum_yokpai"].Visible = false;  //"Col_txtcost_money_sum_yokpai";
            this.GridView66.Columns["Col_txtcost_money_sum_yokpai"].Width = 0;
            this.GridView66.Columns["Col_txtcost_money_sum_yokpai"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_money_sum_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_money_sum_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_qty2_balance_yokma"].Visible = false;  //"Col_txtcost_qty2_balance_yokma";
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokma"].Width = 0;
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokma"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokma"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcost_qty2_balance_yokpai"].Visible = false;  //"Col_txtcost_qty2_balance_yokpai";
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokpai"].Width = 0;
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokpai"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokpai"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcost_qty2_balance_yokpai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtitem_no"].Visible = false;  //"Col_txtitem_no";
            this.GridView66.Columns["Col_txtitem_no"].Width = 0;
            this.GridView66.Columns["Col_txtitem_no"].ReadOnly = true;
            this.GridView66.Columns["Col_txtitem_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtitem_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_mat_status"].Visible = false;  //"Col_mat_status";
            this.GridView66.Columns["Col_mat_status"].Width = 0;
            this.GridView66.Columns["Col_mat_status"].ReadOnly = true;
            this.GridView66.Columns["Col_mat_status"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_mat_status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtface_baking_id"].Visible = true;  //"Col_txtface_baking_id";
            this.GridView66.Columns["Col_txtface_baking_id"].Width = 80;
            this.GridView66.Columns["Col_txtface_baking_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtface_baking_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtface_baking_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtlot_no"].Visible = true;  //"Col_txtlot_no";
            this.GridView66.Columns["Col_txtlot_no"].Width = 180;
            this.GridView66.Columns["Col_txtlot_no"].ReadOnly = true;
            this.GridView66.Columns["Col_txtlot_no"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtlot_no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.Columns["Col_txtqty_cut"].Visible = true;  //"Col_txtqty_cut";
            this.GridView66.Columns["Col_txtqty_cut"].Width = 100;
            this.GridView66.Columns["Col_txtqty_cut"].ReadOnly = true;
            this.GridView66.Columns["Col_txtqty_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtqty_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtqty_after_cut"].Visible = true;  //"Col_txtqty_after_cut";
            this.GridView66.Columns["Col_txtqty_after_cut"].Width = 100;
            this.GridView66.Columns["Col_txtqty_after_cut"].ReadOnly = true;
            this.GridView66.Columns["Col_txtqty_after_cut"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtqty_after_cut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            this.GridView66.Columns["Col_txtcut_id"].Visible = true;  //"Col_txtcut_id";
            this.GridView66.Columns["Col_txtcut_id"].Width = 160;
            this.GridView66.Columns["Col_txtcut_id"].ReadOnly = true;
            this.GridView66.Columns["Col_txtcut_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_txtcut_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            this.GridView66.Columns["Col_1"].Visible = false;  //"Col_1";
            this.GridView66.Columns["Col_1"].Width = 0;
            this.GridView66.Columns["Col_1"].ReadOnly = true;
            this.GridView66.Columns["Col_1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.GridView66.Columns["Col_1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.GridView66.DefaultCellStyle.Font = new Font("Tahoma", 8F);
            this.GridView66.GridColor = Color.FromArgb(227, 227, 227);

            this.GridView66.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.GridView66.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.GridView66.ColumnHeadersDefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            this.GridView66.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Pixel); //Segoe UI, 11pt
            this.GridView66.EnableHeadersVisualStyles = false;

        }
        private void Clear_GridView66()
        {
            this.GridView66.Rows.Clear();
            this.GridView66.Refresh();
        }
        private void GridView66_Color_Column()
        {

            for (int i = 0; i < this.GridView66.Rows.Count - 0; i++)
            {
                GridView66.Rows[i].Cells["Col_txtmat_name"].Style.BackColor = Color.LightGoldenrodYellow;
                GridView66.Rows[i].Cells["Col_txtqty"].Style.BackColor = Color.LightSkyBlue;
                GridView66.Rows[i].Cells["Col_txtqty_after_cut"].Style.BackColor = Color.LightSkyBlue;
            }
        }
        private void GridView66_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.GridView66.Rows[e.RowIndex];

                var cell = row.Cells["Col_txtmat_id"].Value;
                if (cell != null)
                {
                    
                    if (this.txtnumber_in_year.Text == "")
                    {
                        MessageBox.Show("โปรดใส่เลข ชุดที่ ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.PANEL161_SUP_txtsupplier_name.Text == "")
                    {
                        MessageBox.Show("โปรด เลือก Supplier ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.PANEL0105_FACE_BAKING_txtface_baking_name.Text == "")
                    {
                        MessageBox.Show("โปรด เลือก อบหน้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.PANEL1306_WH_txtwherehouse_name.Text == "")
                    {
                        MessageBox.Show("โปรด เลือก คลังสินค้า ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (this.PANEL0107_NUMBER_COLOR_txtnumber_color_name.Text == "")
                    {
                        MessageBox.Show("โปรด เลือก รหัสสี ก่อน !", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //======================================================

                    this.PANEL_MAT_txtmat_id.Text = row.Cells["Col_txtmat_id"].Value.ToString();
                    //======================================================
                    //Fill_DATA_TO_GridView1();
                    SHOW_MAT();
                }
                //=====================
            }
        }
        private void SHOW_MAT()
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

            //PANEL_MAT_Clear_GridView1();

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
                                   //"k021_mat_average.*," +

                                   "b001_05mat_unit1.*," +
                                   "b001_05mat_unit2.*," +
                                   "b001_05mat_unit3.*," +
                                   "b001_05mat_unit4.*," +
                                   "b001_05mat_unit5.*" +

                                   " FROM b001mat" +

                                   " INNER JOIN b001mat_02detail" +
                                   " ON b001mat.cdkey = b001mat_02detail.cdkey" +
                                   " AND b001mat.txtco_id = b001mat_02detail.txtco_id" +
                                   " AND b001mat.txtmat_id = b001mat_02detail.txtmat_id" +

                                   //" INNER JOIN k021_mat_average" +
                                   //" ON k018db_po_record_detail.cdkey = k021_mat_average.cdkey" +
                                   //" AND k018db_po_record_detail.txtco_id = k021_mat_average.txtco_id" +
                                   //" AND k018db_po_record_detail.txtmat_id = k021_mat_average.txtmat_id" +

                                   " INNER JOIN b001_05mat_unit1" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit1.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit1.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit1_id = b001_05mat_unit1.txtmat_unit1_id" +

                                   " INNER JOIN b001_05mat_unit2" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit2.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit2.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit2_id = b001_05mat_unit2.txtmat_unit2_id" +

                                   " INNER JOIN b001_05mat_unit3" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit3.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit3.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit3_id = b001_05mat_unit3.txtmat_unit3_id" +

                                   " INNER JOIN b001_05mat_unit4" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit4.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit4.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit4_id = b001_05mat_unit4.txtmat_unit4_id" +

                                   " INNER JOIN b001_05mat_unit5" +
                                   " ON b001mat_02detail.cdkey = b001_05mat_unit5.cdkey" +
                                   " AND b001mat_02detail.txtco_id = b001_05mat_unit5.txtco_id" +
                                   " AND b001mat_02detail.txtmat_unit5_id = b001_05mat_unit5.txtmat_unit5_id" +

                                   " WHERE (b001mat.cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                   " AND (b001mat.txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                   " AND (b001mat.txtmat_id = '" + this.PANEL_MAT_txtmat_id.Text.Trim() + "')" +
                                   //" AND (k021_mat_average.txtwherehouse_id = '" + this.PANEL1306_WH_txtwherehouse_id.Text.Trim() + "')" +
                                   " ORDER BY b001mat.txtmat_no ASC";

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
                        this.txtmat_unit1_name.Text = dt2.Rows[0]["txtmat_unit1_name"].ToString();
                        this.txtmat_unit1_qty.Text = dt2.Rows[0]["txtmat_unit1_qty"].ToString();
                        this.chmat_unit_status.Text = dt2.Rows[0]["chmat_unit_status"].ToString();
                        this.txtmat_unit2_name.Text = dt2.Rows[0]["txtmat_unit2_name"].ToString();
                        this.txtmat_unit2_qty.Text = dt2.Rows[0]["txtmat_unit2_qty"].ToString();


 
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
        private void GridView66_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Green)
                {

                }
                else
                {
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
            }
        }
        private void GridView66_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Green)
                {

                }
                else
                {
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    GridView66.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Tahoma", 8F);

                }
            }
        }
        private void GridView66_DoubleClick(object sender, EventArgs e)
        {
            selectedRowIndex = GridView66.CurrentRow.Index;

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                if (this.GridView1.Rows[i].Cells["Col_txtlot_no"].Value.ToString() == this.GridView66.Rows[selectedRowIndex].Cells["Col_txtlot_no"].Value.ToString())
                {
                    MessageBox.Show("Lot No นี้ เพิ่มเข้าไปใน ตารางแล้ว ");
                    return;
                }
                if (this.GridView1.Rows[i].Cells["Col_txtmat_id"].Value.ToString() == this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_id"].Value.ToString())
                {

                }
                else
                {
                    MessageBox.Show("ระบบจะให้ส่งย้อมผ้าดิบ ได้ที่ละ 1 รหัสผ้าดิบ ต่อ 1 ใบส่งย้อม เท่านั้น !! ");
                    return;
                }
            }


            GridView66.Rows[selectedRowIndex].DefaultCellStyle.BackColor = Color.Green;

            var index = this.GridView1.Rows.Add();
            this.GridView1.Rows[index].Cells["Col_Auto_num"].Value = ""; //0
            this.GridView1.Rows[index].Cells["Col_txtwherehouse_id"].Value = this.PANEL1306_WH_txtwherehouse_id.Text.ToString();      //1
            this.GridView1.Rows[index].Cells["Col_txtnumber_in_year"].Value =this.txtnumber_in_year.Text.ToString();      //2
            this.GridView1.Rows[index].Cells["Col_txtnumber_mat_id"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtnumber_mat_id"].Value.ToString();      //3
            this.GridView1.Rows[index].Cells["Col_txtnumber_color_id"].Value = this.PANEL0107_NUMBER_COLOR_txtnumber_color_id.Text.ToString();      //4
            this.GridView1.Rows[index].Cells["Col_txtface_baking_id"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtface_baking_id"].Value.ToString();        //5


            this.GridView1.Rows[index].Cells["Col_txtlot_no"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtlot_no"].Value.ToString();      //6
            this.GridView1.Rows[index].Cells["Col_txtfold_number"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtfold_number"].Value.ToString();      //7

            this.GridView1.Rows[index].Cells["Col_txtqty"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtqty"].Value.ToString()));    //8

            this.GridView1.Rows[index].Cells["Col_txtmat_no"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_no"].Value.ToString();      //9
            this.GridView1.Rows[index].Cells["Col_txtmat_id"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_id"].Value.ToString();     //10
            this.GridView1.Rows[index].Cells["Col_txtmat_name"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_name"].Value.ToString();      //11

            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_name"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_unit1_name"].Value.ToString();      //12
            this.GridView1.Rows[index].Cells["Col_txtmat_unit1_qty"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_unit1_qty"].Value.ToString()));      //13

            this.GridView1.Rows[index].Cells["Col_chmat_unit_status"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_chmat_unit_status"].Value.ToString();       //14

            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_name"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_unit2_name"].Value.ToString();     //15
            this.GridView1.Rows[index].Cells["Col_txtmat_unit2_qty"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtmat_unit2_qty"].Value.ToString()));      //16

            this.GridView1.Rows[index].Cells["Col_txtqty2"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtqty2"].Value.ToString()));    //17


            this.GridView1.Rows[index].Cells["Col_txtprice"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtprice"].Value.ToString()));       //18
            this.GridView1.Rows[index].Cells["Col_txtdiscount_rate"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtdiscount_rate"].Value.ToString()));      //19
            this.GridView1.Rows[index].Cells["Col_txtdiscount_money"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtdiscount_money"].Value.ToString()));      //20
            this.GridView1.Rows[index].Cells["Col_txtsum_total"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtsum_total"].Value.ToString()));     //21

            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokma"].Value = ".00";      //22
            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokma"].Value = ".00";       //23
            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokma"].Value = ".00";       //24

            this.GridView1.Rows[index].Cells["Col_txtcost_qty_balance_yokpai"].Value = ".00";       //25
            this.GridView1.Rows[index].Cells["Col_txtcost_qty_price_average_yokpai"].Value = ".00";        //26
            this.GridView1.Rows[index].Cells["Col_txtcost_money_sum_yokpai"].Value = ".00";       //27

            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokma"].Value = ".00";        //28
            this.GridView1.Rows[index].Cells["Col_txtcost_qty2_balance_yokpai"].Value = ".00";        //29

            this.GridView1.Rows[index].Cells["Col_txtitem_no"].Value = this.GridView66.Rows[selectedRowIndex].Cells["Col_txtitem_no"].Value.ToString();       //30

            this.GridView1.Rows[index].Cells["Col_txtqc_id"].Value = "" ;      //31
            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub"].Value = "0";      //32

            //GridView1.Rows[index].Cells["Col_date"].Value = Convert.ToDateTime(dt2.Rows[j]["txtwant_receive_date"]).ToString("yyyy-MM-dd", UsaCulture);     //9
            GridView1.Rows[index].Cells["Col_date"].Value = this.dtpdate_send_mat.Value.ToString("dd-MM-yyyy", UsaCulture);     //9


            //this.GridView1.Columns[40].Name = "Col_txtsum_qty_rib";
            //this.GridView1.Columns[41].Name = "Col_txtsum_qty_rib_want";
            //this.GridView1.Columns[42].Name = "Col_txtsum_qty_rib_receive";
            //this.GridView1.Columns[43].Name = "Col_txtsum_qty_rib_balance";
            this.GridView1.Rows[index].Cells["Col_qty_Cal"].Value = "0";      //32
            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib"].Value = "0";       //20
            this.GridView1.Rows[index].Cells["Col_txtsum_qty_pub_kg"].Value = "0";      //21
            this.GridView1.Rows[index].Cells["Col_txtsum_qty_rib_kg"].Value = "0";       //22

            GridView1.Rows[index].Cells["Col_txtqty_after_cut"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtqty_after_cut"].Value.ToString()));       //21
            GridView1.Rows[index].Cells["Col_txtqty_cut_yokma"].Value = Convert.ToDouble(string.Format("{0:n}", this.GridView66.Rows[selectedRowIndex].Cells["Col_txtqty_cut"].Value.ToString()));     //36
            GridView1.Rows[index].Cells["Col_txtqty_cut_yokpai"].Value = "0";      //37
            GridView1.Rows[index].Cells["Col_txtqty_after_cut_yokpai"].Value = "0";      //37

            //สถานะ Checkbox =======================================================
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                    this.GridView1.Rows[i].Cells["Col_Chk_SELECT"].Value = true;
            }


            Show_Qty_Yokma();
            GridView1_Cal_Sum();
            Sum_group_tax();

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
                                  " FROM c002_05Send_dye_record_trans" +
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
                            TMP = "PPT" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + trans.Trim();
                        }
                        else
                        {
                            TMP = "PPT" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";
                        }

                    }

                    else
                    {
                        W_ID_Select.TRANS_BILL_STATUS = "N";
                        TMP = "PPT" + W_ID_Select.M_BRANCHNAME_SHORT.Trim() + "-" + year_now2.Trim() + "" + month_now.Trim() + "" + day_now.Trim() + "-" + "000001";

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
                this.txtPPT_id.Text = TMP.Trim();
                string RN = TMP.Substring(TMP.Length - 3);
            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================



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

                    cmd2.CommandText = "UPDATE c002_05Send_dye_record SET " +
                                                                 "txtemp_print = '" + W_ID_Select.M_EMP_OFFICE_NAME.Trim() + "'," +
                                                                 "txtemp_print_datetime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", UsaCulture) + "'" +
                                                                " WHERE (cdkey = '" + W_ID_Select.CDKEY.Trim() + "')" +
                                                               " AND (txtco_id = '" + W_ID_Select.M_COID.Trim() + "')" +
                                                               " AND (txtPPT_id = '" + this.txtPPT_id.Text.Trim() + "')";
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

        private void FillDATE_FROM_SERVER()
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

                    cmd2.CommandText = "UPDATE A001_date_now SET " +
                                                                 "datetime_now = GETDATE()";
                    cmd2.ExecuteNonQuery();



                    Cursor.Current = Cursors.WaitCursor;
                    trans.Commit();
                    conn.Close();
                    Cursor.Current = Cursors.Default;

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




            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
            {

                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = conn;

                cmd1.CommandText = "SELECT datetime_now" +
                                  " FROM A001_date_now";
                try
                {
                    cmd1.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //this.txtdate_from_server.Text = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("dd-MM-yyyy", ThaiCulture);          //4
                        //this.txttime_from_server.Text = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("HH:mm:ss", ThaiCulture);          //4

                        string D1 = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("yyyy-MM-dd", ThaiCulture);          //4
                        string T1 = Convert.ToDateTime(dt.Rows[0]["datetime_now"]).ToString("HH:mm:ss", ThaiCulture);          //4
                        W_ID_Select.DATE_FROM_SERVER = D1.ToString();
                        W_ID_Select.TIME_FROM_SERVER = T1.ToString();

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
        //END txtface_baking ประเภท อบหน้า =======================================================================

        //จบส่วนตารางสำหรับบันทึก========================================================================



        //Check ADD FORM========================================================================

        //END txtacc_group_taxรหัส กลุ่มภาษี  =======================================================================


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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GridView1_Cal_Sum();
        }

        private void dtpdate_record_ValueChanged(object sender, EventArgs e)
        {
            this.dtpdate_record.Format = DateTimePickerFormat.Custom;
            this.dtpdate_record.CustomFormat = this.dtpdate_record.Value.ToString("dd-MM-yyyy", UsaCulture);
            this.txtyear.Text = this.dtpdate_record.Value.ToString("yyyy", UsaCulture);

        }

        private void BtnGrid_Click(object sender, EventArgs e)
        {
            W_ID_Select.WORD_TOP = "ระเบียนใบส่งย้อม";
            kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye frm2 = new kondate.soft.HOME03_Production.HOME03_Production_05Send_Dye();
            frm2.Show();

        }

















        //================================================================================
    }
}
