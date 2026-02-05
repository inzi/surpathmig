namespace SurPath
{
    partial class FrmTestingAuthorityDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTestingAuthorityDetails));
            this.lblDepartmentNameMan = new System.Windows.Forms.Label();
            this.lblMandatoryField = new System.Windows.Forms.Label();
            this.txtTestingAuthorityName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.lblDepartmentName = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblDepartmentNameMan
            // 
            this.lblDepartmentNameMan.AutoSize = true;
            this.lblDepartmentNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblDepartmentNameMan.Location = new System.Drawing.Point(125, 53);
            this.lblDepartmentNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDepartmentNameMan.Name = "lblDepartmentNameMan";
            this.lblDepartmentNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblDepartmentNameMan.TabIndex = 2;
            this.lblDepartmentNameMan.Text = "*";
            // 
            // lblMandatoryField
            // 
            this.lblMandatoryField.AutoSize = true;
            this.lblMandatoryField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMandatoryField.ForeColor = System.Drawing.Color.Red;
            this.lblMandatoryField.Location = new System.Drawing.Point(238, 16);
            this.lblMandatoryField.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMandatoryField.Name = "lblMandatoryField";
            this.lblMandatoryField.Size = new System.Drawing.Size(94, 13);
            this.lblMandatoryField.TabIndex = 7;
            this.lblMandatoryField.Text = "* Mandatory Fields";
            // 
            // txtTestingAuthorityName
            // 
            this.txtTestingAuthorityName.Location = new System.Drawing.Point(143, 52);
            this.txtTestingAuthorityName.MaxLength = 200;
            this.txtTestingAuthorityName.Name = "txtTestingAuthorityName";
            this.txtTestingAuthorityName.Size = new System.Drawing.Size(191, 20);
            this.txtTestingAuthorityName.TabIndex = 3;
            this.txtTestingAuthorityName.WaterMark = "Enter Testing Authority Name";
            this.txtTestingAuthorityName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtTestingAuthorityName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestingAuthorityName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtTestingAuthorityName.TextChanged += new System.EventHandler(this.txtTestingAuthorityName_TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(207, 91);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(119, 91);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(10, 11);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(206, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Testing Authority Details";
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.AutoSize = true;
            this.lblDepartmentName.Location = new System.Drawing.Point(11, 55);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(117, 13);
            this.lblDepartmentName.TabIndex = 1;
            this.lblDepartmentName.Text = "&Testing Authority Name";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(336, 54);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 4;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // FrmTestingAuthorityDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(400, 125);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblDepartmentNameMan);
            this.Controls.Add(this.lblMandatoryField);
            this.Controls.Add(this.txtTestingAuthorityName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.lblDepartmentName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTestingAuthorityDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestingAuthorityDetails";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTestingAuthorityDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmTestingAuthorityDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDepartmentNameMan;
        private System.Windows.Forms.Label lblMandatoryField;
        private Controls.TextBoxes.SurTextBox txtTestingAuthorityName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Label lblDepartmentName;
        private System.Windows.Forms.CheckBox chkActive;

    }
}