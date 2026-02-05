using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmExceptions : Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExceptions));
            this.tabsExceptions = new System.Windows.Forms.TabControl();
            this.tabExceptionNotifications = new System.Windows.Forms.TabPage();
            this.dgvClinicExceptions = new System.Windows.Forms.DataGridView();
            this.DonorSelection = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.in_window = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClinicsInRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxMiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorDateNotifiedData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecimenID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClearStarID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecimenDateValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentMethodId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorTestRegisteredDateValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorDateNotified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentDateValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MROType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorTestInfoId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorInitialClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestInfoClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestInfoDepartmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MROTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestRequestedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReasonForTestId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestCategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestPanelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorDateOfBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpecimenDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorRegistrationStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorSSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestOverallResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorTestRegisteredDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.nudNumberToSelect = new System.Windows.Forms.NumericUpDown();
            this.btnRandom = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblSearching = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numMaxRange = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numMinClinics = new System.Windows.Forms.NumericUpDown();
            this.numRangeInMiles = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarMiles = new System.Windows.Forms.TrackBar();
            this.btnNotify = new System.Windows.Forms.Button();
            this.tabSMSReplies = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvSMSReplies = new System.Windows.Forms.DataGridView();
            this.DonorSelection_sms = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DonorFirstName_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorLastName_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SMSReply_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSN_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentName_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorTestRegisteredDate_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateDonorNotified_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorCity_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipCode_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.auto_reply_text_sms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplyToSms_sms = new System.Windows.Forms.DataGridViewButtonColumn();
            this.MarkAsRead_sms = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabClientScheduleExpired = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvSendInScheduler = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.client_name_sis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department_name_sis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notification_sweep_date_sis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notification_start_date_sis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notification_stop_date_sis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDeadlineDonors = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvDeadlineDonors = new System.Windows.Forms.DataGridView();
            this.DonorSelection_dld = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorFirstName_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorLastName_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSN_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOB_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentName_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorTestRegisteredDateValue_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notification_stop_date_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorCity_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipCode_dld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabFormFox = new System.Windows.Forms.TabPage();
            this.ffFlipArchive = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.ffRefresh = new System.Windows.Forms.Button();
            this.ffDaysAgo = new System.Windows.Forms.NumericUpDown();
            this.ffIncludeArchived = new System.Windows.Forms.CheckBox();
            this.dgvFormFox = new System.Windows.Forms.DataGridView();
            this.DonorSelection_ffo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsArchived_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donor_first_name__ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donor_last_name_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSN_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donor_date_of_birth_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donor_city_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.donor_zip_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.client_name_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department_name_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdON_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReferenceTestID_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatedOn_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backend_formfox_orders_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.archived_ffo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabsExceptions.SuspendLayout();
            this.tabExceptionNotifications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClinicExceptions)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberToSelect)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinClinics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRangeInMiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMiles)).BeginInit();
            this.tabSMSReplies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSMSReplies)).BeginInit();
            this.tabClientScheduleExpired.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendInScheduler)).BeginInit();
            this.tabDeadlineDonors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeadlineDonors)).BeginInit();
            this.tabFormFox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ffDaysAgo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormFox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabsExceptions
            // 
            this.tabsExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsExceptions.Controls.Add(this.tabExceptionNotifications);
            this.tabsExceptions.Controls.Add(this.tabSMSReplies);
            this.tabsExceptions.Controls.Add(this.tabClientScheduleExpired);
            this.tabsExceptions.Controls.Add(this.tabDeadlineDonors);
            this.tabsExceptions.Controls.Add(this.tabFormFox);
            this.tabsExceptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabsExceptions.ImageList = this.imageList1;
            this.tabsExceptions.Location = new System.Drawing.Point(0, 2);
            this.tabsExceptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabsExceptions.Multiline = true;
            this.tabsExceptions.Name = "tabsExceptions";
            this.tabsExceptions.SelectedIndex = 0;
            this.tabsExceptions.Size = new System.Drawing.Size(2003, 1006);
            this.tabsExceptions.TabIndex = 72;
            this.tabsExceptions.SelectedIndexChanged += new System.EventHandler(this.tabsExceptions_SelectedIndexChanged);
            // 
            // tabExceptionNotifications
            // 
            this.tabExceptionNotifications.BackColor = System.Drawing.Color.Silver;
            this.tabExceptionNotifications.Controls.Add(this.dgvClinicExceptions);
            this.tabExceptionNotifications.Controls.Add(this.panel1);
            this.tabExceptionNotifications.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabExceptionNotifications.ImageKey = "flag-green-trans-ico.png";
            this.tabExceptionNotifications.Location = new System.Drawing.Point(4, 26);
            this.tabExceptionNotifications.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabExceptionNotifications.Name = "tabExceptionNotifications";
            this.tabExceptionNotifications.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabExceptionNotifications.Size = new System.Drawing.Size(1995, 976);
            this.tabExceptionNotifications.TabIndex = 1;
            this.tabExceptionNotifications.Text = "Clinic Exceptions";
            this.tabExceptionNotifications.Click += new System.EventHandler(this.tabExceptionNotifications_Click);
            // 
            // dgvClinicExceptions
            // 
            this.dgvClinicExceptions.AllowDrop = true;
            this.dgvClinicExceptions.AllowUserToAddRows = false;
            this.dgvClinicExceptions.AllowUserToDeleteRows = false;
            this.dgvClinicExceptions.AllowUserToOrderColumns = true;
            this.dgvClinicExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClinicExceptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClinicExceptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorSelection,
            this.in_window,
            this.ClinicsInRange,
            this.MaxMiles,
            this.DonorDateNotifiedData,
            this.DonorFirstName,
            this.DonorLastName,
            this.SSN,
            this.DOB,
            this.SpecimenID,
            this.ClearStarID,
            this.SpecimenDateValue,
            this.ClientName,
            this.DepartmentName,
            this.Status,
            this.PaymentMode,
            this.PaymentMethodId,
            this.PaymentAmount,
            this.DonorTestRegisteredDateValue,
            this.DonorDateNotified,
            this.PaymentDateValue,
            this.PaymentType,
            this.Result,
            this.TestReason,
            this.DonorCity,
            this.ZipCode,
            this.MROType,
            this.DonorId,
            this.DonorTestInfoId,
            this.DonorInitialClientId,
            this.TestInfoClientId,
            this.TestInfoDepartmentId,
            this.MROTypeId,
            this.PaymentTypeId,
            this.TestRequestedDate,
            this.ReasonForTestId,
            this.TestCategoryId,
            this.TestPanelId,
            this.DonorDateOfBirth,
            this.SpecimenDate,
            this.DonorRegistrationStatus,
            this.TestStatus,
            this.DonorSSN,
            this.TestOverallResult,
            this.PaymentReceived,
            this.PaymentDate,
            this.DonorTestRegisteredDate});
            this.dgvClinicExceptions.Location = new System.Drawing.Point(16, 137);
            this.dgvClinicExceptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvClinicExceptions.Name = "dgvClinicExceptions";
            this.dgvClinicExceptions.ReadOnly = true;
            this.dgvClinicExceptions.RowHeadersVisible = false;
            this.dgvClinicExceptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClinicExceptions.Size = new System.Drawing.Size(1965, 827);
            this.dgvClinicExceptions.TabIndex = 4;
            this.dgvClinicExceptions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvClinicExceptions_CellFormatting);
            this.dgvClinicExceptions.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvClinicExceptions_CellMouseClick);
            this.dgvClinicExceptions.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvClinicExceptions_CellMouseDoubleClick);
            this.dgvClinicExceptions.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvClinicExceptions_CellMouseUp);
            this.dgvClinicExceptions.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ColumnHeaderMouseClick);
            this.dgvClinicExceptions.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvClinicExceptions_DataBindingComplete);
            // 
            // DonorSelection
            // 
            this.DonorSelection.FalseValue = "false";
            this.DonorSelection.HeaderText = "";
            this.DonorSelection.Name = "DonorSelection";
            this.DonorSelection.ReadOnly = true;
            this.DonorSelection.TrueValue = "true";
            this.DonorSelection.Width = 50;
            // 
            // in_window
            // 
            this.in_window.DataPropertyName = "in_window";
            this.in_window.HeaderText = "In Window";
            this.in_window.Name = "in_window";
            this.in_window.ReadOnly = true;
            // 
            // ClinicsInRange
            // 
            this.ClinicsInRange.HeaderText = "Clinics In Range";
            this.ClinicsInRange.Name = "ClinicsInRange";
            this.ClinicsInRange.ReadOnly = true;
            // 
            // MaxMiles
            // 
            this.MaxMiles.DataPropertyName = "clinic_radius_text";
            this.MaxMiles.HeaderText = "Max Miles";
            this.MaxMiles.Name = "MaxMiles";
            this.MaxMiles.ReadOnly = true;
            // 
            // DonorDateNotifiedData
            // 
            this.DonorDateNotifiedData.HeaderText = "DonorDateNotifiedData";
            this.DonorDateNotifiedData.Name = "DonorDateNotifiedData";
            this.DonorDateNotifiedData.ReadOnly = true;
            this.DonorDateNotifiedData.Visible = false;
            // 
            // DonorFirstName
            // 
            this.DonorFirstName.DataPropertyName = "DonorFirstName";
            this.DonorFirstName.HeaderText = "First Name";
            this.DonorFirstName.Name = "DonorFirstName";
            this.DonorFirstName.ReadOnly = true;
            // 
            // DonorLastName
            // 
            this.DonorLastName.DataPropertyName = "DonorLastName";
            this.DonorLastName.HeaderText = "Last Name";
            this.DonorLastName.Name = "DonorLastName";
            this.DonorLastName.ReadOnly = true;
            // 
            // SSN
            // 
            this.SSN.HeaderText = "SSN";
            this.SSN.Name = "SSN";
            this.SSN.ReadOnly = true;
            this.SSN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // DOB
            // 
            this.DOB.HeaderText = "DOB";
            this.DOB.Name = "DOB";
            this.DOB.ReadOnly = true;
            this.DOB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // SpecimenID
            // 
            this.SpecimenID.DataPropertyName = "SpecimenId";
            this.SpecimenID.HeaderText = "Specimen ID";
            this.SpecimenID.Name = "SpecimenID";
            this.SpecimenID.ReadOnly = true;
            this.SpecimenID.Visible = false;
            // 
            // ClearStarID
            // 
            this.ClearStarID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.ClearStarID.DataPropertyName = "DonorClearStarProfId";
            this.ClearStarID.HeaderText = "ClearStar ID";
            this.ClearStarID.Name = "ClearStarID";
            this.ClearStarID.ReadOnly = true;
            this.ClearStarID.Visible = false;
            // 
            // SpecimenDateValue
            // 
            this.SpecimenDateValue.HeaderText = "Specimen Date";
            this.SpecimenDateValue.Name = "SpecimenDateValue";
            this.SpecimenDateValue.ReadOnly = true;
            this.SpecimenDateValue.Visible = false;
            this.SpecimenDateValue.Width = 120;
            // 
            // ClientName
            // 
            this.ClientName.DataPropertyName = "ClientName";
            this.ClientName.HeaderText = "Client";
            this.ClientName.Name = "ClientName";
            this.ClientName.ReadOnly = true;
            // 
            // DepartmentName
            // 
            this.DepartmentName.DataPropertyName = "ClientDepartmentName";
            this.DepartmentName.HeaderText = "Department";
            this.DepartmentName.Name = "DepartmentName";
            this.DepartmentName.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // PaymentMode
            // 
            this.PaymentMode.HeaderText = "Payment Mode";
            this.PaymentMode.Name = "PaymentMode";
            this.PaymentMode.ReadOnly = true;
            this.PaymentMode.Visible = false;
            this.PaymentMode.Width = 115;
            // 
            // PaymentMethodId
            // 
            this.PaymentMethodId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PaymentMethodId.DataPropertyName = "PaymentMethodId";
            this.PaymentMethodId.HeaderText = "PaymentMethodId";
            this.PaymentMethodId.Name = "PaymentMethodId";
            this.PaymentMethodId.ReadOnly = true;
            this.PaymentMethodId.Visible = false;
            // 
            // PaymentAmount
            // 
            this.PaymentAmount.DataPropertyName = "TotalPaymentAmount";
            this.PaymentAmount.HeaderText = "Amount";
            this.PaymentAmount.Name = "PaymentAmount";
            this.PaymentAmount.ReadOnly = true;
            this.PaymentAmount.Visible = false;
            // 
            // DonorTestRegisteredDateValue
            // 
            this.DonorTestRegisteredDateValue.HeaderText = "Date Registered";
            this.DonorTestRegisteredDateValue.Name = "DonorTestRegisteredDateValue";
            this.DonorTestRegisteredDateValue.ReadOnly = true;
            // 
            // DonorDateNotified
            // 
            this.DonorDateNotified.DataPropertyName = "Notified_by_email_timestamp";
            this.DonorDateNotified.HeaderText = "Donor Date Notified";
            this.DonorDateNotified.Name = "DonorDateNotified";
            this.DonorDateNotified.ReadOnly = true;
            // 
            // PaymentDateValue
            // 
            this.PaymentDateValue.HeaderText = "Payment Date";
            this.PaymentDateValue.Name = "PaymentDateValue";
            this.PaymentDateValue.ReadOnly = true;
            this.PaymentDateValue.Visible = false;
            // 
            // PaymentType
            // 
            this.PaymentType.HeaderText = "Payment Type";
            this.PaymentType.Name = "PaymentType";
            this.PaymentType.ReadOnly = true;
            this.PaymentType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Result
            // 
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Visible = false;
            // 
            // TestReason
            // 
            this.TestReason.HeaderText = "Test Reason";
            this.TestReason.Name = "TestReason";
            this.TestReason.ReadOnly = true;
            this.TestReason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.TestReason.Visible = false;
            // 
            // DonorCity
            // 
            this.DonorCity.DataPropertyName = "DonorCity";
            this.DonorCity.HeaderText = "Donor City";
            this.DonorCity.Name = "DonorCity";
            this.DonorCity.ReadOnly = true;
            // 
            // ZipCode
            // 
            this.ZipCode.DataPropertyName = "DonorZipCode";
            this.ZipCode.HeaderText = "Zip Code";
            this.ZipCode.Name = "ZipCode";
            this.ZipCode.ReadOnly = true;
            // 
            // MROType
            // 
            this.MROType.HeaderText = "MRO Type";
            this.MROType.Name = "MROType";
            this.MROType.ReadOnly = true;
            this.MROType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.MROType.Visible = false;
            // 
            // DonorId
            // 
            this.DonorId.DataPropertyName = "DonorId";
            this.DonorId.HeaderText = "DonorId";
            this.DonorId.Name = "DonorId";
            this.DonorId.ReadOnly = true;
            this.DonorId.Visible = false;
            // 
            // DonorTestInfoId
            // 
            this.DonorTestInfoId.DataPropertyName = "DonorTestInfoId";
            this.DonorTestInfoId.HeaderText = "DonorTestInfoId";
            this.DonorTestInfoId.Name = "DonorTestInfoId";
            this.DonorTestInfoId.ReadOnly = true;
            this.DonorTestInfoId.Visible = false;
            // 
            // DonorInitialClientId
            // 
            this.DonorInitialClientId.DataPropertyName = "DonorInitialClientId";
            this.DonorInitialClientId.HeaderText = "DonorInitialClientId";
            this.DonorInitialClientId.Name = "DonorInitialClientId";
            this.DonorInitialClientId.ReadOnly = true;
            this.DonorInitialClientId.Visible = false;
            // 
            // TestInfoClientId
            // 
            this.TestInfoClientId.DataPropertyName = "TestInfoClientId";
            this.TestInfoClientId.HeaderText = "TestInfoClientId";
            this.TestInfoClientId.Name = "TestInfoClientId";
            this.TestInfoClientId.ReadOnly = true;
            this.TestInfoClientId.Visible = false;
            // 
            // TestInfoDepartmentId
            // 
            this.TestInfoDepartmentId.DataPropertyName = "TestInfoDepartmentId";
            this.TestInfoDepartmentId.HeaderText = "TestInfoDepartmentId";
            this.TestInfoDepartmentId.Name = "TestInfoDepartmentId";
            this.TestInfoDepartmentId.ReadOnly = true;
            this.TestInfoDepartmentId.Visible = false;
            // 
            // MROTypeId
            // 
            this.MROTypeId.DataPropertyName = "MROTypeId";
            this.MROTypeId.HeaderText = "MROTypeId";
            this.MROTypeId.Name = "MROTypeId";
            this.MROTypeId.ReadOnly = true;
            this.MROTypeId.Visible = false;
            // 
            // PaymentTypeId
            // 
            this.PaymentTypeId.DataPropertyName = "PaymentTypeId";
            this.PaymentTypeId.HeaderText = "PaymentTypeId";
            this.PaymentTypeId.Name = "PaymentTypeId";
            this.PaymentTypeId.ReadOnly = true;
            this.PaymentTypeId.Visible = false;
            // 
            // TestRequestedDate
            // 
            this.TestRequestedDate.DataPropertyName = "TestRequestedDate";
            this.TestRequestedDate.HeaderText = "TestRequestedDate";
            this.TestRequestedDate.Name = "TestRequestedDate";
            this.TestRequestedDate.ReadOnly = true;
            this.TestRequestedDate.Visible = false;
            // 
            // ReasonForTestId
            // 
            this.ReasonForTestId.DataPropertyName = "ReasonForTestId";
            this.ReasonForTestId.HeaderText = "ReasonForTestId";
            this.ReasonForTestId.Name = "ReasonForTestId";
            this.ReasonForTestId.ReadOnly = true;
            this.ReasonForTestId.Visible = false;
            // 
            // TestCategoryId
            // 
            this.TestCategoryId.DataPropertyName = "TestCategoryId";
            this.TestCategoryId.HeaderText = "TestCategoryId";
            this.TestCategoryId.Name = "TestCategoryId";
            this.TestCategoryId.ReadOnly = true;
            this.TestCategoryId.Visible = false;
            // 
            // TestPanelId
            // 
            this.TestPanelId.DataPropertyName = "TestPanelId";
            this.TestPanelId.HeaderText = "TestPanelId";
            this.TestPanelId.Name = "TestPanelId";
            this.TestPanelId.ReadOnly = true;
            this.TestPanelId.Visible = false;
            // 
            // DonorDateOfBirth
            // 
            this.DonorDateOfBirth.DataPropertyName = "DonorDateOfBirth";
            this.DonorDateOfBirth.HeaderText = "DonorDateOfBirth";
            this.DonorDateOfBirth.Name = "DonorDateOfBirth";
            this.DonorDateOfBirth.ReadOnly = true;
            this.DonorDateOfBirth.Visible = false;
            // 
            // SpecimenDate
            // 
            this.SpecimenDate.DataPropertyName = "SpecimenDate";
            this.SpecimenDate.HeaderText = "SpecimenDate";
            this.SpecimenDate.Name = "SpecimenDate";
            this.SpecimenDate.ReadOnly = true;
            this.SpecimenDate.Visible = false;
            // 
            // DonorRegistrationStatus
            // 
            this.DonorRegistrationStatus.DataPropertyName = "DonorRegistrationStatus";
            this.DonorRegistrationStatus.HeaderText = "DonorRegistrationStatus";
            this.DonorRegistrationStatus.Name = "DonorRegistrationStatus";
            this.DonorRegistrationStatus.ReadOnly = true;
            this.DonorRegistrationStatus.Visible = false;
            // 
            // TestStatus
            // 
            this.TestStatus.DataPropertyName = "TestStatus";
            this.TestStatus.HeaderText = "TestStatus";
            this.TestStatus.Name = "TestStatus";
            this.TestStatus.ReadOnly = true;
            this.TestStatus.Visible = false;
            // 
            // DonorSSN
            // 
            this.DonorSSN.DataPropertyName = "DonorSSN";
            this.DonorSSN.HeaderText = "DonorSSN";
            this.DonorSSN.Name = "DonorSSN";
            this.DonorSSN.ReadOnly = true;
            this.DonorSSN.Visible = false;
            // 
            // TestOverallResult
            // 
            this.TestOverallResult.DataPropertyName = "TestOverallResult";
            this.TestOverallResult.HeaderText = "Test Overall Result";
            this.TestOverallResult.Name = "TestOverallResult";
            this.TestOverallResult.ReadOnly = true;
            this.TestOverallResult.Visible = false;
            // 
            // PaymentReceived
            // 
            this.PaymentReceived.DataPropertyName = "PaymentReceived";
            this.PaymentReceived.HeaderText = "PaymentReceived";
            this.PaymentReceived.Name = "PaymentReceived";
            this.PaymentReceived.ReadOnly = true;
            this.PaymentReceived.Visible = false;
            // 
            // PaymentDate
            // 
            this.PaymentDate.DataPropertyName = "PaymentDate";
            this.PaymentDate.HeaderText = "PaymentDateBind";
            this.PaymentDate.Name = "PaymentDate";
            this.PaymentDate.ReadOnly = true;
            this.PaymentDate.Visible = false;
            // 
            // DonorTestRegisteredDate
            // 
            this.DonorTestRegisteredDate.DataPropertyName = "DonorTestRegisteredDate";
            this.DonorTestRegisteredDate.HeaderText = "DonorTestRegisteredDate";
            this.DonorTestRegisteredDate.Name = "DonorTestRegisteredDate";
            this.DonorTestRegisteredDate.ReadOnly = true;
            this.DonorTestRegisteredDate.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.btnPreview);
            this.panel1.Controls.Add(this.nudNumberToSelect);
            this.panel1.Controls.Add(this.btnRandom);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnNotify);
            this.panel1.Location = new System.Drawing.Point(16, 7);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1796, 103);
            this.panel1.TabIndex = 3;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(404, 47);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(252, 23);
            this.txtEmail.TabIndex = 14;
            this.txtEmail.Visible = false;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(211, 44);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(184, 28);
            this.btnPreview.TabIndex = 13;
            this.btnPreview.Text = "Email Preview(s)";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Visible = false;
            // 
            // nudNumberToSelect
            // 
            this.nudNumberToSelect.Location = new System.Drawing.Point(20, 14);
            this.nudNumberToSelect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudNumberToSelect.Name = "nudNumberToSelect";
            this.nudNumberToSelect.Size = new System.Drawing.Size(59, 23);
            this.nudNumberToSelect.TabIndex = 12;
            this.nudNumberToSelect.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(85, 10);
            this.btnRandom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(117, 28);
            this.btnRandom.TabIndex = 11;
            this.btnRandom.Text = "Random";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblSearching);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.numMaxRange);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.numMinClinics);
            this.panel2.Controls.Add(this.numRangeInMiles);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.trackBarMiles);
            this.panel2.Location = new System.Drawing.Point(688, 12);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1084, 76);
            this.panel2.TabIndex = 5;
            // 
            // lblSearching
            // 
            this.lblSearching.AutoSize = true;
            this.lblSearching.Location = new System.Drawing.Point(741, 41);
            this.lblSearching.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearching.Name = "lblSearching";
            this.lblSearching.Size = new System.Drawing.Size(88, 17);
            this.lblSearching.TabIndex = 17;
            this.lblSearching.Text = "Searching....";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(364, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "Maximum Range";
            // 
            // numMaxRange
            // 
            this.numMaxRange.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMaxRange.Location = new System.Drawing.Point(499, 38);
            this.numMaxRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numMaxRange.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numMaxRange.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMaxRange.Name = "numMaxRange";
            this.numMaxRange.Size = new System.Drawing.Size(84, 23);
            this.numMaxRange.TabIndex = 17;
            this.numMaxRange.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "Minimum # of Clinics";
            // 
            // numMinClinics
            // 
            this.numMinClinics.Location = new System.Drawing.Point(208, 38);
            this.numMinClinics.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numMinClinics.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numMinClinics.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinClinics.Name = "numMinClinics";
            this.numMinClinics.Size = new System.Drawing.Size(133, 23);
            this.numMinClinics.TabIndex = 15;
            this.numMinClinics.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinClinics.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numRangeInMiles
            // 
            this.numRangeInMiles.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numRangeInMiles.Location = new System.Drawing.Point(208, 4);
            this.numRangeInMiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numRangeInMiles.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numRangeInMiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRangeInMiles.Name = "numRangeInMiles";
            this.numRangeInMiles.Size = new System.Drawing.Size(133, 23);
            this.numRangeInMiles.TabIndex = 14;
            this.numRangeInMiles.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRangeInMiles.ValueChanged += new System.EventHandler(this.numRangeInMiles_ValueChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(601, 34);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(133, 28);
            this.button4.TabIndex = 13;
            this.button4.Text = "Find Closest";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Clinic Search Range (Miles)";
            // 
            // trackBarMiles
            // 
            this.trackBarMiles.Location = new System.Drawing.Point(349, 7);
            this.trackBarMiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trackBarMiles.Maximum = 150;
            this.trackBarMiles.Minimum = 1;
            this.trackBarMiles.Name = "trackBarMiles";
            this.trackBarMiles.Size = new System.Drawing.Size(509, 56);
            this.trackBarMiles.TabIndex = 3;
            this.trackBarMiles.Value = 1;
            this.trackBarMiles.Scroll += new System.EventHandler(this.trackBarMiles_Scroll);
            this.trackBarMiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseDown);
            this.trackBarMiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // btnNotify
            // 
            this.btnNotify.Location = new System.Drawing.Point(19, 44);
            this.btnNotify.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNotify.Name = "btnNotify";
            this.btnNotify.Size = new System.Drawing.Size(184, 28);
            this.btnNotify.TabIndex = 1;
            this.btnNotify.Text = "Send In";
            this.btnNotify.UseVisualStyleBackColor = true;
            this.btnNotify.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabSMSReplies
            // 
            this.tabSMSReplies.BackColor = System.Drawing.Color.Silver;
            this.tabSMSReplies.Controls.Add(this.label4);
            this.tabSMSReplies.Controls.Add(this.dgvSMSReplies);
            this.tabSMSReplies.ImageKey = "flag-green-trans-ico.png";
            this.tabSMSReplies.Location = new System.Drawing.Point(4, 26);
            this.tabSMSReplies.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabSMSReplies.Name = "tabSMSReplies";
            this.tabSMSReplies.Size = new System.Drawing.Size(1995, 976);
            this.tabSMSReplies.TabIndex = 2;
            this.tabSMSReplies.Text = "SMS Replies";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(593, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "The records below are inbound SMS messages that need to be replied to or marked a" +
    "s read.";
            // 
            // dgvSMSReplies
            // 
            this.dgvSMSReplies.AllowUserToAddRows = false;
            this.dgvSMSReplies.AllowUserToDeleteRows = false;
            this.dgvSMSReplies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSMSReplies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSMSReplies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorSelection_sms,
            this.DonorFirstName_sms,
            this.DonorLastName_sms,
            this.SMSReply_sms,
            this.SSN_sms,
            this.ClientName_sms,
            this.DepartmentName_sms,
            this.DonorTestRegisteredDate_sms,
            this.DateDonorNotified_sms,
            this.DonorCity_sms,
            this.ZipCode_sms,
            this.auto_reply_text_sms,
            this.ReplyToSms_sms,
            this.MarkAsRead_sms});
            this.dgvSMSReplies.Location = new System.Drawing.Point(13, 70);
            this.dgvSMSReplies.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvSMSReplies.MultiSelect = false;
            this.dgvSMSReplies.Name = "dgvSMSReplies";
            this.dgvSMSReplies.ReadOnly = true;
            this.dgvSMSReplies.RowHeadersVisible = false;
            this.dgvSMSReplies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSMSReplies.Size = new System.Drawing.Size(1965, 894);
            this.dgvSMSReplies.TabIndex = 5;
            this.dgvSMSReplies.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSMSReplies_CellClick);
            this.dgvSMSReplies.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSMSReplies_CellFormatting);
            this.dgvSMSReplies.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSMSReplies_CellMouseDoubleClick);
            this.dgvSMSReplies.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseUp);
            this.dgvSMSReplies.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ColumnHeaderMouseClick);
            this.dgvSMSReplies.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSMSReplies_DataBindingComplete);
            this.dgvSMSReplies.SelectionChanged += new System.EventHandler(this.dgvSMSReplies_SelectionChanged);
            // 
            // DonorSelection_sms
            // 
            this.DonorSelection_sms.HeaderText = "";
            this.DonorSelection_sms.Name = "DonorSelection_sms";
            this.DonorSelection_sms.ReadOnly = true;
            this.DonorSelection_sms.Visible = false;
            this.DonorSelection_sms.Width = 50;
            // 
            // DonorFirstName_sms
            // 
            this.DonorFirstName_sms.DataPropertyName = "DonorFirstName";
            this.DonorFirstName_sms.HeaderText = "First Name";
            this.DonorFirstName_sms.Name = "DonorFirstName_sms";
            this.DonorFirstName_sms.ReadOnly = true;
            // 
            // DonorLastName_sms
            // 
            this.DonorLastName_sms.DataPropertyName = "DonorLastName";
            this.DonorLastName_sms.HeaderText = "Last Name";
            this.DonorLastName_sms.Name = "DonorLastName_sms";
            this.DonorLastName_sms.ReadOnly = true;
            // 
            // SMSReply_sms
            // 
            this.SMSReply_sms.DataPropertyName = "reply_text";
            this.SMSReply_sms.HeaderText = "SMS Reply";
            this.SMSReply_sms.Name = "SMSReply_sms";
            this.SMSReply_sms.ReadOnly = true;
            // 
            // SSN_sms
            // 
            this.SSN_sms.HeaderText = "SSN";
            this.SSN_sms.Name = "SSN_sms";
            this.SSN_sms.ReadOnly = true;
            this.SSN_sms.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ClientName_sms
            // 
            this.ClientName_sms.DataPropertyName = "ClientName";
            this.ClientName_sms.HeaderText = "Client";
            this.ClientName_sms.Name = "ClientName_sms";
            this.ClientName_sms.ReadOnly = true;
            // 
            // DepartmentName_sms
            // 
            this.DepartmentName_sms.DataPropertyName = "ClientDepartmentName";
            this.DepartmentName_sms.HeaderText = "Department";
            this.DepartmentName_sms.Name = "DepartmentName_sms";
            this.DepartmentName_sms.ReadOnly = true;
            // 
            // DonorTestRegisteredDate_sms
            // 
            this.DonorTestRegisteredDate_sms.DataPropertyName = "DonorTestRegisteredDate";
            this.DonorTestRegisteredDate_sms.HeaderText = "Date Registered";
            this.DonorTestRegisteredDate_sms.Name = "DonorTestRegisteredDate_sms";
            this.DonorTestRegisteredDate_sms.ReadOnly = true;
            // 
            // DateDonorNotified_sms
            // 
            this.DateDonorNotified_sms.DataPropertyName = "DateDonorNotified";
            this.DateDonorNotified_sms.HeaderText = "Donor Date Notified";
            this.DateDonorNotified_sms.Name = "DateDonorNotified_sms";
            this.DateDonorNotified_sms.ReadOnly = true;
            // 
            // DonorCity_sms
            // 
            this.DonorCity_sms.DataPropertyName = "DonorCity";
            this.DonorCity_sms.HeaderText = "Donor City";
            this.DonorCity_sms.Name = "DonorCity_sms";
            this.DonorCity_sms.ReadOnly = true;
            // 
            // ZipCode_sms
            // 
            this.ZipCode_sms.DataPropertyName = "DonorZipCode";
            this.ZipCode_sms.HeaderText = "Zip Code";
            this.ZipCode_sms.Name = "ZipCode_sms";
            this.ZipCode_sms.ReadOnly = true;
            // 
            // auto_reply_text_sms
            // 
            this.auto_reply_text_sms.DataPropertyName = "auto_reply_text";
            this.auto_reply_text_sms.HeaderText = "Auto Reply";
            this.auto_reply_text_sms.Name = "auto_reply_text_sms";
            this.auto_reply_text_sms.ReadOnly = true;
            // 
            // ReplyToSms_sms
            // 
            this.ReplyToSms_sms.HeaderText = "Reply to SMS";
            this.ReplyToSms_sms.Name = "ReplyToSms_sms";
            this.ReplyToSms_sms.ReadOnly = true;
            this.ReplyToSms_sms.Text = "Reply";
            this.ReplyToSms_sms.UseColumnTextForButtonValue = true;
            // 
            // MarkAsRead_sms
            // 
            this.MarkAsRead_sms.HeaderText = "Mark As Read";
            this.MarkAsRead_sms.Name = "MarkAsRead_sms";
            this.MarkAsRead_sms.ReadOnly = true;
            this.MarkAsRead_sms.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MarkAsRead_sms.Text = "Mark As Read";
            this.MarkAsRead_sms.UseColumnTextForButtonValue = true;
            // 
            // tabClientScheduleExpired
            // 
            this.tabClientScheduleExpired.BackColor = System.Drawing.Color.Silver;
            this.tabClientScheduleExpired.Controls.Add(this.label5);
            this.tabClientScheduleExpired.Controls.Add(this.dgvSendInScheduler);
            this.tabClientScheduleExpired.ImageKey = "flag-green-trans-ico.png";
            this.tabClientScheduleExpired.Location = new System.Drawing.Point(4, 26);
            this.tabClientScheduleExpired.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabClientScheduleExpired.Name = "tabClientScheduleExpired";
            this.tabClientScheduleExpired.Size = new System.Drawing.Size(1995, 976);
            this.tabClientScheduleExpired.TabIndex = 3;
            this.tabClientScheduleExpired.Text = "Send In Scheduler";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(572, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "The following client departments\' send in window has expired and needs to be addr" +
    "essed.";
            // 
            // dgvSendInScheduler
            // 
            this.dgvSendInScheduler.AllowDrop = true;
            this.dgvSendInScheduler.AllowUserToAddRows = false;
            this.dgvSendInScheduler.AllowUserToDeleteRows = false;
            this.dgvSendInScheduler.AllowUserToOrderColumns = true;
            this.dgvSendInScheduler.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSendInScheduler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSendInScheduler.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.client_name_sis,
            this.department_name_sis,
            this.notification_sweep_date_sis,
            this.notification_start_date_sis,
            this.notification_stop_date_sis});
            this.dgvSendInScheduler.Location = new System.Drawing.Point(13, 63);
            this.dgvSendInScheduler.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvSendInScheduler.MultiSelect = false;
            this.dgvSendInScheduler.Name = "dgvSendInScheduler";
            this.dgvSendInScheduler.ReadOnly = true;
            this.dgvSendInScheduler.RowHeadersVisible = false;
            this.dgvSendInScheduler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSendInScheduler.Size = new System.Drawing.Size(1965, 894);
            this.dgvSendInScheduler.TabIndex = 7;
            this.dgvSendInScheduler.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSendInScheduler_CellContentDoubleClick);
            this.dgvSendInScheduler.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSendInScheduler_DataBindingComplete);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Visible = false;
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // client_name_sis
            // 
            this.client_name_sis.DataPropertyName = "ClientName";
            this.client_name_sis.HeaderText = "Client";
            this.client_name_sis.Name = "client_name_sis";
            this.client_name_sis.ReadOnly = true;
            // 
            // department_name_sis
            // 
            this.department_name_sis.DataPropertyName = "ClientDepartmentName";
            this.department_name_sis.HeaderText = "Department";
            this.department_name_sis.Name = "department_name_sis";
            this.department_name_sis.ReadOnly = true;
            // 
            // notification_sweep_date_sis
            // 
            this.notification_sweep_date_sis.DataPropertyName = "notification_sweep_date_date";
            this.notification_sweep_date_sis.HeaderText = "Sweep Date";
            this.notification_sweep_date_sis.Name = "notification_sweep_date_sis";
            this.notification_sweep_date_sis.ReadOnly = true;
            // 
            // notification_start_date_sis
            // 
            this.notification_start_date_sis.DataPropertyName = "notification_start_date_date";
            this.notification_start_date_sis.HeaderText = "Send In Start Date";
            this.notification_start_date_sis.Name = "notification_start_date_sis";
            this.notification_start_date_sis.ReadOnly = true;
            // 
            // notification_stop_date_sis
            // 
            this.notification_stop_date_sis.DataPropertyName = "notification_stop_date_date";
            this.notification_stop_date_sis.HeaderText = "Send In Stop Date";
            this.notification_stop_date_sis.Name = "notification_stop_date_sis";
            this.notification_stop_date_sis.ReadOnly = true;
            // 
            // tabDeadlineDonors
            // 
            this.tabDeadlineDonors.BackColor = System.Drawing.Color.Silver;
            this.tabDeadlineDonors.Controls.Add(this.label6);
            this.tabDeadlineDonors.Controls.Add(this.dgvDeadlineDonors);
            this.tabDeadlineDonors.ImageKey = "flag-green-trans-ico.png";
            this.tabDeadlineDonors.Location = new System.Drawing.Point(4, 26);
            this.tabDeadlineDonors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabDeadlineDonors.Name = "tabDeadlineDonors";
            this.tabDeadlineDonors.Size = new System.Drawing.Size(1995, 976);
            this.tabDeadlineDonors.TabIndex = 4;
            this.tabDeadlineDonors.Text = "Deadline Donors";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(635, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "These are donors who have not been sent it or have registered after the end of th" +
    "e send in window.";
            // 
            // dgvDeadlineDonors
            // 
            this.dgvDeadlineDonors.AllowDrop = true;
            this.dgvDeadlineDonors.AllowUserToAddRows = false;
            this.dgvDeadlineDonors.AllowUserToDeleteRows = false;
            this.dgvDeadlineDonors.AllowUserToOrderColumns = true;
            this.dgvDeadlineDonors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDeadlineDonors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeadlineDonors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorSelection_dld,
            this.dataGridViewTextBoxColumn17,
            this.DonorFirstName_dld,
            this.DonorLastName_dld,
            this.SSN_dld,
            this.DOB_dld,
            this.ClientName_dld,
            this.DepartmentName_dld,
            this.DonorTestRegisteredDateValue_dld,
            this.notification_stop_date_dld,
            this.DonorCity_dld,
            this.ZipCode_dld});
            this.dgvDeadlineDonors.Location = new System.Drawing.Point(13, 63);
            this.dgvDeadlineDonors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDeadlineDonors.MultiSelect = false;
            this.dgvDeadlineDonors.Name = "dgvDeadlineDonors";
            this.dgvDeadlineDonors.ReadOnly = true;
            this.dgvDeadlineDonors.RowHeadersVisible = false;
            this.dgvDeadlineDonors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeadlineDonors.Size = new System.Drawing.Size(1965, 894);
            this.dgvDeadlineDonors.TabIndex = 7;
            this.dgvDeadlineDonors.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDeadlineDonors_CellMouseDoubleClick);
            this.dgvDeadlineDonors.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDeadlineDonors_DataBindingComplete);
            // 
            // DonorSelection_dld
            // 
            this.DonorSelection_dld.HeaderText = "";
            this.DonorSelection_dld.Name = "DonorSelection_dld";
            this.DonorSelection_dld.ReadOnly = true;
            this.DonorSelection_dld.Visible = false;
            this.DonorSelection_dld.Width = 50;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "in_window";
            this.dataGridViewTextBoxColumn17.HeaderText = "In Window";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Visible = false;
            // 
            // DonorFirstName_dld
            // 
            this.DonorFirstName_dld.DataPropertyName = "DonorFirstName";
            this.DonorFirstName_dld.HeaderText = "First Name";
            this.DonorFirstName_dld.Name = "DonorFirstName_dld";
            this.DonorFirstName_dld.ReadOnly = true;
            // 
            // DonorLastName_dld
            // 
            this.DonorLastName_dld.DataPropertyName = "DonorLastName";
            this.DonorLastName_dld.HeaderText = "Last Name";
            this.DonorLastName_dld.Name = "DonorLastName_dld";
            this.DonorLastName_dld.ReadOnly = true;
            // 
            // SSN_dld
            // 
            this.SSN_dld.HeaderText = "SSN";
            this.SSN_dld.Name = "SSN_dld";
            this.SSN_dld.ReadOnly = true;
            this.SSN_dld.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // DOB_dld
            // 
            this.DOB_dld.HeaderText = "DOB";
            this.DOB_dld.Name = "DOB_dld";
            this.DOB_dld.ReadOnly = true;
            this.DOB_dld.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.DOB_dld.Visible = false;
            // 
            // ClientName_dld
            // 
            this.ClientName_dld.DataPropertyName = "ClientName";
            this.ClientName_dld.HeaderText = "Client";
            this.ClientName_dld.Name = "ClientName_dld";
            this.ClientName_dld.ReadOnly = true;
            // 
            // DepartmentName_dld
            // 
            this.DepartmentName_dld.DataPropertyName = "ClientDepartmentName";
            this.DepartmentName_dld.HeaderText = "Department";
            this.DepartmentName_dld.Name = "DepartmentName_dld";
            this.DepartmentName_dld.ReadOnly = true;
            // 
            // DonorTestRegisteredDateValue_dld
            // 
            this.DonorTestRegisteredDateValue_dld.HeaderText = "Date Registered";
            this.DonorTestRegisteredDateValue_dld.Name = "DonorTestRegisteredDateValue_dld";
            this.DonorTestRegisteredDateValue_dld.ReadOnly = true;
            // 
            // notification_stop_date_dld
            // 
            this.notification_stop_date_dld.DataPropertyName = "SendInStopDate";
            this.notification_stop_date_dld.HeaderText = "Send In Stop Date";
            this.notification_stop_date_dld.Name = "notification_stop_date_dld";
            this.notification_stop_date_dld.ReadOnly = true;
            // 
            // DonorCity_dld
            // 
            this.DonorCity_dld.DataPropertyName = "DonorCity";
            this.DonorCity_dld.HeaderText = "Donor City";
            this.DonorCity_dld.Name = "DonorCity_dld";
            this.DonorCity_dld.ReadOnly = true;
            // 
            // ZipCode_dld
            // 
            this.ZipCode_dld.DataPropertyName = "DonorZipCode";
            this.ZipCode_dld.HeaderText = "Zip Code";
            this.ZipCode_dld.Name = "ZipCode_dld";
            this.ZipCode_dld.ReadOnly = true;
            // 
            // tabFormFox
            // 
            this.tabFormFox.BackColor = System.Drawing.Color.Silver;
            this.tabFormFox.Controls.Add(this.ffFlipArchive);
            this.tabFormFox.Controls.Add(this.label8);
            this.tabFormFox.Controls.Add(this.ffRefresh);
            this.tabFormFox.Controls.Add(this.ffDaysAgo);
            this.tabFormFox.Controls.Add(this.ffIncludeArchived);
            this.tabFormFox.Controls.Add(this.dgvFormFox);
            this.tabFormFox.Controls.Add(this.label7);
            this.tabFormFox.ImageKey = "flag-green-trans-ico.png";
            this.tabFormFox.Location = new System.Drawing.Point(4, 26);
            this.tabFormFox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabFormFox.Name = "tabFormFox";
            this.tabFormFox.Size = new System.Drawing.Size(1995, 976);
            this.tabFormFox.TabIndex = 5;
            this.tabFormFox.Text = "FormFox Status";
            this.tabFormFox.Click += new System.EventHandler(this.tabFormFox_Click);
            // 
            // ffFlipArchive
            // 
            this.ffFlipArchive.Location = new System.Drawing.Point(1109, 21);
            this.ffFlipArchive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ffFlipArchive.Name = "ffFlipArchive";
            this.ffFlipArchive.Size = new System.Drawing.Size(271, 28);
            this.ffFlipArchive.TabIndex = 15;
            this.ffFlipArchive.Text = "Archive / Unarchive Selected";
            this.ffFlipArchive.UseVisualStyleBackColor = true;
            this.ffFlipArchive.Click += new System.EventHandler(this.ffFlipArchive_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(511, 18);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Days Overdue";
            // 
            // ffRefresh
            // 
            this.ffRefresh.Location = new System.Drawing.Point(668, 16);
            this.ffRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ffRefresh.Name = "ffRefresh";
            this.ffRefresh.Size = new System.Drawing.Size(100, 28);
            this.ffRefresh.TabIndex = 13;
            this.ffRefresh.Text = "Refresh";
            this.ffRefresh.UseVisualStyleBackColor = true;
            this.ffRefresh.Click += new System.EventHandler(this.ffRefresh_Click);
            // 
            // ffDaysAgo
            // 
            this.ffDaysAgo.Location = new System.Drawing.Point(619, 16);
            this.ffDaysAgo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ffDaysAgo.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.ffDaysAgo.Name = "ffDaysAgo";
            this.ffDaysAgo.Size = new System.Drawing.Size(41, 23);
            this.ffDaysAgo.TabIndex = 12;
            this.ffDaysAgo.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ffIncludeArchived
            // 
            this.ffIncludeArchived.AutoSize = true;
            this.ffIncludeArchived.Location = new System.Drawing.Point(917, 21);
            this.ffIncludeArchived.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ffIncludeArchived.Name = "ffIncludeArchived";
            this.ffIncludeArchived.Size = new System.Drawing.Size(134, 21);
            this.ffIncludeArchived.TabIndex = 11;
            this.ffIncludeArchived.Text = "Include Archived";
            this.ffIncludeArchived.UseVisualStyleBackColor = true;
            this.ffIncludeArchived.CheckedChanged += new System.EventHandler(this.ffIncludeArchived_CheckedChanged);
            // 
            // dgvFormFox
            // 
            this.dgvFormFox.AllowDrop = true;
            this.dgvFormFox.AllowUserToAddRows = false;
            this.dgvFormFox.AllowUserToDeleteRows = false;
            this.dgvFormFox.AllowUserToOrderColumns = true;
            this.dgvFormFox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFormFox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFormFox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorSelection_ffo,
            this.IsArchived_ffo,
            this.donor_first_name__ffo,
            this.donor_last_name_ffo,
            this.SSN_ffo,
            this.donor_date_of_birth_ffo,
            this.donor_city_ffo,
            this.donor_zip_ffo,
            this.client_name_ffo,
            this.department_name_ffo,
            this.createdON_ffo,
            this.ReferenceTestID_ffo,
            this.updatedOn_ffo,
            this.status_ffo,
            this.backend_formfox_orders_id,
            this.archived_ffo});
            this.dgvFormFox.Location = new System.Drawing.Point(13, 63);
            this.dgvFormFox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvFormFox.MultiSelect = false;
            this.dgvFormFox.Name = "dgvFormFox";
            this.dgvFormFox.ReadOnly = true;
            this.dgvFormFox.RowHeadersVisible = false;
            this.dgvFormFox.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFormFox.Size = new System.Drawing.Size(1977, 799);
            this.dgvFormFox.TabIndex = 10;
            this.dgvFormFox.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFormFox_CellClick);
            this.dgvFormFox.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFormFox_CellFormatting);
            this.dgvFormFox.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFormFox_CellMouseDoubleClick);
            this.dgvFormFox.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFormFox_CellMouseUp);
            this.dgvFormFox.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvFormFox_DataBindingComplete);
            this.dgvFormFox.SelectionChanged += new System.EventHandler(this.dgvFormFox_SelectionChanged);
            // 
            // DonorSelection_ffo
            // 
            this.DonorSelection_ffo.HeaderText = "";
            this.DonorSelection_ffo.Name = "DonorSelection_ffo";
            this.DonorSelection_ffo.ReadOnly = true;
            this.DonorSelection_ffo.Width = 50;
            // 
            // IsArchived_ffo
            // 
            this.IsArchived_ffo.HeaderText = "Is Archived";
            this.IsArchived_ffo.Name = "IsArchived_ffo";
            this.IsArchived_ffo.ReadOnly = true;
            // 
            // donor_first_name__ffo
            // 
            this.donor_first_name__ffo.DataPropertyName = "donor_first_name";
            this.donor_first_name__ffo.HeaderText = "First Name";
            this.donor_first_name__ffo.Name = "donor_first_name__ffo";
            this.donor_first_name__ffo.ReadOnly = true;
            // 
            // donor_last_name_ffo
            // 
            this.donor_last_name_ffo.DataPropertyName = "donor_last_name";
            this.donor_last_name_ffo.HeaderText = "Last Name";
            this.donor_last_name_ffo.Name = "donor_last_name_ffo";
            this.donor_last_name_ffo.ReadOnly = true;
            // 
            // SSN_ffo
            // 
            this.SSN_ffo.HeaderText = "SSN";
            this.SSN_ffo.Name = "SSN_ffo";
            this.SSN_ffo.ReadOnly = true;
            this.SSN_ffo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // donor_date_of_birth_ffo
            // 
            this.donor_date_of_birth_ffo.DataPropertyName = "donor_date_of_birth";
            this.donor_date_of_birth_ffo.HeaderText = "DOB";
            this.donor_date_of_birth_ffo.Name = "donor_date_of_birth_ffo";
            this.donor_date_of_birth_ffo.ReadOnly = true;
            this.donor_date_of_birth_ffo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.donor_date_of_birth_ffo.Visible = false;
            // 
            // donor_city_ffo
            // 
            this.donor_city_ffo.DataPropertyName = "donor_city";
            this.donor_city_ffo.HeaderText = "Donor City";
            this.donor_city_ffo.Name = "donor_city_ffo";
            this.donor_city_ffo.ReadOnly = true;
            // 
            // donor_zip_ffo
            // 
            this.donor_zip_ffo.DataPropertyName = "donor_zip";
            this.donor_zip_ffo.HeaderText = "Zip Code";
            this.donor_zip_ffo.Name = "donor_zip_ffo";
            this.donor_zip_ffo.ReadOnly = true;
            // 
            // client_name_ffo
            // 
            this.client_name_ffo.DataPropertyName = "client_name";
            this.client_name_ffo.HeaderText = "Client";
            this.client_name_ffo.Name = "client_name_ffo";
            this.client_name_ffo.ReadOnly = true;
            // 
            // department_name_ffo
            // 
            this.department_name_ffo.DataPropertyName = "department_name";
            this.department_name_ffo.HeaderText = "Department";
            this.department_name_ffo.Name = "department_name_ffo";
            this.department_name_ffo.ReadOnly = true;
            // 
            // createdON_ffo
            // 
            this.createdON_ffo.DataPropertyName = "createdON";
            this.createdON_ffo.HeaderText = "Order Created On";
            this.createdON_ffo.Name = "createdON_ffo";
            this.createdON_ffo.ReadOnly = true;
            // 
            // ReferenceTestID_ffo
            // 
            this.ReferenceTestID_ffo.DataPropertyName = "ReferenceTestID";
            this.ReferenceTestID_ffo.HeaderText = "Reference Test ID";
            this.ReferenceTestID_ffo.Name = "ReferenceTestID_ffo";
            this.ReferenceTestID_ffo.ReadOnly = true;
            // 
            // updatedOn_ffo
            // 
            this.updatedOn_ffo.DataPropertyName = "updatedOn";
            this.updatedOn_ffo.HeaderText = "Last Updated";
            this.updatedOn_ffo.Name = "updatedOn_ffo";
            this.updatedOn_ffo.ReadOnly = true;
            // 
            // status_ffo
            // 
            this.status_ffo.DataPropertyName = "status";
            this.status_ffo.HeaderText = "Status";
            this.status_ffo.Name = "status_ffo";
            this.status_ffo.ReadOnly = true;
            // 
            // backend_formfox_orders_id
            // 
            this.backend_formfox_orders_id.DataPropertyName = "backend_formfox_orders_id";
            this.backend_formfox_orders_id.HeaderText = "backend_formfox_orders_id";
            this.backend_formfox_orders_id.Name = "backend_formfox_orders_id";
            this.backend_formfox_orders_id.ReadOnly = true;
            this.backend_formfox_orders_id.Visible = false;
            // 
            // archived_ffo
            // 
            this.archived_ffo.DataPropertyName = "archived";
            this.archived_ffo.HeaderText = "archivedDB";
            this.archived_ffo.Name = "archived_ffo";
            this.archived_ffo.ReadOnly = true;
            this.archived_ffo.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 16);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(294, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Orders Sent Through FormFox not completed";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "blank-trans-ico.png");
            this.imageList1.Images.SetKeyName(1, "flag-trans-ico.png");
            this.imageList1.Images.SetKeyName(2, "flag-green-trans-ico.png");
            // 
            // FrmExceptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2003, 910);
            this.Controls.Add(this.tabsExceptions);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmExceptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exception Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmExceptions_FormClosed);
            this.Load += new System.EventHandler(this.FrmExceptions_Load);
            this.Shown += new System.EventHandler(this.FrmExceptions_Shown);
            this.tabsExceptions.ResumeLayout(false);
            this.tabExceptionNotifications.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClinicExceptions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberToSelect)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinClinics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRangeInMiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMiles)).EndInit();
            this.tabSMSReplies.ResumeLayout(false);
            this.tabSMSReplies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSMSReplies)).EndInit();
            this.tabClientScheduleExpired.ResumeLayout(false);
            this.tabClientScheduleExpired.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendInScheduler)).EndInit();
            this.tabDeadlineDonors.ResumeLayout(false);
            this.tabDeadlineDonors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeadlineDonors)).EndInit();
            this.tabFormFox.ResumeLayout(false);
            this.tabFormFox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ffDaysAgo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormFox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabsExceptions;
        private System.Windows.Forms.TabPage tabExceptionNotifications;
        private System.Windows.Forms.Button btnNotify;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackBarMiles;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRandom;
        private Button button4;
        private DataGridView dgvClinicExceptions;
        private NumericUpDown numRangeInMiles;
        private Label label2;
        private NumericUpDown numMinClinics;
        private Label label3;
        private NumericUpDown numMaxRange;
        private NumericUpDown nudNumberToSelect;
        private TextBox txtEmail;
        private Button btnPreview;
        private ImageList imageList1;
        private TabPage tabSMSReplies;
        private TabPage tabClientScheduleExpired;
        private TabPage tabDeadlineDonors;

        private DataGridView dgvSMSReplies;
        private Label label4;
        private Label label5;
        private DataGridView dgvSendInScheduler;
        private Label label6;
        private DataGridView dgvDeadlineDonors;
        private DataGridViewCheckBoxColumn DonorSelection_dld;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private DataGridViewTextBoxColumn DonorFirstName_dld;
        private DataGridViewTextBoxColumn DonorLastName_dld;
        private DataGridViewTextBoxColumn SSN_dld;
        private DataGridViewTextBoxColumn DOB_dld;
        private DataGridViewTextBoxColumn ClientName_dld;
        private DataGridViewTextBoxColumn DepartmentName_dld;
        private DataGridViewTextBoxColumn DonorTestRegisteredDateValue_dld;
        private DataGridViewTextBoxColumn notification_stop_date_dld;
        private DataGridViewTextBoxColumn DonorCity_dld;
        private DataGridViewTextBoxColumn ZipCode_dld;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn client_name_sis;
        private DataGridViewTextBoxColumn department_name_sis;
        private DataGridViewTextBoxColumn notification_sweep_date_sis;
        private DataGridViewTextBoxColumn notification_start_date_sis;
        private DataGridViewTextBoxColumn notification_stop_date_sis;
        private DataGridViewCheckBoxColumn DonorSelection_sms;
        private DataGridViewTextBoxColumn DonorFirstName_sms;
        private DataGridViewTextBoxColumn DonorLastName_sms;
        private DataGridViewTextBoxColumn SMSReply_sms;
        private DataGridViewTextBoxColumn SSN_sms;
        private DataGridViewTextBoxColumn ClientName_sms;
        private DataGridViewTextBoxColumn DepartmentName_sms;
        private DataGridViewTextBoxColumn DonorTestRegisteredDate_sms;
        private DataGridViewTextBoxColumn DateDonorNotified_sms;
        private DataGridViewTextBoxColumn DonorCity_sms;
        private DataGridViewTextBoxColumn ZipCode_sms;
        private DataGridViewTextBoxColumn auto_reply_text_sms;
        private DataGridViewButtonColumn ReplyToSms_sms;
        private DataGridViewButtonColumn MarkAsRead_sms;
        private TabPage tabFormFox;
        private DataGridView dgvFormFox;
        private Label label7;
        private CheckBox ffIncludeArchived;
        private NumericUpDown ffDaysAgo;
        private Button ffRefresh;
        private Label label8;
        private Button ffFlipArchive;
        private Label lblSearching;
        private DataGridViewCheckBoxColumn DonorSelection;
        private DataGridViewTextBoxColumn in_window;
        private DataGridViewTextBoxColumn ClinicsInRange;
        private DataGridViewTextBoxColumn MaxMiles;
        private DataGridViewTextBoxColumn DonorDateNotifiedData;
        private DataGridViewTextBoxColumn DonorFirstName;
        private DataGridViewTextBoxColumn DonorLastName;
        private DataGridViewTextBoxColumn SSN;
        private DataGridViewTextBoxColumn DOB;
        private DataGridViewTextBoxColumn SpecimenID;
        private DataGridViewTextBoxColumn ClearStarID;
        private DataGridViewTextBoxColumn SpecimenDateValue;
        private DataGridViewTextBoxColumn ClientName;
        private DataGridViewTextBoxColumn DepartmentName;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn PaymentMode;
        private DataGridViewTextBoxColumn PaymentMethodId;
        private DataGridViewTextBoxColumn PaymentAmount;
        private DataGridViewTextBoxColumn DonorTestRegisteredDateValue;
        private DataGridViewTextBoxColumn DonorDateNotified;
        private DataGridViewTextBoxColumn PaymentDateValue;
        private DataGridViewTextBoxColumn PaymentType;
        private DataGridViewTextBoxColumn Result;
        private DataGridViewTextBoxColumn TestReason;
        private DataGridViewTextBoxColumn DonorCity;
        private DataGridViewTextBoxColumn ZipCode;
        private DataGridViewTextBoxColumn MROType;
        private DataGridViewTextBoxColumn DonorId;
        private DataGridViewTextBoxColumn DonorTestInfoId;
        private DataGridViewTextBoxColumn DonorInitialClientId;
        private DataGridViewTextBoxColumn TestInfoClientId;
        private DataGridViewTextBoxColumn TestInfoDepartmentId;
        private DataGridViewTextBoxColumn MROTypeId;
        private DataGridViewTextBoxColumn PaymentTypeId;
        private DataGridViewTextBoxColumn TestRequestedDate;
        private DataGridViewTextBoxColumn ReasonForTestId;
        private DataGridViewTextBoxColumn TestCategoryId;
        private DataGridViewTextBoxColumn TestPanelId;
        private DataGridViewTextBoxColumn DonorDateOfBirth;
        private DataGridViewTextBoxColumn SpecimenDate;
        private DataGridViewTextBoxColumn DonorRegistrationStatus;
        private DataGridViewTextBoxColumn TestStatus;
        private DataGridViewTextBoxColumn DonorSSN;
        private DataGridViewTextBoxColumn TestOverallResult;
        private DataGridViewTextBoxColumn PaymentReceived;
        private DataGridViewTextBoxColumn PaymentDate;
        private DataGridViewTextBoxColumn DonorTestRegisteredDate;
        private DataGridViewCheckBoxColumn DonorSelection_ffo;
        private DataGridViewTextBoxColumn IsArchived_ffo;
        private DataGridViewTextBoxColumn donor_first_name__ffo;
        private DataGridViewTextBoxColumn donor_last_name_ffo;
        private DataGridViewTextBoxColumn SSN_ffo;
        private DataGridViewTextBoxColumn donor_date_of_birth_ffo;
        private DataGridViewTextBoxColumn donor_city_ffo;
        private DataGridViewTextBoxColumn donor_zip_ffo;
        private DataGridViewTextBoxColumn client_name_ffo;
        private DataGridViewTextBoxColumn department_name_ffo;
        private DataGridViewTextBoxColumn createdON_ffo;
        private DataGridViewTextBoxColumn ReferenceTestID_ffo;
        private DataGridViewTextBoxColumn updatedOn_ffo;
        private DataGridViewTextBoxColumn status_ffo;
        private DataGridViewTextBoxColumn backend_formfox_orders_id;
        private DataGridViewTextBoxColumn archived_ffo;
    }



}