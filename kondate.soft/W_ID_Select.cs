using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
//using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Deployment;
using System.Deployment.Application;
using System.Reflection;

namespace kondate.soft
{
    class W_ID_Select
    {
        //public static string ADATASOURCE = "ASAICAFEKLONG3\\SQLEXPRESS,49170";  //916909b5121b.sn.mynetname.net,6001 // C4PC_AOT\\SQLEXPRESS,49170" //ASAICAFEKLONG3\\SQLEXPRESS,49170   //172.168.0.15\\SQLEXPRESS,49170
        //public static string DATABASE_NAME = "KREST2020"; //KREST2020
        //public static string Lang = "001";// = "001"= ""; //001ไทย, 002Eng,003ลาว,004กัมพูชา,005พม่า
        //public static string CDKEY = "asaicafe25630622";//  = "chaixifactory2562020601"= "";// = "chaixifactory2562020601"= "";
        //public static string M_USERNAME = "admin";// = "admin"= ""; //= "admin"= "";
        //public static string M_COID = "05";// = "01"= ""; //= "01"= ""; //รหัสระบบ
        //public static string M_BRANCHID = "05001";// = "000001"= ""; //= "000001"= ""; //รหัสสาขา
        //public static string PRINT_NAME = "EPSON TM-T88V Receipt5";   //EPSON TM-T88VI Receipt5
        //public static string M_EMP_OFFICE_NAME = "ผู้ดูแลระบบ";// = "ชื่อพนักงาน"= ""

        //===============================================
        public static string ADATASOURCE = "";  //916909b5121b.sn.mynetname.net,6001 // C4PC_AOT\\SQLEXPRESS,49170" //ASAICAFEKLONG3\\SQLEXPRESS,49170   //172.168.0.15\\SQLEXPRESS,49170
        public static string DATABASE_NAME = ""; //KREST2020
        public static string Lang = "001";// = "001"= ""; //001ไทย, 002Eng,003ลาว,004กัมพูชา,005พม่า
        public static string CDKEY = "";//  = "chaixifactory2562020601"= "";// = "chaixifactory2562020601"= "";

        public static string COMPUTER_NAME = "";
        public static string COMPUTER_IP = "";

        public static string M_USERNAME = "";// = "admin"= ""; //= "admin"= "";
        public static string M_USERNAME_TYPE = "";// = "admin"= "";

        public static string M_COID = "";// = "01"= ""; //= "01"= ""; //รหัสระบบ
        public static string M_BRANCHID = "";// = "000001"= ""; //= "000001"= ""; //รหัสสาขา
        public static string PRINT_NAME = "";   //EPSON TM-T88VI Receipt5  พิมพ์ใบเสร็จ
        public static string PRINT_R_NAME1 = "EPSON TM-T88V Receipt5";   //EPSON TM-T88V Receipt5 พิมพ์ครัว1
        public static string PRINT_R_NAME2 = "EPSON TM-T88V Receipt5";   //EPSON TM-T88V Receipt5 พิมพ์ครัว2
        public static string PRINT_R_NAME3 = "EPSON TM-T88V Receipt5";   //EPSON TM-T88VI Receipt5 พิมพ์ครัว3
        public static string M_EMP_OFFICE_NAME = "";// = "ชื่อพนักงาน"= ""

        //=================================================

        //public static string ON_OR_OFF_LINE;
        //public static string conn_string = "Data Source=916909b5121b.sn.mynetname.net,6001;Initial Catalog=KREST6;User Id=sa;Password=Kon51Aot";  //SERVER 192.168.0.1
        //public static string conn_string = "Data Source=" + DATA_SOURCE + ";Initial Catalog=" + DATABASE_NAME + ";User Id=sa;Password=Kon51Aot";  //SERVER 192.168.0.1
        //public static string conn_string = "Data Source=" + ADATASOURCE + ";Initial Catalog=" + DATABASE_NAME + ";User Id=sa;Password=Kon51Aot";  //SERVER 192.168.0.1

