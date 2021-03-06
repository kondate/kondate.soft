namespace kondate.soft
{
    partial class Form_001Test_2panel_top
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_001Test_2panel_top));
            this.panel_top = new System.Windows.Forms.Panel();
            this.btnminimize = new System.Windows.Forms.PictureBox();
            this.btnclose = new System.Windows.Forms.PictureBox();
            this.panel_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnminimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnclose)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_top
            // 
            this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panel_top.Controls.Add(this.btnminimize);
            this.panel_top.Controls.Add(this.btnclose);
            this.panel_top.Location = new System.Drawing.Point(14, 211);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1013, 28);
            this.panel_top.TabIndex = 5;
            // 
            // btnminimize
            // 
            this.btnminimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnminimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnminimize.Image = ((System.Drawing.Image)(resources.GetObject("btnminimize.Image")));
            this.btnminimize.Location = new System.Drawing.Point(940, 3);
            this.btnminimize.Name = "btnminimize";
            this.btnminimize.Size = new System.Drawing.Size(29, 22);
            this.btnminimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnminimize.TabIndex = 17;
            this.btnminimize.TabStop = false;
            // 
            // btnclose
            // 
            this.btnclose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnclose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclose.Image = ((System.Drawing.Image)(resources.GetObject("btnclose.Image")));
            this.btnclose.Location = new System.Drawing.Point(975, 1);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(30, 22);
            this.btnclose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnclose.TabIndex = 15;
            this.btnclose.TabStop = false;
            // 
            // Form_001Test_2panel_top
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 617);
            this.Controls.Add(this.panel_top);
            this.Name = "Form_001Test_2panel_top";
            this.Text = "Form_001Test_panel_1top";
            this.panel_top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnminimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnclose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.PictureBox btnminimize;
        private System.Windows.Forms.PictureBox btnclose;
    }
}