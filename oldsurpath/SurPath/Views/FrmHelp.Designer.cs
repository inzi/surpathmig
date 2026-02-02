namespace SurPath
{
    partial class FrmHelp
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
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblImageheader = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblRelease = new System.Windows.Forms.Label();
            this.lblsurpath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(6, 12);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(127, 20);
            this.lblPageHeader.TabIndex = 1;
            this.lblPageHeader.Text = "About SurPath";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SurPath.Properties.Resources.Logo_Small3;
            this.pictureBox1.Location = new System.Drawing.Point(21, 42);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(41, 56);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblImageheader
            // 
            this.lblImageheader.AutoSize = true;
            this.lblImageheader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImageheader.Location = new System.Drawing.Point(64, 52);
            this.lblImageheader.Name = "lblImageheader";
            this.lblImageheader.Size = new System.Drawing.Size(133, 37);
            this.lblImageheader.TabIndex = 3;
            this.lblImageheader.Text = "SurPath";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(198, 204);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 65;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(28, 124);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(51, 13);
            this.lblVersion.TabIndex = 66;
            this.lblVersion.Text = "Version  :";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(28, 153);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(67, 13);
            this.lblProduct.TabIndex = 67;
            this.lblProduct.Text = "Product ID  :";
            // 
            // lblRelease
            // 
            this.lblRelease.AutoSize = true;
            this.lblRelease.Location = new System.Drawing.Point(28, 181);
            this.lblRelease.Name = "lblRelease";
            this.lblRelease.Size = new System.Drawing.Size(81, 13);
            this.lblRelease.TabIndex = 68;
            this.lblRelease.Text = "Release Date  :";
            // 
            // lblsurpath
            // 
            this.lblsurpath.AutoSize = true;
            this.lblsurpath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblsurpath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsurpath.Location = new System.Drawing.Point(28, 232);
            this.lblsurpath.Name = "lblsurpath";
            this.lblsurpath.Size = new System.Drawing.Size(189, 13);
            this.lblsurpath.TabIndex = 69;
            this.lblsurpath.Text = "All Rights Reserved © 2015, SurScan.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 70;
            this.label1.Text = "1.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "00371-177-0000061-85461";
            this.label2.Visible = false;
            // 
            // FrmHelp
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblsurpath);
            this.Controls.Add(this.lblRelease);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblImageheader);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblPageHeader);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHelp";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About SurPath";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmHelp_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblImageheader;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblRelease;
        private System.Windows.Forms.Label lblsurpath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}