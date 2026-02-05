namespace SurPath
{
    partial class FrmCourtDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCourtDetails));
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.lblCourtName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblCourtNameMan = new System.Windows.Forms.Label();
            this.lblMandatoryField = new System.Windows.Forms.Label();
            this.lblZipMan = new System.Windows.Forms.Label();
            this.lblAddressMan = new System.Windows.Forms.Label();
            this.lblCityMan = new System.Windows.Forms.Label();
            this.lblStateMan = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblUserMan = new System.Windows.Forms.Label();
            this.btnAvailablity = new System.Windows.Forms.Button();
            this.lblEmail = new System.Windows.Forms.Label();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.lblPasswordMan = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.txtPassword = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtUsername = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtCourtName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.SuspendLayout();
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(9, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(114, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Court Details";
            // 
            // lblCourtName
            // 
            this.lblCourtName.AutoSize = true;
            this.lblCourtName.Location = new System.Drawing.Point(9, 44);
            this.lblCourtName.Name = "lblCourtName";
            this.lblCourtName.Size = new System.Drawing.Size(63, 13);
            this.lblCourtName.TabIndex = 1;
            this.lblCourtName.Text = "Cour&t Name";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(248, 285);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(159, 285);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 28;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(9, 83);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "&Address";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(9, 150);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 10;
            this.lblCity.Text = "Cit&y";
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(357, 157);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 16;
            this.lblZipCode.Text = "&Zip Code";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(201, 155);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 13;
            this.lblState.Text = "&State";
            // 
            // lblCourtNameMan
            // 
            this.lblCourtNameMan.AutoSize = true;
            this.lblCourtNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCourtNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblCourtNameMan.Location = new System.Drawing.Point(69, 42);
            this.lblCourtNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCourtNameMan.Name = "lblCourtNameMan";
            this.lblCourtNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblCourtNameMan.TabIndex = 2;
            this.lblCourtNameMan.Text = "*";
            // 
            // lblMandatoryField
            // 
            this.lblMandatoryField.AutoSize = true;
            this.lblMandatoryField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMandatoryField.ForeColor = System.Drawing.Color.Red;
            this.lblMandatoryField.Location = new System.Drawing.Point(380, 9);
            this.lblMandatoryField.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMandatoryField.Name = "lblMandatoryField";
            this.lblMandatoryField.Size = new System.Drawing.Size(94, 13);
            this.lblMandatoryField.TabIndex = 0;
            this.lblMandatoryField.Text = "* Mandatory Fields";
            // 
            // lblZipMan
            // 
            this.lblZipMan.AutoSize = true;
            this.lblZipMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZipMan.ForeColor = System.Drawing.Color.Red;
            this.lblZipMan.Location = new System.Drawing.Point(404, 155);
            this.lblZipMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZipMan.Name = "lblZipMan";
            this.lblZipMan.Size = new System.Drawing.Size(13, 17);
            this.lblZipMan.TabIndex = 17;
            this.lblZipMan.Text = "*";
            // 
            // lblAddressMan
            // 
            this.lblAddressMan.AutoSize = true;
            this.lblAddressMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressMan.ForeColor = System.Drawing.Color.Red;
            this.lblAddressMan.Location = new System.Drawing.Point(54, 81);
            this.lblAddressMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddressMan.Name = "lblAddressMan";
            this.lblAddressMan.Size = new System.Drawing.Size(13, 17);
            this.lblAddressMan.TabIndex = 7;
            this.lblAddressMan.Text = "*";
            // 
            // lblCityMan
            // 
            this.lblCityMan.AutoSize = true;
            this.lblCityMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCityMan.ForeColor = System.Drawing.Color.Red;
            this.lblCityMan.Location = new System.Drawing.Point(33, 148);
            this.lblCityMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCityMan.Name = "lblCityMan";
            this.lblCityMan.Size = new System.Drawing.Size(13, 17);
            this.lblCityMan.TabIndex = 11;
            this.lblCityMan.Text = "*";
            // 
            // lblStateMan
            // 
            this.lblStateMan.AutoSize = true;
            this.lblStateMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStateMan.ForeColor = System.Drawing.Color.Red;
            this.lblStateMan.Location = new System.Drawing.Point(230, 153);
            this.lblStateMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStateMan.Name = "lblStateMan";
            this.lblStateMan.Size = new System.Drawing.Size(13, 17);
            this.lblStateMan.TabIndex = 14;
            this.lblStateMan.Text = "*";
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
            this.cmbState.Location = new System.Drawing.Point(244, 152);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(108, 21);
            this.cmbState.TabIndex = 15;
            this.cmbState.TextChanged += new System.EventHandler(this.cmbState_TextChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(333, 43);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 4;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // lblUserMan
            // 
            this.lblUserMan.AutoSize = true;
            this.lblUserMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMan.ForeColor = System.Drawing.Color.Red;
            this.lblUserMan.Location = new System.Drawing.Point(65, 193);
            this.lblUserMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserMan.Name = "lblUserMan";
            this.lblUserMan.Size = new System.Drawing.Size(13, 16);
            this.lblUserMan.TabIndex = 20;
            this.lblUserMan.Text = "*";
            // 
            // btnAvailablity
            // 
            this.btnAvailablity.Location = new System.Drawing.Point(271, 190);
            this.btnAvailablity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvailablity.Name = "btnAvailablity";
            this.btnAvailablity.Size = new System.Drawing.Size(97, 23);
            this.btnAvailablity.TabIndex = 22;
            this.btnAvailablity.Text = "Availability";
            this.btnAvailablity.UseVisualStyleBackColor = true;
            this.btnAvailablity.Click += new System.EventHandler(this.btnAvailablity_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(9, 195);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(55, 13);
            this.lblEmail.TabIndex = 19;
            this.lblEmail.Text = "Username";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(168, 253);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chkShowPassword.TabIndex = 27;
            this.chkShowPassword.Text = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // lblPasswordMan
            // 
            this.lblPasswordMan.AutoSize = true;
            this.lblPasswordMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordMan.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordMan.Location = new System.Drawing.Point(60, 225);
            this.lblPasswordMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPasswordMan.Name = "lblPasswordMan";
            this.lblPasswordMan.Size = new System.Drawing.Size(13, 16);
            this.lblPasswordMan.TabIndex = 24;
            this.lblPasswordMan.Text = "*";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(9, 227);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 23;
            this.lblPassword.Text = "Password";
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.Location = new System.Drawing.Point(271, 224);
            this.btnResetPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(97, 23);
            this.btnResetPassword.TabIndex = 26;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(83, 225);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPassword.MaxLength = 320;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(182, 20);
            this.txtPassword.TabIndex = 25;
            this.txtPassword.WaterMark = "Enter Password";
            this.txtPassword.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(83, 191);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtUsername.MaxLength = 320;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(182, 20);
            this.txtUsername.TabIndex = 21;
            this.txtUsername.WaterMark = "Enter Username/ Email";
            this.txtUsername.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUsername.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(83, 150);
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
            this.txtAddress2.Location = new System.Drawing.Point(83, 115);
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
            // txtCourtName
            // 
            this.txtCourtName.Location = new System.Drawing.Point(83, 41);
            this.txtCourtName.MaxLength = 200;
            this.txtCourtName.Name = "txtCourtName";
            this.txtCourtName.Size = new System.Drawing.Size(240, 20);
            this.txtCourtName.TabIndex = 3;
            this.txtCourtName.WaterMark = "Enter Court Name";
            this.txtCourtName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtCourtName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCourtName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtCourtName.TextChanged += new System.EventHandler(this.txtCourtName_TextChanged);
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(83, 79);
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
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(413, 153);
            this.txtZipCode.MaxLength = 5;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(84, 20);
            this.txtZipCode.TabIndex = 18;
            this.txtZipCode.WaterMark = "Enter  Zip Code";
            this.txtZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtZipCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZipCode_KeyPress);
            // 
            // FrmCourtDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(506, 319);
            this.Controls.Add(this.txtZipCode);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.lblPasswordMan);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserMan);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnAvailablity);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.cmbState);
            this.Controls.Add(this.lblMandatoryField);
            this.Controls.Add(this.lblZipMan);
            this.Controls.Add(this.lblStateMan);
            this.Controls.Add(this.lblCityMan);
            this.Controls.Add(this.lblAddressMan);
            this.Controls.Add(this.lblCourtNameMan);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.lblZipCode);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.txtCourtName);
            this.Controls.Add(this.txtAddress1);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.lblCourtName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblAddress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCourtDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Court Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCourtDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmCourtDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Label lblCourtName;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblAddress;
        private Controls.TextBoxes.SurTextBox txtAddress1;
        private Controls.TextBoxes.SurTextBox txtCourtName;
        private Controls.TextBoxes.SurTextBox txtAddress2;
        private Controls.TextBoxes.SurTextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblCourtNameMan;
        private System.Windows.Forms.Label lblMandatoryField;
        private System.Windows.Forms.Label lblZipMan;
        private System.Windows.Forms.Label lblAddressMan;
        private System.Windows.Forms.Label lblCityMan;
        private System.Windows.Forms.Label lblStateMan;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblUserMan;
        private Controls.TextBoxes.SurTextBox txtUsername;
        private System.Windows.Forms.Button btnAvailablity;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Label lblPasswordMan;
        private Controls.TextBoxes.SurTextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnResetPassword;
        private Controls.TextBoxes.SurTextBox txtZipCode;


    }
}