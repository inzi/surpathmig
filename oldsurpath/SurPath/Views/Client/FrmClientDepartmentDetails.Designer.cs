using Serilog;

namespace SurPath
{
    partial class FrmClientDepartmentDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClientDepartmentDetails));
            this.lblClientName = new System.Windows.Forms.Label();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.lblClientDeptMandatory = new System.Windows.Forms.Label();
            this.tabRecordKeeping = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblduplicateLabCodeFrom = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtduplicatetoLabCode = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnAddRetain = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNotify3 = new System.Windows.Forms.TextBox();
            this.txtNotify2 = new System.Windows.Forms.TextBox();
            this.txtNotify1 = new System.Windows.Forms.TextBox();
            this.txtInstructions = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkArchived = new System.Windows.Forms.CheckBox();
            this.txtSemester = new System.Windows.Forms.ComboBox();
            this.chkRequired = new System.Windows.Forms.CheckBox();
            this.chkExpires = new System.Windows.Forms.CheckBox();
            this.chkNotifyStudent = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtDueDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dg = new System.Windows.Forms.DataGridView();
            this.DepartmentInfo = new System.Windows.Forms.TabPage();
            this.lblUseFormFox = new System.Windows.Forms.Label();
            this.lblFormFoxCode = new System.Windows.Forms.Label();
            this.txtFormFoxCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.chkRecordKeeping = new System.Windows.Forms.CheckBox();
            this.chkBC = new System.Windows.Forms.CheckBox();
            this.btnClearStarAssign = new System.Windows.Forms.Button();
            this.lblClearStarClientCode = new System.Windows.Forms.Label();
            this.txtClearStarClientCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtLabCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtQuestCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalState = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingState = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtEmail = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtDepartmentName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtSalesRepresentative = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLabCode = new System.Windows.Forms.Label();
            this.lblQuestCode = new System.Windows.Forms.Label();
            this.cmbSalesRepresentative = new System.Windows.Forms.ComboBox();
            this.lblSalesInfo = new System.Windows.Forms.Label();
            this.lblSalesRepresentive = new System.Windows.Forms.Label();
            this.cmbPhysicalState = new System.Windows.Forms.ComboBox();
            this.lblPhysicalZipCode = new System.Windows.Forms.Label();
            this.lblPhysicalState = new System.Windows.Forms.Label();
            this.lblPhysicalCity = new System.Windows.Forms.Label();
            this.lblPhysicalAddress1 = new System.Windows.Forms.Label();
            this.lblPhysicalAddress2 = new System.Windows.Forms.Label();
            this.lblPhysicalAddress = new System.Windows.Forms.Label();
            this.cmbMailingState = new System.Windows.Forms.ComboBox();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.chkSameAsClient = new System.Windows.Forms.CheckBox();
            this.chkSameAsPhysical = new System.Windows.Forms.CheckBox();
            this.lblMailingState = new System.Windows.Forms.Label();
            this.lblMailingCity = new System.Windows.Forms.Label();
            this.lblMailingAddress2 = new System.Windows.Forms.Label();
            this.lblMailingAddress1 = new System.Windows.Forms.Label();
            this.lblMailingAddress = new System.Windows.Forms.Label();
            this.txtFax = new System.Windows.Forms.MaskedTextBox();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblMainContactInfo = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblPaymentMan = new System.Windows.Forms.Label();
            this.pnlPaymentType = new System.Windows.Forms.Panel();
            this.rbInvoiceClient = new System.Windows.Forms.RadioButton();
            this.rbDonorPays = new System.Windows.Forms.RadioButton();
            this.lblPaymentType = new System.Windows.Forms.Label();
            this.lblMROMan = new System.Windows.Forms.Label();
            this.lblCategoryMan = new System.Windows.Forms.Label();
            this.lblDepartmentNameMan = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlMROType = new System.Windows.Forms.Panel();
            this.rbMALL = new System.Windows.Forms.RadioButton();
            this.rbMPOS = new System.Windows.Forms.RadioButton();
            this.lblMROType = new System.Windows.Forms.Label();
            this.chkDNA = new System.Windows.Forms.CheckBox();
            this.chkHair = new System.Windows.Forms.CheckBox();
            this.chkUA = new System.Windows.Forms.CheckBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDepartmentName = new System.Windows.Forms.Label();
            this.tabDeptDetailsTabs = new System.Windows.Forms.TabControl();
            this.tabNotificationSettingsClientDepartmentDetails = new System.Windows.Forms.TabPage();
            this.groupBoxRandomizationSettings = new System.Windows.Forms.GroupBox();
            this.nudMaxSendIns = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.nudDeadlineAlert = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.dtSweepDate = new System.Windows.Forms.DateTimePicker();
            this.label23 = new System.Windows.Forms.Label();
            this.chkForceManualNotificaitons = new System.Windows.Forms.CheckBox();
            this.dtSendInStart = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.dtSendInStop = new System.Windows.Forms.DateTimePicker();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtSMSReply = new System.Windows.Forms.TextBox();
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtPreviewPhone = new System.Windows.Forms.TextBox();
            this.txtPreviewEmail = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPreviewZipCode = new System.Windows.Forms.TextBox();
            this.btnEmailPreview = new System.Windows.Forms.Button();
            this.btnLoadDefaults = new System.Windows.Forms.Button();
            this.btnCopySettings = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.cmboCopyWindowSettings = new System.Windows.Forms.ComboBox();
            this.gboxAdvanced = new System.Windows.Forms.GroupBox();
            this.txtGlobalMROName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtFilenamePDFTemplate = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtFilenameRenderSettings = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnResetJSON = new System.Windows.Forms.Button();
            this.txtJSONTemplateSettings = new System.Windows.Forms.TextBox();
            this.btnResetNotification = new System.Windows.Forms.Button();
            this.btnSaveNotification = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkUseFormFox = new System.Windows.Forms.CheckBox();
            this.ckbWebShowNotifyButton = new System.Windows.Forms.CheckBox();
            this.chkEnableSMS = new System.Windows.Forms.CheckBox();
            this.chkOnsiteTesting = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numDelayHours = new System.Windows.Forms.NumericUpDown();
            this.radASAPorDelay2 = new System.Windows.Forms.RadioButton();
            this.radASAPorDelay1 = new System.Windows.Forms.RadioButton();
            this.groupNotifcationDaySettings = new System.Windows.Forms.GroupBox();
            this.dtSaturdayEnd = new System.Windows.Forms.DateTimePicker();
            this.dtWednesdayEnd = new System.Windows.Forms.DateTimePicker();
            this.dtSaturdayStart = new System.Windows.Forms.DateTimePicker();
            this.chkSunday = new System.Windows.Forms.CheckBox();
            this.dtFridayEnd = new System.Windows.Forms.DateTimePicker();
            this.chkSaturday = new System.Windows.Forms.CheckBox();
            this.dtFridayStart = new System.Windows.Forms.DateTimePicker();
            this.chkMonday = new System.Windows.Forms.CheckBox();
            this.dtThursdayEnd = new System.Windows.Forms.DateTimePicker();
            this.chkTuesday = new System.Windows.Forms.CheckBox();
            this.dtThursdayStart = new System.Windows.Forms.DateTimePicker();
            this.chkWednesday = new System.Windows.Forms.CheckBox();
            this.chkThursday = new System.Windows.Forms.CheckBox();
            this.dtWednesdayStart = new System.Windows.Forms.DateTimePicker();
            this.chkFriday = new System.Windows.Forms.CheckBox();
            this.dtTuesdayEnd = new System.Windows.Forms.DateTimePicker();
            this.dtSundayStart = new System.Windows.Forms.DateTimePicker();
            this.dtTuesdayStart = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.dtMondayEnd = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.dtMondayStart = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.dtSundayEnd = new System.Windows.Forms.DateTimePicker();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.tabRecordKeeping.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.DepartmentInfo.SuspendLayout();
            this.pnlPaymentType.SuspendLayout();
            this.pnlMROType.SuspendLayout();
            this.tabDeptDetailsTabs.SuspendLayout();
            this.tabNotificationSettingsClientDepartmentDetails.SuspendLayout();
            this.groupBoxRandomizationSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxSendIns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadlineAlert)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.groupBoxTools.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gboxAdvanced.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayHours)).BeginInit();
            this.groupNotifcationDaySettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientName.Location = new System.Drawing.Point(12, 35);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(0, 20);
            this.lblClientName.TabIndex = 1;
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(12, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(216, 20);
            this.lblPageHeader.TabIndex = 70;
            this.lblPageHeader.Text = "Client Department Details";
            // 
            // lblClientDeptMandatory
            // 
            this.lblClientDeptMandatory.AutoSize = true;
            this.lblClientDeptMandatory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientDeptMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblClientDeptMandatory.Location = new System.Drawing.Point(230, 15);
            this.lblClientDeptMandatory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClientDeptMandatory.Name = "lblClientDeptMandatory";
            this.lblClientDeptMandatory.Size = new System.Drawing.Size(94, 13);
            this.lblClientDeptMandatory.TabIndex = 69;
            this.lblClientDeptMandatory.Text = "* Mandatory Fields";
            // 
            // tabRecordKeeping
            // 
            this.tabRecordKeeping.AutoScroll = true;
            this.tabRecordKeeping.BackColor = System.Drawing.Color.Silver;
            this.tabRecordKeeping.Controls.Add(this.groupBox1);
            this.tabRecordKeeping.Controls.Add(this.btnAddRetain);
            this.tabRecordKeeping.Controls.Add(this.label9);
            this.tabRecordKeeping.Controls.Add(this.txtID);
            this.tabRecordKeeping.Controls.Add(this.lblID);
            this.tabRecordKeeping.Controls.Add(this.btnUpdate);
            this.tabRecordKeeping.Controls.Add(this.btnClearFields);
            this.tabRecordKeeping.Controls.Add(this.label8);
            this.tabRecordKeeping.Controls.Add(this.txtNotify3);
            this.tabRecordKeeping.Controls.Add(this.txtNotify2);
            this.tabRecordKeeping.Controls.Add(this.txtNotify1);
            this.tabRecordKeeping.Controls.Add(this.txtInstructions);
            this.tabRecordKeeping.Controls.Add(this.txtDescription);
            this.tabRecordKeeping.Controls.Add(this.label7);
            this.tabRecordKeeping.Controls.Add(this.label6);
            this.tabRecordKeeping.Controls.Add(this.chkArchived);
            this.tabRecordKeeping.Controls.Add(this.txtSemester);
            this.tabRecordKeeping.Controls.Add(this.chkRequired);
            this.tabRecordKeeping.Controls.Add(this.chkExpires);
            this.tabRecordKeeping.Controls.Add(this.chkNotifyStudent);
            this.tabRecordKeeping.Controls.Add(this.label3);
            this.tabRecordKeeping.Controls.Add(this.dtDueDate);
            this.tabRecordKeeping.Controls.Add(this.label4);
            this.tabRecordKeeping.Controls.Add(this.label2);
            this.tabRecordKeeping.Controls.Add(this.btnAdd);
            this.tabRecordKeeping.Controls.Add(this.label5);
            this.tabRecordKeeping.Controls.Add(this.dg);
            this.tabRecordKeeping.Location = new System.Drawing.Point(4, 22);
            this.tabRecordKeeping.Margin = new System.Windows.Forms.Padding(2);
            this.tabRecordKeeping.Name = "tabRecordKeeping";
            this.tabRecordKeeping.Padding = new System.Windows.Forms.Padding(2);
            this.tabRecordKeeping.Size = new System.Drawing.Size(1268, 865);
            this.tabRecordKeeping.TabIndex = 1;
            this.tabRecordKeeping.Text = "Record Keeping";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lblduplicateLabCodeFrom);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtduplicatetoLabCode);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(850, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 226);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Duplicate";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(5, 96);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(236, 20);
            this.label12.TabIndex = 78;
            this.label12.Text = "To";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(5, 146);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(236, 31);
            this.label11.TabIndex = 77;
            this.label11.Text = "Use with Caution";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblduplicateLabCodeFrom
            // 
            this.lblduplicateLabCodeFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblduplicateLabCodeFrom.Location = new System.Drawing.Point(7, 65);
            this.lblduplicateLabCodeFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblduplicateLabCodeFrom.Name = "lblduplicateLabCodeFrom";
            this.lblduplicateLabCodeFrom.Size = new System.Drawing.Size(233, 33);
            this.lblduplicateLabCodeFrom.TabIndex = 77;
            this.lblduplicateLabCodeFrom.Text = "LABCODE";
            this.lblduplicateLabCodeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label10.Location = new System.Drawing.Point(5, 30);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(236, 40);
            this.label10.TabIndex = 76;
            this.label10.Text = "Copy all of the document types from one department  to this department";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtduplicatetoLabCode
            // 
            this.txtduplicatetoLabCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtduplicatetoLabCode.Location = new System.Drawing.Point(7, 122);
            this.txtduplicatetoLabCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtduplicatetoLabCode.Name = "txtduplicatetoLabCode";
            this.txtduplicatetoLabCode.Size = new System.Drawing.Size(233, 22);
            this.txtduplicatetoLabCode.TabIndex = 75;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(62, 179);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 33);
            this.button2.TabIndex = 72;
            this.button2.Text = "Copy";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnAddRetain
            // 
            this.btnAddRetain.Location = new System.Drawing.Point(523, 459);
            this.btnAddRetain.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddRetain.Name = "btnAddRetain";
            this.btnAddRetain.Size = new System.Drawing.Size(122, 33);
            this.btnAddRetain.TabIndex = 71;
            this.btnAddRetain.Text = "Add - Retain Fields";
            this.btnAddRetain.UseVisualStyleBackColor = true;
            this.btnAddRetain.Click += new System.EventHandler(this.BtnAddRetain_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(23, 272);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(276, 13);
            this.label9.TabIndex = 70;
            this.label9.Text = "Note: Double Click On Any Item Above  To Edit";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.Location = new System.Drawing.Point(467, 280);
            this.txtID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(36, 13);
            this.txtID.TabIndex = 39;
            this.txtID.TextChanged += new System.EventHandler(this.TxtID_TextChanged);
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(445, 280);
            this.lblID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(18, 13);
            this.lblID.TabIndex = 38;
            this.lblID.Text = "ID";
            this.lblID.TextChanged += new System.EventHandler(this.LblID_TextChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(523, 387);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(122, 33);
            this.btnUpdate.TabIndex = 37;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(523, 350);
            this.btnClearFields.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(122, 33);
            this.btnClearFields.TabIndex = 36;
            this.btnClearFields.Text = "Clear Fields";
            this.btnClearFields.UseVisualStyleBackColor = true;
            this.btnClearFields.Click += new System.EventHandler(this.BtnClearFields_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(380, 517);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Nottify Days 3rd";
            // 
            // txtNotify3
            // 
            this.txtNotify3.Location = new System.Drawing.Point(465, 514);
            this.txtNotify3.Margin = new System.Windows.Forms.Padding(2);
            this.txtNotify3.Name = "txtNotify3";
            this.txtNotify3.Size = new System.Drawing.Size(40, 20);
            this.txtNotify3.TabIndex = 34;
            this.txtNotify3.Text = "10";
            // 
            // txtNotify2
            // 
            this.txtNotify2.Location = new System.Drawing.Point(465, 490);
            this.txtNotify2.Margin = new System.Windows.Forms.Padding(2);
            this.txtNotify2.Name = "txtNotify2";
            this.txtNotify2.Size = new System.Drawing.Size(40, 20);
            this.txtNotify2.TabIndex = 32;
            this.txtNotify2.Text = "30";
            // 
            // txtNotify1
            // 
            this.txtNotify1.Location = new System.Drawing.Point(465, 466);
            this.txtNotify1.Margin = new System.Windows.Forms.Padding(2);
            this.txtNotify1.Name = "txtNotify1";
            this.txtNotify1.Size = new System.Drawing.Size(40, 20);
            this.txtNotify1.TabIndex = 30;
            this.txtNotify1.Text = "60";
            // 
            // txtInstructions
            // 
            this.txtInstructions.Location = new System.Drawing.Point(130, 346);
            this.txtInstructions.Margin = new System.Windows.Forms.Padding(2);
            this.txtInstructions.Multiline = true;
            this.txtInstructions.Name = "txtInstructions";
            this.txtInstructions.Size = new System.Drawing.Size(375, 111);
            this.txtInstructions.TabIndex = 21;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(130, 295);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(375, 20);
            this.txtDescription.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(380, 493);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Nottify Days 2nd";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(380, 469);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Nottify Days 1st";
            // 
            // chkArchived
            // 
            this.chkArchived.AutoSize = true;
            this.chkArchived.Location = new System.Drawing.Point(130, 563);
            this.chkArchived.Name = "chkArchived";
            this.chkArchived.Size = new System.Drawing.Size(68, 17);
            this.chkArchived.TabIndex = 29;
            this.chkArchived.Text = "Archived";
            this.chkArchived.UseVisualStyleBackColor = true;
            // 
            // txtSemester
            // 
            this.txtSemester.FormattingEnabled = true;
            this.txtSemester.Items.AddRange(new object[] {
            "Spring",
            "Summer",
            "Fall",
            "Winter",
            "1st",
            "2nd",
            "3rd",
            "4th",
            "Night",
            "Weekend"});
            this.txtSemester.Location = new System.Drawing.Point(130, 320);
            this.txtSemester.Name = "txtSemester";
            this.txtSemester.Size = new System.Drawing.Size(266, 21);
            this.txtSemester.TabIndex = 28;
            // 
            // chkRequired
            // 
            this.chkRequired.AutoSize = true;
            this.chkRequired.Location = new System.Drawing.Point(130, 540);
            this.chkRequired.Name = "chkRequired";
            this.chkRequired.Size = new System.Drawing.Size(69, 17);
            this.chkRequired.TabIndex = 27;
            this.chkRequired.Text = "Required";
            this.chkRequired.UseVisualStyleBackColor = true;
            // 
            // chkExpires
            // 
            this.chkExpires.AutoSize = true;
            this.chkExpires.Location = new System.Drawing.Point(130, 517);
            this.chkExpires.Name = "chkExpires";
            this.chkExpires.Size = new System.Drawing.Size(60, 17);
            this.chkExpires.TabIndex = 26;
            this.chkExpires.Text = "Expires";
            this.chkExpires.UseVisualStyleBackColor = true;
            // 
            // chkNotifyStudent
            // 
            this.chkNotifyStudent.AutoSize = true;
            this.chkNotifyStudent.Location = new System.Drawing.Point(130, 494);
            this.chkNotifyStudent.Name = "chkNotifyStudent";
            this.chkNotifyStudent.Size = new System.Drawing.Size(93, 17);
            this.chkNotifyStudent.TabIndex = 25;
            this.chkNotifyStudent.Text = "Notify Student";
            this.chkNotifyStudent.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 323);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Semester";
            // 
            // dtDueDate
            // 
            this.dtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDueDate.Location = new System.Drawing.Point(130, 463);
            this.dtDueDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtDueDate.Name = "dtDueDate";
            this.dtDueDate.Size = new System.Drawing.Size(151, 20);
            this.dtDueDate.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AccessibleRole = System.Windows.Forms.AccessibleRole.Cursor;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 466);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Due Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 349);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Instructions to Students";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(523, 424);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(122, 33);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 298);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Document Description";
            // 
            // dg
            // 
            this.dg.AllowUserToAddRows = false;
            this.dg.AllowUserToDeleteRows = false;
            this.dg.AllowUserToOrderColumns = true;
            this.dg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Location = new System.Drawing.Point(5, 5);
            this.dg.MultiSelect = false;
            this.dg.Name = "dg";
            this.dg.ReadOnly = true;
            this.dg.RowHeadersWidth = 25;
            this.dg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg.Size = new System.Drawing.Size(1535, 249);
            this.dg.TabIndex = 16;
            this.dg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dg_CellDoubleClick);
            // 
            // DepartmentInfo
            // 
            this.DepartmentInfo.AutoScroll = true;
            this.DepartmentInfo.BackColor = System.Drawing.Color.Silver;
            this.DepartmentInfo.Controls.Add(this.lblUseFormFox);
            this.DepartmentInfo.Controls.Add(this.lblFormFoxCode);
            this.DepartmentInfo.Controls.Add(this.txtFormFoxCode);
            this.DepartmentInfo.Controls.Add(this.chkRecordKeeping);
            this.DepartmentInfo.Controls.Add(this.chkBC);
            this.DepartmentInfo.Controls.Add(this.btnClearStarAssign);
            this.DepartmentInfo.Controls.Add(this.lblClearStarClientCode);
            this.DepartmentInfo.Controls.Add(this.txtClearStarClientCode);
            this.DepartmentInfo.Controls.Add(this.txtLabCode);
            this.DepartmentInfo.Controls.Add(this.txtQuestCode);
            this.DepartmentInfo.Controls.Add(this.txtMailingZipCode);
            this.DepartmentInfo.Controls.Add(this.txtPhysicalZipCode);
            this.DepartmentInfo.Controls.Add(this.txtPhysicalCity);
            this.DepartmentInfo.Controls.Add(this.txtPhysicalState);
            this.DepartmentInfo.Controls.Add(this.txtPhysicalAddress2);
            this.DepartmentInfo.Controls.Add(this.txtPhysicalAddress1);
            this.DepartmentInfo.Controls.Add(this.txtMailingState);
            this.DepartmentInfo.Controls.Add(this.txtMailingCity);
            this.DepartmentInfo.Controls.Add(this.txtMailingAddress2);
            this.DepartmentInfo.Controls.Add(this.txtMailingAddress1);
            this.DepartmentInfo.Controls.Add(this.txtLastName);
            this.DepartmentInfo.Controls.Add(this.txtEmail);
            this.DepartmentInfo.Controls.Add(this.txtFirstName);
            this.DepartmentInfo.Controls.Add(this.txtDepartmentName);
            this.DepartmentInfo.Controls.Add(this.txtSalesRepresentative);
            this.DepartmentInfo.Controls.Add(this.label1);
            this.DepartmentInfo.Controls.Add(this.lblLabCode);
            this.DepartmentInfo.Controls.Add(this.lblQuestCode);
            this.DepartmentInfo.Controls.Add(this.cmbSalesRepresentative);
            this.DepartmentInfo.Controls.Add(this.lblSalesInfo);
            this.DepartmentInfo.Controls.Add(this.lblSalesRepresentive);
            this.DepartmentInfo.Controls.Add(this.cmbPhysicalState);
            this.DepartmentInfo.Controls.Add(this.lblPhysicalZipCode);
            this.DepartmentInfo.Controls.Add(this.lblPhysicalState);
            this.DepartmentInfo.Controls.Add(this.lblPhysicalCity);
            this.DepartmentInfo.Controls.Add(this.lblPhysicalAddress1);
            this.DepartmentInfo.Controls.Add(this.lblPhysicalAddress2);
            this.DepartmentInfo.Controls.Add(this.lblPhysicalAddress);
            this.DepartmentInfo.Controls.Add(this.cmbMailingState);
            this.DepartmentInfo.Controls.Add(this.lblZipCode);
            this.DepartmentInfo.Controls.Add(this.chkSameAsClient);
            this.DepartmentInfo.Controls.Add(this.chkSameAsPhysical);
            this.DepartmentInfo.Controls.Add(this.lblMailingState);
            this.DepartmentInfo.Controls.Add(this.lblMailingCity);
            this.DepartmentInfo.Controls.Add(this.lblMailingAddress2);
            this.DepartmentInfo.Controls.Add(this.lblMailingAddress1);
            this.DepartmentInfo.Controls.Add(this.lblMailingAddress);
            this.DepartmentInfo.Controls.Add(this.txtFax);
            this.DepartmentInfo.Controls.Add(this.txtPhone);
            this.DepartmentInfo.Controls.Add(this.lblEmail);
            this.DepartmentInfo.Controls.Add(this.lblFax);
            this.DepartmentInfo.Controls.Add(this.lblPhone);
            this.DepartmentInfo.Controls.Add(this.lblLastName);
            this.DepartmentInfo.Controls.Add(this.lblMainContactInfo);
            this.DepartmentInfo.Controls.Add(this.lblFirstName);
            this.DepartmentInfo.Controls.Add(this.chkActive);
            this.DepartmentInfo.Controls.Add(this.lblPaymentMan);
            this.DepartmentInfo.Controls.Add(this.pnlPaymentType);
            this.DepartmentInfo.Controls.Add(this.lblPaymentType);
            this.DepartmentInfo.Controls.Add(this.lblMROMan);
            this.DepartmentInfo.Controls.Add(this.lblCategoryMan);
            this.DepartmentInfo.Controls.Add(this.lblDepartmentNameMan);
            this.DepartmentInfo.Controls.Add(this.btnClose);
            this.DepartmentInfo.Controls.Add(this.btnOk);
            this.DepartmentInfo.Controls.Add(this.pnlMROType);
            this.DepartmentInfo.Controls.Add(this.lblMROType);
            this.DepartmentInfo.Controls.Add(this.chkDNA);
            this.DepartmentInfo.Controls.Add(this.chkHair);
            this.DepartmentInfo.Controls.Add(this.chkUA);
            this.DepartmentInfo.Controls.Add(this.lblCategory);
            this.DepartmentInfo.Controls.Add(this.lblDepartmentName);
            this.DepartmentInfo.Location = new System.Drawing.Point(4, 22);
            this.DepartmentInfo.Margin = new System.Windows.Forms.Padding(2);
            this.DepartmentInfo.Name = "DepartmentInfo";
            this.DepartmentInfo.Padding = new System.Windows.Forms.Padding(2);
            this.DepartmentInfo.Size = new System.Drawing.Size(1268, 865);
            this.DepartmentInfo.TabIndex = 0;
            this.DepartmentInfo.Text = "Department Info";
            this.DepartmentInfo.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // lblUseFormFox
            // 
            this.lblUseFormFox.AutoSize = true;
            this.lblUseFormFox.Location = new System.Drawing.Point(321, 142);
            this.lblUseFormFox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUseFormFox.Name = "lblUseFormFox";
            this.lblUseFormFox.Size = new System.Drawing.Size(61, 13);
            this.lblUseFormFox.TabIndex = 146;
            this.lblUseFormFox.Text = "FF Enabled";
            // 
            // lblFormFoxCode
            // 
            this.lblFormFoxCode.AutoSize = true;
            this.lblFormFoxCode.Location = new System.Drawing.Point(11, 142);
            this.lblFormFoxCode.Name = "lblFormFoxCode";
            this.lblFormFoxCode.Size = new System.Drawing.Size(75, 13);
            this.lblFormFoxCode.TabIndex = 145;
            this.lblFormFoxCode.Text = "FormFox Code";
            // 
            // txtFormFoxCode
            // 
            this.txtFormFoxCode.Location = new System.Drawing.Point(117, 139);
            this.txtFormFoxCode.MaxLength = 35;
            this.txtFormFoxCode.Name = "txtFormFoxCode";
            this.txtFormFoxCode.Size = new System.Drawing.Size(199, 20);
            this.txtFormFoxCode.TabIndex = 144;
            this.txtFormFoxCode.WaterMark = "Not Assigned";
            this.txtFormFoxCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFormFoxCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormFoxCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtFormFoxCode.TextChanged += new System.EventHandler(this.txtFormFoxCode_TextChanged);
            // 
            // chkRecordKeeping
            // 
            this.chkRecordKeeping.AutoSize = true;
            this.chkRecordKeeping.Location = new System.Drawing.Point(123, 194);
            this.chkRecordKeeping.Name = "chkRecordKeeping";
            this.chkRecordKeeping.Size = new System.Drawing.Size(103, 17);
            this.chkRecordKeeping.TabIndex = 143;
            this.chkRecordKeeping.Text = "&Record Keeping";
            this.chkRecordKeeping.UseVisualStyleBackColor = true;
            this.chkRecordKeeping.CheckedChanged += new System.EventHandler(this.chkRecordKeeping_CheckChanged);
            // 
            // chkBC
            // 
            this.chkBC.AutoSize = true;
            this.chkBC.Location = new System.Drawing.Point(224, 170);
            this.chkBC.Name = "chkBC";
            this.chkBC.Size = new System.Drawing.Size(40, 17);
            this.chkBC.TabIndex = 142;
            this.chkBC.Text = "&BC";
            this.chkBC.UseVisualStyleBackColor = true;
            // 
            // btnClearStarAssign
            // 
            this.btnClearStarAssign.Location = new System.Drawing.Point(319, 107);
            this.btnClearStarAssign.Name = "btnClearStarAssign";
            this.btnClearStarAssign.Size = new System.Drawing.Size(56, 23);
            this.btnClearStarAssign.TabIndex = 141;
            this.btnClearStarAssign.Text = "Assign";
            this.btnClearStarAssign.UseVisualStyleBackColor = true;
            this.btnClearStarAssign.Click += new System.EventHandler(this.btnClearStarAssign_Click);
            // 
            // lblClearStarClientCode
            // 
            this.lblClearStarClientCode.AutoSize = true;
            this.lblClearStarClientCode.Location = new System.Drawing.Point(11, 111);
            this.lblClearStarClientCode.Name = "lblClearStarClientCode";
            this.lblClearStarClientCode.Size = new System.Drawing.Size(81, 13);
            this.lblClearStarClientCode.TabIndex = 140;
            this.lblClearStarClientCode.Text = "Clear&Star Code:";
            // 
            // txtClearStarClientCode
            // 
            this.txtClearStarClientCode.Location = new System.Drawing.Point(117, 108);
            this.txtClearStarClientCode.MaxLength = 35;
            this.txtClearStarClientCode.Name = "txtClearStarClientCode";
            this.txtClearStarClientCode.Size = new System.Drawing.Size(199, 20);
            this.txtClearStarClientCode.TabIndex = 139;
            this.txtClearStarClientCode.WaterMark = "Not Assigned";
            this.txtClearStarClientCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtClearStarClientCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClearStarClientCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtLabCode
            // 
            this.txtLabCode.Location = new System.Drawing.Point(118, 54);
            this.txtLabCode.MaxLength = 35;
            this.txtLabCode.Name = "txtLabCode";
            this.txtLabCode.Size = new System.Drawing.Size(198, 20);
            this.txtLabCode.TabIndex = 83;
            this.txtLabCode.WaterMark = "Enter Lab Code";
            this.txtLabCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLabCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtQuestCode
            // 
            this.txtQuestCode.Location = new System.Drawing.Point(118, 82);
            this.txtQuestCode.MaxLength = 35;
            this.txtQuestCode.Name = "txtQuestCode";
            this.txtQuestCode.Size = new System.Drawing.Size(198, 20);
            this.txtQuestCode.TabIndex = 84;
            this.txtQuestCode.WaterMark = "Enter Quest Code";
            this.txtQuestCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtQuestCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuestCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingZipCode
            // 
            this.txtMailingZipCode.Location = new System.Drawing.Point(488, 325);
            this.txtMailingZipCode.MaxLength = 5;
            this.txtMailingZipCode.Name = "txtMailingZipCode";
            this.txtMailingZipCode.Size = new System.Drawing.Size(93, 20);
            this.txtMailingZipCode.TabIndex = 132;
            this.txtMailingZipCode.WaterMark = "Enter  Zip Code";
            this.txtMailingZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtPhysicalZipCode
            // 
            this.txtPhysicalZipCode.Location = new System.Drawing.Point(93, 449);
            this.txtPhysicalZipCode.MaxLength = 5;
            this.txtPhysicalZipCode.Name = "txtPhysicalZipCode";
            this.txtPhysicalZipCode.Size = new System.Drawing.Size(93, 20);
            this.txtPhysicalZipCode.TabIndex = 119;
            this.txtPhysicalZipCode.WaterMark = "Enter  Zip Code";
            this.txtPhysicalZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtPhysicalCity
            // 
            this.txtPhysicalCity.Location = new System.Drawing.Point(93, 418);
            this.txtPhysicalCity.MaxLength = 225;
            this.txtPhysicalCity.Multiline = true;
            this.txtPhysicalCity.Name = "txtPhysicalCity";
            this.txtPhysicalCity.Size = new System.Drawing.Size(113, 20);
            this.txtPhysicalCity.TabIndex = 115;
            this.txtPhysicalCity.WaterMark = "Enter City";
            this.txtPhysicalCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtPhysicalState
            // 
            this.txtPhysicalState.Location = new System.Drawing.Point(256, 417);
            this.txtPhysicalState.MaxLength = 225;
            this.txtPhysicalState.Multiline = true;
            this.txtPhysicalState.Name = "txtPhysicalState";
            this.txtPhysicalState.Size = new System.Drawing.Size(110, 21);
            this.txtPhysicalState.TabIndex = 103;
            this.txtPhysicalState.WaterMark = "Enter  City";
            this.txtPhysicalState.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalState.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalState.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtPhysicalAddress2
            // 
            this.txtPhysicalAddress2.Location = new System.Drawing.Point(93, 386);
            this.txtPhysicalAddress2.MaxLength = 225;
            this.txtPhysicalAddress2.Multiline = true;
            this.txtPhysicalAddress2.Name = "txtPhysicalAddress2";
            this.txtPhysicalAddress2.Size = new System.Drawing.Size(201, 20);
            this.txtPhysicalAddress2.TabIndex = 113;
            this.txtPhysicalAddress2.WaterMark = "Enter Address 2";
            this.txtPhysicalAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtPhysicalAddress1
            // 
            this.txtPhysicalAddress1.Location = new System.Drawing.Point(93, 354);
            this.txtPhysicalAddress1.MaxLength = 225;
            this.txtPhysicalAddress1.Multiline = true;
            this.txtPhysicalAddress1.Name = "txtPhysicalAddress1";
            this.txtPhysicalAddress1.Size = new System.Drawing.Size(201, 20);
            this.txtPhysicalAddress1.TabIndex = 111;
            this.txtPhysicalAddress1.WaterMark = "Enter Address 1";
            this.txtPhysicalAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingState
            // 
            this.txtMailingState.Location = new System.Drawing.Point(646, 291);
            this.txtMailingState.MaxLength = 225;
            this.txtMailingState.Multiline = true;
            this.txtMailingState.Name = "txtMailingState";
            this.txtMailingState.Size = new System.Drawing.Size(110, 21);
            this.txtMailingState.TabIndex = 130;
            this.txtMailingState.WaterMark = "Enter City";
            this.txtMailingState.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingState.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingState.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingCity
            // 
            this.txtMailingCity.Location = new System.Drawing.Point(488, 291);
            this.txtMailingCity.MaxLength = 225;
            this.txtMailingCity.Multiline = true;
            this.txtMailingCity.Name = "txtMailingCity";
            this.txtMailingCity.Size = new System.Drawing.Size(113, 20);
            this.txtMailingCity.TabIndex = 127;
            this.txtMailingCity.WaterMark = "Enter City";
            this.txtMailingCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingAddress2
            // 
            this.txtMailingAddress2.Location = new System.Drawing.Point(488, 262);
            this.txtMailingAddress2.MaxLength = 225;
            this.txtMailingAddress2.Multiline = true;
            this.txtMailingAddress2.Name = "txtMailingAddress2";
            this.txtMailingAddress2.Size = new System.Drawing.Size(198, 20);
            this.txtMailingAddress2.TabIndex = 125;
            this.txtMailingAddress2.WaterMark = "Enter Address 2";
            this.txtMailingAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingAddress1
            // 
            this.txtMailingAddress1.Location = new System.Drawing.Point(488, 230);
            this.txtMailingAddress1.MaxLength = 225;
            this.txtMailingAddress1.Multiline = true;
            this.txtMailingAddress1.Name = "txtMailingAddress1";
            this.txtMailingAddress1.Size = new System.Drawing.Size(198, 20);
            this.txtMailingAddress1.TabIndex = 123;
            this.txtMailingAddress1.WaterMark = "Enter Address 1";
            this.txtMailingAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(488, 82);
            this.txtLastName.MaxLength = 200;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(267, 20);
            this.txtLastName.TabIndex = 101;
            this.txtLastName.WaterMark = "Enter  Last Name";
            this.txtLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(488, 138);
            this.txtEmail.MaxLength = 320;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(224, 20);
            this.txtEmail.TabIndex = 108;
            this.txtEmail.WaterMark = "Enter  Email";
            this.txtEmail.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtEmail.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(488, 53);
            this.txtFirstName.MaxLength = 200;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(267, 20);
            this.txtFirstName.TabIndex = 99;
            this.txtFirstName.WaterMark = "Enter First Name";
            this.txtFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Location = new System.Drawing.Point(118, 24);
            this.txtDepartmentName.MaxLength = 35;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Size = new System.Drawing.Size(198, 20);
            this.txtDepartmentName.TabIndex = 78;
            this.txtDepartmentName.WaterMark = "Enter Department Name";
            this.txtDepartmentName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDepartmentName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepartmentName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtSalesRepresentative
            // 
            this.txtSalesRepresentative.Location = new System.Drawing.Point(129, 508);
            this.txtSalesRepresentative.Name = "txtSalesRepresentative";
            this.txtSalesRepresentative.Size = new System.Drawing.Size(151, 20);
            this.txtSalesRepresentative.TabIndex = 138;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(64, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 82;
            this.label1.Text = "*";
            // 
            // lblLabCode
            // 
            this.lblLabCode.AutoSize = true;
            this.lblLabCode.Location = new System.Drawing.Point(11, 58);
            this.lblLabCode.Name = "lblLabCode";
            this.lblLabCode.Size = new System.Drawing.Size(53, 13);
            this.lblLabCode.TabIndex = 80;
            this.lblLabCode.Text = "&Lab Code";
            // 
            // lblQuestCode
            // 
            this.lblQuestCode.AutoSize = true;
            this.lblQuestCode.Location = new System.Drawing.Point(12, 85);
            this.lblQuestCode.Name = "lblQuestCode";
            this.lblQuestCode.Size = new System.Drawing.Size(66, 13);
            this.lblQuestCode.TabIndex = 81;
            this.lblQuestCode.Text = "&Quest Code:";
            // 
            // cmbSalesRepresentative
            // 
            this.cmbSalesRepresentative.DropDownHeight = 90;
            this.cmbSalesRepresentative.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesRepresentative.FormattingEnabled = true;
            this.cmbSalesRepresentative.IntegralHeight = false;
            this.cmbSalesRepresentative.Items.AddRange(new object[] {
            "(Select)"});
            this.cmbSalesRepresentative.Location = new System.Drawing.Point(129, 508);
            this.cmbSalesRepresentative.Name = "cmbSalesRepresentative";
            this.cmbSalesRepresentative.Size = new System.Drawing.Size(151, 21);
            this.cmbSalesRepresentative.TabIndex = 135;
            // 
            // lblSalesInfo
            // 
            this.lblSalesInfo.AutoSize = true;
            this.lblSalesInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesInfo.Location = new System.Drawing.Point(17, 479);
            this.lblSalesInfo.Name = "lblSalesInfo";
            this.lblSalesInfo.Size = new System.Drawing.Size(105, 13);
            this.lblSalesInfo.TabIndex = 133;
            this.lblSalesInfo.Text = "Sales Information";
            // 
            // lblSalesRepresentive
            // 
            this.lblSalesRepresentive.AutoSize = true;
            this.lblSalesRepresentive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesRepresentive.Location = new System.Drawing.Point(17, 512);
            this.lblSalesRepresentive.Name = "lblSalesRepresentive";
            this.lblSalesRepresentive.Size = new System.Drawing.Size(108, 13);
            this.lblSalesRepresentive.TabIndex = 134;
            this.lblSalesRepresentive.Text = "Sales Representative";
            // 
            // cmbPhysicalState
            // 
            this.cmbPhysicalState.DropDownHeight = 90;
            this.cmbPhysicalState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhysicalState.FormattingEnabled = true;
            this.cmbPhysicalState.IntegralHeight = false;
            this.cmbPhysicalState.Items.AddRange(new object[] {
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
            this.cmbPhysicalState.Location = new System.Drawing.Point(256, 417);
            this.cmbPhysicalState.Name = "cmbPhysicalState";
            this.cmbPhysicalState.Size = new System.Drawing.Size(110, 21);
            this.cmbPhysicalState.TabIndex = 117;
            // 
            // lblPhysicalZipCode
            // 
            this.lblPhysicalZipCode.AutoSize = true;
            this.lblPhysicalZipCode.Location = new System.Drawing.Point(17, 452);
            this.lblPhysicalZipCode.Name = "lblPhysicalZipCode";
            this.lblPhysicalZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblPhysicalZipCode.TabIndex = 118;
            this.lblPhysicalZipCode.Text = "Zip Code";
            // 
            // lblPhysicalState
            // 
            this.lblPhysicalState.AutoSize = true;
            this.lblPhysicalState.Location = new System.Drawing.Point(208, 421);
            this.lblPhysicalState.Name = "lblPhysicalState";
            this.lblPhysicalState.Size = new System.Drawing.Size(32, 13);
            this.lblPhysicalState.TabIndex = 116;
            this.lblPhysicalState.Text = "State";
            // 
            // lblPhysicalCity
            // 
            this.lblPhysicalCity.AutoSize = true;
            this.lblPhysicalCity.Location = new System.Drawing.Point(17, 421);
            this.lblPhysicalCity.Name = "lblPhysicalCity";
            this.lblPhysicalCity.Size = new System.Drawing.Size(24, 13);
            this.lblPhysicalCity.TabIndex = 114;
            this.lblPhysicalCity.Text = "City";
            // 
            // lblPhysicalAddress1
            // 
            this.lblPhysicalAddress1.AutoSize = true;
            this.lblPhysicalAddress1.Location = new System.Drawing.Point(17, 358);
            this.lblPhysicalAddress1.Name = "lblPhysicalAddress1";
            this.lblPhysicalAddress1.Size = new System.Drawing.Size(54, 13);
            this.lblPhysicalAddress1.TabIndex = 110;
            this.lblPhysicalAddress1.Text = "Address 1";
            // 
            // lblPhysicalAddress2
            // 
            this.lblPhysicalAddress2.AutoSize = true;
            this.lblPhysicalAddress2.Location = new System.Drawing.Point(17, 390);
            this.lblPhysicalAddress2.Name = "lblPhysicalAddress2";
            this.lblPhysicalAddress2.Size = new System.Drawing.Size(54, 13);
            this.lblPhysicalAddress2.TabIndex = 112;
            this.lblPhysicalAddress2.Text = "Address 2";
            // 
            // lblPhysicalAddress
            // 
            this.lblPhysicalAddress.AutoSize = true;
            this.lblPhysicalAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhysicalAddress.Location = new System.Drawing.Point(16, 325);
            this.lblPhysicalAddress.Name = "lblPhysicalAddress";
            this.lblPhysicalAddress.Size = new System.Drawing.Size(103, 13);
            this.lblPhysicalAddress.TabIndex = 109;
            this.lblPhysicalAddress.Text = "Physical Address";
            // 
            // cmbMailingState
            // 
            this.cmbMailingState.DropDownHeight = 90;
            this.cmbMailingState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMailingState.FormattingEnabled = true;
            this.cmbMailingState.IntegralHeight = false;
            this.cmbMailingState.Items.AddRange(new object[] {
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
            this.cmbMailingState.Location = new System.Drawing.Point(646, 291);
            this.cmbMailingState.Name = "cmbMailingState";
            this.cmbMailingState.Size = new System.Drawing.Size(110, 21);
            this.cmbMailingState.TabIndex = 129;
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(409, 357);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 131;
            this.lblZipCode.Text = "Zip Code";
            // 
            // chkSameAsClient
            // 
            this.chkSameAsClient.AutoSize = true;
            this.chkSameAsClient.Location = new System.Drawing.Point(123, 295);
            this.chkSameAsClient.Name = "chkSameAsClient";
            this.chkSameAsClient.Size = new System.Drawing.Size(157, 17);
            this.chkSameAsClient.TabIndex = 96;
            this.chkSameAsClient.Text = "Contact Info Same as Client";
            this.chkSameAsClient.UseVisualStyleBackColor = true;
            // 
            // chkSameAsPhysical
            // 
            this.chkSameAsPhysical.AutoSize = true;
            this.chkSameAsPhysical.Location = new System.Drawing.Point(522, 197);
            this.chkSameAsPhysical.Name = "chkSameAsPhysical";
            this.chkSameAsPhysical.Size = new System.Drawing.Size(150, 17);
            this.chkSameAsPhysical.TabIndex = 121;
            this.chkSameAsPhysical.Text = "Same as Physical Address";
            this.chkSameAsPhysical.UseVisualStyleBackColor = true;
            // 
            // lblMailingState
            // 
            this.lblMailingState.AutoSize = true;
            this.lblMailingState.Location = new System.Drawing.Point(604, 295);
            this.lblMailingState.Name = "lblMailingState";
            this.lblMailingState.Size = new System.Drawing.Size(32, 13);
            this.lblMailingState.TabIndex = 128;
            this.lblMailingState.Text = "State";
            // 
            // lblMailingCity
            // 
            this.lblMailingCity.AutoSize = true;
            this.lblMailingCity.Location = new System.Drawing.Point(406, 295);
            this.lblMailingCity.Name = "lblMailingCity";
            this.lblMailingCity.Size = new System.Drawing.Size(24, 13);
            this.lblMailingCity.TabIndex = 126;
            this.lblMailingCity.Text = "City";
            // 
            // lblMailingAddress2
            // 
            this.lblMailingAddress2.AutoSize = true;
            this.lblMailingAddress2.Location = new System.Drawing.Point(406, 266);
            this.lblMailingAddress2.Name = "lblMailingAddress2";
            this.lblMailingAddress2.Size = new System.Drawing.Size(54, 13);
            this.lblMailingAddress2.TabIndex = 124;
            this.lblMailingAddress2.Text = "Address 2";
            // 
            // lblMailingAddress1
            // 
            this.lblMailingAddress1.AutoSize = true;
            this.lblMailingAddress1.Location = new System.Drawing.Point(406, 234);
            this.lblMailingAddress1.Name = "lblMailingAddress1";
            this.lblMailingAddress1.Size = new System.Drawing.Size(54, 13);
            this.lblMailingAddress1.TabIndex = 122;
            this.lblMailingAddress1.Text = "Address 1";
            // 
            // lblMailingAddress
            // 
            this.lblMailingAddress.AutoSize = true;
            this.lblMailingAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMailingAddress.Location = new System.Drawing.Point(407, 199);
            this.lblMailingAddress.Name = "lblMailingAddress";
            this.lblMailingAddress.Size = new System.Drawing.Size(96, 13);
            this.lblMailingAddress.TabIndex = 120;
            this.lblMailingAddress.Text = "Mailing Address";
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(656, 112);
            this.txtFax.Mask = "(999) 000-0000";
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(99, 20);
            this.txtFax.TabIndex = 106;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(488, 111);
            this.txtPhone.Mask = "(999) 000-0000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(120, 20);
            this.txtPhone.TabIndex = 104;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(406, 142);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 107;
            this.lblEmail.Text = "Email";
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(618, 116);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(24, 13);
            this.lblFax.TabIndex = 105;
            this.lblFax.Text = "Fax";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(406, 115);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(38, 13);
            this.lblPhone.TabIndex = 102;
            this.lblPhone.Text = "Phone";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(406, 86);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 100;
            this.lblLastName.Text = "Last Name";
            // 
            // lblMainContactInfo
            // 
            this.lblMainContactInfo.AutoSize = true;
            this.lblMainContactInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainContactInfo.Location = new System.Drawing.Point(406, 28);
            this.lblMainContactInfo.Name = "lblMainContactInfo";
            this.lblMainContactInfo.Size = new System.Drawing.Size(149, 13);
            this.lblMainContactInfo.TabIndex = 97;
            this.lblMainContactInfo.Text = "Main Contact Information";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.Location = new System.Drawing.Point(406, 57);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(60, 13);
            this.lblFirstName.TabIndex = 98;
            this.lblFirstName.Text = "First Name ";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(319, 26);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 79;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // lblPaymentMan
            // 
            this.lblPaymentMan.AutoSize = true;
            this.lblPaymentMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMan.ForeColor = System.Drawing.Color.Red;
            this.lblPaymentMan.Location = new System.Drawing.Point(90, 262);
            this.lblPaymentMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPaymentMan.Name = "lblPaymentMan";
            this.lblPaymentMan.Size = new System.Drawing.Size(13, 17);
            this.lblPaymentMan.TabIndex = 94;
            this.lblPaymentMan.Text = "*";
            // 
            // pnlPaymentType
            // 
            this.pnlPaymentType.Controls.Add(this.rbInvoiceClient);
            this.pnlPaymentType.Controls.Add(this.rbDonorPays);
            this.pnlPaymentType.Location = new System.Drawing.Point(120, 258);
            this.pnlPaymentType.Name = "pnlPaymentType";
            this.pnlPaymentType.Size = new System.Drawing.Size(216, 25);
            this.pnlPaymentType.TabIndex = 95;
            // 
            // rbInvoiceClient
            // 
            this.rbInvoiceClient.AutoSize = true;
            this.rbInvoiceClient.Location = new System.Drawing.Point(93, 3);
            this.rbInvoiceClient.Name = "rbInvoiceClient";
            this.rbInvoiceClient.Size = new System.Drawing.Size(89, 17);
            this.rbInvoiceClient.TabIndex = 1;
            this.rbInvoiceClient.Text = "Invoice Client";
            this.rbInvoiceClient.UseVisualStyleBackColor = true;
            // 
            // rbDonorPays
            // 
            this.rbDonorPays.AutoSize = true;
            this.rbDonorPays.Checked = true;
            this.rbDonorPays.Location = new System.Drawing.Point(3, 3);
            this.rbDonorPays.Name = "rbDonorPays";
            this.rbDonorPays.Size = new System.Drawing.Size(80, 17);
            this.rbDonorPays.TabIndex = 0;
            this.rbDonorPays.TabStop = true;
            this.rbDonorPays.Text = "Donor Pays";
            this.rbDonorPays.UseVisualStyleBackColor = true;
            // 
            // lblPaymentType
            // 
            this.lblPaymentType.AutoSize = true;
            this.lblPaymentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentType.Location = new System.Drawing.Point(18, 264);
            this.lblPaymentType.Name = "lblPaymentType";
            this.lblPaymentType.Size = new System.Drawing.Size(75, 13);
            this.lblPaymentType.TabIndex = 93;
            this.lblPaymentType.Text = "Payment Type";
            // 
            // lblMROMan
            // 
            this.lblMROMan.AutoSize = true;
            this.lblMROMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMROMan.ForeColor = System.Drawing.Color.Red;
            this.lblMROMan.Location = new System.Drawing.Point(77, 225);
            this.lblMROMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMROMan.Name = "lblMROMan";
            this.lblMROMan.Size = new System.Drawing.Size(13, 17);
            this.lblMROMan.TabIndex = 91;
            this.lblMROMan.Text = "*";
            // 
            // lblCategoryMan
            // 
            this.lblCategoryMan.AutoSize = true;
            this.lblCategoryMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryMan.ForeColor = System.Drawing.Color.Red;
            this.lblCategoryMan.Location = new System.Drawing.Point(65, 170);
            this.lblCategoryMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCategoryMan.Name = "lblCategoryMan";
            this.lblCategoryMan.Size = new System.Drawing.Size(13, 17);
            this.lblCategoryMan.TabIndex = 86;
            this.lblCategoryMan.Text = "*";
            // 
            // lblDepartmentNameMan
            // 
            this.lblDepartmentNameMan.AutoSize = true;
            this.lblDepartmentNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblDepartmentNameMan.Location = new System.Drawing.Point(103, 26);
            this.lblDepartmentNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDepartmentNameMan.Name = "lblDepartmentNameMan";
            this.lblDepartmentNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblDepartmentNameMan.TabIndex = 77;
            this.lblDepartmentNameMan.Text = "*";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(109, 582);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 137;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(28, 582);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 136;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlMROType
            // 
            this.pnlMROType.Controls.Add(this.rbMALL);
            this.pnlMROType.Controls.Add(this.rbMPOS);
            this.pnlMROType.Location = new System.Drawing.Point(120, 221);
            this.pnlMROType.Name = "pnlMROType";
            this.pnlMROType.Size = new System.Drawing.Size(140, 25);
            this.pnlMROType.TabIndex = 92;
            // 
            // rbMALL
            // 
            this.rbMALL.AutoSize = true;
            this.rbMALL.Location = new System.Drawing.Point(72, 3);
            this.rbMALL.Name = "rbMALL";
            this.rbMALL.Size = new System.Drawing.Size(53, 17);
            this.rbMALL.TabIndex = 1;
            this.rbMALL.Text = "&MALL";
            this.rbMALL.UseVisualStyleBackColor = true;
            // 
            // rbMPOS
            // 
            this.rbMPOS.AutoSize = true;
            this.rbMPOS.Checked = true;
            this.rbMPOS.Location = new System.Drawing.Point(3, 3);
            this.rbMPOS.Name = "rbMPOS";
            this.rbMPOS.Size = new System.Drawing.Size(56, 17);
            this.rbMPOS.TabIndex = 0;
            this.rbMPOS.TabStop = true;
            this.rbMPOS.Text = "M&POS";
            this.rbMPOS.UseVisualStyleBackColor = true;
            // 
            // lblMROType
            // 
            this.lblMROType.AutoSize = true;
            this.lblMROType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMROType.Location = new System.Drawing.Point(18, 227);
            this.lblMROType.Name = "lblMROType";
            this.lblMROType.Size = new System.Drawing.Size(59, 13);
            this.lblMROType.TabIndex = 90;
            this.lblMROType.Text = "M&RO Type";
            // 
            // chkDNA
            // 
            this.chkDNA.AutoSize = true;
            this.chkDNA.Location = new System.Drawing.Point(270, 170);
            this.chkDNA.Name = "chkDNA";
            this.chkDNA.Size = new System.Drawing.Size(49, 17);
            this.chkDNA.TabIndex = 89;
            this.chkDNA.Text = "&DNA";
            this.chkDNA.UseVisualStyleBackColor = true;
            // 
            // chkHair
            // 
            this.chkHair.AutoSize = true;
            this.chkHair.Location = new System.Drawing.Point(172, 170);
            this.chkHair.Name = "chkHair";
            this.chkHair.Size = new System.Drawing.Size(45, 17);
            this.chkHair.TabIndex = 88;
            this.chkHair.Text = "&Hair";
            this.chkHair.UseVisualStyleBackColor = true;
            // 
            // chkUA
            // 
            this.chkUA.AutoSize = true;
            this.chkUA.Checked = true;
            this.chkUA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUA.Location = new System.Drawing.Point(123, 170);
            this.chkUA.Name = "chkUA";
            this.chkUA.Size = new System.Drawing.Size(41, 17);
            this.chkUA.TabIndex = 87;
            this.chkUA.Text = "&UA";
            this.chkUA.UseVisualStyleBackColor = true;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(18, 172);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.TabIndex = 85;
            this.lblCategory.Text = "&Category";
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.AutoSize = true;
            this.lblDepartmentName.Location = new System.Drawing.Point(13, 28);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(93, 13);
            this.lblDepartmentName.TabIndex = 76;
            this.lblDepartmentName.Text = "&Department Name";
            // 
            // tabDeptDetailsTabs
            // 
            this.tabDeptDetailsTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDeptDetailsTabs.Controls.Add(this.DepartmentInfo);
            this.tabDeptDetailsTabs.Controls.Add(this.tabRecordKeeping);
            this.tabDeptDetailsTabs.Controls.Add(this.tabNotificationSettingsClientDepartmentDetails);
            this.tabDeptDetailsTabs.Location = new System.Drawing.Point(9, 35);
            this.tabDeptDetailsTabs.Margin = new System.Windows.Forms.Padding(2);
            this.tabDeptDetailsTabs.Name = "tabDeptDetailsTabs";
            this.tabDeptDetailsTabs.SelectedIndex = 0;
            this.tabDeptDetailsTabs.Size = new System.Drawing.Size(1276, 891);
            this.tabDeptDetailsTabs.TabIndex = 76;
            this.tabDeptDetailsTabs.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabNotificationSettingsClientDepartmentDetails
            // 
            this.tabNotificationSettingsClientDepartmentDetails.AutoScroll = true;
            this.tabNotificationSettingsClientDepartmentDetails.BackColor = System.Drawing.Color.Silver;
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.groupBoxRandomizationSettings);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.groupBox7);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.groupBoxTools);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.gboxAdvanced);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.btnResetNotification);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.btnSaveNotification);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.groupBox4);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.groupNotifcationDaySettings);
            this.tabNotificationSettingsClientDepartmentDetails.Controls.Add(this.btnAdvanced);
            this.tabNotificationSettingsClientDepartmentDetails.Location = new System.Drawing.Point(4, 22);
            this.tabNotificationSettingsClientDepartmentDetails.Margin = new System.Windows.Forms.Padding(2);
            this.tabNotificationSettingsClientDepartmentDetails.Name = "tabNotificationSettingsClientDepartmentDetails";
            this.tabNotificationSettingsClientDepartmentDetails.Padding = new System.Windows.Forms.Padding(2);
            this.tabNotificationSettingsClientDepartmentDetails.Size = new System.Drawing.Size(1268, 865);
            this.tabNotificationSettingsClientDepartmentDetails.TabIndex = 2;
            this.tabNotificationSettingsClientDepartmentDetails.Text = "Notification Settings";
            this.tabNotificationSettingsClientDepartmentDetails.Click += new System.EventHandler(this.tabPage1_Click_1);
            // 
            // groupBoxRandomizationSettings
            // 
            this.groupBoxRandomizationSettings.Controls.Add(this.nudMaxSendIns);
            this.groupBoxRandomizationSettings.Controls.Add(this.label18);
            this.groupBoxRandomizationSettings.Controls.Add(this.nudDeadlineAlert);
            this.groupBoxRandomizationSettings.Controls.Add(this.label25);
            this.groupBoxRandomizationSettings.Controls.Add(this.label24);
            this.groupBoxRandomizationSettings.Controls.Add(this.dtSweepDate);
            this.groupBoxRandomizationSettings.Controls.Add(this.label23);
            this.groupBoxRandomizationSettings.Controls.Add(this.chkForceManualNotificaitons);
            this.groupBoxRandomizationSettings.Controls.Add(this.dtSendInStart);
            this.groupBoxRandomizationSettings.Controls.Add(this.label21);
            this.groupBoxRandomizationSettings.Controls.Add(this.dtSendInStop);
            this.groupBoxRandomizationSettings.Location = new System.Drawing.Point(427, 5);
            this.groupBoxRandomizationSettings.Name = "groupBoxRandomizationSettings";
            this.groupBoxRandomizationSettings.Size = new System.Drawing.Size(407, 273);
            this.groupBoxRandomizationSettings.TabIndex = 153;
            this.groupBoxRandomizationSettings.TabStop = false;
            this.groupBoxRandomizationSettings.Text = "Randomization Settings";
            // 
            // nudMaxSendIns
            // 
            this.nudMaxSendIns.Location = new System.Drawing.Point(194, 217);
            this.nudMaxSendIns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxSendIns.Name = "nudMaxSendIns";
            this.nudMaxSendIns.Size = new System.Drawing.Size(90, 20);
            this.nudMaxSendIns.TabIndex = 14;
            this.nudMaxSendIns.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(13, 219);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(94, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Max Daily Sendins";
            // 
            // nudDeadlineAlert
            // 
            this.nudDeadlineAlert.Location = new System.Drawing.Point(194, 191);
            this.nudDeadlineAlert.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudDeadlineAlert.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDeadlineAlert.Name = "nudDeadlineAlert";
            this.nudDeadlineAlert.Size = new System.Drawing.Size(90, 20);
            this.nudDeadlineAlert.TabIndex = 12;
            this.nudDeadlineAlert.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(13, 193);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(143, 13);
            this.label25.TabIndex = 11;
            this.label25.Text = "Donor Deadline Alert in Days";
            this.label25.Click += new System.EventHandler(this.label25_Click);
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(13, 51);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(175, 53);
            this.label24.TabIndex = 7;
            this.label24.Text = "Registration Sweep Date (Registrations after this date will be scheduled for send" +
    " in between dates below)";
            // 
            // dtSweepDate
            // 
            this.dtSweepDate.Enabled = false;
            this.dtSweepDate.Location = new System.Drawing.Point(194, 51);
            this.dtSweepDate.Name = "dtSweepDate";
            this.dtSweepDate.Size = new System.Drawing.Size(200, 20);
            this.dtSweepDate.TabIndex = 8;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(13, 125);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(144, 13);
            this.label23.TabIndex = 5;
            this.label23.Text = "Start Send-Ins after this date:";
            // 
            // chkForceManualNotificaitons
            // 
            this.chkForceManualNotificaitons.AutoSize = true;
            this.chkForceManualNotificaitons.Location = new System.Drawing.Point(18, 22);
            this.chkForceManualNotificaitons.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkForceManualNotificaitons.Name = "chkForceManualNotificaitons";
            this.chkForceManualNotificaitons.Size = new System.Drawing.Size(294, 17);
            this.chkForceManualNotificaitons.TabIndex = 0;
            this.chkForceManualNotificaitons.Text = "Force Manual Notification (Disable automatic notification)";
            this.chkForceManualNotificaitons.UseVisualStyleBackColor = true;
            this.chkForceManualNotificaitons.CheckedChanged += new System.EventHandler(this.chkForceManualNotificaitons_CheckedChanged);
            // 
            // dtSendInStart
            // 
            this.dtSendInStart.Enabled = false;
            this.dtSendInStart.Location = new System.Drawing.Point(194, 122);
            this.dtSendInStart.Name = "dtSendInStart";
            this.dtSendInStart.Size = new System.Drawing.Size(200, 20);
            this.dtSendInStart.TabIndex = 6;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(13, 152);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(141, 13);
            this.label21.TabIndex = 3;
            this.label21.Text = "Halt Send-Ins after this date:";
            // 
            // dtSendInStop
            // 
            this.dtSendInStop.Enabled = false;
            this.dtSendInStop.Location = new System.Drawing.Point(194, 149);
            this.dtSendInStop.Name = "dtSendInStop";
            this.dtSendInStop.Size = new System.Drawing.Size(200, 20);
            this.dtSendInStop.TabIndex = 4;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.txtSMSReply);
            this.groupBox7.Location = new System.Drawing.Point(14, 181);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox7.Size = new System.Drawing.Size(407, 97);
            this.groupBox7.TabIndex = 152;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "SMS Reply (140 Characters or less)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(320, 0);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(78, 13);
            this.label20.TabIndex = 1;
            this.label20.Text = "140 Remaining";
            this.label20.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSMSReply
            // 
            this.txtSMSReply.Location = new System.Drawing.Point(10, 17);
            this.txtSMSReply.Margin = new System.Windows.Forms.Padding(2);
            this.txtSMSReply.MaxLength = 140;
            this.txtSMSReply.Multiline = true;
            this.txtSMSReply.Name = "txtSMSReply";
            this.txtSMSReply.Size = new System.Drawing.Size(388, 69);
            this.txtSMSReply.TabIndex = 0;
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.groupBox2);
            this.groupBoxTools.Controls.Add(this.btnLoadDefaults);
            this.groupBoxTools.Controls.Add(this.btnCopySettings);
            this.groupBoxTools.Controls.Add(this.label26);
            this.groupBoxTools.Controls.Add(this.cmboCopyWindowSettings);
            this.groupBoxTools.Location = new System.Drawing.Point(427, 283);
            this.groupBoxTools.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxTools.Size = new System.Drawing.Size(407, 199);
            this.groupBoxTools.TabIndex = 151;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "Tools";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.txtPreviewPhone);
            this.groupBox2.Controls.Add(this.txtPreviewEmail);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtPreviewZipCode);
            this.groupBox2.Controls.Add(this.btnEmailPreview);
            this.groupBox2.Location = new System.Drawing.Point(18, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(358, 116);
            this.groupBox2.TabIndex = 164;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview These Settings";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(28, 40);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(38, 13);
            this.label28.TabIndex = 170;
            this.label28.Text = "Phone";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(28, 17);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(32, 13);
            this.label27.TabIndex = 169;
            this.label27.Text = "Email";
            // 
            // txtPreviewPhone
            // 
            this.txtPreviewPhone.Location = new System.Drawing.Point(117, 37);
            this.txtPreviewPhone.Name = "txtPreviewPhone";
            this.txtPreviewPhone.Size = new System.Drawing.Size(195, 20);
            this.txtPreviewPhone.TabIndex = 168;
            // 
            // txtPreviewEmail
            // 
            this.txtPreviewEmail.Location = new System.Drawing.Point(117, 14);
            this.txtPreviewEmail.Name = "txtPreviewEmail";
            this.txtPreviewEmail.Size = new System.Drawing.Size(195, 20);
            this.txtPreviewEmail.TabIndex = 167;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 65);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 13);
            this.label13.TabIndex = 166;
            this.label13.Text = "For zip code:";
            // 
            // txtPreviewZipCode
            // 
            this.txtPreviewZipCode.Location = new System.Drawing.Point(117, 62);
            this.txtPreviewZipCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtPreviewZipCode.Name = "txtPreviewZipCode";
            this.txtPreviewZipCode.Size = new System.Drawing.Size(66, 20);
            this.txtPreviewZipCode.TabIndex = 165;
            // 
            // btnEmailPreview
            // 
            this.btnEmailPreview.Location = new System.Drawing.Point(117, 88);
            this.btnEmailPreview.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmailPreview.Name = "btnEmailPreview";
            this.btnEmailPreview.Size = new System.Drawing.Size(108, 23);
            this.btnEmailPreview.TabIndex = 164;
            this.btnEmailPreview.Text = "Email Preview";
            this.btnEmailPreview.UseVisualStyleBackColor = true;
            this.btnEmailPreview.Click += new System.EventHandler(this.btnEmailPreview_Click);
            // 
            // btnLoadDefaults
            // 
            this.btnLoadDefaults.Location = new System.Drawing.Point(237, 45);
            this.btnLoadDefaults.Name = "btnLoadDefaults";
            this.btnLoadDefaults.Size = new System.Drawing.Size(139, 23);
            this.btnLoadDefaults.TabIndex = 158;
            this.btnLoadDefaults.Text = "Load Default Settings";
            this.btnLoadDefaults.UseVisualStyleBackColor = true;
            this.btnLoadDefaults.Click += new System.EventHandler(this.btnLoadDefaults_Click);
            // 
            // btnCopySettings
            // 
            this.btnCopySettings.Location = new System.Drawing.Point(319, 18);
            this.btnCopySettings.Name = "btnCopySettings";
            this.btnCopySettings.Size = new System.Drawing.Size(57, 21);
            this.btnCopySettings.TabIndex = 157;
            this.btnCopySettings.Text = "Go";
            this.btnCopySettings.UseVisualStyleBackColor = true;
            this.btnCopySettings.Click += new System.EventHandler(this.button1_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(15, 20);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(98, 13);
            this.label26.TabIndex = 156;
            this.label26.Text = "Copy Settings from:";
            // 
            // cmboCopyWindowSettings
            // 
            this.cmboCopyWindowSettings.FormattingEnabled = true;
            this.cmboCopyWindowSettings.Location = new System.Drawing.Point(121, 19);
            this.cmboCopyWindowSettings.Name = "cmboCopyWindowSettings";
            this.cmboCopyWindowSettings.Size = new System.Drawing.Size(195, 21);
            this.cmboCopyWindowSettings.TabIndex = 155;
            // 
            // gboxAdvanced
            // 
            this.gboxAdvanced.Controls.Add(this.txtGlobalMROName);
            this.gboxAdvanced.Controls.Add(this.label19);
            this.gboxAdvanced.Controls.Add(this.txtFilenamePDFTemplate);
            this.gboxAdvanced.Controls.Add(this.label22);
            this.gboxAdvanced.Controls.Add(this.txtFilenameRenderSettings);
            this.gboxAdvanced.Controls.Add(this.label17);
            this.gboxAdvanced.Controls.Add(this.btnResetJSON);
            this.gboxAdvanced.Controls.Add(this.txtJSONTemplateSettings);
            this.gboxAdvanced.Location = new System.Drawing.Point(848, 5);
            this.gboxAdvanced.Margin = new System.Windows.Forms.Padding(2);
            this.gboxAdvanced.Name = "gboxAdvanced";
            this.gboxAdvanced.Padding = new System.Windows.Forms.Padding(2);
            this.gboxAdvanced.Size = new System.Drawing.Size(397, 477);
            this.gboxAdvanced.TabIndex = 150;
            this.gboxAdvanced.TabStop = false;
            this.gboxAdvanced.Text = "Notification Document Settings";
            this.gboxAdvanced.Visible = false;
            this.gboxAdvanced.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // txtGlobalMROName
            // 
            this.txtGlobalMROName.Location = new System.Drawing.Point(128, 442);
            this.txtGlobalMROName.Margin = new System.Windows.Forms.Padding(2);
            this.txtGlobalMROName.Name = "txtGlobalMROName";
            this.txtGlobalMROName.Size = new System.Drawing.Size(265, 20);
            this.txtGlobalMROName.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 445);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(102, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "MRO Name (Global)";
            // 
            // txtFilenamePDFTemplate
            // 
            this.txtFilenamePDFTemplate.Enabled = false;
            this.txtFilenamePDFTemplate.Location = new System.Drawing.Point(152, 22);
            this.txtFilenamePDFTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.txtFilenamePDFTemplate.Name = "txtFilenamePDFTemplate";
            this.txtFilenamePDFTemplate.Size = new System.Drawing.Size(239, 20);
            this.txtFilenamePDFTemplate.TabIndex = 155;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Enabled = false;
            this.label22.Location = new System.Drawing.Point(8, 24);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(123, 13);
            this.label22.TabIndex = 156;
            this.label22.Text = "PDF Template Filename:";
            // 
            // txtFilenameRenderSettings
            // 
            this.txtFilenameRenderSettings.Location = new System.Drawing.Point(152, 49);
            this.txtFilenameRenderSettings.Margin = new System.Windows.Forms.Padding(2);
            this.txtFilenameRenderSettings.Name = "txtFilenameRenderSettings";
            this.txtFilenameRenderSettings.Size = new System.Drawing.Size(239, 20);
            this.txtFilenameRenderSettings.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 51);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(131, 13);
            this.label17.TabIndex = 154;
            this.label17.Text = "Render Settings Filename:";
            // 
            // btnResetJSON
            // 
            this.btnResetJSON.Location = new System.Drawing.Point(283, 405);
            this.btnResetJSON.Margin = new System.Windows.Forms.Padding(2);
            this.btnResetJSON.Name = "btnResetJSON";
            this.btnResetJSON.Size = new System.Drawing.Size(108, 23);
            this.btnResetJSON.TabIndex = 2;
            this.btnResetJSON.Text = "Clear / Reset";
            this.btnResetJSON.UseVisualStyleBackColor = true;
            this.btnResetJSON.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtJSONTemplateSettings
            // 
            this.txtJSONTemplateSettings.AcceptsReturn = true;
            this.txtJSONTemplateSettings.AcceptsTab = true;
            this.txtJSONTemplateSettings.Location = new System.Drawing.Point(11, 85);
            this.txtJSONTemplateSettings.Margin = new System.Windows.Forms.Padding(2);
            this.txtJSONTemplateSettings.Multiline = true;
            this.txtJSONTemplateSettings.Name = "txtJSONTemplateSettings";
            this.txtJSONTemplateSettings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJSONTemplateSettings.Size = new System.Drawing.Size(382, 306);
            this.txtJSONTemplateSettings.TabIndex = 1;
            this.txtJSONTemplateSettings.Text = "{}";
            this.txtJSONTemplateSettings.WordWrap = false;
            this.txtJSONTemplateSettings.Leave += new System.EventHandler(this.txtJSONTemplateSettings_Leave);
            // 
            // btnResetNotification
            // 
            this.btnResetNotification.Location = new System.Drawing.Point(440, 499);
            this.btnResetNotification.Margin = new System.Windows.Forms.Padding(2);
            this.btnResetNotification.Name = "btnResetNotification";
            this.btnResetNotification.Size = new System.Drawing.Size(75, 23);
            this.btnResetNotification.TabIndex = 1;
            this.btnResetNotification.Text = "Reset";
            this.btnResetNotification.UseVisualStyleBackColor = true;
            this.btnResetNotification.Click += new System.EventHandler(this.btnResetNotification_Click);
            // 
            // btnSaveNotification
            // 
            this.btnSaveNotification.Location = new System.Drawing.Point(346, 499);
            this.btnSaveNotification.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveNotification.Name = "btnSaveNotification";
            this.btnSaveNotification.Size = new System.Drawing.Size(75, 23);
            this.btnSaveNotification.TabIndex = 0;
            this.btnSaveNotification.Text = "Save";
            this.btnSaveNotification.UseVisualStyleBackColor = true;
            this.btnSaveNotification.Visible = false;
            this.btnSaveNotification.Click += new System.EventHandler(this.btnSaveNotification_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkUseFormFox);
            this.groupBox4.Controls.Add(this.ckbWebShowNotifyButton);
            this.groupBox4.Controls.Add(this.chkEnableSMS);
            this.groupBox4.Controls.Add(this.chkOnsiteTesting);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(14, 4);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox4.Size = new System.Drawing.Size(407, 166);
            this.groupBox4.TabIndex = 147;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Departmental Notification Options";
            // 
            // chkUseFormFox
            // 
            this.chkUseFormFox.AutoSize = true;
            this.chkUseFormFox.Location = new System.Drawing.Point(305, 100);
            this.chkUseFormFox.Name = "chkUseFormFox";
            this.chkUseFormFox.Size = new System.Drawing.Size(88, 17);
            this.chkUseFormFox.TabIndex = 31;
            this.chkUseFormFox.Text = "Use FormFox";
            this.chkUseFormFox.UseVisualStyleBackColor = true;
            this.chkUseFormFox.CheckedChanged += new System.EventHandler(this.chkUseFormFox_CheckedChanged);
            // 
            // ckbWebShowNotifyButton
            // 
            this.ckbWebShowNotifyButton.AutoSize = true;
            this.ckbWebShowNotifyButton.Location = new System.Drawing.Point(12, 143);
            this.ckbWebShowNotifyButton.Name = "ckbWebShowNotifyButton";
            this.ckbWebShowNotifyButton.Size = new System.Drawing.Size(168, 17);
            this.ckbWebShowNotifyButton.TabIndex = 30;
            this.ckbWebShowNotifyButton.Text = "Show Send In Button on Web";
            this.ckbWebShowNotifyButton.UseVisualStyleBackColor = true;
            // 
            // chkEnableSMS
            // 
            this.chkEnableSMS.AutoSize = true;
            this.chkEnableSMS.Location = new System.Drawing.Point(12, 122);
            this.chkEnableSMS.Name = "chkEnableSMS";
            this.chkEnableSMS.Size = new System.Drawing.Size(146, 17);
            this.chkEnableSMS.TabIndex = 2;
            this.chkEnableSMS.Text = "Enable SMS Notifications";
            this.chkEnableSMS.UseVisualStyleBackColor = true;
            // 
            // chkOnsiteTesting
            // 
            this.chkOnsiteTesting.AutoSize = true;
            this.chkOnsiteTesting.Location = new System.Drawing.Point(12, 100);
            this.chkOnsiteTesting.Name = "chkOnsiteTesting";
            this.chkOnsiteTesting.Size = new System.Drawing.Size(94, 17);
            this.chkOnsiteTesting.TabIndex = 1;
            this.chkOnsiteTesting.Text = "Onsite Testing";
            this.chkOnsiteTesting.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numDelayHours);
            this.groupBox5.Controls.Add(this.radASAPorDelay2);
            this.groupBox5.Controls.Add(this.radASAPorDelay1);
            this.groupBox5.Location = new System.Drawing.Point(12, 19);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox5.Size = new System.Drawing.Size(386, 75);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            // 
            // numDelayHours
            // 
            this.numDelayHours.Location = new System.Drawing.Point(184, 39);
            this.numDelayHours.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.numDelayHours.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numDelayHours.Name = "numDelayHours";
            this.numDelayHours.Size = new System.Drawing.Size(90, 20);
            this.numDelayHours.TabIndex = 2;
            // 
            // radASAPorDelay2
            // 
            this.radASAPorDelay2.AutoSize = true;
            this.radASAPorDelay2.Location = new System.Drawing.Point(4, 39);
            this.radASAPorDelay2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radASAPorDelay2.Name = "radASAPorDelay2";
            this.radASAPorDelay2.Size = new System.Drawing.Size(177, 17);
            this.radASAPorDelay2.TabIndex = 1;
            this.radASAPorDelay2.Text = "Sample Required within hours ->";
            this.radASAPorDelay2.UseVisualStyleBackColor = true;
            // 
            // radASAPorDelay1
            // 
            this.radASAPorDelay1.AutoSize = true;
            this.radASAPorDelay1.Checked = true;
            this.radASAPorDelay1.Location = new System.Drawing.Point(4, 17);
            this.radASAPorDelay1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radASAPorDelay1.Name = "radASAPorDelay1";
            this.radASAPorDelay1.Size = new System.Drawing.Size(137, 17);
            this.radASAPorDelay1.TabIndex = 0;
            this.radASAPorDelay1.TabStop = true;
            this.radASAPorDelay1.Text = "Sample Required ASAP";
            this.radASAPorDelay1.UseVisualStyleBackColor = true;
            this.radASAPorDelay1.CheckedChanged += new System.EventHandler(this.radASAPorDelay_CheckedChanged);
            // 
            // groupNotifcationDaySettings
            // 
            this.groupNotifcationDaySettings.Controls.Add(this.dtSaturdayEnd);
            this.groupNotifcationDaySettings.Controls.Add(this.dtWednesdayEnd);
            this.groupNotifcationDaySettings.Controls.Add(this.dtSaturdayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.chkSunday);
            this.groupNotifcationDaySettings.Controls.Add(this.dtFridayEnd);
            this.groupNotifcationDaySettings.Controls.Add(this.chkSaturday);
            this.groupNotifcationDaySettings.Controls.Add(this.dtFridayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.chkMonday);
            this.groupNotifcationDaySettings.Controls.Add(this.dtThursdayEnd);
            this.groupNotifcationDaySettings.Controls.Add(this.chkTuesday);
            this.groupNotifcationDaySettings.Controls.Add(this.dtThursdayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.chkWednesday);
            this.groupNotifcationDaySettings.Controls.Add(this.chkThursday);
            this.groupNotifcationDaySettings.Controls.Add(this.dtWednesdayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.chkFriday);
            this.groupNotifcationDaySettings.Controls.Add(this.dtTuesdayEnd);
            this.groupNotifcationDaySettings.Controls.Add(this.dtSundayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.dtTuesdayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.label14);
            this.groupNotifcationDaySettings.Controls.Add(this.dtMondayEnd);
            this.groupNotifcationDaySettings.Controls.Add(this.label15);
            this.groupNotifcationDaySettings.Controls.Add(this.dtMondayStart);
            this.groupNotifcationDaySettings.Controls.Add(this.label16);
            this.groupNotifcationDaySettings.Controls.Add(this.dtSundayEnd);
            this.groupNotifcationDaySettings.Location = new System.Drawing.Point(14, 283);
            this.groupNotifcationDaySettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupNotifcationDaySettings.Name = "groupNotifcationDaySettings";
            this.groupNotifcationDaySettings.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupNotifcationDaySettings.Size = new System.Drawing.Size(407, 198);
            this.groupNotifcationDaySettings.TabIndex = 146;
            this.groupNotifcationDaySettings.TabStop = false;
            this.groupNotifcationDaySettings.Text = "Notification Engine Window Settings";
            // 
            // dtSaturdayEnd
            // 
            this.dtSaturdayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtSaturdayEnd.Location = new System.Drawing.Point(277, 170);
            this.dtSaturdayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtSaturdayEnd.Name = "dtSaturdayEnd";
            this.dtSaturdayEnd.ShowUpDown = true;
            this.dtSaturdayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtSaturdayEnd.TabIndex = 20;
            // 
            // dtWednesdayEnd
            // 
            this.dtWednesdayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtWednesdayEnd.Location = new System.Drawing.Point(277, 102);
            this.dtWednesdayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtWednesdayEnd.Name = "dtWednesdayEnd";
            this.dtWednesdayEnd.ShowUpDown = true;
            this.dtWednesdayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtWednesdayEnd.TabIndex = 11;
            // 
            // dtSaturdayStart
            // 
            this.dtSaturdayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtSaturdayStart.Location = new System.Drawing.Point(116, 171);
            this.dtSaturdayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtSaturdayStart.Name = "dtSaturdayStart";
            this.dtSaturdayStart.ShowUpDown = true;
            this.dtSaturdayStart.Size = new System.Drawing.Size(110, 20);
            this.dtSaturdayStart.TabIndex = 19;
            // 
            // chkSunday
            // 
            this.chkSunday.AutoSize = true;
            this.chkSunday.Location = new System.Drawing.Point(6, 34);
            this.chkSunday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.Size = new System.Drawing.Size(62, 17);
            this.chkSunday.TabIndex = 0;
            this.chkSunday.Tag = "Sunday";
            this.chkSunday.Text = "Sunday";
            this.chkSunday.UseVisualStyleBackColor = true;
            // 
            // dtFridayEnd
            // 
            this.dtFridayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtFridayEnd.Location = new System.Drawing.Point(277, 147);
            this.dtFridayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtFridayEnd.Name = "dtFridayEnd";
            this.dtFridayEnd.ShowUpDown = true;
            this.dtFridayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtFridayEnd.TabIndex = 17;
            // 
            // chkSaturday
            // 
            this.chkSaturday.AutoSize = true;
            this.chkSaturday.Location = new System.Drawing.Point(6, 171);
            this.chkSaturday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.Size = new System.Drawing.Size(68, 17);
            this.chkSaturday.TabIndex = 18;
            this.chkSaturday.Text = "Saturday";
            this.chkSaturday.UseVisualStyleBackColor = true;
            // 
            // dtFridayStart
            // 
            this.dtFridayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtFridayStart.Location = new System.Drawing.Point(116, 148);
            this.dtFridayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtFridayStart.Name = "dtFridayStart";
            this.dtFridayStart.ShowUpDown = true;
            this.dtFridayStart.Size = new System.Drawing.Size(110, 20);
            this.dtFridayStart.TabIndex = 16;
            // 
            // chkMonday
            // 
            this.chkMonday.AutoSize = true;
            this.chkMonday.Location = new System.Drawing.Point(6, 57);
            this.chkMonday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkMonday.Name = "chkMonday";
            this.chkMonday.Size = new System.Drawing.Size(64, 17);
            this.chkMonday.TabIndex = 3;
            this.chkMonday.Tag = "Monday";
            this.chkMonday.Text = "Monday";
            this.chkMonday.UseVisualStyleBackColor = true;
            // 
            // dtThursdayEnd
            // 
            this.dtThursdayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtThursdayEnd.Location = new System.Drawing.Point(277, 125);
            this.dtThursdayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtThursdayEnd.Name = "dtThursdayEnd";
            this.dtThursdayEnd.ShowUpDown = true;
            this.dtThursdayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtThursdayEnd.TabIndex = 14;
            // 
            // chkTuesday
            // 
            this.chkTuesday.AutoSize = true;
            this.chkTuesday.Location = new System.Drawing.Point(6, 80);
            this.chkTuesday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkTuesday.Name = "chkTuesday";
            this.chkTuesday.Size = new System.Drawing.Size(67, 17);
            this.chkTuesday.TabIndex = 6;
            this.chkTuesday.Tag = "Tuesday";
            this.chkTuesday.Text = "Tuesday";
            this.chkTuesday.UseVisualStyleBackColor = true;
            // 
            // dtThursdayStart
            // 
            this.dtThursdayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtThursdayStart.Location = new System.Drawing.Point(116, 126);
            this.dtThursdayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtThursdayStart.Name = "dtThursdayStart";
            this.dtThursdayStart.ShowUpDown = true;
            this.dtThursdayStart.Size = new System.Drawing.Size(110, 20);
            this.dtThursdayStart.TabIndex = 13;
            // 
            // chkWednesday
            // 
            this.chkWednesday.AutoSize = true;
            this.chkWednesday.Location = new System.Drawing.Point(6, 103);
            this.chkWednesday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkWednesday.Name = "chkWednesday";
            this.chkWednesday.Size = new System.Drawing.Size(83, 17);
            this.chkWednesday.TabIndex = 9;
            this.chkWednesday.Text = "Wednesday";
            this.chkWednesday.UseVisualStyleBackColor = true;
            // 
            // chkThursday
            // 
            this.chkThursday.AutoSize = true;
            this.chkThursday.Location = new System.Drawing.Point(6, 126);
            this.chkThursday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkThursday.Name = "chkThursday";
            this.chkThursday.Size = new System.Drawing.Size(70, 17);
            this.chkThursday.TabIndex = 12;
            this.chkThursday.Text = "Thursday";
            this.chkThursday.UseVisualStyleBackColor = true;
            // 
            // dtWednesdayStart
            // 
            this.dtWednesdayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtWednesdayStart.Location = new System.Drawing.Point(116, 103);
            this.dtWednesdayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtWednesdayStart.Name = "dtWednesdayStart";
            this.dtWednesdayStart.ShowUpDown = true;
            this.dtWednesdayStart.Size = new System.Drawing.Size(110, 20);
            this.dtWednesdayStart.TabIndex = 10;
            // 
            // chkFriday
            // 
            this.chkFriday.AutoSize = true;
            this.chkFriday.Location = new System.Drawing.Point(6, 148);
            this.chkFriday.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkFriday.Name = "chkFriday";
            this.chkFriday.Size = new System.Drawing.Size(54, 17);
            this.chkFriday.TabIndex = 15;
            this.chkFriday.Text = "Friday";
            this.chkFriday.UseVisualStyleBackColor = true;
            // 
            // dtTuesdayEnd
            // 
            this.dtTuesdayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtTuesdayEnd.Location = new System.Drawing.Point(277, 79);
            this.dtTuesdayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtTuesdayEnd.Name = "dtTuesdayEnd";
            this.dtTuesdayEnd.ShowUpDown = true;
            this.dtTuesdayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtTuesdayEnd.TabIndex = 8;
            this.dtTuesdayEnd.Tag = "Tuesday";
            // 
            // dtSundayStart
            // 
            this.dtSundayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtSundayStart.Location = new System.Drawing.Point(116, 35);
            this.dtSundayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtSundayStart.Name = "dtSundayStart";
            this.dtSundayStart.ShowUpDown = true;
            this.dtSundayStart.Size = new System.Drawing.Size(110, 20);
            this.dtSundayStart.TabIndex = 1;
            this.dtSundayStart.Tag = "Sunday";
            // 
            // dtTuesdayStart
            // 
            this.dtTuesdayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtTuesdayStart.Location = new System.Drawing.Point(116, 80);
            this.dtTuesdayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtTuesdayStart.Name = "dtTuesdayStart";
            this.dtTuesdayStart.ShowUpDown = true;
            this.dtTuesdayStart.Size = new System.Drawing.Size(110, 20);
            this.dtTuesdayStart.TabIndex = 7;
            this.dtTuesdayStart.Tag = "Tuesday";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 18);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Day of the week";
            // 
            // dtMondayEnd
            // 
            this.dtMondayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtMondayEnd.Location = new System.Drawing.Point(277, 56);
            this.dtMondayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtMondayEnd.Name = "dtMondayEnd";
            this.dtMondayEnd.ShowUpDown = true;
            this.dtMondayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtMondayEnd.TabIndex = 5;
            this.dtMondayEnd.Tag = "Monday";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(114, 18);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Start Notifications";
            // 
            // dtMondayStart
            // 
            this.dtMondayStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtMondayStart.Location = new System.Drawing.Point(116, 57);
            this.dtMondayStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtMondayStart.Name = "dtMondayStart";
            this.dtMondayStart.ShowUpDown = true;
            this.dtMondayStart.Size = new System.Drawing.Size(110, 20);
            this.dtMondayStart.TabIndex = 4;
            this.dtMondayStart.Tag = "Monday";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(275, 18);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(87, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "End Notifications";
            // 
            // dtSundayEnd
            // 
            this.dtSundayEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtSundayEnd.Location = new System.Drawing.Point(277, 34);
            this.dtSundayEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dtSundayEnd.Name = "dtSundayEnd";
            this.dtSundayEnd.ShowUpDown = true;
            this.dtSundayEnd.Size = new System.Drawing.Size(110, 20);
            this.dtSundayEnd.TabIndex = 2;
            this.dtSundayEnd.Tag = "Sunday";
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(534, 499);
            this.btnAdvanced.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(75, 23);
            this.btnAdvanced.TabIndex = 154;
            this.btnAdvanced.Text = "Advanced";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // FrmClientDepartmentDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1296, 937);
            this.Controls.Add(this.tabDeptDetailsTabs);
            this.Controls.Add(this.lblClientDeptMandatory);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.lblClientName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(2048, 1024);
            this.MinimumSize = new System.Drawing.Size(100, 50);
            this.Name = "FrmClientDepartmentDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Client Department Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClientDepartmentDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmClientDepartmentDetails_Load);
            this.Shown += new System.EventHandler(this.FrmClientDepartmentDetails_Shown);
            this.tabRecordKeeping.ResumeLayout(false);
            this.tabRecordKeeping.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.DepartmentInfo.ResumeLayout(false);
            this.DepartmentInfo.PerformLayout();
            this.pnlPaymentType.ResumeLayout(false);
            this.pnlPaymentType.PerformLayout();
            this.pnlMROType.ResumeLayout(false);
            this.pnlMROType.PerformLayout();
            this.tabDeptDetailsTabs.ResumeLayout(false);
            this.tabNotificationSettingsClientDepartmentDetails.ResumeLayout(false);
            this.groupBoxRandomizationSettings.ResumeLayout(false);
            this.groupBoxRandomizationSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxSendIns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadlineAlert)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBoxTools.ResumeLayout(false);
            this.groupBoxTools.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gboxAdvanced.ResumeLayout(false);
            this.gboxAdvanced.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayHours)).EndInit();
            this.groupNotifcationDaySettings.ResumeLayout(false);
            this.groupNotifcationDaySettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Label lblClientDeptMandatory;
        private System.Windows.Forms.TabPage tabRecordKeeping;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblduplicateLabCodeFrom;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtduplicatetoLabCode;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnAddRetain;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label txtID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnClearFields;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNotify3;
        private System.Windows.Forms.TextBox txtNotify2;
        private System.Windows.Forms.TextBox txtNotify1;
        private System.Windows.Forms.TextBox txtInstructions;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkArchived;
        private System.Windows.Forms.ComboBox txtSemester;
        private System.Windows.Forms.CheckBox chkRequired;
        private System.Windows.Forms.CheckBox chkExpires;
        private System.Windows.Forms.CheckBox chkNotifyStudent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtDueDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.TabPage DepartmentInfo;
        private System.Windows.Forms.CheckBox chkRecordKeeping;
        private System.Windows.Forms.CheckBox chkBC;
        private System.Windows.Forms.Button btnClearStarAssign;
        private System.Windows.Forms.Label lblClearStarClientCode;
        private Controls.TextBoxes.SurTextBox txtClearStarClientCode;
        private Controls.TextBoxes.SurTextBox txtLabCode;
        private Controls.TextBoxes.SurTextBox txtQuestCode;
        private Controls.TextBoxes.SurTextBox txtMailingZipCode;
        private Controls.TextBoxes.SurTextBox txtPhysicalZipCode;
        private Controls.TextBoxes.SurTextBox txtPhysicalCity;
        private Controls.TextBoxes.SurTextBox txtPhysicalState;
        private Controls.TextBoxes.SurTextBox txtPhysicalAddress2;
        private Controls.TextBoxes.SurTextBox txtPhysicalAddress1;
        private Controls.TextBoxes.SurTextBox txtMailingState;
        private Controls.TextBoxes.SurTextBox txtMailingCity;
        private Controls.TextBoxes.SurTextBox txtMailingAddress2;
        private Controls.TextBoxes.SurTextBox txtMailingAddress1;
        private Controls.TextBoxes.SurTextBox txtLastName;
        private Controls.TextBoxes.SurTextBox txtEmail;
        private Controls.TextBoxes.SurTextBox txtFirstName;
        private Controls.TextBoxes.SurTextBox txtDepartmentName;
        private System.Windows.Forms.TextBox txtSalesRepresentative;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLabCode;
        private System.Windows.Forms.Label lblQuestCode;
        private System.Windows.Forms.ComboBox cmbSalesRepresentative;
        private System.Windows.Forms.Label lblSalesInfo;
        private System.Windows.Forms.Label lblSalesRepresentive;
        private System.Windows.Forms.ComboBox cmbPhysicalState;
        private System.Windows.Forms.Label lblPhysicalZipCode;
        private System.Windows.Forms.Label lblPhysicalState;
        private System.Windows.Forms.Label lblPhysicalCity;
        private System.Windows.Forms.Label lblPhysicalAddress1;
        private System.Windows.Forms.Label lblPhysicalAddress2;
        private System.Windows.Forms.Label lblPhysicalAddress;
        private System.Windows.Forms.ComboBox cmbMailingState;
        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.CheckBox chkSameAsClient;
        private System.Windows.Forms.CheckBox chkSameAsPhysical;
        private System.Windows.Forms.Label lblMailingState;
        private System.Windows.Forms.Label lblMailingCity;
        private System.Windows.Forms.Label lblMailingAddress2;
        private System.Windows.Forms.Label lblMailingAddress1;
        private System.Windows.Forms.Label lblMailingAddress;
        private System.Windows.Forms.MaskedTextBox txtFax;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblMainContactInfo;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblPaymentMan;
        private System.Windows.Forms.Panel pnlPaymentType;
        private System.Windows.Forms.RadioButton rbInvoiceClient;
        private System.Windows.Forms.RadioButton rbDonorPays;
        private System.Windows.Forms.Label lblPaymentType;
        private System.Windows.Forms.Label lblMROMan;
        private System.Windows.Forms.Label lblCategoryMan;
        private System.Windows.Forms.Label lblDepartmentNameMan;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlMROType;
        private System.Windows.Forms.RadioButton rbMALL;
        private System.Windows.Forms.RadioButton rbMPOS;
        private System.Windows.Forms.Label lblMROType;
        private System.Windows.Forms.CheckBox chkDNA;
        private System.Windows.Forms.CheckBox chkHair;
        private System.Windows.Forms.CheckBox chkUA;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblDepartmentName;
        private System.Windows.Forms.TabControl tabDeptDetailsTabs;
        private System.Windows.Forms.TabPage tabNotificationSettingsClientDepartmentDetails;
        private System.Windows.Forms.GroupBox groupNotifcationDaySettings;
        private System.Windows.Forms.DateTimePicker dtSaturdayEnd;
        private System.Windows.Forms.DateTimePicker dtWednesdayEnd;
        private System.Windows.Forms.DateTimePicker dtSaturdayStart;
        private System.Windows.Forms.CheckBox chkSunday;
        private System.Windows.Forms.DateTimePicker dtFridayEnd;
        private System.Windows.Forms.CheckBox chkSaturday;
        private System.Windows.Forms.DateTimePicker dtFridayStart;
        private System.Windows.Forms.CheckBox chkMonday;
        private System.Windows.Forms.DateTimePicker dtThursdayEnd;
        private System.Windows.Forms.CheckBox chkTuesday;
        private System.Windows.Forms.DateTimePicker dtThursdayStart;
        private System.Windows.Forms.CheckBox chkWednesday;
        private System.Windows.Forms.CheckBox chkThursday;
        private System.Windows.Forms.DateTimePicker dtWednesdayStart;
        private System.Windows.Forms.CheckBox chkFriday;
        private System.Windows.Forms.DateTimePicker dtTuesdayEnd;
        private System.Windows.Forms.DateTimePicker dtSundayStart;
        private System.Windows.Forms.DateTimePicker dtTuesdayStart;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtMondayEnd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtMondayStart;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DateTimePicker dtSundayEnd;
        private System.Windows.Forms.Button btnResetNotification;
        private System.Windows.Forms.Button btnSaveNotification;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown numDelayHours;
        private System.Windows.Forms.RadioButton radASAPorDelay2;
        private System.Windows.Forms.RadioButton radASAPorDelay1;
        private System.Windows.Forms.CheckBox chkForceManualNotificaitons;
        private System.Windows.Forms.GroupBox gboxAdvanced;
        private System.Windows.Forms.Button btnResetJSON;
        private System.Windows.Forms.TextBox txtJSONTemplateSettings;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.TextBox txtGlobalMROName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtSMSReply;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox chkOnsiteTesting;
        private System.Windows.Forms.CheckBox chkEnableSMS;
        private System.Windows.Forms.GroupBox groupBoxRandomizationSettings;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DateTimePicker dtSendInStop;
        private System.Windows.Forms.TextBox txtFilenameRenderSettings;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtFilenamePDFTemplate;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DateTimePicker dtSweepDate;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.DateTimePicker dtSendInStart;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox cmboCopyWindowSettings;
        private System.Windows.Forms.Button btnCopySettings;
        private System.Windows.Forms.NumericUpDown nudDeadlineAlert;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox ckbWebShowNotifyButton;
        private System.Windows.Forms.Button btnLoadDefaults;
        private System.Windows.Forms.NumericUpDown nudMaxSendIns;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtPreviewPhone;
        private System.Windows.Forms.TextBox txtPreviewEmail;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPreviewZipCode;
        private System.Windows.Forms.Button btnEmailPreview;
        private System.Windows.Forms.Label lblFormFoxCode;
        private Controls.TextBoxes.SurTextBox txtFormFoxCode;
        private System.Windows.Forms.CheckBox chkUseFormFox;
        private System.Windows.Forms.Label lblUseFormFox;
    }
}