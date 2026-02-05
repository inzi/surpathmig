namespace SurPath
{
    partial class FrmDepartmentDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDepartmentDetails));
            this.lblDepartmentName = new System.Windows.Forms.Label();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDepartmentName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblMandatoryField = new System.Windows.Forms.Label();
            this.lblDepartmentNameMan = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.AutoSize = true;
            this.lblDepartmentName.Location = new System.Drawing.Point(12, 53);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(93, 13);
            this.lblDepartmentName.TabIndex = 1;
            this.lblDepartmentName.Text = "&Department Name";
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(11, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(165, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Department Details";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(197, 89);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(109, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Location = new System.Drawing.Point(119, 50);
            this.txtDepartmentName.MaxLength = 200;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Size = new System.Drawing.Size(191, 20);
            this.txtDepartmentName.TabIndex = 3;
            this.txtDepartmentName.WaterMark = "Enter Department Name";
            this.txtDepartmentName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDepartmentName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepartmentName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDepartmentName.TextChanged += new System.EventHandler(this.txtDepartmentName_TextChanged);
            // 
            // lblMandatoryField
            // 
            this.lblMandatoryField.AutoSize = true;
            this.lblMandatoryField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMandatoryField.ForeColor = System.Drawing.Color.Red;
            this.lblMandatoryField.Location = new System.Drawing.Point(207, 14);
            this.lblMandatoryField.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMandatoryField.Name = "lblMandatoryField";
            this.lblMandatoryField.Size = new System.Drawing.Size(94, 13);
            this.lblMandatoryField.TabIndex = 7;
            this.lblMandatoryField.Text = "* Mandatory Fields";
            // 
            // lblDepartmentNameMan
            // 
            this.lblDepartmentNameMan.AutoSize = true;
            this.lblDepartmentNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblDepartmentNameMan.Location = new System.Drawing.Point(104, 51);
            this.lblDepartmentNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDepartmentNameMan.Name = "lblDepartmentNameMan";
            this.lblDepartmentNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblDepartmentNameMan.TabIndex = 2;
            this.lblDepartmentNameMan.Text = "*";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(313, 52);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 4;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // FrmDepartmentDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(380, 125);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblDepartmentNameMan);
            this.Controls.Add(this.lblMandatoryField);
            this.Controls.Add(this.txtDepartmentName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.lblDepartmentName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDepartmentDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DepartmentDetails";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDepartmentDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmDepartmentDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDepartmentName;
        private System.Windows.Forms.Label lblPageHeader;
        private Controls.TextBoxes.SurTextBox txtDepartmentName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblMandatoryField;
        private System.Windows.Forms.Label lblDepartmentNameMan;
        private System.Windows.Forms.CheckBox chkActive;
      

    }
}