namespace SurPath
{
    partial class FrmDonorRegistraion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDonorRegistraion));
            this.gboxDonorDetails = new System.Windows.Forms.GroupBox();
            this.chkWalkin = new System.Windows.Forms.CheckBox();
            this.ssndup = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.cmbDonorYear = new System.Windows.Forms.ComboBox();
            this.cmbDonorDate = new System.Windows.Forms.ComboBox();
            this.cmbDonorMonth = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDepartmentNameMan = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.txtPhone2 = new System.Windows.Forms.MaskedTextBox();
            this.txtPhone1 = new System.Windows.Forms.MaskedTextBox();
            this.txtSSN = new System.Windows.Forms.MaskedTextBox();
            this.txtMiddleInitial = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblMiddleInitial = new System.Windows.Forms.Label();
            this.txtLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtEmail = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.lblPhone2 = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblPhone1 = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblDOB = new System.Windows.Forms.Label();
            this.lblSSN = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.cmbSuffix = new System.Windows.Forms.ComboBox();
            this.rbtnMale = new System.Windows.Forms.RadioButton();
            this.rbtnFemale = new System.Windows.Forms.RadioButton();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.gboxClientDetails = new System.Windows.Forms.GroupBox();
            this.cmbClient = new System.Windows.Forms.ComboBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.lblClient = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblMandatoryField = new System.Windows.Forms.Label();
            this.gboxDonorSearch = new System.Windows.Forms.GroupBox();
            this.txtSearchZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.cmbSearchYear = new System.Windows.Forms.ComboBox();
            this.cmbSearchDate = new System.Windows.Forms.ComboBox();
            this.cmbSearchMonth = new System.Windows.Forms.ComboBox();
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.DonorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorSSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorDOB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSearchSSN = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtSearchEmail = new SurPath.Controls.TextBoxes.SurTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtSearchCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSearchLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtSearchFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gboxDonorDetails.SuspendLayout();
            this.gboxClientDetails.SuspendLayout();
            this.gboxDonorSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // gboxDonorDetails
            // 
            this.gboxDonorDetails.Controls.Add(this.chkWalkin);
            this.gboxDonorDetails.Controls.Add(this.ssndup);
            this.gboxDonorDetails.Controls.Add(this.txtZipCode);
            this.gboxDonorDetails.Controls.Add(this.cmbDonorYear);
            this.gboxDonorDetails.Controls.Add(this.cmbDonorDate);
            this.gboxDonorDetails.Controls.Add(this.cmbDonorMonth);
            this.gboxDonorDetails.Controls.Add(this.label12);
            this.gboxDonorDetails.Controls.Add(this.label10);
            this.gboxDonorDetails.Controls.Add(this.label9);
            this.gboxDonorDetails.Controls.Add(this.label7);
            this.gboxDonorDetails.Controls.Add(this.label4);
            this.gboxDonorDetails.Controls.Add(this.label1);
            this.gboxDonorDetails.Controls.Add(this.lblDepartmentNameMan);
            this.gboxDonorDetails.Controls.Add(this.cmbState);
            this.gboxDonorDetails.Controls.Add(this.txtPhone2);
            this.gboxDonorDetails.Controls.Add(this.txtPhone1);
            this.gboxDonorDetails.Controls.Add(this.txtSSN);
            this.gboxDonorDetails.Controls.Add(this.txtMiddleInitial);
            this.gboxDonorDetails.Controls.Add(this.lblMiddleInitial);
            this.gboxDonorDetails.Controls.Add(this.txtLastName);
            this.gboxDonorDetails.Controls.Add(this.lblGender);
            this.gboxDonorDetails.Controls.Add(this.lblSuffix);
            this.gboxDonorDetails.Controls.Add(this.lblLastName);
            this.gboxDonorDetails.Controls.Add(this.txtEmail);
            this.gboxDonorDetails.Controls.Add(this.txtCity);
            this.gboxDonorDetails.Controls.Add(this.txtAddress2);
            this.gboxDonorDetails.Controls.Add(this.txtAddress1);
            this.gboxDonorDetails.Controls.Add(this.txtFirstName);
            this.gboxDonorDetails.Controls.Add(this.lblZipCode);
            this.gboxDonorDetails.Controls.Add(this.lblPhone2);
            this.gboxDonorDetails.Controls.Add(this.lblState);
            this.gboxDonorDetails.Controls.Add(this.lblPhone1);
            this.gboxDonorDetails.Controls.Add(this.lblCity);
            this.gboxDonorDetails.Controls.Add(this.lblAddress2);
            this.gboxDonorDetails.Controls.Add(this.lblAddress1);
            this.gboxDonorDetails.Controls.Add(this.lblEmail);
            this.gboxDonorDetails.Controls.Add(this.lblDOB);
            this.gboxDonorDetails.Controls.Add(this.lblSSN);
            this.gboxDonorDetails.Controls.Add(this.lblFirstName);
            this.gboxDonorDetails.Controls.Add(this.cmbSuffix);
            this.gboxDonorDetails.Controls.Add(this.rbtnMale);
            this.gboxDonorDetails.Controls.Add(this.rbtnFemale);
            this.gboxDonorDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxDonorDetails.Location = new System.Drawing.Point(12, 280);
            this.gboxDonorDetails.Name = "gboxDonorDetails";
            this.gboxDonorDetails.Size = new System.Drawing.Size(806, 211);
            this.gboxDonorDetails.TabIndex = 3;
            this.gboxDonorDetails.TabStop = false;
            this.gboxDonorDetails.Text = "Donor Details";
            // 
            // chkWalkin
            // 
            this.chkWalkin.AutoSize = true;
            this.chkWalkin.Location = new System.Drawing.Point(686, 100);
            this.chkWalkin.Name = "chkWalkin";
            this.chkWalkin.Size = new System.Drawing.Size(62, 17);
            this.chkWalkin.TabIndex = 30;
            this.chkWalkin.Text = "Walk-in";
            this.chkWalkin.UseVisualStyleBackColor = true;
            // 
            // ssndup
            // 
            this.ssndup.Location = new System.Drawing.Point(339, 60);
            this.ssndup.MaxLength = 5;
            this.ssndup.Name = "ssndup";
            this.ssndup.Size = new System.Drawing.Size(68, 20);
            this.ssndup.TabIndex = 38;
            this.ssndup.WaterMark = "";
            this.ssndup.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.ssndup.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ssndup.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.ssndup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZipCode_KeyPress);
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(502, 135);
            this.txtZipCode.MaxLength = 5;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(84, 20);
            this.txtZipCode.TabIndex = 38;
            this.txtZipCode.WaterMark = "Enter  Zip Code";
            this.txtZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtZipCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZipCode_KeyPress);
            // 
            // cmbDonorYear
            // 
            this.cmbDonorYear.DropDownHeight = 90;
            this.cmbDonorYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDonorYear.FormattingEnabled = true;
            this.cmbDonorYear.IntegralHeight = false;
            this.cmbDonorYear.Items.AddRange(new object[] {
            "YYYY"});
            this.cmbDonorYear.Location = new System.Drawing.Point(574, 58);
            this.cmbDonorYear.Name = "cmbDonorYear";
            this.cmbDonorYear.Size = new System.Drawing.Size(52, 21);
            this.cmbDonorYear.TabIndex = 20;
            this.cmbDonorYear.TextChanged += new System.EventHandler(this.cmbDonorYear_TextChanged);
            // 
            // cmbDonorDate
            // 
            this.cmbDonorDate.DropDownHeight = 90;
            this.cmbDonorDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDonorDate.FormattingEnabled = true;
            this.cmbDonorDate.IntegralHeight = false;
            this.cmbDonorDate.ItemHeight = 13;
            this.cmbDonorDate.Items.AddRange(new object[] {
            "DD",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cmbDonorDate.Location = new System.Drawing.Point(532, 58);
            this.cmbDonorDate.Name = "cmbDonorDate";
            this.cmbDonorDate.Size = new System.Drawing.Size(40, 21);
            this.cmbDonorDate.TabIndex = 19;
            this.cmbDonorDate.TextChanged += new System.EventHandler(this.cmbDonorDate_TextChanged);
            // 
            // cmbDonorMonth
            // 
            this.cmbDonorMonth.DropDownHeight = 90;
            this.cmbDonorMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDonorMonth.FormattingEnabled = true;
            this.cmbDonorMonth.IntegralHeight = false;
            this.cmbDonorMonth.ItemHeight = 13;
            this.cmbDonorMonth.Items.AddRange(new object[] {
            "MM",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbDonorMonth.Location = new System.Drawing.Point(488, 58);
            this.cmbDonorMonth.Name = "cmbDonorMonth";
            this.cmbDonorMonth.Size = new System.Drawing.Size(42, 21);
            this.cmbDonorMonth.TabIndex = 18;
            this.cmbDonorMonth.TextChanged += new System.EventHandler(this.cmbDonorMonth_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(672, 60);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 17);
            this.label12.TabIndex = 22;
            this.label12.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(451, 60);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 17);
            this.label10.TabIndex = 17;
            this.label10.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(471, 22);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 17);
            this.label9.TabIndex = 6;
            this.label9.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(299, 60);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(57, 174);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 17);
            this.label4.TabIndex = 41;
            this.label4.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(45, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "*";
            // 
            // lblDepartmentNameMan
            // 
            this.lblDepartmentNameMan.AutoSize = true;
            this.lblDepartmentNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblDepartmentNameMan.Location = new System.Drawing.Point(69, 22);
            this.lblDepartmentNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDepartmentNameMan.Name = "lblDepartmentNameMan";
            this.lblDepartmentNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblDepartmentNameMan.TabIndex = 1;
            this.lblDepartmentNameMan.Text = "*";
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
            this.cmbState.Location = new System.Drawing.Point(339, 135);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(86, 21);
            this.cmbState.TabIndex = 36;
            this.cmbState.TextChanged += new System.EventHandler(this.cmbState_TextChanged);
            // 
            // txtPhone2
            // 
            this.txtPhone2.Location = new System.Drawing.Point(339, 172);
            this.txtPhone2.Mask = "(999) 000-0000";
            this.txtPhone2.Name = "txtPhone2";
            this.txtPhone2.Size = new System.Drawing.Size(86, 20);
            this.txtPhone2.TabIndex = 44;
            this.txtPhone2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPhone2_MouseClick);
            this.txtPhone2.TextChanged += new System.EventHandler(this.txtPhone2_TextChanged);
            // 
            // txtPhone1
            // 
            this.txtPhone1.Location = new System.Drawing.Point(83, 172);
            this.txtPhone1.Mask = "(999) 000-0000";
            this.txtPhone1.Name = "txtPhone1";
            this.txtPhone1.Size = new System.Drawing.Size(91, 20);
            this.txtPhone1.TabIndex = 42;
            this.txtPhone1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPhone1_MouseClick);
            this.txtPhone1.TextChanged += new System.EventHandler(this.txtPhone1_TextChanged);
            // 
            // txtSSN
            // 
            this.txtSSN.Location = new System.Drawing.Point(339, 61);
            this.txtSSN.Mask = "000-00-0000";
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(68, 20);
            this.txtSSN.TabIndex = 15;
            this.txtSSN.Tag = "\'\\0\'";
            this.txtSSN.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSSN_MouseClick);
            this.txtSSN.TextChanged += new System.EventHandler(this.txtSSN_TextChanged);
            this.txtSSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSSN_KeyPress);
            // 
            // txtMiddleInitial
            // 
            this.txtMiddleInitial.Location = new System.Drawing.Point(339, 20);
            this.txtMiddleInitial.Name = "txtMiddleInitial";
            this.txtMiddleInitial.Size = new System.Drawing.Size(68, 20);
            this.txtMiddleInitial.TabIndex = 4;
            this.txtMiddleInitial.WaterMark = "Enter MI";
            this.txtMiddleInitial.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMiddleInitial.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMiddleInitial.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtMiddleInitial.TextChanged += new System.EventHandler(this.txtMiddleInitial_TextChanged);
            // 
            // lblMiddleInitial
            // 
            this.lblMiddleInitial.AutoSize = true;
            this.lblMiddleInitial.Location = new System.Drawing.Point(262, 24);
            this.lblMiddleInitial.Name = "lblMiddleInitial";
            this.lblMiddleInitial.Size = new System.Drawing.Size(65, 13);
            this.lblMiddleInitial.TabIndex = 3;
            this.lblMiddleInitial.Text = "Middle Initial";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(488, 20);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(173, 20);
            this.txtLastName.TabIndex = 7;
            this.txtLastName.WaterMark = "Enter Last Name";
            this.txtLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(631, 62);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(42, 13);
            this.lblGender.TabIndex = 21;
            this.lblGender.Text = "Gender";
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(676, 24);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(33, 13);
            this.lblSuffix.TabIndex = 8;
            this.lblSuffix.Text = "Suffix";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(416, 24);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 5;
            this.lblLastName.Text = "Last Name";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(83, 58);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(173, 20);
            this.txtEmail.TabIndex = 12;
            this.txtEmail.WaterMark = "Enter Email";
            this.txtEmail.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtEmail.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(83, 135);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(173, 20);
            this.txtCity.TabIndex = 33;
            this.txtCity.WaterMark = "Enter City";
            this.txtCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtCity.TextChanged += new System.EventHandler(this.txtCity_TextChanged);
            // 
            // txtAddress2
            // 
            this.txtAddress2.Location = new System.Drawing.Point(339, 98);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(173, 20);
            this.txtAddress2.TabIndex = 29;
            this.txtAddress2.WaterMark = "Enter Address 2";
            this.txtAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAddress2.TextChanged += new System.EventHandler(this.txtAddress2_TextChanged);
            // 
            // txtAddress1
            // 
            this.txtAddress1.Location = new System.Drawing.Point(83, 97);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(173, 20);
            this.txtAddress1.TabIndex = 26;
            this.txtAddress1.WaterMark = "Enter Address 1";
            this.txtAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtAddress1.TextChanged += new System.EventHandler(this.txtAddress1_TextChanged);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(83, 20);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(173, 20);
            this.txtFirstName.TabIndex = 2;
            this.txtFirstName.WaterMark = "Enter First Name";
            this.txtFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(435, 139);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 37;
            this.lblZipCode.Text = "Zip Code";
            // 
            // lblPhone2
            // 
            this.lblPhone2.AutoSize = true;
            this.lblPhone2.Location = new System.Drawing.Point(262, 176);
            this.lblPhone2.Name = "lblPhone2";
            this.lblPhone2.Size = new System.Drawing.Size(47, 13);
            this.lblPhone2.TabIndex = 43;
            this.lblPhone2.Text = "Phone 2";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(262, 139);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 34;
            this.lblState.Text = "State";
            // 
            // lblPhone1
            // 
            this.lblPhone1.AutoSize = true;
            this.lblPhone1.Location = new System.Drawing.Point(13, 176);
            this.lblPhone1.Name = "lblPhone1";
            this.lblPhone1.Size = new System.Drawing.Size(47, 13);
            this.lblPhone1.TabIndex = 40;
            this.lblPhone1.Text = "Phone 1";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(13, 139);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 31;
            this.lblCity.Text = "City";
            // 
            // lblAddress2
            // 
            this.lblAddress2.AutoSize = true;
            this.lblAddress2.Location = new System.Drawing.Point(262, 100);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(54, 13);
            this.lblAddress2.TabIndex = 28;
            this.lblAddress2.Text = "Address 2";
            // 
            // lblAddress1
            // 
            this.lblAddress1.AutoSize = true;
            this.lblAddress1.Location = new System.Drawing.Point(13, 101);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(54, 13);
            this.lblAddress1.TabIndex = 25;
            this.lblAddress1.Text = "Address 1";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(13, 62);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Email";
            // 
            // lblDOB
            // 
            this.lblDOB.AutoSize = true;
            this.lblDOB.Location = new System.Drawing.Point(416, 62);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Size = new System.Drawing.Size(36, 13);
            this.lblDOB.TabIndex = 16;
            this.lblDOB.Text = "D.O.B";
            // 
            // lblSSN
            // 
            this.lblSSN.AutoSize = true;
            this.lblSSN.Location = new System.Drawing.Point(262, 62);
            this.lblSSN.Name = "lblSSN";
            this.lblSSN.Size = new System.Drawing.Size(39, 13);
            this.lblSSN.TabIndex = 13;
            this.lblSSN.Text = "SSN #";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(13, 24);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name";
            // 
            // cmbSuffix
            // 
            this.cmbSuffix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSuffix.FormattingEnabled = true;
            this.cmbSuffix.Items.AddRange(new object[] {
            "(Select)",
            "Sr.",
            "Jr.",
            "II",
            "III",
            "IV",
            "V"});
            this.cmbSuffix.Location = new System.Drawing.Point(716, 21);
            this.cmbSuffix.Name = "cmbSuffix";
            this.cmbSuffix.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbSuffix.Size = new System.Drawing.Size(82, 21);
            this.cmbSuffix.TabIndex = 9;
            this.cmbSuffix.TextChanged += new System.EventHandler(this.cmbSuffix_TextChanged);
            // 
            // rbtnMale
            // 
            this.rbtnMale.AutoSize = true;
            this.rbtnMale.Location = new System.Drawing.Point(750, 60);
            this.rbtnMale.Name = "rbtnMale";
            this.rbtnMale.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbtnMale.Size = new System.Drawing.Size(48, 17);
            this.rbtnMale.TabIndex = 24;
            this.rbtnMale.Text = "Male";
            this.rbtnMale.UseVisualStyleBackColor = true;
            this.rbtnMale.CheckedChanged += new System.EventHandler(this.rbtnMale_CheckedChanged);
            // 
            // rbtnFemale
            // 
            this.rbtnFemale.AutoSize = true;
            this.rbtnFemale.Checked = true;
            this.rbtnFemale.Location = new System.Drawing.Point(686, 60);
            this.rbtnFemale.Name = "rbtnFemale";
            this.rbtnFemale.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbtnFemale.Size = new System.Drawing.Size(59, 17);
            this.rbtnFemale.TabIndex = 23;
            this.rbtnFemale.TabStop = true;
            this.rbtnFemale.Text = "Female";
            this.rbtnFemale.UseVisualStyleBackColor = true;
            this.rbtnFemale.CheckedChanged += new System.EventHandler(this.rbtnFemale_CheckedChanged);
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(12, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(161, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Donor Registration";
            // 
            // gboxClientDetails
            // 
            this.gboxClientDetails.Controls.Add(this.cmbClient);
            this.gboxClientDetails.Controls.Add(this.cmbDepartment);
            this.gboxClientDetails.Controls.Add(this.lblClient);
            this.gboxClientDetails.Controls.Add(this.lblDepartment);
            this.gboxClientDetails.Controls.Add(this.label14);
            this.gboxClientDetails.Controls.Add(this.label13);
            this.gboxClientDetails.Location = new System.Drawing.Point(12, 501);
            this.gboxClientDetails.Name = "gboxClientDetails";
            this.gboxClientDetails.Size = new System.Drawing.Size(806, 70);
            this.gboxClientDetails.TabIndex = 4;
            this.gboxClientDetails.TabStop = false;
            this.gboxClientDetails.Text = "Client Details";
            // 
            // cmbClient
            // 
            this.cmbClient.DropDownHeight = 95;
            this.cmbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClient.FormattingEnabled = true;
            this.cmbClient.IntegralHeight = false;
            this.cmbClient.Items.AddRange(new object[] {
            "(Select Client)"});
            this.cmbClient.Location = new System.Drawing.Point(74, 28);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(255, 21);
            this.cmbClient.TabIndex = 2;
            this.cmbClient.SelectedIndexChanged += new System.EventHandler(this.cmbClient_SelectedIndexChanged);
            this.cmbClient.TextChanged += new System.EventHandler(this.cmbClient_TextChanged);
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownHeight = 95;
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.DropDownWidth = 225;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.IntegralHeight = false;
            this.cmbDepartment.Items.AddRange(new object[] {
            "(Select Department)"});
            this.cmbDepartment.Location = new System.Drawing.Point(445, 28);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(255, 21);
            this.cmbDepartment.TabIndex = 5;
            this.cmbDepartment.TextChanged += new System.EventHandler(this.cmbDepartment_TextChanged);
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(13, 32);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(33, 13);
            this.lblClient.TabIndex = 0;
            this.lblClient.Text = "Client";
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(365, 32);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(62, 13);
            this.lblDepartment.TabIndex = 3;
            this.lblDepartment.Text = "Department";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(426, 30);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 17);
            this.label14.TabIndex = 4;
            this.label14.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(44, 30);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "*";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(425, 580);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(334, 580);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblMandatoryField
            // 
            this.lblMandatoryField.AutoSize = true;
            this.lblMandatoryField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMandatoryField.ForeColor = System.Drawing.Color.Red;
            this.lblMandatoryField.Location = new System.Drawing.Point(717, 16);
            this.lblMandatoryField.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMandatoryField.Name = "lblMandatoryField";
            this.lblMandatoryField.Size = new System.Drawing.Size(94, 13);
            this.lblMandatoryField.TabIndex = 6;
            this.lblMandatoryField.Text = "* Mandatory Fields";
            // 
            // gboxDonorSearch
            // 
            this.gboxDonorSearch.Controls.Add(this.txtSearchZipCode);
            this.gboxDonorSearch.Controls.Add(this.btnReset);
            this.gboxDonorSearch.Controls.Add(this.cmbSearchYear);
            this.gboxDonorSearch.Controls.Add(this.cmbSearchDate);
            this.gboxDonorSearch.Controls.Add(this.cmbSearchMonth);
            this.gboxDonorSearch.Controls.Add(this.dgvSearch);
            this.gboxDonorSearch.Controls.Add(this.txtSearchSSN);
            this.gboxDonorSearch.Controls.Add(this.txtSearchEmail);
            this.gboxDonorSearch.Controls.Add(this.label19);
            this.gboxDonorSearch.Controls.Add(this.txtSearchCity);
            this.gboxDonorSearch.Controls.Add(this.label17);
            this.gboxDonorSearch.Controls.Add(this.label18);
            this.gboxDonorSearch.Controls.Add(this.label16);
            this.gboxDonorSearch.Controls.Add(this.txtSearchLastName);
            this.gboxDonorSearch.Controls.Add(this.txtSearchFirstName);
            this.gboxDonorSearch.Controls.Add(this.label5);
            this.gboxDonorSearch.Controls.Add(this.label8);
            this.gboxDonorSearch.Controls.Add(this.label15);
            this.gboxDonorSearch.Controls.Add(this.btnSearch);
            this.gboxDonorSearch.Location = new System.Drawing.Point(12, 35);
            this.gboxDonorSearch.Name = "gboxDonorSearch";
            this.gboxDonorSearch.Size = new System.Drawing.Size(806, 239);
            this.gboxDonorSearch.TabIndex = 2;
            this.gboxDonorSearch.TabStop = false;
            this.gboxDonorSearch.Text = "Search";
            // 
            // txtSearchZipCode
            // 
            this.txtSearchZipCode.Location = new System.Drawing.Point(497, 53);
            this.txtSearchZipCode.MaxLength = 5;
            this.txtSearchZipCode.Name = "txtSearchZipCode";
            this.txtSearchZipCode.Size = new System.Drawing.Size(94, 20);
            this.txtSearchZipCode.TabIndex = 13;
            this.txtSearchZipCode.WaterMark = "Enter  Zip Code";
            this.txtSearchZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchZipCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearchZipCode_KeyPress);
            // 
            // btnReset
            // 
            this.btnReset.AutoSize = true;
            this.btnReset.Location = new System.Drawing.Point(648, 83);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(69, 23);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cmbSearchYear
            // 
            this.cmbSearchYear.DropDownHeight = 90;
            this.cmbSearchYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchYear.FormattingEnabled = true;
            this.cmbSearchYear.IntegralHeight = false;
            this.cmbSearchYear.Items.AddRange(new object[] {
            "YYYY"});
            this.cmbSearchYear.Location = new System.Drawing.Point(169, 53);
            this.cmbSearchYear.Name = "cmbSearchYear";
            this.cmbSearchYear.Size = new System.Drawing.Size(52, 21);
            this.cmbSearchYear.TabIndex = 9;
            this.cmbSearchYear.TextChanged += new System.EventHandler(this.cmbSearchYear_TextChanged);
            // 
            // cmbSearchDate
            // 
            this.cmbSearchDate.DropDownHeight = 90;
            this.cmbSearchDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchDate.FormattingEnabled = true;
            this.cmbSearchDate.IntegralHeight = false;
            this.cmbSearchDate.ItemHeight = 13;
            this.cmbSearchDate.Items.AddRange(new object[] {
            "DD",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cmbSearchDate.Location = new System.Drawing.Point(127, 53);
            this.cmbSearchDate.Name = "cmbSearchDate";
            this.cmbSearchDate.Size = new System.Drawing.Size(40, 21);
            this.cmbSearchDate.TabIndex = 8;
            this.cmbSearchDate.TextChanged += new System.EventHandler(this.cmbSearchDate_TextChanged);
            // 
            // cmbSearchMonth
            // 
            this.cmbSearchMonth.DropDownHeight = 90;
            this.cmbSearchMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchMonth.FormattingEnabled = true;
            this.cmbSearchMonth.IntegralHeight = false;
            this.cmbSearchMonth.ItemHeight = 13;
            this.cmbSearchMonth.Items.AddRange(new object[] {
            "MM",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbSearchMonth.Location = new System.Drawing.Point(83, 53);
            this.cmbSearchMonth.Name = "cmbSearchMonth";
            this.cmbSearchMonth.Size = new System.Drawing.Size(42, 21);
            this.cmbSearchMonth.TabIndex = 7;
            this.cmbSearchMonth.TextChanged += new System.EventHandler(this.cmbSearchMonth_TextChanged);
            // 
            // dgvSearch
            // 
            this.dgvSearch.AllowUserToAddRows = false;
            this.dgvSearch.AllowUserToDeleteRows = false;
            this.dgvSearch.AllowUserToOrderColumns = true;
            this.dgvSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorID,
            this.FirstName,
            this.LastName,
            this.SSN,
            this.DonorSSN,
            this.DonorDOB,
            this.DOB,
            this.City,
            this.ZipCode,
            this.Email});
            this.dgvSearch.Location = new System.Drawing.Point(6, 112);
            this.dgvSearch.MultiSelect = false;
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.ReadOnly = true;
            this.dgvSearch.RowHeadersWidth = 20;
            this.dgvSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearch.Size = new System.Drawing.Size(793, 121);
            this.dgvSearch.TabIndex = 18;
            this.dgvSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearch_CellClick);
            this.dgvSearch.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearch_ColumnHeaderMouseClick);
            this.dgvSearch.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSearch_DataBindingComplete);
            // 
            // DonorID
            // 
            this.DonorID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DonorID.DataPropertyName = "DonorID";
            this.DonorID.HeaderText = "Donor ID";
            this.DonorID.Name = "DonorID";
            this.DonorID.ReadOnly = true;
            this.DonorID.Visible = false;
            // 
            // FirstName
            // 
            this.FirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FirstName.DataPropertyName = "DonorFirstName";
            this.FirstName.HeaderText = "First Name";
            this.FirstName.Name = "FirstName";
            this.FirstName.ReadOnly = true;
            // 
            // LastName
            // 
            this.LastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LastName.DataPropertyName = "DonorLastName";
            this.LastName.HeaderText = "Last Name";
            this.LastName.Name = "LastName";
            this.LastName.ReadOnly = true;
            // 
            // SSN
            // 
            this.SSN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SSN.HeaderText = "SSN";
            this.SSN.Name = "SSN";
            this.SSN.ReadOnly = true;
            // 
            // DonorSSN
            // 
            this.DonorSSN.DataPropertyName = "DonorSSN";
            this.DonorSSN.HeaderText = "Donor SSN";
            this.DonorSSN.Name = "DonorSSN";
            this.DonorSSN.ReadOnly = true;
            this.DonorSSN.Visible = false;
            // 
            // DonorDOB
            // 
            this.DonorDOB.DataPropertyName = "DonorDateOfBirth";
            this.DonorDOB.HeaderText = "DonorDOB";
            this.DonorDOB.Name = "DonorDOB";
            this.DonorDOB.ReadOnly = true;
            this.DonorDOB.Visible = false;
            // 
            // DOB
            // 
            this.DOB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DOB.HeaderText = "DOB";
            this.DOB.Name = "DOB";
            this.DOB.ReadOnly = true;
            // 
            // City
            // 
            this.City.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.City.DataPropertyName = "DonorCity";
            this.City.HeaderText = "City";
            this.City.Name = "City";
            this.City.ReadOnly = true;
            // 
            // ZipCode
            // 
            this.ZipCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ZipCode.DataPropertyName = "DonorZip";
            this.ZipCode.HeaderText = "Zip Code";
            this.ZipCode.Name = "ZipCode";
            this.ZipCode.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.DataPropertyName = "DonorEmail";
            this.Email.HeaderText = "Email";
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // txtSearchSSN
            // 
            this.txtSearchSSN.Location = new System.Drawing.Point(661, 20);
            this.txtSearchSSN.MaxLength = 4;
            this.txtSearchSSN.Name = "txtSearchSSN";
            this.txtSearchSSN.Size = new System.Drawing.Size(125, 20);
            this.txtSearchSSN.TabIndex = 5;
            this.txtSearchSSN.WaterMark = "Enter Last Four Digits";
            this.txtSearchSSN.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchSSN.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchSSN.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchSSN.TextChanged += new System.EventHandler(this.txtSearchEmail_TextChanged);
            // 
            // txtSearchEmail
            // 
            this.txtSearchEmail.Location = new System.Drawing.Point(661, 53);
            this.txtSearchEmail.Name = "txtSearchEmail";
            this.txtSearchEmail.Size = new System.Drawing.Size(125, 20);
            this.txtSearchEmail.TabIndex = 15;
            this.txtSearchEmail.WaterMark = "Enter Email";
            this.txtSearchEmail.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchEmail.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchEmail.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchEmail.TextChanged += new System.EventHandler(this.txtSearchEmail_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(614, 57);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(32, 13);
            this.label19.TabIndex = 14;
            this.label19.Text = "Email";
            // 
            // txtSearchCity
            // 
            this.txtSearchCity.Location = new System.Drawing.Point(272, 54);
            this.txtSearchCity.Name = "txtSearchCity";
            this.txtSearchCity.Size = new System.Drawing.Size(143, 20);
            this.txtSearchCity.TabIndex = 11;
            this.txtSearchCity.WaterMark = "Enter City";
            this.txtSearchCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchCity.TextChanged += new System.EventHandler(this.txtSearchCity_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(440, 57);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(50, 13);
            this.label17.TabIndex = 12;
            this.label17.Text = "Zip Code";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(239, 57);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "City";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(13, 57);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "D.O.B";
            // 
            // txtSearchLastName
            // 
            this.txtSearchLastName.Location = new System.Drawing.Point(375, 20);
            this.txtSearchLastName.Name = "txtSearchLastName";
            this.txtSearchLastName.Size = new System.Drawing.Size(198, 20);
            this.txtSearchLastName.TabIndex = 3;
            this.txtSearchLastName.WaterMark = "Enter Last  Name";
            this.txtSearchLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchLastName.TextChanged += new System.EventHandler(this.txtSearchLastName_TextChanged);
            // 
            // txtSearchFirstName
            // 
            this.txtSearchFirstName.Location = new System.Drawing.Point(83, 20);
            this.txtSearchFirstName.Name = "txtSearchFirstName";
            this.txtSearchFirstName.Size = new System.Drawing.Size(198, 20);
            this.txtSearchFirstName.TabIndex = 1;
            this.txtSearchFirstName.WaterMark = "Enter First Name";
            this.txtSearchFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSearchFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtSearchFirstName.TextChanged += new System.EventHandler(this.txtSearchFirstName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(614, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "SSN #";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(303, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Last Name";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 24);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "First Name";
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Location = new System.Drawing.Point(729, 83);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 23);
            this.btnSearch.TabIndex = 17;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // FrmDonorRegistraion
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(833, 613);
            this.Controls.Add(this.gboxDonorSearch);
            this.Controls.Add(this.lblMandatoryField);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.gboxClientDetails);
            this.Controls.Add(this.gboxDonorDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDonorRegistraion";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Donor Registration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDonorRegistraion_FormClosing);
            this.Load += new System.EventHandler(this.FrmDonorRegistraion_Load);
            this.gboxDonorDetails.ResumeLayout(false);
            this.gboxDonorDetails.PerformLayout();
            this.gboxClientDetails.ResumeLayout(false);
            this.gboxClientDetails.PerformLayout();
            this.gboxDonorSearch.ResumeLayout(false);
            this.gboxDonorSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.TextBoxes.SurTextBox txtMiddleInitial;
        private System.Windows.Forms.Label lblMiddleInitial;
        private Controls.TextBoxes.SurTextBox txtFirstName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.ComboBox cmbSuffix;
        private System.Windows.Forms.RadioButton rbtnMale;
        private System.Windows.Forms.RadioButton rbtnFemale;
        private Controls.TextBoxes.SurTextBox txtLastName;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.Label lblSSN;
        public System.Windows.Forms.MaskedTextBox txtSSN;
        private System.Windows.Forms.Label lblGender;
        private Controls.TextBoxes.SurTextBox txtEmail;
        private Controls.TextBoxes.SurTextBox txtCity;
        private Controls.TextBoxes.SurTextBox txtAddress2;
        private Controls.TextBoxes.SurTextBox txtAddress1;
        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.MaskedTextBox txtPhone2;
        private System.Windows.Forms.MaskedTextBox txtPhone1;
        private System.Windows.Forms.Label lblPhone2;
        private System.Windows.Forms.Label lblPhone1;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Label lblMandatoryField;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDepartmentNameMan;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.GroupBox gboxDonorDetails;
        public System.Windows.Forms.Label lblPageHeader;
        public System.Windows.Forms.GroupBox gboxClientDetails;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.GroupBox gboxDonorSearch;
        private Controls.TextBoxes.SurTextBox txtSearchLastName;
        private Controls.TextBoxes.SurTextBox txtSearchFirstName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private Controls.TextBoxes.SurTextBox txtSearchEmail;
        private System.Windows.Forms.Label label19;
        private Controls.TextBoxes.SurTextBox txtSearchCity;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorSSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorDOB;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOB;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZipCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.ComboBox cmbSearchYear;
        private System.Windows.Forms.ComboBox cmbSearchDate;
        private System.Windows.Forms.ComboBox cmbSearchMonth;
        private System.Windows.Forms.ComboBox cmbDonorYear;
        private System.Windows.Forms.ComboBox cmbDonorDate;
        private System.Windows.Forms.ComboBox cmbDonorMonth;
        private Controls.TextBoxes.SurTextBox txtSearchSSN;
        private System.Windows.Forms.Button btnReset;
        private Controls.TextBoxes.SurTextBox txtZipCode;
        private Controls.TextBoxes.SurTextBox txtSearchZipCode;
        public System.Windows.Forms.CheckBox chkWalkin;
        public Controls.TextBoxes.SurTextBox ssndup;
    }
}