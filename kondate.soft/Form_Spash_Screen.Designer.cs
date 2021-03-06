namespace kondate.soft
{
    partial class Form_Spash_Screen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Spash_Screen));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Pic_picture = new System.Windows.Forms.PictureBox();
            this.txtpicture_size = new System.Windows.Forms.TextBox();
            this.txtpicture = new System.Windows.Forms.TextBox();
            this.txtco_id = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_picture)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 249);
            this.progressBar1.MarqueeAnimationSpeed = 50;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(553, 25);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 9;
            // 
            // Pic_picture
            // 
            this.Pic_picture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Pic_picture.Image = ((System.Drawing.Image)(resources.GetObject("Pic_picture.Image")));
            this.Pic_picture.Location = new System.Drawing.Point(0, -2);
            this.Pic_picture.Name = "Pic_picture";
            this.Pic_picture.Size = new System.Drawing.Size(550, 250);
            this.Pic_picture.TabIndex = 10;
            this.Pic_picture.TabStop = false;
            // 
            // txtpicture_size
            // 
            this.txtpicture_size.BackColor = System.Drawing.Color.White;
            this.txtpicture_size.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpicture_size.Location = new System.Drawing.Point(396, 199);
            this.txtpicture_size.Name = "txtpicture_size";
            this.txtpicture_size.ReadOnly = true;
            this.txtpicture_size.Size = new System.Drawing.Size(67, 23);
            this.txtpicture_size.TabIndex = 395;
            this.txtpicture_size.Visible = false;
            // 
            // txtpicture
            // 
            this.txtpicture.BackColor = System.Drawing.Color.White;
            this.txtpicture.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpicture.Location = new System.Drawing.Point(469, 199);
            this.txtpicture.Name = "txtpicture";
            this.txtpicture.ReadOnly = true;
            this.txtpicture.Size = new System.Drawing.Size(67, 23);
            this.txtpicture.TabIndex = 396;
            this.txtpicture.Visible = false;
            // 
            // txtco_id
            // 
            this.txtco_id.Location = new System.Drawing.Point(323, 199);
            this.txtco_id.MaxLength = 9;
            this.txtco_id.Name = "txtco_id";
            this.txtco_id.Size = new System.Drawing.Size(67, 21);
            this.txtco_id.TabIndex = 397;
            this.txtco_id.Visible = false;
            // 
            // Form_Spash_Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(553, 274);
            this.Controls.Add(this.txtco_id);
            this.Controls.Add(this.txtpicture);
            this.Controls.Add(this.txtpicture_size);
            this.Controls.Add(this.Pic_picture);
            this.Controls.Add(this.progressBar1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Spash_Screen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form_Spash_Screen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pic_picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox Pic_picture;
        private System.Windows.Forms.TextBox txtpicture_size;
        private System.Windows.Forms.TextBox txtpicture;
        private System.Windows.Forms.TextBox txtco_id;
    }
}