        //CHAIXI_FAC_0001\SQLEXPRESS,49170
        //public static string conn_string = "Data Source=CHAIXI_CHA_0001\\SQLEXPRESS,49170;Initial Catalog=KREST6;User Id=sa;Password=Kon51Aot";  //Computer chaixi chayang
        //public static string conn_string = "Data Source=CHAIXI_FAC_0001\\SQLEXPRESS,49170;Initial Catalog=KREST6;User Id=sa;Password=Kon51Aot"; //Computer chaixi factory
        // public static string conn_string = "Data Source=C4PC_AOT\\SQLEXPRESS,49170;Initial Catalog=KREST6;User Id=sa;Password=Kon51Aot";  //Computer Office
        //public static string conn_string = "Data Source=KONDATE_AOT-PC\\SQLEXPRESS,49170;Initial Catalog=KREST6;User Id=sa;Password=Kon51Aot";  //Macbook pro
        //public static string conn_string = "Data Source=DESKTOP-19L8F9P\\SQLEXPRESS,49170;Initial Catalog=KREST6;User Id=sa;Password=Kon51Aot";  //Notebook Lenovo



        //public static string Crytal_SERVER = "916909b5121b.sn.mynetname.net,6001"; //SERVER 192.168.0.1
        public static string Crytal_SERVER = ADATASOURCE; //SERVER 192.168.0.1
        //public static string Crytal_SERVER = "CHAIXI_CHA_0001\\SQLEXPRESS,49170"; //Computer chaixi Chayang
        //public static string Crytal_SERVER = "CHAIXI_FAC_0001\\SQLEXPRESS,49170"; //Computer chaixi factoryC:\01Project_C\01K_Rest\KRest\KRest\W_ID_Select.cs
        // public static string Crytal_SERVER = "C4PC_AOT\\SQLEXPRESS,49170";  //Computer Office
        //public static string Crytal_SERVER = "KONDATE_AOT-PC\\SQLEXPRESS,49170";   //Macbook Pro
        //public static string Crytal_SERVER = "DESKTOP-19L8F9P\\SQLEXPRESS,49170";  //192.168.1.76  //Notebook Lenovo



        //public static string Crytal_SERVER = "192.168.0.1";
        public static string Crytal_DATABASE = DATABASE_NAME;
        public static string Crytal_USER = "sa";
        public static string Crytal_Pass = "Kon51Aot";

        public static string FROM_FORM = "";

        public static string IDS1 = "";
        public static string IDS2 = "";
        public static string IDS3 = "";
        public static string IDS4 = "";
        public static string IDS5 = "";
        public static string IDS6 = "";
        public static string IDS7 = "";
        public static string IDS8 = "";

        public static string PRICESALE = "";
        public static string DISCOUNTMONEY = "";
        public static string PRICE_AF_DISCOUNT = "";
        public static string PRICE_EXTRA = "";
        public static string PRICE_AF_EXTRA = "";
        public static string TOTAL_AF_VAT = "";


       public static int SLEEP = 0;
       public static string TRANS_ID = "";
        public static string MAT_ID = "";

        public static string TEST = "";
        public static string M_CONAME = ""; //= "บจก ทดสอบระบบ"= "";
        public static string M_BRANCHNAME = ""; //= "สาขาทดสอบระบบ"= "";
        public static string M_BRANCHNAME_SHORT = ""; //= "สาขาทดสอบระบบ"= "";

        public static string DATE_FROM_SERVER = "";
        public static string TIME_FROM_SERVER = "";

        public static string LOG_ID = "";
        public static string LOG_NAME = "";
        public static string DOCUMENT_ID = "";
        public static string VERSION_ID = "";
        public static string WORD_TOP = "";

