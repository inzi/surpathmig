namespace SurPath
{
    partial class FrmJudgeDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmJudgeDetails));
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cmbPrefix = new System.Windows.Forms.ComboBox();
            this.cmbSuffix = new System.Windows.Forms.ComboBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblJudgeFirstNameMan = new System.Windows.Forms.Label();
            this.lblAddressMan = new System.Windows.Forms.Label();
            this.lblZipMan = new System.Windows.Forms.Label();
            this.lblStateMan = new System.Windows.Forms.Label();
            this.lblCityMan = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblUserMan = new System.Windows.Forms.Label();
            this.btnAvailablity = new System.Windows.Forms.Button();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPasswordMan = new System.Windows.Forms.Label();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtUsername = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.txtZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.SuspendLayout();
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(10, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(119, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Judge Details";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(358, 270);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 32;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(271, 270);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(11, 78);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 7;
            this.lblAddress.Text = "&Address";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(11, 43);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "&Name";
            // 
            // cmbPrefix
            // 
            this.cmbPrefix.DropDownHeight = 95;
            this.cmbPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrefix.DropDownWidth = 95;
            this.cmbPrefix.FormattingEnabled = true;
            this.cmbPrefix.IntegralHeight = false;
            this.cmbPrefix.Items.AddRange(new object[] {
            "(Select)",
            "The Honourable"});
            this.cmbPrefix.Location = new System.Drawing.Point(82, 39);
            this.cmbPrefix.MaxLength = 100;
            this.cmbPrefix.Name = "cmbPrefix";
            this.cmbPrefix.Size = new System.Drawing.Size(141, 21);
            this.cmbPrefix.TabIndex = 3;
            this.cmbPrefix.SelectedIndexChanged += new System.EventHandler(this.cmbPrefix_SelectedIndexChanged);
            // 
            // cmbSuffix
            // 
            this.cmbSuffix.DropDownHeight = 75;
            this.cmbSuffix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSuffix.DropDownWidth = 75;
            this.cmbSuffix.FormattingEnabled = true;
            this.cmbSuffix.IntegralHeight = false;
            this.cmbSuffix.Items.AddRange(new object[] {
            "(Select)",
            "Sr.",
            "Jr.",
            "II",
            "III",
            "IV",
            "V"});
            this.cmbSuffix.Location = new System.Drawing.Point(606, 39);
            this.cmbSuffix.MaxLength = 100;
            this.cmbSuffix.Name = "cmbSuffix";
            this.cmbSuffix.Size = new System.Drawing.Size(88, 21);
            this.cmbSuffix.TabIndex = 6;
            this.cmbSuffix.SelectedIndexChanged += new System.EventHandler(this.cmbSuffix_SelectedIndexChanged);
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(11, 148);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 11;
            this.lblCity.Text = "Cit&y";
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(373, 148);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 18;
            this.lblZipCode.Text = "&Zip Code";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(207, 148);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 14;
            this.lblState.Text = "&State";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(590, 14);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "* Mandatory Fields";
            // 
            // lblJudgeFirstNameMan
            // 
            this.lblJudgeFirstNameMan.AutoSize = true;
            this.lblJudgeFirstNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJudgeFirstNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblJudgeFirstNameMan.Location = new System.Drawing.Point(44, 41);
            this.lblJudgeFirstNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblJudgeFirstNameMan.Name = "lblJudgeFirstNameMan";
            this.lblJudgeFirstNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblJudgeFirstNameMan.TabIndex = 2;
            this.lblJudgeFirstNameMan.Text = "*";
            // 
            // lblAddressMan
            // 
            this.lblAddressMan.AutoSize = true;
            this.lblAddressMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressMan.ForeColor = System.Drawing.Color.Red;
            this.lblAddressMan.Location = new System.Drawing.Point(54, 78);
            this.lblAddressMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddressMan.Name = "lblAddressMan";
            this.lblAddressMan.Size = new System.Drawing.Size(13, 17);
            this.lblAddressMan.TabIndex = 8;
            this.lblAddressMan.Text = "*";
            // 
            // lblZipMan
            // 
            this.lblZipMan.AutoSize = true;
            this.lblZipMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZipMan.ForeColor = System.Drawing.Color.Red;
            this.lblZipMan.Location = new System.Drawing.Point(419, 148);
            this.lblZipMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblZipMan.Name = "lblZipMan";
            this.lblZipMan.Size = new System.Drawing.Size(13, 16);
            this.lblZipMan.TabIndex = 19;
            this.lblZipMan.Text = "*";
            // 
            // lblStateMan
            // 
            this.lblStateMan.AutoSize = true;
            this.lblStateMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStateMan.ForeColor = System.Drawing.Color.Red;
            this.lblStateMan.Location = new System.Drawing.Point(237, 148);
            this.lblStateMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStateMan.Name = "lblStateMan";
            this.lblStateMan.Size = new System.Drawing.Size(13, 17);
            this.lblStateMan.TabIndex = 15;
            this.lblStateMan.Text = "*";
            // 
            // lblCityMan
            // 
            this.lblCityMan.AutoSize = true;
            this.lblCityMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCityMan.ForeColor = System.Drawing.Color.Red;
            this.lblCityMan.Location = new System.Drawing.Point(32, 148);
            this.lblCityMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCityMan.Name = "lblCityMan";
            this.lblCityMan.Size = new System.Drawing.Size(13, 17);
            this.lblCityMan.TabIndex = 12;
            this.lblCityMan.Text = "*";
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
            this.cmbState.Location = new System.Drawing.Point(253, 144);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(108, 21);
            this.cmbState.TabIndex = 16;
            this.cmbState.TextChanged += new System.EventHandler(this.cmbState_TextChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(606, 74);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 30;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // lblUserMan
            // 
            this.lblUserMan.AutoSize = true;
            this.lblUserMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMan.ForeColor = System.Drawing.Color.Red;
            this.lblUserMan.Location = new System.Drawing.Point(65, 183);
            this.lblUserMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserMan.Name = "lblUserMan";
            this.lblUserMan.Size = new System.Drawing.Size(13, 16);
            this.lblUserMan.TabIndex = 22;
            this.lblUserMan.Text = "*";
            // 
            // btnAvailablity
            // 
            this.btnAvailablity.Location = new System.Drawing.Point(259, 183);
            this.btnAvailablity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvailablity.Name = "btnAvailablity";
            this.btnAvailablity.Size = new System.Drawing.Size(97, 23);
            this.btnAvailablity.TabIndex = 24;
            this.btnAvailablity.Text = "Availablity";
            this.btnAvailablity.UseVisualStyleBackColor = true;
            this.btnAvailablity.Click += new System.EventHandler(this.btnAvailablity_Click);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(11, 185);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(55, 13);
            this.lblUserName.TabIndex = 21;
            this.lblUserName.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(11, 222);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 25;
            this.lblPassword.Text = "Password";
            // 
            // lblPasswordMan
            // 
            this.lblPasswordMan.AutoSize = true;
            this.lblPasswordMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordMan.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordMan.Location = new System.Drawing.Point(65, 220);
            this.lblPasswordMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPasswordMan.Name = "lblPasswordMan";
            this.lblPasswordMan.Size = new System.Drawing.Size(13, 16);
            this.lblPasswordMan.TabIndex = 26;
            this.lblPasswordMan.Text = "*";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(148, 245);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chkShowPassword.TabIndex = 29;
            this.chkShowPassword.Text = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(82, 219);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPassword.MaxLength = 320;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(168, 20);
            this.txtPassword.TabIndex = 27;
            this.txtPassword.WaterMark = "Enter Password";
            this.txtPassword.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(79, 184);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtUsername.MaxLength = 320;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(171, 20);
            this.txtUsername.TabIndex = 23;
            this.txtUsername.WaterMark = "Enter Username/Email";
            this.txtUsername.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUsername.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(82, 148);
            this.txtCity.MaxLength = 200;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(108, 20);
            this.txtCity.TabIndex = 13;
            this.txtCity.WaterMark = "Enter City";
            this.txtCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtCity.TextChanged += new System.EventHandler(this.txtCity_TextChanged);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(423, 39);
            this.txtLastName.MaxLength = 200;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(170, 20);
            this.txtLastName.TabIndex = 5;
            this.txtLastName.WaterMark = "Enter Last Name";
            this.txtLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(82, 112);
            this.txtAddress2.MaxLength = 200;
            this.txtAddress2.Multiline = true;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(240, 20);
            this.txtAddress2.TabIndex = 10;
            this.txtAddress2.WaterMark = "Enter  Address 2";
            this.txtAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAddress2.TextChanged += new System.EventHandler(this.txtAddress2_TextChanged);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(236, 39);
            this.txtFirstName.MaxLength = 200;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(170, 20);
            this.txtFirstName.TabIndex = 4;
            this.txtFirstName.WaterMark = "Enter  First Name";
            this.txtFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(82, 76);
            this.txtAddress1.MaxLength = 200;
            this.txtAddress1.Multiline = true;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(240, 20);
            this.txtAddress1.TabIndex = 9;
            this.txtAddress1.WaterMark = "Enter  Address 1";
            this.txtAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAddress1.TextChanged += new System.EventHandler(this.txtAddress1_TextChanged);
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.Location = new System.Drawing.Point(259, 220);
            this.btnResetPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(97, 23);
            this.btnResetPassword.TabIndex = 28;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(431, 145);
            this.txtZipCode.MaxLength = 5;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(84, 20);
            this.txtZipCode.TabIndex = 20;
            this.txtZipCode.WaterMark = "Enter  Zip Code";
            this.txtZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtZipCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZipCode_KeyPress);
            // 
            // FrmJudgeDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(705, 300);
            this.Controls.Add(this.txtZipCode);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.lblPasswordMan);
            this.Controls.Add(this.lblUserMan);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.btnAvailablity);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.cmbState);
            this.Controls.Add(this.lblCityMan);
            this.Controls.Add(this.lblStateMan);
            this.Controls.Add(this.lblZipMan);
            this.Controls.Add(this.lblAddressMan);
            this.Controls.Add(this.lblJudgeFirstNameMan);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.lblZipCode);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.cmbSuffix);
            this.Controls.Add(this.cmbPrefix);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtAddress1);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmJudgeDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Judge Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmJudgeDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmJudgeDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPageHeader;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblName;
        private Controls.TextBoxes.SurTextBox txtAddress1;
        private Controls.TextBoxes.SurTextBox txtFirstName;
        private Controls.TextBoxes.SurTextBox txtAddress2;
        private Controls.TextBoxes.SurTextBox txtLastName;
        private System.Windows.Forms.ComboBox cmbPrefix;
        private System.Windows.Forms.ComboBox cmbSuffix;
        private Controls.TextBoxes.SurTextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblJudgeFirstNameMan;
        private System.Windows.Forms.Label lblAddressMan;
        private System.Windows.Forms.Label lblZipMan;
        private System.Windows.Forms.Label lblStateMan;
        private System.Windows.Forms.Label lblCityMan;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblUserMan;
        private Controls.TextBoxes.SurTextBox txtUsername;
        private System.Windows.Forms.Button btnAvailablity;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private Controls.TextBoxes.SurTextBox txtPassword;
        private System.Windows.Forms.Label lblPasswordMan;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnResetPassword;
        private Controls.TextBoxes.SurTextBox txtZipCode;


    }
}