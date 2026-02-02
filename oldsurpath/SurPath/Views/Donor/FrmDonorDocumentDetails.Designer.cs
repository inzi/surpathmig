namespace SurPath
{
    partial class FrmDonorDocumentDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDonorDocumentDetails));
            this.lblTestPanelCode = new System.Windows.Forms.Label();
            this.lblDocumentTypeMan = new System.Windows.Forms.Label();
            this.cmbDocumentType = new System.Windows.Forms.ComboBox();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.txtDonorFiles = new SurPath.Controls.TextBoxes.SurTextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTestPanelCode
            // 
            this.lblTestPanelCode.AutoSize = true;
            this.lblTestPanelCode.Location = new System.Drawing.Point(19, 58);
            this.lblTestPanelCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTestPanelCode.Name = "lblTestPanelCode";
            this.lblTestPanelCode.Size = new System.Drawing.Size(108, 17);
            this.lblTestPanelCode.TabIndex = 1;
            this.lblTestPanelCode.Text = "Document &Type";
            // 
            // lblDocumentTypeMan
            // 
            this.lblDocumentTypeMan.AutoSize = true;
            this.lblDocumentTypeMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentTypeMan.ForeColor = System.Drawing.Color.Red;
            this.lblDocumentTypeMan.Location = new System.Drawing.Point(129, 55);
            this.lblDocumentTypeMan.Name = "lblDocumentTypeMan";
            this.lblDocumentTypeMan.Size = new System.Drawing.Size(15, 20);
            this.lblDocumentTypeMan.TabIndex = 2;
            this.lblDocumentTypeMan.Text = "*";
            // 
            // cmbDocumentType
            // 
            this.cmbDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocumentType.FormattingEnabled = true;
            this.cmbDocumentType.Location = new System.Drawing.Point(189, 53);
            this.cmbDocumentType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbDocumentType.Name = "cmbDocumentType";
            this.cmbDocumentType.Size = new System.Drawing.Size(269, 24);
            this.cmbDocumentType.TabIndex = 3;
            this.cmbDocumentType.TextChanged += new System.EventHandler(this.cmbDocumentType_TextChanged);
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(19, 11);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(184, 25);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Donor Documents";
            // 
            // txtDonorFiles
            // 
            this.txtDonorFiles.BackColor = System.Drawing.Color.White;
            this.txtDonorFiles.Location = new System.Drawing.Point(189, 98);
            this.txtDonorFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDonorFiles.Name = "txtDonorFiles";
            this.txtDonorFiles.ReadOnly = true;
            this.txtDonorFiles.Size = new System.Drawing.Size(269, 22);
            this.txtDonorFiles.TabIndex = 6;
            this.txtDonorFiles.WaterMark = "Select a File";
            this.txtDonorFiles.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDonorFiles.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDonorFiles.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDonorFiles.TextChanged += new System.EventHandler(this.txtDonorFiles_TextChanged);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(467, 97);
            this.btnFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(87, 28);
            this.btnFile.TabIndex = 7;
            this.btnFile.Text = "Browse";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(301, 160);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 28);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(204, 160);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(87, 28);
            this.btnUpload.TabIndex = 8;
            this.btnUpload.Text = "&Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 103);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Document";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(95, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "*";
            // 
            // FrmDonorDocumentDetails
            // 
            this.AcceptButton = this.btnUpload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(592, 222);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.txtDonorFiles);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.cmbDocumentType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDocumentTypeMan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTestPanelCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDonorDocumentDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Donor Documents";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDonorDocumentDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmDonorDocumentDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTestPanelCode;
        private System.Windows.Forms.Label lblDocumentTypeMan;
        private System.Windows.Forms.ComboBox cmbDocumentType;
        private System.Windows.Forms.Label lblPageHeader;
        private Controls.TextBoxes.SurTextBox txtDonorFiles;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}