        //เพิ่มเมนู สิทธิ ใหม
        public static string M_DEPART_NUMBER = "";   //เก็บค่าลำดับ เมื่อคลิ๊กแผนก 
        public static string M_DEPART_NAME = "";  //เก็บค่าชื่อ เมื่อคลิ๊กแผนก 
        public static string M_FORM_NUMBER = ""; // เก็บค่าลำดับ ฟอร์มเมื่อคลิ๊กเปิดฟอร์ม
        public static string M_FORM_NAME = ""; // เก็บชื่อฟอร์มเมื่อคลิ๊กเปิดฟอร์ม
        public static string M_FORM_CAPTION = ""; // เก็บ Caption ฟอร์มเมื่อคลิ๊กเปิดฟอร์ม

        //แบ่งสิทธิเป็น 3 แบบ
        //1. สิทธิเข้าใช้งานฝ่าย เช่น เข้าฝ่ายการเงิน,ฝ่ายผลิต Y = ใช้ได้  N = ไม่ได้
        //2. สิทธิเข้าใช้งานฟอร์ม เช่น เข้าฟอร์มขายสด,ฟอร์ขายเชื่อ Y = ใช้ได้  N = ไม่ได้
        //3. สิทธิเข้าใช้งานเมนูในฟอร์ม เช่น สร้างใหม่,เปิดแก้ไข,ยกเลิกเอกสาร,พิมพ์ Y = ใช้ได้  N = ไม่ได้
        public static string M_DEPART_LOGIN = "";  //1. สิทธิเข้าใช้งานฝ่าย เช่น เข้าฝ่ายการเงิน,ฝ่ายผลิต Y = ใช้ได้  N = ไม่ได้  

        public static string M_FORM_GRID = "";  //1 เข้าดูระเบียนได้
        public static string M_FORM_NEW = ""; //2 สร้างเอกสารใหม่ได้
        public static string M_FORM_OPEN = ""; //3 แก้ไขเอกสารได้
        public static string M_FORM_CANCEL = ""; // 4 ยกเลิกเอกสารได้
        public static string M_FORM_PRINT = "";  //5 ปริ๊นเอกสารได้

        public static string TRANS_BILL_STATUS = "";
        public static string TRANS_Month_STATUS = "";
        public static string RECEIVE_TYPE = "";

        //public static void ADATASOURCE(string str)
        //{

        //}
        public static string GetVersion()
        {
            string ourVersion = string.Empty;
            //if running the deployed application, you can get the version
            //  from the ApplicationDeployment information. If you try
            //  to access this when you are running in Visual Studio, it will not work.
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                ourVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            else
            {
                System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();
                if (assemblyInfo != null)
                    ourVersion = assemblyInfo.GetName().Version.ToString();
            }
            return ourVersion;
        }

        public static void DATE_NOW()
        {
            ////==================================================
            ////ประกาศ Cultureinfo ของแต่ละแบบที่ต้องการ
            //CultureInfo ThaiCulture = new CultureInfo("th-TH");
            //CultureInfo UsaCulture = new CultureInfo("en-US");
            ////ประกาศ DateTime เพื่อมาเป็นเวลาปัจจุบัน
            //DateTime DtNow = new DateTime();
            //DtNow = DateTime.Now;
            //string modifydate = "";
            //string modifytime = "";
            ////string username = W_ID_Select.M_USERNAME.ToString();
            ////===================================================
            //if (W_ID_Select.M_CO_YEAR_FORMAT == "T")
            //{
            //    modifydate = DtNow.ToString("yyyy-MM-dd", ThaiCulture);
            //    modifytime = DtNow.ToString("T", ThaiCulture);
            //    //==================================================
            //}
            //else
            //{
            //    modifydate = DtNow.ToString("yyyy-MM-dd", UsaCulture);
            //    modifytime = DtNow.ToString("E", UsaCulture);
            //    //==================================================
            //}
            //ModifydateNOW = modifydate.ToString();
            //ModifytimeNOW = modifytime.ToString();
            ////=======================================================
            //D_createID = M_USERNAME + ":" + ModifydateNOW + ":" + ModifytimeNOW;
            //D_storyID = M_USERNAME + ":" + ModifydateNOW + ":" + ModifytimeNOW;
            ////=======================================================

        }


    }
}
