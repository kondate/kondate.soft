using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;


namespace kondate.soft.HOME04_Warehouse
{
    public partial class Form1_Generate_Barcode : Form
    {
        public Form1_Generate_Barcode()
        {
            InitializeComponent();
        }

        private void Form1_Generate_Barcode_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.txtmat_id;
        }

        private void btnGenerate_Barcode_Click(object sender, EventArgs e)
        {
            GEN_BARCODE();
        }
        private void GEN_BARCODE()
        {
            if (this.txtmat_id.Text .ToString() == "")
            {
                return;
            }

            string barcode = this.txtmat_id.Text;

            Bitmap bitmap = new Bitmap(barcode.Length * 40, 150);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Font oFont = new System.Drawing.Font("Code 128", 20);   //IDAHC39M Code 39 Barcode
                PointF point = new PointF(2f, 2f);

                SolidBrush black = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);

                graphics.FillRectangle(white, 0, 0, bitmap.Width, bitmap.Height);
                graphics.DrawString("*" + barcode + "*", oFont, black, point);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                pictureBox1_Gen_Barcode.Image = bitmap;
                pictureBox1_Gen_Barcode.Height = bitmap.Height;
                pictureBox1_Gen_Barcode.Width = bitmap.Width;
            }

        }
        private void BtnClose_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtmat_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && this.txtmat_id.Text == "")
            {
                //
            }
            //========================================
            else if (e.KeyChar == (char)Keys.Enter && this.txtmat_id.Text.Trim() != "")
            {
                GEN_BARCODE();
            }
            else if ((e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.') && txtmat_id.Text.Length == 0)
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

        //=================================
    }
}
