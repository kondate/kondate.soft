using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;

using System.Data.SqlClient;

using System.Data.Common;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
// At design time:
//   Set the ImageList's ImageSize properties to the correct values:
//      imlSmallIcons.ImageSize = 32,32
//      imlLargeIcons.ImageSize = 64,64
//   Set the ImageList's ColorDepth properties to the correct values:
//      imlSmallIcons.ColorDepth = Depth32bit
//      imlLargeIcons.ColorDepth = Depth32bit

using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace kondate.soft
{
    public partial class Form_Spash_Screen : Form
    {
        //ประกาศ Cultureinfo ของแต่ละแบบที่ต้องการ
        CultureInfo ThaiCulture = new CultureInfo("th-TH");
        CultureInfo UsaCulture = new CultureInfo("en-US");
        //ประกาศ DateTime เพื่อมาเป็นเวลาปัจจุบัน
        //k003db_master_type_id
        //เชื่อมต่อฐานข้อมูล=======================================================
        //SqlConnection conn = new SqlConnection(KRest.W_ID_Select.conn_string);

        public Form_Spash_Screen()
        {
            InitializeComponent();
        }

        private void Form_Spash_Screen_Load(object sender, EventArgs e)
        {

        }
        private void Load_detail()
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

                string strStored = "k009db_business_sp";
                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Connection = conn;

                cmd1.CommandText = strStored;

                cmd1.Parameters.Add(new SqlParameter("@PRTYPE", SqlDbType.NVarChar)).Value = "SELECT_MAIN";
                cmd1.Parameters.Add(new SqlParameter("@cdkey", SqlDbType.NVarChar)).Value = W_ID_Select.CDKEY.Trim();
                //cmd1.Parameters.Add(new SqlParameter("@lang_id", SqlDbType.VarChar)).Value = W_ID_Select.Lang.Trim();
                cmd1.Parameters.Add(new SqlParameter("@txtco_id", SqlDbType.NVarChar)).Value = W_ID_Select.M_COID.Trim();
                //cmd1.Parameters.Add(new SqlParameter("@txtbranch_id", SqlDbType.NVarChar)).Value = W_ID_Select.M_BRANCHID.Trim();

                try
                {
                    //แบบที่ 3 ใช้ SqlDataAdapter =========================================================
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        this.txtco_id.Text = dt.Rows[0]["txtco_id"].ToString();

                        this.txtpicture_size.Text = dt.Rows[0]["mat_picture_size"].ToString();


                        if (this.txtpicture_size.Text == "")
                        {

                        }
                        else
                        {
                            //ภาพพนักงาน==================================================
                            byte[] imgg = (byte[])(dt.Rows[0]["mat_picture"]);
                            if (imgg == null)
                            {
                                this.Pic_picture.Image = null;
                            }
                            else
                            {
                                MemoryStream mstream = new MemoryStream(imgg);
                                this.Pic_picture.Image = Image.FromStream(mstream);
                                this.Pic_picture.SizeMode = PictureBoxSizeMode.Zoom;
                                this.Pic_picture.BorderStyle = BorderStyle.FixedSingle;
                            }
                            //=======================================================
                        }
                    }
                    else
                    {
                        //  MessageBox.Show("Not found   ", "ผลการทำงาน", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        conn.Close();
                        //  return;
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

            }
            //จบเชื่อมต่อฐานข้อมูล=======================================================

        }

        // Scale the image to fit in the ImageList and add it.
        private void AddImageToImageList(ImageList iml, Bitmap bm,
            string key, float wid, float hgt)
        {
            // Make the bitmap.
            Bitmap iml_bm = new Bitmap(
                iml.ImageSize.Width,
                iml.ImageSize.Height);
            using (Graphics gr = Graphics.FromImage(iml_bm))
            {
                gr.Clear(Color.Transparent);
                gr.InterpolationMode = InterpolationMode.High;

                // See where we need to draw the image to scale it properly.
                RectangleF source_rect = new RectangleF(
                    0, 0, bm.Width, bm.Height);
                RectangleF dest_rect = new RectangleF(
                    0, 0, iml_bm.Width, iml_bm.Height);
                dest_rect = ScaleRect(source_rect, dest_rect);

                // Draw the image.
                gr.DrawImage(bm, dest_rect, source_rect,
                    GraphicsUnit.Pixel);
            }

            // Add the image to the ImageList.
            iml.Images.Add(key, iml_bm);
        }

        // Convert a byte array into an image.
        private Bitmap BytesToImage(byte[] bytes)
        {
            using (MemoryStream image_stream =
                new MemoryStream(bytes))
            {
                Bitmap bm = new Bitmap(image_stream);
                return bm;
            }
        }

        // Scale an image without disorting it.
        // Return a centered rectangle in the destination area.
        private RectangleF ScaleRect(
            RectangleF source_rect, RectangleF dest_rect)
        {
            float source_aspect =
                source_rect.Width / source_rect.Height;
            float wid = dest_rect.Width;
            float hgt = dest_rect.Height;
            float dest_aspect = wid / hgt;

            if (source_aspect > dest_aspect)
            {
                // The source is relatively short and wide.
                // Use all of the available width.
                hgt = wid / source_aspect;
            }
            else
            {
                // The source is relatively tall and thin.
                // Use all of the available height.
                wid = hgt * source_aspect;
            }

            // Center it.
            float x = dest_rect.Left + (dest_rect.Width - wid) / 2;
            float y = dest_rect.Top + (dest_rect.Height - hgt) / 2;
            return new RectangleF(x, y, wid, hgt);
        }

    }
}
