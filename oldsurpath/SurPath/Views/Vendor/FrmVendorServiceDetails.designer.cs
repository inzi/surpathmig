namespace SurPath
{
    partial class FrmVendorServiceDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVendorServiceDetails));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblServiceCost = new System.Windows.Forms.Label();
            this.lblServiceName = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.lblVendorServiceMandatory = new System.Windows.Forms.Label();
            this.lblServiceNameMan = new System.Windows.Forms.Label();
            this.lblServiceCostMan = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlObserved = new System.Windows.Forms.Panel();
            this.rbUnObserved = new System.Windows.Forms.RadioButton();
            this.rbObserved = new System.Windows.Forms.RadioButton();
            this.lblFormType = new System.Windows.Forms.Label();
            this.pnlFormType = new System.Windows.Forms.Panel();
            this.rbNonFederal = new System.Windows.Forms.RadioButton();
            this.rbFederal = new System.Windows.Forms.RadioButton();
            this.lblObserved = new System.Windows.Forms.Label();
            this.lblCategoryMan = new System.Windows.Forms.Label();
            this.rbtnHair = new System.Windows.Forms.RadioButton();
            this.rbtnUA = new System.Windows.Forms.RadioButton();
            this.lblTestCategory = new System.Windows.Forms.Label();
            this.rbtnDNA = new System.Windows.Forms.RadioButton();
            this.txtServiceCost = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtServiceName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.pnlObserved.SuspendLayout();
            this.pnlFormType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(198, 238);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblServiceCost
            // 
            this.lblServiceCost.AutoSize = true;
            this.lblServiceCost.Location = new System.Drawing.Point(17, 82);
            this.lblServiceCost.Name = "lblServiceCost";
            this.lblServiceCost.Size = new System.Drawing.Size(67, 13);
            this.lblServiceCost.TabIndex = 5;
            this.lblServiceCost.Text = "&Service Cost";
            // 
            // lblServiceName
            // 
            this.lblServiceName.AutoSize = true;
            this.lblServiceName.Location = new System.Drawing.Point(17, 44);
            this.lblServiceName.Name = "lblServiceName";
            this.lblServiceName.Size = new System.Drawing.Size(74, 13);
            this.lblServiceName.TabIndex = 1;
            this.lblServiceName.Text = "Service &Name";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(109, 238);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(17, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(192, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Vendor Service Details";
            // 
            // lblVendorServiceMandatory
            // 
            this.lblVendorServiceMandatory.AutoSize = true;
            this.lblVendorServiceMandatory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendorServiceMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblVendorServiceMandatory.Location = new System.Drawing.Point(279, 14);
            this.lblVendorServiceMandatory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVendorServiceMandatory.Name = "lblVendorServiceMandatory";
            this.lblVendorServiceMandatory.Size = new System.Drawing.Size(94, 13);
            this.lblVendorServiceMandatory.TabIndex = 21;
            this.lblVendorServiceMandatory.Text = "* Mandatory Fields";
            // 
            // lblServiceNameMan
            // 
            this.lblServiceNameMan.AutoSize = true;
            this.lblServiceNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblServiceNameMan.Location = new System.Drawing.Point(91, 42);
            this.lblServiceNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblServiceNameMan.Name = "lblServiceNameMan";
            this.lblServiceNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblServiceNameMan.TabIndex = 2;
            this.lblServiceNameMan.Text = "*";
            // 
            // lblServiceCostMan
            // 
            this.lblServiceCostMan.AutoSize = true;
            this.lblServiceCostMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceCostMan.ForeColor = System.Drawing.Color.Red;
            this.lblServiceCostMan.Location = new System.Drawing.Point(83, 80);
            this.lblServiceCostMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblServiceCostMan.Name = "lblServiceCostMan";
            this.lblServiceCostMan.Size = new System.Drawing.Size(13, 17);
            this.lblServiceCostMan.TabIndex = 6;
            this.lblServiceCostMan.Text = "*";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(312, 42);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 4;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            this.chkActive.TextChanged += new System.EventHandler(this.chkActive_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(73, 191);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(94, 149);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "*";
            // 
            // pnlObserved
            // 
            this.pnlObserved.Controls.Add(this.rbUnObserved);
            this.pnlObserved.Controls.Add(this.rbObserved);
            this.pnlObserved.Location = new System.Drawing.Point(109, 138);
            this.pnlObserved.Name = "pnlObserved";
            this.pnlObserved.Size = new System.Drawing.Size(191, 38);
            this.pnlObserved.TabIndex = 15;
            // 
            // rbUnObserved
            // 
            this.rbUnObserved.AutoSize = true;
            this.rbUnObserved.Location = new System.Drawing.Point(97, 10);
            this.rbUnObserved.Name = "rbUnObserved";
            this.rbUnObserved.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbUnObserved.Size = new System.Drawing.Size(83, 17);
            this.rbUnObserved.TabIndex = 1;
            this.rbUnObserved.Text = "Unobserved";
            this.rbUnObserved.UseVisualStyleBackColor = true;
            // 
            // rbObserved
            // 
            this.rbObserved.AutoSize = true;
            this.rbObserved.Location = new System.Drawing.Point(8, 10);
            this.rbObserved.Name = "rbObserved";
            this.rbObserved.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbObserved.Size = new System.Drawing.Size(71, 17);
            this.rbObserved.TabIndex = 0;
            this.rbObserved.Text = "Observed";
            this.rbObserved.UseVisualStyleBackColor = true;
            // 
            // lblFormType
            // 
            this.lblFormType.AutoSize = true;
            this.lblFormType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormType.Location = new System.Drawing.Point(17, 193);
            this.lblFormType.Name = "lblFormType";
            this.lblFormType.Size = new System.Drawing.Size(57, 13);
            this.lblFormType.TabIndex = 16;
            this.lblFormType.Text = "Form Type";
            // 
            // pnlFormType
            // 
            this.pnlFormType.Controls.Add(this.rbNonFederal);
            this.pnlFormType.Controls.Add(this.rbFederal);
            this.pnlFormType.Location = new System.Drawing.Point(109, 184);
            this.pnlFormType.Name = "pnlFormType";
            this.pnlFormType.Size = new System.Drawing.Size(191, 38);
            this.pnlFormType.TabIndex = 18;
            // 
            // rbNonFederal
            // 
            this.rbNonFederal.AutoSize = true;
            this.rbNonFederal.Location = new System.Drawing.Point(97, 9);
            this.rbNonFederal.Name = "rbNonFederal";
            this.rbNonFederal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbNonFederal.Size = new System.Drawing.Size(83, 17);
            this.rbNonFederal.TabIndex = 1;
            this.rbNonFederal.Text = "Non Federal";
            this.rbNonFederal.UseVisualStyleBackColor = true;
            // 
            // rbFederal
            // 
            this.rbFederal.AutoSize = true;
            this.rbFederal.Location = new System.Drawing.Point(8, 9);
            this.rbFederal.Name = "rbFederal";
            this.rbFederal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbFederal.Size = new System.Drawing.Size(60, 17);
            this.rbFederal.TabIndex = 0;
            this.rbFederal.Text = "Federal";
            this.rbFederal.UseVisualStyleBackColor = true;
            // 
            // lblObserved
            // 
            this.lblObserved.AutoSize = true;
            this.lblObserved.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObserved.Location = new System.Drawing.Point(17, 151);
            this.lblObserved.Name = "lblObserved";
            this.lblObserved.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblObserved.Size = new System.Drawing.Size(80, 13);
            this.lblObserved.TabIndex = 13;
            this.lblObserved.Text = "Observed Type";
            // 
            // lblCategoryMan
            // 
            this.lblCategoryMan.AutoSize = true;
            this.lblCategoryMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryMan.ForeColor = System.Drawing.Color.Red;
            this.lblCategoryMan.Location = new System.Drawing.Point(89, 113);
            this.lblCategoryMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCategoryMan.Name = "lblCategoryMan";
            this.lblCategoryMan.Size = new System.Drawing.Size(13, 17);
            this.lblCategoryMan.TabIndex = 9;
            this.lblCategoryMan.Text = "*";
            // 
            // rbtnHair
            // 
            this.rbtnHair.AutoSize = true;
            this.rbtnHair.Location = new System.Drawing.Point(175, 111);
            this.rbtnHair.Name = "rbtnHair";
            this.rbtnHair.Size = new System.Drawing.Size(44, 17);
            this.rbtnHair.TabIndex = 11;
            this.rbtnHair.Text = "Hair";
            this.rbtnHair.UseVisualStyleBackColor = true;
            // 
            // rbtnUA
            // 
            this.rbtnUA.AutoSize = true;
            this.rbtnUA.Location = new System.Drawing.Point(117, 111);
            this.rbtnUA.Name = "rbtnUA";
            this.rbtnUA.Size = new System.Drawing.Size(40, 17);
            this.rbtnUA.TabIndex = 10;
            this.rbtnUA.Text = "UA";
            this.rbtnUA.UseVisualStyleBackColor = true;
            // 
            // lblTestCategory
            // 
            this.lblTestCategory.AutoSize = true;
            this.lblTestCategory.Location = new System.Drawing.Point(17, 115);
            this.lblTestCategory.Name = "lblTestCategory";
            this.lblTestCategory.Size = new System.Drawing.Size(73, 13);
            this.lblTestCategory.TabIndex = 8;
            this.lblTestCategory.Text = "Test Categor&y";
            // 
            // rbtnDNA
            // 
            this.rbtnDNA.AutoSize = true;
            this.rbtnDNA.Location = new System.Drawing.Point(235, 111);
            this.rbtnDNA.Name = "rbtnDNA";
            this.rbtnDNA.Size = new System.Drawing.Size(48, 17);
            this.rbtnDNA.TabIndex = 12;
            this.rbtnDNA.Text = "DNA";
            this.rbtnDNA.UseVisualStyleBackColor = true;
            // 
            // txtServiceCost
            // 
            this.txtServiceCost.Location = new System.Drawing.Point(117, 78);
            this.txtServiceCost.MaxLength = 6;
            this.txtServiceCost.Name = "txtServiceCost";
            this.txtServiceCost.Size = new System.Drawing.Size(191, 20);
            this.txtServiceCost.TabIndex = 7;
            this.txtServiceCost.WaterMark = "Enter Service Cost";
            this.txtServiceCost.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtServiceCost.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceCost.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtServiceCost.TextChanged += new System.EventHandler(this.txtServiceCost_TextChanged);
            this.txtServiceCost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtServiceCost_KeyPress);
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(117, 40);
            this.txtServiceName.MaxLength = 200;
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(191, 20);
            this.txtServiceName.TabIndex = 3;
            this.txtServiceName.WaterMark = "Enter Service Name";
            this.txtServiceName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtServiceName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtServiceName.TextChanged += new System.EventHandler(this.txtServiceName_TextChanged);
            // 
            // FrmVendorServiceDetails
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(377, 268);
            this.Controls.Add(this.lblCategoryMan);
            this.Controls.Add(this.rbtnDNA);
            this.Controls.Add(this.rbtnHair);
            this.Controls.Add(this.rbtnUA);
            this.Controls.Add(this.lblTestCategory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlObserved);
            this.Controls.Add(this.lblFormType);
            this.Controls.Add(this.pnlFormType);
            this.Controls.Add(this.lblObserved);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblServiceCostMan);
            this.Controls.Add(this.lblServiceNameMan);
            this.Controls.Add(this.lblVendorServiceMandatory);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.txtServiceCost);
            this.Controls.Add(this.txtServiceName);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblServiceCost);
            this.Controls.Add(this.lblServiceName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVendorServiceDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vendor Service ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmVendorServiceDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmVendorServiceDetails_Load);
            this.pnlObserved.ResumeLayout(false);
            this.pnlObserved.PerformLayout();
            this.pnlFormType.ResumeLayout(false);
            this.pnlFormType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblServiceCost;
        private System.Windows.Forms.Label lblServiceName;
        private System.Windows.Forms.Button btnOk;
        private Controls.TextBoxes.SurTextBox txtServiceName;
        private Controls.TextBoxes.SurTextBox txtServiceCost;
        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Label lblVendorServiceMandatory;
        private System.Windows.Forms.Label lblServiceNameMan;
        private System.Windows.Forms.Label lblServiceCostMan;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlObserved;
        private System.Windows.Forms.RadioButton rbUnObserved;
        private System.Windows.Forms.RadioButton rbObserved;
        private System.Windows.Forms.Label lblFormType;
        private System.Windows.Forms.Panel pnlFormType;
        private System.Windows.Forms.RadioButton rbNonFederal;
        private System.Windows.Forms.RadioButton rbFederal;
        private System.Windows.Forms.Label lblObserved;
        private System.Windows.Forms.Label lblCategoryMan;
        private System.Windows.Forms.RadioButton rbtnHair;
        private System.Windows.Forms.RadioButton rbtnUA;
        private System.Windows.Forms.Label lblTestCategory;
        private System.Windows.Forms.RadioButton rbtnDNA;

    }
}