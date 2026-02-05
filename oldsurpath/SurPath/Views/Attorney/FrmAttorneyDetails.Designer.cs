namespace SurPath
{
    partial class FrmAttorneyDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAttorneyDetails));
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.MaskedTextBox();
            this.txtFaxNumber = new System.Windows.Forms.MaskedTextBox();
            this.lblMandatoryfield = new System.Windows.Forms.Label();
            this.lblFirstNameMan = new System.Windows.Forms.Label();
            this.lblAddressMan = new System.Windows.Forms.Label();
            this.lblStateMan = new System.Windows.Forms.Label();
            this.lblZipCodeMan = new System.Windows.Forms.Label();
            this.lblCityMan = new System.Windows.Forms.Label();
            this.lblEmailMan = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.txtEmailAddress = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAttorneyLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAttorneyFirstname = new SurPath.Controls.TextBoxes.SurTextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnAvailability = new System.Windows.Forms.Button();
            this.txtZipcode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.SuspendLayout();
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(18, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(138, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Attorney Details";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(275, 274);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(187, 274);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 27;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(18, 239);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 23;
            this.lblEmail.Text = "&Email";
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(18, 206);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(24, 13);
            this.lblFax.TabIndex = 21;
            this.lblFax.Text = "&Fax";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(18, 172);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(38, 13);
            this.lblPhone.TabIndex = 19;
            this.lblPhone.Text = "&Phone";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(18, 74);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "&Address";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(18, 42);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "&Name";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(190, 136);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 13;
            this.lblState.Text = "&State";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(18, 136);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 10;
            this.lblCity.Text = "Cit&y";
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(346, 137);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 16;
            this.lblZipCode.Text = "&Zip Code";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Location = new System.Drawing.Point(75, 169);
            this.txtPhoneNumber.Mask = "(999) 000-0000";
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(240, 20);
            this.txtPhoneNumber.TabIndex = 20;
            this.txtPhoneNumber.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPhoneNumber_MouseClick);
            this.txtPhoneNumber.TextChanged += new System.EventHandler(this.txtPhoneNumber_TextChanged);
            // 
            // txtFaxNumber
            // 
            this.txtFaxNumber.Location = new System.Drawing.Point(75, 203);
            this.txtFaxNumber.Mask = "(999) 000-0000";
            this.txtFaxNumber.Name = "txtFaxNumber";
            this.txtFaxNumber.Size = new System.Drawing.Size(240, 20);
            this.txtFaxNumber.TabIndex = 22;
            this.txtFaxNumber.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtFaxNumber_MouseClick);
            this.txtFaxNumber.TextChanged += new System.EventHandler(this.txtFaxNumber_TextChanged);
            // 
            // lblMandatoryfield
            // 
            this.lblMandatoryfield.AutoSize = true;
            this.lblMandatoryfield.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMandatoryfield.ForeColor = System.Drawing.Color.Red;
            this.lblMandatoryfield.Location = new System.Drawing.Point(443, 14);
            this.lblMandatoryfield.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMandatoryfield.Name = "lblMandatoryfield";
            this.lblMandatoryfield.Size = new System.Drawing.Size(94, 13);
            this.lblMandatoryfield.TabIndex = 29;
            this.lblMandatoryfield.Text = "* Mandatory Fields";
            // 
            // lblFirstNameMan
            // 
            this.lblFirstNameMan.AutoSize = true;
            this.lblFirstNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblFirstNameMan.Location = new System.Drawing.Point(52, 40);
            this.lblFirstNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFirstNameMan.Name = "lblFirstNameMan";
            this.lblFirstNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblFirstNameMan.TabIndex = 2;
            this.lblFirstNameMan.Text = "*";
            // 
            // lblAddressMan
            // 
            this.lblAddressMan.AutoSize = true;
            this.lblAddressMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressMan.ForeColor = System.Drawing.Color.Red;
            this.lblAddressMan.Location = new System.Drawing.Point(61, 72);
            this.lblAddressMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddressMan.Name = "lblAddressMan";
            this.lblAddressMan.Size = new System.Drawing.Size(13, 17);
            this.lblAddressMan.TabIndex = 7;
            this.lblAddressMan.Text = "*";
            // 
            // lblStateMan
            // 
            this.lblStateMan.AutoSize = true;
            this.lblStateMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStateMan.ForeColor = System.Drawing.Color.Red;
            this.lblStateMan.Location = new System.Drawing.Point(221, 134);
            this.lblStateMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStateMan.Name = "lblStateMan";
            this.lblStateMan.Size = new System.Drawing.Size(13, 17);
            this.lblStateMan.TabIndex = 14;
            this.lblStateMan.Text = "*";
            // 
            // lblZipCodeMan
            // 
            this.lblZipCodeMan.AutoSize = true;
            this.lblZipCodeMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZipCodeMan.ForeColor = System.Drawing.Color.Red;
            this.lblZipCodeMan.Location = new System.Drawing.Point(393, 134);
            this.lblZipCodeMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZipCodeMan.Name = "lblZipCodeMan";
            this.lblZipCodeMan.Size = new System.Drawing.Size(13, 17);
            this.lblZipCodeMan.TabIndex = 17;
            this.lblZipCodeMan.Text = "*";
            // 
            // lblCityMan
            // 
            this.lblCityMan.AutoSize = true;
            this.lblCityMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCityMan.ForeColor = System.Drawing.Color.Red;
            this.lblCityMan.Location = new System.Drawing.Point(41, 134);
            this.lblCityMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCityMan.Name = "lblCityMan";
            this.lblCityMan.Size = new System.Drawing.Size(13, 17);
            this.lblCityMan.TabIndex = 11;
            this.lblCityMan.Text = "*";
            // 
            // lblEmailMan
            // 
            this.lblEmailMan.AutoSize = true;
            this.lblEmailMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailMan.ForeColor = System.Drawing.Color.Red;
            this.lblEmailMan.Location = new System.Drawing.Point(50, 237);
            this.lblEmailMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmailMan.Name = "lblEmailMan";
            this.lblEmailMan.Size = new System.Drawing.Size(13, 17);
            this.lblEmailMan.TabIndex = 24;
            this.lblEmailMan.Text = "*";
            // 
            // cmbState
            // 
            this.cmbState.DropDownHeight = 95;
            this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbState.DropDownWidth = 95;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.IntegralHeight = false;
            this.cmbState.Items.AddRange(new object[] {
            "(Select)",
            "AL",
            "AK",
            "AZ",
            "AR",
            "CA",
            "CO",
            "CT",
            "DE",
            "FL",
            "GA",
            "HI",
            "ID",
            "IL",
            "IN",
            "IA",
            "KS",
            "KY",
            "LA",
            "ME",
            "MD",
            "MA",
            "MI",
            "MN",
            "MS",
            "MO",
            "MT",
            "NE",
            "NV",
            "NH",
            "NJ",
            "NM",
            "NY",
            "NC",
            "ND",
            "OH",
            "OK",
            "OR",
            "PA",
            "RI",
            "SC",
            "SD",
            "TN",
            "TX",
            "UT",
            "VT",
            "VA",
            "WA",
            "WV",
            "WI",
            "WY"});
            this.cmbState.Location = new System.Drawing.Point(233, 133);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(108, 21);
            this.cmbState.TabIndex = 15;
            this.cmbState.TextChanged += new System.EventHandler(this.cmbState_TextChanged);
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(75, 235);
            this.txtEmailAddress.MaxLength = 320;
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(240, 20);
            this.txtEmailAddress.TabIndex = 25;
            this.txtEmailAddress.WaterMark = "Enter Email ";
            this.txtEmailAddress.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtEmailAddress.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailAddress.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtEmailAddress.TextChanged += new System.EventHandler(this.txtEmailAddress_TextChanged);
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(75, 133);
            this.txtCity.MaxLength = 200;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(108, 20);
            this.txtCity.TabIndex = 12;
            this.txtCity.WaterMark = "Enter City";
            this.txtCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtCity.TextChanged += new System.EventHandler(this.txtCity_TextChanged);
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(75, 102);
            this.txtAddress2.MaxLength = 200;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(240, 20);
            this.txtAddress2.TabIndex = 9;
            this.txtAddress2.WaterMark = "Enter Address 2";
            this.txtAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAddress2.TextChanged += new System.EventHandler(this.txtAddress2_TextChanged);
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(75, 70);
            this.txtAddress1.MaxLength = 200;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(240, 20);
            this.txtAddress1.TabIndex = 8;
            this.txtAddress1.WaterMark = "Enter Address 1";
            this.txtAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAddress1.TextChanged += new System.EventHandler(this.txtAddress1_TextChanged);
            // 
            // txtAttorneyLastName
            // 
            this.txtAttorneyLastName.Location = new System.Drawing.Point(277, 38);
            this.txtAttorneyLastName.MaxLength = 200;
            this.txtAttorneyLastName.Name = "txtAttorneyLastName";
            this.txtAttorneyLastName.Size = new System.Drawing.Size(191, 20);
            this.txtAttorneyLastName.TabIndex = 4;
            this.txtAttorneyLastName.WaterMark = "Enter Attorney Last Name";
            this.txtAttorneyLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAttorneyLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAttorneyLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAttorneyLastName.TextChanged += new System.EventHandler(this.txtAttorneyLastName_TextChanged);
            // 
            // txtAttorneyFirstname
            // 
            this.txtAttorneyFirstname.Location = new System.Drawing.Point(75, 38);
            this.txtAttorneyFirstname.MaxLength = 200;
            this.txtAttorneyFirstname.Name = "txtAttorneyFirstname";
            this.txtAttorneyFirstname.Size = new System.Drawing.Size(191, 20);
            this.txtAttorneyFirstname.TabIndex = 3;
            this.txtAttorneyFirstname.WaterMark = "Enter Attorney First Name";
            this.txtAttorneyFirstname.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAttorneyFirstname.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAttorneyFirstname.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAttorneyFirstname.TextChanged += new System.EventHandler(this.txtAttorneyFirstname_TextChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(470, 40);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 5;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // btnAvailability
            // 
            this.btnAvailability.Location = new System.Drawing.Point(324, 233);
            this.btnAvailability.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvailability.Name = "btnAvailability";
            this.btnAvailability.Size = new System.Drawing.Size(97, 23);
            this.btnAvailability.TabIndex = 26;
            this.btnAvailability.Text = "Availability";
            this.btnAvailability.UseVisualStyleBackColor = true;
            this.btnAvailability.Click += new System.EventHandler(this.btnAvailability_Click);
            // 
            // txtZipcode
            // 
            this.txtZipcode.Location = new System.Drawing.Point(408, 133);
            this.txtZipcode.MaxLength = 5;
            this.txtZipcode.Name = "txtZipcode";
            this.txtZipcode.Size = new System.Drawing.Size(93, 20);
            this.txtZipcode.TabIndex = 18;
            this.txtZipcode.WaterMark = "Enter  Zip Code";
            this.txtZipcode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtZipcode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipcode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtZipcode.TextChanged += new System.EventHandler(this.txtAttorneyLastName_TextChanged);
            this.txtZipcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZipcode_KeyPress);
            // 
            // FrmAttorneyDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(537, 314);
            this.Controls.Add(this.btnAvailability);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.cmbState);
            this.Controls.Add(this.lblEmailMan);
            this.Controls.Add(this.lblCityMan);
            this.Controls.Add(this.lblZipCodeMan);
            this.Controls.Add(this.lblStateMan);
            this.Controls.Add(this.lblAddressMan);
            this.Controls.Add(this.lblFirstNameMan);
            this.Controls.Add(this.lblMandatoryfield);
            this.Controls.Add(this.txtFaxNumber);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.txtEmailAddress);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.txtAddress1);
            this.Controls.Add(this.txtZipcode);
            this.Controls.Add(this.txtAttorneyLastName);
            this.Controls.Add(this.txtAttorneyFirstname);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.lblZipCode);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblFax);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAttorneyDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attorney Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAttorneyDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmAttorneyDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblName;
        private Controls.TextBoxes.SurTextBox txtAttorneyFirstname;
        private Controls.TextBoxes.SurTextBox txtAttorneyLastName;
        private Controls.TextBoxes.SurTextBox txtAddress1;
        private Controls.TextBoxes.SurTextBox txtAddress2;
        private Controls.TextBoxes.SurTextBox txtCity;
        private Controls.TextBoxes.SurTextBox txtEmailAddress;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.MaskedTextBox txtPhoneNumber;
        private System.Windows.Forms.MaskedTextBox txtFaxNumber;
        private System.Windows.Forms.Label lblMandatoryfield;
        private System.Windows.Forms.Label lblFirstNameMan;
        private System.Windows.Forms.Label lblAddressMan;
        private System.Windows.Forms.Label lblStateMan;
        private System.Windows.Forms.Label lblZipCodeMan;
        private System.Windows.Forms.Label lblCityMan;
        private System.Windows.Forms.Label lblEmailMan;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnAvailability;
        private Controls.TextBoxes.SurTextBox txtZipcode;


    }
}